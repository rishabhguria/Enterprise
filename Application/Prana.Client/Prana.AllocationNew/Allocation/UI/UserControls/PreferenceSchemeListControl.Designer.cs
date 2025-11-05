namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    partial class PreferenceSchemeListControl
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Position");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbarSchemes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnAdd");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnDelete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnCopy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnBulkChanges");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnImport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnExportAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnAdd");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnDelete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnCopy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnBulkChanges");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnImport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BtnExportAll");
            this.ultraPnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.ClientArea_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLstPrefSchemes = new Infragistics.Win.UltraWinListView.UltraListView();
            this.cntxtMnuPreference = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renamePreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllPreferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraPnlMain.ClientArea.SuspendLayout();
            this.ultraPnlMain.SuspendLayout();
            this.ClientArea_Fill_Panel.ClientArea.SuspendLayout();
            this.ClientArea_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstPrefSchemes)).BeginInit();
            this.cntxtMnuPreference.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPnlMain
            // 
            // 
            // ultraPnlMain.ClientArea
            // 
            this.ultraPnlMain.ClientArea.Controls.Add(this.ClientArea_Fill_Panel);
            this.ultraPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPnlMain.Location = new System.Drawing.Point(0, 0);
            this.ultraPnlMain.Name = "ultraPnlMain";
            this.ultraPnlMain.Size = new System.Drawing.Size(290, 428);
            this.ultraPnlMain.TabIndex = 0;
            // 
            // ClientArea_Fill_Panel
            // 
            // 
            // ClientArea_Fill_Panel.ClientArea
            // 
            this.ClientArea_Fill_Panel.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ClientArea_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ClientArea_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientArea_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.ClientArea_Fill_Panel.Name = "ClientArea_Fill_Panel";
            this.ClientArea_Fill_Panel.Size = new System.Drawing.Size(290, 428);
            this.ClientArea_Fill_Panel.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraLstPrefSchemes);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(290, 428);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Allocation Preferences";
            // 
            // ultraLstPrefSchemes
            // 
            this.ultraLstPrefSchemes.ContextMenuStrip = this.cntxtMnuPreference;
            this.ultraLstPrefSchemes.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.BackColor = System.Drawing.SystemColors.Highlight;
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLstPrefSchemes.ItemSettings.ActiveAppearance = appearance1;
            this.ultraLstPrefSchemes.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.True;
            appearance2.FontData.BoldAsString = "False";
            this.ultraLstPrefSchemes.ItemSettings.Appearance = appearance2;
            this.ultraLstPrefSchemes.ItemSettings.HideSelection = false;
            this.ultraLstPrefSchemes.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.ultraLstPrefSchemes.Location = new System.Drawing.Point(3, 16);
            this.ultraLstPrefSchemes.MainColumn.Key = "OprtnPrefSchemes";
            this.ultraLstPrefSchemes.MainColumn.Text = "Allocation Operation Preference Schemes";
            this.ultraLstPrefSchemes.Name = "ultraLstPrefSchemes";
            this.ultraLstPrefSchemes.Size = new System.Drawing.Size(284, 409);
            ultraListViewSubItemColumn1.AllowSorting = Infragistics.Win.DefaultableBoolean.True;
            ultraListViewSubItemColumn1.Key = "Position";
            ultraListViewSubItemColumn1.Sorting = Infragistics.Win.UltraWinListView.Sorting.Ascending;
            ultraListViewSubItemColumn1.Text = "Position";
            this.ultraLstPrefSchemes.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1});
            this.ultraLstPrefSchemes.TabIndex = 0;
            this.ultraLstPrefSchemes.Text = "ultraListView1";
            this.ultraLstPrefSchemes.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraLstPrefSchemes.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.ultraLstPrefSchemes.ViewSettingsList.MultiColumn = false;
            this.ultraLstPrefSchemes.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.ultraLstPrefSchemes_ItemActivated);
            this.ultraLstPrefSchemes.ItemExitedEditMode += new Infragistics.Win.UltraWinListView.ItemExitedEditModeEventHandler(this.ultraLstPrefSchemes_ItemExitedEditMode);
            this.ultraLstPrefSchemes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraLstPrefSchemes_MouseUp);
            // 
            // cntxtMnuPreference
            // 
            this.cntxtMnuPreference.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPreferenceToolStripMenuItem,
            this.deletePreferenceToolStripMenuItem,
            this.copyPreferenceToolStripMenuItem,
            this.renamePreferenceToolStripMenuItem,
            this.importPreferenceToolStripMenuItem,
            this.exportPreferenceToolStripMenuItem,
            this.exportAllPreferenceToolStripMenuItem});
            this.cntxtMnuPreference.Name = "cntxtMnuPreference";
            this.cntxtMnuPreference.Size = new System.Drawing.Size(184, 158);
            this.cntxtMnuPreference.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cntxtMnuPreference_ItemClicked);
            // 
            // addPreferenceToolStripMenuItem
            // 
            this.addPreferenceToolStripMenuItem.Name = "addPreferenceToolStripMenuItem";
            this.addPreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addPreferenceToolStripMenuItem.Tag = "AddPreference";
            this.addPreferenceToolStripMenuItem.Text = "Add Preference";
            // 
            // deletePreferenceToolStripMenuItem
            // 
            this.deletePreferenceToolStripMenuItem.Name = "deletePreferenceToolStripMenuItem";
            this.deletePreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.deletePreferenceToolStripMenuItem.Tag = "DeletePreference";
            this.deletePreferenceToolStripMenuItem.Text = "Delete Preference";
            // 
            // copyPreferenceToolStripMenuItem
            // 
            this.copyPreferenceToolStripMenuItem.Name = "copyPreferenceToolStripMenuItem";
            this.copyPreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.copyPreferenceToolStripMenuItem.Tag = "CopyPreference";
            this.copyPreferenceToolStripMenuItem.Text = "Copy Preference";
            // 
            // renamePreferenceToolStripMenuItem
            // 
            this.renamePreferenceToolStripMenuItem.Name = "renamePreferenceToolStripMenuItem";
            this.renamePreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.renamePreferenceToolStripMenuItem.Tag = "RenamePreference";
            this.renamePreferenceToolStripMenuItem.Text = "Rename Preference";
            // 
            // importPreferenceToolStripMenuItem
            // 
            this.importPreferenceToolStripMenuItem.Name = "importPreferenceToolStripMenuItem";
            this.importPreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.importPreferenceToolStripMenuItem.Tag = "ImportPreference";
            this.importPreferenceToolStripMenuItem.Text = "Import Preference";
            // 
            // exportPreferenceToolStripMenuItem
            // 
            this.exportPreferenceToolStripMenuItem.Name = "exportPreferenceToolStripMenuItem";
            this.exportPreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.exportPreferenceToolStripMenuItem.Tag = "ExportPreference";
            this.exportPreferenceToolStripMenuItem.Text = "Export Preference";
            // 
            // exportAllPreferenceToolStripMenuItem
            // 
            this.exportAllPreferenceToolStripMenuItem.Name = "exportAllPreferenceToolStripMenuItem";
            this.exportAllPreferenceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.exportAllPreferenceToolStripMenuItem.Tag = "ExportAllPreference";
            this.exportAllPreferenceToolStripMenuItem.Text = "Export All Preference";
            // 
            // _PreferenceSchemeListControl_Toolbars_Dock_Area_Left
            // 
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.Name = "_PreferenceSchemeListControl_Toolbars_Dock_Area_Left";
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 428);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool7,
            buttonTool9,
            buttonTool11});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Text = "UltraToolbarSchemes";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool4.SharedPropsInternal.Caption = "Add";
            buttonTool4.SharedPropsInternal.CustomizerCaption = "Add";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool5.SharedPropsInternal.Caption = "Delete";
            buttonTool5.SharedPropsInternal.CustomizerCaption = "Delete";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool6.SharedPropsInternal.Caption = "Copy";
            buttonTool6.SharedPropsInternal.CustomizerCaption = "Copy";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool8.SharedPropsInternal.Caption = "Bulk Change";
            buttonTool8.SharedPropsInternal.CustomizerCaption = "Bulk Change";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool10.SharedPropsInternal.Caption = "Import";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool12.SharedPropsInternal.Caption = "Export All";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool8,
            buttonTool10,
            buttonTool12});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _PreferenceSchemeListControl_Toolbars_Dock_Area_Right
            // 
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(290, 0);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.Name = "_PreferenceSchemeListControl_Toolbars_Dock_Area_Right";
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 428);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _PreferenceSchemeListControl_Toolbars_Dock_Area_Top
            // 
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.Name = "_PreferenceSchemeListControl_Toolbars_Dock_Area_Top";
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(290, 0);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom
            // 
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 428);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.Name = "_PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom";
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(290, 25);
            this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // PreferenceSchemeListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.ultraPnlMain);
            this.Controls.Add(this._PreferenceSchemeListControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._PreferenceSchemeListControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._PreferenceSchemeListControl_Toolbars_Dock_Area_Top);
            this.Name = "PreferenceSchemeListControl";
            this.Size = new System.Drawing.Size(290, 453);
            this.ultraPnlMain.ClientArea.ResumeLayout(false);
            this.ultraPnlMain.ResumeLayout(false);
            this.ClientArea_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ClientArea_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraLstPrefSchemes)).EndInit();
            this.cntxtMnuPreference.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPnlMain;
        private Infragistics.Win.Misc.UltraPanel ClientArea_Fill_Panel;
        private Infragistics.Win.UltraWinListView.UltraListView ultraLstPrefSchemes;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PreferenceSchemeListControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PreferenceSchemeListControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PreferenceSchemeListControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PreferenceSchemeListControl_Toolbars_Dock_Area_Top;
        private System.Windows.Forms.ContextMenuStrip cntxtMnuPreference;
        private System.Windows.Forms.ToolStripMenuItem addPreferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePreferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPreferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renamePreferenceToolStripMenuItem;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.ToolStripMenuItem importPreferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPreferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllPreferenceToolStripMenuItem;
    }
}
