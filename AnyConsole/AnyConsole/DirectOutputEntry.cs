using System.Drawing;

namespace AnyConsole
{
    public class DirectOutputEntry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ColorTextBuilder TextBuilder { get; set; }
        public string Text { get; set; }
        public DirectOutputMode DirectOutputMode { get; set; }
        public bool IsDisplayed { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public bool Clear { get; set; }
        public bool IsCleared { get; set; }
        public int Length => Text != null ? Text.Length : (TextBuilder?.Length ?? 0);

        public DirectOutputEntry(string text, int x, int y, DirectOutputMode directOutputMode, Color? foregroundColor, Color? backgroundColor)
        {
            Text = text;
            X = x;
            Y = y;
            DirectOutputMode = directOutputMode;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public DirectOutputEntry(ColorTextBuilder textBuilder, int x, int y, DirectOutputMode directOutputMode)
        {
            TextBuilder = textBuilder;
            X = x;
            Y = y;
            DirectOutputMode = directOutputMode;
        }
    }
}
