using Prana.BusinessObjects;
using Prana.BusinessObjects.GreenFieldModels;
using Prana.CommonDatabaseAccess;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.CommonDataCache
{
    public class UserCachedData : IDisposable
    {
        AllocationDefaultCollection _allocationDefaultCollection = null;
        string _applicationVersion = null;
        TradingAccountCollection tradingAccounts = null;
        AccountCollection _useraccounts = null;
        CounterPartyCollection _userCounterParties;
        StrategyCollection strategies = null;
        DataTable companyComplianceBorrowers = null;
        List<string> _permittedAUECCV = new List<string>();
        Dictionary<int, DataTable> _permissionSideBasedOnAsset = new Dictionary<int, DataTable>();
        DataTable _permissionOrderSide = new DataTable();
        DataTable _permissionOrderType = new DataTable();
        DataTable _permissionHandlingInst = new DataTable();
        DataTable _permissionExecutionInst = new DataTable();
        DataTable _permissionTIF = new DataTable();
        DataTable _accountsAndAllocationRules = new DataTable();
        Dictionary<int, List<int>> _companyAccounts = new Dictionary<int, List<int>>();
        Dictionary<int, string> _userMasterFunds = new Dictionary<int, string>();
        Dictionary<int, List<int>> _masterFundSubAccountAssociation = new Dictionary<int, List<int>>();
        List<int> _userPermittedMasterFundsBasedOnFunds = new List<int>();
        Dictionary<int, string> _masterFunds = new Dictionary<int, string>();
        private TranferTradeRules _transferTradeRules;
        int _userID = int.MinValue;
        bool _isCommonDataCreated = false;
        private IClientsCommonDataManager _clientsCommonDataManager;
        private IKeyValueDataManager _keyValueDataManager;
        private IList<MasterFundAccountDetails> _userCustomGroups = new List<MasterFundAccountDetails>();

        public IList<MasterFundAccountDetails> UserCustomGroups
        {
            get => _userCustomGroups;
            set => _userCustomGroups = value;
        }



        public Dictionary<int, string> GetUserAccountsAsDict()
        {
            Dictionary<int, string> convertedAccounts = new Dictionary<int, string>();
            try
            {
                if (_useraccounts != null)
                {
                    //int i = 0;
                    foreach (Prana.BusinessObjects.Account account in _useraccounts)
                    {
                        //6281
                        if (!convertedAccounts.ContainsValue(account.Name.ToString()) && account.AccountID != int.MinValue)
                        {
                            convertedAccounts.Add(account.AccountID, account.Name.ToString());
                        }
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
            return convertedAccounts;
        }

        public Dictionary<int, string> GetUserCounterParties()
        {
            Dictionary<int, string> userCounterparties = new Dictionary<int, string>();
            foreach (CounterParty cp in _userCounterParties)
            {
                userCounterparties.Add(cp.CounterPartyID, cp.Name);
            }
            return userCounterparties;
        }

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
        }
        public TradingAccountCollection UserTradingAccounts
        {
            get { return tradingAccounts; }
        }

        public AccountCollection UserAccounts
        {
            get { return _useraccounts; }
        }

        public CounterPartyCollection UserCounterParties
        {
            get { return _userCounterParties; }
        }

        public StrategyCollection UserStrategies
        {
            get { return strategies; }
        }
        public DataTable UserComplianceBorrowers
        {
            get { return companyComplianceBorrowers; }
        }

        public List<string> PermittedAUECCV
        {
            get
            {
                return _permittedAUECCV;
            }
        }

        public bool IsTradingPermitted(int AUECID, int counterPartyID, int VenueID)
        {
            string key = GetAUECIDKey(AUECID, counterPartyID, VenueID);
            return _permittedAUECCV.Contains(key);
        }

        public string GetAUECIDKey(int AUECID, int counterPartyID, int VenueID)
        {
            string key = "AUEC" + AUECID.ToString() + ":C" + counterPartyID.ToString() + ":V" + VenueID.ToString();
            return key;

        }
        public Dictionary<int, DataTable> PermissionSideBasedOnAsset
        {
            get { return _permissionSideBasedOnAsset; }
        }
        public DataTable PermissionOrderSide
        {
            get { return _permissionOrderSide; }
        }
        public DataTable PermissionOrderType
        {
            get { return _permissionOrderType; }
        }
        public DataTable PermissionHandlingInst
        {
            get { return _permissionHandlingInst; }
        }
        public DataTable PermissionExecutionInst
        {
            get { return _permissionExecutionInst; }
        }
        public DataTable PermissionTIF
        {
            get { return _permissionTIF; }
        }
        private List<string> _accountNamesForUser = new List<string>();

        public List<string> AccountNamesForUser
        {
            get { return _accountNamesForUser; }
            set { _accountNamesForUser = value; }
        }

        public Dictionary<int, List<int>> CompanyAccounts
        {
            get { return _companyAccounts; }
        }
        public Dictionary<int, string> UserMasterFunds
        {
            get { return _userMasterFunds; }
            set { _userMasterFunds = value; }
        }
        public List<AllocationDefault> AllocationDefaultCollection
        {
            get { return _allocationDefaultCollection.GetDefaults(); }

        }

        public List<int> UserPermittedMasterFundsBasedOnFunds
        {
            get { return _userPermittedMasterFundsBasedOnFunds; }
        }

        readonly object lockerAllocDefaults = new object();

        public void SetDefaults(List<AllocationDefault> allocationDefaultList)
        {
            lock (lockerAllocDefaults)
            {
                _allocationDefaultCollection.SetDefaults(allocationDefaultList);
                FillAccountAndAllocationDefaultsDataTable();
            }
        }

        public TranferTradeRules TranferTradeRules
        {
            get { return _transferTradeRules; }
        }

        public UserCachedData(int companyUserID)
        {
            _userID = companyUserID;
            _clientsCommonDataManager = WindsorContainerManager.Container.Resolve<IClientsCommonDataManager>();
            _keyValueDataManager = WindsorContainerManager.Container.Resolve<IKeyValueDataManager>();
        }

        public DataTable GetAccountsAndAllocationRules()
        {
            return _accountsAndAllocationRules;
        }
        public void StartCaching(int companyID)
        {
            if (!_isCommonDataCreated)
            {
                //Data is ApplicationConstants here not specific to any User
                _isCommonDataCreated = true;
            }
            try
            {
                _allocationDefaultCollection = new AllocationDefaultCollection();
                _applicationVersion = _clientsCommonDataManager.GetApplicationVersionFromDB();
                tradingAccounts = _clientsCommonDataManager.GetTradingAccounts(_userID);

                _useraccounts = _clientsCommonDataManager.GetAccounts(_userID);
                _userCounterParties = _clientsCommonDataManager.GetCompanyUserCounterParties(_userID);
                _useraccounts.Insert(0, new Prana.BusinessObjects.Account(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                strategies = _clientsCommonDataManager.GetStrategies(_userID);

                strategies.Insert(0, new Prana.BusinessObjects.Strategy(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                if (strategies.IndexOf(0) < 0)
                    strategies.Insert(1, new Strategy(0, "Strategy Unallocated"));

                companyComplianceBorrowers = _clientsCommonDataManager.GetCompanyBorrowers(_userID);
                companyComplianceBorrowers.Rows.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

                _permissionSideBasedOnAsset.Clear();
                _permissionOrderSide.Clear();
                _permissionOrderType.Clear();
                _permissionHandlingInst.Clear();
                _permissionExecutionInst.Clear();
                _permissionTIF.Clear();
                _permittedAUECCV.Clear();
                _accountNamesForUser = _clientsCommonDataManager.GetAccountsForTheUser(_userID);


                DataSet tradingPermissionsDS = _keyValueDataManager.GetAUECandCVPermissionsForUser(_userID, companyID);

                for (int i = 0; i < tradingPermissionsDS.Tables.Count; i++)
                {
                    DataTable tempDT = tradingPermissionsDS.Tables[i];

                    switch (i)
                    {

                        case OrderFields.DS_INDEX_PERMITTED_AUCECV:

                            _permittedAUECCV = tempDT.AsEnumerable()
                              .Select(row => row[0].ToString())
                               .ToList();


                            break;

                        case OrderFields.DS_INDEX_ORDER_SIDE:
                            _permissionOrderSide = new DataTable();
                            _permissionOrderSide.Columns.Add(OrderFields.PROPERTY_ORDER_SIDEID);
                            _permissionOrderSide.Columns.Add(OrderFields.PROPERTY_ORDER_SIDE);
                            foreach (DataRow dr in tempDT.Rows)
                            {
                                int assetKey = int.Parse(dr[0].ToString());

                                if (_permissionSideBasedOnAsset.ContainsKey(assetKey))
                                {
                                    _permissionSideBasedOnAsset[assetKey] = _clientsCommonDataManager.GetSidesByAsset(assetKey);
                                }
                                else
                                {
                                    DataTable keyWiseTable = _clientsCommonDataManager.GetSidesByAsset(assetKey);
                                    _permissionSideBasedOnAsset.Add(assetKey, keyWiseTable);

                                }
                            }
                            foreach (DataTable keyWiseTable in _permissionSideBasedOnAsset.Values)
                            {
                                foreach (DataRow drow in keyWiseTable.Rows)
                                {
                                    bool isOrderSidePresent = _permissionOrderSide.AsEnumerable().Any(row => drow[0].ToString() == row.Field<String>(OrderFields.PROPERTY_ORDER_SIDEID));
                                    if (!isOrderSidePresent)
                                    {
                                        _permissionOrderSide.Rows.Add(drow[0], drow[1]);
                                    }
                                }
                            }
                            break;

                        case OrderFields.DS_INDEX_ORDER_TYPE:
                            _permissionOrderType = new DataTable();
                            _permissionOrderType.Columns.Add(OrderFields.PROPERTY_ORDER_TYPE_ID);
                            _permissionOrderType.Columns.Add(OrderFields.PROPERTY_ORDER_TYPE);
                            foreach (DataRow dr in tempDT.Rows)
                            {
                                string key = dr[1].ToString();
                                _permissionOrderType.Rows.Add(new object[] { key, TagDatabaseManager.GetInstance.GetOrderTypeTextBasedOnID(key) });
                            }

                            break;

                        case OrderFields.DS_INDEX_ORDER_HANDLINGINST:
                            _permissionHandlingInst = new DataTable();
                            _permissionHandlingInst.Columns.Add(OrderFields.PROPERTY_HANDLING_INSTID);
                            _permissionHandlingInst.Columns.Add(OrderFields.CAPTION_HANDLING_INST);
                            foreach (DataRow dr in tempDT.Rows)
                            {
                                string key = dr[1].ToString();
                                _permissionHandlingInst.Rows.Add(new object[] { key, TagDatabaseManager.GetInstance.GetHandlingInstructionTextBasedOnId(key) });
                            }
                            break;

                        case OrderFields.DS_INDEX_ORDER_EXECUTIONINST:
                            _permissionExecutionInst = new DataTable();
                            _permissionExecutionInst.Columns.Add(OrderFields.PROPERTY_EXECUTION_INSTID);
                            _permissionExecutionInst.Columns.Add(OrderFields.CAPTION_EXECUTION_INST);
                            foreach (DataRow dr in tempDT.Rows)
                            {
                                string key = dr[1].ToString();
                                _permissionExecutionInst.Rows.Add(new object[] { key, TagDatabaseManager.GetInstance.GetExecutionInstructionTextBasedOnID(key) });
                            }
                            break;

                        case OrderFields.DS_INDEX_ORDER_TIF:
                            _permissionTIF = new DataTable();
                            _permissionTIF.Columns.Add(OrderFields.PROPERTY_TIFID);
                            _permissionTIF.Columns.Add(OrderFields.PROPERTY_TIF);
                            foreach (DataRow dr in tempDT.Rows)
                            {
                                string key = dr[1].ToString();
                                _permissionTIF.Rows.Add(new object[] { key, TagDatabaseManager.GetInstance.GetTIFTextBasedOnID(key) });
                            }
                            break;

                        default:
                            break;

                    }
                }

                _accountsAndAllocationRules = new DataTable();
                _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1ID);
                _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1NAME);
                _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE);
                FillAccountAndAllocationDefaultsDataTable();

                _masterFundSubAccountAssociation = _keyValueDataManager.GetCompanyMasterFundSubAccountAssociation(companyID);
                _userPermittedMasterFundsBasedOnFunds = _keyValueDataManager.GetUserPermittedMasterFundBasedOnFunds(companyID, _userID);
                //TODO: This master fund should come in commonDataCache, as it create multiple copy in usercache data for user object.
                _masterFunds = _keyValueDataManager.GetAllMasterFunds();

                DataTable dtAccounts = _clientsCommonDataManager.GetAllPermittedAccounts(_userID);
                FillAccountDictionaries(dtAccounts);

                _transferTradeRules = _clientsCommonDataManager.GetTransferTradeRules(companyID);
                List<int> userPermittedMasterFunds = _masterFundSubAccountAssociation.Where(x => x.Value.Intersect(_useraccounts.Cast<Account>().ToList().Select(a => a.AccountID)).Count() > 0).Select(y => y.Key).ToList();
                _userMasterFunds = _masterFunds.Where(x => userPermittedMasterFunds.Contains(x.Key)).ToDictionary(y => y.Key, y => y.Value);
                UpdateTranferTradeRulesForMasterUser(_userID);

                SetUserCustomGroups();
            }
            catch (Exception)
            {
                bool rethrow = Logger.HandleException(new Exception("Problem in Creating Cache."), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        public IList<MasterFundAccountDetails> UsersMasterFundNCustomGrpWithAccounts
        {
            get
            {
                try
                {
                    var masterFunds = GetUserMasterFundWithAccounts();
                    var userMfAndCustomGrpWtAccount = new List<MasterFundAccountDetails>();
                    userMfAndCustomGrpWtAccount.AddRange(masterFunds);
                    userMfAndCustomGrpWtAccount.AddRange(_userCustomGroups);
                    return userMfAndCustomGrpWtAccount;
                }
                catch (Exception e) { Logger.LogError(e, "Error in UsersMasterFundNCustomGrpWithAccounts "); }
                return new List<MasterFundAccountDetails>();
            }
        }

        public void SetUserCustomGroups()
        {
            try
            {
                var userAccountIds = new List<int>();
                foreach (Prana.BusinessObjects.Account account in _useraccounts)
                {
                    if (account.AccountID > 0)
                        userAccountIds.Add(account.AccountID);
                }
                // Get all custom groups

                var allCustomGroups = _keyValueDataManager.GetAllCustomGroups();
                foreach (var masterFund in allCustomGroups)
                {
                    bool hasAllPermissions = masterFund.AccountList.All(account => userAccountIds.Contains(account.AccountId));
                    if (hasAllPermissions)
                        _userCustomGroups.Add(masterFund);
                }
            }
            catch (Exception e) { Logger.LogError(e, "Error in SetUserCustomGroups "); }
        }

        private void FillAccountAndAllocationDefaultsDataTable()
        {
            try
            {
                if (_accountsAndAllocationRules == null)
                {
                    _accountsAndAllocationRules = new DataTable();
                    _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1ID);
                    _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1NAME);
                    _accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE);
                }
                else
                {
                    _accountsAndAllocationRules.Clear();
                }

                if (_useraccounts != null)
                {
                    foreach (Account userAccount in _useraccounts)
                    {
                        DataRow accountRow = _accountsAndAllocationRules.NewRow();
                        accountRow[OrderFields.PROPERTY_LEVEL1ID] = userAccount.AccountID;
                        accountRow[OrderFields.PROPERTY_LEVEL1NAME] = userAccount.Name;
                        if (userAccount.AccountID != int.MinValue)
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = false;
                        }
                        else
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                        }
                        _accountsAndAllocationRules.Rows.Add(accountRow);
                    }
                    foreach (AllocationDefault allocations in _allocationDefaultCollection.GetDefaults())
                    {
                        DataRow accountRow = _accountsAndAllocationRules.NewRow();
                        accountRow[OrderFields.PROPERTY_LEVEL1ID] = allocations.DefaultID;
                        accountRow[OrderFields.PROPERTY_LEVEL1NAME] = allocations.DefaultName;
                        accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = allocations.IsDefaultAllocationRule;
                        _accountsAndAllocationRules.Rows.Add(accountRow);
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
        private void UpdateTranferTradeRulesForMasterUser(int companyUserId)
        {
            try
            {
                if (_transferTradeRules != null && _transferTradeRules.MasterUsersIDs != null && _transferTradeRules.MasterUsersIDs.Contains(companyUserId))
                {
                    _transferTradeRules = new TranferTradeRules
                    {
                        IsAccountChange = true,
                        IsAllowAllUserToCancelReplaceRemove = true,
                        IsAllowAllowedSecuritiesList = _transferTradeRules.IsAllowAllowedSecuritiesList,
                        IsAllowRestrictedSecuritiesList = _transferTradeRules.IsAllowRestrictedSecuritiesList,
                        IsAllowUserToChangeOrderType = true,
                        IsAllowUserToGenerateSub = true,
                        IsAllowUserToTansferTrade = true,
                        IsApplyLimitRulesForReplacingOtherOrders = false,
                        IsApplyLimitRulesForReplacingStagedOrders = false,
                        IsApplyLimitRulesForReplacingSubOrders = false,
                        IsExecutionInstrChange = true,
                        IsHandlingInstrChange = true,
                        IsStrategyChange = true,
                        IsTIFChange = true,
                        IsTradingAccChange = true,
                        IsVenueCPChange = true,
                        MasterUsersIDs = _transferTradeRules.MasterUsersIDs
                    };
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        private void FillAccountDictionaries(DataTable dtAccounts)
        {
            try
            {
                foreach (DataRow dr in dtAccounts.Rows)
                {
                    if (_companyAccounts.ContainsKey(int.Parse(dr[2].ToString().Trim())))
                    {
                        _companyAccounts[int.Parse(dr[2].ToString())].Add(int.Parse(dr[0].ToString()));
                    }
                    else
                    {
                        List<int> accountCollection = new List<int>();
                        accountCollection.Add(int.Parse(dr[0].ToString()));
                        _companyAccounts.Add(int.Parse(dr[2].ToString()), accountCollection);
                    }
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

        private List<MasterFundAccountDetails> GetUserMasterFundWithAccounts()
        {
            var userMasterFundList = new List<MasterFundAccountDetails>();
            var masterFundsResult = new Dictionary<string, List<MasterFundAccountDetails>>
            {
            { "MasterFundsWithAllAccounts", new List<MasterFundAccountDetails>() },
            { "MasterFundsWithPartialAccounts", new List<MasterFundAccountDetails>() }
            };

            try
            {
                var userAccountList = _useraccounts.Cast<Account>().ToList();
                var userAccountIds = userAccountList.Select(x => x.AccountID);
                var allAssociatedAccounts = _masterFundSubAccountAssociation.SelectMany(mf => mf.Value).ToHashSet();
                //1->12,13,14,2
                foreach (var masterFundEntry in _masterFundSubAccountAssociation)
                {
                    int masterFundId = masterFundEntry.Key;
                    List<int> accountInMf = masterFundEntry.Value;

                    bool hasAllPermissions = accountInMf.All(accId => accId > 0 && userAccountIds.Contains(accId));
                    //the user need to have permission to all acnt of mf
                    //then just show that master fund in the list
                    if (hasAllPermissions)
                    {
                        masterFundsResult["MasterFundsWithAllAccounts"].Add(new MasterFundAccountDetails
                        {
                            MasterFundOrGroupId = masterFundId,
                            MasterFundOrGroupName = _masterFunds.ContainsKey(masterFundId) ? _masterFunds[masterFundId] : "",
                            IsCustomGroup = false,
                            AccountList = userAccountList.Where(x => accountInMf.Contains(x.AccountID))
                               .Select(acc => new AccountDto(acc.Name, acc.AccountID))
                               .ToList(),
                            HasAllAccountsHavePermission = true
                        });
                    }
                    else
                    {
                        //master fund with  even partial permission of accnt along wt acnt.
                        masterFundsResult["MasterFundsWithPartialAccounts"].Add(new MasterFundAccountDetails
                        {
                            MasterFundOrGroupId = masterFundId,
                            MasterFundOrGroupName = _masterFunds.ContainsKey(masterFundId) ? _masterFunds[masterFundId] : "",
                            IsCustomGroup = false,
                            AccountList = userAccountList.Where(x => accountInMf.Contains(x.AccountID))
                                   .Select(acc => new AccountDto(acc.Name, acc.AccountID))
                                   .ToList(),
                            HasAllAccountsHavePermission = false
                        });
                    }
                }

                //Allocating a dummy masterfund to all unallocated accounts.
                var unallocatedAccounts = userAccountList.Where(x => !allAssociatedAccounts.Contains(x.AccountID) && x.AccountID >= 0)
                .Select(acc => new AccountDto(acc.Name, acc.AccountID)).ToList();

                if (unallocatedAccounts.Any())
                {
                    masterFundsResult["MasterFundsWithPartialAccounts"].Add(new MasterFundAccountDetails
                    {
                        MasterFundOrGroupId = -1,
                        MasterFundOrGroupName = "Unallocated",
                        IsCustomGroup = false,
                        AccountList = unallocatedAccounts,
                        HasAllAccountsHavePermission = false
                    });
                }

                foreach (var masterFundList in masterFundsResult.Values)
                {
                    userMasterFundList.AddRange(masterFundList);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in  GetUserMasterFundWithAccounts");
            }
            return userMasterFundList.OrderBy(x => x.MasterFundOrGroupName).ToList();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (companyComplianceBorrowers != null)
                {
                    companyComplianceBorrowers.Dispose();
                }
                if (_permissionOrderSide != null)
                {
                    _permissionOrderSide.Dispose();
                }
                if (_permissionOrderType != null)
                {
                    _permissionOrderType.Dispose();
                }
                if (_permissionHandlingInst != null)
                {
                    _permissionHandlingInst.Dispose();
                }
                if (_permissionExecutionInst != null)
                {
                    _permissionExecutionInst.Dispose();
                }
                if (_permissionTIF != null)
                {
                    _permissionTIF.Dispose();
                }
                if (_accountsAndAllocationRules != null)
                {
                    _accountsAndAllocationRules.Dispose();
                }
            }
        }
        #endregion
    }
}
