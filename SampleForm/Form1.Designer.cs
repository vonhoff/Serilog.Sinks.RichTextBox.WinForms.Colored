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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnClear = new System.Windows.Forms.ToolStripButton();
            btnVerbose = new System.Windows.Forms.ToolStripButton();
            btnDebug = new System.Windows.Forms.ToolStripButton();
            btnInformation = new System.Windows.Forms.ToolStripButton();
            btnWarning = new System.Windows.Forms.ToolStripButton();
            btnError = new System.Windows.Forms.ToolStripButton();
            btnFatal = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btnParallelFor = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            btnTaskRun = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            btnObject = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            btnDispose = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            btnReset = new System.Windows.Forms.ToolStripButton();
            panel1 = new System.Windows.Forms.Panel();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            btnAutoScroll = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            btnLogLimit = new System.Windows.Forms.ToolStripButton();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnClear, btnVerbose, btnDebug, btnInformation, btnWarning, btnError, btnFatal, toolStripSeparator1, btnParallelFor, toolStripSeparator2, btnTaskRun, toolStripSeparator3, btnObject, toolStripSeparator4, btnDispose, toolStripSeparator5, btnReset, toolStripSeparator6, btnAutoScroll, toolStripSeparator7, btnLogLimit });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1358, 33);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnClear
            // 
            btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnClear.Image = (System.Drawing.Image)resources.GetObject("btnClear.Image");
            btnClear.Name = "btnClear";
            btnClear.Padding = new System.Windows.Forms.Padding(3);
            btnClear.Size = new System.Drawing.Size(53, 30);
            btnClear.Text = "Clear";
            btnClear.Click += BtnClear_Click;
            // 
            // btnVerbose
            // 
            btnVerbose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnVerbose.Image = (System.Drawing.Image)resources.GetObject("btnVerbose.Image");
            btnVerbose.Name = "btnVerbose";
            btnVerbose.Padding = new System.Windows.Forms.Padding(3);
            btnVerbose.Size = new System.Drawing.Size(72, 30);
            btnVerbose.Text = "Verbose";
            btnVerbose.Click += BtnVerbose_Click;
            // 
            // btnDebug
            // 
            btnDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDebug.Image = (System.Drawing.Image)resources.GetObject("btnDebug.Image");
            btnDebug.Name = "btnDebug";
            btnDebug.Padding = new System.Windows.Forms.Padding(3);
            btnDebug.Size = new System.Drawing.Size(64, 30);
            btnDebug.Text = "Debug";
            btnDebug.Click += BtnDebug_Click;
            // 
            // btnInformation
            // 
            btnInformation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnInformation.Image = (System.Drawing.Image)resources.GetObject("btnInformation.Image");
            btnInformation.Name = "btnInformation";
            btnInformation.Padding = new System.Windows.Forms.Padding(3);
            btnInformation.Size = new System.Drawing.Size(97, 30);
            btnInformation.Text = "Information";
            btnInformation.Click += BtnInformation_Click;
            // 
            // btnWarning
            // 
            btnWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnWarning.Image = (System.Drawing.Image)resources.GetObject("btnWarning.Image");
            btnWarning.Name = "btnWarning";
            btnWarning.Padding = new System.Windows.Forms.Padding(3);
            btnWarning.Size = new System.Drawing.Size(74, 30);
            btnWarning.Text = "Warning";
            btnWarning.Click += BtnWarning_Click;
            // 
            // btnError
            // 
            btnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnError.Image = (System.Drawing.Image)resources.GetObject("btnError.Image");
            btnError.Name = "btnError";
            btnError.Padding = new System.Windows.Forms.Padding(3);
            btnError.Size = new System.Drawing.Size(51, 30);
            btnError.Text = "Error";
            btnError.Click += BtnError_Click;
            // 
            // btnFatal
            // 
            btnFatal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnFatal.Image = (System.Drawing.Image)resources.GetObject("btnFatal.Image");
            btnFatal.Name = "btnFatal";
            btnFatal.Padding = new System.Windows.Forms.Padding(3);
            btnFatal.Size = new System.Drawing.Size(50, 30);
            btnFatal.Text = "Fatal";
            btnFatal.Click += BtnFatal_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // btnParallelFor
            // 
            btnParallelFor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnParallelFor.Image = (System.Drawing.Image)resources.GetObject("btnParallelFor.Image");
            btnParallelFor.Name = "btnParallelFor";
            btnParallelFor.Size = new System.Drawing.Size(133, 30);
            btnParallelFor.Text = "Parallel.For(100*6)";
            btnParallelFor.Click += BtnParallelFor_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btnTaskRun
            // 
            btnTaskRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnTaskRun.Image = (System.Drawing.Image)resources.GetObject("btnTaskRun.Image");
            btnTaskRun.Name = "btnTaskRun";
            btnTaskRun.Padding = new System.Windows.Forms.Padding(3);
            btnTaskRun.Size = new System.Drawing.Size(122, 30);
            btnTaskRun.Text = "Task.Run(100*6)";
            btnTaskRun.Click += BtnTaskRun_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // btnObject
            // 
            btnObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnObject.Image = (System.Drawing.Image)resources.GetObject("btnObject.Image");
            btnObject.Name = "btnObject";
            btnObject.Padding = new System.Windows.Forms.Padding(3);
            btnObject.Size = new System.Drawing.Size(54, 30);
            btnObject.Text = "JSON";
            btnObject.Click += BtnObject_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // btnDispose
            // 
            btnDispose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDispose.Image = (System.Drawing.Image)resources.GetObject("btnDispose.Image");
            btnDispose.Name = "btnDispose";
            btnDispose.Padding = new System.Windows.Forms.Padding(3);
            btnDispose.Size = new System.Drawing.Size(72, 30);
            btnDispose.Text = "Dispose";
            btnDispose.Click += BtnDispose_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
            // 
            // btnReset
            // 
            btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnReset.Image = (System.Drawing.Image)resources.GetObject("btnReset.Image");
            btnReset.Name = "btnReset";
            btnReset.Padding = new System.Windows.Forms.Padding(3);
            btnReset.Size = new System.Drawing.Size(55, 30);
            btnReset.Text = "Reset";
            btnReset.Click += BtnReset_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(richTextBox1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 33);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1358, 610);
            panel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox1.Location = new System.Drawing.Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(1358, 610);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
            // 
            // btnAutoScroll
            // 
            btnAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnAutoScroll.Image = (System.Drawing.Image)resources.GetObject("btnAutoScroll.Image");
            btnAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAutoScroll.Name = "btnAutoScroll";
            btnAutoScroll.Padding = new System.Windows.Forms.Padding(3);
            btnAutoScroll.Size = new System.Drawing.Size(146, 30);
            btnAutoScroll.Text = "Disable Auto Scroll";
            btnAutoScroll.Click += BtnAutoScroll_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(6, 33);
            //
            // btnLogLimit
            //
            btnLogLimit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnLogLimit.Image = (System.Drawing.Image)resources.GetObject("btnLogLimit.Image");
            btnLogLimit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnLogLimit.Name = "btnLogLimit";
            btnLogLimit.Padding = new System.Windows.Forms.Padding(3);
            btnLogLimit.Size = new System.Drawing.Size(146, 30);
            btnLogLimit.Text = "Enable Line Limit";
            btnLogLimit.Click += BtnLogLimit_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1358, 643);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnDispose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnAutoScroll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnLogLimit;
    }
}
