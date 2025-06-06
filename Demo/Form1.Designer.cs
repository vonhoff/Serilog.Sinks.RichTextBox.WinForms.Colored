namespace Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnClear = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            btnVerbose = new System.Windows.Forms.ToolStripButton();
            btnDebug = new System.Windows.Forms.ToolStripButton();
            btnInformation = new System.Windows.Forms.ToolStripButton();
            btnWarning = new System.Windows.Forms.ToolStripButton();
            btnError = new System.Windows.Forms.ToolStripButton();
            btnFatal = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btnParallelFor = new System.Windows.Forms.ToolStripButton();
            btnTaskRun = new System.Windows.Forms.ToolStripButton();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            btnObject = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            btnScalar = new System.Windows.Forms.ToolStripButton();
            btnDictionary = new System.Windows.Forms.ToolStripButton();
            btnStructure = new System.Windows.Forms.ToolStripButton();
            btnComplex = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            btnDispose = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            btnReset = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            btnAutoScroll = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            btnLogLimit = new System.Windows.Forms.ToolStripButton();
            panel1 = new System.Windows.Forms.Panel();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            toolStrip1.SuspendLayout();
            toolStrip2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnClear, toolStripSeparator3, btnVerbose, btnDebug, btnInformation, btnWarning, btnError, btnFatal, toolStripSeparator1, btnParallelFor, btnTaskRun });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            toolStrip1.Size = new System.Drawing.Size(1208, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnClear
            // 
            btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(38, 22);
            btnClear.Text = "Clear";
            btnClear.ToolTipText = "Clear the log output";
            btnClear.Click += btnClear_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnVerbose
            // 
            btnVerbose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnVerbose.Name = "btnVerbose";
            btnVerbose.Size = new System.Drawing.Size(52, 22);
            btnVerbose.Text = "Verbose";
            btnVerbose.ToolTipText = "Log a verbose message";
            btnVerbose.Click += btnVerbose_Click;
            // 
            // btnDebug
            // 
            btnDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDebug.Name = "btnDebug";
            btnDebug.Size = new System.Drawing.Size(46, 22);
            btnDebug.Text = "Debug";
            btnDebug.ToolTipText = "Log a debug message";
            btnDebug.Click += btnDebug_Click;
            // 
            // btnInformation
            // 
            btnInformation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnInformation.Name = "btnInformation";
            btnInformation.Size = new System.Drawing.Size(74, 22);
            btnInformation.Text = "Information";
            btnInformation.ToolTipText = "Log an information message";
            btnInformation.Click += btnInformation_Click;
            // 
            // btnWarning
            // 
            btnWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnWarning.Name = "btnWarning";
            btnWarning.Size = new System.Drawing.Size(56, 22);
            btnWarning.Text = "Warning";
            btnWarning.ToolTipText = "Log a warning message";
            btnWarning.Click += btnWarning_Click;
            // 
            // btnError
            // 
            btnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnError.Name = "btnError";
            btnError.Size = new System.Drawing.Size(36, 22);
            btnError.Text = "Error";
            btnError.ToolTipText = "Log an error message";
            btnError.Click += btnError_Click;
            // 
            // btnFatal
            // 
            btnFatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnFatal.Name = "btnFatal";
            btnFatal.Size = new System.Drawing.Size(36, 22);
            btnFatal.Text = "Fatal";
            btnFatal.ToolTipText = "Log a fatal message";
            btnFatal.Click += btnFatal_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnParallelFor
            // 
            btnParallelFor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnParallelFor.Name = "btnParallelFor";
            btnParallelFor.Size = new System.Drawing.Size(106, 22);
            btnParallelFor.Text = "Parallel.For(100*6)";
            btnParallelFor.ToolTipText = "Demonstrate parallel logging with 600 messages";
            btnParallelFor.Click += btnParallelFor_Click;
            // 
            // btnTaskRun
            // 
            btnTaskRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnTaskRun.Name = "btnTaskRun";
            btnTaskRun.Size = new System.Drawing.Size(95, 22);
            btnTaskRun.Text = "Task.Run(100*6)";
            btnTaskRun.ToolTipText = "Demonstrate async logging with 600 messages";
            btnTaskRun.Click += btnTaskRun_Click;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnObject, toolStripSeparator4, btnScalar, btnDictionary, btnStructure, btnComplex, toolStripSeparator8, btnDispose, toolStripSeparator5, btnReset, toolStripSeparator6, btnAutoScroll, toolStripSeparator7, btnLogLimit });
            toolStrip2.Location = new System.Drawing.Point(0, 25);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            toolStrip2.Size = new System.Drawing.Size(1208, 25);
            toolStrip2.TabIndex = 3;
            toolStrip2.Text = "toolStrip2";
            // 
            // btnObject
            // 
            btnObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnObject.Name = "btnObject";
            btnObject.Size = new System.Drawing.Size(39, 22);
            btnObject.Text = "JSON";
            btnObject.ToolTipText = "Log a JSON object";
            btnObject.Click += btnObject_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnScalar
            // 
            btnScalar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnScalar.Name = "btnScalar";
            btnScalar.Size = new System.Drawing.Size(42, 22);
            btnScalar.Text = "Scalar";
            btnScalar.ToolTipText = "Log scalar values";
            btnScalar.Click += btnScalar_Click;
            // 
            // btnDictionary
            // 
            btnDictionary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDictionary.Name = "btnDictionary";
            btnDictionary.Size = new System.Drawing.Size(65, 22);
            btnDictionary.Text = "Dictionary";
            btnDictionary.ToolTipText = "Log a dictionary object";
            btnDictionary.Click += btnDictionary_Click;
            // 
            // btnStructure
            // 
            btnStructure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnStructure.Name = "btnStructure";
            btnStructure.Size = new System.Drawing.Size(59, 22);
            btnStructure.Text = "Structure";
            btnStructure.ToolTipText = "Log a structured object";
            btnStructure.Click += btnStructure_Click;
            // 
            // btnComplex
            // 
            btnComplex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnComplex.Name = "btnComplex";
            btnComplex.Size = new System.Drawing.Size(58, 22);
            btnComplex.Text = "Complex";
            btnComplex.ToolTipText = "Log a complex nested object";
            btnComplex.Click += btnComplex_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDispose
            // 
            btnDispose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDispose.Name = "btnDispose";
            btnDispose.Size = new System.Drawing.Size(52, 22);
            btnDispose.Text = "Dispose";
            btnDispose.ToolTipText = "Dispose the logger";
            btnDispose.Click += btnDispose_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReset
            // 
            btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(39, 22);
            btnReset.Text = "Reset";
            btnReset.ToolTipText = "Reset the logger";
            btnReset.Click += btnReset_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAutoScroll
            // 
            btnAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnAutoScroll.Name = "btnAutoScroll";
            btnAutoScroll.Size = new System.Drawing.Size(110, 22);
            btnAutoScroll.Text = "Disable Auto Scroll";
            btnAutoScroll.ToolTipText = "Toggle auto-scroll behavior";
            btnAutoScroll.Click += btnAutoScroll_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLogLimit
            // 
            btnLogLimit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnLogLimit.Name = "btnLogLimit";
            btnLogLimit.Size = new System.Drawing.Size(101, 22);
            btnLogLimit.Text = "Enable Line Limit";
            btnLogLimit.ToolTipText = "Toggle log line limit";
            btnLogLimit.Click += btnLogLimit_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(richTextBox1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 50);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Size = new System.Drawing.Size(1208, 535);
            panel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.Font = new System.Drawing.Font("Cascadia Code", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            richTextBox1.Location = new System.Drawing.Point(4, 3);
            richTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(1200, 529);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1208, 585);
            Controls.Add(panel1);
            Controls.Add(toolStrip2);
            Controls.Add(toolStrip1);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(702, 165);
            Name = "Form1";
            Text = "Serilog RichTextBox Demo";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripButton btnVerbose;
        private System.Windows.Forms.ToolStripButton btnDebug;
        private System.Windows.Forms.ToolStripButton btnInformation;
        private System.Windows.Forms.ToolStripButton btnWarning;
        private System.Windows.Forms.ToolStripButton btnError;
        private System.Windows.Forms.ToolStripButton btnFatal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnParallelFor;
        private System.Windows.Forms.ToolStripButton btnTaskRun;
        private System.Windows.Forms.ToolStripButton btnObject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnScalar;
        private System.Windows.Forms.ToolStripButton btnDictionary;
        private System.Windows.Forms.ToolStripButton btnStructure;
        private System.Windows.Forms.ToolStripButton btnComplex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnDispose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnAutoScroll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnLogLimit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
