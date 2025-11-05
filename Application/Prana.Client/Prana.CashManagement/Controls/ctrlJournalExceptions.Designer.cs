namespace Prana.CashManagement.Controls
{
    partial class ctrlJournalExceptions
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
                if (_columnSorted != null)
                {
                    _columnSorted.Dispose();
                }
                if (_journalExceptionsLayout != null)
                {
                    _journalExceptionsLayout.Dispose();
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
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.grdCashExceptions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayouttToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOverriding = new Infragistics.Win.Misc.UltraButton();
            this.btnRunRevaluation = new Infragistics.Win.Misc.UltraButton();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGetEx = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ugbxJournalExceptions = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugbxJExcep = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugbxJExcepParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashExceptions)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJournalExceptions)).BeginInit();
            this.ugbxJournalExceptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJExcep)).BeginInit();
            this.ugbxJExcep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJExcepParams)).BeginInit();
            this.ugbxJExcepParams.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCashExceptions
            // 
            this.grdCashExceptions.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdCashExceptions.DisplayLayout.Appearance = appearance1;
            this.grdCashExceptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashExceptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCashExceptions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCashExceptions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCashExceptions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdCashExceptions.DisplayLayout.Override.CellPadding = 0;
            this.grdCashExceptions.DisplayLayout.Override.CellSpacing = 0;
            appearance2.FontData.Name = "Segoe UI";
            appearance2.FontData.SizeInPoints = 9F;
            appearance2.TextHAlignAsString = "Center";
            this.grdCashExceptions.DisplayLayout.Override.HeaderAppearance = appearance2;
            this.grdCashExceptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdCashExceptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdCashExceptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.FontData.BoldAsString = "True";
            this.grdCashExceptions.DisplayLayout.Override.SelectedRowAppearance = appearance3;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdCashExceptions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdCashExceptions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdCashExceptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance4;
            this.grdCashExceptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCashExceptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCashExceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCashExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCashExceptions.ExitEditModeOnLeave = false;
            this.grdCashExceptions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCashExceptions.Location = new System.Drawing.Point(3, 3);
            this.grdCashExceptions.Name = "grdCashExceptions";
            this.grdCashExceptions.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdCashExceptions.Size = new System.Drawing.Size(1162, 333);
            this.grdCashExceptions.TabIndex = 2;
            this.grdCashExceptions.Text = "Cash Journal";
            this.grdCashExceptions.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdCashExceptions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCashExceptions_InitializeLayout);
            this.grdCashExceptions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCashExceptions_InitializeRow);
            this.grdCashExceptions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdCashExceptions_InitializeGroupByRow);
            this.grdCashExceptions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdCashExceptions_AfterSortChange);
            this.grdCashExceptions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdCashExceptions_BeforeCustomRowFilterDialog);
            this.grdCashExceptions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdCashExceptions_BeforeColumnChooserDisplayed);
            this.grdCashExceptions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdCashExceptions_MouseClick);
            this.grdCashExceptions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdCashExceptions_MouseDown);
            this.grdCashExceptions.BeforeRowFilterDropDown += grdCashExceptions_BeforeRowFilterDropDown;
            this.grdCashExceptions.AfterRowFilterChanged += grdCashExceptions_AfterRowFilterChanged;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayouttToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 48);
            // 
            // saveLayouttToolStripMenuItem
            // 
            this.saveLayouttToolStripMenuItem.Name = "saveLayouttToolStripMenuItem";
            this.saveLayouttToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayouttToolStripMenuItem.Text = "Save Layout";
            this.saveLayouttToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // btnOverriding
            // 
            appearance5.FontData.SizeInPoints = 9F;
            this.btnOverriding.Appearance = appearance5;
            this.btnOverriding.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverriding.Location = new System.Drawing.Point(935, 12);
            this.btnOverriding.Name = "btnOverriding";
            this.btnOverriding.Size = new System.Drawing.Size(164, 24);
            this.btnOverriding.TabIndex = 10;
            this.btnOverriding.Text = "Get Overriding Data";
            this.btnOverriding.Click += new System.EventHandler(this.btnOverriding_Click);
            // 
            // btnRunRevaluation
            // 
            appearance12.FontData.SizeInPoints = 9F;
            this.btnRunRevaluation.Appearance = appearance12;
            this.btnRunRevaluation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunRevaluation.Location = new System.Drawing.Point(1107, 12);
            this.btnRunRevaluation.Name = "btnRunRevaluation";
            this.btnRunRevaluation.Size = new System.Drawing.Size(166, 24);
            this.btnRunRevaluation.TabIndex = 12;
            this.btnRunRevaluation.Text = "Run Manual Revaluation";
            this.btnRunRevaluation.Click += new System.EventHandler(this.btnRunRevaluation_Click);

            // 
            // lblToDate
            // 
            appearance6.FontData.SizeInPoints = 9F;
            appearance6.TextHAlignAsString = "Left";
            appearance6.TextVAlignAsString = "Middle";
            this.lblToDate.Appearance = appearance6;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(646, 12);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(19, 23);
            this.lblToDate.TabIndex = 8;
            this.lblToDate.Text = "To";
            // 
            // dtToDate
            // 
            appearance7.FontData.SizeInPoints = 9F;
            this.dtToDate.Appearance = appearance7;
            this.dtToDate.AutoSize = false;
            this.dtToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDate.Location = new System.Drawing.Point(683, 12);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(109, 23);
            this.dtToDate.TabIndex = 2;
            // 
            // lblFromDate
            // 
            appearance8.FontData.SizeInPoints = 9F;
            appearance8.TextHAlignAsString = "Left";
            appearance8.TextVAlignAsString = "Middle";
            this.lblFromDate.Appearance = appearance8;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(489, 12);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(34, 23);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From";
            // 
            // dtFromDate
            // 
            appearance9.FontData.SizeInPoints = 9F;
            this.dtFromDate.Appearance = appearance9;
            this.dtFromDate.AutoSize = false;
            this.dtFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromDate.Location = new System.Drawing.Point(529, 12);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(109, 23);
            this.dtFromDate.TabIndex = 1;
            // 
            // btnSave
            // 
            appearance10.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance10;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1281, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 24);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetEx
            // 
            appearance11.FontData.SizeInPoints = 9F;
            this.btnGetEx.Appearance = appearance11;
            this.btnGetEx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetEx.Location = new System.Drawing.Point(801, 12);
            this.btnGetEx.Name = "btnGetEx";
            this.btnGetEx.Size = new System.Drawing.Size(126, 24);
            this.btnGetEx.TabIndex = 3;
            this.btnGetEx.Text = "Get Exceptions";
            this.btnGetEx.Click += new System.EventHandler(this.btnGetEx_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 392);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1174, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.BackColor = System.Drawing.Color.White;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // ugbxJournalExceptions
            // 
            this.ugbxJournalExceptions.Controls.Add(this.ugbxJExcep);
            this.ugbxJournalExceptions.Controls.Add(this.ugbxJExcepParams);
            this.ugbxJournalExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxJournalExceptions.Location = new System.Drawing.Point(0, 0);
            this.ugbxJournalExceptions.Name = "ugbxJournalExceptions";
            this.ugbxJournalExceptions.Size = new System.Drawing.Size(1174, 392);
            this.ugbxJournalExceptions.TabIndex = 4;
            // 
            // ugbxJExcep
            // 
            this.ugbxJExcep.Controls.Add(this.grdCashExceptions);
            this.ugbxJExcep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxJExcep.Location = new System.Drawing.Point(3, 50);
            this.ugbxJExcep.Name = "ugbxJExcep";
            this.ugbxJExcep.Size = new System.Drawing.Size(1168, 339);
            this.ugbxJExcep.TabIndex = 104;
            // 
            // ugbxJExcepParams
            // 
            this.ugbxJExcepParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxJExcepParams.Controls.Add(this.dtToDate);
            this.ugbxJExcepParams.Controls.Add(this.lblFromDate);
            this.ugbxJExcepParams.Controls.Add(this.btnSave);
            this.ugbxJExcepParams.Controls.Add(this.lblToDate);
            this.ugbxJExcepParams.Controls.Add(this.dtFromDate);
            this.ugbxJExcepParams.Controls.Add(this.btnGetEx);
            this.ugbxJExcepParams.Controls.Add(this.btnOverriding);
            this.ugbxJExcepParams.Controls.Add(this.btnRunRevaluation);
            this.ugbxJExcepParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxJExcepParams.Location = new System.Drawing.Point(3, 3);
            this.ugbxJExcepParams.Name = "ugbxJExcepParams";
            this.ugbxJExcepParams.Size = new System.Drawing.Size(1168, 47);
            this.ugbxJExcepParams.TabIndex = 103;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(6, 1);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(486, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 11;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxJournalExceptions);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1174, 414);
            this.ultraPanel1.TabIndex = 103;
            // 
            // ctrlJournalExceptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlJournalExceptions";
            this.Size = new System.Drawing.Size(1174, 414);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlJournalExceptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashExceptions)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJournalExceptions)).EndInit();
            this.ugbxJournalExceptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJExcep)).EndInit();
            this.ugbxJExcep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJExcepParams)).EndInit();
            this.ugbxJExcepParams.ResumeLayout(false);
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
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashExceptions;
        private Infragistics.Win.Misc.UltraButton btnOverriding;
        private Infragistics.Win.Misc.UltraButton btnRunRevaluation;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraGroupBox ugbxJournalExceptions;
        private Infragistics.Win.Misc.UltraGroupBox ugbxJExcepParams;
        private Infragistics.Win.Misc.UltraGroupBox ugbxJExcep;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayouttToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
    }
}
