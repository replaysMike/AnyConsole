using System;
using System.Collections.Generic;
using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Console color palette
    /// </summary>
    public class ColorPalette
    {
        /// <summary>
        /// The maximum number of colors that can be drawn at once
        /// </summary>
        public const int MaxColors = 16;
        internal const string MaxColorsExceededMessage = "You cannot allocate more than 16 colors.";

        /// <summary>
        /// Get the colors in the palette
        /// </summary>
        public IDictionary<Enum, Color> Palette { get; }

        public ColorPalette()
        {
            Palette = new Dictionary<Enum, Color>();
        }

        public ColorPalette(IDictionary<Enum, Color> palette)
        {
            if (palette.Count > MaxColors)
                throw new ColorPaletteException(MaxColorsExceededMessage);
            Palette = palette;
        }

        /// <summary>
        /// Add a color to the palette
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void AddColor(Enum name, Color color)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if(Palette.Count >= MaxColors)
                throw new ColorPaletteException(MaxColorsExceededMessage);
            Palette.Add(name, color);
        }

        /// <summary>
        /// Get a color from the palette by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Color? Get(Enum name)
        {
            if(name != null && Palette.ContainsKey(name))
                return Palette[name];
            return null;
        }

        /// <summary>
        /// Remove a color from the palette
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool RemoveColor(Enum name, Color color)
        {
            var isRemoved = false;
            if(name != null && Palette.ContainsKey(name))
                isRemoved = Palette.Remove(name);
            return isRemoved;
        }

        public static implicit operator Dictionary<Enum, Color>(ColorPalette colorPalette)
        {
            return (Dictionary<Enum, Color>)colorPalette.Palette;
        }

        public static implicit operator ColorPalette(Dictionary<Enum, Color> colorPalette)
        {
            return new ColorPalette(colorPalette);
        }
    }
}
