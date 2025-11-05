using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Dashboard.BLL
{
    class DashboardHelper
    {
        /// <summary>
        /// Get final account wise workflow status 
        /// </summary>
        /// <param name="accountWiseWorkflowData"></param>
        /// <returns></returns>
        internal static List<MasterDashboardUIObj> GetConsolidatedDashboardData(ConcurrentDictionary<String, List<WorkflowItem>> accountWiseWorkflowData)
        {
            List<MasterDashboardUIObj> dashboardUIObjects = new List<MasterDashboardUIObj>();
            try
            {
                foreach (string key in accountWiseWorkflowData.Keys)
                {
                    List<WorkflowItem> dashboradDataOfAccount = accountWiseWorkflowData[key];
                    if (dashboradDataOfAccount.Count > 0)
                    {
                        MasterDashboardUIObj accountsFinalStatus = GetConsolidatedAccountsStatus(dashboradDataOfAccount);
                        if (accountsFinalStatus != null)
                        {
                            accountsFinalStatus.AccountID = dashboradDataOfAccount[0].AccountID;
                            accountsFinalStatus.ExecutionDate = dashboradDataOfAccount[0].FileExecutionDate;

                            int clientID = CommonDataCache.CachedDataManager.GetInstance.GetCompanyIDOfAccountID(accountsFinalStatus.AccountID);
                            accountsFinalStatus.CompanyName = CommonDataCache.CachedDataManager.GetCompanyText(clientID);

                            int thirdPartyId = CommonDataCache.CachedDataManager.GetInstance.GetThirdPartyIDOfAccount(accountsFinalStatus.AccountID);
                            accountsFinalStatus.ThirdPartyName = CommonDataCache.CachedDataManager.GetInstance.GetThirdPartyNameByID(thirdPartyId);

                            accountsFinalStatus.AccountName = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(dashboradDataOfAccount[0].AccountID);

                        }
                        dashboardUIObjects.Add(accountsFinalStatus);
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
            return dashboardUIObjects;
        }

        /// <summary>
        /// get accounts find status
        /// </summary>
        /// <param name="dashboradDataOfAccount"></param>
        /// <returns></returns>
        private static MasterDashboardUIObj GetConsolidatedAccountsStatus(List<WorkflowItem> dashboradDataOfAccount)
        {
            MasterDashboardUIObj accountsFinalStatus = new MasterDashboardUIObj();
            try
            {
                accountsFinalStatus.ImportStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.Import);
                accountsFinalStatus.FileUploadStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.FileUpload);
                accountsFinalStatus.ImportIntoAppStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.ImportIntoAPP);
                accountsFinalStatus.SMValidationStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.SMValidation);
                accountsFinalStatus.ReconStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.Recon);

                //TODO
                //accountsFinalStatus.CnclAmndStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.CnclAmnd);
                //accountsFinalStatus.MarkPriceStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.MarkPrice);
                //accountsFinalStatus.ClosingStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.Closing);
                //accountsFinalStatus.ReportingStatus = GetWorkflowStatus(dashboradDataOfAccount, NirvanaWorkFlows.Reporting);
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
            return accountsFinalStatus;
        }

        /// <summary>
        /// get workflow status
        /// </summary>
        /// <param name="workflowDataOfAccount"></param>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        internal static NirvanaTaskStatus GetWorkflowStatus(List<WorkflowItem> workflowDataOfAccount, NirvanaWorkFlows nirvanaWorkFlow)
        {
            try
            {


                int FileUploadStatus = CheckAllStatusIsSame(workflowDataOfAccount, nirvanaWorkFlow);
                if (FileUploadStatus > 0)
                {
                    return (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), FileUploadStatus);
                }
                else if (CheckStausIsRunning(workflowDataOfAccount, nirvanaWorkFlow))
                {
                    return NirvanaTaskStatus.Running;
                }

                else if (CheckStausIsPartialSuccess(workflowDataOfAccount, nirvanaWorkFlow))
                {
                    return NirvanaTaskStatus.PartialSuccess;
                }

                else if (CheckStausIsFailed(workflowDataOfAccount, nirvanaWorkFlow))
                {
                    return NirvanaTaskStatus.Failure;
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
            return NirvanaTaskStatus.Pending;
        }

        /// <summary>
        /// Check Staus Is PartialSuccess
        /// </summary>
        /// <param name="dashboradDataOfAccount"></param>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        private static bool CheckStausIsPartialSuccess(List<WorkflowItem> dashboradDataOfAccount, NirvanaWorkFlows nirvanaWorkFlow)
        {
            try
            {
                int partialSuccessCount = dashboradDataOfAccount.Where(X => X.EventID == (int)nirvanaWorkFlow && (X.StatusID == (int)NirvanaTaskStatus.PartialSuccess || X.StatusID == (int)NirvanaTaskStatus.Success || X.StatusID == (int)NirvanaTaskStatus.Completed || X.StatusID == (int)NirvanaTaskStatus.PendingCompleted)).Count(); ;

                if (partialSuccessCount > 0)
                {
                    return true;
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
            return false;
        }

        /// <summary>
        /// Check Staus Is Failed
        /// </summary>
        /// <param name="dashboradDataOfAccount"></param>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        private static bool CheckStausIsFailed(List<WorkflowItem> dashboradDataOfAccount, NirvanaWorkFlows nirvanaWorkFlow)
        {
            try
            {
                int faildTaskCount = dashboradDataOfAccount.Where(X => X.EventID == (int)nirvanaWorkFlow && (X.StatusID == (int)NirvanaTaskStatus.Failure || X.StatusID == (int)NirvanaTaskStatus.Canceled)).Count();
                if (faildTaskCount > 0)
                {
                    return true;
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
            return false;
        }

        /// <summary>
        /// Check Staus Is Running
        /// </summary>
        /// <param name="dashboradDataOfAccount"></param>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        private static bool CheckStausIsRunning(List<WorkflowItem> dashboradDataOfAccount, NirvanaWorkFlows nirvanaWorkFlow)
        {

            try
            {

                int runningTaskCount = dashboradDataOfAccount.Where(X => X.EventID == (int)nirvanaWorkFlow && (X.StatusID == (int)NirvanaTaskStatus.Running || X.StatusID == (int)NirvanaTaskStatus.Importing)).Count();

                if (runningTaskCount > 0)
                {
                    return true;
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
            return false;
        }

        /// <summary>
        /// Check All Status Is Same and return final status 
        /// </summary>
        /// <param name="dashboradDataOfAccount"></param>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        private static int CheckAllStatusIsSame(List<WorkflowItem> dashboradDataOfAccount, NirvanaWorkFlows nirvanaWorkFlow)
        {

            int workFlowStatusID = 0;
            try
            {
                //String propertyName = GetPropertyName(nirvanaWorkFlow);
                List<WorkflowItem> list = dashboradDataOfAccount.Where(X => X.EventID == (int)nirvanaWorkFlow).Select(X => X).ToList();
                if (list.Count > 0)
                {
                    workFlowStatusID = (int)list[0].StatusID;
                    foreach (WorkflowItem item in list)
                    {
                        if (workFlowStatusID != item.StatusID)
                        {
                            return -1;
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
            return workFlowStatusID;
        }

        /// <summary>
        /// get property name for workflow status
        /// </summary>
        /// <param name="nirvanaWorkFlow"></param>
        /// <returns></returns>
        private static string GetPropertyName(NirvanaWorkFlows nirvanaWorkFlow)
        {
            String propName = string.Empty;
            try
            {
                switch (nirvanaWorkFlow)
                {
                    case NirvanaWorkFlows.FileUpload:
                        propName = "FileUploadStatus";
                        break;
                    case NirvanaWorkFlows.Import:
                        propName = "ImportStatus";
                        break;
                    case NirvanaWorkFlows.SMValidation:
                        propName = "SMValidationStatus";
                        break;

                    case NirvanaWorkFlows.Recon:
                        propName = "ReconStatus";
                        break;

                    case NirvanaWorkFlows.CnclAmnd:
                        propName = "CnclAmndStatus";
                        break;

                    case NirvanaWorkFlows.MarkPrice:
                        propName = "ClosingStatus";
                        break;

                    case NirvanaWorkFlows.Closing:
                        propName = "MarkPriceStatus";
                        break;

                    case NirvanaWorkFlows.Reporting:
                        propName = "ReportingStatus";
                        break;
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
            return propName;
        }

        /// <summary>
        /// get key of date and account for accountwise dict
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="ExecutionDate"></param>
        /// <returns></returns>
        internal static string GetKey(int accountID, DateTime ExecutionDate)
        {
            try
            {
                return accountID + ExecutionDate.ToString("yyyyMMdd");
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
            return string.Empty;
        }



        internal static MasterDashboardUIObj GetDashboardUIObject(WorkflowItem workflowItem)
        {
            MasterDashboardUIObj uiObj = new MasterDashboardUIObj();
            try
            {
                uiObj.AccountID = workflowItem.AccountID;
                uiObj.EventTime = workflowItem.EventRunTime;
                uiObj.ExecutionDate = workflowItem.FileExecutionDate;
                int clientID = CommonDataCache.CachedDataManager.GetInstance.GetCompanyIDOfAccountID(workflowItem.AccountID);
                uiObj.CompanyName = CommonDataCache.CachedDataManager.GetCompanyText(clientID);
                int thirdPartyId = CommonDataCache.CachedDataManager.GetInstance.GetThirdPartyIDOfAccount(workflowItem.AccountID);
                uiObj.ThirdPartyName = CommonDataCache.CachedDataManager.GetInstance.GetThirdPartyNameByID(thirdPartyId);
                uiObj.AccountName = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(workflowItem.AccountID);
                uiObj.UpdateStatus(workflowItem.EventID, workflowItem.StatusID);

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
            return uiObj;
        }

    }
}
