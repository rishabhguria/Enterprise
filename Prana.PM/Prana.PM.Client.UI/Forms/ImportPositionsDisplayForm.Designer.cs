using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI
{
    partial class ImportPositionsDisplayForm
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
                if (headerCheckBox != null)
                {
                    headerCheckBox.Dispose();
                }
                if (_dsColInterstImportValue != null)
                {
                    _dsColInterstImportValue.Dispose();
                }
                if (_dsSecMasterInsert != null)
                {
                    _dsSecMasterInsert.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (NewDatatset != null)
                {
                    NewDatatset.Dispose();
                }
                _importPositionsDisplayForm = null;

                if (_accrualBasis != null)
                {
                    _accrualBasis.Dispose();
                }
                if (_assets != null)
                {
                    _assets.Dispose();
                }
                if (_currencies != null)
                {
                    _currencies.Dispose();
                }
                if (_frequency != null)
                {
                    _frequency.Dispose();
                }
                if (_exchanges != null)
                {
                    _exchanges.Dispose();
                }
                if (_securityType != null)
                {
                    _securityType.Dispose();
                }
                if (subAccount != null)
                {
                    subAccount.Dispose();
                }
                if (_underLying != null)
                {
                    _underLying.Dispose();
                }
                if (_optionType != null)
                {
                    _optionType.Dispose();
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
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportPositionsDisplayForm));
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.grdImportData = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.cntxtMnuPositionImprt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSymbolManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnMapping = new Infragistics.Win.Misc.UltraButton();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.btnSymbolLookup = new Infragistics.Win.Misc.UltraButton();
            this.gridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.feedbackLabel = new System.Windows.Forms.Label();
            this.progressCircle = new Prana.Utilities.UI.UIUtilities.ProgressCircle();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportData)).BeginInit();
            this.cntxtMnuPositionImprt.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ultraButton1.Location = new System.Drawing.Point(297, 6);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 2;
            this.ultraButton1.Text = "Continue";
            this.ultraButton1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // ultraButton2
            // 
            this.ultraButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ultraButton2.Location = new System.Drawing.Point(381, 6);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(75, 23);
            this.ultraButton2.TabIndex = 3;
            this.ultraButton2.Text = "Abort";
            this.ultraButton2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // grdImportData
            // 
            this.grdImportData.CausesValidation = false;
            this.grdImportData.ContextMenuStrip = this.cntxtMnuPositionImprt;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdImportData.DisplayLayout.Appearance = appearance1;
            this.grdImportData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdImportData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdImportData.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdImportData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdImportData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdImportData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdImportData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.BorderColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdImportData.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdImportData.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdImportData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdImportData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdImportData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdImportData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdImportData.DisplayLayout.Override.CellPadding = 0;
            this.grdImportData.DisplayLayout.Override.DefaultColWidth = 80;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BackColor2 = System.Drawing.Color.Transparent;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdImportData.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            this.grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            appearance7.TextHAlignAsString = "Left";
            this.grdImportData.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdImportData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdImportData.DisplayLayout.Override.MaxSelectedRows = 1;
            appearance8.BackColor = System.Drawing.Color.Black;
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.grdImportData.DisplayLayout.Override.RowAlternateAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance9.ForeColor = System.Drawing.Color.Transparent;
            this.grdImportData.DisplayLayout.Override.RowAppearance = appearance9;
            this.grdImportData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.grdImportData.DisplayLayout.Override.SelectedCellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.grdImportData.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.grdImportData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdImportData.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdImportData.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdImportData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdImportData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdImportData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdImportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdImportData.Location = new System.Drawing.Point(0, 0);
            this.grdImportData.Name = "grdImportData";
            this.grdImportData.Size = new System.Drawing.Size(850, 315);
            this.grdImportData.SyncWithCurrencyManager = false;
            this.grdImportData.TabIndex = 1;
            this.grdImportData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdImportData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdImportData_AfterCellUpdate);
            this.grdImportData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdImportData_InitializeLayout);
            this.grdImportData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdImportData_InitializeRow);
            this.grdImportData.FilterRow += new Infragistics.Win.UltraWinGrid.FilterRowEventHandler(this.grdImportData_FilterRow);
            this.grdImportData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdImportData_BeforeCustomRowFilterDialog);
            this.grdImportData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdImportData_BeforeColumnChooserDisplayed);
            this.grdImportData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdImportData_MouseDown);
            // 
            // cntxtMnuPositionImprt
            // 
            this.cntxtMnuPositionImprt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSymbolManuallyToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.saveLayoutToolStripMenuItem});
            this.cntxtMnuPositionImprt.Name = "cntxtMnuPositionImprt";
            this.cntxtMnuPositionImprt.Size = new System.Drawing.Size(192, 70);
            this.cntxtMnuPositionImprt.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMnuPositionImprt_Opening);
            // 
            // addSymbolManuallyToolStripMenuItem
            // 
            this.addSymbolManuallyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addSymbolManuallyToolStripMenuItem.Image")));
            this.addSymbolManuallyToolStripMenuItem.Name = "addSymbolManuallyToolStripMenuItem";
            this.addSymbolManuallyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.addSymbolManuallyToolStripMenuItem.Text = "Add Symbol Manually";
            this.addSymbolManuallyToolStripMenuItem.Click += new System.EventHandler(this.addSymbolManuallyToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportDataToolStripMenuItem.Image")));
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asDefaultToolStripMenuItem,
            this.restoreToDefaultToolStripMenuItem});
            this.saveLayoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            // 
            // asDefaultToolStripMenuItem
            // 
            this.asDefaultToolStripMenuItem.Name = "asDefaultToolStripMenuItem";
            this.asDefaultToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.asDefaultToolStripMenuItem.Text = "As Default";
            this.asDefaultToolStripMenuItem.Click += new System.EventHandler(this.asDefaultToolStripMenuItem_Click);
            // 
            // restoreToDefaultToolStripMenuItem
            // 
            this.restoreToDefaultToolStripMenuItem.Name = "restoreToDefaultToolStripMenuItem";
            this.restoreToDefaultToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.restoreToDefaultToolStripMenuItem.Text = "Restore to Default";
            this.restoreToDefaultToolStripMenuItem.Click += new System.EventHandler(this.restoreToDefaultToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(8, 379);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(850, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = " ";
            // 
            // btnMapping
            // 
            this.btnMapping.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnMapping.Location = new System.Drawing.Point(591, 6);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(127, 23);
            this.btnMapping.TabIndex = 4;
            this.btnMapping.Text = "Sub Account  Mapping";
            this.btnMapping.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnMapping.Click += new System.EventHandler(this.btnMapping_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRefresh.Location = new System.Drawing.Point(727, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(70, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSymbolLookup
            // 
            this.btnSymbolLookup.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSymbolLookup.Location = new System.Drawing.Point(465, 6);
            this.btnSymbolLookup.Name = "btnSymbolLookup";
            this.btnSymbolLookup.Size = new System.Drawing.Size(117, 23);
            this.btnSymbolLookup.TabIndex = 7;
            this.btnSymbolLookup.Text = "Security Master";
            this.btnSymbolLookup.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSymbolLookup.Click += new System.EventHandler(this.btnSymbolLookup_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.panel);
            this.ultraPanel1.ClientArea.Controls.Add(this.grdImportData);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 32);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(850, 315);
            this.ultraPanel1.TabIndex = 8;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.AutoSize = true;
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.Controls.Add(this.feedbackLabel);
            this.panel.Controls.Add(this.progressCircle);
            this.panel.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel.Location = new System.Drawing.Point(268, 124);
            this.panel.Margin = new System.Windows.Forms.Padding(2);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(315, 66);
            this.panel.TabIndex = 33;
            this.panel.Visible = false;
            // 
            // feedbackLabel
            // 
            this.feedbackLabel.AutoSize = true;
            this.feedbackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.feedbackLabel.ForeColor = System.Drawing.Color.Black;
            this.feedbackLabel.Location = new System.Drawing.Point(37, 49);
            this.feedbackLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 4);
            this.feedbackLabel.MinimumSize = new System.Drawing.Size(183, 0);
            this.feedbackLabel.Name = "feedbackLabel";
            this.feedbackLabel.Size = new System.Drawing.Size(183, 13);
            this.feedbackLabel.TabIndex = 6;
            this.feedbackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressCircle
            // 
            this.progressCircle.AutoSize = true;
            this.progressCircle.BackColor = System.Drawing.Color.Transparent;
            this.progressCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(5)))), ((int)(((byte)(90)))));
            this.progressCircle.Location = new System.Drawing.Point(137, 3);
            this.progressCircle.Margin = new System.Windows.Forms.Padding(2);
            this.progressCircle.Name = "progressCircle";
            this.progressCircle.NumberOfTail = 7;
            this.progressCircle.RingColor = System.Drawing.Color.White;
            this.progressCircle.RingThickness = 10;
            this.progressCircle.Size = new System.Drawing.Size(38, 41);
            this.progressCircle.TabIndex = 5;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left
            // 
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.Name = "_ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left";
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 369);
            // 
            // _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right
            // 
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(858, 32);
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.Name = "_ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right";
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 369);
            // 
            // _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top
            // 
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.Name = "_ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top";
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(866, 32);
            // 
            // _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 401);
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.Name = "_ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom";
            this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(866, 8);
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraButton1);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnRefresh);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraButton2);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnMapping);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnSymbolLookup);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Location = new System.Drawing.Point(8, 347);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(850, 32);
            this.ultraPanel2.TabIndex = 13;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblCount.Location = new System.Drawing.Point(505, 383);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 13);
            this.lblCount.TabIndex = 18;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblQuantity.Location = new System.Drawing.Point(655, 382);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(0, 13);
            this.lblQuantity.TabIndex = 19;
            // 
            // ImportPositionsDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 409);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "ImportPositionsDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportPositionsDisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.ImportPositionsDisplayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdImportData)).EndInit();
            this.cntxtMnuPositionImprt.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private PranaUltraGrid grdImportData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraButton btnMapping;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.Misc.UltraButton btnSymbolLookup;
        private System.Windows.Forms.ContextMenuStrip cntxtMnuPositionImprt;
        private System.Windows.Forms.ToolStripMenuItem addSymbolManuallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToDefaultToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter gridExcelExporter;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportPositionsDisplayForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        public System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label feedbackLabel;
        private ProgressCircle progressCircle;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblCount;
        //  private Infragistics.Win.Misc.UltraButton btnScreenshot;
    }
}