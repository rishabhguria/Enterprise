using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PostTradeServices
{
    public class ManualOrderSendHelper
    {
        /// <summary>
        /// The sched
        /// </summary>
        private IScheduler _sched;
        /// <summary>
        /// The sched fact
        /// </summary>
        private ISchedulerFactory _schedFact = new StdSchedulerFactory();
        /// <summary>
        /// The manual order send schedular data full
        /// </summary>
        private List<ManualOrderSendSchedularData> _manualOrderSendSchedularDataFull = null;
        /// <summary>
        /// The i sec master services
        /// </summary>
        private static ISecMasterServices _iSecMasterServices = null;

        /// <summary>
        /// The instance
        /// </summary>
        private static ManualOrderSendHelper _instance;

        protected ManualOrderSendHelper()
        {
            StartScheduler();
        }

        public static ManualOrderSendHelper Instance()
        {

            if (_instance == null)
            {
                _instance = new ManualOrderSendHelper();
            }

            return _instance;
        }

        /// <summary>
        /// Shutdowns the scheduler.
        /// </summary>
        public void ShutdownScheduler()
        {
            _sched.Standby();
        }

        /// <summary>
        /// Starts the scheduler.
        /// </summary>
        public void StartScheduler()
        {
            _sched = _schedFact.GetScheduler();
            _sched.Start();
        }

        /// <summary>
        /// Initilises the specified i sec master services.
        /// </summary>
        /// <param name="iSecMasterServices">The i sec master services.</param>
        /// <param name="companyID">The company identifier.</param>
        public void Initilise(ISecMasterServices iSecMasterServices, int companyID)
        {
            try
            {
                _iSecMasterServices = iSecMasterServices;
                StartScheduler(companyID);
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
        /// Starts the scheduler.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        public void StartScheduler(int companyId)
        {
            try
            {
                ShutdownScheduler();
                StartScheduler();

                _manualOrderSendSchedularDataFull = DataBaseManager.GetManualOrderSendSchedularDataList(companyId);

                foreach (ManualOrderSendSchedularData manualOrderSendSchedularData in _manualOrderSendSchedularDataFull.Where(mOrd => mOrd.SendManualOrderTriggerTime != DateTimeConstants.MinValue).ToList())
                {
                    JobDetail jobdetail = new JobDetail("ManualOrderSendSchedularDataJob :" + manualOrderSendSchedularData.AUECID, "AUEC-SendOrderData", typeof(ManualOrderSendSchedularDataJob));

                    jobdetail.JobDataMap["AUECID"] = manualOrderSendSchedularData.AUECID;
                    jobdetail.JobDataMap["LastManualOrderRunTriggerTime"] = manualOrderSendSchedularData.LastManualOrderRunTriggerTime;
                    jobdetail.JobDataMap["companyId"] = companyId;
                    System.TimeSpan tt = new TimeSpan(864000000000); // for whole 24day set it to 864000000000;  10000000
                    SimpleTrigger st = new SimpleTrigger();
                    st.RepeatCount = -1;
                    st.RepeatInterval = tt;
                    st.StartTimeUtc = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(manualOrderSendSchedularData.SendManualOrderTriggerTime, CachedDataManager.GetInstance.GetAUECTimeZone(manualOrderSendSchedularData.AUECID));
                    st.Name = "ManualOrderSendSchedular " + manualOrderSendSchedularData.AUECID;
                    st.Group = "AUEC-ManualOrderSendSchedular-Triggers";
                    _sched.DeleteJob("ManualOrderSendSchedularDataJob :" + manualOrderSendSchedularData.AUECID, "AUEC-SendOrderData");
                    _sched.ScheduleJob(jobdetail, st);
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

        /// <summary>
        /// Processes the manual order send.
        /// </summary>
        /// <param name="auecId">The auec identifier.</param>
        /// <param name="lastRunTime">The last run time.</param>
        /// <param name="companyId">The company identifier.</param>
        public static void ProcessManualOrderSend(int auecId, DateTime lastRunTime, int companyId)
        {
            try
            {
                List<PranaMessage> messageList = DataBaseManager.GetManualOrderDetails(auecId, lastRunTime);
                DataBaseManager.SetLastManualOrderTriggerTime(auecId, companyId);
                messageList.ForEach(pMsg =>
                {
                    _iSecMasterServices.SetSecuritymasterDetails(pMsg);
                });
                FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().SendManualOrdersToThirdPartyFixLine(messageList);
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
        /// Processes all auec manual orders.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        public void ProcessAllAuecManualOrders(int companyId)
        {
            try
            {
                if (FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().GetFixConnection(-1) == null)
                {
                    try
                    {
                        Logger.HandleException(new Exception("Manual Drops Counterparty disconnected"), LoggingConstants.POLICY_LOGANDSHOW);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                if (_manualOrderSendSchedularDataFull == null || _manualOrderSendSchedularDataFull.Count <= 0)
                    _manualOrderSendSchedularDataFull = DataBaseManager.GetManualOrderSendSchedularDataList(companyId);
                List<PranaMessage> messageList = new List<PranaMessage>();
                _manualOrderSendSchedularDataFull.ForEach(ordobj =>
                {
                    if (ordobj.PermitManualOrderSend)
                    {
                        messageList.AddRange(DataBaseManager.GetManualOrderDetails(ordobj.AUECID, ordobj.LastManualOrderRunTriggerTime));
                        ordobj.LastManualOrderRunTriggerTime = DateTime.UtcNow;
                        DataBaseManager.SetLastManualOrderTriggerTime(ordobj.AUECID, companyId);
                    }
                });
                messageList.ForEach(pMsg =>
                {
                    _iSecMasterServices.SetSecuritymasterDetails(pMsg);
                });
                FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().SendManualOrdersToThirdPartyFixLine(messageList);
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

    }
}
