using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SAPIAdapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prana.MarketDataPermissionService
{
    internal class SapiMarketDataPermissionDetail : MarketDataPermissionDetail
    {
        SapiMarketDataPermissionDetail()
        {
            _sapiManager.AuthenticateUserResponse += AuthenticateUserRespnse;
        }
        private string SapiUsername { get; set; }

        private static Dictionary<int, SapiMarketDataPermissionDetail> _dictUserWiseMarketDataPermissionDetail = new Dictionary<int, SapiMarketDataPermissionDetail>();
        private static SAPIManager _sapiManager = SAPIManager.GetInstance();

        /// <summary>
        /// Sends Permission check request to market data.
        /// </summary>
        /// <param name="marketDataPermissionRequest"></param>
        /// <param name="source"></param>
        /// <param name="iMarketDataPermissionServiceCallback"></param>
        internal static void PermissionCheck(MarketDataPermissionRequest marketDataPermissionRequest, string source, IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback, string userDetails)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(userDetails) && String.Equals(source, SAPIConstants.SAMSARA, StringComparison.OrdinalIgnoreCase))
                {
                    //Handling for Samsara SAPI user 
                    _sapiManager.GetBloombergAuthenticationToken(userDetails);
                }
                else
                {
                    //Handlign for Enterprise SAPI user
                    SapiMarketDataPermissionRequest sapiMarketDataPermissionRequest = ((SapiMarketDataPermissionRequest)marketDataPermissionRequest);

                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SAPI user PermissionCheck request. UserID: {0}, SAPI Username: {1}, IP Address: {2}", sapiMarketDataPermissionRequest.CompanyUserID, sapiMarketDataPermissionRequest.SapiUsername, sapiMarketDataPermissionRequest.IpAddress), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);

                    if (!string.IsNullOrWhiteSpace(sapiMarketDataPermissionRequest.SapiUsername))
                    {
                        lock (_dictUserWiseMarketDataPermissionDetail)
                        {
                            if (!_dictUserWiseMarketDataPermissionDetail.ContainsKey(sapiMarketDataPermissionRequest.CompanyUserID))
                            {
                                SapiMarketDataPermissionDetail obj = new SapiMarketDataPermissionDetail();
                                obj.CompanyUserID = sapiMarketDataPermissionRequest.CompanyUserID;
                                obj.SapiUsername = sapiMarketDataPermissionRequest.SapiUsername;
                                obj.Callback.Add(source, iMarketDataPermissionServiceCallback);

                                _dictUserWiseMarketDataPermissionDetail.Add(sapiMarketDataPermissionRequest.CompanyUserID, obj);
                            }
                            else
                            {
                                _dictUserWiseMarketDataPermissionDetail[sapiMarketDataPermissionRequest.CompanyUserID].SapiUsername = sapiMarketDataPermissionRequest.SapiUsername;

                                if (_dictUserWiseMarketDataPermissionDetail[sapiMarketDataPermissionRequest.CompanyUserID].Callback.ContainsKey(source))
                                {
                                    _dictUserWiseMarketDataPermissionDetail[sapiMarketDataPermissionRequest.CompanyUserID].Callback[source] = iMarketDataPermissionServiceCallback;
                                }
                                else
                                {
                                    _dictUserWiseMarketDataPermissionDetail[sapiMarketDataPermissionRequest.CompanyUserID].Callback.Add(source, iMarketDataPermissionServiceCallback);
                                }
                            }

                            _sapiManager.SendAuthenticateUserRequest(sapiMarketDataPermissionRequest.SapiUsername, sapiMarketDataPermissionRequest.IpAddress, sapiMarketDataPermissionRequest.CompanyUserID);
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SAPI username missing. UserID: {0}", sapiMarketDataPermissionRequest.CompanyUserID), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Verbose);
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

        /// <summary>
        /// Add Permission Subscription from the Cache.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="source"></param>
        /// <param name="iMarketDataPermissionServiceCallback"></param>
        internal static void AddSubscriptionToGetPermissionFromCache(int companyUserID, string source, IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback)
        {
            try
            {
                lock (_dictUserWiseMarketDataPermissionDetail)
                {
                    if (_dictUserWiseMarketDataPermissionDetail.ContainsKey(companyUserID))
                    {
                        SapiMarketDataPermissionDetail marketDataPermissionDetail = _dictUserWiseMarketDataPermissionDetail[companyUserID];

                        if (marketDataPermissionDetail.Callback.ContainsKey(source))
                        {
                            marketDataPermissionDetail.Callback[source] = iMarketDataPermissionServiceCallback;
                        }
                        else
                        {
                            marketDataPermissionDetail.Callback.Add(source, iMarketDataPermissionServiceCallback);
                        }

                        System.Threading.Tasks.Task.Run(new Action(() =>
                        {
                            iMarketDataPermissionServiceCallback.PermissionCheckResponse(companyUserID, marketDataPermissionDetail.HasPermission);
                        }));
                    }
                    else
                    {
                        SapiMarketDataPermissionDetail obj = new SapiMarketDataPermissionDetail();
                        obj.CompanyUserID = companyUserID;
                        obj.Callback.Add(source, iMarketDataPermissionServiceCallback);

                        _dictUserWiseMarketDataPermissionDetail.Add(companyUserID, obj);

                        System.Threading.Tasks.Task.Run(new Action(() =>
                        {
                            iMarketDataPermissionServiceCallback.PermissionCheckResponse(companyUserID, false);
                        }));
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

        /// <summary>
        /// Remove Permission Subscription from the Cache.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="source"></param>
        internal static void RemoveSubscriptionToGetPermissionFromCache(int companyUserID, string source, bool isResponseRequired)
        {
            try
            {
                if (!source.Equals(SAPIConstants.SAMSARA, StringComparison.InvariantCultureIgnoreCase))
                {
                    lock (_dictUserWiseMarketDataPermissionDetail)
                    {
                        if (_dictUserWiseMarketDataPermissionDetail.ContainsKey(companyUserID) && _dictUserWiseMarketDataPermissionDetail[companyUserID].Callback.ContainsKey(source))
                        {
                            IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback = _dictUserWiseMarketDataPermissionDetail[companyUserID].Callback[source];
                            _dictUserWiseMarketDataPermissionDetail[companyUserID].Callback.Remove(source);

                            if (isResponseRequired)
                            {
                                System.Threading.Tasks.Task.Run(new Action(() =>
                                {
                                    try
                                    {
                                        iMarketDataPermissionServiceCallback.PermissionCheckResponse(companyUserID, false);
                                    }
                                    catch
                                    {
                                    }
                                }));
                            }
                        }
                    }
                }

                if (source.Equals(SAPIConstants.SAMSARA, StringComparison.InvariantCultureIgnoreCase) || source.Equals(SAPIConstants.CLIENT, StringComparison.InvariantCultureIgnoreCase))
                {
                    _sapiManager.RemoveUserFromIdentityMapping(companyUserID);
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
        /// User Authentication Respnse handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthenticateUserRespnse(object sender, EventArgs<string, bool> e)
        {
            try
            {
                string sapiUsername = e.Value;
                bool permissionResponse = e.Value2;
                lock (_dictUserWiseMarketDataPermissionDetail)
                {
                    if (_dictUserWiseMarketDataPermissionDetail != null)
                    {
                        SapiMarketDataPermissionDetail marketDataPermissionDetail = _dictUserWiseMarketDataPermissionDetail.Values.FirstOrDefault(t => !string.IsNullOrEmpty(t.SapiUsername) && t.SapiUsername.Equals(sapiUsername));

                        if (marketDataPermissionDetail != null)
                        {
                            bool hasPermission = permissionResponse;

                            marketDataPermissionDetail.HasPermission = hasPermission;

                            if (marketDataPermissionDetail.Callback != null)
                            {
                                List<string> faultedCallbacks = new List<string>();
                                foreach (var kvp in marketDataPermissionDetail.Callback)
                                {
                                    try
                                    {
                                        kvp.Value.PermissionCheckResponse(marketDataPermissionDetail.CompanyUserID, hasPermission);
                                    }
                                    catch
                                    {
                                        faultedCallbacks.Add(kvp.Key);
                                    }
                                }

                                foreach (string callback in faultedCallbacks)
                                {
                                    marketDataPermissionDetail.Callback.Remove(callback);
                                }
                            }
                        }

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SAPI user PermissionCheck response received. UserID: {0}, SAPI Username: {1}, Permission status: {2}", marketDataPermissionDetail.CompanyUserID, marketDataPermissionDetail.SapiUsername, marketDataPermissionDetail.HasPermission), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
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
