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
- WCAG compliant color schemes based on the [Serilog WPF RichTextBox](https://github.com/serilog-contrib/serilog-sinks-richtextbox) sink.

## Getting Started

Install the package from NuGet:

```powershell
Install-Package Serilog.Sinks.RichTextBox.WinForms.Colored
```

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

Configure the logger to use the sink:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.RichTextBox(richTextBox1, theme: ThemePresets.Literate)
    .CreateLogger();

Log.Information("Hello, world!");
```

## Themes

Available built-in themes:

| Theme                       | Description                                                                  |
|-----------------------------|------------------------------------------------------------------------------|
| `ThemePresets.Literate`     | Styled to replicate the default theme of Serilog.Sinks.Console __(default)__ |
| `ThemePresets.Grayscale`    | A theme using only shades of gray, white, and black                          |
| `ThemePresets.Colored`      | A theme based on the original Serilog.Sinks.ColoredConsole sink              |
| `ThemePresets.Luminous`     | A light theme with high contrast for accessibility                           |

The themes based on the original sinks are slightly adjusted to be [WCAG compliant](https://www.w3.org/WAI/WCAG22/Understanding/contrast-minimum), ensuring that the contrast ratio between text and background colors is at least 4.5:1.

You can create your own custom themes by creating a new instance of the [Theme](Serilog.Sinks.RichTextBox.WinForms.Colored/Sinks/RichTextBoxForms/Themes/Theme.cs) class and passing it to the `RichTextBox` extension method. Look at the [existing themes](Serilog.Sinks.RichTextBox.WinForms.Colored/Sinks/RichTextBoxForms/Themes/ThemePresets.cs) for examples.

## Frequently Asked Questions

### Why is the package name so long?

Shorter alternatives were already reserved in the NuGet registry, so a more descriptive name was needed for this implementation.

### Why use a WinForms RichTextBox instead of a WPF RichTextBox?

This sink is specifically designed for WinForms applications to avoid the WPF framework. Using a WPF-based logging component would require adding the entire WPF framework with all its dependencies, greatly increasing the size of the application.

## Support and Contribute

If you find value in this project, there are several ways you can contribute:

- Give the [project](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored) a star on GitHub ⭐
- Support the project through [GitHub Sponsors](https://github.com/sponsors/vonhoff)
- Improve documentation, report bugs, or submit pull requests

## License

Copyright © 2025 Simon Vonhoff & Contributors - Provided under the [Apache License, Version 2.0](LICENSE).
