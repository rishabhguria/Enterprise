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
    public sealed class SMPricingDataHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        DataSet _dsSMPricingDataCollection = new DataSet();
        Dictionary<string, DataRow> _uniqueSymbolDictForSecMasterInsert = new Dictionary<string, DataRow>();

        public SMPricingDataHandler() { }

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

        List<DataRow> _secMasterCollection = new List<DataRow>();

        List<DataRow> _validatedSMPricingDataCollection = new List<DataRow>();
        public List<DataRow> ValidatedSMPricingDataCollection
        {
            get
            {
                return _validatedSMPricingDataCollection;
            }
            set
            {
                _validatedSMPricingDataCollection = value;
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

                _dsSMPricingDataCollection = SetCollection(ds);
                if (_dsSMPricingDataCollection.Tables.Count > 0)
                {
                    //TODO: _SMRequest is used to in method FillSecurityMasterDataFromObj in ctrlRunDownoad
                    //_SMRequest = EnmImportType.SecMasterInsertData.ToString();

                    // check if security existing or not.
                    GetSMDataForSecMasterInsertData();

                    //SavePricingDataCollection();
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
                _secMasterResponseCollection.Add(secMasterObj);

                if ((int)ApplicationConstants.SymbologyCodes.TickerSymbol == requestedSymbologyID)
                {
                    if (_uniqueSymbolDictForSecMasterInsert.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        DataRow[] rows = _dsSMPricingDataCollection.Tables[0].Select("TickerSymbol=" + "'" + secMasterObj.RequestedSymbol + "'");
                        foreach (DataRow row in rows)
                        {
                            if (row["TickerSymbol"].ToString().Equals(secMasterObj.RequestedSymbol))
                            {
                                row["SymbolExistsInSM"] = "Exists";

                                // set isSecApproved status of existing security 
                                if (secMasterObj.IsSecApproved)
                                    row[ApplicationConstants.CONST_SEC_APPROVED_STATUS] = ApplicationConstants.CONST_APPROVED;
                                else
                                    row[ApplicationConstants.CONST_SEC_APPROVED_STATUS] = ApplicationConstants.CONST_UN_APPROVED;
                            }

                            //Purpose : To update _validatedSecMasterCollection with validated sec master
                            if (row["ValidationStatus"].ToString().Equals(ApplicationConstants.ValidationStatus.Validated))
                            {
                                _validatedSMPricingDataCollection.Add(row);
                            }
                        }

                        #region remove elements from _requestedSymbols for which we have received response
                        if (_requestedSymbols.Contains(secMasterObj.RequestedSymbol))
                        {
                            _requestedSymbols.Remove(secMasterObj.RequestedSymbol);
                        }
                        #endregion
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
            return "ImportSMPricingData.xsd";
        }

        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            return new DataTable();
        }

        #endregion

        private DataSet SetCollection(DataSet ds)
        {
            _requestedSymbols.Clear();
            _secMasterCollection.Clear();
            _validatedSMPricingDataCollection = new List<DataRow>();
            _secMasterResponseCollection = new List<SecMasterBaseObj>();
            _countSymbols = 0;
            _countValidatedSymbols = 0;

            DataSet dsSecMaster = null;
            try
            {
                _uniqueSymbolDictForSecMasterInsert.Clear();
                DataTable dtSM = ds.Tables[0];

                //add column of Validation status  for each row 
                if (!dtSM.Columns.Contains("ValidationStatus"))
                {
                    DataColumn dcValidated = new DataColumn("ValidationStatus");
                    dcValidated.DataType = typeof(string);
                    dcValidated.DefaultValue = ApplicationConstants.ValidationStatus.None.ToString();
                    dtSM.Columns.Add(dcValidated);
                }
                if (!dtSM.Columns.Contains("AUECID"))
                {
                    DataColumn dcAUECID = new DataColumn("AUECID");
                    dcAUECID.DataType = typeof(Int32);
                    dcAUECID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcAUECID);
                }
                if (!dtSM.Columns.Contains("Symbol"))
                {
                    DataColumn dcTicker = new DataColumn("Symbol");
                    dcTicker.DataType = typeof(string);
                    dcTicker.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcTicker);
                }

                if (!dtSM.Columns.Contains("Symbol_PK"))
                {
                    DataColumn dcSymbol_PK = new DataColumn("Symbol_PK");
                    dcSymbol_PK.DataType = typeof(Int64);
                    dcSymbol_PK.DefaultValue = 0;
                    dtSM.Columns.Add(dcSymbol_PK);
                }

                //Add Approved Status column in table
                if (!dtSM.Columns.Contains(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
                {
                    DataColumn dcApprovalStatus = new DataColumn(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
                    dcApprovalStatus.DataType = typeof(String);
                    dcApprovalStatus.DefaultValue = ApplicationConstants.CONST_UN_APPROVED;
                    dtSM.Columns.Add(dcApprovalStatus);
                }
                if (!dtSM.Columns.Contains("ValidationError"))
                {
                    DataColumn dcValidationError = new DataColumn("ValidationError");
                    dcValidationError.DataType = typeof(string);
                    dcValidationError.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcValidationError);
                }
                if (!dtSM.Columns.Contains("RowIndex"))
                {
                    DataColumn dcRowIndex = new DataColumn("RowIndex");
                    dcRowIndex.DataType = typeof(int);
                    dcRowIndex.DefaultValue = 0;
                    dtSM.Columns.Add(dcRowIndex);
                }
                if (!dtSM.Columns.Contains("ImportStatus"))
                {
                    DataColumn dcImportStatus = new DataColumn("ImportStatus");
                    dcImportStatus.DataType = typeof(string);
                    dcImportStatus.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcImportStatus);
                }
                dsSecMaster = ds.Clone();
                foreach (DataRow row in dtSM.Rows)
                {
                    //if (!_uniqueSymbolDictForSecMasterInsert.ContainsKey(row["Symbol"].ToString()))
                    //{
                    //    _uniqueSymbolDictForSecMasterInsert.Add(row["Symbol"].ToString(), row);
                    //    DataRow dr = dsSecMaster.Tables["PositionMaster"].NewRow();
                    //    dr.ItemArray = row.ItemArray;
                    //    dsSecMaster.Tables["PositionMaster"].Rows.Add(dr);
                    //}

                    _secMasterCollection.Add(row);
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
            return dsSecMaster;
        }

        private void GetSMDataForSecMasterInsertData()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_uniqueSymbolDictForSecMasterInsert.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<string, DataRow> kvp in _uniqueSymbolDictForSecMasterInsert)
                    {
                        if (!string.IsNullOrEmpty(kvp.Key))
                        {
                            _countSymbols++;
                            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                            secMasterRequestObj.AddData(kvp.Key.Trim(), symbology);
                        }

                        if (!_requestedSymbols.Contains(kvp.Key.Trim()))
                        {
                            _requestedSymbols.Add(kvp.Key.Trim());
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        // For Sec Master Import - Sending request for SM data and return  list of already existing SM data  
                        List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                        // To set _validatedSecMasterCollection list for valid import data
                        ValidateImportData();

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                UpdateCollection(secMasterObj, string.Empty);
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

        private List<SecMasterUIObj> ConvertDSToSecMasterInsertValueCollection(DataSet ds)
        {
            List<SecMasterUIObj> secMasterInsertNewDataobj = null;
            try
            {
                IList toReturn = new List<object>();

                secMasterInsertNewDataobj = new List<SecMasterUIObj>();

                Type type = typeof(SecMasterUIObj);
                if (ds.Tables.Count > 0)
                    toReturn = GeneralUtilities.CreateCollectionFromDataTableForSecMaster(ds.Tables[0], type);

                foreach (Object secmasterObj in toReturn)
                {
                    SecMasterUIObj secMasterUpdatedObj = (SecMasterUIObj)secmasterObj;
                    secMasterInsertNewDataobj.Add(secMasterUpdatedObj);
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
            return secMasterInsertNewDataobj;
        }

        private SecMasterbaseList ValidateSecMasterDataBeforeSave(List<SecMasterUIObj> SecMasterListObj)
        {
            SecMasterbaseList lst = new SecMasterbaseList();
            try
            {
                lst.RequestID = System.Guid.NewGuid().ToString();
                foreach (SecMasterUIObj uiObj in SecMasterListObj)
                {
                    SecMasterBaseObj secMasterBaseObj = null;
                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);

                    switch (baseAssetCategory)
                    {
                        case AssetCategory.Equity:
                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                            secMasterBaseObj = new SecMasterEquityObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Option:
                            secMasterBaseObj = new SecMasterOptObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Future:
                            if ((AssetCategory)uiObj.AssetID == AssetCategory.FXForward)
                            {
                                secMasterBaseObj = new SecMasterFXForwardObj();
                                secMasterBaseObj.FillUIData(uiObj);
                            }
                            else
                            {
                                secMasterBaseObj = new SecMasterFutObj();
                                secMasterBaseObj.FillUIData(uiObj);
                            }

                            break;
                        case AssetCategory.FX:
                            secMasterBaseObj = new SecMasterFxObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Indices:
                            secMasterBaseObj = new SecMasterIndexObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.FixedIncome:
                        case AssetCategory.ConvertibleBond:
                            secMasterBaseObj = new SecMasterFixedIncome();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                    }
                    if (secMasterBaseObj != null)
                    {
                        lst.Add(secMasterBaseObj);
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
            return lst;
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
                        DataTable dtSecMasterCollection = GeneralUtilities.GetDataTableFromList(_secMasterCollection);

                        #region write xml for non validated trades
                        //int countNonValidatedSecMaster = dtSecMasterCollection.AsEnumerable().Count(row => !row.Field<string>("ValidationStatus").Equals(ApplicationConstants.ValidationStatus.Validated.ToString()));
                        //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_NonValidatedSSecMaster", countNonValidatedSecMaster, null);

                        #endregion

                        #region write xml for validated trades
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSecMaster", _validatedSMPricingDataCollection.Count, null);
                        #endregion

                        #region write xml for validated symbols

                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        // ConvertSecMasterBaseObjToUIObj(dtValidatedSymbols); 
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", _countSymbols, null);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", _countNonValidatedSymbols, null);
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

                        #endregion

                        #region symbol validation and total order added to task statistics

                        if (dtSecMasterCollection != null && dtSecMasterCollection.Rows.Count > 0)
                        {
                            UpdateImportDataXML(dtSecMasterCollection);
                        }
                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtStagedOrderCollection, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion


                        #region add dashboard statistics data

                        //int accountCounts = _secMasterCollection.Select(stagedOrder => stagedOrder.Level1ID).Distinct().Count();
                        //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCounts, null);

                        int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                        #endregion

                        //Save data in application where _isSaveDataInApplication is true
                        if (_isSaveDataInApplication)
                        {
                            if (_validatedSMPricingDataCollection.Count == 0)
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
                                SavePricingDataCollection();
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
        /// To check valid input data for import
        /// </summary>
        /// <param name="ds"></param>
        private void ValidateImportData()
        {
            try
            {
                if (_uniqueSymbolDictForSecMasterInsert.Count > 0)
                {
                    foreach (DataRow row in _uniqueSymbolDictForSecMasterInsert.Values)
                    {
                        int auecID = 0;
                        int.TryParse(row["AUECID"].ToString(), out auecID);

                        if (row["Symbol"].ToString().Equals(string.Empty))
                        {
                            row["ValidationStatus"] = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            row["ValidationError"] += "Symbol Required";
                        }
                        if (row["AUECID"].ToString().Equals(string.Empty))
                        {
                            row["ValidationStatus"] = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            row["ValidationError"] += "AUEC Required";
                        }
                        else if (auecID <= 0)
                        {
                            row["ValidationStatus"] = ApplicationConstants.ValidationStatus.NotExists.ToString();
                            row["ValidationError"] += "AUEC not exist";
                        }
                        else
                        {
                            row["ValidationStatus"] = ApplicationConstants.ValidationStatus.Validated.ToString();
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
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _validatedSMPricingDataCollection.Count, _totalImportDataXmlFilePath);
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
        /// To save PricingDataCollection in database
        /// </summary>
        private void SavePricingDataCollection()
        {
            int rowsUpdated = 0;
            bool isPartialSuccess = false;
            string resultofValidation = string.Empty;

            try
            {
                List<SecMasterUIObj> secMasterInsertNewDataobj = null;

                if (_dsSMPricingDataCollection != null && _dsSMPricingDataCollection.Tables.Count > 0)
                {
                    secMasterInsertNewDataobj = ConvertDSToSecMasterInsertValueCollection(_dsSMPricingDataCollection);

                    DataTable dtSecMasterCollection = GeneralUtilities.GetDataTableFromList(_secMasterCollection);

                    // insert values into the DB
                    if (secMasterInsertNewDataobj.Count > 0)
                    {
                        SecMasterbaseList lst = ValidateSecMasterDataBeforeSave(secMasterInsertNewDataobj);
                        if (lst.Count > 0)
                        {
                            try
                            {

                                rowsUpdated = lst.Count;
                                //SecurityMasterManager.Instance.SaveNewSymbols_Import(lst);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                //All the trades that are not imported in application due to error, change their status   
                                foreach (DataRow row in dtSecMasterCollection.Rows)
                                {
                                    row["ImportStatus"] = ImportStatus.ImportError.ToString();
                                }
                            }
                        }
                    }
                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtSecMasterCollection.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtSecMasterCollection);
                    }

                    int importedTradesCount = dtSecMasterCollection.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtSecMasterCollection.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (!isPartialSuccess && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
                    }
                    else if (isPartialSuccess && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.PartialSuccess, null);
                    }
                    else
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dsSMPricingDataCollection.Dispose();
                _timerSecurityValidation.Dispose();
            }
        }

        #endregion
    }
}
