using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Console = Colorful.Console;

namespace AnyConsole
{
    /// <summary>
    /// Extended Console
    /// </summary>
    public partial class ExtendedConsole : IExtendedConsole
    {
        private bool _isDisposed;
        private StringBuilder _screenHeaderBuilder = new StringBuilder();
        private List<ConsoleLogEntry> _displayHistory = new List<ConsoleLogEntry>();
        private StringBuilder _screenLogBuilder = new StringBuilder();
        private IDictionary<string, ICollection<RowContent>> _staticRowContentBuilder = new Dictionary<string, ICollection<RowContent>>();
        private List<DirectOutputEntry> _directOutputEntries = new List<DirectOutputEntry>();
        private List<DirectOutputEntry> _clearOutputEntries = new List<DirectOutputEntry>();
        private Thread _drawingThread;
        private Thread _inputThread;
        private ManualResetEvent _isRunning;
        private ManualResetEvent _bufferingComplete;
        private ManualResetEvent _frameDrawnComplete;
        private StaticRowRenderer _staticRowRenderer;
        private ComponentRenderer _componentRenderer;
        private bool _clearRequested;
        private bool _disableLogProcessing = false;
        private bool _hasLogUpdates = false;
        private bool _isSearchEnabled = false;
        private bool _isHelpEnabled = false;
        private string _searchString;
        private int _searchLineIndex = -1;
        private SemaphoreSlim _historyLock = new SemaphoreSlim(1, 1);
        private HashSet<System.Drawing.Color> _colorTracker = new HashSet<System.Drawing.Color>();
        internal int _bufferYCursor = 0;
        internal ICollection<ConsoleLogEntry> _logHistory = new List<ConsoleLogEntry>();
        internal List<ConsoleLogEntry> _fullLogHistory;
        internal int LogDisplayHeight { get { return Console.WindowHeight - 3; } }

        #region Events
        public delegate void KeyPress(KeyPressEventArgs e);
        public delegate void MousePress(MousePressEventArgs e);
        public delegate void MouseScroll(MouseScrollEventArgs e);
        public delegate void MouseMove(MouseMoveEventArgs e);
        /// <summary>
        /// Fired on key press
        /// </summary>
        public event KeyPress OnKeyPress;
        /// <summary>
        /// Fired on mouse press
        /// </summary>
        public event MousePress OnMousePress;
        /// <summary>
        /// Fired on mouse scroll
        /// </summary>
        public event MouseScroll OnMouseScroll;
        /// <summary>
        /// Fired on mouse move
        /// </summary>
        public event MouseMove OnMouseMove;
        #endregion

        #region Properties
        /// <summary>
        /// Console options
        /// </summary>
        public ConsoleOptions Options { get; }

        /// <summary>
        /// Set/get the console encoding
        /// </summary>
        public Encoding OutputEncoding
        {
            get { return Console.OutputEncoding; }
            set { Console.OutputEncoding = value; }
        }

        /// <summary>
        /// True if stdout is redirected
        /// </summary>
        public bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <summary>
        /// True if stderr is redirected
        /// </summary>
        public bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <summary>
        /// Console configuration
        /// </summary>
        public ExtendedConsoleConfiguration Configuration { get; private set; }
        #endregion

        /// <summary>
        /// Create an extended console
        /// </summary>
        public ExtendedConsole() : this(new ConsoleOptions())
        {
        }

        /// <summary>
        /// Create an extended console
        /// </summary>
        /// <param name="options"></param>
        public ExtendedConsole(ConsoleOptions options)
        {
            Options = options;
            _screenLogBuilder = ConfigureConsole();
            var consoleConfiguration = new ExtendedConsoleConfiguration();
            Configuration = consoleConfiguration;
            Configuration.DataContext.SetData<ExtendedConsole>("ExtendedConsole", this);
        }

        /// <summary>
        /// Configure the console
        /// </summary>
        /// <param name="config"></param>
        public void Configure(Action<ExtendedConsoleConfiguration> config)
        {
            config.Invoke(Configuration);
            _componentRenderer = new ComponentRenderer(this, Configuration.DataContext);
            _staticRowRenderer = new StaticRowRenderer(Configuration, _componentRenderer, Options);

            foreach (var component in Configuration.CustomComponents)
                _componentRenderer.RegisterComponent(component.Key, component.Value);
            Console.BackgroundColor = Configuration.LogHistoryContainer.BackgroundColor ?? Configuration.ColorPalette.Get(Configuration.LogHistoryContainer.BackgroundColorPalette) ?? Style._background;
            Console.Clear();
        }

        public void ToggleDisableProcessing()
        {
            _disableLogProcessing = !_disableLogProcessing;
        }

        /// <summary>
        /// Start the console
        /// </summary>
        public void Start()
        {
            InitializeConsole();
        }

        /// <summary>
        /// Close the console
        /// </summary>
        public void Close()
        {
            // invoke the quit handler
            Configuration.QuitHandler?.Invoke(this);

            DrawShutdown();
            if (!_isDisposed)
                _isRunning?.Set();
        }

        /// <summary>
        /// Block until the application has closed
        /// </summary>
        public void WaitForClose()
        {
            _isRunning.WaitOne();
        }

        public void WaitForBufferComplete()
        {
            _bufferingComplete.WaitOne();
        }

        /// <summary>
        /// Set the cursor position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetCursorPosition(int x, int y)
        {
            if (!IsOutputRedirected)
                Console.SetCursorPosition(x, y);
        }

        private void RegisterComponents()
        {
            var components = _staticRowContentBuilder.SelectMany(x => x.Value.Select(y => y.Component)).Distinct().ToList();
            _componentRenderer.RegisterUsedBuiltInComponents(components);
        }

        private void InitializeConsole()
        {
            RegisterComponents();
            // initialize the database update threads
            _isRunning = new ManualResetEvent(false);
            _bufferingComplete = new ManualResetEvent(false);
            _frameDrawnComplete = new ManualResetEvent(false);
            _clearRequested = false;
            _inputThread = new Thread(new ThreadStart(InputThread));
            //_inputThread.IsBackground = true;
            _inputThread.Start();
            _drawingThread = new Thread(new ThreadStart(DrawingThread));
            //_drawingThread.IsBackground = true;
            _drawingThread.Start();
        }

        private void DrawingThread()
        {
            // if there is no console available, no need to display anything.
            if (_screenLogBuilder == null)
                return;
            _fullLogHistory = new List<ConsoleLogEntry>();
            while (!_isRunning.WaitOne(Configuration.RedrawTimeSpan))
            {
                _historyLock.Wait();
                try
                {
                    _fullLogHistory = ProcessBufferedOutput(_screenLogBuilder, _fullLogHistory);
                    _displayHistory = TrimAndCopyBufferForDisplay(_fullLogHistory);
                    // remove any cleared direct output entries
                    _directOutputEntries = _directOutputEntries.Where(x => !x.IsCleared).ToList();
                }
                finally
                {
                    _historyLock.Release();
                }
                _frameDrawnComplete.Reset();
                DrawScreen(_screenHeaderBuilder, _displayHistory, _hasLogUpdates);
                _hasLogUpdates = false;
            }
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        private StringBuilder ConfigureConsole()
        {
            if (Console.IsOutputRedirected)
                return null;
            var consoleOutputStringWriter = new StringWriter();
            try
            {
                // redirect stdout to an internal buffer.
                // anything we want to display to the screen has to be done through stderr
                PositionConsoleWindow();

                if (Options.Container.HasValue && Options.Container.Value.Size.Width > 0 && Options.Container.Value.Size.Height > 0)
                    Console.SetWindowSize(Options.Container.Value.Size.Width, Options.Container.Value.Size.Height);
                Console.BufferHeight = Console.WindowHeight;
                Console.SetOut(consoleOutputStringWriter);
                Console.CursorVisible = !Options.RenderOptions.HasFlag(RenderOptions.HideCursor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error configuring ExtendedConsole: {ex.Message}");
                return null;
            }
            return consoleOutputStringWriter.GetStringBuilder();
        }

        private List<ConsoleLogEntry> TrimAndCopyBufferForDisplay(List<ConsoleLogEntry> logHistory)
        {
            if (_bufferYCursor > logHistory.Count - LogDisplayHeight)
                _bufferYCursor = logHistory.Count - LogDisplayHeight;
            if (_bufferYCursor < 0)
                _bufferYCursor = 0;
            var skipAmount = logHistory.Count - LogDisplayHeight - _bufferYCursor;
            return logHistory.Skip(skipAmount).Take(LogDisplayHeight).ToList();
        }

        /// <summary>
        /// Take in any new stdout content and append it to the internal screen log.
        /// </summary>
        /// <param name="screenLogBuilder"></param>
        /// <param name="logHistory"></param>
        /// <returns></returns>
        private List<ConsoleLogEntry> ProcessBufferedOutput(StringBuilder screenLogBuilder, List<ConsoleLogEntry> logHistory)
        {
            // get the stdout screen buffer and add it to the internal screen buffer
            var pendingLines = screenLogBuilder.ToString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pendingLine in pendingLines)
            {
                var line = pendingLine;
                var prepend = Configuration.Prepend;
                if (!string.IsNullOrEmpty(line) && line.IndexOf(ConsoleLogEntry.DisableProcessingCode) >= 0)
                {
                    ToggleDisableProcessing();
                    line = line.Substring(1, line.Length - 1);
                }
                if (_disableLogProcessing)
                    prepend = string.Empty;
                logHistory.Add(new ConsoleLogEntry(line, prepend, _disableLogProcessing));
                _hasLogUpdates = true;
                // if we are currently viewing the log history, offset it when new lines are added to prevent scrolling
                if (_bufferYCursor != 0)
                    _bufferYCursor++;
            }
            _bufferingComplete.Set();

            // remove older items not shown to the screen as it's not needed anymore
            var linesToRemove = Math.Abs(Configuration.MaxHistoryLines - logHistory.Count);

            if (logHistory.Count > Configuration.MaxHistoryLines && logHistory.Count >= linesToRemove)
                logHistory.RemoveRange(0, linesToRemove);
            screenLogBuilder.Clear();

            return logHistory;
        }

        private void DrawScreen(StringBuilder infoBuilder, ICollection<ConsoleLogEntry> log, bool logHasUpdates)
        {
            ColorTracker.Clear();
            // we will write any screen contents to stderr, as stdout is redirected internally
            var forceStaticRowDraw = false;
            var stdout = Console.Error;
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var width = Console.WindowWidth;

            Console.SetCursorPosition(0, 0);

            if (_clearRequested)
            {
                _clearRequested = false;
                logHasUpdates = true;
                // force the static headers to redraw
                forceStaticRowDraw = true;
                Console.Clear();
            }

            // write the static row headers
            if (Configuration.StaticRows?.Any() == true)
            {
                foreach (var staticRow in Configuration.StaticRows)
                {
                    infoBuilder.Clear();
                    if (_staticRowContentBuilder.ContainsKey(staticRow.Name))
                    {
                        var content = _staticRowContentBuilder[staticRow.Name];
                        var rowText = _staticRowRenderer
                            .Build(staticRow, content)
                            .Write(stdout, forceStaticRowDraw);
                    }
                    else
                    {
                        // render a blank row
                        _staticRowRenderer
                            .Build(staticRow)
                            .Write(stdout, forceStaticRowDraw);
                    }
                }
            }

            if (logHasUpdates)
            {
                // clear any direct output that needs to be removed
                ClearDirectOutput(stdout);

                // draw any direct output buffers added
                _historyLock.Wait();
                try
                {
                    _directOutputEntries.RemoveAll(x => x.IsDisplayed && x.DirectOutputMode == DirectOutputMode.ClearOnChange);
                }
                finally
                {
                    _historyLock.Release();
                }

                // restore cursor
                var xpos = GetXPosition(Configuration.LogHistoryContainer, 0);
                var ypos = GetYPosition(Configuration.LogHistoryContainer, 0);
                // write the screen log buffer
                Console.SetCursorPosition(xpos, ypos);
                var i = 0;
                var linesToFade = Options.RenderOptions.HasFlag(RenderOptions.FadeHistory) ? 6 : 0;
                var className = string.Empty;
                var rowPrefix = string.Empty;
                var hideClassNamePrefix = Options.RenderOptions.HasFlag(RenderOptions.HideClassNamePrefix);
                var hideLogRowPrefix = Options.RenderOptions.HasFlag(RenderOptions.HideLogRowPrefix);
                var logBackgroundColor = Configuration.LogHistoryContainer.BackgroundColor ?? Configuration.ColorPalette.Get(Configuration.LogHistoryContainer.BackgroundColorPalette) ?? Style._background;
                ColorTracker.SetBackColor(logBackgroundColor);
                _disableLogProcessing = false;
                foreach (var logLine in log)
                {
                    className = string.Empty;
                    rowPrefix = string.Empty;
                    // fade the long lines away
                    var logForegroundColor = Configuration.LogHistoryContainer.ForegroundColor
                        ?? Configuration.ColorPalette.Get(Configuration.LogHistoryContainer.ForegroundColorPalette)
                        ?? Style._foreground;
                    var classNameForegroundColor = Configuration.LogHistoryContainer.PrependColor
                        ?? Configuration.ColorPalette.Get(Configuration.LogHistoryContainer.PrependColorPalette)
                        ?? Style._className;
                    if (logLine.OriginalLine.Contains("|WARN|"))
                        logForegroundColor = Style._warningText;
                    if (logLine.OriginalLine.Contains("|ERROR|"))
                        logForegroundColor = Style._errorText;

                    if (i < linesToFade)
                    {
                        var r = Math.Max(logForegroundColor.R - ((linesToFade - i) * 25), 0);
                        var g = Math.Max(logForegroundColor.G - ((linesToFade - i) * 25), 0);
                        var b = Math.Max(logForegroundColor.B - ((linesToFade - i) * 25), 0);
                        logForegroundColor = System.Drawing.Color.FromArgb(255, r, g, b);
                        var cr = Math.Max(classNameForegroundColor.R - ((linesToFade - i) * 25), 0);
                        var cg = Math.Max(classNameForegroundColor.G - ((linesToFade - i) * 25), 0);
                        var cb = Math.Max(classNameForegroundColor.B - ((linesToFade - i) * 25), 0);
                        classNameForegroundColor = System.Drawing.Color.FromArgb(255, cr, cg, cb);
                    }
                    if (!string.IsNullOrEmpty(logLine.Prepend))
                    {
                        ColorTracker.SetForeColor(classNameForegroundColor);
                        if (!hideClassNamePrefix)
                        {
                            className = $"{logLine.ClassName}";
                            stdout.Write(className);
                        }
                        if (!hideLogRowPrefix)
                        {
                            rowPrefix = logLine.Prepend;
                            stdout.Write(rowPrefix);
                        }
                    }
                    ColorTracker.SetForeColor(logForegroundColor);
                    var truncatedLine = logLine.GetTruncatedLine(Console.BufferWidth, rowPrefix.Length);
                    // fixes an issue when the buffer width and window width are not the same
                    var windowSpacing = Console.BufferWidth - Console.WindowWidth;
                    var spaces = (Console.WindowWidth - className.Length - rowPrefix.Length - truncatedLine.Length) + windowSpacing;
                    if (spaces < 0)
                        spaces = 0;
                    stdout.Write(truncatedLine + new string(' ', spaces));
                    i++;
                }
            }

            if (_isHelpEnabled)
            {
                RenderHelpWindow(stdout);
            }

            RenderDirectOutput(stdout);

            if (Configuration.WindowFrame.Size > 0)
                WindowFrameRenderer.Render(Configuration.WindowFrame, stdout);

            _frameDrawnComplete.Set();
        }

        private void ClearDirectOutput(TextWriter stdout)
        {
            _historyLock.Wait();
            try
            {
                if (_directOutputEntries.Any(x => x.Clear && !x.IsCleared))
                {
                    var cursorLeft = Console.CursorLeft;
                    var cursorTop = Console.CursorTop;
                    var originalBackground = Console.BackgroundColor;

                    foreach (var entry in _directOutputEntries.Where(x => x.Clear && !x.IsCleared))
                    {
                        Console.SetCursorPosition(entry.X, entry.Y);
                        if (entry.Clear)
                        {
                            stdout.Write(new string(' ', entry.Length));
                            entry.IsCleared = true;
                        }
                    }
                    Console.BackgroundColor = originalBackground;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                }
            }
            finally
            {
                _historyLock.Release();
            }
        }

        private void RenderDirectOutput(TextWriter stdout)
        {
            _historyLock.Wait();
            try
            {
                if (_directOutputEntries.Any(x => !x.Clear))
                {
                    var cursorLeft = Console.CursorLeft;
                    var cursorTop = Console.CursorTop;
                    var originalForeground = Console.ForegroundColor;
                    var originalBackground = Console.BackgroundColor;
                    foreach (var entry in _directOutputEntries.Where(x => !x.Clear))
                    {
                        if (entry.Text != null)
                        {
                            // standard text entry
                            Console.SetCursorPosition(entry.X, entry.Y);
                            if (entry.ForegroundColor.HasValue)
                                Console.ForegroundColor = entry.ForegroundColor.Value;
                            if (entry.BackgroundColor.HasValue)
                                Console.BackgroundColor = entry.BackgroundColor.Value;
                            stdout.Write(entry.Text);
                        }
                        else
                        {
                            // textBuilder entry
                            Console.SetCursorPosition(entry.X, entry.Y);
                            var totalLength = 0;
                            foreach (var fragment in entry.TextBuilder.TextFragments)
                            {
                                if (fragment.ForegroundColor.HasValue)
                                    Console.ForegroundColor = fragment.ForegroundColor.Value;
                                if (fragment.BackgroundColor.HasValue)
                                    Console.BackgroundColor = fragment.BackgroundColor.Value;
                                var fragmentText = fragment.Text;
                                if (entry.TextBuilder.MaxLength.HasValue && totalLength + fragmentText.Length > entry.TextBuilder.MaxLength.Value)
                                {
                                    // truncate the text
                                    var len = entry.TextBuilder.MaxLength.Value - (totalLength + fragmentText.Length);
                                    fragmentText = fragmentText.Substring(0, len);
                                    stdout.Write(fragmentText);
                                    break;
                                }
                                stdout.Write(fragmentText);
                                totalLength += fragment.Text.Length;
                            }
                        }
                        entry.IsDisplayed = true;
                    }
                    Console.ForegroundColor = originalForeground;
                    Console.BackgroundColor = originalBackground;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                }
            }
            finally
            {
                _historyLock.Release();
            }
        }

        private void RenderHelpWindow(TextWriter stdout)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var originalForeground = Console.ForegroundColor;
            var originalBackground = Console.BackgroundColor;
            var longestEntry = Configuration.HelpScreen.HelpEntries.OrderByDescending(x => x.Key.Length + x.Description.Length).FirstOrDefault();
            var helpWidth = longestEntry.Key.Length + longestEntry.Description.Length + 4;
            var helpHeight = Configuration.HelpScreen.HelpEntries.Count;

            var helpStartX = width / 2 - helpWidth / 2;
            var helpStartY = height / 2 - helpHeight / 2;
            var i = 0;
            Console.SetCursorPosition(helpStartX, helpStartY);

            var foreColor = Configuration.HelpScreen.ForegroundColor ?? Configuration.ColorPalette.Get(Configuration.HelpScreen.ForegroundColorPalette) ?? Style._foreground;
            var backColor = Configuration.HelpScreen.BackgroundColor ?? Configuration.ColorPalette.Get(Configuration.HelpScreen.BackgroundColorPalette) ?? Style._background;
            var shadowColor = System.Drawing.Color.FromArgb(backColor.A, (int)Math.Max(backColor.R * 0.5, 0), (int)Math.Max(backColor.G * 0.5, 0), (int)Math.Max(backColor.B * 0.5, 0));
            // if we don't have space for the shadow, just draw the background color
            if (ColorTracker.Count >= ColorPalette.MaxColors)
                shadowColor = backColor;
            ColorTracker.SetForeColor(backColor);
            Console.BackgroundColor = originalBackground;
            stdout.Write(new string('▄', helpWidth + 2)); // draw a smaller top margin
            ColorTracker.SetForeColor(foreColor);
            i++;
            foreach (var entry in Configuration.HelpScreen.HelpEntries)
            {
                ColorTracker.SetBackColor(backColor);
                Console.SetCursorPosition(helpStartX, helpStartY + i);
                var content = $"{entry.Key}: {entry.Description}";
                var spaces = helpWidth - content.Length;
                stdout.Write($"  {content}");
                stdout.Write(new string(' ', spaces));
                if (Configuration.HelpScreen.EnableDropShadow)
                {
                    // draw shadow
                    ColorTracker.SetBackColor(shadowColor);
                    stdout.Write(" ");
                }
                i++;
            }
            Console.SetCursorPosition(helpStartX, helpStartY + i);
            ColorTracker.SetBackColor(backColor);
            stdout.Write(new string(' ', helpWidth + 2));
            if (Configuration.HelpScreen.EnableDropShadow)
            {
                // draw shadow
                ColorTracker.SetForeColor(shadowColor);
                ColorTracker.SetBackColor(shadowColor);
                stdout.Write(" ");
                Console.BackgroundColor = originalBackground;
                Console.SetCursorPosition(helpStartX + 1, helpStartY + i + 1);
                stdout.Write(new string('▀', helpWidth + 2)); // draw a smaller bottom margin/shadow
            }
            Console.ForegroundColor = originalForeground;
            Console.BackgroundColor = originalBackground;
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        private int GetXPosition(LogHistoryContainer container, int currentOffset)
        {
            return 0;
        }

        private int GetYPosition(LogHistoryContainer container, int currentOffset)
        {
            var height = Console.WindowHeight;
            var y = 0;
            switch (container.Location)
            {
                case RowLocation.Top:
                    y = currentOffset + container.Index;
                    break;
                case RowLocation.Bottom:
                    y = height - container.Index - currentOffset - 1;
                    break;
                case RowLocation.Middle:
                    break;
            }
            return y;
        }

        /// <summary>
        /// Move the console to a specific location
        /// </summary>
        private void PositionConsoleWindow()
        {
            if (Options?.Container.HasValue == true && !Options.Container.Value.IsEmpty)
            {
                // todo: figure out how to apply this against an XY position
                var hWnd = GetConsoleWindow();
                var mi = MONITORINFO.Default;
                GetMonitorInfo(MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY), ref mi);
                var wp = WINDOWPLACEMENT.Default;
                GetWindowPlacement(hWnd, ref wp);
                wp.NormalPosition = new RECT()
                {
                    Left = -7,
                    Top = mi.rcWork.Top,
                    Right = (wp.NormalPosition.Right - wp.NormalPosition.Left),
                    Bottom = -7 + mi.rcWork.Bottom
                };
                SetWindowPlacement(hWnd, ref wp);
                SetForegroundWindow(hWnd);
            }
        }

        private void ToggleSearch()
        {
            _isSearchEnabled = !_isSearchEnabled;
            if (!_isSearchEnabled)
            {
                _searchString = string.Empty;
                _searchLineIndex = -1;
            }
            SetSearch(_isSearchEnabled);
        }

        private void ToggleHelp()
        {
            _isHelpEnabled = !_isHelpEnabled;
        }

        private void SetSearch(bool isSearchEnabled)
        {
            _isSearchEnabled = isSearchEnabled;
            Configuration.DataContext.SetData<bool>("IsSearchEnabled", _isSearchEnabled);
        }

        private void SetSearchString(string str)
        {
            Configuration.DataContext.SetData<string>("SearchString", str);
        }

        private void AddSearchString(char c)
        {
            if (c == 8 && _searchString.Length > 0)
            {
                _searchString = _searchString.Substring(0, _searchString.Length - 1);
                SetSearchString(_searchString);
            }
            if (c >= 32 && c <= 126)
            {
                _searchString = _searchString + c;
                SetSearchString(_searchString);
            }
        }

        private IDictionary<int, ConsoleLogEntry> ComputeSearchMatches()
        {
            var matches = new Dictionary<int, ConsoleLogEntry>();
            for (var i = 0; i < _fullLogHistory.Count; i++)
            {
                var index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(_fullLogHistory[i].OriginalLine, _searchString, CompareOptions.IgnoreCase);
                if (index >= 0)
                    matches.Add(i, _fullLogHistory[i]);
            }
            Configuration.DataContext.SetData<int>("SearchMatches", matches.Count);
            return matches;
        }

        private void FindNext()
        {
            var matches = ComputeSearchMatches();
            var hasMatches = matches.Any();
            // wrap the search
            if (hasMatches && matches.Count >= _searchLineIndex)
            {
                // get next match
                _searchLineIndex++;
            }

            if (hasMatches && _searchLineIndex >= matches.Count)
                _searchLineIndex = 0; // wrap to first match

            // compute the Y cursor
            if (_searchLineIndex >= 0)
            {
                var historyLineNumber = matches.Skip(_searchLineIndex).Select(x => x.Key).FirstOrDefault();
                if (_fullLogHistory.Count - historyLineNumber - LogDisplayHeight >= 0)
                    _bufferYCursor = _fullLogHistory.Count - historyLineNumber - LogDisplayHeight;
            }
            Configuration.DataContext.SetData<int>("SearchIndex", _searchLineIndex);
        }

        private void FindPrevious()
        {
            var matches = ComputeSearchMatches();
            var hasMatches = matches.Any();
            if (hasMatches && _searchLineIndex >= 0)
            {
                // get next match
                _searchLineIndex--;
            }

            // wrap the search
            if (hasMatches && _searchLineIndex == -1)
                _searchLineIndex = matches.Count - 1; // wrap to last match

            // scroll to the line number
            if (_searchLineIndex >= 0)
            {
                var historyLineNumber = matches.Skip(_searchLineIndex).Select(x => x.Key).FirstOrDefault();
                if (_fullLogHistory.Count - historyLineNumber - LogDisplayHeight >= 0)
                    _bufferYCursor = _fullLogHistory.Count - historyLineNumber - LogDisplayHeight;
            }
            Configuration.DataContext.SetData<int>("SearchIndex", _searchLineIndex);
        }

        private void DrawShutdown()
        {
            _frameDrawnComplete.Reset();
            WriteRaw("Shutdown requested...");
            _frameDrawnComplete.WaitOne();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (isDisposing)
            {
                if (_isRunning?.WaitOne(1) == false)
                    _isRunning?.Set();
                _componentRenderer.Dispose();
                try
                {
                    if (_drawingThread.ThreadState == ThreadState.Running && !_drawingThread.Join(500))
                        _drawingThread.Abort();
                }
                catch (PlatformNotSupportedException)
                {
                    // ignore thread-abort errors on unsupported platforms
                }
                try
                {
                    if (_inputThread.ThreadState == ThreadState.Running && !_inputThread.Join(500))
                        _inputThread.Abort();
                }
                catch (PlatformNotSupportedException)
                {
                    // ignore thread-abort errors on unsupported platforms
                }
                _drawingThread = null;
                _inputThread = null;
                _bufferingComplete.Dispose();
                _frameDrawnComplete.Dispose();
                // _isRunning?.Dispose();
            }
            _isDisposed = true;
        }

        #endregion
    }
}
