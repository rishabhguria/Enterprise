namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    partial class RuleDefViewer
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (timer != null)
                {
                    timer.Dispose();
                }
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
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            this.ultraPnlMain.AutoScroll = true;
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(150, 150);
            this.ultraPnlMain.TabIndex = 0;
            this.ultraPnlMain.PaintClient += new System.Windows.Forms.PaintEventHandler(this.ultraPnlMain_PaintClient);
            // 
            // RuleDefViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPnlMain);
            this.Name = "RuleDefViewer";
            this.ultraPnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
    }
}
