using Colorful;
using Console = Colorful.Console;

namespace AnyConsole
{
    public partial class ExtendedConsole
    {
        /// <summary>
        /// Read from stdinput
        /// </summary>
        /// <returns></returns>
        public string ReadLine() => Console.ReadLine();

        /// <summary>
        /// Write a blank line
        /// </summary>
        public void WriteLine() => Console.WriteLine();

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text"></param>
        public void WriteLine(string text) => Console.WriteLine(text);

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text) => Console.Write(text);

        /// <summary>
        /// Write using a Figlet font
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public void WriteLine(string text, FigletFont font)
        {
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
            Console.WriteAscii(text, font);
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
        }

        /// <summary>
        /// Write using a Figlet font
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public void Write(string text, FigletFont font)
        {
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
            Console.WriteAscii(text, font);
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
        }

        /// <summary>
        /// Write using the default Figlet font
        /// </summary>
        /// <param name="text"></param>
        public void WriteAscii(string text)
        {
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
            Console.WriteAscii(text);
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
        }

        /// <summary>
        /// Write raw text that will not be formatted and will not contain prepend string
        /// </summary>
        /// <param name="text"></param>
        public void WriteRaw(string text)
        {
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
            Console.Write(text);
            Console.Write(ConsoleLogEntry.DisableProcessingCode);
        }
    }
}
