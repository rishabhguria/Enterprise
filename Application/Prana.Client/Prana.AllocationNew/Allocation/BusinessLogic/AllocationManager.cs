using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using Prana.PostTrade;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.Utilities;
using Prana.WCFConnectionMgr;
using Prana.CommonDataCache;
using Prana.BusinessObjects.Constants;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessLogic;
using Prana.BusinessObjects.AppConstants;
using System.Data;
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Enums;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using System.Threading;

namespace Prana.AllocationNew
{
    public class AllocationManager : IPublishing, IDisposable
    {
        private PranaReleaseViewType _releaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
        static ProxyBase<IPranaPositionServices> _pranaPositionServices = null;

        /// <summary>
        /// Indicates allocation groups save is in process, PRANA-11233
        /// </summary>
        private bool _isSaveInProcess = false;

        static AllocationManager()
        {
            try
            {
                CreateAllocationServicesProxy();
                CreateClosingServicesProxy();
                CreateCashManagementProxy();
                CurrentCompanyUser = CachedDataManager.GetInstance.LoggedInUser;
                //commented by omsiv,sep 14, ACA Cleanup
                // GetACAAccounts();
                Initlise();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create Position Services Proxy to connect with Trade Server
        /// </summary>
        public static void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    string endpointAddressInString = ConfigurationManager.AppSettings["PositionManagementEndpointAddress"];
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>(endpointAddressInString);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static List<int> GetListOfUnlockedAccounts(List<int> accountList)
        {
            List<int> unlockedAccounts = new List<int>();
            try
            {
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                //returns List of Currently available accounts on which Lock can be taken
                unlockedAccounts = _pranaPositionServices.InnerChannel.GetListOfUnlockedAccounts(accountList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return unlockedAccounts;
        }

        /// <summary>
        /// Set the accounts to be locked by user on server Cache
        /// </summary>
        /// <param name="accountsToBeLocked"></param>
        /// <returns></returns>
        public static bool SetAccountsLockStatus(List<int> accountsToBeLocked)
        {
            bool isSucessuful = false;
            try
            {
                //GetUserPermittedCompanyList
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                //return true if the Account/CashAccounts required is not locked by any other user
                isSucessuful = _pranaPositionServices.InnerChannel.SetAccountsLockStatus(userID, accountsToBeLocked);
                if (isSucessuful)
                {
                    CachedDataManager.GetInstance.UpdateAccountLockData(accountsToBeLocked);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSucessuful;
        }

        private static CompanyUser currentCompanyUser;
        public static CompanyUser CurrentCompanyUser
        {
            get { return currentCompanyUser; }
            set { currentCompanyUser = value; }
        }

        private object lockerAllocationSave = new object();
        private GenericBindingList<AllocationGroup> _allocatedGroups = new GenericBindingList<AllocationGroup>();
        private bool _isAllocDataReferenced;
        static bool alreadyFilledPostion = false;

        private static Dictionary<String, AllocationGroup> _dictunsavedCancelAmend = new Dictionary<String, AllocationGroup>();
        public Dictionary<String, AllocationGroup> DictunsavedCancelAmend
        {
            get { return _dictunsavedCancelAmend; }
            set { value = _dictunsavedCancelAmend; }
            }

        private Dictionary<string, AllocationGroup> _dictGroups = new Dictionary<string, AllocationGroup>();

        static ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            get { return _allocationServices; }
        }

        static ProxyBase<IAllocationManager> _allocation = null;
        public ProxyBase<IAllocationManager> Allocation
        {
            get { return _allocation; }
        }

        static ProxyBase<IClosingServices> _closingServices = null;
        public ProxyBase<IClosingServices> ClosingServices
        {
            get { return _closingServices; }
        }

        static ProxyBase<ICashManagementService> _cashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            get { return _cashManagementServices; }
        }

        private AllocationManager()
        {
            MakeProxy();
        }

        private static void Initlise()
        {
            _defaultID = int.Parse(DateTime.Now.ToString("ddHHmmss"));
        }

        private static AllocationManager _singleton = null;
        private static object _locker = new object();

        public static AllocationManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new AllocationManager();
                    }
                }
            }
            return _singleton;
        }

        private static void CreateClosingServicesProxy()
        {
            string endpointAddressInString = ConfigurationManager.AppSettings["ClosingEndpointAddress"];
            _closingServices = new ProxyBase<IClosingServices>(endpointAddressInString);
            // EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);

            //NetTcpBinding netTcpBinding = new NetTcpBinding();
            //_proxyClosingServices = ChannelFactory<IClosingServices>.CreateChannel(netTcpBinding, endpointAddress);
        }

        private static void CreateAllocationServicesProxy()
        {
            string endpointAddressInString = ConfigurationManager.AppSettings["AllocationEndpointAddress"];
            _allocationServices = new ProxyBase<IAllocationServices>(endpointAddressInString);
            //_proxyAllocationServices=b.InnerChannel;

            string endpointAddressAllocation = ConfigurationManager.AppSettings["AllocationEndpointAddressNew"];
            _allocation = new ProxyBase<IAllocationManager>(endpointAddressAllocation);

            //EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);
            //NetTcpBinding netTcpBinding = new NetTcpBinding();
            // _proxyAllocationServices = ChannelFactory<IAllocationServices>.CreateChannel(netTcpBinding, endpointAddress);
        }

        private static void CreateCashManagementProxy()
        {
            string endpointAddressInString = ConfigurationManager.AppSettings["CashManagementEndpointAddress"];
            _cashManagementServices = new ProxyBase<ICashManagementService>(endpointAddressInString);
        }

        /// <summary>
        /// New AutoGroup function
        /// </summary>
        /// <param name="allocationPreferences">Preferences</param>
        /// <param name="tempUnallocated">Data to be grouped</param>
        /// <returns>Message</returns>
        public String AutoGroup(AllocationPreferences allocationPreferences, GenericBindingList<AllocationGroup> tempUnallocated)
        {
            try
            {
                int initialCount = 0;
                int finalCount = 0;
                int totalCount = tempUnallocated.Count;

                Dictionary<String, List<AllocationGroup>> tempDictionary = new Dictionary<string, List<AllocationGroup>>();
                List<String> alreadyAddedGroup = new List<string>();

                for (int outerLoop = 0; outerLoop < tempUnallocated.Count; outerLoop++)
                {
                    AllocationGroup outerAllocationGroup = tempUnallocated[outerLoop];

                    String currentGroupId = outerAllocationGroup.GroupID;
                    List<AllocationGroup> tempList = new List<AllocationGroup>();

                    int indexOuter = alreadyAddedGroup.BinarySearch(currentGroupId);
                    if (indexOuter < 0)
                    {
                        tempList.Add(outerAllocationGroup);

                        alreadyAddedGroup.Insert(~indexOuter, currentGroupId);

                        for (int innerLoop = outerLoop + 1; innerLoop < tempUnallocated.Count; innerLoop++)
                        {
                            AllocationGroup innerAllocationGroup = tempUnallocated[innerLoop];

                            int indexInner = alreadyAddedGroup.BinarySearch(innerAllocationGroup.GroupID);

                            if (indexInner >= 0)
                                continue;
                            if (!AllocationRules.AreGroupsGroupable(innerAllocationGroup, outerAllocationGroup, allocationPreferences))
                                continue;
                            alreadyAddedGroup.Insert(~indexInner, innerAllocationGroup.GroupID);
                            tempList.Add(innerAllocationGroup);
                        }

                        if (tempList.Count > 1)
                            tempDictionary.Add(currentGroupId, tempList);
                    }
                }

                foreach (KeyValuePair<string, List<AllocationGroup>> groupItem in tempDictionary)
                {
                    if (groupItem.Value.Count <= 1) continue;
                    initialCount += groupItem.Value.Count;
                    finalCount++;
                    BundleGroups(groupItem.Value);
                }
                if (initialCount > 0)
                {
                    string message = "Out of " + totalCount + " trade(s), " + initialCount + " is/are grouped into " +
                                     finalCount + " group(s)";
                    return message;
                }
                return "Nothing to group.";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return "Some Error has occurred";
            }

        }

        public String AutoGroupOld(AllocationPreferences _allocationPrefs, GenericBindingList<AllocationGroup> tUnAllocatedGroups)
        {
            try
            {
                int initialGroups = 0;
                int finalGroups = 0;
                int totalGroups = tUnAllocatedGroups.Count;

                List<string> groupsList = new List<string>();
                Dictionary<string, List<AllocationGroup>> dict = new Dictionary<string, List<AllocationGroup>>();

                AllocationGroupCollection groupsToBeDeleted = new AllocationGroupCollection();
                AllocationGroupCollection groupsToAdded = new AllocationGroupCollection();
                for (int outerLoop = 0; outerLoop < tUnAllocatedGroups.Count; outerLoop++)
                {
                    AllocationGroup outerGroup = tUnAllocatedGroups[outerLoop];
                    groupsList.Add(outerGroup.GroupID);
                    //Sorting data as it uses binary search insead of contains in inner loop
                    groupsList.Sort();
                    List<AllocationGroup> groups = new List<AllocationGroup>();
                    groups.Add(outerGroup);
                    for (int innerLoop = outerLoop + 1; innerLoop < tUnAllocatedGroups.Count; innerLoop++)
                    {
                        AllocationGroup innerGroup = tUnAllocatedGroups[innerLoop];

                        //Using Binary search to find a specific groupId as searching becomes timetaking when count of groups increases
                        //BinarySearch returns negative of index where specified value can be inserted if not found.
                        //Further details can be found at http://msdn.microsoft.com/en-us/library/w4e7fxsh%28v=vs.80%29.aspx
                        int index = groupsList.BinarySearch(innerGroup.GroupID);

                        if (index < 0)
                        {
                            if (AllocationRules.AreGroupsGroupable(outerGroup, innerGroup, _allocationPrefs))
                            {
                                //Inserting value based on index
                                groupsList.Insert(~index, innerGroup.GroupID);

                                //groupsList.Add(innerGroup.GroupID);

                                groups.Add(innerGroup);
                            }
                        }
                    }
                    if (groups.Count > 1)
                    {
                        dict.Add(outerGroup.GroupID, groups);
                    }
                }
                finalGroups = dict.Count;
                foreach (KeyValuePair<string, List<AllocationGroup>> groupItem in dict)
                {
                    initialGroups += groupItem.Value.Count;
                    BundleGroups(groupItem.Value);
                }
                if (initialGroups > 0)
                {
                    string message = "Out of " + totalGroups + " trade(s), " + initialGroups + " is/are grouped into " +
                                     finalGroups + " group(s)";
                    //string message = "Grouped. Initial Groups: " + initialGroups.ToString() + "/" + totalGroups.ToString() + ", Final Groups: " + finalGroups.ToString();
                    //Logger.Write(message);
                    return message;
                }
                else
                {
                    return "Nothing to group.";
                }
            }
            catch (Exception ex)
            {


                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
                return "Error.";
            }


        }

        public void FillCurrentPositions()
        {
            try
            {
                if (!alreadyFilledPostion)
                {
                    CurrentPositionList.Clear();
                    List<CurrentPosition> list = PositionDataManager.GetCurrentPositions();
                    foreach (CurrentPosition pos in list)
                    {
                        CurrentPositionList.AddPosition(pos);
                    }
                    alreadyFilledPostion = true;
                }
            }

            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void ClearPositions()
        {
            CurrentPositionList.Clear();
            alreadyFilledPostion = false;
        }

        public void ReFillCurrentPositions()
        {
            alreadyFilledPostion = false;
            FillCurrentPositions();
        }

        //Used in ctrlAmendmend
        public void Remove(string groupID)
        {
            lock (lockerAllocationSave)
            {
                _dictGroups.Remove(groupID);
            }
        }
        //Used in ctrlAmendmend
        public void RemoveUnAllocated(AllocationGroup group)
        {
            lock (lockerAllocationSave)
            {
                if (_unAllocatedGroups.Contains(group))
                {
                    _unAllocatedGroups.Remove(group);
                }
            }
        }
        //Used in ctrlAmendmend
        public void RemoveAllocated(AllocationGroup group)
        {
            lock (lockerAllocationSave)
            {
                if (_allocatedGroups.Contains(group))
                {
                    _allocatedGroups.Remove(group);
                }
            }
        }

        public void ResetTheResetDictionary(AllocationGroup group)
        {
            TaxLot newTaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot((PranaBasicMessage)group, group.GroupID);
            group.ResetTheResetDictionary(newTaxlot);
        }
        //Used in AllocationMain,ctrlAmendmend and Allocation Manager
        public GenericBindingList<AllocationGroup> UnAllocatedGroups
        {
            get { return _unAllocatedGroups; }
            set { _unAllocatedGroups = value; }
        }
        //Used in AllocationMain And ctrlAmendmend
        public GenericBindingList<AllocationGroup> AllocatedGroups
        {
            get { return _allocatedGroups; }
            set { _allocatedGroups = value; }
        }

        //Used in AllocationMain,ctrlAmendmend and Allocation Manager
        public List<int> LstAccountLocks
        {
            get { return _lstAccountLocks; }
            set { _lstAccountLocks = value; }
        }

        internal Dictionary<string, AllocationGroup> GetAllocatedUnSavedGroups()
        {
            Dictionary<string, AllocationGroup> groups = new Dictionary<string, AllocationGroup>();
            //List<AllocationGroup> groups = new List<AllocationGroup>();
            lock (lockerAllocationSave)
            {
                foreach (AllocationGroup group in _allocatedGroups)
                {
                    if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        if (!groups.ContainsKey(group.GroupID))
                        {
                            groups.Add(group.GroupID, group);
                        }
                    }
                }
            }
            return groups;
        }

        ///// <summary>
        ///// Basically checks if commission is also referencing the same data or not.
        ///// </summary>

        public bool IsAllocDataReferenced
        {
            get { return _isAllocDataReferenced; }
            set { _isAllocDataReferenced = value; }
        }

        public int SaveGroups(bool isSaveState=true)
        {
            List<AllocationGroup> lstDirtyGroups = new List<AllocationGroup>();
            int rowsAffected = 0;
            Dictionary<string, AllocationGroup> dictUnsavedAuditList = new Dictionary<string, AllocationGroup>();
            try
            {
                List<string> taxlotsList = new List<string>();
                List<AllocationGroup> groups = new List<AllocationGroup>();
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup group in _unAllocatedGroups)
                    {
                        if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.Commission_Changed))
                            group.IsCommissionChanged = true;

                        if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.SoftCommission_Changed))
                            group.IsSoftCommissionChanged = true;

                        if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            //Mukul : 20131118:
                            //Unallocated Groups which are partially or fully closed should not be allowed to save..
                            if ((group.ClosingStatus != Prana.BusinessObjects.AppConstants.ClosingStatus.Open))
                            {
                                lstDirtyGroups.Add(group);
                            }
                            else
                            {
                                groups.Add(group);
                            }
                        }
                    }
                    foreach (AllocationGroup group in _allocatedGroups)
                    {
                        if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            //Mukul : 20131118:
                            //Allocated Groups which are partially or fully closed and have there persistence status as reallocated shouldnt be allowed to save
                            // as that will corrupt the data..This is a temporary solution as the ideal way to handle these scenarios is through data dirty checks while
                            //data is published from closing UI as the persistence status should be same for the both the published group and binded group otherwise the data is dirty and 
                            //needs to be refreshed..
                            if ((group.ClosingStatus != Prana.BusinessObjects.AppConstants.ClosingStatus.Open) && (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated))
                            {
                                //Sandeep: 20131106 (http://jira.nirvanasolutions.com:8080/browse/PRANA-2874)
                                //lstDirtyGroups contain those groups whose closing status has been changed to closed or partially closed after they were reallocated..
                                //Scenario..
                                //1) user opens close Trade by right click a closing transaction..
                                //2.) user reallocates the same closing transaction..
                                //3.) closes that group from close order UI..
                                //4.) clicks save from main allocation UI..
                                //5.) That group should not allowed to save as it will corrupt the data in Pm_Taxlots table and remove the closing trail entry..
                                lstDirtyGroups.Add(group);
                            }
                            else
                            {
                                groups.Add(group);
                            }
                        }
                    }
                    if (_deletedGroups != null)
                    {
                        foreach (AllocationGroup group in _deletedGroups)
                        {
                            if ((group.ClosingStatus != Prana.BusinessObjects.AppConstants.ClosingStatus.Open))
                            {
                                lstDirtyGroups.Add(group);
                            }
                            else
                            {
                                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;
                                groups.Add(group);
                            }
                        }
                    }
                    if (_deletedOmittedGroups != null)
                    {
                        foreach (AllocationGroup group in _deletedOmittedGroups)
                        {
                            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                            if (_unAllocatedGroups.Contains(group))
                            {
                                _unAllocatedGroups.Remove(group);
                            }
                            else
                            {
                                groups.Add(group);
                            }
                        }
                    }
                    if (_dictunderlyingExercisedGroups != null)
                    {
                        foreach (KeyValuePair<string, List<AllocationGroup>> kp in _dictunderlyingExercisedGroups)
                        {
                            groups.AddRange(kp.Value);
                        }
                    }
                }
                List<string> lstAccountLockNotAcquired = new List<string>();
                if (groups.Count > 0)
                {
                    //Narendra Kumar jangir 2013 Mar 05
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2238
                    foreach (AllocationGroup group in groups)
                    {
                        #region check for account lock while saving data
                        foreach (TaxLot tx in group.TaxLots)
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
                                    foreach (TaxLot tx in group.TaxLots)
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
                    }

                    //todo: from list pull distinct accountid
                    //if (CachedDataManager.GetPranaReleaseType().Equals(PranaReleaseViewType.CHMiddleWare) && lstAccountLockNotAcquired.Count > 0)
                    if (lstAccountLockNotAcquired.Count > 0)
                    {
                        MessageBox.Show("Account lock not acquired for account(s) " + String.Join(",", lstAccountLockNotAcquired.ToArray()) + " ,please acquire account lock before proceeding.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }

                    if (ValidateTradeForAccountNAVLock(groups))
                    {
                        SaveEditedGroupsAuditEntry(dictUnsavedAuditList);
                        AuditManager.Instance.SaveAuditList();
                        //Set _isSaveInProcess to true while allocation groups save is started, PRANA-11233
                        _isSaveInProcess = true;
                        if (isSaveState == false)
                            foreach (AllocationGroup group in groups)
                                group.IsAnotherTaxlotAttributesUpdated = false;
                        rowsAffected = _allocation.InnerChannel.SaveGroups(groups, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).Count();
                        _isSaveInProcess = false;
                        SetDefaultPersistenceStatusForCachedgroups();
                        ClearExercisedGroupsDictionary();
                    }
                }
                if (lstDirtyGroups.Count > 0)
                {
                    foreach (AllocationGroup dirtyGroup in lstDirtyGroups)
                    {
                        SetDefaultPersistenceStatus(dirtyGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        public void SaveUnSavedGroups()
        {
            try
            {
                List<AllocationGroup> groups = new List<AllocationGroup>();
                lock (lockerAllocationSave)
                {
                    if (_dictGroups.Count > 0)
                    {
                        foreach (KeyValuePair<string, AllocationGroup> groupKeyValue in _dictGroups)
                        {
                            groups.Add(groupKeyValue.Value);
                        }
                        _allocationServices.InnerChannel.SaveGroups(groups, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                        _dictGroups.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void UnAllocateGroup(AllocationGroup group)
        {
            PostTradeConstants.ORDERSTATE_ALLOCATION prevState = group.State;
            AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.UNALLOCATE, "", "Group Unallocated", currentCompanyUser.CompanyUserID);
            if (group.TaxLots.Count > 0)
            {
                AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.UNALLOCATE, "", "Group Unallocated Taxlots Deleted", currentCompanyUser.CompanyUserID);
            }
            group = _allocationServices.InnerChannel.UnAllocateGroup(group);
            AddGroup(group);
        }

        //Used in AllocationMain 
        AllocationPreferences _allocationPreferences = new AllocationPreferences();
        public void BundleGroups(List<AllocationGroup> groups)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    AllocationGroup newGroup = (AllocationGroup)groups[0].Clone();
                    newGroup.IsModified = false;
                    newGroup.ClearTaxlotDictionary();
                    newGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.New;
                    newGroup.GroupID = _allocationServices.InnerChannel.GenerateGroupID();
                    _dictGroups.Add(newGroup.GroupID, newGroup);
                    bool bTradeDate = _allocationPreferences.AutoGroupingRules.TradeDate;
                    bool bProcessDate = _allocationPreferences.AutoGroupingRules.ProcessDate;

                    //newGroup.TaxLots.Add(unallocatedtaxlot);
                    int i = 0;
                    foreach (AllocationGroup group in groups)
                    {

                        if (i != 0)
                        {
                            newGroup.AddGroup(group);
                        }
                        AddDeletedGroups(group);
                        ///TODO:Puneet Add automated comment
                        AuditManager.Instance.AddGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.GROUP, "", "Trades Grouped(Deleted)", CurrentCompanyUser.CompanyUserID);
                        _dictGroups.Remove(group.GroupID);
                        _unAllocatedGroups.Remove(group);
                        i++;
                    }

                    //Grouping rule to show blank if grouping is allowed with multiple values of
                    //1. Side
                    //2. CounterPartyID
                    //3. VenueID
                    //4. TradingAccountID
                    foreach (AllocationOrder order in newGroup.Orders)
                    {
                        if (newGroup.OrderSideTagValue != String.Empty && newGroup.OrderSideTagValue != order.OrderSideTagValue)
                        {
                            newGroup.OrderSideTagValue = String.Empty;
                            newGroup.OrderSide = String.Empty;
                        }
                        if (newGroup.CounterPartyID != 0 && newGroup.CounterPartyID != order.CounterPartyID)
                        {
                            newGroup.CounterPartyID = 0;
                            newGroup.CounterPartyName = String.Empty;
                        }

                        //Updating trade attributes if different for groups then empty.
                        if (newGroup.UserID != order.CompanyUserID)
                        {
                            newGroup.UserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                            newGroup.CompanyUserName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(newGroup.UserID);
                            newGroup.CompanyUserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute1) || string.IsNullOrWhiteSpace(order.TradeAttribute1)
                            || newGroup.TradeAttribute1.Trim() != order.TradeAttribute1.Trim())
                        {
                            newGroup.TradeAttribute1 = string.Empty;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute2) || string.IsNullOrWhiteSpace(order.TradeAttribute2)
                            || newGroup.TradeAttribute2.Trim() != order.TradeAttribute2.Trim())
                        {
                            newGroup.TradeAttribute2 = string.Empty;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute3) || string.IsNullOrWhiteSpace(order.TradeAttribute3)
                            || newGroup.TradeAttribute3.Trim() != order.TradeAttribute3.Trim())
                        {
                            newGroup.TradeAttribute3 = string.Empty;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute4) || string.IsNullOrWhiteSpace(order.TradeAttribute4)
                            || newGroup.TradeAttribute4.Trim() != order.TradeAttribute4.Trim())
                        {
                            newGroup.TradeAttribute4 = string.Empty;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute5) || string.IsNullOrWhiteSpace(order.TradeAttribute5)
                            || newGroup.TradeAttribute5.Trim() != order.TradeAttribute5.Trim())
                        {
                            newGroup.TradeAttribute5 = string.Empty;
                        }
                        if (string.IsNullOrWhiteSpace(newGroup.TradeAttribute6) || string.IsNullOrWhiteSpace(order.TradeAttribute6)
                            || newGroup.TradeAttribute6.Trim() != order.TradeAttribute6.Trim())
                        {
                            newGroup.TradeAttribute6 = string.Empty;
                        }
                        if (newGroup.VenueID != 0 && newGroup.VenueID != order.VenueID)
                        {
                            newGroup.VenueID = 0;
                            newGroup.Venue = String.Empty;
                        }
                        if (newGroup.TradingAccountID != 0 && newGroup.TradingAccountID != order.TradingAccountID)
                        {
                            newGroup.TradingAccountID = 0;
                            newGroup.TradingAccountName = String.Empty;
                        }
                        if (bTradeDate && newGroup.ProcessDate < order.ProcessDate)
                        {
                            newGroup.ProcessDate = order.ProcessDate;
                        }
                        if (bProcessDate && newGroup.AUECLocalDate < order.AUECLocalDate)
                        {
                            newGroup.AUECLocalDate = order.AUECLocalDate;
                        }
                        //CHMW-3149	[Foreign Positions Settling in Base Currency] Handle grouping/ungrouping for settlement fields
                        if (newGroup.SettlementCurrencyID != order.SettlementCurrencyID)
                        {
                            newGroup.SettlementCurrencyID = 0;
                        }
                    }
                    TaxLot unallocatedtaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot(newGroup, newGroup.GroupID);
                    unallocatedtaxlot.TaxLotState = ApplicationConstants.TaxLotState.New;
                    List<TaxLot> newtaxlots = new List<TaxLot>();
                    newtaxlots.Add(unallocatedtaxlot);
                    newGroup.ResetTaxlotDictionary(newtaxlots);

                    //MUKUL 20121127: Rounding AvgPX here based on rounding prefs set at AUEC level from Admin...
                    AUECRoundingRulesHelper.ApplyRounding(newGroup);
                    AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.GROUP, "", "Trades Grouped(Created)", CurrentCompanyUser.CompanyUserID);
                    _unAllocatedGroups.Add(newGroup);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        //Used in AllocationMain
        public String UnBundleGroups(List<AllocationGroup> groups)
        {
            int initialGroups = 0;
            int finalGroups = 0;
            int totalGroups = groups.Count;
            try
            {
                GenericRepository<AllocationGroup> deletedGroups = new GenericRepository<AllocationGroup>();
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup group in groups)
                    {
                        if (group.Orders.Count > 1) // unbundle all groups orders in seperate group if count is more than one
                        {
                            initialGroups++;
                            _unAllocatedGroups.Remove(group);
                            AuditManager.Instance.AddGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.UNGROUP, "", "Groups Ungrouped(Deleted)", CurrentCompanyUser.CompanyUserID);
                            //AuditManager.Instance.AddGroupToDeletedCollection(group);
                            #region Removing unused inner objects to remove cloning overhead
                            AllocationGroup clonableGroup = (AllocationGroup)group.Clone();
                            clonableGroup.ClearTaxlotDictionary();
                            clonableGroup.Orders.Clear();
                            //clonableGroup.OrdersH.Clear();
                            clonableGroup.Orders = null;
                            clonableGroup.OrdersH = null;
                            #endregion

                            AddDeletedGroups(group);
                            _dictGroups.Remove(group.GroupID);
                            foreach (AllocationOrder order in group.Orders)
                            {
                                finalGroups++;
                                AllocationGroup newGroup = (AllocationGroup)clonableGroup.Clone();//Now object wih only required fields will be cloned

                                newGroup.Orders = new List<AllocationOrder>();
                                newGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.New;
                                //newGroup.Orders = new List<AllocationOrder>();
                                newGroup.GroupID = _allocationServices.InnerChannel.GenerateGroupID();

                                newGroup.SetGroupDetailsFromOrder(order);

                                //Adding mapped data from ChachedDataManager and TagDataBaseManager for
                                //1. OrderSide
                                //2. Venue
                                //3. CounterParty
                                newGroup.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(newGroup.OrderSideTagValue);
                                newGroup.Venue = CachedDataManager.GetInstance.GetVenueText(newGroup.VenueID);
                                newGroup.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(newGroup.CounterPartyID);
                                newGroup.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(newGroup.TradingAccountID);
                                newGroup.CompanyUserName = CachedDataManager.GetInstance.GetUserText(newGroup.CompanyUserID);

                                TaxLot unallocatedtaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot(newGroup, newGroup.GroupID);
                                unallocatedtaxlot.TaxLotState = ApplicationConstants.TaxLotState.New;
                                List<TaxLot> newtaxlots = new List<TaxLot>();
                                newtaxlots.Add(unallocatedtaxlot);
                                newGroup.ResetTaxlotDictionary(newtaxlots);

                                _dictGroups.Add(newGroup.GroupID, newGroup);

                                _unAllocatedGroups.Add(newGroup);
                                AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.UNGROUP, "", "Ungrouped Groups (Created)", CurrentCompanyUser.CompanyUserID);
                            }
                        }
                    }
                }
                if (initialGroups != finalGroups)
                {
                    string message = "Out of " + totalGroups.ToString() + " group(s), " + initialGroups.ToString() + " is/are ungrouped to " + finalGroups.ToString() + " trade(s)";                  
                    return message;
                }
                else
                {
                    return "Nothing to ungroup.";
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return "Error";
            }

        }
        public void AllocateGroup(AllocationGroup group, AllocationLevelList accounts)
        {
            PostTradeConstants.ORDERSTATE_ALLOCATION prevState = group.State;

            bool flagIsReallocate = false;
            if (group.TaxLots.Count > 0)
            {
                AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.REALLOCATE, "", "Trade Reallocated Taxlots Deleted", currentCompanyUser.CompanyUserID);
                flagIsReallocate = true;
            }
            group = _allocationServices.InnerChannel.AllocateGroup(group, accounts);
            if (group.TaxLots.Count > 0)
            {
                if (flagIsReallocate)
                {
                    AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Reallocated", CurrentCompanyUser.CompanyUserID);
                    AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group ReAllocated Taxlots Created", currentCompanyUser.CompanyUserID);
                }
                else
                {
                    AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated from Unallocate", CurrentCompanyUser.CompanyUserID);
                    AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated New Taxlots Created", currentCompanyUser.CompanyUserID);
                }
            }
            AddGroup(group);
        }

        /// <summary>
        /// Returns Groups according to filter conditons
        /// </summary>
        /// <param name="ToAllAUECDatesString">Final date to include</param>
        /// <param name="FromAllAUECDatesString">Initial date to include</param>
        /// <param name="filterList">List of filter conditions</param>
        /// <param name="async">IsAsynchronous</param>
        /// <param name="userID">UserID</param>
        /// <returns>Filtered Groups</returns>
        public List<AllocationGroup> GetGroups(string ToAllAUECDatesString, string FromAllAUECDatesString, Dictionary<String, Dictionary<String, String>> filterList, bool async, int userID)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {

                //            List<AllocationGroup> groups = new List<AllocationGroup>();
                groups = _allocation.InnerChannel.GetGroups(DateTime.Parse(ToAllAUECDatesString), DateTime.Parse(FromAllAUECDatesString), filterList, userID);
                if (!async)
                {
                    AddGroupsToUI(groups);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
            return groups;


        }

        public List<AllocationGroup> GetGroups(string ToAllAUECDatesString, string FromAllAUECDatesString, bool async, int userID)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {

                //            List<AllocationGroup> groups = new List<AllocationGroup>();
                groups = _allocationServices.InnerChannel.GetGroups(ToAllAUECDatesString, FromAllAUECDatesString, userID);
                if (!async)
                {
                    AddGroupsToUI(groups);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
            return groups;


        }

        /// <summary>
        /// Get groups from database for all the requsted taxlotids.
        /// This method is used in new cancel-amd+recon CH UI
        /// </summary>
        /// <param name="lstTaxLotIds"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public List<AllocationGroup> GetGroups(List<string> lstTaxLotIds, bool async)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                groups = _allocationServices.InnerChannel.GetGroupsForTaxLotIDs(lstTaxLotIds);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return groups;
        }

        public void AddGroupsToUI(List<AllocationGroup> groups)
        {
            try
            {
                List<string> userAccounts = CachedDataManager.GetInstance.GetAllAccountIDsForUser();
                ClearData();
                foreach (AllocationGroup group in groups)
                {
                    int countNavLocked = 0;
                    int countNavUnlocked = 0;
                    string taxlotNavLockStatus = string.Empty;
                    NameValueFiller.FillNameDetailsOfMessage(group);
                    bool isGroupAllowed = true;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        // Check AllocatedGroup if it contains user permitted CashAccounts, PRANA-12913
                        if (!userAccounts.Contains(taxlot.Level1ID.ToString()))
                        {
                            isGroupAllowed = false;
                            break;
                        }
                        taxlot.Level1Name = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                        taxlot.Level2Name = CachedDataManager.GetInstance.GetStrategyText(taxlot.Level2ID);
                        taxlot.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlot.OrderSideTagValue);
                        //Added By faisal Shah
                        //Updating NavLockStatus for Both Taxlot and Group Level
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1147
                        DateTime dt = DateTime.MinValue;
                        Boolean isValidTradeDate = DateTime.TryParse(taxlot.AUECLocalDate.ToString(), out dt);
                        if (isValidTradeDate)
                        {
                            bool isTradeAllowed = Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(taxlot.Level1ID, dt);
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
                    SetDefaultPersistenceStatus(group);
                    if(isGroupAllowed)
                    {
                        AddGroup(group);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool IsGroupStatusChanged(AllocationGroup group)
        {
            bool isChanged = false;
            try
            {
                switch (group.PersistenceStatus)
                {
                    case ApplicationConstants.PersistenceStatus.UnGrouped:
                    case ApplicationConstants.PersistenceStatus.New:
                    case ApplicationConstants.PersistenceStatus.Updated:
                    case ApplicationConstants.PersistenceStatus.ReAllocated:
                    case ApplicationConstants.PersistenceStatus.Deleted:
                        isChanged = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isChanged;
        }

        public AllocationGroup GetGroup(string groupID)
        {
            if (_dictGroups.ContainsKey(groupID))
            {
                return _dictGroups[groupID];
            }
            else
                return null;
        }

        public void ClearData()
        {
            lock (lockerAllocationSave)
            {
                // clear data
                _unAllocatedGroups.Clear();
                ClearDictionaryUnsaved();
                _allocatedGroups.Clear();
                _dictGroups.Clear();
                _deletedGroups.Clear();
                _deletedOmittedGroups.Clear();
            }
            AuditManager.Instance.clearAuditListAndDeletedList();
        }
        private GenericBindingList<AllocationGroup> _unAllocatedGroups = new GenericBindingList<AllocationGroup>();
        private AllocationGroupCollection _deletedGroups = new AllocationGroupCollection();
        private AllocationGroupCollection _deletedOmittedGroups = new AllocationGroupCollection();
        private Dictionary<string, List<AllocationGroup>> _dictunderlyingExercisedGroups = new Dictionary<string, List<AllocationGroup>>();
        private List<int> _lstAccountLocks = new List<int>();

        public void AddExercisedGroups(AllocationGroup UnderlyingGroup, AllocationGroup ClosingGroup)
        {
            if (UnderlyingGroup.PersistenceStatus.Equals(ApplicationConstants.PersistenceStatus.Updated))
            {
                if (!_dictunderlyingExercisedGroups.ContainsKey(UnderlyingGroup.GroupID))
                {
                    List<AllocationGroup> groups = new List<AllocationGroup>();
                    groups.Add(UnderlyingGroup);
                    groups.Add(ClosingGroup);
                    _dictunderlyingExercisedGroups.Add(UnderlyingGroup.GroupID, groups);
                }
                else
                {
                    List<AllocationGroup> groupsNew = new List<AllocationGroup>();
                    groupsNew.Add(UnderlyingGroup);
                    groupsNew.Add(ClosingGroup);
                    _dictunderlyingExercisedGroups[UnderlyingGroup.GroupID] = groupsNew;
                }
            }
        }

        public void ClearExercisedGroupsDictionary()
        {
            lock (lockerAllocationSave)
            {
                if (_dictunderlyingExercisedGroups != null)
                {
                    _dictunderlyingExercisedGroups.Clear();
                }
            }
        }

        //Used in AllocationMain,ctrlAmendmend
        public bool AnythingChanged()
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup group in _unAllocatedGroups)
                    {
                        if (IsGroupStatusChanged(group))
                        {
                            return true;
                        }
                    }
                    foreach (AllocationGroup group in _allocatedGroups)
                    {
                        if (IsGroupStatusChanged(group))
                        {
                            return true;
                        }
                    }
                    foreach (AllocationGroup group in _deletedGroups)
                    {
                        if (IsGroupStatusChanged(group))
                        {
                            return true;
                        }
                    }
                    foreach (AllocationGroup group in _deletedOmittedGroups)
                    {
                        if (IsGroupStatusChanged(group))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
            return false;
        }

        internal void SetDefaultPersistenceStatus(AllocationGroup group)
        {
            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.NotChanged;
            group.RemoveDeletedTaxlotsFromResetDictionary();
            foreach (TaxLot taxlot in group.GetAllTaxlots())
            {
                taxlot.TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
            }
            //Added by Narendra Kumar Jangir as on 20 Mar 2013,
            //This field is added to change taxlotPBwise state if another attributes(other than lotid and externaltransid) are changed on edit trade commission UI
            group.IsAnotherTaxlotAttributesUpdated = false;
            //group.ClearResetDictionary();
            //group.ClearUnAllocateTaxlots();
        }

        private void SetDefaultPersistenceStatusForCachedgroups()
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup group in _unAllocatedGroups)
                    {
                        SetDefaultPersistenceStatus(group);
                    }
                    foreach (AllocationGroup group in _allocatedGroups)
                    {
                        SetDefaultPersistenceStatus(group);
                    }
                    _deletedGroups.Clear();
                    _deletedOmittedGroups.Clear();
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// add group in cache and respective allocation type. Used in ctrlAmendmend
        /// </summary>
        /// <param name="allocationGroup"></param>
        public void AddGroup(AllocationGroup allocationGroup)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    if (!_dictGroups.ContainsKey(allocationGroup.GroupID))
                    {
                        _dictGroups.Add(allocationGroup.GroupID, allocationGroup);
                        if (Math.Round(allocationGroup.AllocatedQty, 4) < Math.Round(allocationGroup.CumQty, 4))
                        {
                            _unAllocatedGroups.Add(allocationGroup);
                        }
                        else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            _unAllocatedGroups.Add(allocationGroup);
                        }
                        else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            _allocatedGroups.Add(allocationGroup);
                        }
                    }
                    else
                    {
                        // throw new Exception("Group Already Added");
                        // AllocationGroup oldGroup = _dictGroups[allocationGroup.GroupID];
                        _dictGroups[allocationGroup.GroupID] = allocationGroup;
                        if (Math.Round(allocationGroup.AllocatedQty, 4) < Math.Round(allocationGroup.CumQty, 4))
                        {
                            _allocatedGroups.Remove(allocationGroup);
                            if (!_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Add(allocationGroup);
                            }
                            else
                            {
                                _unAllocatedGroups.UpdateItem(allocationGroup);
                            }

                        }
                        else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            _allocatedGroups.Remove(allocationGroup);
                            if (!_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Add(allocationGroup);
                            }
                        }
                        else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            // _allocatedGroups.Update();
                            //Corrected the condition to solve PRANA-12475
                            if (_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Remove(allocationGroup);
                            }

                            if (!_allocatedGroups.Contains(allocationGroup))
                            {
                                _allocatedGroups.Add(allocationGroup);
                            }
                            else
                            {
                                _allocatedGroups.UpdateItem(allocationGroup);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add or Remove AllocationGroup on the basis of TradingAccounts Permissions
        /// Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-12343
        /// </summary>
        /// <param name="allocationGroup"></param>
         public void AddOrRemoveGroupOnTradingAccount(AllocationGroup allocationGroup, bool isPresent)
        {
            try
            {
                //added lock object while updating allocated and unallocated groups cache,PRANA-13092
                lock (lockerAllocationSave)
                {
                if (isPresent)
                {
                    if (_dictGroups.ContainsKey(allocationGroup.GroupID))
                    {
                        _dictGroups[allocationGroup.GroupID] = allocationGroup;
                        if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                        }
                        else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            if (!_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Add(allocationGroup);
                            }
                        }
                    }
                }
                else
                {
                    if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                    {
                            if (_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Remove(allocationGroup);
                            }
                            if (_allocatedGroups.Contains(allocationGroup))
                            {
                                _allocatedGroups.Remove(allocationGroup);
                            }
                        }
                }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add or remove AllocationGroup on the basis of CashAccounts Permissions
         /// Purpose:  http://jira.nirvanasolutions.com:8080/browse/PRANA-12343
        /// </summary>
        /// <param name="allocationGroup"></param>
        /// <param name="isPresent"></param>
         public void AddOrRemoveGroupOnAccount(AllocationGroup allocationGroup, bool isPresent)
         {
             try
             {
                 //added lock object while updating allocated and unallocated groups cache,PRANA-13092
                 lock (lockerAllocationSave)
                 {
                     if (isPresent)
                     {
                         if (_dictGroups.ContainsKey(allocationGroup.GroupID))
                         {
                             _dictGroups[allocationGroup.GroupID] = allocationGroup;
                             if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                             {
                                 if (_unAllocatedGroups.Contains(allocationGroup))
                                 {
                                     _unAllocatedGroups.Remove(allocationGroup);
                                 }
                                 if (!_allocatedGroups.Contains(allocationGroup))
                                 {
                                     _allocatedGroups.Add(allocationGroup);
                                 }
                             }
                             else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                             {
                                 if (!_unAllocatedGroups.Contains(allocationGroup))
                                 {
                                     _unAllocatedGroups.Add(allocationGroup);
                                 }
                                 if (_allocatedGroups.Contains(allocationGroup))
                                 {
                                     _allocatedGroups.Remove(allocationGroup);
                                 }
                             }
                         }
                     }
                     else
                     {
                         if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED || allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        {
                            if (_unAllocatedGroups.Contains(allocationGroup))
                            {
                                _unAllocatedGroups.Remove(allocationGroup);
                            }
                            if (_allocatedGroups.Contains(allocationGroup))
                            {
                                _allocatedGroups.Remove(allocationGroup);
                            }
                        }
                     }               
                 }
             }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                 // gets out of our layer.
                 bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                 if (rethrow)
                 {
                     throw;
                 }
            }
        }

        public void UpdateRepositoryWithSecMasterInfo(SecMasterbaseList list)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup unallocatedGroup in _unAllocatedGroups)
                    {
                        foreach (SecMasterBaseObj obj in list)
                        {
                            if (unallocatedGroup.Symbol.Equals(obj.TickerSymbol))
                            {
                                unallocatedGroup.CopyBasicDetails(obj);
                            }

                        }
                    }
                    foreach (AllocationGroup allocatedGroup in _allocatedGroups)
                    {
                        foreach (SecMasterBaseObj baseobj in list)
                        {
                            if (allocatedGroup.Symbol.Equals(baseobj.TickerSymbol))
                            {
                                allocatedGroup.CopyBasicDetails(baseobj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Narendra Kumar Jangir 2013 Mar 05
        /// <summary>
        /// This method confirms that there is no duplicate entry in the _deletedGroups cache
        /// This method is used only to check duplicate entry in _deletedGroups cache
        /// </summary>
        /// <param name="group"></param>
        /// <returns>returns true if </returns>
        private bool CheckDuplicate(AllocationGroup group)
        {
            foreach (AllocationGroup algroup in _deletedGroups)
            {
                if (algroup.Orders[0].ParentClOrderID.ToString().Equals(group.Orders[0].ParentClOrderID.ToString()))
                    return true;
            }
            return false;
        }
        public void AddDeletedGroups(AllocationGroup group)
        {
            try
            {
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;
                if (!CheckDuplicate(group))
                {
                    _deletedGroups.Add(group);
                }
                _dictGroups.Remove(group.GroupID);
                group.ResetTaxlotDictionary(group.TaxLots);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        // used in ctrlAmendmend
        public void AddDeletedOmiitedGroups(AllocationGroup group)
        {
            try
            {
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                //TaxLot deletedTaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot((PranaBasicMessage)group, group.GroupID);
                //deletedTaxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                group.UpdateResetDictionaryWithDeletedState();
                lock (lockerAllocationSave)
                {
                    _deletedOmittedGroups.Add(group);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        // used in ctrlAmendmend
        public void ClearDeletedOmiitedGroups()
        {
            lock (lockerAllocationSave)
            {
                _deletedOmittedGroups.Clear();
            }
        }

        private Object lockDefaultID = new Object();
        private static int _defaultID = 0;
        public int GenerateDefaultID()
        {
            lock (lockDefaultID)
            {
                _defaultID++;
            }
            return _defaultID;
        }

        #region IPublishing Members

        DuplexProxyBase<ISubscription> _proxy;
        private void MakeProxy()
        {
            try
            {
                string endpointAddressInString = ConfigurationManager.AppSettings["SubscriptionEndpointAddress"];
                _proxy = new DuplexProxyBase<ISubscription>(endpointAddressInString, this);
                _proxy.Subscribe(Topics.Topic_CreateGroup, null);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                _proxy.Subscribe(Topics.Topic_Closing, null);
                _proxy.Subscribe(Topics.Topic_AllocationPreferenceUpdated, null);

                //Subscribed to Allocation Scheme Publishing Topic
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7811
                _proxy.Subscribe(Topics.Topic_AllocationSchemeUpdated, null);
                // _proxy.ProxyStatusChanged -= new Proxy<ISubscription>.ProxyStatusChangedHandler(_proxy_ProxyStatusChanged);
                // _proxy.ProxyStatusChanged += new Proxy<ISubscription>.ProxyStatusChangedHandler(_proxy_ProxyStatusChanged);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        delegate void PublishHandler(MessageData e, string topicName);

        public event EventHandler NewGroupReceived;
        public event EventHandler UpdateSymbolInfo;
        public event EventHandler UpdateGroupClosingStatusHandler;
        public event EventHandler AllocationPreferenceUpdated;

        /// <summary>
        /// The AllocationSchemeUpdated event to refresh allocation scheme list on allocation UI
        /// </summary>
        public event EventHandler AllocationSchemeUpdated;

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (e.TopicName == Topics.Topic_CreateGroup)
                {
                    //Added this check so that published group should wait while save is in process before adding to allocation group cache and UI, PRANA-11233
                    while (_isSaveInProcess)
                    {
                        Thread.Sleep(200);
                        continue;
                    }
                    System.Object[] dataList = (System.Object[])e.EventData;
                    if (NewGroupReceived != null)
                    {
                        NewGroupReceived(dataList, EventArgs.Empty);
                    }
                }
                if (e.TopicName == Topics.Topic_SecurityMaster)
                {
                    System.Object[] dataList = (System.Object[])e.EventData;
                    if (UpdateSymbolInfo != null)
                    {
                        UpdateSymbolInfo(dataList, EventArgs.Empty);
                    }
                }
                if (e.TopicName == Topics.Topic_Closing)
                {
                    System.Object[] dataList = (System.Object[])e.EventData;
                    if (UpdateGroupClosingStatusHandler != null)
                    {
                        UpdateGroupClosingStatusHandler(dataList, EventArgs.Empty);
                    }
                }
                else if (e.TopicName == Topics.Topic_AllocationPreferenceUpdated)
                {
                    if (AllocationPreferenceUpdated != null)
                        AllocationPreferenceUpdated(e.EventData, EventArgs.Empty);
                }
                //If Allocation Scheme is Published, then raise AllocationSchemeUpdated event
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7811                
                else if (e.TopicName == Topics.Topic_AllocationSchemeUpdated)
                {
                    if (AllocationSchemeUpdated != null)
                        AllocationSchemeUpdated(e.EventData, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void UpdateGroupClosingStatus(List<TaxLot> taxlotsList)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    //collect all taxlots of a group in a dictionary
                    Dictionary<string, List<TaxLot>> groupIDWiseTaxlotsDict = new Dictionary<string, List<TaxLot>>();
                    foreach (TaxLot publishedTaxlot in taxlotsList)
                    {
                        if (groupIDWiseTaxlotsDict.ContainsKey(publishedTaxlot.GroupID))
                        {
                            List<TaxLot> taxlotList = groupIDWiseTaxlotsDict[publishedTaxlot.GroupID];
                            taxlotList.Add(publishedTaxlot);
                            groupIDWiseTaxlotsDict[publishedTaxlot.GroupID] = taxlotList;
                        }
                        else
                        {
                            List<TaxLot> taxlotList = new List<TaxLot>();
                            taxlotList.Add(publishedTaxlot);
                            groupIDWiseTaxlotsDict.Add(publishedTaxlot.GroupID, taxlotList);
                        }
                    }
                    foreach (KeyValuePair<string, List<TaxLot>> kvp in groupIDWiseTaxlotsDict)
                    {
                        AllocationGroup group = GetGroup(kvp.Key);

                        if (group != null)
                        {
                            List<TaxLot> groupTaxlotList = group.TaxLots;
                            List<TaxLot> publishedTaxlotList = groupIDWiseTaxlotsDict[kvp.Key];
                            //update group taxlots
                            foreach (TaxLot updatedtaxlot in publishedTaxlotList)
                            {
                                foreach (TaxLot innerTaxLot in groupTaxlotList)
                                {
                                    if (updatedtaxlot.TaxLotID.Equals(innerTaxLot.TaxLotID))
                                    {
                                        innerTaxLot.ClosingStatus = updatedtaxlot.ClosingStatus;
                                        innerTaxLot.ClosingMode = updatedtaxlot.ClosingMode;
                                        innerTaxLot.ClosingDate = updatedtaxlot.ClosingDate;
                                        innerTaxLot.ClosingAlgo = updatedtaxlot.ClosingAlgo;
                                        break;
                                    }
                                }
                            }
                            //update group close status   
                            group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Open;
                            int closeCount = 0;
                            foreach (TaxLot taxlot in groupTaxlotList)
                            {
                                UpdateGroupStatus(group, taxlot);
                                group.ClosingAlgoText = Enum.GetName(typeof(PostTradeEnums.CloseTradeAlogrithm), taxlot.ClosingAlgo);
                                //update the minimum of close trade of all taxlots in a group
                                if (taxlot.ClosingDate != DateTimeConstants.MinValue
                                    && (group.ClosingDate > taxlot.ClosingDate
                                    || group.ClosingDate == DateTimeConstants.MinValue))
                                {
                                    group.ClosingDate = taxlot.ClosingDate;
                                }
                                if ((taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed))
                                {
                                    group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                                    closeCount++;
                                    break;
                                }
                                if (taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Closed)
                                {
                                    closeCount += 2;
                                }
                            }

                            if ((closeCount / 2) == groupTaxlotList.Count)
                                group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Closed;
                            else if ((closeCount / 2) > 0)
                                group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                            if (group.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Open)
                                group.ClosingDate = DateTimeConstants.MinValue;

                            //update allocated groups collection
                            if (_allocatedGroups.Contains(group))
                            {
                                _allocatedGroups.Update(group);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update group status whether it is generated by Corporate Action, option exercise or physical settlement
        /// </summary>
        /// <param name="group"></param>
        /// <param name="taxlot"></param>
        private void UpdateGroupStatus(AllocationGroup group, TaxLot taxlot)
        {
            try
            {
                if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction))
                {
                    group.GroupStatus = PostTradeEnums.Status.CorporateAction;
                }
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Exercise))
                {
                    group.GroupStatus = PostTradeEnums.Status.IsExercised;
                }
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Physical))
                {
                    group.GroupStatus = PostTradeEnums.Status.Exercise;
                }
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Physical))
                {
                    group.GroupStatus = PostTradeEnums.Status.Exercise;
                }
                // Set group status for cost adjustment
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7194
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.CostBasisAdjustment))
                {
                    group.GroupStatus = PostTradeEnums.Status.CostBasisAdjustment;
                }
                else
                {
                    group.GroupStatus = PostTradeEnums.Status.None;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public string getReceiverUniqueName()
        {
            return "AllocationForm";
        }
        #endregion

        internal bool IsGroupsDirty(AllocationGroup group)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    //Narendra Kumar Jangir 2013 Mar 05
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2238
                    //dirty groups should be checked on the basis of parentclorderID
                    foreach (KeyValuePair<string, AllocationGroup> algroup in _dictGroups)
                    {
                        //object reference error when we do trade from create transaction ui,  algroup.Value.Orders[0].ParentClOrderID is null
                        if (((algroup.Value.Orders.Count > 0) && (algroup.Value.Orders[0].ParentClOrderID != null) && (!algroup.Value.Orders[0].ParentClOrderID.Equals(string.Empty))) && ((group.Orders.Count > 0) && (group.Orders[0].ParentClOrderID != null) && (!group.Orders[0].ParentClOrderID.Equals((string.Empty)))))
                        {
                            if ((algroup.Value.Orders[0].ParentClOrderID.ToString().Equals(group.Orders[0].ParentClOrderID.ToString())) && (algroup.Value.PersistenceStatus != group.PersistenceStatus))
                            {
                                /// On UI dirty message, disable the "Get Data" button (http://209.234.251.99:8080/browse/PRANA-1427)
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        internal bool IsGroupDeleted(string groupID)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup deletedgroup in _deletedGroups)
                    {
                        if (deletedgroup.GroupID == groupID)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        internal void DeleteGroup(string groupID)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    if (_dictGroups.ContainsKey(groupID))
                    {
                        AllocationGroup deletedGroup = _dictGroups[groupID];
                        _unAllocatedGroups.Remove(deletedGroup);
                        _allocatedGroups.Remove(deletedGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public void UpdateFieldsForTaxLotCellChange(string columnKey, string cellText, AllocationGroup gParent, TaxLot taxlot)
        {
            try
            {
                gParent.UpdateGroupPersistenceStatus();
                gParent.IsModified = true;
                gParent.UpdateTaxlotState(taxlot);

                double updatedValue = 0.0;
                bool temp = double.TryParse(cellText, out updatedValue);
                bool isAmountUpdatedByFxRate = false;
                CommissionFields commFields = null;
                TradeAttributes tradeAttr = null;
                switch (columnKey)
                {
                    case OrderFields.PROPERTY_COMMISSION:
                        taxlot.Commission = updatedValue;
                        commFields = new CommissionFields();
                        commFields.Commission = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                        break;
                    case OrderFields.PROPERTY_SETTLEMENTCURRENCY:
                        taxlot.SettlementCurrencyID = Convert.ToInt32(updatedValue);
                        gParent.SettlementCurrencyID = Convert.ToInt32(updatedValue);
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case OrderFields.PROPERTY_SettCurrFXRate:
                        taxlot.SettlCurrFxRate = updatedValue;
                        gParent.SettlCurrFxRate = updatedValue;
                        gParent.SettlCurrAmt = gParent.UpdateTaxlotSettlementAmount(taxlot.SettlCurrFxRateCalc, taxlot.TaxLotID);
                        isAmountUpdatedByFxRate = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-11362
                        if (taxlot.SettlementCurrencyID == CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlot.Level1ID))
                        {
                            taxlot.FXConversionMethodOperator = taxlot.SettlCurrFxRateCalc;
                            taxlot.FXRate = taxlot.SettlCurrFxRate;
                        }
                        break;
                    case OrderFields.PROPERTY_SettCurrFXRateCalc:
                        taxlot.SettlCurrFxRateCalc = cellText;
                        gParent.SettlCurrFxRateCalc = cellText;
                        gParent.SettlCurrAmt = gParent.UpdateTaxlotSettlementAmount(taxlot.SettlCurrFxRateCalc, taxlot.TaxLotID);
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-11362
                        if (taxlot.SettlementCurrencyID == CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlot.Level1ID))
                        {
                            gParent.FXConversionMethodOperator = gParent.SettlCurrFxRateCalc;
                            taxlot.FXConversionMethodOperator = cellText;
                            taxlot.FXRate = taxlot.SettlCurrFxRate;
                        }
                        break;
                    case OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT:
                        taxlot.SettlCurrAmt = updatedValue;
                        gParent.SettlCurrAmt = updatedValue;
                        if (!isAmountUpdatedByFxRate)
                        {
                            gParent.UpdateTaxlotSettlementFxRate(taxlot.SettlCurrFxRateCalc, taxlot.TaxLotID);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                            taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                        }
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-11362
                        if (taxlot.SettlementCurrencyID == CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlot.Level1ID))
                        {
                            taxlot.FXConversionMethodOperator = taxlot.SettlCurrFxRateCalc;
                            taxlot.FXRate = taxlot.SettlCurrFxRate;
                        }
                        break;
                    case OrderFields.PROPERTY_SOFTCOMMISSION:
                        taxlot.SoftCommission = updatedValue;
                        commFields = new CommissionFields();
                        commFields.SoftCommission = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                        break;

                    case OrderFields.PROPERTY_OTHERBROKERFEES:
                        taxlot.OtherBrokerFees = updatedValue;
                        commFields = new CommissionFields();
                        commFields.OtherBrokerFees = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                        break;

                    case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                        taxlot.ClearingBrokerFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.ClearingBrokerFee = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                        break;

                    case OrderFields.PROPERTY_STAMPDUTY:
                        taxlot.StampDuty = updatedValue;
                        commFields = new CommissionFields();
                        commFields.StampDuty = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.StampDuty_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.StampDuty_Changed);
                        break;

                    case OrderFields.PROPERTY_TRANSACTIONLEVY:
                        commFields = new CommissionFields();
                        taxlot.TransactionLevy = updatedValue;
                        commFields.TransactionLevy = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.TransactionLevy_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TransactionLevy_Changed);
                        break;

                    case OrderFields.PROPERTY_CLEARINGFEE:
                        taxlot.ClearingFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.ClearingFee = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.ClearingFee_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.ClearingFee_Changed);
                        break;

                    case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                        taxlot.TaxOnCommissions = updatedValue;
                        commFields = new CommissionFields();
                        commFields.TaxOnCommissions = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                        break;

                    case OrderFields.PROPERTY_MISCFEES:
                        taxlot.MiscFees = updatedValue;
                        commFields = new CommissionFields();
                        commFields.MiscFees = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.MiscFees_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.MiscFees_Changed);
                        break;

                    case OrderFields.PROPERTY_SECFEE:
                        taxlot.SecFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.SecFee = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SecFee_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SecFee_Changed);
                        break;

                    case OrderFields.PROPERTY_OCCFEE:
                        taxlot.OccFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.OccFee = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.OccFee_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.OccFee_Changed);
                        break;

                    case OrderFields.PROPERTY_ORFFEE:
                        taxlot.OrfFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.OrfFee = updatedValue;
                        gParent.UpdateGroupCommissionAndFees(commFields);
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.OrfFee_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.OrfFee_Changed);
                        break;

                    case AllocationConstants.COL_AvgPrice:
                        //TODO: Handle negative avg price
                        if (double.Parse(cellText) < 0)
                        {
                            //e.Cell.CancelUpdate();
                        }
                        else
                        {
                            taxlot.AvgPrice = updatedValue;
                            gParent.SettlCurrAmt = gParent.UpdateTaxlotSettlementAmount(taxlot.SettlCurrFxRateCalc, taxlot.TaxLotID);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                            taxlot.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                            taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-11362
                            if (taxlot.SettlementCurrencyID == CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlot.Level1ID))
                            {
                                taxlot.FXConversionMethodOperator = taxlot.SettlCurrFxRateCalc;
                                taxlot.FXRate = taxlot.SettlCurrFxRate;
                            }
                        }
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_FXRate:
                        taxlot.FXRate = updatedValue;
                        gParent.UpdateTaxlotFXRateAndOperator(updatedValue, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.FxRate_Changed);
                        break;

                    case AllocationConstants.COL_FXConversionMethodOperator:
                        taxlot.FXConversionMethodOperator = cellText;
                        gParent.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, cellText, taxlot.TaxLotID);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                        break;
                    case AllocationConstants.COL_LotID:
                        taxlot.LotId = cellText;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.LotId_Changed);
                        break;
                    case AllocationConstants.COL_ExternalTransId:
                        taxlot.ExternalTransId = cellText;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.ExternalTransId_Changed);
                        break;

                    case AllocationConstants.COL_TradeAttribute1:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute1 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.COL_TradeAttribute2:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute2 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.COL_TradeAttribute3:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute3 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.COL_TradeAttribute4:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute4 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.COL_TradeAttribute5:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute5 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.COL_TradeAttribute6:
                        tradeAttr = new TradeAttributes();
                        tradeAttr.TradeAttribute6 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr, taxlot.TaxLotID);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;
                    case AllocationConstants.CAPTION_TaxlotQty:
                        //following condition indicates that group belongs to only one taxlot
                        if (gParent.TaxLots.Count == 1 && gParent.TaxLots[0].TaxLotQty == gParent.CumQty)
                        {
                            gParent.CumQty = double.Parse(cellText);
                            gParent.AllocatedQty = double.Parse(cellText);
                            gParent.Quantity = double.Parse(cellText);
                            gParent.UpdateTaxLotDataforCumQty(gParent);
                            //gParent.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                            AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                        }
                        break;
                    case AllocationConstants.COl_AUECLocalDate:
                        //following condition indicates that group belongs to only one taxlot
                        if (gParent.TaxLots.Count == 1 && gParent.TaxLots[0].TaxLotQty == gParent.CumQty)
                        {
                            gParent.AUECLocalDate = DateTime.Parse(cellText);
                            gParent.ProcessDate = DateTime.Parse(cellText);
                            gParent.OriginalPurchaseDate = DateTime.Parse(cellText);
                            gParent.ProcessDate = DateTime.Parse(cellText);
                            gParent.AllocationDate = DateTime.Parse(cellText);
                            gParent.NirvanaProcessDate = DateTime.Parse(cellText);
                            gParent.UpdateTaxLotDataforDate(gParent);
                            AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeDate_Changed);
                            AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                            AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.ProcessDate_Changed);
                        }
                        break;
                    case AllocationConstants.COL_CHANGECOMMENT: break;
                    default:
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.TradeEdited);
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool UpdateFieldsForAllocationGroupCellChange(string columnKey, string cellText, bool isTotalCommissionChanged, AllocationGroup gParent)
        {
            try
            {
                gParent.UpdateGroupPersistenceStatus();
                gParent.IsModified = true;

                CommissionFields commFields = null;
                TradeAttributes tradeAttr = null;
                switch (columnKey)
                {
                    case OrderFields.PROPERTY_COMMISSION:
                        gParent.Commission = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.Commission = gParent.Commission;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        //TODO: Fine reason of following line, which is dependent on ultragrid
                        //e.Cell.Row.Refresh(RefreshRow.FireInitializeRow, true);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.Commission_Changed);
                        break;

                    case OrderFields.PROPERTY_SETTLEMENTCURRENCYID:
                        gParent.SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cellText);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        gParent.UpdateSettlementCurrencyInTaxlots(gParent);
                        break;

                    case OrderFields.PROPERTY_SettCurrFXRate:
                        gParent.SettlCurrFxRate = Convert.ToDouble(cellText);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        gParent.UpdateSettlementFxRateInTaxlots(gParent);
                        if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() == gParent.SettlementCurrencyID)
                            gParent.UpdateFXRateAndFXOperator();
                        break;

                    case OrderFields.PROPERTY_SettCurrFXRateCalc:
                        gParent.SettlCurrFxRateCalc = cellText;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.UpdateSettlementFxRateOperatorInTaxlots(gParent);
                        if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() == gParent.SettlementCurrencyID)
                            gParent.UpdateFXRateAndFXOperator();
                        break;

                    case OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT:
                        gParent.SettlCurrAmt = Convert.ToDouble(cellText);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                        gParent.UpdateSettlementPriceInTaxlots(gParent);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() == gParent.SettlementCurrencyID)
                            gParent.UpdateFXRateAndFXOperator();
                        break;

                    case OrderFields.PROPERTY_SOFTCOMMISSION:
                        gParent.SoftCommission = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.SoftCommission = gParent.SoftCommission;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        //e.Cell.Row.Refresh(RefreshRow.FireInitializeRow, true);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SoftCommission_Changed);
                        break;

                    case OrderFields.PROPERTY_OTHERBROKERFEES:
                        gParent.OtherBrokerFees = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.OtherBrokerFees = gParent.OtherBrokerFees;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                        break;

                    case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                        gParent.ClearingBrokerFee = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.ClearingBrokerFee = gParent.ClearingBrokerFee;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                        break;

                    case OrderFields.PROPERTY_STAMPDUTY:
                        gParent.StampDuty = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.StampDuty = gParent.StampDuty;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.StampDuty_Changed);
                        break;

                    case OrderFields.PROPERTY_TRANSACTIONLEVY:
                        gParent.TransactionLevy = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.TransactionLevy = gParent.TransactionLevy;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                        break;

                    case OrderFields.PROPERTY_CLEARINGFEE:
                        gParent.ClearingFee = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.ClearingFee = gParent.ClearingFee;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.ClearingFee_Changed);
                        break;

                    case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                        gParent.TaxOnCommissions = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.TaxOnCommissions = gParent.TaxOnCommissions;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                        break;

                    case OrderFields.PROPERTY_MISCFEES:
                        gParent.MiscFees = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.MiscFees = gParent.MiscFees;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.MiscFees_Changed);
                        break;

                    case OrderFields.PROPERTY_OptionPremiumAdjustment:
                        gParent.OptionPremiumAdjustment = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.OptionPremiumAdjustment = gParent.OptionPremiumAdjustment;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.OptionPremiumAdjustment_Changed);
                        break;

                    case OrderFields.PROPERTY_SECFEE:
                        gParent.SecFee = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.SecFee = gParent.SecFee;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.SecFee_Changed);
                        break;

                    case OrderFields.PROPERTY_OCCFEE:
                        gParent.OccFee = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.OccFee = gParent.OccFee;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.OccFee_Changed);
                        break;

                    case OrderFields.PROPERTY_ORFFEE:
                        gParent.OrfFee = double.Parse(cellText);
                        commFields = new CommissionFields();
                        commFields.OrfFee = gParent.OrfFee;
                        gParent.UpdateTaxlotCommissionAndFees(commFields);
                        isTotalCommissionChanged = true;
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.OrfFee_Changed);
                        break;

                    case AllocationConstants.COL_AvgPrice:
                        if (double.Parse(cellText) < 0)
                        {
                            //e.Cell.CancelUpdate();
                        }
                        gParent.AvgPrice = double.Parse(cellText);
                        gParent.UpdateTaxlotAvgPrice();
                        isTotalCommissionChanged = true;
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                        gParent.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
                        break;

                    case AllocationConstants.COL_ACCRUEDINTEREST:
                        gParent.AccruedInterest = double.Parse(cellText);
                        gParent.UpdateTaxlotAccruedInterest();
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        gParent.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        break;

                    case AllocationConstants.COL_DESCRIPTION:
                        gParent.Description = cellText;
                        gParent.UpdateTaxlotDescription();
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.Description_Changed);
                        gParent.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Description_Changed);
                        break;

                    case AllocationConstants.COL_INTERNALCOMMENTS:
                        gParent.InternalComments = cellText;
                        gParent.UpdateTaxlotIneternalComments();
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.InternalComments_Changed);
                        gParent.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.InternalComments_Changed);
                        break;

                    case AllocationConstants.COL_TradeAttribute1:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute1 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_TradeAttribute2:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute2 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_TradeAttribute3:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute3 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_TradeAttribute4:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute4 = cellText;
                        //UpdateDataforTradeAttributes(gParent);
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_TradeAttribute5:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute5 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_TradeAttribute6:
                        tradeAttr = UpdateDataforTradeAttributes(gParent, string.Empty);
                        tradeAttr.TradeAttribute6 = cellText;
                        gParent.UpdateTaxlotTradeAttributes(tradeAttr);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_CounterPartyName:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_FXRate:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.FxRate_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.FxRate_Changed);
                        break;

                    case AllocationConstants.COL_FXConversionMethodOperator:
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(gParent, TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        break;

                    case AllocationConstants.COL_UNDERLYINGDELTA:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.UnderlyingDelta_Changed);
                        break;

                    case AllocationConstants.COL_COMMISSIONAMOUNT:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.CommissionAmount_Changed);
                        break;

                    case AllocationConstants.COL_COMMISSIONRATE:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.CommissionRate_Changed);
                        break;

                    case AllocationConstants.COL_SOFTCOMMISSIONAMOUNT:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommissionAmount_Changed);
                        break;

                    case AllocationConstants.COL_SOFTCOMMISSIONRATE:
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommissionRate_Changed);
                        break;

                    case AllocationConstants.COL_TransactionType:
                        gParent.UpdateTaxlotTransactionType(cellText);
                        gParent.IsAnotherTaxlotAttributesUpdated = true;
                        gParent.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isTotalCommissionChanged;
        }

        public TradeAttributes UpdateDataforTradeAttributes(AllocationGroup gParent, string taxLotID)
        {
            TradeAttributes tradeAttr = new TradeAttributes();
            try
            {
                if (taxLotID == string.Empty)
                {
                    tradeAttr.TradeAttribute1 = gParent.TradeAttribute1;
                    tradeAttr.TradeAttribute2 = gParent.TradeAttribute2;
                    tradeAttr.TradeAttribute3 = gParent.TradeAttribute3;
                    tradeAttr.TradeAttribute4 = gParent.TradeAttribute4;
                    tradeAttr.TradeAttribute5 = gParent.TradeAttribute5;
                    tradeAttr.TradeAttribute6 = gParent.TradeAttribute6;
                }
                else
                {
                    foreach (TaxLot taxlot in gParent.TaxLots)
                    {
                        if (taxlot.TaxLotID == taxLotID)
                        {
                            tradeAttr.TradeAttribute1 = taxlot.TradeAttribute1;
                            tradeAttr.TradeAttribute2 = taxlot.TradeAttribute2;
                            tradeAttr.TradeAttribute3 = taxlot.TradeAttribute3;
                            tradeAttr.TradeAttribute4 = taxlot.TradeAttribute4;
                            tradeAttr.TradeAttribute5 = taxlot.TradeAttribute5;
                            tradeAttr.TradeAttribute6 = taxlot.TradeAttribute6;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return tradeAttr;

        }

        public void UpdateFieldsForTaxLotAfterCellUpdate(string columnKey, string updatedValue, AllocationGroup allGroup)
        {
            try
            {
                CommissionFields commFields = null;
                switch (columnKey)
                {
                    case OrderFields.PROPERTY_COMMISSION:
                        //taxlot.Commission = updatedValue;
                        commFields = new CommissionFields();
                        commFields.Commission = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_SOFTCOMMISSION:
                        commFields = new CommissionFields();
                        commFields.SoftCommission = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_OTHERBROKERFEES:
                        //taxlot.OtherBrokerFees = updatedValue;
                        commFields = new CommissionFields();
                        commFields.OtherBrokerFees = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                        commFields = new CommissionFields();
                        commFields.ClearingBrokerFee = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_TRANSACTIONLEVY:
                        //taxlot.TransactionLevy = updatedValue;
                        commFields = new CommissionFields();
                        commFields.TransactionLevy = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_MISCFEES:
                        //taxlot.MiscFees = updatedValue;
                        commFields = new CommissionFields();
                        commFields.MiscFees = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                        //taxlot.TaxOnCommissions = updatedValue;
                        commFields = new CommissionFields();
                        commFields.TaxOnCommissions = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_CLEARINGFEE:
                        //taxlot.ClearingFee = updatedValue;
                        commFields = new CommissionFields();
                        commFields.ClearingFee = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_STAMPDUTY:
                        //taxlot.StampDuty = updatedValue;
                        commFields = new CommissionFields();
                        commFields.StampDuty = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_SECFEE:
                        commFields = new CommissionFields();
                        commFields.SecFee = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_OCCFEE:
                        commFields = new CommissionFields();
                        commFields.OccFee = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;
                    case OrderFields.PROPERTY_ORFFEE:
                        commFields = new CommissionFields();
                        commFields.OrfFee = double.Parse(updatedValue);
                        allGroup.UpdateGroupCommissionAndFees(commFields);
                        allGroup.UpdateGroupPersistenceStatus();
                        break;

                    //case COL_TradeAttribute1:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute1 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    //case COL_TradeAttribute2:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute2 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    //case COL_TradeAttribute3:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute3 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    //case COL_TradeAttribute4:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute4 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    //case COL_TradeAttribute5:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute5 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    //case COL_TradeAttribute6:
                    //    tradeAttr = new TradeAttributes();
                    //    tradeAttr.TradeAttribute6 = e.Cell.Text;
                    //    allGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                    //    allGroup.UpdateGroupPersistenceStatus();
                    //    break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// For the passed taxlotid's update old values with the new values 
        /// </summary>
        /// <param name="dtTaxLotsAmended">datatable _dtTaxLotsAmended have taxlotid, columnname, old value and new value</param>
        public void MakeNewCancelAmendChanges(Dictionary<string, List<ApprovedChanges>> dictApprovedChanges)
        {
            try
            {
                List<string> lstTaxlotIDs = new List<string>(dictApprovedChanges.Keys);
                if (lstTaxlotIDs.Count > 0)
                {
                    List<AllocationGroup> lstGroups = GetGroups(lstTaxlotIDs, true);
                    List<AllocationGroup> lstGroupsToDelete = new List<AllocationGroup>();
                    foreach (AllocationGroup group in lstGroups)
                    {
                        List<ApprovedChanges> lstApprovedChanges = dictApprovedChanges[group.TaxLots[0].TaxLotID];

                        SetDefaultPersistenceStatus(group);
                        DictUnsavedAdd(group.GroupID, group);
                        AllocationManager.GetInstance().AddGroup(group);
                        string taxLotID = group.TaxLots[0].TaxLotID;
                        foreach (ApprovedChanges approvedChanges in lstApprovedChanges)
                        {
                            if (approvedChanges.TaxlotStatus.Equals(AmendedTaxLotStatus.Deleted))
                            {
                                //Taxlots which have approval status deleted, will be deleted in two steps.
                                //1. Unallocate all the groups
                                //2. Delete the groups
                                lstGroupsToDelete.Add(group);
                            }
                            else
                            {

                                string columnName = approvedChanges.ColumnName;
                                bool isTotalCommissionChanged = false;
                                switch (columnName)
                                {
                                    case "Quantity":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.CAPTION_TaxlotQty, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        break;
                                    case "TradeDate":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.COl_AUECLocalDate, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        break;
                                    case "AvgPX":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.COL_AvgPrice, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(AllocationConstants.COL_AvgPrice, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "Commission":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_COMMISSION, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_COMMISSION, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "Fees":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_OTHERBROKERFEES, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_OTHERBROKERFEES, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "ClearingFee":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_CLEARINGFEE, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_CLEARINGFEE, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "FXRate":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_FXRate, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_FXRate, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "SoftCommission":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_SOFTCOMMISSION, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_SOFTCOMMISSION, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "FXConversionMethodOperator":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.COL_FXConversionMethodOperator, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(AllocationConstants.COL_FXConversionMethodOperator, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "ClearingBrokerFee":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_CLEARINGBROKERFEE, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_CLEARINGBROKERFEE, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "StampDuty":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_STAMPDUTY, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_STAMPDUTY, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "TransactionLevy":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_TRANSACTIONLEVY, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_TRANSACTIONLEVY, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "MiscFees":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_MISCFEES, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_MISCFEES, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "TaxOnCommissions":
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_TAXONCOMMISSIONS, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_TAXONCOMMISSIONS, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "LotID":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.COL_LotID, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(AllocationConstants.COL_LotID, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case "ExternalTransID":
                                        UpdateFieldsForTaxLotCellChange(AllocationConstants.COL_ExternalTransId, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(AllocationConstants.COL_ExternalTransId, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case OrderFields.PROPERTY_SettCurrFXRate:
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_SettCurrFXRate, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_SettCurrFXRate, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case OrderFields.PROPERTY_SettCurrFXRateCalc:
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_SettCurrFXRateCalc, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_SettCurrFXRateCalc, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                    case OrderFields.PROPERTY_SETTLEMENTCURRENCY:
                                        if (CachedDataManager.GetInstance.GetAllCurrencies().ContainsValue(approvedChanges.NewValue))
                                        {
                                            approvedChanges.OldValue = CachedDataManager.GetInstance.GetAllCurrencies().Where(x => x.Value == approvedChanges.OldValue).First().Key.ToString();
                                            approvedChanges.NewValue = CachedDataManager.GetInstance.GetAllCurrencies().Where(x => x.Value == approvedChanges.NewValue).First().Key.ToString();
                                            UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_SETTLEMENTCURRENCY, approvedChanges.NewValue, group, group.TaxLots[0]);
                                            isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_SETTLEMENTCURRENCY, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        }
                                        else
                                        {
                                            throw new Exception("Currency '" + approvedChanges.NewValue + "' not found in system. ");
                                        }
                                        break;
                                    case OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT:
                                        UpdateFieldsForTaxLotCellChange(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT, approvedChanges.NewValue, group, group.TaxLots[0]);
                                        isTotalCommissionChanged = UpdateFieldsForAllocationGroupCellChange(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT, approvedChanges.NewValue, isTotalCommissionChanged, group);
                                        break;
                                }
                            }
                        }
                    }
                    //Taxlots which have approval status deleted, will be deleted in two steps.
                    //1. Unallocate all the groups
                    //2. Delete the groups
                    if (lstGroupsToDelete.Count > 0)
                    {
                        //1. First step- unallocate group and save unalloxcated data
                        UnAllocateGroup(lstGroupsToDelete);

                        SaveGroups();
                        //2. Delete unallocated groups
                        foreach (AllocationGroup deleteGroup in lstGroupsToDelete)
                        {
                            DeleteGroupCancelAmendUI(deleteGroup);
                        }
                        SaveGroups();
                        ClearData();
                        ClearDictionaryUnsaved();
                    }
                    else
                    {
                        SaveGroups();
                        ClearData();
                        ClearDictionaryUnsaved();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DictUnsavedAdd(string GroupID, AllocationGroup ag)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    if (!_dictunsavedCancelAmend.ContainsKey(GroupID))
                    {
                        _dictunsavedCancelAmend.Add(GroupID, ag);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public int DictUnsavedCount()
        {
            int count = 0;
            try
            {
                lock (lockerAllocationSave)
                {
                    count = _dictunsavedCancelAmend.Count;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return count;
        }

        public bool DictUnsavedContainsKey(string keyGroupId)
        {
            bool ret = false;
            try
            {
                lock (lockerAllocationSave)
                {
                    if (!string.IsNullOrWhiteSpace(keyGroupId))
                        ret = _dictunsavedCancelAmend.ContainsKey(keyGroupId);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ret;
        }

        /// <summary>
        /// Clears the whole dictionary Unsaved which stores the clone of all edited groups
        /// </summary>
        public void ClearDictionaryUnsaved()
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    _dictunsavedCancelAmend.Clear();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes the group with specific groupId from dictionary Unsaved which stores the clone of all edited groups
        /// </summary>
        /// <param name="groupId"></param>
        public void ClearDictionaryUnsaved(string groupId)
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    if (_dictunsavedCancelAmend.ContainsKey(groupId))
                    {
                        _dictunsavedCancelAmend.Remove(groupId);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves the entry in Audit Trail for any changes done in the Group
        /// </summary>
        /// <param name="group"></param>
        public void SaveEditedGroupsAuditEntry(Dictionary<string, AllocationGroup> dictUnsavedAuditLists)
        {
            try
            {
                foreach (KeyValuePair<string, AllocationGroup> kvp in dictUnsavedAuditLists)
                {
                    AllocationGroup agOriginal = null;
                    lock (lockerAllocationSave)
                    {
                        if (_dictunsavedCancelAmend.ContainsKey(kvp.Key))
                        {
                            agOriginal = _dictunsavedCancelAmend[kvp.Key];
                        }
                        else
                        {
                            Logger.Write("Error in saving Audit Trail while saving data.");
                            continue;
                        }
                    }
                    foreach (TradeAuditActionType.ActionType action in kvp.Value.TradeActionsList)
                    {
                        string originalValue = TradeAuditActionType.GetColumnValue(action, agOriginal);
                        AuditManager.Instance.AddGroupToAuditEntry(kvp.Value, false, DateTime.UtcNow, action, originalValue, kvp.Value.ChangeComment, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
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
                                        AuditManager.Instance.AddRowTradeAuditEntry(tax, DateTime.UtcNow, kvp.Key, action, TradeAuditActionType.GetColumnValue(action, taxOriginal), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void CancelEditChanges()
        {
            try
            {
                lock (lockerAllocationSave)
                {
                    foreach (AllocationGroup group in _dictunsavedCancelAmend.Values)
                    {
                        //PostTradeCacheManager.Remove(group.GroupID);
                        Remove(group.GroupID);
                        //PostTradeCacheManager.RemoveAllocated(group);
                        RemoveAllocated(group);
                        //PostTradeCacheManager.RemoveUnAllocated(group);
                        RemoveUnAllocated(group);
                        //PostTradeCacheManager.AddGroup(_dictunsaved[group]);
                        AddGroup(group);
                        //PostTradeCacheManager.ClearDeletedOmiitedGroups();
                        ClearDeletedOmiitedGroups();
                        ResetTheResetDictionary(group);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DeleteGroupCancelAmendUI(AllocationGroup deleteGroup)
        {
            try
            {
                //TODO: Use this userid from common global cache
                int userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                DictUnsavedAdd(deleteGroup.GroupID, (AllocationGroup)deleteGroup.Clone());
                if (String.IsNullOrEmpty(deleteGroup.ChangeComment))
                {
                    AuditManager.Instance.AddGroupToAuditEntry(deleteGroup, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.DELETE, "", "Group deleted", userID);
                }
                else
                {
                    AuditManager.Instance.AddGroupToAuditEntry(deleteGroup, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.DELETE, "", deleteGroup.ChangeComment, userID);
                }

                //delete status is already set in save groups
                //deleteGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                AddDeletedOmiitedGroups(deleteGroup);

                //We are removing group from unAllocated dictionary in save method
                //for Prana mode we have to remove group from unAllocated group cache so that group disappear from UI
                if (!CachedDataManager.GetPranaReleaseType().Equals(PranaReleaseViewType.CHMiddleWare))
                {
                    lock (lockerAllocationSave)
                    {
                        _unAllocatedGroups.Remove(deleteGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroup);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_AllocationPreferenceUpdated);
                    _proxy.Dispose();
                }           
                    if (_allocationPreferences != null)
                        _allocationPreferences = null;
                    if (_allocationServices != null)
                    {
                        _allocationServices.Dispose();
                        _allocationServices = null;
                    }
                    if (_cashManagementServices != null)
                    {
                        _cashManagementServices.Dispose();
                        _cashManagementServices = null;
                    }
                    if (_closingServices != null)
                    {
                        _closingServices.Dispose();
                        _closingServices = null;
                    }
                    if (_deletedGroups != null)
                        _deletedGroups = null;
                    if (_deletedOmittedGroups != null)
                        _deletedOmittedGroups = null;
                    if (_dictGroups != null)
                        _dictGroups = null;
                    if (_dictunderlyingExercisedGroups != null)
                        _dictunderlyingExercisedGroups = null;
                    if (_dictunsavedCancelAmend != null)
                        _dictunsavedCancelAmend = null;
                    if (_lstAccountLocks != null)
                        _lstAccountLocks = null;
                    if (_pranaPositionServices != null)
                    {
                        _pranaPositionServices.Dispose();
                        _pranaPositionServices = null;
                    }
                    if (_unAllocatedGroups != null)
                        _unAllocatedGroups = null;
                }
            
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddAccountIdToLockData(int accountID)
        {
            if (!checkForAccountLock(accountID))
            {
                LstAccountLocks.Add(accountID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>true=account lock acquired</returns>
        public bool checkForAccountLock(int accountID)
        {
            if (LstAccountLocks.Contains(accountID))
                return true;
            else
                return false;
        }

        public List<string>[] getAttributeList()
        {
            return _allocation.InnerChannel.GetTradeAttributes();
        }

        #region Allocation Operation Preference

        /// <summary>
        /// Allocate group
        /// </summary>
        /// <param name="allocationGroupList"></param>
        /// <param name="pref"></param>
        /// <param name="isReallocate"></param>
        /// <param name="isChanged"></param>
        public string AllocateGroup(List<AllocationGroup> allocationGroupList, AllocationOperationPreference pref, bool isReallocate, bool isChanged, bool forceAllocation)
        {
            try
            {
                List<AllocationGroup> allocationList = new List<AllocationGroup>();
                AllocationResponse response = new AllocationResponse();
                int userId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (isReallocate)
                {
                    AllocationRule rule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, PreferenceAccountId = -1, MatchPortfolioPosition = false };
                    response = _allocation.InnerChannel.AllocateByParameter(allocationGroupList, new AllocationParameter(rule, pref.TargetPercentage, -1, userId, true), forceAllocation);
                    if (response != null && response.GroupList != null)
                        allocationList.AddRange(response.GroupList);
                    foreach (AllocationGroup group in allocationGroupList)
                    {
                        if (response.Response == null)
                        {
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.REALLOCATE, "", "Trade Reallocated Taxlots Deleted", currentCompanyUser.CompanyUserID);
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Reallocated", CurrentCompanyUser.CompanyUserID);
                        }
                    }
                }
                else
                {
                    if (isChanged)
                    {
                        response = _allocation.InnerChannel.AllocateByParameter(allocationGroupList, new AllocationParameter(pref.DefaultRule, pref.TargetPercentage, -1, userId, true), forceAllocation);
                        if (response != null && response.GroupList != null)
                            allocationList.AddRange(response.GroupList);
                    }
                    else
                    {
                        response = _allocation.InnerChannel.AllocateByPreference(allocationGroupList, pref.OperationPreferenceId, userId, false, false, forceAllocation);
                        if (response != null && response.GroupList != null)
                            allocationList.AddRange(response.GroupList);
                    }
                    foreach (AllocationGroup group in allocationGroupList)
                    {
                        if (response.Response == null)
                        {
                            AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated from Unallocate", CurrentCompanyUser.CompanyUserID);
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated New Taxlots Created", currentCompanyUser.CompanyUserID);
                        }
                    }
                }
                foreach (AllocationGroup group in allocationList)
                {
                    AddGroup(group);
                    //In case of Reallocation of Allocated Trade,the number of entries on AuditTrail for new Taxlots created will be for the updated no. of Taxlots. 
                    if (isReallocate)
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group ReAllocated Taxlots Created", currentCompanyUser.CompanyUserID);
                    else
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated New Taxlots Created", currentCompanyUser.CompanyUserID);
                }
                return response.Response;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return "Error in allocating";
            }
        }

        /// <summary>
        /// Unallocate group using Allocation manager service
        /// </summary>
        /// <param name="groupList"></param>
        public void UnAllocateGroup(List<AllocationGroup> groupList)
        {
            try
            {
                List<AllocationGroup> groups = _allocation.InnerChannel.UnAllocateGroup(groupList, CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                foreach (AllocationGroup group in groups)
                {
                    AddGroup(group);
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                    {
                        //Trades are deleted on clicking delete button
                        //For ch user we are deleting group and taxlots along with unallocate
                        //As there are post allocated trades and user need to delete trades in one step (not unallocate and delete)
                        DeleteGroupCancelAmendUI(group);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Validate group
        /// </summary>
        /// <param name="allocationGroupList"></param>
        /// <param name="pref"></param>
        /// <param name="isReallocate"></param>
        /// <param name="isChanged"></param>
        public AllocationResponse PreviewAllocation(List<AllocationGroup> allocationGroupList, AllocationOperationPreference pref, bool isReallocate, bool isChanged, bool forceAllocation)
        {
            try
            {
                List<AllocationGroup> allocationList = new List<AllocationGroup>();
                AllocationResponse response = new AllocationResponse();
                int userId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (isReallocate)
                {
                    foreach (AllocationGroup group in allocationGroupList)
                        AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, DateTime.UtcNow, Prana.BusinessObjects.TradeAuditActionType.ActionType.REALLOCATE, "", "Trade Reallocated Taxlots Deleted", currentCompanyUser.CompanyUserID);
                    AllocationRule rule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, PreferenceAccountId = -1, MatchPortfolioPosition = false };
                    response = _allocation.InnerChannel.AllocateByParameter(allocationGroupList, new AllocationParameter(rule, pref.TargetPercentage, -1, userId, true, true), forceAllocation);


                    //foreach (AllocationGroup group in allocationGroupList)
                    //{
                    //AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Reallocated", CurrentCompanyUser.CompanyUserID);
                    //AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group ReAllocated Taxlots Created", currentCompanyUser.CompanyUserID);
                    //}
                }
                else
                {
                    if (isChanged)
                    {
                        response = _allocation.InnerChannel.AllocateByParameter(allocationGroupList, new AllocationParameter(pref.DefaultRule, pref.TargetPercentage, -1, userId, false, true), forceAllocation);

                    }
                    else
                    {
                        response = _allocation.InnerChannel.AllocateByPreference(allocationGroupList, pref.OperationPreferenceId, userId, false, true, forceAllocation);

                    }
                    //foreach (AllocationGroup group in allocationGroupList)
                    //{
                    //AuditManager.Instance.AddGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated from Unallocate", CurrentCompanyUser.CompanyUserID);
                    //AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, false, DateTime.UtcNow, TradeAuditActionType.ActionType.REALLOCATE, "", "Group Allocated New Taxlots Created", currentCompanyUser.CompanyUserID);
                    //}
                }
                return response;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        #endregion
        //Modified By faisal Shah
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3420
        /// <summary>
        /// Validate Trade For Account NAV Lock on saving trade after making changes 
        /// Created By: Omshiv, 15 may 2014
        /// </summary>
        /// <returns></returns>
        private bool ValidateTradeForAccountNAVLock(List<AllocationGroup> groups)
        {
            bool isProcessToSave = true;
            try
            {
                #region NAV lock validation - modified by Omshiv, MArch 2014
                //get IsNAVLockingEnabled or not from cache
                // Modifed By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3588
                //if (_releaseType == PranaReleaseViewType.CHMiddleWare)
                //{
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (AllocationGroup allocationGrp in groups)
                    {
                        //AllocationGroup allocationGrp = row.ListObject as AllocationGroup;
                        if (allocationGrp != null && allocationGrp.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            foreach (TaxLot taxlot in allocationGrp.TaxLots)
                            {

                                //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                                //if (taxlot.Level1ID != null && taxlot.Level1ID != 0) // commented old if as int is never null
                                if (taxlot.Level1ID != 0)
                                {

                                    DateTime tradeDate = taxlot.OriginalPurchaseDate;
                                    isProcessToSave = Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(taxlot.Level1ID, tradeDate);
                                    if (!isProcessToSave)
                                    {
                                        string accountname = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                                        MessageBox.Show("NAV is locked for the account:" + accountname + "\n You can not make changes for locked account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// this method calculates commission for group when counter party is changed
        /// </summary>
        /// <param name="group">the allocation group</param>
        internal static AllocationGroup CalculateCommission(AllocationGroup group)
        {
            AllocationGroup allocationGroup = new AllocationGroup();
            try
            {
                allocationGroup = _allocation.InnerChannel.CalculateCommission(group);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroup;
        }

        /// <summary>
        /// Get master fund target ratio
        /// </summary>
        /// <returns></returns>
        internal DataSet GetAllMasterFunds()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = _allocation.InnerChannel.GetAllMasterFunds();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return ds;
        }
    }
}

