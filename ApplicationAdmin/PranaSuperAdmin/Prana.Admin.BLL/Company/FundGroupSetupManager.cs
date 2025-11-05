using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class AccountGroupSetupManager
    {
        /// <summary>
        /// check backup of accountGroupMapping is initialized or not
        /// </summary>
        public static bool isbackInitialied = false;
        /// <summary>
        /// Dictionary for  collection of accounts
        /// </summary>
        static Dictionary<int, string> _accountCollection = new Dictionary<int, string>();
        /// <summary>
        /// Dictionary for  collection of clients
        /// </summary>
        static Dictionary<int, string> _clientsCollection = new Dictionary<int, string>();
        ///// <summary>
        ///// Dictionary for  collection of _clientAccountMappingCollection
        ///// </summary>
        //static Dictionary<int, string> _clientAccountMappingCollection = new Dictionary<int, string>();
        /// <summary>
        /// Dictionary for  collection of _accountGroupMapping
        /// </summary>
        static Dictionary<int, List<int>> _accountGroupMapping = new Dictionary<int, List<int>>();
        /// <summary>
        /// Dictionary for  collection of _clientGroupMapping
        /// </summary>
        static Dictionary<int, List<int>> _clientGroupMapping = new Dictionary<int, List<int>>();
        /// <summary>
        /// Dictionary for  collection of _clientAccountMapping
        /// </summary>
        static Dictionary<int, List<int>> _clientAccountMapping = new Dictionary<int, List<int>>();
        /// <summary>
        /// Dictionary for  collection of accounts
        /// </summary>
        static Dictionary<int, string> _allaccountCollection = new Dictionary<int, string>();
        /// <summary>
        ///Dictionary to take back up or collection of accounts and groups mapping.
        ////
        static Dictionary<int, List<int>> _backUpMappingCollection;
        /// <summary>
        /// to store max group ID
        /// </summary>
        static int _accountGroupID;

        /// <summary>
        /// Load all data from data base 
        /// </summary>
        public static void InitialiseData(int groupID)
        {
            try
            {
                //Load all clients from Database
                _clientsCollection = AccountGroupSetupMappingDAL.LoadClientsFromDb();
                //Load all accounts and group mapping from Database
                _accountGroupMapping = AccountGroupSetupMappingDAL.LoadAccountGroupMappingFromDb(groupID);
                //Load all accounts and client from Database
                _clientAccountMapping = AccountGroupSetupMappingDAL.LoadAccountByClientFromDb();
                //Load all accounts from Database
                _allaccountCollection = AccountGroupSetupMappingDAL.LoadAccountFromDb();
                //Load all clients with mapped accounts from Database
                _clientGroupMapping = AccountGroupSetupMappingDAL.LoadClientsForMappedAccountsFromDb(groupID);
                //Bind CashAccounts on selection of client
                BindAccountByClient(_clientGroupMapping);
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
        /// Load all data from data base 
        /// </summary>
        public static void BindAccountByClient(Dictionary<int, List<int>> clientList)
        {
            try
            {
                _accountCollection.Clear();
                if (clientList != null)
                {
                    foreach (int gID in clientList.Keys)
                    {
                        foreach (int clientID in clientList[gID])
                        {
                            foreach (string accountName in GetAccountNamesForClient(clientID))
                            {
                                _accountCollection.Add(GetAccountIdByName(accountName), accountName);
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
        }

        /// <summary>
        /// get list of all unmapped accounts
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static List<String> GetUnmappedAccounts(int groupID)
        {
            List<String> unmappedAccounts = new List<string>();
            try
            {
                //if a account is not assigned to the selected group
                //show it in the list of accounts available for mapping
                foreach (string accountName in _accountCollection.Values)
                {
                    //Purpose: IN many to many user account mapping show all unmapped accounts in list of available accounts
                    if (!GetAccountNamesForGroup(groupID).Contains(accountName))
                        unmappedAccounts.Add(accountName);
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
        /// Save the mapping of group and account
        /// </summary>
        /// <param name="companyID">ID of the group</param>
        /// <returns>positive integer if the records are saved</returns>
        public static bool SaveMapping(int groupID, string groupName)
        {
            try
            {
                DataSet dsMapping = GetDataSetFromDictionary(_accountGroupMapping);
                String xmlDoc = ConvertDataSetToXml(dsMapping);
                AccountGroupSetupMappingDAL.SaveDataSetInDb(xmlDoc, groupID, groupName);
                return true;
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
            return false;
        }

        /// <summary>
        /// get list of all unmapped clients
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static List<String> GetUnmappedClients(int groupID)
        {
            List<String> unmappedClients = new List<string>();
            try
            {
                //if a client is not assigned to the selected group
                //show it in the list of client available for mapping
                foreach (string clientName in _clientsCollection.Values)
                {
                    //Purpose: IN many to many user account mapping show all unmapped accounts in list of available accounts
                    if (!GetClientNamesForGroup(groupID).Contains(clientName))
                        unmappedClients.Add(clientName);
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
            unmappedClients.Sort();
            return unmappedClients;
        }

        #region CommonFunctions

        /// <summary>
        /// Get all account names for a particular group
        /// </summary>
        /// <param name="masterFundName"> Selected group</param>
        /// <returns>list of AccountNames for selected group</returns>
        public static List<String> GetAccountNamesForGroup(int groupID)
        {
            List<String> accountNameList = new List<string>();
            try
            {
                if (_accountGroupMapping.ContainsKey(groupID))
                {
                    List<int> _cloneAccountGroupmapping = Global.Utilities.DeepCopyHelper.Clone(_accountGroupMapping[groupID]);
                    foreach (int accountId in _cloneAccountGroupmapping)
                    {
                        if (_accountCollection.ContainsKey(accountId))
                        {
                            accountNameList.Add(_accountCollection[accountId]);
                        }
                        else
                            _accountGroupMapping[groupID].Remove(accountId);
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
        /// Get all account names for a particular client
        /// </summary>
        /// <param name="masterFundName"> Selected client</param>
        /// <returns>list of AccountNames for selected client</returns>
        public static List<String> GetAccountNamesForClient(int clientID)
        {
            List<String> accountNameList = new List<string>();
            try
            {
                if (_clientAccountMapping.ContainsKey(clientID))
                {
                    foreach (int accountId in _clientAccountMapping[clientID])
                    {
                        if (_allaccountCollection.ContainsKey(accountId))
                        {
                            accountNameList.Add(_allaccountCollection[accountId]);
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
        /// Remove  account names for selected group from mapping dictionary named as  _accountGroupMapping
        /// </summary>
        /// <param name="masterFundName">Selected groupID</param>
        /// <param name="accountNames">List of accountNames</param>
        public static void UnMapAccountsFromGroup(int groupID, List<String> accountNames)
        {
            try
            {
                // initialied Back up 
                IsBackInitialied();
                if (!_accountGroupMapping.ContainsKey(groupID))
                {
                    _accountGroupMapping.Add(groupID, new List<int>());
                }
                foreach (string fName in accountNames)
                {
                    // Remove from accountGroupMapping which return true or false check back up() exists then updatebackup() else  call backup() method as copy of dictionary of _accountGroupMapping
                    _accountGroupMapping[groupID].Remove(GetAccountIdByName(fName));
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
        ///Add account names for Selected  group to mapping dictionary named as  _accountGroupMapping
        /// </summary>
        /// <param name="masterFundName">Selected groupID</param>
        /// <param name="accountNames">List of accountNames</param>
        public static void MapAccountsToGroup(int groupID, List<String> accountNames)
        {
            try
            {
                IsBackInitialied();
                if (!_accountGroupMapping.ContainsKey(groupID))
                {
                    _accountGroupMapping.Add(groupID, new List<int>());
                }
                foreach (string fName in accountNames)
                {
                    //add into accountGroupMapping() in which check copy() of dictiory exists then update else take backup as copy of dictionay _accountGroupMapping
                    _accountGroupMapping[groupID].Add(GetAccountIdByName(fName));
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
        /// Get all client names for a particular Group
        /// </summary>
        /// <param name="masterFundName"> Selected Group/param>
        /// <returns>list of clientNames for selected group</returns>
        public static List<String> GetClientNamesForGroup(int groupID)
        {
            List<String> clientNameList = new List<string>();
            try
            {
                if (_clientGroupMapping.ContainsKey(groupID))
                {
                    foreach (int clientId in _clientGroupMapping[groupID])
                    {
                        if (_clientsCollection.ContainsKey(clientId))
                        {
                            clientNameList.Add(_clientsCollection[clientId]);
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
            return clientNameList;
        }

        /// <summary>
        /// Get all client names for mapped accounts of a particular Group 
        /// </summary>
        /// <param name="masterFundName"> Selected Group/param>
        /// <returns>list of client name for selected group</returns>
        public static List<String> GetClientNamesForMappedAccounts(int groupID)
        {
            List<String> clientNameList = new List<string>();
            try
            {
                if (_clientGroupMapping.ContainsKey(groupID))
                {
                    foreach (int clientId in _clientGroupMapping[groupID])
                    {
                        if (_clientGroupMapping.ContainsKey(groupID))
                        {
                            clientNameList.Add(_clientsCollection[clientId]);
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
            return clientNameList;
        }

        /// <summary>
        ///Add client names for Selected  group to mapping dictionary named as  _clientGroupMapping
        /// </summary>
        /// <param name="masterFundName">Selected groupID</param>
        /// <param name="accountNames">List of clientNames</param>
        public static void MapClientstoGroup(int groupID, List<String> clientNames)
        {
            try
            {
                IsBackInitialied();
                if (!_clientGroupMapping.ContainsKey(groupID))
                {
                    _clientGroupMapping.Add(groupID, new List<int>());
                }
                foreach (string cName in clientNames)
                {
                    //add into _clientGroupMapping in which check copy() of dictiory exists then update else take backup as copy of dictionay _clientGroupMapping
                    _clientGroupMapping[groupID].Add(GetClientIdByName(cName));
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
        /// Remove  client names for selected group from mapping dictionary named as  _accountGroupMapping
        /// </summary>
        /// <param name="masterFundName">Selected groupID</param>
        /// <param name="accountNames">List of clientNames</param>
        public static void UnMapClientsFromGroup(int groupID, List<String> clientNames)
        {
            try
            {
                // initialied Back up 
                IsBackInitialied();
                if (!_clientGroupMapping.ContainsKey(groupID))
                {
                    _clientGroupMapping.Add(groupID, new List<int>());
                }
                foreach (string cName in clientNames)
                {
                    // Remove from _clientGroupMapping which return true or false check back up() exists then updatebackup() else  call backup() method as copy of dictionary of _clientGroupMapping
                    _clientGroupMapping[groupID].Remove(GetClientIdByName(cName));
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
        /// Take Back Up of _accountGroupMapping
        /// </summary>
        public static bool IsBackInitialied()
        {
            if (isbackInitialied == false)
            {
                _backUpMappingCollection = new Dictionary<int, List<int>>(_accountGroupMapping.Count, _accountGroupMapping.Comparer);

                //making a deep copy of mapping object
                foreach (int groupId in _accountGroupMapping.Keys)
                {
                    _backUpMappingCollection.Add(groupId, new List<int>());
                    foreach (int accountID in _accountGroupMapping[groupId])
                    {
                        _backUpMappingCollection[groupId].Add(accountID);
                    }
                }
                isbackInitialied = true;
            }
            return isbackInitialied;
        }

        /// <summary>
        /// Get group Id from _accountCollection dictionary
        /// </summary>
        /// <param name="accountName">Given account name to get accountId </param>
        /// <returns>accountId of Given account name</returns>
        private static int GetAccountIdByName(String accountName)
        {
            int accountId = -1;
            try
            {
                foreach (int key in _allaccountCollection.Keys)
                {
                    if (_allaccountCollection[key] == accountName)
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
        /// Get client Id from _clientCollection dictionary
        /// </summary>
        /// <param name="accountName">Given client name to get clientId </param>
        /// <returns>accountId of Given client name</returns>
        public static int GetClientIdByName(String clientName)
        {
            int clientId = -1;
            try
            {
                foreach (int key in _clientsCollection.Keys)
                {
                    if (_clientsCollection[key] == clientName)
                    {
                        clientId = key;
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
            return clientId;
        }

        /// <summary>
        /// Get group Name from groupid
        /// </summary>
        /// <param name="accountName">Given account name to get accountId </param>
        /// <returns>accountId of Given account name</returns>
        //private static int GetGroupNameByID(int groupID)
        //{
        //    string groupName;
        //    try
        //    {
        //        groupName = AccountGroupSetupMappingDAL.GetGroupNameByID(groupID);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return accountId;
        //}

        /// <summary>
        /// For search in list according to search keyword
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="list"></param>
        /// <returns></returns>
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
        ///deep copy of _accountGroupMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUp()
        {
            try
            {
                _backUpMappingCollection = null;
                isbackInitialied = false;
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
        /// Convert Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Group and CashAccounts</param>
        /// <returns>Data Set which is collection of accountIds corresponding to Group Id</returns>
        private static DataSet GetDataSetFromDictionary(Dictionary<int, List<int>> _accountGroupMapping)
        {
            DataTable dt = new DataTable("TABAccountGroupMapping");
            dt.Columns.Add("GroupId", typeof(int));
            dt.Columns.Add("FundId", typeof(int));
            DataSet ds = new DataSet("DSAccountGroupMapping");
            try
            {
                foreach (int gId in _accountGroupMapping.Keys)
                {
                    foreach (int fId in _accountGroupMapping[gId])
                    {
                        dt.Rows.Add(gId, fId);
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
        /// Convert data set to Xml Document
        /// </summary>
        /// <param name="ds">Data set collection</param>
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
        /// returns max account group ID
        /// </summary>
        /// <returns></returns>
        public static int GetMaxAccountGroupID()
        {
            try
            {
                _accountGroupID = AccountGroupSetupMappingDAL.GetMaxGroupID();
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
            return _accountGroupID;
        }
        #endregion
    }
}
