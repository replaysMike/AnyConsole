using System;
using System.Drawing;

namespace AnyConsole
{
    public class RowContent
    {
        public Func<string> Render { get; set; }
        public ColumnLocation Location { get; set; }
        public int Offset { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public RowContent(Func<string> renderer) : this(renderer, ColumnLocation.Left, 0, null, null)
        {
        }

        public RowContent(Func<string> renderer, int offset) : this(renderer, ColumnLocation.Left, offset, null, null)
        {
        }

        public RowContent(Func<string> renderer, ColumnLocation location) : this(renderer, location, 0, null, null)
        {
        }

        public RowContent(Func<string> renderer, ColumnLocation location, int offset) : this(renderer, location, offset, null, null)
        {
        }

        public RowContent(Func<string> renderer, ColumnLocation location, int offset, Color foreColor) : this(renderer, location, offset, foreColor, null)
        {
        }

        public RowContent(Func<string> renderer, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            Render = renderer;
            Location = location;
            Offset = offset;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }
    }
}
