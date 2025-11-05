
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;
namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlImportSetup
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("ImportMethod", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlImportSetup));
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("ImportFormat", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            this.lblImportMethod = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectColumns = new Infragistics.Win.Misc.UltraLabel();
            this.lblMapColumns = new Infragistics.Win.Misc.UltraLabel();
            this.lblMapAUEC = new Infragistics.Win.Misc.UltraLabel();
            this.lblMapSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.cmbImportMethod = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.bindingSourceImportMethodList = new System.Windows.Forms.BindingSource(this.components);
            this.btnSelectColumns = new Infragistics.Win.Misc.UltraButton();
            this.btnMapColumns = new Infragistics.Win.Misc.UltraButton();
            this.btnMapAUEC = new Infragistics.Win.Misc.UltraButton();
            this.btnMapSymbol = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.lblFileFormat = new Infragistics.Win.Misc.UltraLabel();
            this.cmbFileFormat = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.bindingSourceImportFormatList = new System.Windows.Forms.BindingSource(this.components);
            this.btnMapFunds = new Infragistics.Win.Misc.UltraButton();
            this.lblMapFunds = new Infragistics.Win.Misc.UltraLabel();
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            this.bindingSourceDataSourceNameList = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceImportSetup = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportMethod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportMethodList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFileFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportFormatList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceNameList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportSetup)).BeginInit();
            this.SuspendLayout();
            // 
            // lblImportMethod
            // 
            this.lblImportMethod.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImportMethod.Location = new System.Drawing.Point(28, 45);
            this.lblImportMethod.Name = "lblImportMethod";
            this.lblImportMethod.Size = new System.Drawing.Size(100, 23);
            this.lblImportMethod.TabIndex = 1;
            this.lblImportMethod.Text = "Import Method";
            // 
            // lblSelectColumns
            // 
            this.lblSelectColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSelectColumns.Location = new System.Drawing.Point(28, 108);
            this.lblSelectColumns.Name = "lblSelectColumns";
            this.lblSelectColumns.Size = new System.Drawing.Size(80, 15);
            this.lblSelectColumns.TabIndex = 2;
            this.lblSelectColumns.Text = "Select Columns";
            // 
            // lblMapColumns
            // 
            this.lblMapColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMapColumns.Location = new System.Drawing.Point(28, 142);
            this.lblMapColumns.Name = "lblMapColumns";
            this.lblMapColumns.Size = new System.Drawing.Size(71, 15);
            this.lblMapColumns.TabIndex = 3;
            this.lblMapColumns.Text = "Map Columns";
            // 
            // lblMapAUEC
            // 
            this.lblMapAUEC.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMapAUEC.Location = new System.Drawing.Point(28, 171);
            this.lblMapAUEC.Name = "lblMapAUEC";
            this.lblMapAUEC.Size = new System.Drawing.Size(56, 15);
            this.lblMapAUEC.TabIndex = 4;
            this.lblMapAUEC.Text = "Map AUEC";
            this.lblMapAUEC.Click += new System.EventHandler(this.lblMapAUEC_Click);
            // 
            // lblMapSymbol
            // 
            this.lblMapSymbol.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMapSymbol.Location = new System.Drawing.Point(28, 200);
            this.lblMapSymbol.Name = "lblMapSymbol";
            this.lblMapSymbol.Size = new System.Drawing.Size(65, 15);
            this.lblMapSymbol.TabIndex = 5;
            this.lblMapSymbol.Text = "Map Symbol";
            // 
            // cmbImportMethod
            // 
            this.cmbImportMethod.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbImportMethod.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbImportMethod.DataSource = this.bindingSourceImportMethodList;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbImportMethod.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbImportMethod.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbImportMethod.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbImportMethod.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbImportMethod.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbImportMethod.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbImportMethod.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbImportMethod.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbImportMethod.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbImportMethod.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbImportMethod.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbImportMethod.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbImportMethod.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbImportMethod.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbImportMethod.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbImportMethod.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbImportMethod.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbImportMethod.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbImportMethod.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbImportMethod.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbImportMethod.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbImportMethod.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbImportMethod.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbImportMethod.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbImportMethod.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbImportMethod.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbImportMethod.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbImportMethod.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbImportMethod.DisplayMember = "Name";
            this.cmbImportMethod.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbImportMethod.DropDownWidth = 0;
            this.cmbImportMethod.Location = new System.Drawing.Point(122, 45);
            this.cmbImportMethod.Name = "cmbImportMethod";
            this.cmbImportMethod.Size = new System.Drawing.Size(150, 23);
            this.cmbImportMethod.TabIndex = 7;
            this.cmbImportMethod.ValueMember = "ID";
            // 
            // bindingSourceImportMethodList
            // 
            this.bindingSourceImportMethodList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.ImportMethodList);
            // 
            // btnSelectColumns
            // 
            this.btnSelectColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance13.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.select;
            this.btnSelectColumns.Appearance = appearance13;
            this.btnSelectColumns.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSelectColumns.Location = new System.Drawing.Point(122, 108);
            this.btnSelectColumns.Name = "btnSelectColumns";
            this.btnSelectColumns.ShowFocusRect = false;
            this.btnSelectColumns.ShowOutline = false;
            this.btnSelectColumns.Size = new System.Drawing.Size(75, 23);
            this.btnSelectColumns.TabIndex = 8;
            this.btnSelectColumns.Text = "Select";
            this.btnSelectColumns.Click += new System.EventHandler(this.btnSelectColumns_Click);
            // 
            // btnMapColumns
            // 
            this.btnMapColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance14.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.map;
            this.btnMapColumns.Appearance = appearance14;
            this.btnMapColumns.ImageSize = new System.Drawing.Size(75, 23);
            this.btnMapColumns.Location = new System.Drawing.Point(122, 137);
            this.btnMapColumns.Name = "btnMapColumns";
            this.btnMapColumns.ShowFocusRect = false;
            this.btnMapColumns.ShowOutline = false;
            this.btnMapColumns.Size = new System.Drawing.Size(75, 23);
            this.btnMapColumns.TabIndex = 9;
            this.btnMapColumns.Text = "Map";
            this.btnMapColumns.Click += new System.EventHandler(this.btnMapColumns_Click);
            // 
            // btnMapAUEC
            // 
            this.btnMapAUEC.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance15.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.map;
            this.btnMapAUEC.Appearance = appearance15;
            this.btnMapAUEC.ImageSize = new System.Drawing.Size(75, 23);
            this.btnMapAUEC.Location = new System.Drawing.Point(123, 166);
            this.btnMapAUEC.Name = "btnMapAUEC";
            this.btnMapAUEC.ShowFocusRect = false;
            this.btnMapAUEC.ShowOutline = false;
            this.btnMapAUEC.Size = new System.Drawing.Size(75, 23);
            this.btnMapAUEC.TabIndex = 10;
            this.btnMapAUEC.Text = "Map";
            this.btnMapAUEC.Click += new System.EventHandler(this.btnMapAUEC_Click);
            // 
            // btnMapSymbol
            // 
            this.btnMapSymbol.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance16.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.map;
            this.btnMapSymbol.Appearance = appearance16;
            this.btnMapSymbol.Enabled = false;
            this.btnMapSymbol.ImageSize = new System.Drawing.Size(75, 23);
            this.btnMapSymbol.Location = new System.Drawing.Point(122, 195);
            this.btnMapSymbol.Name = "btnMapSymbol";
            this.btnMapSymbol.ShowFocusRect = false;
            this.btnMapSymbol.ShowOutline = false;
            this.btnMapSymbol.Size = new System.Drawing.Size(75, 23);
            this.btnMapSymbol.TabIndex = 11;
            this.btnMapSymbol.Text = "Map";
            this.btnMapSymbol.Click += new System.EventHandler(this.btnMapSymbol_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance17.Image = ((object)(resources.GetObject("appearance17.Image")));
            this.btnSave.Appearance = appearance17;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(28, 271);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            this.btnClose.Appearance = appearance18;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(214, 271);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance32.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_clear;
            this.btnClear.Appearance = appearance32;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(121, 271);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 39;
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFileFormat.Location = new System.Drawing.Point(28, 74);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(100, 23);
            this.lblFileFormat.TabIndex = 40;
            this.lblFileFormat.Text = "File Format";
            // 
            // cmbFileFormat
            // 
            this.cmbFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbFileFormat.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbFileFormat.DataSource = this.bindingSourceImportFormatList;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            appearance20.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFileFormat.DisplayLayout.Appearance = appearance20;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand2.GroupHeadersVisible = false;
            this.cmbFileFormat.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbFileFormat.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFileFormat.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFileFormat.DisplayLayout.GroupByBox.Appearance = appearance21;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFileFormat.DisplayLayout.GroupByBox.BandLabelAppearance = appearance22;
            this.cmbFileFormat.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance23.BackColor2 = System.Drawing.SystemColors.Control;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFileFormat.DisplayLayout.GroupByBox.PromptAppearance = appearance23;
            this.cmbFileFormat.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFileFormat.DisplayLayout.MaxRowScrollRegions = 1;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFileFormat.DisplayLayout.Override.ActiveCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.SystemColors.Highlight;
            appearance25.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFileFormat.DisplayLayout.Override.ActiveRowAppearance = appearance25;
            this.cmbFileFormat.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFileFormat.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFileFormat.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Silver;
            appearance27.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFileFormat.DisplayLayout.Override.CellAppearance = appearance27;
            this.cmbFileFormat.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFileFormat.DisplayLayout.Override.CellPadding = 0;
            appearance28.BackColor = System.Drawing.SystemColors.Control;
            appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance28.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFileFormat.DisplayLayout.Override.GroupByRowAppearance = appearance28;
            appearance29.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbFileFormat.DisplayLayout.Override.HeaderAppearance = appearance29;
            this.cmbFileFormat.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFileFormat.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance30.BackColor = System.Drawing.SystemColors.Window;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            this.cmbFileFormat.DisplayLayout.Override.RowAppearance = appearance30;
            this.cmbFileFormat.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance31.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFileFormat.DisplayLayout.Override.TemplateAddRowAppearance = appearance31;
            this.cmbFileFormat.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFileFormat.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFileFormat.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFileFormat.DisplayMember = "Name";
            this.cmbFileFormat.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFileFormat.DropDownWidth = 0;
            this.cmbFileFormat.Location = new System.Drawing.Point(122, 74);
            this.cmbFileFormat.Name = "cmbFileFormat";
            this.cmbFileFormat.Size = new System.Drawing.Size(150, 23);
            this.cmbFileFormat.TabIndex = 41;
            this.cmbFileFormat.ValueMember = "ID";
            // 
            // bindingSourceImportFormatList
            // 
            this.bindingSourceImportFormatList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.ImportFormatList);
            // 
            // btnMapFunds
            // 
            this.btnMapFunds.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance19.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.map;
            this.btnMapFunds.Appearance = appearance19;
            this.btnMapFunds.ImageSize = new System.Drawing.Size(75, 23);
            this.btnMapFunds.Location = new System.Drawing.Point(122, 224);
            this.btnMapFunds.Name = "btnMapFunds";
            this.btnMapFunds.ShowFocusRect = false;
            this.btnMapFunds.ShowOutline = false;
            this.btnMapFunds.Size = new System.Drawing.Size(75, 23);
            this.btnMapFunds.TabIndex = 43;
            this.btnMapFunds.Text = "Map";
            this.btnMapFunds.Click += new System.EventHandler(this.btnMapFunds_Click);
            // 
            // lblMapFunds
            // 
            this.lblMapFunds.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMapFunds.Location = new System.Drawing.Point(28, 229);
            this.lblMapFunds.Name = "lblMapFunds";
            this.lblMapFunds.Size = new System.Drawing.Size(59, 15);
            this.lblMapFunds.TabIndex = 42;
            this.lblMapFunds.Text = "Map Funds";
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(28, 12);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(245, 29);
            this.ctrlSourceName1.TabIndex = 44;
            // 
            // bindingSourceDataSourceNameList
            // 
            this.bindingSourceDataSourceNameList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSourceNameIDList);
            // 
            // bindingSourceImportSetup
            // 
            this.bindingSourceImportSetup.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.ImportSetup);
            // 
            // CtrlImportSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ctrlSourceName1);
            this.Controls.Add(this.btnMapFunds);
            this.Controls.Add(this.lblMapFunds);
            this.Controls.Add(this.cmbFileFormat);
            this.Controls.Add(this.lblFileFormat);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnMapSymbol);
            this.Controls.Add(this.btnMapAUEC);
            this.Controls.Add(this.btnMapColumns);
            this.Controls.Add(this.btnSelectColumns);
            this.Controls.Add(this.cmbImportMethod);
            this.Controls.Add(this.lblMapSymbol);
            this.Controls.Add(this.lblMapAUEC);
            this.Controls.Add(this.lblMapColumns);
            this.Controls.Add(this.lblSelectColumns);
            this.Controls.Add(this.lblImportMethod);
            this.Enabled = true;
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlImportSetup";
            this.Size = new System.Drawing.Size(293, 314);
            this.Load += new System.EventHandler(this.CtrlImportSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportMethod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportMethodList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFileFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportFormatList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceNameList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImportSetup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblImportMethod;
        private Infragistics.Win.Misc.UltraLabel lblSelectColumns;
        private Infragistics.Win.Misc.UltraLabel lblMapColumns;
        private Infragistics.Win.Misc.UltraLabel lblMapAUEC;
        private Infragistics.Win.Misc.UltraLabel lblMapSymbol;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbImportMethod;
        private Infragistics.Win.Misc.UltraButton btnSelectColumns;
        private Infragistics.Win.Misc.UltraButton btnMapColumns;
        private Infragistics.Win.Misc.UltraButton btnMapAUEC;
        private Infragistics.Win.Misc.UltraButton btnMapSymbol;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.BindingSource bindingSourceImportSetup;
        private System.Windows.Forms.BindingSource bindingSourceDataSourceNameList;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraLabel lblFileFormat;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFileFormat;
        private System.Windows.Forms.BindingSource bindingSourceImportMethodList;
        private System.Windows.Forms.BindingSource bindingSourceImportFormatList;
        private Infragistics.Win.Misc.UltraButton btnMapFunds;
        private Infragistics.Win.Misc.UltraLabel lblMapFunds;
        private CtrlSourceName ctrlSourceName1;
    }
}
