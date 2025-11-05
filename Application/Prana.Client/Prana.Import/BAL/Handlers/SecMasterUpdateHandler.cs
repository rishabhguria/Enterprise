using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Import
{
    sealed public class SecMasterUpdateHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        DataSet _dsSecMasterUpdate = new DataSet();
        Dictionary<string, SecMasterUpdateDataByImportUI> _secMasterUpdateDatauniqueSymbolDict = new Dictionary<string, SecMasterUpdateDataByImportUI>();
        // list collection of update Security Master data 
        SecMasterUpdateDataByImportList _secMasterUpdateDataobj = new SecMasterUpdateDataByImportList();
        int _rowsUpdated = 0;

        public SecMasterUpdateHandler() { }

        #region Local Members

        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        bool _isSaveDataInApplication = false;

        TaskResult _currentResult = new TaskResult();
        private System.Timers.Timer _timerSecurityValidation;
        private static object _timerLock = new Object();
        List<string> _requestedSymbols = new List<string>();

        private List<SecMasterUpdateDataByImportUI> _secMasterCollection = new List<SecMasterUpdateDataByImportUI>();
        public List<SecMasterUpdateDataByImportUI> SecMasterCollection
        {
            get
            {
                return _secMasterCollection;
            }
            set
            {
                _secMasterCollection = value;
            }
        }

        SecMasterUpdateDataByImportList _validatedSecMasterCollection = new SecMasterUpdateDataByImportList();


        private List<SecMasterBaseObj> _secMasterResponseCollection = new List<SecMasterBaseObj>();
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

        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;
        #endregion

        #region Events

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
                UpdateCollection(e.Value, string.Empty);

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

        #region IImportHandler Members

        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _currentResult = taskResult as TaskResult;
                _isSaveDataInApplication = isSaveDataInApplication;

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);

                UpdateSecMasterUpdateDataValueCollection(ds);
                GetSMDataForSecMasterUpdateData();
                //UpdateSecMasterCollection();
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
        /// To update existing secMaster collection in DB
        /// </summary>
        private void UpdateSecMasterCollection()
        {
            try
            {
                string resultofValidation = string.Empty;
                if (_validatedSecMasterCollection.Count > 0)
                {
                    _validatedSecMasterCollection.RequestID = System.Guid.NewGuid().ToString();
                    SecurityMasterManager.Instance.UpdateSymbols_Import(_validatedSecMasterCollection);
                    // TODO : Need to update return type of UpdateSymbols_Import function in SecurityMasterManager class
                    _rowsUpdated = _validatedSecMasterCollection.Count;
                }

                if (_rowsUpdated > 0)
                {
                    resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                }

                if (_currentResult != null)
                {
                    if (_secMasterUpdateDataobj.Count == _validatedSecMasterCollection.Count && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                    }
                    else if ((_secMasterUpdateDataobj.Count != _validatedSecMasterCollection.Count) && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Partial Success", null);
                    }
                    else
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Failure", null);
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
                    //Return TaskResult which was received from ImportManager as event argument
                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                    TaskSpecificDataPointsPreUpdate = null;
                }

            }
        }

        readonly object _object = new object();

        /// <summary>
        /// To update validated and non validated symbols using sec master response
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <param name="collectionKey"></param>
        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            try
            {
                int requestedSymbologyID = secMasterObj.RequestedSymbology;

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
                if ((int)ApplicationConstants.SymbologyCodes.TickerSymbol == requestedSymbologyID)
                {
                    if (_secMasterUpdateDatauniqueSymbolDict.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _requestedSymbols for which we have received response
                        if (_requestedSymbols.Contains(secMasterObj.RequestedSymbol))
                        {
                            _requestedSymbols.Remove(secMasterObj.RequestedSymbol);
                        }
                        #endregion

                        SecMasterUpdateDataByImportUI secMasterUpdateDataObj = _secMasterUpdateDatauniqueSymbolDict[secMasterObj.RequestedSymbol];

                        secMasterUpdateDataObj.TickerSymbol = secMasterObj.TickerSymbol;
                        secMasterUpdateDataObj.AUECID = secMasterObj.AUECID;
                        secMasterUpdateDataObj.ExistingLongName = secMasterObj.LongName;
                        secMasterUpdateDataObj.ExistingMultiplier = secMasterObj.Multiplier;
                        secMasterUpdateDataObj.ExistingUnderlyingSymbol = secMasterObj.UnderLyingSymbol;


                        //Purpose : To update _validatedSecMasterCollection with validated sec master

                        if (secMasterUpdateDataObj.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                        {
                            lock (_object)
                            {
                                _validatedSecMasterCollection.Add(secMasterUpdateDataObj);
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

        /// <summary>
        /// To set validation file name
        /// </summary>
        /// <returns></returns>
        public string GetXSDName()
        {
            return "ImportSecMasterInsertAndUpdate.xsd";
        }

        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            return new DataTable();
        }

        #endregion

        /// <summary>
        /// To set _secMasterUpdateDatauniqueSymbolDict dictionary for requested symbols
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSecMasterUpdateDataValueCollection(DataSet ds)
        {
            try
            {
                _secMasterUpdateDatauniqueSymbolDict.Clear();
                _requestedSymbols.Clear();
                _secMasterCollection.Clear();
                _secMasterUpdateDataobj.Clear();
                _validatedSecMasterCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;

                IList toReturn = new List<object>();

                Type type = typeof(SecMasterUpdateDataByImportUI);
                if (ds.Tables.Count > 0)
                    toReturn = GeneralUtilities.CreateCollectionFromDataTableForSecMaster(ds.Tables[0], type);

                foreach (Object secmasterObj in toReturn)
                {
                    SecMasterUpdateDataByImportUI secMasterUpdatedObj = (SecMasterUpdateDataByImportUI)secmasterObj;
                    secMasterUpdatedObj.AUECID = int.MinValue;
                    if (string.IsNullOrWhiteSpace(secMasterUpdatedObj.TickerSymbol))
                    {
                        secMasterUpdatedObj.ValidationError += "Ticker Symbol Required";
                    }
                    else if (!_secMasterUpdateDatauniqueSymbolDict.ContainsKey(secMasterUpdatedObj.TickerSymbol))
                    {
                        _secMasterUpdateDatauniqueSymbolDict.Add(secMasterUpdatedObj.TickerSymbol, secMasterUpdatedObj);
                        _secMasterUpdateDataobj.Add(secMasterUpdatedObj);

                        #region add elements to _requestedSymbols for which request has to be send
                        if (!_requestedSymbols.Contains(secMasterUpdatedObj.TickerSymbol))
                        {
                            _requestedSymbols.Add(secMasterUpdatedObj.TickerSymbol);
                        }
                        #endregion
                    }
                    _secMasterCollection.Add(secMasterUpdatedObj);
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

        private void GetSMDataForSecMasterUpdateData()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_secMasterUpdateDatauniqueSymbolDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<string, SecMasterUpdateDataByImportUI> kvp in _secMasterUpdateDatauniqueSymbolDict)
                    {
                        if (!string.IsNullOrEmpty(kvp.Key))
                        {
                            _countSymbols++;
                            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                            secMasterRequestObj.AddData(kvp.Key.Trim(), symbology);
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                UpdateCollection(secMasterObj, string.Empty);
                            }
                        }

                        //If we get response for all the requested symbols then call the TimerRefresh_Tick to save the changes
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
                    if (_secMasterCollection != null)
                    {
                        UpdateSecMasterCollectionWithValidImport();
                        // Data for sec master validation from api
                        DataTable dtSecMasterCollection = GeneralUtilities.GetDataTableFromList(_secMasterCollection);

                        #region write xml for validated symbols being updated in DB
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSecMaster", _validatedSecMasterCollection.Count, null);
                        #endregion

                        #region write xml for validated symbols from secmaster
                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", _countSymbols, null);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", (_countSymbols - _countValidatedSymbols), null);
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
                        if (dtValidatedSymbols != null)
                        {
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        #region write xml for non validated symbols from import file

                        int countNonValidatedSecMaster = dtSecMasterCollection.AsEnumerable().Count(row => !row.Field<string>("ValidationStatus").Equals(ApplicationConstants.ValidationStatus.Validated.ToString()));
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_NonValidatedSSecMaster", countNonValidatedSecMaster, null);

                        dtSecMasterCollection.TableName = "ImportData";
                        if (dtSecMasterCollection != null && dtSecMasterCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing.
                            //for re-run upload & re-run Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtSecMasterCollection);
                            //}
                        }
                        #endregion


                        #region add dashboard statistics data

                        int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                        #endregion

                        //Save data in application when _isSaveDataInApplication is true (symbols updated in DB from import file)
                        if (_isSaveDataInApplication)
                        {
                            if (_validatedSecMasterCollection.Count == 0)
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
                                UpdateSecMasterCollection();
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

        /// <summary>
        /// To check valid input data for sec master update import
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSecMasterCollectionWithValidImport()
        {
            try
            {
                if (_secMasterCollection.Count > 0)
                {
                    foreach (SecMasterUpdateDataByImportUI secMasterUpdObj in _secMasterCollection)
                    {
                        if (string.IsNullOrWhiteSpace(secMasterUpdObj.TickerSymbol))
                        {
                            secMasterUpdObj.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            secMasterUpdObj.ValidationError = "Ticker Symbol Required";
                        }
                        if (secMasterUpdObj.AUECID <= 0 || secMasterUpdObj.AUECID == int.MinValue)
                        {
                            secMasterUpdObj.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            secMasterUpdObj.ValidationError = "AUEC not exist";
                        }
                        else
                        {
                            secMasterUpdObj.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
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

        /// <summary>
        /// To update Import data xml file
        /// </summary>
        /// <param name="dtSecMasterCollection"></param>
        private void UpdateImportDataXML(DataTable dtSecMasterCollection)
        {
            try
            {
                dtSecMasterCollection.TableName = "ImportData";
                if (dtSecMasterCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtSecMasterCollection.Columns["RowIndex"];
                    dtSecMasterCollection.PrimaryKey = columns;
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (dtSecMasterCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtSecMasterCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtSecMasterCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtSecMasterCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _validatedSecMasterCollection.Count, _totalImportDataXmlFilePath);
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

        #region IDisposable Members

        public void Dispose()
        {
            _dsSecMasterUpdate.Dispose();
            _timerSecurityValidation.Dispose();
        }

        #endregion
    }
}
