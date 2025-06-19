using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public static class TokenRendererExtensions
    {
        public static void Render(this ITokenRenderer renderer, LogEvent logEvent, RichTextBox richTextBox)
        {
            var adapter = new RichTextBoxCanvasAdapter(richTextBox);
            renderer.Render(logEvent, adapter);
        }
    }
}