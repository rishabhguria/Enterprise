using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.PostTrade
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class AuditTrailService : IAuditTrailService
    {
        public int SaveAuditList(List<TradeAuditEntry> tradeAuditCollection)
        {
            int rowCount = 0;
            try
            {
                rowCount = AuditTrailDataManager.Instance.SaveAuditList(tradeAuditCollection);
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
            return rowCount;
        }

        public int SaveAuditListForDailyValuation(List<TradeAuditEntry> tradeAuditCollection)
        {
            int rowCount = 0;
            try
            {
                rowCount = AuditTrailDataManager.Instance.SaveAuditListForDailyValuation(tradeAuditCollection);
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
            return rowCount;
        }

        public int SaveAuditListForCashJournal(List<CashJournalAuditEntry> tradeAuditCollection)
        {
            int rowCount = 0;
            try
            {
                rowCount = AuditTrailDataManager.Instance.SaveAuditListForCashJournal(tradeAuditCollection);
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
            return rowCount;
        }

        public int SaveAuditDeletedTaxlots(List<PM_Taxlots_DeletedAudit> deletedTaxlots)
        {
            int rowCount = 0;
            try
            {
                rowCount = AuditTrailDataManager.Instance.SaveAuditDeletedTaxlots(deletedTaxlots);
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
            return rowCount;
        }

        public int SaveAuditDeletedGroups(List<T_Group_DeletedAudit> deletedGroups)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = AuditTrailDataManager.Instance.SaveAuditDeletedGroups(deletedGroups);
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
            return rowsAffected;
        }

        public int SaveAuditDeletedSwap(List<SwapParameters> deletedSwaps)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = AuditTrailDataManager.Instance.SaveAuditDeletedSwap(deletedSwaps);
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
            return rowsAffected;
        }

        public DataTable GetTradeAuditsForGroups(List<string> groupIds, string ignoredUser, string accountIdsCommaSeparated)
        {
            DataTable dsAuditEntriesForSpecificGroups = new DataTable();
            try
            {
                dsAuditEntriesForSpecificGroups = AuditTrailDataManager.Instance.GetTradeAuditsForGroups(groupIds, ignoredUser, accountIdsCommaSeparated);
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
            return dsAuditEntriesForSpecificGroups;
        }

        public DataTable GetTradeAuditsForDates(AuditTrailFilterParams auditTrailSearchParams)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = AuditTrailDataManager.Instance.GetTradeAuditsForDates(auditTrailSearchParams);
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
            return dt;
        }

        public string GetIgnoredUsersForAudit(int companyId, int companyUserId)
        {
            string ignored = "";
            try
            {
                ignored = AuditTrailDataManager.Instance.GetIgnoredUsersForAudit(companyId, companyUserId);
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
            return ignored;
        }


        /// <summary>
        /// GetDetailsGroupTaxlotForIds
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="taxlotId"></param>
        /// <returns></returns>
        public DataTable GetDetailsGroupTaxlotForIds(string groupId, string taxlotId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = AuditTrailDataManager.Instance.GetDetailsGroupTaxlotForIds(groupId, taxlotId);
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
            return dt;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
        /// <summary>
        /// Get Order Details ByIds
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="taxlotId"></param>
        /// <returns></returns>
        public DataTable GetOrderDetailsByIds(string parentOrderID, string clOrderId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = AuditTrailDataManager.Instance.GetOrderDetailsByIds(parentOrderID, clOrderId);
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
            return dt;
        }


    }
}
