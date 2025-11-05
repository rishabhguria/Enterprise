namespace Prana.AuditManager.UI
{
    partial class AuditControl
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
            this.ultraExpandableGroupBoxPanelAudit = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBoxMain = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxMain)).BeginInit();
            this.ultraExpandableGroupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraExpandableGroupBoxPanelAudit
            // 
            this.ultraExpandableGroupBoxPanelAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanelAudit.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanelAudit.Name = "ultraExpandableGroupBoxPanelAudit";
            this.ultraExpandableGroupBoxPanelAudit.Size = new System.Drawing.Size(294, 93);
            this.ultraExpandableGroupBoxPanelAudit.TabIndex = 1;
            // 
            // ultraExpandableGroupBoxMain
            // 
            this.ultraExpandableGroupBoxMain.Controls.Add(this.ultraExpandableGroupBoxPanelAudit);
            this.ultraExpandableGroupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxMain.ExpandedSize = new System.Drawing.Size(300, 115);
            this.ultraExpandableGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxMain.Name = "ultraExpandableGroupBoxMain";
            this.ultraExpandableGroupBoxMain.Size = new System.Drawing.Size(300, 115);
            this.ultraExpandableGroupBoxMain.TabIndex = 0;
            this.ultraExpandableGroupBoxMain.Text = "Audit Trail";
            // 
            // AuditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraExpandableGroupBoxMain);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AuditControl";
            this.Size = new System.Drawing.Size(300, 115);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxMain)).EndInit();
            this.ultraExpandableGroupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Modified By : Manvendra P.
        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-1078

        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelAudit;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBoxMain;
        
    }
}
