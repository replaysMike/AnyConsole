using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using Console = Colorful.Console;

namespace AnyConsole
{
    /// <summary>
    /// Window frame renderer
    /// </summary>
    public class WindowFrameRenderer
    {
        /// <summary>
        /// Render a window frame
        /// </summary>
        /// <param name="windowFrame"></param>
        /// <param name="output"></param>
        public static void Render(WindowFrame windowFrame, TextWriter output)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var row = new string(' ', width);
            var lastRow = new string(' ', width - 1);
            var originalBackColor = Console.BackgroundColor;
            ColorTracker.SetBackColor(windowFrame.Color);
            Console.SetCursorPosition(0, 0);
            output.Write(row);
            Console.SetCursorPosition(0, height - 1);
            output.Write(lastRow);

            // write vertical bars
            for(var y = 1; y < height - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                output.Write(' ');
                Console.SetCursorPosition(width - 1, y);
                output.Write(' ');
            }
            Console.BackgroundColor = originalBackColor;
        }
    }
}
