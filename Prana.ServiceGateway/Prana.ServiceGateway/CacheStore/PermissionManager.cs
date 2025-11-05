using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Utility;
using Serilog;

namespace Prana.ServiceGateway.CacheStore
{
    public class PermissionManager
    {
        #region Variables

        private Serilog.ILogger _logger = Log.ForContext<PermissionManager>();
        private static IKafkaManager _kafkaManager;

        /// <summary>
        /// User wise permitted trading accounts cache
        /// </summary>
        Dictionary<int, List<int>> _userPermittedTradingAccounts = new Dictionary<int, List<int>>();

        /// <summary>
        /// User wise permitted accounts cache
        /// </summary>
        Dictionary<int, Dictionary<int, string>> _userPermittedAccountsDictionary = new Dictionary<int, Dictionary<int, string>>();

        /// <summary>
        /// User wise permitted funds
        /// </summary>
        Dictionary<int, List<int>> _userPermittedMasterFunds = new Dictionary<int, List<int>>();

        /// <summary>
        /// User wise unallocated accounts permission
        /// </summary>
        Dictionary<int, bool> _userUnallocatedAccountsPermission = new Dictionary<int, bool>();

        ///<summary>
        ///User wise market data permitted cache 
        ///</summary>
        Dictionary<int, bool> _userMarketDataPermissionCache = new Dictionary<int, bool>();

        #endregion

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The Singleton instance
        /// </summary>
        private static PermissionManager _permissionManager = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static PermissionManager GetInstance()
        {
            lock (_lock)
            {
                if (_permissionManager == null)
                    _permissionManager = new PermissionManager();
                return _permissionManager;
            }
        }
        #endregion

        public void Initialize(IKafkaManager kafkaManager)
        {
            try
            {
                _kafkaManager = kafkaManager;

                #region SubscribeAndConsume 
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_PermittedMasterFundsBasedOnFundsResponse, KafkaManager_CreateUserPermittedMasterFundsAndFunds);
                _ = _kafkaManager.Produce(KafkaConstants.TOPIC_InitializeLoggedInUserRequest, new RequestResponseModel(0, string.Empty));
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Initialize of Permission Manager");
            }
        }

        /// <summary>
        /// Get User list for which Blotter data can be send based on User Permitted Trading Accounts
        /// </summary>
        public Dictionary<string, string> GetPermittedUserToSendData(string data)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                foreach (var tradingAccounts in _userPermittedTradingAccounts)
                {
                    BlotterResponse blotterData = JsonConvert.DeserializeObject<BlotterResponse>(data);
                    List<BlotterOrder> orderTabData = new List<BlotterOrder>();
                    foreach (BlotterOrder orderTabOrder in blotterData.OrderTabData)
                    {
                        if (tradingAccounts.Value.Contains(orderTabOrder.TradingAccountID))
                        {
                            // Filter out non-permitted orders from the OrderCollection
                            orderTabOrder.OrderCollection = orderTabOrder.OrderCollection.Where(order => tradingAccounts.Value.Contains(order.TradingAccountID)).ToList();
                            orderTabData.Add(orderTabOrder);
                        }
                    }
                    blotterData.WorkingTabData = blotterData.WorkingTabData.Where(subOrder => tradingAccounts.Value.Contains(subOrder.TradingAccountID)).ToList();
                    blotterData.OrderTabData = orderTabData;
                    if (orderTabData.Count != 0 || blotterData.WorkingTabData.Count != 0)
                    {
                        result.Add(tradingAccounts.Key.ToString(), JsonConvert.SerializeObject(blotterData));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetPermittedUserToSendData");
            }
            return result;
        }

        /// <summary>
        /// GetUserPermittedFunds
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<int> GetUserPermittedMasterFunds(int userID)
        {
            try
            {
                return _userPermittedMasterFunds[userID];
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetUserPermittedFunds");
            }
            return null;
        }

        /// <summary>
        /// GetUserUnallocatedPermission
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool GetUserUnallocatedPermission(int userID)
        {
            try
            {
                return _userUnallocatedAccountsPermission[userID];
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetUserUnallocatedPermission");
            }
            return true;
        }

        private void KafkaManager_CreateUserPermittedMasterFundsAndFunds(string topic, RequestResponseModel message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message.Data))
                {
                    int companyUserID = message.CompanyUserID;

                    Dictionary<string, string> parsedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                    List<int> permittedMasterFunds = JsonConvert.DeserializeObject<List<int>>(parsedData["permittedMasterFunds"]);
                    Dictionary<int, string> permittedAccounts = JsonConvert.DeserializeObject<Dictionary<int, string>>(parsedData["permittedAccounts"]);
                    bool isUnallocatedAccountsPermission = Convert.ToBoolean(parsedData["isUnallocatedAccountPermitted"]);

                    _userPermittedAccountsDictionary[companyUserID] = permittedAccounts;
                    _userUnallocatedAccountsPermission[companyUserID] = isUnallocatedAccountsPermission;
                    _userPermittedMasterFunds[companyUserID] = permittedMasterFunds;

                    _logger.Information(MessageConstants.CONST_MASTER_FUND_CACHE_CREATED + companyUserID);
                }    
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in KafkaManager_CreateUserPermittedMasterFundsAndFunds");
            }
        }

        /// <summary>
        /// Fetch from user wise Market Data Permission cache
        /// </summary>
        /// <param name="companyUserID"></param>
        public bool FetchMarketDataPermission(int companyUserID)
        {
            try
            {
                if (_userMarketDataPermissionCache.ContainsKey(companyUserID))
                {
                    return _userMarketDataPermissionCache[companyUserID];
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in FetchMarketDataPermission");
            }
            return false;
        }

        /// <summary>
        /// Creates user wise Marlet Data Permission cache
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="marketDataPermission"></param>
        public void AddOrUpdateMarketDataPermissionCache(int companyUserID, string marketDataPermission)
        {
            try
            {
                if(_userMarketDataPermissionCache.ContainsKey(companyUserID))
                {
                    _userMarketDataPermissionCache[companyUserID] = Convert.ToBoolean(marketDataPermission);
                }
                else
                {
                    _userMarketDataPermissionCache.Add(companyUserID, Convert.ToBoolean(marketDataPermission));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in AddOrUpdateMarketDataPermissionCache");
            }
        }

        /// <summary>
        /// Filter Remove keys on the base of user wise permitted cache
        /// </summary>
        /// <param name="key"></param>
        public List<int> FilterAppliedForRemovedKeys(string key)
        {
            List<int> result = new List<int>();
            try
            {
                string compressionID = key.Split('_')[0];

                if (_userPermittedAccountsDictionary != null)
                {
                    foreach (KeyValuePair<int, Dictionary<int, string>> userPermittedAccount in _userPermittedAccountsDictionary)
                    {
                        if (userPermittedAccount.Value.ContainsKey(Convert.ToInt32(compressionID)) || (Convert.ToInt32(compressionID) == -1 && _userUnallocatedAccountsPermission[userPermittedAccount.Key]))
                        {
                            result.Add(userPermittedAccount.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in FilterAppliedForRemovedKeys");
            }
            return result;
        }

        /// <summary>
        /// Filter Data Updates on the base of user wise permitted cache
        /// </summary>
        /// <param name="data"></param>
        public List<int> FilterAppliedForDataUpdates(KeyValuePair<string, dynamic> data)
        {
            List<int> result = new List<int>();
            try
            {
                string compressionID = data.Key.Split('_')[0];
                dynamic compressionObject = data.Value;
                bool isPermissionUpdateOverride = string.IsNullOrEmpty(Convert.ToString(compressionObject.PermOverride))? true: Convert.ToBoolean(compressionObject.PermOverride);

                if (_userPermittedAccountsDictionary != null)
                {
                    foreach (KeyValuePair<int, Dictionary<int, string>> userPermittedAccount in _userPermittedAccountsDictionary)
                    {
                        if (userPermittedAccount.Value.ContainsKey(Convert.ToInt32(compressionID)) || (Convert.ToInt32(compressionID) == -1 && _userUnallocatedAccountsPermission[userPermittedAccount.Key]))
                        {
                            if ((_userMarketDataPermissionCache.ContainsKey(userPermittedAccount.Key) && _userMarketDataPermissionCache[userPermittedAccount.Key]) || isPermissionUpdateOverride)
                                result.Add(userPermittedAccount.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in CreateUserWisePermittedFundCache");
            }
            return result;
        }

        /// <summary>
        /// Filter Start up data on the base of user wise permitted cache
        /// </summary>
        /// <param name="data"></param>
        /// <param name="companyUserID"></param>
        public Dictionary<string, dynamic> FilterDataBasedOnPermittedAccountsForStartUpData(string data, int companyUserID = int.MinValue)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    Dictionary<string, dynamic> parsedData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);

                    foreach (KeyValuePair<string, dynamic> keyValuePair in parsedData)
                    {
                        string compressionID = keyValuePair.Key.Split('_')[0];
                        if (_userPermittedAccountsDictionary != null && _userPermittedAccountsDictionary.ContainsKey(companyUserID))
                        {
                            if (_userPermittedAccountsDictionary[companyUserID].ContainsKey(Convert.ToInt32(compressionID)) || (Convert.ToInt32(compressionID) == -1 && _userUnallocatedAccountsPermission[companyUserID]))
                            {
                                result.Add(keyValuePair.Key, keyValuePair.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in FilterDataBasedOnPermittedAccounts");
            }
            return result;
        }

        /// <summary>
        /// UpdateCacheOnLoginUser
        /// </summary>
        public void UpdateCacheOnLoginUser(dynamic companyUserData)
        {
            try
            {
                int userId = Convert.ToInt32(companyUserData.CompanyUserID);
                var tradingAccounts = companyUserData.TradingAccounts;
                //This call is needed in order to create _userPermitted cache for already logged in user(in case of quit and restart)
                _ = _kafkaManager.Produce(KafkaConstants.TOPIC_InitializeLoggedInUserRequest, new RequestResponseModel(0, string.Empty));
                if (tradingAccounts != null)
                {
                    List<int> tradingAccountIds = new List<int>();
                    foreach (var account in tradingAccounts)
                        tradingAccountIds.Add(Convert.ToInt32(account.TradingAccountID));

                    if (tradingAccountIds != null)
                        _userPermittedTradingAccounts[userId] = tradingAccountIds;
                }
                else
                    _logger.Information(MessageConstants.MSG_TRADING_ACCOUNTS_NOT_AVAILABLE + " {userId}", userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpdateCacheOnLoginUser");
            }
        }

        /// <summary>
        /// UpdateCacheOnLogoutUser
        /// </summary>
        public void UpdateCacheOnLogoutUser(int companyUserId)
        {
            try
            {
                if (_userPermittedTradingAccounts.ContainsKey(companyUserId))
                {
                    _userPermittedTradingAccounts.Remove(companyUserId);
                }
                if (_userPermittedAccountsDictionary.ContainsKey(companyUserId))
                {
                    _userPermittedAccountsDictionary.Remove(companyUserId);
                }
                if (_userPermittedMasterFunds.ContainsKey(companyUserId))
                {
                    _userPermittedMasterFunds.Remove(companyUserId);
                }
                if (_userUnallocatedAccountsPermission.ContainsKey(companyUserId))
                {
                    _userUnallocatedAccountsPermission.Remove(companyUserId);
                }
                if(_userMarketDataPermissionCache.ContainsKey(companyUserId))
                {
                    _userMarketDataPermissionCache.Remove(companyUserId);   
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpdateCacheOnLogoutUser");
            }
        }

        /// <summary>
        /// CheckIfLoggedInUserExists - This method will help to check if the user is logged in Web Application or not.
        /// </summary>
        public bool CheckIfLoggedInUserExists(int companyUserId)
        {
            return _userPermittedTradingAccounts.ContainsKey(companyUserId);
        }
    }
}