using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Console Options
    /// </summary>
    public class ConsoleOptions
    {
        private static readonly RenderOptions _defaultRenderOptions = RenderOptions.HideCursor | RenderOptions.FadeHistory;
        private static readonly InputOptions _defaultInputOptions = InputOptions.UseBuiltInKeyOperations;
        private static readonly int _defaultTextSpacing = 1;

        /// <summary>
        /// Console render options
        /// </summary>
        public RenderOptions RenderOptions { get; set; }

        /// <summary>
        /// Input handling options
        /// </summary>
        public InputOptions InputOptions { get; set; }

        /// <summary>
        /// The position of the console window
        /// </summary>
        public Rectangle? Container { get; set; }

        /// <summary>
        /// The spacing width between text components (default: 1 char)
        /// </summary>
        public int TextSpacing { get; set; }

        /// <summary>
        /// Create a new console options
        /// </summary>
        public ConsoleOptions() : this(_defaultRenderOptions)
        {
            
        }

        public ConsoleOptions(int textSpacing) : this(_defaultRenderOptions, _defaultInputOptions, textSpacing, null, null)
        {

        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="renderOptions">Rendering options</param>
        public ConsoleOptions(RenderOptions renderOptions) : this(renderOptions, _defaultInputOptions, _defaultTextSpacing, null, null)
        {
        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="inputOptions">Input handling options</param>
        public ConsoleOptions(InputOptions inputOptions) : this(_defaultRenderOptions, inputOptions, _defaultTextSpacing, null, null)
        {
        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="renderOptions">Rendering options</param>
        /// <param name="inputOptions">Input handling options</param>
        public ConsoleOptions(RenderOptions renderOptions, InputOptions inputOptions) : this(renderOptions, inputOptions, _defaultTextSpacing, null, null)
        {
        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="renderOptions">Rendering options</param>
        /// <param name="inputOptions">Input handling options</param>
        /// <param name="position">Position of the console</param>
        /// <param name="size">Size of the window</param>
        public ConsoleOptions(RenderOptions renderOptions, InputOptions inputOptions, int textSpacing, Point? position, Size? size)
        {
            TextSpacing = textSpacing;
            RenderOptions = renderOptions;
            Container = new Rectangle(position ?? Point.Empty, size ?? Size.Empty);
        }
    }
}
