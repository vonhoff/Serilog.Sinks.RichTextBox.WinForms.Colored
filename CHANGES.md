## Release Notes - Serilog.Sinks.RichTextBox.WinForms.Colored v3.0.0

### Major Release - Breaking Changes

This major release focuses on performance optimization, UI stability, and streamlined configuration while simplifying the configuration API.

### Breaking Changes

- **Simplified Configuration Options**: Reduced configurable options to focus only on the most relevant and commonly used settings:
  - Removed `messageBatchSize` parameter (no longer needed)
  - Removed `messagePendingInterval` parameter (no longer needed)
  - Renamed `appliedTheme` parameter to `theme` for consistency

- **Theme System Overhaul**: Complete redesign of the theme system to align with the Serilog WPF sink. Previous theme names (`Dark`, `Light`, `DarkClassic`, `LightClassic`) have been replaced with new theme presets. All themes now include WCAG compliance with proper contrast ratio.

- **Enhanced Memory Management**: The `maxLogLines` parameter now has improved validation (1-512 range) with a default of 256 lines to ensure optimal performance. While not mandatory, proper configuration is recommended to prevent performance degradation from excessive log entries in the WinForms control.

### New Theme System

Available built-in themes:

| Theme                       | Description                                                                  |
|-----------------------------|------------------------------------------------------------------------------|
| `ThemePresets.Literate`     | Styled to replicate the default theme of Serilog.Sinks.Console (default) |
| `ThemePresets.Grayscale`    | A theme using only shades of gray, white, and black                          |
| `ThemePresets.Colored`      | A theme based on the original Serilog.Sinks.ColoredConsole sink              |
| `ThemePresets.Luminous`     | A new light theme with high contrast for accessibility                           |

The themes based on the original sinks are slightly adjusted to be WCAG compliant, ensuring that the contrast ratio between text and background colors is at least 4.5:1. `Luminous` is a new theme specifically created for this sink.

### Bug Fixes

- **Fixed UI Freezing**: Resolved critical UI freeze issues caused by SystemEvents when using RichTextBox controls on background threads.

- **Fixed Auto-scroll on .NET Framework**: Corrected auto-scroll behavior that wasn't working properly on .NET Framework applications.

### Performance Improvements

- **Optimized Rendering Logic**: Removed the off-screen RichTextBox dependency and improved the rendering pipeline for better performance and reduced memory usage.

- **Streamlined Processing**: Removed unnecessary batching parameters to simplify internal processing logic.

### Migration Guide

Due to breaking changes, please update your existing configurations:

1. **Update Theme Names**: Replace old theme names with new equivalents:
   - `Dark` or `DarkClassic` → `ThemePresets.Colored` or `ThemePresets.Grayscale` or `ThemePresets.Literate`
   - `Light` or `LightClassic` → `ThemePresets.Luminous`

2. **Update Parameter Names**: 
   - `appliedTheme` → `theme`
   - Remove `messageBatchSize` and `messagePendingInterval` parameters (no longer supported)

I recommend pairing this sink with a file sink for persistent logging storage, as it's not practical to have thousands of log entries displayed in a RichTextBox control.

### Recommended Configuration

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.RichTextBox(richTextBox1, 
        theme: ThemePresets.Literate,
        maxLogLines: 64) // Optional, defaults to 256
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day) // Recommended for persistence
    .CreateLogger();
```

### Full Changelog

- Reduced configurable options to only the most relevant ones (breaking change)
- Renamed `appliedTheme` parameter to `theme` (breaking change)
- Removed `messageBatchSize` and `messagePendingInterval` parameters (breaking change)
- Completely redesigned theme system with new theme names and WCAG compliance (breaking change)
- Added new `Luminous` theme for high contrast accessibility
- Enhanced `maxLogLines` validation with 1-512 range limit
- Fixed UI freeze caused by SystemEvents on background-thread RichTextBox
- Optimized performance by removing the off-screen RichTextBox and improving rendering logic
- Fixed auto-scroll issue on .NET Framework

### Resources

- [GitHub Repository](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored)
- [NuGet Package](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)