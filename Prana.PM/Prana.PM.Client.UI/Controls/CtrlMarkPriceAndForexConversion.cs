using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.CommonDataCache.Cache_Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.Client.UI.Forms;
using Prana.PubSubService.Interfaces;
using Prana.SocketCommunication;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.ImportExportUtilities.Excel;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public delegate void MarkPriceSaveHandler(int rowsAffected);
    public partial class CtrlMarkPriceAndForexConversion : Prana.PM.Client.UI.Controls.CtrlTabularUI, ILiveFeedCallback, IPublishing
    {
        // All Currency Standard-Pairs which are checked for while validating so no reverse fx pairs get saved after.
        DataTable _allStandardPairs;
        DataTable _allNonExistingFxCurrencyPairs;
        int _maxDigitsNumber;
        public string _output;
        List<TradeAuditEntry> _tradeAuditCollection_DailyValuation = new List<TradeAuditEntry>();
        Dictionary<string, List<DateTime>> _symbolWiseChangedDatesForMarkPrice = null;
        List<string> _columnsChanged = new List<string>();
        Dictionary<int, List<String>> _accountSymbols = new Dictionary<int, List<String>>();
        bool _isDefaultFilterToShowAccountWiseDataOnDailyValuation = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsDefaultFilterToShowAccountWiseDataOnDailyValuation"));
        bool _isFilteringAccountWiseDataAllowedOnDailyValuation = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsFilteringAccountWiseDataAllowedOnDailyValuation"));
        bool _isDataCopiedForex = false;
        private bool _getSameDayClosedDataOnDV = false;
        private DateTime _dateforNAVLockValidation = DateTime.Now;

        public CtrlMarkPriceAndForexConversion()
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    string maxDigits = ConfigurationHelper.Instance.GetAppSettingValueByKey("MaxDigitsOnForexConversion");
                    if (!string.IsNullOrEmpty(maxDigits) && int.TryParse(maxDigits, out _maxDigitsNumber))
                    {
                        if (_maxDigitsNumber < 2)
                            _maxDigitsNumber = 10;
                    }
                    else
                    {
                        _maxDigitsNumber = 10;
                    }
                    InitializeComponent();
                    CreateSubscriptionServicesProxy();
                    CreatePricingServiceProxy();
                    if (CashManagementServices == null)
                        CreateCashManagementProxy();
                    this.ConfirmationSaveClicked += new EventHandler(ctrlMarkPriceAndForexConversion_ConfirmationSaveClicked);

                    //Bharat Kumar Jangir (25 July 2013)
                    //http://jira.nirvanasolutions.com:8080/browse/MAERISLAND-89
                    _dateSelected = DateTime.Now;
                    _datePrevious = DateTime.Now;
                    _lstDeletedData = new List<CompanyAccountCashCurrencyValue>();
                    _getSameDayClosedDataOnDV = _pricingServicesProxy.InnerChannel.GetSameDayClosedDataConfigValue();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Global variable for having only modified rows when modified from daily valuation UI.
        private bool _isDataCopiedFromBackDate = false;

        private List<CompanyAccountCashCurrencyValue> _lstDeletedData;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        private void CreatePricingServiceProxy()
        {
            _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
        }

        //Enum used to view data in UI depending upon the type selected.
        private enum MethdologySelected
        {
            Daily = 0,
            Weekly = 1,
            Monthly = 2
        }

        //int _userID = int.MinValue;
        IPricingAnalysis _pricingAnalysis = null;
        public IPricingAnalysis PricingAnalysis
        {
            set { _pricingAnalysis = value; }
        }

        public event EventHandler ConfirmationSaveClicked;
        private string _tabPageSelected = string.Empty;
        private bool _isMarketDataBlocked = false;
        System.Collections.Specialized.NameValueCollection _exchangeGroupings = new System.Collections.Specialized.NameValueCollection();
        System.Collections.Generic.Dictionary<string, MarketTimes> _exchangeGroupingAUECTime = new Dictionary<string, MarketTimes>();
        System.Collections.Generic.Dictionary<string, int> _exchangeGroupingAUECID = new Dictionary<string, int>();

        public static DateTime _dateSelected = DateTime.Now;
        public static DateTime _datePrevious = DateTime.Now;
        public static bool _dateTypedIndtDateMonth = false;
        public static bool _dateTypedIndtLiveFeed = false;
        public static bool _tabChanged = false;

        private MethdologySelected _methodologySelected = MethdologySelected.Daily;

        private const string TabName_MarkPrice = "tabPageMarkPrice";
        private const string TabName_ForexConversion = "tabPageForexConversion";
        private const string TabName_NAV = "tabPageNAV";
        private const string TabName_DailyCash = "tabPageDailyCash";
        private const string TabName_CollateralInterest = "tabPageCollateralInterest";
        private const string TabName_Beta = "tabPageDailyBeta";
        private const string TabName_TradingVol = "tabPageDailyTradingVol";
        private const string TabName_Delta = "tabPageDailyDelta";
        private const string TabName_Outstanding = "tabPageDailyOutstandings";
        private const string TabName_PerformanceNumbers = "tabPagePerformanceNumbers";
        private const string TabName_FXMarkPrice = "tabPagefxmarkPrices";
        private const string TabName_StartOfMonthCapitalAccount = "tabPageStartOfMonthCapitalAccount";
        private const string TabName_UserDefinedMTDPnL = "tabPageUserDefinedMTDPnL";
        private const string TabName_DailyCreditLimit = "tabPageDailyCreditLimit";
        private const string TabName_DailyVolatility = "tabPageDailyVolatility";
        private const string TabName_DailyDividendYield = "tabPageDailyDividendYield";
        private const string TabName_VWAP = "tabPageVWAP";
        private const string TabName_CollateralPrice = "tabPageCollateralPrice";
        private const string Const_Nothing = "Nothing";

        //Initailizes the UI for the first time load and doing tasks like binding up the grid etc.
        public void SetupControl(string tabPageName)
        {
            try
            {
                _dateSelected = (DateTime)dtDateMonth.Value;
                //_userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                optDaily.Checked = true;
                this.optMonthly.Location = new System.Drawing.Point(71, 31);
                optLastPrice.Checked = true;
                optOverwriteAllSymbols.Checked = true;
                optUseLiveFeed.Checked = true;
                lblLastDate.Text = "";
                lblPreviousDate.Text = "";
                lblIMidDate.Text = "";
                lblMidDate.Text = "";
                lblSelectedFeedDate.Text = "";
                lblLiveFeedDatePrice.Text = "";

                btnGetFilteredData.Visible = true;
                btnClearFilter.Visible = true;
                txtSymbolFilteration.Visible = true;
                lblFilteredSymbol.Visible = true;
                grpBoxLiveFeedHandling.Visible = true;

                ultraLabel1.Visible = true;
                optOverwriteAllSymbols.Visible = true;
                optOverwriteAllZeroSymbols.Visible = true;
                optOMIPrice.Visible = false;

                lblCopyFromDate.Visible = true;
                dtDateMonth.Visible = true;
                dtCopyFromDate.Visible = true;
                btnFetchData.Visible = true;
                grpSelectDateMethodology.Visible = true;
                pnlAccountCopy.Visible = false;
                this.ultrapanelTop.Size = new System.Drawing.Size(1094, 220);

                _tabPageSelected = tabPageName;
                _exchangeGroupings = ConfigurationHelper.Instance.LoadSectionBySectionName("UnderlyingGroupings");
                //If Forex tab is selected.
                if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    SetupForexConversion();
                    pnlAccountCopy.Visible = _isFilteringAccountWiseDataAllowedOnDailyValuation;
                    if (_isFilteringAccountWiseDataAllowedOnDailyValuation)
                        ultrapanelTop.Size = new System.Drawing.Size(1094, 272);
                    SetupCopyCombos();
                }
                else if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    SetupMarkPrice();
                    pnlAccountCopy.Visible = _isFilteringAccountWiseDataAllowedOnDailyValuation;
                    if (_isFilteringAccountWiseDataAllowedOnDailyValuation)
                        ultrapanelTop.Size = new System.Drawing.Size(1094, 272);
                    SetupCopyCombos();
                }
                else if (_tabPageSelected.Equals(TabName_FXMarkPrice))
                {
                    cmbExchangeGroup.Visible = false;
                    lblExchangeGroup.Visible = false;
                    optUseImportExport.Visible = false;
                    grpBoxImportExport.Visible = false;
                    grdPivotDisplay.AllowDrop = true;
                    grpBoxFilter.Visible = false;
                    btnImport.Visible = false;
                    btnExport.Visible = false;
                    optDaily.Visible = true;
                    optMonthly.Visible = false;
                    grpBoxLiveFeedHandling.Visible = false;
                    grpSelectDateMethodology.Visible = true;
                    optPreviousPrice.Visible = false;
                    optIMidPrice.Visible = false;
                    optMidPrice.Visible = false;
                    optSelectedFeedPrice.Visible = false;
                    optLastPrice.Visible = false;
                    optLastPrice.Text = "Last Price";
                    lblUpdatePrice.Visible = true;
                    lblUpdatePrice.Text = "Update Price:";
                    lblLastDate.Visible = false;
                    lblPreviousDate.Visible = false;
                    lblIMidDate.Visible = false;
                    lblMidDate.Visible = false;
                    lblSelectedFeedDate.Visible = false;
                    btnGetLiveFeedData.Text = "Get Prices";
                    grpBoxLiveFeedHandling.Text = "Update Prices";
                    pnlAccountCopy.Visible = _isFilteringAccountWiseDataAllowedOnDailyValuation;
                    if (_isFilteringAccountWiseDataAllowedOnDailyValuation)
                        ultrapanelTop.Size = new System.Drawing.Size(1094, 272);
                    SetupCopyCombos();
                }
                else if (_tabPageSelected.Equals(TabName_NAV))
                {
                    SetupNAV();
                }
                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    SetupDailyCash();
                }
                else if (_tabPageSelected.Equals(TabName_CollateralInterest))
                {
                    SetUpCollateralInterest();
                }
                else if (_tabPageSelected.Equals(TabName_Beta))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                    grpBoxLiveFeedHandling.Text = "Update Prices";
                }
                else if (_tabPageSelected.Equals(TabName_TradingVol))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                    grpBoxLiveFeedHandling.Text = "Update Prices";
                }
                else if (_tabPageSelected.Equals(TabName_Delta))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Outstanding))
                {
                    ShowHideButtons();
                    cmbExchangeGroup.Visible = false;
                    lblExchangeGroup.Visible = false;
                    optLastPrice.Visible = false;
                    lblUpdatePrice.Visible = false;
                    grpBoxLiveFeedHandling.Text = "Update Prices";
                }
                else if (_tabPageSelected.Equals(TabName_PerformanceNumbers))
                {
                    BindAccounts();
                    cmbExchangeGroup.Visible = false;
                    lblExchangeGroup.Visible = false;
                    optUseImportExport.Visible = false;
                    grpBoxImportExport.Visible = false;
                    grdPivotDisplay.AllowDrop = false;
                    grpBoxFilter.Visible = false;
                    btnImport.Visible = false;
                    btnExport.Visible = false;
                    optDaily.Visible = false;
                    optMonthly.Visible = false;
                    grpBoxFilter.Visible = false;
                    grpBoxLiveFeedHandling.Visible = false;
                    lblFilteredSymbol.Visible = false;
                    txtSymbolFilteration.Visible = false;
                    btnClearFilter.Visible = false;
                    btnGetFilteredData.Visible = false;
                    btnGetLiveFeedData.Text = "Get Prices";
                    lblSelectDateView.Visible = true;
                    grpBoxLiveFeedHandling.Text = "Update Prices";
                }
                else if (_tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount))
                {
                    SetupStartOfMonthCapitalAccount();
                }
                else if (_tabPageSelected.Equals(TabName_UserDefinedMTDPnL))
                {
                    SetupUserDefinedMTDPnL();
                }
                else if (_tabPageSelected.Equals(TabName_DailyCreditLimit))
                {
                    SetupDailyCreditLimit();
                }
                else if (_tabPageSelected.Equals(TabName_DailyVolatility))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                    grpBoxLiveFeedHandling.Text = "Update Volatility";
                }
                else if (_tabPageSelected.Equals(TabName_DailyDividendYield))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                    grpBoxLiveFeedHandling.Text = "Update Dividend Yield";

                }
                else if (_tabPageSelected.Equals(TabName_VWAP))
                {
                    ShowHideButtons();
                    if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                    {
                        cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                        cmbExchangeGroup.Value = "Nothing";
                        cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                    }
                    grpBoxLiveFeedHandling.Text = "Update VWAP";

                }
                else if (_tabPageSelected.Equals(TabName_CollateralPrice))
                {
                    SetupCollateralPrice();
                    pnlAccountCopy.Visible = _isFilteringAccountWiseDataAllowedOnDailyValuation;
                    if (_isFilteringAccountWiseDataAllowedOnDailyValuation)
                        ultrapanelTop.Size = new System.Drawing.Size(1094, 272);
                    SetupCopyCombos();
                }
                // bind grid for selected tab

                BindGridForSelectedTab(_tabPageSelected);
                //ankit
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-1677
                //Filters saved for various tabs. AUEC filter has to be cleared as it was being saved with all other filters.
                if (tabPageName.Equals(TabName_Beta) || tabPageName.Equals(TabName_TradingVol) || tabPageName.Equals(TabName_Delta) || tabPageName.Equals(TabName_MarkPrice) || tabPageName.Equals(TabName_DailyVolatility) || tabPageName.Equals(TabName_DailyDividendYield) || tabPageName.Equals(TabName_VWAP))
                {
                    if (grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters.Exists("AUECIdentifier"))
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].ClearFilterConditions();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetupCollateralPrice()
        {
            cmbExchangeGroup.Visible = false;
            lblExchangeGroup.Visible = false;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = true;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            optDaily.Visible = true;
            optMonthly.Visible = false;
            grpBoxLiveFeedHandling.Visible = false;
            grpSelectDateMethodology.Visible = true;
            optPreviousPrice.Visible = false;
            optIMidPrice.Visible = false;
            optMidPrice.Visible = false;
            optSelectedFeedPrice.Visible = false;
            optLastPrice.Visible = false;
            optLastPrice.Text = "Last Price";
            lblUpdatePrice.Visible = true;
            lblUpdatePrice.Text = "Update Price:";
            lblLastDate.Visible = false;
            lblPreviousDate.Visible = false;
            lblIMidDate.Visible = false;
            lblMidDate.Visible = false;
            lblSelectedFeedDate.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForCollateralPrice()
        {
            DataTable dtCollateralPrices = new DataTable();
            dtCollateralPrices = _pricingServicesProxy.InnerChannel.GetCollateralPriceDateWise(_dateSelected, 0);
            int colLength = dtCollateralPrices.Columns.Count;
            GetListName(tabName);
            AddAcountNameCollateralPrice(dtCollateralPrices);
            dtCollateralPrices.AcceptChanges();
            grdPivotDisplay.DataSource = null;
            grdPivotDisplay.DataSource = dtCollateralPrices;
            _dtGridDataSource = dtCollateralPrices;
        }

        private void AddAcountNameCollateralPrice(DataTable dt)
        {
            try
            {
                dt.Columns.Add("AccountName", typeof(System.String));
                foreach (DataRow row in dt.Rows)
                {
                    row["AccountName"] = CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(row["FundID"]));
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
        private void SetupCopyCombos()
        {
            try
            {
                cmbCopyFromAccount.DataSource = null;
                AccountCollection accounts = CommonDataCache.WindsorContainerManager.GetAccounts();
                accounts.Insert(0, new Account(0, "Select Account"));
                cmbCopyFromAccount.DataSource = accounts;
                cmbCopyFromAccount.DisplayMember = "Name";
                cmbCopyFromAccount.ValueMember = "AccountID";
                cmbCopyFromAccount.SelectedIndex = 0;
                Utils.UltraDropDownFilter(cmbAccount, "Name");

                multiSelectDropDown1.AddItemsToTheCheckList(CachedDataManager.GetInstance.GetAccounts(), CheckState.Unchecked);
                multiSelectDropDown1.AdjustCheckListBoxWidth();
                multiSelectDropDown1.SetTitleText(multiSelectDropDown1.GetNoOfCheckedItems());
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

        private void ShowHideButtons()
        {
            cmbExchangeGroup.Visible = true;
            lblExchangeGroup.Visible = true;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = true;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            grpBoxLiveFeedHandling.Visible = true;
            lblFilteredSymbol.Visible = true;
            optLastPrice.Visible = true;
            optLastPrice.Text = "Last Value";
            optPreviousPrice.Visible = false;
            optIMidPrice.Visible = false;
            optMidPrice.Visible = false;
            optSelectedFeedPrice.Visible = false;
            optDaily.Visible = false;
            optMonthly.Visible = false;
            grpBoxLiveFeedHandling.Visible = true;
            grpSelectDateMethodology.Visible = true;
            lblUpdatePrice.Visible = true;
            lblUpdatePrice.Text = "Update Data:";
            optOverwriteAllZeroSymbols.Text = "Symbol With Zero Values";
            btnGetLiveFeedData.Enabled = true;
            optDaily.Visible = true;
            optMonthly.Visible = true;
            btnGetLiveFeedData.Text = "Get Data";
            ultraLabel1.Visible = false;
            optOverwriteAllSymbols.Visible = false;
            optOverwriteAllZeroSymbols.Visible = false;
        }

        private Dictionary<int, string> _currencyDict = new Dictionary<int, string>();
        private CurrencyCollection _currencyCollection = new CurrencyCollection();
        private AccountCollection _accountCollection = new AccountCollection();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbFromCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbToCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbLocalCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbBaseCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbAccount = new Infragistics.Win.UltraWinGrid.UltraDropDown();

        private void BindComboBoxes()
        {
            try
            {
                _currencyCollection = WindsorContainerManager.GetCurrenciesWithSymbol();
                cmbFromCurrency.DataSource = null;
                cmbFromCurrency.DataSource = _currencyCollection;
                cmbFromCurrency.DisplayMember = "Symbol";
                cmbFromCurrency.ValueMember = "CurrencyID";

                Utils.UltraDropDownFilter(cmbFromCurrency, "Symbol");
                cmbToCurrency.DataSource = null;
                cmbToCurrency.DataSource = _currencyCollection;
                cmbToCurrency.DisplayMember = "Symbol";
                cmbToCurrency.ValueMember = "CurrencyID";

                Utils.UltraDropDownFilter(cmbToCurrency, "Symbol");

                cmbLocalCurrency.DataSource = null;
                cmbLocalCurrency.DataSource = _currencyCollection;
                cmbLocalCurrency.DisplayMember = "Symbol";
                cmbLocalCurrency.ValueMember = "CurrencyID";
                Utils.UltraDropDownFilter(cmbLocalCurrency, "Symbol");
                cmbLocalCurrency.DisplayLayout.Bands[0].Columns["Symbol"].Width = 100;

                cmbBaseCurrency.DataSource = null;
                cmbBaseCurrency.DataSource = _currencyCollection;
                cmbBaseCurrency.DisplayMember = "Symbol";
                cmbBaseCurrency.ValueMember = "CurrencyID";
                Utils.UltraDropDownFilter(cmbBaseCurrency, "Symbol");
                cmbBaseCurrency.DisplayLayout.Bands[0].Columns["Symbol"].Width = 100;

                BindAccounts();
                // filled the Dictionary
                foreach (Prana.BusinessObjects.Currency currency in _currencyCollection)
                {
                    if (!_currencyDict.ContainsKey(currency.CurrencyID))
                    {
                        _currencyDict.Add(currency.CurrencyID, currency.Symbol);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindAccounts()
        {
            try
            {
                _accountCollection = CommonDataCache.WindsorContainerManager.GetAccounts();
                cmbAccount.DataSource = null;
                cmbAccount.DataSource = _accountCollection;
                cmbAccount.DisplayMember = "Name";
                cmbAccount.ValueMember = "AccountID";
                Utils.UltraDropDownFilter(cmbAccount, "Name");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BindAUECFilter()
        {
            try
            {
                cmbExchangeGroup.DataSource = null;
                ValueList underlyingGroupingValueList = new ValueList();
                int keysItemCount = _exchangeGroupings.Count;
                string valueListItemData = string.Empty;
                string valueListItemValue = string.Empty;
                ValueListItem valueListItem = null;
                string[] auecIdentifierGroup = null;
                MarketTimes marketTimes = new MarketTimes();
                string exchangeGroup = string.Empty;
                int auecID = int.MinValue;
                for (int i = 0; i < keysItemCount; i++)
                {
                    valueListItemData = _exchangeGroupings.GetKey(i);
                    valueListItemValue = _exchangeGroupings[valueListItemData];
                    valueListItem = new ValueListItem(valueListItemValue, valueListItemData);

                    underlyingGroupingValueList.ValueListItems.Add(valueListItem);

                    //Adding AUEC timezone time for each exchange group.
                    auecIdentifierGroup = _exchangeGroupings[valueListItemData].ToString().Split(',');
                    exchangeGroup = valueListItemData;
                    marketTimes = MarketStartEndClearanceTimes.GetInstance().GetAUECMarketTimesByIdentifier(auecIdentifierGroup[0].ToString());

                    if (!_exchangeGroupingAUECTime.ContainsKey(exchangeGroup))
                    {
                        _exchangeGroupingAUECTime.Add(exchangeGroup, marketTimes);
                    }
                    auecID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(auecIdentifierGroup[0].ToString());
                    if (!_exchangeGroupingAUECID.ContainsKey(exchangeGroup))
                    {
                        _exchangeGroupingAUECID.Add(exchangeGroup, auecID);
                    }
                }
                valueListItem = new ValueListItem("Nothing", ApplicationConstants.C_COMBO_SELECT);
                underlyingGroupingValueList.ValueListItems.Insert(0, valueListItem);
                cmbExchangeGroup.DataSource = null;
                cmbExchangeGroup.DataSource = underlyingGroupingValueList.ValueListItems;
                cmbExchangeGroup.DisplayMember = "DisplayText";
                cmbExchangeGroup.ValueMember = "DataValue";
                Utils.UltraComboFilter(cmbExchangeGroup, "DisplayText");
                cmbExchangeGroup.Value = "Nothing";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get Accounts Value List
        /// </summary>
        /// <returns></returns>
        private ValueList GetAccountsValueList()
        {
            try
            {
                Dictionary<int, string> accountCollection = CachedDataManager.GetInstance.GetAccountsWithFullName();
                ValueList accountsValList = new ValueList();
                foreach (int accountID in accountCollection.Keys)
                {
                    accountsValList.ValueListItems.Add(accountID, accountCollection[accountID]);
                }
                return accountsValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get Accounts Value List by masterFundId
        /// </summary>
        /// <returns></returns>
        private ValueList GetAccountsValueList(int masterFundId)
        {
            try
            {
                ValueList accountsValList = new ValueList();
                if (CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation().ContainsKey(masterFundId))
                {
                    var accounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[masterFundId];

                    foreach (int accountID in accounts)
                    {
                        var accountName = CachedDataManager.GetInstance.GetAccount(accountID);
                        accountsValList.ValueListItems.Add(accountID, accountName);
                    }
                }
                return accountsValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get Funds Value List
        /// </summary>
        /// <returns></returns>
        private ValueList GetFundsValueList()
        {
            try
            {
                Dictionary<int, string> fundCollection = CachedDataManager.GetInstance.GetAllMasterFunds();
                ValueList accountsValList = new ValueList();
                accountsValList.ValueListItems.Add(0, "-Select-");
                foreach (int accountID in fundCollection.Keys)
                {
                    accountsValList.ValueListItems.Add(accountID, fundCollection[accountID]);
                }
                return accountsValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        DataTable _dtGridDataSource = new DataTable();

        string tabName = string.Empty;

        private Dictionary<string, List<ColumnData>> _dict_Layout = new Dictionary<string, List<ColumnData>>();

        private void GetListName(string tabName)
        {
            if (tabName.Equals(TabName_MarkPrice))
            {
                _dict_Layout[TabName_MarkPrice] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_CollateralPrice)
            {
                _dict_Layout[TabName_CollateralPrice] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_FXMarkPrice)
            {
                _dict_Layout[TabName_FXMarkPrice] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_ForexConversion)
            {
                _dict_Layout[TabName_ForexConversion] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_NAV)
            {
                _dict_Layout[TabName_NAV] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_DailyCash)
            {
                _dict_Layout[TabName_DailyCash] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_CollateralInterest)
            {
                _dict_Layout[TabName_CollateralInterest] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_Beta)
            {
                _dict_Layout[TabName_Beta] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_TradingVol)
            {
                _dict_Layout[TabName_TradingVol] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_Delta)
            {
                _dict_Layout[TabName_Delta] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_Outstanding)
            {
                _dict_Layout[TabName_Outstanding] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_PerformanceNumbers)
            {
                _dict_Layout[TabName_PerformanceNumbers] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_StartOfMonthCapitalAccount)
            {
                _dict_Layout[TabName_StartOfMonthCapitalAccount] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_UserDefinedMTDPnL)
            {
                _dict_Layout[TabName_UserDefinedMTDPnL] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_DailyCreditLimit)
            {
                _dict_Layout[TabName_DailyCreditLimit] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_DailyVolatility)
            {
                _dict_Layout[TabName_DailyVolatility] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_DailyDividendYield)
            {
                _dict_Layout[TabName_DailyDividendYield] = GetGridColumnLayout(grdPivotDisplay);
            }
            else if (tabName == TabName_VWAP)
            {
                _dict_Layout[TabName_VWAP] = GetGridColumnLayout(grdPivotDisplay);
            }
        }

        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    if (gridCol.Key == _datePrevious.Date.ToString("MM/dd/yyyy") || gridCol.Key == _dateSelected.Date.ToString("MM/dd/yyyy") || gridCol.Key == DateTime.Now.ToString("MM/dd/yyyy"))
                    {
                        colData.Key = _dateSelected.Date.ToString("MM/dd/yyyy");
                        colData.Caption = _dateSelected.Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        colData.Key = gridCol.Key;
                        colData.Caption = gridCol.Header.Caption;
                    }
                    colData.Hidden = gridCol.Hidden;

                    //Filter Settings
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
                    }
                    listGridCols.Add(colData);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return listGridCols;
        }

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;
            try
            {

                if (listColData.Count > 0)
                {
                    //Set Columns Properties
                    foreach (ColumnData colData in listColData)
                    {
                        // Skip processing if the column key is null or empty
                        if (string.IsNullOrEmpty(colData.Key))
                        {
                            Logger.LogMsg(LoggerLevel.Information, $"Received empty/null key for ColumnData. Key: {colData?.Key ?? "Unknown"}, Data: {colData}. Skipping processing.");
                            continue;
                        }

                        if (gridColumns.Exists(colData.Key) || gridColumns.Exists(_datePrevious.Date.ToString("MM/dd/yyyy")) || gridColumns.Exists(_dateSelected.Date.ToString("MM/dd/yyyy")) || gridColumns.Exists(DateTime.Now.ToString("MM/dd/yyyy")))
                        {
                            if (colData.Key.Equals("OriginalFromCurrency") || colData.Key.Equals("OriginalToCurrency"))
                                colData.Hidden = true;
                            UltraGridColumn gridCol = gridColumns[colData.Key];
                            gridCol.Hidden = colData.Hidden;
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-2151
                            //In case FXMarkPrice tab, the caption of column Mark Price must not be changed to Selected date.
                            if (gridCol.Header.Caption != "Mark Price")
                                gridCol.Header.Caption = colData.Caption;
                            // Filter Settings  
                            if (colData.FilterConditionList.Count > 0)
                            {
                                band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                                foreach (FilterCondition fCond in colData.FilterConditionList)
                                {
                                    if (!band.ColumnFilters[colData.Key].FilterConditions.Contains(fCond))
                                        band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
                }
            }
            #region Catch  
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information  
                // gets out of our layer.  
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        private void SetForexGridFilterLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            try
            {
                UltraGridBand band = grid.DisplayLayout.Bands[0];
                ColumnsCollection gridColumns = band.Columns;
                if (listColData.Count > 0)
                {
                    foreach (ColumnData colData in listColData)
                    {
                        if (gridColumns.Exists(colData.Key))
                        {
                            UltraGridColumn gridCol = gridColumns[colData.Key];
                            gridCol.Hidden = colData.Hidden;
                            gridCol.Header.Caption = colData.Caption;
                            if (colData.FilterConditionList.Count > 0)
                            {
                                band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                                foreach (FilterCondition fCond in colData.FilterConditionList)
                                {
                                    if (!band.ColumnFilters[colData.Key].FilterConditions.Contains(fCond))
                                        band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
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

        // if blnSelectedTab=True means Mark Price Tab is selected else Forex Rate Tab is selected
        private void BindGridForSelectedTab(string strSelectedTab)
        {
            try
            {
                if (strSelectedTab.Equals(TabName_MarkPrice))
                {
                    BindGridForMarkPrice(strSelectedTab);
                    tabName = TabName_MarkPrice;

                    ApplyAccountFilteration();
                    if (_dict_Layout.ContainsKey(TabName_MarkPrice))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_MarkPrice]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_MarkPrice, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_MarkPrice]);
                    }
                    SetFxFxForwardFilteration();
                }

                else if (strSelectedTab.Equals(TabName_FXMarkPrice))
                {
                    BindGridForMarkPrice(strSelectedTab);
                    tabName = TabName_FXMarkPrice;

                    ApplyAccountFilteration();
                    if (_dict_Layout.ContainsKey(TabName_FXMarkPrice))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_FXMarkPrice]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_FXMarkPrice, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_FXMarkPrice]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_CollateralPrice))
                {
                    BindGridForCollateralPrice();
                    tabName = TabName_CollateralPrice;
                    ApplyAccountFilteration();

                    if (_dict_Layout.ContainsKey(TabName_CollateralPrice))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_CollateralPrice]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_CollateralPrice, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_CollateralPrice]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_ForexConversion))
                {
                    _output = BindGridForForexConversion(string.Empty);
                    tabName = TabName_ForexConversion;

                    if (_dict_Layout.ContainsKey(TabName_ForexConversion))
                        SetForexGridFilterLayout(grdPivotDisplay, _dict_Layout[TabName_ForexConversion]);
                    else
                    {
                        ApplyAccountFilteration();
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_ForexConversion, cd);
                        SetForexGridFilterLayout(grdPivotDisplay, _dict_Layout[TabName_ForexConversion]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_NAV))
                {
                    BindGridForNAV();
                    tabName = TabName_NAV;

                    if (_dict_Layout.ContainsKey(TabName_NAV))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_NAV]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_NAV, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_NAV]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_DailyCash))
                {
                    BindGridForDailyCash();
                    tabName = TabName_DailyCash;

                    if (_dict_Layout.ContainsKey(TabName_DailyCash))
                    {
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyCash]);
                        UltraGridColumn holcol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Date"];
                        holcol.Header.VisiblePosition = 0;
                        if (_methodologySelected.Equals(MethdologySelected.Daily))
                            holcol.Hidden = true;
                        else
                            holcol.Hidden = false;
                    }
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_DailyCash, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyCash]);
                    }
                }

                else if (strSelectedTab.Equals(TabName_CollateralInterest))
                {
                    BindGridForCollateralInterest();
                    tabName = TabName_CollateralInterest;

                    if (_dict_Layout.ContainsKey(TabName_CollateralInterest))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_CollateralInterest]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_CollateralInterest, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_CollateralInterest]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_Beta))
                {
                    // Kuldeep Ag: The monthly view was disabled in case of Betas because on beta tab we displays value of beta 1 by default.
                    // And due to this on saving on monthly tab all the betas were being saved as '1' unnecessarily.
                    optMonthly.Visible = false;
                    DataTable dtDailybeta = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailybeta = WindsorContainerManager.GetBetaValueDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailybeta = WindsorContainerManager.GetBetaValueDateWise(_dateSelected, _dateSelected, 2);
                    }
                    // Kuldeep Ag: This method updates the beta values to '1' if they are null or '0', as we never shows beta as '0'.
                    dtDailybeta = SetDefaultBetaValue(dtDailybeta);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailybeta;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailybeta.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();

                    tabName = TabName_Beta;

                    if (_dict_Layout.ContainsKey(TabName_Beta))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Beta]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_Beta, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Beta]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_TradingVol))
                {
                    DataTable dtDailyTradingVol = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyTradingVol = WindsorContainerManager.GetTradingVolDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailyTradingVol = WindsorContainerManager.GetTradingVolDateWise(_dateSelected, _dateSelected, 2);
                    }
                    ConvertBlankNumericValues(dtDailyTradingVol);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyTradingVol;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyTradingVol.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();
                    tabName = TabName_TradingVol;

                    if (_dict_Layout.ContainsKey(TabName_TradingVol))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_TradingVol]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_TradingVol, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_TradingVol]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_Delta))
                {
                    DataTable dtDailyDelta = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyDelta = WindsorContainerManager.GetDeltaValueDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailyDelta = WindsorContainerManager.GetDeltaValueDateWise(_dateSelected, _dateSelected, 2);
                    }
                    ConvertBlankNumericValues(dtDailyDelta);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyDelta;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyDelta.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();
                    tabName = TabName_Delta;

                    if (_dict_Layout.ContainsKey(TabName_Delta))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Delta]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_Delta, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Delta]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_Outstanding))
                {
                    DataTable dtoutStanding = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtoutStanding = WindsorContainerManager.GetOutSatndingDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtoutStanding = WindsorContainerManager.GetOutSatndingDateWise(_dateSelected, _dateSelected, 2);
                    }
                    ConvertBlankNumericValues(dtoutStanding);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtoutStanding;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    { dtoutStanding.Columns[3].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy"); }
                    tabName = TabName_Outstanding;

                    if (_dict_Layout.ContainsKey(TabName_Outstanding))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Outstanding]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_Outstanding, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_Outstanding]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_PerformanceNumbers))
                {
                    DataTable dtDailyPerformanceNumbers = new DataTable();
                    dtDailyPerformanceNumbers = WindsorContainerManager.GetPerformanceNumberValueDateWise(_dateSelected);

                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyPerformanceNumbers;
                    tabName = TabName_PerformanceNumbers;

                    if (_dict_Layout.ContainsKey(TabName_PerformanceNumbers))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_PerformanceNumbers]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_PerformanceNumbers, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_PerformanceNumbers]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_StartOfMonthCapitalAccount))
                {
                    BindGridForStartOfMonthCapitalAccount();
                    tabName = TabName_StartOfMonthCapitalAccount;

                    if (_dict_Layout.ContainsKey(TabName_StartOfMonthCapitalAccount))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_StartOfMonthCapitalAccount]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_StartOfMonthCapitalAccount, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_StartOfMonthCapitalAccount]);
                    }
                }

                else if (strSelectedTab.Equals(TabName_UserDefinedMTDPnL))
                {
                    BindGridForUserDefinedMTDPnL();
                    tabName = TabName_UserDefinedMTDPnL;

                    if (_dict_Layout.ContainsKey(TabName_UserDefinedMTDPnL))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_UserDefinedMTDPnL]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_UserDefinedMTDPnL, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_UserDefinedMTDPnL]);
                    }
                }

                else if (strSelectedTab.Equals(TabName_DailyCreditLimit))
                {
                    BindGridForDailyCreditLimit();
                    tabName = TabName_DailyCreditLimit;

                    if (_dict_Layout.ContainsKey(TabName_DailyCreditLimit))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyCreditLimit]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_DailyCreditLimit, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyCreditLimit]);
                    }
                }

                else if (strSelectedTab.Equals(TabName_DailyDividendYield))
                {
                    optMonthly.Visible = false;
                    DataTable dtDailyDividendYield = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyDividendYield = WindsorContainerManager.GetDividendYieldValueDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailyDividendYield = WindsorContainerManager.GetDividendYieldValueDateWise(_dateSelected, _dateSelected, 2);
                    }
                    dtDailyDividendYield = SetDefaultDividendYieldValue(dtDailyDividendYield);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyDividendYield;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyDividendYield.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();

                    tabName = TabName_DailyDividendYield;

                    if (_dict_Layout.ContainsKey(TabName_DailyDividendYield))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyDividendYield]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_DailyDividendYield, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyDividendYield]);
                    }

                }
                else if (strSelectedTab.Equals(TabName_DailyVolatility))
                {
                    optMonthly.Visible = false;
                    DataTable dtDailyVolatility = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyVolatility = WindsorContainerManager.GetVolatilityValueDateWise(_dateSelected, _dateSelected, 0);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailyVolatility = WindsorContainerManager.GetVolatilityValueDateWise(_dateSelected, _dateSelected, 2);
                    }
                    dtDailyVolatility = SetDefaultVolatilityValue(dtDailyVolatility);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyVolatility;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyVolatility.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();

                    tabName = TabName_DailyVolatility;

                    if (_dict_Layout.ContainsKey(TabName_DailyVolatility))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyVolatility]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_DailyVolatility, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_DailyVolatility]);
                    }
                }
                else if (strSelectedTab.Equals(TabName_VWAP))
                {
                    DataTable dtDailyVWAP = new DataTable();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyVWAP = WindsorContainerManager.GetVWAPValueDateWise(_dateSelected, 0, _getSameDayClosedDataOnDV);
                    }
                    else if (_methodologySelected.Equals(MethdologySelected.Weekly))
                    {
                    }
                    else
                    {
                        dtDailyVWAP = WindsorContainerManager.GetVWAPValueDateWise(_dateSelected, 2, _getSameDayClosedDataOnDV);
                    }
                    ConvertBlankNumericValues(dtDailyVWAP);
                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtDailyVWAP;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDailyVWAP.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    ResetFilterAfterRebindingGrid();

                    tabName = TabName_VWAP;

                    if (_dict_Layout.ContainsKey(TabName_VWAP))
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_VWAP]);
                    else
                    {
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_VWAP, cd);
                        SetGridColumnLayout(grdPivotDisplay, _dict_Layout[TabName_VWAP]);
                    }
                }



                if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists(_datePrevious.ToString("MM/dd/yyyy")))
                {
                    UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns[_datePrevious.ToString("MM/dd/yyyy")];
                    colDate.Hidden = false;
                }
                else if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists(_dateSelected.ToString("MM/dd/yyyy")))
                {
                    UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns[_dateSelected.ToString("MM/dd/yyyy")];
                    colDate.Hidden = false;
                }
                else if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists(DateTime.Now.ToString("MM/dd/yyyy")))
                {
                    UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns[DateTime.Now.ToString("MM/dd/yyyy")];
                    colDate.Hidden = false;
                }
                SetLiveFeedPriceStatus();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DataTable SetDefaultBetaValue(DataTable dtDailybeta)
        {
            try
            {
                double oneValue = 1;
                // We use index value 3 to update beta value as the value of beta is always coming on this place.
                if (dtDailybeta != null && dtDailybeta.Rows.Count >= 0)
                {
                    foreach (DataRow dRow in dtDailybeta.Rows)
                    {
                        if (dRow[4] is DBNull || Convert.ToDouble(dRow[4]) == 0)
                        {
                            dRow[4] = oneValue;
                        }
                    }
                    dtDailybeta.AcceptChanges();
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
            return dtDailybeta;
        }

        private DataTable SetDefaultVolatilityValue(DataTable dtVolatility)
        {
            try
            {
                double oneValue = 0;

                if (dtVolatility != null && dtVolatility.Rows.Count >= 0)
                {
                    foreach (DataRow dRow in dtVolatility.Rows)
                    {
                        if (dRow[4] is DBNull || Convert.ToDouble(dRow[4]) == 0)
                        {
                            dRow[4] = oneValue;
                        }
                    }
                    dtVolatility.AcceptChanges();
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
            return dtVolatility;
        }

        /// <summary>
        /// Sets the default vwap value.
        /// </summary>
        /// <param name="dtVWAP">The dt vwap.</param>
        /// <returns></returns>
        private DataTable SetDefaultVWAPValue(DataTable dtVWAP)
        {
            try
            {
                double oneValue = 0;

                if (dtVWAP != null && dtVWAP.Rows.Count >= 0)
                {
                    foreach (DataRow dRow in dtVWAP.Rows)
                    {
                        if (dRow[4] is DBNull || Convert.ToDouble(dRow[4]) == 0)
                        {
                            dRow[4] = oneValue;
                        }
                    }
                    dtVWAP.AcceptChanges();
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
            return dtVWAP;
        }

        private DataTable SetDefaultDividendYieldValue(DataTable dtDividendYield)
        {
            try
            {
                double oneValue = 0;
                if (dtDividendYield != null && dtDividendYield.Rows.Count >= 0)
                {
                    foreach (DataRow dRow in dtDividendYield.Rows)
                    {
                        if (dRow[4] is DBNull || Convert.ToDouble(dRow[4]) == 0)
                        {
                            dRow[4] = oneValue;
                        }
                    }
                    dtDividendYield.AcceptChanges();
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
            return dtDividendYield;
        }

        private void ResetFilterAfterRebindingGrid()
        {
            try
            {
                txtSymbolFilteration.Text = string.Empty;
                SetExchangeGroupFilteration();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Adds new to row to the grid depending upon the tab and view(Monthly, Daily) selected.
        public void AddNewRow()
        {
            try
            {
                _dtGridDataSource = (DataTable)grdPivotDisplay.DataSource;

                if (_dtGridDataSource == null)
                {
                    _dtGridDataSource = new DataTable();
                    _dtGridDataSource.Columns.Add(new DataColumn("FromCurrencyID"));
                    _dtGridDataSource.Columns.Add(new DataColumn("ToCurrencyID"));
                    _dtGridDataSource.Columns.Add(new DataColumn("Symbol"));
                    _dtGridDataSource.Columns.Add(new DataColumn("AccountID"));
                    _dtGridDataSource.Columns.Add(new DataColumn("Account"));
                    _dtGridDataSource.Columns.Add(new DataColumn("Date"));
                    _dtGridDataSource.Columns.Add(new DataColumn("Summary"));
                }

                double zeroValue = 0;
                DataRow dtRow = null;
                string filterExchangeGroup = cmbExchangeGroup.Value.ToString();

                #region Never called this method as we are not having add button on mark price tab
                //if (_tabPageSelected.Equals(TabName_MarkPrice))
                //{
                //    if (filterExchangeGroup.Equals("Indices-Indices"))
                //    {
                //        dtRow = _dtGridDataSource.NewRow();
                //        dtRow["Symbol"] = string.Empty;
                //        dtRow["AUECID"] = 0;
                //        dtRow["AUECIdentifier"] = "Indices-Indices";

                //        foreach (DataColumn dCol in _dtGridDataSource.Columns)
                //        {
                //            if (!dCol.ColumnName.ToString().Equals("Symbol") && !dCol.ColumnName.ToString().Equals("AUECID") && !dCol.ColumnName.ToString().Equals("AUECIdentifier"))
                //            {
                //                dtRow[dCol] = zeroValue;
                //            }
                //        }

                //        _dtGridDataSource.Rows.Add(dtRow);
                //        grdPivotDisplay.DataSource = null;
                //        grdPivotDisplay.DataSource = _dtGridDataSource;
                //        grdPivotDisplay.Update();

                //        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

                //        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].FilterConditions.Add(FilterComparisionOperator.Equals, filterExchangeGroup);

                //        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].LogicalOperator = FilterLogicalOperator.Or;

                //        UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                //        colSymbol.CellActivation = Activation.AllowEdit;
                //    }
                //}
                #endregion

                if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    if (_dtGridDataSource != null)
                    {
                        CurrencyPairCache.GetInstance().CreateOrUpdateCacheFromDataTable(_dtGridDataSource);
                        AddAccountWiseCurrenyPair accountWiseCurrency = new AddAccountWiseCurrenyPair();
                        bool isSuccessfullyAdded = accountWiseCurrency.ShowAddCurrencyForm(this.FindForm());
                        if (isSuccessfullyAdded)
                        {
                            if (accountWiseCurrency.StandardCurrency != null)
                            {
                                #region Adding row for 0 AccountId
                                DataRow drNew = _dtGridDataSource.NewRow();
                                drNew["FromCurrencyID"] = accountWiseCurrency.StandardCurrency.FromCurrency;
                                drNew["ToCurrencyID"] = accountWiseCurrency.StandardCurrency.ToCurrency;
                                drNew["Symbol"] = accountWiseCurrency.StandardCurrency.Symbol;
                                drNew["AccountID"] = 0;
                                drNew["Account"] = string.Empty;
                                drNew["Summary"] = string.Empty;
                                for (int col = 0; col < _dtGridDataSource.Columns.Count; col++)
                                {
                                    if (!_dtGridDataSource.Columns[col].ColumnName.Equals("FromCurrencyID") &&
                                        !_dtGridDataSource.Columns[col].ColumnName.Equals("ToCurrencyID") &&
                                        !_dtGridDataSource.Columns[col].ColumnName.Equals("Symbol") &&
                                        !_dtGridDataSource.Columns[col].ColumnName.Equals("Summary") &&
                                        !_dtGridDataSource.Columns[col].ColumnName.Equals("Account") &&
                                        !_dtGridDataSource.Columns[col].ColumnName.Equals("AccountID"))
                                    {
                                        drNew[_dtGridDataSource.Columns[col].ColumnName] = 0;
                                    }
                                }
                                _dtGridDataSource.Rows.Add(drNew);
                                #endregion

                                foreach (Account account in _accountCollection)
                                {
                                    DataRow dr = _dtGridDataSource.NewRow();
                                    dr["FromCurrencyID"] = accountWiseCurrency.StandardCurrency.FromCurrency;
                                    dr["ToCurrencyID"] = accountWiseCurrency.StandardCurrency.ToCurrency;
                                    dr["Symbol"] = accountWiseCurrency.StandardCurrency.Symbol;
                                    dr["AccountID"] = account.AccountID;
                                    dr["Account"] = account.AccountID > 0 ? CachedDataManager.GetInstance.GetAccountsWithFullName()[account.AccountID] : string.Empty;
                                    dr["Summary"] = string.Empty;
                                    for (int col = 0; col < _dtGridDataSource.Columns.Count; col++)
                                    {
                                        if (!_dtGridDataSource.Columns[col].ColumnName.Equals("FromCurrencyID") &&
                                            !_dtGridDataSource.Columns[col].ColumnName.Equals("ToCurrencyID") &&
                                            !_dtGridDataSource.Columns[col].ColumnName.Equals("Symbol") &&
                                            !_dtGridDataSource.Columns[col].ColumnName.Equals("Summary") &&
                                            !_dtGridDataSource.Columns[col].ColumnName.Equals("Account") &&
                                            !_dtGridDataSource.Columns[col].ColumnName.Equals("AccountID"))
                                        {
                                            dr[_dtGridDataSource.Columns[col].ColumnName] = 0;
                                        }
                                    }
                                    _dtGridDataSource.Rows.Add(dr);
                                }
                            }

                            _dtGridDataSource.Columns[5].ColumnName = dtDateMonth.DateTime.ToString("MM/dd/yyyy");
                            grdPivotDisplay.DataSource = null;
                            grdPivotDisplay.DataSource = _dtGridDataSource;

                            ApplyAccountFilteration();
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    dtRow = _dtGridDataSource.NewRow();
                    int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    int localCurrencyID = int.Parse(((Currency)_currencyCollection[0]).CurrencyID.ToString());
                    if (_accountCollection.Count > 0)
                    {
                        dtRow["FundID"] = ((Account)_accountCollection[0]).AccountID;
                    }
                    if (_currencyCollection.Count > 0)
                    {
                        dtRow["LocalCurrencyID"] = localCurrencyID;
                    }
                    dtRow["BaseCurrencyID"] = companyBaseCurrencyID;
                    dtRow["CashValueLocal"] = 0;
                    dtRow["CashValueBase"] = 0;

                    _dtGridDataSource.Rows.Add(dtRow);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = _dtGridDataSource;
                    grdPivotDisplay.Update();
                }
                else if (_tabPageSelected.Equals(TabName_CollateralInterest))
                {
                    dtRow = _dtGridDataSource.NewRow();
                    if (_accountCollection.Count > 0)
                    {
                        dtRow["FundID"] = ((Account)_accountCollection[0]).AccountID;
                    }

                    _dtGridDataSource.Rows.Add(dtRow);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = _dtGridDataSource;
                    grdPivotDisplay.Update();
                }
                else if (_tabPageSelected.Equals(TabName_PerformanceNumbers))
                {
                    dtRow = _dtGridDataSource.NewRow();
                    if (_accountCollection.Count > 0)
                    {
                        dtRow["FundID"] = ((Account)_accountCollection[0]).AccountID;
                    }

                    dtRow["MTDValue"] = 0;
                    dtRow["QTDValue"] = 0;
                    dtRow["YTDValue"] = 0;

                    dtRow["MTDReturn"] = 0;
                    dtRow["QTDReturn"] = 0;
                    dtRow["YTDReturn"] = 0;

                    _dtGridDataSource.Rows.Add(dtRow);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = _dtGridDataSource;
                    grdPivotDisplay.Update();
                }
                else if (_tabPageSelected.Equals(TabName_NAV) || _tabPageSelected.Equals(TabName_CollateralInterest) || _tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount) || _tabPageSelected.Equals(TabName_UserDefinedMTDPnL) || _tabPageSelected.Equals(TabName_DailyCreditLimit))
                {
                    dtRow = _dtGridDataSource.NewRow();
                    for (int i = 1; i < dtRow.ItemArray.Length; i++)
                    {
                        dtRow[i] = zeroValue;
                    }
                    _dtGridDataSource.Rows.Add(dtRow);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = _dtGridDataSource;
                    grdPivotDisplay.Update();
                }
                else
                {
                    dtRow = _dtGridDataSource.NewRow();
                    _dtGridDataSource.Rows.Add(dtRow);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = _dtGridDataSource;
                    grdPivotDisplay.Update();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Removing row from the grid depending upon the valid selection.
        public void RemoveRow()
        {
            try
            {
                if (grdPivotDisplay.ActiveRow != null)
                {
                    if (_tabPageSelected.Equals(TabName_DailyCash))
                    {
                        DataRow dRow = ((System.Data.DataRowView)(grdPivotDisplay.ActiveRow.ListObject)).Row;

                        CompanyAccountCashCurrencyValue cashValue = new CompanyAccountCashCurrencyValue();
                        cashValue.AccountID = int.Parse(dRow["FundID"].ToString());
                        cashValue.LocalCurrencyID = int.Parse(dRow["LocalCurrencyID"].ToString());
                        cashValue.BaseCurrencyID = int.Parse(dRow["BaseCurrencyID"].ToString());
                        cashValue.Date = !string.IsNullOrWhiteSpace(dRow["Date"].ToString()) ? Convert.ToDateTime(dRow["Date"].ToString()).Date : DateTimeConstants.MinValue;

                        if (!string.IsNullOrEmpty(dRow["Date"].ToString()) && grdPivotDisplay.ActiveRow.Delete(false))
                        {
                            _lstDeletedData.Add(cashValue);
                        }
                    }
                    else if (_tabPageSelected.Equals(TabName_MarkPrice))
                    {
                        if (cmbExchangeGroup.Value != null && cmbExchangeGroup.Value.ToString().Equals("Indices-Indices"))
                        {
                            grdPivotDisplay.ActiveRow.Delete(true);
                            _dtGridDataSource.AcceptChanges();
                        }
                    }
                    else if (_tabPageSelected.Equals(TabName_DailyCreditLimit) || _tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount) || _tabPageSelected.Equals(TabName_UserDefinedMTDPnL))
                    {
                        grdPivotDisplay.ActiveRow.Delete(true);
                    }
                    else
                    {
                        grdPivotDisplay.ActiveRow.Delete(true);
                        _dtGridDataSource.AcceptChanges();
                    }
                }
                else
                {
                    InformationMessageBox.Display("Please select a row to delete", "Information");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdPivotDisplay_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                foreach (UltraGridColumn col in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption == "AUECID" || col.Header.Caption == "AUECIdentifier")
                    {
                        col.Hidden = true;
                    }
                }

                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.Header.VisiblePosition = 0;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.Width = 150;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.Header.VisiblePosition = 1;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";

                    UltraGridColumn colAccount = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Account"];
                    colAccount.Header.Fixed = true;
                    colAccount.Header.VisiblePosition = 2;
                    colAccount.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccount.Width = 200;
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Hidden = !_isFilteringAccountWiseDataAllowedOnDailyValuation;

                    UltraGridColumn colAccountId = grdPivotDisplay.DisplayLayout.Bands[0].Columns["AccountID"];
                    colAccountId.Header.Fixed = true;
                    colAccountId.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountId.CellActivation = Activation.NoEdit;
                    colAccountId.Hidden = true;

                    UltraGridColumn colForwardPoints = grdPivotDisplay.DisplayLayout.Bands[0].Columns["ForwardPoints"];
                    colForwardPoints.Header.Fixed = true;
                    colForwardPoints.Header.Appearance.TextHAlign = HAlign.Center;
                    colForwardPoints.Hidden = true;

                    UltraGridColumn colFxRate = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FxRate"];
                    colFxRate.Header.Fixed = true;
                    colFxRate.Header.Appearance.TextHAlign = HAlign.Center;
                    colFxRate.Hidden = true;

                    for (int iCol = 1; iCol < grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count; iCol++)
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].MaxLength = 20;
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].Header.Appearance.TextHAlign = HAlign.Center;
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_CollateralPrice))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.Header.VisiblePosition = 0;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.Width = 150;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.Header.VisiblePosition = 1;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";

                    UltraGridColumn colAccount = grdPivotDisplay.DisplayLayout.Bands[0].Columns["AccountName"];
                    colAccount.Header.Fixed = true;
                    colAccount.Header.VisiblePosition = 2;
                    colAccount.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccount.Width = 200;
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Hidden = !_isFilteringAccountWiseDataAllowedOnDailyValuation;
                    colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccount.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccount.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccount.Header.Caption = "Account";

                    UltraGridColumn colAccountId = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    colAccountId.Header.Fixed = true;
                    colAccountId.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountId.CellActivation = Activation.NoEdit;
                    colAccountId.Hidden = true;

                    UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns["DATE"];
                    colDate.Header.Appearance.TextHAlign = HAlign.Center;
                    colDate.Hidden = true;

                    UltraGridColumn colCollateralPrice = grdPivotDisplay.DisplayLayout.Bands[0].Columns["CollateralPrice"];
                    colCollateralPrice.Header.Fixed = true;
                    colCollateralPrice.Header.VisiblePosition = 3;
                    colCollateralPrice.Header.Appearance.TextHAlign = HAlign.Center;
                    colCollateralPrice.CharacterCasing = CharacterCasing.Upper;
                    colCollateralPrice.CellActivation = Activation.AllowEdit;
                    colCollateralPrice.Header.Caption = "Collateral Price";

                    UltraGridColumn colHaircut = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Haircut"];
                    colHaircut.Header.Fixed = true;
                    colHaircut.Header.VisiblePosition = 4;
                    colHaircut.Header.Appearance.TextHAlign = HAlign.Center;
                    colHaircut.CharacterCasing = CharacterCasing.Upper;
                    colHaircut.CellActivation = Activation.AllowEdit;
                    colHaircut.Header.Caption = "Haircut(%)";

                    UltraGridColumn colRebateOnMV = grdPivotDisplay.DisplayLayout.Bands[0].Columns["RebateOnMV"];
                    colRebateOnMV.Header.Fixed = true;
                    colRebateOnMV.Header.VisiblePosition = 5;
                    colRebateOnMV.Header.Appearance.TextHAlign = HAlign.Center;
                    colRebateOnMV.CharacterCasing = CharacterCasing.Upper;
                    colRebateOnMV.CellActivation = Activation.AllowEdit;
                    colRebateOnMV.Header.Caption = "Fee/Rebate on MV(%)";

                    UltraGridColumn colRebateOnCollateral = grdPivotDisplay.DisplayLayout.Bands[0].Columns["RebateOnCollateral"];
                    colRebateOnCollateral.Header.Fixed = true;
                    colRebateOnCollateral.Header.VisiblePosition = 6;
                    colRebateOnCollateral.Header.Appearance.TextHAlign = HAlign.Center;
                    colRebateOnCollateral.CharacterCasing = CharacterCasing.Upper;
                    colRebateOnCollateral.CellActivation = Activation.AllowEdit;
                    colRebateOnCollateral.Header.Caption = "Fee/Rebate on Collateral(%)";

                    UltraGridColumn colAssetId = grdPivotDisplay.DisplayLayout.Bands[0].Columns["AssetID"];
                    colAssetId.Header.Appearance.TextHAlign = HAlign.Center;
                    colAssetId.Hidden = true;

                    for (int iCol = 1; iCol < grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count; iCol++)
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].MaxLength = 20;
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].Header.Appearance.TextHAlign = HAlign.Center;
                        grdPivotDisplay.DisplayLayout.Bands[0].Columns[iCol].InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_FXMarkPrice))
                {

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];

                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;
                    colSymbol.Header.VisiblePosition = 0;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.Header.Fixed = true;
                    colSymbol.Width = 150;

                    UltraGridColumn colBloombergSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloombergSymbol.Header.Caption = "Bloomberg Symbol";
                    colBloombergSymbol.CellActivation = Activation.NoEdit;
                    colBloombergSymbol.Header.VisiblePosition = 1;
                    colBloombergSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloombergSymbol.Header.Fixed = true;
                    colBloombergSymbol.CharacterCasing = CharacterCasing.Upper;

                    UltraGridColumn colAccount = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Account"];
                    colAccount.Header.Fixed = true;
                    colAccount.Width = 200;
                    colAccount.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccount.Header.VisiblePosition = 2;
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Hidden = !_isFilteringAccountWiseDataAllowedOnDailyValuation;

                    UltraGridColumn colAccountId = grdPivotDisplay.DisplayLayout.Bands[0].Columns["AccountID"];
                    colAccountId.Header.Fixed = true;
                    colAccountId.Hidden = true;
                    colAccountId.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountId.CellActivation = Activation.NoEdit;

                    UltraGridColumn colFxorwardPoints = grdPivotDisplay.DisplayLayout.Bands[0].Columns["ForwardPoints"];
                    colFxorwardPoints.Header.VisiblePosition = 3;
                    colFxorwardPoints.Header.Caption = "Forward Points";
                    colFxorwardPoints.Header.Appearance.TextHAlign = HAlign.Center;
                    colFxorwardPoints.Header.Fixed = true;

                    UltraGridColumn colFxRate = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FxRate"];
                    colFxRate.CellActivation = Activation.NoEdit;
                    colFxRate.Header.VisiblePosition = 4;
                    colFxRate.Header.Appearance.TextHAlign = HAlign.Center;
                    colFxRate.Header.Fixed = true;

                    foreach (UltraGridColumn col in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        DateTime dateOut = DateTime.Now;
                        _colName = col.Header.Caption;
                        //string symbol = string.Empty;
                        if (DateTime.TryParse(_colName, out dateOut))
                        {
                            grdPivotDisplay.DisplayLayout.Bands[0].Columns[_colName].Header.Caption = "Mark Price";
                            grdPivotDisplay.DisplayLayout.Bands[0].Columns[_colName].Header.VisiblePosition = 5;
                            break;
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    UltraGridColumn colFromCurrency = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FromCurrencyID"];
                    colFromCurrency.ValueList = cmbFromCurrency;
                    colFromCurrency.Header.Caption = "From Currency";
                    colFromCurrency.Header.Appearance.TextHAlign = HAlign.Center;
                    colFromCurrency.CellActivation = Activation.NoEdit;
                    colFromCurrency.Header.VisiblePosition = 1;
                    colFromCurrency.Width = 130;
                    colFromCurrency.Header.Fixed = true;

                    UltraGridColumn colToCurrency = grdPivotDisplay.DisplayLayout.Bands[0].Columns["ToCurrencyID"];
                    colToCurrency.ValueList = cmbFromCurrency;
                    colToCurrency.Header.Caption = "To Currency";
                    colToCurrency.Header.Appearance.TextHAlign = HAlign.Center;
                    colToCurrency.CellActivation = Activation.NoEdit;
                    colToCurrency.Header.VisiblePosition = 2;
                    colToCurrency.Width = 130;
                    colToCurrency.Header.Fixed = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.CellActivation = Activation.AllowEdit;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Caption = "Symbol";
                    colSymbol.CellActivation = Activation.NoEdit;
                    colSymbol.Header.VisiblePosition = 3;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.Width = 130;
                    colSymbol.Header.Fixed = true;

                    UltraGridColumn colAccount = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Account"];
                    colAccount.Header.Fixed = true;
                    colAccount.Width = 200;
                    colAccount.Header.VisiblePosition = 4;
                    colAccount.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Hidden = !_isFilteringAccountWiseDataAllowedOnDailyValuation;

                    UltraGridColumn colAccountId = grdPivotDisplay.DisplayLayout.Bands[0].Columns["AccountID"];
                    colAccountId.Header.Fixed = true;
                    colAccountId.CellActivation = Activation.NoEdit;
                    colAccountId.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountId.Hidden = true;

                    UltraGridColumn colSummary = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Summary"];
                    colSummary.CellActivation = Activation.NoEdit;
                    colSummary.Header.Caption = "Summary";
                    colSummary.Width = 200;
                    colSummary.Header.VisiblePosition = 5;
                    colSummary.Header.Appearance.TextHAlign = HAlign.Center;
                    colSummary.Hidden = !optDaily.Checked;
                    colSummary.Header.Fixed = true;

                    if (!optDaily.Checked)
                    {
                        foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                        {
                            if (!column.Header.Caption.Equals("From Currency") &&
                                !column.Header.Caption.Equals("To Currency") &&
                                !column.Header.Caption.Equals("Symbol") &&
                                !column.Header.Caption.Equals("Account"))
                            {
                                column.MaxLength = _maxDigitsNumber;
                                column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                                column.CellActivation = Activation.AllowEdit;
                            }
                            column.Header.Appearance.TextHAlign = HAlign.Center;
                        }
                    }
                    else if (optDaily.Checked)
                    {
                        UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns[5];
                        colDate.MaxLength = _maxDigitsNumber;
                        colDate.Header.Appearance.TextHAlign = HAlign.Center;
                        colDate.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        colDate.CellActivation = Activation.AllowEdit;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_NAV) || _tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount) || _tabPageSelected.Equals(TabName_UserDefinedMTDPnL))
                {
                    UltraGridColumn colAccountID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    // colAccountID.ValueList = GetAccountsValueList();
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccountID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccountID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountID.Width = 150;
                    colAccountID.Header.VisiblePosition = 0;

                    if (_tabPageSelected.Equals(TabName_NAV))
                    {
                        UltraGridColumn colMasterFundID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["MasterFund"];
                        colMasterFundID.ValueList = GetFundsValueList();
                        colMasterFundID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colMasterFundID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                        colMasterFundID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                        colMasterFundID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colMasterFundID.Header.Caption = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? "Client" : "Master Fund";
                        colMasterFundID.Width = 150;
                        colMasterFundID.Header.VisiblePosition = 0;
                        colMasterFundID.Header.Appearance.TextHAlign = HAlign.Center;
                        colAccountID.Header.VisiblePosition = 1;
                    }

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyCreditLimit))
                {
                    UltraGridColumn colAccountID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    colAccountID.ValueList = GetAccountsValueList();
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccountID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccountID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Width = 150;
                    colAccountID.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountID.Header.VisiblePosition = 0;
                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }

                    UltraGridColumn colLongDebitLimit = grdPivotDisplay.DisplayLayout.Bands[0].Columns["LongDebitLimit"];
                    colLongDebitLimit.Header.Caption = "Long Debit Limit";
                    colLongDebitLimit.Header.Appearance.TextHAlign = HAlign.Center;

                    UltraGridColumn colShortCreditLimit = grdPivotDisplay.DisplayLayout.Bands[0].Columns["ShortCreditLimit"];
                    colShortCreditLimit.Header.Caption = "Short Credit Limit";
                    colShortCreditLimit.Header.Appearance.TextHAlign = HAlign.Center;

                    UltraGridColumn colLongDebitBalance = grdPivotDisplay.DisplayLayout.Bands[0].Columns["LongDebitBalance"];
                    colLongDebitBalance.Header.Caption = "Long Debit Balance";
                    colLongDebitBalance.Header.Appearance.TextHAlign = HAlign.Center;

                    UltraGridColumn colShortCreditBalance = grdPivotDisplay.DisplayLayout.Bands[0].Columns["ShortCreditBalance"];
                    colShortCreditBalance.Header.Caption = "Short Credit Balance";
                    colShortCreditBalance.Header.Appearance.TextHAlign = HAlign.Center;
                }
                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    UltraGridColumn colAccountID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    colAccountID.ValueList = GetAccountsValueList();
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccountID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccountID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountID.Width = 150;
                    colAccountID.Header.VisiblePosition = 1;

                    UltraGridColumn colLocalCurrency = grdPivotDisplay.DisplayLayout.Bands[0].Columns["LocalCurrencyID"];
                    colLocalCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colLocalCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colLocalCurrency.ValueList = cmbLocalCurrency;
                    colLocalCurrency.Header.Caption = "Local Currency";
                    colLocalCurrency.Header.Appearance.TextHAlign = HAlign.Center;
                    cmbLocalCurrency.DisplayLayout.Bands[0].Columns["Symbol"].Width = 100;
                    colLocalCurrency.Width = 100;
                    colLocalCurrency.Header.VisiblePosition = 2;

                    UltraGridColumn colCashValueLocal = grdPivotDisplay.DisplayLayout.Bands[0].Columns["CashValueLocal"];
                    colCashValueLocal.Header.Caption = "Cash Value Local";
                    colCashValueLocal.Header.Appearance.TextHAlign = HAlign.Center;
                    colCashValueLocal.Format = "0.0000";
                    colCashValueLocal.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colCashValueLocal.Header.VisiblePosition = 3;

                    UltraGridColumn colBaseCurrency = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BaseCurrencyID"];
                    colBaseCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colBaseCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBaseCurrency.ValueList = cmbBaseCurrency;
                    colBaseCurrency.Header.Caption = "Base Currency";
                    colBaseCurrency.Header.Appearance.TextHAlign = HAlign.Center;
                    cmbBaseCurrency.DisplayLayout.Bands[0].Columns["Symbol"].Width = 100;
                    colBaseCurrency.Width = 100;
                    colBaseCurrency.CellActivation = Activation.NoEdit;
                    colBaseCurrency.Header.VisiblePosition = 4;

                    UltraGridColumn colCashValueBase = grdPivotDisplay.DisplayLayout.Bands[0].Columns["CashValueBase"];
                    colCashValueBase.Header.Caption = "Cash Value Base";
                    colCashValueBase.Format = "0.0000";
                    colCashValueBase.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colCashValueBase.Header.Appearance.TextHAlign = HAlign.Center;
                    colCashValueBase.Header.VisiblePosition = 5;
                    colBaseCurrency.CellActivation = Activation.ActivateOnly;

                    UltraGridColumn holcol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Date"];
                    holcol.Header.Appearance.TextHAlign = HAlign.Center;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        holcol.Hidden = true;
                    }
                    else
                    {
                        holcol.Hidden = false;
                    }
                    UltraGridColumn colCashCurrencyID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["CashCurrencyID"];
                    colCashCurrencyID.Hidden = true;
                    colCashCurrencyID.Header.Appearance.TextHAlign = HAlign.Center;
                }
                else if (_tabPageSelected.Equals(TabName_CollateralInterest))
                {
                    UltraGridColumn colAccountID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    colAccountID.ValueList = GetAccountsValueList();
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccountID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccountID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountID.Width = 150;
                    colAccountID.Header.VisiblePosition = 1;

                    UltraGridColumn colBenchmarkName = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BenchmarkName"];
                    colBenchmarkName.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBenchmarkName.Header.Caption = "Benchmark Name";
                    colBenchmarkName.Header.Appearance.TextHAlign = HAlign.Center;
                    colBenchmarkName.Width = 150;
                    colBenchmarkName.Header.VisiblePosition = 2;
                    colBenchmarkName.CellActivation = Activation.AllowEdit;

                    UltraGridColumn colBenchmarkRate = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BenchmarkRate"];
                    colBenchmarkRate.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBenchmarkRate.Header.Caption = "Benchmark Rate";
                    colBenchmarkRate.Header.Appearance.TextHAlign = HAlign.Center;
                    colBenchmarkRate.Width = 150;
                    colBenchmarkRate.Header.VisiblePosition = 3;

                    UltraGridColumn colSpread = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Spread"];
                    colSpread.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colSpread.Header.Caption = "Spread";
                    colSpread.Header.Appearance.TextHAlign = HAlign.Center;
                    colSpread.Width = 150;
                    colSpread.Header.VisiblePosition = 4;

                    UltraGridColumn holcol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Date"];
                    holcol.Header.Appearance.TextHAlign = HAlign.Center;
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        holcol.Hidden = true;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Beta))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_TradingVol))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.CellActivation = Activation.NoEdit;

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_PerformanceNumbers))
                {
                    UltraGridColumn colAccountID = grdPivotDisplay.DisplayLayout.Bands[0].Columns["FundID"];
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.ValueList = cmbAccount;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Header.Appearance.TextHAlign = HAlign.Center;
                    colAccountID.Header.VisiblePosition = 1;
                    colAccountID.Width = 125;

                    UltraGridColumn colMTDValue = grdPivotDisplay.DisplayLayout.Bands[0].Columns["MTDValue"];
                    colMTDValue.Header.Caption = "MTD P&L";
                    colMTDValue.Format = "0.0000";
                    colMTDValue.Header.Appearance.TextHAlign = HAlign.Center;
                    colMTDValue.Width = 125;
                    colMTDValue.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colMTDValue.Header.VisiblePosition = 2;

                    UltraGridColumn colQTDValue = grdPivotDisplay.DisplayLayout.Bands[0].Columns["QTDValue"];
                    colQTDValue.Header.Caption = "QTD P&L";
                    colQTDValue.Format = "0.0000";
                    colQTDValue.Width = 125;
                    colQTDValue.Header.Appearance.TextHAlign = HAlign.Center;
                    colQTDValue.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colQTDValue.Header.VisiblePosition = 3;

                    UltraGridColumn colYTDValue = grdPivotDisplay.DisplayLayout.Bands[0].Columns["YTDValue"];
                    colYTDValue.Header.Caption = "YTD P&L";
                    colYTDValue.Format = "0.0000";
                    colYTDValue.Width = 125;
                    colYTDValue.Header.Appearance.TextHAlign = HAlign.Center;
                    colYTDValue.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colYTDValue.Header.VisiblePosition = 4;

                    UltraGridColumn colMTDReturn = grdPivotDisplay.DisplayLayout.Bands[0].Columns["MTDReturn"];
                    colMTDReturn.Header.Caption = "MTD Return";
                    colMTDReturn.Format = "0.00";
                    colMTDReturn.Width = 125;
                    colMTDReturn.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colMTDReturn.Header.Appearance.TextHAlign = HAlign.Center;
                    colMTDReturn.Header.VisiblePosition = 5;

                    UltraGridColumn colQTDReturn = grdPivotDisplay.DisplayLayout.Bands[0].Columns["QTDReturn"];
                    colQTDReturn.Header.Caption = "QTD Return";
                    colQTDReturn.Format = "0.00";
                    colQTDReturn.Width = 125;
                    colQTDReturn.Header.Appearance.TextHAlign = HAlign.Center;
                    colQTDReturn.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colQTDReturn.Header.VisiblePosition = 6;

                    UltraGridColumn colYTDReturn = grdPivotDisplay.DisplayLayout.Bands[0].Columns["YTDReturn"];
                    colYTDReturn.Header.Caption = "YTD Return";
                    colYTDReturn.Format = "0.00";
                    colYTDReturn.Width = 125;
                    colYTDReturn.Header.Appearance.TextHAlign = HAlign.Center;
                    colYTDReturn.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colYTDReturn.Header.VisiblePosition = 7;

                    UltraGridColumn colDate = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Date"];
                    colDate.Header.Appearance.TextHAlign = HAlign.Center;
                    colDate.Hidden = true;
                }
                else if (_tabPageSelected.Equals(TabName_Delta))
                {
                    // Symbol Was Editable For this Tab. So Making it Uneditable
                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Outstanding))
                {
                    // Symbol Was Editable For this Tab. So Making it Uneditable
                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyVolatility))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyDividendYield))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_VWAP))
                {
                    grdPivotDisplay.DisplayLayout.UseFixedHeaders = true;

                    UltraGridColumn colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Header.Appearance.TextHAlign = HAlign.Center;
                    colSymbol.CellActivation = Activation.NoEdit;

                    UltraGridColumn colBloomberg = grdPivotDisplay.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.Header.Appearance.TextHAlign = HAlign.Center;
                    colBloomberg.CellActivation = Activation.NoEdit;
                    colBloomberg.Header.Caption = "Bloomberg Symbol";

                    foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        column.Header.Appearance.TextHAlign = HAlign.Center;
                    }
                }

                grdPivotDisplay.DisplayLayout.Appearance.BackColor = Color.Black;
                grdPivotDisplay.DisplayLayout.Bands[0].Override.RowAppearance.ForeColor = Color.White;
                grdPivotDisplay.DisplayLayout.Bands[0].Override.RowAppearance.BackColor = Color.Black;
                grdPivotDisplay.DisplayLayout.Bands[0].Override.SelectedRowAppearance.ForeColor = Color.White;
                grdPivotDisplay.DisplayLayout.Bands[0].Override.SelectedRowAppearance.BackColor = Color.Black;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtDateMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _datePrevious = _dateSelected;
                _dateforNAVLockValidation = _dateSelected;

                _dateTypedIndtDateMonth = true;
                //If there is some change done by the user before changing the date time selector.
                bool result = IsDataSourceChanged();

                //Depending upon the result value of the date time selector is changed. 
                //If some changes are done by the user.
                if (result.Equals(true))
                {
                    //Asking user about saving/not saving/cancelling the data before changing the time selector value.
                    DialogResult dlgResult = AskSaveConfirmation();
                    if (dlgResult.Equals(DialogResult.Yes))
                    {
                        //Fire the event for saving data by giving notification to the control's parent form.
                        if (ConfirmationSaveClicked != null)
                        {
                            ConfirmationSaveClicked(this, EventArgs.Empty);
                        }
                        dtDateMonth.CloseUp();
                        _dateSelected = (DateTime)dtDateMonth.Value;
                    }
                    else if (dlgResult.Equals(DialogResult.No))
                    {
                        dtDateMonth.CloseUp();
                    }
                    else
                    {
                        this.dtDateMonth.ValueChanged -= new System.EventHandler(this.dtDateMonth_ValueChanged);
                        this.dtDateMonth.AfterCloseUp -= new System.EventHandler(this.dtDateMonth_AfterCloseUp);
                        this.dtDateMonth.Value = _datePrevious;
                        this.dtDateMonth.CloseUp();
                        _dateTypedIndtDateMonth = false;
                        this.dtDateMonth.ValueChanged += new System.EventHandler(this.dtDateMonth_ValueChanged);
                        this.dtDateMonth.AfterCloseUp += new System.EventHandler(this.dtDateMonth_AfterCloseUp);
                    }
                }
                _dateforNAVLockValidation = (DateTime)dtDateMonth.Value;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtDateMonth_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_dateTypedIndtDateMonth == true)
                {
                    _dateTypedIndtDateMonth = false;
                    BindGrid();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtLiveFeed.Value = _dateSelected;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (optDaily.Checked.Equals(true))
                {
                    //If there is some change done by the user before changing the Daily option button.
                    bool result = IsDataSourceChanged();
                    //Depending upon the result value of the Daily option button is changed. 
                    //If some changes are done by the user.

                    ControlsVisibilityChange(true);

                    if (result.Equals(true) && !_tabChanged)
                    {
                        //Asking user about saving/not saving/cancelling the data before changing the Daily option button.
                        DialogResult dlgResult = AskSaveConfirmation();
                        if (dlgResult.Equals(DialogResult.Yes))
                        {
                            //Fire the event for saving data by giving notification to the control's parent form.
                            if (ConfirmationSaveClicked != null)
                            {
                                ConfirmationSaveClicked(this, EventArgs.Empty);
                            }
                            _methodologySelected = MethdologySelected.Daily;
                            BindGridForSelectedTab(_tabPageSelected);
                        }
                        else if (dlgResult.Equals(DialogResult.Cancel))
                        {
                            optDaily.Checked = false;
                            this.optMonthly.CheckedChanged -= new System.EventHandler(this.optMonthly_CheckedChanged);
                            optMonthly.Checked = true;
                            this.optMonthly.CheckedChanged += new System.EventHandler(this.optMonthly_CheckedChanged);
                            ControlsVisibilityChange(false);
                        }
                        else
                        {
                            _methodologySelected = MethdologySelected.Daily;
                            BindGridForSelectedTab(_tabPageSelected);
                        }
                    }
                    else
                    {
                        _methodologySelected = MethdologySelected.Daily;
                        BindGridForSelectedTab(_tabPageSelected);

                        DateTime dtToBePassed = (DateTime)dtLiveFeed.Value;
                        ScrollToSelectedDate(dtToBePassed);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //bool _dataSourceTableChanged = false;
        private void optMonthly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (optMonthly.Checked.Equals(true))
                {
                    ControlsVisibilityChange(false);

                    //If there is some change done by the user before changing the Monthly option button.
                    bool result = IsDataSourceChanged();
                    //Depending upon the result value of the Monthly option button is changed. 
                    //If some changes are done by the user.
                    if (result.Equals(true) && !_tabChanged)
                    {
                        //Asking user about saving/not saving/cancelling the data before changing the Monthly option button.
                        DialogResult dlgResult = AskSaveConfirmation();
                        if (dlgResult.Equals(DialogResult.Yes))
                        {
                            //Fire the event for saving data by giving notification to the control's parent form.
                            if (ConfirmationSaveClicked != null)
                            {
                                ConfirmationSaveClicked(this, EventArgs.Empty);
                            }
                            _methodologySelected = MethdologySelected.Monthly;
                            BindGridForSelectedTab(_tabPageSelected);
                        }
                        else if (dlgResult.Equals(DialogResult.Cancel))
                        {
                            optMonthly.Checked = false;
                            this.optDaily.CheckedChanged -= new System.EventHandler(this.optDaily_CheckedChanged);
                            optDaily.Checked = true;
                            this.optDaily.CheckedChanged += new System.EventHandler(this.optDaily_CheckedChanged);
                            ControlsVisibilityChange(true);
                        }
                        else
                        {
                            _methodologySelected = MethdologySelected.Monthly;
                            BindGridForSelectedTab(_tabPageSelected);
                        }
                    }
                    else
                    {
                        _methodologySelected = MethdologySelected.Monthly;
                        BindGridForSelectedTab(_tabPageSelected);

                        DateTime dtToBePassed = (DateTime)dtLiveFeed.Value;
                        ScrollToSelectedDate(dtToBePassed);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DialogResult AskSaveConfirmation()
        {
            DialogResult dlgResult = DialogResult.Yes;
            dlgResult = ConfirmationMessageBox.Display("Do you want to save the changes done in the data?", "CONFIRMATION");
            return dlgResult;
        }
        /// <summary>
        /// Controls Visibility Change
        /// </summary>
        /// <param name="isVisible"></param>
        private void ControlsVisibilityChange(bool isVisible)
        {
            btnFetchData.Visible = isVisible;
            lblCopyFromDate.Visible = isVisible;
            dtCopyFromDate.Visible = isVisible;

            if (isVisible)
                dtDateMonth.MaskInput = "{LOC}mm/dd/yyyy";
            else
                dtDateMonth.MaskInput = "{LOC}mm/yyyy";
        }

        public bool IsDataSourceChanged()
        {
            grdPivotDisplay.UpdateData();
            bool result = false;

            DataTable currentTable = grdPivotDisplay.DataSource as DataTable;
            if (currentTable == null)
            {
                return false;
            }
            if (currentTable.DataSet == null)
            {
                //Checking for the every row in the grid if it is modified by user or not.
                foreach (DataRow dRow in currentTable.Rows)
                {
                    if (dRow.RowState == DataRowState.Added || dRow.RowState == DataRowState.Detached || dRow.RowState == DataRowState.Modified || dRow.RowState == DataRowState.Deleted)
                    {
                        result = true;
                        //dRow.AcceptChanges();
                        break;
                    }
                }
                return result;
            }
            else
            {
                if (_isDataCopiedForex == true) { _isDataCopiedForex = false; return true; }
                return currentTable.DataSet.HasChanges();
            }
        }

        //Property which sends the data back to the parent form when accessed after validation.
        public DataTable GetDataToSave()
        {
            try
            {
                if (_tabPageSelected.Equals(TabName_MarkPrice) || _tabPageSelected.Equals(TabName_FXMarkPrice))
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        int rowIndex = 1;
                        bool isValidatedData = true;
                        if (!ValidateDateWithNAVLock())
                            return null;

                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            foreach (DataColumn dCol in _dtGridDataSource.Columns)
                            {
                                if (!dCol.ColumnName.Equals("Symbol") && !dCol.ColumnName.Equals("AUECID") && !dCol.ColumnName.Equals("AUECIdentifier") && !dCol.ColumnName.Equals("BloombergSymbol") && !dCol.ColumnName.Equals("Account") && !dCol.ColumnName.Equals("AccountID"))
                                {
                                    //If mark price field is blank.
                                    if (!dRow["Symbol"].ToString().Equals("") && dRow[dCol.ColumnName].ToString().Equals(""))
                                    {
                                        isValidatedData = false;
                                        InformationMessageBox.Display("Please enter the mark price for Symbol '" + dRow["Symbol"], "Mark Price Save");
                                        break;
                                    }
                                }
                            }
                            rowIndex++;
                            if (dtDateMonth.Value != dtCopyFromDate.Value && (dRow.RowState == DataRowState.Unchanged && _isDataCopiedFromBackDate) && (!dRow[6].ToString().Equals("0") || !dRow["ForwardPoints"].ToString().Equals("0")))
                                dRow.SetModified();
                        }
                        if (isValidatedData.Equals(true))
                        {
                            SetDefaultGridAppreance(_colName);
                            DataTable dtChanges = ((DataTable)grdPivotDisplay.DataSource).GetChanges();
                            _dtGridDataSource.AcceptChanges();
                            return dtChanges;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        if (!ValidateDateWithNAVLock())
                            return null;
                        bool isValidated = CheckValidationForForex();
                        if (isValidated)
                        {
                            SetDefaultGridAppreance(_colName);
                            DataTable dtChanges = ((DataTable)grdPivotDisplay.DataSource).GetChanges();

                            return dtChanges;
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_NAV) || _tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount) || _tabPageSelected.Equals(TabName_UserDefinedMTDPnL) || _tabPageSelected.Equals(TabName_PerformanceNumbers) || _tabPageSelected.Equals(TabName_DailyCreditLimit))
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        if (_tabPageSelected.Equals(TabName_NAV) && !ValidateDateWithNAVLock())
                            return null;
                        _dtGridDataSource.AcceptChanges();
                        SetDefaultGridAppreance(_colName);
                        return (DataTable)grdPivotDisplay.DataSource;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_CollateralInterest))
                {
                    bool isValidated = CheckValidationForDailyCashTab();
                    if (isValidated)
                    {
                        _dtGridDataSource.AcceptChanges();
                        SetDefaultGridAppreance(_colName);
                        return (DataTable)grdPivotDisplay.DataSource;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    if (!ValidateDateWithNAVLock())
                        return null;
                    bool isValidated = CheckValidationForDailyCashTab();
                    if (isValidated)
                    {
                        _dtGridDataSource.AcceptChanges();
                        SetDefaultGridAppreance(_colName);
                        return (DataTable)grdPivotDisplay.DataSource;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_Delta) || _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Outstanding) || _tabPageSelected.Equals(TabName_DailyVolatility) || _tabPageSelected.Equals(TabName_DailyDividendYield) || _tabPageSelected.Equals(TabName_VWAP))
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        DataTable dtBeta = new DataTable();
                        dtBeta = (DataTable)grdPivotDisplay.DataSource;
                        dtBeta.AcceptChanges();
                        _dtGridDataSource.AcceptChanges();
                        SetDefaultGridAppreance(_colName);
                        return (DataTable)grdPivotDisplay.DataSource;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_CollateralPrice))
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        SetDefaultGridAppreance(_colName);
                        DataTable dtChanges = ((DataTable)grdPivotDisplay.DataSource).GetChanges();
                        _dtGridDataSource.AcceptChanges();
                        if (_isDataCopiedFromBackDate)
                            return (DataTable)grdPivotDisplay.DataSource;
                        else
                            return dtChanges;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        //private bool CheckValidationForPerformanceNumberTab()
        //{
        //    try
        //    {
        //        DataTable dtPerformanceNumber = new DataTable();
        //        dtPerformanceNumber = (DataTable)grdPivotDisplay.DataSource;
        //        dtPerformanceNumber.AcceptChanges();
        //        if (dtPerformanceNumber.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtPerformanceNumber.Rows.Count; i++)
        //            {
        //                dtPerformanceNumber.Rows[i]["Date"] = _dateSelected;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return true;
        //}

        private bool CheckValidationForForex()
        {
            try
            {
                int rowIndex = 0;
                DataTable dtForexConversion = new DataTable();
                dtForexConversion = (DataTable)grdPivotDisplay.DataSource;
                if (dtForexConversion.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtForexConversion.Rows)
                    {
                        foreach (DataColumn dc in dtForexConversion.Columns)
                        {
                            if (dc.ColumnName != "FromCurrencyID" && dc.ColumnName != "ToCurrencyID" && dc.ColumnName != "Symbol" && dc.ColumnName != "Summary" && dc.ColumnName != "AccountID")
                            {
                                if (dr["FromCurrencyID"] != System.DBNull.Value && dr["ToCurrencyID"] != System.DBNull.Value)
                                {
                                    if (int.Parse(dr["FromCurrencyID"].ToString()) != 0 && int.Parse(dr["ToCurrencyID"].ToString()) != 0)
                                    {
                                        if (dr[dc.ColumnName] == System.DBNull.Value)
                                        {
                                            InformationMessageBox.Display("Please fill the value in the date column for each Forex Conversion Factor in the row - " + (rowIndex + 1), "Forex Conversion");
                                            return false;
                                        }
                                        else if (String.IsNullOrEmpty(dr["Symbol"].ToString()))
                                        {
                                            InformationMessageBox.Display("Please fill the value of Symbol in the row - " + (rowIndex + 1), "Forex Conversion");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        rowIndex = rowIndex + 1;
                        if (dtDateMonth.Value != dtCopyFromDate.Value && (dr.RowState == DataRowState.Unchanged && _isDataCopiedFromBackDate))
                            dr.SetModified();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // File open dialog, ask user to import mark prices.
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
                openFileDialog1.Title = "Select file for import";
                openFileDialog1.Filter = "Text Files (*.xls)|*.xls";
                string strFileName = string.Empty;
                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {
                    strFileName = openFileDialog1.FileName;
                }
                else if (importResult == DialogResult.Cancel)
                {
                    InformationMessageBox.Display("Operation cancelled by User", "Mark Price Import");
                    return;
                }
                UpdationAfterImporting(strFileName);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Method used to update the values in the grid after importing mark prices from the file.
        private void UpdationAfterImporting(string filePath)
        {
            string strFileName = filePath;
            DataSet result = new DataSet();

            ExcelDataReader spreadSheet = null;
            //string fileName = string.Empty;
            FileStream fs = null;
            try
            {
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                WarningMessageBox.Display("File is in use. Close file and retry", "Mark Price Import");
                return;
            }
            try
            {
                spreadSheet = new ExcelDataReader(fs);

                if (spreadSheet != null && spreadSheet.WorkbookData != null && spreadSheet.WorkbookData.Tables != null && spreadSheet.WorkbookData.Tables.Count != 0)
                {
                    // create a new table whose columns will be in proper organized according to our requirement
                    DataTable tableMarkPriceImported = new DataTable();
                    // result is a Dataset , get the excel sheet values into the Dataset named result
                    result = spreadSheet.WorkbookData;
                    // copy all the record from result Dataset to the Table dt
                    tableMarkPriceImported = result.Tables[0].Copy();

                    // now rename the columns ot the Table, 
                    // xml create a row of Headers of the excel sheet, so 1st row will give the headers
                    // of the table so rename the columns
                    for (int j = 0; j < result.Tables[0].Columns.Count; j++)
                    {
                        tableMarkPriceImported.Columns[j].ColumnName = result.Tables[0].Rows[0].ItemArray[j].ToString();
                    }
                    // delete the 1st row of the table because columns has given the same name as the headers
                    tableMarkPriceImported.Rows[0].Delete();
                    tableMarkPriceImported.TableName = "TableMarkPriceImported";


                    DataTable tableGridMarkPrices = _dtGridDataSource;
                    //string colName = string.Empty;
                    //DateTime colParesedDate = DateTime.Now;
                    DateTime dateImported = (DateTime)dtLiveFeed.Value;

                    //string importedDateColumn = dateImported.ToString("MM/dd/yyyy");
                    //If grid already have some rows.
                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Looping through the grid to update values in the grid from the imported data.
                        foreach (DataRow rowTableGridMarkPrices in tableGridMarkPrices.Rows)
                        {
                            foreach (DataRow rowTableMarkPriceImported in tableMarkPriceImported.Rows)
                            {
                                //int colLen = rowTableGridMarkPrices.Table.Columns.Count;
                                //colName = rowTableGridMarkPrices.Table.Columns[colLen - 1].ColumnName;
                                foreach (DataColumn col in tableGridMarkPrices.Columns)
                                {
                                    DateTime dateParsedOut = DateTime.Now;
                                    if (DateTime.TryParse(col.ToString(), out dateParsedOut))
                                    {
                                        //If the symbols match i.e. in the grid and in the imported data.
                                        if (dateParsedOut.Equals(dateImported) && string.Compare(rowTableGridMarkPrices["Symbol"].ToString().Trim().ToUpper(), rowTableMarkPriceImported["Symbol"].ToString().Trim().ToUpper(), true) == 0 && string.Compare(rowTableGridMarkPrices["Account"].ToString().Trim(), rowTableMarkPriceImported["Account"].ToString().Trim(), true) == 0)
                                        {
                                            rowTableGridMarkPrices[col] = rowTableMarkPriceImported["MarkPrice"];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    WarningMessageBox.Display("Data file should have 3 columns with Header 'Symbol', 'Account' and 'MarkPrice'. \n Please select the correct file and try again.", "Mark Price Import");
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private void grdPivotDisplay_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        //Checking if the to and from currency are not the same and assigning the forex conversion rate to the
        //column when some To_currency and From_Currency is changed.
        private void grdPivotDisplay_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    string ColumnKey = e.Cell.Column.Key;
                    bool result = ValidateRowForMarkPrice(ColumnKey);
                    string symbol = grdPivotDisplay.ActiveRow.Cells["Symbol"].Text;
                    if (result.Equals(true))
                    {
                        e.Cell.CancelUpdate();
                    }
                    if (optMonthly.Checked == true)
                    {
                        if (_symbolWiseChangedDatesForMarkPrice == null)
                            _symbolWiseChangedDatesForMarkPrice = new Dictionary<string, List<DateTime>>();
                        if (result.Equals(false) && !string.IsNullOrEmpty(symbol))
                        {
                            if (!_symbolWiseChangedDatesForMarkPrice.ContainsKey(symbol))
                            {
                                _symbolWiseChangedDatesForMarkPrice.Add(symbol, new List<DateTime>());
                                _symbolWiseChangedDatesForMarkPrice[symbol].Add(Convert.ToDateTime(ColumnKey));
                            }
                            else
                            {
                                _symbolWiseChangedDatesForMarkPrice[symbol].Add(Convert.ToDateTime(ColumnKey));
                            }
                        }
                    }
                }

                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    if (e.Cell.Column.Key.Equals("CashValueBase"))
                    {
                        _blnCashValueBaseUpdated = true;
                    }
                    else
                    {
                        _blnCashValueBaseUpdated = false;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_NAV))
                {
                    // DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    string column = e.Cell.Column.Key;
                    if (column.Equals("FundID"))
                    {
                        isDataValidToSaveForNav = ValidateRowForNAV();
                        if (!isDataValidToSaveForNav)
                        {
                            e.Cell.CancelUpdate();
                        }

                    }

                    if (column.Equals("MasterFund"))
                    {
                        int masterFundId = Convert.ToInt32(e.Cell.Column.ValueList.GetValue(e.Cell.Column.ValueList.SelectedItemIndex));
                        var accountList = GetAccountsValueList(masterFundId);
                        if (accountList != null && accountList.ValueListItems.Count > 0)
                        {

                            int accountId = GetAccountNotExistsForMasterFund(accountList);
                            if (accountId > 0)
                            {
                                grdPivotDisplay.ActiveRow.Cells["FundID"].ValueList = accountList;
                                grdPivotDisplay.ActiveRow.Cells["FundID"].SetValue(accountId, false);
                            }
                            else
                            {
                                InformationMessageBox.Display("All Accounts already exists,please select different one.", "NAV");
                                e.Cell.CancelUpdate();
                            }

                        }

                    }
                    e.Cell.Appearance.ForeColor = Color.Red;
                }
                else if (_tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount))
                {
                    // DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    string column = e.Cell.Column.Key;
                    if (column.Equals("FundID"))
                    {
                        isDataValidToSaveForStartOfMonthCapitalAccount = ValidateRowForStartOfMonthCapitalAccount();
                        if (!isDataValidToSaveForStartOfMonthCapitalAccount)
                        {
                            e.Cell.CancelUpdate();
                        }
                    }
                    e.Cell.Appearance.ForeColor = Color.Red;
                }
                else if (_tabPageSelected.Equals(TabName_UserDefinedMTDPnL))
                {
                    //DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    string column = e.Cell.Column.Key;
                    if (column.Equals("FundID"))
                    {
                        isDataValidToSaveForUserDefinedMTDPnL = ValidateRowForUserDefinedMTDPnL();
                        if (!isDataValidToSaveForUserDefinedMTDPnL)
                        {
                            e.Cell.CancelUpdate();
                        }
                    }
                    e.Cell.Appearance.ForeColor = Color.Red;
                }
                else if (_tabPageSelected.Equals(TabName_DailyCreditLimit))
                {
                    DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                    string column = e.Cell.Column.Key;
                    if (column.Equals("FundID"))
                    {
                        isDataValidToSaveForDailyCreditLimit = ValidateRowForDailyCreditLimit();
                        if (!isDataValidToSaveForDailyCreditLimit)
                        {
                            e.Cell.CancelUpdate();
                        }
                    }
                    e.Cell.Appearance.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //PRANA-13689
        //KashishG.,2016-05-10
        private static bool hasCellChanged(DataRow row, DataColumn col, string grdText)
        {
            if (grdText.Equals("DataCopied") && (col.ColumnName != "FromCurrencyID" && col.ColumnName != "ToCurrencyID" && col.ColumnName != "Symbol" && col.ColumnName != "Summary" && col.ColumnName != "Account"))
            {
                return true;
            }
            if (!row.HasVersion(DataRowVersion.Original))
            {
                // Row has been added. All columns have changed. 
                return true;
            }
            if (!row.HasVersion(DataRowVersion.Current))
            {
                // Row has been removed. No columns have changed.
                return false;
            }
            var originalVersion = row[col, DataRowVersion.Original];
            var currentVersion = row[col, DataRowVersion.Current];
            if (originalVersion == DBNull.Value && currentVersion == DBNull.Value)
            {
                return false;
            }
            else if (originalVersion != DBNull.Value && currentVersion != DBNull.Value)
            {
                return !originalVersion.Equals(currentVersion);
            }
            return true;
        }

        bool _blnCashValueBaseUpdated = false;
        //Updating the value for coversion rate to the column depending upon the currency choosen.
        private void grdPivotDisplay_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                e.Cell.Appearance.ForeColor = Color.Red;
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                }
                else if (_tabPageSelected.Equals(TabName_FXMarkPrice))
                {
                    UltraGridRow activeRow = grdPivotDisplay.ActiveRow;
                    if (activeRow.Cells["ForwardPoints"].Value.ToString().Equals(string.Empty))
                    {
                        activeRow.Cells["ForwardPoints"].Value = activeRow.Cells["ForwardPoints"].OriginalValue;
                    }
                    if (activeRow.Cells[8].Value.ToString().Equals(string.Empty))
                    {
                        activeRow.Cells[8].Value = activeRow.Cells[8].OriginalValue;
                    }
                    double forwardPoints = Convert.ToDouble(activeRow.Cells["ForwardPoints"].Value.ToString());
                    double fxrate = Convert.ToDouble(activeRow.Cells["FxRate"].Value.ToString());
                    //string dateKey = string.Empty;
                    double markPrice = 0.0;

                    markPrice = Convert.ToDouble(activeRow.Cells[8].Value.ToString());
                    if (e.Cell.Column.Key.Equals("ForwardPoints"))
                    {
                        grdPivotDisplay.AfterCellUpdate -= new CellEventHandler(grdPivotDisplay_AfterCellUpdate);
                        activeRow.Cells[8].Value = Convert.ToDecimal(forwardPoints) + Convert.ToDecimal(fxrate);
                        grdPivotDisplay.AfterCellUpdate += new CellEventHandler(grdPivotDisplay_AfterCellUpdate);
                    }
                    DateTime dateOut = DateTime.MinValue;
                    if (DateTime.TryParse(e.Cell.Column.Key, out dateOut))
                    {
                        grdPivotDisplay.AfterCellUpdate -= new CellEventHandler(grdPivotDisplay_AfterCellUpdate);
                        double fwdPoints = Convert.ToDouble(Convert.ToDecimal(markPrice) - Convert.ToDecimal(fxrate));
                        activeRow.Cells["ForwardPoints"].Value = fwdPoints;
                        grdPivotDisplay.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPivotDisplay_AfterCellUpdate);
                    }
                }
                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    UltraGridRow activeRow = grdPivotDisplay.ActiveRow;

                    string fromCurrency = activeRow.Cells["FromCurrencyID"].Text;
                    string toCurrency = activeRow.Cells["ToCurrencyID"].Text;

                    if (optDaily.Checked)
                    {
                        if ((!String.IsNullOrEmpty(fromCurrency) && !fromCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!String.IsNullOrEmpty(toCurrency) && !toCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)))
                        {
                            if (activeRow.Cells[5].Value != System.DBNull.Value && Convert.ToDouble(activeRow.Cells[5].Value) > 0)
                            {
                                activeRow.Cells["Summary"].Value = "1 " + fromCurrency + " = " + Convert.ToDouble(activeRow.Cells[5].Value) + " " + toCurrency;
                            }
                            else
                            {
                                activeRow.Cells["Summary"].Value = "";
                            }
                        }
                        else
                        {
                            activeRow.Cells["Summary"].Value = "";
                        }
                    }
                    else if (!optMonthly.Checked)
                    {
                        e.Cell.CancelUpdate();
                        return;
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyCash))
                {
                    int result;
                    if (e.Cell.Column.Key.Equals("FundID"))
                    {
                        if (int.TryParse(e.Cell.Text, out result))
                        {
                            e.Cell.Value = DBNull.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(grdPivotDisplay.ActiveRow.Cells["FundID"].Text))
                    {
                        DailyCash_AfterCellUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool ValidateRowForMarkPrice(string columnKey)
        {
            bool result = false;
            try
            {
                if (columnKey.Equals("Symbol"))
                {
                    string symbol = grdPivotDisplay.ActiveRow.Cells["Symbol"].Text;
                    if (!String.IsNullOrEmpty(symbol) && !symbol.StartsWith("$"))
                    {
                        InformationMessageBox.Display("Index Symbol should start with $.", "Mark Price");
                        return true;
                    }

                    int currentIndex = grdPivotDisplay.ActiveRow.Index;
                    int checkIndex = 0;

                    //If the same Symbol already exists.
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdPivotDisplay.Rows)
                    {
                        string drSymbol = dr.Cells["Symbol"].Text;
                        checkIndex = dr.Index;
                        if (!String.IsNullOrEmpty(symbol) && !String.IsNullOrEmpty(drSymbol) && symbol.Equals(drSymbol) && checkIndex != currentIndex)
                        {
                            result = true;
                            InformationMessageBox.Display("Symbol already exists,please enter different Symbol.", "Mark Price");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        void grdPivotDisplay_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in filePaths)
            {
                UpdationAfterImporting(filePath);
            }
        }

        void grdPivotDisplay_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }

        bool _btnGetLiveFeedClicked = false;
        string _colName = string.Empty;
        //Getting the live feed data for the selected date and updating in in the grid where the data for the 
        //selected is lying.
        private void btnGetLiveFeedData_Click(object sender, EventArgs e)
        {
            try
            {
                if (optUseLiveFeed.Checked)
                {
                    if (MarketDataValidation.CheckMarketDataPermissioning())
                    {
                        if (!CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.MarketDataTypes.Contains(Prana.BusinessObjects.LiveFeed.LiveFeedConstants.LevelOne))
                        {
                            InformationMessageBox.Display("Live Feed Permission is not available.");
                            return;
                        }

                        if (_tabPageSelected.Equals(TabName_ForexConversion) || _tabPageSelected.Equals(TabName_MarkPrice))
                        {
                            _accountSymbols.Clear();
                            UltraGridRow[] FilteredRows = grdPivotDisplay.Rows.GetFilteredInNonGroupByRows();
                            string dtLiveFeedPrices = dtLiveFeed.DateTime.ToString("MM/dd/yyyy");
                            int index = _dtGridDataSource.Columns.IndexOf(dtLiveFeedPrices);
                            foreach (UltraGridRow row in FilteredRows)
                            {
                                int accountID = Convert.ToInt32(row.Cells["AccountID"].Value.ToString());
                                string symbol = row.Cells["Symbol"].Value.ToString();
                                double price = 0.0;
                                if (!row.Cells[index].Text.Equals(String.Empty))
                                {
                                    price = (double)((Infragistics.Win.UltraWinGrid.UltraGridCell)(row.Cells[index])).Value;
                                }

                                if ((optOverwriteAllZeroSymbols.Checked == true && price.Equals(0.0)) || optOverwriteAllSymbols.Checked == true)
                                {
                                    if (_accountSymbols.ContainsKey(accountID))
                                    {
                                        List<String> tempSymbols = _accountSymbols[accountID];
                                        if (!tempSymbols.Contains(symbol))
                                        {
                                            tempSymbols.Add(symbol);
                                        }
                                    }
                                    else
                                    {
                                        List<String> tempSymbols = new List<string>();
                                        tempSymbols.Add(symbol);
                                        _accountSymbols.Add(accountID, tempSymbols);
                                    }
                                }
                            }
                        }
                        GetLiveFeedData();
                    }
                }
                else if (optOMIPrice.Checked)
                {
                    GetdataFromOMI();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetdataFromOMI()
        {
            try
            {
                List<string> requestedSymbols = new List<string>();
                _dtGridDataSource.AcceptChanges();
                #region Markprice
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    DateTime dtLiveFeedTime = (DateTime)dtLiveFeed.Value;
                    bool optOverwriteAllSymbolsValue = false;
                    if (optOverwriteAllSymbols.Checked)
                    {
                        optOverwriteAllSymbolsValue = true;
                    }
                    //string keyExchangeGroup = cmbExchangeGroup.Value.ToString(); ;
                    //string symbolAUECIdentifier = string.Empty;
                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Sending request for all the symbols if the option for all symbols is selected.
                        if (optOverwriteAllSymbolsValue.Equals(true))
                        {
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        foreach (UltraGridRow filteredRow in this.grdPivotDisplay.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                        {
                                            symbol = filteredRow.Cells["Symbol"].Value.ToString();

                                            if (!requestedSymbols.Contains(symbol))
                                            {
                                                requestedSymbols.Add(symbol);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Where symbols have 0 value then the LiveFeed data for those symbols is requested. 
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        double markPrice = 0;
                                        foreach (UltraGridRow filteredRow in this.grdPivotDisplay.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                        {
                                            markPrice = double.Parse(filteredRow.Cells[_colName].Value.ToString());
                                            if (markPrice <= 0)
                                            {
                                                symbol = filteredRow.Cells["Symbol"].Value.ToString();
                                            }
                                            if (!requestedSymbols.Contains(symbol))
                                            {
                                                requestedSymbols.Add(symbol);
                                            }
                                        }
                                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in this.grdPivotDisplay.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                        {
                                            if (double.Parse(ulRow.Cells[_colName].Value.ToString()) <= 0)
                                            {
                                                ulRow.Cells[_colName].Appearance.ForeColor = Color.Red;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                List<UserOptModelInput> listSymbolOMIdata = _pricingServicesProxy.InnerChannel.GetDataFromOMI(requestedSymbols);
                UpdateMarkPriceAndFXDataWithOMIData(listSymbolOMIdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetLiveFeedData()
        {
            try
            {
                //Collect the symbols for fetching the data.
                List<string> requestedSymbols = new List<string>();
                Dictionary<string, fxInfo> requestedFxSymbols = new Dictionary<string, fxInfo>();

                _btnGetLiveFeedClicked = true;
                //https://jira.nirvanasolutions.com:8443/browse/PRANA-19542
                //_dtGridDataSource.AcceptChanges();

                //Requesting live feed data for the mark price tab.
                #region TabName_Delta
                if (_tabPageSelected.Equals(TabName_Delta))
                {
                    DataTable dt = ((DataTable)grdPivotDisplay.DataSource);
                    dt.AcceptChanges();
                    DateTime dtLiveFeedTime = (DateTime)dtLiveFeed.Value;
                    if (dt.Rows.Count > 0)
                    {
                        //Sending request for all the symbols if the option for all symbols is selected.                       
                        foreach (DataColumn col in dt.Columns)
                        {
                            DateTime dateOut = DateTime.Now;
                            DateTime dateGrid = DateTime.Now;
                            _colName = col.ColumnName;
                            string symbol = string.Empty;
                            AssetCategory asset = AssetCategory.None;
                            if (DateTime.TryParse(_colName, out dateOut))
                            {
                                dateGrid = dateOut;
                                if (dtLiveFeedTime.Equals(dateGrid))
                                {
                                    foreach (DataRow dRow in dt.Rows)
                                    {
                                        symbol = dRow["Symbol"].ToString();
                                        CachedDataManager.GetInstance.GetAssetIDFromAUECID(Convert.ToInt16(dRow["AUECID"].ToString()), ref asset);
                                        asset = Mapper.GetBaseAssetCategory(asset);
                                        if (!String.IsNullOrEmpty(symbol) && asset == AssetCategory.Option)
                                        {
                                            if (!requestedSymbols.Contains(symbol))
                                            {
                                                requestedSymbols.Add(symbol);
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Markprice
                else if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    DateTime dtLiveFeedTime = (DateTime)dtLiveFeed.Value;
                    bool optOverwriteAllSymbolsValue = false;
                    if (optOverwriteAllSymbols.Checked.Equals(true))
                    {
                        optOverwriteAllSymbolsValue = true;
                    }
                    //string keyExchangeGroup = cmbExchangeGroup.Value.ToString(); ;
                    //string symbolAUECIdentifier = string.Empty;

                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Sending request for all the symbols if the option for all symbols is selected.
                        if (optOverwriteAllSymbolsValue.Equals(true))
                        {
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        foreach (UltraGridRow filteredRow in this.grdPivotDisplay.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                        {
                                            symbol = filteredRow.Cells["Symbol"].Value.ToString();
                                            if (!requestedSymbols.Contains(symbol))
                                            {
                                                requestedSymbols.Add(symbol);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Where symbols have 0 value then the LiveFeed data for those symbols is requested. 
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        double markPrice = 0;
                                        foreach (UltraGridRow filteredRow in this.grdPivotDisplay.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                        {
                                            markPrice = double.Parse(filteredRow.Cells[_colName].Value.ToString());
                                            if (markPrice <= 0)
                                            {
                                                symbol = filteredRow.Cells["Symbol"].Value.ToString();
                                            }
                                            if (!requestedSymbols.Contains(symbol))
                                            {
                                                requestedSymbols.Add(symbol);
                                            }
                                        }
                                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in grdPivotDisplay.Rows)
                                        {
                                            if (double.Parse(ulRow.Cells[_colName].Value.ToString()) <= 0)
                                            {
                                                ulRow.Cells[_colName].Appearance.ForeColor = Color.Red;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Forex
                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                {
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-19542
                    //_dtGridDataSource.AcceptChanges();

                    DateTime dtLiveFeedTime = (DateTime)dtLiveFeed.Value;
                    bool optOverwriteAllSymbolsValue = false;
                    if (optOverwriteAllSymbols.Checked.Equals(true))
                    {
                        optOverwriteAllSymbolsValue = true;
                    }
                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Sending request for all the symbols if the option for all symbols is selected.
                        if (optOverwriteAllSymbolsValue.Equals(true))
                        {
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                                        {

                                            symbol = dRow["Symbol"].ToString();
                                            if (!String.IsNullOrEmpty(symbol))
                                            {
                                                if (!requestedFxSymbols.ContainsKey(symbol))
                                                {
                                                    int fromCurrencyID = int.Parse((dRow["FromCurrencyID"]).ToString());
                                                    int toCurrencyID = int.Parse((dRow["ToCurrencyID"]).ToString());

                                                    string forexSymbol = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetPranaForexSymbolFromCurrencies(fromCurrencyID, toCurrencyID);

                                                    fxInfo fxSymbolInfo = new fxInfo();
                                                    fxSymbolInfo.FromCurrencyID = fromCurrencyID;
                                                    fxSymbolInfo.ToCurrencyID = toCurrencyID;
                                                    fxSymbolInfo.PranaSymbol = forexSymbol;
                                                    fxSymbolInfo.CategoryCode = AssetCategory.Forex;
                                                    requestedFxSymbols.Add(symbol, fxSymbolInfo);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Where symbols have 0 value then the LiveFeed data for those symbols is requested.
                            foreach (DataColumn col in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        double markPrice = 0;
                                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                                        {
                                            markPrice = double.Parse(dRow[_colName].ToString());
                                            if (markPrice <= 0)
                                            {
                                                symbol = dRow["Symbol"].ToString();
                                                if (!requestedSymbols.Contains(symbol))
                                                {
                                                    requestedSymbols.Add(symbol);
                                                }
                                            }
                                        }
                                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in grdPivotDisplay.Rows)
                                        {
                                            if (double.Parse(ulRow.Cells[_colName].Value.ToString()) <= 0)
                                            {
                                                ulRow.Cells[_colName].Appearance.ForeColor = Color.Red;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region AllOtherTabs
                else if (_tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Outstanding) || _tabPageSelected.Equals(TabName_DailyVolatility) || _tabPageSelected.Equals(TabName_DailyDividendYield) || _tabPageSelected.Equals(TabName_VWAP))
                {
                    DataTable dt = ((DataTable)grdPivotDisplay.DataSource);
                    dt.AcceptChanges();
                    DateTime dtLiveFeedTime = (DateTime)dtLiveFeed.Value;
                    if (dt.Rows.Count > 0)
                    {
                        //Sending request for all the symbols if the option for all symbols is selected.
                        if (optOverwriteAllSymbols.Checked)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        foreach (DataRow dRow in dt.Rows)
                                        {
                                            symbol = dRow["Symbol"].ToString();
                                            if (!String.IsNullOrEmpty(symbol))
                                            {
                                                if (!requestedSymbols.Contains(symbol))
                                                {
                                                    requestedSymbols.Add(symbol);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Where symbols have 0 value then the LiveFeed data for those symbols is requested.
                            foreach (DataColumn col in dt.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                DateTime dateGrid = DateTime.Now;
                                _colName = col.ColumnName;
                                string symbol = string.Empty;
                                if (DateTime.TryParse(_colName, out dateOut))
                                {
                                    dateGrid = dateOut;
                                    if (dtLiveFeedTime.Equals(dateGrid))
                                    {
                                        double values = 0;
                                        foreach (DataRow dRow in dt.Rows)
                                        {
                                            values = double.Parse(dRow[_colName].ToString());
                                            if (values <= 0)
                                            {
                                                symbol = dRow["Symbol"].ToString();
                                                if (!requestedSymbols.Contains(symbol))
                                                {
                                                    requestedSymbols.Add(symbol);
                                                }
                                            }
                                        }
                                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in grdPivotDisplay.Rows)
                                        {
                                            if (double.Parse(ulRow.Cells[_colName].Value.ToString()) <= 0)
                                            {
                                                ulRow.Cells[_colName].Appearance.ForeColor = Color.Red;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                SendGetLiveFeedSnapshotMsg(requestedSymbols, new List<fxInfo>(requestedFxSymbols.Values));
                #endregion
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

        private delegate void UIThreadMarshellerGreekscalc(object sender, EventArgs<ResponseObj> e);

        void _pricingAnalysis_GreeksCalculated(object sender, EventArgs<ResponseObj> e)
        {
            try
            {
                // send to main UI thread
                UIThreadMarshellerGreekscalc mi = new UIThreadMarshellerGreekscalc(_pricingAnalysis_GreeksCalculated);
                if (UIValidation.GetInstance().validate(grdPivotDisplay))
                {
                    if (grdPivotDisplay.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { this, e });
                    }
                    else
                    {
                        if (_tabPageSelected.Equals(TabName_Delta))
                        {
                            foreach (KeyValuePair<string, OptionGreeks> item in e.Value.CalculatedGreeks)
                            {
                                //DateTime dt = (DateTime)DateTime.Now.Date;
                                string symbol = item.Key;
                                double delta = item.Value.Delta;
                                if (_colName.Equals(String.Empty))
                                {
                                    return;
                                }
                                DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);
                                if (dtSource != null)
                                {
                                    foreach (DataRow dRow in dtSource.Rows)
                                    {
                                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0)
                                        {
                                            if (delta.Equals(double.MinValue))
                                            {
                                                dRow[_colName] = 0;
                                            }
                                            else
                                            {
                                                dRow[_colName] = delta;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                _pricingAnalysis.GreeksCalculated -= new EventHandler<EventArgs<ResponseObj>>(_pricingAnalysis_GreeksCalculated);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// It refreshes the data on the exposure pnl service.
        /// </summary>
        /// <param name="subscriptionType"></param>
        public void SendGetLiveFeedSnapshotMsg(List<string> symbols, List<fxInfo> listFxSymbols)
        {
            try
            {
                if (symbols.Count > 0)
                {
                    _pricingServicesProxy.InnerChannel.RequestSnapshot(symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                }
                if (listFxSymbols.Count > 0)
                {
                    _pricingServicesProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, false, null, true);
                }
            }
            catch
            {
                MessageBox.Show("PricingService2 not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public delegate void L1ObjHandler(SymbolData level1Data);
        public delegate void L1ListObjHandler(List<SymbolData> level1Data);

        private void UpdateMarkPriceAndFXDataWithLiveFeedDataList(List<SymbolData> l1DataList)
        {
            try
            {
                foreach (SymbolData l1Data in l1DataList)
                {
                    UpdateMarkPriceAndFXDataWithLiveFeedData(l1Data);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optOMIPrice_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (optOMIPrice.Checked)
                {
                    btnGetLiveFeedData.Enabled = true;
                    grpBoxImportExport.Enabled = false;
                    grpBoxLiveFeedWhole.Enabled = true;
                    grpBoxSymbolUpdationChoice.Enabled = true;

                    grdPivotDisplay.AllowDrop = true;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    lblUpdatePrice.Enabled = false;
                    optLastPrice.Enabled = false;
                    optIMidPrice.Enabled = false;
                    optMidPrice.Enabled = false;
                    optSelectedFeedPrice.Enabled = false;
                    optPreviousPrice.Enabled = false;
                    lblIMidDate.Enabled = false;
                    lblMidDate.Enabled = false;
                    lblSelectedFeedDate.Enabled = false;
                    lblLastDate.Enabled = false;
                    lblPreviousDate.Enabled = false;
                    lblLastDate.Visible = false;
                    lblIMidDate.Visible = false;
                    lblMidDate.Visible = false;
                    lblPreviousDate.Visible = false;
                    lblSelectedFeedDate.Visible = false;
                }
                else
                {
                    grpBoxLiveFeedWhole.Enabled = true;
                    grpBoxSymbolUpdationChoice.Enabled = true;
                    grpBoxImportExport.Enabled = false;

                    grdPivotDisplay.AllowDrop = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    lblLastDate.Visible = true;
                    lblIMidDate.Visible = true;
                    lblMidDate.Visible = true;
                    lblPreviousDate.Visible = true;
                    lblSelectedFeedDate.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateMarkPriceAndFXDataWithOMIData(List<UserOptModelInput> symbolWiseOMIdata)
        {
            try
            {
                //Updating the symbol's price when tab mark price is selected.
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    foreach (UserOptModelInput userOMI in symbolWiseOMIdata)
                    {
                        // DateTime dt = (DateTime)DateTime.Now.Date;
                        string symbol = userOMI.Symbol;
                        //double price = userOMI.LastPrice;
                        //bool uselastPrice = userOMI.LastPriceUsed;

                        if (_colName.Equals(String.Empty))
                        {
                            return;
                        }
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName))
                            {
                                if (userOMI.LastPrice.Equals(double.MinValue) && userOMI.LastPriceUsed)
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    if (userOMI.LastPriceUsed)
                                    {
                                        dRow[_colName] = userOMI.LastPrice;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateMarkPriceAndFXDataWithLiveFeedData(SymbolData l1data)
        {
            try
            {
                //Updating the symbol's price when tab mark price is selected.
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    string symbol = l1data.Symbol;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }

                    //If the last price data is required for the symbol.
                    if (optLastPrice.Checked.Equals(true))
                    {
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName) && _accountSymbols.ContainsKey(Convert.ToInt32(dRow["AccountID"].ToString())) && _accountSymbols[Convert.ToInt32(dRow["AccountID"].ToString())].Contains(dRow["Symbol"].ToString()))
                            {
                                if (l1data.LastPrice.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    dRow[_colName] = l1data.LastPrice;
                                }
                            }
                        }
                    }
                    else if (optPreviousPrice.Checked.Equals(true))
                    {
                        //If the close price data is required for the symbol.
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName) && _accountSymbols.ContainsKey(Convert.ToInt32(dRow["AccountID"].ToString())) && _accountSymbols[Convert.ToInt32(dRow["AccountID"].ToString())].Contains(dRow["Symbol"].ToString()))
                            {
                                if (l1data.Previous.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    dRow[_colName] = l1data.Previous;
                                }
                            }
                        }
                    }
                    else if (optMidPrice.Checked.Equals(true))
                    {
                        //If the close price data is required for the symbol.
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName) && _accountSymbols.ContainsKey(Convert.ToInt32(dRow["AccountID"].ToString())) && _accountSymbols[Convert.ToInt32(dRow["AccountID"].ToString())].Contains(dRow["Symbol"].ToString()))
                            {
                                if (l1data.Mid.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    dRow[_colName] = l1data.Mid;
                                }
                            }
                        }
                    }
                    else if (optSelectedFeedPrice.Checked.Equals(true))
                    {
                        //If the close price data is required for the symbol.
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName) && _accountSymbols.ContainsKey(Convert.ToInt32(dRow["AccountID"].ToString())) && _accountSymbols[Convert.ToInt32(dRow["AccountID"].ToString())].Contains(dRow["Symbol"].ToString()))
                            {
                                if (l1data.SelectedFeedPrice.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    dRow[_colName] = l1data.SelectedFeedPrice;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dRow in _dtGridDataSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName) && _accountSymbols.ContainsKey(Convert.ToInt32(dRow["AccountID"].ToString())) && _accountSymbols[Convert.ToInt32(dRow["AccountID"].ToString())].Contains(dRow["Symbol"].ToString()))
                            {
                                if (l1data.iMid.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    _btnGetLiveFeedClicked = true;
                                    dRow[_colName] = l1data.iMid;
                                }
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                {

                    string symbol = l1data.Symbol;
                    UltraGridRow[] filteredRows = grdPivotDisplay.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in filteredRows)
                    {
                        int fromCurrencyID = int.Parse((row.Cells["FromCurrencyID"]).Value.ToString());
                        int toCurrencyID = int.Parse((row.Cells["ToCurrencyID"]).Value.ToString());
                        string forexSymbol = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetPranaForexSymbolFromCurrencies(fromCurrencyID, toCurrencyID);
                        if (forexSymbol.Equals(symbol) && grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists(_colName))
                        {
                            if (optLastPrice.Checked)
                            {
                                SetLiveForexRate(l1data.LastPrice, row, fromCurrencyID, toCurrencyID);
                            }
                            else if (optPreviousPrice.Checked)
                            {
                                SetLiveForexRate(l1data.Previous, row, fromCurrencyID, toCurrencyID);
                            }
                            else if (optMidPrice.Checked)
                            {
                                SetLiveForexRate(l1data.Mid, row, fromCurrencyID, toCurrencyID);
                            }
                            else if (optSelectedFeedPrice.Checked)
                            {
                                SetLiveForexRate(l1data.SelectedFeedPrice, row, fromCurrencyID, toCurrencyID);
                            }
                            else
                            {
                                SetLiveForexRate(l1data.iMid, row, fromCurrencyID, toCurrencyID);
                            }
                        }
                        grdPivotDisplay.UpdateData();
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Delta))
                {
                    if (l1data is OptionSymbolData)
                    {
                        //DateTime dt = (DateTime)DateTime.Now.Date;
                        string symbol = l1data.Symbol;
                        double delta = ((OptionSymbolData)(l1data)).Delta;
                        if (_colName.Equals(String.Empty))
                        {
                            return;
                        }
                        DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);

                        if (dtSource != null)
                        {
                            foreach (DataRow dRow in dtSource.Rows)
                            {
                                if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0)
                                {
                                    if (delta.Equals(double.MinValue))
                                    {
                                        dRow[_colName] = 0;
                                    }
                                    else
                                    {
                                        dRow[_colName] = delta;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Beta))
                {
                    //DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    double beta = l1data.Beta_5yrMonthly;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }
                    DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);
                    foreach (DataRow dRow in dtSource.Rows)
                    {
                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                        {
                            if (beta.Equals(double.MinValue))
                            {
                                dRow[_colName] = 0;
                            }
                            else
                            {
                                dRow[_colName] = Math.Round(beta, 4);
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_TradingVol))
                {
                    DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    double tradeVolume = l1data.TradeVolume;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }
                    DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);

                    foreach (DataRow dRow in dtSource.Rows)
                    {
                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                        {
                            if (tradeVolume.Equals(double.MinValue))
                            {
                                dRow[_colName] = 0;
                            }
                            else
                            {
                                dRow[_colName] = Math.Round(tradeVolume, 4);
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Outstanding))
                {
                    DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    // As responses are received for equitysymbol data only
                    if (l1data is EquitySymbolData)
                    {
                        double outstandingShares = ((EquitySymbolData)l1data).SharesOutstanding;
                        if (_colName.Equals(String.Empty))
                        {
                            return;
                        }
                        DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);
                        foreach (DataRow dRow in dtSource.Rows)
                        {
                            if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                            {
                                if (outstandingShares.Equals(double.MinValue))
                                {
                                    dRow[_colName] = 0;
                                }
                                else
                                {
                                    dRow[_colName] = Math.Round(outstandingShares, 4);
                                }
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyVolatility))
                {
                    DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    double volatility = l1data.FinalImpliedVol;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }
                    DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);

                    foreach (DataRow dRow in dtSource.Rows)
                    {
                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                        {
                            if (volatility.Equals(double.MinValue))
                            {
                                dRow[_colName] = 0;
                            }
                            else
                            {
                                dRow[_colName] = Math.Round(volatility, 4);
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_DailyDividendYield))
                {
                    DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    double dividendYield = l1data.FinalDividendYield;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }
                    DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);

                    foreach (DataRow dRow in dtSource.Rows)
                    {
                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                        {
                            if (dividendYield.Equals(double.MinValue))
                            {
                                dRow[_colName] = 0;
                            }
                            else
                            {
                                dRow[_colName] = Math.Round(dividendYield, 4);
                            }
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_VWAP))
                {
                    DateTime dt = (DateTime)DateTime.Now.Date;
                    string symbol = l1data.Symbol;
                    double VWAP = l1data.VWAP;
                    if (_colName.Equals(String.Empty))
                    {
                        return;
                    }
                    DataTable dtSource = ((DataTable)grdPivotDisplay.DataSource);

                    foreach (DataRow dRow in dtSource.Rows)
                    {
                        if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && dtSource.Columns.Contains(_colName))
                        {
                            if (VWAP.Equals(double.MinValue))
                            {
                                dRow[_colName] = 0;
                            }
                            else
                            {
                                dRow[_colName] = Math.Round(VWAP, 4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Exporting the mark price data to excel file.
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPivotDisplay.DataSource != null)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                    string pathName = null;
                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        pathName = saveFileDialog1.FileName;
                    }
                    else
                    {
                        return;
                    }
                    string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                    workBook.Worksheets.Add(workbookName);
                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                    workBook = this.ultraGridExcelExporter1.Export(this.grdPivotDisplay, workBook.Worksheets[workbookName]);
                    workBook.Save(pathName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //When some filter for symbols is required in the grid starting from the first characters
        //of the symbols entered in the filter text box.
        //string[] _symbolFilterArrayPrevious = null;
        private void btnGetFilteredData_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPivotDisplay.Rows.Count.Equals(0))
                {
                    MessageBox.Show("No data to apply filter", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //ankit : For clearing the filters when filter textbox is empty and enter is pressed
                    if (string.IsNullOrEmpty(txtSymbolFilteration.Text))
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Symbol"].ClearFilterConditions();
                    }
                    else
                    {
                        GetSymbolFilteration();
                    }
                    if (txtSymbolFilteration.Focus())
                    {
                        txtSymbolFilteration.SelectionStart = 0;
                        txtSymbolFilteration.SelectionLength = txtSymbolFilteration.Text.Length;
                    }
                }
                if (txtSymbolFilteration.Focus())
                {
                    txtSymbolFilteration.SelectionStart = 0;
                    txtSymbolFilteration.SelectionLength = txtSymbolFilteration.Text.Length;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ApplyAccountFilteration()
        {
            try
            {
                if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists("Account"))
                {
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Account"].ClearFilterConditions();
                    if (_isDefaultFilterToShowAccountWiseDataOnDailyValuation)
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Account"].FilterConditions.Add(FilterComparisionOperator.NotEquals, string.Empty);
                    }
                    else
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Account"].FilterConditions.Add(FilterComparisionOperator.Equals, string.Empty);
                    }
                }
                else if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists("AccountName"))
                {
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AccountName"].ClearFilterConditions();
                    if (_isDefaultFilterToShowAccountWiseDataOnDailyValuation)
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AccountName"].FilterConditions.Add(FilterComparisionOperator.NotEquals, string.Empty);
                    }
                    else
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AccountName"].FilterConditions.Add(FilterComparisionOperator.Equals, string.Empty);
                    }
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

        private void GetSymbolFilteration()
        {
            try
            {
                if (_dtGridDataSource.Rows.Count > 0)
                {
                    string filterSymbol = string.Empty;
                    //string filterSymbolRemove = string.Empty;
                    string symbolString = txtSymbolFilteration.Text.ToString();
                    if (!symbolString.Equals(string.Empty))
                    {
                        string[] symbolFilterArray = symbolString.Split(',');

                        int lenFilterConditions = grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.Count;
                        for (int i = 0; i < lenFilterConditions; i++)
                        {
                            grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.RemoveAt(i);
                        }

                        //Loop to implement more than one symbol filter.
                        foreach (string symbol in symbolFilterArray)
                        {
                            filterSymbol = symbol.Trim();
                            grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.Add(FilterComparisionOperator.Contains, filterSymbol);
                        }
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["Symbol"].LogicalOperator = FilterLogicalOperator.Or;
                        //_symbolFilterArrayPrevious = symbolFilterArray;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CtrlMarkPriceAndForexConversion_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                btnGetLiveFeedData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetLiveFeedData.ForeColor = System.Drawing.Color.White;
                btnGetLiveFeedData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetLiveFeedData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetLiveFeedData.UseAppStyling = false;
                btnGetLiveFeedData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFetchData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFetchData.ForeColor = System.Drawing.Color.White;
                btnFetchData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFetchData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFetchData.UseAppStyling = false;
                btnFetchData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClearFilter.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClearFilter.ForeColor = System.Drawing.Color.White;
                btnClearFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClearFilter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClearFilter.UseAppStyling = false;
                btnClearFilter.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetFilteredData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetFilteredData.ForeColor = System.Drawing.Color.White;
                btnGetFilteredData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetFilteredData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetFilteredData.UseAppStyling = false;
                btnGetFilteredData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAccountCopy.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAccountCopy.ForeColor = System.Drawing.Color.White;
                btnAccountCopy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAccountCopy.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAccountCopy.UseAppStyling = false;
                btnAccountCopy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        void CtrlMarkPriceAndForexConversion_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                if (CashManagementServices != null)
                {
                    CashManagementServices.Dispose();
                    CashManagementServices = null;
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

        //Clearing all the filters entered earlier.
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            if (grdPivotDisplay.Rows.Count.Equals(0))
            {
                MessageBox.Show("No Filter Applied To Clear", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                foreach (UltraGridColumn column in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Key.Equals("Symbol") || column.Key.Equals("BloombergSymbol"))
                    {
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters[column.Key].ClearFilterConditions();
                    }
                }
                txtSymbolFilteration.Clear();
            }
        }

        bool _allowTabChange = true;
        //Property which gives the result depending upon whether the data is changed in one tab before going 
        //to the other tab. When the data is changed then it asks for the saving the data. Also this user
        //action can be cancelled.
        public bool AllowTabChange
        {
            get
            {
                bool result = false;
                result = IsDataSourceChanged();
                if (result.Equals(true))
                {
                    DialogResult dlgResult = AskSaveConfirmation();
                    if (dlgResult.Equals(DialogResult.Yes))
                    {
                        if (ConfirmationSaveClicked != null)
                        {
                            ConfirmationSaveClicked(this, EventArgs.Empty);
                        }
                        _tabChanged = true;
                        _allowTabChange = true;
                        return _allowTabChange;
                    }
                    else if (dlgResult.Equals(DialogResult.Cancel))
                    {
                        _tabChanged = false;
                        _allowTabChange = false;
                        return _allowTabChange;
                    }
                    else
                    {
                        _tabChanged = true;
                        _allowTabChange = true;
                        return _allowTabChange;
                    }
                }
                else
                {
                    _tabChanged = false;
                    _allowTabChange = true;
                    return _allowTabChange;
                }
            }
        }

        void dtLiveFeed_ValueChanged(object sender, System.EventArgs e)
        {
            if (_datePrevious != (DateTime)dtLiveFeed.Value)
            {
                _dateTypedIndtLiveFeed = true;
            }

            #region CommentedCode
            //try
            //{
            //    dtDateMonth.Value = (DateTime)dtLiveFeed.Value;
            //    SetLiveFeedPriceStatus();
            //    DateTime dtToBePassed = (DateTime)dtLiveFeed.Value;
            //    ScrollToSelectedDate(dtToBePassed);
            //    dtLiveFeed.CloseUp();
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            #endregion
        }

        private void dtLiveFeed_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (_dateTypedIndtLiveFeed == true)
                {
                    _dateTypedIndtLiveFeed = false;

                    dtDateMonth.Value = (DateTime)dtLiveFeed.Value;
                    SetLiveFeedPriceStatus();
                    DateTime dtToBePassed = (DateTime)dtLiveFeed.Value;
                    ScrollToSelectedDate(dtToBePassed);
                    BindGrid();
                    if (_methodologySelected.Equals(MethdologySelected.Daily))
                    {
                        dtDateMonth.Value = _dateSelected;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ScrollToSelectedDate(DateTime dateToBeUsedForScrolling)
        {
            try
            {
                string columnName = string.Empty;
                string columnNameTobeSet = string.Empty;

                //Changing the column color to "LightSteelBlue" for which the live feed data is requested.
                if (_dtGridDataSource != null)
                {
                    foreach (DataColumn col in _dtGridDataSource.Columns)
                    {
                        DateTime dateOut = dateToBeUsedForScrolling;
                        //DateTime dateGrid = DateTime.Now;
                        columnName = col.ColumnName;
                        // string symbol = string.Empty;
                        if (DateTime.TryParse(columnName, out dateOut))
                        {
                            DateTime dtColumnDateForCheck = DateTime.Parse(columnName);
                            if (dtColumnDateForCheck.Equals(dtLiveFeed.Value))
                            {
                                columnNameTobeSet = columnName;
                                if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Contains(columnName))
                                {
                                    grdPivotDisplay.DisplayLayout.Bands[0].Columns[columnName].CellAppearance.BackColor = Color.LightSteelBlue;
                                }
                                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in grdPivotDisplay.Rows)
                                {
                                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in ulRow.Cells)
                                    {
                                        cell.Appearance.ForeColor = Color.White;
                                    }
                                }
                            }
                            else
                            {
                                SetDefaultGridAppreance(columnName);
                            }
                        }
                    }
                    if (!columnNameTobeSet.Equals(string.Empty))
                    {
                        if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Contains(columnNameTobeSet))
                        {
                            UltraGridColumn ultraColumn = grdPivotDisplay.DisplayLayout.Bands[0].Columns[columnNameTobeSet];
                            grdPivotDisplay.ActiveColScrollRegion.ScrollColIntoView(ultraColumn, true);// .Position = 20;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetDefaultGridAppreance(string columnName)
        {
            try
            {
                if (!columnName.Equals(string.Empty))
                {
                    Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grdPivotDisplay.DisplayLayout.Bands[0].Columns;
                    if (columns.Exists(columnName))
                    {
                        columns[columnName].CellAppearance.BackColor = Color.Black;
                    }
                }
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ulRow in grdPivotDisplay.Rows)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in ulRow.Cells)
                    {
                        cell.Appearance.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtSymbolFilteration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtSymbolFilteration.Focused && e.KeyChar == '\r')
            {
                // click the Filter button
                this.btnGetFilteredData.PerformClick();
                // don't allow the Enter key to pass to textbox
                e.Handled = true;
            }
        }

        //DataTable _dtExchangeGroupFilteredTable = new DataTable();
        private void cmbAUEC_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbExchangeGroup.Value != null && grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count > 0 && !cmbExchangeGroup.ToString().Contains("FX"))
                {
                    foreach (UltraGridColumn grdCol in grdPivotDisplay.DisplayLayout.Bands[0].Columns)
                    {
                        if (!grdCol.Key.Equals("Account"))
                        {
                            grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters[grdCol.Key].ClearFilterConditions();
                        }
                    }
                }
                SetExchangeGroupFilteration();
                SetFxFxForwardFilteration();
                txtSymbolFilteration.Text = "";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        DateTime _lastDate = DateTime.Now;
        DateTime _previousCloseDate = DateTime.Now;
        string _exchangeGroupingText = string.Empty;

        private void SetExchangeGroupFilteration()
        {
            try
            {
                btnGetLiveFeedData.Enabled = true;
                string filterExchangeGroup = string.Empty;
                if (cmbExchangeGroup.Value != null && grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count > 0 && !cmbExchangeGroup.ToString().Contains("FX"))
                {
                    string exchangeGroupString = cmbExchangeGroup.Value.ToString();

                    UltraGridColumn colSymbol = null;

                    if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Exists("Symbol"))
                    {
                        colSymbol = grdPivotDisplay.DisplayLayout.Bands[0].Columns["Symbol"];
                    }
                    else
                    {
                        return;
                    }
                    if (!exchangeGroupString.Equals("Nothing"))
                    {
                        _exchangeGroupingText = cmbExchangeGroup.Text;
                        string[] exchangeFilterArray = exchangeGroupString.Split(',');

                        //Loop to implement all exchange identifiers in exchange filter.
                        foreach (string exchangeIdentifier in exchangeFilterArray)
                        {
                            filterExchangeGroup = exchangeIdentifier.Trim();

                            grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].FilterConditions.Add(FilterComparisionOperator.Equals, filterExchangeGroup);
                        }
                        grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].LogicalOperator = FilterLogicalOperator.Or;
                        GetLastAndPreviousDate();
                        if (exchangeGroupString.Equals("Indices-Indices"))
                        {
                            colSymbol.CellActivation = Activation.AllowEdit;
                        }
                        else
                        {
                            colSymbol.CellActivation = Activation.NoEdit;
                        }
                    }
                    else
                    {
                        btnGetLiveFeedData.Enabled = false;
                        colSymbol.CellActivation = Activation.NoEdit;
                    }
                    SetLiveFeedPriceStatus();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetFxFxForwardFilteration()
        {
            if (_tabPageSelected.Equals(TabName_MarkPrice))
            {
                if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    //Divya: As we have to maintain the exchange group filteration applied from Combobox, to remove Fx Fxforwards from Mark price
                    // tab , I am applying a filter on AUECId instead of AUECIdentifier
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECId"].FilterConditions.Add(FilterComparisionOperator.NotEquals, "33");
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECId"].FilterConditions.Add(FilterComparisionOperator.NotEquals, "32");
                }
            }
            else if (_tabPageSelected.Equals(TabName_FXMarkPrice))
            {
                if (grdPivotDisplay.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].FilterConditions.Add(FilterComparisionOperator.Equals, "FX-FXFWD");
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].FilterConditions.Add(FilterComparisionOperator.Equals, "FX-FX");
                    grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].LogicalOperator = FilterLogicalOperator.Or;
                }
            }
        }

        private void GetLastAndPreviousDate()
        {
            try
            {
                DateTime dateToBeCoverted = DateTime.Now;
                dateToBeCoverted = DateTime.UtcNow;
                DateTime dtUTCLiveFeed = dateToBeCoverted;
                int auecID = 0;
                if (_exchangeGroupingAUECID.ContainsKey(cmbExchangeGroup.Text.ToString()))
                {
                    auecID = _exchangeGroupingAUECID[cmbExchangeGroup.Text.ToString()];

                    DateTime auecLocalTime1 = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dtUTCLiveFeed, CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    bool isBusinessDay = BusinessDayCalculator.GetInstance().IsBusinessDayForAUEC(auecLocalTime1, auecID);

                    DateTime exchangeGroupDateTime = (DateTime)((MarketTimes)_exchangeGroupingAUECTime[cmbExchangeGroup.Text.ToString()]).MarketStartTime;
                    if (isBusinessDay)
                    {
                        if (auecLocalTime1.TimeOfDay > exchangeGroupDateTime.TimeOfDay)
                        {
                            _lastDate = auecLocalTime1;
                        }
                        else
                        {
                            //Find out the previous business day for this auec and say it last date.
                            _lastDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(auecLocalTime1, -1, auecID);
                        }
                    }
                    else
                    {
                        //Find out the previous business day for this auec and say it last date.                       
                        _lastDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(auecLocalTime1, -1, auecID);
                    }
                    //Getting the previous close date.
                    _previousCloseDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_lastDate, -1, auecID); //Didnt get the correct result i.e. the previous day.
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void optUseImportExport_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (optUseImportExport.Checked)
                {
                    grpBoxImportExport.Enabled = true;
                    grpBoxLiveFeedWhole.Enabled = false;
                    grpBoxSymbolUpdationChoice.Enabled = false;
                    grdPivotDisplay.AllowDrop = true;
                    btnImport.Enabled = true;
                    btnExport.Enabled = true;
                }
                else
                {
                    grpBoxLiveFeedWhole.Enabled = true;
                    grpBoxSymbolUpdationChoice.Enabled = true;
                    grpBoxImportExport.Enabled = false;
                    grdPivotDisplay.AllowDrop = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optUseLiveFeed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (optUseLiveFeed.Checked.Equals(true))
                {
                    grpBoxLiveFeedWhole.Enabled = true;
                    grpBoxSymbolUpdationChoice.Enabled = true;
                    grpBoxImportExport.Enabled = false;
                    grdPivotDisplay.AllowDrop = false;
                    btnImport.Enabled = false;
                    btnExport.Enabled = false;
                    btnGetLiveFeedData.Enabled = false;
                    lblUpdatePrice.Enabled = true;
                    optLastPrice.Enabled = true;
                    optIMidPrice.Enabled = true;
                    optMidPrice.Enabled = true;
                    optSelectedFeedPrice.Enabled = true;
                    optPreviousPrice.Enabled = true;
                    lblIMidDate.Enabled = true;
                    lblMidDate.Enabled = true;
                    lblSelectedFeedDate.Enabled = true;
                    lblLastDate.Enabled = true;
                    lblPreviousDate.Enabled = true;
                }
                else
                {
                    grpBoxImportExport.Enabled = true;
                    grpBoxLiveFeedWhole.Enabled = false;
                    grpBoxSymbolUpdationChoice.Enabled = false;
                    grdPivotDisplay.AllowDrop = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optLastPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (!(_exchangeGroupingText.Equals(null)) && !(_exchangeGroupingText.Equals("Nothing")) && !(_exchangeGroupingText.Equals(string.Empty)))
            {
                GetLastAndPreviousDate();
            }
            SetLiveFeedPriceStatus();
        }

        private void optPreviousPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(_exchangeGroupingText.Equals(null)) && !(_exchangeGroupingText.Equals("Nothing")) && !(_exchangeGroupingText.Equals(string.Empty)))
                {
                    GetLastAndPreviousDate();
                }
                SetLiveFeedPriceStatus();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optMidPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(_exchangeGroupingText.Equals(null)) && !(_exchangeGroupingText.Equals("Nothing")) && !(_exchangeGroupingText.Equals(string.Empty)))
                {
                    GetLastAndPreviousDate();
                }
                SetLiveFeedPriceStatus();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optSelectedFeedPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(_exchangeGroupingText.Equals(null)) && !(_exchangeGroupingText.Equals("Nothing")) && !(_exchangeGroupingText.Equals(string.Empty)))
                {
                    GetLastAndPreviousDate();
                }
                SetLiveFeedPriceStatus();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetLiveFeedPriceStatus()
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI && CachedDataManager.IsMarketDataBlocked)
                {
                    _isMarketDataBlocked = true;
                }
                if (_tabPageSelected.Equals(TabName_MarkPrice))
                {
                    //DateTime dateBeforeToday = DateTime.Today.AddDays(-1);
                    DateTime liveFeedDate = DateTime.Parse(dtLiveFeed.Value.ToString());
                    string lastDateString = _lastDate.ToString(("MM/dd/yyyy"));
                    string closingDateString = _previousCloseDate.ToString(("MM/dd/yyyy"));
                    string liveFeedDateString = liveFeedDate.ToString("MM/dd/yyyy");
                    if (cmbExchangeGroup.Value != null)
                    {
                        if (liveFeedDateString.Equals(lastDateString) && optLastPrice.Checked.Equals(true) && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (liveFeedDateString.Equals(closingDateString) && optPreviousPrice.Checked.Equals(true) && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (liveFeedDateString.Equals(lastDateString) && optIMidPrice.Checked.Equals(true) && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (liveFeedDateString.Equals(lastDateString) && optMidPrice.Checked.Equals(true) && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (liveFeedDateString.Equals(lastDateString) && optSelectedFeedPrice.Checked.Equals(true))
                        {
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblLastDate.Text = "";
                            lblPreviousDate.Text = "";
                            lblIMidDate.Text = "";
                            lblMidDate.Text = "";
                            lblSelectedFeedDate.Text = "";
                        }
                        else
                        {
                            lblPreviousDate.Text = "(" + _previousCloseDate.ToString("MM/dd/yyyy") + ")";
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblIMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblMidDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            lblSelectedFeedDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = false;
                        }

                        if (optPreviousPrice.Checked.Equals(true) && _pricingServicesProxy.InnerChannel.GetSameDayClosedDataConfigValue() && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            btnGetLiveFeedData.Enabled = true;
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_TradingVol))
                {
                    DateTime liveFeedDate = DateTime.Parse(dtLiveFeed.Value.ToString());
                    string lastDateString = _lastDate.ToString(("MM/dd/yyyy"));
                    string liveFeedDateString = liveFeedDate.ToString("MM/dd/yyyy");

                    if (cmbExchangeGroup.Value != null)
                    {
                        if (liveFeedDateString.Equals(lastDateString) && !cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = true;
                        }
                        else if (cmbExchangeGroup.Value.Equals("Nothing"))
                        {
                            lblLastDate.Text = "";
                            lblPreviousDate.Text = "";
                            btnGetLiveFeedData.Enabled = false;
                        }
                        else
                        {
                            lblLastDate.Text = "(" + _lastDate.ToString("MM/dd/yyyy") + ")";
                            btnGetLiveFeedData.Enabled = false;
                        }
                    }
                }
                else if (_tabPageSelected.Equals(TabName_Outstanding))
                {
                    DateTime liveFeedDate = DateTime.Parse(dtLiveFeed.Value.ToString());
                    string closingDateString = _previousCloseDate.ToString(("MM/dd/yyyy"));
                    string liveFeedDateString = liveFeedDate.ToString("MM/dd/yyyy");
                    if (liveFeedDateString.Equals(closingDateString))
                    {
                        btnGetLiveFeedData.Enabled = true;
                    }
                    else
                    {
                        btnGetLiveFeedData.Enabled = false;
                    }
                }
                else
                {
                    lblLastDate.Text = string.Empty;
                    lblPreviousDate.Text = string.Empty;
                    lblIMidDate.Text = string.Empty;
                    lblMidDate.Text = string.Empty;
                    lblSelectedFeedDate.Text = string.Empty;
                }
                if (_isMarketDataBlocked && (_tabPageSelected.Equals(TabName_ForexConversion) ||
                _tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_VWAP) ||
                _tabPageSelected.Equals(TabName_DailyVolatility) || _tabPageSelected.Equals(TabName_DailyDividendYield) ||
                _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Delta) ||
                _tabPageSelected.Equals(TabName_Outstanding)))
                {
                    btnGetLiveFeedData.Enabled = false;
                    optUseLiveFeed.Enabled = false;
                }
                else if (_isMarketDataBlocked && _tabPageSelected.Equals(TabName_MarkPrice))
                {
                    optUseLiveFeed.Enabled = false;
                    optOMIPrice.Checked = true;
                    if (optOMIPrice.Checked && cmbExchangeGroup.Value.Equals(Const_Nothing))
                        btnGetLiveFeedData.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optOverwriteAllSymbols_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(_exchangeGroupingText.Equals(null)) && !(_exchangeGroupingText.Equals("Nothing")) && !(_exchangeGroupingText.Equals(string.Empty)))
                {
                    GetLastAndPreviousDate();
                }
                SetLiveFeedPriceStatus();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void ResetGrid()
        {
            if (grdPivotDisplay.ActiveCell != null)
            {
                grdPivotDisplay.ActiveCell = grdPivotDisplay.Rows[0].Cells[0];
            }
        }

        ICommunicationManager _exposurePnlCommunicationManager = null;
        public ICommunicationManager ExPNLCommMgrInstance
        {
            set
            {
                _exposurePnlCommunicationManager = value;
                if (_exposurePnlCommunicationManager != null)
                {
                    _exposurePnlCommunicationManager.MessageReceived += new MessageReceivedDelegate(_exposurePnlCommunicationManager_MessageReceived);
                }
            }
        }

        void _exposurePnlCommunicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage qMsg = e.Value;
                if (!this.IsDisposed)
                {
                    string message = qMsg.Message.ToString();
                    string msgType = PranaMessageFormatter.GetMessageType(message);
                    List<SymbolData> l1DataList = new List<SymbolData>();
                    switch (msgType)
                    {
                        //Problem with _isUpdating.
                        case PranaMessageConstants.MSG_GetLiveFeedSnapShot:

                            string[] strArr = message.Split('^')[1].Split('~');

                            for (int i = 0; i < strArr.Length - 1; i++)
                            {
                                // sud: need to add here for option also.
                                SymbolData l1Data = LiveFeedDataInstanceCreater.CreateDataInstance(strArr[i]);
                                l1DataList.Add(l1Data);
                            }
                            if (UIValidation.GetInstance().validate(this))
                            {
                                if (this.InvokeRequired)
                                {
                                    L1ListObjHandler l1ListObjHandler = new L1ListObjHandler(UpdateMarkPriceAndFXDataWithLiveFeedDataList);
                                    this.BeginInvoke(l1ListObjHandler, new object[] { l1DataList });
                                }
                                else
                                {
                                    UpdateMarkPriceAndFXDataWithLiveFeedDataList(l1DataList);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdPivotDisplay_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (_btnGetLiveFeedClicked.Equals(true))
                {
                    if (e.Row.Cells.Exists(_colName))
                    {
                        e.Row.Cells[_colName].Appearance.ForeColor = Color.Red;
                    }
                }
                _btnGetLiveFeedClicked = false;
                //Setting account list based on maserfund.
                if (_tabPageSelected.Equals(TabName_NAV))
                {
                    var masterFundName = e.Row.Cells["MasterFund"].Text;
                    var accountList = GetAccountsValueList(CachedDataManager.GetInstance.GetMasterFundID(masterFundName));
                    if (accountList != null && accountList.ValueListItems.Count > 0)
                    {
                        e.Row.Cells["FundID"].ValueList = accountList;
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        L1ObjHandler l1ObjHandler = new L1ObjHandler(UpdateMarkPriceAndFXDataWithLiveFeedData);
                        this.BeginInvoke(l1ObjHandler, new object[] { data });
                    }
                    else
                    {
                        UpdateMarkPriceAndFXDataWithLiveFeedData(data);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        void ctrlMarkPriceAndForexConversion_ConfirmationSaveClicked(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDataToSave = new DataTable();
                dtDataToSave = GetDataToSave();
                switch (_tabPageSelected)
                {
                    case TabName_MarkPrice:
                        SaveMarkPriceData(dtDataToSave);
                        break;
                    case TabName_FXMarkPrice:
                        SaveMarkPriceData(dtDataToSave);
                        break;

                    case TabName_ForexConversion:
                        SaveForexConversionData(dtDataToSave);
                        break;

                    case TabName_NAV:
                        SaveNAVValues(dtDataToSave);
                        break;

                    case TabName_DailyCash:
                        SaveDailyCashValues(dtDataToSave);
                        break;

                    case TabName_CollateralInterest:
                        SaveCollateralInterestValues(dtDataToSave);
                        break;

                    case TabName_Beta:
                        SaveDailyBetaValues(dtDataToSave);
                        break;

                    case TabName_TradingVol:
                        SaveDailyTradingVolValues(dtDataToSave);
                        break;

                    case TabName_Delta:
                        SaveDailyDeltaValues(dtDataToSave);
                        break;

                    case TabName_StartOfMonthCapitalAccount:
                        SaveStartOfMonthCapitalAccountValues(dtDataToSave);
                        break;

                    case TabName_UserDefinedMTDPnL:
                        SaveUserDefinedMTDPnLValues(dtDataToSave);
                        break;

                    case TabName_CollateralPrice:
                        SaveCollateralPriceValues(dtDataToSave);
                        break;

                    case TabName_Outstanding:
                        SaveDailyOutStandingValues(dtDataToSave);
                        break;

                    case TabName_PerformanceNumbers:
                        SavePerformanceNumberValues(dtDataToSave);
                        break;

                    case TabName_DailyCreditLimit:
                        SaveDailyCreditLimitValues(dtDataToSave);
                        break;

                    case TabName_DailyVolatility:
                        SaveDailyVolatility(dtDataToSave);
                        break;

                    case TabName_DailyDividendYield:
                        SaveDailyDividendYield(dtDataToSave);
                        break;
                    case TabName_VWAP:
                        SaveDailyVWAP(dtDataToSave);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves the collateral price values.
        /// </summary>
        /// <param name="dtDataToSave">The dt data to save.</param>
        /// <returns></returns>
        internal int SaveCollateralPriceValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtCollateralPrice = null;

                //Getting the Collateral Price data from the control to save it.
                dtCollateralPrice = dtDataToSave;
                if (dtCollateralPrice != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtCollateralPriceTemp = new DataTable();
                    dtCollateralPriceTemp.TableName = "DailyCollateralPrice";
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("Symbol"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("Date"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("FundId"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("CollateralPrice"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("Haircut"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnMV"));
                    dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnCollateral"));
                    foreach (DataRow dr in dtCollateralPrice.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            DataRow drNew = dtCollateralPriceTemp.NewRow();
                            drNew["Date"] = dtDateMonth.Value;
                            drNew["FundId"] = dr["FundId"];
                            drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                            drNew["CollateralPrice"] = dr["CollateralPrice"];
                            drNew["Haircut"] = dr["Haircut"];
                            drNew["RebateOnMV"] = dr["RebateOnMV"];
                            drNew["RebateOnCollateral"] = dr["RebateOnCollateral"];
                            dtCollateralPriceTemp.Rows.Add(drNew);
                            dtCollateralPriceTemp.AcceptChanges();
                        }
                    }
                    if (dtCollateralPriceTemp != null && dtCollateralPriceTemp.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveCollateralValues(dtCollateralPriceTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        internal int SaveDailyOutStandingValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtOutStanding = dtDataToSave;
                if (dtOutStanding != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtOutStandingNew = new DataTable();
                    dtOutStandingNew.TableName = "DailyOutStandings";
                    dtOutStandingNew.Columns.Add(new DataColumn("Symbol"));
                    dtOutStandingNew.Columns.Add(new DataColumn("Date"));
                    dtOutStandingNew.Columns.Add(new DataColumn("OutStandings"));
                    dtOutStandingNew.Columns.Add(new DataColumn("AUECID"));
                    foreach (DataRow dr in dtOutStanding.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtOutStanding.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtOutStandingNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["OutStandings"] = dr[dc.ColumnName];
                                    drNew["AUECID"] = dr["AUECID"];
                                    dtOutStandingNew.Rows.Add(drNew);
                                    dtOutStandingNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (dtOutStandingNew != null && dtOutStandingNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveOutStandings(dtOutStandingNew);
                    }
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
            return rowsAffected;
        }

        internal int SaveDailyDeltaValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDelta = dtDataToSave;
                if (dtDelta != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtDeltaNew = new DataTable();
                    dtDeltaNew.TableName = "DailyDelta";
                    dtDeltaNew.Columns.Add(new DataColumn("Symbol"));
                    dtDeltaNew.Columns.Add(new DataColumn("Date"));
                    dtDeltaNew.Columns.Add(new DataColumn("Delta"));
                    foreach (DataRow dr in dtDelta.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!dr["Symbol"].ToString().Equals(""))
                        {
                            foreach (DataColumn dc in dtDelta.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtDeltaNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["Delta"] = dr[dc.ColumnName];
                                    dtDeltaNew.Rows.Add(drNew);
                                    dtDeltaNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    rowsAffected = WindsorContainerManager.SaveDelta(dtDeltaNew);
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
            return rowsAffected;
        }

        internal int SaveDailyTradingVolValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtTradingVol = dtDataToSave;
                if (dtTradingVol != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtTradingVolNew = new DataTable();
                    dtTradingVolNew.TableName = "DailyTradingVol";
                    dtTradingVolNew.Columns.Add(new DataColumn("Symbol"));
                    dtTradingVolNew.Columns.Add(new DataColumn("Date"));
                    dtTradingVolNew.Columns.Add(new DataColumn("TradingVolume"));

                    foreach (DataRow dr in dtTradingVol.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!dr["Symbol"].ToString().Equals(""))
                        {
                            foreach (DataColumn dc in dtTradingVol.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtTradingVolNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["TradingVolume"] = dr[dc.ColumnName];
                                    dtTradingVolNew.Rows.Add(drNew);
                                    dtTradingVolNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    rowsAffected = WindsorContainerManager.SaveTradingVolume(dtTradingVolNew);
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
            return rowsAffected;
        }

        internal int SaveDailyBetaValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtBeta = dtDataToSave;
                if (dtBeta != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtBetaNew = new DataTable();
                    dtBetaNew.TableName = "DailyBeta";
                    dtBetaNew.Columns.Add(new DataColumn("Symbol"));
                    dtBetaNew.Columns.Add(new DataColumn("Date"));
                    dtBetaNew.Columns.Add(new DataColumn("Beta"));
                    dtBetaNew.Columns.Add(new DataColumn("AUECID"));

                    foreach (DataRow dr in dtBeta.Rows)
                    {
                        //Assigning the row having symbol being not blank.

                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtBeta.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtBetaNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["Beta"] = dr[dc.ColumnName];
                                    drNew["AUECID"] = dr["AUECID"];
                                    dtBetaNew.Rows.Add(drNew);
                                    dtBetaNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (dtBetaNew != null && dtBetaNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveBeta(dtBetaNew);
                    }
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
            return rowsAffected;
        }

        internal int SavePerformanceNumberValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtPerformanceNumberValues = null;

                //Getting the daily cash values from the control to save it.
                dtPerformanceNumberValues = dtDataToSave;
                if (dtPerformanceNumberValues != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtPerformanceNumberValuesTemp = new DataTable();
                    dtPerformanceNumberValuesTemp.TableName = "DailyPerformanceNumbers";
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("FundID"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("MTDValue"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("QTDValue"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("YTDValue"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("MTDReturn"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("QTDReturn"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("YTDReturn"));
                    dtPerformanceNumberValuesTemp.Columns.Add(new DataColumn("Date"));
                    foreach (DataRow dr in dtPerformanceNumberValues.Rows)
                    {
                        DataRow drNew = dtPerformanceNumberValuesTemp.NewRow();
                        if (dr["MTDValue"].Equals(System.DBNull.Value))
                        {
                            dr["MTDValue"] = 0;
                        }
                        if (dr["QTDValue"].Equals(System.DBNull.Value))
                        {
                            dr["QTDValue"] = 0;
                        }
                        if (dr["YTDValue"].Equals(System.DBNull.Value))
                        {
                            dr["YTDValue"] = 0;
                        }
                        if (dr["MTDReturn"].Equals(System.DBNull.Value))
                        {
                            dr["MTDReturn"] = 0;
                        }
                        if (dr["QTDReturn"].Equals(System.DBNull.Value))
                        {
                            dr["QTDReturn"] = 0;
                        }
                        if (dr["YTDReturn"].Equals(System.DBNull.Value))
                        {
                            dr["YTDReturn"] = 0;
                        }
                        drNew["FundID"] = dr["FundID"];
                        drNew["MTDValue"] = dr["MTDValue"];
                        drNew["QTDValue"] = dr["QTDValue"];
                        drNew["YTDValue"] = dr["YTDValue"];
                        drNew["MTDReturn"] = dr["MTDReturn"];
                        drNew["QTDReturn"] = dr["QTDReturn"];
                        drNew["YTDReturn"] = dr["YTDReturn"];
                        drNew["Date"] = (DateTime)dtDateMonth.Value;

                        dtPerformanceNumberValuesTemp.Rows.Add(drNew);
                        dtPerformanceNumberValuesTemp.AcceptChanges();
                    }
                    if (dtPerformanceNumberValuesTemp != null && dtPerformanceNumberValuesTemp.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SavePerformanceNumberValues(dtPerformanceNumberValuesTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        internal int SaveDailyVolatility(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtVolatility = dtDataToSave;
                if (dtVolatility != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtVolatilityNew = new DataTable();
                    dtVolatilityNew.TableName = "DailyVolatility";
                    dtVolatilityNew.Columns.Add(new DataColumn("Symbol"));
                    dtVolatilityNew.Columns.Add(new DataColumn("Date"));
                    dtVolatilityNew.Columns.Add(new DataColumn("Volatility"));
                    dtVolatilityNew.Columns.Add(new DataColumn("AUECID"));

                    foreach (DataRow dr in dtVolatility.Rows)
                    {
                        //Assigning the row having symbol being not blank.

                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtVolatility.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtVolatilityNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["Volatility"] = dr[dc.ColumnName];
                                    drNew["AUECID"] = dr["AUECID"];
                                    dtVolatilityNew.Rows.Add(drNew);
                                    dtVolatilityNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (dtVolatilityNew != null && dtVolatilityNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveVolatility(dtVolatilityNew);
                    }
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
            return rowsAffected;
        }

        internal int SaveDailyDividendYield(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDividendYield = dtDataToSave;
                if (dtDividendYield != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtDividendYieldNew = new DataTable();
                    dtDividendYieldNew.TableName = "DailyDividendYield";
                    dtDividendYieldNew.Columns.Add(new DataColumn("Symbol"));
                    dtDividendYieldNew.Columns.Add(new DataColumn("Date"));
                    dtDividendYieldNew.Columns.Add(new DataColumn("DividendYield"));
                    dtDividendYieldNew.Columns.Add(new DataColumn("AUECID"));

                    foreach (DataRow dr in dtDividendYield.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtDividendYield.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtDividendYieldNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["DividendYield"] = dr[dc.ColumnName];
                                    drNew["AUECID"] = dr["AUECID"];
                                    dtDividendYieldNew.Rows.Add(drNew);
                                    dtDividendYieldNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (dtDividendYieldNew != null && dtDividendYieldNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveDividendYield(dtDividendYieldNew);
                    }
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
            return rowsAffected;
        }

        /// <summary>
        /// Saves the daily vwap.
        /// </summary>
        /// <param name="dtDataToSave">The dt data to save.</param>
        /// <returns></returns>
        internal int SaveDailyVWAP(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtVWAP = dtDataToSave;
                if (dtVWAP != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtVWAPNew = new DataTable();
                    dtVWAPNew.TableName = "DailyVWAP";
                    dtVWAPNew.Columns.Add(new DataColumn("Symbol"));
                    dtVWAPNew.Columns.Add(new DataColumn("Date"));
                    dtVWAPNew.Columns.Add(new DataColumn("VWAP"));
                    dtVWAPNew.Columns.Add(new DataColumn("AUECID"));

                    foreach (DataRow dr in dtVWAP.Rows)
                    {
                        //Assigning the row having symbol being not blank.

                        if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtVWAP.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "BloombergSymbol")
                                {
                                    DataRow drNew = dtVWAPNew.NewRow();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["VWAP"] = dr[dc.ColumnName];
                                    drNew["AUECID"] = dr["AUECID"];
                                    dtVWAPNew.Rows.Add(drNew);
                                    dtVWAPNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (dtVWAPNew != null && dtVWAPNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveVWAP(dtVWAPNew);
                    }
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
            return rowsAffected;
        }

        #region Mark Price Section

        private void SetupMarkPrice()
        {
            try
            {
                cmbExchangeGroup.Visible = true;
                lblExchangeGroup.Visible = true;
                optUseImportExport.Visible = true;
                grpBoxImportExport.Visible = true;
                grdPivotDisplay.AllowDrop = true;
                grpBoxFilter.Visible = true;
                btnImport.Visible = true;
                btnExport.Visible = true;
                optOMIPrice.Visible = true;
                optDaily.Visible = true;
                optMonthly.Visible = true;
                grpBoxLiveFeedHandling.Visible = true;
                grpSelectDateMethodology.Visible = true;

                if (cmbExchangeGroup.DataSource == null)
                {
                    BindAUECFilter();
                }
                else if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
                {
                    cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
                    cmbExchangeGroup.Value = "Nothing";
                    cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
                }

                optPreviousPrice.Visible = true;
                optIMidPrice.Visible = true;
                optMidPrice.Visible = true;
                optSelectedFeedPrice.Visible = true;
                optLastPrice.Visible = true;
                optLastPrice.Text = "Last Price";
                lblUpdatePrice.Visible = true;
                lblUpdatePrice.Text = "Update Price:";
                lblLastDate.Visible = true;
                lblPreviousDate.Visible = true;
                lblIMidDate.Visible = true;
                lblMidDate.Visible = true;
                lblSelectedFeedDate.Visible = true;
                btnGetLiveFeedData.Text = "Get Prices";
                grpBoxLiveFeedHandling.Text = "Update Prices";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindGridForMarkPrice(string selectedTab)
        {
            //Get mark prices data for symbols for the selected date and methodology.
            DataTable dtMarkPrices = new DataTable();
            try
            {
                bool isFxFXForwardData = false;
                if (selectedTab.Equals(TabName_FXMarkPrice))
                {
                    isFxFXForwardData = true;
                }
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo = _pricingServicesProxy.InnerChannel.GetMarkPriceForDate(_dateSelected, 0, isFxFXForwardData);
                    dtMarkPrices = GetTableFromSymbolMarkPriceDict(accountSymbolMarkPriceInfo);
                }
                else if (_methodologySelected.Equals(MethdologySelected.Monthly))
                {
                    Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo = _pricingServicesProxy.InnerChannel.GetMarkPriceForDate(_dateSelected, 2, isFxFXForwardData);
                    dtMarkPrices = GetTableFromSymbolMarkPriceDict(accountSymbolMarkPriceInfo);
                }
                //Assigning the mark price 0 to the symbols whose mark price is blank and changing the symbol case to the capital if it is in the lowwe case.
                if (selectedTab.Equals(TabName_MarkPrice))
                {
                    ConvertBlankNumericValues(dtMarkPrices);
                    if (dtMarkPrices != null)
                    {
                        GetListName(tabName);
                        grdPivotDisplay.DataSource = null;
                        grdPivotDisplay.DataSource = dtMarkPrices;
                        ResetFilterAfterRebindingGrid();
                        SetFxFxForwardFilteration();
                        _dtGridDataSource = dtMarkPrices;
                    }
                    else
                        grdPivotDisplay.DataSource = null;
                }
                if (selectedTab.Equals(TabName_FXMarkPrice))
                {
                    ConvertBlankNumericValues(dtMarkPrices);
                    GetListName(tabName);
                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtMarkPrices;
                    SetFxFxForwardFilteration();
                    _dtGridDataSource = dtMarkPrices;
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

        internal int SaveMarkPriceData(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtMarkPrices = null;
                //Getting the mark price data from the control to save it.
                dtMarkPrices = dtDataToSave;
                if (dtMarkPrices != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtMarkPricesNew = new DataTable();
                    dtMarkPricesNew.TableName = "MarkPriceImport";
                    dtMarkPricesNew.Columns.Add(new DataColumn("Symbol"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("Date"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("MarkPrice"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("MarkPriceImportType"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("ForwardPoints"));
                    //Added AUECID as it will be used at pricing server end to update cache
                    dtMarkPricesNew.Columns.Add(new DataColumn("AUECID"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("AUECIdentifier"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("OriginalMarkPrice"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("AccountID"));

                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!dr["Symbol"].ToString().Equals(""))
                        {
                            foreach (DataColumn dc in dtMarkPrices.Columns)
                            {
                                if (dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "ForwardPoints" && dc.ColumnName != "FxRate" && dc.ColumnName != "BloombergSymbol" && dc.ColumnName != "AccountID" && dc.ColumnName != "Account")
                                {
                                    if (_symbolWiseChangedDatesForMarkPrice != null && (!_symbolWiseChangedDatesForMarkPrice.ContainsKey(dr["Symbol"].ToString()) || !_symbolWiseChangedDatesForMarkPrice[dr["Symbol"].ToString()].Contains(DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null))))
                                    {
                                        continue;
                                    }

                                    DataRow drNew = dtMarkPricesNew.NewRow();

                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["MarkPrice"] = dr[dc.ColumnName];
                                    drNew["OriginalMarkPrice"] = dr[dc.ColumnName, DataRowVersion.Original];
                                    // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                                    // L stands for Live feed Data
                                    drNew["MarkPriceImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.L.ToString();
                                    drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                    drNew["ForwardPoints"] = dr["ForwardPoints"].ToString();
                                    //Added AUECID as it will be used at pricing server end to update cache
                                    if (dtMarkPrices.Columns.Contains("AUECID") && dr["AUECID"] != System.DBNull.Value)
                                        drNew["AUECID"] = dr["AUECID"].ToString().ToUpper();
                                    if (dtMarkPrices.Columns.Contains("AUECIdentifier") && dr["AUECIdentifier"] != System.DBNull.Value)
                                        drNew["AUECIdentifier"] = dr["AUECIdentifier"].ToString();
                                    if (dtMarkPrices.Columns.Contains("AccountID") && dr["AccountID"] != System.DBNull.Value)
                                        drNew["AccountID"] = dr["AccountID"].ToString();

                                    dtMarkPricesNew.Rows.Add(drNew);
                                    dtMarkPricesNew.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (_symbolWiseChangedDatesForMarkPrice != null)
                    {
                        _symbolWiseChangedDatesForMarkPrice.Clear();
                    }
                    if (dtMarkPricesNew != null && dtMarkPricesNew.Rows.Count > 0)
                    {
                        AddDailyDataAuditEntry(dtMarkPricesNew, Prana.BusinessObjects.TradeAuditActionType.ActionType.MarkPrice_Changed, "Mark Price Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        AuditManager.Instance.SaveAuditListForDailyValuation(_tradeAuditCollection_DailyValuation);
                        _tradeAuditCollection_DailyValuation.Clear();
                        // as Original Mark Price column is just needed for Audit Trail.
                        dtMarkPricesNew.Columns.Remove("OriginalMarkPrice");
                        //Modifeid by omshiv, isAutoApprove mark prices, for prana mode it is false
                        bool isAutoApprove = true;
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPricesNew, isAutoApprove);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            _isDataCopiedFromBackDate = false;
            return rowsAffected;
        }
        #endregion

        /// <summary>
        ///  To copy data according to the selected filters(Exchange group)
        /// Author : Divya Bansal
        /// </summary>
        /// <param name="strSelectedTab"></param>
        private void BindGridforSelectedTabCopydata(string strSelectedTab)
        {
            try
            {
                DataTable dtOriginal = new DataTable();
                if (strSelectedTab.Equals(TabName_MarkPrice))
                {
                    Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo = _pricingServicesProxy.InnerChannel.GetMarkPriceForDate(_dateSelected, 0, false);
                    dtOriginal = GetTableFromSymbolMarkPriceDict(accountSymbolMarkPriceInfo);
                }
                else if (strSelectedTab.Equals(TabName_Beta))
                {
                    dtOriginal = WindsorContainerManager.GetBetaValueDateWise(_dateSelected, _dateSelected, 0);
                }
                else if (strSelectedTab.Equals(TabName_TradingVol))
                {
                    dtOriginal = WindsorContainerManager.GetTradingVolDateWise(_dateSelected, _dateSelected, 0);
                }
                else if (strSelectedTab.Equals(TabName_Delta))
                {
                    dtOriginal = WindsorContainerManager.GetDeltaValueDateWise(_dateSelected, _dateSelected, 0);
                }
                else if (strSelectedTab.Equals(TabName_DailyVolatility))
                {
                    dtOriginal = WindsorContainerManager.GetVolatilityValueDateWise(_dateSelected, _dateSelected, 0);
                }
                else if (strSelectedTab.Equals(TabName_DailyDividendYield))
                {
                    dtOriginal = WindsorContainerManager.GetDividendYieldValueDateWise(_dateSelected, _dateSelected, 0);
                }
                else if (strSelectedTab.Equals(TabName_VWAP))
                {
                    dtOriginal = WindsorContainerManager.GetVWAPValueDateWise(_dateSelected, 0, _getSameDayClosedDataOnDV);
                }

                UltraGridRow[] FilteredRows = grdPivotDisplay.Rows.GetFilteredInNonGroupByRows();
                List<String> symbols = new List<string>();
                Dictionary<int, List<String>> accountSymbols = new Dictionary<int, List<String>>();
                if (strSelectedTab.Equals(TabName_MarkPrice))
                {
                    foreach (UltraGridRow row in FilteredRows)
                    {
                        if (accountSymbols.ContainsKey(Convert.ToInt32(row.Cells["AccountID"].Value.ToString())))
                        {
                            List<String> tempSymbols = accountSymbols[Convert.ToInt32(row.Cells["AccountID"].Value.ToString())];
                            if (!tempSymbols.Contains(row.Cells["Symbol"].Value.ToString()))
                            {
                                tempSymbols.Add(row.Cells["Symbol"].Value.ToString());
                            }
                        }
                        else
                        {
                            List<String> tempSymbols = new List<string>();
                            tempSymbols.Add(row.Cells["Symbol"].Value.ToString());
                            accountSymbols.Add(Convert.ToInt32(row.Cells["AccountID"].Value.ToString()), tempSymbols);
                        }
                    }
                }
                else
                {
                    foreach (UltraGridRow row in FilteredRows)
                    {
                        if (!symbols.Contains(row.Cells["Symbol"].Value.ToString()))
                        {
                            symbols.Add(row.Cells["Symbol"].Value.ToString());
                        }
                    }
                }

                if (dtOriginal != null)
                {
                    DataTable dtClone = dtOriginal.Clone();

                    foreach (DataRow dr in dtOriginal.Rows)
                    {
                        if (strSelectedTab.Equals(TabName_MarkPrice))
                        {
                            if (accountSymbols != null && accountSymbols.ContainsKey(Convert.ToInt32(dr["AccountID"].ToString())) && accountSymbols[Convert.ToInt32(dr["AccountID"].ToString())].Contains(dr["Symbol"].ToString()))
                            {
                                dtClone.ImportRow(dr);
                            }
                        }
                        else
                        {
                            if (symbols != null && symbols.Contains(dr["Symbol"].ToString()))
                            {
                                dtClone.ImportRow(dr);
                            }
                        }
                    }
                    ConvertBlankNumericValues(dtClone);
                    DataTable dtFinalCopied = (DataTable)grdPivotDisplay.DataSource;
                    // Kuldeep Ag: Copy from functionality was not handled properly in case of Betas, So added this.
                    if (strSelectedTab.Equals(TabName_Beta) || strSelectedTab.Equals(TabName_TradingVol) || strSelectedTab.Equals(TabName_Delta) || strSelectedTab.Equals(TabName_DailyVolatility) || strSelectedTab.Equals(TabName_DailyDividendYield) || strSelectedTab.Equals(TabName_VWAP))
                    {
                        foreach (DataRow dr in dtClone.Rows)
                        {
                            foreach (DataRow drow in dtFinalCopied.Rows)
                            {
                                if (dr["Symbol"].ToString().Equals(drow["Symbol"].ToString()))
                                {
                                    drow[4] = dr[4];
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dtClone.Rows)
                        {
                            foreach (DataRow drow in dtFinalCopied.Rows)
                            {
                                if (dr["Symbol"].ToString().Equals(drow["Symbol"].ToString()) && Convert.ToInt32(dr["AccountID"].ToString()) == Convert.ToInt32(drow["AccountID"].ToString()))
                                {
                                    if (dtFinalCopied.Columns.Contains("ForwardPoints"))
                                    {
                                        if (dtClone.Columns.Contains("ForwardPoints"))
                                        {
                                            drow["ForwardPoints"] = dr["ForwardPoints"];
                                            drow[8] = dr[8];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (strSelectedTab.Equals(TabName_Delta) || strSelectedTab.Equals(TabName_TradingVol) || strSelectedTab.Equals(TabName_Beta) || strSelectedTab.Equals(TabName_DailyVolatility) || strSelectedTab.Equals(TabName_DailyDividendYield) || strSelectedTab.Equals(TabName_VWAP))
                    {
                        dtFinalCopied.Columns[4].ColumnName = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                    }
                    grdPivotDisplay.DataSource = dtFinalCopied;
                    ResetFilterAfterRebindingGrid();
                    _dtGridDataSource = dtFinalCopied;
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

        private DataTable GetTableFromSymbolMarkPriceDict(Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo)
        {
            DataTable dt = null;
            try
            {

                if (accountSymbolMarkPriceInfo != null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Symbol", typeof(System.String));
                    dt.Columns.Add("Account", typeof(System.String));
                    dt.Columns.Add("AccountID", typeof(System.Int32));
                    dt.Columns.Add("AUECID", typeof(System.Int32));
                    dt.Columns.Add("AUECIdentifier", typeof(System.String));
                    dt.Columns.Add("ForwardPoints", typeof(System.Double));
                    dt.Columns.Add("FxRate", typeof(System.Double));
                    dt.Columns.Add("BloombergSymbol", typeof(System.String));
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["Symbol"], dt.Columns["AccountID"] };
                }
                else
                {
                    return dt;
                }

                foreach (DateTime markPriceDate in accountSymbolMarkPriceInfo.Keys)
                {
                    Dictionary<int, Dictionary<string, MarkPriceInfo>> symbolMarkPriceInfo = accountSymbolMarkPriceInfo[markPriceDate];
                    foreach (int AccountID in symbolMarkPriceInfo.Keys)
                    {
                        string date;
                        if (_methodologySelected.Equals(MethdologySelected.Daily))
                            date = dtDateMonth.DateTime.ToString("MM/dd/yyyy");
                        else
                            date = markPriceDate.Date.ToString("MM/dd/yyyy");

                        if (!dt.Columns.Contains(date))
                        {
                            dt.Columns.Add(date, typeof(System.Double));
                        }

                        Dictionary<string, MarkPriceInfo> symbolMarkInfo = symbolMarkPriceInfo[AccountID];
                        foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in symbolMarkInfo)
                        {
                            Object[] objArray = { symbolMarkKeyValue.Key, symbolMarkKeyValue.Value.AccountID };
                            DataRow symbolRow = dt.Rows.Find(objArray);
                            if (symbolRow == null)
                            {
                                symbolRow = dt.NewRow();
                                symbolRow["Symbol"] = symbolMarkKeyValue.Key;
                                symbolRow["Account"] = symbolMarkKeyValue.Value.AccountID > 0 ? CachedDataManager.GetInstance.GetAccountsWithFullName()[symbolMarkKeyValue.Value.AccountID] : string.Empty;
                                symbolRow["AccountID"] = symbolMarkKeyValue.Value.AccountID;
                                symbolRow["AUECID"] = symbolMarkKeyValue.Value.AUECID;
                                symbolRow["AUECIdentifier"] = symbolMarkKeyValue.Value.AUECIdentifier;
                                symbolRow["FxRate"] = symbolMarkKeyValue.Value.FxRate;
                                symbolRow["BloombergSymbol"] = symbolMarkKeyValue.Value.BloombergSymbol;
                                dt.Rows.Add(symbolRow);
                            }

                            if (markPriceDate.Date.Equals(symbolMarkKeyValue.Value.DateActual.Date))
                            {
                                symbolRow[date] = symbolMarkKeyValue.Value.MarkPrice;
                                double forwardPoints = symbolMarkKeyValue.Value.ForwardPoints;
                                symbolRow["ForwardPoints"] = forwardPoints;
                            }
                            else
                            {
                                symbolRow[date] = 0;
                                symbolRow["ForwardPoints"] = 0;
                            }
                        }
                    }
                }
                dt.AcceptChanges();
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
            return dt;
        }

        #region Forex Section
        private void SetupForexConversion()
        {
            try
            {
                cmbExchangeGroup.Visible = false;
                lblExchangeGroup.Visible = false;
                btnGetLiveFeedData.Enabled = true;
                optUseImportExport.Visible = false;
                txtSymbolFilteration.Clear();
                grpBoxImportExport.Visible = false;
                grpBoxFilter.Visible = false;
                BindComboBoxes();

                grdPivotDisplay.AllowDrop = false;
                btnImport.Visible = false;
                btnExport.Visible = false;
                optDaily.Visible = true;
                optMonthly.Visible = true;
                //Rahul 20120307  Details: http://jira.nirvanasolutions.com:8080/browse/PRANA-1844
                optPreviousPrice.Visible = true;
                optIMidPrice.Visible = true;
                optMidPrice.Visible = true;
                optSelectedFeedPrice.Visible = true;
                optLastPrice.Visible = true;
                optLastPrice.Text = "Last Price";
                lblUpdatePrice.Visible = true;
                lblUpdatePrice.Text = "Update Price:";
                lblLastDate.Visible = true;
                lblPreviousDate.Visible = true;
                lblIMidDate.Visible = true;
                lblMidDate.Visible = true;
                lblSelectedFeedDate.Visible = true;
                grpBoxLiveFeedHandling.Visible = true;
                grpSelectDateMethodology.Visible = true;
                btnGetLiveFeedData.Text = "Get Prices";
                grpBoxLiveFeedHandling.Text = "Update Prices";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string BindGridForForexConversion(string dataCopied)
        {
            string result = string.Empty;
            try
            {
                DataSet currencyPairs = CommonDataCache.WindsorContainerManager.GetAllStandardCurrencyPairs();
                _allStandardPairs = currencyPairs.Tables[0];
                _allNonExistingFxCurrencyPairs = currencyPairs.Tables[1];
                result = string.Join(",", _allNonExistingFxCurrencyPairs.AsEnumerable().Select(row => row["LeadCurrencyID"].ToString() + "-" + row["VsCurrencyID"].ToString()));

                //Get Convertion Rate data for symbols for the selected date and methodology.
                DataTable dtConvertionRate = new DataTable();
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtConvertionRate = WindsorContainerManager.GetConversionRateDateWise(_dateSelected, 0);
                }
                else
                {
                    dtConvertionRate = WindsorContainerManager.GetConversionRateDateWise(_dateSelected, 2);
                }

                DataColumn dt = new DataColumn("Account");
                dtConvertionRate.Columns.Add(dt);

                //Assigning the mark price 0 to the symbols whose mark price is blank.
                int colLength = dtConvertionRate.Columns.Count;

                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtConvertionRate.Columns[5].ColumnName = dtDateMonth.DateTime.ToString("MM/dd/yyyy");
                }
                if (dtConvertionRate.Rows.Count > 0)
                {
                    foreach (DataRow dRow in dtConvertionRate.Rows)
                    {
                        for (int i = 0; i < colLength; i++)
                        {
                            if (dRow[i].ToString().Equals(""))
                            {
                                dRow[i] = 0;
                            }
                        }

                        if (optDaily.Checked)
                        {
                            int fromCurrency, toCurrency;
                            double conversionRate;
                            if (int.TryParse(dRow["FromCurrencyID"].ToString(), out fromCurrency) && int.TryParse(dRow["ToCurrencyID"].ToString(), out toCurrency) && double.TryParse(dRow[5].ToString(), out conversionRate) && conversionRate > 0)
                            {
                                dRow["Summary"] = "1" + CachedDataManager.GetInstance.GetCurrencyText(fromCurrency) + " = " + conversionRate + " " + CachedDataManager.GetInstance.GetCurrencyText(toCurrency);
                            }
                        }

                        dRow["Account"] = Convert.ToInt32(dRow["AccountID"]) > 0 ? CachedDataManager.GetInstance.GetAccountsWithFullName()[Convert.ToInt32(dRow["AccountID"])] : string.Empty;
                    }
                    dtConvertionRate.AcceptChanges();

                    GetListName(tabName);

                    grdPivotDisplay.DataSource = null;
                    grdPivotDisplay.DataSource = dtConvertionRate;
                    if (!string.IsNullOrEmpty(dataCopied))
                    {
                        grdPivotDisplay.Text = dataCopied;
                    }
                    _dtGridDataSource = dtConvertionRate;
                }
                else
                {
                    grdPivotDisplay.DataSource = null;
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
            return result;
        }

        internal int SaveForexConversionData(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtForexConversion = null;
                //Getting the forex converion data from the control to save it.
                dtForexConversion = dtDataToSave;
                if (dtForexConversion != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtForexConversionTemp = new DataTable();
                    dtForexConversionTemp.Columns.Add(new DataColumn("FromCurrencyID"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("ToCurrencyID"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("ConversionType"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("Date"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("ConversionFactor"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("Symbol"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("OriginalConversionFactor"));
                    dtForexConversionTemp.Columns.Add(new DataColumn("AccountID"));
                    DataTable dtDeletedForexConversionData = dtForexConversionTemp.Copy();
                    foreach (DataRow dr in dtForexConversion.Rows)
                    {
                        _columnsChanged.Clear();
                        foreach (DataColumn dc in dtForexConversion.Columns)
                        {
                            if (!_columnsChanged.Contains(dc.ColumnName) && hasCellChanged(dr, dc, grdPivotDisplay.Text))
                                _columnsChanged.Add(dc.ColumnName);

                            if (dc.ColumnName != "FromCurrencyID" && dc.ColumnName != "ToCurrencyID" && dc.ColumnName != "Symbol" && dc.ColumnName != "Summary" && dc.ColumnName != "Account" && dc.ColumnName != "AccountID")
                            {
                                if (_columnsChanged.Contains(dc.ColumnName) && double.Parse(dr[dc.ColumnName].ToString()) > 0)
                                {
                                    DataRow drNew = dtForexConversionTemp.NewRow();
                                    drNew["FromCurrencyID"] = dr["FromCurrencyID"];
                                    drNew["ToCurrencyID"] = dr["ToCurrencyID"];
                                    drNew["AccountID"] = dr["AccountID"];
                                    if (dr["Symbol"] != System.DBNull.Value || dr["Symbol"].ToString() != string.Empty)
                                    {
                                        drNew["Symbol"] = dr["Symbol"];
                                    }
                                    drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                    drNew["ConversionFactor"] = dr[dc.ColumnName];
                                    if (dr.RowState != DataRowState.Added)
                                        drNew["OriginalConversionFactor"] = dr[dc.ColumnName, DataRowVersion.Original];
                                    else
                                        drNew["OriginalConversionFactor"] = 0;

                                    if (drNew["FromCurrencyID"] != System.DBNull.Value && int.Parse(drNew["FromCurrencyID"].ToString()) != 0 && drNew["ToCurrencyID"] != System.DBNull.Value && int.Parse(drNew["ToCurrencyID"].ToString()) != 0 && drNew["ConversionFactor"] != System.DBNull.Value)
                                    {
                                        if (double.Parse(drNew["ConversionFactor"].ToString()) > 0)
                                        {
                                            drNew["ConversionFactor"] = Convert.ToDouble(dr[dc.ColumnName].ToString());
                                            dtForexConversionTemp.Rows.Add(drNew);
                                            dtForexConversionTemp.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (dtDeletedForexConversionData != null && dtDeletedForexConversionData.Rows.Count > 0)
                    {
                        AddDailyDataAuditEntry(dtDeletedForexConversionData, Prana.BusinessObjects.TradeAuditActionType.ActionType.ForexRate_Changed, "Forex Rate Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                    }
                    if (dtForexConversionTemp != null && dtForexConversionTemp.Rows.Count > 0)
                    {
                        AddDailyDataAuditEntry(dtForexConversionTemp, Prana.BusinessObjects.TradeAuditActionType.ActionType.ForexRate_Changed, "Forex Rate Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        AuditManager.Instance.SaveAuditListForDailyValuation(_tradeAuditCollection_DailyValuation);
                        _tradeAuditCollection_DailyValuation.Clear();
                        dtForexConversionTemp.Columns.Remove("OriginalConversionFactor");
                        dtForexConversionTemp.TableName = "Table1";
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveForexRate(dtForexConversionTemp);
                        _allStandardPairs = CommonDataCache.WindsorContainerManager.GetAllStandardCurrencyPairs().Tables[0];
                        if (rowsAffected > 0)
                        {
                            _dtGridDataSource.AcceptChanges();
                            _isDataCopiedFromBackDate = false;
                            CashManagementServices.InnerChannel.UpdateDayEndBaseCashByForexRate(dtForexConversionTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        #endregion

        /// <summary>
        /// Adds entry to the Audit List for Mark Price and Forex Rate Changed
        /// </summary>
        /// <param name="dt">Not Null, the Data Table from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the change by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddDailyDataAuditEntry(DataTable dt, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (dt != null && comment != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.OriginalDate = DateTime.Parse(row["Date"].ToString());
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.GroupID = int.MinValue.ToString();
                        newEntry.TaxLotClosingId = "";
                        newEntry.TaxLotID = "";
                        newEntry.OrderSideTagValue = "";
                        newEntry.OriginalValue = string.Empty;

                        switch (action)
                        {
                            case TradeAuditActionType.ActionType.MarkPrice_Changed:
                                newEntry.OriginalValue = row["OriginalMarkPrice"].ToString();
                                newEntry.Symbol = row["Symbol"].ToString();
                                newEntry.Level1ID = Convert.ToInt32(row["AccountID"].ToString());
                                break;

                            case TradeAuditActionType.ActionType.ForexRate_Changed:
                                newEntry.OriginalValue = row["OriginalConversionFactor"].ToString();
                                newEntry.Symbol = row["Symbol"].ToString();
                                newEntry.Level1ID = Convert.ToInt32(row["AccountID"].ToString());
                                break;

                            case TradeAuditActionType.ActionType.DailyFundNAV_Changed:
                                newEntry.Level1ID = Convert.ToInt32(row["FundID"].ToString());
                                newEntry.OriginalValue = row["OriginalNAV", DataRowVersion.Original].ToString();
                                break;

                            case TradeAuditActionType.ActionType.DailyFundCash_Changed:
                                newEntry.Level1ID = Convert.ToInt32(row["FundID"].ToString());
                                int currencyID = Convert.ToInt32(row["LocalCurrencyID"].ToString());
                                newEntry.Symbol = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                                newEntry.OriginalValue = row["CashValueLocal", DataRowVersion.Original].ToString();
                                break;

                            case TradeAuditActionType.ActionType.CollateralInterest_Changed:
                                newEntry.Level1ID = Convert.ToInt32(row["FundID"].ToString());
                                newEntry.OriginalValue = row["Spread", DataRowVersion.Original].ToString();
                                break;
                        }
                        newEntry.Source = TradeAuditActionType.ActionSource.DailyValuation;
                        _tradeAuditCollection_DailyValuation.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
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
            return true;
        }

        #region NAV Section
        bool isDataValidToSaveForNav = true;
        private void SetupNAV()
        {
            isDataValidToSaveForNav = true;
            BindAccounts();
            grpBoxLiveFeedHandling.Visible = false;
            txtSymbolFilteration.Clear();
            grdPivotDisplay.AllowDrop = false;
            btnGetFilteredData.Visible = false;
            btnClearFilter.Visible = false;
            txtSymbolFilteration.Visible = false;
            lblFilteredSymbol.Visible = false;
            lblSelectDateView.Visible = true;
            optDaily.Visible = true;
            optMonthly.Visible = true;
            btnGetLiveFeedData.Text = "Get Prices";
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForNAV()
        {
            //Get Convertion Rate data for symbols for the selected date and methodology.
            DataTable dtNAVValues = new DataTable();
            if (_methodologySelected.Equals(MethdologySelected.Daily))
            {
                dtNAVValues = WindsorContainerManager.GetNAVValueDateWise(_dateSelected, _dateSelected, 0);
            }
            else if (_methodologySelected.Equals(MethdologySelected.Weekly))
            {
                //dtMarkPrices = MarkPositionManager.GetMarkPricesForGivenDate(_dateSelected, 0, 0, _dateSelected, _dateSelected, _methodologySelected);
            }
            else
            {
                dtNAVValues = WindsorContainerManager.GetNAVValueDateWise(_dateSelected, _dateSelected, 2);
            }

            //Assigning the mark price 0 to the symbols whose mark price is blank.
            int colLength = dtNAVValues.Columns.Count;
            foreach (DataRow dRow in dtNAVValues.Rows)
            {
                for (int i = 0; i < colLength; i++)
                {
                    if (dRow[i].ToString().Equals(""))
                    {
                        dRow[i] = 0;
                        dRow.AcceptChanges();
                    }
                }
            }
            if (dtNAVValues.Rows.Count == 0)
            {
                DataRow dtRow = dtNAVValues.NewRow();
                dtNAVValues.Rows.Add(dtRow);
                dtRow.AcceptChanges();
            }

            if (_methodologySelected.Equals(MethdologySelected.Daily))
            {
                dtNAVValues.Columns[1].ColumnName = dtDateMonth.DateTime.ToString("MM/dd/yyyy");
            }
            GetListName(tabName);
            dtNAVValues.AcceptChanges();
            grdPivotDisplay.DataSource = null;
            grdPivotDisplay.DataSource = dtNAVValues;
            _dtGridDataSource = dtNAVValues;
        }

        private int GetAccountNotExistsForMasterFund(ValueList accountList)
        {
            foreach (var item in accountList.ValueListItems)
            {
                string account = item.DisplayText;
                if (!grdPivotDisplay.Rows.Any(x => x.Cells["FundID"].Text.Equals(account)))
                {
                    return (int)item.DataValue;
                }
            }
            return 0;
        }

        private bool ValidateRowForNAV()
        {
            bool isValid = true;
            string account = grdPivotDisplay.ActiveRow.Cells["FundID"].Text;
            int currentIndex = grdPivotDisplay.ActiveRow.Index;
            int checkIndex = 0;
            if (String.IsNullOrEmpty(account))
            {
                isValid = false;
                return isValid;
            }

            //If the same Account already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdPivotDisplay.Rows)
            {
                string dAccount = dr.Cells["FundID"].Text;
                checkIndex = dr.Index;
                if (account == dAccount && checkIndex != currentIndex)
                {
                    isValid = false;
                    InformationMessageBox.Display("Account already exists,please select different one.", "NAV");
                    break;
                }
            }
            return isValid;
        }

        internal int SaveNAVValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtNAVValue = null;

                //Getting the forex converion data from the control to save it.
                dtNAVValue = dtDataToSave;
                if (dtNAVValue != null && isDataValidToSaveForNav)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtNAVValueTemp = new DataTable();
                    dtNAVValueTemp.Columns.Add(new DataColumn("Date"));
                    dtNAVValueTemp.Columns.Add(new DataColumn("FundID"));
                    dtNAVValueTemp.Columns.Add(new DataColumn("NAVValue"));
                    dtNAVValueTemp.Columns.Add(new DataColumn("OriginalNAV"));
                    foreach (DataRow dr in dtNAVValue.Rows)
                    {
                        foreach (DataColumn dc in dtNAVValue.Columns)
                        {
                            if (dc.ColumnName != "NAVValue" && dc.ColumnName != "FundID" && dc.ColumnName != "MasterFund")
                            {
                                DataRow drNew = dtNAVValueTemp.NewRow();
                                drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);

                                if (dr["FundID"] != System.DBNull.Value)
                                    drNew["FundID"] = dr["FundID"];
                                else
                                {
                                    InformationMessageBox.Display("Please Select A Account !", "NAV");
                                    isDataValidToSaveForNav = false;
                                    break;
                                }
                                if (dr[dc.ColumnName] == System.DBNull.Value)
                                {
                                    InformationMessageBox.Display("Value Can't be NULL !", "NAV");
                                    isDataValidToSaveForNav = false;
                                    break;
                                }
                                if (drNew["Date"] != System.DBNull.Value)
                                {
                                    drNew["NAVValue"] = Math.Round(Convert.ToDouble(dr[dc.ColumnName].ToString()), 4);
                                    drNew["OriginalNAV"] = dr[dc.ColumnName, DataRowVersion.Original];
                                    dtNAVValueTemp.Rows.Add(drNew);
                                    dtNAVValueTemp.AcceptChanges();
                                }
                            }
                        }
                        if (!isDataValidToSaveForNav)
                            break;
                    }
                    if (dtNAVValueTemp != null && dtNAVValueTemp.Rows.Count > 0 && isDataValidToSaveForNav)
                    {
                        rowsAffected = WindsorContainerManager.SaveNAVValues(dtNAVValueTemp);
                        if (rowsAffected > 0)
                        {
                            AddDailyDataAuditEntry(dtNAVValueTemp, Prana.BusinessObjects.TradeAuditActionType.ActionType.DailyFundNAV_Changed, "Fund NAV Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                            AuditManager.Instance.SaveAuditList(_tradeAuditCollection_DailyValuation);
                            _tradeAuditCollection_DailyValuation.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        #endregion

        #region Daily/DayEnd Cash Section
        private void SetupDailyCash()
        {
            BindComboBoxes();
            cmbExchangeGroup.Visible = false;
            lblExchangeGroup.Visible = false;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = false;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            optDaily.Visible = true;
            optMonthly.Visible = true;
            grpBoxFilter.Visible = false;
            grpBoxLiveFeedHandling.Visible = false;
            lblFilteredSymbol.Visible = false;
            txtSymbolFilteration.Visible = false;
            btnClearFilter.Visible = false;
            btnGetFilteredData.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            lblSelectDateView.Visible = true;
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForDailyCash()
        {
            //Get daily traded cash values for the date selected.
            if (_tabPageSelected.Equals(TabName_DailyCash))
            {
                DataTable dtDailyCash = new DataTable();
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtDailyCash = WindsorContainerManager.GetDailyCash(_dateSelected, 0);
                }
                else if (_methodologySelected.Equals(MethdologySelected.Monthly))
                {
                    dtDailyCash = WindsorContainerManager.GetDailyCash(_dateSelected, 2);
                }
                GetListName(tabName);
                grdPivotDisplay.DataSource = null;
                grdPivotDisplay.DataSource = dtDailyCash;
            }
        }

        private void SetUpCollateralInterest()
        {
            BindAccounts();
            grpBoxLiveFeedHandling.Visible = false;
            txtSymbolFilteration.Clear();
            grdPivotDisplay.AllowDrop = false;
            btnGetFilteredData.Visible = true;
            btnClearFilter.Visible = false;
            txtSymbolFilteration.Visible = false;
            lblFilteredSymbol.Visible = false;
            lblSelectDateView.Visible = true;
            optDaily.Visible = true;
            optMonthly.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }
        private void BindGridForCollateralInterest()
        {
            if (_tabPageSelected.Equals(TabName_CollateralInterest))
            {
                DataTable dtCollateralinterest = new DataTable();
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtCollateralinterest = WindsorContainerManager.GetCollateralInterest(_dateSelected, 0);
                    /*dtCollateralinterest.Columns.Add("Account", typeof(System.String));
                    dtCollateralinterest.Columns.Add("Benchmark Name", typeof(System.String));
                    dtCollateralinterest.Columns.Add("Benchmark Rate", typeof(System.String));
                    dtCollateralinterest.Columns.Add("Spread", typeof(System.String));*/
                }
                GetListName(tabName);
                grdPivotDisplay.DataSource = null;
                grdPivotDisplay.DataSource = dtCollateralinterest;
            }
        }

        internal int SaveDailyCashValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDailyCashValues = null;

                //Getting the daily cash values from the control to save it.
                dtDailyCashValues = dtDataToSave;
                GenericBindingList<CompanyAccountCashCurrencyValue> lsDayEndToSave;
                if (dtDailyCashValues != null)
                {
                    lsDayEndToSave = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                    CompanyAccountCashCurrencyValue _dayEnd;
                    Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> DateWiseDayEndDictionary = new Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>>();
                    foreach (DataRow dr in dtDailyCashValues.Rows)
                    {
                        _dayEnd = new CompanyAccountCashCurrencyValue();
                        if (dr["CashValueBase"].Equals(System.DBNull.Value))
                        {
                            dr["CashValueBase"] = 0;
                        }
                        if (dr["CashValueLocal"].Equals(System.DBNull.Value))
                        {
                            dr["CashValueLocal"] = 0;
                        }
                        _dayEnd.AccountID = Convert.ToInt32(dr["FundID"]);
                        _dayEnd.BaseCurrencyID = Convert.ToInt32(dr["BaseCurrencyID"]);
                        _dayEnd.CashValueBase = Convert.ToDecimal(dr["CashValueBase"]);
                        _dayEnd.Date = Convert.ToDateTime(dr["Date"]);
                        _dayEnd.LocalCurrencyID = Convert.ToInt32(dr["LocalCurrencyID"]);
                        _dayEnd.CashValueLocal = Convert.ToDecimal(dr["CashValueLocal"]);
                        if (DateWiseDayEndDictionary.ContainsKey(_dayEnd.Date.ToShortDateString()))
                            DateWiseDayEndDictionary[_dayEnd.Date.ToShortDateString()].Add(_dayEnd);
                        else
                        {
                            lsDayEndToSave = new GenericBindingList<CompanyAccountCashCurrencyValue>();
                            lsDayEndToSave.Add(_dayEnd);
                            DateWiseDayEndDictionary.Add(_dayEnd.Date.ToShortDateString(), lsDayEndToSave);
                        }
                        _dateSelected = (DateTime)dtDateMonth.Value;
                    }
                    //Code Change:20120228:Ishant Kathuria-It will now give proper 
                    //message that data is saved if all the rows are deleted on daily cash tab
                    if (lsDayEndToSave != null)
                    {
                        //no change would be required here as the default value for the second parameter is null
                        CashManagementServices.InnerChannel.SaveDayEndData(DateWiseDayEndDictionary, _lstDeletedData, int.MinValue);
                        rowsAffected++;

                        AddDailyDataAuditEntry(dtDataToSave, Prana.BusinessObjects.TradeAuditActionType.ActionType.DailyFundCash_Changed, "Fund Currency Cash Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        AuditManager.Instance.SaveAuditList(_tradeAuditCollection_DailyValuation);
                        _tradeAuditCollection_DailyValuation.Clear();
                    }
                    dtDailyCashValues.AcceptChanges();
                    _lstDeletedData.Clear();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        internal int SaveCollateralInterestValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtCollateralInterestValues = null;

                //Getting the CI values from the control to save it.
                dtCollateralInterestValues = dtDataToSave;
                GenericBindingList<CollateralInterestValue> lsCIToSave;
                if (dtCollateralInterestValues != null)
                {
                    lsCIToSave = new GenericBindingList<CollateralInterestValue>();
                    CollateralInterestValue _CI;
                    Dictionary<string, GenericBindingList<CollateralInterestValue>> DateWiseCIDictionary = new Dictionary<string, GenericBindingList<CollateralInterestValue>>();
                    foreach (DataRow dr in dtCollateralInterestValues.Rows)
                    {
                        _CI = new CollateralInterestValue();
                        _CI.Date = Convert.ToDateTime(dr["Date"]);
                        _CI.AccountID = Convert.ToInt32(dr["FundID"]);
                        if (dr["BenchmarkName"] != System.DBNull.Value)
                        {
                            _CI.BenchmarkName = Convert.ToString(dr["BenchmarkName"]);
                        }
                        else
                        {
                            InformationMessageBox.Display("Please Fill The Benchmark Name Field !", "Collateral Interest");
                            break;
                        }
                        if (dr["BenchmarkRate"] != System.DBNull.Value)
                        {
                            _CI.BenchmarkRate = Convert.ToInt32(dr["BenchmarkRate"]);
                        }
                        else
                        {
                            InformationMessageBox.Display("Please Fill The Benchmark Rate Field !", "Collateral Interest");
                            break;
                        }
                        if (dr["Spread"] != System.DBNull.Value)
                        {
                            _CI.Spread = Convert.ToInt32(dr["Spread"]);
                        }
                        else
                        {
                            InformationMessageBox.Display("Please Fill The Spread Field !", "Collateral Interest");
                            break;
                        }
                        if (DateWiseCIDictionary.ContainsKey(_CI.Date.ToShortDateString()))
                            DateWiseCIDictionary[_CI.Date.ToShortDateString()].Add(_CI);
                        else
                        {
                            lsCIToSave = new GenericBindingList<CollateralInterestValue>();
                            lsCIToSave.Add(_CI);
                            DateWiseCIDictionary.Add(_CI.Date.ToShortDateString(), lsCIToSave);
                        }
                        _dateSelected = (DateTime)dtDateMonth.Value;
                    }
                    //Code Change:20120228:Ishant Kathuria-It will now give proper 
                    //message that data is saved if all the rows are deleted on daily cash tab
                    if (lsCIToSave != null)
                    {
                        List<CollateralInterestValue> _LSTDeletedData = new List<CollateralInterestValue>();
                        //no change would be required here as the default value for the second parameter is null
                        CashManagementServices.InnerChannel.SaveCIData(DateWiseCIDictionary, _LSTDeletedData, int.MinValue);
                        rowsAffected++;

                        AddDailyDataAuditEntry(dtDataToSave, Prana.BusinessObjects.TradeAuditActionType.ActionType.CollateralInterest_Changed, "Collateral Interest Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        AuditManager.Instance.SaveAuditList(_tradeAuditCollection_DailyValuation);
                        _tradeAuditCollection_DailyValuation.Clear();
                    }
                    dtCollateralInterestValues.AcceptChanges();
                    // _LSTDeletedData.Clear();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        private bool CheckValidationForDailyCashTab()
        {
            try
            {
                int rowIndex = 0;
                DataTable dtDailyCash = new DataTable();
                dtDailyCash = (DataTable)grdPivotDisplay.DataSource;
                dtDailyCash.AcceptChanges();
                if (dtDailyCash.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDailyCash.Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted)
                            continue;
                        foreach (DataColumn dc in dtDailyCash.Columns)
                        {
                            if (dc.ColumnName.Equals("FundID"))
                            {
                                if (dr[dc.ColumnName] == DBNull.Value)
                                {
                                    InformationMessageBox.Display("Please select the account in the row - " + (rowIndex + 1), "Daily Cash");
                                    return false;
                                }
                            }
                            if (dc.ColumnName.Equals("LocalCurrencyID"))
                            {
                                if (int.Parse(dr[dc.ColumnName].ToString()) <= 0)
                                {
                                    InformationMessageBox.Display("Please select the local currency in the row - " + (rowIndex + 1), "Daily Cash");
                                    return false;
                                }
                            }
                        }
                        if (_methodologySelected.Equals(MethdologySelected.Daily))
                        {
                            if (dtDailyCash.Rows[rowIndex].RowState != DataRowState.Deleted)
                            {
                                dtDailyCash.Rows[rowIndex]["Date"] = dtDateMonth.DateTime.Date;
                                rowIndex = rowIndex + 1;
                                if (dr.RowState == DataRowState.Unchanged)
                                {
                                    dr.SetModified();
                                }
                            }
                        }
                        else
                        {
                            if (DBNull.Value.Equals(dr["Date"]) || dr["Date"].ToString().Length <= 8)
                            {
                                InformationMessageBox.Display("Please Select a Valid Date in the row " + (rowIndex + 1), "Daily Cash");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        private void DailyCash_AfterCellUpdate()
        {
            UltraGridRow activeRow = grdPivotDisplay.ActiveRow;
            int baseCurrencyID = (int)activeRow.Cells["BaseCurrencyID"].Value;
            int localCurrencyID = (int)activeRow.Cells["LocalCurrencyID"].Value;
            int accountID = 0;
            if (activeRow.Cells.Exists("FundID"))
            {
                accountID = (int)activeRow.Cells["FundID"].Value;
            }
            double cashValueLocal = 0.0;
            if (!string.IsNullOrEmpty(activeRow.Cells["CashValueLocal"].Text))
            {
                cashValueLocal = double.Parse(activeRow.Cells["CashValueLocal"].Text.ToString());
            }
            if (!_blnCashValueBaseUpdated)
            {
                if (!baseCurrencyID.Equals(localCurrencyID))
                {
                    activeRow.Cells["CashValueBase"].Column.CellActivation = Activation.ActivateOnly;
                    int companyID = int.Parse(CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
                    if (cashValueLocal != 0)
                    {
                        ForexConverter.GetInstance(companyID, Convert.ToDateTime(dtDateMonth.Value)).GetForexData(Convert.ToDateTime(dtDateMonth.Value));
                        //CHMW-3132	Account wise fx rate handling for expiration settlement
                        ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(companyID).GetConversionRateFromCurrenciesForGivenDate(Convert.ToInt32(localCurrencyID), Convert.ToInt32(baseCurrencyID), Convert.ToDateTime(dtDateMonth.Value), accountID);
                        if (conversionRate != null)
                        {
                            if (conversionRate.ConversionMethod.Equals(Operator.D))
                            {
                                if (conversionRate.RateValue != 0)
                                    activeRow.Cells["CashValueBase"].Value = cashValueLocal / conversionRate.RateValue;
                            }
                            else
                            {
                                activeRow.Cells["CashValueBase"].Value = cashValueLocal * conversionRate.RateValue;
                            }
                        }
                    }
                    else
                    {
                        activeRow.Cells["CashValueBase"].Value = cashValueLocal;
                    }
                }
                else
                {
                    activeRow.Cells["CashValueBase"].Value = cashValueLocal;
                    activeRow.Cells["CashValueBase"].Column.CellActivation = Activation.ActivateOnly;
                }
            }
        }
        #endregion

        #region Subscribe Section
        DuplexProxyBase<ISubscription> _SubscriptionProxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                _SubscriptionProxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _SubscriptionProxy.Subscribe(Topics.Topic_DayEndCash, null);
                _SubscriptionProxy.Subscribe(Topics.Topic_DailyCreditLimit, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region IPublishing Members
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                Publish(e, topicName);
                            };
                        this.BeginInvoke(del);
                        return;
                    }
                    switch (topicName)
                    {
                        case Topics.Topic_DayEndCash:
                            if (_tabPageSelected.Equals(TabName_DailyCash))
                                BindGridForDailyCash();//Here shouldn't be db call                        
                            break;
                        case Topics.Topic_DailyCreditLimit:
                            if (_tabPageSelected.Equals(TabName_DailyCreditLimit))
                                BindGridForDailyCreditLimit();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public string getReceiverUniqueName()
        {
            return "CtrlMarkPriceAndForexConversion";
        }
        #endregion

        #region Cash Management Proxy Section

        static ProxyBase<ICashManagementService> _CashManagementServices = null;
        public static ProxyBase<ICashManagementService> CashManagementServices
        {
            set { _CashManagementServices = value; }
            get { return _CashManagementServices; }
        }

        public static void CreateCashManagementProxy()
        {
            CashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
        }

        #endregion

        #region Start Of Month Capital Account Section
        bool isDataValidToSaveForStartOfMonthCapitalAccount = true;

        private void SetupStartOfMonthCapitalAccount()
        {
            isDataValidToSaveForStartOfMonthCapitalAccount = true;
            BindAccounts();
            cmbExchangeGroup.Visible = false;
            lblExchangeGroup.Visible = false;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = false;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            optDaily.Visible = false;
            optMonthly.Checked = true;
            optMonthly.Visible = true;
            this.optMonthly.Location = new System.Drawing.Point(6, 31);
            grpBoxFilter.Visible = false;
            grpBoxLiveFeedHandling.Visible = false;
            lblFilteredSymbol.Visible = false;
            txtSymbolFilteration.Visible = false;
            btnClearFilter.Visible = false;
            btnGetFilteredData.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            lblSelectDateView.Visible = true;
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForStartOfMonthCapitalAccount()
        {
            //Get Convertion Rate data for symbols for the selected date and methodology.
            DataTable dtStartOfMonthCapitalAccountValues = new DataTable();

            // Currently only implementation for Monthly View
            dtStartOfMonthCapitalAccountValues = WindsorContainerManager.GetStartOfMonthCapitalAccountValuesDateWise(new DateTime(_dateSelected.Year, _dateSelected.Month, 01), 0);

            //Assigning the mark price 0 to the symbols whose mark price is blank.
            int colLength = dtStartOfMonthCapitalAccountValues.Columns.Count;
            foreach (DataRow dRow in dtStartOfMonthCapitalAccountValues.Rows)
            {
                for (int i = 0; i < colLength; i++)
                {
                    if (dRow[i].ToString().Equals(""))
                    {
                        dRow[i] = 0;
                        dRow.AcceptChanges();
                    }
                }
            }
            if (dtStartOfMonthCapitalAccountValues.Rows.Count == 0)
            {
                DataRow dtRow = dtStartOfMonthCapitalAccountValues.NewRow();
                dtStartOfMonthCapitalAccountValues.Rows.Add(dtRow);
                dtRow.AcceptChanges();
            }
            if (_methodologySelected.Equals(MethdologySelected.Monthly))
            {
                dtStartOfMonthCapitalAccountValues.Columns[1].ColumnName = dtDateMonth.DateTime.ToString("MM/yyyy");
            }
            GetListName(tabName);
            grdPivotDisplay.DataSource = null;
            grdPivotDisplay.DataSource = dtStartOfMonthCapitalAccountValues;
            _dtGridDataSource = dtStartOfMonthCapitalAccountValues;
        }

        private bool ValidateRowForStartOfMonthCapitalAccount()
        {
            bool isValid = true;
            string account = grdPivotDisplay.ActiveRow.Cells["FundID"].Text;

            int currentIndex = grdPivotDisplay.ActiveRow.Index;
            int checkIndex = 0;
            if (String.IsNullOrEmpty(account))
            {
                isValid = false;
                return isValid;
            }
            //If the same Account already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdPivotDisplay.Rows)
            {
                string dAccount = dr.Cells["FundID"].Text;
                checkIndex = dr.Index;
                if (account == dAccount && checkIndex != currentIndex)
                {
                    isValid = false;
                    InformationMessageBox.Display("Account already exists,please select different one.", "Start of Month Capital Account");

                    break;
                }
            }
            return isValid;
        }

        internal int SaveStartOfMonthCapitalAccountValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtStartOfMonthCapitalAccountValue = null;

                dtStartOfMonthCapitalAccountValue = dtDataToSave;
                if (dtStartOfMonthCapitalAccountValue != null)
                {
                    isDataValidToSaveForStartOfMonthCapitalAccount = true;

                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtStartOfMonthCapitalAccountValueTemp = new DataTable();
                    dtStartOfMonthCapitalAccountValueTemp.Columns.Add(new DataColumn("Date"));
                    dtStartOfMonthCapitalAccountValueTemp.Columns.Add(new DataColumn("FundID"));
                    dtStartOfMonthCapitalAccountValueTemp.Columns.Add(new DataColumn("StartOfMonthCapitalAccount"));

                    foreach (DataRow dr in dtStartOfMonthCapitalAccountValue.Rows)
                    {
                        foreach (DataColumn dc in dtStartOfMonthCapitalAccountValue.Columns)
                        {
                            if (dc.ColumnName != "StartOfMonthCapitalAccount" && dc.ColumnName != "FundID")
                            {
                                DataRow drNew = dtStartOfMonthCapitalAccountValueTemp.NewRow();
                                drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/yyyy", null);

                                if (dr["FundID"] != System.DBNull.Value)
                                    drNew["FundID"] = dr["FundID"];
                                else
                                {
                                    InformationMessageBox.Display("Please Select A Account !", "Start of Month Capital Account");
                                    isDataValidToSaveForStartOfMonthCapitalAccount = false;
                                    break;
                                }
                                if (dr[dc.ColumnName] == System.DBNull.Value)
                                {
                                    InformationMessageBox.Display("Value Can't be NULL !", "Start of Month Capital Account");
                                    isDataValidToSaveForStartOfMonthCapitalAccount = false;
                                    break;
                                }
                                if (drNew["Date"] != System.DBNull.Value)
                                {
                                    drNew["StartOfMonthCapitalAccount"] = Math.Round(Convert.ToDouble(dr[dc.ColumnName].ToString()), 4);
                                    dtStartOfMonthCapitalAccountValueTemp.Rows.Add(drNew);
                                    dtStartOfMonthCapitalAccountValueTemp.AcceptChanges();
                                }
                            }
                        }
                        if (!isDataValidToSaveForStartOfMonthCapitalAccount)
                            break;
                    }
                    if (dtStartOfMonthCapitalAccountValueTemp != null && dtStartOfMonthCapitalAccountValueTemp.Rows.Count > 0 && isDataValidToSaveForStartOfMonthCapitalAccount)
                    {
                        rowsAffected = WindsorContainerManager.SaveStartOfMonthCapitalAccountValues(dtStartOfMonthCapitalAccountValueTemp);
                    }
                }
                else
                {
                    //For Deleting all rows for specific date
                    if (IsDataSourceChanged())
                    {
                        DateTime deletionDate = DateTime.MinValue;
                        DataTable tempDT = grdPivotDisplay.DataSource as DataTable;
                        foreach (DataColumn dc in tempDT.Columns)
                        {
                            if (dc.ColumnName != "StartOfMonthCapitalAccount" && dc.ColumnName != "FundID")
                            {
                                deletionDate = DateTime.ParseExact(dc.ColumnName, "MM/yyyy", null);
                                break;
                            }
                        }
                        rowsAffected = WindsorContainerManager.DeleteStartOfMonthCapitalAccountValues(deletionDate);
                        tempDT.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        #endregion

        #region User Defined MTD PnL Section
        bool isDataValidToSaveForUserDefinedMTDPnL = true;
        private void SetupUserDefinedMTDPnL()
        {
            isDataValidToSaveForUserDefinedMTDPnL = true;
            BindAccounts();
            cmbExchangeGroup.Visible = false;
            lblExchangeGroup.Visible = false;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = false;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            optDaily.Visible = true;
            optMonthly.Visible = true;
            grpBoxFilter.Visible = false;
            grpBoxLiveFeedHandling.Visible = false;
            lblFilteredSymbol.Visible = false;
            txtSymbolFilteration.Visible = false;
            btnClearFilter.Visible = false;
            btnGetFilteredData.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            lblSelectDateView.Visible = true;
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForUserDefinedMTDPnL()
        {
            //Get Convertion Rate data for symbols for the selected date and methodology.
            DataTable dtUserDefinedMTDPnLValues = new DataTable();
            if (_methodologySelected.Equals(MethdologySelected.Daily))
            {
                dtUserDefinedMTDPnLValues = WindsorContainerManager.GetUserDefinedMTDPnLValuesDateWise(_dateSelected, 0);
            }
            else
            {
                dtUserDefinedMTDPnLValues = WindsorContainerManager.GetUserDefinedMTDPnLValuesDateWise(_dateSelected, 1);
            }

            //Assigning the mark price 0 to the symbols whose mark price is blank.
            int colLength = dtUserDefinedMTDPnLValues.Columns.Count;
            foreach (DataRow dRow in dtUserDefinedMTDPnLValues.Rows)
            {
                for (int i = 0; i < colLength; i++)
                {
                    if (dRow[i].ToString().Equals(""))
                    {
                        dRow[i] = 0;
                        dRow.AcceptChanges();
                    }
                }
            }
            if (dtUserDefinedMTDPnLValues.Rows.Count == 0)
            {
                DataRow dtRow = dtUserDefinedMTDPnLValues.NewRow();
                dtUserDefinedMTDPnLValues.Rows.Add(dtRow);
                dtRow.AcceptChanges();
            }
            if (_methodologySelected.Equals(MethdologySelected.Daily))
            {
                dtUserDefinedMTDPnLValues.Columns[1].ColumnName = dtDateMonth.DateTime.ToString("MM/dd/yyyy");
            }

            GetListName(tabName);

            grdPivotDisplay.DataSource = null;
            grdPivotDisplay.DataSource = dtUserDefinedMTDPnLValues;
            _dtGridDataSource = dtUserDefinedMTDPnLValues;
        }

        private bool ValidateRowForUserDefinedMTDPnL()
        {
            bool isValid = true;
            string account = grdPivotDisplay.ActiveRow.Cells["FundID"].Text;

            int currentIndex = grdPivotDisplay.ActiveRow.Index;
            int checkIndex = 0;

            if (String.IsNullOrEmpty(account))
            {
                isValid = false;
                return isValid;
            }
            //If the same Account already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdPivotDisplay.Rows)
            {
                string dAccount = dr.Cells["FundID"].Text;
                checkIndex = dr.Index;
                if (account == dAccount && checkIndex != currentIndex)
                {
                    isValid = false;
                    InformationMessageBox.Display("Account already exists,please select different one.", "User Defined MTD PnL");
                    break;
                }
            }
            return isValid;
        }

        internal int SaveUserDefinedMTDPnLValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtUserDefinedMTDPnLValue = null;

                dtUserDefinedMTDPnLValue = dtDataToSave;
                if (dtUserDefinedMTDPnLValue != null)
                {
                    isDataValidToSaveForUserDefinedMTDPnL = true;

                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtUserDefinedMTDPnLValueTemp = new DataTable();
                    dtUserDefinedMTDPnLValueTemp.Columns.Add(new DataColumn("Date"));
                    dtUserDefinedMTDPnLValueTemp.Columns.Add(new DataColumn("FundID"));
                    dtUserDefinedMTDPnLValueTemp.Columns.Add(new DataColumn("UserDefinedMTDPnL"));

                    foreach (DataRow dr in dtUserDefinedMTDPnLValue.Rows)
                    {
                        foreach (DataColumn dc in dtUserDefinedMTDPnLValue.Columns)
                        {
                            if (dc.ColumnName != "UserDefinedMTDPnL" && dc.ColumnName != "FundID")
                            {
                                DataRow drNew = dtUserDefinedMTDPnLValueTemp.NewRow();
                                drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);

                                if (dr["FundID"] != System.DBNull.Value)
                                    drNew["FundID"] = dr["FundID"];
                                else
                                {
                                    InformationMessageBox.Display("Please Select A Account !", "User Defined MTD PnL");
                                    isDataValidToSaveForUserDefinedMTDPnL = false;
                                    break;
                                }
                                if (dr[dc.ColumnName] == System.DBNull.Value)
                                {
                                    InformationMessageBox.Display("Value Can't be NULL !", "User Defined MTD PnL");
                                    isDataValidToSaveForUserDefinedMTDPnL = false;
                                    break;
                                }
                                if (drNew["Date"] != System.DBNull.Value)
                                {
                                    drNew["UserDefinedMTDPnL"] = Math.Round(Convert.ToDouble(dr[dc.ColumnName].ToString()), 4);
                                    dtUserDefinedMTDPnLValueTemp.Rows.Add(drNew);
                                    dtUserDefinedMTDPnLValueTemp.AcceptChanges();
                                }
                            }
                        }
                        if (!isDataValidToSaveForUserDefinedMTDPnL)
                            break;
                    }
                    if (dtUserDefinedMTDPnLValueTemp != null && dtUserDefinedMTDPnLValueTemp.Rows.Count > 0 && isDataValidToSaveForUserDefinedMTDPnL)
                    {
                        rowsAffected = WindsorContainerManager.SaveUserDefinedMTDPnLValues(dtUserDefinedMTDPnLValueTemp);
                    }
                }
                else
                {
                    //For Deleting all rows for specific date
                    if (IsDataSourceChanged())
                    {
                        DateTime deletionDate = DateTime.MinValue;
                        DataTable tempDT = grdPivotDisplay.DataSource as DataTable;
                        foreach (DataColumn dc in tempDT.Columns)
                        {
                            if (dc.ColumnName != "UserDefinedMTDPnL" && dc.ColumnName != "FundID")
                            {
                                deletionDate = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                break;
                            }
                        }
                        rowsAffected = WindsorContainerManager.DeleteUserDefinedMTDPnLValues(deletionDate);
                        tempDT.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        #endregion

        #region Daily Credit Limit Section
        bool isDataValidToSaveForDailyCreditLimit = true;

        private void SetupDailyCreditLimit()
        {
            isDataValidToSaveForDailyCreditLimit = true;
            BindAccounts();
            cmbExchangeGroup.Visible = false;
            lblExchangeGroup.Visible = false;
            optUseImportExport.Visible = false;
            grpBoxImportExport.Visible = false;
            grdPivotDisplay.AllowDrop = false;
            grpBoxFilter.Visible = false;
            btnImport.Visible = false;
            btnExport.Visible = false;
            optDaily.Visible = false;
            optMonthly.Checked = false;
            optMonthly.Visible = false;
            this.optMonthly.Location = new System.Drawing.Point(6, 31);
            grpBoxFilter.Visible = false;
            grpBoxLiveFeedHandling.Visible = false;
            lblFilteredSymbol.Visible = false;
            txtSymbolFilteration.Visible = false;
            btnClearFilter.Visible = false;
            btnGetFilteredData.Visible = false;
            btnGetLiveFeedData.Text = "Get Prices";
            lblSelectDateView.Visible = false;
            lblCopyFromDate.Visible = false;
            dtDateMonth.Visible = false;
            dtCopyFromDate.Visible = false;
            btnFetchData.Visible = false;
            grpSelectDateMethodology.Visible = false;
            grpBoxLiveFeedHandling.Text = "Update Prices";
        }

        private void BindGridForDailyCreditLimit()
        {
            DataTable dtDailyCreditLimitValues = CashManagementServices.InnerChannel.GetDailyCreditLimitValues();

            if (dtDailyCreditLimitValues != null)
            {
                int colLength = dtDailyCreditLimitValues.Columns.Count;
                foreach (DataRow dRow in dtDailyCreditLimitValues.Rows)
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        if (dRow[i].ToString().Equals(""))
                        {
                            dRow[i] = 0;
                            dRow.AcceptChanges();
                        }
                    }
                }
                if (dtDailyCreditLimitValues.Rows.Count == 0)
                {
                    DataRow dtRow = dtDailyCreditLimitValues.NewRow();
                    dtDailyCreditLimitValues.Rows.Add(dtRow);
                    dtRow.AcceptChanges();
                }
                GetListName(tabName);

                grdPivotDisplay.DataSource = null;
                grdPivotDisplay.DataSource = dtDailyCreditLimitValues;
                _dtGridDataSource = dtDailyCreditLimitValues;
            }
        }

        private bool ValidateRowForDailyCreditLimit()
        {
            bool isValid = true;
            string account = grdPivotDisplay.ActiveRow.Cells["FundID"].Text;

            int currentIndex = grdPivotDisplay.ActiveRow.Index;
            int checkIndex = 0;

            if (String.IsNullOrEmpty(account))
            {
                isValid = false;
                return isValid;
            }
            //If the same Account already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdPivotDisplay.Rows)
            {
                string dAccount = dr.Cells["FundID"].Text;
                checkIndex = dr.Index;
                if (account == dAccount && checkIndex != currentIndex)
                {
                    isValid = false;
                    InformationMessageBox.Display("Account already exists, please select different one.", "Daily Credit Limit");

                    break;
                }
            }
            return isValid;
        }

        internal int SaveDailyCreditLimitValues(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDailyCreditLimitValue = null;
                dtDailyCreditLimitValue = dtDataToSave;

                if (dtDailyCreditLimitValue != null)
                {
                    isDataValidToSaveForDailyCreditLimit = true;

                    DataTable dtDailyCreditLimitValueTemp = new DataTable();
                    dtDailyCreditLimitValueTemp.Columns.Add(new DataColumn("FundID"));
                    dtDailyCreditLimitValueTemp.Columns.Add(new DataColumn("LongDebitLimit"));
                    dtDailyCreditLimitValueTemp.Columns.Add(new DataColumn("ShortCreditLimit"));
                    dtDailyCreditLimitValueTemp.Columns.Add(new DataColumn("LongDebitBalance"));
                    dtDailyCreditLimitValueTemp.Columns.Add(new DataColumn("ShortCreditBalance"));

                    foreach (DataRow dr in dtDailyCreditLimitValue.Rows)
                    {
                        DataRow drNew = dtDailyCreditLimitValueTemp.NewRow();
                        if (dr["FundID"] != System.DBNull.Value)
                        {
                            drNew["FundID"] = dr["FundID"];
                        }
                        else
                        {
                            InformationMessageBox.Display("Please Select A Account !", "Daily Credit Limit");
                            isDataValidToSaveForDailyCreditLimit = false;
                            break;
                        }
                        if (dr["LongDebitLimit"] == System.DBNull.Value || dr["ShortCreditLimit"] == System.DBNull.Value || dr["LongDebitBalance"] == System.DBNull.Value || dr["ShortCreditBalance"] == System.DBNull.Value)
                        {
                            InformationMessageBox.Display("Value Can't be NULL !", "Daily Credit Limit");
                            isDataValidToSaveForDailyCreditLimit = false;
                            break;
                        }
                        else
                        {
                            drNew["LongDebitLimit"] = Math.Round(Convert.ToDouble(dr["LongDebitLimit"].ToString()), 4);
                            drNew["ShortCreditLimit"] = Math.Round(Convert.ToDouble(dr["ShortCreditLimit"].ToString()), 4);
                            drNew["LongDebitBalance"] = Math.Round(Convert.ToDouble(dr["LongDebitBalance"].ToString()), 4);
                            drNew["ShortCreditBalance"] = Math.Round(Convert.ToDouble(dr["ShortCreditBalance"].ToString()), 4);
                        }
                        dtDailyCreditLimitValueTemp.Rows.Add(drNew);
                        dtDailyCreditLimitValueTemp.AcceptChanges();
                    }

                    if (dtDailyCreditLimitValueTemp != null && dtDailyCreditLimitValueTemp.Rows.Count > 0 && isDataValidToSaveForDailyCreditLimit)
                    {
                        dtDailyCreditLimitValueTemp.TableName = "Table1";
                        //Daily Credit Limit values are kind of cash, so we are using Cash Mgmt services
                        rowsAffected = CashManagementServices.InnerChannel.SaveDailyCreditLimitValues(dtDailyCreditLimitValueTemp, false);
                    }
                }
                else
                {
                    if (IsDataSourceChanged())
                    {
                        DataTable dtDailyCreditLimitEmptyValue = new DataTable();
                        dtDailyCreditLimitEmptyValue.Columns.Add(new DataColumn("FundID"));
                        dtDailyCreditLimitEmptyValue.Columns.Add(new DataColumn("LongDebitLimit"));
                        dtDailyCreditLimitEmptyValue.Columns.Add(new DataColumn("ShortCreditLimit"));
                        dtDailyCreditLimitEmptyValue.Columns.Add(new DataColumn("LongDebitBalance"));
                        dtDailyCreditLimitEmptyValue.Columns.Add(new DataColumn("ShortCreditBalance"));

                        dtDailyCreditLimitEmptyValue.TableName = "Table1";
                        //Daily Credit Limit values are kind of cash, so we are using Cash Mgmt services
                        rowsAffected = CashManagementServices.InnerChannel.SaveDailyCreditLimitValues(dtDailyCreditLimitEmptyValue, false);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        #endregion

        DialogResult copydateresult;
        private void btnFetchData_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtDateMonth.DateTime.Date != dtCopyFromDate.DateTime.Date)
                {
                    if (grdPivotDisplay.Rows.Count > 0)
                    {
                        bool flag = false;
                        string dateColumnKey = dtDateMonth.DateTime.Date.ToString("MM/dd/yyyy");
                        if ((_tabPageSelected.Equals(TabName_MarkPrice)) || (_tabPageSelected.Equals(TabName_ForexConversion)) || (_tabPageSelected.Equals(TabName_NAV)))
                        {
                            RowsCollection allrows = grdPivotDisplay.Rows;
                            foreach (UltraGridRow row in allrows)
                            {
                                if (!flag.Equals(true))
                                {
                                    string dateColumnValues = row.Cells[dateColumnKey].Value.ToString();
                                    if (!dateColumnValues.Equals("0"))
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                        else if (_tabPageSelected.Equals(TabName_DailyCash))
                        {
                            flag = true;
                        }
                        else if (_tabPageSelected.Equals(TabName_CollateralPrice))
                        {
                            RowsCollection allrows = grdPivotDisplay.Rows;
                            foreach (UltraGridRow row in allrows)
                            {
                                if (!flag.Equals(true))
                                {
                                    string collateral = row.Cells["CollateralPrice"].Value.ToString();
                                    string haircut = row.Cells["Haircut"].Value.ToString();
                                    string rebateOnMV = row.Cells["RebateOnMV"].Value.ToString();
                                    string rebateOnCollateral = row.Cells["RebateOnCollateral"].Value.ToString();
                                    if (!collateral.Equals("0") || !haircut.Equals("0") || !rebateOnMV.Equals("0") || !rebateOnCollateral.Equals("0"))
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                        else if (_tabPageSelected.Equals(TabName_PerformanceNumbers))
                        {
                            string dateColumnValues = dateColumnKey;
                            if (!dateColumnValues.Equals(string.Empty))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            RowsCollection allrows = grdPivotDisplay.Rows;
                            foreach (UltraGridRow row in allrows)
                            {
                                if (!flag.Equals(true))
                                {
                                    string dateColumnValues = row.Cells[dateColumnKey].Value.ToString();
                                    if (!dateColumnValues.Equals(string.Empty))
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                        if (flag.Equals(true))
                        {
                            copydateresult = MessageBox.Show("Do you want to override the data for  " + dtDateMonth.DateTime.ToString("MM/dd/yyyy") + "?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (copydateresult.ToString().Equals("Yes"))
                            {
                                try
                                {
                                    _isDataCopiedFromBackDate = true;
                                    _dateSelected = (DateTime)dtCopyFromDate.Value;
                                    if (_tabPageSelected.Equals(TabName_MarkPrice) || _tabPageSelected.Equals(TabName_Delta) || _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Beta))
                                    {
                                        // to copy the data only for the selected filter exchange group
                                        BindGridforSelectedTabCopydata(_tabPageSelected);
                                    }
                                    else if (_tabPageSelected.Equals(TabName_ForexConversion))
                                    {
                                        BindGridForForex(_tabPageSelected);
                                    }
                                    else
                                    {
                                        BindGridForSelectedTab(_tabPageSelected);
                                    }

                                    if (_tabPageSelected.Equals(TabName_MarkPrice))
                                    {
                                        SetExchangeGroupFilteration();
                                        SetFxFxForwardFilteration();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Invoke our policy that is responsible for making sure no secure information
                                    // gets out of our layer.
                                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                    if (rethrow)
                                    {
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            try
                            {
                                //http://jira.nirvanasolutions.com:8080/browse/KIN-50
                                _isDataCopiedFromBackDate = true;
                                _dateSelected = (DateTime)dtCopyFromDate.Value;

                                if (_tabPageSelected.Equals(TabName_MarkPrice) || _tabPageSelected.Equals(TabName_Delta) || _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_DailyVolatility) || _tabPageSelected.Equals(TabName_DailyDividendYield) || _tabPageSelected.Equals(TabName_VWAP))
                                {
                                    // to copy the data only for the selected filter exchange group
                                    BindGridforSelectedTabCopydata(_tabPageSelected);
                                }
                                else if (_tabPageSelected.Equals(TabName_ForexConversion))
                                {
                                    BindGridForForex(_tabPageSelected);
                                }
                                else
                                {
                                    BindGridForSelectedTab(_tabPageSelected);
                                }

                                if (_tabPageSelected.Equals(TabName_MarkPrice))
                                {
                                    SetExchangeGroupFilteration();
                                    SetFxFxForwardFilteration();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        DialogResult dlgResultCopy = MessageBox.Show("Do you want copy the data from " + dtCopyFromDate.DateTime.ToString("MM/dd/yyyy") + " for " + dtDateMonth.DateTime.ToString("MM/dd/yyyy") + "?", "CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgResultCopy.Equals(DialogResult.Yes))
                        {
                            _dateSelected = (DateTime)dtCopyFromDate.Value;
                            if (_tabPageSelected.Equals(TabName_MarkPrice) || _tabPageSelected.Equals(TabName_Delta) || _tabPageSelected.Equals(TabName_TradingVol) || _tabPageSelected.Equals(TabName_Beta) || _tabPageSelected.Equals(TabName_DailyVolatility) || _tabPageSelected.Equals(TabName_DailyDividendYield) || _tabPageSelected.Equals(TabName_VWAP))
                            {
                                // to copy the data only for the selected filter exchange group
                                BindGridforSelectedTabCopydata(_tabPageSelected);
                            }
                            else if (_tabPageSelected.Equals(TabName_ForexConversion))
                            {
                                BindGridForForex(_tabPageSelected);
                            }
                            else
                            {
                                BindGridForSelectedTab(_tabPageSelected);
                            }

                            if (_tabPageSelected.Equals(TabName_MarkPrice))
                            {
                                SetExchangeGroupFilteration();
                                SetFxFxForwardFilteration();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("SELECT DATE and COPY FROM dates are equal, Please select different dates", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // This method is specially created for copy from functionality of FX. So please check all references before you use it anywhere else.
        private void BindGridForForex(string tabPageSelected)
        {
            try
            {
                if (tabPageSelected.Equals(TabName_ForexConversion))
                {
                    _output = BindGridForForexConversion("DataCopied");
                    tabName = TabName_ForexConversion;

                    if (_dict_Layout.ContainsKey(TabName_ForexConversion))
                    {
                        _isDataCopiedForex = true;
                        SetForexGridFilterLayout(grdPivotDisplay, _dict_Layout[TabName_ForexConversion]);
                    }
                    else
                    {
                        ApplyAccountFilteration();
                        List<ColumnData> cd = new List<ColumnData>();
                        _dict_Layout.Add(TabName_ForexConversion, cd);
                        SetForexGridFilterLayout(grdPivotDisplay, _dict_Layout[TabName_ForexConversion]);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CtrlMarkPriceAndForexConversion_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyData == 131142)
            {
                txtSymbolFilteration.Focus();
            }
        }

        private void ConvertBlankNumericValues(DataTable dtNum)
        {
            try
            {
                if (dtNum != null && dtNum.Columns.Count > 0)
                {
                    foreach (DataRow dr in dtNum.Rows)
                    {
                        foreach (DataColumn dCol in dtNum.Columns)
                        {
                            if ((dCol.DataType == typeof(System.Single)
                               || dCol.DataType == typeof(System.Double)
                               || dCol.DataType == typeof(System.Decimal)
                               || dCol.DataType == typeof(System.Byte)
                               || dCol.DataType == typeof(System.Int16)
                               || dCol.DataType == typeof(System.Int32)
                               || dCol.DataType == typeof(System.Int64))
                               && (dr[dCol].ToString().Equals("0") || dr[dCol].ToString().Equals("")))
                            {
                                dr[dCol] = 0;
                            }
                        }
                        if (dtNum.Columns.Contains("Symbol"))
                        {
                            if (!dr["Symbol"].ToString().ToUpper().Equals(dr["Symbol"].ToString()))
                            {
                                dr["Symbol"] = dr["Symbol"].ToString().ToUpper();
                            }
                        }
                        dr.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindGrid()
        {
            _dateSelected = (DateTime)dtDateMonth.Value;
            BindGridForSelectedTab(_tabPageSelected);

            if (optOMIPrice.Checked)
            {
                btnGetLiveFeedData.Enabled = true;
            }
        }

        private void dtDateMonth_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                if (_dateTypedIndtDateMonth)
                {
                    _dateTypedIndtDateMonth = false;
                    BindGrid();
                }

                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtLiveFeed.Value = _dateSelected;
                    SetLiveFeedPriceStatus();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtLiveFeed_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                _dateTypedIndtLiveFeed = false;
                dtDateMonth.Value = (DateTime)dtLiveFeed.Value;
                SetLiveFeedPriceStatus();
                DateTime dtToBePassed = (DateTime)dtLiveFeed.Value;
                ScrollToSelectedDate(dtToBePassed);
                BindGrid();
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    dtDateMonth.Value = _dateSelected;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Added by: Bharat Raturi
        /// Delete the removed entries
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-6431
        /// </summary>
        internal void DeleteDailyCashEntries()
        {
            try
            {
                _lstDeletedData.ForEach(item => WindsorContainerManager.DeleteDailyCashValue(item.AccountID, item.LocalCurrencyID, item.BaseCurrencyID, item.Date));
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

        private void btnAccountCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCopyFromAccount.Value != null && multiSelectDropDown1.GetNoOfCheckedItems() != 0)
                {
                    CopyAccountData copyAccountData = CopyAccountDataFactory.GetAccountDataCopier(_tabPageSelected, Int32.Parse(cmbCopyFromAccount.Value.ToString()), multiSelectDropDown1.GetSelectedItemsInDictionary().Keys.ToList(), grdPivotDisplay.DataSource as DataTable);
                    copyAccountData.CopyDataToAccounts(grdPivotDisplay.DataSource as DataTable);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetLiveForexRate(double liveFeedPrice, UltraGridRow row, int fromCurrencyID, int toCurrencyID)
        {
            try
            {
                if (liveFeedPrice == double.MinValue)
                {
                    row.Cells[_colName].Value = 0;
                }
                else
                {
                    _btnGetLiveFeedClicked = true;
                    row.Cells[_colName].Value = liveFeedPrice;
                    row.Cells["Summary"].Value = "1 " + _currencyDict[fromCurrencyID] + " = " + liveFeedPrice + " " + _currencyDict[toCurrencyID];
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

        /// <summary>
        /// This method is used to validate the selected date against the NAV Lock date
        /// </summary>    
        private bool ValidateDateWithNAVLock()
        {
            try
            {
                DateTime date = _dateforNAVLockValidation;
                if (optMonthly.Checked)
                    date = new DateTime(date.Year, date.Month, 1);

                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(date))
                {
                    MessageBox.Show("The date for some of the data you’ve chosen to for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString() + "). Please reach out to your Support Team for further assistance.", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
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
            return true;
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(grdPivotDisplay, exportFilePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
