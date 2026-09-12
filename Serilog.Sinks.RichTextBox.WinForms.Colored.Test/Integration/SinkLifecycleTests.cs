using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System.Windows.Forms;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class SinkLifecycleTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void Dispose_DrainsEventsEmittedBeforeDisposal()
        {
            _ = _richTextBox.Handle;

            _sink.Emit(CreateEvent("first"));
            _sink.Emit(CreateEvent("second"));
            _sink.Dispose();

            Application.DoEvents();

            Assert.Contains("first", _richTextBox.Text);
            Assert.Contains("second", _richTextBox.Text);
        }

        [Fact]
        public void Dispose_CancelsMessageProcessing()
        {
            var testRichTextBox = new RichTextBox();
            var testOptions = new RichTextBoxSinkOptions(
                theme: ThemePresets.Literate,
                autoScroll: true,
                maxLogLines: 100
            );

            var testSink = new RichTextBoxSink(testRichTextBox, testOptions);

            try
            {
                testSink.Dispose();

                // Give the background thread time to complete
                Thread.Sleep(100);
                Assert.Throws<ObjectDisposedException>(() => testSink.Emit(new LogEvent(
                    DateTimeOffset.Now,
                    LogEventLevel.Information,
                    null,
                    new MessageTemplate(new[] { new TextToken("Test") }),
                    Array.Empty<LogEventProperty>())));
            }
            finally
            {
                try
                {
                    testSink.Dispose();
                }
                catch
                {
                }
                testRichTextBox.Dispose();
            }
        }

        private static LogEvent CreateEvent(string message)
        {
            return new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                new MessageTemplate(new[] { new TextToken(message) }),
                Array.Empty<LogEventProperty>());
        }
    }
}
