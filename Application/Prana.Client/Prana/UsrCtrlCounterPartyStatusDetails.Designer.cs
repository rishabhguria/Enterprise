namespace Prana
{
    partial class UsrCtrlCounterPartyStatusDetails
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
            this.panelCounterPartiesStatus = new Infragistics.Win.Misc.UltraPanel();
            this.panelCounterPartiesStatus.SuspendLayout();
            this.ClientArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCounterPartiesStatus
            // 
            this.panelCounterPartiesStatus.AutoScroll = true;
            this.panelCounterPartiesStatus.AutoScrollMargin = new System.Drawing.Size(0, 15);
            this.panelCounterPartiesStatus.AutoScrollMinSize = new System.Drawing.Size(0, 5);
            //this.panelCounterPartiesStatus.AutoSize = true;
            this.panelCounterPartiesStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCounterPartiesStatus.Location = new System.Drawing.Point(0, 0);
            this.panelCounterPartiesStatus.Name = "panelCounterPartiesStatus";
            //this.panelCounterPartiesStatus.Size = new System.Drawing.Size(150, 200);
            this.panelCounterPartiesStatus.TabIndex = 0;
            // 
            // UsrCtrlCounterPartyStatusDetails
            // 
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 15);
            this.AutoScrollMinSize = new System.Drawing.Size(0, 5);
            //this.AutoSize = true;
            // 
            // 
            // 
            this.ClientArea.Controls.Add(this.panelCounterPartiesStatus);
            this.Size = new System.Drawing.Size(150, 200);
            this.panelCounterPartiesStatus.ResumeLayout(false);
            this.panelCounterPartiesStatus.PerformLayout();
            this.ClientArea.ResumeLayout(false);
            this.ClientArea.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel panelCounterPartiesStatus;
    }
}
