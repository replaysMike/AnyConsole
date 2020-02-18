using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Internal styles for unspecified colors
    /// </summary>
    internal static class Style
    {
        internal static readonly Color _background = Color.FromArgb(20, 20, 20);
        internal static readonly Color _foreground = Color.FromArgb(200, 200, 200);
        internal static readonly Color _className = Color.FromArgb(0, 200, 0);

        internal static readonly Color _menuBackground = Color.FromArgb(80, 0, 0);
        internal static readonly Color _applicationName = Color.FromArgb(240, 0, 0);
        internal static readonly Color _menuText = Color.FromArgb(255, 255, 0);
        internal static readonly Color _menuSecondaryBackground = Color.FromArgb(50, 50, 50);
        internal static readonly Color _menuSecondaryText = Color.FromArgb(80, 80, 80);

        internal static readonly Color _errorText = Color.FromArgb(100, 0, 0);
        internal static readonly Color _warningText = Color.FromArgb(100, 100, 0);
        internal static readonly Color _serviceErrorText = Color.FromArgb(255, 0, 0);

        internal static readonly Color _helpForeground = Color.FromArgb(200, 200, 200);
        internal static readonly Color _helpBackground = Color.FromArgb(20, 20, 20);
    }
}
