using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Tests.Integration
{
    public abstract class RichTextBoxSinkTestBase : IDisposable
    {
        protected readonly RichTextBox _richTextBox;
        protected readonly RichTextBoxSink _sink;
        protected readonly Theme _defaultTheme;
        protected readonly TemplateRenderer _renderer;
        protected readonly MessageTemplateParser _parser;
        protected bool _disposed;

        protected RichTextBoxSinkTestBase()
        {
            _richTextBox = new RichTextBox();
            _defaultTheme = ThemePresets.Literate;
            _renderer = new TemplateRenderer(_defaultTheme, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine}{Exception}", null);
            _parser = new MessageTemplateParser();

            var options = new RichTextBoxSinkOptions(
                appliedTheme: _defaultTheme,
                autoScroll: true,
                maxLogLines: 1000
            );

            _sink = new RichTextBoxSink(_richTextBox, options);
        }

        protected string RenderAndGetText(LogEvent logEvent, string outputTemplate, IFormatProvider? formatProvider = null)
        {
            _richTextBox.Clear();
            var renderer = new TemplateRenderer(_defaultTheme, outputTemplate, formatProvider);
            renderer.Render(logEvent, _richTextBox);
            return _richTextBox.Text.TrimEnd('\n', '\r');
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (!_disposed)
            {
                _sink?.Dispose();
                _richTextBox?.Dispose();
                _disposed = true;
            }
        }
    }
}