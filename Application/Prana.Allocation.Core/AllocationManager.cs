// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 08-27-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="AllocationManager.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Constants;
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.DataAccess;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.Extensions;
using Prana.Allocation.Core.Factories;
using Prana.Allocation.Core.FormulaStore;
using Prana.Allocation.Core.Helper;
using Prana.Allocation.Core.Managers;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade;
using Prana.ServerCommon;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// The Core namespace.
/// </summary>
namespace Prana.Allocation.Core
{
    /// <summary>
    /// Implementation of IAllocationManager
    /// This class defines the actual working of allocation for given list of allocation group
    /// <para>It also handles AllocationOperationPreference as well as state on which allocation will be based</para>
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class AllocationManager : IAllocationManager
    {
        #region Intitialize services
        /// <summary>
        /// Closing service set by dependency injection
        /// TODO: Need to check the correct way to do this
        /// </summary>
        /// <value>The closing services.</value>
        public IClosingServices ClosingServices
        {
            set
            {
                ServiceProxyConnector.ClosingProxy = value;
                if (_m_PersistenceManager != null)
                    _m_PersistenceManager.ClosingServices = value;
            }
        }

        /// <summary>
        /// Securitymaster service set by dependency injection
        /// TODO: Need to check the correct way to do this
        /// </summary>
        /// <value>The sec master services.</value>
        public ISecMasterServices SecMasterServices
        {
            set
            {
                ServiceProxyConnector.SecmasterProxy = value;
                if (_m_PersistenceManager != null)
                    _m_PersistenceManager.SecMasterServices = value;
            }
        }

        /// <summary>
        /// Activity service set by dependency injection
        /// TODO: Need to check the correct way to do this
        /// </summary>
        /// <value>The activity services.</value>
        public IActivityServices ActivityServices
        {
            set { ServiceProxyConnector.ActivityService = value; }
        }

        public ISecMasterOTCService SecMasterOTCService
        {
            set { ServiceProxyConnector.SecMasterOTCService = value; }
        }

        /// <summary>
        /// Sets the cash management service.
        /// </summary>
        /// <value>
        /// The cash management service.
        /// </value>
        public ICashManagementService CashManagementService
        {
            set { ServiceProxyConnector.CashManagementService = value; }
        }

        /// <summary>
        /// Sets the position management services.
        /// </summary>
        /// <value>
        /// The position management services.
        /// </value>
        public IPranaPositionServices PositionManagementServices
        {
            set { ServiceProxyConnector.PositionManagementServices = value; }
        }

        #endregion

        #region Members

        /// <summary>
        /// FactoryInstance to create or get IAllocationGeneratorInstance
        /// </summary>
        private IAllocationGeneratorFactory _factory;

        /// <summary>
        /// The locker
        /// </summary>
        readonly object _locker = new object();

        /// <summary>
        /// The m persistence manager
        /// </summary>
        static PersistenceManager _m_PersistenceManager;

        /// <summary>
        /// The locker allocation save
        /// </summary>
        private readonly object lockerAllocationSave = new object();
        private Dictionary<string, long> replacedOrders = new Dictionary<string, long>();
        private Dictionary<string, string> pendingReplaceOrders = new Dictionary<string, string>();

        #endregion

        #region Constructor
        /// <summary>
        /// Public constructor to create instance of given class
        /// This constructor also initialize allocation cache objects
        /// </summary>
        public AllocationManager()
        {
            if (_m_PersistenceManager == null)
                _m_PersistenceManager = new PersistenceManager();
        }

        #endregion

        #region IAllocationManager Members

        /// <summary>
        /// Initializes the cache and other state of the manager
        /// </summary>
        public void Initialize()
        {
            try
            {
                //ServiceConnector.ClosingProxy = closingServices;
                //ServiceConnector.SecmasterProxy = secMasterServices;
                // Initializing factory of type AllocationGeneratorFactory
                _factory = new AllocationGeneratorFactory();

                // Initializing cache so that database queries and other stuff should not be hit when first allocation group arrives for allocation
                PreferenceManager.GetInstance.Initialize();
                StateCacheStore.Instance.Initialize();
                AllocationGroupCache.Instance.Initialize();
                TradeAttributeCache.Instance.Initialize(AllocationPrefDataManager.GetTradeAttrbsLists());
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
        }

        #region Allocate/Unallocate
        /// <summary>
        /// This method generate the AllocationOutputResult which can used to further allocate the groups
        /// <para>In this method AllocationParameter is provided from outside</para>
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated</param>
        /// <param name="parameter">Already provided AllocationParameter object to use while allocation</param>
        /// <returns>AllocationOutputResult object containing the account-wise allocation for each allocation group provided in the parameters</returns>
        private AllocationResponse AllocateByAllocationParameter(List<AllocationGroup> groupList, AllocationParameter parameter, bool forceAllocation = false, bool isReallocatedFromBlotter = false)
        {
            AllocationResponse allocationResponse = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + parameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);

                AllocationBaseType baseType = parameter.CheckListWisePreference.BaseType;
                List<AllocationGroup> oldAllocationGroups = AllocationManagerHelper.GetOldAllocationGroups(groupList, parameter.UserId);
                //If userId == -1 treated as incoming trade.
                if (parameter.UserId == -1)
                {
                    AllocationPostInfoCache.Instance.AddUpdateParameterForGroupFromTT(groupList, ref parameter);
                    if (parameter.PreferenceId == int.MinValue) // int.MinValue for unallocation, -1 for manual and any int for pref id
                    {
                        allocationResponse.GroupList = UnAllocateGroup(groupList, parameter.UserId);
                        return allocationResponse;
                    }
                }
                else
                {
                    //if not -1 then coming from allocation update parameter for user in cache
                    AllocationPostInfoCache.Instance.AddUpdateUserParameterForGroups(groupList, parameter.UserId, parameter);
                }
                if (parameter.PreferenceId != int.MinValue && parameter.PreferenceId != -1)
                    groupList.ForEach(x => x.AllocationSchemeID = parameter.PreferenceId);

                // parameter.DoCheckSide = AllocationManagerHelper.IsValidateCheckSideForGroups(groupList, parameter, forceAllocation, oldAllocationGroups, GetCompanyWisePreference(CommonDataCache.CachedDataManager.GetInstance.GetCompanyID()).DoCheckSide);


                //Updating user state for preview
                AllocationManagerHelper.UpdateUserStateCache(groupList, parameter.UserId, parameter.IsPreview, -1);


                List<AllocationGroup> unAllocatedGroups = new List<AllocationGroup>();
                List<AllocationGroup> allocatedGroups = new List<AllocationGroup>();


                parameter.ForceAllocation = forceAllocation;
                AllocationOutputResult result = _factory.GetGenerator(baseType).Generate(groupList, parameter);

                if (result != null)
                {
                    if (result.AllocationFailedSymbols.Count > 0)
                    {
                        foreach (AllocationGroup group in groupList)
                        {
                            if (result.AllocationFailedSymbols.Contains(group.IsSwapped ? group.Symbol + "-Swap" : group.Symbol))
                            {
                                unAllocatedGroups.Add(group);
                            }
                            else
                            {
                                allocatedGroups.Add(group);
                            }
                        }
                    }
                    else
                        allocatedGroups = groupList;
                }
                //adding old state for new fill, userId = -1 is treated as incoming trade and only one trade is there in this scenario
                if (parameter.UserId == -1)
                {
                    if (result.AllocationFailedSymbols.Count > 0 || !String.IsNullOrWhiteSpace(result.Error))
                    {
                        allocationResponse.AddAllocationResponse(result.Error, UnAllocateGroup(unAllocatedGroups, -1));
                    }
                }

                if (allocatedGroups.Count > 0)
                {
                    // used to calculate AccruedInterest while Reallocating the trade on Alloc UI,	PRANA-7410
                    ServiceProxyConnector.CashManagementService.CalculateAccruedInterest(allocatedGroups.Cast<PranaBasicMessage>().ToList());
                    AllocationManagerHelper.AllocateInAccounts(parameter, allocatedGroups, result, isReallocatedFromBlotter);
                    AllocationManagerHelper.UpdateUserStateCache(allocatedGroups, parameter.UserId, parameter.IsPreview, 1);
                    allocationResponse.AddAllocationResponse(result.Error, allocatedGroups);
                }
                else
                    allocationResponse.Response = result.Error;

                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + parameter.UserId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PARAMETER);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                allocationResponse = new AllocationResponse { Response = "Something went wrong. Cannot allocate. Please contact administrator.", GroupList = new List<AllocationGroup>() };
            }
            return allocationResponse;
        }



        /// <summary>
        /// This method generate the AllocationOutputResult which can used to further allocate the groups
        /// <para>In this method AllocationParameter is provided from outside</para>
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated</param>
        /// <param name="parameter">Already provided AllocationParameter object to use while allocation</param>
        /// <param name="isPTTAllocation">use PTT pref for allocation</param>
        /// <returns>AllocationOutputResult object containing the account-wise allocation for each allocation group provided in the parameters</returns>
        public AllocationResponse AllocateByParameter(List<AllocationGroup> groupList, AllocationParameter allocationParameter, bool forceAllocation, bool isPTTAllocation = false, bool isReallocatedFromBlotter = false)
        {
            AllocationResponse response = new AllocationResponse();
            try
            {
                //For incoming trade from TT/Fix, check the post info cache for changed allocation from allocation UI. 
                //If allocation has been changed, then call AllocateByPreference with dummy preferenceID to use current allocation from post info cache, otherwise use original allocation details to allocate trade, PRANA-26905
                if (allocationParameter.UserId == -1)
                {
                    bool existsInCache = AllocationPostInfoCache.Instance.IsExistsInPostGroupCache(groupList[0].GroupID);
                    if (existsInCache)
                    {
                        response = AllocateByPreference(groupList, -1, allocationParameter.UserId, allocationParameter.IsPreview, forceAllocation, isPTTAllocation);
                        return response;
                    }
                }
                List<AllocationGroup> groupListPTT = new List<AllocationGroup>();
                if (isPTTAllocation)
                {
                    AllocationManagerHelper.BifurcateGroupsForPTTAllocation(ref groupList, groupListPTT);
                    if (groupListPTT.Count > 0)
                    {
                        AllocationResponse responsePST = AllocateByPTTPreference(groupListPTT, allocationParameter.UserId, allocationParameter.IsPreview, forceAllocation);
                        response.AddAllocationResponse(responsePST);
                    }
                }
                if (groupList.Count > 0)
                {
                    AllocationResponse allocationResponse = AllocateByAllocationParameter(groupList, allocationParameter, forceAllocation, isReallocatedFromBlotter);
                    response.AddAllocationResponse(allocationResponse);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// This method will generate the result and allocate according to that
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated using predefined settings</param>
        /// <param name="preferenceId">Operation preferenceId to be used. This preference is predefined and has target percentage and other checklists</param>
        /// <param name="userId">UserId who requested allocation</param>
        /// <returns>It should return the Output result for given list of AllocationGroup</returns>
        public AllocationResponse AllocateByPreference(List<AllocationGroup> groupList, int preferenceId, int userId, bool isPreview = false, bool forceAllocation = false, bool isPTTAllocation = false, bool isReallocatedFromBlotter = false)
        {
            AllocationResponse response = new AllocationResponse();
            try
            {
                //For incoming trade from TT/Fix, check the post info cache for changed allocation from allocation UI. 
                //If allocation has been changed, then use current allocation from post info cache, otherwise use original allocation details to allocate trade, PRANA-26905
                if (userId == -1)
                {
                    AllocationParameter param = AllocationPostInfoCache.Instance.GetCurrentParameterForGroup(groupList[0]);
                    if (param != null)
                    {
                        //In case of preferenceID being -1(manual allocation) or minValue(unallocated trade), use parameter from post info cache, otherwise use preferenceID to allocate
                        if (param.PreferenceId == int.MinValue)
                        {
                            response.GroupList = UnAllocateGroup(groupList, userId);
                            return response;
                        }
                        else if (param.PreferenceId == -1)
                        {
                            param.UpdateUserId(userId);
                            param.IsPreview = isPreview;
                            response = AllocateByAllocationParameter(groupList, param, forceAllocation);
                            return response;
                        }
                        else
                        {
                            preferenceId = param.PreferenceId;
                        }
                    }
                }
                List<AllocationGroup> groupListPTT = new List<AllocationGroup>();
                if (isPTTAllocation)
                {
                    AllocationManagerHelper.BifurcateGroupsForPTTAllocation(ref groupList, groupListPTT);
                    if (groupListPTT.Count > 0)
                    {
                        AllocationResponse responsePST = AllocateByPTTPreference(groupListPTT, userId, isPreview, forceAllocation);
                        response.AddAllocationResponse(responsePST);
                    }
                }
                AllocationOperationPreference calculatedPref = PreferenceManager.GetInstance.GetPreferenceById(preferenceId);
                if (calculatedPref != null)
                {
                    AllocationResponse responseCalculated = AllocateByCalculatedPreference(groupList, calculatedPref, userId, isPreview, forceAllocation, isReallocatedFromBlotter);
                    response.AddAllocationResponse(responseCalculated);
                }
                else
                {
                    AllocationMasterFundPreference masterFundPref = PreferenceManager.GetInstance.GetMasterFundPreferenceById(preferenceId);

                    if (masterFundPref != null)
                    {
                        AllocationResponse responseMasterFund = AllocateByMasterFundPreference(groupList, masterFundPref, userId, isPreview, forceAllocation);
                        response.AddAllocationResponse(responseMasterFund);
                    }
                    else
                    {
                        string fixedPref = PreferenceManager.GetInstance.GetAllocationSchemeNameByID(preferenceId);
                        if (!string.IsNullOrWhiteSpace(fixedPref))
                        {
                            AllocationResponse responseFixedPref = ValidateAndAllocateBySymbol(groupList, new KeyValuePair<int, string>(preferenceId, fixedPref), userId, forceAllocation);
                            response.AddAllocationResponse(responseFixedPref);
                        }
                        else
                        {
                            response.SetAllocationResponse("Preference not found, so allocation cannot be done", new List<AllocationGroup>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                response.SetAllocationResponse("Something went wrong. Cannot allocate. Please contact administrator.", new List<AllocationGroup>());
            }
            return response;
        }

        /// <summary>
        /// Allocates the by calculated preference.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="pref">The preference.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <returns></returns>
        private AllocationResponse AllocateByCalculatedPreference(List<AllocationGroup> groupList, AllocationOperationPreference pref, int userId, bool isPreview = false, bool forceAllocation = false, bool isReallocatedFromBlotter = false)
        {
            AllocationResponse result = new AllocationResponse();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
                // Traversing through all the checklists and generating AllocationOutputResult for each list of AllocationGroup
                Dictionary<AllocationParameter, List<AllocationGroup>> allocationRuleWiseAllocationGroup = AllocationManagerHelper.GetBaseWiseAllocation(groupList, pref, userId, true, isPreview);
                Parallel.ForEach(allocationRuleWiseAllocationGroup.Keys, parameter =>
                {
                    parameter.IsVirtual = pref.IsVirtual;
                    AllocationResponse resIntermediate = AllocateByAllocationParameter(allocationRuleWiseAllocationGroup[parameter], parameter, forceAllocation, isReallocatedFromBlotter);
                    result.AddAllocationResponse(resIntermediate);
                });

                List<AllocationGroup> remainingGroups = groupList.Except(result.GroupList).ToList();
                if (remainingGroups.Count > 0 && !remainingGroups.Count.Equals(groupList.Count))
                {
                    AllocationResponse partialResult = AllocateByCalculatedPreference(remainingGroups, pref, userId, isPreview, forceAllocation, isReallocatedFromBlotter);
                    result.Response = partialResult.Response;
                    if (partialResult.GroupList != null)
                        result.GroupList.AddRange(partialResult.GroupList);
                }

                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Allocates the by master fund preference.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="mfPreferenceId">The mf preference identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <returns></returns>
        private AllocationResponse AllocateByMasterFundPreference(List<AllocationGroup> groupList, AllocationMasterFundPreference masterFundPref, int userId, bool isPreview = false, bool forceAllocation = false)
        {
            try
            {
                /* 1. Get masterfund distribution output on basis of master fund default rule for each symbol
                 * 2. For each symbol in output collection, repeat steps 3 to 5
                 * 3. Get calculated preference with general rules based percentage distribution from master fund allocation output
                 * 4. Get allocation resopnse by using AllocateByCalculatedPreference() method using above preference and symbol allocation groups
                 * 5. Add allocation response to final result
                 */
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.ALLOCATE_GROUP_BY_MFPREFERENCE);


                //set force allocation to false, as check side will always be considered with leveling, even if asset classes are disabled for check side 
                if (masterFundPref.DefaultRule.RuleType == MatchingRuleType.Leveling)
                    forceAllocation = false;

                AllocationResponse result = new AllocationResponse();
                List<AllocationGroup> oldAllocationGroups = AllocationManagerHelper.GetOldAllocationGroups(groupList, userId);

                //Update StateCache by removing state for Fills of current groups, so that Match Closing can be checked for the selected trades

                MasterFundAllocationOutputResult mfAllocationResult = _factory.GetMasterFundAllocationGenerator(masterFundPref.DefaultRule.BaseType).Generate(groupList, masterFundPref, userId);


                if (string.IsNullOrWhiteSpace(mfAllocationResult.ErrorMessage))
                {
                    foreach (string symbol in mfAllocationResult.OutputCollection.Keys)
                    {
                        MasterFundAllocationOutput mfAllocOutput = mfAllocationResult.OutputCollection[symbol];
                        if (!string.IsNullOrWhiteSpace(mfAllocOutput.ErrorMessage) && mfAllocOutput.MatchClosingTransaction == MatchClosingTransactionType.None)
                            result.AddAllocationResponse(mfAllocOutput.ErrorMessage, null);
                        else
                        {
                            string errorMessage = string.Empty;
                            //get calculated preference with general rules based percentage distribution from master fund allocation output
                            AllocationOperationPreference calculatedPref = GetPreferenceForMFDistribution(masterFundPref, mfAllocOutput, userId, forceAllocation, out errorMessage);
                            if (string.IsNullOrWhiteSpace(errorMessage) || !(mfAllocOutput.MatchClosingTransaction == MatchClosingTransactionType.None || MasterFundAllocationHelper.IsLongPositionInSelectedGroups(mfAllocOutput.SymbolWiseGroupList)))
                            {
                                //Set IsVirtual property to true for generated Calculated Pref, 
                                //and if IsVirtual is true, then don't pick parameter from post info cache and use the allocation percentage calculated using MF pref 
                                calculatedPref.IsVirtual = true;
                                AllocationResponse resIntermediate = AllocateByCalculatedPreference(mfAllocOutput.SymbolWiseGroupList, calculatedPref, userId, isPreview, forceAllocation);
                                if (!string.IsNullOrWhiteSpace(resIntermediate.Response) && mfAllocOutput.MatchClosingTransaction != MatchClosingTransactionType.None)
                                {
                                    //Remove general rules from calculated pref and try to allocate remaining groups again if match closing is possible - PRANA-26787
                                    List<AllocationGroup> remainingGroups = mfAllocOutput.SymbolWiseGroupList.Except(resIntermediate.GroupList).ToList();
                                    if (remainingGroups.Count > 0)
                                    {
                                        bool isPreferenceUpdated = MasterFundAllocationHelper.UpdatePreferenceForMatchClosing(ref calculatedPref, remainingGroups);
                                        if (isPreferenceUpdated)
                                        {
                                            AllocationResponse partialResult = AllocateByCalculatedPreference(remainingGroups, calculatedPref, userId, isPreview, forceAllocation);
                                            resIntermediate.Response = string.Empty;
                                            resIntermediate.AddAllocationResponse(partialResult);
                                        }
                                    }
                                }
                                result.AddAllocationResponse(resIntermediate);
                            }
                            else
                            {
                                result.AddAllocationResponse(errorMessage, null);
                            }
                        }
                    }
                }
                else
                    result.Response += mfAllocationResult.ErrorMessage;
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.ALLOCATE_GROUP_BY_MFPREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return new AllocationResponse { Response = "Something went wrong. Cannot allocate. Please contact administrator.", GroupList = null };
            }
        }

        /// <summary>
        /// Gets calculated preference with general rules based percentage distribution from master fund allocation output
        /// </summary>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <param name="mfAllocOutput">The mf alloc output.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private AllocationOperationPreference GetPreferenceForMFDistribution(AllocationMasterFundPreference masterFundPref, MasterFundAllocationOutput mfAllocOutput, int userId, bool forceAllocation, out string errorMessage)
        {
            AllocationOperationPreference calculatedPref = new AllocationOperationPreference();
            string errorString = string.Empty;
            try
            {

                /* 1. Get master fund wise calculated preferences
                 * 2. Update master fund calculated preferences, add general rule with default rule and percentage if preference doesnt contain the general rule
                 * 3. For each master fund, repeat steps 2 to 6
                 * 4. Get updated calculated preference for master fund
                 * 5. update virtual group list for to get master fund allocation
                 * 6. get general rule wise virtual allocation group list
                 * 7. Foreach general rule, get allocation response for virtual group list by using AllocateByCalculatedPreference() method and update Allocation output
                 * 8. get target percentage in accounts from allocation response group allocations collection on basis of general rule
                 * 9. Get calculated preference from general rule wise target pecentage, also use targetQuantity_currentPreference as default rule
                 * 10. Use MatchClosingTransaction value from masterFundDefaultRule for generated pref
                 * 11. If MatchClosingTransaction is of type selectedmasterfunds update list of funds for generated pref from mf preference
                 */
                Dictionary<int, AllocationOperationPreference> masterFundPreferences = masterFundPref.MasterFundPreference.Keys.ToDictionary(t => t, t => PreferenceManager.GetInstance.GetPreferenceById(masterFundPref.MasterFundPreference[t]));
                List<int> masterFundPrefAccounts = MasterFundAllocationHelper.GetSelectedAccountsList(masterFundPreferences.Keys.ToList());

                if (string.IsNullOrWhiteSpace(mfAllocOutput.ErrorMessage))
                {
                    masterFundPreferences = mfAllocOutput.MasterFundQuantity.Where(x => x.Value != 0.0M).ToDictionary(t => t.Key, t => masterFundPreferences[t.Key]);
                    MasterFundAllocationHelper.UpdateMasterFundPreferences(ref masterFundPreferences);
                    SerializableDictionary<CheckListWisePreference, AllocationLevelList> generalRuleWiseAllocations = new SerializableDictionary<CheckListWisePreference, AllocationLevelList>();
                    object accountPercentageLocker = new object();
                    AllocationRule rule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, MatchClosingTransaction = mfAllocOutput.MatchClosingTransaction };

                    Parallel.ForEach(masterFundPreferences.Keys, (mfId, state) =>
                    {
                        AllocationOperationPreference calcPref = masterFundPreferences[mfId];
                        List<AllocationGroup> groupList = mfAllocOutput.OrderSideWiseVirtualGroups[mfId];

                        Dictionary<CheckListWisePreference, List<AllocationGroup>> allocationRuleWiseAllocationGroup = AllocationManagerHelper.GetGeneralRuleWiseAllocation(groupList, calcPref);
                        Parallel.ForEach(allocationRuleWiseAllocationGroup.Keys, (generalRule, checkListState) =>
                        {
                            AllocationResponse resp = AllocateByCalculatedPreference(allocationRuleWiseAllocationGroup[generalRule], calcPref, userId, true, true);
                            if (string.IsNullOrWhiteSpace(resp.Response) && resp.GroupList.Count > 0)
                            {
                                lock (accountPercentageLocker)
                                {
                                    generalRule.TryUpdateDefaultRule(rule);
                                    generalRule.TryUpdateAccountsList(masterFundPrefAccounts);
                                    generalRule.TryUpdateTargetPercentage(new SerializableDictionary<int, AccountValue>());
                                    if (generalRuleWiseAllocations.ContainsKey(generalRule))
                                        generalRuleWiseAllocations[generalRule].Merge(resp.GroupList[0].Allocations);
                                    else
                                        generalRuleWiseAllocations.Add(generalRule, resp.GroupList[0].Allocations);
                                }
                            }
                            else if (!errorString.Contains(resp.Response.Trim()))
                            {
                                errorString += "\n" + resp.Response.Trim();
                                checkListState.Break();
                            }
                        });

                        if (!string.IsNullOrWhiteSpace(errorString))
                            state.Break();
                    });

                    if (string.IsNullOrWhiteSpace(errorString))
                    {
                        SerializableDictionary<CheckListWisePreference, SerializableDictionary<int, AccountValue>> generalRuleWiseTargetPercentage = new SerializableDictionary<CheckListWisePreference, SerializableDictionary<int, AccountValue>>();
                        foreach (CheckListWisePreference generalRule in generalRuleWiseAllocations.Keys)
                        {
                            lock (accountPercentageLocker)
                            {
                                generalRuleWiseTargetPercentage.Add(generalRule, AllocationManagerHelper.GetTargetPercentageFromGroupAllocationCollection(generalRuleWiseAllocations[generalRule]));
                            }
                        }
                        // after getting allocation percentage on basis of calculated preference for master funds, allocate groups using allocation pref generated from checklist wise preferences            
                        calculatedPref = AllocationManagerHelper.GetCalculatedPreference(generalRuleWiseTargetPercentage, masterFundPref.MasterFundPreferenceId);
                    }
                }

                if (mfAllocOutput.MatchClosingTransaction != MatchClosingTransactionType.None && !(string.IsNullOrWhiteSpace(errorString) && string.IsNullOrWhiteSpace(mfAllocOutput.ErrorMessage)))
                {
                    //If Match Closing Transaction is possible and mfAllocator or account distribution gave error msg then generate a calculated pref with empty prorata list and empty target percentage for allocation
                    AllocationRule defaultRule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.Prorata, MatchClosingTransaction = mfAllocOutput.MatchClosingTransaction, ProrataAccountList = new List<int>() };
                    calculatedPref.TryUpdateDefaultRule(defaultRule);
                    calculatedPref.TryUpdateAccountsList(new List<int>(masterFundPrefAccounts));
                    calculatedPref.OperationPreferenceId = masterFundPref.MasterFundPreferenceId;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                errorString = "Something went wrong. Please contact administrator";
            }
            errorMessage = errorString;
            return calculatedPref;
        }

        /// <summary>
        /// Unallocate and returns unallocated groups
        /// </summary>
        /// <param name="groups">Groups to be unallocated</param>
        /// <param name="userId">User who requested to unallocate</param>
        /// <returns>Unallocated groups</returns>
        public List<AllocationGroup> UnAllocateGroup(List<AllocationGroup> groups, int userId)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.UNALLOCATE_GROUPS);
                if (userId != -1)
                {
                    AllocationManagerHelper.UpdateUserStateCache(groups, userId, false, -1);

                    //Adding blank parameter with 0 as preference id
                    AllocationParameter parameter = new AllocationParameter(null, null, int.MinValue, userId, false);
                    AllocationPostInfoCache.Instance.AddUpdateUserParameterForGroups(groups, userId, parameter);
                }
                // used to calculate AccruedInterest while Reallocating the trade on Alloc UI,	PRANA-7410
                ServiceProxyConnector.CashManagementService.CalculateAccruedInterest(groups.Cast<PranaBasicMessage>().ToList());
                foreach (AllocationGroup group in groups)
                {
                    group.UnallocateGroup();
                    group.AllocationSchemeID = 0;
                }
                if (userId != -1)
                    AllocationManagerHelper.UpdateUserStateCache(groups, userId, false, 1);

                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.UNALLOCATE_GROUPS);
                return groups;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// This method allocates by PTTPreference id which is passes to method AllocateByPreference 
        /// </summary>
        /// <param name="groupList">List of allocation group which will be allocated using predefined settings</param>
        /// <param name="operationPreferenceId">Operation preferenceId to be used. This preference is predefined and has target percentage and other checklists</param>
        /// <param name="userId">UserId who requested allocation</param>
        /// <returns>It should return the Output result for given list of AllocationGroup</returns>
        public AllocationResponse AllocateByPTTPreference(List<AllocationGroup> groupList, int userId, bool isPreview = false, bool forceAllocation = false)
        {
            AllocationResponse result = new AllocationResponse();
            try
            {
                foreach (AllocationGroup allocationGroup in groupList)
                {
                    AllocationOperationPreference pref = PreferenceManager.GetInstance.GetPreferenceById(allocationGroup.OriginalAllocationPreferenceID);
                    if (pref != null)
                    {
                        List<AllocationGroup> allocationGroups = new List<AllocationGroup> { allocationGroup };
                        AllocationResponse resIntermediate = AllocateByCalculatedPreference(allocationGroups, pref, userId, isPreview, forceAllocation);
                        result.AddAllocationResponse(resIntermediate);
                    }
                    else
                    {
                        string fixedPref = PreferenceManager.GetInstance.GetAllocationSchemeNameByID(allocationGroup.OriginalAllocationPreferenceID);
                        if (!string.IsNullOrWhiteSpace(fixedPref))
                        {
                            List<AllocationGroup> groups = new List<AllocationGroup>();
                            groups.Add(allocationGroup);
                            AllocationResponse responseFixedPref = ValidateAndAllocateBySymbol(groups, new KeyValuePair<int, string>(allocationGroup.OriginalAllocationPreferenceID, fixedPref), userId, forceAllocation);
                            result.AddAllocationResponse(responseFixedPref);
                        }
                        else
                        {
                            AllocationMasterFundPreference masterFundPref = PreferenceManager.GetInstance.GetMasterFundPreferenceById(allocationGroup.OriginalAllocationPreferenceID);

                            if (masterFundPref != null)
                            {
                                AllocationResponse responseMasterFund = AllocateByMasterFundPreference(groupList, masterFundPref, userId, isPreview, forceAllocation);
                                result.AddAllocationResponse(responseMasterFund);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Validate groups before allocating by Symbol
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="prefrenceSelected">The prefrence selected.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isForceAllocationSelected">if set to <c>true</c> [is force allocation selected].</param>
        /// <returns></returns>
        private AllocationResponse ValidateAndAllocateBySymbol(List<AllocationGroup> groups, KeyValuePair<int, string> prefrenceSelected, int userId, bool isForceAllocationSelected)
        {
            try
            {
                AllocationResponse responseObjectList = new AllocationResponse();
                if (groups.Count > 0)
                {
                    AllocationCompanyWisePref pref = GetCompanyWisePreference(CachedDataManager.GetInstance.GetCompanyID());
                    bool isMasterFundRatio = pref.EnableMasterFundAllocation && pref.IsOneSymbolOneMasterFundAllocation;
                    int accountID = pref.DefaultRule.PreferenceAccountId;

                    foreach (AllocationGroup group in groups)
                    {
                        //TODO: Improve code for Allocation by symbol there are multiple calls on server for every group
                        AllocationResponse responseObject;
                        group.ErrorMessage = string.Empty;
                        AllocationLevelList accounts = new AllocationLevelList();
                        bool isSwap = false;
                        string result = ValidateAllocationAccountsByAllocationScheme(group, ref accounts, prefrenceSelected.Value, ref isSwap, accountID, isMasterFundRatio);
                        if (result == string.Empty)
                            responseObject = AllocateBySymbol(group, prefrenceSelected, accounts, userId, isForceAllocationSelected);
                        else
                        {
                            group.ErrorMessage = result;
                            responseObject = new AllocationResponse();
                            responseObject.GroupList = null;
                            responseObject.Response = result;
                        }
                        responseObjectList.AddAllocationResponse(responseObject);
                    }
                }
                return responseObjectList;
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
        /// Allocates by symbol.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="accounts">The accounts.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isForceAllocationSelected">if set to <c>true</c> [is force allocation selected].</param>
        /// <returns></returns>
        private AllocationResponse AllocateBySymbol(AllocationGroup group, KeyValuePair<int, string> allocationSchemeName, AllocationLevelList accounts, int userId, bool isForceAllocationSelected)
        {
            try
            {
                group.AllocationSchemeName = allocationSchemeName.Value;
                group.AllocationSchemeID = allocationSchemeName.Key;
                List<AllocationGroup> groupList = new List<AllocationGroup>();
                groupList.Add(group);
                SerializableDictionary<int, AccountValue> accountValue = new SerializableDictionary<int, AccountValue>();
                if (accounts != null && accounts.Collection.Count > 0)
                {
                    decimal percentageTotal = 0M;
                    foreach (AllocationLevelClass allocations in accounts.Collection)
                    {
                        decimal percentage = (decimal)allocations.Percentage;
                        percentageTotal += percentage;
                        accountValue.Add(allocations.LevelnID, new AccountValue(allocations.LevelnID, percentage));
                    }
                    if (percentageTotal != 100M)
                    {
                        accountValue[accounts.Collection[0].LevelnID].AddValue(100M - percentageTotal);
                    }
                }
                AllocationRule rule = new AllocationRule();
                rule.BaseType = AllocationBaseType.CumQuantity;
                rule.RuleType = MatchingRuleType.None;
                rule.MatchClosingTransaction = MatchClosingTransactionType.None;
                rule.PreferenceAccountId = -1;

                AllocationResponse response = null;
                if (groupList.Count > 0)
                    response = AllocateByParameter(groupList, new AllocationParameter(rule, accountValue, -1, userId, true), isForceAllocationSelected);
                return response;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Virtual allocation of In Market Quantities - This thing is doing just after real allocation of fills
        /// 
        /// "virtualAllocationGroup" on when at the start of this function holds the updated information after fills have been received for an order
        /// As Compliance needs to be updated with new fills (In Market data) and information received from market the new allocation is applied as below
        /// - To be returned virtualAllocationGroup's new CumQty = Remaining Qty of input virtualAllocationGroup
        /// - Each to be returned taxlot in virtualAllocationGroup's is allocated by prorata derived from real allocation
        /// - Extra shares are assigned with consistent logic to one account
        ///
        /// e.g. before execution f1: 100
        ///                       f2: 100
        ///                       f3: 100
        ///                 VirtualAllocationGroup CumQty: 200
        ///                 VirtualAllocationGroup Qty: 500
        ///                
        ///	and execution of 100 shares happens and the input allocation group to this function has below status
        ///	
        /// After the execution of this function the virtual allocation group returned and sent to esper will be, prorated calculation using targetpercentage from allocation level class.
        ///                      f1: 134
        ///                      f2: 133
        ///                      f3: 133
        ///                VirtualAllocationGroup CumQty: 100
        ///                VirtualAllocationGroup Qty: 500
        ///                
        /// fill of 100 qty divided in
        ///                      f1: 34 
        ///                      f2: 33
        ///                      f3: 33
        ///                VirtualAllocationGroup CumQty: 100
        ///                VirtualAllocationGroup Qty: 500
        /// </summary>
        /// <param name="virtualAllocationGroup"></param>
        /// <returns></returns>
        public AllocationGroup DoVirtualAllocation(AllocationGroup virtualAllocationGroup)
        {
            try
            {
                SerializableDictionary<int, AccountValue> accountWiseTargetAllocation = new SerializableDictionary<int, AccountValue>();
                foreach (AllocationLevelClass allocationLevelClass in virtualAllocationGroup.Allocations.Collection)
                {
                    if (!accountWiseTargetAllocation.ContainsKey(allocationLevelClass.LevelnID))
                        accountWiseTargetAllocation.Add(allocationLevelClass.LevelnID, new AccountValue(allocationLevelClass.LevelnID, allocationLevelClass.TargetPercentage));
                }

                List<TaxLot> virtualTaxlots = virtualAllocationGroup.GetAllTaxlots();

                //For Compliance InMarket Handling - Remaining Qty is equal to Virtual Group's Cum Qty
                virtualAllocationGroup.CumQty = virtualAllocationGroup.Quantity - virtualAllocationGroup.CumQty;

                if (virtualAllocationGroup.CumQty > 0 && virtualTaxlots != null && virtualTaxlots.Count > 0)
                {
                    int sideMultiplier = Calculations.GetSideMultilpier(virtualAllocationGroup.OrderSideTagValue);
                    AllocationOutput output = AllocationManagerHelper.GetAllocationOutput(virtualAllocationGroup, accountWiseTargetAllocation, -1, false);

                    if (output.AccountValueCollection != null && output.AccountValueCollection.Count > 0)
                    {
                        foreach (TaxLot tempTaxlot in virtualTaxlots)
                        {
                            double virtualAllocation = Convert.ToDouble(output.GetValueForAccount(tempTaxlot.Level1ID));
                            if (tempTaxlot.Level2ID > 0)
                            {
                                AllocationLevelClass accountValue = virtualAllocationGroup.Allocations.Collection.Find(account => account.LevelnID == tempTaxlot.Level1ID);
                                if (accountValue != null)
                                {
                                    AllocationLevelClass strategyValue = accountValue.Childs.Collection.Find(x => x.LevelnID == tempTaxlot.Level2ID);
                                    if (strategyValue != null)
                                        virtualAllocation = (virtualAllocation * strategyValue.Percentage) / 100d;
                                }
                            }
                            tempTaxlot.ExecutedQty = virtualAllocation;
                            tempTaxlot.TaxLotQty = virtualAllocation;
                            tempTaxlot.CumQty = virtualAllocationGroup.CumQty;
                        }
                    }
                    else
                    {
                        foreach (TaxLot tempTaxlot in virtualTaxlots)
                        {
                            tempTaxlot.ExecutedQty = virtualAllocationGroup.CumQty;
                            tempTaxlot.TaxLotQty = virtualAllocationGroup.CumQty;
                            tempTaxlot.CumQty = virtualAllocationGroup.CumQty;
                        }
                    }
                }
                else if (virtualAllocationGroup.CumQty == 0)
                {
                    foreach (TaxLot tempTaxlot in virtualTaxlots)
                    {
                        tempTaxlot.ExecutedQty = 0;
                        tempTaxlot.TaxLotQty = 0;
                        tempTaxlot.CumQty = 0;
                    }
                }

                return virtualAllocationGroup;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }
        #endregion

        #region Allocation Preference
        /// <summary>
        /// This method update AllocationOperationPreference for given company
        /// </summary>
        /// <param name="preference">Preference which to be updated</param>
        /// <returns>True if updated successfully, otherwise false</returns>
        public PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.UPDATE_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.UpdatePreference(preference);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.UPDATE_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot update preference. Please contact administrator", Preference = null };
            }
        }

        /// <summary>
        /// This method imports AllocationOperationPreference
        /// </summary>
        /// <param name="preference">Preference which to be imported</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportPreference(AllocationOperationPreference preference)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.ImportPreference(preference);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot import preference. Please contact administrator", Preference = null };
            }
        }

        /// <summary>
        /// This method imports AllocationMasterFundPreference
        /// </summary>
        /// <param name="preference">Preference which to be imported</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportMasterfundPreference(AllocationMasterFundPreference preference, List<AllocationOperationPreference> mfcalculatedPref)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.ImportMasterFundPreference(preference, mfcalculatedPref);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot import preference. Please contact administrator", Preference = null };
            }
        }

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>AllocationOperationPreference.</returns>
        public AllocationOperationPreference GetPreferenceById(int preferenceId)
        {
            try
            {
                return PreferenceManager.GetInstance.GetPreferenceById(preferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the allocation preference name by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        public string GetAllocationPreferenceNameById(int preferenceId)
        {
            string prefName = string.Empty;
            try
            {
                prefName = PreferenceManager.GetInstance.GetAllocationPreferenceNameById(preferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return prefName;
        }

        /// <summary>
        /// Gets the allocation preference Id by Name.
        /// </summary>
        /// <param name="prefName">The preference Name.</param>
        /// <returns></returns>
        public int GetAllocationPreferenceIdByName(string prefName)
        {
            int prefId = 0;
            try
            {
                prefId = PreferenceManager.GetInstance.GetAllocationPreferenceIdByName(prefName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return prefId;
        }

        /// <summary>
        /// If masterfund allocation is enabled then returns master fund related allocation operation preferences, other wise return account wise allocation preferences set for given company
        /// </summary>
        /// <param name="companyId">Id of the company for which preference is required</param>
        /// <param name="userId"></param>
        /// <returns>
        /// Collection of AllocationOperationPreference objects
        /// </returns>
        public List<AllocationOperationPreference> GetCalculatedPreferencesByCompanyId(int companyId, int userId)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_OPERATION_PREFERENCE);
                List<AllocationOperationPreference> allocationOperationPreferences = PreferenceManager.GetInstance.GetCalculatedPreferencesByCompanyId(companyId, userId);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_ALLOCATION_OPERATION_PREFERENCE);
                return allocationOperationPreferences;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Adds the given preference information to database and cache
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId of the preference</param>
        /// <param name="allocationPrefType"></param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>Update result of the preference</returns>
        public PreferenceUpdateResult AddPreference(string name, int companyId, AllocationPreferencesType allocationPrefType, bool isPrefVisible, string rebalancerFileName = "")
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.ADD_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.AddPreference(name, companyId, allocationPrefType, isPrefVisible, rebalancerFileName);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.ADD_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot add preference. Please contact adminstrator" };
            }
        }

        /// <summary>
        /// Copy preference with new name
        /// </summary>
        /// <param name="preferenceId">preferenceId from which to be copied</param>
        /// <param name="name">Name of the new preference</param>
        /// <returns>PreferenceUpdate result</returns>
        public PreferenceUpdateResult CopyPreference(int preferenceId, string name, AllocationPreferencesType prefType)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.COPY_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.CopyPreference(preferenceId, name, prefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.COPY_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot copy preference. Please contact adminstrator" };
            }
        }

        /// <summary>
        /// Delete preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be deleted</param>
        /// <param name="allocationPrefType"></param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        public PreferenceUpdateResult DeletePreference(int preferenceId, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.DELETE_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.DeletePreference(preferenceId, allocationPrefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.DELETE_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot delete preference. Please contact adminstrator" };
            }
        }

        /// <summary>
        /// Renames preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be Renamed</param>
        /// <param name="name">New name of preference which to be Renamed</param>
        /// <param name="allocationPrefType"></param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        public PreferenceUpdateResult RenamePreference(int preferenceId, string name, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.RENAME_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.RenamePreference(preferenceId, name, allocationPrefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + preferenceId + AllocationLoggingConstants.PREFERENCE_NAME + name + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.RENAME_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot rename preference. Please contact administrator" };
            }
        }

        /// <summary>
        /// Saves Default rule for company.
        /// </summary>
        /// <param name="defaultRule">Allocation rule</param>
        /// <param name="companyId">Company id</param>
        /// <returns></returns>
        public bool SaveCompanyWisePreference(AllocationCompanyWisePref defaultPref)
        {
            try
            {
                bool result = PreferenceManager.GetInstance.SaveCompanyWisePreference(defaultPref);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Gets Default rule for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>AllcoationRule</returns>
        public AllocationCompanyWisePref GetCompanyWisePreference(int companyId)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.GET_COMPANYWISE_PREFERENCE_DEFAULT_RULE);
                AllocationCompanyWisePref rule = PreferenceManager.GetInstance.GetCompanyWisePreference(companyId);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_COMPANYWISE_PREFERENCE_DEFAULT_RULE);
                return rule;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>
        /// AllocationOperationPreference.
        /// </returns>
        public AllocationOperationPreference GetPreferenceById(int companyId, int userId, int preferenceId)
        {
            AllocationOperationPreference preference = null;
            try
            {
                preference = PreferenceManager.GetInstance.GetPreferenceById(preferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return preference;
        }

        public Dictionary<int, string> GetInvisibleAllocationPreferences()
        {
            Dictionary<int, string> preferencesDict = new Dictionary<int, string>();
            try
            {
                preferencesDict = PreferenceManager.GetInstance.GetInvisibleAllocationPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return preferencesDict;
        }

        /// <summary>
        /// Gets the allocation preferences, If MF allocation is enabled then this method returns MF preferences list otherwise account level calculated  preferences
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllocationPreferences(int companyId, int userId, bool isLevelingPerferenceRequired, bool isProrataByNavPerferenceRequired)
        {
            Dictionary<int, string> preferencesDict = new Dictionary<int, string>();
            try
            {
                preferencesDict = PreferenceManager.GetInstance.GetAllocationPreferences(companyId, userId, isLevelingPerferenceRequired, isProrataByNavPerferenceRequired);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return preferencesDict;
        }

        /// <summary>
        /// Gets the master fund preference by company identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId(int companyId, int userId)
        {
            try
            {
                List<AllocationMasterFundPreference> allocationOperationPreferences = PreferenceManager.GetInstance.GetMasterFundPrefByCompanyId(companyId, userId);
                return allocationOperationPreferences;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Saves the master fund preference.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <param name="mfCalculatedPrefs"></param>
        /// <returns></returns>
        public PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs)
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.SAVE_MF_ALLOCATION_PREFERENCE);
                PreferenceUpdateResult result = PreferenceManager.GetInstance.SaveMasterFundPreference(mfPreference, mfCalculatedPrefs);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.SAVE_MF_ALLOCATION_PREFERENCE);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot update masterfund preference. Please contact administrator", MasterFundPreference = null };
            }
        }

        /// <summary>
        /// Gets the master fund preference by identifier.
        /// </summary>
        /// <param name="mfPreferenceId">The mf preference identifier.</param>
        /// <returns></returns>
        public AllocationMasterFundPreference GetMasterFundPreferenceById(int mfPreferenceId)
        {
            AllocationMasterFundPreference mfPref = new AllocationMasterFundPreference();
            try
            {
                mfPref = PreferenceManager.GetInstance.GetMasterFundPreferenceById(mfPreferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return mfPref;
        }

        /// <summary>
        /// This method refreshes the allocation preference cache
        /// </summary>
        public void RefreshAllocationPreferenceCache()
        {
            try
            {
                PreferenceManager.GetInstance.RefreshPreferenceCache();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Get and save groups
        /// <summary>
        /// Returns the list of group for given date range along with the filter list provided
        /// </summary>
        /// <param name="toDate">Date upto which data is required (including)</param>
        /// <param name="fromTime">Date from which data is required (including)</param>
        /// <param name="filterList">List of filter which will be applied</param>
        /// <param name="userId">User which requires the data. UserId as -1 if not from any client</param>
        /// <returns>List of allocation groups</returns>
        public List<AllocationGroup> GetGroups(DateTime toDate, DateTime fromTime, AllocationPrefetchFilter filterList, int userId = -1)
        {
            try
            {
                Logger.LoggerWrite("Fetching Groups, fromTime:" + fromTime + ", toDate" + toDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_ALLOCATION_SERVICE);

                lock (_locker)
                {
                    AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_DATA);
                    // Clearing the state which is defined for given user. 
                    // Before calling this either any changes done by current user should already have been saved or not required
                    if (userId != -1)
                        UserWiseStateCache.Instance.ClearStateForUser(userId);
                    Task<List<AllocationGroup>> allocationGroups = AllocationGroupCache.Instance.GetGroupsAsync(toDate, fromTime, filterList, userId);
                    AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_ALLOCATION_DATA);
                    return allocationGroups.Result;
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
                return null;
            }
        }

        /// <summary>
        /// Removes the manual execution.
        /// </summary>
        /// <param name="clOrderId">The cl order identifier.</param>
        public bool RemoveManualExecution(string clOrderId, DateTime orderDate)
        {
            bool result = false;
            try
            {
                lock (_locker)
                {
                    AllocationPrefetchFilter filter = new AllocationPrefetchFilter();
                    DateTime toDate = DateTime.UtcNow.Date;
                    DateTime fromDate = orderDate.AddDays(-1);
                    filter.Allocated.Add("AccountID", string.Join(",", Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccounts().Keys));
                    filter.Allocated.Add("FromDate", fromDate.ToString());
                    List<AllocationGroup> allocationGroups = AllocationGroupCache.Instance.GetGroupsAsync(toDate, fromDate, filter, -1).Result;

                    AllocationGroup group = allocationGroups.FirstOrDefault(grp => grp.Orders.Any(ord => clOrderId.Equals(ord.ClOrderID)));

                    if (group != null && group.Orders.Count == 1)
                    {
                        StateCacheStore.Instance.UpdateCache(new List<AllocationGroup> { group }, -1);
                        UserWiseStateCache.Instance.UpdateStateForUser(new List<AllocationGroup> { group }, -1);
                        group.CumQty = 0;
                        //needed for reset taxlot dictionary
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;
                        if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            foreach (TaxLot taxlot in group.TaxLots)
                            {
                                taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                            }
                        }
                        group.ResetTaxlotDictionary(group.TaxLots);
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                        List<AllocationGroup> deletedGroupList = new List<AllocationGroup> { group };
                        SaveGroupsForFills(deletedGroupList, string.Empty, true);
                        PublishingHelper.Publish(deletedGroupList, -1, true, true, true);
                        result = true;
                    }
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
            return result;
        }

        /// <summary>
        /// Reallocates the group blotter.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="allocationParameter">The allocation parameter.</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public AllocationResponse ReallocateGroup_Blotter(string groupId, AllocationParameter allocationParameter, int preferenceId, int userId, string clOrderId)
        {
            bool isReallocatedFromBlotter = true;
            AllocationResponse allocationResponse = new AllocationResponse();
            try
            {
                lock (_locker)
                {
                    Expression<Func<AllocationGroup, bool>> predicate = un => groupId.Equals(un.GroupID);
                    List<AllocationGroup> oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                    if (oldAllocationGroups != null && oldAllocationGroups.Count > 0)
                    {
                        List<AllocationGroup> clonedGroups = DeepCopyHelper.Clone(oldAllocationGroups);
                        if (allocationParameter != null)
                        {
                            clonedGroups.ForEach(x => x.AllocationSchemeID = 0);
                            allocationResponse = AllocateByParameter(clonedGroups, allocationParameter, false, false, isReallocatedFromBlotter);
                        }
                        else
                            allocationResponse = AllocateByPreference(clonedGroups, preferenceId, userId, false, false, false, isReallocatedFromBlotter);
                        if (allocationResponse.Response == string.Empty && allocationResponse.GroupList != null && allocationResponse.GroupList.Count > 0)
                        {
                            allocationResponse.GroupList.ForEach(grp =>
                            {
                                grp.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                            });
                            List<AllocationGroup> resultGroups = SaveGroups(allocationResponse.GroupList, userId, true);
                            if (resultGroups != null && resultGroups.Count > 0)
                            {
                                // StateCacheStore.Instance.UpdateCache(oldAllocationGroups, -1);
                                //StateCacheStore.Instance.UpdateCache(resultGroups, 1);
                                allocationResponse.OldAllocationGroups = oldAllocationGroups;
                                PublishingHelper.Publish(allocationResponse.GroupList, -1, true, true);
                            }
                            else
                                allocationResponse.Response = "Failed to allocate the order, please check the logs.";
                        }
                    }
                    else
                        allocationResponse.Response = "Failed to allocate the order, please check if order has been grouped.";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allocationResponse;
        }

        /// <summary>
        /// Save the given list of allocation group provided
        /// </summary>
        /// <param name="groups">List of allocation groups provided</param>
        /// <param name="userId">UserId who requested save</param>
        /// <returns>true if saved successfully, otherwise false</returns>
        public List<AllocationGroup> SaveGroups(List<AllocationGroup> groups, int userId, bool isComingForReallocation = false)
        {
            Logger.LoggerWrite("Saving Groups by userId:" + userId, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_ALLOCATION_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"Groups", groups},
                {"UserId", userId}
            });

            try
            {
                lock (_locker)
                {
                    bool result = GroupCache.GetInstance().CheckGroupsForQuantityChange(groups);
                    if (result)
                    {
                        AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.SAVE_GROUPS);
                        // Mark Groups dirty if any user saved changes from allocation UI manually, i.e. User Id is not -1
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-8041

                        GroupCache.GetInstance().MarkGroupDirty(groups);

                        List<string> groupIdList = (from g in groups select g.GroupID).ToList();
                        Expression<Func<AllocationGroup, bool>> predicate = un => groupIdList.Contains(un.GroupID);
                        List<AllocationGroup> oldAllocationGroup = AllocationGroupCache.Instance.GetGroups(predicate);

                        oldAllocationGroup.ForEach(grp => GroupCache.GetInstance().DeleteFromAutoGroupingCache(grp));
                        StateCacheStore.Instance.UpdateCache(oldAllocationGroup, -1);

                        int saveResult = AllocationGroupCache.Instance.SaveGroups(groups);
                        Logger.LoggerWrite("Saved Groups. Result : " + saveResult, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_ALLOCATION_SERVICE);
                        bool isNormalPublish= ValidatePublishOnlyForWeb(groups);
                        if (saveResult > 0 && isNormalPublish)
                        {
                            StateCacheStore.Instance.UpdateCache(groups, 1);
                            if (userId > 0)
                            {
                                UserWiseStateCache.Instance.ClearStateForUser(userId);
                                AllocationPostInfoCache.Instance.UpdatePostInfoCacheFromUser(userId, groupIdList);
                            }

                            // added logging when currency ID is blank or have int.MinValue, in this case Currency will be blank, PRANA-10662
                            groups.ForEach(x => { if (string.IsNullOrWhiteSpace(x.CurrencyID.ToString()) || x.CurrencyID == int.MinValue) Logger.LoggerWrite(string.Format("Currency ID is blank for {0} trade having groupID: {1}", x.Symbol, x.GroupID)); });

                            #region Account Reallocation Update For Multi Day Trades
                            if (isComingForReallocation)
                            {
                                foreach (AllocationGroup group in groups)
                                {
                                    foreach (AllocationOrder order in group.Orders)
                                    {
                                        var clOrderId = order.ClOrderID;
                                        var latestClOrderIdAfterReplace = OrderCacheManager.DictMultiDayClOrderIDMapping.ContainsKey(clOrderId) ? OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId] : string.Empty;
                                        var parentClOrderId = latestClOrderIdAfterReplace;
                                        var latestClOrderId = OrderCacheManager.StagedSubsCollection.ContainsKey(parentClOrderId) ? OrderCacheManager.StagedSubsCollection[parentClOrderId].dictOrderIDWiseNewCLOrderIDs.Values.OrderByDescending(x => x.clOrderID).ToList()[0].clOrderID : string.Empty;
                                        if (latestClOrderId == clOrderId)
                                        {
                                            if (OrderCacheManager.DictMultiDayOrderOriginalClOrderIdAfterReplace.ContainsKey(parentClOrderId))
                                            {
                                                parentClOrderId = OrderCacheManager.DictMultiDayOrderOriginalClOrderIdAfterReplace[parentClOrderId];
                                            }
                                            var msg = OrderCacheManager.GetCachedOrder(clOrderId);
                                            if (msg != null && OrderCacheManager.HasMultiDayHistory(msg))
                                            {
                                                OrderCacheManager.SaveMultiDayOrderAllocation(latestClOrderIdAfterReplace, group.GroupID);
                                                OrderCacheManager.SaveMultiDayOrderAllocation(parentClOrderId, group.GroupID);
                                                if (group != null)
                                                {
                                                    order.ParentClOrderID = parentClOrderId;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            PranaMessage pranaMessage = OrderCacheManager.GetCachedOrder(clOrderId);
                                            if (pranaMessage != null && OrderCacheManager.HasMultiDayHistory(pranaMessage))
                                            {
                                                order.ParentClOrderID = clOrderId;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            PublishingHelper.Publish(groups, userId, true, false);
                        }
                        else
                        {
                            if(!isNormalPublish)
                                PublishingHelper.PublishGroupToWeb(groups, userId);
                            StateCacheStore.Instance.UpdateCache(oldAllocationGroup, 1);
                            return new List<AllocationGroup>();
                        }
                        TradeAttributeCache.Instance.UpdateTradeAttribLists(groups);
                        AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userId + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.SAVE_GROUPS);
                        return groups;
                    }
                    else
                    {
                        return new List<AllocationGroup>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_ALLOCATION_SERVICE);
                return null;
            }
        }

        /// <summary>
        /// Determines whether publishing the allocation group update should be skipped.
        /// This is applicable only in Web context (not Enterprise),
        /// and the publishing is done if the first group's cumulative quantity is greater than zero.
        /// </summary>
        /// <param name="groups">List of allocation groups</param>
        /// <returns>
        /// True if publishing should be done (i.e., CumQty > 0 in the first group), otherwise false.
        /// </returns>
        private bool ValidatePublishOnlyForWeb(List<AllocationGroup> groups)
        {
            // Ensure the group list is not null or empty
            if (groups == null || groups.Count == 0)
                return false;

            // If the first group's CumQty is greater than 0, publish it
            return groups[0].CumQty > 0;
        }
        /// <summary>
        /// This method gets taxlot from old Allocation group if changes have been made to the values Trade Attributes
        /// </summary>
        /// <param name="lstOldAllocationGroup"></param>
        /// <param name="lstUpdatedAllocationGroup"></param>
        /// <returns></returns>
        private List<TaxLot> GetUpdatedTaxlotsFromOldAllocationGroup(List<AllocationGroup> lstOldAllocationGroup, List<AllocationGroup> lstUpdatedAllocationGroup)
        {
            List<TaxLot> updatedTaxlots = new List<TaxLot>();
            try
            {
                List<TaxLot> taxlotsOldAllocationGroup = (from oldAllocationGroup in lstOldAllocationGroup 
                                                          from groupAllTaxlots in oldAllocationGroup.GetAllTaxlots()
                                                          select groupAllTaxlots).ToList();
                List<TaxLot> taxlotsUpdatedAllocationGroup = (from UpdatedAllocationGroup in lstUpdatedAllocationGroup
                                                              from groupAllTaxlots in UpdatedAllocationGroup.GetAllTaxlots()
                                                          select groupAllTaxlots).ToList();
                foreach(TaxLot updatedTaxlot in taxlotsUpdatedAllocationGroup)
                {
                    if(updatedTaxlot.TaxLotState == ApplicationConstants.TaxLotState.Updated)
                    {
                        TaxLot oldTaxlot = taxlotsOldAllocationGroup.FirstOrDefault(x => x.TaxLotID == updatedTaxlot.TaxLotID);
                        if (oldTaxlot != null && !updatedTaxlots.Contains(oldTaxlot))
                        {
                            if (updatedTaxlot.TradeAttribute1 != oldTaxlot.TradeAttribute1 || updatedTaxlot.TradeAttribute2 != oldTaxlot.TradeAttribute2 ||
                                updatedTaxlot.TradeAttribute3 != oldTaxlot.TradeAttribute3 || updatedTaxlot.TradeAttribute4 != oldTaxlot.TradeAttribute4 ||
                                updatedTaxlot.TradeAttribute5 != oldTaxlot.TradeAttribute5 || updatedTaxlot.TradeAttribute6 != oldTaxlot.TradeAttribute6)
                            {
                                if (string.IsNullOrWhiteSpace(oldTaxlot.OrderSide))
                                    oldTaxlot.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(oldTaxlot.OrderSideTagValue);
                                //Setting Level1ID as -1 for unallocated trades.
                                if (oldTaxlot.Level1ID.Equals(0))
                                    oldTaxlot.Level1ID = -1;
                                updatedTaxlots.Add(oldTaxlot);
                            }
                        }
                    }
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
            return updatedTaxlots;
        }

        /// <summary>
        /// Update the state of allocation for groups provided
        /// </summary>
        /// <param name="groups">List of allocation groups provided</param>
        /// <returns>true if updated successfully, otherwise false</returns>
        public bool UpdateState(List<AllocationGroup> groups)
        {
            try
            {
                //Removing old state then adding new state.
                List<string> groupIdList = (from g in groups select g.GroupID).ToList();
                Expression<Func<AllocationGroup, bool>> predicate = un => groupIdList.Contains(un.GroupID);
                List<AllocationGroup> oldAllocationGroup = AllocationGroupCache.Instance.GetGroups(predicate);

                StateCacheStore.Instance.UpdateCache(oldAllocationGroup, -1);
                UserWiseStateCache.Instance.UpdateStateForUser(oldAllocationGroup, -1);

                bool resultGroupCache = AllocationGroupCache.Instance.UpdateGroupsCache(groups);

                if (resultGroupCache)
                {
                    bool resultState = StateCacheStore.Instance.UpdateCache(groups, 1);
                    if (resultState)
                        UserWiseStateCache.Instance.UpdateStateForUser(groups, 1);
                    return true;
                }
                else
                    return false;// && resultGroupCache;                    
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
                return false;
            }
        }

        /// <summary>
        /// Get groups list for groups Id list
        /// </summary>
        /// <param name="groupIdList">List of group id</param>
        /// <returns>List of groups</returns>
        public List<AllocationGroup> GetGroupsById(List<string> groupIdList)
        {
            try
            {
                Expression<Func<AllocationGroup, bool>> predicate = un => groupIdList.Contains(un.GroupID);
                return AllocationGroupCache.Instance.GetGroups(predicate);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
        #endregion

        #region Calculate Commission
        /// <summary>
        /// this method calculates commission for group when counter party is changed
        /// </summary>
        /// <param name="group">the allocation group</param>
        public AllocationGroup CalculateCommission(AllocationGroup group)
        {
            try
            {
                if (group.IsRecalculateCommission)
                {
                    group.CommissionSource = (int)CommisionSource.Auto;
                    group.SoftCommissionSource = (int)CommisionSource.Auto;
                    group.CommSource = CommisionSource.Auto;
                    group.SoftCommSource = CommisionSource.Auto;
                    group.IsRecalculateCommission = false;
                }
                CommissionCalculator.GetInstance.StartCalculation(group);
                return group;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        #region Re-Calculate Other Fee
        /// <summary>
        /// this method calculates commission for group when counter party is changed
        /// </summary>
        /// <param name="group">the allocation group</param>
        public AllocationGroup ReCalculateOtherFeeForGroup(AllocationGroup group, List<OtherFeeType> listofFeesToApply)
        {
            try
            {
                CommissionCalculator.GetInstance.ReCalculateOtherFeeForGroup(group, listofFeesToApply);
                return group;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="groupList"></param>
        /// <param name="isGroupWise"></param>
        /// <returns></returns>
        public List<AllocationGroup> ApplyCommissionBulkChange(CommissionRule commissionRule, List<AllocationGroup> groupList, bool isGroupWise)
        {
            try
            {
                return CommissionCalculator.GetInstance.ApplyCommissionBulkChange(commissionRule, groupList, isGroupWise);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return new List<AllocationGroup>();
            }
        }
        #endregion

        #region Misc operations
        /// <summary>
        /// returns List of Trade attributes.
        /// </summary>
        /// <returns></returns>
        public List<string>[] GetTradeAttributes()
        {
            return TradeAttributeCache.Instance.GetTradeAttributes();
        }

        /// <summary>
        /// Update Trade attribute cache.
        /// </summary>
        /// <param name="groupList"></param>
        public void UpdateAttribList(List<AllocationGroup> groupList)
        {
            try
            {
                TradeAttributeCache.Instance.UpdateTradeAttribLists(groupList);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get master funds
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMasterFundsRatio()
        {
            DataSet ds = null;
            try
            {
                ds = PreferenceManager.GetInstance.GetAllMasterFundsRatio();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        /// Save master fund target ratio
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SaveMasterFundTargetRatio(DataSet ds)
        {
            bool result = false;
            try
            {
                result = PreferenceManager.GetInstance.SaveMasterFundTargetRatio(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Save Attribute names
        /// </summary>
        /// <param name="ds"></param>
        public void SaveAttributeNames(DataSet ds)
        {
            try
            {
                PreferenceManager.GetInstance.SaveAttributeNames(ds);
                TradeAttributeCache.Instance.RefreshAttributeCache();
                PublishingHelper.PublishTradeAttributePreferences(ds);
                PublishingHelper.PublishPreferenceUpdate(-1, -1);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the attribute names.
        /// </summary>
        /// <returns></returns>
        public DataSet GetAttributeNames()
        {
            try
            {
                return PreferenceManager.GetInstance.GetAttributeNames();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        /// <summary>
        /// Gets the current state for symbolList.
        /// </summary>
        /// <param name="symbolList">The symbolList.</param>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public List<CurrentSymbolState> GetCurrentStateForSymbol(List<string> symbolList, int userID)
        {
            List<CurrentSymbolState> currentStateCollection = new List<CurrentSymbolState>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.GET_CURRENT_STATE_FOR_SYMBOL);
                foreach (string symbol in symbolList)
                {
                    Dictionary<int, AccountValue> CumQtyState = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.CumQuantity, symbol, userID);
                    Dictionary<int, AccountValue> NotionalState = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.Notional, symbol, userID);
                    if (CumQtyState == null && NotionalState == null)
                    {
                        CurrentSymbolState currentState = new CurrentSymbolState
                        {
                            Symbol = symbol,
                            AccountId = -1,
                            AccountName = "No State for Symbol",
                            Quantity = 0.0M,
                            Notional = 0.0M
                        };
                        currentStateCollection.Add(currentState);
                        continue;
                    }
                    if (CumQtyState == null)
                        CumQtyState = new Dictionary<int, AccountValue>();
                    if (NotionalState == null)
                        NotionalState = new Dictionary<int, AccountValue>();
                    List<int> totalFunds = CumQtyState.Keys.ToList().Concat(NotionalState.Keys.ToList().Where(kvp => !CumQtyState.ContainsKey(kvp))).ToList();

                    foreach (int accountID in totalFunds)
                    {
                        if ((CumQtyState.ContainsKey(accountID) && CumQtyState[accountID].Value == 0) && (NotionalState.ContainsKey(accountID) && NotionalState[accountID].Value == 0)
                            || (!CumQtyState.ContainsKey(accountID)) && (NotionalState.ContainsKey(accountID) && NotionalState[accountID].Value == 0)
                            || (!NotionalState.ContainsKey(accountID)) && (CumQtyState.ContainsKey(accountID) && CumQtyState[accountID].Value == 0)
                            )
                        {
                            continue;
                        }
                        CurrentSymbolState currentState = new CurrentSymbolState
                        {
                            Symbol = symbol,
                            AccountId = accountID,
                            AccountName = CachedDataManager.GetInstance.GetAccountText(accountID),
                            Quantity = (CumQtyState.ContainsKey(accountID)) ? CumQtyState[accountID].Value : 0.0M,
                            Notional = (NotionalState.ContainsKey(accountID)) ? NotionalState[accountID].Value : 0.0M
                        };
                        currentStateCollection.Add(currentState);

                    }
                    if (!currentStateCollection.Any(item => item.Symbol.Equals(symbol)))
                    {
                        CurrentSymbolState currentState = new CurrentSymbolState
                        {
                            Symbol = symbol,
                            AccountId = -1,
                            AccountName = "No State for Symbol",
                            Quantity = 0.0M,
                            Notional = 0.0M
                        };
                        currentStateCollection.Add(currentState);
                    }
                }
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_CURRENT_STATE_FOR_SYMBOL);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return currentStateCollection;
        }

        /// <summary>
        /// GetCurrentStateForSymbolInAccount
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userID"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public decimal GetCurrentStateForSymbolInAccount(string symbol, int userID, int accountId)
        {
            decimal quantity = 0;
            try
            {
                Dictionary<int, AccountValue> CumQtyState = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.CumQuantity, symbol, userID);
                if (CumQtyState != null)
                {
                    foreach (int account in CumQtyState.Keys)
                    {
                        if (account == accountId)
                        {
                            quantity = CumQtyState[account].Value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return quantity;
        }

        #endregion

        #region Calculate Accrued Interest

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
                //SecMasterBaseObj secMasterObj = ServiceConnector.SecmasterProxy.GetSecMasterDataForSymbol(allocationGroup.Symbol);
                //if (secMasterObj != null)
                //{
                //    int daysToSettlementFixedIncome = ((SecMasterFixedIncome)secMasterObj).DaysToSettlement;
                //    if (daysToSettlementFixedIncome > 0)
                //        allocationGroup.SettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(allocationGroup.ProcessDate, daysToSettlementFixedIncome, allocationGroup.AUECID);
                //    else
                //        allocationGroup.SettlementDate = allocationGroup.ProcessDate;
                //}
                List<AllocationGroup> groupList = new List<AllocationGroup> { (AllocationGroup)allocationGroup.Clone() };
                List<PranaBasicMessage> updatedList = ServiceProxyConnector.CashManagementService.CalculateAccruedInterest(groupList.Cast<PranaBasicMessage>().ToList());
                accuredInterest = updatedList[0].AccruedInterest;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return accuredInterest;
        }
        #endregion

        #endregion

        #region IAllocationManager Members GroupCache methods used in PostTradeCacheManager

        /// <summary>
        /// Create a group and check if it is dirty group
        /// </summary>
        /// <param name="order">the order</param>
        /// <param name="isDirty">Dirty group: suppose a partially filled unAllocated trade comes through drop copy, in the mean while we allocate it from Allocation UI,
        /// this group is marked as Dirty and kept it in group cache. When next fill comes, we check it in the Dirty group cache, if exists, we update it as per the new fill.</param>
        /// <returns>the allocation group</returns>
        public AllocationGroup CreateGroup(Order order, ref bool isDirty, bool isReal)
        {
            try
            {
                return GroupCache.GetInstance().CreateGroup(order, ref isDirty, isReal);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets GroupID for order
        /// </summary>
        public string GetGroupID(string parentClOrderID)
        {
            try
            {
                return GroupCache.GetInstance().GetGroupID(parentClOrderID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Add the allocation group
        /// </summary>
        /// <param name="group">The allocation group</param>
        public void AddGroup(AllocationGroup group)
        {
            try
            {
                UpdateState(new List<AllocationGroup> { group });
                GroupCache.GetInstance().AddGroup(group);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// checks if groups are unsaved
        /// </summary>
        /// <returns>true if groups are unsaved, false otherwise</returns>
        public bool CheckIfUnsavedGroups()
        {
            return GroupCache.GetInstance().CheckIfUnsavedGroups();
        }

        /// <summary>
        /// mark group as dirty
        /// </summary>
        /// <param name="clonedGroup">the allocation group</param>
        private void MarkGroupDirty(AllocationGroup clonedGroup)
        {
            try
            {
                GroupCache.GetInstance().MarkGroupDirty(clonedGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        #region Data Corruption Changes Methods

        /// <summary>
        /// Creates the real virtual allocation group.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="isCached">if set to <c>true</c> [is cached].</param>
        /// <param name="isReal">if set to <c>true</c> [is real].</param>
        /// <returns></returns>
        public AllocationGroup CreateRealVirtualAllocationGroup(Order order, bool isCached, bool isReal, bool isForceAllocation = false)
        {
            AllocationGroup group = null;
            try
            {
                if (isReal)
                {
                    //take lock in case of real group so as to synchronize data
                    lock (_locker)
                    {
                        group = CreateAllocationGroup(ref order, isCached, isReal, isForceAllocation);
                    }
                }
                else
                    //create virtual group without any lock
                    group = CreateAllocationGroup(ref order, isCached, isReal, isForceAllocation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return group;
        }

        /// <summary>
        /// Automatics the group incoming groups.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="level1ID">The level1 identifier.</param>
        /// <param name="autoGroupingRules">The automatic grouping rules.</param>
        /// <param name="autoGroupingFunds">The automatic grouping funds.</param>
        /// <returns></returns>
        private bool AutoGroupIncomingGroups(ref AllocationGroup group, int level1ID, AutoGroupingRules autoGroupingRules, ref List<AllocationGroup> deletedGroupsToRemove)
        {
            try
            {
                Object[] temp = AllocationAutoGroupingHelper.GroupWithExisting(autoGroupingRules, group);
                if (((List<AllocationGroup>)temp[0]).Count > 0)
                {
                    group = ((List<AllocationGroup>)temp[1])[0];
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.New;
                    GroupCache.GetInstance().UpdateAutoGroupsCache(group);
                    List<AllocationGroup> deletedGroups = (List<AllocationGroup>)temp[0];
                    foreach (AllocationGroup grp in deletedGroups)
                    {
                        grp.CumQty = 0;
                        GroupCache.GetInstance().UpdateAutoGroupsCache(grp);
                        grp.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;
                    }
                    PublishingHelper.Publish(deletedGroups, -1, true, false);
                    MarkGroupDirty((AllocationGroup)group.Clone());
                    if (level1ID > 0)
                    {
                        AllocationParameter parameter = new AllocationParameter(null, null, int.MinValue, -1, true);
                        AllocationPostInfoCache.Instance.AddUpdateParameterForGroupFromTT(deletedGroups, ref parameter);
                    }
                    deletedGroupsToRemove = deletedGroups;
                    AuditManager.Instance.SaveAuditList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is automatic group enabled for preference] [the specified level1 identifier].
        /// </summary>
        /// <param name="level1ID">The level1 identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is automatic group enabled for preference] [the specified level1 identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAutoGroupEnabledForPref(int level1ID)
        {
            try
            {
                List<int> autoGroupingFunds = PreferenceManager.GetInstance.GetAutoGroupingFunds();
                IEnumerable<int> prefAccounts = GetSelectedAccountsFromPref(level1ID);
                if (prefAccounts != null && prefAccounts.All(fund => autoGroupingFunds.Contains(fund)))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Gets the selected accounts from preference.
        /// </summary>
        /// <param name="level1ID">The level1 identifier.</param>
        /// <returns></returns>
        public IEnumerable<int> GetSelectedAccountsFromPref(int level1ID)
        {
            IEnumerable<int> prefAccounts = null;
            if (level1ID <= 0)
                prefAccounts = new List<int>() { 0 };
            else if (!string.IsNullOrWhiteSpace(CachedDataManager.GetInstance.GetAccountText(level1ID)))
                prefAccounts = new List<int>() { level1ID };
            else
            {
                AllocationOperationPreference calculatedPref = PreferenceManager.GetInstance.GetPreferenceById(level1ID);
                if (calculatedPref != null)
                {
                    if (calculatedPref.IsMatchClosingUsed() && StateCacheStore.Instance.MatchClosingTransactionAtPortfolioOnly)
                        prefAccounts = CachedDataManager.GetInstance.GetAccounts().Keys;
                    else
                        prefAccounts = calculatedPref.GetSelectedAccountsList(true);
                }
                else
                {
                    AllocationMasterFundPreference masterFundPref = PreferenceManager.GetInstance.GetMasterFundPreferenceById(level1ID);
                    if (masterFundPref != null)
                    {
                        if (masterFundPref.IsMatchClosingUsed())
                            prefAccounts = CachedDataManager.GetInstance.GetAccounts().Keys;
                        else
                            prefAccounts = MasterFundAllocationHelper.GetSelectedAccountsList(masterFundPref.MasterFundPreference.Keys.ToList());
                    }
                }
            }

            return prefAccounts;
        }

        /// <summary>
        /// Creates the real/virtual allocation group.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="isCached">if set to <c>true</c> [is cached].</param>
        /// <param name="isReal">if set to <c>true</c> [is real].</param>
        /// <returns></returns>
        private AllocationGroup CreateAllocationGroup(ref Order order, bool isCached, bool isReal, bool isForceAllocation = false)
        {
            AllocationGroup group = null;
            bool isBustOrder = false;
            bool setNameFromFile = false;
            try
            {
                bool isDirty = false;
                // Sandeep Singh,July 2, 2013: this is used to create a group from an order and checks is group dirty
                // Dirty group: suppose a partially filled unAllocated trade comes through drop copy, in the mean while we allocate it from Allocation UI,
                // this group is marked as Dirty and kept it in the group cache. When next fill comes, we check it in the Dirty group cache, if exists, we update it as per the new fill.
                // Moved the GroupCache to CacheStore in Allocation.Core, so called this function through AllocationManager
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7506

                if (order.MsgType != FIXConstants.MSGTransferUser)
                {
                    group = CreateGroup(order, ref isDirty, isReal);
                    AutoGroupingRules autoGroupingRules = PreferenceManager.GetInstance.GetAutoGroupingPreferences();

                    //Set order Side value if it is blank
                    if (string.IsNullOrWhiteSpace(group.OrderSide))
                        group.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(group.OrderSideTagValue);

                    //Getting Groups for group Id from Allocation Core as it contains groups with updated state.
                    List<string> groupIdList = new List<string>();
                    groupIdList.Add(group.GroupID);
                    List<AllocationGroup> groupList = null;
                    List<AllocationGroup> deletedGroupsToRemove = new List<AllocationGroup>();
                    if (autoGroupingRules != null && autoGroupingRules.AutoGroup && isReal && (order.OriginatorType == 3 || !string.IsNullOrEmpty(order.StagedOrderID)) && IsAutoGroupEnabledForPref(order.Level1ID))
                    {
                        AllocationGroup grp = GroupCache.GetInstance().AutoGroupingCache.Find(g => g.GroupID == group.GroupID);
                        if (grp != null)
                            groupList = new List<AllocationGroup>() { grp };
                    }
                    else
                        groupList = GetGroupsById(groupIdList);
                    bool isMerged = false;
                    bool groupAlreadyExist = false;
                    //Updating closing status for groups.
                    if (groupList != null && groupList.Count == 1)
                    {
                        groupAlreadyExist = true;
                        group.ClosingStatus = groupList[0].ClosingStatus;
                        if (groupList[0].CumQty != 0)
                        {
                            group.Commission = groupList[0].Commission;
                            if (groupList[0].IsCommissionChanged)
                            {
                                group.CommissionAmt = groupList[0].Commission;//Adding commission in commission amt as it is extra field for moving commission from TT and edit trades
                                group.CommissionRate = groupList[0].CommissionRate;
                                group.CommissionSource = groupList[0].CommissionSource;
                            }
                            group.CommissionText = groupList[0].CommissionText;

                            group.SoftCommission = groupList[0].SoftCommission;
                            if (groupList[0].IsSoftCommissionChanged)
                            {
                                group.SoftCommissionAmt = groupList[0].SoftCommission;//Adding SoftCommission in SoftCommission amt as it is extra field for moving commission from TT and edit trades
                                group.SoftCommissionCalcBasis = groupList[0].SoftCommissionCalcBasis;
                                group.SoftCommissionRate = groupList[0].SoftCommissionRate;
                                group.SoftCommissionSource = groupList[0].SoftCommissionSource;
                                group.SoftCommSource = groupList[0].SoftCommSource;
                            }
                        }
                    }
                    bool justReplaced = false;
                    if ((order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD) && (GroupCache.GetInstance().IsOrderInPendingReplaceState(order.StagedOrderID)
                        || GroupCache.GetInstance().IsOrderInPendingReplaceState(order.OrigClOrderID)) && order.MsgSeqNum != Int64.MinValue && order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PartiallyFilled))
                    {
                        justReplaced = true;
                        if (replacedOrders.ContainsKey(order.OrigClOrderID) && order.MsgSeqNum == replacedOrders[order.OrigClOrderID])
                        {
                            replacedOrders.Remove(order.OrigClOrderID);
                            GroupCache.GetInstance().removeFromPendingReplaceDict(order.OrigClOrderID);
                        }
                        else if (!replacedOrders.ContainsKey(order.OrigClOrderID))
                            replacedOrders.Add(order.OrigClOrderID, order.MsgSeqNum);
                    }
                    if (order.OrderStatusTagValue.Equals(FIXConstants.EXECTYPE_PendingReplace))
                        GroupCache.GetInstance().addInPendingReplaceDict(order.ParentClOrderID, order.ClOrderID);
                    if (((((group.TransactionSourceTag == (int)TransactionSource.TradingTicket) || (group.TransactionSourceTag == (int)TransactionSource.HotButton)) && group.IsManualGroup == false) || group.TransactionSourceTag == (int)TransactionSource.FIX) && !GroupCache.GetInstance().IsGrpExistInAutoGroupingCache(group.GroupID))
                    {
                        if (autoGroupingRules != null && autoGroupingRules.AutoGroup && !isDirty && isReal &&
                            (order.OriginatorType == 3 || (!string.IsNullOrEmpty(order.StagedOrderID) && order.MsgType.Equals(FIXConstants.MSGExecution)))
                            && IsAutoGroupEnabledForPref(order.Level1ID) && !order.IsMultiDayParentOrder && !justReplaced)
                        {
                            if (order.Level1ID > 0)
                            {
                                group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
                                group.AllocationSchemeID = order.Level1ID;
                                if (order.Level2ID > 0)
                                    group.StrategyID = order.Level2ID;
                            }
                            isMerged = AutoGroupIncomingGroups(ref group, order.Level1ID, autoGroupingRules, ref deletedGroupsToRemove);
                            if (isMerged)
                            {
                                AllocationGroup grp = GroupCache.GetInstance().AutoGroupingCache.Find(g => g.GroupID == group.GroupID);
                                if (grp != null)
                                    groupList = new List<AllocationGroup>() { grp };
                            }
                        }
                    }
                    if (!isDirty) // add allocation Details
                    {
                        ///MUKUL 20121002
                        //condition to check if the order has been busted..
                        if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated && group.CumQty <= 0 && order.MsgType != FIXConstants.MSGOrder)
                        {
                            isBustOrder = true;
                        }
                        #region PREALLOCATE
                        //Unwinding group
                        if (isReal)
                            UnwindIfReallocated(ref group);

                        group.IsPreAllocated = true;
                        setNameFromFile = false;

                        if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            isForceAllocation = true;
                        }
                        //If Level1ID is min value then allocationDetails will be null.

                        AllocationGroup removeGroup = (AllocationGroup)group.Clone(); ;
                        UpdateStateCache(removeGroup, -1, groupList);
                        //false as force allocation as check side need to be checked.
                        AllocationLevelList allocationDetails = GetAllocationDetails(order.Level1ID, "", order.Level2ID, group, order.CumQty, out setNameFromFile, !isCached, isForceAllocation, order.ClOrderID);
                        UpdateStateCache(removeGroup, 1, groupList);
                        UpdateGroupAndStateCache(deletedGroupsToRemove);

                        if (allocationDetails != null && allocationDetails.Collection.Count > 0)
                        {
                            if (isBustOrder)
                            {
                                //MUKUL 20121002 (Bust Trades Issue Resolved)
                                //Busted trades were not getting removed from PM since for busted groups the cumQty =0 and hence no taxlots were getting created (and therefore published) due to a check on cumqty!=0 in  ValidateAllocationDetailsAndAllocate function..
                                //For the Bust Information to flow properly we have created Dummy Taxlots(with original TaxlotID) and publish them with deleted state..
                                //In CreateTaxlots  new taxlot objects are created  On every subsequent fill but with original TaxlotID (groupID + FUNDID+ StrategyID) at runtime) which solves the purpose..
                                group.AddAccounts(allocationDetails);
                                List<TaxLot> dummyTaxlots = group.CreateTaxlots();

                                // resetting the Taxlot dictionary as this dictionary is being used to publish taxlots everywhere across application..
                                // Dummy taxlots have been only added to taxlot dictionary(only for publishing purpose) and not to the taxlots collection at group level as busted case handling at DB level has been done accordingly...
                                group.ResetTaxlotDictionary(dummyTaxlots);
                                group.TaxLots.Clear();
                            }
                            else if (groupList != null && groupList.Count > 0)
                            {
                                //Setting status of taxlots to updated as after fills status should be updated and groupList[0] contains old state so updating.
                                groupList[0].ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Updated);
                                //If old group contains taxlots then updating in new group from old.
                                List<TaxLot> taxlotList = new List<TaxLot>(groupList[0].GetAllTaxlots());
                                foreach (TaxLot taxlot in taxlotList)
                                {
                                    group.ResetTheResetDictionary(taxlot);
                                }
                            }
                            ValidateAllocationDetailsAndAllocate(group, ref allocationDetails, "", setNameFromFile);
                        }
                        else
                        {
                            group.IsPreAllocated = false;
                            order.Level1ID = int.MinValue;
                            //Unallocating group if check side fails.
                            if (groupList != null && groupList.Count > 0)
                            {
                                //Setting status of taxlots to updated as after fills status should be updated and groupList[0] contains old state so updating.
                                groupList[0].ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Updated);
                                //If old group contains taxlots then updating in new group from old.
                                // if (groupList[0].GetAllTaxlots() != null && groupList[0].GetAllTaxlots().Count > 0)
                                // {
                                List<TaxLot> taxlotList = new List<TaxLot>(groupList[0].GetAllTaxlots());
                                foreach (TaxLot taxlot in taxlotList)
                                {
                                    group.ResetTheResetDictionary(taxlot);
                                }
                                //}
                                //Unallocating group to generate deleted taxlot.
                                group.UnallocateGroup();
                            }
                            if (groupList == null)
                            {
                                group.UnallocateGroup();
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        // Sandeep Singh,July 2, 2013: this else part has been moved from GroupCache to here.
                        // If a group is marked as Dirty, then common code should be used to validate and allocate.
                        // before that group.ProrataAllocate method was used to validate and Allocate group.

                        group.UpdateOrder(order);
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        /* While adding fills to allocated group, first unwind the group if closed.
                         * Then create AllocationLevelList by using GetAllocationDetails() function which returns allocation level list after check side check
                         * If checkside fails, group is unallocated
                         * http://jira.nirvanasolutions.com:8080/browse/PRANA-8041
                         */

                        //Unwinding group
                        if (isReal)
                            UnwindIfReallocated(ref group);

                        //If Level1ID is min value then allocationDetails will be null, false as force allocation as check side need to be checked.
                        setNameFromFile = false;
                        AllocationGroup removeGroup = (AllocationGroup)group.Clone(); ;
                        UpdateStateCache(removeGroup, -1, null);
                        AllocationLevelList allocationDetails = GetAllocationDetails(order.Level1ID, "", order.Level2ID, group, order.CumQty, out setNameFromFile, !isCached, false, order.ClOrderID);
                        UpdateStateCache(removeGroup, 1, null);
                        UpdateGroupAndStateCache(deletedGroupsToRemove);

                        if (group.CumQty > 0 && group.Allocations.Collection.Count > 0)
                        {
                            group.AllocatedQty = group.CumQty;
                            if (allocationDetails != null && allocationDetails.Collection.Count > 0)
                                ValidateAllocationDetailsAndAllocate(group, ref allocationDetails, "", setNameFromFile);
                            else
                            {
                                UnAllocateGroupOnCheckSideFail(ref group, ref order, ref groupList);
                            }
                        }
                        if (!groupAlreadyExist)
                        {
                            group.CommissionAmt = order.CommissionAmt;
                            group.SoftCommissionAmt = order.SoftCommissionAmt;
                        }
                    }
                    if (group.TransactionSourceTag == (int)TransactionSource.FIX || group.Orders.All(ord => ord.TransactionSourceTag == (int)TransactionSource.FIX) || ((group.TransactionSourceTag == (int)TransactionSource.TradingTicket && group.IsManualGroup == false) && group.Orders.All(ords => ords.TransactionSourceTag == (int)TransactionSource.TradingTicket)))
                    {
                        int avgRounding = CachedDataManager.GetInstance.GetAvgPriceRounding();
                        if (avgRounding >= 0)
                        {
                            group.AvgPrice = Math.Round(group.AvgPrice, avgRounding, MidpointRounding.AwayFromZero);
                            group.UpdateTaxlotAvgPrice();
                        }
                    }
                    if (group.Orders.Count == 1)
                    {
                        if (order.TradeAttribute1 != string.Empty)
                            group.TradeAttribute1 = order.TradeAttribute1;
                        if (order.TradeAttribute2 != string.Empty)
                            group.TradeAttribute2 = order.TradeAttribute2;
                        if (order.TradeAttribute3 != string.Empty)
                            group.TradeAttribute3 = order.TradeAttribute3;
                        if (order.TradeAttribute4 != string.Empty)
                            group.TradeAttribute4 = order.TradeAttribute4;
                        if (order.TradeAttribute5 != string.Empty)
                            group.TradeAttribute5 = order.TradeAttribute5;
                        if (order.TradeAttribute6 != string.Empty)
                            group.TradeAttribute6 = order.TradeAttribute6;

                        // Set trade attribute values for non-empty entries
                        group.SetNonEmptyTradeAttributes(order.GetTradeAttributesAsDict());                       
                    }

                    CommissionCalculator.GetInstance.StartCalculation(group);
                    //CommissionAmt is the commission added on TT at order level
                    if (group.CommSource != CommisionSource.Auto)//Updating commission in any case (from TT or fills)
                    {
                        //As the commission amt is available on Prana basic message.If some commission is added on TT,We will copy it here to Group. 
                        group.Commission = group.CommissionAmt;
                    }
                    if (group.SoftCommSource != CommisionSource.Auto)
                    {
                        group.SoftCommission = group.SoftCommissionAmt;
                    }
                    group.DistributeCommisionInTaxLot(group.CommSource != CommisionSource.Auto, group.SoftCommSource != CommisionSource.Auto);

                    List<AllocationGroup> groups = new List<AllocationGroup>();
                    groups.Add(group);
                    if (isBustOrder && group.IsPreAllocated)
                    {
                        //MUKUL 20121002: 
                        //do nothing as the taxlot Dictionary(at group level) has already been resetted above for preallocated busted trades..
                    }
                    else
                    {
                        AllocationCheck(groups);
                    }
                    if (isBustOrder)
                    {
                        //MUKUL 20121002:
                        //setting the Taxlot State to deleted in case of Bust Trades..
                        //This check applies for both unallocated and preallocated groups as earlier unallocated busted groups were getting published with updated Taxlot State...
                        group.ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Deleted);
                    }

                    if (!isDirty && order.MsgType != FIXConstants.MSGOrder)
                    {
                        // Added a check if there is any taxlot with deleted state in group then need not update status to updated as in this case check side is violating so need to unallocate group.
                        if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && !isBustOrder && group.GetAllTaxlots().Select(x => x.TaxLotState != ApplicationConstants.TaxLotState.Deleted).ToList().Count == 0)
                        {
                            group.ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Updated);
                        }
                    }
                    if (isCached)
                    {
                        // Moved the GroupCache to CacheStore in Allocation.Core, so called this function through AllocationManager
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7506
                        AddGroup(group);
                        if (autoGroupingRules != null && autoGroupingRules.AutoGroup && isReal &&
                            (order.OriginatorType == 3 || (!string.IsNullOrEmpty(order.StagedOrderID) && order.MsgType.Equals(FIXConstants.MSGExecution)))
                            && IsAutoGroupEnabledForPref(order.Level1ID) && !order.IsMultiDayParentOrder)
                            GroupCache.GetInstance().UpdateAutoGroupsCache(group);
                    }
                    UpdateAttribList(groups);
                    if (isReal && order.MsgType == FIXConstants.MSGOrder && !string.IsNullOrEmpty(order.StagedOrderID))
                    {
                        AuditManager.Instance.AddGroupToAuditEntry(group, false, group.TaxLots.Count > 0 ? TradeAuditActionType.ActionType.REALLOCATE : TradeAuditActionType.ActionType.UNALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.GroupCreatedForTrade.ToString(), group.CompanyUserID);
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, string.Empty, "", TradeAuditActionType.AllocationAuditComments.TaxlotCreatedForTrade.ToString(), group.CompanyUserID);
                        AuditManager.Instance.SaveAuditList();
                    }
                }
                else
                {
                    group = CreateGroup(order, ref isDirty, false);
                    if (!isDirty)
                    {
                        CommissionCalculator.GetInstance.StartCalculation(group);
                        //CommissionAmt is the commission added on TT at order level
                        if (group.CommSource != CommisionSource.Auto)//Updating commission in any case (from TT or fills)
                        {
                            //As the commission amt is available on Prana basic message.If some commission is added on TT,We will copy it here to Group. 
                            group.Commission = group.CommissionAmt;
                        }
                        if (group.SoftCommSource != CommisionSource.Auto)
                        {
                            group.SoftCommission = group.SoftCommissionAmt;
                        }
                        group.DistributeCommisionInTaxLot(group.CommSource != CommisionSource.Auto, group.SoftCommSource != CommisionSource.Auto);

                    }
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                    AllocationLevelList allocationDetails = GetAllocationDetails(order.Level1ID, "", order.Level2ID, group, order.CumQty, out setNameFromFile, !isCached, false, order.ClOrderID);
                    if (allocationDetails != null && allocationDetails.Collection.Count > 0)
                        ValidateAllocationDetailsAndAllocate(group, ref allocationDetails, "", setNameFromFile);

                    AddGroup(group);
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
            return group;
        }
        /// <summary>
        /// Update Group And StateCache Note:- Update  merged groups in cache after the check side check
        /// </summary>
        /// <param name="DeletedGroupsToRemove"></param>
        private void UpdateGroupAndStateCache(List<AllocationGroup> DeletedGroupsToRemove)
        {
            try
            {
                if (DeletedGroupsToRemove != null && DeletedGroupsToRemove.Count > 0)
                {
                    foreach (AllocationGroup grp in DeletedGroupsToRemove)
                    {
                        AddGroup(grp);
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
        }
        /// <summary>
        /// Update State Cache: - Update state cache in case of Merged orders, maintain the cache before and after check side check (ShortClosing and long closing)
        /// </summary>
        /// <param name="removeGroup"></param>
        /// <param name="flag"></param>
        /// <param name="removeGrouplist"></param>
        private static void UpdateStateCache(AllocationGroup removeGroup, int flag, List<AllocationGroup> removeGrouplist)
        {
            try
            {
                GroupPositionType positionType = CheckSideHelper.GetPositionKey(removeGroup.OrderSideTagValue);
                if (removeGroup.UserID == -1 && removeGroup.OrderCount >= 2 && !positionType.Equals(GroupPositionType.LongOpening) && !positionType.Equals(GroupPositionType.ShortOpening))
                {
                    StateCacheStore.Instance.UpdateCache(new List<AllocationGroup> { removeGroup }, flag);
                    UserWiseStateCache.Instance.UpdateStateForUser(new List<AllocationGroup> { removeGroup }, flag);
                }
                if (removeGrouplist != null && !positionType.Equals(GroupPositionType.LongOpening) && !positionType.Equals(GroupPositionType.ShortOpening))
                {
                    StateCacheStore.Instance.UpdateCache(removeGrouplist, flag);
                    UserWiseStateCache.Instance.UpdateStateForUser(removeGrouplist, flag);
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
        }

        /// <summary>
        /// Get Un Saved Groups Data
        /// </summary>
        /// <param name="groupListNew"></param>
        /// <param name="groupList"></param>
        public void GetUnSavedGroupsData(ref List<AllocationGroup> groupListNew, ref List<AllocationGroup> groupList)
        {
            try
            {
                GroupCache.GetInstance().GetAllocationGroups(ref groupListNew, ref groupList);
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
        }






        /// <summary>
        /// Saves the un saved groups.
        /// </summary>
        public void SaveUnSavedGroups(List<AllocationGroup> groupListNew, List<AllocationGroup> groupList)
        {
            int rowsAffectedNew = 0;
            int rowsAffected = 0;
            try
            {
                lock (_locker)
                {
                    //List<AllocationGroup> groupListNew = new List<AllocationGroup>();
                    //List<AllocationGroup> groupList = new List<AllocationGroup>();

                    //[Rahul:20130222]This function will finally clear the unsaved groups dictionary.
                    // Moved the GroupCache to CacheStore in Allocation.Core, so called this function through AllocationManager
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7506
                    //GroupCache.GetInstance().GetAllocationGroups(ref groupListNew, ref groupList);

                    // Fist save those groups which are newly created.
                    if (groupListNew.Count > 0)
                    {
                        rowsAffectedNew = SaveGroupsForFills(groupListNew, string.Empty, true);
                        foreach (AllocationGroup group in groupListNew)
                        {
                            if (!OrderCacheManager.DictMultiDayGroupIdAndParentClOrderIdMapping.ContainsKey(group.Orders[0].ParentClOrderID))
                            {
                                OrderCacheManager.DictMultiDayGroupIdAndParentClOrderIdMapping.Add(group.Orders[0].ParentClOrderID, group.GroupID);
                            }
                        }

                    }
                    // Further updates on the existing groups are saved here.
                    if (groupList.Count > 0)
                    {
                        rowsAffected = SaveGroupsForFills(groupList, string.Empty, true);
                    }
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
        }

        /// <summary>
        /// Unwinding trade if Closed when fills arrives.
        /// </summary>
        /// <param name="group">Allocation Group</param>
        private void UnwindIfReallocated(ref AllocationGroup group)
        {
            try
            {
                Dictionary<string, DateTime> dictSymbolsForUnwinding = new Dictionary<string, DateTime>();
                if (group.ClosingStatus != ClosingStatus.Open && group.CumQty != 0)
                {
                    // these are the symbols which are in partially executed state and closed fully or partially
                    if (!(dictSymbolsForUnwinding.ContainsKey(group.Symbol)))
                    {
                        dictSymbolsForUnwinding.Add(group.Symbol, group.ProcessDate);
                    }
                }
                if (dictSymbolsForUnwinding.Count > 0)
                {
                    UnwindClosingBySymbolAndDate(dictSymbolsForUnwinding);
                    group.ClosingStatus = ClosingStatus.Open;
                }
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
        /// Unwinds the closing by symbol and date.
        /// </summary>
        /// <param name="dictSymbols">The dictionary symbols.</param>
        public void UnwindClosingBySymbolAndDate(Dictionary<string, DateTime> dictSymbols)
        {
            try
            {
                List<string> listIDs = ServiceProxyConnector.ClosingProxy.GetIDsToUnwindFromDB(dictSymbols);
                if (listIDs.Count > 0)
                {
                    string TaxlotClosingID = listIDs[0];
                    string TaxlotID = listIDs[1];
                    UnWindClosing(TaxlotClosingID, TaxlotID, string.Empty);
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
        }

        /// <summary>
        /// Uns the wind closing.
        /// </summary>
        /// <param name="TaxlotClosingID">The taxlot closing identifier.</param>
        /// <param name="taxlotIDList">The taxlot identifier list.</param>
        /// <param name="TaxlotClosingIDWithClosingDate">The taxlot closing identifier with closing date.</param>
        /// <returns></returns>
        public ClosingData UnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate)
        {
            ClosingData closedData = null;
            List<string> listGroupIDs = new List<string>();
            try
            {
                #region Update last calc revaluation date
                string[] arrTaxlotClosingIDWithClosingDate = TaxlotClosingIDWithClosingDate.Split(',');

                List<string> closingDates = new List<string>();
                foreach (string TaxLotClosingId in arrTaxlotClosingIDWithClosingDate)
                {
                    string[] TaxLotClosingIdWithDate = TaxLotClosingId.Split('_');

                    //update reval date for unwinded taxlots
                    DateTime closingDate = DateTime.MinValue;

                    if (TaxLotClosingIdWithDate.Length > 2 && DateTime.TryParse(TaxLotClosingIdWithDate[1], out closingDate) && !string.IsNullOrEmpty(TaxLotClosingIdWithDate[2]))
                    {
                        closingDates.Add(TaxLotClosingIdWithDate[1] + "_" + TaxLotClosingIdWithDate[2]);
                    }
                }
                ServiceProxyConnector.ActivityService.GenerateCashActivity(closingDates, CashTransactionType.Unwinding);
                #endregion
                //listGroupIDs contains IDs which have been created as a result of Expiration or exercise as we need to delete these groups 
                listGroupIDs = ServiceProxyConnector.ClosingProxy.UnWindClosingFromDatabase(TaxlotClosingID, false);

                if (listGroupIDs.Count > 0)
                {
                    DeleteGroups(listGroupIDs);
                }

                ServiceProxyConnector.ClosingProxy.UnwindPositions(TaxlotClosingID);

                closedData = ServiceProxyConnector.ClosingProxy.FetchUpdatedTaxlotsFromDB(taxlotIDList, listGroupIDs);
                closedData.PositionsToUnwind = TaxlotClosingID;
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
            return closedData;
        }

        /// <summary>
        /// Deletes the groups.
        /// </summary>
        /// <param name="groupIDs">The group i ds.</param>
        /// <returns></returns>
        public int DeleteGroups(List<string> groupIDs)
        {
            int rowsAffected = 0;
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                //Narendra Kumar Jangir, Sept 12 2013
                //Check count of groupIDs, if it is greater than zero than perform deletion of group(For closing these are expired/settled group)
                if (groupIDs != null && groupIDs.Count > 0)
                {
                    StringBuilder listGrpIds = new StringBuilder();
                    int i = 0;
                    foreach (string groupID in groupIDs)
                    {
                        i++;
                        listGrpIds.Append("'");

                        listGrpIds.Append(groupID);
                        listGrpIds.Append("'");

                        if (groupIDs.Count != i)
                        {
                            listGrpIds.Append(",");
                        }
                    }
                    if (listGrpIds.Length > 0)
                        groups = _m_PersistenceManager.GetGroups(listGrpIds.ToString());
                    foreach (AllocationGroup group in groups)
                    {

                        if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            foreach (TaxLot taxlot in group.TaxLots)
                            {
                                taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                            }
                        }
                        if (group.TaxLots.Count == 0)
                        {
                            List<TaxLot> taxlots = new List<TaxLot>();
                            TaxLot taxlot = CreateUnAllocatedTaxLot(group, group.GroupID);
                            taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                            taxlots.Add(taxlot);
                            group.ResetTaxlotDictionary(taxlots);
                            // CopySwapParameters(taxlots, group);                             
                        }
                        else
                            group.ResetTaxlotDictionary(group.TaxLots);
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                    }
                    UpdateState(groups);
                    //Following line SaveGroupsFromServer() moved inside the check of groupIDs,since it should be called if there is any groupid to delete
                    rowsAffected = SaveGroupsForFills(groups, string.Empty, true);
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
            return rowsAffected;
        }

        /// <summary>
        /// Saves the groups for fills.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="connString">The connection string.</param>
        /// <param name="fromServer">if set to <c>true</c> [from server].</param>
        /// <returns></returns>
        public int SaveGroupsForFills(List<AllocationGroup> groups, string connString, bool fromServer)
        {
            Dictionary<string, DateTime> dictSymbolsForUnwinding = new Dictionary<string, DateTime>();
            XmlSaveHandler xmlSaveMgr = null;
            int rowsAffected = 0;
            try
            {
                lock (_locker)
                {
                    xmlSaveMgr = new XmlSaveHandler();
                    foreach (AllocationGroup group in groups)
                    {
                        if (!fromServer) // update from client so we need to update the server GroupCache
                        {
                            AllocationGroup clonedGroup = (AllocationGroup)group.Clone();
                            // Moved the GroupCache to CacheStore in Allocation.Core, so called this function through AllocationManager
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7506
                            MarkGroupDirty(clonedGroup);
                            clonedGroup.SetDefaultPersistenceStatus();
                        }
                        if (connString == string.Empty)
                        {
                            ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(group);
                            // If the company name is not received from the securitymasterd then fill the description
                            if (String.IsNullOrEmpty(group.CompanyName))
                            {
                                group.CompanyName = group.Description;
                            }
                            if (ServiceProxyConnector.ClosingProxy.CheckGroupStatus(group) == PostTradeEnums.Status.None)
                            {
                                xmlSaveMgr.CreateXmls(group);
                            }
                            else
                            {
                                try
                                {
                                    if (group.PersistenceStatus.Equals(ApplicationConstants.PersistenceStatus.ReAllocated))
                                    {
                                        // these are the symbols which are in partially executed state and closed fully or partially
                                        if (!(dictSymbolsForUnwinding.ContainsKey(group.Symbol)))
                                        {
                                            dictSymbolsForUnwinding.Add(group.Symbol, group.ProcessDate);
                                        }
                                    }
                                    xmlSaveMgr.CreateXmls(group);
                                }
                                catch (Exception ex)
                                {
                                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                    if (rethrow)
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                        else
                        {
                            xmlSaveMgr.CreateXmls(group);
                        }
                    }

                    // need to unwind the closing of all these symbols for all these symbols to avoid corruption as their closing data has been removed from PM_taxlots by reallocate  
                    if (dictSymbolsForUnwinding.Count > 0)
                    {
                        //Removing as moved before allocation
                        //UnwindClosingBySymbolAndDate(dictSymbolsForUnwinding);
                    }

                    rowsAffected = AllocationGroupCache.Instance.SaveGroupsPostTrade(connString, xmlSaveMgr, groups);

                    if (groups[0].Orders.Count > 0)
                    {
                        string clOrderId = groups[0].Orders[0].ClOrderID;
                        PranaMessage pranaMessage = OrderCacheManager.GetCachedOrder(clOrderId);
                        if (pranaMessage != null && OrderCacheManager.HasMultiDayHistory(pranaMessage))
                        {
                            string parentClOrderId = OrderCacheManager.DictMultiDayClOrderIDMapping.ContainsKey(clOrderId) ? OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId] : string.Empty;
                            string orderStatus = string.Empty;
                            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                                orderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value.ToString();
                            if ((orderStatus.Equals(FIXConstants.ORDSTATUS_PartiallyFilled) || orderStatus.Equals(FIXConstants.ORDSTATUS_Filled))
                                && !string.IsNullOrEmpty(parentClOrderId))
                            {
                                OrderCacheManager.SaveMultiDayOrderAllocation(parentClOrderId, groups[0].GroupID);
                            }
                        }
                    }

                    if (groups != null && groups.Count > 0 && (groups[0].TransactionSource != TransactionSource.FIX || (groups[0].TransactionSource != TransactionSource.TradingTicket && groups[0].IsManualGroup == false)))
                        PublishingHelper.PublishImportAcknowledgment(true);

                    ClearCacheMasterFundBasedPositions();

                    try
                    {
                        // Check if Reprocessing is required or not
                        // only if it is internal system
                        if (connString == string.Empty)
                        {
                            bool isProcessingReq = ServiceProxyConnector.PositionManagementServices.CheckIfReprocessingRequired(GetTaxlotsFromGroups(groups));
                            if (isProcessingReq)
                                ServiceProxyConnector.PositionManagementServices.ReProcessSnapShot();
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
            try
            {
                // only if it is internal system
                if (connString == string.Empty)
                {
                    AllocationGroupCreated(groups);
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
            return rowsAffected;
        }

        /// <summary>
        /// Gets the taxlots from groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private List<TaxLot> GetTaxlotsFromGroups(List<AllocationGroup> groups)
        {
            List<TaxLot> taxlotsList = new List<TaxLot>();
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    taxlotsList.AddRange(group.GetAllTaxlots());
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
            return taxlotsList;
        }

        /// <summary>
        /// Publish valid allocation groups created.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private void AllocationGroupCreated(List<AllocationGroup> groups)
        {
            try
            {
                List<AllocationGroup> validGroups = new List<AllocationGroup>();
                foreach (AllocationGroup group in groups)
                {
                    if (group.Orders.Count > 0)
                    {
                        string parentClOrdId = group.Orders.OrderByDescending(x => x.ParentClOrderID).ToList()[0].ParentClOrderID;
                        string clOrdId = group.Orders.OrderByDescending(x => x.ParentClOrderID).ToList()[0].ClOrderID;
                        string groupId = OrderCacheManager.GetMultiDayOrderAllocationByClOrderid(parentClOrdId);
                        var msg = OrderCacheManager.GetCachedOrder(clOrdId);
                        if (groupId != null && (msg.MessageType == FIXConstants.MSGOrderCancelReplaceRequest || msg.MessageType == FIXConstants.MSGOrderCancelRequest))
                        {
                            Expression<Func<AllocationGroup, bool>> predicate = un => groupId.Equals(un.GroupID);
                            List<AllocationGroup> oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                            group.Allocations.Collection.Clear();
                            foreach (AllocationLevelClass oldGroup in oldAllocationGroups[0].Allocations.Collection)
                            {
                                group.Allocations.Collection.Add(oldGroup);
                            }
                        }
                    }
                    if (group.CumQty > 0)
                    {
                        validGroups.Add(group);
                    }
                    else if ((group.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated) && (group.CumQty == 0))
                    {
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                        validGroups.Add(group);
                    }
                    if (group.TransactionSource == TransactionSource.Closing)
                    {
                        if (group.TransactionType == "Exercise" && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.New)
                        {
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExercise.ToString(), group.CompanyUserID);
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, string.Empty, "", TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExercise.ToString(), group.CompanyUserID);
                            AuditManager.Instance.SaveAuditList();
                        }
                        else if (group.TransactionType == "Exercise" && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted)
                        {
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExercise.ToString(), group.CompanyUserID);
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, string.Empty, "", TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExercise.ToString(), group.CompanyUserID);
                            AuditManager.Instance.SaveAuditList();
                        }
                        else if (group.TransactionType == "Expire" && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.New)
                        {
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExpire.ToString(), group.CompanyUserID);
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, string.Empty, "", TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExpire.ToString(), group.CompanyUserID);
                            AuditManager.Instance.SaveAuditList();
                        }
                        else if (group.TransactionType == "Expire" && group.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted)
                        {
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExpire.ToString(), group.CompanyUserID);
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, string.Empty, "", TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExpire.ToString(), group.CompanyUserID);
                            AuditManager.Instance.SaveAuditList();
                        }
                    }
                }
                PublishingHelper.Publish(validGroups, -1, false, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Allocates Multi Day Order After ReAllocation
        /// </summary>
        /// <param name="group"></param>
        /// <param name="level1ID"></param>
        /// <param name="level2ID"></param>
        /// <param name="isPreview"></param>
        /// <param name="forceAllocation"></param>
        /// <param name="clOrderId"></param>
        /// <returns></returns>
        private AllocationDefault AllocateMultiDayOrderAfterReAllocation(AllocationGroup group, int level1ID, int level2ID, bool isPreview, bool forceAllocation, string clOrderId, List<AllocationGroup> oldAllocationGroups)
        {
            AllocationDefault allocationDefault = new AllocationDefault();
            try
            {

                SerializableDictionary<int, AccountValue> perc = new SerializableDictionary<int, AccountValue>();
                foreach (var allocGrp in oldAllocationGroups)
                {
                    decimal sumOfPercentage = 0;
                    int lastItemLevelnID = 0;

                    foreach (var item in allocGrp.Allocations.Collection)
                    {
                        if (!perc.ContainsKey(item.LevelnID))
                        {
                            lastItemLevelnID = item.LevelnID;
                            sumOfPercentage += (decimal)item.Percentage;

                            var accountValue = new AccountValue(item.LevelnID, (decimal)item.Percentage);
                            if (level2ID != int.MinValue)
                                accountValue.StrategyValueList.Add(new StrategyValue(level2ID, 100, 0));
                            perc.Add(item.LevelnID, accountValue);
                        }
                    }

                    if (sumOfPercentage < 100 && sumOfPercentage != 0)
                    {
                        decimal percentageLeftOver = 100 - sumOfPercentage;
                        if (perc.ContainsKey(lastItemLevelnID))
                        {
                            perc[lastItemLevelnID].Value += percentageLeftOver;
                        }
                    }
                }

                AllocationParameter parameter = new AllocationParameter(
                               new AllocationRule()
                               {
                                   BaseType = AllocationBaseType.CumQuantity,
                                   RuleType = MatchingRuleType.None,
                                   MatchClosingTransaction = MatchClosingTransactionType.None,
                                   PreferenceAccountId = -1,
                                   ProrataAccountList = new List<int>(),
                                   ProrataDaysBack = 0,
                               }, perc, -1, -1, true, isPreview);

                AllocationGroup grp = (AllocationGroup)group.Clone();
                var response = AllocateByParameter(new List<AllocationGroup> { grp }, parameter, forceAllocation);

                if (String.IsNullOrWhiteSpace(response.Response) && response.GroupList.Count > 0)
                {
                    group.AccruedInterest = response.GroupList[0].AccruedInterest;
                    if (response.GroupList[0].Allocations.Collection.Count > 0)
                    {
                        allocationDefault.DefaultAllocationLevelList = response.GroupList[0].Allocations;
                        //update allocation scheme id and name, PRANA-20901
                        group.AllocationSchemeID = grp.AllocationSchemeID;
                        group.AllocationSchemeName = grp.AllocationSchemeName;
                    }
                    else
                        return null;
                }
                else
                {
                    group.IsManuallyModified = true;
                    string error = "Error processing pre-allocation for account: " + CachedDataManager.GetInstance.GetAccountText(level1ID);
                    InformationReporter.GetInstance.Write(error);
                    Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    InformationReporter.GetInstance.Write(response.Response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationDefault;
        }

        /// <summary>
        /// Created by Mukul 2012-10-02
        /// This function is refactored from the Original Function CreateAndValidateAllocationDetailsFromLevelIDS
        /// Note that no change has been made in the function - only refactoring is done
        /// </summary>
        /// <param name="level1ID"></param>
        /// <param name="accountName"></param>
        /// <param name="level2ID"></param>
        /// <param name="group"></param>
        /// <param name="cumQty"></param>
        /// <param name="setNameFromFile"></param>
        /// <returns>The main objective of this function is to return the Allocation Details so that taxlot can be created</returns>
        private AllocationLevelList GetAllocationDetails(int level1ID, String accountName, int level2ID, AllocationGroup group, double cumQty, out bool setNameFromFile, bool isPreview, bool forceAllocation, string clOrderId = "")
        {
            setNameFromFile = false;
            AllocationDefault allocationDefault = new AllocationDefault();
            try
            {
                //Added condition to set default strategy id equals to 0, PRANA-12400
                if (level2ID == int.MinValue)
                    level2ID = 0;
                ///TODO: Need comments - why do we need to check for String.Empty, please clarify - Ashish Poddar 20121002
                // OLD COMMENTS: this is set automation  account name will already be mapped from automation code
                if (accountName != string.Empty)
                {
                    allocationDefault = new AllocationDefault();
                    allocationDefault.DefaultName = "";
                    AllocationLevelClass account = new AllocationLevelClass(string.Empty);
                    if (level1ID == 0 || level1ID == int.MinValue)
                    {
                        ///TODO: Need comments, what is the purpose of this??
                        ///Ashish Poddar 20121002
                        account.LevelnID = AllocationIDGenerator.GetTempAccountID();
                    }
                    else
                    {
                        account.LevelnID = level1ID;
                    }
                    account.Percentage = 100;
                    allocationDefault.DefaultAllocationLevelList = new AllocationLevelList();
                    allocationDefault.DefaultAllocationLevelList.Add(account);
                    setNameFromFile = true;
                    SetGroupDetailsInAllocation(group, allocationDefault.DefaultAllocationLevelList);
                }
                else
                {
                    // Handling for Multi Day trades
                    if (!string.IsNullOrEmpty(clOrderId) && OrderCacheManager.DictMultiDayClOrderIDMapping.ContainsKey(clOrderId) && !string.IsNullOrEmpty(OrderCacheManager.GetMultiDayOrderAllocationByClOrderid(OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId])) && group.CumQty > 0)
                    {
                        var groupId = OrderCacheManager.GetMultiDayOrderAllocationByClOrderid(OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId]);
                        Expression<Func<AllocationGroup, bool>> predicate = un => groupId.Equals(un.GroupID);
                        List<AllocationGroup> oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                        allocationDefault = AllocateMultiDayOrderAfterReAllocation(group, level1ID, level2ID, isPreview, forceAllocation, clOrderId, oldAllocationGroups);
                        if (allocationDefault == null)
                        {
                            return null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(clOrderId) && OrderCacheManager.DictMultiDayClOrderIDMapping.ContainsKey(clOrderId) && OrderCacheManager.DictMultiDayClOrderIdParentGroupIdMapping.ContainsKey(OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId]) && group.CumQty > 0)
                    {
                        var groupId = OrderCacheManager.DictMultiDayClOrderIdParentGroupIdMapping[OrderCacheManager.DictMultiDayClOrderIDMapping[clOrderId]];
                        Expression<Func<AllocationGroup, bool>> predicate = un => groupId.Equals(un.GroupID);
                        List<AllocationGroup> oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                        allocationDefault = AllocateMultiDayOrderAfterReAllocation(group, level1ID, level2ID, isPreview, forceAllocation, clOrderId, oldAllocationGroups);

                        if (allocationDefault == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        // this is for local database
                        ///Ashish 20121002
                        ///This section updated allocation rules - in the object. Based on Level1ID specified in FIX Rules

                        // New changes for allocation preference.
                        // First checking for preference defined in system. If not defined then it will try to find any account defined with the given Id
                        AllocationOperationPreference pref = GetPreferenceById(level1ID);
                        AllocationMasterFundPreference masterFundPref = GetMasterFundPreferenceById(level1ID);
                        string fixedPref = GetAllocationSchemeNameByID(level1ID);
                        if (pref != null || masterFundPref != null || !string.IsNullOrWhiteSpace(fixedPref))
                        {
                            AllocationResponse response = new AllocationResponse();
                            AllocationGroup grp = (AllocationGroup)group.Clone();
                            string preferenceName = pref != null ? pref.OperationPreferenceName : masterFundPref != null ? masterFundPref.MasterFundPreferenceName : fixedPref;

                            if (group.CumQty > 0)
                            {
                                response = AllocateByPreference(new List<AllocationGroup> { grp }, level1ID, -1, isPreview, forceAllocation);
                                AllocationGroup gr = new AllocationGroup(); ;
                                if (String.IsNullOrWhiteSpace(response.Response))
                                {
                                    gr = response.GroupList[0];
                                    allocationDefault.DefaultAllocationLevelList = gr.Allocations;
                                    group.AccruedInterest = gr.AccruedInterest;
                                    //update allocation scheme id and name, PRANA-20901
                                    group.AllocationSchemeID = grp.AllocationSchemeID;
                                    group.AllocationSchemeName = grp.AllocationSchemeName;
                                }
                                else
                                {
                                    string error = "Error processing pre-allocation for Preference: " + preferenceName;
                                    InformationReporter.GetInstance.Write(error);
                                    Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                    Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                    InformationReporter.GetInstance.Write(response.Response);
                                    return null;
                                }
                            }
                            else
                            {
                                //https://jira.nirvanasolutions.com:8443/browse/PRANA-23879 
                                SerializableDictionary<int, AccountValue> targetPercentage = new SerializableDictionary<int, AccountValue>();
                                AllocationLevelList list = new AllocationLevelList();
                                grp.CumQty = grp.Quantity;
                                AllocationResponse allocationResponse = AllocateByPreference(new List<AllocationGroup> { grp }, level1ID, -1, true, forceAllocation);
                                if (string.IsNullOrWhiteSpace(allocationResponse.Response) && allocationResponse.GroupList.Count > 0)
                                {
                                    //Change Allocation Scheme Name for Stage Order, PRANA-25449
                                    group.AllocationSchemeID = grp.AllocationSchemeID;
                                    group.AllocationSchemeName = grp.AllocationSchemeName;
                                    targetPercentage = AllocationManagerHelper.GetTargetPercentageFromGroupAllocationCollection(allocationResponse.GroupList[0].Allocations);
                                }

                                foreach (AccountValue val in targetPercentage.Values)
                                {
                                    AllocationLevelClass levelClass = new AllocationLevelClass(group.GroupID);
                                    levelClass.LevelnID = val.AccountId;
                                    levelClass.Name = CachedDataManager.GetInstance.GetAccountText(val.AccountId);
                                    levelClass.Percentage = (float)val.Value;

                                    foreach (StrategyValue sVal in val.StrategyValueList)
                                    {
                                        AllocationLevelClass sClass = new AllocationLevelClass(group.GroupID);
                                        sClass.LevelnID = sVal.StrategyId;
                                        sClass.Percentage = (float)sVal.Value;
                                        sClass.Name = CachedDataManager.GetInstance.GetStrategyText(sVal.StrategyId);
                                        levelClass.AddChilds(sClass);
                                    }
                                    list.Add(levelClass);
                                }
                                allocationDefault.DefaultAllocationLevelList = list;
                                SetGroupDetailsInAllocation(group, allocationDefault.DefaultAllocationLevelList);
                            }
                            allocationDefault.DefaultID = level1ID;
                            allocationDefault.DefaultName = preferenceName;
                            allocationDefault.IsDefaultAllocationRule = true;
                        }
                        else // If no preference is defined with the given id then this should be some Id of the account
                        {
                            if (level1ID == int.MinValue || !String.IsNullOrEmpty(CommonDataCache.CachedDataManager.GetInstance.GetAccount(level1ID)))
                            {
                                if (group.CumQty > 0)
                                {
                                    AllocationResponse response = new AllocationResponse();
                                    SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
                                    percentage.Add(level1ID, new AccountValue(level1ID, 100));
                                    // Kuldeep: Added Strategy Quantity 0 as we do use only percentage in this case.
                                    if (level2ID != int.MinValue)
                                        percentage[level1ID].StrategyValueList.Add(new StrategyValue(level2ID, 100, 0));
                                    AllocationParameter parameter = null;
                                    //If account id is min value then send it for allocation with pref id int.minValue
                                    if (level1ID == int.MinValue)
                                        parameter = new AllocationParameter(
                                        new AllocationRule()
                                        {
                                            BaseType = AllocationBaseType.CumQuantity,
                                            RuleType = MatchingRuleType.None,
                                            MatchClosingTransaction = MatchClosingTransactionType.None,
                                            PreferenceAccountId = -1,
                                            ProrataAccountList = new List<int>(),
                                            ProrataDaysBack = 0,
                                        }, percentage, int.MinValue, -1, true, isPreview);
                                    else
                                        parameter = new AllocationParameter(
                                       new AllocationRule()
                                       {
                                           BaseType = AllocationBaseType.CumQuantity,
                                           RuleType = MatchingRuleType.None,
                                           MatchClosingTransaction = MatchClosingTransactionType.None,
                                           PreferenceAccountId = -1,
                                           ProrataAccountList = new List<int>(),
                                           ProrataDaysBack = 0,
                                       }, percentage, -1, -1, true, isPreview);

                                    AllocationGroup grp = (AllocationGroup)group.Clone();
                                    response = AllocateByParameter(new List<AllocationGroup> { grp }, parameter, forceAllocation);

                                    AllocationGroup gr = new AllocationGroup(); ;
                                    if (String.IsNullOrWhiteSpace(response.Response) && response.GroupList.Count > 0)
                                    {
                                        gr = response.GroupList[0];
                                        group.AccruedInterest = gr.AccruedInterest;
                                        if (gr.Allocations.Collection.Count > 0)
                                        {
                                            allocationDefault.DefaultAllocationLevelList = gr.Allocations;
                                            //update allocation scheme id and name, PRANA-20901
                                            group.AllocationSchemeID = grp.AllocationSchemeID;
                                            group.AllocationSchemeName = grp.AllocationSchemeName;
                                        }
                                        else
                                            return null;

                                    }
                                    else
                                    {
                                        group.IsManuallyModified = true;
                                        string error = "Error processing pre-allocation for account: " + CachedDataManager.GetInstance.GetAccountText(level1ID);
                                        InformationReporter.GetInstance.Write(error);
                                        Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                        Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                        InformationReporter.GetInstance.Write(response.Response);
                                        return null;
                                    }
                                }
                                else
                                {
                                    if (level1ID == int.MinValue)
                                        return null;
                                    allocationDefault = CachedDataManager.GetInstance.GetAllocationDefaultForAllocation(level1ID);
                                    SetGroupDetailsInAllocation(group, allocationDefault.DefaultAllocationLevelList);
                                    //Change Allocation Scheme Name for Stage Order allocated using single account, PRANA-25449
                                    group.AllocationSchemeID = level1ID;
                                    group.AllocationSchemeName = "Manual";
                                }
                            }
                            else
                            {
                                string error = "Account or prefrence has been deleted: " + level1ID;
                                //InformationReporter.GetInstance.Write(error);      // commenting so that multiple messages are not shown on server.
                                Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                return null;
                            }
                        }
                    }
                }
                ///Comments by Ashish: 20121002
                /// allocationDefault.DefaultName represents the Allocation Default used, if any
                /// If allocation is already done using Default ALlocation Rule from Preferences, no Strategy Allocation should be done. 
                /// If Default Allocation is not Done and Only Account Allocation is done, then this section will proceed. 
                /// If Level2ID is present in FIX rules then strategy allocation will be done
                if (allocationDefault.DefaultName == string.Empty && level2ID != int.MinValue) // strategy allocation
                {
                    AllocationLevelClass strategy = new AllocationLevelClass(group.GroupID);
                    strategy.LevelnID = level2ID;
                    strategy.Percentage = 100;
                    strategy.AllocatedQty = cumQty;
                    AllocationLevelClass account = allocationDefault.DefaultAllocationLevelList.GetAllocationLevel(level1ID);
                    if (account != null)
                    {
                        account.AddChilds(strategy);
                    }
                }
                //the updation of allocation details is not required when allocation is done by allocation manager, called it specifically in other cases, PRANA-11089 
                //SetGroupDetailsInAllocation(group, allocationDefault.DefaultAllocationLevelList);
                //AllocationLevelList DefaultAllocationLevelList = allocationDefault.DefaultAllocationLevelList;
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
            return allocationDefault.DefaultAllocationLevelList;
        }

        /// <summary>
        /// Sets the group details in allocation.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="allocationLevelList">The allocation level list.</param>
        private void SetGroupDetailsInAllocation(AllocationGroup group, AllocationLevelList allocationLevelList)
        {
            bool shouldAllowFractional = PostTradeHelper.ISFractionalAllocationAllowed(group.AssetID);
            foreach (AllocationLevelClass account in allocationLevelList.Collection)
            {
                account.GroupID = group.GroupID;
                if (shouldAllowFractional)
                {
                    account.AllocatedQty = (account.Percentage * group.CumQty) / 100;
                }
                else
                {
                    account.AllocatedQty = Convert.ToInt64((account.Percentage * group.CumQty) / 100);
                }
                if (account.Childs != null)
                {
                    foreach (AllocationLevelClass strategy in account.Childs.Collection)
                    {
                        strategy.GroupID = group.GroupID;
                        if (shouldAllowFractional)
                        {
                            strategy.AllocatedQty = (strategy.Percentage * account.AllocatedQty) / 100;
                        }
                        else
                        {
                            strategy.AllocatedQty = Convert.ToInt64((strategy.Percentage * account.AllocatedQty) / 100);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the allocation details and allocate.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="DefaultAllocationLevelList">The default allocation level list.</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="setNameFromFile">if set to <c>true</c> [set name from file].</param>
        private void ValidateAllocationDetailsAndAllocate(AllocationGroup group, ref AllocationLevelList DefaultAllocationLevelList, String accountName, bool setNameFromFile)
        {
            try
            {
                // The validation is already done while getting DefaultAllocationLevelList, no need to validate details here, PRANA-11089 
                // string result = ValidateAllocationAccounts(group, ref DefaultAllocationLevelList, CachedDataManager.GetInstance.GetlastPreferencedAccountID(), false, true, false);

                group.Allocate(DefaultAllocationLevelList);

                if (setNameFromFile) // accountname and accountid received from file 
                {
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        taxlot.Level1Name = accountName;
                    }
                }
                else  // setging from local databse
                {
                    group.SetNameDetailsInGroup();
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
        }

        /// <summary>
        /// If check side fails, unallocate the group and update state in group list
        /// </summary>
        /// <param name="group">The allocation group</param>
        /// <param name="order">The order from which group is created/updated</param>
        /// <param name="groupList">List of Allocation groups</param>
        private void UnAllocateGroupOnCheckSideFail(ref AllocationGroup group, ref Order order, ref List<AllocationGroup> groupList)
        {
            try
            {
                group.IsPreAllocated = false;
                order.Level1ID = int.MinValue;
                //Unallocating group if check side fails.
                if (groupList != null && groupList.Count > 0)
                {
                    //Setting status of taxlots to updated as after fills status should be updated and groupList[0] contains old state so updating.
                    groupList[0].ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Updated);
                    //If old group contains taxlots then updating in new group from old.
                    if (groupList[0].GetAllTaxlots() != null && groupList[0].GetAllTaxlots().Count > 0)
                        group.ResetTheResetDictionary(groupList[0].GetAllTaxlots()[0]);
                    //Unallocating group to generate deleted taxlot.
                    group.UnallocateGroup();
                    group.IsManuallyModified = true;
                }
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
        /// Checks the allocation.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private void AllocationCheck(List<AllocationGroup> groups)
        {
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    group.AllocationCheck();
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
        /// Retrieves a list of allocation groups based on the provided date range and allocation filter.
        /// - Calls the data manager to fetch grouped order details.
        /// </summary>
        /// <param name="toDate">The end date for filtering grouped orders.</param>
        /// <param name="fromDate">The start date for filtering grouped orders.</param>
        /// <param name="filterList">The allocation prefetch filter containing criteria such as group IDs or account IDs.</param>
        /// <returns>A list of <see cref="AllocationGroup"/> objects matching the filter and date range.</returns>
        public List<AllocationGroup> GetGroupedOrderDetails(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, bool fetchAllValues)
        {
            string prefName = string.Empty;
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
               groups = AllocationGroupDataManager.GetGroups(toDate, fromDate, filterList, fetchAllValues).Values.ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groups;
        }

        #endregion

        #region Allocation By Scheme Members

        /// <summary>
        /// Returns Allocation Scheme Name from cache by passing its ID
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <returns></returns>
        public string GetAllocationSchemeNameByID(int allocationSchemeID)
        {
            string prefName = string.Empty;
            try
            {
                prefName = PreferenceManager.GetInstance.GetAllocationSchemeNameByID(allocationSchemeID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prefName;
        }

        /// <summary>
        /// Gets the currency list for allocation scheme.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrencyListForAllocationScheme()
        {
            List<string> currencyList = new List<string>();
            try
            {
                currencyList = PreferenceManager.GetInstance.GetCurrencyListForAllocationScheme();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return currencyList;
        }

        /// <summary>
        /// update Master account scheme based positions for each account
        /// Created by: omshiv, 20 Jan 2014
        /// </summary>
        /// <param name="group"></param>
        public void UpdateAccountWisePostionInCache(AllocationGroup group)
        {
            try
            {
                PreferenceManager.GetInstance.UpdateAccountWisePostionInCache(group);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets all allocation scheme names.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllAllocationSchemeNames()
        {
            Dictionary<int, string> allocationScheme = null;
            try
            {
                allocationScheme = PreferenceManager.GetInstance.GetAllAllocationSchemeNames();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation schemes by source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllocationSchemesBySource(FixedPreferenceCreationSource source)
        {
            Dictionary<int, string> allocationScheme = null;
            try
            {
                allocationScheme = PreferenceManager.GetInstance.GetAllocationSchemesBySource(source);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Validates the allocation accounts by allocation scheme.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="newaccounts">The newaccounts.</param>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="isSwap">if set to <c>true</c> [is swap].</param>
        /// <param name="preferencedAccountID">The preferenced account identifier.</param>
        /// <param name="isMasterFundRatioAllocation">if set to <c>true</c> [is master fund ratio allocation].</param>
        /// <returns></returns>
        private string ValidateAllocationAccountsByAllocationScheme(AllocationGroup allocationGroup, ref AllocationLevelList newaccounts, string allocationSchemeName, ref bool isSwap, int preferencedAccountID, bool isMasterFundRatioAllocation)
        {
            try
            {
                //get allocation scheme by Name from DB first time
                PreferenceManager.GetInstance.GetAllocationSchemeSymbolWise(allocationSchemeName);
                Dictionary<string, List<DataRow>> symbolwiseDict = PreferenceManager.GetInstance.GetSymbolWiseDictionary(allocationSchemeName);
                if (symbolwiseDict != null)
                {
                    string key = AllocationManagerHelper.GetAllocationSchemeKeyForGroup(allocationGroup, PreferenceManager.GetInstance.GetAllocationSchemeKey());
                    if (symbolwiseDict.ContainsKey(key))
                    {
                        //here we checking taxlot in grp is closed/exercised/closed, if so then return to client. 
                        string groupStatus = GetGroupStatus(allocationGroup);
                        if (!string.IsNullOrEmpty(groupStatus))
                        {
                            return groupStatus;
                        }

                        // update sec master related information like round lot
                        ServiceProxyConnector.SecmasterProxy.SetSecuritymasterDetails(allocationGroup);

                        decimal roundLot = allocationGroup.RoundLot;
                        if (roundLot > 0)
                        {
                            AllocationLevelList list = new AllocationLevelList();
                            decimal allowedqty = Convert.ToDecimal(allocationGroup.CumQty);
                            decimal qtyAfterRoundLot = allowedqty / roundLot;

                            //local variables
                            decimal accountsQty = 0;
                            double accountsPercentage = 0;
                            //double remainingQty = 0.0;
                            // integer part of quantity after round lot
                            decimal integralQty = Math.Truncate(qtyAfterRoundLot);
                            // decimal part if quantity is fractional
                            decimal residualQty = (qtyAfterRoundLot - integralQty) * roundLot;
                            //symbolwise allocation scheme
                            List<DataRow> lstRows = symbolwiseDict[key];

                            //modified by - omshiv, 15 Jan 2014, if isMasterFundRatioAllocation scheme is enabled then get current postion from cache
                            if (isMasterFundRatioAllocation)
                            {
                                string errMessage = string.Empty;
                                errMessage = PreferenceManager.GetInstance.GetPositionsFromSymbolAndNamewiseAccountAllocationScheme(allocationSchemeName, key, ref lstRows);
                                if (!string.IsNullOrWhiteSpace(errMessage))
                                    return errMessage;
                            }
                            // isFulllyAllocatedbyMasterRatio variable is used in the Master account ratio allocation scheme. If trade is allocated fully to one account then there is no need to add in the other account, 
                            // just update percentage in other accounts
                            bool isFulllyAllocatedbyMasterRatio = false;
                            try
                            {
                                // Put a check to sort data in such a way so that if 
                                //      1. it is masterfundratioallocation
                                //      2. count of rows in list is more than 0
                                //      3. If it is short transaction
                                //      4. and if it contains column "Quantity"
                                // Then sort the rows by Quantity
                                if (isMasterFundRatioAllocation && lstRows.Count > 0
                                        && lstRows[0].Table.Columns.Contains("Quantity")
                                        && lstRows[0].Table.Columns.Contains("TotalQty")
                                        && lstRows[0].Table.Columns.Contains("TargetAllocationPct"))
                                {
                                    //TotalQty
                                    //Quantity
                                    //TargetAllocationPct

                                    // Added this to check if prorata values are correct, if Quantity, TotalQty or TargetAllocationPct is blank then it gives message that prorata is not calculated properly
                                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7713
                                    double val;
                                    bool isProrataCorrect = true;

                                    lstRows.ForEach(x =>
                                    {
                                        if (!Double.TryParse(x["TargetAllocationPct"].ToString(), out val) || !Double.TryParse(x["TotalQty"].ToString(), out val) || !Double.TryParse(x["Quantity"].ToString(), out val))
                                            isProrataCorrect = false;
                                    });

                                    if (!isProrataCorrect)
                                    {
                                        return allocationGroup.Symbol + ": Prorata is not generated properly, please generate Prorata again.";
                                    }

                                    if (Calculations.GetSideMultilpier(allocationGroup.OrderSideTagValue) < 0)
                                        lstRows = lstRows.OrderByDescending(x => (Convert.ToDouble(x["Quantity"]) / Convert.ToDouble(x["TotalQty"]) * 100) - Convert.ToDouble(x["TargetAllocationPct"])).ToList();
                                    else
                                        lstRows = lstRows.OrderBy(x => (Convert.ToDouble(x["Quantity"]) / Convert.ToDouble(x["TotalQty"]) * 100) - Convert.ToDouble(x["TargetAllocationPct"])).ToList();
                                }
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                    throw;
                            }
                            foreach (DataRow row in lstRows)
                            {
                                //modified by - omshiv, 15 Jan 2014, if isMasterFundRatioAllocation scheme is enabled then Allocat accounts wrt target %
                                if (isMasterFundRatioAllocation)
                                {
                                    //check is TargetAllocationPct exists in row
                                    if (row.Table.Columns.Contains("TargetAllocationPct"))
                                    {
                                        GetAccountListforMFRatioScheme(allocationGroup, list, integralQty, row, ref isFulllyAllocatedbyMasterRatio);
                                    }
                                    else
                                    {
                                        return allocationGroup.Symbol + ": For Master Account ratio allocation, Target ratio not available. Please review settings.";
                                    }
                                }
                                else
                                {
                                    List<string> currencyList = PreferenceManager.GetInstance.GetCurrencyListForAllocationScheme();
                                    if (currencyList != null && currencyList.Count > 0 && !currencyList.Contains(row["Currency"].ToString()) && row["TradeType"].ToString().ToLower().Equals("swap"))
                                    {
                                        if (PreferenceManager.GetInstance.PBEBMapping(row["PB"].ToString(), allocationGroup.CounterPartyID))
                                        {
                                            isSwap = GetAccountList(allocationGroup, isSwap, list, integralQty, row);
                                        }
                                    }
                                    else
                                    {
                                        isSwap = GetAccountList(allocationGroup, isSwap, list, integralQty, row);
                                    }
                                }
                            }
                            newaccounts = DeepCopyHelper.Clone(list);
                            foreach (AllocationLevelClass account in newaccounts.Collection)
                            {
                                accountsQty += Convert.ToDecimal(account.AllocatedQty);
                                accountsPercentage += account.Percentage;
                            }

                            accountsPercentage = Convert.ToSingle(Math.Round(accountsPercentage, 0));
                            //modified by omshiv, mesaage if symbol belong to different PB's account in pair
                            if (isMasterFundRatioAllocation && !(100M).EqualsPrecise(accountsPercentage))
                            {
                                return allocationGroup.Symbol + ": Sum of Accounts percentage should be 100. For Master Account allocation, symbol should be in same PB's Accounts.";
                            }
                            else if (!(100M).EqualsPrecise(accountsPercentage))
                            {
                                return allocationGroup.Symbol + ": Sum of Accounts percentage should be 100.";
                            }

                            double remainingQty = 0.0;
                            if (qtyAfterRoundLot > 0 && allocationGroup.CumQty > 0)
                            {
                                remainingQty = CalculateAccountAllocations(allocationGroup.CumQty, accountsQty, integralQty, ref list, residualQty, qtyAfterRoundLot, roundLot, preferencedAccountID, true);
                            }
                            //RemoveAccountsWithZeroQty(ref newaccounts);
                            foreach (AllocationLevelClass account in newaccounts.Collection)
                            {
                                string strategyResult = CheckStrategyQtyPerInAccounts(account, roundLot);
                                if (strategyResult != string.Empty)
                                {
                                    return strategyResult;
                                }
                            }
                            if (remainingQty != 0)
                            {
                                return allocationGroup.Symbol + ": Can not allocate more than cum qty";
                            }
                        }
                        else
                        {
                            return allocationGroup.Symbol + ": Symbol round lot : " + roundLot;
                        }
                    }
                    else
                    {
                        return allocationGroup.Symbol + ": Symbol and Side do not exist in the selected allocation scheme";
                    }
                }
                else
                {
                    return "Allocation Scheme does not exists";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the group status.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns></returns>
        private string GetGroupStatus(AllocationGroup allocationGroup)
        {
            try
            {
                if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && allocationGroup.AllocationDate != DateTimeConstants.MinValue)
                {
                    return "This Group is already Allocated!";
                }

                PostTradeEnums.Status GroupStatus = ServiceProxyConnector.ClosingProxy.CheckGroupStatus(allocationGroup);
                if (GroupStatus.Equals(PostTradeEnums.Status.Closed))
                {
                    return "Group is Partially or Fully Closed. Can't be Allocated Again";
                }
                if (GroupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                {
                    return "First undo the applied corporate action to make any changes. ";
                }
                if (GroupStatus.Equals(PostTradeEnums.Status.Exercise) || GroupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually))
                {
                    return "Group is generated by Exercise";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the account listfor mf ratio scheme.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="list">The list.</param>
        /// <param name="integralQty">The integral qty.</param>
        /// <param name="row">The row.</param>
        /// <param name="isFulllyAllocatedbyMasterRatio">if set to <c>true</c> [is fullly allocatedby master ratio].</param>
        private static void GetAccountListforMFRatioScheme(AllocationGroup allocationGroup, AllocationLevelList list, decimal integralQty, DataRow row, ref bool isFulllyAllocatedbyMasterRatio)
        {
            try
            {
                if (Convert.ToDouble(row["TargetAllocationPct"].ToString()) > 0)
                {
                    AllocationLevelClass account = new AllocationLevelClass(allocationGroup.GroupID);
                    account.LevelnID = Convert.ToInt32(row["FundID"].ToString());
                    account.Name = CachedDataManager.GetInstance.GetAccountText(account.LevelnID);

                    //calculating required shares to allocate in account for maintaain the target ration
                    //and then set new values.
                    // Target Percentage Master Account wise
                    double targetPct = Convert.ToDouble(row["TargetAllocationPct"].ToString());
                    // new trade quantity to allocate
                    double qtyToAllocate = Convert.ToDouble(integralQty);
                    // total open positions Symbol wise i.e. picked from database, sum of all accounts for same symbol
                    double currentPositions = Convert.ToDouble(row["TotalQty"].ToString());
                    //get side multiplier for new trade that comes to allocate
                    int sideMultiplier_NewTrade = Calculations.GetSideMultilpier(allocationGroup.OrderSideTagValue);
                    // Account wise open quantity that we pick from database
                    double currentQtyInAccount = Convert.ToDouble(row["Quantity"].ToString());

                    //get side multiplier on the basis of open positions. We pick -ve qty in case of short positions from database.
                    int sideMultiplier_OpenPositions = 0;
                    if (currentPositions < 0)
                    {
                        sideMultiplier_OpenPositions = -1;
                    }
                    else
                    {
                        sideMultiplier_OpenPositions = 1;
                    }

                    // total quantity is sum of open quantity and new trade quantity
                    double toatalQty = currentPositions + (qtyToAllocate * sideMultiplier_NewTrade);

                    // if open positions and new quantity of a symbol are equal and sides are opposite then there is no need to allocate on the basis of
                    // Master Account Allocation ratio. Example: Currenct allocation: MSFT 100 shares, F1: 0 and F2: 100, target 70 and 30, then no use to allocate 
                    // because new allocation will generate box positions. Just allocate as open positions and return.
                    if (Math.Abs(currentPositions).Equals(Math.Abs(qtyToAllocate)) && sideMultiplier_NewTrade != sideMultiplier_OpenPositions)
                    {
                        account.AllocatedQty = Math.Abs(Convert.ToDouble(row["Quantity"].ToString()));
                        account.Percentage = float.Parse(row["Percentage"].ToString());
                        list.Add(account);

                        row["Percentage"] = account.Percentage;
                        row["Quantity"] = Convert.ToInt32(row["Quantity"].ToString()) + account.AllocatedQty * sideMultiplier_NewTrade;
                        row["TotalQty"] = toatalQty;

                        return;
                    }

                    // Account wise new trade's allocation to target shares 
                    double requiredShares = ((targetPct * toatalQty) / 100) - (currentQtyInAccount);

                    // if Open postions are long and new trade to be allocated is sell side
                    // then make the requiredShares to absolute value
                    // if open positions are short and new trade is also short side then change requiredShares as given below.
                    if (sideMultiplier_NewTrade.Equals(-1) && sideMultiplier_OpenPositions.Equals(1))
                    {
                        requiredShares = Math.Abs(requiredShares);
                    }
                    else if (sideMultiplier_NewTrade.Equals(-1) && sideMultiplier_OpenPositions.Equals(-1))
                    {
                        requiredShares = (-1) * requiredShares;
                    }

                    if (requiredShares > 0 && !isFulllyAllocatedbyMasterRatio)
                    {
                        double actualSharestoAllocate = 0;
                        if (requiredShares > qtyToAllocate)
                        {
                            actualSharestoAllocate = qtyToAllocate;
                            isFulllyAllocatedbyMasterRatio = true;
                        }
                        else
                        {
                            actualSharestoAllocate = Math.Round(requiredShares);
                        }
                        double noSharesAfterAllocation = currentQtyInAccount + (actualSharestoAllocate * sideMultiplier_NewTrade);

                        account.AllocatedQty = actualSharestoAllocate;
                        account.Percentage = (float)(noSharesAfterAllocation * 100 / toatalQty);
                    }
                    else
                    {
                        account.AllocatedQty = 0;
                        account.Percentage = (float)(currentQtyInAccount * 100 / toatalQty);
                    }

                    row["Percentage"] = account.Percentage;
                    row["Quantity"] = Convert.ToInt32(row["Quantity"].ToString()) + (account.AllocatedQty * sideMultiplier_NewTrade);
                    row["TotalQty"] = toatalQty;

                    list.Add(account);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the account list.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="isSwap">if set to <c>true</c> [is swap].</param>
        /// <param name="list">The list.</param>
        /// <param name="integralQty">The integral qty.</param>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        private static bool GetAccountList(AllocationGroup allocationGroup, bool isSwap, AllocationLevelList list, decimal integralQty, DataRow row)
        {
            try
            {
                if (Convert.ToDouble(row["Percentage"].ToString()) > 0)
                {
                    AllocationLevelClass account = new AllocationLevelClass(allocationGroup.GroupID);
                    account.AllocatedQty = Convert.ToDouble((Convert.ToDecimal(row["Percentage"].ToString()) * integralQty) / Convert.ToDecimal(100));
                    account.AllocatedQty = Math.Round(account.AllocatedQty, 0);
                    account.LevelnID = Convert.ToInt32(row["FundID"].ToString());
                    account.Name = CachedDataManager.GetInstance.GetAccountText(account.LevelnID);
                    account.Percentage = float.Parse(row["Percentage"].ToString());

                    list.Add(account);
                    if (row["TradeType"].ToString().ToLower().Equals("swap"))
                        isSwap = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return isSwap;
        }

        /// <summary>
        /// Calculates the account allocations.
        /// </summary>
        /// <param name="allowedQty">The allowed qty.</param>
        /// <param name="accountsQty">The accounts qty.</param>
        /// <param name="integralQty">The integral qty.</param>
        /// <param name="newaccounts">The newaccounts.</param>
        /// <param name="residualQty">The residual qty.</param>
        /// <param name="qtyAfterRoundLot">The qty after round lot.</param>
        /// <param name="roundLot">The round lot.</param>
        /// <param name="preferencedAccountID">The preferenced account identifier.</param>
        /// <param name="ApplyRoundRobin">if set to <c>true</c> [apply round robin].</param>
        /// <returns></returns>
        private double CalculateAccountAllocations(double allowedQty, decimal accountsQty, decimal integralQty, ref AllocationLevelList newaccounts, decimal residualQty, decimal qtyAfterRoundLot, decimal roundLot, int preferencedAccountID, bool ApplyRoundRobin)
        {
            double remainingQty = 0.0;
            decimal remainingQtyDecimal = 0;
            bool isRemoveExtraShare = false;
            if (preferencedAccountID == -1 || preferencedAccountID == int.MinValue)
            {
                //do nothing and proceed with the value of Apply RoundRobin parameter coming from calling function as in some cases it is specifically coming 
                // as false.. This is specifically done as this function is called mutliple times when pari-passu allocation is on and since round robin switches to
                //the next account with each calling (in case an ambiguity of extra share is there) we need to make sure that round robin is specifically triggered only with the final allocation..
            }
            else
            {
                ApplyRoundRobin = false;
            }
            AllocationLevelClass accountToAdj = null;
            try
            {
                if (accountsQty != integralQty)
                {
                    remainingQty = Convert.ToDouble(integralQty - accountsQty);
                    remainingQtyDecimal = decimal.Parse(remainingQty.ToString());
                    if (remainingQtyDecimal > 0)
                    {
                        accountToAdj = AllocateTransaction(ref newaccounts, double.Parse(integralQty.ToString()), ref remainingQtyDecimal, false, ApplyRoundRobin, preferencedAccountID);
                        if (accountToAdj.Childs != null)
                        {
                            foreach (AllocationLevelClass strategy in accountToAdj.Childs.Collection)
                            {
                                strategy.AllocatedQty = (strategy.Percentage * accountToAdj.AllocatedQty) / 100.0;
                            }
                        }
                        if (ApplyRoundRobin)
                        {
                            CachedDataManager.GetInstance.SetPreferencedAccountID(accountToAdj.LevelnID);
                        }
                    }
                    else
                    {
                        //apply round robin while removing extra share as no specific account should be targeted for removing that extra share..
                        int index = GetIndexToStartWith(ref newaccounts, ApplyRoundRobin, preferencedAccountID, true);
                        RemoveExtraShare(ref newaccounts, index, ref remainingQtyDecimal, ApplyRoundRobin);
                        if (remainingQtyDecimal != 0)
                        {
                            RemoveExtraShare(ref newaccounts, 0, ref remainingQtyDecimal, ApplyRoundRobin);
                        }
                        isRemoveExtraShare = true;
                    }
                }
                foreach (AllocationLevelClass account in newaccounts.Collection)
                {
                    account.Percentage = (float)((Convert.ToDecimal(account.AllocatedQty) * 100 / qtyAfterRoundLot));
                    account.AllocatedQty = account.AllocatedQty * roundLot.ToDoublePrecise();
                }

                if (residualQty != 0)
                {
                    accountToAdj = AllocateTransaction(ref newaccounts, allowedQty, ref residualQty, true, ApplyRoundRobin, preferencedAccountID);

                    //If all accounts integral qty becomes zero after rounding off then we have to allocate the 
                    //residualQty to one of the account in order to validate.
                    if (residualQty != 0)
                    {
                        accountToAdj = AllocateTransaction(ref newaccounts, allowedQty, ref residualQty, false, ApplyRoundRobin, preferencedAccountID);
                    }
                    if (!isRemoveExtraShare && ApplyRoundRobin)
                    {
                        CachedDataManager.GetInstance.SetPreferencedAccountID(accountToAdj.LevelnID);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            remainingQty = double.Parse(remainingQtyDecimal.ToString());
            return remainingQty;
        }

        /// <summary>
        /// It will remove the accounts in group with zero quantity. Eg: When trade comes from Trading ticket, fitst
        /// it comes with zero cum quantity.
        /// </summary>
        /// <param name="accountList"></param>
        private void RemoveAccountsWithZeroQty(ref AllocationLevelList accountList)
        {
            try
            {
                // remove accounts having zero qty
                List<AllocationLevelClass> accountsToRemove = new List<AllocationLevelClass>();
                foreach (AllocationLevelClass account in accountList.Collection)
                {
                    if (account.AllocatedQty == 0)
                    {
                        accountsToRemove.Add(account);
                    }
                }
                foreach (AllocationLevelClass account in accountsToRemove)
                {
                    accountList.Collection.Remove(account);
                }
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
        /// Check the Quantity allocated to strategies from accounts.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="roundLot"></param>
        /// <returns></returns>
        private string CheckStrategyQtyPerInAccounts(AllocationLevelClass account, decimal roundLot)
        {
            try
            {
                decimal strategyQty = 0;
                if (account.Childs != null)
                {
                    decimal strategyPercentage = 0;
                    decimal allowedQty = Convert.ToDecimal(account.AllocatedQty);
                    decimal qtyAfterRoundLot = allowedQty / roundLot;

                    foreach (AllocationLevelClass strategy in account.Childs.Collection)
                    {
                        strategy.AllocatedQty = Math.Round(Convert.ToDouble(Convert.ToDecimal(strategy.Percentage) * qtyAfterRoundLot) / 100, 0);
                        strategyQty += Convert.ToDecimal(strategy.AllocatedQty);
                        strategyPercentage += Convert.ToDecimal(strategy.Percentage);
                    }
                    if (Math.Round(strategyPercentage) != 100)
                    {
                        return "Sum of Strategy percentage should be 100!";
                    }

                    decimal integralQty = Math.Truncate(qtyAfterRoundLot);
                    decimal residualQty = (qtyAfterRoundLot - integralQty) * roundLot;

                    AllocationLevelList newStrategies = account.Childs;

                    double remainingQty = 0.0;
                    if (qtyAfterRoundLot > 0 && allowedQty > 0)
                    {
                        remainingQty = CalculateAccountAllocations(Convert.ToDouble(allowedQty), strategyQty, integralQty, ref newStrategies, residualQty, qtyAfterRoundLot, roundLot, CachedDataManager.GetInstance.GetlastPreferencedAccountID(), false);
                    }
                    RemoveAccountsWithZeroQty(ref newStrategies);
                    if (remainingQty != 0)
                    {
                        return "Can not allocate more than cum qty!";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Removes the extra share.
        /// </summary>
        /// <param name="newaccounts">The newaccounts.</param>
        /// <param name="index">The index.</param>
        /// <param name="remainingQtyDecimal">The remaining qty decimal.</param>
        /// <param name="ApplyRoundRobin">if set to <c>true</c> [apply round robin].</param>
        private void RemoveExtraShare(ref AllocationLevelList newaccounts, int index, ref decimal remainingQtyDecimal, bool ApplyRoundRobin)
        {
            try
            {
                for (int i = index; i <= newaccounts.Collection.Count - 1; i++)
                {
                    if (remainingQtyDecimal == 0)
                    {
                        break;
                    }
                    else
                    {
                        AllocationLevelClass accountToAdjNew = newaccounts.Collection[i];
                        // check account quantiy should be greater that remaining qty
                        if (decimal.Parse(accountToAdjNew.AllocatedQty.ToString()) >= Math.Abs(remainingQtyDecimal))
                        {
                            accountToAdjNew.AllocatedQty--;
                            remainingQtyDecimal++;

                            if (ApplyRoundRobin)
                            {
                                CachedDataManager.GetInstance.SetPreferencedAccountID(accountToAdjNew.LevelnID);
                            }
                        }
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
        /// Allocates the transaction.
        /// </summary>
        /// <param name="newaccounts">The newaccounts.</param>
        /// <param name="allowedQty">The allowed qty.</param>
        /// <param name="QtytoAllocate">The qtyto allocate.</param>
        /// <param name="checkNonZero">if set to <c>true</c> [check non zero].</param>
        /// <param name="ApplyRoundRobin">if set to <c>true</c> [apply round robin].</param>
        /// <param name="preferencedAccountID">The preferenced account identifier.</param>
        /// <returns></returns>
        private AllocationLevelClass AllocateTransaction(ref AllocationLevelList newaccounts, double allowedQty, ref decimal QtytoAllocate, bool checkNonZero, bool ApplyRoundRobin, int preferencedAccountID)
        {
            AllocationLevelClass accountToAdj = null;
            //MUKUL:2013/09/19 we need to move in a cyclical way in order to allocate that residual share 
            //hence we will first sort the accounts list by account ID and start from the account next to account which was last given preference while allocating that
            //residual share...
            try
            {
                int index = GetIndexToStartWith(ref newaccounts, ApplyRoundRobin, preferencedAccountID, false);
                if (newaccounts.Collection.Count > index)
                {
                    for (int i = index; i < newaccounts.Collection.Count; i++)
                    {
                        accountToAdj = newaccounts.Collection[i];
                        if (accountToAdj.AllocatedQty > 0 || !checkNonZero)
                        {
                            int result = 0;
                            int.TryParse(QtytoAllocate.ToString(), out result);
                            if (result != 0 && ApplyRoundRobin)
                            {
                                // if the quantity is integral then we have to make sure that the integral part is allocated one by one to all the accounts as there may be 
                                //scenarios where the remaining qty is greater than 1. so we cannot straight away allocate the whole part to one account..

                                accountToAdj.AllocatedQty = Convert.ToDouble(Convert.ToDecimal(accountToAdj.AllocatedQty) + 1);
                                accountToAdj.Percentage = Convert.ToSingle((Convert.ToDecimal(accountToAdj.AllocatedQty) * 100) / Convert.ToDecimal(allowedQty));
                                QtytoAllocate = QtytoAllocate - 1;
                                if (QtytoAllocate <= 0)
                                {
                                    break;
                                }
                                else
                                {
                                    int counter = (i + 1);
                                    if (counter >= newaccounts.Collection.Count)
                                    {
                                        i = -1;
                                        index = 0;
                                        continue;
                                    }
                                }
                            }
                            //if the quantity is fractional or the preference is to set the extra shares to specific account then allocate the remaining shares to account at the preferenced index and break out of the loop..
                            else
                            {
                                accountToAdj.AllocatedQty = Convert.ToDouble(Convert.ToDecimal(accountToAdj.AllocatedQty) + QtytoAllocate);
                                accountToAdj.Percentage = Convert.ToSingle((Convert.ToDecimal(accountToAdj.AllocatedQty) * 100) / Convert.ToDecimal(allowedQty));
                                QtytoAllocate = 0;
                                break;
                            }
                        }
                    }
                    if (QtytoAllocate != 0)
                    {
                        for (int i = 0; i < newaccounts.Collection.Count; i++)
                        {
                            accountToAdj = newaccounts.Collection[i];
                            if (accountToAdj.AllocatedQty > 0 || !checkNonZero)
                            {
                                accountToAdj.AllocatedQty = Convert.ToDouble(Convert.ToDecimal(accountToAdj.AllocatedQty) + QtytoAllocate);
                                accountToAdj.Percentage = Convert.ToSingle((Convert.ToDecimal(accountToAdj.AllocatedQty) * 100) / Convert.ToDecimal(allowedQty));
                                QtytoAllocate = 0;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountToAdj;
        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        public AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName)
        {
            AllocationFixedPreference allocationScheme = null;
            try
            {
                allocationScheme = PreferenceManager.GetInstance.GetAllocationSchemeByName(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the index to start with.
        /// </summary>
        /// <param name="newaccounts">The newaccounts.</param>
        /// <param name="ApplyRoundRobin">if set to <c>true</c> [apply round robin].</param>
        /// <param name="preferencedAccountID">The preferenced account identifier.</param>
        /// <param name="isRemoveShare">if set to <c>true</c> [is remove share].</param>
        /// <returns></returns>
        private int GetIndexToStartWith(ref AllocationLevelList newaccounts, bool ApplyRoundRobin, int preferencedAccountID, bool isRemoveShare)
        {
            int index = 0;
            try
            {
                newaccounts.Collection.Sort(delegate (AllocationLevelClass t1, AllocationLevelClass t2) { return t1.LevelnID.CompareTo(t2.LevelnID); });
                int lastPreferencedaccountID = int.MinValue;
                if (ApplyRoundRobin)
                {
                    lastPreferencedaccountID = CachedDataManager.GetInstance.GetlastPreferencedAccountID();
                }
                else
                {
                    if (preferencedAccountID != -1 && preferencedAccountID != int.MinValue)
                    {
                        lastPreferencedaccountID = preferencedAccountID;
                        if (!newaccounts.Contains(lastPreferencedaccountID))
                        {
                            //selected account ID not present in the collection then we should default it to account with the highest percentage...
                            if (newaccounts.Collection.Count > 0)
                            {
                                newaccounts.Collection.Sort(delegate (AllocationLevelClass t1, AllocationLevelClass t2) { return (t1.Percentage.CompareTo(t2.Percentage) * (-1)); });
                                lastPreferencedaccountID = newaccounts.Collection[0].LevelnID;
                            }
                        }
                    }
                }
                if (lastPreferencedaccountID != int.MinValue)
                {
                    foreach (AllocationLevelClass level1Class in newaccounts.Collection)
                    {
                        //extra share should not be removed from the account for which preference is set...
                        if (isRemoveShare && (preferencedAccountID != -1 && preferencedAccountID != int.MinValue))
                        {
                            if (level1Class.LevelnID != lastPreferencedaccountID)
                            {
                                break;
                            }
                        }
                        //no extra share to specific account preference is set...
                        else
                        {
                            if (level1Class.LevelnID == lastPreferencedaccountID)
                            {
                                break;
                            }
                        }
                        index++;
                    }
                    if (ApplyRoundRobin)
                    {
                        index++;
                    }
                    if (newaccounts.Collection.Count <= index)
                    {
                        //pick the first account in the collection...
                        index = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return index;
        }

        /// <summary>
        /// Saves and Updates Allocation Scheme in T_AllocationScheme 
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocationSchemeDate">The allocation scheme date.</param>
        /// <param name="allocationSchemeXML">The allocation scheme XML.</param>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        public int SaveAllocationScheme(AllocationFixedPreference fixedPref)
        {
            int allocationSchemeID = 0;
            try
            {
                allocationSchemeID = PreferenceManager.GetInstance.SaveAllocationScheme(fixedPref);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemeID;
        }

        /// <summary>
        /// Deletes Allocation Scheme from T_AllocationScheme 
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        public bool DeleteAllocationScheme(int allocationSchemeID, string schemeName)
        {
            bool isSaved = false;
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.DELETE_ALLOCATION_SCHEME);
                int affectedRow = PreferenceManager.GetInstance.DeleteAllocationScheme(allocationSchemeID, schemeName);
                if (affectedRow > 0)
                {
                    isSaved = true;
                    // publish deleted scheme on allocation UI
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-16542
                    PublishingHelper.PublishAllocationSchemeUpdated(-1, allocationSchemeID);
                }
                else
                {
                    isSaved = false;
                }
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.DELETE_ALLOCATION_SCHEME);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Returns the recon report for the given scheme on given allocation date
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="fromAllocationDate">From allocation date.</param>
        /// <param name="toAllocationDate">To allocation date.</param>
        /// <returns></returns>
        public DataSet GetAllocationSchemeReconReport(string allocationSchemeName, DateTime fromAllocationDate, DateTime toAllocationDate)
        {
            DataSet dsAllocationScheme = null;
            try
            {
                dsAllocationScheme = PostTradeDataManager.GetAllocationSchemeReconReport(allocationSchemeName, fromAllocationDate, toAllocationDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dsAllocationScheme;
        }

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="baseMsg">The base MSG.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        public TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID)
        {
            TaxLot taxLot = null;
            try
            {
                taxLot = CommonHelper.CreateUnAllocatedTaxLot(baseMsg, groupID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return taxLot;
        }

        /// <summary>
        /// Generates the group identifier.
        /// </summary>
        /// <returns></returns>
        public string GenerateGroupID()
        {
            try
            {
                return AllocationIDGenerator.GenerateGroupID();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets all commission rules.
        /// </summary>
        /// <returns></returns>
        public List<CommissionRule> GetAllCommissionRules()
        {
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.GET_ALL_COMMISSIONRULE);
                List<CommissionRule> allCommissionRules = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_ALL_COMMISSIONRULE);
                return allCommissionRules;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the account pb details.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAccountPBDetails()
        {
            Dictionary<int, string> accountPBDetails = new Dictionary<int, string>();
            try
            {
                accountPBDetails = PostTradeDataManager.GetAccountPBDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return accountPBDetails;
        }

        /// <summary>
        /// Gets the taxlot details to update external transaction identifier.
        /// </summary>
        /// <param name="taxlotID">The taxlot identifier.</param>
        /// <returns></returns>
        public DataTable GetTaxlotDetailsToUpdateExternalTransactionID(string taxlotID)
        {
            try
            {
                DataSet ds = PostTradeDataManager.GetTaxlotDetailsToUpdateExternalTransactionID(taxlotID);
                if (ds != null && ds.Tables[0] != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Clear cache masterfund based postions chache
        /// </summary>
        public void ClearCacheMasterFundBasedPositions()
        {
            try
            {
                PreferenceManager.GetInstance.ClearCacheMasterFundBasedPositions();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Import Allocation Group Members

        /// <summary>
        /// Creates the allcation group from position master.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public AllocationGroup CreateAllcationGroupFromPositionMaster(PositionMaster position)
        {
            AllocationGroup group = new AllocationGroup();
            try
            {

                position.AUECLocalDate = position.PositionStartDate;
                group.GroupID = AllocationIDGenerator.GenerateGroupID();
                group.AssetID = position.AssetID;
                if (position.AccountID > 0)
                {
                    group.AllocatedQty = position.NetPosition;
                    group.IsPreAllocated = true;
                }
                group.OrderSideTagValue = position.SideTagValue;
                if (string.IsNullOrWhiteSpace(group.OrderSide))
                    group.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(group.OrderSideTagValue);
                group.PositionTagValue = CommonHelper.GetPositionTagBySide(group.OrderSideTagValue);
                group.Symbol = position.Symbol;
                group.Quantity = position.NetPosition;
                group.CumQty = position.NetPosition;
                group.AvgPrice = position.CostBasis;
                group.AssetID = position.AssetID;
                group.UnderlyingID = position.UnderlyingID;
                group.ExchangeID = position.ExchangeID;
                group.CurrencyID = position.CurrencyID;
                group.AUECID = position.AUECID;
                group.TradingAccountID = position.TradingAccountID;
                group.CompanyUserID = position.UserID;
                group.CounterPartyID = position.CounterPartyID;
                group.VenueID = position.VenueID;
                group.CumQty = position.NetPosition;
                group.IsManualGroup = true;
                group.AUECLocalDate = Convert.ToDateTime(position.AUECLocalDate);

                group.NirvanaProcessDate = position.NirvanaProcessDate;
                if (group.NirvanaProcessDate <= DateTimeConstants.MinValue)
                    group.NirvanaProcessDate = group.AUECLocalDate;

                group.ProcessDate = Convert.ToDateTime(position.ProcessDate);
                group.OriginalPurchaseDate = Convert.ToDateTime(position.OriginalPurchaseDate);
                group.Commission = position.Commission;
                group.SoftCommission = position.SoftCommission;
                group.OtherBrokerFees = position.Fees;
                group.ClearingBrokerFee = position.ClearingBrokerFee;
                group.StampDuty = position.StampDuty;
                group.TransactionLevy = position.TransactionLevy;
                group.ClearingFee = position.ClearingFee;
                group.TaxOnCommissions = position.TaxOnCommissions;
                group.MiscFees = position.MiscFees;
                group.SecFee = position.SecFee;
                group.OccFee = position.OccFee;
                group.OrfFee = position.OrfFee;
                group.TaxLotClosingId = position.TaxLotClosingId;
                group.UnderlyingSymbol = position.UnderlyingSymbol;
                group.SettlementDate = Convert.ToDateTime(position.PositionSettlementDate);
                group.AccruedInterest = position.AccruedInterest;
                group.AccrualBasis = position.AccrualBasis;
                group.BondType = position.BondType;
                group.Freq = position.Freq;
                group.FirstCouponDate = position.FirstCouponDate;
                group.FXConversionMethodOperator = position.FXConversionMethodOperator.ToString();
                group.MaturityDate = position.MaturityDate;
                group.IsZero = position.IsZero;
                if (position.TransactionSource.Equals(TransactionSource.Closing))
                {
                    if (position.IsManualyExerciseAssign)
                        group.GroupStatus = PostTradeEnums.Status.ExerciseAssignManually;
                    else
                        group.GroupStatus = PostTradeEnums.Status.Exercise;
                }
                group.IssueDate = position.IssueDate;
                group.CouponRate = position.Coupon;
                group.ContractMultiplier = position.Multiplier;
                //updated RoundLot field value from secMasterObject,PRANA-12674 
                group.RoundLot = position.RoundLot;

                if (string.IsNullOrEmpty(position.PositionExpirationDate))
                {
                    group.ExpirationDate = DateTimeConstants.MinValue;
                }
                else
                {
                    group.ExpirationDate = Convert.ToDateTime(position.PositionExpirationDate);
                }
                group.Description = position.Description;
                group.InternalComments = position.InternalComments;
                group.LeadCurrencyID = position.LeadCurrencyID;
                group.VsCurrencyID = position.VsCurrencyID;

                group.FXRate = position.FXRate;
                group.CommissionSource = Convert.ToInt32(position.CommissionSource);
                group.SoftCommissionSource = Convert.ToInt32(position.SoftCommissionSource);

                //Temprary Commented
                group.FXConversionMethodOperator = position.FXConversionMethodOperator.ToString();
                // Added by Sandeep as on 25-Feb-2013, these 2 fields are optional 
                // some times our clients can provide these and we need to keep in our database.
                group.LotId = position.LotId;
                group.ExternalTransId = position.ExternalTransId;

                group.TradeAttribute1 = position.TradeAttribute1;
                group.TradeAttribute2 = position.TradeAttribute2;
                group.TradeAttribute3 = position.TradeAttribute3;
                group.TradeAttribute4 = position.TradeAttribute4;
                group.TradeAttribute5 = position.TradeAttribute5;
                group.TradeAttribute6 = position.TradeAttribute6;
                group.SetTradeAttribute(position.GetTradeAttributesAsDict());
                group.TransactionType = position.TransactionType;

                group.TransactionSource = position.TransactionSource;
                group.TransactionSourceTag = (int)position.TransactionSource;

                group.WorkflowState = (int)position.WorkflowState;
                if (position.IsSwapped == 1)
                {
                    group.IsSwapped = true;
                    SwapParameters swapParameter = new SwapParameters();
                    swapParameter.BenchMarkRate = position.BenchMarkRate;
                    swapParameter.DayCount = position.DayCount;
                    swapParameter.Differential = position.Differential;
                    swapParameter.FirstResetDate = Convert.ToDateTime(position.FirstResetDate);
                    swapParameter.GroupID = group.GroupID;
                    swapParameter.NotionalValue = position.NotionalValue;
                    swapParameter.OrigCostBasis = position.CostBasis;
                    swapParameter.OrigTransDate = Convert.ToDateTime(position.OrigTransDate);
                    swapParameter.ResetFrequency = position.ResetFrequency;
                    swapParameter.SwapDescription = position.SwapDescription;

                    group.SwapParameters = swapParameter;
                }
                group.SettlementCurrencyID = position.SettlementCurrencyID;
                group.OptionPremiumAdjustment = position.OptionPremiumAdjustment;
                group.ChangeType = position.ChangeType;
                AllocationOrder allocOrder = new AllocationOrder();
                allocOrder.ClOrderID = position.ExternalOrderID;
                allocOrder.ParentClOrderID = position.ExternalOrderID;
                allocOrder.GroupID = group.GroupID;
                allocOrder.CumQty = position.NetPosition;

                // Corrected http://jira.nirvanasolutions.com:8080/browse/PRANA-2106
                //More fields added as they are needed while groupin data
                allocOrder.Quantity = position.NetPosition;
                allocOrder.OriginalPurchaseDate = DateTime.Parse(position.OriginalPurchaseDate);
                allocOrder.AUECLocalDate = DateTime.Parse(position.AUECLocalDate);
                allocOrder.ProcessDate = DateTime.Parse(position.ProcessDate);
                allocOrder.VenueID = position.VenueID;
                allocOrder.CounterPartyID = position.CounterPartyID;
                allocOrder.TradingAccountID = position.TradingAccountID;
                allocOrder.OrderSideTagValue = position.SideTagValue;
                allocOrder.SettlementDate = DateTime.Parse(position.PositionSettlementDate);

                // Assign the file id and name to the trade so that it can be identified from which file the trade was imported
                allocOrder.ImportFileLogObj = new ImportFileLog();
                if (position.ImportFileID != 0)
                {
                    allocOrder.ImportFileLogObj.ImportFileID = position.ImportFileID; ;
                }
                allocOrder.AvgPrice = position.CostBasis;
                allocOrder.InternalComments = position.InternalComments;
                allocOrder.Description = position.Description;
                allocOrder.SettlementCurrencyID = position.SettlementCurrencyID;
                allocOrder.ChangeType = position.ChangeType;
                allocOrder.TransactionSource = position.TransactionSource;
                allocOrder.TransactionSourceTag = (int)position.TransactionSource;
                group.AddOrder(allocOrder);

                List<AllocationGroup> groups = new List<AllocationGroup>();
                groups.Add(group);
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
            return group;
        }

        /// <summary>
        /// Createands the save group from taxlot.
        /// </summary>
        /// <param name="allocatedTrade">The allocated trade.</param>
        /// <returns></returns>
        public List<AllocationGroup> CreateandSaveGroupFromTaxlot(List<TaxLot> allocatedTrade, bool isCopyTradeAttrbsPrefUsed)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                foreach (TaxLot taxlot in allocatedTrade)
                {
                    AllocationGroup group = CreateCloseAllcationGroupFromAllocationTrade(taxlot, isCopyTradeAttrbsPrefUsed);
                    //set account details
                    groups.Add(group);
                }
                UpdateState(groups);
                SaveGroupsForFills(groups, string.Empty, true);
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
            return groups;
        }

        /// <summary>
        /// This creates the closing transaction for the supplied allocatedtrade
        /// </summary>
        /// <param name="allocatedTrade"></param>
        /// <returns></returns>
        private AllocationGroup CreateCloseAllcationGroupFromAllocationTrade(TaxLot allocatedTrade, bool isCopyTradeAttrbsPrefUsed)
        {
            AllocationGroup group = new AllocationGroup();
            try
            {
                //Added to remove blank field, PRANA-7215
                group.CompanyUserID = allocatedTrade.CompanyUserID;
                group.VenueID = allocatedTrade.VenueID;
                group.OrderTypeTagValue = allocatedTrade.OrderTypeTagValue;
                group.TradingAccountID = allocatedTrade.TradingAccountID;

                //Updated company user name in group, PRANA-12482
                group.CompanyUserName = allocatedTrade.CompanyUserName;

                group.GroupID = AllocationIDGenerator.GenerateGroupID();
                group.AssetID = (int)allocatedTrade.AssetCategoryValue;
                group.AllocatedQty = allocatedTrade.SettledQty;
                group.OrderSideTagValue = allocatedTrade.OrderSideTagValue;
                group.Symbol = allocatedTrade.Symbol;
                group.Quantity = allocatedTrade.SettledQty;
                group.UnderlyingID = (int)allocatedTrade.UnderlyingID;
                group.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(allocatedTrade.AUECID);
                group.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(group.ExchangeID);
                group.CurrencyID = allocatedTrade.CurrencyID;
                group.AUECID = allocatedTrade.AUECID;
                group.CumQty = allocatedTrade.SettledQty;
                group.IsManualGroup = false;
                group.IsNDF = allocatedTrade.IsNDF;
                group.FixingDate = allocatedTrade.FixingDate;
                group.CounterPartyID = allocatedTrade.CounterPartyID;
                //Narendra Kumar Jangir, August 27 2013
                //Set TransactionType from allocated trade to group so that it can be saved in DB
                group.TransactionType = allocatedTrade.TransactionType;
                group.TransactionSource = allocatedTrade.TransactionSource;
                group.TransactionSourceTag = allocatedTrade.TransactionSourceTag;
                //PRANA-34584 Nothing generated in Trade Attribute fileds in Allocation while Exercising/Assigning
                if (isCopyTradeAttrbsPrefUsed)
                {
                    group.TradeAttribute1 = allocatedTrade.TradeAttribute1;
                    group.TradeAttribute2 = allocatedTrade.TradeAttribute2;
                    group.TradeAttribute3 = allocatedTrade.TradeAttribute3;
                    group.TradeAttribute4 = allocatedTrade.TradeAttribute4;
                    group.TradeAttribute5 = allocatedTrade.TradeAttribute5;
                    group.TradeAttribute6 = allocatedTrade.TradeAttribute6;
                    group.SetTradeAttribute(allocatedTrade.GetTradeAttributesAsDict()); 
                }
                //PRANA-8500 - [Closing] Incorrect settlement field for the generated trade while Exercising/Assigning and Expiring option
                group.SettlementCurrencyID = allocatedTrade.SettlementCurrencyID;
                /// Settlementdate for the closing transaction is equal to the trade date of allocatedtrade.
                if (allocatedTrade.ISSwap)
                {
                    ///Rahul 20120206
                    ///Updated for Swap Expire/Expire and rollOver cases..
                    ///Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1743
                    group.SettlementDate = allocatedTrade.SettlementDate;
                    group.AUECLocalDate = allocatedTrade.AUECLocalDate;
                    group.OriginalPurchaseDate = allocatedTrade.OriginalPurchaseDate;
                    group.ProcessDate = allocatedTrade.AUECLocalDate;
                }
                else
                {
                    group.SettlementDate = (group.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FXForward) ? allocatedTrade.SettlementDate : allocatedTrade.AUECModifiedDate;
                    group.AUECLocalDate = Convert.ToDateTime(allocatedTrade.AUECModifiedDate.Date.AddHours(20).ToString());
                    group.OriginalPurchaseDate = allocatedTrade.AUECModifiedDate;
                    group.ProcessDate = allocatedTrade.AUECModifiedDate;
                }
                group.FXConversionMethodOperator = allocatedTrade.FXConversionMethodOperator;
                group.FXRate = allocatedTrade.FXRate;

                ///Why added 20 hrs in the closed date?

                if (!allocatedTrade.ISSwap)
                {
                    switch (allocatedTrade.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                            group.OrderSideTagValue = FIXConstants.SIDE_Sell;
                            break;
                        case FIXConstants.SIDE_Buy_Open:
                            group.OrderSideTagValue = FIXConstants.SIDE_Sell_Closed;
                            break;
                        case FIXConstants.SIDE_Buy_Closed:
                            group.OrderSideTagValue = FIXConstants.SIDE_Sell_Open;
                            break;
                        case FIXConstants.SIDE_Sell_Open:
                            group.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;
                            break;
                        case FIXConstants.SIDE_SellShort:
                            group.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;
                            break;
                        case FIXConstants.SIDE_Sell:
                            group.OrderSideTagValue = FIXConstants.SIDE_Buy;
                            break;
                        case FIXConstants.SIDE_Sell_Closed:
                            group.OrderSideTagValue = FIXConstants.SIDE_Buy_Open;
                            break;
                        default:
                            group.OrderSideTagValue = FIXConstants.SIDE_Sell_Closed;
                            break;
                    }
                }
                group.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(group.OrderSideTagValue);
                group.PositionTagValue = CommonHelper.GetPositionTagBySide(group.OrderSideTagValue);

                // group.SettlementDate = Convert.ToDateTime(position.PositionSettlementDate);
                switch (allocatedTrade.ClosingMode)
                {
                    case ClosingMode.SwapExpireAndRollover:
                    case ClosingMode.SwapExpire:
                        if (allocatedTrade.ISSwap)
                        {
                            group.SwapParameters = allocatedTrade.SwapParameters;
                            group.IsSwapped = true;
                            group.TaxLotClosingId = allocatedTrade.TaxLotClosingId;
                        }
                        group.AvgPrice = allocatedTrade.SwapParameters.ClosingPrice;
                        ///Commented because of Dates in swap related cases.
                        //group.AUECLocalDate = allocatedTrade.SwapParameters.FirstResetDate;
                        break;

                    case ClosingMode.Cash:
                    case ClosingMode.CashSettleinBaseCurrency:
                    case ClosingMode.Expire:
                        if (allocatedTrade.TransactionSource == TransactionSource.Closing && (allocatedTrade.AssetID == (int)AssetCategory.FX || allocatedTrade.AssetID == (int)AssetCategory.FXForward))
                        {
                            group.AvgPrice = allocatedTrade.AvgPrice;
                            group.FXRate = allocatedTrade.AvgPrice;
                        }
                        else
                            group.AvgPrice = allocatedTrade.CashSettledPrice;
                        break;

                    case ClosingMode.Exercise:
                    case ClosingMode.Physical:
                        if (!allocatedTrade.IsExerciseAtZero)
                        {
                            switch (allocatedTrade.OrderSideTagValue)
                            {
                                case FIXConstants.SIDE_Buy:
                                case FIXConstants.SIDE_Buy_Open:
                                case FIXConstants.SIDE_Buy_Cover:
                                case FIXConstants.SIDE_Buy_Closed:
                                    //group.MiscFees = 0;
                                    group.OptionPremiumAdjustment = (-1) * (allocatedTrade.AvgPrice * allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty + (allocatedTrade.OpenTotalCommissionandFees * allocatedTrade.SettledQty / allocatedTrade.TaxLotQty));
                                    break;
                                default:
                                    //group.MiscFees = 0;
                                    group.OptionPremiumAdjustment = allocatedTrade.AvgPrice * allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty - (allocatedTrade.OpenTotalCommissionandFees * allocatedTrade.SettledQty / allocatedTrade.TaxLotQty);
                                    break;
                            }
                        }
                        break;
                    default:
                        group.AvgPrice = 0.0d;
                        break;
                }

                if (allocatedTrade.Level1ID > 0)
                {
                    group.IsPreAllocated = true;
                    AllocationLevelClass account = new AllocationLevelClass(group.GroupID);
                    account.LevelnID = allocatedTrade.Level1ID;
                    account.Name = allocatedTrade.Level1Name;
                    account.Percentage = 100;
                    account.AllocatedQty = allocatedTrade.SettledQty;
                    if (allocatedTrade.Level2ID > 0)
                    {
                        AllocationLevelClass strategy = new AllocationLevelClass(group.GroupID);
                        strategy.LevelnID = allocatedTrade.Level2ID;
                        strategy.Percentage = 100;
                        strategy.AllocatedQty = allocatedTrade.SettledQty;
                        account.AddChilds(strategy);
                    }
                    AllocationLevelList accounts = new AllocationLevelList();
                    accounts.Add(account);
                    group.Allocate(accounts);

                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        taxlot.ClosingMode = allocatedTrade.ClosingMode;
                        taxlot.IsNDF = allocatedTrade.IsNDF;
                        taxlot.FixingDate = allocatedTrade.FixingDate;
                    }
                }
                else
                {
                    group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
                }
                group.SetNameDetailsInGroup();
                List<AllocationGroup> groups = new List<AllocationGroup>();
                groups.Add(group);
                AllocationCheck(groups);
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
            return group;
        }

        /// <summary>
        /// Creates the and save positions. used in ctrlCreateAndImportPosition 
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <returns></returns>
        public List<AllocationGroup> CreateAndSavePositions(List<PositionMaster> postionMasterList)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                lock (lockerAllocationSave)
                {
                    return SavePositionMasterList(postionMasterList, string.Empty);
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
            return groups;
        }

        /// <summary>
        /// Creates the and save positions from import.
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <returns></returns>
        public async Task<int> CreateAndSavePositionsFromImport(List<PositionMaster> postionMasterList)
        {
            int groupListCount = int.MinValue;
            try
            {
                groupListCount = await System.Threading.Tasks.Task.Run(() => { return CreateAndSavePositionsFromImportAsync(postionMasterList); });
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
            return groupListCount;
        }

        /// <summary>
        /// Creates the and save positions from import asynchronous.
        /// used in ctrlRunDownload, on ctrlRunDownload control there is no use of Groups, it was just increasing congestion
        /// overload has been created because the same function is used on closing module also
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <returns></returns>
        private int CreateAndSavePositionsFromImportAsync(List<PositionMaster> postionMasterList)
        {
            List<AllocationGroup> groupList = new List<AllocationGroup>();
            try
            {
                groupList = SavePositionMasterList(postionMasterList, string.Empty);
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
            return groupList.Count;
        }

        /// <summary>
        /// Saves the position master list.
        /// </summary>
        /// <param name="postionMasterList">The postion master list.</param>
        /// <param name="connString">The connection string.</param>
        /// <returns></returns>
        private List<AllocationGroup> SavePositionMasterList(List<PositionMaster> postionMasterList, string connString)
        {
            XmlSaveHandler xmlSaveMgr = new XmlSaveHandler();
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                //For Saving in table
                xmlSaveMgr.CreatePositionsXml(postionMasterList);
                if (connString == string.Empty)
                {
                    xmlSaveMgr.SavePositionThrouhXml();
                    groups = CreateGroupsFromPositions(postionMasterList, false, connString);
                }
                else
                {
                    xmlSaveMgr.SavePositionThrouhXml(connString);
                    groups = CreateGroupsFromPositions(postionMasterList, true, connString);
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
            return groups;
        }

        /// <summary>
        /// This method creates groups from position master, allocate groups and save groups
        /// </summary>
        /// <param name="positions">list of positions</param>
        /// <param name="overridelocalname"></param>
        /// <param name="connString">connection string</param>
        /// <returns>list of allocation groups</returns>
        private List<AllocationGroup> CreateGroupsFromPositions(List<PositionMaster> positions, bool overridelocalname, string connString)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            Dictionary<string, List<AllocationGroup>> groupListAutoGroup = new Dictionary<string, List<AllocationGroup>>();
            Dictionary<string, List<AllocationGroup>> groupListDict = new Dictionary<string, List<AllocationGroup>>();
            // Create account Strategy cache to group allocation groups on basis of account and strategy and send this list for allocation, PRANA-12314
            Dictionary<string, List<AllocationGroup>> accountStrategyGroupCache = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                AutoGroupingRules autoGroupRules = PreferenceManager.GetInstance.GetAutoGroupingPreferences();
                //creating a list of group which having same AccountID and StrategyID then add this list in dictionary with AccountID and StrategyID key.
                positions.ForEach(position =>
                {
                    AllocationGroup group = CreateAllcationGroupFromPositionMaster(position);
                    string key = (position.AccountID > 0) ? GetAccountStrategyKey(position.AccountID, position.StrategyID) : "unallocated";
                    if (groupListAutoGroup.ContainsKey(key))
                        groupListAutoGroup[key].Add(group);
                    else if (groupListDict.ContainsKey(key))
                        groupListDict[key].Add(group);
                    else if (autoGroupRules != null && autoGroupRules.AutoGroup && IsAutoGroupEnabledForPref(position.AccountID))
                        groupListAutoGroup.Add(key, new List<AllocationGroup>() { group });
                    else
                        groupListDict.Add(key, new List<AllocationGroup>() { group });
                }
                );
                if (autoGroupRules != null && autoGroupRules.AutoGroup && groupListAutoGroup.Count > 0 && positions[0].TransactionSource.Equals(TransactionSource.TradeImport))
                {
                    AllocationAutoGroupingHelper.AutoGroupImportedTrades(autoGroupRules, groupListAutoGroup, -1).ToList().ForEach(x => groupListDict.Add(x.Key, x.Value));
                }
                else
                {
                    groupListAutoGroup.ToList().ForEach(x => groupListDict.Add(x.Key, x.Value));
                }

                foreach (var cacheKey in groupListDict.Keys)
                {
                    if (cacheKey != "unallocated")
                    {
                        groupListDict[cacheKey].ForEach(group =>
                        {
                            if (accountStrategyGroupCache.Keys.Contains(cacheKey))
                                accountStrategyGroupCache[cacheKey].Add(group);
                            else
                                accountStrategyGroupCache.Add(cacheKey, new List<AllocationGroup> { group });
                            groups.Add(group);
                        });
                    }
                    else
                    {
                        groupListDict[cacheKey].ForEach(group =>
                        {
                            string genratedCacheKey = GetAccountStrategyKey(int.MinValue, 0);
                            if (accountStrategyGroupCache.Keys.Contains(genratedCacheKey))
                                accountStrategyGroupCache[genratedCacheKey].Add(group);
                            else
                                accountStrategyGroupCache.Add(genratedCacheKey, new List<AllocationGroup> { group });
                            groups.Add(group);
                        });
                    }
                }
                //allocate groups in chunks based on account and strategy
                foreach (string accountStrategyKey in accountStrategyGroupCache.Keys)
                {
                    int accountID = 0;
                    int strategyID = 0;
                    GetAccountAndStrategyIDFromKey(accountStrategyKey, out accountID, out strategyID);
                    CreateAndValidateAllocationDetailsFromLevelIDS(accountID, strategyID, accountStrategyGroupCache[accountStrategyKey]);
                    AllocationCheck(accountStrategyGroupCache[accountStrategyKey]);
                    // SaveGroupsForFills(accountStrategyGroupCache[accountStrategyKey], connString, true);  sachin:- PRANA-34810 
                    foreach (AllocationGroup group in accountStrategyGroupCache[accountStrategyKey])
                    {
                        AddGroup(group);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groups;
        }

        /// <summary>
        /// generates unique key on basis of account and strategy ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="strategyID">strategy ID</param>
        /// <returns>unique key</returns>
        private string GetAccountStrategyKey(int accountID, int strategyID)
        {
            string uniqueKey = string.Empty;
            try
            {
                uniqueKey = "F|" + accountID + "|S|" + strategyID;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return uniqueKey;
        }

        /// <summary>
        /// gets value of account and strategy ID from accountStrategyKey
        /// </summary>
        /// <param name="accountStrategyKey">unique accountStrategy key</param>
        /// <param name="accountID">account ID</param>
        /// <param name="strategyID">strategyID</param>
        private void GetAccountAndStrategyIDFromKey(string accountStrategyKey, out int accountID, out int strategyID)
        {
            try
            {
                string[] idList = accountStrategyKey.Split('|');
                accountID = Convert.ToInt32(idList[1]);
                strategyID = Convert.ToInt32(idList[3]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                accountID = 0;
                strategyID = 0;
            }
        }

        /// <summary>
        /// Creates the and validate allocation details from level ids.
        /// </summary>
        /// <param name="level1ID">The level1 identifier.</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="level2ID">The level2 identifier.</param>
        /// <param name="group">The group.</param>
        /// <param name="cumQty">The cum qty.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        private void CreateAndValidateAllocationDetailsFromLevelIDS(int level1ID, String accountName, int level2ID, AllocationGroup group, double cumQty, bool forceAllocation = false)
        {
            try
            {
                bool setNameFromFile = false;
                AllocationLevelList DefaultAllocationLevelList = GetAllocationDetails(level1ID, accountName, level2ID, group, cumQty, out setNameFromFile, false, forceAllocation);
                if (DefaultAllocationLevelList != null && DefaultAllocationLevelList.Collection.Count > 0)
                    ValidateAllocationDetailsAndAllocate(group, ref DefaultAllocationLevelList, accountName, setNameFromFile);
                else
                {
                    group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
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
        }

        /// <summary>
        /// This method is created for data import groups allocation
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="strategyID">strategy ID</param>
        /// <param name="groups">list of allocation groups</param>
        /// <param name="forceAllocation">force allocation</param>
        private void CreateAndValidateAllocationDetailsFromLevelIDS(int accountID, int strategyID, List<AllocationGroup> groups, bool forceAllocation = false)
        {
            try
            {
                bool setNameFromFile = false;
                List<AllocationGroup> clonedgroupList = new List<AllocationGroup>();
                clonedgroupList.AddRange(DeepCopyHelper.Clone(groups));
                Dictionary<string, AllocationDefault> allocationdefault = GetAllocationDetails(accountID, strategyID, clonedgroupList, out setNameFromFile, true, forceAllocation);
                if (allocationdefault != null)
                {
                    Parallel.ForEach(allocationdefault.Keys, groupID =>
                    {
                        AllocationGroup group = groups.FirstOrDefault(x => x.GroupID == groupID);
                        if (allocationdefault[groupID] == null || allocationdefault[groupID].DefaultAllocationLevelList == null || allocationdefault[groupID].DefaultAllocationLevelList.Collection.Count == 0)
                            group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
                        else
                        {
                            AllocationLevelList def = allocationdefault[groupID].DefaultAllocationLevelList;
                            ValidateAllocationDetailsAndAllocate(group, ref def, "", setNameFromFile);
                            //update allocation scheme id and name, PRANA-20901
                            AllocationGroup grp = clonedgroupList.FirstOrDefault(y => y.GroupID == group.GroupID);
                            if (grp != null)
                            {
                                group.AllocationSchemeID = grp.AllocationSchemeID;
                                group.AllocationSchemeName = grp.AllocationSchemeName;
                            }
                        }

                    });
                }
                else
                    groups.ForEach(group => group.State = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method is created for Data Import
        /// </summary>
        /// <param name="level1ID">account ID</param>
        /// <param name="level2ID">strategy ID</param>
        /// <param name="groups">list of allocation groups</param>
        /// <param name="setNameFromFile">set name from file</param>
        /// <param name="isPreview"> Is preview</param>
        /// <param name="forceAllocation">force allocation</param>
        /// <returns>returns allocation result</returns>
        private Dictionary<string, AllocationDefault> GetAllocationDetails(int level1ID, int level2ID, List<AllocationGroup> groups, out bool setNameFromFile, bool isPreview, bool forceAllocation)
        {
            //Added condition to set default strategy id equals to 0, PRANA-12400
            if (level2ID == int.MinValue)
                level2ID = 0;
            setNameFromFile = false;
            Dictionary<string, AllocationDefault> allocationDefault = new Dictionary<string, AllocationDefault>();
            try
            {
                AllocationOperationPreference pref = GetPreferenceById(level1ID);
                AllocationMasterFundPreference masterFundPref = GetMasterFundPreferenceById(level1ID);
                string fixedPref = GetAllocationSchemeNameByID(level1ID);
                if (pref != null || masterFundPref != null || !string.IsNullOrWhiteSpace(fixedPref))
                {
                    AllocationResponse response = new AllocationResponse();
                    string prefenceName = pref != null ? pref.OperationPreferenceName : masterFundPref != null ? masterFundPref.MasterFundPreferenceName : fixedPref;

                    response = AllocateByPreference(groups, level1ID, -1, isPreview, forceAllocation);
                    if (response != null && response.GroupList.Count > 0)
                    {
                        response.GroupList.ForEach(group =>
                        {
                            AllocationDefault allocDef = new AllocationDefault();
                            allocDef.DefaultAllocationLevelList = group.Allocations;
                            if (allocationDefault.Keys.Contains(group.GroupID))
                                allocationDefault[group.GroupID] = allocDef;
                            else
                                allocationDefault.Add(group.GroupID, allocDef);
                        });
                        if (!string.IsNullOrWhiteSpace(response.Response))
                        {
                            string error = "Error processing pre-allocation for some symbols for Preference: " + prefenceName;
                            InformationReporter.GetInstance.Write(error);
                            Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            InformationReporter.GetInstance.Write(response.Response);
                        }
                    }
                    else
                    {
                        string error = "Error processing pre-allocation for Preference: " + prefenceName;
                        InformationReporter.GetInstance.Write(error);
                        Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write(response.Response);
                        return null;
                    }
                    Parallel.ForEach(allocationDefault.Values, allocDef =>
                    {
                        allocDef.DefaultID = level1ID;
                        allocDef.DefaultName = prefenceName;
                        allocDef.IsDefaultAllocationRule = true;
                    });

                }
                else // If no preference is defined with the given id then this should be some Id of the account
                {

                    if (level1ID == int.MinValue || !String.IsNullOrEmpty(CommonDataCache.CachedDataManager.GetInstance.GetAccount(level1ID)))
                    {
                        AllocationResponse response = new AllocationResponse();
                        //
                        SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
                        percentage.Add(level1ID, new AccountValue(level1ID, 100));
                        // Kuldeep: Added Strategy Quantity 0 as we do use only percentage in this case.
                        if (level2ID != int.MinValue)
                            percentage[level1ID].StrategyValueList.Add(new StrategyValue(level2ID, 100, 0));
                        AllocationParameter parameter = null;
                        //If account id is min value then send it for allocation with pref id int.minValue
                        if (level1ID == int.MinValue)
                            parameter = new AllocationParameter(
                                new AllocationRule()
                                {
                                    BaseType = AllocationBaseType.CumQuantity,
                                    RuleType = MatchingRuleType.None,
                                    MatchClosingTransaction = MatchClosingTransactionType.None,
                                    PreferenceAccountId = -1,
                                    ProrataAccountList = new List<int>(),
                                    ProrataDaysBack = 0,
                                }, percentage, int.MinValue, -1, true, isPreview);
                        else
                            parameter = new AllocationParameter(
                            new AllocationRule()
                            {
                                BaseType = AllocationBaseType.CumQuantity,
                                RuleType = MatchingRuleType.None,
                                MatchClosingTransaction = MatchClosingTransactionType.None,
                                PreferenceAccountId = -1,
                                ProrataAccountList = new List<int>(),
                                ProrataDaysBack = 0,
                            }, percentage, -1, -1, true, isPreview);

                        response = AllocateByParameter(groups, parameter, forceAllocation);
                        if (response != null && response.GroupList.Count > 0)
                        {
                            response.GroupList.ForEach(group =>
                            {
                                AllocationDefault allocDef = new AllocationDefault();
                                allocDef.DefaultAllocationLevelList = group.Allocations;

                                if (!(group.Allocations.Collection.Count > 0))
                                {
                                    allocDef = null;
                                }

                                if (allocationDefault.Keys.Contains(group.GroupID))
                                    allocationDefault[group.GroupID] = allocDef;
                                else
                                    allocationDefault.Add(group.GroupID, allocDef);
                            });
                            if (!string.IsNullOrWhiteSpace(response.Response))
                            {
                                string error = "Error processing pre-allocation for some symbols for account: " + CachedDataManager.GetInstance.GetAccountText(level1ID);
                                InformationReporter.GetInstance.Write(error);
                                Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                InformationReporter.GetInstance.Write(response.Response);
                            }
                        }
                        else
                        {
                            string error = "Error processing pre-allocation for account: " + CachedDataManager.GetInstance.GetAccountText(level1ID);
                            InformationReporter.GetInstance.Write(error);
                            Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            Logger.LoggerWrite(response.Response, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            InformationReporter.GetInstance.Write(response.Response);
                            return null;
                        }
                    }
                    else
                    {
                        string error = "Account or prefrence has been deleted: " + level1ID;
                        Logger.LoggerWrite(error, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        return null;
                    }
                }

                ///Comments by Ashish: 20121002
                /// allocationDefault.DefaultName represents the Allocation Default used, if any
                /// If allocation is already done using Default ALlocation Rule from Preferences, no Strategy Allocation should be done. 
                /// If Default Allocation is not Done and Only Account Allocation is done, then this section will proceed. 
                /// If Level2ID is present in FIX rules then strategy allocation will be done
                Parallel.ForEach(allocationDefault.Keys, groupID =>
                {
                    AllocationDefault allocDef = allocationDefault[groupID];
                    AllocationGroup group = groups.FirstOrDefault(x => x.GroupID == groupID);
                    if (allocDef != null && allocDef.DefaultName == string.Empty && level2ID != int.MinValue) // strategy allocation
                    {
                        AllocationLevelClass strategy = new AllocationLevelClass(group.GroupID);
                        strategy.LevelnID = level2ID;
                        strategy.Percentage = 100;
                        strategy.AllocatedQty = group.CumQty;
                        AllocationLevelClass account = allocDef.DefaultAllocationLevelList.GetAllocationLevel(level1ID);
                        if (account != null)
                        {
                            account.AddChilds(strategy);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationDefault;
        }

        #endregion

        #region Corporate Action On Allocation Groups Members

        /// <summary>
        /// Creates the allcation group from taxlot base.
        /// used in CorpAction\NameChangerule\Name Change
        /// </summary>
        /// <param name="taxlotBase">The taxlot base.</param>
        /// <returns></returns>
        public AllocationGroup CreateAllcationGroupFromTaxlotBase(TaxlotBase taxlotBase)
        {
            AllocationGroup group = new AllocationGroup();
            try
            {
                group.GroupID = AllocationIDGenerator.GenerateGroupID();
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.CorporateAction;
                group.AssetID = (int)taxlotBase.AssetCategory;
                group.UnderlyingID = (int)taxlotBase.Underlying;
                group.ExchangeID = taxlotBase.ExchangeID;
                group.CurrencyID = taxlotBase.CurrencyID;
                group.OrderSideTagValue = taxlotBase.OrderSideTagValue;
                group.Symbol = taxlotBase.Symbol;
                group.Quantity = taxlotBase.OpenQty;
                group.AvgPrice = taxlotBase.AvgPrice;
                group.AUECID = taxlotBase.AUECID;
                group.AUECLocalDate = taxlotBase.AUECLocalDate;
                group.ProcessDate = taxlotBase.ProcessDate;
                if (taxlotBase.OriginalPurchaseDate == DateTimeConstants.MinValue)
                {
                    group.OriginalPurchaseDate = group.ProcessDate;
                }
                else
                {
                    group.OriginalPurchaseDate = taxlotBase.OriginalPurchaseDate;
                }

                //group.AllocationDate  = taxlotBase.AUECLocalDate;
                group.CorpActionID = taxlotBase.CorpActionID;
                group.IsPreAllocated = true;
                group.PositionTagValue = (int)taxlotBase.PositionTag;
                group.ParentTaxlot_PK = (long)taxlotBase.ParentTaxlot_PK;
                group.OrderTypeTagValue = taxlotBase.OrderTypeTagValue;
                group.CounterPartyID = taxlotBase.CounterPartyID;
                group.VenueID = taxlotBase.VenueID;
                group.CumQty = taxlotBase.OpenQty;
                group.AllocatedQty = taxlotBase.AllocatedQty;
                group.ListID = taxlotBase.ListID;
                group.ISProrataActive = taxlotBase.ISProrataActive;
                group.AutoGrouped = taxlotBase.AutoGrouped;
                group.IsManualGroup = taxlotBase.IsManualGroup;
                group.Description = taxlotBase.Description;
                group.InternalComments = taxlotBase.InternalComments;
                group.FXRate = taxlotBase.FXRate;
                group.FXConversionMethodOperator = taxlotBase.FXConversionMethodOperator;
                group.TradingAccountID = taxlotBase.TradingAccountID;
                group.Commission = taxlotBase.OpenTotalCommissionandFees;
                group.CompanyUserID = taxlotBase.UserID;
                group.TradeAttribute1 = taxlotBase.TradeAttribute1;
                group.TradeAttribute2 = taxlotBase.TradeAttribute2;
                group.TradeAttribute3 = taxlotBase.TradeAttribute3;
                group.TradeAttribute4 = taxlotBase.TradeAttribute4;
                group.TradeAttribute5 = taxlotBase.TradeAttribute5;
                group.TradeAttribute6 = taxlotBase.TradeAttribute6;
                group.LotId = taxlotBase.LotId;
                group.ExternalTransId = taxlotBase.ExternalTransId;
                group.NotionalChange = taxlotBase.NotionalChange;
                group.AssetName = taxlotBase.AssetCategory.ToString();
                group.UnderlyingName = taxlotBase.Underlying.ToString();
                group.TransactionType = taxlotBase.TransactionType;

                group.SettlementDate = taxlotBase.SettlementDate;
                //true as force allocation as check side need not to be checked.
                CreateAndValidateAllocationDetailsFromLevelIDS(taxlotBase.Level1ID, "", taxlotBase.Level2ID, group, taxlotBase.OpenQty, true);
                List<AllocationGroup> groups = new List<AllocationGroup>();
                groups.Add(group);
                AllocationCheck(groups);
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
            return group;
        }

        /// <summary>
        /// used in CorpAction. Create allocation group from groupid to delete
        /// </summary>
        /// <param name="taxlotBase"></param>
        /// <returns></returns>
        public int DeleteGroupsFromCA(DataTable dtable)
        {
            int rowsAffected = 0;
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                foreach (DataRow row in dtable.Rows)
                {
                    AllocationGroup group = new AllocationGroup();
                    group.GroupID = row["GroupID"].ToString();
                    group.Symbol = row["Symbol"].ToString();
                    group.Quantity = Convert.ToDouble(row["TaxlotQty"].ToString());
                    group.AllocatedQty = Convert.ToDouble(row["TaxlotQty"].ToString());
                    group.CumQty = Convert.ToDouble(row["TaxlotQty"].ToString());
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.New;
                    group.AUECID = Convert.ToInt32(row["AUECID"].ToString());
                    group.AssetID = Convert.ToInt32(row["AssetID"].ToString());
                    group.OrderSideTagValue = row["OrderSideTagValue"].ToString();
                    group.AUECLocalDate = Convert.ToDateTime(row["AUECLocalDate"]);

                    int accountID = Convert.ToInt32(row["FundID"].ToString());
                    int strategyID = Convert.ToInt32(row["Level2ID"].ToString());

                    CreateAndValidateAllocationDetailsFromLevelIDS(accountID, "", strategyID, group, group.CumQty);
                    groups.Add(group);
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                    }
                    group.ResetTaxlotDictionary(group.TaxLots);
                }

                // Chunk Size picked from App.config
                int chunkSize = int.Parse(ConfigurationManager.AppSettings["CAToSaveGroupChunkSize"]);

                // Delete Groups in Chunks  
                List<List<AllocationGroup>> groupChunks = ChunkingManager.CreateChunks<AllocationGroup>(groups, chunkSize);
                foreach (List<AllocationGroup> groupChunkstoSave in groupChunks)
                {
                    UpdateState(groupChunkstoSave);
                    rowsAffected = SaveGroupsForFills(groupChunkstoSave, string.Empty, true);
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
            return rowsAffected;
        }

        #endregion

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
