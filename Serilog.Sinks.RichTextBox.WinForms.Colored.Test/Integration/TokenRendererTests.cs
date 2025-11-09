using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class TokenRendererTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void ExceptionTokenRenderer_RendersExceptionWithStackFrames()
        {
            Exception? exception = null;
            try
            {
                throw new InvalidOperationException("Test exception");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Error,
                exception,
                _parser.Parse("Error occurred"),
                Array.Empty<LogEventProperty>());

            var renderer = new ExceptionTokenRenderer(_defaultTheme);
            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("InvalidOperationException", result);
            Assert.Contains("Test exception", result);
            
            var lines = result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines.Length > 1, $"Exception should have multiple lines including stack trace. Got: {lines.Length} lines. Text: {result}");
        }

        [Fact]
        public void ExceptionTokenRenderer_RendersMultipleLines()
        {
            Exception? exception = null;
            try
            {
                throw new InvalidOperationException("Test exception");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Error,
                exception,
                _parser.Parse("Error occurred"),
                Array.Empty<LogEventProperty>());

            var renderer = new ExceptionTokenRenderer(_defaultTheme);
            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            var lines = result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines.Length > 1, $"Exception should have multiple lines. Got: {lines.Length} lines. Text: {result}");
        }

        [Fact]
        public void ExceptionTokenRenderer_HandlesNullException()
        {
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                _parser.Parse("No exception"),
                Array.Empty<LogEventProperty>());

            var renderer = new ExceptionTokenRenderer(_defaultTheme);
            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Empty(result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersNonStringScalarValue()
        {
            var template = _parser.Parse("Number: {Number}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Number");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Number", new ScalarValue(42)) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("42", result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersBooleanValue()
        {
            var template = _parser.Parse("IsValid: {IsValid}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "IsValid");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("IsValid", new ScalarValue(true)) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("True", result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersStructureValue()
        {
            var template = _parser.Parse("User: {User}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "User");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var structureValue = new StructureValue(new[]
            {
                new LogEventProperty("Id", new ScalarValue(123)),
                new LogEventProperty("Name", new ScalarValue("John"))
            }, "User");

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("User", structureValue) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("Id", result);
            Assert.Contains("123", result);
            Assert.Contains("Name", result);
            Assert.Contains("John", result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersSequenceValue()
        {
            var template = _parser.Parse("Items: {Items}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Items");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var sequenceValue = new SequenceValue(new[]
            {
                new ScalarValue(1),
                new ScalarValue(2),
                new ScalarValue(3)
            });

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Items", sequenceValue) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("1", result);
            Assert.Contains("2", result);
            Assert.Contains("3", result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersDictionaryValue()
        {
            var template = _parser.Parse("Config: {Config}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Config");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var dict = new Dictionary<ScalarValue, LogEventPropertyValue>
            {
                { new ScalarValue("key1"), new ScalarValue("value1") },
                { new ScalarValue("key2"), new ScalarValue(42) }
            };
            var dictionaryValue = new DictionaryValue(dict);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Config", dictionaryValue) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("key1", result);
            Assert.Contains("value1", result);
            Assert.Contains("key2", result);
            Assert.Contains("42", result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_HandlesMissingProperty()
        {
            var template = _parser.Parse("Missing: {Missing}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Missing");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                Array.Empty<LogEventProperty>());

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Empty(result);
        }

        [Fact]
        public void EventPropertyTokenRenderer_RendersStringValue()
        {
            var template = _parser.Parse("Message: {Message}");
            var propertyToken = template.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Message");
            var renderer = new EventPropertyTokenRenderer(_defaultTheme, propertyToken, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Message", new ScalarValue("Hello World")) });

            renderer.Render(logEvent, _canvas);

            var result = _richTextBox.Text;
            Assert.Contains("Hello World", result);
        }
    }
}

