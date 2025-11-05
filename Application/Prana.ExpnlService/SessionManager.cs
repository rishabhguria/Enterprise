using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.ExpnlService
{
    class SessionManager
    {
        #region Properties Description
        //DistinctAccountPermissionSets
        //(1)           F1
        //(2)           F2
        //(3)           F3
        //(4)           F4
        //(5)           F5
        //(6)           -1
        //(7)           F1, F2, F3, F4, F5, -1
        //(8)           F1, F2, F3
        //(9)           F1, F2, F4
        //(10)          F1, F4, F5
        //(11)          F1, F3, F4
        //(12)          F1, F2
        //(13)          F3, F4, F5

        //UserAndDistinctAccountPermissionSetsMapping
        //U1            (7)         --All account permission
        //U2            (8)         --F1, F2, F3 permission
        //U3            (9)         --F1, F2, F4 permission
        //U4            (7)         --All account permission
        //U5            (11)        --F1, F3, F4 permission
        //U6            (8)         --F1, F2, F3 permission

        //AccountAndDistinctAccountPermissionSetsMapping
        //F1            (1)
        //F2            (2)
        //F3            (3)
        //F4            (4)
        //F5            (5)
        //-1            (6)

        //DynamicAccountSetandDistinctAccountPermissionSetMapping
        //10024         (12)
        //10025         (13)
        //10026         (1)
        #endregion

        private static object _lockerObj = new object();

        private static int _uniqueAutoIncrementID = 1;
        public static int UniqueAutoIncrementID
        {
            get { return _uniqueAutoIncrementID; }
            set { _uniqueAutoIncrementID = value; }
        }

        //AccountID Vs DistinctAccountPermissionSets Key
        private static Dictionary<int, int> _accountAndDistinctAccountPermissionSetsMapping = new Dictionary<int, int>();
        public static Dictionary<int, int> AccountAndDistinctAccountPermissionSetsMapping
        {
            get { return _accountAndDistinctAccountPermissionSetsMapping; }
        }

        //UserID Vs DistinctAccountPermissionSets Key
        private static Dictionary<string, int> _userAndDistinctAccountPermissionSetsMapping = new Dictionary<string, int>();

        //DistinctAccountPermissionSets Key (Unique Auto Increment ID) Vs Distinct Account Permission Sets
        private static Dictionary<int, List<int>> _distinctAccountPermissionSets = new Dictionary<int, List<int>>();
        public static Dictionary<int, List<int>> DistinctAccountPermissionSets
        {
            get { return _distinctAccountPermissionSets; }
        }

        private static int _uniqueDynamicAutoIncrementID = 10024;
        public static int UniqueDynamicAutoIncrementID
        {
            get { return _uniqueDynamicAutoIncrementID; }
            set { _uniqueDynamicAutoIncrementID = value; }
        }

        //DynamicDistinctAccountPermissionSets Key (Unique Dynamic Auto Increment ID) Vs Dynamic Distinct Account Permission Sets
        private static Dictionary<int, int> _dynamicDistinctAccountPermissionSets = new Dictionary<int, int>();
        public static Dictionary<int, int> DynamicDistinctAccountPermissionSets
        {
            get { return _dynamicDistinctAccountPermissionSets; }
        }

        //DynamicDistinctAccountPermissionSets Key  Vs List <tab,UserIDs>
        private static Dictionary<int, List<Tuple<string, string>>> _dynamicDistinctAccountPermissionSetsAndUsersMapping = new Dictionary<int, List<Tuple<string, string>>>();
        public static Dictionary<int, List<Tuple<string, string>>> DynamicDistinctAccountPermissionSetsAndUsersMapping
        {
            get { return _dynamicDistinctAccountPermissionSetsAndUsersMapping; }
        }

        public static void AddDistinctAccountPermissionSet(string userID, List<int> listOfaccounts)
        {
            try
            {
                lock (_lockerObj)
                {
                    int count = 0;
                    foreach (KeyValuePair<int, List<int>> kvp in _distinctAccountPermissionSets)
                    {
                        //if the count is different, then set will be distinct for sure. Thus we have to check only for equal number of accounts
                        if (kvp.Value.Count == listOfaccounts.Count)
                        {
                            count = 0;
                            foreach (int accountID in listOfaccounts)
                            {
                                if (!kvp.Value.Contains(accountID))
                                {
                                    count++;
                                    break;
                                }
                            }

                            //Means same accountset permission already exist in the system, So providing the same DistinctAccountKey to new user
                            if (count == 0)
                            {
                                if (!_userAndDistinctAccountPermissionSetsMapping.ContainsKey(userID))
                                {
                                    _userAndDistinctAccountPermissionSetsMapping.Add(userID, kvp.Key);
                                }
                                break;
                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                    if (count > 0 || _distinctAccountPermissionSets.Count == 0)
                    {
                        _distinctAccountPermissionSets.Add(_uniqueAutoIncrementID, listOfaccounts);

                        if (!_userAndDistinctAccountPermissionSetsMapping.ContainsKey(userID))
                            _userAndDistinctAccountPermissionSetsMapping.Add(userID, _uniqueAutoIncrementID);

                        _uniqueAutoIncrementID++;
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

        public static void AddDynamicDistinctAccountPermissionSets(string userID, string tabKey, List<int> listOfAccounts)
        {
            try
            {
                lock (_lockerObj)
                {
                    int count = 0;
                    foreach (KeyValuePair<int, List<int>> kvp in DistinctAccountPermissionSets)
                    {
                        if (kvp.Value.Count == listOfAccounts.Count)
                        {
                            count = 0;
                            if (listOfAccounts.Any(accountID => !kvp.Value.Contains(accountID)))
                            {
                                count++;
                            }

                            if (count == 0)
                            {
                                int dynamicDistinctAccountSetKey = _dynamicDistinctAccountPermissionSets.FirstOrDefault(dynamicPermissionSet => dynamicPermissionSet.Value == kvp.Key).Key;
                                if (dynamicDistinctAccountSetKey == 0)
                                {
                                    dynamicDistinctAccountSetKey = UniqueDynamicAutoIncrementID;
                                    _dynamicDistinctAccountPermissionSets.Add(dynamicDistinctAccountSetKey, kvp.Key);
                                    _dynamicDistinctAccountPermissionSetsAndUsersMapping.Add(dynamicDistinctAccountSetKey,
                                        new List<Tuple<string, string>> { new Tuple<string, string>(tabKey, userID) });
                                    UniqueDynamicAutoIncrementID++;
                                }
                                else if (_dynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(dynamicDistinctAccountSetKey))
                                {
                                    if (!_dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Contains(new Tuple<string, string>(tabKey, userID)))
                                    {
                                        _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Add(new Tuple<string, string>(tabKey, userID));
                                    }
                                }
                                break;
                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                    if (count > 0 || _distinctAccountPermissionSets.Count == 0)
                    {
                        _distinctAccountPermissionSets.Add(_uniqueAutoIncrementID, listOfAccounts);
                        _dynamicDistinctAccountPermissionSets.Add(_uniqueDynamicAutoIncrementID, _uniqueAutoIncrementID);

                        if (!_dynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(_uniqueDynamicAutoIncrementID))
                            _dynamicDistinctAccountPermissionSetsAndUsersMapping.Add(_uniqueDynamicAutoIncrementID,
                                new List<Tuple<string, string>> { new Tuple<string, string>(tabKey, userID) });

                        _uniqueDynamicAutoIncrementID++;
                        _uniqueAutoIncrementID++;
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

        public static void DeleteDynamicDistinctAccountPermissionSet(string userID, string tabKey, List<int> listOFaccounts)
        {
            try
            {
                lock (_lockerObj)
                {
                    int distictAccountSetWisePermissionKey = _distinctAccountPermissionSets.FirstOrDefault(accountPermissionSet => IsContentEqualInLists(accountPermissionSet.Value, listOFaccounts)).Key;
                    if (!_accountAndDistinctAccountPermissionSetsMapping.Values.Contains(distictAccountSetWisePermissionKey) &&
                        !_userAndDistinctAccountPermissionSetsMapping.Values.Contains(distictAccountSetWisePermissionKey))
                    {
                        int dynamicDistinctAccountSetKey = _dynamicDistinctAccountPermissionSets.FirstOrDefault(dynamicPermissionSet => dynamicPermissionSet.Value == distictAccountSetWisePermissionKey).Key;
                        if (_dynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(dynamicDistinctAccountSetKey))
                        {
                            if (_dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Count == 1 &&
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First().Item1 == tabKey && _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First().Item2 == userID)
                            {
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping.Remove(dynamicDistinctAccountSetKey);
                                _dynamicDistinctAccountPermissionSets.Remove(dynamicDistinctAccountSetKey);
                                _distinctAccountPermissionSets.Remove(distictAccountSetWisePermissionKey);
                            }
                            else if (_dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Count > 1)
                            {
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Remove(
                                    _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First(
                                        x => x.Item1 == tabKey && x.Item2 == userID));
                            }
                        }
                    }
                    else
                    {
                        int dynamicDistinctAccountSetKey = _dynamicDistinctAccountPermissionSets.FirstOrDefault(dynamicPermissionSet => dynamicPermissionSet.Value == distictAccountSetWisePermissionKey).Key;
                        if (_dynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(dynamicDistinctAccountSetKey))
                        {
                            if (_dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Count == 1 &&
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First().Item1 == tabKey && _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First().Item2 == userID)
                            {
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping.Remove(dynamicDistinctAccountSetKey);
                                _dynamicDistinctAccountPermissionSets.Remove(dynamicDistinctAccountSetKey);
                            }
                            else if (_dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Count > 1)
                            {
                                _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].Remove(
                                    _dynamicDistinctAccountPermissionSetsAndUsersMapping[dynamicDistinctAccountSetKey].First(
                                        x => x.Item1 == tabKey && x.Item2 == userID));
                            }
                        }
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

        internal static bool IsContentEqualInLists(List<int> list1, List<int> list2)
        {
            return list1.Count == list2.Count // assumes unique values in each list
                && new HashSet<int>(list1).SetEquals(list2);
        }

        public static void RemoveClient(string userId)
        {
            try
            {
                lock (_lockerObj)
                {
                    bool isRemoveDistinctAccountSet = true;
                    bool isDistinctAccountSetInAccountOrMasterFundOrUserSet = false;
                    if (_userAndDistinctAccountPermissionSetsMapping.ContainsKey(userId))
                    {
                        int distinctAccountKey = _userAndDistinctAccountPermissionSetsMapping[userId];
                        _userAndDistinctAccountPermissionSetsMapping.Remove(userId);

                        foreach (KeyValuePair<int, int> kvp in _accountAndDistinctAccountPermissionSetsMapping)
                        {
                            if (kvp.Value == distinctAccountKey)
                            {
                                isDistinctAccountSetInAccountOrMasterFundOrUserSet = true;
                                break;
                            }
                        }

                        if (!isDistinctAccountSetInAccountOrMasterFundOrUserSet)
                        {
                            foreach (KeyValuePair<string, int> kvp in _userAndDistinctAccountPermissionSetsMapping)
                            {
                                if (kvp.Value == distinctAccountKey)
                                {
                                    isDistinctAccountSetInAccountOrMasterFundOrUserSet = true;
                                    break;
                                }
                            }
                        }

                        if (!isDistinctAccountSetInAccountOrMasterFundOrUserSet)
                        {
                            List<int> probableRemovableKeys = _dynamicDistinctAccountPermissionSets.Where(x => x.Value == distinctAccountKey).Select(probableRemovableKvp => probableRemovableKvp.Key).ToList();
                            if (probableRemovableKeys.Where(probableRemovableKey => _dynamicDistinctAccountPermissionSetsAndUsersMapping.ContainsKey(probableRemovableKey)).Any(probableRemovableKey => _dynamicDistinctAccountPermissionSetsAndUsersMapping[probableRemovableKey].Any(
                                x => (x.Item2 == userId)) && _dynamicDistinctAccountPermissionSetsAndUsersMapping[probableRemovableKey].Count > 1))
                            {
                                isRemoveDistinctAccountSet = false;
                            }
                            if (isRemoveDistinctAccountSet)
                            {
                                _distinctAccountPermissionSets.Remove(distinctAccountKey);
                            }
                        }

                        List<int> removableUSerMappingKeys = new List<int>();
                        foreach (KeyValuePair<int, List<Tuple<string, string>>> dynamicUserKeyValuePair in _dynamicDistinctAccountPermissionSetsAndUsersMapping)
                        {
                            if (dynamicUserKeyValuePair.Value.All(tup => tup.Item2 == userId))
                            {
                                removableUSerMappingKeys.Add(dynamicUserKeyValuePair.Key);
                            }
                            else
                            {
                                List<Tuple<string, string>> listTupples = dynamicUserKeyValuePair.Value.Where(tup => tup.Item2 == userId).ToList();

                                var pair = dynamicUserKeyValuePair;
                                foreach (Tuple<string, string> tuple in listTupples.Where(tuple => pair.Value.Contains(tuple)))
                                {
                                    dynamicUserKeyValuePair.Value.Remove(tuple);
                                }
                            }
                        }

                        foreach (int removableUserMappingKey in removableUSerMappingKeys)
                        {
                            _dynamicDistinctAccountPermissionSets.Remove(removableUserMappingKey);
                            _dynamicDistinctAccountPermissionSetsAndUsersMapping.Remove(removableUserMappingKey);
                        }

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
    }
}
