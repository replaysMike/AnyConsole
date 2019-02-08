using System;
using System.Collections.Generic;

namespace AnyConsole
{
    /// <summary>
    /// The default help screen
    /// </summary>
    public class DefaultHelpScreen : IHelpScreen
    {
        public ICollection<HelpEntry> HelpEntries { get; }
        public System.Drawing.Color? ForegroundColor { get; }
        public System.Drawing.Color? BackgroundColor { get; }
        public Enum ForegroundColorPalette { get; }
        public Enum BackgroundColorPalette { get; }
        public bool EnableDropShadow { get; set; }

        public DefaultHelpScreen(Enum foregroundColor, Enum backgroundColor) : this()
        {
            ForegroundColorPalette = foregroundColor;
            BackgroundColorPalette = backgroundColor;
        }

        public DefaultHelpScreen(System.Drawing.Color foregroundColor, System.Drawing.Color backgroundColor) : this()
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public DefaultHelpScreen()
        {
            EnableDropShadow = true;
            HelpEntries = new List<HelpEntry>
            {
                new HelpEntry("MouseScroll", "Scroll history buffer"),
                new HelpEntry(System.ConsoleKey.Home, "Start of history buffer"),
                new HelpEntry("End, Esc", "End of history buffer"),
                new HelpEntry("CTRL-S", "Search history buffer"),
                new HelpEntry(System.ConsoleKey.F3, "Find Next"),
                new HelpEntry("SHIFT-F3", "Find Previous"),
                new HelpEntry(System.ConsoleKey.H, "Help screen"),
                new HelpEntry(System.ConsoleKey.Q, "Quit"),
            };
        }
    }
}
