using Serilog.Sinks.RichTextBoxForms.Rtf;

namespace Serilog.Tests
{
    public sealed class RichTextBoxCanvasAdapter : IRtfCanvas
    {
        private readonly RichTextBox _richTextBox;

        public RichTextBoxCanvasAdapter(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        }

        public int TextLength => _richTextBox.TextLength;

        public int SelectionStart
        {
            get => _richTextBox.SelectionStart;
            set => _richTextBox.SelectionStart = value;
        }

        public int SelectionLength
        {
            get => _richTextBox.SelectionLength;
            set => _richTextBox.SelectionLength = value;
        }

        public Color SelectionColor
        {
            get => _richTextBox.SelectionColor;
            set => _richTextBox.SelectionColor = value;
        }

        public Color SelectionBackColor
        {
            get => _richTextBox.SelectionBackColor;
            set => _richTextBox.SelectionBackColor = value;
        }

        public void AppendText(string text) => _richTextBox.AppendText(text);
    }
}