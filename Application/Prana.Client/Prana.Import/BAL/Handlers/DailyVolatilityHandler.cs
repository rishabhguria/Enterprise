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
    sealed public class DailyVolatilityHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables
        // list collection of import volatility values
        /// <summary>
        /// The _volatility value collection
        /// </summary>
        List<VolatilityImport> _volatilityValueCollection = new List<VolatilityImport>();
        /// <summary>
        /// The _current result
        /// </summary>
        TaskResult _currentResult = new TaskResult();
        /// <summary>
        /// The _validated volatility value collection
        /// </summary>
        List<VolatilityImport> _validatedVolatilityValueCollection = new List<VolatilityImport>();
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
        /// The _volatility symbology wise dictionary
        /// </summary>
        Dictionary<int, Dictionary<string, List<VolatilityImport>>> _volatilitySymbologyWiseDict = new Dictionary<int, Dictionary<string, List<VolatilityImport>>>();

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
        Dictionary<string, VolatilityImport> _dictRequestedSymbol = new Dictionary<string, VolatilityImport>();

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
        /// Initializes a new instance of the <see cref="DailyVolatilityHandler"/> class.
        /// </summary>
        public DailyVolatilityHandler() { }

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
                GetSMDataForVolatilityImport();


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
        /// Saves the volatility values.
        /// </summary>
        private void SaveVolatilityValues()
        {
            try
            {
                int rowsUpdated = 0;
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                //UpdateValidatedDailyVolatilityCollection();
                if (_validatedVolatilityValueCollection.Count > 0)
                {
                    // total number of records inserted

                    DataTable dtVolatilityValues = CreateDataTableForVolatilityImport();
                    DataTable dtVolatilityValueImportData = GeneralUtilities.GetDataTableFromList(_validatedVolatilityValueCollection);

                    DataTable dtVolatilityTableFromCollection = GeneralUtilities.CreateTableFromCollection<VolatilityImport>(dtVolatilityValues, _validatedVolatilityValueCollection);

                    try
                    {
                        if ((dtVolatilityTableFromCollection != null) && (dtVolatilityTableFromCollection.Rows.Count > 0))
                        {
                            rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveVolatility(dtVolatilityTableFromCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtVolatilityValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }

                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtVolatilityValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtVolatilityValueImportData);
                    }

                    int importedTradesCount = dtVolatilityValueImportData.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtVolatilityValueImportData.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (_currentResult != null)
                    {
                        if (_volatilityValueCollection.Count == _validatedVolatilityValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                        }
                        else if ((_volatilityValueCollection.Count != _validatedVolatilityValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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

                if (_volatilitySymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<VolatilityImport>> dictSymbols = _volatilitySymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<VolatilityImport> listVolatilityValues = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (VolatilityImport volatilityImport in listVolatilityValues)
                        {
                            validateAllSymbols(volatilityImport, secMasterObj);

                            volatilityImport.Symbol = pranaSymbol;
                            volatilityImport.CUSIP = cuspiSymbol;
                            volatilityImport.ISIN = isinSymbol;
                            volatilityImport.SEDOL = sedolSymbol;
                            volatilityImport.Bloomberg = bloombergSymbol;
                            volatilityImport.RIC = reutersSymbol;
                            volatilityImport.OSIOptionSymbol = osiOptionSymbol;
                            volatilityImport.IDCOOptionSymbol = idcoOptionSymbol;
                            volatilityImport.OpraOptionSymbol = opraOptionSymbol;
                            volatilityImport.AUECID = secMasterObj.AUECID;

                            if (volatilityImport.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                _validatedVolatilityValueCollection.Add(volatilityImport);
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
            return "ImportVolatilityValues.xsd";
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
                _volatilitySymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                //_volatilityValueCollection.Clear();
                _volatilityValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(VolatilityImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    VolatilityImport volatilityValue = new VolatilityImport();
                    volatilityValue.Symbol = string.Empty;
                    volatilityValue.Volatility = 0;
                    volatilityValue.Date = string.Empty;
                    volatilityValue.AUECID = 0;
                    volatilityValue.RowIndex = irow;
                    volatilityValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, volatilityValue, irow);

                    //TODO: Need to handle user selected date 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        volatilityValue.Date = dtn.ToString(DATEFORMAT);
                    }
                    else
                    {
                        if (!volatilityValue.Date.Equals(string.Empty))
                        {
                            bool isParsed = false;
                            double outResult;
                            isParsed = double.TryParse(volatilityValue.Date, out outResult);
                            if (isParsed)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(volatilityValue.Date));
                                volatilityValue.Date = dtn.ToString(DATEFORMAT);
                            }
                            else
                            {
                                try
                                {
                                    DateTime dtn = Convert.ToDateTime(volatilityValue.Date);
                                    volatilityValue.Date = dtn.ToString(DATEFORMAT);
                                }
                                // To check invalid date format from xslt
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                    volatilityValue.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                    if (!Regex.IsMatch(volatilityValue.ValidationError, "Invalid Date Format", RegexOptions.IgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(volatilityValue.ValidationError))
                                            volatilityValue.ValidationError += Seperators.SEPERATOR_8;
                                        volatilityValue.ValidationError += " Invalid Date Format ";
                                    }
                                }
                            }
                        }
                    }
                    string uniqueID = string.Empty;

                    //if (string.IsNullOrEmpty(volatilityValue.Symbology))
                    //{
                    //    if (!string.IsNullOrEmpty(volatilityValue.Symbol))
                    //    {
                    //        volatilityValue.Symbology = "Symbol";
                    //    }
                    //}

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(volatilityValue.Symbology))
                    {
                        SetSymbology(volatilityValue);
                    }

                    switch (volatilityValue.Symbology.Trim().ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(volatilityValue.Symbol))
                        case "SYMBOL":
                            volatilityValue.Symbol = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + volatilityValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + volatilityValue.Symbol), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.Symbol.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[0];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.Symbol))
                                {
                                    List<VolatilityImport> volatilitySymbolWiseList = volatilitySameSymbologyDict[volatilityValue.Symbol];
                                    volatilitySymbolWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.Symbol] = volatilitySymbolWiseList;
                                    _volatilitySymbologyWiseDict[0] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[0].Add(volatilityValue.Symbol, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbolDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameSymbolDict.Add(volatilityValue.Symbol, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(0, volatilitySameSymbolDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(volatilityValue.RIC))
                        case "RIC":
                            volatilityValue.RIC = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + volatilityValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + volatilityValue.RIC), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.RIC.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[1];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.RIC))
                                {
                                    List<VolatilityImport> volatilityRICWiseList = volatilitySameSymbologyDict[volatilityValue.RIC];
                                    volatilityRICWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.RIC] = volatilityRICWiseList;
                                    _volatilitySymbologyWiseDict[1] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilityList = new List<VolatilityImport>();
                                    volatilityList.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[1].Add(volatilityValue.RIC, volatilityList);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilityList = new List<VolatilityImport>();
                                volatilityList.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> VolatilitySameRICDict = new Dictionary<string, List<VolatilityImport>>();
                                VolatilitySameRICDict.Add(volatilityValue.RIC, volatilityList);
                                _volatilitySymbologyWiseDict.Add(1, VolatilitySameRICDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(volatilityValue.ISIN))
                        case "ISIN":
                            volatilityValue.ISIN = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + volatilityValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + volatilityValue.ISIN), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.ISIN.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[2];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.ISIN))
                                {
                                    List<VolatilityImport> volatilityISINWiseList = volatilitySameSymbologyDict[volatilityValue.ISIN];
                                    volatilityISINWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.ISIN] = volatilityISINWiseList;
                                    _volatilitySymbologyWiseDict[2] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[2].Add(volatilityValue.ISIN, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameISINDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameISINDict.Add(volatilityValue.ISIN, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(2, volatilitySameISINDict);
                            }
                            break;

                        case "SEDOL":
                            volatilityValue.SEDOL = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + volatilityValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + volatilityValue.SEDOL), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.SEDOL.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[3];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.SEDOL))
                                {
                                    List<VolatilityImport> volatilitySEDOLWiseList = volatilitySameSymbologyDict[volatilityValue.SEDOL];
                                    volatilitySEDOLWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.SEDOL] = volatilitySEDOLWiseList;
                                    _volatilitySymbologyWiseDict[3] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[3].Add(volatilityValue.SEDOL, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySEDOLDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySEDOLDict.Add(volatilityValue.SEDOL, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(3, volatilitySEDOLDict);
                            }
                            break;

                        case "CUSIP":
                            volatilityValue.CUSIP = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + volatilityValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + volatilityValue.CUSIP), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.CUSIP.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[4];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.CUSIP))
                                {
                                    List<VolatilityImport> volatilityCUSIPWiseList = volatilitySameSymbologyDict[volatilityValue.CUSIP];
                                    volatilityCUSIPWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.CUSIP] = volatilityCUSIPWiseList;
                                    _volatilitySymbologyWiseDict[4] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[4].Add(volatilityValue.CUSIP, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameCUSIPDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameCUSIPDict.Add(volatilityValue.CUSIP, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(4, volatilitySameCUSIPDict);
                            }
                            break;
                        case "BLOOMBERG":
                            volatilityValue.Bloomberg = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + volatilityValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + volatilityValue.Bloomberg), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.Bloomberg.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[5];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.Bloomberg))
                                {
                                    List<VolatilityImport> volatilityBloombergWiseList = volatilitySameSymbologyDict[volatilityValue.Bloomberg];
                                    volatilityBloombergWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.Bloomberg] = volatilityBloombergWiseList;
                                    _volatilitySymbologyWiseDict[5] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[5].Add(volatilityValue.Bloomberg, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameBloombergDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameBloombergDict.Add(volatilityValue.Bloomberg, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(5, volatilitySameBloombergDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(volatilityValue.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            volatilityValue.OSIOptionSymbol = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + volatilityValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + volatilityValue.OSIOptionSymbol), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.OSIOptionSymbol.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[6];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.OSIOptionSymbol))
                                {
                                    List<VolatilityImport> volatilityOSIWiseList = volatilitySameSymbologyDict[volatilityValue.OSIOptionSymbol];
                                    volatilityOSIWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.OSIOptionSymbol] = volatilityOSIWiseList;
                                    _volatilitySymbologyWiseDict[6] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[6].Add(volatilityValue.OSIOptionSymbol, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameOSIDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameOSIDict.Add(volatilityValue.OSIOptionSymbol, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(6, volatilitySameOSIDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(volatilityValue.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            volatilityValue.IDCOOptionSymbol = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + volatilityValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + volatilityValue.IDCOOptionSymbol), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[7];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.IDCOOptionSymbol))
                                {
                                    List<VolatilityImport> volatilityIDCOWiseList = volatilitySameSymbologyDict[volatilityValue.IDCOOptionSymbol];
                                    volatilityIDCOWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.IDCOOptionSymbol] = volatilityIDCOWiseList;
                                    _volatilitySymbologyWiseDict[7] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[7].Add(volatilityValue.IDCOOptionSymbol, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameIDCODict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameIDCODict.Add(volatilityValue.IDCOOptionSymbol, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(7, volatilitySameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(volatilityValue.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            volatilityValue.OpraOptionSymbol = volatilityValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + volatilityValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + volatilityValue.OpraOptionSymbol), volatilityValue);
                            }
                            uniqueID = volatilityValue.Date + volatilityValue.OpraOptionSymbol.Trim().ToUpper();
                            if (_volatilitySymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[8];
                                if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.OpraOptionSymbol))
                                {
                                    List<VolatilityImport> volatilityOpraWiseList = volatilitySameSymbologyDict[volatilityValue.OpraOptionSymbol];
                                    volatilityOpraWiseList.Add(volatilityValue);
                                    volatilitySameSymbologyDict[volatilityValue.OpraOptionSymbol] = volatilityOpraWiseList;
                                    _volatilitySymbologyWiseDict[8] = volatilitySameSymbologyDict;
                                }
                                else
                                {
                                    List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                    volatilitylist.Add(volatilityValue);
                                    _volatilitySymbologyWiseDict[8].Add(volatilityValue.OpraOptionSymbol, volatilitylist);
                                }
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                Dictionary<string, List<VolatilityImport>> volatilitySameOpraDict = new Dictionary<string, List<VolatilityImport>>();
                                volatilitySameOpraDict.Add(volatilityValue.OpraOptionSymbol, volatilitylist);
                                _volatilitySymbologyWiseDict.Add(7, volatilitySameOpraDict);
                            }
                            break;
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _volatilityValueCollection.Add(volatilityValue);
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
        /// <param name="volatilityValue">The volatility value.</param>
        private static void SetSymbology(VolatilityImport volatilityValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(volatilityValue.Symbol))
                    volatilityValue.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.RIC))
                    volatilityValue.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.ISIN))
                    volatilityValue.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.SEDOL))
                    volatilityValue.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.CUSIP))
                    volatilityValue.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.Bloomberg))
                    volatilityValue.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.OSIOptionSymbol))
                    volatilityValue.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.IDCOOptionSymbol))
                    volatilityValue.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(volatilityValue.OpraOptionSymbol))
                    volatilityValue.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
        /// Gets the sm data for volatility import.
        /// </summary>
        private void GetSMDataForVolatilityImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_volatilitySymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<VolatilityImport>>> kvp in _volatilitySymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<VolatilityImport>> symbolDict = _volatilitySymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<VolatilityImport>> keyvaluepair in symbolDict)
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
        /// Creates the data table for volatility import.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDataTableForVolatilityImport()
        {
            DataTable dtVolatilityNew = new DataTable();
            try
            {
                dtVolatilityNew.TableName = "DailyVolatility";

                DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
                DataColumn colDate = new DataColumn("Date", typeof(string));
                DataColumn colVolatilityPrice = new DataColumn("Volatility", typeof(double));
                DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

                dtVolatilityNew.Columns.Add(colSymbol);
                dtVolatilityNew.Columns.Add(colDate);
                dtVolatilityNew.Columns.Add(colVolatilityPrice);
                dtVolatilityNew.Columns.Add(colAUECID);
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
            return dtVolatilityNew;
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

                    if (_volatilityValueCollection != null)
                    {
                        DataTable dtVolatilityValueCollection = GeneralUtilities.GetDataTableFromList(_volatilityValueCollection);

                        List<VolatilityImport> lstNonValidatedVolatilityValue = _volatilityValueCollection.FindAll(volatilityValue => volatilityValue.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                        DataTable dtNonValidatedVolatilityValue = GeneralUtilities.GetDataTableFromList(lstNonValidatedVolatilityValue);
                        dtNonValidatedVolatilityValue.TableName = "NonValidatedVolatilityValue";
                        //if (dtNonValidatedVolatilityValue != null && dtNonValidatedVolatilityValue.Rows.Count > 0)
                        //{
                        //    dtNonValidatedVolatilityValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                        //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedVolatilityValue", lstNonValidatedVolatilityValue.Count, nonValidatedXmlFilePath);
                        //}

                        DataTable dtValidatedVolatilityValue = GeneralUtilities.GetDataTableFromList(_validatedVolatilityValueCollection);
                        dtValidatedVolatilityValue.TableName = "ValidatedVolatilityValue";
                        if (dtValidatedVolatilityValue != null && dtValidatedVolatilityValue.Rows.Count > 0)
                        {
                            dtValidatedVolatilityValue.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath);
                            if (_currentResult != null)
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedVolatilityValue", _validatedVolatilityValueCollection.Count, _validatedSymbolsXmlFilePath);
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

                        if (dtVolatilityValueCollection != null && dtVolatilityValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing. For re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtVolatilityValueCollection);
                            //}
                        }
                        #endregion

                        #region write xml for validated symbols

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtValidatedVolatilityValue, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        if (_isSaveDataInApplication)
                        {
                            if (_validatedVolatilityValueCollection.Count == 0 && _currentResult != null)
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
                                //Validated VOlatility Value objects will be added to _validatedVolatilityValueCollection
                                if (_volatilityValueCollection.Count == _validatedVolatilityValueCollection.Count)
                                {
                                    SaveVolatilityValues();
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
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols, Dictionary<string, VolatilityImport> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, VolatilityImport> item in dictRequestedSymbol)
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
        /// <param name="volatilityValue">The volatility value.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        private void validateAllSymbols(VolatilityImport volatilityValue, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                volatilityValue.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(volatilityValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    volatilityValue.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (volatilityValue.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(volatilityValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    volatilityValue.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (volatilityValue.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    volatilityValue.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (volatilityValue.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    volatilityValue.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (volatilityValue.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    volatilityValue.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (volatilityValue.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    volatilityValue.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (volatilityValue.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    volatilityValue.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (volatilityValue.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(volatilityValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    volatilityValue.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(volatilityValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (volatilityValue.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    volatilityValue.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(volatilityValue.MismatchType))
                    {
                        volatilityValue.MismatchType += ", ";
                    }
                    volatilityValue.MismatchType += "Symbol Mismatch";
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
        /// <param name="dtVolatilityValueCollection">The dt volatility value collection.</param>
        private void UpdateImportDataXML(DataTable dtVolatilityValueCollection)
        {
            dtVolatilityValueCollection.TableName = "ImportData";
            try
            {
                if (dtVolatilityValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtVolatilityValueCollection.Columns["RowIndex"];
                    dtVolatilityValueCollection.PrimaryKey = columns;
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
                        if (dtVolatilityValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtVolatilityValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtVolatilityValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtVolatilityValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _volatilityValueCollection.Count, _totalImportDataXmlFilePath);
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