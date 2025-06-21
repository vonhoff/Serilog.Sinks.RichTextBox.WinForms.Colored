using Serilog.Events;
using System.Globalization;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class SpecialCaseFormattingTests : RichTextBoxSinkTestBase
    {
        [Theory]
        [InlineData("Bytes", new byte[] { 1, 2, 3, 4 }, "{Bytes}", "{Message}", "\"AQIDBA==\"")]
        [InlineData("Time", "2023-01-15T10:30:45.0000000Z", "{Time:O}", "{Message}", "2023-01-15T10:30:45.0000000Z")]
        [InlineData("TimeOffset", "2023-01-15T10:30:45.0000000+02:00", "{TimeOffset:O}", "{Message}", "2023-01-15T10:30:45.0000000+02:00")]
        [InlineData("Duration", "03:25:45", "{Duration}", "{Message}", "03:25:45")]
        [InlineData("Id", "00000000-0000-0000-0000-000000000000", "{Id}", "{Message}", "00000000-0000-0000-0000-000000000000")]
        [InlineData("Link", "http://example.com/path", "{Link}", "{Message}", "http://example.com/path")]
        public void ScalarTypes_SpecialCases_DefaultFormatting_RendersCorrectly(string propertyName, object value, string template, string outputTemplate, string expected)
        {
            // Arrange
            var scalarValue = value switch
            {
                byte[] bytes => new ScalarValue(bytes),
                string str when propertyName == "Time" => new ScalarValue(DateTime.Parse(str, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)),
                string str when propertyName == "TimeOffset" => new ScalarValue(DateTimeOffset.Parse(str, CultureInfo.InvariantCulture)),
                string str when propertyName == "Duration" => new ScalarValue(TimeSpan.Parse(str)),
                string str when propertyName == "Id" => new ScalarValue(Guid.Parse(str)),
                string str when propertyName == "Link" => new ScalarValue(new Uri(str)),
                _ => throw new ArgumentException($"Unsupported value type for {propertyName}")
            };

            var prop = new LogEventProperty(propertyName, scalarValue);
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse(template), new[] { prop });

            // Act & Assert
            Assert.Equal(expected, RenderAndGetText(logEvent, outputTemplate, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_ByteArray_RendersCorrectly()
        {
            // Arrange
            var bytes = new byte[] { 1, 2, 3, 4 };
            var byteProp = new LogEventProperty("Bytes", new ScalarValue(bytes));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Bytes}"), new[] { byteProp });

            // Act & Assert
            Assert.Equal($"\"{Convert.ToBase64String(bytes)}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_DateTime_RendersCorrectly()
        {
            // Arrange
            var dt = new DateTime(2023, 1, 15, 10, 30, 45, DateTimeKind.Utc);
            var dtProp = new LogEventProperty("Time", new ScalarValue(dt));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Time}"), new[] { dtProp });

            // Act & Assert
            Assert.Equal($"\"{dt.ToString("O")}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_DateTimeOffset_RendersCorrectly()
        {
            // Arrange
            var dto = new DateTimeOffset(2023, 1, 15, 10, 30, 45, TimeSpan.FromHours(2));
            var dtoProp = new LogEventProperty("TimeOffset", new ScalarValue(dto));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{TimeOffset}"), new[] { dtoProp });

            // Act & Assert
            Assert.Equal($"\"{dto.ToString("O")}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_TimeSpan_RendersCorrectly()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(12345);
            var tsProp = new LogEventProperty("Duration", new ScalarValue(ts));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Duration}"), new[] { tsProp });

            // Act & Assert
            Assert.Equal($"\"{ts.ToString()}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_Guid_RendersCorrectly()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var guidProp = new LogEventProperty("Id", new ScalarValue(guid));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Id}"), new[] { guidProp });

            // Act & Assert
            Assert.Equal($"\"{guid.ToString("D")}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_Uri_RendersCorrectly()
        {
            // Arrange
            var uri = new Uri("http://test.com/path");
            var uriProp = new LogEventProperty("Link", new ScalarValue(uri));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Link}"), new[] { uriProp });

            // Act & Assert
            Assert.Equal($"\"{uri.ToString()}\"", RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void JsonFormatting_Decimal_RendersCorrectly()
        {
            // Arrange
            var decVal = 10.33m;
            var decProp = new LogEventProperty("Scalar", new ScalarValue(decVal));
            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Scalar}"), new[] { decProp });

            // Act & Assert
            Assert.Equal(decVal.ToString(CultureInfo.InvariantCulture), RenderAndGetText(logEvent, "{Message:j}", CultureInfo.InvariantCulture));
        }
    }
}