using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Serilog.Sinks.RichTextBoxForms.Common
{
    /// <summary>
    /// Removes <see cref="Microsoft.Win32.SystemEvents.UserPreferenceChanged"/> subscriptions that were registered
    /// from worker threads, preventing the classic WinForms "SystemEvents freeze". The implementation is the renamed
    /// successor of the original <c>SystemEventsFix</c> helper.
    /// </summary>
    public static class SystemEventsThreadSanitizer
    {
        private static int _uiThreadId = -1;
        private static bool _isApplied;
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Ensures off-thread subscriptions are detached. The first call must provide the primary UI-thread id so
        /// that its legitimate handlers remain intact. Subsequent calls are no-ops.
        /// </summary>
        public static void EnsureApplied(int uiThreadId)
        {
            if (_uiThreadId == -1)
            {
                _uiThreadId = uiThreadId;
            }

            if (_isApplied)
                return;

            lock (_syncRoot)
            {
                if (_isApplied) return;
                try
                {
                    RemoveOffThreadHandlers();
                }
                catch
                {
                    // Best-effort – swallow to keep logging functional if framework internals change.
                }

                _isApplied = true;
            }
        }

        private static void RemoveOffThreadHandlers()
        {
            var seType = typeof(SystemEvents);
            var handlersField = seType.GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Static)
                               ?? seType.GetField("s_handlers", BindingFlags.NonPublic | BindingFlags.Static);
            if (handlersField == null) return;

            var handlers = handlersField.GetValue(null);
            if (handlers == null) return;

            var valuesProp = handlers.GetType().GetProperty("Values");
            if (valuesProp == null) return;

            if (valuesProp.GetValue(handlers) is not IEnumerable values) return;

            foreach (var invokeInfos in values.Cast<IEnumerable>())
            {
                if (invokeInfos == null) continue;
                foreach (var invokeInfo in invokeInfos.Cast<object>().ToArray())
                {
                    var syncContextField = invokeInfo.GetType().GetField("_syncContext", BindingFlags.NonPublic | BindingFlags.Instance);
                    var delegateField = invokeInfo.GetType().GetField("_delegate", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (syncContextField == null || delegateField == null) continue;

                    if (syncContextField.GetValue(invokeInfo) is not WindowsFormsSynchronizationContext wfSync)
                        continue;

                    var destThreadRefField = typeof(WindowsFormsSynchronizationContext).GetField("destinationThreadRef", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (destThreadRefField == null) continue;
                    if (destThreadRefField.GetValue(wfSync) is not WeakReference threadRef || !threadRef.IsAlive)
                        continue;
                    if (threadRef.Target is not Thread thread) continue;

                    if (thread.ManagedThreadId == _uiThreadId) continue; // keep UI-thread handlers

                    if (delegateField.GetValue(invokeInfo) is not Delegate dlg) continue;

                    try
                    {
                        var handler = dlg as UserPreferenceChangedEventHandler ??
                                      (UserPreferenceChangedEventHandler)Delegate.CreateDelegate(typeof(UserPreferenceChangedEventHandler), dlg.Target, dlg.Method);
                        SystemEvents.UserPreferenceChanged -= handler;
                    }
                    catch
                    {
                        // swallow – signature mismatch etc.
                    }
                }
            }
        }
    }
} 