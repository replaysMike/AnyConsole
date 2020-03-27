using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AnyConsole
{
    /// <summary>
    /// Builds colored text strings
    /// </summary>
    public class ColorTextBuilder
    {
        public List<ColoredTextFragment> TextFragments { get; private set; }

        /// <summary>
        /// Get the total text length the string
        /// </summary>
        public int Length => TextFragments?.Sum(x => x.Text?.Length ?? 0) ?? 0;

        /// <summary>
        /// Get the calculated width of the longest line in the text block
        /// </summary>
        public int Width => GetFragmentWidth();

        /// <summary>
        /// Get the calculated height of the text block
        /// </summary>
        public int Height => GetFragmentHeight();

        /// <summary>
        /// Get/set the total maximum length of the string
        /// </summary>
        public int? MaxLength { get; set; }

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

        private int GetFragmentWidth()
        {
            var width = 0;
            var line = new StringBuilder();
            foreach(var fragment in TextFragments)
            {
                line.Append(fragment.Text);
                if (fragment.Text.Contains(Environment.NewLine))
                {
                    line.Replace(Environment.NewLine, string.Empty);
                    width = Math.Max(line.Length, width);
                    line = new StringBuilder();
                }
            }
            return width;
        }

        private int GetFragmentHeight()
        {
            var str = ToString().Replace("\r", string.Empty);
            var lineCount = str.Count(c => c == '\n');

            // if there is any trailing text after the final newline, count it as a new line
            var lastNewline = str.LastIndexOf('\n');
            if (lastNewline < str.Length - 1)
                lineCount++;

            return lineCount;
        }

        public ColorTextBuilder Prepend(ColorTextBuilder builder)
        {
            TextFragments.InsertRange(0, builder.TextFragments);
            return this;
        }

        public ColorTextBuilder PrependIf(bool condition, ColorTextBuilder builder)
        {
            if (condition)
                Prepend(builder);
            return this;
        }

        public ColorTextBuilder Prepend(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Insert(0, new ColoredTextFragment(text, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder PrependIf(bool condition, string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition)
                TextFragments.Insert(0, new ColoredTextFragment(text, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder PrependLine(ColorTextBuilder builder)
        {
            TextFragments.Insert(0, new ColoredTextFragment(Environment.NewLine));
            TextFragments.InsertRange(0, builder.TextFragments);
            return this;
        }

        public ColorTextBuilder PrependLine(string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            TextFragments.Insert(0, new ColoredTextFragment(text + Environment.NewLine, foregroundColor, backgroundColor));
            return this;
        }

        public ColorTextBuilder PrependLineIf(bool condition, string text, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            if (condition)
                PrependLine(text, foregroundColor, backgroundColor);
            return this;
        }

        public ColorTextBuilder PrependLine()
        {
            TextFragments.Insert(0, new ColoredTextFragment(Environment.NewLine));
            return this;
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

        public ColorTextBuilder Clear()
        {
            TextFragments.Clear();
            return this;
        }

        /// <summary>
        /// Interlace two builders together on the Y axis
        /// </summary>
        /// <param name="builder2"></param>
        /// <param name="fixedColumnSpacing">Optional amount of spacing between lines on X axis</param>
        /// <param name="fixedColumnWidth">Optional width of fixed columns for the data being interlaced</param>
        /// <returns></returns>
        public ColorTextBuilder Interlace(ColorTextBuilder builder2, int fixedColumnSpacing = 0, int fixedColumnWidth = 0)
        {
            var interlacedBuilder = new ColorTextBuilder();

            var enumerator1 = TextFragments.GetEnumerator();
            var enumerator2 = builder2.TextFragments.GetEnumerator();

            var processedRightCount = 0;
            var currentLineWidth = 0;
            while (enumerator1.MoveNext())
            {
                var line = enumerator1.Current;
                if (line.Text.Contains(Environment.NewLine))
                {
                    // move to the next builder
                    line.Text = line.Text.Replace(Environment.NewLine, string.Empty);
                    // add spaces to format content in a column if asked
                    if (fixedColumnWidth > 0 && line.Text.Length > fixedColumnWidth)
                        line.Text = line.Text.Substring(0, fixedColumnWidth);
                    if (fixedColumnWidth > 0 && line.Text.Length < fixedColumnWidth)
                        line.Text = line.Text + new string(' ', fixedColumnWidth - (currentLineWidth + line.Text.Length));
                    currentLineWidth = 0;
                    interlacedBuilder.TextFragments.Add(line);

                    // add spaces if we are requested to separate the blocks of text
                    if (fixedColumnSpacing > 0)
                        interlacedBuilder.TextFragments.Add(new ColoredTextFragment(new string(' ', fixedColumnSpacing), line.ForegroundColor, line.BackgroundColor));

                    // start printing the next builder
                    var hasRightSideLines = false;
                    while (enumerator2.MoveNext())
                    {
                        processedRightCount++;
                        hasRightSideLines = true;
                        var line2 = enumerator2.Current;
                        interlacedBuilder.TextFragments.Add(line2);
                        if (line2.Text.Contains(Environment.NewLine))
                        {
                            // done with this line, move back to parent builder and start the next line
                            break;
                        }
                    }
                    // if we ran out of right side items, add a line break
                    if (!hasRightSideLines)
                        interlacedBuilder.TextFragments.Add(new ColoredTextFragment(Environment.NewLine));
                }
                else
                {
                    interlacedBuilder.TextFragments.Add(line);
                    currentLineWidth = line.Text.Length;
                }
            }

            // extra right side data
            if (processedRightCount < builder2.TextFragments.Count)
            {
                var startOfLine = true;
                while (enumerator2.MoveNext())
                {
                    var line2 = enumerator2.Current;
                    if (startOfLine)
                    {
                        var leftPadding = new string(' ', fixedColumnWidth + fixedColumnSpacing);
                        interlacedBuilder.TextFragments.Add(new ColoredTextFragment(leftPadding));
                    }
                    interlacedBuilder.TextFragments.Add(line2);
                    if (line2.Text.Contains(Environment.NewLine))
                    {
                        startOfLine = true;
                        // done with this line, move back to parent builder and start the next line
                        continue;
                    }
                    startOfLine = false;
                }
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
