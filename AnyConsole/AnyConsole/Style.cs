using System.Drawing;

namespace AnyConsole
{
    internal static class Style
    {
        internal static readonly Color Background = Color.FromArgb(20, 20, 20);
        internal static readonly Color Foreground = Color.FromArgb(200, 200, 200);
        internal static readonly Color ClassName = Color.FromArgb(0, 200, 0);

        internal static readonly Color MenuBackground = Color.FromArgb(80, 0, 0);
        internal static readonly Color ApplicationName = Color.FromArgb(240, 0, 0);
        internal static readonly Color MenuText = Color.FromArgb(255, 255, 0);
        internal static readonly Color MenuSecondaryBackground = Color.FromArgb(50, 50, 50);
        internal static readonly Color MenuSecondaryText = Color.FromArgb(80, 80, 80);

        internal static readonly Color ErrorText = Color.FromArgb(100, 0, 0);
        internal static readonly Color WarningText = Color.FromArgb(100, 100, 0);
        internal static readonly Color ServiceErrorText = Color.FromArgb(255, 0, 0);

        internal static readonly Color HelpForeground = Color.FromArgb(200, 200, 200);
        internal static readonly Color HelpBackground = Color.FromArgb(20, 20, 20);
    }
}
