using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.MarketDataPermissionService
{
    internal class ActivMarketDataPermissionDetail : MarketDataPermissionDetail
    {
        private static Dictionary<int, ActivMarketDataPermissionDetail> _dictUserWiseMarketDataPermissionDetail = new Dictionary<int, ActivMarketDataPermissionDetail>();
        private static Prana.ActivAdapter.ActivManager _activManager = Prana.ActivAdapter.ActivManager.GetInstance();

        internal static void PermissionCheck(MarketDataPermissionRequest marketDataPermissionRequest, string source, IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback)
        {
            try
            {
                ActivMarketDataPermissionRequest activMarketDataPermissionRequest = ((ActivMarketDataPermissionRequest)marketDataPermissionRequest);

                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ACTIV user PermissionCheck request. UserID: {0}, ACTIV Username: {1}", activMarketDataPermissionRequest.CompanyUserID, activMarketDataPermissionRequest.ActivUsername), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);

                if (!string.IsNullOrWhiteSpace(activMarketDataPermissionRequest.ActivUsername))
                {
                    lock (_dictUserWiseMarketDataPermissionDetail)
                    {
                        if (!_dictUserWiseMarketDataPermissionDetail.ContainsKey(activMarketDataPermissionRequest.CompanyUserID))
                        {
                            ActivMarketDataPermissionDetail obj = new ActivMarketDataPermissionDetail();
                            obj.CompanyUserID = activMarketDataPermissionRequest.CompanyUserID;
                            obj.Callback.Add(source, iMarketDataPermissionServiceCallback);

                            _dictUserWiseMarketDataPermissionDetail.Add(activMarketDataPermissionRequest.CompanyUserID, obj);
                        }
                        else
                        {
                            if (_dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].Callback.ContainsKey(source))
                            {
                                _dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].Callback[source] = iMarketDataPermissionServiceCallback;
                            }
                            else
                            {
                                _dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].Callback.Add(source, iMarketDataPermissionServiceCallback);
                            }
                        }

                        bool hasPermission = _activManager.VerifyUserDetails(activMarketDataPermissionRequest.ActivUsername, activMarketDataPermissionRequest.ActivPassword);
                        _dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].HasPermission = hasPermission;

                        List<string> faultedCallbacks = new List<string>();
                        foreach (var kvp in _dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].Callback)
                        {
                            try
                            {
                                kvp.Value.PermissionCheckResponse(activMarketDataPermissionRequest.CompanyUserID, hasPermission);
                            }
                            catch
                            {
                                faultedCallbacks.Add(kvp.Key);
                            }
                        }

                        foreach (string callback in faultedCallbacks)
                        {
                            _dictUserWiseMarketDataPermissionDetail[activMarketDataPermissionRequest.CompanyUserID].Callback.Remove(callback);
                        }

                        //Request to ACTIV API
                        System.Threading.Tasks.Task.Run(new Action(() =>
                        {
                            iMarketDataPermissionServiceCallback.PermissionCheckResponse(activMarketDataPermissionRequest.CompanyUserID, hasPermission);
                        }));
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ACTIV username missing. UserID: {0}", activMarketDataPermissionRequest.CompanyUserID), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
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

        internal static void AddSubscriptionToGetPermissionFromCache(int companyUserID, string source, IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback)
        {
            try
            {
                lock (_dictUserWiseMarketDataPermissionDetail)
                {
                    if (_dictUserWiseMarketDataPermissionDetail.ContainsKey(companyUserID))
                    {
                        ActivMarketDataPermissionDetail marketDataPermissionDetail = _dictUserWiseMarketDataPermissionDetail[companyUserID];

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
                        ActivMarketDataPermissionDetail obj = new ActivMarketDataPermissionDetail();
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

        internal static void RemoveSubscriptionToGetPermissionFromCache(int companyUserID, string source, bool isResponseRequired)
        {
            try
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
