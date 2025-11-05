using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.CashManagement.Controls
{
    partial class ctrlEditableActivity
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
                if (_activityLayout != null)
                {
                    _activityLayout.Dispose();
                }
                if (_proxy != null)
                {
                    _proxy.Dispose();
                    _proxy = null;
                }
                if (_vlTransactionSource != null)
                {
                    _vlTransactionSource.Dispose();
                    _vlTransactionSource = null;
                }
                if (_vlCurrencies != null)
                {
                    _vlCurrencies.Dispose();
                    _vlCurrencies = null;
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripActivity = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraOSDateSelection = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnGetActivities = new Infragistics.Win.Misc.UltraButton();
            this.dtPickerUpper = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.dtPickerlower = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.mnsActivity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.grdActivity = new PranaUltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ugbxActivityDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugbxActivityParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOSDateSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).BeginInit();
            this.mnsActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdActivity)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityDetails)).BeginInit();
            this.ugbxActivityDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityParams)).BeginInit();
            this.ugbxActivityParams.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripActivity});
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1271, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripActivity
            // 
            this.toolStripActivity.Name = "toolStripActivity";
            this.toolStripActivity.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraOSDateSelection
            // 
            appearance1.FontData.SizeInPoints = 9F;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraOSDateSelection.Appearance = appearance1;
            this.ultraOSDateSelection.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            this.ultraOSDateSelection.CheckedIndex = 1;
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            valueListItem1.Appearance = appearance2;
            valueListItem1.DataValue = "settlementDate";
            valueListItem1.DisplayText = "Settlement Date";
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Middle";
            valueListItem2.Appearance = appearance3;
            valueListItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem2.DataValue = "tradeDate";
            valueListItem2.DisplayText = "Trade Date";
            valueListItem3.DataValue = "bothdates";
            valueListItem3.DisplayText = "All";
            this.ultraOSDateSelection.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.ultraOSDateSelection.ItemSpacingHorizontal = 5;
            this.ultraOSDateSelection.Location = new System.Drawing.Point(801, 12);
            this.ultraOSDateSelection.Name = "ultraOSDateSelection";
            this.ultraOSDateSelection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraOSDateSelection.Size = new System.Drawing.Size(255, 23);
            this.ultraOSDateSelection.TabIndex = 105;
            this.ultraOSDateSelection.Text = "Trade Date";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(1190, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(68, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnGetActivities
            // 
            this.btnGetActivities.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetActivities.Location = new System.Drawing.Point(1070, 12);
            this.btnGetActivities.Name = "btnGetActivities";
            this.btnGetActivities.Size = new System.Drawing.Size(111, 23);
            this.btnGetActivities.TabIndex = 2;
            this.btnGetActivities.Text = "Get Activities";
            this.btnGetActivities.Click += new System.EventHandler(this.btnGetActivities_Click);
            // 
            // dtPickerUpper
            // 
            appearance4.FontData.SizeInPoints = 9F;
            this.dtPickerUpper.Appearance = appearance4;
            this.dtPickerUpper.AutoSize = false;
            this.dtPickerUpper.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerUpper.Location = new System.Drawing.Point(673, 12);
            this.dtPickerUpper.Name = "dtPickerUpper";
            this.dtPickerUpper.Size = new System.Drawing.Size(109, 23);
            this.dtPickerUpper.TabIndex = 1;
            // 
            // lblTo
            // 
            appearance5.TextHAlignAsString = "Left";
            appearance5.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance5;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(646, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 98;
            this.lblTo.Text = "To";
            // 
            // dtPickerlower
            // 
            appearance6.FontData.SizeInPoints = 9F;
            this.dtPickerlower.Appearance = appearance6;
            this.dtPickerlower.AutoSize = false;
            this.dtPickerlower.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerlower.Location = new System.Drawing.Point(530, 12);
            this.dtPickerlower.Name = "dtPickerlower";
            this.dtPickerlower.Size = new System.Drawing.Size(109, 23);
            this.dtPickerlower.TabIndex = 0;
            // 
            // lblFrom
            // 
            appearance7.TextHAlignAsString = "Left";
            appearance7.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance7;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(490, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(34, 23);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From";
            // 
            // mnsActivity
            // 
            this.mnsActivity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.mnsActivity.Name = "mnsActivity";
            this.mnsActivity.Size = new System.Drawing.Size(108, 48);
            this.inboxControlStyler1.SetStyleSettings(this.mnsActivity, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // grdActivity
            // 
            this.grdActivity.ContextMenuStrip = this.contextMenuStrip1;
            appearance8.BackColor = System.Drawing.Color.Black;
            this.grdActivity.DisplayLayout.Appearance = appearance8;
            this.grdActivity.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdActivity.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivity.DisplayLayout.MaxColScrollRegions = 1;
            this.grdActivity.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdActivity.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdActivity.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivity.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivity.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivity.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdActivity.DisplayLayout.Override.CellPadding = 0;
            this.grdActivity.DisplayLayout.Override.CellSpacing = 0;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.ForeColor = System.Drawing.Color.White;
            this.grdActivity.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.FontData.Name = "Segoe UI";
            appearance10.FontData.SizeInPoints = 9F;
            appearance10.TextHAlignAsString = "Center";
            this.grdActivity.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdActivity.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdActivity.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.Black;
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.TextHAlignAsString = "Right";
            appearance11.TextVAlignAsString = "Middle";
            this.grdActivity.DisplayLayout.Override.RowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.FontData.BoldAsString = "True";
            this.grdActivity.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.grdActivity.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivity.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivity.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdActivity.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivity.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdActivity.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdActivity.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdActivity.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.grdActivity.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdActivity.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdActivity.DisplayLayout.UseFixedHeaders = true;
            this.grdActivity.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActivity.ExitEditModeOnLeave = false;
            this.grdActivity.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdActivity.Location = new System.Drawing.Point(3, 3);
            this.grdActivity.Name = "grdActivity";
            this.grdActivity.Size = new System.Drawing.Size(1265, 397);
            this.grdActivity.TabIndex = 1;
            this.grdActivity.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdActivity.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivity.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdActivity_AfterCellUpdate);
            this.grdActivity.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdActivity_InitializeLayout);
            this.grdActivity.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdActivity_InitializeRow);
            this.grdActivity.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdActivity_InitializeGroupByRow);
            this.grdActivity.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdActivity_CellChange);
            this.grdActivity.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdActivity_AfterSortChange);
            this.grdActivity.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdActivity_BeforeCustomRowFilterDialog);
            this.grdActivity.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdActivity_BeforeColumnChooserDisplayed);
            this.grdActivity.BeforeRowFilterDropDown += grdActivity_BeforeRowFilterDropDown;
            this.grdActivity.AfterRowFilterChanged += grdActivity_AfterRowFilterChanged;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // ugbxActivityDetails
            // 
            this.ugbxActivityDetails.Controls.Add(this.grdActivity);
            this.ugbxActivityDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxActivityDetails.Location = new System.Drawing.Point(0, 47);
            this.ugbxActivityDetails.Name = "ugbxActivityDetails";
            this.ugbxActivityDetails.Size = new System.Drawing.Size(1271, 403);
            this.ugbxActivityDetails.TabIndex = 13;
            // 
            // ugbxActivityParams
            // 
            this.ugbxActivityParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxActivityParams.Controls.Add(this.lblFrom);
            this.ugbxActivityParams.Controls.Add(this.ultraOSDateSelection);
            this.ugbxActivityParams.Controls.Add(this.btnExport);
            this.ugbxActivityParams.Controls.Add(this.dtPickerlower);
            this.ugbxActivityParams.Controls.Add(this.btnGetActivities);
            this.ugbxActivityParams.Controls.Add(this.dtPickerUpper);
            this.ugbxActivityParams.Controls.Add(this.lblTo);
            this.ugbxActivityParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxActivityParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxActivityParams.Name = "ugbxActivityParams";
            this.ugbxActivityParams.Size = new System.Drawing.Size(1271, 47);
            this.ugbxActivityParams.TabIndex = 14;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(3, 2);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(481, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 106;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxActivityDetails);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxActivityParams);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1271, 472);
            this.ultraPanel1.TabIndex = 106;
            // 
            // ctrlEditableActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlEditableActivity";
            this.Size = new System.Drawing.Size(1271, 472);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlEditableActivity_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOSDateSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).EndInit();
            this.mnsActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdActivity)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityDetails)).EndInit();
            this.ugbxActivityDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityParams)).EndInit();
            this.ugbxActivityParams.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnGetActivities;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerUpper;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerlower;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private System.Windows.Forms.ContextMenuStrip mnsActivity;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripActivity;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOSDateSelection;
        private PranaUltraGrid grdActivity;
        private Infragistics.Win.Misc.UltraGroupBox ugbxActivityDetails;
        private Infragistics.Win.Misc.UltraGroupBox ugbxActivityParams;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
    }
}
