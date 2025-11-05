using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Hubs;

namespace Prana.ServiceGateway.Services
{
    public class AuthenticateUserService : IAuthenticateUserService, IDisposable
    {
        private readonly ILogger<AuthenticateUserService> _logger;
        private readonly IKafkaManager _kafkaManager;

        private readonly IHubManager _hubManager;

        public AuthenticateUserService(IKafkaManager kafkaManager, IHubManager hubManager,
            ILogger<AuthenticateUserService> logger)
        {
            this._logger = logger;
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
        }

        public void Initialize()
        {
            try
            {
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceInitialized, AuthServiceInitialized);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_ForcefulLogoutWeb, ForcefulLogoutWeb);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergEventResponse, ProcessBloombergEventResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Initialize of AuthenticateUserService");
            }
        }
        public async Task<string> LoginUser(RequestResponseModel requestResponseObj)
        {
            try
            {
                Task<string> task = _kafkaManager.ProduceAndConsume(requestResponseObj, KafkaConstants.TOPIC_AuthServiceRequest, KafkaConstants.TOPIC_AuthServiceResponse);
                return await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoginUser of AuthenticateUserService");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task<string> LogoutUser(RequestResponseModel requestResponseObj)
        {
            try
            {
                Task<string> task = _kafkaManager.ProduceAndConsume(requestResponseObj, KafkaConstants.TOPIC_LogoutRequest, KafkaConstants.TOPIC_LogoutResponse);
                return await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LogoutUser of AuthenticateUserService");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get status of services running or not(trade,auth,gateway)
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetStatusForLogin(RequestResponseModel requestResponseObj)
        {
            try
            {
                Task<string> task = _kafkaManager.ProduceAndConsume(requestResponseObj, KafkaConstants.TOPIC_ServiceStatusRequest, KafkaConstants.TOPIC_ServiceStatusResponse);
                return await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStatusForLogin of AuthenticateUserService");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Update cache for login user in trade server
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateCacheForLoginUser(RequestResponseModel requestResponseObj)
        {
            try
            {
                _kafkaManager.Produce(KafkaConstants.TOPIC_UpdateCacheRequestForLoginUser, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCacheForLoginUser of AuthenticateUserService");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }

        private void ForcefulLogoutWeb(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_LogoutResponse, message.CompanyUserID.ToString(), message.CompanyUserID, message.CorrelationId);
                var tradingTicketCache = TradingTicketCache.GetInstance();
                tradingTicketCache.ClearUserCacheData(message.CompanyUserID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForcefulLogoutWeb");
            }
        }

        public void ConnectionStatusDisconnected()
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_ConnectionStatusDisconnected, KafkaConstants.TOPIC_StatusDisconnected, 0, String.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConnectionStatusDisconnected");
            }
        }

        private void AuthServiceInitialized(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_AuthServiceInitialized, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthServiceInitialized");
            }
        }

        /// <summary>
        /// This Method handles the encryption of password when user enters password from Swagger UI 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<int> EncrytPassword(string password)
        {
            try
            {
                List<int> asciiCodes = new List<int>();
                for (int i = 0; i < password.Length; i++)
                {
                    char character = password[i];
                    int asciiCode = (int)character;
                    if (i % 2 == 0)
                    {
                        asciiCode += 2;
                    }
                    else
                    {
                        asciiCode -= 1;
                    }
                    asciiCodes.Add(asciiCode);
                }
                return asciiCodes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ProcessBloombergAuthentication(RequestResponseModel requestResponseObj)
        {
            try
            {
                if (PermissionManager.GetInstance().CheckIfLoggedInUserExists(requestResponseObj.CompanyUserID))
                {
                    _kafkaManager.Produce(KafkaConstants.TOPIC_BloombergAuthentication, requestResponseObj);
                }
                else
                {
                    _logger.LogInformation("User is not logged in, so not processing Bloomberg Authentication . User Id : " + requestResponseObj.CompanyUserID);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessBloombergAuthentication");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void ProcessBloombergEventResponse(string topic, RequestResponseModel message)
        {
            try
            {
                PermissionManager.GetInstance().AddOrUpdateMarketDataPermissionCache(message.CompanyUserID, message.Data);

                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_BloombergEventResponse + "_" + message.CompanyUserID.ToString(), message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessBloombergEventResponse");
            }
        }
    }
}
