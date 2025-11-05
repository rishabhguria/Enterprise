using Prana.BusinessObjects;
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
using System.Windows.Forms;

namespace Prana.Import
{
    sealed public class ActivitiesHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        #region local variables
        const string DATEFORMAT = "MM/dd/yyyy";

        string _executionName;
        string _dashboardXmlDirectoryPath;
        string _refDataDirectoryPath;
        TaskResult _currentResult = new TaskResult();
        private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);

        public event EventHandler TaskSpecificDataPointsPreUpdate;
        List<DividendImport> _dividendValueCollection = new List<DividendImport>();
        List<DividendImport> _validatedDividendValueCollection = new List<DividendImport>();
        Dictionary<int, Dictionary<string, List<DividendImport>>> _dividendSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DividendImport>>>();
        #endregion
        #region singleton

        private static volatile ActivitiesHandler instance;
        private static object syncRoot = new Object();

        public ActivitiesHandler() { }

        public static ActivitiesHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ActivitiesHandler();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion



        #region IImportHandler Members
        /// <summary>
        /// Processes Request
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                WireEvents();
                _currentResult = taskResult as TaskResult;
                //As discussed with sandeep ji, it must be checked for Netposition/ Transactions and DividendValue import
                SecurityMasterManager.Instance.GenerateSMMapping(ds);

                _executionName = Path.GetFileName(_currentResult.GetDashBoardXmlPath());
                _dashboardXmlDirectoryPath = Path.GetDirectoryName(_currentResult.GetDashBoardXmlPath());
                if (!Directory.Exists(Application.StartupPath + _dashboardXmlDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + _dashboardXmlDirectoryPath);
                }

                _refDataDirectoryPath = _dashboardXmlDirectoryPath + @"\RefData";
                if (!Directory.Exists(Application.StartupPath + _refDataDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + _refDataDirectoryPath);
                }

                //if (runUpload.IsPriceToleranceChecked)
                //{
                //    DataTable dt = ValidatePriceTolerance(ds);

                //    string validatedPriceToleranceXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_PriceValidatedDividendPrices" + ".xml";
                //    string nonValidatedPriceToleranceXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_PriceNonValidatedDividendPrices" + ".xml";

                //    dt.WriteXml(Application.StartupPath + validatedPriceToleranceXmlFilePath);
                //    ds.Tables[0].WriteXml(Application.StartupPath + nonValidatedPriceToleranceXmlFilePath);

                //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PriceValidatedDividendPrices", dt.Rows.Count, validatedPriceToleranceXmlFilePath);
                //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PriceNonValidatedDividendPrices", ds.Tables[0].Rows.Count, nonValidatedPriceToleranceXmlFilePath);

                //    //Price tolrerance validated for all DividendValue
                //    if (ds.Tables[0].Rows.Count == 0)
                //    {
                //        ds.Tables.Clear();
                //        ds.Tables.Add(dt);
                //        SetCollection(ds);
                //        GetSMDataForDividendImport();
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
                SetCollection(ds);
                GetSMDataForDividendImport();
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
        private void SaveActivities()
        {
            try
            {
                // insert positions into the DB

                // total number of records inserted                                                   
                DataSet dsInserted = GeneralUtilities.CreateDataSetFromCollection(_dividendValueCollection, null);

                if (dsInserted != null && dsInserted.Tables[0].Rows.Count > 0)
                {
                    dsInserted = ServiceManager.Instance.CashManagementServices.InnerChannel.SaveManualCashDividend(dsInserted);
                }
                else
                {
                    //MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //}
                //else
                //{
                //    //MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
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

                if (_dividendSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                {
                    Dictionary<string, List<DividendImport>> dictSymbols = _dividendSymbologyWiseDict[requestedSymbologyID];
                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                    {
                        List<DividendImport> listDividend = dictSymbols[secMasterObj.RequestedSymbol];
                        foreach (DividendImport dividendImport in listDividend)
                        {
                            dividendImport.Symbol = pranaSymbol;
                            dividendImport.CUSIP = cuspiSymbol;
                            dividendImport.ISIN = isinSymbol;
                            dividendImport.SEDOL = sedolSymbol;
                            dividendImport.Bloomberg = bloombergSymbol;
                            dividendImport.RIC = reutersSymbol;
                            dividendImport.OSIOptionSymbol = osiOptionSymbol;
                            dividendImport.IDCOOptionSymbol = idcoOptionSymbol;
                            dividendImport.AUECID = secMasterObj.AUECID;
                            dividendImport.OSIOptionSymbol = osiOptionSymbol;
                            dividendImport.IDCOOptionSymbol = idcoOptionSymbol;
                            dividendImport.OpraOptionSymbol = opraOptionSymbol;
                            if (dividendImport.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                            {
                                _validatedDividendValueCollection.Add(dividendImport);
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

        public string GetXSDName()
        {
            return "ImportActivities.xsd";
        }

        #endregion

        private void SetCollection(DataSet ds)
        {
            try
            {
                _dividendSymbologyWiseDict.Clear();
                _dividendValueCollection = new List<DividendImport>();
                _validatedDividendValueCollection = new List<DividendImport>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(DividendImport).ToString());
                DataTable dTable = ds.Tables[0];
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    DividendImport dividendObj = new DividendImport();
                    dividendObj.Symbol = string.Empty;
                    dividendObj.Amount = 0;
                    dividendObj.ExDate = string.Empty;
                    dividendObj.PayoutDate = string.Empty;
                    dividendObj.AUECID = 0;
                    dividendObj.AccountName = string.Empty;
                    dividendObj.FundID = 0;
                    dividendObj.ActivityTypeId = 0;
                    dividendObj.ActivityType = string.Empty;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(dividendObj, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(dividendObj, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                        }
                    }
                    if (dividendObj.UserId == 0)
                        dividendObj.UserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                    // Only blank Dates are updated from UI
                    //TODO: Handle user selected date    
                    //if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    //{
                    //    dividendObj.ExDate = _userSelectedDate;
                    //}
                    //else if (!string.IsNullOrEmpty(dividendObj.ExDate))
                    if (!string.IsNullOrEmpty(dividendObj.ExDate))
                    {  // ExDate Parsing
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.ExDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.ExDate));
                            dividendObj.ExDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.ExDate);
                            dividendObj.ExDate = dtn.ToString(DATEFORMAT);
                        }
                    }

                    // Only blank Dates are updated from UI

                    //TODO: Handle user selected date    
                    //if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    //{
                    //    dividendObj.PayoutDate = _userSelectedDate;
                    //}
                    // PayoutDate Parsing 
                    //else if (!string.IsNullOrEmpty(dividendObj.PayoutDate))
                    if (!string.IsNullOrEmpty(dividendObj.PayoutDate))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.PayoutDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.PayoutDate));
                            dividendObj.PayoutDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.PayoutDate);
                            dividendObj.PayoutDate = dtn.ToString(DATEFORMAT);
                        }
                    }

                    // Only blank Dates are updated from UI
                    //exdate and payout will be available only for dividends i.e. there should be symbol
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2629

                    //TODO: Handle user selected date    
                    //if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    //{
                    //    dividendObj.RecordDate = _userSelectedDate;
                    //}
                    //else if (!string.IsNullOrEmpty(dividendObj.RecordDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    if (!string.IsNullOrEmpty(dividendObj.RecordDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {  // RecordDate Parsing
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.RecordDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.RecordDate));
                            dividendObj.RecordDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.RecordDate);
                            dividendObj.RecordDate = dtn.ToString(DATEFORMAT);
                        }
                    }
                    // Only blank Dates are updated from UI
                    //exdate and payout will be available only for dividends i.e. there should be symbol
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2629

                    //TODO: Handle user selected date
                    //if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    //{
                    //    dividendObj.DeclarationDate = _userSelectedDate;
                    //}
                    // DeclarationDate Parsing 
                    // else if (!string.IsNullOrEmpty(dividendObj.DeclarationDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    if (!string.IsNullOrEmpty(dividendObj.DeclarationDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.DeclarationDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.DeclarationDate));
                            dividendObj.DeclarationDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.DeclarationDate);
                            dividendObj.DeclarationDate = dtn.ToString(DATEFORMAT);
                        }
                    }
                    //TODO: Handle user selected date
                    //if (_userSelectedAccountValue != int.MinValue)
                    //{
                    //    dividendObj.AccountID = _userSelectedAccountValue;
                    //    dividendObj.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    //}
                    //else if (!String.IsNullOrEmpty(dividendObj.AccountName))
                    if (!String.IsNullOrEmpty(dividendObj.AccountName))
                    {
                        dividendObj.FundID = CachedDataManager.GetInstance.GetAccountID(dividendObj.AccountName.Trim());
                    }
                    //Fill ActivityTypeId from cache based on ActivityType
                    if (dividendObj.ActivityTypeId <= 0)
                    {
                        dividendObj.ActivityTypeId = CachedDataManager.GetActivityTypeID(dividendObj.ActivityType.Trim());
                    }

                    // Key: 0 for Ticker
                    switch (dividendObj.Symbology)
                    {
                        //if (!String.IsNullOrEmpty(dividendObj.Symbol))
                        case "Symbol":
                            if (_dividendSymbologyWiseDict.ContainsKey(0))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[0];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.Symbol))
                                {
                                    List<DividendImport> dividendSymbolWiseList = dividendSameSymbologyDict[dividendObj.Symbol];
                                    dividendSymbolWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.Symbol] = dividendSymbolWiseList;
                                    _dividendSymbologyWiseDict[0] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[0].Add(dividendObj.Symbol, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameSymbolDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameSymbolDict.Add(dividendObj.Symbol, dividendlist);
                                _dividendSymbologyWiseDict.Add(0, dividendSameSymbolDict);
                            }
                            break;
                        // Key: 1 for RIC
                        //else if (!String.IsNullOrEmpty(dividendObj.RIC))
                        case "RIC":
                            if (_dividendSymbologyWiseDict.ContainsKey(1))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[1];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.RIC))
                                {
                                    List<DividendImport> dividendRICWiseList = dividendSameSymbologyDict[dividendObj.RIC];
                                    dividendRICWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.RIC] = dividendRICWiseList;
                                    _dividendSymbologyWiseDict[1] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendList = new List<DividendImport>();
                                    dividendList.Add(dividendObj);
                                    _dividendSymbologyWiseDict[1].Add(dividendObj.RIC, dividendList);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendList = new List<DividendImport>();
                                dividendList.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameRICDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameRICDict.Add(dividendObj.RIC, dividendList);
                                _dividendSymbologyWiseDict.Add(1, dividendSameRICDict);
                            }
                            break;
                        // Key: 2 for ISIN
                        //else if (!String.IsNullOrEmpty(dividendObj.ISIN))
                        case "ISIN":
                            if (_dividendSymbologyWiseDict.ContainsKey(2))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[2];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.ISIN))
                                {
                                    List<DividendImport> dividendISINWiseList = dividendSameSymbologyDict[dividendObj.ISIN];
                                    dividendISINWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.ISIN] = dividendISINWiseList;
                                    _dividendSymbologyWiseDict[2] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[2].Add(dividendObj.ISIN, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameISINDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameISINDict.Add(dividendObj.ISIN, dividendlist);
                                _dividendSymbologyWiseDict.Add(2, dividendSameISINDict);
                            }
                            break;
                        // Key: 3 for SEDOL
                        // else if (!String.IsNullOrEmpty(dividendObj.SEDOL))
                        case "SEDOL":
                            if (_dividendSymbologyWiseDict.ContainsKey(3))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[3];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.SEDOL))
                                {
                                    List<DividendImport> dividendSEDOLWiseList = dividendSameSymbologyDict[dividendObj.SEDOL];
                                    dividendSEDOLWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.SEDOL] = dividendSEDOLWiseList;
                                    _dividendSymbologyWiseDict[3] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[3].Add(dividendObj.SEDOL, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSEDOLDict = new Dictionary<string, List<DividendImport>>();
                                dividendSEDOLDict.Add(dividendObj.SEDOL, dividendlist);
                                _dividendSymbologyWiseDict.Add(3, dividendSEDOLDict);
                            }
                            break;
                        // Key: 4 for CUSIP
                        //else if (!String.IsNullOrEmpty(dividendObj.CUSIP))
                        case "CUSIP":
                            if (_dividendSymbologyWiseDict.ContainsKey(4))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[4];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.CUSIP))
                                {
                                    List<DividendImport> dividendCUSIPWiseList = dividendSameSymbologyDict[dividendObj.CUSIP];
                                    dividendCUSIPWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.CUSIP] = dividendCUSIPWiseList;
                                    _dividendSymbologyWiseDict[4] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[4].Add(dividendObj.CUSIP, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameCUSIPDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameCUSIPDict.Add(dividendObj.CUSIP, dividendlist);
                                _dividendSymbologyWiseDict.Add(4, dividendSameCUSIPDict);
                            }
                            break;
                        // Key: 5 for Bloomberg
                        // else if (!String.IsNullOrEmpty(dividendObj.Bloomberg))
                        case "Bloomberg":
                            if (_dividendSymbologyWiseDict.ContainsKey(5))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[5];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.Bloomberg))
                                {
                                    List<DividendImport> dividendBloombergWiseList = dividendSameSymbologyDict[dividendObj.Bloomberg];
                                    dividendBloombergWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.Bloomberg] = dividendBloombergWiseList;
                                    _dividendSymbologyWiseDict[5] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[5].Add(dividendObj.Bloomberg, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameBloombergDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameBloombergDict.Add(dividendObj.Bloomberg, dividendlist);
                                _dividendSymbologyWiseDict.Add(5, dividendSameBloombergDict);
                            }
                            break;
                        // Key: 6 for OSIOptionSymbol
                        //else if (!String.IsNullOrEmpty(dividendObj.OSIOptionSymbol))
                        case "OSIOptionSymbol":
                            if (_dividendSymbologyWiseDict.ContainsKey(6))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[6];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.OSIOptionSymbol))
                                {
                                    List<DividendImport> dividendOSIWiseList = dividendSameSymbologyDict[dividendObj.OSIOptionSymbol];
                                    dividendOSIWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.OSIOptionSymbol] = dividendOSIWiseList;
                                    _dividendSymbologyWiseDict[6] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[6].Add(dividendObj.OSIOptionSymbol, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameOSIDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameOSIDict.Add(dividendObj.OSIOptionSymbol, dividendlist);
                                _dividendSymbologyWiseDict.Add(6, dividendSameOSIDict);
                            }
                            break;
                        // Key: 7 for IDCOOptionSymbol
                        //else if (!String.IsNullOrEmpty(dividendObj.IDCOOptionSymbol))
                        case "IDCOOptionSymbol":
                            if (_dividendSymbologyWiseDict.ContainsKey(7))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[7];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.IDCOOptionSymbol))
                                {
                                    List<DividendImport> dividendIDCOWiseList = dividendSameSymbologyDict[dividendObj.IDCOOptionSymbol];
                                    dividendIDCOWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.IDCOOptionSymbol] = dividendIDCOWiseList;
                                    _dividendSymbologyWiseDict[7] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[7].Add(dividendObj.IDCOOptionSymbol, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameIDCODict = new Dictionary<string, List<DividendImport>>();
                                dividendSameIDCODict.Add(dividendObj.IDCOOptionSymbol, dividendlist);
                                _dividendSymbologyWiseDict.Add(7, dividendSameIDCODict);
                            }
                            break;
                        // Key: 8 for OpraOptionSymbol
                        // else if (!String.IsNullOrEmpty(dividendObj.OpraOptionSymbol))
                        case "OpraOptionSymbol":
                            if (_dividendSymbologyWiseDict.ContainsKey(8))
                            {
                                Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[8];
                                if (dividendSameSymbologyDict.ContainsKey(dividendObj.OpraOptionSymbol))
                                {
                                    List<DividendImport> dividendOpraWiseList = dividendSameSymbologyDict[dividendObj.OpraOptionSymbol];
                                    dividendOpraWiseList.Add(dividendObj);
                                    dividendSameSymbologyDict[dividendObj.OpraOptionSymbol] = dividendOpraWiseList;
                                    _dividendSymbologyWiseDict[8] = dividendSameSymbologyDict;
                                }
                                else
                                {
                                    List<DividendImport> dividendlist = new List<DividendImport>();
                                    dividendlist.Add(dividendObj);
                                    _dividendSymbologyWiseDict[8].Add(dividendObj.OpraOptionSymbol, dividendlist);
                                }
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                Dictionary<string, List<DividendImport>> dividendSameOpraDict = new Dictionary<string, List<DividendImport>>();
                                dividendSameOpraDict.Add(dividendObj.OpraOptionSymbol, dividendlist);
                                _dividendSymbologyWiseDict.Add(8, dividendSameOpraDict);
                            }
                            break;
                    }
                    _dividendValueCollection.Add(dividendObj);
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
        /// Gets SM Data For Dividend Import
        /// </summary>
        private void GetSMDataForDividendImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_dividendSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<DividendImport>>> kvp in _dividendSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<DividendImport>> symbolDict = _dividendSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<DividendImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //    secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = SecurityMasterManager.Instance.SendRequest(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            UpdateCollection(secMasterObj, string.Empty);
                        }
                    }
                    //Import only if all the symbols are validated
                    if (_dividendValueCollection.Count == _validatedDividendValueCollection.Count)
                    {
                        SaveActivities();
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
        /// Update activity collection asynchronously for each secmaster response
        /// After validating all the symbols we will do import process.
        /// For a single invalidated taxlot import will be cancelled
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void SecurityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                //update positoion master collection
                //Validated PositionMaster objects will be added to _validatedPositionMasterCollection
                UpdateCollection(e.Value, string.Empty);
                //UpdateValidatedPositionsCollection();
                if (_dividendValueCollection.Count == _validatedDividendValueCollection.Count)
                {
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
        /// Wires Events
        /// </summary>
        public void WireEvents()
        {
            try
            {
                SecurityMasterManager.Instance.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(SecurityMaster_SecMstrDataResponse);
                //new SecMasterDataHandler(SecurityMaster_SecMstrDataResponse);
                _timerRefresh.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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
                _timerRefresh.Elapsed -= new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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
                string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedDividendValue" + ".xml";
                string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedDividendValue" + ".xml";

                List<DividendImport> lstNonValidatedDividend = _dividendValueCollection.FindAll(dividentValue => dividentValue.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                DataTable dtNonValidatedDividend = GeneralUtilities.GetDataTableFromList(lstNonValidatedDividend);
                dtNonValidatedDividend.TableName = "NonValidatedDividendValue";
                if (dtNonValidatedDividend != null && dtNonValidatedDividend.Rows.Count > 0)
                {
                    dtNonValidatedDividend.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedDividendValue", lstNonValidatedDividend.Count, nonValidatedXmlFilePath);
                }

                DataTable dtValidatedDividendValue = GeneralUtilities.GetDataTableFromList(_validatedDividendValueCollection);
                dtValidatedDividendValue.TableName = "ValidatedDividendValue";
                if (dtValidatedDividendValue != null && dtValidatedDividendValue.Rows.Count > 0)
                {
                    dtValidatedDividendValue.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedDividendValue", _validatedDividendValueCollection.Count, validatedXmlFilePath);
                }

                //Validated PositionMaster objects will be added to _validatedPositionMasterCollection
                if (_dividendValueCollection.Count == _validatedDividendValueCollection.Count)
                {
                    SaveActivities();
                    if (TaskSpecificDataPointsPreUpdate != null)
                    {
                        //Return TaskResult which was recieved from ImportManager as event argument
                        TaskSpecificDataPointsPreUpdate(this, _currentResult);
                    }
                }
                else
                {
                    if (TaskSpecificDataPointsPreUpdate != null)
                    {
                        _currentResult.Error = new Exception("All the trades are not validated, total trades are: " + _dividendValueCollection.Count + " and validated trades are : " + _validatedDividendValueCollection.Count);
                        //Return TaskResult which was recieved from ImportManager as event argument
                        TaskSpecificDataPointsPreUpdate(this, _currentResult);
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
                _timerRefresh.Stop();
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
                _timerRefresh.Stop();
                _timerRefresh.Start();
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
            _timerRefresh.Dispose();
        }

        #endregion
    }
}