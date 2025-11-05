using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessLogic.TradeAuditTrail
{
    internal sealed class AuditDataManager
    {
        private static volatile AuditDataManager instance;
        private static readonly object syncRoot = new Object();


        public static AuditDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AuditDataManager();
                    }
                }
                return instance;
            }
        }


        private AuditDataManager()
        {
            try
            {
                CreateAuditTrailServicesProxy();
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

        static ProxyBase<IAuditTrailService> _auditTrailServices = null;

        /// <summary>
        /// Private function creates an AuditService Proxy. Audit Trail Service should only be used using Audit Manager
        /// </summary>
        private static void CreateAuditTrailServicesProxy()
        {
            try
            {
                _auditTrailServices = new ProxyBase<IAuditTrailService>("TradeAuditTrailServiceEndpointAddress");
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
        /// Gets ignored users for audit trail from db for the current user
        /// </summary>
        /// <param name="companyId">company Id of current user</param>
        /// <param name="companyUserId">company userid of the current user</param>
        /// <returns></returns>
        public string GetIgnoredUserIds(int companyId, int companyUserId)
        {
            string ignored = "";
            try
            {
                ignored = _auditTrailServices.InnerChannel.GetIgnoredUsersForAudit(companyId, companyUserId);
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
            return ignored;
        }


        public bool SaveAuditDeletedGroups(List<T_Group_DeletedAudit> deletedGroups)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditDeletedGroups(deletedGroups);
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
            return true;
        }

        public bool SaveAuditDeletedSwap(List<SwapParameters> swapParameters)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditDeletedSwap(swapParameters);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB, clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditList(List<TradeAuditEntry> tradeAuditCollection)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditList(tradeAuditCollection);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB, clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditDeletedTaxlots(List<PM_Taxlots_DeletedAudit> deletedTaxlots)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditDeletedTaxlots(deletedTaxlots);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB specifically for Daily
        /// Valuation Data (i.e. MarkPrice and Fx-Rate), clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditListForDailyValuation(List<TradeAuditEntry> tradeAuditCollection)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditListForDailyValuation(tradeAuditCollection);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB specifically for Daily
        /// Valuation Data (i.e. MarkPrice and Fx-Rate), clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditListForCashJournal(List<CashJournalAuditEntry> cashAuditCollection)
        {
            try
            {
                _auditTrailServices.InnerChannel.SaveAuditListForCashJournal(cashAuditCollection);
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
            return true;
        }

        /// <summary>
        /// Gets the audit datatable using audit trail service
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="ignoredUser"></param>
        /// <param name="accountIdsCommaSeparated"></param>
        /// <returns></returns>
        public DataTable GetAuditDataByGroupIds(List<string> groupIds, string ignoredUser, string accountIdsCommaSeparated)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _auditTrailServices.InnerChannel.GetTradeAuditsForGroups(groupIds, ignoredUser, accountIdsCommaSeparated);
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
            return dt;
        }

        /// <summary>
        /// gets the audit data from db using audit service for filters on auditUI
        /// </summary>
        /// <param name="from">fram date</param>
        /// <param name="till">till date</param>
        /// <param name="symbol">symbols comma separated</param>
        /// <param name="accountIds">accountids comma separated</param>
        /// <param name="orderSideTagValues">comma separated</param>
        /// <returns></returns>
        public DataTable GetAuditUIDataByDate(AuditTrailFilterParams auditTrailFilterParams)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _auditTrailServices.InnerChannel.GetTradeAuditsForDates(auditTrailFilterParams);
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
            return dt;
        }

        public DataTable GetGroupTaxlotForIds(string groupId, string taxlotId)
        {
            return _auditTrailServices.InnerChannel.GetDetailsGroupTaxlotForIds(groupId, taxlotId);
        }

        public DataTable GetOrderDetailsByIds(string parentOrderID, string clOrderId)
        {
            return _auditTrailServices.InnerChannel.GetOrderDetailsByIds(parentOrderID, clOrderId);
        }
    }
}
