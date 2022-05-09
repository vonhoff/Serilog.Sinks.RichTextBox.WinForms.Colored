namespace SampleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnVerbose = new System.Windows.Forms.ToolStripButton();
            this.btnDebug = new System.Windows.Forms.ToolStripButton();
            this.btnInformation = new System.Windows.Forms.ToolStripButton();
            this.btnWarning = new System.Windows.Forms.ToolStripButton();
            this.btnError = new System.Windows.Forms.ToolStripButton();
            this.btnFatal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnParallelFor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTaskRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnObject = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClear,
            this.btnVerbose,
            this.btnDebug,
            this.btnInformation,
            this.btnWarning,
            this.btnError,
            this.btnFatal,
            this.toolStripSeparator1,
            this.btnParallelFor,
            this.toolStripSeparator2,
            this.btnTaskRun,
            this.toolStripSeparator3,
            this.btnObject});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(997, 33);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(3);
            this.btnClear.Size = new System.Drawing.Size(53, 30);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnVerbose
            // 
            this.btnVerbose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnVerbose.Image = ((System.Drawing.Image)(resources.GetObject("btnVerbose.Image")));
            this.btnVerbose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnVerbose.Name = "btnVerbose";
            this.btnVerbose.Padding = new System.Windows.Forms.Padding(3);
            this.btnVerbose.Size = new System.Drawing.Size(72, 30);
            this.btnVerbose.Text = "Verbose";
            this.btnVerbose.Click += new System.EventHandler(this.BtnVerbose_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDebug.Image = ((System.Drawing.Image)(resources.GetObject("btnDebug.Image")));
            this.btnDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Padding = new System.Windows.Forms.Padding(3);
            this.btnDebug.Size = new System.Drawing.Size(64, 30);
            this.btnDebug.Text = "Debug";
            this.btnDebug.Click += new System.EventHandler(this.BtnDebug_Click);
            // 
            // btnInformation
            // 
            this.btnInformation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInformation.Image = ((System.Drawing.Image)(resources.GetObject("btnInformation.Image")));
            this.btnInformation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInformation.Name = "btnInformation";
            this.btnInformation.Padding = new System.Windows.Forms.Padding(3);
            this.btnInformation.Size = new System.Drawing.Size(97, 30);
            this.btnInformation.Text = "Information";
            this.btnInformation.Click += new System.EventHandler(this.BtnInformation_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnWarning.Image = ((System.Drawing.Image)(resources.GetObject("btnWarning.Image")));
            this.btnWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Padding = new System.Windows.Forms.Padding(3);
            this.btnWarning.Size = new System.Drawing.Size(74, 30);
            this.btnWarning.Text = "Warning";
            this.btnWarning.Click += new System.EventHandler(this.BtnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnError.Image = ((System.Drawing.Image)(resources.GetObject("btnError.Image")));
            this.btnError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnError.Name = "btnError";
            this.btnError.Padding = new System.Windows.Forms.Padding(3);
            this.btnError.Size = new System.Drawing.Size(51, 30);
            this.btnError.Text = "Error";
            this.btnError.Click += new System.EventHandler(this.BtnError_Click);
            // 
            // btnFatal
            // 
            this.btnFatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFatal.Image = ((System.Drawing.Image)(resources.GetObject("btnFatal.Image")));
            this.btnFatal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFatal.Name = "btnFatal";
            this.btnFatal.Padding = new System.Windows.Forms.Padding(3);
            this.btnFatal.Size = new System.Drawing.Size(50, 30);
            this.btnFatal.Text = "Fatal";
            this.btnFatal.Click += new System.EventHandler(this.BtnFatal_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // btnParallelFor
            // 
            this.btnParallelFor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnParallelFor.Image = ((System.Drawing.Image)(resources.GetObject("btnParallelFor.Image")));
            this.btnParallelFor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnParallelFor.Name = "btnParallelFor";
            this.btnParallelFor.Size = new System.Drawing.Size(133, 30);
            this.btnParallelFor.Text = "Parallel.For(100*6)";
            this.btnParallelFor.Click += new System.EventHandler(this.BtnParallelFor_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btnTaskRun
            // 
            this.btnTaskRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTaskRun.Image = ((System.Drawing.Image)(resources.GetObject("btnTaskRun.Image")));
            this.btnTaskRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTaskRun.Name = "btnTaskRun";
            this.btnTaskRun.Padding = new System.Windows.Forms.Padding(3);
            this.btnTaskRun.Size = new System.Drawing.Size(122, 30);
            this.btnTaskRun.Text = "Task.Run(100*6)";
            this.btnTaskRun.Click += new System.EventHandler(this.BtnTaskRun_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // btnObject
            // 
            this.btnObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnObject.Image = ((System.Drawing.Image)(resources.GetObject("btnObject.Image")));
            this.btnObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnObject.Name = "btnObject";
            this.btnObject.Padding = new System.Windows.Forms.Padding(3);
            this.btnObject.Size = new System.Drawing.Size(54, 30);
            this.btnObject.Text = "JSON";
            this.btnObject.Click += new System.EventHandler(this.BtnObject_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(997, 610);
            this.panel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(997, 610);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 643);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnTaskRun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnObject;
    }
}
