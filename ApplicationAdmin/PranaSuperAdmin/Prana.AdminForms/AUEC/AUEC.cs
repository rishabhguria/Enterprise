#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.AdminForms;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CommonObjects;
using Prana.BusinessObjects.Enums;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for AUEC.
    /// </summary>
    public class AUEC : System.Windows.Forms.Form
    {
        //User defined constant Definitions.
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "AUEC : ";
        private const int TABCOMPLIANCE = 4;
        private const int TABEXCHANGE = 0;
        private const int TABMARKETFEES = 1;
        private const int TABASSETWISEPERMISSIONS = 5;
        public const int var = 0;
        private const string NULLVALUE = "0";

        private const string NULLYEAR = "10/10/2000";

        private const string TABNAME_INITIAL = "tbc";
        private const string TABPAGENAME_INITIAL = "tabPgNm";
        private const string USERCONTROLNAME_INITIAL = "ctrl";
        private const string TABKEY_INITIAL = "key";
        private const string TABCONTROLNAME_INITIAL = "tbcCtrl";
        #region Private and Protected Members

        private System.Windows.Forms.TreeView trvAsset;
        private System.Windows.Forms.Button btnAdd;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcAUEC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;


        private System.Windows.Forms.TextBox txtExchangeNameFull;
        private System.Windows.Forms.TextBox txtShortName;

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtPurchaseSecFees;
        private System.Windows.Forms.TextBox txtPurchaseStamp;
        private System.Windows.Forms.TextBox txtSaleStamp;
        private System.Windows.Forms.TextBox txtSaleLevy;
        private System.Windows.Forms.TextBox txtPurchaseLevy;
        private System.Windows.Forms.TextBox txtSalesSecFees;

        private Prana.Admin.Controls.TimeControl txtPreMarketTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtPreMarketTradingEndTime;
        private Prana.Admin.Controls.TimeControl txtLunchTimeStartTime;
        private Prana.Admin.Controls.TimeControl txtLunchTimeEndTime;
        private Prana.Admin.Controls.TimeControl txtPostMarketTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtPostMarketTradingEndTime;
        private Prana.Admin.Controls.TimeControl txtRegularTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtRegularTradingEndTime;

        private System.Windows.Forms.TextBox txtSettlementDays;
        //		private System.Windows.Forms.TextBox txtDayLightSaving;
        private System.Windows.Forms.DateTimePicker txtDayLightSaving;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button btnAddHoliday;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox txtHolidayDescription;
        private System.Windows.Forms.DateTimePicker dttHolidayDate;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.DateTimePicker txtDayLightSavingTime;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbcExchangeDetails;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblIdentifierName;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor cmbTimeZone;
        private System.Windows.Forms.Button btnDeleteHoliday;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkPostMarket;
        private System.Windows.Forms.CheckBox chkLunchTime;
        private System.Windows.Forms.CheckBox chkPreMarket;
        private System.Windows.Forms.CheckBox chkRegularMarket;
        private System.Windows.Forms.Label label10;

        private System.Windows.Forms.Label lblPreMarket;
        private System.Windows.Forms.Label lblRegularTime;
        private System.Windows.Forms.Label lblLunchTime;
        private System.Windows.Forms.Label lblPostMarket;
        private System.Windows.Forms.Button btnUploadHolidays;
        private System.Windows.Forms.Button btnLoadDefaultHolidays;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label lblBaseCurrency;
        private System.Windows.Forms.Label lblOtherCurrency;
        private System.Windows.Forms.Label lblSymbolConvention;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label lblExchangeIdentifier;
        private System.Windows.Forms.TextBox txtExchangeIdentifier;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExchangeLogo;
        private System.Windows.Forms.Label lblExchangeLogo;
        private System.Windows.Forms.Label lblFlag;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label76;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUnits;
        private System.Windows.Forms.GroupBox grpDtails;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSymbol;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbShortSaleConfirmation;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbProvideAccountnamewithTrade;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbProvideidentifierwithtrade;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbIdentifierName;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCurrencyConversion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnUploadAUECFlag;
        private System.Windows.Forms.Button btnUploadAUECLogo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHoliday;
        private System.Windows.Forms.TextBox txtMultiplier;
        private System.Windows.Forms.Label lblAUECName;
        private System.Windows.Forms.Label lblAUECCombination;

        #endregion
        private UltraCombo cmbSymbolConvention;
        private UltraCombo cmbFlag;
        private UltraCombo cmbOtherCurrency;
        private UltraCombo cmbCurrency;
        private Label label3;
        private Label label18;
        private Label lblSelectWeeklyHolidays;
        private CheckedListBox checkedlstWeeklyHolidays;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlOtherFee;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Label label24;
        private TextBox txtSettlementDaysSell;
        private UltraCombo cmbState;
        private Label lblYear;
        private ComboBox CmbYear;
        private Label lblCalendars;
        private Label lblEditCalendar;
        private UltraCombo cmbCalendars;
        private CheckBox cBoxAuec;
        private Button btnRefresh;
        private Button btnNewState;
        private Button btnNewCountry;
        private IContainer components;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel AUEC_Fill_Panel;
        // new label added for roundlot, PRANA-11159
        private Label label25;
        private Label label31;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtRoundLot;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraPanel upnlAssetWisePermissions;
        private CheckedListBox checkedlstSide;
        private Label label55;
        private Label label51;
        private Label label32;
        private Label label33;
        private TextBox txtMarketDataProviderExchangeIdentifier;
        private Label lblMarketDataProviderExchangeIdentifier;
        Prana.Admin.Controls.AUECAudit ctrlAUECAudit = new Prana.Admin.Controls.AUECAudit();
        public AUEC()
        {
            InitializeComponent();
            SetUpMenuPermissions();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (trvAsset != null)
                {
                    trvAsset.Dispose();
                }
                if (btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (ultraTabPageControl2 != null)
                {
                    ultraTabPageControl2.Dispose();
                }
                if (ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (ultraTabSharedControlsPage2 != null)
                {
                    ultraTabSharedControlsPage2.Dispose();
                }
                if (ultraTabPageControl4 != null)
                {
                    ultraTabPageControl4.Dispose();
                }
                if (ultraTabPageControl5 != null)
                {
                    ultraTabPageControl5.Dispose();
                }
                if (tbcAUEC != null)
                {
                    tbcAUEC.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label15 != null)
                {
                    label15.Dispose();
                }
                if (label16 != null)
                {
                    label16.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (label21 != null)
                {
                    label21.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (label23 != null)
                {
                    label23.Dispose();
                }
                if (label24 != null)
                {
                    label24.Dispose();
                }
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (label26 != null)
                {
                    label26.Dispose();
                }
                if (label27 != null)
                {
                    label27.Dispose();
                }
                if (label28 != null)
                {
                    label28.Dispose();
                }
                if (label29 != null)
                {
                    label29.Dispose();
                }
                if (label30 != null)
                {
                    label30.Dispose();
                }
                if (label31 != null)
                {
                    label31.Dispose();
                }
                if (label32 != null)
                {
                    label32.Dispose();
                }
                if (label33 != null)
                {
                    label33.Dispose();
                }
                if (label40 != null)
                {
                    label40.Dispose();
                }
                if (label41 != null)
                {
                    label41.Dispose();
                }
                if (label42 != null)
                {
                    label42.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (label44 != null)
                {
                    label44.Dispose();
                }
                if (label45 != null)
                {
                    label45.Dispose();
                }
                if (label46 != null)
                {
                    label46.Dispose();
                }
                if (label47 != null)
                {
                    label47.Dispose();
                }
                if (label48 != null)
                {
                    label48.Dispose();
                }
                if (label49 != null)
                {
                    label49.Dispose();
                }
                if (label50 != null)
                {
                    label50.Dispose();
                }
                if (label51 != null)
                {
                    label51.Dispose();
                }
                if (label55 != null)
                {
                    label55.Dispose();
                }
                if (label57 != null)
                {
                    label57.Dispose();
                }
                if (label58 != null)
                {
                    label58.Dispose();
                }
                if (label59 != null)
                {
                    label59.Dispose();
                }
                if (label60 != null)
                {
                    label60.Dispose();
                }
                if (label65 != null)
                {
                    label65.Dispose();
                }
                if (label67 != null)
                {
                    label67.Dispose();
                }
                if (label68 != null)
                {
                    label68.Dispose();
                }
                if (label71 != null)
                {
                    label71.Dispose();
                }
                if (label76 != null)
                {
                    label76.Dispose();
                }
                if (lblIdentifierName != null)
                {
                    lblIdentifierName.Dispose();
                }
                if (lblPreMarket != null)
                {
                    lblPreMarket.Dispose();
                }
                if (lblRegularTime != null)
                {
                    lblRegularTime.Dispose();
                }
                if (lblLunchTime != null)
                {
                    lblLunchTime.Dispose();
                }
                if (lblPostMarket != null)
                {
                    lblPostMarket.Dispose();
                }
                if (lblBaseCurrency != null)
                {
                    lblBaseCurrency.Dispose();
                }
                if (lblOtherCurrency != null)
                {
                    lblOtherCurrency.Dispose();
                }
                if (lblSymbolConvention != null)
                {
                    lblSymbolConvention.Dispose();
                }
                if (lblExchangeIdentifier != null)
                {
                    lblExchangeIdentifier.Dispose();
                }
                if (lblExchangeLogo != null)
                {
                    lblExchangeLogo.Dispose();
                }
                if (lblFlag != null)
                {
                    lblFlag.Dispose();
                }
                if (lblAUECName != null)
                {
                    lblAUECName.Dispose();
                }
                if (lblAUECCombination != null)
                {
                    lblAUECCombination.Dispose();
                }
                if (lblSelectWeeklyHolidays != null)
                {
                    lblSelectWeeklyHolidays.Dispose();
                }
                if (lblYear != null)
                {
                    lblYear.Dispose();
                }
                if (lblCalendars != null)
                {
                    lblCalendars.Dispose();
                }
                if (lblEditCalendar != null)
                {
                    lblEditCalendar.Dispose();
                }
                if (lblMarketDataProviderExchangeIdentifier != null)
                {
                    lblMarketDataProviderExchangeIdentifier.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (txtExchangeNameFull != null)
                {
                    txtExchangeNameFull.Dispose();
                }
                if (groupBox2 != null)
                {
                    groupBox2.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (txtPurchaseSecFees != null)
                {
                    txtPurchaseSecFees.Dispose();
                }
                if (txtPurchaseStamp != null)
                {
                    txtPurchaseStamp.Dispose();
                }
                if (txtSaleStamp != null)
                {
                    txtSaleStamp.Dispose();
                }
                if (txtSaleLevy != null)
                {
                    txtSaleLevy.Dispose();
                }
                if (txtPurchaseLevy != null)
                {
                    txtPurchaseLevy.Dispose();
                }
                if (txtSalesSecFees != null)
                {
                    txtSalesSecFees.Dispose();
                }
                if (txtPreMarketTradingEndTime != null)
                {
                    txtPreMarketTradingEndTime.Dispose();
                }
                if (txtPreMarketTradingStartTime != null)
                {
                    txtPreMarketTradingStartTime.Dispose();
                }
                if (txtLunchTimeEndTime != null)
                {
                    txtLunchTimeEndTime.Dispose();
                }
                if (txtLunchTimeStartTime != null)
                {
                    txtLunchTimeStartTime.Dispose();
                }
                if (txtPostMarketTradingEndTime != null)
                {
                    txtPostMarketTradingEndTime.Dispose();
                }
                if (txtPostMarketTradingStartTime != null)
                {
                    txtPostMarketTradingStartTime.Dispose();
                }
                if (txtRegularTradingStartTime != null)
                {
                    txtRegularTradingStartTime.Dispose();
                }
                if (txtRegularTradingEndTime != null)
                {
                    txtRegularTradingEndTime.Dispose();
                }
                if (txtSettlementDays != null)
                {
                    txtSettlementDays.Dispose();
                }
                if (txtDayLightSaving != null)
                {
                    txtDayLightSaving.Dispose();
                }
                if (groupBox3 != null)
                {
                    groupBox3.Dispose();
                }
                if (btnAddHoliday != null)
                {
                    btnAddHoliday.Dispose();
                }
                if (groupBox5 != null)
                {
                    groupBox5.Dispose();
                }
                if (groupBox6 != null)
                {
                    groupBox6.Dispose();
                }
                if (txtHolidayDescription != null)
                {
                    txtHolidayDescription.Dispose();
                }
                if (dttHolidayDate != null)
                {
                    dttHolidayDate.Dispose();
                }
                if (txtDayLightSavingTime != null)
                {
                    txtDayLightSavingTime.Dispose();
                }
                if (tbcExchangeDetails != null)
                {
                    tbcExchangeDetails.Dispose();
                }
                if (groupBox7 != null)
                {
                    groupBox7.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (cmbTimeZone != null)
                {
                    cmbTimeZone.Dispose();
                }
                if (btnDeleteHoliday != null)
                {
                    btnDeleteHoliday.Dispose();
                }
                if (chkPostMarket != null)
                {
                    chkPostMarket.Dispose();
                }
                if (chkLunchTime != null)
                {
                    chkLunchTime.Dispose();
                }
                if (chkPreMarket != null)
                {
                    chkPreMarket.Dispose();
                }
                if (chkRegularMarket != null)
                {
                    chkRegularMarket.Dispose();
                }
                if (btnUploadHolidays != null)
                {
                    btnUploadHolidays.Dispose();
                }
                if (btnLoadDefaultHolidays != null)
                {
                    btnLoadDefaultHolidays.Dispose();
                }
                if (txtExchangeIdentifier != null)
                {
                    txtExchangeIdentifier.Dispose();
                }
                if (cmbExchangeLogo != null)
                {
                    cmbExchangeLogo.Dispose();
                }
                if (cmbUnits != null)
                {
                    cmbUnits.Dispose();
                }
                if (grpDtails != null)
                {
                    grpDtails.Dispose();
                }
                if (grdSymbol != null)
                {
                    grdSymbol.Dispose();
                }
                if (cmbShortSaleConfirmation != null)
                {
                    cmbShortSaleConfirmation.Dispose();
                }
                if (cmbProvideAccountnamewithTrade != null)
                {
                    cmbProvideAccountnamewithTrade.Dispose();
                }
                if (cmbProvideidentifierwithtrade != null)
                {
                    cmbProvideidentifierwithtrade.Dispose();
                }
                if (cmbIdentifierName != null)
                {
                    cmbIdentifierName.Dispose();
                }
                if (cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if (cmbCurrencyConversion != null)
                {
                    cmbCurrencyConversion.Dispose();
                }
                if (panel1 != null)
                {
                    panel1.Dispose();
                }
                if (panel3 != null)
                {
                    panel3.Dispose();
                }
                if (btnUploadAUECFlag != null)
                {
                    btnUploadAUECFlag.Dispose();
                }
                if (btnUploadAUECLogo != null)
                {
                    btnUploadAUECLogo.Dispose();
                }
                if (openFileDialog1 != null)
                {
                    openFileDialog1.Dispose();
                }
                if (grdHoliday != null)
                {
                    grdHoliday.Dispose();
                }
                if (txtMultiplier != null)
                {
                    txtMultiplier.Dispose();
                }
                if (cmbSymbolConvention != null)
                {
                    cmbSymbolConvention.Dispose();
                }
                if (cmbFlag != null)
                {
                    cmbFlag.Dispose();
                }
                if (cmbOtherCurrency != null)
                {
                    cmbOtherCurrency.Dispose();
                }
                if (cmbCurrency != null)
                {
                    cmbCurrency.Dispose();
                }
                if (checkedlstWeeklyHolidays != null)
                {
                    checkedlstWeeklyHolidays.Dispose();
                }
                if (tabCtrlOtherFee != null)
                {
                    tabCtrlOtherFee.Dispose();
                }
                if (txtSettlementDaysSell != null)
                {
                    txtSettlementDaysSell.Dispose();
                }
                if (cmbState != null)
                {
                    cmbState.Dispose();
                }
                if (CmbYear != null)
                {
                    CmbYear.Dispose();
                }
                if (cmbCalendars != null)
                {
                    cmbCalendars.Dispose();
                }
                if (cBoxAuec != null)
                {
                    cBoxAuec.Dispose();
                }
                if (btnRefresh != null)
                {
                    btnRefresh.Dispose();
                }
                if (btnNewCountry != null)
                {
                    btnNewCountry.Dispose();
                }
                if (btnNewState != null)
                {
                    btnNewState.Dispose();
                }
                if (appStylistRuntime1 != null)
                {
                    appStylistRuntime1.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (AUEC_Fill_Panel != null)
                {
                    AUEC_Fill_Panel.Dispose();
                }
                if (txtRoundLot != null)
                {
                    txtRoundLot.Dispose();
                }
                if (upnlAssetWisePermissions != null)
                {
                    upnlAssetWisePermissions.Dispose();
                }
                if (checkedlstSide != null)
                {
                    checkedlstSide.Dispose();
                }
                if (txtMarketDataProviderExchangeIdentifier != null)
                {
                    txtMarketDataProviderExchangeIdentifier.Dispose();
                }
                if (ctrlAUECAudit != null)
                {
                    ctrlAUECAudit.Dispose();
                }
                if (frmNewAUEC != null)
                {
                    frmNewAUEC.Dispose();
                }
                if (frmUploadAUECHolidays != null)
                {
                    frmUploadAUECHolidays.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        private bool chkAddAUEC = false;
        private bool chkDeleteAUEC = false;
        private bool chkEditAUEC = false;
        private Calendars _calendars = AUECManager.GetCalendar();
        //This method fetches the user permissions from the database.
        private void SetUpMenuPermissions()
        {
            // Prana.Admin.BLL.Preferences preferences = Prana.Admin.BLL.Preferences.Instance;

            ModuleResources module = ModuleResources.AUEC;
            AuthAction action = AuthAction.Write;
            var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
            if (hasAccess)
            {
                chkAddAUEC = true;
                chkDeleteAUEC = true;
                chkEditAUEC = true;
            }

            //If the user doesn't have the permissions to add or delete AUEC then the respective Add or Delete buttons are
            //disabled so that he/she can't add or delete the AUEC.
            if (chkAddAUEC == false)
            {
                btnAdd.Enabled = false;
            }
            if (chkDeleteAUEC == false)
            {
                btnDelete.Enabled = false;
            }
            if (chkEditAUEC == false)
            {
                btnSave.Enabled = false;
            }

            // modified by Bhavana on July 7, 2014
            // purpose : To show logged user in audit details.
            int loggedInUserID = AuthorizationManager.GetInstance()._authorizedPrincipal.UserId;
            AuditManager.BLL.AuditHandler.GetInstance().SetUIonPermission(hasAccess, loggedInUserID);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AUEC));
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
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tbcExchangeDetails = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnNewState = new System.Windows.Forms.Button();
            this.btnNewCountry = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.txtSettlementDaysSell = new System.Windows.Forms.TextBox();
            this.cmbFlag = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnUploadAUECLogo = new System.Windows.Forms.Button();
            this.btnUploadAUECFlag = new System.Windows.Forms.Button();
            this.txtPreMarketTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtPreMarketTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtRegularTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtRegularTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtLunchTimeStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtLunchTimeEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtPostMarketTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtPostMarketTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbExchangeLogo = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblExchangeLogo = new System.Windows.Forms.Label();
            this.lblFlag = new System.Windows.Forms.Label();
            this.lblPostMarket = new System.Windows.Forms.Label();
            this.lblLunchTime = new System.Windows.Forms.Label();
            this.lblRegularTime = new System.Windows.Forms.Label();
            this.lblPreMarket = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkRegularMarket = new System.Windows.Forms.CheckBox();
            this.chkPostMarket = new System.Windows.Forms.CheckBox();
            this.chkLunchTime = new System.Windows.Forms.CheckBox();
            this.chkPreMarket = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.cmbTimeZone = new Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor();
            this.txtDayLightSavingTime = new System.Windows.Forms.DateTimePicker();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtDayLightSaving = new System.Windows.Forms.DateTimePicker();
            this.txtSettlementDays = new System.Windows.Forms.TextBox();
            this.grpDtails = new System.Windows.Forms.GroupBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txtMarketDataProviderExchangeIdentifier = new System.Windows.Forms.TextBox();
            this.lblMarketDataProviderExchangeIdentifier = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbSymbolConvention = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblAUECCombination = new System.Windows.Forms.Label();
            this.lblAUECName = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.cmbUnits = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label76 = new System.Windows.Forms.Label();
            this.txtExchangeIdentifier = new System.Windows.Forms.TextBox();
            this.lblExchangeIdentifier = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.lblSymbolConvention = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.txtExchangeNameFull = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabCtrlOtherFee = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPurchaseStamp = new System.Windows.Forms.TextBox();
            this.txtPurchaseSecFees = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtSalesSecFees = new System.Windows.Forms.TextBox();
            this.txtPurchaseLevy = new System.Windows.Forms.TextBox();
            this.txtSaleLevy = new System.Windows.Forms.TextBox();
            this.txtSaleStamp = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cBoxAuec = new System.Windows.Forms.CheckBox();
            this.cmbCalendars = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblEditCalendar = new System.Windows.Forms.Label();
            this.lblCalendars = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.CmbYear = new System.Windows.Forms.ComboBox();
            this.grdHoliday = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblSelectWeeklyHolidays = new System.Windows.Forms.Label();
            this.checkedlstWeeklyHolidays = new System.Windows.Forms.CheckedListBox();
            this.label59 = new System.Windows.Forms.Label();
            this.btnUploadHolidays = new System.Windows.Forms.Button();
            this.label58 = new System.Windows.Forms.Label();
            this.dttHolidayDate = new System.Windows.Forms.DateTimePicker();
            this.label40 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtHolidayDescription = new System.Windows.Forms.TextBox();
            this.btnAddHoliday = new System.Windows.Forms.Button();
            this.btnDeleteHoliday = new System.Windows.Forms.Button();
            this.btnLoadDefaultHolidays = new System.Windows.Forms.Button();
            this.label57 = new System.Windows.Forms.Label();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdSymbol = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtRoundLot = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.label31 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOtherCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtMultiplier = new System.Windows.Forms.TextBox();
            this.lblOtherCurrency = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbCurrencyConversion = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label60 = new System.Windows.Forms.Label();
            this.cmbIdentifierName = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbProvideidentifierwithtrade = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbProvideAccountnamewithTrade = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbShortSaleConfirmation = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.lblIdentifierName = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.upnlAssetWisePermissions = new Infragistics.Win.Misc.UltraPanel();
            this.label32 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.checkedlstSide = new System.Windows.Forms.CheckedListBox();
            this.label55 = new System.Windows.Forms.Label();
            this.trvAsset = new System.Windows.Forms.TreeView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbcAUEC = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.AUEC_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.tbcExchangeDetails.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExchangeLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).BeginInit();
            this.grpDtails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlOtherFee)).BeginInit();
            this.tabCtrlOtherFee.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalendars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbol)).BeginInit();
            this.ultraTabPageControl5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOtherCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyConversion)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifierName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvideidentifierwithtrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvideAccountnamewithTrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortSaleConfirmation)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            this.upnlAssetWisePermissions.ClientArea.SuspendLayout();
            this.upnlAssetWisePermissions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcAUEC)).BeginInit();
            this.tbcAUEC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.AUEC_Fill_Panel.ClientArea.SuspendLayout();
            this.AUEC_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcExchangeDetails
            // 
            this.tbcExchangeDetails.Controls.Add(this.groupBox2);
            this.tbcExchangeDetails.Controls.Add(this.grpDtails);
            this.tbcExchangeDetails.Location = new System.Drawing.Point(1, 20);
            this.tbcExchangeDetails.Name = "tbcExchangeDetails";
            this.tbcExchangeDetails.Size = new System.Drawing.Size(487, 608);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.Controls.Add(this.btnNewState);
            this.groupBox2.Controls.Add(this.btnNewCountry);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.txtSettlementDaysSell);
            this.groupBox2.Controls.Add(this.cmbFlag);
            this.groupBox2.Controls.Add(this.btnUploadAUECLogo);
            this.groupBox2.Controls.Add(this.btnUploadAUECFlag);
            this.groupBox2.Controls.Add(this.txtPreMarketTradingStartTime);
            this.groupBox2.Controls.Add(this.txtPreMarketTradingEndTime);
            this.groupBox2.Controls.Add(this.txtRegularTradingEndTime);
            this.groupBox2.Controls.Add(this.txtRegularTradingStartTime);
            this.groupBox2.Controls.Add(this.txtLunchTimeStartTime);
            this.groupBox2.Controls.Add(this.txtLunchTimeEndTime);
            this.groupBox2.Controls.Add(this.txtPostMarketTradingStartTime);
            this.groupBox2.Controls.Add(this.txtPostMarketTradingEndTime);
            this.groupBox2.Controls.Add(this.cmbState);
            this.groupBox2.Controls.Add(this.cmbCountry);
            this.groupBox2.Controls.Add(this.cmbExchangeLogo);
            this.groupBox2.Controls.Add(this.lblExchangeLogo);
            this.groupBox2.Controls.Add(this.lblFlag);
            this.groupBox2.Controls.Add(this.lblPostMarket);
            this.groupBox2.Controls.Add(this.lblLunchTime);
            this.groupBox2.Controls.Add(this.lblRegularTime);
            this.groupBox2.Controls.Add(this.lblPreMarket);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.chkRegularMarket);
            this.groupBox2.Controls.Add(this.chkPostMarket);
            this.groupBox2.Controls.Add(this.chkLunchTime);
            this.groupBox2.Controls.Add(this.chkPreMarket);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label49);
            this.groupBox2.Controls.Add(this.label48);
            this.groupBox2.Controls.Add(this.label47);
            this.groupBox2.Controls.Add(this.label46);
            this.groupBox2.Controls.Add(this.label45);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.cmbTimeZone);
            this.groupBox2.Controls.Add(this.txtDayLightSavingTime);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.label41);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtDayLightSaving);
            this.groupBox2.Controls.Add(this.txtSettlementDays);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(4, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 407);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Trading Hours";
            // 
            // btnNewState
            // 
            this.btnNewState.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNewState.Location = new System.Drawing.Point(391, 374);
            this.btnNewState.Name = "btnNewState";
            this.btnNewState.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnNewState, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnNewState.TabIndex = 117;
            this.btnNewState.Text = "Add New";
            this.btnNewState.UseVisualStyleBackColor = true;
            this.btnNewState.Click += new System.EventHandler(this.btnNewState_Click);
            // 
            // btnNewCountry
            // 
            this.btnNewCountry.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNewCountry.Location = new System.Drawing.Point(391, 352);
            this.btnNewCountry.Name = "btnNewCountry";
            this.btnNewCountry.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnNewCountry, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnNewCountry.TabIndex = 116;
            this.btnNewCountry.Text = "Add New";
            this.btnNewCountry.UseVisualStyleBackColor = true;
            this.btnNewCountry.Click += new System.EventHandler(this.btnNewCountry_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label24.Location = new System.Drawing.Point(394, 308);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(30, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label24, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label24.TabIndex = 115;
            this.label24.Text = "days";
            // 
            // txtSettlementDaysSell
            // 
            this.txtSettlementDaysSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSettlementDaysSell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSettlementDaysSell.Location = new System.Drawing.Point(349, 305);
            this.txtSettlementDaysSell.MaxLength = 3;
            this.txtSettlementDaysSell.Name = "txtSettlementDaysSell";
            this.txtSettlementDaysSell.Size = new System.Drawing.Size(40, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSettlementDaysSell, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSettlementDaysSell.TabIndex = 114;
            this.txtSettlementDaysSell.GotFocus += new System.EventHandler(this.txtSettlementDaysSell_GotFocus);
            this.txtSettlementDaysSell.LostFocus += new System.EventHandler(this.txtSettlementDaysSell_LostFocus);
            // 
            // cmbFlag
            // 
            this.cmbFlag.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbFlag.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFlag.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance1.BorderColor = System.Drawing.Color.Silver;
            this.cmbFlag.DisplayLayout.Override.RowAppearance = appearance1;
            this.cmbFlag.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFlag.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFlag.DropDownWidth = 0;
            this.cmbFlag.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFlag.LimitToList = true;
            this.cmbFlag.Location = new System.Drawing.Point(220, 396);
            this.cmbFlag.Name = "cmbFlag";
            this.cmbFlag.Size = new System.Drawing.Size(156, 21);
            this.cmbFlag.TabIndex = 24;
            this.cmbFlag.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFlag.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbFlag_InitializeLayout);
            this.cmbFlag.GotFocus += new System.EventHandler(this.cmbFlag_GotFocus);
            this.cmbFlag.LostFocus += new System.EventHandler(this.cmbFlag_LostFocus);
            // 
            // btnUploadAUECLogo
            // 
            this.btnUploadAUECLogo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUploadAUECLogo.Location = new System.Drawing.Point(392, 418);
            this.btnUploadAUECLogo.Name = "btnUploadAUECLogo";
            this.btnUploadAUECLogo.Size = new System.Drawing.Size(74, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnUploadAUECLogo, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnUploadAUECLogo.TabIndex = 27;
            this.btnUploadAUECLogo.Text = "Upload";
            this.btnUploadAUECLogo.UseVisualStyleBackColor = false;
            this.btnUploadAUECLogo.Click += new System.EventHandler(this.btnUploadAUECLogo_Click);
            // 
            // btnUploadAUECFlag
            // 
            this.btnUploadAUECFlag.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUploadAUECFlag.Location = new System.Drawing.Point(392, 394);
            this.btnUploadAUECFlag.Name = "btnUploadAUECFlag";
            this.btnUploadAUECFlag.Size = new System.Drawing.Size(74, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnUploadAUECFlag, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnUploadAUECFlag.TabIndex = 25;
            this.btnUploadAUECFlag.Text = "Upload";
            this.btnUploadAUECFlag.UseVisualStyleBackColor = false;
            this.btnUploadAUECFlag.Click += new System.EventHandler(this.btnUploadAUECFlag_Click);
            // 
            // txtPreMarketTradingStartTime
            // 
            this.txtPreMarketTradingStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtPreMarketTradingStartTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtPreMarketTradingStartTime.Location = new System.Drawing.Point(220, 60);
            this.txtPreMarketTradingStartTime.Name = "txtPreMarketTradingStartTime";
            this.txtPreMarketTradingStartTime.Size = new System.Drawing.Size(138, 21);
            this.txtPreMarketTradingStartTime.TabIndex = 8;
            this.txtPreMarketTradingStartTime.Time = "0:0";
            // 
            // txtPreMarketTradingEndTime
            // 
            this.txtPreMarketTradingEndTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtPreMarketTradingEndTime.Location = new System.Drawing.Point(220, 82);
            this.txtPreMarketTradingEndTime.Name = "txtPreMarketTradingEndTime";
            this.txtPreMarketTradingEndTime.Size = new System.Drawing.Size(138, 21);
            this.txtPreMarketTradingEndTime.TabIndex = 9;
            this.txtPreMarketTradingEndTime.Time = "0:0";
            // 
            // txtRegularTradingEndTime
            // 
            this.txtRegularTradingEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRegularTradingEndTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtRegularTradingEndTime.ForeColor = System.Drawing.Color.Black;
            this.txtRegularTradingEndTime.Location = new System.Drawing.Point(220, 144);
            this.txtRegularTradingEndTime.Name = "txtRegularTradingEndTime";
            this.txtRegularTradingEndTime.Size = new System.Drawing.Size(140, 21);
            this.txtRegularTradingEndTime.TabIndex = 12;
            this.txtRegularTradingEndTime.Time = "0:0";
            // 
            // txtRegularTradingStartTime
            // 
            this.txtRegularTradingStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRegularTradingStartTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtRegularTradingStartTime.ForeColor = System.Drawing.Color.Black;
            this.txtRegularTradingStartTime.Location = new System.Drawing.Point(220, 122);
            this.txtRegularTradingStartTime.Name = "txtRegularTradingStartTime";
            this.txtRegularTradingStartTime.Size = new System.Drawing.Size(138, 21);
            this.txtRegularTradingStartTime.TabIndex = 11;
            this.txtRegularTradingStartTime.Time = "0:0";
            // 
            // txtLunchTimeStartTime
            // 
            this.txtLunchTimeStartTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtLunchTimeStartTime.Location = new System.Drawing.Point(220, 186);
            this.txtLunchTimeStartTime.Name = "txtLunchTimeStartTime";
            this.txtLunchTimeStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtLunchTimeStartTime.TabIndex = 14;
            this.txtLunchTimeStartTime.Time = "0:0";
            // 
            // txtLunchTimeEndTime
            // 
            this.txtLunchTimeEndTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtLunchTimeEndTime.Location = new System.Drawing.Point(220, 212);
            this.txtLunchTimeEndTime.Name = "txtLunchTimeEndTime";
            this.txtLunchTimeEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtLunchTimeEndTime.TabIndex = 15;
            this.txtLunchTimeEndTime.Time = "0:0";
            // 
            // txtPostMarketTradingStartTime
            // 
            this.txtPostMarketTradingStartTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtPostMarketTradingStartTime.Location = new System.Drawing.Point(220, 256);
            this.txtPostMarketTradingStartTime.Name = "txtPostMarketTradingStartTime";
            this.txtPostMarketTradingStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtPostMarketTradingStartTime.TabIndex = 17;
            this.txtPostMarketTradingStartTime.Time = "0:0";
            // 
            // txtPostMarketTradingEndTime
            // 
            this.txtPostMarketTradingEndTime.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtPostMarketTradingEndTime.Location = new System.Drawing.Point(220, 280);
            this.txtPostMarketTradingEndTime.Name = "txtPostMarketTradingEndTime";
            this.txtPostMarketTradingEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtPostMarketTradingEndTime.TabIndex = 18;
            this.txtPostMarketTradingEndTime.Time = "0:0";
            // 
            // cmbState
            // 
            this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance2.BorderColor = System.Drawing.Color.Silver;
            this.cmbState.DisplayLayout.Override.RowAppearance = appearance2;
            this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbState.DropDownWidth = 0;
            this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbState.LimitToList = true;
            this.cmbState.Location = new System.Drawing.Point(220, 374);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(156, 21);
            this.cmbState.TabIndex = 23;
            this.cmbState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbState.GotFocus += new System.EventHandler(this.cmbState_GotFocus);
            this.cmbState.LostFocus += new System.EventHandler(this.cmbState_LostFocus);
            // 
            // cmbCountry
            // 
            this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance3.BorderColor = System.Drawing.Color.Silver;
            this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance3;
            this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCountry.DropDownWidth = 0;
            this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCountry.LimitToList = true;
            this.cmbCountry.Location = new System.Drawing.Point(220, 352);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(156, 21);
            this.cmbCountry.TabIndex = 22;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
            this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
            // 
            // cmbExchangeLogo
            // 
            this.cmbExchangeLogo.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbExchangeLogo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbExchangeLogo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance4.BorderColor = System.Drawing.Color.Silver;
            this.cmbExchangeLogo.DisplayLayout.Override.RowAppearance = appearance4;
            this.cmbExchangeLogo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExchangeLogo.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExchangeLogo.DropDownWidth = 0;
            this.cmbExchangeLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbExchangeLogo.LimitToList = true;
            this.cmbExchangeLogo.Location = new System.Drawing.Point(220, 418);
            this.cmbExchangeLogo.Name = "cmbExchangeLogo";
            this.cmbExchangeLogo.Size = new System.Drawing.Size(156, 21);
            this.cmbExchangeLogo.TabIndex = 26;
            this.cmbExchangeLogo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbExchangeLogo.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbExchangeLogo_InitializeLayout);
            this.cmbExchangeLogo.GotFocus += new System.EventHandler(this.cmbExchangeLogo_GotFocus);
            this.cmbExchangeLogo.LostFocus += new System.EventHandler(this.cmbExchangeLogo_LostFocus);
            // 
            // lblExchangeLogo
            // 
            this.lblExchangeLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblExchangeLogo.Location = new System.Drawing.Point(8, 418);
            this.lblExchangeLogo.Name = "lblExchangeLogo";
            this.lblExchangeLogo.Size = new System.Drawing.Size(34, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblExchangeLogo, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblExchangeLogo.TabIndex = 113;
            this.lblExchangeLogo.Text = "Logo";
            // 
            // lblFlag
            // 
            this.lblFlag.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFlag.Location = new System.Drawing.Point(8, 398);
            this.lblFlag.Name = "lblFlag";
            this.lblFlag.Size = new System.Drawing.Size(30, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblFlag, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblFlag.TabIndex = 111;
            this.lblFlag.Text = "Flag";
            // 
            // lblPostMarket
            // 
            this.lblPostMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPostMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPostMarket.Location = new System.Drawing.Point(8, 240);
            this.lblPostMarket.Name = "lblPostMarket";
            this.lblPostMarket.Size = new System.Drawing.Size(66, 12);
            this.inboxControlStyler1.SetStyleSettings(this.lblPostMarket, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblPostMarket.TabIndex = 36;
            this.lblPostMarket.Text = "Post Market";
            // 
            // lblLunchTime
            // 
            this.lblLunchTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLunchTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLunchTime.Location = new System.Drawing.Point(8, 170);
            this.lblLunchTime.Name = "lblLunchTime";
            this.lblLunchTime.Size = new System.Drawing.Size(70, 12);
            this.inboxControlStyler1.SetStyleSettings(this.lblLunchTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblLunchTime.TabIndex = 35;
            this.lblLunchTime.Text = "Lunch Time";
            // 
            // lblRegularTime
            // 
            this.lblRegularTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRegularTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRegularTime.Location = new System.Drawing.Point(8, 106);
            this.lblRegularTime.Name = "lblRegularTime";
            this.lblRegularTime.Size = new System.Drawing.Size(76, 12);
            this.inboxControlStyler1.SetStyleSettings(this.lblRegularTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblRegularTime.TabIndex = 34;
            this.lblRegularTime.Text = "Regular Time";
            // 
            // lblPreMarket
            // 
            this.lblPreMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPreMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPreMarket.Location = new System.Drawing.Point(8, 44);
            this.lblPreMarket.Name = "lblPreMarket";
            this.lblPreMarket.Size = new System.Drawing.Size(62, 12);
            this.inboxControlStyler1.SetStyleSettings(this.lblPreMarket, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblPreMarket.TabIndex = 33;
            this.lblPreMarket.Text = "Pre Market";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(8, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(166, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label10, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label10.TabIndex = 31;
            this.label10.Text = "Regular Market End Time(Local)";
            // 
            // chkRegularMarket
            // 
            this.chkRegularMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkRegularMarket.Location = new System.Drawing.Point(220, 100);
            this.chkRegularMarket.Name = "chkRegularMarket";
            this.chkRegularMarket.Size = new System.Drawing.Size(16, 24);
            this.inboxControlStyler1.SetStyleSettings(this.chkRegularMarket, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkRegularMarket.TabIndex = 10;
            this.chkRegularMarket.CheckStateChanged += new System.EventHandler(this.chkRegularMarket_CheckStateChanged);
            // 
            // chkPostMarket
            // 
            this.chkPostMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkPostMarket.Location = new System.Drawing.Point(220, 234);
            this.chkPostMarket.Name = "chkPostMarket";
            this.chkPostMarket.Size = new System.Drawing.Size(16, 24);
            this.inboxControlStyler1.SetStyleSettings(this.chkPostMarket, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkPostMarket.TabIndex = 16;
            this.chkPostMarket.CheckStateChanged += new System.EventHandler(this.chkPostMarket_CheckStateChanged);
            // 
            // chkLunchTime
            // 
            this.chkLunchTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkLunchTime.Location = new System.Drawing.Point(220, 164);
            this.chkLunchTime.Name = "chkLunchTime";
            this.chkLunchTime.Size = new System.Drawing.Size(16, 24);
            this.inboxControlStyler1.SetStyleSettings(this.chkLunchTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkLunchTime.TabIndex = 13;
            this.chkLunchTime.CheckStateChanged += new System.EventHandler(this.chkLunchTime_CheckStateChanged);
            // 
            // chkPreMarket
            // 
            this.chkPreMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkPreMarket.Location = new System.Drawing.Point(220, 39);
            this.chkPreMarket.Name = "chkPreMarket";
            this.chkPreMarket.Size = new System.Drawing.Size(16, 24);
            this.inboxControlStyler1.SetStyleSettings(this.chkPreMarket, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.chkPreMarket.TabIndex = 7;
            this.chkPreMarket.CheckStateChanged += new System.EventHandler(this.chkPreMarket_CheckStateChanged);
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(8, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(170, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label9, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label9.TabIndex = 22;
            this.label9.Text = "Regular Market Start Time(Local)";
            // 
            // label49
            // 
            this.label49.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label49.ForeColor = System.Drawing.Color.Red;
            this.label49.Location = new System.Drawing.Point(38, 376);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label49, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label49.TabIndex = 21;
            this.label49.Text = "*";
            // 
            // label48
            // 
            this.label48.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label48.ForeColor = System.Drawing.Color.Red;
            this.label48.Location = new System.Drawing.Point(54, 354);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label48, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label48.TabIndex = 20;
            this.label48.Text = "*";
            // 
            // label47
            // 
            this.label47.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label47.ForeColor = System.Drawing.Color.Red;
            this.label47.Location = new System.Drawing.Point(192, 330);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label47, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label47.TabIndex = 19;
            this.label47.Text = "*";
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label46.ForeColor = System.Drawing.Color.Red;
            this.label46.Location = new System.Drawing.Point(207, 309);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label46, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label46.TabIndex = 18;
            this.label46.Text = "*";
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label45.ForeColor = System.Drawing.Color.Red;
            this.label45.Location = new System.Drawing.Point(142, 22);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label45, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label45.TabIndex = 17;
            this.label45.Text = "*";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label30.Location = new System.Drawing.Point(267, 307);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(76, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label30, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label30.TabIndex = 15;
            this.label30.Text = "days          Sell";
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTimeZone.Location = new System.Drawing.Point(222, 18);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.ShowOverflowIndicator = true;
            this.cmbTimeZone.Size = new System.Drawing.Size(166, 20);
            this.cmbTimeZone.TabIndex = 6;
            this.cmbTimeZone.Text = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi";
            this.cmbTimeZone.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTimeZone.ValueChanged += new System.EventHandler(this.cmbTimeZone_ValueChanged);
            this.cmbTimeZone.GotFocus += new System.EventHandler(this.cmbTimeZone_GotFocus);
            this.cmbTimeZone.LostFocus += new System.EventHandler(this.cmbTimeZone_LostFocus);
            // 
            // txtDayLightSavingTime
            // 
            this.txtDayLightSavingTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDayLightSavingTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtDayLightSavingTime.Location = new System.Drawing.Point(320, 328);
            this.txtDayLightSavingTime.Name = "txtDayLightSavingTime";
            this.txtDayLightSavingTime.ShowUpDown = true;
            this.txtDayLightSavingTime.Size = new System.Drawing.Size(120, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtDayLightSavingTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtDayLightSavingTime.TabIndex = 21;
            this.txtDayLightSavingTime.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
            this.txtDayLightSavingTime.GotFocus += new System.EventHandler(this.txtDayLightSavingTime_GotFocus);
            this.txtDayLightSavingTime.LostFocus += new System.EventHandler(this.txtDayLightSavingTime_LostFocus);
            // 
            // label42
            // 
            this.label42.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label42.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label42.Location = new System.Drawing.Point(8, 376);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(34, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label42, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label42.TabIndex = 5;
            this.label42.Text = "State";
            // 
            // label41
            // 
            this.label41.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label41.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label41.Location = new System.Drawing.Point(8, 354);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(52, 14);
            this.inboxControlStyler1.SetStyleSettings(this.label41, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label41.TabIndex = 4;
            this.label41.Text = "Country";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(8, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label4.TabIndex = 2;
            this.label4.Text = "Pre-Market End Time(Local)";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(8, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label5.TabIndex = 1;
            this.label5.Text = "Pre-Market Start Time(Local)";
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(8, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label6, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label6.TabIndex = 0;
            this.label6.Text = "Relationship to GMT (+/-)";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(8, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label7, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label7.TabIndex = 0;
            this.label7.Text = "Lunch Start Time(Local)";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(8, 216);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label8, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label8.TabIndex = 1;
            this.label8.Text = "Lunch End Time(Local)";
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(8, 285);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(152, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label11, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label11.TabIndex = 0;
            this.label11.Text = "Post-Market End Time (Local)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(8, 307);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(195, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label12, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label12.TabIndex = 1;
            this.label12.Text = "Settlement Date     Trade Date+     Buy";
            // 
            // label13
            // 
            this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(8, 326);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(186, 26);
            this.inboxControlStyler1.SetStyleSettings(this.label13, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label13.TabIndex = 2;
            this.label13.Text = "Day Light Savings Conversion date - (MM/DD/YY hh:mm:ss)";
            // 
            // label14
            // 
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(8, 264);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(154, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label14, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label14.TabIndex = 12;
            this.label14.Text = "Post-Market Start Time (Local)";
            // 
            // txtDayLightSaving
            // 
            this.txtDayLightSaving.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDayLightSaving.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDayLightSaving.Location = new System.Drawing.Point(220, 328);
            this.txtDayLightSaving.Name = "txtDayLightSaving";
            this.txtDayLightSaving.Size = new System.Drawing.Size(95, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtDayLightSaving, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtDayLightSaving.TabIndex = 20;
            this.txtDayLightSaving.Value = new System.DateTime(2005, 7, 22, 0, 0, 0, 0);
            this.txtDayLightSaving.GotFocus += new System.EventHandler(this.txtDayLightSaving_GotFocus);
            this.txtDayLightSaving.LostFocus += new System.EventHandler(this.txtDayLightSaving_LostFocus);
            // 
            // txtSettlementDays
            // 
            this.txtSettlementDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSettlementDays.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSettlementDays.Location = new System.Drawing.Point(220, 305);
            this.txtSettlementDays.MaxLength = 3;
            this.txtSettlementDays.Name = "txtSettlementDays";
            this.txtSettlementDays.Size = new System.Drawing.Size(40, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSettlementDays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSettlementDays.TabIndex = 19;
            this.txtSettlementDays.GotFocus += new System.EventHandler(this.txtSettlementDays_GotFocus);
            this.txtSettlementDays.LostFocus += new System.EventHandler(this.txtSettlementDays_LostFocus);
            // 
            // grpDtails
            // 
            this.grpDtails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpDtails.Controls.Add(this.label33);
            this.grpDtails.Controls.Add(this.txtMarketDataProviderExchangeIdentifier);
            this.grpDtails.Controls.Add(this.lblMarketDataProviderExchangeIdentifier);
            this.grpDtails.Controls.Add(this.label18);
            this.grpDtails.Controls.Add(this.cmbSymbolConvention);
            this.grpDtails.Controls.Add(this.lblAUECCombination);
            this.grpDtails.Controls.Add(this.lblAUECName);
            this.grpDtails.Controls.Add(this.label71);
            this.grpDtails.Controls.Add(this.cmbUnits);
            this.grpDtails.Controls.Add(this.label76);
            this.grpDtails.Controls.Add(this.txtExchangeIdentifier);
            this.grpDtails.Controls.Add(this.lblExchangeIdentifier);
            this.grpDtails.Controls.Add(this.label50);
            this.grpDtails.Controls.Add(this.lblSymbolConvention);
            this.grpDtails.Controls.Add(this.label44);
            this.grpDtails.Controls.Add(this.txtExchangeNameFull);
            this.grpDtails.Controls.Add(this.label2);
            this.grpDtails.Controls.Add(this.label1);
            this.grpDtails.Controls.Add(this.txtShortName);
            this.grpDtails.Controls.Add(this.label15);
            this.grpDtails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpDtails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpDtails.Location = new System.Drawing.Point(3, 4);
            this.grpDtails.Name = "grpDtails";
            this.grpDtails.Size = new System.Drawing.Size(478, 187);
            this.inboxControlStyler1.SetStyleSettings(this.grpDtails, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.grpDtails.TabIndex = 0;
            this.grpDtails.TabStop = false;
            this.grpDtails.Text = "Details";
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label33.ForeColor = System.Drawing.Color.Red;
            this.label33.Location = new System.Drawing.Point(232, 134);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label33, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label33.TabIndex = 91;
            this.label33.Text = "*";
            // 
            // txtMarketDataProviderExchangeIdentifier
            // 
            this.txtMarketDataProviderExchangeIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMarketDataProviderExchangeIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMarketDataProviderExchangeIdentifier.Location = new System.Drawing.Point(259, 132);
            this.txtMarketDataProviderExchangeIdentifier.MaxLength = 50;
            this.txtMarketDataProviderExchangeIdentifier.Name = "txtMarketDataProviderExchangeIdentifier";
            this.txtMarketDataProviderExchangeIdentifier.Size = new System.Drawing.Size(154, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtMarketDataProviderExchangeIdentifier, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtMarketDataProviderExchangeIdentifier.TabIndex = 89;
            // 
            // lblMarketDataProviderExchangeIdentifier
            // 
            this.lblMarketDataProviderExchangeIdentifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMarketDataProviderExchangeIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMarketDataProviderExchangeIdentifier.Location = new System.Drawing.Point(12, 136);
            this.lblMarketDataProviderExchangeIdentifier.Name = "lblMarketDataProviderExchangeIdentifier";
            this.lblMarketDataProviderExchangeIdentifier.Size = new System.Drawing.Size(220, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblMarketDataProviderExchangeIdentifier, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblMarketDataProviderExchangeIdentifier.TabIndex = 90;
            this.lblMarketDataProviderExchangeIdentifier.Text = "Market Data Provider\'s Exchange Identifier";
            //
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(146, 112);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label18, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label18.TabIndex = 88;
            this.label18.Text = "*";
            // 
            // cmbSymbolConvention
            // 
            this.cmbSymbolConvention.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbSymbolConvention.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSymbolConvention.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSymbolConvention.DropDownWidth = 0;
            this.cmbSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSymbolConvention.Location = new System.Drawing.Point(259, 88);
            this.cmbSymbolConvention.Name = "cmbSymbolConvention";
            this.cmbSymbolConvention.Size = new System.Drawing.Size(154, 21);
            this.cmbSymbolConvention.TabIndex = 3;
            this.cmbSymbolConvention.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSymbolConvention.GotFocus += new System.EventHandler(this.cmbSymbolConvention_GotFocus);
            this.cmbSymbolConvention.LostFocus += new System.EventHandler(this.cmbSymbolConvention_LostFocus);
            // 
            // lblAUECCombination
            // 
            this.lblAUECCombination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAUECCombination.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAUECCombination.Location = new System.Drawing.Point(261, 24);
            this.lblAUECCombination.Name = "lblAUECCombination";
            this.lblAUECCombination.Size = new System.Drawing.Size(200, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblAUECCombination, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblAUECCombination.TabIndex = 86;
            // 
            // lblAUECName
            // 
            this.lblAUECName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAUECName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAUECName.Location = new System.Drawing.Point(12, 26);
            this.lblAUECName.Name = "lblAUECName";
            this.lblAUECName.Size = new System.Drawing.Size(36, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblAUECName, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblAUECName.TabIndex = 85;
            this.lblAUECName.Text = "AUEC";
            // 
            // label71
            // 
            this.label71.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label71.ForeColor = System.Drawing.Color.Red;
            this.label71.Location = new System.Drawing.Point(44, 158);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label71, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label71.TabIndex = 84;
            this.label71.Text = "*";
            // 
            // cmbUnits
            // 
            this.cmbUnits.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbUnits.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUnits.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUnits.DropDownWidth = 0;
            this.cmbUnits.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbUnits.Location = new System.Drawing.Point(259, 154);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Size = new System.Drawing.Size(154, 21);
            this.cmbUnits.TabIndex = 5;
            this.cmbUnits.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUnits.GotFocus += new System.EventHandler(this.cmbUnits_GotFocus);
            this.cmbUnits.LostFocus += new System.EventHandler(this.cmbUnits_LostFocus);
            // 
            // label76
            // 
            this.label76.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label76.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label76.Location = new System.Drawing.Point(12, 158);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(36, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label76, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label76.TabIndex = 82;
            this.label76.Text = "Units";
            // 
            // txtExchangeIdentifier
            // 
            this.txtExchangeIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExchangeIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtExchangeIdentifier.Location = new System.Drawing.Point(259, 110);
            this.txtExchangeIdentifier.MaxLength = 50;
            this.txtExchangeIdentifier.Name = "txtExchangeIdentifier";
            this.txtExchangeIdentifier.Size = new System.Drawing.Size(154, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtExchangeIdentifier, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtExchangeIdentifier.TabIndex = 4;
            this.txtExchangeIdentifier.GotFocus += new System.EventHandler(this.txtExchangeIdentifier_GotFocus);
            this.txtExchangeIdentifier.LostFocus += new System.EventHandler(this.txtExchangeIdentifier_LostFocus);
            // 
            // lblExchangeIdentifier
            // 
            this.lblExchangeIdentifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblExchangeIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblExchangeIdentifier.Location = new System.Drawing.Point(12, 114);
            this.lblExchangeIdentifier.Name = "lblExchangeIdentifier";
            this.lblExchangeIdentifier.Size = new System.Drawing.Size(136, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblExchangeIdentifier, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblExchangeIdentifier.TabIndex = 79;
            this.lblExchangeIdentifier.Text = "AUEC Exchange Identifier";
            // 
            // label50
            // 
            this.label50.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label50.ForeColor = System.Drawing.Color.Red;
            this.label50.Location = new System.Drawing.Point(111, 92);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label50, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label50.TabIndex = 78;
            this.label50.Text = "*";
            // 
            // lblSymbolConvention
            // 
            this.lblSymbolConvention.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolConvention.Location = new System.Drawing.Point(12, 92);
            this.lblSymbolConvention.Name = "lblSymbolConvention";
            this.lblSymbolConvention.Size = new System.Drawing.Size(120, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblSymbolConvention, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblSymbolConvention.TabIndex = 20;
            this.lblSymbolConvention.Text = "Symbol Convention";
            // 
            // label44
            // 
            this.label44.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label44.ForeColor = System.Drawing.Color.Red;
            this.label44.Location = new System.Drawing.Point(80, 67);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label44, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label44.TabIndex = 19;
            this.label44.Text = "*";
            // 
            // txtExchangeNameFull
            // 
            this.txtExchangeNameFull.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExchangeNameFull.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtExchangeNameFull.Location = new System.Drawing.Point(259, 44);
            this.txtExchangeNameFull.MaxLength = 50;
            this.txtExchangeNameFull.Name = "txtExchangeNameFull";
            this.txtExchangeNameFull.Size = new System.Drawing.Size(154, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtExchangeNameFull, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtExchangeNameFull.TabIndex = 1;
            this.txtExchangeNameFull.GotFocus += new System.EventHandler(this.txtExchangeNameFull_GotFocus);
            this.txtExchangeNameFull.LostFocus += new System.EventHandler(this.txtExchangeNameFull_LostFocus);
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 1;
            this.label2.Text = "Short Name";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 0;
            this.label1.Text = "Full Name of Exchange";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(259, 66);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(154, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtShortName, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtShortName.TabIndex = 2;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(130, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label15, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label15.TabIndex = 16;
            this.label15.Text = "*";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.tabCtrlOtherFee);
            this.ultraTabPageControl2.Controls.Add(this.groupBox3);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(487, 608);
            // 
            // tabCtrlOtherFee
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCtrlOtherFee.ActiveTabAppearance = appearance5;
            this.tabCtrlOtherFee.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlOtherFee.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tabCtrlOtherFee.Location = new System.Drawing.Point(2, 3);
            this.tabCtrlOtherFee.Name = "tabCtrlOtherFee";
            this.tabCtrlOtherFee.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tabCtrlOtherFee.Size = new System.Drawing.Size(482, 572);
            this.tabCtrlOtherFee.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCtrlOtherFee.TabIndex = 13;
            this.tabCtrlOtherFee.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCtrlOtherFee_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(1, 20);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(480, 551);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.Controls.Add(this.txtPurchaseStamp);
            this.groupBox3.Controls.Add(this.txtPurchaseSecFees);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.txtSalesSecFees);
            this.groupBox3.Controls.Add(this.txtPurchaseLevy);
            this.groupBox3.Controls.Add(this.txtSaleLevy);
            this.groupBox3.Controls.Add(this.txtSaleStamp);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox3.Location = new System.Drawing.Point(2, 485);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(482, 120);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fee";
            this.groupBox3.Visible = false;
            // 
            // txtPurchaseStamp
            // 
            this.txtPurchaseStamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseStamp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPurchaseStamp.Location = new System.Drawing.Point(197, 58);
            this.txtPurchaseStamp.MaxLength = 9;
            this.txtPurchaseStamp.Name = "txtPurchaseStamp";
            this.txtPurchaseStamp.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtPurchaseStamp, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtPurchaseStamp.TabIndex = 3;
            this.txtPurchaseStamp.Text = "0";
            this.txtPurchaseStamp.GotFocus += new System.EventHandler(this.txtPurchaseStamp_GotFocus);
            this.txtPurchaseStamp.LostFocus += new System.EventHandler(this.txtPurchaseStamp_LostFocus);
            // 
            // txtPurchaseSecFees
            // 
            this.txtPurchaseSecFees.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseSecFees.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPurchaseSecFees.Location = new System.Drawing.Point(197, 36);
            this.txtPurchaseSecFees.MaxLength = 9;
            this.txtPurchaseSecFees.Name = "txtPurchaseSecFees";
            this.txtPurchaseSecFees.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtPurchaseSecFees, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtPurchaseSecFees.TabIndex = 1;
            this.txtPurchaseSecFees.Text = "0";
            this.txtPurchaseSecFees.GotFocus += new System.EventHandler(this.txtPurchaseSecFees_GotFocus);
            this.txtPurchaseSecFees.LostFocus += new System.EventHandler(this.txtPurchaseSecFees_LostFocus);
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.Location = new System.Drawing.Point(42, 82);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label17, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label17.TabIndex = 2;
            this.label17.Text = "Levy(BP)";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label21.Location = new System.Drawing.Point(42, 60);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(72, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label21, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label21.TabIndex = 1;
            this.label21.Text = "Stamp(BP)";
            // 
            // label16
            // 
            this.label16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label16.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label16.Location = new System.Drawing.Point(346, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(42, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label16, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label16.TabIndex = 0;
            this.label16.Text = "Sale";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(42, 38);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(132, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label19, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label19.TabIndex = 10;
            this.label19.Text = "Sec Fees(Cents/Share)";
            // 
            // label20
            // 
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label20.Location = new System.Drawing.Point(211, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label20, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label20.TabIndex = 11;
            this.label20.Text = "Purchase";
            this.label20.Click += new System.EventHandler(this.label20_Click);
            // 
            // txtSalesSecFees
            // 
            this.txtSalesSecFees.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSalesSecFees.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSalesSecFees.Location = new System.Drawing.Point(324, 36);
            this.txtSalesSecFees.MaxLength = 9;
            this.txtSalesSecFees.Name = "txtSalesSecFees";
            this.txtSalesSecFees.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSalesSecFees, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSalesSecFees.TabIndex = 2;
            this.txtSalesSecFees.Text = "0";
            this.txtSalesSecFees.GotFocus += new System.EventHandler(this.txtSalesSecFees_GotFocus);
            this.txtSalesSecFees.LostFocus += new System.EventHandler(this.txtSalesSecFees_LostFocus);
            // 
            // txtPurchaseLevy
            // 
            this.txtPurchaseLevy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseLevy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPurchaseLevy.Location = new System.Drawing.Point(197, 80);
            this.txtPurchaseLevy.MaxLength = 9;
            this.txtPurchaseLevy.Name = "txtPurchaseLevy";
            this.txtPurchaseLevy.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtPurchaseLevy, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtPurchaseLevy.TabIndex = 5;
            this.txtPurchaseLevy.Text = "0";
            this.txtPurchaseLevy.GotFocus += new System.EventHandler(this.txtPurchaseLevy_GotFocus);
            this.txtPurchaseLevy.LostFocus += new System.EventHandler(this.txtPurchaseLevy_LostFocus);
            // 
            // txtSaleLevy
            // 
            this.txtSaleLevy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSaleLevy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSaleLevy.Location = new System.Drawing.Point(324, 80);
            this.txtSaleLevy.MaxLength = 9;
            this.txtSaleLevy.Name = "txtSaleLevy";
            this.txtSaleLevy.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSaleLevy, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSaleLevy.TabIndex = 6;
            this.txtSaleLevy.Text = "0";
            this.txtSaleLevy.GotFocus += new System.EventHandler(this.txtSaleLevy_GotFocus);
            this.txtSaleLevy.LostFocus += new System.EventHandler(this.txtSaleLevy_LostFocus);
            // 
            // txtSaleStamp
            // 
            this.txtSaleStamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSaleStamp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSaleStamp.Location = new System.Drawing.Point(324, 58);
            this.txtSaleStamp.MaxLength = 9;
            this.txtSaleStamp.Name = "txtSaleStamp";
            this.txtSaleStamp.Size = new System.Drawing.Size(104, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSaleStamp, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSaleStamp.TabIndex = 4;
            this.txtSaleStamp.Text = "0";
            this.txtSaleStamp.GotFocus += new System.EventHandler(this.txtSaleStamp_GotFocus);
            this.txtSaleStamp.LostFocus += new System.EventHandler(this.txtSaleStamp_LostFocus);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.AutoScroll = true;
            this.ultraTabPageControl3.Controls.Add(this.btnRefresh);
            this.ultraTabPageControl3.Controls.Add(this.cBoxAuec);
            this.ultraTabPageControl3.Controls.Add(this.cmbCalendars);
            this.ultraTabPageControl3.Controls.Add(this.lblEditCalendar);
            this.ultraTabPageControl3.Controls.Add(this.lblCalendars);
            this.ultraTabPageControl3.Controls.Add(this.lblYear);
            this.ultraTabPageControl3.Controls.Add(this.CmbYear);
            this.ultraTabPageControl3.Controls.Add(this.grdHoliday);
            this.ultraTabPageControl3.Controls.Add(this.groupBox5);
            this.ultraTabPageControl3.Controls.Add(this.label57);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(487, 608);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(376, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnRefresh, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnRefresh.TabIndex = 68;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cBoxAuec
            // 
            this.cBoxAuec.AutoSize = true;
            this.cBoxAuec.Location = new System.Drawing.Point(11, 49);
            this.cBoxAuec.Name = "cBoxAuec";
            this.cBoxAuec.Size = new System.Drawing.Size(109, 17);
            this.inboxControlStyler1.SetStyleSettings(this.cBoxAuec, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cBoxAuec.TabIndex = 67;
            this.cBoxAuec.Text = "Show All Holidays";
            this.cBoxAuec.UseVisualStyleBackColor = true;
            this.cBoxAuec.CheckedChanged += new System.EventHandler(this.cBoxAuec_CheckedChanged);
            // 
            // cmbCalendars
            // 
            this.cmbCalendars.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCalendars.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCalendars.Location = new System.Drawing.Point(247, 17);
            this.cmbCalendars.Name = "cmbCalendars";
            this.cmbCalendars.Size = new System.Drawing.Size(100, 23);
            this.cmbCalendars.TabIndex = 66;
            this.cmbCalendars.ValueChanged += new System.EventHandler(this.cmbCalendars_ValueChanged);
            // 
            // lblEditCalendar
            // 
            this.lblEditCalendar.AutoSize = true;
            this.lblEditCalendar.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Pixel);
            this.lblEditCalendar.ForeColor = System.Drawing.Color.Blue;
            this.lblEditCalendar.Location = new System.Drawing.Point(139, 49);
            this.lblEditCalendar.Name = "lblEditCalendar";
            this.lblEditCalendar.Size = new System.Drawing.Size(161, 14);
            this.inboxControlStyler1.SetStyleSettings(this.lblEditCalendar, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblEditCalendar.TabIndex = 65;
            this.lblEditCalendar.Text = "Create/Edit Holiday Calendar";
            this.lblEditCalendar.Click += new System.EventHandler(this.lblEditCalendar_Click);
            // 
            // lblCalendars
            // 
            this.lblCalendars.AutoSize = true;
            this.lblCalendars.Location = new System.Drawing.Point(139, 20);
            this.lblCalendars.Name = "lblCalendars";
            this.lblCalendars.Size = new System.Drawing.Size(87, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblCalendars, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblCalendars.TabIndex = 64;
            this.lblCalendars.Text = "Select Calendars";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(7, 20);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblYear, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblYear.TabIndex = 57;
            this.lblYear.Text = "Year";
            // 
            // CmbYear
            // 
            this.CmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(42, 17);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(78, 21);
            this.inboxControlStyler1.SetStyleSettings(this.CmbYear, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.CmbYear.TabIndex = 56;
            this.CmbYear.SelectedValueChanged += new System.EventHandler(this.CmbYear_SelectedValueChanged);
            // 
            // grdHoliday
            // 
            this.grdHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHoliday.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdHoliday.DisplayLayout.GroupByBox.Hidden = true;
            this.grdHoliday.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHoliday.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdHoliday.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdHoliday.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            this.grdHoliday.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdHoliday.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdHoliday.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdHoliday.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdHoliday.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdHoliday.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdHoliday.Location = new System.Drawing.Point(4, 72);
            this.grdHoliday.Name = "grdHoliday";
            this.grdHoliday.Size = new System.Drawing.Size(460, 189);
            this.grdHoliday.TabIndex = 55;
            this.grdHoliday.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdHoliday.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox5.Controls.Add(this.lblSelectWeeklyHolidays);
            this.groupBox5.Controls.Add(this.checkedlstWeeklyHolidays);
            this.groupBox5.Controls.Add(this.label59);
            this.groupBox5.Controls.Add(this.btnUploadHolidays);
            this.groupBox5.Controls.Add(this.label58);
            this.groupBox5.Controls.Add(this.dttHolidayDate);
            this.groupBox5.Controls.Add(this.label40);
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.txtHolidayDescription);
            this.groupBox5.Controls.Add(this.btnAddHoliday);
            this.groupBox5.Controls.Add(this.btnDeleteHoliday);
            this.groupBox5.Controls.Add(this.btnLoadDefaultHolidays);
            this.groupBox5.Location = new System.Drawing.Point(4, 259);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(460, 232);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            // 
            // lblSelectWeeklyHolidays
            // 
            this.lblSelectWeeklyHolidays.Location = new System.Drawing.Point(115, 97);
            this.lblSelectWeeklyHolidays.Name = "lblSelectWeeklyHolidays";
            this.lblSelectWeeklyHolidays.Size = new System.Drawing.Size(115, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblSelectWeeklyHolidays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblSelectWeeklyHolidays.TabIndex = 28;
            this.lblSelectWeeklyHolidays.Text = "Select weekly holidays";
            // 
            // checkedlstWeeklyHolidays
            // 
            this.checkedlstWeeklyHolidays.CheckOnClick = true;
            this.checkedlstWeeklyHolidays.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstWeeklyHolidays.Location = new System.Drawing.Point(238, 97);
            this.checkedlstWeeklyHolidays.Name = "checkedlstWeeklyHolidays";
            this.checkedlstWeeklyHolidays.Size = new System.Drawing.Size(150, 132);
            this.inboxControlStyler1.SetStyleSettings(this.checkedlstWeeklyHolidays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedlstWeeklyHolidays.TabIndex = 27;
            // 
            // label59
            // 
            this.label59.ForeColor = System.Drawing.Color.Red;
            this.label59.Location = new System.Drawing.Point(189, 22);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label59, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label59.TabIndex = 26;
            this.label59.Text = "*";
            this.label59.Visible = false;
            // 
            // btnUploadHolidays
            // 
            this.btnUploadHolidays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(233)))), ((int)(((byte)(200)))));
            this.btnUploadHolidays.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadHolidays.BackgroundImage")));
            this.btnUploadHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUploadHolidays.Location = new System.Drawing.Point(243, 71);
            this.btnUploadHolidays.Name = "btnUploadHolidays";
            this.btnUploadHolidays.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnUploadHolidays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnUploadHolidays.TabIndex = 5;
            this.btnUploadHolidays.UseVisualStyleBackColor = false;
            this.btnUploadHolidays.Visible = false;
            this.btnUploadHolidays.Click += new System.EventHandler(this.btnUploadHolidays_Click);
            // 
            // label58
            // 
            this.label58.ForeColor = System.Drawing.Color.Red;
            this.label58.Location = new System.Drawing.Point(212, 46);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label58, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label58.TabIndex = 25;
            this.label58.Text = "*";
            this.label58.Visible = false;
            // 
            // dttHolidayDate
            // 
            this.dttHolidayDate.CustomFormat = "\'\'";
            this.dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dttHolidayDate.Location = new System.Drawing.Point(226, 20);
            this.dttHolidayDate.Name = "dttHolidayDate";
            this.dttHolidayDate.Size = new System.Drawing.Size(131, 21);
            this.inboxControlStyler1.SetStyleSettings(this.dttHolidayDate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.dttHolidayDate.TabIndex = 1;
            this.dttHolidayDate.Visible = false;
            this.dttHolidayDate.ValueChanged += new System.EventHandler(this.dttHolidayDate_ValueChanged);
            this.dttHolidayDate.GotFocus += new System.EventHandler(this.dttHolidayDate_GotFocus);
            this.dttHolidayDate.LostFocus += new System.EventHandler(this.dttHolidayDate_LostFocus);
            // 
            // label40
            // 
            this.label40.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label40.Location = new System.Drawing.Point(120, 22);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(68, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label40, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label40.TabIndex = 4;
            this.label40.Text = "Holiday Date";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label40.Visible = false;
            // 
            // label29
            // 
            this.label29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label29.Location = new System.Drawing.Point(120, 46);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(92, 17);
            this.inboxControlStyler1.SetStyleSettings(this.label29, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label29.TabIndex = 2;
            this.label29.Text = "Short Description";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label29.Visible = false;
            // 
            // txtHolidayDescription
            // 
            this.txtHolidayDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHolidayDescription.Location = new System.Drawing.Point(226, 44);
            this.txtHolidayDescription.MaxLength = 50;
            this.txtHolidayDescription.Name = "txtHolidayDescription";
            this.txtHolidayDescription.Size = new System.Drawing.Size(131, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtHolidayDescription, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtHolidayDescription.TabIndex = 2;
            this.txtHolidayDescription.Visible = false;
            this.txtHolidayDescription.GotFocus += new System.EventHandler(this.txtHolidayDescription_GotFocus);
            this.txtHolidayDescription.LostFocus += new System.EventHandler(this.txtHolidayDescription_LostFocus);
            // 
            // btnAddHoliday
            // 
            this.btnAddHoliday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAddHoliday.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddHoliday.BackgroundImage")));
            this.btnAddHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddHoliday.Location = new System.Drawing.Point(84, 72);
            this.btnAddHoliday.Name = "btnAddHoliday";
            this.btnAddHoliday.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnAddHoliday, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnAddHoliday.TabIndex = 3;
            this.btnAddHoliday.UseVisualStyleBackColor = false;
            this.btnAddHoliday.Visible = false;
            this.btnAddHoliday.Click += new System.EventHandler(this.btnAddHoliday_Click);
            // 
            // btnDeleteHoliday
            // 
            this.btnDeleteHoliday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDeleteHoliday.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteHoliday.BackgroundImage")));
            this.btnDeleteHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteHoliday.Location = new System.Drawing.Point(162, 72);
            this.btnDeleteHoliday.Name = "btnDeleteHoliday";
            this.btnDeleteHoliday.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnDeleteHoliday, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnDeleteHoliday.TabIndex = 4;
            this.btnDeleteHoliday.UseVisualStyleBackColor = false;
            this.btnDeleteHoliday.Visible = false;
            this.btnDeleteHoliday.Click += new System.EventHandler(this.btnDeleteHoliday_Click);
            // 
            // btnLoadDefaultHolidays
            // 
            this.btnLoadDefaultHolidays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btnLoadDefaultHolidays.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadDefaultHolidays.BackgroundImage")));
            this.btnLoadDefaultHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadDefaultHolidays.Location = new System.Drawing.Point(324, 72);
            this.btnLoadDefaultHolidays.Name = "btnLoadDefaultHolidays";
            this.btnLoadDefaultHolidays.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnLoadDefaultHolidays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnLoadDefaultHolidays.TabIndex = 6;
            this.btnLoadDefaultHolidays.UseVisualStyleBackColor = false;
            this.btnLoadDefaultHolidays.Visible = false;
            this.btnLoadDefaultHolidays.Click += new System.EventHandler(this.btnLoadDefaultHolidays_Click);
            // 
            // label57
            // 
            this.label57.ForeColor = System.Drawing.Color.Red;
            this.label57.Location = new System.Drawing.Point(7, 589);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(96, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label57, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label57.TabIndex = 24;
            this.label57.Text = "* Required Field";
            this.label57.Visible = false;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.grdSymbol);
            this.ultraTabPageControl4.Enabled = false;
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(487, 608);
            // 
            // grdSymbol
            // 
            this.grdSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSymbol.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdSymbol.DisplayLayout.GroupByBox.Hidden = true;
            this.grdSymbol.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSymbol.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdSymbol.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdSymbol.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSymbol.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSymbol.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSymbol.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdSymbol.Location = new System.Drawing.Point(3, 4);
            this.grdSymbol.Name = "grdSymbol";
            this.grdSymbol.Size = new System.Drawing.Size(481, 310);
            this.grdSymbol.TabIndex = 94;
            this.grdSymbol.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.groupBox7);
            this.ultraTabPageControl5.Controls.Add(this.groupBox6);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(487, 608);
            this.ultraTabPageControl5.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl5_Paint);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox7.Controls.Add(this.txtRoundLot);
            this.groupBox7.Controls.Add(this.label31);
            this.groupBox7.Controls.Add(this.label25);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.cmbOtherCurrency);
            this.groupBox7.Controls.Add(this.cmbCurrency);
            this.groupBox7.Controls.Add(this.txtMultiplier);
            this.groupBox7.Controls.Add(this.lblOtherCurrency);
            this.groupBox7.Controls.Add(this.label43);
            this.groupBox7.Controls.Add(this.lblBaseCurrency);
            this.groupBox7.Controls.Add(this.label22);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Controls.Add(this.cmbCurrencyConversion);
            this.groupBox7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox7.Location = new System.Drawing.Point(1, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(476, 147);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox7, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox7.TabIndex = 36;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Conversions";
            // 
            // txtRoundLot
            // 
            appearance6.FontData.BoldAsString = "False";
            this.txtRoundLot.Appearance = appearance6;
            this.txtRoundLot.ClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
            this.txtRoundLot.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtRoundLot.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Double;
            this.txtRoundLot.InputMask = "{double:10.10}";
            this.txtRoundLot.Location = new System.Drawing.Point(262, 108);
            this.txtRoundLot.Name = "txtRoundLot";
            this.txtRoundLot.NonAutoSizeHeight = 21;
            this.txtRoundLot.PromptChar = ' ';
            this.txtRoundLot.Size = new System.Drawing.Size(121, 21);
            this.txtRoundLot.TabIndex = 86;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.Red;
            this.label31.Location = new System.Drawing.Point(149, 108);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(14, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label31, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label31.TabIndex = 85;
            this.label31.Text = "*";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label25.Location = new System.Drawing.Point(92, 108);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(78, 21);
            this.inboxControlStyler1.SetStyleSettings(this.label25, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label25.TabIndex = 83;
            this.label25.Text = "Round Lot";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(138, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label3.TabIndex = 82;
            this.label3.Text = "*";
            // 
            // cmbOtherCurrency
            // 
            this.cmbOtherCurrency.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOtherCurrency.DisplayLayout.Appearance = appearance7;
            this.cmbOtherCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbOtherCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOtherCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOtherCurrency.DisplayLayout.GroupByBox.Appearance = appearance8;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOtherCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance9;
            this.cmbOtherCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOtherCurrency.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance10.BackColor2 = System.Drawing.SystemColors.Control;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOtherCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance10;
            this.cmbOtherCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOtherCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOtherCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.Highlight;
            appearance12.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOtherCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            this.cmbOtherCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOtherCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOtherCurrency.DisplayLayout.Override.CardAreaAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            appearance14.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOtherCurrency.DisplayLayout.Override.CellAppearance = appearance14;
            this.cmbOtherCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOtherCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance15.BackColor = System.Drawing.SystemColors.Control;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOtherCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance15;
            appearance16.TextHAlignAsString = "Left";
            this.cmbOtherCurrency.DisplayLayout.Override.HeaderAppearance = appearance16;
            this.cmbOtherCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOtherCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.Color.Silver;
            this.cmbOtherCurrency.DisplayLayout.Override.RowAppearance = appearance17;
            this.cmbOtherCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOtherCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance18;
            this.cmbOtherCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOtherCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOtherCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOtherCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOtherCurrency.DropDownWidth = 0;
            this.cmbOtherCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbOtherCurrency.Location = new System.Drawing.Point(262, 40);
            this.cmbOtherCurrency.MaxDropDownItems = 12;
            this.cmbOtherCurrency.Name = "cmbOtherCurrency";
            this.cmbOtherCurrency.Size = new System.Drawing.Size(121, 21);
            this.cmbOtherCurrency.TabIndex = 2;
            this.cmbOtherCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOtherCurrency.GotFocus += new System.EventHandler(this.cmbOtherCurrency_GotFocus);
            this.cmbOtherCurrency.LostFocus += new System.EventHandler(this.cmbOtherCurrency_LostFocus);
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrency.DisplayLayout.Appearance = appearance19;
            this.cmbCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.GroupByBox.Appearance = appearance20;
            appearance21.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance21;
            this.cmbCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrency.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance22;
            this.cmbCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.SystemColors.Highlight;
            appearance24.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance24;
            this.cmbCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.Override.CardAreaAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrency.DisplayLayout.Override.CellAppearance = appearance26;
            this.cmbCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance27.BackColor = System.Drawing.SystemColors.Control;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance27;
            appearance28.TextHAlignAsString = "Left";
            this.cmbCurrency.DisplayLayout.Override.HeaderAppearance = appearance28;
            this.cmbCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrency.DisplayLayout.Override.RowAppearance = appearance29;
            this.cmbCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance30;
            this.cmbCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrency.DropDownWidth = 0;
            this.cmbCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrency.Location = new System.Drawing.Point(262, 18);
            this.cmbCurrency.MaxDropDownItems = 12;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(121, 21);
            this.cmbCurrency.TabIndex = 1;
            this.cmbCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrency.ValueChanged += new System.EventHandler(this.cmbCurrency_ValueChanged);
            this.cmbCurrency.GotFocus += new System.EventHandler(this.cmbCurrency_GotFocus);
            this.cmbCurrency.LostFocus += new System.EventHandler(this.cmbCurrency_LostFocus);
            // 
            // txtMultiplier
            // 
            this.txtMultiplier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMultiplier.Location = new System.Drawing.Point(262, 84);
            this.txtMultiplier.MaxLength = 9;
            this.txtMultiplier.Name = "txtMultiplier";
            this.txtMultiplier.Size = new System.Drawing.Size(121, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtMultiplier, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtMultiplier.TabIndex = 4;
            // 
            // lblOtherCurrency
            // 
            this.lblOtherCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOtherCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOtherCurrency.Location = new System.Drawing.Point(92, 42);
            this.lblOtherCurrency.Name = "lblOtherCurrency";
            this.lblOtherCurrency.Size = new System.Drawing.Size(82, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblOtherCurrency, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblOtherCurrency.TabIndex = 78;
            this.lblOtherCurrency.Text = "Other Currency";
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(168, 20);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label43, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label43.TabIndex = 75;
            this.label43.Text = "*";
            // 
            // lblBaseCurrency
            // 
            this.lblBaseCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblBaseCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBaseCurrency.Location = new System.Drawing.Point(92, 20);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.Size = new System.Drawing.Size(80, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblBaseCurrency, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblBaseCurrency.TabIndex = 73;
            this.lblBaseCurrency.Text = "Base Currency";
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label22.Location = new System.Drawing.Point(92, 64);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(110, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label22, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label22.TabIndex = 71;
            this.label22.Text = "Currency Conversion";
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label23.Location = new System.Drawing.Point(92, 88);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label23, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label23.TabIndex = 68;
            this.label23.Text = "Multiplier";
            // 
            // cmbCurrencyConversion
            // 
            this.cmbCurrencyConversion.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            appearance31.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrencyConversion.DisplayLayout.Appearance = appearance31;
            this.cmbCurrencyConversion.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbCurrencyConversion.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrencyConversion.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance32.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance32.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance32.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyConversion.DisplayLayout.GroupByBox.Appearance = appearance32;
            appearance33.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyConversion.DisplayLayout.GroupByBox.BandLabelAppearance = appearance33;
            this.cmbCurrencyConversion.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrencyConversion.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance34.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance34.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyConversion.DisplayLayout.GroupByBox.PromptAppearance = appearance34;
            this.cmbCurrencyConversion.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrencyConversion.DisplayLayout.MaxRowScrollRegions = 1;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrencyConversion.DisplayLayout.Override.ActiveCellAppearance = appearance35;
            appearance36.BackColor = System.Drawing.SystemColors.Highlight;
            appearance36.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrencyConversion.DisplayLayout.Override.ActiveRowAppearance = appearance36;
            this.cmbCurrencyConversion.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrencyConversion.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyConversion.DisplayLayout.Override.CardAreaAppearance = appearance37;
            appearance38.BorderColor = System.Drawing.Color.Silver;
            appearance38.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrencyConversion.DisplayLayout.Override.CellAppearance = appearance38;
            this.cmbCurrencyConversion.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrencyConversion.DisplayLayout.Override.CellPadding = 0;
            appearance39.BackColor = System.Drawing.SystemColors.Control;
            appearance39.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance39.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance39.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyConversion.DisplayLayout.Override.GroupByRowAppearance = appearance39;
            appearance40.TextHAlignAsString = "Left";
            this.cmbCurrencyConversion.DisplayLayout.Override.HeaderAppearance = appearance40;
            this.cmbCurrencyConversion.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrencyConversion.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrencyConversion.DisplayLayout.Override.RowAppearance = appearance41;
            this.cmbCurrencyConversion.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance42.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrencyConversion.DisplayLayout.Override.TemplateAddRowAppearance = appearance42;
            this.cmbCurrencyConversion.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrencyConversion.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrencyConversion.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrencyConversion.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrencyConversion.DropDownWidth = 0;
            this.cmbCurrencyConversion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrencyConversion.Location = new System.Drawing.Point(262, 62);
            this.cmbCurrencyConversion.MaxDropDownItems = 12;
            this.cmbCurrencyConversion.Name = "cmbCurrencyConversion";
            this.cmbCurrencyConversion.Size = new System.Drawing.Size(121, 21);
            this.cmbCurrencyConversion.TabIndex = 3;
            this.cmbCurrencyConversion.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrencyConversion.GotFocus += new System.EventHandler(this.cmbCurrencyConversion_GotFocus);
            this.cmbCurrencyConversion.LostFocus += new System.EventHandler(this.cmbCurrencyConversion_LostFocus);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox6.Controls.Add(this.label60);
            this.groupBox6.Controls.Add(this.cmbIdentifierName);
            this.groupBox6.Controls.Add(this.cmbProvideidentifierwithtrade);
            this.groupBox6.Controls.Add(this.cmbProvideAccountnamewithTrade);
            this.groupBox6.Controls.Add(this.cmbShortSaleConfirmation);
            this.groupBox6.Controls.Add(this.label68);
            this.groupBox6.Controls.Add(this.label67);
            this.groupBox6.Controls.Add(this.label65);
            this.groupBox6.Controls.Add(this.lblIdentifierName);
            this.groupBox6.Controls.Add(this.label28);
            this.groupBox6.Controls.Add(this.label27);
            this.groupBox6.Controls.Add(this.label26);
            this.groupBox6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox6.Location = new System.Drawing.Point(3, 157);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(474, 155);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox6, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox6.TabIndex = 35;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Compliance";
            // 
            // label60
            // 
            this.label60.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label60.ForeColor = System.Drawing.Color.Red;
            this.label60.Location = new System.Drawing.Point(92, 118);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(102, 15);
            this.inboxControlStyler1.SetStyleSettings(this.label60, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label60.TabIndex = 37;
            this.label60.Text = "* Required Field";
            // 
            // cmbIdentifierName
            // 
            this.cmbIdentifierName.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbIdentifierName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbIdentifierName.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbIdentifierName.DropDownWidth = 0;
            this.cmbIdentifierName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbIdentifierName.Location = new System.Drawing.Point(262, 88);
            this.cmbIdentifierName.MaxDropDownItems = 12;
            this.cmbIdentifierName.Name = "cmbIdentifierName";
            this.cmbIdentifierName.Size = new System.Drawing.Size(121, 21);
            this.cmbIdentifierName.TabIndex = 10;
            this.cmbIdentifierName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbIdentifierName.GotFocus += new System.EventHandler(this.cmbIdentifierName_GotFocus);
            this.cmbIdentifierName.LostFocus += new System.EventHandler(this.cmbIdentifierName_LostFocus);
            // 
            // cmbProvideidentifierwithtrade
            // 
            this.cmbProvideidentifierwithtrade.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            appearance43.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Appearance = appearance43;
            this.cmbProvideidentifierwithtrade.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbProvideidentifierwithtrade.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbProvideidentifierwithtrade.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance44.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbProvideidentifierwithtrade.DisplayLayout.GroupByBox.Appearance = appearance44;
            appearance45.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbProvideidentifierwithtrade.DisplayLayout.GroupByBox.BandLabelAppearance = appearance45;
            this.cmbProvideidentifierwithtrade.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbProvideidentifierwithtrade.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance46.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance46.BackColor2 = System.Drawing.SystemColors.Control;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance46.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbProvideidentifierwithtrade.DisplayLayout.GroupByBox.PromptAppearance = appearance46;
            this.cmbProvideidentifierwithtrade.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbProvideidentifierwithtrade.DisplayLayout.MaxRowScrollRegions = 1;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.ActiveCellAppearance = appearance47;
            appearance48.BackColor = System.Drawing.SystemColors.Highlight;
            appearance48.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.ActiveRowAppearance = appearance48;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.CardAreaAppearance = appearance49;
            appearance50.BorderColor = System.Drawing.Color.Silver;
            appearance50.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.CellAppearance = appearance50;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.CellPadding = 0;
            appearance51.BackColor = System.Drawing.SystemColors.Control;
            appearance51.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance51.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance51.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.GroupByRowAppearance = appearance51;
            appearance52.TextHAlignAsString = "Left";
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.HeaderAppearance = appearance52;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.BorderColor = System.Drawing.Color.Silver;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.RowAppearance = appearance53;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance54.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbProvideidentifierwithtrade.DisplayLayout.Override.TemplateAddRowAppearance = appearance54;
            this.cmbProvideidentifierwithtrade.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbProvideidentifierwithtrade.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbProvideidentifierwithtrade.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbProvideidentifierwithtrade.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbProvideidentifierwithtrade.DropDownWidth = 0;
            this.cmbProvideidentifierwithtrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbProvideidentifierwithtrade.Location = new System.Drawing.Point(262, 66);
            this.cmbProvideidentifierwithtrade.MaxDropDownItems = 12;
            this.cmbProvideidentifierwithtrade.Name = "cmbProvideidentifierwithtrade";
            this.cmbProvideidentifierwithtrade.Size = new System.Drawing.Size(121, 21);
            this.cmbProvideidentifierwithtrade.TabIndex = 9;
            this.cmbProvideidentifierwithtrade.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbProvideidentifierwithtrade.ValueChanged += new System.EventHandler(this.cmbProvideidentifierwithtrade_ValueChanged);
            this.cmbProvideidentifierwithtrade.GotFocus += new System.EventHandler(this.cmbProvideidentifierwithtrade_GotFocus);
            this.cmbProvideidentifierwithtrade.LostFocus += new System.EventHandler(this.cmbProvideidentifierwithtrade_LostFocus);
            // 
            // cmbProvideAccountnamewithTrade
            // 
            this.cmbProvideAccountnamewithTrade.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance55.BackColor = System.Drawing.SystemColors.Window;
            appearance55.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Appearance = appearance55;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance56.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance56.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance56.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.GroupByBox.Appearance = appearance56;
            appearance57.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.GroupByBox.BandLabelAppearance = appearance57;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance58.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance58.BackColor2 = System.Drawing.SystemColors.Control;
            appearance58.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance58.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.GroupByBox.PromptAppearance = appearance58;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.MaxRowScrollRegions = 1;
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.ActiveCellAppearance = appearance59;
            appearance60.BackColor = System.Drawing.SystemColors.Highlight;
            appearance60.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.ActiveRowAppearance = appearance60;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance61.BackColor = System.Drawing.SystemColors.Window;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.CardAreaAppearance = appearance61;
            appearance62.BorderColor = System.Drawing.Color.Silver;
            appearance62.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.CellAppearance = appearance62;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.CellPadding = 0;
            appearance63.BackColor = System.Drawing.SystemColors.Control;
            appearance63.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance63.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance63.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.GroupByRowAppearance = appearance63;
            appearance64.TextHAlignAsString = "Left";
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.HeaderAppearance = appearance64;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance65.BackColor = System.Drawing.SystemColors.Window;
            appearance65.BorderColor = System.Drawing.Color.Silver;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.RowAppearance = appearance65;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance66.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.Override.TemplateAddRowAppearance = appearance66;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbProvideAccountnamewithTrade.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbProvideAccountnamewithTrade.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbProvideAccountnamewithTrade.DropDownWidth = 0;
            this.cmbProvideAccountnamewithTrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbProvideAccountnamewithTrade.Location = new System.Drawing.Point(262, 44);
            this.cmbProvideAccountnamewithTrade.MaxDropDownItems = 12;
            this.cmbProvideAccountnamewithTrade.Name = "cmbProvideAccountnamewithTrade";
            this.cmbProvideAccountnamewithTrade.Size = new System.Drawing.Size(121, 21);
            this.cmbProvideAccountnamewithTrade.TabIndex = 8;
            this.cmbProvideAccountnamewithTrade.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbProvideAccountnamewithTrade.GotFocus += new System.EventHandler(this.cmbProvideAccountnamewithTrade_GotFocus);
            this.cmbProvideAccountnamewithTrade.LostFocus += new System.EventHandler(this.cmbProvideAccountnamewithTrade_LostFocus);
            // 
            // cmbShortSaleConfirmation
            // 
            this.cmbShortSaleConfirmation.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            appearance67.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbShortSaleConfirmation.DisplayLayout.Appearance = appearance67;
            this.cmbShortSaleConfirmation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cmbShortSaleConfirmation.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbShortSaleConfirmation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance68.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance68.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance68.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance68.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbShortSaleConfirmation.DisplayLayout.GroupByBox.Appearance = appearance68;
            appearance69.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbShortSaleConfirmation.DisplayLayout.GroupByBox.BandLabelAppearance = appearance69;
            this.cmbShortSaleConfirmation.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbShortSaleConfirmation.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance70.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance70.BackColor2 = System.Drawing.SystemColors.Control;
            appearance70.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance70.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbShortSaleConfirmation.DisplayLayout.GroupByBox.PromptAppearance = appearance70;
            this.cmbShortSaleConfirmation.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbShortSaleConfirmation.DisplayLayout.MaxRowScrollRegions = 1;
            appearance71.BackColor = System.Drawing.SystemColors.Window;
            appearance71.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.ActiveCellAppearance = appearance71;
            appearance72.BackColor = System.Drawing.SystemColors.Highlight;
            appearance72.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.ActiveRowAppearance = appearance72;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.CardAreaAppearance = appearance73;
            appearance74.BorderColor = System.Drawing.Color.Silver;
            appearance74.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.CellAppearance = appearance74;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.CellPadding = 0;
            appearance75.BackColor = System.Drawing.SystemColors.Control;
            appearance75.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance75.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance75.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.GroupByRowAppearance = appearance75;
            appearance76.TextHAlignAsString = "Left";
            this.cmbShortSaleConfirmation.DisplayLayout.Override.HeaderAppearance = appearance76;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance77.BackColor = System.Drawing.SystemColors.Window;
            appearance77.BorderColor = System.Drawing.Color.Silver;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.RowAppearance = appearance77;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance78.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbShortSaleConfirmation.DisplayLayout.Override.TemplateAddRowAppearance = appearance78;
            this.cmbShortSaleConfirmation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbShortSaleConfirmation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbShortSaleConfirmation.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbShortSaleConfirmation.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbShortSaleConfirmation.DropDownWidth = 0;
            this.cmbShortSaleConfirmation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbShortSaleConfirmation.Location = new System.Drawing.Point(262, 22);
            this.cmbShortSaleConfirmation.MaxDropDownItems = 12;
            this.cmbShortSaleConfirmation.Name = "cmbShortSaleConfirmation";
            this.cmbShortSaleConfirmation.Size = new System.Drawing.Size(121, 21);
            this.cmbShortSaleConfirmation.TabIndex = 7;
            this.cmbShortSaleConfirmation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbShortSaleConfirmation.GotFocus += new System.EventHandler(this.cmbShortSaleConfirmation_GotFocus);
            this.cmbShortSaleConfirmation.LostFocus += new System.EventHandler(this.cmbShortSaleConfirmation_LostFocus);
            // 
            // label68
            // 
            this.label68.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label68.ForeColor = System.Drawing.Color.Red;
            this.label68.Location = new System.Drawing.Point(210, 26);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label68, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label68.TabIndex = 75;
            this.label68.Text = "*";
            // 
            // label67
            // 
            this.label67.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label67.ForeColor = System.Drawing.Color.Red;
            this.label67.Location = new System.Drawing.Point(228, 48);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label67, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label67.TabIndex = 74;
            this.label67.Text = "*";
            // 
            // label65
            // 
            this.label65.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label65.ForeColor = System.Drawing.Color.Red;
            this.label65.Location = new System.Drawing.Point(236, 70);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label65, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label65.TabIndex = 73;
            this.label65.Text = "*";
            // 
            // lblIdentifierName
            // 
            this.lblIdentifierName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIdentifierName.Location = new System.Drawing.Point(92, 92);
            this.lblIdentifierName.Name = "lblIdentifierName";
            this.lblIdentifierName.Size = new System.Drawing.Size(96, 14);
            this.inboxControlStyler1.SetStyleSettings(this.lblIdentifierName, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblIdentifierName.TabIndex = 66;
            this.lblIdentifierName.Text = "Name of Identifier";
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label28.Location = new System.Drawing.Point(92, 70);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(148, 14);
            this.inboxControlStyler1.SetStyleSettings(this.label28, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label28.TabIndex = 65;
            this.label28.Text = "Provide Identifier with Trade";
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label27.Location = new System.Drawing.Point(92, 48);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(158, 14);
            this.inboxControlStyler1.SetStyleSettings(this.label27, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label27.TabIndex = 64;
            this.label27.Text = "Provide Account name with Trade";
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label26.Location = new System.Drawing.Point(92, 26);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(126, 14);
            this.inboxControlStyler1.SetStyleSettings(this.label26, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label26.TabIndex = 63;
            this.label26.Text = "Short Sale Confirmation";
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.upnlAssetWisePermissions);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(487, 608);
            // 
            // upnlAssetWisePermissions
            // 
            // 
            // upnlAssetWisePermissions.ClientArea
            // 
            this.upnlAssetWisePermissions.ClientArea.Controls.Add(this.label32);
            this.upnlAssetWisePermissions.ClientArea.Controls.Add(this.label51);
            this.upnlAssetWisePermissions.ClientArea.Controls.Add(this.checkedlstSide);
            this.upnlAssetWisePermissions.ClientArea.Controls.Add(this.label55);
            this.upnlAssetWisePermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlAssetWisePermissions.Location = new System.Drawing.Point(0, 0);
            this.upnlAssetWisePermissions.Name = "upnlAssetWisePermissions";
            this.upnlAssetWisePermissions.Size = new System.Drawing.Size(487, 608);
            this.upnlAssetWisePermissions.TabIndex = 0;
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label32.ForeColor = System.Drawing.Color.Red;
            this.label32.Location = new System.Drawing.Point(83, 382);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(102, 15);
            this.inboxControlStyler1.SetStyleSettings(this.label32, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label32.TabIndex = 56;
            this.label32.Text = "* Required Field";
            // 
            // label51
            // 
            this.label51.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label51.ForeColor = System.Drawing.Color.Red;
            this.label51.Location = new System.Drawing.Point(107, 292);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(12, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label51, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label51.TabIndex = 55;
            this.label51.Text = "*";
            // 
            // checkedlstSide
            // 
            this.checkedlstSide.CheckOnClick = true;
            this.checkedlstSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstSide.Location = new System.Drawing.Point(253, 246);
            this.checkedlstSide.Name = "checkedlstSide";
            this.checkedlstSide.Size = new System.Drawing.Size(150, 116);
            this.inboxControlStyler1.SetStyleSettings(this.checkedlstSide, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedlstSide.TabIndex = 52;
            // 
            // label55
            // 
            this.label55.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label55.Location = new System.Drawing.Point(83, 292);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(30, 12);
            this.inboxControlStyler1.SetStyleSettings(this.label55, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label55.TabIndex = 53;
            this.label55.Text = "Side";
            // 
            // trvAsset
            // 
            this.trvAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trvAsset.BackColor = System.Drawing.Color.White;
            this.trvAsset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvAsset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.trvAsset.FullRowSelect = true;
            this.trvAsset.HideSelection = false;
            this.trvAsset.HotTracking = true;
            this.trvAsset.Location = new System.Drawing.Point(12, 2);
            this.trvAsset.Name = "trvAsset";
            this.trvAsset.ShowLines = false;
            this.trvAsset.Size = new System.Drawing.Size(184, 595);
            this.inboxControlStyler1.SetStyleSettings(this.trvAsset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.trvAsset.TabIndex = 0;
            this.trvAsset.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvAsset_AfterSelect);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(20, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnAdd, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbcAUEC
            // 
            appearance79.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance79.BackColor2 = System.Drawing.Color.White;
            appearance79.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tbcAUEC.ActiveTabAppearance = appearance79;
            this.tbcAUEC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcAUEC.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbcAUEC.Controls.Add(this.tbcExchangeDetails);
            this.tbcAUEC.Controls.Add(this.ultraTabPageControl2);
            this.tbcAUEC.Controls.Add(this.ultraTabPageControl3);
            this.tbcAUEC.Controls.Add(this.ultraTabPageControl4);
            this.tbcAUEC.Controls.Add(this.ultraTabPageControl5);
            this.tbcAUEC.Controls.Add(this.ultraTabPageControl1);
            this.tbcAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tbcAUEC.Location = new System.Drawing.Point(6, 0);
            this.tbcAUEC.Name = "tbcAUEC";
            this.tbcAUEC.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbcAUEC.Size = new System.Drawing.Size(489, 629);
            this.tbcAUEC.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcAUEC.TabIndex = 3;
            appearance80.ForeColor = System.Drawing.Color.Black;
            ultraTab6.ActiveAppearance = appearance80;
            ultraTab6.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab6.Key = "ExchangeDetails";
            ultraTab6.TabPage = this.tbcExchangeDetails;
            ultraTab6.Text = "Exchange Details";
            appearance81.ForeColor = System.Drawing.Color.Black;
            ultraTab7.ActiveAppearance = appearance81;
            ultraTab7.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab7.Key = "MarketFees";
            ultraTab7.TabPage = this.ultraTabPageControl2;
            ultraTab7.Text = "Market Fees";
            appearance82.ForeColor = System.Drawing.Color.Black;
            ultraTab8.ActiveAppearance = appearance82;
            ultraTab8.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab8.Key = "Holidays";
            ultraTab8.TabPage = this.ultraTabPageControl3;
            ultraTab8.Text = "Holidays";
            appearance83.ForeColor = System.Drawing.Color.Black;
            ultraTab9.ActiveAppearance = appearance83;
            ultraTab9.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab9.Key = "MasterSymbolList";
            ultraTab9.TabPage = this.ultraTabPageControl4;
            ultraTab9.Text = "Master Symbol List";
            ultraTab9.Visible = false;
            appearance84.ForeColor = System.Drawing.Color.Black;
            ultraTab10.ActiveAppearance = appearance84;
            ultraTab10.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab10.Key = "Compliance";
            ultraTab10.TabPage = this.ultraTabPageControl5;
            ultraTab10.Text = "Compliance";
            ultraTab1.Key = "AssetWisePermissions";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Asset Wise Permissions";
            this.tbcAUEC.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab1});
            this.tbcAUEC.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tbcAUEC_SelectedTabChanged);
            this.tbcAUEC.Click += new System.EventHandler(this.tbcAUEC_Click);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(487, 608);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(370, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnSave, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(450, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnClose, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(100, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnDelete, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbcAUEC);
            this.panel1.Location = new System.Drawing.Point(202, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 632);
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Location = new System.Drawing.Point(12, 615);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(711, 33);
            this.inboxControlStyler1.SetStyleSettings(this.panel3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel3.TabIndex = 6;
            // 
            // AUEC_Fill_Panel
            // 
            // 
            // AUEC_Fill_Panel.ClientArea
            // 
            this.AUEC_Fill_Panel.ClientArea.Controls.Add(this.trvAsset);
            this.AUEC_Fill_Panel.ClientArea.Controls.Add(this.panel3);
            this.AUEC_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.AUEC_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AUEC_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AUEC_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.AUEC_Fill_Panel.Name = "AUEC_Fill_Panel";
            this.AUEC_Fill_Panel.Size = new System.Drawing.Size(704, 662);
            this.AUEC_Fill_Panel.TabIndex = 0;
            // 
            // AUEC
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(704, 662);
            this.Controls.Add(this.AUEC_Fill_Panel);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 700);
            this.Name = "AUEC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Prana: AUEC";
            this.Load += new System.EventHandler(this.AUEC_Load);
            this.tbcExchangeDetails.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExchangeLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).EndInit();
            this.grpDtails.ResumeLayout(false);
            this.grpDtails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbolConvention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlOtherFee)).EndInit();
            this.tabCtrlOtherFee.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalendars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbol)).EndInit();
            this.ultraTabPageControl5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOtherCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyConversion)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifierName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvideidentifierwithtrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvideAccountnamewithTrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortSaleConfirmation)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.upnlAssetWisePermissions.ClientArea.ResumeLayout(false);
            this.upnlAssetWisePermissions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcAUEC)).EndInit();
            this.tbcAUEC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.AUEC_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AUEC_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        /// <summary>
        /// Various Bind methods are called on the on Load event of the AUEC form. These bind methods vary 
        /// from filling the Combo Boxes, Numeric UpDowns to Tree view controls used in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AUEC_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindSymbolConvention();
                BindIdentifiers();
                InitializeCompliance();
                BindSymbol();
                BindCountry();
                BindCurrency();
                BindOtherCurrency();
                BindUnits();
                BindStates();
                BindSide();
                BindYear();
                BindFlags();
                BindLogos();
                BindWeeklyHolidays();
                AddOtherFeeTabs();
                BindAssetTree();
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("AUEC_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "AUEC_Load", null);


                #endregion
            }
        }

        private void AddOtherFeeTabs()
        {
            //tbcAUEC.Tabs.Add(new Infragicstics.Win.UltraWinTabControl.UltraTabPageControl());
            //tbcAUEC.Tabs.Count
            //EnumerationValueList listForCalculationBasis = Prana.BusinessLogic.CommissionEnumHelper.GetNewListForCalculationBasis();
            EnumerationValueList listForOtherFee = Prana.Utilities.CommissionEnumHelper.GetOtherFeeList();
            Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbcFeeDetails = null;
            foreach (Prana.BusinessObjects.EnumerationValue otherFee in listForOtherFee)
            {
                //if (otherFee.DisplayText.Equals(OtherFeeType.None.ToString()))
                //{
                //    continue;
                //}
                //if (calculationBasis.DisplayText.Equals(CommissionCalculationBasis.AvgPrice))
                //{
                //    //
                //}
                //grpDtails.Visible = false;
                string tabPageNameControlName = TABNAME_INITIAL + otherFee.DisplayText;
                string tabPageName = TABPAGENAME_INITIAL + otherFee.DisplayText;
                string keyName = TABKEY_INITIAL + otherFee.DisplayText;
                string userCtrlName = USERCONTROLNAME_INITIAL + otherFee.DisplayText;
                string tabControlName = TABCONTROLNAME_INITIAL + otherFee.DisplayText;
                string otherFeeName = otherFee.Value.ToString();

                //Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbcFeeDetails;//tbcFeeDetails;
                tbcFeeDetails = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
                //tbcFeeDetails.SuspendLayout();
                //tbcFeeDetails.Controls.Add(this.groupBox2);
                //tbcFeeDetails.Controls.Add(this.grpDtails);
                tbcFeeDetails.Location = new System.Drawing.Point(15, 5);
                tbcFeeDetails.Name = tabPageNameControlName;
                //tbcFeeDetails.Size = new System.Drawing.Size(506, 608); BB
                //tbcFeeDetails.Size = new System.Drawing.Size(490, 590); 

                Infragistics.Win.Appearance appearanceFee = new Infragistics.Win.Appearance();
                appearanceFee.ForeColor = System.Drawing.Color.Black;

                Infragistics.Win.UltraWinTabControl.UltraTab ultraTabFee = new Infragistics.Win.UltraWinTabControl.UltraTab();
                ultraTabFee.ActiveAppearance = appearanceFee;
                ultraTabFee.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
                ultraTabFee.Key = keyName;
                ultraTabFee.TabPage = tbcFeeDetails;
                ultraTabFee.Text = otherFeeName;

                Prana.Admin.Controls.OtherFee ctrlOtherFee = new Prana.Admin.Controls.OtherFee();
                //ctrlOtherFee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                ctrlOtherFee.Location = new System.Drawing.Point(10, 10);
                ctrlOtherFee.Name = userCtrlName;
                //ctrlOtherFee.Size = new System.Drawing.Size(480, 580);
                ctrlOtherFee.OtherFeeType = (OtherFeeType)Enum.Parse(typeof(OtherFeeType), otherFee.DisplayText);
                //ctrlOtherFee.TabIndex = 45;

                tbcFeeDetails.Controls.Add(ctrlOtherFee);
                //ultraTabFee.
                //tbcFeeDetails.Controls.Add(tbcFeeDetails);
                //ultraTabFee.TabPage = tbcFeeDetails;



                tabCtrlOtherFee.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTabFee
            });

                tabCtrlOtherFee.Controls.Add(tbcFeeDetails);
                //tbcFeeDetails.ResumeLayout(false);
                ctrlOtherFee.SetupControl();
                //tabCtrlOtherFee.Tabs.Add(
            }
            //tabCtrlOtherFee.Controls.Add(tbcFeeDetails);
        }

        private void BindFlags()
        {
            Prana.Admin.BLL.Flags flags = GeneralManager.GetFlags();
            //Inserting the - Select - option in the Combo Box at the top.
            flags.Insert(0, new Flag(int.MinValue, C_COMBO_SELECT, null));
            this.cmbFlag.DataSource = null;
            this.cmbFlag.DataSource = flags;
            this.cmbFlag.DisplayMember = "CountryFlagName";
            this.cmbFlag.ValueMember = "CountryFlagID";
            this.cmbFlag.Value = int.MinValue;
        }

        private void BindFlags(Flags flags)
        {
            //Inserting the - Select - option in the Combo Box at the top.
            //flags.Insert(0, new Flag(int.MinValue, C_COMBO_SELECT, null));
            cmbFlag.Refresh();
            cmbFlag.DataSource = null;
            cmbFlag.DataSource = flags;
            cmbFlag.DisplayMember = "CountryFlagName";
            cmbFlag.ValueMember = "CountryFlagID";
            cmbFlag.Value = int.MinValue;
            cmbFlag.DisplayLayout.Bands[0].Columns["CountryFlagID"].Hidden = true;
            cmbFlag.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This combo is binded by all the existing logos in the database.
        /// </summary>
        private void BindLogos()
        {
            //GetLogos method fetches the existing logos from the database.
            Logos logos = GeneralManager.GetLogos();
            //Inserting the - Select - option in the Combo Box at the top.
            logos.Insert(0, new Logo(int.MinValue, C_COMBO_SELECT, null));

            cmbExchangeLogo.DataSource = null;
            cmbExchangeLogo.DataSource = logos;
            cmbExchangeLogo.DisplayMember = "LogoName";
            cmbExchangeLogo.ValueMember = "LogoID";
            cmbExchangeLogo.Value = int.MinValue;
        }

        private void BindLogos(Logos logos)
        {
            //Inserting the - Select - option in the Combo Box at the top.
            cmbExchangeLogo.Refresh();
            cmbExchangeLogo.DataSource = null;


            cmbExchangeLogo.DisplayMember = "LogoName";
            cmbExchangeLogo.ValueMember = "LogoID";
            cmbExchangeLogo.DataSource = logos;
            cmbExchangeLogo.Value = int.MinValue;
            cmbExchangeLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

            cmbExchangeLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This method binds the existing <see cref="SymbolConvention"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindSymbolConvention()
        {
            //GetSymbolConvention method fetches the existing symbolConventions from the database.
            Prana.Admin.BLL.SymbolConventions symbolConventions = SymbolManager.GetSymbolConventions();
            //Inserting the - Select - option in the Combo Box at the top.
            symbolConventions.Insert(0, new Prana.Admin.BLL.SymbolConvention(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            this.cmbSymbolConvention.DataSource = null;
            this.cmbSymbolConvention.DataSource = symbolConventions;
            this.cmbSymbolConvention.DisplayMember = "SymbolConventionName";
            this.cmbSymbolConvention.ValueMember = "SymbolConventionID";
            this.cmbSymbolConvention.Value = int.MinValue;


        }

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindCurrency()
        {
            //GetCurrencies method fetches the existing currencies from the database.
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
            //Inserting the - Select - option in the Combo Box at the top.
            currencies.Insert(0, new Prana.Admin.BLL.Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            this.cmbCurrency.DataSource = null;
            this.cmbCurrency.DataSource = currencies;
            this.cmbCurrency.DisplayMember = "CurrencySymbol";
            this.cmbCurrency.ValueMember = "CurencyID";
            this.cmbCurrency.Value = int.MinValue;
            //this.cmbCurrency.co


        }

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property. This combo box is relatively same to the currency 
        /// combobox except the logical sense that this control holds the other currency which can be traded 
        /// on the exchange.
        /// </summary>
        private void BindOtherCurrency()
        {
            //GetCurrencies method fetches the existing currencies from the database.
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
            //Inserting the - Select - option in the Combo Box at the top.
            currencies.Insert(0, new Prana.Admin.BLL.Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            this.cmbOtherCurrency.DataSource = null;
            this.cmbOtherCurrency.DataSource = currencies;
            this.cmbOtherCurrency.DisplayMember = "CurrencySymbol";
            //this.cmbOtherCurrency.DisplayMember = "CurrencyName";
            this.cmbOtherCurrency.ValueMember = "CurencyID";
            this.cmbOtherCurrency.Value = int.MinValue;
            this.cmbCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;


        }
        private void BindYear()
        {
            try
            {
                List<int> year = new List<int>();
                foreach (Calendar calendar in _calendars)
                {
                    if (!year.Contains(calendar.CalendarYear))
                    {
                        year.Add(calendar.CalendarYear);
                    }
                }

                if (year.Count != 0)
                {
                    year.Sort();
                    CmbYear.DataSource = year;
                    CmbYear.SelectedItem = (object)DateTime.Now.Year;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
        /// countries object to its datasource property.
        /// </summary>
        private void BindCountry()
        {
            //GetCountries method fetches the existing countries from the database.
            Countries countries = GeneralManager.GetCountries();
            //Inserting the - Select - option in the Combo Box at the top.
            countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
            cmbCountry.DisplayMember = "Name";
            cmbCountry.ValueMember = "CountryID";
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
        }

        /// <summary>
        /// This method binds the existing <see cref="Units"/> in the ComboBox control by assigning the 
        /// units object to its datasource property.
        /// </summary>
        private void BindUnits()
        {
            //GetUnits method fetches the existing units from the database.
            Units units = AUECManager.GetUnits();
            //Inserting the - Select - option in the Combo Box at the top.
            units.Insert(0, new Prana.Admin.BLL.Unit(int.MinValue, C_COMBO_SELECT));
            cmbUnits.DisplayMember = "UnitName";
            cmbUnits.ValueMember = "UnitID";
            cmbUnits.DataSource = null;
            cmbUnits.DataSource = units;
            cmbUnits.Value = int.MinValue;
            this.cmbUnits.LostFocus += new System.EventHandler(this.cmbUnits_LostFocus);
            this.cmbUnits.GotFocus += new System.EventHandler(this.cmbUnits_GotFocus);

        }

        /// <summary>
        /// This method binds the existing <see cref="Symbols"/> in the ComboBox control by assigning the 
        /// symbols object to its datasource property.
        /// </summary>
        private void BindSymbol()
        {
            //GetSymbols method fetches the existing symbols from the database.
            Symbols symbols = AUECManager.GetSymbols();
            //Creating the DataTable to be used as a source for the ComboBox.
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Company");
            dt.Columns.Add("Symbol");
            object[] row = new object[2];

            foreach (Symbol symbol in symbols)
            {
                row[0] = symbol.CompanySymbol;
                row[1] = symbol.Company;
                dt.Rows.Add(row);
            }
            grdSymbol.DataSource = dt;
        }

        /// <summary>
        /// This method binds the existing <see cref="Identifiers"/> in the ComboBox control by assigning the 
        /// identifiers object to its datasource property.
        /// </summary>
        private void BindIdentifiers()
        {
            //GetIdentifiers method fetches the existing symbols from the database.
            Identifiers identifiers = AUECManager.GetIdentifiers();
            //Checking the object for the null value before assigning it to the combo box.
            //			if (identifiers.Count > 0 )
            //			{
            //Inserting the - Select - option in the Combo Box at the top.
            identifiers.Insert(0, new Prana.Admin.BLL.Identifier(int.MinValue, "None"));
            cmbIdentifierName.DataSource = null;
            cmbIdentifierName.DataSource = identifiers;
            cmbIdentifierName.DisplayMember = "IdentifierName";
            cmbIdentifierName.ValueMember = "IdentifierID";
            cmbIdentifierName.Text = "None";
            //			}
        }

        /// <summary>
        /// This method binds the existing <see cref="States"/> in the ComboBox control by assigning the 
        /// states object to its datasource property.
        /// </summary>
        private void BindStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = GeneralManager.GetStates();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
        }

        /// <summary>
        /// This method empties the ComboBox from any state by assigning the states object to null value.
        /// </summary>
        private void BindEmptyStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = new States();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
        }

        /// <summary>
        /// This method binds the listbox with all the available sides in the database.
        /// </summary>
        private void BindSide()
        {
            Prana.Admin.BLL.Sides sides = OrderManager.GetSides();
            if (sides.Count > 0)
            {
                checkedlstSide.DataSource = sides;
                checkedlstSide.DisplayMember = "Name";
                checkedlstSide.ValueMember = "SideID";
            }
        }



        /// <summary>
        /// This method binds the listbox with the list of week days in the database to be used as weekly holidays.
        /// </summary>
        private void BindWeeklyHolidays()
        {
            List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection = new List<Prana.BusinessObjects.WeeklyHoliday>();
            weeklyHolidaysCollection = AUECManager.GetAllWeeklyHolidaysCollection();
            if (weeklyHolidaysCollection.Count > 0)
            {
                checkedlstWeeklyHolidays.DataSource = null;
                checkedlstWeeklyHolidays.DataSource = weeklyHolidaysCollection;
                checkedlstWeeklyHolidays.DisplayMember = "WeeklyHolidayName";
                checkedlstWeeklyHolidays.ValueMember = "WeeklyHolidayID";
            }
        }

        private bool _isStored = false;
        private bool _isStoredCopy = false;
        //private int _assetID = int.MinValue;
        //private int _underlyingID = int.MinValue;
        //private int _exchangeID ; //= int.MinValue;

        /// <summary>
        /// This method binds the existing <see cref="Assets"/>, <see cref="UnderLyings"/>, 
        /// <see cref="Exchanges"/> in the Tree control mealwhile assigning the id to each node as it becomes 
        /// the part of the tree. These id's have the value corresponding to the type of what they are ie. 
        /// Asset, UnderLying or Exchange. These id's then become the NodeID of the tree, which are further 
        /// used to fetch the corresponding details pertaining to it from the database. 
        /// </summary>
        private void BindAssetTree()
        {
            //To clear the tree of any node before binding it afresh.
            trvAsset.Nodes.Clear();

            bool alExi = false;
            int assetID = int.MinValue;
            int underLyingID = int.MinValue;
            //bool existsAlready = false;
            int exchangeID = int.MinValue;
            bool newExchange = false;
            int assetLoop = 0;
            int underlyingLoop = 0;
            int exchangeLoop = 0;
            int selectedAssetNode = assetLoop;
            int selectedUnderLyingNode = underlyingLoop;
            int selectedExchangeNode = exchangeLoop;
            //gotFirstNode varriable is used to check that whether there is any node in the tree to bind.
            bool gotFirstNode = false;

            //GetAsse ts method is used to fetch the existing assets in the database.
            //Assets assets = AssetManager.GetAssets();	
            //Assets assets = AssetManager.GetAUECAssets();	

            int newAUECAssetID = int.MinValue;
            int newAUECUnderLyingID = int.MinValue;
            int newAUECExchangeID = int.MinValue;
            if (newAUEC != null)
            {
                newAUECAssetID = newAUEC.AssetID;
                newAUECUnderLyingID = newAUEC.UnderlyingID;
                newAUECExchangeID = newAUEC.ExchangeID;
            }
            else
            {
                newAUECAssetID = int.MinValue;
                newAUECUnderLyingID = int.MinValue;
                newAUECExchangeID = int.MinValue;
            }
            Prana.Admin.BLL.Assets assets = AssetManager.GetAUECAssets(newAUECAssetID);

            //Loop through all the assets and assigning each asset node an id corresponding to its unique 
            //value in the database.
            foreach (Prana.Admin.BLL.Asset asset in assets)
            {
                assetLoop++;
                underlyingLoop = 0;
                exchangeLoop = 0;
                assetID = int.Parse(asset.AssetID.ToString());
                Font font = new Font("Vedana", 8.25F, System.Drawing.FontStyle.Bold);

                TreeNode treeNodeAsset = new TreeNode(asset.Name);
                //Making the root node to bold by assigning it to the font object defined above. 
                treeNodeAsset.NodeFont = font;
                NodeDetails assetNode = new NodeDetails(NodeType.Asset, asset.AssetID, exchangeID);
                treeNodeAsset.Tag = assetNode;// asset.AssetID;
                //bool isExchange = false;

                Prana.Admin.BLL.UnderLyings auecUnderLyings = new Prana.Admin.BLL.UnderLyings();
                if (newAUECAssetID == asset.AssetID)
                {
                    auecUnderLyings = AssetManager.GetAUECUnderLyings(asset.AssetID, newAUECUnderLyingID);
                }
                else
                {
                    //auecUnderLyings = AssetManager.GetAUECUnderLyings(asset.AssetID, int.MinValue);
                    auecUnderLyings = AssetManager.GetAUECUnderLyings(asset.AssetID, int.MinValue);
                }

                //foreach(UnderLying underLying in asset.UnderLyings)
                foreach (Prana.Admin.BLL.UnderLying underLying in auecUnderLyings)
                {
                    underlyingLoop++;
                    exchangeLoop = 0;
                    underLyingID = int.Parse(underLying.UnderlyingID.ToString());
                    TreeNode treeNodeUnderlying = new TreeNode(underLying.Name);
                    treeNodeUnderlying.NodeFont = font;
                    NodeDetails underlyingNode = new NodeDetails(NodeType.Underlying, underLying.UnderlyingID, exchangeID);
                    treeNodeUnderlying.Tag = underlyingNode; //underLying.UnderlyingID;
                    //GetAUEC method is used to get all the asset-underlying-exchange-currency combinations 
                    //corresponding to the particular assetID and underlyingID combination.
                    Prana.Admin.BLL.AUECs auecs = AUECManager.GetAUEC(asset.AssetID, underLying.UnderlyingID);
                    Prana.Admin.BLL.AUEC hhAUEC = new Prana.Admin.BLL.AUEC();

                    if (_isStored == true && auecs.Count <= 0 && asset.AssetID == newAUECAssetID && underLying.UnderlyingID == newAUECUnderLyingID)
                    {
                        alExi = false;
                        newExchange = true;

                    }
                    else
                    {
                        foreach (Prana.Admin.BLL.AUEC auec in auecs)
                        {
                            if (_isStored == true && auec.AssetID == newAUECAssetID && auec.UnderlyingID == newAUECUnderLyingID)
                            {
                                if (newAUECExchangeID == auec.ExchangeID)
                                {
                                    alExi = true;
                                    newExchange = false;
                                    MessageBox.Show(this, "This AUEC already exists.", "Prana Alert", MessageBoxButtons.OK);
                                    break;
                                }
                                else
                                {
                                    alExi = false;
                                    newExchange = true;
                                }
                            }
                        }
                    }
                    if (newExchange == true)
                    {
                        if (alExi == false)
                        {
                            exchangeID = int.Parse(newAUECExchangeID.ToString());
                            auecs.Add(newAUEC);
                            newExchange = false;
                            _isStoredCopy = true;
                            _isStored = false; //So that at next time the newly added exchange is not shown again without saving it.
                        }
                    }

                    //auecExchanges = AUECManager.get
                    //isExchange = false;
                    foreach (Prana.Admin.BLL.AUEC auec in auecs)
                    {
                        //isExchange = true;
                        if (gotFirstNode == false)
                        {
                            selectedAssetNode = assetLoop - 1;
                            selectedUnderLyingNode = underlyingLoop - 1;
                            selectedExchangeNode = exchangeLoop;
                            gotFirstNode = true;
                        }

                        exchangeLoop++;
                        Prana.Admin.BLL.Exchange exchange = auec.Exchange;

                        TreeNode treeNodeExchange = new TreeNode();

                        //Was used earlier before the changes 27-09-06.
                        //NodeDetails exchangeNode = new NodeDetails(NodeType.Exchange, auec.AUECExchangeID, auec.ExchangeID);

                        if (auec.AUECID > 0)
                        {
                            treeNodeExchange = new TreeNode(auec.DisplayName);
                        }
                        else
                        {
                            treeNodeExchange = new TreeNode(exchange.DisplayName);
                        }
                        //NodeDetails exchangeNode = new NodeDetails(NodeType.Exchange, auec.AUECID, auec.ExchangeID); 
                        NodeDetails exchangeNode = new NodeDetails(NodeType.Exchange, auec.AUECID, assetID, underLyingID, auec.ExchangeID);
                        treeNodeExchange.Tag = exchangeNode;//auec.AUECID;




                        //Check to make sure that the new AUECExchange added is not already exists in the DB or tree.  
                        //(auec.AUECID > 0) is checked so that atleast already existing node is added.
                        //						if(newAUEC.ExchangeID == auec.ExchangeID && newAUEC.AssetID == assetID && newAUEC.UnderlyingID == underLyingID && auec.AUECID > 0)
                        //						{
                        //							existsAlready = true;
                        //						}
                        //						
                        //
                        //						if(auec.AUECExchangeID == exchange.AUECExchangeID)
                        //						{
                        //							if(existsAlready == true)
                        //							{
                        //								existsAlready = false;
                        //								//MessageBox.Show("The newly added AUECExchange already exists !!");
                        //							}
                        //							else
                        //							{
                        treeNodeUnderlying.Nodes.Add(treeNodeExchange);
                        //							}
                        //						}



                    }
                    if (underLying.UnderlyingID > 0)
                    {
                        treeNodeAsset.Nodes.Add(treeNodeUnderlying);
                    }
                }

                if (asset.AssetID > 0)
                {
                    trvAsset.Nodes.Add(treeNodeAsset);
                }
            }

            //The tree is expanded by showing all the inner nodes of the root nodes present in the tree control.
            trvAsset.ExpandAll();
            //Selecting the first auec combination node in the tree if it exists.
            if (assets.Count > 0 && gotFirstNode == true && trvAsset.Nodes.Count > 0)
            {
                trvAsset.SelectedNode = trvAsset.Nodes[selectedAssetNode].Nodes[selectedUnderLyingNode].Nodes[selectedExchangeNode];
            }
            else
            {
                //Making the tabs disabled if there is nothing to show in the tree.
                tbcAUEC.Enabled = false;
            }
        }

        /// <summary>
        /// BindHoliday method binds holidays assigned to the seleced node or exchange in the tree.
        /// </summary>
        public void BindHoliday()
        {
            try
            {
                if (trvAsset.SelectedNode == null)
                {
                    //				Common.ResetStatusPanel(stbAUEC);
                    //				Common.SetStatusPanel(stbAUEC, "Please select Exchange!");
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    tbcAUEC.Enabled = true;
                    DisableTabsForAssetNode(nodeDetails.Type);
                    switch (nodeDetails.Type)
                    {
                        case NodeType.Asset:
                            tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                            break;
                        case NodeType.Underlying:
                            tbcAUEC.Enabled = false;
                            break;

                        case NodeType.Exchange:
                            int auecID = nodeDetails.NodeID;
                            int exchangeID = int.MinValue;

                            Prana.Admin.BLL.AUEC auec = new Prana.Admin.BLL.AUEC();
                            if (auecID > 0)//if some existing AUECexchange is selected. 
                            {
                                //auecExchange = AUECManager.GetAUECExchange(auecExchangeID);
                                auec = AUECManager.GetAUECDetails(auecID);
                                exchangeID = int.Parse(auec.ExchangeID.ToString());
                            }
                            else
                            {
                                exchangeID = int.Parse(newAUEC.ExchangeID.ToString());//To load default holidays for a selected exchange.
                            }

                            //cBoxAuec.CheckedChanged +=new EventHandler(cBoxAuec_CheckedChanged);
                            //GetHolidays method fetches the holidays for the selected exchange from the database.
                            Holidays holidays = null;
                            if (!cBoxAuec.Checked)
                            {
                                if (CmbYear.SelectedValue != null)
                                {
                                    holidays = AUECManager.GetHolidays(auecID, exchangeID, int.Parse(CmbYear.SelectedValue.ToString()), 0);
                                }
                            }
                            else
                            {
                                holidays = AUECManager.GetHolidays(auecID, exchangeID, int.MinValue, 1);
                            }

                            //Assigning the holiday grid's datasource property to holidays object if it has some values.
                            if (holidays != null && holidays.Count != 0)
                            {
                                this.grdHoliday.DataSource = holidays;

                                grdHoliday.Rows.Band.Columns["AUECExchangeID"].Hidden = true;
                                grdHoliday.Rows.Band.Columns["ExchangeID"].Hidden = true;
                                grdHoliday.Rows.Band.Columns["HolidayID"].Hidden = true;

                                foreach (Holiday holiday in holidays)
                                {
                                    holiday.AUECID = auecID;
                                    //holiday.HolidayID = int.MinValue;
                                }

                                ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
                                foreach (UltraGridColumn column in columns)
                                {
                                    if (column.Key != "Date")
                                    {
                                        if (column.Key != "Description")
                                        {
                                            // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                                            if (column.Key != "MarketOff")
                                            {
                                                if (column.Key != "SettlementOff")
                                                {
                                                    column.Hidden = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Hidden = false;
                                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
                                grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 1;
                                grdHoliday.DisplayLayout.Bands[0].Columns["MarketOff"].Header.VisiblePosition = 2;
                                grdHoliday.DisplayLayout.Bands[0].Columns["SettlementOff"].Header.VisiblePosition = 3;
                                grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                            }
                            else
                            {
                                Holiday holiay = new Holiday(int.MinValue, int.MinValue, "", Prana.BusinessObjects.DateTimeConstants.MinValue);
                                Holidays nullHolidays = new Holidays();
                                nullHolidays.Add(holiay);
                                grdHoliday.DataSource = nullHolidays;
                                //grdHoliday.Rows[0].Hidden = true;

                                ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
                                foreach (UltraGridColumn column in columns)
                                {
                                    if (column.Key != "StringDate")
                                    {
                                        if (column.Key != "Description")
                                        {
                                            // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                                            if (column.Key != "MarketOff")
                                            {
                                                if (column.Key != "SettlementOff")
                                                {
                                                    column.Hidden = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!grdHoliday.DisplayLayout.Bands[0].Columns.Exists("StringDate"))
                                {
                                    grdHoliday.DisplayLayout.Bands[0].Columns.Add("StringDate", "Date");
                                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                                    //grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
                                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                                    //grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Width = 246;
                                }
                                else
                                {
                                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                                    //grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Width = 246;
                                    grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                                    grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                                }
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }



        /// <summary>
        /// Deleting the particular node of the tree on the click event of the delete button in the form. The
        /// node is checked whether it is selected or not. Also it is checked for its type ie. Asset, 
        /// Underlying or Exchange and then calling particular delete method depending upon its type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool deleteResult = false;
                //Check whether any node is selected or not.
                //				if(trvAsset.SelectedNode.Parent != null)
                //				{
                if (trvAsset.SelectedNode == null)
                {
                    //Nothing is selected in Node tree.
                    //						Common.ResetStatusPanel(stbAUEC);
                    //						Common.SetStatusPanel(stbAUEC, "Please select Exchange!");
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    tbcAUEC.Enabled = true;
                    DisableTabsForAssetNode(nodeDetails.Type);
                    switch (nodeDetails.Type)
                    {

                        case NodeType.Asset:
                            tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                            break;
                        case NodeType.Underlying:
                            tbcAUEC.Enabled = false;
                            break;
                        case NodeType.Exchange:
                            //Exchange(AUEC) is selected in Node tree.
                            int auecID = nodeDetails.NodeID;
                            NodeDetails assetNode = (NodeDetails)trvAsset.SelectedNode.Parent.Parent.Tag;
                            NodeDetails underLyingNode = (NodeDetails)trvAsset.SelectedNode.Parent.Tag;
                            int assetID = assetNode.NodeID;
                            int underLyingID = underLyingNode.NodeID;

                            if (MessageBox.Show(this, "Do you want to delete this AUEC:"
                                + trvAsset.SelectedNode.Parent.Parent.Text
                                + "/" + trvAsset.SelectedNode.Parent.Text
                                + "/" + trvAsset.SelectedNode.Text + " ?", "AUEC Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (auecID != int.MinValue)
                                {
                                    bool flag = AUECManager.DeleteAUECExchange(auecID, assetID, underLyingID);
                                    if (flag == false)
                                    {
                                        MessageBox.Show(this, "This AUEC is referred in CounterPartyVenne/CommissionRules/Company.\n Please remove references first to delete it.", "Prana Alert");
                                    }
                                    else
                                    {
                                        AUECManager.DeleteSM_AECCS(auecID, 0);
                                        deleteResult = true;
                                    }
                                }
                                else
                                {
                                    deleteResult = true;
                                }
                            }
                            break;
                    }
                    //Check if some node is delelted or not.
                    if (deleteResult)
                    {
                        //After deleting rebinding tree as to show the existing values from the database.
                        newAUEC = null;
                        BindAssetTree();
                    }
                }
            }
            //			}
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnEdit_Click", null);


                #endregion
            }
        }

        Prana.Admin.BLL.AUEC newAUEC = null;
        private NewAUEC frmNewAUEC = null;
        /// <summary>
        /// This method adds a node in the tree on click event of the add button in the form. After clicking 
        /// the add button a pop up opens asking the user to select particular asset, underlying and exchange
        /// to add in the tree. After selecting the valid options from the popup, when the Save button is 
        /// clicked the particular exchange is added in the tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = null;
                NodeDetails selectedNodeDetails = null;
                int exchangeID = int.MinValue;
                if ((TreeNode)trvAsset.SelectedNode != null)
                {
                    nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    selectedNodeDetails = new NodeDetails(NodeType.Asset, nodeDetails.NodeID, exchangeID);
                }
                if (frmNewAUEC == null)
                {
                    frmNewAUEC = new NewAUEC();
                }
                if (trvAsset.SelectedNode != null)
                {
                    switch (nodeDetails.Type)
                    {
                        case NodeType.Asset:
                            frmNewAUEC.AssetID = nodeDetails.NodeID;
                            frmNewAUEC.UnderlyingID = int.MinValue;
                            break;

                        case NodeType.Underlying:
                            frmNewAUEC.UnderlyingID = nodeDetails.NodeID;
                            frmNewAUEC.AssetID = ((NodeDetails)trvAsset.SelectedNode.Parent.Tag).NodeID;
                            break;

                        case NodeType.Exchange:
                            frmNewAUEC.AssetID = ((NodeDetails)trvAsset.SelectedNode.Parent.Parent.Tag).NodeID;
                            frmNewAUEC.UnderlyingID = ((NodeDetails)trvAsset.SelectedNode.Parent.Tag).NodeID;
                            selectedNodeDetails = new NodeDetails(NodeType.Exchange, nodeDetails.NodeID, exchangeID);
                            break;
                    }
                }
                frmNewAUEC.ShowDialog(this);
                //The popup dialog when disposed, returns the auec object.
                if (frmNewAUEC.SavedAUEC != null)
                {
                    newAUEC = new Prana.Admin.BLL.AUEC();
                    newAUEC = frmNewAUEC.SavedAUEC;
                }

                //				//Check to make sure that the new AUECExchange added is not already exists in the DB or tree. 
                //				int assetID = int.Parse(newAUEC.AssetID.ToString());
                //				int underLyingID = int.Parse(newAUEC.UnderlyingID.ToString());
                //				exchangeID = int.Parse(newAUEC.ExchangeID.ToString());
                //				//xchan



                //If the object returned is not null then the tree is again binded.

                _isStored = frmNewAUEC.IsStored;

                if (newAUEC != null)
                {
                    BindAssetTree();
                }
                if (selectedNodeDetails != null)
                {
                    SelectTreeNode(selectedNodeDetails);

                    //If the object returned is not null then the newly added AUEC is shown selected.
                    if (newAUEC != null)
                    {
                        ReSetTree(newAUEC);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnAdd_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnEdit_Click", null);


                #endregion
            }
        }


        /// <summary>
        /// This method selects the node in the tree as per the object passed to it by the auecExchangeID.
        /// </summary>
        /// <param name="auec"></param>
        private void ReSetTreeAfterSave(Prana.Admin.BLL.AUEC auec)
        {
            int assetCounter = 0;
            int underlyingCounter = 0;
            //			int exchangeCounter = 0;

            foreach (TreeNode node in trvAsset.Nodes)
            {
                //Loop through ASSET nodes
                if (((NodeDetails)node.Tag).NodeID == auec.AssetID)
                {
                    foreach (TreeNode underlyingNode in trvAsset.Nodes[assetCounter].Nodes)
                    {
                        //Loop through Underlying nodes
                        if (((NodeDetails)underlyingNode.Tag).NodeID == auec.UnderlyingID)
                        {
                            foreach (TreeNode exchangeNode in trvAsset.Nodes[assetCounter].Nodes[underlyingCounter].Nodes)
                            {
                                //Loop through Exchange nodes
                                //if(((NodeDetails)exchangeNode.Tag).NodeID == auec.AUECExchangeID)
                                if (((NodeDetails)exchangeNode.Tag).NodeID == auec.AUECID)
                                {
                                    trvAsset.SelectedNode = exchangeNode;
                                }
                            }
                        }
                        underlyingCounter++;
                    }
                    break;
                }
                assetCounter++;
            }
        }

        /// <summary>
        /// This method selects the node in the tree as per the object passed to it.
        /// </summary>
        /// <param name="auec"></param>
        private void ReSetTree(Prana.Admin.BLL.AUEC auec)
        {
            int assetCounter = 0;
            int underlyingCounter = 0;
            //			int exchangeCounter = 0;

            foreach (TreeNode node in trvAsset.Nodes)
            {
                //Loop through ASSET nodes
                if (((NodeDetails)node.Tag).NodeID == auec.AssetID)
                {
                    foreach (TreeNode underlyingNode in trvAsset.Nodes[assetCounter].Nodes)
                    {
                        //Loop through Underlying nodes
                        if (((NodeDetails)underlyingNode.Tag).NodeID == auec.UnderlyingID)
                        {
                            foreach (TreeNode exchangeNode in trvAsset.Nodes[assetCounter].Nodes[underlyingCounter].Nodes)
                            {
                                //Loop through Exchange nodes
                                if (((NodeDetails)exchangeNode.Tag).NodeID == auec.AUECID)
                                {
                                    trvAsset.SelectedNode = exchangeNode;
                                }
                            }
                        }
                        underlyingCounter++;
                    }
                    break;
                }
                assetCounter++;
            }
        }

        /// <summary>
        /// This method closes the form when clicked the close button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        //        private void AUEC_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        //        {
        //////			if(MdiParent.ActiveMdiChild != this)
        //////			{				
        //////				BindAssetTree();	
        //////			}
        //        }

        private bool ValidateExchangeForm()
        {
            bool result = false;
            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^\d+$");

            errorProvider1.SetError(cmbSymbolConvention, "");
            errorProvider1.SetError(txtExchangeIdentifier, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(txtDayLightSaving, "");
            errorProvider1.SetError(txtSettlementDays, "");
            errorProvider1.SetError(txtSettlementDaysSell, "");
            errorProvider1.SetError(cmbUnits, "");
            errorProvider1.SetError(cmbTimeZone, "");
            errorProvider1.SetError(cmbCurrency, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(txtExchangeNameFull, "");
            errorProvider1.SetError(cmbCurrencyConversion, "");

            if (trvAsset.SelectedNode == null)
            {
                errorProvider1.SetError(trvAsset, "Please select a valid node!");
            }
            else
            {
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                tbcAUEC.Enabled = true;
                DisableTabsForAssetNode(nodeDetails.Type);
                switch (nodeDetails.Type)
                {

                    case NodeType.Asset:
                        tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                        break;

                    case NodeType.Underlying:
                        tbcAUEC.Enabled = false;
                        break;
                    case NodeType.Exchange:
                        //Exchange(AUEC) is selecte in Node tree.
                        int aUECExc = nodeDetails.NodeID;
                        int aUECExchangeID = nodeDetails.NodeID;

                        //Exchange exchange = new Exchange();
                        if (txtExchangeNameFull.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtExchangeNameFull, "Please Enter Exchange Full Name.!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtExchangeNameFull.Focus();
                            return result;
                        }

                        if (txtShortName.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtShortName, "Please Enter Exchange Short Name!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtShortName.Focus();
                            return result;
                        }

                        if (txtExchangeIdentifier.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtExchangeIdentifier, "Please Enter Identifier!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtExchangeIdentifier.Focus();
                            return result;
                        }

                        if (int.Parse(cmbSymbolConvention.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbSymbolConvention, "Please Select Symbol Convention!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            cmbSymbolConvention.Focus();
                            return result;
                        }

                        if (int.Parse(cmbUnits.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbUnits, "Please Enter Units!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            cmbUnits.Focus();
                            return result;
                        }

                        if (txtSettlementDays.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtSettlementDays, "Please Enter Settlement Days!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtSettlementDays.Focus();
                            return result;
                        }

                        if (!rgnumber.IsMatch(txtSettlementDays.Text.Trim()))
                        {
                            errorProvider1.SetError(txtSettlementDays, "Please Enter integer value!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtSettlementDays.Focus();
                            return result;
                        }
                        if (txtSettlementDaysSell.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtSettlementDaysSell, "Please Enter Settlement Days!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtSettlementDaysSell.Focus();
                            return result;
                        }

                        if (!rgnumber.IsMatch(txtSettlementDaysSell.Text.Trim()))
                        {
                            errorProvider1.SetError(txtSettlementDaysSell, "Please Enter integer value!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtSettlementDaysSell.Focus();
                            return result;
                        }

                        if (txtDayLightSaving.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtDayLightSaving, "Please Enter Day light savings!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            txtDayLightSaving.Focus();
                            return result;
                        }

                        if (int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbCountry, "Please Select Country!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            cmbCountry.Focus();
                            return result;
                        }

                        if (int.Parse(cmbState.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbState, "Please Select State!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABEXCHANGE];
                            cmbState.Focus();
                            return result;
                        }
                        result = true;
                        break;
                }
            }
            return result;
        }

        //Compliance validateCompliance = new Compliance();
        private bool ValidateComplianceForm(Prana.Admin.BLL.AUEC validatedAUEC, Prana.Admin.BLL.Sides validateSides, Prana.Admin.BLL.OrderTypes validateOrderTypes)
        {
            bool result = false;
            decimal temp;
            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*(\.\d+)?$");

            errorProvider1.SetError(cmbCurrency, "");
            errorProvider1.SetError(cmbOtherCurrency, "");
            errorProvider1.SetError(cmbShortSaleConfirmation, "");
            errorProvider1.SetError(cmbProvideAccountnamewithTrade, "");
            errorProvider1.SetError(cmbProvideidentifierwithtrade, "");
            errorProvider1.SetError(cmbIdentifierName, "");
            errorProvider1.SetError(txtMultiplier, "");
            errorProvider1.SetError(txtRoundLot, "");

            if (trvAsset.SelectedNode == null)
            {
                errorProvider1.SetError(trvAsset, "Please select a valid node!");
            }
            else
            {
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                switch (nodeDetails.Type)
                {

                    case NodeType.Asset:
                        tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                        break;

                    case NodeType.Underlying:
                        tbcAUEC.Enabled = false;
                        break;
                    case NodeType.Exchange:
                        if (int.Parse(cmbCurrency.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbCurrency, "Please Select Base Currency!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            cmbCurrency.Focus();
                            result = false;
                            return result;
                        }
                        else if (txtMultiplier.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtMultiplier, "Please enter Multiplier!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            txtMultiplier.Focus();
                            result = false;
                            return result;
                        }
                        else if (!rgnumber.IsMatch(txtMultiplier.Text.Trim()))
                        {
                            errorProvider1.SetError(txtMultiplier, "Please enter numeric values for Multiplier!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            txtMultiplier.Focus();
                            result = false;
                            return result;
                        }
                        // validation for round lot, PRANA-11159
                        else if (txtRoundLot.Text.Trim().Length == 0)
                        {
                            errorProvider1.SetError(txtRoundLot, "Please enter Round Lot!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            txtRoundLot.Focus();
                            result = false;
                            return result;
                        }
                        else if (!decimal.TryParse(txtRoundLot.Text.Trim(), out temp))
                        {
                            errorProvider1.SetError(txtRoundLot, "Please enter numeric values for Round Lot!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            txtRoundLot.Focus();
                            result = false;
                            return result;
                        }
                        else if (Convert.ToDecimal(txtRoundLot.Text.Trim()) <= 0)
                        {
                            errorProvider1.SetError(txtRoundLot, "Please enter Round Lot greater than 0!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            txtRoundLot.Focus();
                            result = false;
                            return result;
                        }

                        else if (int.Parse(cmbShortSaleConfirmation.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbShortSaleConfirmation, "Please Select Short Sale Confirmation!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            cmbShortSaleConfirmation.Focus();
                            result = false;
                            return result;
                        }
                        else if (int.Parse(cmbProvideAccountnamewithTrade.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbProvideAccountnamewithTrade, "Please Select Provident Account Name with Trade!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            cmbProvideAccountnamewithTrade.Focus();
                            result = false;
                            return result;
                        }
                        else if (int.Parse(cmbProvideidentifierwithtrade.Value.ToString()) == int.MinValue)
                        {
                            errorProvider1.SetError(cmbProvideidentifierwithtrade, "Please Select Identifier with trade!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            cmbProvideidentifierwithtrade.Focus();
                            result = false;
                            return result;
                        }

                        else if (int.Parse(cmbIdentifierName.Value.ToString()) == int.MinValue && cmbProvideidentifierwithtrade.Text.ToString() == "Yes")
                        {
                            errorProvider1.SetError(cmbIdentifierName, "Please Select Identifier!");
                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABCOMPLIANCE];
                            cmbIdentifierName.Focus();
                            result = false;
                            return result;
                        }
                        else
                        {
                            result = true;

                            validatedAUEC.CurrencyID = int.Parse(cmbCurrency.Value.ToString());
                            validatedAUEC.OtherCurrencyID = int.Parse(cmbOtherCurrency.Value.ToString());
                            validatedAUEC.Multiplier = double.Parse(txtMultiplier.Text.ToString());
                            validatedAUEC.IsShortSaleConfirmation = (int.Parse(cmbShortSaleConfirmation.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ShortSaleConfirmation : (int)Prana.Admin.BLL.Options.No);
                            validatedAUEC.ProvideAccountNameWithTrade = (int.Parse(cmbProvideAccountnamewithTrade.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ProvidentAccountNameWithTrade : (int)Prana.Admin.BLL.Options.No);
                            validatedAUEC.ProvideIdentifierNameWithTrade = (int.Parse(cmbProvideidentifierwithtrade.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ProvidentIdentifierWithTrade : (int)Prana.Admin.BLL.Options.No);
                            validatedAUEC.IdentifierID = int.Parse(cmbIdentifierName.Value.ToString());
                            validatedAUEC.UnitID = int.Parse(cmbUnits.Value.ToString());

                            int sideID = int.MinValue;
                            Prana.Admin.BLL.Side side = new Prana.Admin.BLL.Side();

                            for (int i = 0, count = checkedlstSide.CheckedItems.Count; i < count; i++)
                            {
                                sideID = int.Parse((((Prana.Admin.BLL.Side)checkedlstSide.CheckedItems[i]).SideID.ToString()));
                                validateSides.Add(new Prana.Admin.BLL.Side(sideID, "", ""));
                            }

                        }
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// This method calls the save methods for Exchange, Market and Compliance on click event of the 
        /// button in the form. If any one of these methods returns false ie. falis to save then it exits the 
        /// method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (trvAsset.SelectedNode == null)
                {
                    errorProvider1.SetError(trvAsset, "A valid node should be selected before saving process");
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;

                    switch (nodeDetails.Type)
                    {
                        case NodeType.Asset:
                            SaveDataForAssetLevelTabs(nodeDetails);
                            break;
                        case NodeType.Exchange:
                            SaveDataForAuecLevelTabs();
                            break;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }

        private void SaveDataForAssetLevelTabs(NodeDetails nodeDetails)
        {
            try
            {
                errorProvider1.SetError(checkedlstSide, "");

                if (checkedlstSide.CheckedItems.Count <= 0)
                {
                    errorProvider1.SetError(checkedlstSide, "Please Select Side!");
                    tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABASSETWISEPERMISSIONS];
                    checkedlstSide.Focus();
                    return;
                }

                int sideID = int.MinValue;
                int assetID = nodeDetails.NodeID;
                Prana.Admin.BLL.Sides sides = new Prana.Admin.BLL.Sides();
                Prana.Admin.BLL.Side side = new Prana.Admin.BLL.Side();

                for (int i = 0, count = checkedlstSide.CheckedItems.Count; i < count; i++)
                {
                    sideID = int.Parse((((Prana.Admin.BLL.Side)checkedlstSide.CheckedItems[i]).SideID.ToString()));
                    sides.Add(new Prana.Admin.BLL.Side(sideID, "", ""));
                }
                if (!AUECManager.SaveAssetSide(assetID, sides))
                {
                    errorProvider1.SetError(checkedlstSide, "The Sides could not be saved due to an internal error");
                    tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABASSETWISEPERMISSIONS];
                    checkedlstSide.Focus();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SaveDataForAuecLevelTabs()
        {
            bool saveExchangeDetails = false;
            bool saveMaketFees = false;
            bool saveCompliance = false;

            Prana.Admin.BLL.Exchange exchange = new Prana.Admin.BLL.Exchange();
            Holidays newHolidays = new Holidays();
            MarketFee newMarketFee = new MarketFee();

            saveExchangeDetails = ValidateExchangeForm();
            if (saveExchangeDetails == true)
            {
                Prana.Admin.BLL.AUEC auecDetail = new Prana.Admin.BLL.AUEC();
                Prana.Admin.BLL.Sides validateSides = new Prana.Admin.BLL.Sides();
                Prana.Admin.BLL.OrderTypes validateOrderTypes = new Prana.Admin.BLL.OrderTypes();
                saveCompliance = ValidateComplianceForm(auecDetail, validateSides, validateOrderTypes);
                if (saveCompliance == true)
                {
                    if (_isStoredCopy == true)
                    {
                        newHolidays = ((Holidays)grdHoliday.DataSource);
                    }
                    if (SaveMaketFees() == true)
                    {
                        auecDetail.PurchaseLevy = Double.Parse(txtPurchaseLevy.Text.ToString());
                        auecDetail.PurchaseSecFees = Double.Parse(txtPurchaseSecFees.Text.ToString());
                        auecDetail.PurchaseStamp = Double.Parse(txtPurchaseStamp.Text.ToString());
                        auecDetail.SaleLevy = Double.Parse(txtSaleLevy.Text.ToString());
                        auecDetail.SaleSecFees = Double.Parse(txtSalesSecFees.Text.ToString());
                        auecDetail.SaleStamp = Double.Parse(txtSaleStamp.Text.ToString());

                        EnumerationValueList listForOtherFee = Prana.Utilities.CommissionEnumHelper.GetOtherFeeList();
                        List<OtherFeeRule> allOtherFeeRules = new List<OtherFeeRule>();
                        int index = 0;
                        saveMaketFees = true;
                        foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabCtrlOtherFee.Tabs)
                        {
                            OtherFeeRule otherFeeRule = new OtherFeeRule();
                            Prana.BusinessObjects.EnumerationValue otherFeeValue =
                                (Prana.BusinessObjects.EnumerationValue)listForOtherFee[index];

                            string usrCtrlName = USERCONTROLNAME_INITIAL + otherFeeValue.DisplayText;
                            string tbcName = TABCONTROLNAME_INITIAL + otherFeeValue.DisplayText;

                            Prana.Admin.Controls.OtherFee otherFeeControl =
                                (Prana.Admin.Controls.OtherFee)tabCtrlOtherFee.Tabs[index].TabPage.Controls[usrCtrlName];
                            if (otherFeeControl.IsOtherFeeRuleEntered().Equals(true))
                            {
                                otherFeeRule = otherFeeControl.GetValidatedOtherFeeRule();
                                if (otherFeeRule == null)
                                {
                                    saveMaketFees = false;
                                    break;
                                }
                                allOtherFeeRules.Add(otherFeeRule);
                            }
                            index++;
                        }

                        bool IsCyclePresentInLong = isCalculationFeeBasisCyclic(allOtherFeeRules, true);
                        bool IsCyclePresentInShort = isCalculationFeeBasisCyclic(allOtherFeeRules, false);
                        if (IsCyclePresentInLong && IsCyclePresentInShort)
                        {
                            MessageBox.Show("Please select valid calculation basis. Beacuse it has cyclic dependancy with other fee.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            saveMaketFees = false;
                        }
                        else if (IsCyclePresentInLong)
                        {
                            MessageBox.Show("Please select valid Long calculation basis. Beacuse it has cyclic dependancy with other fee.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            saveMaketFees = false;
                        }
                        else if (IsCyclePresentInShort)
                        {
                            MessageBox.Show("Please select valid Short calculation basis. Beacuse it has cyclic dependancy with other fee.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            saveMaketFees = false;
                        }

                        if (saveMaketFees.Equals(true))
                        {
                            auecDetail = SaveCompliance(auecDetail);

                            int resultantAUECID = SaveExchangeDetails(auecDetail);
                            if (resultantAUECID > 0)
                            {
                                if (saveExchangeDetails)
                                {
                                    BindFlags();
                                    BindLogos();
                                }
                                else
                                {
                                    return;
                                }
                                bool nullHoliday = false;
                                Holidays holidays = new Holidays();
                                if (_isStoredCopy == true)
                                {
                                    holidays = newHolidays;

                                    txtPurchaseLevy.Text = newMarketFee.PurchaseLevy.ToString();
                                    txtPurchaseSecFees.Text = newMarketFee.PurchaseSecFees.ToString();
                                    txtPurchaseStamp.Text = newMarketFee.PurchaseStamp.ToString();
                                    txtSaleLevy.Text = newMarketFee.SaleLevy.ToString();
                                    txtSalesSecFees.Text = newMarketFee.SaleSecFees.ToString();
                                    txtSaleStamp.Text = newMarketFee.SaleStamp.ToString();

                                    _isStoredCopy = false;
                                }
                                else
                                {
                                    holidays = ((Holidays)grdHoliday.DataSource);
                                }

                                if (holidays.Count == 1)
                                {
                                    foreach (Holiday holiday in holidays)
                                    {
                                        if (holiday.Description == "")
                                        {
                                            nullHoliday = true;
                                        }
                                    }
                                }
                                Prana.Admin.BLL.AUECs auecs = new Prana.Admin.BLL.AUECs();
                                Prana.Admin.BLL.AUEC auec = AUECManager.GetAUEC(resultantAUECID);
                                auecs.Add(auec);


                                if (!cBoxAuec.Checked)
                                {
                                    if ((cmbCalendars.SelectedRow != null) && (CmbYear.SelectedValue != null))
                                    {
                                        errorProvider1.SetError(cmbCalendars, "");
                                        if (cmbCalendars.SelectedRow.Cells["CalendarName"].Text != C_COMBO_SELECT)
                                        {
                                            string name = cmbCalendars.SelectedRow.Cells["CalendarName"].Text;
                                            int year = int.Parse(CmbYear.SelectedValue.ToString());
                                            if (nullHoliday)
                                            {
                                                holidays.Clear();
                                            }
                                            AUECManager.UpdateAuecHolidays(holidays, year, auecs);

                                            AUECManager.SaveCalendarAUEC(name, year, resultantAUECID);

                                            // To save AUEC audit details
                                            if (auecDetail.AUECID == int.MinValue)
                                            {
                                                AuditManager.BLL.AuditHandler.GetInstance()
                                                    .AuditDataForGivenInstance(ctrlAUECAudit, auec,
                                                        AuditManager.Definitions.Enum.AuditAction.AUECCreated);
                                            }
                                            else
                                            {
                                                AuditManager.BLL.AuditHandler.GetInstance()
                                                    .AuditDataForGivenInstance(ctrlAUECAudit, auecDetail,
                                                        AuditManager.Definitions.Enum.AuditAction.AUECUpdated);
                                            }
                                        }
                                        else
                                        {
                                            errorProvider1.SetError(cmbCalendars, "Please select holiday calendar");
                                            tbcAUEC.SelectedTab = tbcAUEC.Tabs[2];
                                            cmbCalendars.Focus();
                                        }
                                    }
                                }

                                List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection =
                                    new List<Prana.BusinessObjects.WeeklyHoliday>();

                                int weeklyHolidayID = 0;
                                string weeklyHolidayName = string.Empty;
                                for (int i = 0, count = checkedlstWeeklyHolidays.CheckedItems.Count; i < count; i++)
                                {
                                    weeklyHolidayID =
                                        int.Parse(
                                            (((Prana.BusinessObjects.WeeklyHoliday)checkedlstWeeklyHolidays.CheckedItems[i])
                                                .WeeklyHolidayID.ToString()));
                                    weeklyHolidayName =
                                        (((Prana.BusinessObjects.WeeklyHoliday)checkedlstWeeklyHolidays.CheckedItems[i])
                                            .WeeklyHolidayName);
                                    weeklyHolidaysCollection.Add(new Prana.BusinessObjects.WeeklyHoliday(weeklyHolidayID,
                                        weeklyHolidayName, resultantAUECID));
                                }
                                int resultWeekOffSave = AUECManager.SaveAUECWeeklyHolidays(weeklyHolidaysCollection,
                                    resultantAUECID);

                                foreach (OtherFeeRule otherFeeRule in allOtherFeeRules)
                                {
                                    otherFeeRule.AUECID = resultantAUECID;
                                }
                                int resultOtherFeeSave = AUECManager.SaveAUECOtherFees(resultantAUECID, allOtherFeeRules);


                                BindAssetTree();
                                auecDetail.AUECID = resultantAUECID;
                                ReSetTreeAfterSave(auecDetail);

                            }
                        }
                    }
                }
            }
        }


        public int _aUECExchangeID = int.MinValue;
        public int _auecID = int.MinValue;

        private int SaveExchangeDetails(Prana.Admin.BLL.AUEC auecDetail)
        {
            int result = int.MinValue;
            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^\d+$");

            errorProvider1.SetError(cmbSymbolConvention, "");
            //errorProvider1.SetError(txtExchangeIdentifier, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(txtDayLightSaving, "");
            errorProvider1.SetError(txtSettlementDays, "");
            errorProvider1.SetError(txtSettlementDaysSell, "");
            errorProvider1.SetError(cmbUnits, "");
            errorProvider1.SetError(cmbTimeZone, "");
            errorProvider1.SetError(cmbCurrency, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(txtExchangeNameFull, "");
            errorProvider1.SetError(cmbCurrencyConversion, "");

            if (trvAsset.SelectedNode == null)
            {
                errorProvider1.SetError(trvAsset, "Please select Exchange!");
            }
            else
            {
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                tbcAUEC.Enabled = true;
                DisableTabsForAssetNode(nodeDetails.Type);
                switch (nodeDetails.Type)
                {

                    case NodeType.Asset:
                        tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                        break;

                    case NodeType.Underlying:
                        tbcAUEC.Enabled = false;
                        break;
                    case NodeType.Exchange:
                        int aUECExc = nodeDetails.NodeID;
                        int aUECExchangeID = nodeDetails.NodeID;
                        int auecID = nodeDetails.NodeID;
                        auecDetail.AUECID = auecID;
                        auecDetail.AssetID = nodeDetails.AssetID;
                        auecDetail.UnderlyingID = nodeDetails.UnderLyingID;
                        auecDetail.ExchangeID = nodeDetails.ExchangeID;

                        auecDetail.FullName = txtExchangeNameFull.Text;
                        auecDetail.DisplayName = txtShortName.Text;
                        auecDetail.SymbolConventionID = int.Parse(cmbSymbolConvention.Value.ToString());
                        auecDetail.ExchangeIdentifier = txtExchangeIdentifier.Text;
                        auecDetail.MarketDataProviderExchangeIdentifier = txtMarketDataProviderExchangeIdentifier.Text;
                        auecDetail.CurrencyID = int.Parse(cmbCurrency.Value.ToString());

                        auecDetail.Unit = int.Parse(cmbUnits.Value.ToString());

                        Double timeZoneHours = ((Infragistics.Win.TimeZoneInfo)(((Infragistics.Win.UltraWinEditors.UltraComboEditor)(cmbTimeZone)).Value)).UtcOffset.TotalHours;
                        auecDetail.TimeZone = cmbTimeZone.Text;
                        auecDetail.TimeZoneOffSet = timeZoneHours;

                        //Following varraibles initalized.
                        DateTime oldDateTime = new DateTime();
                        DateTime upDatedDateTime = new DateTime();


                        //If this checkbox is checked then only the PreMarketTradingStartTime's value is stored
                        //else for the time being some temporary value equivalent to null is stored in the database, 
                        //which in our case is year 2000.
                        if (chkPreMarket.Checked == true)
                        {
                            oldDateTime = DateTime.Parse(txtPreMarketTradingStartTime.Time);
                            upDatedDateTime = DateTime.Parse(txtPreMarketTradingStartTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.PreMarketTradingStartTime = upDatedDateTime;

                            oldDateTime = DateTime.Parse(txtPreMarketTradingEndTime.Time);
                            upDatedDateTime = DateTime.Parse(txtPreMarketTradingEndTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.PreMarketTradingEndTime = upDatedDateTime;

                            //exchange.PreMarketTradingStartTime = DateTime.Parse(txtPreMarketTradingStartTime.Time);
                            //exchange.PreMarketTradingEndTime = DateTime.Parse(txtPreMarketTradingEndTime.Time);
                        }
                        else
                        {
                            auecDetail.PreMarketTradingStartTime = DateTime.Parse(NULLYEAR);
                            auecDetail.PreMarketTradingEndTime = DateTime.Parse(NULLYEAR);
                        }

                        if (chkLunchTime.Checked == true)
                        {
                            oldDateTime = DateTime.Parse(txtLunchTimeStartTime.Time);
                            upDatedDateTime = DateTime.Parse(txtLunchTimeStartTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.LunchTimeStartTime = upDatedDateTime;

                            oldDateTime = DateTime.Parse(txtLunchTimeEndTime.Time);
                            upDatedDateTime = DateTime.Parse(txtLunchTimeEndTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.LunchTimeEndTime = upDatedDateTime;

                            //exchange.LunchTimeStartTime = DateTime.Parse(txtLunchTimeStartTime.Time);
                            //exchange.LunchTimeEndTime = DateTime.Parse(txtLunchTimeEndTime.Time);
                        }
                        else
                        {
                            auecDetail.LunchTimeStartTime = DateTime.Parse(NULLYEAR);
                            auecDetail.LunchTimeEndTime = DateTime.Parse(NULLYEAR);
                        }

                        if (chkRegularMarket.Checked == true)
                        {
                            oldDateTime = DateTime.Parse(txtRegularTradingStartTime.Time);
                            upDatedDateTime = DateTime.Parse(txtRegularTradingStartTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.RegularTradingStartTime = upDatedDateTime;

                            oldDateTime = DateTime.Parse(txtRegularTradingEndTime.Time);
                            upDatedDateTime = DateTime.Parse(txtRegularTradingEndTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.RegularTradingEndTime = upDatedDateTime;

                            //exchange.RegularTradingStartTime = DateTime.Parse(txtRegularTradingStartTime.Time);
                            //exchange.RegularTradingEndTime = DateTime.Parse(txtRegularTradingEndTime.Time);
                        }
                        else
                        {
                            auecDetail.RegularTradingStartTime = DateTime.Parse(NULLYEAR);
                            auecDetail.RegularTradingEndTime = DateTime.Parse(NULLYEAR);
                        }

                        //If this checkbox is checked then only the chkPostMarketStartTime's value is stored
                        //else for the time being some temporary value equivalent to null is stored in the database, 
                        //which in our case is year 2000.
                        if (chkPostMarket.Checked == true)
                        {
                            oldDateTime = DateTime.Parse(txtPostMarketTradingStartTime.Time);
                            upDatedDateTime = DateTime.Parse(txtPostMarketTradingStartTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.PostMarketTradingStartTime = upDatedDateTime;

                            oldDateTime = DateTime.Parse(txtPostMarketTradingEndTime.Time);
                            upDatedDateTime = DateTime.Parse(txtPostMarketTradingEndTime.Time);
                            upDatedDateTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(upDatedDateTime, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetail.TimeZone));

                            auecDetail.PostMarketTradingEndTime = upDatedDateTime;

                            //exchange.PostMarketTradingStartTime = DateTime.Parse(txtPostMarketTradingStartTime.Time);
                            //exchange.PostMarketTradingEndTime = DateTime.Parse(txtPostMarketTradingEndTime.Time);
                        }
                        else
                        {
                            auecDetail.PostMarketTradingStartTime = DateTime.Parse(NULLYEAR);
                            auecDetail.PostMarketTradingEndTime = DateTime.Parse(NULLYEAR);
                        }


                        auecDetail.PreMarketCheck = (chkPreMarket.Checked == true ? 1 : 0);
                        auecDetail.PostMarketCheck = (chkPostMarket.Checked == true ? 1 : 0);

                        auecDetail.RegularTimeCheck = (chkRegularMarket.Checked == true ? 1 : 0);
                        auecDetail.LunchTimeCheck = (chkLunchTime.Checked == true ? 1 : 0);

                        auecDetail.SettlementDaysBuy = int.Parse(txtSettlementDays.Text.Trim());
                        auecDetail.SettlementDaysSell = int.Parse(txtSettlementDaysSell.Text.Trim());
                        auecDetail.DayLightSaving = txtDayLightSaving.Value.ToShortDateString() + " " + txtDayLightSavingTime.Value.ToShortTimeString();
                        auecDetail.Country = int.Parse(cmbCountry.Value.ToString());
                        auecDetail.StateID = int.Parse(cmbState.Value.ToString());

                        int exchangeID = int.MinValue;
                        if (newAUEC != null && aUECExchangeID == int.MinValue)
                        {
                            exchangeID = int.Parse(newAUEC.ExchangeID.ToString());
                        }
                        if (exchangeID > 0)
                        {
                            auecDetail.ExchangeID = int.Parse(newAUEC.ExchangeID.ToString());
                        }
                        int flagID = int.Parse(cmbFlag.Value.ToString());

                        #region FlagSave
                        int countryFlagID = int.MinValue;
                        byte[] flagImage = null;
                        Flag flag = new Flag();
                        flag.CountryFlagID = int.Parse(cmbFlag.Value.ToString());
                        countryFlagID = int.Parse(cmbFlag.Value.ToString());
                        flag.CountryFlagName = cmbFlag.Text.ToString();
                        if (cmbFlag.Text.ToString() != "" && countryFlagID == int.MinValue)
                        {
                            flag.CountryFlagImage = (byte[])cmbFlag.ActiveRow.Cells["CountryFlagImage"].Value;
                        }
                        else
                        {
                            flag.CountryFlagImage = null;
                        }
                        if (flag.CountryFlagImage != flagImage)
                        {
                            countryFlagID = GeneralManager.SaveCountryFlag(flag);
                        }
                        #endregion
                        if (countryFlagID > int.MinValue)
                        {
                            auecDetail.CountryFlagID = countryFlagID;
                        }
                        else
                        {
                            auecDetail.CountryFlagID = int.Parse(cmbFlag.Value.ToString());
                        }

                        #region LogoSave
                        int logoID = int.MinValue;
                        byte[] logoImage = null;
                        Logo logo = new Logo();
                        logo.LogoID = int.Parse(cmbExchangeLogo.Value.ToString());
                        logoID = int.Parse(cmbExchangeLogo.Value.ToString());
                        logo.LogoName = cmbExchangeLogo.Text.ToString();
                        if (cmbExchangeLogo.Text.ToString() != "" && logoID == int.MinValue)
                        {
                            logo.LogoImage = (byte[])cmbExchangeLogo.ActiveRow.Cells["LogoImage"].Value;
                        }
                        else
                        {
                            logo.LogoImage = null;
                        }
                        if (logo.LogoImage != logoImage)
                        {
                            logoID = GeneralManager.SaveLogo(logo);
                        }
                        #endregion
                        if (logoID > int.MinValue)
                        {
                            auecDetail.LogoID = logoID;
                        }
                        else
                        {
                            auecDetail.LogoID = int.Parse(cmbExchangeLogo.Value.ToString());
                        }
                        auecDetail.CurrencyConversion = int.Parse(cmbCurrencyConversion.Value.ToString());
                        int isID = nodeDetails.NodeID;

                        _auecID = AUECManager.SaveAUECDetails(auecDetail);

                        if (_auecID == -1)
                        {
                            MessageBox.Show("AUEC with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            result = _auecID;
                            return result;
                        }
                        else if (_auecID == -2)
                        {
                            MessageBox.Show("AUEC with the same exchange identifier already exists.", "Prana Alert", MessageBoxButtons.OK);
                            result = _auecID;
                            return result;
                        }
                        else
                        {
                            //modified by omshiv, Saving on CSM in case of CH release  
                            int isSavedOnCSM = AUECManager.SaveSMAuecDetails(auecDetail, _auecID);
                            if (isSavedOnCSM == -1)
                            {
                                MessageBox.Show("Central Security Master connection string is not available. Please contact to admin.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            AUECManager.SaveAUECDetailsForVenue(auecDetail);
                            AUECManager.SaveAUECCounterPartyVenues(_auecID);
                        }
                        break;
                }
            }
            result = _auecID;
            return result;
        }

        /// <summary>
        /// This method saves the Market Fees for the selected auec while applying the validations on the
        /// textboxes controls.
        /// </summary>
        /// <returns>true: if successful, false: if fails to save.</returns>
        private bool SaveMaketFees()
        {
            bool result = true;
            //System.Text.RegularExpressions.Regex rgNumber = new System.Text.RegularExpressions.Regex(@"^\d+$"); 
            System.Text.RegularExpressions.Regex rgNumber = new System.Text.RegularExpressions.Regex(@"(^-?\d{1,6}\.$)|(^-?\d{1,6}$)|(^-?\d{1,6}\.\d{0,2}$)");

            MarketFee marketFee = new MarketFee();
            errorProvider1.SetError(txtPurchaseSecFees, "");
            errorProvider1.SetError(txtSalesSecFees, "");
            errorProvider1.SetError(txtPurchaseStamp, "");
            errorProvider1.SetError(txtSaleStamp, "");
            errorProvider1.SetError(txtPurchaseLevy, "");
            errorProvider1.SetError(txtSaleLevy, "");
            if (trvAsset.SelectedNode == null)
            {
                //				Common.ResetStatusPanel(stbAUEC);
                //				Common.SetStatusPanel(stbAUEC, "Please select Exchange!");
            }
            else
            {
                //int auecID = (int)trvAsset.SelectedNode.Tag;
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                tbcAUEC.Enabled = true;
                DisableTabsForAssetNode(nodeDetails.Type);
                switch (nodeDetails.Type)
                {

                    case NodeType.Asset:
                        tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                        break;

                    case NodeType.Underlying:
                        tbcAUEC.Enabled = false;
                        break;
                    case NodeType.Exchange:

                        if (int.Parse(txtPurchaseSecFees.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtPurchaseSecFees.Text.Trim()))
                            {
                                errorProvider1.SetError(txtPurchaseSecFees, "Please Give Purchase Sec Fees a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtPurchaseSecFees.Focus();
                                return result;
                            }
                            marketFee.PurchaseSecFees = Double.Parse(txtPurchaseSecFees.Text.ToString());
                        }
                        else
                        {
                            marketFee.PurchaseSecFees = 0;
                        }

                        if (int.Parse(txtSalesSecFees.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtSalesSecFees.Text.Trim()))
                            {
                                errorProvider1.SetError(txtSalesSecFees, "Please Give Sales Sec Fees a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtSalesSecFees.Focus();
                                return result;
                            }
                            marketFee.SaleSecFees = Double.Parse(txtSalesSecFees.Text.ToString());
                        }
                        else
                        {
                            marketFee.SaleSecFees = 0;
                        }


                        if (int.Parse(txtPurchaseStamp.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtPurchaseStamp.Text.Trim()))
                            {
                                errorProvider1.SetError(txtPurchaseStamp, "Please Give Purchase Stamp a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtPurchaseStamp.Focus();
                                return result;
                            }
                            marketFee.PurchaseStamp = Double.Parse(txtPurchaseStamp.Text.ToString());
                        }
                        else
                        {
                            marketFee.PurchaseStamp = 0;
                        }

                        if (int.Parse(txtSaleStamp.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtSaleStamp.Text.Trim()))
                            {
                                errorProvider1.SetError(txtSaleStamp, "Please Give Sales Stamp a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtSaleStamp.Focus();
                                return result;
                            }
                            marketFee.SaleStamp = Double.Parse(txtSaleStamp.Text.ToString());
                        }
                        else
                        {
                            marketFee.SaleStamp = 0;
                        }

                        if (int.Parse(txtPurchaseLevy.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtPurchaseLevy.Text.Trim()))
                            {
                                errorProvider1.SetError(txtPurchaseLevy, "Please Give Purchase Levy a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtPurchaseLevy.Focus();
                                return result;
                            }
                            marketFee.PurchaseLevy = Double.Parse(txtPurchaseLevy.Text.ToString());
                        }
                        else
                        {
                            marketFee.PurchaseLevy = 0;
                        }

                        if (int.Parse(txtSaleLevy.Text.Trim().Length.ToString()) > 0)
                        {
                            if (!rgNumber.IsMatch(txtSaleLevy.Text.Trim()))
                            {
                                errorProvider1.SetError(txtSaleLevy, "Please Give Sale Levy a legal value!");
                                tbcAUEC.SelectedTab = tbcAUEC.Tabs[TABMARKETFEES];
                                txtSaleLevy.Focus();
                                return result;
                            }

                            marketFee.SaleLevy = Double.Parse(txtSaleLevy.Text.ToString());
                        }
                        else
                        {
                            marketFee.SaleLevy = 0;
                        }
                        break;
                }
            }
            return result;
        }

        private bool SaveHolidays()
        {
            //ToDo: Save Holidays
            return false;
        }

        private bool SaveMasterSymbolList()
        {
            //ToDo: Save Symbol list
            return false;
        }

        private Prana.Admin.BLL.AUEC SaveCompliance(Prana.Admin.BLL.AUEC auecDetail)
        {
            auecDetail.CurrencyID = int.Parse(cmbCurrency.Value.ToString());
            auecDetail.OtherCurrencyID = int.Parse(cmbOtherCurrency.Value.ToString());
            //	compliance.Multiplier = int.Parse(spnMultiplier.Value.ToString());
            auecDetail.Multiplier = double.Parse(txtMultiplier.Text.ToString());
            auecDetail.RoundLot = Convert.ToDecimal(txtRoundLot.Text.ToString());
            auecDetail.IsShortSaleConfirmation = (int.Parse(cmbShortSaleConfirmation.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ShortSaleConfirmation : (int)Prana.Admin.BLL.Options.No);
            auecDetail.ProvideAccountNameWithTrade = (int.Parse(cmbProvideAccountnamewithTrade.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ProvidentAccountNameWithTrade : (int)Prana.Admin.BLL.Options.No);
            auecDetail.ProvideIdentifierNameWithTrade = (int.Parse(cmbProvideidentifierwithtrade.Value.ToString()) == (int)Prana.Admin.BLL.Options.Yes ? (int)Prana.Admin.BLL.ComplianceOptions.ProvidentIdentifierWithTrade : (int)Prana.Admin.BLL.Options.No);
            auecDetail.IdentifierID = int.Parse(cmbIdentifierName.Value.ToString()); //cmbIdentifierName.SelectedValue.ToString();					
            auecDetail.UnitID = int.Parse(cmbUnits.Value.ToString());

            //This method saves the auec related compliance to the database.
            //result = AUECManager.SaveAUECCompliance(auecID, compliance);
            return auecDetail;
        }

        private void tbcAUEC_Click(object sender, System.EventArgs e)
        {
            try
            {
                BindHoliday();
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("tbcAUEC_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "tbcAUEC_Click", null);


                #endregion
            }
        }

        private void btnAddHoliday_Click(object sender, System.EventArgs e)
        {

            try
            {
                //This check is done to ensure that the holiday is assigned to some already added node. 
                if (trvAsset.SelectedNode == null)
                {
                    //					Common.ResetStatusPanel(stbAUEC);
                    //					Common.SetStatusPanel(stbAUEC, "Please select Exchange!");
                }
                else
                {
                    //int auecID = (int)trvAsset.SelectedNode.Tag;
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    tbcAUEC.Enabled = true;
                    DisableTabsForAssetNode(nodeDetails.Type);
                    switch (nodeDetails.Type)
                    {

                        case NodeType.Asset:
                            tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                            break;
                        case NodeType.Underlying:
                            tbcAUEC.Enabled = false;
                            break;
                        case NodeType.Exchange:
                            int auecID = nodeDetails.NodeID;
                            Holidays holidays = ((Holidays)grdHoliday.DataSource);
                            // Check if Description for the new Holiday is given
                            if (txtHolidayDescription.Text.Trim() == "")
                            {
                                MessageBox.Show("Please Enter Description", "Prana Alert");

                            }
                            else
                            {
                                //Check if Date already exists in the Holiday List
                                bool flag = false;
                                //int masterRevertCheck = int.MinValue;
                                foreach (Holiday holiday in holidays)
                                {
                                    if (holiday.Date.ToShortDateString() == dttHolidayDate.Value.ToShortDateString())
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag == false)
                                {
                                    Holiday holiday1 = new Holiday(int.MinValue, auecID, txtHolidayDescription.Text, dttHolidayDate.Value);
                                    holidays.Add(holiday1);
                                    if (auecID != int.MinValue)
                                    {
                                        AUECManager.SaveAUECHolidays(holiday1, auecID);
                                        BindHoliday();
                                    }
                                    else
                                    {
                                        this.grdHoliday.DataSource = holidays;
                                        grdHoliday.DataBind();
                                    }
                                    MessageBox.Show("Holiday added.", "Prana Alert", MessageBoxButtons.OK);
                                    txtHolidayDescription.Text = "";
                                    dttHolidayDate.Value = DateTime.Now;
                                }
                                else
                                {
                                    BindHoliday();
                                    MessageBox.Show("Date " + dttHolidayDate.Value.ToShortDateString() + " already exists in the List!");
                                }
                            }
                            break;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnAddHoliday_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnAddHoliday_Click", null);


                #endregion
            }
        }


        /// <summary>
        /// This method fills corresponding tabs according to the auec seelected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvAsset_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                tbcAUEC.Enabled = true;
                DisableTabsForAssetNode(nodeDetails.Type);
                switch (nodeDetails.Type)
                {

                    case NodeType.Asset:
                        tbcAUEC.Tabs["AssetWisePermissions"].Selected = true;
                        int assetID = nodeDetails.NodeID;
                        if (assetID > 0)
                        {
                            Prana.Admin.BLL.Sides sides = AUECManager.GetAssetSides(assetID);

                            if (sides.Count > 0)
                            {
                                for (int j = 0; j < checkedlstSide.Items.Count; j++)
                                {
                                    checkedlstSide.SetItemChecked(j, false);
                                }
                                foreach (Prana.Admin.BLL.Side side in sides)
                                {
                                    for (int j = 0; j < checkedlstSide.Items.Count; j++)
                                    {
                                        if (((Prana.Admin.BLL.Side)checkedlstSide.Items[j]).SideID == int.Parse(side.SideID.ToString()))
                                        {
                                            checkedlstSide.SetItemChecked(j, true);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case NodeType.Underlying:
                        tbcAUEC.Enabled = false;
                        break;
                    case NodeType.Exchange:
                        int auecID = nodeDetails.NodeID;
                        Prana.Admin.BLL.Exchange masterExchange = new Prana.Admin.BLL.Exchange();
                        Prana.Admin.BLL.AUEC auec = new Prana.Admin.BLL.AUEC();
                        if (auecID > 0)
                        {
                            auec = AUECManager.GetAUECDetails(auecID);
                            SetupExchangeTab(auec);
                            InitializeAUECAudit(auec.AUECID);
                        }
                        else
                        {
                            int exchangeID = int.Parse(newAUEC.ExchangeID.ToString());
                            auec.ExchangeID = exchangeID;

                            auec.AssetID = nodeDetails.AssetID;
                            auec.UnderlyingID = nodeDetails.UnderLyingID;
                            SetupExchangeTab(auec);
                        }
                        if (auec != null)
                        {

                            lblAUECCombination.Text = auec.AUECString;

                            ResetComboes();
                            if (CmbYear.SelectedValue != null)
                            {
                                string name = AUECManager.GetCalendar(auecID, int.Parse(CmbYear.SelectedValue.ToString()));
                                if (name == string.Empty)
                                {
                                    cmbCalendars.SelectedRow = cmbCalendars.Rows[0];
                                }
                                else
                                {
                                    try
                                    {
                                        foreach (UltraGridRow row in cmbCalendars.Rows)
                                        {
                                            if (row.Cells["CalendarName"].Text == name)
                                            {
                                                cmbCalendars.ActiveRow = (UltraGridRow)row;
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                        if (rethrow)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                        }

                        if (auecID > 0)
                        {
                            List<OtherFeeRule> otherFeeRuleList = AUECManager.GetAUECOtherFeeRules(auecID);
                            SetupMarketFeesTab(auec);
                            SetUpOtherFeeRulesTab(auecID, otherFeeRuleList);
                            SetupComplianceTab(auec);
                            SetupAUECWeeklyHolidays(auecID);
                        }
                        else
                        {
                            SetupDefaultWeeklyHolidays();
                            ResetOtherFeeTabs(auecID);
                        }

                        BindHoliday();

                        this.dttHolidayDate.CustomFormat = "\'\'";
                        this.dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                        this.dttHolidayDate.Value = DateTime.Now;
                        break;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("trvAsset_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvAsset_AfterSelect", null);


                #endregion
            }
        }

        private void DisableTabsForAssetNode(NodeType type)
        {
            foreach (var tab in tbcAUEC.Tabs)
            {
                if (tab.Key.Equals("AssetWisePermissions"))
                {
                    tab.Enabled = type == NodeType.Asset;
                }
                else
                {
                    tab.Enabled = type != NodeType.Asset;
                }
            }
        }

        private void SetUpOtherFeeRulesTab(int auecID, List<OtherFeeRule> otherFeeRuleList)
        {
            int index = ResetOtherFeeTabs(auecID);


            index = 0;
            foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabCtrlOtherFee.Tabs)
            {
                foreach (OtherFeeRule otherFeeRule in otherFeeRuleList)
                {
                    if (tab.Key.Equals(TABKEY_INITIAL + otherFeeRule.OtherFeeType.ToString()))
                    {
                        string usrCtrlName = USERCONTROLNAME_INITIAL + otherFeeRule.OtherFeeType;
                        string tbcName = TABCONTROLNAME_INITIAL + otherFeeRule.OtherFeeType;

                        Prana.Admin.Controls.OtherFee otherFeeControl = (Prana.Admin.Controls.OtherFee)tabCtrlOtherFee.Tabs[index].TabPage.Controls[usrCtrlName];
                        otherFeeControl.BindControlWithAUECID(auecID, otherFeeRule);
                    }
                }
                index++;
            }
        }

        private int ResetOtherFeeTabs(int auecID)
        {
            EnumerationValueList listForOtherFee = Prana.Utilities.CommissionEnumHelper.GetOtherFeeList();
            int index = 0;
            foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabCtrlOtherFee.Tabs)
            {
                OtherFeeRule otherFeeRule = new OtherFeeRule();
                Prana.BusinessObjects.EnumerationValue otherFeeValue = (Prana.BusinessObjects.EnumerationValue)listForOtherFee[index];

                string usrCtrlName = USERCONTROLNAME_INITIAL + otherFeeValue.DisplayText;
                string tbcName = TABCONTROLNAME_INITIAL + otherFeeValue.DisplayText;

                Prana.Admin.Controls.OtherFee otherFeeControl = (Prana.Admin.Controls.OtherFee)tabCtrlOtherFee.Tabs[index].TabPage.Controls[usrCtrlName];
                otherFeeControl.ResetControl(auecID);
                index++;
            }
            return index;
        }

        /// <summary>
        /// Set up the default weekly holidays in the list box i.e. saturday and sunday.
        /// </summary>
        private void SetupDefaultWeeklyHolidays()
        {
            for (int j = 0; j < checkedlstWeeklyHolidays.Items.Count; j++)
            {
                checkedlstWeeklyHolidays.SetItemChecked(j, false);
            }
            for (int j = 0; j < checkedlstWeeklyHolidays.Items.Count; j++)
            {
                if (((Prana.BusinessObjects.WeeklyHoliday)checkedlstWeeklyHolidays.Items[j]).WeeklyHolidayName == DayOfWeek.Saturday.ToString() || ((Prana.BusinessObjects.WeeklyHoliday)checkedlstWeeklyHolidays.Items[j]).WeeklyHolidayName == DayOfWeek.Sunday.ToString())
                {
                    checkedlstWeeklyHolidays.SetItemChecked(j, true);
                }
            }
        }

        /// <summary>
        /// Setup the Weekly holidays list box with the holidays saved as per the AUEC.
        /// </summary>
        private void SetupAUECWeeklyHolidays(int auecID)
        {
            for (int j = 0; j < checkedlstWeeklyHolidays.Items.Count; j++)
            {
                checkedlstWeeklyHolidays.SetItemChecked(j, false);
            }

            List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection = AUECManager.GetAUECWeeklyHolidaysCollection(auecID);
            if (weeklyHolidaysCollection.Count > 0)
            {
                foreach (Prana.BusinessObjects.WeeklyHoliday weeklyHolidays in weeklyHolidaysCollection)
                {
                    for (int j = 0; j < checkedlstWeeklyHolidays.Items.Count; j++)
                    {
                        if (((Prana.BusinessObjects.WeeklyHoliday)checkedlstWeeklyHolidays.Items[j]).WeeklyHolidayID == int.Parse(weeklyHolidays.WeeklyHolidayID.ToString()))
                        {
                            checkedlstWeeklyHolidays.SetItemChecked(j, true);
                        }
                    }
                }
            }
        }

        private void ResetComboes()
        {
            ColumnsCollection columns = cmbSymbolConvention.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "SymbolConventionName")
                {
                    column.Hidden = true;
                }
            }
            cmbSymbolConvention.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsUnits = cmbUnits.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsUnits)
            {
                if (column.Key != "UnitName")
                {
                    column.Hidden = true;
                }
            }
            cmbUnits.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsCountry = cmbCountry.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCountry)
            {
                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
            }
            cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsState = cmbState.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsState)
            {
                if (column.Key != "StateName")
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsFlag = cmbFlag.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsFlag)
            {
                if (column.Key != "CountryFlagImage")
                {
                    if (column.Key != "CountryFlagName")
                    {
                        column.Hidden = true;
                    }
                }
            }
            cmbFlag.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsLogo = cmbExchangeLogo.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsLogo)
            {
                if (column.Key != "LogoImage")
                {
                    if (column.Key != "LogoName")
                    {
                        column.Hidden = true;
                    }
                }
            }
            cmbExchangeLogo.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsBaseCurrecncies = cmbCurrency.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsBaseCurrecncies)
            {
                if (column.Key != "CurrencySymbol")
                {
                    column.Hidden = true;
                }
            }
            cmbCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsOtherCurrecncies = cmbOtherCurrency.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsOtherCurrecncies)
            {
                if (column.Key != "CurrencySymbol")
                {
                    column.Hidden = true;
                }
            }
            cmbOtherCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsSSC = cmbShortSaleConfirmation.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsSSC)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }
            cmbShortSaleConfirmation.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsFNWT = cmbProvideAccountnamewithTrade.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsFNWT)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }
            cmbProvideAccountnamewithTrade.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsIWT = cmbProvideidentifierwithtrade.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsIWT)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }
            cmbProvideidentifierwithtrade.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsIdentifier = cmbIdentifierName.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsIdentifier)
            {
                if (column.Key != "IdentifierName")
                {
                    column.Hidden = true;
                }
            }
            cmbIdentifierName.DisplayLayout.Bands[0].ColHeadersVisible = false;

            ColumnsCollection columnsCurrencyConversion = cmbCurrencyConversion.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCurrencyConversion)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }
            cmbCurrencyConversion.DisplayLayout.Bands[0].ColHeadersVisible = false;

            //CmbYear.SelectedItem = (object)DateTime.Now.Year;

        }

        /// <summary>
        /// This method sets up the <see cref="Exchange"/>tab after selecting a particular auec.
        /// </summary>
        /// <param name="auec"></param>
        private void SetupExchangeTab(Prana.Admin.BLL.AUEC auecDetails)
        {
            //Exchange exchange = auec.Exchange;
            //			int auecExchangeID = int.MinValue;
            //			auecExchangeID = auec.AUECID; //This is actually the AUECExchangeID.
            int auecID = auecDetails.AUECID;

            if (auecID > 0)
            {

                chkPreMarket.Checked = (int.Parse(auecDetails.PreMarketCheck.ToString()) == 1 ? true : false);
                chkPostMarket.Checked = (int.Parse(auecDetails.PostMarketCheck.ToString()) == 1 ? true : false);
                chkRegularMarket.Checked = (int.Parse(auecDetails.RegularTimeCheck.ToString()) == 1 ? true : false);
                chkLunchTime.Checked = (int.Parse(auecDetails.LunchTimeCheck.ToString()) == 1 ? true : false);

                txtExchangeNameFull.Text = auecDetails.FullName;
                txtShortName.Text = auecDetails.DisplayName;
                cmbSymbolConvention.Value = auecDetails.SymbolConventionID;
                txtExchangeIdentifier.Text = auecDetails.ExchangeIdentifier;
                txtMarketDataProviderExchangeIdentifier.Text = auecDetails.MarketDataProviderExchangeIdentifier;

                //TODO: shift the following two lines to setcompliance functions.
                //cmbCurrency.Value = auecDetails.Currency;
                cmbCurrency.Value = auecDetails.CurrencyID;
                cmbUnits.Value = auecDetails.Unit;

                cmbTimeZone.Text = auecDetails.TimeZone;
                //cmbTimeZone.SelectedIndex = int.Parse(auecExchange.TimeZone.ToString()); 
                DateTime dateTimeStart = new DateTime();
                DateTime dateTimeEnd = new DateTime();
                DateTime dateTimeStartFinal = new DateTime();
                DateTime dateTimeEndFinal = new DateTime();
                double timeZoneOffSet = auecDetails.TimeZoneOffSet;


                if (chkPreMarket.Checked == true)
                {
                    dateTimeStart = auecDetails.PreMarketTradingStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtPreMarketTradingStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.PreMarketTradingEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtPreMarketTradingEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;

                    //txtPreMarketTradingStartTime.Time = auecExchange.PreMarketTradingStartTime.Hour + ":" + auecExchange.PreMarketTradingStartTime.Minute;
                    //txtPreMarketTradingEndTime.Time = auecExchange.PreMarketTradingEndTime.Hour + ":" + auecExchange.PreMarketTradingEndTime.Minute;
                }
                else
                {
                    txtPreMarketTradingStartTime.Time = auecDetails.PreMarketTradingStartTime.Hour + ":" + auecDetails.PreMarketTradingStartTime.Minute;
                    //txtPreMarketTradingEndTime.Time = auecDetails.PreMarketTradingEndTime.Hour + ":" + auecDetails.PreMarketTradingEndTime.Minute;
                    txtPreMarketTradingEndTime.Time = 0 + ":" + 0;
                    txtPreMarketTradingStartTime.Enabled = false;
                    txtPreMarketTradingEndTime.Enabled = false;
                }

                if (chkLunchTime.Checked == true)
                {
                    dateTimeStart = auecDetails.LunchTimeStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtLunchTimeStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.LunchTimeEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtLunchTimeEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;

                    //txtLunchTimeStartTime.Time = auecExchange.LunchTimeStartTime.Hour + ":" + auecExchange.LunchTimeStartTime.Minute;
                    //txtLunchTimeEndTime.Time = auecExchange.LunchTimeEndTime.Hour + ":" + auecExchange.LunchTimeEndTime.Minute;			
                }
                else
                {
                    txtLunchTimeStartTime.Time = auecDetails.LunchTimeStartTime.Hour + ":" + auecDetails.LunchTimeStartTime.Minute;
                    txtLunchTimeEndTime.Time = auecDetails.LunchTimeEndTime.Hour + ":" + auecDetails.LunchTimeEndTime.Minute;
                    txtLunchTimeStartTime.Enabled = false;
                    txtLunchTimeEndTime.Enabled = false;
                }

                if (chkRegularMarket.Checked == true)
                {
                    dateTimeStart = auecDetails.RegularTradingStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtRegularTradingStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.RegularTradingEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtRegularTradingEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;

                    //txtRegularTradingStartTime.Time = auecExchange.RegularTradingStartTime.Hour + ":" + auecExchange.RegularTradingStartTime.Minute;
                    //txtRegularTradingEndTime.Time = auecExchange.RegularTradingEndTime.Hour + ":" + auecExchange.RegularTradingEndTime.Minute;
                }
                else
                {
                    txtRegularTradingStartTime.Time = auecDetails.RegularTradingStartTime.Hour + ":" + auecDetails.RegularTradingStartTime.Minute;
                    txtRegularTradingEndTime.Time = auecDetails.RegularTradingEndTime.Hour + ":" + auecDetails.RegularTradingEndTime.Minute;
                    txtRegularTradingStartTime.Enabled = false;
                    txtRegularTradingEndTime.Enabled = false;
                }
                if (chkPostMarket.Checked == true)
                {
                    dateTimeStart = auecDetails.PostMarketTradingStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone)); ;
                    txtPostMarketTradingStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.PostMarketTradingEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.TimeZone));
                    txtPostMarketTradingEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;

                    //txtPostMarketTradingStartTime.Time = auecExchange.PostMarketTradingStartTime.Hour + ":" + auecExchange.PostMarketTradingStartTime.Minute;
                    //txtPostMarketTradingEndTime.Time = auecExchange.PostMarketTradingEndTime.Hour + ":" + auecExchange.PostMarketTradingEndTime.Minute;
                }
                else
                {
                    txtPostMarketTradingStartTime.Time = auecDetails.PostMarketTradingStartTime.Hour + ":" + auecDetails.PostMarketTradingStartTime.Minute;
                    txtPostMarketTradingEndTime.Time = auecDetails.PostMarketTradingEndTime.Hour + ":" + auecDetails.PostMarketTradingEndTime.Minute;
                    txtPostMarketTradingStartTime.Enabled = false;
                    txtPostMarketTradingEndTime.Enabled = false;
                }

                if (int.Parse(auecDetails.SettlementDaysBuy.ToString()) < 0)
                {
                    txtSettlementDays.Text = "";
                }
                else
                {
                    txtSettlementDays.Text = auecDetails.SettlementDaysBuy.ToString();
                }
                if (int.Parse(auecDetails.SettlementDaysSell.ToString()) < 0)
                {
                    txtSettlementDaysSell.Text = "";
                }
                else
                {
                    txtSettlementDaysSell.Text = auecDetails.SettlementDaysSell.ToString();
                }
                cmbCountry.Value = auecDetails.Country;
                cmbState.Value = auecDetails.StateID;
                cmbCurrencyConversion.Value = auecDetails.CurrencyConversion;
                cmbFlag.Value = auecDetails.CountryFlagID;
                cmbExchangeLogo.Value = auecDetails.LogoID;

                if (auecDetails.DayLightSaving != "")
                {
                    txtDayLightSaving.Value = DateTime.Parse(auecDetails.DayLightSaving.ToString());
                    txtDayLightSavingTime.Value = DateTime.Parse(auecDetails.DayLightSaving.ToString());
                }
                else
                {
                    txtDayLightSaving.Value = DateTime.Now;
                    txtDayLightSavingTime.Value = DateTime.Now;
                }
                cmbSymbolConvention.Value = int.Parse(auecDetails.SymbolConventionID.ToString());
            }
            else
            {
                DateTime dateTimeStart = new DateTime();
                DateTime dateTimeEnd = new DateTime();
                DateTime dateTimeStartFinal = new DateTime();
                DateTime dateTimeEndFinal = new DateTime();
                double timeZoneOffSet = auecDetails.Exchange.TimeZoneOffSet;


                txtExchangeNameFull.Text = auecDetails.Exchange.Name;
                txtShortName.Text = auecDetails.Exchange.DisplayName;
                txtExchangeIdentifier.Text = auecDetails.Exchange.ExchangeIdentifier;
                txtMarketDataProviderExchangeIdentifier.Text = auecDetails.MarketDataProviderExchangeIdentifier;
                cmbTimeZone.Text = auecDetails.Exchange.TimeZone;

                chkRegularMarket.Checked = (int.Parse(auecDetails.Exchange.RegularTimeCheck.ToString()) == 1 ? true : false);
                chkLunchTime.Checked = (int.Parse(auecDetails.Exchange.LunchTimeCheck.ToString()) == 1 ? true : false);
                Prana.BusinessObjects.TimeZone auecTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(auecDetails.Exchange.TimeZone);
                if (auecTimeZone == null)
                {
                    auecTimeZone = Prana.BusinessObjects.TimeZoneInfo.EasternTimeZone;
                    cmbTimeZone.Text = auecTimeZone.DisplayName;
                    MessageBox.Show(this, "Using default time zone because exchange time-zone is not valid.", "Prana Alert", MessageBoxButtons.OK);
                }

                if (chkRegularMarket.Checked == true)
                {
                    dateTimeStart = auecDetails.Exchange.RegularTradingStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, auecTimeZone); ;
                    txtRegularTradingStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.Exchange.RegularTradingEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, auecTimeZone);
                    txtRegularTradingEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;
                }
                else
                {
                    txtRegularTradingStartTime.Time = 0 + ":" + 0;
                    txtRegularTradingEndTime.Time = 0 + ":" + 0;
                }
                if (chkLunchTime.Checked == true)
                {
                    dateTimeStart = auecDetails.Exchange.LunchTimeStartTime;
                    dateTimeStartFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeStart, auecTimeZone);
                    txtLunchTimeStartTime.Time = dateTimeStartFinal.Hour + ":" + dateTimeStartFinal.Minute;

                    dateTimeEnd = auecDetails.Exchange.LunchTimeEndTime;
                    dateTimeEndFinal = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTimeEnd, auecTimeZone);
                    txtLunchTimeEndTime.Time = dateTimeEndFinal.Hour + ":" + dateTimeEndFinal.Minute;
                }
                else
                {
                    txtLunchTimeStartTime.Time = 0 + ":" + 0;
                    txtLunchTimeEndTime.Time = 0 + ":" + 0;
                }


                cmbCountry.Value = auecDetails.Exchange.Country;
                cmbState.Value = auecDetails.Exchange.StateID;
                cmbFlag.Value = auecDetails.Exchange.CountryFlagID;
                cmbExchangeLogo.Value = auecDetails.Exchange.LogoID;

                cmbSymbolConvention.Text = C_COMBO_SELECT;
                cmbUnits.Text = C_COMBO_SELECT;
                chkPreMarket.Checked = false;
                chkPostMarket.Checked = false;
                txtPreMarketTradingStartTime.Time = 0 + ":" + 0;
                txtPreMarketTradingEndTime.Time = 0 + ":" + 0;
                txtPostMarketTradingStartTime.Time = 0 + ":" + 0;
                txtPostMarketTradingEndTime.Time = 0 + ":" + 0;
                txtSettlementDays.Text = "";
                txtSettlementDaysSell.Text = "";
                txtDayLightSaving.Text = "";

                RefreshMarketFee();
                RefreshComplianceDetails();

            }
        }


        /// <summary>
        /// This method shows the Market Fee values against the selected auec. 
        /// </summary>
        /// <param name="auec"></param>
        private void SetupMarketFeesTab(Prana.Admin.BLL.AUEC auec)
        {
            if (auec != null)
            {
                #region Old Code
                ////This(MarketFee) property of auec fetches the marketfee value from the database.
                //MarketFee marketFee = auec.MarketFee;

                //if(marketFee != null)
                //{
                //    txtPurchaseSecFees.Text = marketFee.PurchaseSecFees.ToString();
                //    txtSalesSecFees.Text = marketFee.SaleSecFees.ToString();
                //    txtPurchaseStamp.Text = marketFee.PurchaseStamp.ToString();
                //    txtSaleStamp.Text = marketFee.SaleStamp.ToString();
                //    txtPurchaseLevy.Text = marketFee.PurchaseLevy.ToString();
                //    txtSaleLevy.Text = marketFee.SaleLevy.ToString();
                //}
                #endregion
                //This(MarketFee) property of auec fetches the marketfee value from the database.
                txtPurchaseSecFees.Text = auec.PurchaseSecFees.ToString();
                txtSalesSecFees.Text = auec.SaleSecFees.ToString();
                txtPurchaseStamp.Text = auec.PurchaseStamp.ToString();
                txtSaleStamp.Text = auec.SaleStamp.ToString();
                txtPurchaseLevy.Text = auec.PurchaseLevy.ToString();
                txtSaleLevy.Text = auec.SaleLevy.ToString();
            }
            else
            {
                RefreshMarketFee();
            }
        }

        private void RefreshMarketFee()
        {
            txtPurchaseSecFees.Text = NULLVALUE;
            txtSalesSecFees.Text = NULLVALUE;
            txtPurchaseStamp.Text = NULLVALUE;
            txtSaleStamp.Text = NULLVALUE;
            txtPurchaseLevy.Text = NULLVALUE;
            txtSaleLevy.Text = NULLVALUE;
        }

        //		/// <summary>
        //		/// This method shows the holidays against the selected auec.
        //		/// </summary>
        //		/// <param name="auec"></param>
        //		private void SetupHolidayTab(Prana.Admin.BLL.AUEC auec)
        //		{
        //			Holidays holidays = AUECManager.GetHolidays(auec.AUECID);
        //
        //			if(holidays != null)
        //			{
        //			//	BindHoliday();
        //				this.grdHoliday.DataSource = holidays;
        //			}
        //		}

        //SK 20061009 removed Compliance class and references
        //private void SetupComplainceTabBeforeSave(Compliance compliance, Sides sides, OrderTypes orderTypes)
        //{
        //    //TODO: Some changes.
        //    if (_isStoredCopy == true)
        //    {
        //        cmbCurrency.Value = int.Parse(compliance.BaseCurrencyID.ToString());
        //        cmbOtherCurrency.Value = int.Parse(compliance.OtherCurrencyID.ToString());

        //        int multiplier = int.Parse(compliance.Multiplier.ToString());
        //        if (multiplier != int.MinValue)
        //        {
        //            txtMultiplier.Text = compliance.Multiplier.ToString();
        //        }

        //        //cmb compliance.IdentifierName =  "CMTA"; //cmbIdentifierName.SelectedValue.ToString();
        //        cmbIdentifierName.Value = int.Parse(compliance.IdentifierID.ToString());

        //        //cmbUnits.Value = int.Parse(cmbOtherCurrency.Value.ToString());

        //        if (int.Parse(compliance.IsShortSaleConfirmation.ToString()) == int.MinValue)
        //        {
        //            cmbShortSaleConfirmation.Text = C_COMBO_SELECT;
        //        }
        //        else if (((int)Prana.Admin.BLL.ComplianceOptions.ShortSaleConfirmation) == int.Parse(compliance.IsShortSaleConfirmation.ToString()))
        //        {
        //            cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.Yes.ToString();
        //        }
        //        else
        //        {
        //            cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.No.ToString();
        //        }
        //        if (int.Parse(compliance.ProvideAccountNameWithTrade.ToString()) == int.MinValue)
        //        {
        //            cmbProvideAccountnamewithTrade.Text = C_COMBO_SELECT;
        //        }
        //        else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentAccountNameWithTrade) == int.Parse(compliance.ProvideAccountNameWithTrade.ToString()))
        //        {
        //            cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
        //        }
        //        else
        //        {
        //            cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.No.ToString();
        //        }
        //        if (int.Parse(compliance.ProvideIdentifierNameWithTrade.ToString()) == int.MinValue)
        //        {
        //            cmbProvideidentifierwithtrade.Text = C_COMBO_SELECT;
        //        }
        //        else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentIdentifierWithTrade) == int.Parse(compliance.ProvideIdentifierNameWithTrade.ToString()))
        //        {
        //            cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
        //            cmbIdentifierName.Enabled = true;
        //        }
        //        else
        //        {
        //            cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.No.ToString();
        //            cmbIdentifierName.Enabled = false;
        //        }

        //        for (int j = 0; j < checkedlstSide.Items.Count; j++)
        //        {
        //            checkedlstSide.SetItemChecked(j, false);
        //        }

        //        if (sides.Count > 0)
        //        {
        //            foreach (Prana.Admin.BLL.Side side in sides)
        //            {
        //                for (int j = 0; j < checkedlstSide.Items.Count; j++)
        //                {
        //                    if (((Prana.Admin.BLL.Side)checkedlstSide.Items[j]).SideID == int.Parse(side.SideID.ToString()))
        //                    {
        //                        checkedlstSide.SetItemChecked(j, true);
        //                    }
        //                }
        //            }
        //        }

        //        for (int j = 0; j < checkedlstOrderTypes.Items.Count; j++)
        //        {
        //            checkedlstOrderTypes.SetItemChecked(j, false);
        //        }
        //        if (orderTypes.Count > 0)
        //        {
        //            foreach (Prana.Admin.BLL.OrderType orderType in orderTypes)
        //            {
        //                for (int j = 0; j < checkedlstOrderTypes.Items.Count; j++)
        //                {
        //                    if (((Prana.Admin.BLL.OrderType)checkedlstOrderTypes.Items[j]).OrderTypesID == int.Parse(orderType.OrderTypesID.ToString()))
        //                    {
        //                        checkedlstOrderTypes.SetItemChecked(j, true);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //

        /// <summary>
        /// This method shows the values of compliance against the selected auec.
        /// </summary>
        /// <param name="auec"></param>
        private void SetupComplianceTab(Prana.Admin.BLL.AUEC auec)
        {
            if (auec != null)
            {
                #region Old Code
                //Compliance compliance = auec.Compliance;

                //compliance.AUECID = auec.AUECID;

                //int auecID = int.Parse(auec.AUECID.ToString());
                //Sides sides = AUECManager.GetAUECSides(auecID);	
                //OrderTypes orderTypes = AUECManager.GetAUECOrderTypes(auecID);

                //cmbCurrency.Value = int.Parse(compliance.BaseCurrencyID.ToString());
                //cmbOtherCurrency.Value = int.Parse(compliance.OtherCurrencyID.ToString());

                //int multiplier = int.Parse(compliance.Multiplier.ToString());
                //if(multiplier != int.MinValue)
                //{
                //    txtMultiplier.Text = compliance.Multiplier.ToString();
                //}

                ////cmb compliance.IdentifierName =  "CMTA"; //cmbIdentifierName.SelectedValue.ToString();
                //cmbIdentifierName.Value = int.Parse(compliance.IdentifierID.ToString());

                ////cmbUnits.Value = int.Parse(cmbOtherCurrency.Value.ToString());

                //if(int.Parse(auec.Compliance.IsShortSaleConfirmation.ToString()) == int.MinValue)
                //{
                //    cmbShortSaleConfirmation.Text = C_COMBO_SELECT;
                //}
                //else if (((int)Prana.Admin.BLL.ComplianceOptions.ShortSaleConfirmation) == int.Parse(auec.Compliance.IsShortSaleConfirmation.ToString()))
                //{
                //    cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.Yes.ToString();
                //}
                //else
                //{
                //    cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.No.ToString();
                //}
                //if(int.Parse(auec.Compliance.ProvideAccountNameWithTrade.ToString()) == int.MinValue)
                //{
                //    cmbProvideAccountnamewithTrade.Text = C_COMBO_SELECT;
                //}
                //else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentAccountNameWithTrade) == int.Parse(auec.Compliance.ProvideAccountNameWithTrade.ToString()))
                //{
                //    cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
                //}
                //else
                //{
                //    cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.No.ToString();
                //}
                //if(int.Parse(auec.Compliance.ProvideIdentifierNameWithTrade.ToString()) == int.MinValue)
                //{
                //    cmbProvideidentifierwithtrade.Text = C_COMBO_SELECT;
                //}
                //else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentIdentifierWithTrade) == int.Parse(auec.Compliance.ProvideIdentifierNameWithTrade.ToString()))
                //{
                //    cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
                //    cmbIdentifierName.Enabled = true;
                //}
                //else
                //{		
                //    cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.No.ToString();
                //    cmbIdentifierName.Enabled = false;
                //}
                #endregion

                int auecID = int.Parse(auec.AUECID.ToString());
                Prana.Admin.BLL.OrderTypes orderTypes = new BLL.OrderTypes();

                cmbCurrency.Value = auec.CurrencyID; //parsing already integer value makes no sense..
                cmbOtherCurrency.Value = auec.OtherCurrencyID;

                if (auec.Multiplier != int.MinValue)
                {
                    txtMultiplier.Text = auec.Multiplier.ToString();
                }
                if (auec.RoundLot != decimal.MinValue)
                    txtRoundLot.Text = auec.RoundLot.ToString();

                cmbIdentifierName.Value = auec.IdentifierID;

                if (int.Parse(auec.IsShortSaleConfirmation.ToString()) == int.MinValue)
                {
                    cmbShortSaleConfirmation.Text = C_COMBO_SELECT;
                }
                else if (((int)Prana.Admin.BLL.ComplianceOptions.ShortSaleConfirmation) == int.Parse(auec.IsShortSaleConfirmation.ToString()))
                {
                    cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.Yes.ToString();
                }
                else
                {
                    cmbShortSaleConfirmation.Text = Prana.Admin.BLL.Options.No.ToString();
                }
                if (int.Parse(auec.ProvideAccountNameWithTrade.ToString()) == int.MinValue)
                {
                    cmbProvideAccountnamewithTrade.Text = C_COMBO_SELECT;
                }
                else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentAccountNameWithTrade) == int.Parse(auec.ProvideAccountNameWithTrade.ToString()))
                {
                    cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
                }
                else
                {
                    cmbProvideAccountnamewithTrade.Text = Prana.Admin.BLL.Options.No.ToString();
                }
                if (int.Parse(auec.ProvideIdentifierNameWithTrade.ToString()) == int.MinValue)
                {
                    cmbProvideidentifierwithtrade.Text = C_COMBO_SELECT;
                }
                else if (((int)Prana.Admin.BLL.ComplianceOptions.ProvidentIdentifierWithTrade) == int.Parse(auec.ProvideIdentifierNameWithTrade.ToString()))
                {
                    cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.Yes.ToString();
                    cmbIdentifierName.Enabled = true;
                }
                else
                {
                    cmbProvideidentifierwithtrade.Text = Prana.Admin.BLL.Options.No.ToString();
                    cmbIdentifierName.Enabled = false;
                }
            }
            else
            {
                RefreshComplianceDetails();
            }
        }

        private void RefreshComplianceDetails()
        {
            cmbCurrency.Text = C_COMBO_SELECT;
            cmbOtherCurrency.Text = C_COMBO_SELECT;
            //spnMultiplier.Value = 0;
            txtMultiplier.Text = "";
            cmbShortSaleConfirmation.Text = C_COMBO_SELECT;
            cmbProvideAccountnamewithTrade.Text = C_COMBO_SELECT;
            cmbProvideidentifierwithtrade.Text = C_COMBO_SELECT;
            cmbIdentifierName.Text = "None";
            //checkedlstSide.Refresh();
            //checkedlstOrderTypes.Refresh();	
            for (int j = 0; j < checkedlstSide.Items.Count; j++)
            {
                checkedlstSide.SetItemChecked(j, false);
            }

        }

        /// <summary>
        /// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
        /// </summary>
        /// <param name="nodeDetails"></param>
        private void SelectTreeNode(NodeDetails nodeDetails)
        {
            switch (nodeDetails.Type)
            {
                case NodeType.Asset:
                    foreach (TreeNode node in trvAsset.Nodes[0].Nodes)
                    {
                        if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                        {
                            trvAsset.SelectedNode = node;
                            break;
                        }
                    }
                    break;

                case NodeType.Underlying:
                    foreach (TreeNode node in trvAsset.Nodes[1].Nodes)
                    {
                        if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                        {
                            trvAsset.SelectedNode = node;
                            break;
                        }
                    }
                    break;

                case NodeType.Exchange:
                    foreach (TreeNode node in trvAsset.Nodes[0].Nodes[0].Nodes)
                    {
                        if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                        {
                            trvAsset.SelectedNode = node;
                            break;
                        }
                    }
                    break;
            }
        }

        #region Temp Functions. Remove them.

        /// <summary>
        /// This method fills the compliance combo boxes with the some default values at the begining of form
        /// load.
        /// </summary>
        private void InitializeCompliance()
        {
            //ToDo: Remove this thing and put original things.

            //Creating a datatable and filling it with some rows.
            System.Data.DataTable dt = new System.Data.DataTable();

            //CurrencyConvention
            dt.Columns.Add("Data");
            dt.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            //Inserting the - Select - option in the Combo Box at the top.
            dt.Rows.Add(row);

            row[0] = "Direct";
            row[1] = "0";
            dt.Rows.Add(row);
            row[0] = "InDirect";
            row[1] = "1";
            dt.Rows.Add(row);
            cmbCurrencyConversion.DataSource = null;
            cmbCurrencyConversion.DataSource = dt;
            cmbCurrencyConversion.DisplayMember = "Data";
            cmbCurrencyConversion.ValueMember = "Value";
            cmbCurrencyConversion.Text = C_COMBO_SELECT;


            //cmbShortSaleConfirmation
            cmbShortSaleConfirmation.DataSource = null;
            cmbShortSaleConfirmation.DataSource = GetDataTable();
            cmbShortSaleConfirmation.DisplayMember = "Data";
            cmbShortSaleConfirmation.ValueMember = "Value";
            cmbShortSaleConfirmation.Text = C_COMBO_SELECT;

            //cmbProvideAccountnamewithTrade
            cmbProvideAccountnamewithTrade.DataSource = null;
            cmbProvideAccountnamewithTrade.DataSource = GetDataTable();
            cmbProvideAccountnamewithTrade.DisplayMember = "Data";
            cmbProvideAccountnamewithTrade.ValueMember = "Value";
            cmbProvideAccountnamewithTrade.Text = C_COMBO_SELECT;

            //cmbProvideidentifierwithtrade
            cmbProvideidentifierwithtrade.DataSource = null;
            cmbProvideidentifierwithtrade.DataSource = GetDataTable();
            cmbProvideidentifierwithtrade.DisplayMember = "Data";
            cmbProvideidentifierwithtrade.ValueMember = "Value";
            cmbProvideidentifierwithtrade.Text = C_COMBO_SELECT;


            //cmbIdentifierName.Value = 0;
        }

        /// <summary>
        /// This method fills the datable with a default row and two boolean rows.
        /// </summary>
        /// <returns>A datatable's object.</returns>
        private System.Data.DataTable GetDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            object[] row = new object[2];
            dt.Columns.Add("Data");
            dt.Columns.Add("Value");

            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dt.Rows.Add(row);

            row = new object[2];
            row[0] = "Yes";
            row[1] = "1";
            dt.Rows.Add(row);
            row[0] = "No";
            row[1] = "0";
            dt.Rows.Add(row);
            return dt;
        }

        #endregion

        private void label20_Click(object sender, System.EventArgs e)
        {

        }


        #region NodeDetails

        class NodeDetails
        {
            private NodeType _type = NodeType.Asset;
            private int _nodeID = int.MinValue;
            private int _exchangeID = int.MinValue;

            private int _assetID = int.MinValue;
            private int _underLyingID = int.MinValue;

            public NodeDetails()
            {
            }

            public NodeDetails(NodeType type, int nodeID, int exchangeID)
            {
                _type = type;
                _nodeID = nodeID;
                _exchangeID = exchangeID;
            }

            public NodeDetails(NodeType type, int nodeID, int assetID, int underLyingID, int exchangeID)
            {
                _type = type;
                _nodeID = nodeID;
                _assetID = assetID;
                _underLyingID = underLyingID;
                _exchangeID = exchangeID;
            }

            public NodeType Type
            {
                get { return _type; }
                set { _type = value; }
            }
            public int NodeID
            {
                get { return _nodeID; }
                set { _nodeID = value; }
            }
            public int ExchangeID
            {
                get { return _exchangeID; }
                set { _exchangeID = value; }
            }
            public int AssetID
            {
                get { return _assetID; }
                set { _assetID = value; }
            }
            public int UnderLyingID
            {
                get { return _underLyingID; }
                set { _underLyingID = value; }
            }
        }

        enum NodeType
        {
            Asset = 1,
            Underlying = 2,
            Exchange = 3
        }

        #endregion

        #region Focus Color
        private void txtExchangeNameFull_GotFocus(object sender, System.EventArgs e)
        {
            txtExchangeNameFull.BackColor = Color.LemonChiffon;
        }
        private void txtExchangeNameFull_LostFocus(object sender, System.EventArgs e)
        {
            txtExchangeNameFull.BackColor = Color.White;
        }
        private void txtExchangeIdentifier_GotFocus(object sender, System.EventArgs e)
        {
            txtExchangeIdentifier.BackColor = Color.LemonChiffon;
        }
        private void txtExchangeIdentifier_LostFocus(object sender, System.EventArgs e)
        {
            txtExchangeIdentifier.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void cmbUnits_GotFocus(object sender, System.EventArgs e)
        {
            cmbUnits.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbUnits_LostFocus(object sender, System.EventArgs e)
        {
            cmbUnits.Appearance.BackColor = Color.White;
        }
        private void txtPreMarketTradingStartTime_GotFocus(object sender, System.EventArgs e)
        {
            txtPreMarketTradingStartTime.BackColor = Color.LemonChiffon;
        }
        private void txtPreMarketTradingStartTime_LostFocus(object sender, System.EventArgs e)
        {
            txtPreMarketTradingStartTime.BackColor = Color.White;
        }
        private void txtPreMarketTradingEndTime_GotFocus(object sender, System.EventArgs e)
        {
            txtPreMarketTradingEndTime.BackColor = Color.LemonChiffon;
        }
        private void txtPreMarketTradingEndTime_LostFocus(object sender, System.EventArgs e)
        {
            txtPreMarketTradingEndTime.BackColor = Color.White;
        }
        private void txtLunchTimeStartTime_GotFocus(object sender, System.EventArgs e)
        {
            txtLunchTimeStartTime.BackColor = Color.LemonChiffon;
        }
        private void txtLunchTimeStartTime_LostFocus(object sender, System.EventArgs e)
        {
            txtLunchTimeStartTime.BackColor = Color.White;
        }
        private void txtLunchTimeEndTime_GotFocus(object sender, System.EventArgs e)
        {
            txtLunchTimeEndTime.BackColor = Color.LemonChiffon;
        }
        private void txtLunchTimeEndTime_LostFocus(object sender, System.EventArgs e)
        {
            txtLunchTimeEndTime.BackColor = Color.White;
        }
        private void txtRegularTradingStartTime_GotFocus(object sender, System.EventArgs e)
        {
            txtRegularTradingStartTime.BackColor = Color.LemonChiffon;
        }
        private void txtRegularTradingStartTime_LostFocus(object sender, System.EventArgs e)
        {
            txtRegularTradingStartTime.BackColor = Color.White;
        }
        private void txtRegularTradingEndTime_GotFocus(object sender, System.EventArgs e)
        {
            txtRegularTradingEndTime.BackColor = Color.LemonChiffon;
        }
        private void txtRegularTradingEndTime_LostFocus(object sender, System.EventArgs e)
        {
            txtRegularTradingEndTime.BackColor = Color.White;
        }
        private void txtPostMarketTradingStartTime_GotFocus(object sender, System.EventArgs e)
        {
            txtPostMarketTradingStartTime.BackColor = Color.LemonChiffon;
        }
        private void txtPostMarketTradingStartTime_LostFocus(object sender, System.EventArgs e)
        {
            txtPostMarketTradingStartTime.BackColor = Color.White;
        }
        private void txtPostMarketTradingEndTime_GotFocus(object sender, System.EventArgs e)
        {
            txtPostMarketTradingEndTime.BackColor = Color.LemonChiffon;
        }
        private void txtPostMarketTradingEndTime_LostFocus(object sender, System.EventArgs e)
        {
            txtPostMarketTradingEndTime.BackColor = Color.White;
        }
        private void txtSettlementDays_GotFocus(object sender, System.EventArgs e)
        {
            txtSettlementDays.BackColor = Color.LemonChiffon;
        }
        private void txtSettlementDays_LostFocus(object sender, System.EventArgs e)
        {
            txtSettlementDays.BackColor = Color.White;
        }
        private void txtSettlementDaysSell_GotFocus(object sender, System.EventArgs e)
        {
            txtSettlementDaysSell.BackColor = Color.LemonChiffon;
        }
        private void txtSettlementDaysSell_LostFocus(object sender, System.EventArgs e)
        {
            txtSettlementDaysSell.BackColor = Color.White;
        }
        private void txtDayLightSaving_GotFocus(object sender, System.EventArgs e)
        {
            txtDayLightSaving.BackColor = Color.LemonChiffon;
        }
        private void txtDayLightSaving_LostFocus(object sender, System.EventArgs e)
        {
            txtDayLightSaving.BackColor = Color.White;
        }
        private void cmbCountry_GotFocus(object sender, System.EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCountry_LostFocus(object sender, System.EventArgs e)
        {
            cmbCountry.Appearance.BackColor = Color.White;
        }

        private void txtSalesSecFees_GotFocus(object sender, System.EventArgs e)
        {
            txtSalesSecFees.BackColor = Color.LemonChiffon;
        }
        private void txtSalesSecFees_LostFocus(object sender, System.EventArgs e)
        {
            txtSalesSecFees.BackColor = Color.White;
        }
        private void txtPurchaseSecFees_GotFocus(object sender, System.EventArgs e)
        {
            txtPurchaseSecFees.BackColor = Color.LemonChiffon;
        }
        private void txtPurchaseSecFees_LostFocus(object sender, System.EventArgs e)
        {
            txtPurchaseSecFees.BackColor = Color.White;
        }
        private void txtPurchaseStamp_GotFocus(object sender, System.EventArgs e)
        {
            txtPurchaseStamp.BackColor = Color.LemonChiffon;
        }
        private void txtPurchaseStamp_LostFocus(object sender, System.EventArgs e)
        {
            txtPurchaseStamp.BackColor = Color.White;
        }
        private void txtSaleStamp_GotFocus(object sender, System.EventArgs e)
        {
            txtSaleStamp.BackColor = Color.LemonChiffon;
        }
        private void txtSaleStamp_LostFocus(object sender, System.EventArgs e)
        {
            txtSaleStamp.BackColor = Color.White;
        }
        private void txtPurchaseLevy_GotFocus(object sender, System.EventArgs e)
        {
            txtPurchaseLevy.BackColor = Color.LemonChiffon;
        }
        private void txtPurchaseLevy_LostFocus(object sender, System.EventArgs e)
        {
            txtPurchaseLevy.BackColor = Color.White;
        }
        private void txtSaleLevy_GotFocus(object sender, System.EventArgs e)
        {
            txtSaleLevy.BackColor = Color.LemonChiffon;
        }
        private void txtSaleLevy_LostFocus(object sender, System.EventArgs e)
        {
            txtSaleLevy.BackColor = Color.White;
        }
        private void dttHolidayDate_GotFocus(object sender, System.EventArgs e)
        {
            dttHolidayDate.BackColor = Color.LemonChiffon;
        }
        private void dttHolidayDate_LostFocus(object sender, System.EventArgs e)
        {
            dttHolidayDate.BackColor = Color.White;
        }
        private void cmbCurrencyConversion_GotFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyConversion.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCurrencyConversion_LostFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyConversion.Appearance.BackColor = Color.White;
        }
        private void txtMultiplier_GotFocus(object sender, System.EventArgs e)
        {

            txtMultiplier.BackColor = Color.LemonChiffon;
        }
        private void txtMultiplier_LostFocus(object sender, System.EventArgs e)
        {
            txtMultiplier.BackColor = Color.White;
        }

        private void cmbShortSaleConfirmation_GotFocus(object sender, System.EventArgs e)
        {
            cmbShortSaleConfirmation.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbShortSaleConfirmation_LostFocus(object sender, System.EventArgs e)
        {
            cmbShortSaleConfirmation.Appearance.BackColor = Color.White;
        }
        private void cmbProvideAccountnamewithTrade_GotFocus(object sender, System.EventArgs e)
        {
            cmbProvideAccountnamewithTrade.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbProvideAccountnamewithTrade_LostFocus(object sender, System.EventArgs e)
        {
            cmbProvideAccountnamewithTrade.Appearance.BackColor = Color.White;
        }
        private void cmbProvideidentifierwithtrade_GotFocus(object sender, System.EventArgs e)
        {
            cmbProvideidentifierwithtrade.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbProvideidentifierwithtrade_LostFocus(object sender, System.EventArgs e)
        {
            cmbProvideidentifierwithtrade.Appearance.BackColor = Color.White;
        }
        private void cmbIdentifierName_GotFocus(object sender, System.EventArgs e)
        {
            cmbIdentifierName.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbIdentifierName_LostFocus(object sender, System.EventArgs e)
        {
            cmbIdentifierName.Appearance.BackColor = Color.White;
        }
        private void txtHolidayDescription_GotFocus(object sender, System.EventArgs e)
        {
            txtHolidayDescription.BackColor = Color.LemonChiffon;
        }
        private void txtHolidayDescription_LostFocus(object sender, System.EventArgs e)
        {
            txtHolidayDescription.BackColor = Color.White;
        }

        private void txtDayLightSavingTime_GotFocus(object sender, System.EventArgs e)
        {
            txtDayLightSavingTime.BackColor = Color.LemonChiffon;
        }
        private void txtDayLightSavingTime_LostFocus(object sender, System.EventArgs e)
        {
            txtDayLightSavingTime.BackColor = Color.White;
        }

        private void cmbTimeZone_GotFocus(object sender, System.EventArgs e)
        {
            cmbTimeZone.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbTimeZone_LostFocus(object sender, System.EventArgs e)
        {
            cmbTimeZone.Appearance.BackColor = Color.White;
            //cmbTimeZone.CloseEditorButtonDropDowns();

        }
        private void cmbExchangeLogo_GotFocus(object sender, System.EventArgs e)
        {
            cmbExchangeLogo.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbExchangeLogo_LostFocus(object sender, System.EventArgs e)
        {
            cmbExchangeLogo.Appearance.BackColor = Color.White;
        }
        private void cmbSymbolConvention_GotFocus(object sender, System.EventArgs e)
        {
            cmbSymbolConvention.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbSymbolConvention_LostFocus(object sender, System.EventArgs e)
        {
            cmbSymbolConvention.Appearance.BackColor = Color.White;
        }
        private void cmbCurrency_GotFocus(object sender, System.EventArgs e)
        {
            cmbCurrency.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCurrency_LostFocus(object sender, System.EventArgs e)
        {
            cmbCurrency.Appearance.BackColor = Color.White;
        }
        private void cmbFlag_GotFocus(object sender, System.EventArgs e)
        {
            cmbFlag.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbFlag_LostFocus(object sender, System.EventArgs e)
        {
            cmbFlag.Appearance.BackColor = Color.White;
        }
        private void cmbOtherCurrency_GotFocus(object sender, System.EventArgs e)
        {
            cmbOtherCurrency.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbOtherCurrency_LostFocus(object sender, System.EventArgs e)
        {
            cmbOtherCurrency.Appearance.BackColor = Color.White;
        }
        private void cmbState_GotFocus(object sender, System.EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbState_LostFocus(object sender, System.EventArgs e)
        {
            cmbState.Appearance.BackColor = Color.White;
        }

        #endregion

        private void ultraTabPageControl5_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        /// <summary>
        /// Saves the holidays in the database on the click event of the Save button in the form.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveHoliday_Click(object sender, System.EventArgs e)
        {
            try
            {
                //				Holidays holidays = ((Holidays)grdHoliday.DataSource);
                //				
                //				AUECManager.SaveAUECHolidays(holidays);
                //				//Calls the BindHoliday method as soon as it saves the holidays in the database to reflect
                //				//the changes for it in the database.
                //				BindHoliday();
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSaveHoliday_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSaveHoliday_Click", null);


                #endregion
            }

        }

        /// <summary>
        /// Deletes the holiday on the click event of the button in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteHoliday_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Checks if it has any holiday to delete in the grid.
                if (grdHoliday.Rows.Count > 0)
                {
                    string holidayDescription = grdHoliday.ActiveRow.Cells["Description"].Text.ToString();
                    if (holidayDescription != "")
                    {
                        if (MessageBox.Show(this, "Do you want to delete this Holiday?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int id = int.Parse(grdHoliday.ActiveRow.Cells["HolidayId"].Value.ToString());

                            AUECManager.DeleteHoliday(id);
                            //Calls the BindHoliday method as soon as it saves the holidays in the database to reflect
                            //the changes for it in the database.
                            BindHoliday();
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDeleteHoliday_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnDeleteHoliday_Click", null);


                #endregion
            }
        }

        /// <summary>
        /// Calls the refresh method associated with it and its parent's refresh method when this combo's 
        /// value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTimeZone_ValueChanged(object sender, System.EventArgs e)
        {
            this.Refresh();
            //this.Parent.Refresh();
            this.Owner.Refresh();
        }

        /// <summary>
        /// Enables the TextBox-txtRegularTradingStartTime when CheckBox-chkRegularMarket's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRegularMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkRegularMarket.Checked == true)
            {
                txtRegularTradingStartTime.Enabled = true;
                txtRegularTradingEndTime.Enabled = true;
            }
            else
            {
                txtRegularTradingStartTime.Enabled = false;
                txtRegularTradingEndTime.Enabled = false;
            }
        }

        private void cmbCountry_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCountry.Value != null)
            {
                int countryID = int.Parse(cmbCountry.Value.ToString());
                if (countryID > 0)
                {
                    //GetStates method fetches the existing states from the database.
                    States states = GeneralManager.GetStates(countryID);
                    if (states.Count > 0)
                    {
                        states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
                        cmbState.DisplayMember = "StateName";
                        cmbState.ValueMember = "StateID";
                        cmbState.DataSource = null;
                        cmbState.DataSource = states;
                        cmbState.Value = int.MinValue;

                        ColumnsCollection cmbStatescolumns = cmbState.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in cmbStatescolumns)
                        {
                            if (column.Key != "StateName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }
                    }
                }
                else
                {
                    BindEmptyStates();
                }
            }
        }

        /// <summary>
        /// Enables the TextBox-txtLunchTimeStartTime when CheckBox-chkLunchTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLunchTime_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkLunchTime.Checked == true)
            {
                txtLunchTimeStartTime.Enabled = true;
                txtLunchTimeEndTime.Enabled = true;
            }
            else
            {
                txtLunchTimeStartTime.Enabled = false;
                txtLunchTimeEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtPreMarketTradingStartTime when CheckBox-chkPreMarketStartTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPreMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkPreMarket.Checked == true)
            {
                txtPreMarketTradingStartTime.Enabled = true;
                txtPreMarketTradingEndTime.Enabled = true;
            }
            else
            {
                txtPreMarketTradingStartTime.Enabled = false;
                txtPreMarketTradingEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtPostMarketTradingEndTime when CheckBox-chkPostMarketEndTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPostMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkPostMarket.Checked == true)
            {
                txtPostMarketTradingStartTime.Enabled = true;
                txtPostMarketTradingEndTime.Enabled = true;
            }
            else
            {
                txtPostMarketTradingStartTime.Enabled = false;
                txtPostMarketTradingEndTime.Enabled = false;
            }
        }

        private UploadAUECHolidays frmUploadAUECHolidays = null;
        private void btnUploadHolidays_Click(object sender, System.EventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;

            if (frmUploadAUECHolidays == null)
            {
                frmUploadAUECHolidays = new UploadAUECHolidays();
            }
            frmUploadAUECHolidays.AUECID = nodeDetails.NodeID;
            //frmUploadAUECHolidays.ShowDialog(this);
            frmUploadAUECHolidays.ShowDialog(this.Parent);
        }

        private void btnLoadDefaultHolidays_Click(object sender, System.EventArgs e)
        {
            NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
            int auecID = int.MinValue;
            auecID = nodeDetails.NodeID;
            Prana.Admin.BLL.AUEC auec = new Prana.Admin.BLL.AUEC();
            //Commented the following line as the exchangeID can be retrieved frm auec itself.
            //auecExchange = AUECManager.GetAUECExchange(auecExchangeID);
            auec = AUECManager.GetAUECDetails(auecID);

            int exchangeID = int.MinValue;
            exchangeID = auec.ExchangeID;

            //			Prana.Admin.BLL.Holidays checkHolidays = new Prana.Admin.BLL.Holidays();
            //			checkHolidays = AUECManager.GetHolidays(auecID, exchangeID);

            Holidays exchangeHolidays = new Holidays();
            exchangeHolidays = ExchangeManager.GetHolidays(exchangeID);
            foreach (Holiday holiday in exchangeHolidays)
            {
                holiday.AUECID = auecID;
            }
            bool result = false;
            result = AUECManager.SaveAUECHolidaysExchangeDefault(exchangeHolidays, auecID);
            if (result == true)
            {
                //Holidays Saved.
                BindHoliday();
            }
            else
            {
                //Holidays not saved.
            }

        }

        private void cmbFlag_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.DefaultRowHeight = 20;
            Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
            aImageRenderer.DrawBorderShadow = false;
            e.Layout.Bands[0].Columns["CountryFlagImage"].Editor = aImageRenderer;
        }

        private void tbcAUEC_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (trvAsset.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    tbcAUEC.Enabled = true;
                    DisableTabsForAssetNode(nodeDetails.Type);
                    switch (nodeDetails.Type)
                    {
                        case NodeType.Underlying:
                            tbcAUEC.Enabled = false;
                            break;
                        case NodeType.Exchange:
                            if (tbcAUEC.Tabs[1].Selected) //Market fee tab selection is checked.
                            {
                                if (tabCtrlOtherFee.Tabs.Count > 0)
                                {
                                    tabCtrlOtherFee.ActiveTab = tabCtrlOtherFee.Tabs[0];
                                    tabCtrlOtherFee.BringToFront();
                                    tabCtrlOtherFee.Enabled = true;
                                    tabCtrlOtherFee.Tabs[0].Selected = true;
                                    tabCtrlOtherFee.Tabs[0].Active = true;
                                    tabCtrlOtherFee.Tabs[0].Enabled = true;
                                    //tabCtrlOtherFee.Tabs[0].TabPage.Refresh();

                                    for (int index = 0; index < tabCtrlOtherFee.Tabs.Count; index++)
                                    {
                                        tabCtrlOtherFee.Tabs[index].Selected = true;
                                    }
                                    if (tabCtrlOtherFee.Tabs.Count > 0)
                                    {
                                        tabCtrlOtherFee.Tabs[0].Selected = true;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("trvAsset_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvAsset_AfterSelect", null);


                #endregion
            }
        }

        private void grbAsset_Enter(object sender, System.EventArgs e)
        {

        }

        public byte[] m_barrImg = null;
        public string _fullName = string.Empty;
        public string _flagName = string.Empty;
        private void btnUploadAUECFlag_Click(object sender, System.EventArgs e)
        {
            try
            {
                long m_lImageFileLength = long.MinValue;

                openFileDialog1.InitialDirectory = "DeskTop";
                openFileDialog1.Filter = "Icon Files (*.ico)|*.ico";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string strFn = openFileDialog1.FileName;
                    //pictureBox1.Image=Image.FromFile(strFn);

                    FileInfo fiImage = new FileInfo(strFn);
                    m_lImageFileLength = fiImage.Length;

                    _fullName = fiImage.FullName;
                    _flagName = fiImage.Name;

                    FileStream fs = new FileStream(strFn, FileMode.Open,
                        FileAccess.Read, FileShare.Read);

                    m_barrImg = new byte[Convert.ToInt32(m_lImageFileLength)];
                    int iBytesRead = fs.Read(m_barrImg, 0,
                        Convert.ToInt32(m_lImageFileLength));
                    fs.Close();

                    Flags flags = (Flags)cmbFlag.DataSource;
                    int flagCount = flags.Count;
                    flags.Insert(flagCount, new Flag(int.MinValue, _flagName, m_barrImg));
                    BindFlags(flags);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            # endregion

            #region LogEntry
            finally
            {
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "ExchangeForm_Load", null);


            }
            #endregion
        }

        private void cmbExchangeLogo_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.DefaultRowHeight = 20;
            Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
            aImageRenderer.DrawBorderShadow = false;
            e.Layout.Bands[0].Columns["LogoImage"].Editor = aImageRenderer;
        }

        public string _logoName = string.Empty;
        private void btnUploadAUECLogo_Click(object sender, System.EventArgs e)
        {
            try
            {
                long m_lImageFileLength = long.MinValue;

                openFileDialog1.InitialDirectory = "DeskTop";
                openFileDialog1.Filter = "Icon Files (*.ico)|*.ico";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string strFn = openFileDialog1.FileName;

                    FileInfo fiImage = new FileInfo(strFn);
                    m_lImageFileLength = fiImage.Length;

                    _fullName = fiImage.FullName;
                    _logoName = fiImage.Name;

                    FileStream fs = new FileStream(strFn, FileMode.Open,
                        FileAccess.Read, FileShare.Read);

                    m_barrImg = new byte[Convert.ToInt32(m_lImageFileLength)];
                    int iBytesRead = fs.Read(m_barrImg, 0,
                        Convert.ToInt32(m_lImageFileLength));
                    fs.Close();

                    Logos logos = (Logos)cmbExchangeLogo.DataSource;
                    int logoCount = logos.Count;
                    logos.Insert(logoCount, new Logo(int.MinValue, _logoName, m_barrImg));
                    BindLogos(logos);
                    //openFileDialog1.d
                }
            }

            #region LogEntry
            finally
            {
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "ExchangeForm_Load", null);


            }
            #endregion
        }


        private void dttHolidayDate_ValueChanged(object sender, System.EventArgs e)
        {
            dttHolidayDate.Format = DateTimePickerFormat.Short;
        }

        private void cmbProvideidentifierwithtrade_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbProvideAccountnamewithTrade.Value != null)
            {
                int val = int.Parse(cmbProvideidentifierwithtrade.Value.ToString());
                if (cmbProvideidentifierwithtrade.Text.ToString() == "No")
                {
                    cmbIdentifierName.Value = int.MinValue;
                    cmbIdentifierName.Enabled = false;
                }
                else
                {
                    cmbIdentifierName.Enabled = true;
                }
            }
        }

        private void cmbOtherCurrency_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbOtherCurrency.Value != null)
            {
                int otherCurrencyValue = int.Parse(cmbOtherCurrency.Value.ToString());
                if (otherCurrencyValue == int.MinValue)
                {
                    cmbCurrencyConversion.Text = C_COMBO_SELECT;
                    cmbCurrencyConversion.Enabled = false;
                }
                else
                {
                    cmbCurrencyConversion.Enabled = true;
                }
            }
        }

        private void cmbCurrency_ValueChanged(object sender, EventArgs e)
        {
            int baseCurrencyValue = int.Parse(cmbCurrency.Value.ToString());
            if (baseCurrencyValue != int.MinValue)
            {
                Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();
                Prana.Admin.BLL.Currency removeCurrency = new Prana.Admin.BLL.Currency();
                foreach (Prana.Admin.BLL.Currency currency in currencies)
                {
                    if (currency.CurencyID == baseCurrencyValue)
                    {
                        removeCurrency = currency;
                        break;
                    }
                }
                currencies.Remove(removeCurrency);
                currencies.Insert(0, new Prana.Admin.BLL.Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
                this.cmbOtherCurrency.DataSource = null;
                this.cmbOtherCurrency.DataSource = currencies;
                this.cmbOtherCurrency.DisplayMember = "CurrencySymbol";
                this.cmbOtherCurrency.ValueMember = "CurencyID";
                this.cmbOtherCurrency.Value = int.MinValue;

                ColumnsCollection cmbOtherCurrencycolumns = cmbOtherCurrency.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in cmbOtherCurrencycolumns)
                {
                    if (column.Key != "CurrencySymbol")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        cmbOtherCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
                }
            }
        }


        private void tabCtrlOtherFee_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //tabCtrlOtherFee.Tabs[0].TabPage
        }


        private void CmbYear_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                int year = int.Parse(CmbYear.SelectedValue.ToString());
                Calendars calendars = new Calendars();
                calendars.Insert(0, new Calendar(int.MinValue, C_COMBO_SELECT, int.MinValue));
                foreach (Calendar calendar in _calendars)
                {
                    if (calendar.CalendarYear.Equals(year))
                    {
                        calendars.Add(calendar);
                    }
                }

                cmbCalendars.DataSource = null;
                cmbCalendars.DataSource = calendars;
                ColumnsCollection columns = cmbCalendars.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn col in columns)
                {
                    if (col.Key != "CalendarName")
                    {
                        col.Hidden = true;
                    }

                }
                cmbCalendars.DisplayLayout.Bands[0].ColHeadersVisible = false;

                if (trvAsset.SelectedNode != null)
                {
                    NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                    int auecID = nodeDetails.NodeID;
                    string name = string.Empty;
                    if (CmbYear.SelectedValue != null)
                    {
                        name = AUECManager.GetCalendar(auecID, int.Parse(CmbYear.SelectedValue.ToString()));
                    }


                    if (name.Equals(string.Empty))
                    {
                        cmbCalendars.SelectedRow = cmbCalendars.Rows[0];
                    }
                    else
                    {
                        foreach (UltraGridRow row in cmbCalendars.Rows)
                        {
                            if (row.Cells["CalendarName"].Text == name)
                            {
                                cmbCalendars.SelectedRow = row;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }


        private void lblEditCalendar_Click(object sender, EventArgs e)
        {
            try
            {
                Prana.AdminForms.CalendarHolidays cholidays = CalendarHolidays.GetInstance();
                cholidays.Show();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbCalendars_ValueChanged(object sender, EventArgs e)
        {
            Holidays holidays = new Holidays();
            try
            {
                // BindYear();
                if (cmbCalendars.SelectedRow != null && CmbYear.SelectedItem != null)
                {

                    holidays = AUECManager.GetCalendarHolidays(int.Parse(cmbCalendars.SelectedRow.Cells["CalendarID"].Text));
                    if (holidays.Count != 0)
                    {
                        grdHoliday.DataSource = holidays;
                        showColumns(grdHoliday);
                    }
                }
                else
                {
                    BindHoliday();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void showColumns(UltraGrid grid)
        {
            //Show only date and description columns to the grid
            ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn col in columns)
            {
                if (col.Key != "Date")
                {
                    if (col.Key != "Description")
                    {
                        // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                        if (col.Key != "MarketOff")
                        {
                            if (col.Key != "SettlementOff")
                            {
                                col.Hidden = true;
                            }
                        }
                    }
                }
            }

            UltraGridColumn colDate = grdHoliday.DisplayLayout.Bands[0].Columns["Date"];
            colDate.Header.VisiblePosition = 0;
            colDate.Hidden = false;

            UltraGridColumn colDescription = grdHoliday.DisplayLayout.Bands[0].Columns["Description"];
            colDescription.Header.VisiblePosition = 1;
            colDescription.Hidden = false;

            UltraGridColumn colMarketOff = grdHoliday.DisplayLayout.Bands[0].Columns["MarketOff"];
            colMarketOff.Header.VisiblePosition = 3;
            colMarketOff.Hidden = false;
            colMarketOff.CellActivation = Activation.AllowEdit;

            UltraGridColumn colSettlementOff = grdHoliday.DisplayLayout.Bands[0].Columns["SettlementOff"];
            colSettlementOff.Header.VisiblePosition = 4;
            colSettlementOff.Hidden = false;
            colSettlementOff.CellActivation = Activation.AllowEdit;

        }


        private void cBoxAuec_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Holidays holidays = null;
                NodeDetails nodeDetails = (NodeDetails)trvAsset.SelectedNode.Tag;
                int auecID = nodeDetails.NodeID;
                int exchangeID = int.MinValue;
                object selectedItem = CmbYear.SelectedValue;
                if (!cBoxAuec.Checked)
                {
                    CmbYear.Enabled = true;
                    cmbCalendars.Enabled = true;
                    if (selectedItem != null)
                    {
                        holidays = AUECManager.GetHolidays(auecID, exchangeID, int.Parse(selectedItem.ToString()), 0);
                    }
                }
                else
                {
                    CmbYear.Enabled = false;
                    cmbCalendars.Enabled = false;
                    holidays = AUECManager.GetHolidays(auecID, exchangeID, int.MinValue, 1);
                }

                if (holidays != null && holidays.Count != 0)
                {
                    this.grdHoliday.DataSource = holidays;

                    ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "Date")
                        {
                            if (column.Key != "Description")
                            {
                                // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                                if (column.Key != "MarketOff")
                                {
                                    if (column.Key != "SettlementOff")
                                    {
                                        column.Hidden = true;
                                    }
                                }
                            }
                        }
                    }
                    grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Hidden = false;
                    grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
                    grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 1;
                    grdHoliday.DisplayLayout.Bands[0].Columns["MarketOff"].Header.VisiblePosition = 2;
                    grdHoliday.DisplayLayout.Bands[0].Columns["SettlementOff"].Header.VisiblePosition = 3;
                    grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                    grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                }
                else
                {
                    Holiday holiday = new Holiday(int.MinValue, int.MinValue, "", Prana.BusinessObjects.DateTimeConstants.MinValue);
                    Holidays nullHolidays = new Holidays();
                    nullHolidays.Add(holiday);
                    grdHoliday.DataSource = nullHolidays;

                    ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "StringDate")
                        {
                            if (column.Key != "Description")
                            {
                                // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                                if (column.Key != "MarketOff")
                                {
                                    if (column.Key != "SettlementOff")
                                    {
                                        column.Hidden = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!grdHoliday.DisplayLayout.Bands[0].Columns.Exists("StringDate"))
                    {
                        grdHoliday.DisplayLayout.Bands[0].Columns.Add("StringDate", "Date");
                        grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                        grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                    }
                    else
                    {
                        grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                        grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                        grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                        grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {

            _calendars.Clear();
            _calendars = AUECManager.GetCalendar();
            BindYear();

        }

        private void btnNewCountry_Click(object sender, EventArgs e)
        {
            CountryForm frmCountry = CountryForm.GetInstance();
            frmCountry.Show();
        }

        private void btnNewState_Click(object sender, EventArgs e)
        {
            StateForm frmState = StateForm.GetInstance();
            frmState.Show();
        }

        /// <summary>
        /// To initialize audit control for AUEC.
        /// </summary>
        private void InitializeAUECAudit(int AUECID)
        {
            try
            {
                ctrlAUECAudit.Location = new System.Drawing.Point(7, 895);
                ctrlAUECAudit.Dock = DockStyle.Fill;
                ultraTabPageControl3.Controls.Add(ctrlAUECAudit);
                ctrlAUECAudit.InitializeControl(AUECID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool isCalculationFeeBasisCyclic(List<OtherFeeRule> allOtherFeeRules, bool isLong)
        {
            try
            {
                int key = 0;
                int feeCount = Enum.GetNames(typeof(OtherFeeType)).Length;
                int[] degree = new int[feeCount];
                List<int>[] adjList = new List<int>[feeCount];
                for (int i = 0; i < feeCount; i++)
                    adjList[i] = new List<int>();
                for (int i = 0; i < allOtherFeeRules.Count; i++)
                {
                    if (allOtherFeeRules[i].IsCriteriaApplied)
                        continue;
                    if (isLong)
                        key = (int)allOtherFeeRules[i].LongCalculationBasis - (int)CalculationFeeBasis.StampDuty;
                    else
                        key = (int)allOtherFeeRules[i].ShortCalculationBasis - (int)CalculationFeeBasis.StampDuty;
                    if (key >= 0)
                    {
                        degree[(int)allOtherFeeRules[i].OtherFeeType]++;
                        adjList[key].Add((int)allOtherFeeRules[i].OtherFeeType);
                    }
                }
                Queue<int> q = new Queue<int>();
                for (int i = 0; i < feeCount; i++)
                {
                    if (degree[i] == 0)
                    {
                        q.Enqueue(i);
                    }
                }
                while (q.Count > 0)
                {
                    int feeid = q.Dequeue();
                    for (int i = 0; i < adjList[feeid].Count; i++)
                    {
                        if (--degree[adjList[feeid][i]] == 0)
                            q.Enqueue(adjList[feeid][i]);
                    }
                    feeCount--;
                }
                if (feeCount == 0)
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }
    }
}
