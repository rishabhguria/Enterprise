using Bloomberglp.Blpapi;
using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.SAPIAdapter.Models;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static Bloomberglp.Blpapi.Event;

namespace Prana.SAPIAdapter
{
    public class SAPIManager : IDisposable, ILiveFeedAdapter
    {
        private static object _lockerObject = new object();
        private string _clientServerAddress = string.Empty;
        private string _clientConnectionPassword = string.Empty;
        private string _sapiPort = string.Empty;
        private const string encryptionKey = @"sblw-3hn8-sqoy19";
        private const string CONST_SAPICredentialDetails = "SAPICredentialDetails";
        private const string CONST_SAPIClientCredentials = "SAPIClientCredentials";
        private const string CONST_SAPITokenAuthURL = "SAPITokenAuthURL";
        private const string CONST_SAPITrustMaterial = "SAPITrustMaterial";
        private const string CONST_SAPIAuthApplication = "SAPIAuthApplication";
        private Dictionary<string, MarketDataSymbolResponse> _dictSymbolDataInfo = new Dictionary<string, MarketDataSymbolResponse>();
        private List<string> _lstSubscribedSymbol = new List<string>();
        private SessionOptions _sessionOptions = null;
        private Session _session = null;
        private Session _authenticateUserSession = null;
        private bool _isSessionStarted = false;
        private bool _isAuthenticateUserSessionStarted = false;
        private Dictionary<string, List<string>> _dictAuthenticateUsernameIPAddress = new Dictionary<string, List<string>>();
        private static readonly Name CONST_TokenSuccess = Name.GetName("TokenGenerationSuccess");
        private static readonly Name CONST_TokenFailure = Name.GetName("TokenGenerationFailure");
        private static readonly Name CONST_AuthorizationSuccess = Name.GetName("AuthorizationSuccess");
        private static readonly Name CONST_AuthorizationFailure = Name.GetName("AuthorizationFailure");
        private static readonly Name CONST_AuthorizationRevoked = Name.GetName("AuthorizationRevoked");
        private bool _configEnableMarketDataSimulationForAutomation = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableMarketDataSimulationForAutomation"]);
        private int _hitCounter = 0;
        private int _authHitCounter = 0;

        // Adding Vaiables for Samsara SAPI handling
        private static readonly Name ReturnEids = Name.GetName("returnEids");
        //This Dictionary maintains User Identity Mapping with User Id. Generated after successful authorization from SAPI.
        private Dictionary<int, UserIdentity> _dictUserIdentityMapping = new Dictionary<int, UserIdentity>();
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(1200) };
        private static readonly Dictionary<string, IMarketDataPermissionServiceCallback> CallbackChannels = new Dictionary<string, IMarketDataPermissionServiceCallback>();
        private static readonly Dictionary<int, HashSet<string>> failedEntitlementSymbols = new Dictionary<int, HashSet<string>>();
        // Caches all public instance properties of SymbolData (and its derived types) for efficient reflection-based access.
        private static readonly PropertyInfo[] SymbolDataProperties = typeof(SymbolData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        #region Services
        private Service _referenceService = null;
        private Service _authService = null;
        #endregion

        /// <summary>
        /// Event handler for User Authentication Response.
        /// </summary>
        public event EventHandler<EventArgs<string, bool>> AuthenticateUserResponse;

        private static SAPIManager _instance = null;
        public static SAPIManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new SAPIManager();
                        }
                    }
                }
                return _instance;
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
                return null;
            }
        }

        #region Constructor
        public SAPIManager()
        {
            try
            {
                LoadCredentials();
                MarketDataAdapterExtension.CreateSecMasterServicesProxy();
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
        #endregion

        #region Private Methods
        private SessionOptions CreateSessionOptions(bool sessionOptionToAuthenticateUser)
        {
            SessionOptions sessionOptions = new SessionOptions();
            try
            {
                List<SessionOptions.ServerAddress> servers = new List<SessionOptions.ServerAddress>();

                TlsOptions tlsOptions = null;

                string tt1 = new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString();

                string clientCredentials = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPIClientCredentials));
                string trustMaterial = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPITrustMaterial));
                string authApplicationName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPIAuthApplication);
                if (!string.IsNullOrEmpty(_sapiPort))
                {
                    SessionOptions.ServerAddress serverAddress = new SessionOptions.ServerAddress(_clientServerAddress, int.Parse(_sapiPort));
                    servers.Add(serverAddress);
                }
                if (clientCredentials != null &&
                    !string.IsNullOrEmpty(_clientConnectionPassword) &&
                    trustMaterial != null)
                {
                    using (var password = new System.Security.SecureString())
                    {
                        foreach (char c in _clientConnectionPassword)
                        {
                            password.AppendChar(c);
                        }

                        tlsOptions = TlsOptions.CreateFromFiles(
                            clientCredentials,
                            password,
                            trustMaterial);
                    }
                }
                sessionOptions.ServerAddresses = servers.ToArray();
                sessionOptions.TlsOptions = tlsOptions;
                if (sessionOptionToAuthenticateUser)
                    sessionOptions.AuthenticationOptions = SAPIConstants.Const_AuthenticationOption + authApplicationName;
                else
                {
                    AuthApplication authApplication = new AuthApplication(authApplicationName);
                    AuthOptions sessionIdentityAuthOptions = new AuthOptions(authApplication);
                    sessionOptions.SetSessionIdentityOptions(sessionIdentityAuthOptions);
                }

            }
            catch (CryptographicException ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error occurred while establishing a connection with SAPI. Error: {0}", ex.Message), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                return null;
            }
            catch (ArgumentException ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error occurred while establishing a connection with SAPI. Error: {0}", ex.Message), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                return null;
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
            return sessionOptions;
        }

        private void CreateSessionUserAuthentication()
        {
            try
            {
                if (_isAuthenticateUserSessionStarted && _authenticateUserSession != null)
                {
                    _authenticateUserSession.Stop();
                }
                SessionOptions authenticateUseSessionOptions = CreateSessionOptions(true);
                if (authenticateUseSessionOptions != null)
                {
                    _authenticateUserSession = new Session(authenticateUseSessionOptions, ProcessEventUserAuthenticate);
                    _isAuthenticateUserSessionStarted = _authenticateUserSession.Start();
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

        private string GetAssetWiseFieldToRequest(AssetCategory assetCategory, string policy)
        {
            string fields = string.Empty;
            try
            {
                Dictionary<string, string> dictAssetWiseFields = new Dictionary<string, string>();
                if (policy == SAPIConstants.Const_Snapshot)
                    dictAssetWiseFields = DeepCopyHelper.Clone<Dictionary<string, string>>(MarketDataAdapterExtension.DictAssetWiseFieldsSnapshot);
                else
                    dictAssetWiseFields = DeepCopyHelper.Clone<Dictionary<string, string>>(MarketDataAdapterExtension.DictAssetWiseFieldsSubcription);
                switch (assetCategory)
                {
                    case AssetCategory.Equity:
                        fields = dictAssetWiseFields[SAPIConstants.Const_Equity];
                        break;
                    case AssetCategory.Future:
                        fields = dictAssetWiseFields[SAPIConstants.Const_Future];
                        break;
                    case AssetCategory.EquityOption:
                    case AssetCategory.Option:
                        fields = dictAssetWiseFields[SAPIConstants.Const_EquityOption];
                        break;
                    case AssetCategory.FutureOption:
                        fields = dictAssetWiseFields[SAPIConstants.Const_FutureOption];
                        break;
                    case AssetCategory.FX:
                        fields = dictAssetWiseFields[SAPIConstants.Const_FX];
                        break;
                    case AssetCategory.FXForward:
                        fields = dictAssetWiseFields[SAPIConstants.Const_FXForward];
                        break;
                    case AssetCategory.FixedIncome:
                        fields = dictAssetWiseFields[SAPIConstants.Const_FixedIncome];
                        break;
                    default:
                        fields = SAPIConstants.Const_FieldStr;
                        break;
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
            return fields;
        }

        private string GetSubscriptionOptions()
        {
            string options = string.Empty;
            try
            {
                string finalInterval = string.Empty;
                double configRefreshInterval = 0;
                if (double.TryParse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, SAPIConstants.Const_RefreshInterval), out configRefreshInterval))
                {
                    if (configRefreshInterval < double.Parse(SAPIConstants.Const_MinIntervalTime))
                        finalInterval = SAPIConstants.Const_MinIntervalTime;
                    else if (configRefreshInterval > double.Parse(SAPIConstants.Const_MaxIntervalTime))
                        finalInterval = SAPIConstants.Const_MaxIntervalTime;
                    else
                        finalInterval = configRefreshInterval.ToString();

                    options = SAPIConstants.Const_Options_Interval + finalInterval;
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
            return options;
        }

        private void ProcessEvent(Event eventObj, Session session)
        {
            try
            {
                switch (eventObj.Type)
                {
                    case EventType.SUBSCRIPTION_DATA:
                        ProcessSubscriptionDataEvent(eventObj);
                        break;
                    case EventType.SUBSCRIPTION_STATUS:
                        ProcessSubscriptionStatus(eventObj);
                        break;
                    case EventType.RESPONSE:
                    case EventType.PARTIAL_RESPONSE:
                        ProcessSnapShotDataEvent(eventObj);
                        break;

                    default:
                        return;
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

        private void ProcessEventUserAuthenticate(Event eventObj, Session session)
        {
            try
            {
                switch (eventObj.Type)
                {
                    case EventType.RESPONSE:
                    case EventType.PARTIAL_RESPONSE:
                    case EventType.AUTHORIZATION_STATUS:
                        ProcessAuthorizationDataEvent(eventObj);
                        break;

                    default:
                        return;
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

        private SymbolData FillSymbolData(Element fieldData, string symbol, bool isSnapShot)
        {
            SymbolData symbolData = new SymbolData();
            MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse();
            string AssetClass = string.Empty;
            string underlyingSymbol = string.Empty;
            try
            {
                if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_AssetClass)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_AssetClass)).IsNull)
                    AssetClass = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_AssetClass));

                if (_dictSymbolDataInfo.ContainsKey(symbol))
                    marketDataSymbolResponse = _dictSymbolDataInfo[symbol];
                else
                    marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);

                if (marketDataSymbolResponse != null && !string.IsNullOrEmpty(marketDataSymbolResponse.BloombergSymbol))
                    symbolData = MarketDataAdapterExtension.GetSnapShotSymbolData(marketDataSymbolResponse.BloombergSymbol);
                if (symbolData == null)
                {
                    if (marketDataSymbolResponse != null)
                    {
                        if (marketDataSymbolResponse.AssetCategory == AssetCategory.None)
                        {
                            switch (AssetClass)
                            {
                                case SAPIConstants.Const_Option:
                                    if (marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Index) || marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Comdty) || marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Curncy))
                                        marketDataSymbolResponse.AssetCategory = AssetCategory.FutureOption;
                                    else
                                        marketDataSymbolResponse.AssetCategory = AssetCategory.EquityOption;
                                    break;
                                case SAPIConstants.Const_FixedIncome:
                                    marketDataSymbolResponse.AssetCategory = AssetCategory.FixedIncome;
                                    break;
                                case SAPIConstants.Const_index:
                                case SAPIConstants.Const_Future:
                                    marketDataSymbolResponse.AssetCategory = AssetCategory.Future;
                                    break;
                                case SAPIConstants.Const_Equity:
                                    marketDataSymbolResponse.AssetCategory = AssetCategory.Equity;
                                    break;
                                default:
                                    break;
                            }
                        }
                        AssetCategory category = marketDataSymbolResponse.AssetCategory;
                        switch (category)
                        {
                            case AssetCategory.Equity:
                                symbolData = new EquitySymbolData();
                                symbolData.CategoryCode = category;
                                symbolData.UnderlyingSymbol = symbol;
                                if (isSnapShot)
                                {
                                    string compositeCode = string.Empty;
                                    string exchangeCode = string.Empty;
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Comp_Code)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Comp_Code)).IsNull)
                                        compositeCode = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Comp_Code));

                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate)));

                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                    {
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                        exchangeCode = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                    }

                                    if (marketDataSymbolResponse.BloombergSymbol != null)
                                    {
                                        string bloombergSymbol = marketDataSymbolResponse.BloombergSymbol;
                                        int bloombergCodePosition = bloombergSymbol.IndexOf(' ') + 1;
                                        if (compositeCode != string.Empty)
                                            symbolData.BloombergSymbol = bloombergSymbol.Substring(0, bloombergCodePosition) + compositeCode + bloombergSymbol.Substring(bloombergCodePosition + 2);
                                        if (marketDataSymbolResponse.BloombergSymbol != symbolData.BloombergSymbol)
                                            symbolData.BloombergSymbolWithExchangeCode = marketDataSymbolResponse.BloombergSymbol;
                                        else if (exchangeCode != string.Empty)
                                            symbolData.BloombergSymbolWithExchangeCode = bloombergSymbol.Substring(0, bloombergCodePosition) + exchangeCode + bloombergSymbol.Substring(bloombergCodePosition + 2);
                                    }
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch_RT)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch_RT));
                                }
                                break;
                            case AssetCategory.Future:
                                symbolData = new FutureSymbolData();
                                symbolData.CategoryCode = category;
                                symbolData.UnderlyingSymbol = symbol;
                                if (isSnapShot)
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));

                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Fut_Multiplier)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Fut_Multiplier)).IsNull)
                                        symbolData.Multiplier = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Fut_Multiplier));

                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Fut_Expiration_Date)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Fut_Expiration_Date)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Fut_Expiration_Date)));
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch_RT)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch_RT));
                                }
                                break;
                            case AssetCategory.EquityOption:
                            case AssetCategory.Option:
                                symbolData = new OptionSymbolData();
                                symbolData.CategoryCode = category;
                                if (isSnapShot)
                                {
                                    string exchangeCode = string.Empty;
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Option_Multiplier)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Option_Multiplier)).IsNull)
                                        symbolData.Multiplier = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Option_Multiplier));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate)));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)).IsNull)
                                        underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderlyingTicker));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                    {
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                        exchangeCode = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                    }
                                    if (!string.IsNullOrEmpty(marketDataSymbolResponse.BloombergSymbol))
                                    {
                                        string bloombergSymbol = marketDataSymbolResponse.BloombergSymbol;
                                        int bloombergCodePosition = bloombergSymbol.IndexOf(' ') + 1;
                                        symbolData.BloombergSymbol = bloombergSymbol;
                                        if (exchangeCode != string.Empty)
                                            symbolData.BloombergSymbolWithExchangeCode = bloombergSymbol.Substring(0, bloombergCodePosition) + exchangeCode + bloombergSymbol.Substring(bloombergCodePosition + 2);
                                    }
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Option_Multiplier_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Option_Multiplier_RT)).IsNull)
                                        symbolData.Multiplier = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Option_Multiplier_RT));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)).IsNull)
                                        underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch_RT)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch_RT));
                                }
                                if (!string.IsNullOrWhiteSpace(underlyingSymbol))
                                {
                                    underlyingSymbol = underlyingSymbol + " Equity";
                                    MarketDataSymbolResponse marketDataSymbolResponseEquityOption = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(underlyingSymbol);

                                    if (marketDataSymbolResponseEquityOption != null)
                                        symbolData.UnderlyingSymbol = marketDataSymbolResponseEquityOption.TickerSymbol;
                                    else
                                    {
                                        marketDataSymbolResponseEquityOption = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                        {
                                            MarketDataProvider = MarketDataProvider.SAPI,
                                            CategoryCode = AssetCategory.Equity,
                                            BloombergSymbol = underlyingSymbol
                                        });

                                        if (marketDataSymbolResponseEquityOption != null)
                                            symbolData.UnderlyingSymbol = marketDataSymbolResponseEquityOption.TickerSymbol;
                                    }
                                }
                                if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                    symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                break;
                            case AssetCategory.FutureOption:
                                symbolData = new OptionSymbolData();
                                symbolData.CategoryCode = category;
                                if (isSnapShot)
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate)));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)).IsNull)
                                        underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderlyingTicker));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)).IsNull)
                                        underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch_RT)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch_RT));
                                }
                                if (!string.IsNullOrWhiteSpace(underlyingSymbol))
                                {

                                    if (marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Comdty))
                                        underlyingSymbol = underlyingSymbol + SAPIConstants.Const_Comdty;
                                    else if (marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Index))
                                        underlyingSymbol = underlyingSymbol + SAPIConstants.Const_Index;
                                    else if (marketDataSymbolResponse.BloombergSymbol.Contains(SAPIConstants.Const_Curncy))
                                        underlyingSymbol = underlyingSymbol + SAPIConstants.Const_Curncy;
                                    symbolData.UnderlyingSymbol = underlyingSymbol;
                                }
                                break;
                            case AssetCategory.FX:
                                symbolData = new FxSymbolData();
                                symbolData.CategoryCode = category;
                                symbolData.ListedExchange = "FX";
                                symbolData.UnderlyingSymbol = symbol;
                                break;
                            case AssetCategory.FXForward:
                                symbolData = new FxForwardContractSymbolData();
                                symbolData.CategoryCode = category;
                                string[] symbolPartFXForward = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (symbolPartFXForward.Length > 1)
                                    symbolData.UnderlyingSymbol = symbolPartFXForward[0];
                                else
                                {
                                    if (isSnapShot)
                                    {
                                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderlyingTicker)).IsNull)
                                            underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderlyingTicker));
                                    }
                                    else
                                    {
                                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT)).IsNull)
                                            underlyingSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_UnderLyingTicker_RT));
                                    }
                                    symbolData.UnderlyingSymbol = underlyingSymbol;
                                }
                                symbolData.ListedExchange = "FX";
                                break;
                            case AssetCategory.FixedIncome:
                                symbolData = new FixedIncomeSymbolData();
                                symbolData.CategoryCode = category;
                                symbolData.UnderlyingSymbol = symbol;
                                symbolData.ListedExchange = "OTC";
                                if (isSnapShot)
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate)));
                                }
                                break;
                            default:
                                symbolData = new EquitySymbolData();
                                symbolData.CategoryCode = category;
                                symbolData.UnderlyingSymbol = symbol;
                                if (isSnapShot)
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate)).IsNull)
                                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate)));
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch));
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Exch_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Exch_RT)).IsNull)
                                        symbolData.ListedExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Exch_RT));
                                }
                                break;
                        }
                        symbolData.AUECID = marketDataSymbolResponse.AUECID;
                    }
                    else
                        symbolData = new EquitySymbolData();
                }
                symbolData.Symbol = symbol;

                if (symbolData.CategoryCode == AssetCategory.FixedIncome)
                {
                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ID_CUSIP)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ID_CUSIP)).IsNull)
                        symbolData.Symbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ID_CUSIP));
                    symbolData.UnderlyingSymbol = symbolData.Symbol;
                    marketDataSymbolResponse.TickerSymbol = symbolData.Symbol;
                    MarketDataAdapterExtension.AddMarketDataForTickerSymbolToCache(symbolData.Symbol, marketDataSymbolResponse);
                }

                if (marketDataSymbolResponse != null && !string.IsNullOrEmpty(marketDataSymbolResponse.BloombergSymbol))
                {
                    if (string.IsNullOrEmpty(symbolData.BloombergSymbol))
                        symbolData.BloombergSymbol = marketDataSymbolResponse.BloombergSymbol;
                    else if (symbolData.BloombergSymbol != marketDataSymbolResponse.BloombergSymbol)
                        symbolData.BloombergSymbolWithExchangeCode = marketDataSymbolResponse.BloombergSymbol;
                    if (string.IsNullOrEmpty(symbolData.BloombergSymbolWithExchangeCode))
                        symbolData.BloombergSymbolWithExchangeCode = symbolData.BloombergSymbol;
                }

                symbolData.MarketDataProvider = MarketDataProvider.SAPI;

                if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VWAP)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VWAP)).IsNull)
                    symbolData.VWAP = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_VWAP));

                if (isSnapShot)
                {
                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ID_CUSIP)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ID_CUSIP)).IsNull)
                        symbolData.CusipNo = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ID_CUSIP));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ID_ISIN)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ID_ISIN)).IsNull)
                        symbolData.ISIN = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ID_ISIN));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D)).IsNull)
                        symbolData.Volume10DAvg = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D)).IsNull)
                        symbolData.AverageVolume20Day = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LAST_PRICE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LAST_PRICE)).IsNull)
                        symbolData.LastPrice = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LAST_PRICE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BidExch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BidExch)).IsNull)
                        symbolData.BidExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_BidExch));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_AskExch)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_AskExch)).IsNull)
                        symbolData.AskExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_AskExch));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_StrikePrice)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_StrikePrice)).IsNull)
                        symbolData.StrikePrice = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_StrikePrice));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OpenInterest)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OpenInterest)).IsNull)
                        symbolData.OpenInterest = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_OpenInterest));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Beta)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Beta)).IsNull)
                        symbolData.Beta_5yrMonthly = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Beta));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Delta)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Delta)).IsNull)
                        symbolData.Delta = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Delta));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Currency)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Currency)).IsNull)
                        symbolData.CurencyCode = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Currency));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LAST_UPDATE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LAST_UPDATE)).IsNull)
                        symbolData.UpdateTime = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_LAST_UPDATE)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM)).IsNull)
                    {
                        bool isDelayedStream = fieldData.GetElementAsBool(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM));

                        symbolData.PricingStatus = isDelayedStream ? PricingStatus.Delayed : PricingStatus.RealTime;
                    }

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY)).IsNull)
                        symbolData.DelayInterval = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME)).IsNull)
                        symbolData.TotalVolume = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_VOLUME));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME)).IsNull)
                        symbolData.TradeVolume = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_VOLUME));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_COUNTRY_ISO)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_COUNTRY_ISO)).IsNull)
                        symbolData.CountryID = MarketDataAdapterExtension.GetCountryIdFromBloombergCode(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_COUNTRY_ISO)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BID)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BID)).IsNull)
                        symbolData.Bid = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_BID));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BID_SIZE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BID_SIZE)).IsNull)
                        symbolData.BidSize = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_BID_SIZE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP)).IsNull)
                        symbolData.MarketCapitalization = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ASK)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ASK)).IsNull)
                        symbolData.Ask = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_ASK));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ASK_SIZE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ASK_SIZE)).IsNull)
                        symbolData.AskSize = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_ASK_SIZE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_HIGH)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_HIGH)).IsNull)
                        symbolData.High = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_HIGH));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LOW)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LOW)).IsNull)
                        symbolData.Low = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LOW));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_HIGH_52WEEK)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_HIGH_52WEEK)).IsNull)
                        symbolData.High52W = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_HIGH_52WEEK));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LOW_52WEEK)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LOW_52WEEK)).IsNull)
                        symbolData.Low52W = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LOW_52WEEK));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OPEN_INTEREST)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OPEN_INTEREST)).IsNull)
                        symbolData.OpenInterest = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_OPEN_INTEREST));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING)).IsNull)
                        symbolData.SharesOutstanding = Convert.ToInt64(Convert.ToDecimal(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING))) * 1000000);

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE)).IsNull)
                        symbolData.Previous = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LastTick)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LastTick)).IsNull)
                        symbolData.LastTick = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_LastTick));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_RoundLot)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_RoundLot)).IsNull)
                        symbolData.Multiplier = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_RoundLot));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Change)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Change)).IsNull)
                        symbolData.Change = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Change));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Sedol)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Sedol)).IsNull)
                        symbolData.SedolSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Sedol));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OSIOptSym)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OSIOptSym)).IsNull)
                        symbolData.OSIOptionSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_OSIOptSym));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Open)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Open)).IsNull)
                        symbolData.Open = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Open));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DivYield)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DivYield)).IsNull)
                        symbolData.DividendYield = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_DivYield));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DividendGross)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DividendGross)).IsNull)
                        symbolData.Dividend = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_DividendGross));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_AnnualDividend)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_AnnualDividend)).IsNull)
                        symbolData.AnnualDividend = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_AnnualDividend));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DividendExDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DividendExDate)).IsNull)
                        symbolData.XDividendDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_DividendExDate)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CompanyName)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CompanyName)).IsNull)
                        symbolData.FullCompanyName = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_CompanyName));

                    if (marketDataSymbolResponse != null && (marketDataSymbolResponse.AssetCategory == AssetCategory.EquityOption || marketDataSymbolResponse.AssetCategory == AssetCategory.Option || marketDataSymbolResponse.AssetCategory == AssetCategory.FutureOption))
                    {
                        string optionType = string.Empty;
                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OptionType)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OptionType)).IsNull)
                            optionType = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_OptionType));
                        if (optionType == SAPIConstants.Const_Put)
                            symbolData.PutOrCall = OptionType.PUT;
                        else if (optionType == SAPIConstants.Const_Call)
                            symbolData.PutOrCall = OptionType.CALL;
                    }

                    if (marketDataSymbolResponse != null && (marketDataSymbolResponse.AssetCategory == AssetCategory.FixedIncome))
                    {
                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_MaturityDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_MaturityDate)).IsNull)
                            symbolData.MaturityDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_MaturityDate)));

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_IssueDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_IssueDate)).IsNull)
                            symbolData.IssueDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_IssueDate)));

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_AccuralBasis)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_AccuralBasis)).IsNull)
                        {
                            symbolData.AccrualBasis = GetAccuralBasis(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_AccuralBasis)));
                            symbolData.AccrualBasisID = (int)symbolData.AccrualBasis;
                        }

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CouponFrequency)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CouponFrequency)).IsNull)
                        {
                            symbolData.Frequency = GetCouponFrequency(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_CouponFrequency)));
                            symbolData.CouponFrequencyID = (int)symbolData.Frequency;
                        }

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Coupon)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Coupon)).IsNull)
                            symbolData.Coupon = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Coupon));

                        // Set the IsZero field to true if Coupon is zero, otherwise set it to false
                        symbolData.IsZero = (symbolData.Coupon == 0.0);

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BondType)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BondType)).IsNull)
                        {
                            symbolData.BondType = GetBondType(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_BondType)));
                            symbolData.BondTypeID = (int)symbolData.BondType;
                        }

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_FirstCouponDate)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_FirstCouponDate)).IsNull)
                            symbolData.FirstCouponDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_FirstCouponDate)));
                    }
                }
                else
                {
                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ID_CUSIP_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ID_CUSIP_RT)).IsNull)
                        symbolData.CusipNo = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ID_CUSIP_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ID_ISIN_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ID_ISIN_RT)).IsNull)
                        symbolData.ISIN = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ID_ISIN_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D_RT)).IsNull)
                        symbolData.Volume10DAvg = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_VOLUME_AVG_10D_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D_RT)).IsNull)
                        symbolData.AverageVolume20Day = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_VOLUME_AVG_20D_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LAST_PRICE_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LAST_PRICE_RT)).IsNull)
                        symbolData.LastPrice = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LAST_PRICE_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BID_EXCHANGE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BID_EXCHANGE)).IsNull)
                        symbolData.BidExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_BID_EXCHANGE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ASK_EXCHANGE)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ASK_EXCHANGE)).IsNull)
                        symbolData.AskExchange = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ASK_EXCHANGE));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ExpDate_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ExpDate_RT)).IsNull)
                        symbolData.ExpirationDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_ExpDate_RT)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_StrikePrice_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_StrikePrice_RT)).IsNull)
                        symbolData.StrikePrice = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_StrikePrice_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OpenInterest_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OpenInterest_RT)).IsNull)
                        symbolData.OpenInterest = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_OpenInterest_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Currency_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Currency_RT)).IsNull)
                        symbolData.CurencyCode = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Currency_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LastUpdateTime)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LastUpdateTime)).IsNull)
                        symbolData.UpdateTime = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_LastUpdateTime)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM)).IsNull)
                    {
                        bool isDelayedStream = fieldData.GetElementAsBool(Name.GetName(SAPIConstants.Const_IS_DELAYED_STREAM));

                        symbolData.PricingStatus = isDelayedStream ? PricingStatus.Delayed : PricingStatus.RealTime;
                    }

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY)).IsNull)
                        symbolData.DelayInterval = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_EXCHANGE_DELAY));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_RT)).IsNull)
                        symbolData.TotalVolume = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_VOLUME_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_VOLUME_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_VOLUME_RT)).IsNull)
                        symbolData.TradeVolume = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_VOLUME_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Country_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Country_RT)).IsNull)
                        symbolData.CountryID = MarketDataAdapterExtension.GetCountryIdFromBloombergCode(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Country_RT)));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BID_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BID_RT)).IsNull)
                        symbolData.Bid = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_BID_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_BID_SIZE_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_BID_SIZE_RT)).IsNull)
                        symbolData.BidSize = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_BID_SIZE_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP)).IsNull)
                        symbolData.MarketCapitalization = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_CUR_MKT_CAP));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ASK_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ASK_RT)).IsNull)
                        symbolData.Ask = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_ASK_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_ASK_SIZE_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_ASK_SIZE_RT)).IsNull)
                        symbolData.AskSize = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_ASK_SIZE_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_HIGH_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_HIGH_RT)).IsNull)
                        symbolData.High = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_HIGH_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LOW_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LOW_RT)).IsNull)
                        symbolData.Low = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LOW_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_HIGH_52WEEK_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_HIGH_52WEEK_RT)).IsNull)
                        symbolData.High52W = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_HIGH_52WEEK_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LOW_52WEEK_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LOW_52WEEK_RT)).IsNull)
                        symbolData.Low52W = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_LOW_52WEEK_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_RT_OPEN_INTEREST)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_RT_OPEN_INTEREST)).IsNull)
                        symbolData.OpenInterest = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_RT_OPEN_INTEREST));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING_RT)).IsNull)
                        symbolData.SharesOutstanding = Convert.ToInt64(Convert.ToDecimal(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_CURRENT_SHARES_OUTSTANDING_RT))) * 1000000);

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE_REALTIME)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE_REALTIME)).IsNull)
                        symbolData.Previous = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_PREV_CLOSE_VALUE_REALTIME));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_LastTick_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_LastTick_RT)).IsNull)
                        symbolData.LastTick = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_LastTick_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_RoundLot_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_RoundLot_RT)).IsNull)
                        symbolData.Multiplier = fieldData.GetElementAsInt64(Name.GetName(SAPIConstants.Const_RoundLot_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Change)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Change)).IsNull)
                        symbolData.Change = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Change));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Sedol)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Sedol)).IsNull)
                        symbolData.SedolSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_Sedol));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OSIOptSym)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OSIOptSym)).IsNull)
                        symbolData.OSIOptionSymbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_OSIOptSym));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Open)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Open)).IsNull)
                        symbolData.Open = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Open));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DivYield_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DivYield_RT)).IsNull)
                        symbolData.DividendYield = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_DivYield_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DividendGross_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DividendGross_RT)).IsNull)
                        symbolData.Dividend = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_DividendGross_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_AnnualDividend_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_AnnualDividend_RT)).IsNull)
                        symbolData.AnnualDividend = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_AnnualDividend_RT));

                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_DividendExDate_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_DividendExDate_RT)).IsNull)
                        symbolData.XDividendDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_DividendExDate_RT)));

                    if (marketDataSymbolResponse != null && (marketDataSymbolResponse.AssetCategory == AssetCategory.EquityOption || marketDataSymbolResponse.AssetCategory == AssetCategory.Option || marketDataSymbolResponse.AssetCategory == AssetCategory.FutureOption))
                    {
                        string optionType = string.Empty;
                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_OptionType_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_OptionType_RT)).IsNull)
                            optionType = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_OptionType_RT));
                        if (optionType == SAPIConstants.Const_Put)
                            symbolData.PutOrCall = OptionType.PUT;
                        else if (optionType == SAPIConstants.Const_Call)
                            symbolData.PutOrCall = OptionType.CALL;
                    }

                    if (marketDataSymbolResponse != null && (marketDataSymbolResponse.AssetCategory == AssetCategory.FixedIncome))
                    {
                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_MaturityDate_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_MaturityDate_RT)).IsNull)
                            symbolData.MaturityDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_MaturityDate_RT)));

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_IssueDate_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_IssueDate_RT)).IsNull)
                            symbolData.IssueDate = DateTime.Parse(fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_IssueDate_RT)));

                        if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_Coupon_RT)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_Coupon_RT)).IsNull)
                            symbolData.Coupon = fieldData.GetElementAsFloat64(Name.GetName(SAPIConstants.Const_Coupon_RT));

                        // Set the IsZero field to true if Coupon is zero, otherwise set it to false
                        symbolData.IsZero = (symbolData.Coupon == 0.0);
                    }
                }

                bool isOnlyTime = symbolData.UpdateTime.Date == DateTime.MinValue.Date;
                if (isOnlyTime && marketDataSymbolResponse != null)
                {
                    string time = symbolData.UpdateTime.ToString();
                    DateTime currentDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(marketDataSymbolResponse.AUECID));
                    symbolData.UpdateTime = (currentDate.Date + TimeSpan.ParseExact(time, "hhmmssfff", CultureInfo.InvariantCulture)).ToUniversalTime();
                }
                MarketDataAdapterExtension.AddToSnapShotSymbolDataCollection(ref symbolData, MarketDataProvider.SAPI);

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
            return symbolData;
        }

        private string GetToken(EventQueue tokenEventQueue)
        {
            string token = string.Empty;
            try
            {
                Event eventObj = tokenEventQueue.NextEvent();
                if (eventObj.Type == Event.EventType.TOKEN_STATUS)
                {
                    foreach (Message msg in eventObj)
                    {
                        if (msg.MessageType == CONST_TokenSuccess)
                        {
                            token = msg.GetElementAsString(Name.GetName(SAPIConstants.Const_Token));
                            LogAndDisplayOnInformationReporter.GetInstance.Write("Token generation success: " + token, LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);

                        }
                        else if (msg.MessageType == CONST_TokenFailure)
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(msg.ToString(), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                            break;
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
            return token;
        }

        private void ProcessSubscriptionStatus(Event eventObj)
        {
            try
            {
                foreach (Bloomberglp.Blpapi.Message msg in eventObj)
                {
                    CorrelationID cid = msg.CorrelationID;
                    string topic = (string)cid.Object;

                    if (msg.MessageType.Equals(Names.SubscriptionFailure))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter($"Subscription for {topic} failed.", LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                    }
                    else if (msg.MessageType.Equals(Names.SubscriptionTerminated))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter($"Subscription for {topic} terminated", LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
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

        private void ProcessSnapShotDataEvent(Event eventObj)
        {
            try
            {
                foreach (Bloomberglp.Blpapi.Message msg in eventObj)
                {
                    Element ReferenceDataResponse = msg.AsElement;
                    if (!ReferenceDataResponse.HasElement(Name.GetName(SAPIConstants.Const_ResponseError)))
                    {
                        Element securityDataArray = ReferenceDataResponse.GetElement(Name.GetName(SAPIConstants.Const_SecurityData));
                        for (int i = 0; i < securityDataArray.NumValues; i++)
                        {
                            Element securityData = securityDataArray.GetValueAsElement(i);
                            int sequenceNumber =
                            securityData.GetElementAsInt32(Name.GetName(SAPIConstants.Const_SequenceNumber));
                            if (securityData.HasElement(Name.GetName(SAPIConstants.Const_SecurityError)))
                            {
                                string err = securityData.GetElement(Name.GetName(SAPIConstants.Const_SecurityError)).GetElementAsString(Name.GetName(SAPIConstants.Const_Msg));
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(err, LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                                return;
                            }
                            else
                            {
                                string symbol = string.Empty;
                                string security = string.Empty;
                                Element fieldData = securityData.GetElement(Name.GetName(SAPIConstants.Const_FieldData));

                                if (securityData.HasElement(Name.GetName(SAPIConstants.Const_Security)))
                                    security = securityData.GetElementAsString(Name.GetName(SAPIConstants.Const_Security));
                                if (security != string.Empty)
                                {
                                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketDataSymbolUsingCache(security);
                                    if (marketDataSymbolResponse != null)
                                        symbol = marketDataSymbolResponse.TickerSymbol;
                                    else
                                        symbol = security;
                                }
                                else
                                {
                                    if (fieldData.HasElement(Name.GetName(SAPIConstants.Const_TICKER)) && !fieldData.GetElement(Name.GetName(SAPIConstants.Const_TICKER)).IsNull)
                                        symbol = fieldData.GetElementAsString(Name.GetName(SAPIConstants.Const_TICKER));
                                }

                                SymbolData snapShotData = FillSymbolData(fieldData, symbol, true);
                                bool entitlementcheck = PerformSnapshotDataEntitlementCheck(securityData, symbol);
                                if (entitlementcheck)
                                {
                                    Data obj = new Data();
                                    obj.Info = snapShotData;
                                    if (SnapShotDataResponse != null)
                                        SnapShotDataResponse(this, obj);
                                }
                            }
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

        private void ProcessSubscriptionDataEvent(Event eventObj)
        {
            try
            {
                foreach (Bloomberglp.Blpapi.Message msg in eventObj)
                {
                    string topic = (string)msg.CorrelationID.Object;
                    Element MarketDataResponse = msg.AsElement;
                    if (!MarketDataResponse.HasElement(Name.GetName(SAPIConstants.Const_ResponseError)))
                    {
                        string symbol = topic;
                        Element fieldData = MarketDataResponse;
                        SymbolData symbolData = FillSymbolData(fieldData, symbol, false);
                        bool entitlementCheck = PerformSubscriptionDataEntitlementCheck(fieldData, symbol);
                        if (entitlementCheck)
                        {
                            Data obj = new Data();
                            obj.Info = symbolData;
                            if (ContinuousDataResponse != null)
                                ContinuousDataResponse(this, obj);
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

        private void ProcessAuthorizationDataEvent(Event eventObj)
        {
            try
            {
                lock (_dictAuthenticateUsernameIPAddress)
                {
                    foreach (Message msg in eventObj.GetMessages())
                    {
                        string topic = string.Empty;
                        string username = string.Empty;
                        string ipAddress = string.Empty;
                        bool userPermission = false;
                        bool isSamsaraAuthorizationRequest = false;

                        if (msg?.CorrelationID?.IsObject == true && msg.CorrelationID.Object is string correlationIdStr && !string.IsNullOrWhiteSpace(correlationIdStr))
                        {
                            topic = correlationIdStr;
                            var lstParameters = topic.Split(',').Select(p => p.Trim()).ToList();

                            if (lstParameters.Count >= 2)
                            {
                                if (lstParameters[1].Equals("$" + SAPIConstants.SAMSARA + "$", StringComparison.OrdinalIgnoreCase))
                                {
                                    isSamsaraAuthorizationRequest = true;
                                }
                                else
                                {
                                    username = lstParameters[1];
                                    if (lstParameters.Count >= 3)
                                    {
                                        ipAddress = lstParameters[2];
                                    }
                                }
                            }
                        }
                        HandleUserIdentityCreation(msg, topic, isSamsaraAuthorizationRequest);
                        //Authorization Handling for enterprise
                        if (msg.MessageType == CONST_AuthorizationSuccess)
                        {
                            if (_dictAuthenticateUsernameIPAddress.ContainsKey(username))
                                _dictAuthenticateUsernameIPAddress[username] = new List<string>();
                            userPermission = true;
                            //_authenticateUserSession.Cancel(new CorrelationID(topic));
                        }
                        else if (msg.MessageType == CONST_AuthorizationFailure || msg.MessageType == CONST_AuthorizationRevoked)
                        {
                            int userID = Int32.Parse(topic.Split(',').ToList()[0]);
                            if (_dictUserIdentityMapping.ContainsKey(userID))
                            {
                                string authenticatedUserTopic = _dictUserIdentityMapping[userID].Topic;
                                if (authenticatedUserTopic.Equals(topic, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (isSamsaraAuthorizationRequest)
                                    {
                                        HandleSamsaraSAPIEvent(userID, false);
                                    }
                                    RemoveUserFromIdentityMapping(userID);
                                }
                            }

                            if (_dictAuthenticateUsernameIPAddress.ContainsKey(username)
                                && _dictAuthenticateUsernameIPAddress[username].Contains(ipAddress))
                                _dictAuthenticateUsernameIPAddress[username].Remove(ipAddress);
                        }
                        if (_dictAuthenticateUsernameIPAddress.ContainsKey(username)
                           && _dictAuthenticateUsernameIPAddress[username].Count == 0)
                        {
                            _dictAuthenticateUsernameIPAddress.Remove(username);
                            if (AuthenticateUserResponse != null)
                                AuthenticateUserResponse(this, new EventArgs<string, bool>(username, userPermission));
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

        private CouponFrequency GetCouponFrequency(string bloombergFreq)
        {
            CouponFrequency frequency = new CouponFrequency();
            try
            {
                // Converts a Bloomberg coupon frequency value to an internal CouponFrequency enum.
                if (Enum.TryParse(bloombergFreq, true, out BloombergCouponFrequency result))
                {
                    switch (result)
                    {
                        case BloombergCouponFrequency.Annual:
                            frequency = CouponFrequency.Annually;
                            break;
                        case BloombergCouponFrequency.SemiAnnual:
                            frequency = CouponFrequency.SemiAnnually;
                            break;
                        case BloombergCouponFrequency.Quarterly:
                            frequency = CouponFrequency.Quarterly;
                            break;
                        case BloombergCouponFrequency.Monthly:
                            frequency = CouponFrequency.Monthly;
                            break;
                        case BloombergCouponFrequency.NoCoupon:
                            frequency = CouponFrequency.None;
                            break;
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
            return frequency;
        }

        private AccrualBasis GetAccuralBasis(string bloombergAccuralBasis)
        {
            AccrualBasis accrualBasis = new AccrualBasis();
            try
            {
                // Converts a Bloomberg accrual basis value to an internal AccrualBasis enum.
                foreach (BloombergAccrualBasis enumVal in Enum.GetValues(typeof(BloombergAccrualBasis)))
                {
                    var field = enumVal.GetType().GetField(enumVal.ToString());
                    var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                    if (bloombergAccuralBasis.Contains(attribute.Description))
                    {
                        switch (enumVal)
                        {
                            case BloombergAccrualBasis.Actual365:
                                accrualBasis = AccrualBasis.Actual_365;
                                break;
                            case BloombergAccrualBasis.Actual360:
                                accrualBasis = AccrualBasis.Actual_360;
                                break;
                            case BloombergAccrualBasis.Thirty360:
                                accrualBasis = AccrualBasis.Accrual_30_360;
                                break;
                            case BloombergAccrualBasis.ThirtyE360:
                                accrualBasis = AccrualBasis.Accrual_30E_360;
                                break;
                            case BloombergAccrualBasis.ActualActual:
                                accrualBasis = AccrualBasis.Actual_Actual;
                                break;
                        }
                        break;
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
            return accrualBasis;
        }

        private BusinessObjects.AppConstants.SecurityType GetBondType(string bloombergBondType)
        {
            BusinessObjects.AppConstants.SecurityType bondType = new BusinessObjects.AppConstants.SecurityType();
            try
            {
                // Converts a Bloomberg security type value to an internal SecurityType enum.
                if (Enum.TryParse(bloombergBondType, true, out BloombergSecurityType result))
                {
                    switch (result)
                    {
                        case BloombergSecurityType.CORP:
                            bondType = BusinessObjects.AppConstants.SecurityType.Corporate;
                            break;
                        case BloombergSecurityType.GOVT:
                            bondType = BusinessObjects.AppConstants.SecurityType.Treasury;
                            break;
                        case BloombergSecurityType.MUNI:
                            bondType = BusinessObjects.AppConstants.SecurityType.Municipal;
                            break;
                        case BloombergSecurityType.SOV:
                            bondType = BusinessObjects.AppConstants.SecurityType.Sovereign;
                            break;
                        case BloombergSecurityType.AGY:
                            bondType = BusinessObjects.AppConstants.SecurityType.Agency;
                            break;
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
            return bondType;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sends user Authentication request to SAPI.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ipAddresses"></param>
        public void SendAuthenticateUserRequest(string username, string ipAddresses, int userId)
        {
            try
            {
                lock (_dictAuthenticateUsernameIPAddress)
                {
                    bool isServiceOpen = false;
                    if (_isAuthenticateUserSessionStarted)
                        isServiceOpen = _authenticateUserSession.OpenService(SAPIConstants.Const_ApiAuth);
                    if (isServiceOpen)
                    {
                        List<string> lstIpAdresses = ipAddresses.Split(',').ToList();
                        if (_dictAuthenticateUsernameIPAddress.ContainsKey(username))
                            _dictAuthenticateUsernameIPAddress[username] = lstIpAdresses;
                        else
                            _dictAuthenticateUsernameIPAddress.Add(username, lstIpAdresses);

                        _authService = _authenticateUserSession.GetService(SAPIConstants.Const_ApiAuth);

                        List<string> lstIpAdressesToBeRemoved = new List<string>();

                        foreach (string ipAddress in lstIpAdresses)
                        {
                            string correlationIDstr = userId + "," + username + "," + ipAddress + "," + Guid.NewGuid();
                            Identity userIdentity = _authenticateUserSession.CreateIdentity();
                            CorrelationID correlationID = new CorrelationID(correlationIDstr);

                            EventQueue tokenEventQueue = new EventQueue();
                            _authenticateUserSession.GenerateToken(username, ipAddress, new CorrelationID(tokenEventQueue), tokenEventQueue);
                            string token = GetToken(tokenEventQueue);
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                Request authRequest = _authService.CreateAuthorizationRequest();
                                authRequest.Set(Name.GetName(SAPIConstants.Const_Token), token);
                                _authenticateUserSession.SendAuthorizationRequest(authRequest, userIdentity, correlationID);
                                _authHitCounter += 1;
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Authentication Request User: {0} , Time: {1}, Bloomberg User Authentication Request Counter: {2}", username, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt"), _authHitCounter.ToString()), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                            }
                            else
                            {
                                lstIpAdressesToBeRemoved.Add(ipAddress);
                            }
                        }

                        if (_dictAuthenticateUsernameIPAddress.ContainsKey(username))
                        {
                            _dictAuthenticateUsernameIPAddress[username].RemoveAll(item => lstIpAdressesToBeRemoved.Contains(item));
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

        /// <summary>
        /// Resubscribes the symbols after updating subscription fields from PUI.
        /// </summary>
        public void ResubscribeSymbols()
        {
            try
            {
                if (_lstSubscribedSymbol != null)
                {
                    List<string> subscribedSymbolList = DeepCopyHelper.Clone<List<string>>(_lstSubscribedSymbol);
                    foreach (string symbol in subscribedSymbolList)
                    {
                        DeleteSymbol(symbol);
                        if (_isSessionStarted)
                        {
                            string options = GetSubscriptionOptions();
                            MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse();
                            if (_dictSymbolDataInfo.ContainsKey(symbol))
                                marketDataSymbolResponse = _dictSymbolDataInfo[symbol];
                            if (marketDataSymbolResponse != null && marketDataSymbolResponse.BloombergSymbol != string.Empty)
                            {
                                string fields = GetAssetWiseFieldToRequest(marketDataSymbolResponse.AssetCategory, SAPIConstants.Const_Subscription);
                                List<Subscription> subscriptions = new List<Subscription>();
                                subscriptions.Add(new Subscription(marketDataSymbolResponse.BloombergSymbol, fields, options, new CorrelationID(symbol)));
                                _session.Subscribe(subscriptions);
                                _lstSubscribedSymbol.Add(symbol);
                                _hitCounter += (fields.Count(c => c == ',') + 1);
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Re-Subscription request symbol: {0}, Bloomberg Symbol: {1}, Request Sent for Following Fields: {2}, Time: {3}, Bloomberg Request Counter: {4} ", symbol, marketDataSymbolResponse.BloombergSymbol, fields, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt"), _hitCounter.ToString()), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                            }
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
        #endregion

        #region Credential Management
        /// <summary>
        /// Loads credentials.
        /// </summary>
        private void LoadCredentials()
        {
            try
            {
                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPICredentialDetails));

                if (File.Exists(credentialsFilePath))
                {
                    string encryptedProperties = File.ReadAllText(credentialsFilePath, Encoding.UTF8);
                    string decryptedProperties = TripleDESEncryptDecrypt.TripleDESDecryption(encryptedProperties, encryptionKey);
                    if (!string.IsNullOrWhiteSpace(decryptedProperties))
                    {
                        string[] decryptedPropertiesValue = decryptedProperties.Split(Seperators.SEPERATOR_6);

                        _clientServerAddress = decryptedPropertiesValue[0];
                        _clientConnectionPassword = decryptedPropertiesValue[1];
                        _sapiPort = decryptedPropertiesValue[2];
                    }
                }
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
        }

        /// <summary>
        /// To get credentials.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCredentials()
        {
            return new List<string>()
            {
                _clientServerAddress,
                _clientConnectionPassword,
                _sapiPort
            };
        }

        public bool UpdateCredentials(string serverAddress, string clientPassword, string port)
        {
            try
            {
                _clientServerAddress = serverAddress;
                _clientConnectionPassword = clientPassword;
                _sapiPort = port;

                string credentials = serverAddress + Seperators.SEPERATOR_6 + clientPassword + Seperators.SEPERATOR_6 + port + Seperators.SEPERATOR_6;

                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPICredentialDetails));

                if (!File.Exists(credentialsFilePath))
                {
                    if (string.IsNullOrWhiteSpace(credentialsFilePath))
                        throw new Exception("SAPI credentials storage path is not valid. Please contact administrator.");
                    else if (!Directory.Exists(credentialsFilePath))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(credentialsFilePath));
                    }
                }
                File.WriteAllText(credentialsFilePath, TripleDESEncryptDecrypt.TripleDESEncryption(credentials, encryptionKey));
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessException ex = new UnauthorizedAccessException("Access to SAPI credentials storage is denied. Please contact administrator.");
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw ex;
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
        #endregion

        #region ILiveFeedAdapter Methods
        /// <summary>
        /// SAPIManager.Connect
        /// </summary>
        public void Connect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    if (_isSessionStarted)
                    {
                        MarketDataAdapterExtension.ClearCache();
                        Disconnect();
                    }
                    _sessionOptions = CreateSessionOptions(false);
                    if (_sessionOptions != null)
                    {
                        _session = new Session(_sessionOptions, ProcessEvent);
                        _isSessionStarted = _session.Start();
                    }

                    if (_isSessionStarted)
                    {
                        if (Connected != null)
                        {
                            Connected(this, (new EventArgs<bool>(true)));
                        }
                    }
                    else
                    {
                        if (Disconnected != null)
                        {
                            Disconnected(this, (new EventArgs<bool>(false)));
                        }
                    }
                    CreateSessionUserAuthentication();
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

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    if ((_session != null) && _isSessionStarted)
                    {
                        if (_lstSubscribedSymbol.Count > 0)
                        {
                            foreach (string symbol in _lstSubscribedSymbol)
                                _session.Cancel(new CorrelationID(symbol));
                            _lstSubscribedSymbol.Clear();
                            _dictSymbolDataInfo.Clear();
                        }
                        _session.Stop();
                        _isSessionStarted = false;
                        _authenticateUserSession.Stop();
                        _isAuthenticateUserSessionStarted = false;
                    }
                    LogAndDisplayOnInformationReporter.GetInstance.Write("Disconnected from SAPI", LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
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
        /// This method calls for the subscription of the symbols
        /// </summary>
        /// <param name="symbol"></param>
        public void GetContinuousData(string symbol)
        {
            try
            {
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: SAPIManager.GetContinuousData() entered: {0}, Time: {1}", symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));
                if (!_configEnableMarketDataSimulationForAutomation && !_lstSubscribedSymbol.Contains(symbol))
                {
                    string options = GetSubscriptionOptions();
                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);
                    bool isServiceOpen = false;
                    if (_isSessionStarted)
                        isServiceOpen = _session.OpenService(SAPIConstants.Const_Mktdata);

                    if (marketDataSymbolResponse != null)
                    {
                        string fields = GetAssetWiseFieldToRequest(marketDataSymbolResponse.AssetCategory, SAPIConstants.Const_Subscription);
                        if (isServiceOpen && !string.IsNullOrEmpty(marketDataSymbolResponse.BloombergSymbol))
                        {
                            if (_dictSymbolDataInfo.ContainsKey(symbol))
                                _dictSymbolDataInfo[symbol] = marketDataSymbolResponse;
                            else
                                _dictSymbolDataInfo.Add(symbol, marketDataSymbolResponse);
                            List<Subscription> subscriptions = new List<Subscription>();
                            subscriptions.Add(new Subscription(marketDataSymbolResponse.BloombergSymbol, fields, options, new CorrelationID(symbol)));
                            _session.Subscribe(subscriptions);
                            _lstSubscribedSymbol.Add(symbol);
                            _hitCounter += (fields.Count(c => c == ',') + 1);
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Subscription request symbol: {0}, Bloomberg Symbol: {1}, Request Sent for Following Fields: {2}, Time: {3}, Bloomberg Request Counter: {4} ", symbol, marketDataSymbolResponse.BloombergSymbol, fields, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt"), _hitCounter.ToString()), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
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
            return;
        }

        /// <summary>
        /// Get Snapshot Data
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="completeInfo"></param>
        public void GetSnapShotData(string symbol, Prana.Global.ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {
            try
            {
                if (symbol.Contains(SAPIConstants.Const_Index) || symbol.Contains(SAPIConstants.Const_Comdty) || symbol.Contains(SAPIConstants.Const_Curncy))
                {

                    AssetCategory assetCategory = MarketDataAdapterExtension.GetAssetCategoryBloombergSymbol(symbol);
                    if (assetCategory != AssetCategory.FX && assetCategory != AssetCategory.FXForward)
                        symbologyCode = Prana.Global.ApplicationConstants.SymbologyCodes.BloombergSymbol;
                }
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: SAPIManager.GetSnapShotData() entered: {0}, Time: {1}", symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    bool haltRequest = false;
                    //if (_snapshotRequests.ContainsKey(symbol))
                    //{
                    //    Stack<DateTime> timeStack = _snapshotRequests[symbol];
                    //    if (DateTime.Now < timeStack.Peek().AddMinutes(2))
                    //    {
                    //        haltRequest = true;
                    //    }
                    //    else
                    //    {
                    //        _snapshotRequests[symbol].Push(DateTime.Now);
                    //    }
                    //}
                    //else
                    //{
                    //    Stack<DateTime> timeStack = new Stack<DateTime>();
                    //    timeStack.Push(DateTime.Now);

                    //    _snapshotRequests.Add(symbol, timeStack);
                    //}

                    if (!haltRequest)
                    {
                        string reqSymbol = string.Empty;
                        MarketDataSymbolResponse marketDataSymbolResponse = null;
                        string fieldStr = string.Empty;
                        bool isServiceOpen = false;
                        if (_isSessionStarted)
                            isServiceOpen = _session.OpenService(SAPIConstants.Const_RefData);
                        if (isServiceOpen)
                        {
                            _referenceService = _session.GetService(SAPIConstants.Const_RefData);
                            Request request = _referenceService.CreateRequest(SAPIConstants.Const_ReferenceDataRequest);
                            if (symbologyCode == ApplicationConstants.SymbologyCodes.BloombergSymbol)
                            {
                                reqSymbol = symbol;
                                AssetCategory assetCategory = MarketDataAdapterExtension.GetAssetCategoryBloombergSymbol(symbol);
                                if (assetCategory != AssetCategory.None)
                                {
                                    MarketDataSymbolResponse marketDataSymbolGenerateTickerResponse = MarketDataAdapterExtension.GetTickerSymbolFromMarketData(new SymbolData()
                                    {
                                        MarketDataProvider = MarketDataProvider.SAPI,
                                        CategoryCode = assetCategory,
                                        BloombergSymbol = symbol
                                    });
                                    if (marketDataSymbolGenerateTickerResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolGenerateTickerResponse.TickerSymbol))
                                    {
                                        marketDataSymbolResponse = marketDataSymbolGenerateTickerResponse;
                                        symbol = marketDataSymbolGenerateTickerResponse.TickerSymbol;
                                    }
                                    else
                                        marketDataSymbolResponse = new MarketDataSymbolResponse() { TickerSymbol = symbol, BloombergSymbol = symbol, AssetCategory = assetCategory };
                                }
                                else
                                    marketDataSymbolResponse = new MarketDataSymbolResponse() { TickerSymbol = symbol, BloombergSymbol = symbol, AssetCategory = assetCategory };
                                if (!_dictSymbolDataInfo.ContainsKey(symbol))
                                    _dictSymbolDataInfo.Add(symbol, marketDataSymbolResponse);
                                else
                                    _dictSymbolDataInfo[symbol] = marketDataSymbolResponse;
                                MarketDataAdapterExtension.AddMarketDataForTickerSymbolToCache(symbol, marketDataSymbolResponse);
                                fieldStr = GetAssetWiseFieldToRequest(marketDataSymbolResponse.AssetCategory, SAPIConstants.Const_Snapshot);
                            }
                            else
                            {
                                marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);
                                if (marketDataSymbolResponse != null)
                                {
                                    reqSymbol = marketDataSymbolResponse.BloombergSymbol;
                                    fieldStr = GetAssetWiseFieldToRequest(marketDataSymbolResponse.AssetCategory, SAPIConstants.Const_Snapshot);
                                    if (_dictSymbolDataInfo.ContainsKey(symbol))
                                        _dictSymbolDataInfo[symbol] = marketDataSymbolResponse;
                                    else
                                        _dictSymbolDataInfo.Add(symbol, marketDataSymbolResponse);
                                }
                            }
                            if (!string.IsNullOrEmpty(reqSymbol))
                            {
                                request.Append(Name.GetName(SAPIConstants.Const_Securities), reqSymbol);
                                List<string> fields = fieldStr.Split(',').ToList();
                                foreach (string field in fields)
                                {
                                    request.Append(Name.GetName(SAPIConstants.Const_Fields), field);
                                    _hitCounter++;
                                }
                                //Adding parameter to get Eids with snapshot request
                                request.Set(ReturnEids, true);
                                _session.SendRequest(request, null);
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Snapshot request symbol: {0}, Bloomberg Symbol: {1}, Request Sent for Following Fields: {2}, Time: {3}, Bloomberg Request Counter: {4}", symbol, reqSymbol, fieldStr, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt"), _hitCounter.ToString()), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                            }
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="underlyingSymbol"></param>
        /// <param name="month"></param>
        /// <param name="lowerstrike"></param>
        /// <param name="upperstrike"></param>
        /// <param name="turnaroundid"></param>
        /// <param name="categoryCode"></param>
        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            if (!_configEnableMarketDataSimulationForAutomation)
            {
            }
        }

        /// <summary>
        /// Delete Security
        /// </summary>
        /// <param name="symbol"></param>
        public void DeleteSymbol(string symbol)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation && _lstSubscribedSymbol.Contains(symbol))
                {
                    if (_isSessionStarted)
                        _session.Cancel(new CorrelationID(symbol));
                    _lstSubscribedSymbol.Remove(symbol);
                }
                MarketDataAdapterExtension.RemoveMarketDataSymbolInformation(symbol);
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

        public List<SymbolData> GetAvailableLiveFeed()
        {
            return MarketDataAdapterExtension.GetAvailableLiveFeed();
        }

        public Task<object> GetLiveDataDirectlyFromFeed()
        {
            return null;
        }

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("SAPI CheckIfInternationalSymbols not implemented yet", LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
            return null;
        }

        public Dictionary<string, string> GetUserInformation()
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            return new Dictionary<string, string>();
        }

        ///<inheritdoc/>
        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {

        }
        ///<inheritdoc/>
        public void UpdateSecurityDetails(BusinessObjects.SecurityMasterBusinessObjects.SecMasterbaseList secMasterList)
        {

        }

        public event EventHandler<EventArgs<bool>> Connected;

        public event EventHandler<EventArgs<bool>> Disconnected;

        public event EventHandler<Data> ContinuousDataResponse;

        public event EventHandler<Data> SnapShotDataResponse;

#pragma warning disable CS0067
        //Suppressing this warning as this event is not used in the current implementation but can't remove this event as it is part of the interface contract.
        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
#pragma warning restore CS0067
        #endregion

        #region Samsara SAPI handling

        public async void GetBloombergAuthenticationToken(string userDetails)
        {
            BloombergAccessTokenDTO bloombergAccessTokenDTO;
            try
            {
                bloombergAccessTokenDTO = JsonConvert.DeserializeObject<BloombergAccessTokenDTO>(userDetails);
                var callbackChannel = OperationContext.Current?.GetCallbackChannel<IMarketDataPermissionServiceCallback>();
                if (callbackChannel != null)
                {
                    lock (CallbackChannels)
                    {
                        CallbackChannels[bloombergAccessTokenDTO.UserId.ToString()] = callbackChannel;
                    }
                }

                var body = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(SAPIConstants.GRANT_TYPE, bloombergAccessTokenDTO.GrantType),
                    new KeyValuePair<string, string>(SAPIConstants.CODE, bloombergAccessTokenDTO.Code),
                    new KeyValuePair<string, string>(SAPIConstants.CODE_VERIFIER, bloombergAccessTokenDTO.CodeVerifier),
                    new KeyValuePair<string, string>(SAPIConstants.CLIENT_ID, bloombergAccessTokenDTO.ClientId),
                    new KeyValuePair<string, string>(SAPIConstants.REDIRECT_URI, bloombergAccessTokenDTO.RedirectUri)
                });

                HttpResponseMessage responseTask = null;
                string BloombergAuthenticationTokenURL = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_SAPISettings, CONST_SAPITokenAuthURL);
                responseTask = await client.PostAsync(BloombergAuthenticationTokenURL, body);

                var result = await responseTask.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
                if (jsonResponse != null && jsonResponse.AccessToken != null)
                {
                    //received access token from bloomberg SAPI for the user.
                    SendAuthorizationUserRequestSamsara(bloombergAccessTokenDTO.UserId, jsonResponse.AccessToken);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return;
            }
        }

        public void SendAuthorizationUserRequestSamsara(int userID, string token)
        {
            try
            {
                bool isServiceOpen = false;
                if (_isAuthenticateUserSessionStarted)
                    isServiceOpen = _authenticateUserSession.OpenService(SAPIConstants.Const_ApiAuth);
                if (isServiceOpen)
                {
                    //Correlation ID is necessary to retrieve the user Identity from bloomberg sapi.
                    string correlationIDstr = userID + ",$" + SAPIConstants.SAMSARA + "$," + Guid.NewGuid();
                    _authService = _authenticateUserSession.GetService(SAPIConstants.Const_ApiAuth);
                    Identity userIdentity = _authenticateUserSession.CreateIdentity();

                    if (_dictUserIdentityMapping.ContainsKey(userID))
                    {
                        //User is already authenticated. Sending an event to samsara frontend for authorization success.
                        HandleSamsaraSAPIEvent(userID, true);
                    }
                    else
                    {
                        CorrelationID correlationID = new CorrelationID(correlationIDstr);
                        Request authRequest = _authService.CreateAuthorizationRequest();
                        authRequest.Set(Name.GetName(SAPIConstants.Const_Token), token);
                        _authenticateUserSession.SendAuthorizationRequest(authRequest, userIdentity, correlationID);
                        _authHitCounter += 1;

                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Authentication Request User: {0} , Time: {1}, Bloomberg User Authentication Request Counter: {2}", userID, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt"), _authHitCounter.ToString()), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                    }

                }
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
        }

        public void HandleUserIdentityCreation(Message msg, string topic, bool isSamsaraAuthorizationRequest)
        {
            try
            {
                if (msg.MessageType == CONST_AuthorizationSuccess)
                {
                    //Authorization success for the user.
                    Identity userIdentity = _authenticateUserSession.GetAuthorizedIdentity(new CorrelationID(topic));
                    if (userIdentity != null)
                    {
                        int userID = Int32.Parse(topic.Split(',').ToList()[0]);
                        if (_dictUserIdentityMapping.ContainsKey(userID))
                        {
                            _dictUserIdentityMapping[userID] = new UserIdentity(topic, userIdentity);
                        }
                        else
                        {
                            _dictUserIdentityMapping.Add(userID, new UserIdentity(topic, userIdentity));
                        }
                        if (isSamsaraAuthorizationRequest)
                        {
                            //Handling for sending an event to samsara frontend for authorization success.
                            HandleSamsaraSAPIEvent(userID, true);
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

        public void HandleSamsaraSAPIEvent(int userId, bool isUserEntitled)
        {
            try
            {
                if (CallbackChannels.TryGetValue(userId.ToString(), out var callbackChannel))
                {
                    try
                    {
                        callbackChannel.PermissionCheckResponse(userId, isUserEntitled);
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
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("No callback channel found for user: {0}", userId), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
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

        public bool HandleUserEntitlementCheck(Service referencedService, string symbol, List<int> entitlements)
        {
            try
            {
                if (entitlements.Count == 0)
                {
                    //In case entitlements are empty, this means user is entitled to access the data.

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Entitlement check passed for symbol: {0} for the logged-in user(s) because no EID was received in the response.", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                    return true;
                }
                else if (_dictUserIdentityMapping.Count == 0)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("No logged-in user found. Assuming the request is from Expnl. Entitlement check passed for symbol: {0}.", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                    return true;
                }
                else
                {
                    bool result = true;
                    foreach (KeyValuePair<int, UserIdentity> user in _dictUserIdentityMapping)
                    {
                        Identity userIdentity = user.Value.Identity;
                        if (userIdentity != null && referencedService != null)
                        {
                            List<int> failedEntitlements = new List<int>();
                            if (!userIdentity.HasEntitlements(entitlements.ToArray(), referencedService))
                            {
                                result = false; // If any user fails the entitlement check, the overall result is false.

                                //User Id failed entitlement check , hence adding in the dictionary
                                AddSymbolEntryInFailedEntitlement(user.Key, symbol);
                            }
                        }
                        else
                        {
                            result = false; // If user identity is null or service is not available, entitlement check fails.
                        }
                    }
                    return result; // If all users pass the entitlement check, return true.
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
            return false;
        }

        public bool PerformSnapshotDataEntitlementCheck(Element securityData, string symbol)
        {
            bool entitlementCheck = true;
            try
            {
                if (!CheckIfFailedEntitlementContainsSymbol(symbol))
                {
                    //Creating entitlement check for the security
                    Name eidName = new Name(SAPIConstants.Const_EID);
                    List<int> eidsList = new List<int>();
                    if (securityData.HasElement(eidName) && !securityData.GetElement(eidName).IsNull)
                    {
                        bool isArray = securityData.GetElement(eidName).IsArray;
                        //fetch eid values from this array 
                        if (isArray)
                        {
                            Element eidArray = securityData.GetElement(eidName);
                            for (int j = 0; j < eidArray.NumValues; j++)
                            {
                                int eidValue = eidArray.GetValueAsInt32(j);
                                eidsList.Add(eidValue);
                            }
                        }
                        else
                        {
                            string eid = securityData.GetElementAsString(eidName);
                            if (!string.IsNullOrEmpty(eid))
                            {
                                if (int.TryParse(eid, out int eidValue))
                                {
                                    eidsList.Add(eidValue); // Add the single eid to the list
                                }
                            }
                        }

                        bool isServiceOpen = false;
                        if (_session!=null && _isSessionStarted)
                            isServiceOpen = _session.OpenService(SAPIConstants.Const_RefData);
                        if (isServiceOpen)
                        {
                            Service snapshotServiceReference = _session.GetService(SAPIConstants.Const_RefData);
                            entitlementCheck = HandleUserEntitlementCheck(snapshotServiceReference, symbol, eidsList);
                            if (!entitlementCheck)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Snapshot entitlement check failed for symbol: {0} for the logged-in user(s).", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
                            }
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("RefData service closed for symbol: {0}.", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
                        }
                    }
                }
                else
                {
                    //Failed entitlement check for the security
                    entitlementCheck = false;
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Snapshot entitlement CheckIfFailedEntitlementContainsSymbol failed for symbol: {0} for the logged-in user(s).", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
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
            return entitlementCheck;
        }

        public bool PerformSubscriptionDataEntitlementCheck(Element fieldData, string symbol)
        {
            bool entitlementCheck = true;
            try
            {
                if (!CheckIfFailedEntitlementContainsSymbol(symbol))
                {
                    Name eidName = new Name(SAPIConstants.Const_EidSubscription);
                    List<int> eidsList = new List<int>();
                    if (fieldData.HasElement(eidName) && !fieldData.GetElement(eidName).IsNull)
                    {
                        int eidValue = fieldData.GetElementAsInt32(eidName);
                        eidsList.Add(eidValue);

                        bool isServiceOpen = false;
                        if (_session!=null && _isSessionStarted)
                            isServiceOpen = _session.OpenService(SAPIConstants.Const_Mktdata);
                        if (isServiceOpen)
                        {
                            Service continuosDataServiceReference = _session.GetService(SAPIConstants.Const_Mktdata);
                            entitlementCheck = HandleUserEntitlementCheck(continuosDataServiceReference, symbol, eidsList);
                            if (!entitlementCheck)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Subscription entitlement check failed for symbol: {0} for the logged-in user(s).", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
                            }
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Mktdata service closed for symbol: {0}.", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
                        }
                    }
                }
                else
                {
                    //If symbol is already in failed entitlement list, then send the event to all identity users.
                    entitlementCheck = false;
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Subscription entitlement CheckIfFailedEntitlementContainsSymbol failed for symbol: {0} for the logged-in user(s).", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
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
            return entitlementCheck;
        }

        public void RemoveUserFromIdentityMapping(int userId)
        {
            try
            {
                string topic = string.Empty;
                if (_dictUserIdentityMapping.ContainsKey(userId))
                {
                    topic = _dictUserIdentityMapping[userId].Topic;
                    _dictUserIdentityMapping.Remove(userId);
                }
                if (CallbackChannels.ContainsKey(userId.ToString()))
                {
                    CallbackChannels.Remove(userId.ToString());
                }

                //Removing the user identity from the session at the time of logout 
                if (!string.IsNullOrEmpty(topic) && _authenticateUserSession != null)
                    _authenticateUserSession.Cancel(new CorrelationID(topic));

                //When User logout, remove the user from failed entitlement list.
                if (failedEntitlementSymbols.ContainsKey(userId))
                {
                    failedEntitlementSymbols.Remove(userId);
                }
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("UserID {0} has been successfully removed from the identity mapping and the Failed Entitlement list due to logout.", userId), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Error occurred during logout while removing user ID {0} from identity mapping and Failed Entitlement list.", userId), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Information);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        // Checks globally across all users if the symbol exists
        public bool CheckIfFailedEntitlementContainsSymbol(string symbol)
        {
            try
            {
                foreach (var symbols in failedEntitlementSymbols.Values)
                {
                    if (symbols.Contains(symbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Security '{0}' found in the Failed Entitlement list, so we are blocking the received response.", symbol), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Verbose);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return false;
        }

        // Adds a symbol for a specific user
        public void AddSymbolEntryInFailedEntitlement(int userId, string symbol)
        {
            try
            {
                if (!failedEntitlementSymbols.TryGetValue(userId, out var symbols))
                {
                    symbols = new HashSet<string>();
                    failedEntitlementSymbols[userId] = symbols;
                }
                symbols.Add(symbol);
                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("New security '{0}' added to the Failed Entitlement list due to lack of access for UserID {1}.", symbol, userId), LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Verbose);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.

            // Always use SuppressFinalize() in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Overloaded Implementation of Dispose.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// <list type="bulleted">Dispose(bool isDisposing) executes in two distinct scenarios.
        /// <item>If <paramref name="isDisposing"/> equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.</item>
        /// <item>If <paramref name="isDisposing"/> equals <c>false</c>, the method has been called
        /// by the runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</item></list>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "session")]
        protected virtual void Dispose(bool isDisposing)
        {
            // TODO If you need thread safety, use a lock around these
            // operations, as well as in your methods that use the resource.

            // Explicitly set root references to null to expressly tell the GarbageCollector
            // that the resources have been disposed of and its ok to release the memory
            // allocated for them.
            if (isDisposing)
            {
                // Release all managed resources here
                // Need to unregister/detach yourself from the events. Always make sure
                // the object is not null first before trying to unregister/detach them!
                // Failure to unregister can be a BIG source of memory leaks
                // If this is a WinForm/UI control, uncomment this code
                //session.Dispose();
                //if (components != null)
                //{
                //    components.Dispose();
                //}
                // Release all unmanaged resources here
            }
        }

        #endregion
    }
}