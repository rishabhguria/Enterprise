using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.ServiceCommon.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IAuditTrailService : IServiceOnDemandStatus
    {
        [OperationContract]
        int SaveAuditList(List<TradeAuditEntry> tradeAuditCollection);

        [OperationContract]
        int SaveAuditListForDailyValuation(List<TradeAuditEntry> tradeAuditCollection);

        [OperationContract]
        int SaveAuditListForCashJournal(List<CashJournalAuditEntry> cashAuditCollection);

        [OperationContract]
        int SaveAuditDeletedGroups(List<T_Group_DeletedAudit> deletedGroups);

        [OperationContract]
        int SaveAuditDeletedTaxlots(List<PM_Taxlots_DeletedAudit> deletedTaxlots);

        [OperationContract]
        int SaveAuditDeletedSwap(List<SwapParameters> _deletedSwaps);

        [OperationContract]
        DataTable GetTradeAuditsForGroups(List<string> groupIds, string ignoredUser, string accountIdsCommaSeparated);

        [OperationContract]
        DataTable GetTradeAuditsForDates(AuditTrailFilterParams auditTrailSearchParams);

        [OperationContract]
        string GetIgnoredUsersForAudit(int companyId, int companyUserId);

        [OperationContract]
        DataTable GetDetailsGroupTaxlotForIds(string groupId, string taxlotId);

        [OperationContract]
        DataTable GetOrderDetailsByIds(string parentOrderID, string clOrderId);

    }
}
