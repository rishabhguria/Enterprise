using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
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

namespace Prana.Import
{
    sealed public class ForedxPriceHandler : IImportHandler, IImportITaskHandler, IDisposable
    {
        // list collection of import forex price values
        List<ForexPriceImport> _forexPriceValueCollection = new List<ForexPriceImport>();
        // list collection of validated forex price values
        List<ForexPriceImport> _validatedForexPriceValueCollection = new List<ForexPriceImport>();

        const string DATEFORMAT = "MM/dd/yyyy";

        public ForedxPriceHandler() { }

        #region local variables
        TaskResult _currentResult = new TaskResult();
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        //string _executionName;
        private string _validatedSymbolsXmlFilePath = string.Empty;
        private string _totalImportDataXmlFilePath = string.Empty;

        private System.Timers.Timer _timerSecurityValidation;
        //int _countSymbols = 0;
        //int _countValidatedSymbols = 0;
        //int _countNonValidatedSymbols = 0;
        //List<SecMasterBaseObj> _secMasterResponseCollection = new List<SecMasterBaseObj>();
        /// <summary>
        /// added by: Bharat raturi, 22 may 2014
        /// purpose: flag variable to indicate whether the data is to be saved in application
        /// </summary>
        bool _isSaveDataInApplication = false;
        //BackgroundWorker _bgwImportData = new BackgroundWorker();

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
                //_bgwImportData.DoWork += new DoWorkEventHandler(bgwImportData_DoWork);
                //_bgwImportData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
                //_bgwImportData.WorkerSupportsCancellation = true;
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
                //_bgwImportData.DoWork -= new DoWorkEventHandler(bgwImportData_DoWork);
                //_bgwImportData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
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

        ///// <summary>
        ///// Update ForexPrice collection asynchronously for each secmaster response
        ///// After validating all the symbols we will do import process.
        ///// For a single invalidated ForexPrice import will be cancelled
        ///// </summary>
        ///// <param name="secMasterObj"></param>
        //private void SecurityMaster_SecMstrDataResponse(SecMasterBaseObj secMasterObj)
        //{
        //    try
        //    {
        //        //update forex price collection
        //        //Validated ForexPrice objects will be added to _validatedForexPriceValueCollection
        //        UpdateCollection(secMasterObj, string.Empty);

        //        //If we get response for all the requsted symbols then call the TimerRefresh_Tick to save the changes
        //        //This may be possible that we get response for all the symbols for but some data is not validated
        //        //e.g. date or account missing in file, data will be invalidated
        //        if (_countSymbols == _countValidatedSymbols + _countNonValidatedSymbols)
        //        {
        //            _timerSecurityValidation.Stop();
        //            TimerRefresh_Tick(this, null);
        //        }
        //        else
        //        {
        //            ResetTimer();
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

                ImportHelper.SetDirectoryPath(_currentResult, ref _validatedSymbolsXmlFilePath, ref _totalImportDataXmlFilePath);

                SaveImportedFileDetails(runUpload);
                SetCollection(ds, runUpload);
                // Purpose : To check valid forexPrice collection before saving
                ValidateForexPriceCollection();
                //SaveForexPrices();
                TimerRefresh_Tick(null, null);
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

        public string GetXSDName()
        {
            return "ImportForexPrice.xsd";
        }

        public void UpdateCollection(BusinessObjects.SecurityMasterBusinessObjects.SecMasterBaseObj secMasterObj, string collectionKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To reconcile data
        /// </summary>
        /// <returns></returns>
        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Local Functions

        private void SetCollection(DataSet ds, RunUpload runUpload)
        {
            try
            {
                _forexPriceValueCollection.Clear();
                _validatedForexPriceValueCollection.Clear();
                //_countSymbols = 0;
                //_countValidatedSymbols = 0;
                List<string> currencyStandardPairs = RunUploadManager.GetCurrencyStandardPairs();

                List<string> forexUniqueIdsList = new List<string>();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(ForexPriceImport).ToString());
                DataTable dTable = ds.Tables[0];

                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    ForexPriceImport forexPriceValue = new ForexPriceImport();
                    forexPriceValue.BaseCurrencyID = 0;
                    forexPriceValue.SettlementCurrencyID = 0;
                    forexPriceValue.ForexPrice = 0;
                    forexPriceValue.Date = string.Empty;
                    forexPriceValue.RowIndex = irow;
                    forexPriceValue.ImportStatus = Prana.BusinessObjects.AppConstants.ImportStatus.NotImported.ToString();
                    forexPriceValue.Source = (int)Enum.Parse(typeof(PricingSource), PricingSource.Gateway.ToString());

                    ImportHelper.SetProperty(typeToLoad, ds, forexPriceValue, irow);

                    // Purpose : To check for user selected date
                    if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                    {
                        DateTime dtn = Convert.ToDateTime(runUpload.SelectedDate);
                        forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                    }

                    // TODO : Need to remove conversion of date to double and check with function DateConvertFunction in prana tools for proper date check validation.
                    // Date converted to double for case when there is number in date field in xls file. 
                    else if (!forexPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(forexPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(forexPriceValue.Date));
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(forexPriceValue.Date);
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    else
                    {
                        forexPriceValue.ValidationError += "Date Required";
                    }

                    // Purpose : To handle user selected account
                    if (runUpload.IsUserSelectedAccount && !runUpload.SelectedAccount.Equals(String.Empty))
                    {
                        forexPriceValue.AccountID = runUpload.SelectedAccount;
                        forexPriceValue.AccountName = CachedDataManager.GetInstance.GetAccountText(runUpload.SelectedAccount);
                    }
                    else if (!string.IsNullOrWhiteSpace(forexPriceValue.AccountName))
                    {
                        forexPriceValue.AccountID = CachedDataManager.GetInstance.GetAccountID(forexPriceValue.AccountName);
                    }

                    if (!string.IsNullOrEmpty(forexPriceValue.BaseCurrency))
                    {
                        forexPriceValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(forexPriceValue.SettlementCurrency))
                    {
                        forexPriceValue.SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.SettlementCurrency.Trim());
                    }

                    if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.D) && forexPriceValue.ForexPrice != 0)
                    {
                        forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
                    }

                    if (forexPriceValue.ForexPrice > 0)
                    {
                        string forexUniqueID = forexPriceValue.Date + forexPriceValue.BaseCurrencyID + forexPriceValue.SettlementCurrencyID;
                        if (!forexUniqueIdsList.Contains(forexUniqueID))
                        {
                            forexUniqueIdsList.Add(forexUniqueID);

                            if (forexPriceValue.BaseCurrencyID > 0 && forexPriceValue.SettlementCurrencyID > 0 && forexPriceValue.BaseCurrencyID != forexPriceValue.SettlementCurrencyID)
                            {
                                string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
                                if (currencyStandardPairs.Contains(uniqueID))
                                {
                                    _forexPriceValueCollection.Add(forexPriceValue);
                                }
                                else
                                {
                                    ForexPriceCurrencyStandardPairCheck(currencyStandardPairs, forexPriceValue);
                                }
                            }
                            else
                            {
                                _forexPriceValueCollection.Add(forexPriceValue);
                                forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
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

        private void UpdateDayEndBaseCashByForexRate(List<ForexPriceImport> forexPriceValueCollection)
        {
            try
            {
                DataTable dtForexConversionTemp = new DataTable();
                dtForexConversionTemp.Columns.Add(new DataColumn("FromCurrencyID"));
                dtForexConversionTemp.Columns.Add(new DataColumn("ToCurrencyID"));
                dtForexConversionTemp.Columns.Add(new DataColumn("Date"));
                dtForexConversionTemp.Columns.Add(new DataColumn("ConversionFactor"));
                for (int counter = 0; counter < forexPriceValueCollection.Count; counter++)
                {
                    DataRow drNew = dtForexConversionTemp.NewRow();
                    drNew["FromCurrencyID"] = forexPriceValueCollection[counter].BaseCurrencyID;
                    drNew["ToCurrencyID"] = forexPriceValueCollection[counter].SettlementCurrencyID;
                    drNew["Date"] = DateTime.ParseExact(forexPriceValueCollection[counter].Date, "MM/dd/yyyy", null);
                    drNew["ConversionFactor"] = forexPriceValueCollection[counter].ForexPrice;
                    dtForexConversionTemp.Rows.Add(drNew);
                    dtForexConversionTemp.AcceptChanges();
                }
                dtForexConversionTemp.TableName = "Table1";

                ServiceManager.Instance.CashManagementServices.InnerChannel.UpdateDayEndBaseCashByForexRate(dtForexConversionTemp);
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

        private void ForexPriceCurrencyStandardPairCheck(List<string> currencyStandardPairs, ForexPriceImport forexPriceValue)
        {
            try
            {
                //  to restore the old value,if currency pair does not exits in the application
                double storedForexValue = double.MinValue;

                ///Condition Commented by Ashish. 16th Feb 09
                //if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.M) && forexPriceValue.ForexPrice != 0)
                //{
                //    storedForexValue = forexPriceValue.ForexPrice;
                //    forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
                //}
                int baseCurrencyID = forexPriceValue.BaseCurrencyID;
                forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
                forexPriceValue.SettlementCurrencyID = baseCurrencyID;

                string baseCurrency = forexPriceValue.BaseCurrency;
                forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
                forexPriceValue.SettlementCurrency = baseCurrency;

                //Added by Ashish. 
                storedForexValue = forexPriceValue.ForexPrice;
                forexPriceValue.ForexPrice = 1 / storedForexValue;

                // new ID 
                string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
                if (currencyStandardPairs.Contains(uniqueID))
                {
                    _forexPriceValueCollection.Add(forexPriceValue);
                }
                else // if currency standard pair does not exits in the cache, then restore old values and display the same
                {
                    int currencyID = forexPriceValue.BaseCurrencyID;
                    forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
                    forexPriceValue.SettlementCurrencyID = currencyID;

                    string currency = forexPriceValue.BaseCurrency;
                    forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
                    forexPriceValue.SettlementCurrency = currency;

                    if (storedForexValue != double.MinValue)
                    {
                        forexPriceValue.ForexPrice = storedForexValue;
                    }
                    _forexPriceValueCollection.Add(forexPriceValue);
                    forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
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

        //private void UpdateValidatedForexPriceCollection()
        //{
        //    try
        //    {
        //        _validatedForexPriceValueCollection = new List<ForexPriceImport>();

        //        // TODO : invalidPositions is irrelevant as not being used anywhere
        //        int invalidPositions = 0;
        //        foreach (ForexPriceImport forexPrice in _forexPriceValueCollection)
        //        {
        //            if (forexPrice.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
        //            {
        //                _validatedForexPriceValueCollection.Add(forexPrice);
        //            }
        //            else
        //{
        //                invalidPositions++;
        //}
        //        }

        //        // modified: omshiv, Jan 28, 2014, If user select invalid rows then we will prompt to user,
        //        //if user cancels
        //        //if (invalidPositions > 0)
        //        //{
        //            //DialogResult result = MessageBox.Show("You have selected invalid trades also, but they will not import." + System.Environment.NewLine + "Do you want to continue? Click 'Yes' for continue and 'No' for cancel import. ", "Nirvana Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //            //if (result == DialogResult.No)
        //            //{
        //            //    _validatedForexPriceValueCollection.Clear();
        //            //}

        //        //}

        //}
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
                //string validatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ValidatedForexPrices" + ".xml";
                //string nonValidatedXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_NonValidatedForexPrices" + ".xml";
                //string totalImportDataXmlFilePath = _refDataDirectoryPath + @"\" + _executionName + "_ImportData" + ".xml";

                if (_forexPriceValueCollection != null)
                {
                    //List<ForexPriceImport> lstNonValidatedForexPrices = _forexPriceValueCollection.FindAll(forexPrice => forexPrice.ValidationStatus != ApplicationConstants.ValidationStatus.Validated.ToString());
                    //DataTable dtNonValidatedForexPrices = GeneralUtilities.GetDataTableFromList(lstNonValidatedForexPrices);
                    //dtNonValidatedForexPrices.TableName = "NonValidatedForexPrices";

                    #region write xml for non validated trades
                    int countNonValidatedForexPrices = _forexPriceValueCollection.Count(forexPrice => forexPrice.Validated != ApplicationConstants.ValidationStatus.Validated.ToString());
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("NonValidatedForexPrice", countNonValidatedForexPrices, null);
                    #endregion

                    #region write xml for validated trades
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ValidatedForexPrice", _validatedForexPriceValueCollection.Count, null);
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

                    DataTable dtForexPriceValueCollection = GeneralUtilities.GetDataTableFromList(_forexPriceValueCollection);

                    if (dtForexPriceValueCollection != null && dtForexPriceValueCollection.Rows.Count > 0)
                    {
                        UpdateImportDataXML(dtForexPriceValueCollection);
                    }

                    #endregion

                    #region For symbol validation

                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", Constants.SymbolValidationStatus.NotApplicable, null);

                    #endregion

                    #region add dashboard statistics data

                    int accountCounts = _forexPriceValueCollection.Select(forexPrice => forexPrice.AccountID).Distinct().Count();
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("AccountCount", accountCounts, null);

                    //int symbolsPendingApprovalCounts = _secMasterResponseCollection.Count(secMasterObj => secMasterObj.IsSecApproved != true);
                    //_currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SecApproveFailedCount", symbolsPendingApprovalCounts, null);

                    #endregion

                    #region SaveForexPrices
                    if (_isSaveDataInApplication)
                    {
                        if (_validatedForexPriceValueCollection.Count == 0)
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
                            SaveForexPrices();
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
                //UnwireEvents();
            }
            #endregion
        }

        //private void bgwImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
        //        {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
        //            MessageBox.Show("Operation has been cancelled!", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Mark prices saved successfully", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        UnwireEvents();
        //        if (TaskSpecificDataPointsPreUpdate != null)
        //        {
        //            //Return TaskResult which was recieved from ImportManager as event argument
        //            TaskSpecificDataPointsPreUpdate(this, _currentResult);
        //            TaskSpecificDataPointsPreUpdate = null;
        //        }

        //    }
        //}

        //private void bgwImportData_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    int rowsUpdated = 0;
        //    try
        //    {
        //        if (_forexPriceValueCollection.Count > 0)
        //        {
        //            // insert cash values into the DB
        //            if (_forexPriceValueCollection.Count > 0)
        //            {
        //                UpdateValidatedForexPriceCollection();
        //                // total number of records inserted
        //                //totalRecordsCount = _forexPriceValueCollection.Count;
        //                rowsUpdated = RunUploadManager.SaveRunUploadFileDataForForexPrice(_validatedForexPriceValueCollection);
        //                if (rowsUpdated > 0)
        //                {
        //                    UpdateDayEndBaseCashByForexRate(_validatedForexPriceValueCollection);
        //                }
        //            }
        //            else
        //            {
        //                //MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }

        //            if (rowsUpdated > 0)
        //            {
        //                //gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
        //                //gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    //SaveForexPrices();
        //}

        //private void SaveForexPrices()
        //{
        //    //UpdateValidatedMarkPriceCollection();
        //    // insert cash values into the DB
        //    if (_validatedForexPriceValueCollection.Count > 0)
        //    {
        //        // total number of records inserted
        //        //totalRecordsCount = _markPriceValueCollection.Count;


        //        DataTable dtMarkPrices = CreateDataTableForForexPriceImport();

        //        DataTable dtMarkPriceTableFromCollection = GeneralUtilities.CreateTableFromCollection<ForexPriceImport>(dtMarkPrices, _validatedForexPriceValueCollection);

        //        if ((dtMarkPriceTableFromCollection != null) && (dtMarkPriceTableFromCollection.Rows.Count > 0))
        //        {
        //            rowsUpdated = ServiceManager.Instance.PricingServices.InnerChannel.SaveMarkPrices(dtMarkPriceTableFromCollection);
        //        }
        //        //rowsUpdated = RunUploadManager.SaveRunUploadFileDataForMarkPrice(dtMarkPricesNew,_markPriceValueCollection);
        //    }
        //    else
        //{
        //        //MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //private DataTable CreateDataTableForForexPriceImport()
        //{
        //    DataTable dtMarkPricesNew = new DataTable();
        //    try
        //    {
        //        dtMarkPricesNew.TableName = "MarkPriceImport";

        //        DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
        //        DataColumn colDate = new DataColumn("Date", typeof(string));
        //        DataColumn colMarkPrice = new DataColumn("MarkPrice", typeof(double));
        //        DataColumn colForwardPoints = new DataColumn("ForwardPoints", typeof(double));
        //        DataColumn colMarkPriceImportType = new DataColumn("MarkPriceImportType", typeof(string));
        //        DataColumn colAccountID = new DataColumn("AccountID", typeof(int));

        //        dtMarkPricesNew.Columns.Add(colSymbol);
        //        dtMarkPricesNew.Columns.Add(colDate);
        //        dtMarkPricesNew.Columns.Add(colMarkPrice);
        //        dtMarkPricesNew.Columns.Add(colForwardPoints);
        //        dtMarkPricesNew.Columns.Add(colMarkPriceImportType);
        //        dtMarkPricesNew.Columns.Add(colAccountID);
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
        //    return dtMarkPricesNew;
        //}


        /// <summary>
        /// To import Forex prices in DB
        /// </summary>
        private void SaveForexPrices()
        {
            int rowsUpdated = 0;
            try
            {
                bool isPartialSuccess = false;
                string resultofValidation = string.Empty;
                // insert cash values into the DB
                if (_validatedForexPriceValueCollection.Count > 0)
                {
                    DataTable dtForexPriceValueCollection = GeneralUtilities.GetDataTableFromList(_validatedForexPriceValueCollection);
                    try
                    {
                        //UpdateValidatedForexPriceCollection();
                        // total number of records inserted
                        //totalRecordsCount = _forexPriceValueCollection.Count;
                        rowsUpdated = RunUploadManager.SaveRunUploadFileDataForForexPrice(_validatedForexPriceValueCollection);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                        //All the trades that are not imported in application due to error, change their status   
                        foreach (DataRow row in dtForexPriceValueCollection.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.ImportError.ToString();
                        }
                    }
                    if (rowsUpdated > 0)
                    {
                        UpdateDayEndBaseCashByForexRate(_validatedForexPriceValueCollection);

                        //All the trades that are imported in application change their status   
                        foreach (DataRow row in dtForexPriceValueCollection.Rows)
                        {
                            row["ImportStatus"] = ImportStatus.Imported.ToString();
                        }
                        resultofValidation = Constants.ImportCompletionStatus.Success.ToString();
                        UpdateImportDataXML(dtForexPriceValueCollection);
                    }

                    int importedTradesCount = dtForexPriceValueCollection.Select("ImportStatus ='Imported'").Length;
                    if (importedTradesCount != dtForexPriceValueCollection.Rows.Count)
                    {
                        isPartialSuccess = true;
                    }

                    // TODO: Use enum Import status in appConstants to update ImportStatus
                    if (_forexPriceValueCollection.Count == _validatedForexPriceValueCollection.Count && !isPartialSuccess && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Success", null);
                    }
                    else if ((_forexPriceValueCollection.Count != _validatedForexPriceValueCollection.Count || isPartialSuccess) && resultofValidation == "Success")
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Partial Success", null);
                    }
                    else
                    {
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Failure", null);
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
        /// <param name="dtForexPriceValueCollection"></param>
        private void UpdateImportDataXML(DataTable dtForexPriceValueCollection)
        {
            try
            {
                dtForexPriceValueCollection.TableName = "ImportData";
                if (dtForexPriceValueCollection.Columns.Contains("RowIndex"))
                {
                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = dtForexPriceValueCollection.Columns["RowIndex"];
                    dtForexPriceValueCollection.PrimaryKey = columns;
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
                        if (dtForexPriceValueCollection.Columns.Contains("BrokenRulesCollection"))
                        {
                            dtForexPriceValueCollection.Columns.Remove("BrokenRulesCollection");
                        }
                        dtForexPriceValueCollection.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtForexPriceValueCollection.WriteXml(Application.StartupPath + _totalImportDataXmlFilePath);
                _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportData", _forexPriceValueCollection.Count, _totalImportDataXmlFilePath);
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

        private void SaveImportedFileDetails(RunUpload runUpload)
        {
            try
            {
                //_importedFileName = runUpload.FileName;
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

        /// <summary>
        /// To set _validatedForexPriceValueCollection list for validated forexPrice
        /// </summary>
        private void ValidateForexPriceCollection()
        {
            try
            {
                if (_forexPriceValueCollection.Count > 0)
                {
                    foreach (ForexPriceImport forexPrice in _forexPriceValueCollection)
                    {
                        if (forexPrice.Validated.Equals(ApplicationConstants.ValidationStatus.Validated.ToString()))
                        {
                            _validatedForexPriceValueCollection.Add(forexPrice);
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
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _timerSecurityValidation.Dispose();
        }

        #endregion
    }
}
