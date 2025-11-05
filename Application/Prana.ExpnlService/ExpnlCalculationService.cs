using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Prana.ExpnlService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ExpnlCalculationService : IExpnlCalculationService
    {
        public Dictionary<int, decimal> GetDayPNLForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, decimal> accountWiseDayPNL = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(symbol))
                                        {
                                            accountWiseDayPNL.Add(accountId, Convert.ToDecimal(distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].DayPnL));
                                        }
                                        else
                                        {
                                            accountWiseDayPNL.Add(accountId, 0);
                                        }
                                    }
                                    else
                                    {
                                        accountWiseDayPNL.Add(accountId, 0);
                                    }
                                }
                                else
                                {
                                    accountWiseDayPNL.Add(accountId, 0);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return accountWiseDayPNL;
        }

        public Dictionary<int, decimal> GetGrossExposureForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, decimal> accountWiseGrossExposure = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        #region swap NetExposure for same symbol
                                        double swapNetExposure = 0;
                                        if (compressedData.OutputCompressedData.ContainsKey(accountId))
                                        {
                                            foreach (ExposurePnlCacheItem exposurePnlCacheItem in compressedData.OutputCompressedData[accountId])
                                            {
                                                if (exposurePnlCacheItem.Symbol == symbol && exposurePnlCacheItem.IsSwap)
                                                {
                                                    swapNetExposure += exposurePnlCacheItem.NetExposure;
                                                }
                                            }
                                        }
                                        #endregion
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(symbol))
                                        {
                                            accountWiseGrossExposure.Add(accountId, Convert.ToDecimal(Math.Abs(distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].NetExposure - swapNetExposure)));
                                        }
                                        else
                                        {
                                            accountWiseGrossExposure.Add(accountId, 0);
                                        }
                                    }
                                    else
                                    {
                                        accountWiseGrossExposure.Add(accountId, 0);
                                    }
                                }
                                else
                                {
                                    accountWiseGrossExposure.Add(accountId, 0);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return accountWiseGrossExposure;
        }

        public Dictionary<int, decimal> GetAccountNAV(List<int> accountIds, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, decimal> dictAccountNAV = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        dictAccountNAV.Add(accountId, Convert.ToDecimal(distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.NetAssetValue));
                                    }
                                    else
                                    {
                                        dictAccountNAV.Add(accountId, 0);
                                    }
                                }
                                else
                                {
                                    dictAccountNAV.Add(accountId, 0);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return dictAccountNAV;
        }

        public Dictionary<int, decimal> GetPositionForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder, bool isAddingSwap = true, bool isInMarketIncluded = false)
        {
            Dictionary<int, decimal> accountWisePositions = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        #region swap position for same symbol
                                        double swapPosition = 0;
                                        if (isAddingSwap && compressedData.OutputCompressedData.ContainsKey(accountId))
                                        {
                                            foreach (ExposurePnlCacheItem exposurePnlCacheItem in compressedData.OutputCompressedData[accountId])
                                            {
                                                if (exposurePnlCacheItem.Symbol == symbol && exposurePnlCacheItem.IsSwap)
                                                {
                                                    swapPosition += exposurePnlCacheItem.Quantity;
                                                }
                                            }
                                        }
                                        #endregion
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(symbol))
                                        {
                                            accountWisePositions.Add(accountId, Convert.ToDecimal(distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].NetPosition - swapPosition));
                                        }
                                        else
                                        {
                                            accountWisePositions.Add(accountId, 0);
                                        }
                                    }
                                    else
                                    {
                                        accountWisePositions.Add(accountId, 0);
                                    }
                                }
                                else
                                {
                                    accountWisePositions.Add(accountId, 0);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
                if (isInMarketIncluded)
                {
                    Dictionary<int, decimal> dictMarketNetPosition = TradingRulesInMarketCache.GetInstance().GetInMarketNetPostionOfSymbolInAccounts(symbol, accountIds);
                    foreach (var postionWithAccount in dictMarketNetPosition)
                    {
                        accountWisePositions[postionWithAccount.Key] += postionWithAccount.Value;
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
            return accountWisePositions;
        }

        /// <summary>
        /// Gets the symbol share out standing.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>decimal</returns>
        public decimal GetSymbolShareOutStanding(string symbol)
        {
            decimal SymbolShareOutStandingValue = 0;
            try
            {
                SymbolData symbolLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(symbol, false);
                if (symbolLevel1Data != null)
                {
                    SymbolShareOutStandingValue = symbolLevel1Data.SharesOutstanding;
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
            return SymbolShareOutStandingValue;
        }
        /// <summary>
        /// Gets the position with side for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <param name="errorStringBuilder">The error string builder.</param>
        /// <returns></returns>
        public Dictionary<int, double> GetPositionWithSideForSymbolAndAccounts(string symbol, List<int> accountIds, bool isInMarketIncluded, string orderSideTagValue)
        {
            double totalPosition = 0;
            Dictionary<int, double> accountWisePositionDict = new Dictionary<int, double>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();
                    accountIds.ForEach(accountId =>
                    {
                        if (compressedData.OutputCompressedData.ContainsKey(accountId))
                        {
                            totalPosition = 0;
                            foreach (var ord in compressedData.OutputCompressedData[accountId])
                            {
                                if (ord.Symbol.Equals(symbol) && (ord.OrderSideTagValue.Equals(orderSideTagValue) || (ord.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed && orderSideTagValue == FIXConstants.SIDE_SellShort) || (ord.OrderSideTagValue == FIXConstants.SIDE_Sell && orderSideTagValue == FIXConstants.SIDE_Buy)))
                                    totalPosition += ord.Quantity;
                            }
                            accountWisePositionDict.Add(accountId, Math.Abs(totalPosition));
                        }
                    });
                }
                if (isInMarketIncluded)
                {
                    Dictionary<int, double> accountWiseInMarketPosition = new Dictionary<int, double>();
                    accountWiseInMarketPosition = TradingRulesInMarketCache.GetInstance().GetPositionWithSideOfSymbolInAccounts(symbol, accountIds, orderSideTagValue);
                    foreach (int accountId in accountWiseInMarketPosition.Keys)
                    {
                        if (accountWisePositionDict.ContainsKey(accountId))
                        {
                            accountWisePositionDict[accountId] += accountWiseInMarketPosition[accountId];
                        }
                        else
                        {
                            accountWisePositionDict.Add(accountId, accountWiseInMarketPosition[accountId]);
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
            return accountWisePositionDict;//Math.Abs(totalPosition );
        }

        public Dictionary<int, Tuple<decimal, decimal>> GetPositionForSymbolAndAccountsForRefreshPositions(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, Tuple<decimal, decimal>> accountWisePositions = new Dictionary<int, Tuple<decimal, decimal>>();
            try
            {
                bool isSwapIncluded = false;
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();

                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                #region dictionary for swaps
                                Dictionary<string, Dictionary<bool, List<ExposurePnlCacheItem>>> dictPositionTypeEpnlCacheItem = new Dictionary<string, Dictionary<bool, List<ExposurePnlCacheItem>>>();
                                if (isSwapIncluded && compressedData.OutputCompressedData.ContainsKey(accountId))
                                {
                                    foreach (ExposurePnlCacheItem exposurePnlCacheItem in compressedData.OutputCompressedData[accountId])
                                    {
                                        if (exposurePnlCacheItem.Asset == AssetCategory.Equity.ToString()
                                         || exposurePnlCacheItem.Asset == AssetCategory.PrivateEquity.ToString()
                                         || exposurePnlCacheItem.Asset == AssetCategory.FixedIncome.ToString())
                                        {
                                            if (!dictPositionTypeEpnlCacheItem.ContainsKey(exposurePnlCacheItem.Symbol))
                                            {
                                                dictPositionTypeEpnlCacheItem.Add(exposurePnlCacheItem.Symbol, new Dictionary<bool, List<ExposurePnlCacheItem>>() { { exposurePnlCacheItem.IsSwap, new List<ExposurePnlCacheItem>() { exposurePnlCacheItem } } });
                                            }
                                            else
                                            {
                                                if (!dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol].ContainsKey(exposurePnlCacheItem.IsSwap))
                                                {
                                                    dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol].Add(exposurePnlCacheItem.IsSwap, new List<ExposurePnlCacheItem>() { exposurePnlCacheItem });
                                                }
                                                else
                                                {
                                                    dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol][exposurePnlCacheItem.IsSwap].Add(exposurePnlCacheItem);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(symbol))
                                        {

                                            decimal qty = Convert.ToDecimal(distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].NetPosition);
                                            if (isSwapIncluded && dictPositionTypeEpnlCacheItem.ContainsKey(symbol) && dictPositionTypeEpnlCacheItem[symbol].ContainsKey(true))
                                            {
                                                decimal swapQty = Convert.ToDecimal(dictPositionTypeEpnlCacheItem[symbol][true].Sum(s => s.Quantity));

                                                PositionType swapSide = swapQty > 0 ? PositionType.Long : PositionType.Short;
                                                if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].PositionSideMV == swapSide)
                                                {
                                                    if (qty > swapQty)
                                                    {
                                                        qty += swapQty;
                                                    }
                                                }
                                                else
                                                {
                                                    qty += Math.Abs(swapQty);
                                                }
                                                Tuple<decimal, decimal> tpl = new Tuple<decimal, decimal>(qty, swapQty);
                                                accountWisePositions.Add(accountId, tpl);
                                            }
                                            else
                                            {
                                                Tuple<decimal, decimal> tpl = new Tuple<decimal, decimal>(qty, 0);
                                                accountWisePositions.Add(accountId, tpl);
                                            }
                                        }
                                        else
                                        {
                                            Tuple<decimal, decimal> tpl = new Tuple<decimal, decimal>(0, 0);
                                            accountWisePositions.Add(accountId, tpl);
                                        }
                                    }
                                    else
                                    {
                                        Tuple<decimal, decimal> tpl = new Tuple<decimal, decimal>(0, 0);
                                        accountWisePositions.Add(accountId, tpl);
                                    }
                                }
                                else
                                {
                                    Tuple<decimal, decimal> tpl = new Tuple<decimal, decimal>(0, 0);
                                    accountWisePositions.Add(accountId, tpl);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return accountWisePositions;
        }

        public decimal GetPXSelectedFeedForSymbol(string symbol, ref StringBuilder errorStringBuilder)
        {
            double pxSelectedFeed = 0;
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (compressedData != null)
                    {
                        if (compressedData.OutputCompressedData != null && compressedData.OutputCompressedData.Values != null)
                        {
                            List<ExposurePnlCacheItemList> dictionary = compressedData.OutputCompressedData.Values.ToList();
                            foreach (ExposurePnlCacheItem exposurePnlCacheItem in from exposurePnlCacheItemList in dictionary from exposurePnlCacheItem in exposurePnlCacheItemList where exposurePnlCacheItem.Symbol.Equals(symbol) select exposurePnlCacheItem)
                            {
                                pxSelectedFeed = exposurePnlCacheItem.SelectedFeedPrice;
                                break;
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return Decimal.Parse(Math.Round(pxSelectedFeed, 4).ToString());
        }

        public decimal GetPXSelectedFeedBaseForSymbol(string symbol, ref StringBuilder errorStringBuilder)
        {
            double pxSelectedFeedBase = 0;
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (compressedData != null)
                    {
                        if (compressedData.OutputCompressedData != null && compressedData.OutputCompressedData.Values != null)
                        {
                            List<ExposurePnlCacheItemList> dictionary = compressedData.OutputCompressedData.Values.ToList();
                            foreach (ExposurePnlCacheItem exposurePnlCacheItem in from exposurePnlCacheItemList in dictionary from exposurePnlCacheItem in exposurePnlCacheItemList where exposurePnlCacheItem.Symbol.Equals(symbol) select exposurePnlCacheItem)
                            {
                                pxSelectedFeedBase = exposurePnlCacheItem.SelectedFeedPriceInBaseCurrency;
                                break;
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return Decimal.Parse(Math.Round(pxSelectedFeedBase, 4).ToString());
        }

        public Dictionary<int, decimal> GetFxRateForSymbolAndAccounts(string symbol, List<int> accountIds, int auecId, int currencyID, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, decimal> accountWiseFxRate = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (compressedData != null)
                    {
                        if (compressedData.OutputCompressedData != null && compressedData.OutputCompressedData.Values != null)
                        {
                            List<ExposurePnlCacheItemList> dictionary = compressedData.OutputCompressedData.Values.ToList();
                            foreach (int accountId in accountIds)
                            {
                                accountWiseFxRate.Add(accountId, 1);
                                bool ratesFoundForAccount = false;
                                foreach (ExposurePnlCacheItem exposurePnlCacheItem in from exposurePnlCacheItemList in dictionary from exposurePnlCacheItem in exposurePnlCacheItemList where exposurePnlCacheItem.Symbol.Equals(symbol) && exposurePnlCacheItem.Level1ID == accountId select exposurePnlCacheItem)
                                {
                                    ratesFoundForAccount = true;
                                    //accountWiseFxRate[accountId] = Decimal.Parse(exposurePnlCacheItem.FxRate.ToString());
                                    accountWiseFxRate[accountId] = exposurePnlCacheItem.FxRate.ToDecimal();
                                    break;
                                }
                                if (!ratesFoundForAccount)
                                {
                                    SymbolData fxLevel1Data = null;

                                    int accountBaseCurrency;
                                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountId))
                                    {
                                        accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountId];
                                    }
                                    else
                                    {
                                        accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                                    }
                                    if (currencyID != accountBaseCurrency)
                                    {
                                        string forexSymbol = ForexConverter.GetInstance(DatabaseManager.GetInstance().CompanyID).GetPranaForexSymbolFromCurrencies(currencyID, accountBaseCurrency);
                                        fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(forexSymbol, currencyID, accountBaseCurrency, AssetCategory.Forex);

                                        //FX Rate on PM not considering AUEC dates
                                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-8032
                                        DateTime latestAUECDate = DateTime.Now;
                                        if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates.ContainsKey(auecId))
                                            latestAUECDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[auecId];

                                        if (fxLevel1Data != null)
                                        {
                                            accountWiseFxRate[accountId] = fxLevel1Data.SelectedFeedPrice.ToDecimal();
                                        }
                                        else
                                        {
                                            //CHMW-3132
                                            //if fxrate is not coming from LiveFeed then we are filling fxRate from daily valuation
                                            ConversionRate todaysConversionRate = ForexConverter.GetInstance(DatabaseManager.GetInstance().CompanyID).GetConversionRateForCurrencyToBaseCurrency(currencyID, latestAUECDate, accountId);
                                            if (todaysConversionRate != null)
                                            {
                                                if (todaysConversionRate.ConversionMethod == Operator.M)
                                                {
                                                    accountWiseFxRate[accountId] = todaysConversionRate.RateValue.ToDecimal();
                                                }
                                                else if (todaysConversionRate.RateValue != 0 && todaysConversionRate.ConversionMethod == Operator.D)
                                                {
                                                    accountWiseFxRate[accountId] = (1 / todaysConversionRate.RateValue).ToDecimal();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
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
            return accountWiseFxRate;
        }

        ///TODO: Need to add a common method, Because of Redundancy of the code
        /// <summary>
        /// Gets the accounts start of day nav.
        /// </summary>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="errorStringBuilder">The error string builder.</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetAccountsStartOfDayNAV(List<int> accountIds, ref StringBuilder errorStringBuilder)
        {
            Dictionary<int, decimal> dictAccountStartOfDayNAV = new Dictionary<int, decimal>();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            foreach (int accountId in accountIds)
                            {
                                if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(accountId))
                                {
                                    if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                    {
                                        DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                        dictAccountStartOfDayNAV.Add(accountId, Convert.ToDecimal(distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.YesterdayNAV));
                                    }
                                    else
                                    {
                                        dictAccountStartOfDayNAV.Add(accountId, 0);
                                    }
                                }
                                else
                                {
                                    dictAccountStartOfDayNAV.Add(accountId, 0);
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return dictAccountStartOfDayNAV;
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
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                accountWiseNav = GetAccountNAV(fundList, ref errorMessage);

                foreach (string symbol in taxlotList.Select(x => x.Symbol).Distinct())
                {
                    symbolAccountWiseMarketValue.Add(symbol, new Dictionary<int, double>());
                }
                errorMessage = GetAccountWiseMarketValue(ref symbolAccountWiseMarketValue);

                groupWiseMarketValue = ExPnlCache.Instance.GetMarketValue(taxlotList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Gets the account wise market value.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <param name="errorStringBuilder">The error string builder.</param>
        /// <returns></returns>
        private StringBuilder GetAccountWiseMarketValue(ref Dictionary<string, Dictionary<int, double>> symbolAccountWiseMarketValue)
        {
            StringBuilder errorStringBuilder = new StringBuilder();
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            List<int> accountIds = SessionManager.AccountAndDistinctAccountPermissionSetsMapping.Keys.ToList();
                            foreach (int accountId in accountIds)
                            {
                                if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                {
                                    DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                    foreach (string symbol in distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.Select(y => y.Key))
                                    {
                                        if (symbolAccountWiseMarketValue.ContainsKey(symbol))
                                        {
                                            if (symbolAccountWiseMarketValue[symbol].ContainsKey(accountId))
                                                symbolAccountWiseMarketValue[symbol].Remove(accountId);
                                            symbolAccountWiseMarketValue[symbol].Add(accountId, distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[symbol].NetMarketValue);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorStringBuilder;
        }

        /// <summary>
        /// Updates the in market taxlots.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        public void UpdateInMarketTaxlots(List<TaxLot> taxlotList, bool isStartUpData)
        {
            try
            {
                TradingRulesInMarketCache.GetInstance().addToCache(taxlotList, isStartUpData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region Rebalancer

        public RebalancerData GetRebalancerData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder)
        {
            RebalancerData rebalancerData = new RebalancerData();
            List<RebalancerDto> accountSymbolRebalancerData = new List<RebalancerDto>();
            try
            {
                bool isRealTimePos = RebalPositionType == RebalancerEnums.RebalancerPositionsType.RealTimePositions;
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        rebalancerData.SymbolWisePriceAndFx.Clear();
                        if (dictionary != null)
                        {
                            rebalancerData.AccountWiseNAV = new List<AccountLevelNAV>();
                            foreach (int accountId in accountIds)
                            {
                                AccountLevelNAV accountLevelNAV = new AccountLevelNAV() { AccountId = accountId };
                                if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                {
                                    DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                    double cashInBaseCurrency = 0;
                                    //Account wise Cash in base currency
                                    cashInBaseCurrency += (isRealTimePos ? distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.CashProjected : distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.StartOfDayCash);
                                    accountLevelNAV.CashInBaseCurrency = (decimal)cashInBaseCurrency;

                                    double accrualsInBaseCurrency = 0;
                                    //Account wise accruals in base currency
                                    accrualsInBaseCurrency += isRealTimePos
                                        ? distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.StartOfDayAccruals + distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.DayAccruals
                                        : distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.StartOfDayAccruals;
                                    accountLevelNAV.AccrualsInBaseCurrency = (decimal)accrualsInBaseCurrency;

                                    #region dictionary for swaps
                                    Dictionary<string, Dictionary<bool, List<ExposurePnlCacheItem>>> dictPositionTypeEpnlCacheItem = new Dictionary<string, Dictionary<bool, List<ExposurePnlCacheItem>>>();
                                    if (compressedData.OutputCompressedData.ContainsKey(accountLevelNAV.AccountId))
                                    {
                                        foreach (ExposurePnlCacheItem exposurePnlCacheItem in compressedData.OutputCompressedData[accountLevelNAV.AccountId])
                                        {
                                            if (exposurePnlCacheItem.Asset == AssetCategory.Equity.ToString()
                                             || exposurePnlCacheItem.Asset == AssetCategory.PrivateEquity.ToString()
                                             || exposurePnlCacheItem.Asset == AssetCategory.FixedIncome.ToString())
                                            {
                                                if (!dictPositionTypeEpnlCacheItem.ContainsKey(exposurePnlCacheItem.Symbol))
                                                {
                                                    dictPositionTypeEpnlCacheItem.Add(exposurePnlCacheItem.Symbol, new Dictionary<bool, List<ExposurePnlCacheItem>>() { { exposurePnlCacheItem.IsSwap, new List<ExposurePnlCacheItem>() { exposurePnlCacheItem } } });
                                                }
                                                else
                                                {
                                                    if (!dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol].ContainsKey(exposurePnlCacheItem.IsSwap))
                                                    {
                                                        dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol].Add(exposurePnlCacheItem.IsSwap, new List<ExposurePnlCacheItem>() { exposurePnlCacheItem });
                                                    }
                                                    else
                                                    {
                                                        dictPositionTypeEpnlCacheItem[exposurePnlCacheItem.Symbol][exposurePnlCacheItem.IsSwap].Add(exposurePnlCacheItem);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> kvpSymbolWiseSummary in distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary)
                                    {
                                        //If symbols contains in dictionary then it is of allowed asset classes.
                                        if (dictPositionTypeEpnlCacheItem.ContainsKey(kvpSymbolWiseSummary.Value.Symbol))
                                        {
                                            //if for a symbol, swap and non swap securities both exist
                                            if (dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol].ContainsKey(false) && dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol].ContainsKey(true))
                                            {
                                                //Add equity security
                                                ExposurePnlCacheItem exposurePnlCacheItem = dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol][false].FirstOrDefault();
                                                RebalancerDto rebalancerDto = CreateRebalancerDto(rebalancerData, isRealTimePos, accountLevelNAV, kvpSymbolWiseSummary, exposurePnlCacheItem);

                                                //Add equity swap security
                                                RebalancerDto rebalancerDtoSwap = CreateRebalancerDto(rebalancerData, isRealTimePos, accountLevelNAV, kvpSymbolWiseSummary, exposurePnlCacheItem);
                                                double swapYesterdayNetPosition = 0;
                                                double swapNetPosition = 0;
                                                foreach (ExposurePnlCacheItem swapExposurePnlCacheItem in dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol][true])
                                                {
                                                    if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates.ContainsKey(swapExposurePnlCacheItem.AUECID))
                                                    {
                                                        DateTime currentDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[swapExposurePnlCacheItem.AUECID];
                                                        //If the item is of yesterday only then it would contribute to yesterday Net Positions
                                                        if (swapExposurePnlCacheItem.TradeDate != null && swapExposurePnlCacheItem.TradeDate.Value.Date < currentDate.Date)
                                                        {
                                                            swapYesterdayNetPosition += swapExposurePnlCacheItem.Quantity;
                                                            swapNetPosition += swapExposurePnlCacheItem.Quantity;
                                                        }
                                                        else
                                                        {
                                                            swapNetPosition += swapExposurePnlCacheItem.Quantity;
                                                        }
                                                    }
                                                    accountLevelNAV.SwapNavAdjustment += (decimal)(isRealTimePos ? swapExposurePnlCacheItem.MarketValueInBaseCurrency : swapExposurePnlCacheItem.YesterdayMarketValueInBaseCurrency);
                                                }
                                                if (rebalancerDtoSwap != null)
                                                {
                                                    double qty = isRealTimePos ? swapNetPosition : swapYesterdayNetPosition;
                                                    if (qty != 0)
                                                    {
                                                        rebalancerDtoSwap.Quantity = Math.Abs((decimal)qty);
                                                        rebalancerDtoSwap.Asset = "EquitySwap";
                                                        rebalancerDtoSwap.Side = qty > 0 ? PositionType.Long : PositionType.Short;
                                                        accountSymbolRebalancerData.Add(rebalancerDtoSwap);
                                                    }
                                                }
                                                if (rebalancerDto != null)
                                                {
                                                    double qty = isRealTimePos ? swapNetPosition : swapYesterdayNetPosition;
                                                    if (rebalancerDtoSwap.Side == rebalancerDto.Side)
                                                    {
                                                        if (rebalancerDto.Quantity < rebalancerDtoSwap.Quantity)
                                                        {
                                                            rebalancerDto.Side = rebalancerDto.Side == PositionType.Long ? PositionType.Short : PositionType.Long;
                                                        }
                                                        else
                                                        {
                                                            rebalancerDto.Quantity += (decimal)qty;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rebalancerDto.Quantity += Math.Abs((decimal)qty);
                                                    }
                                                    if (rebalancerDto.Quantity != 0)
                                                    {
                                                        accountSymbolRebalancerData.Add(rebalancerDto);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bool isSwapAvailables = false;
                                                if (dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol].ContainsKey(true))
                                                    isSwapAvailables = true;
                                                ExposurePnlCacheItem exposurePnlCacheItem = dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol][isSwapAvailables].FirstOrDefault();
                                                RebalancerDto rebalancerDto = CreateRebalancerDto(rebalancerData, isRealTimePos, accountLevelNAV, kvpSymbolWiseSummary, exposurePnlCacheItem);
                                                if (rebalancerDto != null)
                                                {
                                                    if (exposurePnlCacheItem.IsSwap)
                                                    {
                                                        rebalancerDto.Asset = "EquitySwap";
                                                        accountLevelNAV.SwapNavAdjustment += (decimal)(isRealTimePos ? dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol][isSwapAvailables].Sum(x => x.MarketValueInBaseCurrency) : dictPositionTypeEpnlCacheItem[kvpSymbolWiseSummary.Value.Symbol][isSwapAvailables].Sum(x => x.YesterdayMarketValueInBaseCurrency));
                                                    }
                                                    if (rebalancerDto.Quantity != 0)
                                                        accountSymbolRebalancerData.Add(rebalancerDto);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            accountLevelNAV.OtherAssetsMarketValue += (decimal)(isRealTimePos ? kvpSymbolWiseSummary.Value.NetMarketValue : kvpSymbolWiseSummary.Value.YesterdayMarketValue);
                                        }
                                    }
                                }
                                rebalancerData.AccountWiseNAV.Add(accountLevelNAV);
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                        foreach (var kvp in compressedData.OutputCompressedData)
                        {
                            foreach (ExposurePnlCacheItem kvp2 in kvp.Value)
                                if (!rebalancerData.SymbolWiseYesterDayPrice.ContainsKey(kvp2.Symbol))
                                {
                                    rebalancerData.SymbolWiseYesterDayPrice.Add(kvp2.Symbol, (decimal)kvp2.YesterdayMarkPrice);
                                }
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
                rebalancerData.RebalancerDtos = accountSymbolRebalancerData;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rebalancerData;
        }

        private RebalancerDto CreateRebalancerDto(RebalancerData rebalancerData, bool isRealTimePos, AccountLevelNAV accountLevelNAV, KeyValuePair<string, ExposureAndPnlOrderSummary> kvpSymbolWiseSummary, ExposurePnlCacheItem exposurePnlCacheItem)
        {
            try
            {
                RebalancerDto rebalancerDto = null;
                if (exposurePnlCacheItem != null)
                {
                    rebalancerDto = new RebalancerDto();
                    rebalancerDto.AccountId = accountLevelNAV.AccountId;
                    rebalancerDto.Symbol = kvpSymbolWiseSummary.Key;
                    rebalancerDto.BloombergSymbol = exposurePnlCacheItem.BloombergSymbol;
                    rebalancerDto.FactSetSymbol = exposurePnlCacheItem.FactSetSymbol;
                    rebalancerDto.ActivSymbol = exposurePnlCacheItem.ActivSymbol;
                    rebalancerDto.Quantity = Math.Abs((decimal)(isRealTimePos ? kvpSymbolWiseSummary.Value.NetPosition : kvpSymbolWiseSummary.Value.YesterdayNetPosition));
                    rebalancerDto.Side = (isRealTimePos ? kvpSymbolWiseSummary.Value.NetPosition : kvpSymbolWiseSummary.Value.YesterdayNetPosition) > 0 ? PositionType.Long : PositionType.Short;
                    //rebalancerDto.Side = kvpSymbolWiseSummary.Value.PositionSideMV;
                    rebalancerDto.AUECID = exposurePnlCacheItem.AUECID;
                    rebalancerDto.Sector = exposurePnlCacheItem.UDASector;
                    if (isRealTimePos)
                    {
                        rebalancerDto.Price = rebalancerDto.Price = (decimal)exposurePnlCacheItem.SelectedFeedPrice;
                        rebalancerDto.FXRate = (decimal)(exposurePnlCacheItem.FxRate != null ? exposurePnlCacheItem.FxRate : 0);
                    }
                    else
                    {
                        //Handling for stale closing marks
                        if (exposurePnlCacheItem.YesterdayMarkPriceStr.Contains("*"))
                        {
                            rebalancerDto.IsStaleClosingMark = true;
                        }
                        //Handling for stale fx rate
                        if (exposurePnlCacheItem.FXRateOnTradeDateStr.Contains("*"))
                        {
                            rebalancerDto.IsStaleFxRate = true;
                        }
                        rebalancerDto.Price = (decimal)exposurePnlCacheItem.YesterdayMarkPrice;
                        rebalancerDto.FXRate = (decimal)exposurePnlCacheItem.YesterdayFXRate;
                    }
                    //Handling of FxRate based on FXConversionMethodOperator
                    if (rebalancerDto.FXRate != 0 &&
                         exposurePnlCacheItem.FXConversionMethodOperator == Operator.D)
                    {
                        rebalancerDto.FXRate = 1 / rebalancerDto.FXRate;
                    }
                    rebalancerDto.Multiplier = (decimal)exposurePnlCacheItem.Multiplier;
                    rebalancerDto.LeveragedFactor = (decimal)exposurePnlCacheItem.LeveragedFactor;
                    rebalancerDto.Delta = (decimal)exposurePnlCacheItem.Delta;
                    rebalancerDto.Asset = exposurePnlCacheItem.Asset;

                    accountLevelNAV.SecuritiesMarketValue += (decimal)(isRealTimePos
                    ? kvpSymbolWiseSummary.Value.NetMarketValue
                    : kvpSymbolWiseSummary.Value.YesterdayMarketValue);
                    if (!rebalancerData.SymbolWisePriceAndFx.ContainsKey(rebalancerDto.Symbol))
                    {
                        PriceAndFx priceAndFx = new PriceAndFx();
                        priceAndFx.price = rebalancerDto.Price;
                        priceAndFx.fx = rebalancerDto.FXRate;
                        rebalancerData.SymbolWisePriceAndFx.Add(rebalancerDto.Symbol, priceAndFx);
                    }
                    rebalancerDto.BloombergSymbolWithExchangeCode = exposurePnlCacheItem.BloombergSymbolWithExchangeCode;
                }
                return rebalancerDto;
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

        public Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolioData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string, decimal> dictSymbolTolerancePercentage)
        {
            Dictionary<string, ModelPortfolioSecurityDto> modelPortfolioData = new Dictionary<string, ModelPortfolioSecurityDto>();
            try
            {
                bool isRealTimePos = RebalPositionType == RebalancerEnums.RebalancerPositionsType.RealTimePositions;
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var calculatedSummaries = groupingComponents[0].GetCalculatedSummaries();
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (calculatedSummaries != null)
                    {
                        Dictionary<int, DistinctAccountSetWiseSummaryCollection> dictionary = calculatedSummaries.OutputAccountSetWiseConsolidatedSummary;
                        if (dictionary != null)
                        {
                            decimal totalAccountWiseNav = 0;
                            foreach (int accountId in accountIds)
                            {
                                if (dictionary.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                                {
                                    DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = dictionary[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]];
                                    totalAccountWiseNav += (decimal)(isRealTimePos
                                        ? distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.NetAssetValue
                                        : distinctAccountSetWiseSummaryCollection.ConsolidationDashBoardSummary.YesterdayNAV);
                                    foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> kvpSymbolWiseSummary in distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary)
                                    {
                                        ExposurePnlCacheItem symbolInformation = compressedData.OutputCompressedData[accountId].FirstOrDefault(x => x.Symbol == kvpSymbolWiseSummary.Key);
                                        if (symbolInformation != null)
                                        {
                                            if (symbolInformation.Asset == AssetCategory.Equity.ToString()
                                                || (symbolInformation.Asset == AssetCategory.Equity.ToString() && symbolInformation.IsSwap)
                                                || symbolInformation.Asset == AssetCategory.PrivateEquity.ToString())
                                            {
                                                decimal tolerancePercentage = 0;
                                                string symbol = kvpSymbolWiseSummary.Key;
                                                decimal marketValue = 0;
                                                if (compressedData.OutputCompressedData != null)
                                                {
                                                    marketValue = (decimal)(isRealTimePos ? kvpSymbolWiseSummary.Value.NetMarketValue : kvpSymbolWiseSummary.Value.YesterdayMarketValue);
                                                }
                                                if (dictSymbolTolerancePercentage.ContainsKey(symbol))
                                                    tolerancePercentage = dictSymbolTolerancePercentage[symbol];
                                                if (modelPortfolioData.ContainsKey(symbol))
                                                    modelPortfolioData[symbol].TargetPercentage += marketValue;
                                                else
                                                {
                                                    ModelPortfolioSecurityDto modelPortfolioSecurityDto = new ModelPortfolioSecurityDto()
                                                    {
                                                        AUECID = symbolInformation.AUECID,
                                                        Symbol = symbol,
                                                        BloombergSymbol = symbolInformation.BloombergSymbol,
                                                        FactSetSymbol = symbolInformation.FactSetSymbol,
                                                        ActivSymbol = symbolInformation.ActivSymbol,
                                                        Asset = symbolInformation.Asset,
                                                        Delta = (decimal)symbolInformation.Delta,
                                                        LeveragedFactor = (decimal)symbolInformation.LeveragedFactor,
                                                        Multiplier = (decimal)symbolInformation.Multiplier,
                                                        Price = (decimal)(isRealTimePos ? symbolInformation.SelectedFeedPrice : symbolInformation.YesterdayMarkPrice),
                                                        FXRate = (decimal)(isRealTimePos ? (symbolInformation.FxRate != null ? (double)symbolInformation.FxRate : 0) : symbolInformation.YesterdayFXRate),
                                                        Sector = symbolInformation.UDASector,
                                                        TargetPercentage = marketValue,
                                                        BloombergSymbolWithExchangeCode = symbolInformation.BloombergSymbolWithExchangeCode,
                                                        TolerancePercentage = tolerancePercentage
                                                    };
                                                    modelPortfolioData.Add(symbol, modelPortfolioSecurityDto);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            List<string> keys = new List<string>(modelPortfolioData.Keys);
                            foreach (var key in keys)
                            {
                                if (totalAccountWiseNav == 0)
                                    modelPortfolioData[key].TargetPercentage = 0;
                                else
                                    modelPortfolioData[key].TargetPercentage = (modelPortfolioData[key].TargetPercentage / totalAccountWiseNav) * 100;
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return modelPortfolioData;
        }

        public Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolios(Dictionary<string, decimal> dictSymbolTargetPercentage, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string,decimal> dictSymbolTolerancePercentage)
        {
            Dictionary<string, ModelPortfolioSecurityDto> modelPortfolioData = new Dictionary<string, ModelPortfolioSecurityDto>();
            bool isRealTimePos = RebalPositionType == RebalancerEnums.RebalancerPositionsType.RealTimePositions;
            try
            {
                List<IGroupingComponent> groupingComponents = ServiceManager.GetInstance().ICompressor;
                if (groupingComponents != null)
                {
                    var compressedData = groupingComponents[0].GetCompressedData();
                    if (compressedData != null)
                    {
                        if (compressedData.OutputCompressedData != null && compressedData.OutputCompressedData.Values != null)
                        {
                            List<ExposurePnlCacheItemList> ExposurePnlCacheItemListOfList = compressedData.OutputCompressedData.Values.ToList();
                            foreach (ExposurePnlCacheItemList exposurePnlCacheItemList in ExposurePnlCacheItemListOfList)
                            {
                                foreach (ExposurePnlCacheItem exposurePnlCacheItem in exposurePnlCacheItemList)
                                {
                                    decimal tolerancePercentage = 0;
                                    if (dictSymbolTargetPercentage.ContainsKey(exposurePnlCacheItem.Symbol))
                                    {
                                        if (dictSymbolTolerancePercentage.ContainsKey(exposurePnlCacheItem.Symbol))
                                            tolerancePercentage = dictSymbolTolerancePercentage[exposurePnlCacheItem.Symbol];

                                        if (!modelPortfolioData.ContainsKey(exposurePnlCacheItem.Symbol))
                                        {
                                            ModelPortfolioSecurityDto modelPortfolioSecurityDto = new ModelPortfolioSecurityDto()
                                            {
                                                AUECID = exposurePnlCacheItem.AUECID,
                                                Symbol = exposurePnlCacheItem.Symbol,
                                                BloombergSymbol = exposurePnlCacheItem.BloombergSymbol,
                                                FactSetSymbol = exposurePnlCacheItem.FactSetSymbol,
                                                ActivSymbol = exposurePnlCacheItem.ActivSymbol,
                                                Asset = exposurePnlCacheItem.Asset,
                                                Delta = (decimal)exposurePnlCacheItem.Delta,
                                                LeveragedFactor = (decimal)exposurePnlCacheItem.LeveragedFactor,
                                                Multiplier = (decimal)exposurePnlCacheItem.Multiplier,
                                                Price = (decimal)(isRealTimePos ? exposurePnlCacheItem.SelectedFeedPrice : exposurePnlCacheItem.YesterdayMarkPrice),
                                                FXRate = (decimal)(isRealTimePos ? (exposurePnlCacheItem.FxRate != null ? (double)exposurePnlCacheItem.FxRate : 0) : exposurePnlCacheItem.YesterdayFXRate),
                                                Sector = exposurePnlCacheItem.UDASector,
                                                TargetPercentage = dictSymbolTargetPercentage[exposurePnlCacheItem.Symbol],
                                                BloombergSymbolWithExchangeCode = exposurePnlCacheItem.BloombergSymbolWithExchangeCode,
                                                TolerancePercentage = tolerancePercentage

                                            };
                                            modelPortfolioData.Add(exposurePnlCacheItem.Symbol, modelPortfolioSecurityDto);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorStringBuilder.Clear();
                            errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                        }
                    }
                    else
                    {
                        errorStringBuilder.Clear();
                        errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return modelPortfolioData;
        }

        public Dictionary<string, decimal> RefreshPrices(List<string> symbolList, ref StringBuilder errorStringBuilder)
        {
            Dictionary<string, decimal> symbolPrice = new Dictionary<string, decimal>();
            try
            {
                if (symbolList != null)
                {
                    foreach (string symbol in symbolList)
                    {
                        if (!string.IsNullOrWhiteSpace(symbol))
                        {
                            symbolPrice.Add(symbol, GetPXSelectedFeedForSymbol(symbol, ref errorStringBuilder));
                        }
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return symbolPrice;
        }

        public Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>> RefreshPositions(Dictionary<string, List<int>> symbolandAccountIdsInformation, ref StringBuilder errorStringBuilder)
        {
            Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>> symbolPosition = new Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>>();
            try
            {
                if (symbolandAccountIdsInformation != null)
                {
                    foreach (KeyValuePair<string, List<int>> kvp in symbolandAccountIdsInformation)
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Key))
                        {
                            symbolPosition.Add(kvp.Key, GetPositionForSymbolAndAccountsForRefreshPositions(kvp.Key, kvp.Value, ref errorStringBuilder));
                        }
                    }
                }
                else
                {
                    errorStringBuilder.Clear();
                    errorStringBuilder.Append("Data not calculated by Calculation Engine yet. Please wait for a while and click on Calculate again.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return symbolPosition;
        }
        #endregion

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