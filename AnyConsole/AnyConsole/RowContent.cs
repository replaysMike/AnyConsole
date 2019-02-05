using System.Drawing;

namespace AnyConsole
{
    public class RowContent
    {
        public enum ContentTypes
        {
            Static,
            Component
        }

        public ContentTypes ContentType { get; }
        public string Label { get; }
        public string StaticContent { get; }
        public Component Component { get; }
        public object ComponentParameter { get; }
        public string ComponentName { get; }
        public ColumnLocation Location { get; }
        public int Offset { get; }
        public Color? ForegroundColor { get; }
        public Color? BackgroundColor { get; }
        public long RenderCount { get; set; }

        public RowContent(Component component, string componentName) : this(component, null, componentName, ColumnLocation.Left, 0, null, null, null)
        {
        }

        public RowContent(Component component, string label, string componentName) : this(component, label, componentName, ColumnLocation.Left, 0, null, null, null)
        {
        }

        public RowContent(Component component, string componentName, int offset) : this(component, null, componentName, ColumnLocation.Left, offset, null, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location) : this(component, null, componentName, location, 0, null, null, null)
        {
        }

        public RowContent(Component component, string label, string componentName, ColumnLocation location) : this(component, label, componentName, location, 0, null, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location, int offset) : this(component, null, componentName, location, offset, null, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location, int offset, Color foreColor) : this(component, null, componentName, location, offset, null, foreColor, null)
        {
        }

        public RowContent(Component component, string label, string componentName, ColumnLocation location, int offset, object componentParameter, Color? foreColor, Color? backColor)
        {
            ContentType = ContentTypes.Component;
            Label = label;
            Component = component;
            ComponentParameter = componentParameter;
            ComponentName = componentName;
            Location = location;
            Offset = offset;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }

        public RowContent(string staticContent, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            ContentType = ContentTypes.Static;
            Component = Component.StaticContent;
            StaticContent = staticContent;
            Location = location;
            Offset = offset;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }

        public override string ToString()
        {
            if (Component == Component.Custom)
                return $"{ComponentName}";
            else if (Component == Component.StaticContent)
                return StaticContent;
            else
                return Component.ToString();
        }
    }
}
