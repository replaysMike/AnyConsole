using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnyConsole
{
    public class StaticRowConfig
    {
        /// <summary>
        /// The name of the row
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The locked location of the row
        /// </summary>
        public RowLocation Location { get; internal set; }

        /// <summary>
        /// The index number of the row
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// The default foreground color for the row
        /// </summary>
        public Color? ForegroundColor { get; internal set; }

        /// <summary>
        /// The default background color for the row
        /// </summary>
        public Color? BackgroundColor { get; internal set; }
    }
}
