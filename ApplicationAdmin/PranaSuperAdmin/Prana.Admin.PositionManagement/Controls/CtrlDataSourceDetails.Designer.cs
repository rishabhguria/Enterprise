namespace Nirvana.Admin.PositionManagement.Controls
{
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using System.ComponentModel.Design;
    partial class CtrlDataSourceDetails
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("DataSourceType", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DataSourceTypeID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DataSourceTypeName");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlDataSourceDetails));
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("DataSourceStatus", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StatusId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            this.lblSourceType = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSourceType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.bindingSourceDataSourceTypeList = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblSourceTypeRequired = new Infragistics.Win.Misc.UltraLabel();
            this.errDataSource = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblSourceStatus = new Infragistics.Win.Misc.UltraLabel();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.cmbStatus = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.bindingSourceStatusList = new System.Windows.Forms.BindingSource(this.components);
            this.lblDataSourceName = new Infragistics.Win.Misc.UltraLabel();
            this.lblDataSourceNameHeading = new Infragistics.Win.Misc.UltraLabel();
            this.ctrlAddressDetails1 = new Nirvana.Admin.PositionManagement.Controls.CtrlAddressDetails();
            this.dsPrimaryContactDetails1 = new Nirvana.Admin.PositionManagement.Controls.CtrlDSPrimaryContactDetails();
            this.bindingSourceDataSource = new System.Windows.Forms.BindingSource(this.components);
            this.ctrlSourceName1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSourceName();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceTypeList)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceStatusList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSourceType
            // 
            this.lblSourceType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSourceType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSourceType.Location = new System.Drawing.Point(16, 40);
            this.lblSourceType.Name = "lblSourceType";
            this.lblSourceType.Size = new System.Drawing.Size(66, 15);
            this.lblSourceType.TabIndex = 14;
            this.lblSourceType.Text = "Source Type";
            // 
            // cmbSourceType
            // 
            this.cmbSourceType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbSourceType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbSourceType.DataSource = this.bindingSourceDataSourceTypeList;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSourceType.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbSourceType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbSourceType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSourceType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSourceType.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSourceType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbSourceType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSourceType.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbSourceType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSourceType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSourceType.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSourceType.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbSourceType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSourceType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSourceType.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSourceType.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbSourceType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSourceType.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSourceType.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbSourceType.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbSourceType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSourceType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbSourceType.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbSourceType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSourceType.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbSourceType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSourceType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSourceType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSourceType.DisplayMember = "DataSourceTypeName";
            this.cmbSourceType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSourceType.FlatMode = true;
            this.cmbSourceType.Location = new System.Drawing.Point(109, 35);
            this.cmbSourceType.Name = "cmbSourceType";
            this.cmbSourceType.Size = new System.Drawing.Size(150, 21);
            this.cmbSourceType.TabIndex = 2;
            this.cmbSourceType.ValueMember = "DataSourceTypeID";
            // 
            // bindingSourceDataSourceTypeList
            // 
            this.bindingSourceDataSourceTypeList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSourceTypeList);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(4, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 30);
            this.panel1.TabIndex = 25;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            this.btnClear.Appearance = appearance13;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(132, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnClose.Appearance = appearance14;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(233, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnSave.Appearance = appearance15;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(29, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSourceTypeRequired
            // 
            this.lblSourceTypeRequired.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance16.ForeColor = System.Drawing.Color.Red;
            this.lblSourceTypeRequired.Appearance = appearance16;
            this.lblSourceTypeRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSourceTypeRequired.Location = new System.Drawing.Point(93, 40);
            this.lblSourceTypeRequired.Name = "lblSourceTypeRequired";
            this.lblSourceTypeRequired.Size = new System.Drawing.Size(10, 15);
            this.lblSourceTypeRequired.TabIndex = 2;
            this.lblSourceTypeRequired.Text = "*";
            // 
            // errDataSource
            // 
            this.errDataSource.ContainerControl = this;
            this.errDataSource.DataSource = this.bindingSourceDataSource;
            // 
            // lblSourceStatus
            // 
            this.lblSourceStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSourceStatus.Location = new System.Drawing.Point(16, 67);
            this.lblSourceStatus.Name = "lblSourceStatus";
            this.lblSourceStatus.Size = new System.Drawing.Size(72, 20);
            this.lblSourceStatus.TabIndex = 26;
            this.lblSourceStatus.Text = "Status";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance29.Image = ((object)(resources.GetObject("appearance29.Image")));
            this.btnImport.Appearance = appearance29;
            this.btnImport.ImageSize = new System.Drawing.Size(75, 23);
            this.btnImport.Location = new System.Drawing.Point(272, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.ShowFocusRect = false;
            this.btnImport.Size = new System.Drawing.Size(68, 20);
            this.btnImport.TabIndex = 28;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbStatus.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbStatus.DataSource = this.bindingSourceStatusList;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStatus.DisplayLayout.Appearance = appearance17;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbStatus.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbStatus.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStatus.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.GroupByBox.Appearance = appearance18;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStatus.DisplayLayout.GroupByBox.BandLabelAppearance = appearance19;
            this.cmbStatus.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance20.BackColor2 = System.Drawing.SystemColors.Control;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStatus.DisplayLayout.GroupByBox.PromptAppearance = appearance20;
            this.cmbStatus.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStatus.DisplayLayout.MaxRowScrollRegions = 1;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            appearance21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStatus.DisplayLayout.Override.ActiveCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.SystemColors.Highlight;
            appearance22.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStatus.DisplayLayout.Override.ActiveRowAppearance = appearance22;
            this.cmbStatus.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbStatus.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.Override.CardAreaAppearance = appearance23;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStatus.DisplayLayout.Override.CellAppearance = appearance24;
            this.cmbStatus.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStatus.DisplayLayout.Override.CellPadding = 0;
            appearance25.BackColor = System.Drawing.SystemColors.Control;
            appearance25.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance25.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.Override.GroupByRowAppearance = appearance25;
            appearance26.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbStatus.DisplayLayout.Override.HeaderAppearance = appearance26;
            this.cmbStatus.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStatus.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.Color.Silver;
            this.cmbStatus.DisplayLayout.Override.RowAppearance = appearance27;
            this.cmbStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStatus.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.cmbStatus.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStatus.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStatus.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStatus.DisplayMember = "Name";
            this.cmbStatus.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbStatus.FlatMode = true;
            this.cmbStatus.Location = new System.Drawing.Point(109, 67);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 21);
            this.cmbStatus.TabIndex = 30;
            this.cmbStatus.ValueMember = "StatusId";
            // 
            // bindingSourceStatusList
            // 
            this.bindingSourceStatusList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSourceStatusList);
            // 
            // lblDataSourceName
            // 
            this.lblDataSourceName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDataSourceName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblDataSourceName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblDataSourceName.Location = new System.Drawing.Point(109, 7);
            this.lblDataSourceName.Name = "lblDataSourceName";
            this.lblDataSourceName.Size = new System.Drawing.Size(150, 20);
            this.lblDataSourceName.TabIndex = 32;
            // 
            // lblDataSourceNameHeading
            // 
            this.lblDataSourceNameHeading.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDataSourceNameHeading.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblDataSourceNameHeading.Location = new System.Drawing.Point(16, 11);
            this.lblDataSourceNameHeading.Name = "lblDataSourceNameHeading";
            this.lblDataSourceNameHeading.Size = new System.Drawing.Size(90, 15);
            this.lblDataSourceNameHeading.TabIndex = 33;
            this.lblDataSourceNameHeading.Text = "Name of Source";
            // 
            // ctrlAddressDetails1
            // 
            this.ctrlAddressDetails1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlAddressDetails1.DataMember = "";
            this.ctrlAddressDetails1.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.AddressDetails);
            this.ctrlAddressDetails1.Location = new System.Drawing.Point(16, 89);
            this.ctrlAddressDetails1.Name = "ctrlAddressDetails1";
            this.ctrlAddressDetails1.Size = new System.Drawing.Size(250, 188);
            this.ctrlAddressDetails1.TabIndex = 29;
            // 
            // dsPrimaryContactDetails1
            // 
            this.dsPrimaryContactDetails1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dsPrimaryContactDetails1.AutoSize = true;
            this.dsPrimaryContactDetails1.DataMember = "";
            this.dsPrimaryContactDetails1.DataSource = null;
            this.dsPrimaryContactDetails1.Location = new System.Drawing.Point(13, 283);
            this.dsPrimaryContactDetails1.Name = "dsPrimaryContactDetails1";
            this.dsPrimaryContactDetails1.Size = new System.Drawing.Size(257, 192);
            this.dsPrimaryContactDetails1.TabIndex = 0;
            // 
            // bindingSourceDataSource
            // 
            this.bindingSourceDataSource.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSource);
            // 
            // ctrlSourceName1
            // 
            this.ctrlSourceName1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlSourceName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSourceName1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSourceName1.Location = new System.Drawing.Point(51, 80);
            this.ctrlSourceName1.Name = "ctrlSourceName1";
            this.ctrlSourceName1.Size = new System.Drawing.Size(245, 27);
            this.ctrlSourceName1.TabIndex = 31;
            this.ctrlSourceName1.Visible = false;
            // 
            // CtrlDataSourceDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.lblDataSourceNameHeading);
            this.Controls.Add(this.lblDataSourceName);
            this.Controls.Add(this.ctrlSourceName1);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.ctrlAddressDetails1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.lblSourceStatus);
            this.Controls.Add(this.lblSourceTypeRequired);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dsPrimaryContactDetails1);
            this.Controls.Add(this.cmbSourceType);
            this.Controls.Add(this.lblSourceType);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlDataSourceDetails";
            this.Size = new System.Drawing.Size(345, 513);
            this.Load += new System.EventHandler(this.CtrlDataSourceDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSourceTypeList)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceStatusList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDataSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblSourceType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSourceType;
        private CtrlDSPrimaryContactDetails dsPrimaryContactDetails1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraLabel lblSourceTypeRequired;
        private System.Windows.Forms.ErrorProvider errDataSource;
        private System.Windows.Forms.BindingSource bindingSourceDataSource;
        private Infragistics.Win.Misc.UltraLabel lblSourceStatus;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private CtrlAddressDetails ctrlAddressDetails1;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private BindingSource bindingSourceDataSourceTypeList;
        private BindingSource bindingSourceStatusList;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbStatus;
        private Infragistics.Win.Misc.UltraLabel lblDataSourceName;
        private Infragistics.Win.Misc.UltraLabel lblDataSourceNameHeading;
        private CtrlSourceName ctrlSourceName1;

    }
}
