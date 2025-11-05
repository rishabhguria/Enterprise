using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Prana.ExpnlService
{
    public class DataCalculator : IDataCalculator
    {
        private int _companyID;
        private SummaryCalculator _summaryCalculator = null;
        public void SetCompanyID(int companyID)
        {
            _companyID = companyID;
        }

        #region IDataCalculator Members
        public void CalculateData(ExposureAndPnlOrderCollection uncalculatedData, ref Dictionary<int, ExposureAndPnlOrderCollection> accountWiseOrderCollection, Dictionary<int, List<int>> distinctAccountPermissionSets)
        {
            try
            {
                DoCalculations(ref uncalculatedData, ref accountWiseOrderCollection, distinctAccountPermissionSets);
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

        public void CalculateSummariesFromData(ref Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummaryCollection, ref Dictionary<int, ExposureAndPnlOrderSummary> accountwiseSummary, ref Dictionary<int, ExposureAndPnlOrderCollection> calculatedData, Dictionary<int, List<int>> distinctAccountPermissionSets, bool isNAVSaved)
        {
            try
            {
                if (!isNAVSaved && accountwiseSummary != null)
                {
                    foreach (ExposureAndPnlOrderSummary uncalculatedSummary in accountwiseSummary.Values)
                    {
                        uncalculatedSummary.YesterdayNAV = 0.0;
                    }
                }

                _summaryCalculator.CalculatePreExistingSummary(distinctAccountPermissionSets, accountwiseSummary);
                _summaryCalculator.CalculateConsolidationSummary(distinctAccountPermissionSets, isNAVSaved);

                _summaryCalculator.GetConsolidationSummary(ref distinctAccountSetWiseSummaryCollection);
                _summaryCalculator.GetFinalAccountWiseSummary(ref accountwiseSummary);

                GetNAVandReturnValues(accountwiseSummary, isNAVSaved);
                GetNAVandReturnValuesConsolidated(distinctAccountSetWiseSummaryCollection, isNAVSaved);
                UpdateAccountSummariesFromCalculatedData(calculatedData);
                CalculatePercentageExposuresandKeyReturns(accountwiseSummary);
                CalculatePercentageExposuresandKeyReturnsConsolidated(distinctAccountSetWiseSummaryCollection);
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

        private void CalculatePercentageExposuresandKeyReturnsConsolidated(Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummary)
        {
            try
            {
                //All these values not calculated as percentage as specifying format on client grid will ensure this will be represented as a percentage
                foreach (KeyValuePair<int, DistinctAccountSetWiseSummaryCollection> kvp in distinctAccountSetWiseSummary)
                {
                    if (kvp.Value.ConsolidationDashBoardSummary.NetAssetValue > 0)
                    {
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentExposure = kvp.Value.ConsolidationDashBoardSummary.NetExposure / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentBetaAdjExposure = kvp.Value.ConsolidationDashBoardSummary.BetaAdjustedExposure / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentCashProjected = kvp.Value.ConsolidationDashBoardSummary.CashProjected / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentGrossMktValue = kvp.Value.ConsolidationDashBoardSummary.GrossMarketValue / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentLongMktValue = kvp.Value.ConsolidationDashBoardSummary.LongMarketValue / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentShortMktValue = kvp.Value.ConsolidationDashBoardSummary.ShortMarketValue / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.PercentUnderlyingGrossExposure = kvp.Value.ConsolidationDashBoardSummary.UnderlyingGrossExposure / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.PercentNetMarketValue = kvp.Value.ConsolidationDashBoardSummary.NetMarketValue / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.PercentUnderlyingLongExposure = kvp.Value.ConsolidationDashBoardSummary.UnderlyingLongExposure / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.PercentUnderlyingShortExposure = kvp.Value.ConsolidationDashBoardSummary.UnderlyingShortExposure / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentBetaAdjustedGrossExposureUnderlying = kvp.Value.ConsolidationDashBoardSummary.BetaAdjustedGrossExposureUnderlying / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentBetaAdjustedLongExposureUnderlying = kvp.Value.ConsolidationDashBoardSummary.BetaAdjustedLongExposureUnderlying / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentBetaAdjustedShortExposureUnderlying = kvp.Value.ConsolidationDashBoardSummary.BetaAdjustedShortExposureUnderlying / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                    }
                    if (kvp.Value.ConsolidationDashBoardSummary.GrossExposure != 0)
                    {
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentExposureGross = kvp.Value.ConsolidationDashBoardSummary.NetExposure / kvp.Value.ConsolidationDashBoardSummary.GrossExposure;
                    }
                    if (kvp.Value.ConsolidationDashBoardSummary.GrossMarketValue != 0)
                    {
                        kvp.Value.ConsolidationDashBoardSummary.PercentNetMarketValueGrossMV = kvp.Value.ConsolidationDashBoardSummary.NetMarketValue / kvp.Value.ConsolidationDashBoardSummary.GrossMarketValue;
                        kvp.Value.ConsolidationDashBoardSummary.DayReturnGrossMarketValue = kvp.Value.ConsolidationDashBoardSummary.DayPnL / kvp.Value.ConsolidationDashBoardSummary.GrossMarketValue;
                    }
                    if (kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV > 0)
                    {
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentDayPnLLong = kvp.Value.ConsolidationDashBoardSummary.DayPnLLong / kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentDayPnLShort = kvp.Value.ConsolidationDashBoardSummary.DayPnLShort / kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV;
                        kvp.Value.ConsolidationDashBoardSummary.NetPercentDayPnLFX = kvp.Value.ConsolidationDashBoardSummary.DayPnLFX / kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV;
                    }

                    if (kvp.Value.ConsolidationDashBoardSummary.UnderlyingShortExposure != 0)
                    {
                        kvp.Value.ConsolidationDashBoardSummary.LongShortExposureRatioUnderlying = Math.Abs(kvp.Value.ConsolidationDashBoardSummary.UnderlyingLongExposure / kvp.Value.ConsolidationDashBoardSummary.UnderlyingShortExposure);
                    }
                    kvp.Value.ConsolidationDashBoardSummary.MTDReturn = (1 + kvp.Value.ConsolidationDashBoardSummary.DayReturn) * (1 + kvp.Value.ConsolidationDashBoardSummary.MTDReturn / 100) - 1;
                    kvp.Value.ConsolidationDashBoardSummary.QTDReturn = (1 + kvp.Value.ConsolidationDashBoardSummary.DayReturn) * (1 + kvp.Value.ConsolidationDashBoardSummary.QTDReturn / 100) - 1;
                    kvp.Value.ConsolidationDashBoardSummary.YTDReturn = (1 + kvp.Value.ConsolidationDashBoardSummary.DayReturn) * (1 + kvp.Value.ConsolidationDashBoardSummary.YTDReturn / 100) - 1;
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

        private void GetNAVandReturnValuesConsolidated(Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummary, bool isNAVSaved)
        {
            try
            {
                foreach (KeyValuePair<int, DistinctAccountSetWiseSummaryCollection> kvp in distinctAccountSetWiseSummary)
                {
                    //Start of Day NAV double in one case
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-13448
                    List<int> distinctAccountSetWiseSummaryOfAccounts = new List<int>(SessionManager.AccountAndDistinctAccountPermissionSetsMapping.Values);
                    if (!distinctAccountSetWiseSummaryOfAccounts.Contains(kvp.Key))
                    {
                        if (!isNAVSaved)
                        {
                            // NetAssetValue  is same as shadowNAV
                            kvp.Value.ConsolidationDashBoardSummary.NetAssetValue = kvp.Value.ConsolidationDashBoardSummary.CashProjected + kvp.Value.ConsolidationDashBoardSummary.NetMarketValue + kvp.Value.ConsolidationDashBoardSummary.StartOfDayAccruals + kvp.Value.ConsolidationDashBoardSummary.DayAccruals;
                            kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV = kvp.Value.ConsolidationDashBoardSummary.StartOfDayCash + kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV + kvp.Value.ConsolidationDashBoardSummary.StartOfDayAccruals;
                            if (kvp.Value.ConsolidationDashBoardSummary.NetAssetValue > 0)
                            {
                                kvp.Value.ConsolidationDashBoardSummary.PNLContributionPercentageSummary = kvp.Value.ConsolidationDashBoardSummary.DayPnL / kvp.Value.ConsolidationDashBoardSummary.NetAssetValue;
                            }
                            else
                            {
                                kvp.Value.ConsolidationDashBoardSummary.PNLContributionPercentageSummary = 0;
                            }
                        }
                        if (kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV == 0)
                        {
                            kvp.Value.ConsolidationDashBoardSummary.DayReturn = 0;
                        }
                        else if (kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV > 0)
                        {
                            kvp.Value.ConsolidationDashBoardSummary.DayReturn = kvp.Value.ConsolidationDashBoardSummary.DayPnL / kvp.Value.ConsolidationDashBoardSummary.YesterdayNAV;
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

        public void CalculateIndexReturns(DataSet indexSymbols, ref DataTable returnsTable, Dictionary<string, double> indicesMarkPriceCache)
        {
            try
            {
                if (indexSymbols != null && indexSymbols.Tables != null && indexSymbols.Tables.Count > 0 && indexSymbols.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in indexSymbols.Tables[0].Rows)
                    {
                        SymbolData indexData = null;
                        string indexSymbol = row["IndexSymbol"].ToString();
                        indexData = LiveFeedManager.Instance.GetDynamicSymbolData(indexSymbol);

                        double selectedFeedPrice = 0.0;
                        if (indexData != null)
                        {
                            if (indexData.PricingSource.Equals(PricingSource.UserDefined))
                                selectedFeedPrice = indexData.SelectedFeedPrice;
                            else
                            {
                                switch (CommonCacheHelper.SelectedFeedPrice)
                                {
                                    case SelectedFeedPrice.Ask:
                                        selectedFeedPrice = indexData.Ask;
                                        break;
                                    case SelectedFeedPrice.Bid:
                                        selectedFeedPrice = indexData.Bid;
                                        break;
                                    case SelectedFeedPrice.Mid:
                                        selectedFeedPrice = indexData.Mid;
                                        break;
                                    case SelectedFeedPrice.iMid:
                                        selectedFeedPrice = indexData.iMid;
                                        break;
                                    case SelectedFeedPrice.Last:
                                    default:
                                        selectedFeedPrice = indexData.LastPrice;
                                        break;
                                }
                            }
                        }

                        foreach (string enumValue in Enum.GetNames(typeof(Duration)))
                        {
                            string key = indexSymbol + "_" + enumValue;
                            if (returnsTable.Columns.Contains(key))
                            {
                                if (selectedFeedPrice == 0)
                                {
                                    returnsTable.Rows[0][key] = 0;
                                }
                                else
                                {
                                    double markValue = 0;
                                    if (indicesMarkPriceCache.ContainsKey(key))
                                    {
                                        markValue = indicesMarkPriceCache[key];
                                        if (markValue != 0.0)
                                        {
                                            returnsTable.Rows[0][key] = (selectedFeedPrice - markValue) / markValue;
                                        }
                                        else
                                        {
                                            returnsTable.Rows[0][key] = 0;
                                        }
                                    }
                                    else
                                    {
                                        returnsTable.Rows[0][key] = 0;
                                    }
                                }
                            }
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

        public void UpdateCurrentDatesAndClearanceTime(Dictionary<int, DateTime> updatedAuecWiseAdjustedCurrentDates, Dictionary<int, DateTime> updatedClearanceTimes)
        {
            try
            {
                _calculatorProvider.UpdateCurrentDatesAndClearanceTime(updatedAuecWiseAdjustedCurrentDates, updatedClearanceTimes);
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
        #endregion

        ICalculatorProvider _calculatorProvider;
        internal DataCalculator(Dictionary<int, DateTime> auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            try
            {
                _summaryCalculator = new SummaryCalculator();
                _calculatorProvider = new CalculatorProvider(auecWiseAdjustedCurrentDates, clearanceTimes);
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

        void Calculate(EPnlOrder order)
        {
            try
            {
                ICalculator valueCalculator = _calculatorProvider.GetCalculator(order.ClassID);
                if (valueCalculator != null)
                {
                    valueCalculator.Calculate(order);
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

        private void DoCalculations(ref ExposureAndPnlOrderCollection uncalculatedOrderCollection, ref Dictionary<int, ExposureAndPnlOrderCollection> accountWiseOrderCollection, Dictionary<int, List<int>> distinctAccountPermissionSets)
        {
            try
            {
                _summaryCalculator.ClearCache();
                ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
                int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                Dictionary<string, AdviceSymbolInfo> requests = new Dictionary<string, AdviceSymbolInfo>();
                if (uncalculatedOrderCollection != null && uncalculatedOrderCollection.Count != 0)
                {
                    for (int i = 0; i < uncalculatedOrderCollection.Count; i++)
                    {
                        EPnlOrder uncalculatedOrder = uncalculatedOrderCollection[i];

                        if (accountWiseOrderCollection.ContainsKey(uncalculatedOrder.Level1ID))
                        {
                            if (!accountWiseOrderCollection[uncalculatedOrder.Level1ID].Contains(uncalculatedOrder.ID))
                            {
                                accountWiseOrderCollection[uncalculatedOrder.Level1ID].Add(uncalculatedOrder);
                            }
                            else
                            {
                                accountWiseOrderCollection[uncalculatedOrder.Level1ID].UpdateOrder(uncalculatedOrder);
                            }
                        }
                        else
                        {
                            ExposureAndPnlOrderCollection temp = new ExposureAndPnlOrderCollection();
                            temp.Add(uncalculatedOrder);
                            accountWiseOrderCollection.Add(uncalculatedOrder.Level1ID, temp);
                        }

                        AssetCategory baseAssetcategory = Mapper.GetBaseAssetCategory(uncalculatedOrder.Asset);
                        if (baseAssetcategory == AssetCategory.Option)
                        {
                            uncalculatedOrder.GetLastIfZero = CommonCacheHelper.LastIfZeroForOptions;
                        }
                        else
                        {
                            uncalculatedOrder.GetLastIfZero = CommonCacheHelper.LastIfZero;
                        }
                        if (uncalculatedOrder.AUECID > 0 && uncalculatedOrder.Quantity > 0)
                        {
                            int accountBaseCurrency;
                            if (accountWiseBaseCurrencyID.ContainsKey(uncalculatedOrder.Level1ID))
                            {
                                accountBaseCurrency = accountWiseBaseCurrencyID[uncalculatedOrder.Level1ID];
                            }
                            else
                            {
                                accountBaseCurrency = companyBaseCurrencyID;
                            }
                            if (uncalculatedOrder.CurrencyID != accountBaseCurrency)
                            {
                                string forexSymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencies(uncalculatedOrder.CurrencyID, accountBaseCurrency);
                                if (!requests.ContainsKey(forexSymbol))
                                {
                                    requests.Add(forexSymbol, new AdviceSymbolInfo(forexSymbol, uncalculatedOrder.CurrencyID, accountBaseCurrency, AssetCategory.Forex));
                                }
                            }
                            if (!requests.ContainsKey(uncalculatedOrder.Symbol))
                            {
                                if (uncalculatedOrder is EPnLOrderFX)
                                {
                                    requests.Add(uncalculatedOrder.Symbol, new AdviceSymbolInfo(uncalculatedOrder.Symbol, ((EPnLOrderFX)uncalculatedOrder).LeadCurrencyID, accountBaseCurrency, AssetCategory.Forex));
                                }
                                else if (uncalculatedOrder is EPnLOrderFXForward)
                                {
                                    requests.Add(uncalculatedOrder.Symbol, new AdviceSymbolInfo(uncalculatedOrder.Symbol, ((EPnLOrderFXForward)uncalculatedOrder).LeadCurrencyID, accountBaseCurrency, AssetCategory.Forex));
                                }
                                else
                                {
                                    requests.Add(uncalculatedOrder.Symbol, new AdviceSymbolInfo(uncalculatedOrder.Symbol));
                                }
                                if (!requests.ContainsKey(uncalculatedOrder.UnderlyingSymbol))
                                    requests.Add(uncalculatedOrder.UnderlyingSymbol, new AdviceSymbolInfo(uncalculatedOrder.UnderlyingSymbol));
                            }
                        }
                    }
                    LiveFeedManager.Instance.GetLiveFeedForSymbolList(requests);

                    Parallel.ForEach(uncalculatedOrderCollection, uncalculatedOrder =>
                    {
                        //Apply Base currency calculations 
                        //Normally AUECID should not be zero or less than. Just to avoid service crash
                        if (uncalculatedOrder.AUECID > 0 && uncalculatedOrder.Quantity > 0)
                        {
                            SymbolData fxLevel1Data = null;

                            int accountBaseCurrency;
                            if (!accountWiseBaseCurrencyID.TryGetValue(uncalculatedOrder.Level1ID, out accountBaseCurrency))
                            {
                                accountBaseCurrency = companyBaseCurrencyID;
                            }
                            if (uncalculatedOrder.CurrencyID != accountBaseCurrency)
                            {
                                if (uncalculatedOrder.Asset == AssetCategory.FX || uncalculatedOrder.Asset == AssetCategory.FXForward)
                                    fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(uncalculatedOrder.Symbol, uncalculatedOrder.CurrencyID, accountBaseCurrency, AssetCategory.Forex);
                                else
                                {
                                    string forexSymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencies(uncalculatedOrder.CurrencyID, accountBaseCurrency);
                                    fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(forexSymbol, uncalculatedOrder.CurrencyID, accountBaseCurrency, AssetCategory.Forex);
                                }

                                //FX Rate on PM not considering AUEC dates
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8032
                                DateTime latestAUECDate = DateTime.Now;
                                if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates.ContainsKey(uncalculatedOrder.AUECID))
                                    latestAUECDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[uncalculatedOrder.AUECID];
                                int accountCurrency = accountWiseBaseCurrencyID.ContainsKey(uncalculatedOrder.Level1ID) ? accountWiseBaseCurrencyID[uncalculatedOrder.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                                if (fxLevel1Data != null)
                                {
                                    if ((uncalculatedOrder is EPnLOrderFX && ((EPnLOrderFX)uncalculatedOrder).LeadCurrencyID != accountBaseCurrency)
                                        || (uncalculatedOrder is EPnLOrderFXForward && ((EPnLOrderFXForward)uncalculatedOrder).LeadCurrencyID != accountBaseCurrency))
                                        uncalculatedOrder.FxRate = fxLevel1Data.SelectedFeedPrice;
                                    else if (uncalculatedOrder is EPnLOrderFX || uncalculatedOrder is EPnLOrderFXForward)
                                        uncalculatedOrder.FxRate = fxLevel1Data.SelectedFeedPrice != 0 ? 1 / fxLevel1Data.SelectedFeedPrice : 0;
                                    else
                                        uncalculatedOrder.FxRate = fxLevel1Data.SelectedFeedPrice;
                                }
                                else
                                {
                                    //if fxrate is not coming from LiveFeed then we are filling fxRate from daily valuation
                                    ConversionRate todaysConversionRate = null;
                                    lock (_forexLocker)
                                    {
                                        todaysConversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(uncalculatedOrder.CurrencyID, latestAUECDate, uncalculatedOrder.Level1ID);
                                    }
                                    if (todaysConversionRate != null)
                                    {
                                        if (uncalculatedOrder.Asset == AssetCategory.FX || uncalculatedOrder.Asset == AssetCategory.FXForward)
                                        {
                                            if ((uncalculatedOrder is EPnLOrderFXForward && ((EPnLOrderFXForward)uncalculatedOrder).LeadCurrencyID != accountCurrency) || (uncalculatedOrder is EPnLOrderFX && ((EPnLOrderFX)uncalculatedOrder).LeadCurrencyID != accountCurrency))
                                                uncalculatedOrder.FxRate = todaysConversionRate.RateValue;
                                            else
                                                uncalculatedOrder.FxRate = todaysConversionRate.RateValue == 0 ? 0 : 1 / todaysConversionRate.RateValue;
                                        }
                                        else if (todaysConversionRate.ConversionMethod == Operator.M)
                                        {
                                            uncalculatedOrder.FxRate = todaysConversionRate.RateValue;
                                        }
                                        else if (todaysConversionRate.RateValue != 0 && todaysConversionRate.ConversionMethod == Operator.D)
                                        {
                                            uncalculatedOrder.FxRate = (1 / todaysConversionRate.RateValue);
                                        }
                                    }
                                }
                                ConversionRate yesterdaysConversionRate = null;
                                lock (_forexLocker)
                                {
                                    yesterdaysConversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(uncalculatedOrder.CurrencyID, latestAUECDate.AddDays(-1), uncalculatedOrder.Level1ID);
                                }
                                if (yesterdaysConversionRate != null)
                                {
                                    if (yesterdaysConversionRate.RateValue != 0 && yesterdaysConversionRate.ConversionMethod == Operator.D)
                                    {
                                        uncalculatedOrder.YesterdayFXRate = (1 / yesterdaysConversionRate.RateValue);
                                    }
                                    else
                                    {
                                        uncalculatedOrder.YesterdayFXRate = yesterdaysConversionRate.RateValue;
                                    }
                                }
                                if (!(uncalculatedOrder is EPnLOrderFXForward || uncalculatedOrder is EPnLOrderFX))
                                {
                                    switch (CachedDataManager.GetInstance.GetCurrencyText(uncalculatedOrder.CurrencyID))
                                    {
                                        case "EUR":
                                        case "GBP":
                                        case "NZD":
                                        case "AUD":
                                            uncalculatedOrder.FxRateDisplay = uncalculatedOrder.FxRate;
                                            break;

                                        default:
                                            uncalculatedOrder.FxRateDisplay = uncalculatedOrder.FxRate != 0 ? (1 / uncalculatedOrder.FxRate) : 0;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                uncalculatedOrder.YesterdayFXRate = 1;
                                uncalculatedOrder.FxRateDisplay = 1;
                                string currencyText = string.Empty;
                                if (uncalculatedOrder is EPnLOrderFXForward)
                                {
                                    EPnLOrderFXForward temp = uncalculatedOrder as EPnLOrderFXForward;
                                    int reqCurrency = temp.LeadCurrencyID.Equals(accountBaseCurrency) ? temp.VsCurrencyID : temp.LeadCurrencyID;
                                    currencyText = CachedDataManager.GetInstance.GetCurrencyText(reqCurrency);
                                    fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(uncalculatedOrder.Symbol, uncalculatedOrder.CurrencyID, accountBaseCurrency, AssetCategory.Forex);
                                    if (fxLevel1Data != null)
                                    {
                                        if (temp.LeadCurrencyID != accountBaseCurrency)
                                            temp.FxRate = fxLevel1Data.SelectedFeedPrice;
                                        else
                                            temp.FxRate = fxLevel1Data.SelectedFeedPrice != 0 ? 1 / fxLevel1Data.SelectedFeedPrice : 0;
                                    }
                                }
                                else if (uncalculatedOrder is EPnLOrderFX)
                                {
                                    EPnLOrderFX temp = uncalculatedOrder as EPnLOrderFX;
                                    int reqCurrency = temp.LeadCurrencyID.Equals(accountBaseCurrency) ? temp.VsCurrencyID : temp.LeadCurrencyID;
                                    currencyText = CachedDataManager.GetInstance.GetCurrencyText(reqCurrency);
                                    fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(uncalculatedOrder.Symbol, reqCurrency, accountBaseCurrency, AssetCategory.Forex);
                                    if (fxLevel1Data != null)
                                    {
                                        if (temp.LeadCurrencyID != accountBaseCurrency)
                                            temp.FxRate = fxLevel1Data.SelectedFeedPrice;
                                        else
                                            temp.FxRate = fxLevel1Data.SelectedFeedPrice != 0 ? 1 / fxLevel1Data.SelectedFeedPrice : 0;
                                    }
                                }
                                if (fxLevel1Data == null)
                                {
                                    uncalculatedOrder.FxRate = 1;
                                    uncalculatedOrder.FxRateDisplay = 0;
                                }
                                else if (!(uncalculatedOrder is EPnLOrderFXForward || uncalculatedOrder is EPnLOrderFX))
                                    uncalculatedOrder.FxRateDisplay = (fxLevel1Data.SelectedFeedPrice - fxLevel1Data.ForwardPoints);
                            }

                            //Trade date conversion rate kept to calculate the notional value.
                            //the following bool value is set true if FX rate is available with trade
                            //if not received with trade, this value is picked from Mark Prices, and * is appended in this case
                            if (!uncalculatedOrder.IsFXRateSavedWithTrade)
                            {
                                ConversionRate conversionRateTradeDt = null;
                                lock (_forexLocker)
                                {
                                    conversionRateTradeDt = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(uncalculatedOrder.CurrencyID, uncalculatedOrder.TransactionDate, uncalculatedOrder.Level1ID);
                                }
                                if (conversionRateTradeDt != null)
                                {
                                    if (uncalculatedOrder.FXRateOnTradeDate <= 0.0)
                                    {
                                        uncalculatedOrder.FXConversionMethodOnTradeDate = Operator.M;
                                        if (conversionRateTradeDt.ConversionMethod == Operator.D)
                                            uncalculatedOrder.FXRateOnTradeDate = conversionRateTradeDt.RateValue == 0 ? 0 : 1 / conversionRateTradeDt.RateValue;
                                        else
                                            uncalculatedOrder.FXRateOnTradeDate = conversionRateTradeDt.RateValue;
                                    }
                                    SetFXRateStringOnTradeDate(uncalculatedOrder, conversionRateTradeDt);
                                }
                            }
                            else
                            {
                                uncalculatedOrder.FXRateOnTradeDateStr = uncalculatedOrder.FXRateOnTradeDate.ToString();
                            }
                            Calculate(uncalculatedOrder);
                        }
                    });
                    //make the separate loop because in the parallel loop not maintain the order that's why PositionBeforeZero value is changing.
                    //and some time position side is changed 
                    // https://jira.nirvanasolutions.com:8443/browse/PRANA-32843
                    foreach (var uncalculatedOrder in uncalculatedOrderCollection)
                        _summaryCalculator.AddOrder(uncalculatedOrder, distinctAccountPermissionSets);
                    _summaryCalculator.CalculateDistinctPermissionSummaryUsingAccountSummary(distinctAccountPermissionSets);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private object _forexLocker = new object();
        //int count = 0;
        //int elapsed = 0;
        private void SetFXRateStringOnTradeDate(EPnlOrder exposureAndPnlOrder, ConversionRate conversionRateTradeDt)
        {
            try
            {
                if (conversionRateTradeDt.Date.Equals(DateTimeConstants.MinValue) && conversionRateTradeDt.RateValue > 0)
                {
                    exposureAndPnlOrder.FXRateOnTradeDateStr = "1";
                }
                else if (conversionRateTradeDt.Date.Equals(DateTimeConstants.MinValue) && conversionRateTradeDt.RateValue == 0)
                {
                    exposureAndPnlOrder.FXRateOnTradeDateStr = "0";
                }
                else
                {
                    exposureAndPnlOrder.FXRateOnTradeDateStr = exposureAndPnlOrder.FXRateOnTradeDate + "*";
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

        private void GetNAVandReturnValues(Dictionary<int, ExposureAndPnlOrderSummary> accountWiseSummary, bool isNAVsaved)
        {
            try
            {
                if (accountWiseSummary != null)
                {
                    foreach (ExposureAndPnlOrderSummary uncalculatedSummary in accountWiseSummary.Values)
                    {
                        if (!isNAVsaved)
                        {
                            // NetAssetValue is same as shadowNAV
                            uncalculatedSummary.NetAssetValue = uncalculatedSummary.CashProjected + uncalculatedSummary.NetMarketValue + uncalculatedSummary.StartOfDayAccruals + uncalculatedSummary.DayAccruals;
                            uncalculatedSummary.YesterdayNAV = uncalculatedSummary.StartOfDayCash + uncalculatedSummary.YesterdayNAV + uncalculatedSummary.StartOfDayAccruals;
                            if (uncalculatedSummary.NetAssetValue > 0)
                            {
                                uncalculatedSummary.PNLContributionPercentageSummary = uncalculatedSummary.DayPnL / uncalculatedSummary.NetAssetValue;
                            }
                            else
                            {
                                uncalculatedSummary.PNLContributionPercentageSummary = 0;
                            }
                        }
                        if (uncalculatedSummary.YesterdayNAV == 0)
                        {
                            uncalculatedSummary.DayReturn = 0;
                        }
                        else if (uncalculatedSummary.YesterdayNAV > 0)
                        {
                            uncalculatedSummary.DayReturn = uncalculatedSummary.DayPnL / uncalculatedSummary.YesterdayNAV;
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

        private void UpdateAccountSummariesFromCalculatedData(Dictionary<int, ExposureAndPnlOrderCollection> calculatedData)
        {
            try
            {
                if (calculatedData != null)
                {
                    foreach (KeyValuePair<int, ExposureAndPnlOrderCollection> temp in calculatedData)
                    {
                        for (int i = 0; i < temp.Value.Count; i++)
                        {
                            EPnlOrder epnlOrder = temp.Value[i];
                            _summaryCalculator.FillSummaryValuesInOrder(ref epnlOrder);

                            CalculatePercentUnderlyingGrossAndBetaAdjExposure(epnlOrder);
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

        private void CalculatePercentageExposuresandKeyReturns(Dictionary<int, ExposureAndPnlOrderSummary> accountwiseSummary)
        {
            try
            {
                if (accountwiseSummary != null)
                {
                    //All these values not calculated as percentage as specifying format on client grid will ensure this will be represented as a percentage
                    foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> var in accountwiseSummary)
                    {
                        if (var.Value.NetAssetValue > 0)
                        {
                            var.Value.NetPercentExposure = var.Value.NetExposure / var.Value.NetAssetValue;
                            var.Value.NetPercentBetaAdjExposure = var.Value.BetaAdjustedExposure / var.Value.NetAssetValue;
                            var.Value.NetPercentCashProjected = var.Value.CashProjected / var.Value.NetAssetValue;
                            var.Value.NetPercentGrossMktValue = var.Value.GrossMarketValue / var.Value.NetAssetValue;
                            var.Value.NetPercentLongMktValue = var.Value.LongMarketValue / var.Value.NetAssetValue;
                            var.Value.NetPercentShortMktValue = var.Value.ShortMarketValue / var.Value.NetAssetValue;
                            var.Value.PercentUnderlyingGrossExposure = var.Value.UnderlyingGrossExposure / var.Value.NetAssetValue;
                            var.Value.PercentNetMarketValue = var.Value.NetMarketValue / var.Value.NetAssetValue;
                            var.Value.PercentUnderlyingLongExposure = var.Value.UnderlyingLongExposure / var.Value.NetAssetValue;
                            var.Value.PercentUnderlyingShortExposure = var.Value.UnderlyingShortExposure / var.Value.NetAssetValue;
                            var.Value.NetPercentBetaAdjustedGrossExposureUnderlying = var.Value.BetaAdjustedGrossExposureUnderlying / var.Value.NetAssetValue;
                            var.Value.NetPercentBetaAdjustedLongExposureUnderlying = var.Value.BetaAdjustedLongExposureUnderlying / var.Value.NetAssetValue;
                            var.Value.NetPercentBetaAdjustedShortExposureUnderlying = var.Value.BetaAdjustedShortExposureUnderlying / var.Value.NetAssetValue;
                        }

                        if (var.Value.GrossMarketValue != 0)
                        {
                            var.Value.PercentNetMarketValueGrossMV = (var.Value.NetMarketValue / var.Value.GrossMarketValue);
                        }

                        if (var.Value.YesterdayNAV > 0)
                        {
                            var.Value.NetPercentDayPnLLong = var.Value.DayPnLLong / var.Value.YesterdayNAV;
                            var.Value.NetPercentDayPnLShort = var.Value.DayPnLShort / var.Value.YesterdayNAV;
                            var.Value.NetPercentDayPnLFX = var.Value.DayPnLFX / var.Value.YesterdayNAV;
                        }

                        if (var.Value.UnderlyingShortExposure != 0)
                        {
                            var.Value.LongShortExposureRatioUnderlying = Math.Abs(var.Value.UnderlyingLongExposure / var.Value.UnderlyingShortExposure);
                        }
                        if (var.Value.GrossMarketValue != 0)
                        {
                            var.Value.DayReturnGrossMarketValue = var.Value.DayPnL / var.Value.GrossMarketValue;
                        }

                        //var.Value.MTDReturn = (1 + var.Value.DayReturn) * (1 + var.Value.MTDReturn / 100) - 1;
                        //var.Value.QTDReturn = (1 + var.Value.DayReturn) * (1 + var.Value.QTDReturn / 100) - 1;
                        //var.Value.YTDReturn = (1 + var.Value.DayReturn) * (1 + var.Value.YTDReturn / 100) - 1;
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

        private void CalculatePercentUnderlyingGrossAndBetaAdjExposure(EPnlOrder epnlOrder)
        {
            try
            {
                if (epnlOrder.Asset.Equals(AssetCategory.FX) || epnlOrder.Asset.Equals(AssetCategory.FXForward))
                {
                    epnlOrder.BetaAdjGrossExposure = Math.Abs(epnlOrder.MarketValueInBaseCurrency);
                }
                else
                {
                    if (epnlOrder.LeveragedFactor != 0)
                    {
                        epnlOrder.BetaAdjGrossExposure = Math.Abs((epnlOrder.NetExposureInBaseCurrency / epnlOrder.LeveragedFactor)) * Math.Abs(epnlOrder.Beta);
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

        /// <summary>
        /// Clears the summary calculator cache.
        /// </summary>
        public void ClearSummaryCalculatorCache()
        {
            try
            {
                _summaryCalculator.ClearCache();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}