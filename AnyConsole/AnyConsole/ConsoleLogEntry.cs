using System;

namespace AnyConsole
{
    /// <summary>
    /// Console redirection log entry
    /// </summary>
    public class ConsoleLogEntry
    {
        /// <summary>
        /// A special encoding character to indicate processing should be disabled
        /// </summary>
        public const char DisableProcessingCode = '\u0026';

        public bool DisableProcessing { get; set; }
        public string OriginalLine { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Prepend { get; set; } = string.Empty;

        /// <summary>
        /// Console redirection log entry
        /// </summary>
        /// <param name="originalLine"></param>
        public ConsoleLogEntry(string originalLine, string prepend, bool disableProcessing)
        {
            OriginalLine = originalLine;
            Prepend = prepend;
            DisableProcessing = disableProcessing;
        }

        /// <summary>
        /// Get the truncated line for display
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="prefixLength"></param>
        /// <returns></returns>
        public string GetTruncatedLine(int maxWidth, int prefixLength)
        {
            return TruncateLine(FormatLine(OriginalLine), maxWidth, prefixLength);
        }

        private string TruncateLine(string line, int maxWidth, int prefixLength)
        {
            var stringLength = line.Length + ClassName.Length + prefixLength;
            if (stringLength > maxWidth)
                return line.Substring(stringLength - maxWidth);
            return line;
        }

        private string FormatLine(string line)
        {
            if (!DisableProcessing)
            {
                var parts = line.Split(new string[] { "|" }, StringSplitOptions.None);
                if (parts.Length >= 3)
                {
                    var classParts = parts[parts.Length - 3].Split(new string[] { "." }, StringSplitOptions.None);
                    ClassName = classParts[classParts.Length - 1];
                }
                if (parts.Length >= 2)
                    return parts[parts.Length - 2];
            }
            return line;
        }
    }
}
