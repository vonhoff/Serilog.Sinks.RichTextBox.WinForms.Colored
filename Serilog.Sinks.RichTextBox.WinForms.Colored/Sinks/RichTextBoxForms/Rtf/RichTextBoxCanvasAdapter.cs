using System.Drawing;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    /// <summary>
    /// Thin wrapper so existing WinForms <see cref="RichTextBox"/> can be passed where an <see cref="IRtfCanvas"/> is expected.
    /// </summary>
    internal sealed class RichTextBoxCanvasAdapter : IRtfCanvas
    {
        private readonly RichTextBox _rtb;

        public RichTextBoxCanvasAdapter(RichTextBox rtb)
        {
            _rtb = rtb;
        }

        public int TextLength => _rtb.TextLength;

        public int SelectionStart { get => _rtb.SelectionStart; set => _rtb.SelectionStart = value; }
        public int SelectionLength { get => _rtb.SelectionLength; set => _rtb.SelectionLength = value; }
        public Color SelectionColor { get => _rtb.SelectionColor; set => _rtb.SelectionColor = value; }
        public Color SelectionBackColor { get => _rtb.SelectionBackColor; set => _rtb.SelectionBackColor = value; }
        public void AppendText(string text) => _rtb.AppendText(text);
    }
}