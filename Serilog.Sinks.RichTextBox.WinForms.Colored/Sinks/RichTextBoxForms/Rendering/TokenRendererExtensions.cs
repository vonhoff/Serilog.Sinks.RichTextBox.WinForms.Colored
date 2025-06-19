using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using System;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public static class TokenRendererExtensions
    {
        public static void Render(this ITokenRenderer renderer, LogEvent logEvent, RichTextBox richTextBox)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (richTextBox == null) throw new ArgumentNullException(nameof(richTextBox));

            var builder = new RtfBuilder(richTextBox.ForeColor, richTextBox.BackColor);
            renderer.Render(logEvent, builder);
            richTextBox.AppendRtf(builder.Rtf, autoScroll: true, maxLogLines: int.MaxValue);
        }
    }
}