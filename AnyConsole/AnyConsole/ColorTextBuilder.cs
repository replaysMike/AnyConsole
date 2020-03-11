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
        public List<ColoredTextFragment> TextFragments { get; private set; }

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

        public ColorTextBuilder Append(ColorTextBuilder builder)
        {
            TextFragments.AddRange(builder.TextFragments);
            return this;
        }

        public ColorTextBuilder AppendIf(bool condition, ColorTextBuilder builder)
        {
            if (condition)
                TextFragments.AddRange(builder.TextFragments);
            return this;
        }

        public ColorTextBuilder Append(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(text, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendIf(bool condition, string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition)
                TextFragments.Add(new ColoredTextFragment(text, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendLine(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(text + Environment.NewLine, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendLineIf(bool condition, string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if(condition)
                TextFragments.Add(new ColoredTextFragment(text + Environment.NewLine, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendLine()
        {
            TextFragments.Add(new ColoredTextFragment(Environment.NewLine));
            return this;
        }

        public override string ToString() => string.Join("", TextFragments.Select(x => x.Text));

        public static implicit operator string(ColorTextBuilder textBuilder) => textBuilder.ToString();
    }
}
