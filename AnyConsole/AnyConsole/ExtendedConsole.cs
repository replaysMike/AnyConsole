using System;
using System.Collections.Generic;
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
    public partial class ExtendedConsole : IDisposable
    {
        private static readonly int _drawingIntervalMs = 66;
        private static readonly int _defaultBufferHistoryLinesLength = 1024;
        private StringBuilder _screenHeaderBuilder = new StringBuilder();
        private StringBuilder _screenLogBuilder = new StringBuilder();
        private IDictionary<string, ICollection<RowContent>> _staticRowContentBuilder = new Dictionary<string, ICollection<RowContent>>();
        private ICollection<ConsoleLogEntry> _logHistory = new List<ConsoleLogEntry>();
        private Thread _drawingThread;
        private Thread _inputThread;
        private ManualResetEvent _isRunning;
        private bool _isDisposed;
        private ExtendedConsoleConfiguration _config;
        private StaticRowRenderer _staticRowRenderer = new StaticRowRenderer();
        private int _bufferYCursor = 0;
        
        /// <summary>
        /// Console options
        /// </summary>
        public ConsoleOptions Options { get; }

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
        }

        /// <summary>
        /// Start the console
        /// </summary>
        public void Start()
        {
            InitializeConsole();
        }

        /// <summary>
        /// Block until the application has closed
        /// </summary>
        public void WaitForClose()
        {
            _isRunning.WaitOne();
        }

        public string ReadLine() => Console.ReadLine();
        public void WriteLine() => Console.WriteLine();
        public void WriteLine(string text) => Console.WriteLine(text);
        public void Write(string text) => Console.Write(text);

        private void InitializeConsole()
        {
            // initialize the database update threads
            _isRunning = new ManualResetEvent(false);
            _inputThread = new Thread(new ThreadStart(InputThread));
            _inputThread.IsBackground = true;
            _inputThread.Start();
            _drawingThread = new Thread(new ThreadStart(DrawingThread));
            _drawingThread.IsBackground = true;
            _drawingThread.Start();
        }

        private void DrawingThread()
        {
            // if there is no console available, no need to display anything.
            if (_screenLogBuilder == null)
                return;
            var screenHeaderBuilder = new StringBuilder();
            var fullLogHistory = new List<ConsoleLogEntry>();

            while (!_isRunning.WaitOne(_drawingIntervalMs))
            {
                fullLogHistory = ProcessBufferedOutput(_screenLogBuilder, fullLogHistory);
                var displayHistory = TrimBufferForDisplay(fullLogHistory);
                DrawStaticHeader(screenHeaderBuilder, displayHistory);
            }
        }

        private void InputThread()
        {
            // if there is no console available, no need to display anything.
            if (_screenLogBuilder == null)
                return;

            while (!_isRunning.WaitOne(1))
            {
                var hWnd = GetStdHandle(STD_INPUT_HANDLE);
                var buffer = new INPUT_RECORD[128];
                var eventsRead = 0U;
                var success = GetNumberOfConsoleInputEvents(hWnd, out eventsRead);
                if (eventsRead > 0)
                {
                    var keyRead = ReadConsoleInput(hWnd, buffer, (uint)buffer.Length, out eventsRead);
                    if (keyRead)
                    {
                        for (var z = 0; z < eventsRead; z++)
                        {
                            switch (buffer[z].EventType)
                            {
                                case KEYBOARD_EVENT:
                                    //Console.Write($"{buffer[z].KeyEvent.wVirtualKeyCode}");
                                    switch(buffer[z].KeyEvent.wVirtualKeyCode)
                                    {
                                        case 36:// end
                                            // scroll to start
                                            _bufferYCursor = _defaultBufferHistoryLinesLength;
                                            break;
                                        case 27:// esc
                                        case 35:// home
                                            // scroll to end
                                            _bufferYCursor = 0;
                                            break;
                                        case 81:// q
                                            Dispose();
                                            break;
                                    }
                                    break;
                                case MOUSE_EVENT:
                                    if (buffer[z].MouseEvent.dwEventFlags == MOUSE_WHEELED)
                                    {
                                        var isWheelUp = buffer[z].MouseEvent.dwButtonState == MOUSE_WHEELUP;
                                        var isWheelDown = buffer[z].MouseEvent.dwButtonState == MOUSE_WHEELDOWN;
                                        if (isWheelUp)
                                            _bufferYCursor--;
                                        if (isWheelDown)
                                            _bufferYCursor++;
                                    }
                                    break;
                            }
                        }
                    }
                }
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

                if(Options.Container.HasValue && Options.Container.Value.Size.Width > 0 && Options.Container.Value.Size.Height > 0)
                    Console.SetWindowSize(Options.Container.Value.Size.Width, Options.Container.Value.Size.Height);
                Console.BufferHeight = Console.WindowHeight;
                Console.SetOut(consoleOutputStringWriter);
                Console.BackgroundColor = Style.Background;
                Console.Clear();
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
            var displayHeight = Console.WindowHeight - 3;
            if (_bufferYCursor < 0)
                _bufferYCursor = 0;
            if (_bufferYCursor > logHistory.Count - displayHeight)
                _bufferYCursor = logHistory.Count - displayHeight;
            var skipAmount = logHistory.Count - displayHeight - _bufferYCursor;
            return logHistory.Skip(skipAmount).Take(displayHeight).ToList();
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
            var pendingLines = screenLogBuilder.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pendingLine in pendingLines)
                logHistory.Add(new ConsoleLogEntry(pendingLine, Console.WindowWidth));

            // remove older items not shown to the screen as it's not needed anymore
            //var linesToRemove = Math.Abs(height - logHistory.Count);
            var linesToRemove = Math.Abs(_defaultBufferHistoryLinesLength - logHistory.Count);
            
            if (logHistory.Count > _defaultBufferHistoryLinesLength && logHistory.Count >= linesToRemove)
                logHistory.RemoveRange(0, linesToRemove);
            screenLogBuilder.Clear();

            return logHistory;
        }

        private void DrawStaticHeader(StringBuilder infoBuilder, ICollection<ConsoleLogEntry> log)
        {
            // we will write any screen contents to stderr, as stdout is redirected internally
            var stdout = Console.Error;
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var width = Console.WindowWidth;
            
            Console.CursorVisible = !Options.RenderOptions.HasFlag(RenderOptions.HideCursor);
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

            // restore cursor
            Console.SetCursorPosition(GetXPosition(_config.LogHistoryContainer, 0), GetYPosition(_config.LogHistoryContainer, 0));
            Console.CursorVisible = true;
            // write the screen log buffer
            var i = 0;
            var linesToFade = Options.RenderOptions.HasFlag(RenderOptions.FadeHistory) ? 6 : 0;
            foreach (var logLine in log)
            {
                // fade the long lines away
                var logForegroundColor = Style.Foreground;
                var classNameForegroundColor = Style.ClassName;
                if (logLine.OriginalLine.Contains("|WARN|"))
                    logForegroundColor = Style.WarningText;
                if (logLine.OriginalLine.Contains("|ERROR|"))
                    logForegroundColor = Style.ErrorText;

                if (i < linesToFade)
                {
                    logForegroundColor = System.Drawing.Color.FromArgb(Math.Max(Style.Background.R + 20, logForegroundColor.R - ((linesToFade - i) * 25)), Math.Max(Style.Background.G + 20, logForegroundColor.G - ((linesToFade - i) * 25)), Math.Max(Style.Background.B + 20, logForegroundColor.B - ((linesToFade - i) * 25)));
                    //classNameForegroundColor = System.Drawing.Color.FromArgb(Math.Max(Style.Background.R + 20, classNameForegroundColor.R - ((linesToFade - i) * 25)), Math.Max(Style.Background.G + 20, classNameForegroundColor.G - ((linesToFade - i) * 25)), Math.Max(Style.Background.B + 20, classNameForegroundColor.B - ((linesToFade - i) * 25)));
                }
                Console.ForegroundColor = classNameForegroundColor;
                var className = $"{logLine.ClassName}> ";
                stdout.Write(className);

                Console.ForegroundColor = logForegroundColor;
                var spaces = (Console.WindowWidth - className.Length - logLine.TruncatedLine.Length);
                if (spaces < 0)
                    spaces = 0;
                stdout.Write(logLine.TruncatedLine + new string(' ', spaces));
                i++;
            }

            if (_config.WindowFrame.Size > 0)
                WindowFrameRenderer.Render(_config.WindowFrame, stdout);

            Console.CursorVisible = !Options.RenderOptions.HasFlag(RenderOptions.HideCursor);
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

        private void DrawShutdown()
        {
            Console.ForegroundColor = Style.ServiceErrorText;
            Console.Error.WriteLine("Shutdown requested...");
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
                DrawShutdown();

                _isRunning?.Set();
                _drawingThread?.Join(500);
                _inputThread?.Join(500);
                _drawingThread = null;
                _inputThread = null;
                _isRunning?.Dispose();
                _isRunning = null;
            }
            _isDisposed = true;
        }

        #endregion
    }
}
