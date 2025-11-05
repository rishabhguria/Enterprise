using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enumerators;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Prana.CashManagement
{
    public class CashDataManager : IDisposable
    {
        #region Members

        /// <summary>
        /// The singleton
        /// </summary>
        private static CashDataManager _singleton = null;

        /// <summary>
        /// The locker
        /// </summary>
        private static object _locker = new object();

        /// <summary>
        /// The timer
        /// </summary>
        Timer _timer;

        /// <summary>
        /// The interval
        /// </summary>
        double _interval = 0;

        /// <summary>
        /// The cash impact, Contains The Traded Data
        /// </summary>
        GenericBindingList<Transaction> _cashImpact = new GenericBindingList<Transaction>();

        /// <summary>
        /// The cash activity
        /// </summary>
        List<CashActivity> _cashActivity = new List<CashActivity>();

        /// <summary>
        /// The day end data, Contains the previous Day DayEnd Data
        /// </summary>
        List<CompanyAccountCashCurrencyValue> _dayEndData = new List<CompanyAccountCashCurrencyValue>();

        /// <summary>
        /// The cash impact bt two dates
        /// </summary>
        List<Transaction> _cashImpactBtTwoDates = new List<Transaction>();

        /// <summary>
        /// The cash exceptions
        /// </summary>
        List<Transaction> _cashExceptions = new List<Transaction>();

        /// <summary>
        /// The ls activity exceptions
        /// </summary>
        private List<CashActivity> _lsActivityExceptions = new List<CashActivity>();

        /// <summary>
        /// The open position date wise
        /// </summary>
        List<TaxLot> _openPositionDateWise = new List<TaxLot>();

        /// <summary>
        /// The base currency identifier
        /// </summary>
        private static int _baseCurrencyID = int.MinValue;

        /// <summary>
        /// The current row in validation process
        /// </summary>
        TransactionEntry _currentRowInValidationProcess = null;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cash impact.
        /// </summary>
        /// <value>
        /// The cash impact.
        /// </value>
        public GenericBindingList<Transaction> CashImpact
        {
            get { return _cashImpact; }
            set { _cashImpact = value; }
        }

        /// <summary>
        /// Gets or sets the cash activity.
        /// </summary>
        /// <value>
        /// The cash activity.
        /// </value>
        public List<CashActivity> CashActivity
        {
            get { return _cashActivity; }
            set { _cashActivity = value; }
        }

        /// <summary>
        /// Gets or sets the day end data.
        /// </summary>
        /// <value>
        /// The day end data.
        /// </value>
        public List<CompanyAccountCashCurrencyValue> DayEndData
        {
            get { return _dayEndData; }
            set { _dayEndData = value; }
        }

        /// <summary>
        /// Gets or sets the cash impact bt two dates.
        /// </summary>
        /// <value>
        /// The cash impact bt two dates.
        /// </value>
        public List<Transaction> CashImpactBtTwoDates
        {
            get { return _cashImpactBtTwoDates; }
            set { _cashImpactBtTwoDates = value; }
        }

        /// <summary>
        /// Gets or sets the cash exceptions.
        /// </summary>
        /// <value>
        /// The cash exceptions.
        /// </value>
        public List<Transaction> CashExceptions
        {
            get { return _cashExceptions; }
            set { _cashExceptions = value; }
        }

        /// <summary>
        /// Gets or sets the ls activity exception.
        /// </summary>
        /// <value>
        /// The ls activity exception.
        /// </value>
        public List<CashActivity> lsActivityException
        {
            get { return _lsActivityExceptions; }
            set { _lsActivityExceptions = value; }
        }

        /// <summary>
        /// Gets or sets the open position date wise.
        /// </summary>
        /// <value>
        /// The open position date wise.
        /// </value>
        public List<TaxLot> OpenPositionDateWise
        {
            get { return _openPositionDateWise; }
            set { _openPositionDateWise = value; }
        }

        /// <summary>
        /// Gets the base currency identifier.
        /// </summary>
        /// <value>
        /// The base currency identifier.
        /// </value>
        public static int BaseCurrencyID
        {
            get
            {
                if (_baseCurrencyID == int.MinValue)
                {
                    _baseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                return _baseCurrencyID;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static CashDataManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new CashDataManager();
                        if (ActivityServices == null)
                            CreateActivityServicesProxy();
                        if (CashManagementServices == null)
                            CreateCashManagementProxy();
                    }
                }
            }
            return _singleton;
        }

        #endregion

        #region Proxy Section

        /// <summary>
        /// The security master
        /// </summary>
        ISecurityMasterServices _securityMaster = null;

        /// <summary>
        /// Sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
            }
        }

        /// <summary>
        /// The cash management services
        /// </summary>
        static ProxyBase<ICashManagementService> _cashManagementServices = null;

        /// <summary>
        /// Gets or sets the cash management services.
        /// </summary>
        /// <value>
        /// The cash management services.
        /// </value>
        public static ProxyBase<ICashManagementService> CashManagementServices
        {
            set { _cashManagementServices = value; }
            get { return _cashManagementServices; }
        }

        /// <summary>
        /// The activity services
        /// </summary>
        static ProxyBase<IActivityServices> _activityServices = null;

        /// <summary>
        /// Gets or sets the activity services.
        /// </summary>
        /// <value>
        /// The activity services.
        /// </value>
        public static ProxyBase<IActivityServices> ActivityServices
        {
            set { _activityServices = value; }
            get { return _activityServices; }
        }

        /// <summary>
        /// Creates the cash management proxy.
        /// </summary>
        public static void CreateCashManagementProxy()
        {
            CashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
            CashManagementServices.DisconnectedEvent += new Proxy<ICashManagementService>.ConnectionEventHandler(CashManagementServices_DisconnectedEvent);
        }

        /// <summary>
        /// Handles the DisconnectedEvent event of the CashManagementServices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void CashManagementServices_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                //we need connection to be live so that it can reconnect trade server when it is restarted, PRANA-28745 
                //if (CashManagementServices != null)
                //    CashManagementServices.Dispose();
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
        /// Creates the activity services proxy.
        /// </summary>
        public static void CreateActivityServicesProxy()
        {
            ActivityServices = new ProxyBase<IActivityServices>("TradeActivityServiceEndpointAddress");
            ActivityServices.DisconnectedEvent += new Proxy<IActivityServices>.ConnectionEventHandler(ActivityServices_DisconnectedEvent);
        }

        /// <summary>
        /// Handles the DisconnectedEvent event of the ActivityServices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void ActivityServices_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (ActivityServices != null)
                    ActivityServices.Dispose();
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

        #region General Methods

        /// <summary>
        /// Gets the cash preferences.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        internal CashPreferences GetCashPreferences(int accountID)
        {
            CashPreferences objCashPreferences = new CashPreferences();
            try
            {
                Dictionary<int, CashPreferences> dictCashPref = null;

                try
                {
                    dictCashPref = _cashManagementServices.InnerChannel.GetCashPreferences();
                }
                catch
                {
                }

                if (dictCashPref != null && dictCashPref.ContainsKey(accountID))
                {
                    objCashPreferences = dictCashPref[accountID];
                }
                else
                {
                    return null;
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
            return objCashPreferences;
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        private void LogMessage(string action, string tabName, string fromDate, string toDate, String accountIDs)
        {
            try
            {
                string user = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                LogExtensions.LogMessage(PranaModules.GENERAL_LEDGER_MODULE, user, action, tabName, fromDate, toDate, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region DayEndCash Section

        /// <summary>
        /// Gets the cash impact.
        /// Modified by: Bharat raturi, 24 jul 2014
        /// purpose: send the string of accountIDs as the parameter to get the values for the specified accounts only 
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetCashImpact(DateTime fromDate, DateTime toDate, String accountIDs)
        {
            List<Transaction> lsCashImpact = new List<Transaction>();
            int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_DAY_END_CASH, fromDate.ToString(), toDate.ToString(), accountIDs);

                try
                {
                    lsCashImpact = await _cashManagementServices.InnerChannel.GetCashImpact(fromDate, toDate, accountIDs, userID).ConfigureAwait(false);
                }
                catch
                {
                    throw new Exception("TradeService not connected");
                }

                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
                if (CashImpact != null)
                {
                    CashImpact.Clear();
                    if (lsCashImpact != null)
                    {
                        CashImpact.AddList(lsCashImpact);
                    }
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
            return lsCashImpact;
        }

        /// <summary>
        /// Gets the transactions beetween two dates.
        /// Modifed by: Bharat raturi, 25 jul 2014
        /// purpose: Send the accountsIDs for getting the transactions 
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsBeetweenTwoDates(DateTime startDate, DateTime endDate, String accountIDs, CashManagementEnums.ActivitySource activitySource)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, "Get " + activitySource, startDate.ToString(), endDate.ToString(), accountIDs);

                try
                {
                    CashImpactBtTwoDates = _cashManagementServices.InnerChannel.GetTransactionsBeetweenTwoDates(startDate, endDate, accountIDs, (int)activitySource, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                }
                catch
                {
                    throw new Exception("TradeService not connected");
                }

                LogMessage(LoggingConstants.RESPONSE_RECEIVED, "Get " + activitySource, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return CashImpactBtTwoDates;
        }

        /// <summary>
        /// Gets the day end data.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <param name="dtEndDate">The dt end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public async Task<List<CompanyAccountCashCurrencyValue>> GetDayEndData(DateTime givenDate, DateTime dtEndDate, string accountIDs)
        {
            try
            {
                DayEndData = await _cashManagementServices.InnerChannel.GetDayEndCash(givenDate, dtEndDate, accountIDs).ConfigureAwait(false);
            }
            catch
            {
                throw new Exception("TradeService not connected");
            }
            return DayEndData;
        }

        /// <summary>
        /// Modified by: Bharat raturi, 2 dec 2014
        /// Save the day end date account-wise for selected dates
        /// </summary>
        /// <param name="dateWiseDayEndDataDictionary">Dictionary of values</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="accountIDs">comma separated IDs of the selected accounts</param>
        public void SaveDayEndData(Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseDayEndDataDictionary, List<CompanyAccountCashCurrencyValue> lstDeletedData, string accountIDs)
        {
            int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.SAVE_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
                _cashManagementServices.InnerChannel.SaveDayEndData(dateWiseDayEndDataDictionary, lstDeletedData, userID, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.SAVE_DAY_END_CASH, string.Empty, string.Empty, string.Empty);
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
        /// Saves the specified ls activity.
        /// </summary>
        /// <param name="lsActivity">The ls activity.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <returns></returns>
        public int Save(List<CashActivity> lsActivity, string activitySource)
        {
            int result = 0;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.SAVE_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
                List<TradeAuditEntry> activityAuditCollection = new List<TradeAuditEntry>();
                lsActivity.ForEach(x => activityAuditCollection.Add(GetTradeAuditEntryFromActivity(x, activitySource)));
                AuditManager.Instance.SaveAuditList(activityAuditCollection);
                result = _activityServices.InnerChannel.SaveActivity(lsActivity, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, false);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.SAVE_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the trade audit entry from activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="activitySource">The exception source.</param>
        /// <returns></returns>
        private TradeAuditEntry GetTradeAuditEntryFromActivity(CashActivity activity, string activitySource)
        {
            TradeAuditEntry auditEntry = new TradeAuditEntry();
            try
            {
                auditEntry.GroupID = int.MinValue.ToString();
                auditEntry.Action = TradeAuditActionType.ActionType.ActivityException;
                auditEntry.TaxLotID = activity.FKID;
                auditEntry.Comment = activitySource;
                auditEntry.CompanyUserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                auditEntry.Symbol = activity.Symbol;
                auditEntry.Level1ID = activity.AccountID;
                auditEntry.AUECLocalDate = DateTime.Now;
                auditEntry.OrderSideTagValue = string.Empty;
                auditEntry.OriginalDate = activity.Date;
                auditEntry.OriginalValue = string.Empty;
                auditEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Cash;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return auditEntry;
        }

        /// <summary>
        /// Saves the daily calculations.
        /// </summary>
        /// <param name="lsDailyCalculations">The ls daily calculations.</param>
        /// <returns></returns>
        internal async System.Threading.Tasks.Task SaveDailyCalculations(List<CashActivity> lsDailyCalculations)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.SAVE_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
                await SaveforMTM(lsDailyCalculations.Where(act => act.ActivityState == ApplicationConstants.TaxLotState.Deleted).ToList());
                await SaveforMTM(lsDailyCalculations.Where(act => act.ActivityState != ApplicationConstants.TaxLotState.Deleted).ToList());
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.SAVE_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Savefors the MTM.
        /// By KashishG.,PRANA-22141
        /// Saving Activities for MTM only
        /// </summary>
        /// <param name="lsActivity">The ls activity.</param>
        /// <returns></returns>
        private async Task<int> SaveforMTM(List<CashActivity> lsActivity)
        {
            int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            try
            {
                return await _activityServices.InnerChannel.SaveActivityforMTM(lsActivity, userID).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        /// <summary>
        /// Saves the specified ls transaction.
        /// </summary>
        /// <param name="lsTransaction">The ls transaction.</param>
        /// <param name="lsTransactionEntries">The ls transaction entries.</param>
        /// <param name="journalSource">The journal source.</param>
        /// <param name="activitySource">The activity source.</param>
        public void Save(List<Transaction> lsTransaction, Dictionary<string, TransactionEntry> lsTransactionEntries, string journalSource, string activitySource)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, "Save " + activitySource, string.Empty, string.Empty, string.Empty);
                if (!string.IsNullOrWhiteSpace(journalSource))
                {
                    List<TradeAuditEntry> journalAuditCollection = new List<TradeAuditEntry>();
                    lsTransaction.ForEach(x => journalAuditCollection.Add(GetTradeAuditEntryFromJournal(x, journalSource)));
                    AuditManager.Instance.SaveAuditList(journalAuditCollection);
                }
                _cashManagementServices.InnerChannel.SaveTransactions(lsTransaction, lsTransactionEntries, activitySource, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, false);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, "Save " + activitySource, string.Empty, string.Empty, string.Empty);
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
        /// Gets the trade audit entry from journal.
        /// </summary>
        /// <param name="journal">The journal.</param>
        /// <param name="journalSource">The journal source.</param>
        /// <returns></returns>
        private TradeAuditEntry GetTradeAuditEntryFromJournal(Transaction journal, string journalSource)
        {
            TradeAuditEntry auditEntry = new TradeAuditEntry();
            try
            {
                auditEntry.GroupID = int.MinValue.ToString();
                auditEntry.Action = TradeAuditActionType.ActionType.JournalException;
                auditEntry.TaxLotID = journal.ActivityId_FK;
                auditEntry.Comment = journalSource;
                auditEntry.CompanyUserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                auditEntry.Symbol = (journal.TransactionEntries != null && journal.TransactionEntries.Count > 0) ? journal.TransactionEntries.First().GetSymbol() : journal.GetSymbol();
                auditEntry.Level1ID = (journal.TransactionEntries != null && journal.TransactionEntries.Count > 0) ? journal.TransactionEntries.First().GetAccountID() : journal.GetAccountID();
                auditEntry.AUECLocalDate = DateTime.Now;
                auditEntry.OrderSideTagValue = string.Empty;
                auditEntry.OriginalDate = journal.Date;
                auditEntry.OriginalValue = string.Empty;
                auditEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Cash;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return auditEntry;
        }

        #endregion

        #region Cash Journal Section

        /// <summary>
        /// Generates the transaction entry identifier.
        /// </summary>
        /// <returns></returns>
        public string GenerateTransactionEntryID()
        {
            string strID = string.Empty;
            try
            {
                strID = _cashManagementServices.InnerChannel.GenerateID();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return strID;
        }

        /// <summary>
        /// Handles the SecMstrDataResponse event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{SecMasterBaseObj}"/> instance containing the event data.</param>
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                _timer.Stop();
                if (_currentRowInValidationProcess != null)
                {
                    _currentRowInValidationProcess.properityChanged("Symbol", "Symbol Validated !");
                    _currentRowInValidationProcess = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Validates the symbol.
        /// </summary>
        /// <param name="row">The row.</param>
        public void ValidateSymbol(TransactionEntry row)
        {
            if (_securityMaster != null && _securityMaster.IsConnected)
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.AddData(row.Symbol, ApplicationConstants.PranaSymbology);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = this.GetHashCode();
                row.properityChanged("Symbol", "Symbol is in validation process !");
                _currentRowInValidationProcess = row;
                if (_currentRowInValidationProcess != null)
                    _currentRowInValidationProcess.properityChanged("Symbol", "Symbol Not Validated !");
                _securityMaster.SendRequest(reqObj);
                StartTimer();
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void StartTimer()
        {
            if (_interval == 0)
                _interval = Convert.ToDouble(ConfigurationManager.AppSettings["SymbolValidationTimeOut"]);
            if (_interval == 0)
                _interval = 3000;
            _timer = new Timer(_interval);
            _timer.AutoReset = false;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Enable it
        }

        /// <summary>
        /// Handles the Elapsed event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_currentRowInValidationProcess != null)
                _currentRowInValidationProcess.properityChanged("Symbol", "Symbol Not Validated !");
            _currentRowInValidationProcess = null;
        }

        /// <summary>
        /// Creates the journal entries.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <param name="userId">The user identifier, PRANA-9776.</param>
        /// <returns></returns>
        public int CreateJournalEntries(DataSet ds, CashTransactionType transactionSource, int userId, bool IsMultileg = false)
        {
            try
            {
                return _cashManagementServices.InnerChannel.CreateJournalEntries(ds, transactionSource, userId, IsMultileg);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        #endregion

        #region Daily Calculation Section

        /// <summary>
        /// Gets the open positions for daily calculation.
        /// changed by: bharat raturi, 15 jul 2014
        /// purpose: get an extra parameter accountIDs 
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<TaxLot>>> GetOpenPositionsForDailyCalculation(DateTime fromDate, DateTime toDate, String accountIDs)
        {
            Dictionary<string, List<TaxLot>> dicOpenPositionDateWise = new Dictionary<string, List<TaxLot>>();
            int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_DAILY_CALCULATION, fromDate.ToString(), toDate.ToString(), accountIDs);

                try
                {
                    dicOpenPositionDateWise = await _cashManagementServices.InnerChannel.GetOpenPositionsForDailyCalculation(fromDate, toDate, accountIDs, userID).ConfigureAwait(false);
                }
                catch
                {
                    throw new Exception("TradeService not connected");
                }

                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_DAILY_CALCULATION, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dicOpenPositionDateWise;
        }

        /// <summary>
        /// Calculates the daily cash impact.
        /// </summary>
        /// <param name="dicOpenPositionsAndTransactionsDateWise">The dic open positions and transactions date wise.</param>
        /// <returns></returns>
        internal List<CashActivity> CalculateDailyCashImpact(Dictionary<string, List<TaxLot>> dicOpenPositionsAndTransactionsDateWise)
        {
            try
            {
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.CALCULATE_DAILY_CASH, string.Empty, string.Empty, string.Empty);
                List<TaxLot> lsM2MCalculatedData = CashManagementServices.InnerChannel.CalculateDailyCashImpact(dicOpenPositionsAndTransactionsDateWise, userID);
                List<CashActivity> cashImpact = ActivityServices.InnerChannel.CreateActivity(lsM2MCalculatedData, CashTransactionType.DailyCalculation);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.CALCULATE_DAILY_CASH, string.Empty, string.Empty, string.Empty);
                return cashImpact;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the daily calculated beta
        /// Modified by: Bharat raturi, 16 Sep 2014
        /// purpose: send the parameter AccountIDs to get the records for only the selected accounts
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">Comma separated string of account ids</param>
        /// <returns>
        /// List of cash activities
        /// </returns>
        internal async Task<List<CashActivity>> GetAlreadyCalculatedDailyCashData(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<CashActivity> lsAlreadyCalculatedCashData = null;
            try
            {
                //Send the account IDs to filter the data
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4915
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_PRECALCULATED_DAILY_CALCULATIONS, fromDate.ToString(), toDate.ToString(), string.Empty);
                lsAlreadyCalculatedCashData = await _activityServices.InnerChannel.GetAlreadyCalculatedDailyCashData(fromDate, toDate, accountIDs).ConfigureAwait(false);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_PRECALCULATED_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lsAlreadyCalculatedCashData;
        }

        #endregion

        #region Journal Exceptions Section

        /// <summary>
        /// modified by: Bharat raturi, 31 jul 2014
        /// Get the journal exceptions for selected accounts only
        /// </summary>
        /// <param name="dtFrom">Start date</param>
        /// <param name="dtTo">end date</param>
        /// <param name="accountIDs">comma separated string of accountIDs</param>
        /// <returns>
        /// List of transaction exceptions
        /// </returns>
        internal List<Transaction> GetJournalExceptions(DateTime dtFrom, DateTime dtTo, string accountIDs)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_JOURNAL_EXCEPTIONS, dtFrom.ToString(), dtTo.ToString(), accountIDs);
                List<Transaction> journals = _cashManagementServices.InnerChannel.GetJournalExceptions(dtFrom, dtTo, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_JOURNAL_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
                return journals;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the transactions for overriding.
        /// Modified by: Bharat Raturi, 16 jul 2014
        /// purpose: Get the transactions for the accounts sent as the string of comma separated values
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsForOverriding(DateTime startDate, DateTime endDate, String accountIDs)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_OVERRIDING_JOURNALS, startDate.ToString(), endDate.ToString(), string.Empty);
                List<Transaction> journals = _cashManagementServices.InnerChannel.GetTransactionsForOverriding(startDate, endDate, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_OVERRIDING_JOURNALS, string.Empty, string.Empty, string.Empty);
                return journals;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion

        #region Activity Exceptions Section

        /// <summary>
        /// Modified by: Bharat raturi, 1- aug 2014
        /// purpose: send the accountIDs to filter the activities at the DB level
        /// Get the activity exceptions
        /// </summary>
        /// <param name="dtFrom">Start date</param>
        /// <param name="dtTo">end date</param>
        /// <param name="accountIDs">Comma separated AccountIds</param>
        internal void GetActivityExceptions(DateTime dtFrom, DateTime dtTo, string accountIDs)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_ACTIVITY_EXCEPTIONS, dtFrom.ToString(), dtTo.ToString(), string.Empty);
                lsActivityException = _activityServices.InnerChannel.GetActivityExceptions(dtFrom, dtTo, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
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
        /// Gets the overriding activity.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        internal void GetOverridingActivity(DateTime startDate, DateTime endDate, string accountIDs)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_OVERRIDING_ACTIVITY, startDate.ToString(), endDate.ToString(), string.Empty);
                lsActivityException = _activityServices.InnerChannel.GetOverridingActivity(startDate, endDate, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_OVERRIDING_ACTIVITY, string.Empty, string.Empty, string.Empty);
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
        /// Saves the journal exception.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public long SaveJournalException(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            long response = -1;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GENERATE_JOURNAL_EXCEPTIONS, fromDate.ToString(), toDate.ToString(), accountIDs);
                response = _activityServices.InnerChannel.SaveJournalException(fromDate, toDate, accountIDs);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GENERATE_JOURNAL_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        #endregion

        #region Activity Section

        /// <summary>
        /// Get the Details of the activities between the dates provided as the arguments for the CashAccounts that are supplied
        /// as the string of the comma separated IDs
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="activityDateType">Type of the activity date.</param>
        public void GetActivity(DateTime fromDate, DateTime toDate, String accountIDs, String activityDateType)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_ACTIVITY, fromDate.ToString(), toDate.ToString(), accountIDs);
                CashActivity = _activityServices.InnerChannel.GetActivity(fromDate, toDate, accountIDs, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, activityDateType, true);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_ACTIVITY, string.Empty, string.Empty, string.Empty);
                NameValueFiller.SetNameValues(CashActivity);
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

        #region Account Setup Section

        /// <summary>
        /// Updates the cash accounts tables in database.
        /// </summary>
        /// <param name="dataSetMasterCategory">The data set master category.</param>
        internal void UpdateCashAccountsTablesInDB(DataSet dataSetMasterCategory)
        {
            try
            {
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _cashManagementServices.InnerChannel.UpdateCashAccountsTablesInDB(dataSetMasterCategory, userID);
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
        /// TODO
        /// </summary>
        /// <param name="dataSetActivities">The data set activities.</param>
        internal void UpdateCashActivityTablesInDB(DataSet dataSetActivities)
        {
            try
            {
                if (dataSetActivities.Tables.Contains(CashManagementConstants.TABLE_SUBACCOUNTSTYPE))
                {
                    DataTable table = dataSetActivities.Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE].GetChanges(DataRowState.Added | DataRowState.Modified);
                    if (table != null)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            row[CashManagementConstants.COLUMN_SUBACCOUNTTYPE] = row[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].ToString().Trim();
                        }
                    }
                }
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _cashManagementServices.InnerChannel.UpdateCashActivityTablesInDB(dataSetActivities, userID);
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

        #region Chart of CashAccounts Section

        /// <summary>
        /// It returns the account balances as on balance date.
        /// </summary>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        public void CalculateAndSaveBalances(DateTime endDate, string accountIDs)
        {
            try
            {
                //added by: Bharat raturi, 21 jul 2014, send the user ID to get the AUEC ID of the company of the user
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _cashManagementServices.InnerChannel.CalculateAndSaveBalances(endDate, userID, accountIDs);
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
        /// Modified by: Bharat raturi, 31 jul 2014
        /// Get the data for selected accounts only
        /// It returns the account balances as on balance date.
        /// </summary>
        /// <param name="balDate">Date till which the balances are to be gotten</param>
        /// <param name="accountIDs">comma separated accountIDs</param>
        /// <returns></returns>
        public DataSet GetAccountBalancesAsOnDate(DateTime balDate, string accountIDs)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_ACCOUNT_BALANCES, string.Empty, balDate.ToString(), accountIDs);
                DataSet accountBal = _cashManagementServices.InnerChannel.GetAccountBalancesAsOnDate(balDate, accountIDs, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_ACCOUNT_BALANCES, string.Empty, string.Empty, string.Empty);
                return accountBal;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// It returns the account balances as on balance date.
        /// </summary>
        /// <param name="subAccountID">The sub account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public DataSet GetSubAccountTransactionEntriesForDateRange(int subAccountID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_SUB_ACCOUNT_DETAILS, fromDate.ToString(), toDate.ToString(), subAccountID.ToString());
                DataSet subAccountBal = _cashManagementServices.InnerChannel.GetSubAccountTransactionEntriesForDateRange(subAccountID, fromDate, toDate);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_SUB_ACCOUNT_DETAILS, string.Empty, string.Empty, string.Empty);
                return subAccountBal;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the transactions by transaction identifier.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsByTransactionID(string transactionID)
        {
            List<Transaction> lstTransaction = new List<Transaction>();
            try
            {
                lstTransaction = _cashManagementServices.InnerChannel.GetTransactionsByTransactionID(transactionID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lstTransaction;
        }

        /// <summary>
        /// Gets the activities by activity identifier.
        /// </summary>
        /// <param name="activityIds">The activity ids.</param>
        /// <returns></returns>
        public List<CashActivity> GetActivitiesByActivityId(string activityIds)
        {
            List<CashActivity> cashActivity = new List<CashActivity>();
            try
            {
                cashActivity = _activityServices.InnerChannel.GetActivitiesByActivityId(activityIds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return cashActivity;
        }

        /// <summary>
        /// Get All Activities of the same TransactionId
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<CashActivity> GetActivitiesByTransactionId(string transactionId)
        {
            List<CashActivity> cashActivity = new List<CashActivity>();
            try
            {
                cashActivity = _activityServices.InnerChannel.GetActivitiesByTransactionId(transactionId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return cashActivity;
        }

        /// <summary>
        /// Modified by: Bharat raturi, 7 aug 2014
        /// It runs the revaluation till endDate.
        /// For Manual Revaluation we have date range so we will provide two dates i.e. fromDate and toDate
        /// For Normal Revaluation fromDate will be null as for that we only need end Date.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="balDate">The bal date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="isManualRevaluation">if set to <c>true</c> [is manual revaluation].</param>
        /// <param name="_isGetAccountBalancesClicked">if set to <c>true</c> [is get account balances clicked].</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        public async Task<bool> RunRevaluationProcess(Nullable<DateTime> fromDate, DateTime balDate, string accountIDs, bool isManualRevaluation, bool _isGetAccountBalancesClicked, string sourceType)
        {
            bool result = false;
            try
            {
                //added by: Bharat Raturi, 17 jul 2014
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                LogMessage(LoggingConstants.REQUEST_SENT, sourceType, fromDate.ToString(), balDate.ToString(), accountIDs);
                Logger.LoggerWrite("RunRevaluationProcess Starts... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                var revaluationStartTime = DateTime.Now;
                Dictionary<CashManagementEnums.OperationMode, List<int>> dictAccountIdByRevaluationOperationMode = _cashManagementServices.InnerChannel.GetAccountIdByRevaluationOperationMode();
                string operationModeWiseAccountIDs = string.Empty;
                foreach (KeyValuePair<CashManagementEnums.OperationMode, List<int>> kvp in dictAccountIdByRevaluationOperationMode)
                {
                    DateTime updatedBalDate = balDate;
                    operationModeWiseAccountIDs = GetFilterAccountID(kvp.Value, accountIDs);
                    if (kvp.Key.Equals(CashManagementEnums.OperationMode.DailyProcess))
                    {
                        updatedBalDate = GetRevalDateDailyProcess(balDate);
                    }
                    if (!string.IsNullOrWhiteSpace(operationModeWiseAccountIDs))
                        result = await _cashManagementServices.InnerChannel.RunRevaluationProcess(fromDate, updatedBalDate, userID, operationModeWiseAccountIDs, isManualRevaluation, _isGetAccountBalancesClicked).ConfigureAwait(false);
                }
                Logger.LoggerWrite("RunRevaluationProcess Complete. Total time taken by RunRevaluationProcess:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                LogMessage(LoggingConstants.RESPONSE_RECEIVED, sourceType, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the is revaluation in progress.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <returns></returns>
        public bool GetIsRevaluationInProgress(string accountIds)
        {
            try
            {
                if (_cashManagementServices != null && _cashManagementServices.InnerChannel != null)
                    return _cashManagementServices.InnerChannel.GetIsRevaluationInProgress(accountIds);
            }
            catch
            {
                throw new Exception("TradeService not connected");
            }
            return false;
        }

        /// <summary>
        /// Gets the last calculation balance details.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, BalanceUpdateDetail> GetLastCalculationBalanceDetails()
        {
            try
            {
                return _cashManagementServices.InnerChannel.GetLastCalcBalanceDetails();
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
            return null;
        }

        /// <summary>
        /// Gets the last calculation revaluation date.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RevaluationUpdateDetail> GetLastCalculationRevaluationDate()
        {
            try
            {
                return _cashManagementServices.InnerChannel.GetLastCalcRevalutionDetail();
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
            return null;
        }

        /// <summary>
        /// Gets the last calculation revaluation date.
        /// </summary>
        /// <returns></returns>
        public string GetInvalidFundsForSymbolLevel(string fundIds, Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            try
            {
                return _cashManagementServices.InnerChannel.GetInvalidFundsForSymbolLevel(fundIds, startDate, endDate);
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
            return null;
        }
        /// <summary>
        /// Get filter accountId according to operation mode
        /// </summary>
        /// <param name="accountIDs"></param>
        /// //csv to list
        /// //list to csv and return
        /// <returns></returns>
        private string GetFilterAccountID(List<int> lstaccount, string accountIDs)
        {
            string csvCommonAccounts = string.Empty;
            try
            {
                if (lstaccount != null && lstaccount.Count > 0 && !string.IsNullOrEmpty(accountIDs))
                {
                    List<int> accountIdList = accountIDs.Split(',').Select(int.Parse).ToList();
                    List<int> lstCommonAccounts = new List<int>(lstaccount.Intersect(accountIdList));
                    csvCommonAccounts = string.Join(",", lstCommonAccounts.Select(x => x.ToString()).ToArray());
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
            return csvCommonAccounts;
        }
        /// <summary>
        /// It will returns the Revaluation date for Accounts of Daily Process Operation Mode
        /// </summary>
        /// <param name="balDate">selected date by the user from Client UI</param>
        /// <returns></returns>
        private DateTime GetRevalDateDailyProcess(DateTime balDate)
        {
            DateTime revalDate = DateTime.MinValue;
            try
            {
                string dailyProcessDaysStr = CachedDataManager.GetInstance.GetPranaPreferenceByKey("RevaluationDailyProcessDays");
                DateTime dateCurrent = DateTime.Now;
                DateTime TNRevalDate = dateCurrent.AddDays(-int.Parse(dailyProcessDaysStr));

                if (balDate.Date < TNRevalDate.Date)
                    revalDate = TNRevalDate;
                else
                    revalDate = balDate;
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
            return revalDate;
        }

        #endregion

        #region Cash Transactions

        /// <summary>
        /// Saves the imorted cash dividend.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DataSet SaveImortedCashDividend(DataSet ds)
        {
            try
            {
                return _cashManagementServices.InnerChannel.SaveManualCashDividend(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Saves the manual cash dividend.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        internal DataSet SaveManualCashDividend(DataSet dataSet)
        {
            DataSet ds = null;
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.SAVE_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
                ds = _cashManagementServices.InnerChannel.SaveManualCashDividend(dataSet);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.SAVE_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        /// Gets the cash dividends for given dates.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="commaSeparatedAccountIds">The comma separated account ids.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns></returns>
        internal DataSet GetCashDividendsForGivenDates(string symbol, string commaSeparatedAccountIds, string dateType, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                LogMessage(LoggingConstants.REQUEST_SENT, LoggingConstants.GET_CASH_TRANSACTIONS, dateFrom.ToString(), dateTo.ToString(), commaSeparatedAccountIds);
                DataSet cashDividends = _cashManagementServices.InnerChannel.GetCashDividends(symbol, commaSeparatedAccountIds, dateType, dateFrom, dateTo, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                LogMessage(LoggingConstants.RESPONSE_RECEIVED, LoggingConstants.GET_CASH_TRANSACTIONS, string.Empty, string.Empty, string.Empty);
                return cashDividends;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                }
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        #endregion
    }
}
