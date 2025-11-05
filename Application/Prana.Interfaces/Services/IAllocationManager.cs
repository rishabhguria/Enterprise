// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-27-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="IAllocationManager.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.PositionManagement;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

/// <summary>
/// The Interfaces namespace.
/// </summary>
namespace Prana.Interfaces
{
    /// <summary>
    /// This is base interface for manager class of allocation
    /// Now this is exposed as service
    /// </summary>
    [ServiceContract]
    public interface IAllocationManager : IServiceOnDemandStatus
    {

        #region General Methods (not exposed as service)

        /// <summary>
        /// Initializes the cache and other state of the manager
        /// </summary>
        void Initialize();

        #endregion

        /// <summary>
        /// This method will generate the result and allocate according to that
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated using predefined settings</param>
        /// <param name="operationPreferenceId">Operation preferenceId to be used. This preference is predefined and has target percentage and other checklists</param>
        /// <param name="userId">UserId who requested allocation</param>
        /// <returns>It should return the Output result for given list of AllocationGroup</returns>
        [OperationContract(Name = "AllocateByPreference")]
        AllocationResponse AllocateByPreference(List<AllocationGroup> groupList, int operationPreferenceId, int userId, bool isPreview = false, bool forceAllocation = false, bool isPTTAllocation = false, bool isReallocatedFromBlotter = false);

        /// <summary>
        /// This method generate the AllocationOutputResult which can used to further allocate the groups PTT Group will be allocated based on the 
        /// PTT Parameter
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated</param>
        /// <param name="parameter">Already provided AllocationParameter object to use while allocation</param>
        /// <param name="isPTTAllocation">use PTT pref for allocation</param>
        /// <returns>AllocationOutputResult object containing the account-wise allocation for each allocation group provided in the parameters</returns>
        [OperationContract(Name = "AllocateByParameter")]
        AllocationResponse AllocateByParameter(List<AllocationGroup> allocationGroupList, AllocationParameter allocationParameter, bool forceAllocation = false, bool isPTTAllocation = false, bool isReallocatedFromBlotter = false);

        /// <summary>
        /// Unallocate and returns unallocated groups
        /// </summary>
        /// <param name="groups">Groups to be unallocated</param>
        /// <param name="userId">User who requested to unallocate</param>
        /// <returns>Unallocated groups</returns>
        [OperationContract]
        List<AllocationGroup> UnAllocateGroup(List<AllocationGroup> groups, int userId);

        /// <summary>
        /// This method update given AllocationOperationPreference
        /// </summary>
        /// <param name="preference">Preference which to be updated</param>
        /// <returns>True if updated successfully, otherwise false</returns>
        [OperationContract]
        PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference);

        /// <summary>
        /// If masterfund allocation is enabled then returns master fund related allocation operation preferences, other wise return account wise allocation preferences set for given company
        /// </summary>
        /// <param name="companyId">Id of the company for which preference is required</param>
        /// <returns>Collection of AllocationOperationPreference objects</returns>
        [OperationContract]
        List<AllocationOperationPreference> GetCalculatedPreferencesByCompanyId(int companyId, int userId);

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>AllocationOperationPreference.</returns>
        [OperationContract]
        AllocationOperationPreference GetPreferenceById(int preferenceId);

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>AllocationOperationPreference.</returns>
        [OperationContract]
        IEnumerable<int> GetSelectedAccountsFromPref(int preferenceId);

        /// <summary>
        /// Gets the allocation preference name by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        [OperationContract]
        string GetAllocationPreferenceNameById(int preferenceId);

        /// <summary>
        /// Gets the allocation preference Id by Name.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        [OperationContract]
        int GetAllocationPreferenceIdByName(string prefName);

        /// <summary>
        /// Adds the given preference information to database and cache
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId of the preference</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>Update result of the preference</returns>
        [OperationContract]
        PreferenceUpdateResult AddPreference(string name, int companyId, AllocationPreferencesType allocationPrefType, bool isPrefVisible, string rebalancerFileName = "");

        /// <summary>
        /// Copy preference with new name
        /// </summary>
        /// <param name="preferenceId">preferenceId from which to be copied</param>
        /// <param name="name">Name of the new preference</param>
        /// <returns>PreferenceUpdate result</returns>
        [OperationContract]
        PreferenceUpdateResult CopyPreference(int preferenceId, string name, AllocationPreferencesType prefType);

        /// <summary>
        /// Delete preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be deleted</param>
        /// <returns>PreferenceUpdate result</returns>
        [OperationContract]
        PreferenceUpdateResult DeletePreference(int preferenceId, AllocationPreferencesType allocationPrefType);

        /// <summary>
        /// Renames preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be Renamed</param>
        /// <param name="name">New name of preference which to be Renamed</param>
        /// <returns>PreferenceUpdate result</returns>
        [OperationContract]
        PreferenceUpdateResult RenamePreference(int preferenceId, string name, AllocationPreferencesType allocationPrefType);

        /// <summary>
        /// Imports the given preference information to database and cache
        /// </summary>
        /// <param name="pref"></param>
        [OperationContract]
        PreferenceUpdateResult ImportPreference(AllocationOperationPreference preference);

        /// <summary>
        /// Imports the given master fund preference information to database and cache
        /// </summary>
        /// <param name="pref"></param>
        [OperationContract]
        PreferenceUpdateResult ImportMasterfundPreference(AllocationMasterFundPreference preference, List<AllocationOperationPreference> mfCalculatedPref);

        /// <summary>
        /// Returns the list of group for given date range along with the filter list provided
        /// </summary>
        /// <param name="toDate">Date upto which data is required (including)</param>
        /// <param name="fromTime">Date from which data is required (including)</param>
        /// <param name="filterList">List of filter which will be applied</param>
        /// <param name="userId">User which requires the data. UserId as -1 if not from any client</param>
        /// <returns>List of allocation groups</returns>
        [OperationContract]
        List<AllocationGroup> GetGroups(DateTime toDate, DateTime fromTime, AllocationPrefetchFilter filterList, int userId = -1);


        [OperationContract]
        bool RemoveManualExecution(string clOrderId, DateTime orderDate);

        /// <summary>
        /// Reallocates the group blotter.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="allocationParameter">The allocation parameter.</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [OperationContract]
        AllocationResponse ReallocateGroup_Blotter(string groupId, AllocationParameter allocationParameter, int preferenceId, int userId, string clOrderId = "");

        /// <summary>
        /// Save the given list of allocation group provided
        /// </summary>
        /// <param name="groups">List of allocation groups provided</param>
        /// <param name="userId">UserId who requested save</param>
        /// <returns>true if saved successfully, otherwise false</returns>
        [OperationContract]
        List<AllocationGroup> SaveGroups(List<AllocationGroup> groups, int userId, bool isComingForReallocation = false);

        /// <summary>
        /// Save the given list of allocation group provided
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="connString">The connection string.</param>
        /// <param name="fromServer">if set to <c>true</c> [from server].</param>
        /// <returns>Number of groups saved</returns>
        [OperationContract]
        int SaveGroupsForFills(List<AllocationGroup> groups, string connString, bool fromServer);

        /// <summary>
        /// Update the state of allocation for groups provided
        /// </summary>
        /// <param name="groups">List of allocation groups provided</param>
        /// <returns>true if updated successfully, otherwise false</returns>
        [OperationContract]
        bool UpdateState(List<AllocationGroup> groups);

        /// <summary>
        /// Saves the un saved groups.
        /// </summary>
        [OperationContract]
        void SaveUnSavedGroups(List<AllocationGroup> groupListNew, List<AllocationGroup> groupList);

        /// <summary>
        /// Get Un Saved GroupsData
        /// </summary>
        /// <param name="groupListNew"></param>
        /// <param name="groupList"></param>
        [OperationContract]
        void GetUnSavedGroupsData(ref List<AllocationGroup> groupListNew, ref List<AllocationGroup> groupList);


        /// <summary>
        /// Update the defaultRule for company
        /// </summary>
        /// <param name="defaultRule">default Rule for company</param>
        /// <param name="companyId">company id of user saving rule.</param>
        [OperationContract]
        bool SaveCompanyWisePreference(AllocationCompanyWisePref defaultPref);

        /// <summary>
        /// Returns default rule for company id
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [OperationContract]
        AllocationCompanyWisePref GetCompanyWisePreference(int companyId);

        /// <summary>
        /// Gets the trade attributes.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string>[] GetTradeAttributes();

        /// <summary>
        /// Updates the attribute list.
        /// </summary>
        /// <param name="group">The group.</param>
        [OperationContract]
        void UpdateAttribList(List<AllocationGroup> group);

        /// <summary>
        /// Get groups list for groups Id list
        /// </summary>
        /// <param name="groupIdList">List of group id</param>
        /// <returns>List of groups</returns>
        List<AllocationGroup> GetGroupsById(List<string> groupIdList);

        #region GroupCache methods used in PostTradeCacheManager

        /// <summary>
        /// Create a group and check if it is dirty group
        /// </summary>
        /// <param name="order">the order</param>
        /// <param name="isDirty">Dirty group: suppose a partially filled unAllocated trade comes through drop copy, in the mean while we allocate it from Allocation UI,
        /// this group is marked as Dirty and kept it in group cache. When next fill comes, we check it in the Dirty group cache, if exists, we update it as per the new fill.</param>
        /// <returns>the allocation group</returns>
        [OperationContract]
        AllocationGroup CreateGroup(Order order, ref bool isDirty, bool isReal);

        /// <summary>
        /// Add the allocation group
        /// </summary>
        /// <param name="group">The allocation group</param>
        [OperationContract]
        void AddGroup(AllocationGroup group);

        /// <summary>
        /// checks if groups are unsaved
        /// </summary>
        /// <returns>true if groups are unsaved, false otherwise</returns>
        [OperationContract]
        bool CheckIfUnsavedGroups();

        #endregion

        /// <summary>
        /// this method calculates commission for group when counter party is changed
        /// </summary>
        /// <param name="group">the allocation group</param>
        [OperationContract]
        AllocationGroup CalculateCommission(AllocationGroup group);

        /// <summary>
        /// this method re-calculates other fee for group when average price is changed
        /// </summary>
        /// <param name="group">the allocation group</param>
        [OperationContract]
        AllocationGroup ReCalculateOtherFeeForGroup(AllocationGroup group, List<OtherFeeType> listofFeesToApply);

        /// <summary>
        /// Get master funds
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DataSet GetAllMasterFundsRatio();

        /// <summary>
        /// Save master fund target ratio
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveMasterFundTargetRatio(DataSet ds);

        /// <summary>
        /// Save Attribute names
        /// </summary>
        /// <param name="ds"></param>
        [OperationContract]
        void SaveAttributeNames(DataSet ds);

        /// <summary>
        /// Gets the attribute names.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DataSet GetAttributeNames();

        /// <summary>
        /// Gets the current state for symbolsList.
        /// </summary>
        /// <param name="symbolList">The symbolsList.</param>
        /// <param name="userID">current user id</param>
        /// <returns></returns>
        [OperationContract]
        List<CurrentSymbolState> GetCurrentStateForSymbol(List<string> symbolList, int userID);

        /// <summary>
        /// Calculates commission for Bulk change
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="groupList"></param>
        /// <param name="isGroupWise"></param>
        /// <returns></returns>
        [OperationContract]
        List<AllocationGroup> ApplyCommissionBulkChange(CommissionRule commissionRule, List<AllocationGroup> groupList, bool isGroupWise);

        /// <summary>
        /// This method allocates by PTTPreference id which is passes to method AllocateByPreference 
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated using predefined settings</param>
        /// <param name="userId">UserId who requested allocation</param>
        /// <returns>It should return the Output result List for given list of AllocationGroup</returns>
        [OperationContract]
        AllocationResponse AllocateByPTTPreference(List<AllocationGroup> groupList, int userId, bool isPreview = false, bool forceAllocation = false);

        /// <summary>
        /// Calculates the accured interest.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns>double</returns>
        [OperationContract]
        double CalculateAccuredInterest(AllocationGroup allocationGroup);

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>AllocationOperationPreference.</returns>
        [OperationContract(Name = "GetPreferenceByIdByUser")]
        AllocationOperationPreference GetPreferenceById(int companyId, int userId, int preferenceId);

        /// <summary>
        /// Creates the real virtual allocation group.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="isCached">if set to <c>true</c> [is cached].</param>
        /// <param name="isReal">if set to <c>true</c> [is real].</param>
        /// <returns></returns>
        [OperationContract]
        AllocationGroup CreateRealVirtualAllocationGroup(Order order, bool isCached, bool isReal, bool isForceAllocaation = false);

        /// <summary>
        /// Uns the wind closing.
        /// </summary>
        /// <param name="TaxlotClosingID">The taxlot closing identifier.</param>
        /// <param name="taxlotIDList">The taxlot identifier list.</param>
        /// <param name="TaxlotClosingIDWithClosingDate">The taxlot closing identifier with closing date.</param>
        /// <returns></returns>
        [OperationContract]
        ClosingData UnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate);

        /// <summary>
        /// Gets the allocation scheme name by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract(Name = "GetAllocationSchemeNameByID")]
        string GetAllocationSchemeNameByID(int id);

        /// <summary>
        /// Gets the currency list for allocation scheme.
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetCurrencyListForAllocationScheme")]
        List<string> GetCurrencyListForAllocationScheme();

        /// <summary>
        /// Updates the account wise postion in cache.
        /// </summary>
        /// <param name="group">The group.</param>
        [OperationContract(Name = "UpdateAccountWisePostionInCache")]
        void UpdateAccountWisePostionInCache(AllocationGroup group);

        /// <summary>
        /// Gets all allocation scheme names.
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAllAllocationSchemeNames")]
        Dictionary<int, string> GetAllAllocationSchemeNames();

        /// <summary>
        /// Gets the allocation schemes by source.
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAllocationSchemesBySource")]
        Dictionary<int, string> GetAllocationSchemesBySource(FixedPreferenceCreationSource source);

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        [OperationContract(Name = "DeleteAllocationScheme")]
        bool DeleteAllocationScheme(int schemeID, string schemeName);

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        [OperationContract(Name = "GetAllocationSchemeByName")]
        AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName);

        /// <summary>
        /// Saves the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="Date">The date.</param>
        /// <param name="allocationXML">The allocation XML.</param>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        [OperationContract(Name = "SaveAllocationScheme")]
        int SaveAllocationScheme(AllocationFixedPreference fixedPref);

        /// <summary>
        /// Gets the allocation scheme recon report.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="fromAllocationDate">From allocation date.</param>
        /// <param name="toAllocationDate">To allocation date.</param>
        /// <returns></returns>
        [OperationContract(Name = "GetAllocationSchemeReconReport")]
        DataSet GetAllocationSchemeReconReport(string allocationSchemeName, DateTime fromAllocationDate, DateTime toAllocationDate);

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="baseMsg">The base MSG.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        [OperationContract]
        TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID);

        /// <summary>
        /// Generates the group identifier.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GenerateGroupID();

        /// <summary>
        /// Gets all commission rules.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CommissionRule> GetAllCommissionRules();

        /// <summary>
        /// Gets the account pb details.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, string> GetAccountPBDetails();

        /// <summary>
        /// Gets the taxlot details to update external transaction identifier.
        /// </summary>
        /// <param name="taxlotID">The taxlot identifier.</param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetTaxlotDetailsToUpdateExternalTransactionID(string taxlotID);

        /// <summary>
        /// Gets the taxlot details to update external transaction identifier.
        /// </summary>
        /// <param name="taxlotID">The taxlot identifier.</param>
        /// <returns></returns>
        [OperationContract]
        void ClearCacheMasterFundBasedPositions();

        /// <summary>
        /// Gets the master fund preference by company identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId(int companyId, int userId);

        /// <summary>
        /// Saves the master fund preference.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <returns></returns>
        [OperationContract]
        PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs);

        /// <summary>
        /// Gets the master fund preference by identifier.
        /// </summary>
        /// <param name="mfPreferenceId">The mf preference identifier.</param>
        /// <returns></returns>
        [OperationContract]
        AllocationMasterFundPreference GetMasterFundPreferenceById(int mfPreferenceId);

        /// <summary>
        /// Gets the invisible allocation preferences.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, string> GetInvisibleAllocationPreferences();
        /// <summary>
        /// Gets the allocation preferences, If MF allocation is enabled then this method returns MF preferences list otherwise account level calculated  preferences
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, string> GetAllocationPreferences(int companyId, int userId, bool isLevelingPerferenceRequired, bool isProrataByNavPerferenceRequired);

        /// <summary>
        /// Creates the allcation group from position master.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        [OperationContract]
        AllocationGroup CreateAllcationGroupFromPositionMaster(PositionMaster position);

        /// <summary>
        /// Createands the save group from taxlot.
        /// </summary>
        /// <param name="allocatedTrade">The allocated trade.</param>
        /// <param name="isCopyTradeAttrbsPrefUsed">Determine if copy trade attribute is used</param>
        /// <returns></returns>
        [OperationContract]
        List<AllocationGroup> CreateandSaveGroupFromTaxlot(List<TaxLot> allocatedTrade, bool isCopyTradeAttrbsPrefUsed);

        /// <summary>
        /// Creates the and save positions from import.
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <returns></returns>
        [OperationContract]
        Task<int> CreateAndSavePositionsFromImport(List<PositionMaster> postionMasterList);

        /// <summary>
        /// Creates the and save positions.
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <returns></returns>
        [OperationContract]
        List<AllocationGroup> CreateAndSavePositions(List<PositionMaster> postionMasterList);

        /// <summary>
        /// Creates the allcation group from taxlot base.
        /// </summary>
        /// <param name="taxlotBase">The taxlot base.</param>
        /// <returns></returns>
        [OperationContract]
        AllocationGroup CreateAllcationGroupFromTaxlotBase(TaxlotBase taxlotBase);

        /// <summary>
        /// Deletes the groups from ca.
        /// </summary>
        /// <param name="dTable">The d table.</param>
        /// <returns></returns>
        [OperationContract]
        int DeleteGroupsFromCA(DataTable dTable);

        /// <summary>
        /// Virtual allocation of In Market Quantities - This thing is doing just after real allocation of fills
        /// </summary>
        /// <param name="virtualAllocationGroup"></param>
        /// <returns></returns>
        AllocationGroup DoVirtualAllocation(AllocationGroup virtualAllocationGroup);

        /// <summary>
        /// Gets GroupID for order
        /// </summary>
        /// <param name="parentClOrderID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves a list of allocation groups based on the provided date range and allocation filter. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userID"></param>
        /// <param name="accountId"></param>
        [OperationContract]
        List<AllocationGroup> GetGroupedOrderDetails(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, bool fetchAllValues = false);

        /// <summary>
        /// This method refreshes the allocation preference cache
        /// </summary>
        void RefreshAllocationPreferenceCache();
    }
}
