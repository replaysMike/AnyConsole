﻿using Colorful;
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
    public partial class ExtendedConsole : IExtendedConsole, IDisposable
    {
        private StringBuilder _screenHeaderBuilder = new StringBuilder();
        private StringBuilder _screenLogBuilder = new StringBuilder();
        private IDictionary<string, ICollection<RowContent>> _staticRowContentBuilder = new Dictionary<string, ICollection<RowContent>>();
        private Thread _drawingThread;
        private Thread _inputThread;
        private ManualResetEvent _isRunning;
        private ManualResetEvent _bufferingComplete;
        private ManualResetEvent _frameDrawnComplete;
        private bool _isDisposed;
        private ExtendedConsoleConfiguration _config;
        private StaticRowRenderer _staticRowRenderer;
        private ComponentRenderer _componentRenderer;
        internal int _bufferYCursor = 0;
        internal ICollection<ConsoleLogEntry> _logHistory = new List<ConsoleLogEntry>();
        internal List<ConsoleLogEntry> _fullLogHistory;
        internal int LogDisplayHeight { get { return Console.WindowHeight - 3; } }
        private bool _disableLogProcessing = false;
        private bool _hasLogUpdates = false;
        private bool _isSearchEnabled = false;
        private bool _isHelpEnabled = false;
        private string _searchString;
        private int _searchLineIndex = -1;
        private SemaphoreSlim _historyLock = new SemaphoreSlim(1, 1);
        private HashSet<System.Drawing.Color> _colorTracker = new HashSet<System.Drawing.Color>();

        #region Events
        public delegate void KeyPress(KeyPressEventArgs e);
        public delegate void MousePress(MousePressEventArgs e);
        public delegate void MouseScroll(MouseScrollEventArgs e);
        public delegate void MouseMove(MouseMoveEventArgs e);
        public event KeyPress OnKeyPress;
        public event MousePress OnMousePress;
        public event MouseScroll OnMouseScroll;
        public event MouseMove OnMouseMove;
        #endregion

        #region Properties
        /// <summary>
        /// Console options
        /// </summary>
        public ConsoleOptions Options { get; }
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
        }

        /// <summary>
        /// Configure the console
        /// </summary>
        /// <param name="config"></param>
        public void Configure(Action<ExtendedConsoleConfiguration> config)
        {
            var consoleConfiguration = new ExtendedConsoleConfiguration();
            config.Invoke(consoleConfiguration);
            _config = consoleConfiguration;
            _config.DataContext.SetData<ExtendedConsole>("ExtendedConsole", this);

            _componentRenderer = new ComponentRenderer(this, _config.DataContext);
            foreach (var component in _config.CustomComponents)
                _componentRenderer.RegisterComponent(component.Key, component.Value);
            _staticRowRenderer = new StaticRowRenderer(_config, _componentRenderer, Options);
            Console.BackgroundColor = _config.LogHistoryContainer.BackgroundColor ?? _config.ColorPalette.Get(_config.LogHistoryContainer.BackgroundColorPalette) ?? Style.Background;
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
            var screenHeaderBuilder = new StringBuilder();
            var displayHistory = new List<ConsoleLogEntry>();
            _fullLogHistory = new List<ConsoleLogEntry>();
            while (!_isRunning.WaitOne(_config.RedrawTimeSpan))
            {
                _historyLock.Wait();
                try
                {
                    _fullLogHistory = ProcessBufferedOutput(_screenLogBuilder, _fullLogHistory);
                    displayHistory = TrimBufferForDisplay(_fullLogHistory);
                }
                finally
                {
                    _historyLock.Release();
                }
                _frameDrawnComplete.Reset();
                DrawStaticHeader(screenHeaderBuilder, displayHistory, _hasLogUpdates);
            }
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

        private List<ConsoleLogEntry> TrimBufferForDisplay(List<ConsoleLogEntry> logHistory)
        {
            if (_bufferYCursor < 0)
                _bufferYCursor = 0;
            if (_bufferYCursor > logHistory.Count - LogDisplayHeight)
                _bufferYCursor = logHistory.Count - LogDisplayHeight;
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
                var prepend = _config.Prepend;
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
            var linesToRemove = Math.Abs(_config.MaxHistoryLines - logHistory.Count);

            if (logHistory.Count > _config.MaxHistoryLines && logHistory.Count >= linesToRemove)
                logHistory.RemoveRange(0, linesToRemove);
            screenLogBuilder.Clear();

            return logHistory;
        }

        private void DrawStaticHeader(StringBuilder infoBuilder, ICollection<ConsoleLogEntry> log, bool logHasUpdates)
        {
            ColorTracker.Clear();
            // we will write any screen contents to stderr, as stdout is redirected internally
            var stdout = Console.Error;
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var width = Console.WindowWidth;

            Console.SetCursorPosition(0, 0);

            // write the static row headers
            if (_config.StaticRows?.Any() == true)
            {
                foreach (var staticRow in _config.StaticRows)
                {
                    infoBuilder.Clear();
                    if (_staticRowContentBuilder.ContainsKey(staticRow.Name))
                    {
                        var content = _staticRowContentBuilder[staticRow.Name];
                        var rowText = _staticRowRenderer
                            .Build(staticRow, content)
                            .Write(stdout);
                    }
                    else
                    {
                        // render a blank row
                        _staticRowRenderer
                            .Build(staticRow)
                            .Write(stdout);
                    }
                }
            }

            if (logHasUpdates)
            {
                // restore cursor
                var xpos = GetXPosition(_config.LogHistoryContainer, 0);
                var ypos = GetYPosition(_config.LogHistoryContainer, 0);
                // write the screen log buffer
                Console.SetCursorPosition(xpos, ypos);
                var i = 0;
                var linesToFade = Options.RenderOptions.HasFlag(RenderOptions.FadeHistory) ? 6 : 0;
                var className = string.Empty;
                var rowPrefix = string.Empty;
                var hideClassNamePrefix = Options.RenderOptions.HasFlag(RenderOptions.HideClassNamePrefix);
                var hideLogRowPrefix = Options.RenderOptions.HasFlag(RenderOptions.HideLogRowPrefix);
                var logBackgroundColor = _config.LogHistoryContainer.BackgroundColor ?? _config.ColorPalette.Get(_config.LogHistoryContainer.BackgroundColorPalette) ?? Style.Background;
                ColorTracker.SetBackColor(logBackgroundColor);
                _disableLogProcessing = false;
                foreach (var logLine in log)
                {
                    className = string.Empty;
                    rowPrefix = string.Empty;
                    // fade the long lines away
                    var logForegroundColor = _config.LogHistoryContainer.ForegroundColor
                        ?? _config.ColorPalette.Get(_config.LogHistoryContainer.ForegroundColorPalette)
                        ?? Style.Foreground;
                    var classNameForegroundColor = Style.ClassName;
                    if (logLine.OriginalLine.Contains("|WARN|"))
                        logForegroundColor = Style.WarningText;
                    if (logLine.OriginalLine.Contains("|ERROR|"))
                        logForegroundColor = Style.ErrorText;

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
                    var spaces = (Console.WindowWidth - className.Length - rowPrefix.Length - truncatedLine.Length);
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

            if (_config.WindowFrame.Size > 0)
                WindowFrameRenderer.Render(_config.WindowFrame, stdout);

            _frameDrawnComplete.Set();
        }

        private void RenderHelpWindow(TextWriter stdout)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var originalForeground = Console.ForegroundColor;
            var originalBackground = Console.BackgroundColor;
            var longestEntry = _config.HelpScreen.HelpEntries.OrderByDescending(x => x.Key.Length + x.Description.Length).FirstOrDefault();
            var helpWidth = longestEntry.Key.Length + longestEntry.Description.Length + 4;
            var helpHeight = _config.HelpScreen.HelpEntries.Count;

            var helpStartX = width / 2 - helpWidth / 2;
            var helpStartY = height / 2 - helpHeight / 2;
            var i = 0;
            Console.SetCursorPosition(helpStartX, helpStartY);

            var foreColor = _config.HelpScreen.ForegroundColor ?? _config.ColorPalette.Get(_config.HelpScreen.ForegroundColorPalette) ?? Style.Foreground;
            var backColor = _config.HelpScreen.BackgroundColor ?? _config.ColorPalette.Get(_config.HelpScreen.BackgroundColorPalette) ?? Style.Background;
            var shadowColor = System.Drawing.Color.FromArgb(backColor.A, (int)Math.Max(backColor.R * 0.5, 0), (int)Math.Max(backColor.G * 0.5, 0), (int)Math.Max(backColor.B * 0.5, 0));
            // if we don't have space for the shadow, just draw the background color
            if (ColorTracker.Count >= ColorPalette.MaxColors)
                shadowColor = backColor;
            ColorTracker.SetForeColor(backColor);
            Console.BackgroundColor = originalBackground;
            stdout.Write(new string('▄', helpWidth + 2)); // draw a smaller top margin
            ColorTracker.SetForeColor(foreColor);
            i++;
            foreach (var entry in _config.HelpScreen.HelpEntries)
            {
                ColorTracker.SetBackColor(backColor);
                Console.SetCursorPosition(helpStartX, helpStartY + i);
                var content = $"{entry.Key}: {entry.Description}";
                var spaces = helpWidth - content.Length;
                stdout.Write($"  {content}");
                stdout.Write(new string(' ', spaces));
                if (_config.HelpScreen.EnableDropShadow)
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
            if (_config.HelpScreen.EnableDropShadow)
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
            _config.DataContext.SetData<bool>("IsSearchEnabled", isSearchEnabled);
        }

        private void SetSearchString(string str)
        {
            _config.DataContext.SetData<string>("SearchString", str);
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
            _config.DataContext.SetData<int>("SearchMatches", matches.Count);
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
            _config.DataContext.SetData<int>("SearchIndex", _searchLineIndex);
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
            _config.DataContext.SetData<int>("SearchIndex", _searchLineIndex);
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
