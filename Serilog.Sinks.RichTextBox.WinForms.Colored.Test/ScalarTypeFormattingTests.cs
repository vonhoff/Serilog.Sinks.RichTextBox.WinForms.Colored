using Serilog.Events;
using System.Globalization;
using Xunit;

namespace Serilog.Tests
{
    public class ScalarTypeFormattingTests : RichTextBoxSinkTestBase
    {
        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        [InlineData((byte)1, "1")]
        [InlineData((short)2, "2")]
        [InlineData((ushort)3, "3")]
        [InlineData(4, "4")]
        [InlineData(5u, "5")]
        [InlineData(6L, "6")]
        [InlineData(7UL, "7")]
        [InlineData(8.1f, "8.1")]
        [InlineData(9.2d, "9.2")]
        [InlineData(10.33, "10.33")]
        [InlineData("test string", "\"test string\"")]
        [InlineData(null, "null")]
        public void ScalarTypes_DefaultFormatting_RendersCorrectly(object? scalarValue, string expectedString)
        {
            var prop = new LogEventProperty("Scalar", new ScalarValue(scalarValue));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Scalar}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message}", CultureInfo.InvariantCulture);

            if (scalarValue is float || scalarValue is double || scalarValue is decimal)
            {
                var expectedFormatted = Convert.ToDecimal(scalarValue).ToString(CultureInfo.InvariantCulture);
                if (scalarValue is float) expectedFormatted = ((float)scalarValue).ToString(CultureInfo.InvariantCulture);
                if (scalarValue is double) expectedFormatted = ((double)scalarValue).ToString(CultureInfo.InvariantCulture);
                Assert.Equal(expectedFormatted, actual);
            }
            else
            {
                Assert.Equal(expectedString, actual);
            }
        }

        [Theory]
        [InlineData(true, "true")]
        [InlineData(false, "false")]
        [InlineData((byte)1, "1")]
        [InlineData((short)2, "2")]
        [InlineData((ushort)3, "3")]
        [InlineData(4, "4")]
        [InlineData(5u, "5")]
        [InlineData(6L, "6")]
        [InlineData(7UL, "7")]
        [InlineData(8.1f, "8.1")]
        [InlineData(9.2d, "9.2")]
        [InlineData("test string", "\"test string\"")]
        [InlineData(null, "null")]
        public void ScalarTypes_JsonFormatting_RendersCorrectly(object? scalarValue, string expectedJsonFragment)
        {
            var prop = new LogEventProperty("Scalar", new ScalarValue(scalarValue));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Scalar}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture);

            if (scalarValue is decimal decVal)
            {
                Assert.Equal(decVal.ToString(CultureInfo.InvariantCulture), actual);
            }
            else if (scalarValue is float fVal)
            {
                Assert.Equal(fVal.ToString("R", CultureInfo.InvariantCulture), actual);
            }
            else if (scalarValue is double dVal)
            {
                Assert.Equal(dVal.ToString("R", CultureInfo.InvariantCulture), actual);
            }
            else
            {
                Assert.Equal(expectedJsonFragment, actual);
            }
        }

        [Fact]
        public void ScalarTypes_LiteralStringFormatting_RendersCorrectly()
        {
            var prop = new LogEventProperty("ScalarString", new ScalarValue("quoted string"));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{ScalarString:l}"), new[] { prop });

            // {Prop:l} should render string unquoted
            Assert.Equal("quoted string", RenderAndGetText(logEvent, "{Message}"));

            var logEventMsgLiteral = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{ScalarString}"), new[] { prop });
            // {Message:l} with {Prop} should render string unquoted
            Assert.Equal("quoted string", RenderAndGetText(logEventMsgLiteral, "{Message:l}"));
        }
    }
}