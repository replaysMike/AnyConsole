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
        public string StaticContent { get; }
        public Component Component { get; }
        public string ComponentName { get; }
        public ColumnLocation Location { get; }
        public int Offset { get; }
        public Color? ForegroundColor { get; }
        public Color? BackgroundColor { get; }

        public RowContent(Component component, string componentName) : this(component, componentName, ColumnLocation.Left, 0, null, null)
        {
        }

        public RowContent(Component component, string componentName, int offset) : this(component, componentName, ColumnLocation.Left, offset, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location) : this(component, componentName, location, 0, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location, int offset) : this(component, componentName, location, offset, null, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location, int offset, Color foreColor) : this(component, componentName, location, offset, foreColor, null)
        {
        }

        public RowContent(Component component, string componentName, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            ContentType = ContentTypes.Component;
            Component = component;
            ComponentName = componentName;
            Location = location;
            Offset = offset;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }

        public RowContent(string staticContent, ColumnLocation location, int offset, Color? foreColor, Color? backColor)
        {
            ContentType = ContentTypes.Static;
            StaticContent = staticContent;
            Location = location;
            Offset = offset;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }
    }
}
