using Microsoft.AspNetCore.SignalR;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Utility;
using Serilog;
using Serilog.Context;
using System.Collections.Concurrent;

namespace Prana.ServiceGateway.Hubs
{
    public class HubClientConnectionManagerRTPNLUpdates : IHubManagerRTPNLUpdates
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HubClientConnectionManagerRTPNLUpdates> _logger;
        private readonly IHubContext<ServiceGatewayHubRTPNLUpdates> _hubContext;

        public HubClientConnectionManagerRTPNLUpdates(IConfiguration configuration,
            ILogger<HubClientConnectionManagerRTPNLUpdates> logger,
            IHubContext<ServiceGatewayHubRTPNLUpdates> hubManager)
        {
            _configuration = configuration;
            _logger = logger;
            _hubContext = hubManager;
        }



        private static ConcurrentDictionary<string, List<string>> _userConnections = new ConcurrentDictionary<string, List<string>>();

        //This dictionary is responsible for holding the user's that are currently using rtpnl page.
        private static ConcurrentDictionary<int, bool> _userRtpnlUpdatesList = new ConcurrentDictionary<int, bool>();



        /// <summary>
        /// Method to add client connection in the dictionary for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param> 
        public static void AddConnection(string userId, string connectionId)
        {
            try
            {
                _userConnections.AddOrUpdate(userId,
                    new List<string> { connectionId },
                    (key, existingList) =>
                    {
                        lock (existingList)
                        {
                            if (!existingList.Contains(connectionId))
                            {
                                existingList.Add(connectionId);
                            }
                        }
                        return existingList;
                    });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding to the dictionary");
            }
        }

        /// <summary>
        /// Method to remove client connection in the dictionary for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param> 
        public static void RemoveConnection(string userId, string connectionId)
        {
            try
            {
                if (_userConnections.TryGetValue(userId, out var connections))
                {
                    lock (connections)
                    {
                        connections.Remove(connectionId);
                        if (connections.Count == 0)
                        {
                            _userConnections.TryRemove(userId, out _);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while removing from the dictionary");
            }
        }

        /// <summary>
        /// Method to fetch client connections from the dictionary for the user
        /// </summary>
        /// <param name="userId"></param>
        public static List<string> GetConnections(string userId)
        {
            try
            {
                if (_userConnections.TryGetValue(userId, out var connections))
                {
                    lock (connections)
                    {
                        // Returning a copy of the connections
                        return new List<string>(connections);
                    }
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching client connections");
                return new List<string>();
            }
        }



        /// <summary>
        /// Adds a userId to the list
        /// </summary>
        /// <param name="userId"></param>
        public static void AddUserToRTPNLUpdatesList(int userId)
        {
            try
            {
                // Add user to the list if they are not already present
                _userRtpnlUpdatesList.TryAdd(userId, true);
                Log.Information($"User {userId} added to the list.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding user to the list.");
            }
        }

        /// <summary>
        /// Removes a userId from the list
        /// </summary>
        /// <param name="userId"></param>
        public static void RemoveUserFromRTPNLUpdatesList(int userId)
        {
            try
            {
                // Remove user from the list
                var isRemoved = _userRtpnlUpdatesList.TryRemove(userId, out _);
                if (isRemoved)
                    Log.Information($"User {0} removed from the list.", userId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while removing user from the list. UserId:" + userId);
            }
        }

        /// <summary>
        /// Checks if a userId exists in the list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UserExistsInRTPNLUpdatesList(int userId)
        {
            try
            {
                // Check if userId exists in the list
                return _userRtpnlUpdatesList.ContainsKey(userId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking user existence in the list.");
                return false;
            }
        }

        /// <summary>
        /// Method to send the singalR message to all the connections of the user
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param> 
        public async void SendUserBasedMessage(string topic, RequestResponseModel message)
        {
            try
            {
                bool isForcefulBroadcast = Boolean.Parse(_configuration[ServicesMethodConstants.CONST_FORCEFULBROADCAST].ToString());
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);
                if (!isForcefulBroadcast)
                {
                    int userId = message.CompanyUserID;
                    //fetching connections for all the users
                    var connections = HubClientConnectionManagerRTPNLUpdates.GetConnections(userId.ToString());
                    if (userId != 0 && connections != null && connections.Count > 0)
                    {
                        _logger.LogTrace("sending user response for user id : " + userId + " , number of connections : " + connections.Count + " topic : " + topic);
                        foreach (var connectionId in connections)
                        {
                            //Passing signalR message client wise
                            await _hubContext.Clients.Client(connectionId).SendAsync(topic, message);
                        }
                    }
                    else
                    {
                        //(1)In case some issue comes in fetching userID connections , then switching to broadcast protocol 
                        //(2)When User Id comes 0 , then we will broadcast.
                        BroadcastMessage(message, topic);
                    }
                }
                else
                {
                    //Forcefully broadcasting to each client.
                    BroadcastMessage(message, topic);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending SignalR message");
                //in case some error has occurred , as a fallback we are publishing data
                BroadcastMessage(message, topic);
            }

        }

        /// <summary>
        /// Method to send the singalR message to all the connections of the user . Overload that works without requestresponseModel object
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param> 
        /// <param name="userId"></param>
        /// <param name="CorrelationId"></param>
        /// <param name="isCompressedData"></param> 
        public async void SendUserBasedMessage(string topic, string data, int userId, string CorrelationId, bool isCompressedData = false)
        {
            try
            {
                bool isForcefulBroadcast = Boolean.Parse(_configuration[ServicesMethodConstants.CONST_FORCEFULBROADCAST].ToString());
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, userId);
                if (!isForcefulBroadcast)
                {
                    //fetching connections for all the users
                    var connections = HubClientConnectionManagerRTPNLUpdates.GetConnections(userId.ToString());
                    if (userId != 0 && connections != null && connections.Count > 0)
                    {
                        _logger.LogTrace("sending user response data for user id : " + userId + " , number of connections : " + connections.Count + " topic : " + topic);
                        foreach (var connectionId in connections)
                        {
                            //byte[] compressedMessage = new byte[0];
                            //if (isCompressedData)
                            //{
                            //    compressedMessage = HelperFunctions.CompressData(data);
                            //}
                            //Passing signalR message client wise
                            //await _hubContext.Clients.Client(connectionId).SendAsync(topic, isCompressedData ? compressedMessage : data);
                            await _hubContext.Clients.Client(connectionId).SendAsync(topic, data);
                        }
                    }
                    else
                    {
                        //(1)In case some issue comes in fetching userID connections , then switching to broadcast protocol 
                        //(2)When User Id comes 0 , then we will broadcast.
                        BroadcastMessage(data, topic, userId);
                    }
                }
                else
                {
                    //Forcefully broadcasting to each client.
                    BroadcastMessage(data, topic, userId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending SignalR message");
                //in case some error has occurred , as a fallback we are publishing data
                BroadcastMessage(data, topic, userId);
            }

        }

        /// <summary>
        /// Method to broadcast the singalR message to all the users
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param> 
        public async void BroadcastMessage(RequestResponseModel message, string topic)
        {
            try
            {
                _logger.LogTrace("broadcast happened for user  : " + message.CompanyUserID + " for topic : " + topic);
                //broadcasting message to all signalR clients.
                await _hubContext.Clients.All.SendAsync(topic, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while broadcasting SignalR message");
            }
        }


        /// <summary>
        /// Method to broadcast the singalR message to all the users ,  Overload that works without requestresponseModel object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="topic"></param>
        /// <param name="Userid"></param>
        public async void BroadcastMessage(string data, string topic, int Userid)
        {
            try
            {
                _logger.LogTrace("broadcast data happened for user  : " + Userid + " for topic : " + topic);
                //broadcasting message to all signalR clients.
                await _hubContext.Clients.All.SendAsync(topic, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while broadcasting SignalR message");
            }
        }
    }
}
