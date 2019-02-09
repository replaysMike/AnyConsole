# AnyConsole
[![nuget](https://img.shields.io/nuget/v/AnyConsole.svg)](https://www.nuget.org/packages/AnyConsole/)
[![nuget](https://img.shields.io/nuget/dt/AnyConsole.svg)](https://www.nuget.org/packages/AnyConsole/)
[![Build status](https://ci.appveyor.com/api/projects/status/xr7gebcdins0hs4f?svg=true)](https://ci.appveyor.com/project/MichaelBrown/anyconsole)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/c04b064c4a2141a48cd148cfb08d57d6)](https://www.codacy.com/app/replaysMike/AnyConsole?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=replaysMike/AnyConsole&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://api.codacy.com/project/badge/Coverage/c04b064c4a2141a48cd148cfb08d57d6)](https://www.codacy.com/app/replaysMike/AnyConsole?utm_source=github.com&utm_medium=referral&utm_content=replaysMike/AnyConsole&utm_campaign=Badge_Coverage)

A CSharp library that gives you better console handling for that classic ASCII look.

## Description

AnyConsole was designed for server service windows, utility tooling and log output windows. It's very handy if you need static header/footers on your consoles while maintaining scrollable areas, adding visual components or more advanced input management such as mouse control. It integrates very well with TopShelf for services.

## Installation
Install AnyConsole from the Package Manager Console:
```
PM> Install-Package AnyConsole
```

## Visual Examples

Example from Test Runner
![AnyConsole](https://github.com/replaysMike/AnyConsole/wiki/screenshots/AnyConsole_example.png)
Can be configured to contain any number of static rows with components, static text and colors.

## Usage

```csharp
var console = new ExtendedConsole();
// create a data context. You can pass this to your own application and use it to pass data to custom components for display.
var myDataContext = new ConsoleDataContext();
console.Configure(config =>
{
    config.SetStaticRow("Header", RowLocation.Top, Color.White, Color.DarkRed);
    config.SetStaticRow("SubHeader", RowLocation.Top, 1, Color.White, Color.FromArgb(30, 30, 30));
    config.SetStaticRow("Footer", RowLocation.Bottom, Color.White, Color.DarkBlue);
    config.SetLogHistoryContainer(RowLocation.Top, 2);
    config.SetDataContext(myDataContext);
    config.SetUpdateInterval(TimeSpan.FromMilliseconds(100));
    config.SetMaxHistoryLines(1000);
    config.SetHelpScreen();
    config.SetQuitHandler((consoleInstance) => {
        // do something special when quit occurs
    });
});
console.OnKeyPress += Console_OnKeyPress;
console.WriteRow("Header", "Test Console", ColumnLocation.Left, Color.Yellow); // show text on the left
console.WriteRow("Header", Component.Time, ColumnLocation.Right); // show the time on the right
console.WriteRow("SubHeader", "This is a test application console", ColumnLocation.Left, Color.FromArgb(60, 60, 60));
console.Start();
console.WaitForClose();
```

## Features
- Static rows/headers/footers display
- Text alignment
- Keyboard/mouse event handling (windows only)
- Overrides stdout for static display of scrollable content
- Full color support thanks to [Colorful](http://colorfulconsole.com/) integration!
- ASCII [Figlet](http://www.figlet.org/) fonts
- Help screen support
- Component rendering (built-in rendered components, and custom components of your own)
- Text output formatting of logs through stdout hijacking
- Multithreaded

## RGB Color support

Custom color support allows for specifying up to 16 custom colors of your choice. No longer limited to Windows Console color definitions thanks to [Colorful](http://colorfulconsole.com/) integration.

You can specify `System.Drawing.Color` colors when writing data, or create a named color palette. Using a color palette is a nicer option as you can define styles and guarantee you won't go over your 16 color maximum. If you exceed 16 colors when using `System.Drawing.Color` color definitions a `ColorPaletteException` will be thrown.

### Color palette example

```csharp
// define your custom palette enum
public enum Style
{
    Foreground,
    Background,
    HeaderBackground,
    SubHeaderBackground,
    SubHeaderForeground,
    FooterBackground,
    LogHistoryBackground,
    Highlight
}

var console = new ExtendedConsole();
// create a data context. You can pass this to your own application and use it to pass data to custom components for display.
var myDataContext = new ConsoleDataContext();
console.Configure(config =>
{
    // use a custom color palette for drawing
    config.SetColorPalette(new Dictionary<Enum, Color>{
        { Style.Foreground, Color.White },
        { Style.Background, Color.Black },
        { Style.HeaderBackground, Color.DarkRed },
        { Style.SubHeaderBackground, Color.FromArgb(30, 30, 30) },
        { Style.SubHeaderForeground, Color.FromArgb(60, 60, 60) },
        { Style.FooterBackground, Color.DarkBlue },
        { Style.LogHistoryBackground, Color.FromArgb(100, 100, 100) },
        { Style.Highlight, Color.Yellow },
    });
    config.SetStaticRow("Header", RowLocation.Top, Style.Foreground, Style.HeaderBackground);
    config.SetLogHistoryContainer(RowLocation.Top, 2, Style.LogHistoryBackground);
    config.SetDataContext(myDataContext);
    config.SetUpdateInterval(TimeSpan.FromMilliseconds(100));
    config.SetMaxHistoryLines(1000);
    config.SetHelpScreen(Style.Foreground, Style.FooterBackground);
});
console.WriteRow("Header", "Test Console", ColumnLocation.Left, Style.Highlight);
console.WriteRow("Header", Component.DateTimeUtc, ColumnLocation.Right, componentParameter: "MMMM dd yyyy hh:mm tt");
console.Start();

// write some text to stdout
Console.WriteLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
Console.WriteLine("Pellentesque hendrerit dui sit amet ultricies iaculis.");

console.WaitForClose();
```

## Using [Figlet](http://www.figlet.org/) Fonts

AnyConsole supports using ASCII fonts for rendering. To use the default (built-in) Figlet font:

```csharp
console.WriteAscii("Moo");
/*
   __  ___        
  /  |/  /__  ___ 
 / /|_/ / _ \/ _ \
/_/  /_/\___/\___/
*/
```

To use a custom Figlet font:
```csharp
var font = FigletFont.Load("chunky.flf");
console.Write("Moo", font);
/*
 _______              
|   |   |.-----.-----.
|       ||  _  |  _  |
|__|_|__||_____|_____|
*/
```

## Built-in key handling

When enabled (by default) some basic operation is permitted. The following keys will be bound:

* [H] - display help
* [Home] - go to start of buffer log
* [End] / [Esc] - go to end of buffer log and resume scrolling
* [Q] - quit application
* [MouseScroll] - scroll the buffer log

## Components

Components allow you to easily add UI elements to your console application, that self update and render. Common usage would be a DateTime component that always displays the current time.

### Built-in components
- DateTime
- Memory usage
- Disk usage
- Cpu usage
- IP Address
- Log search, buffer information

### Log search component

When the LogSearch component is added to a row, the console will listen for the CTRL-S key to enable searching of the log. Enter some search text and press F3 or SHIFT-F3 to iterate through the results.

### Custom components

Custom components need to simply implement IComponent and it will be rendered to the display. Custom components can either do processing in a provided `Tick` handler or can implement it's own thread management. 

A custom component implements `IComponent` and optionally `IDisposable`. Components can receive data through the `ConsoleDataContext` provided when `Setup` is called.

### Basic example
This example shows how you might show the number of connected users to your application:

```csharp
public class ConnectedUsersComponent : IComponent
{
    private string _value;
    private ConsoleDataContext _dataContext;
    private MyApplicationServer _server;

    public bool HasUpdates { get; private set; }

    public bool HasCustomThreadManagement => false;

    public string Render(object parameters)
    {
        // called when HasUpdates=true
        try
        {
            return _value;
        }
        finally
        {
            HasUpdates = false;
        }
    }

    public void Setup(ConsoleDataContext dataContext, string name, IExtendedConsole console)
    {
        // perform any one-time setup needed
        // you can pass data to your component using the provided dataContext
        _dataContext = dataContext;
    }

    public void Tick(ulong tickCount)
    {
        // called when the console says you can update data
        if (_server == null)
        {
            // cache the game server context reference so we don't need to keep looking it up
            var server = _dataContext.GetData<MyApplicationServer>("MyApplicationServer");
            if (server != null)
                _server = server;
        }
        else
        {
            var newValue = $"Connected Users: {_server.ConnectedUsers}";
            if (!newValue.Equals(_value))
            {
                _value = newValue;
                HasUpdates = true;
            }
        }
    }
}
```

### Custom thread-management in a Custom Component

```csharp
public class RandomNumberComponent : IComponent, IDisposable
{
    private bool _isDisposed;
    private ManualResetEvent _isRunning = new ManualResetEvent(false);
    private Thread _updateThread;
    private bool _hasUpdates;
    private int _randomNumber;
    private string _name;
    private IExtendedConsole _console;
    private ConsoleDataContext _dataContext;

    public bool HasUpdates { get { return _hasUpdates; } }

    public bool HasCustomThreadManagement => true;

    public RandomNumberComponent()
    {
    }

    public string Render(object parameters)
    {
        try
        {
            return $"RND: {_randomNumber}";
        }
        finally
        {
            _hasUpdates = false;
        }
    }

    public void Setup(ConsoleDataContext dataContext, string name, IExtendedConsole console)
    {
        // perform any one-time setup needed
        // you can pass data to your component using the provided dataContext
        _dataContext = dataContext;
        // we don't need these but we will store them
        _name = name;
        _console = console;

        _isRunning.Reset();
        _updateThread = new Thread(new ThreadStart(UpdateThread));
        _updateThread.IsBackground = true;
        _updateThread.Priority = ThreadPriority.BelowNormal;
        _updateThread.Start();
    }

    private void UpdateThread()
    {
        _randomNumber = GenerateRandomNumber();
        while (!_isRunning.WaitOne(2000))
        {
            _randomNumber = GenerateRandomNumber();
            _hasUpdates = true;
        }
    }

    private int GenerateRandomNumber()
    {
        return new Random().Next(1, 1000);
    }

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_isDisposed)
            return;

        if (isDisposing)
        {
            _isRunning.Set();
            _updateThread?.Join(500);
            _updateThread = null;
            _isRunning?.Dispose();
            _isRunning = null;
        }
        _isDisposed = true;
    }

    public void Tick(ulong tickCount)
    {
        // not used as we do our own thread management for data updates
    }

    #endregion
}
```

Custom components simplify displaying custom data on the screen that may periodically update.
