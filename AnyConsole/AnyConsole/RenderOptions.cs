using System;

namespace AnyConsole
{
    /// <summary>
    /// The rendering options
    /// </summary>
    [Flags]
    public enum RenderOptions
    {
        None = 0,
        FadeHistory = 1,
        HideCursor = 2,
    }
}
