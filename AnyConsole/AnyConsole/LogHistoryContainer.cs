namespace AnyConsole
{
    public class LogHistoryContainer
    {
        public RowLocation Location { get; set; }
        public int Index { get; set; }
        public LogHistoryContainer(RowLocation location, int index)
        {
            Location = location;
            Index = index;
        }
    }
}
