namespace Prana.PM.Client.UI
{
    partial class ctrlProgress
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.lblImporting = new System.Windows.Forms.Label();
            this.pBarProgressing = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.panelProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelProgress
            // 
            this.panelProgress.Controls.Add(this.lblImporting);
            this.panelProgress.Controls.Add(this.pBarProgressing);
            this.panelProgress.Controls.Add(this.lblProgress);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProgress.Location = new System.Drawing.Point(0, 0);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(529, 35);
            this.panelProgress.TabIndex = 91;
            this.panelProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.panelProgress_Paint);
            // 
            // lblImporting
            // 
            this.lblImporting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImporting.AutoSize = true;
            this.lblImporting.Location = new System.Drawing.Point(180, 10);
            this.lblImporting.Name = "lblImporting";
            this.lblImporting.Size = new System.Drawing.Size(50, 13);
            this.lblImporting.TabIndex = 88;
            //this.lblImporting.Text = "Importing";
            // 
            // pBarProgressing
            // 
            this.pBarProgressing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pBarProgressing.Location = new System.Drawing.Point(411, 7);
            this.pBarProgressing.Name = "pBarProgressing";
            this.pBarProgressing.Size = new System.Drawing.Size(114, 20);
            this.pBarProgressing.TabIndex = 87;
            this.pBarProgressing.Text = "[Formatted]";
            this.pBarProgressing.Hide();
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            //this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(350, 10);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(48, 13);
            this.lblProgress.TabIndex = 89;
            //this.lblProgress.Text = "Progress";
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 1000;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // ctrlProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelProgress);
            this.Name = "ctrlProgress";
            this.Size = new System.Drawing.Size(529, 35);
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.Label lblImporting;
        public Infragistics.Win.UltraWinProgressBar.UltraProgressBar pBarProgressing;
        private System.Windows.Forms.Label lblProgress;
        public System.Windows.Forms.Timer timerProgress;
    }
}
