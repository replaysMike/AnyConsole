using System.Collections.Generic;
using Console = Colorful.Console;

namespace AnyConsole
{
    /// <summary>
    /// Color tracks how many colors are currently used in the active console
    /// </summary>
    public static class ColorTracker
    {
        public static HashSet<System.Drawing.Color> _colorTracker = new HashSet<System.Drawing.Color>();

        public static int Count {
            get
            {
                return _colorTracker.Count;
            }
        }

        public static void Clear()
        {
            _colorTracker.Clear();
        }

        public static void SetForeColor(System.Drawing.Color color)
        {
            AddColor(color);
            Console.ForegroundColor = color;
        }

        public static void SetBackColor(System.Drawing.Color color)
        {
            AddColor(color);
            Console.BackgroundColor = color;
        }

        public static void AddColor(System.Drawing.Color color)
        {
            if (_colorTracker.Contains(color))
            {
                // do nothing
            }
            else
            {
                if (_colorTracker.Count >= ColorPalette.MaxColors)
                    throw new ColorPaletteException(ColorPalette.MaxColorsExceededMessage);
                _colorTracker.Add(color);
            }
        }
    }
}
