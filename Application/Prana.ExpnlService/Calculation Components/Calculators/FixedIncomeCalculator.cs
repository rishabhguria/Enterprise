using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.ExpnlService
{
    internal class FixedIncomeCalculator : Calculator
    {

        public FixedIncomeCalculator(Dictionary<int, DateTime> _auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            base.AUECWiseAdjustedDateTime = _auecWiseAdjustedCurrentDates;
            base.ClearanceDateTime = clearanceTimes;
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

                        #region Special handling for Convertible Bond Asset Class
                        if (order.Asset == AssetCategory.ConvertibleBond)
                        {
                            ((EPnLOrderFixedIncome)order).Delta = symbolLevel1Data.Delta;
                            ((EPnLOrderFixedIncome)order).DeltaSource = symbolLevel1Data.DeltaSource;
                        }
                        #endregion

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
            // Multiplied div yield by 100 as it is supplied in decimal terms everywhere
            if (symbolLevel1Data.DividendYield != double.MinValue)
            {
                order.DividendYield = symbolLevel1Data.FinalDividendYield * 100;
            }

            if (symbolLevel1Data.CategoryCode == AssetCategory.Equity)
            {
                if (((EquitySymbolData)symbolLevel1Data).SharesOutstanding != double.MinValue)
                {
                    order.SharesOutstanding = ((EquitySymbolData)symbolLevel1Data).SharesOutstanding;
                }
            }
            if (symbolLevel1Data.ForwardPoints != double.MinValue)
            {
                order.ForwardPoints = symbolLevel1Data.ForwardPoints;
            }
            order.LastUpdatedUTC = symbolLevel1Data.UpdateTime.ToLocalTime();
            order.TradeVolume = symbolLevel1Data.TotalVolume;

            #region Special handling for Convertible Bond Asset Class
            if (order.Asset == AssetCategory.ConvertibleBond)
            {
                if (String.IsNullOrEmpty(order.UnderlyingSymbol))
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Underlying Symbol for option symbol " + order.Symbol + " is empty ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                SymbolData l1UnderlyingData = LiveFeedManager.Instance.GetDynamicSymbolData(order.UnderlyingSymbol);
                if (l1UnderlyingData != null)
                {
                    order.UnderlyingStockPrice = l1UnderlyingData.SelectedFeedPrice;

                    order.YesterdayUnderlyingMarkPrice = l1UnderlyingData.MarkPrice;
                    order.YesterdayUnderlyingMarkPriceStr = l1UnderlyingData.MarkPriceStr;
                    order.AverageVolume20DayUnderlyingSymbol = l1UnderlyingData.AverageVolume20Day;
                }
            }

            #endregion
        }

        internal override void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                double costBasisPNL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                if (order.ClassID == EPnLClassID.EPnLOrderFixedIncome)
                {
                    order.CostBasisUnrealizedPnL = costBasisPNL;
                }

                order.CostBasisUnrealizedPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.FXRateOnTradeDate);

                double dayPNL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier) + order.EarnedDividendLocal;

                if (order.ClassID == EPnLClassID.EPnLOrderFixedIncome)
                {
                    order.DayPnL = dayPNL;
                }

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
                    order.EarnedDividendBase = order.EarnedDividendLocal * order.FxRate;
                    order.DayPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.YesterdayFXRate) + order.EarnedDividendBase;
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

        internal override void CalculateExposureAndMarketValue(EPnlOrder order)
        {
            try
            {
                //As avg price is not considered here, multiplier won't be divided by 100
                order.DeltaAdjPosition = BusinessLogic.Calculations.GetDeltaAdjustedPosition(order.Quantity, order.Multiplier, order.SideMultiplier, ((EPnLOrderFixedIncome)order).Delta);

                if (order.Asset == AssetCategory.ConvertibleBond && order.UnderlyingStockPrice == 0)
                {
                    order.NetExposure = 0;
                    order.SetNetExposureInBaseCurrency();
                }

                if (order.Asset == AssetCategory.ConvertibleBond)
                {
                    order.NetExposure = BusinessLogic.Calculations.GetNetExposure(order.Quantity, order.UnderlyingStockPrice, order.Multiplier, order.SideMultiplier, ((EPnLOrderFixedIncome)order).Delta, order.LeveragedFactor);
                }
                else
                {
                    order.NetExposure = BusinessLogic.Calculations.GetNetExposure(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier, ((EPnLOrderFixedIncome)order).Delta, order.LeveragedFactor);
                }
                order.SetNetExposureInBaseCurrency();

                order.Exposure = order.NetExposure;
                order.ExposureInBaseCurrency = order.NetExposureInBaseCurrency;

                order.BetaAdjExposure = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposure, order.Beta, order.LeveragedFactor);
                order.BetaAdjExposureInBaseCurrency = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposureInBaseCurrency, order.Beta, order.LeveragedFactor);

                order.MarketValue = BusinessLogic.Calculations.GetMarketValue(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier);
                order.SetMarketValueInBaseCurrency();

                if (order.MarketValueInBaseCurrency != 0)
                {
                    order.PercentDayPnLGrossMV = order.DayPnLInBaseCurrency / Math.Abs(order.MarketValueInBaseCurrency) * 100;
                    order.PercentDayPnLNetMV = order.DayPnLInBaseCurrency / order.MarketValueInBaseCurrency * 100;
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
                //if todays date, equal to negative of notional
                if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                {
                    order.CashImpact = -1 * order.NetNotionalValue;
                    order.SetCashImpactInBaseCurrency();
                }
                else // back date, cash file adjusted for this transaction
                {
                    order.CashImpact = 0.0;
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

        public override void Calculate_GenericFXDayPNL(EPnlOrder order, double selectedSecurityPriceLocal, double currentFXRate, double availablePreviousFxRate)
        {
            try
            {
                //Ashish and Gaurav 20120726 - Divide the price by 100 for fixed income trades.
                order.FxDayPnl = (order.Quantity) * (order.Multiplier) * order.SideMultiplier * (selectedSecurityPriceLocal) * (currentFXRate - availablePreviousFxRate);
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

        public override void CalculateFxCostBasisPnL(EPnlOrder order)
        {
            try
            {
                double previousFXRate = order.FXRateOnTradeDate;
                if (previousFXRate == double.MinValue)
                {
                    previousFXRate = 0.0;
                }

                order.FxCostBasisPnl = order.Quantity * (order.Multiplier) * order.SideMultiplier * order.AvgPrice * (order.FxRate - previousFXRate);
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

        public override void CalculateTradeCostBasisPnL(EPnlOrder order)
        {
            try
            {
                if (!order.FxRate.Equals(0))
                {
                    order.TradeCostBasisPnl = (order.SelectedFeedPrice - order.AvgPrice) * order.Quantity * (order.Multiplier) * order.SideMultiplier * order.FxRate;
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