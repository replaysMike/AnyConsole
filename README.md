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

-coming shortly-

## Usage

```csharp
var console = new ExtendedConsole();
console.Configure(config =>
{
    config.SetStaticRow("Header", RowLocation.Top, Color.White, Color.DarkRed);
    config.SetStaticRow("SubHeader", RowLocation.Top, 1, Color.White, Color.FromArgb(30, 30, 30));
    config.SetStaticRow("Footer", RowLocation.Bottom, Color.White, Color.DarkBlue);
    config.SetLogHistoryContainer(RowLocation.Top, 2);
});
console.WriteRow("Header", "My Game Server", ColumnLocation.Left, Color.Yellow); // show text on the left
console.WriteRow("Header", Component.Time, ColumnLocation.Right); // show the time on the right
console.WriteRow("SubHeader", "This is a test application console", ColumnLocation.Left, Color.FromArgb(60, 60, 60));
console.Start();
console.WaitForClose();
```

## Features
- Static headers display
- Text alignment
- Keyboard/mouse event handling (windows only)
- Overrides stdout for static display of scrollable content
- Full color support thanks to Colorful integration!
- Component rendering (built-in rendered components, and custom components of your own)
- Text output formatting of logs
- Multithreaded

## Components

Components allow you to easily add UI elements to your console application, that self update and render. Common usage would be a DateTime component that always displays the current time.

### Built-in components
- DateTime
- Memory usage
- Drive usage
- Drive activity
- IP Address
- Network Bandwidth

### Custom components

Custom components need to simply implement IComponent and it will be rendered to the display. More details to come...
