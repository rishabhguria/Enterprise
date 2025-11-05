using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class UserSetupManager
    {
        /// <summary>
        /// check backup of groupUserMapping is initialized or not
        /// </summary>
        public static bool isbackInitialied = false;
        /// <summary>
        /// Dictionary for  collection of groups
        /// </summary>
        static Dictionary<int, string> _groupCollection = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for  collection of groupsUserMapping
        /// </summary>
        static Dictionary<int, List<int>> _groupUserMapping = new Dictionary<int, List<int>>();
        /// <summary>
        /// Dictionary for  collection of tradingAccountUserMapping
        /// </summary>
        //static Dictionary<int, string> _tradingAccountUserMapping = new Dictionary<int, string>();
        /// <summary>
        ///Dictionary to take back up or collection of groups and companyUser mapping.
        ////
        static Dictionary<int, List<int>> _backUpMappingCollection;
        /// <summary>
        /// to store max user ID
        /// </summary>
        static int _userID;

        /// <summary>
        /// Load all data from data base 
        /// </summary>
        public static void InitialiseData(int userID)
        {
            try
            {
                //Load all group from Database
                _groupCollection = UserSetupMappingDAL.LoadGroupFromDb();
                //Load all group and User mapping from Database
                _groupUserMapping = UserSetupMappingDAL.LoadGroupUserMappingFromDb(userID);
                //Load all trading account and User mapping from Database
                //_tradingAccountUserMapping = UserSetupMappingDAL.LoadTradingAccountUserMappingFromDb(userID);
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
        /// get list of all unmapped groups
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static List<String> GetUnmappedGroups(int UserID)
        {
            List<String> unmappedGroups = new List<string>();
            try
            {
                //if a group is not assigned to the selected user
                //show it in the list of groups available for mapping
                foreach (string groupName in _groupCollection.Values)
                {
                    //Purpose: IN many to many user group mapping show all unmapped groups in list of available groups
                    if (!GetGroupNamesForCompanyUser(UserID).Contains(groupName))
                        unmappedGroups.Add(groupName);
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
            unmappedGroups.Sort();
            return unmappedGroups;
        }

        /// <summary>
        /// Function to get CompanyUser details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User GetCompanyUserDetailsFromDB(int userID)
        {
            User companyUser = new User();
            try
            {
                companyUser = UserSetupMappingDAL.GetCompanyUserDetails(userID);
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
            return companyUser;
        }

        /// <summary>
        /// Save the mapping of user and group
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>positive integer if the records are saved</returns>
        public static int SaveMapping(int userID, User user, Dictionary<int, string> dicTradingAccounts)
        {
            int errorNumber = 0;
            try
            {
                DataSet dsMapping = GetDataSetFromDictionary(_groupUserMapping);
                DataSet dsTradingAccountMapping = GetTradingDataSetFromDictionary(userID, dicTradingAccounts);
                String xmlDoc = ConvertDataSetToXml(dsMapping);
                string xmlTrading = ConvertDataSetToXml(dsTradingAccountMapping);
                errorNumber = UserSetupMappingDAL.SaveDataSetInDb(xmlDoc, user, xmlTrading);

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
            return errorNumber;
        }

        #region CommonFunctions

        /// <summary>
        /// Get all group names for a particular company user
        /// </summary>
        /// <param name="masterFundName"> Selected company user</param>
        /// <returns>list of AccountNames for selected group Name</returns>
        public static List<String> GetGroupNamesForCompanyUser(int UserID)
        {
            List<String> groupNameList = new List<string>();
            try
            {
                if (_groupUserMapping.ContainsKey(UserID))
                {
                    foreach (int groupId in _groupUserMapping[UserID])
                    {
                        if (_groupCollection.ContainsKey(groupId))
                        {
                            groupNameList.Add(_groupCollection[groupId]);
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
            return groupNameList;
        }

        /// <summary>
        /// Remove  group names for selected companyuser from mapping dictionary named as  _groupUserMapping
        /// </summary>
        /// <param name="masterFundName">Selected userID</param>
        /// <param name="accountNames">List of groupNames</param>
        public static void UnMapGroupsFromUser(int userID, List<String> groupNames)
        {
            try
            {
                // initialied Back up 
                IsBackInitialied();
                if (!_groupUserMapping.ContainsKey(userID))
                {
                    _groupUserMapping.Add(userID, new List<int>());
                }
                foreach (string grpName in groupNames)
                {
                    // Remove from groupUserMappingin which return true or false check back up() exists then updatebackup() else  call backup() method as copy of dictionary of _groupUserMapping
                    _groupUserMapping[userID].Remove(GetGroupIdByName(grpName));
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
        ///Add group names for Selected  companyUser to mapping dictionary named as  _groupUserMapping
        /// </summary>
        /// <param name="masterFundName">Selected userID</param>
        /// <param name="accountNames">List of groupNames</param>
        public static void MapGroupsToUser(int userID, List<String> groupNames)
        {
            try
            {
                IsBackInitialied();
                if (!_groupUserMapping.ContainsKey(userID))
                {
                    _groupUserMapping.Add(userID, new List<int>());
                }
                foreach (string grpName in groupNames)
                {
                    //add into groupUserMapping() in which check copy() of dictiory exists then update else take backup as copy of dictionay _groupUserMapping
                    _groupUserMapping[userID].Add(GetGroupIdByName(grpName));
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
        /// Take Back Up of _groupUserMapping
        /// </summary>
        public static bool IsBackInitialied()
        {
            if (isbackInitialied == false)
            {
                _backUpMappingCollection = new Dictionary<int, List<int>>(_groupUserMapping.Count, _groupUserMapping.Comparer);

                //making a deep copy of mapping object
                foreach (int userId in _groupUserMapping.Keys)
                {
                    _backUpMappingCollection.Add(userId, new List<int>());
                    foreach (int groupID in _groupUserMapping[userId])
                    {
                        _backUpMappingCollection[userId].Add(groupID);
                    }
                }
                isbackInitialied = true;
            }
            return isbackInitialied;
        }

        /// <summary>
        /// Get group Id from _groupCollection dictionary
        /// </summary>
        /// <param name="accountName">Given group name to get group id </param>
        /// <returns>group Id of Given group Name</returns>
        private static int GetGroupIdByName(String groupName)
        {
            int groupId = -1;
            try
            {
                foreach (int key in _groupCollection.Keys)
                {
                    if (_groupCollection[key] == groupName)
                    {
                        groupId = key;
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
            return groupId;
        }

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
        ///deep copy of _groupUserMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
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
        /// Convert _groupUserMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Group and Company User</param>
        /// <returns>Data Set which is collection of GroupIds corresponding to Company user Id</returns>
        private static DataSet GetDataSetFromDictionary(Dictionary<int, List<int>> _groupUserMapping)
        {
            DataTable dt = new DataTable("TABGroupCompanyUserMapping");
            dt.Columns.Add("CompanyUserId", typeof(int));
            dt.Columns.Add("GroupId", typeof(int));
            DataSet ds = new DataSet("DSGroupCompanyUserMapping");
            try
            {
                foreach (int uId in _groupUserMapping.Keys)
                {
                    foreach (int gId in _groupUserMapping[uId])
                    {
                        dt.Rows.Add(uId, gId);
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
        /// returns mapped trading accounts dataset
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="dictTradingAccounts"></param>
        /// <returns></returns>
        private static DataSet GetTradingDataSetFromDictionary(int userID, Dictionary<int, string> dictTradingAccounts)
        {
            DataTable dt = new DataTable("TABTradingAccountsUserMapping");
            dt.Columns.Add("CompanyUserId", typeof(int));
            dt.Columns.Add("TradingAccountId", typeof(int));
            DataSet ds = new DataSet("DSTradingAccountsUserMapping");
            try
            {
                foreach (int tId in dictTradingAccounts.Keys)
                {
                    dt.Rows.Add(userID, tId);
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
        /// <param name="ds">Data set collection of GroupIDs and CompanyUser Ids</param>
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
        /// returns max user ID
        /// </summary>
        /// <returns></returns>
        public static int GetMaxUserID()
        {
            try
            {
                _userID = UserSetupMappingDAL.GetMaxUserID();
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
            return _userID;
        }

        /// <summary>
        /// Get dictionary of all trading accounts
        /// </summary>
        /// <returns></returns>
        public static TradingAccounts GetTradingAccounts(int CompanyID)
        {
            TradingAccounts tradingAccounts = new TradingAccounts();
            try
            {
                Dictionary<int, string> dicTradingAccounts = CachedDataManager.GetInstance.GetAllTradingAccount();
                foreach (KeyValuePair<int, string> pair in dicTradingAccounts)
                {
                    TradingAccount account = new TradingAccount();
                    account.TradingAccountsID = pair.Key;
                    account.TradingShortName = pair.Value;
                    account.TradingAccountName = pair.Value;
                    account.CompanyID = CompanyID;

                    tradingAccounts.Add(account);
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
            return tradingAccounts;
        }

        /// <summary>
        /// Get dictionary of all mapped trading accounts
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllMappedTradingAccounts(int userID)
        {
            Dictionary<int, string> dicTradingAccounts = new Dictionary<int, string>();
            try
            {
                dicTradingAccounts = UserSetupMappingDAL.LoadTradingAccountUserMappingFromDb(userID);
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
            return dicTradingAccounts;
        }
        #endregion
    }
}
