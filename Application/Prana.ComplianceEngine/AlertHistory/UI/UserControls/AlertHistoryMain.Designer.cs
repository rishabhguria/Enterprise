namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    partial class AlertHistoryMain
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
            if (alertOperations1 != null)
                alertOperations1 = null;
            if (alertGrid1 != null)
                alertGrid1 = null;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.alertOperations1 = new Prana.ComplianceEngine.AlertHistory.UI.UserControls.AlertOperations();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.alertGrid1 = new Prana.ComplianceEngine.AlertHistory.UI.UserControls.AlertGrid();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.alertGrid1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPanel1.ClientArea.Controls.Add(this.alertOperations1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1131, 650);
            this.ultraPanel1.TabIndex = 0;
            // 
            // alertOperations1
            // 
            this.alertOperations1.AutoScroll = true;
            this.alertOperations1.Dock = System.Windows.Forms.DockStyle.Top;
            this.alertOperations1.Location = new System.Drawing.Point(0, 0);
            this.alertOperations1.Name = "alertOperations1";
            this.alertOperations1.Size = new System.Drawing.Size(1131, 40);
            this.alertOperations1.TabIndex = 0;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 40);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 40;
            this.ultraSplitter1.Size = new System.Drawing.Size(1131, 6);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // alertGrid1
            // 
            this.alertGrid1.AutoScroll = true;
            this.alertGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertGrid1.Location = new System.Drawing.Point(0, 46);
            this.alertGrid1.Name = "alertGrid1";
            this.alertGrid1.Size = new System.Drawing.Size(1131, 604);
            this.alertGrid1.TabIndex = 2;
            this.alertGrid1.Load += new System.EventHandler(this.alertGrid1_Load);
            // 
            // AlertHistoryMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "AlertHistoryMain";
            this.Size = new System.Drawing.Size(1131, 650);
            this.Load += new System.EventHandler(this.AlertHistoryMain_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private AlertOperations alertOperations1;
        private AlertGrid alertGrid1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
    }
}
