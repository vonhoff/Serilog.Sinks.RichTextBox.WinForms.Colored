using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class JsonFormattingTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void JsonFormatting_BehavesAsExpected()
        {
            var stringProp = new LogEventProperty("StringProp", new ScalarValue("hello"));
            var complexProp = new LogEventProperty("ComplexProp", new StructureValue(new[] { new LogEventProperty("Id", new ScalarValue(123)) }, "MyObj"));

            var logEventSimple = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Value: {StringProp}"), new[] { stringProp });
            Assert.Equal("Value: \"hello\"", RenderAndGetText(logEventSimple, "{Message:j}"));

            var logEventComplex = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Value: {ComplexProp}"), new[] { complexProp });
            Assert.Equal("Value: {\"Id\": 123, \"$type\": \"MyObj\"}", RenderAndGetText(logEventComplex, "{Message:j}"));

            var logEventComplexPropJson = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Default message, JSON prop: {ComplexProp:j}"), new[] { complexProp });
            Assert.Equal("Default message, JSON prop: {\"Id\": 123, \"$type\": \"MyObj\"}", RenderAndGetText(logEventComplexPropJson, "{Message}"));

            var logEventComplexPropJsonLiteralMessage = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Literal message, JSON prop: {ComplexProp:j}"), new[] { complexProp });
            Assert.Equal("Literal message, JSON prop: {\"Id\": 123, \"$type\": \"MyObj\"}", RenderAndGetText(logEventComplexPropJsonLiteralMessage, "{Message:l}"));
        }

        [Fact]
        public void DictionaryValue_FormatsAsExpected()
        {
            var dict = new Dictionary<ScalarValue, LogEventPropertyValue>
            {
                { new ScalarValue("a"), new ScalarValue(1) },
                { new ScalarValue("b"), new ScalarValue("hello") },
                { new ScalarValue("c"), new ScalarValue(null) }
            };
            var dictProp = new LogEventProperty("DictProp", new DictionaryValue(dict));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DictProp:j}"), new[] { dictProp });
            Assert.Equal("{\"a\": 1, \"b\": \"hello\", \"c\": null}", RenderAndGetText(logEvent, "{Message:l}"));
        }

        [Fact]
        public void SpecialDoubleValues_FormatAsStrings()
        {
            var nanProp = new LogEventProperty("NanProp", new ScalarValue(double.NaN));
            var nanEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{NanProp:j}"), new[] { nanProp });
            Assert.Equal("\"NaN\"", RenderAndGetText(nanEvent, "{Message:j}"));
        }

        [Fact]
        public void CharValue_FormatsAsString()
        {
            var charProp = new LogEventProperty("CharProp", new ScalarValue('c'));
            var charEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{CharProp:j}"), new[] { charProp });
            Assert.Equal("\"c\"", RenderAndGetText(charEvent, "{Message:j}"));
        }

        [Fact]
        public void StringValues_AreProperlyEscaped()
        {
            var str = "a\"b\\c\n\r\f\t";
            var strProp = new LogEventProperty("StrProp", new ScalarValue(str));
            var strEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{StrProp:j}"), new[] { strProp });
            Assert.Equal("\"a\\\"b\\\\c\\n\\r\\f\\t\"", RenderAndGetText(strEvent, "{Message:j}"));
        }

        [Fact]
        public void SpecialFloatValues_FormatAsStrings()
        {
            var nanProp = new LogEventProperty("NanProp", new ScalarValue(float.NaN));
            var nanEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{NanProp:j}"), new[] { nanProp });
            Assert.Equal("\"NaN\"", RenderAndGetText(nanEvent, "{Message:j}"));
        }

        [Fact]
        public void ControlCharacters_AreEscapedAsUnicode()
        {
            var controlChars = new[]
            {
                '\u0000', // null
                '\u0001', // start of heading
                '\u0007', // bell
                '\u0008', // backspace
            };

            foreach (var c in controlChars)
            {
                var str = $"test{c}string";
                var strProp = new LogEventProperty("StrProp", new ScalarValue(str));
                var strEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{StrProp:j}"), new[] { strProp });

                var expected = $"\"test\\u{((int)c).ToString("X4")}string\"";
                Assert.Equal(expected, RenderAndGetText(strEvent, "{Message:j}"));
            }
        }

        [Fact]
        public void PrettyPrintJson_FormatsNestedObjectsWithIndentation()
        {
            var nestedProp = new LogEventProperty("Nested", new StructureValue(new[]
            {
                new LogEventProperty("Id", new ScalarValue(123)),
                new LogEventProperty("Name", new ScalarValue("test"))
            }, "MyObj"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Nested:j}"), new[] { nestedProp });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "{\n    \"Id\": 123,\n    \"Name\": \"test\",\n    \"$type\": \"MyObj\"\n}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PrettyPrintJson_FormatsArraysWithIndentation()
        {
            var arrayProp = new LogEventProperty("Array", new SequenceValue(new[]
            {
                new ScalarValue(1),
                new ScalarValue(2),
                new ScalarValue(3)
            }));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Array:j}"), new[] { arrayProp });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "[\n    1,\n    2,\n    3\n]";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PrettyPrintJson_FormatsDictionariesWithIndentation()
        {
            var dict = new Dictionary<ScalarValue, LogEventPropertyValue>
            {
                { new ScalarValue("a"), new ScalarValue(1) },
                { new ScalarValue("b"), new ScalarValue("hello") }
            };
            var dictProp = new LogEventProperty("DictProp", new DictionaryValue(dict));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DictProp:j}"), new[] { dictProp });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "{\n    \"a\": 1,\n    \"b\": \"hello\"\n}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PrettyPrintJson_FormatsNestedStructures()
        {
            var inner = new StructureValue(new[]
            {
                new LogEventProperty("Value", new ScalarValue(42))
            }, "Inner");
            var outer = new LogEventProperty("Outer", new StructureValue(new[]
            {
                new LogEventProperty("Inner", inner),
                new LogEventProperty("Name", new ScalarValue("test"))
            }, "Outer"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Outer:j}"), new[] { outer });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "{\n    \"Inner\": {\n        \"Value\": 42,\n        \"$type\": \"Inner\"\n    },\n    \"Name\": \"test\",\n    \"$type\": \"Outer\"\n}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PrettyPrintJson_EmptyCollectionsFormatCorrectly()
        {
            var emptyArray = new LogEventProperty("EmptyArray", new SequenceValue(Array.Empty<LogEventPropertyValue>()));
            var emptyDict = new LogEventProperty("EmptyDict", new DictionaryValue(new Dictionary<ScalarValue, LogEventPropertyValue>()));
            var emptyObject = new LogEventProperty("EmptyObject", new StructureValue(Array.Empty<LogEventProperty>(), null));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Array: {EmptyArray:j}, Dict: {EmptyDict:j}, Object: {EmptyObject:j}"), new[] { emptyArray, emptyDict, emptyObject });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            Assert.Contains("Array: []", result);
            Assert.Contains("Dict: {}", result);
            Assert.Contains("Object: {}", result);
        }

        [Fact]
        public void PrettyPrintJson_UsesTabsWhenConfigured()
        {
            var prop = new LogEventProperty("Test", new StructureValue(new[]
            {
                new LogEventProperty("Id", new ScalarValue(123))
            }, "MyObj"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Test:j}"), new[] { prop });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "{\n\t\t\t\t\"Id\": 123,\n\t\t\t\t\"$type\": \"MyObj\"\n}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PrettyPrintJson_RespectsSpacesPerIndent()
        {
            var prop = new LogEventProperty("Test", new StructureValue(new[]
            {
                new LogEventProperty("Id", new ScalarValue(123))
            }, "MyObj"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Test:j}"), new[] { prop });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 2);

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            var expected = "{\n  \"Id\": 123,\n  \"$type\": \"MyObj\"\n}";
            Assert.Equal(expected, result);
        }


        [Fact]
        public void CompactJson_StillWorksByDefault()
        {
            var complexProp = new LogEventProperty("ComplexProp", new StructureValue(new[] { new LogEventProperty("Id", new ScalarValue(123)) }, "MyObj"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("Value: {ComplexProp}"), new[] { complexProp });
            Assert.Equal("Value: {\"Id\": 123, \"$type\": \"MyObj\"}", RenderAndGetText(logEvent, "{Message:j}"));
        }

        [Fact]
        public void ScalarValue_IFormattableButNotNumericValueType()
        {
            var enumValue = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            var prop = new LogEventProperty("EnumProp", new ScalarValue(enumValue));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{EnumProp:j}"), new[] { prop });

            var result = RenderAndGetText(logEvent, "{Message:j}");
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ScalarValue_NonFormattableObject()
        {
            var plainObject = new object();
            var prop = new LogEventProperty("ObjectProp", new ScalarValue(plainObject));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{ObjectProp:j}"), new[] { prop });

            var result = RenderAndGetText(logEvent, "{Message:j}");
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.StartsWith("\"", result);
            Assert.EndsWith("\"", result);
        }

        [Fact]
        public void PrettyPrintJson_DictionaryWithNullKey()
        {
            var dict = new Dictionary<ScalarValue, LogEventPropertyValue>
            {
                { new ScalarValue(null), new ScalarValue("value") }
            };
            var dictProp = new LogEventProperty("DictProp", new DictionaryValue(dict));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DictProp:j}"), new[] { dictProp });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            Assert.Contains("\"null\"", result);
            Assert.Contains("\"value\"", result);
        }

        [Fact]
        public void PrettyPrintJson_DictionaryWithNonStringKey()
        {
            var dict = new Dictionary<ScalarValue, LogEventPropertyValue>
            {
                { new ScalarValue(123), new ScalarValue("value") }
            };
            var dictProp = new LogEventProperty("DictProp", new DictionaryValue(dict));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DictProp:j}"), new[] { dictProp });

            var options = new RichTextBoxSinkOptions(
                theme: _defaultTheme,
                prettyPrintJson: true,
                spacesPerIndent: 4,

            var result = RenderAndGetText(logEvent, "{Message:l}", options);
            Assert.Contains("\"123\"", result);
            Assert.Contains("\"value\"", result);
        }
    }
}