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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnVerbose = new System.Windows.Forms.ToolStripButton();
            this.btnDebug = new System.Windows.Forms.ToolStripButton();
            this.btnInformation = new System.Windows.Forms.ToolStripButton();
            this.btnWarning = new System.Windows.Forms.ToolStripButton();
            this.btnError = new System.Windows.Forms.ToolStripButton();
            this.btnFatal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnParallelFor = new System.Windows.Forms.ToolStripButton();
            this.btnTaskRun = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnObject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnScalar = new System.Windows.Forms.ToolStripButton();
            this.btnDictionary = new System.Windows.Forms.ToolStripButton();
            this.btnStructure = new System.Windows.Forms.ToolStripButton();
            this.btnComplex = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDispose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnVerbose,
            this.btnDebug,
            this.btnInformation,
            this.btnWarning,
            this.btnError,
            this.btnFatal,
            this.toolStripSeparator1,
            this.btnParallelFor,
            this.btnTaskRun});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1035, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnVerbose
            // 
            this.btnVerbose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnVerbose.Name = "btnVerbose";
            this.btnVerbose.Size = new System.Drawing.Size(52, 22);
            this.btnVerbose.Text = "Verbose";
            this.btnVerbose.ToolTipText = "Log a verbose message";
            this.btnVerbose.Click += new System.EventHandler(this.btnVerbose_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(46, 22);
            this.btnDebug.Text = "Debug";
            this.btnDebug.ToolTipText = "Log a debug message";
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnInformation
            // 
            this.btnInformation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInformation.Name = "btnInformation";
            this.btnInformation.Size = new System.Drawing.Size(74, 22);
            this.btnInformation.Text = "Information";
            this.btnInformation.ToolTipText = "Log an information message";
            this.btnInformation.Click += new System.EventHandler(this.btnInformation_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(56, 22);
            this.btnWarning.Text = "Warning";
            this.btnWarning.ToolTipText = "Log a warning message";
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(36, 22);
            this.btnError.Text = "Error";
            this.btnError.ToolTipText = "Log an error message";
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // btnFatal
            // 
            this.btnFatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFatal.Name = "btnFatal";
            this.btnFatal.Size = new System.Drawing.Size(36, 22);
            this.btnFatal.Text = "Fatal";
            this.btnFatal.ToolTipText = "Log a fatal message";
            this.btnFatal.Click += new System.EventHandler(this.btnFatal_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnParallelFor
            // 
            this.btnParallelFor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnParallelFor.Name = "btnParallelFor";
            this.btnParallelFor.Size = new System.Drawing.Size(112, 22);
            this.btnParallelFor.Text = "Parallel.For(1000*6)";
            this.btnParallelFor.ToolTipText = "Demonstrate parallel logging with 6000 messages";
            this.btnParallelFor.Click += new System.EventHandler(this.btnParallelFor_Click);
            // 
            // btnTaskRun
            // 
            this.btnTaskRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTaskRun.Name = "btnTaskRun";
            this.btnTaskRun.Size = new System.Drawing.Size(101, 22);
            this.btnTaskRun.Text = "Task.Run(1000*6)";
            this.btnTaskRun.ToolTipText = "Demonstrate async logging with 6000 messages";
            this.btnTaskRun.Click += new System.EventHandler(this.btnTaskRun_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnObject,
            this.toolStripSeparator4,
            this.btnScalar,
            this.btnDictionary,
            this.btnStructure,
            this.btnComplex,
            this.toolStripSeparator8,
            this.btnDispose,
            this.toolStripSeparator5,
            this.btnReset,
            this.toolStripSeparator6,
            this.btnAutoScroll});
            this.toolStrip2.Location = new System.Drawing.Point(0, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStrip2.Size = new System.Drawing.Size(1035, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnObject
            // 
            this.btnObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnObject.Name = "btnObject";
            this.btnObject.Size = new System.Drawing.Size(39, 22);
            this.btnObject.Text = "JSON";
            this.btnObject.ToolTipText = "Log a JSON object";
            this.btnObject.Click += new System.EventHandler(this.btnObject_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnScalar
            // 
            this.btnScalar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnScalar.Name = "btnScalar";
            this.btnScalar.Size = new System.Drawing.Size(42, 22);
            this.btnScalar.Text = "Scalar";
            this.btnScalar.ToolTipText = "Log scalar values";
            this.btnScalar.Click += new System.EventHandler(this.btnScalar_Click);
            // 
            // btnDictionary
            // 
            this.btnDictionary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDictionary.Name = "btnDictionary";
            this.btnDictionary.Size = new System.Drawing.Size(65, 22);
            this.btnDictionary.Text = "Dictionary";
            this.btnDictionary.ToolTipText = "Log a dictionary object";
            this.btnDictionary.Click += new System.EventHandler(this.btnDictionary_Click);
            // 
            // btnStructure
            // 
            this.btnStructure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStructure.Name = "btnStructure";
            this.btnStructure.Size = new System.Drawing.Size(59, 22);
            this.btnStructure.Text = "Structure";
            this.btnStructure.ToolTipText = "Log a structured object";
            this.btnStructure.Click += new System.EventHandler(this.btnStructure_Click);
            // 
            // btnComplex
            // 
            this.btnComplex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnComplex.Name = "btnComplex";
            this.btnComplex.Size = new System.Drawing.Size(58, 22);
            this.btnComplex.Text = "Complex";
            this.btnComplex.ToolTipText = "Log a complex nested object";
            this.btnComplex.Click += new System.EventHandler(this.btnComplex_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDispose
            // 
            this.btnDispose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDispose.Name = "btnDispose";
            this.btnDispose.Size = new System.Drawing.Size(52, 22);
            this.btnDispose.Text = "Dispose";
            this.btnDispose.ToolTipText = "Dispose the logger";
            this.btnDispose.Click += new System.EventHandler(this.btnDispose_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReset
            // 
            this.btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(39, 22);
            this.btnReset.Text = "Reset";
            this.btnReset.ToolTipText = "Reset the logger";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAutoScroll
            // 
            this.btnAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAutoScroll.Name = "btnAutoScroll";
            this.btnAutoScroll.Size = new System.Drawing.Size(110, 22);
            this.btnAutoScroll.Text = "Disable Auto Scroll";
            this.btnAutoScroll.ToolTipText = "Toggle auto-scroll behavior";
            this.btnAutoScroll.Click += new System.EventHandler(this.btnAutoScroll_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(1035, 457);
            this.panel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Cascadia Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1029, 451);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(604, 148);
            this.Name = "Form1";
            this.Text = "Serilog RichTextBox Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
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
    }
}
