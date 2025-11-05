using Prana.ActivityHandler.DAL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.QueueManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Prana.ActivityHandler
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ActivityServices : IActivityServices, IDisposable
    {
        #region Services Ref

        private ICashManagementService _cashManagementServices;
        public ICashManagementService CashManagementServices
        {
            set
            {
                _cashManagementServices = value;
                ServiceProxyConnector.CashManagementServices = value;
                _cashManagementServices.SetActicityServices(this);
            }
        }

        IClosingServices _closingServices = null;
        public IClosingServices ClosingServices
        {
            set
            {
                _closingServices = value;
                ServiceProxyConnector.ClosingServices = value;
                _closingServices.SetActicityServices(this);
            }
        }

        public IFixedIncomeAdapter FixedIncomeAdapter
        {
            set
            {
                ServiceProxyConnector.FixedIncomeAdapter = value;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The fund wise MSM queue
        /// </summary>
        private static Dictionary<string, MSMQQueueManager> _fundWiseMSMQueue = null;

        /// <summary>
        /// The fund wise MSM queue locker
        /// </summary>
        private static readonly object _fundWiseMSMQueueLocker = new object();

        /// <summary>
        /// The reval running for fund i ds activity services
        /// </summary>
        public static List<string> _revalRunningForFundIDsActivityServices = null;

        /// <summary>
        /// The cash activity queue fund wise
        /// </summary>
        private MSMQQueueManager _cashActivityQueueFundWise = null;

        private BufferBlock<List<AllocationGroup>> allocationDataBuffer;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityServices"/> class.
        /// </summary>
        public ActivityServices()
        {
            CreatePublishingProxy();

            ServiceProxyConnector.AllocationServices = new ProxyBase<IAllocationServices>("TradeAllocationServiceEndpointAddress");
        }

        /// <summary>
        /// Initializes the cache and other state of the manager
        /// </summary>
        public void Initialize()
        {
            try
            {
                allocationDataBuffer = new BufferBlock<List<AllocationGroup>>();
                System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(allocationDataBuffer)).ConfigureAwait(false);
                lock (_fundWiseMSMQueueLocker)
                {
                    _fundWiseMSMQueue = new Dictionary<string, MSMQQueueManager>();
                    _revalRunningForFundIDsActivityServices = new List<string>();
                    ProcessCashActivityQueuesOnServerStartup();
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
        /// Consume Buffer List<AllocationGroup> Async being set by Publishing helper from Allocation.Core
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        async System.Threading.Tasks.Task<List<AllocationGroup>> ConsumeBufferMessageAsync(IReceivableSourceBlock<List<AllocationGroup>> source)
        {
            try
            {
                while (await source.OutputAvailableAsync())
                {
                    List<AllocationGroup> groupList;
                    while (source.TryReceive(out groupList))
                    {
                        List<TaxLot> taxlotsList = (from g in groupList
                                                    from t in g.GetAllTaxlots()
                                                    select t).ToList();
                        GenerateCashActivity(taxlotsList, CashTransactionType.Trading);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        public void GenerateTradingCashActivity(List<AllocationGroup> groups)
        {
            try
            {
                BufferAllocGroups(allocationDataBuffer,groups);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }



        /// <summary>
        /// Generates the cash activity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <param name="callingFrom">The calling from.</param>
        /// <returns></returns>
        public List<CashActivity> GenerateCashActivity<T>(T data, CashTransactionType activitySource, CashTransactionType? callingFrom = null)
        {
            List<CashActivity> lsCashActivity = null;
            List<CashActivity> lsCashActivityClosing = null;
            try
            {
                lsCashActivity = ActivityHelperClass.CreateCashActivity(data, activitySource);
                lsCashActivityClosing = SaveActivityForClosedTaxlots(data, activitySource);
                if (lsCashActivity != null && lsCashActivityClosing != null && lsCashActivityClosing.Count > 0)
                {
                    lsCashActivity.AddRange(lsCashActivityClosing);
                }
                else if (lsCashActivityClosing != null && lsCashActivityClosing.Count > 0)
                {
                    lsCashActivity = lsCashActivityClosing;
                }
                if (lsCashActivity != null && lsCashActivity.Count > 0)
                {
                    if (callingFrom != null && callingFrom == CashTransactionType.Revaluation)
                    {
                        //-1 userId is used to indicate server internal call
                        SaveActivity(lsCashActivity, -1, false);
                    }
                    else
                    {
                        foreach (CashActivity activity in lsCashActivity)
                        {
                            int accountID = activity.AccountID;
                            if (IsRequiredToQueue(accountID))
                            {
                                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_CASHACTIVITY_QUEUE, activity);
                                GetFundMSMQueue(activity.AccountID.ToString(), true).SendMessage(qMsg);
                            }
                            else
                            {
                                List<CashActivity> templsCashActivity = new List<CashActivity>();
                                templsCashActivity.Add(activity);
                                //-1 userId is used to indicate server internal call
                                SaveActivity(templsCashActivity, -1, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsCashActivity;
        }

        /// <summary>
        /// Processes the cash activity queue.
        /// </summary>
        /// <param name="accountIDs">The account i ds.</param>
        public void ProcessCashActivityQueue(string accountIDs)
        {
            try
            {
                string[] accountIDsArray = accountIDs.Split(',');
                foreach (string accountID in accountIDsArray)
                {
                    MSMQQueueManager queue = GetFundMSMQueue(accountID, false);
                    if (queue != null)
                    {
                        queue.StartListeningCashActivityQueue();
                    }
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
        /// Creates the activity.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <returns></returns>
        public List<CashActivity> CreateActivity(List<TaxLot> data, CashTransactionType activitySource)
        {
            try
            {
                List<CashActivity> activities = ActivityHelperClass.CreateCashActivity(data, activitySource);
                return activities;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Creates the activity.
        /// PRANA-9776 New Parameter UserId 
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="activitySource">The activity source.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public List<CashActivity> CreateActivity(DataSet data, CashTransactionType activitySource, int userId, bool isSymbolLevelAccruals)
        {
            try
            {
                int index = isSymbolLevelAccruals ? 1 : 0;
                DataSet dtActivity = ActivityHelperClass.CreateCashActivityDatatable(data, userId, isSymbolLevelAccruals);

                DataTable tblDeletedActivity = new DataTable();
                DataTable tblNewActivity = new DataTable();

                if (dtActivity.Tables[index].Rows.Count > 0)
                {
                    var Deletedrows = dtActivity.Tables[index].AsEnumerable().Where(row => row.Field<string>("ActivityState") == "Deleted");
                    if (Deletedrows.Any())
                        tblDeletedActivity = Deletedrows.CopyToDataTable();
                    var Newrows = dtActivity.Tables[index].AsEnumerable().Where(row => row.Field<string>("ActivityState") == "New");
                    if (Newrows.Any())
                        tblNewActivity = Newrows.CopyToDataTable();


                    string FKIDlist = "";
                    if (tblDeletedActivity.Rows.Count > 0)
                        FKIDlist = string.Join(",", tblDeletedActivity.AsEnumerable().Select(r => r.Field<string>("FKID")).ToArray());

                    //PRANA-9776
                    if (!isSymbolLevelAccruals)
                    {
                        ActivityDataManager.SaveBulkActivity(tblNewActivity, FKIDlist);
                    }

                    //List<CashActivity> lsCashActivity = ActivityHelperClass.CreateCashActivity(data, activitySource);
                    List<CashActivity> lsCashActivity = tblNewActivity.AsEnumerable().Select
                        (dr => new CashActivity
                        {
                            ActivityTypeId = Convert.ToInt32(dr["ActivityTypeId_FK"]),
                            AccountID = Convert.ToInt32(dr["FundID"]),
                            TransactionSource = (CashTransactionType)(Convert.ToInt32(dr["TransactionSource"])),
                            ActivitySource = (ActivitySource)(Convert.ToInt32(dr["ActivitySource"])),
                            Symbol = Convert.ToString(dr["Symbol"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            CurrencyID = Convert.ToInt32(dr["CurrencyID"]),
                            Description = Convert.ToString(dr["Description"]),
                            SideMultiplier = Convert.ToInt32(dr["SideMultiplier"]),
                            Date = Convert.ToDateTime(dr["TradeDate"]),
                            FXRate = dr["FxRate"] != DBNull.Value ? Convert.ToDouble(dr["FxRate"]) : 0,
                            FXConversionMethodOperator = dr["FXConversionMethodOperator"] != DBNull.Value ? Convert.ToString(dr["FXConversionMethodOperator"]) : string.Empty,
                            FKID = dr["FKID"] != DBNull.Value ? Convert.ToString(dr["FKID"]) : string.Empty,
                            ActivityId = Convert.ToString(dr["ActivityID"]),
                            UniqueKey = Convert.ToString(dr["UniqueKey"]),
                            ActivityNumber = Convert.ToInt32(dr["ActivityNumber"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            ActivityState = Convert.ToString(dr["ActivityState"]).Equals("New") ? ApplicationConstants.TaxLotState.New : ApplicationConstants.TaxLotState.Deleted,
                            SubAccountID = (dtActivity.Tables[0].Columns.Contains("SubAccountID") && dr["SubAccountID"] != DBNull.Value) ? Convert.ToInt32(dr["SubAccountID"]) : 0,
                            UserId = Convert.ToInt32(dr["UserId"])//PRANA-9776
                        }
                        ).ToList();

                    if (lsCashActivity != null && lsCashActivity.Count > 0)
                    {
                        //-1 userId is used to indicate server internal call
                        SaveActivity(lsCashActivity, -1, isSymbolLevelAccruals);
                        return lsCashActivity;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Get the list of activities for the given date range and the accounts passed as the string of the comma separated values
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public List<CashActivity> GetActivity(DateTime fromDate, DateTime toDate, String accountIDs, int userId, String activityDateType, bool isActivitySource)
        {
            try
            {
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_ACTIVITY, fromDate.ToString(), toDate.ToString(), string.Empty);
                List<CashActivity> lsCashActivity = ActivityDataManager.GetActivity(fromDate, toDate, accountIDs, activityDateType, isActivitySource);
                NameValueFiller.SetNameValues(lsCashActivity);
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_ACTIVITY, string.Empty, string.Empty, string.Empty);
                return lsCashActivity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the activities by activity identifier.
        /// </summary>
        /// <param name="activityIds">The activity ids.</param>
        /// <returns></returns>
        public List<CashActivity> GetActivitiesByActivityId(string activityIds)
        {
            try
            {
                List<CashActivity> lsCashActivity = ActivityDataManager.GetActivitiesByActivityId(activityIds);
                NameValueFiller.SetNameValues(lsCashActivity);
                return lsCashActivity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Get All Activities of the same TransactionId
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<CashActivity> GetActivitiesByTransactionId(string transactionId)
        {
            try
            {
                List<CashActivity> lsCashActivity = ActivityDataManager.GetActivitiesByTransactionId(transactionId);
                NameValueFiller.SetNameValues(lsCashActivity);
                return lsCashActivity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Saves the activity.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public int SaveActivity(List<CashActivity> lsCashActivity, int userId, bool isSymbolLevelAccruals)
        {
            try
            {
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.SAVE_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);

                lock (_locker)
                {
                    if (lsCashActivity != null && lsCashActivity.Count > 0)
                    {
                        if (lsCashActivity[0].TransactionSource != CashTransactionType.Revaluation)
                        {
                            ActivityDataManager.SaveActivity(lsCashActivity);
                        }
                        _cashManagementServices.GenerateJournalEntry(lsCashActivity, userId, isSymbolLevelAccruals);
                        if (!isSymbolLevelAccruals)
                        {
                            NameValueFiller.SetNameValues(lsCashActivity);
                            PublishCashActivity(lsCashActivity);
                        }
                    }
                }
                if (userId != -1)
                    LogMessage(userId, LoggingConstants.RESPONSE_SENT, LoggingConstants.SAVE_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);

                if (lsCashActivity == null)
                {
                    throw new ArgumentNullException(nameof(lsCashActivity));
                }
                  
                return lsCashActivity.Count;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return 0;
        }


        /// <summary>
        /// Saves the activities for MTM only
        /// PRANA-22141
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> SaveActivityforMTM(List<CashActivity> lsCashActivity, int userID)
        {
            LogMessage(userID, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.SAVE_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
            if (lsCashActivity.Count > 0 && !(lsCashActivity[0].ActivityState == ApplicationConstants.TaxLotState.Deleted))
            {
                System.Threading.Tasks.Task<int> rslt = System.Threading.Tasks.Task<int>.Factory.StartNew(() => _cashManagementServices.SaveCashActionsforMTM(userID));
            }
            int result = await System.Threading.Tasks.Task.Run(() => { return SaveActivityAsync(lsCashActivity); });
            LogMessage(userID, LoggingConstants.RESPONSE_SENT, LoggingConstants.SAVE_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
            return result;
        }

        /// <summary>
        /// Get daily cash data for selected accounts
        /// Modified by: Bharat Raturi, 16 Sep 2014
        /// purpose: to get the records for selected accounts by sending the comma separated string of account ids
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountIDs">Comma separated string of account IDs</param>
        /// <returns>list of cash activities</returns>
        public async Task<List<CashActivity>> GetAlreadyCalculatedDailyCashData(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_PRECALCULATED_DAILY_CALCULATIONS, fromDate.ToString(), toDate.ToString(), accountIDs);
            List<CashActivity> lsAlreadyCalculatedCashData = await System.Threading.Tasks.Task.Run(() => { return GetAlreadyCalculatedDailyCashDataAsync(fromDate, toDate, accountIDs); });
            LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_PRECALCULATED_DAILY_CALCULATIONS, string.Empty, string.Empty, string.Empty);
            return lsAlreadyCalculatedCashData;
        }

        /// <summary>
        /// Modified by: Bharat raturi, 1- aug 2014
        /// purpose: filter the activities at the DB level according to the cash management start dates of the respective accounts
        /// Get the activity exceptions
        /// </summary>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">end date</param>
        /// <param name="accountIDs">comma separated accountIDs</param>
        /// <returns></returns>
        public List<CashActivity> GetActivityExceptions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_ACTIVITY_EXCEPTIONS, fromDate.ToString(), toDate.ToString(), accountIDs);
                List<CashActivity> activities = ActivityDataManager.GetActivityExceptions(fromDate, toDate, accountIDs);
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_ACTIVITY_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
                return activities;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the overriding activity.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<CashActivity> GetOverridingActivity(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GET_OVERRIDING_ACTIVITY, fromDate.ToString(), toDate.ToString(), accountIDs);
                List<CashActivity> activities = ActivityDataManager.GetOverridingActivity(fromDate, toDate, accountIDs);
                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GET_OVERRIDING_ACTIVITY, string.Empty, string.Empty, string.Empty);
                return activities;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
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
            try
            {
                LogMessage(-1, LoggingConstants.REQUEST_RECEIVED, LoggingConstants.GENERATE_JOURNAL_EXCEPTIONS, fromDate.ToString(), toDate.ToString(), accountIDs);
                long activitiesCount = 0;
                DataSet ds = ActivityDataManager.SaveManualJournalExceptions(fromDate, toDate, accountIDs);

                if (ds.Tables.Count > 0)
                {
                    DataTable dt_Activities = ds.Tables[0];  // for deafult 
                    //TODO: Surendra :Comment
                    if (ds.Tables.Count == 3)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)  // server side activitytype
                            CachedDataManager.GetActivityType().Add(row[1].ToString().Trim(), int.Parse(row[0].ToString().Trim()));//(FillKeyValuePair(row, 0));

                        foreach (DataRow row in ds.Tables[1].Rows)   //server side activityjournalmapping
                            CachedDataManager.GetActivityJournalMapping().Rows.Add(row.ItemArray);

                        PublishActivityType(ds.Tables[0]);
                        PublishActivityTypeMapping(ds.Tables[1]);
                        dt_Activities = ds.Tables[2];
                    }
                    activitiesCount = dt_Activities.Rows.Count;
                    List<CashActivity> activities = new List<CashActivity>();

                    #region Listing Activities for Subscription

                    /* if any field added to activity further nedd to set 0 in sp. 
                        not handled the code in Code for null values. Made this keeping in mind 
                            Nothing is Null in activity table. */

                    foreach (DataRow dr in dt_Activities.Rows)
                    {
                        activities.Add(new CashActivity()
                        {
                            ActivityId = dr["ActivityId"].ToString(),
                            ActivityTypeId = int.Parse(dr["ActivityTypeId_FK"].ToString()),
                            FKID = dr["FKID"].ToString(),
                            BalanceType = (BalanceType)int.Parse(dr["BalanceType"].ToString()),
                            Symbol = dr["Symbol"].ToString(),
                            AccountID = int.Parse(dr["AccountID"].ToString()),
                            Date = Convert.ToDateTime(dr["TradeDate"].ToString()),
                            SettlementDate = Convert.ToDateTime(dr["SettlementDate"].ToString()),
                            CurrencyID = int.Parse(dr["CurrencyID"].ToString()),
                            LeadCurrencyID = int.Parse(dr["LeadCurrencyID"].ToString()),
                            VsCurrencyID = int.Parse(dr["VsCurrencyID"].ToString()),
                            ClosedQty = decimal.Parse(dr["ClosedQty"].ToString()),
                            Amount = decimal.Parse(dr["Amount"].ToString()),
                            Commission = decimal.Parse(dr["Commission"].ToString()),
                            SoftCommission = decimal.Parse(dr["SoftCommission"].ToString()),
                            OtherBrokerFees = decimal.Parse(dr["OtherBrokerFees"].ToString()),
                            ClearingBrokerFee = decimal.Parse(dr["ClearingBrokerFee"].ToString()),
                            StampDuty = decimal.Parse(dr["StampDuty"].ToString()),
                            TransactionLevy = decimal.Parse(dr["TransactionLevy"].ToString()),
                            ClearingFee = decimal.Parse(dr["ClearingFee"].ToString()),
                            TaxOnCommissions = decimal.Parse(dr["TaxOnCommissions"].ToString()),
                            MiscFees = decimal.Parse(dr["MiscFees"].ToString()),
                            OptionPremiumAdjustment = dr["OptionPremiumAdjustment"] is DBNull ? Convert.ToDecimal(0.0) : decimal.Parse(dr["OptionPremiumAdjustment"].ToString()),
                            SecFee = decimal.Parse(dr["SecFee"].ToString()),
                            OccFee = decimal.Parse(dr["OccFee"].ToString()),
                            OrfFee = decimal.Parse(dr["OrfFee"].ToString()),
                            FXRate = double.Parse(dr["FXRate"].ToString()),
                            FXConversionMethodOperator = dr["FXConversionMethodOperator"].ToString(),
                            ActivitySource = (ActivitySource)(int.Parse(dr["ActivitySource"].ToString())),
                            TransactionSource = (CashTransactionType)(int.Parse(dr["TransactionSource"].ToString())),
                            ActivityNumber = int.Parse(dr["ActivityNumber"].ToString()),
                            Description = dr["Description"].ToString(),
                            SubActivity = dr["SubActivity"].ToString(),
                            SideMultiplier = int.Parse(dr["SideMultiplier"].ToString()),
                            //PRANA-9777
                            ModifyDate = dr["ModifyDate"] is DBNull ? DateTimeConstants.MinValue : Convert.ToDateTime(dr["ModifyDate"]),
                            EntryDate = dr["EntryDate"] is DBNull ? DateTimeConstants.MinValue : Convert.ToDateTime(dr["EntryDate"]),
                            //PRANA-9776
                            UserId = dr["UserId"] is DBNull ? 0 : Convert.ToInt32(dr["UserId"]),
                            ActivityState = ApplicationConstants.TaxLotState.Updated   // it is needed for modified activity to be displayed through subscription.Works same as Activitystate_New with additional replace exiting activity with modified on Activity Tab. 
                        }
                        );
                    }
                    #endregion

                    NameValueFiller.SetNameValues(activities);
                    PublishHistoricalActivity(activities);
                }

                LogMessage(-1, LoggingConstants.RESPONSE_SENT, LoggingConstants.GENERATE_JOURNAL_EXCEPTIONS, string.Empty, string.Empty, string.Empty);
                return activitiesCount;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return -1;
        }

        /// <summary>
        /// Adds to reval running cache.
        /// </summary>
        /// <param name="accountIDsArray">The account i ds array.</param>
        public void AddToRevalRunningCache(string[] accountIDsArray)
        {
            try
            {
                _revalRunningForFundIDsActivityServices.AddRange(accountIDsArray);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes from reval running cache.
        /// </summary>
        /// <param name="accountIDsArray">The account i ds array.</param>
        public void DeleteFromRevalRunningCache(string[] accountIDsArray)
        {
            try
            {
                _revalRunningForFundIDsActivityServices = _revalRunningForFundIDsActivityServices.Except(accountIDsArray).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region IPublishing Members

        /// <summary>
        /// The publish lock
        /// </summary>
        private static readonly object _publishLock = new object();

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private void CreatePublishingProxy()
        {
            _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
        }

        /// <summary>
        /// Publishes the cash activity.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        private void PublishCashActivity(List<CashActivity> lsCashActivity)
        {
            try
            {
                if (lsCashActivity != null && lsCashActivity.Count > 0)
                {
                    if ((lsCashActivity[0].TransactionSource.ToString().Equals(CashTransactionType.Revaluation.ToString())))
                    {
                        List<object> data = new List<object>();
                        var publishList = lsCashActivity.Where(act => (_cashManagementServices.GetCashPreferences().ContainsKey(act.AccountID) && _cashManagementServices.GetCashPreferences()[act.AccountID].IsPublishRevaluationData)).ToList();
                        if (publishList != null)
                        {
                            data.AddRange(publishList);
                        }
                        if (publishList == null || publishList.Count != lsCashActivity.Count)
                        {
                            data.Add("Revaluation is done but some or all of the data is not published, click on get data to fetch data.");
                        }
                        MessageData e = new MessageData();
                        e.EventData = data;
                        e.TopicName = Topics.Topic_RevaluationActivity;
                        //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_RevaluationActivity);
                        CentralizePublish(e);
                    }
                    else
                    {
                        MessageData e = new MessageData();
                        e.EventData = lsCashActivity;
                        e.TopicName = Topics.Topic_CashActivity;
                        //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_CashActivity);
                        CentralizePublish(e);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the historical activity.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        public static void PublishHistoricalActivity(List<CashActivity> lsCashActivity)
        {
            MessageData e = new MessageData();
            e.EventData = lsCashActivity;
            e.TopicName = Topics.Topic_HistoricalJournalActivity;
            // _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_HistoricalJournalActivity);
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the type of the activity.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static void PublishActivityType(DataTable dt)
        {
            MessageData e = new MessageData();
            e.EventData = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetListFromDataTable(dt);
            e.TopicName = Topics.Topic_ActivityType;
            // _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_ActivityType);
            CentralizePublish(e);
        }

        /// <summary>
        /// Publishes the activity type mapping.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static void PublishActivityTypeMapping(DataTable dt)
        {
            MessageData e = new MessageData();
            e.EventData = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetListFromDataTable(dt);
            e.TopicName = Topics.Topic_ActivityJournalMapping;
            // _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_ActivityJournalMapping);
            CentralizePublish(e);
        }

        /// <summary>
        /// Centralizes the publish.
        /// </summary>
        /// <param name="msgData">The MSG data.</param>
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            if (rethrow)
                                throw;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
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
            if (_cashActivityQueueFundWise != null && isDisposing)
            {
                _cashActivityQueueFundWise.Dispose();
                _cashActivityQueueFundWise = null;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="action">The action.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        private void LogMessage(int userId, string action, string tabName, string fromDate, string toDate, String accountIDs)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsGeneralLedgerLoggingRequired"]))
                {
                    string user = (userId == -1) ? string.Empty : CachedDataManager.GetInstance.GetUserText(userId);
                    LogExtensions.LogMessage(PranaModules.GENERAL_LEDGER_MODULE, user, action, tabName, fromDate, toDate, accountIDs);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the already calculated daily cash data asynchronous.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        private List<CashActivity> GetAlreadyCalculatedDailyCashDataAsync(DateTime fromDate, DateTime toDate, String accountIDs)
        {
            List<CashActivity> lsAlreadyCalculatedCashData = null;
            try
            {
                lsAlreadyCalculatedCashData = ActivityDataManager.GetAlreadyCalculatedDailyCashData(fromDate, toDate, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return lsAlreadyCalculatedCashData;
        }

        /// <summary>
        /// Saves the activity asynchronous.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        /// <returns></returns>
        private int SaveActivityAsync(List<CashActivity> lsCashActivity)
        {
            try
            {
                lock (_locker)
                {
                    if (lsCashActivity != null && lsCashActivity.Count > 0)
                    {
                        if (lsCashActivity[0].TransactionSource != CashTransactionType.Revaluation)
                        {
                            ActivityDataManager.SaveActivity(lsCashActivity);
                        }
                        _cashManagementServices.GenerateJournalEntry(lsCashActivity, -1, false);
                        NameValueFiller.SetNameValues(lsCashActivity);
                        PublishCashActivity(lsCashActivity);
                    }
                    else
                    {
                        Logger.LoggerWrite("lsCashActivity is null in SaveActivityAsync of ActivityServices.cs", LoggingConstants.CATEGORY_INFORMATION);
                        return 0;
                    }
                }
                return lsCashActivity.Count;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return 0;
        }

        /// <summary>
        /// Processes the cash activity queues on server startup.
        /// </summary>
        private void ProcessCashActivityQueuesOnServerStartup()
        {
            try
            {
                Dictionary<int, string> dictFunds = CachedDataManager.GetInstance.GetAccounts();
                List<int> lstAllAccountIds = dictFunds.Keys.ToList();
                foreach (int accountID in lstAllAccountIds)
                {
                    string queueName = ConfigurationManager.AppSettings[PranaServerConstants.CASHACTIVITY_QUEUE_PATH].ToString() + "_" + Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString() + "_" + accountID;
                    if (MessageQueue.Exists(queueName))
                    {
                        _cashActivityQueueFundWise = new MSMQQueueManager(queueName);
                        _cashActivityQueueFundWise.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(cashActivityQueue_MessageQueued);
                        _fundWiseMSMQueue.Add(accountID.ToString(), _cashActivityQueueFundWise);
                        _cashActivityQueueFundWise.StartListeningCashActivityQueue();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the MessageQueued event of the cashActivityQueue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{QueueMessage}"/> instance containing the event data.</param>
        private void cashActivityQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                if (message.MsgType.Equals(CustomFIXConstants.MSG_CASHACTIVITY_QUEUE))
                {
                    CashActivity cashActivity = message.Message as CashActivity;
                    if (cashActivity != null)
                    {
                        List<CashActivity> tempLsCashActivity = new List<CashActivity>();
                        cashActivity.ActivityId = uIDGenerator.GenerateID();
                        tempLsCashActivity.Add(cashActivity);
                        //-1 userId is used to indicate server internal call
                        SaveActivity(tempLsCashActivity, -1, false);
                    }
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
        /// Saves the activity for closed taxlots.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="activitysource">The activitysource.</param>
        /// <returns></returns>
        private List<CashActivity> SaveActivityForClosedTaxlots(object data, CashTransactionType activitysource)
        {
            try
            {
                List<Position> netPositions = null;
                string taxLotIds = string.Empty;
                List<CashActivity> lsCashActivity = null;
                if (activitysource.Equals(CashTransactionType.Trading))
                {
                    List<TaxLot> lsTaxlots = data as List<TaxLot>;
                    if (lsTaxlots != null)
                    {
                        foreach (TaxLot t in lsTaxlots)
                        {
                            if ((t.ClosingStatus.Equals(ClosingStatus.Closed) || t.ClosingStatus.Equals(ClosingStatus.PartiallyClosed) && t.TaxLotState.Equals(ApplicationConstants.TaxLotState.Updated))
                                && ((t.AssetID == (int)AssetCategory.Future) || (t.AssetID == (int)AssetCategory.FutureOption) || (t.AssetID == (int)AssetCategory.FX)
                                || (t.AssetID == (int)AssetCategory.FXForward) || (t.AssetID == (int)AssetCategory.FXForward)))
                            {
                                taxLotIds += t.TaxLotID.ToString() + Seperators.SEPERATOR_8;
                            }
                        }
                    }
                }
                if (taxLotIds.Length > 0)
                {
                    netPositions = _closingServices.GetNetPositionsForTaxlotIds(taxLotIds);
                    lsCashActivity = ActivityHelperClass.CreateCashActivity(netPositions, CashTransactionType.Closing);
                    return lsCashActivity;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the fund MSM queue.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="isCreateNewMSMQ">if set to <c>true</c> [is create new MSMQ].</param>
        /// <returns></returns>
        private MSMQQueueManager GetFundMSMQueue(string accountID, bool isCreateNewMSMQ)
        {
            try
            {
                lock (_fundWiseMSMQueueLocker)
                {
                    if (_fundWiseMSMQueue.ContainsKey(accountID))
                    {
                        return _fundWiseMSMQueue[accountID];
                    }
                    else if (isCreateNewMSMQ)
                    {
                        _cashActivityQueueFundWise = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.CASHACTIVITY_QUEUE_PATH].ToString() + "_" + Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString() + "_" + accountID);
                        _cashActivityQueueFundWise.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(cashActivityQueue_MessageQueued);
                        _fundWiseMSMQueue.Add(accountID, _cashActivityQueueFundWise);
                        return _cashActivityQueueFundWise;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Determines whether [is required to queue] [the specified account identifier].
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is required to queue] [the specified account identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRequiredToQueue(int accountID)
        {
            bool isRequiredToQueue = false;
            try
            {
                MSMQQueueManager fundMSMQ = GetFundMSMQueue(accountID.ToString(), false);
                if (_revalRunningForFundIDsActivityServices.Contains(accountID.ToString()))
                {
                    isRequiredToQueue = true;
                }
                else if (fundMSMQ != null && MSMQQueueManager.IsContainsMessages(fundMSMQ._queue))
                {
                    isRequiredToQueue = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isRequiredToQueue;
        }

        /// <summary>
        /// Buffer Message
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        void BufferAllocGroups(ITargetBlock<List<AllocationGroup>> target, List<AllocationGroup> allocGroups)
        {
            try
            {
                target.Post(allocGroups);
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion
    }
}
