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
    public sealed class DailyDividendYieldHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables

        // list collection of import dividendyield values
        /// <summary>
        /// The _dividend yield value collection
        /// </summary>
        private List<DividendYieldImport> _dividendYieldValueCollection = new List<DividendYieldImport>();
        /// <summary>
        /// The _current result
        /// </summary>
        private TaskResult _currentResult = new TaskResult();
        /// <summary>
        /// The _validate dividend yield value collection
        /// </summary>
        private List<DividendYieldImport> _validateDividendYieldValueCollection = new List<DividendYieldImport>();
        //private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);
        /// <summary>
        /// The _timer security validation
        /// </summary>
        private System.Timers.Timer _timerSecurityValidation;
        /// <summary>
        /// The _is save data in application
        /// </summary>
        private bool _isSaveDataInApplication = false;
        /// <summary>
        /// The _count symbols
        /// </summary>
        private int _countSymbols = 0;
        /// <summary>
        /// The _count validated symbols
        /// </summary>
        private int _countValidatedSymbols = 0;
        /// <summary>
        /// The _count non validated symbols
        /// </summary>
        private int _countNonValidatedSymbols = 0;

        /// <summary>
        /// The _dividend yield symbology wise dictionary
        /// </summary>
        private Dictionary<int, Dictionary<string, List<DividendYieldImport>>> _dividendYieldSymbologyWiseDict =
            new Dictionary<int, Dictionary<string, List<DividendYieldImport>>>();

        /// <summary>
        /// The dateformat
        /// </summary>
        private const string DATEFORMAT = "MM/dd/yyyy";
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
        private Dictionary<string, DividendYieldImport> _dictRequestedSymbol =
            new Dictionary<string, DividendYieldImport>();

        /// <summary>
        /// The _sec master response collection
        /// </summary>
        private List<SecMasterBaseObj> _secMasterResponseCollection = new List<SecMasterBaseObj>();

        /// <summary>
        /// Gets or sets the sec master response collection.
        /// </summary>
        /// <value>
        /// The sec master response collection.
        /// </value>
        public List<SecMasterBaseObj> SecMasterResponseCollection
        {
            get { return _secMasterResponseCollection; }
            set { _secMasterResponseCollection = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyDividendYieldHandler"/> class.
        /// </summary>
        public DailyDividendYieldHandler()
        {
        }

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

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath,
                    ref _totalImportDataXmlFilePath);

                SetCollection(ds, runUpload);
                GetSMDataForDividendYieldImport();
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
        /// Saves the dividend yield values.
        /// </summary>
        private void SaveDividendYieldValues()
        {
            try
            {
                int rowsUpdated = 0;
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                if (_validateDividendYieldValueCollection.Count > 0)
                {
                    // total number of records inserted

                    DataTable dtDividendYieldValues = CreateDataTableForDividendYieldImport();
                    DataTable dtDividendYieldValueImportData =
                        GeneralUtilities.GetDataTableFromList(_validateDividendYieldValueCollection);

                    DataTable dtDividendTableFromCollection =
                        GeneralUtilities.CreateTableFromCollection<DividendYieldImport>(dtDividendYieldValues,
                            _validateDividendYieldValueCollection);

                    try
                    {
                        if ((dtDividendTableFromCollection != null) && (dtDividendTableFromCollection.Rows.Count > 0))
                        {
                            rowsUpdated =
                                ServiceManager.Instance.PricingServices.InnerChannel.SaveDividendYield(
                                    dtDividendTableFromCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtDividendYieldValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }

                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtDividendYieldValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtDividendYieldValueImportData);
                    }

                    int importedTradesCount = dtDividendYieldValueImportData.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtDividendYieldValueImportData.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (_currentResult != null)
                    {
                        if (_dividendYieldValueCollection.Count == _validateDividendYieldValueCollection.Count &&
                            !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus",
                                "Success", null);
                        }
                        else if ((_dividendYieldValueCollection.Count != _validateDividendYieldValueCollection.Count ||
                                  isPartialSuccess) && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus",
                                "Partial Success", null);
                        }
                        else
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus",
                                "Failure", null);
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

                string isinSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                string cuspiSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                string sedolSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                string reutersSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                string bloombergSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                string osiOptionSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                string idcoOptionSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                string opraOptionSymbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

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

                if (_dividendYieldSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<DividendYieldImport>> dictSymbols =
                        _dividendYieldSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response

                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 +
                                     secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }

                        #endregion

                        List<DividendYieldImport> listDividendYieldValues = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (DividendYieldImport dividendYieldImport in listDividendYieldValues)
                        {
                            validateAllSymbols(dividendYieldImport, secMasterObj);

                            dividendYieldImport.Symbol = pranaSymbol;
                            dividendYieldImport.CUSIP = cuspiSymbol;
                            dividendYieldImport.ISIN = isinSymbol;
                            dividendYieldImport.SEDOL = sedolSymbol;
                            dividendYieldImport.Bloomberg = bloombergSymbol;
                            dividendYieldImport.RIC = reutersSymbol;
                            dividendYieldImport.OSIOptionSymbol = osiOptionSymbol;
                            dividendYieldImport.IDCOOptionSymbol = idcoOptionSymbol;
                            dividendYieldImport.OpraOptionSymbol = opraOptionSymbol;
                            dividendYieldImport.AUECID = secMasterObj.AUECID;

                            if (
                                dividendYieldImport.ValidationStatus.Equals(
                                    ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                _validateDividendYieldValueCollection.Add(dividendYieldImport);
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
            return "ImportDividendYieldValues.xsd";
        }

        #endregion

        /// <summary>
        /// Sets the collection.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="runUpload">The run upload.</param>
        private void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _dividendYieldSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                //_dividendYieldValueCollection.Clear();
                _dividendYieldValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly =
                    System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(DividendYieldImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    DividendYieldImport dividendYieldValue = new DividendYieldImport();
                    dividendYieldValue.Symbol = string.Empty;
                    dividendYieldValue.DividendYield = 0;
                    dividendYieldValue.Date = string.Empty;
                    dividendYieldValue.AUECID = 0;
                    dividendYieldValue.RowIndex = irow;
                    dividendYieldValue.ImportStatus =
                        Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, dividendYieldValue, irow);

                    //TODO: Need to handle user selected date 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) &&
                        !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        dividendYieldValue.Date = dtn.ToString(DATEFORMAT);
                    }
                    else
                    {
                        if (!dividendYieldValue.Date.Equals(string.Empty))
                        {
                            bool isParsed = false;
                            double outResult;
                            isParsed = double.TryParse(dividendYieldValue.Date, out outResult);
                            if (isParsed)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendYieldValue.Date));
                                dividendYieldValue.Date = dtn.ToString(DATEFORMAT);
                            }
                            else
                            {
                                try
                                {
                                    DateTime dtn = Convert.ToDateTime(dividendYieldValue.Date);
                                    dividendYieldValue.Date = dtn.ToString(DATEFORMAT);
                                }
                                // To check invalid date format from xslt
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                    dividendYieldValue.ValidationStatus =
                                        ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                    if (
                                        !Regex.IsMatch(dividendYieldValue.ValidationError, "Invalid Date Format",
                                            RegexOptions.IgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(dividendYieldValue.ValidationError))
                                            dividendYieldValue.ValidationError += Seperators.SEPERATOR_8;
                                        dividendYieldValue.ValidationError += " Invalid Date Format ";
                                    }
                                }
                            }
                        }
                    }
                    string uniqueID = string.Empty;

                    //if (string.IsNullOrEmpty(dividendYieldValue.Symbology))
                    //{
                    //    if (!string.IsNullOrEmpty(dividendYieldValue.Symbol))
                    //    {
                    //        dividendYieldValue.Symbology = "Symbol";
                    //    }
                    //}

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(dividendYieldValue.Symbology))
                    {
                        SetSymbology(dividendYieldValue);
                    }

                    switch (dividendYieldValue.Symbology.Trim().ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(dividendYieldValue.Symbol))
                        case "SYMBOL":
                            dividendYieldValue.Symbol = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + dividendYieldValue.Symbol),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.Symbol.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[0];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.Symbol))
                                {
                                    List<DividendYieldImport> dividendYieldSymbolWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.Symbol];
                                    dividendYieldSymbolWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.Symbol] =
                                        dividendYieldSymbolWiseList;
                                    _dividendYieldSymbologyWiseDict[0] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[0].Add(dividendYieldValue.Symbol, dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYileldSameSymbolDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYileldSameSymbolDict.Add(dividendYieldValue.Symbol, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(0, dividendYileldSameSymbolDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.RIC))
                        case "RIC":
                            dividendYieldValue.RIC = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + dividendYieldValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + dividendYieldValue.RIC),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.RIC.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[1];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.RIC))
                                {
                                    List<DividendYieldImport> dividendYieldRICWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.RIC];
                                    dividendYieldRICWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.RIC] = dividendYieldRICWiseList;
                                    _dividendYieldSymbologyWiseDict[1] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendYieldList = new List<DividendYieldImport>();
                                    dividendYieldList.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[1].Add(dividendYieldValue.RIC, dividendYieldList);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendYieldList = new List<DividendYieldImport>();
                                dividendYieldList.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameRICDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameRICDict.Add(dividendYieldValue.RIC, dividendYieldList);
                                _dividendYieldSymbologyWiseDict.Add(1, dividendYieldSameRICDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.ISIN))
                        case "ISIN":
                            dividendYieldValue.ISIN = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + dividendYieldValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + dividendYieldValue.ISIN),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.ISIN.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[2];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.ISIN))
                                {
                                    List<DividendYieldImport> dividendYieldISINWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.ISIN];
                                    dividendYieldISINWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.ISIN] = dividendYieldISINWiseList;
                                    _dividendYieldSymbologyWiseDict[2] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[2].Add(dividendYieldValue.ISIN, dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameISINDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameISINDict.Add(dividendYieldValue.ISIN, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(2, dividendYieldSameISINDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.SEDOL))
                        case "SEDOL":
                            dividendYieldValue.SEDOL = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + dividendYieldValue.SEDOL),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.SEDOL.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[3];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.SEDOL))
                                {
                                    List<DividendYieldImport> dividendYieldSEDOLWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.SEDOL];
                                    dividendYieldSEDOLWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.SEDOL] =
                                        dividendYieldSEDOLWiseList;
                                    _dividendYieldSymbologyWiseDict[3] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[3].Add(dividendYieldValue.SEDOL, dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSEDOLDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSEDOLDict.Add(dividendYieldValue.SEDOL, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(3, dividendYieldSEDOLDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.CUSIP))
                        case "CUSIP":
                            dividendYieldValue.CUSIP = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + dividendYieldValue.CUSIP),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.CUSIP.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[4];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.CUSIP))
                                {
                                    List<DividendYieldImport> dividendYieldCUSIPWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.CUSIP];
                                    dividendYieldCUSIPWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.CUSIP] =
                                        dividendYieldCUSIPWiseList;
                                    _dividendYieldSymbologyWiseDict[4] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[4].Add(dividendYieldValue.CUSIP, dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameCUSIPDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameCUSIPDict.Add(dividendYieldValue.CUSIP, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(4, dividendYieldSameCUSIPDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.Bloomberg))
                        case "BLOOMBERG":
                            dividendYieldValue.Bloomberg = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + dividendYieldValue.Bloomberg),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.Bloomberg.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[5];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.Bloomberg))
                                {
                                    List<DividendYieldImport> dividendYieldBloombergWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.Bloomberg];
                                    dividendYieldBloombergWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.Bloomberg] =
                                        dividendYieldBloombergWiseList;
                                    _dividendYieldSymbologyWiseDict[5] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[5].Add(dividendYieldValue.Bloomberg,
                                        dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameBloombergDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameBloombergDict.Add(dividendYieldValue.Bloomberg, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(5, dividendYieldSameBloombergDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            dividendYieldValue.OSIOptionSymbol = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(
                                    ("6" + Seperators.SEPERATOR_6 + dividendYieldValue.OSIOptionSymbol),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.OSIOptionSymbol.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[6];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.OSIOptionSymbol))
                                {
                                    List<DividendYieldImport> dividendYieldOSIWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.OSIOptionSymbol];
                                    dividendYieldOSIWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.OSIOptionSymbol] =
                                        dividendYieldOSIWiseList;
                                    _dividendYieldSymbologyWiseDict[6] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[6].Add(dividendYieldValue.OSIOptionSymbol,
                                        dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameOSIDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameOSIDict.Add(dividendYieldValue.OSIOptionSymbol, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(6, dividendYieldSameOSIDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(dividendYieldValue.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            dividendYieldValue.IDCOOptionSymbol = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(
                                    ("7" + Seperators.SEPERATOR_6 + dividendYieldValue.IDCOOptionSymbol),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[7];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.IDCOOptionSymbol))
                                {
                                    List<DividendYieldImport> dividendYieldIDCOWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.IDCOOptionSymbol];
                                    dividendYieldIDCOWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.IDCOOptionSymbol] =
                                        dividendYieldIDCOWiseList;
                                    _dividendYieldSymbologyWiseDict[7] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[7].Add(dividendYieldValue.IDCOOptionSymbol,
                                        dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameIDCODict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameIDCODict.Add(dividendYieldValue.IDCOOptionSymbol, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(7, dividendYieldSameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(dividendYieldValue.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            dividendYieldValue.OpraOptionSymbol = dividendYieldValue.Symbol.Trim().ToUpper();
                            if (
                                !_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 +
                                                                  dividendYieldValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(
                                    ("8" + Seperators.SEPERATOR_6 + dividendYieldValue.OpraOptionSymbol),
                                    dividendYieldValue);
                            }
                            uniqueID = dividendYieldValue.Date + dividendYieldValue.OpraOptionSymbol.Trim().ToUpper();
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict =
                                    _dividendYieldSymbologyWiseDict[8];
                                if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.OpraOptionSymbol))
                                {
                                    List<DividendYieldImport> dividendYieldOpraWiseList =
                                        dividendYieldSameSymbologyDict[dividendYieldValue.OpraOptionSymbol];
                                    dividendYieldOpraWiseList.Add(dividendYieldValue);
                                    dividendYieldSameSymbologyDict[dividendYieldValue.OpraOptionSymbol] =
                                        dividendYieldOpraWiseList;
                                    _dividendYieldSymbologyWiseDict[8] = dividendYieldSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                    dividendyieldlist.Add(dividendYieldValue);
                                    _dividendYieldSymbologyWiseDict[8].Add(dividendYieldValue.OpraOptionSymbol,
                                        dividendyieldlist);
                                }
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                Dictionary<string, List<DividendYieldImport>> dividendYieldSameOpraDict =
                                    new Dictionary<string, List<DividendYieldImport>>();
                                dividendYieldSameOpraDict.Add(dividendYieldValue.OpraOptionSymbol, dividendyieldlist);
                                _dividendYieldSymbologyWiseDict.Add(7, dividendYieldSameOpraDict);
                            }
                            break;
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _dividendYieldValueCollection.Add(dividendYieldValue);
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
        /// <param name="dividendYieldValue">The dividend yield value.</param>
        private static void SetSymbology(DividendYieldImport dividendYieldValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(dividendYieldValue.Symbol))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.RIC))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.ISIN))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.SEDOL))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.CUSIP))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.Bloomberg))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.OSIOptionSymbol))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.IDCOOptionSymbol))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(dividendYieldValue.OpraOptionSymbol))
                    dividendYieldValue.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
        /// Gets the sm data for dividend yield import.
        /// </summary>
        private void GetSMDataForDividendYieldImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_dividendYieldSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (
                        KeyValuePair<int, Dictionary<string, List<DividendYieldImport>>> kvp in
                            _dividendYieldSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<DividendYieldImport>> symbolDict =
                            _dividendYieldSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<DividendYieldImport>> keyvaluepair in symbolDict)
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
        /// Creates the data table for dividend yield import.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDataTableForDividendYieldImport()
        {
            DataTable dtDividendYieldNew = new DataTable();
            try
            {
                dtDividendYieldNew.TableName = "DailyDividendYield";

                DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
                DataColumn colDate = new DataColumn("Date", typeof(string));
                DataColumn colDividendYield = new DataColumn("DividendYield", typeof(double));
                DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

                dtDividendYieldNew.Columns.Add(colSymbol);
                dtDividendYieldNew.Columns.Add(colDate);
                dtDividendYieldNew.Columns.Add(colDividendYield);
                dtDividendYieldNew.Columns.Add(colAUECID);
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
            return dtDividendYieldNew;
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
        private object objectLock = new Object();

        /// <summary>
        /// Occurs when [update task specific data points].
        /// </summary>
        event EventHandler IImportITaskHandler.UpdateTaskSpecificDataPoints
        {
            add
            {
                lock (objectLock)
                {
                    if (TaskSpecificDataPointsPreUpdate == null ||
                        !TaskSpecificDataPointsPreUpdate.GetInvocationList().Contains(value))
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
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse +=
                    new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
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
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse -=
                    new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
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
                    if (_dividendYieldValueCollection != null)
                    {
                        DataTable dtDividendYieldValueCollection =
                            GeneralUtilities.GetDataTableFromList(_dividendYieldValueCollection);

                        List<DividendYieldImport> lstNonValidatedDividendYieldValue =
                            _dividendYieldValueCollection.FindAll(
                                dividendYieldValue =>
                                    dividendYieldValue.ValidationStatus !=
                                    ApplicationConstants.ValidationStatus.Validated.ToString());
                        DataTable dtNonValidatedDividendYieldValue =
                            GeneralUtilities.GetDataTableFromList(lstNonValidatedDividendYieldValue);
                        dtNonValidatedDividendYieldValue.TableName = "NonValidatedDividendYieldValue";
                        //if (dtNonValidatedDividendYieldValue != null && dtNonValidatedDividendYieldValue.Rows.Count > 0)
                        //{
                        //    dtNonValidatedDividendYieldValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                        //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedDividendYieldValue", lstNonValidatedDividendYieldValue.Count, nonValidatedXmlFilePath);
                        //}

                        DataTable dtValidatedDividendYieldValue =
                            GeneralUtilities.GetDataTableFromList(_validateDividendYieldValueCollection);
                        dtValidatedDividendYieldValue.TableName = "ValidatedDividendYieldValue";
                        if (dtValidatedDividendYieldValue != null && dtValidatedDividendYieldValue.Rows.Count > 0)
                        {
                            dtValidatedDividendYieldValue.WriteXml(Application.StartupPath +
                                                                   _validatedSymbolsXmlFilePath);
                            if (_currentResult != null)
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(
                                    "ValidatedDividendYieldValue", _validateDividendYieldValueCollection.Count,
                                    _validatedSymbolsXmlFilePath);
                        }

                        #region update task specific data

                        DataTable dtValidatedSymbols =
                            SecMasterHelper.getInstance()
                                .ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        if (_currentResult != null)
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols",
                                _countSymbols, null);
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols",
                                _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols",
                                _countNonValidatedSymbols, null);
                            if (_countSymbols == _countValidatedSymbols)
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation",
                                    Constants.SymbolValidationStatus.Success, null);
                            }
                            else if (_countValidatedSymbols == 0)
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation",
                                    Constants.SymbolValidationStatus.Failure, null);
                            }
                            else
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation",
                                    Constants.SymbolValidationStatus.PartialSuccess, null);
                            }
                        }
                        if (dtValidatedSymbols != null)
                        {
                            AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols, _dictRequestedSymbol);
                        }

                        #endregion

                        #region symbol validation added to task statistics

                        if (dtDividendYieldValueCollection != null && dtDividendYieldValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing. For re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtDividendYieldValueCollection);
                            //}
                        }

                        #endregion

                        #region write xml for validated symbols

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtValidatedDividendYieldValue, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }

                        #endregion

                        if (_isSaveDataInApplication)
                        {
                            if (_validateDividendYieldValueCollection.Count == 0 && _currentResult != null)
                            {
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus",
                                    ImportStatus.Failure, null);
                                if (TaskSpecificDataPointsPreUpdate != null)
                                {
                                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                    TaskSpecificDataPointsPreUpdate = null;
                                }
                                UnwireEvents();
                            }
                            else
                            {
                                //Validated DividendYieldValue objects will be added to _validateDividendYieldValueCollection
                                if (_dividendYieldValueCollection.Count == _validateDividendYieldValueCollection.Count)
                                {
                                    SaveDividendYieldValues();
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
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols,
            Dictionary<string, DividendYieldImport> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, DividendYieldImport> item in dictRequestedSymbol)
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
        /// <param name="dividendYieldValue">The dividend yield value.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        private void validateAllSymbols(DividendYieldImport dividendYieldValue, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                dividendYieldValue.Symbol =
                    secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(dividendYieldValue.CUSIP) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    dividendYieldValue.CUSIP =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.CUSIP) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.CUSIP !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~")
                            .Append(
                                secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol]
                                    .ToString());
                    }
                }
                if (string.IsNullOrEmpty(dividendYieldValue.ISIN) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    dividendYieldValue.ISIN =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.ISIN) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString
                                 ()))
                {
                    if (dividendYieldValue.ISIN !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString
                            ())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~")
                            .Append(
                                secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol]
                                    .ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.SEDOL) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    dividendYieldValue.SEDOL =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.SEDOL) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.SEDOL !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~")
                            .Append(
                                secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol]
                                    .ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.Bloomberg) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]
                            .ToString()))
                {
                    dividendYieldValue.Bloomberg =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]
                            .ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.Bloomberg) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.Bloomberg !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~")
                            .Append(
                                secMasterObj.SymbologyMapping[
                                    (int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.RIC) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    dividendYieldValue.RIC =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.RIC) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.RIC !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~")
                            .Append(
                                secMasterObj.SymbologyMapping[
                                    (int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.OSIOptionSymbol) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol]
                            .ToString()))
                {
                    dividendYieldValue.OSIOptionSymbol =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol]
                            .ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.OSIOptionSymbol) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.OSIOptionSymbol !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~")
                            .Append(
                                secMasterObj.SymbologyMapping[
                                    (int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.IDCOOptionSymbol) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]
                            .ToString()))
                {
                    dividendYieldValue.IDCOOptionSymbol =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]
                            .ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.IDCOOptionSymbol) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.IDCOOptionSymbol !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~")
                            .Append(
                                secMasterObj.SymbologyMapping[
                                    (int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(dividendYieldValue.OpraOptionSymbol) &&
                    !string.IsNullOrEmpty(
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol]
                            .ToString()))
                {
                    dividendYieldValue.OpraOptionSymbol =
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol]
                            .ToString();
                }
                else if (!string.IsNullOrEmpty(dividendYieldValue.OpraOptionSymbol) &&
                         !string.IsNullOrEmpty(
                             secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol]
                                 .ToString()))
                {
                    if (dividendYieldValue.OpraOptionSymbol !=
                        secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol]
                            .ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~")
                            .Append(
                                secMasterObj.SymbologyMapping[
                                    (int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    dividendYieldValue.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(dividendYieldValue.MismatchType))
                    {
                        dividendYieldValue.MismatchType += ", ";
                    }
                    dividendYieldValue.MismatchType += "Symbol Mismatch";
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
        /// <param name="dtDividendYieldValueCollection">The dt dividend yield value collection.</param>
        private void UpdateImportDataXML(DataTable dtDividendYieldValueCollection)
        {
            dtDividendYieldValueCollection.TableName = "ImportData";
            try
            {
                if (dtDividendYieldValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtDividendYieldValueCollection.Columns["RowIndex"];
                    dtDividendYieldValueCollection.PrimaryKey = columns;
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
                        if (dtDividendYieldValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtDividendYieldValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtDividendYieldValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtDividendYieldValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData",
                        _dividendYieldValueCollection.Count, _totalImportDataXmlFilePath);
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