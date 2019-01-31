using System;

namespace AnyConsole
{
    /// <summary>
    /// The rendering options
    /// </summary>
    [Flags]
    public enum RenderOptions
    {
        /// <summary>
        /// No render options
        /// </summary>
        None = 0,
        /// <summary>
        /// Fade the historical log near the top of the screen
        /// </summary>
        FadeHistory = 1,
        /// <summary>
        /// Hide the text cursor
        /// </summary>
        HideCursor = 2,
        /// <summary>
        /// Hide the row prefix character
        /// </summary>
        HideLogRowPrefix = 4,
        /// <summary>
        /// Hide row class prefix
        /// </summary>
        HideClassNamePrefix = 8
    }
}
