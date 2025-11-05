using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Prana.Import
{
    sealed public class OptionModelInputHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        public OptionModelInputHandler() { }

        #region local variables

        const string DATEFORMAT = "MM/dd/yyyy";
        // list collection of option model input
        //List<UserOptModelInput> _OMICollection = new List<UserOptModelInput>();
        List<UserOptModelInput> _OMICollection = new List<UserOptModelInput>();

        List<UserOptModelInput> _validatedOMICollection = new List<UserOptModelInput>();

        Dictionary<int, Dictionary<string, UserOptModelInput>> _OMISymbologyWiseDict = new Dictionary<int, Dictionary<string, UserOptModelInput>>();

        Dictionary<string, UserOptModelInput> _dictRequestedSymbol = new Dictionary<string, UserOptModelInput>();

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
        private System.Timers.Timer _timerSecurityValidation;
        TaskResult _currentResult = new TaskResult();
        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        //private bool _isReimporting = false;
        //public event EventHandler ReImportCompleted;
        private static object _timerLock = new Object();

        /// <summary>
        /// added by: Bharat raturi, 22 may 2014
        /// purpose: flag variable to indicate whether the data is to be saved in application
        /// </summary>
        bool _isSaveDataInApplication = false;
        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;
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
                _isSaveDataInApplication = isSaveDataInApplication;
                _currentResult = taskResult as TaskResult;
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

                SetCollection(ds);
                GetSMDataForOMIImport();
                //SaveOMIValues();
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
            throw new NotImplementedException();
        }

        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            try
            {
                //Symbols which get response either from db or from api or from cache will be validated symbols
                //so we are increasing here counter for validated symbols and adding response to list
                //This list will be used to write the xml of validated and Non validated symbols
                if (secMasterObj.AUECID > 0)
                {
                    _countValidatedSymbols++;
                }
                else
                {
                    _countNonValidatedSymbols++;
                }
                _secMasterResponseCollection.Add(secMasterObj);

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
                int AUECId = secMasterObj.AUECID;
                if (_OMISymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, UserOptModelInput> dictSymbols = _OMISymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion

                        UserOptModelInput omiInput = dictSymbols[secMasterObj.RequestedSymbol];
                        omiInput.Symbol = pranaSymbol;
                        omiInput.CUSIP = cuspiSymbol;
                        omiInput.ISIN = isinSymbol;
                        omiInput.SEDOL = sedolSymbol;
                        omiInput.Bloomberg = bloombergSymbol;
                        omiInput.RIC = reutersSymbol;
                        omiInput.OSIOptionSymbol = osiOptionSymbol;
                        omiInput.IDCOOptionSymbol = idcoOptionSymbol;
                        omiInput.OpraOptionSymbol = opraOptionSymbol;
                        omiInput.AuecID = AUECId;

                        if (omiInput.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                        {
                            lock (_timerLock)
                            {
                                _validatedOMICollection.Add(omiInput);
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

        // TODO : Needs to be removed as hadled in Import setup UI.
        public string GetXSDName()
        {
            return "OptionModelInputs.xsd";
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
                //update positoion master collection
                //Validated PositionMaster objects will be added to _validatedPositionMasterCollection
                UpdateCollection(e.Value, string.Empty);

                //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
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

        #endregion

        /// <summary>
        /// Saves the omi values.
        /// </summary>
        private void SaveOMIValues()
        {
            bool isOMIImport = false;
            bool isPartialSuccess = false;
            string resultofValidation = string.Empty;

            try
            {
                // insert OMI values into the DB
                if (_validatedOMICollection.Count > 0)
                {
                    DataTable dtOMIValueCollection = GeneralUtilities.GetDataTableFromList(_validatedOMICollection);

                    try
                    {
                        // total number of records inserted
                        //totalRecordsCount = _omiValueCollection.Count;
                        _rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveRunUploadFileDataForOMI(_validatedOMICollection);
                        if (_rowsUpdated > 0)
                        {
                            RunUploadManager.UpdateOMI(_rowsUpdated, ref isOMIImport);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtOMIValueCollection.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }
                    if (_currentResult != null)
                    {
                        if (_rowsUpdated > 0)
                        {
                            //All the trades that are imported in application change their status   
                            foreach (DataRow row in dtOMIValueCollection.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.Imported.ToString();
                            }
                            resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                            UpdateImportDataXML(dtOMIValueCollection);
                        }

                        int importedTradesCount = dtOMIValueCollection.Select("ImportStatus ='Imported'").Length;
                        if (importedTradesCount != dtOMIValueCollection.Rows.Count)
                        {
                            isPartialSuccess = true;
                        }

                        if (_OMICollection.Count == _validatedOMICollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
                        }
                        else if ((_OMICollection.Count != _validatedOMICollection.Count || isPartialSuccess) && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.PartialSuccess, null);
                        }
                        else
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// <param name="dtOMIValueCollection"></param>
        private void UpdateImportDataXML(DataTable dtOMIValueCollection)
        {
            try
            {
                dtOMIValueCollection.TableName = "ImportData";
                if (dtOMIValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtOMIValueCollection.Columns["RowIndex"];
                    dtOMIValueCollection.PrimaryKey = columns;
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
                        if (dtOMIValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtOMIValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtOMIValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtOMIValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _OMICollection.Count, _totalImportDataXmlFilePath);
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

        private void GetSMDataForOMIImport()
        {
            try
            {
                if (_OMISymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, UserOptModelInput>> kvp in _OMISymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, UserOptModelInput> symbolDict = _OMISymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, UserOptModelInput> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    //This counter will be used in dashboard xml for total requested symbols
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

        private void SetCollection(DataSet ds)
        {
            try
            {
                _OMISymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                _OMICollection = new List<UserOptModelInput>();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                lock (_timerLock)
                {
                    _validatedOMICollection = new List<UserOptModelInput>();
                }
                _secMasterResponseCollection = new List<SecMasterBaseObj>();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(UserOptModelInput).ToString());
                Dictionary<string, UserOptModelInput> omiValueSameSymbolDict = new Dictionary<string, UserOptModelInput>();

                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    UserOptModelInput OMIvalue = new UserOptModelInput();
                    OMIvalue.RowIndex = irow;
                    OMIvalue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    //ImportHelper.SetProperty(typeToLoad, ds, OMIvalue, irow);

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(OMIvalue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(OMIvalue, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);



                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Boolean"))

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(OMIvalue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(OMIvalue, XmlConvert.ToBoolean((ds.Tables[0].Rows[irow][icol]).ToString()), null);

                                }
                        }
                    }

                    string uniqueID = string.Empty;
                    int reqSymbology = 0;
                    string reqSymbol = string.Empty;

                    #region Creating Symbologywise dictionary

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(OMIvalue.Symbology))
                    {
                        SetSymbology(OMIvalue);
                    }

                    switch (OMIvalue.Symbology.Trim().ToUpper())
                    {
                        case "SYMBOL":
                            uniqueID = OMIvalue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + OMIvalue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + OMIvalue.Symbol), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                            reqSymbol = OMIvalue.Symbol.Trim().ToUpper();
                            break;
                        case "RIC":
                            uniqueID = OMIvalue.RIC.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + OMIvalue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + OMIvalue.RIC), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.ReutersSymbol;
                            reqSymbol = OMIvalue.RIC.Trim().ToUpper();

                            break;
                        case "ISIN":
                            uniqueID = OMIvalue.ISIN.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + OMIvalue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + OMIvalue.ISIN), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.ISINSymbol;
                            reqSymbol = OMIvalue.ISIN.Trim().ToUpper();
                            break;
                        case "SEDOL":
                            uniqueID = OMIvalue.SEDOL.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + OMIvalue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + OMIvalue.SEDOL), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                            reqSymbol = OMIvalue.SEDOL.Trim().ToUpper();
                            break;
                        case "CUSIP":
                            uniqueID = OMIvalue.CUSIP.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + OMIvalue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + OMIvalue.CUSIP), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                            reqSymbol = OMIvalue.CUSIP.Trim().ToUpper();
                            break;
                        case "BLOOMBERG":
                            uniqueID = OMIvalue.Bloomberg.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + OMIvalue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + OMIvalue.Bloomberg), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                            reqSymbol = OMIvalue.Bloomberg.Trim().ToUpper();
                            break;
                        case "OSIOPTIONSYMBOL":
                            uniqueID = OMIvalue.OSIOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + OMIvalue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + OMIvalue.OSIOptionSymbol), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol;
                            reqSymbol = OMIvalue.OSIOptionSymbol.Trim().ToUpper();
                            break;
                        case "IDCOOPTIONSYMBOL":
                            uniqueID = OMIvalue.IDCOOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + OMIvalue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + OMIvalue.IDCOOptionSymbol), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol;
                            reqSymbol = OMIvalue.IDCOOptionSymbol.Trim().ToUpper();
                            break;
                        case "OPRAOPTIONSYMBOL":
                            uniqueID = OMIvalue.OpraOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + OMIvalue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + OMIvalue.OpraOptionSymbol), OMIvalue);
                            }
                            reqSymbology = (int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol;
                            reqSymbol = OMIvalue.OpraOptionSymbol.Trim().ToUpper();
                            break;
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(reqSymbol))
                    {
                        if (_OMISymbologyWiseDict.ContainsKey(reqSymbology))
                        {
                            if (_OMISymbologyWiseDict[reqSymbology].ContainsKey(reqSymbol))
                            {
                                _OMISymbologyWiseDict[reqSymbology][reqSymbol] = OMIvalue;
                            }
                            else
                            {
                                _OMISymbologyWiseDict[reqSymbology].Add(reqSymbol, OMIvalue);
                            }
                        }
                        else
                        {
                            omiValueSameSymbolDict = new Dictionary<string, UserOptModelInput>();
                            omiValueSameSymbolDict.Add(reqSymbol, OMIvalue);
                            _OMISymbologyWiseDict.Add(reqSymbology, omiValueSameSymbolDict);
                        }
                    }
                    if (!omiValueSameSymbolDict.ContainsKey(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        omiValueSameSymbolDict.Add(uniqueID, OMIvalue);
                    }
                    _OMICollection.Add(OMIvalue);
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
        /// <param name="optModelInputObj">OMI object</param>
        private static void SetSymbology(UserOptModelInput optModelInputObj)
        {
            try
            {
                if (!String.IsNullOrEmpty(optModelInputObj.Symbol))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.RIC))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.ISIN))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.SEDOL))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.CUSIP))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.Bloomberg))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.OSIOptionSymbol))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.IDCOOptionSymbol))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(optModelInputObj.OpraOptionSymbol))
                    optModelInputObj.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
                    //string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedOMIValues" + ".xml";
                    //string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedOMIValues" + ".xml";
                    //string validatedSymbolsXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedSymbols" + ".xml";
                    //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";

                    if (_OMICollection != null && _currentResult != null)
                    {
                        DataTable dtOMIValueCollection = GeneralUtilities.GetDataTableFromList(_OMICollection);

                        #region write xml for non validated trades
                        int countNonValidatedOMI = _OMICollection.Count(omiValue => omiValue.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_NonValidatedOMIValues", countNonValidatedOMI, null);
                        #endregion

                        #region write xml for validated trades
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("_ValidatedOMIValues", _validatedOMICollection.Count, null);
                        #endregion

                        #region write xml for validated symbols
                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

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
                            AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols);
                        }
                        #endregion

                        #region symbol validation and total omi added to task statistics

                        if (dtOMIValueCollection != null && dtOMIValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing.
                            //for re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtOMIValueCollection);
                            //}
                        }

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtOMIValueCollection, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        #region add dashboard statistics data

                        //int accountCounts = _OMICollection.Select(positionMaster => positionMaster.AccountID).Distinct().Count();
                        //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCounts, null);

                        int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                        #endregion

                        #region SaveOMIValues
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

                            if (_validatedOMICollection.Count == 0)
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                                if (TaskSpecificDataPointsPreUpdate != null)
                                {
                                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                    TaskSpecificDataPointsPreUpdate = null;
                                }
                                UnwireEvents();
                            }
                        }
                        #endregion
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
                    if (_isSaveDataInApplication && _validatedOMICollection != null && _validatedOMICollection.Count > 0)
                    {
                        SaveOMIValues();
                        _validatedOMICollection.Clear();
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


        private void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols)
        {
            try
            {
                foreach (KeyValuePair<string, UserOptModelInput> item in _dictRequestedSymbol)
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

        /// <summary>
        /// validate symbol whether there is any Mismatch or not
        /// </summary>
        /// <param name="UserOptModelInputObj"></param>
        /// <param name="secMasterObj"></param>
        //private void validateAllSymbols(UserOptModelInput omiObj, SecMasterBaseObj secMasterObj)
        //{
        //    try
        //    {
        //        StringBuilder mismatchComment = new StringBuilder();
        //        bool isSymbolMismatch = false;
        //        markPriceObj.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
        //        if (string.IsNullOrEmpty(markPriceObj.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
        //        {
        //            markPriceObj.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
        //        {
        //            if (markPriceObj.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
        //            }
        //        }
        //        if (string.IsNullOrEmpty(markPriceObj.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
        //        {
        //            markPriceObj.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
        //        {
        //            if (markPriceObj.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
        //        {
        //            markPriceObj.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
        //        {
        //            if (markPriceObj.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
        //        {
        //            markPriceObj.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
        //        {
        //            if (markPriceObj.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
        //        {
        //            markPriceObj.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
        //        {
        //            if (markPriceObj.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
        //        {
        //            markPriceObj.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
        //        {
        //            if (markPriceObj.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
        //        {
        //            markPriceObj.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
        //        {
        //            if (markPriceObj.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
        //            }
        //        }

        //        if (string.IsNullOrEmpty(markPriceObj.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
        //        {
        //            markPriceObj.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
        //        }
        //        else if (!string.IsNullOrEmpty(markPriceObj.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
        //        {
        //            if (markPriceObj.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
        //            {
        //                isSymbolMismatch = true;
        //                mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
        //            }
        //        }

        //        if (mismatchComment.Length > 0)
        //        {
        //            markPriceObj.MisMatchDetails += mismatchComment.ToString();
        //        }
        //        if (isSymbolMismatch)
        //        {
        //            if (!string.IsNullOrEmpty(markPriceObj.MismatchType))
        //            {
        //                markPriceObj.MismatchType += ", ";
        //            }
        //            markPriceObj.MismatchType += "Symbol Mismatch";
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

        #region IDisposable Members

        public void Dispose()
        {
            _timerSecurityValidation.Dispose();
        }

        #endregion
    }
}
