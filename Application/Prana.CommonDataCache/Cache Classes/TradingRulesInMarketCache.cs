using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CommonDataCache
{
    public class TradingRulesInMarketCache
    {
        /// <summary>
        /// The in market cache
        /// </summary>
        private Dictionary<int, Dictionary<string, TaxLot>> _inMarketCache = new Dictionary<int, Dictionary<string, TaxLot>>();

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static TradingRulesInMarketCache _instance = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private TradingRulesInMarketCache()
        { }

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static TradingRulesInMarketCache GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new TradingRulesInMarketCache();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Adds to cache.
        /// </summary>
        /// <param name="reqId">The req identifier.</param>
        /// <param name="taxlotList">The taxlot list.</param>
        public void addToCache(List<TaxLot> taxlotList, bool isStartUpData)
        {
            try
            {
                lock (_lock)
                {
                    if (isStartUpData)
                    {
                        _inMarketCache.Clear();
                    }

                    foreach (var taxlot in taxlotList)
                    {
                        if (taxlot.TaxLotState == Global.ApplicationConstants.TaxLotState.Deleted)
                        {
                            if (_inMarketCache.ContainsKey(taxlot.Level1ID))
                            {
                                _inMarketCache[taxlot.Level1ID].Remove(taxlot.TaxLotID);
                            }
                        }
                        else
                        {
                            if (_inMarketCache.ContainsKey(taxlot.Level1ID))
                            {
                                if (_inMarketCache[taxlot.Level1ID].ContainsKey(taxlot.TaxLotID))
                                {
                                    _inMarketCache[taxlot.Level1ID][taxlot.TaxLotID] = taxlot;
                                }
                                else
                                {
                                    _inMarketCache[taxlot.Level1ID].Add(taxlot.TaxLotID, taxlot);
                                }
                            }
                            else
                            {
                                Dictionary<string, TaxLot> dictAccountTaxlot = new Dictionary<string, TaxLot>() { { taxlot.TaxLotID, taxlot } };
                                _inMarketCache.Add(taxlot.Level1ID, dictAccountTaxlot);
                            }
                        }
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
        /// Gets the in market net postion of symbol in accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIdLst">The account identifier LST.</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetInMarketNetPostionOfSymbolInAccounts(string symbol, List<int> accountIdLst)
        {
            Dictionary<int, decimal> inMarketPostionAccountWiseDict = new Dictionary<int, decimal>();
            try
            {
                lock (_lock)
                {
                    accountIdLst.ForEach(accountId =>
                    {
                        double inMarketPostions = 0.0;
                        if (_inMarketCache.ContainsKey(accountId))
                        {
                            foreach (var taxlot in _inMarketCache[accountId].Values)
                            {
                                if (taxlot.Symbol.Equals(symbol))
                                    inMarketPostions += taxlot.TaxLotQty * taxlot.SideMultiplier;
                            }
                        }
                        inMarketPostionAccountWiseDict[accountId] = (decimal)inMarketPostions;
                    });
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
            return inMarketPostionAccountWiseDict;
        }

        /// <summary>
        /// Gets the position with side of symbol in accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIdLst">The account identifier LST.</param>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <returns></returns>
        public Dictionary<int, double> GetPositionWithSideOfSymbolInAccounts(string symbol, List<int> accountIdLst, string orderSideTagValue)
        {
            double inMarketPostions = 0.0;
            Dictionary<int, double> accountWiseInMarketPositions = new Dictionary<int, double>();
            try
            {

                lock (_lock)
                {
                    accountIdLst.ForEach(accountId =>
                    {
                        if (_inMarketCache.ContainsKey(accountId))
                        {
                            foreach (var taxlot in _inMarketCache[accountId].Values)
                            {
                                if (taxlot.Symbol.Equals(symbol) && ((taxlot.OrderSideTagValue.Equals(orderSideTagValue) || (taxlot.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed && orderSideTagValue == FIXConstants.SIDE_SellShort) || (taxlot.OrderSideTagValue == FIXConstants.SIDE_Sell && orderSideTagValue == FIXConstants.SIDE_Buy))))
                                {
                                    inMarketPostions += taxlot.TaxLotQty * taxlot.SideMultiplier;
                                }
                            }
                            accountWiseInMarketPositions.Add(accountId, Math.Abs(inMarketPostions));
                        }
                    });
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
            return accountWiseInMarketPositions;
        }

        /// <summary>
        /// Gets the in market net postion of symbol in given account.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountId">The account identifier</param>
        /// <returns></returns>
        public double GetInMarketNetPostionOfSymbolInAccount(string symbol, int accountId)
        {
            double inMarketPostions = 0;
            try
            {
                lock (_lock)
                {

                    if (_inMarketCache.ContainsKey(accountId))
                    {
                        foreach (var taxlot in _inMarketCache[accountId].Values)
                        {
                            if (taxlot.Symbol.Equals(symbol))
                                inMarketPostions += taxlot.TaxLotQty * taxlot.SideMultiplier;
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
            return inMarketPostions;
        }
    }
}
