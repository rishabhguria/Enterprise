using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.UIEventAggregator;
using Prana.UIEventAggregator.Events;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.ServiceConnector
{
    /// <summary>
    /// Wrapper of Expnl Se4rvice contacts it and brings out data
    /// </summary>
    public class ExpnlServiceConnector : IDisposable
    {
        /// <summary>
        /// The _expnl connector service
        /// </summary>
        private ProxyBase<IExpnlCalculationService> _expnlCalculationService = null;

        #region SingletonInstance
        /// <summary>
        /// The _lock
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The _expnl service connector
        /// </summary>
        private static ExpnlServiceConnector _expnlServiceConnector = null;

        /// <summary>
        /// Prevents a default instance of the <see cref="ExpnlServiceConnector"/> class from being created.
        /// </summary>
        private ExpnlServiceConnector()
        {
            try
            {
                _expnlCalculationService = new ProxyBase<IExpnlCalculationService>("ExpnlCalculationServiceEndpointAddress");
                _expnlCalculationService.ConnectedEvent += ExpnlCalculationService_ConnectedEvent;
                _expnlCalculationService.DisconnectedEvent += ExpnlCalculationService_DisconnectedEvent;
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
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static ExpnlServiceConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_expnlServiceConnector == null)
                        _expnlServiceConnector = new ExpnlServiceConnector();
                    return _expnlServiceConnector;
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
                var expnlConnectorServiceChannel = _expnlCalculationService.InnerChannel;

                if (expnlConnectorServiceChannel != null)
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

        /// <summary>
        /// Gets the gross exposure for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetGrossExposureForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetGrossExposureForSymbolAndAccounts(symbol, accountIds, ref errorMessage);
        }
        /// <summary>
        /// Gets the Day PNL for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetDayPNLForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetDayPNLForSymbolAndAccounts(symbol, accountIds, ref errorMessage);
        }

        /// <summary>
        /// Gets the account nav.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetAccountNAV(List<int> accountIds, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetAccountNAV(accountIds, ref errorMessage);
        }

        /// <summary>
        /// Gets the master fund nav.
        /// </summary>
        /// <param name="fundList">The fund list.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetMasterFundNAV(List<int> fundList, ref StringBuilder errorMessage)
        {
            Dictionary<int, decimal> currentNavDictionary = new Dictionary<int, decimal>();
            try
            {
                Dictionary<int, List<int>> mfAssociatedAccounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                List<int> mfAccountList = mfAssociatedAccounts.Where(x => fundList.Contains(x.Key)).SelectMany(y => y.Value).ToList();
                Dictionary<int, decimal> accountNavDictionary = GetAccountNAV(mfAccountList, ref errorMessage);
                foreach (int mfId in mfAssociatedAccounts.Keys)
                {
                    decimal mfNAV = accountNavDictionary.Where(x => mfAssociatedAccounts[mfId].Contains(x.Key)).Sum(y => y.Value);
                    currentNavDictionary.Add(mfId, mfNAV);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currentNavDictionary;
        }

        /// <summary>
        /// Gets the accounts start of day nav.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetAccountsStartOfDayNAV(List<int> accountIds, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetAccountsStartOfDayNAV(accountIds, ref errorMessage);
        }

        /// <summary>
        /// Gets the master fund startof day nav.
        /// </summary>
        /// <param name="fundList">The fund list.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetMasterFundStartofDayNAV(List<int> fundList, ref StringBuilder errorMessage)
        {
            Dictionary<int, decimal> currentNavDictionary = new Dictionary<int, decimal>();
            try
            {
                Dictionary<int, List<int>> mfAssociatedAccounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                List<int> mfAccountList = mfAssociatedAccounts.Where(x => fundList.Contains(x.Key)).SelectMany(y => y.Value).ToList();
                Dictionary<int, decimal> accountNavDictionary = GetAccountsStartOfDayNAV(mfAccountList, ref errorMessage);
                foreach (int mfId in mfAssociatedAccounts.Keys)
                {
                    decimal mfNAV = accountNavDictionary.Where(x => mfAssociatedAccounts[mfId].Contains(x.Key)).Sum(y => y.Value);
                    currentNavDictionary.Add(mfId, mfNAV);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currentNavDictionary;
        }

        /// <summary>
        /// Gets the position for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetPositionForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorMessage, bool isAddingSwap = true, bool isAddInMarketPostion = false)
        {
            return _expnlCalculationService.InnerChannel.GetPositionForSymbolAndAccounts(symbol, accountIds, ref errorMessage, isAddingSwap, isAddInMarketPostion);
        }

        /// <summary>
        /// Gets the symbol share out standing.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>decimal</returns>
        public decimal GetSymbolShareOutStanding(string symbol)
        {
            return _expnlCalculationService.InnerChannel.GetSymbolShareOutStanding(symbol);
        }

        public Dictionary<int, double> GetPositionWithSideForSymbolAndAccounts(string symbol, List<int> accountIds, bool isAddInMarketPostion, string orderSideTagValue)
        {
            return _expnlCalculationService.InnerChannel.GetPositionWithSideForSymbolAndAccounts(symbol, accountIds, isAddInMarketPostion, orderSideTagValue);
        }
        /// <summary>
        /// Gets the px selected feed for symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public decimal GetPXSelectedFeedForSymbol(string symbol, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetPXSelectedFeedForSymbol(symbol, ref errorMessage);
        }

        /// <summary>
        /// Gets the px selected feed base for symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public decimal GetPXSelectedFeedBaseForSymbol(string symbol, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetPXSelectedFeedBaseForSymbol(symbol, ref errorMessage);
        }

        /// <summary>
        /// Gets the fx rate for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetFxRateForSymbolAndAccounts(string symbol, List<int> accountIds, int auecId, int currencyID, ref StringBuilder errorMessage)
        {
            return _expnlCalculationService.InnerChannel.GetFxRateForSymbolAndAccounts(symbol, accountIds, auecId, currencyID, ref errorMessage);
        }

        /// <summary>
        /// Expnls the connector service on connected event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExpnlCalculationService_ConnectedEvent(object sender, EventArgs eventArgs)
        {
            try
            {
                _isExpnlServiceConnected = true;

                EventAggregator.GetInstance.PublishEvent(new PTTExpnlStatus
                {
                    IsExpnlServiceConnected = true
                });
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
        /// Handles the DisconnectedEvent event of the _expnlConnectorService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExpnlCalculationService_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                _isExpnlServiceConnected = false;

                EventAggregator.GetInstance.PublishEvent(new PTTExpnlStatus
                {
                    IsExpnlServiceConnected = false
                });
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
        /// Gets the values for leveling.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <param name="fundList">The fund list.</param>
        /// <param name="accountWiseNav">The account wise nav.</param>
        /// <param name="groupWiseMarketValue">The group wise market value.</param>
        /// <param name="symbolAccountWiseMarketValue">The symbol account wise market value.</param>
        /// <returns></returns>
        public StringBuilder GetValuesForLeveling(List<TaxLot> taxlotList, List<int> fundList, ref Dictionary<int, decimal> accountWiseNav, ref Dictionary<string, double> groupWiseMarketValue, ref Dictionary<string, Dictionary<int, double>> symbolAccountWiseMarketValue)
        {
            return _expnlCalculationService.InnerChannel.GetValuesForLeveling(taxlotList, fundList, ref accountWiseNav, ref groupWiseMarketValue, ref symbolAccountWiseMarketValue);
        }

        /// <summary>
        /// Gets the mf values for leveling.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <param name="fundList">The fund list.</param>
        /// <param name="masterFundWiseNav">The master fund wise nav.</param>
        /// <param name="groupWiseMarketValue">The group wise market value.</param>
        /// <param name="symbolMFWiseMarketValue">The symbol mf wise market value.</param>
        /// <returns></returns>
        public StringBuilder GetMFValuesForLeveling(List<TaxLot> taxlotList, List<int> fundList, ref Dictionary<int, decimal> masterFundWiseNav, ref Dictionary<string, double> groupWiseMarketValue, ref Dictionary<string, Dictionary<int, double>> symbolMFWiseMarketValue)
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                Dictionary<int, List<int>> mfAssociatedAccounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                List<int> mfAccountList = mfAssociatedAccounts.Where(x => fundList.Contains(x.Key)).SelectMany(y => y.Value).ToList();
                Dictionary<int, decimal> accountNavDictionary = new Dictionary<int, decimal>();
                Dictionary<string, Dictionary<int, double>> accountWiseMarketValue = new Dictionary<string, Dictionary<int, double>>();
                errorMessage = ExpnlServiceConnector.GetInstance().GetValuesForLeveling(taxlotList, mfAccountList, ref accountNavDictionary, ref groupWiseMarketValue, ref accountWiseMarketValue);
                foreach (int mfId in mfAssociatedAccounts.Keys)
                {
                    decimal mfNAV = accountNavDictionary.Where(x => mfAssociatedAccounts[mfId].Contains(x.Key)).Sum(y => y.Value);
                    masterFundWiseNav.Add(mfId, mfNAV);
                }
                foreach (string symbol in accountWiseMarketValue.Keys)
                {
                    foreach (int mfId in mfAssociatedAccounts.Keys)
                    {
                        double mfMarketValue = accountWiseMarketValue[symbol].Where(x => mfAssociatedAccounts[mfId].Contains(x.Key)).Sum(y => y.Value);
                        if (!symbolMFWiseMarketValue.ContainsKey(symbol))
                            symbolMFWiseMarketValue.Add(symbol, new Dictionary<int, double>());
                        symbolMFWiseMarketValue[symbol].Add(mfId, mfMarketValue);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        public RebalancerData GetRebalancerData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder)
        {
            return _expnlCalculationService.InnerChannel.GetRebalancerData(accountIds, RebalPositionType, ref errorStringBuilder);
        }

        public Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolioData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string, decimal> dictSymbolTolerancePercentage)
        {
            return _expnlCalculationService.InnerChannel.GetModelPortfolioData(accountIds, RebalPositionType, ref errorStringBuilder, dictSymbolTolerancePercentage);
        }

        public Dictionary<string, decimal> RefreshPrices(List<string> symbolInformation, ref StringBuilder errorStringBuilder)
        {
            return _expnlCalculationService.InnerChannel.RefreshPrices(symbolInformation, ref errorStringBuilder);
        }

        public Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>> RefreshPositions(Dictionary<string, List<int>> symbolandAccountIdsInformation, ref StringBuilder errorStringBuilder)
        {
            return _expnlCalculationService.InnerChannel.RefreshPositions(symbolandAccountIdsInformation, ref errorStringBuilder);
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
                    if (_expnlCalculationService != null)
                        _expnlCalculationService.Dispose();
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

        public Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolios(Dictionary<string, decimal> dictSymbolTargetPercentage, RebalancerEnums.RebalancerPositionsType rebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string, decimal> dictSymbolTolerancePercentage)
        {
            return _expnlCalculationService.InnerChannel.GetModelPortfolios(dictSymbolTargetPercentage, rebalPositionType, ref errorStringBuilder, dictSymbolTolerancePercentage);
        }

        /// <summary>
        /// Updates the in market taxlots.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        public void UpdateInMarketTaxlots(List<TaxLot> taxlotList, bool isStartUpData = false)
        {
            if (CachedDataManager.GetInstance.GetIsMarketDataPermissionEnabledForTradingRules())
                _expnlCalculationService.InnerChannel.UpdateInMarketTaxlots(taxlotList, isStartUpData);
        }
    }
}