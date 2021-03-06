﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static AnyConsole.ExtendedConsole;

namespace AnyConsole
{
    /// <summary>
    /// Extended console
    /// </summary>
    public interface IExtendedConsole : IDisposable
    {
        /// <summary>
        /// Console options
        /// </summary>
        ConsoleOptions Options { get; }

        /// <summary>
        /// Set/get the console encoding
        /// </summary>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// True if stdout is redirected
        /// </summary>
        bool IsOutputRedirected { get; }

        /// <summary>
        /// True if stderr is redirected
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// Set the cursor position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetCursorPosition(int x, int y);

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
        /// Window left position
        /// </summary>
        int WindowLeft { get; set; }

        /// <summary>
        /// Window top position
        /// </summary>
        int WindowTop { get; set; }

        /// <summary>
        /// Window height
        /// </summary>
        int WindowHeight { get; set; }

        /// <summary>
        /// Window width
        /// </summary>
        int WindowWidth { get; set; }

        /// <summary>
        /// Cursor left position
        /// </summary>
        int CursorLeft { get; set; }

        /// <summary>
        /// Cursor top position
        /// </summary>
        int CursorTop { get; set; }

        /// <summary>
        /// Console title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Get/set the foreground color
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Get/set the background color
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Get/set if the cursor should be visible
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Get/set the font name
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Get/set the font X size
        /// </summary>
        short FontXSize { get; set; }

        /// <summary>
        /// Get/set the font Y size
        /// </summary>
        short FontYSize { get; set; }

        /// <summary>
        /// Get/set the font weight
        /// </summary>
        int FontWeight { get; set; }

        /// <summary>
        /// Check if a character is in a given font
        /// </summary>
        /// <param name="character"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        bool CheckIfCharInFont(char character, Font font);

        /// <summary>
        /// Get the character ranges available for a font
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        ICollection<FontRange> GetFontUnicodeRanges(Font font);

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
        /// Writes the specified string value, followed by the current line terminator to the standard input stream
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(StringBuilder text);

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator to the standard input stream
        /// </summary>
        /// <param name="textBuilder"></param>
        void WriteLine(ColorTextBuilder textBuilder);

        /// <summary>
        /// Writes the specified string value to the standard input stream
        /// </summary>
        /// <param name="text"></param>
        void Write(string text);

        /// <summary>
        /// Writes the specified string value to the standard input stream
        /// </summary>
        /// <param name="text"></param>
        void Write(StringBuilder text);

        /// <summary>
        /// Writes the specified string value to the standard input stream
        /// </summary>
        /// <param name="textBuilder"></param>
        void Write(ColorTextBuilder textBuilder);

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

        void WriteAscii(string text);
        void WriteAscii(StringBuilder text);
        void WriteAscii(ColorTextBuilder textBuilder);

        /// <summary>
        /// Write raw text that will not be formatted and will not contain prepend string
        /// </summary>
        /// <param name="text"></param>
        void WriteRaw(string text);

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        void WriteAt(string text, int xPos, int yPos, DirectOutputMode directOutputMode, Color? foregroundColor = null, Color? backgroundColor = null);

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        void WriteAt(string text, int xPos, int yPos);

        /// <summary>
        /// Write directly at a given position of the screen
        /// </summary>
        /// <param name="textBuilder"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="directOutputMode"></param>
        void WriteAt(ColorTextBuilder textBuilder, int xPos, int yPos, DirectOutputMode directOutputMode);

        /// <summary>
        /// Clear the console log
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove a statically placed text entry from start position to end position
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        int ClearAtRange(int startX, int startY, int endX, int endY);

        /// <summary>
        /// Remove a statically placed text entry
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int ClearAt(int x, int y);
    }
}
