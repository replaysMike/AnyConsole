using System.Drawing;

namespace AnyConsole
{
    public class DirectOutputEntry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; }
        public DirectOutputMode DirectOutputMode { get; set; }
        public bool IsDisplayed { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public DirectOutputEntry(string text, int x, int y, DirectOutputMode directOutputMode, Color? foregroundColor, Color? backgroundColor)
        {
            Text = text;
            X = x;
            Y = y;
            DirectOutputMode = directOutputMode;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
