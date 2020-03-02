using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnyConsole
{
    /// <summary>
    /// Builds colored text strings
    /// </summary>
    public class ColorTextBuilder
    {
        public ICollection<ColoredTextFragment> TextFragments { get; private set; }

        public int Length => TextFragments?.Sum(x => x.Text?.Length ?? 0) ?? 0;

        /// <summary>
        /// Create a new TextBuilder
        /// </summary>
        public static ColorTextBuilder Create => new ColorTextBuilder();

        /// <summary>
        /// Builds colored text strings
        /// </summary>
        public ColorTextBuilder()
        {
            TextFragments = new List<ColoredTextFragment>(10);
        }

        public ColorTextBuilder Append(string text)
        {
            TextFragments.Add(new ColoredTextFragment(text, Color.Gray));
            return this;
        }

        public ColorTextBuilder Append(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(text, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendLine(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(text + Environment.NewLine, foregroundColor, backgroundColor));
            return this;
        }

        public override string ToString()
        {
            return string.Join("", TextFragments);
        }
    }
}
