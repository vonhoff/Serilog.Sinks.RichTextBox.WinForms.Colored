using Serilog.Events;
using Xunit;

namespace Serilog.Tests
{
    public class BasicFormattingTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void Constructor_InitializesRichTextBoxCorrectly()
        {
            Assert.True(_richTextBox.ReadOnly);
            Assert.False(_richTextBox.DetectUrls);
            Assert.Equal(_defaultTheme.DefaultStyle.Foreground, _richTextBox.ForeColor);
            Assert.Equal(_defaultTheme.DefaultStyle.Background, _richTextBox.BackColor);
        }

        [Fact]
        public void Emit_WithScalarValues_FormatsCorrectly()
        {
            // Arrange
            var template = _parser.Parse("String value: {String}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("String", new ScalarValue("test")) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("String value: test", text);
        }

        [Fact]
        public void Emit_WithDictionaryValue_FormatsCorrectly()
        {
            // Arrange
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1",
                ["key2"] = 42
            };

            var dictValue = new DictionaryValue(
                dict.Select(kv => new KeyValuePair<ScalarValue, LogEventPropertyValue>(
                    new ScalarValue(kv.Key),
                    new ScalarValue(kv.Value)
                ))
            );

            var template = _parser.Parse("Dictionary: {@Dict}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Dict", dictValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Dictionary: {", text);
            Assert.Contains("[key1]=value1", text);
            Assert.Contains("[key2]=42", text);
        }

        [Fact]
        public void Emit_WithSequenceValue_FormatsCorrectly()
        {
            // Arrange
            var array = new object[] { 1, 2, 3, "test" };
            var sequenceValue = new SequenceValue(array.Select(x => new ScalarValue(x)));

            var template = _parser.Parse("Array: {@Array}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Array", sequenceValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Array: [", text);
            Assert.Contains("1, 2, 3, test", text);
        }

        [Fact]
        public void Emit_WithStructureValue_FormatsCorrectly()
        {
            // Arrange
            var structureValue = new StructureValue(new[]
            {
                new LogEventProperty("Name", new ScalarValue("Test")),
                new LogEventProperty("Value", new ScalarValue(42)),
                new LogEventProperty("IsActive", new ScalarValue(true))
            });

            var template = _parser.Parse("Object: {Object}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Object", structureValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Object: {", text);
            Assert.Contains("Name=Test", text);
            Assert.Contains("Value=42", text);
            Assert.Contains("IsActive=True", text);
        }

        [Fact]
        public void Emit_WithComplexNestedValue_FormatsCorrectly()
        {
            // Arrange
            var complex = new StructureValue(new[]
            {
                new LogEventProperty("Name", new ScalarValue("Test")),
                new LogEventProperty("Numbers", new SequenceValue(new[] { 1, 2, 3 }.Select(x => new ScalarValue(x)))),
                new LogEventProperty("Settings", new StructureValue(new[]
                {
                    new LogEventProperty("enabled", new ScalarValue(true)),
                    new LogEventProperty("count", new ScalarValue(5))
                }))
            });

            var template = _parser.Parse("Complex: {Complex}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Complex", complex) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Complex: {", text);
            Assert.Contains("Name=Test", text);
            Assert.Contains("Numbers=[1, 2, 3]", text);
            Assert.Contains("Settings={", text);
            Assert.Contains("enabled=True", text);
            Assert.Contains("count=5", text);
        }

        [Fact]
        public void Emit_WithSequenceValue_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var array = new object[] { 1, 2, 3, "test" };
            var sequenceValue = new SequenceValue(array.Select(x => new ScalarValue(x)));

            var template = _parser.Parse("Array: {@Array:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Array", sequenceValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Array: [", text);
            Assert.Contains("1, 2, 3, \"test\"", text);
        }

        [Fact]
        public void Emit_WithNestedSequenceValue_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var nestedArray = new object[] { new object[] { 1, 2 }, new object[] { 3, 4 } };
            var sequenceValue = new SequenceValue(nestedArray.Select(x =>
                new SequenceValue(((object[])x).Select(y => new ScalarValue(y)))));

            var template = _parser.Parse("NestedArray: {@NestedArray:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("NestedArray", sequenceValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("NestedArray: [", text);
            Assert.Contains("[[1, 2], [3, 4]]", text);
        }

        [Fact]
        public void Emit_WithEmptySequenceValue_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var emptyArray = new object[] { };
            var sequenceValue = new SequenceValue(emptyArray.Select(x => new ScalarValue(x)));

            var template = _parser.Parse("EmptyArray: {@EmptyArray:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("EmptyArray", sequenceValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("EmptyArray: []", text);
        }

        [Fact]
        public void Emit_WithStructureValue_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var structureValue = new StructureValue(new[]
            {
                new LogEventProperty("Name", new ScalarValue("Test")),
                new LogEventProperty("Value", new ScalarValue(42)),
                new LogEventProperty("IsActive", new ScalarValue(true))
            });

            var template = _parser.Parse("Object: {@Object:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Object", structureValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Object: {", text);
            Assert.Contains("\"Name\": \"Test\"", text);
            Assert.Contains("\"Value\": 42", text);
            Assert.Contains("\"IsActive\": true", text);
        }

        [Fact]
        public void Emit_WithStructureValueWithTypeTag_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var structureValue = new StructureValue(
                new[]
                {
                    new LogEventProperty("Name", new ScalarValue("Test")),
                    new LogEventProperty("Value", new ScalarValue(42))
                },
                "TestType");

            var template = _parser.Parse("Object: {@Object:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Object", structureValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Object: {", text);
            Assert.Contains("\"Name\": \"Test\"", text);
            Assert.Contains("\"Value\": 42", text);
            Assert.Contains("\"$type\": \"TestType\"", text);
        }

        [Fact]
        public void Emit_WithEmptyStructureValue_JsonFormatting_FormatsCorrectly()
        {
            // Arrange
            var structureValue = new StructureValue(Array.Empty<LogEventProperty>());

            var template = _parser.Parse("Object: {@Object:j}");
            var logEvent = new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Information,
                null,
                template,
                new[] { new LogEventProperty("Object", structureValue) });

            // Act
            _renderer.Render(logEvent, _richTextBox);

            // Assert
            var text = _richTextBox.Text;
            Assert.Contains("Object: {}", text);
        }
    }
}