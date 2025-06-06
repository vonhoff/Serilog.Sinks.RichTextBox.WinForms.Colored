using Serilog.Events;
using Xunit;

namespace Serilog.Tests
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
    }
}