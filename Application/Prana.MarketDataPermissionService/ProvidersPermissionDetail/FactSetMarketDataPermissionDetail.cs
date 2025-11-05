using FactSet.Datafeed;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prana.MarketDataPermissionService
{
    internal class FactSetMarketDataPermissionDetail : MarketDataPermissionDetail
    {
        private string FactSetSerialNumber { get; set; }

        private bool IsFactSetSupportUser { get; set; }

        private RTConsumer.RTSubscription rtSubscription { get; set; }

        private static Dictionary<int, FactSetMarketDataPermissionDetail> _dictUserWiseMarketDataPermissionDetail = new Dictionary<int, FactSetMarketDataPermissionDetail>();
        private static Prana.FactSetAdapter.FactSetManager _factSetManager = Prana.FactSetAdapter.FactSetManager.GetInstance();
        private static string _factSetPermissionService = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FactSetSettings, "FactSetPermissionService");

        internal static void PermissionCheck(MarketDataPermissionRequest marketDataPermissionRequest, string source, IMarketDataPermissionServiceCallback iMarketDataPermissionServiceCallback)
        {
            try
            {
                FactSetMarketDataPermissionRequest factSetMarketDataPermissionRequest = ((FactSetMarketDataPermissionRequest)marketDataPermissionRequest);

                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet user PermissionCheck request. UserID: {0}, FactSet Serial Number: {1}, IsSupportUser: {2}, IP Address: {3}", factSetMarketDataPermissionRequest.CompanyUserID, factSetMarketDataPermissionRequest.FactSetSerialNumber, factSetMarketDataPermissionRequest.IsFactSetSupportUser, factSetMarketDataPermissionRequest.IpAddress), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);

                if (!string.IsNullOrWhiteSpace(factSetMarketDataPermissionRequest.FactSetSerialNumber))
                {
                    lock (_dictUserWiseMarketDataPermissionDetail)
                    {
                        if (!_dictUserWiseMarketDataPermissionDetail.ContainsKey(factSetMarketDataPermissionRequest.CompanyUserID))
                        {
                            FactSetMarketDataPermissionDetail obj = new FactSetMarketDataPermissionDetail();
                            obj.CompanyUserID = factSetMarketDataPermissionRequest.CompanyUserID;
                            obj.FactSetSerialNumber = factSetMarketDataPermissionRequest.FactSetSerialNumber;
                            obj.IsFactSetSupportUser = factSetMarketDataPermissionRequest.IsFactSetSupportUser;
                            obj.Callback.Add(source, iMarketDataPermissionServiceCallback);

                            _dictUserWiseMarketDataPermissionDetail.Add(factSetMarketDataPermissionRequest.CompanyUserID, obj);
                        }
                        else
                        {
                            _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].FactSetSerialNumber = factSetMarketDataPermissionRequest.FactSetSerialNumber;
                            _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].IsFactSetSupportUser = factSetMarketDataPermissionRequest.IsFactSetSupportUser;

                            if (_dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].Callback.ContainsKey(source))
                            {
                                _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].Callback[source] = iMarketDataPermissionServiceCallback;
                            }
                            else
                            {
                                _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].Callback.Add(source, iMarketDataPermissionServiceCallback);
                            }
                        }

                        RTRequest req = new RTRequest(_factSetPermissionService, factSetMarketDataPermissionRequest.FactSetSerialNumber);
                        req.Options = factSetMarketDataPermissionRequest.IpAddress;

                        if (factSetMarketDataPermissionRequest.IsFactSetSupportUser)
                        {
                            if (_factSetManager.RTConsumerSupport != null)
                            {
                                if (_dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription != null)
                                    _factSetManager.RTConsumerSupport.Cancel(_dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription);

                                _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription = _factSetManager.RTConsumerSupport.MakeRequest(req, PermissionResponseHandler);
                            }
                        }
                        else
                        {
                            if (_factSetManager.RTConsumer != null)
                            {
                                if (_dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription != null)
                                    _factSetManager.RTConsumer.Cancel(_dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription);

                                _dictUserWiseMarketDataPermissionDetail[factSetMarketDataPermissionRequest.CompanyUserID].rtSubscription = _factSetManager.RTConsumer.MakeRequest(req, PermissionResponseHandler);
                            }
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet username & serial number missing. UserID: {0}", factSetMarketDataPermissionRequest.CompanyUserID), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Verbose);
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
                        FactSetMarketDataPermissionDetail marketDataPermissionDetail = _dictUserWiseMarketDataPermissionDetail[companyUserID];

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
                            try
                            {
                                iMarketDataPermissionServiceCallback.PermissionCheckResponse(companyUserID, marketDataPermissionDetail.HasPermission);
                            }
                            catch
                            {
                            }
                        }));
                    }
                    else
                    {
                        FactSetMarketDataPermissionDetail obj = new FactSetMarketDataPermissionDetail();
                        obj.CompanyUserID = companyUserID;
                        obj.Callback.Add(source, iMarketDataPermissionServiceCallback);

                        _dictUserWiseMarketDataPermissionDetail.Add(companyUserID, obj);

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
                        
                        // Remove the entire entry if logout is performed from Client or Samsara
                        if (source == FactSetAdapter.FactSetConstants.CLIENT || source == FactSetAdapter.FactSetConstants.SAMSARA)
                        {
                            _dictUserWiseMarketDataPermissionDetail.Remove(companyUserID);
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet user PermissionCheck subscription removed. UserID: {0}, Source: {1}", companyUserID, source), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                        }
                            
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

        private static void PermissionResponseHandler(RTConsumer.RTSubscription rtSubscription, RTMessage rtMessage)
        {
            try
            {
                if (rtMessage.IsError)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet user PermissionCheck response received. FactSet Serial Number: {0}, Error: {1}, ErrorDescription: {2}", rtSubscription.Key, rtMessage.Error, rtMessage.ErrorDescription), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                }
                else
                {
                    lock (_dictUserWiseMarketDataPermissionDetail)
                    {
                        if (_dictUserWiseMarketDataPermissionDetail != null)
                        {
                            FactSetMarketDataPermissionDetail marketDataPermissionDetail = _dictUserWiseMarketDataPermissionDetail.Values.FirstOrDefault(t => !string.IsNullOrEmpty(t.FactSetSerialNumber) && t.FactSetSerialNumber.Equals(rtSubscription.Key));

                            if (marketDataPermissionDetail != null)
                            {
                                bool hasPermission = rtMessage[RTFieldId.USER_LOGIN_STATUS].Value == "1";

                                if (rtMessage.IsClosed || (rtMessage.IsComplete && rtSubscription.IsSnapshot) || !hasPermission)
                                {
                                    if (marketDataPermissionDetail.IsFactSetSupportUser)
                                    {
                                        if (_factSetManager.RTConsumerSupport != null)
                                            _factSetManager.RTConsumerSupport.Cancel(rtSubscription);
                                    }
                                    else
                                    {
                                        if (_factSetManager.RTConsumer != null)
                                            _factSetManager.RTConsumer.Cancel(rtSubscription);
                                    }
                                    marketDataPermissionDetail.rtSubscription = null;
                                }

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

                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("FactSet user PermissionCheck response received. UserID: {0}, FactSet Serial Number: {1}, Workstation Login status: {2}", marketDataPermissionDetail.CompanyUserID, rtSubscription.Key, rtMessage[RTFieldId.USER_LOGIN_STATUS].Value), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("FactSet user PermissionCheck response received but unable to get user details. FactSet Serial Number: {0}, Workstation Login status: {1}", rtSubscription.Key, rtMessage[RTFieldId.USER_LOGIN_STATUS].Value), LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
