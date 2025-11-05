using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import.Helper;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Prana.Import
{
    sealed public class PositionAndTransactionHandler : IImportHandler, IImportITaskHandler, IDisposable
    {

        public PositionAndTransactionHandler() { }

        #region constants
        private const string CONST_Success = "Success";
        private const string CONST_PartialSuccess = "Partial Success";
        private const string CONST_Failure = "Failure";
        #endregion

        #region member variables
        const int MAXCHUNKSIZE = 1000;
        private int _recordsProcessed = int.MinValue;
        private string _filePath = string.Empty;
        private int _importedFileId;
        #region to remove
        //TODO: remove the following events
        public event EventHandler WorkCompleted;
        public event EventHandler ReImportCompleted;
        public delegate void ProgressEventHandler(object sender, EventArgs<string, bool, Progress> e);
        public event ProgressEventHandler ProgressEvent;
        private bool _isReimporting = false;
        int _rowIndex = int.MinValue;
        public const string PrimaryKey = "RowID";
        #endregion
        RunUpload _runUpload = null;
        BackgroundWorker _bgwImportData = new BackgroundWorker();
        private System.Timers.Timer _timerSecurityValidation;
        private readonly object _lock = new object();
        //private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);
        TaskResult _currentResult = new TaskResult();
        //Why this line of code need Csla dll reference
        Dictionary<int, Dictionary<string, List<PositionMaster>>> _positionMasterSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<PositionMaster>>>();
        #region FilePath
        // Moved to ImportHelper class
        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;

        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;
        #endregion

        List<PositionMaster> _positionMasterCollection = null;
        int _countSymbols = 0;
        int _countValidatedSymbols = 0;
        int _countNonValidatedSymbols = 0;
        bool _isSaveDataInApplication = false;
        Dictionary<string, PositionMaster> _dictRequestedSymbol = new Dictionary<string, PositionMaster>();
        bool _isNavLockDateValidated = true;

        public List<PositionMaster> PositionMasterCollection
        {
            get
            {
                return _positionMasterCollection;
            }
            set
            {
                _positionMasterCollection = value;
            }
        }

        List<PositionMaster> _validatedPositionMasterCollection = new List<PositionMaster>();
        public List<PositionMaster> ValidatedPositionMasterCollection
        {
            get
            {
                return _validatedPositionMasterCollection;
            }
            set
            {
                _validatedPositionMasterCollection = value;
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

        //This is being set true if any single postion has isDisplaySwapColumns true.
        //Why we are driving this property by looping in each position?
        bool isDisplaySwapColumns;
        private int _importedTradesCount = 0;

        /// <summary>
        /// This will be set when server will save the groups chunk through Import.
        /// </summary>
        private bool _isGroupSaved = false;
        public bool IsGroupSaved
        {
            get { return _isGroupSaved; }
            set { _isGroupSaved = value; }
        }

        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;
        #endregion

        #region IImportHandler Members
        /// <summary>
        /// Process request 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="runUpload"></param>
        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                _timerSecurityValidation = new System.Timers.Timer(CachedDataManager.GetSecurityValidationTimeOut());
                //As discussed with sandeep ji, it must be checked for Netposition/ Transactions and markprice import
                SecurityMasterManager.Instance.GenerateSMMapping(ds);
                WireEvents();
                _runUpload = runUpload;
                _currentResult = taskResult as TaskResult;
                _isSaveDataInApplication = isSaveDataInApplication;

                //Modified by omshiv, oct 14, get ExecutionDate from import file for NirvanaProcessDate in System
                string strExecutionDate = string.Empty;
                if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("ExecutionDate"))
                    strExecutionDate = _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["ExecutionDate"].ToString();
                if (!string.IsNullOrWhiteSpace(strExecutionDate))
                {
                    DateTime executionDate = DateTime.MinValue;
                    if (!DateTime.TryParseExact(strExecutionDate, ApplicationConstants.DateFormat,
                           CultureInfo.InvariantCulture, DateTimeStyles.None, out executionDate))
                    {
                        executionDate = runUpload.BatchStartDate;
                    }
                    runUpload.ExecutionDate = executionDate;
                }


                #region xml file path moved to SetDirectoryPath method in ImportHelper class
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
                #endregion

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);
                SaveImportedFileDetails(runUpload);
                SetCollections(ds, runUpload);
                if (!_isNavLockDateValidated)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsNavLockDateValidated", false, null);
                else
                    GetSMDataForTaxlotImport();

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
        /// Update posistion master collection asynchronously for each secmaster response
        /// After validating all the symbols we will do import process.
        /// For a single invalideted taxlot import will be cancelled
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void SecurityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                UpdateCollection(e.Value, string.Empty);
                //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
                //This may be possible that we get response for all the symbols for but some securities are not validated
                //e.g. Side not picking in file, trdae will be invalidated
                //if (_validatedPositionMasterCollection.Count == _positionMasterCollection.Count)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        readonly object _object = new object();
        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            try
            {
                int requestedSymbologyID = secMasterObj.RequestedSymbology;
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

                lock (_object)
                {
                    _secMasterResponseCollection.Add(secMasterObj);
                }
                if (_positionMasterSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<PositionMaster>> dictSymbols = _positionMasterSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        #region remove elements from _dictRequestedSymbol for which we have received response
                        string key = requestedSymbologyID.ToString() + Seperators.SEPERATOR_6 + secMasterObj.RequestedSymbol;
                        if (_dictRequestedSymbol.ContainsKey(key))
                        {
                            _dictRequestedSymbol.Remove(key);
                        }
                        #endregion
                        List<PositionMaster> listPosMaster = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (PositionMaster positionMaster in listPosMaster)
                        {
                            validateAllSymbols(positionMaster, secMasterObj);

                            switch (secMasterObj.AssetCategory)
                            {
                                case AssetCategory.EquityOption:
                                    SecMasterOptObj secMasterOptObj = (SecMasterOptObj)secMasterObj;
                                    positionMaster.Call_Put = secMasterOptObj.PutOrCall;
                                    positionMaster.StrikePrice = secMasterOptObj.StrikePrice;
                                    positionMaster.ExpirationDate = secMasterOptObj.ExpirationDate.ToString();

                                    break;
                                case AssetCategory.Future:
                                    SecMasterFutObj secMasterFutObj = (SecMasterFutObj)secMasterObj;
                                    positionMaster.ExpirationDate = secMasterFutObj.ExpirationDate.ToString();

                                    break;
                                default:

                                    break;
                            }
                            #region setting prop

                            if (positionMaster.AUECID == int.MinValue || positionMaster.AUECID == 0)
                            {
                                positionMaster.AUECID = secMasterObj.AUECID;
                            }


                            positionMaster.IsSecApproved = secMasterObj.IsSecApproved;

                            positionMaster.AssetID = secMasterObj.AssetID;
                            positionMaster.AssetType = (AssetCategory)secMasterObj.AssetID;

                            if (positionMaster.UnderlyingID == int.MinValue || positionMaster.UnderlyingID == 0)
                            {
                                positionMaster.UnderlyingID = secMasterObj.UnderLyingID;
                            }
                            if (positionMaster.ExchangeID == int.MinValue || positionMaster.ExchangeID == 0)
                            {
                                positionMaster.ExchangeID = secMasterObj.ExchangeID;
                            }
                            // TODO : Currency - pickup from securitymaster
                            if (positionMaster.CurrencyID == int.MinValue || positionMaster.CurrencyID == 0)
                            {
                                positionMaster.CurrencyID = secMasterObj.CurrencyID;
                                //PRANA-9628	[Import] - Settlement currency field comes out none instead of Trade Currency while importing.
                                if (positionMaster.SettlementCurrencyID > 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(positionMaster.SettlementCurrencyID)
                                    && positionMaster.SettlCurrencyName != CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID])
                                {
                                    positionMaster.SettlCurrencyName = CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID];
                                }
                            }

                            // Trade Date
                            positionMaster.PositionStartDate = positionMaster.PositionStartDate;
                            if (!string.IsNullOrEmpty(positionMaster.PositionStartDate))
                            {
                                string[] splitDateFieldSlash = positionMaster.PositionStartDate.Split('/');
                                if (splitDateFieldSlash.Length == 1)
                                {
                                    string[] splitDateFieldWithDash = positionMaster.PositionStartDate.Split('-');
                                    if (splitDateFieldWithDash.Length == 1)
                                    {
                                        bool blnIsTrue;
                                        double result;
                                        blnIsTrue = double.TryParse(positionMaster.PositionStartDate, out result);
                                        if (blnIsTrue)
                                        {
                                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionStartDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                                            positionMaster.PositionStartDate = dtn.ToShortDateString();

                                            UpdatePositionMasterAUECandSettlementDate(positionMaster);
                                        }
                                    }
                                    else
                                    {
                                        UpdatePositionMasterAUECandSettlementDate(positionMaster);
                                    }
                                }
                                else
                                {
                                    UpdatePositionMasterAUECandSettlementDate(positionMaster);
                                }
                            }

                            // Process Date
                            if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                            {
                                string[] splitDateFieldSlash = positionMaster.ProcessDate.Split('/');
                                if (splitDateFieldSlash.Length == 1)
                                {
                                    string[] splitDateFieldWithDash = positionMaster.ProcessDate.Split('-');
                                    if (splitDateFieldWithDash.Length == 1)
                                    {
                                        bool blnIsTrue;
                                        double result;
                                        blnIsTrue = double.TryParse(positionMaster.ProcessDate, out result);
                                        if (blnIsTrue)
                                        {
                                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.ProcessDate));
                                            positionMaster.ProcessDate = dtn.ToShortDateString();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                positionMaster.ProcessDate = positionMaster.AUECLocalDate;
                            }

                            // Original Purchase Date
                            if (!string.IsNullOrEmpty(positionMaster.OriginalPurchaseDate))
                            {
                                string[] splitDateFieldSlash = positionMaster.OriginalPurchaseDate.Split('/');
                                if (splitDateFieldSlash.Length == 1)
                                {
                                    string[] splitDateFieldWithDash = positionMaster.OriginalPurchaseDate.Split('-');
                                    if (splitDateFieldWithDash.Length == 1)
                                    {
                                        bool blnIsTrue;
                                        double result;
                                        blnIsTrue = double.TryParse(positionMaster.OriginalPurchaseDate, out result);
                                        if (blnIsTrue)
                                        {
                                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.OriginalPurchaseDate));
                                            positionMaster.OriginalPurchaseDate = dtn.ToShortDateString();
                                        }
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                            {
                                positionMaster.OriginalPurchaseDate = positionMaster.ProcessDate;
                            }
                            else
                            {
                                positionMaster.OriginalPurchaseDate = positionMaster.AUECLocalDate;
                            }

                            UpdatePositionMasterObj(positionMaster, secMasterObj);

                            #endregion
                            if (positionMaster.ValidationStatus.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                lock (_object)
                                {
                                    _validatedPositionMasterCollection.Add(positionMaster);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// validate symbol weather there is any Mismatch or not
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="secMasterObj"></param>
        private void validateAllSymbols(PositionMaster positionMaster, SecMasterBaseObj secMasterObj)
        {
            try
            {
                StringBuilder mismatchComment = new StringBuilder();
                bool isSymbolMismatch = false;
                positionMaster.Symbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                if (string.IsNullOrEmpty(positionMaster.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    positionMaster.CUSIP = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.CUSIP) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString()))
                {
                    if (positionMaster.CUSIP != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~CUSIP~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString());
                    }
                }
                if (string.IsNullOrEmpty(positionMaster.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    positionMaster.ISIN = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.ISIN) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString()))
                {
                    if (positionMaster.ISIN != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~ISIN~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    positionMaster.SEDOL = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.SEDOL) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString()))
                {
                    if (positionMaster.SEDOL != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~SEDOL~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    positionMaster.Bloomberg = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.Bloomberg) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString()))
                {
                    if (positionMaster.Bloomberg != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~Bloomberg~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    positionMaster.RIC = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.RIC) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString()))
                {
                    if (positionMaster.RIC != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~RIC~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    positionMaster.OSIOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.OSIOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString()))
                {
                    if (positionMaster.OSIOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OSIOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    positionMaster.IDCOOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.IDCOOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString()))
                {
                    if (positionMaster.IDCOOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~IDCOOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString());
                    }
                }

                if (string.IsNullOrEmpty(positionMaster.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    positionMaster.OpraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                }
                else if (!string.IsNullOrEmpty(positionMaster.OpraOptionSymbol) && !string.IsNullOrEmpty(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString()))
                {
                    if (positionMaster.OpraOptionSymbol != secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString())
                    {
                        isSymbolMismatch = true;
                        mismatchComment.Append("~OpraOptionSymbol~").Append(secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString());
                    }
                }

                if (positionMaster.Multiplier == 0 && secMasterObj.Multiplier != 0)
                {
                    positionMaster.Multiplier = secMasterObj.Multiplier;
                }
                else if (positionMaster.Multiplier != 0 && secMasterObj.Multiplier != 0)
                {
                    if (positionMaster.Multiplier != secMasterObj.Multiplier)
                    {
                        mismatchComment.Append("~Multiplier~").Append(secMasterObj.Multiplier.ToString());
                        if (!string.IsNullOrEmpty(positionMaster.MismatchType))
                        {
                            positionMaster.MismatchType += ", ";
                            if (!positionMaster.MismatchType.Contains("Multiplier Mismatch"))
                            {
                                positionMaster.MismatchType += "Multiplier Mismatch";
                            }
                        }
                    }
                }
                if (positionMaster.CurrencyID != 0 && secMasterObj.CurrencyID != 0)
                {
                    if (positionMaster.CurrencyID != secMasterObj.CurrencyID)
                    {
                        mismatchComment.Append("~Currency~").Append(secMasterObj.CurrencyID.ToString());
                        if (!string.IsNullOrEmpty(positionMaster.MismatchType))
                        {
                            positionMaster.MismatchType += ", ";
                            if (!positionMaster.MismatchType.Contains("Currency Mismatch"))
                            {
                                positionMaster.MismatchType += "Currency Mismatch";
                            }
                        }
                    }
                }
                positionMaster.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                positionMaster.Call_Put = (int)OptionType.NONE;

                if (mismatchComment.Length > 0)
                {
                    positionMaster.MisMatchDetails += mismatchComment.ToString();
                }
                if (isSymbolMismatch)
                {
                    if (!string.IsNullOrEmpty(positionMaster.MismatchType))
                    {
                        positionMaster.MismatchType += ", ";
                        if (!positionMaster.MismatchType.Contains("Symbol Mismatch"))
                        {
                            positionMaster.MismatchType += "Symbol Mismatch";
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
            return "ImportPositions.xsd";
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

        #region Private Methods
        private void SetCollections(DataSet ds, RunUpload runUpload)
        {
            try
            {
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();



                _positionMasterSymbologyWiseDict.Clear();
                _dictRequestedSymbol.Clear();
                _positionMasterCollection = new List<PositionMaster>();
                _validatedPositionMasterCollection = new List<PositionMaster>();
                _secMasterResponseCollection = new List<SecMasterBaseObj>();
                _countSymbols = 0;
                _countValidatedSymbols = 0;
                isDisplaySwapColumns = false;
                ImportHelper.FillCurrenciesDictionary();
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    PositionMaster positionMaster = new PositionMaster();
                    positionMaster.AUECID = 0;
                    positionMaster.UnderlyingID = 0;
                    positionMaster.ExchangeID = 0;
                    positionMaster.PositionStartDate = string.Empty;
                    positionMaster.AccountName = string.Empty;
                    positionMaster.IsSecApproved = false;
                    //Add row index column to position master table which will act as aprimary key to the file
                    positionMaster.RowIndex = irow;
                    positionMaster.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();
                    positionMaster.ImportFileID = _importedFileId;
                    positionMaster.NirvanaProcessDate = runUpload.ExecutionDate;


                    #region setting prop through reflection

                    //ImportHelper.SetProperty(typeToLoad, ds, positionMaster, irow);
                    ImportHelper.SetPropertyNew(ds.Tables[0].Rows[irow], positionMaster);



                    #region moved to secMasterHelper class
                    //for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    //{
                    //    try
                    //    {
                    //        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();
                    //        // assign into property
                    //        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                    //        if (propInfo != null)
                    //        {
                    //            Type dataType = propInfo.PropertyType;
                    //            if (dataType.FullName.Equals("System.String"))
                    //            {
                    //                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                    //                {
                    //                    propInfo.SetValue(positionMaster, string.Empty, null);
                    //                }
                    //                else
                    //                {
                    //                    propInfo.SetValue(positionMaster, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);
                    //                }
                    //            }
                    //            else if (dataType.FullName.Equals("System.Double"))
                    //            {
                    //                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                    //                {
                    //                    propInfo.SetValue(positionMaster, 0, null);
                    //                }
                    //                else
                    //                {
                    //                    bool blnIsTrue;
                    //                    double result;
                    //                    blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                    //                    if (blnIsTrue)
                    //                    {
                    //                        propInfo.SetValue(positionMaster, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                    //                    }
                    //                    else
                    //                    {
                    //                        propInfo.SetValue(positionMaster, double.MinValue, null);
                    //                    }
                    //                }
                    //            }
                    //            else if (dataType.FullName.Equals("System.Int32"))
                    //            {
                    //                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                    //                {
                    //                    propInfo.SetValue(positionMaster, 0, null);
                    //                }
                    //                else
                    //                {
                    //                    bool blnIsTrue;
                    //                    int result;
                    //                    blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                    //                    if (blnIsTrue)
                    //                    {
                    //                        propInfo.SetValue(positionMaster, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                    //                    }
                    //                    else
                    //                    {
                    //                        propInfo.SetValue(positionMaster, 0, null);
                    //                    }
                    //                }
                    //            }
                    //            else if (dataType.FullName.Equals("System.Int64"))
                    //            {
                    //                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                    //                {
                    //                    propInfo.SetValue(positionMaster, 0, null);
                    //                }
                    //                else
                    //                {
                    //                    bool blnIsTrue;
                    //                    Int64 result;
                    //                    blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                    //                    if (blnIsTrue)
                    //                    {
                    //                        propInfo.SetValue(positionMaster, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                    //                    }
                    //                    else
                    //                    {
                    //                        propInfo.SetValue(positionMaster, 0, null);
                    //                    }
                    //                }
                    //            }
                    //            else if (dataType.FullName.Equals("System.Boolean"))
                    //            {
                    //if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                    //{
                    //                    propInfo.SetValue(positionMaster, 0, null);
                    //                }
                    //                else
                    //                {
                    //                    bool blnIsTrue;
                    //                    bool result;
                    //                    blnIsTrue = bool.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                    //                    if (blnIsTrue)
                    //                    {
                    //                        propInfo.SetValue(positionMaster, Convert.ToBoolean(ds.Tables[0].Rows[irow][icol]), null);
                    //}
                    //else
                    //{
                    //                        propInfo.SetValue(positionMaster, 0, null);
                    //                    }
                    //                }
                    //            }
                    //            else if (dataType.BaseType.Equals(typeof(System.Enum)))//dataType.FullName.Equals("Prana.BusinessObjects.AppConstants.Operator"))
                    //            {
                    //                //if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                    //                //{
                    //                //    propInfo.SetValue(positionMaster, Operator.M, null);
                    //                //}
                    //                //else
                    //                //{
                    //                //    if (ds.Tables[0].Rows[irow][icol].ToString().Trim().ToUpper().Equals(Operator.M.ToString()))
                    //                //    {
                    //                //        propInfo.SetValue(positionMaster, Operator.M, null);
                    //                //    }
                    //                //    else if (ds.Tables[0].Rows[irow][icol].ToString().Trim().ToUpper().Equals(Operator.D.ToString()))
                    //                //    {
                    //                //        propInfo.SetValue(positionMaster, Operator.D, null);
                    //                //    }
                    //                //    }
                    //                //}
                    //                //Enum handling on generic basis since we are also dealing with another column 
                    //                //CommissionSource now.
                    //                string colValue = ds.Tables[0].Rows[irow][icol].ToString();
                    //                object value = null;
                    //                if (!string.IsNullOrEmpty(colValue))
                    //    {
                    //                    value = Enum.Parse(dataType, colValue, true);
                    //                    propInfo.SetValue(positionMaster, value, null);
                    //    }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    //        if (rethrow)
                    //        {
                    //            throw;
                    //    }
                    //    }
                    //}
                    #endregion

                    #endregion

                    #region Setting prop through caches

                    positionMaster.UserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                    // Rewrote the Code in UpdatePositionMasterObj 
                    //positionMaster.CompanyID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                    //here we are setting trading account id to zero to raise property change event of position master
                    //if trading accounts are permitted to user then trading account id will set
                    positionMaster.TradingAccountID = 0;
                    foreach (TradingAccount tradingAccount in CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.TradingAccounts)
                    {
                        positionMaster.TradingAccountID = tradingAccount.TradingAccountID;
                        break;
                    }

                    positionMaster.PranaMsgType = OrderFields.PranaMsgTypes.ImportPosition;
                    positionMaster.ExternalOrderID = uIDGenerator.GenerateID();
                    //TODO:Gaurav : Need to fill these 
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        positionMaster.PositionStartDate = dtn.ToShortDateString();
                    }

                    //Narendra: Currently these fields are hardcoded 
                    else if (String.IsNullOrEmpty(positionMaster.PositionStartDate))
                    {
                        positionMaster.PositionStartDate = DateTime.Now.Date.ToShortDateString();
                    }
                    //}

                    if (!String.IsNullOrEmpty(positionMaster.SideTagValue))
                    {
                        positionMaster.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionMaster.SideTagValue);
                    }
                    //TODO:Gaurav : Need to fill these 

                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        positionMaster.AccountID = runUpload.SelectedAccount;
                        positionMaster.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.AccountName))
                    {
                        positionMaster.AccountID = CachedDataManager.GetInstance.GetAccountID(positionMaster.AccountName.Trim());
                    }

                    if (!String.IsNullOrEmpty(positionMaster.Strategy))
                    {
                        positionMaster.StrategyID = CachedDataManager.GetInstance.GetStrategyID(positionMaster.Strategy.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(positionMaster.ExecutingBroker))
                    {
                        positionMaster.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(positionMaster.ExecutingBroker); ;
                    }

                    //if (positionMaster.CounterPartyID > 0)
                    //{
                    //    positionMaster.ExecutingBroker = CachedDataManager.GetInstance.GetCounterPartyText(positionMaster.CounterPartyID);
                    //}

                    //Narendra Kumar Jangir, Sept 03 2013
                    //TransactionType added while trade is imported.
                    if (string.IsNullOrEmpty(positionMaster.TransactionType))
                    {
                        //default TransactionType should be trade
                        //positionMaster.TransactionType = TradingTransactionType.Trade.ToString().ToUpper();
                        // added by Sandeep Singh, 11 Nov, 2014. 
                        //Default TransactionType should be side as transaction type is the super set of side
                        positionMaster.TransactionType = CachedDataManager.GetInstance.GetTransactionTypeAcronymByOrderSideTagValue(positionMaster.SideTagValue);
                    }

                    // added by Sandeep Singh, 11 Nov, 2014
                    positionMaster.TransactionSource = TransactionSource.TradeImport;

                    #endregion

                    #region NAVLock Validation
                    if (DateTime.TryParse(positionMaster.PositionStartDate, out DateTime _date))
                    {
                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(_date))
                            _isNavLockDateValidated = false;
                    } 
                    #endregion

                    #region Swap Related Properties
                    if (positionMaster.IsSwapped == 1)
                    {
                        if (!isDisplaySwapColumns)
                            isDisplaySwapColumns = true;

                        positionMaster.NotionalValue = positionMaster.CostBasis * positionMaster.NetPosition;
                        //TODO:Gaurav : Need to fill these 

                        //if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                        //    positionMaster.OrigTransDate = _userSelectedDate;

                    }
                    #endregion

                    #region Creating Symbologywise dictionary

                    //if symbology blank from xslt then pick default symbology 
                    if (string.IsNullOrEmpty(positionMaster.Symbology))
                    {
                        SetSymbology(positionMaster);
                    }

                    switch (positionMaster.Symbology.ToUpper())
                    {
                        // if (!String.IsNullOrEmpty(positionMaster.Symbol))
                        //as discussed with Puneet
                        //here we are converting each symbol to capital as bloomberg deals with securities having caps
                        //and we need to match requested symbol with response symbol

                        case "SYMBOL":
                            positionMaster.Symbol = positionMaster.Symbol.Trim().ToUpper();
                            // Set symbol mapping status to true if symbology is symbol
                            positionMaster.IsSymbolMapped = true;
                            if (!_dictRequestedSymbol.ContainsKey("0" + Seperators.SEPERATOR_6 + positionMaster.Symbol))
                            {
                                _dictRequestedSymbol.Add(("0" + Seperators.SEPERATOR_6 + positionMaster.Symbol), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[0];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.Symbol))
                                {
                                    List<PositionMaster> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[positionMaster.Symbol];
                                    positionMasterSymbolWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.Symbol] = positionMasterSymbolWiseList;
                                    _positionMasterSymbologyWiseDict[0] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[0].Add(positionMaster.Symbol, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbolDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameSymbolDict.Add(positionMaster.Symbol, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(0, positionMasterSameSymbolDict);
                            }

                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.RIC))
                        case "RIC":
                            positionMaster.RIC = positionMaster.RIC.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("1" + Seperators.SEPERATOR_6 + positionMaster.RIC))
                            {
                                _dictRequestedSymbol.Add(("1" + Seperators.SEPERATOR_6 + positionMaster.RIC), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[1];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.RIC))
                                {
                                    List<PositionMaster> positionMasterRICWiseList = positionMasterSameSymbologyDict[positionMaster.RIC];
                                    positionMasterRICWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.RIC] = positionMasterRICWiseList;
                                    _positionMasterSymbologyWiseDict[1] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[1].Add(positionMaster.RIC, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameRICDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameRICDict.Add(positionMaster.RIC, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(1, positionMasterSameRICDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.ISIN))
                        case "ISIN":
                            positionMaster.ISIN = positionMaster.ISIN.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("2" + Seperators.SEPERATOR_6 + positionMaster.ISIN))
                            {
                                _dictRequestedSymbol.Add(("2" + Seperators.SEPERATOR_6 + positionMaster.ISIN), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[2];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.ISIN))
                                {
                                    List<PositionMaster> positionMasterISINWiseList = positionMasterSameSymbologyDict[positionMaster.ISIN];
                                    positionMasterISINWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.ISIN] = positionMasterISINWiseList;
                                    _positionMasterSymbologyWiseDict[2] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[2].Add(positionMaster.ISIN, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameISINDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameISINDict.Add(positionMaster.ISIN, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(2, positionMasterSameISINDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.SEDOL))
                        case "SEDOL":
                            positionMaster.SEDOL = positionMaster.SEDOL.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("3" + Seperators.SEPERATOR_6 + positionMaster.SEDOL))
                            {
                                _dictRequestedSymbol.Add(("3" + Seperators.SEPERATOR_6 + positionMaster.SEDOL), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[3];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.SEDOL))
                                {
                                    List<PositionMaster> positionMasterSEDOLWiseList = positionMasterSameSymbologyDict[positionMaster.SEDOL];
                                    positionMasterSEDOLWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.SEDOL] = positionMasterSEDOLWiseList;
                                    _positionMasterSymbologyWiseDict[3] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[3].Add(positionMaster.SEDOL, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameSEDOLDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameSEDOLDict.Add(positionMaster.SEDOL, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(3, positionMasterSameSEDOLDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.CUSIP))
                        case "CUSIP":
                            positionMaster.CUSIP = positionMaster.CUSIP.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("4" + Seperators.SEPERATOR_6 + positionMaster.CUSIP))
                            {
                                _dictRequestedSymbol.Add(("4" + Seperators.SEPERATOR_6 + positionMaster.CUSIP), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[4];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.CUSIP))
                                {
                                    List<PositionMaster> positionMasterCUSIPWiseList = positionMasterSameSymbologyDict[positionMaster.CUSIP];
                                    positionMasterCUSIPWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.CUSIP] = positionMasterCUSIPWiseList;
                                    _positionMasterSymbologyWiseDict[4] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[4].Add(positionMaster.CUSIP, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameCUSIPDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameCUSIPDict.Add(positionMaster.CUSIP, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(4, positionMasterSameCUSIPDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.Bloomberg))
                        case "BLOOMBERG":
                            positionMaster.Bloomberg = positionMaster.Bloomberg.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("5" + Seperators.SEPERATOR_6 + positionMaster.Bloomberg))
                            {
                                _dictRequestedSymbol.Add(("5" + Seperators.SEPERATOR_6 + positionMaster.Bloomberg), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[5];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.Bloomberg))
                                {
                                    List<PositionMaster> positionMasterBloombergWiseList = positionMasterSameSymbologyDict[positionMaster.Bloomberg];
                                    positionMasterBloombergWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.Bloomberg] = positionMasterBloombergWiseList;
                                    _positionMasterSymbologyWiseDict[5] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[5].Add(positionMaster.Bloomberg, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameBloombergDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameBloombergDict.Add(positionMaster.Bloomberg, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(5, positionMasterSameBloombergDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.OSIOptionSymbol))
                        case "OSIOPTIONSYMBOL":
                            positionMaster.OSIOptionSymbol = positionMaster.OSIOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("6" + Seperators.SEPERATOR_6 + positionMaster.OSIOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("6" + Seperators.SEPERATOR_6 + positionMaster.OSIOptionSymbol), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[6];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OSIOptionSymbol))
                                {
                                    List<PositionMaster> positionMasterOSIWiseList = positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol];
                                    positionMasterOSIWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol] = positionMasterOSIWiseList;
                                    _positionMasterSymbologyWiseDict[6] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[6].Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameOSIDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameOSIDict.Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(6, positionMasterSameOSIDict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.IDCOOptionSymbol))
                        case "IDCOOPTIONSYMBOL":
                            positionMaster.IDCOOptionSymbol = positionMaster.IDCOOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("7" + Seperators.SEPERATOR_6 + positionMaster.IDCOOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("7" + Seperators.SEPERATOR_6 + positionMaster.IDCOOptionSymbol), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[7];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.IDCOOptionSymbol))
                                {
                                    List<PositionMaster> positionMasterIDCOWiseList = positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol];
                                    positionMasterIDCOWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol] = positionMasterIDCOWiseList;
                                    _positionMasterSymbologyWiseDict[7] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[7].Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameIDCODict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameIDCODict.Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(7, positionMasterSameIDCODict);
                            }
                            break;
                        //else if (!String.IsNullOrEmpty(positionMaster.OpraOptionSymbol))
                        case "OPRAOPTIONSYMBOL":
                            positionMaster.OpraOptionSymbol = positionMaster.OpraOptionSymbol.Trim().ToUpper();
                            if (!_dictRequestedSymbol.ContainsKey("8" + Seperators.SEPERATOR_6 + positionMaster.OpraOptionSymbol))
                            {
                                _dictRequestedSymbol.Add(("8" + Seperators.SEPERATOR_6 + positionMaster.OpraOptionSymbol), positionMaster);
                            }
                            if (_positionMasterSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[8];
                                if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OpraOptionSymbol))
                                {
                                    List<PositionMaster> positionMasterOpraWiseList = positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol];
                                    positionMasterOpraWiseList.Add(positionMaster);
                                    positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol] = positionMasterOpraWiseList;
                                    _positionMasterSymbologyWiseDict[8] = positionMasterSameSymbologyDict;
                                }
                                else
                                {
                                    List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                    positionMasterlist.Add(positionMaster);
                                    _positionMasterSymbologyWiseDict[8].Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                                }
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                Dictionary<string, List<PositionMaster>> positionMasterSameOpraDict = new Dictionary<string, List<PositionMaster>>();
                                positionMasterSameOpraDict.Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                                _positionMasterSymbologyWiseDict.Add(7, positionMasterSameOpraDict);
                            }
                            break;
                    }
                    #endregion

                    //modified by omshiv, added Workflowstate of taxlot
                    if (_isSaveDataInApplication)
                        positionMaster.WorkflowState = NirvanaWorkFlowsStats.Imported;
                    _positionMasterCollection.Add(positionMaster);

                }

                // stopwatch.Stop();
                // Logger.LoggerWrite("Time taking in set collection in import" + stopwatch.ElapsedMilliseconds);
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
        /// <param name="positionMaster"></param>
        private static void SetSymbology(PositionMaster positionMaster)
        {
            try
            {
                if (!String.IsNullOrEmpty(positionMaster.Symbol))
                    positionMaster.Symbology = "Symbol";
                else if (!String.IsNullOrEmpty(positionMaster.RIC))
                    positionMaster.Symbology = "RIC";
                else if (!String.IsNullOrEmpty(positionMaster.ISIN))
                    positionMaster.Symbology = "ISIN";
                else if (!String.IsNullOrEmpty(positionMaster.SEDOL))
                    positionMaster.Symbology = "SEDOL";
                else if (!String.IsNullOrEmpty(positionMaster.CUSIP))
                    positionMaster.Symbology = "CUSIP";
                else if (!String.IsNullOrEmpty(positionMaster.Bloomberg))
                    positionMaster.Symbology = "Bloomberg";
                else if (!String.IsNullOrEmpty(positionMaster.OSIOptionSymbol))
                    positionMaster.Symbology = "OSIOptionSymbol";
                else if (!String.IsNullOrEmpty(positionMaster.IDCOOptionSymbol))
                    positionMaster.Symbology = "IDCOOptionSymbol";
                else if (!String.IsNullOrEmpty(positionMaster.OpraOptionSymbol))
                    positionMaster.Symbology = "OpraOptionSymbol";
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

        private void GetSMDataForTaxlotImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_positionMasterSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<PositionMaster>>> kvp in _positionMasterSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<PositionMaster>> symbolDict = _positionMasterSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<PositionMaster>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    //We are increasing total symbols counter as each request is for a separate symbol
                                    //This counter will be used in dashboard xml
                                    _countSymbols++;
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                // secMasterRequestObj.AddNewRow();
                            }
                        }
                    }
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Status", NirvanaTaskStatus.Importing, null);
                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                        //SecurityMaster_SecMstrDataResponse(SecMasterBaseObj secMasterObj) method will be called for each response and collection will be updated asynchronously


                        //Update collection of position master for the symbol response which is fetched from cache
                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                UpdateCollection(secMasterObj, string.Empty);
                            }
                        }

                        //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
                        //This may be possible that we get response for all the symbols for but some data is not validated
                        //e.g. Side not picking in file, trdae will be invalidated
                        //if (_positionMasterCollection.Count == _validatedPositionMasterCollection.Count)
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
                    else
                    {
                        _timerSecurityValidation.Stop();
                        TimerSecurityValidation_Tick(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (_currentResult != null)
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Status", NirvanaTaskStatus.Failure, null);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Merge position master with security details
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        private PositionMaster UpdatePositionMasterObj(PositionMaster positionMaster, SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (positionMaster.AUECID == int.MinValue || positionMaster.AUECID == 0)
                {
                    positionMaster.AUECID = secMasterObj.AUECID;
                }


                positionMaster.IsSecApproved = secMasterObj.IsSecApproved;

                positionMaster.AssetID = secMasterObj.AssetID;
                positionMaster.AssetType = (AssetCategory)secMasterObj.AssetID;

                if (positionMaster.UnderlyingID == int.MinValue || positionMaster.UnderlyingID == 0)
                {
                    positionMaster.UnderlyingID = secMasterObj.UnderLyingID;
                }
                if (positionMaster.ExchangeID == int.MinValue || positionMaster.ExchangeID == 0)
                {
                    positionMaster.ExchangeID = secMasterObj.ExchangeID;
                }
                // TODO : Currency - pickup from securitymaster
                if (positionMaster.CurrencyID == int.MinValue || positionMaster.CurrencyID == 0)
                {
                    positionMaster.CurrencyID = secMasterObj.CurrencyID;
                    //PRANA-9628	[Import] - Settlement currency field comes out none instead of Trade Currency while importing.
                    if (positionMaster.SettlementCurrencyID > 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(positionMaster.SettlementCurrencyID)
                        && positionMaster.SettlCurrencyName != CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID])
                    {
                        positionMaster.SettlCurrencyName = CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID];
                    }
                }

                positionMaster.CompanyID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;

                foreach (TradingAccount tradingAccount in CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.TradingAccounts)
                {
                    positionMaster.TradingAccountID = tradingAccount.TradingAccountID;
                    break;
                }



                // Trade Date
                positionMaster.PositionStartDate = positionMaster.PositionStartDate;
                if (!string.IsNullOrEmpty(positionMaster.PositionStartDate))
                {
                    string[] splitDateFieldSlash = positionMaster.PositionStartDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.PositionStartDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.PositionStartDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionStartDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                                positionMaster.PositionStartDate = dtn.ToShortDateString();

                                UpdatePositionMasterAUECandSettlementDate(positionMaster);
                            }
                        }
                        else
                        {
                            UpdatePositionMasterAUECandSettlementDate(positionMaster);
                        }
                    }
                    else
                    {
                        UpdatePositionMasterAUECandSettlementDate(positionMaster);
                    }
                }

                // Process Date
                if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                {
                    string[] splitDateFieldSlash = positionMaster.ProcessDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.ProcessDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.ProcessDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.ProcessDate));
                                positionMaster.ProcessDate = dtn.ToShortDateString();
                            }
                        }
                    }
                }
                else
                {
                    positionMaster.ProcessDate = positionMaster.AUECLocalDate;
                }

                // Original Purchase Date
                if (!string.IsNullOrEmpty(positionMaster.OriginalPurchaseDate))
                {
                    string[] splitDateFieldSlash = positionMaster.OriginalPurchaseDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.OriginalPurchaseDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.OriginalPurchaseDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.OriginalPurchaseDate));
                                positionMaster.OriginalPurchaseDate = dtn.ToShortDateString();
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                {
                    positionMaster.OriginalPurchaseDate = positionMaster.ProcessDate;
                }
                else
                {
                    positionMaster.OriginalPurchaseDate = positionMaster.AUECLocalDate;
                }


                //modified by : omshiv, jan 2014
                //this on after security approval status. becuase if security is approved then show data missing staus other wise it show Unapproved
                if (!String.IsNullOrEmpty(positionMaster.SideTagValue))
                {
                    positionMaster.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionMaster.SideTagValue);
                }
                else
                {
                    positionMaster.Side = string.Empty;
                }

                if (!String.IsNullOrEmpty(positionMaster.AccountName))
                {
                    positionMaster.AccountID = CachedDataManager.GetInstance.GetAccountID(positionMaster.AccountName.Trim());
                }
                if (!String.IsNullOrEmpty(positionMaster.Strategy))
                {
                    positionMaster.StrategyID = CachedDataManager.GetInstance.GetStrategyID(positionMaster.Strategy.Trim());
                }
                if (positionMaster.CounterPartyID > 0)
                {
                    positionMaster.ExecutingBroker = CachedDataManager.GetInstance.GetCounterPartyText(positionMaster.CounterPartyID);
                }


                if (Regex.IsMatch(positionMaster.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                {
                    if (positionMaster.ValidationError.Length > "Account Lock Required".Length)
                    {
                        List<string> valerror = positionMaster.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                        valerror.Remove("Account Lock Required");
                        positionMaster.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                    }
                    else
                    {
                        positionMaster.ValidationError = string.Empty;
                        positionMaster.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                    }
                }
                else if (Regex.IsMatch(positionMaster.ValidationError, "Account Mapping Required", RegexOptions.IgnoreCase))
                {
                    if (positionMaster.ValidationError.Length > "Account Mapping Required".Length)
                    {
                        List<string> valerror = positionMaster.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                        valerror.Remove("Account Mapping Required");
                        positionMaster.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                    }
                    else
                    {
                        positionMaster.ValidationError = string.Empty;
                        positionMaster.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                    }
                }

                // Purpose : check valid Position startDate when it is not null or empty
                DateTime positionDate;
                bool isDateParsedSuccessfully = false;
                isDateParsedSuccessfully = DateTime.TryParse(positionMaster.PositionStartDate, out positionDate);
                if (!isDateParsedSuccessfully)
                {
                    if (!Regex.IsMatch(positionMaster.ValidationError, "Invalid Date Format", RegexOptions.IgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(positionMaster.ValidationError))
                            positionMaster.ValidationError += Seperators.SEPERATOR_8;
                        positionMaster.ValidationError += " Invalid Date Format";
                    }
                    positionMaster.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
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

            return positionMaster;
        }

        private void UpdatePositionMasterAUECandSettlementDate(PositionMaster positionMaster)
        {
            try
            {

                DateTime dt;
                bool isDateParsedSuccessfully = false;
                isDateParsedSuccessfully = DateTime.TryParse(positionMaster.PositionStartDate, out dt);
                if (isDateParsedSuccessfully)
                {
                    dt = Convert.ToDateTime(positionMaster.PositionStartDate);
                    positionMaster.AUECLocalDate = dt.ToString(Constants.DATEFORMAT);

                    if (!string.IsNullOrEmpty(positionMaster.PositionSettlementDate))
                    {
                        //this is special handling of date when we get date from excel file in number format
                        string[] splitDateFieldSlash = positionMaster.PositionSettlementDate.Split('/');
                        if (splitDateFieldSlash.Length == 1)
                        {
                            string[] splitDateFieldWithDash = positionMaster.PositionSettlementDate.Split('-');
                            if (splitDateFieldWithDash.Length == 1)
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(positionMaster.PositionSettlementDate, out result);
                                if (blnIsTrue)
                                {
                                    DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionSettlementDate));
                                    positionMaster.PositionSettlementDate = dtn.ToString(Constants.DATEFORMAT);
                                }
                            }
                        }
                    }
                    else
                    {
                        //DateTime dt = TimeZoneHelper.GetAUECLocalDateFromUTC(positionMaster.AUECID, Convert.ToDateTime(positionMaster.PositionStartDate));

                        int auecSettlementPeriod = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECSettlementPeriod(positionMaster.AUECID, positionMaster.SideTagValue);
                        DateTime positionSettlementDate = DateTimeConstants.MinValue;
                        if (auecSettlementPeriod == 0)
                        {
                            positionSettlementDate = Convert.ToDateTime(positionMaster.PositionStartDate);
                        }
                        else
                        {
                            positionSettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(Convert.ToDateTime(positionMaster.PositionStartDate), auecSettlementPeriod, positionMaster.AUECID); ;
                        }
                        positionMaster.PositionSettlementDate = positionSettlementDate.ToShortDateString();
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
        #endregion

        private static object _locker = new Object();

        void bgwImportData_DoWork(object sender, DoWorkEventArgs e)
        {
            List<PositionMaster> positionsNotImported = new List<PositionMaster>();
            try
            {
                lock (_locker)
                {
                    bool isValidToTrade = validateTradeForAccountNAVLock();

                    if (isValidToTrade)
                    {
                        //chunkSize to Import data..
                        int chunkSize = int.Parse(ConfigurationManager.AppSettings["ImportChunkSize"]);
                        chunkSize = (chunkSize >= MAXCHUNKSIZE) ? MAXCHUNKSIZE : chunkSize;

                        int recordsProcessed = 0;
                        int count = 0;
                        bool shouldReturn = false;

                        List<List<PositionMaster>> positionChunks = ChunkingManager.CreateChunks<PositionMaster>(_validatedPositionMasterCollection, chunkSize);

                        foreach (List<PositionMaster> positionChunk in positionChunks)
                        {
                            if (positionChunk.Count < chunkSize)
                            {
                                chunkSize = positionChunk.Count;
                            }
                            string key = recordsProcessed + Seperators.SEPERATOR_7 + (recordsProcessed + chunkSize);
                            this.IsGroupSaved = false;
                            try
                            {
                                if (_bgwImportData.CancellationPending)
                                {
                                    e.Cancel = true;
                                    _recordsProcessed = recordsProcessed;
                                    AddPositions(ref positionsNotImported, positionChunks, ref count);
                                    positionChunks.Clear();
                                    WritePositionsToFile(positionsNotImported);
                                    if (WorkCompleted != null)
                                    {
                                        WorkCompleted(this, null);
                                    }
                                    return;
                                }

                                string message = "Processing records from " + key;
                                int rowsUpdated = int.MinValue;
                                ShowProgress(message, false, Progress.Running);

                                try
                                {

                                    rowsUpdated = ServiceManager.Instance.AllocationServices.InnerChannel.CreateAndSavePositionsFromImport(positionChunk).Result;
                                    count++;
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                    shouldReturn = true;

                                    //All the trades of positionchunk that are not imported in application due to error, change their status   
                                    foreach (PositionMaster item in positionChunk)
                                    {
                                        item.ImportStatus = ImportStatus.ImportError.ToString();
                                    }

                                    AddPositions(ref positionsNotImported, positionChunks, ref count);
                                    positionChunks.Clear();
                                }
                                if (shouldReturn)
                                    break;

                                //check to verify whether all records have been successfully saved.
                                if (rowsUpdated.Equals(positionChunk.Count) || this.IsGroupSaved)
                                {
                                    recordsProcessed += rowsUpdated;
                                    ShowProgress(recordsProcessed.ToString(), true, Progress.Running);
                                }

                                //All the trades that are imported in application change their status   
                                foreach (PositionMaster item in positionChunk)
                                {
                                    item.ImportStatus = "Imported";
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                shouldReturn = true;

                                //All the trades of positionchunk that are not imported in application due to error, change their status   
                                foreach (PositionMaster item in positionChunk)
                                {
                                    item.ImportStatus = "Needs to check whether imported in application. Application error";
                                }

                                AddPositions(ref positionsNotImported, positionChunks, ref count);
                                positionChunks.Clear();
                            }
                            if (shouldReturn)
                                break;
                        }


                        if (positionsNotImported != null && positionsNotImported.Count > 0)
                        {
                            e.Result = WritePositionsToFile(positionsNotImported);
                        }
                        else
                        {
                            e.Result = Prana.Import.Constants.ImportCompletionStatus.Success;
                            _recordsProcessed = recordsProcessed;
                            positionChunks.Clear();
                        }
                    }
                    else
                    {
                        e.Result = Prana.Import.Constants.ImportCompletionStatus.ImportError;
                        _recordsProcessed = 0;

                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgwImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SendFailedTradesByMail(false);
                bool isPartialSuccess = false;
                int positionMasterCount = int.MinValue;

                if (_positionMasterCollection != null)
                {
                    positionMasterCount = _positionMasterCollection.Count;
                }
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else
                {
                    if (e.Cancelled)
                    {
                        _positionMasterCollection = null;
                    }
                    else
                    {
                        String message = e.Result.ToString() as string;
                        //data to be saved in file after trades is imported.
                        //this will be true only when importing.
                        //for re-run upload & re-run Symbol validation it will be false.
                        if (_isSaveDataInApplication)
                        {

                            if (_positionMasterCollection != null)
                            {
                                //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";
                                DataTable dtPositionMasterCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.GetDataTableFromList(_positionMasterCollection);
                                UpdateImportDataXML(dtPositionMasterCollection);

                                int importedTradesCount = dtPositionMasterCollection.Select("ImportStatus ='Imported'").Length;
                                if (importedTradesCount != dtPositionMasterCollection.Rows.Count)
                                {
                                    isPartialSuccess = true;
                                }
                            }

                        }

                        if (!String.IsNullOrEmpty(message) && (message != Prana.Import.Constants.ImportCompletionStatus.Success.ToString()) && !_recordsProcessed.Equals(_positionMasterCollection.Count))
                        {
                            StringBuilder boxMessage = new StringBuilder();
                            boxMessage.AppendLine("Some positions/transactions could not be imported. Kindly reimport from Reimport tab!");
                            MessageBox.Show(boxMessage.ToString(), "Information");
                        }
                        else
                        {
                            if (_isReimporting && File.Exists(_filePath))
                            {
                                File.Delete(_filePath);
                                if (ReImportCompleted != null)
                                {
                                    ReImportCompleted(this, null);
                                }
                            }

                            SetRecordsImported();
                            _positionMasterCollection = null;
                        }
                    }
                }
                string resultofValidation = e.Result.ToString();
                if (positionMasterCount == _validatedPositionMasterCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", CONST_Success, null);
                }
                else if ((positionMasterCount != _validatedPositionMasterCollection.Count || isPartialSuccess) && resultofValidation == "Success")
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", CONST_PartialSuccess, null);
                }
                else
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", CONST_Failure, null);
                }

                #region updateLast ExecutionDate
                if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("ImportStatus"))
                {
                    if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary["ImportStatus"].ToString().Contains(CONST_Success))
                    {
                        string filePath = string.Empty;
                        if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("Task"))
                            filePath = Application.StartupPath + @"\DashBoardData\Import\" + _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["Task"].ToString() + ".xml";
                        DataSet ds = new DataSet();
                        if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("ExecutionDate"))
                        {
                            string strExecutionDate = _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["ExecutionDate"].ToString();
                            DateTime executionDate = DateTime.MinValue;
                            if (!DateTime.TryParseExact(strExecutionDate, ApplicationConstants.DateFormat,
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out executionDate))
                            {
                                executionDate = _runUpload.BatchStartDate;
                            }
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1215
                            bool isFileCorrupt = true;
                            DateTime lastExeutionDate = DateTime.MinValue;
                            if (File.Exists(filePath))
                            {
                                //modified by omshiv, reading XML using XmlTextWriter
                                using (FileStream filestream = File.OpenRead(filePath))
                                {
                                    BufferedStream buffered = new BufferedStream(filestream);
                                    ds.ReadXml(buffered);
                                }
                                //ds.ReadXml(filePath);
                                //Modified To check if file is corrupt or not
                                if (ds != null && ds.Tables.Count > 0 &&
                                    ds.Tables[0].Columns.Contains("LastExecutionDate") &&
                                    DateTime.TryParseExact(ds.Tables[0].Rows[0]["LastExecutionDate"].ToString(), ApplicationConstants.DateFormat,
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out lastExeutionDate))
                                {
                                    TimeSpan t = executionDate - lastExeutionDate;
                                    isFileCorrupt = false;
                                    if (t.Ticks > 0)
                                    {
                                        ds.Tables[0].Rows[0]["LastExecutionDate"] = executionDate.ToString(ApplicationConstants.DateFormat);
                                        //ds.Tables[0].WriteXml(filePath);
                                        isFileCorrupt = true;
                                    }
                                }
                            }
                            if (isFileCorrupt)
                            {
                                ds = new DataSet();
                                ds.Tables.Add("ExecutionDate");
                                ds.Tables[0].Columns.Add("LastExecutionDate");
                                ds.Tables[0].Rows.Add(executionDate.ToString(ApplicationConstants.DateFormat));
                                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                                }
                                using (XmlTextWriter xmlWriter = new XmlTextWriter(filePath, Encoding.UTF8))
                                {
                                    ds.WriteXml(xmlWriter);
                                }
                                // ds.WriteXml(filePath);
                            }
                        }
                    }
                }
                #endregion
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
                UnwireEvents();
                if (TaskSpecificDataPointsPreUpdate != null && _currentResult.ExecutionInfo.TaskInfo != null)
                {
                    //Return TaskResult which was recieved from ImportManager as event argument
                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                    TaskSpecificDataPointsPreUpdate = null;
                }

            }
        }

        /// <summary>
        /// To update Import data xml file
        /// </summary>
        /// <param name="dtPositionMasterCollection"></param>
        private void UpdateImportDataXML(DataTable dtPositionMasterCollection)
        {
            try
            {
                bool isImportDataXMLUpdated = false;
                dtPositionMasterCollection.TableName = "ImportData";
                //if (_isSaveDataInApplication)
                //{
                if (dtPositionMasterCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    //create primary key fr datatable
                    columns[0] = dtPositionMasterCollection.Columns["RowIndex"];
                    dtPositionMasterCollection.PrimaryKey = columns;
                }
                //if there is already a file then read from it which trades are already imported so that previously imported trades are not set to unImported after file is written again.
                if (File.Exists(Application.StartupPath + _totalImportDataXmlFilePath))
                {
                    DataSet ds = new DataSet();

                    //modified by omshiv, read xml using buffered stream
                    String filePath = Application.StartupPath + _totalImportDataXmlFilePath;


                    using (FileStream filestream = File.OpenRead(filePath))
                    {
                        BufferedStream buffered = new BufferedStream(filestream);
                        ds.ReadXml(buffered);
                    }

                    // ds.ReadXml(Application.StartupPath + _totalImportDataXmlFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        // TODO : For now there were conflict error while merging the table with the column so it is removed.
                        if (dtPositionMasterCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtPositionMasterCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        _importedTradesCount = ds.Tables[0].Select("ImportStatus ='Imported'").Length;
                        if (_importedTradesCount > 0 && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("IsReRunSymbolValidation") && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["IsReRunSymbolValidation"].ToString().Equals("True"))
                        {
                            if (ds.Tables[0].Columns.Contains("RowIndex"))
                            {
                                DataColumn[] columns = new DataColumn[1];
                                columns[0] = ds.Tables[0].Columns["RowIndex"];
                                ds.Tables[0].PrimaryKey = columns;
                            }
                            ds.Tables[0].Merge(dtPositionMasterCollection, true, MissingSchemaAction.Ignore);
                            using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                            {
                                ds.WriteXml(xmlWrite);
                            }
                            isImportDataXMLUpdated = true;
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRunSymbolValidation", false, null);
                        }
                        else
                        {
                            dtPositionMasterCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }
                }
                //}
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _positionMasterCollection.Count, _totalImportDataXmlFilePath);
                if (!isImportDataXMLUpdated)
                {
                    try
                    {
                        using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _totalImportDataXmlFilePath, Encoding.UTF8))
                        {
                            dtPositionMasterCollection.WriteXml(xmlWrite);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "Process Failed", null);
                        _currentResult.Error = new Exception("The process cannot access the file because it is being used by another process.");
                    }
                }
                // dtPositionMasterCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);

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


        private bool validateTradeForAccountNAVLock()
        {
            bool isProcessToSave = true;
            try
            {
                #region NAV lock validation - modified by Omshiv, MArch 2014
                //get IsNAVLockingEnabled or not from cache
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (PositionMaster position in _positionMasterCollection)
                    {


                        //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                        if (!string.IsNullOrEmpty(position.AccountName))
                        {
                            DateTime tradeDate = DateTime.MinValue;
                            if (DateTime.TryParse(position.PositionStartDate, out tradeDate))
                            {
                                bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(position.AccountID, tradeDate);
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

        private void AddPositions(ref List<PositionMaster> positionsNotImported, List<List<PositionMaster>> positionChunks, ref int count)
        {
            try
            {
                if (this.IsGroupSaved)
                    count++;

                List<List<PositionMaster>> positionsToBeDumped = positionChunks.GetRange(count, positionChunks.Count - count);
                foreach (List<PositionMaster> positionList in positionsToBeDumped)
                {
                    positionsNotImported.AddRange(positionList);
                }
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

        private object WritePositionsToFile(List<PositionMaster> positionsNotImported)
        {
            object result = new object();
            //TODO: Directory Path is hardcoded here
            //string directoryPath = _ctrlImportPreferences.ImportPrefs.DirectoryPath;
            string directoryPath = @"c:\";
            if (directoryPath.Equals(string.Empty))
            {
                directoryPath = System.Windows.Forms.Application.StartupPath;
            }
            string path = directoryPath + @"\Import Data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string date = DateTime.Now.ToString("MM-dd-yyyy", DateTimeFormatInfo.InvariantInfo);
                string dirPath = path + @"\" + date;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                StringBuilder filePath = new StringBuilder();
                filePath.Append(dirPath);
                filePath.Append(@"\");
                filePath.Append(GetCurrentTimeStamp());
                filePath.Append(Seperators.SEPERATOR_13);
                filePath.Append(positionsNotImported[0].UserID);
                filePath.Append(Seperators.SEPERATOR_13);

                if (_filePath.Equals(string.Empty))
                {
                    //string fileName = gridRunUpload.Rows[_rowIndex].Cells[CAPTION_FilePath].Value.ToString();
                    string fileName = string.Empty;
                    string exactFile = fileName.Substring(fileName.LastIndexOf(@"\"));

                    if (exactFile.Contains("."))
                        filePath.Append(exactFile.Substring(1, exactFile.IndexOf('.') - 1));
                    else
                        filePath.Append(exactFile);
                }
                else
                {
                    string file = _filePath.Substring(_filePath.LastIndexOf(@"\") + 1);
                    string exactFile = file.Substring(0, file.IndexOf("."));
                    string[] seperators = new string[] { Seperators.SEPERATOR_13 };
                    string[] fileSubstring = exactFile.Split(seperators, StringSplitOptions.None);
                    StringBuilder fileName = new StringBuilder();
                    for (int i = 2; i <= fileSubstring.Length - 1; i++)
                    {
                        fileName.Append(fileSubstring[i]);
                        fileName.Append(Seperators.SEPERATOR_13);
                    }

                    filePath.Append(fileName.ToString().Substring(0, fileName.Length - 1));

                }

                CSVFileFormatter formatter = new CSVFileFormatter();
                formatter.CreateFile(positionsNotImported, null, filePath.ToString() + ".csv", null);
                result = Prana.Import.Constants.ImportCompletionStatus.ImportError;
            }
            catch (Exception ex)
            {
                result = Prana.Import.Constants.ImportCompletionStatus.FileWriteError;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private string GetCurrentTimeStamp()
        {
            return DateTime.Now.ToLocalTime().ToString("MMddyyyyhhmmss", DateTimeFormatInfo.InvariantInfo);
        }

        private void ShowProgress(string message, bool addMessage, Progress progress)
        {
            try
            {
                //TODO: Following code commented, need to find a way to update progress
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

        private void SetRecordsImported()
        {
            try
            {
                if (!_recordsProcessed.Equals(int.MinValue) && !_rowIndex.Equals(int.MinValue))
                {
                    if (_recordsProcessed > 0)
                    {
                        //TODO: Uncomment following code

                        //gridRunUpload.Rows[_rowIndex].Cells[CAPTION_NumberOfRecords].Value = _recordsProcessed;
                        //gridRunUpload.Rows[_rowIndex].Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                    }
                }
                _recordsProcessed = int.MinValue;
            }
            catch (Exception ex)
            {
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
                SetReImportVariables(string.Empty, false);

                if (ProgressEvent != null)
                {
                    ProgressEvent(this, new EventArgs<string, bool, Progress>(string.Empty, false, Progress.End));
                }
                _timerSecurityValidation.Elapsed -= new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                _bgwImportData.DoWork -= new DoWorkEventHandler(bgwImportData_DoWork);
                _bgwImportData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
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

        private void SetReImportVariables(string filePath, bool isReimporting)
        {
            _isReimporting = isReimporting;
            _filePath = filePath;
        }

        #region IImportHandler Members

        /// <summary>
        /// Update validation staus for the symbols which have passed price validation
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
                    DataTable dt1 = WindsorContainerManager.FetchMarkPricesAccountWiseForLastBusinessDay();
                    Dictionary<string, string> dictColumnsToKey = new Dictionary<string, string>();

                    dictColumnsToKey.Add("FundName", "FundName");
                    dictColumnsToKey.Add("Symbol", "Symbol");


                    List<string> lstColumnsToReconcile = _runUpload.PriceToleranceColumns.Split(',').ToList();

                    DATAReconciler.RecocileData(dt1, ds.Tables[0], lstColumnsToReconcile, dictColumnsToKey, _runUpload.PriceTolerance, ComparisionType.Numeric);
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



        public void ValidatePriceTolerances(DataTable dtPositionMasterCollection)
        {

            try
            {

                Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>();
                List<string> lstColumnsToKey = new List<string>();
                #region price tolerance check
                if (_runUpload.IsPriceToleranceChecked)
                {
                    //select distict account and symbol
                    var Test = _positionMasterCollection
                            .Select(positionMaster => new { positionMaster.AccountID, positionMaster.Symbol })
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
                    dtLastBusinessDayMarkPrices.Columns.Add("AccountID", typeof(int));
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

                        DATAReconciler.RecocileData(dtPositionMasterCollection, dtLastBusinessDayMarkPrices, lstColumnsToKey, dictColumnsToReconcile, _runUpload.PriceTolerance, ComparisionType.Numeric);
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

        #endregion
        /// <summary>
        /// Wires the Event
        /// </summary>
        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(SecurityMaster_SecMstrDataResponse);
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerSecurityValidation_Tick);
                _bgwImportData.DoWork += new DoWorkEventHandler(bgwImportData_DoWork);
                _bgwImportData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
                _bgwImportData.WorkerSupportsCancellation = true;
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

        private static object _timerLock = new Object();

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
            bool isSavingData = false;
            try
            {
                lock (_timerLock)
                {
                    //string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedTaxlots" + ".xml";
                    //string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedTaxlots" + ".xml";
                    //string validatedSymbolsXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedSymbols" + ".xml";
                    //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";


                    if (_positionMasterCollection != null)
                    {
                        DataTable dtPositionMasterCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.GetDataTableFromList(_positionMasterCollection);
                        ValidatePriceTolerances(dtPositionMasterCollection);

                        #region write xml for non validated trades
                        int countNonValidatedtaxlots = _positionMasterCollection.Count(positionMaster => positionMaster.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedTaxlots", countNonValidatedtaxlots, null);

                        #endregion

                        #region write xml for validated trades
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedTaxlots", _validatedPositionMasterCollection.Count, null);
                        #endregion

                        #region write xml for validated symbols

                        //DataTable dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(_secMasterResponseCollection);
                        DataTable dtValidatedSymbols = SecMasterHelper.getInstance().ConvertSecMasterBaseObjCollectionToUIObjDataTable(_secMasterResponseCollection);

                        // ConvertSecMasterBaseObjToUIObj(dtValidatedSymbols);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TotalSymbols", _countSymbols, null);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedSymbols", _countValidatedSymbols, _validatedSymbolsXmlFilePath);
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedSymbols", _countNonValidatedSymbols, null);

                        if (dtValidatedSymbols != null)
                        {
                            //SecMasterHelper.getInstance().AddNotExistSecuritiesToSecMasterCollection(dtValidatedSymbols, _dictRequestedSymbol);
                        }
                        #endregion

                        #region symbol validation and total taxlots added to task statistics
                        //dtValidatedSymbols
                        //DataTable dtPositionMasterCollection = GeneralUtilities.GetDataTableFromList(_positionMasterCollection);

                        if (dtPositionMasterCollection != null && dtPositionMasterCollection.Rows.Count > 0)
                        {
                            //data to be saved in file after trades is imported.
                            //this will be true only when importing.
                            //for re-run upload & re-reun Symbol validation it will be false.
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1124
                            // this check is removed as if import is not done due to some error then error must be updated in file. so file must be written bothh the times.
                            // here as well as at bgwImportData_RunWorkerCompleted event of importing
                            // if (!_isSaveDataInApplication)
                            //{
                            UpdateImportDataXML(dtPositionMasterCollection);
                            //}
                        }
                        //Fields that are to be written to be first time should be checked
                        int symbolsPendingApprovalCounts = int.MinValue;
                        //Using Nullable int to allow null values returned by Linq Query
                        int? accountCount = int.MinValue;

                        if (dtValidatedSymbols != null)
                        {
                            //ValidateSymbols(dtPositionMasterCollection, ref dtValidatedSymbols);
                            dtValidatedSymbols.TableName = "ValidatedSymbols";

                            using (XmlTextWriter xmlWrite = new XmlTextWriter(Application.StartupPath + _validatedSymbolsXmlFilePath, Encoding.UTF8))
                            {
                                dtValidatedSymbols.WriteXml(xmlWrite);
                            }
                            // dtValidatedSymbols.WriteXml(Application.StartupPath + _validatedSymbolsXmlFilePath, false);
                        }
                        #endregion


                        #region add dashboard statistics data
                        if (accountCount == int.MinValue)
                        {
                            accountCount = _positionMasterCollection.Select(positionMaster => positionMaster.AccountName).Distinct().Count();
                            if (accountCount == null)
                            {
                                accountCount = 0;
                            }
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCount, null);

                            var names = _positionMasterCollection.Select(positionMaster => positionMaster.AccountName).Distinct().FirstOrDefault();
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Accounts", names, names);
                        }
                        if (symbolsPendingApprovalCounts == int.MinValue)
                        {
                            symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);
                        }
                        #endregion

                        // Purpose : To update symbol validation status if validatedSymbols Xml not exist and trades not partially imported.
                        if (_importedTradesCount == 0 && !_isSaveDataInApplication)
                        {
                            UpdateSymbolValidationStatus();
                        }
                        if (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("SymbolValidation") && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["SymbolValidation"].ToString() == string.Empty)
                        {
                            UpdateSymbolValidationStatus();
                        }
                        //Validated PositionMaster objects will be added to _validatedPositionMasterCollections
                        //Save data in application where _isSaveDataInApplication is true
                        if (_isSaveDataInApplication)
                        {
                            if (_validatedPositionMasterCollection.Count == 0)
                            {
                                if (_importedTradesCount == 0)
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", CONST_Failure, null);
                                }
                                if (TaskSpecificDataPointsPreUpdate != null && _currentResult.ExecutionInfo.TaskInfo != null)
                                {
                                    TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                    TaskSpecificDataPointsPreUpdate = null;
                                }
                                UnwireEvents();
                            }
                            else
                            {
                                isSavingData = true;
                                if (!_bgwImportData.IsBusy)
                                {
                                    _bgwImportData.RunWorkerAsync();
                                }
                                else
                                {
                                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", CONST_Failure, null);
                                }
                            }
                        }
                        else
                        {
                            if (TaskSpecificDataPointsPreUpdate != null)
                            {
                                if (!_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("SymbolValidation") || (_currentResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("SymbolValidation") && _currentResult.TaskStatistics.TaskSpecificData.AsDictionary["SymbolValidation"].ToString() == string.Empty))
                                {
                                    if (_countSymbols == _countValidatedSymbols && _countSymbols != 0)
                                    {
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_Success, null);
                                    }
                                    else if (_countValidatedSymbols == 0)
                                    {
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_Failure, null);
                                    }
                                    else
                                    {
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_PartialSuccess, null);
                                    }
                                }
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
                if (!isSavingData)
                    SendFailedTradesByMail(true);
            }
        }

        /// <summary>
        /// Sends the failed trades by mail.
        /// </summary>
        /// <param name="sendAllTrades">if set to <c>true</c> [send all trades].</param>
        private void SendFailedTradesByMail(bool sendAllTrades)
        {
            try
            {
                if (sendAllTrades)
                {
                    FTPTradeEmailHelper.SendErrorEmail(_runUpload.ProcessedFilePath);
                }
                else
                {
                    object data = _currentResult.TaskStatistics.TaskSpecificData.GetRefValueForKey("OriginalData");
                    if (data != null)
                    {
                        DataTable dt = (DataTable)data;
                        int i = _positionMasterCollection.Count >= dt.Rows.Count ? 0 : 1;
                        for (int j = _positionMasterCollection.Count - 1; j >= 0; j--)
                        {
                            PositionMaster pos = _positionMasterCollection[j];
                            if (pos.ImportStatus == "Imported")
                                dt.Rows.RemoveAt(i + j);

                        }
                        dt.AcceptChanges();
                        string filePath = Path.Combine(Path.GetDirectoryName(_runUpload.ProcessedFilePath)
                        , "Failed_" + Path.GetFileName(_runUpload.ProcessedFilePath));
                        CSVFileHealper.ProduceCSV(dt, null, filePath, false);

                        FTPTradeEmailHelper.SendErrorEmail(filePath);
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
        /// To update symbol validation status
        /// </summary>
        private void UpdateSymbolValidationStatus()
        {
            try
            {
                if (_countSymbols == _countValidatedSymbols && _countSymbols != 0)
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_Success, null);
                }
                else if (_countValidatedSymbols == 0)
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_Failure, null);
                }
                else
                {
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", CONST_PartialSuccess, null);
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



        private void ValidateSymbols(DataTable dtPositionMasterCollection, ref DataTable dtValidatedSymbols)
        {
            try
            {
                List<SecMasterUIObj> lstSecMasterBaseObj = SecMasterHelper.getInstance().ConvertSecMasterBaseObjDataTableToUIObjCollection(dtValidatedSymbols);
                dtValidatedSymbols = Prana.Utilities.MiscUtilities.GeneralUtilities.GetDataTableFromList(lstSecMasterBaseObj);

                Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>();

                //todo: in current structure all the requested symbol are also set to TickerSymbol
                //e.g. CUSIP is set to TickerSymbol, Bloomberg set to TickerSymbol
                //So key to match column is Symbol
                //TODO: Make generic this mapping
                dictColumnsToReconcile.Add("Symbol", "TickerSymbol");
                dictColumnsToReconcile.Add("CurrencyID", "CurrencyID");
                dictColumnsToReconcile.Add("Bloomberg", "BloombergSymbol");
                dictColumnsToReconcile.Add("SEDOL", "SedolSymbol");
                dictColumnsToReconcile.Add("ISIN", "ISINSymbol");
                dictColumnsToReconcile.Add("RIC", "ReutersSymbol");
                dictColumnsToReconcile.Add("CUSIP", "CusipSymbol");
                dictColumnsToReconcile.Add("Multiplier", "Multiplier");
                List<string> lstColumnsToKey = new List<string>();
                lstColumnsToKey.Add("Symbol");

                DATAReconciler.RecocileData(dtPositionMasterCollection, dtValidatedSymbols, lstColumnsToKey, dictColumnsToReconcile, _runUpload.PriceTolerance, ComparisionType.Exact);



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
        /// Resets the timer
        /// </summary>
        private void ResetTimer()
        {
            try
            {
                // Modified By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3635
                lock (_lock)
                {
                    //modified by sachin mishra Purpose: Jira-CHMW-3318 
                    if (_timerSecurityValidation != null)
                    {
                        _timerSecurityValidation.Stop();
                        _timerSecurityValidation.Start();
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
        /// To save imported file details in DB to check duplicate trades for import
        /// </summary>
        /// <param name="runUpload"></param>
        private int SaveImportedFileDetails(RunUpload runUpload)
        {
            try
            {
                _importedFileId = ImportDataManager.SaveImportedFileDetails(runUpload.RawFilePath, runUpload.FilePath, runUpload.ImportTypeAcronym, runUpload.FileLastModifiedUTCTime);
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
            return _importedFileId;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (_timerSecurityValidation != null)
            {
                _timerSecurityValidation.Dispose();
            }
            if (_bgwImportData != null)
            {
                _bgwImportData.Dispose();
            }
            if (_runUpload != null)
            {
                _runUpload.Dispose();
            }
        }

        #endregion
    }
}