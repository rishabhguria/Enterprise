using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Interfaces;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.StringUtilities;
using System.Collections.Generic;
using Prana.CommonDataCache;
using System.Data;
using Prana.WCFConnectionMgr;
using System.Configuration;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.Allocation.Common.Definitions;
using Prana.Utilities.UIUtilities;


namespace Prana.AllocationNew
{
    /// <summary>
    /// Summary description for AllocationPreferences.
    /// </summary>
    public class AllocationPreferencesUserControl : System.Windows.Forms.UserControl, IPreferencesSavedClicked
    {

        #region Forms Elements
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tlbPreferences;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditor8;
        private Infragistics.Win.Misc.UltraLabel Auto;
        private Infragistics.Win.Misc.UltraLabel label9;
        private Infragistics.Win.Misc.UltraLabel label12;
        private Infragistics.Win.Misc.UltraLabel label13;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxVenue;
        private Infragistics.Win.Misc.UltraLabel label5;
        private Infragistics.Win.Misc.UltraLabel label6;
        //private Infragistics.Win.Misc.UltraLabel lblIntegrateAccountStrategy;
        private Infragistics.Win.Misc.UltraLabel lblAvgPricing;
        //private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxIntegrateAccountStrategy;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxAvgPricing;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxRoundLot;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorExeLessTotalText;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorUnAllocateText;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorExeLessTotalBack;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorUnAllocateBack;
        private Infragistics.Win.Misc.UltraLabel lblTradingAcc;
        private Infragistics.Win.Misc.UltraLabel lblBuyBCV;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradingAcc;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxBuyBCV;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxCounterParty;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeDate;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxProcessDate;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes1;
        //private Infragistics.Win.Misc.UltraButton btnAddNewStrategy;
        //private Infragistics.Win.UltraWinGrid.UltraGrid grdDefaultStrategies;
        private Infragistics.Win.Misc.UltraLabel label3;
        private IContainer components;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateEqualTotalBack;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateEqualTotalText;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateLessTotalText;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateLessTotalBack;
        //private bool isInitialized;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private Prana.BusinessObjects.CompanyUser _loginUser;
        private Infragistics.Win.Misc.UltraLabel label8;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorSelectedRowTextColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorSelectedRowBackColor;
        private Infragistics.Win.Misc.UltraGroupBox groupBox2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        private AccountStrategyMappingUserCtrlNew accountStrategyMappingUserCtrlNew1;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private UltraCombo cmbbxdefaults;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtbxDefaultName;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private AllocationColumns accountColumns;
        #endregion
        AllocationDefaultCollection _defaults = new AllocationDefaultCollection();
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private ErrorProvider errorProvider;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.Misc.UltraButton btnDeleteScheme;
        private Infragistics.Win.Misc.UltraButton btnEditScheme;
        private ListBox lstSchemes;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.Misc.UltraGroupBox grpDefaultAllocation;
        private Infragistics.Win.Misc.UltraLabel lblDefaultAllocation;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoAccountSymbol;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkClearAllocationAccountControlNumber;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAllocationMethodologyRevertToAccount;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAllocateBasedonLatestPositions;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAllocateEditPrefrences;
        private Infragistics.Win.Misc.UltraLabel ultralblDisableCheckSideAssets;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbDisableCheckSide;
        private Infragistics.Win.Misc.UltraLabel ultralblAssetsWithCommissionInNetAmount;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraCmbAssetsWithCommissionInNetAmount;
        private Infragistics.Win.Misc.UltraLabel ultralblSetPrecisionValue;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumSetPrecisionValue;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabAttributes;
        private Prana.AllocationNew.Allocation.UI.Preferences.CtrlAttributeRename ctrlAttributeRename2;
        private Infragistics.Win.Misc.UltraLabel lblProcessDate;
        private Infragistics.Win.Misc.UltraLabel lblTradeDate;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxIsPariPassuAllocation;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxAllocateExtraShare;
        private UltraCombo cmbAccounts;
        // modified by omshiv, Jan 2014- added new control and tab for master fund ration allocation scheme
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private Allocation.UI.Preferences.CtrlMasterFundRatio ctrlMasterFundRatio;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.Misc.UltraGroupBox ultraGrpDefaultRule;
        private Allocation.UI.AllocationDefaultRuleControl allocationDefaultRuleControl1;
        private Allocation.UI.UserControls.AllocationPrefMainControl allocationPrefMainControl1;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute6;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes6;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute5;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes5;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes4;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute3;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes3;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradeAttributes2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxIncludeSavewtState;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxIncludeSavewtoutState;
        //private Infragistics.Win.Misc.UltraLabel lblError;
        //private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        //private Prana.AllocationNew.Allocation.UI.Preferences.AccountingMethodsUsrCtrl accountingMethodsUsrCtrl1;





        #region Private Members

        private AllocationPreferences _allocationPreferences;


        #endregion


        #region Constructors

        public AllocationPreferencesUserControl()
        {
            //
            // Required for Windows Form Designer support
            //

            InitializeComponent();



        }
        public void SetUpDefaultsUI()
        {
            try
            {
                StrategyCollection strategies = CachedDataManager.GetInstance.GetUserStrategies();
                AccountCollection accounts = CachedDataManager.GetInstance.GetUserAccounts();
                AccountCollection _accounts = new AccountCollection();
                foreach (Account account in accounts)
                {
                    if (account.AccountID != int.MinValue)
                    {
                        _accounts.Add(account);
                    }
                }
                accountStrategyMappingUserCtrlNew1.SetUp(_accounts, strategies, true);
                //accountStrategyMappingUserCtrlNew1.Dock = DockStyle.Fill;
                accountStrategyMappingUserCtrlNew1.SetSelectionStatus(true);
                ctrlAttributeRename2.IntializeControl();
                ctrlMasterFundRatio.IntializeControl(_allocationPreferences.GeneralRules.isMasterFundRatioAllocation);

                // addd a default to create new
                btnAdd_Click(null, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            try
            {
                _loginUser = user;
                AllocationPreferencesManager.SetUp(_loginUser.CompanyUserID);

                LoadPreferences();
                SetUpDefaultsUI();
                //SetUpAccountingMethodsUI();
                SetPreferences();
                cmbbxdefaults.Value = int.MinValue;
                //txtbxDefaultName.Visible = false;
                CreateAllocationServicesProxy();
                BindAllocationSchemeList();
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (this._allocationServices != null)
                {
                    this._allocationServices.Dispose();
                }
                UnwireEvents();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///unbind events 
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                allocationPrefMainControl1.IsAllocationPrefTabSelected -= new EventHandler(allocationPrefMainControl1_IsAllocationPrefTabSelected);
                //AllocationPreferencesManager.NoOfMasterFund -= new EventHandler(allocationPrefManager_NoOfMasterFund);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.accountColumns = new Prana.AllocationNew.AllocationColumns();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGrpDefaultRule = new Infragistics.Win.Misc.UltraGroupBox();
            this.allocationDefaultRuleControl1 = new Prana.AllocationNew.Allocation.UI.AllocationDefaultRuleControl();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAccounts = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.chkBoxAllocateExtraShare = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxIsPariPassuAllocation = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAllocateBasedonLatestPositions = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAllocateEditPrefrences = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAllocationMethodologyRevertToAccount = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkClearAllocationAccountControlNumber = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraCmbDisableCheckSide = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultralblDisableCheckSideAssets = new Infragistics.Win.Misc.UltraLabel();
            this.ultralblSetPrecisionValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraNumSetPrecisionValue = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultralblAssetsWithCommissionInNetAmount=new Infragistics.Win.Misc.UltraLabel();
            this.ultraCmbAssetsWithCommissionInNetAmount = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.groupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkbxRoundLot = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblAvgPricing = new Infragistics.Win.Misc.UltraLabel();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxAvgPricing = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage4 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.label8 = new Infragistics.Win.Misc.UltraLabel();
            this.ColorSelectedRowTextColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorSelectedRowBackColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ColorAllocateLessTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.ColorAllocateLessTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label12 = new Infragistics.Win.Misc.UltraLabel();
            this.ColorAllocateEqualTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorAllocateEqualTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label9 = new Infragistics.Win.Misc.UltraLabel();
            this.label13 = new Infragistics.Win.Misc.UltraLabel();
            this.ColorUnAllocateBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorUnAllocateText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorExeLessTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorExeLessTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label6 = new Infragistics.Win.Misc.UltraLabel();
            this.label5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblProcessDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeDate = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxCounterParty = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxTradeDate = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxProcessDate = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.Auto = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditor8 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradingAcc = new Infragistics.Win.Misc.UltraLabel();
            this.lblBuyBCV = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradingAcc = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxBuyBCV = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxVenue = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute6 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes6 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute5 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes5 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute4 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes4 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute3 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes3 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute2 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes2 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradeAttribute1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkbxTradeAttributes1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.txtbxDefaultName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.cmbbxdefaults = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.accountStrategyMappingUserCtrlNew1 = new Prana.AllocationNew.AccountStrategyMappingUserCtrlNew();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lstSchemes = new System.Windows.Forms.ListBox();
            this.btnDeleteScheme = new Infragistics.Win.Misc.UltraButton();
            this.btnEditScheme = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpDefaultAllocation = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblDefaultAllocation = new Infragistics.Win.Misc.UltraLabel();
            this.uoAccountSymbol = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.tabAttributes = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlAttributeRename2 = new Prana.AllocationNew.Allocation.UI.Preferences.CtrlAttributeRename();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlMasterFundRatio = new Prana.AllocationNew.Allocation.UI.Preferences.CtrlMasterFundRatio();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.allocationPrefMainControl1 = new Prana.AllocationNew.Allocation.UI.UserControls.AllocationPrefMainControl();
            this.tlbPreferences = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkbxIncludeSavewtState = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxIncludeSavewtoutState = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraTabPageControl10.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpDefaultRule)).BeginInit();
            this.ultraGrpDefaultRule.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxAllocateExtraShare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxIsPariPassuAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocateBasedonLatestPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocateEditPrefrences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocationMethodologyRevertToAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClearAllocationAccountControlNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxIncludeSavewtState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxIncludeSavewtoutState)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxRoundLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxAvgPricing)).BeginInit();
            this.ultraTabPageControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).BeginInit();
            this.ultraTabControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowTextColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowBackColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalText)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxProcessDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradingAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxBuyBCV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes1)).BeginInit();
            this.ultraTabPageControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxdefaults)).BeginInit();
            this.accountStrategyMappingUserCtrlNew1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl7.SuspendLayout();
            this.grpDefaultAllocation.SuspendLayout();
            this.tabAttributes.SuspendLayout();
            this.ultraTabPageControl8.SuspendLayout();
            this.ultraTabPageControl12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlbPreferences)).BeginInit();
            this.tlbPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoAccountSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.accountColumns);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(850, 321);
            // 
            // accountColumns
            // 
            this.accountColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.accountColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountColumns.Location = new System.Drawing.Point(0, 0);
            this.accountColumns.Name = "accountColumns";
            this.accountColumns.Size = new System.Drawing.Size(850, 321);
            this.accountColumns.TabIndex = 1;
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(850, 321);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraGroupBox3);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(852, 342);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox3.Controls.Add(this.ultraGrpDefaultRule);
            this.ultraGroupBox3.Controls.Add(this.groupBox1);
            this.ultraGroupBox3.Controls.Add(this.groupBox2);
            this.ultraGroupBox3.Location = new System.Drawing.Point(8, 12);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(834, 333);
            this.ultraGroupBox3.TabIndex = 0;
            // 
            // ultraGrpDefaultRule
            // 
            this.ultraGrpDefaultRule.Controls.Add(this.allocationDefaultRuleControl1);
            this.ultraGrpDefaultRule.Location = new System.Drawing.Point(435, 22);
            this.ultraGrpDefaultRule.Name = "ultraGrpDefaultRule";
            this.ultraGrpDefaultRule.Size = new System.Drawing.Size(385, 247);
            this.ultraGrpDefaultRule.TabIndex = 2;
            this.ultraGrpDefaultRule.Text = "Default Rule";
            // 
            // allocationDefaultRuleControl1
            // 
            this.allocationDefaultRuleControl1.AutoScroll = true;
            this.allocationDefaultRuleControl1.AutoSize = true;
            this.allocationDefaultRuleControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.allocationDefaultRuleControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationDefaultRuleControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allocationDefaultRuleControl1.Location = new System.Drawing.Point(3, 17);
            this.allocationDefaultRuleControl1.Margin = new System.Windows.Forms.Padding(4);
            this.allocationDefaultRuleControl1.Name = "allocationDefaultRuleControl1";
            this.allocationDefaultRuleControl1.Size = new System.Drawing.Size(379, 227);
            this.allocationDefaultRuleControl1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbAccounts);
            this.groupBox1.Controls.Add(this.chkBoxAllocateExtraShare);
            this.groupBox1.Controls.Add(this.chkBoxIsPariPassuAllocation);
            this.groupBox1.Controls.Add(this.chkAllocateBasedonLatestPositions);
            this.groupBox1.Controls.Add(this.chkAllocateEditPrefrences);
            this.groupBox1.Controls.Add(this.chkAllocationMethodologyRevertToAccount);
            this.groupBox1.Controls.Add(this.chkClearAllocationAccountControlNumber);
            this.groupBox1.Controls.Add(this.ultralblDisableCheckSideAssets);
            this.groupBox1.Controls.Add(this.ultraCmbDisableCheckSide);
            this.groupBox1.Controls.Add(this.ultralblSetPrecisionValue);
            this.groupBox1.Controls.Add(this.ultraNumSetPrecisionValue);
            this.groupBox1.Controls.Add(this.ultralblAssetsWithCommissionInNetAmount);
            this.groupBox1.Controls.Add(this.ultraCmbAssetsWithCommissionInNetAmount);
            this.groupBox1.Controls.Add(this.chkbxIncludeSavewtState);
            this.groupBox1.Controls.Add(this.chkbxIncludeSavewtoutState);
            this.groupBox1.Location = new System.Drawing.Point(21, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 250);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // cmbAccounts
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAccounts.DisplayLayout.Appearance = appearance1;
            this.cmbAccounts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbAccounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccounts.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAccounts.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAccounts.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAccounts.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAccounts.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccounts.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbAccounts.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAccounts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbAccounts.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbAccounts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAccounts.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAccounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccounts.Location = new System.Drawing.Point(191, 84);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(100, 23);
            this.cmbAccounts.TabIndex = 5;
            this.cmbAccounts.Visible = false;
            // 
            // chkBoxAllocateExtraShare
            // 
            appearance13.FontData.BoldAsString = "False";
            appearance13.FontData.Name = "Tahoma";
            appearance13.FontData.SizeInPoints = 8F;
            this.chkBoxAllocateExtraShare.Appearance = appearance13;
            this.chkBoxAllocateExtraShare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkBoxAllocateExtraShare.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkBoxAllocateExtraShare.Location = new System.Drawing.Point(47, 83);
            this.chkBoxAllocateExtraShare.Name = "chkBoxAllocateExtraShare";
            this.chkBoxAllocateExtraShare.Size = new System.Drawing.Size(143, 24);
            this.chkBoxAllocateExtraShare.TabIndex = 4;
            this.chkBoxAllocateExtraShare.Text = "Allocate Extra Share to:";
            this.chkBoxAllocateExtraShare.Visible = false;
            this.chkBoxAllocateExtraShare.CheckedChanged += new System.EventHandler(this.chkBoxAllocateExtraShare_CheckedChanged);
            this.chkBoxAllocateExtraShare.MouseHover += new System.EventHandler(this.chkBoxAllocateExtraShare_MouseHover);
            // 
            // chkBoxIsPariPassuAllocation
            // 
            appearance14.FontData.BoldAsString = "False";
            appearance14.FontData.Name = "Tahoma";
            appearance14.FontData.SizeInPoints = 8F;
            this.chkBoxIsPariPassuAllocation.Appearance = appearance14;
            this.chkBoxIsPariPassuAllocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkBoxIsPariPassuAllocation.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkBoxIsPariPassuAllocation.Location = new System.Drawing.Point(47, 110);
            this.chkBoxIsPariPassuAllocation.Name = "chkBoxIsPariPassuAllocation";
            this.chkBoxIsPariPassuAllocation.Size = new System.Drawing.Size(341, 22);
            this.chkBoxIsPariPassuAllocation.TabIndex = 3;
            this.chkBoxIsPariPassuAllocation.Text = "Ensure Pari-Passu Allocation";
            this.chkBoxIsPariPassuAllocation.Visible = false;
            this.chkBoxIsPariPassuAllocation.CheckedChanged += new System.EventHandler(this.chkBoxIsPariPassuAllocation_CheckedChanged);
            // 
            // chkAllocateEditPrefrences
            // 
            appearance62.FontData.BoldAsString = "False";
            appearance62.FontData.Name = "Tahoma";
            appearance62.FontData.SizeInPoints = 8F;
            this.chkAllocateEditPrefrences.Appearance = appearance62;
            this.chkAllocateEditPrefrences.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocateEditPrefrences.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocateEditPrefrences.Location = new System.Drawing.Point(47, 120);
            this.chkAllocateEditPrefrences.Name = "chkAllocateEditPrefrences";
            this.chkAllocateEditPrefrences.Size = new System.Drawing.Size(326, 15);
            this.chkAllocateEditPrefrences.TabIndex = 3;
            this.chkAllocateEditPrefrences.Text = "Edit Preferences during Allocation";
            // 
            // chkAllocateBasedonLatestPositions
            // 
            appearance15.FontData.BoldAsString = "False";
            appearance15.FontData.Name = "Tahoma";
            appearance15.FontData.SizeInPoints = 8F;
            this.chkAllocateBasedonLatestPositions.Appearance = appearance15;
            this.chkAllocateBasedonLatestPositions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocateBasedonLatestPositions.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocateBasedonLatestPositions.Location = new System.Drawing.Point(47, 94);
            this.chkAllocateBasedonLatestPositions.Name = "chkAllocateBasedonLatestPositions";
            this.chkAllocateBasedonLatestPositions.Size = new System.Drawing.Size(326, 26);
            this.chkAllocateBasedonLatestPositions.TabIndex = 2;
            this.chkAllocateBasedonLatestPositions.Text = "Auto Allocate Closing Transactions to avoid boxed Positions";
            this.chkAllocateBasedonLatestPositions.CheckedChanged += new System.EventHandler(this.chkAllocateBasedonLatestPositions_CheckedChanged);
            // 
            // chkAllocationMethodologyRevertToAccount
            // 
            appearance16.FontData.BoldAsString = "False";
            appearance16.FontData.Name = "Tahoma";
            appearance16.FontData.SizeInPoints = 8F;
            this.chkAllocationMethodologyRevertToAccount.Appearance = appearance16;
            this.chkAllocationMethodologyRevertToAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocationMethodologyRevertToAccount.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkAllocationMethodologyRevertToAccount.Location = new System.Drawing.Point(47, 77);
            this.chkAllocationMethodologyRevertToAccount.Name = "chkAllocationMethodologyRevertToAccount";
            this.chkAllocationMethodologyRevertToAccount.Size = new System.Drawing.Size(244, 15);
            this.chkAllocationMethodologyRevertToAccount.TabIndex = 1;
            this.chkAllocationMethodologyRevertToAccount.Text = "Allocation Methodology default  back to Account";
            // 
            // chkClearAllocationAccountControlNumber
            // 
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance17.FontData.BoldAsString = "False";
            appearance17.FontData.Name = "Tahoma";
            appearance17.FontData.SizeInPoints = 8F;
            this.chkClearAllocationAccountControlNumber.Appearance = appearance17;
            this.chkClearAllocationAccountControlNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkClearAllocationAccountControlNumber.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkClearAllocationAccountControlNumber.Location = new System.Drawing.Point(47, 58);
            this.chkClearAllocationAccountControlNumber.Name = "chkClearAllocationAccountControlNumber";
            this.chkClearAllocationAccountControlNumber.Size = new System.Drawing.Size(222, 15);
            this.chkClearAllocationAccountControlNumber.TabIndex = 0;
            this.chkClearAllocationAccountControlNumber.Text = "Clear Allocation Qty and % Numbers";
            //
            // chkbxIncludeSavewtState
            // 
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance17.FontData.BoldAsString = "False";
            appearance17.FontData.Name = "Tahoma";
            appearance17.FontData.SizeInPoints = 8F;
            this.chkbxIncludeSavewtState.Appearance = appearance17;
            this.chkbxIncludeSavewtState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIncludeSavewtState.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIncludeSavewtState.Location = new System.Drawing.Point(47, 40);
            this.chkbxIncludeSavewtState.Name = "chkbxIncludeSavewtState";
            this.chkbxIncludeSavewtState.Size = new System.Drawing.Size(222, 15);
            this.chkbxIncludeSavewtState.TabIndex = 0;
            this.chkbxIncludeSavewtState.Text = "Include Save With State";
            // 
            // chkbxIncludeSavewtoutState
            // 
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance17.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance17.FontData.BoldAsString = "False";
            appearance17.FontData.Name = "Tahoma";
            appearance17.FontData.SizeInPoints = 8F;
            this.chkbxIncludeSavewtoutState.Appearance = appearance17;
            this.chkbxIncludeSavewtoutState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIncludeSavewtoutState.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIncludeSavewtoutState.Location = new System.Drawing.Point(47, 22);
            this.chkbxIncludeSavewtoutState.Name = "chkbxIncludeSavewtoutState";
            this.chkbxIncludeSavewtoutState.Size = new System.Drawing.Size(222, 15);
            this.chkbxIncludeSavewtoutState.TabIndex = 0;
            this.chkbxIncludeSavewtoutState.Text = "Include Save Without State";
            //
            //ultralblDisableCheckSideAssets
            //
            this.ultralblDisableCheckSideAssets.Appearance = appearance17;
            this.ultralblDisableCheckSideAssets.Name = "ultralblDisableCheckSideAssets";
            this.ultralblDisableCheckSideAssets.TabIndex = 4;
            this.ultralblDisableCheckSideAssets.Text = "Disable checkside for Assets";
            this.ultralblDisableCheckSideAssets.Size = new System.Drawing.Size(170, 35);
            this.ultralblDisableCheckSideAssets.Location = new System.Drawing.Point(47, 145);
            //
            //ultraCmbDisableCheckSide
            //
            this.ultraCmbDisableCheckSide.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraCmbDisableCheckSide.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraCmbDisableCheckSide.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraCmbDisableCheckSide.CheckedListSettings.ListSeparator = ",";
            this.ultraCmbDisableCheckSide.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbDisableCheckSide.Location = new System.Drawing.Point(230,145);
            this.ultraCmbDisableCheckSide.Name = "ultraCmbDisableCheckSide";
            this.ultraCmbDisableCheckSide.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbDisableCheckSide.TabIndex = 20;
            // 
            //ultralblSetPrecisionValue
            //
            this.ultralblSetPrecisionValue.Appearance = appearance17;
            this.ultralblSetPrecisionValue.Name = "ultralblSetPrecisionValue";
            this.ultralblSetPrecisionValue.TabIndex = 5;
            this.ultralblSetPrecisionValue.Text = "Set Precision Value";
            this.ultralblSetPrecisionValue.Size = new System.Drawing.Size(170, 35);
            this.ultralblSetPrecisionValue.Location = new System.Drawing.Point(47, 180);
            //
            //ultraNumSetPrecisionValue
            //
            this.ultraNumSetPrecisionValue.Location = new System.Drawing.Point(230, 180);
            this.ultraNumSetPrecisionValue.Name = "ultraNumSetPrecisionValue";
            this.ultraNumSetPrecisionValue.Size = new System.Drawing.Size(168, 25);
            this.ultraNumSetPrecisionValue.TabIndex = 21;
            this.ultraNumSetPrecisionValue.PromptChar=' ';
            this.ultraNumSetPrecisionValue.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            //
            //ultralblAssetsWithOnlyCommissionsInNetAmount
            //
            this.ultralblAssetsWithCommissionInNetAmount.Appearance = appearance17;
            this.ultralblAssetsWithCommissionInNetAmount.Name = "ultralblAssetsWithOnlyCommissionsInNetAmount";
            this.ultralblAssetsWithCommissionInNetAmount.TabIndex = 6;
            this.ultralblAssetsWithCommissionInNetAmount.Text = "Use Commission in Net Amount";
            this.ultralblAssetsWithCommissionInNetAmount.Size = new System.Drawing.Size(170, 40);
            this.ultralblAssetsWithCommissionInNetAmount.Location = new System.Drawing.Point(47,210);
            //
            //ultraCmbAssetsWithOnlyCommissionsInNetAmount
            //
            this.ultraCmbAssetsWithCommissionInNetAmount.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraCmbAssetsWithCommissionInNetAmount.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraCmbAssetsWithCommissionInNetAmount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraCmbAssetsWithCommissionInNetAmount.CheckedListSettings.ListSeparator = ",";
            this.ultraCmbAssetsWithCommissionInNetAmount.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraCmbAssetsWithCommissionInNetAmount.Location = new System.Drawing.Point(230, 215);
            this.ultraCmbAssetsWithCommissionInNetAmount.Name = "ultraCmbAssetsWithCommissionsInNetAmount";
            this.ultraCmbAssetsWithCommissionInNetAmount.Size = new System.Drawing.Size(168, 25);
            this.ultraCmbAssetsWithCommissionInNetAmount.TabIndex = 20;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkbxRoundLot);
            this.groupBox2.Controls.Add(this.lblAvgPricing);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkbxAvgPricing);
            this.groupBox2.Location = new System.Drawing.Point(21, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(408, 67);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rules";
            // 
            // chkbxRoundLot
            // 
            this.chkbxRoundLot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxRoundLot.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxRoundLot.Enabled = false;
            this.chkbxRoundLot.Location = new System.Drawing.Point(47, 21);
            this.chkbxRoundLot.Name = "chkbxRoundLot";
            this.chkbxRoundLot.Size = new System.Drawing.Size(14, 13);
            this.chkbxRoundLot.TabIndex = 0;
            this.chkbxRoundLot.Text = "ultraCheckEditor1";
            // 
            // lblAvgPricing
            // 
            this.lblAvgPricing.AutoSize = true;
            this.lblAvgPricing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblAvgPricing.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAvgPricing.Location = new System.Drawing.Point(75, 40);
            this.lblAvgPricing.Name = "lblAvgPricing";
            this.lblAvgPricing.Size = new System.Drawing.Size(122, 13);
            this.lblAvgPricing.TabIndex = 3;
            this.lblAvgPricing.Text = "Average Pricing Allowed";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(75, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Apply Round Lot Rules";
            // 
            // chkbxAvgPricing
            // 
            this.chkbxAvgPricing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxAvgPricing.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxAvgPricing.Enabled = false;
            this.chkbxAvgPricing.Location = new System.Drawing.Point(47, 41);
            this.chkbxAvgPricing.Name = "chkbxAvgPricing";
            this.chkbxAvgPricing.Size = new System.Drawing.Size(14, 13);
            this.chkbxAvgPricing.TabIndex = 2;
            this.chkbxAvgPricing.Text = "ultraCheckEditor2";
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.ultraTabControl2);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(852, 342);
            // 
            // ultraTabControl2
            // 
            this.ultraTabControl2.ActiveTabAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance18.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl2.Appearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl2.ClientAreaAppearance = appearance19;
            this.ultraTabControl2.Controls.Add(this.ultraTabSharedControlsPage4);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl10);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl11);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl9);
            this.ultraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl2.Name = "ultraTabControl2";
            this.ultraTabControl2.SharedControlsPage = this.ultraTabSharedControlsPage4;
            this.ultraTabControl2.Size = new System.Drawing.Size(852, 342);
            this.ultraTabControl2.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabControl2.TabIndex = 8;
            ultraTab1.TabPage = this.ultraTabPageControl10;
            ultraTab1.Text = "AllocationAccount Columns";
            ultraTab2.TabPage = this.ultraTabPageControl9;
            this.ultraTabControl2.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl2.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage4
            // 
            this.ultraTabSharedControlsPage4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage4.Name = "ultraTabSharedControlsPage4";
            this.ultraTabSharedControlsPage4.Size = new System.Drawing.Size(850, 321);
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Location = new System.Drawing.Point(0, 0);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(200, 100);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.label8);
            this.ultraTabPageControl3.Controls.Add(this.ColorSelectedRowTextColor);
            this.ultraTabPageControl3.Controls.Add(this.ColorSelectedRowBackColor);
            this.ultraTabPageControl3.Controls.Add(this.ultraGroupBox2);
            this.ultraTabPageControl3.Controls.Add(this.ultraGroupBox1);
            this.ultraTabPageControl3.Controls.Add(this.label6);
            this.ultraTabPageControl3.Controls.Add(this.label5);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(852, 350);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(36, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 19);
            this.label8.TabIndex = 24;
            this.label8.Text = "Selected Row Color";
            // 
            // ColorSelectedRowTextColor
            // 
            this.ColorSelectedRowTextColor.AllowEmpty = false;
            this.ColorSelectedRowTextColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance20.BorderColor = System.Drawing.Color.Black;
            appearance20.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorSelectedRowTextColor.Appearance = appearance20;
            this.ColorSelectedRowTextColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowTextColor.ButtonAppearance = appearance21;
            this.ColorSelectedRowTextColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorSelectedRowTextColor.Color = System.Drawing.Color.Black;
            this.ColorSelectedRowTextColor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorSelectedRowTextColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorSelectedRowTextColor.Location = new System.Drawing.Point(358, 227);
            this.ColorSelectedRowTextColor.Name = "ColorSelectedRowTextColor";
            this.ColorSelectedRowTextColor.Size = new System.Drawing.Size(115, 20);
            this.ColorSelectedRowTextColor.TabIndex = 25;
            this.ColorSelectedRowTextColor.Text = "Black";
            this.ColorSelectedRowTextColor.UseAppStyling = false;
            this.ColorSelectedRowTextColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorSelectedRowBackColor
            // 
            this.ColorSelectedRowBackColor.AllowEmpty = false;
            this.ColorSelectedRowBackColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance22.BorderColor = System.Drawing.Color.Black;
            appearance22.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorSelectedRowBackColor.Appearance = appearance22;
            this.ColorSelectedRowBackColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowBackColor.ButtonAppearance = appearance23;
            this.ColorSelectedRowBackColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorSelectedRowBackColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowBackColor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorSelectedRowBackColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorSelectedRowBackColor.Location = new System.Drawing.Point(214, 227);
            this.ColorSelectedRowBackColor.Name = "ColorSelectedRowBackColor";
            this.ColorSelectedRowBackColor.Size = new System.Drawing.Size(115, 20);
            this.ColorSelectedRowBackColor.TabIndex = 23;
            this.ColorSelectedRowBackColor.Text = "192, 255, 192";
            this.ColorSelectedRowBackColor.UseAppStyling = false;
            this.ColorSelectedRowBackColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateLessTotalText);
            this.ultraGroupBox2.Controls.Add(this.label2);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateLessTotalBack);
            this.ultraGroupBox2.Controls.Add(this.label12);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateEqualTotalText);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateEqualTotalBack);
            this.ultraGroupBox2.Location = new System.Drawing.Point(3, 115);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(520, 81);
            this.ultraGroupBox2.TabIndex = 22;
            this.ultraGroupBox2.Text = "Allocated Row Colors";
            // 
            // ColorAllocateLessTotalText
            // 
            this.ColorAllocateLessTotalText.AllowEmpty = false;
            this.ColorAllocateLessTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance24.BorderColor = System.Drawing.Color.Black;
            appearance24.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateLessTotalText.Appearance = appearance24;
            this.ColorAllocateLessTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalText.ButtonAppearance = appearance25;
            this.ColorAllocateLessTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateLessTotalText.Color = System.Drawing.Color.Black;
            this.ColorAllocateLessTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateLessTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateLessTotalText.Location = new System.Drawing.Point(356, 39);
            this.ColorAllocateLessTotalText.Name = "ColorAllocateLessTotalText";
            this.ColorAllocateLessTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateLessTotalText.TabIndex = 20;
            this.ColorAllocateLessTotalText.Text = "Black";
            this.ColorAllocateLessTotalText.UseAppStyling = false;
            this.ColorAllocateLessTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(36, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 19);
            this.label2.TabIndex = 19;
            this.label2.Text = "Allocated < Quantity";
            // 
            // ColorAllocateLessTotalBack
            // 
            this.ColorAllocateLessTotalBack.AllowEmpty = false;
            this.ColorAllocateLessTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateLessTotalBack.Appearance = appearance26;
            this.ColorAllocateLessTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalBack.ButtonAppearance = appearance27;
            this.ColorAllocateLessTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateLessTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateLessTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateLessTotalBack.Location = new System.Drawing.Point(212, 41);
            this.ColorAllocateLessTotalBack.Name = "ColorAllocateLessTotalBack";
            this.ColorAllocateLessTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateLessTotalBack.TabIndex = 18;
            this.ColorAllocateLessTotalBack.Text = "255, 192, 192";
            this.ColorAllocateLessTotalBack.UseAppStyling = false;
            this.ColorAllocateLessTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(36, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 19);
            this.label12.TabIndex = 9;
            this.label12.Text = "AllocatedQty=Quantity";
            // 
            // ColorAllocateEqualTotalText
            // 
            this.ColorAllocateEqualTotalText.AllowEmpty = false;
            this.ColorAllocateEqualTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance28.BorderColor = System.Drawing.Color.Black;
            appearance28.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateEqualTotalText.Appearance = appearance28;
            this.ColorAllocateEqualTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalText.ButtonAppearance = appearance29;
            this.ColorAllocateEqualTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateEqualTotalText.Color = System.Drawing.Color.Black;
            this.ColorAllocateEqualTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateEqualTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateEqualTotalText.Location = new System.Drawing.Point(356, 17);
            this.ColorAllocateEqualTotalText.Name = "ColorAllocateEqualTotalText";
            this.ColorAllocateEqualTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateEqualTotalText.TabIndex = 16;
            this.ColorAllocateEqualTotalText.Text = "Black";
            this.ColorAllocateEqualTotalText.UseAppStyling = false;
            this.ColorAllocateEqualTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorAllocateEqualTotalBack
            // 
            this.ColorAllocateEqualTotalBack.AllowEmpty = false;
            this.ColorAllocateEqualTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance30.BorderColor = System.Drawing.Color.Black;
            appearance30.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateEqualTotalBack.Appearance = appearance30;
            this.ColorAllocateEqualTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalBack.ButtonAppearance = appearance31;
            this.ColorAllocateEqualTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateEqualTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateEqualTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateEqualTotalBack.Location = new System.Drawing.Point(212, 17);
            this.ColorAllocateEqualTotalBack.Name = "ColorAllocateEqualTotalBack";
            this.ColorAllocateEqualTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateEqualTotalBack.TabIndex = 3;
            this.ColorAllocateEqualTotalBack.Text = "192, 255, 192";
            this.ColorAllocateEqualTotalBack.UseAppStyling = false;
            this.ColorAllocateEqualTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.label9);
            this.ultraGroupBox1.Controls.Add(this.label13);
            this.ultraGroupBox1.Controls.Add(this.ColorUnAllocateBack);
            this.ultraGroupBox1.Controls.Add(this.ColorUnAllocateText);
            this.ultraGroupBox1.Controls.Add(this.ColorExeLessTotalBack);
            this.ultraGroupBox1.Controls.Add(this.ColorExeLessTotalText);
            this.ultraGroupBox1.Location = new System.Drawing.Point(2, 18);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(520, 80);
            this.ultraGroupBox1.TabIndex = 21;
            this.ultraGroupBox1.Text = "UnAllocated Row Colors";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(34, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(144, 19);
            this.label9.TabIndex = 6;
            this.label9.Text = "CumQty =Quantity";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(34, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 19);
            this.label13.TabIndex = 10;
            this.label13.Text = "CumQty < Quantity";
            // 
            // ColorUnAllocateBack
            // 
            this.ColorUnAllocateBack.AllowEmpty = false;
            this.ColorUnAllocateBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance32.BorderColor = System.Drawing.Color.Black;
            appearance32.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorUnAllocateBack.Appearance = appearance32;
            this.ColorUnAllocateBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            appearance33.BorderColor = System.Drawing.Color.Black;
            this.ColorUnAllocateBack.ButtonAppearance = appearance33;
            this.ColorUnAllocateBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorUnAllocateBack.Color = System.Drawing.SystemColors.ActiveCaption;
            this.ColorUnAllocateBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorUnAllocateBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorUnAllocateBack.Location = new System.Drawing.Point(212, 18);
            this.ColorUnAllocateBack.Name = "ColorUnAllocateBack";
            this.ColorUnAllocateBack.Size = new System.Drawing.Size(115, 20);
            this.ColorUnAllocateBack.TabIndex = 0;
            this.ColorUnAllocateBack.Text = "ActiveCaption";
            this.ColorUnAllocateBack.UseAppStyling = false;
            this.ColorUnAllocateBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorUnAllocateText
            // 
            this.ColorUnAllocateText.AllowEmpty = false;
            this.ColorUnAllocateText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance34.BorderColor = System.Drawing.Color.Black;
            appearance34.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorUnAllocateText.Appearance = appearance34;
            this.ColorUnAllocateText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorUnAllocateText.ButtonAppearance = appearance35;
            this.ColorUnAllocateText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorUnAllocateText.Color = System.Drawing.Color.Black;
            this.ColorUnAllocateText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorUnAllocateText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorUnAllocateText.Location = new System.Drawing.Point(356, 20);
            this.ColorUnAllocateText.Name = "ColorUnAllocateText";
            this.ColorUnAllocateText.Size = new System.Drawing.Size(115, 20);
            this.ColorUnAllocateText.TabIndex = 13;
            this.ColorUnAllocateText.Text = "Black";
            this.ColorUnAllocateText.UseAppStyling = false;
            this.ColorUnAllocateText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorExeLessTotalBack
            // 
            this.ColorExeLessTotalBack.AllowEmpty = false;
            this.ColorExeLessTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance36.BorderColor = System.Drawing.Color.Black;
            appearance36.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorExeLessTotalBack.Appearance = appearance36;
            this.ColorExeLessTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalBack.ButtonAppearance = appearance37;
            this.ColorExeLessTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorExeLessTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorExeLessTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorExeLessTotalBack.Location = new System.Drawing.Point(212, 42);
            this.ColorExeLessTotalBack.Name = "ColorExeLessTotalBack";
            this.ColorExeLessTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorExeLessTotalBack.TabIndex = 4;
            this.ColorExeLessTotalBack.Text = "255, 192, 192";
            this.ColorExeLessTotalBack.UseAppStyling = false;
            this.ColorExeLessTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorExeLessTotalText
            // 
            this.ColorExeLessTotalText.AllowEmpty = false;
            this.ColorExeLessTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance38.BorderColor = System.Drawing.Color.Black;
            appearance38.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorExeLessTotalText.Appearance = appearance38;
            this.ColorExeLessTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalText.ButtonAppearance = appearance39;
            this.ColorExeLessTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorExeLessTotalText.Color = System.Drawing.Color.Black;
            this.ColorExeLessTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorExeLessTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorExeLessTotalText.Location = new System.Drawing.Point(356, 44);
            this.ColorExeLessTotalText.Name = "ColorExeLessTotalText";
            this.ColorExeLessTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorExeLessTotalText.TabIndex = 17;
            this.ColorExeLessTotalText.Text = "Black";
            this.ColorExeLessTotalText.UseAppStyling = false;
            this.ColorExeLessTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(358, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "TextColor";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(214, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "RowColor";
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.ultraGroupBox4);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(852, 342);
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Controls.Add(this.lblProcessDate);
            this.ultraGroupBox4.Controls.Add(this.lblTradeDate);
            this.ultraGroupBox4.Controls.Add(this.chkbxCounterParty);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeDate);
            this.ultraGroupBox4.Controls.Add(this.chkbxProcessDate);
            this.ultraGroupBox4.Controls.Add(this.label3);
            this.ultraGroupBox4.Controls.Add(this.Auto);
            this.ultraGroupBox4.Controls.Add(this.ultraCheckEditor8);
            this.ultraGroupBox4.Controls.Add(this.lblTradingAcc);
            this.ultraGroupBox4.Controls.Add(this.lblBuyBCV);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradingAcc);
            this.ultraGroupBox4.Controls.Add(this.label4);
            this.ultraGroupBox4.Controls.Add(this.chkbxBuyBCV);
            this.ultraGroupBox4.Controls.Add(this.chkbxVenue);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute6);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes6);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute5);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes5);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute4);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes4);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute3);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes3);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute2);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes2);
            this.ultraGroupBox4.Controls.Add(this.lblTradeAttribute1);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradeAttributes1);
            this.ultraGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox4.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(852, 342);
            this.ultraGroupBox4.TabIndex = 15;
            // 
            // lblProcessDate
            // 
            this.lblProcessDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblProcessDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblProcessDate.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblProcessDate.Location = new System.Drawing.Point(541, 126);
            this.lblProcessDate.Name = "lblProcessDate";
            this.lblProcessDate.Size = new System.Drawing.Size(106, 13);
            this.lblProcessDate.TabIndex = 18;
            this.lblProcessDate.Text = "And Process Date";
            // 
            // lblTradeDate
            // 
            this.lblTradeDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeDate.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeDate.Location = new System.Drawing.Point(541, 94);
            this.lblTradeDate.Name = "lblTradeDate";
            this.lblTradeDate.Size = new System.Drawing.Size(95, 13);
            this.lblTradeDate.TabIndex = 17;
            this.lblTradeDate.Text = "And Trade Date";
            // 
            // chkbxCounterParty
            // 
            this.chkbxCounterParty.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxCounterParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxCounterParty.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxCounterParty.Location = new System.Drawing.Point(264, 99);
            this.chkbxCounterParty.Name = "chkbxCounterParty";
            this.chkbxCounterParty.Size = new System.Drawing.Size(14, 14);
            this.chkbxCounterParty.TabIndex = 14;
            this.chkbxCounterParty.Text = "ultraCheckEditor6";
            // 
            // chkbxTradeDate
            // 
            this.chkbxTradeDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeDate.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeDate.Location = new System.Drawing.Point(491, 94);
            this.chkbxTradeDate.Name = "chkbxTradeDate";
            this.chkbxTradeDate.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeDate.TabIndex = 14;
            this.chkbxTradeDate.Text = "ultraCheckEditor6";
            this.chkbxTradeDate.CheckedChanged += new System.EventHandler(this.chkbxTradeDate_CheckedChanged);
            // 
            // chkbxProcessDate
            // 
            this.chkbxProcessDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxProcessDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxProcessDate.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxProcessDate.Location = new System.Drawing.Point(491, 129);
            this.chkbxProcessDate.Name = "chkbxProcessDate";
            this.chkbxProcessDate.Size = new System.Drawing.Size(14, 14);
            this.chkbxProcessDate.TabIndex = 14;
            this.chkbxProcessDate.Text = "ultraCheckEditor6";
            this.chkbxProcessDate.CheckedChanged += new System.EventHandler(this.chkbxProcessDate_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label3.Location = new System.Drawing.Point(314, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = ApplicationConstants.CONST_BROKER;
            // 
            // Auto
            // 
            this.Auto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Auto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Auto.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Auto.Location = new System.Drawing.Point(265, 36);
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(335, 31);
            this.Auto.TabIndex = 12;
			//CHMW-3149	[Foreign Positions Settling in Base Currency] Handle grouping/ungrouping for settlement fields
            this.Auto.Text = "Auto AllocationGroup Orders with Same Side, Same Symbol and Same Settlement Currency";
            // 
            // ultraCheckEditor8
            // 
            this.ultraCheckEditor8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ultraCheckEditor8.Checked = true;
            this.ultraCheckEditor8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ultraCheckEditor8.Enabled = false;
            this.ultraCheckEditor8.Location = new System.Drawing.Point(213, 37);
            this.ultraCheckEditor8.Name = "ultraCheckEditor8";
            this.ultraCheckEditor8.Size = new System.Drawing.Size(16, 20);
            this.ultraCheckEditor8.TabIndex = 11;
            this.ultraCheckEditor8.Text = "ultraCheckEditor8";
            // 
            // lblTradingAcc
            // 
            this.lblTradingAcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradingAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradingAcc.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradingAcc.Location = new System.Drawing.Point(541, 159);
            this.lblTradingAcc.Name = "lblTradingAcc";
            this.lblTradingAcc.Size = new System.Drawing.Size(146, 23);
            this.lblTradingAcc.TabIndex = 9;
            this.lblTradingAcc.Text = "And Same Trading A/c";
            // 
            // lblBuyBCV
            // 
            this.lblBuyBCV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBuyBCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblBuyBCV.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblBuyBCV.Location = new System.Drawing.Point(314, 159);
            this.lblBuyBCV.Name = "lblBuyBCV";
            this.lblBuyBCV.Size = new System.Drawing.Size(146, 23);
            this.lblBuyBCV.TabIndex = 8;
            this.lblBuyBCV.Text = "And Buy + BCV";
            this.lblBuyBCV.Visible = false;
            // 
            // chkbxTradingAcc
            // 
            this.chkbxTradingAcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradingAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradingAcc.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradingAcc.Location = new System.Drawing.Point(491, 163);
            this.chkbxTradingAcc.Name = "chkbxTradingAcc";
            this.chkbxTradingAcc.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradingAcc.TabIndex = 5;
            this.chkbxTradingAcc.Text = "ultraCheckEditor4";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(314, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "And Same Venue";
            // 
            // chkbxBuyBCV
            // 
            this.chkbxBuyBCV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxBuyBCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxBuyBCV.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxBuyBCV.Location = new System.Drawing.Point(264, 163);
            this.chkbxBuyBCV.Name = "chkbxBuyBCV";
            this.chkbxBuyBCV.Size = new System.Drawing.Size(14, 14);
            this.chkbxBuyBCV.TabIndex = 4;
            this.chkbxBuyBCV.Text = "ultraCheckEditor5";
            this.chkbxBuyBCV.Visible = false;                      
            // 
            // chkbxVenue
            // 
            this.chkbxVenue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxVenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxVenue.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxVenue.Location = new System.Drawing.Point(264, 129);
            this.chkbxVenue.Name = "chkbxVenue";
            this.chkbxVenue.Size = new System.Drawing.Size(14, 14);
            this.chkbxVenue.TabIndex = 3;
            this.chkbxVenue.Text = "ultraCheckEditor6";
            // 
            // lblTradeAttribute6
            // 
            this.lblTradeAttribute6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute6.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute6.Location = new System.Drawing.Point(314, 159);
            this.lblTradeAttribute6.Name = "lblTradeAttribute6";
            this.lblTradeAttribute6.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute6.TabIndex = 17;
            this.lblTradeAttribute6.Tag = "TradeAttribute6";
            this.lblTradeAttribute6.Text = "And Trade Attributes 6";
            // 
            // chkbxTradeAttributes6
            // 
            this.chkbxTradeAttributes6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes6.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            //this.chkbxTradeAttributes6.Location = new System.Drawing.Point(491, 256);
            this.chkbxTradeAttributes6.Location = new System.Drawing.Point(264, 163);               
            this.chkbxTradeAttributes6.Name = "chkbxTradeAttributes6";
            this.chkbxTradeAttributes6.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes6.TabIndex = 14;
            this.chkbxTradeAttributes6.Text = "And Trade Date";
            // 
            // lblTradeAttribute5
            // 
            this.lblTradeAttribute5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute5.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute5.Location = new System.Drawing.Point(314, 255);
            this.lblTradeAttribute5.Name = "lblTradeAttribute5";
            this.lblTradeAttribute5.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute5.TabIndex = 17;
            this.lblTradeAttribute5.Tag = "TradeAttribute5";
            this.lblTradeAttribute5.Text = "And Trade Attributes 5";
            // 
            // chkbxTradeAttributes5
            // 
            this.chkbxTradeAttributes5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes5.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes5.Location = new System.Drawing.Point(264, 255);
            this.chkbxTradeAttributes5.Name = "chkbxTradeAttributes5";
            this.chkbxTradeAttributes5.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes5.TabIndex = 14;
            this.chkbxTradeAttributes5.Text = "And Trade Date";
            // 
            // lblTradeAttribute4
            // 
            this.lblTradeAttribute4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute4.Location = new System.Drawing.Point(541, 224);
            this.lblTradeAttribute4.Name = "lblTradeAttribute4";
            this.lblTradeAttribute4.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute4.TabIndex = 17;
            this.lblTradeAttribute4.Tag = "TradeAttribute4";
            this.lblTradeAttribute4.Text = "And Trade Attributes 4";
            // 
            // chkbxTradeAttributes4
            // 
            this.chkbxTradeAttributes4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes4.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes4.Location = new System.Drawing.Point(491, 224);
            this.chkbxTradeAttributes4.Name = "chkbxTradeAttributes4";
            this.chkbxTradeAttributes4.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes4.TabIndex = 14;
            this.chkbxTradeAttributes4.Text = "And Trade Date";
            // 
            // lblTradeAttribute3
            // 
            this.lblTradeAttribute3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute3.Location = new System.Drawing.Point(314, 224);
            this.lblTradeAttribute3.Name = "lblTradeAttribute3";
            this.lblTradeAttribute3.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute3.TabIndex = 17;
            this.lblTradeAttribute3.Tag = "TradeAttribute3";
            this.lblTradeAttribute3.Text = "And Trade Attributes 3";
            // 
            // chkbxTradeAttributes3
            // 
            this.chkbxTradeAttributes3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes3.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes3.Location = new System.Drawing.Point(264, 224);
            this.chkbxTradeAttributes3.Name = "chkbxTradeAttributes3";
            this.chkbxTradeAttributes3.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes3.TabIndex = 14;
            this.chkbxTradeAttributes3.Text = "And Trade Date";
            // 
            // lblTradeAttribute2
            // 
            this.lblTradeAttribute2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute2.Location = new System.Drawing.Point(541, 195);
            this.lblTradeAttribute2.Name = "lblTradeAttribute2";
            this.lblTradeAttribute2.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute2.TabIndex = 17;
            this.lblTradeAttribute2.Tag = "TradeAttribute2";
            this.lblTradeAttribute2.Text = "And Trade Attributes 2";
            // 
            // chkbxTradeAttributes2
            // 
            this.chkbxTradeAttributes2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes2.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes2.Location = new System.Drawing.Point(491, 195);
            this.chkbxTradeAttributes2.Name = "chkbxTradeAttributes2";
            this.chkbxTradeAttributes2.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes2.TabIndex = 14;
            this.chkbxTradeAttributes2.Text = "And Trade Date";
            // 
            // lblTradeAttribute1
            // 
            this.lblTradeAttribute1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradeAttribute1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradeAttribute1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradeAttribute1.Location = new System.Drawing.Point(314, 194);
            this.lblTradeAttribute1.Name = "lblTradeAttribute1";
            this.lblTradeAttribute1.Size = new System.Drawing.Size(130, 13);
            this.lblTradeAttribute1.TabIndex = 17;
            this.lblTradeAttribute1.Tag = "TradeAttribute1";
            this.lblTradeAttribute1.Text = "And Trade Attributes 1";
            // 
            // chkbxTradeAttributes1
            // 
            this.chkbxTradeAttributes1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradeAttributes1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradeAttributes1.Location = new System.Drawing.Point(264, 194);
            this.chkbxTradeAttributes1.Name = "chkbxTradeAttributes1";
            this.chkbxTradeAttributes1.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradeAttributes1.TabIndex = 14;
            this.chkbxTradeAttributes1.Text = "And Trade Date";
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.btnSave);
            this.ultraTabPageControl5.Controls.Add(this.txtbxDefaultName);
            this.ultraTabPageControl5.Controls.Add(this.btnDelete);
            this.ultraTabPageControl5.Controls.Add(this.btnAdd);
            this.ultraTabPageControl5.Controls.Add(this.cmbbxdefaults);
            this.ultraTabPageControl5.Controls.Add(this.accountStrategyMappingUserCtrlNew1);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(852, 342);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.Moccasin;
            this.btnSave.Location = new System.Drawing.Point(302, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 24);
            this.btnSave.TabIndex = 113;
            this.btnSave.Text = "SaveDefault";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtbxDefaultName
            // 
            this.txtbxDefaultName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtbxDefaultName.Location = new System.Drawing.Point(277, 3);
            this.txtbxDefaultName.Name = "txtbxDefaultName";
            this.txtbxDefaultName.Size = new System.Drawing.Size(100, 21);
            this.txtbxDefaultName.TabIndex = 112;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.Moccasin;
            this.btnDelete.Location = new System.Drawing.Point(226, 315);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 24);
            this.btnDelete.TabIndex = 109;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAdd.BackColor = System.Drawing.Color.Moccasin;
            this.btnAdd.Location = new System.Drawing.Point(149, 315);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 24);
            this.btnAdd.TabIndex = 107;
            this.btnAdd.Text = "AddNew";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbbxdefaults
            // 
            this.cmbbxdefaults.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance40.BackColor = System.Drawing.SystemColors.Window;
            appearance40.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxdefaults.DisplayLayout.Appearance = appearance40;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxdefaults.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxdefaults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxdefaults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbbxdefaults.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance41.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance41.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance41.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.Appearance = appearance41;
            appearance42.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.BandLabelAppearance = appearance42;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance43.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance43.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxdefaults.DisplayLayout.GroupByBox.PromptAppearance = appearance43;
            this.cmbbxdefaults.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxdefaults.DisplayLayout.MaxRowScrollRegions = 1;
            appearance44.BackColor = System.Drawing.SystemColors.Window;
            appearance44.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxdefaults.DisplayLayout.Override.ActiveCellAppearance = appearance44;
            appearance45.BackColor = System.Drawing.SystemColors.Highlight;
            appearance45.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxdefaults.DisplayLayout.Override.ActiveRowAppearance = appearance45;
            this.cmbbxdefaults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxdefaults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance46.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.Override.CardAreaAppearance = appearance46;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            appearance47.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxdefaults.DisplayLayout.Override.CellAppearance = appearance47;
            this.cmbbxdefaults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxdefaults.DisplayLayout.Override.CellPadding = 0;
            appearance48.BackColor = System.Drawing.SystemColors.Control;
            appearance48.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance48.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance48.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxdefaults.DisplayLayout.Override.GroupByRowAppearance = appearance48;
            appearance49.TextHAlignAsString = "Left";
            this.cmbbxdefaults.DisplayLayout.Override.HeaderAppearance = appearance49;
            this.cmbbxdefaults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxdefaults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance50.BackColor = System.Drawing.SystemColors.Window;
            appearance50.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxdefaults.DisplayLayout.Override.RowAppearance = appearance50;
            this.cmbbxdefaults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance51.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxdefaults.DisplayLayout.Override.TemplateAddRowAppearance = appearance51;
            this.cmbbxdefaults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxdefaults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxdefaults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxdefaults.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxdefaults.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxdefaults.Location = new System.Drawing.Point(191, 3);
            this.cmbbxdefaults.Name = "cmbbxdefaults";
            this.cmbbxdefaults.Size = new System.Drawing.Size(65, 21);
            this.cmbbxdefaults.TabIndex = 110;
            this.cmbbxdefaults.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxdefaults.ValueChanged += new System.EventHandler(this.cmbbxdefaults_ValueChanged);
            // 
            // accountStrategyMappingUserCtrlNew1
            // 
            this.accountStrategyMappingUserCtrlNew1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accountStrategyMappingUserCtrlNew1.AutoScroll = true;
            this.accountStrategyMappingUserCtrlNew1.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.accountStrategyMappingUserCtrlNew1.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.accountStrategyMappingUserCtrlNew1.Location = new System.Drawing.Point(6, 30);
            this.accountStrategyMappingUserCtrlNew1.Name = "accountStrategyMappingUserCtrlNew1";
            this.accountStrategyMappingUserCtrlNew1.Size = new System.Drawing.Size(517, 279);
            this.accountStrategyMappingUserCtrlNew1.TabIndex = 105;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.lblMessage);
            this.ultraTabPageControl2.Controls.Add(this.lstSchemes);
            this.ultraTabPageControl2.Controls.Add(this.btnDeleteScheme);
            this.ultraTabPageControl2.Controls.Add(this.btnEditScheme);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(852, 342);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(16, 276);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(214, 13);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "*Double Click to open and edit the scheme.";
            // 
            // lstSchemes
            // 
            this.lstSchemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSchemes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSchemes.ColumnWidth = 200;
            this.lstSchemes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSchemes.FormattingEnabled = true;
            this.lstSchemes.Location = new System.Drawing.Point(19, 22);
            this.lstSchemes.MultiColumn = true;
            this.lstSchemes.Name = "lstSchemes";
            this.lstSchemes.ScrollAlwaysVisible = true;
            this.lstSchemes.Size = new System.Drawing.Size(487, 249);
            this.lstSchemes.Sorted = true;
            this.lstSchemes.TabIndex = 0;
            this.lstSchemes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSchemes_MouseDoubleClick);
            // 
            // btnDeleteScheme
            // 
            this.btnDeleteScheme.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDeleteScheme.Location = new System.Drawing.Point(271, 297);
            this.btnDeleteScheme.Name = "btnDeleteScheme";
            this.btnDeleteScheme.Size = new System.Drawing.Size(81, 23);
            this.btnDeleteScheme.TabIndex = 2;
            this.btnDeleteScheme.Text = "Delete";
            this.btnDeleteScheme.Click += new System.EventHandler(this.btnDeleteScheme_Click);
            // 
            // btnEditScheme
            // 
            this.btnEditScheme.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEditScheme.Location = new System.Drawing.Point(171, 297);
            this.btnEditScheme.Name = "btnEditScheme";
            this.btnEditScheme.Size = new System.Drawing.Size(82, 23);
            this.btnEditScheme.TabIndex = 1;
            this.btnEditScheme.Text = "Edit";
            this.btnEditScheme.Click += new System.EventHandler(this.btnEditScheme_Click);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Controls.Add(this.grpDefaultAllocation);
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(852, 342);
            // 
            // grpDefaultAllocation
            // 
            this.grpDefaultAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDefaultAllocation.Controls.Add(this.lblDefaultAllocation);
            this.grpDefaultAllocation.Controls.Add(this.uoAccountSymbol);
            this.grpDefaultAllocation.Location = new System.Drawing.Point(3, 3);
            this.grpDefaultAllocation.Name = "grpDefaultAllocation";
            this.grpDefaultAllocation.Size = new System.Drawing.Size(516, 336);
            this.grpDefaultAllocation.TabIndex = 0;
            this.grpDefaultAllocation.TabStop = false;
            // 
            // lblDefaultAllocation
            // 
            this.lblDefaultAllocation.AutoSize = true;
            this.lblDefaultAllocation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultAllocation.Location = new System.Drawing.Point(108, 54);
            this.lblDefaultAllocation.Name = "lblDefaultAllocation";
            this.lblDefaultAllocation.Size = new System.Drawing.Size(80, 13);
            this.lblDefaultAllocation.TabIndex = 0;
            this.lblDefaultAllocation.Text = "Allocation By";
            // 
            // rdbSymbol
            // 
            //this.rdbSymbol.AutoSize = true;
            //this.rdbSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //this.rdbSymbol.Location = new System.Drawing.Point(288, 52);
            //this.rdbSymbol.Name = "rdbSymbol";
            //this.rdbSymbol.Size = new System.Drawing.Size(59, 17);
            //this.rdbSymbol.TabIndex = 2;
            //this.rdbSymbol.Text = "Symbol";
            //this.rdbSymbol.UseVisualStyleBackColor = true;
            //
            //uoSymbolAccount
            //
            this.uoAccountSymbol.CheckedIndex = 0;
            valueListItem1.DataValue = "Account";
            valueListItem1.DisplayText = "Account";
            valueListItem2.DataValue = "Symbol";
            valueListItem2.DisplayText = "Symbol";
            this.uoAccountSymbol.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.uoAccountSymbol.Location = new System.Drawing.Point(218, 52);
            this.uoAccountSymbol.Name = "uoNewOrder";
            this.uoAccountSymbol.Size = new System.Drawing.Size(130, 17);
            this.uoAccountSymbol.TabIndex = 33;
            this.uoAccountSymbol.Text = "Symbol";
            // 
            // tabAttributes
            // 
            this.tabAttributes.Controls.Add(this.ctrlAttributeRename2);
            this.tabAttributes.Location = new System.Drawing.Point(-10000, -10000);
            this.tabAttributes.Name = "tabAttributes";
            this.tabAttributes.Size = new System.Drawing.Size(852, 342);
            this.tabAttributes.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl8_Paint);
            // 
            // ctrlAttributeRename2
            //
            if (!CustomThemeHelper.ApplyTheme)
            {
                this.ctrlAttributeRename2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            else
            {
                this.ctrlAttributeRename2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            }
            this.ctrlAttributeRename2.Location = new System.Drawing.Point(18, 19);
            this.ctrlAttributeRename2.Name = "ctrlAttributeRename2";
            this.ctrlAttributeRename2.Size = new System.Drawing.Size(501, 283);
            this.ctrlAttributeRename2.TabIndex = 0;
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Controls.Add(this.ctrlMasterFundRatio);
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(852, 342);
            // 
            // ctrlMasterFundRatio
            // 
            if (!CustomThemeHelper.ApplyTheme)
            {
                this.ctrlMasterFundRatio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            else
            {
                this.ctrlMasterFundRatio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            }
            this.ctrlMasterFundRatio.Location = new System.Drawing.Point(23, 18);
            this.ctrlMasterFundRatio.Name = "ctrlMasterFundRatio";
            this.ctrlMasterFundRatio.Size = new System.Drawing.Size(482, 321);
            this.ctrlMasterFundRatio.TabIndex = 3;
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Controls.Add(this.allocationPrefMainControl1);
            this.ultraTabPageControl12.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(852, 342);
            // 
            // allocationPrefMainControl1
            // 
            this.allocationPrefMainControl1.AutoScroll = true;
            this.allocationPrefMainControl1.AutoSize = true;
            this.allocationPrefMainControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationPrefMainControl1.Location = new System.Drawing.Point(0, 0);
            this.allocationPrefMainControl1.Name = "allocationPrefMainControl1";
            this.allocationPrefMainControl1.Size = new System.Drawing.Size(852, 342);
            this.allocationPrefMainControl1.TabIndex = 0;
            // 
            // tlbPreferences
            // 
            appearance52.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance52.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance52.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tlbPreferences.ActiveTabAppearance = appearance52;
            appearance53.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.tlbPreferences.Appearance = appearance53;
            this.tlbPreferences.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.tlbPreferences.ClientAreaAppearance = appearance54;
            this.tlbPreferences.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl1);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl3);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl4);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl5);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl6);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl2);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl7);
            this.tlbPreferences.Controls.Add(this.tabAttributes);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl8);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl12);
            this.tlbPreferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbPreferences.Location = new System.Drawing.Point(0, 0);
            this.tlbPreferences.Name = "tlbPreferences";
            this.tlbPreferences.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tlbPreferences.Size = new System.Drawing.Size(854, 380);
            this.tlbPreferences.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tlbPreferences.TabIndex = 0;
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance55.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab6.Appearance = appearance55;
            ultraTab6.TabPage = this.ultraTabPageControl1;
            ultraTab6.Text = "General";
            ultraTab7.TabPage = this.ultraTabPageControl6;
            ultraTab7.Text = "Columns";
            ultraTab7.Visible = false;
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab8.Appearance = appearance56;
            ultraTab8.TabPage = this.ultraTabPageControl3;
            ultraTab8.Text = "Colors";
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab9.Appearance = appearance57;
            ultraTab9.TabPage = this.ultraTabPageControl4;
            ultraTab9.Text = "Auto Grouping Rules";
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance58.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab10.Appearance = appearance58;
            ultraTab10.TabPage = this.ultraTabPageControl5;
            ultraTab10.Text = "Allocation By Account";
            ultraTab10.Visible = false;
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab4.Appearance = appearance59;
            ultraTab4.TabPage = this.ultraTabPageControl2;
            ultraTab4.Text = "Allocation By Symbol";
            if (!CustomThemeHelper.ApplyTheme)
            {
            appearance60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab3.Appearance = appearance60;
            ultraTab3.TabPage = this.ultraTabPageControl7;
            ultraTab3.Text = "Default Allocation";
            if (!CustomThemeHelper.ApplyTheme)
            {
                appearance61.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            }
            ultraTab5.Appearance = appearance61;
            ultraTab5.Key = "tabAttribute";
            ultraTab5.TabPage = this.tabAttributes;
            ultraTab5.Text = "Attribute Renaming";
            ultraTab11.TabPage = this.ultraTabPageControl8;
            ultraTab11.Text = "Master Fund Ratio Allocation";
            ultraTab12.Key = "tabAllocationPref";
            ultraTab12.TabPage = this.ultraTabPageControl12;
            ultraTab12.Text = "Allocation Preferences";
            this.tlbPreferences.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab4,
            ultraTab3,
            ultraTab5,
            ultraTab11,
            ultraTab12});
            this.tlbPreferences.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            this.tlbPreferences.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tlbPreferences_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.AutoScroll = true;
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(852, 342);
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // AllocationPreferencesUserControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.tlbPreferences);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AllocationPreferencesUserControl";
            this.Size = new System.Drawing.Size(854, 363);
            this.Load += new System.EventHandler(this.AllocationPreferencesUserControl_Load);
            this.ultraTabPageControl10.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrpDefaultRule)).EndInit();
            this.ultraGrpDefaultRule.ResumeLayout(false);
            this.ultraGrpDefaultRule.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxAllocateExtraShare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxIsPariPassuAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocateBasedonLatestPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocateEditPrefrences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAllocationMethodologyRevertToAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClearAllocationAccountControlNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxIncludeSavewtState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxIncludeSavewtoutState)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxRoundLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxAvgPricing)).EndInit();
            this.ultraTabPageControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).EndInit();
            this.ultraTabControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowTextColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowBackColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalText)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkbxCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxProcessDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradingAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxBuyBCV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxTradeAttributes1)).EndInit();
            this.ultraTabPageControl5.ResumeLayout(false);
            this.ultraTabPageControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxdefaults)).EndInit();
            this.accountStrategyMappingUserCtrlNew1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            this.ultraTabPageControl7.ResumeLayout(false);
            this.grpDefaultAllocation.ResumeLayout(false);
            this.grpDefaultAllocation.PerformLayout();
            this.tabAttributes.ResumeLayout(false);
            this.ultraTabPageControl8.ResumeLayout(false);
            this.ultraTabPageControl12.ResumeLayout(false);
            this.ultraTabPageControl12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uoAccountSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbPreferences)).EndInit();
            this.tlbPreferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Preferences
        private void SetPreferences()
        {
            try
            {
                if (_allocationPreferences == null) return;

                #region General Rules

                if (_allocationPreferences.GeneralRules.ApplyRoundLotRules)
                {
                    chkbxRoundLot.Checked = true;
                }
                else
                {
                    chkbxRoundLot.Checked = false;

                }
                if (_allocationPreferences.GeneralRules.AveragePricingAllowed)
                {
                    chkbxAvgPricing.Checked = true;
                }
                else
                {
                    chkbxAvgPricing.Checked = false;
                }

                if (_allocationPreferences.GeneralRules.ClearAllocationAccountControlNumer)
                {
                    chkClearAllocationAccountControlNumber.Checked = true;
                }
                else
                {
                    chkClearAllocationAccountControlNumber.Checked = false;
                }

                //Include Save With State checkbox

                if (_allocationPreferences.GeneralRules.IncludeSavewtState)
                {
                    chkbxIncludeSavewtState.Checked = true;
                }
                else
                {
                    chkbxIncludeSavewtState.Checked = false;
                }
                //Include Save Without State checkbox
                if (_allocationPreferences.GeneralRules.IncludeSavewtoutState)
                {
                    chkbxIncludeSavewtoutState.Checked = true;
                }
                else
                {
                    chkbxIncludeSavewtoutState.Checked = false;
                }

                if (_allocationPreferences.GeneralRules.AllocationMethodologyRevertToAccount)
                {
                    chkAllocationMethodologyRevertToAccount.Checked = true;
                }
                else
                {
                    chkAllocationMethodologyRevertToAccount.Checked = false;
                }

                if (chkAllocateBasedonLatestPositions.Checked != _allocationPreferences.GeneralRules.AllocateBasedonOpenPositions)
                {
                    if (_allocationPreferences.GeneralRules.AllocateBasedonOpenPositions)
                    {
                        chkAllocateBasedonLatestPositions.Checked = true;
                    }
                    else
                    {
                        chkAllocateBasedonLatestPositions.Checked = false;
                    }
                }

                if (_allocationPreferences.GeneralRules.IsPariPassuAllocation)
                {
                    chkBoxIsPariPassuAllocation.Checked = true;
                }
                else
                {
                    chkBoxIsPariPassuAllocation.Checked = false;
                }

                if (chkBoxAllocateExtraShare.Checked != _allocationPreferences.GeneralRules.AllocateExtraShareToSelectedAccount)
                {
                    if (_allocationPreferences.GeneralRules.AllocateExtraShareToSelectedAccount)
                    {
                        chkBoxAllocateExtraShare.Checked = true;
                    }
                    else
                    {
                        chkBoxAllocateExtraShare.Checked = false;
                    }
                }


                BindAccounts();

                if (cmbAccounts.DataSource != null && _allocationPreferences.GeneralRules.SelectedAccountID != int.MinValue && _allocationPreferences.GeneralRules.SelectedAccountID != -1)
                {
                    cmbAccounts.Value = _allocationPreferences.GeneralRules.SelectedAccountID;
                }
                //if(_allocationPreferences.GeneralRules.IntegrateAccountAndStrategyBundling)
                //{
                //    chkbxIntegrateAccountStrategy.Checked=true;
                //}
                //else
                //{
                //    chkbxIntegrateAccountStrategy.Checked=false;

                //}




                #endregion

              

                #region Colors
                ColorUnAllocateBack.Color = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                ColorUnAllocateBack.Appearance.BackColor = ColorUnAllocateBack.Color;
                ColorUnAllocateBack.Appearance.ForeColor = ColorUnAllocateBack.Color;
                ColorUnAllocateBack.Appearance.BorderColor = ColorUnAllocateBack.Color;
                //ColorUnAllocateBack.Text=" ";

                ColorUnAllocateText.Color = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor);
                ColorUnAllocateText.Appearance.BackColor = ColorUnAllocateText.Color;
                ColorUnAllocateText.Appearance.ForeColor = ColorUnAllocateText.Color;
                ColorUnAllocateText.Appearance.BorderColor = ColorUnAllocateText.Color;
                //ColorUnAllocateText.Text=" ";



                ColorAllocateEqualTotalBack.Color = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
                ColorAllocateEqualTotalBack.Appearance.BackColor = ColorAllocateEqualTotalBack.Color;
                ColorAllocateEqualTotalBack.Appearance.ForeColor = ColorAllocateEqualTotalBack.Color;
                ColorAllocateEqualTotalBack.Appearance.BorderColor = ColorAllocateEqualTotalBack.Color;
                //ColorAllocateEqualTotalBack.Text =" ";

                ColorAllocateEqualTotalText.Color = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor);
                ColorAllocateEqualTotalText.Appearance.BackColor = ColorAllocateEqualTotalText.Color;
                ColorAllocateEqualTotalText.Appearance.ForeColor = ColorAllocateEqualTotalText.Color;
                ColorAllocateEqualTotalText.Appearance.BorderColor = ColorAllocateEqualTotalText.Color;
                //ColorAllocateEqualTotalText.Text=" ";

                ColorExeLessTotalBack.Color = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
                ColorExeLessTotalBack.Appearance.BackColor = ColorExeLessTotalBack.Color;
                ColorExeLessTotalBack.Appearance.ForeColor = ColorExeLessTotalBack.Color;
                ColorExeLessTotalBack.Appearance.BorderColor = ColorExeLessTotalBack.Color;
                //ColorExeLessTotalBack.Text=" ";

                ColorExeLessTotalText.Color = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor);
                ColorExeLessTotalText.Appearance.BackColor = ColorExeLessTotalText.Color;
                ColorExeLessTotalText.Appearance.ForeColor = ColorExeLessTotalText.Color;
                ColorExeLessTotalText.Appearance.BorderColor = ColorExeLessTotalText.Color;
                //ColorExeLessTotalText.Text=" ";
                ColorAllocateLessTotalText.Color = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor);
                ColorAllocateLessTotalText.Appearance.BackColor = ColorAllocateLessTotalText.Color;
                ColorAllocateLessTotalText.Appearance.ForeColor = ColorAllocateLessTotalText.Color;
                ColorAllocateLessTotalText.Appearance.BorderColor = ColorAllocateLessTotalText.Color;

                ColorAllocateLessTotalBack.Color = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor);
                ColorAllocateLessTotalBack.Appearance.BackColor = ColorAllocateLessTotalBack.Color;
                ColorAllocateLessTotalBack.Appearance.ForeColor = ColorAllocateLessTotalBack.Color;
                ColorAllocateLessTotalBack.Appearance.BorderColor = ColorAllocateLessTotalBack.Color;

                ColorSelectedRowBackColor.Color = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                ColorSelectedRowBackColor.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                ColorSelectedRowBackColor.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                ColorSelectedRowBackColor.Appearance.BorderColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);

                ColorSelectedRowTextColor.Color = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                ColorSelectedRowTextColor.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                ColorSelectedRowTextColor.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                ColorSelectedRowTextColor.Appearance.BorderColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);

                #endregion

                //isInitialized = true;

                #region AutoGrouping Rules
                if (_allocationPreferences.AutoGroupingRules.CounterParty)
                    chkbxCounterParty.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes1)
                    chkbxTradeAttributes1.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes2)
                    chkbxTradeAttributes2.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes3)
                    chkbxTradeAttributes3.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes4)
                    chkbxTradeAttributes4.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes5)
                    chkbxTradeAttributes5.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradeAttributes6)
                    chkbxTradeAttributes6.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.Venue)
                    chkbxVenue.Checked = true;
                if (_allocationPreferences.AutoGroupingRules.BuyAndBCV)
                    chkbxBuyBCV.Checked = true;

                if (_allocationPreferences.AutoGroupingRules.TradingAccount)
                    chkbxTradingAcc.Checked = true;
                if (_allocationPreferences.AutoGroupingRules.TradeDate)
                    chkbxTradeDate.Checked = true;
                if (_allocationPreferences.AutoGroupingRules.ProcessDate)
                    chkbxProcessDate.Checked = true;



                #endregion

                #region Columns
                accountColumns.SetUp(_allocationPreferences, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                // strategyColumns.SetUp(_allocationPreferences, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
                #endregion

                #region AllocationByAccountOrSymbol

                if (_allocationPreferences.AllocationByAccountOrSymbol.Account)
                    uoAccountSymbol.CheckedIndex = 0;

                if (_allocationPreferences.AllocationByAccountOrSymbol.Symbol)
                    uoAccountSymbol.CheckedIndex = 1;

                #endregion

                BindDefaults();

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void BindDefaults()
        {
            try
            {
                cmbbxdefaults.DataSource = null;
                cmbbxdefaults.DataSource = _defaults.GetDefaultsDataTable();
                cmbbxdefaults.DataBind();
                cmbbxdefaults.DisplayMember = "Name";
                cmbbxdefaults.ValueMember = "ID";
                cmbbxdefaults.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                // cmbbxdefaults.Value = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindAccounts()
        {
            Dictionary<int, string> dictionaryAccounts = CachedDataManager.GetInstance.GetAccounts();
            List<EnumerationValue> values = new List<EnumerationValue>();
            EnumerationValue valueSelect = new EnumerationValue("-Select-", -1);
            values.Add(valueSelect);

            foreach (KeyValuePair<int, string> kp in dictionaryAccounts)
            {
                EnumerationValue value = new EnumerationValue(kp.Value, kp.Key);

                values.Add(value);

            }
            cmbAccounts.DataSource = null;
            cmbAccounts.DataSource = values;
            cmbAccounts.DataBind();
            cmbAccounts.DisplayMember = "DisplayText";
            cmbAccounts.ValueMember = "Value";
            if (values.Count > 1)
            {
                cmbAccounts.Value = values[1].Value;
            }
            else
            {
                cmbAccounts.Value = -1;
            }
            cmbAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;
            this.cmbAccounts.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;

            cmbAccounts.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

        }
        private void LoadPreferences()
        {
            _allocationPreferences = AllocationPreferencesManager.GetPreferences();
            _defaults.SetDefaults(_allocationPreferences.AllocationDefaultList);
            
            // get from database
        }

        #endregion


        #region Color Changed Events
        private void ColorChanged(object sender, System.EventArgs e)
        {

            Infragistics.Win.UltraWinEditors.UltraColorPicker ultraColorPicker = (Infragistics.Win.UltraWinEditors.UltraColorPicker)sender;
            System.Drawing.Color selectedColor = ultraColorPicker.Color;
            ultraColorPicker.Appearance.BackColor = selectedColor;
            ultraColorPicker.Appearance.BorderColor = selectedColor;
            ultraColorPicker.Appearance.ForeColor = selectedColor;


        }


        #endregion

        #region IPreferences Members

        public bool Save()
        {
            try
            {
                _allocationPreferences = GetLatestPrefData(false);
                ctrlAttributeRename2.SaveAttributeNames();
                #region Masterfund Allocation scheme
                //added by Omshiv, 15 Jan 2014
                //save masfter account ratio allocation to DB and get isMFRatioSchemeEnabled to save with preferences in XML

                //TODO used isSaved for show message to user

                //Added condition to check masterfund no, PRANA-11674
                if (_allocationPreferences.GeneralRules.isMasterFundRatioAllocation)
                {
                    if (ctrlMasterFundRatio.GetNoOfMasterFunds() > 0)
                    {
                bool isSaved = ctrlMasterFundRatio.SaveMasterFundTargetRatio();
                        if (!isSaved)
                            return false;
                    }
                    else
                    {
                        MessageBox.Show("Cannot enable MasterFund Ratio Allocation as there is no MasterFund.", "Nirvana Allocation Preferences", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                #endregion

                return this.SaveAllocationPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return true;
        }

        public void RestoreDefault()
        {
            _allocationPreferences = AllocationPreferencesManager.GetDefualtPreferences();
            SetPreferences();
        }

        public UserControl Reference()
        {
            return this;
        }
        public IPreferenceData GetPrefs()
        {
            AllocationPreferences allocationPreferences = GetLatestPrefData(true);
            return allocationPreferences;
        }


        // we have more than one tab in the Livefeed preferences, so need to select a particular Tab from a particular module
        // so declare a property in the IPreference interface
        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }

        public event EventHandler SaveClicked;

        ProxyBase<IAllocationServices> _allocationServices = null;

        private void CreateAllocationServicesProxy()
        {
            string endpointAddressInString = ConfigurationManager.AppSettings["AllocationEndpointAddress"];
            _allocationServices = new ProxyBase<IAllocationServices>(endpointAddressInString);

        }


        #endregion

        #region Properties

        public AllocationPreferences AllocationPreferences
        {

            get { return _allocationPreferences; }
            set
            {
                _allocationPreferences = value;


            }



        }


        #endregion

        private Dictionary<string, int> _allocationSchemes = null;
        public void BindAllocationSchemeList()
        {
            try
            {
                DataTable dt = _allocationServices.InnerChannel.GetAllAllocationSchemeNames();
                lstSchemes.DataSource = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    _allocationSchemes = new Dictionary<string, int>();
                    foreach (DataRow dRow in dt.Rows)
                    {
                        int schemeID = Convert.ToInt32(dRow["AllocationSchemeID"]);
                        string schemeName = dRow["AllocationSchemeName"].ToString();

                        if (!_allocationSchemes.ContainsKey(schemeName) && !schemeName.Equals(string.Empty))
                        {
                            _allocationSchemes.Add(schemeName, schemeID);
                        }
                    }
                    lstSchemes.DataSource = new BindingSource(_allocationSchemes, null);
                    lstSchemes.DisplayMember = "Key";
                    lstSchemes.ValueMember = "Value";
                    lstSchemes.Refresh();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool SaveAllocationPreferences()
        {
            bool isAllocationPrefSaved = false;
            try
            {   //First checked for SavePreferences. If return false then don't save other preferences also, PRANA-10389
                //validate and save default rule first, PRANA-12174
                isAllocationPrefSaved = SaveDefaultRule();
                if (isAllocationPrefSaved)
                    isAllocationPrefSaved = allocationPrefMainControl1.Save();
                if(isAllocationPrefSaved)
                    isAllocationPrefSaved = AllocationPreferencesManager.SavePreferences(_allocationPreferences);
                #region Saving AccountDefaults

                //CachedDataManager.GetInstance.SetAllocationDefaults(_defaults);

                // _closingServices.InnerChannel.UpdatePresetPrefernces(_allocationPreferences.AccountingMethods);
                //CachedDataManager.GetInstance.SetAccountingMethods(_allocationPreferences.AccountingMethods);

                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return isAllocationPrefSaved;
        }

        /// <summary>
        /// Save default rule from general tab
        /// </summary>
        private bool SaveDefaultRule()
        {
            bool isSaved = false;
            try
            {
                //added validation check for default rule, PRANA-12174
                if (ValidateDefaultRule())
                {
                    //DisabledAssets list created to save selected assets in allocation preference 
                    List<int> disabledAssets = new List<int>();
                    foreach (ValueListItem item in ultraCmbDisableCheckSide.CheckedItems)
                    {
                        disabledAssets.Add((int)(item.DataValue));
                    }
                    //AssetsWithCommissionInNetAmount List created to save select assets in Allocation preference, PRANA-11024
                    List<int> assetsWithCommissionInNetAmount = new List<int>();
                    foreach (ValueListItem item in ultraCmbAssetsWithCommissionInNetAmount.CheckedItems)
                    {
                        assetsWithCommissionInNetAmount.Add((int)(item.DataValue));
                    }
                    AllocationRule defaultRule = allocationDefaultRuleControl1.GetCurrentValues();
                    int companyId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                    AllocationCompanyWisePref defaultPref = new AllocationCompanyWisePref() { CompanyId = companyId, DefaultRule = defaultRule, DoCheckSide = chkAllocateBasedonLatestPositions.Checked, AllowEditPreferences = chkAllocateEditPrefrences.Checked, DisableCheckSideForAssets = disabledAssets, PrecisionDigit = Convert.ToInt32(this.ultraNumSetPrecisionValue.Value), AssetsWithCommissionInNetAmount = assetsWithCommissionInNetAmount };
                    AllocationManager.GetInstance().Allocation.InnerChannel.SaveDefaultRule(defaultPref);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSaved;
        }

        /// <summary>
        /// validation for default rule preferences,PRANA-12174
        /// </summary>
        /// <returns></returns>
        private bool ValidateDefaultRule()
        {
            bool isValidationSucessful = true;
            try
            {
                if (Convert.ToInt32(this.ultraNumSetPrecisionValue.Value) > 28)
                {
                    MessageBox.Show(this, "Percision Value should be less than or equal to 28.", "Nirvana Allocation Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValidationSucessful = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return isValidationSucessful;
        }



        private AllocationPreferences GetLatestPrefData(bool Prompt)
        {
            AllocationPreferences allocationPref = new AllocationPreferences();
            try
            {

                #region General Rules
                if (chkbxRoundLot.Checked)
                    allocationPref.GeneralRules.ApplyRoundLotRules = true;
                else
                    allocationPref.GeneralRules.ApplyRoundLotRules = false;

                if (chkbxAvgPricing.Checked)
                    allocationPref.GeneralRules.AveragePricingAllowed = true;
                else
                    allocationPref.GeneralRules.AveragePricingAllowed = false;

                if (chkClearAllocationAccountControlNumber.Checked)
                    allocationPref.GeneralRules.ClearAllocationAccountControlNumer = true;
                else
                    allocationPref.GeneralRules.ClearAllocationAccountControlNumer = false;

                if (chkbxIncludeSavewtState.Checked)
                    allocationPref.GeneralRules.IncludeSavewtState = true;
                else
                    allocationPref.GeneralRules.IncludeSavewtState = false;

                if (chkbxIncludeSavewtoutState.Checked)
                    allocationPref.GeneralRules.IncludeSavewtoutState = true;
                else
                    allocationPref.GeneralRules.IncludeSavewtoutState = false;

                if (chkAllocationMethodologyRevertToAccount.Checked)
                    allocationPref.GeneralRules.AllocationMethodologyRevertToAccount = true;
                else
                    allocationPref.GeneralRules.AllocationMethodologyRevertToAccount = false;

                if (chkAllocateBasedonLatestPositions.Checked)
                    allocationPref.GeneralRules.AllocateBasedonOpenPositions = true;
                else
                    allocationPref.GeneralRules.AllocateBasedonOpenPositions = false;

                if (chkBoxIsPariPassuAllocation.Checked)
                    allocationPref.GeneralRules.IsPariPassuAllocation = true;
                else
                    allocationPref.GeneralRules.IsPariPassuAllocation = false;

                bool isMFRatioSchemeEnabled = ctrlMasterFundRatio.GetIsMAsterAccountAllocEnabed();
                allocationPref.GeneralRules.isMasterFundRatioAllocation = isMFRatioSchemeEnabled;

                if (chkBoxAllocateExtraShare.Checked)
                    allocationPref.GeneralRules.AllocateExtraShareToSelectedAccount = true;
                else
                    allocationPref.GeneralRules.AllocateExtraShareToSelectedAccount = false;

                if (cmbAccounts.DataSource != null && cmbAccounts.Value != null)
                {
                    allocationPref.GeneralRules.SelectedAccountID = int.Parse(cmbAccounts.Value.ToString());
                }

                #endregion

                #region Colors
                allocationPref.RowProperties.UnAllocatedBackColor = ColorUnAllocateBack.Color.ToArgb();
                allocationPref.RowProperties.UnAllocatedTextColor = ColorUnAllocateText.Color.ToArgb();

                allocationPref.RowProperties.AllocatedEqualTotalQtyBackColor = ColorAllocateEqualTotalBack.Color.ToArgb();
                allocationPref.RowProperties.AllocatedEqualTotalQtyTextColor = ColorAllocateEqualTotalText.Color.ToArgb();

                allocationPref.RowProperties.AllocatedLessTotalQtyBackColor = ColorAllocateLessTotalBack.Color.ToArgb();
                allocationPref.RowProperties.AllocatedLessTotalQtyTextColor = ColorAllocateLessTotalText.Color.ToArgb();


                allocationPref.RowProperties.ExecutedLessTotalQtyBackColor = ColorExeLessTotalBack.Color.ToArgb();
                allocationPref.RowProperties.ExecutedLessTotalQtyTextColor = ColorExeLessTotalText.Color.ToArgb();
                allocationPref.RowProperties.SelectedRowBackColor = ColorSelectedRowBackColor.Color.ToArgb();
                allocationPref.RowProperties.SelectedRowTextColor = ColorSelectedRowTextColor.Color.ToArgb();
                #endregion

                #region AutoGrouping Rules
                allocationPref.AutoGroupingRules.CounterParty = chkbxCounterParty.Checked;
                allocationPref.AutoGroupingRules.Venue = chkbxVenue.Checked;
                allocationPref.AutoGroupingRules.BuyAndBCV = chkbxBuyBCV.Checked;
                allocationPref.AutoGroupingRules.TradingAccount = chkbxTradingAcc.Checked;
                allocationPref.AutoGroupingRules.TradeDate = chkbxTradeDate.Checked;
                allocationPref.AutoGroupingRules.ProcessDate = chkbxProcessDate.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes1 = chkbxTradeAttributes1.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes2 = chkbxTradeAttributes2.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes3 = chkbxTradeAttributes3.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes4 = chkbxTradeAttributes4.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes5 = chkbxTradeAttributes5.Checked;
                allocationPref.AutoGroupingRules.TradeAttributes6 = chkbxTradeAttributes6.Checked;

                #endregion

                #region Columns
                allocationPref.ColumnList.UnAllocatedGridColumns.DisplayColumns = accountColumns.UnAllocatedColumns.DisplayColumns;
                allocationPref.ColumnList.AllocatedGridColumns.DisplayColumns = accountColumns.AllocatedColumns.DisplayColumns;
                allocationPref.UnAllocatedColumns = _allocationPreferences.UnAllocatedColumns;
                allocationPref.AllocatedColumns = _allocationPreferences.AllocatedColumns;
                #endregion

                #region Allocation By Account or Symbol
                allocationPref.AllocationByAccountOrSymbol.Account = uoAccountSymbol.CheckedIndex == 0 ? true : false;
                allocationPref.AllocationByAccountOrSymbol.Symbol = uoAccountSymbol.CheckedIndex == 1 ? true : false;

                #endregion



                allocationPref.AllocationDefaultList = _defaults.GetDefaults();


                //allocationPref.AccountingMethods = _allocationPreferences.AccountingMethods;
                return allocationPref;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }



        private void cmbbxdefaults_ValueChanged(object sender, EventArgs e)
        {
            if (cmbbxdefaults.Value != null)
            {
                txtbxDefaultName.Text = string.Empty;
                int DefaultID = int.Parse(cmbbxdefaults.Value.ToString());
                AllocationDefault allocationDefault = _defaults.GetDefault(DefaultID);
                if (allocationDefault.DefaultAllocationLevelList == null)
                {
                    allocationDefault.DefaultAllocationLevelList = new AllocationLevelList();
                    //allocationDefault.DefaultName = string.Empty;
                }

                accountStrategyMappingUserCtrlNew1.SetAllocationDefault(allocationDefault);
                if (allocationDefault.DefaultName != ApplicationConstants.C_COMBO_SELECT)
                {
                    txtbxDefaultName.Text = allocationDefault.DefaultName;
                }
                else
                {
                    txtbxDefaultName.Text = string.Empty;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationDefault allocationdefault = ValidateAndGetDefault();
                if (allocationdefault != null)
                {
                    _defaults.Add(allocationdefault);
                    BindDefaults();
                    cmbbxdefaults.Value = allocationdefault.DefaultID;
                    SaveChangesInDefaults();
                }

                // txtbxDefaultName.Visible = false;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SaveChangesInDefaults()
        {
            try
            {
                CachedDataManager.GetInstance.SetAllocationDefaults(_defaults);
                if (SaveClicked != null)
                {
                    SaveClicked(this, null);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private AllocationDefault ValidateAndGetDefault()
        {
            errorProvider.SetError(btnSave, "");
            if (txtbxDefaultName.Text.Trim() == string.Empty)
            {
                errorProvider.SetError(btnSave, "Please Enter default Name");
                return null;
            }
            AllocationDefault allocationdefault = accountStrategyMappingUserCtrlNew1.AllocationDefault;
            if (allocationdefault == null)
            {
                errorProvider.SetError(btnSave, "Please click on Add New to Add a New Default");
                return null;
            }
            allocationdefault.DefaultAllocationLevelList = accountStrategyMappingUserCtrlNew1.GetAllocationAccounts(new AllocationGroup());
            if (allocationdefault.DefaultAllocationLevelList.GetSumOfPercentageLevel1() != 100.0)
            {
                errorProvider.SetError(btnSave, "Sum of accounts Percentage should be 100");

                return null;
            }
            int wrongAccountID = allocationdefault.DefaultAllocationLevelList.CheckSumOfPercentageLevel2();
            if (wrongAccountID != 0)
            {
                errorProvider.SetError(btnSave, "Sum of Strategies Percentage should be 100");

                return null;
            }


            allocationdefault.DefaultName = txtbxDefaultName.Text.Trim().ToUpper();
            // accountStrategyMappingUserCtrlNew1.ge
            return allocationdefault;

        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbbxdefaults.Value != null && cmbbxdefaults.Value.ToString() != int.MinValue.ToString())
            {
                int DefaultID = int.Parse(cmbbxdefaults.Value.ToString());
                _defaults.Remove(DefaultID);
                //set text boxes to empty
                txtbxDefaultName.Text = "";
                AllocationDefault allocationdefault = new AllocationDefault();
                allocationdefault.DefaultAllocationLevelList = new AllocationLevelList();
                allocationdefault.DefaultID = AllocationManager.GetInstance().GenerateDefaultID();
                accountStrategyMappingUserCtrlNew1.SetAllocationDefault(allocationdefault);
                //endreows
                BindDefaults();
                cmbbxdefaults.Value = int.MinValue;
                SaveChangesInDefaults();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AllocationGroup group = new AllocationGroup();
            AllocationDefault allocationdefault = new AllocationDefault();
            allocationdefault.DefaultAllocationLevelList = new AllocationLevelList();
            allocationdefault.DefaultID = AllocationManager.GetInstance().GenerateDefaultID();
            accountStrategyMappingUserCtrlNew1.SetAllocationDefault(allocationdefault);
            //cmbbxdefaults.Visible = tru;
            // txtbxDefaultName.Visible = true ;
            txtbxDefaultName.Text = "";
            cmbbxdefaults.Value = int.MinValue;
        }

        private void btnEditScheme_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstSchemes.SelectedValue != null)
                {
                    if (_schemeForm == null)
                    {
                        _schemeForm = new AllocationSchemeForm();
                        _schemeForm.FormClosedInformation += new EventHandler(_schemeForm_FormClosedInformation);
                    }
                    else
                    {
                        _schemeForm.BringToFront();
                    }
                    _schemeForm.BringToFront();
                    _schemeForm.Text = "Edit Allocation Scheme";
                    _schemeForm.BindEditAllocationScheme(AllocationScheme.Edit, lstSchemes.Text);
                    _schemeForm.Show();

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _schemeForm_FormClosedInformation(object sender, EventArgs e)
        {
            _schemeForm = null;
        }



        private void btnDeleteScheme_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstSchemes.SelectedValue != null)
                {
                    string schemeName = lstSchemes.Text;
                    int schemeID = _allocationSchemes[schemeName];
                    DialogResult result = MessageBox.Show("Do you really want to delete the scheme?", "Nirvana Allocation Preferences", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result.Equals(DialogResult.Yes))
                    {
                        bool isDeleted = _allocationServices.InnerChannel.DeleteAllocationScheme(schemeID, schemeName);
                        if (isDeleted)
                        {
                            _allocationSchemes.Remove(schemeName);
                            lstSchemes.DataSource = null;
                            if (_allocationSchemes.Count > 0)
                            {
                                lstSchemes.DataSource = new BindingSource(_allocationSchemes, null);
                                lstSchemes.DisplayMember = "Key";
                                lstSchemes.ValueMember = "Value";
                            }
                            MessageBox.Show("Scheme successfully deleted!", "Nirvana Allocation Preferences", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Groups have been allocated using this scheme! Hence,it can't be deleted!", "Nirvana Allocation Preferences", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        AllocationSchemeForm _schemeForm = null;

        private void lstSchemes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstSchemes.SelectedValue != null)
                {
                    string selectedScheme = lstSchemes.Text;

                    if (_schemeForm == null)
                    {
                        _schemeForm = new AllocationSchemeForm();
                        _schemeForm.FormClosedInformation += new EventHandler(_schemeForm_FormClosedInformation);
                    }
                    else
                    {
                        _schemeForm.BringToFront();
                    }
                    _schemeForm.Text = "Edit Allocation Scheme";
                    _schemeForm.BindEditAllocationScheme(AllocationScheme.Edit, selectedScheme);
                    _schemeForm.Show();
                    _schemeForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraTabPageControl8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tlbPreferences_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //if (tlbPreferences.SelectedTab.Key.Equals("tabAttribute"))
            //{
            //    ctrlAttributeRename2.IntializeControl();
            //}
        }

        private void chkbxProcessDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxProcessDate.Checked == false && chkbxTradeDate.Checked == false)
            {
                MessageBox.Show("Please select Either Trade Date or Process Date");
                chkbxProcessDate.Checked = true;
            }
        }

        private void chkbxTradeDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxProcessDate.Checked == false && chkbxTradeDate.Checked == false)
            {
                MessageBox.Show("Please select Either Trade Date or Process Date");
                chkbxTradeDate.Checked = true;
            }
        }

        private void chkBoxAllocateExtraShare_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxAllocateExtraShare.Checked)
            {
                //chkAllocateBasedonLatestPositions.Checked = true;
                //chkBoxIsPariPassuAllocation.Enabled = false;
                //chkBoxIsPariPassuAllocation.Checked = false;
                cmbAccounts.Enabled = true;

                if (cmbAccounts.Value != null)
                {
                    _allocationPreferences.GeneralRules.SelectedAccountID = int.Parse(cmbAccounts.Value.ToString());

                }
            }
            else
            {

                _allocationPreferences.GeneralRules.SelectedAccountID = -1;
                //chkBoxIsPariPassuAllocation.Enabled = true;
                //chkAllocateBasedonLatestPositions.Enabled = false;
                // chkAllocateBasedonLatestPositions.Checked = false;

                cmbAccounts.Enabled = false;
            }
        }


        private void chkBoxAllocateExtraShare_MouseHover(object sender, EventArgs e)
        {
            // this.ultraToolTipManager1.ShowToolTip(chkBoxAllocateExtraShare);
        }

        private void chkBoxIsPariPassuAllocation_CheckedChanged(object sender, EventArgs e)
        {

            //if (chkBoxIsPariPassuAllocation.Checked)
            //{
            //    chkBoxAllocateExtraShare.Enabled = false;
            //    chkBoxAllocateExtraShare.Checked = false;
            //}
            //else
            //{
            //    chkBoxAllocateExtraShare.Enabled = true;
            //}
        }
        /// <summary>
        /// Added to disable or enable ultralblDisableCheckSideAssets and ultraCmbDisableCheckSide according to chkAllocateBasedonLatestPositions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllocateBasedonLatestPositions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraCmbDisableCheckSide.Enabled = chkAllocateBasedonLatestPositions.Checked;
                ultralblDisableCheckSideAssets.Enabled = chkAllocateBasedonLatestPositions.Checked;
                if (chkAllocateBasedonLatestPositions.Checked.Equals(false))
                    ultraCmbDisableCheckSide.Value = null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Load event for UI
        /// Loading default rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllocationPreferencesUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                //bind assets name to ultraCmbDisableCheckSide
                ultraCmbDisableCheckSide.DataSource = new BindingSource(CommonDataCache.CachedData.GetInstance().Asset, null);
                ultraCmbDisableCheckSide.DisplayMember = "Value";
                ultraCmbDisableCheckSide.ValueMember = "Key";
                ultraCmbDisableCheckSide.Enabled = chkAllocateBasedonLatestPositions.Checked;
                ultralblDisableCheckSideAssets.Enabled = chkAllocateBasedonLatestPositions.Checked;

                //bind assets name to ultraCmbAssetsWithCommissionsInNetAmount, PRANA-11024
                ultraCmbAssetsWithCommissionInNetAmount.DataSource = new BindingSource(CommonDataCache.CachedData.GetInstance().Asset, null);
                ultraCmbAssetsWithCommissionInNetAmount.DisplayMember = "Value";
                ultraCmbAssetsWithCommissionInNetAmount.ValueMember = "Key";

                #region Default Rule
                AllocationCompanyWisePref rule = AllocationManager.GetInstance().Allocation.InnerChannel.GetDefaultRule(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID);
                if (rule == null)
                {
                    allocationDefaultRuleControl1.SetValues(new AllocationRule());
                    chkAllocateBasedonLatestPositions.Checked = false;
                    chkAllocateEditPrefrences.Checked = false;
                }
                else
                {
                    allocationDefaultRuleControl1.SetValues(rule.DefaultRule);
                    chkAllocateBasedonLatestPositions.Checked = rule.DoCheckSide;
                    chkAllocateEditPrefrences.Checked = rule.AllowEditPreferences;
                    ultraNumSetPrecisionValue.Value = rule.PrecisionDigit;
                    if (rule.DisableCheckSideForAssets != null)
                        ultraCmbDisableCheckSide.Value = rule.DisableCheckSideForAssets;
                    //set ultraCmbAssetsWithCommissionInNetAmount value, PRANA-11024
                    if (rule.AssetsWithCommissionInNetAmount != null)
                        ultraCmbAssetsWithCommissionInNetAmount.Value = rule.AssetsWithCommissionInNetAmount;
                }              
                #endregion
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                BindEvents();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
}
            }
        }
        /// <summary>
        ///bind events 
        /// </summary>
        private void BindEvents()
        {
            try
            {
                allocationPrefMainControl1.IsAllocationPrefTabSelected += new EventHandler(allocationPrefMainControl1_IsAllocationPrefTabSelected);
                //AllocationPreferencesManager.NoOfMasterFund += new EventHandler(allocationPrefManager_NoOfMasterFund);                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        ///// <summary>
        ///// Added to send no of masterfunds, PRANA-10389
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void allocationPrefManager_NoOfMasterFund(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int noOfMasterFunds = ctrlMasterFundRatio.GetNoOfMasterFunds();
        //        AllocationPreferencesManager.SetMasterFundCount(noOfMasterFunds);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        
        /// <summary>
        /// added to check if allocation pref tab is selected or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allocationPrefMainControl1_IsAllocationPrefTabSelected(object sender, EventArgs e)
        {
            try
            {
                if (tlbPreferences.ActiveTab.Key.Equals("tabAllocationPref"))
                    allocationPrefMainControl1.UpdateIsCurrentTabSelcted(true);
                else
                    allocationPrefMainControl1.UpdateIsCurrentTabSelcted(false);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAdd.BackColor = System.Drawing.Color.FromArgb(55,67,85);
                btnAdd.ForeColor = System.Drawing.Color.White;
                btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdd.UseAppStyling = false;
                btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDeleteScheme.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnDeleteScheme.ForeColor = System.Drawing.Color.White;
                btnDeleteScheme.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDeleteScheme.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDeleteScheme.UseAppStyling = false;
                btnDeleteScheme.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnEditScheme.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnEditScheme.ForeColor = System.Drawing.Color.White;
                btnEditScheme.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnEditScheme.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnEditScheme.UseAppStyling = false;
                btnEditScheme.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
    }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
}
            }
        }
        
        /// <summary>
        /// Delete invalid new added preference 
        /// </summary>
        /// <returns>false if user want to delete, true otherwise</returns>
        public bool RemoveInvalidNewPreferences()
        {
            try
            {
                return allocationPrefMainControl1.RemoveInvalidNewPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
