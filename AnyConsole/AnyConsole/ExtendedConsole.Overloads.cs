using Colorful;
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
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, default(Color?), default(Color?), null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="componentParameter">A parameter to pass to the component</param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, object componentParameter)
        {
            AddRowContent(rowName, null, component, string.Empty, location, 0, componentParameter, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, label, component, string.Empty, location, 0, componentParameter, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, null, component, componentName, location, 0, null, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, label, component, componentName, location, 0, null, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, default(Color?), null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="componentName">The name of the custom component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor">Foreground color</param>
        public void WriteRow(string rowName, Component component, string componentName, ColumnLocation location, Enum foregroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, null, null);
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
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, null, null);
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
        public void WriteRow(string rowName, string label, Component component, string componentName, ColumnLocation location, Enum foregroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, null, null);
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
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, backgroundColor, null);
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
        public void WriteRow(string rowName, Component component, string componentName, ColumnLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, null, component, componentName, location, 0, null, foregroundColor, backgroundColor, null);
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
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, backgroundColor, null);
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
        public void WriteRow(string rowName, string label, Component component, string componentName, ColumnLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            if (component != Component.Custom)
                throw new InvalidOperationException($"Argument component must be specified as Custom when providing a componentName of '{componentName}'");
            AddRowContent(rowName, label, component, componentName, location, 0, null, foregroundColor, backgroundColor, null);
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
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Enum foregroundColor)
        {
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, null, null);
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
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location, Enum foregroundColor)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, null, null);
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
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, backgroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, Component component, ColumnLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            AddRowContent(rowName, null, component, string.Empty, location, 0, null, foregroundColor, backgroundColor, null);
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
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, backgroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="label">A label to prepend to the value</param>
        /// <param name="component">The component to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string label, Component component, ColumnLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            AddRowContent(rowName, label, component, string.Empty, location, 0, null, foregroundColor, backgroundColor, null);
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
            AddRowContent(rowName, text, location, 0, foregroundColor, null, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Enum foregroundColor)
        {
            AddRowContent(rowName, text, location, 0, foregroundColor, null, null);
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
            AddRowContent(rowName, text, location, 0, foregroundColor, backgroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location">The location to render the text</param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public void WriteRow(string rowName, string text, ColumnLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            AddRowContent(rowName, text, location, 0, foregroundColor, backgroundColor, null);
        }

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location">The location to render the text</param>
        public void WriteRow(string rowName, string text, ColumnLocation location)
        {
            AddRowContent(rowName, text, location, 0, default(Color?), default(Color?), null);
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
            AddRowContent(rowName, text, location, offset, default(Color?), default(Color?), null);
        }

        /// <summary>
        /// Write to a static row in an ascii font
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location">The location to render the text</param>
        /// <param name="offset"></param>
        public void WriteAsciiRow(string rowName, string text, ColumnLocation location, int offset)
        {
            AddRowContent(rowName, text, location, offset, default(Color?), default(Color?), FigletFont.Default);
        }

        /// <summary>
        /// Write to a static row in an ascii font
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location">The location to render the text</param>
        /// <param name="offset"></param>
        /// <param name="font">The font to use</param>
        public void WriteAsciiRow(string rowName, string text, ColumnLocation location, int offset, FigletFont font)
        {
            AddRowContent(rowName, text, location, offset, default(Color?), default(Color?), font);
        }

        private void AddRowContent(string rowName, string text, ColumnLocation location, int offset, Color? foregroundColor, Color? backgroundColor, FigletFont font)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(text, location, offset, foregroundColor, backgroundColor, font));
        }

        private void AddRowContent(string rowName, string text, ColumnLocation location, int offset, Enum foregroundColor, Enum backgroundColor, FigletFont font)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(text, location, offset, foregroundColor, backgroundColor, font));
        }

        private void AddRowContent(string rowName, string label, Component component, string componentName, ColumnLocation location, int offset, object componentParameter, Color? foregroundColor, Color? backgroundColor, FigletFont font)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(component, label, componentName, location, offset, componentParameter, foregroundColor, backgroundColor, font));
        }

        private void AddRowContent(string rowName, string label, Component component, string componentName, ColumnLocation location, int offset, object componentParameter, Enum foregroundColor, Enum backgroundColor, FigletFont font)
        {
            if (!_staticRowContentBuilder.ContainsKey(rowName))
                _staticRowContentBuilder.Add(rowName, new List<RowContent>());
            _staticRowContentBuilder[rowName].Add(new RowContent(component, label, componentName, location, offset, componentParameter, foregroundColor, backgroundColor, font));
        }
    }
}
