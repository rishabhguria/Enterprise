using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class ctrlCloseTradefromAllocation : UserControl
    {
        public event EventHandler SaveLayout;

        public delegate void SetStatusMsg(string message);
        public event EventHandler<EventArgs<string>> SetStatusMessage;

        CheckBoxOnHeader_CreationFilter _headerCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter();
        private const string HEADCOL_CheckBox = "checkBox";

        private const string COL_OrderSideTagValue = "OrderSideTagValue";

        private const string FILTER_ALL = "All";
        private const string SIDE_BUYTOCLOSE = "BUYTOCLOSE";
        string _currencyColumnFormat = "0.0000";
        string _quantityColumnFormat = "0.00000000";

        ClosingPreferences _closingPreferences = null;

        //Narendra Kumar Jangir, Oct 02 2013
        //http://jira.nirvanasolutions.com:8080/browse/SS-80
        //declaration and initialization of global closing algorithm
        Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm _closingAlgo = Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm.MANUAL;

        //declaration and initialization of global closing field
        Prana.BusinessObjects.PostTradeEnums.ClosingField _closingField = Prana.BusinessObjects.PostTradeEnums.ClosingField.Default;

        bool _isCloseTradeInitialized = false;

        public delegate void DisableEnableFormHandler(string message, bool Flag, bool TimerFlag);
        public event EventHandler<EventArgs<string, bool, bool>> DisableEnableParentForm;

        //#region Open Trades columns

        //private const string COLUMN_LEVEL1NAME = "Level1Name";
        //private const string COLUMN_AUECLOCALDATE = "AUECLocalDate";
        //private const string COLUMN_ORIGPURCHASEDATE = "OriginalPurchaseDate";
        //private const string COLUMN_TAXLOTQTY = "TaxLotQty";
        //private const string COLUMN_AVGPRICE = "AvgPrice";
        //private const string COLUMN_LEVEL2NAME = "Level2Name";
        //private const string COLUMN_ORDERSIDE = "OrderSide";
        //private const string COLUMN_QUANTITYTOCLOSE = "TaxLotQtyToClose";

        //private const string CAPTION_LEVEL1NAME = "Account";
        //private const string CAPTION_AUECLOCALDATE = "Trade Date";
        //private const string CAPTION_ORIGPURCHASEDATE = "Original Purchase Date";
        //private const string CAPTION_TAXLOTQTY = "Quantity";
        //private const string CAPTION_AVGPRICE = "Unit Cost";
        //private const string CAPTION_LEVEL2NAME = "Strategy";
        //private const string CAPTION_ORDERSIDE = "Side";
        //private const string CAPTION_QUANTITYTOCLOSE = "Close Quantity";
        //#endregion

        //#region Grid Columns for Close Trades

        //const string COL_ID = "ID";

        //const string COL_StartDate = "StartDate";
        //const string COL_LastActivityDate = "LastActivityDate";
        //const string COL_PositionTag = "PositionalTag";
        //const string COL_ClosingTag = "ClosingPositionTag";
        //const string COL_AccountValue = "AccountValue";
        //const string COL_PNLPOSITION = "PNLWhenTaxLotsPopulated";
        //const string COL_PNL = "CostBasisRealizedPNL";
        //const string COL_StartTaxLotID = "StartTaxLotID";
        //const string COL_PositionStartQuantity = "PositionStartQty";
        //const string COL_AccountID = "AccountID";
        //const string COL_Multiplier = "Multiplier";
        //const string COL_AUECID = "AUECID";
        //const string COL_RealizedPNL = "CostBasisRealizedPNL";
        //const string COL_RecordType = "RecordType";
        //const string COL_Status = "Status";
        //const string COL_EndDate = "EndDate";
        //const string COL_Description = "Description";
        //const string COL_Strategy = "Strategy";
        //const string COL_StrategyID = "StrategyID";
        //const string COL_MarkPriceForMonth = "MarkPriceForMonth";
        //const string COL_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
        //const string COL_NotionalValue = "NotionalValue";
        //const string COL_AvgPriceRealizedPL = "AvgPriceRealizedPL";
        //const string COL_SymbolAveragePrice = "SymbolAveragePrice";
        //const string COL_AUECLocalCloseDate = "AUECLocalCloseDate";
        //const string COL_CloseDate = "TimeOfSaveUTC";

        //const string COL_GeneratedTaxlotSymbol = "GeneratedTaxlotSymbol";
        //const string COL_Exchange = "Exchange";
        //const string COL_OpenQty = "OpenQty";
        //const string COL_CurrencyID = "CurrencyID";
        //const string COL_Currency = "Currency";
        ////const string COL_UnderlyingName = "Underlying";
        //const string COL_ClosingID = "ClosingID";

        //const string PositionalSide_Long = "Long";
        //const string PositionalSide_Short = "Short";
        //const string COL_PositionSide = "Side";
        //const string COL_TradeDatePosition = "TradeDate";

        //const string COL_AllocationID = "TaxLotID";
        //const string COL_TradeDate = "AUECLocalDate";
        //const string COL_ProcessDate = "ProcessDate";
        //const string COL_OriginalPurchaseDate = "OriginalPurchaseDate";
        //const string COL_ClosingTradeDate = "ClosingTradeDate";
        //const string COL_TradeDateUTC = "TradeDateUTC";
        //const string COL_Side = "OrderSide";
        //const string COL_ClosingSide = "ClosingSide";
        //const string COL_Symbol = "Symbol";
        //const string COL_SecurityFullName = "CompanyName";
        //const string COL_OpenQuantity = "TaxLotQty";
        //const string COL_ClosedQty = "ClosedQty";
        //const string COL_AveragePrice = "AvgPrice";
        //const string COL_OpenAveragePrice = "OpenAveragePrice";
        //const string COL_ClosedAveragePrice = "ClosedAveragePrice";
        //const string COL_Account = "Level1Name";
        //const string COL_SideID = "OrderSideTagValue";
        //const string COL_IsPosition = "IsPosition";
        //const string COL_AUEC = "AUECID";
        //const string COL_PositionTaxlotID = "PositionTaxlotID";
        //const string COL_OpenCommission = "OpenTotalCommissionandFees";
        //const string COL_PositionCommission = "PositionTotalCommissionandFees";
        //const string COL_OpenFees = "OtherBrokerOpenFees";
        //const string COL_PositionFees = "PositionOtherBrokerFees";
        //const string COL_ClosedCommission = "ClosedTotalCommissionandFees";
        //const string COL_ClosingTotalCommissionandFees = "ClosingTotalCommissionandFees";
        //const string COL_NetNotionalValue = "NetNotionalValue";
        //const string COL_StrategyValue = "Level2Name";
        //const string COL_SettledQty = "SettledQty";
        //const string COL_CashSettledPrice = "CashSettledPrice";
        //const string COL_ClosingMode = "ClosingMode";
        //const string COL_IsExpired_Settled = "IsExpired_Settled";
        //const string COL_AssetCategoryValue = "AssetCategoryValue";
        //const string COL_ExpiryDate = "ExpirationDate";
        //const string COL_Underlying = "UnderlyingName";
        //const string COL_UnitCost = "UnitCost";
        //const string COL_PositionTagValue = "PositionTag";
        //const string COL_IsSwap = "ISSwap";
        //const string COL_LotId = "LotId";
        //const string COL_ExternalTransId = "ExternalTransId";

        //const string CAP_TaxlotId = "Taxlot ID";
        //const string CAP_Account = "Account";
        //const string CAP_Strategy = "Strategy";
        //const string CAP_TradeDate = "Trade Date";
        //const string CAP_ProcessDate = "Process Date";
        //const string CAP_OriginalPurchaseDate = "OriginalPurchase Date";
        //const string CAP_PositionType = "Position Type";
        //const string CAP_ClosedPositionType = "Closing Position Type";
        //const string CAP_Symbol = "Symbol";
        //const string CAP_StartQty = "Start Quantity";
        //const string CAP_CloseQty = "Qty Closed";
        //const string CAP_OpenQty = "Open Qty";
        //const string CAP_AvgPrice = "Opening Price";
        //const string CAP_AvgClosingPrice = "Closing Price";
        //const string CAP_Commission = "Total Fees and Commission";
        //const string CAP_Fees = "OtherBrokerFees";
        //const string CAP_OtherFees = "Other Fees";
        //const string CAP_RealizedPNL = "Realized PNL(C.B.)";
        //const string CAP_AUECCloseDt = "AUEC Close Date";
        //const string CAP_CloseDt = "Closing Date";
        //const string CAP_Side = "Side";
        //const string CAP_OpeningSide = "Opening Side";
        //const string CAP_ClosingSide = "Closing Side";
        //const string CAP_NetNotional = "Net Notional";
        //const string CAP_SecurityFullName = "Security Name";
        //const string CAP_ClosingMode = "Closing Mode";
        //const string CAP_SettlementPrice = "Settlement Price";
        //const string CAP_AssetCategory = "Asset";
        //const string CAP_Exchange = "Exchange";
        //const string CAP_PositionCommission = "Opening Fees & Commission";
        //const string CAP_Currency = "Currency";
        //const string CAP_Underlying = "Underlying";
        //const string CAP_ClosingID = "ClosingID";
        //const string CAP_IsSwapped = "IsSwapped";

        //const string _currencyColumnFormat = "#,#.00";

        //#endregion

        Dictionary<string, TaxLot> _dictSellTaxLotsAndPositions = new Dictionary<string, TaxLot>();

        //dictionary _dictClosingQuantityAccountAndStrategyWise contains closing quantity for each account+strategy
        Dictionary<string, double> _dictClosingQuantityAccountAndStrategyWise = new Dictionary<string, double>();

        Dictionary<string, double> _dictAccountWiseQtyToClose = new Dictionary<string, double>();

        BackgroundWorker _bgFetchData = new BackgroundWorker();

        public ctrlCloseTradefromAllocation()
        {
            InitializeComponent();

            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                //setup for fetch data
                //_bgFetchData = new BackgroundWorker();
                _bgFetchData.DoWork += new DoWorkEventHandler(_bgFetchData_DoWork);
                _bgFetchData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgFetchData_RunWorkerCompleted);
                _bgFetchData.WorkerSupportsCancellation = true;
            }
        }

        ProxyBase<IClosingServices> _closingServices = null;
        ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IClosingServices> ClosingServices
        {
            set
            {
                _closingServices = value;
            }
        }
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        private List<TaxLot> GetClosingTaxlotsList()
        {
            return new List<TaxLot>(_dictSellTaxLotsAndPositions.Values);
        }

        private void SetClosingTaxlotsList(List<TaxLot> closingTaxLots)
        {
            _dictSellTaxLotsAndPositions.Clear();
            foreach (TaxLot taxLot in closingTaxLots)
            {
                if (!_dictSellTaxLotsAndPositions.ContainsKey(taxLot.TaxLotID))
                {
                    _dictSellTaxLotsAndPositions.Add(taxLot.TaxLotID, taxLot);
                }
            }
        }
        /// <summary>
        /// This dictionay populated at close order ui opening and updated when closing and unwinding done
        /// </summary>
        /// <param name="closingTaxLots"></param>
        public void UpdateSellOpenQtyAccountAndStrategyWise()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            UpdateSellOpenQtyAccountAndStrategyWise();

                        }));
                    }
                    else
                    {
                        List<TaxLot> closingTaxLots = GetClosingTaxlotsList();
                        _dictClosingQuantityAccountAndStrategyWise.Clear();
                        foreach (TaxLot taxLot in closingTaxLots)
                        {
                            //key for this dictionay will be account+~+strategy(Here ~ is a seperator)
                            string key = taxLot.Level1Name + Seperators.SEPERATOR_6 + taxLot.Level2Name;
                            if (!_dictClosingQuantityAccountAndStrategyWise.ContainsKey(key))
                            {
                                _dictClosingQuantityAccountAndStrategyWise.Add(key, taxLot.TaxLotQty);
                            }
                            else
                            {
                                _dictClosingQuantityAccountAndStrategyWise[key] += taxLot.TaxLotQty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// returns available close quantity for particular account
        /// </summary>
        /// <param name="closingTaxLots"></param>
        private double GetSellOpenQtyAccountWise(string accountName)
        {
            double TaxLotQty = 0;
            try
            {
                foreach (KeyValuePair<string, double> kvp in _dictClosingQuantityAccountAndStrategyWise)
                {
                    string[] key = kvp.Key.Split(Seperators.SEPERATOR_6);
                    if (accountName.Equals(key[0]))
                    {
                        TaxLotQty += kvp.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return TaxLotQty;
        }


        public void SetUp(AllocationGroup group)
        {
            try
            {
                Logger.LoggerWrite("Start: Setup method for close order UI", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                stopwatch.Start();
                if (_closingPreferences != null)
                {
                    int i = 0;
                    _quantityColumnFormat = "0." + i.ToString().PadLeft(_closingPreferences.QtyRoundoffDigits, '0');//_quantityColumnFormat = i.ToString("D4");
                    _currencyColumnFormat = "0." + i.ToString().PadLeft(_closingPreferences.PriceRoundOffDigits, '0');
                }


                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            SetUp(group);

                        }));
                    }
                    else
                    {
                        if (!group.State.Equals(PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED))
                        {
                            //Get opening positions for only those accounts for which closing group/taxlots have allocation
                            //i.e. group have data allocated in account1 than positional taxlots will be fetched for account1 only
                            if (group != null)
                            {
                                ClosingClientSideMapper.AllocationGroup = group;
                                string accountIDs = string.Empty;
                                if (group.AccountID != 0)
                                {
                                    accountIDs = group.AccountID.ToString();
                                    ClosingClientSideMapper.AccountId = group.AccountID;
                                }
                                else
                                {
                                    accountIDs = GetAccountIDsFromGroup(group);
                                    if (accountIDs.Length > 0)
                                        accountIDs = accountIDs.Substring(0, accountIDs.Length - 1);
                                }
                                //pass AllocationGroup and AccountIds as arguments to backgroundworker to fetch data.
                                object[] arguments = new object[2];
                                arguments[0] = group;
                                arguments[1] = accountIDs;
                                if (!_bgFetchData.IsBusy)
                                {
                                    //wingrid performance improve
                                    //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html
                                    //set displaylayout.maxbanddepth  between 5 to 8
                                    //DisplayLayout.ScrollStyle = ScrollStyle.Deferred;
                                    this.grdOpenTrades.Enabled = false;
                                    Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdOpenTrades);
                                    this.grdOpenTrades.BeginUpdate();
                                    this.grdOpenTrades.SuspendRowSynchronization();

                                    this.grdCloseTrades.Enabled = false;
                                    Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdCloseTrades);
                                    this.grdCloseTrades.BeginUpdate();
                                    this.grdCloseTrades.SuspendRowSynchronization();

                                    _bgFetchData.RunWorkerAsync(arguments);
                                }
                            }
                        }
                        else
                        {
                            RemoveAllDetails();
                        }
                    }
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Logger.LoggerWrite("End: Setup method for close order UI (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RemoveAllDetails()
        {
            try
            {
                lblSymbol.Text = string.Empty;
                lblTradeDate.Text = string.Empty;
                lblSide.Text = string.Empty;
                lblSide.Tag = string.Empty;
                lblAvgPrice.Text = string.Empty;
                lblBroker.Text = string.Empty;
                lblAllocatedQty.Text = string.Empty;
                lblOpenLots.Text = string.Empty;
                //cmbAccounts = null;
                //cmbStrategy = null;
                //cmbClosingMethodlogy = null;
                //ClosingClientSideMapper.OpenTaxlotsToPopulate = null;
                if (grdOpenTrades != null && grdOpenTrades.DataSource != null)
                {
                    grdOpenTrades.DataSource = null;
                }
                //grdOpenTrades.ResetDisplayLayout();
                //grdOpenTrades.Layouts.Clear(); 
                _dictSellTaxLotsAndPositions.Clear();

                _dictClosingQuantityAccountAndStrategyWise.Clear();

                ClosingClientSideMapper.GroupId = string.Empty;
                ClosingClientSideMapper.Symbol = string.Empty;
                ClosingClientSideMapper.AccountId = 0;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string GetAccountIDsFromGroup(AllocationGroup group)
        {
            string accountIDs = string.Empty;
            try
            {
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    accountIDs = accountIDs + taxlot.Level1ID + Seperators.SEPERATOR_8;
                }

            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accountIDs;
        }

        private void SetControlsDataSource(AllocationGroup group)
        {
            try
            {
                SetMainDetails(group);

                SetGridDataSources();

                List<TaxLot> buyTaxLotsAndPositions = GetPositionalTaxlotsFromPositionGrid();
                lblOpenLots.Text = GetOpenQtySum(buyTaxLotsAndPositions).ToString();

                cmbAccounts.Focus();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private TaxLot GetTaxlotByAccountID(AllocationGroup group)
        {
            try
            {
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    if (group.AccountID == taxlot.Level1ID)
                    {
                        return taxlot;

                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;

        }

        internal void SetMainDetails(AllocationGroup group)
        {
            try
            {
                //BindCombos();
                ClosingClientSideMapper.GroupId = group.GroupID;
                lblSymbol.Text = group.Symbol;
                lblTradeDate.Text = group.AUECLocalDate.Date.ToShortDateString();
                //during fetching updated group from db side text and counter party text are not available in the group
                //if (string.IsNullOrEmpty(group.OrderSide))
                //   lblSide.Text = TagDatabaseManager.GetInstance.GetOrderSideText(group.OrderSideTagValue);
                //else
                lblSide.Text = group.OrderSide;
                lblSide.Tag = group.OrderSideTagValue;
                lblAvgPrice.Text = group.AvgPrice.ToString();
                //if (string.IsNullOrEmpty(group.CounterPartyName))
                //    lblBroker.Text = CachedDataManager.GetInstance.GetCounterPartyText(group.CounterPartyID);
                //else
                lblBroker.Text = group.CounterPartyName;
                if (group.AccountID != 0 && group.AccountID != int.MinValue)
                {
                    cmbAccounts.Value = group.AccountID;
                    //As there is no strategy check while closing, so no need to filter data with strategy
                    //cmbStrategy.Value = group.StrategyID;
                    // if one taxlot is selected from allocation then taxlot qty qill be allocated qty else group allocated qty
                    TaxLot taxlot = GetTaxlotByAccountID(group);
                    if (taxlot != null)
                    {
                        lblAllocatedQty.Text = taxlot.TaxLotQty.ToString();
                    }
                    else
                    {
                        lblAllocatedQty.Text = string.Format("0");
                    }
                }
                else
                {
                    lblAllocatedQty.Text = group.AllocatedQty.ToString();
                    cmbAccounts.Value = int.MinValue;
                }
                UpdateAllocatedAndAvailableQuantityAtClosingAndUnwinding(null, 0);
                //update cache so that grids binded collection can be updated based on side and symbol
                ClosingClientSideMapper.Side = group.OrderSideTagValue;
                ClosingClientSideMapper.Symbol = group.Symbol;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add(HEADCOL_CheckBox, "");
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].CellClickAction = CellClickAction.EditAndSelectText;
                SetCheckBoxAtFirstPosition(grid);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Width = 10;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void SetGridDataSources()
        {
            try
            {
                if (grdOpenTrades != null && grdOpenTrades.DataSource == null)
                    //grdOpenTrades.DataSource = null;
                    grdOpenTrades.DataSource = ClosingClientSideMapper.OpenTaxlotsToPopulate;

                if (grdCloseTrades != null && grdCloseTrades.DataSource == null)
                    //grdCloseTrades.DataSource = null;
                    grdCloseTrades.DataSource = ClosingClientSideMapper.Netpositions;

                //SetFiltersToOpenTradesGrid();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private void SetFiltersToOpenTradesGrid()
        //{
        //    try
        //    {
        //        UltraGridBand grdBand = grdOpenTrades.DisplayLayout.Bands[0];
        //        if (grdBand.Columns.Count > 0)
        //        {
        //            //clear account filter
        //            grdBand.ColumnFilters[COL_OrderSideTagValue].ClearFilterConditions();
        //            grdBand.ColumnFilters[COL_OrderSideTagValue].LogicalOperator = FilterLogicalOperator.Or;
        //            //apply side filter for positional taxlot grid
        //            if (lblSide.Tag.ToString().Equals(FIXConstants.SIDE_Sell) || lblSide.Tag.ToString().Equals(FIXConstants.SIDE_Sell_Closed))
        //            {
        //                grdBand.ColumnFilters[COL_OrderSideTagValue].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy);
        //                grdBand.ColumnFilters[COL_OrderSideTagValue].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Closed);
        //            }
        //            else if (lblSide.Tag.ToString().Equals(FIXConstants.SIDE_Buy_Closed))
        //            {
        //                grdBand.ColumnFilters[COL_OrderSideTagValue].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_SellShort);
        //                grdBand.ColumnFilters[COL_OrderSideTagValue].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell_Open);
        //            }
        //            //apply symbol filter for positional taxlot grid
        //            grdBand.ColumnFilters[ClosingConstants.COL_Symbol].FilterConditions.Add(FilterComparisionOperator.Equals, lblSymbol.Text);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        private void BindCombos()
        {
            try
            {
                BindClosingMethodlogyCombo();
                BindAccountCombo();
                BindStrategyCombo();
                BindClosingFieldCombo();
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

        private void BindStrategyCombo()
        {
            try
            {
                Dictionary<int, string> dictStrategy = CommonDataCache.CachedDataManager.GetInstance.GetAllStrategies();
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                dictStrategy = GeneralUtilities.SortDictionaryByValues<int, string>(dictStrategy);
                DataTable dtStrategy = new DataTable();
                dtStrategy.Columns.Add("ID");
                dtStrategy.Columns.Add("Name");

                dtStrategy.Rows.Add(new object[] { int.MinValue, FILTER_ALL });

                foreach (KeyValuePair<int, string> kvp in dictStrategy)
                {
                    dtStrategy.Rows.Add(new object[] { kvp.Key, kvp.Value });
                }
                if (cmbStrategy != null)
                {
                    cmbStrategy.DataSource = dtStrategy;
                    cmbStrategy.DataBind();

                    cmbStrategy.DisplayMember = "Name";
                    cmbStrategy.ValueMember = "ID";
                    cmbStrategy.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                    cmbStrategy.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Strategy";
                    cmbStrategy.Value = int.MinValue;
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

        private void BindAccountCombo()
        {
            try
            {
                Dictionary<int, string> dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetAccountsWithFullName();
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                dictAccounts = GeneralUtilities.SortDictionaryByValues<int, string>(dictAccounts);
                DataTable dtAccounts = new DataTable();
                dtAccounts.Columns.Add("ID");
                dtAccounts.Columns.Add("Name");
                dtAccounts.Rows.Add(new object[] { int.MinValue, FILTER_ALL });

                foreach (KeyValuePair<int, string> kvp in dictAccounts)
                {
                    dtAccounts.Rows.Add(new object[] { kvp.Key, kvp.Value });
                }
                if (cmbAccounts != null)
                {
                    cmbAccounts.DataSource = dtAccounts;
                    cmbAccounts.DataBind();

                    cmbAccounts.DisplayMember = "Name";
                    cmbAccounts.ValueMember = "ID";
                    cmbAccounts.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                    cmbAccounts.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "CashAccounts";
                    cmbAccounts.Value = int.MinValue;
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

        private void BindClosingMethodlogyCombo()
        {
            try
            {
                List<EnumerationValue> lst = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                List<EnumerationValue> closingAlgos = new List<EnumerationValue>();
                foreach (EnumerationValue value in lst)
                {
                    //Modified By : Manvendra Jira : PRANA-10341
                    if ((!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.Multiple.ToString())))
                    {
                        closingAlgos.Add(value);
                    }
                }
                if (cmbClosingMethodlogy != null)
                {
                    cmbClosingMethodlogy.DataSource = closingAlgos;
                    cmbClosingMethodlogy.DisplayMember = "DisplayText";
                    cmbClosingMethodlogy.ValueMember = "Value";
                    Utils.UltraComboFilter(cmbClosingMethodlogy, "DisplayText");
                    //TODO: default value will be based on closing preferences
                    if (_closingPreferences != null)
                        cmbClosingMethodlogy.Value = (int)(_closingPreferences.ClosingMethodology.ClosingAlgo);

                    int i = 0;

                    //show tooltip to show closing Algos
                    foreach (EnumerationValue closingAlgosName in closingAlgos)
                    {
                        cmbClosingMethodlogy.Rows[i].ToolTipText = closingAlgosName.DisplayText;
                        i++;
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

        private void BindClosingFieldCombo()
        {
            try
            {
                List<EnumerationValue> closingField = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.ClosingField));

                if (cmbClosingField != null)
                {
                    cmbClosingField.DataSource = closingField;
                    cmbClosingField.DisplayMember = "DisplayText";
                    cmbClosingField.ValueMember = "Value";
                    Utils.UltraComboFilter(cmbClosingField, "DisplayText");
                    //TODO: default value will be based on closing preferences
                    if (_closingPreferences != null)
                        cmbClosingField.Value = (int)(_closingPreferences.ClosingMethodology.ClosingField);

                    int i = 0;
                    //show tooltip to show Closing Field
                    foreach (EnumerationValue ClosingFieldName in closingField)
                    {
                        cmbClosingField.Rows[i].ToolTipText = ClosingFieldName.DisplayText;
                        i++;
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

        private void SetOpenTradesGridColumns(UltraGrid grid)
        {
            try
            {
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];
                ColumnsCollection taxLotColumns = gridBand.Columns;
                foreach (UltraGridColumn column in taxLotColumns)
                {
                    column.Hidden = true;
                    column.CellActivation = Activation.NoEdit;
                }
                UltraGridColumn colLevel1Name = gridBand.Columns[ClosingConstants.COLUMN_LEVEL1NAME];
                colLevel1Name.Hidden = false;
                colLevel1Name.Header.Caption = ClosingConstants.CAPTION_LEVEL1NAME;
                colLevel1Name.Header.VisiblePosition = 1;
                colLevel1Name.CellActivation = Activation.NoEdit;

                UltraGridColumn colAuecLocalDate = gridBand.Columns[ClosingConstants.COLUMN_AUECLOCALDATE];
                colAuecLocalDate.Hidden = false;
                colAuecLocalDate.Header.VisiblePosition = 2;
                colAuecLocalDate.Header.Caption = ClosingConstants.CAPTION_AUECLOCALDATE;
                colAuecLocalDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colOrigPurchaseDate = gridBand.Columns[ClosingConstants.COLUMN_ORIGPURCHASEDATE];
                colOrigPurchaseDate.Hidden = false;
                colOrigPurchaseDate.Header.VisiblePosition = 3;
                colOrigPurchaseDate.Header.Caption = ClosingConstants.CAPTION_ORIGPURCHASEDATE;
                colOrigPurchaseDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colTaxLotQty = gridBand.Columns[ClosingConstants.COLUMN_TAXLOTQTY];
                colTaxLotQty.Hidden = false;
                colTaxLotQty.Header.VisiblePosition = 4;
                colTaxLotQty.Header.Caption = ClosingConstants.CAPTION_TAXLOTQTY;
                colTaxLotQty.Format = _quantityColumnFormat;
                colTaxLotQty.CellActivation = Activation.NoEdit;

                UltraGridColumn colAvgPrice = gridBand.Columns[ClosingConstants.COLUMN_AVGPRICE];
                colAvgPrice.Hidden = false;
                colAvgPrice.Header.VisiblePosition = 5;
                colAvgPrice.Header.Caption = ClosingConstants.CAPTION_AVGPRICE;
                colAvgPrice.Format = _currencyColumnFormat;
                colAvgPrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colLevel2Name = gridBand.Columns[ClosingConstants.COLUMN_LEVEL2NAME];
                colLevel2Name.Hidden = false;
                colLevel2Name.Header.Caption = ClosingConstants.CAPTION_LEVEL2NAME;
                colLevel2Name.Header.VisiblePosition = 6;
                colLevel2Name.CellActivation = Activation.NoEdit;

                UltraGridColumn colOrderSide = gridBand.Columns[ClosingConstants.COLUMN_ORDERSIDE];
                colOrderSide.Hidden = false;
                colOrderSide.Header.Caption = ClosingConstants.CAPTION_ORDERSIDE;
                colOrderSide.Header.VisiblePosition = 7;
                colOrderSide.CellActivation = Activation.NoEdit;

                UltraGridColumn colQtyToClose = gridBand.Columns[ClosingConstants.COLUMN_QUANTITYTOCLOSE];
                colQtyToClose.Hidden = false;
                colQtyToClose.Header.Caption = ClosingConstants.CAPTION_QUANTITYTOCLOSE;
                colQtyToClose.Header.VisiblePosition = 8;
                colQtyToClose.CellActivation = Activation.AllowEdit;
                colQtyToClose.Format = _quantityColumnFormat;
                colQtyToClose.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                //colQtyToClose.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Integer;
                //colQtyToClose.MaskInput = "nnnnnnnnnn";
                //colQtyToClose.InvalidValueBehavior = InvalidValueBehavior.RevertValueAndRetainFocus;


                //IncludedInColumnChooser(grdCloseTrades, IncludedColumns());

                //foreach (UltraGridRow row in grdCloseTrades.Rows)
                //{
                //    row.Appearance.ForeColor = Color.Green;
                //}

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.Hidden = false;
                    tradeAttributeCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
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
        /// 
        /// </summary>
        /// <param name="grid"></param>
        private void SetCloseTradesGridColumns(UltraGrid grid)
        {
            try
            {
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];

                UltraGridColumn colClosingTradeDate = gridBand.Columns[ClosingConstants.COL_ClosingTradeDate];
                colClosingTradeDate.Header.VisiblePosition = 1;
                colClosingTradeDate.Width = 80;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_AccountValue];
                colAccount.Header.VisiblePosition = 2;
                colAccount.Width = 100;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_Strategy];
                colStrategyIDValue.Header.VisiblePosition = 3;
                colStrategyIDValue.Width = 120;


                UltraGridColumn colPositionTag = gridBand.Columns[ClosingConstants.COL_PositionTag];
                colPositionTag.Header.VisiblePosition = 4;
                colPositionTag.Width = 70;


                UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                colAssetCategoryValue.Header.VisiblePosition = 5;
                colAssetCategoryValue.Width = 60;


                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Header.VisiblePosition = 6;
                colSymbol.Width = 70;


                UltraGridColumn colClosingQuantity = gridBand.Columns[ClosingConstants.COL_ClosedQty];
                colClosingQuantity.Header.VisiblePosition = 7;
                colClosingQuantity.Width = 70;


                UltraGridColumn colOpenAveragePrice = gridBand.Columns[ClosingConstants.COL_OpenAveragePrice];
                colOpenAveragePrice.Header.VisiblePosition = 8;
                colOpenAveragePrice.Width = 75;


                UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
                colClosedAveragePrice.Header.VisiblePosition = 9;
                colClosedAveragePrice.Width = 70;


                UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
                colCommision.Header.VisiblePosition = 10;
                colCommision.Width = 80;

                UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
                colRealizedPNL.Header.VisiblePosition = 11;
                colRealizedPNL.Width = 90;

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.Hidden = true;
                    tradeAttributeCol.CellActivation = Activation.NoEdit;
                }

                #region commented
                //UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
                //colExchange.Header.Caption = ClosingConstants.CAP_Exchange;
                //colExchange.Hidden = true;

                // Modified By : Pranay Deep, 28th Sept 2015
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-11239
                //UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                //colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;
                //colClosingAlgo.Hidden = true;

                //UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
                //colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;
                //colSide.Hidden = true;


                //UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
                //colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;
                //colPositionCommision.Format = _currencyColumnFormat;
                //colPositionCommision.Hidden = true;

                //UltraGridColumn colClosingSide = gridBand.Columns[ClosingConstants.COL_ClosingSide];
                //colClosingSide.Header.Caption = ClosingConstants.CAP_ClosingSide;
                //colClosingSide.Hidden = true;

                //UltraGridColumn colPositionID = gridBand.Columns[ClosingConstants.COL_ID];
                //colPositionID.Header.Caption = ClosingConstants.CAP_TaxlotId;
                //colPositionID.Hidden = true;

                //UltraGridColumn colCurrency = gridBand.Columns[ClosingConstants.COL_Currency];
                //colCurrency.Header.Caption = ClosingConstants.CAP_Currency;
                //colCurrency.Hidden = true;

                //UltraGridColumn colUnderlying = gridBand.Columns[ClosingConstants.COL_Underlying];
                //colUnderlying.Header.Caption = ClosingConstants.CAP_Underlying;
                //colUnderlying.Hidden = true;

                //UltraGridColumn colClosingID = gridBand.Columns[ClosingConstants.COL_ClosingID];
                //colClosingID.Header.Caption = ClosingConstants.CAP_ClosingID;
                //colClosingID.Hidden = true;

                //UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
                //colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;
                //colClosingMode.Hidden = true;

                //UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
                //colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;
                //colClosingTag.Hidden = true;
                //// Modified By : Manvendra Prajapati
                //// Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8746
                //UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
                //colTradeDate.Header.Caption = ClosingPrefManager.GetCaptionBasedonClosingDateType(); //ClosingConstants.CAP_TradeDate;
                //colTradeDate.Hidden = true;
                //colTradeDate.CellActivation = Activation.NoEdit;

                //UltraGridColumn colNotionalValue = gridBand.Columns[ClosingConstants.COL_NotionalValue];
                //colNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colNotionalValue.Format = _currencyColumnFormat;
                //colNotionalValue.Hidden = true;

                //UltraGridColumn colAccountID = gridBand.Columns[ClosingConstants.COL_AccountID];
                //colAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colAccountID.Hidden = true;

                //UltraGridColumn colSettleExpire = gridBand.Columns[ClosingConstants.COL_IsExpired_Settled];
                //colSettleExpire.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colSettleExpire.Hidden = true;

                //UltraGridColumn colTimeOfSaveUTC = gridBand.Columns[ClosingConstants.COL_CloseDate];
                //colTimeOfSaveUTC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colTimeOfSaveUTC.Hidden = true;

                //UltraGridColumn colStrategyID = gridBand.Columns[ClosingConstants.COL_StrategyID];
                //colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colStrategyID.Hidden = true;

                //UltraGridColumn colCurrencyID = gridBand.Columns[ClosingConstants.COL_CurrencyID];
                //colCurrencyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colCurrencyID.Hidden = true;

                //UltraGridColumn ColAUECID = gridBand.Columns[ClosingConstants.COL_AUECID];
                //ColAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //ColAUECID.Hidden = true;

                //UltraGridColumn ColDescription = gridBand.Columns[ClosingConstants.COL_Description];
                //ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //ColDescription.Hidden = true;

                //UltraGridColumn ColMultiplier = gridBand.Columns[ClosingConstants.COL_Multiplier];
                //ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //ColMultiplier.Hidden = true;

                //  UltraGridColumn ColCostBasisGrossPNL = gridBand.Columns[ClosingConstants.COL_CostBasisGrossPNL];
                //  ColCostBasisGrossPNL.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //  ColCostBasisGrossPNL.Format = _currencyColumnFormat;
                ////  ColCostBasisGrossPNL.Hidden = true;

                //  UltraGridColumn COLNotionalChange = gridBand.Columns[ClosingConstants.COL_NotionalChange];
                //  COLNotionalChange.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //  COLNotionalChange.Format = _currencyColumnFormat;
                // // COLNotionalChange.Hidden = true;
                #endregion
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
        /// Set summary for close trade columns
        /// </summary>
        private void SetSummaryAndCaptionForCloseTradeColumns()
        {
            try
            {
                UltraGridBand gridBand = grdCloseTrades.DisplayLayout.Bands[0];

                UltraGridColumn colClosingTradeDate = gridBand.Columns[ClosingConstants.COL_ClosingTradeDate];
                colClosingTradeDate.Header.Caption = ClosingConstants.CAP_CloseDt;
                colClosingTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colClosingTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_AccountValue];
                colAccount.Header.Caption = ClosingConstants.CAP_Account;
                colAccount.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_Strategy];
                colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;
                colStrategyIDValue.CellActivation = Activation.NoEdit;


                UltraGridColumn colPositionTag = gridBand.Columns[ClosingConstants.COL_PositionTag];
                colPositionTag.Header.Caption = ClosingConstants.CAP_PositionType;
                colPositionTag.CellActivation = Activation.NoEdit;


                UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                colAssetCategoryValue.Header.Caption = ClosingConstants.CAP_AssetCategory;
                colAssetCategoryValue.CellActivation = Activation.NoEdit;


                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;
                colSymbol.CellActivation = Activation.NoEdit;


                UltraGridColumn colClosingQuantity = gridBand.Columns[ClosingConstants.COL_ClosedQty];
                colClosingQuantity.Header.Caption = ClosingConstants.CAP_CloseQty;
                colClosingQuantity.Format = _quantityColumnFormat;
                colClosingQuantity.CellActivation = Activation.NoEdit;


                UltraGridColumn colOpenAveragePrice = gridBand.Columns[ClosingConstants.COL_OpenAveragePrice];
                colOpenAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
                colOpenAveragePrice.Format = _currencyColumnFormat;
                colOpenAveragePrice.CellActivation = Activation.NoEdit;


                UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
                colClosedAveragePrice.Header.Caption = ClosingConstants.CAP_AvgClosingPrice;
                colClosedAveragePrice.Format = _currencyColumnFormat;
                colClosedAveragePrice.CellActivation = Activation.NoEdit;


                UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
                colCommision.Header.Caption = ClosingConstants.COL_ClosingTotalCommissionandFees;
                colCommision.Format = _currencyColumnFormat;
                colCommision.Hidden = true;
                colCommision.CellActivation = Activation.NoEdit;

                UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
                colRealizedPNL.Header.Caption = ClosingConstants.CAP_RealizedPNL;
                colRealizedPNL.Format = _currencyColumnFormat;
                colRealizedPNL.CellActivation = Activation.NoEdit;

                UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
                colExchange.Header.Caption = ClosingConstants.CAP_Exchange;
                colExchange.Hidden = true;

                // Modified By : Pranay Deep, 28th Sept 2015
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-11239
                UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;
                colClosingAlgo.Hidden = true;

                UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
                colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;
                colSide.Hidden = true;


                UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
                colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;
                colPositionCommision.Format = _currencyColumnFormat;
                colPositionCommision.Hidden = true;

                UltraGridColumn colClosingSide = gridBand.Columns[ClosingConstants.COL_ClosingSide];
                colClosingSide.Header.Caption = ClosingConstants.CAP_ClosingSide;
                colClosingSide.Hidden = true;

                UltraGridColumn colPositionID = gridBand.Columns[ClosingConstants.COL_ID];
                colPositionID.Header.Caption = ClosingConstants.CAP_TaxlotId;
                colPositionID.Hidden = true;

                UltraGridColumn colCurrency = gridBand.Columns[ClosingConstants.COL_Currency];
                colCurrency.Header.Caption = ClosingConstants.CAP_Currency;
                colCurrency.Hidden = true;

                UltraGridColumn colUnderlying = gridBand.Columns[ClosingConstants.COL_Underlying];
                colUnderlying.Header.Caption = ClosingConstants.CAP_Underlying;
                colUnderlying.Hidden = true;

                UltraGridColumn colClosingID = gridBand.Columns[ClosingConstants.COL_ClosingID];
                colClosingID.Header.Caption = ClosingConstants.CAP_ClosingID;
                colClosingID.Hidden = true;

                UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
                colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;
                colClosingMode.Hidden = true;

                UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
                colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;
                colClosingTag.Hidden = true;
                // Modified By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8746
                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
                colTradeDate.Header.Caption = ClosingPrefManager.GetCaptionBasedonClosingDateType(); //ClosingConstants.CAP_TradeDate;
                colTradeDate.Hidden = true;
                colTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colNotionalValue = gridBand.Columns[ClosingConstants.COL_NotionalValue];
                colNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colNotionalValue.Format = _currencyColumnFormat;
                colNotionalValue.Hidden = true;

                UltraGridColumn colAccountID = gridBand.Columns[ClosingConstants.COL_AccountID];
                colAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colAccountID.Hidden = true;

                UltraGridColumn colSettleExpire = gridBand.Columns[ClosingConstants.COL_IsExpired_Settled];
                colSettleExpire.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSettleExpire.Hidden = true;

                UltraGridColumn colTimeOfSaveUTC = gridBand.Columns[ClosingConstants.COL_CloseDate];
                colTimeOfSaveUTC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colTimeOfSaveUTC.Hidden = true;

                UltraGridColumn colStrategyID = gridBand.Columns[ClosingConstants.COL_StrategyID];
                colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colStrategyID.Hidden = true;

                UltraGridColumn colCurrencyID = gridBand.Columns[ClosingConstants.COL_CurrencyID];
                colCurrencyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colCurrencyID.Hidden = true;

                UltraGridColumn ColAUECID = gridBand.Columns[ClosingConstants.COL_AUECID];
                ColAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColAUECID.Hidden = true;

                UltraGridColumn ColDescription = gridBand.Columns[ClosingConstants.COL_Description];
                ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColDescription.Hidden = true;

                UltraGridColumn ColMultiplier = gridBand.Columns[ClosingConstants.COL_Multiplier];
                ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColMultiplier.Hidden = true;
                UltraGridColumn ColCostBasisGrossPNL = gridBand.Columns[ClosingConstants.COL_CostBasisGrossPNL];
                ColCostBasisGrossPNL.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColCostBasisGrossPNL.Format = _currencyColumnFormat;


                UltraGridColumn COLNotionalChange = gridBand.Columns[ClosingConstants.COL_NotionalChange];
                COLNotionalChange.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                COLNotionalChange.Format = _currencyColumnFormat;

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
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
        /// Sets the allocated trade grids row appearance.
        /// </summary>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        /// <param name="isLongAllocatedTradesGrid">if set to <c>true</c> [is long allocated trades grid].</param>
        private void SetOpenTradesGridRowAppearance(InitializeRowEventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    e.Row.Appearance.ForeColor = Color.GreenYellow;
                }
                else
                {
                    e.Row.Appearance.ForeColor = Color.FromArgb(39, 174, 96);
                }
                grdOpenTrades.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.EditAndSelectText;
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

        private void SetCloseTradesGridRowAppearance(InitializeRowEventArgs e)
        {
            try
            {
                //string side = string.Empty;
                if (!CustomThemeHelper.ApplyTheme)
                {
                    if (_isCloseTradeInitialized && !e.Row.Appearance.ForeColor.Equals(Color.White))
                        e.Row.Appearance.ForeColor = Color.Violet;
                    else
                        e.Row.Appearance.ForeColor = Color.White;
                }
                else
                {
                    if (_isCloseTradeInitialized && !e.Row.Appearance.ForeColor.Equals(Color.Black))
                        e.Row.Appearance.ForeColor = Color.FromArgb(127, 0, 254);
                    else
                        e.Row.Appearance.ForeColor = Color.Black;
                }

                //e.Row.Appearance.BackColor = Color.Lavender;
                //grdCloseTrades.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
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

        //private List<string> IncludedColumns()
        //{
        //    List<string> lst = new List<string>();
        //    try
        //    {
        //        lst.Add(HEADCOL_CheckBox);
        //        lst.Add(ClosingConstants.COLUMN_LEVEL1NAME);
        //        lst.Add(ClosingConstants.COLUMN_AUECLOCALDATE);
        //        lst.Add(ClosingConstants.COLUMN_ORIGPURCHASEDATE);
        //        lst.Add(ClosingConstants.COLUMN_TAXLOTQTY);
        //        lst.Add(ClosingConstants.COLUMN_AVGPRICE);
        //        lst.Add(ClosingConstants.COLUMN_LEVEL2NAME);
        //        lst.Add(ClosingConstants.COLUMN_QUANTITYTOCLOSE);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return lst;
        //}

        //private void IncludedInColumnChooser(UltraGrid grid, List<string> lst)
        //{
        //    try
        //    {
        //        foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
        //        {
        //            grid.DisplayLayout.Bands[0].Columns[column.Key].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        }

        //        foreach (string column in lst)
        //        {
        //            grid.DisplayLayout.Bands[0].Columns[column].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private List<TaxLot> GetPositionalTaxlotsFromPositionGrid()
        {
            List<TaxLot> buyTaxLotsAndPositions = new List<TaxLot>();
            try
            {
                if (grdOpenTrades != null && grdOpenTrades.Rows != null && grdOpenTrades.Rows.Count > 0)
                {
                    grdOpenTrades.UpdateData();
                    UltraGridRow[] grdFilteredRows = grdOpenTrades.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in grdFilteredRows)
                    {
                        buyTaxLotsAndPositions.Add((TaxLot)row.ListObject);
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
            return buyTaxLotsAndPositions;
        }

        /// <summary>
        /// get populated closed quantity for taxlots
        /// </summary>
        /// <param name="ClosedData"></param>
        /// <returns></returns>
        private Dictionary<string, double> GetPopulatedClosedQtyForTaxlots(ClosingData ClosedData)
        {
            Dictionary<string, double> dictClosedPositionalTaxlots = new Dictionary<string, double>();
            try
            {
                foreach (TaxLot taxLot in ClosedData.Taxlots)
                {
                    if (!dictClosedPositionalTaxlots.ContainsKey(taxLot.TaxLotID.ToString()))
                    {
                        dictClosedPositionalTaxlots.Add(taxLot.TaxLotID.ToString(), taxLot.TaxLotQtyToClose);
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
            return dictClosedPositionalTaxlots;
        }

        private void PopulateCloseQty(Dictionary<string, double> dictClosedPositionalTaxlots)
        {
            try
            {
                _dictAccountWiseQtyToClose.Clear();
                foreach (UltraGridRow row in grdOpenTrades.Rows.GetFilteredInNonGroupByRows())
                {
                    TaxLot taxLot = (TaxLot)row.ListObject;
                    if (dictClosedPositionalTaxlots.ContainsKey(taxLot.TaxLotID.ToString()))
                    {
                        //grdOpenTrades.AfterCellUpdate -= new CellEventHandler(grdOpenTrades_AfterCellUpdate);
                        row.Cells[ClosingConstants.COLUMN_QUANTITYTOCLOSE].Value = dictClosedPositionalTaxlots[taxLot.TaxLotID.ToString()];
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
        /// This method removes taxlots which have QtyToClose field zero
        /// </summary>
        /// <param name="buyTaxLotsAndPositions"></param>
        /// <returns>
        /// total openquantity so that validation can be done before closing
        /// </returns>
        private double RemovePositionalTradesWithZeroQtyToCloseAndWithGreaterTradeDate(ref List<TaxLot> buyTaxLotsAndPositions)
        {
            double totalOpenQty = 0;
            try
            {
                for (int i = 0; i < buyTaxLotsAndPositions.Count; i++)
                {
                    //set closing date based on preferences
                    SetClosingDateBasedOnPreferences(buyTaxLotsAndPositions[i]);
                    totalOpenQty += buyTaxLotsAndPositions[i].TaxLotQtyToClose;
                    if (buyTaxLotsAndPositions[i].TaxLotQtyToClose == 0)
                    {
                        buyTaxLotsAndPositions.Remove(buyTaxLotsAndPositions[i]);
                        i--;
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
            return totalOpenQty;
        }

        private void SetClosingDateBasedOnPreferences(TaxLot taxlot)
        {
            try
            {

                switch (_closingPreferences.DateType)
                {
                    case PostTradeEnums.DateType.AuecLocalDate:
                        taxlot.ClosingDate = taxlot.AUECLocalDate;
                        break;
                    case PostTradeEnums.DateType.ProcessDate:
                        taxlot.ClosingDate = taxlot.ProcessDate;
                        break;
                    case PostTradeEnums.DateType.OriginalPurchaseDate:
                        taxlot.ClosingDate = taxlot.OriginalPurchaseDate;
                        break;
                    default:
                        taxlot.ClosingDate = taxlot.OriginalPurchaseDate;
                        break;
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
        /// get total no. of open positions for the selected group/taxlot
        /// </summary>
        /// <param name="openTaxlots"></param>
        /// <returns></returns>
        private double GetOpenQtySum(List<TaxLot> openTaxlots)
        {
            double openQtySum = 0;

            try
            {
                foreach (TaxLot taxlot in openTaxlots)
                {
                    openQtySum = openQtySum + Convert.ToDouble(taxlot.TaxLotQty);
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
            return openQtySum;
        }

        /// <summary>
        /// Check for close trade error in closed data
        /// </summary>
        /// <param name="ClosedData"></param>
        /// <returns></returns>
        private bool CheckForCloseTradeError(ClosingData ClosedData)
        {
            try
            {
                if (ClosedData != null)
                {
                    if (ClosedData.IsNavLockFailed)
                    {
                        MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    if (ClosedData.ClosedPositions.Count > 0)
                    {
                        //show warning message if closing algorithm is PRESET, in this case data will be closed for accounts which have invalid secondary sort criteria
                        if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                        {
                            MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        else
                        {
                            //InformationMessageBox.Display("Close Trade Data Saved!!!");
                            return false;
                        }
                    }
                    else
                    {
                        //show warning message if closing algorithm is not PRESET, in this case data will be closed if valid secondary sort criteria given from the closing UI
                        if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                        {
                            MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        else
                        {
                            InformationMessageBox.Display("Nothing to Close");
                            return true;
                        }
                    }
                }
                else
                    InformationMessageBox.Display("Nothing to Close");
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
            return false;
        }

        #region ComboBox Events for account and strategy

        /// <summary>
        /// Apply account filter on grid on the basis of account selected from account combobox
        /// if All selected from combobox than remove account filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccounts_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccounts != null && !string.IsNullOrEmpty(cmbAccounts.Text))
                {
                    //get name of selected account
                    string accountName = cmbAccounts.Text.ToString();
                    UltraGridBand grdBand = grdOpenTrades.DisplayLayout.Bands[0];
                    if (grdBand.Columns.Count > 0)
                    {
                        //clear account filter
                        grdBand.ColumnFilters[ClosingConstants.COLUMN_LEVEL1NAME].ClearFilterConditions();
                        if (!accountName.Equals(FILTER_ALL))
                        {
                            //add filter to grid on the basis of selected value from account combobox
                            grdBand.ColumnFilters[ClosingConstants.COLUMN_LEVEL1NAME].FilterConditions.Add(FilterComparisionOperator.Equals, accountName);
                        }
                    }
                    if (cmbStrategy != null && !string.IsNullOrEmpty(cmbStrategy.Text))
                        UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(accountName, cmbStrategy.Text);
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
        /// Apply strategy filter on grid on the basis of strategy selected from strategy combobox
        /// if All selected from combobox than remove strategy filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStrategy_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStrategy != null && !string.IsNullOrEmpty(cmbStrategy.Text))
                {
                    //get name of selected strategy
                    string strategy = cmbStrategy.Text.ToString();
                    UltraGridBand grdBand = grdOpenTrades.DisplayLayout.Bands[0];
                    if (grdBand.Columns.Count > 0)
                    {
                        //clear strategy filter
                        grdBand.ColumnFilters[ClosingConstants.COLUMN_LEVEL2NAME].ClearFilterConditions();
                        if (!strategy.Equals(FILTER_ALL))
                        {
                            //add filter to grid on the basis of selected value from strategy combobox
                            grdBand.ColumnFilters[ClosingConstants.COLUMN_LEVEL2NAME].FilterConditions.Add(FilterComparisionOperator.Equals, strategy);
                        }
                        if (cmbAccounts != null && !string.IsNullOrEmpty(cmbAccounts.Text))
                            UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(cmbAccounts.Text, strategy);
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

        #endregion


        #region grdOpenTrades Methods

        private void grdOpenTrades_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                SetOpenTradesGridColumns(grdOpenTrades);
                //if (!grdOpenTrades.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                //{
                //    AddCheckBoxinGrid(grdOpenTrades, _headerCheckBoxUnallocated);
                //}

                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                //e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;

                if (!CustomThemeHelper.ApplyTheme)
                {
                    grdOpenTrades.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);
                    grdOpenTrades.DisplayLayout.Override.RowAppearance.BackColor2 = Color.Transparent;
                    grdOpenTrades.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Black;
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

        private void grdOpenTrades_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                SetOpenTradesGridRowAppearance(e);
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
        /// does not allow user to enter qty to close more than open qty of taxlot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdOpenTrades_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                //get accountname from row which is being updated
                string accountName = e.Cell.Row.Cells[ClosingConstants.COLUMN_LEVEL1NAME].Text;
                //get original value of cell
                double originalValue = double.Parse(e.Cell.OriginalValue.ToString());
                double updatedValue = 0;
                //updated value: get newly entered value
                double.TryParse(e.Cell.Text, out updatedValue);
                //get value of text of label Quantity To Close
                double QtyToCloseText = double.Parse(lblAllocatedQty.Text);
                //get available close qty for account for which current row belongs
                double AvailableTaxLotCloseQtyForAccount = GetSellOpenQtyAccountWise(accountName);
                double AllotedTaxLotCloseQtyForAccount = 0;
                //based on alloted and available quantity for a particular account, does not allow user to enter quantity greater than available
                if (_dictAccountWiseQtyToClose.ContainsKey(accountName))
                {

                    //if entered value is less than previous value
                    if (originalValue > updatedValue)
                    {
                        AllotedTaxLotCloseQtyForAccount = (_dictAccountWiseQtyToClose[accountName] - (originalValue - updatedValue));
                    }
                    //if entered value is greater than previous value and alloted quantity is less than available quantity
                    else if ((_dictAccountWiseQtyToClose[accountName] - (originalValue - updatedValue)) < AllotedTaxLotCloseQtyForAccount)
                    {
                        AllotedTaxLotCloseQtyForAccount = (_dictAccountWiseQtyToClose[accountName] - (originalValue - updatedValue));
                    }
                    else
                    {
                        AllotedTaxLotCloseQtyForAccount = _dictAccountWiseQtyToClose[accountName] - (originalValue - updatedValue);
                    }
                }
                //check for updated AllotedTaxLotCloseQtyForAccount
                if (AllotedTaxLotCloseQtyForAccount > AvailableTaxLotCloseQtyForAccount)
                {
                    if (_dictAccountWiseQtyToClose.ContainsKey(accountName))
                        SetStatusMessage(this, new EventArgs<string>("Available Quantity to close for account " + accountName + " is only: " + (AvailableTaxLotCloseQtyForAccount - _dictAccountWiseQtyToClose[accountName])));
                    else
                        SetStatusMessage(this, new EventArgs<string>("Available Quantity to close for account " + accountName + " is only: " + (AvailableTaxLotCloseQtyForAccount)));
                    grdOpenTrades.AfterCellUpdate -= new CellEventHandler(grdOpenTrades_AfterCellUpdate);
                    e.Cell.Value = 0;
                    grdOpenTrades.AfterCellUpdate += new CellEventHandler(grdOpenTrades_AfterCellUpdate);
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2314
                    //"Qty to close" value is not updating automatically when user manually edit close Qty value.
                    lblAllocatedQty.Text = Convert.ToString(QtyToCloseText + originalValue);
                    //Whenever we change TaxlotQtyToClose greater than available qty for that account then it revert back to zero showing message on status strip.
                    //but we are not updating _dictAccountWiseQtyToClose and because of this once we violet the qty rule we are not able to update the TaxlotQtyToClose
                    _dictAccountWiseQtyToClose[accountName] -= originalValue;
                }
                else
                {
                    //to handle use case when account f1 is fully closed but have open positions
                    //when we enter close quantity in that field than lblAllocatedQty.Text become negative 
                    if ((QtyToCloseText + (originalValue - updatedValue)) >= 0)
                    {
                        lblAllocatedQty.Text = Convert.ToString(QtyToCloseText + (originalValue - updatedValue));
                        //update accountwise alloted quantity
                        if (!_dictAccountWiseQtyToClose.ContainsKey(accountName))
                        {
                            _dictAccountWiseQtyToClose.Add(accountName, updatedValue);
                        }
                        else
                        {
                            _dictAccountWiseQtyToClose[accountName] += (updatedValue - originalValue);
                        }
                    }
                    else
                    {
                        SetStatusMessage(this, new EventArgs<string>("Available Quantity to close for account " + accountName + " is only: 0"));
                        grdOpenTrades.AfterCellUpdate -= new CellEventHandler(grdOpenTrades_AfterCellUpdate);
                        e.Cell.Value = 0;
                        grdOpenTrades.AfterCellUpdate += new CellEventHandler(grdOpenTrades_AfterCellUpdate);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdOpenTrades_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                //does not allow to be a close quantity cell blank, make it zero
                double TaxLotQty = Convert.ToDouble(e.Cell.Row.Cells[ClosingConstants.COLUMN_TAXLOTQTY].Text);
                double updatedValue = 0;
                bool isDouble = double.TryParse(e.Cell.Text, out updatedValue);
                if (!isDouble)
                {
                    grdOpenTrades.CellChange -= new CellEventHandler(grdOpenTrades_CellChange);
                    e.Cell.Value = 0.0;
                    grdOpenTrades.CellChange += new CellEventHandler(grdOpenTrades_CellChange);
                }
                else if (updatedValue > TaxLotQty)
                {
                    //grdOpenTrades.AfterCellUpdate -= new CellEventHandler(grdOpenTrades_AfterCellUpdate);
                    grdOpenTrades.CellChange -= new CellEventHandler(grdOpenTrades_CellChange);
                    e.Cell.Value = TaxLotQty;
                    grdOpenTrades.CellChange += new CellEventHandler(grdOpenTrades_CellChange);
                    //grdOpenTrades.AfterCellUpdate += new CellEventHandler(grdOpenTrades_AfterCellUpdate);

                    string statusMessage = "Close quantity cannot be greater than available open quantity";
                    SetStatusMessage(this, new EventArgs<string>(statusMessage));
                }

                //update closing algo to manual if user changes close quantity
                if (e.Cell.Column.Key.Equals(ClosingConstants.COLUMN_QUANTITYTOCLOSE) && (e.Cell.OriginalValue != e.Cell.Value))
                {
                    _closingAlgo = PostTradeEnums.CloseTradeAlogrithm.MANUAL;
                }
                //grdOpenTrades.UpdateData();
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
        /// Does not allow user to enter quantity greater than the available open quantity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdOpenTrades_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                SetStatusMessage(this, new EventArgs<string>(string.Empty));
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
        /// Move to next Close Qty cell on Up and down arrow key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdOpenTrades_KeyDown(object sender, KeyEventArgs e)
        {
            // int keyValue = e.KeyCode;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    grdOpenTrades.PerformAction(UltraGridAction.BelowCell);
                    grdOpenTrades.PerformAction(UltraGridAction.EnterEditMode);
                    e.Handled = true;
                    break;

                case Keys.Up:
                    grdOpenTrades.PerformAction(UltraGridAction.AboveCell);
                    grdOpenTrades.PerformAction(UltraGridAction.EnterEditMode);
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region grdCloseTrades Methods
        private void grdCloseTrades_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            try
            {
                SetCloseTradesGridColumns(grdCloseTrades);

                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.Override.CellClickAction = CellClickAction.Edit;

                grdCloseTrades.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);
                grdCloseTrades.DisplayLayout.Override.RowAppearance.BackColor2 = Color.Transparent;
                grdCloseTrades.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Black;

                if (ClosingPrefManager.ClosingLayout.CloseOrderNetPositionColumns.Count > 0)
                {
                    List<ColumnData> listLongColData = ClosingPrefManager.ClosingLayout.CloseOrderNetPositionColumns;
                    SetGridColumnLayout(grdCloseTrades, listLongColData);
                }
                else
                {
                    // setting the default layout
                    SetCloseTradesGridColumns(grdCloseTrades);
                }

                //Set summary and caption for close trade grid columns
                SetSummaryAndCaptionForCloseTradeColumns();

                //add checkbox at first column in the grid
                if (!grdCloseTrades.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdCloseTrades, _headerCheckBoxUnallocated);
                }
                //Added By : Manvendra Prajapati
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8071
                UltraGridBand band = e.Layout.Bands[0];
                if (band.Columns.Exists(ClosingConstants.COL_ClosingAlgo))
                {
                    band.Columns[ClosingConstants.COL_ClosingAlgo].ValueList = GetValueListForClosingAlgo();
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
        /// Get Value List of Closing Algo
        /// </summary>
        /// <returns></returns>
        private ValueList GetValueListForClosingAlgo()
        {
            ValueList ClosingAlgoList = new ValueList();
            try
            {
                List<EnumerationValue> ClosingAlgoEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                foreach (EnumerationValue var in ClosingAlgoEnumList)
                {
                    ClosingAlgoList.ValueListItems.Add(var.Value, var.DisplayText);
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
            return ClosingAlgoList;
        }
        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();

            try
            {
                // Hide All
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                }

                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        gridCol.Format = colData.Format;
                        gridCol.Header.Caption = colData.Caption;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        gridCol.CellActivation = Activation.NoEdit;

                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }
                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                if ((colData.Key.Equals(ClosingConstants.COL_ClosingTradeDate)) && colData.FilterConditionList.Count == 1 && colData.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && colData.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                }
                                else
                                {
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }

        private void grdCloseTrades_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                SetCloseTradesGridRowAppearance(e);

                if ((ClosingMode)e.Row.Cells[ClosingConstants.COL_ClosingMode].Value == ClosingMode.CorporateAction)
                {
                    switch ((PositionTag)e.Row.Cells[ClosingConstants.COL_PositionTag].Value)
                    {
                        case PositionTag.Long:
                        case PositionTag.Short:
                            {
                                switch ((PositionTag)e.Row.Cells[ClosingConstants.COL_ClosingTag].Value)
                                {

                                    case PositionTag.LongWithdrawal:
                                    case PositionTag.ShortWithdrawal:
                                        {
                                            e.Row.Hidden = true;
                                            break;
                                        }
                                    default:
                                        e.Row.Hidden = true;
                                        break;
                                }
                                break;
                            }
                        default:
                            e.Row.Hidden = true;
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
        #endregion

        #region Button Click Events

        /// <summary>
        /// Populate closing on the grid on the basis of given closing algo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //set closed quantity to zero for all the rows when populate button is clicked so that new closed qty population can be done
                btnClear_Click(null, null);

                ClosingData ClosedData = new ClosingData();
                List<TaxLot> buyTaxLotsAndPositions = GetPositionalTaxlotsFromPositionGrid();
                //PostTradeEnums.CloseTradeAlogrithm _closingAlgo = PostTradeEnums.CloseTradeAlogrithm.NONE;
                if (cmbClosingMethodlogy.Value != null)
                    _closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)(cmbClosingMethodlogy.Value);
                if (cmbClosingField.Value != null)
                    _closingField = (PostTradeEnums.ClosingField)(cmbClosingField.Value);

                List<TaxLot> SellTaxLotsAndPositions = GetClosingTaxlotsList();

                List<TaxLot> listPositionalTaxLots = buyTaxLotsAndPositions;
                List<TaxLot> listClosingTaxLots = SellTaxLotsAndPositions;
                //in case of closig side BuyToClose taxlots will be reversed i.e. buy taxlots will be sell taxlots and vice versa
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2306
                if (lblSide.Text.ToUpper().Replace(" ", string.Empty).Equals((SIDE_BUYTOCLOSE)))
                {
                    listPositionalTaxLots = SellTaxLotsAndPositions;
                    listClosingTaxLots = buyTaxLotsAndPositions;
                }
                if (listPositionalTaxLots.Count > 0 && listClosingTaxLots.Count > 0 && !(_closingAlgo.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE)))
                {
                    //isManual field will be false when closing algo is PRESET, in all other algos this field will be true
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2303
                    //Object Reference error while selecting Preset Algo and then hit close button.
                    ClosingParameters closingParams = new ClosingParameters();
                    //buyTaxLotsAndPositions, sellTaxLotsAndPositions, algorithm = selected from algo derpdown, IsShortWithBuyAndBuyToCover=false, IsSellWithBuyToClose = false, isManual = true(not from preset algo), isDragDrop = false, isFromServer = false, SecondarySortCriteria=None, isVirtualClosingPopulate = true, isOverrideWithUserClosing = false ,isMatchStrategy=according to checkbox 
                    if (_closingAlgo.Equals(PostTradeEnums.CloseTradeAlogrithm.PRESET))
                    {
                        closingParams.BuyTaxLotsAndPositions = listPositionalTaxLots;
                        closingParams.SellTaxLotsAndPositions = listClosingTaxLots;
                        closingParams.Algorithm = _closingAlgo;
                        closingParams.IsShortWithBuyAndBuyToCover = false;
                        closingParams.IsSellWithBuyToClose = false;
                        closingParams.IsManual = false;
                        closingParams.IsDragDrop = false;
                        closingParams.IsFromServer = false;
                        closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                        closingParams.IsVirtualClosingPopulate = true;
                        closingParams.IsOverrideWithUserClosing = false;
                        closingParams.IsMatchStrategy = !chkMatchStrategy.Checked;
                        closingParams.ClosingField = _closingField;
                        closingParams.IsCopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;
                        ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                        //ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(listPositionalTaxLots, listClosingTaxLots, _closingAlgo, false, false, false, false, false, PostTradeEnums.SecondarySortCriteria.None, true, false, !chkMatchStrategy.Checked);
                    }
                    else
                    {
                        closingParams.BuyTaxLotsAndPositions = listPositionalTaxLots;
                        closingParams.SellTaxLotsAndPositions = listClosingTaxLots;
                        closingParams.Algorithm = _closingAlgo;
                        closingParams.IsShortWithBuyAndBuyToCover = false;
                        closingParams.IsSellWithBuyToClose = false;
                        closingParams.IsManual = true;
                        closingParams.IsDragDrop = false;
                        closingParams.IsFromServer = false;
                        closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                        closingParams.IsVirtualClosingPopulate = true;
                        closingParams.IsOverrideWithUserClosing = false;
                        closingParams.IsMatchStrategy = !chkMatchStrategy.Checked;
                        closingParams.ClosingField = _closingField;
                        closingParams.IsCopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;
                        ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                        // ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(listPositionalTaxLots, listClosingTaxLots, _closingAlgo, false, false, true, false, false, PostTradeEnums.SecondarySortCriteria.None, true, false, !chkMatchStrategy.Checked);
                    }
                }
                bool isError = CheckForCloseTradeError(ClosedData);

                if (!isError)
                {
                    Dictionary<string, double> dictClosedPositionalTaxlots = GetPopulatedClosedQtyForTaxlots(ClosedData);

                    PopulateCloseQty(dictClosedPositionalTaxlots);
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
        /// clear all the populated closed quantity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                _dictAccountWiseQtyToClose.Clear();
                foreach (UltraGridRow row in grdOpenTrades.Rows.GetFilteredInNonGroupByRows())
                {
                    row.Cells[ClosingConstants.COLUMN_QUANTITYTOCLOSE].Value = 0;
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

        #endregion

        /// <summary>
        /// close the populated open positions with the given closed quantity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _isCloseTradeInitialized = true;
                ClosingData ClosedData = new ClosingData();
                List<TaxLot> buyTaxLotsAndPositions = GetPositionalTaxlotsFromPositionGrid();
                //ClosingClientSideMapper.AvailableTaxlotsQuantity

                //Closing algorithm will set when clicking on close button.
                //PostTradeEnums.CloseTradeAlogrithm _closingAlgo = PostTradeEnums.CloseTradeAlogrithm.NONE;
                //if (cmbClosingMethodlogy.Value != null)
                //_closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)(cmbClosingMethodlogy.Value);

                RemovePositionalTradesWithZeroQtyToCloseAndWithGreaterTradeDate(ref buyTaxLotsAndPositions);

                List<TaxLot> SellTaxLotsAndPositions = GetClosingTaxlotsList();

                //Fill TaxlotQtyToClose in closing taxlots
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5723
                FillTaxlotQtyToClose(buyTaxLotsAndPositions, SellTaxLotsAndPositions);

                List<TaxLot> listPositionalTaxLots;
                List<TaxLot> listClosingTaxLots;
                //in case of closig side BuyToClose taxlots will be reversed i.e. buy taxlots will be sell taxlots and vice versa
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2306
                if (lblSide.Text.ToUpper().Replace(" ", string.Empty).Equals((SIDE_BUYTOCLOSE)))
                {
                    listPositionalTaxLots = SellTaxLotsAndPositions;
                    listClosingTaxLots = buyTaxLotsAndPositions;
                }
                else
                {
                    listPositionalTaxLots = buyTaxLotsAndPositions;
                    listClosingTaxLots = SellTaxLotsAndPositions;
                }
                if (listPositionalTaxLots.Count > 0 && listClosingTaxLots.Count > 0)
                {
                    //buyTaxLotsAndPositions, sellTaxLotsAndPositions, algorithm = selected from algo derpdown, IsShortWithBuyAndBuyToCover=false, IsSellWithBuyToClose = false, isManual = true(not from preset algo), isDragDrop = false, isFromServer = false, SecondarySortCriteria=None, isVirtualClosingPopulate = false, isOverrideWithUserClosing = true, isMatchStrategy=according to checkbox  

                    ClosingParameters closingParams = new ClosingParameters();
                    closingParams.BuyTaxLotsAndPositions = listPositionalTaxLots;
                    closingParams.SellTaxLotsAndPositions = listClosingTaxLots;
                    closingParams.Algorithm = _closingAlgo;
                    closingParams.IsShortWithBuyAndBuyToCover = false;
                    closingParams.IsSellWithBuyToClose = false;
                    closingParams.IsManual = false;
                    closingParams.IsDragDrop = false;
                    closingParams.IsFromServer = false;
                    closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                    closingParams.IsVirtualClosingPopulate = false;
                    closingParams.IsOverrideWithUserClosing = true;
                    closingParams.IsMatchStrategy = !chkMatchStrategy.Checked;
                    closingParams.ClosingField = _closingField;
                    closingParams.IsCopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;
                    ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                    // ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(listPositionalTaxLots, listClosingTaxLots, _closingAlgo, false, false, false, false, false, PostTradeEnums.SecondarySortCriteria.None, false, true, !chkMatchStrategy.Checked);

                    bool isError = CheckForCloseTradeError(ClosedData);

                    if (!isError && ClosedData.ClosedPositions.Count > 0)
                    {
                        //UpdateClosingTaxlots(ClosedData.Taxlots);
                        //UpdateSellOpenQtyAccountAndStrategyWise();
                        UpdateAllocatedAndAvailableQuantityAtClosingAndUnwinding(ClosedData, 0);
                        //UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(cmbAccounts.Text,cmbStrategy.Text);
                        InformationMessageBox.Display("Close Trade Data Saved");
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
        /// Update closing taxlots when closing is done
        /// </summary>
        /// <param name="closingTaxLots"></param>
        public void UpdateClosingTaxlots(List<TaxLot> listTaxLots)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            UpdateClosingTaxlots(listTaxLots);

                        }));
                    }
                    else
                    {
                        foreach (TaxLot taxLot in listTaxLots)
                        {
                            if (_dictSellTaxLotsAndPositions.ContainsKey(taxLot.TaxLotID))
                            {
                                if (taxLot.ClosingStatus == ClosingStatus.Closed)
                                    _dictSellTaxLotsAndPositions.Remove(taxLot.TaxLotID);
                                else
                                    _dictSellTaxLotsAndPositions[taxLot.TaxLotID] = taxLot;
                            }
                            else
                            {
                                if (taxLot.GroupID.Equals(ClosingClientSideMapper.GroupId) && !(taxLot.ClosingStatus == ClosingStatus.Closed))
                                {
                                    _dictSellTaxLotsAndPositions.Add(taxLot.TaxLotID, taxLot);
                                }
                            }
                        }
                        //RemoveZeroOpenQtyClosingTaxlots();
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
        /// Update Allocated And Available Quantity Based on Closing and Unwinding
        /// </summary>
        /// <param name="closingData"></param>
        /// <param name="QtyUnwindTaxLots"></param>
        private void UpdateAllocatedAndAvailableQuantityAtClosingAndUnwinding(ClosingData closingData, double QtyUnwindTaxLots)
        {
            try
            {
                double QtyAllocated = 0;
                double QtyAvailable = 0;

                CalculateCloseAndAvailableQty(cmbAccounts.Text, cmbStrategy.Text, null, null, ref QtyAvailable, ref QtyAllocated);

                if (closingData != null && closingData.ClosedPositions.Count > 0)
                {
                    foreach (Position pos in closingData.ClosedPositions)
                    {
                        QtyAvailable += pos.ClosedQty;
                        if (_dictAccountWiseQtyToClose.ContainsKey(pos.AccountValue.FullName))
                        {
                            _dictAccountWiseQtyToClose[pos.AccountValue.FullName] -= pos.ClosedQty;
                        }
                    }
                    QtyAvailable = QtyAvailable * (-1);
                }
                else
                {
                    QtyAvailable = QtyUnwindTaxLots;
                    lblAllocatedQty.Text = QtyAllocated.ToString();
                }
                //lblAllocatedQty.Text = QtyAllocated.ToString();
                //lblOpenLots.Text = Convert.ToString(double.Parse(string.IsNullOrEmpty(lblOpenLots.Text) ? "0" : lblOpenLots.Text) + QtyAvailable);
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
        /// Update available lots quantity from parent form close trade
        /// </summary>
        public void UpdateAllocatedAndAvailableQtyFromParentForm()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(cmbAccounts.Text, cmbStrategy.Text);
                        }));
                    }
                    else
                    {
                        UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(cmbAccounts.Text, cmbStrategy.Text);
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
        /// Update Allocated And Available Quantity Based On Filter of Account and Strategy
        /// </summary>
        /// <param name="AccountFilter"></param>
        /// <param name="StrategyFilter"></param>
        private void UpdateAllocatedAndAvailableQtyBasedOnAccountAndStrategyFilter(string AccountFilter, string StrategyFilter)
        {
            try
            {
                List<TaxLot> listClosingTaxLots = GetClosingTaxlotsList();
                List<TaxLot> listPositionalTaxLots = GetPositionalTaxlotsFromPositionGrid();
                //_dictClosingQuantityAccountAndStrategyWise
                double AvailableQty = 0;
                double AllocatedQty = 0;

                CalculateCloseAndAvailableQty(AccountFilter, StrategyFilter, listClosingTaxLots, listPositionalTaxLots, ref AvailableQty, ref AllocatedQty);

                lblAllocatedQty.Text = AllocatedQty.ToString();
                lblOpenLots.Text = AvailableQty.ToString();
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


        private void CalculateCloseAndAvailableQty(string AccountFilter, string StrategyFilter, List<TaxLot> listClosingTaxLots, List<TaxLot> listPositionalTaxLots, ref double AvailableQty, ref double AllocatedQty)
        {
            try
            {
                //both filters accounts and strategy are applied
                if (!AccountFilter.Equals(FILTER_ALL) && !StrategyFilter.Equals(FILTER_ALL))
                {
                    if (_dictClosingQuantityAccountAndStrategyWise.ContainsKey(AccountFilter + Seperators.SEPERATOR_6 + StrategyFilter))
                    {
                        AllocatedQty = _dictClosingQuantityAccountAndStrategyWise[AccountFilter + Seperators.SEPERATOR_6 + StrategyFilter];
                    }
                    if (listClosingTaxLots != null)
                    {
                        foreach (TaxLot taxLot in listPositionalTaxLots)
                        {
                            if (AccountFilter.Equals(taxLot.Level1Name) && StrategyFilter.Equals(taxLot.Level2Name))
                            {
                                AvailableQty += taxLot.TaxLotQty;
                                AllocatedQty -= taxLot.TaxLotQtyToClose;
                            }
                        }
                    }
                }
                //if only account filter applied
                else if (!AccountFilter.Equals(FILTER_ALL) && StrategyFilter.Equals(FILTER_ALL))
                {
                    foreach (KeyValuePair<string, double> kvp in _dictClosingQuantityAccountAndStrategyWise)
                    {
                        string[] key = kvp.Key.Split(Seperators.SEPERATOR_6);
                        if (AccountFilter.Equals(key[0]))
                        {
                            AllocatedQty += kvp.Value;
                        }
                    }
                    if (listPositionalTaxLots != null)
                    {
                        foreach (TaxLot taxLot in listPositionalTaxLots)
                        {
                            if (AccountFilter.Equals(taxLot.Level1Name))
                            {
                                AvailableQty += taxLot.TaxLotQty;
                                AllocatedQty -= taxLot.TaxLotQtyToClose;
                            }
                        }
                    }
                }
                //if only strategy filter applied
                else if (AccountFilter.Equals(FILTER_ALL) && !StrategyFilter.Equals(FILTER_ALL))
                {
                    foreach (KeyValuePair<string, double> kvp in _dictClosingQuantityAccountAndStrategyWise)
                    {
                        string[] key = kvp.Key.Split(Seperators.SEPERATOR_6);
                        if (StrategyFilter.Equals(key[1]))
                        {
                            AllocatedQty += kvp.Value;
                        }
                    }
                    if (listPositionalTaxLots != null)
                    {
                        foreach (TaxLot taxLot in listPositionalTaxLots)
                        {
                            if (StrategyFilter.Equals(taxLot.Level2Name))
                            {
                                AvailableQty += taxLot.TaxLotQty;
                                AllocatedQty -= taxLot.TaxLotQtyToClose;
                            }
                        }
                    }
                }
                //no filter applied
                else
                {
                    foreach (KeyValuePair<string, double> kvp in _dictClosingQuantityAccountAndStrategyWise)
                    {
                        AllocatedQty += kvp.Value;
                    }
                    if (listPositionalTaxLots != null)
                    {
                        foreach (TaxLot taxLot in listPositionalTaxLots)
                        {
                            AvailableQty += taxLot.TaxLotQty;
                            AllocatedQty -= taxLot.TaxLotQtyToClose;
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

        #region Unwinding Methods

        private void unwindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UnwindSelectedTaxlots();
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

        private void UnwindSelectedTaxlots()
        {
            try
            {
                UltraGridRow[] filteredRows = grdCloseTrades.Rows.GetFilteredInNonGroupByRows();
                double QtyToUnwind = 0;
                GenericBindingList<Position> posList = new GenericBindingList<Position>();

                Dictionary<string, DateTime> dictTaxlotIds = new Dictionary<string, DateTime>();
                foreach (UltraGridRow row in filteredRows)
                {
                    if (row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                    {
                        Position pos = (Position)row.ListObject;
                        posList.Add((Position)row.ListObject);
                        if (!dictTaxlotIds.ContainsKey(pos.ID))
                            dictTaxlotIds.Add(pos.ID, pos.ClosingTradeDate);
                        if (!dictTaxlotIds.ContainsKey(pos.ClosingID))
                            dictTaxlotIds.Add(pos.ClosingID, pos.ClosingTradeDate);
                    }
                }
                if (posList.Count == 0)
                {
                    return;
                }
                // Disable unwinding for cost adjusted trades
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7102
                foreach (UltraGridRow row in filteredRows)
                {
                    Position selectedPos = (Position)row.ListObject;
                    if (selectedPos.ClosingMode == ClosingMode.CostBasisAdjustment)
                    {
                        MessageBox.Show(this, "Cannot unwind data for Cost Adjustment taxlots.", "Warning!", MessageBoxButtons.OK);
                        return;
                    }
                }

                UltraGridRow[] filteredRowsNew = new UltraGridRow[filteredRows.Length];
                filteredRows.CopyTo(filteredRowsNew, 0);
                StringBuilder s = new StringBuilder();
                StringBuilder taxlotId = new StringBuilder();
                StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
                List<string> taxlotidList = new List<string>();
                DialogResult userChoice = MessageBox.Show("This will unwind the closing,Would you like to proceed ?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (userChoice == DialogResult.Yes)
                {
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            //if (DisableEnableParentForm != null)
                            //{
                            //    _statusMessage = "Unwinding Data Please Wait";
                            //    this.BeginInvoke(DisableEnableParentForm, _statusMessage, false, true);
                            //    //DisableEnableParentForm(_statusMessage, false, true);
                            //}
                        }
                    }
                    StringBuilder message = new StringBuilder();

                    Dictionary<string, StatusInfo> positionStatusDict = _closingServices.InnerChannel.ArePositionEligibletoUnwind(dictTaxlotIds);

                    foreach (Position position in posList)
                    {
                        if (position != null)
                        {
                            //Available taxlotQty will be updated iff unwind positions are of same taxlot as of closing group/taxlot
                            if (position.Symbol.Equals(lblSymbol.Text))
                                QtyToUnwind += position.ClosedQty;
                            if (!((positionStatusDict.ContainsKey(position.ID)) || (positionStatusDict.ContainsKey(position.ClosingID))))
                            {
                                s.Append(position.TaxLotClosingId.ToString());
                                s.Append(",");

                                taxlotClosingIDWithClosingDate.Append(position.TaxLotClosingId.ToString());
                                taxlotClosingIDWithClosingDate.Append('_');
                                taxlotClosingIDWithClosingDate.Append(position.ClosingTradeDate.ToString());
                                taxlotClosingIDWithClosingDate.Append('_');
                                taxlotClosingIDWithClosingDate.Append(position.AccountValue.ID.ToString());
                                taxlotClosingIDWithClosingDate.Append(",");
                                //positionTowind.Add(position);
                                if (!taxlotidList.Contains(position.ID.ToString()))
                                {
                                    taxlotId.Append(position.ID.ToString());
                                    taxlotId.Append(",");
                                }
                                if (!taxlotidList.Contains(position.ClosingID.ToString()))
                                {
                                    taxlotId.Append(position.ClosingID.ToString());
                                    taxlotId.Append(",");
                                }
                            }
                            else
                            {
                                if (positionStatusDict.ContainsKey(position.ID) || positionStatusDict.ContainsKey(position.ClosingID))
                                {
                                    if ((positionStatusDict.ContainsKey(position.ID) && positionStatusDict[position.ID].Status.Equals(PostTradeEnums.Status.CorporateAction)) || (positionStatusDict.ContainsKey(position.ClosingID) && positionStatusDict[position.ClosingID].Status.Equals(PostTradeEnums.Status.CorporateAction)))
                                    {
                                        message.Append("TaxlotID : ");
                                        message.Append(position.ID.ToString());
                                        message.Append(",");
                                        message.Append(position.ClosingID.ToString());
                                        message.Append(" has corporate action on future date,First undo Corporate action then unwind");
                                        message.Append(Environment.NewLine);
                                    }
                                }
                                if (positionStatusDict.ContainsKey(position.ID))
                                {
                                    foreach (KeyValuePair<string, PostTradeEnums.Status> kp in positionStatusDict[position.ID].ExercisedUnderlying)
                                    {
                                        message.Append("Exercised Underlying IDs : ");
                                        if (positionStatusDict[position.ID].ExercisedUnderlying.Keys.Count > 0)
                                        {
                                            foreach (string key in positionStatusDict[position.ID].ExercisedUnderlying.Keys)
                                            {
                                                string id = key;
                                                message.Append(id);
                                                message.Append(" , ");
                                            }
                                            message.Append("generated by TaxlotID : ");
                                            message.Append(position.ID.ToString());
                                            message.Append("  is closed, First unwind the underlying to continue");
                                            message.Append(Environment.NewLine);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (message.Length > 0)
                    {
                        MessageBox.Show(message.ToString(), "Close trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        Dictionary<string, StatusInfo> dictFutureDateClosedInfo = _closingServices.InnerChannel.GetFutureDateClosingInfo(s.ToString());
                        if (dictFutureDateClosedInfo != null && dictFutureDateClosedInfo.Count > 0)
                        {
                            foreach (KeyValuePair<string, StatusInfo> kp in dictFutureDateClosedInfo)
                            {
                                message.Append(kp.Value.Details);
                                if (kp.Value.Status.Equals(PostTradeEnums.Status.CorporateAction))
                                {
                                    message.Append("  (Corporate Action)");
                                }
                                else
                                {
                                    message.Append("  (closed)");
                                }
                                message.Append(Environment.NewLine);
                            }

                            message.Append("in future date time. First unwind.");

                            MessageBox.Show(message.ToString(), "Close trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            ClosingData closingData = _allocationServices.InnerChannel.UnWindClosing(s.ToString(), taxlotId.ToString(), taxlotClosingIDWithClosingDate.ToString());
                            //UpdateClosingTaxlots(closingData.Taxlots);
                            //UpdateSellOpenQtyAccountAndStrategyWise();
                            UpdateAllocatedAndAvailableQuantityAtClosingAndUnwinding(closingData, QtyToUnwind);
                        }
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveLayout != null)
                {
                    SaveLayout(null, EventArgs.Empty);
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

        public UltraGrid GetCloseOrderGrid()
        {
            try
            {
                return grdCloseTrades;
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
            return null;
        }

        #endregion

        #region BackGroundWorker to Fetch Data Events

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        void _bgFetchData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgFetchData.CancellationPending)//checks for cancel request
                {
                    object[] arguments = e.Argument as object[];
                    AllocationGroup group = arguments[0] as AllocationGroup;
                    string accountIDs = arguments[1] as string;


                    Logger.LoggerWrite("Start: Data fetching from database for Close Order i.e. Close Order tab", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    stopwatch.Start();

                    ClosingData ClosingData = ClosingClientSideMapper.GetClosingDataForASymbol(group.Symbol, _closingServices, accountIDs, group.OrderSideTagValue, group.GroupID);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    Logger.LoggerWrite("End: Data fetched from database for Close Order UI i.e. Close Order tab time (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                    object[] result = new object[2];
                    result[0] = group;
                    result[1] = ClosingData;
                    e.Result = result;
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

        void _bgFetchData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Fetch Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Result != null)
                {
                    Logger.LoggerWrite("Start: After Data fetching from db for Close Order, cache initilization starts ", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    stopwatch.Start();

                    object[] arguments = e.Result as object[];
                    AllocationGroup group = arguments[0] as AllocationGroup;
                    ClosingData ClosingData = arguments[1] as ClosingData;

                    List<TaxLot> ClosingTaxLots = new List<TaxLot>();

                    if (ClosingData != null)
                    {
                        ClosingClientSideMapper.CreateRepository(ClosingData);
                    }
                    foreach (TaxLot taxlot in ClosingData.TaxlotsToPopulate)
                    {
                        //taxlots with the same group id to group should be added to the closingtaxlots
                        if (taxlot.GroupID.Equals(group.GroupID))
                            ClosingTaxLots.Add(taxlot);
                        //taxlots with the same closing side(Sell/BuyToCover) should not be shown at positional taxlot grid
                        else if (!taxlot.OrderSideTagValue.Equals(group.OrderSideTagValue))
                            ClosingClientSideMapper.OpenTaxlotsToPopulate.Add(taxlot);
                    }

                    SetClosingTaxlotsList(ClosingTaxLots);
                    UpdateSellOpenQtyAccountAndStrategyWise();
                    SetControlsDataSource(group);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    Logger.LoggerWrite("End: After Data fetching from db for Close Order, cache initilization completed time (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

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
            finally
            {
                if (DisableEnableParentForm != null)
                {
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(string.Empty, true, false));
                }
                this.grdCloseTrades.Enabled = true;
                this.grdCloseTrades.ResumeRowSynchronization();
                this.grdCloseTrades.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdCloseTrades);

                this.grdOpenTrades.Enabled = true;
                this.grdOpenTrades.ResumeRowSynchronization();
                this.grdOpenTrades.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdOpenTrades);
            }
        }

        #endregion

        private void ctrlCloseTradefromAllocation_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    SetButtonsColor();
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);

                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        this.grpBoxGroupAttributes.Appearance.BackColor = System.Drawing.Color.Black;
                    }
                    Logger.LoggerWrite("Start: Load method for close order UI", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    stopwatch.Start();
                    if (_closingServices != null)
                    {
                        _closingPreferences = _closingServices.InnerChannel.GetPreferences();
                    }
                    chkCopyOpeningTradeAttributes.Checked = _closingPreferences.CopyOpeningTradeAttributes;
                    BindCombos();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    Logger.LoggerWrite("End: Load method for close order UI (in Milliseconds): " + ts.TotalMilliseconds, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
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

        private void SetButtonsColor()
        {
            try
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClear.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClear.ForeColor = System.Drawing.Color.White;
                btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        internal void SetControlsAsReadOnly()
        {
            try
            {
                grdCloseTrades.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                grdOpenTrades.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                btnClear.Enabled = false;
                btnClose.Enabled = false;
                btnSave.Enabled = false;
                cmbClosingMethodlogy.Enabled = false;
                cmbAccounts.Enabled = false;
                cmbStrategy.Enabled = false;
                cmbClosingField.Enabled = false;
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
        /// Fill TaxlotQty to close in closing taxlots
        /// </summary>
        /// <param name="listPositionalTaxLots"></param>
        /// <param name="listClosingTaxLots"></param>
        private void FillTaxlotQtyToClose(List<TaxLot> listPositionalTaxLots, List<TaxLot> listClosingTaxLots)
        {
            try
            {
                Dictionary<int, double> dictAccountIdWithQty = new Dictionary<int, double>();
                foreach (TaxLot taxLot in listPositionalTaxLots)
                {
                    if (!dictAccountIdWithQty.ContainsKey(taxLot.Level1ID))
                    {
                        dictAccountIdWithQty.Add(taxLot.Level1ID, taxLot.TaxLotQtyToClose);
                    }
                    else
                    {
                        dictAccountIdWithQty[taxLot.Level1ID] += taxLot.TaxLotQtyToClose;
                    }
                }
                foreach (TaxLot taxLot in listClosingTaxLots)
                {
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-5655
                    //As taxlotQtyToClose comes 0 when user click save button on close order UI.
                    //we have to fill taxlotQtyToClose for closing taxlot too because we use this in post recon amendments UI.
                    //From post recon amendment UI we are sending taxLot.TaxLotQtyToClose already filled so apply extra check to check that TaxLotQtyToClose is zero or not 
                    if (dictAccountIdWithQty.ContainsKey(taxLot.Level1ID) && taxLot.TaxLotQtyToClose == 0)
                    {
                        if (taxLot.TaxLotQty < dictAccountIdWithQty[taxLot.Level1ID])
                            taxLot.TaxLotQtyToClose = taxLot.TaxLotQty;
                        else
                            taxLot.TaxLotQtyToClose = dictAccountIdWithQty[taxLot.Level1ID];
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

        private void grdCloseTrades_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (grdCloseTrades.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdCloseTrades);
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

        void grd_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void grdOpenTrades_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(ClosingConstants.COL_TradeDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdOpenTrades.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdOpenTrades.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        void grdCloseTrades_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            if ((e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
            {
                grdCloseTrades.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                grdCloseTrades.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
            }
        }

        public UltraGrid GetGridInstance()
        {
            return grdOpenTrades;
        }

        private void cmbClosingField_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbClosingField);
                toolTipInfo.ToolTipText = cmbClosingField.Text;
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbClosingMethodlogy_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbClosingMethodlogy);
                toolTipInfo.ToolTipText = cmbClosingMethodlogy.Text;
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbStrategy_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbStrategy);
                toolTipInfo.ToolTipText = cmbStrategy.Text;
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbAccounts_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbAccounts);
                toolTipInfo.ToolTipText = cmbAccounts.Text;
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCloseTrades_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        public void ExportDataForAutomation(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "grdOpenTrades")
                {
                    exporter.Export(grdOpenTrades, filePath);
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
