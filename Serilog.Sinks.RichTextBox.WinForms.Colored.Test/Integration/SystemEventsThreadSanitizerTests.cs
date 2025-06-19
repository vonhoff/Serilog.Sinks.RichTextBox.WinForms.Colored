using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Serilog.Sinks.RichTextBoxForms.Common;
using Xunit;

namespace Serilog.Tests.Integration
{
    public class SystemEventsThreadSanitizerTests
    {
        private static int UiThreadId => Thread.CurrentThread.ManagedThreadId;

        [Fact]
        public void OffThreadSubscription_IsRemoved()
        {
            // Arrange – create RichTextBox on a separate STA thread to simulate sink buffer behaviour.
            var keeper = new ManualResetEventSlim(false);
            var workerReady = new ManualResetEventSlim(false);
            int workerThreadId = -1;

            Thread worker = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                workerThreadId = Thread.CurrentThread.ManagedThreadId;
                using var rtb = new RichTextBox();
                _ = rtb.Handle; // force handle to install WindowsFormsSynchronizationContext

                // Explicitly subscribe to ensure there is a handler linked to this worker thread.
                UserPreferenceChangedEventHandler dummyHandler = (_, __) => { /* no-op */ };
                SystemEvents.UserPreferenceChanged += dummyHandler;
                workerReady.Set();
                keeper.Wait(); // keep thread alive until test completes
                // Unsubscribe to avoid side-effects when test finishes.
                SystemEvents.UserPreferenceChanged -= dummyHandler;
            })
            {
                IsBackground = true
            };
            worker.SetApartmentState(ApartmentState.STA);
            worker.Start();

            // Wait until worker has created the control and handler.
            Assert.True(workerReady.Wait(TimeSpan.FromSeconds(5)), "Worker thread did not signal readiness.");

            // Sanity-check: ensure the off-thread subscription is present before fix.
            Assert.True(HasOffThreadSubscription(workerThreadId), "Expected off-thread subscription before sanitizer.");

            // Act – apply sanitizer.
            SystemEventsThreadSanitizer.EnsureApplied(UiThreadId);

            // Assert – subscription should be removed.
            Assert.False(HasOffThreadSubscription(workerThreadId), "Off-thread subscription should be removed by sanitizer.");

            // Cleanup
            keeper.Set();
            worker.Join();
        }

        [Fact]
        public void EnsureApplied_IsIdempotent()
        {
            // Act / Assert – multiple calls should not throw.
            SystemEventsThreadSanitizer.EnsureApplied(UiThreadId);
            SystemEventsThreadSanitizer.EnsureApplied(UiThreadId);
        }

        private static bool HasOffThreadSubscription(int threadId)
        {
            var seType = typeof(SystemEvents);
            var handlersField = seType.GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Static)
                               ?? seType.GetField("s_handlers", BindingFlags.NonPublic | BindingFlags.Static);
            if (handlersField == null) return false;
            var handlers = handlersField.GetValue(null);
            if (handlers == null) return false;

            var valuesProp = handlers.GetType().GetProperty("Values");
            if (valuesProp == null) return false;
            if (valuesProp.GetValue(handlers) is not System.Collections.IEnumerable values) return false;

            foreach (var invokeInfos in values.Cast<System.Collections.IEnumerable>())
            {
                foreach (var invokeInfo in invokeInfos.Cast<object>())
                {
                    var syncContextField = invokeInfo.GetType().GetField("_syncContext", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (syncContextField == null) continue;
                    if (syncContextField.GetValue(invokeInfo) is not WindowsFormsSynchronizationContext wfSync) continue;

                    var destThreadRefField = typeof(WindowsFormsSynchronizationContext).GetField("destinationThreadRef", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (destThreadRefField == null) continue;
                    if (destThreadRefField.GetValue(wfSync) is not WeakReference threadRef || !threadRef.IsAlive) continue;
                    if (threadRef.Target is not Thread thread) continue;

                    if (thread.ManagedThreadId == threadId)
                        return true;
                }
            }
            return false;
        }
    }
} 