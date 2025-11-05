using System.Net;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    partial class ctrlPTTPreferences
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraTabLongPageControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.masterFundAccountGridControlForLongTab = new Prana.Rebalancer.PercentTradingTool.Preferences.MasterFundAccountGridControl();
            this.ultraTabShortPageControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.masterFundAccountGridControlForShortTab = new Prana.Rebalancer.PercentTradingTool.Preferences.MasterFundAccountGridControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.lblMasterFund = new Infragistics.Win.Misc.UltraLabel();
            this.grpBxMasterFundAccount = new Infragistics.Win.Misc.UltraGroupBox();
            this.masterFundAccountGridControlForGroupBox = new Prana.Rebalancer.PercentTradingTool.Preferences.MasterFundAccountGridControl();
            this.chkboxShortLongPref = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraTabShortLong = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdAccountFactor = new PranaUltraGrid();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCalculationType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblType = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncreaseDecimaldigits = new Infragistics.Win.Misc.UltraLabel();
            this.cmbMasterFundOrAccountValue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblCalculationValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelRemoveAccountsWithZeroNAV = new Infragistics.Win.Misc.UltraLabel();
            this.chkRemoveAccountsWithZeroNAV = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numIncreaseDecimalPrecision = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.cmbAddSet = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblOfto = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCombineAccountTotal = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.multiSelectDropDownAccount = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.ultraTabLongPageControl.SuspendLayout();
            this.ultraTabShortPageControl.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxMasterFundAccount)).BeginInit();
            this.grpBxMasterFundAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkboxShortLongPref)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabShortLong)).BeginInit();
            this.ultraTabShortLong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculationType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMasterFundOrAccountValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveAccountsWithZeroNAV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncreaseDecimalPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAddSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCombineAccountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabLongPageControl
            // 
            this.ultraTabLongPageControl.Controls.Add(this.masterFundAccountGridControlForLongTab);
            this.ultraTabLongPageControl.Location = new System.Drawing.Point(1, 23);
            this.ultraTabLongPageControl.Name = "ultraTabLongPageControl";
            this.ultraTabLongPageControl.Size = new System.Drawing.Size(438, 328);
            // 
            // masterFundAccountGridControlForLongTab
            // 
            this.masterFundAccountGridControlForLongTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterFundAccountGridControlForLongTab.Location = new System.Drawing.Point(0, 0);
            this.masterFundAccountGridControlForLongTab.Name = "masterFundAccountGridControlForLongTab";
            this.masterFundAccountGridControlForLongTab.Size = new System.Drawing.Size(438, 328);
            this.masterFundAccountGridControlForLongTab.TabIndex = 0;
            // 
            // ultraTabShortPageControl
            // 
            this.ultraTabShortPageControl.Controls.Add(this.masterFundAccountGridControlForShortTab);
            this.ultraTabShortPageControl.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabShortPageControl.Name = "ultraTabShortPageControl";
            this.ultraTabShortPageControl.Size = new System.Drawing.Size(438, 328);
            // 
            // masterFundAccountGridControlForShortTab
            // 
            this.masterFundAccountGridControlForShortTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterFundAccountGridControlForShortTab.Location = new System.Drawing.Point(0, 0);
            this.masterFundAccountGridControlForShortTab.Name = "masterFundAccountGridControlForShortTab";
            this.masterFundAccountGridControlForShortTab.Size = new System.Drawing.Size(438, 328);
            this.masterFundAccountGridControlForShortTab.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMasterFund);
            this.ultraPanel1.ClientArea.Controls.Add(this.grpBxMasterFundAccount);
            this.ultraPanel1.ClientArea.Controls.Add(this.chkboxShortLongPref);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraTabShortLong);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessage);
            this.ultraPanel1.Location = new System.Drawing.Point(3, 9);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(960, 509);
            this.ultraPanel1.TabIndex = 3;
            // 
            // lblMasterFund
            // 
            this.lblMasterFund.Location = new System.Drawing.Point(344, 8);
            this.lblMasterFund.Name = "lblMasterFund";
            this.lblMasterFund.Size = new System.Drawing.Size(203, 23);
            this.lblMasterFund.TabIndex = 10;
            this.lblMasterFund.Text = "Master Fund - Account Preference";
            // 
            // grpBxMasterFundAccount
            // 
            this.grpBxMasterFundAccount.Controls.Add(this.masterFundAccountGridControlForGroupBox);
            this.grpBxMasterFundAccount.Location = new System.Drawing.Point(347, 58);
            this.grpBxMasterFundAccount.Name = "grpBxMasterFundAccount";
            this.grpBxMasterFundAccount.Size = new System.Drawing.Size(442, 354);
            this.grpBxMasterFundAccount.TabIndex = 9;
            // 
            // masterFundAccountGridControlForGroupBox
            // 
            this.masterFundAccountGridControlForGroupBox.Location = new System.Drawing.Point(0, 0);
            this.masterFundAccountGridControlForGroupBox.Name = "masterFundAccountGridControlForGroupBox";
            this.masterFundAccountGridControlForGroupBox.Size = new System.Drawing.Size(442, 354);
            this.masterFundAccountGridControlForGroupBox.TabIndex = 0;
            // 
            // chkboxShortLongPref
            // 
            this.chkboxShortLongPref.Location = new System.Drawing.Point(344, 31);
            this.chkboxShortLongPref.Name = "chkboxShortLongPref";
            this.chkboxShortLongPref.Size = new System.Drawing.Size(187, 20);
            this.chkboxShortLongPref.TabIndex = 8;
            this.chkboxShortLongPref.Text = "Use Long/Short Preference";
            this.chkboxShortLongPref.CheckedChanged += new System.EventHandler(this.chkboxShortLongPref_Click);
            // 
            // ultraTabShortLong
            // 
            this.ultraTabShortLong.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabShortLong.Controls.Add(this.ultraTabLongPageControl);
            this.ultraTabShortLong.Controls.Add(this.ultraTabShortPageControl);
            this.ultraTabShortLong.Location = new System.Drawing.Point(347, 58);
            this.ultraTabShortLong.Name = "ultraTabShortLong";
            this.ultraTabShortLong.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabShortLong.Size = new System.Drawing.Size(442, 354);
            this.ultraTabShortLong.TabIndex = 7;
            ultraTab1.FixedWidth = 100;
            ultraTab1.TabPage = this.ultraTabLongPageControl;
            ultraTab1.Text = "Long";
            ultraTab2.FixedWidth = 100;
            ultraTab2.TabPage = this.ultraTabShortPageControl;
            ultraTab2.Text = "Short";
            this.ultraTabShortLong.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabShortLong.Visible = false;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(438, 328);
            // 
            // ultraLabel2
            // 
            appearance1.ForeColor = System.Drawing.Color.Red;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Top";
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(348, 440);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(213, 14);
            this.ultraLabel2.TabIndex = 6;
            this.ultraLabel2.Text = "and changes made here will reflect in all the users.";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.grdAccountFactor);
            this.ultraGroupBox1.Location = new System.Drawing.Point(9, 325);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(329, 163);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.Text = "Account Factors";
            // 
            // grdAccountFactor
            // 
            this.grdAccountFactor.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdAccountFactor.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountFactor.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdAccountFactor.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountFactor.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountFactor.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.grdAccountFactor.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountFactor.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdAccountFactor.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountFactor.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this.grdAccountFactor.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountFactor.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance2.BackColor = System.Drawing.Color.Red;
            this.grdAccountFactor.DisplayLayout.Override.RowSelectorAppearance = appearance2;
            this.grdAccountFactor.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.grdAccountFactor.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountFactor.DisplayLayout.UseFixedHeaders = true;
            this.grdAccountFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAccountFactor.Location = new System.Drawing.Point(3, 16);
            this.grdAccountFactor.Name = "grdAccountFactor";
            this.grdAccountFactor.Size = new System.Drawing.Size(323, 144);
            this.grdAccountFactor.TabIndex = 1;
            this.grdAccountFactor.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdAccountFactor_BeforeExitEditMode);
            this.grdAccountFactor.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAccountFactor_BeforeCustomRowFilterDialog);
            // 
            // lblMessage
            // 
            appearance3.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance3.ForeColor = System.Drawing.Color.Red;
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Top";
            this.lblMessage.Appearance = appearance3;
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.lblMessage.Location = new System.Drawing.Point(347, 424);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(343, 14);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "*In Master Fund-Account Preference accounts permitted to user will only be visibl" +
    "e  ";
            this.lblMessage.WrapText = false;
            // 
            // cmbCalculationType
            // 
            this.cmbCalculationType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbCalculationType.Location = new System.Drawing.Point(172, 61);
            this.cmbCalculationType.Name = "cmbCalculationType";
            this.cmbCalculationType.NullText = "-Select-";
            this.cmbCalculationType.Size = new System.Drawing.Size(144, 21);
            this.cmbCalculationType.TabIndex = 33;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(11, 61);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(89, 14);
            this.lblType.TabIndex = 34;
            this.lblType.Text = "Calculation Type";
            // 
            // lblIncreaseDecimaldigits
            // 
            this.lblIncreaseDecimaldigits.AutoSize = true;
            this.lblIncreaseDecimaldigits.Location = new System.Drawing.Point(11, 214);
            this.lblIncreaseDecimaldigits.Name = "lblIncreaseDecimaldigits";
            this.lblIncreaseDecimaldigits.Size = new System.Drawing.Size(121, 14);
            this.lblIncreaseDecimaldigits.TabIndex = 37;
            this.lblIncreaseDecimaldigits.Text = "Increase Decimal digits";
            // 
            // cmbMasterFundOrAccountValue
            // 
            this.cmbMasterFundOrAccountValue.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbMasterFundOrAccountValue.Location = new System.Drawing.Point(172, 137);
            this.cmbMasterFundOrAccountValue.Name = "cmbMasterFundOrAccountValue";
            this.cmbMasterFundOrAccountValue.NullText = "-Select-";
            this.cmbMasterFundOrAccountValue.Size = new System.Drawing.Size(144, 21);
            this.cmbMasterFundOrAccountValue.TabIndex = 38;
            this.cmbMasterFundOrAccountValue.SelectionChanged += new System.EventHandler(this.cmbMasterFundOrAccountValue_SelectionChanged);
            // 
            // lblCalculationValue
            // 
            this.lblCalculationValue.AutoSize = true;
            this.lblCalculationValue.Location = new System.Drawing.Point(11, 137);
            this.lblCalculationValue.Name = "lblCalculationValue";
            this.lblCalculationValue.Size = new System.Drawing.Size(109, 14);
            this.lblCalculationValue.TabIndex = 39;
            this.lblCalculationValue.Text = "MasterFund/Account";
            // 
            // ultraLabelRemoveAccountsWithZeroNAV
            // 
            this.ultraLabelRemoveAccountsWithZeroNAV.AutoSize = true;
            this.ultraLabelRemoveAccountsWithZeroNAV.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.ultraLabelRemoveAccountsWithZeroNAV.Location = new System.Drawing.Point(11, 246);
            this.ultraLabelRemoveAccountsWithZeroNAV.Name = "ultraLabelRemoveAccountsWithZeroNAV";
            this.ultraLabelRemoveAccountsWithZeroNAV.Size = new System.Drawing.Size(201, 12);
            this.ultraLabelRemoveAccountsWithZeroNAV.TabIndex = 40;
            this.ultraLabelRemoveAccountsWithZeroNAV.Text = "Remove MasterFunds/ Accounts with ≤ 0 NAV";
            // 
            // chkRemoveAccountsWithZeroNAV
            // 
            this.chkRemoveAccountsWithZeroNAV.Checked = true;
            this.chkRemoveAccountsWithZeroNAV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemoveAccountsWithZeroNAV.Location = new System.Drawing.Point(302, 246);
            this.chkRemoveAccountsWithZeroNAV.Name = "chkRemoveAccountsWithZeroNAV";
            this.chkRemoveAccountsWithZeroNAV.Size = new System.Drawing.Size(14, 20);
            this.chkRemoveAccountsWithZeroNAV.TabIndex = 41;
            // 
            // numIncreaseDecimalPrecision
            // 
            this.numIncreaseDecimalPrecision.Location = new System.Drawing.Point(172, 214);
            this.numIncreaseDecimalPrecision.MaxValue = 6;
            this.numIncreaseDecimalPrecision.MinValue = 0;
            this.numIncreaseDecimalPrecision.Name = "numIncreaseDecimalPrecision";
            this.numIncreaseDecimalPrecision.Size = new System.Drawing.Size(144, 21);
            this.numIncreaseDecimalPrecision.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numIncreaseDecimalPrecision.SpinIncrement = 1;
            this.numIncreaseDecimalPrecision.TabIndex = 42;
            // 
            // cmbAddSet
            // 
            this.cmbAddSet.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbAddSet.Location = new System.Drawing.Point(172, 23);
            this.cmbAddSet.Name = "cmbAddSet";
            this.cmbAddSet.NullText = "-Select-";
            this.cmbAddSet.Size = new System.Drawing.Size(144, 21);
            this.cmbAddSet.TabIndex = 45;
            // 
            // lblOfto
            // 
            this.lblOfto.AutoSize = true;
            this.lblOfto.Location = new System.Drawing.Point(11, 23);
            this.lblOfto.Name = "lblOfto";
            this.lblOfto.Size = new System.Drawing.Size(39, 14);
            this.lblOfto.TabIndex = 46;
            this.lblOfto.Text = "+ / - / =";
            // 
            // cmbCombineAccountTotal
            // 
            this.cmbCombineAccountTotal.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbCombineAccountTotal.Location = new System.Drawing.Point(172, 99);
            this.cmbCombineAccountTotal.Name = "cmbCombineAccountTotal";
            this.cmbCombineAccountTotal.NullText = "-Select-";
            this.cmbCombineAccountTotal.Size = new System.Drawing.Size(144, 21);
            this.cmbCombineAccountTotal.TabIndex = 47;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(11, 99);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(128, 14);
            this.ultraLabel1.TabIndex = 48;
            this.ultraLabel1.Text = "Combined Account Total";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(11, 175);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(45, 14);
            this.lblAccount.TabIndex = 50;
            this.lblAccount.Text = "Account";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.multiSelectDropDownAccount);
            this.ultraGroupBox2.Controls.Add(this.lblAccount);
            this.ultraGroupBox2.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox2.Controls.Add(this.cmbCombineAccountTotal);
            this.ultraGroupBox2.Controls.Add(this.lblOfto);
            this.ultraGroupBox2.Controls.Add(this.cmbAddSet);
            this.ultraGroupBox2.Controls.Add(this.numIncreaseDecimalPrecision);
            this.ultraGroupBox2.Controls.Add(this.chkRemoveAccountsWithZeroNAV);
            this.ultraGroupBox2.Controls.Add(this.ultraLabelRemoveAccountsWithZeroNAV);
            this.ultraGroupBox2.Controls.Add(this.lblCalculationValue);
            this.ultraGroupBox2.Controls.Add(this.cmbMasterFundOrAccountValue);
            this.ultraGroupBox2.Controls.Add(this.lblIncreaseDecimaldigits);
            this.ultraGroupBox2.Controls.Add(this.lblType);
            this.ultraGroupBox2.Controls.Add(this.cmbCalculationType);
            this.ultraGroupBox2.Location = new System.Drawing.Point(12, 16);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(329, 312);
            this.ultraGroupBox2.TabIndex = 2;
            this.ultraGroupBox2.Text = "Input Parameters";
            // 
            // multiSelectDropDownAccount
            // 
            this.multiSelectDropDownAccount.Location = new System.Drawing.Point(172, 175);
            this.multiSelectDropDownAccount.Name = "multiSelectDropDownAccount";
            this.multiSelectDropDownAccount.Size = new System.Drawing.Size(144, 21);
            this.multiSelectDropDownAccount.TabIndex = 49;
            this.multiSelectDropDownAccount.TitleText = "";
            // 
            // ctrlPTTPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlPTTPreferences";
            this.Size = new System.Drawing.Size(966, 574);
            this.ultraTabLongPageControl.ResumeLayout(false);
            this.ultraTabShortPageControl.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBxMasterFundAccount)).EndInit();
            this.grpBxMasterFundAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkboxShortLongPref)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabShortLong)).EndInit();
            this.ultraTabShortLong.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculationType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMasterFundOrAccountValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveAccountsWithZeroNAV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncreaseDecimalPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAddSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCombineAccountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCalculationType;
        private Infragistics.Win.Misc.UltraLabel lblType;
        private Infragistics.Win.Misc.UltraLabel lblIncreaseDecimaldigits;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbMasterFundOrAccountValue;
        private Infragistics.Win.Misc.UltraLabel lblCalculationValue;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRemoveAccountsWithZeroNAV;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRemoveAccountsWithZeroNAV;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numIncreaseDecimalPrecision;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAddSet;
        private Infragistics.Win.Misc.UltraLabel lblOfto;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCombineAccountTotal;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblAccount;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownAccount;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private PranaUltraGrid grdAccountFactor;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkboxShortLongPref;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabShortLong;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabLongPageControl;
        private MasterFundAccountGridControl masterFundAccountGridControlForLongTab;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabShortPageControl;
        private MasterFundAccountGridControl masterFundAccountGridControlForShortTab;
        private Infragistics.Win.Misc.UltraGroupBox grpBxMasterFundAccount;
        private MasterFundAccountGridControl masterFundAccountGridControlForGroupBox;
        private Infragistics.Win.Misc.UltraLabel lblMasterFund;
    }
}
