# Serilog.Sinks.RichTextBox.WinForms.Colored

[![NuGet Downloads](https://img.shields.io/nuget/dt/Serilog.Sinks.RichTextBox.WinForms.Colored.svg)](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)
[![workflow](https://img.shields.io/github/actions/workflow/status/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored/build.yml)](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored/actions)
[![Latest version](https://img.shields.io/nuget/v/Serilog.Sinks.RichTextBox.WinForms.Colored.svg)](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A [Serilog](https://github.com/serilog/serilog) sink that writes log events to a [WinForms RichTextBox](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/richtextbox-control-overview-windows-forms) with support for coloring and custom themes.

![Screenshot of Serilog.Sinks.RichTextBox.WinForms.Colored in action](https://raw.githubusercontent.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored/master/screenshot.png)

## Features

- Colored log events in a WinForms RichTextBox control
- Multiple theme presets with customization options
- Auto-scrolling to latest messages
- Line limiting to control memory usage
- High-performance asynchronous processing
- WCAG compliant color schemes

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
        theme: ThemePresets.Literate,
        autoScroll: true,
        maxLogLines: 250,
        batchSize: 500,
        flushInterval: 16,
        queueCapacity: 1000,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        formatProvider: null,
        minimumLogEventLevel: LogEventLevel.Verbose)
    .CreateLogger();
```

### Themes

Available built-in themes:

| Theme                       | Description                                                                  |
|-----------------------------|------------------------------------------------------------------------------|
| `ThemePresets.Literate`     | A literate theme with syntax highlighting and structured data formatting (default). |
| `ThemePresets.Grayscale`    | A monochrome theme with high contrast for accessibility.                     |
| `ThemePresets.Colored`      | A colorful theme with vibrant colors for different log levels.              |

You can customize the themes by creating your own theme instance or modifying the existing ones. The themes support various style tokens for different parts of the log message, including:
- Text colors for different log levels
- Syntax highlighting for strings, numbers, and booleans
- Background colors for error and fatal messages
- Custom styling for null values and invalid content

### Configuration Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `richTextBoxControl` | RichTextBox | Required | The RichTextBox control to write logs to |
| `theme` | Theme | `ThemePresets.Literate` | The theme to apply for coloring |
| `autoScroll` | bool | `true` | Whether to automatically scroll to the latest message |
| `maxLogLines` | int | `1000` | Maximum number of log lines to keep in memory |
| `batchSize` | int | `250` | Number of log events to process in a batch |
| `flushInterval` | int | `16` | Interval in milliseconds to flush logs |
| `queueCapacity` | int | `10000` | Maximum number of log events in the processing queue |
| `outputTemplate` | string | `"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"` | The output template for formatting log messages |
| `formatProvider` | IFormatProvider | `null` | Culture-specific formatting provider |
| `minimumLogEventLevel` | LogEventLevel | `LogEventLevel.Verbose` | Minimum log level to process |

## Frequently Asked Questions

### Why is the package name so long?

Shorter alternatives were already reserved in the NuGet registry, so a more descriptive name was needed for this implementation.

### Why use a WinForms RichTextBox instead of a WPF RichTextBox?

This sink is designed for WinForms applications to avoid unnecessary dependencies. Using a WPF-based logging component would require adding the entire WPF framework, greatly increasing the size of the application for a small logging feature.

## Support and Contribute

If you find value in this project, there are several ways you can contribute:

- Give the [project](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored) a star on GitHub.
- Support the project through [GitHub Sponsors](https://github.com/sponsors/vonhoff).
- Improve documentation, report bugs, or submit pull requests.

## License

This project is licensed under the [Apache License, Version 2.0](LICENSE).
