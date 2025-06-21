using Serilog.Sinks.RichTextBoxForms.Formatting;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class TextCasingTests
    {
        [Theory]
        [InlineData("abc", "u", "ABC")]
        [InlineData("AbC", "w", "abc")]
        public void Format_ReturnsExpected(string input, string format, string expected)
        {
            var actual = TextFormatter.Format(input, format);
            Assert.Equal(expected, actual);
        }
    }
}