using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper;
using Serilog;
using Newtonsoft.Json;
using Prana.ServiceGateway.Constants;
using Newtonsoft.Json.Linq;
using Prana.KafkaWrapper.Extension.Classes;

namespace Prana.ServiceGateway.CacheStore
{
    internal class TradingTicketCache
    {
        private Serilog.ILogger _logger = Log.ForContext<TradingTicketCache>();
        private static IKafkaManager _kafkaManager;

        /// <summary>
        /// Logged in users wise TT Data.
        /// </summary>
        private Dictionary<int, dynamic> _tradingTicketCacheData = new Dictionary<int, dynamic>();

        /// <summary>
        /// Logged in users wise broker Data.
        /// </summary>
        private Dictionary<int, dynamic> _brokerData = new Dictionary<int, dynamic>();

        public Dictionary<int, dynamic> BrokerData 
        {
            get { return _brokerData; }
        }

        #region SingletonInstance
        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The Singleton instance
        /// </summary>
        private static TradingTicketCache _tradingTicketCache = null;

        internal static TradingTicketCache GetInstance()
        {
            lock (_lock)
            {
                if (_tradingTicketCache == null)
                    _tradingTicketCache = new TradingTicketCache();
                return _tradingTicketCache;
            }
        }
        #endregion

        internal void Initialize(IKafkaManager kafkaManager)
        {
            try
            {
                _kafkaManager = kafkaManager;

                #region SubscribeAndConsume 
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTradingTicketDataResponse, KafkaManager_TradingTicketDataReceived);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTTCollectionBrokerDataResponse, KafkaManager_BrokerDataReceived);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_InitializeLoggedOutUsers);
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Initialize of Trading Ticket Cache");
            }
        }

        /// <summary>
        /// Fetch Trading Ticket Data for a specific user
        /// </summary>
        internal string GetTradingTicketData(int userId)
        {
            if (_tradingTicketCacheData.ContainsKey(userId))
            {
                return JsonConvert.SerializeObject(_tradingTicketCacheData[userId]);
            }
            return null; // Or return a default value
        }

        /// <summary>
        /// Fetch Broker Data for a specific user
        /// </summary>
        internal string GetBrokerData(int userId)
        {
            if (_brokerData.ContainsKey(userId))
            {
                return JsonConvert.SerializeObject(_brokerData[userId]);
            }
            return null; // Or return a default value
        }

        /// <summary>
        /// Creates user wise Trading Ticket Data cache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_TradingTicketDataReceived(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserId = message.CompanyUserID;
                if (!string.IsNullOrEmpty(message.Data))
                {
                    if (!_tradingTicketCacheData.ContainsKey(companyUserId))
                    {
                        _tradingTicketCacheData.Add(companyUserId, message.Data);
                    }
                    else
                    {
                        // Update existing cache data
                        _tradingTicketCacheData[companyUserId] = message.Data;
                    }

                    _logger.Information(MessageConstants.MSG_TRADING_TICKET_CACHE_UPDATED + companyUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in KafkaManager_TradingTicketDataReceived");
            }
        }

        /// <summary>
        /// Creates user wise Broker Data cache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_BrokerDataReceived(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserId = message.CompanyUserID;
                if (!string.IsNullOrEmpty(message.Data))
                {
                    if (!_brokerData.ContainsKey(companyUserId))
                    {
                        _brokerData.Add(companyUserId, message.Data);
                        _logger.Information(MessageConstants.MSG_BROKER_CACHE_UPDATED + companyUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in KafkaManager_BrokerDataReceived");
            }
        }

        /// <summary>
        /// Remove entry from Trading Ticket Data cache at logout
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_InitializeLoggedOutUsers(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserId = message.CompanyUserID;
                if (!string.IsNullOrEmpty(message.Data))
                {
                    ClearUserCacheData(companyUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in KafkaManager_InitializeLoggedOutUsers");
            }
        }

        public void ClearUserCacheData(int userId)
        {
            try
            {
                if (_tradingTicketCacheData.ContainsKey(userId))
                {
                    _tradingTicketCacheData.Remove(userId);
                    _logger.Information(MessageConstants.MSG_DATA_REMOVED_FROM_TRADING_TICKET_CACHE + userId);
                }
                if (_brokerData.ContainsKey(userId))
                {
                    _brokerData.Remove(userId);
                    _logger.Information(MessageConstants.MSG_DATA_REMOVED_FROM_BROKER_CACHE + userId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in ClearUserCacheData");
            }
        }

        /// <summary>
        /// Updated the cache data on broker connection change
        /// </summary> 
        /// <param name="message"></param>
        public void UpdateBrokerCacheData(RequestResponseModel message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message.Data))
                {
                    dynamic brokerStatus = JsonConvert.DeserializeObject<dynamic>(message.Data);
                    foreach (var data in _brokerData)
                    {
                        var brokerDataObj = JsonConvert.DeserializeObject<dynamic>(_brokerData[data.Key]);
                        var brokerInfo = JsonConvert.DeserializeObject<List<JObject>>(brokerDataObj.BrokerConnectionStatus.ToString());

                        foreach (var connection in brokerInfo)
                        {
                            if (connection[ServicesMethodConstants.CONST_COUNTER_PARTY_ID] == brokerStatus.CounterPartyID && connection[ServicesMethodConstants.CONST_CONNECTION_ID] == brokerStatus.ConnectionID)
                            {
                                // Update the connection with new broker status
                                connection[ServicesMethodConstants.CONST_CONNECTION_STATUS] = brokerStatus.ConnStatus;
                            }
                        }

                        brokerDataObj.BrokerConnectionStatus = JsonConvert.SerializeObject(brokerInfo);

                        // Update the cache
                        _brokerData[data.Key] = JsonConvert.SerializeObject(brokerDataObj);
                        _logger.Information(MessageConstants.MSG_BROKER_CONN_STATUS_UPDATED);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in KafkaManager_UpdateCacheData");
            }
        }
    }
}
