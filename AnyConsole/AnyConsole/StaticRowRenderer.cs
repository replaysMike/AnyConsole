using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Console = Colorful.Console;

namespace AnyConsole
{
    public class StaticRowRenderer
    {
        /// <summary>
        /// Render an empty row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public StaticRowStream Build(StaticRowConfig row)
        {
            var stream = new StaticRowStream(row, null);
            return stream;
        }

        /// <summary>
        /// Render a row with content
        /// </summary>
        /// <param name="row"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public StaticRowStream Build(StaticRowConfig row, ICollection<RowContent> content)
        {
            var stream = new StaticRowStream(row, content);

            return stream;
        }
    }

    public class StaticRowStream
    {
        private StaticRowConfig _row;
        private readonly ICollection<RowContent> _content;
        public StaticRowStream(StaticRowConfig row, ICollection<RowContent> content)
        {
            _row = row;
            _content = content;
        }

        public string Write(TextWriter output)
        {
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var h = Console.BufferHeight;
            var lh = Console.LargestWindowHeight;

            var builder = new StringBuilder();
            var xPosition = 0;
            var yPosition = 0;
            var originalForeColor = Console.ForegroundColor;
            var originalBackColor = Console.BackgroundColor;

            // render the background for the row
            Console.ForegroundColor = _row.ForegroundColor ?? originalForeColor;
            Console.BackgroundColor = _row.BackgroundColor ?? originalBackColor;
            var y = GetYPosition(_row.Location, _row.Index, yPosition);
            Console.SetCursorPosition(0, y);
            if (y == height - 1) width -= 1;
            builder.Append(new string(' ', width));
            output.Write(builder.ToString());
            // reset position
            Console.SetCursorPosition(cursorLeft, cursorTop);
            builder.Clear();

            if (_content?.Any() == true)
            {
                foreach (var item in _content)
                {
                    Console.ForegroundColor = item.ForegroundColor ?? _row.ForegroundColor ?? originalForeColor;
                    Console.BackgroundColor = item.BackgroundColor ?? _row.BackgroundColor ?? originalBackColor;
                    var x = GetXPosition(item.Location, xPosition);
                    y = GetYPosition(_row.Location, _row.Index, yPosition);
                    var str = item.Render.Invoke();
                    builder.Append(str);
                    Console.SetCursorPosition(x, y);
                    output.Write(str);
                    xPosition += str.Length;
                }
            }
            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;

            // return the rendered string
            return builder.ToString();
        }

        private int GetXPosition(ColumnLocation loc, int currentOffset)
        {
            var width = Console.WindowWidth;
            var x = 0;
            switch (loc)
            {
                case ColumnLocation.Left:
                    x = currentOffset;
                    break;
                case ColumnLocation.Right:
                    x = width - currentOffset - 1;
                    break;
                case ColumnLocation.Center:
                    break;
            }
            return x;
        }

        private int GetYPosition(RowLocation loc, int index, int currentOffset)
        {
            var height = Console.WindowHeight;
            var y = 0;
            switch (loc)
            {
                case RowLocation.Top:
                    y = currentOffset + index;
                    break;
                case RowLocation.Bottom:
                    y = height - index - currentOffset - 1;
                    break;
                case RowLocation.Middle:
                    break;
            }
            return y;
        }
    }
}
