using System;

namespace AnyConsole
{
    public class HelpEntry
    {
        /// <summary>
        /// The key shortcut
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Helpful description
        /// </summary>
        public string Description { get; }

        public HelpEntry(ConsoleKey key, string description)
        {
            Key = key.ToString();
            Description = description;
        }

        public HelpEntry(string key, string description)
        {
            Key = key;
            Description = description;
        }

    }
}
