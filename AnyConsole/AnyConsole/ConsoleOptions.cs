using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Console Options
    /// </summary>
    public class ConsoleOptions
    {
        /// <summary>
        /// Console render options
        /// </summary>
        public RenderOptions RenderOptions { get; set; }

        /// <summary>
        /// The position of the console window
        /// </summary>
        public Rectangle? Container { get; set; }

        /// <summary>
        /// Create a new console options
        /// </summary>
        public ConsoleOptions() : this(RenderOptions.None)
        {
            
        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="renderOptions"></param>
        public ConsoleOptions(RenderOptions renderOptions) : this(renderOptions, null, null)
        {
        }

        /// <summary>
        /// Create a new console options
        /// </summary>
        /// <param name="renderOptions">Rendering options</param>
        /// <param name="position">Position of the console</param>
        /// <param name="size">Size of the window</param>
        public ConsoleOptions(RenderOptions renderOptions, Point? position, Size? size)
        {
            RenderOptions = renderOptions;
            Container = new Rectangle(position ?? Point.Empty, size ?? Size.Empty);
        }
    }
}
