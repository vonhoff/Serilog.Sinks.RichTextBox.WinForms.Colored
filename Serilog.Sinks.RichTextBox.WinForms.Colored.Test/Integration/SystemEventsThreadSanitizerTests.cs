using Serilog.Sinks.RichTextBoxForms.Common;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class SystemEventsThreadSanitizerTests
    {
        private static int UiThreadId => Thread.CurrentThread.ManagedThreadId;

        [Fact]
        public void EnsureApplied_IsIdempotent()
        {
            // Act / Assert – multiple calls should not throw.
            SystemEventsThreadSanitizer.EnsureApplied(UiThreadId);
            SystemEventsThreadSanitizer.EnsureApplied(UiThreadId);
        }
    }
}