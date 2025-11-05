using Prana.Authentication.Common;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

namespace Prana.Authentication
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class ClientConnectivity : IClientConnectivityService
    {
        private static readonly Dictionary<int, ClientInfo> _dictLoggedInUser = new Dictionary<int, ClientInfo>();
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static ClientConnectivity _clientConnectivity = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static ClientConnectivity GetInstance()
        {
            lock (_lock)
            {
                if (_clientConnectivity == null)
                    _clientConnectivity = new ClientConnectivity();
                return _clientConnectivity;
            }
        }
        #endregion

        /// <summary>
        /// Adds Client info into cache.
        /// </summary>
        /// <param name="companyUserId"></param>
        public void AddClientInfoInCache(int companyUserId)
        {
            try
            {
                if (!_dictLoggedInUser.ContainsKey(companyUserId))
                {
                    ClientInfo obj = new ClientInfo();
                    obj.CompanyUserId = companyUserId;
                    obj.Callback.Add(companyUserId, OperationContext.Current.GetCallbackChannel<IClientConnectivityServiceCallback>());
                    _dictLoggedInUser.Add(companyUserId, obj);
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

        /// <summary>
        /// Removes Client info from cache.
        /// </summary>
        /// <param name="companyUserID"></param>
        public void RemoveClientInfoFromCache(int companyUserID, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb)
        {
            try
            {
                lock (_dictLoggedInUser)
                {
                    if (_dictLoggedInUser.ContainsKey(companyUserID) && _dictLoggedInUser[companyUserID].Callback.ContainsKey(companyUserID))
                    {
                        IClientConnectivityServiceCallback iClientConnectivityServiceCallback = _dictLoggedInUser[companyUserID].Callback[companyUserID];
                        _dictLoggedInUser[companyUserID].Callback.Remove(companyUserID);
                        _dictLoggedInUser.Remove(companyUserID);

                        if (isForcefulLogoutEnterprise)
                        {
                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                try
                                {
                                    iClientConnectivityServiceCallback.ClientConnectivityResponseEnterprise(companyUserID, true);
                                }
                                catch (Exception ex)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_KILLING_REQUEST_ERROR_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTENTERPRISE + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME + " Error: " + ex.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                }
                            }));
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_REMOVED_USER_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTENTERPRISE + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }
                        else if (isForcefulLogoutWeb)
                        {
                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                try
                                {
                                    iClientConnectivityServiceCallback.ClientConnectivityResponseWeb(companyUserID, true);
                                }
                                catch (Exception ex)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_KILLING_REQUEST_ERROR_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTWEB + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME + " Error: " + ex.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                }
                            }));
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_REMOVED_USER_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTWEB + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (isForcefulLogoutEnterprise)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_KILLING_REQUEST_ERROR_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTENTERPRISE + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME + " Error: " + ex.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                else if (isForcefulLogoutWeb)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_FORCEFULLY_KILLING_REQUEST_ERROR_FROM_CACHE + companyUserID + AuthenticationConstants.MSG_ISFORCEFULLOGOUTWEB + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME + " Error: " + ex.Message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the cache for logged in user from Samsara.
        /// </summary>
        /// <param name="companyUserID"></param>
        public AuthenticatedUserInfo UpdateCacheForLoginUser(string companyUserId, string userName, string token)
        {
            AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
            try
            {
                if (!string.IsNullOrEmpty(companyUserId))
                {
                    authUser = AuthenticateUser.GetInstance().CheckUserAlreadyLoggedIn(int.Parse(companyUserId), userName, true, token);
                    if (string.IsNullOrEmpty(authUser.ErrorMessage) || authUser.AuthenticationType==AuthenticationTypes.WebAlreadyLoggedInForAnotherWebSession)
                    {
                        AuthenticateUser.GetInstance().AddLoggedInUser(int.Parse(companyUserId));
                        AddClientInfoInCache(Convert.ToInt32(companyUserId));
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_CACHE_UPDATED + companyUserId + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    else
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_UPDATE_CACHE_REQUEST_FAILED + authUser.ErrorMessage + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
            return authUser;
        }
    }
}
