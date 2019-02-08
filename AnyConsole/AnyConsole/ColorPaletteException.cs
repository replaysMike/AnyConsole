using System;

namespace AnyConsole
{
    /// <summary>
    /// Color palette exception
    /// </summary>
    public class ColorPaletteException : Exception
    {
        /// <summary>
        /// Create a color palette exception
        /// </summary>
        /// <param name="message"></param>
        public ColorPaletteException(string message) : base(message)
        {

        }
    }
}
