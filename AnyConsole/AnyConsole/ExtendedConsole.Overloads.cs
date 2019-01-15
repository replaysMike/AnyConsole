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
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, Component component, ColumnLocation location)
        {
            AddRowContent(rowName, component, string.Empty, location, 0, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, Component component, string componentName, ColumnLocation location)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, component, componentName, location, 0, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foreColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor)
        {
            AddRowContent(rowName, component, string.Empty, location, 0, foreColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foreColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor, Color backColor)
        {
            AddRowContent(rowName, component, string.Empty, location, 0, foreColor, backColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location">The location to render the text</param>
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
        /// <param name="location">The location to render the text</param>
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
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, string text, ColumnLocation location)
        {
            AddRowContent(rowName, text, location, 0, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location">The location to render the text</param>
        /// <param name="offset"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, int offset)
        {
            AddRowContent(rowName, text, location, offset, null, null);
        }

        private void AddRowContent(string rowName, string text, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(text, location, offset, foreColor, backColor));
        }

        private void AddRowContent(string rowName, Component component, string componentName, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(component, componentName, location, offset, foreColor, backColor));
        }
    }
}
