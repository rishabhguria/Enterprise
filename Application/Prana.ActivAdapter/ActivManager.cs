using ActivFinancial.Middleware.Application;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Prana.ActivAdapter
{
    public class ActivManager : ILiveFeedAdapter, IDisposable
    {
        private NirvanaContentGatewayClient _nirvanaContentGatewayClient = null;

        private const string encryptionKey = @"sblw-3hn8-sqoy19";
        private string _username = string.Empty;
        private string _password = string.Empty;
        private static object _lockerObject = new object();
        private bool _configEnableMarketDataSimulationForAutomation = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableMarketDataSimulationForAutomation"]);
        private int _waitTimeInBetweenConnectionResetEvents = int.Parse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "WaitTimeInBetweenConnectionResetEvents"));

        private static ActivManager _instance = null;
        public static ActivManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ActivManager();
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
        private ActivManager()
        {
            try
            {
                LoadCredentials();
                MarketDataAdapterExtension.CreateSecMasterServicesProxy();

                // construct application settings
                Settings setting = new Settings();

                // construct new application.
                ActivApplication application = new ActivApplication(setting);

                // start a thread to run the application. Async callbacks will be processed by this thread
                application.StartThread();

                _nirvanaContentGatewayClient = new NirvanaContentGatewayClient(application);
                _nirvanaContentGatewayClient.IsConnected += NirvanaContentGatewayClient_IsConnected;
                _nirvanaContentGatewayClient.SnapShotDataResponse += NirvanaContentGatewayClient_SnapShotDataResponse;
                _nirvanaContentGatewayClient.ContinuousDataResponse += NirvanaContentGatewayClient_ContinuousDataResponse;
                _nirvanaContentGatewayClient.RetryConnection += NirvanaContentGatewayClient_RetryConnection;
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
        #endregion

        #region Private Methods
        private void NirvanaContentGatewayClient_IsConnected(object sender, bool e)
        {
            try
            {
                if (e)
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

        private void NirvanaContentGatewayClient_SnapShotDataResponse(object sender, Data e)
        {
            try
            {
                if (SnapShotDataResponse != null)
                    SnapShotDataResponse(this, e);
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

        private void NirvanaContentGatewayClient_ContinuousDataResponse(object sender, Data e)
        {
            try
            {
                if (ContinuousDataResponse != null)
                    ContinuousDataResponse(this, e);
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

        private void NirvanaContentGatewayClient_RetryConnection(object sender, EventArgs e)
        {
            try
            {
                Connect();
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
        #endregion

        #region Credential Management
        private void LoadCredentials()
        {
            try
            {
                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ActivCredentialDetails"));

                if (File.Exists(credentialsFilePath))
                {
                    string encryptedProperties = File.ReadAllText(credentialsFilePath, Encoding.UTF8);
                    string decryptedProperties = TripleDESEncryptDecrypt.TripleDESDecryption(encryptedProperties, encryptionKey);
                    if (!string.IsNullOrWhiteSpace(decryptedProperties))
                    {
                        string[] decryptedPropertiesValue = decryptedProperties.Split(Seperators.SEPERATOR_6);

                        _username = decryptedPropertiesValue[0];
                        _password = decryptedPropertiesValue[1];
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

        public List<string> GetCredentials()
        {
            return new List<string>()
            {
                _username,
                _password
            };
        }

        public bool UpdateCredentials(string username, string password)
        {
            try
            {
                _username = username;
                _password = password;

                string credentials = _username + Seperators.SEPERATOR_6 + _password;
                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ActivSettings, "ActivCredentialDetails"));

                if (!File.Exists(credentialsFilePath))
                {
                    if (string.IsNullOrWhiteSpace(credentialsFilePath))
                        throw new Exception("ACTIV credentials storage path is not valid. Please contact administrator.");
                    else if (!Directory.Exists(credentialsFilePath))
                        Directory.CreateDirectory(Path.GetDirectoryName(credentialsFilePath));
                }
                File.WriteAllText(credentialsFilePath, TripleDESEncryptDecrypt.TripleDESEncryption(credentials, encryptionKey));
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessException ex = new UnauthorizedAccessException("Access to ACTIV credentials storage is denied. Please contact administrator.");
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

        #region Public Methods
        public bool VerifyUserDetails(string username, string password)
        {
            bool isValidCredentials = false;
            try
            {
                // construct application settings
                Settings setting = new Settings();

                // construct new application.
                ActivApplication application = new ActivApplication(setting);

                // start a thread to run the application. Async callbacks will be processed by this thread
                application.StartThread();

                NirvanaContentGatewayClient nirvanaContentGatewayClient = new NirvanaContentGatewayClient(application);

                isValidCredentials = nirvanaContentGatewayClient.VerifyUserDetails(username, password);

                nirvanaContentGatewayClient = null;
                application.PostDiesToThreads();
                application.WaitForThreadsToExit();
                application = null;
                setting = null;
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

            return isValidCredentials;
        }
        #endregion

        #region ILiveFeedAdapter Methods
        public void Connect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    _nirvanaContentGatewayClient.Disconnect();
                    System.Threading.Thread.Sleep(_waitTimeInBetweenConnectionResetEvents);
                    _nirvanaContentGatewayClient.Connect(_username, _password);
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

        public void Disconnect()
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    _nirvanaContentGatewayClient.Disconnect();
                }

                _nirvanaContentGatewayClient.Application.PostDiesToThreads();
                _nirvanaContentGatewayClient.Application.WaitForThreadsToExit();
                _nirvanaContentGatewayClient = null;
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

        public void GetContinuousData(string tickerSymbol)
        {
            try
            {
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: ActivManager.GetContinuousData() entered: {0}, Time: {1}", tickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));

                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(tickerSymbol))
                {
                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(tickerSymbol);

                    if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ContinuousData Request: {0}, ACTIVSymbol: {1}", tickerSymbol, marketDataSymbolResponse.ActivSymbol), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);

                        _nirvanaContentGatewayClient.GetData(marketDataSymbolResponse, true);
                        _nirvanaContentGatewayClient.GetData(marketDataSymbolResponse, false);
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

        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {
            try
            {
                MarketDataAdapterExtension.SecurityValidationLogging(string.Format("SecurityValidationLogging: ActivManager.GetSnapShotData() entered: {0}, Time: {1}", symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")));

                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(symbol))
                {
                    if (symbologyCode == ApplicationConstants.SymbologyCodes.TickerSymbol)
                    {
                        MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);

                        if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request: {0}, ACTIVSymbol: {1}", symbol, marketDataSymbolResponse.ActivSymbol), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);

                            _nirvanaContentGatewayClient.GetData(marketDataSymbolResponse, true);
                        }
                    }
                    else if (symbologyCode == ApplicationConstants.SymbologyCodes.ActivSymbol)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SnapShotData Request of ACTIVSymbol: {0}", symbol), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);

                        MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse()
                        {
                            ActivSymbol = symbol
                        };

                        if (symbol.ToUpper().EndsWith(".O"))
                        {
                            marketDataSymbolResponse.AssetCategory = AssetCategory.EquityOption;
                            _nirvanaContentGatewayClient.GetData(marketDataSymbolResponse, true);
                        }
                        else
                            _nirvanaContentGatewayClient.GetData(marketDataSymbolResponse, true);
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

        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(underlyingSymbol))
                {
                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(underlyingSymbol);

                    if (marketDataSymbolResponse != null && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("OptionChainData Request: {0}, ActivSymbol: {1}", underlyingSymbol, marketDataSymbolResponse.ActivSymbol), LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Verbose);

                        List<OptionStaticData> optionChainStaticData = _nirvanaContentGatewayClient.GetOptionChain(marketDataSymbolResponse.ActivSymbol, optionChainFilter);
                        OptionChainResponse(null, new EventArgs<string, List<OptionStaticData>>(underlyingSymbol, optionChainStaticData));
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

        public void DeleteSymbol(string tickerSymbol)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation && !string.IsNullOrWhiteSpace(tickerSymbol))
                {
                    _nirvanaContentGatewayClient.DeleteSymbol(tickerSymbol);
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
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ACTIV CheckIfInternationalSymbols not implemented yet", LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Warning);
            return null;
        }

        public Dictionary<string, string> GetUserInformation()
        {
            try
            {
                return _nirvanaContentGatewayClient.GetUserInformation();
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

            return new Dictionary<string, string>();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            try
            {
                return _nirvanaContentGatewayClient.GetUserPermissionsInformation();
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

            return new List<Dictionary<string, string>>();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            try
            {
                return _nirvanaContentGatewayClient.GetSubscriptionInformation();
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

            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            return _nirvanaContentGatewayClient.GetTickersLastStatusCode();
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

        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nirvanaContentGatewayClient.Dispose();
            }
        }
        #endregion
    }
}