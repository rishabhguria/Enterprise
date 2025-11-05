using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Dashboard.BLL;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.Dashboard
{
    public class DashboardManager : IMasterDashboard
    {
        #region singleton
        //private  volatile DashboardManager instance;
        //private static object lockObject = new Object();

        //public DashboardManager()
        //{

        //}

        //public DashboardManager Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (lockObject)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new DashboardManager();
        //                }
        //            }
        //        }

        //        return instance;
        //    }
        //}
        #endregion

        // 
        public delegate void WorkflowEventListener(object sender, EventArgs e);
        public static event WorkflowEventListener WorkflowListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        internal static ConcurrentDictionary<String, List<WorkflowItem>> GetDashboardDataForDate(DateTime fromDate, String xmlAccounts, bool isSearchByFileExecutionDate)
        {
            try
            {
                DataSet WorkflowData = DashboardDataManager.GetDashboradDatafromDB(fromDate, xmlAccounts, isSearchByFileExecutionDate);
                ConcurrentDictionary<String, List<WorkflowItem>> accountWiseWorkflowData = ProcessDataforUI(WorkflowData);
                return accountWiseWorkflowData;///

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
        /// 
        /// </summary>
        /// <param name="WorkflowData"></param>
        private static ConcurrentDictionary<String, List<WorkflowItem>> ProcessDataforUI(DataSet WorkflowData)
        {
            ConcurrentDictionary<String, List<WorkflowItem>> accountWiseDashboardData = new ConcurrentDictionary<string, List<WorkflowItem>>();
            try
            {

                object resultLockerObject = new object();

                Parallel.ForEach(WorkflowData.Tables[0].AsEnumerable(), dr =>
                {
                    WorkflowItem dashboardObj = WorkflowItem.GetWorkFlowItem(dr);
                    lock (resultLockerObject)
                    {
                        // _dashboardUIObjects.Add(dashboardObj);

                        String key = DashboardHelper.GetKey(dashboardObj.AccountID, dashboardObj.FileExecutionDate); //TODO check run time or execution date
                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            if (!accountWiseDashboardData.ContainsKey(key))
                            {
                                accountWiseDashboardData.TryAdd(key, new List<WorkflowItem> { dashboardObj });
                            }
                            else
                            {
                                accountWiseDashboardData[key].Add(dashboardObj);
                            }
                        }
                    }
                });



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
            return accountWiseDashboardData;
        }


        #region IMasterDashboard Members

        /// <summary>
        /// get account wise batchs from DB
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, List<int>> GetAccountBatchMapping()
        {
            try
            {
                return DashboardDataManager.GetAccountBatchMapping();
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
            return null;
        }

        /// <summary>
        /// hanlde workflow event publishing on dashborad UI
        /// </summary>
        /// <param name="workflowItems"></param>
        public void PublishWorkFlowItems(List<WorkflowItem> workflowItems)
        {
            try
            {
                //  UpdateAccountWiseWorkflowData(workflowItems);

                if (WorkflowListener != null)
                {
                    WorkFlowEventArgs e = new WorkFlowEventArgs(workflowItems);
                    WorkflowListener(this, e);
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
        /// Save workflow data in DB
        /// </summary>
        /// <param name="workflowDataAsXML"></param>
        public void SaveWorkflowData(String workflowDataAsXML)
        {

            try
            {
                DashboardDataManager.SaveDashboradDataInDB(workflowDataAsXML);
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

        #region IMasterDashboard Members


        public int GetAccountIDByAccountName(string accountName)
        {
            int accountID = int.MinValue;
            try
            {
                accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
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
            return accountID;
        }

        #endregion
    }

    internal class WorkFlowEventArgs : EventArgs
    {
        internal List<WorkflowItem> items = new List<WorkflowItem>();
        internal WorkFlowEventArgs(List<WorkflowItem> items)
        {
            this.items = items;
        }

    }
}
