using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class ClearFunctionalityTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void Clear_HidesExistingLogEntries_ButShowsNewOnes()
        {
            var template1 = _parser.Parse("First message");
            var logEvent1 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template1,
                Array.Empty<LogEventProperty>());

            var template2 = _parser.Parse("Second message");
            var logEvent2 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template2,
                Array.Empty<LogEventProperty>());

            _sink.Emit(logEvent1);
            _sink.Emit(logEvent2);
            Thread.Sleep(200);

            var initialText = _richTextBox.Text;
            Assert.Contains("First message", initialText);
            Assert.Contains("Second message", initialText);

            _sink.Clear();
            Thread.Sleep(200);

            var clearedText = _richTextBox.Text;
            Assert.DoesNotContain("First message", clearedText);
            Assert.DoesNotContain("Second message", clearedText);

            var template3 = _parser.Parse("Third message after clear");
            var logEvent3 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template3,
                Array.Empty<LogEventProperty>());

            _sink.Emit(logEvent3);
            Thread.Sleep(200);

            var afterClearText = _richTextBox.Text;
            Assert.DoesNotContain("First message", afterClearText);
            Assert.DoesNotContain("Second message", afterClearText);
            Assert.Contains("Third message after clear", afterClearText);
        }

        [Fact]
        public void Restore_RestoresAllLogEntries_AfterClear()
        {
            var template1 = _parser.Parse("First message");
            var logEvent1 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template1,
                Array.Empty<LogEventProperty>());

            var template2 = _parser.Parse("Second message");
            var logEvent2 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template2,
                Array.Empty<LogEventProperty>());

            _sink.Emit(logEvent1);
            _sink.Emit(logEvent2);
            Thread.Sleep(200);

            _sink.Clear();
            Thread.Sleep(200);

            var template3 = _parser.Parse("Third message after clear");
            var logEvent3 = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template3,
                Array.Empty<LogEventProperty>());

            _sink.Emit(logEvent3);
            Thread.Sleep(200);

            _sink.Restore();
            Thread.Sleep(200);

            var restoredText = _richTextBox.Text;
            Assert.Contains("First message", restoredText);
            Assert.Contains("Second message", restoredText);
            Assert.Contains("Third message after clear", restoredText);
        }

        [Fact]
        public void Clear_WithoutAnyMessages_DoesNotThrow()
        {
            _sink.Clear();
            Thread.Sleep(200);

            Assert.True(string.IsNullOrWhiteSpace(_richTextBox.Text));
        }

        [Fact]
        public void Restore_WithoutPreviousClear_DoesNotThrow()
        {
            var template = _parser.Parse("Test message");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                Array.Empty<LogEventProperty>());

            _sink.Emit(logEvent);
            Thread.Sleep(200);

            _sink.Restore();
            Thread.Sleep(200);

            var text = _richTextBox.Text;
            Assert.Contains("Test message", text);
        }
    }
} 