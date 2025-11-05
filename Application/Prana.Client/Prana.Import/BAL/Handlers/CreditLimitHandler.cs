using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Import
{
    class CreditLimitHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        const string DATEFORMAT = "MM/dd/yyyy";

        public CreditLimitHandler() { }

        // list collection of Credit Limit
        List<DailyCreditLimit> _dailyCreditLimitCollection = new List<DailyCreditLimit>();
        // list collection of validated Credit Limit
        List<DailyCreditLimit> _validatedCreditLimitCollection = new List<DailyCreditLimit>();

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

            SaveImportedFileDetails(runUpload);
            SetCollection(ds, runUpload);
            //SaveDailyCreditLimit();
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
            return "ImportDailyCreditLimit.xsd";
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
                _dailyCreditLimitCollection.Clear();
                _validatedCreditLimitCollection.Clear();

                Dictionary<string, DailyCreditLimit> dailyCreditLimitCollection = new Dictionary<string, DailyCreditLimit>();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(DailyCreditLimit).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    DailyCreditLimit dailyCreditLimit = new DailyCreditLimit();
                    dailyCreditLimit.RowIndex = irow;
                    dailyCreditLimit.ImportStatus = ImportStatus.NotImported.ToString();

                    ImportHelper.SetProperty(typeToLoad, ds, dailyCreditLimit, irow);

                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        dailyCreditLimit.AccountID = runUpload.SelectedAccount;
                        dailyCreditLimit.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!String.IsNullOrEmpty(dailyCreditLimit.AccountName))
                    {
                        dailyCreditLimit.AccountID = CachedDataManager.GetInstance.GetAccountID(dailyCreditLimit.AccountName.Trim());
                    }

                    if (dailyCreditLimitCollection.ContainsKey(dailyCreditLimit.AccountName))
                    {
                        if (dailyCreditLimit.LongDebitBalance != 0)
                            dailyCreditLimitCollection[dailyCreditLimit.AccountName].LongDebitBalance = dailyCreditLimit.LongDebitBalance;
                        else if (dailyCreditLimit.ShortCreditBalance != 0)
                            dailyCreditLimitCollection[dailyCreditLimit.AccountName].ShortCreditBalance = dailyCreditLimit.ShortCreditBalance;
                    }
                    else
                    {
                        dailyCreditLimitCollection.Add(dailyCreditLimit.AccountName, dailyCreditLimit);
                    }

                    _dailyCreditLimitCollection.Add(dailyCreditLimit);
                }

                //foreach (KeyValuePair<string, DailyCreditLimit> kvp in dailyCreditLimitCollection)
                //{
                //    _dailyCreditLimitCollection.Add(kvp.Value);
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


                //List<DailyCreditLimit> lstNonValidatedCreditLimit = _dailyCreditLimitCollection.FindAll(cashCurrency => cashCurrency.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                //DataTable dtNonValidatedCreditLimit = GeneralUtilities.GetDataTableFromList(lstNonValidatedCreditLimit);
                //dtNonValidatedCreditLimit.TableName = "NonValidatedCreditLimit";

                #region write xml for non validated trades
                int countNonValidatedCreditLimit = _dailyCreditLimitCollection.Count(dailyCreditLimit => dailyCreditLimit.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedCreditLimit", countNonValidatedCreditLimit, null);
                #endregion

                #region write xml for validated trades
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedCreditLimit", _validatedCreditLimitCollection.Count, null);
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

                DataTable dtCreditLimitCollection = GeneralUtilities.GetDataTableFromList(_dailyCreditLimitCollection);

                if (dtCreditLimitCollection != null && dtCreditLimitCollection.Rows.Count > 0)
                {
                    UpdateImportDataXML(dtCreditLimitCollection);
                }

                #endregion

                #region For symbol validation

                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.NotApplicable, null);

                #endregion

                #region add dashboard statistics data

                int accountCounts = _dailyCreditLimitCollection.Select(creditLimit => creditLimit.AccountID).Distinct().Count();
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCounts, null);

                //int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                #endregion

                #region Import into Application

                if (_isSaveDataInApplication)
                {
                    if (_dailyCreditLimitCollection.Count == _validatedCreditLimitCollection.Count)
                    {
                        //Import status will be set to Success
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
                        //_bgwImportData.RunWorkerAsync();
                        SaveDailyCreditLimit();
                        if (TaskSpecificDataPointsPreUpdate != null)
                        {
                            TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            TaskSpecificDataPointsPreUpdate = null;
                        }
                        UnwireEvents();
                    }
                    else if (_validatedCreditLimitCollection.Count == 0)
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
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.PartialSuccess, null);
                        //_bgwImportData.RunWorkerAsync();
                        SaveDailyCreditLimit();
                        if (TaskSpecificDataPointsPreUpdate != null)
                        {
                            TaskSpecificDataPointsPreUpdate(this, _currentResult);
                            TaskSpecificDataPointsPreUpdate = null;
                        }
                        UnwireEvents();
                    }
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
        /// To import daily credit limit in DB
        /// </summary>
        private void SaveDailyCreditLimit()
        {
            int rowsUpdated = 0;
            bool isPartialSuccess = false;
            string resultofValidation = string.Empty;

            try
            {
                // insert cash values into the DB
                if (_dailyCreditLimitCollection.Count > 0)
                {
                    UpdateValidatedCreditLimitCollection();

                    if (_validatedCreditLimitCollection.Count > 0)
                    {
                        DataTable dtDailyCreditLimit = CreateDataTableForDailyCreditLimitImport();
                        DataTable dtDailyCreditLimitCollection = GeneralUtilities.CreateTableFromCollection<DailyCreditLimit>(dtDailyCreditLimit, _validatedCreditLimitCollection);

                        DataTable dtCreditLimitImportData = GeneralUtilities.GetDataTableFromList(_validatedCreditLimitCollection);
                        try
                        {
                            if ((dtDailyCreditLimitCollection != null) && (dtDailyCreditLimitCollection.Rows.Count > 0))
                            {
                                rowsUpdated = ServiceManager.Instance.CashManagementServices.InnerChannel.SaveDailyCreditLimitValues(dtDailyCreditLimitCollection, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                            //All the trades that are not imported in application due to error, change their status   
                            foreach (DataRow row in dtCreditLimitImportData.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.ImportError.ToString();
                            }
                        }

                        if (rowsUpdated > 0)
                        {
                            //All the trades that are imported in application change their status   
                            foreach (DataRow row in dtCreditLimitImportData.Rows)
                            {
                                row["ImportStatus"] = ImportStatus.Imported.ToString();
                            }
                            resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                            UpdateImportDataXML(dtCreditLimitImportData);
                        }

                        int importedTradesCount = dtCreditLimitImportData.Select("ImportStatus ='Imported'").Length;
                        if (importedTradesCount != dtCreditLimitImportData.Rows.Count)
                        {
                            isPartialSuccess = true;
                        }

                        if (_dailyCreditLimitCollection.Count == _validatedCreditLimitCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", ImportStatus.Success, null);
                        }
                        else if ((_dailyCreditLimitCollection.Count != _validatedCreditLimitCollection.Count || isPartialSuccess) && resultofValidation == "Success")
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
        /// To update Import data xml file
        /// </summary>
        /// <param name="dtCreditLimitCollection"></param>
        private void UpdateImportDataXML(DataTable dtCreditLimitCollection)
        {
            try
            {
                dtCreditLimitCollection.TableName = "ImportData";
                if (dtCreditLimitCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtCreditLimitCollection.Columns["RowIndex"];
                    dtCreditLimitCollection.PrimaryKey = columns;
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
                        if (dtCreditLimitCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtCreditLimitCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtCreditLimitCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);

                    }
                }
                dtCreditLimitCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _dailyCreditLimitCollection.Count, _totalImportDataXmlFilePath);
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
        /// To create data table for credit limit
        /// </summary>
        /// <returns>dtDailyCreditLimit</returns>
        private DataTable CreateDataTableForDailyCreditLimitImport()
        {
            DataTable dtDailyCreditLimit = new DataTable();
            try
            {
                dtDailyCreditLimit.TableName = "Table1";

                DataColumn colAccountID = new DataColumn("FundID", typeof(int));
                DataColumn colLongDebitLimit = new DataColumn("LongDebitLimit", typeof(double));
                DataColumn colShortCreditLimit = new DataColumn("ShortCreditLimit", typeof(double));
                DataColumn colLongDebitBalance = new DataColumn("LongDebitBalance", typeof(double));
                DataColumn colShortCreditBalance = new DataColumn("ShortCreditBalance", typeof(double));

                dtDailyCreditLimit.Columns.Add(colAccountID);
                dtDailyCreditLimit.Columns.Add(colLongDebitLimit);
                dtDailyCreditLimit.Columns.Add(colShortCreditLimit);
                dtDailyCreditLimit.Columns.Add(colLongDebitBalance);
                dtDailyCreditLimit.Columns.Add(colShortCreditBalance);
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
            return dtDailyCreditLimit;
        }


        /// <summary>
        /// To update _validatedCreditLimitCollection list 
        /// </summary>
        private void UpdateValidatedCreditLimitCollection()
        {
            try
            {
                foreach (DailyCreditLimit creditLimit in _dailyCreditLimitCollection)
                {
                    if (creditLimit.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                    {
                        _validatedCreditLimitCollection.Add(creditLimit);
                    }

                    #region to check account lock
                    if (Regex.IsMatch(creditLimit.ValidationError, "Account Lock Required", RegexOptions.IgnoreCase))
                    {
                        if (creditLimit.ValidationError.Length > "Account Lock Required".Length)
                        {
                            List<string> valerror = creditLimit.ValidationError.Split(Seperators.SEPERATOR_8[0]).ToList();
                            valerror.Remove("Account Lock Required");
                            creditLimit.ValidationError = string.Join(Seperators.SEPERATOR_8, valerror.ToArray());
                        }
                        else
                        {
                            creditLimit.ValidationError = string.Empty;
                            creditLimit.Validated = ApplicationConstants.ValidationStatus.Validated.ToString();
                        }
                    }
                    #endregion
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
