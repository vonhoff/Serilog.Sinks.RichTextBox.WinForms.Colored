using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Themes;
using Serilog.Sinks.RichTextBoxForms.Rtf;

namespace Serilog.Tests.Integration
{
    public abstract class RichTextBoxSinkTestBase : IDisposable
    {
        protected readonly RichTextBox _richTextBox;
        protected readonly RichTextBoxSink _sink;
        protected readonly Theme _defaultTheme;
        protected readonly TemplateRenderer _renderer;
        protected readonly MessageTemplateParser _parser;
        protected readonly RichTextBoxCanvasAdapter _canvas;
        protected bool _disposed;

        protected RichTextBoxSinkTestBase()
        {
            _richTextBox = new RichTextBox();
            _defaultTheme = ThemePresets.Literate;
            _renderer = new TemplateRenderer(_defaultTheme, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine}{Exception}", null);
            _parser = new MessageTemplateParser();

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                autoScroll: true,
                maxLogLines: 1000
            );

            _sink = new RichTextBoxSink(_richTextBox, options);

            // Wrap the RichTextBox in an IRtfCanvas adapter so that unit tests
            // can interact with the updated rendering API without extensive
            // rewrites.
            _canvas = new RichTextBoxCanvasAdapter(_richTextBox);
        }

        protected string RenderAndGetText(LogEvent logEvent, string outputTemplate, IFormatProvider? formatProvider = null)
        {
            _richTextBox.Clear();
            var renderer = new TemplateRenderer(_defaultTheme, outputTemplate, formatProvider);
            renderer.Render(logEvent, _canvas);
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