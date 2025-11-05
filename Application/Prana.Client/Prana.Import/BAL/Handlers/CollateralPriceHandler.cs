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
    sealed public class CollateralPriceHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables
        // list collection of import Collateral values
        /// <summary>
        /// The _Collateral value collection
        /// </summary>
        List<CollateralImport> _collateralValueCollection = new List<CollateralImport>();
        /// <summary>
        /// The _current result
        /// </summary>
        TaskResult _currentResult = new TaskResult();
        /// <summary>
        /// The _validated Collateral value collection
        /// </summary>
        List<CollateralImport> _validatedCollateralValueCollection = new List<CollateralImport>();
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
        /// The _Collateral symbology wise dictionary
        /// </summary>
        Dictionary<int, Dictionary<string, List<CollateralImport>>> _collateralSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<CollateralImport>>>();

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
        Dictionary<string, CollateralImport> _dictRequestedSymbol = new Dictionary<string, CollateralImport>();

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
        /// Initializes a new instance of the <see cref="CollateralPriceHandler"/> class.
        /// </summary>
        public CollateralPriceHandler() { }

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
                GetSMDataForCollateralImport();


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
        /// Saves the Collateral values.
        /// </summary>
        private void SaveCollateralValues()
        {
            try
            {
                int rowsUpdated = 0;
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                //UpdateValidatedDailyCollateralCollection();
                if (_validatedCollateralValueCollection.Count > 0)
                {
                    // total number of records inserted

                    DataTable dtCollateralValues = CreateDataTableForCollateralImport();
                    DataTable dtCollateralValueImportData = GeneralUtilities.GetDataTableFromList(_validatedCollateralValueCollection);

                    DataTable dtCollateralTableFromCollection = GeneralUtilities.CreateTableFromCollection<CollateralImport>(dtCollateralValues, _validatedCollateralValueCollection);

                    try
                    {
                        if ((dtCollateralTableFromCollection != null) && (dtCollateralTableFromCollection.Rows.Count > 0))
                        {
                            rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveCollateralValues(dtCollateralTableFromCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtCollateralValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }

                    if (rowsUpdated > 0)
                    {
                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtCollateralValueImportData.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtCollateralValueImportData);
                    }

                    int importedTradesCount = dtCollateralValueImportData.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtCollateralValueImportData.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    if (_currentResult != null)
                    {
                        if (_collateralValueCollection.Count == _validatedCollateralValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                        }
                        else if ((_collateralValueCollection.Count != _validatedCollateralValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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

                if (_collateralSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<CollateralImport>> dictSymbols = _collateralSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<CollateralImport> listCollateralValues = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (CollateralImport CollateralImport in listCollateralValues)
                        {
                            validateAllSymbols(CollateralImport, secMasterObj);

                            CollateralImport.Symbol = pranaSymbol;
                            CollateralImport.CUSIP = cuspiSymbol;
                            CollateralImport.ISIN = isinSymbol;
                            CollateralImport.SEDOL = sedolSymbol;
                            CollateralImport.Bloomberg = bloombergSymbol;
                            CollateralImport.RIC = reutersSymbol;
                            CollateralImport.OSIOptionSymbol = osiOptionSymbol;
                            CollateralImport.IDCOOptionSymbol = idcoOptionSymbol;
                            CollateralImport.OpraOptionSymbol = opraOptionSymbol;
                            CollateralImport.AUECID = secMasterObj.AUECID;

                            if (CollateralImport.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                _validatedCollateralValueCollection.Add(CollateralImport);
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
            return "ImportCollateralPrice.xsd";
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
                _collateralSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                //_CollateralValueCollection.Clear();
                _collateralValueCollection.Clear();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(CollateralImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    CollateralImport collateralValue = new CollateralImport();
                    collateralValue.Symbol = string.Empty;
                    collateralValue.FundId = 0;
                    collateralValue.CollateralPrice = 0;
                    collateralValue.Haircut = 0;
                    collateralValue.RebateOnMV = 0;
                    collateralValue.RebateOnCollateral = 0;
                    collateralValue.Date = string.Empty;
                    collateralValue.AUECID = 0;
                    collateralValue.RowIndex = irow;
                    collateralValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, collateralValue, irow);
                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        collateralValue.FundId = runUpload.SelectedAccount;
                        collateralValue.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!string.IsNullOrEmpty(collateralValue.AccountName))
                    {
                        collateralValue.FundId = CachedDataManager.GetInstance.GetAccountID(collateralValue.AccountName.Trim());
                    }
                    //TODO: Need to handle user selected date 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        collateralValue.Date = dtn.ToString(DATEFORMAT);
                    }
                    else
                    {
                        if (!collateralValue.Date.Equals(string.Empty))
                        {
                            bool isParsed = false;
                            double outResult;
                            isParsed = double.TryParse(collateralValue.Date, out outResult);
                            if (isParsed)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(collateralValue.Date));
                                collateralValue.Date = dtn.ToString(DATEFORMAT);
                            }
                            else
                            {
                                try
                                {
                                    DateTime dtn = Convert.ToDateTime(collateralValue.Date);
                                    collateralValue.Date = dtn.ToString(DATEFORMAT);
                                }
                                // To check invalid date format from xslt
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                                    collateralValue.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                    if (!Regex.IsMatch(collateralValue.ValidationError, "Invalid Date Format", RegexOptions.IgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(collateralValue.ValidationError))
                                            collateralValue.ValidationError += Seperators.SEPERATOR_8;
                                        collateralValue.ValidationError += " Invalid Date Format ";
                                    }
                                }
                            }
                        }
                    }
                    string uniqueID = string.Empty;

                    //if (string.IsNullOrEmpty(collateralValue.Symbology))
                    //{
                    //    if (!string.IsNullOrEmpty(collateralValue.Symbol))
                    //    {
                    //        collateralValue.Symbology = "Symbol";
                    //    }
                    //}

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(collateralValue.Symbology))
                    {
                        SetSymbology(collateralValue);
                    }

                    switch (collateralValue.Symbology.Trim().ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(collateralValue.Symbol))
                        case "SYMBOL":
                            collateralValue.Symbol = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + collateralValue.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + collateralValue.Symbol), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.Symbol.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[0];
                                if (collateralSameSymbologyDict.ContainsKey(collateralValue.Symbol))
                                {
                                    List<CollateralImport> collateralSymbolWiseList = collateralSameSymbologyDict[collateralValue.Symbol];
                                    collateralSymbolWiseList.Add(collateralValue);
                                    collateralSameSymbologyDict[collateralValue.Symbol] = collateralSymbolWiseList;
                                    _collateralSymbologyWiseDict[0] = collateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[0].Add(collateralValue.Symbol, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbolDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameSymbolDict.Add(collateralValue.Symbol, collaterallist);
                                _collateralSymbologyWiseDict.Add(0, CollateralSameSymbolDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(CollateralValue.RIC))
                        case "RIC":
                            collateralValue.RIC = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + collateralValue.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + collateralValue.RIC), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.RIC.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[1];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.RIC))
                                {
                                    List<CollateralImport> CollateralRICWiseList = CollateralSameSymbologyDict[collateralValue.RIC];
                                    CollateralRICWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.RIC] = CollateralRICWiseList;
                                    _collateralSymbologyWiseDict[1] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> CollateralList = new List<CollateralImport>();
                                    CollateralList.Add(collateralValue);
                                    _collateralSymbologyWiseDict[1].Add(collateralValue.RIC, CollateralList);
                                }
                            }
                            else
                            {
                                List<CollateralImport> CollateralList = new List<CollateralImport>();
                                CollateralList.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameRICDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameRICDict.Add(collateralValue.RIC, CollateralList);
                                _collateralSymbologyWiseDict.Add(1, CollateralSameRICDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(CollateralValue.ISIN))
                        case "ISIN":
                            collateralValue.ISIN = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + collateralValue.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + collateralValue.ISIN), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.ISIN.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[2];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.ISIN))
                                {
                                    List<CollateralImport> CollateralISINWiseList = CollateralSameSymbologyDict[collateralValue.ISIN];
                                    CollateralISINWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.ISIN] = CollateralISINWiseList;
                                    _collateralSymbologyWiseDict[2] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[2].Add(collateralValue.ISIN, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameISINDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameISINDict.Add(collateralValue.ISIN, Collaterallist);
                                _collateralSymbologyWiseDict.Add(2, CollateralSameISINDict);
                            }
                            break;

                        case "SEDOL":
                            collateralValue.SEDOL = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + collateralValue.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + collateralValue.SEDOL), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.SEDOL.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[3];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.SEDOL))
                                {
                                    List<CollateralImport> CollateralSEDOLWiseList = CollateralSameSymbologyDict[collateralValue.SEDOL];
                                    CollateralSEDOLWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.SEDOL] = CollateralSEDOLWiseList;
                                    _collateralSymbologyWiseDict[3] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[3].Add(collateralValue.SEDOL, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSEDOLDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSEDOLDict.Add(collateralValue.SEDOL, Collaterallist);
                                _collateralSymbologyWiseDict.Add(3, CollateralSEDOLDict);
                            }
                            break;

                        case "CUSIP":
                            collateralValue.CUSIP = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + collateralValue.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + collateralValue.CUSIP), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.CUSIP.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[4];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.CUSIP))
                                {
                                    List<CollateralImport> CollateralCUSIPWiseList = CollateralSameSymbologyDict[collateralValue.CUSIP];
                                    CollateralCUSIPWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.CUSIP] = CollateralCUSIPWiseList;
                                    _collateralSymbologyWiseDict[4] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[4].Add(collateralValue.CUSIP, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameCUSIPDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameCUSIPDict.Add(collateralValue.CUSIP, Collaterallist);
                                _collateralSymbologyWiseDict.Add(4, CollateralSameCUSIPDict);
                            }
                            break;
                        case "BLOOMBERG":
                            collateralValue.Bloomberg = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + collateralValue.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + collateralValue.Bloomberg), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.Bloomberg.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[5];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.Bloomberg))
                                {
                                    List<CollateralImport> CollateralBloombergWiseList = CollateralSameSymbologyDict[collateralValue.Bloomberg];
                                    CollateralBloombergWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.Bloomberg] = CollateralBloombergWiseList;
                                    _collateralSymbologyWiseDict[5] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[5].Add(collateralValue.Bloomberg, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameBloombergDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameBloombergDict.Add(collateralValue.Bloomberg, Collaterallist);
                                _collateralSymbologyWiseDict.Add(5, CollateralSameBloombergDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(CollateralValue.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            collateralValue.OSIOptionSymbol = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + collateralValue.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + collateralValue.OSIOptionSymbol), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.OSIOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[6];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.OSIOptionSymbol))
                                {
                                    List<CollateralImport> CollateralOSIWiseList = CollateralSameSymbologyDict[collateralValue.OSIOptionSymbol];
                                    CollateralOSIWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.OSIOptionSymbol] = CollateralOSIWiseList;
                                    _collateralSymbologyWiseDict[6] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[6].Add(collateralValue.OSIOptionSymbol, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameOSIDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameOSIDict.Add(collateralValue.OSIOptionSymbol, Collaterallist);
                                _collateralSymbologyWiseDict.Add(6, CollateralSameOSIDict);
                            }
                            break;
                        // else if (!String.IsNullOrEmpty(CollateralValue.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            collateralValue.IDCOOptionSymbol = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + collateralValue.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + collateralValue.IDCOOptionSymbol), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.IDCOOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[7];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.IDCOOptionSymbol))
                                {
                                    List<CollateralImport> CollateralIDCOWiseList = CollateralSameSymbologyDict[collateralValue.IDCOOptionSymbol];
                                    CollateralIDCOWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.IDCOOptionSymbol] = CollateralIDCOWiseList;
                                    _collateralSymbologyWiseDict[7] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[7].Add(collateralValue.IDCOOptionSymbol, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameIDCODict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameIDCODict.Add(collateralValue.IDCOOptionSymbol, Collaterallist);
                                _collateralSymbologyWiseDict.Add(7, CollateralSameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(CollateralValue.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            collateralValue.OpraOptionSymbol = collateralValue.Symbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + collateralValue.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + collateralValue.OpraOptionSymbol), collateralValue);
                            }
                            uniqueID = collateralValue.Date + collateralValue.OpraOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                            if (_collateralSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<CollateralImport>> CollateralSameSymbologyDict = _collateralSymbologyWiseDict[8];
                                if (CollateralSameSymbologyDict.ContainsKey(collateralValue.OpraOptionSymbol))
                                {
                                    List<CollateralImport> CollateralOpraWiseList = CollateralSameSymbologyDict[collateralValue.OpraOptionSymbol];
                                    CollateralOpraWiseList.Add(collateralValue);
                                    CollateralSameSymbologyDict[collateralValue.OpraOptionSymbol] = CollateralOpraWiseList;
                                    _collateralSymbologyWiseDict[8] = CollateralSameSymbologyDict;
                                }
                                else
                                {
                                    List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                    Collaterallist.Add(collateralValue);
                                    _collateralSymbologyWiseDict[8].Add(collateralValue.OpraOptionSymbol, Collaterallist);
                                }
                            }
                            else
                            {
                                List<CollateralImport> Collaterallist = new List<CollateralImport>();
                                Collaterallist.Add(collateralValue);
                                Dictionary<string, List<CollateralImport>> CollateralSameOpraDict = new Dictionary<string, List<CollateralImport>>();
                                CollateralSameOpraDict.Add(collateralValue.OpraOptionSymbol, Collaterallist);
                                _collateralSymbologyWiseDict.Add(7, CollateralSameOpraDict);
                            }
                            break;
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _collateralValueCollection.Add(collateralValue);
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
        /// <param name="CollateralValue">The Collateral value.</param>
        private static void SetSymbology(CollateralImport CollateralValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(CollateralValue.Symbol))
                    CollateralValue.Symbology = Constants.ImportSymbologies.Symbol.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.RIC))
                    CollateralValue.Symbology = Constants.ImportSymbologies.RIC.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.ISIN))
                    CollateralValue.Symbology = Constants.ImportSymbologies.ISIN.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.SEDOL))
                    CollateralValue.Symbology = Constants.ImportSymbologies.SEDOL.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.CUSIP))
                    CollateralValue.Symbology = Constants.ImportSymbologies.CUSIP.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.Bloomberg))
                    CollateralValue.Symbology = Constants.ImportSymbologies.Bloomberg.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.OSIOptionSymbol))
                    CollateralValue.Symbology = Constants.ImportSymbologies.OSIOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.IDCOOptionSymbol))
                    CollateralValue.Symbology = Constants.ImportSymbologies.IDCOOptionSymbol.ToString();
                else if (!String.IsNullOrEmpty(CollateralValue.OpraOptionSymbol))
                    CollateralValue.Symbology = Constants.ImportSymbologies.OpraOptionSymbol.ToString();
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
        /// Gets the sm data for Collateral import.
        /// </summary>
        private void GetSMDataForCollateralImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_collateralSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<CollateralImport>>> kvp in _collateralSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<CollateralImport>> symbolDict = _collateralSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<CollateralImport>> keyvaluepair in symbolDict)
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
        /// Creates the data table for Collateral import.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDataTableForCollateralImport()
        {
            DataTable dtCollateralPriceTemp = new DataTable();
            try
            {
                dtCollateralPriceTemp.TableName = "DailyCollateralPrice";
                dtCollateralPriceTemp.Columns.Add(new DataColumn("Symbol", typeof(string)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("Date", typeof(string)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("FundId", typeof(Int32)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("CollateralPrice", typeof(double)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("Haircut", typeof(double)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnMV", typeof(double)));
                dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnCollateral", typeof(double)));
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
            return dtCollateralPriceTemp;
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

                    if (_collateralValueCollection != null)
                    {
                        DataTable dtCollateralValueCollection = GeneralUtilities.GetDataTableFromList(_collateralValueCollection);

                        List<CollateralImport> lstNonValidatedCollateralValue = _collateralValueCollection.FindAll(CollateralValue => CollateralValue.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                        DataTable dtNonValidatedCollateralValue = GeneralUtilities.GetDataTableFromList(lstNonValidatedCollateralValue);
                        dtNonValidatedCollateralValue.TableName = "NonValidatedCollateralValue";
                        //if (dtNonValidatedCollateralValue != null && dtNonValidatedCollateralValue.Rows.Count > 0)
                        //{
                        //    dtNonValidatedCollateralValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                        //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedCollateralValue", lstNonValidatedCollateralValue.Count, nonValidatedXmlFilePath);
                        //}

                        DataTable dtValidatedCollateralValue = GeneralUtilities.GetDataTableFromList(_validatedCollateralValueCollection);
                        dtValidatedCollateralValue.TableName = "ValidatedCollateralValue";
                        if (dtValidatedCollateralValue != null && dtValidatedCollateralValue.Rows.Count > 0)
                        {
                            dtValidatedCollateralValue.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath);
                            if (_currentResult != null)
                                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedCollateralValue", _validatedCollateralValueCollection.Count, _validatedSymbolsXmlFilePath);
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

                        if (dtCollateralValueCollection != null && dtCollateralValueCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing. For re-run upload & re-reun Symbol validation it will be false.
                            //if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtCollateralValueCollection);
                            //}
                        }
                        #endregion

                        #region write xml for validated symbols

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtValidatedCollateralValue, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";
                            dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion

                        if (_isSaveDataInApplication)
                        {
                            if (_validatedCollateralValueCollection.Count == 0 && _currentResult != null)
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
                                //Validated Collateral Value objects will be added to _validatedCollateralValueCollection
                                if (_collateralValueCollection.Count == _validatedCollateralValueCollection.Count)
                                {
                                    SaveCollateralValues();
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
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols, Dictionary<string, CollateralImport> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, CollateralImport> item in dictRequestedSymbol)
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
        /// <param name="CollateralValue">The Collateral value.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        private void validateAllSymbols(CollateralImport CollateralValue, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                CollateralValue.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(CollateralValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    CollateralValue.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (CollateralValue.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(CollateralValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    CollateralValue.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (CollateralValue.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    CollateralValue.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (CollateralValue.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    CollateralValue.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (CollateralValue.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    CollateralValue.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (CollateralValue.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    CollateralValue.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (CollateralValue.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    CollateralValue.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (CollateralValue.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(CollateralValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    CollateralValue.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(CollateralValue.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (CollateralValue.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (mismatchComment.Length > 0)
                {
                    CollateralValue.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(CollateralValue.MismatchType))
                    {
                        CollateralValue.MismatchType += ", ";
                    }
                    CollateralValue.MismatchType += "Symbol Mismatch";
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
        /// <param name="dtCollateralValueCollection">The dt Collateral value collection.</param>
        private void UpdateImportDataXML(DataTable dtCollateralValueCollection)
        {
            dtCollateralValueCollection.TableName = "ImportData";
            try
            {
                if (dtCollateralValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtCollateralValueCollection.Columns["RowIndex"];
                    dtCollateralValueCollection.PrimaryKey = columns;
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
                        if (dtCollateralValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtCollateralValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtCollateralValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtCollateralValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _collateralValueCollection.Count, _totalImportDataXmlFilePath);
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