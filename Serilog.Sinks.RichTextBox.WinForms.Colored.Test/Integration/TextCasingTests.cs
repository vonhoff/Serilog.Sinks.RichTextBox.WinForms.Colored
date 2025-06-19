using Serilog.Sinks.RichTextBoxForms.Common;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class TextCasingTests
    {
        [Theory]
        [InlineData("abc", "u", "ABC")]
        [InlineData("AbC", "w", "abc")]
        [InlineData("AbC", null, "AbC")]
        public void Format_ReturnsExpected(string input, string? format, string expected)
        {
            var actual = TextCasing.Format(input, format);
            Assert.Equal(expected, actual);
        }
    }
}