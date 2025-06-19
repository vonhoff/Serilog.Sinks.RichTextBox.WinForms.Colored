using Serilog.Events;
using System.Globalization;
using Xunit;

namespace Serilog.Tests.Integration
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
                if (scalarValue is float v) expectedFormatted = v.ToString(CultureInfo.InvariantCulture);
                if (scalarValue is double v1) expectedFormatted = v1.ToString(CultureInfo.InvariantCulture);
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

        [Fact]
        public void DateTime_WithUSCulture_RendersCorrectly()
        {
            var dateTime = new DateTime(2025, 6, 6, 12, 9, 55, 421);
            var prop = new LogEventProperty("DateTime", new ScalarValue(dateTime));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DateTime}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message}", new CultureInfo("en-US"));
            Assert.Equal("6/6/2025 12:09:55 PM", actual);
        }

        [Fact]
        public void DateTime_WithGermanCulture_RendersCorrectly()
        {
            var dateTime = new DateTime(2025, 6, 6, 12, 9, 55, 421);
            var prop = new LogEventProperty("DateTime", new ScalarValue(dateTime));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DateTime}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message}", new CultureInfo("de-DE"));
            Assert.Equal("06.06.2025 12:09:55", actual);
        }

        [Fact]
        public void DateTimeOffset_WithUSCulture_RendersCorrectly()
        {
            var dateTimeOffset = new DateTimeOffset(2025, 6, 6, 12, 9, 55, 421, TimeSpan.FromHours(2));
            var prop = new LogEventProperty("DateTimeOffset", new ScalarValue(dateTimeOffset));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DateTimeOffset}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message}", new CultureInfo("en-US"));
            Assert.Equal("6/6/2025 12:09:55 PM +02:00", actual);
        }

        [Fact]
        public void DateTimeOffset_WithGermanCulture_RendersCorrectly()
        {
            var dateTimeOffset = new DateTimeOffset(2025, 6, 6, 12, 9, 55, 421, TimeSpan.FromHours(2));
            var prop = new LogEventProperty("DateTimeOffset", new ScalarValue(dateTimeOffset));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{DateTimeOffset}"), new[] { prop });

            string actual = RenderAndGetText(logEvent, "{Message}", new CultureInfo("de-DE"));
            Assert.Equal("06.06.2025 12:09:55 +02:00", actual);
        }
    }
}