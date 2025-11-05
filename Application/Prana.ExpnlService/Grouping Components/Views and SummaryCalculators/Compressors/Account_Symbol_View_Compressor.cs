using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators.Compressors
{
    public class Account_Symbol_View_Compressor : GenericCompressor, ICompressor
    {

        public CompressedDataDictionaries GetData(Dictionary<int, ExposureAndPnlOrderCollection> calculatedTaxLots, ExposureAndPnlOrderCollection markedCollection, Dictionary<int, DistinctAccountSetWiseSummaryCollection> compressedDistinctAccountSetWiseSummaryCollection, Dictionary<int, ExposureAndPnlOrderSummary> compressedAccountSummaries)
        {
            CompressedDataDictionaries dictToReturn = new CompressedDataDictionaries();
            try
            {
                dictToReturn = GetCompressedData(calculatedTaxLots);
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
            return dictToReturn;
        }

        #region ICompressor Members
        public ExposurePnlCacheItemList GetContainingTaxlots(string compressedRowID, int accountID, DistinctAccountSetWiseSummaryCollection outputAccountSetWiseConsolidatedSummary)
        {
            ExposurePnlCacheItemList listOfEpnlCacheItems = new ExposurePnlCacheItemList();

            try
            {
                List<string> listOfTaxlotIds = DictOfTaxlots[compressedRowID];
                if (InputOrderCollection.ContainsKey(accountID))
                {
                    ExposureAndPnlOrderCollection accountWiseOrderCollection = InputOrderCollection[accountID];
                    EPnlOrder tempEpnlOrder;
                    ExposurePnlCacheItem uiObject;
                    foreach (string taxlotid in listOfTaxlotIds)
                    {
                        tempEpnlOrder = accountWiseOrderCollection[taxlotid];
                        uiObject = new ExposurePnlCacheItem();

                        switch (tempEpnlOrder.ClassID)
                        {
                            case EPnLClassID.EPnlOrder:
                                tempEpnlOrder.GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderEquity:
                                ((EPnLOrderEquity)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderOption:
                                ((EPnLOrderOption)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderFuture:
                                ((EPnLOrderFuture)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderFX:
                                ((EPnLOrderFX)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderEquitySwap:
                                ((EPnLOrderEquitySwap)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderFXForward:
                                ((EPnLOrderFXForward)tempEpnlOrder).GetBindableObject(uiObject);
                                break;

                            case EPnLClassID.EPnLOrderFixedIncome:
                                ((EPnLOrderFixedIncome)tempEpnlOrder).GetBindableObject(uiObject);
                                break;
                        }
                        uiObject.HasBeenSentToUser = 0;
                        FillOrderWithSummaryValues(outputAccountSetWiseConsolidatedSummary, uiObject);
                        listOfEpnlCacheItems.Add(uiObject);
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
            return listOfEpnlCacheItems;
        }

        #endregion ICompressor Members

        protected override void CalculateSpecificDetails(ExposurePnlCacheItem uiObject, EPnlOrder taxlot)
        {
            try
            {
                if (uiObject.ClosingPrice != null)
                {
                    if (uiObject.ClosingPrice != taxlot.ClosingPrice)
                    {
                        uiObject.ClosingPrice = null;
                    }
                }

                if (uiObject.AvgPrice != null)
                {
                    if (uiObject.OrderSideTagValue != taxlot.OrderSideTagValue)
                    {
                        uiObject.AvgPrice = null;
                    }
                    else
                    {
                        double cumulativeNotional = Math.Abs(uiObject.Quantity) * Convert.ToDouble(uiObject.AvgPrice) + taxlot.Quantity * taxlot.AvgPrice;
                        double cumulativeQuantity = Math.Abs(uiObject.Quantity) + taxlot.Quantity;
                        if (cumulativeQuantity != 0)
                        {
                            uiObject.AvgPrice = Math.Round(cumulativeNotional / cumulativeQuantity, 4);
                        }
                        else
                        {
                            uiObject.AvgPrice = null;
                        }
                    }
                }

                if (uiObject.CounterPartyName != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.CounterPartyName != taxlot.CounterPartyName)
                    {
                        uiObject.CounterPartyName = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.Description != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.Description != taxlot.Description)
                    {
                        uiObject.Description = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.ExchangeID != -1)
                {
                    if (uiObject.ExchangeID != taxlot.ExchangeID)
                    {
                        uiObject.ExchangeID = -1;
                        uiObject.Exchange = ApplicationConstants.C_Multiple;
                    }
                }

                if (taxlot.ExDividendDate != null)
                {
                    uiObject.ExDividendDate = taxlot.ExDividendDate;
                }

                if (uiObject.FeedPriceOperator != Operator.Multiple)
                {
                    if (uiObject.FeedPriceOperator != taxlot.FeedPriceOperator)
                    {
                        uiObject.FeedPriceOperator = Operator.Multiple;
                    }
                }

                if (uiObject.FXConversionMethodOperator != Operator.Multiple)
                {
                    if (uiObject.FXConversionMethodOperator != taxlot.FXConversionMethodOperator)
                    {
                        uiObject.FXConversionMethodOperator = Operator.Multiple;
                    }
                }

                if (uiObject.FxRate != null)
                {
                    if (uiObject.FxRate != taxlot.FxRate)
                    {
                        uiObject.FxRate = null;
                    }
                }

                if (uiObject.FXRateOnTradeDateStr != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.FXRateOnTradeDateStr != taxlot.FXRateOnTradeDateStr)
                    {
                        uiObject.FXRateOnTradeDateStr = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.Level2ID != int.MinValue)
                {
                    if (uiObject.Level2ID != taxlot.Level2ID)
                    {
                        uiObject.Level2ID = int.MinValue;
                        uiObject.Level2Name = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.MasterStrategyID != -1)
                {
                    if (uiObject.MasterStrategyID != taxlot.MasterStrategyID)
                    {
                        uiObject.MasterStrategyID = -1;
                        uiObject.MasterStrategy = ApplicationConstants.C_Multiple;
                    }
                }


                if (uiObject.OrderSideTagValue != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.OrderSideTagValue != taxlot.OrderSideTagValue)
                    {
                        uiObject.OrderSideTagValue = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeDate != null)
                {
                    if (uiObject.TradeDate != taxlot.TransactionDate)
                    {
                        uiObject.TradeDate = null;
                    }
                }

                if (uiObject.TransactionSide != PositionType.Multiple.ToString())
                {
                    if (uiObject.TransactionSide != taxlot.TransactionSide.ToString())
                    {
                        uiObject.TransactionSide = PositionType.Multiple.ToString();
                    }
                }

                if (uiObject.UserName != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.UserName != taxlot.UserName)
                    {
                        uiObject.UserName = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.FxRateDisplay != null)
                {
                    if (uiObject.FxRateDisplay != taxlot.FxRateDisplay)
                    {
                        uiObject.FxRateDisplay = null;
                    }
                }

                if (uiObject.TransactionType != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TransactionType != taxlot.TransactionType)
                    {
                        uiObject.TransactionType = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute1 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute1 != taxlot.TradeAttribute1)
                    {
                        uiObject.TradeAttribute1 = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute2 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute2 != taxlot.TradeAttribute2)
                    {
                        uiObject.TradeAttribute2 = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute3 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute3 != taxlot.TradeAttribute2)
                    {
                        uiObject.TradeAttribute3 = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute4 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute4 != taxlot.TradeAttribute2)
                    {
                        uiObject.TradeAttribute4 = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute5 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute5 != taxlot.TradeAttribute2)
                    {
                        uiObject.TradeAttribute5 = ApplicationConstants.C_Multiple;
                    }
                }

                if (uiObject.TradeAttribute6 != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.TradeAttribute6 != taxlot.TradeAttribute2)
                    {
                        uiObject.TradeAttribute6 = ApplicationConstants.C_Multiple;
                    }
                }

                foreach (var uiObj in uiObject.GetTradeAttributesAsDict())
                {
                    if (uiObj.Value != ApplicationConstants.C_Multiple && uiObj.Value != taxlot.GetTradeAttributeValue(uiObj.Key))
                    {
                        uiObject.SetTradeAttributeValue(uiObj.Key, ApplicationConstants.C_Multiple);
                    }
                }

                if (uiObject.PositionSideExposureBoxed != ApplicationConstants.C_Multiple)
                {
                    if (uiObject.PositionSideExposureBoxed != taxlot.PositionSideExposureBoxed.ToString())
                    {
                        uiObject.PositionSideExposureBoxed = ApplicationConstants.C_Multiple;
                    }
                }

                uiObject.YesterdayMarketValue += taxlot.YesterdayMarketValue;
                uiObject.YesterdayMarketValueInBaseCurrency += taxlot.YesterdayMarketValueInBaseCurrency;
                uiObject.CashImpact += taxlot.CashImpact;
                uiObject.CashImpactInBaseCurrency += taxlot.CashImpactInBaseCurrency;
                uiObject.CostBasisUnrealizedPnL += taxlot.CostBasisUnrealizedPnL;
                uiObject.CostBasisUnrealizedPnLInBaseCurrency += taxlot.CostBasisUnrealizedPnLInBaseCurrency;
                uiObject.FxCostBasisPnl += taxlot.FxCostBasisPnl;
                uiObject.FxDayPnl += taxlot.FxDayPnl;
                uiObject.TradeCostBasisPnl += taxlot.TradeCostBasisPnl;
                uiObject.TradeDayPnl += taxlot.TradeDayPnl;
                uiObject.DayPnL += taxlot.DayPnL;

                //Kashish G., PRANA-32392
                //TODO: Below code is short term soln. For permanent solution, the column data type should be changed to decimal...
                decimal sum_DayPNL = (decimal)uiObject.DayPnLInBaseCurrency;
                decimal DayPNL = (decimal)taxlot.DayPnLInBaseCurrency;
                decimal totalDayPNL = sum_DayPNL + DayPNL;
                uiObject.DayPnLInBaseCurrency = (double)totalDayPNL;

                uiObject.EarnedDividendBase += taxlot.EarnedDividendBase;
                uiObject.EarnedDividendLocal += taxlot.EarnedDividendLocal;
                uiObject.NetExposure += taxlot.NetExposure;
                uiObject.Exposure += taxlot.Exposure;
                uiObject.DeltaAdjPosition += taxlot.DeltaAdjPosition;
                uiObject.DeltaAdjPositionLME += taxlot.DeltaAdjPositionLME;
                uiObject.MarketValue += taxlot.MarketValue;

                //Kashish G., PRANA-32392
                //TODO: Below code is short term soln. For permanent solution, the column data type should be changed to decimal...
                decimal sum_MV = (decimal)uiObject.MarketValueInBaseCurrency;
                decimal MV = (decimal)taxlot.MarketValueInBaseCurrency;
                decimal totalMV = sum_MV + MV;
                uiObject.MarketValueInBaseCurrency = (double)totalMV;

                if (uiObject.NetNotionalValueInBaseCurrency + taxlot.NetNotionalValueInBaseCurrency != 0)
                {
                    uiObject.PercentageGainLoss = (uiObject.PercentageGainLoss * uiObject.NetNotionalValueInBaseCurrency + taxlot.NetNotionalValueInBaseCurrency * taxlot.PercentageGainLoss) / (uiObject.NetNotionalValueInBaseCurrency + taxlot.NetNotionalValueInBaseCurrency);
                    uiObject.PercentageGainLossCostBasis = (uiObject.PercentageGainLossCostBasis * uiObject.NetNotionalValueInBaseCurrency + taxlot.NetNotionalValueInBaseCurrency * taxlot.PercentageGainLossCostBasis) / (uiObject.NetNotionalValueInBaseCurrency + taxlot.NetNotionalValueInBaseCurrency);
                }

                //Kashish G., PRANA-32392
                //TODO: Below code is short term soln. For permanent solution, the column data type should be changed to decimal...
                decimal sum_NetExp = (decimal)uiObject.NetExposureInBaseCurrency;
                decimal NetExp = (decimal)taxlot.NetExposureInBaseCurrency;
                decimal totalNetExp = sum_NetExp + NetExp;
                uiObject.NetExposureInBaseCurrency = (double)totalNetExp;

                uiObject.ExposureInBaseCurrency += taxlot.ExposureInBaseCurrency;
                uiObject.NetNotionalValue += taxlot.NetNotionalValue;
                uiObject.NetNotionalValueInBaseCurrency += taxlot.NetNotionalValueInBaseCurrency;
                uiObject.BetaAdjExposure += taxlot.BetaAdjExposure;
                uiObject.BetaAdjExposureInBaseCurrency += taxlot.BetaAdjExposureInBaseCurrency;
                uiObject.PercentageAverageVolume += taxlot.PercentageAverageVolume;
                uiObject.PercentageAverageVolumeDeltaAdjusted += taxlot.PercentageAverageVolumeDeltaAdjusted;
                uiObject.PremiumDollar += taxlot.PremiumDollar;
                uiObject.GrossExposure = Math.Abs(uiObject.NetExposureInBaseCurrency);
                uiObject.GrossExposureLocal = Math.Abs(uiObject.NetExposure);
                uiObject.GrossMarketValue = Math.Abs(uiObject.MarketValueInBaseCurrency);
                uiObject.LastPrice = taxlot.LastPrice;
                uiObject.LastUpdatedUTC = taxlot.LastUpdatedUTC;

                if (uiObject.Asset.Equals(AssetCategory.FX.ToString()) || uiObject.Asset.Equals(AssetCategory.FXForward.ToString()))
                {
                    uiObject.BetaAdjGrossExposure = Math.Abs(uiObject.NetExposureInBaseCurrency) * Math.Abs(uiObject.Beta);
                }
                else
                {
                    if (uiObject.LeveragedFactor != 0)
                    {
                        uiObject.BetaAdjGrossExposure = (Math.Abs((uiObject.NetExposureInBaseCurrency / taxlot.LeveragedFactor)) * Math.Abs(uiObject.Beta));
                    }
                }

                uiObject.Quantity = uiObject.Quantity + taxlot.SideMultiplier * taxlot.Quantity;
                uiObject.SideMultiplier = uiObject.Quantity >= 0 ? 1 : -1;
                uiObject.NetNotionalForCostBasisBreakEven += taxlot.NetNotionalForCostBasisBreakEven;

                if (uiObject.CostBasisBreakEven != null)
                {
                    if (uiObject.Quantity != 0)
                    {
                        double multiplier;
                        if (double.TryParse(uiObject.Multiplier.ToString(), out multiplier))
                        {
                            uiObject.CostBasisBreakEven = (uiObject.NetNotionalForCostBasisBreakEven / (uiObject.Quantity * multiplier));
                        }
                    }
                    else
                    {
                        uiObject.CostBasisBreakEven = null;
                    }
                }

                switch (taxlot.ClassID)
                {
                    case EPnLClassID.EPnLOrderEquitySwap:
                        uiObject.DayInterest += ((EPnLOrderEquitySwap)taxlot).DayInterest;
                        uiObject.TotalInterest += ((EPnLOrderEquitySwap)taxlot).TotalInterest;
                        break;
                    case EPnLClassID.EPnLOrderOption:
                        uiObject.UnderlyingValueForOptions += taxlot.UnderlyingValueForOptions;
                        break;
                }

                uiObject.NavTouch = taxlot.NavTouch;
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

        protected override string getKey(EPnlOrder taxlot)
        {
            string account_Symbol_key = String.Empty;
            try
            {
                account_Symbol_key = taxlot.Level1ID + taxlot.Symbol;
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
            return account_Symbol_key;
        }
    }
}
