using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class PropertiesTokenRendererTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void Render_WithAdditionalProperties_FormatsCorrectly()
        {
            // Arrange
            var template = _parser.Parse("Message: {Message}");
            var outputTemplate = _parser.Parse("Message: {Message} {Properties}");
            var token = outputTemplate.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Properties");
            var renderer = new PropertiesTokenRenderer(_defaultTheme, token, outputTemplate, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[]
                {
                    new LogEventProperty("Message", new ScalarValue("test")),
                    new LogEventProperty("Additional", new ScalarValue("value"))
                });

            // Act
            renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Additional=\"value\"", text);
            Assert.DoesNotContain("Message=\"test\"", text);
        }

        [Fact]
        public void Render_WithJsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var template = _parser.Parse("Message: {Message}");
            var outputTemplate = _parser.Parse("Message: {Message} {Properties:j}");
            var token = outputTemplate.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Properties");
            var renderer = new PropertiesTokenRenderer(_defaultTheme, token, outputTemplate, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[]
                {
                    new LogEventProperty("Message", new ScalarValue("test")),
                    new LogEventProperty("Number", new ScalarValue(42)),
                    new LogEventProperty("Boolean", new ScalarValue(true))
                });

            // Act
            renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("\"Number\": 42", text);
            Assert.Contains("\"Boolean\": true", text);
            Assert.DoesNotContain("\"Message\": \"test\"", text);
        }

        [Fact]
        public void Render_WithNestedProperties_FormatsCorrectly()
        {
            // Arrange
            var template = _parser.Parse("Message: {Message}");
            var outputTemplate = _parser.Parse("Message: {Message} {Properties}");
            var token = outputTemplate.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Properties");
            var renderer = new PropertiesTokenRenderer(_defaultTheme, token, outputTemplate, null);

            var nestedStructure = new StructureValue(new[]
            {
                new LogEventProperty("NestedValue", new ScalarValue("nested"))
            });

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[]
                {
                    new LogEventProperty("Message", new ScalarValue("test")),
                    new LogEventProperty("Nested", nestedStructure)
                });

            // Act
            renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Nested={", text);
            Assert.Contains("NestedValue=\"nested\"", text);
        }

        [Fact]
        public void Render_WithNoAdditionalProperties_FormatsCorrectly()
        {
            // Arrange
            var template = _parser.Parse("Message: {Message}");
            var outputTemplate = _parser.Parse("Message: {Message} {Properties}");
            var token = outputTemplate.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Properties");
            var renderer = new PropertiesTokenRenderer(_defaultTheme, token, outputTemplate, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[]
                {
                    new LogEventProperty("Message", new ScalarValue("test"))
                });

            // Act
            renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Equal("{}", text);
        }

        [Fact]
        public void Render_WithPropertiesInOutputTemplate_ExcludesThem()
        {
            // Arrange
            var template = _parser.Parse("Message: {Message}");
            var outputTemplate = _parser.Parse("Message: {Message} {Properties} {Custom}");
            var token = outputTemplate.Tokens.OfType<PropertyToken>().Single(t => t.PropertyName == "Properties");
            var renderer = new PropertiesTokenRenderer(_defaultTheme, token, outputTemplate, null);

            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[]
                {
                    new LogEventProperty("Message", new ScalarValue("test")),
                    new LogEventProperty("Custom", new ScalarValue("value")),
                    new LogEventProperty("Additional", new ScalarValue("extra"))
                });

            // Act
            renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Additional=\"extra\"", text);
            Assert.DoesNotContain("Custom=\"value\"", text);
            Assert.DoesNotContain("Message=\"test\"", text);
        }
    }
}