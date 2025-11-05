using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.ExceptionHandling;
using System.Collections.Generic;
using System;
using Prana.ServiceGateway.Contracts;
using Prana.KafkaWrapper.Extension.Classes;

namespace Prana.ServiceGateway.Services
{
    public class WatchlistDataService : IWatchlistDataService, IDisposable
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly ILogger<WatchlistDataService> _logger;

        public WatchlistDataService(IKafkaManager kafkaManager,
            ILogger<WatchlistDataService> logger)
        {
            _kafkaManager = kafkaManager;
            this._logger = logger;
        }

        public async Task<Dictionary<string, int>> GetTabNames(int userId)
        {
            try
            {
                Dictionary<string, int> data = null;
                RequestResponseModel requestResponseObj = new(userId, null, null);

                string message = await _kafkaManager.ProduceAndConsume(requestResponseObj, KafkaConstants.TOPIC_WatchlistTabNamesRequest, KafkaConstants.TOPIC_WatchlistTabNamesResponse);
                if (message != null)
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, int>>(message);
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTabNames");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task<Dictionary<string, HashSet<string>>> GetTabWiseSymbols(int userId)
        {
            try
            {
                Dictionary<string, HashSet<string>> data = null;
                RequestResponseModel requestResponseObj = new(userId, null, null);
                string message = await _kafkaManager.ProduceAndConsume(requestResponseObj, 
                    KafkaConstants.TOPIC_WatchlistTabWiseSymbolsRequest, 
                    KafkaConstants.TOPIC_WatchlistTabWiseSymbolsResponse);

                if (message != null)
                    data = JsonConvert.DeserializeObject<Dictionary<string, HashSet<string>>>(message);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTabWiseSymbols");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task AddTab(int userId, string tabName)
        {
            try
            {
                RequestResponseModel requestResponseObj = new(userId, tabName, null);
                await _kafkaManager.Produce(KafkaConstants.TOPIC_WatchlistAddTabRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task RenameTab(int userId, string oldTabName, string newTabName)
        {
            try
            {
                RequestResponseModel requestResponseObj = new(userId, 
                    JsonConvert.SerializeObject(new string[] { oldTabName, newTabName }),
                    null);
                await _kafkaManager.Produce(KafkaConstants.TOPIC_WatchlistRenameTabRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RenameTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task DeleteTab(int userId, string tabName)
        {
            try
            {
                RequestResponseModel requestResponseObj = new(userId, tabName, null);
                await _kafkaManager.Produce(KafkaConstants.TOPIC_WatchlistDeleteTabRequest, 
                    requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task AddSymbolInTab(int userId, string symbol, string tabName)
        {
            try
            {
                RequestResponseModel requestResponseObj = new(userId, JsonConvert.SerializeObject(new string[] { symbol, tabName }), null);

                await _kafkaManager.Produce(KafkaConstants.TOPIC_WatchlistAddSymbolInTabRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddSymbolInTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task RemoveSymbolFromTab(int userId, string symbol, string tabName)
        {
            try
            {
                RequestResponseModel requestResponseObj = new(userId, 
                    JsonConvert.SerializeObject(new string[] { symbol, tabName }),
                    null);
                await _kafkaManager.Produce(KafkaConstants.TOPIC_WatchlistRemoveSymbolFromTabRequest, 
                    requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveSymbolFromTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
