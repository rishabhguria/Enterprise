using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.Admin.BLL;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CashManagement;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.PM.Client.UI
{
    public partial class ImportPositionsDisplayForm : Form, ILaunchForm, IRefresh
    {
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        private static ImportPositionsDisplayForm _importPositionsDisplayForm = null;
        bool blnCloseforSave = false;
        const string _importTypeCash = "Cash";
        const string _importTypeStagedOrder = "Staged Order";
        const string _importTypeNetPosition = "Net Position";
        const string _importTypeTransaction = "Transaction";
        const string _importTypeMarkPrice = "Mark Price";
        const string _importTypeBeta = "DailyBeta";
        const string _importTypeActivity = "Activities";
        const string _importTypeForexPrice = "Forex Price";
        const string _importTypeSecMasterInsert = "SMInsert";
        const string _importTypeSecMasterUpdateData = "SMUpdate";
        const string CONSTSTR_TableName = "PositionMaster";
        const string _importTypeOMI = "Option Model Inputs";
        const string _importTypeAllocationScheme = "Allocation Scheme";
        const string _importTypeDailyCreditLimit = "Credit Limit";
        const string _importTypeDoubleEntryCash = "Double Entry Cash";
        const string _importTypeMultilegJournalImport = "Multileg Journal Import";
        const string _importTypeSettlementDateCash = "SettlementDate Cash";
        const string _importTypeColIntMasterInsert = "Collateral Interest";
        private const string _importTypeDailyVolatility = "Daily Volatility";
        private const string _importTypeDailyDividendYield = "Daily Dividend Yield";
        private const string _importTypeDailyVWAP = "Daily VWAP";
        private const string _importTypeCollateralPrice = "Collateral Price";
        private const string CAPTION_FEEDBACK_MESSAGE = "({0}/{1}) records have been successfully uploaded";
        public bool IsComingFromBlotter { get; set; }
        public EventHandler<EventArgs<List<OrderSingle>>> SendTradesToBlotterEvent = null;
        public EventHandler<EventArgs<int>> UpdateFeedbackMessageEvent = null;
        public EventHandler<EventArgs<bool>> UpdateAfterCloseEvent = null;
        public EventHandler<EventArgs<int>> UpdateTotalCountAfterGroupingEvent = null;
        public int NoOfDoubleEntryCashCreated { get; set; }
        public int NoOfMultiLegJournalCreated { get; set; }
        string _positionType = string.Empty;
        int _loggedInUserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        private int _symbolsSentToBlotterCount = 0;
        private int _unsuccessfulTradeCount = 0;
        private int _totalTradeCount = 0;
        private int _groupByColumnCount = 0;

        private ImportPositionsDisplayForm()
        {
            InitializeComponent();
            if (!ModuleManager.CheckModulePermissioning(btnSymbolLookup.Text, btnSymbolLookup.Text))
                btnSymbolLookup.Enabled = false;
            UpdateFeedbackMessageEvent += UpdateFeedbackMessage;
            UpdateTotalCountAfterGroupingEvent += UpdateTotalCountAfterGrouping;
        }

        public static ImportPositionsDisplayForm GetInstance()
        {
            if (_importPositionsDisplayForm == null)
            {
                _importPositionsDisplayForm = new ImportPositionsDisplayForm();
            }
            return _importPositionsDisplayForm;
        }

        //needed as cell updated is listened to for change in OrderIDs
        //we do not want the event to be raise until once the grid is binded

        //Added constant for IsSecApproved status of Security
        public const string PROPERTY_IS_APPROVED = "IsSecApproved";
        public const string PROPERTY_SEC_APPROVAL_STATUS = "SecApprovalStatus";

        #region Transaction and Positions Visible Columns
        public const string PROPERTY_SYMBOL = "Symbol";
        public const string PROPERTY_SIDE = "Side";
        public const string PROPERTY_QUANTITY = "NetPosition";
        public const string PROPERTY_AVGPRICE = "CostBasis";
        public const string PROPERTY_ASSETTYPE = "AssetType";
        public const string PROPERTY_AUECDATE = "AUECLocalDate";
        public const string PROPERTY_SETTLEMENTDATE = "PositionSettlementDate";
        public const string PROPERTY_FUND = "AccountName";
        public const string PROPERTY_PBSymbol = "PBSymbol";
        public const string PROPERTY_Validated = "Validated";
        public const string PROPERTY_IsValid = "IsValid";
        public const string PROPERTY_PBAssetType = "PBAssetType";
        public const string PROPERTY_ExecutingBroker = "ExecutingBroker";
        public const string PROPERTY_FXRate = "FXRate";
        public const string PROPERTY_FXConversionMethodOperator = "FXConversionMethodOperator";
        public const string PROPERTY_PROCESSDATE = "ProcessDate";
        public const string PROPERTY_ORIGINALPURCHASEDATE = "OriginalPurchaseDate";
        public const string PROPERTY_OTHERBROKERFEE = "Fees";
        public const string CAPTION_OTHERBROKERFEE = "Fees";
        public const string PROPERTY_COMMISSIONSOURCE = "CommissionSource";
        public const string PROPERTY_SOFTCOMMISSIONSOURCE = "SoftCommissionSource";
        public const string PROPERTY_TRANSACTIONTYPE = "TransactionType";
        public const string CAPTION_TRANSACTIONTYPE = "Transaction Type";
        #endregion

        #region CashCurrencyColumns
        public const string HEADCOLCASH_BaseCurrency = "BaseCurrency";
        public const string HEADCOLCASH_LocalCurrency = "LocalCurrency";
        public const string HEADCOLCASH_CashValueBase = "CashValueBase";
        public const string HEADCOLCASH_CashValueLocal = "CashValueLocal";
        public const string HEADCOLCASH_Date = "Date";
        public const string HEADCOLCASH_FUND = "AccountName";
        public const string HEADCOLCASH_PositionType = "PositionType";
        #endregion

        #region SettlementCashCurrencyColumns
        public const string HEADCOLSETTLEMENTCASH_BaseCurrency = "SettlementDateBaseCurrency";
        public const string HEADCOLSETTLEMENTCASH_LocalCurrency = "SettlementDateLocalCurrency";
        public const string HEADCOLSETTLEMENTCASH_CashValueBase = "SettlementDateCashValueBase";
        public const string HEADCOLSETTLEMENTCASH_CashValueLocal = "SettlementDateCashValueLocal";
        public const string HEADCOLSETTLEMENTCASH_Date = "SettlementDate";
        public const string HEADCOLSETTLEMENTCASH_FUND = "AccountName";
        public const string HEADCOLSETTLEMENTCASH_PositionType = "PositionType";
        #endregion

        #region Mark Price Import Columns
        public const string HEADCOLMARK_MarkPrice = "MarkPrice";
        public const string HEADCOLMARK_Date = "Date";
        public const string HEADCOLMARK_Symbol = "Symbol";
        #endregion

        #region Activity Import Columns
        public const string HEADCOLDIV_Amount = "Amount";
        public const string HEADCOLDIV_ExDate = "ExDate";
        public const string HEADCOLDIV_PayOutDate = "PayoutDate";
        public const string HEADCOLDIV_RecordDate = "RecordDate";
        public const string HEADCOLDIV_DeclarationDate = "DeclarationDate";
        public const string HEADCOLDIV_FUND = "AccountName";
        public const string HEADCOLDIV_Symbol = "Symbol";
        public const string HEADCOLDIV_ActivityType = "ActivityType";
        public const string HEADCOLDIV_PBSymbol = "PBSymbol";
        #endregion

        #region Beta Import Columns
        public const string HEADCOLBETA_Price = "Beta";
        public const string HEADCOLBETA_Symbol = "Symbol";
        public const string HEADCOLBETA_PBSymbol = "PBSymbol";
        public const string HEADCOLBETA_Date = "Date";
        #endregion

        #region Forex Price Import Columns
        public const string HEADCOLFOREX_ForexPrice = "ForexPrice";
        public const string HEADCOLFOREX_Date = "Date";
        public const string HEADCOLFOREX_BaseCurrency = "BaseCurrency";
        public const string HEADCOLFOREX_SettlementCurrency = "SettlementCurrency";
        #endregion

        #region Daily Credit Limit Columns
        public const string HEADCOLCREDITLIMIT_FUND = "AccountName";
        public const string HEADCOLCREDITLIMIT_LongDebitBalance = "LongDebitBalance";
        public const string HEADCOLCREDITLIMIT_ShortCreditBalance = "ShortCreditBalance";
        #endregion

        #region swap
        public const string HEADCOLSWAP_FirstResetDate = "FirstResetDate";
        public const string HEADCOLSWAP_DayCount = "DayCount";
        public const string HEADCOLSWAP_BenchMarkRate = "BenchMarkRate";
        public const string HEADCOLSWAP_Differential = "Differential";
        public const string HEADCOLSWAP_ResetFrequency = "ResetFrequency";
        public const string HEADCOLSWAP_OrigTransDate = "OrigTransDate";
        public const string HEADCOLSWAP_SwapDescription = "SwapDescription";
        #endregion

        #region Volatility Import Columns
        public const string HEADCOLVOLATILITY_Price = "Volatility";
        public const string HEADCOLVOLATILITY_Symbol = "Symbol";
        public const string HEADCOLVOLATILITY_PBSymbol = "PBSymbol";
        public const string HEADCOLVOLATILITY_Date = "Date";
        #endregion

        #region VWAP Import Columns
        public const string HEADCOLVWAP_Price = "VWAP";
        public const string HEADCOLVWAP_Symbol = "Symbol";
        public const string HEADCOLVWAP_PBSymbol = "PBSymbol";
        public const string HEADCOLVWAP_Date = "Date";
        #endregion

        #region Collateral Import Columns
        public const string HEADCOLCOLLATERAL_Price = "CollateralPrice";
        public const string HEADCOLCOLLATERAL_Haircut = "Haircut";
        public const string HEADCOLCOLLATERAL_FeeRebateMV = "RebateOnMV";
        public const string HEADCOLCOLLATERAL_FeeRebateCollateral = "RebateOnCollateral";
        public const string HEADCOLCOLLATERAL_Fund = "AccountName";
        public const string HEADCOLCOLLATERAL_Symbol = "Symbol";
        public const string HEADCOLCOLLATERAL_PBSymbol = "PBSymbol";
        public const string HEADCOLCOLLATERAL_Date = "Date";
        #endregion

        #region Dividend Yield Import Columns
        public const string HEADCOLDIVIDENDYIELD_Price = "DividendYield";
        public const string HEADCOLDIVIDENDYIELD_Symbol = "Symbol";
        public const string HEADCOLDIVIDENDYIELD_PBSymbol = "PBSymbol";
        public const string HEADCOLDIVIDENDYIELD_Date = "Date";
        #endregion

        #region Stage Yield Import Columns
        public const string HEADCOL_Account = "Account";
        public const string HEADCOL_Symbol = "Symbol";
        public const string HEADCOL_OrderSide = "OrderSide";
        public const string HEADCOL_Quantity = "Quantity";
        public const string HEADCOL_AssetName = "AssetName";
        public const string HEADCOL_Date = "TransactionTime";
        public const string HEADCOL_Strategy = "Strategy";
        public const string HEADCOL_Broker = "CounterPartyName";
        public const string HEADCOL_Venue = "Venue";
        public const string HEADCOL_TIF = "TIF";
        public const string HEADCOL_OrderType = "OrderType";
        public const string HEADCOL_ValidationStatus = "ValidationStatus";
        #endregion

        #region Collateral Interest Import Columns
        public const string HEADCOL_FundID = "Account";
        public const string HEADCOL_BenchmarkName = "BenchmarkName";
        public const string HEADCOL_BenchmarkRate = "BenchmarkRate";
        public const string HEADCOL_Spread = "Spread";
        public const string HEADCOL_CollateralValidationStatus = "ValidationStatus";
        #endregion

        #region Multileg Journal Import Columns
        public const string HEADCOL_AccountName = "AccountName";
        public const string HEADCOL_AccountSide = "AccountSide";
        public const string HEADCOL_AccountID = "FundID";
        public const string HEADCOL_DATE = "Date";
        public const string HEADCOL_Description = "Description";
        public const string HEADCOL_CurrencyName = "CurrencyName";
        public const string HEADCOL_CurrencyID = "CurrencyID";
        public const string HEADCOL_CashSubAccount = "Cash-SubAccount";
        public const string HEADCOL_FXRate = "FXRate";
        public const string HEADCOL_FXConversionMethodOperator = "FXConversionMethodOperator";
        public const string HEADCOL_EntryID = "EntryID";
        public const string HEADCOL_IsNonTradingTransaction = "IsNonTradingTransaction";
        public const string HEADCOL_PRIMARYKEY = "PRIMARY_KEY";
        public const string HEADCOL_checkBox = "checkBox";
        #endregion

        private DataSet _dsColInterstImportValue;
        public DataSet ValidatedCollateralInterestValue
        {
            get { return _dsColInterstImportValue; }
        }

        private DataSet _dsSecMasterInsert;
        public DataSet ValidatedSMInsertValue
        {
            get { return _dsSecMasterInsert; }
        }

        SerializableDictionary<string, DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, DynamicUDA>();
        private SecMasterUpdateDataByImportList _validatedSecMasterUpdateDataValues;
        public SecMasterUpdateDataByImportList ValidatedSMUpdateDataValues
        {
            get { return _validatedSecMasterUpdateDataValues; }
        }

        private List<UserOptModelInput> _validatedOmiValue;
        public List<UserOptModelInput> ValidatedOmiValue
        {
            get { return _validatedOmiValue; }
        }

        private List<PositionMaster> _validatedPositions;
        public List<PositionMaster> ValidatedPositions
        {
            get { return _validatedPositions; }
        }

        private List<CashCurrencyValue> _validatedCashCurrencyValue;
        public List<CashCurrencyValue> ValidatedCashCurrencyValue
        {
            get { return _validatedCashCurrencyValue; }
        }

        private List<SettlementDateCashCurrencyValue> _validatedSettlementDateCashCurrencyValue;
        public List<SettlementDateCashCurrencyValue> ValidatedSettlementDateCashCurrencyValue
        {
            get { return _validatedSettlementDateCashCurrencyValue; }
        }

        private List<MarkPriceImport> _validatedMarkpriceValue;
        public List<MarkPriceImport> ValidatedMarkPriceValue
        {
            get { return _validatedMarkpriceValue; }
        }

        private List<BetaImport> _validatedBetaValues;
        public List<BetaImport> ValidatedBetaValues
        {
            get { return _validatedBetaValues; }
        }

        private List<DividendImport> _validatedCashTransactionValue;
        public List<DividendImport> ValidatedCashTransactionValue
        {
            get { return _validatedCashTransactionValue; }
        }

        private List<ForexPriceImport> _validatedForexpriceValue;
        public List<ForexPriceImport> ValidatedForexPriceValue
        {
            get { return _validatedForexpriceValue; }
        }

        private List<DailyCreditLimit> _validatedDailyCreditLimitValue;
        public List<DailyCreditLimit> ValidatedDailyCreditLimitValue
        {
            get { return _validatedDailyCreditLimitValue; }
        }

        private List<VolatilityImport> _validatedDailyVolatilityValue;
        public List<VolatilityImport> ValidatedDailyVolatilityValue
        {
            get { return _validatedDailyVolatilityValue; }
        }

        private List<CollateralImport> _validatedDailyCollateralValue;
        public List<CollateralImport> ValidatedDailyCollateralValue
        {
            get { return _validatedDailyCollateralValue; }
        }

        private List<VWAPImport> _validatedDailyVWAPValue;
        public List<VWAPImport> ValidatedDailyVWAPValue
        {
            get { return _validatedDailyVWAPValue; }
        }

        private List<DividendYieldImport> _vaidatedDailyDividendYieldValue;
        public List<DividendYieldImport> ValudatedDailyDividendYieldValue
        {
            get { return _vaidatedDailyDividendYieldValue; }
        }

        private List<OrderSingle> _validatedStageOrder;
        public List<OrderSingle> ValidatedStageOrder
        {
            get { return _validatedStageOrder; }
        }

        public void BindImportPositions(List<PositionMaster> importPositions, string positionType, bool isDisplaySwapColumn)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importPositions;
            GetVisibleColumnsListForPositions(isDisplaySwapColumn);
            this.Text = "Import Positions";
        }

        public void BindImportCash(List<CashCurrencyValue> importCashCurrencyValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importCashCurrencyValues;
            this.Text = "Import Cash Values";
        }

        public void BindImportSettlementDateCash(List<SettlementDateCashCurrencyValue> importSettlementDateCashCurrencyValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importSettlementDateCashCurrencyValues;
            this.Text = "Import Settlement Date Cash Currency Values";
        }

        public void BindImportMarkPrice(List<MarkPriceImport> importMarkPriceValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importMarkPriceValues;
            this.Text = "Import Mark Prices";
        }

        public void BindImportCashTransaction(List<DividendImport> importCashTransactionValues, string positionType)
        {
            GetAllDefaults();
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importCashTransactionValues;
            this.Text = "Import Cash Transactions";
        }

        internal void BindImportBeta(List<BetaImport> importBetaValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importBetaValues;
            this.Text = "Import Beta Values";
        }

        public void BindImportForexPrice(List<ForexPriceImport> importForexPriceValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importForexPriceValues;
            this.Text = "Import Forex Prices";
        }

        public void BindImportStageorder(List<OrderSingle> importstageorder, string positionType)
        {
            try
            {
                _positionType = positionType;
                grdImportData.DisplayLayout.MaxBandDepth = 1;
                grdImportData.DataSource = null;
                grdImportData.DataSource = importstageorder;
                this.Text = "Staged Order";
                int _orderCount = 0;
                double _orderQuantity = 0;

                foreach (OrderSingle order in importstageorder)
                {
                    if (order.ValidationStatus == ApplicationConstants.ValidationStatus.Validated.ToString())
                    {
                        _orderCount++;
                        _orderQuantity += order.Quantity;
                    }
                }
                lblCount.Text = "Order Count : " + _orderCount.ToString();
                lblQuantity.Text = "Order Quantity : " + _orderQuantity.ToString();
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

        public void BindSecMasterUpdateData(List<SecMasterUpdateDataByImportUI> lstSMUpdateDataValues, string importType, SerializableDictionary<string, DynamicUDA> dynamicUDACache)
        {
            _dynamicUDACache = dynamicUDACache;
            _positionType = importType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = lstSMUpdateDataValues;
            GetAllDefaults();
            this.Text = "Security Master Update Data";
        }

        public void BindSecMasterData(DataSet ds, string importType, SerializableDictionary<string, DynamicUDA> dynamicUDACache)
        {
            _dynamicUDACache = dynamicUDACache;
            _positionType = importType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = ds;
            GetAllDefaults();
            UltraGridBand band = this.grdImportData.DisplayLayout.Bands[0];
            SetGridColumns(band);
            grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
            grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
            grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
            grdImportData.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            this.Text = "Import Security Master Data";
        }

        public void BindColMasterData(DataSet ds, string importType)
        {
            _positionType = importType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = ds;
            GetAllDefaults();
            grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
            grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
            grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
            grdImportData.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
        }

        public void BindImportOMIData(List<UserOptModelInput> lstOMIvalues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = lstOMIvalues;

            UltraGridBand band = this.grdImportData.DisplayLayout.Bands[0];
            OMIDataSetGridColumns(band);

            this.Text = "Import Option Model Input Values";
        }

        private void OMIDataSetGridColumns(UltraGridBand gridBand)
        {
            gridBand.Columns["Bloomberg"].Header.Caption = "Bloomberg Symbol";
            gridBand.Columns["Bloomberg"].Header.Column.Width = 150;
            gridBand.Columns["SecurityDescription"].Header.Caption = "Security Description";
            gridBand.Columns["SecurityDescription"].Header.Column.Width = 150;
            gridBand.Columns["UnderlyingSymbol"].Header.Caption = "Underlying Symbol";
            gridBand.Columns["UnderlyingSymbol"].Header.Column.Width = 150;
            gridBand.Columns["ProxySymbol"].Header.Caption = "Proxy Symbol";
            gridBand.Columns["ProxySymbol"].Header.Column.Width = 150;
            gridBand.Columns["PSSymbol"].Header.Caption = "PS Symbol";
            gridBand.Columns["PSSymbol"].Header.Column.Width = 150;
            gridBand.Columns["ExpirationDate"].Header.Caption = "Expiration Date";
            gridBand.Columns["ExpirationDate"].Header.Column.Width = 150;
            gridBand.Columns["Volatility"].Header.Caption = "Volatility";
            gridBand.Columns["Volatility"].Header.Column.Width = 100;
            gridBand.Columns["IntRate"].Header.Caption = "Interest Rate";
            gridBand.Columns["IntRate"].Header.Column.Width = 100;
            gridBand.Columns["Dividend"].Header.Caption = "Dividend";
            gridBand.Columns["Dividend"].Header.Column.Width = 100;
            gridBand.Columns["StockBorrowCost"].Header.Caption = "StockBorrowCost";
            gridBand.Columns["StockBorrowCost"].Header.Column.Width = 100;
            gridBand.Columns["Delta"].Header.Caption = "Proxy Symbol";
            gridBand.Columns["Delta"].Header.Column.Width = 100;
            gridBand.Columns["LastPrice"].Header.Caption = "Last Price";
            gridBand.Columns["LastPrice"].Header.Column.Width = 100;
            gridBand.Columns["ForwardPoints"].Header.Caption = "Forward Point";
            gridBand.Columns["ForwardPoints"].Header.Column.Width = 100;
            gridBand.Columns["SharesOutstanding"].Header.Caption = "Shares Outstanding";
            gridBand.Columns["SharesOutstanding"].Header.Column.Width = 100;


            if (gridBand.Columns.Exists("VolatilityUsed"))
                gridBand.Columns["VolatilityUsed"].Hidden = true;
            if (gridBand.Columns.Exists("HistoricalVolUsed"))
                gridBand.Columns["HistoricalVolUsed"].Hidden = true;
            if (gridBand.Columns.Exists("IntRateUsed"))
                gridBand.Columns["IntRateUsed"].Hidden = true;
            if (gridBand.Columns.Exists("DividendUsed"))
                gridBand.Columns["DividendUsed"].Hidden = true;
            if (gridBand.Columns.Exists("StockBorrowCost"))
                gridBand.Columns["StockBorrowCostUsed"].Hidden = true;
            if (gridBand.Columns.Exists("DeltaUsed"))
                gridBand.Columns["DeltaUsed"].Hidden = true;
            if (gridBand.Columns.Exists("LastPriceUsed"))
                gridBand.Columns["LastPriceUsed"].Hidden = true;
            if (gridBand.Columns.Exists("TheoreticalPriceUsed"))
                gridBand.Columns["TheoreticalPriceUsed"].Hidden = true;
            if (gridBand.Columns.Exists("ProxySymbolUsed"))
                gridBand.Columns["ProxySymbolUsed"].Hidden = true;
            if (gridBand.Columns.Exists("ForwardPointsUsed"))
                gridBand.Columns["ForwardPointsUsed"].Hidden = true;
            if (gridBand.Columns.Exists("IsDirtyToSave"))
                gridBand.Columns["IsDirtyToSave"].Hidden = true;
            if (gridBand.Columns.Exists("SharesOutstandingUsed"))
                gridBand.Columns["SharesOutstandingUsed"].Hidden = true;
            if (gridBand.Columns.Exists("ClosingMarkUsed"))
                gridBand.Columns["ClosingMarkUsed"].Hidden = true;
            if (gridBand.Columns.Exists("ManualInput"))
                gridBand.Columns["ManualInput"].Hidden = true;
            if (gridBand.Columns.Exists("IsHistorical"))
                gridBand.Columns["IsHistorical"].Hidden = true;
            if (gridBand.Columns.Exists("ValidationError"))
                gridBand.Columns["ValidationError"].Hidden = true;
            if (gridBand.Columns.Exists("ImportStatus"))
                gridBand.Columns["ImportStatus"].Hidden = true;
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_VSCURRENCYID))
                gridBand.Columns[OrderFields.PROPERTY_VSCURRENCYID].Hidden = true;
            if (gridBand.Columns.Exists(OrderFields.PROPERTY_LEADCURRENCYID))
                gridBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID].Hidden = true;
            if (gridBand.Columns.Exists("Symbology"))
                gridBand.Columns["Symbology"].Hidden = true;
            if (gridBand.Columns.Exists("PersistenceStatus"))
                gridBand.Columns["PersistenceStatus"].Hidden = true;
            if (gridBand.Columns.Exists("PBSymbol"))
                gridBand.Columns["PBSymbol"].Hidden = true;
        }

        public void BindImportDailyCreditLimit(List<DailyCreditLimit> importDailyCreditLimitValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importDailyCreditLimitValues;
            this.Text = "Import Daily Credit Limit Values";
        }

        public void BindImportedMultilegJournalImport(DataSet ds, string _userSelectedDate, int _userSelectedAccountValue, string positionType, EventHandler LaunchFormMapping)
        {
            try
            {
                NewDatatset = ds;
                LaunchForm = LaunchFormMapping;
                _positionType = positionType;
                this.Text = "Multileg Journal Import";
                DataTable dt = ds.Tables[0];

                DataColumn dc = new DataColumn(HEADCOL_PRIMARYKEY);
                dc.DataType = typeof(Int32);
                dt.Columns.Add(dc);

                if (!dt.Columns.Contains(HEADCOL_AccountID))
                {
                    DataColumn dcAccountID = new DataColumn(HEADCOL_AccountID);
                    dcAccountID.DataType = typeof(Int32);
                    dt.Columns.Add(dcAccountID);
                }
                if (!dt.Columns.Contains(HEADCOL_DATE))
                {
                    DataColumn dcPayoutDate = new DataColumn(HEADCOL_DATE);
                    dcPayoutDate.DataType = typeof(DateTime);
                    dt.Columns.Add(dcPayoutDate);
                }
                if (!dt.Columns.Contains(HEADCOL_AccountName))
                {
                    DataColumn dcAccountName = new DataColumn(HEADCOL_AccountName);
                    dcAccountName.DataType = typeof(DateTime);
                    dt.Columns.Add(dcAccountName);
                }
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    dt.Rows[i][HEADCOL_PRIMARYKEY] = i;
                    if (!_userSelectedDate.Equals(String.Empty))
                    {
                        dt.Rows[i][HEADCOL_DATE] = _userSelectedDate;
                    }
                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        dt.Rows[i][HEADCOL_AccountID] = _userSelectedAccountValue;
                        dt.Rows[i][HEADCOL_AccountName] = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    }
                    else
                    {
                        dt.Rows[i][HEADCOL_AccountID] = CachedDataManager.GetInstance.GetAccountID(dt.Rows[i][HEADCOL_AccountName].ToString());
                    }
                    SetNAVLockDateValidationError(dt.Rows[i]);
                }

                dt.Columns[HEADCOL_PRIMARYKEY].AutoIncrement = true;
                dt.PrimaryKey = new DataColumn[1] { dt.Columns[HEADCOL_PRIMARYKEY] };

                grdImportData.DataSource = null;
                grdImportData.DataSource = ds;
                grdImportData.DisplayLayout.Override.CellClickAction = CellClickAction.Edit;
                subAccount.SortStyle = ValueListSortStyle.Ascending;

                UltraGridBand band = this.grdImportData.DisplayLayout.Bands[0];
                UltraGridColumn colValidate = band.Columns[PROPERTY_Validated];
                colValidate.Header.Caption = "Validation Status";
                colValidate.AllowGroupBy = DefaultableBoolean.True;

                UltraGridColumn colEnteryID = band.Columns[HEADCOL_EntryID];
                colEnteryID.Header.Caption = "Entry ID";
                colEnteryID.AllowGroupBy = DefaultableBoolean.True;

                grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                grdImportData.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                grdImportData.DisplayLayout.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;


                band.Columns[HEADCOL_AccountID].Hidden = true;
                band.Columns[HEADCOL_PRIMARYKEY].Hidden = true;
                band.Columns[HEADCOL_AccountName].Header.Caption = "Account Name";
                if (band.Columns.Exists(HEADCOL_CurrencyName))
                    band.Columns[HEADCOL_CurrencyName].Header.Caption = "Currency Name";
                if (band.Columns.Exists(HEADCOL_FXRate))
                    band.Columns[HEADCOL_FXRate].Header.Caption = "FX Rate";
                if (band.Columns.Exists(HEADCOL_CurrencyID))
                    band.Columns[HEADCOL_CurrencyID].Hidden = true;
                if (band.Columns.Exists(HEADCOL_IsNonTradingTransaction))
                {
                    band.Columns[HEADCOL_IsNonTradingTransaction].Hidden = true;
                    band.Columns[HEADCOL_IsNonTradingTransaction].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                if (band.Columns.Exists(HEADCOL_Symbol))
                {
                    band.Columns[HEADCOL_Symbol].Hidden = true;
                }
                if (band.Columns.Exists(HEADCOL_checkBox))
                {
                    grdImportData.CreationFilter = headerCheckBox;
                }
                foreach (var col in band.Columns)
                {
                    col.CellActivation = Activation.NoEdit;
                }
                MultiLegGridSetting(this.grdImportData.DisplayLayout.Bands[0]);
                MergeCellAndRowSetting(this.grdImportData);
                band.SortedColumns.Add(PROPERTY_Validated, false, true);
                band.SortedColumns.Add(HEADCOL_EntryID, false, true);
                grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
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

        private static void MergeCellAndRowSetting(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CurrencyName].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CurrencyID].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_checkBox].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_checkBox].MergedCellEvaluator = new MyMergedCellEvaluator();
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_AccountName].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_AccountID].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_DATE].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_Description].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_EntryID].MergedCellStyle = MergedCellStyle.Always;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_Symbol].MergedCellStyle = MergedCellStyle.Always;

                foreach (UltraGridRow ultraGridRow in grid.Rows)
                {
                    AccountSide entryAcSide = (AccountSide)Enum.Parse(typeof(AccountSide), ultraGridRow.Cells[HEADCOL_AccountSide].Text.ToString());
                    if (entryAcSide == AccountSide.CR)
                    {
                        foreach (UltraGridCell cell in ultraGridRow.Cells)
                        {
                            if (!(cell.Column.Key.Equals(HEADCOL_checkBox) ||
                                cell.Column.Key.Equals(HEADCOL_Symbol) ||
                                cell.Column.Key.Equals(HEADCOL_AccountName) ||
                                cell.Column.Key.Equals(HEADCOL_AccountID) ||
                                cell.Column.Key.Equals(HEADCOL_DATE) ||
                                cell.Column.Key.Equals(HEADCOL_EntryID) ||
                                cell.Column.Key.Equals(HEADCOL_CurrencyName) ||
                                cell.Column.Key.Equals(HEADCOL_CurrencyID) ||
                                cell.Column.Key.Equals(PROPERTY_Validated) ||
                                cell.Column.Key.Equals(HEADCOL_PRIMARYKEY) ||
                                cell.Column.Key.Equals(HEADCOL_Description)))
                            {
                                cell.Appearance.BackColor = Color.FromArgb(134, 134, 134);
                                cell.SelectedAppearance.BackColor = Color.FromArgb(134, 134, 134);
                                cell.ActiveAppearance.BackColor = Color.FromArgb(134, 134, 134);

                                cell.Appearance.ForeColor = Color.Black;
                                cell.SelectedAppearance.ForeColor = Color.Black;
                                cell.ActiveAppearance.ForeColor = Color.Black;
                            }
                        }
                    }
                }

                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                    appearance.BackColor = System.Drawing.Color.Black;
                    appearance.ForeColor = System.Drawing.Color.White;
                    appearance.BorderColor = System.Drawing.Color.DimGray;
                    appearance.BorderColor2 = System.Drawing.Color.DimGray;

                    appearance.TextVAlignAsString = "Middle";
                    grid.DisplayLayout.Bands[0].Override.RowAppearance = appearance;
                }
                else
                {
                    Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                    appearance.BorderColor = Color.FromArgb(33, 44, 57);
                    appearance.BorderColor2 = Color.FromArgb(33, 44, 57);

                    appearance.TextVAlignAsString = "Middle";
                    grid.DisplayLayout.Bands[0].Override.RowAppearance = appearance;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static void MultiLegGridSetting(UltraGridBand band)
        {
            try
            {
                foreach (ColumnFilter filter in band.ColumnFilters)
                {
                    if (filter.Column.Hidden)
                    {
                        filter.ClearFilterConditions();
                    }
                }

                #region header setting

                FontData fontData = band.Override.HeaderAppearance.FontData;
                fontData.Bold = DefaultableBoolean.False;
                fontData.SizeInPoints = 8;
                fontData.Name = "Tahoma";
                band.Override.HeaderAppearance.TextHAlign = HAlign.Center;
                band.Override.HeaderAppearance.TextVAlign = VAlign.Middle;
                band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
                band.Override.HeaderClickAction = HeaderClickAction.SortSingle;

                Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                appearance2.BackColor = System.Drawing.Color.Silver;
                appearance2.BackColor2 = System.Drawing.Color.DimGray;
                appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                band.Layout.GroupByBox.Appearance = appearance2;
                #endregion

                #region Row Setting

                Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                if (!CustomThemeHelper.ApplyTheme)
                {
                    appearance.BackColor = System.Drawing.Color.Black;
                    appearance.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    appearance.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                }
                appearance.BorderColor2 = System.Drawing.Color.DimGray;
                appearance.TextHAlignAsString = "Middle";
                appearance.TextVAlignAsString = "Middle";
                band.Override.RowAppearance = appearance;

                band.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                #endregion

                UpdateColumnTextAlignment(band.Columns);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void grdImportData_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_positionType.Equals(_importTypeMultilegJournalImport))
                {
                    UltraGrid grid = (UltraGrid)sender;

                    // Was the click on a check indicator
                    //UIElement element = grid.DisplayLayout.UIElement.LastElementEntered;
                    UIElement element = grid.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                    CheckIndicatorUIElement checkIndicatorUIElement = element as CheckIndicatorUIElement;
                    if (null == checkIndicatorUIElement)
                        checkIndicatorUIElement = element.GetAncestor(typeof(CheckIndicatorUIElement)) as CheckIndicatorUIElement;

                    if (null == checkIndicatorUIElement)
                        return;

                    // Get the cell
                    UltraGridCell cell = checkIndicatorUIElement.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (null == cell)
                        return;

                    if (cell.Column.Key == HEADCOL_checkBox)
                    {
                        UltraGridCell[] mergedCells = cell.GetMergedCells();
                        if (null != mergedCells && mergedCells.Length > 0)
                        {
                            bool newValue = !((bool)mergedCells[0].Value);
                            foreach (UltraGridCell mergedcell in mergedCells)
                            {
                                mergedcell.Value = newValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal static void UpdateColumnTextAlignment(ColumnsCollection columnsCollection)
        {
            try
            {
                columnsCollection.Cast<UltraGridColumn>().ToList().ForEach(col =>
                {
                    switch (col.DataType.Name)
                    {
                        case "String":
                        case "DateTime":
                        case "Boolean":
                            col.CellAppearance.TextHAlign = HAlign.Right;
                            break;

                        case "Int32":
                        case "Int64":
                        case "Double":
                        case "Single":
                        case "Decimal":
                            col.CellAppearance.TextHAlign = HAlign.Right;
                            if (string.IsNullOrWhiteSpace(col.Format))
                                col.Format = "#,###,###,###,##0.##############";
                            break;

                        default:
                            col.CellAppearance.TextHAlign = HAlign.Right;
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        ListEventAargs l = new ListEventAargs();
        public void BindImportedDoubleEntryCash(DataSet ds, string _userSelectedDate, int _userSelectedAccountValue, string positionType, EventHandler LaunchFormMapping)
        {
            try
            {
                LaunchForm = LaunchFormMapping;
                _positionType = positionType;
                this.Text = "Double Entry Cash Import";
                DataTable dt = ds.Tables[0];

                DataColumn dc = new DataColumn("PRIMARY_KEY");
                dc.DataType = typeof(Int32);
                dt.Columns.Add(dc);
                // add AccountID if not exists
                if (!dt.Columns.Contains("FundID"))
                {
                    DataColumn dcAccountID = new DataColumn("FundID");
                    dcAccountID.DataType = typeof(Int32);
                    dt.Columns.Add(dcAccountID);
                }
                if (!dt.Columns.Contains("Date"))
                {
                    DataColumn dcPayoutDate = new DataColumn("Date");
                    dcPayoutDate.DataType = typeof(DateTime);
                    dt.Columns.Add(dcPayoutDate);
                }
                if (!dt.Columns.Contains("AccountName"))
                {
                    DataColumn dcAccountName = new DataColumn("AccountName");
                    dcAccountName.DataType = typeof(DateTime);
                    dt.Columns.Add(dcAccountName);
                }
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    dt.Rows[i]["PRIMARY_KEY"] = i;
                    if (!_userSelectedDate.Equals(String.Empty))
                    {
                        dt.Rows[i]["Date"] = _userSelectedDate;
                    }
                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        dt.Rows[i]["FundID"] = _userSelectedAccountValue;
                        dt.Rows[i]["AccountName"] = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    }
                    else
                    {
                        dt.Rows[i]["FundID"] = CachedDataManager.GetInstance.GetAccountID(dt.Rows[i]["AccountName"].ToString());
                    }
                }

                dt.Columns["PRIMARY_KEY"].AutoIncrement = true;
                dt.PrimaryKey = new DataColumn[1] { dt.Columns["PRIMARY_KEY"] };

                grdImportData.DataSource = null;
                grdImportData.DataSource = ds;
                grdImportData.DisplayLayout.Override.CellClickAction = CellClickAction.Edit;
                subAccount.SortStyle = ValueListSortStyle.Ascending;

                UltraGridBand band = this.grdImportData.DisplayLayout.Bands[0];
                UltraGridColumn colValidate = band.Columns[PROPERTY_Validated];
                colValidate.Header.Caption = "Validation Status";
                colValidate.AllowGroupBy = DefaultableBoolean.True;

                grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                band.SortedColumns.Add(PROPERTY_Validated, false, true);
                grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";

                band.Columns["FundID"].Hidden = true;
                band.Columns["PRIMARY_KEY"].Hidden = true;
                band.Columns["AccountName"].CellActivation = Activation.NoEdit;
                band.Columns["AccountName"].Header.Caption = "Account Name";
                if (band.Columns.Exists("CurrencyName"))
                    band.Columns["CurrencyName"].Header.Caption = "Currency Name";

                if (band.Columns.Exists("CRCurrencyName"))
                    band.Columns["CRCurrencyName"].Header.Caption = "Cr Currency Name";

                if (band.Columns.Exists("DRCurrencyName"))
                    band.Columns["DRCurrencyName"].Header.Caption = "Dr Currency Name";

                if (band.Columns.Exists("CRFXRate"))
                    band.Columns["CRFXRate"].Header.Caption = "Cr Fx Rate";

                if (band.Columns.Exists("DRFXRate"))
                    band.Columns["DRFXRate"].Header.Caption = "Dr Fx Rate";

                band.Columns["Date"].CellActivation = Activation.NoEdit;
                if (band.Columns.Exists("CurrencyID"))
                    band.Columns["CurrencyID"].Hidden = true;
                band.Columns["JournalEntries"].Header.Caption = "Journal Entries";
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
        internal void BindImportVolatility(List<VolatilityImport> importvolatilityValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importvolatilityValues;
            this.Text = "Import Volatility Values";
        }
        internal void BindImportVWAP(List<VWAPImport> importVWAPValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importVWAPValues;
            this.Text = "Import VWAP Values";
        }
        internal void BindImportCollateral(List<CollateralImport> importCollateralValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importCollateralValues;
            this.Text = "Import Collateral Values";
        }
        internal void BindImportDividendYield(List<DividendYieldImport> importDividendYieldValues, string positionType)
        {
            _positionType = positionType;
            grdImportData.DataSource = null;
            grdImportData.DataSource = importDividendYieldValues;
            this.Text = "Import Dividend Yield Values";
        }

        List<string> _visibleColumns = new List<string>();
        private List<string> GetVisibleColumnsListForPositions(bool isDisplaySwapColumn)
        {
            if (_visibleColumns.Count > 0)
            {
                _visibleColumns.Clear();
            }
            _visibleColumns.Add(PROPERTY_SYMBOL);
            _visibleColumns.Add(PROPERTY_SIDE);
            _visibleColumns.Add(PROPERTY_QUANTITY);
            _visibleColumns.Add(PROPERTY_AVGPRICE);
            _visibleColumns.Add(PROPERTY_AUECDATE);
            _visibleColumns.Add(PROPERTY_SETTLEMENTDATE);
            _visibleColumns.Add(PROPERTY_FUND);
            _visibleColumns.Add(PROPERTY_ASSETTYPE);
            _visibleColumns.Add(PROPERTY_PBSymbol);
            _visibleColumns.Add(PROPERTY_Validated);
            _visibleColumns.Add(PROPERTY_PBAssetType);
            _visibleColumns.Add(PROPERTY_ExecutingBroker);
            _visibleColumns.Add(OrderFields.PROPERTY_COMMISSION);
            _visibleColumns.Add(OrderFields.PROPERTY_SOFTCOMMISSION);
            _visibleColumns.Add(PROPERTY_FXRate);
            _visibleColumns.Add(PROPERTY_FXConversionMethodOperator);
            _visibleColumns.Add(PROPERTY_PROCESSDATE);
            _visibleColumns.Add(PROPERTY_ORIGINALPURCHASEDATE);
            _visibleColumns.Add(PROPERTY_OTHERBROKERFEE);
            _visibleColumns.Add(OrderFields.PROPERTY_CLEARINGBROKERFEE);
            _visibleColumns.Add(OrderFields.PROPERTY_STAMPDUTY);
            _visibleColumns.Add(OrderFields.PROPERTY_TRANSACTIONLEVY);
            _visibleColumns.Add(OrderFields.PROPERTY_CLEARINGFEE);
            _visibleColumns.Add(OrderFields.PROPERTY_TAXONCOMMISSIONS);
            _visibleColumns.Add(OrderFields.PROPERTY_MISCFEES);
            _visibleColumns.Add(PROPERTY_COMMISSIONSOURCE);
            _visibleColumns.Add(PROPERTY_SOFTCOMMISSIONSOURCE);
            _visibleColumns.Add(OrderFields.PROPERTY_SECFEE);
            _visibleColumns.Add(OrderFields.PROPERTY_OCCFEE);
            _visibleColumns.Add(OrderFields.PROPERTY_ORFFEE);
            _visibleColumns.Add("LotId");
            _visibleColumns.Add("ExternalTransId");
            _visibleColumns.Add("TransactionType");
            if (isDisplaySwapColumn)
            {
                _visibleColumns.Add(HEADCOLSWAP_BenchMarkRate);
                _visibleColumns.Add(HEADCOLSWAP_DayCount);
                _visibleColumns.Add(HEADCOLSWAP_Differential);
                _visibleColumns.Add(HEADCOLSWAP_FirstResetDate);
                _visibleColumns.Add(HEADCOLSWAP_OrigTransDate);
                _visibleColumns.Add(HEADCOLSWAP_ResetFrequency);
                _visibleColumns.Add(HEADCOLSWAP_SwapDescription);

            }
            //Added column for isApproved satus of security
            _visibleColumns.Add(PROPERTY_SEC_APPROVAL_STATUS);
            _visibleColumns.Add("checkBox");
            _visibleColumns.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYNAME);
            _visibleColumns.Add(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT);
            return _visibleColumns;
        }

        public void RefreshGridGroup(object sender, EventArgs<string> e)
        {
            string symbol = e.Value;
            grdImportData.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
            toolStripStatusLabel1.Text = "Last Updated Symbol : " + symbol;
        }

        private ValueList _assets = new ValueList();
        private ValueList _underLying = new ValueList();
        private ValueList _exchanges = new ValueList();
        private ValueList _currencies = new ValueList();
        private ValueList _optionType = new ValueList();
        private ValueList _frequency = new ValueList();
        private ValueList _securityType = new ValueList();
        private ValueList _accrualBasis = new ValueList();

        private void GetAllDefaults()
        {

            Dictionary<int, string> dictAssets = CachedDataManager.GetInstance.GetAllAssets();
            Dictionary<int, string> dictUnderlyings = CachedDataManager.GetInstance.GetAllUnderlyings();
            Dictionary<int, string> dictExchanges = CachedDataManager.GetInstance.GetAllExchanges();
            Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();

            _assets.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> item in dictAssets)
            {
                _assets.ValueListItems.Add(item.Key, item.Value);
            }
            _underLying.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> item in dictUnderlyings)
            {
                _underLying.ValueListItems.Add(item.Key, item.Value);
            }
            _exchanges.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> item in dictExchanges)
            {
                _exchanges.ValueListItems.Add(item.Key, item.Value);
            }
            _currencies.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> item in dictCurrencies)
            {
                _currencies.ValueListItems.Add(item.Key, item.Value);
            }

            string[] members = Enum.GetNames(typeof(BusinessObjects.AppConstants.OptionType));
            foreach (string member in members)
            {
                string name = member;
                int i = Convert.ToInt32(Enum.Parse(typeof(BusinessObjects.AppConstants.OptionType), name));
                _optionType.ValueListItems.Add(i, name);
            }
            List<EnumerationValue> securityType = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(SecurityType));
            _securityType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (EnumerationValue value in securityType)
            {
                _securityType.ValueListItems.Add(value.Value, value.DisplayText);
            }
            List<EnumerationValue> accrualBasis = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(AccrualBasis));
            foreach (EnumerationValue value in accrualBasis)
            {
                _accrualBasis.ValueListItems.Add(value.Value, value.DisplayText);
            }

            List<EnumerationValue> frequency = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(CouponFrequency));
            foreach (EnumerationValue value in frequency)
            {
                _frequency.ValueListItems.Add(value.Value, value.DisplayText);
            }
        }


        private void SetGridColumns(UltraGridBand gridBand)
        {
            int visiblePosition = 1;

            UltraGridColumn colAsset = gridBand.Columns[OrderFields.PROPERTY_ASSET_ID];
            colAsset.Header.VisiblePosition = visiblePosition++;
            colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colAsset.CellActivation = Activation.NoEdit;
            colAsset.Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
            colAsset.ValueList = _assets;
            colAsset.Header.Column.Width = 70;
            colAsset.AutoSizeMode = ColumnAutoSizeMode.Default;

            UltraGridColumn colUnderLying = gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID];
            colUnderLying.Header.VisiblePosition = visiblePosition++;
            colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colUnderLying.CellActivation = Activation.NoEdit;
            colUnderLying.Header.Caption = OrderFields.CAPTION_UNDERLYING_NAME;
            colUnderLying.ValueList = _underLying;
            colUnderLying.Header.Column.Width = 70;

            UltraGridColumn ColExchnage = gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID];
            ColExchnage.Header.VisiblePosition = visiblePosition++;
            ColExchnage.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColExchnage.CellActivation = Activation.NoEdit;
            ColExchnage.Header.Caption = OrderFields.CAPTION_EXCHANGE;
            ColExchnage.ValueList = _exchanges;
            ColExchnage.Header.Column.Width = 70;

            UltraGridColumn ColCurrency = gridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
            ColCurrency.Header.VisiblePosition = visiblePosition++;
            ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColCurrency.CellActivation = Activation.NoEdit;
            ColCurrency.Header.Caption = "Currency";
            ColCurrency.ValueList = _currencies;
            ColCurrency.Header.Column.Width = 70;

            UltraGridColumn ColTickerSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()];
            ColTickerSymbol.Header.VisiblePosition = visiblePosition++;
            ColTickerSymbol.Header.Column.Width = 100;
            ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
            ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
            ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColTickerSymbol.NullText = String.Empty;

            UltraGridColumn ColLongName = gridBand.Columns[OrderFields.PROPERTY_LONGNAME];
            ColLongName.Width = 100;
            ColLongName.Header.VisiblePosition = visiblePosition++;
            ColLongName.Header.Column.Width = 150;
            ColLongName.Header.Caption = OrderFields.CAPTION_LONGNAME;
            ColLongName.CharacterCasing = CharacterCasing.Upper;
            ColLongName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColLongName.NullText = String.Empty;

            UltraGridColumn ColReutersSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()];
            ColReutersSymbol.Header.VisiblePosition = visiblePosition++;
            ColReutersSymbol.Header.Column.Width = 100;
            ColReutersSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
            ColReutersSymbol.CharacterCasing = CharacterCasing.Upper;
            ColReutersSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColReutersSymbol.NullText = String.Empty;

            UltraGridColumn ColBloombergSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()];
            ColBloombergSymbol.Header.VisiblePosition = visiblePosition++;
            ColBloombergSymbol.Header.Column.Width = 100;
            ColBloombergSymbol.Width = 70;
            ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
            ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColBloombergSymbol.NullText = String.Empty;
            ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;

            UltraGridColumn ColCusipSymbol = gridBand.Columns[OrderFields.PROPERTY_CUSIPSYMBOL];
            ColCusipSymbol.Header.VisiblePosition = visiblePosition++;
            ColCusipSymbol.Header.Column.Width = 100;
            ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
            ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColCusipSymbol.NullText = String.Empty;
            ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;

            UltraGridColumn ColISINSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()];
            ColISINSymbol.Header.VisiblePosition = visiblePosition++;
            ColISINSymbol.Header.Column.Width = 100;
            ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
            ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColISINSymbol.NullText = String.Empty;
            ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;

            UltraGridColumn ColSedolSymbol = gridBand.Columns[OrderFields.PROPERTY_SEDOLSYMBOL];
            ColSedolSymbol.Header.VisiblePosition = visiblePosition++;
            ColSedolSymbol.Header.Column.Width = 100;
            ColSedolSymbol.CharacterCasing = CharacterCasing.Upper;
            ColSedolSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColSedolSymbol.NullText = String.Empty;
            ColSedolSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;

            UltraGridColumn ColOSIOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()];
            ColOSIOptionSymbol.Header.VisiblePosition = visiblePosition++;
            ColOSIOptionSymbol.Header.Column.Width = 150;
            ColOSIOptionSymbol.CharacterCasing = CharacterCasing.Upper;
            ColOSIOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColOSIOptionSymbol.NullText = String.Empty;
            ColOSIOptionSymbol.Header.Caption = OrderFields.CAPTION_OSIOPTIONSYMBOL;

            UltraGridColumn ColIDCOOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()];
            ColIDCOOptionSymbol.Header.VisiblePosition = visiblePosition++;
            ColIDCOOptionSymbol.Header.Column.Width = 150;
            ColIDCOOptionSymbol.CharacterCasing = CharacterCasing.Upper;
            ColIDCOOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColIDCOOptionSymbol.NullText = String.Empty;
            ColIDCOOptionSymbol.Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;

            UltraGridColumn ColOPRAOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()];
            ColOPRAOptionSymbol.Header.VisiblePosition = visiblePosition++;
            ColOPRAOptionSymbol.Header.Column.Width = 150;
            ColOPRAOptionSymbol.CharacterCasing = CharacterCasing.Upper;
            ColOPRAOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColOPRAOptionSymbol.NullText = String.Empty;
            ColOPRAOptionSymbol.Header.Caption = OrderFields.CAPTION_OPRAOPTIONSYMBOL;

            UltraGridColumn ColMultiplier = gridBand.Columns["Multiplier"];
            ColMultiplier.Header.VisiblePosition = visiblePosition++;
            ColMultiplier.Header.Column.Width = 70;
            ColMultiplier.Header.Caption = "Multiplier";
            ColMultiplier.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
            ColMultiplier.NullText = null;

            UltraGridColumn ColIssueDate = gridBand.Columns["IssueDate"];
            ColIssueDate.Header.Column.Width = 100;
            ColIssueDate.Header.VisiblePosition = visiblePosition++;
            ColIssueDate.Header.Caption = "Issue Date";
            ColIssueDate.AutoSizeMode = ColumnAutoSizeMode.Default;

            UltraGridColumn ColFirstCouponDate = gridBand.Columns["FirstCouponDate"];
            ColFirstCouponDate.Header.VisiblePosition = visiblePosition++;
            ColFirstCouponDate.Header.Column.Width = 100;
            ColFirstCouponDate.Header.Caption = "FirstCouponDate";

            UltraGridColumn ColExpirationOrSettlementDate = gridBand.Columns["ExpirationDate"];
            ColExpirationOrSettlementDate.Header.VisiblePosition = visiblePosition++;
            ColExpirationOrSettlementDate.Header.Caption = OrderFields.CAPTION_EXPIRATIONDATE; //"Expiration Date";
            ColExpirationOrSettlementDate.Header.Column.Width = 100;
            ColExpirationOrSettlementDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColExpirationOrSettlementDate.NullText = "1/1/1800";
            ColExpirationOrSettlementDate.CellActivation = Activation.NoEdit;

            UltraGridColumn ColFixingDate = gridBand.Columns["FixingDate"];
            ColFixingDate.Header.VisiblePosition = 21;
            ColFixingDate.Header.Caption = "Fixing Date";
            ColFixingDate.Header.Column.Width = 100;
            ColFixingDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColFixingDate.NullText = "1/1/1800";

            UltraGridColumn ColIsNDF = gridBand.Columns["IsNDF"];
            ColIsNDF.Header.VisiblePosition = 22;
            ColIsNDF.Header.Caption = "Is NDF";
            ColIsNDF.Header.Column.Width = 70;

            UltraGridColumn ColUnderLyingSymbol = gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
            ColUnderLyingSymbol.Header.VisiblePosition = visiblePosition++;
            ColUnderLyingSymbol.Header.Column.Width = 100;
            ColUnderLyingSymbol.Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
            ColUnderLyingSymbol.CharacterCasing = CharacterCasing.Upper;
            ColUnderLyingSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColUnderLyingSymbol.NullText = String.Empty;

            UltraGridColumn ColOptionType = gridBand.Columns[OrderFields.PROPERTY_PUT_CALL];
            ColOptionType.Header.VisiblePosition = visiblePosition++;
            ColOptionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColOptionType.CellActivation = Activation.NoEdit;
            ColOptionType.ValueList = _optionType;
            ColOptionType.Header.Column.Width = 70;
            ColOptionType.Header.Caption = OrderFields.CAPTION_PUT_CALL;
            ColOptionType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColOptionType.NullText = String.Empty;

            UltraGridColumn ColStrikePirce = gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE];
            ColStrikePirce.Header.VisiblePosition = visiblePosition++;
            ColStrikePirce.Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
            ColStrikePirce.Header.Column.Width = 70;
            ColStrikePirce.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
            ColStrikePirce.NullText = null;

            UltraGridColumn ColDelta = gridBand.Columns["Delta"];
            ColDelta.NullText = null;
            ColDelta.Header.Column.Width = 70;
            ColDelta.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
            ColDelta.Header.VisiblePosition = visiblePosition++;
            ColDelta.Header.Caption = "Leveraged Factor";

            UltraGridColumn ColBondTypeID = gridBand.Columns["BondTypeID"];
            ColBondTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColBondTypeID.ValueList = _securityType;
            ColBondTypeID.CellActivation = Activation.NoEdit;
            ColBondTypeID.Header.Column.Width = 100;
            ColBondTypeID.Header.Caption = "Bond Type";

            UltraGridColumn ColAccrualBasis = gridBand.Columns["AccrualBasisID"];
            ColAccrualBasis.Header.Column.Width = 100;
            ColAccrualBasis.Header.VisiblePosition = visiblePosition++;
            ColAccrualBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColAccrualBasis.Header.Caption = OrderFields.CAPTION_ACCRUALBASIS;
            ColAccrualBasis.ValueList = _accrualBasis;
            ColAccrualBasis.CellActivation = Activation.NoEdit;
            ColAccrualBasis.Header.Column.Width = 100;

            UltraGridColumn ColFrequency = gridBand.Columns["CouponFrequencyID"];
            ColFrequency.Header.VisiblePosition = visiblePosition++;
            ColFrequency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColFrequency.Header.Caption = "Coupon Frequency";
            ColFrequency.ValueList = _frequency;
            ColFrequency.CellActivation = Activation.NoEdit;
            ColFrequency.Header.Column.Width = 100;

            UltraGridColumn ColCoupon = gridBand.Columns["Coupon"];
            ColCoupon.Header.VisiblePosition = visiblePosition++;
            ColCoupon.Header.Caption = "Coupon (%)";
            ColCoupon.Header.VisiblePosition = visiblePosition++;
            ColFrequency.Header.Column.Width = 100;

            UltraGridColumn ColUDAAssetClassID = gridBand.Columns["UDAAssetClassID"];
            ColUDAAssetClassID.Header.VisiblePosition = visiblePosition++;
            ColUDAAssetClassID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColUDAAssetClassID.Header.Caption = "UDA Asset";
            ColUDAAssetClassID.ValueList = SecMasterHelper.getInstance().UDAAssets;
            ColUDAAssetClassID.CellActivation = Activation.NoEdit;
            ColUDAAssetClassID.Header.Column.Width = 100;

            UltraGridColumn ColUDASectorID = gridBand.Columns["UDASectorID"];
            ColUDASectorID.Header.VisiblePosition = visiblePosition++;
            ColUDASectorID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColUDASectorID.Header.Caption = "UDA Sector";
            ColUDASectorID.ValueList = SecMasterHelper.getInstance().UDASectors;
            ColUDASectorID.CellActivation = Activation.NoEdit;
            ColUDASectorID.Header.Column.Width = 100;

            UltraGridColumn ColUDASubSectorID = gridBand.Columns["UDASubSectorID"];
            ColUDASubSectorID.Header.VisiblePosition = visiblePosition++;
            ColUDASubSectorID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColUDASubSectorID.Header.Caption = "UDA SubSector";
            ColUDASubSectorID.CellActivation = Activation.NoEdit;
            ColUDASubSectorID.ValueList = SecMasterHelper.getInstance().UDASubSectors;
            ColUDASubSectorID.Header.Column.Width = 100;

            UltraGridColumn ColUDASecurityTypeID = gridBand.Columns["UDASecurityTypeID"];
            ColUDASecurityTypeID.Header.VisiblePosition = visiblePosition++;
            ColUDASecurityTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColUDASecurityTypeID.Header.Caption = "UDA Security";
            ColUDASecurityTypeID.CellActivation = Activation.NoEdit;
            ColUDASecurityTypeID.ValueList = SecMasterHelper.getInstance().UDASecurityTypes;
            ColUDASecurityTypeID.Header.Column.Width = 100;

            UltraGridColumn ColUDACountryID = gridBand.Columns["UDACountryID"];
            ColUDACountryID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ColUDACountryID.Header.Caption = "UDA Country";
            ColUDACountryID.Header.VisiblePosition = visiblePosition++;
            ColUDACountryID.ValueList = SecMasterHelper.getInstance().UDACountries;
            ColUDACountryID.CellActivation = Activation.NoEdit;
            ColUDACountryID.Header.Column.Width = 100;

            UltraGridColumn ColStrikePriceMultiplier = gridBand.Columns["StrikePriceMultiplier"];
            ColStrikePriceMultiplier.Header.Caption = "Strike Price Multiplier";
            ColStrikePriceMultiplier.Header.Column.Width = 70;
            ColStrikePriceMultiplier.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
            ColStrikePriceMultiplier.NullText = null;

            UltraGridColumn ColEsignalOptionRoot = gridBand.Columns["EsignalOptionRoot"];
            ColEsignalOptionRoot.Header.Caption = "Esignal Option Root";
            ColEsignalOptionRoot.Header.VisiblePosition = visiblePosition++;
            ColEsignalOptionRoot.Header.Column.Width = 150;
            ColEsignalOptionRoot.CharacterCasing = CharacterCasing.Upper;
            ColEsignalOptionRoot.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColEsignalOptionRoot.NullText = String.Empty;

            UltraGridColumn ColBloombergOptionRoot = gridBand.Columns["BloombergOptionRoot"];
            ColBloombergOptionRoot.Header.Caption = "Bloomberg Option Root";
            ColBloombergOptionRoot.Header.Column.Width = 150;
            ColBloombergOptionRoot.Header.VisiblePosition = visiblePosition++;
            ColBloombergOptionRoot.CharacterCasing = CharacterCasing.Upper;
            ColBloombergOptionRoot.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ColBloombergOptionRoot.NullText = String.Empty;

            UltraGridColumn ColLeadCurrency = gridBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID];
            UltraGridColumn ColVsCurrency = gridBand.Columns[OrderFields.PROPERTY_VSCURRENCYID];
            ColLeadCurrency.ValueList = _currencies;
            ColLeadCurrency.CellActivation = Activation.NoEdit;
            ColLeadCurrency.Header.Caption = "LeadCurrency";
            ColLeadCurrency.Header.VisiblePosition = visiblePosition++;
            ColLeadCurrency.Header.Column.Width = 70;

            ColVsCurrency.ValueList = _currencies;
            ColVsCurrency.CellActivation = Activation.NoEdit;
            ColVsCurrency.Header.VisiblePosition = visiblePosition++;
            ColVsCurrency.Header.Caption = "VsCurrency";
            ColVsCurrency.Header.Column.Width = 70;

            UltraGridColumn ColAUECID = gridBand.Columns["AUECID"];
            ColAUECID.Header.VisiblePosition = visiblePosition++;
            ColAUECID.Header.Caption = "AUECID";
            ColAUECID.Header.Column.Width = 70;

            UltraGridColumn ColIsZero = gridBand.Columns["IsZero"];
            ColIsZero.Header.VisiblePosition = visiblePosition++;
            ColIsZero.Hidden = false;
            ColIsZero.Header.Caption = "IsZero";
            ColIsZero.CellActivation = Activation.NoEdit;

            UltraGridColumn ColMaturityDate = gridBand.Columns["MaturityDate"];
            ColMaturityDate.Header.VisiblePosition = visiblePosition++;
            ColMaturityDate.Header.Caption = "Maturity Date";
            ColMaturityDate.CellActivation = Activation.NoEdit;
            ColMaturityDate.Header.Column.Width = 100;

            UltraGridColumn ColDaysToSettlement = gridBand.Columns["DaysToSettlement"];
            ColDaysToSettlement.Header.VisiblePosition = visiblePosition++;
            ColDaysToSettlement.Header.Caption = "DaysToSettlement";
            ColDaysToSettlement.CellActivation = Activation.NoEdit;
            ColDaysToSettlement.Header.Column.Width = 70;

            UltraGridColumn colValidate = gridBand.Columns[ApplicationConstants.CONST_VALIDATION_STATUS];
            colValidate.Header.Caption = "Validation Status";
            colValidate.Header.VisiblePosition = visiblePosition++;
            colValidate.AllowGroupBy = DefaultableBoolean.True;
            colValidate.Header.Column.Width = 70;

            UltraGridColumn ColSymbolExistsInSM = gridBand.Columns["SymbolExistsInSM"];
            ColSymbolExistsInSM.Hidden = true;

            UltraGridColumn ColSymbol_PK = gridBand.Columns["Symbol_PK"];
            ColSymbol_PK.Header.VisiblePosition = visiblePosition++;
            ColSymbol_PK.Header.Caption = "ColSymbol_PK";
            ColSymbol_PK.CellActivation = Activation.NoEdit;

            UltraGridColumn ColValidationStatus = gridBand.Columns["ValidationStatus"];
            ColValidationStatus.Header.VisiblePosition = visiblePosition++;
            ColValidationStatus.Header.Caption = "Validation Status";
            ColValidationStatus.CellActivation = Activation.NoEdit;
            ColValidationStatus.Header.Column.Width = 70;

            SetGridUDAColumns(gridBand);

        }

        /// <summary>
        /// set dynamic UDA columns properties
        /// </summary>
        /// <param name="gridBand">The gridband</param>
        /// <param name="dynamicUDAcache">The dynamic UDAs cache</param>
        private void SetGridUDAColumns(UltraGridBand gridBand)
        {
            try
            {
                //UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];
                if (gridBand != null)
                {
                    if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                    {
                        foreach (string uda in _dynamicUDACache.Keys)
                        {
                            if (gridBand.Columns.Exists(uda))
                            {
                                UltraGridColumn gridUDAcolumn = gridBand.Columns[uda];
                                gridUDAcolumn.Header.Caption = _dynamicUDACache[uda].HeaderCaption.ToString();
                                gridUDAcolumn.Header.Column.Width = 100;

                                if (_dynamicUDACache[uda].MasterValues != null && _dynamicUDACache[uda].MasterValues.Count > 0)
                                {
                                    gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                                }
                                else
                                {
                                    gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                                    gridUDAcolumn.CellMultiLine = DefaultableBoolean.False;
                                }
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
        /// <summary>
        /// Add dynamic UDAs to grdData
        /// </summary>        
        private void SetDynamicUDA(UltraGridBand gridBand)
        {
            try
            {
                if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                {
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (!gridBand.Columns.Exists(uda))
                        {
                            UltraGridColumn gridUDAcolumn = gridBand.Columns.Add(uda);
                            gridUDAcolumn.Header.Caption = _dynamicUDACache[uda].HeaderCaption.ToString();
                            gridUDAcolumn.Header.Column.Width = 100;

                            if (_dynamicUDACache[uda].MasterValues != null && _dynamicUDACache[uda].MasterValues.Count > 0)
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            }
                            else
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;
                                gridUDAcolumn.CellMultiLine = DefaultableBoolean.False;
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


        private void ImportPositionsDisplayForm_Load(object sender, EventArgs e)
        {
            try
            {
                btnMapping.Visible = false;
                btnRefresh.Visible = false;
                grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                UltraGridBand band = this.grdImportData.DisplayLayout.Bands[0];
                if (!band.Columns.Exists("checkBox"))
                {
                    band.Columns.Add("checkBox");
                }
                UltraGridColumn colcheckBox = band.Columns["checkBox"];
                colcheckBox.Header.VisiblePosition = 0;
                colcheckBox.Hidden = false;

                //Setting columns based on import type.
                switch (_positionType)
                {
                    #region _importTypeCash
                    case _importTypeCash:

                        UltraGridColumn colAccount = band.Columns[HEADCOLCASH_FUND];
                        colAccount.Header.Caption = "Account";
                        colAccount.Header.VisiblePosition = 1;

                        UltraGridColumn colDate = band.Columns[HEADCOLCASH_Date];
                        colDate.Header.Caption = "Date";
                        colDate.Header.VisiblePosition = 2;

                        UltraGridColumn colBaseCurrency = band.Columns[HEADCOLCASH_BaseCurrency];
                        colBaseCurrency.Header.Caption = "Currency(Base)";
                        colBaseCurrency.Header.VisiblePosition = 3;
                        colBaseCurrency.Width = 100;

                        UltraGridColumn colCashValueBase = band.Columns[HEADCOLCASH_CashValueBase];
                        colCashValueBase.Header.Caption = "Cash Value(Base)";
                        colCashValueBase.Header.VisiblePosition = 4;
                        colCashValueBase.Width = 100;

                        UltraGridColumn colLocalCurrency = band.Columns[HEADCOLCASH_LocalCurrency];
                        colLocalCurrency.Header.Caption = "Currency(Local)";
                        colLocalCurrency.Header.VisiblePosition = 5;
                        colLocalCurrency.Width = 100;

                        UltraGridColumn colCashValueLocal = band.Columns[HEADCOLCASH_CashValueLocal];
                        colCashValueLocal.Header.Caption = "Cash Value(Local)";
                        colCashValueLocal.Header.VisiblePosition = 6;
                        colCashValueLocal.Width = 100;

                        UltraGridColumn colPositionType = band.Columns[HEADCOLCASH_PositionType];
                        colPositionType.Header.Caption = "Position Type";
                        colPositionType.Header.VisiblePosition = 7;

                        UltraGridColumn colValidate = band.Columns[PROPERTY_Validated];
                        colValidate.Header.Caption = "Validation Status";
                        colValidate.Header.VisiblePosition = 8;
                        colValidate.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    //Created By: Pooja Porwal
                    //Date:12 Feb 2015
                    //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5820

                    #region _importTypeSettlementDateCash
                    case _importTypeSettlementDateCash:

                        UltraGridColumn colSettlementDateAccount = band.Columns[HEADCOLSETTLEMENTCASH_FUND];
                        colSettlementDateAccount.Header.Caption = "Account";
                        colSettlementDateAccount.Header.VisiblePosition = 1;

                        UltraGridColumn colSettlementCashDate = band.Columns[HEADCOLSETTLEMENTCASH_Date];
                        colSettlementCashDate.Header.Caption = "Settlement Date";
                        colSettlementCashDate.Header.VisiblePosition = 2;

                        UltraGridColumn colSettlementDateBaseCurrency = band.Columns[HEADCOLSETTLEMENTCASH_BaseCurrency];
                        colSettlementDateBaseCurrency.Header.Caption = "Currency (Base)";
                        colSettlementDateBaseCurrency.Header.VisiblePosition = 3;
                        colSettlementDateBaseCurrency.Width = 100;

                        UltraGridColumn colSettlementDateCashValueBase = band.Columns[HEADCOLSETTLEMENTCASH_CashValueBase];
                        colSettlementDateCashValueBase.Header.Caption = "Cash Value (Base)";
                        colSettlementDateCashValueBase.Header.VisiblePosition = 4;
                        colSettlementDateCashValueBase.Width = 100;

                        UltraGridColumn colSettlementDateLocalCurrency = band.Columns[HEADCOLSETTLEMENTCASH_LocalCurrency];
                        colSettlementDateLocalCurrency.Header.Caption = "Currency (Local)";
                        colSettlementDateLocalCurrency.Header.VisiblePosition = 5;
                        colSettlementDateLocalCurrency.Width = 100;

                        UltraGridColumn colSettlementDateCashValueLocal = band.Columns[HEADCOLSETTLEMENTCASH_CashValueLocal];
                        colSettlementDateCashValueLocal.Header.Caption = "Cash Value (Local)";
                        colSettlementDateCashValueLocal.Header.VisiblePosition = 6;
                        colSettlementDateCashValueLocal.Width = 100;

                        UltraGridColumn colSettlementDatePositionType = band.Columns[HEADCOLSETTLEMENTCASH_PositionType];
                        colSettlementDatePositionType.Header.Caption = "Position Type";
                        colSettlementDatePositionType.Header.VisiblePosition = 7;

                        UltraGridColumn colSettlementDateValidate = band.Columns[PROPERTY_Validated];
                        colSettlementDateValidate.Header.Caption = "Validation Status";
                        colSettlementDateValidate.Header.VisiblePosition = 8;
                        colSettlementDateValidate.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeTransaction / importTypeNetPosition
                    case _importTypeTransaction:
                    case _importTypeNetPosition:
                        foreach (UltraGridColumn col in grdImportData.DisplayLayout.Bands[0].Columns)
                        {
                            col.Hidden = true;
                            if (_visibleColumns.Contains(col.Key))
                            {
                                col.Hidden = false;
                            }
                        }

                        UltraGridColumn colSymbol = band.Columns[PROPERTY_SYMBOL];
                        colSymbol.Header.Caption = "Application Symbol";
                        colSymbol.Header.VisiblePosition = 1;

                        UltraGridColumn colPBSymbol = band.Columns[PROPERTY_PBSymbol];
                        colPBSymbol.Header.Caption = "PB Symbol";
                        colPBSymbol.Header.VisiblePosition = 2;
                        colPBSymbol.Width = 120;

                        UltraGridColumn colSide = band.Columns[PROPERTY_SIDE];
                        colSide.Header.Caption = "Side";
                        colSide.Header.VisiblePosition = 3;

                        UltraGridColumn colTransactionType = band.Columns[PROPERTY_TRANSACTIONTYPE];
                        colTransactionType.Header.Caption = CAPTION_TRANSACTIONTYPE;
                        colTransactionType.Header.VisiblePosition = 4;

                        UltraGridColumn colQty = band.Columns[PROPERTY_QUANTITY];
                        colQty.Header.Caption = "Quantity";
                        colQty.Header.VisiblePosition = 5;

                        UltraGridColumn colAvgPrice = band.Columns[PROPERTY_AVGPRICE];
                        colAvgPrice.Header.Caption = "Avg Price";
                        colAvgPrice.Header.VisiblePosition = 6;

                        UltraGridColumn colAsset = band.Columns[PROPERTY_ASSETTYPE];
                        colAsset.Header.Caption = "Application Asset Type";
                        colAsset.Header.VisiblePosition = 7;

                        UltraGridColumn colPBAsset = band.Columns[PROPERTY_PBAssetType];
                        colPBAsset.Header.Caption = "PB Asset Type";
                        colPBAsset.Header.VisiblePosition = 8;

                        UltraGridColumn colAUEDDate = band.Columns[PROPERTY_AUECDATE];
                        colAUEDDate.Header.Caption = "Trade Date";
                        colAUEDDate.Header.VisiblePosition = 9;

                        UltraGridColumn colProcessDate = band.Columns[PROPERTY_PROCESSDATE];
                        colProcessDate.Header.Caption = "Process Date";
                        colProcessDate.Header.VisiblePosition = 10;

                        UltraGridColumn colOrigDate = band.Columns[PROPERTY_ORIGINALPURCHASEDATE];
                        colOrigDate.Header.Caption = "Original Purchase Date";
                        colOrigDate.Header.VisiblePosition = 11;

                        UltraGridColumn colSettlementDate = band.Columns[PROPERTY_SETTLEMENTDATE];
                        colSettlementDate.Header.Caption = "Settlement Date";
                        colSettlementDate.Header.VisiblePosition = 12;

                        UltraGridColumn colAccountPos = band.Columns[PROPERTY_FUND];
                        colAccountPos.Header.Caption = "Account";
                        colAccountPos.Header.VisiblePosition = 13;

                        UltraGridColumn colExeBroker = band.Columns[PROPERTY_ExecutingBroker];
                        colExeBroker.Header.Caption = "Executing Broker";
                        colExeBroker.Header.VisiblePosition = 14;

                        UltraGridColumn colCommission = band.Columns[OrderFields.PROPERTY_COMMISSION];
                        colCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                        colCommission.Header.VisiblePosition = 15;

                        UltraGridColumn colOtherBrokerFee = band.Columns[PROPERTY_OTHERBROKERFEE];
                        colOtherBrokerFee.Header.Caption = CAPTION_OTHERBROKERFEE;
                        colOtherBrokerFee.Header.VisiblePosition = 16;

                        UltraGridColumn colClearingBrokerFee = band.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                        colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                        colClearingBrokerFee.Header.VisiblePosition = 17;

                        UltraGridColumn colStampDuty = band.Columns[OrderFields.PROPERTY_STAMPDUTY];
                        colStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                        colStampDuty.Header.VisiblePosition = 18;

                        UltraGridColumn colTransactionLevy = band.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                        colTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                        colTransactionLevy.Header.VisiblePosition = 19;

                        UltraGridColumn colClearingFee = band.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                        //Clearing Fee used as AUEC Fee1
                        colClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                        colClearingFee.Header.VisiblePosition = 20;

                        UltraGridColumn colTaxonComm = band.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                        colTaxonComm.Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                        colTaxonComm.Header.VisiblePosition = 21;

                        UltraGridColumn colMiscFees = band.Columns[OrderFields.PROPERTY_MISCFEES];
                        //MiscFees used as AUEC Fee2
                        colMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                        colMiscFees.Header.VisiblePosition = 22;

                        UltraGridColumn colSecFee = band.Columns[OrderFields.PROPERTY_SECFEE];
                        colSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                        colSecFee.Header.VisiblePosition = 23;

                        UltraGridColumn colOccFee = band.Columns[OrderFields.PROPERTY_OCCFEE];
                        colOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                        colOccFee.Header.VisiblePosition = 24;

                        UltraGridColumn colOrfFee = band.Columns[OrderFields.PROPERTY_ORFFEE];
                        colOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                        colOrfFee.Header.VisiblePosition = 25;

                        UltraGridColumn colPosValidate = band.Columns[ApplicationConstants.CONST_VALIDATION_STATUS];
                        colPosValidate.Header.Caption = "Validation Status";
                        colPosValidate.Header.VisiblePosition = 26;
                        colPosValidate.AllowGroupBy = DefaultableBoolean.True;

                        UltraGridColumn colCommissionSource = band.Columns[PROPERTY_COMMISSIONSOURCE];
                        colCommissionSource.Header.Caption = "Commission Source";
                        colCommissionSource.Header.VisiblePosition = 27;

                        UltraGridColumn colSoftCommissionSource = band.Columns[PROPERTY_SOFTCOMMISSIONSOURCE];
                        colSoftCommissionSource.Header.Caption = "Soft Commission Source";
                        colSoftCommissionSource.Header.VisiblePosition = 28;

                        //Added  Is Approved Status of security

                        UltraGridColumn colIsApproved = band.Columns[PROPERTY_SEC_APPROVAL_STATUS];
                        colIsApproved.Header.Caption = "Approved Status";
                        colIsApproved.Header.VisiblePosition = 29;
                        colIsApproved.AllowGroupBy = DefaultableBoolean.True;

                        UltraGridColumn colOptionPremiumAdjustment = band.Columns[OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT];
                        //MiscFees used as AUEC Fee2
                        colOptionPremiumAdjustment.Header.Caption = OrderFields.CAPTION_OPTION_PREMIUM_ADJUSTMENT;
                        colOptionPremiumAdjustment.Header.VisiblePosition = 30;

                        #region swap
                        band.Columns[HEADCOLSWAP_SwapDescription].Header.Caption = "Swap Description";
                        band.Columns[HEADCOLSWAP_OrigTransDate].Header.Caption = "Orig. Trans. Date";
                        band.Columns[HEADCOLSWAP_ResetFrequency].Header.Caption = "Reset Frequency";
                        band.Columns[HEADCOLSWAP_BenchMarkRate].Header.Caption = "BenchMark Rate";
                        band.Columns[HEADCOLSWAP_FirstResetDate].Header.Caption = "First Reset Date";
                        #endregion

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeMarkPrice
                    case _importTypeMarkPrice:

                        UltraGridColumn colSymbolMP = band.Columns[HEADCOLMARK_Symbol];
                        colSymbolMP.Header.Caption = "Symbol";
                        colSymbolMP.Header.VisiblePosition = 1;
                        colSymbolMP.Width = 100;

                        UltraGridColumn colDateMP = band.Columns[HEADCOLMARK_Date];
                        colDateMP.Header.Caption = "Date";
                        colDateMP.Header.VisiblePosition = 2;
                        colDateMP.Width = 100;

                        UltraGridColumn colMarkPrice = band.Columns[HEADCOLMARK_MarkPrice];
                        colMarkPrice.Header.Caption = "Mark Price";
                        colMarkPrice.Header.VisiblePosition = 3;
                        colMarkPrice.Width = 150;


                        UltraGridColumn colIsSecApprovedMP = band.Columns[PROPERTY_IS_APPROVED];
                        colIsSecApprovedMP.Hidden = true;

                        UltraGridColumn colValidateMP = band.Columns[ApplicationConstants.CONST_VALIDATION_STATUS];
                        colValidateMP.Header.Caption = "Validation Status";
                        colValidateMP.Header.VisiblePosition = 4;
                        colValidateMP.AllowGroupBy = DefaultableBoolean.True;

                        UltraGridColumn colIsApprovedMP = band.Columns[PROPERTY_SEC_APPROVAL_STATUS];
                        colIsApprovedMP.Header.Caption = "Approved Status";
                        colIsApprovedMP.Header.VisiblePosition = 5;
                        colIsApprovedMP.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeBeta
                    case _importTypeBeta:
                        UltraGridColumn colSymbolBETA = band.Columns[HEADCOLBETA_Symbol];
                        colSymbolBETA.Header.Caption = "Symbol";
                        colSymbolBETA.Header.VisiblePosition = 1;
                        colSymbolBETA.Width = 100;

                        UltraGridColumn colDateBETA = band.Columns[HEADCOLBETA_Date];
                        colDateBETA.Header.Caption = "Date";
                        colDateBETA.Header.VisiblePosition = 2;
                        colDateBETA.Width = 100;

                        UltraGridColumn colPriceBETA = band.Columns[HEADCOLBETA_Price];
                        colPriceBETA.Header.Caption = "Beta Price";
                        colPriceBETA.Header.VisiblePosition = 3;
                        colPriceBETA.Width = 150;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeCashTransaction
                    case _importTypeActivity:

                        UltraGridColumn columnSymbol = band.Columns[HEADCOLDIV_Symbol];
                        columnSymbol.Header.Caption = "Symbol";
                        columnSymbol.Header.VisiblePosition = 1;
                        columnSymbol.Width = 100;

                        UltraGridColumn colAccountName = band.Columns[HEADCOLDIV_FUND];
                        colAccountName.Header.Caption = "Account Name";
                        colAccountName.Header.VisiblePosition = 2;
                        colAccountName.Width = 100;

                        UltraGridColumn colDividend = band.Columns[HEADCOLDIV_Amount];
                        colDividend.Header.Caption = "Amount";
                        colDividend.Header.VisiblePosition = 3;
                        colDividend.Width = 150;

                        UltraGridColumn columnPBSymbol = band.Columns[HEADCOLDIV_PBSymbol];
                        columnPBSymbol.Header.Caption = "PB Symbol";
                        columnPBSymbol.Header.VisiblePosition = 4;
                        columnPBSymbol.Width = 100;

                        UltraGridColumn colExDate = band.Columns[HEADCOLDIV_ExDate];
                        colExDate.Header.Caption = "Ex Date";
                        colExDate.Header.VisiblePosition = 5;
                        colExDate.Width = 100;

                        UltraGridColumn colPayOutDate = band.Columns[HEADCOLDIV_PayOutDate];
                        colPayOutDate.Header.Caption = "PayOut Date";
                        colPayOutDate.Header.VisiblePosition = 6;
                        colPayOutDate.Width = 100;

                        UltraGridColumn colCashTransactionType = band.Columns[HEADCOLDIV_ActivityType];
                        colCashTransactionType.Header.Caption = "Activity Type";
                        colCashTransactionType.Header.VisiblePosition = 7;
                        colCashTransactionType.Width = 100;

                        UltraGridColumn colValidateDIV = band.Columns[PROPERTY_Validated];
                        colValidateDIV.Header.Caption = "Validation Status";
                        colValidateDIV.Header.VisiblePosition = 8;
                        colValidateDIV.AllowGroupBy = DefaultableBoolean.True;

                        UltraGridColumn ColCurrencyName = band.Columns[OrderFields.PROPERTY_CURRENCYNAME];
                        ColCurrencyName.Header.VisiblePosition = 9;
                        ColCurrencyName.Header.Caption = "Currency";
                        ColCurrencyName.Header.Column.Width = 100;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeForexPrice
                    case _importTypeForexPrice:

                        UltraGridColumn colDateFX = band.Columns[HEADCOLFOREX_Date];
                        colDateFX.Header.Caption = "Date";
                        colDateFX.Header.VisiblePosition = 1;
                        colDateFX.Width = 100;

                        UltraGridColumn colBaseCurrencyFX = band.Columns[HEADCOLFOREX_BaseCurrency];
                        colBaseCurrencyFX.Header.Caption = "From Currency";
                        colBaseCurrencyFX.Header.VisiblePosition = 2;
                        colBaseCurrencyFX.Width = 150;

                        UltraGridColumn colSettlementCurrency = band.Columns[HEADCOLFOREX_SettlementCurrency];
                        colSettlementCurrency.Header.Caption = "To Currency";
                        colSettlementCurrency.Header.VisiblePosition = 3;
                        colSettlementCurrency.Width = 150;

                        UltraGridColumn colForexPrice = band.Columns[HEADCOLFOREX_ForexPrice];
                        colForexPrice.Header.Caption = "Forex Price";
                        colForexPrice.Header.VisiblePosition = 4;
                        colForexPrice.Width = 150;

                        if (!grdImportData.DisplayLayout.Bands[0].Columns.Exists("Summary"))
                        {
                            UltraGridColumn colSummary = grdImportData.DisplayLayout.Bands[0].Columns.Add("Summary");
                            colSummary.Width = 150;
                            colSummary.Header.Caption = "Summary";
                            colSummary.Header.VisiblePosition = 5;
                        }


                        UltraGridColumn colValidateFX = band.Columns[PROPERTY_Validated];
                        colValidateFX.Header.Caption = "Validation Status";
                        colValidateFX.Header.VisiblePosition = 6;
                        colValidateFX.AllowGroupBy = DefaultableBoolean.True;


                        UltraGridColumn colConversionOperator = band.Columns["FXConversionMethodOperator"];
                        colConversionOperator.Hidden = true;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";

                        UltraGridRow[] rows = grdImportData.Rows.GetAllNonGroupByRows();
                        if (rows != null)
                        {
                            foreach (UltraGridRow row in rows)
                            {
                                if (row.Cells[HEADCOLFOREX_ForexPrice].Value != System.DBNull.Value)
                                {
                                    if (Convert.ToDouble(row.Cells[HEADCOLFOREX_ForexPrice].Value) > 0)
                                    {
                                        row.Cells["Summary"].Value = "1 " + row.Cells[HEADCOLFOREX_BaseCurrency].Value + "= " + Convert.ToDouble(row.Cells[HEADCOLFOREX_ForexPrice].Value) + " " + row.Cells[HEADCOLFOREX_SettlementCurrency].Value;
                                    }
                                }
                                else
                                {
                                    row.Cells["Summary"].Value = "";
                                }
                            }
                        }
                        break;
                    #endregion

                    #region _importTypeOMI
                    case _importTypeOMI:
                        grdImportData.DisplayLayout.Bands[0].Columns["Symbol"].Header.VisiblePosition = 1;

                        grdImportData.DisplayLayout.Bands[0].Columns["IntRateUsed"].Header.VisiblePosition = 2;
                        grdImportData.DisplayLayout.Bands[0].Columns["IntRateUsed"].Header.Caption = "";
                        grdImportData.DisplayLayout.Bands[0].Columns["IntRateUsed"].Width = 15;
                        grdImportData.DisplayLayout.Bands[0].Columns["IntRateUsed"].Header.Enabled = false;
                        grdImportData.DisplayLayout.Bands[0].Columns["IntRate"].Header.VisiblePosition = 3;

                        grdImportData.DisplayLayout.Bands[0].Columns["VolatilityUsed"].Header.Caption = "";
                        grdImportData.DisplayLayout.Bands[0].Columns["VolatilityUsed"].Width = 15;
                        grdImportData.DisplayLayout.Bands[0].Columns["VolatilityUsed"].Header.Enabled = false;
                        grdImportData.DisplayLayout.Bands[0].Columns["VolatilityUsed"].Header.VisiblePosition = 4;
                        grdImportData.DisplayLayout.Bands[0].Columns["Volatility"].Header.VisiblePosition = 5;

                        grdImportData.DisplayLayout.Bands[0].Columns["DividendUsed"].Header.VisiblePosition = 6;
                        grdImportData.DisplayLayout.Bands[0].Columns["DividendUsed"].Header.Caption = "";
                        grdImportData.DisplayLayout.Bands[0].Columns["DividendUsed"].Width = 15;
                        grdImportData.DisplayLayout.Bands[0].Columns["DividendUsed"].Header.Enabled = false;
                        grdImportData.DisplayLayout.Bands[0].Columns["Dividend"].Header.VisiblePosition = 7;

                        grdImportData.DisplayLayout.Bands[0].Columns["DeltaUsed"].Header.VisiblePosition = 8;
                        grdImportData.DisplayLayout.Bands[0].Columns["DeltaUsed"].Header.Caption = "";
                        grdImportData.DisplayLayout.Bands[0].Columns["DeltaUsed"].Width = 15;
                        grdImportData.DisplayLayout.Bands[0].Columns["DeltaUsed"].Header.Enabled = false;
                        grdImportData.DisplayLayout.Bands[0].Columns["Delta"].Header.VisiblePosition = 9;

                        grdImportData.DisplayLayout.Bands[0].Columns["LastPriceUsed"].Header.VisiblePosition = 10;
                        grdImportData.DisplayLayout.Bands[0].Columns["LastPriceUsed"].Header.Caption = "";
                        grdImportData.DisplayLayout.Bands[0].Columns["LastPriceUsed"].Width = 15;
                        grdImportData.DisplayLayout.Bands[0].Columns["LastPriceUsed"].Header.Enabled = false;
                        grdImportData.DisplayLayout.Bands[0].Columns["LastPrice"].Header.VisiblePosition = 11;

                        UltraGridColumn colValidateOMI = band.Columns[PROPERTY_Validated];
                        colValidateOMI.Header.Caption = "Validation Status";
                        colValidateOMI.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeSecMasterUpdateData
                    case _importTypeSecMasterUpdateData:

                        #region commented
                        grdImportData.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                        grdImportData.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                        grdImportData.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                        UltraGridColumn ColTickerSymbol = band.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()];
                        ColTickerSymbol.Header.VisiblePosition = 1;
                        ColTickerSymbol.Header.Column.Width = 100;
                        ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                        ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColTickerSymbol.NullText = String.Empty;

                        UltraGridColumn ColLongNameExisting = band.Columns["ExistingLongName"];
                        ColLongNameExisting.Width = 100;
                        ColLongNameExisting.Header.VisiblePosition = 2;
                        ColLongNameExisting.Header.Column.Width = 150;
                        ColLongNameExisting.Header.Caption = "Existing Description";
                        ColLongNameExisting.CharacterCasing = CharacterCasing.Upper;
                        ColLongNameExisting.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColLongNameExisting.NullText = String.Empty;

                        UltraGridColumn ColLongName = band.Columns[OrderFields.PROPERTY_LONGNAME];
                        ColLongName.Width = 100;
                        ColLongName.Header.VisiblePosition = 3;
                        ColLongName.Header.Column.Width = 150;
                        ColLongName.Header.Caption = OrderFields.CAPTION_LONGNAME;
                        ColLongName.CharacterCasing = CharacterCasing.Upper;
                        ColLongName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColLongName.NullText = String.Empty;

                        UltraGridColumn ColMultiplierExisting = band.Columns["ExistingMultiplier"];
                        ColMultiplierExisting.Header.VisiblePosition = 4;
                        ColMultiplierExisting.Header.Column.Width = 70;
                        ColMultiplierExisting.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        ColMultiplierExisting.NullText = null;

                        UltraGridColumn ColMultiplier = band.Columns["Multiplier"];
                        ColMultiplier.Header.VisiblePosition = 5;
                        ColMultiplier.Header.Column.Width = 70;
                        ColMultiplier.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        ColMultiplier.NullText = null;

                        UltraGridColumn ColUnderLyingSymbolExisting = band.Columns["ExistingUnderlyingSymbol"];
                        ColUnderLyingSymbolExisting.Header.VisiblePosition = 6;
                        ColUnderLyingSymbolExisting.Header.Column.Width = 70;
                        ColUnderLyingSymbolExisting.Header.Caption = "Existing Underlying Symbol";
                        ColUnderLyingSymbolExisting.CharacterCasing = CharacterCasing.Upper;
                        ColUnderLyingSymbolExisting.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColUnderLyingSymbolExisting.NullText = String.Empty;

                        UltraGridColumn ColUnderLyingSymbol = band.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
                        ColUnderLyingSymbol.Header.VisiblePosition = 7;
                        ColUnderLyingSymbol.Header.Column.Width = 70;
                        ColUnderLyingSymbol.Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                        ColUnderLyingSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColUnderLyingSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColUnderLyingSymbol.NullText = String.Empty;

                        UltraGridColumn ColPBSymbol = band.Columns["PBSymbol"];
                        ColPBSymbol.Header.VisiblePosition = 8;
                        ColPBSymbol.Header.Column.Width = 70;

                        UltraGridColumn colAssetSMUpdate = band.Columns[OrderFields.PROPERTY_ASSET_ID];
                        colAssetSMUpdate.Header.VisiblePosition = 9;
                        colAssetSMUpdate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colAssetSMUpdate.CellActivation = Activation.NoEdit;
                        colAssetSMUpdate.Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
                        colAssetSMUpdate.ValueList = _assets;
                        colAssetSMUpdate.Header.Column.Width = 70;

                        UltraGridColumn colUnderLying = band.Columns[OrderFields.PROPERTY_UNDERLYING_ID];
                        colUnderLying.Header.VisiblePosition = 10;
                        colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colUnderLying.CellActivation = Activation.NoEdit;
                        colUnderLying.Header.Caption = OrderFields.CAPTION_UNDERLYING_NAME;
                        colUnderLying.ValueList = _underLying;
                        colUnderLying.Header.Column.Width = 70;

                        UltraGridColumn ColExchnage = band.Columns[OrderFields.PROPERTY_EXCHANGEID];
                        ColExchnage.Header.VisiblePosition = 11;
                        ColExchnage.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColExchnage.CellActivation = Activation.NoEdit;
                        ColExchnage.Header.Caption = OrderFields.CAPTION_EXCHANGE;
                        ColExchnage.ValueList = _exchanges;
                        ColExchnage.Header.Column.Width = 100;

                        UltraGridColumn ColCurrency = band.Columns[OrderFields.PROPERTY_CURRENCYID];
                        ColCurrency.Header.VisiblePosition = 12;
                        ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColCurrency.CellActivation = Activation.NoEdit;
                        ColCurrency.Header.Caption = "Currency";
                        ColCurrency.ValueList = _currencies;
                        ColCurrency.Header.Column.Width = 100;

                        if (band.Columns.Contains("ExpirationOrSettlementDate"))
                        {
                            UltraGridColumn ColExpirationOrSettlementDate = band.Columns["ExpirationOrSettlementDate"];
                            ColExpirationOrSettlementDate.Header.Caption = OrderFields.CAPTION_EXPIRATIONDATE; //"Expiration Date";
                            ColExpirationOrSettlementDate.Header.Column.Width = 70;
                            ColExpirationOrSettlementDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                            ColExpirationOrSettlementDate.NullText = "1/1/1800";
                            ColExpirationOrSettlementDate.CellActivation = Activation.NoEdit;
                        }

                        UltraGridColumn ColOptionType = band.Columns[OrderFields.PROPERTY_PUT_CALL];
                        ColOptionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColOptionType.CellActivation = Activation.NoEdit;
                        ColOptionType.ValueList = _optionType;
                        ColOptionType.Header.Column.Width = 70;
                        ColOptionType.Header.Caption = OrderFields.CAPTION_PUT_CALL;
                        ColOptionType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColOptionType.NullText = String.Empty;

                        UltraGridColumn ColStrikePirce = band.Columns[OrderFields.PROPERTY_STRIKE_PRICE];
                        ColStrikePirce.Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                        ColStrikePirce.Header.Column.Width = 70;
                        ColStrikePirce.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        ColStrikePirce.NullText = null;

                        UltraGridColumn ColLeadCurrency = band.Columns[OrderFields.PROPERTY_LEADCURRENCYID];
                        UltraGridColumn ColVsCurrency = band.Columns[OrderFields.PROPERTY_VSCURRENCYID];
                        ColLeadCurrency.ValueList = _currencies;
                        ColLeadCurrency.Header.Caption = "LeadCurrency";
                        ColLeadCurrency.Header.Column.Width = 100;
                        ColLeadCurrency.CellActivation = Activation.NoEdit;
                        ColVsCurrency.ValueList = _currencies;
                        ColVsCurrency.Header.Caption = "VsCurrency";
                        ColVsCurrency.CellActivation = Activation.NoEdit;
                        ColVsCurrency.Header.Column.Width = 100;

                        UltraGridColumn ColReutresSymbol = band.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()];
                        ColReutresSymbol.Header.Column.Width = 100;
                        ColReutresSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColReutresSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                        ColReutresSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColReutresSymbol.NullText = String.Empty;

                        UltraGridColumn ColBloombergSymbol = band.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()];
                        ColBloombergSymbol.Width = 70;
                        ColBloombergSymbol.Header.Column.Width = 100;
                        ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;
                        ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColBloombergSymbol.NullText = String.Empty;

                        UltraGridColumn ColCusipSymbol = band.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()];
                        ColCusipSymbol.Width = 70;
                        ColCusipSymbol.Header.Column.Width = 100;
                        ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                        ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColCusipSymbol.NullText = String.Empty;

                        UltraGridColumn ColISINSymbol = band.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()];
                        ColISINSymbol.Header.Column.Width = 100;
                        ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                        ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColISINSymbol.NullText = String.Empty;

                        UltraGridColumn ColSEDOLSymbol = band.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()];
                        ColSEDOLSymbol.Header.Column.Width = 100;
                        ColSEDOLSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColSEDOLSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                        ColSEDOLSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColSEDOLSymbol.NullText = String.Empty;

                        UltraGridColumn ColOSIOptionSymbol = band.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()];
                        ColOSIOptionSymbol.Header.Column.Width = 150;
                        ColOSIOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColOSIOptionSymbol.Header.Caption = OrderFields.CAPTION_OSIOPTIONSYMBOL;
                        ColOSIOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColOSIOptionSymbol.NullText = String.Empty;

                        UltraGridColumn ColIDCOOptionSymbol = band.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()];
                        ColIDCOOptionSymbol.Header.Column.Width = 150;
                        ColIDCOOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColIDCOOptionSymbol.Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;
                        ColIDCOOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColIDCOOptionSymbol.NullText = String.Empty;

                        UltraGridColumn ColDelta = band.Columns["Delta"];
                        ColDelta.Header.Column.Width = 70;
                        ColDelta.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        ColDelta.NullText = null;
                        ColDelta.Header.Caption = "Leveraged Factor";

                        UltraGridColumn ColOPRAOptionSymbol = band.Columns[ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()];
                        ColOPRAOptionSymbol.Header.Column.Width = 100;
                        ColOPRAOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                        ColOPRAOptionSymbol.Header.Caption = OrderFields.CAPTION_OPRAOPTIONSYMBOL;
                        ColOPRAOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                        ColOPRAOptionSymbol.NullText = String.Empty;


                        UltraGridColumn ColSector = band.Columns[OrderFields.PROPERTY_SECTOR];
                        ColSector.Header.Column.Width = 70;
                        ColSector.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        ColSector.NullText = null;

                        UltraGridColumn ColAccrualBasis = band.Columns["AccrualBasisID"];
                        ColAccrualBasis.Header.Column.Width = 100;
                        ColAccrualBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColAccrualBasis.Header.Caption = OrderFields.CAPTION_ACCRUALBASIS;
                        ColAccrualBasis.ValueList = _accrualBasis;
                        ColAccrualBasis.CellActivation = Activation.NoEdit;
                        ColAccrualBasis.Header.Column.Width = 100;

                        UltraGridColumn ColFrequency = band.Columns["CouponFrequencyID"];
                        ColFrequency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColFrequency.Header.Caption = "Coupon Frequency";
                        ColFrequency.ValueList = _frequency;
                        ColFrequency.CellActivation = Activation.NoEdit;
                        ColFrequency.Header.Column.Width = 100;

                        UltraGridColumn ColUDAAssetClassID = band.Columns["UDAAssetClass"];
                        ColUDAAssetClassID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColUDAAssetClassID.Header.Caption = "UDA Asset";
                        ColUDAAssetClassID.ValueList = SecMasterHelper.getInstance().UDAAssets;
                        ColUDAAssetClassID.CellActivation = Activation.NoEdit;
                        ColUDAAssetClassID.Header.Column.Width = 100;
                        band.Columns["UDAAssetClassID"].Hidden = true;

                        UltraGridColumn ColUDASectorID = band.Columns["UDASector"];
                        ColUDASectorID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColUDASectorID.Header.Caption = "UDA Sector";
                        ColUDASectorID.ValueList = SecMasterHelper.getInstance().UDASectors;
                        ColUDASectorID.CellActivation = Activation.NoEdit;
                        ColUDASectorID.Header.Column.Width = 100;
                        band.Columns["UDASectorID"].Hidden = true;

                        UltraGridColumn ColUDASubSectorID = band.Columns["UDASubSector"];
                        ColUDASubSectorID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColUDASubSectorID.Header.Caption = "UDA SubSector";
                        ColUDASubSectorID.CellActivation = Activation.NoEdit;
                        ColUDASubSectorID.ValueList = SecMasterHelper.getInstance().UDASubSectors;
                        ColUDASubSectorID.Header.Column.Width = 100;
                        band.Columns["UDASubSectorID"].Hidden = true;

                        UltraGridColumn ColUDASecurityTypeID = band.Columns["UDASecurityType"];
                        ColUDASecurityTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColUDASecurityTypeID.Header.Caption = "UDA Security";
                        ColUDASecurityTypeID.CellActivation = Activation.NoEdit;
                        ColUDASecurityTypeID.ValueList = SecMasterHelper.getInstance().UDASecurityTypes;
                        ColUDASecurityTypeID.Header.Column.Width = 100;
                        band.Columns["UDASecurityTypeID"].Hidden = true;

                        UltraGridColumn ColUDACountryID = band.Columns["UDACountry"];
                        ColUDACountryID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        ColUDACountryID.Header.Caption = "UDA Country";
                        ColUDACountryID.ValueList = SecMasterHelper.getInstance().UDACountries;
                        ColUDACountryID.CellActivation = Activation.NoEdit;
                        ColUDACountryID.Header.Column.Width = 100;
                        band.Columns["UDACountryID"].Hidden = true;

                        UltraGridColumn colValidationStatus = band.Columns["ValidationStatus"];
                        colValidationStatus.Header.Caption = "Validation Status";

                        UltraGridColumn colValidateSMUpdateData = band.Columns[PROPERTY_Validated];
                        colValidateSMUpdateData.Header.Caption = "Validation Status";
                        colValidateSMUpdateData.Header.VisiblePosition = 13;
                        colValidateSMUpdateData.AllowGroupBy = DefaultableBoolean.True;
                        colValidateSMUpdateData.Header.Column.Width = 150;
                        SetDynamicUDA(band);
                        SetGridUDAColumns(band);
                        UltraGridColumn colBondTypeID = band.Columns["SecurityTypeID"];
                        colBondTypeID.ValueList = _securityType;
                        colBondTypeID.Header.Caption = "Bond Type";
                        colBondTypeID.Header.VisiblePosition = 14;
                        colBondTypeID.CellActivation = Activation.NoEdit;
                        colBondTypeID.Header.Column.Width = 150;

                        #endregion
                        //SetGridColumns(band);
                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeSecMasterInsert
                    case _importTypeSecMasterInsert:
                        addSymbolManuallyToolStripMenuItem.Visible = false;
                        break;
                    #endregion

                    #region _importTypeDailyCreditLimit
                    case _importTypeDailyCreditLimit:

                        UltraGridColumn colDailyCreditLimitAccount = band.Columns[HEADCOLCREDITLIMIT_FUND];
                        colDailyCreditLimitAccount.Header.Caption = "Account";
                        colDailyCreditLimitAccount.Header.VisiblePosition = 1;
                        colDailyCreditLimitAccount.Width = 200;

                        UltraGridColumn colLongDebitBalance = band.Columns[HEADCOLCREDITLIMIT_LongDebitBalance];
                        colLongDebitBalance.Header.Caption = "Long Debit Balance";
                        colLongDebitBalance.Header.VisiblePosition = 2;
                        colLongDebitBalance.Width = 150;

                        UltraGridColumn colShortCreditBalance = band.Columns[HEADCOLCREDITLIMIT_ShortCreditBalance];
                        colShortCreditBalance.Header.Caption = "Short Credit Balance";
                        colShortCreditBalance.Header.VisiblePosition = 3;
                        colShortCreditBalance.Width = 150;

                        UltraGridColumn colDailyCreditLimitValidate = band.Columns[PROPERTY_Validated];
                        colDailyCreditLimitValidate.Header.Caption = "Validation Status";
                        colDailyCreditLimitValidate.Header.VisiblePosition = 4;
                        colDailyCreditLimitValidate.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(PROPERTY_Validated, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeDailyVolatility
                    case _importTypeDailyVolatility:
                        UltraGridColumn colSymbolVOLATILITY = band.Columns[HEADCOLVOLATILITY_Symbol];
                        colSymbolVOLATILITY.Header.Caption = "Symbol";
                        colSymbolVOLATILITY.Header.VisiblePosition = 1;
                        colSymbolVOLATILITY.Width = 100;

                        UltraGridColumn colDateVOLATILITY = band.Columns[HEADCOLVOLATILITY_Date];
                        colDateVOLATILITY.Header.Caption = "Date";
                        colDateVOLATILITY.Header.VisiblePosition = 2;
                        colDateVOLATILITY.Width = 100;

                        UltraGridColumn colPriceVOLATILITY = band.Columns[HEADCOLVOLATILITY_Price];
                        colPriceVOLATILITY.Header.Caption = "DailyVolatility";
                        colPriceVOLATILITY.Header.VisiblePosition = 3;
                        colPriceVOLATILITY.Width = 150;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeDailyVWAP
                    case _importTypeDailyVWAP:
                        UltraGridColumn colSymbolVWAP = band.Columns[HEADCOLVWAP_Symbol];
                        colSymbolVWAP.Header.Caption = "Symbol";
                        colSymbolVWAP.Header.VisiblePosition = 1;
                        colSymbolVWAP.Width = 100;

                        UltraGridColumn colDateVWAP = band.Columns[HEADCOLVWAP_Date];
                        colDateVWAP.Header.Caption = "Date";
                        colDateVWAP.Header.VisiblePosition = 2;
                        colDateVWAP.Width = 100;

                        UltraGridColumn colPriceVWAP = band.Columns[HEADCOLVWAP_Price];
                        colPriceVWAP.Header.Caption = "DailyVWAP";
                        colPriceVWAP.Header.VisiblePosition = 3;
                        colPriceVWAP.Width = 150;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeCollateralPrice
                    case _importTypeCollateralPrice:
                        UltraGridColumn colSymbolCollateral = band.Columns[HEADCOLCOLLATERAL_Symbol];
                        colSymbolCollateral.Header.Caption = "Symbol";
                        colSymbolCollateral.Header.VisiblePosition = 1;
                        colSymbolCollateral.Width = 100;

                        UltraGridColumn colDateCollateral = band.Columns[HEADCOLCOLLATERAL_Date];
                        colDateCollateral.Header.Caption = "Date";
                        colDateCollateral.Header.VisiblePosition = 2;
                        colDateCollateral.Width = 100;

                        UltraGridColumn colFundCollateral = band.Columns[HEADCOLCOLLATERAL_Fund];
                        colFundCollateral.Header.Caption = "Account Name";
                        colFundCollateral.Header.VisiblePosition = 3;
                        colFundCollateral.Width = 150;

                        UltraGridColumn colPriceCollateral = band.Columns[HEADCOLCOLLATERAL_Price];
                        colPriceCollateral.Header.Caption = "Collateral Price";
                        colPriceCollateral.Header.VisiblePosition = 4;
                        colPriceCollateral.Width = 150;

                        UltraGridColumn colHaircutCollateral = band.Columns[HEADCOLCOLLATERAL_Haircut];
                        colHaircutCollateral.Header.Caption = "Haircut(%)";
                        colHaircutCollateral.Header.VisiblePosition = 5;
                        colHaircutCollateral.Width = 150;

                        UltraGridColumn colFeeRebateMVCollateral = band.Columns[HEADCOLCOLLATERAL_FeeRebateMV];
                        colFeeRebateMVCollateral.Header.Caption = "Fee/Rebate on MV(%)";
                        colFeeRebateMVCollateral.Header.VisiblePosition = 6;
                        colFeeRebateMVCollateral.Width = 150;

                        UltraGridColumn colFeeRebateCollateral = band.Columns[HEADCOLCOLLATERAL_FeeRebateCollateral];
                        colFeeRebateCollateral.Header.Caption = "Fee/Rebate on Collateral(%)";
                        colFeeRebateCollateral.Header.VisiblePosition = 7;
                        colFeeRebateCollateral.Width = 150;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeDailyDividendYield
                    case _importTypeDailyDividendYield:
                        UltraGridColumn colSymbolDIVIDENDYIELD = band.Columns[HEADCOLDIVIDENDYIELD_Symbol];
                        colSymbolDIVIDENDYIELD.Header.Caption = "Symbol";
                        colSymbolDIVIDENDYIELD.Header.VisiblePosition = 1;
                        colSymbolDIVIDENDYIELD.Width = 100;

                        UltraGridColumn colDateDIVIDENDYIELD = band.Columns[HEADCOLDIVIDENDYIELD_Date];
                        colDateDIVIDENDYIELD.Header.Caption = "Date";
                        colDateDIVIDENDYIELD.Header.VisiblePosition = 2;
                        colDateDIVIDENDYIELD.Width = 100;

                        UltraGridColumn colPriceDIVIDENDYIELD = band.Columns[HEADCOLDIVIDENDYIELD_Price];
                        colPriceDIVIDENDYIELD.Header.Caption = "DividendYield Price";
                        colPriceDIVIDENDYIELD.Header.VisiblePosition = 3;
                        colPriceDIVIDENDYIELD.Width = 150;

                        band.SortedColumns.Add(ApplicationConstants.CONST_VALIDATION_STATUS, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeStagedOrder
                    case _importTypeStagedOrder:

                        foreach (UltraGridColumn col in grdImportData.DisplayLayout.Bands[0].Columns)
                        {
                            col.Hidden = true;
                            if (_visibleColumns.Contains(col.Key))
                            {
                                col.Hidden = false;
                            }
                        }
                        UltraGridColumn colCheckBox = band.Columns["checkbox"];
                        colcheckBox.Hidden = false;

                        DataTable dataTableTIF = TagDatabase.GetInstance().TIF;
                        ValueList timeInForce = new ValueList();
                        foreach (DataRow item in dataTableTIF.Rows)
                        {
                            timeInForce.ValueListItems.Add(Convert.ToString(item[1]), Convert.ToString(item[2]));
                        }
                        timeInForce.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);

                        if (grdImportData.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_TIF_TAGVALUE))
                        {
                            grdImportData.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].ValueList = timeInForce;
                            grdImportData.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TIF_TAGVALUE].CellActivation = Activation.NoEdit;
                        }

                        UltraGridColumn colStageSymbol = band.Columns[HEADCOL_Symbol];
                        colStageSymbol.Header.Caption = "Symbol";
                        colStageSymbol.Header.VisiblePosition = 1;
                        colStageSymbol.Width = 150;
                        colStageSymbol.Hidden = false;

                        UltraGridColumn colStageSide = band.Columns[HEADCOL_OrderSide];
                        colStageSide.Header.Caption = "Side";
                        colStageSide.Header.VisiblePosition = 2;
                        colStageSide.Width = 100;
                        colStageSide.Hidden = false;

                        UltraGridColumn colStageAssetName = band.Columns[HEADCOL_AssetName];
                        colStageAssetName.Header.Caption = "Asset";
                        colStageAssetName.Header.VisiblePosition = 3;
                        colStageAssetName.Width = 100;
                        colStageAssetName.Hidden = false;

                        UltraGridColumn colStageQuantity = band.Columns[HEADCOL_Quantity];
                        colStageQuantity.Header.Caption = "Quantity";
                        colStageQuantity.Header.VisiblePosition = 4;
                        colStageQuantity.Width = 100;
                        colStageQuantity.Hidden = false;

                        UltraGridColumn colStageAccount = band.Columns[HEADCOL_Account];
                        colStageAccount.Header.Caption = "Account";
                        colStageAccount.Header.VisiblePosition = 5;
                        colStageAccount.Width = 200;
                        colStageAccount.Hidden = false;

                        UltraGridColumn colStageDate = band.Columns[HEADCOL_Date];
                        colStageDate.Header.Caption = "Date";
                        colStageDate.Header.VisiblePosition = 6;
                        colStageDate.Width = 150;
                        colStageDate.Hidden = false;

                        UltraGridColumn colStageTIF = band.Columns[HEADCOL_TIF];
                        colStageTIF.Header.Caption = "TIF";
                        colStageTIF.Header.VisiblePosition = 7;
                        colStageTIF.Width = 100;
                        colStageTIF.Hidden = false;

                        UltraGridColumn colStageOrderType = band.Columns[HEADCOL_OrderType];
                        colStageOrderType.Header.Caption = "Order Type";
                        colStageOrderType.Header.VisiblePosition = 8;
                        colStageOrderType.Width = 100;
                        colStageOrderType.Hidden = false;

                        UltraGridColumn colStageStrategy = band.Columns[HEADCOL_Strategy];
                        colStageStrategy.Header.Caption = "Strategy";
                        colStageStrategy.Header.VisiblePosition = 9;
                        colStageStrategy.Width = 150;
                        colStageStrategy.Hidden = false;

                        UltraGridColumn colStageBroker = band.Columns[HEADCOL_Broker];
                        colStageBroker.Header.Caption = "Broker";
                        colStageBroker.Header.VisiblePosition = 10;
                        colStageBroker.Width = 100;
                        colStageBroker.Hidden = false;

                        UltraGridColumn colStageVenue = band.Columns[HEADCOL_Venue];
                        colStageVenue.Header.Caption = "Venue";
                        colStageVenue.Header.VisiblePosition = 11;
                        colStageVenue.Width = 100;
                        colStageVenue.Hidden = false;

                        UltraGridColumn stageOrderValidate = band.Columns[HEADCOL_ValidationStatus];
                        stageOrderValidate.Header.Caption = "Validation Status";
                        stageOrderValidate.Header.VisiblePosition = 26;
                        stageOrderValidate.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(HEADCOL_ValidationStatus, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    #region _importTypeColIntMasterInsert
                    case _importTypeColIntMasterInsert:

                        foreach (UltraGridColumn col in grdImportData.DisplayLayout.Bands[0].Columns)
                        {
                            col.Hidden = true;
                            if (_visibleColumns.Contains(col.Key))
                            {
                                col.Hidden = false;
                            }
                        }

                        UltraGridColumn colCollateralCheckBox = band.Columns["checkbox"];
                        colCollateralCheckBox.Hidden = false;

                        UltraGridColumn colCollateralFundID = band.Columns[HEADCOL_FundID];
                        colCollateralFundID.Header.Caption = "Account";
                        colCollateralFundID.Header.VisiblePosition = 1;
                        colCollateralFundID.Width = 100;
                        colCollateralFundID.Hidden = false;

                        UltraGridColumn colCollateralBenchmarkName = band.Columns[HEADCOL_BenchmarkName];
                        colCollateralBenchmarkName.Header.Caption = "BenchmarkName";
                        colCollateralBenchmarkName.Header.VisiblePosition = 2;
                        colCollateralBenchmarkName.Width = 100;
                        colCollateralBenchmarkName.Hidden = false;

                        UltraGridColumn colCollateralBenchmarkRate = band.Columns[HEADCOL_BenchmarkRate];
                        colCollateralBenchmarkRate.Header.Caption = "BenchmarkRate";
                        colCollateralBenchmarkRate.Header.VisiblePosition = 3;
                        colCollateralBenchmarkRate.Width = 100;
                        colCollateralBenchmarkRate.Hidden = false;

                        UltraGridColumn colCollateralSpread = band.Columns[HEADCOL_Spread];
                        colCollateralSpread.Header.Caption = "Spread";
                        colCollateralSpread.Header.VisiblePosition = 4;
                        colCollateralSpread.Width = 100;
                        colCollateralSpread.Hidden = false;

                        UltraGridColumn colCollateralValidation = band.Columns[HEADCOL_CollateralValidationStatus];
                        colCollateralValidation.Header.Caption = "Validation Status";
                        colCollateralValidation.Header.VisiblePosition = 26;
                        colCollateralValidation.AllowGroupBy = DefaultableBoolean.True;

                        band.SortedColumns.Add(colCollateralValidation, false, true);
                        grdImportData.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                        break;
                    #endregion

                    default:
                        break;
                }
                blnCloseforSave = false;
                toolStripStatusLabel1.Text = " ";

                //Loading saved layout on grid - om

                String impotTypetext = this.Text.Replace(" ", "_");
                String filePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\ImportUI_" + impotTypetext + ".xml";
                if (File.Exists(filePath))
                {
                    grdImportData.DisplayLayout.LoadFromXml(filePath, PropertyCategories.All);
                    toolStripStatusLabel1.Text = "Saved Layout loaded! ";
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DATA);
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);

                        if (_positionType == _importTypeMultilegJournalImport)
                        {
                            CustomThemeHelper.SetThemeProperties(this.grdImportData, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_MULTILEG_IMPORT);
                        }
                    }
                }
                BusinessObjects.Classes.NAVLockDateRule.NAVLockDate = CachedDataManager.GetInstance.NAVLockDate;
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
                ultraButton1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton2.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton2.ForeColor = System.Drawing.Color.White;
                ultraButton2.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
                ultraButton2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton2.UseAppStyling = false;
                ultraButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnMapping.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnMapping.ForeColor = System.Drawing.Color.White;
                btnMapping.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
                btnMapping.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnMapping.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnMapping.UseAppStyling = false;
                btnMapping.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSymbolLookup.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSymbolLookup.ForeColor = System.Drawing.Color.White;
                btnSymbolLookup.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
                btnSymbolLookup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSymbolLookup.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSymbolLookup.UseAppStyling = false;
                btnSymbolLookup.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ContinueClick();
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

        delegate void EnableDisableLoaderCallback(bool enable);
        /// <summary>
        /// This method is for Enable and Disable Loader
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableDisableLoader(bool enabled)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    EnableDisableLoaderCallback d = new EnableDisableLoaderCallback(EnableDisableLoader);
                    this.Invoke(d, new object[] { enabled });
                }
                else
                {
                    this.Enabled = !enabled;
                    panel.Visible = enabled;
                    panel.Enabled = enabled;
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
        private void ContinueClick()
        {
            try
            {
                switch (_positionType)
                {
                    case _importTypeCash:

                        _validatedCashCurrencyValue = new List<CashCurrencyValue>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString().Equals("Validated") && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedCashCurrencyValue.Add((CashCurrencyValue)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeSettlementDateCash:

                        _validatedSettlementDateCashCurrencyValue = new List<SettlementDateCashCurrencyValue>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString().Equals("Validated") && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedSettlementDateCashCurrencyValue.Add((SettlementDateCashCurrencyValue)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeTransaction:
                    case _importTypeNetPosition:

                        _validatedPositions = new List<PositionMaster>();
                        int invalidPositions = 0;
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString().Equals("Validated") && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedPositions.Add((PositionMaster)row.ListObject);
                            }
                            else if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                invalidPositions++;
                            }
                        }

                        // modified: omshiv, Jan 28, 2014, If user select invalid rows then we will prompt to user,
                        //if user cancle
                        if (invalidPositions > 0)
                        {
                            DialogResult result = MessageBox.Show("You have selected invalid trades also, but they will not import." + System.Environment.NewLine + "Do you want to continue? Click 'Yes' for continue and 'No' for cancel import. ", "Nirvana Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.No)
                            {
                                _validatedPositions.Clear();
                            }
                        }
                        break;

                    case _importTypeStagedOrder:
                        _validatedStageOrder = new List<OrderSingle>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedStageOrder.Add((OrderSingle)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeMarkPrice:
                        _validatedMarkpriceValue = new List<MarkPriceImport>();
                        //GetFilteredInNonGroupByRows
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedMarkpriceValue.Add((MarkPriceImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeBeta:
                        _validatedBetaValues = new List<BetaImport>();
                        //GetFilteredInNonGroupByRows
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedBetaValues.Add((BetaImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeForexPrice:
                        _validatedForexpriceValue = new List<ForexPriceImport>();
                        //GetFilteredInNonGroupByRows
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedForexpriceValue.Add((ForexPriceImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeActivity:

                        _validatedCashTransactionValue = new List<DividendImport>();
                        //GetFilteredInNonGroupByRows
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedCashTransactionValue.Add((DividendImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeOMI:
                        _validatedOmiValue = new List<UserOptModelInput>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedOmiValue.Add((UserOptModelInput)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeSecMasterInsert:
                        _dsSecMasterInsert = ((DataSet)grdImportData.DataSource).Clone();

                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true") && row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString().Equals("Validated"))
                            {
                                DataRowView drView = (DataRowView)row.ListObject;
                                DataRow dr = _dsSecMasterInsert.Tables[CONSTSTR_TableName].NewRow();
                                dr.ItemArray = drView.Row.ItemArray;
                                _dsSecMasterInsert.Tables[CONSTSTR_TableName].Rows.Add(dr);
                            }
                        }
                        break;

                    case _importTypeColIntMasterInsert:
                        _dsColInterstImportValue = ((DataSet)grdImportData.DataSource).Clone();

                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true") && row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString().Equals("Validated"))
                            {
                                DataRowView drView = (DataRowView)row.ListObject;
                                DataRow dr = _dsColInterstImportValue.Tables[CONSTSTR_TableName].NewRow();
                                dr.ItemArray = drView.Row.ItemArray;
                                _dsColInterstImportValue.Tables[CONSTSTR_TableName].Rows.Add(dr);
                            }
                        }
                        break;

                    case _importTypeSecMasterUpdateData:
                        _validatedSecMasterUpdateDataValues = new SecMasterUpdateDataByImportList();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[PROPERTY_Validated].Value.ToString().Equals("Validated") && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedSecMasterUpdateDataValues.Add((SecMasterUpdateDataByImportUI)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeDailyCreditLimit:
                        _validatedDailyCreditLimitValue = new List<DailyCreditLimit>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells["Validated"].Value.ToString().Equals("Validated") && row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                            {
                                _validatedDailyCreditLimitValue.Add((DailyCreditLimit)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeDoubleEntryCash:
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            //CashAccountCache.GetInstance.SaveCashDividendValue(ds.GetXml());
                            //PRANA-9776 CachedDataManager.GetInstance.LoggedInUser.CompanyUserID
                            int numberOfEntriesCreated = CashDataManager.GetInstance().CreateJournalEntries(ds, CashTransactionType.ImportedEditableData, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);

                            if (ds != null && ds.Tables.Count > 0)
                                AddAuditEntryForDoubleEntryImport(ds.Tables[0], "Manual Journal Imported");

                            this.NoOfDoubleEntryCashCreated = numberOfEntriesCreated;
                            if (numberOfEntriesCreated == 0)
                            {
                                MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Position is selected, Please select at least One", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;

                    case _importTypeMultilegJournalImport:
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            DataSet NewDataset = new DataSet();
                            ds.Tables[0].DefaultView.Sort = "PRIMARY_KEY";
                            DataTable dt = ds.Tables[0].DefaultView.ToTable();
                            NewDataset.Tables.Add(dt);
                            ds = NewDataset;
                            int numberOfEntriesCreated = CashDataManager.GetInstance().CreateJournalEntries(ds, CashTransactionType.ImportedEditableData, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, true);

                            if (ds != null && ds.Tables.Count > 0)
                                AddAuditEntryForDoubleEntryImport(ds.Tables[0], "Multileg Journal Imported");

                            this.NoOfMultiLegJournalCreated = numberOfEntriesCreated;
                            if (numberOfEntriesCreated == 0)
                            {
                                MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Position is selected, Please select at least One", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;

                    case _importTypeDailyVolatility:
                        _validatedDailyVolatilityValue = new List<VolatilityImport>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedDailyVolatilityValue.Add((VolatilityImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeDailyVWAP:
                        _validatedDailyVWAPValue = new List<VWAPImport>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedDailyVWAPValue.Add((VWAPImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeCollateralPrice:
                        _validatedDailyCollateralValue = new List<CollateralImport>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _validatedDailyCollateralValue.Add((CollateralImport)row.ListObject);
                            }
                        }
                        break;

                    case _importTypeDailyDividendYield:
                        _vaidatedDailyDividendYieldValue = new List<DividendYieldImport>();
                        foreach (UltraGridRow row in grdImportData.Rows.GetAllNonGroupByRows())
                        {
                            if (row.Cells[ApplicationConstants.CONST_VALIDATION_STATUS].Value.ToString() == "Validated" && row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                            {
                                _vaidatedDailyDividendYieldValue.Add((DividendYieldImport)row.ListObject);
                            }
                        }
                        break;

                    default:
                        break;
                }
                blnCloseforSave = true;
                if (!IsComingFromBlotter)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (ValidatedStageOrder.Count > 0)
                    {
                        _totalTradeCount = ValidatedStageOrder.Count;
                        SendTradesToBlotter();
                    }
                    else
                    {
                        MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
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
        /// <summary>
        /// This method send trades to the blotter
        /// </summary>
        private void SendTradesToBlotter()
        {
            try
            {
                if (SendTradesToBlotterEvent != null)
                {
                    SendTradesToBlotterEvent.BeginInvoke(this, new EventArgs<List<OrderSingle>>(ValidatedStageOrder), null, null);
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
        /// This method is for updating feedback messgae for blotter stage import
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        delegate void FeedbackMessageCallback(string text);
        private void UpdateFeedbackMessage(object sender, EventArgs<int> e)
        {
            int tradeNumber = e.Value;
            if(tradeNumber + _unsuccessfulTradeCount >= _totalTradeCount)
            {
                CloseImportForm();
            }
            else
            {
                if (tradeNumber != -1)
                {
                    _symbolsSentToBlotterCount++;
                    SetMessage(string.Format(CAPTION_FEEDBACK_MESSAGE, _symbolsSentToBlotterCount, _totalTradeCount));
                }
                else
                {
                    _unsuccessfulTradeCount++;
                    if (tradeNumber + _unsuccessfulTradeCount >= _totalTradeCount)
                    {
                        CloseImportForm();
                    }
                }
            }
        }
        /// <summary>
        /// This method is for setting the display messages
        /// </summary>
        /// <param name="text"></param>
        private void SetMessage(string text)
        {
            try
            {
                if (feedbackLabel.InvokeRequired)
                {
                    FeedbackMessageCallback d = new FeedbackMessageCallback(SetMessage);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    feedbackLabel.Text = text;
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
        /// This method is for updating the total trade count after grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTotalCountAfterGrouping(object sender, EventArgs<int> e)
        {
            try
            {
                _totalTradeCount = e.Value;
                SetMessage(string.Format(CAPTION_FEEDBACK_MESSAGE, _symbolsSentToBlotterCount, _totalTradeCount));
                EnableDisableLoader(true);
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
        /// This method is for closing the import form
        /// </summary>
        delegate void CloseFormCallback();
        private void CloseImportForm()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    CloseFormCallback close = new CloseFormCallback(CloseImportForm);
                    this.Invoke(close, new object[] {});
                }
                else
                {
                    this.Close();
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
        public bool AddAuditEntryForDoubleEntryImport(DataTable dt, string comment)
        {
            try
            {
                if (dt != null && comment != null)
                {
                    List<CashJournalAuditEntry> lstCashJournalAudit = new List<CashJournalAuditEntry>();
                    DataColumnCollection columns = dt.Columns;
                    int userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (columns.Contains("CurrencyId") && Convert.ToInt32(row["CurrencyId"]) != 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(Convert.ToInt32(row["CurrencyID"])) && columns.Contains("AccountName") && CachedDataManager.GetInstance.GetAccountID(Convert.ToString(row["AccountName"])) != int.MinValue)
                        {
                            CashJournalAuditEntry cashauidtentry = new CashJournalAuditEntry(Convert.ToDateTime(row["Date"]), CachedDataManager.GetInstance.GetAllCurrencies()[Convert.ToInt32(row["CurrencyID"])], CachedDataManager.GetInstance.GetAccountID(Convert.ToString(row["AccountName"])), comment, userId, 0, string.Empty, 0.0M, 0.0M, 0.0);
                            lstCashJournalAudit.Add(cashauidtentry);
                        }
                        else if (columns.Contains("CurrencyName") && !string.IsNullOrEmpty(Convert.ToString(row["CurrencyName"])) && columns.Contains("AccountName") && CachedDataManager.GetInstance.GetAccountID(Convert.ToString(row["AccountName"])) != int.MinValue)
                        {
                            CashJournalAuditEntry cashauidtentry = new CashJournalAuditEntry(Convert.ToDateTime(row["Date"]), Convert.ToString(row["CurrencyName"]), CachedDataManager.GetInstance.GetAccountID(Convert.ToString(row["AccountName"])), comment, userId, 0, string.Empty, 0.0M, 0.0M, 0.0);
                            lstCashJournalAudit.Add(cashauidtentry);
                        }

                    }

                    if (lstCashJournalAudit.Count > 0)
                        AuditManager.Instance.SaveAuditListForCashJournal(lstCashJournalAudit);
                    lstCashJournalAudit.Clear();
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

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImportPositionsDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (blnCloseforSave == false)
                {
                    if (MessageBox.Show("Are you sure to abort the import process ?", "Import", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Abort;
                        SendTradesToBlotterEvent = null;
                        _importPositionsDisplayForm = null;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    _importPositionsDisplayForm = null;
                }
                if (UpdateAfterCloseEvent != null && !e.Cancel)
                {
                    if(ValidatedStageOrder != null && ValidatedStageOrder.Count > 0)
                    {
                        UpdateAfterCloseEvent(this, new EventArgs<bool>(blnCloseforSave));
                    }
                    else
                    {
                        //This is to handle scenario where no valid records selected to send to blotter
                        UpdateAfterCloseEvent(this, new EventArgs<bool>(false));
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

        private void grdImportData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                AddCheckBoxinGrid(grdImportData, headerCheckBox);
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
        /// This method is for handing the event when we make any change on cell level
        /// </summary>
        /// <param name="sender">Ultragrid</param>
        /// <param name="e">Cell event argument</param>
        private void grdImportData_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                // When the user changes a checkbox cell, we have to update the header checkbox. 
                var grid = (UltraGrid)sender;

                // We only care about the boolean column
                if (e.Cell.Column.Key == HEADCOL_checkBox)
                {
                    // Store the new CheckState of the header checkbox. Start off with null, 
                    // so we have a clean slate. 
                    CheckState? checkState = null;

                    // Loop through the rows. 
                    var rows = e.Cell.Row.ParentCollection;

                    //To count slected rows on sub grid
                    var selectRowCount = 0;
                    foreach (var row in rows)
                    {
                        var rowCheckState = bool.Parse(row.Cells["checkBox"].Text);

                        if (true == rowCheckState)
                        {
                            selectRowCount++;
                        }
                    }
                    grid.EventManager.SetEnabled(GridEventIds.AfterHeaderCheckStateChanged, false);
                    try
                    {
                        if (selectRowCount == 0)
                        {
                            checkState = CheckState.Unchecked;
                        }
                        else if (selectRowCount == rows.Count)
                        {
                            checkState = CheckState.Checked;
                        }
                        else
                        {
                            checkState = CheckState.Indeterminate;
                        }
                        e.Cell.Column.SetHeaderCheckedState(rows, checkState.Value);
                    }
                    finally
                    {
                        grid.EventManager.SetEnabled(GridEventIds.AfterHeaderCheckStateChanged, true);
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
        /// This method is for handing the event when Grouping is changed
        /// </summary>
        /// <param name="sender">Ultragrid</param>
        /// <param name="e">Ultragrid Band event argument</param>
        private void grdImportData_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                int currentGroupByColumnCount = 0;
                for (int i = 0; i < e.Band.SortedColumns.Count; i++)
                {
                    UltraGridColumn sortColumn = e.Band.SortedColumns[i];

                    if (sortColumn.IsGroupByColumn)
                        currentGroupByColumnCount += 1;
                }

                if (currentGroupByColumnCount != _groupByColumnCount)
                {
                    UnCheckHeaderAndGridCheckbox(sender);
                    _groupByColumnCount = currentGroupByColumnCount;
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
        /// This method is uncheck all the rows when when grouping is applied or removed
        /// </summary>
        /// <param name="sender">Ultragrid</param>
        private void UnCheckHeaderAndGridCheckbox(object sender)
        {
            try
            {
                var grid = (UltraGrid)sender;
                grid.EventManager.SetEnabled(GridEventIds.AfterHeaderCheckStateChanged, false);
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_checkBox].SetHeaderCheckedState(grid.Rows, CheckState.Unchecked);
                grid.EventManager.SetEnabled(GridEventIds.AfterHeaderCheckStateChanged, true);
                foreach (var row in grid.Rows)
                {
                    if (row is UltraGridGroupByRow)
                    {
                        UltraGridGroupByRow innerRow = row as UltraGridGroupByRow;
                        SetGroupByRowInnerRowCheckBox(innerRow);
                    }
                    else
                    {
                        SetGridRowCheckBox(row);
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

        /// <summary>
        /// This method is for handing the event when Infragistic Header checkbox changes
        /// </summary>
        /// <param name="sender">UltraGrid</param>
        /// <param name="e">Header Checkbox event argument</param>
        private void grdImportData_AfterHeaderCheckStateChanged(object sender, Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                RowsCollection rows = e.Rows;
                var checkState = e.Column.GetHeaderCheckedState(rows);
                foreach (var row in rows)
                {
                    SetGridRowCheckBox(row, checkState, row.HiddenResolved);
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
        /// This method is to change the checkbox of GroupByRow's InnerRow
        /// </summary>
        /// <param name="groupByRow">UltraGridGroupByRow Collection</param>
        private void SetGroupByRowInnerRowCheckBox(UltraGridGroupByRow groupByRow)
        {
            try
            {
                foreach (var row in groupByRow.Rows)
                {
                    if (row is UltraGridGroupByRow)
                    {
                        UltraGridGroupByRow innerRow = row as UltraGridGroupByRow;
                        SetGroupByRowInnerRowCheckBox(innerRow);
                    }
                    else
                    {
                        SetGridRowCheckBox(row);
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

        /// <summary>
        /// This method is to change the checkbox of particular row
        /// </summary>
        /// <param name="row">Ultragrid row</param>
        /// <param name="checkState">CheckBox state</param>
        /// <param name="row_HiddenResolved">row is Hidden/Not Hidden (true- Hidden,false- Not Hidden)</param>
        private void SetGridRowCheckBox(UltraGridRow row, CheckState checkState = CheckState.Unchecked, bool row_HiddenResolved = false)
        {
            try
            {
                if (row_HiddenResolved == false && row.Cells != null)
                {
                    if (row.Cells.Exists(HEADCOL_checkBox))
                        row.Cells[HEADCOL_checkBox].Value = checkState == CheckState.Checked ? CheckState.Checked : CheckState.Unchecked;
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

        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        /// <summary>
        /// For adding the Header Checkbox on Grid
        /// </summary>
        /// <param name="grid">UltraGrid</param>
        /// <param name="headerCheckBox">Custom Created Header CheckBox class object</param>
        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
            grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.EditAndSelectText;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            if (_positionType == _importTypeStagedOrder)
            {
                //To make header checkbox and applying checkbox handling
                _groupByColumnCount = 1;
                grid.CellChange += grdImportData_CellChange;
                grid.AfterHeaderCheckStateChanged += grdImportData_AfterHeaderCheckStateChanged;
                grid.AfterSortChange += grdImportData_AfterSortChange;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].SortIndicator = SortIndicator.Disabled;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.CheckBoxVisibility = Infragistics.Win.UltraWinGrid.HeaderCheckBoxVisibility.Always;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.CheckBoxSynchronization = Infragistics.Win.UltraWinGrid.HeaderCheckBoxSynchronization.None;
            }
            else
            {
                grid.CreationFilter = headerCheckBox;
                headerCheckBox._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBox__CLICKED);
            }
            SetCheckBoxAtFirstPosition(grid);
            if (_positionType == "SubAccount Cash")
            {
                DataSet dsNew = CachedDataManager.GetInstance.GetMasterCategorySubCategoryTables();
                if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                {
                    if (!dsNew.Relations.Contains("SubCashAccounts"))
                    {
                        //dsNew.Relations.Add("SubCashAccounts", dsNew.Tables["CashAccounts"].Columns["AccountID"], dsNew.Tables["SubCashAccounts"].Columns["AccountID"], false);
                    }
                    subAccount = new ValueList();
                    foreach (DataRow dr in dsNew.Tables["CashAccounts"].Rows)
                    {
                        DataRow[] subAccounts = dr.GetChildRows("SubCashAccounts");
                        foreach (DataRow row in subAccounts)
                        {
                            subAccount.ValueListItems.Add(Convert.ToInt32(row["SubAccountID"]), row["Acronym"].ToString());
                        }
                    }
                }
            }
            if (_positionType.Equals(_importTypeTransaction) || _positionType.Equals(_importTypeNetPosition))
            {
                grid.DisplayLayout.Bands[0].Columns["TransactionType"].ValueList = ValueListHelper.GetInstance.GetTransactionTypeValueList().Clone();
                grid.DisplayLayout.Bands[0].Columns["TransactionType"].CellActivation = Activation.NoEdit;
            }
        }

        void headerCheckBox__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            if (_positionType == "SubAccount Cash")
            {
                if (ds == null)
                {
                    DataSet dsNew = ((DataSet)grdImportData.DataSource);
                    ds = dsNew.Clone();
                }
                if (e.CurrentCheckState == CheckState.Checked)
                {
                    foreach (UltraGridRow drNew in e.Rows.GetFilteredInNonGroupByRows())
                    {
                        DataRowView drn = (DataRowView)drNew.ListObject;
                        if (!ds.Tables["PositionMaster"].Rows.Contains(drn.Row["PRIMARY_KEY"]))
                        {
                            DataRow dr = ds.Tables["PositionMaster"].NewRow();
                            dr.ItemArray = drn.Row.ItemArray;
                            ds.Tables["PositionMaster"].Rows.Add(dr);
                        }
                    }
                }
                else if (e.CurrentCheckState == CheckState.Unchecked)
                {
                    foreach (UltraGridRow drNew in e.Rows.GetFilteredInNonGroupByRows())
                    {
                        DataRowView drn = (DataRowView)drNew.ListObject;
                        if (ds.Tables["PositionMaster"].Rows.Contains(drn.Row["PRIMARY_KEY"]))
                        {
                            ds.Tables[0].DefaultView.Sort = "PRIMARY_KEY";
                            int i = ds.Tables["PositionMaster"].DefaultView.Find(drn.Row["PRIMARY_KEY"]);
                            ds.Tables["PositionMaster"].Rows.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        DataSet ds = null;
        DataSet NewDatatset = null;
        private void SetErrorForMultileg(DataRow row)
        {
            try
            {
                foreach (DataColumn dc in row.Table.Columns)
                {
                    if (!(dc.Caption == HEADCOL_Symbol || dc.Caption == HEADCOL_Description))
                    {
                        if (row[dc].Equals(String.Empty))
                        {                           
                            row.SetColumnError(dc.Caption, "Select " + dc.Caption + "!");
                            row[PROPERTY_Validated] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
                            int EntryID = Convert.ToInt32(row[HEADCOL_EntryID]);
                            foreach (DataRow rowobject in NewDatatset.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(rowobject[HEADCOL_EntryID]) == EntryID)
                                {
                                    rowobject[PROPERTY_Validated] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
                                }

                            }
                        }
                        else
                        {
                            row.SetColumnError(dc.Caption, "");
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
        private void SetError(DataRow row)
        {
            try
            {
                string Error = "";
                if (row.Table.Columns.Contains("CRCurrencyName") || row.Table.Columns.Contains("DRCurrencyName") || row.Table.Columns.Contains("CRFXRate") || row.Table.Columns.Contains("DRFXRate"))
                {
                    if (!row.Table.Columns.Contains("CRCurrencyName"))
                    {
                        Error = "Cr Currency Required!";
                    }
                    else if (!row.Table.Columns.Contains("DRCurrencyName"))
                    {
                        Error = "Dr Currency Required!";
                    }
                    else if (!row.Table.Columns.Contains("CRFXRate"))
                    {
                        Error = "Cr Fx Rate Required!";
                    }
                    else if (!row.Table.Columns.Contains("DRFXRate"))
                    {
                        Error = "Dr Fx Rate Required!";
                    }

                    if (!string.IsNullOrEmpty(Error))
                    {
                        row.RowError = Error;
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }
                }

                foreach (DataColumn dc in row.Table.Columns)
                {
                    if (row[dc].Equals(String.Empty))
                    {
                        row.SetColumnError(dc.Caption, "Select" + dc.Caption + "!");
                        row[PROPERTY_Validated] = "NotValidated";
                    }
                    else if (dc.ColumnName.Equals("CRFXRate"))
                    {
                        if (Convert.ToDecimal(row[dc]) <= 0)
                        {
                            row.SetColumnError(dc.Caption, "Cr Fx Rate should greater than zero!");
                            row[PROPERTY_Validated] = "NotValidated";
                        }
                    }
                    else if (dc.ColumnName.Equals("DRFXRate"))
                    {
                        if (Convert.ToDecimal(row[dc]) <= 0)
                        {
                            row.SetColumnError(dc.Caption, "Dr Fx Rate should greater than zero!");
                            row[PROPERTY_Validated] = "NotValidated";
                        }
                    }
                    else
                    {
                        row.SetColumnError(dc.Caption, "");
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

        int EntryId = 0;
        int iteration = 0;
        bool iterationcheck = false;
        private void SetForMultilegEntryImport(DataRow row)
        {
            try
            {
                int errorposition = 0;
                int countposition = 0;
                if (row[PROPERTY_Validated].Equals("InvalidData"))
                {
                    iteration += 1;
                    if (EntryId != Convert.ToInt32(row[HEADCOL_EntryID]))
                    {
                        EntryId = Convert.ToInt32(row[HEADCOL_EntryID]);
                        iterationcheck = false;
                    }
                    foreach (DataRow rowobject in NewDatatset.Tables[0].Rows)
                    {
                        if (EntryId == Convert.ToInt32(rowobject[HEADCOL_EntryID]))
                            countposition = countposition + 1;
                    }
                    if (countposition % 2 == 0)
                        errorposition = countposition / 2;
                    else
                        errorposition = countposition / 2 + 1;

                    if (iteration == errorposition)
                    {
                        row.RowError = "CR and DR amount should be equal! ";
                        row[PROPERTY_Validated] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
                        iterationcheck = true;
                    }
                    else
                    {
                        row[PROPERTY_Validated] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
                    }
                    if (iterationcheck)
                        iteration = 0;
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

        private void SetForDoubleEntryImport(DataRow row)
        {
            try
            {
                if (row["Validated"].Equals("InvalidData"))
                {
                    row.RowError = "CR and DR amount should be equal! ";
                    row[PROPERTY_Validated] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
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

        private void SetForCollateralInterest(DataRow row)
        {
            try
            {
                if (row["ValidationStatus"].Equals("InvalidData"))
                {
                    row.RowError = "Account ID is invalid ";
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.InvalidData.ToString());
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
        /// Validation FOR SM Import
        /// </summary>
        /// <param name="row"></param>
        private void SetForSMImportError(DataRow row)
        {
            try
            {
                if (row["SymbolExistsInSM"].Equals("Exists"))
                {
                    row.RowError = "Symbol already exists in Security Master, ";
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.AlreadyExists.ToString());
                }
                else if
                (Convert.ToInt32(row["AUECID"].ToString()).Equals(int.MinValue) || Convert.ToInt32(row["AUECID"].ToString()).Equals(0)
                || Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(int.MinValue) || Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(0)
                || Convert.ToInt32(row["ExchangeID"].ToString()).Equals(int.MinValue) || Convert.ToInt32(row["ExchangeID"].ToString()).Equals(0)
                || Convert.ToInt32(row["UnderLyingID"].ToString()).Equals(int.MinValue) || Convert.ToInt32(row["UnderLyingID"].ToString()).Equals(0)
                || Convert.ToInt32(row["CurrencyID"].ToString()).Equals(int.MinValue) || Convert.ToInt32(row["CurrencyID"].ToString()).Equals(0)
                || Convert.ToDouble(row["Multiplier"].ToString()).Equals(double.MinValue) || Convert.ToDouble(row["Multiplier"].ToString()).Equals(0) || Convert.ToDouble(row["Multiplier"].ToString()).Equals(0.0)
                || string.IsNullOrWhiteSpace(row["TickerSymbol"].ToString())
                || string.IsNullOrWhiteSpace(row["LongName"].ToString()))
                {
                    row.RowError = row.RowError + "invalid, please check AUEC, Ticker Symbol, Description, Multiplier, ";
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                }
                else if ((Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(2) || Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(4)) //Equity Option and Future Option
                    &&
                    (string.IsNullOrWhiteSpace(row["PutOrCall"].ToString())
                    || Convert.ToInt32(row["PutOrCall"].ToString()).Equals(2)
                    || Convert.ToDouble(row["StrikePrice"].ToString()).Equals(0.0)
                    || Convert.ToDouble(row["StrikePrice"].ToString()).Equals(0)
                    || row["UnderLyingSymbol"].Equals(String.Empty)
                    || string.IsNullOrWhiteSpace(row["ExpirationDate"].ToString())
                    || Convert.ToDateTime(row["ExpirationDate"].ToString()).Equals(DateTimeConstants.MinValue)))
                {
                    row.RowError = row.RowError + "invalid, please check Put/Call, Strike Price, Underlying Symbol, Expiration Date, ";
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                }
                else if ((Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(3)) //Future
                    &&
                    (row["UnderLyingSymbol"].Equals(String.Empty)
                    || row["ExpirationDate"].Equals(String.Empty)
                    || row["ExpirationDate"].Equals(DateTimeConstants.MinValue)))
                {
                    row.RowError = row.RowError + "invalid, please check Underlying Symbol,Expiration Date, ";
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                }
                else if (Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(5) //FX and FxForward
                    || Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(11))
                {
                    if (Convert.ToInt32(row[OrderFields.PROPERTY_LEADCURRENCYID].ToString()).Equals(0)
                    || Convert.ToInt32(row[OrderFields.PROPERTY_VSCURRENCYID].ToString()).Equals(0)
                    || Convert.ToInt32(row[OrderFields.PROPERTY_LEADCURRENCYID].ToString()) == Convert.ToInt32(row[OrderFields.PROPERTY_VSCURRENCYID].ToString())
                    || Convert.ToInt32(row[OrderFields.PROPERTY_LEADCURRENCYID].ToString()) == int.MinValue
                    || Convert.ToInt32(row[OrderFields.PROPERTY_VSCURRENCYID].ToString()) == int.MinValue
                    || Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(11) && row["ExpirationDate"].Equals(DateTimeConstants.MinValue))
                    {
                        row.RowError = row.RowError + "invalid, please check Expiration Date, LeadCurrency and VsCurrency";
                        row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                    }
                    else if (!CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(Convert.ToInt32(row[OrderFields.PROPERTY_LEADCURRENCYID])))
                    {
                        row.RowError = row.RowError + " " + OrderFields.PROPERTY_LEADCURRENCYID + " " + row[OrderFields.PROPERTY_LEADCURRENCYID].ToString().Trim() + " Undefined, ";
                        row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                    }
                    else if (!CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(Convert.ToInt32(row[OrderFields.PROPERTY_VSCURRENCYID])))
                    {
                        row.RowError = row.RowError + " " + OrderFields.PROPERTY_VSCURRENCYID + " " + row[OrderFields.PROPERTY_VSCURRENCYID].ToString().Trim() + " Undefined, ";
                        row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                    }
                    else if (!row["IsNDF"].ToString().Equals("FALSE") && !row["IsNDF"].ToString().Equals("TRUE"))
                    {
                        row.RowError = row.RowError + " IsNDF " + row["IsNDF"].ToString().Trim() + " Undefined, ";
                        row["IsNDF"] = false;
                    }
                }
                else if (Convert.ToInt32(row[OrderFields.PROPERTY_ASSET_ID].ToString()).Equals(8)) //Fixed Income
                {
                    if (string.IsNullOrWhiteSpace(row["MaturityDate"].ToString())
                    || row["MaturityDate"].Equals(DateTimeConstants.MinValue)
                    || string.IsNullOrWhiteSpace(row["FirstCouponDate"].ToString())
                    || string.IsNullOrWhiteSpace(row["ExpirationDate"].ToString())
                    || row["ExpirationDate"].Equals(DateTimeConstants.MinValue)
                    || string.IsNullOrWhiteSpace(row["UnderLyingSymbol"].ToString())
                    || (Convert.ToInt32(row["DaysToSettlement"].ToString()).Equals(int.MinValue))
                    || (Convert.ToInt32(row["DaysToSettlement"].ToString()) <= 0))
                    {
                        row.RowError = row.RowError + "invalid, please check UnderLying Symbol, Expiration Date, Maturity Date,First Coupon Date or Days To Settlement, ";
                        row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                    }
                    else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.AccrualBasis), Convert.ToInt32(row["AccrualBasisID"])))
                    {
                        row.RowError = row.RowError + " " + "AccrualBasisID" + " " + row["AccrualBasisID"].ToString().Trim() + " Undefined, ";
                        row["AccrualBasisID"] = 0;
                    }
                    else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.CouponFrequency), Convert.ToInt32(row["CouponFrequencyID"])))
                    {
                        row.RowError = row.RowError + " " + "CouponFrequencyID" + " " + row["CouponFrequencyID"].ToString().Trim() + " Undefined, ";
                        row["CouponFrequencyID"] = 0;
                    }
                    else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.SecurityType), Convert.ToInt32(row["BondTypeID"])))
                    {
                        row.RowError = row.RowError + " " + "BondTypeID" + " " + row["BondTypeID"].ToString().Trim() + " Undefined, ";
                        row["BondTypeID"] = 0;
                    }
                    else if ((row["IsZero"].ToString().ToLower().Equals("false")
                        && (Convert.ToDouble(row["Coupon"].ToString()).Equals(0)
                        || Convert.ToDouble(row["Coupon"].ToString()).Equals(0.0))))
                    {
                        row.RowError = row.RowError + "invalid, please check Coupon Rate, ";
                        row[ApplicationConstants.CONST_VALIDATION_STATUS] = Prana.Utilities.UI.MiscUtilities.EnumHelper.GetFormatedText(ApplicationConstants.ValidationStatus.MissingData.ToString());
                    }
                    else if (!row["IsZERO"].ToString().Equals("TRUE") && !row["IsZERO"].ToString().Equals("FALSE"))
                    {
                        row.RowError = row.RowError + " IsZERO " + row["IsZERO"].ToString().Trim() + " Undefined, ";
                    }
                }

                if (row.Table.Columns.Contains("UDAAssetClass") && row["UDAAssetClass"].ToString().Contains(","))
                {
                    row.RowError = row.RowError + " UDAAssetClass " + row["UDAAssetClass"].ToString().Trim() + " Undefined, ";
                }
                if (row.Table.Columns.Contains("UDASector") && row["UDASector"].ToString().Contains(","))
                {
                    row.RowError = row.RowError + " UDASector " + row["UDASector"].ToString().Trim() + " Undefined, ";
                }
                if (row.Table.Columns.Contains("UDASubSector") && row["UDASubSector"].ToString().Contains(","))
                {
                    row.RowError = row.RowError + " UDASubSector " + row["UDASubSector"].ToString().Trim() + " Undefined, ";
                }
                if (row.Table.Columns.Contains("UDASecurityType") && row["UDASecurityType"].ToString().Contains(","))
                {
                    row.RowError = row.RowError + " UDASecurityType " + row["UDASecurityType"].ToString().Trim() + " Undefined, ";
                }
                if (row.Table.Columns.Contains("UDACountry") && row["UDACountry"].ToString().Contains(","))
                {
                    row.RowError = row.RowError + " UDACountry " + row["UDACountry"].ToString().Trim() + " Undefined, ";
                }
                if (string.IsNullOrWhiteSpace(row.RowError))
                {
                    row[ApplicationConstants.CONST_VALIDATION_STATUS] = ApplicationConstants.ValidationStatus.Validated.ToString();
                    row.RowError = "";
                }
                row.RowError = row.RowError.Trim();
                if (!string.IsNullOrWhiteSpace(row.RowError) && row.RowError[row.RowError.Length - 1] == ',')
                {
                    row.RowError = row.RowError.Substring(0, row.RowError.Length - 1) + ".";
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

        private void grdImportData_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //Narendra Kumar Jangir, Sept 11 2013
                //Following two import types are removed

                //if (_positionType.Equals("SubAccount Cash") || _positionType.Equals(_importTypeCashTransactions))
                //{
                //    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                //    SetError(row);
                //}
                if (_positionType.Equals(_importTypeMultilegJournalImport))
                {
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                    SetErrorForMultileg(row);
                    SetForMultilegEntryImport(row);
                }
                if (_positionType.Equals(_importTypeDoubleEntryCash))
                {
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                    SetError(row);
                    SetForDoubleEntryImport(row);
                    SetNAVLockDateValidationError(row);
                }
                else if (_positionType.Equals(_importTypeSecMasterInsert))
                {
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                    SetForSMImportError(row);
                }
                else if (_positionType.Equals(_importTypeColIntMasterInsert))
                {
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                    SetForCollateralInterest(row);
                }
                else if (_positionType.Equals(_importTypeSecMasterUpdateData))
                {
                    SecMasterUpdateDataByImportUI updateDataByImportUIobj = (SecMasterUpdateDataByImportUI)(e.Row.ListObject);
                    BindDynamicUDAValue(_dynamicUDACache, updateDataByImportUIobj, e.Row);
                    SetForSMUpdateError(updateDataByImportUIobj);
                    e.Row.DataErrorInfo.RowError = updateDataByImportUIobj.ValidationError;
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
        /// Validation for SM Update
        /// </summary>
        /// <param name="updateDataByImportUIobj"></param>
        private void SetForSMUpdateError(SecMasterUpdateDataByImportUI updateDataByImportUIobj)
        {
            try
            {
                if (updateDataByImportUIobj.Validated.Equals("Validated"))
                {
                    if (updateDataByImportUIobj.Multiplier.Equals(double.MinValue) || updateDataByImportUIobj.Multiplier.Equals(0) || updateDataByImportUIobj.Multiplier.Equals(0.0)
                       || string.IsNullOrWhiteSpace(updateDataByImportUIobj.LongName))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check Description, Multiplier, ";
                    }
                    else if ((updateDataByImportUIobj.AssetID.Equals(2) || updateDataByImportUIobj.AssetID.Equals(4)) //Equity Option and Future Option
                           &&
                           (string.IsNullOrWhiteSpace(updateDataByImportUIobj.PutOrCall.ToString())
                           || updateDataByImportUIobj.PutOrCall.Equals(2)
                           || updateDataByImportUIobj.StrikePrice.Equals(0.0)
                           || updateDataByImportUIobj.StrikePrice.Equals(0)
                           || string.IsNullOrWhiteSpace(updateDataByImportUIobj.UnderLyingSymbol)
                           || string.IsNullOrWhiteSpace(updateDataByImportUIobj.ExpirationDate.ToString())
                           || updateDataByImportUIobj.ExpirationDate.Equals(DateTimeConstants.MinValue)))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check Put/Call, Strike Price, Underlying Symbol, Expiration Date, ";
                    }
                    else if (updateDataByImportUIobj.AssetID.Equals(3) //Future
                        &&
                        (string.IsNullOrWhiteSpace(updateDataByImportUIobj.UnderLyingSymbol)
                        || string.IsNullOrWhiteSpace(updateDataByImportUIobj.ExpirationDate.ToString())
                        || updateDataByImportUIobj.ExpirationDate.Equals(DateTimeConstants.MinValue)))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check Underlying Symbol, Expiration Date, ";
                    }
                    else if (updateDataByImportUIobj.AssetID.Equals(5) //FX and FxForward
                           || updateDataByImportUIobj.AssetID.Equals(11))
                    {
                        if (updateDataByImportUIobj.LeadCurrencyID.Equals(0)
                        || updateDataByImportUIobj.VsCurrencyID.Equals(0)
                        || updateDataByImportUIobj.LeadCurrencyID == updateDataByImportUIobj.VsCurrencyID
                        || updateDataByImportUIobj.LeadCurrencyID == int.MinValue
                        || updateDataByImportUIobj.VsCurrencyID == int.MinValue
                        || updateDataByImportUIobj.AssetID.Equals(11) && updateDataByImportUIobj.ExpirationDate.Equals(DateTimeConstants.MinValue))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check Expiration Date, LeadCurrency and VsCurrency";
                        }
                        else if (!CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(updateDataByImportUIobj.LeadCurrencyID))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " " + OrderFields.PROPERTY_LEADCURRENCYID + " " + updateDataByImportUIobj.LeadCurrencyID + " Undefined, ";
                        }
                        else if (!CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(updateDataByImportUIobj.VsCurrencyID))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " " + OrderFields.PROPERTY_VSCURRENCYID + " " + updateDataByImportUIobj.VsCurrencyID + " Undefined, ";
                        }
                    }
                    else if (updateDataByImportUIobj.AssetID.Equals(8)) //Fixed Income
                    {
                        if (string.IsNullOrWhiteSpace(updateDataByImportUIobj.MaturityDay.ToString())
                        || updateDataByImportUIobj.MaturityDay.Equals(DateTimeConstants.MinValue)
                        || string.IsNullOrWhiteSpace(updateDataByImportUIobj.FirstCouponDate.ToString())
                        || string.IsNullOrWhiteSpace(updateDataByImportUIobj.ExpirationDate.ToString())
                        || updateDataByImportUIobj.ExpirationDate.Equals(DateTimeConstants.MinValue)
                        || string.IsNullOrWhiteSpace(updateDataByImportUIobj.UnderLyingSymbol)
                        || updateDataByImportUIobj.DaysToSettlement.Equals(int.MinValue)
                        || updateDataByImportUIobj.DaysToSettlement <= 0)
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check UnderLying Symbol, Expiration Date, Maturity Date,First Coupon Date or Days To Settlement, ";
                        }
                        else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.AccrualBasis), updateDataByImportUIobj.AccrualBasisID))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " " + "AccrualBasisID" + " " + updateDataByImportUIobj.AccrualBasisID + " Undefined, ";
                        }
                        else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.CouponFrequency), updateDataByImportUIobj.CouponFrequencyID))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " " + "CouponFrequencyID" + " " + updateDataByImportUIobj.CouponFrequencyID + " Undefined, ";
                        }
                        else if (!Enum.IsDefined(typeof(BusinessObjects.AppConstants.SecurityType), updateDataByImportUIobj.SecurityTypeID))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " " + "BondTypeID" + " " + updateDataByImportUIobj.SecurityTypeID + " Undefined, ";
                        }
                        else if ((updateDataByImportUIobj.IsZero.Equals("false")
                            && (updateDataByImportUIobj.Coupon.Equals(0)
                            || updateDataByImportUIobj.Coupon.Equals(0.0))))
                        {
                            updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + "invalid, please check Coupon Rate, ";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.UDAAssetClass) && updateDataByImportUIobj.UDAAssetClass.Contains(","))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " UDAAssetClass " + updateDataByImportUIobj.UDAAssetClass.Trim() + " Undefined, ";
                    }
                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.UDASector) && updateDataByImportUIobj.UDASector.Contains(","))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " UDASector " + updateDataByImportUIobj.UDASector.Trim() + " Undefined, ";
                    }
                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.UDASubSector) && updateDataByImportUIobj.UDASubSector.Contains(","))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " UDASubSector " + updateDataByImportUIobj.UDASubSector.Trim() + " Undefined, ";
                    }
                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.UDASecurityType) && updateDataByImportUIobj.UDASecurityType.Contains(","))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " UDASecurityType " + updateDataByImportUIobj.UDASecurityType.Trim() + " Undefined, ";
                    }
                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.UDACountry) && updateDataByImportUIobj.UDACountry.Contains(","))
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError + " UDACountry " + updateDataByImportUIobj.UDACountry.Trim() + " Undefined, ";
                    }

                    updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError.Trim();
                    if (!string.IsNullOrWhiteSpace(updateDataByImportUIobj.ValidationError) && updateDataByImportUIobj.ValidationError[updateDataByImportUIobj.ValidationError.Length - 1] == ',')
                    {
                        updateDataByImportUIobj.ValidationError = updateDataByImportUIobj.ValidationError.Substring(0, updateDataByImportUIobj.ValidationError.Length - 1) + ".";
                    }
                    if (!String.IsNullOrWhiteSpace(updateDataByImportUIobj.ValidationError))
                    {
                        updateDataByImportUIobj.Validated = "NotValidated";
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

        private void BindDynamicUDAValue(SerializableDictionary<string, DynamicUDA> dynamicUDAcache, SecMasterUpdateDataByImportUI secMasterUpdateDataByImportUI, UltraGridRow ultraGridRow)
        {
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (grdImportData.DisplayLayout.Bands[0].Columns.Exists(uda))
                        {
                            if (secMasterUpdateDataByImportUI.DynamicUDA.ContainsKey(uda) && secMasterUpdateDataByImportUI.DynamicUDA[uda].ToString() != string.Empty)
                                ultraGridRow.Cells[uda].Value = secMasterUpdateDataByImportUI.DynamicUDA[uda].ToString();
                            else
                                ultraGridRow.Cells[uda].Value = dynamicUDAcache[uda].DefaultValue;
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

        private void SetForASchemeError(DataRow row)
        {
            try
            {
                if (row["Symbol"].Equals(String.Empty))
                {
                    row.RowError = "invalid, please Symbol";
                    row[PROPERTY_Validated] = "NotValidated";
                }
                else
                {
                    row[PROPERTY_Validated] = "Validated";
                    row.RowError = "";
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

        private void grdImportData_FilterRow(object sender, FilterRowEventArgs e)
        {
            if (e.RowFilteredOut)
            {
                if (_positionType.Equals("SubAccount Cash"))
                {
                    if (ds == null)
                    {
                        DataSet dsNew = ((DataSet)grdImportData.DataSource);
                        ds = dsNew.Clone();
                    }
                    e.Row.Cells["checkBox"].Value = false;
                    DataRowView drv = (DataRowView)e.Row.ListObject;
                    DataRow dr = drv.Row;
                    if (ds.Tables["PositionMaster"].Rows.Contains(dr["PRIMARY_KEY"]))
                    {
                        ds.Tables[0].DefaultView.Sort = "PRIMARY_KEY";

                        int i = ds.Tables["PositionMaster"].DefaultView.Find(dr["PRIMARY_KEY"]);
                        ds.Tables["PositionMaster"].Rows.RemoveAt(i);
                    }
                }
            }
        }

        ValueList subAccount = new ValueList();

        private void grdImportData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (_positionType == _importTypeDoubleEntryCash)
            {
                if (e.Cell.Column.Key == "checkBox")
                {
                    if (ds == null)
                    {
                        DataSet dsNew = ((DataSet)grdImportData.DataSource);
                        ds = dsNew.Clone();
                    }

                    if (e.Cell.Value != null && e.Cell.Text.ToString().ToLower().Equals("true"))
                    {
                        DataRow dr = ds.Tables["PositionMaster"].NewRow();
                        DataRowView drView = (DataRowView)e.Cell.Row.ListObject;

                        if (!ds.Tables["PositionMaster"].Rows.Contains(dr["PRIMARY_KEY"]))
                        {

                            dr.ItemArray = drView.Row.ItemArray;
                            ds.Tables["PositionMaster"].Rows.Add(dr);
                        }
                    }
                    else if (e.Cell.Value != null && e.Cell.Text.ToString().ToLower() == "false")
                    {
                        DataRowView drv = (DataRowView)e.Cell.Row.ListObject;
                        DataRow dr = drv.Row;
                        if (ds.Tables["PositionMaster"].Rows.Contains(dr["PRIMARY_KEY"]))
                        {
                            ds.Tables[0].DefaultView.Sort = "PRIMARY_KEY";

                            int i = ds.Tables["PositionMaster"].DefaultView.Find(dr["PRIMARY_KEY"]);
                            ds.Tables["PositionMaster"].Rows.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    if (e.Cell.Value != null && e.Cell.Row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                    {
                        DataRowView drv = (DataRowView)e.Cell.Row.ListObject;
                        DataRow dr = drv.Row;
                        if (ds.Tables["PositionMaster"].Rows.Contains(dr["PRIMARY_KEY"]))
                        {
                            ds.Tables[0].DefaultView.Sort = "PRIMARY_KEY";

                            int i = ds.Tables["PositionMaster"].DefaultView.Find(dr["PRIMARY_KEY"]);

                            ds.Tables["PositionMaster"].Rows.RemoveAt(i);
                        }
                        DataRow drNew = ds.Tables["PositionMaster"].NewRow();
                        DataRowView drView = (DataRowView)e.Cell.Row.ListObject;

                        if (!ds.Tables["PositionMaster"].Rows.Contains(drNew["PRIMARY_KEY"]))
                        {
                            drNew.ItemArray = drView.Row.ItemArray;
                            ds.Tables["PositionMaster"].Rows.Add(drNew);
                        }
                    }
                }
            }
            if (_positionType == _importTypeMultilegJournalImport)
            {
                if (e.Cell.Column.Key == HEADCOL_checkBox)
                {
                    if (ds == null)
                    {
                        DataSet dsNew = ((DataSet)grdImportData.DataSource);
                        ds = dsNew.Clone();
                    }

                    if (e.Cell.Value != null && e.Cell.Text.ToString().ToLower().Equals("true"))
                    {
                        DataRow dr = ds.Tables["PositionMaster"].NewRow();
                        DataRowView drView = (DataRowView)e.Cell.Row.ListObject;

                        if (!ds.Tables["PositionMaster"].Rows.Contains(dr[HEADCOL_PRIMARYKEY]))
                        {

                            dr.ItemArray = drView.Row.ItemArray;
                            if (!ds.Tables["PositionMaster"].Rows.Contains(dr[HEADCOL_PRIMARYKEY]))
                                ds.Tables["PositionMaster"].Rows.Add(dr);
                        }
                    }
                    else if (e.Cell.Value != null && e.Cell.Text.ToString().ToLower() == "false")
                    {
                        DataRowView drv = (DataRowView)e.Cell.Row.ListObject;
                        DataRow dr = drv.Row;
                        DataTable NewDataTable = null;
                        DataSet NewDataset = null;
                        NewDataTable = ds.Tables["PositionMaster"].Clone();
                        if (ds.Tables["PositionMaster"].Rows.Contains(dr[HEADCOL_PRIMARYKEY]))
                        {
                            int EntryID = Convert.ToInt32(dr[HEADCOL_EntryID]);
                            for (int i = 0; i < ds.Tables["PositionMaster"].Rows.Count; i++)
                            {
                                if (Convert.ToInt32(ds.Tables["PositionMaster"].Rows[i][HEADCOL_EntryID]) != EntryID)
                                    NewDataTable.ImportRow(ds.Tables["PositionMaster"].Rows[i]);
                            }
                            NewDataset = new DataSet();
                            NewDataset.Tables.Add(NewDataTable);
                            ds = NewDataset;
                        }
                    }
                }
            }
        }

        private ListEventAargs GetStrings()
        {
            return l;
        }

        #region ILaunchForm Members
        public event EventHandler LaunchForm;
        #endregion

        private void btnMapping_Click(object sender, EventArgs e)
        {
            if (LaunchForm != null)
            {
                LaunchForm(this, GetStrings());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (refersh != null)
            {
                refersh(null, null);
            }
        }

        public event EventHandler refersh;

        /// <summary>
        /// SymbolLookup click handle to Get All un approved SM on SM UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolLookup_Click(object sender, EventArgs e)
        {
            try
            {
                ListEventAargs args = new ListEventAargs();
                Dictionary<String, String> argDict = new Dictionary<string, string>();
                argDict.Add("Action", SecMasterConstants.SecurityActions.APPROVE.ToString());
                argDict.Add(ApplicationConstants.CONST_IS_SECURITY_APPROVED, "false");
                args.argsObject = argDict;
                args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP);
                if (LaunchForm != null)
                {
                    LaunchForm(this, args);
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
        /// Add new symbol manually from SM UI
        ///created by omshiv, Nov 2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSymbolManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdImportData.ActiveRow != null)
                {
                    PositionMaster positionRow = grdImportData.ActiveRow.ListObject as PositionMaster;
                    if (positionRow != null)
                    {

                        ListEventAargs args = new ListEventAargs();
                        Dictionary<String, String> argDict = new Dictionary<string, string>();
                        SecMasterUIObj secMasterUI = new SecMasterUIObj();

                        secMasterUI.BloombergSymbol = positionRow.Bloomberg;
                        secMasterUI.TickerSymbol = positionRow.Symbol.Trim();
                        secMasterUI.SedolSymbol = positionRow.SEDOL;
                        if (positionRow.Multiplier != 0)
                            secMasterUI.Multiplier = positionRow.Multiplier;
                        secMasterUI.LongName = positionRow.Description;
                        secMasterUI.IDCOOptionSymbol = positionRow.IDCOOptionSymbol;
                        secMasterUI.ISINSymbol = positionRow.ISIN;

                        // Added secMaster UI object to args by this we can send sec master values to symbol lookup UI. - omshiv Nov, 2013
                        argDict.Add("SecMaster", binaryFormatter.Serialize(secMasterUI));
                        argDict.Add("Action", SecMasterConstants.SecurityActions.ADD.ToString());
                        args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP);
                        args.argsObject = argDict;
                        if (LaunchForm != null)
                        {
                            LaunchForm(this, args);
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select trade row to add symbol!";
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
        /// Checking for Add symbol context menu shwn to user or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMnuPositionImprt_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (grdImportData.ActiveRow != null)
                {
                    PositionMaster positionRow = grdImportData.ActiveRow.ListObject as PositionMaster;
                    if (positionRow != null)
                    {
                        if (!positionRow.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.NotExists.ToString()))
                        {
                            addSymbolManuallyToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            addSymbolManuallyToolStripMenuItem.Enabled = true;
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select trade row to add symbol!";
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
        /// 
        ///created by omshiv, Nov 2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String impotTypetext = this.Text.Replace(" ", "_");
                String filePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\ImportUI_" + impotTypetext + ".xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                grdImportData.DisplayLayout.SaveAsXml(filePath, PropertyCategories.All);
                toolStripStatusLabel1.Text = "Layout has been Saved!";
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
        /// 
        ///created by omshiv, Nov 2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String impotTypetext = this.Text.Replace(" ", "_");
                String filePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\ImportUI_" + impotTypetext + ".xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
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
        ///  handle click on export Data ToolStripMenuItem 
        ///created by omshiv, Nov 2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Saving to Excel file. This launches the Save dialog for the user to select the Save Path
                if (grdImportData.Rows.Count > 0)
                {
                    CreateExcel(ExcelUtilities.FindSavePathForExcel());
                }
                else
                    toolStripStatusLabel1.Text = "No data to Export";
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
            finally
            {
                //Any cleanup code
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// Create Excel on specified path
        ///created by omshiv, Nov 2013
        /// </summary>
        /// <param name="filepath"></param>
        private void CreateExcel(String filepath)
        {
            try
            {
                if (filepath != null)
                {
                    gridExcelExporter.Export(grdImportData, filepath);
                    toolStripStatusLabel1.Text = "Grid data successfully downloaded to " + filepath;
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

        private void grdImportData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdImportData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdImportData);
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
        private void grdImportData_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        /// <summary>
        /// Set NAVLockDate Validation Error on given row
        /// </summary>
        /// <param name="row"></param>
        private void SetNAVLockDateValidationError(DataRow row)
        {
            try
            {
                if (DateTime.TryParse(row[HEADCOL_DATE].ToString(), out DateTime date))
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(date))
                    {
                        string rowError = string.IsNullOrEmpty(row.RowError) ? string.Empty : row.RowError + "\n";                    
                        row[PROPERTY_Validated] = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                        row.RowError = rowError + "The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString() + "). Please reach out to your Support Team for further assistance.";
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
                exporter.Export(grdImportData, exportFilePath);
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
    }

    public class MyMergedCellEvaluator : IMergedCellEvaluator
    {
        public bool ShouldCellsBeMerged(UltraGridRow row1, UltraGridRow row2, UltraGridColumn column)
        {
            try
            {
                return Convert.ToInt32(row1.Cells["EntryID"].Value) == Convert.ToInt32(row2.Cells["EntryID"].Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return false;
        }
    }
}
