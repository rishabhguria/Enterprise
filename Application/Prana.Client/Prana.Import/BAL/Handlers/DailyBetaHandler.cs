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
using System.Xml;

namespace Prana.Import
{
    sealed public class DailyBetaHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables
        // list collection of import beta values
        List<BetaImport> _betaValueCollection = new List<BetaImport>();
        TaskResult _currentResult = new TaskResult();
        List<BetaImport> _validatedBetaValueCollection = new List<BetaImport>();
        //private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);
        private System.Timers.Timer _timerSecurityValidation;
        bool _isSaveDataInApplication = false;
        private int _importedTradesCount = 0;
        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        Dictionary<int, Dictionary<string, List<BetaImport>>> _betaSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<BetaImport>>>();

        const string DATEFORMAT = "MM/dd/yyyy";
        public event EventHandler TaskSpecificDataPointsPreUpdate;

        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        Dictionary<string, BetaImport> _dictRequestedSymbol = new Dictionary<string, BetaImport>();

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

        public DailyBetaHandler() { }

        #region IImportHandler Members
        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                WireEvents();
                _currentResult = taskResult as TaskResult;
                //As discussed with sandeep ji, it must be checked for Netposition/ Transactions and BetaValue import
                SecurityMasterManager.Instance.GenerateSMMapping(ds);
                _isSaveDataInApplication = isSaveDataInApplication;

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);

                //if (runUpload.IsPriceToleranceChecked)
                //{
                //    DataTable dt = ValidatePriceTolerance(ds);

                //    string validatedPriceToleranceXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_PriceValidatedBetaPrices" + ".xml";
                //    string nonValidatedPriceToleranceXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_PriceNonValidatedBetaPrices" + ".xml";

                //    dt.WriteXml(Application.StartupPath + validatedPriceToleranceXmlFilePath);
                //    ds.Tables[0].WriteXml(Application.StartupPath + nonValidatedPriceToleranceXmlFilePath);

                //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PriceValidatedBetaPrices", dt.Rows.Count, validatedPriceToleranceXmlFilePath);
                //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PriceNonValidatedBetaPrices", ds.Tables[0].Rows.Count, nonValidatedPriceToleranceXmlFilePath);

                //    //Price tolrerance validated for all BetaValue
                //    if (ds.Tables[0].Rows.Count == 0)
                //    {
                //        ds.Tables.Clear();
                //        ds.Tables.Add(dt);
                //        SetCollection(ds);
                //        GetSMDataForBetaImport();
                //        //all rows validarted price tolerance
                //        //MessageBox.Show("Import Done successfully", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //    else
                //    {
                //        if (TaskSpecificDataPointsPreUpdate != null)
                //        {
                //            _currentResult.Error = new Exception(ds.Tables[0].Rows.Count + " trades failed price validation");
                //            //Return TaskResult which was recieved from ImportManager as event argument
                //            TaskSpecificDataPointsPreUpdate(this, _currentResult);
                //        }
                //    }
                //}
                ////_SMRequest used in method FillSecurityMasterDataFromObj in ctrlReconDownload
                ////_SMRequest = EnmImportType.BetaImport.ToString;                
                //else
                //{
                SetCollection(ds, runUpload);
                GetSMDataForBetaImport();

                //SaveBetaValues();
                //}               
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

        private void SaveBetaValues()
        {
            try
            {
                int rowsUpdated = 0;
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                //UpdateValidatedDailyBetaCollection();
                if (_validatedBetaValueCollection.Count > 0)
                {
                    // total number of records inserted

                    DataTable dtBetaValues = CreateDataTableForBetaImport();
                    DataTable dtBetaValueImportData = GeneralUtilities.GetDataTableFromList(_validatedBetaValueCollection);

                    DataTable dtBetaTableFromCollection = GeneralUtilities.CreateTableFromCollection<BetaImport>(dtBetaValues, _validatedBetaValueCollection);

                    try
                    {
                        if ((dtBetaTableFromCollection != null) && (dtBetaTableFromCollection.Rows.Count > 0))
                        {
                            rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveBeta(dtBetaTableFromCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtBetaValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                        if (_currentResult != null)
                            _currentResult.Error = new Exception(ex.Message);
                    }

                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtBetaValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtBetaValueImportData);
                    }

                    int importedTradesCount = dtBetaValueImportData.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtBetaValueImportData.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (_currentResult != null)
                    {
                        if (_betaValueCollection.Count == _validatedBetaValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                        }
                        else if ((_betaValueCollection.Count != _validatedBetaValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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

        readonly object _object = new object();
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
                lock (_object)
                {
                    _secMasterResponseCollection.Add(secMasterObj);
                }

                if (_betaSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<BetaImport>> dictSymbols = _betaSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<BetaImport> listBetaValues = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (BetaImport betaImport in listBetaValues)
                        {
                            validateAllSymbols(betaImport, secMasterObj);

                            betaImport.Symbol = pranaSymbol;
                            betaImport.CUSIP = cuspiSymbol;
                            betaImport.ISIN = isinSymbol;
                            betaImport.SEDOL = sedolSymbol;
                            betaImport.Bloomberg = bloombergSymbol;
                            betaImport.RIC = reutersSymbol;
                            betaImport.OSIOptionSymbol = osiOptionSymbol;
                            betaImport.IDCOOptionSymbol = idcoOptionSymbol;
                            betaImport.OpraOptionSymbol = opraOptionSymbol;
                            betaImport.AUECID = secMasterObj.AUECID;

                            if (betaImport.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                lock (_object)
                                {
                                    _validatedBetaValueCollection.Add(betaImport);
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

        // TODO : Needs to be removed as hadled in Import setup UI.
        public string GetXSDName()
        {
            return "ImportBetaValues.xsd";
        }

        #endregion

        void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _betaSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                //_betaValueCollection.Clear();
                _betaValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(BetaImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    BetaImport betaValue = new BetaImport();
                    betaValue.Symbol = string.Empty;
                    betaValue.Beta = 0;
                    betaValue.Date = string.Empty;
                    betaValue.AUECID = 0;
                    betaValue.RowIndex = irow;
                    betaValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, betaValue, irow);

                    //TODO: Need to handle user selected date 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        betaValue.Date = dtn.ToString(DATEFORMAT);
                    }
                    else
                    {
                        if (!betaValue.Date.Equals(string.Empty))
                        {
                            bool isParsed = false;
                            double outResult;
                            isParsed = double.TryParse(betaValue.Date, out outResult);
                            if (isParsed)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(betaValue.Date));
                                betaValue.Date = dtn.ToString(DATEFORMAT);
                            }
                            else
                            {
                                try
                                {
                                    DateTime dtn = Convert.ToDateTime(betaValue.Date);
                                    betaValue.Date = dtn.ToString(DATEFORMAT);
                                }
                                // To check invalid date format from xslt
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                    betaValue.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                    if (!Regex.IsMatch(betaValue.ValidationError, "Invalid Date Format", RegexOptions.IgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(betaValue.ValidationError))
                                            betaValue.ValidationError += Seperators.SEPERATOR_8;
                                        betaValue.ValidationError += " Invalid Date Format ";
                                    }
                                }
                            }
                        }
                    }
                    string uniqueID = string.Empty;

                    //if (string.IsNullOrEmpty(betaValue.Symbology))
                    //{
                    //    if (!string.IsNullOrEmpty(betaValue.Symbol))
                    //    {
                    //        betaValue.Symbology = "Symbol";
                    //    }
                    //}

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(betaValue.Symbology))
                    {
                        SetSymbology(betaValue);
                    }

                    switch (betaValue.Symbology.Trim().ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(betaValue.Symbol))
                        case "SYMBOL":
                            betaValue.Symbol = betaValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + betaValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + betaValue.Symbol), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.Symbol.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[0];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.Symbol))
                                {
                                    List<BetaImport> betaSymbolWiseList = betaSameSymbologyDict[betaValue.Symbol];
                                    betaSymbolWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.Symbol] = betaSymbolWiseList;
                                    _betaSymbologyWiseDict[0] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[0].Add(betaValue.Symbol, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameSymbolDict = new Dictionary<string, List<BetaImport>>();
                                betaSameSymbolDict.Add(betaValue.Symbol, betalist);
                                _betaSymbologyWiseDict.Add(0, betaSameSymbolDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.RIC))
                        case "RIC":
                            betaValue.RIC = betaValue.RIC.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + betaValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + betaValue.RIC), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.RIC.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[1];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.RIC))
                                {
                                    List<BetaImport> betaRICWiseList = betaSameSymbologyDict[betaValue.RIC];
                                    betaRICWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.RIC] = betaRICWiseList;
                                    _betaSymbologyWiseDict[1] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betaList = new List<BetaImport>();
                                    betaList.Add(betaValue);
                                    _betaSymbologyWiseDict[1].Add(betaValue.RIC, betaList);
                                }
                            }
                            else
                            {
                                List<BetaImport> betaList = new List<BetaImport>();
                                betaList.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameRICDict = new Dictionary<string, List<BetaImport>>();
                                betaSameRICDict.Add(betaValue.RIC, betaList);
                                _betaSymbologyWiseDict.Add(1, betaSameRICDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.ISIN))
                        case "ISIN":
                            betaValue.ISIN = betaValue.ISIN.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + betaValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + betaValue.ISIN), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.ISIN.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[2];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.ISIN))
                                {
                                    List<BetaImport> betaISINWiseList = betaSameSymbologyDict[betaValue.ISIN];
                                    betaISINWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.ISIN] = betaISINWiseList;
                                    _betaSymbologyWiseDict[2] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[2].Add(betaValue.ISIN, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameISINDict = new Dictionary<string, List<BetaImport>>();
                                betaSameISINDict.Add(betaValue.ISIN, betalist);
                                _betaSymbologyWiseDict.Add(2, betaSameISINDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.SEDOL))
                        case "SEDOL":
                            betaValue.SEDOL = betaValue.SEDOL.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + betaValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + betaValue.SEDOL), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.SEDOL.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[3];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.SEDOL))
                                {
                                    List<BetaImport> betaSEDOLWiseList = betaSameSymbologyDict[betaValue.SEDOL];
                                    betaSEDOLWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.SEDOL] = betaSEDOLWiseList;
                                    _betaSymbologyWiseDict[3] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[3].Add(betaValue.SEDOL, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSEDOLDict = new Dictionary<string, List<BetaImport>>();
                                betaSEDOLDict.Add(betaValue.SEDOL, betalist);
                                _betaSymbologyWiseDict.Add(3, betaSEDOLDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.CUSIP))
                        case "CUSIP":
                            betaValue.CUSIP = betaValue.CUSIP.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + betaValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + betaValue.CUSIP), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.CUSIP.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[4];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.CUSIP))
                                {
                                    List<BetaImport> betaCUSIPWiseList = betaSameSymbologyDict[betaValue.CUSIP];
                                    betaCUSIPWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.CUSIP] = betaCUSIPWiseList;
                                    _betaSymbologyWiseDict[4] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[4].Add(betaValue.CUSIP, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameCUSIPDict = new Dictionary<string, List<BetaImport>>();
                                betaSameCUSIPDict.Add(betaValue.CUSIP, betalist);
                                _betaSymbologyWiseDict.Add(4, betaSameCUSIPDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.Bloomberg))
                        case "BLOOMBERG":
                            betaValue.Bloomberg = betaValue.Bloomberg.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + betaValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + betaValue.Bloomberg), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.Bloomberg.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[5];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.Bloomberg))
                                {
                                    List<BetaImport> betaBloombergWiseList = betaSameSymbologyDict[betaValue.Bloomberg];
                                    betaBloombergWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.Bloomberg] = betaBloombergWiseList;
                                    _betaSymbologyWiseDict[5] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[5].Add(betaValue.Bloomberg, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameBloombergDict = new Dictionary<string, List<BetaImport>>();
                                betaSameBloombergDict.Add(betaValue.Bloomberg, betalist);
                                _betaSymbologyWiseDict.Add(5, betaSameBloombergDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            betaValue.OSIOptionSymbol = betaValue.OSIOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + betaValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + betaValue.OSIOptionSymbol), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.OSIOptionSymbol.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[6];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.OSIOptionSymbol))
                                {
                                    List<BetaImport> betaOSIWiseList = betaSameSymbologyDict[betaValue.OSIOptionSymbol];
                                    betaOSIWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.OSIOptionSymbol] = betaOSIWiseList;
                                    _betaSymbologyWiseDict[6] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[6].Add(betaValue.OSIOptionSymbol, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameOSIDict = new Dictionary<string, List<BetaImport>>();
                                betaSameOSIDict.Add(betaValue.OSIOptionSymbol, betalist);
                                _betaSymbologyWiseDict.Add(6, betaSameOSIDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(betaValue.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            betaValue.IDCOOptionSymbol = betaValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + betaValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + betaValue.IDCOOptionSymbol), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.IDCOOptionSymbol.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[7];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.IDCOOptionSymbol))
                                {
                                    List<BetaImport> betaIDCOWiseList = betaSameSymbologyDict[betaValue.IDCOOptionSymbol];
                                    betaIDCOWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.IDCOOptionSymbol] = betaIDCOWiseList;
                                    _betaSymbologyWiseDict[7] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[7].Add(betaValue.IDCOOptionSymbol, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameIDCODict = new Dictionary<string, List<BetaImport>>();
                                betaSameIDCODict.Add(betaValue.IDCOOptionSymbol, betalist);
                                _betaSymbologyWiseDict.Add(7, betaSameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(betaValue.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            betaValue.OpraOptionSymbol = betaValue.OpraOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + betaValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + betaValue.OpraOptionSymbol), betaValue);
                            }
                            uniqueID = betaValue.Date + betaValue.OpraOptionSymbol.Trim().ToUpper();
                            if (_betaSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[8];
                                if (betaSameSymbologyDict.ContainsKey(betaValue.OpraOptionSymbol))
                                {
                                    List<BetaImport> betaOpraWiseList = betaSameSymbologyDict[betaValue.OpraOptionSymbol];
                                    betaOpraWiseList.Add(betaValue);
                                    betaSameSymbologyDict[betaValue.OpraOptionSymbol] = betaOpraWiseList;
                                    _betaSymbologyWiseDict[8] = betaSameSymbologyDict;
                                }
                                else
                                {
                                    List<BetaImport> betalist = new List<BetaImport>();
                                    betalist.Add(betaValue);
                                    _betaSymbologyWiseDict[8].Add(betaValue.OpraOptionSymbol, betalist);
                                }
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                Dictionary<string, List<BetaImport>> betaSameOpraDict = new Dictionary<string, List<BetaImport>>();
                                betaSameOpraDict.Add(betaValue.OpraOptionSymbol, betalist);
                                _betaSymbologyWiseDict.Add(7, betaSameOpraDict);
                            }
                            break;
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _betaValueCollection.Add(betaValue);
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
        /// <param name="betaValue"></param>
        private static void SetSymbology(BetaImport betaValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(betaValue.Symbol))
                    betaValue.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(betaValue.RIC))
                    betaValue.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(betaValue.ISIN))
                    betaValue.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(betaValue.SEDOL))
                    betaValue.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(betaValue.CUSIP))
                    betaValue.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(betaValue.Bloomberg))
                    betaValue.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(betaValue.OSIOptionSymbol))
                    betaValue.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(betaValue.IDCOOptionSymbol))
                    betaValue.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(betaValue.OpraOptionSymbol))
                    betaValue.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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

        private void GetSMDataForBetaImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_betaSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<BetaImport>>> kvp in _betaSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<BetaImport>> symbolDict = _betaSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<BetaImport>> keyvaluepair in symbolDict)
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

        private DataTable CreateDataTableForBetaImport()
        {
            DataTable dtBetaNew = new DataTable();
            try
            {
                dtBetaNew.TableName = "DailyBeta";

                DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
                DataColumn colDate = new DataColumn("Date", typeof(string));
                DataColumn colBetaPrice = new DataColumn("Beta", typeof(double));
                DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

                dtBetaNew.Columns.Add(colSymbol);
                dtBetaNew.Columns.Add(colDate);
                dtBetaNew.Columns.Add(colBetaPrice);
                dtBetaNew.Columns.Add(colAUECID);
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
            return dtBetaNew;
        }
        /// <summary>
        /// This method is no more used
        /// </summary>
        //private void UpdateValidatedDailyBetaCollection()
        //{
        //    try
        //    {
        //        _validatedBetaValueCollection = new List<BetaImport>();

        //        int invalidPositions = 0;
        //        foreach (BetaImport dailyBeta in _betaValueCollection)
        //        {
        //            if (dailyBeta.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
        //            {
        //                _validatedBetaValueCollection.Add(dailyBeta);
        //            }
        //            else
        //            {
        //                invalidPositions++;
        //            }
        //        }

        //        // modified: omshiv, Jan 28, 2014, If user select invalid rows then we will prompt to user,
        //        //if user cancle
        //        if (invalidPositions > 0)
        //        {
        //            //DialogResult result = MessageBox.Show("You have selected invalid trades also, but they will not import." + System.Environment.NewLine + "Do you want to continue? Click 'Yes' for continue and 'No' for cancel import. ", "Nirvana Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //            //if (result == DialogResult.No)
        //            {
        //                _validatedBetaValueCollection.Clear();
        //            }

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

        /// <summary>
        /// Update activity collection asynchronously for each secmaster response
        /// After validating all the symbols we will do import process.
        /// For a single invalidated import will be cancelled
        /// </summary>
        /// <param name="secMasterObj"></param>
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
        /// 
        /// </summary>
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

        private static object _timerLock = new Object();

        /// <summary>
        /// If all the trades are validated then data will be imported in system
        /// Partial validated trades will not be saved in system
        /// Statistics xml will be written for validated and non validated trades
        /// Statistics xml will be written for validated symbols
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (_timerLock)
                {

                    if (_betaValueCollection != null)
                    {
                        DataTable dtBetaValueCollection = GeneralUtilities.GetDataTableFromList(_betaValueCollection);

                        List<BetaImport> lstNonValidatedBetaValue = _betaValueCollection.FindAll(betaValue => betaValue.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                        DataTable dtNonValidatedBetaValue = GeneralUtilities.GetDataTableFromList(lstNonValidatedBetaValue);
                        dtNonValidatedBetaValue.TableName = "NonValidatedBetaValue";
                        //if (dtNonValidatedBetaValue != null && dtNonValidatedBetaValue.Rows.Count > 0)
                        //{
                        //    dtNonValidatedBetaValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                        //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedBetaValue", lstNonValidatedBetaValue.Count, nonValidatedXmlFilePath);
                        //}

                        DataTable dtValidatedBetaValue = GeneralUtilities.GetDataTableFromList(_validatedBetaValueCollection);
                        dtValidatedBetaValue.TableName = "ValidatedBetaValue";
                        if (dtValidatedBetaValue != null && dtValidatedBetaValue.Rows.Count > 0)
                        {
                            dtValidatedBetaValue.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath);
                            if (_currentResult != null)
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedBetaValue", _validatedBetaValueCollection.Count, _validatedSymbolsXmlFilePath);
                        }

                        #region update task specific data

                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        if (_currentResult != null)
                        {
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
                        if (dtValidatedSymbols != null)
                        {
                            AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols, _dictRequestedSymbol);
                        }
                        #endregion

                        #region symbol validation added to task statistics

                        if (dtBetaValueCollection != null && dtBetaValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing. For re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtBetaValueCollection);
                            //}
                        }
                        #endregion

                        #region write xml for validated symbols

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtValidatedBetaValue, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        if (_isSaveDataInApplication)
                        {
                            if (_validatedBetaValueCollection.Count == 0 && _currentResult != null)
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
                            {
                                //Validated BetaValue objects will be added to _validatedBetaValueCollection
                                if (_betaValueCollection.Count == _validatedBetaValueCollection.Count)
                                {
                                    SaveBetaValues();
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
        ///  Now we receive response of all the symbols from BB api, either they are validated or non validated.
        ///  To handle worst case if we didn't receive response from api, we need this method
        /// </summary>
        /// <param name="dtValidatedSymbols"></param>
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols, Dictionary<string, BetaImport> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, BetaImport> item in dictRequestedSymbol)
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
        /// <param name="betaValue"></param>
        /// <param name="secMasterObj"></param>
        private void validateAllSymbols(BetaImport betaValue, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                betaValue.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(betaValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    betaValue.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (betaValue.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(betaValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    betaValue.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (betaValue.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    betaValue.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (betaValue.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    betaValue.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (betaValue.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    betaValue.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (betaValue.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    betaValue.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (betaValue.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    betaValue.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (betaValue.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(betaValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    betaValue.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(betaValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (betaValue.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    betaValue.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(betaValue.MismatchType))
                    {
                        betaValue.MismatchType += ", ";
                    }
                    betaValue.MismatchType += "Symbol Mismatch";
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
        /// <param name="dtBetaValueCollection"></param>
        private void UpdateImportDataXML(DataTable dtBetaValueCollection)
        {
            bool isImportDataXMLUpdated = false;
            dtBetaValueCollection.TableName = "ImportData";
            try
            {
                if (dtBetaValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtBetaValueCollection.Columns["RowIndex"];
                    dtBetaValueCollection.PrimaryKey = columns;
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
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1200
                        // TODO : For now there were conflict error while merging the table with the column "BrokenRulesCollection" so it is removed.
                        if (dtBetaValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtBetaValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        if (_importedTradesCount > 0 && _currentResult != null && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("IsReRunSymbolValidation") && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["IsReRunSymbolValidation"].ToString().Equals("True"))
                        {
                            if (ds.Tables[0].Columns.Contains("RowIndex"))
                            {
                                DataColumn[] columns = new DataColumn[1];
                                columns[0] = ds.Tables[0].Columns["RowIndex"];
                                ds.Tables[0].PrimaryKey = columns;
                            }
                            ds.Tables[0].Merge(dtBetaValueCollection, true, MissingSchemaAction.Ignore);
                            using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                            {
                                ds.WriteXml(xmlWrite);
                            }
                            isImportDataXMLUpdated = true;
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRunSymbolValidation", false, null);
                        }
                        else
                        {
                            dtBetaValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }
                }
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _betaValueCollection.Count, _totalImportDataXmlFilePath);

                if (!isImportDataXMLUpdated)
                {
                    try
                    {
                        using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                        {
                            dtBetaValueCollection.WriteXml(xmlWrite);
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

        #region IDisposable Members

        public void Dispose()
        {
            _timerSecurityValidation.Dispose();
        }

        #endregion
    }
}
