namespace Prana.ComplianceEngine
{
    partial class ComplianceEngine
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
                if (ruleGroupingMain1 != null)
                {
                    ruleGroupingMain1 = null;
                }
                if (pendingApprovalMain1 != null)
                {
                    pendingApprovalMain1 = null;
                }
                if (alertHistoryMainNew1 != null)
                {
                    alertHistoryMainNew1 = null;
                }
                if (ruleDefinitionMainNew1 != null)
                {
                    ruleDefinitionMainNew1 = null;
                }
                if (statusBarRefreshTimer != null)
                {
                    statusBarRefreshTimer.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComplianceEngine));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ruleDefinitionMainNew1 = new Prana.ComplianceEngine.RuleDefinition.UI.UserControls.RuleDefinitionMainNew();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ruleGroupingMain1 = new Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls.RuleGroupingMain();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraStatusBar2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.alertHistoryMainNew1 = new Prana.ComplianceEngine.AlertHistory.UI.UserControls.AlertHistoryMain();
            this.pendingApprovalMain1 = new Prana.ComplianceEngine.Pending_Approval_UI.UI.PendingApprovalMain();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ufmDefault = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar2)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ufmDefault)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraPnlMain);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 21);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1119, 596);
            // 
            // ultraPnlMain
            // 
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ruleDefinitionMainNew1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraStatusBar1);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(1119, 596);
            this.ultraPnlMain.TabIndex = 0;
            // 
            // ruleDefinitionMainNew1
            // 
            this.ruleDefinitionMainNew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleDefinitionMainNew1.Location = new System.Drawing.Point(0, 0);
            this.ruleDefinitionMainNew1.MinimumSize = new System.Drawing.Size(1131, 560);
            this.ruleDefinitionMainNew1.Name = "ruleDefinitionMainNew1";
            this.ruleDefinitionMainNew1.Size = new System.Drawing.Size(1131, 567);
            this.ruleDefinitionMainNew1.TabIndex = 2;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 567);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 23;
            this.ultraSplitter1.Size = new System.Drawing.Size(1119, 6);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 573);
            this.ultraStatusBar1.MaximumSize = new System.Drawing.Size(0, 23);
            this.ultraStatusBar1.MinimumSize = new System.Drawing.Size(1131, 23);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(1131, 23);
            this.ultraStatusBar1.TabIndex = 0;
            this.ultraStatusBar1.Text = "ultraStatusBar1";
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ultraPanel2);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1119, 596);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.ultraPanel3);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1119, 596);
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ruleGroupingMain1);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraSplitter2);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraStatusBar2);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1119, 596);
            this.ultraPanel2.TabIndex = 0;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.pendingApprovalMain1);
            //this.ultraPanel3.ClientArea.Controls.Add(this.ultraSplitter2);
            //this.ultraPanel3.ClientArea.Controls.Add(this.ultraStatusBar2);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1119, 596);
            this.ultraPanel3.TabIndex = 0;
            // 
            // ruleGroupingMain1
            // 
            this.ruleGroupingMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleGroupingMain1.Location = new System.Drawing.Point(0, 0);
            this.ruleGroupingMain1.MinimumSize = new System.Drawing.Size(1119, 596);
            this.ruleGroupingMain1.Name = "ruleGroupingMain1";
            this.ruleGroupingMain1.Size = new System.Drawing.Size(1119, 596);
            this.ruleGroupingMain1.TabIndex = 0;
            // 
            // pendingApprovalMain1
            // 
            this.pendingApprovalMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pendingApprovalMain1.Location = new System.Drawing.Point(0, 0);
            this.pendingApprovalMain1.Name = "pendingApprovalMain1";
            this.pendingApprovalMain1.TabIndex = 4;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(250)))), ((int)(((byte)(247)))));
            this.ultraSplitter2.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(0, 567);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 23;
            this.ultraSplitter2.Size = new System.Drawing.Size(1119, 6);
            this.ultraSplitter2.TabIndex = 1;
            // 
            // ultraStatusBar2
            // 
            this.ultraStatusBar2.Location = new System.Drawing.Point(0, 573);
            this.ultraStatusBar2.Name = "ultraStatusBar2";
            this.ultraStatusBar2.Size = new System.Drawing.Size(1119, 23);
            this.ultraStatusBar2.TabIndex = 0;
            this.ultraStatusBar2.Text = "ultraStatusBar2";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ultraPanel1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1119, 596);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.alertHistoryMainNew1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            //this.ultraPanel1.Size = new System.Drawing.Size(1119, 596);
            this.ultraPanel1.TabIndex = 0;
            // 
            // alertHistoryMainNew1
            // 
            this.alertHistoryMainNew1.AutoScroll = true;
            this.alertHistoryMainNew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertHistoryMainNew1.Location = new System.Drawing.Point(0, 0);
            this.alertHistoryMainNew1.MinimumSize = new System.Drawing.Size(1131, 560);
            this.alertHistoryMainNew1.Name = "alertHistoryMainNew1";
            this.alertHistoryMainNew1.Size = new System.Drawing.Size(1131, 596);
            this.alertHistoryMainNew1.TabIndex = 0;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(4, 27);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            //this.ultraTabControl1.Size = new System.Drawing.Size(1123, 619);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.Key = "ruleDefinition";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Rule Definition";
            ultraTab3.Key = "groupRules";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Rule Groups";
            ultraTab2.Key = "alertHistory";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Alert History";
            ultraTab4.Key = "pendingApproval";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Pending Approval";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab3,
            ultraTab2,
            ultraTab4
            });
            this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1119, 596);
            // 
            // ufmDefault
            // 
            this.ufmDefault.Form = this;
            this.ufmDefault.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this.ufmDefault.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // _ComplianceEngine_UltraFormManager_Dock_Area_Left
            // 
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.FormManager = this.ufmDefault;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.Name = "_ComplianceEngine_UltraFormManager_Dock_Area_Left";
            this._ComplianceEngine_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 619);
            // 
            // _ComplianceEngine_UltraFormManager_Dock_Area_Right
            // 
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.FormManager = this.ufmDefault;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1127, 27);
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.Name = "_ComplianceEngine_UltraFormManager_Dock_Area_Right";
            this._ComplianceEngine_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 619);
            // 
            // _ComplianceEngine_UltraFormManager_Dock_Area_Top
            // 
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.FormManager = this.ufmDefault;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.Name = "_ComplianceEngine_UltraFormManager_Dock_Area_Top";
            this._ComplianceEngine_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1131, 27);
            // 
            // _ComplianceEngine_UltraFormManager_Dock_Area_Bottom
            // 
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.FormManager = this.ufmDefault;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 646);
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.Name = "_ComplianceEngine_UltraFormManager_Dock_Area_Bottom";
            this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1131, 4);
            // 
            // ComplianceEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 650);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this._ComplianceEngine_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ComplianceEngine_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ComplianceEngine_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ComplianceEngine_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1131, 650);
            this.Name = "ComplianceEngine";
            this.Text = "Compliance Engine";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ComlianceEngine_FormClosed);
            this.Load += new System.EventHandler(this.ComplianceEngine_Load);
            this.Shown += new System.EventHandler(this.ComplianceEngineOld_Shown);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl4.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar2)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ufmDefault)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
        private RuleDefinition.UI.UserControls.RuleDefinitionMainNew ruleDefinitionMainNew1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ufmDefault;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ComplianceEngine_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ComplianceEngine_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ComplianceEngine_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ComplianceEngine_UltraFormManager_Dock_Area_Bottom;
        private AlertHistory.UI.UserControls.AlertHistoryMain alertHistoryMainNew1;
        private Pending_Approval_UI.UI.PendingApprovalMain pendingApprovalMain1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private RuleDefinition.RulePluggins.UserControls.RuleGroupingMain ruleGroupingMain1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar2;
    }
}