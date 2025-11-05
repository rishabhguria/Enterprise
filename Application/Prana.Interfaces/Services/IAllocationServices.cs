using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.PositionManagement;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IAllocationServices : IServiceOnDemandStatus
    {
        [OperationContract]
        AllocationGroup UnAllocateGroup(AllocationGroup group);

        [OperationContract]
        int SaveGroups(List<AllocationGroup> groups, int userID);

        [OperationContract]
        AllocationGroup CreateAllcationGroupFromTaxlotBase(TaxlotBase taxlotBase);

        [OperationContract]
        AllocationGroup CreateAllocationGroup(Order order, bool isCached);

        [OperationContract]
        TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID);

        void CalculateCommissionOrderWise(ref Order order);

        void SaveUnSavedGroups(List<AllocationGroup> groupListNew, List<AllocationGroup> groupList);

        void GetUnSavedGroupsData(ref List<AllocationGroup> groupListNew, ref List<AllocationGroup> groupList);

        AllocationOperationPreference GetPreferenceById(int level1ID);

        PreferenceUpdateResult AddPreference(string prefName, int companyId, AllocationPreferencesType calculatedAllocationPreference, bool isPrefVisible);

        PreferenceUpdateResult UpdatePreference(AllocationOperationPreference aop);

        IClosingServices ClosingServices
        {
            set;
        }

        [OperationContract]
        ClosingData UnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate);

        [OperationContract]
        List<TaxLot> GetPositions(DateTime fromDate, DateTime toDate, string accountIDs);

        [OperationContract]
        bool CheckIfUnsavedGroups();

        /// <summary>
        /// Get groups from database for all the requsted taxlotids.
        /// This method is used in new cancel-amd+recon CH UI
        /// </summary>
        /// <param name="taxLotIds"></param>
        /// <returns></returns>
        [OperationContract]
        List<AllocationGroup> GetGroupsForTaxLotIDs(List<string> lstTaxLotIds);

        [OperationContract]
        int DeleteGroupsFromCA(DataTable dTable);

        /// <summary>
        /// Create allocation group virtually.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        [OperationContract]
        AllocationGroup CreateVirtualAllocationGroup(Order order, bool isCached, bool isForceAllocaation = false);

        /// <summary>
        /// Virtual allocation of In Market Quantities - This thing is doing just after real allocation of fills
        /// </summary>
        /// <param name="virtualAllocationGroup"></param>
        /// <returns></returns>
        [OperationContract]
        AllocationGroup DoVirtualAllocation(AllocationGroup virtualAllocationGroup);

        /// <summary>
        /// Gets the allocation preference name by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        [OperationContract]
        string GetAllocationPreferenceNameById(int preferenceId);

        /// <summary>
        /// Gets GroupID for order
        /// </summary>
        /// <param name="parentClOrderID"></param>
        /// <returns></returns>
        [OperationContract]
        string GetGroupID(string parentClOrderID);

        /// <summary>
        /// GetCurrentStateForSymbolInAccount
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userID"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [OperationContract]
        decimal GetCurrentStateForSymbolInAccount(string symbol, int userID, int accountId);
    }
}
