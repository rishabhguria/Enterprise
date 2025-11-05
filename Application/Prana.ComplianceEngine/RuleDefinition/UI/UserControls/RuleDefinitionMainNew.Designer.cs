namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    partial class RuleDefinitionMainNew
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
            if (ruleNavigator != null)
                ruleNavigator = null;
            if (notificationSettings != null)
                notificationSettings = null;
            if (ruleDefViewer != null)
                ruleDefViewer = null;
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
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ruleDefViewer = new Prana.ComplianceEngine.RuleDefinition.UI.UserControls.RuleDefViewer();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraBtnReload = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.notificationSettings = new Prana.ComplianceEngine.RuleDefinition.UI.UserControls.NotificationSettings();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ruleNavigator = new Prana.ComplianceEngine.RuleDefinition.UI.UserControls.RuleNavigator();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ruleDefViewer);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraPanel1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraSplitter2);
            this.ultraPnlMain.ClientArea.Controls.Add(this.notificationSettings);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ruleNavigator);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(1131, 650);
            this.ultraPnlMain.TabIndex = 10;
            // 
            // ruleDefViewer
            // 
            this.ruleDefViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleDefViewer.Location = new System.Drawing.Point(250, 47);
            this.ruleDefViewer.Name = "ruleDefViewer";
            this.ruleDefViewer.Size = new System.Drawing.Size(881, 471);
            this.ruleDefViewer.TabIndex = 7;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraBtnReload);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraBtnSave);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(250, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(881, 47);
            this.ultraPanel1.TabIndex = 6;
            // 
            // ultraBtnReload
            // 
            this.ultraBtnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraBtnReload.Location = new System.Drawing.Point(695, 9);
            this.ultraBtnReload.Name = "ultraBtnReload";
            this.ultraBtnReload.Size = new System.Drawing.Size(84, 23);
            this.ultraBtnReload.TabIndex = 1;
            this.ultraBtnReload.Text = "Reload Rule";
            this.ultraBtnReload.Click += new System.EventHandler(this.ultraBtnReload_Click);
            // 
            // ultraBtnSave
            // 
            this.ultraBtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraBtnSave.Location = new System.Drawing.Point(785, 9);
            this.ultraBtnSave.Name = "ultraBtnSave";
            this.ultraBtnSave.Size = new System.Drawing.Size(75, 23);
            this.ultraBtnSave.TabIndex = 0;
            this.ultraBtnSave.Text = "Save";
            this.ultraBtnSave.Click += new System.EventHandler(this.ultraBtnSave_Click);
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(250, 518);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 97;
            this.ultraSplitter2.Size = new System.Drawing.Size(881, 6);
            this.ultraSplitter2.TabIndex = 5;
            // 
            // notificationSettings
            // 
            this.notificationSettings.AutoScroll = true;
            this.notificationSettings.BackColor = System.Drawing.SystemColors.Control;
            this.notificationSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.notificationSettings.Location = new System.Drawing.Point(250, 524);
            this.notificationSettings.Name = "notificationSettings";
            this.notificationSettings.Size = new System.Drawing.Size(881, 126);
            this.notificationSettings.TabIndex = 4;
            this.notificationSettings.Load += new System.EventHandler(this.notificationSettings_Load);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.Location = new System.Drawing.Point(244, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 230;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 650);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // ruleNavigator
            // 
            this.ruleNavigator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ruleNavigator.Dock = System.Windows.Forms.DockStyle.Left;
            this.ruleNavigator.Location = new System.Drawing.Point(0, 0);
            this.ruleNavigator.Name = "ruleNavigator";
            this.ruleNavigator.Size = new System.Drawing.Size(244, 650);
            this.ruleNavigator.TabIndex = 0;
            this.ruleNavigator.Load += new System.EventHandler(this.ruleNavigator_Load);
            // 
            // RuleDefinitionMainNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPnlMain);
            this.MinimumSize = new System.Drawing.Size(1131, 650);
            this.Name = "RuleDefinitionMainNew";
            this.Size = new System.Drawing.Size(1131, 650);
            this.Load += new System.EventHandler(this.RuleDefinitionMainNew_Load);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private NotificationSettings notificationSettings;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private RuleNavigator ruleNavigator;
        private RuleDefViewer ruleDefViewer;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton ultraBtnSave;
        private Infragistics.Win.Misc.UltraButton ultraBtnReload;
       // private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;


    }
}
