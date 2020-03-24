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
        public int? MaxLength;

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
                Append(builder);
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

        public ColorTextBuilder AppendLine(ColorTextBuilder builder)
        {
            TextFragments.AddRange(builder.TextFragments);
            TextFragments.Add(new ColoredTextFragment(Environment.NewLine));
            return this;
        }

        public ColorTextBuilder AppendLine(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(text + Environment.NewLine, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendLineIf(bool condition, string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition)
                AppendLine(text, foregroundColor, backgroundColor);
            return this;
        }

        public ColorTextBuilder AppendLine()
        {
            TextFragments.Add(new ColoredTextFragment(Environment.NewLine));
            return this;
        }

        public ColorTextBuilder Append(Func<int, string> action, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Add(new ColoredTextFragment(action.Invoke(Length), foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder AppendIf(bool condition, Func<int, string> action, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition)
                Append(action, foregroundColor, backgroundColor);
            return this;
        }

        public ColorTextBuilder AppendIf(Func<int, bool> condition, Func<int, string> action, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition?.Invoke(Length) == true)
                Append(action, foregroundColor, backgroundColor);
            return this;
        }

        public ColorTextBuilder Append(Func<int, ColorTextBuilder> action)
        {
            TextFragments.AddRange(action.Invoke(Length).TextFragments);
            return this;
        }

        public ColorTextBuilder AppendIf(bool condition, Func<int, ColorTextBuilder> action)
        {
            if (condition)
                Append(action);
            return this;
        }

        public ColorTextBuilder AppendIf(Func<int, bool> condition, Func<int, ColorTextBuilder> action)
        {
            if (condition?.Invoke(Length) == true)
                Append(action);
            return this;
        }

        public ColorTextBuilder Truncate(int length)
        {
            MaxLength = length;
            return this;
        }

        public ColorTextBuilder TruncateIf(bool condition, int length)
        {
            if (condition)
                MaxLength = length;
            return this;
        }

        public ColorTextBuilder TruncateIf(Func<int, bool> condition, int length)
        {
            if (condition?.Invoke(Length) == true)
                MaxLength = length;
            return this;
        }

        /// <summary>
        /// Interlace two builders together on the Y axis
        /// </summary>
        /// <param name="builder2"></param>
        /// <param name="xSpacing">Optional amount of spacing between lines on X axis</param>
        /// <returns></returns>
        public ColorTextBuilder Interlace(ColorTextBuilder builder2, int xSpacing = 0)
        {
            var interlacedBuilder = new ColorTextBuilder();

            var index = 0;
            foreach(var line in TextFragments)
            {
                interlacedBuilder.TextFragments.Add(line);
                if (builder2.TextFragments.Count > index)
                {
                    if (xSpacing > 0)
                        interlacedBuilder.TextFragments.Add(new ColoredTextFragment(new string(' ', xSpacing), line.ForegroundColor, line.BackgroundColor));
                    interlacedBuilder.TextFragments.Add(builder2.TextFragments[index]);
                }
                index++;
            }

            return interlacedBuilder;
        }

        public override string ToString()
        {
            var str = string.Join("", TextFragments.Select(x => x.Text));
            if (MaxLength.HasValue)
                str = str.Substring(0, MaxLength.Value);
            return str;
        }

        public static implicit operator string(ColorTextBuilder textBuilder) => textBuilder.ToString();
    }
}
