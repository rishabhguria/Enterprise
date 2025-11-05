namespace Prana.CashManagement.Controls
{
    partial class ctrlActivityExceptions
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
                if (time != null)
                {
                    time.Dispose();
                }
                if (_activityExceptionsLayout != null)
                {
                    _activityExceptionsLayout.Dispose();
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.generateJournalExceptions = new Infragistics.Win.Misc.UltraButton();
            this.btnOverriding = new Infragistics.Win.Misc.UltraButton();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGetEx = new Infragistics.Win.Misc.UltraButton();
            this.grdActivityExceptions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ugbxActivityExceptions = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugbxActivityExcepParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityExceptions)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityExceptions)).BeginInit();
            this.ugbxActivityExceptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityExcepParams)).BeginInit();
            this.ugbxActivityExcepParams.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 436);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1332, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // generateJournalExceptions
            // 
            appearance1.FontData.SizeInPoints = 9F;
            this.generateJournalExceptions.Appearance = appearance1;
            this.generateJournalExceptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateJournalExceptions.Location = new System.Drawing.Point(890, 12);
            this.generateJournalExceptions.Name = "generateJournalExceptions";
            this.generateJournalExceptions.Size = new System.Drawing.Size(207, 24);
            this.generateJournalExceptions.TabIndex = 12;
            this.generateJournalExceptions.Text = "Generate Journal Exception";
            this.generateJournalExceptions.Click += new System.EventHandler(this.generateJournalExceptions_Click_1);
            // 
            // btnOverriding
            // 
            appearance2.FontData.SizeInPoints = 9F;
            this.btnOverriding.Appearance = appearance2;
            this.btnOverriding.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverriding.Location = new System.Drawing.Point(1102, 12);
            this.btnOverriding.Name = "btnOverriding";
            this.btnOverriding.Size = new System.Drawing.Size(154, 24);
            this.btnOverriding.TabIndex = 10;
            this.btnOverriding.Text = "Get Overriding Data";
            this.btnOverriding.Click += new System.EventHandler(this.btnOverriding_Click);
            // 
            // lblToDate
            // 
            appearance3.FontData.SizeInPoints = 9F;
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Middle";
            this.lblToDate.Appearance = appearance3;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(632, 12);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(23, 23);
            this.lblToDate.TabIndex = 8;
            this.lblToDate.Text = "To";
            // 
            // dtToDate
            // 
            this.dtToDate.AutoSize = false;
            this.dtToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDate.Location = new System.Drawing.Point(655, 12);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(105, 23);
            this.dtToDate.TabIndex = 2;
            // 
            // lblFromDate
            // 
            appearance4.FontData.SizeInPoints = 9F;
            appearance4.TextHAlignAsString = "Left";
            appearance4.TextVAlignAsString = "Middle";
            this.lblFromDate.Appearance = appearance4;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(483, 12);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(40, 23);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From";
            // 
            // dtFromDate
            // 
            this.dtFromDate.AutoSize = false;
            this.dtFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromDate.Location = new System.Drawing.Point(523, 12);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(109, 23);
            this.dtFromDate.TabIndex = 1;
            // 
            // btnSave
            // 
            appearance5.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance5;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1261, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 24);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetEx
            // 
            appearance6.FontData.SizeInPoints = 9F;
            this.btnGetEx.Appearance = appearance6;
            this.btnGetEx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetEx.Location = new System.Drawing.Point(765, 12);
            this.btnGetEx.Name = "btnGetEx";
            this.btnGetEx.Size = new System.Drawing.Size(120, 24);
            this.btnGetEx.TabIndex = 3;
            this.btnGetEx.Text = "Get Exceptions";
            this.btnGetEx.Click += new System.EventHandler(this.btnGetEx_Click);
            // 
            // grdActivityExceptions
            // 
            this.grdActivityExceptions.ContextMenuStrip = this.contextMenuStrip1;
            appearance7.BackColor = System.Drawing.Color.Black;
            this.grdActivityExceptions.DisplayLayout.Appearance = appearance7;
            this.grdActivityExceptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdActivityExceptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivityExceptions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityExceptions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdActivityExceptions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdActivityExceptions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdActivityExceptions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivityExceptions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityExceptions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityExceptions.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivityExceptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdActivityExceptions.DisplayLayout.Override.CellPadding = 0;
            this.grdActivityExceptions.DisplayLayout.Override.CellSpacing = 0;
            appearance8.FontData.Name = "Segoe UI";
            appearance8.FontData.SizeInPoints = 8F;
            appearance8.TextHAlignAsString = "Center";
            this.grdActivityExceptions.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdActivityExceptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdActivityExceptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdActivityExceptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdActivityExceptions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.FontData.BoldAsString = "True";
            this.grdActivityExceptions.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdActivityExceptions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivityExceptions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivityExceptions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdActivityExceptions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdActivityExceptions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdActivityExceptions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdActivityExceptions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdActivityExceptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance10;
            this.grdActivityExceptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdActivityExceptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdActivityExceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdActivityExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActivityExceptions.ExitEditModeOnLeave = false;
            this.grdActivityExceptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdActivityExceptions.Location = new System.Drawing.Point(3, 3);
            this.grdActivityExceptions.Name = "grdActivityExceptions";
            this.grdActivityExceptions.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdActivityExceptions.Size = new System.Drawing.Size(1326, 383);
            this.grdActivityExceptions.TabIndex = 2;
            this.grdActivityExceptions.Text = "Cash Journal";
            this.grdActivityExceptions.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdActivityExceptions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdActivityExceptions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCashExceptions_InitializeLayout);
            this.grdActivityExceptions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdCashExceptions_InitializeGroupByRow);
            this.grdActivityExceptions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdCashExceptions_AfterSortChange);
            this.grdActivityExceptions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdActivityExceptions_BeforeCustomRowFilterDialog);
            this.grdActivityExceptions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdActivityExceptions_BeforeColumnChooserDisplayed);
            this.grdActivityExceptions.BeforeRowFilterDropDown += grdActivityExceptions_BeforeRowFilterDropDown;
            this.grdActivityExceptions.AfterRowFilterChanged += grdActivityExceptions_AfterRowFilterChanged;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 48);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // ugbxActivityExceptions
            // 
            this.ugbxActivityExceptions.Controls.Add(this.grdActivityExceptions);
            this.ugbxActivityExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxActivityExceptions.Location = new System.Drawing.Point(0, 47);
            this.ugbxActivityExceptions.Name = "ugbxActivityExceptions";
            this.ugbxActivityExceptions.Size = new System.Drawing.Size(1332, 389);
            this.ugbxActivityExceptions.TabIndex = 3;
            // 
            // ugbxActivityExcepParams
            // 
            this.ugbxActivityExcepParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxActivityExcepParams.Controls.Add(this.generateJournalExceptions);
            this.ugbxActivityExcepParams.Controls.Add(this.dtToDate);
            this.ugbxActivityExcepParams.Controls.Add(this.lblFromDate);
            this.ugbxActivityExcepParams.Controls.Add(this.btnGetEx);
            this.ugbxActivityExcepParams.Controls.Add(this.lblToDate);
            this.ugbxActivityExcepParams.Controls.Add(this.dtFromDate);
            this.ugbxActivityExcepParams.Controls.Add(this.btnSave);
            this.ugbxActivityExcepParams.Controls.Add(this.btnOverriding);
            this.ugbxActivityExcepParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxActivityExcepParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxActivityExcepParams.Name = "ugbxActivityExcepParams";
            this.ugbxActivityExcepParams.Size = new System.Drawing.Size(1332, 47);
            this.ugbxActivityExcepParams.TabIndex = 4;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(3, 2);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(480, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 13;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxActivityExceptions);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxActivityExcepParams);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1332, 458);
            this.ultraPanel1.TabIndex = 103;
            // 
            // ctrlActivityExceptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlActivityExceptions";
            this.Size = new System.Drawing.Size(1332, 458);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlActivityExceptions_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityExceptions)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityExceptions)).EndInit();
            this.ugbxActivityExceptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxActivityExcepParams)).EndInit();
            this.ugbxActivityExcepParams.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnGetEx;
        private Infragistics.Win.Misc.UltraLabel lblToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.Misc.UltraButton btnOverriding;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraButton generateJournalExceptions;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdActivityExceptions;
        private Infragistics.Win.Misc.UltraGroupBox ugbxActivityExceptions;
        private Infragistics.Win.Misc.UltraGroupBox ugbxActivityExcepParams;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;


    }
}
