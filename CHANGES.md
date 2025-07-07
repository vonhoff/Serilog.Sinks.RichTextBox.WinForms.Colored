## Release Notes - Serilog.Sinks.RichTextBox.WinForms.Colored v3.1.2

### What Changed

- Fixed a bug where cross-thread log messages during form construction caused `System.InvalidOperationException` due to premature access of the RichTextBox.

### Contributors

- [Steffen S. Hellestøl](https://github.com/SteffenSH)

### Resources

- [GitHub Repository](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored)
- [NuGet Package](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)