namespace Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls
{
    partial class RuleGroupingMain
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
            if (ruleGroupingControl1 != null)
                ruleGroupingControl1 = null;
            if (notificationSettings1 != null)
                notificationSettings1 = null;
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
            this.ruleGroupingControl1 = new Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls.RuleGroupingControl();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.notificationSettings1 = new Prana.ComplianceEngine.RuleDefinition.UI.UserControls.NotificationSettings();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraPnlButtons = new Infragistics.Win.Misc.UltraPanel();
            this.ultraBtnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            this.ultraPnlButtons.ClientArea.SuspendLayout();
            this.ultraPnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ruleGroupingControl1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraSplitter2);
            this.ultraPnlMain.ClientArea.Controls.Add(this.notificationSettings1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraPnlButtons);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(1131, 650);
            this.ultraPnlMain.TabIndex = 0;
            // 
            // ruleGroupingControl1
            // 
            this.ruleGroupingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleGroupingControl1.Location = new System.Drawing.Point(0, 46);
            this.ruleGroupingControl1.Name = "ruleGroupingControl1";
            this.ruleGroupingControl1.Size = new System.Drawing.Size(1131, 472);
            this.ruleGroupingControl1.TabIndex = 4;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(0, 518);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 126;
            this.ultraSplitter2.Size = new System.Drawing.Size(1131, 6);
            this.ultraSplitter2.TabIndex = 3;
            // 
            // notificationSettings1
            // 
            this.notificationSettings1.AutoScroll = true;
            this.notificationSettings1.BackColor = System.Drawing.SystemColors.Control;
            this.notificationSettings1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.notificationSettings1.Location = new System.Drawing.Point(0, 524);
            this.notificationSettings1.Name = "notificationSettings1";
            this.notificationSettings1.Size = new System.Drawing.Size(1131, 126);
            this.notificationSettings1.TabIndex = 2;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 40);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 40;
            this.ultraSplitter1.Size = new System.Drawing.Size(1131, 6);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // ultraPnlButtons
            // 
            // 
            // ultraPnlButtons.ClientArea
            // 
            this.ultraPnlButtons.ClientArea.Controls.Add(this.ultraBtnSave);
            this.ultraPnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPnlButtons.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlButtons.Name = "ultraPnlButtons";
            this.ultraPnlButtons.Size = new System.Drawing.Size(1131, 40);
            this.ultraPnlButtons.TabIndex = 0;
            // 
            // ultraBtnSave
            // 
            this.ultraBtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraBtnSave.Location = new System.Drawing.Point(495, 11);
            this.ultraBtnSave.Name = "ultraBtnSave";
            this.ultraBtnSave.Size = new System.Drawing.Size(75, 23);
            this.ultraBtnSave.TabIndex = 0;
            this.ultraBtnSave.Text = "Save";
            this.ultraBtnSave.Click += new System.EventHandler(this.ultraBtnSave_Click);
            // 
            // RuleGroupingMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPnlMain);
            this.MinimumSize = new System.Drawing.Size(1131, 650);
            this.Name = "RuleGroupingMain";
            this.Size = new System.Drawing.Size(1131, 650);
            this.Load += new System.EventHandler(this.RuleGroupingMain_Load);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            this.ultraPnlButtons.ClientArea.ResumeLayout(false);
            this.ultraPnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraPanel ultraPnlButtons;
        private RuleGroupingControl ruleGroupingControl1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private UI.UserControls.NotificationSettings notificationSettings1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraButton ultraBtnSave;

    }
}
