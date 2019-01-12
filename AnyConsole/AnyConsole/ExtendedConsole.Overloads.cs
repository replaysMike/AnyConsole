using System;
using System.Collections.Generic;
using System.Drawing;

namespace AnyConsole
{
    public partial class ExtendedConsole
    {
        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location)
        {
            AddRowContent(rowName, () => ComponentRenderer.Render(component), location, 0, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor)
        {
            AddRowContent(rowName, () => ComponentRenderer.Render(component), location, 0, foreColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor, Color backColor)
        {
            AddRowContent(rowName, () => ComponentRenderer.Render(component), location, 0, foreColor, backColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Color foreColor)
        {
            AddRowContent(rowName, text, location, 0, foreColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Color foreColor, Color backColor)
        {
            AddRowContent(rowName, text, location, 0, foreColor, backColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location)
        {
            AddRowContent(rowName, text, location, 0, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="offset"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, int offset)
        {
            AddRowContent(rowName, text, location, offset, null, null);
        }

        private void AddRowContent(string rowName, string text, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(() => text, location, offset, foreColor, backColor));
        }

        private void AddRowContent(string rowName, Func<string> renderer, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(renderer, location, offset, foreColor, backColor));
        }
    }
}
