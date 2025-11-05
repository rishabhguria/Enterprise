using ActivFinancial.ContentPlatform.ContentGatewayApi;
using ActivFinancial.ContentPlatform.ContentGatewayApi.Common;
using ActivFinancial.ContentPlatform.ContentGatewayApi.Consts;
using ActivFinancial.ContentPlatform.ContentGatewayApi.RequestParameters;
using ActivFinancial.Middleware;
using ActivFinancial.Middleware.ActivBase;
using ActivFinancial.Middleware.Application;
using ActivFinancial.Middleware.FieldTypes;
using ActivFinancial.Middleware.Misc;
using ActivFinancial.Middleware.Service;
using ActivFinancial.Middleware.System;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;

namespace Prana.ActivAdapter
{
    internal class NirvanaContentGatewayClient : ContentGatewayClient, IDisposable
    {
        private int _conflationParametersType = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ConflationParametersType"));
        private int _conflationParametersInterval = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ConflationParametersInterval"));
        private bool _conflationParametersShouldEnableDynamicConflation = bool.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ConflationParametersShouldEnableDynamicConflation"));
        private bool _logonFlagDisconnectExisting = bool.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "LogonFlagDisconnectExisting"));
        private int _maxRetryAttempts = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "MaxLogonRetryAttempts"));
        private bool _isSkipOddLotTrades = bool.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "IsSkipOddLotTrades"));

        private int _tableNo_US_EQUITY = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "TableNo_US_EQUITY"));
        private int _tableNo_US_EQUITYOPTION = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "TableNo_US_EQUITYOPTION"));
        private string[] _inclusion_EventTypes = (ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "InclusionEventTypes")).Split(',');
        private string[] _exclusion_EventTypes = (ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ExclusionEventTypes")).Split(',');
        private string[] _configMarketDataStartTime = (ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "MarketDataStartTime")).Split(':');
        private string[] _configMarketDataStopTime = (ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "MarketDataStopTime")).Split(':');

        private int _currentRetryAttempt = 0;

        internal event EventHandler<bool> IsConnected;
        internal event EventHandler<Data> ContinuousDataResponse;
        internal event EventHandler<Data> SnapShotDataResponse;
        internal event EventHandler RetryConnection;

        private FieldListValidator fieldListValidator;
        private string serviceId;
        private long nextRequestId;
        private object requestIDLocker = new object();
        private Timer currentRetryAttemptClearTimer;
        private Dictionary<string, long> dictSubscriptionCookie = new Dictionary<string, long>();
        private Dictionary<RequestId, bool> dictSubscriptionDetails = new Dictionary<RequestId, bool>();
        private Dictionary<string, string> dictTickersLastStatusCode = new Dictionary<string, string>();

        private Dictionary<string, string> dictEncryptedActivAndTickerMapping = new Dictionary<string, string>();
        private Dictionary<string, string> dictActivEncryptedAndActivMapping = new Dictionary<string, string>();
        private Dictionary<string, string> dictMergentActivAndActivMapping = new Dictionary<string, string>();
        private HashSet<StatusCode> listStatus = new HashSet<StatusCode>();

        #region Constructor
        internal NirvanaContentGatewayClient(ActivApplication application)
            : base(application)
        {
            try
            {
                this.serviceId = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ActivServiceId");
                this.fieldListValidator = new FieldListValidator(this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            this.currentRetryAttemptClearTimer = new Timer(10000);
            this.currentRetryAttemptClearTimer.Elapsed += currentRetryAttemptClearTimer_Elapsed;
        }
        #endregion

        #region Connection/Disconnection
        internal void Connect(string username, string password)
        {
            try
            {
                StatusCode statusCode = ConnectionCheck(username, password);
                if (statusCode == StatusCode.StatusCodeSuccess && IsConnected != null)
                {
                    if (!currentRetryAttemptClearTimer.Enabled)
                        currentRetryAttemptClearTimer.Start();

                    #region Cleanup
                    lock (dictSubscriptionCookie)
                    {
                        dictSubscriptionCookie = new Dictionary<string, long>();
                    }

                    lock (dictSubscriptionDetails)
                    {
                        dictSubscriptionDetails = new Dictionary<RequestId, bool>();
                    }

                    lock (dictTickersLastStatusCode)
                    {
                        dictTickersLastStatusCode = new Dictionary<string, string>();
                    }

                    lock (dictEncryptedActivAndTickerMapping)
                    {
                        dictEncryptedActivAndTickerMapping = new Dictionary<string, string>();
                    }

                    lock (dictActivEncryptedAndActivMapping)
                    {
                        dictActivEncryptedAndActivMapping = new Dictionary<string, string>();
                    }

                    lock (dictMergentActivAndActivMapping)
                    {
                        dictMergentActivAndActivMapping = new Dictionary<string, string>();
                    }
                    #endregion

                    IsConnected(this, true);

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Connected to ACTIV: {0}", username), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                }
                else
                {
                    IsConnected(this, false);

                    if (statusCode != StatusCode.StatusCodeNotConnected)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to connect from ACTIV: {0}, StatusCode: {1}", username, statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                    }

                    if (statusCode != StatusCode.StatusCodeInUse && RetryConnection != null && _currentRetryAttempt > 0 && ++_currentRetryAttempt <= _maxRetryAttempts)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Retrying connection to ACTIV gateway. Attempt {0}", _currentRetryAttempt), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                        RetryConnection(this, null);
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

        internal bool VerifyUserDetails(string username, string password)
        {
            try
            {
                StatusCode statusCode = ConnectionCheck(username, password);
                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("User's ACTIV credential verification success: {0}", username), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                    return true;
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("User's ACTIV credential verification failed: {0} StatusCode: {1}", username, statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                    return false;
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
            return false;
        }

        private StatusCode ConnectionCheck(string username, string password)
        {
            try
            {
                IList<ServiceInstance> serviceInstanceList = new List<ServiceInstance>();

                IDictionary<string, object> attributes = new Dictionary<string, object>();
                attributes.Add(FileConfiguration.FileLocation, Application.Settings.ServiceLocationIniFile);

                StatusCode statusCode = ServiceApi.FindServices(ServiceApi.ConfigurationTypeFile, serviceId, attributes, serviceInstanceList);
                if (statusCode != StatusCode.StatusCodeSuccess)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Failed to find service <{0}>, {1}.\n", serviceId, statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Warning);
                    return statusCode;
                }

                // here we are just going to pick the first service that is returned, and its first access point url
                ConnectParameters connectParameters = new ConnectParameters();
                ServiceInstance serviceInstance = serviceInstanceList[0];

                string url = serviceInstance.ServiceAccessPointList[0].Url;

                connectParameters.ServiceId = serviceInstance.ServiceId;
                connectParameters.Url = url;
                connectParameters.UserId = username;
                connectParameters.Password = password;

                if (_logonFlagDisconnectExisting)
                {
                    connectParameters.LogonFlags |= ConnectParameters.LogonFlagDisconnectExisting;
                }

                return base.Connect(connectParameters, DefaultTimeout);
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
            return StatusCode.StatusCodeNotConnected;
        }

        internal new void Disconnect()
        {
            try
            {
                StatusCode statusCode = base.Disconnect();
                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    IsConnected(this, false);

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Disconnected from ACTIV."), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
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

        //override public void OnConnect()
        //{
        //    try
        //    {
        //        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Connected to ACTIV."), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);

        //        if (IsConnected != null)
        //        {
        //            IsConnected(this, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //override public void OnConnectFailed(StatusCode statusCode)
        //{
        //    try
        //    {
        //        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Disconnected from ACTIV while making connection. StatusCode: {0}", statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);

        //        if (IsConnected != null)
        //        {
        //            IsConnected(this, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //override public void OnDisconnect()
        //{
        //    try
        //    {
        //        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Disconnected from ACTIV."), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);

        //        if (IsConnected != null)
        //        {
        //            IsConnected(this, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        override public void OnBreak()
        {
            try
            {
                currentRetryAttemptClearTimer.Stop();
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Connection to ACTIV gateway has broken."), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);

                if (IsConnected != null)
                {
                    IsConnected(this, false);
                }

                if (RetryConnection != null && ++_currentRetryAttempt <= _maxRetryAttempts)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Retrying connection to ACTIV gateway. Attempt {0}", _currentRetryAttempt), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                    RetryConnection(this, null);
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

        private void currentRetryAttemptClearTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentRetryAttemptClearTimer.Stop();
            _currentRetryAttempt = 0;
        }
        #endregion

        #region MarketData
        internal void GetData(MarketDataSymbolResponse marketDataSymbolResponse, bool isSnapshotData)
        {
            try
            {
                long currentRequestID;
                lock (requestIDLocker)
                {
                    currentRequestID = ++this.nextRequestId;
                }

                RequestId requestID = new RequestId(currentRequestID);
                GetEqual.RequestParameters requestParameters = new GetEqual.RequestParameters();

                if (marketDataSymbolResponse.AssetCategory == AssetCategory.EquityOption)
                {
                    AddEquityOptionFieldsInRequest(requestParameters, marketDataSymbolResponse, isSnapshotData);
                }
                else
                {
                    AddEquityFieldsInRequest(requestParameters, marketDataSymbolResponse, isSnapshotData);
                }

                if (isSnapshotData)
                {
                    requestParameters.SubscribeParameters.Type = SubscribeParameters.TypeEnum.TypeNone;
                }
                else
                {
                    if (_inclusion_EventTypes != null && _inclusion_EventTypes.Length > 0 && !(_inclusion_EventTypes.Length == 1 && string.IsNullOrWhiteSpace(_inclusion_EventTypes[0])))
                    {
                        requestParameters.SubscribeParameters.Type = SubscribeParameters.TypeEnum.TypeEventTypeFilterIncludeList;

                        foreach (string eventType in _inclusion_EventTypes)
                        {
                            requestParameters.SubscribeParameters.EventTypeList.Add(Convert.ToByte(eventType.Trim()));
                        }
                    }
                    else if (_exclusion_EventTypes != null && _exclusion_EventTypes.Length > 0 && !(_exclusion_EventTypes.Length == 1 && string.IsNullOrWhiteSpace(_exclusion_EventTypes[0])))
                    {
                        requestParameters.SubscribeParameters.Type = SubscribeParameters.TypeEnum.TypeEventTypeFilterExcludeList;

                        foreach (string eventType in _exclusion_EventTypes)
                        {
                            requestParameters.SubscribeParameters.EventTypeList.Add(Convert.ToByte(eventType.Trim()));
                        }
                    }
                    else
                    {
                        requestParameters.SubscribeParameters.Type = SubscribeParameters.TypeEnum.TypeFull;
                    }
                }

                if (SubscribeParameters.TypeEnum.TypeFull == requestParameters.SubscribeParameters.Type)
                {
                    switch (_conflationParametersType)
                    {
                        case 0:
                            requestParameters.ConflationParameters.Type = ConflationType.ConflationTypeNone;
                            break;
                        case 1:
                            requestParameters.ConflationParameters.Type = ConflationType.ConflationTypeQuote;
                            requestParameters.ConflationParameters.Interval = _conflationParametersInterval;
                            break;
                        case 2:
                            requestParameters.ConflationParameters.Type = ConflationType.ConflationTypeTrade;
                            requestParameters.ConflationParameters.Interval = _conflationParametersInterval;
                            break;
                    }
                    requestParameters.ConflationParameters.ShouldEnableDynamicConflation = _conflationParametersShouldEnableDynamicConflation;
                }

                if (SubscribeParameters.TypeEnum.TypeNone != requestParameters.SubscribeParameters.Type)
                {
                    requestParameters.Flags |= RealtimeRequestParameters.FlagUseRequestIdInUpdates;
                }

                requestParameters.PermissionLevel = PermissionLevels.PermissionLevelDefault;
                requestParameters.UserContext = "";

                if (UserSettingConstants.IsDebugModeEnabled)
                {
                    string reqString = Prana.Global.Utilities.JsonHelper.SerializeObject(requestParameters);
                    LogAndDisplayOnInformationReporter.GetInstance.Write("Nirvana Internal Logging - Posting Request: " + Environment.NewLine + "Request id: " + requestID + ", Symbol: " + marketDataSymbolResponse.ActivSymbol + Environment.NewLine + reqString, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
                }

                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request for ActivSymbol: {0}", marketDataSymbolResponse.ActivSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                StatusCode statusCode = Equal.PostRequest(this, requestID, requestParameters, ContentGatewayClient.WaitInfiniteTimeout);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    lock (dictSubscriptionDetails)
                    {
                        if (dictSubscriptionDetails.ContainsKey(requestID))
                        {
                            dictSubscriptionDetails[requestID] = isSnapshotData;
                        }
                        else
                        {
                            dictSubscriptionDetails.Add(requestID, isSnapshotData);
                        }
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

        private void GetSharesOutstanding(MarketDataSymbolResponse marketDataSymbolResponse)
        {
            try
            {
                long currentRequestID;
                lock (requestIDLocker)
                {
                    currentRequestID = ++this.nextRequestId;
                }

                string activSymbol = marketDataSymbolResponse.TickerSymbol + ".MER-T/" + GetLastQuarterEndDate();
                lock (dictMergentActivAndActivMapping)
                {
                    if (!dictMergentActivAndActivMapping.ContainsKey(activSymbol))
                    {
                        dictMergentActivAndActivMapping.Add(activSymbol, marketDataSymbolResponse.ActivSymbol);
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Data added in dictMergentActivAndActivMapping. Activ Symbol: " + marketDataSymbolResponse.ActivSymbol + ", MergentActiv Symbol: " + activSymbol, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                    }
                }

                RequestId requestID = new RequestId(currentRequestID);
                GetEqual.RequestParameters requestParameters = new GetEqual.RequestParameters();

                requestParameters.SymbolIdList.Add(new SymbolId(TableNumbers.TABLE_NO_NA_MERGENT_COMPANY_HISTORY, activSymbol));

                RequestBlock requestBlock1 = new RequestBlock();
                requestBlock1.Flags |= RequestBlock.FlagNone;
                requestBlock1.RelationshipId = RelationshipIds.RelationshipIdNone;
                requestBlock1.FieldIdList.Add(FieldIds.FID_SHARES_OUTSTANDING);
                requestParameters.RequestBlockList.Add(requestBlock1);

                requestParameters.SubscribeParameters.Type = SubscribeParameters.TypeEnum.TypeNone;
                requestParameters.PermissionLevel = PermissionLevels.PermissionLevelDefault;
                requestParameters.UserContext = "";

                Equal.PostRequest(this, requestID, requestParameters, ContentGatewayClient.WaitInfiniteTimeout);
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

        internal List<OptionStaticData> GetOptionChain(string underlyingMarketSymbol, OptionChainFilter optionChainFilter)
        {
            List<OptionStaticData> optionStaticDataChain = new List<OptionStaticData>();
            try
            {
                FieldListValidator fieldListValidator = new FieldListValidator(this);
                string rootSymbol = string.Empty;

                if (optionChainFilter.LowerStrike == 0 && optionChainFilter.UpperStrike == 0)
                {
                    optionChainFilter.LowerStrike = (optionChainFilter.UnderlyingSymbolLastTradedPrice - (optionChainFilter.UnderlyingSymbolLastTradedPrice * optionChainFilter.StrikeTolerancePercentage / 100));
                    optionChainFilter.UpperStrike = (optionChainFilter.UnderlyingSymbolLastTradedPrice + (optionChainFilter.UnderlyingSymbolLastTradedPrice * optionChainFilter.StrikeTolerancePercentage / 100));
                }

                RequestBlock optionRootRequestBlock = new RequestBlock();
                optionRootRequestBlock.RelationshipId = RelationshipIds.RELATIONSHIP_ID_OPTION_ROOT;
                optionRootRequestBlock.FieldIdList.Add(FieldIds.FID_STRIKE_PRICE_LIST);
                optionRootRequestBlock.FieldIdList.Add(FieldIds.FID_EXPIRATION_DATE_LIST);

                GetMatch.RequestParameters optionRootsRequestParameters = new GetMatch.RequestParameters();
                optionRootsRequestParameters.RequestBlockList.Add(optionRootRequestBlock);
                optionRootsRequestParameters.SymbolIdList.Add(new SymbolId(underlyingMarketSymbol));
                GetMatch.ResponseParameters optionRootsResponseParameters = new GetMatch.ResponseParameters();

                StatusCode statusCode = Match.SendRequest(this, optionRootsRequestParameters, optionRootsResponseParameters);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    List<Date> expirationDates = new List<Date>();
                    List<Rational> strikePrices = new List<Rational>();

                    RequestBlock requestBlockOptions = new RequestBlock();
                    requestBlockOptions.FieldIdList.Add(FieldIds.FID_SYMBOL);
                    requestBlockOptions.FieldIdList.Add(FieldIds.FID_EXPIRATION_DATE);
                    requestBlockOptions.FieldIdList.Add(FieldIds.FID_STRIKE_PRICE);
                    requestBlockOptions.FieldIdList.Add(FieldIds.FID_OPTION_TYPE);

                    GetPattern.RequestParameters optionsRequestParameters = new GetPattern.RequestParameters();
                    optionsRequestParameters.RequestBlockList.Add(requestBlockOptions);
                    optionsRequestParameters.SymbolPatternList.Clear();
                    GetPattern.ResponseParameters optionsResponseParameters = new GetPattern.ResponseParameters();
                    optionsResponseParameters.ResponseBlockList.Clear();

                    foreach (ResponseBlock responseBlock in optionRootsResponseParameters.ResponseBlockList)
                    {
                        if (responseBlock.IsValidResponse() && responseBlock.RelationshipId == RelationshipIds.RELATIONSHIP_ID_OPTION_ROOT)
                        {
                            try
                            {
                                try
                                {
                                    fieldListValidator.Initialize(responseBlock.FieldData);
                                }
                                catch (MiddlewareException e)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Failed to initialize fieldListValidator, {0}\n", e.StatusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
                                    return optionStaticDataChain;
                                }

                                rootSymbol = responseBlock.ResponseKey.Symbol;

                                List<Date> rootExpirationDates = new List<Date>();
                                List<Rational> rootStrikePrices = new List<Rational>();

                                FieldListValidator.Field expirationDatesField = fieldListValidator.GetField(FieldIds.FID_EXPIRATION_DATE_LIST);
                                if (expirationDatesField != null && expirationDatesField.FieldStatus == FieldStatuses.FieldStatusDefined)
                                {
                                    BinaryString expirationDatesBinaryString = (BinaryString)expirationDatesField.FieldType;
                                    UsEquityOptionHelper.GetExpirationDateList(expirationDatesBinaryString, rootExpirationDates);

                                    Date startDate;
                                    if (optionChainFilter.ExpirationDate.Month == System.DateTime.Now.Month)
                                        startDate = new Date(optionChainFilter.ExpirationDate.Year, optionChainFilter.ExpirationDate.Month, System.DateTime.Now.Day);
                                    else
                                        startDate = new Date(optionChainFilter.ExpirationDate.Year, optionChainFilter.ExpirationDate.Month, 1);
                                    Date endDate = new Date(optionChainFilter.ExpirationDate.Year, optionChainFilter.ExpirationDate.Month, new System.DateTime(optionChainFilter.ExpirationDate.Year, optionChainFilter.ExpirationDate.Month, 1).AddMonths(1).AddDays(-1).Day);

                                    if ((startDate != null && startDate.Initialized) && (endDate != null && endDate.Initialized))
                                    {
                                        foreach (Date expirationDate in rootExpirationDates)
                                        {
                                            if (expirationDate.CompareTo(endDate) > 0)
                                                continue;

                                            if (expirationDate.CompareTo(startDate) < 0)
                                                continue;

                                            if (!expirationDates.Contains(expirationDate))
                                                expirationDates.Add(expirationDate);
                                        }
                                    }
                                }

                                FieldListValidator.Field strikePricesField = fieldListValidator.GetField(FieldIds.FID_STRIKE_PRICE_LIST);
                                if (strikePricesField != null && strikePricesField.FieldStatus == FieldStatuses.FieldStatusDefined)
                                {
                                    Blob srikePricesBlob = (Blob)strikePricesField.FieldType;
                                    UsEquityOptionHelper.GetStrikePriceList(srikePricesBlob, rootStrikePrices);

                                    Rational lower = new Rational((long)optionChainFilter.LowerStrike);
                                    Rational upper = new Rational((long)optionChainFilter.UpperStrike);

                                    if ((upper != null && upper.Initialized) && (lower != null && lower.Initialized))
                                    {
                                        foreach (Rational strikePrice in rootStrikePrices)
                                        {
                                            if (strikePrice.CompareTo(upper) > 0)
                                                continue;

                                            if (strikePrice.CompareTo(lower) < 0)
                                                continue;

                                            if (!strikePrices.Contains(strikePrice))
                                                strikePrices.Add(strikePrice);
                                        }
                                    }
                                }
                            }
                            catch (MiddlewareException e)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write("Middleware Exception: " + e.Message, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                            }
                        }
                    }

                    if (expirationDates.Count != 0 && strikePrices.Count != 0)
                    {
                        StringBuilder sbPattern = new StringBuilder();

                        foreach (Date expirationDate in expirationDates)
                        {
                            foreach (Rational strikePrice in strikePrices)
                            {
                                try
                                {

                                    if (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT || optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL)
                                    {
                                        sbPattern.Length = 0;
                                        UsEquityOptionHelper.BuildAliasSymbol(sbPattern, GetOptionRoot(rootSymbol), expirationDate, UsEquityOptionHelper.OptionTypeEnum.OptionTypeCall, strikePrice, "*");
                                        optionsRequestParameters.SymbolPatternList.Add(new SymbolId(TableNumbers.TABLE_NO_NA_EQUITY_OPTION_ALIAS, sbPattern.ToString()));
                                    }

                                    if (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT || optionChainFilter.OptionTypeFilter == OptionTypeFilter.PUT)
                                    {
                                        sbPattern.Length = 0;
                                        UsEquityOptionHelper.BuildAliasSymbol(sbPattern, GetOptionRoot(rootSymbol), expirationDate, UsEquityOptionHelper.OptionTypeEnum.OptionTypePut, strikePrice, "*");
                                        optionsRequestParameters.SymbolPatternList.Add(new SymbolId(TableNumbers.TABLE_NO_NA_EQUITY_OPTION_ALIAS, sbPattern.ToString()));
                                    }
                                }
                                catch (MiddlewareException e)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.Write("Middleware Exception: " + e.Message, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                                }
                            }
                        }
                    }
                    else
                        return optionStaticDataChain;

                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Request for ActivSymbol: {0}", rootSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                    statusCode = Pattern.SendRequest(this, optionsRequestParameters, optionsResponseParameters);

                    if (statusCode == StatusCode.StatusCodeSuccess)
                    {
                        Dictionary<System.DateTime, List<OptionStaticData>> expirationDateWiseOptionStaticData = new Dictionary<System.DateTime, List<OptionStaticData>>();
                        StringBuilder stringBuilder = new StringBuilder();

                        foreach (ResponseBlock responseBlock in optionsResponseParameters.ResponseBlockList)
                        {
                            if (responseBlock.IsValidResponse())
                            {
                                try
                                {
                                    SymbolData optionData = new SymbolData();
                                    bool isOddLotTrade = false;

                                    while (optionData.ExpirationDate == DateTimeConstants.MinValue || optionData.StrikePrice == 0)
                                        ParseResponse(ref optionData, responseBlock.FieldData, stringBuilder, ref isOddLotTrade);

                                    OptionStaticData optionStaticData = new OptionStaticData()
                                    {
                                        ExpirationDate = optionData.ExpirationDate,
                                        PutOrCall = optionData.PutOrCall,
                                        StrikePrice = optionData.StrikePrice,
                                        ActivSymbol = responseBlock.ResolvedKey.Symbol
                                    };

                                    MarketDataSymbolResponse underlyingSymbol = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(underlyingMarketSymbol);
                                    if (underlyingSymbol != null)
                                        optionStaticData.UnderlyingSymbol = underlyingSymbol.TickerSymbol;
                                    else
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Underlying symbol not found for ActivSymbol: {0}", responseBlock.ResolvedKey.Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                                        continue;
                                    }
                                    string bbgSymbol;
                                    OptionDetail optionDetail = MarketDataAdapterExtension.GenerateOptionDataFromMarketDataSymbol(underlyingSymbol, optionStaticData, out bbgSymbol);
                                    MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse()
                                    {
                                        TickerSymbol = optionDetail.Symbol,
                                        AUECID = optionDetail.AUECID,
                                        ActivSymbol = responseBlock.ResolvedKey.Symbol
                                    };
                                    if (optionDetail.AssetCategory == AssetCategory.Equity || optionDetail.AssetCategory == AssetCategory.Indices)
                                        marketDataSymbolResponse.AssetCategory = AssetCategory.EquityOption;
                                    else if (optionDetail.AssetCategory == AssetCategory.Future)
                                        marketDataSymbolResponse.AssetCategory = AssetCategory.FutureOption;

                                    MarketDataAdapterExtension.AddMarketDataForTickerSymbolToCache(optionDetail.Symbol, marketDataSymbolResponse);
                                    if (!string.IsNullOrEmpty(optionDetail.Symbol))
                                        optionStaticData.Symbol = optionDetail.Symbol;
                                    else
                                    {
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to generate ticker symbol for {0} with AUECID: {1} and Underlying Symbol: {2}", marketDataSymbolResponse.ActivSymbol, marketDataSymbolResponse.AUECID, underlyingSymbol.TickerSymbol), LoggingConstants.CATEGORY_ERROR, 1, 1, TraceEventType.Error);
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Unable to generate ticker symbol for {0} with AUECID: {1} and Underlying Symbol: {2}", marketDataSymbolResponse.ActivSymbol, marketDataSymbolResponse.AUECID, underlyingSymbol.TickerSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Error);
                                    }
                                    if (!string.IsNullOrEmpty(bbgSymbol))
                                        optionStaticData.BloombergSymbol = bbgSymbol;
                                    if (!expirationDateWiseOptionStaticData.ContainsKey(optionStaticData.ExpirationDate))
                                        expirationDateWiseOptionStaticData.Add(optionStaticData.ExpirationDate, new List<OptionStaticData>() { optionStaticData });
                                    else
                                        expirationDateWiseOptionStaticData[optionStaticData.ExpirationDate].Add(optionStaticData);
                                }
                                catch (MiddlewareException e)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.Write("Middleware Exception: " + e.Message, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                                }
                            }
                        }
                        LogAndDisplayOnInformationReporter.GetInstance.Write(stringBuilder.ToString(), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Applying Filters on {0} options of {1}", optionsResponseParameters.ResponseBlockList.Count, rootSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);

                        System.Threading.Tasks.Parallel.ForEach(expirationDateWiseOptionStaticData, kvp => kvp.Value.Sort((p, n) => p.StrikePrice.CompareTo(n.StrikePrice)));

                        foreach (System.DateTime expirationDate in expirationDateWiseOptionStaticData.Keys)
                        {
                            if (expirationDateWiseOptionStaticData[expirationDate].Count > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                            {
                                int maxLowerNumberOfOptions, maxUpperNumberOfOptions;

                                if (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT)
                                {
                                    if (optionChainFilter.MaxNumberOfStrikes % 2 == 0)
                                        maxLowerNumberOfOptions = maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes;
                                    else
                                    {
                                        maxLowerNumberOfOptions = optionChainFilter.MaxNumberOfStrikes + 1;
                                        maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes - 1;
                                    }
                                }
                                else
                                {
                                    if (optionChainFilter.MaxNumberOfStrikes % 2 == 0)
                                        maxLowerNumberOfOptions = maxUpperNumberOfOptions = optionChainFilter.MaxNumberOfStrikes / 2;
                                    else
                                    {
                                        maxLowerNumberOfOptions = (optionChainFilter.MaxNumberOfStrikes + 1) / 2;
                                        maxUpperNumberOfOptions = (optionChainFilter.MaxNumberOfStrikes - 1) / 2;
                                    }
                                }

                                int pivot = expirationDateWiseOptionStaticData[expirationDate].FindIndex(o => o.StrikePrice > optionChainFilter.UnderlyingSymbolLastTradedPrice);
                                int lowerStrikeOptionsCount = expirationDateWiseOptionStaticData[expirationDate].Where(o => o.StrikePrice < optionChainFilter.UnderlyingSymbolLastTradedPrice).Count();
                                int upperStrikeOptionsCount = expirationDateWiseOptionStaticData[expirationDate].Where(o => o.StrikePrice > optionChainFilter.UnderlyingSymbolLastTradedPrice).Count();

                                if (pivot == -1)
                                {
                                    if (lowerStrikeOptionsCount == 0)
                                    {
                                        pivot = 0;
                                        if (upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                            upperStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes);
                                    }
                                    else if (upperStrikeOptionsCount == 0)
                                    {
                                        pivot = expirationDateWiseOptionStaticData[expirationDate].Count;
                                        if (lowerStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                            lowerStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes);
                                    }
                                }
                                else if (lowerStrikeOptionsCount > maxLowerNumberOfOptions && upperStrikeOptionsCount < maxUpperNumberOfOptions)
                                {
                                    lowerStrikeOptionsCount += maxUpperNumberOfOptions - upperStrikeOptionsCount;

                                    if (lowerStrikeOptionsCount + upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                        lowerStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes) - upperStrikeOptionsCount;
                                }
                                else if (lowerStrikeOptionsCount < maxLowerNumberOfOptions && upperStrikeOptionsCount > maxUpperNumberOfOptions)
                                {
                                    upperStrikeOptionsCount += maxLowerNumberOfOptions - lowerStrikeOptionsCount;

                                    if (lowerStrikeOptionsCount + upperStrikeOptionsCount > (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes))
                                        upperStrikeOptionsCount = (optionChainFilter.OptionTypeFilter == OptionTypeFilter.CALL_PUT ? optionChainFilter.MaxNumberOfStrikes * 2 : optionChainFilter.MaxNumberOfStrikes) - lowerStrikeOptionsCount;
                                }
                                else if (lowerStrikeOptionsCount >= maxLowerNumberOfOptions && upperStrikeOptionsCount >= maxUpperNumberOfOptions)
                                {
                                    lowerStrikeOptionsCount = maxLowerNumberOfOptions;
                                    upperStrikeOptionsCount = maxUpperNumberOfOptions;
                                }

                                for (int lowerIndex = pivot - 1; lowerIndex >= 0 && lowerStrikeOptionsCount > 0; lowerIndex--, lowerStrikeOptionsCount--)
                                    optionStaticDataChain.Add(expirationDateWiseOptionStaticData[expirationDate][lowerIndex]);

                                for (int upperIndex = pivot; upperIndex < expirationDateWiseOptionStaticData[expirationDate].Count && upperStrikeOptionsCount > 0; upperIndex++, upperStrikeOptionsCount--)
                                    optionStaticDataChain.Add(expirationDateWiseOptionStaticData[expirationDate][upperIndex]);
                            }
                            else
                            {
                                optionStaticDataChain.AddRange(expirationDateWiseOptionStaticData[expirationDate]);
                            }
                        }

                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Filtered {0} options of {1}", optionStaticDataChain.Count, rootSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write("Got " + Enum.GetName(typeof(StatusCode), statusCode) + " while sending request for the options of symbol: " + underlyingMarketSymbol, LoggingConstants.CATEGORY_ERROR, 1, 1, TraceEventType.Verbose);
                    LogAndDisplayOnInformationReporter.GetInstance.Write("Got " + Enum.GetName(typeof(StatusCode), statusCode) + " while sending request for the options of symbol: " + underlyingMarketSymbol, LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
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
            return optionStaticDataChain;
        }

        private string GetOptionRoot(string underlyingMarketSymbol)
        {
            try
            {
                int i = underlyingMarketSymbol.LastIndexOf('.');
                if (i == -1)
                    return string.Empty;

                return underlyingMarketSymbol.Substring(0, i);
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
            return string.Empty;
        }

        private void AddEquityFieldsInRequest(GetEqual.RequestParameters requestParameters, MarketDataSymbolResponse marketDataSymbolResponse, bool isSnapshotData)
        {
            try
            {
                requestParameters.SymbolIdList.Add(new SymbolId(_tableNo_US_EQUITY, marketDataSymbolResponse.ActivSymbol));

                RequestBlock requestBlock1 = new RequestBlock();
                requestBlock1.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                requestBlock1.RelationshipId = RelationshipIds.RelationshipIdNone;
                requestBlock1.FieldIdList.Add(FieldIds.FID_LOT_SIZE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_SEDOL_CODE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_CUMULATIVE_VOLUME);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID_SIZE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID_EXCHANGE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK_SIZE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK_EXCHANGE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_HIGH);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_LOW);
                requestBlock1.FieldIdList.Add(FieldIds.FID_OPEN);
                requestBlock1.FieldIdList.Add(FieldIds.FID_PREVIOUS_CLOSE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_DATE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_TIME);
                requestBlock1.FieldIdList.Add(FieldIds.FID_CURRENCY);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_CONDITION);
                requestParameters.RequestBlockList.Add(requestBlock1);

                if (isSnapshotData)
                {
                    RequestBlock requestBlock2 = new RequestBlock();
                    requestBlock2.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                    requestBlock2.RelationshipId = RelationshipIds.RELATIONSHIP_ID_SECURITY;
                    requestBlock2.FieldIdList.Add(FieldIds.FID_PRIMARY_EXCHANGE);
                    requestBlock2.FieldIdList.Add(FieldIds.FID_DIVIDEND);
                    requestBlock2.FieldIdList.Add(FieldIds.FID_EX_DATE);
                    requestBlock2.FieldIdList.Add(FieldIds.FID_INDICATED_ANNUAL_DIVIDEND);
                    requestParameters.RequestBlockList.Add(requestBlock2);

                    RequestBlock requestBlock3 = new RequestBlock();
                    requestBlock3.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                    requestBlock3.RelationshipId = RelationshipIds.RELATIONSHIP_ID_COMPANY;
                    requestBlock3.FieldIdList.Add(FieldIds.FID_NAME);
                    requestParameters.RequestBlockList.Add(requestBlock3);

                    RequestBlock requestBlock4 = new RequestBlock();
                    requestBlock4.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                    requestBlock4.RelationshipId = RelationshipIds.RELATIONSHIP_ID_ANALYTICS;
                    requestBlock4.FieldIdList.Add(FieldIds.FID_BETA);
                    requestParameters.RequestBlockList.Add(requestBlock4);

                    RequestBlock requestBlock5 = new RequestBlock();
                    requestBlock5.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                    requestBlock5.RelationshipId = RelationshipIds.RELATIONSHIP_ID_TRADING_ANALYTICS;
                    requestBlock5.FieldIdList.Add(FieldIds.FID_VWAP);
                    requestParameters.RequestBlockList.Add(requestBlock5);
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

        private void AddEquityOptionFieldsInRequest(GetEqual.RequestParameters requestParameters, MarketDataSymbolResponse marketDataSymbolResponse, bool isSnapshotData)
        {
            try
            {
                requestParameters.SymbolIdList.Add(new SymbolId(_tableNo_US_EQUITYOPTION, marketDataSymbolResponse.ActivSymbol));

                RequestBlock requestBlock1 = new RequestBlock();
                requestBlock1.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                requestBlock1.RelationshipId = RelationshipIds.RelationshipIdNone;
                requestBlock1.FieldIdList.Add(FieldIds.FID_SEDOL_CODE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_OPTION_TYPE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_OPEN_INTEREST);
                requestBlock1.FieldIdList.Add(FieldIds.FID_EXPIRATION_DATE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_STRIKE_PRICE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_CUMULATIVE_VOLUME);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID_SIZE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_BID_EXCHANGE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK_SIZE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_ASK_EXCHANGE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_HIGH);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_LOW);
                requestBlock1.FieldIdList.Add(FieldIds.FID_OPEN);
                requestBlock1.FieldIdList.Add(FieldIds.FID_PREVIOUS_CLOSE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_DATE);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_TIME);
                requestBlock1.FieldIdList.Add(FieldIds.FID_TRADE_CONDITION);
                requestParameters.RequestBlockList.Add(requestBlock1);

                if (isSnapshotData)
                {
                    RequestBlock requestBlock2 = new RequestBlock();
                    requestBlock2.Flags |= RequestBlock.FlagIgnoreInvalidFields;
                    requestBlock2.RelationshipId = RelationshipIds.RELATIONSHIP_ID_OPTION_ROOT;
                    requestBlock2.FieldIdList.Add(FieldIds.FID_UNDERLYING_SYMBOL);
                    requestBlock2.FieldIdList.Add(FieldIds.FID_CONTRACT_SIZE);
                    requestBlock2.FieldIdList.Add(FieldIds.FID_CURRENCY);
                    requestParameters.RequestBlockList.Add(requestBlock2);
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

        internal void DeleteSymbol(string symbol)
        {
            try
            {
                long subscriptionCookie = 0;
                lock (dictSubscriptionCookie)
                {
                    if (dictSubscriptionCookie.ContainsKey(symbol))
                    {
                        subscriptionCookie = dictSubscriptionCookie[symbol];
                    }
                }

                StatusCode statusCode = base.Unsubscribe(subscriptionCookie);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    lock (dictSubscriptionCookie)
                    {
                        dictSubscriptionCookie.Remove(symbol);
                    }

                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Symbol: {0} subscription stopped", symbol), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Unable to stop subscription. Symbol: {0}, StatusCode: {1}", symbol, statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
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

        override public void OnGetEqualResponse(HeapMessage response)
        {
            try
            {
                StringBuilder loggingOutput = new StringBuilder();

                if (UserSettingConstants.IsDebugModeEnabled)
                {
                    loggingOutput.Append(string.Format("OnGetEqualResponse(): Request id [{0}] - status {1}\n", response.RequestId.ToString(), response.StatusCode.ToString()));
                }

                if (!IsValidResponse(response))
                {
                    if (listStatus.Add(response.StatusCode))
                    {
                        if (response.StatusCode == StatusCode.StatusCodeQuotaExceeded)
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Symbol Subscription Quota has been exceeded.", LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                        }
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Invalid response received with status code: " + response.StatusCode + ". Message: " + response.Message, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Invalid response received with status code: " + response.StatusCode + ". Message: " + response.Message, LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                    }

                    return;
                }

                RealtimeResponseParameters responseParameters = new RealtimeResponseParameters();
                StatusCode statusCode = Equal.Deserialize(this, response, responseParameters);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    bool isSnapshotData = false;
                    lock (dictSubscriptionDetails)
                    {
                        if (dictSubscriptionDetails.ContainsKey(response.RequestId))
                        {
                            isSnapshotData = dictSubscriptionDetails[response.RequestId];
                        }
                    }

                    if (responseParameters.ResponseBlockList.Count > 0)
                    {
                        bool isOddLotTrade = false;
                        bool isValidResponse = false;

                        if (MiscConsts.SubscriptionCookieUndefined != responseParameters.SubscriptionCookie && !isSnapshotData)
                        {
                            lock (dictSubscriptionCookie)
                            {
                                if (dictSubscriptionCookie.ContainsKey(responseParameters.ResponseBlockList[0].RequestedKey.Symbol))
                                {
                                    dictSubscriptionCookie[responseParameters.ResponseBlockList[0].RequestedKey.Symbol] = responseParameters.SubscriptionCookie;
                                }
                                else
                                {
                                    dictSubscriptionCookie.Add(responseParameters.ResponseBlockList[0].RequestedKey.Symbol, responseParameters.SubscriptionCookie);
                                }
                            }
                        }

                        if (UserSettingConstants.IsDebugModeEnabled)
                        {
                            loggingOutput.Append(string.Format("Subscription cookie ............... {0}\n", SubscriptionCookieToString(responseParameters.SubscriptionCookie)));
                        }

                        SymbolData snapShotData = MarketDataAdapterExtension.GetSnapShotSymbolData(responseParameters.ResponseBlockList[0].ResponseKey.Symbol);
                        if (snapShotData == null)
                        {
                            if (responseParameters.ResponseBlockList[0].ResolvedKey.Symbol.EndsWith(".O"))
                            {
                                snapShotData = new OptionSymbolData();
                                snapShotData.CategoryCode = AssetCategory.EquityOption;
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Option Response received for ActivSymbol: {0}", responseParameters.ResponseBlockList[0].ResolvedKey.Symbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                            }
                            else
                            {
                                snapShotData = new EquitySymbolData();
                                snapShotData.CategoryCode = AssetCategory.Equity;
                            }
                        }
                        snapShotData.MarketDataProvider = MarketDataProvider.ACTIV;
                        snapShotData.ActivSymbol = responseParameters.ResponseBlockList[0].ResolvedKey.Symbol;
                        snapShotData.PricingStatus = ((responseParameters.ResponseBlockList[0].permissionLevel == 0) ? PricingStatus.RealTime : PricingStatus.Delayed);
                        snapShotData.DelayInterval = responseParameters.ResponseBlockList[0].PermissionLevelData.DelayPeriod.ToString();

                        lock (dictTickersLastStatusCode)
                        {
                            if (dictTickersLastStatusCode.ContainsKey(responseParameters.ResponseBlockList[0].RequestedKey.Symbol))
                                dictTickersLastStatusCode[responseParameters.ResponseBlockList[0].RequestedKey.Symbol] = string.Empty;
                        }

                        MarketDataSymbolResponse marketDataSymbolResponse = null;

                        for (int count = 0, size = responseParameters.ResponseBlockList.Count; count < size; ++count)
                        {
                            ResponseBlock responseBlock = responseParameters.ResponseBlockList[count];

                            if (UserSettingConstants.IsDebugModeEnabled)
                            {
                                loggingOutput.Append(string.Format("**** Response block {0}/{1} ****\n", (count + 1), size));
                                loggingOutput.Append(string.Format("Requested key ..................... {0}\n", responseBlock.RequestedKey.ToString()));
                                loggingOutput.Append(string.Format("Resolved key ...................... {0}\n", responseBlock.ResolvedKey.ToString()));
                                loggingOutput.Append(string.Format("Relationship id {0,3} ............... {1}\n", (int)responseBlock.RelationshipId, EnumDescription.GetDescription(responseBlock.StatusCode)));
                                loggingOutput.Append(string.Format("Flags ............................. {0}\n", responseBlock.FlagsToString(responseBlock.Flags)));

                                if (responseBlock.IsResponseKeyDefined())
                                {
                                    loggingOutput.Append(string.Format("Response key ...................... {0}\n", responseBlock.ResponseKey.ToString()));
                                }

                                DisplayPermissionInfo(responseBlock.PermissionId, responseBlock.permissionLevel, responseBlock.PermissionLevelData, loggingOutput);
                            }

                            if (!responseBlock.IsValidResponse())
                            {
                                lock (dictTickersLastStatusCode)
                                {
                                    if (dictTickersLastStatusCode.ContainsKey(responseParameters.ResponseBlockList[0].RequestedKey.Symbol))
                                    {
                                        if (!string.IsNullOrEmpty(dictTickersLastStatusCode[responseParameters.ResponseBlockList[0].RequestedKey.Symbol]))
                                            dictTickersLastStatusCode[responseParameters.ResponseBlockList[0].RequestedKey.Symbol] += string.Format(", {0} - {1}", count + 1, responseBlock.StatusCode.ToString());
                                        else
                                            dictTickersLastStatusCode[responseParameters.ResponseBlockList[0].RequestedKey.Symbol] += string.Format("{0} - {1}", count + 1, responseBlock.StatusCode.ToString());
                                    }
                                    else
                                        dictTickersLastStatusCode.Add(responseParameters.ResponseBlockList[0].RequestedKey.Symbol, string.Format("{0} - {1}", count + 1, responseBlock.StatusCode.ToString()));
                                }
                                continue;
                            }
                            else if (responseBlock.FieldData.IsEmpty())
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Empty response received. Symbol: {0}, Block Number: {1}, StatusCode: {2}", responseParameters.ResponseBlockList[0].RequestedKey.Symbol, count, responseBlock.StatusCode), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Information);
                                continue;
                            }
                            else
                            {
                                isValidResponse = true;

                                double lastPriceBeforeOddLot = snapShotData.LastPrice;
                                ParseResponse(ref snapShotData, responseBlock.FieldData, loggingOutput, ref isOddLotTrade);

                                if (_isSkipOddLotTrades && isOddLotTrade)
                                {
                                    snapShotData.LastPrice = lastPriceBeforeOddLot;
                                    loggingOutput.AppendLine(string.Format("Due to IsSkipOddLotTrades config, Odd lot trade price skipped. In this case, Last : {0}", snapShotData.LastPrice));
                                }
                            }
                        }

                        marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(responseParameters.ResponseBlockList[0].RequestedKey.Symbol);

                        if (marketDataSymbolResponse == null)
                            marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(snapShotData);

                        if (marketDataSymbolResponse != null && !string.IsNullOrEmpty(marketDataSymbolResponse.TickerSymbol))
                        {
                            if (isValidResponse)
                            {
                                snapShotData.Symbol = marketDataSymbolResponse.TickerSymbol;

                                if (string.IsNullOrWhiteSpace(snapShotData.ListedExchange))
                                    snapShotData.ListedExchange = MarketDataAdapterExtension.GetExchangeName(marketDataSymbolResponse.AUECID);

                                if (responseParameters.ResponseBlockList[0].IsResponseKeyDefined())
                                {
                                    lock (dictEncryptedActivAndTickerMapping)
                                    {
                                        if (!dictEncryptedActivAndTickerMapping.ContainsKey(responseParameters.ResponseBlockList[0].ResponseKey.Symbol))
                                        {
                                            dictEncryptedActivAndTickerMapping.Add(responseParameters.ResponseBlockList[0].ResponseKey.Symbol, marketDataSymbolResponse.TickerSymbol);
                                            LogAndDisplayOnInformationReporter.GetInstance.Write("Data added in dictEncryptedActivAndTickerMapping. Ticker Symbol: " + marketDataSymbolResponse.TickerSymbol + ", EncryptedActiv Symbol: " + responseParameters.ResponseBlockList[0].ResponseKey.Symbol, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                                        }
                                    }

                                    lock (dictActivEncryptedAndActivMapping)
                                    {
                                        if (!dictActivEncryptedAndActivMapping.ContainsKey(responseParameters.ResponseBlockList[0].ResponseKey.Symbol))
                                        {
                                            dictActivEncryptedAndActivMapping.Add(responseParameters.ResponseBlockList[0].ResponseKey.Symbol, marketDataSymbolResponse.ActivSymbol);
                                            LogAndDisplayOnInformationReporter.GetInstance.Write("Data added in dictActivAndEncryptedActivMapping. EncryptedActiv Symbol: " + responseParameters.ResponseBlockList[0].ResponseKey.Symbol + ", Activ Symbol: " + marketDataSymbolResponse.ActivSymbol, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);
                                        }
                                    }
                                }

                                DispatchMessage(snapShotData, loggingOutput, isSnapshotData, isValidResponse);

                                if (isSnapshotData && snapShotData.CategoryCode == AssetCategory.Equity)
                                    GetSharesOutstanding(marketDataSymbolResponse);
                            }
                        }
                        else
                        {
                            //Handling of SharesOutstanding
                            isValidResponse = false;
                            string activSymbol = string.Empty;
                            lock (dictMergentActivAndActivMapping)
                            {
                                if (dictMergentActivAndActivMapping.ContainsKey(responseParameters.ResponseBlockList[0].RequestedKey.Symbol))
                                {
                                    activSymbol = dictMergentActivAndActivMapping[responseParameters.ResponseBlockList[0].RequestedKey.Symbol];
                                }
                            }

                            if (!string.IsNullOrEmpty(activSymbol))
                            {
                                if (UserSettingConstants.IsDebugModeEnabled)
                                {
                                    loggingOutput.Append(string.Format("Subscription cookie ............... {0}\n", SubscriptionCookieToString(responseParameters.SubscriptionCookie)));
                                }

                                snapShotData = MarketDataAdapterExtension.GetSnapShotSymbolData(activSymbol);
                                if (snapShotData != null)
                                {
                                    for (int count = 0, size = responseParameters.ResponseBlockList.Count; count < size; ++count)
                                    {
                                        ResponseBlock responseBlock = responseParameters.ResponseBlockList[count];

                                        if (UserSettingConstants.IsDebugModeEnabled)
                                        {
                                            loggingOutput.Append(string.Format("**** Response block {0}/{1} ****\n", (count + 1), size));
                                            loggingOutput.Append(string.Format("Requested key ..................... {0}\n", responseBlock.RequestedKey.ToString()));
                                            loggingOutput.Append(string.Format("Resolved key ...................... {0}\n", responseBlock.ResolvedKey.ToString()));
                                            loggingOutput.Append(string.Format("Relationship id {0,3} ............... {1}\n", (int)responseBlock.RelationshipId, EnumDescription.GetDescription(responseBlock.StatusCode)));
                                            loggingOutput.Append(string.Format("Flags ............................. {0}\n", responseBlock.FlagsToString(responseBlock.Flags)));

                                            if (responseBlock.IsResponseKeyDefined())
                                            {
                                                loggingOutput.Append(string.Format("Response key ...................... {0}\n", responseBlock.ResponseKey.ToString()));
                                            }

                                            DisplayPermissionInfo(responseBlock.PermissionId, responseBlock.permissionLevel, responseBlock.PermissionLevelData, loggingOutput);
                                        }

                                        if (responseBlock.IsValidResponse() && !responseBlock.FieldData.IsEmpty())
                                        {
                                            isValidResponse = true;
                                            ParseResponse(ref snapShotData, responseBlock.FieldData, loggingOutput, ref isOddLotTrade);
                                        }
                                    }

                                    DispatchMessage(snapShotData, loggingOutput, true, isValidResponse);
                                }
                            }
                            else
                            {
                                if (UserSettingConstants.IsDebugModeEnabled)
                                {
                                    loggingOutput.Append(string.Format("ACTIV Symbol ...................... {0}\n", responseParameters.ResponseBlockList[0].ResolvedKey.Symbol));
                                    LogAndDisplayOnInformationReporter.GetInstance.Write("Nirvana Internal Logging - Response Received (ShareOutstanding - Skipped):" + Environment.NewLine + loggingOutput.ToString(), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Failed to deserialize response, {0}\n", statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
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

        override public void OnRecordUpdate(HeapMessage response)
        {
            try
            {
                StringBuilder loggingOutput = new StringBuilder();

                RecordUpdate recordUpdate = new RecordUpdate();
                StatusCode statusCode = RecordUpdateHelper.Deserialize(this, response, recordUpdate);

                if (UserSettingConstants.IsDebugModeEnabled)
                {
                    loggingOutput.Append(string.Format("**** Update received for {0} ****\n", recordUpdate.SymbolId.ToString()));

                    if ((recordUpdate.Flags & RecordUpdate.FlagRequestKey) != 0)
                        loggingOutput.Append(string.Format("Request key ....................... {0}\n", recordUpdate.RequestKey.ToString()));

                    if ((recordUpdate.Flags & RecordUpdate.FlagRelationshipId) != 0)
                        loggingOutput.Append(string.Format("Relationship id ................... {0}\n", (int)recordUpdate.RelationshipId));

                    if (response.RequestId.Id.Length > 0)
                        loggingOutput.Append(string.Format("Request id ........................ {0}\n", response.RequestId.ToString()));

                    loggingOutput.Append(string.Format("Flags ............................. {0}\n", RecordUpdate.FlagsToString(recordUpdate.Flags)));
                    loggingOutput.Append(string.Format("Update id ......................... {0}\n", (int)recordUpdate.UpdateId));
                    loggingOutput.Append(string.Format("Event type ........................ {0}\n", recordUpdate.EventType));
                    DisplayPermissionInfo(recordUpdate.PermissionId, recordUpdate.PermissionLevel, recordUpdate.PermissionLevelData, loggingOutput);
                }

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    bool isOddLotTrade = false;
                    SymbolData snapShotData = null;

                    lock (dictActivEncryptedAndActivMapping)
                    {
                        if (dictActivEncryptedAndActivMapping.ContainsKey(recordUpdate.SymbolId.Symbol))
                        {
                            snapShotData = MarketDataAdapterExtension.GetSnapShotSymbolData(dictActivEncryptedAndActivMapping[recordUpdate.SymbolId.Symbol]);
                            if (snapShotData.CategoryCode == AssetCategory.EquityOption)
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Continous Option Response received for Symbol: {0}, ActivSymbol: {1}", snapShotData.Symbol, snapShotData.ActivSymbol), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                        }
                    }

                    if (snapShotData != null)
                    {
                        snapShotData.PricingStatus = ((recordUpdate.PermissionLevel == 0) ? PricingStatus.RealTime : PricingStatus.Delayed);
                        snapShotData.DelayInterval = recordUpdate.PermissionLevelData.DelayPeriod.ToString();

                        double lastPriceBeforeOddLot = snapShotData.LastPrice;
                        ParseResponse(ref snapShotData, recordUpdate.FieldData, loggingOutput, ref isOddLotTrade);

                        if (_isSkipOddLotTrades && isOddLotTrade)
                        {
                            snapShotData.LastPrice = lastPriceBeforeOddLot;
                            loggingOutput.AppendLine(string.Format("Due to IsSkipOddLotTrades config, Odd lot trade price skipped. In this case, Last : {0}", snapShotData.LastPrice));
                        }

                        DispatchMessage(snapShotData, loggingOutput, false, true);
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Failed to deserialize response, {0}\n", statusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
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

        private void DispatchMessage(SymbolData snapShotData, StringBuilder loggingOutput, bool isSnapshotData, bool isValidResponse)
        {
            try
            {
                if (isValidResponse)
                {
                    MarketDataAdapterExtension.AddToSnapShotSymbolDataCollection(ref snapShotData, MarketDataProvider.ACTIV);

                    Data obj = new Data();
                    obj.Info = snapShotData;
                    if (obj.Info != null)
                    {
                        if (isSnapshotData)
                        {
                            MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: NirvanaContentGatewayClient.DispatchMessage() > SnapshotData released from Adapter for Symbol: {0}, Time: {1}", snapShotData.Symbol, System.DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss:fff")));

                            if (SnapShotDataResponse != null)
                                SnapShotDataResponse(this, obj);
                        }
                        else if (ContinuousDataResponse != null)
                        {
                            System.DateTime _marketDataStartTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, Convert.ToInt32(_configMarketDataStartTime[0]), Convert.ToInt32(_configMarketDataStartTime[1]), 0);
                            System.DateTime _marketDataStopTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, Convert.ToInt32(_configMarketDataStopTime[0]), Convert.ToInt32(_configMarketDataStopTime[1]), 0);

                            if (((Convert.ToInt32(_configMarketDataStartTime[0]) == 0 && Convert.ToInt32(_configMarketDataStartTime[1]) == 0) || System.DateTime.Now > _marketDataStartTime)
                                && ((Convert.ToInt32(_configMarketDataStopTime[0]) == 0 && Convert.ToInt32(_configMarketDataStopTime[1]) == 0) || System.DateTime.Now < _marketDataStopTime))
                                ContinuousDataResponse(this, obj);
                            else
                                loggingOutput.AppendLine("This message has been skipped due to MarketDataStopTime config.");
                        }
                    }

                    if (UserSettingConstants.IsDebugModeEnabled)
                    {
                        StringBuilder loggingOutputDetailed = new StringBuilder();
                        loggingOutputDetailed.AppendLine(string.Format("Ticker Symbol: {0}", snapShotData.Symbol));
                        loggingOutputDetailed.AppendLine(string.Format("ACTIV Symbol: {0}", snapShotData.ActivSymbol));
                        loggingOutputDetailed.AppendLine(string.Format("Last: {0}", snapShotData.LastPrice));
                        loggingOutputDetailed.AppendLine(string.Format("Ask: {0}", snapShotData.Ask));
                        loggingOutputDetailed.AppendLine(string.Format("Bid: {0}", snapShotData.Bid));
                        loggingOutputDetailed.AppendLine(string.Format("Closing Price: {0}", snapShotData.Previous));
                        loggingOutputDetailed.AppendLine(string.Format("Update Time: {0}", snapShotData.UpdateTime));

                        LogAndDisplayOnInformationReporter.GetInstance.Write("Detailed Logging - Response Dispatched:" + Environment.NewLine + loggingOutputDetailed.ToString(), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
                    }
                }

                if (UserSettingConstants.IsDebugModeEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write("Nirvana Internal Logging - Response Received:" + Environment.NewLine + loggingOutput.ToString(), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
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

        private void ParseResponse(ref SymbolData snapShotData, FieldData fieldData, StringBuilder loggingOutput, ref bool isOddLotTrade)
        {
            try
            {
                System.DateTime updateDate = System.DateTime.MinValue;
                System.TimeSpan updateTimeSpan = System.TimeSpan.Zero;

                try
                {
                    fieldListValidator.Initialize(fieldData);
                }
                catch (MiddlewareException e)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Failed to initialize fieldListValidator, {0}\n", e.StatusCode.ToString()), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Error);
                    return;
                }

                foreach (FieldListValidator.Field field in fieldListValidator)
                {
                    StringBuilder buff = new StringBuilder();

                    if (UserSettingConstants.IsDebugModeEnabled)
                    {
                        UniversalFieldHelper universalFieldHelper = MetaData.GetUniversalFieldHelper2(this, field.FieldId);
                        buff.Append(universalFieldHelper == null ? "" : universalFieldHelper.Name);
                        buff.Append(" [");
                        buff.Append(field.FieldId);
                        buff.Append("]");

                        while (buff.Length < 35)
                            buff.Append(".");
                    }

                    if (FieldStatuses.FieldStatusDefined == field.FieldStatus)
                    {
                        if (UserSettingConstants.IsDebugModeEnabled)
                        {
                            loggingOutput.Append(string.Format("{0} {1}{2}\n", buff.ToString(), field.FieldType.ToString(), (field.DoesUpdateLastValue() ? "" : " *")));
                        }

                        switch (field.FieldId)
                        {
                            case FieldIds.FID_TRADE_CONDITION:
                                //https://support.activfinancial.com/modules/feed_info/index.php/feed-detail.php?feed=Cta
                                if (HexToString(field.FieldType.ToString()).Contains("I"))
                                    isOddLotTrade = true;
                                break;
                            case FieldIds.FID_UNDERLYING_SYMBOL:
                                MarketDataSymbolResponse fssrEO = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(field.FieldType.ToString());
                                if (fssrEO != null)
                                    snapShotData.UnderlyingSymbol = fssrEO.TickerSymbol;
                                else
                                {
                                    string underlyingSymbol = field.FieldType.ToString().Split('.')[0];
                                    if (underlyingSymbol.StartsWith("="))
                                    {
                                        //Handling of Index symbols (Format =SPX.WI)
                                        snapShotData.UnderlyingSymbol = "$" + underlyingSymbol.Substring(1);
                                    }
                                    else
                                    {
                                        snapShotData.UnderlyingSymbol = underlyingSymbol;
                                    }
                                }
                                break;
                            case FieldIds.FID_PRIMARY_EXCHANGE:
                                snapShotData.ListedExchange = field.FieldType.ToString();
                                break;
                            case FieldIds.FID_LOT_SIZE:
                            case FieldIds.FID_CONTRACT_SIZE:
                                snapShotData.Multiplier = Convert.ToInt64(field.FieldType.ToString());
                                break;
                            case FieldIds.FID_VWAP:
                                snapShotData.VWAP = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_CUSIP:
                                snapShotData.CusipNo = field.FieldType.ToString();
                                break;
                            case FieldIds.FID_SEDOL_CODE:
                                snapShotData.SedolSymbol = field.FieldType.ToString();
                                break;
                            //case FieldIds.:
                            //    snapShotData.Volume10DAvg = ;
                            //    break;
                            //case FieldIds.:
                            //    snapShotData.AverageVolume20Day = ;
                            //    break;
                            case FieldIds.FID_OPTION_TYPE:
                                if (field.FieldType.ToString().Equals("P"))
                                    snapShotData.PutOrCall = BusinessObjects.AppConstants.OptionType.PUT;
                                else
                                    snapShotData.PutOrCall = BusinessObjects.AppConstants.OptionType.CALL;
                                break;
                            case FieldIds.FID_OPEN_INTEREST:
                                snapShotData.OpenInterest = Convert.ToDouble(((UInt)field.FieldType).Value);
                                break;
                            case FieldIds.FID_EXPIRATION_DATE:
                                snapShotData.ExpirationDate = System.DateTime.ParseExact(field.FieldType.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                break;
                            case FieldIds.FID_STRIKE_PRICE:
                                snapShotData.StrikePrice = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_TRADE:
                                snapShotData.LastPrice = ((TRational)field.FieldType).Value.Double;
                                break;
                            case FieldIds.FID_CUMULATIVE_VOLUME:
                                snapShotData.TotalVolume = snapShotData.TradeVolume = Convert.ToInt64(((UInt)field.FieldType).Value);
                                break;
                            case FieldIds.FID_BID:
                                snapShotData.Bid = ((TRational)field.FieldType).Value.Double;
                                break;
                            case FieldIds.FID_BID_SIZE:
                                snapShotData.BidSize = Convert.ToInt64(((UInt)field.FieldType).Value);
                                break;
                            case FieldIds.FID_BID_EXCHANGE:
                                snapShotData.BidExchange = field.FieldType.ToString();
                                break;
                            case FieldIds.FID_ASK:
                                snapShotData.Ask = ((TRational)field.FieldType).Value.Double;
                                break;
                            case FieldIds.FID_ASK_SIZE:
                                snapShotData.AskSize = Convert.ToInt64(((UInt)field.FieldType).Value);
                                break;
                            case FieldIds.FID_ASK_EXCHANGE:
                                snapShotData.AskExchange = field.FieldType.ToString();
                                break;
                            case FieldIds.FID_TRADE_HIGH:
                                snapShotData.High = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_TRADE_LOW:
                                snapShotData.Low = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_OPEN:
                                snapShotData.Open = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_SHARES_OUTSTANDING:
                                snapShotData.SharesOutstanding = Convert.ToInt64(Convert.ToDecimal(field.FieldType.ToString()));
                                break;
                            case FieldIds.FID_PREVIOUS_CLOSE:
                                snapShotData.Previous = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_NAME:
                                snapShotData.FullCompanyName = field.FieldType.ToString().Trim();
                                break;
                            //case FieldIds.:
                            //    ((OptionSymbolData)snapShotData).OSIOptionSymbol = ;
                            //    break;
                            //case FieldIds.:
                            //    ((OptionSymbolData)snapShotData).IDCOOptionSymbol = ;
                            //    break;
                            //case FieldIds.:
                            //    ((OptionSymbolData)snapShotData).OpraSymbol = ;
                            //    break;
                            case FieldIds.FID_TRADE_DATE:
                                System.DateTime.TryParseExact(field.FieldType.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate);
                                if (updateDate == System.DateTime.MinValue || updateDate == DateTimeConstants.MinValue)
                                    updateDate = System.DateTime.Today;
                                break;
                            case FieldIds.FID_TRADE_TIME:
                                updateTimeSpan = (TimeSpan.ParseExact(field.FieldType.ToString(), new string[] { @"hh\:mm\:ss\.fff", @"hh\:mm\:ss" }, null));
                                break;
                            //case FieldIds.:
                            //    snapShotData.LastTick = ;
                            //    break;
                            case FieldIds.FID_BETA:
                                snapShotData.Beta_5yrMonthly = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_DIVIDEND_YIELD_5_YEAR_AVERAGE:
                                snapShotData.DividendYield = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_DIVIDEND:
                                snapShotData.Dividend = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_EX_DATE:
                                snapShotData.XDividendDate = System.DateTime.ParseExact(field.FieldType.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                break;
                            //case FieldIds.:
                            //    snapShotData.DividendInterval = ;
                            //    break;
                            case FieldIds.FID_DIVIDENDS_PAID_PER_SHARE:
                                snapShotData.DividendAmtRate = float.Parse(field.FieldType.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                break;
                            case FieldIds.FID_INDICATED_ANNUAL_DIVIDEND:
                                snapShotData.AnnualDividend = ((Rational)field.FieldType).Double;
                                break;
                            case FieldIds.FID_CURRENCY:
                                snapShotData.CurencyCode = field.FieldType.ToString();
                                break;
                        }
                    }
                    else
                    {
                        if (UserSettingConstants.IsDebugModeEnabled)
                        {
                            loggingOutput.Append(string.Format("{0} {1}{2}\n", buff.ToString(), FieldStatuses.FieldStatusToString(field.FieldStatus), (field.DoesUpdateLastValue() ? "" : " *")));
                        }
                    }
                }

                if (updateDate != System.DateTime.MinValue && updateTimeSpan != System.TimeSpan.Zero)
                {
                    snapShotData.UpdateTime = (updateDate + updateTimeSpan).ToUniversalTime();
                }

                //Change is not coming from ACTIV, so applying custom logic
                snapShotData.Change = snapShotData.LastPrice - snapShotData.Previous;

                if (UserSettingConstants.IsDebugModeEnabled)
                {
                    loggingOutput.AppendLine(string.Format("Odd Lot Trade: " + isOddLotTrade.ToString()));
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

        private void DisplayPermissionInfo(int permissionId, byte permissionLevel, PermissionLevelData permissionLevelData, StringBuilder output)
        {
            switch (permissionId)
            {
                case PermissionIds.PermissionIdUnknown:
                    output.Append("Permission id ..................... unknown\n");
                    break;

                case PermissionIds.PermissionIdList:
                    output.Append("Permission id ..................... list\n");
                    break;

                default:
                    output.Append(string.Format("Permission id ..................... {0}\n", (int)permissionId));
                    break;
            }

            if (PermissionLevels.PermissionLevelDefault != permissionLevel)
            {
                output.Append(string.Format("Permission level .................. {0}\n", PermissionLevels.PermissionLevelToString(permissionLevel)));

                switch (permissionLevel)
                {
                    case PermissionLevels.PermissionLevelRealtime:
                        // No data for realtime permission level
                        break;

                    case PermissionLevels.PermissionLevelDelayed:
                        // Delayed response block. display delay in minutes.
                        output.Append(string.Format("Delay period (mins) ............... {0}\n", permissionLevelData.DelayPeriod));
                        break;
                }
            }
        }

        private string SubscriptionCookieToString(long subscriptionCookie)
        {
            if (MiscConsts.SubscriptionCookieUndefined == subscriptionCookie)
                return "undefined";

            return subscriptionCookie.ToString();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            return dictTickersLastStatusCode;
        }

        private string GetLastQuarterEndDate()
        {
            string endMonth = string.Empty;
            int endYear = System.DateTime.Now.Year;
            int endDay = 31;

            switch (GetLastQuarterDates(System.DateTime.Now.Month))
            {
                case "Q1":
                    endMonth = "03";
                    break;

                case "Q2":
                    endMonth = "06";
                    endDay = 30;
                    break;

                case "Q3":
                    endMonth = "09";
                    endDay = 30;
                    break;

                case "Q4":
                    endMonth = "12";
                    endYear = System.DateTime.Now.Year - 1;
                    break;
            }

            return string.Format("{0}{1}{2}", endYear, endMonth, endDay);
        }

        private string GetLastQuarterDates(int month)
        {
            string quarter = string.Empty;

            if (month >= 1 && month <= 3)
            {
                quarter = "Q4";
            }
            if (month >= 4 && month <= 6)
            {
                quarter = "Q1";
            }
            if (month >= 7 && month <= 9)
            {
                quarter = "Q2";
            }
            if (month >= 10 && month <= 12)
            {
                quarter = "Q3";
            }

            return quarter;
        }

        public string HexToString(string hexString)
        {
            try
            {
                if (hexString == null || (hexString.Length & 1) == 1)
                {
                    throw new ArgumentException();
                }
                var sb = new StringBuilder();
                for (var i = 2; i < hexString.Length; i += 2)
                {
                    var hexChar = hexString.Substring(i, 2);
                    sb.Append((char)Convert.ToByte(hexChar, 16));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }
        #endregion

        #region MetaData
        public Dictionary<string, string> GetUserInformation()
        {
            Dictionary<string, string> userInformation = new Dictionary<string, string>();

            try
            {
                UserRecord userRecord;
                try
                {
                    userRecord = MetaData.GetPermissionContext(this, string.Empty).UserRecord;

                    userInformation.Add("User id", userRecord.UserId);
                    userInformation.Add("Password", userRecord.Password.ToString(false));
                    userInformation.Add("User type", userRecord.UserType.Initialized ? EnumDescription.GetDescription((UserTypeEnum)(userRecord.UserType.Value)) : userRecord.UserType.ToString());
                    userInformation.Add("User flags", userRecord.UserFlags.Value.ToString());
                    userInformation.Add("Server ids", string.Join(",", new List<string>(userRecord.ServerIdList).ToArray()));
                    userInformation.Add("Parent user ids", string.Join(",", new List<string>(userRecord.ParentUserIdList).ToArray()));
                    userInformation.Add("Start date", userRecord.StartDate.ToString());
                    userInformation.Add("Expiration date", userRecord.ExpirationDate.ToString());
                    userInformation.Add("Max concurrent logons", userRecord.MaxNumberOfConcurrentUsers.ToString());
                    userInformation.Add("Max tx queue bytes", userRecord.MaxTxQueueBytes.ToString());
                    userInformation.Add("Response block hard limit", userRecord.ResponseBlockHardLimit.ToString());
                    userInformation.Add("Response block quota", userRecord.ResponseBlockQuota.ToString());
                    userInformation.Add("Subscription cookie quota", userRecord.SubscriptionCookieQuota.ToString());
                    userInformation.Add("Symbol subscription quota", userRecord.SymbolSubscriptionQuota.ToString());

                    if (userRecord.AllowedExecutableNamePatternList.Count != 0)
                    {
                        userInformation.Add("Allowed executables", string.Join(",", new List<string>(userRecord.AllowedExecutableNamePatternList).ToArray()));
                    }
                    if (userRecord.RequiredSubLogonExecutableNamePatternList.Count != 0)
                    {
                        userInformation.Add("Required sub-logon executables", string.Join(",", new List<string>(userRecord.RequiredSubLogonExecutableNamePatternList).ToArray()));
                    }
                    if (userRecord.AllowedSubLogonExecutableNamePatternList.Count != 0)
                    {
                        userInformation.Add("Allowed sub-logon executable", string.Join(",", new List<string>(userRecord.AllowedSubLogonExecutableNamePatternList).ToArray()));
                    }

                    userInformation.Add("Update id", userRecord.UpdateId.ToString());
                }
                catch (MiddlewareException) { }
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

            return userInformation;
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            List<Dictionary<string, string>> userPermissionsInformation = new List<Dictionary<string, string>>();

            try
            {
                PermissionInfo permissionInfo = new PermissionInfo();
                StatusCode statusCode = MetaData.GetPermissionInfo(this, permissionInfo);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    foreach (var permissionInfoMap in permissionInfo.PermissionLevelInfoList)
                    {
                        Dictionary<string, string> permissions = new Dictionary<string, string>();

                        foreach (int id in permissionInfoMap.Keys)
                        {
                            permissions.Add(id.ToString(), permissionInfoMap[id].Name);
                        }

                        userPermissionsInformation.Add(permissions);
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

            return userPermissionsInformation;
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            Dictionary<string, int> subscriptionInformation = new Dictionary<string, int>();

            try
            {
                SubscriptionInfo subscriptionInfo = new SubscriptionInfo();

                // Realtime
                StatusCode statusCode = GetSubscriptionInfo(0, subscriptionInfo);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    foreach (SymbolId symbolId in subscriptionInfo.SymbolSubscriptionMap.Keys)
                    {
                        string tickerSymbol = string.Empty;
                        lock (dictEncryptedActivAndTickerMapping)
                        {
                            if (dictEncryptedActivAndTickerMapping.ContainsKey(symbolId.Symbol))
                            {
                                tickerSymbol = dictEncryptedActivAndTickerMapping[symbolId.Symbol];
                            }
                        }

                        if (string.IsNullOrEmpty(tickerSymbol))
                        {
                            MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(symbolId.Symbol);
                            if (marketDataSymbolResponse != null)
                            {
                                tickerSymbol = marketDataSymbolResponse.TickerSymbol;
                            }
                        }

                        if (!subscriptionInformation.ContainsKey(tickerSymbol))
                            subscriptionInformation.Add(tickerSymbol, subscriptionInfo.SymbolSubscriptionMap[symbolId]);
                    }
                }

                // Delayed
                statusCode = GetSubscriptionInfo(1, subscriptionInfo);

                if (statusCode == StatusCode.StatusCodeSuccess)
                {
                    foreach (SymbolId symbolId in subscriptionInfo.SymbolSubscriptionMap.Keys)
                    {
                        string tickerSymbol = string.Empty;
                        lock (dictEncryptedActivAndTickerMapping)
                        {
                            if (dictEncryptedActivAndTickerMapping.ContainsKey(symbolId.Symbol))
                            {
                                tickerSymbol = dictEncryptedActivAndTickerMapping[symbolId.Symbol];
                            }
                        }

                        if (string.IsNullOrEmpty(tickerSymbol))
                        {
                            MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(symbolId.Symbol);
                            if (marketDataSymbolResponse != null)
                            {
                                tickerSymbol = marketDataSymbolResponse.TickerSymbol;
                            }
                        }

                        if (!subscriptionInformation.ContainsKey(tickerSymbol))
                            subscriptionInformation.Add(tickerSymbol, subscriptionInfo.SymbolSubscriptionMap[symbolId]);
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

            return subscriptionInformation;
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                currentRetryAttemptClearTimer.Dispose();
            }
        }
        #endregion
    }
}