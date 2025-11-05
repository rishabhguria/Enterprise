namespace Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls
{
    partial class RuleGroupingControl
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleGroupingControl));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGrpButtons = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraBtnAssignSelected = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnAssignAll = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnUnassignAll = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnUnassignSelected = new Infragistics.Win.Misc.UltraButton();
            this.ultraGrpUnassignedRules = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLstUnAssigned = new Infragistics.Win.UltraWinListView.UltraListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ultraSplitter3 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraTxtSearchUnassigned = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraGrpAssignedRules = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLstAssignedRules = new Infragistics.Win.UltraWinListView.UltraListView();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraTxtSearchAssigned = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraGrpRuleGroups = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLstGroup = new Infragistics.Win.UltraWinListView.UltraListView();
            this.cntxtMnuGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraTxtSearchGroup = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpButtons)).BeginInit();
            this.ultraGrpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpUnassignedRules)).BeginInit();
            this.ultraGrpUnassignedRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstUnAssigned)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchUnassigned)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpAssignedRules)).BeginInit();
            this.ultraGrpAssignedRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstAssignedRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchAssigned)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpRuleGroups)).BeginInit();
            this.ultraGrpRuleGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraGrpButtons);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraGrpUnassignedRules);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraGrpAssignedRules);
            this.ultraPnlMain.ClientArea.Controls.Add(this.ultraGrpRuleGroups);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(1131, 438);
            this.ultraPnlMain.TabIndex = 0;
            // 
            // ultraGrpButtons
            // 
            this.ultraGrpButtons.Controls.Add(this.ultraBtnAssignSelected);
            this.ultraGrpButtons.Controls.Add(this.ultraBtnAssignAll);
            this.ultraGrpButtons.Controls.Add(this.ultraBtnUnassignAll);
            this.ultraGrpButtons.Controls.Add(this.ultraBtnUnassignSelected);
            this.ultraGrpButtons.Location = new System.Drawing.Point(355, 40);
            this.ultraGrpButtons.Name = "ultraGrpButtons";
            this.ultraGrpButtons.Size = new System.Drawing.Size(68, 358);
            this.ultraGrpButtons.TabIndex = 3;
            // 
            // ultraBtnAssignSelected
            // 
            this.ultraBtnAssignSelected.Location = new System.Drawing.Point(17, 42);
            this.ultraBtnAssignSelected.Name = "ultraBtnAssignSelected";
            this.ultraBtnAssignSelected.Size = new System.Drawing.Size(36, 33);
            this.ultraBtnAssignSelected.TabIndex = 3;
            this.ultraBtnAssignSelected.Text = ">";
            this.ultraBtnAssignSelected.Click += new System.EventHandler(this.ultraBtnAssignSelected_Click);
            // 
            // ultraBtnAssignAll
            // 
            this.ultraBtnAssignAll.Location = new System.Drawing.Point(17, 81);
            this.ultraBtnAssignAll.Name = "ultraBtnAssignAll";
            this.ultraBtnAssignAll.Size = new System.Drawing.Size(36, 33);
            this.ultraBtnAssignAll.TabIndex = 2;
            this.ultraBtnAssignAll.Text = ">>";
            this.ultraBtnAssignAll.Click += new System.EventHandler(this.ultraBtnAssignAll_Click);
            // 
            // ultraBtnUnassignAll
            // 
            this.ultraBtnUnassignAll.Location = new System.Drawing.Point(17, 232);
            this.ultraBtnUnassignAll.Name = "ultraBtnUnassignAll";
            this.ultraBtnUnassignAll.Size = new System.Drawing.Size(36, 33);
            this.ultraBtnUnassignAll.TabIndex = 1;
            this.ultraBtnUnassignAll.Text = "<<";
            this.ultraBtnUnassignAll.Click += new System.EventHandler(this.ultraBtnUnassignAll_Click);
            // 
            // ultraBtnUnassignSelected
            // 
            this.ultraBtnUnassignSelected.Location = new System.Drawing.Point(17, 193);
            this.ultraBtnUnassignSelected.Name = "ultraBtnUnassignSelected";
            this.ultraBtnUnassignSelected.Size = new System.Drawing.Size(36, 33);
            this.ultraBtnUnassignSelected.TabIndex = 0;
            this.ultraBtnUnassignSelected.Text = "<";
            this.ultraBtnUnassignSelected.Click += new System.EventHandler(this.ultraBtnUnassignSelected_Click);
            // 
            // ultraGrpUnassignedRules
            // 
            this.ultraGrpUnassignedRules.Controls.Add(this.ultraLstUnAssigned);
            this.ultraGrpUnassignedRules.Controls.Add(this.ultraSplitter3);
            this.ultraGrpUnassignedRules.Controls.Add(this.ultraTxtSearchUnassigned);
            this.ultraGrpUnassignedRules.Location = new System.Drawing.Point(69, 40);
            this.ultraGrpUnassignedRules.Name = "ultraGrpUnassignedRules";
            this.ultraGrpUnassignedRules.Size = new System.Drawing.Size(209, 358);
            this.ultraGrpUnassignedRules.TabIndex = 2;
            this.ultraGrpUnassignedRules.Text = "Un Assigned Rules";
            // 
            // ultraLstUnAssigned
            // 
            this.ultraLstUnAssigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLstUnAssigned.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance1.Image = global::Prana.ComplianceEngine.Properties.Resources.Category;
            this.ultraLstUnAssigned.ItemSettings.Appearance = appearance1;
            this.ultraLstUnAssigned.ItemSettings.HideSelection = false;
            this.ultraLstUnAssigned.Location = new System.Drawing.Point(3, 43);
            this.ultraLstUnAssigned.Name = "ultraLstUnAssigned";
            this.ultraLstUnAssigned.Size = new System.Drawing.Size(203, 312);
            this.ultraLstUnAssigned.TabIndex = 9;
            this.ultraLstUnAssigned.Text = "ultraListView3";
            this.ultraLstUnAssigned.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraLstUnAssigned.ViewSettingsList.ImageList = this.imageList1;
            this.ultraLstUnAssigned.ViewSettingsList.MultiColumn = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Category.ico");
            // 
            // ultraSplitter3
            // 
            this.ultraSplitter3.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter3.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter3.Location = new System.Drawing.Point(3, 37);
            this.ultraSplitter3.Name = "ultraSplitter3";
            this.ultraSplitter3.RestoreExtent = 21;
            this.ultraSplitter3.Size = new System.Drawing.Size(203, 6);
            this.ultraSplitter3.TabIndex = 8;
            // 
            // ultraTxtSearchUnassigned
            // 
            this.ultraTxtSearchUnassigned.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraTxtSearchUnassigned.Location = new System.Drawing.Point(3, 16);
            this.ultraTxtSearchUnassigned.Name = "ultraTxtSearchUnassigned";
            this.ultraTxtSearchUnassigned.Size = new System.Drawing.Size(203, 21);
            this.ultraTxtSearchUnassigned.TabIndex = 7;
            this.ultraTxtSearchUnassigned.Text = "Search";
            this.ultraTxtSearchUnassigned.Visible = false;
            // 
            // ultraGrpAssignedRules
            // 
            this.ultraGrpAssignedRules.Controls.Add(this.ultraLstAssignedRules);
            this.ultraGrpAssignedRules.Controls.Add(this.ultraSplitter2);
            this.ultraGrpAssignedRules.Controls.Add(this.ultraTxtSearchAssigned);
            this.ultraGrpAssignedRules.Location = new System.Drawing.Point(844, 40);
            this.ultraGrpAssignedRules.Name = "ultraGrpAssignedRules";
            this.ultraGrpAssignedRules.Size = new System.Drawing.Size(209, 358);
            this.ultraGrpAssignedRules.TabIndex = 1;
            this.ultraGrpAssignedRules.Text = "Assigned Rules";
            // 
            // ultraLstAssignedRules
            // 
            this.ultraLstAssignedRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLstAssignedRules.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance2.Image = global::Prana.ComplianceEngine.Properties.Resources.Category;
            this.ultraLstAssignedRules.ItemSettings.Appearance = appearance2;
            this.ultraLstAssignedRules.ItemSettings.HideSelection = false;
            this.ultraLstAssignedRules.Location = new System.Drawing.Point(3, 43);
            this.ultraLstAssignedRules.Name = "ultraLstAssignedRules";
            this.ultraLstAssignedRules.Size = new System.Drawing.Size(203, 312);
            this.ultraLstAssignedRules.TabIndex = 6;
            this.ultraLstAssignedRules.Text = "ultraListView2";
            this.ultraLstAssignedRules.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraLstAssignedRules.ViewSettingsList.ImageList = this.imageList1;
            this.ultraLstAssignedRules.ViewSettingsList.MultiColumn = false;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter2.Location = new System.Drawing.Point(3, 37);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 21;
            this.ultraSplitter2.Size = new System.Drawing.Size(203, 6);
            this.ultraSplitter2.TabIndex = 5;
            // 
            // ultraTxtSearchAssigned
            // 
            this.ultraTxtSearchAssigned.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraTxtSearchAssigned.Location = new System.Drawing.Point(3, 16);
            this.ultraTxtSearchAssigned.Name = "ultraTxtSearchAssigned";
            this.ultraTxtSearchAssigned.Size = new System.Drawing.Size(203, 21);
            this.ultraTxtSearchAssigned.TabIndex = 4;
            this.ultraTxtSearchAssigned.Text = "Search";
            this.ultraTxtSearchAssigned.Visible = false;
            // 
            // ultraGrpRuleGroups
            // 
            this.ultraGrpRuleGroups.Controls.Add(this.ultraLstGroup);
            this.ultraGrpRuleGroups.Controls.Add(this.ultraSplitter1);
            this.ultraGrpRuleGroups.Controls.Add(this.ultraTxtSearchGroup);
            this.ultraGrpRuleGroups.Location = new System.Drawing.Point(546, 40);
            this.ultraGrpRuleGroups.Name = "ultraGrpRuleGroups";
            this.ultraGrpRuleGroups.Size = new System.Drawing.Size(209, 358);
            this.ultraGrpRuleGroups.TabIndex = 0;
            this.ultraGrpRuleGroups.Text = "Rule Groups";
            // 
            // ultraLstGroup
            // 
            this.ultraLstGroup.ContextMenuStrip = this.cntxtMnuGroups;
            this.ultraLstGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLstGroup.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.True;
            appearance3.Image = global::Prana.ComplianceEngine.Properties.Resources.Category;
            this.ultraLstGroup.ItemSettings.Appearance = appearance3;
            this.ultraLstGroup.ItemSettings.HideSelection = false;
            this.ultraLstGroup.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.ultraLstGroup.Location = new System.Drawing.Point(3, 43);
            this.ultraLstGroup.Name = "ultraLstGroup";
            this.ultraLstGroup.Size = new System.Drawing.Size(203, 312);
            this.ultraLstGroup.TabIndex = 3;
            this.ultraLstGroup.Text = "ultraListView1";
            this.ultraLstGroup.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraLstGroup.ViewSettingsList.ImageList = this.imageList1;
            this.ultraLstGroup.ViewSettingsList.MultiColumn = false;
            this.ultraLstGroup.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.ultraLstGroup_ItemActivated);
            this.ultraLstGroup.ItemDoubleClick += new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.ultraLstGroup_ItemDoubleClick);
            this.ultraLstGroup.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraLstGroup_ItemSelectionChanged);
            this.ultraLstGroup.ItemEnteredEditMode += new Infragistics.Win.UltraWinListView.ItemEnteredEditModeEventHandler(this.ultraLstGroup_ItemEnteredEditMode);
            this.ultraLstGroup.ItemExitedEditMode += new Infragistics.Win.UltraWinListView.ItemExitedEditModeEventHandler(this.ultraLstGroup_ItemExitedEditMode);
            this.ultraLstGroup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraLstGroup_MouseUp);
            // 
            // cntxtMnuGroups
            // 
            this.cntxtMnuGroups.Name = "cntxtMnuGroups";
            this.cntxtMnuGroups.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cntxtMnuGroups.Size = new System.Drawing.Size(61, 4);
            this.cntxtMnuGroups.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cntxtMnuGroups_ItemClicked);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(3, 37);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 21;
            this.ultraSplitter1.Size = new System.Drawing.Size(203, 6);
            this.ultraSplitter1.TabIndex = 2;
            // 
            // ultraTxtSearchGroup
            // 
            this.ultraTxtSearchGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraTxtSearchGroup.Location = new System.Drawing.Point(3, 16);
            this.ultraTxtSearchGroup.Name = "ultraTxtSearchGroup";
            this.ultraTxtSearchGroup.Size = new System.Drawing.Size(203, 21);
            this.ultraTxtSearchGroup.TabIndex = 1;
            this.ultraTxtSearchGroup.Text = "Search";
            this.ultraTxtSearchGroup.Visible = false;
            // 
            // RuleGroupingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPnlMain);
            this.Name = "RuleGroupingControl";
            this.Size = new System.Drawing.Size(1131, 438);
            this.Load += new System.EventHandler(this.RuleGroupingControl_Load);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpButtons)).EndInit();
            this.ultraGrpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpUnassignedRules)).EndInit();
            this.ultraGrpUnassignedRules.ResumeLayout(false);
            this.ultraGrpUnassignedRules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstUnAssigned)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchUnassigned)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpAssignedRules)).EndInit();
            this.ultraGrpAssignedRules.ResumeLayout(false);
            this.ultraGrpAssignedRules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstAssignedRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchAssigned)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpRuleGroups)).EndInit();
            this.ultraGrpRuleGroups.ResumeLayout(false);
            this.ultraGrpRuleGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtSearchGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraGroupBox ultraGrpButtons;
        private Infragistics.Win.Misc.UltraButton ultraBtnAssignSelected;
        private Infragistics.Win.Misc.UltraButton ultraBtnAssignAll;
        private Infragistics.Win.Misc.UltraButton ultraBtnUnassignAll;
        private Infragistics.Win.Misc.UltraButton ultraBtnUnassignSelected;
        private Infragistics.Win.Misc.UltraGroupBox ultraGrpUnassignedRules;
        private Infragistics.Win.UltraWinListView.UltraListView ultraLstUnAssigned;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtSearchUnassigned;
        private Infragistics.Win.Misc.UltraGroupBox ultraGrpAssignedRules;
        private Infragistics.Win.UltraWinListView.UltraListView ultraLstAssignedRules;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtSearchAssigned;
        private Infragistics.Win.Misc.UltraGroupBox ultraGrpRuleGroups;
        private Infragistics.Win.UltraWinListView.UltraListView ultraLstGroup;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtSearchGroup;
        private System.Windows.Forms.ContextMenuStrip cntxtMnuGroups;
        private System.Windows.Forms.ImageList imageList1;
    }
}
