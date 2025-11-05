using Prana.Allocation.Client.CacheStore;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.PositionManagement;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Prana.Global.Utilities;

namespace Prana.Allocation.Client
{
    public sealed class AllocationClientManager : IPublishing, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when [action after save].
        /// </summary>
        public event EventHandler<EventArgs<ActionAfterSavingData>> ActionAfterSave;

        /// <summary>
        /// Occurs when [allocation data change].
        /// </summary>
        public event EventHandler<EventArgs<bool>> AllocationDataChange;

        /// <summary>
        /// Occurs when [allocation data saved].
        /// </summary>
        public event EventHandler AllocationDataSaved;

        /// <summary>
        /// Occurs when [allocation data mismacthed].
        /// </summary>
        public event EventHandler AllocationDataChanged;

        /// <summary>
        /// Occurs when [allocation preferences saved].
        /// </summary>
        public event EventHandler<EventArgs<AllocationPreferencesCollection>> AllocationPreferencesSaved;

        /// <summary>
        /// Occurs when [allocation preference updated].
        /// </summary>
        public event EventHandler AllocationPreferenceUpdated;

        /// <summary>
        /// Occurs when [allocation scheme updated].
        /// </summary>
        public event EventHandler AllocationSchemeUpdated;

        /// <summary>
        /// Occurs when [new group received].
        /// </summary>
        public event EventHandler<EventArgs<List<AllocationGroup>>> NewGroupReceived;

        /// <summary>
        /// Occurs when [server proxy connected disconnected event].
        /// </summary>
        internal event EventHandler<EventArgs<bool>> ServerProxyConnectedDisconnectedEvent;

        /// <summary>
        /// Occurs when [update allocation data].
        /// </summary>
        public event EventHandler<EventArgs<AllocationResponse, bool>> UpdateAllocationData;

        /// <summary>
        /// Occurs when [update state of symbol].
        /// </summary>
        public event EventHandler<EventArgs<List<CurrentSymbolState>>> UpdateStateOfSymbol;

        /// <summary>
        /// Occurs when [update status bar].
        /// </summary>
        public event EventHandler<EventArgs<string>> UpdateStatusBar;

        /// <summary>
        /// Occurs when [New fills comes in for groups which was grouped/ungrouped].
        /// </summary>
        public event EventHandler<EventArgs<string>> GroupChangedAtServerSide;

        #endregion Events

        #region Members

        /// <summary>
        /// The _dispatcher
        /// </summary>
        static Dispatcher _dispatcher;

        /// <summary>
        /// filter List
        /// </summary>
        AllocationPrefetchFilter _filterList;

        /// <summary>
        /// User Id
        /// </summary>
        int _userId = Int32.MinValue;

        /// <summary>
        /// The _singleton
        /// </summary>
        private static AllocationClientManager _singleton;

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The _locker allocation save
        /// </summary>
        private readonly object _lockerAllocationSave = new object();

        /// <summary>
        /// The proxy
        /// </summary>
        private DuplexProxyBase<ISubscription> _proxy;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AllocationClientManager"/> class from being created.
        /// </summary>
        private AllocationClientManager()
        {
            try
            {
                _userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                MakeProxy();
                _dispatcher = Dispatcher.CurrentDispatcher;
                AllocationClientPreferenceManager.GetInstance.AllocationPreferencesSaved += AllocationClientManagerAllocationPreferencesSaved;
                AllocationClientServiceConnector.ServerProxyConnectedDisconnectedEvent += AllocationClientServiceConnector_ServerProxyConnectedDisconnectedEvent;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the deleted omiited groups.
        /// </summary>
        /// <param name="group">The group.</param>
        public void AddDeletedOmiitedGroups(AllocationGroup group)
        {
            try
            {
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                group.UpdateResetDictionaryWithDeletedState();
                AllocationClientGroupCache.GetInstance.AddDeletedOmittedGroup(group);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the exercised groups.
        /// </summary>
        /// <param name="UnderlyingGroup">The underlying group.</param>
        /// <param name="ClosingGroup">The closing group.</param>
        public void AddExercisedGroups(AllocationGroup UnderlyingGroup, AllocationGroup ClosingGroup)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.AddExercisedGroups(UnderlyingGroup, ClosingGroup);
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
        /// Adds the group.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void AddGroup(AllocationGroup group)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.AddGroup(group);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Adds the groups to UI.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public List<AllocationGroup> AddGroupsToUI(List<AllocationGroup> groups)
        {
            List<AllocationGroup> permittedgroups = new List<AllocationGroup>();
            try
            {
                var userAccounts = CachedDataManager.GetInstance.GetAllAccountIDsForUser();
                ClearData();
                foreach (var group in groups)
                {
                    var countNavLocked = 0;
                    var countNavUnlocked = 0;
                    var taxlotNavLockStatus = string.Empty;
                    NameValueFiller.FillNameDetailsOfMessage(group);
                    var isGroupAllowed = true;
                    //Filtering un allocated date based on masterfund
                    if (group.TaxLots.Count() == 0 && CachedDataManager.GetInstance.IsShowMasterFundonTT() && _filterList.Unallocated.ContainsKey(AllocationUIConstants.COMPANY_MASTER_FUND_ID))
                    {
                        var masterFundFilter = _filterList.Unallocated[AllocationUIConstants.COMPANY_MASTER_FUND_ID].Split(',').ToList().Select(x => CachedDataManager.GetInstance.GetMasterFund(Convert.ToInt32(x))).ToList();
                        if (!string.IsNullOrWhiteSpace(group.TradeAttribute6) && !masterFundFilter.Contains(group.TradeAttribute6))
                        {
                            isGroupAllowed = false;
                        }
                    }
                    foreach (var taxlot in group.TaxLots)
                    {
                        // Check AllocatedGroup if it contains user permitted Accounts, PRANA-12913
                        if (!userAccounts.Contains(taxlot.Level1ID.ToString()))
                        {
                            isGroupAllowed = false;
                            break;
                        }
                        taxlot.Level1Name = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                        taxlot.Level2Name = CachedDataManager.GetInstance.GetStrategyText(taxlot.Level2ID);
                        taxlot.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlot.OrderSideTagValue);
                        var dt = DateTime.MinValue;
                        var isValidTradeDate = DateTime.TryParse(taxlot.AUECLocalDate.ToString(), out dt);
                        if (isValidTradeDate)
                        {
                            var isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(taxlot.Level1ID, dt);
                            if (isTradeAllowed)
                            {
                                taxlot.NavLockStatus = ApplicationConstants.CONST_UNLOCKED;
                                countNavUnlocked++;
                            }
                            else
                            {
                                taxlot.NavLockStatus = ApplicationConstants.CONST_LOCKED;
                                countNavLocked++;
                            }
                        }
                        taxlotNavLockStatus = taxlot.NavLockStatus;
                    }
                    if (countNavLocked > 0 && countNavUnlocked > 0)
                    {
                        group.NavLockStatus = ApplicationConstants.C_Multiple;
                    }
                    else
                    {
                        group.NavLockStatus = taxlotNavLockStatus;
                    }
                    AllocationClientGroupCache.GetInstance.SetDefaultPersistenceStatus(group);
                    if (isGroupAllowed)
                    {
                        AllocationClientGroupCache.GetInstance.AddGroup(group);
                        permittedgroups.Add(group);
                    }
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return permittedgroups;
        }

        /// <summary>
        /// Allocated by Account 
        /// </summary>
        /// <param name="allocationGroupList"></param>
        /// <param name="pref"></param>
        /// <param name="isReallocate"></param>
        /// <param name="isChanged"></param>
        /// <param name="forceAllocation"></param>
        /// <returns></returns>
        internal AllocationResponse AllocateByAccount(List<AllocationGroup> allocationGroupList, AllocationParameter allocParameter, int prefId, bool isReallocate, bool forceAllocation, bool pttAllocationSelected)
        {
            try
            {
                AllocationResponse response = new AllocationResponse();
                if (isReallocate)
                {
                    AllocationRule rule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, PreferenceAccountId = -1, MatchClosingTransaction = MatchClosingTransactionType.None };
                    if (rule == null || allocParameter == null)
                    {
                        response.Response = "Please select a valid preference";
                    }
                    else
                    {
                        //set allocation scheme id equal to 0 in case of manual reallocation, PRANA-20901
                        allocationGroupList.ForEach(x => x.AllocationSchemeID = 0);
                        response = AllocationClientDataManager.GetInstance.AllocateByParameter(allocationGroupList, new AllocationParameter(rule, allocParameter.TargetPercentage, -1, _userId, true), forceAllocation, pttAllocationSelected);
                    }
                }
                else
                {
                    if (prefId == Int32.MinValue)
                    {
                        response = AllocationClientDataManager.GetInstance.AllocateByParameter(allocationGroupList, new AllocationParameter(allocParameter.CheckListWisePreference, allocParameter.TargetPercentage, -1, _userId, true), forceAllocation, pttAllocationSelected);
                    }
                    else
                    {
                        response = AllocationClientDataManager.GetInstance.AllocateByPreference(allocationGroupList, prefId, _userId, false, forceAllocation, pttAllocationSelected);
                    }
                }
                UpdateAuditTrialAndUI(allocationGroupList, response, isReallocate);
                return response;
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
        /// Allocate by the PTT preference.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <param name="isPreview">if set to <c>true</c> [is preview].</param>
        /// <param name="isReallocate">is reallocating trade</param>
        /// <returns></returns>
        internal AllocationResponse AllocateByPTTPreference(List<AllocationGroup> groups, bool isForceAllocationSelected, bool isPreview, bool isReallocate)
        {
            AllocationResponse response = new AllocationResponse();
            try
            {
                response = AllocationClientDataManager.GetInstance.AllocateByPTTPreference(groups, _userId, isPreview, isForceAllocationSelected);
                UpdateAuditTrialAndUI(groups, response, isReallocate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Allocates the by fixed preference.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="pref">The preference.</param>
        /// <param name="isForceAllocationSelected">if set to <c>true</c> [is force allocation selected].</param>
        /// <param name="isPTTAllocationSelected">if set to <c>true</c> [is PTT allocation selected].</param>
        /// <param name="isReallocate">if set to <c>true</c> [is reallocate].</param>
        /// <returns></returns>
        internal AllocationResponse AllocateByFixedPreference(List<AllocationGroup> groups, KeyValuePair<int, string> pref, bool isForceAllocationSelected, bool isPTTAllocationSelected, bool isReallocate)
        {
            AllocationResponse response = new AllocationResponse();
            try
            {
                response = AllocationClientDataManager.GetInstance.AllocateByPreference(groups, pref.Key, _userId, false, isForceAllocationSelected, isPTTAllocationSelected);
                UpdateAuditTrialAndUI(groups, response, isReallocate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Updates the audit trial.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="isReallocate">if set to <c>true</c> [is reallocate].</param>
        private void UpdateAuditTrialAndUI(List<AllocationGroup> previousAllocationGroup, AllocationResponse response, bool isReallocate)
        {
            try
            {
                List<AllocationGroup> allocationList = new List<AllocationGroup>();
                if (response != null && response.GroupList != null)
                    allocationList.AddRange(response.GroupList);
                foreach (AllocationGroup group in allocationList)
                {
                    //on Allocation Update the TradeAttribute6 if IsShowMasterFundonTT is enabled
                    if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    {
                        if (group.TaxLots.Count > 0)
                        {
                            group.TradeAttribute6 = FieldCalculator.GetMasterFundName(group);
                            EditTradeHelper.UpdateTradeAttributes(AllocationUIConstants.TradeAttribute6, group, null);
                            foreach (var taxlot in group.TaxLots)
                            {
                                var masterfundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(taxlot.Level1ID);
                                taxlot.TradeAttribute6 = CachedDataManager.GetInstance.GetMasterFund(masterfundId);
                            }
                        }
                        group.CompanyUserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                        group.IsModified = true;
                    }
                    group.IsSelected = false;
                    AddGroup(group);
                    //In case of Reallocation of Allocated Trade,the number of entries on AuditTrail for new Taxlots created will be for the updated no. of Taxlots. 
                    if (isReallocate)
                    {
                        var allocatedTaxlotIds = allocationList.SelectMany(x => x.TaxLots).Select(y => y.TaxLotID).ToList();
                        previousAllocationGroup.ForEach(x =>
                        {
                            var unallocatedTaxlots = x.TaxLots.ToList();
                            foreach (TaxLot taxlot in unallocatedTaxlots)
                            {
                                AuditManager.Instance.AddDeletedTaxlotsFromGroupToAuditEntry(x.GroupID, taxlot, taxlot.TaxLotQty.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString(), _userId);
                            }
                        });
                        // AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(previousAllocationGroup, true, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, group.Quantity.ToString(), "0", "Taxlots Deleted", _userId);
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.TaxlotCreated.ToString(), _userId);
                    }
                    else
                    {
                        var oldgroup = (previousAllocationGroup == null || previousAllocationGroup.Count == 0) ? group : previousAllocationGroup.Single(x => x.GroupID == group.GroupID);
                        AuditManager.Instance.AddGroupToAuditEntry(oldgroup, false, TradeAuditActionType.ActionType.REALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeleted.ToString(), _userId);
                        AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.GroupCreated.ToString(), _userId);
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.TaxlotCreated.ToString(), _userId);
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
        /// Handles the AllocationPreferencesSaved event of the AllocationClientManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationCompanyWisePref}"/> instance containing the event data.</param>
        private void AllocationClientManagerAllocationPreferencesSaved(object sender, EventArgs<AllocationPreferencesCollection> e)
        {
            try
            {
                if (AllocationPreferencesSaved != null)
                    AllocationPreferencesSaved(this, new EventArgs<AllocationPreferencesCollection>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ServerProxyConnectedDisconnectedEvent event of the AllocationClientServiceConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void AllocationClientServiceConnector_ServerProxyConnectedDisconnectedEvent(object sender, EventArgs<bool> e)
        {
            try
            {
                if (_dispatcher.CheckAccess())
                {
                    if (ServerProxyConnectedDisconnectedEvent != null)
                        ServerProxyConnectedDisconnectedEvent(this, new EventArgs<bool>(e.Value));

                    if (e.Value)
                        ClearData();
                }
                else
                    _dispatcher.Invoke(() => { AllocationClientServiceConnector_ServerProxyConnectedDisconnectedEvent(sender, e); });
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Anythings the changed.
        /// </summary>
        /// <returns></returns>
        internal bool AnythingChanged()
        {
            bool isAnythingChanged = false;
            try
            {
                isAnythingChanged = AllocationClientGroupCache.GetInstance.AnythingChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isAnythingChanged;
        }

        /// <summary>
        /// Applies the grouping.
        /// </summary>
        /// <param name="unallocatedGroups">The unallocated groups.</param>
        internal void ApplyGrouping(List<AllocationGroup> unallocatedGroups)
        {
            try
            {
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(true));

                BackgroundWorker bgWorkerAutoGroup = new BackgroundWorker();
                bgWorkerAutoGroup.DoWork += new DoWorkEventHandler(AutoGroupStarted);
                bgWorkerAutoGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AutoGroupCompleted);
                bgWorkerAutoGroup.RunWorkerAsync(unallocatedGroups);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Applies the un grouping.
        /// </summary>
        /// <param name="groups">The unallocated groups.</param>
        internal void ApplyUnGrouping(List<AllocationGroup> groups)
        {
            try
            {
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(true));
                BackgroundWorker bgWorkerAutoGroup = new BackgroundWorker();
                bgWorkerAutoGroup.DoWork += new DoWorkEventHandler(UnGroupStarted);
                bgWorkerAutoGroup.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UnGroupCompleted);
                bgWorkerAutoGroup.RunWorkerAsync(groups);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Automatics the group completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Exception"></exception>
        public void AutoGroupCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string message = string.Empty;
            try
            {
                if (e.Error == null)
                {
                    object[] groupingResult = e.Result as object[];
                    UpdateGroupingData(groupingResult);
                    message = groupingResult[0].ToString();
                }
                else
                {
                    message = "Something went wrong. Grouping is not completed.";
                    bool rethrow = Logger.HandleException(e.Error, LoggingConstants.POLICY_LOGONLY);
                    if (rethrow)
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>(message));
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(false));
            }
        }

        /// <summary>
        /// Automatics the group started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        public void AutoGroupStarted(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = AllocationManualGroupingHelper.AutoGroup(AllocationClientPreferenceManager.GetInstance.GetUserWisePreferences(), (List<AllocationGroup>)e.Argument, _userId);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                e.Result = "Error";
            }
        }

        /// <summary>
        /// Perform Unallocation of selected data on a background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgUnAllocateData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AllocationCompanyWisePref pref = GetCompanyWisePreferences();
                bool isMasterFundRatio = pref.EnableMasterFundAllocation && pref.IsOneSymbolOneMasterFundAllocation;
                List<AllocationGroup> allocationGroups = e.Argument as List<AllocationGroup>;
                StringBuilder alreadyClosedError = new StringBuilder();
                List<AllocationGroup> groupList = new List<AllocationGroup>();
                Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(allocationGroups);

                //Ankit Gupta: https://jira.nirvanasolutions.com:8443/browse/PRANA-24886
                foreach (AllocationGroup group in allocationGroups)
                {
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];
                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        group.AllocationSchemeID = 0;
                        group.AllocationSchemeName = string.Empty;
                        group.ErrorMessage = string.Empty;
                        groupList.Add(group);

                        if (isMasterFundRatio)
                            AllocationClientDataManager.GetInstance.UpdateAccountWisePostionInCache(group);

                        #region add elements to audit list
                        //this code is moved from method AllocationManager.GetInstance().UnAllocateGroup to reduce the iteration
                        AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.UNALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeleted.ToString(), _userId);
                        if (group.TaxLots.Count > 0)
                        {
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, TradeAuditActionType.ActionType.UNALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString(), _userId);
                        }
                        AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.UNALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.GroupCreated.ToString(), _userId);
                        #endregion
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is fully or partially closed.");
                            alreadyClosedError.Append(Environment.NewLine);
                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                        {
                            alreadyClosedError.Append("On GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(", corporate  Action is applied. First undo the corporate action to unallocate the trade.");
                            alreadyClosedError.Append(Environment.NewLine);

                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.Exercise) || groupStatus.Equals(PostTradeEnums.Status.IsExercised) || groupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually))
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is generated by exercise.");
                            alreadyClosedError.Append(Environment.NewLine);

                        }
                        else if (groupStatus.Equals(PostTradeEnums.Status.CostBasisAdjustment))     //Don't allow to unallocate for group generated by cost adjustment: http://jira.nirvanasolutions.com:8080/browse/PRANA-6806
                        {
                            alreadyClosedError.Append("GroupID : ");
                            alreadyClosedError.Append(group.GroupID);
                            alreadyClosedError.Append(" is generated by cost adjustment.");
                            alreadyClosedError.Append(Environment.NewLine);
                        }
                    }
                }

                List<AllocationGroup> groups = AllocationClientDataManager.GetInstance.UnAllocateGroups(groupList, _userId);
                //Writing alreadyClosed to file and returning path to e.result
                if (alreadyClosedError.Length > 0)
                {
                    //TODO: Remove reference for Windows.Forms
                    e.Result = new AllocationResponse() { Response = WriteResponseToFile(alreadyClosedError.ToString(), @"\Logs\UnallocationLog.txt"), GroupList = groups };
                }
                else
                {
                    e.Result = new AllocationResponse() { Response = UnAllocationCompletionStatus.Success.ToString(), GroupList = groups };
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Starts on main UI thread when background worker finishes unallocation of data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgUnAllocateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    AllocationResponse result = e.Result as AllocationResponse;
                    if (result.GroupList != null)
                    {
                        foreach (var group in result.GroupList)
                        {
                            AllocationClientGroupCache.GetInstance.AddGroup(group);
                        }
                    }
                    String message = result.Response;
                    if (!String.IsNullOrEmpty(message) && (message != UnAllocationCompletionStatus.Success.ToString()))
                    {

                        StringBuilder boxMessage = new StringBuilder();
                        boxMessage.AppendLine("Some groups could not be unallocated.");
                        if (message == UnAllocationCompletionStatus.FileWriteError.ToString())
                        {
                            boxMessage.Append("While writing the groupid(s) in the file, some issue occured. Please fetch the data and unallocate the trades again.");
                            MessageBox.Show(boxMessage.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            boxMessage.Append("Do you want to view details?");
                            MessageBoxResult dr = MessageBox.Show(boxMessage.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (dr == MessageBoxResult.Yes)
                            {
                                System.Diagnostics.Process.Start(message);
                            }
                        }
                        message = "Some groups could not be unallocated.";
                    }
                    else
                        message = "Unallocation Completed.";

                    if (result != null && result.GroupList != null)
                        result.GroupList.ForEach(x => x.IsSelected = false);

                    if (UpdateAllocationData != null)
                        UpdateAllocationData(this, new EventArgs<AllocationResponse, bool>(new AllocationResponse() { Response = message, GroupList = result.GroupList }, false));
                }
                else
                {

                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Unallocation Failed."));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Writes the response to file.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        internal string WriteResponseToFile(string error, string fileName)
        {
            String path = System.Windows.Forms.Application.StartupPath + fileName;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                    streamWriter.WriteLine(Environment.NewLine + DateTime.Now.ToString());
                    streamWriter.Write(error);
                    return path;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return UnAllocationCompletionStatus.FileWriteError.ToString();
        }

        /// <summary>
        /// Calculates the accured interest.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns></returns>
        internal double CalculateAccuredInterest(AllocationGroup allocationGroup)
        {
            double accuredInterest = allocationGroup.AccruedInterest;
            try
            {
                accuredInterest = AllocationClientDataManager.GetInstance.CalculateAccuredInterest(allocationGroup);
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
        internal AllocationGroup CalculateCommission(AllocationGroup group)
        {
            AllocationGroup allocationGroup = new AllocationGroup();
            try
            {
                allocationGroup = AllocationClientDataManager.GetInstance.CalculateCommission(group);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroup;
        }

        /// <summary>
        /// Cancels the edit changes.
        /// </summary>
        private void CancelEditChanges()
        {
            //TODO:UndoChanges
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public void ClearData()
        {
            try
            {
                AllocationClientGroupCache.GetInstance.ClearCaches();
                AuditManager.Instance.clearAuditListAndDeletedList();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the un alloc taxlot.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        internal TaxLot CreateUnAllocTaxlot(AllocationGroup group)
        {
            TaxLot unallocatedtaxlot = new TaxLot();
            try
            {
                unallocatedtaxlot = AllocationClientDataManager.GetInstance.CreateUnAllocatedTaxLot(group, group.GroupID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return unallocatedtaxlot;
        }

        /// <summary>
        /// Handles the DoWork event of the currentStateBackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void currentStateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<string> symbolList = e.Argument as List<string>;
                List<CurrentSymbolState> currentSymbolState = new List<CurrentSymbolState>();
                if (symbolList.Count > 0)
                    currentSymbolState = AllocationClientDataManager.GetInstance.GetCurrentStateForSymbol(symbolList, _userId);
                e.Result = currentSymbolState;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the currentStateBackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void currentStateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                    MessageBox.Show("Cancelled!", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK);
                else if (e.Error != null)
                    MessageBox.Show("Error: " + e.Error.Message, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK);
                else
                {
                    List<CurrentSymbolState> currentSymbolState = e.Result as List<CurrentSymbolState>;
                    if (UpdateStateOfSymbol != null)
                        UpdateStateOfSymbol(this, new EventArgs<List<CurrentSymbolState>>(currentSymbolState));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        /// <summary>
        /// Deletes the allocation group 
        /// </summary>
        /// <param name="deleteGroup">The delete group.</param>
        public void DeleteAllocationGroup(AllocationGroup deleteGroup)
        {
            try
            {
                //TODO: Use this userid from common global cache
                AllocationClientGroupCache.GetInstance.AddDictUnsavedCancelAmmend(deleteGroup.GroupID, (AllocationGroup)deleteGroup.Clone());
                if (string.IsNullOrEmpty(deleteGroup.ChangeComment))
                    AuditManager.Instance.AddGroupToAuditEntry(deleteGroup, true, TradeAuditActionType.ActionType.DELETE, deleteGroup.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeleted.ToString(), _userId);
                else
                    AuditManager.Instance.AddGroupToAuditEntry(deleteGroup, true, TradeAuditActionType.ActionType.DELETE, deleteGroup.Quantity.ToString(), "0", deleteGroup.ChangeComment, _userId);

                //delete status is already set in save groups
                //deleteGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                AddDeletedOmiitedGroups(deleteGroup);

                //We are removing group from unAllocated dictionary in save method
                //for Prana mode we have to remove group from unAllocated group cache so that group disappear from UI
                List<AllocationGroup> deleteGroups = new List<AllocationGroup>();
                deleteGroups.Add(deleteGroup);
                AllocationClientGroupCache.GetInstance.RemoveUnallocatedGroup(deleteGroups);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        internal void DeleteGroup(string groupId)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.DeleteGroup(groupId);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Dictionaries the unsaved add.
        /// </summary>
        /// <param name="GroupID">The group identifier.</param>
        /// <param name="ag">The ag.</param>
        public void DictUnsavedAdd(string GroupID, AllocationGroup ag)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.AddDictUnsavedCancelAmmend(GroupID, ag);
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
        /// Dictionaries the unsaved contains key.
        /// </summary>
        /// <param name="keyGroupId">The key group identifier.</param>
        /// <returns></returns>
        public bool DictUnsavedContainsKey(string keyGroupId)
        {
            try
            {
                return AllocationClientGroupCache.GetInstance.DictUnsavedContainsKey(keyGroupId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Handles the DoWork event of the fetchDataAsyc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void fetchDataAsyc_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var data = (object[])e.Argument;
                var toAllAUECDatesString = data[0].ToString();
                var fromAllAUECDatesString = data[1].ToString();
                var filterList = data[2] as AllocationPrefetchFilter;
                List<AllocationGroup> groups = AllocationClientDataManager.GetInstance.GetAllocationData(DateTime.Parse(toAllAUECDatesString), DateTime.Parse(fromAllAUECDatesString), filterList, _userId);
                e.Result = groups;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the fetchDataAsyc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void fetchDataAsyc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string message = string.Empty;
            try
            {
                var groups = e.Result as List<AllocationGroup>;
                if (groups == null)
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Nothing to load."));
                }
                else
                {
                    string statusBarMessage = groups.Count == 0 ? "Nothing to load." : "Trade(s)/group(s) loaded.";

                    List<AllocationGroup> permittedGroup = AddGroupsToUI(groups);
                    if (UpdateAllocationData != null)
                        UpdateAllocationData(this, new EventArgs<AllocationResponse, bool>(new AllocationResponse() { Response = statusBarMessage, GroupList = permittedGroup }, true));
                }
            }
            catch (Exception ex)
            {
                message = "Something went wrong. Please Get Data again.";
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                //TODO: Combine UpdateAllocationData and UpdateStatusBar events as they are updating status bar
                if (!string.IsNullOrWhiteSpace(message))
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>(message));
                }

                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(false));
            }
        }

        /// <summary>
        /// Gets the account pb details.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetAccountPBDetails()
        {
            Dictionary<int, string> accountDetails = new Dictionary<int, string>();
            try
            {
                accountDetails = AllocationClientDataManager.GetInstance.GetAccountPBDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountDetails;
        }

        /// <summary>
        ///     Used to get all the commissionRules from the server side,PRANA-13273
        /// </summary>
        /// <returns></returns>
        internal List<CommissionRule> GetAllCommissionRule()
        {
            var allCommissionRules = new List<CommissionRule>();
            try
            {
                allCommissionRules = AllocationClientDataManager.GetInstance.GetAllCommissionRules(_userId);
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
        /// Gets the allocated groups.
        /// </summary>
        /// <returns></returns>
        internal GenericBindingList<AllocationGroup> GetAllocatedGroups()
        {
            try
            {
                return AllocationClientGroupCache.GetInstance.GetAllocatedGroups();
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
        /// Gets the allocation data.
        /// </summary>
        /// <param name="toAllAUECDatesString">To all auec dates string.</param>
        /// <param name="fromAllAUECDatesString">From all auec dates string.</param>
        /// <param name="filterList">The filter list.</param>
        internal void GetAllocationData(string toAllAUECDatesString, string fromAllAUECDatesString, AllocationPrefetchFilter filterList)
        {
            try
            {
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(true));

                _filterList = filterList;

                var fetchDataAsyc = new BackgroundWorker();
                fetchDataAsyc.RunWorkerCompleted += fetchDataAsyc_RunWorkerCompleted;
                fetchDataAsyc.DoWork += fetchDataAsyc_DoWork;
                fetchDataAsyc.RunWorkerAsync(new object[] { toAllAUECDatesString, fromAllAUECDatesString, filterList });
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets the allocation preference.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal AllocationOperationPreference GetAllocationPreference(int prefId)
        {
            try
            {
                return AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(prefId);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the master fund preference.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal AllocationMasterFundPreference GetMasterFundPreference(int prefId)
        {
            try
            {
                return AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(prefId);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the attribute list.
        /// </summary>
        /// <returns></returns>
        public List<string>[] GetAttributeList()
        {
            try
            {
                return AllocationClientDataManager.GetInstance.GetTradeAttributes(_userId);
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
        /// Gets the default rules.
        /// </summary>
        /// <returns>The allocation preferences</returns>
        internal AllocationCompanyWisePref GetCompanyWisePreferences()
        {
            try
            {
                return AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences();
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
        /// Gets the current state for symbolList.
        /// </summary>
        /// <param name="symbolList">The symbolList.</param>
        internal void GetCurrentStateForSymbol(List<string> symbolList)
        {
            try
            {
                BackgroundWorker currentStateBackgroundWorker = new BackgroundWorker();
                currentStateBackgroundWorker.DoWork += new DoWorkEventHandler(currentStateBackgroundWorker_DoWork);
                currentStateBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(currentStateBackgroundWorker_RunWorkerCompleted);
                currentStateBackgroundWorker.RunWorkerAsync(symbolList);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets the group status for group list.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        internal Dictionary<string, PostTradeEnums.Status> GetGroupStatus(List<AllocationGroup> groups)
        {
            Dictionary<string, PostTradeEnums.Status> statusDictionary = new Dictionary<string, PostTradeEnums.Status>();
            try
            {
                statusDictionary = AllocationClientDataManager.GetInstance.GetGroupStatus(groups);
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
        /// Gets the groups for bulk change.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private List<AllocationGroup> GetGroupsToUpdate(List<AllocationGroup> groups, bool allowExerciseGroups)
        {
            List<AllocationGroup> updateGroupList = new List<AllocationGroup>();
            try
            {
                StringBuilder groupsNotUpdated = new StringBuilder();

                Dictionary<string, PostTradeEnums.Status> groupStatusDictionary = GetGroupStatus(groups);
                List<string> exercisedGroupIds = new List<string>();
                if (!allowExerciseGroups)
                    exercisedGroupIds = groupStatusDictionary.Where(x => x.Value.Equals(PostTradeEnums.Status.Exercise) || x.Value.Equals(PostTradeEnums.Status.ExerciseAssignManually)).Select(y => y.Key).ToList();
                IEnumerable<string> exercisedOffsetGroupIds = groups.Where(x => x.TransactionType.Equals(TradingTransactionType.Exercise.ToString())
                    || x.TransactionType.Equals(TradingTransactionType.Assignment.ToString())).Select(x => x.GroupID);
                exercisedGroupIds.AddRange(exercisedOffsetGroupIds.Where(x => groupStatusDictionary.ContainsKey(x) && groupStatusDictionary[x] == PostTradeEnums.Status.Closed));

                List<string> corporateActionGroupIds = groupStatusDictionary.Where(x => x.Value.Equals(PostTradeEnums.Status.CorporateAction)).Select(y => y.Key).ToList();
                if (exercisedGroupIds.Count > 0)
                {
                    groupsNotUpdated.Append("GroupID(s) : ");
                    groupsNotUpdated.Append(string.Join(",", exercisedGroupIds.ToArray()));
                    groupsNotUpdated.Append(" is/are generated by Exercise so cannot be modified.");
                    groupsNotUpdated.Append(Environment.NewLine);
                }
                if (corporateActionGroupIds.Count > 0)
                {
                    groupsNotUpdated.Append("GroupID(s) : ");
                    groupsNotUpdated.Append(string.Join(",", corporateActionGroupIds.ToArray()));
                    groupsNotUpdated.Append(" Corporate Action applied so cannot be modified.");
                    groupsNotUpdated.Append(Environment.NewLine);
                }

                groups.Where(x => !(exercisedGroupIds.Contains(x.GroupID)) && !(corporateActionGroupIds.Contains(x.GroupID))).ToList().ForEach(group =>
                {
                    DictUnsavedAdd(group.GroupID, (AllocationGroup)group.Clone());
                    group.UpdateGroupPersistenceStatus();
                    group.IsModified = true;
                    group.IsAnotherTaxlotAttributesUpdated = true;
                    updateGroupList.Add(group);
                });

                if (groupsNotUpdated.Length > 0)
                    CommonAllocationMethods.ShowUnmodifiedGroupsDetails(groupsNotUpdated);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updateGroupList;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static AllocationClientManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new AllocationClientManager();
                    }
                }
            }
            return _singleton;
        }

        /// <summary>
        /// Gets the preferences list after publishing of preference
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetOperationPreferencesList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                List<AllocationOperationPreference> allocationOperationpreferenceList = AllocationClientDataManager.GetInstance.GetPreferenceByCompanyId(CachedDataManager.GetInstance.GetCompanyID(), _userId);
                foreach (AllocationOperationPreference pref in allocationOperationpreferenceList)
                {
                    if (pref.DefaultRule.RuleType == MatchingRuleType.Leveling &&
                           !AllocationSubModulePermission.IsLevelingPermitted)
                        continue;
                    else if (pref.DefaultRule.RuleType == MatchingRuleType.ProrataByNAV &&
                       !AllocationSubModulePermission.IsProrataByNavPermitted)
                        continue;

                    if (!pref.OperationPreferenceName.StartsWith("*Custom#_") && !pref.OperationPreferenceName.StartsWith("*WorkArea#_") && !pref.OperationPreferenceName.StartsWith("*PTT#_"))
                        preferenceList.Add(pref.OperationPreferenceId, pref.OperationPreferenceName);
                }
                preferenceList = AllocationClientPreferenceManager.GetInstance.UpdateSorting(preferenceList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        internal Dictionary<int, string> GetMasterFundPreferenceList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                List<AllocationMasterFundPreference> allocationMasterFundPreferenceList = AllocationClientDataManager.GetInstance.GetMasterFundPreferenceByCompanyId(CachedDataManager.GetInstance.GetCompanyID(), _userId);
                foreach (AllocationMasterFundPreference pref in allocationMasterFundPreferenceList)
                {
                    preferenceList.Add(pref.MasterFundPreferenceId, pref.MasterFundPreferenceName);
                }
                preferenceList = AllocationClientPreferenceManager.GetInstance.UpdateSorting(preferenceList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        /// <summary>
        /// Gets the PTT allocation preference.
        /// </summary>
        /// <param name="pttAllocationPrefernceId">The PTT allocation preference identifier.</param>
        /// <returns></returns>
        internal AllocationOperationPreference GetPTTAllocationPreference(int pttAllocationPrefernceId)
        {
            AllocationOperationPreference pref = new AllocationOperationPreference();
            try
            {
                pref = AllocationClientDataManager.GetInstance.GetPTTAllocationPreference(pttAllocationPrefernceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return pref;
        }

        /// <summary>
        /// Gets the taxlot details.
        /// </summary>
        /// <param name="taxlotId">The taxlot identifier.</param>
        /// <returns></returns>
        internal DataTable GetTaxlotDetails(string taxlotId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = AllocationClientDataManager.GetInstance.GetTaxlotDetailsUpdateExternalTransaction(taxlotId);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataTable;
        }

        /// <summary>
        /// Gets the unallocated groups.
        /// </summary>
        /// <returns></returns>
        internal GenericBindingList<AllocationGroup> GetUnallocatedGroups()
        {
            try
            {
                return AllocationClientGroupCache.GetInstance.GetUnallocatedGroups();
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
        /// Gets the allocation preferences.
        /// </summary>
        /// <returns></returns>
        internal AllocationPreferences GetUserWisePreferences()
        {
            try
            {
                return AllocationClientPreferenceManager.GetInstance.GetUserWisePreferences();
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
        /// Determines whether [is group deleted] [the specified group identifier].
        /// </summary>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        internal bool IsGroupDeleted(string groupID)
        {
            try
            {
                return AllocationClientGroupCache.GetInstance.IsGroupDeleted(groupID);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is groups dirty] [the specified group].
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        internal bool IsGroupsDirty(AllocationGroup group)
        {
            try
            {
                lock (_lockerAllocationSave)
                {
                    return AllocationClientGroupCache.GetInstance.IsGroupsDirty(group);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return false;
        }

        /// <summary>
        /// Launches the closing wizard.
        /// </summary>
        internal void LaunchClosingWizard()
        {
            try
            {
                ClosingWizardHelper.GetInstance().ClosingServices = AllocationClientServiceConnector.ClosingServices;
                ClosingWizardHelper.GetInstance().LaunchClosingWizard();
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
        /// Makes the proxy.
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                List<FilterData> filter = new List<FilterData>();
                FilterDataForSameUsers filterData = new FilterDataForSameUsers();
                filterData.UserId = _userId;
                filter.Add(filterData);
                _proxy.Subscribe(Topics.Topic_CreateGroup, filter);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                _proxy.Subscribe(Topics.Topic_Closing, null);
                _proxy.Subscribe(Topics.Topic_AllocationPreferenceUpdated, null);
                _proxy.Subscribe(Topics.Topic_AllocationSchemeUpdated, null);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Previews the allocation.
        /// </summary>
        /// <param name="allocationGroupList">The allocation group list.</param>
        /// <param name="pref">The preference.</param>
        /// <param name="isReallocate">if set to <c>true</c> [is reallocate].</param>
        /// <param name="isChanged">if set to <c>true</c> [is changed].</param>
        /// <param name="forceAllocation">if set to <c>true</c> [force allocation].</param>
        /// <returns></returns>
        public AllocationResponse PreviewAllocation(List<AllocationGroup> allocationGroupList, AllocationOperationPreference pref, bool isReallocate, bool isChanged, bool forceAllocation)
        {
            try
            {
                var response = new AllocationResponse();
                if (isReallocate)
                {
                    foreach (var group in allocationGroupList)
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, TradeAuditActionType.ActionType.REALLOCATE, "", "", "Trade Reallocated Taxlots Deleted", _userId);
                    var rule = new AllocationRule
                    {
                        BaseType = AllocationBaseType.CumQuantity,
                        RuleType = MatchingRuleType.None,
                        PreferenceAccountId = -1,
                        MatchClosingTransaction = MatchClosingTransactionType.None
                    };
                    response = AllocationClientDataManager.GetInstance.AllocateByParameter(allocationGroupList, new AllocationParameter(rule, pref.TargetPercentage, -1, _userId, true, true), forceAllocation);
                }
                else
                {
                    if (isChanged)
                        response = AllocationClientDataManager.GetInstance.AllocateByParameter(allocationGroupList, new AllocationParameter(pref.DefaultRule, pref.TargetPercentage, -1, _userId, true, true), forceAllocation);
                    else
                        response = AllocationClientDataManager.GetInstance.AllocateByPreference(allocationGroupList, pref.OperationPreferenceId, _userId, true, forceAllocation);
                }
                return response;
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
        /// Previews the allocation data.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <param name="pref">The preference.</param>
        /// <param name="isForceAllocationSelected">if set to <c>true</c> [is force allocation selected].</param>
        /// <param name="isChanged">if set to <c>true</c> [is changed].</param>
        /// <returns></returns>
        internal AllocationResponse PreviewAllocationData(List<AllocationGroup> allocationGroup, AllocationOperationPreference pref, bool isForceAllocationSelected, bool isChanged)
        {
            try
            {
                AllocationResponse response = PreviewAllocation(allocationGroup, pref, false, isChanged, isForceAllocationSelected);
                return response;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Res the calculate commissions.
        /// </summary>
        /// <param name="commissionRule">The commission rule.</param>
        /// <param name="groups">The groups.</param>
        /// <param name="isGroupWise">if set to <c>true</c> [is group wise].</param>
        /// <returns></returns>
        internal string ReCalculateCommissions(CommissionRule commissionRule, List<AllocationGroup> groups, bool isGroupWise)
        {
            string response = string.Empty;
            try
            {
                if (groups.Count > 0)
                {
                    List<AllocationGroup> updateGroupList = GetGroupsToUpdate(groups, true);
                    List<AllocationGroup> updateGroups = AllocationClientDataManager.GetInstance.ApplyCommissionBulkChange(commissionRule, updateGroupList, isGroupWise);
                    if (updateGroups != null)
                        AllocationClientGroupCache.GetInstance.AddUpdateGroups(updateGroups);
                    response = "Commission and Fee calculated for the selected record(s).";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                response = "Something went wrong, bulk changes on commission have not been applied";
            }
            return response;
        }

        /// <summary>
        /// Saves the allocation scheme.
        /// </summary>
        /// <param name="prorataDate">The prorata date.</param>
        internal void SaveAllocationScheme(string schemeName, DateTime prorataDate, string schemeBasis)
        {
            try
            {
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>("Prorata Calculation Started..."));
                List<object> arguments = new List<object>();
                arguments.Add(schemeName);
                arguments.Add(prorataDate);
                arguments.Add(schemeBasis);
                BackgroundWorker saveAllocSchemeAsyncWorker = new BackgroundWorker();
                saveAllocSchemeAsyncWorker.DoWork += new DoWorkEventHandler(saveAllocSchemeAsyncWorker_DoWork);
                saveAllocSchemeAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveAllocSchemeAsyncWorker_RunWorkerCompleted);
                saveAllocSchemeAsyncWorker.RunWorkerAsync(arguments);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the DoWork event of the saveAllocSchemeAsyncWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        void saveAllocSchemeAsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object> args = e.Argument as List<object>;
                string schemeName = args[0].ToString();
                DateTime prorataDate = Convert.ToDateTime(args[1]);
                string schemeBasis = args[2].ToString();
                AllocationCompanyWisePref pref = GetCompanyWisePreferences();
                //Get position based on IsMasterFundRatioAllocation enabled or not.
                bool isMFRatioSchemEnabled = pref.EnableMasterFundAllocation && pref.IsOneSymbolOneMasterFundAllocation;
                int allocationSchemeID = AllocationSchemeImportHelper.SaveAllocationSchemeFromApp(schemeName, prorataDate, schemeBasis, isMFRatioSchemEnabled);
                e.Result = allocationSchemeID;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the saveAllocSchemeAsyncWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        void saveAllocSchemeAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (e.Cancelled)
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Allocation Prorata calculation cancelled"));
                }
                else if (e.Error != null)
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Error: " + e.Error.Message));
                }

                if (Convert.ToInt32(e.Result) > 0)
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Allocation Prorata % calculation completed"));
                }
                else
                {
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string>("Allocation Prorata % calculation failed"));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="actionAfterSaving">The action after saving.</param>
        /// <param name="isSaveState">if set to <c>true</c> [is save state].</param>
        internal void SaveDataAsync(ActionAfterSavingData actionAfterSaving, bool isSaveState)
        {
            try
            {
                object[] actionHandler = new object[] { (object)actionAfterSaving, (object)isSaveState, (object)true };

                if (actionAfterSaving == ActionAfterSavingData.CloseAllocation)
                    UnSubscribeProxy();

                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(true));

                BackgroundWorker saveDataAsyncWorker = new BackgroundWorker();
                saveDataAsyncWorker.DoWork += new DoWorkEventHandler(saveDataAsync_DoWork);
                saveDataAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveDataAsync_RunWorkerCompleted);
                saveDataAsyncWorker.RunWorkerAsync(actionHandler);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the saveDataAsync control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        internal void saveDataAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] inputParameters = e.Argument as object[];
                bool isSaveState = (bool)inputParameters[1];
                bool isComingForReallocation = (bool)inputParameters[2];
                bool isDataChangedAtServerSide = false;
                bool isNAVLocked = false;
                List<AllocationGroup> responseGroups = SaveGroups(ref isDataChangedAtServerSide, ref isNAVLocked, isSaveState, isComingForReallocation);
                inputParameters[0] = ActionAfterSavingData.GetData;
                object[] parameters = new object[] { inputParameters, (object)responseGroups, isDataChangedAtServerSide };
                e.Result = parameters;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the saveDataAsync control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        internal void saveDataAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int rowsAffected = 0;
            StringBuilder statusBarMessage = new StringBuilder(string.Empty);
            bool isDataChangedAtServerSide = false;
            try
            {
                if (e.Cancelled)
                    MessageBox.Show("Cancelled!", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK);
                else if (e.Error != null)
                    MessageBox.Show("Error: " + e.Error.Message, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK);

                else
                {
                    object[] parameters = e.Result as object[];
                    List<AllocationGroup> groups = ((List<AllocationGroup>)parameters[1]);
                    isDataChangedAtServerSide = (bool)parameters[2];
                    rowsAffected = groups.Count;
                    AllocationClientGroupCache.GetInstance.ClearDictUnsavedCancelAmend();
                    if (rowsAffected < 0)
                    {
                        if (AllocationDataChange != null)
                            AllocationDataChange(this, new EventArgs<bool>(true));
                        return;
                    }
                    statusBarMessage.Append(rowsAffected > 0 ? "Allocation data saved." : "Nothing to Save.");

                    object[] arguments = parameters[0] as object[];
                    ActionAfterSavingData saveDataAction = (ActionAfterSavingData)arguments[0];
                    switch (saveDataAction)
                    {
                        case ActionAfterSavingData.GetData:
                            if (ActionAfterSave != null)
                                ActionAfterSave(this, new EventArgs<ActionAfterSavingData>(ActionAfterSavingData.GetData));
                            break;
                        case ActionAfterSavingData.ClearData:
                            ClearData();
                            break;
                        case ActionAfterSavingData.CancelEditChanges:
                            CancelEditChanges();
                            break;
                        case ActionAfterSavingData.CloseAllocation:
                            if (ActionAfterSave != null)
                                ActionAfterSave(this, new EventArgs<ActionAfterSavingData>(ActionAfterSavingData.CloseAllocation));
                            break;
                    }
                    if (NewGroupReceived != null)
                        NewGroupReceived(this, new EventArgs<List<AllocationGroup>>(groups));

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                if (rowsAffected >= 0)
                {
                    if (AllocationDataChange != null)
                        AllocationDataChange(this, new EventArgs<bool>(false));

                    if (AllocationDataSaved != null)
                        AllocationDataSaved(this, EventArgs.Empty);
                }
                if (!isDataChangedAtServerSide && UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>(statusBarMessage.ToString()));
            }
        }

        /// <summary>
        /// Saves the entry in Audit Trail for any changes done in the Group
        /// </summary>
        /// <param name="dictUnsavedAuditLists">The dictionary unsaved audit lists.</param>
        public void SaveEditedGroupsAuditEntry(Dictionary<string, AllocationGroup> dictUnsavedAuditLists)
        {
            try
            {
                foreach (KeyValuePair<string, AllocationGroup> kvp in dictUnsavedAuditLists)
                {
                    AllocationGroup agOriginal = null;
                    AllocationGroup newVal = null;
                    newVal = AllocationClientGroupCache.GetInstance.GetGroup(kvp.Key);
                    if (AllocationClientGroupCache.GetInstance.DictUnsavedContainsKey(kvp.Key))
                    {
                        agOriginal = AllocationClientGroupCache.GetInstance.GetDictUnsavedKeyValue(kvp.Key);
                    }
                    else
                    {
                        Logger.LoggerWrite("Error in saving Audit Trail while saving data.");
                        continue;
                    }
                    foreach (TradeAuditActionType.ActionType action in kvp.Value.TradeActionsList)
                    {
                        string originalValue = string.Empty;
                        string newValue = string.Empty;
                        if (action.Equals(Prana.BusinessObjects.TradeAuditActionType.ActionType.SettlCurrency_Changed))
                        {
                            originalValue = CachedDataManager.GetInstance.GetCurrencyText(agOriginal.SettlementCurrencyID);
                            newValue = CachedDataManager.GetInstance.GetCurrencyText(newVal.SettlementCurrencyID);
                        }
                        else if (action.Equals(Prana.BusinessObjects.TradeAuditActionType.ActionType.BookAsSwap))
                        {
                            originalValue = TradeAuditActionType.GetColumnValue(action, agOriginal);
                            newValue = DerivedAssetCategory.EquitySwap.ToString();
                        }
                        else
                        {
                            originalValue = TradeAuditActionType.GetColumnValue(action, agOriginal);
                            newValue = TradeAuditActionType.GetColumnValue(action, newVal);
                        }

                        AuditManager.Instance.AddGroupToAuditEntry(kvp.Value, false, action, originalValue, newValue, kvp.Value.ChangeComment, _userId);
                    }
                    foreach (TaxLot tax in kvp.Value.TaxLots)
                    {
                        if (tax.TradeActionsList.Count > 0)
                        {
                            foreach (TaxLot taxOriginal in agOriginal.TaxLots)
                            {
                                if (taxOriginal.TaxLotID == tax.TaxLotID)
                                {
                                    foreach (TradeAuditActionType.ActionType action in tax.TradeActionsList)
                                    {
                                        string taxOriginalValue = string.Empty;
                                        string taxNewValue = string.Empty;
                                        if (action.Equals(Prana.BusinessObjects.TradeAuditActionType.ActionType.SettlCurrency_Changed))
                                        {
                                            taxOriginalValue = CachedDataManager.GetInstance.GetCurrencyText(taxOriginal.SettlementCurrencyID);
                                            taxNewValue = CachedDataManager.GetInstance.GetCurrencyText(newVal.SettlementCurrencyID);
                                        }
                                        else if (action.Equals(Prana.BusinessObjects.TradeAuditActionType.ActionType.BookAsSwap))
                                        {
                                            taxOriginalValue = agOriginal.AssetName.ToString();
                                            taxNewValue = DerivedAssetCategory.EquitySwap.ToString();
                                        }
                                        else
                                        {
                                            taxOriginalValue = TradeAuditActionType.GetColumnValue(action, taxOriginal);
                                            taxNewValue = TradeAuditActionType.GetColumnValue(action, newVal);
                                        }

                                        AuditManager.Instance.AddRowTradeAuditEntry(tax, kvp.Key, action, taxOriginalValue, taxNewValue, _userId);
                                    }
                                }
                            }
                        }
                        tax.TradeActionsList.Clear();
                    }
                    kvp.Value.TradeActionsList.Clear();
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
        /// Saves the groups.
        /// </summary>
        /// <param name="isSaveState">if set to <c>true</c> [is save state].</param>
        /// <returns></returns>
        public List<AllocationGroup> SaveGroups(ref bool isDataChangedAtServerSide,ref bool isNAVLocked, bool isSaveState = true, bool isComingForReallocation = false)
        {
            List<AllocationGroup> lstDirtyGroups = new List<AllocationGroup>();
            // int rowsAffected = 0;
            List<AllocationGroup> responseGroups = new List<AllocationGroup>();
            var dictUnsavedAuditList = new Dictionary<string, AllocationGroup>();
            try
            {
                List<string> taxlotsList = new List<string>();
                List<AllocationGroup> groups = new List<AllocationGroup>();
                lock (_lockerAllocationSave)
                {
                    AllocationClientGroupCache.GetInstance.UpdateAndGetGroupsForSave(lstDirtyGroups, groups);
                }
                var lstAccountLockNotAcquired = new List<string>();
                if (groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        #region check for account lock while saving data

                        foreach (var tx in group.TaxLots)
                        {
                            if (!string.IsNullOrWhiteSpace(tx.Level1Name))
                            {
                                if (!CachedDataManager.GetInstance.isAccountLocked(tx.Level1ID))
                                {
                                    lstAccountLockNotAcquired.Add(tx.Level1Name);
                                }
                            }
                        }

                        #endregion

                        if (!string.IsNullOrWhiteSpace(group.GroupID))
                        {
                            if (!dictUnsavedAuditList.ContainsKey(group.GroupID))
                            {
                                if (group.TradeActionsList.Count > 0)
                                {
                                    dictUnsavedAuditList.Add(group.GroupID, group);
                                }
                                else
                                {
                                    foreach (var tx in group.TaxLots)
                                    {
                                        if (tx.TradeActionsList.Count > 0)
                                        {
                                            dictUnsavedAuditList.Add(group.GroupID, group);
                                            break;
                                        }
                                    }
                                }
                                taxlotsList.AddRange(group.GetAllTaxlotIDs());
                            }
                        }
                        //set the value of ismanuallyModified for all groups which are grouped, ungrouped or edited.
                        group.IsManuallyModified = true;
                    }

                    //todo: from list pull distinct accountid
                    if (lstAccountLockNotAcquired.Count > 0)
                    {
                        MessageBox.Show("Account lock not acquired for account(s) " + String.Join(",", lstAccountLockNotAcquired.ToArray()) + " ,please acquire account lock before proceeding.", "Account Lock", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return null;
                    }
                    DateTime lowestDate = groups.Min(g => g.AUECLocalDate);
                    if(!CachedDataManager.GetInstance.ValidateNAVLockDate(lowestDate))
                    {
                        MessageBox.Show("The date for some of the data you’ve chosen, precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButton.OK, MessageBoxImage.Information);
                        isNAVLocked = true;
                        return responseGroups;
                    }
                    if (ValidateTradeForAccountNAVLock(groups))
                    {
                        SaveEditedGroupsAuditEntry(dictUnsavedAuditList);
                        AuditManager.Instance.SaveAuditList();
                        if (isSaveState == false)
                            foreach (var group in groups)
                                group.IsAnotherTaxlotAttributesUpdated = false;

                        responseGroups = AllocationClientDataManager.GetInstance.SaveAllocationGroups(groups, _userId, isComingForReallocation);
                        if (responseGroups == null || responseGroups.Count == 0)
                        {
                            if (GroupChangedAtServerSide != null)
                            {
                                isDataChangedAtServerSide = true;
                                if (GroupChangedAtServerSide != null)
                                    GroupChangedAtServerSide(this, new EventArgs<string>("Groups Changed at server side."));
                            }
                        }
                        SetDefaultPersistenceStatusForCachedgroups();
                        AllocationClientGroupCache.GetInstance.ClearExercisedGroupsDictionary();
                    }
                }
                if (lstDirtyGroups.Count > 0)
                {
                    foreach (var dirtyGroup in lstDirtyGroups)
                    {
                        AllocationClientGroupCache.GetInstance.SetDefaultPersistenceStatus(dirtyGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

            return responseGroups;
        }

        /// <summary>
        /// Saves the layout.
        /// </summary>
        /// <param name="allocationUserWisePref">The allocation user wise preference.</param>
        internal void SaveLayout(AllocationPreferences allocationUserWisePref)
        {
            try
            {
                AllocationClientPreferenceManager.GetInstance.SaveAllocationUserWisePreferences(allocationUserWisePref);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default persistence status.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void SetDefaultPersistenceStatus(AllocationGroup group)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.SetDefaultPersistenceStatus(group);
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
        /// Sets the default persistence status for cachedgroups.
        /// </summary>
        private void SetDefaultPersistenceStatusForCachedgroups()
        {
            try
            {
                lock (_lockerAllocationSave)
                {
                    AllocationClientGroupCache.GetInstance.SetDefaultPersistenceStatusForGroups();
                    AllocationClientGroupCache.GetInstance.ClearDeletedGroups();
                    AllocationClientGroupCache.GetInstance.ClearDeletedOmittedGroups();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Starts a background worker to unallocate data. disables grids and button which will automatically be enabled on completed event.
        /// It also uses beginUpdate and endupdate methods of grid to change binding collection.
        /// </summary>
        /// <param name="allocationGroups">List of allocation group which is to be unallocated</param>
        internal void UnAllocateGroupAsync(List<AllocationGroup> allocationGroups)
        {
            try
            {
                BackgroundWorker bgUnAllocateData = new BackgroundWorker();
                bgUnAllocateData.DoWork += new DoWorkEventHandler(bgUnAllocateData_DoWork);
                bgUnAllocateData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgUnAllocateData_RunWorkerCompleted);
                bgUnAllocateData.RunWorkerAsync(allocationGroups);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ungroup completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Exception"></exception>
        public void UnGroupCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string message = string.Empty;
            try
            {
                if (e.Error == null)
                {
                    object[] groupingResult = e.Result as object[];
                    UpdateGroupingData(groupingResult);

                    message = groupingResult[0].ToString();
                }
                else
                {
                    message = "Something went wrong. Ungrouping is not done correctly.";
                    bool rethrow = Logger.HandleException(e.Error, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw new Exception(e.Error.Message);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string>(message));
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(false));
            }
        }

        /// <summary>
        /// Ungroup started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void UnGroupStarted(object sender, DoWorkEventArgs e)
        {
            try
            {
                //String result = null;
                List<AllocationGroup> groups = e.Argument as List<AllocationGroup>;
                if (groups != null)
                {
                    e.Result = AllocationManualGroupingHelper.UngroupData(groups, _userId);
                }
                //  e.Result = result;
                //if (UpdateStatusBar != null)
                //{
                //    UpdateStatusBar(this, new EventArgs<string>(e.Result.ToString()));
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Uns the subscribe proxy.
        /// </summary>
        internal void UnSubscribeProxy()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroup);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_AllocationPreferenceUpdated);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_AllocationSchemeUpdated);
                    _proxy.Dispose();
                    _proxy = null;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the bulk change groups.
        /// </summary>
        /// <param name="BulkChangeGroups">The bulk change groups.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        internal string UpdateBulkChangeGroups(BulkChangesGroupLevel bulkChangeGroups, List<AllocationGroup> groups)
        {
            string response = string.Empty;
            try
            {
                bool isExerciseGroupsAllowed = BulkChangeHelper.AllowBulkChangeForExerciseGroups(bulkChangeGroups);
                List<AllocationGroup> updateGroupList = GetGroupsToUpdate(groups, isExerciseGroupsAllowed);
                if (updateGroupList.Count > 0)
                {
                    BulkChangeHelper.UpdateBulkChangeGroups(bulkChangeGroups, updateGroupList);
                    updateGroupList.ForEach(x => x.PropertyHasChanged());
                    response = "Bulk changes updated for the selected record(s).";
                }
                else
                    response = "No valid record(s) for the required change(s).";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                response = "Something went wrong, bulk changes have not been applied";
            }
            return response;
        }

        /// <summary>
        /// Updates the group closing status.
        /// </summary>
        /// <param name="taxlotsList">The taxlots list.</param>
        internal void UpdateGroupClosingStatus(List<TaxLot> taxlotsList)
        {
            try
            {
                bool isDirtyData = AllocationClientGroupCache.GetInstance.UpdateGroupClosingStatus(taxlotsList);
                if (isDirtyData && AllocationDataChanged != null)
                {
                    AllocationDataChanged(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates the grouping data.
        /// </summary>
        /// <param name="groupingResult">The grouping result.</param>
        private static void UpdateGroupingData(object[] groupingResult)
        {
            try
            {
                if (groupingResult[1] != null)
                {
                    foreach (AllocationGroup group in (List<AllocationGroup>)groupingResult[1])
                    {
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;

                        if (!AllocationClientGroupCache.GetInstance.CheckDuplicate(group))
                            AllocationClientGroupCache.GetInstance.AddDeletedGroup(group);
                        AllocationClientGroupCache.GetInstance.DeleteGroup(group.GroupID);
                        AllocationClientGroupCache.GetInstance.RemoveDictGroup(group.GroupID);

                        // AllocationClientGroupCache.GetInstance.RemoveDictGroup(group.GroupID);
                        group.ResetTaxlotDictionary(group.TaxLots);
                    }
                }
                if (groupingResult[2] != null)
                {
                    foreach (AllocationGroup newGroup in (List<AllocationGroup>)groupingResult[2])
                    {
                        AllocationClientGroupCache.GetInstance.AddGroup(newGroup);
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
        /// Updates the repository with sec master information.
        /// </summary>
        /// <param name="list">The list.</param>
        private void UpdateRepositoryWithSecMasterInfo(SecMasterbaseList list)
        {
            try
            {
                AllocationClientGroupCache.GetInstance.UpdateRepositoryWithSecMasterInfo(list);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the trade attribute bulk changes.
        /// </summary>
        /// <param name="isGroupLevel">if set to <c>true</c> [is group level].</param>
        /// <param name="tradeAttributeGroups">The trade attribute groups.</param>
        /// <param name="accountsNames">The accounts names.</param>
        /// <param name="allocatedGroups">The allocated groups.</param>
        /// <param name="unallocatedGroups">The unallocated groups.</param>
        /// <returns></returns>
        internal string UpdateTradeAttributeBulkChanges(bool isGroupLevel, TradeAttributes tradeAttributeGroups, List<AllocationGroup> groups, List<int> accountIDs)
        {
            string result = string.Empty;
            try
            {
                List<AllocationGroup> updateGroupList = GetGroupsToUpdate(groups, true);
                if (updateGroupList.Count > 0)
                {
                    if (isGroupLevel)
                    {
                        BulkChangeHelper.UpdateTradeAttributeGroups(tradeAttributeGroups, updateGroupList);
                    }
                    else
                    {
                        BulkChangeHelper.UpdateTradeAttributeTaxlotLevels(accountIDs, updateGroupList, tradeAttributeGroups);
                    }
                    updateGroupList.ForEach(x => x.PropertyHasChanged());
                    result = "Bulk Trade Attributes changes updated for the selected record(s).";
                }
                else
                    result = "No valid record(s) for the required change(s).";
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
        ///     Validate Trade For Account NAV Lock on saving trade after making changes
        ///     Created By: Omshiv, 15 may 2014
        /// </summary>
        /// <returns></returns>
        private bool ValidateTradeForAccountNAVLock(List<AllocationGroup> groups)
        {
            try
            {
                #region NAV lock validation - modified by Omshiv, MArch 2014

                //get IsNAVLockingEnabled or not from cache
                // Modifed By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3588
                //if (_releaseType == PranaReleaseViewType.CHMiddleWare)
                //{
                var isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (var allocationGrp in groups)
                    {
                        //AllocationGroup allocationGrp = row.ListObject as AllocationGroup;
                        if (allocationGrp != null &&
                            allocationGrp.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            foreach (var taxlot in allocationGrp.TaxLots)
                            {
                                //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                                //if (taxlot.Level1ID != null && taxlot.Level1ID != 0) // commented old if as int is never null
                                if (taxlot.Level1ID != 0)
                                {
                                    var tradeDate = taxlot.OriginalPurchaseDate;
                                    var isProcessToSave = NAVLockManager.GetInstance.ValidateTrade(taxlot.Level1ID, tradeDate);
                                    if (!isProcessToSave)
                                    {
                                        return isProcessToSave;
                                    }
                                }
                            }
                        }
                    }
                }

                //}

                #endregion
            }

            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// ReCalculates the other fee.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        internal AllocationGroup ReCalculateOtherFeeForGroup(AllocationGroup group, List<OtherFeeType> listofFeesToApply)
        {
            AllocationGroup allocationGroup = new AllocationGroup();
            try
            {
                allocationGroup = AllocationClientDataManager.GetInstance.ReCalculateOtherFeeForGroup(group, listofFeesToApply);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroup;
        }

        #endregion Methods

        #region IPublishing Members

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "AllocationForm";
        }

        /// <summary>
        /// Publishes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="topicName">Name of the topic.</param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (_dispatcher.CheckAccess())
                {
                    object[] dataList = (object[])e.EventData;
                    switch (e.TopicName)
                    {
                        case Topics.Topic_CreateGroup:
                            List<AllocationGroup> responseGroups = dataList.Where(x => x is AllocationGroup).Select(y => (y as AllocationGroup)).ToList();
                            if (NewGroupReceived != null)
                                NewGroupReceived(this, new EventArgs<List<AllocationGroup>>(responseGroups));
                            break;

                        case Topics.Topic_SecurityMaster:
                            SecMasterbaseList list = new SecMasterbaseList();
                            foreach (Object obj in dataList)
                            {
                                SecMasterBaseObj secMasterObj = (SecMasterBaseObj)obj;
                                list.Add(secMasterObj);
                            }
                            UpdateRepositoryWithSecMasterInfo(list);
                            break;

                        case Topics.Topic_Closing:
                            List<TaxLot> taxlotsList = dataList.Where(x => x is TaxLot).Select(y => (y as TaxLot)).ToList();
                            if (taxlotsList != null)
                                UpdateGroupClosingStatus(taxlotsList);
                            break;

                        case Topics.Topic_AllocationPreferenceUpdated:
                            AllocationCompanyWisePreferenceCache.GetInstance.UpdateCompanyWisePreferences();
                            if (AllocationPreferenceUpdated != null)
                                AllocationPreferenceUpdated(e.EventData, EventArgs.Empty);
                            break;

                        case Topics.Topic_AllocationSchemeUpdated:
                            AllocationClientPreferenceManager.GetInstance.UpdateFixedPreferencesOnImport();
                            if (AllocationSchemeUpdated != null)
                                AllocationSchemeUpdated(e.EventData, EventArgs.Empty);
                            break;                      
                    }
                }
                else
                    _dispatcher.Invoke(() => { Publish(e, topicName); });
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    UnSubscribeProxy();

                    if (_singleton != null)
                        _singleton = null;

                    //UnWire Events
                    if (AllocationClientPreferenceManager.GetInstance != null)
                    {
                        AllocationClientPreferenceManager.GetInstance.AllocationPreferencesSaved -= AllocationClientManagerAllocationPreferencesSaved;
                        AllocationClientPreferenceManager.GetInstance.Dispose();
                    }
                    //Dispose Group Cache
                    if (AllocationClientGroupCache.GetInstance != null)
                        AllocationClientGroupCache.GetInstance.Dispose();

                    //Unwire Event of AllocationClientServiceConnector
                    AllocationClientServiceConnector.ServerProxyConnectedDisconnectedEvent -= AllocationClientServiceConnector_ServerProxyConnectedDisconnectedEvent;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
