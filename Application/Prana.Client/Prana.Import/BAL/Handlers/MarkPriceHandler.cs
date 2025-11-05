using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Prana.Import
{
    sealed public class MarkPriceHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        public MarkPriceHandler() { }

        #region local variables

        const string DATEFORMAT = "MM/dd/yyyy";
        // list collection of import mark price values
        List<MarkPriceImport> _markPriceValueCollection = new List<MarkPriceImport>();
        //List<Tuple<MarkPriceImport, int, string>> _markPriceCollectionTuple = new List<Tuple<MarkPriceImport, int, string>>();

        // List of validated mark price values to be saved in DB
        List<MarkPriceImport> _validatedMarkPriceValueCollection = new List<MarkPriceImport>();

        Dictionary<int, Dictionary<string, List<MarkPriceImport>>> _markPriceSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<MarkPriceImport>>>();

        //Dictionary<string, XmlNode> _SMMappingCOLList = new Dictionary<string, XmlNode>();

        Dictionary<string, MarkPriceImport> _dictRequestedSymbol = new Dictionary<string, MarkPriceImport>();

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

        int _rowsUpdated = 0;
        //int _rowIndex = int.MinValue;
        RunUpload _runUpload = null;
        private System.Timers.Timer _timerSecurityValidation;
        //private System.Timers.Timer _timerRefresh = new System.Timers.Timer(10 * 60 * 1000);
        TaskResult _currentResult = new TaskResult();
        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;

        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;
        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        private static object _timerLock = new Object();

        BackgroundWorker _bgwImportData = new BackgroundWorker();

        /// <summary>
        /// added by: Bharat raturi, 22 may 2014
        /// purpose: flag variable to indicate whether the data is to be saved in application
        /// </summary>
        bool _isSaveDataInApplication = false;
        private int _importedTradesCount = 0;
        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;
        #endregion

        #region IImportHandler Members

        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _isSaveDataInApplication = isSaveDataInApplication;
                _runUpload = runUpload;
                _currentResult = taskResult as TaskResult;
                //As discussed with sandeep ji, it must be checked for Netposition/ Transactions and markprice import
                SecurityMasterManager.Instance.GenerateSMMapping(ds);

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);

                // Price Tolerance check moved to ValidateSymbols method

                SetCollection(ds, runUpload);
                GetSMDataForMarkPriceImport();

                //SaveMarkPrices();
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
        /// Update validation status for the symbols which have passed price validation
        /// </summary>
        /// <returns></returns>
        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            DataTable dtOutputTable = null;
            try
            {
                if (_runUpload != null && _runUpload.IsPriceToleranceChecked)
                {
                    //TODO: Need to use new markrice SP having account+symbol mark price
                    //DataTable dt1 = MarkDataManager.FetchMarkPricesAccountWiseForLastBusinessDay();
                    //Dictionary<string, string> dictColumnsToKey = new Dictionary<string, string>();

                    //dictColumnsToKey.Add("AccountName", "AccountName");
                    //dictColumnsToKey.Add("Symbol", "Symbol");


                    //List<string> lstColumnsToReconcile = _runUpload.PriceToleranceColumns.Split(',').ToList();

                    //dtOutputTable = DATAReconciler.RecocileData(dt1, ds.Tables[0], lstColumnsToReconcile, dictColumnsToKey, _runUpload.PriceTolerance, ComparisionType.Numeric);
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
            return dtOutputTable;
        }

        readonly object _object = new object();
        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            try
            {
                //Symbols which get response either from db or from api or from cache will be validated symbols
                //so we are increasing here counter for validated symbols and adding response to list
                //This list will be used to write the xml of validtaed and Non validated symbols
                if (secMasterObj.AUECID > 0)
                {
                    _countValidatedSymbols++;
                }
                else
                {
                    _countNonValidatedSymbols++;
                }
                lock (_object)
                {
                    _secMasterResponseCollection.Add(secMasterObj);
                }
                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                if (_markPriceSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<MarkPriceImport>> dictSymbols = _markPriceSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<MarkPriceImport> listMarkPrice = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (MarkPriceImport markPriceImport in listMarkPrice)
                        {
                            validateAllSymbols(markPriceImport, secMasterObj);

                            markPriceImport.Symbol = pranaSymbol;
                            markPriceImport.CUSIP = cuspiSymbol;
                            markPriceImport.ISIN = isinSymbol;
                            markPriceImport.SEDOL = sedolSymbol;
                            markPriceImport.Bloomberg = bloombergSymbol;
                            markPriceImport.RIC = reutersSymbol;
                            markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                            markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                            markPriceImport.OpraOptionSymbol = opraOptionSymbol;
                            markPriceImport.AUECID = secMasterObj.AUECID;
                            markPriceImport.IsSecApproved = secMasterObj.IsSecApproved;

                            UpdateMarkPriceObj(markPriceImport, secMasterObj);

                            if (markPriceImport.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()) && !string.IsNullOrWhiteSpace(markPriceImport.Symbol))
                            {
                                lock (_object)
                                {
                                    _validatedMarkPriceValueCollection.Add(markPriceImport);
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

        public string GetXSDName()
        {
            return "ImportMarkPrice.xsd";
        }

        #endregion

        #region Events

        /// <summary>
        /// added by: Bharat Raturi, 22 may 2014
        /// attach the event for response from sec master
        /// </summary>
        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(SecurityMaster_SecMstrDataResponse);
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                //_bgwImportData.DoWork += new DoWorkEventHandler(bgwImportData_DoWork);
                //_bgwImportData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
                //_bgwImportData.WorkerSupportsCancellation = true;
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
        /// UnWires Event
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                _timerSecurityValidation.Elapsed -= new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                //_bgwImportData.DoWork -= new DoWorkEventHandler(bgwImportData_DoWork);
                //_bgwImportData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(SecurityMaster_SecMstrDataResponse);
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
        /// Update mark price collection asynchronously for each secmaster response
        /// After validating all the symbols we will do import process.
        /// For a single invalidated markprice import will be cancelled
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void SecurityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                UpdateCollection(e.Value, string.Empty);

                //If we get response for all the requested symbols then call the TimerRefresh_Tick to save the changes
                //This may be possible that we get response for all the symbols for but some data is not validated
                //e.g. date or account missing in file, data will be invalidated
                if (_countSymbols == _countValidatedSymbols)
                {
                    _timerSecurityValidation.Stop();
                    TimerSecurityValidation_Tick(this, null);
                }
                else
                {
                    ResetTimer();
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

        //private static object _locker = new Object();

        //void bgwImportData_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        lock (_locker)
        //        {
        //            SaveMarkPrices();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void bgwImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        //if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
        //        //{              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
        //        //    MessageBox.Show("Operation has been cancelled!", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //}
        //        //else
        //        //{
        //        //    MessageBox.Show("Mark prices saved successfully", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        //}

        //        bool isPartialSuccess = false;
        //        int MarkPriceCount = int.MinValue;

        //        if (_markPriceValueCollection != null)
        //        {
        //            MarkPriceCount = _markPriceValueCollection.Count;
        //}
        //        if (e.Error != null)
        //        {
        //            MessageBox.Show(e.Error.Message);
        //        }
        //else
        //{
        //            if (e.Cancelled)
        //            {
        //                _markPriceValueCollection = null;
        //}
        //            else
        //            {
        //                String message = e.Result.ToString() as string;
        //                //data to be saved in file after trades is imported.
        //                //this will be true only when importing.
        //                //for re-run upload & re-run Symbol validation it will be false.
        //                if (_isSaveDataInApplication)
        //                {

        //                    if (_markPriceValueCollection != null)
        //                    {
        //                        //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";
        //                        DataTable dtMarkPriceCollection = GeneralUtilities.GetDataTableFromList(_markPriceValueCollection);
        //                        dtMarkPriceCollection.TableName = "ImportData";
        //                        if (dtMarkPriceCollection.Columns.Contains("RowIndex"))
        //                        {
        //                            DataColumn[] columns = new DataColumn[1];
        //                            //create primary key fr datatable
        //                            columns[0] = dtMarkPriceCollection.Columns["RowIndex"];
        //                            dtMarkPriceCollection.PrimaryKey = columns;
        //                        }
        //                        //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
        //                        if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
        //                        {
        //                            DataSet ds = new DataSet();
        //                            ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
        //                            if (ds != null && ds.Tables.Count > 0)
        //                            {
        //                                // TODO : For now there were conflict error while merging the table with the column so it is removed.
        //                                if (dtMarkPriceCollection.Columns.Contains("BrokenRulesCollection"))
        //                                {
        //                                    dtMarkPriceCollection.Columns.Remove("BrokenRulesCollection");
        //                                }
        //                                dtMarkPriceCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
        //                            }
        //                        }
        //                        int importedTradesCount = dtMarkPriceCollection.Select("ImportStatus ='Imported'").Length;
        //                        if (importedTradesCount != dtMarkPriceCollection.Rows.Count)
        //                        {
        //                            isPartialSuccess = true;
        //                        }
        //                        dtMarkPriceCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
        //                        if (_currentResult != null)
        //                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _markPriceValueCollection.Count, _totalImportDataXmlFilePath);
        //                    }

        //                }

        //                if (!String.IsNullOrEmpty(message) && (message != Constants.ImportCompletionStatus.Success.ToString()) && !_recordsProcessed.Equals(_markPriceValueCollection.Count))
        //                {
        //                    StringBuilder boxMessage = new StringBuilder();
        //                    boxMessage.AppendLine("Some mark price could not be imported. Kindly reimport from Reimport tab!");
        //                    MessageBox.Show(boxMessage.ToString(), "Information");
        //                }
        //                else
        //                {
        //                    if (_isReimporting && File.Exists(_filePath))
        //                    {
        //                        File.Delete(_filePath);
        //                        if (ReImportCompleted != null)
        //                        {
        //                            ReImportCompleted(this, null);
        //                        }
        //                    }

        //                    _recordsProcessed = int.MinValue;
        //                    _markPriceValueCollection = null;
        //                }
        //            }
        //        }
        //        string resultofValidation = e.Result.ToString();
        //        if (_currentResult != null)
        //        {
        //            if (MarkPriceCount == _validatedMarkPriceValueCollection.Count && !isPartialSuccess && resultofValidation == ImportStatus.Success.ToString())
        //            {
        //                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
        //            }
        //            else if ((MarkPriceCount != _validatedMarkPriceValueCollection.Count || isPartialSuccess) && resultofValidation == ImportStatus.Success.ToString())
        //            {
        //                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.PartialSuccess, null);
        //            }
        //            else
        //            {
        //                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
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
        //    finally
        //    {
        //        UnwireEvents();
        //        if (TaskSpecificDataPointsPreUpdate != null && _currentResult != null)
        //        {
        //            //Return TaskResult which was recieved from ImportManager as event argument
        //            TaskSpecificDataPointsPreUpdate(this, _currentResult);
        //            TaskSpecificDataPointsPreUpdate = null;
        //        }
        //    }
        //}
        #endregion

        private void SaveMarkPrices()
        {
            try
            {
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                bool isValidToTrade = validateTradeForAccountNAVLock();

                if (isValidToTrade)
                {
                    // insert cash values into the DB
                    if (_validatedMarkPriceValueCollection.Count > 0)
                    {
                        // total number of records inserted
                        //totalRecordsCount = _markPriceValueCollection.Count;

                        DataTable dtMarkPrices = CreateDataTableForMarkPriceImport();
                        DataTable dtMarkPriceImportData = GeneralUtilities.GetDataTableFromList(_validatedMarkPriceValueCollection);

                        DataTable dtMarkPriceTableFromCollection = GeneralUtilities.CreateTableFromCollection<MarkPriceImport>(dtMarkPrices, _validatedMarkPriceValueCollection);

                        try
                        {
                            if ((dtMarkPriceTableFromCollection != null) && (dtMarkPriceTableFromCollection.Rows.Count > 0))
                            {
                                //modified by omshiv, sending IsAutoapprove mark prices or not
                                _rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveMarkPrices(dtMarkPriceTableFromCollection, false);
                            }
                            //rowsUpdated = RunUploadManager.SaveRunUploadFileDataForMarkPrice(dtMarkPricesNew,_markPriceValueCollection);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                            //All the trades that are not imported in application due to error, change their status   
                            foreach (DataRow row in dtMarkPriceImportData.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.ImportError.ToString();
                            }
                            if (_currentResult != null)
                                _currentResult.Error = new Exception(ex.Message);
                        }

                        if (_rowsUpdated > 0)
                        {
                            //All the trades that are imported in application change their status   
                            foreach (DataRow row in dtMarkPriceImportData.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.Imported.ToString();
                            }
                            resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                            UpdateImportDataXML(dtMarkPriceImportData);
                        }

                        int importedTradesCount = dtMarkPriceImportData.Select("ImportStatus ='Imported'").Length;
                        if (importedTradesCount != dtMarkPriceImportData.Rows.Count)
                        {
                            isPartialSuccess = true;
                        }

                        if (_currentResult != null)
                        {
                            if (_markPriceValueCollection.Count == _validatedMarkPriceValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                            }
                            else if ((_markPriceValueCollection.Count != _validatedMarkPriceValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Partial Success", null);
                            }
                            else
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Failure", null);
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
            finally
            {
                if (TaskSpecificDataPointsPreUpdate != null)
                {
                    //Return TaskResult which was recieved from ImportManager as event argument
                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                    TaskSpecificDataPointsPreUpdate = null;
                }

            }
        }

        /// <summary>
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1507
        /// </summary>
        /// <returns></returns>
        private bool validateTradeForAccountNAVLock()
        {
            bool isProcessToSave = true;
            try
            {
                #region NAV lock validation
                //get IsNAVLockingEnabled or not from cache
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (MarkPriceImport markPriceTuple in _markPriceValueCollection)
                    {
                        //if account selected then only check NAV locked or not for selected account
                        if (!string.IsNullOrEmpty(markPriceTuple.AccountName))
                        {
                            DateTime tradeDate = DateTime.MinValue;
                            if (DateTime.TryParse(markPriceTuple.Date, out tradeDate))
                            {
                                bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(markPriceTuple.AccountID, tradeDate);
                                if (!isTradeAllowed)
                                {
                                    MessageBox.Show("NAV is locked for selected account. You can not allow to trade on this trade date.", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    isProcessToSave = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Account name is not available!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            isProcessToSave = false;
                            break;
                        }
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

            return isProcessToSave;
        }

        private void GetSMDataForMarkPriceImport()
        {
            try
            {
                if (_markPriceSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<MarkPriceImport>>> kvp in _markPriceSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<MarkPriceImport>> symbolDict = _markPriceSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<MarkPriceImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    //We are increasing total symbols counter as each request is for a separate symbol
                                    //This counter will be used in dashboard xml
                                    _countSymbols++;
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //  secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                        //Update collection of mark price import for the symbol response which is fetched from cache
                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                UpdateCollection(secMasterObj, string.Empty);
                            }
                        }

                        //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
                        //This may be possible that we get response for all the symbols for but some data is not validated
                        //e.g. date or account missing in file, data will be non validated
                        if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
                        {
                            //In this method if all the trades are validated then data will be saved to db otherwise import will be cancelled
                            _timerSecurityValidation.Stop();
                            TimerSecurityValidation_Tick(null, null);
                        }
                        else
                        {
                            _timerSecurityValidation.Start();
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

        private DataTable CreateDataTableForMarkPriceImport()
        {
            DataTable dtMarkPricesNew = new DataTable();
            try
            {
                dtMarkPricesNew.TableName = "MarkPriceImport";

                DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
                DataColumn colDate = new DataColumn("Date", typeof(string));
                DataColumn colMarkPrice = new DataColumn("MarkPrice", typeof(double));
                DataColumn colForwardPoints = new DataColumn("ForwardPoints", typeof(double));
                DataColumn colMarkPriceImportType = new DataColumn("MarkPriceImportType", typeof(string));
                DataColumn colAccountID = new DataColumn("FundID", typeof(int));
                DataColumn colSourceID = new DataColumn("Source", typeof(int));

                dtMarkPricesNew.Columns.Add(colSymbol);
                dtMarkPricesNew.Columns.Add(colDate);
                dtMarkPricesNew.Columns.Add(colMarkPrice);
                dtMarkPricesNew.Columns.Add(colForwardPoints);
                dtMarkPricesNew.Columns.Add(colMarkPriceImportType);
                dtMarkPricesNew.Columns.Add(colAccountID);
                dtMarkPricesNew.Columns.Add(colSourceID);
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
            return dtMarkPricesNew;
        }

        private void UpdateMarkPriceObj(MarkPriceImport markPriceImport, SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (markPriceImport.IsForexRequired.Trim().ToUpper().Equals("TRUE"))
                {
                    if (!string.IsNullOrEmpty(markPriceImport.Date))
                    {
                        //CHMW-3132	Account wise fx rate handling for expiration settlement                    
                        int accountBaseCurrencyID;
                        if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(markPriceImport.AccountID))
                        {
                            accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[markPriceImport.AccountID];
                        }
                        else
                        {
                            accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                        }
                        if (accountBaseCurrencyID != secMasterObj.CurrencyID)
                        {
                            int companyID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                            ForexConverter.GetInstance(companyID, Convert.ToDateTime(markPriceImport.Date)).GetForexData(Convert.ToDateTime(markPriceImport.Date));
                            ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(companyID).GetConversionRateFromCurrenciesForGivenDate(accountBaseCurrencyID, secMasterObj.CurrencyID, Convert.ToDateTime(markPriceImport.Date), markPriceImport.AccountID);
                            if (conversionRate != null)
                            {
                                if (conversionRate.ConversionMethod == Operator.D)
                                {
                                    if (conversionRate.RateValue != 0)
                                        markPriceImport.MarkPrice = markPriceImport.MarkPrice / conversionRate.RateValue;
                                }
                                else
                                {
                                    markPriceImport.MarkPrice = markPriceImport.MarkPrice * conversionRate.RateValue;
                                }
                            }
                        }
                    }
                }

                if (Regex.IsMatch(markPriceImport.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                {
                    if (markPriceImport.ValidationError.Length > "Account Lock Required".Length)
                    {
                        List<string> valerror = markPriceImport.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                        valerror.Remove("Account Lock Required");
                        markPriceImport.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                    }
                    else
                    {
                        markPriceImport.ValidationError = string.Empty;
                        markPriceImport.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
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

        private void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _markPriceSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                _markPriceValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _validatedMarkPriceValueCollection = new List<MarkPriceImport>();
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(MarkPriceImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    MarkPriceImport markPriceValue = new MarkPriceImport();
                    markPriceValue.Symbol = string.Empty;
                    markPriceValue.MarkPrice = 0;
                    markPriceValue.ForwardPoints = 0;
                    markPriceValue.Date = string.Empty;
                    markPriceValue.AUECID = 0;
                    markPriceValue.RowIndex = irow;
                    markPriceValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();
                    markPriceValue.Source = (int)Enum.Parse(typeof(PricingSource), PricingSource.Gateway.ToString());

                    ImportHelper.SetProperty(typeToLoad, ds, markPriceValue, irow);

                    // Purpose : Handle user selected date

                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        markPriceValue.Date = dtn.ToString(DATEFORMAT);
                    }

                    else if (!markPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(markPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(markPriceValue.Date));
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(markPriceValue.Date);
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    string uniqueID = string.Empty;

                    #region fill data through cache

                    // Purpose : To handle user selected account
                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        markPriceValue.AccountID = runUpload.SelectedAccount;
                        markPriceValue.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!string.IsNullOrWhiteSpace(markPriceValue.AccountName))
                    {
                        markPriceValue.AccountID = CachedDataManager.GetInstance.GetAccountID(markPriceValue.AccountName);
                    }
                    #endregion

                    #region Creating Symbologywise dictionary

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(markPriceValue.Symbology))
                    {
                        SetSymbology(markPriceValue);
                    }

                    switch (markPriceValue.Symbology.Trim().ToUpper())
                    {
                        case "SYMBOL":
                            uniqueID = markPriceValue.Date + markPriceValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + markPriceValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + markPriceValue.Symbol), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[0];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Symbol))
                                {
                                    List<MarkPriceImport> markPriceSymbolWiseList = markPriceSameSymbologyDict[markPriceValue.Symbol];
                                    markPriceSymbolWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.Symbol] = markPriceSymbolWiseList;
                                    _markPriceSymbologyWiseDict[0] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[0].Add(markPriceValue.Symbol, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbolDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameSymbolDict.Add(markPriceValue.Symbol, markPricelist);
                                _markPriceSymbologyWiseDict.Add(0, markPriceSameSymbolDict);
                            }
                            break;
                        case "RIC":
                            uniqueID = markPriceValue.Date + markPriceValue.RIC.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + markPriceValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + markPriceValue.RIC), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[1];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.RIC))
                                {
                                    List<MarkPriceImport> markPriceRICWiseList = markPriceSameSymbologyDict[markPriceValue.RIC];
                                    markPriceRICWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.RIC] = markPriceRICWiseList;
                                    _markPriceSymbologyWiseDict[1] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                                    markPirceList.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[1].Add(markPriceValue.RIC, markPirceList);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                                markPirceList.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameRICDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameRICDict.Add(markPriceValue.RIC, markPirceList);
                                _markPriceSymbologyWiseDict.Add(1, markPriceSameRICDict);
                            }
                            break;
                        case "ISIN":
                            uniqueID = markPriceValue.Date + markPriceValue.ISIN.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + markPriceValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + markPriceValue.ISIN), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[2];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.ISIN))
                                {
                                    List<MarkPriceImport> markPriceISINWiseList = markPriceSameSymbologyDict[markPriceValue.ISIN];
                                    markPriceISINWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.ISIN] = markPriceISINWiseList;
                                    _markPriceSymbologyWiseDict[2] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[2].Add(markPriceValue.ISIN, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameISINDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameISINDict.Add(markPriceValue.ISIN, markPricelist);
                                _markPriceSymbologyWiseDict.Add(2, markPriceSameISINDict);
                            }
                            break;
                        case "SEDOL":
                            uniqueID = markPriceValue.Date + markPriceValue.SEDOL.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + markPriceValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + markPriceValue.SEDOL), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[3];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.SEDOL))
                                {
                                    List<MarkPriceImport> markPriceSEDOLWiseList = markPriceSameSymbologyDict[markPriceValue.SEDOL];
                                    markPriceSEDOLWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.SEDOL] = markPriceSEDOLWiseList;
                                    _markPriceSymbologyWiseDict[3] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[3].Add(markPriceValue.SEDOL, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSEDOLDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSEDOLDict.Add(markPriceValue.SEDOL, markPricelist);
                                _markPriceSymbologyWiseDict.Add(3, markPriceSEDOLDict);
                            }
                            break;
                        case "CUSIP":
                            uniqueID = markPriceValue.Date + markPriceValue.CUSIP.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + markPriceValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + markPriceValue.CUSIP), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[4];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.CUSIP))
                                {
                                    List<MarkPriceImport> markPriceCUSIPWiseList = markPriceSameSymbologyDict[markPriceValue.CUSIP];
                                    markPriceCUSIPWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.CUSIP] = markPriceCUSIPWiseList;
                                    _markPriceSymbologyWiseDict[4] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[4].Add(markPriceValue.CUSIP, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameCUSIPDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameCUSIPDict.Add(markPriceValue.CUSIP, markPricelist);
                                _markPriceSymbologyWiseDict.Add(4, markPriceSameCUSIPDict);
                            }
                            break;
                        case "BLOOMBERG":
                            uniqueID = markPriceValue.Date + markPriceValue.Bloomberg.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + markPriceValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + markPriceValue.Bloomberg), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[5];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Bloomberg))
                                {
                                    List<MarkPriceImport> markPriceBloombergWiseList = markPriceSameSymbologyDict[markPriceValue.Bloomberg];
                                    markPriceBloombergWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.Bloomberg] = markPriceBloombergWiseList;
                                    _markPriceSymbologyWiseDict[5] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[5].Add(markPriceValue.Bloomberg, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameBloombergDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameBloombergDict.Add(markPriceValue.Bloomberg, markPricelist);
                                _markPriceSymbologyWiseDict.Add(5, markPriceSameBloombergDict);
                            }
                            break;
                        case "OSIOPTIONSYMBOL":
                            uniqueID = markPriceValue.Date + markPriceValue.OSIOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + markPriceValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + markPriceValue.OSIOptionSymbol), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[6];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OSIOptionSymbol))
                                {
                                    List<MarkPriceImport> markPriceOSIWiseList = markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol];
                                    markPriceOSIWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol] = markPriceOSIWiseList;
                                    _markPriceSymbologyWiseDict[6] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[6].Add(markPriceValue.OSIOptionSymbol, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameOSIDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameOSIDict.Add(markPriceValue.OSIOptionSymbol, markPricelist);
                                _markPriceSymbologyWiseDict.Add(6, markPriceSameOSIDict);
                            }
                            break;
                        case "IDCOOPTIONSYMBOL":
                            uniqueID = markPriceValue.Date + markPriceValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + markPriceValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + markPriceValue.IDCOOptionSymbol), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[7];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.IDCOOptionSymbol))
                                {
                                    List<MarkPriceImport> markPriceIDCOWiseList = markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol];
                                    markPriceIDCOWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol] = markPriceIDCOWiseList;
                                    _markPriceSymbologyWiseDict[7] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[7].Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameIDCODict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameIDCODict.Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                                _markPriceSymbologyWiseDict.Add(7, markPriceSameIDCODict);
                            }
                            break;
                        case "OPRAOPTIONSYMBOL":
                            uniqueID = markPriceValue.Date + markPriceValue.OpraOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + markPriceValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + markPriceValue.OpraOptionSymbol), markPriceValue);
                            }
                            if (_markPriceSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[8];
                                if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OpraOptionSymbol))
                                {
                                    List<MarkPriceImport> markPriceOpraWiseList = markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol];
                                    markPriceOpraWiseList.Add(markPriceValue);
                                    markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol] = markPriceOpraWiseList;
                                    _markPriceSymbologyWiseDict[8] = markPriceSameSymbologyDict;
                                }
                                else
                                {
                                    List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                    markPricelist.Add(markPriceValue);
                                    _markPriceSymbologyWiseDict[8].Add(markPriceValue.OpraOptionSymbol, markPricelist);
                                }
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                Dictionary<string, List<MarkPriceImport>> markPriceSameOpraDict = new Dictionary<string, List<MarkPriceImport>>();
                                markPriceSameOpraDict.Add(markPriceValue.OpraOptionSymbol, markPricelist);
                                _markPriceSymbologyWiseDict.Add(7, markPriceSameOpraDict);
                            }
                            break;
                    }
                    #endregion

                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _markPriceValueCollection.Add(markPriceValue);
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
        /// Set symbology if symbology is blank
        /// </summary>
        /// <param name="markPriceObj">mark price object</param>
        private static void SetSymbology(MarkPriceImport markPriceObj)
        {
            try
            {
                if (!String.IsNullOrEmpty(markPriceObj.Symbol))
                    markPriceObj.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.RIC))
                    markPriceObj.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.ISIN))
                    markPriceObj.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.SEDOL))
                    markPriceObj.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.CUSIP))
                    markPriceObj.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.Bloomberg))
                    markPriceObj.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.OSIOptionSymbol))
                    markPriceObj.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.IDCOOptionSymbol))
                    markPriceObj.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(markPriceObj.OpraOptionSymbol))
                    markPriceObj.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
        /// This method is no more used
        /// </summary>
        //private void UpdateValidatedMarkPriceCollection()
        //{
        //    try
        //    {
        //        _validatedMarkPriceValueCollection = new List<MarkPriceImport>();
        //        _validatedMarkPriceValueCollection = _markPriceValueCollection.FindAll(markPrice => markPrice.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()));
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

        public void ValidatePriceTolerances(DataTable dtMarkPriceValueCollection)
        {
            try
            {
                Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>();
                List<string> lstColumnsToKey = new List<string>();
                #region price tolerance check
                if (_runUpload.IsPriceToleranceChecked)
                {
                    //select distict account and symbol
                    var Test = _markPriceValueCollection
                            .Select(markPrice => new { markPrice.AccountID, markPrice.Symbol })
                            .Distinct()
                            .ToList();
                    Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollection = new Dictionary<int, Dictionary<string, double>>();

                    foreach (var item in Test)
                    {
                        Dictionary<string, double> dictSymbolCollection = new Dictionary<string, double>();
                        if (!dictAccountSymbolCollection.ContainsKey(item.AccountID))
                        {
                            dictSymbolCollection.Add(item.Symbol, int.MinValue);
                            dictAccountSymbolCollection.Add(item.AccountID, dictSymbolCollection);
                        }
                        else
                        {
                            if (!dictAccountSymbolCollection[item.AccountID].ContainsKey(item.Symbol))
                            {
                                dictAccountSymbolCollection[item.AccountID].Add(item.Symbol, int.MinValue);
                            }
                        }
                    }
                    //fill last business day mark prices from mark price cache
                    ServiceManager.Instance.PricingServices.InnerChannel.GetMarkPriceForAccountSymbolCollection(ref dictAccountSymbolCollection);

                    DataTable dtLastBusinessDayMarkPrices = new DataTable();
                    dtLastBusinessDayMarkPrices.Columns.Add("FundID", typeof(int));
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

                        DATAReconciler.RecocileData(dtMarkPriceValueCollection, dtLastBusinessDayMarkPrices, lstColumnsToKey, dictColumnsToReconcile, _runUpload.PriceTolerance, ComparisionType.Numeric);
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

        #region IImportITaskHandler Members
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

        #region Local Functions
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
                lock (_timerLock)
                {
                    if (_markPriceValueCollection != null)
                    {
                        DataTable dtMarkPriceValueCollection = GeneralUtilities.GetDataTableFromList(_markPriceValueCollection);
                        ValidatePriceTolerances(dtMarkPriceValueCollection);

                        if (_currentResult != null)
                        {
                            #region write xml for non validated trades
                            int countNonValidatedMarkprices = _markPriceValueCollection.Count(markPrice => markPrice.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedMarkPrice", countNonValidatedMarkprices, null);
                            #endregion

                            #region write xml for validated trades
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedMarkPrice", _validatedMarkPriceValueCollection.Count, null);
                            #endregion

                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", _countSymbols, null);
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", _countNonValidatedSymbols, null);
                            // Purpose : To update symbol validation status if validatedSymbols Xml not exist and trades not partially imported.
                            if (_importedTradesCount == 0 && !_isSaveDataInApplication)
                            {
                                if (_countSymbols == _countValidatedSymbols)
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.Success, null);
                                }
                                else if (_countValidatedSymbols == 0)
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.Failure, null);
                                }
                                else
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.PartialSuccess, null);
                                }
                            }
                        }

                        #region write xml for validated symbols
                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);
                        if (dtValidatedSymbols != null)
                        {
                            AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols);
                        }
                        #endregion

                        #region symbol validation and total mark price added to task statistics

                        if (dtMarkPriceValueCollection != null && dtMarkPriceValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing.
                            //for re-run upload & re-reun Symbol validation it will be false.
                            if (!_isSaveDataInApplication)
                            {
                                UpdateImportDataXML(dtMarkPriceValueCollection);
                            }
                        }

                        int symbolsPendingApprovalCounts = int.MinValue;

                        if (dtValidatedSymbols != null)
                        {
                            //Modified By : Manvendra Prajapati
                            //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8560
                            ValidateSymbols(ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";

                            if (dtValidatedSymbols.Columns.Contains("RequestedSymbol"))
                            {
                                List<DataRow> rows = dtValidatedSymbols.AsEnumerable().Where(r => r.Field<string>("RequestedSymbol") == null || r.Field<string>("RequestedSymbol") == string.Empty).ToList();
                                foreach (var row in rows.ToList())
                                { row.Delete(); }
                            }
                            //if file already exist then merge it to get proper data on symbol validation UI
                            if (File.Exists(Application.StartupPath + _validatedSymbolsXmlFilePath))
                            {
                                DataSet ds = new DataSet();
                                using (FileStream filestream = File.OpenRead(Application.StartupPath + _validatedSymbolsXmlFilePath))
                                {
                                    BufferedStream buffered = new BufferedStream(filestream);
                                    ds.ReadXml(buffered);
                                }

                                if (ds != null && ds.Tables.Count > 0)
                                {
                                    if (_isSaveDataInApplication)
                                    {
                                        DataTable dtValidSymbolsFromXML = ds.Tables[0].Copy();
                                        //add primary key to merge data and avoid making copy of redundant data
                                        DataColumn[] columns = new DataColumn[2];
                                        if (dtValidatedSymbols.Columns.Contains("RequestedSymbol"))
                                        {
                                            columns[0] = dtValidatedSymbols.Columns["RequestedSymbol"];
                                        }
                                        if (dtValidatedSymbols.Columns.Contains("RequestedSymbology"))
                                        {
                                            columns[1] = dtValidatedSymbols.Columns["RequestedSymbology"];
                                        }
                                        dtValidatedSymbols.PrimaryKey = columns;

                                        DataColumn[] keys = dtValidatedSymbols.PrimaryKey.Clone() as DataColumn[];
                                        DataColumn[] keysNew = new DataColumn[keys.Length];
                                        for (int i = 0; i < keys.Length; i++)
                                        {
                                            keysNew[i] = dtValidSymbolsFromXML.Columns[keys[i].ColumnName];
                                        }
                                        dtValidSymbolsFromXML.PrimaryKey = keysNew;

                                        dtValidatedSymbols.Merge(dtValidSymbolsFromXML, true, MissingSchemaAction.Ignore);
                                    }

                                    #region add Task Specific Data

                                    if (_currentResult != null)
                                    {
                                        int countSymbols = dtValidatedSymbols.AsEnumerable()
                                        .Count(row => row.Field<string>("RequestedSymbol") != ""
                                        || row.Field<int>("RequestedSymbology").ToString() == "");
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", countSymbols, null);

                                        int countValidatedSymbols = dtValidatedSymbols.AsEnumerable()
                                                             .Count(row => row.Field<int?>("DataSource") != 0 && row.Field<int?>("DataSource") != null);
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", countValidatedSymbols, _validatedSymbolsXmlFilePath);

                                        int countNonValidatedSymbols = countSymbols - countValidatedSymbols;
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", countNonValidatedSymbols, null);


                                        symbolsPendingApprovalCounts = dtValidatedSymbols.AsEnumerable()
                                                            .Count(row => row.Field<string>("SecApprovalStatus") == "UnApproved");
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                                        // Purpose : To update symbol validation status if not partially imported.
                                        if (_importedTradesCount == 0 && !_isSaveDataInApplication)
                                        {
                                            if (_countSymbols == _countValidatedSymbols && _countSymbols != 0)
                                            {
                                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.Success, null);
                                            }
                                            else if (_countValidatedSymbols == 0)
                                            {
                                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.Failure, null);
                                            }
                                            else
                                            {
                                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.PartialSuccess, null);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _validatedSymbolsXmlFilePath, Encoding.UTF8))
                            {
                                dtValidatedSymbols.WriteXml(xmlWrite);
                            }
                        }
                        #endregion

                        #region add dashboard statistics data

                        if (_currentResult != null)
                        {
                            int accountCounts = _markPriceValueCollection.Select(markPrice => markPrice.AccountID).Distinct().Count();
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FundCount", accountCounts, null);

                            if (symbolsPendingApprovalCounts == int.MinValue)
                            {
                                symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);
                            }
                        }
                        #endregion

                        #region SaveMarkPrices
                        if (_isSaveDataInApplication)
                        {
                            //if (_markPriceValueCollection.Count == _validatedMarkPriceValueCollection.Count)
                            //{
                            //    //Import status will be set to Success
                            //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
                            //    //SaveMarkPrices();
                            //    _bgwImportData.RunWorkerAsync();
                            //    if (TaskSpecificDataPointsPreUpdate != null)
                            //    {
                            //        TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            //        TaskSpecificDataPointsPreUpdate = null;
                            //    }                
                            //    UnwireEvents();
                            //}
                            //else if (_validatedMarkPriceValueCollection.Count == 0)
                            //{
                            //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                            //    if (TaskSpecificDataPointsPreUpdate != null)
                            //    {
                            //        TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            //        TaskSpecificDataPointsPreUpdate = null;
                            //    }                
                            //    UnwireEvents();
                            //}
                            //else
                            //{
                            //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.PartialSuccess, null);
                            //    _bgwImportData.RunWorkerAsync();
                            //    //SaveMarkPrices();
                            //    if (TaskSpecificDataPointsPreUpdate != null)
                            //    {
                            //        TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            //        TaskSpecificDataPointsPreUpdate = null;
                            //    }
                            //    UnwireEvents();
                            //}

                            if (_validatedMarkPriceValueCollection.Count == 0 && _currentResult != null)
                            {
                                if (_importedTradesCount == 0)
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                                }
                                if (TaskSpecificDataPointsPreUpdate != null)
                                {
                                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                    TaskSpecificDataPointsPreUpdate = null;
                                }
                                UnwireEvents();
                            }
                            else
                                SaveMarkPrices();
                            //_bgwImportData.RunWorkerAsync();
                        }
                        #endregion
                        else
                        {
                            if (TaskSpecificDataPointsPreUpdate != null && _currentResult != null)
                            {
                                TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                TaskSpecificDataPointsPreUpdate = null;
                            }
                            UnwireEvents();
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
            finally
            {
                _timerSecurityValidation.Stop();
            }
        }

        private void ValidateSymbols(ref DataTable dtValidatedSymbols)
        {
            try
            {
                List<SecMasterUIObj> lstSecMasterBaseObj = SecMasterHelper.getInstance().ConvertSecMasterBaseObjDataTableToUIObjCollection(dtValidatedSymbols);
                dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(lstSecMasterBaseObj);

                #region commented No reconciler for Mark Price Import
                //Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>();

                ////todo: in current structure all the requested symbol are also set to TickerSymbol
                ////e.g. CUSIP is set to TickerSymbol, Bloomberg set to TickerSymbol
                ////So key to match column is Symbol
                ////TODO: Make generic this mapping
                //dictColumnsToReconcile.Add("Symbol", "RequestedSymbol");
                //dictColumnsToReconcile.Add("CurrencyID", "CurrencyID");
                //dictColumnsToReconcile.Add("Bloomberg", "BloombergSymbol");
                //dictColumnsToReconcile.Add("SEDOL", "SedolSymbol");
                //dictColumnsToReconcile.Add("ISIN", "ISINSymbol");
                //dictColumnsToReconcile.Add("RIC", "ReutersSymbol");
                //dictColumnsToReconcile.Add("CUSIP", "CusipSymbol");
                //dictColumnsToReconcile.Add("Multiplier", "Multiplier");
                //List<string> lstColumnsToKey = new List<string>();
                //lstColumnsToKey.Add("Symbol");

                //DATAReconciler.RecocileData(dtMarkPriceValueCollection, dtValidatedSymbols, lstColumnsToKey, dictColumnsToReconcile, _runUpload.PriceTolerance, ComparisionType.Exact);
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

        ///// <summary>
        ///// Convert sec master base object to sec master UI object
        ///// UI object can be binded to UI easily
        ///// </summary>
        ///// <param name="dtValidatedSymbols"></param>
        //private void ConvertSecMasterBaseObjToUIObj(DataTable dtValidatedSymbols)
        //{
        //    try
        //    {
        //        List<SecMasterUIObj> _secMasterUIobj = new List<SecMasterUIObj>();
        //        foreach (DataRow dr in dtValidatedSymbols.Rows)
        //        {
        //            SecMasterUIObj secMasterUIobj = new SecMasterUIObj();
        //            Transformer.CreateObjThroughReflection(dr, secMasterUIobj);
        //            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)secMasterUIobj.AssetID);

        //            switch (baseAssetCategory)
        //            {
        //                case BusinessObjects.AppConstants.AssetCategory.Option:

        //                    if (dr.Table.Columns.Contains("OPTExpiration") && dr["OPTExpiration"] != System.DBNull.Value)
        //                    {
        //                        DateTime expirationDate = DateTimeConstants.MinValue;
        //                        DateTime.TryParse(dr["OPTExpiration"].ToString(), out expirationDate);
        //                        secMasterUIobj.ExpirationDate = expirationDate;
        //                    }

        //                    if (dr.Table.Columns.Contains("OptionName") && dr["OptionName"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.LongName = dr["OptionName"].ToString();
        //                    }
        //                    if (dr.Table.Columns.Contains("Type") && dr["Type"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.PutOrCall = (Convert.ToInt32(dr["Type"]));
        //                    }
        //                    if (dr.Table.Columns.Contains("OSISymbol") && dr["OSISymbol"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.OSIOptionSymbol = dr["OSISymbol"].ToString();
        //                    }
        //                    if (dr.Table.Columns.Contains("IDCOSymbol") && dr["IDCOSymbol"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.IDCOOptionSymbol = dr["IDCOSymbol"].ToString();
        //                    }
        //                    if (dr.Table.Columns.Contains("OPRASymbol") && dr["OPRASymbol"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.OPRAOptionSymbol = dr["OPRASymbol"].ToString();
        //                    }
        //                    if (dr.Table.Columns.Contains("OPTMultiplier") && dr["OPTMultiplier"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.Multiplier = double.Parse(dr["OPTMultiplier"].ToString());
        //                    }

        //                    break;

        //                case BusinessObjects.AppConstants.AssetCategory.Future:

        //                    if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
        //                    {
        //                        if (dr.Table.Columns.Contains("FUTExpiration") && dr["FUTExpiration"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["FxContractName"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("FxForwardMultiplier") && dr["FxForwardMultiplier"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.Multiplier = double.Parse(dr["FxForwardMultiplier"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (dr.Table.Columns.Contains("FUTExpiration") && dr["FUTExpiration"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FUTMultiplier") && dr["FUTMultiplier"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.Multiplier = double.Parse(dr["FUTMultiplier"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("FutureName") && dr["FutureName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["FutureName"].ToString();
        //                        }
        //                    }
        //                    break;
        //                case BusinessObjects.AppConstants.AssetCategory.FX:

        //                    if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.LongName = dr["FxContractName"].ToString();
        //                    }
        //                    if (dr.Table.Columns.Contains("FxMultiplier") && dr["FxMultiplier"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.Multiplier = Convert.ToDouble(dr["FxMultiplier"].ToString());
        //                    }
        //                    if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
        //                    }
        //                    if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
        //                    }
        //                    if (dr.Table.Columns.Contains("FxExpirationDate") && dr["FxExpirationDate"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FxExpirationDate"].ToString());
        //                    }
        //                    else
        //                    {

        //                    }
        //                    break;
        //                case BusinessObjects.AppConstants.AssetCategory.Indices:
        //                    if (dr.Table.Columns.Contains("IndexLongName") && dr["IndexLongName"] != System.DBNull.Value)
        //                    {
        //                        secMasterUIobj.LongName = dr["IndexLongName"].ToString();
        //                    }
        //                    break;
        //                case BusinessObjects.AppConstants.AssetCategory.FixedIncome:
        //                case BusinessObjects.AppConstants.AssetCategory.ConvertibleBond:
        //                    {
        //                        if (dr.Table.Columns.Contains("FixedIncomeLongName") && dr["FixedIncomeLongName"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.LongName = dr["FixedIncomeLongName"].ToString();
        //                        }
        //                        if (dr.Table.Columns.Contains("FIMultiplier") && dr["FIMultiplier"] != System.DBNull.Value)
        //                        {
        //                            secMasterUIobj.Multiplier = Convert.ToDouble(dr["FIMultiplier"].ToString());
        //                        }
        //                        if (dr.Table.Columns.Contains("MaturityDate") && dr["MaturityDate"] != System.DBNull.Value)
        //{
        //                            secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["MaturityDate"].ToString());
        //                        }
        //                        break;
        //                        //if (dr["CouponFrequency"] != System.DBNull.Value)
        //                        //{
        //                        //    secMasterUIobj.FreqID = Convert.ToInt32(dr["CouponFrequency"].ToString());
        //                        //}
        //}

        //            }
        //            _secMasterUIobj.Insert(_secMasterUIobj.Count, secMasterUIobj);// can use Add but left it unchanged

        //        }
        //        dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(_secMasterUIobj);
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

        /// <summary>
        /// Resets the timer
        /// </summary>
        private void ResetTimer()
        {
            try
            {
                _timerSecurityValidation.Stop();
                _timerSecurityValidation.Start();
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

        ///// <summary>
        ///// This Method Converts SecMaster Base Object into SecMasterUIObject which we convert to DataTable.
        ///// </summary>
        ///// <param name="_secMasterResponseCollection"></param>
        ///// <returns>DataTable created from SecMasterUiObjects List</returns>
        //private DataTable ConvertSecMasterBaseObjToUIObj(List<SecMasterBaseObj> _secMasterResponseCollection)
        //{
        //    DataTable dtValidatedSymbols = null;
        //    try
        //    {
        //        List<SecMasterUIObj> _secMasterUIObject = new List<SecMasterUIObj>();
        //        foreach (SecMasterBaseObj SMObj in _secMasterResponseCollection)
        //        {
        //            SecMasterUIObj smUIObj = new SecMasterUIObj();
        //            Transformer.CreateObjFromObjThroughReflection(SMObj, smUIObj);
        //            SecMasterHelper.SetAssetWiseSMFileds(SMObj, smUIObj);
        //            smUIObj.SymbolType = SymbolType.Unchanged;
        //            _secMasterUIObject.Add(smUIObj);
        //        }
        //        dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(_secMasterUIObject);
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
        //    return dtValidatedSymbols;
        //}


        private void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols)
        {
            try
            {
                foreach (KeyValuePair<string, MarkPriceImport> item in _dictRequestedSymbol)
                {
                    DataRow row = dtValidatedSymbols.NewRow();
                    //if (row.Table.Columns.Contains("IsSecApproved"))
                    //{
                    //    row["IsSecApproved"] = item.Value.Symbol;
                    //}
                    switch (item.Value.Symbology)
                    {
                        case "Symbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["TickerSymbol"] = item.Value.Symbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.Symbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "0";
                            }

                            break;
                        case "RIC":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["ReutersSymbol"] = item.Value.RIC;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.RIC;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "1";
                            }
                            break;
                        case "Bloomberg":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["BloombergSymbol"] = item.Value.Bloomberg;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.Bloomberg;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "5";
                            }
                            break;
                        case "CUSIP":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["CUSIPSymbol"] = item.Value.CUSIP;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.CUSIP;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "4";
                            }
                            break;
                        case "ISIN":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["ISINSymbol"] = item.Value.ISIN;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.ISIN;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "2";
                            }
                            break;
                        case "SEDOL":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["SEDOLSymbol"] = item.Value.SEDOL;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.SEDOL;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "3";
                            }
                            break;
                        case "OSIOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["OSIOptionSymbol"] = item.Value.OSIOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.OSIOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "6";
                            }
                            break;
                        case "IDCOOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["IDCOOptionSymbol"] = item.Value.IDCOOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.IDCOOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "7";
                            }
                            break;
                        case "OpraOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["OPRAOptionSymbol"] = item.Value.OpraOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.OpraOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "8";
                            }
                            break;
                    }
                    dtValidatedSymbols.Rows.Add(row);
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

        ///// <summary>
        ///// Get SymbologyId for corresponding name for symbolwise dictionary
        ///// </summary>
        ///// <param name="symbologyColumnName"></param>
        //private int GetSymbologyID(string symbologyColumnName)
        //{
        //    try
        //    {
        //        switch (symbologyColumnName.ToUpper())
        //        {
        //            case "SYMBOL": return (int)Constants.ImportSymbologies.Symbol;
        //            case "RIC": return (int)Constants.ImportSymbologies.RIC;
        //            case "ISIN": return (int)Constants.ImportSymbologies.ISIN;
        //            case "SEDOL": return (int)Constants.ImportSymbologies.SEDOL;
        //            case "CUSIP": return (int)Constants.ImportSymbologies.CUSIP;
        //            case "BLOOMBERG": return (int)Constants.ImportSymbologies.Bloomberg;
        //            case "OSIOPTIONSYMBOL": return (int)Constants.ImportSymbologies.OSIOptionSymbol;
        //            case "IDCOOPTIONSYMBOL": return (int)Constants.ImportSymbologies.IDCOOptionSymbol;
        //            case "OPRAOPTIONSYMBOL": return (int)Constants.ImportSymbologies.OpraOptionSymbol;
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
        //    return 0;
        //}

        /// <summary>
        /// validate symbol whether there is any Mismatch or not
        /// </summary>
        /// <param name="MarkPriceImport"></param>
        /// <param name="secMasterObj"></param>
        private void validateAllSymbols(MarkPriceImport markPriceObj, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                markPriceObj.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(markPriceObj.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    markPriceObj.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (markPriceObj.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(markPriceObj.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    markPriceObj.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (markPriceObj.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    markPriceObj.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (markPriceObj.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    markPriceObj.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (markPriceObj.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    markPriceObj.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (markPriceObj.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    markPriceObj.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (markPriceObj.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    markPriceObj.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (markPriceObj.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(markPriceObj.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    markPriceObj.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(markPriceObj.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (markPriceObj.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    markPriceObj.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(markPriceObj.MismatchType))
                    {
                        markPriceObj.MismatchType += ", ";
                    }
                    markPriceObj.MismatchType += "Symbol Mismatch";
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
        /// To update Import data xml file
        /// </summary>
        /// <param name="dtMarkPriceValueCollection"></param>
        private void UpdateImportDataXML(DataTable dtMarkPriceValueCollection)
        {
            try
            {
                bool isImportDataXMLUpdated = false;
                dtMarkPriceValueCollection.TableName = "ImportData";

                if (dtMarkPriceValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtMarkPriceValueCollection.Columns["RowIndex"];
                    dtMarkPriceValueCollection.PrimaryKey = columns;
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    String filePath = Application.StartupPath + _totalImportDataXmlFilePath;
                    using (FileStream filestream = File.OpenRead(filePath))
                    {
                        BufferedStream buffered = new BufferedStream(filestream);
                        ds.ReadXml(buffered);
                    }

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        // TODO : For now there were conflict error while merging the table with the column so it is removed.
                        if (dtMarkPriceValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtMarkPriceValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        _importedTradesCount = ds.Tables[0].Select("ImportStatus ='Imported'").Length;
                        if (ds.Tables[0].Columns.Contains("RowIndex"))
                        {
                            while (ds.Tables[0].ChildRelations.Count > 0)
                            {
                                var relation = ds.Tables[0].ChildRelations[0];
                                ds.Tables[relation.ChildTable.TableName].Constraints.Remove(relation.RelationName);
                                ds.Relations.Remove(relation);
                            }

                            while (ds.Tables[0].ParentRelations.Count > 0)
                            {
                                ds.Relations.Remove(ds.Tables[0].ParentRelations[0]);
                            }

                            ds.Tables[0].Constraints.Clear();

                            DataColumn[] columns = new DataColumn[1];
                            columns[0] = ds.Tables[0].Columns["RowIndex"];
                            ds.Tables[0].PrimaryKey = columns;
                        }
                        if (_importedTradesCount > 0 && _currentResult != null && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("IsReRunSymbolValidation") && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["IsReRunSymbolValidation"].ToString().Equals("True"))
                        {
                            ds.Tables[0].Merge(dtMarkPriceValueCollection, true, MissingSchemaAction.Ignore);
                            using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                            {
                                ds.WriteXml(xmlWrite);
                            }
                            isImportDataXMLUpdated = true;
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRunSymbolValidation", false, null);
                        }
                        else
                        {
                            dtMarkPriceValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }
                }
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _markPriceValueCollection.Count, _totalImportDataXmlFilePath);
                if (!isImportDataXMLUpdated)
                {
                    try
                    {
                        using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                        {
                            dtMarkPriceValueCollection.WriteXml(xmlWrite);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        if (_currentResult != null)
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "Process Failed", null);
                            _currentResult.Error = new Exception("The process cannot access the file because it is being used by another process.");
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

        #endregion

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
                if (_bgwImportData != null)
                {
                    _bgwImportData.Dispose();
                }
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