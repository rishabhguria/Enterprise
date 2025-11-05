using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.TradeManager.Extension
{
    public class BlotterCacheManager : IDisposable
    {
        private static BlotterCacheManager _blotterCacheManager = new BlotterCacheManager();
        private DataTable dtUserTradingAccountData = new DataTable();
        private ProxyBase<IPranaPositionServices> _positionManagementServices;

        private BlotterCacheManager()
        {
            _positionManagementServices = CreatePositionManagementProxy();
        }

        public static BlotterCacheManager GetInstance()
        {
            return _blotterCacheManager;
        }

        public void GetTradingAccountUsersByUserID(int _userID)
        {
            try
            {
                dtUserTradingAccountData = BlotterDataManager.GetInstance().GetAllUsersByUserID(_userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public Dictionary<int, string> GetUsersByTradingAccountIDandAUECID(int _traAccID, int AUECID)
        {
            Dictionary<int, string> dt = new Dictionary<int, string>();
            try
            {
                foreach (DataRow row in dtUserTradingAccountData.Select("TradinAccID = '" + _traAccID.ToString() + "'" + " and AUECID = '" + AUECID.ToString() + "'"))
                {
                    dt.Add(int.Parse(row[1].ToString()), row[2].ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dt;
        }

        public SortedDictionary<string, int> GetAllUsers()
        {
            SortedDictionary<string, int> dt = new SortedDictionary<string, int>();
            try
            {
                foreach (DataRow row in dtUserTradingAccountData.Rows)
                {
                    if (!dt.ContainsKey(row[2].ToString()))
                    {
                        dt.Add(row[2].ToString(), int.Parse(row[1].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dt;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_positionManagementServices != null)
                    {
                        _positionManagementServices.Dispose();
                        _positionManagementServices = null;
                    }
                    if (dtUserTradingAccountData != null)
                    {
                        dtUserTradingAccountData.Dispose();
                        dtUserTradingAccountData = null;
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
        /// Gets the allocation details by cl order identifier.
        /// </summary>
        /// <param name="parentClOrderID">The parent cl order identifier.</param>
        /// <returns></returns>
        public DataTable GetAllocationDetailsByClOrderID(string parentClOrderIDs)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = BlotterDataManager.GetInstance().GetAllocationDetailsByClOrderID(parentClOrderIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dt;
        }

        /// <summary>
        /// Gets the group identifier date by cl order identifier.
        /// </summary>
        /// <param name="parentClOrderID">The parent cl order identifier.</param>
        /// <returns></returns>
        public DataTable GetGroupIdDateByClOrderID(string parentClOrderID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = BlotterDataManager.GetInstance().GetGroupIdDateByClOrderID(parentClOrderID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dt;
        }

        /// <summary>
        /// Hides the order from blotter.
        /// </summary>
        /// <param name="parentClOrderIds">The parent cl order ids.</param>
        /// <returns></returns>
        public void HideOrderFromBlotter(List<string> listParentClOrderId, List<string> listParentClOrderIdOfSubOrders, bool isAllSubOrdersRemovable, int companyUserID, List<int> uniqueTradingAccounts)
        {
            try
            {
                List<string> tempAllParentClOrderId = new List<string>(listParentClOrderId);
                tempAllParentClOrderId.AddRange(listParentClOrderIdOfSubOrders);

                int rowsAffected = _positionManagementServices.InnerChannel.HideOrderFromBlotter(String.Join(",", tempAllParentClOrderId), isAllSubOrdersRemovable, companyUserID, uniqueTradingAccounts);

                if (rowsAffected > 0)
                    ComplianceServiceConnector.GetInstance().HideOrderFromBlotter(listParentClOrderId);
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
        /// Hides the sub orders from blotter.
        /// </summary>
        /// <param name="subOrderClOrderId">The list sub orders cl order identifier.</param>
        /// <param name="companyUserID">The company user identifier.</param>
        /// <param name="uniqueTradingAccounts">The unique trading accounts.</param>
        public void HideSubOrderFromBlotter(string subOrderClOrderId, int companyUserID, List<int> uniqueTradingAccounts)
        {
            try
            {
                int rowsAffected = _positionManagementServices.InnerChannel.HideSubOrderFromBlotter(subOrderClOrderId, companyUserID, uniqueTradingAccounts);
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

        private ProxyBase<IPranaPositionServices> CreatePositionManagementProxy()
        {
            try
            {
                return new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }
    }
}