using Prana.Global;
namespace Prana.ClientCommon
{
    partial class AllocationSchemeForm
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
                if (_dsAllocationScheme != null)
                {
                    _dsAllocationScheme.Dispose();
                }
                if (_dsSchemeNames != null)
                {
                    _dsSchemeNames.Dispose();
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
            this.grdScheme = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.dtSchemeDate = new System.Windows.Forms.DateTimePicker();
            this.lblSchemeDate = new System.Windows.Forms.Label();
            this.cmbAllocationScheme = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSchemeName = new System.Windows.Forms.Label();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnGetScheme = new Infragistics.Win.Misc.UltraButton();
            this.stripStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMatched = new System.Windows.Forms.Label();
            this.lblMisMatched = new System.Windows.Forms.Label();
            this.grpMisMatched = new System.Windows.Forms.Panel();
            this.grpMatched = new System.Windows.Forms.Panel();
            this.grpNotTraded = new System.Windows.Forms.Panel();
            this.lblNotTraded = new System.Windows.Forms.Label();
            this.ExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AllocationSchemeForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.grdScheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocationScheme)).BeginInit();
            this.stripStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AllocationSchemeForm_Fill_Panel.ClientArea.SuspendLayout();
            this.AllocationSchemeForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdScheme
            // 
            this.grdScheme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdScheme.CausesValidation = false;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdScheme.DisplayLayout.Appearance = appearance1;
            this.grdScheme.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdScheme.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdScheme.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdScheme.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdScheme.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdScheme.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdScheme.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Gold;
            appearance5.BorderColor = System.Drawing.Color.Black;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdScheme.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdScheme.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdScheme.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdScheme.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdScheme.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdScheme.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdScheme.DisplayLayout.Override.CellPadding = 0;
            this.grdScheme.DisplayLayout.Override.DefaultColWidth = 80;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.grdScheme.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            this.grdScheme.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            appearance7.TextHAlignAsString = "Left";
            this.grdScheme.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdScheme.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdScheme.DisplayLayout.Override.MaxSelectedRows = 1;
            appearance8.BackColor = System.Drawing.Color.Black;
            appearance8.ForeColor = System.Drawing.Color.Silver;
            this.grdScheme.DisplayLayout.Override.RowAlternateAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance9.ForeColor = System.Drawing.Color.Lime;
            this.grdScheme.DisplayLayout.Override.RowAppearance = appearance9;
            this.grdScheme.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance10.BackColor = System.Drawing.Color.Gold;
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.grdScheme.DisplayLayout.Override.SelectedCellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.grdScheme.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.grdScheme.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdScheme.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdScheme.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdScheme.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdScheme.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdScheme.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdScheme.Location = new System.Drawing.Point(-1, 34);
            this.grdScheme.Name = "grdScheme";
            this.grdScheme.Size = new System.Drawing.Size(834, 341);
            this.grdScheme.SyncWithCurrencyManager = false;
            this.grdScheme.TabIndex = 0;
            this.grdScheme.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
            this.grdScheme.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdScheme_InitializeLayout);
            this.grdScheme.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdScheme_InitializeRow);
            this.grdScheme.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_CellChange);
            this.grdScheme.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdScheme_BeforeCustomRowFilterDialog);
            this.grdScheme.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdScheme_BeforeColumnChooserDisplayed);
            // 
            // dtSchemeDate
            // 
            this.dtSchemeDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtSchemeDate.Location = new System.Drawing.Point(54, 7);
            this.dtSchemeDate.Name = "dtSchemeDate";
            this.dtSchemeDate.Size = new System.Drawing.Size(90, 20);
            this.dtSchemeDate.TabIndex = 1;
            this.dtSchemeDate.ValueChanged += new System.EventHandler(this.dtSchemeDate_ValueChanged);
            // 
            // lblSchemeDate
            // 
            this.lblSchemeDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSchemeDate.AutoSize = true;
            this.lblSchemeDate.Location = new System.Drawing.Point(19, 11);
            this.lblSchemeDate.Name = "lblSchemeDate";
            this.lblSchemeDate.Size = new System.Drawing.Size(30, 13);
            this.lblSchemeDate.TabIndex = 2;
            this.lblSchemeDate.Text = "Date";
            // 
            // cmbAllocationScheme
            // 
            this.cmbAllocationScheme.AutoSize = false;
            this.cmbAllocationScheme.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbAllocationScheme.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAllocationScheme.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAllocationScheme.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbAllocationScheme.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAllocationScheme.Location = new System.Drawing.Point(379, 7);
            this.cmbAllocationScheme.Name = "cmbAllocationScheme";
            this.cmbAllocationScheme.Size = new System.Drawing.Size(231, 22);
            this.cmbAllocationScheme.TabIndex = 3;
            this.cmbAllocationScheme.ValueChanged += new System.EventHandler(this.cmbAllocationScheme_ValueChanged);
            // 
            // lblSchemeName
            // 
            this.lblSchemeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSchemeName.AutoSize = true;
            this.lblSchemeName.Location = new System.Drawing.Point(296, 11);
            this.lblSchemeName.Name = "lblSchemeName";
            this.lblSchemeName.Size = new System.Drawing.Size(77, 13);
            this.lblSchemeName.TabIndex = 4;
            this.lblSchemeName.Text = "Scheme Name";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(291, 380);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(421, 380);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGetScheme
            // 
            this.btnGetScheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetScheme.Location = new System.Drawing.Point(625, 6);
            this.btnGetScheme.Name = "btnGetScheme";
            this.btnGetScheme.Size = new System.Drawing.Size(114, 23);
            this.btnGetScheme.TabIndex = 7;
            this.btnGetScheme.Text = "Get Scheme";
            this.btnGetScheme.Click += new System.EventHandler(this.btnGetScheme_Click);
            // 
            // stripStatus
            // 
            this.stripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.stripStatus.Location = new System.Drawing.Point(4, 472);
            this.stripStatus.Name = "stripStatus";
            this.stripStatus.Size = new System.Drawing.Size(833, 22);
            this.stripStatus.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // lblMatched
            // 
            this.lblMatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMatched.AutoSize = true;
            this.lblMatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatched.Location = new System.Drawing.Point(33, 386);
            this.lblMatched.Name = "lblMatched";
            this.lblMatched.Size = new System.Drawing.Size(48, 13);
            this.lblMatched.TabIndex = 11;
            this.lblMatched.Text = "Matched";
            // 
            // lblMisMatched
            // 
            this.lblMisMatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMisMatched.AutoSize = true;
            this.lblMisMatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMisMatched.Location = new System.Drawing.Point(111, 386);
            this.lblMisMatched.Name = "lblMisMatched";
            this.lblMisMatched.Size = new System.Drawing.Size(64, 13);
            this.lblMisMatched.TabIndex = 12;
            this.lblMisMatched.Text = "MisMatched";
            // 
            // grpMisMatched
            // 
            this.grpMisMatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpMisMatched.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpMisMatched.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grpMisMatched.Location = new System.Drawing.Point(90, 386);
            this.grpMisMatched.Name = "grpMisMatched";
            this.grpMisMatched.Size = new System.Drawing.Size(15, 15);
            this.grpMisMatched.TabIndex = 15;
            // 
            // grpMatched
            // 
            this.grpMatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpMatched.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.grpMatched.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grpMatched.Location = new System.Drawing.Point(12, 386);
            this.grpMatched.Name = "grpMatched";
            this.grpMatched.Size = new System.Drawing.Size(15, 15);
            this.grpMatched.TabIndex = 14;
            // 
            // grpNotTraded
            // 
            this.grpNotTraded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpNotTraded.BackColor = System.Drawing.Color.White;
            this.grpNotTraded.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grpNotTraded.Location = new System.Drawing.Point(12, 406);
            this.grpNotTraded.Name = "grpNotTraded";
            this.grpNotTraded.Size = new System.Drawing.Size(15, 15);
            this.grpNotTraded.TabIndex = 16;
            // 
            // lblNotTraded
            // 
            this.lblNotTraded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotTraded.AutoSize = true;
            this.lblNotTraded.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotTraded.Location = new System.Drawing.Point(33, 406);
            this.lblNotTraded.Name = "lblNotTraded";
            this.lblNotTraded.Size = new System.Drawing.Size(219, 13);
            this.lblNotTraded.TabIndex = 17;
            this.lblNotTraded.Text = "Symbol not allocated on given allocation date\r\n";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(156, 11);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(20, 13);
            this.lblToDate.TabIndex = 18;
            this.lblToDate.Text = "To";
            // 
            // dtToDate
            // 
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtToDate.Location = new System.Drawing.Point(181, 7);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(91, 20);
            this.dtToDate.TabIndex = 19;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AllocationSchemeForm_Fill_Panel
            // 
            // 
            // AllocationSchemeForm_Fill_Panel.ClientArea
            // 
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.dtToDate);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblToDate);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblNotTraded);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.grpNotTraded);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.grpMatched);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.grpMisMatched);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblMatched);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblMisMatched);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.btnGetScheme);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.btnClose);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblSchemeName);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.cmbAllocationScheme);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.lblSchemeDate);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.dtSchemeDate);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.Controls.Add(this.grdScheme);
            this.AllocationSchemeForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationSchemeForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationSchemeForm_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.AllocationSchemeForm_Fill_Panel.Name = "AllocationSchemeForm_Fill_Panel";
            this.AllocationSchemeForm_Fill_Panel.Size = new System.Drawing.Size(833, 445);
            this.AllocationSchemeForm_Fill_Panel.TabIndex = 9;
            // 
            // _AllocationSchemeForm_UltraFormManager_Dock_Area_Left
            // 
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.Name = "_AllocationSchemeForm_UltraFormManager_Dock_Area_Left";
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 467);
            // 
            // _AllocationSchemeForm_UltraFormManager_Dock_Area_Right
            // 
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(837, 27);
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.Name = "_AllocationSchemeForm_UltraFormManager_Dock_Area_Right";
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 467);
            // 
            // _AllocationSchemeForm_UltraFormManager_Dock_Area_Top
            // 
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.Name = "_AllocationSchemeForm_UltraFormManager_Dock_Area_Top";
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(841, 27);
            // 
            // _AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 494);
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.Name = "_AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom";
            this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(841, 4);
            // 
            // AllocationSchemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(841, 498);
            this.Controls.Add(this.AllocationSchemeForm_Fill_Panel);
            this.Controls.Add(this.stripStatus);
            this.Controls.Add(this._AllocationSchemeForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AllocationSchemeForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AllocationSchemeForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "AllocationSchemeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allocation Scheme";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AllocationSchemeForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AllocationSchemeForm_FormClosed);
            this.Load += new System.EventHandler(this.AllocationSchemeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdScheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocationScheme)).EndInit();
            this.stripStatus.ResumeLayout(false);
            this.stripStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AllocationSchemeForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AllocationSchemeForm_Fill_Panel.ClientArea.PerformLayout();
            this.AllocationSchemeForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdScheme;
        private System.Windows.Forms.DateTimePicker dtSchemeDate;
        private System.Windows.Forms.Label lblSchemeDate;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAllocationScheme;
        private System.Windows.Forms.Label lblSchemeName;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private Infragistics.Win.Misc.UltraButton btnGetScheme;
        private System.Windows.Forms.StatusStrip stripStatus;
        private System.Windows.Forms.Label lblMatched;
        private System.Windows.Forms.Label lblMisMatched;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel grpMatched;
        private System.Windows.Forms.Panel grpMisMatched;
        private System.Windows.Forms.Panel grpNotTraded;
        private System.Windows.Forms.Label lblNotTraded;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ExcelExporter;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel AllocationSchemeForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationSchemeForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationSchemeForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationSchemeForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AllocationSchemeForm_UltraFormManager_Dock_Area_Bottom;
    }
}