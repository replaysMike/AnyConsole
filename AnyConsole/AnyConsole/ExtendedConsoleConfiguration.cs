﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AnyConsole
{
    /// <summary>
    /// ExtendedConsole Configuration
    /// </summary>
    public class ExtendedConsoleConfiguration
    {
        private const int DefaultMaxHistoryLines = 1000;
        internal LogHistoryContainer LogHistoryContainer { get; }
        internal ICollection<StaticRowConfig> StaticRows { get; }
        internal IDictionary<string, Type> CustomComponents { get; }
        internal WindowFrame WindowFrame { get; set; }
        internal ConsoleDataContext DataContext { get; set; }
        internal TimeSpan RedrawTimeSpan { get; set; }
        internal int MaxHistoryLines { get; set; }
        internal IHelpScreen HelpScreen { get; set; }
        internal ColorPalette ColorPalette { get; set; }
        internal string Prepend { get; set; } = "> ";
        internal Action<ExtendedConsole> QuitHandler { get; set; }


        public ExtendedConsoleConfiguration()
        {
            MaxHistoryLines = DefaultMaxHistoryLines;
            StaticRows = new List<StaticRowConfig>();
            CustomComponents = new Dictionary<string, Type>();
            LogHistoryContainer = new LogHistoryContainer(RowLocation.Top, 0);
            WindowFrame = WindowFrame.None;
            RedrawTimeSpan = TimeSpan.FromMilliseconds(100);
            DataContext = new ConsoleDataContext();
            HelpScreen = new DefaultHelpScreen();
            ColorPalette = new ColorPalette();
        }

        /// <summary>
        /// Set a handler to call when quit is requested
        /// </summary>
        /// <param name="action"></param>
        public void SetQuitHandler(Action<ExtendedConsole> action)
        {
            QuitHandler = action;
        }

        /// <summary>
        /// Set the data context to use for sending data to custom components
        /// </summary>
        /// <param name="context"></param>
        public void SetDataContext(ConsoleDataContext context)
        {
            DataContext = context;
        }

        /// <summary>
        /// Set the interval in which the UI will issue redraw frames. Default: 100ms
        /// </summary>
        /// <param name="timeSpan"></param>
        public void SetUpdateInterval(TimeSpan timeSpan)
        {
            RedrawTimeSpan = timeSpan;
        }

        /// <summary>
        /// Set the max number of history lines to keep (buffer lines). Default: 10,000
        /// </summary>
        /// <param name="timeSpan"></param>
        public void SetMaxHistoryLines(int maxHistoryLines)
        {
            MaxHistoryLines = maxHistoryLines;
        }

        /// <summary>
        /// Set a custom color palette to reference by name
        /// </summary>
        /// <param name="colorPalette"></param>
        public void SetColorPalette(ColorPalette colorPalette)
        {
            ColorPalette = colorPalette;
        }

        /// <summary>
        /// Set a window frame
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public void SetWindowFrame(Color color, int size)
        {
            WindowFrame = new WindowFrame(color, size);
        }

        /// <summary>
        /// Enable the default help screen
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public void SetHelpScreen(Color foregroundColor, Color backgroundColor)
        {
            HelpScreen = new DefaultHelpScreen(foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Enable the default help screen
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public void SetHelpScreen(Enum foregroundColor, Enum backgroundColor)
        {
            HelpScreen = new DefaultHelpScreen(foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Set the help screen to use
        /// </summary>
        /// <param name="helpScreen"></param>
        public void SetHelpScreen(IHelpScreen helpScreen)
        {
            HelpScreen = helpScreen;
        }

        /// <summary>
        /// Set the string prepended to each log history entry
        /// </summary>
        /// <param name="prependString"></param>
        public void SetLogPrependString(string prependString)
        {
            Prepend = prependString;
        }

        /// <summary>
        /// Register a custom component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="component"></param>
        public void RegisterComponent(string name, Type component)
        {
            if (!component.GetInterfaces().Contains(typeof(IComponent)))
                throw new ArgumentException(nameof(component), $"Type must implement IComponent");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            CustomComponents.Add(name, component);
        }

        /// <summary>
        /// Register a custom component
        /// </summary>
        /// <typeparam name="TComponent">Type implementing IComponent</typeparam>
        /// <param name="name"></param>
        public void RegisterComponent<TComponent>(string name)
            where TComponent : IComponent
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            CustomComponents.Add(name, typeof(TComponent));
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Color foregroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Enum foregroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.ForegroundColorPalette = foregroundColor;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Color foregroundColor, Color backgroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.ForegroundColor = foregroundColor;
            LogHistoryContainer.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Color prependColor, Color foregroundColor, Color backgroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.PrependColor = prependColor;
            LogHistoryContainer.ForegroundColor = foregroundColor;
            LogHistoryContainer.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Enum foregroundColor, Enum backgroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.ForegroundColorPalette = foregroundColor;
            LogHistoryContainer.BackgroundColorPalette = backgroundColor;
        }

        /// <summary>
        /// Set the location of where screen output will be buffered to
        /// </summary>
        /// <param name="location"></param>
        /// <param name="index"></param>
        public void SetLogHistoryContainer(RowLocation location, int index, Enum prependColor, Enum foregroundColor, Enum backgroundColor)
        {
            LogHistoryContainer.Location = location;
            LogHistoryContainer.Index = index;
            LogHistoryContainer.PrependColorPalette = prependColor;
            LogHistoryContainer.ForegroundColorPalette = foregroundColor;
            LogHistoryContainer.BackgroundColorPalette = backgroundColor;
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        public void SetStaticRow(string name, RowLocation location)
        {
            SetStaticRow(name, location, 0, default(Color?), default(Color?));
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="foregroundColor">Foreground color to use</param>
        public void SetStaticRow(string name, RowLocation location, Color? foregroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, null);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="foregroundColor">Foreground color to use</param>
        public void SetStaticRow(string name, RowLocation location, Enum foregroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, null);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, Color? foregroundColor, Color? backgroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, Enum foregroundColor, Enum backgroundColor)
        {
            SetStaticRow(name, location, 0, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="index">The 0-based row index to offset from the location</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, int index, Color? foregroundColor, Color? backgroundColor)
        {
            StaticRows.Add(new StaticRowConfig { Name = name, Location = location, Index = index, ForegroundColor = foregroundColor, BackgroundColor = backgroundColor });
        }

        /// <summary>
        /// Set a static console row
        /// </summary>
        /// <param name="name">The name for the row</param>
        /// <param name="location">The location to snap to</param>
        /// <param name="index">The 0-based row index to offset from the location</param>
        /// <param name="foregroundColor">The row's default foreground color</param>
        /// <param name="backgroundColor">The row's default background color</param>
        public void SetStaticRow(string name, RowLocation location, int index, Enum foregroundColor, Enum backgroundColor)
        {
            StaticRows.Add(new StaticRowConfig { Name = name, Location = location, Index = index, ForegroundColorPalette = foregroundColor, BackgroundColorPalette = backgroundColor });
        }
    }
}
