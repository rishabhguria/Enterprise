using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
namespace Prana.ServiceConnector
{
    public class RebalancerServiceApi : IRebalancerBLService, IDisposable
    {

        /// <summary>
        /// The rebalancer service
        /// </summary>
        private ProxyBase<IRebalancerBLService> _rebalancerService = null;

        #region SingletonInstance
        /// <summary>
        /// The _lock
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The _expnl service connector
        /// </summary>
        private static RebalancerServiceApi _rebalancerServiceApi = null;


        /// <summary>
        /// Prevents a default instance of the <see cref="RebalancerServiceApi"/> class from being created.
        /// </summary>
        private RebalancerServiceApi()
        {
            try
            {
                _rebalancerService = new ProxyBase<IRebalancerBLService>("TradeRebalancerBLServiceEndpointAddress");
                _rebalancerService.ConnectedEvent += RebalancerServiceOnConnectedEvent;
                _rebalancerService.DisconnectedEvent += RebalancerServiceOnDisConnectedEvent;
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

        private void RebalancerServiceOnDisConnectedEvent(object sender, EventArgs e)
        {

        }

        private void RebalancerServiceOnConnectedEvent(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static RebalancerServiceApi GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_rebalancerServiceApi == null)
                        _rebalancerServiceApi = new RebalancerServiceApi();
                    return _rebalancerServiceApi;
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
            return null;
        }
        #endregion

        private bool _isExpnlServiceConnected;
        public bool IsExpnlServiceConnected
        {
            get { return _isExpnlServiceConnected; }
            set { _isExpnlServiceConnected = value; }
        }

        /// <summary>
        /// Tries the get channel.
        /// </summary>
        /// <returns></returns>
        public string TryGetChannel()
        {
            try
            {
                var rebalancerServiceChannel = _rebalancerService.InnerChannel;

                if (rebalancerServiceChannel != null)
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public Dictionary<int, List<int>> GetAllCustomFundGroupsMapping()
        {
            return _rebalancerService.InnerChannel.GetAllCustomFundGroupsMapping();
        }

        public Dictionary<int, string> GetAllCustomFundGroups()
        {
            return _rebalancerService.InnerChannel.GetAllCustomFundGroups();
        }

        public List<ModelPortfolioDto> GetModelPortfolios()
        {
            return _rebalancerService.InnerChannel.GetModelPortfolios();
        }

        public bool SaveEditModelPortfolio(ModelPortfolioDto modelPortfolioDto, bool isEdit)
        {
            return _rebalancerService.InnerChannel.SaveEditModelPortfolio(modelPortfolioDto, isEdit);
        }

        public bool DeleteModelPortfolio(int modelPortfolioId)
        {
            return _rebalancerService.InnerChannel.DeleteModelPortfolio(modelPortfolioId);
        }

        public bool SaveCustomGroupMapping(CustomGroupDto customGroupDto)
        {
            return _rebalancerService.InnerChannel.SaveCustomGroupMapping(customGroupDto);
        }

        public bool DeleteCustomGroupMapping(int customGroupId)
        {
            return _rebalancerService.InnerChannel.DeleteCustomGroupMapping(customGroupId);
        }

        public bool UpdateRebalPreferences(RebalPreferencesDto rebalPreferences)
        {
            return _rebalancerService.InnerChannel.UpdateRebalPreferences(rebalPreferences);
        }

        public bool UpdateRebalPreferencesForAllAccounts(string preferenceKey,
            Dictionary<int, string> preferenceDictionary)
        {
            return _rebalancerService.InnerChannel.UpdateRebalPreferencesForAllAccounts(preferenceKey, preferenceDictionary);
        }

        public Dictionary<Tuple<int, string>, string> GetRebalPreferences()
        {
            return _rebalancerService.InnerChannel.GetRebalPreferences();
        }


        #region IDisposable

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_rebalancerServiceApi != null)
                        _rebalancerServiceApi = null;
                    if (_rebalancerService != null)
                    {
                        _rebalancerService.ConnectedEvent -= RebalancerServiceOnConnectedEvent;
                        _rebalancerService.DisconnectedEvent -= RebalancerServiceOnDisConnectedEvent;
                        _rebalancerService.Dispose();
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
        #endregion

        public bool SaveRebalancerTradeList(DateTime selectedDate, string smartName, int id, string strtradeList)
        {
            return _rebalancerService.InnerChannel.SaveRebalancerTradeList(selectedDate, smartName, id, strtradeList);
        }

        public Dictionary<string, int> GetRebalancerTradeListNames(DateTime selectedDate)
        {
            return _rebalancerService.InnerChannel.GetRebalancerTradeListNames(selectedDate);
        }

        public string GetTradeList(int tradeListId)
        {
            return _rebalancerService.InnerChannel.GetTradeList(tradeListId);
        }

        public string GetSmartName()
        {
            return _rebalancerService.InnerChannel.GetSmartName();
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }

}
