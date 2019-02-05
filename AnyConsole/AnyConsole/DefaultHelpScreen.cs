using System.Collections.Generic;

namespace AnyConsole
{
    /// <summary>
    /// The default help screen
    /// </summary>
    public class DefaultHelpScreen : IHelpScreen
    {
        public ICollection<HelpEntry> HelpEntries { get; }

        public DefaultHelpScreen()
        {
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
