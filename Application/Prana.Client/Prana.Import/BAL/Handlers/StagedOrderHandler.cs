using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import.Helper;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Prana.Import
{
    sealed public class StagedOrderHandler : IImportHandler, IImportITaskHandler, IDisposable
    {

        private static volatile StagedOrderHandler instance;
        private static object syncRoot = new Object();


        public event System.EventHandler<EventArgs<TaskResult>> ImportCompleted;


        private StagedOrderHandler() { }

        public static StagedOrderHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new StagedOrderHandler();

                        }
                    }
                }

                return instance;
            }
        }

        #region LocalVariables
        Dictionary<string, StageImportDataList> _collection = new Dictionary<string, StageImportDataList>();
        int _importedFileId = 0;
        string _importedFileName = string.Empty;

        /// <summary>
        /// Scheme ID
        /// </summary>
        private int _allocationSchemeID = 0;

        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        bool _isSaveDataInApplication = false;
        RunUpload _runUpload = null;
        TaskResult _currentResult = new TaskResult();
        bool _isAutoImport = false;
        Dictionary<string, OrderSingle> _dictRequestedSymbol = new Dictionary<string, OrderSingle>();

        List<OrderSingle> _stagedOrderCollection = null;

        /// <summary>
        /// Scheme ID
        /// </summary>
        public int AllocationSchemeID
        {
            get
            {
                return _allocationSchemeID;
            }
            set
            {
                _allocationSchemeID = value;
            }
        }

        public List<OrderSingle> StagedOrderCollection
        {
            get
            {
                return _stagedOrderCollection;
            }
            set
            {
                _stagedOrderCollection = value;
            }
        }

        List<OrderSingle> _validatedStagedOrderCollection = new List<OrderSingle>();
        public List<OrderSingle> ValidatedStagedOrderCollection
        {
            get
            {
                return _validatedStagedOrderCollection;
            }
            set
            {
                _validatedStagedOrderCollection = value;
            }
        }

        List<SecMasterBaseObj> _secMasterResponseCollection = new List<SecMasterBaseObj>();
        public List<SecMasterBaseObj> SecMasterResponseCollection
        {
            get
            {
                return _secMasterResponseCollection;
            }
            set
            {
                _secMasterResponseCollection = value;
            }
        }

        private System.Timers.Timer _timerSecurityValidation;
        private static object _timerLock = new Object();

        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;

        /// <summary>
        /// Event handler to generate new Allocation scheme.
        /// </summary>
        public event EventHandler<EventArgs<DataSet, string, List<SecMasterBaseObj>>> GenerateNewAllocationScheme;

        #endregion

        #region Events

        /// <summary>
        /// Creates and Allocates scheme
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="Filename"></param>
        private void GenerateAllocationScheme(DataSet ds, string Filename, List<SecMasterBaseObj> secMasterResponseCollection)
        {
            try
            {
                if (GenerateNewAllocationScheme != null)
                {
                    GenerateNewAllocationScheme(this, new EventArgs<DataSet, string, List<SecMasterBaseObj>>(ds, Filename, secMasterResponseCollection));
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
        /// wires event  
        /// </summary>
        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
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
        /// UnWires the event
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                //SetReImportVariables(string.Empty, false);

                _timerSecurityValidation.Elapsed -= new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
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

        public void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                //UpdateCollection(e.Value, string.Empty);
                lock (syncRoot)
                {
                    _secMasterResponseCollection.Add(e.Value);

                    if (e.Value.AUECID > 0)
                    {
                        _countValidatedSymbols++;
                    }
                    else
                    {
                        _countNonValidatedSymbols++;
                    }
                    //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
                    //This may be possible that we get response for all the symbols for but some securities are not validated
                    //e.g. Side not picking in file, trade will be invalidated
                    if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
                    {
                        _timerSecurityValidation.Stop();
                        TimerSecurityValidation_Tick(this, null);
                    }
                    else
                    {
                        ResetTimer();
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
        /// http://msdn.microsoft.com/en-us/library/ak9w5846.aspx
        /// </summary>
        object objectLock = new Object();
        event EventHandler IImportITaskHandler.UpdateTaskSpecificDataPoints
        {
            add
            {
                lock (objectLock)
                {
                    if (TaskSpecificDataPointsPreUpdate == null || !TaskSpecificDataPointsPreUpdate.GetInvocationList().Contains(value))
                    {
                        TaskSpecificDataPointsPreUpdate += value;
                    }
                }
            }
            remove
            {
                lock (objectLock)
                {
                    TaskSpecificDataPointsPreUpdate -= value;
                }
            }
        }
        #endregion

        #region IImportHandler Members
        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _runUpload = runUpload;
                _currentResult = taskResult as TaskResult;
                _isSaveDataInApplication = isSaveDataInApplication;
                if (_currentResult != null)
                    _isAutoImport = Convert.ToBoolean(_currentResult.TaskStatistics.TaskSpecificData.GetValueForKey("IsAutoImport"));
                //_executionName = Path.GetFileName(_currentResult.GetDashBoardXmlPath());
                //_dashboardXmlDirectoryPath = Path.GetDirectoryName(_currentResult.GetDashBoardXmlPath());
                //if (!Directory.Exists(Application.StartupPath + _dashboardXmlDirectoryPath))
                //{
                //    Directory.CreateDirectory(Application.StartupPath + _dashboardXmlDirectoryPath);
                //}
                //_refDataDirectoryPath = _dashboardXmlDirectoryPath + @"\RefData";
                //if (!Directory.Exists(Application.StartupPath + _refDataDirectoryPath))
                //{
                //    Directory.CreateDirectory(Application.StartupPath + _refDataDirectoryPath);
                //}
                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);

                SaveImportedFileDetails(runUpload);
               
                SetCollection(ds.Tables[0], runUpload);
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
                else
                {
                    ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.EXCEPTION, ex.Message);
                }
            }
        }

        public void UpdateCollection(SecMasterBaseObj secMasterObj, string key)
        {
            try
            {
                int requestedSymbologyID = secMasterObj.RequestedSymbology;
                string symbol = secMasterObj.RequestedSymbol;
                //It must be in lock as update will be come from SecurityMasterManager's OnResponse event
                lock (syncRoot)
                {
                    List<OrderSingle> stagedOrders = new List<OrderSingle>();
                    if (!string.IsNullOrEmpty(key) && _collection.ContainsKey(key) && _collection[key] != null && _collection[key].Contains(requestedSymbologyID, symbol))
                    {
                        //get the OrderSingleList for the Symbology and Symbol and save to Stagedorders
                        stagedOrders = _collection[key].getStageImportData(requestedSymbologyID, symbol).getOrderSingleList();
                        EnrichAndSaveCollection(secMasterObj, stagedOrders);
                        _collection[key].Remove(requestedSymbologyID, symbol);
                    }
                    else
                    {
                        foreach (KeyValuePair<string, StageImportDataList> kvp in _collection)
                        {
                            if (kvp.Value != null && kvp.Value.Contains(requestedSymbologyID, symbol))
                            {
                                //get the OrderSingleList for the Symbology and Symbol and save to Stagedorders
                                stagedOrders = kvp.Value.getStageImportData(requestedSymbologyID, symbol).getOrderSingleList();
                                EnrichAndSaveCollection(secMasterObj, stagedOrders);
                                //kvp.Value.SymbolWiseStagedOrderCollection.Remove(secMasterObj.TickerSymbol);

                                #region remove elements from _dictRequestedSymbol for which we have received response
                                string requestedKey = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + symbol;
                                if (_dictRequestedSymbol.ContainsKey(requestedKey))
                                {
                                    _dictRequestedSymbol.Remove(requestedKey);
                                }
                                #endregion

                                //#region to check account lock
                                //bool accountLocked = true;
                                //foreach (OrderSingle orderSingleObj in stagedOrders)
                                //{
                                //    if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                                //    {
                                //        accountLocked = CachedDataManager.GetInstance.isAccountLocked(orderSingleObj.Level1ID);
                                //    }
                                //    else
                                //    {
                                //        accountLocked = true;
                                //    }
                                //    if (!accountLocked && orderSingleObj.Level1ID != int.MinValue)
                                //    {
                                //        if (!Regex.IsMatch(orderSingleObj.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                                //        {
                                //            orderSingleObj.ValidationError += "Account Lock Required";
                                //        }
                                //        orderSingleObj.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                //    }
                                //    else if (Regex.IsMatch(orderSingleObj.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                                //    {
                                //        if (orderSingleObj.ValidationError.Length > "Account Lock Required".Length)
                                //        {
                                //            List<string> valerror = orderSingleObj.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                                //            valerror.Remove("Account Lock Required");
                                //            orderSingleObj.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                                //        }
                                //        else
                                //        {
                                //            orderSingleObj.ValidationError = string.Empty;
                                //            orderSingleObj.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                                //        }
                                //    }
                                //}
                                //#endregion
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

        public string GetXSDName()
        {
            return "ImportBlotterTrades.xsd";
        }

        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            return new DataTable();
        }
        #endregion

        #region Private Methods
        private void SaveImportedFileDetails(RunUpload runUpload)
        {
            try
            {
                _importedFileName = runUpload.FileName;
                _importedFileId = ImportDataManager.SaveImportedFileDetails(runUpload.FileName, runUpload.FilePath, runUpload.ImportTypeAcronym, runUpload.FileLastModifiedUTCTime);
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
        public SecMasterRequestObj SecMasterRequestObj;

        private void SetCollection(DataTable outputDataTable, RunUpload runUpload)
        {
            try
            {
                _collection.Clear();
                _dictRequestedSymbol.Clear();
                _stagedOrderCollection = new List<OrderSingle>();
                _validatedStagedOrderCollection = new List<OrderSingle>();
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                _countSymbols = 0;
                _countValidatedSymbols = 0;

                if (outputDataTable != null)
                {
                    lock (syncRoot)
                    {
                        SecMasterRequestObj = new SecMasterRequestObj();
                        if (!_collection.ContainsKey(runUpload.Key))
                        {
                            _collection.Add(runUpload.Key, new StageImportDataList());

                            for (int counter = 0; counter < outputDataTable.Rows.Count; counter++)
                            {
                                ApplicationConstants.SymbologyCodes code = ApplicationConstants.SymbologyCodes.TickerSymbol;
                                if (outputDataTable.Columns.Contains("Symbology"))
                                    code = (ApplicationConstants.SymbologyCodes)Enum.Parse(typeof(ApplicationConstants.SymbologyCodes), outputDataTable.Rows[counter]["Symbology"].ToString());
                                int symbology = (int)code;

                                OrderSingle order = GetOrderFromDataRow(outputDataTable.Rows[counter]);
                                order.RowIndex = counter;
                                order.ImportStatus = ImportStatus.NotImported.ToString();
                                order.TransactionSource = TransactionSource.TradeImport;
                                order.TransactionSourceTag = (int)TransactionSource.TradeImport;

                                if (order.Symbol != string.Empty)
                                {
                                    if (!_collection[runUpload.Key].Contains(symbology, order.Symbol))
                                    {
                                        _countSymbols++;
                                        SecMasterRequestObj.AddData(order.Symbol.Trim(), code);
                                    }
                                    AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();
                                    _collection[runUpload.Key].Add(symbology, order.Symbol, order, accountCollection);
                                    if (!_dictRequestedSymbol.ContainsKey(symbology.ToString() + Seperators.SEPERATOR_6 + order.Symbol))
                                    {
                                        _dictRequestedSymbol.Add((symbology.ToString() + Seperators.SEPERATOR_6 + order.Symbol), order);
                                    }
                                }

                                if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                                {
                                    DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                                    order.TransactionTime = Convert.ToDateTime(dtn.ToShortDateString());
                                }

                                if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                                {
                                    order.Level1ID = runUpload.SelectedAccount;
                                }

                                _stagedOrderCollection.Add(order);
                            }

                            ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName,
                          string.Empty, "Sending SM validation request from StagedOrderHandler");
                            GetSMDataForStagedOrder(SecMasterRequestObj);



                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _collection.Clear();
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                else
                {
                    ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.EXCEPTION, ex.Message);
                }
            }
        }

        /// <summary>
        /// Arranges the dataset for use as scheme
        /// </summary>
        /// <param name="schemeSet"></param>
        private void ArrangeDataSet(DataSet schemeSet)
        {
            try
            {
                foreach (DataTable table in schemeSet.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        row["FundName"] = CachedDataManager.GetInstance.GetAccount(int.Parse(row["FundID"].ToString()));
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

        private void EnrichAndSaveCollection(SecMasterBaseObj secMasterObj, List<OrderSingle> stagedOrders)
        {
            try
            {
                foreach (OrderSingle stageOrder in stagedOrders)
                {
                    stageOrder.AssetID = secMasterObj.AssetID;
                    stageOrder.UnderlyingID = secMasterObj.UnderLyingID;
                    stageOrder.ExchangeID = secMasterObj.ExchangeID;
                    stageOrder.CurrencyID = secMasterObj.CurrencyID;
                    stageOrder.AUECID = secMasterObj.AUECID;
                    stageOrder.Symbol = secMasterObj.TickerSymbol;
                    stageOrder.BloombergSymbol = secMasterObj.BloombergSymbol;
                    stageOrder.ISINSymbol = secMasterObj.ISINSymbol;
                    stageOrder.CusipSymbol = secMasterObj.CusipSymbol;
                    stageOrder.SEDOLSymbol = secMasterObj.SedolSymbol;
                    stageOrder.IDCOSymbol = secMasterObj.IDCOOptionSymbol;
                    stageOrder.SettlementCurrencyID = secMasterObj.CurrencyID;
                    stageOrder.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                    if (stageOrder.Level1ID == -1)
                    {
                        stageOrder.OriginalAllocationPreferenceID = AllocationSchemeID;
                        stageOrder.Level1ID = AllocationSchemeID;
                    }

                    switch (secMasterObj.AssetCategory)
                    {
                        case AssetCategory.EquityOption:
                        case AssetCategory.Option:
                        case AssetCategory.FutureOption:
                        case AssetCategory.Future:
                            if (stageOrder.OrderSideTagValue == "1")
                            {
                                stageOrder.OrderSideTagValue = "A";
                                stageOrder.OrderSide = "Buy to Open";
                            }
                            if (stageOrder.OrderSideTagValue == "2")
                            {
                                stageOrder.OrderSideTagValue = "D";
                                stageOrder.OrderSide = "Sell to Close";
                            }
                            if (stageOrder.OrderSideTagValue == "5")
                            {
                                stageOrder.OrderSideTagValue = "C";
                                stageOrder.OrderSide = "Sell to Open";
                            }
                            /// Buy to close remains same.
                            break;
                    }
                    Prana.AlgoStrategyControls.AlgoPropertiesHelper.SetAlgoParameters(stageOrder);
                    SetExpireTime(stageOrder);
                    try
                    {
                        if (!TradeManager.TradeManager.GetInstance().SendBlotterTrades(stageOrder, 0))
                        {
                            Logger.LoggerWrite("StageOrder not saved. Symbol :" + stageOrder.Symbol + " AvgPrice " + stageOrder.AvgPrice + " Quantity " + stageOrder.Quantity);
                            if (_isAutoImport)
                                stageOrder.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        if (rethrow)
                        {
                            throw;
                        }
                    }

                    if (stageOrder.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                    {
                        _validatedStagedOrderCollection.Add(stageOrder);
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
        /// Set Expire Time
        /// </summary>
        /// <param name="orderSingle"></param>
        private static void SetExpireTime(OrderSingle orderSingle)
        {
            try
            {
                if (!String.IsNullOrEmpty(orderSingle.TIF) && orderSingle.TIF.Equals(FIXConstants.TIF_GTD))
                {
                    DateTime dtValue;
                    if (DateTime.TryParse(orderSingle.ExpireTime, out dtValue))
                    {
                        DateTime Dt = dtValue.Date;
                        DateTime TimeStamp = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(orderSingle.AUECID);
                        orderSingle.ExpireTime = new DateTime(Dt.Year, Dt.Month, Dt.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                    }
                    else
                    {
                        orderSingle.ExpireTime = string.Empty;
                    }
                }
                else
                {
                    orderSingle.ExpireTime = string.Empty;
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
        private OrderSingle GetOrderFromDataRow(DataRow dTableRow)
        {
            OrderSingle orderRequest = new OrderSingle();
            try
            {
                orderRequest.Quantity = 0.0;
                orderRequest.MsgType = FIXConstants.MSGOrder;
                orderRequest.DiscretionInst = "0";
                orderRequest.DiscretionOffset = 0.0;
                orderRequest.AUECLocalDate = DateTime.Now;
                orderRequest.PNP = "0";
                orderRequest.PegDifference = 0.0;
                orderRequest.SwapParameters = null;
                orderRequest.AvgPrice = 0.0;
                orderRequest.Price = 0.0;
                orderRequest.StopPrice = 0.0;
                orderRequest.Text = String.Empty;
                orderRequest.HandlingInstruction = "3";
                orderRequest.OrderTypeTagValue = "1";
                orderRequest.OrderType = "MARKET";
                orderRequest.TIF = "0";
                orderRequest.Level1ID = int.MinValue;
                orderRequest.Level2ID = int.MinValue;
                if (CommonDataCache.CachedDataManager.GetInstance.GetUserTradingAccounts().Count > 0)
                    orderRequest.TradingAccountID = ((Prana.BusinessObjects.TradingAccount)CommonDataCache.CachedDataManager.GetInstance.GetUserTradingAccounts()[0]).TradingAccountID;
                orderRequest.ExecutionInstruction = "G";
                orderRequest.AUECID = 1;
                orderRequest.AssetID = 1;
                orderRequest.UnderlyingID = 1;
                orderRequest.CumQty = 0.0;
                orderRequest.CurrencyID = 1;
                orderRequest.ExchangeID = 1;
                orderRequest.CommissionRate = 0.0;
                orderRequest.CalcBasis = CalculationBasis.Auto;
                orderRequest.AlgoStrategyID = string.Empty;
                orderRequest.AlgoProperties = new OrderAlgoStartegyParameters();
                orderRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                orderRequest.CompanyUserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                if (dTableRow.Table.Columns.Contains("Symbol"))
                {
                    orderRequest.Symbol = Convert.ToString(dTableRow["Symbol"]);
                }
                if (dTableRow.Table.Columns.Contains("Quantity"))
                {
                    double tempQty = orderRequest.Quantity;
                    if (Double.TryParse(dTableRow["Quantity"].ToString(), out tempQty))
                        orderRequest.Quantity = tempQty;
                }
                if (dTableRow.Table.Columns.Contains("AvgPrice"))
                {
                    double tempAvPx = orderRequest.AvgPrice;
                    if (Double.TryParse(dTableRow["AvgPrice"].ToString(), out tempAvPx))
                        orderRequest.AvgPrice = tempAvPx;
                }

                if (dTableRow.Table.Columns.Contains("Price"))
                {
                    double tempDouble = orderRequest.Price;
                    if (Double.TryParse(dTableRow["Price"].ToString(), out tempDouble))
                        orderRequest.Price = tempDouble;
                }

                if (dTableRow.Table.Columns.Contains("StopPrice"))
                {
                    double tempDouble = orderRequest.StopPrice;
                    if (Double.TryParse(dTableRow["StopPrice"].ToString(), out tempDouble))
                        orderRequest.StopPrice = tempDouble;
                }

                if (dTableRow.Table.Columns.Contains("AuecLocalDate"))
                {
                    DateTime tempDate = orderRequest.AUECLocalDate;
                    if (DateTime.TryParse(dTableRow["AuecLocalDate"].ToString(), out tempDate))
                        orderRequest.AUECLocalDate = tempDate;
                }

                if (dTableRow.Table.Columns.Contains("Text"))
                {
                    orderRequest.Text = Convert.ToString(dTableRow["Text"]);
                }

                if (dTableRow.Table.Columns.Contains("AlgoStrategyID"))
                {
                    orderRequest.AlgoStrategyID = Convert.ToString(dTableRow["AlgoStrategyID"]);
                }

                if (dTableRow.Table.Columns.Contains("OrderSideTagValue"))
                {
                    orderRequest.OrderSideTagValue = Convert.ToString(dTableRow["OrderSideTagValue"]);
                }

                if (dTableRow.Table.Columns.Contains("Venue"))
                {
                    orderRequest.Venue = Convert.ToString(dTableRow["Venue"]);
                }

                if (dTableRow.Table.Columns.Contains("VenueID"))
                {
                    int tempInt = orderRequest.VenueID;
                    if (Int32.TryParse(dTableRow["VenueID"].ToString(), out tempInt))
                        orderRequest.VenueID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("CounterPartyName"))
                {
                    orderRequest.CounterPartyName = Convert.ToString(dTableRow["CounterPartyName"]);
                }

                if (dTableRow.Table.Columns.Contains("CounterPartyID"))
                {
                    int tempInt = orderRequest.CounterPartyID;
                    if (Int32.TryParse(dTableRow["CounterPartyID"].ToString(), out tempInt))
                        orderRequest.CounterPartyID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("HandlingInstruction"))
                {
                    orderRequest.HandlingInstruction = Convert.ToString(dTableRow["HandlingInstruction"]);
                }

                if (dTableRow.Table.Columns.Contains("OrderTypeTagValue"))
                {
                    orderRequest.OrderTypeTagValue = Convert.ToString(dTableRow["OrderTypeTagValue"]);
                }

                if (dTableRow.Table.Columns.Contains("TIF"))
                {
                    orderRequest.TIF = Convert.ToString(dTableRow["TIF"]);
                }

                if (dTableRow.Table.Columns.Contains("Level1ID"))
                {
                    int tempInt = orderRequest.Level1ID;
                    if (Int32.TryParse(dTableRow["Level1ID"].ToString(), out tempInt))
                        orderRequest.Level1ID = tempInt;
                }
                else if (dTableRow.Table.Columns.Contains("Account"))
                {
                    orderRequest.Account = dTableRow["Account"].ToString().Trim();
                    orderRequest.Level1ID = CachedDataManager.GetInstance.GetAccountID(orderRequest.Account.Trim());
                }

                if (dTableRow.Table.Columns.Contains("Level2ID"))
                {
                    int tempInt = orderRequest.Level2ID;
                    if (Int32.TryParse(dTableRow["Level2ID"].ToString(), out tempInt))
                        orderRequest.Level2ID = tempInt;
                }
                else if (dTableRow.Table.Columns.Contains("Strategy"))
                {
                    orderRequest.Strategy = dTableRow["Strategy"].ToString().Trim();
                    orderRequest.Level2ID = CachedDataManager.GetInstance.GetStrategyID(orderRequest.Strategy.Trim());
                }

                if (dTableRow.Table.Columns.Contains("TradingAccountID"))
                {
                    int tempInt = orderRequest.TradingAccountID;
                    if (Int32.TryParse(dTableRow["TradingAccountID"].ToString(), out tempInt))
                        orderRequest.TradingAccountID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("ExecutionInstruction"))
                {
                    orderRequest.ExecutionInstruction = Convert.ToString(dTableRow["ExecutionInstruction"]);
                }

                if (dTableRow.Table.Columns.Contains("AUECID"))
                {
                    int tempInt = orderRequest.AUECID;
                    if (Int32.TryParse(dTableRow["AUECID"].ToString(), out tempInt))
                        orderRequest.AUECID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("AssetID"))
                {
                    int tempInt = orderRequest.AssetID;
                    if (Int32.TryParse(dTableRow["AssetID"].ToString(), out tempInt))
                        orderRequest.AssetID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("UnderlyingID"))
                {
                    int tempInt = orderRequest.UnderlyingID;
                    if (Int32.TryParse(dTableRow["UnderlyingID"].ToString(), out tempInt))
                        orderRequest.UnderlyingID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("UserID"))
                {
                    int tempInt = orderRequest.CompanyUserID;
                    if (Int32.TryParse(dTableRow["UserID"].ToString(), out tempInt))
                        orderRequest.CompanyUserID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("CumQty"))
                {
                    double tempDouble = orderRequest.CumQty;
                    if (Double.TryParse(dTableRow["CumQty"].ToString(), out tempDouble))
                        orderRequest.CumQty = tempDouble;
                }

                if (dTableRow.Table.Columns.Contains("CurrencyID"))
                {
                    int tempInt = orderRequest.CurrencyID;
                    if (Int32.TryParse(dTableRow["CurrencyID"].ToString(), out tempInt))
                        orderRequest.CurrencyID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("ExchangeID"))
                {
                    int tempInt = orderRequest.ExchangeID;
                    if (Int32.TryParse(dTableRow["ExchangeID"].ToString(), out tempInt))
                        orderRequest.ExchangeID = tempInt;
                }

                if (dTableRow.Table.Columns.Contains("CommissionRate"))
                {
                    double tempDouble = orderRequest.CommissionRate;
                    if (Double.TryParse(dTableRow["CommissionRate"].ToString(), out tempDouble))
                        orderRequest.CommissionRate = tempDouble;
                }

                if (dTableRow.Table.Columns.Contains("CalcBasis"))
                {
                    orderRequest.CalcBasis = (CalculationBasis)Convert.ToInt32(dTableRow["CalcBasis"]);
                }

                if (dTableRow.Table.Columns.Contains("FXRate"))
                {
                    double tempDouble = orderRequest.FXRate;
                    if (Double.TryParse(dTableRow["FXRate"].ToString(), out tempDouble))
                        orderRequest.FXRate = tempDouble;
                }

                if (dTableRow.Table.Columns.Contains("FXConversionMethodOperator"))
                {
                    orderRequest.FXConversionMethodOperator = dTableRow["FXConversionMethodOperator"].ToString();
                }
                if (dTableRow.Table.Columns.Contains("ExpireTime"))
                {
                    orderRequest.ExpireTime = dTableRow["ExpireTime"].ToString();
                }
                orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                orderRequest.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                orderRequest.ImportFileID = _importedFileId;
                orderRequest.ImportFileName = _importedFileName;
                // TODO :  Needs to be handled on OrderSingle class
                // orderRequest.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                return orderRequest;
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
            return orderRequest;
        }

        public void GetSMDataForStagedOrder(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {
                if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                {
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            if (secMasterObj.AUECID > 0)
                            {
                                _countValidatedSymbols++;
                            }
                            else
                            {
                                _countNonValidatedSymbols++;
                            }
                            _secMasterResponseCollection.Add(secMasterObj);
                            //UpdateCollection(secMasterObj, _runUpload.Key);
                        }
                    }
                    //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
                    //This may be possible that we get response for all the symbols for but some data is not validated
                    //e.g. Side not picking in file, trade will be invalidated
                    if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
                    {
                        //In this method if all the trades are validated then data will be saved to db otherwise import will be cancelled
                        _timerSecurityValidation.Stop();
                        TimerSecurityValidation_Tick(null, null);
                    }
                    //Start timer and wait for response from api or db
                    else
                    {
                        ResetTimer();
                    }
                }
                else
                {
                    if (_currentResult != null && _isAutoImport)
                    {
                        _currentResult.Error = new Exception("No data");      
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
        #endregion



        /// <summary>
        /// If all the trades are validated then data will be imported in system
        /// Partial validated trades will not be saved in system
        /// Statistics xml will be written for validated and non validated trades
        /// Statistics xml will be written for validated symbols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerSecurityValidation_Tick(object sender, EventArgs e)
        {
            try
            {
                _timerSecurityValidation.Stop();
                lock (_timerLock)
                {
                    DataTable schemeTable = _collection[_runUpload.Key].GetScheme();
                    if (schemeTable != null)
                    {
                        DataSet schemeSet = new DataSet();
                        schemeSet.Tables.Add(schemeTable);
                        schemeSet.DataSetName = "DocumentElement";

                        ArrangeDataSet(schemeSet);

                        GenerateAllocationScheme(schemeSet, _runUpload.FileName, _secMasterResponseCollection);
                    }

                    OnSecurityValidationCompleted();
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
            finally
            {
                _timerSecurityValidation.Stop();
            }
        }

        private void SendValidatedTradesToBlotter()
        {
            if (_secMasterResponseCollection != null && _secMasterResponseCollection.Count > 0)
            {
                foreach (SecMasterBaseObj secMasterObj in _secMasterResponseCollection)
                {
                    UpdateCollection(secMasterObj, _runUpload.Key);
                }
            }
        }
        private void OnSecurityValidationCompleted()
        {

            try
            {
                SendValidatedTradesToBlotter();
                if (_stagedOrderCollection != null && _currentResult != null)
                {
                    DataTable dtStagedOrderCollection = GeneralUtilities.GetDataTableFromList(_stagedOrderCollection);
                    ValidatePriceTolerances(dtStagedOrderCollection);

                    #region write xml for non validated trades
                    int countNonValidatedStagedOrder = _stagedOrderCollection.Count(stagedOrder => stagedOrder.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedStagedOrder", countNonValidatedStagedOrder, null);

                    #endregion

                    #region write xml for validated trades
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedStagedOrder", _validatedStagedOrderCollection.Count, null);
                    #endregion

                    #region write xml for validated symbols

                    //DataTable dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(_secMasterResponseCollection);
                    DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                    // ConvertSecMasterBaseObjToUIObj(dtValidatedSymbols); 
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", _countSymbols, null);
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", _countNonValidatedSymbols, null);
                    if (_countSymbols == _countValidatedSymbols)
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", ImportStatus.Success, null);
                    }
                    else if (_countValidatedSymbols == 0)
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", ImportStatus.Failure, null);
                    }
                    else
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", ImportStatus.PartialSuccess, null);
                    }
                    if (dtValidatedSymbols != null)
                    {
                        // Todo : Needs to add symbology in orderSingle object
                        //AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols);
                    }
                    #endregion

                    #region symbol validation and total order added to task statistics

                    if (dtStagedOrderCollection != null && dtStagedOrderCollection.Rows.Count > 0)
                    {
                        //data to be saved in file after trades is imported.
                        //this will be true only when importing.
                        //for re-run upload & re-reun Symbol validation it will be false.
                        //if (!_isSaveDataInApplication)
                        //{
                        UpdateImportDataXML(dtStagedOrderCollection);
                        //}
                    }
                    if (dtValidatedSymbols != null)
                    {
                        //ValidateSymbols(dtStagedOrderCollection, ref dtValidatedSymbols);
                        dtValidatedSymbols.TableName = "ValidatedSymbols";
                        using (XmlTextWriter xmlWriter = new XmlTextWriter(Application.StartupPath + _validatedSymbolsXmlFilePath, Encoding.UTF8))
                        {
                            dtValidatedSymbols.WriteXml(xmlWriter);
                        }
                        // dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                    }
                    #endregion


                    #region add dashboard statistics data

                    int accountCounts = _stagedOrderCollection.Select(stagedOrder => stagedOrder.Level1ID).Distinct().Count();
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCounts, null);

                    int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                    #endregion
                    //Validated PositionMaster objects will be added to _validatedPositionMasterCollections
                    //Save data in application where _isSaveDataInApplication is true
                    if (_isSaveDataInApplication)
                    {
                        if (_validatedStagedOrderCollection.Count == 0)
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                            if (TaskSpecificDataPointsPreUpdate != null)
                            {
                                TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                TaskSpecificDataPointsPreUpdate = null;
                            }
                            UnwireEvents();
                        }
                        else
                        {
                            //_bgwImportData.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        if (TaskSpecificDataPointsPreUpdate != null)
                        {
                            TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            TaskSpecificDataPointsPreUpdate = null;
                        }
                        UnwireEvents();
                    }
                }

                //As per current implementation sending reconcillation email only for the Auto-import
                if (_currentResult != null && _isAutoImport)
                {
                    double quantitySumFromValidatedOrders = 0.0;
                    foreach (OrderSingle order in ValidatedStagedOrderCollection)
                    {
                        quantitySumFromValidatedOrders += order.Quantity;
                    }
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("QuantitySumFromValidatedOrders", quantitySumFromValidatedOrders, null);
                    if (ImportCompleted != null)
                        ImportCompleted(this, new EventArgs<TaskResult>(_currentResult));

                }


                List<string> allsymbols = _stagedOrderCollection.Where(x => x.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.NotExists.ToString()) || x.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.None.ToString())).Select(x => x.Symbol).ToList();
                if (allsymbols.Count > 0)
                {
                    foreach (var item in _validatedStagedOrderCollection.Select(x => x.Symbol).ToList())
                    {
                        if (allsymbols.Contains(item))
                        {
                            allsymbols.Remove(item);
                        }
                    }
                    Logger.LoggerWrite("Security is either nonexistent or invalid : " + string.Join(",", allsymbols), "AutoImport_Logging", 1, 1, System.Diagnostics.TraceEventType.Information, "log-entry", null);
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


        public void ValidatePriceTolerances(DataTable dtPositionMasterCollection)
        {
            try
            {

                Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>();
                List<string> lstColumnsToKey = new List<string>();
                #region price tolerance check
                if (_runUpload.IsPriceToleranceChecked)
                {
                    //select distict account and symbol
                    var Test = _stagedOrderCollection
                            .Select(stagedOrder => new { stagedOrder.Level1ID, stagedOrder.Symbol })
                            .Distinct()
                            .ToList();
                    Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollection = new Dictionary<int, Dictionary<string, double>>();

                    foreach (var item in Test)
                    {
                        Dictionary<string, double> dictSymbolCollection = new Dictionary<string, double>();
                        if (!dictAccountSymbolCollection.ContainsKey(item.Level1ID))
                        {
                            dictSymbolCollection.Add(item.Symbol, int.MinValue);
                            dictAccountSymbolCollection.Add(item.Level1ID, dictSymbolCollection);
                        }
                        else
                        {
                            if (!dictAccountSymbolCollection[item.Level1ID].ContainsKey(item.Symbol))
                            {
                                dictAccountSymbolCollection[item.Level1ID].Add(item.Symbol, int.MinValue);
                            }
                        }
                    }
                    //fill last business day mark prices from mark price cache
                    ServiceManager.Instance.PricingServices.InnerChannel.GetMarkPriceForAccountSymbolCollection(ref dictAccountSymbolCollection);

                    DataTable dtLastBusinessDayMarkPrices = new DataTable();
                    dtLastBusinessDayMarkPrices.Columns.Add("AccountID", typeof(int));
                    dtLastBusinessDayMarkPrices.Columns.Add("Symbol", typeof(string));
                    dtLastBusinessDayMarkPrices.Columns.Add("MarkPrice", typeof(double));
                    foreach (KeyValuePair<int, Dictionary<string, double>> accountSymbolItem in dictAccountSymbolCollection)
                    {
                        foreach (KeyValuePair<string, double> symbolItem in accountSymbolItem.Value)
                        {
                            if (symbolItem.Value > 0)
                                dtLastBusinessDayMarkPrices.Rows.Add(accountSymbolItem.Key, symbolItem.Key, symbolItem.Value);
                        }
                    }
                    if (dtLastBusinessDayMarkPrices.Rows.Count > 0)
                    {
                        lstColumnsToKey.Clear();

                        //TODO: 
                        //lstColumnsToKey.Add("AccountID");
                        lstColumnsToKey.Add("Symbol");

                        dictColumnsToReconcile.Clear();

                        dictColumnsToReconcile.Add("Symbol", "Symbol");
                        dictColumnsToReconcile.Add("CostBasis", "MarkPrice");

                        DATAReconciler.RecocileData(dtPositionMasterCollection, dtLastBusinessDayMarkPrices, lstColumnsToKey, dictColumnsToReconcile, _runUpload.PriceTolerance, ComparisionType.Numeric);
                        //DataSet ds = new DataSet();
                        //ds.Tables.Add(dtPositionMasterCollection);
                        //SetCollections(ds);
                    }
                }
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


        /// <summary>
        /// Resets the timer
        /// </summary>
        private void ResetTimer()
        {
            try
            {
                lock (_timerLock)
                {
                    _timerSecurityValidation.Stop();
                    _timerSecurityValidation.Start();
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
        /// To update Import data xml file
        /// </summary>
        /// <param name="dtStagedOrderCollection"></param>
        private void UpdateImportDataXML(DataTable dtStagedOrderCollection)
        {
            try
            {
                dtStagedOrderCollection.TableName = "ImportData";
                if (dtStagedOrderCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtStagedOrderCollection.Columns["RowIndex"];
                    dtStagedOrderCollection.PrimaryKey = columns;
                }
                // Purpose : Removing AlgoProperties column from ImportData xml
                // AlgoProperties contains IDictionary which can not be serialized during xml write
                if (dtStagedOrderCollection.Columns.Contains("AlgoProperties"))
                {
                    dtStagedOrderCollection.Columns.Remove("AlgoProperties");
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (dtStagedOrderCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtStagedOrderCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtStagedOrderCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtStagedOrderCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                if (_currentResult != null)
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _validatedStagedOrderCollection.Count, _totalImportDataXmlFilePath);
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

        //TODO : Needs to be removed

        /// <summary>
        /// Function to set if reimport and filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isReimporting"></param>
        //private void SetReImportVariables(string filePath, bool isReimporting)
        //{
        //    _isReimporting = isReimporting;
        //    _filePath = filePath;
        //}

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timerSecurityValidation != null)
                {
                    _timerSecurityValidation.Dispose();
                }
                if (_runUpload != null)
                {
                    _runUpload.Dispose();
                }
            }
        }

        #endregion
    }
}