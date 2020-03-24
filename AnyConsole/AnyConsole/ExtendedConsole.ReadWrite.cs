using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Colorful;
using Console = Colorful.Console;

namespace AnyConsole
{
    public partial class ExtendedConsole
    {
        public int WindowLeft { get { return Console.WindowLeft; } set { Console.WindowLeft = value; } }
        public int WindowTop { get { return Console.WindowTop; } set { Console.WindowTop = value; } }
        public int WindowHeight { get { return Console.WindowHeight; } set { Console.WindowHeight = value; } }
        public int WindowWidth { get { return Console.WindowWidth; } set { Console.WindowWidth = value; } }

        public int CursorLeft { get { return Console.CursorLeft; } set { Console.CursorLeft = value; } }
        public int CursorTop { get { return Console.CursorTop; } set { Console.CursorTop = value; } }
        public bool CursorVisible { get { return Console.CursorVisible; } set { Console.CursorVisible = value; } }

        public Color ForegroundColor { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; } }
        public Color BackgroundColor { get { return Console.BackgroundColor; } set { Console.BackgroundColor = value; } }
        public string Title { get { return Console.Title; } set { Console.Title = value; } }
        public string FontName
        {
            get {
                var font = GetCurrentFont();
                return font.FontName;
            }
            set {
                SetCurrentFont(value);
            }
        }

        public short FontXSize { get { var font = GetCurrentFont(); return font.XSize; } set { SetCurrentFont(FontName, value, null, null); } }
        public short FontYSize { get { var font = GetCurrentFont(); return font.YSize; } set { SetCurrentFont(FontName, null, value, null); } }
        public int FontWeight { get { var font = GetCurrentFont(); return font.Weight; } set { SetCurrentFont(FontName, null, null, value); } }

        public bool CheckIfCharInFont(char character, Font font) => CheckIfCharacterInFont(character, font);
        public ICollection<FontRange> GetFontUnicodeRanges(Font font) => GetUnicodeRangesForFont(font);

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
        public void WriteLine(StringBuilder text) => Console.WriteLine(text.ToString());

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text) => Console.Write(text);

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text"></param>
        public void Write(StringBuilder text) => Console.Write(text.ToString());

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
        public void WriteLine(StringBuilder text, FigletFont font) => WriteLine(text.ToString(), font);

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="textBuilder"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode">The mode which indicates when the text should be cleared</param>
        public void WriteLine(ColorTextBuilder textBuilder)
        {
            // not yet supported
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
        /// Write using a Figlet font
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public void Write(StringBuilder text, FigletFont font) => Write(text.ToString(), font);

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="textBuilder"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode">The mode which indicates when the text should be cleared</param>
        public void Write(ColorTextBuilder textBuilder)
        {
            // not yet supported
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
        /// Write using the default Figlet font
        /// </summary>
        /// <param name="text"></param>
        public void WriteAscii(StringBuilder text) => WriteAscii(text.ToString());

        /// <summary>
        /// Write using the default Figlet font
        /// </summary>
        /// <param name="text"></param>
        public void WriteAscii(ColorTextBuilder textBuilder) => WriteAscii(textBuilder.ToString());

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

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public void WriteAt(string text, int xPos, int yPos)
        {
            WriteAt(text, xPos, yPos, DirectOutputMode.ClearOnChange, null, null);
        }

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode">The mode which indicates when the text should be cleared</param>
        public void WriteAt(string text, int xPos, int yPos, DirectOutputMode directOutputMode, Color? foregroundColor = null, Color? backgroundColor = null)
        {
            var left = Console.CursorLeft;
            var top = Console.CursorTop;
            Console.SetCursorPosition(xPos, yPos);
            _historyLock.Wait();
            try
            {
                _directOutputEntries.Add(new DirectOutputEntry(text, xPos, yPos, directOutputMode, foregroundColor, backgroundColor));
            }
            finally
            {
                _historyLock.Release();
            }
            Console.SetCursorPosition(left, top);
            _hasLogUpdates = true;
        }

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="textBuilder"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode">The mode which indicates when the text should be cleared</param>
        public void WriteAt(ColorTextBuilder textBuilder, int xPos, int yPos, DirectOutputMode directOutputMode)
        {
            var left = Console.CursorLeft;
            var top = Console.CursorTop;
            Console.SetCursorPosition(xPos, yPos);
            _historyLock.Wait();
            try
            {
                _directOutputEntries.Add(new DirectOutputEntry(textBuilder, xPos, yPos, directOutputMode));
            }
            finally
            {
                _historyLock.Release();
            }
            Console.SetCursorPosition(left, top);
            _hasLogUpdates = true;
        }

        /// <summary>
        /// Remove a statically placed text entry
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public int ClearAt(int x, int y)
        {
            var entriesCleared = 0;
            _historyLock.Wait();
            try
            {
                entriesCleared = _directOutputEntries
                    .Where(i => i.X == x && i.Y == y)
                    .Select(i => i.Clear = true)
                    .Count();
            }
            finally
            {
                _historyLock.Release();
            }
            _hasLogUpdates = true;
            return entriesCleared;
        }

        /// <summary>
        /// Remove a statically placed text entry from start position to end position
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public int ClearAtRange(int startX, int startY, int endX, int endY)
        {
            var entriesCleared = 0;
            _historyLock.Wait();
            try
            {
                entriesCleared = _directOutputEntries
                    .Where(i => i.X >= startX && i.X <= endX && i.Y >= startY && i.Y <= endY)
                    .Select(i => i.Clear = true)
                    .Count();
            }
            finally
            {
                _historyLock.Release();
            }
            _hasLogUpdates = true;
            return entriesCleared;
        }

        /// <summary>
        /// Clear the console log
        /// </summary>
        public void Clear()
        {
            _historyLock.Wait();
            try
            {
                _directOutputEntries.Clear();
                _screenLogBuilder.Clear();
                _fullLogHistory.Clear();
                _displayHistory.Clear();
                _clearRequested = true;
            }
            finally
            {
                _historyLock.Release();
            }
            _hasLogUpdates = true;
        }

        public void Flush()
        {
            _frameDrawnComplete.WaitOne();
        }
    }
}
