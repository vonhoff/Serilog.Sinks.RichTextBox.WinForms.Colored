using System.Drawing;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    /// <summary>
    /// Lightweight surface that renderers can draw rich-text fragments on. Implemented by <see cref="RtfBuilder"/> and adapted for <see cref="System.Windows.Forms.RichTextBox"/> at runtime.
    /// </summary>
    public interface IRtfCanvas
    {
        int TextLength { get; }

        int SelectionStart { get; set; }
        int SelectionLength { get; set; }

        Color SelectionColor { get; set; }
        Color SelectionBackColor { get; set; }

        void AppendText(string text);
    }
}