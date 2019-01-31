using System.Drawing;

namespace AnyConsole
{
    public class LogHistoryContainer
    {
        public RowLocation Location { get; set; }
        public int Index { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public LogHistoryContainer(RowLocation location, int index)
        {
            Location = location;
            Index = index;
        }
    }
}
