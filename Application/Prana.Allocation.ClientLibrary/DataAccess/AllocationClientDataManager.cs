using Prana.Allocation.Common.Constants;
using Prana.Allocation.Common.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Allocation.ClientLibrary.DataAccess
{
    public class AllocationClientDataManager
    {
        #region Members

        /// <summary>
        /// The _singelton instance
        /// </summary>
        private static AllocationClientDataManager _singeltonInstance = new AllocationClientDataManager();

        /// <summary>
        /// lock Object
        /// </summary>
        private static readonly object lockObject = new object();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary><value>The get instance</value>
        public static AllocationClientDataManager GetInstance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (lockObject)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new AllocationClientDataManager();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Allocates the by parameter.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="allocationParameter">The allocation parameter.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <returns></returns>
        public AllocationResponse AllocateByParameter(List<AllocationGroup> allocationGroupList, AllocationParameter allocationParameter, bool forceAllocation)
        {
            var response = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + allocationParameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);
                response = AllocationClientServiceConnector.Allocation.InnerChannel.AllocateByParameter(allocationGroupList, allocationParameter, forceAllocation);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + allocationParameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);
                AllocationLoggingHelper.LoggerWriteMessage(response.Response.Replace("\n", System.Environment.NewLine));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Allocates by parameter overload for PTT.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="allocationParameter">The allocation parameter.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <param name="pttAllocationSelected">if set to <c>true</c> [PTT Allocation ].</param>
        /// <returns></returns>
        public AllocationResponse AllocateByParameter(List<AllocationGroup> allocationGroupList, AllocationParameter allocationParameter, bool forceAllocation, bool pttAllocationSelected)
        {
            var response = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + allocationParameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);
                response = AllocationClientServiceConnector.Allocation.InnerChannel.AllocateByParameter(allocationGroupList, allocationParameter, forceAllocation, pttAllocationSelected);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + allocationParameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);
                AllocationLoggingHelper.LoggerWriteMessage(response.Response.Replace("\n", System.Environment.NewLine));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Allocates by preference overload for PTT.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="operationPreferenceId">The operation preference identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <param name="pttAllocationSelected">if set to <c>true</c> [PTT Allocation ].</param>
        /// <returns></returns>
        public AllocationResponse AllocateByPreference(List<AllocationGroup> allocationGroupList, int operationPreferenceId, int userId, bool isPreview, bool forceAllocation, bool pttAllocationSelected = false)
        {
            var response = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
                response = AllocationClientServiceConnector.Allocation.InnerChannel.AllocateByPreference(allocationGroupList, operationPreferenceId, userId, isPreview, forceAllocation, pttAllocationSelected);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
                AllocationLoggingHelper.LoggerWriteMessage(response.Response.Replace("\n", System.Environment.NewLine));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Allocate by the PTT preference.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="doStrategyAllocation">if set to <c>true</c> [do strategy allocation].</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <returns></returns>
        public AllocationResponse AllocateByPTTPreference(List<AllocationGroup> allocationGroupList, int userId, bool isPreview, bool forceAllocation)
        {
            AllocationResponse response = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
                response = AllocationClientServiceConnector.Allocation.InnerChannel.AllocateByPTTPreference(allocationGroupList, userId, isPreview, forceAllocation);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
                AllocationLoggingHelper.LoggerWriteMessage(response.Response.Replace("\n", System.Environment.NewLine));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Sends request to server to apply bulk change on group.
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="groupList"></param>
        /// <param name="isGroupWise"></param>
        /// <returns></returns>
        public List<AllocationGroup> ApplyCommissionBulkChange(CommissionRule commissionRule, List<AllocationGroup> groupList, bool isGroupWise)
        {
            try
            {
                return AllocationClientServiceConnector.Allocation.InnerChannel.ApplyCommissionBulkChange(commissionRule, groupList, isGroupWise);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return new List<AllocationGroup>();
            }
        }

        /// <summary>
        /// Calculates the accured interest.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns></returns>
        public double CalculateAccuredInterest(AllocationGroup allocationGroup)
        {
            double accuredInterest = allocationGroup.AccruedInterest;
            try
            {
                accuredInterest = AllocationClientServiceConnector.Allocation.InnerChannel.CalculateAccuredInterest(allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accuredInterest;
        }

        /// <summary>
        /// Calculates the commission.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public AllocationGroup CalculateCommission(AllocationGroup group)
        {
            var allocationGroup = new AllocationGroup();
            try
            {
                allocationGroup = AllocationClientServiceConnector.Allocation.InnerChannel.CalculateCommission(group);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroup;
        }

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="newGroup">The new group.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public TaxLot CreateUnAllocatedTaxLot(AllocationGroup newGroup, string groupId)
        {//This should be either in Taxlot Object or Group Object There should not be server call for this.
            TaxLot unallocatedtaxlot = new TaxLot();
            try
            {
                unallocatedtaxlot = AllocationClientServiceConnector.Allocation.InnerChannel.CreateUnAllocatedTaxLot(newGroup, groupId);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return unallocatedtaxlot;
        }

        /// <summary>
        /// Generates the group identifier.
        /// </summary>
        /// <returns></returns>
        public string GenerateGroupID()
        {
            string result = String.Empty;
            try
            {
                result = AllocationClientServiceConnector.Allocation.InnerChannel.GenerateGroupID();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the account pb details.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAccountPBDetails()
        {
            var accountDetails = new Dictionary<int, string>();
            try
            {
                accountDetails = AllocationClientServiceConnector.Allocation.InnerChannel.GetAccountPBDetails();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountDetails;
        }

        /// <summary>
        /// Gets all commission rules.
        /// </summary>
        /// <returns></returns>
        public List<CommissionRule> GetAllCommissionRules(int userID)
        {
            var allCommissionRules = new List<CommissionRule>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ALL_COMMISSIONRULE);
                allCommissionRules = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllCommissionRules();
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALL_COMMISSIONRULE);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allCommissionRules;
        }

        /// <summary>
        /// Gets the allocation data.
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <param name="filterList">The filter list.</param>
        /// <param name="p">The p.</param>
        public List<AllocationGroup> GetAllocationData(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, int userID)
        {
            var allocationGroups = new List<AllocationGroup>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ALLOCATION_DATA);
                allocationGroups = AllocationClientServiceConnector.Allocation.InnerChannel.GetGroups(toDate, fromDate, filterList, userID);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_DATA);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroups;
        }

        public bool RemoveManualExecution(string clOrderId, DateTime orderDate)
        {
            return AllocationClientServiceConnector.Allocation.InnerChannel.RemoveManualExecution(clOrderId, orderDate);
        }

        /// <summary>
        /// Reallocates the group blotter.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="allocationParameter">The allocation parameter.</param>
        /// <returns></returns>
        public AllocationResponse ReallocateGroup_Blotter(string groupId, AllocationParameter allocationParameter, int preferenceId, int userId, string clOrderId = "")
        {
            try
            {
                return AllocationClientServiceConnector.Allocation.InnerChannel.ReallocateGroup_Blotter(groupId, allocationParameter, preferenceId, userId, clOrderId);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;

        }

        /// <summary>
        /// Gets the current state for symbol.
        /// </summary>
        /// <param name="symbolList">The symbol list.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public List<CurrentSymbolState> GetCurrentStateForSymbol(List<string> symbolList, int userId)
        {
            var currentSymbolState = new List<CurrentSymbolState>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_CURRENT_STATE_FOR_SYMBOL);
                currentSymbolState = AllocationClientServiceConnector.Allocation.InnerChannel.GetCurrentStateForSymbol(symbolList, userId);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_CURRENT_STATE_FOR_SYMBOL);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currentSymbolState;
        }

        /// <summary>
        /// Gets the group status.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public Dictionary<string, PostTradeEnums.Status> GetGroupStatus(List<AllocationGroup> groups)
        {
            Dictionary<string, PostTradeEnums.Status> statusDictionary = new Dictionary<string, PostTradeEnums.Status>();
            try
            {
                statusDictionary = AllocationClientServiceConnector.ClosingServices.InnerChannel.GetGroupStatus(groups);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return statusDictionary;
        }

        /// <summary>
        /// Gets the preference by company identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public List<AllocationOperationPreference> GetPreferenceByCompanyId(int companyId, int userID)
        {
            List<AllocationOperationPreference> allocationOperationpreferenceList = new List<AllocationOperationPreference>();
            try
            {
                allocationOperationpreferenceList = AllocationClientServiceConnector.Allocation.InnerChannel.GetCalculatedPreferencesByCompanyId(companyId, userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationOperationpreferenceList;
        }

        /// <summary>
        /// Gets the master fund preference by company identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public List<AllocationMasterFundPreference> GetMasterFundPreferenceByCompanyId(int companyId, int userID)
        {
            List<AllocationMasterFundPreference> allocationMasterFundPreferenceList = new List<AllocationMasterFundPreference>();
            try
            {
                allocationMasterFundPreferenceList = AllocationClientServiceConnector.Allocation.InnerChannel.GetMasterFundPrefByCompanyId(companyId, userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationMasterFundPreferenceList;
        }

        /// <summary>
        /// Gets the PTT allocation preference.
        /// </summary>
        /// <param name="pttAllocationPrefernceId">The PTT allocation preference identifier.</param>
        /// <returns></returns>
        public AllocationOperationPreference GetPTTAllocationPreference(int pttAllocationPrefernceId)
        {
            AllocationOperationPreference pref = new AllocationOperationPreference();
            try
            {
                pref = AllocationClientServiceConnector.Allocation.InnerChannel.GetPreferenceById(pttAllocationPrefernceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pref;
        }

        /// <summary>
        /// Gets the taxlot details update external transaction.
        /// </summary>
        /// <param name="taxlotId">The taxlot identifier.</param>
        /// <returns></returns>
        public System.Data.DataTable GetTaxlotDetailsUpdateExternalTransaction(string taxlotId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = AllocationClientServiceConnector.Allocation.InnerChannel.GetTaxlotDetailsToUpdateExternalTransactionID(taxlotId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataTable;
        }

        /// <summary>
        /// Gets the trade attributes.
        /// </summary>
        /// <returns></returns>
        public List<string>[] GetTradeAttributes(int userID)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ATTRIBUTE_LIST);
                List<string>[] attributeList = AllocationClientServiceConnector.Allocation.InnerChannel.GetTradeAttributes();
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ATTRIBUTE_LIST);
                return attributeList;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Save Allocation Groups
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AllocationGroup> SaveAllocationGroups(List<AllocationGroup> groups, int userId, bool isComingForReallocation = false)
        {
            List<AllocationGroup> responseGroups = new List<AllocationGroup>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.SAVE_GROUPS);
                responseGroups = AllocationClientServiceConnector.Allocation.InnerChannel.SaveGroups(groups, userId, isComingForReallocation);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.SAVE_GROUPS);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return responseGroups;
        }

        /// <summary>
        /// Uns the allocate groups.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="userId">The p.</param>
        public List<AllocationGroup> UnAllocateGroups(List<AllocationGroup> groupList, int userId)
        {
            var groups = new List<AllocationGroup>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.UNALLOCATE_GROUPS);
                groups = AllocationClientServiceConnector.Allocation.InnerChannel.UnAllocateGroup(groupList, userId);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.UNALLOCATE_GROUPS);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groups;
        }

        /// <summary>
        /// Updates the account wise postion in cache.
        /// </summary>
        /// <param name="group">The group.</param>
        public void UpdateAccountWisePostionInCache(AllocationGroup group)
        {
            try
            {
                AllocationClientServiceConnector.Allocation.InnerChannel.UpdateAccountWisePostionInCache(group);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Re-Calculates the other fee.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public AllocationGroup ReCalculateOtherFeeForGroup(AllocationGroup group, List<OtherFeeType> listofFeesToApply)
        {
            var allocationGroup = new AllocationGroup();
            try
            {
                allocationGroup = AllocationClientServiceConnector.Allocation.InnerChannel.ReCalculateOtherFeeForGroup(group, listofFeesToApply);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroup;
        }

        /// <summary>
        /// Gets the allocation preferences.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isLevelingPerferenceRequired">if set to <c>true</c> [is leveling perference required].</param>
        /// <param name="isProrataByNavPerferenceRequired">if set to <c>true</c> [is prorata by nav perference required].</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllocationPreferences(int companyId, int userId, bool isLevelingPerferenceRequired, bool isProrataByNavPerferenceRequired)
        {
            Dictionary<int, string> allocationPreferences = null;
            try
            {
                allocationPreferences = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllocationPreferences(companyId, userId, isLevelingPerferenceRequired, isProrataByNavPerferenceRequired);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationPreferences;
        }

        #endregion Methods

    }
}
