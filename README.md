# Serilog.Sinks.RichTextBox.WinForms.Colored

[![NuGet Downloads](https://img.shields.io/nuget/dt/Serilog.Sinks.RichTextBox.WinForms.Colored.svg)](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)
[![Latest version](https://img.shields.io/nuget/v/Serilog.Sinks.RichTextBox.WinForms.Colored.svg)](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A [Serilog](https://github.com/serilog/serilog) sink that writes log events to
a [WinForms RichTextBox](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/richtextbox-control-overview-windows-forms)
with support for coloring and custom themes.

![Screenshot of Serilog.Sinks.RichTextBox.WinForms.Colored in action](https://raw.githubusercontent.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored/main/Assets/screenshot.png)

## Features

- Write log events to a WinForms RichTextBox control
- Customizable themes (Dark and Light presets available)
- Configurable output templates
- Auto-scrolling option
- Line limit control

## Installation

Install the package from NuGet:

```powershell
Install-Package Serilog.Sinks.RichTextBox.WinForms.Colored
```

## Usage

### Basic Setup

Declare your RichTextBox control:

```csharp
private System.Windows.Forms.RichTextBox richTextBox1;

private void InitializeComponent()
{
    this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
    this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
    this.richTextBox1.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
    this.richTextBox1.Location = new System.Drawing.Point(0, 0);
    this.richTextBox1.Name = "richTextBox1";
}
```

Configure the logger:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.RichTextBox(richTextBox1)
    .CreateLogger();

Log.Information("Hello, world!");
```

### Advanced Configuration

You can customize the sink using various parameters from the RichTextBox extension method:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.RichTextBox(
        richTextBoxControl: richTextBox1,
        minimumLogEventLevel: LogEventLevel.Debug,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        theme: ThemePresets.Light,
        messageBatchSize: 100,
        messagePendingInterval: 10,
        autoScroll: true,
        maxLogLines: 1000)
    .CreateLogger();

```

### Themes

Available built-in themes:

| Theme                | Description                                             |
|----------------------|---------------------------------------------------------|
| `ThemePresets.Dark`  | Similar to the default theme of _Serilog.Sinks.Console_ |
| `ThemePresets.Light` | Light background with contrasting colors                |

## Support and Contribute

If you find value in this project, there are several ways you can contribute:

- **Become a Sponsor:** Support the project through [GitHub Sponsors](https://github.com/sponsors/vonhoff).
- **Show Your Appreciation:** Give the [project](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored) a star on GitHub.
- **Contribute:** Improve documentation, report bugs, or submit pull requests.

## License

This project is licensed under the [Apache License, Version 2.0](LICENSE).
