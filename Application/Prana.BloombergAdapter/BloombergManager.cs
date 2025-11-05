using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BloombergAdapter
{
    /// <summary>
    /// This is a blank ILiveFeedAdapter implementation to be used for Bloomberg Provider
    /// </summary>
    public class BloombergManager : ILiveFeedAdapter
    {
        #pragma warning disable CS0067
        //Suppressing these warnings as these events are not used in the current implementation but can't remove these events as they are part of the interface contract.
        public event EventHandler<EventArgs<bool>> Connected;
        public event EventHandler<EventArgs<bool>> Disconnected;
        public event EventHandler<Data> ContinuousDataResponse;
        public event EventHandler<Data> SnapShotDataResponse;
        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        public event EventHandler<EventArgs<string>> FactSetSymbolDenied;
        #pragma warning restore CS0067

        private string _clientConnectionUsername = string.Empty;
        private string _clientConnectionPassword = string.Empty;
        private const string encryptionKey = @"sblw-3hn8-sqoy19";
        public string BBGSymbol
        {
            get;
            set;
        }

        private static BloombergManager _instance;

        /// <summary>
        /// The locker object
        /// </summary>
        private static object _lockerObject = new object();
        /// <summary>
        /// Singleton Instance for the BloombergManager which fetch data.
        /// </summary>
        /// <returns>
        /// Instance of BloombergManager
        /// </returns>
        public static BloombergManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new BloombergManager();
                        }
                    }
                }

                return _instance;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<string> GetCredentials()
        {
            return new List<string>()
            {
                _clientConnectionUsername,
                _clientConnectionPassword
            };
        }
        void LoadCredentials()
        {
            try
            {
                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_BloombergSettings, "BloombergCredentialDetails"));

                if (File.Exists(credentialsFilePath))
                {
                    string encryptedProperties = File.ReadAllText(credentialsFilePath, Encoding.UTF8);
                    string decryptedProperties = TripleDESEncryptDecrypt.TripleDESDecryption(encryptedProperties, encryptionKey);
                    if (!string.IsNullOrWhiteSpace(decryptedProperties))
                    {
                        string[] decryptedPropertiesValue = decryptedProperties.Split(Seperators.SEPERATOR_6);

                        _clientConnectionUsername = decryptedPropertiesValue[0];
                        _clientConnectionPassword = decryptedPropertiesValue[1];
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

        public bool UpdateCredentials(string _clientUsername, string _clientPassword)
        {
            try
            {
                _clientConnectionUsername = _clientUsername;
                _clientConnectionPassword = _clientPassword;

                string credentials = _clientUsername + Seperators.SEPERATOR_6 + _clientPassword + Seperators.SEPERATOR_6;

                string credentialsFilePath = Path.Combine(new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.ToString(), ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_BloombergSettings, "BloombergCredentialDetails"));

                if (!File.Exists(credentialsFilePath))
                {
                    if (string.IsNullOrWhiteSpace(credentialsFilePath))
                        throw new Exception("FactSet credentials storage path is not valid. Please contact administrator.");
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
                UnauthorizedAccessException ex = new UnauthorizedAccessException("Access to FactSet credentials storage is denied. Please contact administrator.");
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

        private BloombergManager()
        {
            LoadCredentials();
        }

        #region UnusedMethods
        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void DeleteSymbol(string symbol)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public List<SymbolData> GetAvailableLiveFeed()
        {
            throw new NotImplementedException();
        }

        public void GetContinuousData(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetLiveDataDirectlyFromFeed()
        {
            throw new NotImplementedException();
        }

        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            throw new NotImplementedException();
        }

        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {
            //Call BBG conversion method
            //  MarketDataSymbolResponse resp = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);

        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetUserInformation()
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            throw new NotImplementedException();
        }

        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {
            throw new NotImplementedException();
        }

        public void UpdateSecurityDetails(SecMasterbaseList secMasterList)
        {
            throw new NotImplementedException();
        }

        void ConvertToBBGSymbol(string Symbol)
        {
            //Convert the given symbol to BBG symbol
        }
        #endregion
    }
}
