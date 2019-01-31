using System;

namespace AnyConsole
{
    /// <summary>
    /// Console redirection log entry
    /// </summary>
    public class ConsoleLogEntry
    {
        public string OriginalLine { get; set; } = string.Empty;
        public string TruncatedLine { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// Console redirection log entry
        /// </summary>
        /// <param name="originalLine"></param>
        /// <param name="maxWidth"></param>
        public ConsoleLogEntry(string originalLine, int maxWidth)
        {
            OriginalLine = originalLine;
            TruncatedLine = TruncateLine(FormatLine(originalLine), maxWidth);
        }

        private string TruncateLine(string line, int maxWidth)
        {
            var stringLength = line.Length + ClassName.Length;
            if (stringLength > maxWidth)
                return line.Substring(stringLength - maxWidth);
            return line;
        }

        private string FormatLine(string line)
        {
            var parts = line.Split(new string[] { "|" }, StringSplitOptions.None);
            if (parts.Length >= 3)
            {
                var classParts = parts[parts.Length - 3].Split(new string[] { "." }, StringSplitOptions.None);
                ClassName = classParts[classParts.Length - 1];
            }
            if (parts.Length >= 2)
                return parts[parts.Length - 2];
            return line;
        }
    }
}
