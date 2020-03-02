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
        /// The renderer responsible for rendering components
        /// </summary>
        internal ComponentRenderer ComponentRenderer { get; }

        internal ConsoleOptions ConsoleOptions { get; }

        internal ExtendedConsoleConfiguration Config { get; }

        public StaticRowRenderer(ExtendedConsoleConfiguration config, ComponentRenderer componentRenderer, ConsoleOptions options)
        {
            Config = config;
            ComponentRenderer = componentRenderer;
            ConsoleOptions = options;
        }

        /// <summary>
        /// Render an empty row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public StaticRowStream Build(StaticRowConfig row)
        {
            var stream = new StaticRowStream(this, row, null);
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
            var stream = new StaticRowStream(this, row, content);

            return stream;
        }
    }

    public class StaticRowStream
    {
        private StaticRowConfig _row;
        private readonly ICollection<RowContent> _content;
        private StaticRowRenderer _renderer;
        private int _leftMargin = 0;
        private int _rightMargin = 0;

        public StaticRowStream(StaticRowRenderer renderer, StaticRowConfig row, ICollection<RowContent> content)
        {
            _renderer = renderer;
            _row = row;
            _content = content;
        }

        public string Write(TextWriter output, bool forceDraw = false)
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

            if (_content?.Any() == true)
            {
                var contentHasUpdates = _renderer.ComponentRenderer.HasUpdates(_content) || forceDraw;

                if (contentHasUpdates)
                {
                    // render the background for the row
                    var foreColor = _row.ForegroundColor ?? _renderer.Config.ColorPalette.Get(_row.ForegroundColorPalette) ?? originalForeColor;
                    var backColor = _row.BackgroundColor ?? _renderer.Config.ColorPalette.Get(_row.BackgroundColorPalette) ?? originalBackColor;
                    ColorTracker.SetForeColor(foreColor);
                    ColorTracker.SetBackColor(backColor);
                    var y = GetYPosition(_row.Location, _row.Index, yPosition);
                    Console.SetCursorPosition(0, y);
                    if (y == height - 1) width -= 1;
                    builder.Append(new string(' ', width));
                    output.Write(builder.ToString());
                    // reset position
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    builder.Clear();

                    foreach (var item in _content)
                    {
                        var itemColor = item.ForegroundColor ?? _renderer.Config.ColorPalette.Get(item.ForegroundColorPalette) ?? _row.ForegroundColor ?? _renderer.Config.ColorPalette.Get(_row.ForegroundColorPalette) ?? originalForeColor;
                        var itemBackColor = item.BackgroundColor ?? _renderer.Config.ColorPalette.Get(item.BackgroundColorPalette) ?? _row.BackgroundColor ?? _renderer.Config.ColorPalette.Get(_row.BackgroundColorPalette) ?? originalBackColor;
                        ColorTracker.SetForeColor(itemColor);
                        ColorTracker.SetBackColor(itemBackColor);

                        // render the component
                        string renderedContent = null;
                        if (item.ContentType == RowContent.ContentTypes.Static)
                            renderedContent = item.StaticContent;
                        else if (item.ContentType == RowContent.ContentTypes.Component)
                            renderedContent = $"{item.Label}{_renderer.ComponentRenderer.Render(item.Component, item.ComponentName, item.ComponentParameter)}";

                        if (item.Location == ColumnLocation.Right)
                            renderedContent = new string(' ', _renderer.ConsoleOptions.TextSpacing) + renderedContent;
                        else
                            renderedContent = renderedContent + new string(' ', _renderer.ConsoleOptions.TextSpacing);

                        builder.Append(renderedContent);

                        y = GetYPosition(_row.Location, _row.Index, yPosition);
                        var x = GetXPosition(item.Location, y, renderedContent.Length, _leftMargin, _rightMargin);
                        Console.SetCursorPosition(x, y);
                        output.Write(renderedContent);
                        xPosition += renderedContent.Length;
                        if (item.Location == ColumnLocation.Right)
                            _rightMargin += renderedContent.Length;
                        else
                            _leftMargin += renderedContent.Length;
                        item.RenderCount++;
                    }
                }
            }
            ColorTracker.SetForeColor(originalForeColor);
            ColorTracker.SetBackColor(originalBackColor);

            // return the rendered string
            return builder.ToString();
        }

        private int GetXPosition(ColumnLocation loc, int y, int contentWidth, int leftMargin, int rightMargin)
        {
            var width = Console.WindowWidth;
            var x = 0;
            switch (loc)
            {
                case ColumnLocation.Left:
                    x = leftMargin;
                    break;
                case ColumnLocation.Right:
                    x = width - contentWidth - rightMargin - (y == Console.WindowHeight - 1 ? 1 : 0);
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
