using Prana.BusinessObjects.Compliance.Alerting;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.Pending_Approval_UI.BLL
{
    internal class PendingApprovalCache
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PendingApprovalCache _pendingApprovalCache = null;


        /// <summary>
        /// private cunstructor, Initialises the Dictionary
        /// </summary>
        private PendingApprovalCache()
        {
            try
            {
                this._preTradeApprovalCache = new Dictionary<string, PreTradeApprovalInfo>();
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
        internal static PendingApprovalCache GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_pendingApprovalCache == null)
                        _pendingApprovalCache = new PendingApprovalCache();
                    return _pendingApprovalCache;
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
        /// Cache to store the pre trade approval data
        /// </summary>
        private Dictionary<string, PreTradeApprovalInfo> _preTradeApprovalCache;

        public Dictionary<string, PreTradeApprovalInfo> PreTradeApprovalCache
        {
            get { return _preTradeApprovalCache; }
            set { _preTradeApprovalCache = value; }
        }
    }
}
