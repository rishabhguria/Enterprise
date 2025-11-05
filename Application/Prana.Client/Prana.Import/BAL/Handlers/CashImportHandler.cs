using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Import
{
    class CashImportHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        const string DATEFORMAT = "MM/dd/yyyy";
        CompanyUser _companyUser;

        public CashImportHandler() { }

        // list collection of cash currency values
        List<CashCurrencyValue> _cashCurrencyValueCollection = new List<CashCurrencyValue>();
        // list collection of validated currency values
        List<CashCurrencyValue> _validatedCurrencyValueCollection = new List<CashCurrencyValue>();

        #region local variables
        TaskResult _currentResult = new TaskResult();
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        //string _executionName;
        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        private System.Timers.Timer _timerSecurityValidation;

        /// purpose: flag variable to indicate whether the data is to be saved in application
        bool _isSaveDataInApplication = false;
        int _rowsUpdated = 0;

        #endregion

        #region event handler
        public event EventHandler TaskSpecificDataPointsPreUpdate;
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
                _timerSecurityValidation.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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
                _timerSecurityValidation.Elapsed -= new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
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

        public void ProcessRequest(System.Data.DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
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

            _companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;

            SaveImportedFileDetails(runUpload);
            SetCollection(ds, runUpload);
            // Purpose : To check valid cashCurrency collection before saving
            ValidateCurrencyValueCollection();
            //SaveCashCurrency();
            TimerRefresh_Tick(null, null);
        }

        public void UpdateCollection(BusinessObjects.SecurityMasterBusinessObjects.SecMasterBaseObj secMasterObj, string collectionKey)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable ValidatePriceTolerance(System.Data.DataSet ds)
        {
            throw new NotImplementedException();
        }

        public string GetXSDName()
        {
            return "ImportCash.xsd";
        }
        #endregion

        # region Local functions

        /// <summary>
        /// To save imported file details in DB
        /// </summary>
        /// <param name="runUpload"></param>
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

        private void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _cashCurrencyValueCollection.Clear();
                _validatedCurrencyValueCollection.Clear();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(CashCurrencyValue).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    CashCurrencyValue cashCurrencyValue = new CashCurrencyValue();
                    cashCurrencyValue.BaseCurrencyID = 0;
                    cashCurrencyValue.LocalCurrencyID = 0;
                    cashCurrencyValue.Date = string.Empty;
                    cashCurrencyValue.RowIndex = irow;
                    cashCurrencyValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, cashCurrencyValue, irow);

                    // Purpose : To check for user selected date
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                    }

                    else if (!cashCurrencyValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(cashCurrencyValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(cashCurrencyValue.Date));
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(cashCurrencyValue.Date);
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    else
                    {
                        cashCurrencyValue.ValidationError += "Date Required";
                    }
                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        cashCurrencyValue.AccountID = runUpload.SelectedAccount;
                        cashCurrencyValue.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!string.IsNullOrEmpty(cashCurrencyValue.AccountName))
                    {
                        cashCurrencyValue.AccountID = CachedDataManager.GetInstance.GetAccountID(cashCurrencyValue.AccountName.Trim());
                    }

                    if (!string.IsNullOrEmpty(cashCurrencyValue.BaseCurrency))
                    {
                        cashCurrencyValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(cashCurrencyValue.LocalCurrency))
                    {
                        cashCurrencyValue.LocalCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.LocalCurrency.Trim());
                    }

                    if (cashCurrencyValue.CashValueBase == 0 && cashCurrencyValue.CashValueLocal != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                //CHMW-3132	Account wise fx rate handling for expiration settlement
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date), cashCurrencyValue.AccountID);
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }
                    if (cashCurrencyValue.CashValueLocal == 0 && cashCurrencyValue.CashValueBase != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                //CHMW-3132	Account wise fx rate handling for expiration settlement
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date), cashCurrencyValue.AccountID);
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }

                    _cashCurrencyValueCollection.Add(cashCurrencyValue);
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

        /// <summary>
        /// If all the trades are validated then data will be imported in system
        /// Partial validated trades will not be saved in system
        /// Statistics xml will be written for validated and non validated trades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                //string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedCashCurrency" + ".xml";
                //string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedCashCurrency" + ".xml";
                //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";

                if (_cashCurrencyValueCollection != null)
                {
                    //List<CashCurrencyValue> lstNonValidatedCashCurrency = _cashCurrencyValueCollection.FindAll(cashCurrency => cashCurrency.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                    //DataTable dtNonValidatedCashCurrency = GeneralUtilities.GetDataTableFromList(lstNonValidatedCashCurrency);
                    //dtNonValidatedCashCurrency.TableName = "NonValidatedCashCurrency";

                    #region write xml for non validated trades
                    int countNonValidatedCashCurrency = _cashCurrencyValueCollection.Count(cashCurrency => cashCurrency.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedCashCurrency", countNonValidatedCashCurrency, null);
                    #endregion

                    #region write xml for validated trades
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedCashCurrency", _validatedCurrencyValueCollection.Count, null);
                    #endregion

                    #region commented
                    //commented by: Bharat raturi, 22 may 2014
                    //purpose: replace the code with the code for writing the import data file
                    //if (dtNonValidatedMarkPrices != null && dtNonValidatedMarkPrices.Rows.Count > 0)
                    //{
                    //    dtNonValidatedMarkPrices.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                    //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedMarkPrices", lstNonValidatedMarkPrices.Count, nonValidatedXmlFilePath);
                    //}

                    //DataTable dtValidatedMarkPrices = GeneralUtilities.GetDataTableFromList(_validatedMarkPriceValueCollection);
                    //dtValidatedMarkPrices.TableName = "ValidatedMarkPrices";
                    //if (dtValidatedMarkPrices != null && dtValidatedMarkPrices.Rows.Count > 0)
                    //{
                    //    dtValidatedMarkPrices.WriteXml(Application.StartupPath + nonValidatedXmlFilePath);
                    //    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedMarkPrices", _validatedMarkPriceValueCollection.Count, validatedXmlFilePath);
                    //}
                    #endregion

                    #region total import data added to task statistics

                    DataTable dtCashCurrencyCollection = GeneralUtilities.GetDataTableFromList(_cashCurrencyValueCollection);

                    if (dtCashCurrencyCollection != null && dtCashCurrencyCollection.Rows.Count > 0)
                    {
                        UpdateImportDataXML(dtCashCurrencyCollection);
                    }

                    #endregion

                    #region For symbol validation

                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.NotApplicable, null);

                    #endregion

                    #region add dashboard statistics data

                    int accountCounts = _cashCurrencyValueCollection.Select(cashCurrency => cashCurrency.AccountID).Distinct().Count();
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FundCount", accountCounts, null);

                    //int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                    //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                    #endregion

                    #region Import into Application

                    if (_isSaveDataInApplication)
                    {
                        if (_validatedCurrencyValueCollection.Count == 0)
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Failure, null);
                            //_bgwImportData.RunWorkerAsync();
                            if (TaskSpecificDataPointsPreUpdate != null)
                            {
                                TaskSpecificDataPointsPreUpdate(this, _currentResult);
                                TaskSpecificDataPointsPreUpdate = null;
                            }
                            UnwireEvents();
                        }
                        else
                            SaveCashCurrency();
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
                UnwireEvents();
            }
            #endregion
        }

        /// <summary>
        /// To import cash currency in DB
        /// </summary>
        private void SaveCashCurrency()
        {
            try
            {
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                bool isValidToTrade = validateTradeForAccountNAVLock();

                if (isValidToTrade)
                {
                    // insert cash values into the DB
                    if (_validatedCurrencyValueCollection.Count > 0)
                    {
                        DataTable dtCashCurrencyCollection = GeneralUtilities.GetDataTableFromList(_validatedCurrencyValueCollection);
                        try
                        {
                            //UpdateValidatedCashCurrencyCollection();
                            /// total number of records inserted
                            //totalRecordsCount = _cashCurrencyValueCollection.Count;
                            _rowsUpdated = RunUploadManager.SaveRunUploadFileDataForCash(_validatedCurrencyValueCollection);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                            //All the trades that are not imported in application due to error, change their status   
                            foreach (DataRow row in dtCashCurrencyCollection.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.ImportError.ToString();
                            }
                        }

                        if (_rowsUpdated > 0)
                        {
                            //All the trades that are imported in application change their status   
                            foreach (DataRow row in dtCashCurrencyCollection.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.Imported.ToString();
                            }
                            resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                            UpdateImportDataXML(dtCashCurrencyCollection);
                        }

                        int importedTradesCount = dtCashCurrencyCollection.Select("ImportStatus ='Imported'").Length;
                        if (importedTradesCount != dtCashCurrencyCollection.Rows.Count)
                        {
                            isPartialSuccess = true;
                        }

                        if (_cashCurrencyValueCollection.Count == _validatedCurrencyValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                        }
                        else if ((_cashCurrencyValueCollection.Count != _validatedCurrencyValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (TaskSpecificDataPointsPreUpdate != null)
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
        /// <param name="dtCashCurrencyCollection"></param>
        private void UpdateImportDataXML(DataTable dtCashCurrencyCollection)
        {
            try
            {
                dtCashCurrencyCollection.TableName = "ImportData";
                if (dtCashCurrencyCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtCashCurrencyCollection.Columns["RowIndex"];
                    dtCashCurrencyCollection.PrimaryKey = columns;
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
                        if (dtCashCurrencyCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtCashCurrencyCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtCashCurrencyCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtCashCurrencyCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _cashCurrencyValueCollection.Count, _totalImportDataXmlFilePath);
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
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1508
        /// </summary>
        /// <returns></returns>
        private bool validateTradeForAccountNAVLock()
        {
            bool isProcessToSave = true;
            try
            {
                #region NAV lock validation
                //get IsNAVLockingEnabled or not from cache
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (CashCurrencyValue cashCurrencyTuple in _cashCurrencyValueCollection)
                    {
                        //if account selected then only check NAV locked or not for selected account
                        if (!string.IsNullOrEmpty(cashCurrencyTuple.AccountName))
                        {
                            DateTime tradeDate = DateTime.MinValue;
                            if (DateTime.TryParse(cashCurrencyTuple.Date, out tradeDate))
                            {
                                bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(cashCurrencyTuple.AccountID, tradeDate);
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

        /// <summary>
        /// To set _validatedCurrencyValueCollection list for validated CashCurrencyValue
        /// </summary>
        private void ValidateCurrencyValueCollection()
        {
            try
            {
                if (_cashCurrencyValueCollection.Count > 0)
                {
                    foreach (CashCurrencyValue cashCurrency in _cashCurrencyValueCollection)
                    {
                        if (cashCurrency.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                        {
                            _validatedCurrencyValueCollection.Add(cashCurrency);
                        }

                        #region to check account lock
                        if (Regex.IsMatch(cashCurrency.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                        {
                            if (cashCurrency.ValidationError.Length > "Account Lock Required".Length)
                            {
                                List<string> valerror = cashCurrency.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                                valerror.Remove("Account Lock Required");
                                cashCurrency.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                            }
                            else
                            {
                                cashCurrency.ValidationError = string.Empty;
                                cashCurrency.Validated = ApplicationConstants.ValidationStatus.Validated.ToString();
                            }
                        }
                        else if (Regex.IsMatch(cashCurrency.ValidationError, "Account Mapping Required", RegexOptions.IgnoreCase))
                        {
                            if (cashCurrency.ValidationError.Length > "Account Mapping Required".Length)
                            {
                                List<string> valerror = cashCurrency.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                                valerror.Remove("Account Mapping Required");
                                cashCurrency.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                            }
                            else
                            {
                                cashCurrency.ValidationError = string.Empty;
                                cashCurrency.Validated = ApplicationConstants.ValidationStatus.Validated.ToString();
                            }
                        }
                        #endregion
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
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                _timerSecurityValidation.Dispose();
            }
        }

        #endregion
    }
}
