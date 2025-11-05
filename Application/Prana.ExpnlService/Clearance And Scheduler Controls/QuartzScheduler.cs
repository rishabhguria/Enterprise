using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    internal class QuartzScheduler
    {
        private static QuartzScheduler _quartzScheduler = null;
        private IScheduler _sched;
        private ISchedulerFactory _schedFact = new StdSchedulerFactory();
        private int _scheduldeAuec;

        public QuartzScheduler()
        {
            // get a scheduler
            StartScheduler();
        }

        public static QuartzScheduler GetInstance()
        {
            if (_quartzScheduler == null)
            {
                _quartzScheduler = new QuartzScheduler();
            }
            return _quartzScheduler;
        }

        public Dictionary<List<int>, DateTime> CollapseEventsAtSameTime(Dictionary<int, DateTime> sortedAuecClearanceTimeDict)
        {
            List<KeyValuePair<int, DateTime>> result = new List<KeyValuePair<int, DateTime>>(sortedAuecClearanceTimeDict);
            Dictionary<List<int>, DateTime> finalDict = new Dictionary<List<int>, DateTime>();

            for (int i = 0; i < result.Count;)
            {
                List<int> temp = new List<int>();
                // temp.Add(result[i].Key);
                int j;
                for (j = i; j < result.Count; j++)
                {
                    if (result[i].Value == result[j].Value)
                    {
                        temp.Add(result[j].Key);
                    }
                    else
                    {
                        break;
                    }
                }
                finalDict.Add(temp, result[i].Value);
                i = j;
            }
            return finalDict;
        }

        // create an function that add the trigger corresponding to the auecid.
        //protoype could be  to create the trigger corresponding to auec time.
        public void CreateTriggerForAuecsClearance(Dictionary<int, DateTime> auecClearanceTimeInUtc, List<int> _inUseAUECIDs)
        {
            Dictionary<int, DateTime> UpdatedauecClearanceTimeInUtc = new Dictionary<int, DateTime>();
            foreach (KeyValuePair<int, DateTime> kvp in auecClearanceTimeInUtc)
            {
                if (_inUseAUECIDs.Contains(kvp.Key))
                {
                    UpdatedauecClearanceTimeInUtc.Add(kvp.Key, kvp.Value);
                }
            }

            ShutdownScheduler();
            StartScheduler();
            Dictionary<int, DateTime> sortedAuecClearanceTimeDict = new Dictionary<int, DateTime>();
            sortedAuecClearanceTimeDict = SortDictionary(UpdatedauecClearanceTimeInUtc);
            Dictionary<List<int>, DateTime> consolidatedclearanceDateTimeDict = new Dictionary<List<int>, DateTime>();
            consolidatedclearanceDateTimeDict = CollapseEventsAtSameTime(sortedAuecClearanceTimeDict);
            _scheduldeAuec = 0;

            foreach (KeyValuePair<List<int>, DateTime> auecClearanceTime in consolidatedclearanceDateTimeDict)
            {
                _scheduldeAuec++;
                if (_sched.GetJobDetail("ClearanceJobForAuec :" + _scheduldeAuec.ToString(), "AUEC-Clearance-Jobs") != null)
                {
                    _sched.DeleteJob("ClearanceJobForAuec :" + auecClearanceTime.Key.ToString(), "AUEC-Clearance");
                }

                JobDetail jobdetail = new JobDetail("ClearanceJobForAuec :" + _scheduldeAuec.ToString(), "AUEC-Clearance", typeof(ClearanceJob));
                jobdetail.JobDataMap["AUECID"] = auecClearanceTime.Key;
                List<int> temp = new List<int>();
                temp = (List<int>)jobdetail.JobDataMap["AUECID"];

                System.TimeSpan tt = TimeSpan.FromDays(1);
                SimpleTrigger st = new SimpleTrigger();
                st.RepeatCount = -1;
                st.RepeatInterval = tt;
                st.StartTimeUtc = auecClearanceTime.Value;
                st.Name = "TriggerAuec " + _scheduldeAuec.ToString();
                st.Group = "AUEC-Clearance-Triggers";
                _sched.ScheduleJob(jobdetail, st);
            }
        }

        //Kashish G. : PRANA-7184
        // Added in case if an Order comes with AuecID not present currently
        //then we need to schedule Trigger for the New Auec
        public void CreateTriggerForNewAuecsClearance(int auecID)
        {
            try
            {
                Dictionary<int, DateTime> clearanceTime = null;
                _scheduldeAuec++;
                if (_sched.GetJobDetail("ClearanceJobForAuec :" + _scheduldeAuec.ToString(), "AUEC-Clearance-Jobs") != null)
                {
                    _sched.DeleteJob("ClearanceJobForAuec :" + _scheduldeAuec.ToString(), "AUEC-Clearance");
                }
                JobDetail jobdetail = new JobDetail("ClearanceJobForAuec :" + _scheduldeAuec.ToString(), "AUEC-Clearance", typeof(ClearanceJob));

                List<int> temp = new List<int>();
                temp.Add(auecID);
                jobdetail.JobDataMap["AUECID"] = temp;

                clearanceTime = WindsorContainerManager.FetchClearanceTime();
                System.TimeSpan tt = TimeSpan.FromDays(1);
                SimpleTrigger st = new SimpleTrigger();
                st.RepeatCount = -1;
                st.RepeatInterval = tt;
                if (clearanceTime.ContainsKey(auecID))
                {
                    st.StartTimeUtc = clearanceTime[auecID];
                }
                st.Name = "TriggerAuec " + _scheduldeAuec.ToString();
                st.Group = "AUEC-Clearance-Triggers";
                _sched.ScheduleJob(jobdetail, st);
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

        public void ShutdownScheduler()
        {
            _sched.Shutdown();
        }

        public Dictionary<int, DateTime> SortDictionary(Dictionary<int, DateTime> auecClearanceTimeInUtc)
        {
            List<KeyValuePair<int, DateTime>> result = new List<KeyValuePair<int, DateTime>>(auecClearanceTimeInUtc);
            result.Sort(delegate (
                                  KeyValuePair<int, DateTime> first,
                                  KeyValuePair<int, DateTime> second)
                                 {
                                     return first.Value.CompareTo(second.Value);
                                 }
                        );
            Dictionary<int, DateTime> sortedDic = new Dictionary<int, DateTime>();
            foreach (KeyValuePair<int, DateTime> item in result)
            {
                sortedDic.Add(item.Key, item.Value);
            }
            return sortedDic;
        }

        public void StartScheduler()
        {
            _sched = _schedFact.GetScheduler();
            _sched.Start();
        }
    }
}