using Serilog.Events;
using System.Globalization;
using Xunit;

namespace Serilog.Tests
{
    public class SpecialCaseFormattingTests : RichTextBoxSinkTestBase
    {
        [Fact]
        public void ScalarTypes_SpecialCases_DefaultFormatting_RendersCorrectly()
        {
            // byte[] (Base64 encoded string, quoted)
            var byteProp = new LogEventProperty("Bytes", new ScalarValue(new byte[] { 1, 2, 3, 4 }));
            var logEventBytes = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Bytes}"), new[] { byteProp });
            Assert.Equal($"\"{Convert.ToBase64String(new byte[] { 1, 2, 3, 4 })}\"", RenderAndGetText(logEventBytes, "{Message}", CultureInfo.InvariantCulture));

            // DateTime (ISO 8601 like, not quoted for display by default)
            var dt = new DateTime(2023, 1, 15, 10, 30, 45, DateTimeKind.Utc);
            var dtProp = new LogEventProperty("Time", new ScalarValue(dt));
            var logEventDt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Time}"), new[] { dtProp });
            Assert.Equal(dt.ToString("O"), RenderAndGetText(logEventDt, "{Message}", CultureInfo.InvariantCulture));

            // DateTimeOffset (ISO 8601 like, not quoted for display by default)
            var dto = new DateTimeOffset(2023, 1, 15, 10, 30, 45, TimeSpan.FromHours(2));
            var dtoProp = new LogEventProperty("TimeOffset", new ScalarValue(dto));
            var logEventDto = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{TimeOffset}"), new[] { dtoProp });
            Assert.Equal(dto.ToString("O"), RenderAndGetText(logEventDto, "{Message}", CultureInfo.InvariantCulture));

            // TimeSpan (c format, not quoted for display by default)
            var ts = TimeSpan.FromSeconds(12345);
            var tsProp = new LogEventProperty("Duration", new ScalarValue(ts));
            var logEventTs = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Duration}"), new[] { tsProp });
            Assert.Equal(ts.ToString("c"), RenderAndGetText(logEventTs, "{Message}", CultureInfo.InvariantCulture));

            // Guid (D format, not quoted for display by default)
            var guid = Guid.NewGuid();
            var guidProp = new LogEventProperty("Id", new ScalarValue(guid));
            var logEventGuid = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Id}"), new[] { guidProp });
            Assert.Equal(guid.ToString("D"), RenderAndGetText(logEventGuid, "{Message}", CultureInfo.InvariantCulture));

            // Uri (Original string, not quoted for display by default)
            var uri = new Uri("http://example.com/path");
            var uriProp = new LogEventProperty("Link", new ScalarValue(uri));
            var logEventUri = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Link}"), new[] { uriProp });
            Assert.Equal(uri.ToString(), RenderAndGetText(logEventUri, "{Message}", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void ScalarTypes_SpecialCases_JsonFormatting_RendersCorrectly()
        {
            // byte[] (Base64 encoded string, JSON quoted)
            var bytes = new byte[] { 1, 2, 3, 4 };
            var byteProp = new LogEventProperty("Bytes", new ScalarValue(bytes));
            var logEventBytes = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Bytes}"), new[] { byteProp });
            Assert.Equal($"\"{Convert.ToBase64String(bytes)}\"", RenderAndGetText(logEventBytes, "{Message:j}", CultureInfo.InvariantCulture));

            // DateTime (ISO 8601 "O" format, JSON quoted)
            var dt = new DateTime(2023, 1, 15, 10, 30, 45, DateTimeKind.Utc);
            var dtProp = new LogEventProperty("Time", new ScalarValue(dt));
            var logEventDt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Time}"), new[] { dtProp });
            Assert.Equal($"\"{dt.ToString("O")}\"", RenderAndGetText(logEventDt, "{Message:j}", CultureInfo.InvariantCulture));

            // DateTimeOffset (ISO 8601 "O" format, JSON quoted)
            var dto = new DateTimeOffset(2023, 1, 15, 10, 30, 45, TimeSpan.FromHours(2));
            var dtoProp = new LogEventProperty("TimeOffset", new ScalarValue(dto));
            var logEventDto = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{TimeOffset}"), new[] { dtoProp });
            Assert.Equal($"\"{dto.ToString("O")}\"", RenderAndGetText(logEventDto, "{Message:j}", CultureInfo.InvariantCulture));

            // TimeSpan (ToString() format, JSON quoted)
            var ts = TimeSpan.FromSeconds(12345);
            var tsProp = new LogEventProperty("Duration", new ScalarValue(ts));
            var logEventTs = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Duration}"), new[] { tsProp });
            Assert.Equal($"\"{ts.ToString()}\"", RenderAndGetText(logEventTs, "{Message:j}", CultureInfo.InvariantCulture));

            // Guid (D format, JSON quoted)
            var guid = Guid.NewGuid();
            var guidProp = new LogEventProperty("Id", new ScalarValue(guid));
            var logEventGuid = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Id}"), new[] { guidProp });
            Assert.Equal($"\"{guid.ToString("D")}\"", RenderAndGetText(logEventGuid, "{Message:j}", CultureInfo.InvariantCulture));

            // Uri (ToString(), JSON quoted)
            var uri = new Uri("http://test.com/path");
            var uriProp = new LogEventProperty("Link", new ScalarValue(uri));
            var logEventUri = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Link}"), new[] { uriProp });
            Assert.Equal($"\"{uri.ToString()}\"", RenderAndGetText(logEventUri, "{Message:j}", CultureInfo.InvariantCulture));

            // Decimal for JSON (rendered as number)
            var decVal = 10.33m;
            var decProp = new LogEventProperty("Scalar", new ScalarValue(decVal));
            var logEventDec = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, _parser.Parse("{Scalar}"), new[] { decProp });
            Assert.Equal(decVal.ToString(CultureInfo.InvariantCulture), RenderAndGetText(logEventDec, "{Message:j}", CultureInfo.InvariantCulture));
        }
    }
}