## Release Notes - Serilog.Sinks.RichTextBox.WinForms.Colored v3.1.0

### Feature Release

This release introduces the ability to clear and restore the log view in a non-destructive way.

### What Changed

- Added `Clear()` and `Restore()` methods to the sink, allowing you to hide all current log entries from the view without deleting them from memory. New log events after clearing are still shown. You can restore the view to show all buffered events at any time (unless overwritten by new logs).
- You can now use `.WriteTo.RichTextBox(richTextBox, out sink, ...)` to capture the sink instance and call `Clear()`/`Restore()` directly.

**Note:** The buffer is still a fixed size (default up to 2048 lines). If you clear and then log enough new events to fill the buffer, the oldest (hidden) events will be overwritten and cannot be restored.

### Resources

- [GitHub Repository](https://github.com/vonhoff/Serilog.Sinks.RichTextBox.WinForms.Colored)
- [NuGet Package](https://www.nuget.org/packages/Serilog.Sinks.RichTextBox.WinForms.Colored)