using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CoreService.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PricingService2Manager
{
    public class PricingService2Manager : IDisposable, IServiceStatusCallback
    {
        private static PricingService2Manager _pricingService2Manager = null;
        private DuplexProxyBase<IPricingService2> _pricingService2Proxy = null;

        private PricingService2Manager()
        {
            try
            {
                _pricingService2Proxy = new DuplexProxyBase<IPricingService2>("PricingService2EndpointAddress", this);
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

        public static PricingService2Manager GetInstance
        {
            get
            {
                if (_pricingService2Manager == null)
                {
                    _pricingService2Manager = new PricingService2Manager();
                }
                return _pricingService2Manager;
            }
        }

        #region IServiceStatusCallback Methods
        public void HeartbeatReceived()
        {
        }

        public void ServiceClosed()
        {
        }
        #endregion

        #region IContainerService Methods
        public async System.Threading.Tasks.Task RequestStartupData()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.RequestStartupData();
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

        public async System.Threading.Tasks.Task<byte[]> OpenLog()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.OpenLog();
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

        public async System.Threading.Tasks.Task<byte[]> LoadLog()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.LoadLog();
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

            return null;
        }

        public async System.Threading.Tasks.Task StopService()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.StopService();
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

        public async System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetClientServicesStatus();
            }
            catch
            {
                return new List<HostedService>();
            }
        }

        public async System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SetDebugModeStatus(isDebugModeEnabled);
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

        public async System.Threading.Tasks.Task<bool> GetDebugModeStatus()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetDebugModeStatus();
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

        #region IPricingService2 Methods
        public async System.Threading.Tasks.Task SetRiskLoggingStatus(bool isRiskLoggingEnabled)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SetRiskLoggingStatus(isRiskLoggingEnabled);
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

        public async System.Threading.Tasks.Task<bool> GetRiskLoggingStatus()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetRiskLoggingStatus();
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

        public async System.Threading.Tasks.Task RiskAPIEachCalDurationThreasholdToLogUpdateTime(int timeValue)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.RiskAPIEachCalDurationThreasholdToLogUpdateTime(timeValue);
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

        public async System.Threading.Tasks.Task<object> GetRiskAPIEachCalDurationThreasholdToLogUpdateTime()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetRiskAPIEachCalDurationThreasholdToLogUpdateTime();
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

        public async System.Threading.Tasks.Task<List<EnumerationValue>> GetPricingModels()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetPricingModels();
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

        public async System.Threading.Tasks.Task<WinDaleParams> GetWinDaleParams()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetWinDaleParams();
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

        public async System.Threading.Tasks.Task SaveWinDaleParams(WinDaleParams winDaleParams)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SaveWinDaleParams(winDaleParams);
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

        public async System.Threading.Tasks.Task<MarketDataProvider?> GetFeedProvider()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetFeedProvider();
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
            return null;
        }

        public async System.Threading.Tasks.Task<SecondaryMarketDataProvider?> GetSecondaryFeedProvider()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetSecondaryFeedProvider();
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
            return null;
        }
        public async System.Threading.Tasks.Task UpdateLiveFeedDetails(string username, string password, string host = "", string port = "", string supportUsername = "", string supportPassword = "")
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.UpdateLiveFeedDetails(username, password, host, port, supportUsername, supportPassword);
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

        public async System.Threading.Tasks.Task SecondaryUpdateLiveFeedDetails(string username, string password)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SecondaryUpdateLiveFeedDetails(username, password);
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

        public async System.Threading.Tasks.Task RestartLiveFeed()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.RestartLiveFeed();
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

        public async System.Threading.Tasks.Task<List<string>> DataManagerSetup()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.DataManagerSetup();
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

        public async System.Threading.Tasks.Task<List<string>> SecondaryDataManagerSetup()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.SecondaryDataManagerSetup();
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

        public async System.Threading.Tasks.Task DataManagerClose()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.DataManagerClose();
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

        public async System.Threading.Tasks.Task RequestSymbol(string requestedSymbol)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.RequestSymbol(requestedSymbol);
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

        public async System.Threading.Tasks.Task GetServices()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.GetServices();
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

        public async System.Threading.Tasks.Task InitializeLiveFeedViewerData()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.InitializeLiveFeedViewerData();
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

        public async System.Threading.Tasks.Task<List<SymbolData>> GetLiveFeedDataList()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetLiveFeedDataList();
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

        public async System.Threading.Tasks.Task StopLiveFeedViewerData()
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.StopLiveFeedViewerData();
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

        public async System.Threading.Tasks.Task DeleteAdvisedSymbol(string symbol)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.DeleteAdvisedSymbol(symbol);
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

        public async System.Threading.Tasks.Task<Dictionary<string, MarketDataSymbolResponse>> GetAllMarketDataSymbolInformation()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetAllMarketDataSymbolInformation();
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
            return null;
        }

        public async System.Threading.Tasks.Task RefreshMarketDataSymbolInformation(string tickerSymbol)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.RefreshMarketDataSymbolInformation(tickerSymbol);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
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

        public async System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetSnapshotsSymbolData()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetSnapshotsSymbolData();
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

            return new Dictionary<string, SymbolData>();
        }

        public async System.Threading.Tasks.Task<List<string>> GetAdvicedSymbols()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetAdvicedSymbols();
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

            return new List<string>();
        }

        public async System.Threading.Tasks.Task<Dictionary<string, string>> GetTickersLastStatusCode()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetTickersLastStatusCode();
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

        public async System.Threading.Tasks.Task<Dictionary<string, int>> GetSubscriptionInformation()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetSubscriptionInformation();
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

        public async System.Threading.Tasks.Task<Dictionary<string, string>> GetUserInformation()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetUserInformation();
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

        public async System.Threading.Tasks.Task<List<Dictionary<string, string>>> GetUserPermissionsInformation()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetUserPermissionsInformation();
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

        /// <summary>
        /// Get Updated Live Feed Data From Live Cache
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetUpdatedLiveFeedDataFromLiveCache()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetUpdatedLiveFeedDataFromLiveCache();
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

            return new Dictionary<string, SymbolData>();
        }

        /// <summary>
        /// Get Live Data Directly From Feed provider
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<object> GetLiveDataDirectlyFromFeed()
        {
            try
            {
                var data = await _pricingService2Proxy.InnerChannel.GetLiveDataDirectlyFromFeed();
                return data;
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

            return null;
        }

        /// <summary>
        /// Set Debug Enable Disable
        /// </summary>
        /// <param name="isDebugEnable"></param>
        /// <param name="pctTolerance"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SetDebugEnableDisable(isDebugEnable, pctTolerance);

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
        /// Get Debug Enable Disable Params
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<Tuple<bool, double>> GetDebugEnableDisableParams()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetDebugEnableDisableParams();

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
            return null;
        }
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
                _pricingService2Proxy.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Get Subscribed Symbols Monitoring Data
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<DataTable> GetSubscribedSymbolsMonitoringData()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetSubscribedSymbolsMonitoringData();

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
            return null;
        }

        /// <summary>
        /// Get the SAPI Request Field Data from DB
        /// </summary>
        public async System.Threading.Tasks.Task<DataSet> GetSAPIRequestFieldData(string requestField)
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetSAPIRequestFieldData(requestField);

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
            return null;
        }

        /// <summary>
        /// Save the SAPI Requst Field data in the DB
        /// </summary>
        public async System.Threading.Tasks.Task SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField)
        {
            try
            {
                await _pricingService2Proxy.InnerChannel.SaveSAPIRequestFieldData(saveDataSetTemp, requestField);

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
        /// This method is to get the factset contract type
        /// </summary>
        /// <returns>FactSetContractType</returns>
        public async System.Threading.Tasks.Task<FactSetContractType?> GetFactsetContractType()
        {
            try
            {
                return await _pricingService2Proxy.InnerChannel.GetFactsetContractType();
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
            return null;
        }
    }
}
