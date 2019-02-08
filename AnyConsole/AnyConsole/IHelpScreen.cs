using System;
using System.Collections.Generic;

namespace AnyConsole
{
    /// <summary>
    /// Help screen
    /// </summary>
    public interface IHelpScreen
    {
        /// <summary>
        /// Get the help entries to display
        /// </summary>
        ICollection<HelpEntry> HelpEntries { get; }

        /// <summary>
        /// Get the foreground color
        /// </summary>
        System.Drawing.Color? ForegroundColor { get; }

        /// <summary>
        /// Get the background color
        /// </summary>
        System.Drawing.Color? BackgroundColor { get; }

        /// <summary>
        /// Get the foreground color palette
        /// </summary>
        Enum ForegroundColorPalette { get; }

        /// <summary>
        /// Get the background color palette
        /// </summary>
        Enum BackgroundColorPalette { get; }

        /// <summary>
        /// True to draw a drop shadow
        /// </summary>
        bool EnableDropShadow { get; set; }
    }
}
