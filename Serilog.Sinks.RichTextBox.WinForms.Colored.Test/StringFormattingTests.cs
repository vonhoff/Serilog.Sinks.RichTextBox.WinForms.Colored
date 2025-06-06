using Serilog.Events;
using Xunit;

namespace Serilog.Tests
{
    public class StringFormattingTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void StringQuoting_BehavesAsExpected()
        {
            var prop = new LogEventProperty("StringProp", new ScalarValue("hello world"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Test: {StringProp}"), new[] { prop });

            // Default message template: {Message} -> property should be quoted
            Assert.Equal("Test: \"hello world\"", RenderAndGetText(logEvent, "{Message}"));

            // Message template with :l -> property should NOT be quoted
            Assert.Equal("Test: hello world", RenderAndGetText(logEvent, "{Message:l}"));

            // Property template with :l -> property should NOT be quoted
            Assert.Equal("Test: hello world", RenderAndGetText(logEvent, "Test: {StringProp:l}"));

            // Message template with :l, property template default -> property should NOT be quoted (inherited)
            var logEventForInheritedLiteral = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{StringProp}"), new[] { prop });
            Assert.Equal("hello world", RenderAndGetText(logEventForInheritedLiteral, "{Message:l}"));
        }

        [Fact]
        public void StringificationOperator_BehavesAsExpected()
        {
            var array = new[] { 1, 2, 3 };
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Received {$Data}"), new[] { new LogEventProperty("Data", new ScalarValue(array.ToString())) });

            // Default: {$Data} should be quoted string of type name
            Assert.Equal("Received \"System.Int32[]\"", RenderAndGetText(logEvent, "{Message}"));

            // With :l on message: {$Data} should be unquoted string of type name
            Assert.Equal("Received System.Int32[]", RenderAndGetText(logEvent, "{Message:l}"));

            // With :j on message: {$Data} should be JSON quoted string of type name
            Assert.Equal("Received \"System.Int32[]\"", RenderAndGetText(logEvent, "{Message:j}"));

            // With :l on property: {$Data:l} should be unquoted string of type name
            var logEventPropLiteral = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Received {$Data:l}"), new[] { new LogEventProperty("Data", new ScalarValue(array.ToString())) });
            Assert.Equal("Received System.Int32[]", RenderAndGetText(logEventPropLiteral, "{Message}"));

            // With :j on property: {$Data:j} should be JSON quoted string of type name
            var logEventPropJson = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Received {$Data:j}"), new[] { new LogEventProperty("Data", new ScalarValue(array.ToString())) });
            Assert.Equal("Received \"System.Int32[]\"", RenderAndGetText(logEventPropJson, "{Message}"));
        }
    }
}