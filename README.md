<div align="center">
  <img src="Assets/serilog-sink-nuget.png" alt="Serilog.Sinks.RichTextBox.WinForms" width="100" />
</div>

<h1 align="center">Serilog.Sinks.RichTextBox.WinForms.Colored</h1>
<div align="center">

A [Serilog](https://serilog.net) sink that writes log events to any WinForms [RichTextBox](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/richtextbox-control-overview-windows-forms) control with coloring and custom theme support. 

![Screenshot of Serilog.Sinks.RichTextBox.WinForms in action](Assets/screenshot.png)
This sink is based on the [WPF RichTextBox Sink](https://github.com/serilog-contrib/serilog-sinks-richtextbox) by C. Augusto Proiete.
</div>

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

## Getting started :rocket:

Declare your [RichTextBox](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/richtextbox-control-overview-windows-forms) control and give it a name that you can reference it from the code-behind. e.g.:

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

Then enable the sink using `WriteTo.RichTextBox()`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.RichTextBox(richTextBox1)
    .CreateLogger();

Log.Information("Hello, world!");
```

Log events will be written to the `RichTextBox` control:

```
[11:54:36 INF] Hello, world!
```

### Themes

Themes can be specified when configuring the sink:

```csharp
    .WriteTo.RichTextBox(MyRichTextBox, theme: RichTextBoxConsoleTheme.Dark)
```

The following built-in themes are available at this time:

| Theme                               | Description
| ----------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| `RichTextBoxConsoleTheme.Dark`      | Styled to replicate the default theme of  _Serilog.Sinks.Console_; **This is the default when no theme is specified** |
| `RichTextBoxConsoleTheme.Light`     | A theme with a light background and contrasting colors.                                                               |

### Output templates

The format of events to the RichTextBox can be modified using the `outputTemplate` configuration parameter:

```csharp
    .WriteTo.RichTextBox(MyRichTextBox,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
```

The default template, shown in the example above, uses built-in properties like `Timestamp` and `Level`. Properties from events, including those attached using [enrichers](https://github.com/serilog/serilog/wiki/Enrichment), can also appear in the output template.

---

_Copyright &copy; 2022 Simon Vonhoff & Contributors - Provided under the [Apache License, Version 2.0](LICENSE)._
