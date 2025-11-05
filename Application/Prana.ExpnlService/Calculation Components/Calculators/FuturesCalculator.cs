using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.ExpnlService
{
    class FuturesCalculator : Calculator
    {
        bool _calculateFxGainLossOnFutures = true;
        bool _isM2MIncludedInCash = true;

        public FuturesCalculator(Dictionary<int, DateTime> auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            base.AUECWiseAdjustedDateTime = auecWiseAdjustedCurrentDates;
            base.ClearanceDateTime = clearanceTimes;
            _calculateFxGainLossOnFutures = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnFutures"));
            _isM2MIncludedInCash = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsM2MIncludedInCash"));
        }

        public override void CalculateFxCostBasisPnL(EPnlOrder order)
        {
            if (_calculateFxGainLossOnFutures)
            {
                base.CalculateFxCostBasisPnL(order);
            }
            else
            {
                return;
            }
        }

        public override void CalculateFxDayPnL(EPnlOrder order)
        {
            if (_calculateFxGainLossOnFutures)
            {
                base.CalculateFxDayPnL(order);
            }
            else
            {
                return;
            }
        }

        public override void OverridePnlInBaseCurrency(EPnlOrder order)
        {
            base.OverridePnlInBaseCurrency(order);
            order.DayPnLInBaseCurrency = order.TradeDayPnl + order.FxDayPnl;
            order.CostBasisUnrealizedPnLInBaseCurrency = order.TradeCostBasisPnl + order.FxCostBasisPnl;
            if (_isM2MIncludedInCash)
            {
                order.MarketValue = order.DayPnL;
                order.MarketValueInBaseCurrency = order.DayPnLInBaseCurrency;
            }
            else
            {
                order.MarketValue = order.CostBasisUnrealizedPnL;
                order.MarketValueInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;
            }

            if (order.MarketValueInBaseCurrency != 0)
            {
                order.PercentDayPnLGrossMV = order.DayPnLInBaseCurrency / Math.Abs(order.MarketValueInBaseCurrency) * 100;
                order.PercentDayPnLNetMV = order.DayPnLInBaseCurrency / order.MarketValueInBaseCurrency * 100;
            }
        }

        internal override double FillPriceInfo(EPnlOrder order)
        {
            double yesterdayMarkPrice = 0.0;
            try
            {
                if (base.ClearanceDateTime.ContainsKey(order.AUECID))
                {
                    SymbolData symbolLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(order.Symbol);
                    if (symbolLevel1Data != null)
                    {
                        order.YesterdayMarkPrice = symbolLevel1Data.MarkPrice;
                        order.YesterdayMarkPriceStr = symbolLevel1Data.MarkPriceStr;
                        order.YesterdayUnderlyingMarkPrice = symbolLevel1Data.MarkPrice;
                        order.YesterdayUnderlyingMarkPriceStr = symbolLevel1Data.MarkPriceStr;
                        yesterdayMarkPrice = order.YesterdayMarkPrice;
                        order.PricingSource = symbolLevel1Data.PricingSource;
                        order.PricingStatus = symbolLevel1Data.PricingStatus;
                        FillUpdatedLiveFeedData(order, symbolLevel1Data);
                    }
                    if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                    {
                        yesterdayMarkPrice = order.AvgPrice;
                    }

                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Clearance Time not available for AUEC ID : " + order.AUECID + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt"), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
            return yesterdayMarkPrice;
        }

        private void FillUpdatedLiveFeedData(EPnlOrder order, SymbolData symbolLevel1Data)
        {
            if (null != symbolLevel1Data)
            {
                if (symbolLevel1Data.AverageVolume20Day != double.MinValue)
                {
                    order.AverageVolume20Day = symbolLevel1Data.AverageVolume20Day;
                    order.AverageVolume20DayUnderlyingSymbol = symbolLevel1Data.AverageVolume20Day;
                }
                if (symbolLevel1Data.SelectedFeedPrice != double.MinValue)
                {
                    order.SelectedFeedPrice = symbolLevel1Data.SelectedFeedPrice;
                    order.UnderlyingStockPrice = symbolLevel1Data.SelectedFeedPrice;
                    order.SelectedFeedPriceInBaseCurrency = symbolLevel1Data.SelectedFeedPrice * order.FxRate;
                }
                //Fill the Ask, Bid & Last price of order on the basis of which calculation is done
                if (symbolLevel1Data.Ask != double.MinValue)
                {
                    order.AskPrice = symbolLevel1Data.Ask;
                }
                if (symbolLevel1Data.Bid != double.MinValue)
                {
                    order.BidPrice = symbolLevel1Data.Bid;
                }
                if (symbolLevel1Data.LastPrice != double.MinValue)
                {
                    order.LastPrice = symbolLevel1Data.LastPrice;
                }
                if (symbolLevel1Data.High != double.MinValue)
                {
                    order.HighPrice = symbolLevel1Data.High;
                }
                if (symbolLevel1Data.Low != double.MinValue)
                {
                    order.LowPrice = symbolLevel1Data.Low;
                }
                if (symbolLevel1Data.Mid != double.MinValue)
                {
                    order.MidPrice = symbolLevel1Data.Mid;
                }
                if (symbolLevel1Data.iMid != double.MinValue)
                {
                    order.iMidPrice = symbolLevel1Data.iMid;
                }
                if (symbolLevel1Data.Previous != double.MinValue)
                {
                    order.ClosingPrice = symbolLevel1Data.Previous;
                }
                if (symbolLevel1Data is FutureSymbolData)
                {
                    if (((FutureSymbolData)symbolLevel1Data).OpenInterest != double.MinValue)
                    {
                        order.SharesOutstanding = ((FutureSymbolData)symbolLevel1Data).SharesOutstanding;
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Incorrect symbol or asset class for Symbol: " + order.Symbol + ", Asset: " + order.Asset.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                if (symbolLevel1Data.ForwardPoints != double.MinValue)
                {
                    order.ForwardPoints = symbolLevel1Data.ForwardPoints;
                }
                order.LastUpdatedUTC = symbolLevel1Data.UpdateTime.ToLocalTime();
                order.TradeVolume = symbolLevel1Data.TotalVolume;
            }
        }

        internal override void CalculateExposureAndMarketValue(EPnlOrder order)
        {
            try
            {
                order.DeltaAdjPosition = BusinessLogic.Calculations.GetDeltaAdjustedPosition(order.Quantity, 1, order.SideMultiplier, 1);
                order.NetExposure = BusinessLogic.Calculations.GetNetExposure(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier, 1, order.LeveragedFactor);
                order.SetNetExposureInBaseCurrency();

                //Bharat Jangir (6 May 2015)
                //Handling of Currency Futures/Future Options
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8159
                if (((EPnLOrderFuture)order).IsCurrencyFuture)
                {
                    order.Exposure = BusinessLogic.Calculations.GetLocalExposureForFutureAndFutureOptions(order.Quantity, order.Multiplier, order.SideMultiplier, order.FxRate);
                    order.ExposureInBaseCurrency = BusinessLogic.Calculations.GetExposureInBaseCurrencyForFutureAndFutureOptions(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier, order.FxRate);
                }
                else
                {
                    order.Exposure = order.NetExposure;
                    order.ExposureInBaseCurrency = order.NetExposureInBaseCurrency;
                }

                order.BetaAdjExposure = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposure, order.Beta, order.LeveragedFactor);
                order.BetaAdjExposureInBaseCurrency = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposureInBaseCurrency, order.Beta, order.LeveragedFactor);
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

        public override void CalculateDeltaAdjustedLME(EPnlOrder order)
        {
            if (order.ExchangeName.ToUpper().Equals("COMX") && order.Symbol.ToUpper().StartsWith("HG") && (order.Asset == AssetCategory.Future || order.Asset == AssetCategory.FutureOption))
            {
                order.DeltaAdjPositionLME = order.DeltaAdjPosition * ApplicationConstants.DELTAADJPOSITION_LME_MULTIPLIER;
            }
            else
            {
                order.DeltaAdjPositionLME = order.DeltaAdjPosition;
            }
        }

        /// <summary>
        /// This method is responsible for PNL Calculation
        /// </summary>
        internal override void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                order.CostBasisUnrealizedPnL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                order.CostBasisUnrealizedPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.FXRateOnTradeDate);

                order.DayPnL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier);
                int accountBaseCurrency;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(order.Level1ID))
                {
                    accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[order.Level1ID];
                }
                else
                {
                    accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (order.CurrencyID != accountBaseCurrency)
                {
                    order.DayPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.YesterdayFXRate);
                }
                else
                {
                    order.SetDayPnLInBaseCurrency();
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

        internal override void CalculateCashImpact(EPnlOrder order)
        {
            try
            {
                order.CashImpact = 0;
                order.CashImpactInBaseCurrency = 0;
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

        internal override void CalculateMarketCap(EPnlOrder order)
        {
            try
            {
                order.MarketCapitalization = 0.0;
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

        internal override void CalculateNotional(EPnlOrder order)
        {
            try
            {
                order.NetNotionalValue = BusinessLogic.Calculations.GetNotional(order.Quantity, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                order.SetNetNotionalInCompanyBaseCurrency();
                order.NetNotionalForCostBasisBreakEven = order.NetNotionalValue;
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
    }
}