using System.Collections.Generic;
using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// ExtendedConsole Configuration
    /// </summary>
    public class ExtendedConsoleConfiguration
    {
        internal LogHistoryContainer LogHistoryContainer { get; }
        internal ICollection<StaticRowConfig> StaticRows { get; }
        internal WindowFrame WindowFrame { get; set; }

        public ExtendedConsoleConfiguration()
        {
            StaticRows = new List<StaticRowConfig>();
            LogHistoryContainer = new LogHistoryContainer(RowLocation.Top, 0);
            WindowFrame = WindowFrame.None;
        }

        /// <summary>
        /// Set a window frame
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public void SetWindowFrame(Color color, int size)
        {
            WindowFrame = new WindowFrame(color, size);
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        public void SetStaticRow(string name, RowLocation location)
        {
            SetStaticRow(name, location, 0, null, null);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        public void SetStaticRow(string name, RowLocation location, Color? foregroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, null);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, Color? foregroundColor, Color? backgroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="index">The 0-based row index to offset from the location</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, int index, Color? foregroundColor, Color? backgroundColor)
        {
            StaticRows.Add(new StaticRowConfig { Name = name, Location = location, Index = index, ForegroundColor = foregroundColor, BackgroundColor = backgroundColor });
        }
    }
}
