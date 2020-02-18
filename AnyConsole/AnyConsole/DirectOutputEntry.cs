namespace AnyConsole
{
    public class DirectOutputEntry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; }
        public DirectOutputMode DirectOutputMode { get; set; }
        public bool IsDisplayed { get; set; }

        public DirectOutputEntry(string text, int x, int y, DirectOutputMode directOutputMode)
        {
            Text = text;
            X = x;
            Y = y;
            DirectOutputMode = directOutputMode;
        }
    }
}
