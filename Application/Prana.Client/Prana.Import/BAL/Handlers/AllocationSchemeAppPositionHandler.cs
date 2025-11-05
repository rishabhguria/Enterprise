using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
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
    class AllocationSchemeAppPositionHandler : IImportHandler, IImportITaskHandler, IDisposable
    {

        private static object syncRoot = new Object();

        public AllocationSchemeAppPositionHandler() { }

        #region Local Members

        List<string> _requestedSymbols = new List<string>();
        //private BackgroundWorker _bgSaveCollection = new BackgroundWorker();

        Dictionary<string, Data> _collection = new Dictionary<string, Data>();
        struct Data
        {
            public DataSet DataSetAllocationScheme;
            public string FileName;
        }

        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        bool _isSaveDataInApplication = false;
        //private bool _isReimporting = false;
        //private string _filePath = string.Empty;
        TaskResult _currentResult = new TaskResult();
        private System.Timers.Timer _timerSecurityValidation;
        private static object _timerLock = new Object();

        DataSet _dsAllocationSchemeCollection = new DataSet();

        List<DataRow> _validatedAllocationSchemeCollection = new List<DataRow>();
        public List<DataRow> ValidatedAllocationSchemeCollection
        {
            get
            {
                return _validatedAllocationSchemeCollection;
            }
            set
            {
                _validatedAllocationSchemeCollection = value;
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

        #region Events

        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                //_bgSaveCollection.DoWork += new DoWorkEventHandler(_bgSaveCollection_DoWork);
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
                //_bgSaveCollection.DoWork -= new DoWorkEventHandler(_bgSaveCollection_DoWork);
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

        #region IImportHandler Members

        public void ProcessRequest(DataSet dataSet, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _currentResult = taskResult as TaskResult;
                _isSaveDataInApplication = isSaveDataInApplication;
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
                SetCollection(dataSet, runUpload);
                ValidateImportData();
                //SaveCollection();
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

        public void UpdateCollection(SecMasterBaseObj secMasterObj, string key)
        {
            try
            {
                //It must be in lock as update will be come from SecurityMasterManager's OnResponse event
                lock (syncRoot)
                {
                    string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //Symbols which get response either from db or from api or from cache will be validated symbols
                    //so we are increasing here counter for validated symbols and adding response to list
                    //This list will be used to write the xml of validtaed symbols
                    if (secMasterObj.AUECID > 0)
                    {
                        _countValidatedSymbols++;
                    }
                    else
                    {
                        _countNonValidatedSymbols++;
                    }
                    _secMasterResponseCollection.Add(secMasterObj);

                    if (_requestedSymbols.Count > 0 && _requestedSymbols.Contains(pranaSymbol))
                    {
                        _requestedSymbols.Remove(pranaSymbol);

                        foreach (KeyValuePair<string, Data> kvp in _collection)
                        {
                            if (kvp.Value.DataSetAllocationScheme != null && kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in kvp.Value.DataSetAllocationScheme.Tables[0].Select("Symbol = '" + pranaSymbol + "'"))
                                {
                                    row["IsSymbolValidatedFromSM"] = ApplicationConstants.ValidationStatus.Validated;

                                    // TODO : ValidationStatus needs to be implemented for correct import data
                                    row["Validated"] = ApplicationConstants.ValidationStatus.Validated;
                                }
                                kvp.Value.DataSetAllocationScheme.AcceptChanges();
                            }
                        }
                    }
                    else
                    {
                        SaveCollection();
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
            return "ImportAllocationScheme.xsd";
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
                ImportDataManager.SaveImportedFileDetails(runUpload.FileName, runUpload.FilePath, runUpload.ImportTypeAcronym, runUpload.FileLastModifiedUTCTime);
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

        private void SetCollection(DataSet dataSet, RunUpload runUpload)
        {
            try
            {
                _collection.Clear();
                _requestedSymbols.Clear();
                _dsAllocationSchemeCollection.Clear();
                _validatedAllocationSchemeCollection = new List<DataRow>();
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                _countSymbols = 0;
                _countValidatedSymbols = 0;

                if (dataSet != null)
                {
                    lock (syncRoot)
                    {
                        if (!_collection.ContainsKey(runUpload.Key))
                        {
                            _collection.Add(runUpload.Key, new Data());

                            //string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                            SecurityMasterManager.Instance.GenerateSMMapping(dataSet);

                            List<string> currencyListForAlloScheme = ServiceManager.Instance.AllocationServices.InnerChannel.GetCurrencyListForAllocationScheme();
                            //_dsAllocationScheme = AllocationSchemeImportHelper.UpdateAllocationSchemeValueCollection(dataSet, currencyListForAlloScheme, false);
                            Dictionary<int, Dictionary<string, List<DataRow>>> allocationSchemeSymbologyWiseDict = AllocationSchemeImportHelper.AllocationSchemeSymbologyWiseDict;

                            if (_collection[runUpload.Key].DataSetAllocationScheme == null)
                            {
                                Data data = _collection[runUpload.Key];
                                data.FileName = runUpload.FileName;
                                data.DataSetAllocationScheme = AllocationSchemeImportHelper.UpdateAllocationSchemeValueCollection(dataSet, currencyListForAlloScheme, false);
                                _collection[runUpload.Key] = data;
                            }

                            if (_collection[runUpload.Key].DataSetAllocationScheme != null && _collection[runUpload.Key].DataSetAllocationScheme.Tables.Count > 0 && allocationSchemeSymbologyWiseDict != null && allocationSchemeSymbologyWiseDict.Count > 0)
                            {
                                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                                foreach (KeyValuePair<int, Dictionary<string, List<DataRow>>> kvp in allocationSchemeSymbologyWiseDict)
                                {
                                    ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;
                                    Dictionary<string, List<DataRow>> symbolDict = allocationSchemeSymbologyWiseDict[kvp.Key];

                                    if (symbolDict.Count > 0)
                                    {
                                        foreach (KeyValuePair<string, List<DataRow>> keyvaluepair in symbolDict)
                                        {
                                            if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                            {
                                                _countSymbols++;
                                                secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                            }

                                            if (!_requestedSymbols.Contains(keyvaluepair.Key))
                                            {
                                                _requestedSymbols.Add(keyvaluepair.Key);
                                            }
                                        }
                                    }
                                }
                                _dsAllocationSchemeCollection = _collection[runUpload.Key].DataSetAllocationScheme;
                                GetSMDataForAllocationScheme(secMasterRequestObj, runUpload.Key);
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

        private void GetSMDataForAllocationScheme(SecMasterRequestObj secMasterRequestObj, string key)
        {
            try
            {
                if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                {
                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SecurityMaster.SendRequestList(secMasterRequestObj);
                    if (!_timerSecurityValidation.Enabled)
                    {
                        _timerSecurityValidation.Enabled = true;
                    }
                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            UpdateCollection(secMasterObj, key);
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

        private void SaveCollection()
        {
            if (_timerSecurityValidation.Enabled)
            {
                _timerSecurityValidation.Stop();
            }
            //_bgSaveCollection.RunWorkerAsync();

            try
            {
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                int _allocationschemeID = int.MinValue;

                foreach (KeyValuePair<string, Data> kvp in _collection)
                {
                    if (kvp.Value.DataSetAllocationScheme != null && kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in kvp.Value.DataSetAllocationScheme.Tables[0].Select("IsSymbolValidatedFromSM <> 'Validated'"))
                        {
                            row.Delete();
                        }
                        kvp.Value.DataSetAllocationScheme.AcceptChanges();

                        DataSet tempDataSetAllocationScheme = new DataSet();
                        tempDataSetAllocationScheme = kvp.Value.DataSetAllocationScheme.Copy();

                        DataTable tempDataTableAllocationScheme = new DataTable();
                        tempDataTableAllocationScheme = kvp.Value.DataSetAllocationScheme.Tables[0];

                        if (kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
                        {
                            //// remove extra columns  
                            tempDataSetAllocationScheme = AllocationSchemeImportHelper.GetUpdatedDataSet(tempDataSetAllocationScheme);

                            try
                            {
                                // insert data into the DB

                                AllocationFixedPreference fixedPref = new AllocationFixedPreference(int.MinValue, kvp.Value.FileName, tempDataSetAllocationScheme.GetXml(), DateTime.Now, true, FixedPreferenceCreationSource.SchemeImport);
                                _allocationschemeID = _allocationschemeID = ServiceManager.Instance.AllocationServices.InnerChannel.SaveAllocationScheme(fixedPref);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                //All the trades that are not imported in application due to error, change their status   
                                foreach (DataRow row in tempDataTableAllocationScheme.Rows)
                                {
                                    row["ImportStatus"] = ImportStatus.ImportError.ToString();
                                }
                            }

                            if (_allocationschemeID > 0)
                            {
                                //All the trades that are imported in application change their status   
                                foreach (DataRow row in tempDataTableAllocationScheme.Rows)
                                {
                                    row["ImportStatus"] = ImportStatus.Imported.ToString();
                                }
                                resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                                UpdateImportDataXML(tempDataTableAllocationScheme);
                            }

                            int importedTradesCount = tempDataTableAllocationScheme.Select("ImportStatus ='Imported'").Length;
                            if (importedTradesCount != tempDataTableAllocationScheme.Rows.Count)
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
                }
                // _requestedSymbols.Clear();
                //_collection.Clear();
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
        /// <param name="dtSecMasterCollection"></param>
        private void UpdateImportDataXML(DataTable dtSecMasterCollection)
        {
            try
            {
                DataTable tempdtAllocationScheme = new DataTable();
                tempdtAllocationScheme = dtSecMasterCollection.Copy();

                tempdtAllocationScheme.TableName = "ImportData";

                if (tempdtAllocationScheme.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = tempdtAllocationScheme.Columns["RowIndex"];
                    tempdtAllocationScheme.PrimaryKey = columns;
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1200
                        // TODO : For now there were conflict error while merging the table with the column so it is removed.
                        if (tempdtAllocationScheme.Columns.Contains("BrokenRulesCollection"))
                        {
                            tempdtAllocationScheme.Columns.Remove("BrokenRulesCollection");
                        }
                        tempdtAllocationScheme.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }

                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", tempdtAllocationScheme.Rows.Count, _totalImportDataXmlFilePath);

                //modified by omshiv, writing XML using XmlTextWriter
                using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _validatedSymbolsXmlFilePath, Encoding.UTF8))
                {
                    tempdtAllocationScheme.WriteXml(xmlWrite);
                }
                // tempdtAllocationScheme.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
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

        //void _bgSaveCollection_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<string, Data> kvp in _collection)
        //        {
        //            if (kvp.Value.DataSetAllocationScheme != null && kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow row in kvp.Value.DataSetAllocationScheme.Tables[0].Select("IsSymbolValidatedFromSM <> 'Validated'"))
        //                {
        //                    row.Delete();
        //                }
        //                kvp.Value.DataSetAllocationScheme.AcceptChanges();

        //                if (kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
        //                {
        //                    //// remove extra columns  
        //                    DataSet tempDataSetAllocationScheme = AllocationSchemeImportHelper.GetUpdatedDataSet(kvp.Value.DataSetAllocationScheme);

        //                    // insert data into the DB
        //                    int _allocationschemeID = _allocationschemeID = ServiceManager.Instance.AllocationServices.InnerChannel.SaveAllocationScheme(kvp.Value.FileName, DateTime.Now, tempDataSetAllocationScheme.GetXml(), int.MinValue);
        //                }
        //            }
        //        }
        //        _requestedSymbols.Clear();
        //        _collection.Clear();
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

        private void TimerSecurityValidation_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (_timerLock)
                {
                    //string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedAllocationSchemes" + ".xml";
                    //string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedAllocationSchemes" + ".xml";
                    //string validatedSymbolsXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedSymbols" + ".xml";
                    //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";

                    // TODO : should be called in procesRequest()

                    foreach (KeyValuePair<string, Data> kvp in _collection)
                    {
                        if (kvp.Value.DataSetAllocationScheme != null && kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in kvp.Value.DataSetAllocationScheme.Tables[0].Select("Validated = 'Validated'"))
                            {
                                _validatedAllocationSchemeCollection.Add(row);
                            }
                        }
                    }

                    if (_dsAllocationSchemeCollection != null)
                    {
                        DataTable dtSecMasterCollection = new DataTable();
                        //DataTable dtSecMasterCollection = GeneralUtilities.GetDataTableFromList(_allocationSchemeCollection);
                        //ValidatePriceTolerances(dtPositionMasterCollection);

                        #region write xml for non validated trades
                        int countNonValidatedAllocationSchemes = 0;

                        dtSecMasterCollection = _dsAllocationSchemeCollection.Tables[0];
                        countNonValidatedAllocationSchemes = dtSecMasterCollection.AsEnumerable().Count(row => !row.Field<string>("Validated").Equals(ApplicationConstants.ValidationStatus.Validated.ToString()));

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_NonValidatedAllocationSchemes", countNonValidatedAllocationSchemes, null);
                        #endregion

                        #region update task for validated trades

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_ValidatedAllocationSchemes", _validatedAllocationSchemeCollection.Count, null);
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
                        //if (dtValidatedSymbols != null)
                        //{
                        //    AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols);
                        //}
                        #endregion

                        #region symbol validation and total taxlots added to task statistics

                        if (dtSecMasterCollection != null && dtSecMasterCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing.
                            //for re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtSecMasterCollection);
                            //}
                        }
                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtSecMasterCollection, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            using (XmlTextWriter xmlWriter = new XmlTextWriter(Application.StartupPath + _validatedSymbolsXmlFilePath, Encoding.UTF8))
                            {
                                dtValidatedSymbols.WriteXml(xmlWriter);
                            }
                            // dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion


                        #region add dashboard statistics data

                        int accountCounts = 0;

                        // TODO : needs to improve linq query to handle generic datatype
                        Type dataType = _dsAllocationSchemeCollection.Tables[0].Columns["FundID"].DataType;

                        dtSecMasterCollection = _dsAllocationSchemeCollection.Tables[0];
                        if (dataType.Equals(typeof(int)))
                        {
                            var dtRows = (from row in dtSecMasterCollection.AsEnumerable()
                                          select row.Field<int>("FundID"));
                            int.TryParse(dtRows.Distinct().Count().ToString(), out accountCounts);
                        }
                        else if (dataType.Equals(typeof(string)))
                        {
                            var dtRows = (from row in dtSecMasterCollection.AsEnumerable()
                                          select row.Field<string>("FundID"));
                            int.TryParse(dtRows.Distinct().Count().ToString(), out accountCounts);
                        }

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FundCount", accountCounts, null);

                        int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                        #endregion
                        //Validated PositionMaster objects will be added to _validatedPositionMasterCollections
                        //Save data in application where _isSaveDataInApplication is true
                        if (_isSaveDataInApplication)
                        {
                            if (_validatedAllocationSchemeCollection.Count == 0)
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
                                SaveCollection();
                            // _bgwImportData.RunWorkerAsync();
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
        }

        /// <summary>
        /// To check valid input data for import
        /// </summary>
        private void ValidateImportData()
        {
            try
            {
                if (_collection.Count > 0)
                {
                    foreach (KeyValuePair<string, Data> kvp in _collection)
                    {
                        if (kvp.Value.DataSetAllocationScheme != null && kvp.Value.DataSetAllocationScheme.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in kvp.Value.DataSetAllocationScheme.Tables[0].Rows)
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(row["FundName"].ToString());

                                if (row["FundName"].ToString().Equals(string.Empty))
                                {
                                    row["Validated"] = ApplicationConstants.ValidationStatus.MissingData.ToString();
                                    row["ValidationError"] += "Account Required";
                                }
                                if (row["Quantity"].ToString().Equals(string.Empty))
                                {
                                    row["Validated"] = ApplicationConstants.ValidationStatus.MissingData.ToString();
                                    row["ValidationError"] += "Quantity Required";
                                }
                                if (row["Symbol"].ToString().Equals(string.Empty))
                                {
                                    row["Validated"] = ApplicationConstants.ValidationStatus.MissingData.ToString();
                                    row["ValidationError"] += "Symbol Required";
                                }
                                else if (accountID <= 0)
                                {
                                    row["Validated"] = ApplicationConstants.ValidationStatus.NotExists.ToString();
                                    row["ValidationError"] += "Account not exist";
                                }
                                else
                                {
                                    row["Validated"] = ApplicationConstants.ValidationStatus.Validated.ToString();
                                }
                            }
                            kvp.Value.DataSetAllocationScheme.AcceptChanges();
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
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timerSecurityValidation.Dispose();
                _dsAllocationSchemeCollection.Dispose();
            }
        }

        #endregion
    }
}
