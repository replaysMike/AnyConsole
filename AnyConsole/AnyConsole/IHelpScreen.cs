using System.Collections.Generic;

namespace AnyConsole
{
    public interface IHelpScreen
    {
        ICollection<HelpEntry> HelpEntries { get; }
    }
}
