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
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="componentParameter">A parameter to pass to the component</param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location, object componentParameter)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, componentParameter, null, null);
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
            AddRowContent(rowName, null, component, componentName, location, 0, null, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, string label, Component component, string componentName, ColumnLocation location)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, label, component, componentName, location, 0, null, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor">Foreground color</param>
        public void WriteRow(string rowName, Component component, string componentName, ColumnLocation location, Color foregroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor">Foreground color</param>
        public void WriteRow(string rowName, string label, Component component, string componentName, ColumnLocation location, Color foregroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        public void WriteRow(string rowName, Component component, string componentName, ColumnLocation location, Color foregroundColor, Color backgroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        public void WriteRow(string rowName, string label, Component component, string componentName, ColumnLocation location, Color foregroundColor, Color backgroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foregroundColor)
        {
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location, Color foregroundColor)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Color foregroundColor, Color backgroundColor)
        {
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location, Color foregroundColor, Color backgroundColor)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Color foregroundColor)
        {
            AddRowContent(rowName, text, location, 0, foregroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Color foregroundColor, Color backgroundColor)
        {
            AddRowContent(rowName, text, location, 0, foregroundColor, backgroundColor);
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

        private void AddRowContent(string rowName, string text, ColumnLocation location, int offset, Color? foregroundColor, Color? backgroundColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(text, location, offset, foregroundColor, backgroundColor));
        }

        private void AddRowContent(string rowName, string label, Component component, string componentName, ColumnLocation location, int offset, object componentParameter, Color? foregroundColor, Color? backgroundColor)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(component, label, componentName, location, offset, componentParameter, foregroundColor, backgroundColor));
        }
    }
}
