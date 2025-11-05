using Infragistics.Win;
namespace Prana.PM.Admin.UI.Controls
{
    partial class CtrlMapColumns
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
                _unlockedApplicationColumnList.Dispose();
                _lockedApplicationColumnList.Dispose();
                components.Dispose();
            }
            _isInitialized = false;
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
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlMapColumns));
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("TableType", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TableTypeName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TableTypeID");
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            this.grdMapColumns = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.lblNameOfSource = new Infragistics.Win.Misc.UltraLabel();
            this.lblDataSourceName = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSourceColumn = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbApplicationColumn = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.bindingSourceTableType = new System.Windows.Forms.BindingSource(this.components);
            this.cmbTableType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblTableType = new Infragistics.Win.Misc.UltraLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.isLockingEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdMapColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplicationColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTableType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTableType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdMapColumns
            // 
            this.grdMapColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance52.BackColor = System.Drawing.SystemColors.Window;
            appearance52.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdMapColumns.DisplayLayout.Appearance = appearance52;
            ultraGridBand3.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdMapColumns.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.grdMapColumns.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMapColumns.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance53.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance53.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance53.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance53.BorderColor = System.Drawing.SystemColors.Window;
            this.grdMapColumns.DisplayLayout.GroupByBox.Appearance = appearance53;
            appearance54.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdMapColumns.DisplayLayout.GroupByBox.BandLabelAppearance = appearance54;
            this.grdMapColumns.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMapColumns.DisplayLayout.GroupByBox.Hidden = true;
            appearance55.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance55.BackColor2 = System.Drawing.SystemColors.Control;
            appearance55.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance55.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdMapColumns.DisplayLayout.GroupByBox.PromptAppearance = appearance55;
            this.grdMapColumns.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMapColumns.DisplayLayout.MaxRowScrollRegions = 1;
            appearance56.BackColor = System.Drawing.SystemColors.Window;
            appearance56.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdMapColumns.DisplayLayout.Override.ActiveCellAppearance = appearance56;
            appearance57.BackColor = System.Drawing.SystemColors.Highlight;
            appearance57.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdMapColumns.DisplayLayout.Override.ActiveRowAppearance = appearance57;
            this.grdMapColumns.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdMapColumns.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance58.BackColor = System.Drawing.SystemColors.Window;
            this.grdMapColumns.DisplayLayout.Override.CardAreaAppearance = appearance58;
            appearance59.BorderColor = System.Drawing.Color.Silver;
            appearance59.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdMapColumns.DisplayLayout.Override.CellAppearance = appearance59;
            this.grdMapColumns.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdMapColumns.DisplayLayout.Override.CellPadding = 0;
            appearance60.BackColor = System.Drawing.SystemColors.Control;
            appearance60.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance60.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance60.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance60.BorderColor = System.Drawing.SystemColors.Window;
            this.grdMapColumns.DisplayLayout.Override.GroupByRowAppearance = appearance60;
            appearance61.TextHAlign = Infragistics.Win.HAlign.Left;
            this.grdMapColumns.DisplayLayout.Override.HeaderAppearance = appearance61;
            this.grdMapColumns.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdMapColumns.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance62.BackColor = System.Drawing.SystemColors.Window;
            appearance62.BorderColor = System.Drawing.Color.Silver;
            this.grdMapColumns.DisplayLayout.Override.RowAppearance = appearance62;
            this.grdMapColumns.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdMapColumns.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance63.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdMapColumns.DisplayLayout.Override.TemplateAddRowAppearance = appearance63;
            this.grdMapColumns.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMapColumns.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMapColumns.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMapColumns.Location = new System.Drawing.Point(3, 55);
            this.grdMapColumns.Name = "grdMapColumns";
            this.grdMapColumns.Size = new System.Drawing.Size(485, 239);
            this.grdMapColumns.TabIndex = 0;
            this.grdMapColumns.Text = "ultraGrid1";
            this.grdMapColumns.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMapColumns_InitializeRow);
            this.grdMapColumns.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMapColumns_InitializeLayout);
            this.grdMapColumns.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMapColumns_AfterCellUpdate);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance64.Image = ((object)(resources.GetObject("appearance64.Image")));
            this.btnClose.Appearance = appearance64;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(340, 301);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance65.Image = ((object)(resources.GetObject("appearance65.Image")));
            this.btnSave.Appearance = appearance65;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(82, 301);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance66.Image = global::Prana.PM.Admin.UI.Properties.Resources.btn_clear;
            this.btnClear.Appearance = appearance66;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(211, 301);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 35;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblNameOfSource
            // 
            this.lblNameOfSource.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNameOfSource.Location = new System.Drawing.Point(15, 13);
            this.lblNameOfSource.Name = "lblNameOfSource";
            this.lblNameOfSource.Size = new System.Drawing.Size(84, 15);
            this.lblNameOfSource.TabIndex = 36;
            this.lblNameOfSource.Text = "Name of Source";
            // 
            // lblDataSourceName
            // 
            this.lblDataSourceName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDataSourceName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDataSourceName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblDataSourceName.Location = new System.Drawing.Point(104, 12);
            this.lblDataSourceName.Name = "lblDataSourceName";
            this.lblDataSourceName.Size = new System.Drawing.Size(120, 17);
            this.lblDataSourceName.TabIndex = 37;
            // 
            // cmbSourceColumn
            // 
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            appearance67.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSourceColumn.DisplayLayout.Appearance = appearance67;
            this.cmbSourceColumn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSourceColumn.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance68.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance68.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance68.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance68.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSourceColumn.DisplayLayout.GroupByBox.Appearance = appearance68;
            appearance69.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSourceColumn.DisplayLayout.GroupByBox.BandLabelAppearance = appearance69;
            this.cmbSourceColumn.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance70.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance70.BackColor2 = System.Drawing.SystemColors.Control;
            appearance70.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance70.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSourceColumn.DisplayLayout.GroupByBox.PromptAppearance = appearance70;
            this.cmbSourceColumn.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSourceColumn.DisplayLayout.MaxRowScrollRegions = 1;
            appearance71.BackColor = System.Drawing.SystemColors.Window;
            appearance71.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSourceColumn.DisplayLayout.Override.ActiveCellAppearance = appearance71;
            appearance72.BackColor = System.Drawing.SystemColors.Highlight;
            appearance72.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSourceColumn.DisplayLayout.Override.ActiveRowAppearance = appearance72;
            this.cmbSourceColumn.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSourceColumn.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSourceColumn.DisplayLayout.Override.CardAreaAppearance = appearance73;
            appearance74.BorderColor = System.Drawing.Color.Silver;
            appearance74.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSourceColumn.DisplayLayout.Override.CellAppearance = appearance74;
            this.cmbSourceColumn.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSourceColumn.DisplayLayout.Override.CellPadding = 0;
            appearance75.BackColor = System.Drawing.SystemColors.Control;
            appearance75.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance75.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance75.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSourceColumn.DisplayLayout.Override.GroupByRowAppearance = appearance75;
            appearance76.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbSourceColumn.DisplayLayout.Override.HeaderAppearance = appearance76;
            this.cmbSourceColumn.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSourceColumn.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance77.BackColor = System.Drawing.SystemColors.Window;
            appearance77.BorderColor = System.Drawing.Color.Silver;
            this.cmbSourceColumn.DisplayLayout.Override.RowAppearance = appearance77;
            this.cmbSourceColumn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance78.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSourceColumn.DisplayLayout.Override.TemplateAddRowAppearance = appearance78;
            this.cmbSourceColumn.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSourceColumn.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSourceColumn.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSourceColumn.DisplayMember = "";
            this.cmbSourceColumn.Location = new System.Drawing.Point(286, 4);
            this.cmbSourceColumn.Name = "cmbSourceColumn";
            this.cmbSourceColumn.Size = new System.Drawing.Size(95, 33);
            this.cmbSourceColumn.TabIndex = 38;
            this.cmbSourceColumn.Text = "ultraDropDown1";
            this.cmbSourceColumn.ValueMember = "";
            this.cmbSourceColumn.Visible = false;
            // 
            // cmbApplicationColumn
            // 
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            appearance79.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbApplicationColumn.DisplayLayout.Appearance = appearance79;
            this.cmbApplicationColumn.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbApplicationColumn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbApplicationColumn.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance80.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance80.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance80.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance80.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplicationColumn.DisplayLayout.GroupByBox.Appearance = appearance80;
            appearance81.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplicationColumn.DisplayLayout.GroupByBox.BandLabelAppearance = appearance81;
            this.cmbApplicationColumn.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance82.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance82.BackColor2 = System.Drawing.SystemColors.Control;
            appearance82.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance82.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplicationColumn.DisplayLayout.GroupByBox.PromptAppearance = appearance82;
            this.cmbApplicationColumn.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbApplicationColumn.DisplayLayout.MaxRowScrollRegions = 1;
            appearance83.BackColor = System.Drawing.SystemColors.Window;
            appearance83.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbApplicationColumn.DisplayLayout.Override.ActiveCellAppearance = appearance83;
            appearance84.BackColor = System.Drawing.SystemColors.Highlight;
            appearance84.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbApplicationColumn.DisplayLayout.Override.ActiveRowAppearance = appearance84;
            this.cmbApplicationColumn.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbApplicationColumn.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance85.BackColor = System.Drawing.SystemColors.Window;
            this.cmbApplicationColumn.DisplayLayout.Override.CardAreaAppearance = appearance85;
            appearance86.BorderColor = System.Drawing.Color.Silver;
            appearance86.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbApplicationColumn.DisplayLayout.Override.CellAppearance = appearance86;
            this.cmbApplicationColumn.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbApplicationColumn.DisplayLayout.Override.CellPadding = 0;
            appearance87.BackColor = System.Drawing.SystemColors.Control;
            appearance87.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance87.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance87.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplicationColumn.DisplayLayout.Override.GroupByRowAppearance = appearance87;
            appearance88.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbApplicationColumn.DisplayLayout.Override.HeaderAppearance = appearance88;
            this.cmbApplicationColumn.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbApplicationColumn.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance89.BackColor = System.Drawing.SystemColors.Window;
            appearance89.BorderColor = System.Drawing.Color.Silver;
            this.cmbApplicationColumn.DisplayLayout.Override.RowAppearance = appearance89;
            this.cmbApplicationColumn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance90.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbApplicationColumn.DisplayLayout.Override.TemplateAddRowAppearance = appearance90;
            this.cmbApplicationColumn.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.cmbApplicationColumn.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbApplicationColumn.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbApplicationColumn.DisplayMember = "";
            this.cmbApplicationColumn.DropDownWidth = 0;
            this.cmbApplicationColumn.Location = new System.Drawing.Point(286, 35);
            this.cmbApplicationColumn.Name = "cmbApplicationColumn";
            this.cmbApplicationColumn.Size = new System.Drawing.Size(150, 30);
            this.cmbApplicationColumn.TabIndex = 39;
            this.cmbApplicationColumn.Text = "ultraDropDown1";
            this.cmbApplicationColumn.ValueMember = "";
            this.cmbApplicationColumn.Visible = false;
            // 
            // bindingSourceTableType
            // 
            this.bindingSourceTableType.DataSource = typeof(Prana.PM.BLL.TableTypeList);
            // 
            // cmbTableType
            // 
            this.cmbTableType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbTableType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbTableType.DataSource = null;
            this.cmbTableType.DataSource = this.bindingSourceTableType;
            appearance91.BackColor = System.Drawing.SystemColors.Window;
            appearance91.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTableType.DisplayLayout.Appearance = appearance91;
            this.cmbTableType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbTableType.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbTableType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTableType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance92.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance92.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance92.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance92.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTableType.DisplayLayout.GroupByBox.Appearance = appearance92;
            appearance93.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTableType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance93;
            this.cmbTableType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance94.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance94.BackColor2 = System.Drawing.SystemColors.Control;
            appearance94.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance94.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTableType.DisplayLayout.GroupByBox.PromptAppearance = appearance94;
            this.cmbTableType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTableType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance95.BackColor = System.Drawing.SystemColors.Window;
            appearance95.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTableType.DisplayLayout.Override.ActiveCellAppearance = appearance95;
            appearance96.BackColor = System.Drawing.SystemColors.Highlight;
            appearance96.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTableType.DisplayLayout.Override.ActiveRowAppearance = appearance96;
            this.cmbTableType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTableType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance97.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTableType.DisplayLayout.Override.CardAreaAppearance = appearance97;
            appearance98.BorderColor = System.Drawing.Color.Silver;
            appearance98.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTableType.DisplayLayout.Override.CellAppearance = appearance98;
            this.cmbTableType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTableType.DisplayLayout.Override.CellPadding = 0;
            appearance99.BackColor = System.Drawing.SystemColors.Control;
            appearance99.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance99.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance99.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance99.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTableType.DisplayLayout.Override.GroupByRowAppearance = appearance99;
            appearance100.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbTableType.DisplayLayout.Override.HeaderAppearance = appearance100;
            this.cmbTableType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTableType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance101.BackColor = System.Drawing.SystemColors.Window;
            appearance101.BorderColor = System.Drawing.Color.Silver;
            this.cmbTableType.DisplayLayout.Override.RowAppearance = appearance101;
            this.cmbTableType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance102.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTableType.DisplayLayout.Override.TemplateAddRowAppearance = appearance102;
            this.cmbTableType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTableType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTableType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTableType.DisplayMember = "TableTypeName";
            this.cmbTableType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTableType.DropDownWidth = 0;
            this.cmbTableType.UseFlatMode = DefaultableBoolean.True;
            this.cmbTableType.Location = new System.Drawing.Point(325, 7);
            this.cmbTableType.MaxLength = 50;
            this.cmbTableType.Name = "cmbTableType";
            this.cmbTableType.Size = new System.Drawing.Size(150, 21);
            this.cmbTableType.TabIndex = 45;
            this.cmbTableType.ValueMember = "TableTypeID";
            this.cmbTableType.ValueChanged += new System.EventHandler(this.cmbTableType_ValueChanged);
            // 
            // lblTableType
            // 
            this.lblTableType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTableType.Location = new System.Drawing.Point(260, 13);
            this.lblTableType.Name = "lblTableType";
            this.lblTableType.Size = new System.Drawing.Size(62, 15);
            this.lblTableType.TabIndex = 44;
            this.lblTableType.Text = "Table Type";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // isLockingEnabled
            // 
            this.isLockingEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.isLockingEnabled.AutoSize = true;
            this.isLockingEnabled.Checked = true;
            this.isLockingEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isLockingEnabled.Location = new System.Drawing.Point(115, 34);
            this.isLockingEnabled.Name = "isLockingEnabled";
            this.isLockingEnabled.Size = new System.Drawing.Size(332, 17);
            this.isLockingEnabled.TabIndex = 46;
            this.isLockingEnabled.Text = "Map only one DataSource column with Single Application Column";
            this.isLockingEnabled.UseVisualStyleBackColor = true;
            this.isLockingEnabled.CheckedChanged += new System.EventHandler(this.isLockingEnabled_CheckedChanged);
            // 
            // CtrlMapColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.isLockingEnabled);
            this.Controls.Add(this.cmbTableType);
            this.Controls.Add(this.lblTableType);
            this.Controls.Add(this.cmbApplicationColumn);
            this.Controls.Add(this.cmbSourceColumn);
            this.Controls.Add(this.lblDataSourceName);
            this.Controls.Add(this.lblNameOfSource);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdMapColumns);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlMapColumns";
            this.Size = new System.Drawing.Size(494, 330);
            ((System.ComponentModel.ISupportInitialize)(this.grdMapColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplicationColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTableType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTableType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdMapColumns;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraLabel lblNameOfSource;
        private Infragistics.Win.Misc.UltraLabel lblDataSourceName;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbSourceColumn;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbApplicationColumn;
        private System.Windows.Forms.BindingSource bindingSourceTableType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTableType;
        private Infragistics.Win.Misc.UltraLabel lblTableType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox isLockingEnabled;

    }
}
