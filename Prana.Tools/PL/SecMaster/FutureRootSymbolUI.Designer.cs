namespace Prana.Tools
{
    partial class FutureRootSymbolUI
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
            UnwireEvents();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if(_dtRootSymbolData != null)
                {
                    _dtRootSymbolData.Dispose();
                }
                if (_UDAAssets != null)
                {
                    _UDAAssets.Dispose();
                }
                if (_UDACountry != null)
                {
                    _UDACountry.Dispose();
                }
                if (_UDASectors != null)
                {
                    _UDASectors.Dispose();
                }
                if (_UDASecurityTypes != null)
                {
                    _UDASecurityTypes.Dispose();
                }
                if (_UDASubSectors != null)
                {
                    _UDASubSectors.Dispose();
                }
                if (_approvalSatus != null)
                {
                    _approvalSatus.Dispose();
                }
                if (_timer != null)
                {
                    _timer.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnAddRow = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.chkBoxCutOffTime = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblexchange1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblexchange2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.txtSearch = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSearch = new Infragistics.Win.Misc.UltraLabel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.FutureRootSymbolUI_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxCutOffTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.SuspendLayout();
            this.FutureRootSymbolUI_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.grdData.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.Override.CellPadding = 0;
            this.grdData.DisplayLayout.Override.CellSpacing = 0;
            this.grdData.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.TextHAlignAsString = "Center";
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.ForeColor = System.Drawing.Color.Orange;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance5.ForeColor = System.Drawing.Color.Orange;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdData.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.grdData.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdData.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            appearance8.BackColor = System.Drawing.SystemColors.Info;
            this.grdData.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance8;
            this.grdData.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdData.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.grdData.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.UseFixedHeaders = true;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdData.Location = new System.Drawing.Point(3, 86);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(910, 321);
            this.grdData.TabIndex = 1;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_AfterCellUpdate);
            this.grdData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_CellChange);
            this.grdData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            this.grdData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColorInternal = System.Drawing.Color.Silver;
            this.btnSave.Location = new System.Drawing.Point(511, 412);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(4, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(915, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 85;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // btnAddRow
            // 
            this.btnAddRow.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddRow.BackColorInternal = System.Drawing.Color.Silver;
            this.btnAddRow.Location = new System.Drawing.Point(328, 412);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(92, 23);
            this.btnAddRow.TabIndex = 86;
            this.btnAddRow.Text = "Add New Row";
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColorInternal = System.Drawing.Color.Silver;
            this.btnDelete.Location = new System.Drawing.Point(428, 412);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete Row";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkBoxCutOffTime
            // 
            this.chkBoxCutOffTime.AutoSize = true;
            this.chkBoxCutOffTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxCutOffTime.Location = new System.Drawing.Point(26, 12);
            this.chkBoxCutOffTime.Name = "chkBoxCutOffTime";
            this.chkBoxCutOffTime.Size = new System.Drawing.Size(160, 17);
            this.chkBoxCutOffTime.TabIndex = 87;
            this.chkBoxCutOffTime.Text = "Use CutOffTime for Futures";
            this.chkBoxCutOffTime.CheckedChanged += new System.EventHandler(this.chkBoxCutOffTime_CheckedChanged);
            // 
            // lblexchange1
            // 
            this.lblexchange1.AutoSize = true;
            this.lblexchange1.BackColorInternal = System.Drawing.SystemColors.Control;
            this.lblexchange1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblexchange1.ForeColor = System.Drawing.Color.Firebrick;
            this.lblexchange1.Location = new System.Drawing.Point(23, 36);
            this.lblexchange1.Name = "lblexchange1";
            this.lblexchange1.Size = new System.Drawing.Size(252, 14);
            this.lblexchange1.TabIndex = 88;
            this.lblexchange1.Text = "* Enter the E-Signal Symbol suffix as Exchange";
            // 
            // lblexchange2
            // 
            this.lblexchange2.AutoSize = true;
            this.lblexchange2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblexchange2.ForeColor = System.Drawing.Color.Firebrick;
            this.lblexchange2.Location = new System.Drawing.Point(23, 60);
            this.lblexchange2.Name = "lblexchange2";
            this.lblexchange2.Size = new System.Drawing.Size(393, 14);
            this.lblexchange2.TabIndex = 89;
            this.lblexchange2.Text = "* For US Symbols without suffix, Please leave the Exchange Column blank";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(814, 59);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 23);
            this.btnGetData.TabIndex = 92;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(706, 61);
            this.txtSearch.MaxLength = 30;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(102, 21);
            this.txtSearch.TabIndex = 90;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(660, 64);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(43, 14);
            this.lblSearch.TabIndex = 91;
            this.lblSearch.Text = "Search:";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // FutureRootSymbolUI_Fill_Panel
            // 
            // 
            // FutureRootSymbolUI_Fill_Panel.ClientArea
            // 
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.btnGetData);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.txtSearch);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.lblSearch);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.lblexchange2);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.lblexchange1);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.chkBoxCutOffTime);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.btnAddRow);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.btnDelete);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.Controls.Add(this.grdData);
            this.FutureRootSymbolUI_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FutureRootSymbolUI_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FutureRootSymbolUI_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.FutureRootSymbolUI_Fill_Panel.Name = "FutureRootSymbolUI_Fill_Panel";
            this.FutureRootSymbolUI_Fill_Panel.Size = new System.Drawing.Size(915, 460);
            this.FutureRootSymbolUI_Fill_Panel.TabIndex = 86;
            // 
            // _FutureRootSymbolUI_UltraFormManager_Dock_Area_Left
            // 
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.Name = "_FutureRootSymbolUI_UltraFormManager_Dock_Area_Left";
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 482);
            // 
            // _FutureRootSymbolUI_UltraFormManager_Dock_Area_Right
            // 
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(919, 27);
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.Name = "_FutureRootSymbolUI_UltraFormManager_Dock_Area_Right";
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 482);
            // 
            // _FutureRootSymbolUI_UltraFormManager_Dock_Area_Top
            // 
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.Name = "_FutureRootSymbolUI_UltraFormManager_Dock_Area_Top";
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(923, 27);
            // 
            // _FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 509);
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.Name = "_FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom";
            this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(923, 4);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenu.Name = "cntxtMnu";
            this.contextMenu.Size = new System.Drawing.Size(138, 26);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenu, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // FutureRootSymbolUI
            // 
            this.AcceptButton = this.btnGetData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 513);
            this.Controls.Add(this.FutureRootSymbolUI_Fill_Panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom);
            this.Name = "FutureRootSymbolUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Future Root Symbol";
            this.Load += new System.EventHandler(this.FutureRootSymbolUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxCutOffTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.ResumeLayout(false);
            this.FutureRootSymbolUI_Fill_Panel.ClientArea.PerformLayout();
            this.FutureRootSymbolUI_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraButton btnAddRow;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxCutOffTime;
        private Infragistics.Win.Misc.UltraLabel lblexchange1;
        private Infragistics.Win.Misc.UltraLabel lblexchange2;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSearch;
        private Infragistics.Win.Misc.UltraLabel lblSearch;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel FutureRootSymbolUI_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FutureRootSymbolUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FutureRootSymbolUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FutureRootSymbolUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _FutureRootSymbolUI_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
    }
}
