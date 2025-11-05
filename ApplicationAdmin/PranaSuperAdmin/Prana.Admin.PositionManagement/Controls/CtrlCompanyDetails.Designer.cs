using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;
namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlCompanyDetails
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("CompanyType", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTypeID");
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
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("User", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Password");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCompanyDetails));
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            this.txtLabel = new Infragistics.Win.Misc.UltraLabel();
            this.txtCompanyFullName = new System.Windows.Forms.TextBox();
            this.lblShortName = new Infragistics.Win.Misc.UltraLabel();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtfilePath = new System.Windows.Forms.TextBox();
            this.lblLogo = new Infragistics.Win.Misc.UltraLabel();
            this.lblShortNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblFullNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmbCompanyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblCompanyType = new Infragistics.Win.Misc.UltraLabel();
            this.grpLicensing = new Infragistics.Win.Misc.UltraGroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblLicences = new Infragistics.Win.Misc.UltraLabel();
            this.grpAdminContact = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblUserNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblFaxNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtFaxNumber = new System.Windows.Forms.TextBox();
            this.lblFaxNumber = new Infragistics.Win.Misc.UltraLabel();
            this.lblHomeNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtHomeNumberRequired = new System.Windows.Forms.TextBox();
            this.lblHomeNumber = new Infragistics.Win.Misc.UltraLabel();
            this.lblPagerNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtPagerNumber = new System.Windows.Forms.TextBox();
            this.lblPagerNumber = new Infragistics.Win.Misc.UltraLabel();
            this.lblCellNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblPasswordRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblLoginNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblPassword = new Infragistics.Win.Misc.UltraLabel();
            this.lblLoginName = new Infragistics.Win.Misc.UltraLabel();
            this.cmbUsers = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblUserName = new Infragistics.Win.Misc.UltraLabel();
            this.lblWorkNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblEmailRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblLastNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblFirstNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtACCellNumber = new System.Windows.Forms.TextBox();
            this.lblACCellNumber = new Infragistics.Win.Misc.UltraLabel();
            this.txtACWorkNumber = new System.Windows.Forms.TextBox();
            this.lblACWorkNumber = new Infragistics.Win.Misc.UltraLabel();
            this.txtACEmail = new System.Windows.Forms.TextBox();
            this.lblACEmail = new Infragistics.Win.Misc.UltraLabel();
            this.txtACLastName = new System.Windows.Forms.TextBox();
            this.lblACLastName = new Infragistics.Win.Misc.UltraLabel();
            this.txtACTitle = new System.Windows.Forms.TextBox();
            this.txtACFirstName = new System.Windows.Forms.TextBox();
            this.lblACTitle = new Infragistics.Win.Misc.UltraLabel();
            this.lblACFirstName = new Infragistics.Win.Misc.UltraLabel();
            this.lblPCTitle = new Infragistics.Win.Misc.UltraLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnUploadLogo = new Infragistics.Win.Misc.UltraButton();
            this.ctrlAddressDetails1 = new Nirvana.Admin.PositionManagement.Controls.CtrlAddressDetails();
            this.bindingSourceUserList = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceCompanyTypeList = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceCompanyDetails = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpLicensing)).BeginInit();
            this.grpLicensing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpAdminContact)).BeginInit();
            this.grpAdminContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyTypeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLabel
            // 
            this.txtLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLabel.Location = new System.Drawing.Point(2, 20);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(45, 17);
            this.txtLabel.TabIndex = 0;
            this.txtLabel.Text = "Name";
            // 
            // txtCompanyFullName
            // 
            this.txtCompanyFullName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCompanyFullName.Location = new System.Drawing.Point(95, 17);
            this.txtCompanyFullName.MaxLength = 50;
            this.txtCompanyFullName.Name = "txtCompanyFullName";
            this.txtCompanyFullName.Size = new System.Drawing.Size(150, 21);
            this.txtCompanyFullName.TabIndex = 1;
            // 
            // lblShortName
            // 
            this.lblShortName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblShortName.Location = new System.Drawing.Point(1, 45);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(65, 17);
            this.lblShortName.TabIndex = 14;
            this.lblShortName.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtShortName.Location = new System.Drawing.Point(95, 43);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(150, 21);
            this.txtShortName.TabIndex = 15;
            // 
            // txtfilePath
            // 
            this.txtfilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtfilePath.Location = new System.Drawing.Point(95, 69);
            this.txtfilePath.MaxLength = 50;
            this.txtfilePath.Name = "txtfilePath";
            this.txtfilePath.Size = new System.Drawing.Size(123, 21);
            this.txtfilePath.TabIndex = 34;
            // 
            // lblLogo
            // 
            this.lblLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLogo.Location = new System.Drawing.Point(3, 72);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(53, 15);
            this.lblLogo.TabIndex = 35;
            this.lblLogo.Text = "Logo";
            // 
            // lblShortNameRequired
            // 
            this.lblShortNameRequired.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.lblShortNameRequired.Appearance = appearance1;
            this.lblShortNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortNameRequired.Location = new System.Drawing.Point(81, 46);
            this.lblShortNameRequired.Name = "lblShortNameRequired";
            this.lblShortNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblShortNameRequired.TabIndex = 36;
            this.lblShortNameRequired.Text = "*";
            // 
            // lblFullNameRequired
            // 
            this.lblFullNameRequired.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance2.ForeColor = System.Drawing.Color.Red;
            this.lblFullNameRequired.Appearance = appearance2;
            this.lblFullNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFullNameRequired.Location = new System.Drawing.Point(81, 16);
            this.lblFullNameRequired.Name = "lblFullNameRequired";
            this.lblFullNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblFullNameRequired.TabIndex = 37;
            this.lblFullNameRequired.Text = "*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.InitialDirectory = "c:\\\\";
            // 
            // cmbCompanyType
            // 
            this.cmbCompanyType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbCompanyType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbCompanyType.DataSource = this.bindingSourceCompanyTypeList;
            appearance3.BackColor = System.Drawing.SystemColors.Window;
            appearance3.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanyType.DisplayLayout.Appearance = appearance3;
            this.cmbCompanyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbCompanyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCompanyType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyType.DisplayLayout.GroupByBox.Appearance = appearance4;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance5;
            this.cmbCompanyType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance6.BackColor2 = System.Drawing.SystemColors.Control;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyType.DisplayLayout.GroupByBox.PromptAppearance = appearance6;
            this.cmbCompanyType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanyType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanyType.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.Highlight;
            appearance8.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanyType.DisplayLayout.Override.ActiveRowAppearance = appearance8;
            this.cmbCompanyType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyType.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanyType.DisplayLayout.Override.CellAppearance = appearance10;
            this.cmbCompanyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanyType.DisplayLayout.Override.CellPadding = 0;
            appearance11.BackColor = System.Drawing.SystemColors.Control;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyType.DisplayLayout.Override.GroupByRowAppearance = appearance11;
            appearance12.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCompanyType.DisplayLayout.Override.HeaderAppearance = appearance12;
            this.cmbCompanyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCompanyType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyType.DisplayLayout.Override.RowAppearance = appearance13;
            this.cmbCompanyType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanyType.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this.cmbCompanyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanyType.DisplayMember = "Type";
            this.cmbCompanyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyType.DropDownWidth = 0;
            this.cmbCompanyType.FlatMode = true;
            this.cmbCompanyType.Location = new System.Drawing.Point(95, 95);
            this.cmbCompanyType.MaxLength = 50;
            this.cmbCompanyType.Name = "cmbCompanyType";
            this.cmbCompanyType.Size = new System.Drawing.Size(150, 21);
            this.cmbCompanyType.TabIndex = 39;
            this.cmbCompanyType.ValueMember = "CompanyTypeID";
            // 
            // lblCompanyType
            // 
            this.lblCompanyType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCompanyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCompanyType.Location = new System.Drawing.Point(1, 98);
            this.lblCompanyType.Name = "lblCompanyType";
            this.lblCompanyType.Size = new System.Drawing.Size(80, 15);
            this.lblCompanyType.TabIndex = 40;
            this.lblCompanyType.Text = "Company Type";
            // 
            // grpLicensing
            // 
            this.grpLicensing.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpLicensing.Controls.Add(this.numericUpDown1);
            this.grpLicensing.Controls.Add(this.lblLicences);
            this.grpLicensing.Location = new System.Drawing.Point(4, 301);
            this.grpLicensing.Name = "grpLicensing";
            this.grpLicensing.Size = new System.Drawing.Size(243, 50);
            this.grpLicensing.SupportThemes = false;
            this.grpLicensing.TabIndex = 41;
            this.grpLicensing.Text = "Licensing";
            this.grpLicensing.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(117, 22);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(94, 21);
            this.numericUpDown1.TabIndex = 42;
            // 
            // lblLicences
            // 
            this.lblLicences.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLicences.Location = new System.Drawing.Point(9, 24);
            this.lblLicences.Name = "lblLicences";
            this.lblLicences.Size = new System.Drawing.Size(103, 15);
            this.lblLicences.TabIndex = 41;
            this.lblLicences.Text = "No. of User Licences";
            // 
            // grpAdminContact
            // 
            this.grpAdminContact.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpAdminContact.Controls.Add(this.lblUserNameRequired);
            this.grpAdminContact.Controls.Add(this.lblFaxNumberRequired);
            this.grpAdminContact.Controls.Add(this.txtFaxNumber);
            this.grpAdminContact.Controls.Add(this.lblFaxNumber);
            this.grpAdminContact.Controls.Add(this.lblHomeNumberRequired);
            this.grpAdminContact.Controls.Add(this.txtHomeNumberRequired);
            this.grpAdminContact.Controls.Add(this.lblHomeNumber);
            this.grpAdminContact.Controls.Add(this.lblPagerNumberRequired);
            this.grpAdminContact.Controls.Add(this.txtPagerNumber);
            this.grpAdminContact.Controls.Add(this.lblPagerNumber);
            this.grpAdminContact.Controls.Add(this.lblCellNumberRequired);
            this.grpAdminContact.Controls.Add(this.txtPassword);
            this.grpAdminContact.Controls.Add(this.txtLogin);
            this.grpAdminContact.Controls.Add(this.lblPasswordRequired);
            this.grpAdminContact.Controls.Add(this.lblLoginNameRequired);
            this.grpAdminContact.Controls.Add(this.lblPassword);
            this.grpAdminContact.Controls.Add(this.lblLoginName);
            this.grpAdminContact.Controls.Add(this.cmbUsers);
            this.grpAdminContact.Controls.Add(this.lblUserName);
            this.grpAdminContact.Controls.Add(this.lblWorkNumberRequired);
            this.grpAdminContact.Controls.Add(this.lblEmailRequired);
            this.grpAdminContact.Controls.Add(this.lblLastNameRequired);
            this.grpAdminContact.Controls.Add(this.lblFirstNameRequired);
            this.grpAdminContact.Controls.Add(this.txtACCellNumber);
            this.grpAdminContact.Controls.Add(this.lblACCellNumber);
            this.grpAdminContact.Controls.Add(this.txtACWorkNumber);
            this.grpAdminContact.Controls.Add(this.lblACWorkNumber);
            this.grpAdminContact.Controls.Add(this.txtACEmail);
            this.grpAdminContact.Controls.Add(this.lblACEmail);
            this.grpAdminContact.Controls.Add(this.txtACLastName);
            this.grpAdminContact.Controls.Add(this.lblACLastName);
            this.grpAdminContact.Controls.Add(this.txtACTitle);
            this.grpAdminContact.Controls.Add(this.txtACFirstName);
            this.grpAdminContact.Controls.Add(this.lblACTitle);
            this.grpAdminContact.Controls.Add(this.lblACFirstName);
            this.grpAdminContact.Controls.Add(this.lblPCTitle);
            this.grpAdminContact.Location = new System.Drawing.Point(246, 5);
            this.grpAdminContact.Name = "grpAdminContact";
            this.grpAdminContact.Size = new System.Drawing.Size(248, 346);
            this.grpAdminContact.SupportThemes = false;
            this.grpAdminContact.TabIndex = 43;
            this.grpAdminContact.Text = "Admin Contact";
            this.grpAdminContact.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            // 
            // lblUserNameRequired
            // 
            appearance15.ForeColor = System.Drawing.Color.Red;
            this.lblUserNameRequired.Appearance = appearance15;
            this.lblUserNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserNameRequired.Location = new System.Drawing.Point(71, 102);
            this.lblUserNameRequired.Name = "lblUserNameRequired";
            this.lblUserNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblUserNameRequired.TabIndex = 86;
            this.lblUserNameRequired.Text = "*";
            // 
            // lblFaxNumberRequired
            // 
            appearance16.ForeColor = System.Drawing.Color.Red;
            this.lblFaxNumberRequired.Appearance = appearance16;
            this.lblFaxNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFaxNumberRequired.Location = new System.Drawing.Point(71, 312);
            this.lblFaxNumberRequired.Name = "lblFaxNumberRequired";
            this.lblFaxNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblFaxNumberRequired.TabIndex = 85;
            this.lblFaxNumberRequired.Text = "*";
            // 
            // txtFaxNumber
            // 
            this.txtFaxNumber.Location = new System.Drawing.Point(85, 308);
            this.txtFaxNumber.Name = "txtFaxNumber";
            this.txtFaxNumber.Size = new System.Drawing.Size(150, 21);
            this.txtFaxNumber.TabIndex = 83;
            // 
            // lblFaxNumber
            // 
            this.lblFaxNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFaxNumber.Location = new System.Drawing.Point(6, 311);
            this.lblFaxNumber.Name = "lblFaxNumber";
            this.lblFaxNumber.Size = new System.Drawing.Size(74, 15);
            this.lblFaxNumber.TabIndex = 84;
            this.lblFaxNumber.Text = "Fax #";
            // 
            // lblHomeNumberRequired
            // 
            appearance17.ForeColor = System.Drawing.Color.Red;
            this.lblHomeNumberRequired.Appearance = appearance17;
            this.lblHomeNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHomeNumberRequired.Location = new System.Drawing.Point(71, 288);
            this.lblHomeNumberRequired.Name = "lblHomeNumberRequired";
            this.lblHomeNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblHomeNumberRequired.TabIndex = 82;
            this.lblHomeNumberRequired.Text = "*";
            // 
            // txtHomeNumberRequired
            // 
            this.txtHomeNumberRequired.Location = new System.Drawing.Point(85, 282);
            this.txtHomeNumberRequired.Name = "txtHomeNumberRequired";
            this.txtHomeNumberRequired.Size = new System.Drawing.Size(150, 21);
            this.txtHomeNumberRequired.TabIndex = 80;
            // 
            // lblHomeNumber
            // 
            this.lblHomeNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHomeNumber.Location = new System.Drawing.Point(6, 285);
            this.lblHomeNumber.Name = "lblHomeNumber";
            this.lblHomeNumber.Size = new System.Drawing.Size(74, 15);
            this.lblHomeNumber.TabIndex = 81;
            this.lblHomeNumber.Text = "Home #";
            // 
            // lblPagerNumberRequired
            // 
            appearance18.ForeColor = System.Drawing.Color.Red;
            this.lblPagerNumberRequired.Appearance = appearance18;
            this.lblPagerNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPagerNumberRequired.Location = new System.Drawing.Point(71, 260);
            this.lblPagerNumberRequired.Name = "lblPagerNumberRequired";
            this.lblPagerNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblPagerNumberRequired.TabIndex = 79;
            this.lblPagerNumberRequired.Text = "*";
            // 
            // txtPagerNumber
            // 
            this.txtPagerNumber.Location = new System.Drawing.Point(86, 256);
            this.txtPagerNumber.Name = "txtPagerNumber";
            this.txtPagerNumber.Size = new System.Drawing.Size(150, 21);
            this.txtPagerNumber.TabIndex = 77;
            // 
            // lblPagerNumber
            // 
            this.lblPagerNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPagerNumber.Location = new System.Drawing.Point(6, 259);
            this.lblPagerNumber.Name = "lblPagerNumber";
            this.lblPagerNumber.Size = new System.Drawing.Size(74, 15);
            this.lblPagerNumber.TabIndex = 78;
            this.lblPagerNumber.Text = "Pager #";
            // 
            // lblCellNumberRequired
            // 
            appearance19.ForeColor = System.Drawing.Color.Red;
            this.lblCellNumberRequired.Appearance = appearance19;
            this.lblCellNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCellNumberRequired.Location = new System.Drawing.Point(71, 234);
            this.lblCellNumberRequired.Name = "lblCellNumberRequired";
            this.lblCellNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblCellNumberRequired.TabIndex = 76;
            this.lblCellNumberRequired.Text = "*";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(85, 152);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 21);
            this.txtPassword.TabIndex = 75;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(85, 126);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(150, 21);
            this.txtLogin.TabIndex = 74;
            // 
            // lblPasswordRequired
            // 
            appearance20.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordRequired.Appearance = appearance20;
            this.lblPasswordRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPasswordRequired.Location = new System.Drawing.Point(71, 149);
            this.lblPasswordRequired.Name = "lblPasswordRequired";
            this.lblPasswordRequired.Size = new System.Drawing.Size(10, 15);
            this.lblPasswordRequired.TabIndex = 73;
            this.lblPasswordRequired.Text = "*";
            // 
            // lblLoginNameRequired
            // 
            appearance21.ForeColor = System.Drawing.Color.Red;
            this.lblLoginNameRequired.Appearance = appearance21;
            this.lblLoginNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLoginNameRequired.Location = new System.Drawing.Point(71, 126);
            this.lblLoginNameRequired.Name = "lblLoginNameRequired";
            this.lblLoginNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblLoginNameRequired.TabIndex = 72;
            this.lblLoginNameRequired.Text = "*";
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(6, 151);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(66, 23);
            this.lblPassword.TabIndex = 71;
            this.lblPassword.Text = "Password";
            // 
            // lblLoginName
            // 
            this.lblLoginName.Location = new System.Drawing.Point(5, 125);
            this.lblLoginName.Name = "lblLoginName";
            this.lblLoginName.Size = new System.Drawing.Size(66, 23);
            this.lblLoginName.TabIndex = 70;
            this.lblLoginName.Text = "Login Name";
            // 
            // cmbUsers
            // 
            this.cmbUsers.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbUsers.DataSource = this.bindingSourceUserList;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUsers.DisplayLayout.Appearance = appearance22;
            this.cmbUsers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn5.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbUsers.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbUsers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUsers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance23.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUsers.DisplayLayout.GroupByBox.Appearance = appearance23;
            appearance24.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUsers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance24;
            this.cmbUsers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUsers.DisplayLayout.GroupByBox.PromptAppearance = appearance25;
            this.cmbUsers.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUsers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            appearance26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUsers.DisplayLayout.Override.ActiveCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.SystemColors.Highlight;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUsers.DisplayLayout.Override.ActiveRowAppearance = appearance27;
            this.cmbUsers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUsers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUsers.DisplayLayout.Override.CardAreaAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            appearance29.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUsers.DisplayLayout.Override.CellAppearance = appearance29;
            this.cmbUsers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUsers.DisplayLayout.Override.CellPadding = 0;
            appearance30.BackColor = System.Drawing.SystemColors.Control;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUsers.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            appearance31.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbUsers.DisplayLayout.Override.HeaderAppearance = appearance31;
            this.cmbUsers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUsers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            this.cmbUsers.DisplayLayout.Override.RowAppearance = appearance32;
            this.cmbUsers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUsers.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.cmbUsers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUsers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUsers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUsers.DisplayMember = "UserName";
            this.cmbUsers.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUsers.DropDownWidth = 0;
            this.cmbUsers.FlatMode = true;
            this.cmbUsers.Location = new System.Drawing.Point(85, 100);
            this.cmbUsers.MaxLength = 50;
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(150, 21);
            this.cmbUsers.TabIndex = 69;
            this.cmbUsers.ValueMember = "ID";
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUserName.Location = new System.Drawing.Point(7, 103);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(62, 15);
            this.lblUserName.TabIndex = 68;
            this.lblUserName.Text = "User Name";
            // 
            // lblWorkNumberRequired
            // 
            appearance34.ForeColor = System.Drawing.Color.Red;
            this.lblWorkNumberRequired.Appearance = appearance34;
            this.lblWorkNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblWorkNumberRequired.Location = new System.Drawing.Point(71, 207);
            this.lblWorkNumberRequired.Name = "lblWorkNumberRequired";
            this.lblWorkNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblWorkNumberRequired.TabIndex = 67;
            this.lblWorkNumberRequired.Text = "*";
            // 
            // lblEmailRequired
            // 
            appearance35.ForeColor = System.Drawing.Color.Red;
            this.lblEmailRequired.Appearance = appearance35;
            this.lblEmailRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmailRequired.Location = new System.Drawing.Point(71, 181);
            this.lblEmailRequired.Name = "lblEmailRequired";
            this.lblEmailRequired.Size = new System.Drawing.Size(10, 15);
            this.lblEmailRequired.TabIndex = 66;
            this.lblEmailRequired.Text = "*";
            // 
            // lblLastNameRequired
            // 
            appearance36.ForeColor = System.Drawing.Color.Red;
            this.lblLastNameRequired.Appearance = appearance36;
            this.lblLastNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLastNameRequired.Location = new System.Drawing.Point(71, 51);
            this.lblLastNameRequired.Name = "lblLastNameRequired";
            this.lblLastNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblLastNameRequired.TabIndex = 65;
            this.lblLastNameRequired.Text = "*";
            // 
            // lblFirstNameRequired
            // 
            appearance37.ForeColor = System.Drawing.Color.Red;
            this.lblFirstNameRequired.Appearance = appearance37;
            this.lblFirstNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFirstNameRequired.Location = new System.Drawing.Point(71, 22);
            this.lblFirstNameRequired.Name = "lblFirstNameRequired";
            this.lblFirstNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblFirstNameRequired.TabIndex = 64;
            this.lblFirstNameRequired.Text = "*";
            // 
            // txtACCellNumber
            // 
            this.txtACCellNumber.Location = new System.Drawing.Point(86, 230);
            this.txtACCellNumber.Name = "txtACCellNumber";
            this.txtACCellNumber.Size = new System.Drawing.Size(150, 21);
            this.txtACCellNumber.TabIndex = 57;
            // 
            // lblACCellNumber
            // 
            this.lblACCellNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACCellNumber.Location = new System.Drawing.Point(6, 233);
            this.lblACCellNumber.Name = "lblACCellNumber";
            this.lblACCellNumber.Size = new System.Drawing.Size(66, 15);
            this.lblACCellNumber.TabIndex = 63;
            this.lblACCellNumber.Text = "Cell #";
            // 
            // txtACWorkNumber
            // 
            this.txtACWorkNumber.Location = new System.Drawing.Point(85, 204);
            this.txtACWorkNumber.Name = "txtACWorkNumber";
            this.txtACWorkNumber.Size = new System.Drawing.Size(150, 21);
            this.txtACWorkNumber.TabIndex = 56;
            // 
            // lblACWorkNumber
            // 
            this.lblACWorkNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACWorkNumber.Location = new System.Drawing.Point(5, 207);
            this.lblACWorkNumber.Name = "lblACWorkNumber";
            this.lblACWorkNumber.Size = new System.Drawing.Size(74, 15);
            this.lblACWorkNumber.TabIndex = 62;
            this.lblACWorkNumber.Text = "Work #";
            // 
            // txtACEmail
            // 
            this.txtACEmail.Location = new System.Drawing.Point(85, 178);
            this.txtACEmail.Name = "txtACEmail";
            this.txtACEmail.Size = new System.Drawing.Size(150, 21);
            this.txtACEmail.TabIndex = 55;
            // 
            // lblACEmail
            // 
            this.lblACEmail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACEmail.Location = new System.Drawing.Point(6, 181);
            this.lblACEmail.Name = "lblACEmail";
            this.lblACEmail.Size = new System.Drawing.Size(34, 15);
            this.lblACEmail.TabIndex = 61;
            this.lblACEmail.Text = "E-Mail";
            // 
            // txtACLastName
            // 
            this.txtACLastName.Location = new System.Drawing.Point(85, 48);
            this.txtACLastName.Name = "txtACLastName";
            this.txtACLastName.Size = new System.Drawing.Size(150, 21);
            this.txtACLastName.TabIndex = 53;
            // 
            // lblACLastName
            // 
            this.lblACLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACLastName.Location = new System.Drawing.Point(6, 51);
            this.lblACLastName.Name = "lblACLastName";
            this.lblACLastName.Size = new System.Drawing.Size(57, 15);
            this.lblACLastName.TabIndex = 59;
            this.lblACLastName.Text = "Last Name";
            // 
            // txtACTitle
            // 
            this.txtACTitle.Location = new System.Drawing.Point(85, 74);
            this.txtACTitle.Name = "txtACTitle";
            this.txtACTitle.Size = new System.Drawing.Size(150, 21);
            this.txtACTitle.TabIndex = 54;
            // 
            // txtACFirstName
            // 
            this.txtACFirstName.Location = new System.Drawing.Point(85, 23);
            this.txtACFirstName.Name = "txtACFirstName";
            this.txtACFirstName.Size = new System.Drawing.Size(150, 21);
            this.txtACFirstName.TabIndex = 52;
            // 
            // lblACTitle
            // 
            this.lblACTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACTitle.Location = new System.Drawing.Point(6, 77);
            this.lblACTitle.Name = "lblACTitle";
            this.lblACTitle.Size = new System.Drawing.Size(26, 15);
            this.lblACTitle.TabIndex = 60;
            this.lblACTitle.Text = "Title";
            // 
            // lblACFirstName
            // 
            this.lblACFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACFirstName.Location = new System.Drawing.Point(6, 25);
            this.lblACFirstName.Name = "lblACFirstName";
            this.lblACFirstName.Size = new System.Drawing.Size(58, 17);
            this.lblACFirstName.TabIndex = 58;
            this.lblACFirstName.Text = "First Name";
            // 
            // lblPCTitle
            // 
            this.lblPCTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCTitle.Location = new System.Drawing.Point(-29, 105);
            this.lblPCTitle.Name = "lblPCTitle";
            this.lblPCTitle.Size = new System.Drawing.Size(26, 15);
            this.lblPCTitle.TabIndex = 51;
            this.lblPCTitle.Text = "Title";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.ultraButton1);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(81, 357);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 30);
            this.panel1.TabIndex = 47;
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance38.Image = ((object)(resources.GetObject("appearance38.Image")));
            this.ultraButton1.Appearance = appearance38;
            this.ultraButton1.ImageSize = new System.Drawing.Size(75, 23);
            this.ultraButton1.Location = new System.Drawing.Point(131, 4);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.ShowFocusRect = false;
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance39.Image = ((object)(resources.GetObject("appearance39.Image")));
            this.btnClear.Appearance = appearance39;
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(132, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance40.Image = ((object)(resources.GetObject("appearance40.Image")));
            this.btnClose.Appearance = appearance40;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(233, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance41.Image = ((object)(resources.GetObject("appearance41.Image")));
            this.btnSave.Appearance = appearance41;
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
            // btnUploadLogo
            // 
            this.btnUploadLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance42.Image = ((object)(resources.GetObject("appearance42.Image")));
            this.btnUploadLogo.Appearance = appearance42;
            this.btnUploadLogo.Location = new System.Drawing.Point(224, 72);
            this.btnUploadLogo.Name = "btnUploadLogo";
            this.btnUploadLogo.ShowFocusRect = false;
            this.btnUploadLogo.ShowOutline = false;
            this.btnUploadLogo.Size = new System.Drawing.Size(16, 16);
            this.btnUploadLogo.TabIndex = 48;
            this.btnUploadLogo.Click += new System.EventHandler(this.btnUploadLogo_Click);
            // 
            // ctrlAddressDetails1
            // 
            this.ctrlAddressDetails1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlAddressDetails1.DataMember = "";
            this.ctrlAddressDetails1.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.AddressDetails);
            this.ctrlAddressDetails1.Location = new System.Drawing.Point(0, 117);
            this.ctrlAddressDetails1.Name = "ctrlAddressDetails1";
            this.ctrlAddressDetails1.Size = new System.Drawing.Size(245, 188);
            this.ctrlAddressDetails1.TabIndex = 46;
            // 
            // bindingSourceUserList
            // 
            this.bindingSourceUserList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.UserList);
            // 
            // bindingSourceCompanyTypeList
            // 
            this.bindingSourceCompanyTypeList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.CompanyTypeList);
            // 
            // bindingSourceCompanyDetails
            // 
            this.bindingSourceCompanyDetails.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.Company);
            // 
            // CtrlCompanyDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnUploadLogo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ctrlAddressDetails1);
            this.Controls.Add(this.grpAdminContact);
            this.Controls.Add(this.grpLicensing);
            this.Controls.Add(this.cmbCompanyType);
            this.Controls.Add(this.lblCompanyType);
            this.Controls.Add(this.lblFullNameRequired);
            this.Controls.Add(this.lblShortNameRequired);
            this.Controls.Add(this.txtfilePath);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.txtShortName);
            this.Controls.Add(this.lblShortName);
            this.Controls.Add(this.txtCompanyFullName);
            this.Controls.Add(this.txtLabel);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCompanyDetails";
            this.Size = new System.Drawing.Size(497, 393);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpLicensing)).EndInit();
            this.grpLicensing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpAdminContact)).EndInit();
            this.grpAdminContact.ResumeLayout(false);
            this.grpAdminContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyTypeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel txtLabel;
        private System.Windows.Forms.TextBox txtCompanyFullName;
        private Infragistics.Win.Misc.UltraLabel lblShortName;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.TextBox txtfilePath;
        private Infragistics.Win.Misc.UltraLabel lblLogo;
        private Infragistics.Win.Misc.UltraLabel lblShortNameRequired;
        private Infragistics.Win.Misc.UltraLabel lblFullNameRequired;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyType;
        private Infragistics.Win.Misc.UltraLabel lblCompanyType;
        private Infragistics.Win.Misc.UltraGroupBox grpLicensing;
        private Infragistics.Win.Misc.UltraLabel lblLicences;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Infragistics.Win.Misc.UltraGroupBox grpAdminContact;
        private Infragistics.Win.Misc.UltraLabel lblPCTitle;
        private Infragistics.Win.Misc.UltraLabel lblWorkNumberRequired;
        private Infragistics.Win.Misc.UltraLabel lblEmailRequired;
        private Infragistics.Win.Misc.UltraLabel lblLastNameRequired;
        private Infragistics.Win.Misc.UltraLabel lblFirstNameRequired;
        private System.Windows.Forms.TextBox txtACCellNumber;
        private Infragistics.Win.Misc.UltraLabel lblACCellNumber;
        private System.Windows.Forms.TextBox txtACWorkNumber;
        private Infragistics.Win.Misc.UltraLabel lblACWorkNumber;
        private System.Windows.Forms.TextBox txtACEmail;
        private Infragistics.Win.Misc.UltraLabel lblACEmail;
        private System.Windows.Forms.TextBox txtACLastName;
        private Infragistics.Win.Misc.UltraLabel lblACLastName;
        private System.Windows.Forms.TextBox txtACTitle;
        private System.Windows.Forms.TextBox txtACFirstName;
        private Infragistics.Win.Misc.UltraLabel lblACTitle;
        private Infragistics.Win.Misc.UltraLabel lblACFirstName;
        private Infragistics.Win.Misc.UltraLabel lblUserName;
        private Infragistics.Win.Misc.UltraLabel lblLoginName;
        private Infragistics.Win.Misc.UltraLabel lblPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private Infragistics.Win.Misc.UltraLabel lblPasswordRequired;
        private Infragistics.Win.Misc.UltraLabel lblLoginNameRequired;
        private System.Windows.Forms.TextBox txtPassword;
        private Infragistics.Win.Misc.UltraLabel lblCellNumberRequired;
        private Infragistics.Win.Misc.UltraLabel lblFaxNumberRequired;
        private System.Windows.Forms.TextBox txtFaxNumber;
        private Infragistics.Win.Misc.UltraLabel lblFaxNumber;
        private Infragistics.Win.Misc.UltraLabel lblHomeNumberRequired;
        private System.Windows.Forms.TextBox txtHomeNumberRequired;
        private Infragistics.Win.Misc.UltraLabel lblHomeNumber;
        private Infragistics.Win.Misc.UltraLabel lblPagerNumberRequired;
        private System.Windows.Forms.TextBox txtPagerNumber;
        private Infragistics.Win.Misc.UltraLabel lblPagerNumber;
        private Infragistics.Win.Misc.UltraLabel lblUserNameRequired;
        private CtrlAddressDetails ctrlAddressDetails1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton btnUploadLogo;
        private System.Windows.Forms.BindingSource bindingSourceCompanyDetails;
        private System.Windows.Forms.BindingSource bindingSourceCompanyTypeList;
        private System.Windows.Forms.BindingSource bindingSourceUserList;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUsers;
    }
}
