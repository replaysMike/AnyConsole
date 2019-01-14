using System;
using System.Drawing;

namespace AnyConsole
{
    /// <summary>
    /// Extended console
    /// </summary>
    public interface IExtendedConsole
    {
        /// <summary>
        /// Console options
        /// </summary>
        ConsoleOptions Options { get; }

        /// <summary>
        /// Configure the console
        /// </summary>
        /// <param name="config"></param>
        void Configure(Action<ExtendedConsoleConfiguration> config);

        /// <summary>
        /// Start the console
        /// </summary>
        void Start();

        /// <summary>
        /// Close the console
        /// </summary>
        void Close();

        /// <summary>
        /// Block until the application has closed
        /// </summary>
        void WaitForClose();

        /// <summary>
        /// Reads the next line of characters from the standard input stream
        /// </summary>
        /// <returns></returns>
        string ReadLine();

        /// <summary>
        /// Writes the current line terminator to the standard input stream
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the standard input stream
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text);

        /// <summary>
        /// Writes the specified string value to the standard input stream
        /// </summary>
        /// <param name="text"></param>
        void Write(string text);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        void WriteRow(string rowName, Component component, ColumnLocation location);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="component">The component to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        void WriteRow(string rowName, Component component, ColumnLocation location, Color foreColor, Color backColor);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        void WriteRow(string rowName, string text, ColumnLocation location, Color foreColor);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text">The text to render</param>
        /// <param name="location"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        void WriteRow(string rowName, string text, ColumnLocation location, Color foreColor, Color backColor);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        void WriteRow(string rowName, string text, ColumnLocation location);

        /// <summary>
        /// Write to a static row
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="offset"></param>
        void WriteRow(string rowName, string text, ColumnLocation location, int offset);
    }
}
