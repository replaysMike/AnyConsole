using System.Drawing;

namespace AnyConsole
{
    public struct ColoredTextFragment
    {
        public string Text { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public ColoredTextFragment(string text) : this(text, null, null)
        {
        }

        public ColoredTextFragment(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            Text = text;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
