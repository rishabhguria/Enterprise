using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.Pending_Approval_UI.DAL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.Pending_Approval_UI.BLL
{
    internal class PendingApprovalManager
    {
        /// <summary>
        /// Event To Update the grid
        /// </summary>
        public event EventHandler<EventArgs<UltraGridRow, bool, string>> UpdateGrid;


        /// <summary>
        /// Event To Update the grid in case of Multi User
        /// </summary>
        public event EventHandler<EventArgs<List<Alert>, string, string, int>> UpdateGridForMultiUser;

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PendingApprovalManager _pendingApprovalManager = null;


        /// <summary>
        /// private cunstructor, Initialises the Dictionary
        /// </summary>
        private PendingApprovalManager()
        {
            try
            {
                PendingApprovalConnector.GetInstance().InitializeAmqp();
                PendingApprovalConnector.GetInstance().UpdateGridForMultiUser += PendingApprovalManager_UpdateGridForMultiUser;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PendingApprovalManager GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_pendingApprovalManager == null)
                        _pendingApprovalManager = new PendingApprovalManager();
                    return _pendingApprovalManager;
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

        #endregion

        /// <summary>
        /// Update Pending Approval Grid in case of Multi User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendingApprovalManager_UpdateGridForMultiUser(object sender, EventArgs<List<Alert>, string, string, int> e)
        {
            try
            {
                int userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                //e.Value4 contains the User Id who clicked on Approve or Block button. In case of same user who clicked on button,
                //No need to perform Multi user publishing for that user
                if (UpdateGridForMultiUser != null && !(e.Value4.Equals(userId)))
                    UpdateGridForMultiUser(this, new EventArgs<List<Alert>, string, string, int>(e.Value, e.Value2, e.Value3, e.Value4));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Getting Data from Server pending Approval Cache
        /// </summary>
        internal Dictionary<String, PreTradeApprovalInfo> GetPendingApprovalData()
        {
            try
            {
                Dictionary<String, PreTradeApprovalInfo> pendingApprovalCache = PendingApprovalConnector.GetInstance().GetPendingApprovalData();
                PendingApprovalCache.GetInstance().PreTradeApprovalCache = pendingApprovalCache;
                return pendingApprovalCache;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Approve/Block Button clicked
        /// </summary>
        /// <param name="btnName"></param>
        internal void ApproveBlockBtnClicked(PreTradeActionType actionType, string basketName, UltraGridRow row)
        {
            try
            {
                List<Alert> alerts = new List<Alert>();
                string defaultValue = "";
                string userName = defaultValue;
                int userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                foreach (PreTradeApprovalInfo preTradeApprovalInfo in PendingApprovalCache.GetInstance().PreTradeApprovalCache.Values)
                {
                    if (preTradeApprovalInfo.MultiTradeName.Equals(basketName))
                    {
                        List<Alert> allowedAlerts = new List<Alert>();
                        preTradeApprovalInfo.TriggeredAlerts.ForEach(alert =>
                        {
                            List<string> userIds = new List<String>(alert.OverrideUserId.ToString().Split(','));
                            if ((userIds.Contains(userId.ToString())))
                            {
                                alert.ComplianceOfficerNotes = row.Cells["ComplianceOfficerNotes"].Value.ToString();
                                allowedAlerts.Add(alert);
                            }
                        });
                        alerts.AddRange(allowedAlerts);
                        alerts.ForEach(x =>
                        {
                            //Update the preTradeActionType, OverrideUserId, ActionUser and ActionUserName in Alert
                            x.ActionUser = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                            x.PreTradeActionType = actionType;
                            x.OverrideUserId = PendingApprovalHelperClass.RemoveUserId(x.OverrideUserId);
                            if (x.ActionUser != -1)
                            {
                                if (x.ActionUser != 0)
                                    userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(x.ActionUser);
                                if (string.IsNullOrEmpty(userName))
                                    userName = defaultValue;
                            }
                            x.ActionUserName = userName;
                        });
                    }
                }

                PreTradeActionType preTradeActionType = PendingApprovalConnector.GetInstance().ApproveBlockBtnClicked(alerts);

                if (preTradeActionType.Equals(PreTradeActionType.NoAction))
                {
                    if (UpdateGrid != null)
                        UpdateGrid(this, new EventArgs<UltraGridRow, bool, string>(row, false, userName));
                }
                else if (preTradeActionType.Equals(PreTradeActionType.Allowed) || preTradeActionType.Equals(PreTradeActionType.Blocked))
                {
                    if (UpdateGrid != null)
                        UpdateGrid(this, new EventArgs<UltraGridRow, bool, string>(row, true, userName));
                    PendingApprovalCache.GetInstance().PreTradeApprovalCache.Remove(basketName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
