using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Window frame
    /// </summary>
    public class WindowFrame
    {
        public Color Color { get; set; }
        public int Size { get; set; }

        /// <summary>
        /// Create a window frame
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public WindowFrame(Color color, int size)
        {
            Color = color;
            Size = size;
        }

        /// <summary>
        /// No window frame
        /// </summary>
        public static WindowFrame None => new WindowFrame(Color.Transparent, 0);
    }
}
