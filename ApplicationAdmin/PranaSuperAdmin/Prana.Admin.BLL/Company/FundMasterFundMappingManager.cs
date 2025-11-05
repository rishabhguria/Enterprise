using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// Class to provide business logic for the master fund-account mapping
    /// </summary>
    public static class AccountMasterFundMappingManager
    {
        /// <summary>
        /// check backup of AccountmasterFundMapping is initialized or not
        /// </summary>
        public static bool isbackInitialied = false;

        /// <summary>
        /// check backup of TradingAccountmasterFundMapping is initialized or not
        /// </summary>
        public static bool isbackInitialiedTradingAccount = false;

        /// <summary>
        /// check backup of MasterFund is initialized or not      
        /// </summary>
        public static bool isbackUpInitialiedMasterFund = false;

        /// <summary>
        /// Set default master Fund name NewMF
        /// </summary>
        static String defaultMasterFundName = "NewMF";

        /// <summary>
        /// Dictionary to take back up or collection of masterFund.
        /// 
        static Dictionary<int, String> _backUpMasterFundCollection;

        /// <summary>
        ///Dictionary to take back up or collection of CashAccounts and masterFund mapping.
        ////
        static Dictionary<int, List<int>> _backUpMappingCollection;

        /// <summary>
        ///Dictionary to take back up or collection of CashAccounts and masterFund mapping.
        ////
        static Dictionary<int, int> _backUpTradingAccountMappingCollection;

        /// <summary>
        ///  Dictionary for  collection of accounts
        /// </summary>
        static Dictionary<int, string> _accountCollection = new Dictionary<int, string>();

        /// <summary>
        ///  Dictionary for  collection of accounts
        /// </summary>
        static TradingAccounts _tradingAccountCollection = new TradingAccounts();

        /// <summary>
        /// Dictionary for  collection of MasterFunds
        /// </summary>
        static Dictionary<int, string> _masterFundCollection = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for  collection of accountsMasterFundMapping
        /// </summary>
        static Dictionary<int, List<int>> _accountMasterFundMapping = new Dictionary<int, List<int>>();

        /// <summary>
        /// Dictionary for  collection of accountsMasterFundMapping
        /// </summary>
        static Dictionary<int, int> _masterFundTradingAccountMapping = new Dictionary<int, int>();

        /// <summary>
        /// Gets or sets the master fund trading account mapping.
        /// </summary>
        /// <value>
        /// The master fund trading account mapping.
        /// </value>
        public static bool IsTradingAccountMappedToMF(int tradingAccountID)
        {
            return _masterFundTradingAccountMapping.Values.Contains(tradingAccountID);
        }

        /// <summary>
        ///  Dictionary for  collection of master funds status
        /// </summary>
        static Dictionary<int, int> _masterFundStatus = new Dictionary<int, int>();

        /// <summary>
        /// Dictionary to take back up or collection of masterFund for Audit.
        /// 
        static Dictionary<int, String> _backUpMasterForAudit;
        //Added By Faisal Shah
        //dated 02/07/14
        static int newID = -2;

        static Dictionary<int, List<int>> _masterFundAddUpdateStatus = new Dictionary<int, List<int>>();

        /// <summary>
        /// method for get all masterFundName List from __masterFundCollection dictionary and return list of master fund Name
        /// </summary>
        /// <returns>list of masterFundName</returns>
        public static List<String> GetAllMasterFundName()
        {
            List<String> masterFundNames = new List<string>();
            try
            {
                foreach (String name in _masterFundCollection.Values)
                {
                    masterFundNames.Add(name);
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
            return masterFundNames;
        }

        //added By: Bharat raturi, 21-apr-2014
        //store the max master fundid in a variable
        static int _maxMasterFundID;

        /// <summary>
        /// Get all Account names for a particular masterFund name
        /// </summary>
        /// <param name="masterFundName"> Selected masterFundName</param>
        /// <returns>list of AccountNames for selected master Fund Name</returns>

        public static List<String> GetAccountNamesForMasterFund(String masterFundName)
        {
            List<String> accountNameList = new List<string>();
            try
            {
                int masterFundId = GetMasterFundIdByName(masterFundName);

                if (masterFundId == -1)
                    return null;

                if (_accountMasterFundMapping.ContainsKey(masterFundId))
                {
                    foreach (int accountId in _accountMasterFundMapping[masterFundId])
                    {
                        if (_accountCollection.ContainsKey(accountId))
                        {
                            accountNameList.Add(_accountCollection[accountId]);
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
            return accountNameList;
        }

        /// <summary>
        /// Gets the trading account for master fund.
        /// </summary>
        /// <param name="masterFundName">Name of the master fund.</param>
        /// <returns></returns>
        public static int GetTradingAccountForMasterFund(String masterFundName)
        {
            int tradingAccountID = -1;
            try
            {
                int masterFundId = GetMasterFundIdByName(masterFundName);

                if (masterFundId > 0 && _masterFundTradingAccountMapping.ContainsKey(masterFundId))
                {
                    tradingAccountID = _masterFundTradingAccountMapping[masterFundId];
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
            return tradingAccountID;
        }

        public static int GetTradingAccountForMasterFund(int mFID)
        {
            int tradingAccountID = -1;
            try
            {
                if (_masterFundTradingAccountMapping.ContainsKey(mFID))
                {
                    tradingAccountID = _masterFundTradingAccountMapping[mFID];
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
            return tradingAccountID;
        }
        /// <summary>
        /// Get all Unassigned Account names from Mapping Dictionary _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFund">The name of the master fund for which the unmapped accounts are to be bound</param>
        /// <returns>List of Unmapped Account name</returns>
        public static List<String> GetUnmappedAccounts(string masterFund)
        {
            List<String> unmappedAccounts = new List<string>();
            try
            {
                //if a strategy is not assigned to the selected master fund
                //show it in the list of strategies available for mapping
                foreach (string accountName in _accountCollection.Values)
                {
                    //modified by: Bharat Raturi, Date: 03/04/2014
                    //Purpose: IN many to many account-master fund mapping show all the accounts in the list
                    //of unassigned accounts that are not mapped with this master fund
                    if (!String.IsNullOrEmpty(masterFund))
                    {
                        if (!GetAccountNamesForMasterFund(masterFund).Contains(accountName))
                            unmappedAccounts.Add(accountName);
                    }
                    else
                    {
                        unmappedAccounts.Add(accountName);
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
            unmappedAccounts.Sort();
            return unmappedAccounts;
        }
        //try
        //{
        //    foreach (int accountId in _accountCollection.Keys)
        //    {
        //        // Check a account id is associated with a particular masterFund or not 
        //        bool isFound = false;
        //        foreach (List<int> accountList in _accountMasterFundMapping.Values)
        //        {
        //            if (accountList.Contains(accountId))
        //            {
        //                isFound = true;
        //            }
        //        }
        //        //if not true or not mapped then add this account id to unmappedAccount collection
        //        if (!isFound)
        //        {
        //            unmappedAccounts.Add(_accountCollection[accountId]);
        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    // Invoke our policy that is responsible for making sure no secure information
        //    // gets out of our layer.
        //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //    if (rethrow)
        //    {
        //        throw;
        //    }
        //}
        //    return unmappedAccounts;
        //}

        //added by: Bharat raturi, 22 apr 2014
        //purpose: Load the mapping if one to many mapping is allowed
        /// <summary>
        /// Get all Unassigned Account names from Mapping Dictionary _accountMasterFundMapping
        /// </summary>
        /// <returns>List of Unmapped Account name</returns>
        public static List<String> GetUnmappedAccountsForOnetoMany()
        {
            List<String> unmappedAccounts = new List<string>();
            try
            {
                foreach (int accountId in _accountCollection.Keys)
                {
                    // Check a account id is associated with a particular masterFund or not 
                    bool isFound = false;
                    foreach (List<int> accountList in _accountMasterFundMapping.Values)
                    {
                        if (accountList.Contains(accountId))
                        {
                            isFound = true;
                        }
                    }
                    //if not true or not mapped then add this account id to unmappedAccount collection
                    if (!isFound)
                    {
                        unmappedAccounts.Add(_accountCollection[accountId]);
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
            unmappedAccounts.Sort();
            return unmappedAccounts;
        }

        /// <summary>
        /// Search the list for matching account names
        /// </summary>
        /// <param name="searchKeyword">The keyword entered in the textbox</param>
        /// <param name="list">List of items in the listbox</param>
        /// <returns>The list of matching items</returns>
        public static List<String> SerachForKeyword(String searchKeyword, List<String> list)
        {
            List<String> foundStrings = new List<string>();
            try
            {

                if (list != null)
                {
                    foreach (String key in list)
                    {
                        if (key.Trim().ToLower().Contains(searchKeyword.Trim().ToLower()))
                            foundStrings.Add(key);
                    }
                }
                else
                {
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
            return foundStrings;
        }

        /// <summary>
        /// Take Back Up of Account masterFund mapping Relation ship to reuse it 
        /// </summary>
        public static bool IsBackInitialied()
        {

            if (isbackInitialied == false)
            {
                _backUpMappingCollection = new Dictionary<int, List<int>>(_accountMasterFundMapping.Count, _accountMasterFundMapping.Comparer);

                //making a deep copy of mapping object
                foreach (int masterFundId in _accountMasterFundMapping.Keys)
                {
                    _backUpMappingCollection.Add(masterFundId, new List<int>());
                    foreach (int accountId in _accountMasterFundMapping[masterFundId])
                    {
                        _backUpMappingCollection[masterFundId].Add(accountId);
                    }
                }

                isbackInitialied = true;
                // return true;
            }
            //else
            //    return false;
            return isbackInitialied;

        }

        /// <summary>
        /// Take Back Up of Account masterFund mapping Relation ship to reuse it 
        /// </summary>
        public static bool IsBackInitialiedTradingAccount()
        {

            if (isbackInitialiedTradingAccount == false)
            {
                _backUpTradingAccountMappingCollection = new Dictionary<int, int>(_masterFundTradingAccountMapping.Count, _masterFundTradingAccountMapping.Comparer);

                //making a deep copy of mapping object
                foreach (int masterFundId in _masterFundTradingAccountMapping.Keys)
                {
                    _backUpTradingAccountMappingCollection.Add(masterFundId, _masterFundTradingAccountMapping[masterFundId]);
                }

                isbackInitialiedTradingAccount = true;

            }
            return isbackInitialiedTradingAccount;

        }

        /// <summary>
        /// Restore AccountMaster Fund Mapping Dictionary from back up of _accountMasterFundMapping named as _backUpMappingCollection 
        /// </summary>
        public static void RestoreBackUp()
        {
            if (isbackInitialied == true)
            {
                _accountMasterFundMapping = _backUpMappingCollection;
                CleanBackUp();
            }

        }

        /// <summary>
        ///deep copy of _accountMasterFundMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUp()
        {
            _backUpMappingCollection = null;
            isbackInitialied = false;
        }

        /// <summary>
        /// Restore AccountMaster Fund Mapping Dictionary from back up of _accountMasterFundMapping named as _backUpMappingCollection 
        /// </summary>
        public static void RestoreBackUpTA()
        {
            if (isbackInitialiedTradingAccount == true)
            {
                _masterFundTradingAccountMapping = _backUpTradingAccountMappingCollection;
                CleanBackUpTA();
            }

        }

        /// <summary>
        ///deep copy of _accountMasterFundMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUpTA()
        {
            _backUpTradingAccountMappingCollection = null;
            isbackInitialiedTradingAccount = false;
        }

        /// <summary>
        /// return true or false given master Fund name is already exist or not
        /// </summary>
        /// <param name="list">collelction of masterFund name</param>
        /// <returns>true if exist else false</returns>
        public static bool IsMasterFundNameExist(params String[] list)
        {
            bool isexistNewname = false;
            List<String> tempList = new List<string>();
            foreach (String val in _masterFundCollection.Values)
                tempList.Add(val);
            tempList.Sort();
            int res = tempList.BinarySearch(list[0], StringComparer.OrdinalIgnoreCase);
            if (res >= 0)
            {
                isexistNewname = true;
            }
            return isexistNewname;

        }

        /// <summary>
        /// Save masterFund in dictionary collection wnem it delete rename or add
        /// </summary>
        /// <param name="id">0-add,1-rename,2-delete</param>
        /// <param name="newName"></param>
        /// <param name="list">1 name always new name and then all old name </param>
        public static void ManageMasterFund(int id, params String[] list)
        {

            try
            {
                if (!isbackUpInitialiedMasterFund)
                {
                    isbackUpInitialiedMasterFund = IsBackInitialiedMasterFundCollection();
                }
                if (!isbackInitialied)
                {
                    isbackInitialied = IsBackInitialied();
                }
                if (!isbackInitialiedTradingAccount)
                {
                    isbackInitialiedTradingAccount = IsBackInitialiedTradingAccount();
                }
                if (id == 0)
                {
                    //Code Commented by Faisal Shah
                    //03/07/14
                    //Added a temp ID till we save the data
                    #region CommentedCode
                    //int newId = _maxMasterFundID;
                    //_maxMasterFundID += 1;//11;
                    //List<int> idList = new List<int>(_masterFundCollection.Keys);
                    //if (idList.Count != 0)
                    //{
                    //    idList.Sort();
                    //    newId = idList[_masterFundCollection.Keys.Count - 1] + 1;
                    //}
                    #endregion

                    _masterFundCollection.Add(newID, list[0]);
                    newID--;
                }
                else
                {
                    if (id == 1)
                    {
                        int mId = GetMasterFundIdByName(list[1]);
                        _masterFundCollection[mId] = list[0];
                        //Modified By Faisal Shah
                        //Dated 03/07/14
                        // Adding Items that are updated or deleted to a Dictionary.
                        if (mId > 0)
                        {
                            if (!_masterFundAddUpdateStatus.ContainsKey(1))
                            {
                                _masterFundAddUpdateStatus.Add(1, new List<int>());
                                _masterFundAddUpdateStatus[1].Add(mId);
                            }
                            else
                                _masterFundAddUpdateStatus[1].Add(mId);
                        }
                    }
                    else
                        if (id == 2)
                    {
                        int mId = GetMasterFundIdByName(list[1]);
                        if (_accountMasterFundMapping.ContainsKey(mId))
                        {
                            _accountMasterFundMapping.Remove(mId);
                        }
                        if (!_masterFundAddUpdateStatus.ContainsKey(2))
                        {
                            _masterFundAddUpdateStatus.Add(2, new List<int>());
                            _masterFundAddUpdateStatus[2].Add(mId);
                        }
                        else
                            _masterFundAddUpdateStatus[2].Add(mId);
                        _masterFundCollection.Remove(mId);
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
        /// return true if back up of master Fund collection is intialized
        /// </summary>
        /// <returns>true  or false </returns>
        public static bool IsBackInitialiedMasterFundCollection()
        {
            bool result = false;
            try
            {
                if (isbackUpInitialiedMasterFund == false)
                {
                    _backUpMasterFundCollection = new Dictionary<int, String>(_masterFundCollection.Count, _masterFundCollection.Comparer);
                    //making a deep copy of mapping object
                    foreach (int key in _masterFundCollection.Keys)
                    {
                        _backUpMasterFundCollection.Add(key, _masterFundCollection[key]);
                    }
                    isbackUpInitialiedMasterFund = true;
                    result = true;
                }
                else
                    result = false;
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
            return result;
        }

        /// <summary>
        /// Restore AccountMaster Fund Mapping Dictionary from back up of _accountMasterFundMapping named as _backUpMappingCollection 
        /// </summary>
        public static void RestoreBackUpMasterFund()
        {
            try
            {
                if (isbackUpInitialiedMasterFund == true)
                {
                    _masterFundCollection = _backUpMasterFundCollection;
                    CleanBackUpMasterFund();
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
        ///deep copy of _accountMasterFundMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUpMasterFund()
        {
            try
            {
                _backUpMasterFundCollection = null;
                isbackUpInitialiedMasterFund = false;
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
        /// Remove  account names  for Selected  masterFundName from mapping dictionary named as  _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFundName">Selected MasterFund name</param>
        /// <param name="accountNames">list of assigned Account names to be unassigned</param>
        public static void UnassignAccounts(String masterFundName, List<String> accountNames)
        {
            try
            {
                // initialied Back up 
                IsBackInitialied();
                //get masterFund Id from given masterFund name
                int masterFundId = GetMasterFundIdByName(masterFundName);
                if (!_accountMasterFundMapping.ContainsKey(masterFundId))
                {
                    _accountMasterFundMapping.Add(masterFundId, new List<int>());
                }
                foreach (string fname in accountNames)
                {
                    // RemoveFromAccountMasterFundMapping()in which return true or false check back up() is exist then updatebackup() else  call backup() method as copy of dictionary of _accountMasterFundMapping
                    _accountMasterFundMapping[masterFundId].Remove(GetAccountIdByName(fname));
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
        ///Add account names for Selected  masterFundName to mapping dictionary named as  _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFundName">Slected masterFundName</param>
        /// <param name="accountNames">List of Accountnames to be assigned in Slected materAccount name</param>
        public static void AssignAccounts(String masterFundName, List<String> accountNames)
        {
            try
            {
                IsBackInitialied();
                int masterFundId = GetMasterFundIdByName(masterFundName);
                if (!_accountMasterFundMapping.ContainsKey(masterFundId))
                {
                    _accountMasterFundMapping.Add(masterFundId, new List<int>());
                }
                foreach (string fName in accountNames)
                {
                    //addIntoAccountMasterFundMApping() in which check copy() of dictionary is exist then update else take backup as copy of dictionay _accountMasterFundMapping
                    _accountMasterFundMapping[masterFundId].Add(GetAccountIdByName(fName));
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
        ///Add account names for Selected  masterFundName to mapping dictionary named as  _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFundName">Slected masterFundName</param>
        /// <param name="accountNames">List of Accountnames to be assigned in Slected materAccount name</param>
        public static void AssignTradingAccount(String masterFundName, int tradingAccountID)
        {
            try
            {
                IsBackInitialiedTradingAccount();
                int masterFundId = GetMasterFundIdByName(masterFundName);
                if (tradingAccountID == -1)
                {
                    _masterFundTradingAccountMapping.Remove(masterFundId);
                }
                else
                {
                    _masterFundTradingAccountMapping[masterFundId] = tradingAccountID;
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
        /// Get Account Id From name from _accountCollection dictionary
        /// </summary>
        /// <param name="accountName">Given Account name to get Account id </param>
        /// <returns>Account Id of Given Account Name</returns>
        private static int GetAccountIdByName(String accountName)
        {
            int accountId = -1;
            try
            {
                foreach (int key in _accountCollection.Keys)
                {
                    if (_accountCollection[key] == accountName)
                    {
                        accountId = key;
                        break;
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
            return accountId;
        }

        /// <summary>
        /// Get masterFund Id From name from _masterFundCollection dictionary
        /// </summary>
        /// <param name="accountName">Given MasterFund name to get MasterFund id </param>
        /// <returns>MasterFund Id of Given MasterFund Name</returns>
        public static int GetMasterFundIdByName(String masterFundName)
        {
            int masterFundId = -1;
            try
            {
                foreach (int key in _masterFundCollection.Keys)
                {
                    if (_masterFundCollection[key] == masterFundName)
                    {
                        masterFundId = key;
                        break;
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
            return masterFundId;
        }

        /// <summary>
        /// Save the mapping and the master funds
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>positive integer if the records are saved</returns>
        public static int SaveMapping(int companyID)
        {
            int r = -1;
            _masterFundStatus.Clear();
            try
            {
                //Modified By Faisal Shah
                //Dated 03/07/14
                //Adding added fields to Dictionary and assigning MasterFundID to added MasterFunds
                //DataSet dsMapping = GetDataSetFromDictionary(_accountMasterFundMapping);
                //String xmlDoc = ConvertDataSetToXml(dsMapping);
                _maxMasterFundID = MasterFundMappingDAL.GetNewMasterFundID();
                for (int newkey = newID; newkey <= -2; newkey++)
                {
                    if (_masterFundCollection.Keys.Contains(newkey))
                    {
                        _masterFundCollection.Add(_maxMasterFundID, _masterFundCollection[newkey]);
                        _masterFundCollection.Remove(newkey);
                        if (!_masterFundAddUpdateStatus.ContainsKey(0))
                        {
                            _masterFundAddUpdateStatus.Add(0, new List<int>());
                            _masterFundAddUpdateStatus[0].Add(_maxMasterFundID);
                        }
                        else
                            _masterFundAddUpdateStatus[0].Add(_maxMasterFundID);
                        //Added By Faisal Shah on 18/07/14
                        if (_accountMasterFundMapping.Keys.Contains(newkey))
                        {
                            if (!_accountMasterFundMapping.ContainsKey(_maxMasterFundID))
                            {
                                _accountMasterFundMapping.Add(_maxMasterFundID, _accountMasterFundMapping[newkey]);
                                _accountMasterFundMapping.Remove(newkey);
                            }
                        }
                        if (_masterFundTradingAccountMapping.Keys.Contains(newkey))
                        {
                            if (!_masterFundTradingAccountMapping.ContainsKey(_maxMasterFundID))
                            {
                                _masterFundTradingAccountMapping.Add(_maxMasterFundID, _masterFundTradingAccountMapping[newkey]);
                                _masterFundTradingAccountMapping.Remove(newkey);
                            }
                        }
                        _maxMasterFundID++;
                    }

                }

                DataSet dsMapping = GetDataSetFromDictionary();
                String xmlDoc = ConvertDataSetToXml(dsMapping);

                DataSet dsTAMapping = GetDataSetFromDictionaryTA();
                String xmlDocTA = ConvertDataSetToXml(dsTAMapping);
                //Modified By Faisal Shah 06/08/14
                //Added CH Release as Parameter to execute separate Code for two releases
                DataSet dsMasterFund = GetDataSetFromMasterFundDictionary(_masterFundCollection, companyID);
                string xml = ConvertDataSetToXml(dsMasterFund);
                bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                _masterFundAddUpdateStatus.Clear();
                r = MasterFundMappingDAL.SaveDataSetInDb(xmlDoc, xmlDocTA, xml, companyID, isPermanentDeletion);


                // For audit trail
                if (_masterFundCollection != null)
                {
                    CachedDataManager.UpdateMasterFunds(_masterFundCollection);
                    foreach (int key in _masterFundCollection.Keys)
                    {
                        if (!_masterFundStatus.ContainsKey(key))
                        {
                            if (_backUpMasterForAudit.ContainsKey(key))
                            {
                                // status is set to 3 for update
                                _masterFundStatus.Add(key, 3);
                            }
                            else
                            {
                                // status is set to 1 for create
                                _masterFundStatus.Add(key, 1);
                            }
                        }
                    }
                }

                if (_backUpMasterForAudit != null)
                {
                    foreach (int key in _backUpMasterForAudit.Keys)
                    {
                        if (!_masterFundStatus.ContainsKey(key))
                        {
                            if (!_masterFundCollection.ContainsKey(key))
                            {
                                // status is set to 2 for delete
                                _masterFundStatus.Add(key, 2);
                            }
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
            return r;
        }

        /// <summary>
        /// Get status dictionary for audit
        /// </summary>
        /// <returns>dictionary for auditStatus of master funds</returns>
        public static Dictionary<int, int> GetStatusForAudit()
        {
            try
            {
                return _masterFundStatus;
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
            return _masterFundStatus;
        }

        /// <summary>
        /// dictionary data is converted into data set
        /// </summary>
        /// <param name="_masterFundCollection">dictionary of master Fund name and id</param>
        /// <returns>data set </returns>
        private static DataSet GetDataSetFromMasterFundDictionary(Dictionary<int, string> _masterFundCollection, int companyID)
        {
            DataTable dt = new DataTable("Table1");
            DataSet ds = new DataSet("NewDataSet");
            try
            {
                dt.Columns.Add("CompanyMasterFundID", typeof(int));
                dt.Columns.Add("MasterFundName", typeof(string));
                dt.Columns.Add("CompanyID", typeof(int));
                //Added  by Faisal Shah
                //Dated 03/07/14
                //For Additional info if we need to add update or delete a record
                //Modified By Faisal Shah 06/08/14
                //CH logic and Prana Logic Separation due to additional Column IsActive in CH Release.
                bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                //if (!isPermanentDeletion)
                //{
                dt.Columns.Add("QueryType", typeof(int));

                foreach (KeyValuePair<int, List<int>> kvp in _masterFundAddUpdateStatus)
                {
                    if (kvp.Key == 0 || kvp.Key == 1)
                    {
                        foreach (int keyforMaster in kvp.Value)
                        {
                            dt.Rows.Add(keyforMaster, _masterFundCollection[keyforMaster], companyID, kvp.Key);

                        }
                    }
                    else if (kvp.Key == 2)
                    {
                        foreach (int keyforMaster in kvp.Value)
                        {
                            dt.Rows.Add(keyforMaster, "ToBeDeleted", companyID, kvp.Key);
                        }
                    }
                }
                ds.Tables.Add(dt);
            }
            //    else
            //    {
            //        foreach (int key in _masterFundCollection.Keys)
            //        {
            //            dt.Rows.Add(key, _masterFundCollection[key], companyID);
            //        }
            //        ds.Tables.Add(dt);
            //    }
            //}
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
            return ds;
        }

        /// <summary>
        /// Convert _accountmasterFundMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Account masterFund name</param>
        /// <returns>Data Set which is collection of AccountIds corresponding to masterFund Id</returns>
        private static DataSet GetDataSetFromDictionary()
        {
            DataTable dt = new DataTable("TABFundMasterFundMapping");
            dt.Columns.Add("CompanyMasterFundId", typeof(int));
            dt.Columns.Add("CompanyFundId", typeof(int));
            DataSet ds = new DataSet("DSFundMasterFundMapping");
            try
            {
                foreach (int mId in _accountMasterFundMapping.Keys)
                {
                    foreach (int fId in _accountMasterFundMapping[mId])
                    {
                        dt.Rows.Add(mId, fId);
                    }
                }
                ds.Tables.Add(dt);

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
            return ds;
        }

        /// <summary>
        /// Convert _accountmasterFundMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Account masterFund name</param>
        /// <returns>Data Set which is collection of AccountIds corresponding to masterFund Id</returns>
        private static DataSet GetDataSetFromDictionaryTA()
        {
            DataTable dt = new DataTable("TABMasterFundTradingAccountMapping");
            dt.Columns.Add("CompanyMasterFundId", typeof(int));
            dt.Columns.Add("CompanyTradingAccountId", typeof(int));
            DataSet ds = new DataSet("DSMasterFundTradingAccountMapping");
            try
            {
                foreach (int mId in _masterFundTradingAccountMapping.Keys)
                {
                    dt.Rows.Add(mId, _masterFundTradingAccountMapping[mId]);
                }
                ds.Tables.Add(dt);

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
            return ds;
        }

        /// <summary>
        /// Convert data set to Xml Document
        /// </summary>
        /// <param name="ds">Data set collection of AccountIDs and MasterFund Ids</param>
        /// <returns>xml documents</returns>
        private static string ConvertDataSetToXml(DataSet ds)
        {
            string xml = null;
            try
            {
                xml = ds.GetXml();
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
            return xml;
        }

        /// <summary>
        /// Load all dta from data base 
        /// </summary>
        public static void InitialiseData(int companyID)
        {
            try
            {
                _masterFundCollection = MasterFundMappingDAL.LoadMasterFundFromDb(companyID);
                //Load all Account From Data base
                _accountCollection = MasterFundMappingDAL.LoadAccountsFromDb(companyID);
                //Load all Account MasterFund  mapping from data base
                _accountMasterFundMapping = MasterFundMappingDAL.LoadAccountMasterFundMappingFromDb(companyID);
                //Load all MasterFund Trading Account mapping from data base
                _masterFundTradingAccountMapping = MasterFundMappingDAL.LoadMasterFundTradingAccountMappingFromDb(companyID);
                //Load all Trading Account From Data base
                _tradingAccountCollection = CompanyManager.GetTradingAccountsForCompany(companyID);
                //Commented BY Faisal Shah
                //Dated 03/07/14
                //_maxMasterFundID=MasterFundMappingDAL.GetNewMasterFundID();
                // For audit
                _backUpMasterForAudit = MasterFundMappingDAL.LoadMasterFundFromDb(companyID);
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
        /// Get the closing methods from the database
        /// </summary>
        /// <returns>The datatable holding the closing methodology details</returns>
        public static DataTable GetTradingAccounts()
        {
            DataTable dtClosingMethod = new DataTable();
            dtClosingMethod.Columns.Add("TradingAccountID", typeof(int));
            dtClosingMethod.Columns.Add("TradingAccountName", typeof(string));
            try
            {
                dtClosingMethod.Rows.Add(-1, "-Select-");
                foreach (TradingAccount tAccount in _tradingAccountCollection)
                {
                    dtClosingMethod.Rows.Add(tAccount.TradingAccountsID, tAccount.TradingAccountName);
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
            return dtClosingMethod;
        }

        //added by: Bharat raturi, 23 apr 2014
        //purpose: Check if many to many mapping is set
        /// <summary>
        /// Check if there is one to many mapping doen previously
        /// </summary>
        /// <returns>true if many to many mapping, false if one to many mapping</returns>
        public static bool IsManyToManyMapping()
        {
            List<int> tempAccountList = new List<int>();
            foreach (int masterFundID in _accountMasterFundMapping.Keys)
            {
                tempAccountList.AddRange(_accountMasterFundMapping[masterFundID]);
            }
            tempAccountList.Sort();
            if (tempAccountList.Distinct().Count() != tempAccountList.Count)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To get Master Fund ID for Audit Trail 
        /// </summary>
        public static List<int> GetMasterFundID(int companyID)
        {
            List<int> masterFundIdList = null;
            try
            {
                masterFundIdList = new List<int>(MasterFundMappingDAL.LoadMasterFundFromDb(companyID).Keys);
                return masterFundIdList;
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
            return masterFundIdList;
        }

        /// <summary>
        /// genetae new name for master Fund
        /// </summary>
        /// <returns>new name for master Fund</returns>
        public static String GetRuntimeMasterFundName()
        {
            String returnMasterFundNameValue = defaultMasterFundName;
            try
            {
                if (_masterFundCollection.ContainsValue(defaultMasterFundName))
                {
                    bool breakLoop = false;
                    int tempAppendKey = 1;
                    while (!breakLoop)
                    {
                        if (!_masterFundCollection.ContainsValue(defaultMasterFundName + tempAppendKey))
                        {
                            returnMasterFundNameValue = defaultMasterFundName + tempAppendKey;
                            breakLoop = true;
                        }
                        else
                        {
                            tempAppendKey++;
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
            return returnMasterFundNameValue;
        }

        /// <summary>
        /// Checks the master fund association.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool CheckMasterFundAssociation(int companyId, string name)
        {
            try
            {
                int masterFundId = GetMasterFundIdByName(name);
                if (MasterFundMappingDAL.isMasterFundAssociated(companyId, masterFundId))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks the fund associated.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="masterFundName">Name of the master fund.</param>
        /// <param name="unSelectAccounts">The un select accounts.</param>
        /// <returns></returns>
        public static string CheckFundAssociated(int companyId, string masterFundName, List<string> unSelectAccounts)
        {
            try
            {
                int masterFundId = GetMasterFundIdByName(masterFundName);
                foreach (string accountName in unSelectAccounts)
                {
                    int accountId = GetAccountIdByName(accountName);
                    if (MasterFundMappingDAL.isFundAssociated(companyId, masterFundId, accountId))
                    {
                        return accountName;
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
            return null;
        }

    }
}
