using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Themes;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class SinkLifecycleTests : RichTextBoxSinkTestBase
    {
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
                // Act & Assert
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
    }
}