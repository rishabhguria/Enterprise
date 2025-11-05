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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Import
{
    /// <summary>
    /// 
    /// </summary>
    sealed public class DailyVWAPHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables
        // list collection of import vWAP values
        /// <summary>
        /// The _vWAP value collection
        /// </summary>
        List<VWAPImport> _vWAPValueCollection = new List<VWAPImport>();
        /// <summary>
        /// The _current result
        /// </summary>
        TaskResult _currentResult = new TaskResult();
        /// <summary>
        /// The _validated vWAP value collection
        /// </summary>
        List<VWAPImport> _validatedVWAPValueCollection = new List<VWAPImport>();
        //private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);
        /// <summary>
        /// The _timer security validation
        /// </summary>
        private System.Timers.Timer _timerSecurityValidation;
        /// <summary>
        /// The _is save data in application
        /// </summary>
        bool _isSaveDataInApplication = false;
        /// <summary>
        /// The _count symbols
        /// </summary>
        int _countSymbols = 0;
        /// <summary>
        /// The _count validated symbols
        /// </summary>
        int _countValidatedSymbols = 0;
        /// <summary>
        /// The _count non validated symbols
        /// </summary>
        int _countNonValidatedSymbols = 0;
        /// <summary>
        /// The _vWAP symbology wise dictionary
        /// </summary>
        Dictionary<int, Dictionary<string, List<VWAPImport>>> _vWAPSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<VWAPImport>>>();

        /// <summary>
        /// The dateformat
        /// </summary>
        const string DATEFORMAT = "MM/dd/yyyy";
        /// <summary>
        /// Occurs when [task specific data points pre update].
        /// </summary>
        public event EventHandler TaskSpecificDataPointsPreUpdate;

        /// <summary>
        /// The _validated symbols XML file path
        /// </summary>
        private string _validatedSymbolsXmlFilePath = string.Empty;
        /// <summary>
        /// The _total import data XML file path
        /// </summary>
        private string _totalImportDataXmlFilePath = string.Empty;

        /// <summary>
        /// The _dict requested symbol
        /// </summary>
        Dictionary<string, VWAPImport> _dictRequestedSymbol = new Dictionary<string, VWAPImport>();

        /// <summary>
        /// The _sec master response collection
        /// </summary>
        List<SecMasterBaseObj> _secMasterResponseCollection = new List<SecMasterBaseObj>();
        /// <summary>
        /// Gets or sets the sec master response collection.
        /// </summary>
        /// <value>
        /// The sec master response collection.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyVWAPHandler"/> class.
        /// </summary>
        public DailyVWAPHandler() { }

        #region IImportHandler Members
        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="runUpload">The run upload.</param>
        /// <param name="taskResult">The task result.</param>
        /// <param name="isSaveDataInApplication">if set to <c>true</c> [is save data in application].</param>
        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _currentResult = taskResult as TaskResult;

                SecurityMasterManager.Instance.GenerateSMMapping(ds);
                _isSaveDataInApplication = isSaveDataInApplication;

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);


                SetCollection(ds, runUpload);
                GetSMDataForVWAPImport();


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
        /// Saves the vWAP values.
        /// </summary>
        private void SaveVWAPValues()
        {
            try
            {
                int rowsUpdated = 0;
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                //UpdateValidatedDailyVWAPCollection();
                if (_validatedVWAPValueCollection.Count > 0)
                {
                    // total number of records inserted

                    DataTable dtVWAPValues = CreateDataTableForVWAPImport();
                    DataTable dtVWAPValueImportData = GeneralUtilities.GetDataTableFromList(_validatedVWAPValueCollection);

                    DataTable dtVWAPTableFromCollection = GeneralUtilities.CreateTableFromCollection<VWAPImport>(dtVWAPValues, _validatedVWAPValueCollection);

                    try
                    {
                        if ((dtVWAPTableFromCollection != null) && (dtVWAPTableFromCollection.Rows.Count > 0))
                        {
                            rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveVWAP(dtVWAPTableFromCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtVWAPValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }

                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtVWAPValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtVWAPValueImportData);
                    }

                    int importedTradesCount = dtVWAPValueImportData.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtVWAPValueImportData.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (_currentResult != null)
                    {
                        if (_vWAPValueCollection.Count == _validatedVWAPValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                        }
                        else if ((_vWAPValueCollection.Count != _validatedVWAPValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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
                if (TaskSpecificDataPointsPreUpdate != null && _currentResult != null)
                {
                    //Return TaskResult which was recieved from ImportManager as event argument
                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                    TaskSpecificDataPointsPreUpdate = null;
                }

            }
        }

        /// <summary>
        /// Updates the collection.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        /// <param name="collectionKey">The collection key.</param>
        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            try
            {
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
                //Symbols which get response either from db or from api or from cache will be validated symbols
                //so we are increasing here counter for validated symbols and adding response to list
                //This list will be used to write the xml of validated symbols
                if (secMasterObj.AUECID > 0)
                {
                    _countValidatedSymbols++;
                }
                else
                {
                    _countNonValidatedSymbols++;
                }
                _secMasterResponseCollection.Add(secMasterObj);

                if (_vWAPSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<VWAPImport>> dictSymbols = _vWAPSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<VWAPImport> listVWAPValues = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (VWAPImport vWAPImport in listVWAPValues)
                        {
                            validateAllSymbols(vWAPImport, secMasterObj);

                            vWAPImport.Symbol = pranaSymbol;
                            vWAPImport.CUSIP = cuspiSymbol;
                            vWAPImport.ISIN = isinSymbol;
                            vWAPImport.SEDOL = sedolSymbol;
                            vWAPImport.Bloomberg = bloombergSymbol;
                            vWAPImport.RIC = reutersSymbol;
                            vWAPImport.OSIOptionSymbol = osiOptionSymbol;
                            vWAPImport.IDCOOptionSymbol = idcoOptionSymbol;
                            vWAPImport.OpraOptionSymbol = opraOptionSymbol;
                            vWAPImport.AUECID = secMasterObj.AUECID;

                            if (vWAPImport.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                _validatedVWAPValueCollection.Add(vWAPImport);
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
        /// <summary>
        /// Gets the name of the XSD.
        /// </summary>
        /// <returns></returns>
        public string GetXSDName()
        {
            return "ImportVWAPValues.xsd";
        }

        #endregion

        /// <summary>
        /// Sets the collection.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="runUpload">The run upload.</param>
        void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _vWAPSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                //_vWAPValueCollection.Clear();
                _vWAPValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(VWAPImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    VWAPImport vWAPValue = new VWAPImport();
                    vWAPValue.Symbol = string.Empty;
                    vWAPValue.VWAP = 0;
                    vWAPValue.Date = string.Empty;
                    vWAPValue.AUECID = 0;
                    vWAPValue.RowIndex = irow;
                    vWAPValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, vWAPValue, irow);

                    //TODO: Need to handle user selected date 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        vWAPValue.Date = dtn.ToString(DATEFORMAT);
                    }
                    else
                    {
                        if (!vWAPValue.Date.Equals(string.Empty))
                        {
                            bool isParsed = false;
                            double outResult;
                            isParsed = double.TryParse(vWAPValue.Date, out outResult);
                            if (isParsed)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(vWAPValue.Date));
                                vWAPValue.Date = dtn.ToString(DATEFORMAT);
                            }
                            else
                            {
                                try
                                {
                                    DateTime dtn = Convert.ToDateTime(vWAPValue.Date);
                                    vWAPValue.Date = dtn.ToString(DATEFORMAT);
                                }
                                // To check invalid date format from xslt
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                    vWAPValue.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                    if (!Regex.IsMatch(vWAPValue.ValidationError, "Invalid Date Format", RegexOptions.IgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(vWAPValue.ValidationError))
                                            vWAPValue.ValidationError += Seperators.SEPERATOR_8;
                                        vWAPValue.ValidationError += " Invalid Date Format ";
                                    }
                                }
                            }
                        }
                    }
                    string uniqueID = string.Empty;

                    //if (string.IsNullOrEmpty(vWAPValue.Symbology))
                    //{
                    //    if (!string.IsNullOrEmpty(vWAPValue.Symbol))
                    //    {
                    //        vWAPValue.Symbology = "Symbol";
                    //    }
                    //}

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(vWAPValue.Symbology))
                    {
                        SetSymbology(vWAPValue);
                    }

                    switch (vWAPValue.Symbology.Trim().ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(vWAPValue.Symbol))
                        case "SYMBOL":
                            vWAPValue.Symbol = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + vWAPValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + vWAPValue.Symbol), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.Symbol.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[0];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.Symbol))
                                {
                                    List<VWAPImport> vWAPSymbolWiseList = vWAPSameSymbologyDict[vWAPValue.Symbol];
                                    vWAPSymbolWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.Symbol] = vWAPSymbolWiseList;
                                    _vWAPSymbologyWiseDict[0] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[0].Add(vWAPValue.Symbol, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbolDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameSymbolDict.Add(vWAPValue.Symbol, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(0, vWAPSameSymbolDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(vWAPValue.RIC))
                        case "RIC":
                            vWAPValue.RIC = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + vWAPValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + vWAPValue.RIC), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.RIC.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[1];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.RIC))
                                {
                                    List<VWAPImport> vWAPRICWiseList = vWAPSameSymbologyDict[vWAPValue.RIC];
                                    vWAPRICWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.RIC] = vWAPRICWiseList;
                                    _vWAPSymbologyWiseDict[1] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPList = new List<VWAPImport>();
                                    vWAPList.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[1].Add(vWAPValue.RIC, vWAPList);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPList = new List<VWAPImport>();
                                vWAPList.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> VWAPSameRICDict = new Dictionary<string, List<VWAPImport>>();
                                VWAPSameRICDict.Add(vWAPValue.RIC, vWAPList);
                                _vWAPSymbologyWiseDict.Add(1, VWAPSameRICDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(vWAPValue.ISIN))
                        case "ISIN":
                            vWAPValue.ISIN = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + vWAPValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + vWAPValue.ISIN), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.ISIN.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[2];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.ISIN))
                                {
                                    List<VWAPImport> vWAPISINWiseList = vWAPSameSymbologyDict[vWAPValue.ISIN];
                                    vWAPISINWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.ISIN] = vWAPISINWiseList;
                                    _vWAPSymbologyWiseDict[2] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[2].Add(vWAPValue.ISIN, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameISINDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameISINDict.Add(vWAPValue.ISIN, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(2, vWAPSameISINDict);
                            }
                            break;

                        case "SEDOL":
                            vWAPValue.SEDOL = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + vWAPValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + vWAPValue.SEDOL), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.SEDOL.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[3];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.SEDOL))
                                {
                                    List<VWAPImport> vWAPSEDOLWiseList = vWAPSameSymbologyDict[vWAPValue.SEDOL];
                                    vWAPSEDOLWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.SEDOL] = vWAPSEDOLWiseList;
                                    _vWAPSymbologyWiseDict[3] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[3].Add(vWAPValue.SEDOL, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSEDOLDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSEDOLDict.Add(vWAPValue.SEDOL, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(3, vWAPSEDOLDict);
                            }
                            break;

                        case "CUSIP":
                            vWAPValue.CUSIP = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + vWAPValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + vWAPValue.CUSIP), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.CUSIP.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[4];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.CUSIP))
                                {
                                    List<VWAPImport> vWAPCUSIPWiseList = vWAPSameSymbologyDict[vWAPValue.CUSIP];
                                    vWAPCUSIPWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.CUSIP] = vWAPCUSIPWiseList;
                                    _vWAPSymbologyWiseDict[4] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[4].Add(vWAPValue.CUSIP, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameCUSIPDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameCUSIPDict.Add(vWAPValue.CUSIP, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(4, vWAPSameCUSIPDict);
                            }
                            break;
                        case "BLOOMBERG":
                            vWAPValue.Bloomberg = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + vWAPValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + vWAPValue.Bloomberg), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.Bloomberg.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[5];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.Bloomberg))
                                {
                                    List<VWAPImport> vWAPBloombergWiseList = vWAPSameSymbologyDict[vWAPValue.Bloomberg];
                                    vWAPBloombergWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.Bloomberg] = vWAPBloombergWiseList;
                                    _vWAPSymbologyWiseDict[5] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[5].Add(vWAPValue.Bloomberg, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameBloombergDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameBloombergDict.Add(vWAPValue.Bloomberg, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(5, vWAPSameBloombergDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(vWAPValue.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            vWAPValue.OSIOptionSymbol = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + vWAPValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + vWAPValue.OSIOptionSymbol), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.OSIOptionSymbol.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[6];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.OSIOptionSymbol))
                                {
                                    List<VWAPImport> vWAPOSIWiseList = vWAPSameSymbologyDict[vWAPValue.OSIOptionSymbol];
                                    vWAPOSIWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.OSIOptionSymbol] = vWAPOSIWiseList;
                                    _vWAPSymbologyWiseDict[6] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[6].Add(vWAPValue.OSIOptionSymbol, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameOSIDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameOSIDict.Add(vWAPValue.OSIOptionSymbol, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(6, vWAPSameOSIDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(vWAPValue.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            vWAPValue.IDCOOptionSymbol = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + vWAPValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + vWAPValue.IDCOOptionSymbol), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[7];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.IDCOOptionSymbol))
                                {
                                    List<VWAPImport> vWAPIDCOWiseList = vWAPSameSymbologyDict[vWAPValue.IDCOOptionSymbol];
                                    vWAPIDCOWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.IDCOOptionSymbol] = vWAPIDCOWiseList;
                                    _vWAPSymbologyWiseDict[7] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[7].Add(vWAPValue.IDCOOptionSymbol, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameIDCODict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameIDCODict.Add(vWAPValue.IDCOOptionSymbol, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(7, vWAPSameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(vWAPValue.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            vWAPValue.OpraOptionSymbol = vWAPValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + vWAPValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + vWAPValue.OpraOptionSymbol), vWAPValue);
                            }
                            uniqueID = vWAPValue.Date + vWAPValue.OpraOptionSymbol.Trim().ToUpper();
                            if (_vWAPSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[8];
                                if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.OpraOptionSymbol))
                                {
                                    List<VWAPImport> vWAPOpraWiseList = vWAPSameSymbologyDict[vWAPValue.OpraOptionSymbol];
                                    vWAPOpraWiseList.Add(vWAPValue);
                                    vWAPSameSymbologyDict[vWAPValue.OpraOptionSymbol] = vWAPOpraWiseList;
                                    _vWAPSymbologyWiseDict[8] = vWAPSameSymbologyDict;
                                }
                                else
                                {
                                    List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                    vWAPlist.Add(vWAPValue);
                                    _vWAPSymbologyWiseDict[8].Add(vWAPValue.OpraOptionSymbol, vWAPlist);
                                }
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                Dictionary<string, List<VWAPImport>> vWAPSameOpraDict = new Dictionary<string, List<VWAPImport>>();
                                vWAPSameOpraDict.Add(vWAPValue.OpraOptionSymbol, vWAPlist);
                                _vWAPSymbologyWiseDict.Add(7, vWAPSameOpraDict);
                            }
                            break;
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _vWAPValueCollection.Add(vWAPValue);
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
        /// <param name="vWAPValue">The vWAP value.</param>
        private static void SetSymbology(VWAPImport vWAPValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(vWAPValue.Symbol))
                    vWAPValue.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.RIC))
                    vWAPValue.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.ISIN))
                    vWAPValue.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.SEDOL))
                    vWAPValue.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.CUSIP))
                    vWAPValue.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.Bloomberg))
                    vWAPValue.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.OSIOptionSymbol))
                    vWAPValue.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.IDCOOptionSymbol))
                    vWAPValue.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(vWAPValue.OpraOptionSymbol))
                    vWAPValue.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
        /// Gets the sm data for vWAP import.
        /// </summary>
        private void GetSMDataForVWAPImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_vWAPSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<VWAPImport>>> kvp in _vWAPSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<VWAPImport>> symbolDict = _vWAPSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<VWAPImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    _countSymbols++;
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
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

                        if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
                        {
                            _timerSecurityValidation.Stop();
                            TimerRefresh_Tick(null, null);
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

        /// <summary>
        /// Creates the data table for vWAP import.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDataTableForVWAPImport()
        {
            DataTable dtVWAPNew = new DataTable();
            try
            {
                dtVWAPNew.TableName = "DailyVWAP";

                DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
                DataColumn colDate = new DataColumn("Date", typeof(string));
                DataColumn colVWAPPrice = new DataColumn("VWAP", typeof(double));
                DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

                dtVWAPNew.Columns.Add(colSymbol);
                dtVWAPNew.Columns.Add(colDate);
                dtVWAPNew.Columns.Add(colVWAPPrice);
                dtVWAPNew.Columns.Add(colAUECID);
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
            return dtVWAPNew;
        }
        /// <summary>
        /// This method is no more used
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{SecMasterBaseObj}"/> instance containing the event data.</param>
        private void SecurityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                //update positoion master collection
                UpdateCollection(secMasterObj, string.Empty);
                if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
                {
                    _timerSecurityValidation.Stop();
                    TimerRefresh_Tick(this, null);
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

        #region IImportHandler Members

        /// <summary>
        /// Validates the price tolerance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            return new DataTable();
        }

        #endregion

        #region IImportITaskHandler Members
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ak9w5846.aspx
        /// </summary>
        object objectLock = new Object();
        /// <summary>
        /// Occurs when [update task specific data points].
        /// </summary>
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
        /// <summary>
        /// wires event
        /// </summary>
        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(SecurityMaster_SecMstrDataResponse);
                //_timerRefresh.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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
        /// Unwires Events
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                _timerSecurityValidation.Elapsed -= new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
                //_timerRefresh.Elapsed -= new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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
        /// The _timer lock
        /// </summary>
        private static object _timerLock = new Object();

        /// <summary>
        /// If all the trades are validated then data will be imported in system
        /// Partial validated trades will not be saved in system
        /// Statistics xml will be written for validated and non validated trades
        /// Statistics xml will be written for validated symbols
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (_timerLock)
                {

                    if (_vWAPValueCollection != null)
                    {
                        DataTable dtVWAPValueCollection = GeneralUtilities.GetDataTableFromList(_vWAPValueCollection);

                        List<VWAPImport> lstNonValidatedVWAPValue = _vWAPValueCollection.FindAll(vWAPValue => vWAPValue.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                        DataTable dtNonValidatedVWAPValue = GeneralUtilities.GetDataTableFromList(lstNonValidatedVWAPValue);
                        dtNonValidatedVWAPValue.TableName = "NonValidatedVWAPValue";
                        //if (dtNonValidatedVWAPValue != null && dtNonValidatedVWAPValue.Rows.Count > 0)
                        //{
                        //    dtNonValidatedVWAPValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                        //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedVWAPValue", lstNonValidatedVWAPValue.Count, nonValidatedXmlFilePath);
                        //}

                        DataTable dtValidatedVWAPValue = GeneralUtilities.GetDataTableFromList(_validatedVWAPValueCollection);
                        dtValidatedVWAPValue.TableName = "ValidatedVWAPValue";
                        if (dtValidatedVWAPValue != null && dtValidatedVWAPValue.Rows.Count > 0)
                        {
                            dtValidatedVWAPValue.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath);
                            if (_currentResult != null)
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedVWAPValue", _validatedVWAPValueCollection.Count, _validatedSymbolsXmlFilePath);
                        }

                        #region update task specific data

                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        if (_currentResult != null)
                        {
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
                        }
                        if (dtValidatedSymbols != null)
                        {
                            AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols, _dictRequestedSymbol);
                        }
                        #endregion

                        #region symbol validation added to task statistics

                        if (dtVWAPValueCollection != null && dtVWAPValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing. For re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtVWAPValueCollection);
                            //}
                        }
                        #endregion

                        #region write xml for validated symbols

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtValidatedVWAPValue, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        if (_isSaveDataInApplication)
                        {
                            if (_validatedVWAPValueCollection.Count == 0 && _currentResult != null)
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
                                //Validated VWAP Value objects will be added to _validatedVWAPValueCollection
                                if (_vWAPValueCollection.Count == _validatedVWAPValueCollection.Count)
                                {
                                    SaveVWAPValues();
                                    if (TaskSpecificDataPointsPreUpdate != null && _currentResult != null)
                                    {
                                        //Return TaskResult which was received from ImportManager as event argument
                                        TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TaskSpecificDataPointsPreUpdate != null && _currentResult != null)
                            {
                                //Return TaskResult which was received from ImportManager as event argument
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
                //_timerRefresh.Stop();
                UnwireEvents();
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
                //_timerRefresh.Stop();
                //_timerRefresh.Start();
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
        /// Add securities to the
        /// Now we receive response of all the symbols from BB api, either they are validated or non validated.
        /// To handle worst case if we didn't receive response from api, we need this method
        /// </summary>
        /// <param name="dtValidatedSymbols">The dt validated symbols.</param>
        /// <param name="dictRequestedSymbol">The dictionary requested symbol.</param>
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols, Dictionary<string, VWAPImport> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, VWAPImport> item in dictRequestedSymbol)
                {
                    DataRow row = dtValidatedSymbols.NewRow();
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
        /// validate symbol weather there is any Mismatch or not
        /// </summary>
        /// <param name="vWAPValue">The vWAP value.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        private void validateAllSymbols(VWAPImport vWAPValue, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                vWAPValue.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(vWAPValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    vWAPValue.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (vWAPValue.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(vWAPValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    vWAPValue.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (vWAPValue.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    vWAPValue.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (vWAPValue.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    vWAPValue.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (vWAPValue.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    vWAPValue.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (vWAPValue.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    vWAPValue.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (vWAPValue.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    vWAPValue.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (vWAPValue.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(vWAPValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    vWAPValue.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(vWAPValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (vWAPValue.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    vWAPValue.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(vWAPValue.MismatchType))
                    {
                        vWAPValue.MismatchType += ", ";
                    }
                    vWAPValue.MismatchType += "Symbol Mismatch";
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
        /// <param name="dtVWAPValueCollection">The dt vWAP value collection.</param>
        private void UpdateImportDataXML(DataTable dtVWAPValueCollection)
        {
            dtVWAPValueCollection.TableName = "ImportData";
            try
            {
                if (dtVWAPValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtVWAPValueCollection.Columns["RowIndex"];
                    dtVWAPValueCollection.PrimaryKey = columns;
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1200
                        // TODO : For now there were conflict error while merging the table with the column "BrokenRulesCollection" so it is removed.
                        if (dtVWAPValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtVWAPValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtVWAPValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtVWAPValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _vWAPValueCollection.Count, _totalImportDataXmlFilePath);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _timerSecurityValidation.Dispose();
        }

        #endregion
    }
}