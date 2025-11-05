using Prana.PNL.UI.Controls;
namespace Prana.PNL.UI.Forms
{
    partial class PNLMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tbcPNL = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.tbcPNL)).BeginInit();
            this.tbcPNL.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(2, 21);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(947, 366);
            // 
            // tbcPNL
            // 
            this.tbcPNL.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbcPNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcPNL.Location = new System.Drawing.Point(0, 0);
            this.tbcPNL.Name = "tbcPNL";
            this.tbcPNL.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbcPNL.Size = new System.Drawing.Size(951, 389);
            this.tbcPNL.TabIndex = 0;
            this.tbcPNL.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // PNLMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 389);
            this.Controls.Add(this.tbcPNL);
            this.Name = "PNLMain";
            this.Text = "PNL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPNLMain_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PNLMain_FormClosing);
            this.Load += new System.EventHandler(this.frmPNLMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbcPNL)).EndInit();
            this.tbcPNL.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcPNL;

    }
}