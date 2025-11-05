using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.PM.Client.UI
{
    public static class CustomSummariesFactory
    {
        public static ICustomSummaryCalculator GetSummaryFromName(string summaryTypeName, string summaryColName)
        {
            ICustomSummaryCalculator summaryToReturn;
            try
            {
                switch (summaryTypeName)
                {
                    case "Sum":
                        summaryToReturn = new Sum(summaryColName);
                        break;
                    case "Text":
                        summaryToReturn = new TextSummary(summaryColName);
                        break;
                    case "Gross":
                        summaryToReturn = new GrossCalculator(summaryColName);
                        break;
                    default:
                        summaryToReturn = new TextSummary(summaryColName);
                        break;
                }
                return summaryToReturn;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }

        }

        #region Custom Summary Calculator Class Normal Sum
        public class Sum : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private string _summaryColName = string.Empty;

            internal Sum(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (value == null)
                    {
                        return;
                    }
                    if (!String.IsNullOrEmpty(value.ToString()))
                    {
                        string quantity = value.ToString() ?? string.Empty;
                        if (quantity.Equals(string.Empty))
                        {
                            return;
                        }

                        double nQuantity = 0.0;
                        if (quantity != PMConstants.doubleEpsilonValString || quantity.ToString() != PMConstants.intMinValString)
                        {
                            nQuantity = Convert.ToDouble(quantity);
                        }
                        this.totals += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (double.IsNaN(this.totals))
                {
                    return null;
                }
                else if (double.IsInfinity(this.totals))
                {
                    return null;
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class Position Summary [Position]
        public class PositionSummary : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private bool isMultiple = false;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            internal PositionSummary(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.previousSymbol = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    string symbol = string.Empty;
                    object valueSymbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]);
                    if (valueSymbol == null)
                    {
                        return;
                    }
                    symbol = valueSymbol.ToString();

                    if (previousSymbol == string.Empty)
                    {
                        previousSymbol = symbol;
                    }

                    if (symbol != previousSymbol || symbol == ApplicationConstants.C_Multiple)
                    {
                        isMultiple = true;
                        return;
                    }

                    string quantity = string.Empty;
                    object quantityValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (quantityValue == null)
                    {
                        return;
                    }

                    quantity = quantityValue.ToString();
                    if (string.IsNullOrEmpty(quantity))
                    {
                        isUndefined = true;
                        return;
                    }

                    double nQuantity = 0.0;

                    if (quantity != string.Empty || quantity != PMConstants.doubleEpsilonValString || quantity.ToString() != PMConstants.intMinValString)// || nSideMultiplier != 0)
                    {
                        nQuantity = Convert.ToDouble(quantity);
                    }
                    this.totals += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    if ((summarySettings.SourceColumn.Key.Equals(PMConstants.COL_PercentageAverageVolumeDeltaAdjusted)) || (summarySettings.SourceColumn.Key.Equals(PMConstants.COL_PercentageAverageVolume)))
                        return Math.Abs(this.totals);
                    else
                        return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderTotalsSummary [avg price]
        public class OrderTotalsSummary : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private double totalQty = 0.0;
            private bool isMultiple = false;
            private string previousSide = string.Empty;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;
            private string _summaryColumn = string.Empty;

            internal OrderTotalsSummary(string summaryColumn)
            {
                _summaryColumn = summaryColumn;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.totalQty = 0;
                this.previousSide = string.Empty;
                this.previousSymbol = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple || isUndefined)
                    {
                        return;
                    }

                    string side = string.Empty;
                    object sideValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_SideName]);
                    if (sideValue == null)
                    {
                        return;
                    }
                    side = sideValue.ToString();

                    string symbol = string.Empty;
                    object symbolValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]);
                    if (symbolValue == null)
                    {
                        return;
                    }
                    symbol = symbolValue.ToString();

                    if (previousSide == string.Empty && previousSymbol == string.Empty)
                    {
                        previousSide = side;
                        previousSymbol = symbol;
                    }
                    if (_summaryColumn != PMConstants.COL_CostBasisBreakEven)
                    {
                        if (side != previousSide || symbol != previousSymbol)
                        {
                            isMultiple = true;
                            return;
                        }
                    }
                    else
                    {
                        if (symbol != previousSymbol || symbol == ApplicationConstants.C_Multiple)
                        {
                            isMultiple = true;
                            return;
                        }
                    }

                    if (side == string.Empty || symbol == string.Empty)
                    {
                        return;
                    }
                    object unitPrice = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_AvgPrice]) ?? null;
                    object netNotional = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_NetNotionalForCostBasisBreakEven]) ?? null;
                    object quantity = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Quantity]) ?? null;

                    //if any of the value in the row is blank then we wil display undefined. If values are different then we'll use Multiple

                    if (unitPrice == null)
                    {
                        if (_summaryColumn != PMConstants.COL_CostBasisBreakEven)
                        {
                            isMultiple = true;
                            return;
                        }
                    }
                    object rowMultiplier;
                    rowMultiplier = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Multiplier]) ?? null;
                    if (rowMultiplier == null)
                    {
                        isUndefined = true;
                        return;
                    }
                    if (unitPrice == null || quantity == null)
                    {
                        isUndefined = true;
                        return;
                    }

                    double nUnitPrice = 0.0;
                    double nQuantity = 0.0;
                    double multiplier = 0.0;
                    double nNetNotional = 0.0;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != PMConstants.doubleEpsilonValString || unitPrice.ToString() != PMConstants.intMinValString)
                    {
                        double.TryParse(unitPrice.ToString(), out nUnitPrice);
                    }
                    if (quantity.ToString() != string.Empty || quantity.ToString() != PMConstants.doubleEpsilonValString || quantity.ToString() != PMConstants.intMinValString)
                    {
                        nQuantity = Convert.ToDouble(quantity);
                    }
                    if (_summaryColumn == PMConstants.COL_CostBasisBreakEven)
                    {
                        if (rowMultiplier.ToString() != string.Empty)
                        {
                            double.TryParse(rowMultiplier.ToString(), out multiplier);
                        }
                        if (netNotional.ToString() != string.Empty || netNotional.ToString() != PMConstants.doubleEpsilonValString || netNotional.ToString() != PMConstants.intMinValString)
                        {
                            nNetNotional = double.Parse(netNotional.ToString());
                        }
                    }

                    if (_summaryColumn == PMConstants.COL_CostBasisBreakEven)
                    {
                        this.totals += nNetNotional;

                        //Math.Round is applied because nQuantity * multiplier is giving incorrect results
                        //50 * 2.3 is showing = 114.99999999999999 so we are rounding its from 10th decimal place
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-11197
                        this.totalQty += Math.Round(nQuantity * multiplier, 10);
                    }
                    else
                    {
                        this.totals += nQuantity * nUnitPrice;
                        this.totalQty += nQuantity;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                double avgPrice = 0.0;
                try
                {
                    if (isMultiple)
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                    else if (isUndefined == true)
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                    // This gets called when the every row has been processed so here is where we
                    // would return the calculated summary value.
                    else if (this.totalQty != 0 && isMultiple == false)
                    {
                        avgPrice = this.totals / this.totalQty;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
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
                return avgPrice;
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderExposure [Exposure]
        public class OrderExposure : ICustomSummaryCalculator
        {
            private decimal totals = 0;
            private bool isMultiple = false;
            private string previousCurrency = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            /// <summary>
            /// Only pass NetExposure or NetExposureBase
            /// </summary>
            /// <param name="summaryColName"></param>
            internal OrderExposure(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    string currency = string.Empty;
                    object currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_CurrencySymbol]);
                    if (currencyValue == null)
                    {
                        return;
                    }

                    if (_summaryColName == PMConstants.COL_Exposure)
                    {
                        currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_RiskCurrency]);
                        if (currencyValue == null)
                        {
                            return;
                        }
                    }

                    currency = currencyValue.ToString();

                    if (String.IsNullOrEmpty(currency))
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        previousCurrency = currency;
                    }
                    if (currency != previousCurrency)
                    {
                        ///If this is day pnl local then don't add summaries otherwise add in case of base currency column
                        if (_summaryColName == PMConstants.COL_NetExposure
                            || _summaryColName == PMConstants.COL_MarketValue
                            || _summaryColName == PMConstants.COL_DayInterest
                            || _summaryColName == PMConstants.COL_TotalInterest
                            || _summaryColName == PMConstants.COL_CostBasisUnRealizedPNL
                            || _summaryColName == PMConstants.COL_BetaAdjExposure
                            || _summaryColName == PMConstants.COL_YesterdayMarketValue
                            || _summaryColName == PMConstants.COL_Exposure)
                        {
                            isMultiple = true;
                            return;
                        }
                    }

                    string exposure = string.Empty;
                    object exposureValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (exposureValue == null)
                    {
                        return;
                    }
                    exposure = exposureValue.ToString();

                    //string asset = string.Empty;
                    object assetValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Asset]);
                    if (assetValue == null)
                    {
                        return;
                    }
                    // asset = assetValue.ToString();

                    if (exposure == null || exposure.Equals(string.Empty) || exposure == PMConstants.doubleMinValString)
                    {
                        isUndefined = true;
                        return;
                    }
                    double nExposure1 = 0.0;

                    if (_summaryColName != PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency)
                    {
                        nExposure1 = Convert.ToDouble(exposure);
                    }
                    else if (exposure.ToString() != string.Empty || exposure.ToString() != PMConstants.doubleEpsilonValString || exposure.ToString() != PMConstants.intMinValString)
                    {
                        nExposure1 = Convert.ToDouble(exposure);
                    }

                    this.totals += (decimal)nExposure1;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderNotional [Notional]
        public class OrderNotional : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private bool isMultiple = false;
            private string previousCurrency = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            /// <summary>
            /// Only pass NetExposure or NetExposureBase
            /// </summary>
            /// <param name="summaryColName"></param>
            internal OrderNotional(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    string currency = string.Empty;
                    object currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_CurrencySymbol]);
                    if (currencyValue == null)
                    {
                        return;
                    }
                    currency = currencyValue.ToString();
                    if (currency == null || currency == string.Empty)
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        previousCurrency = currency;
                    }

                    if (currency != previousCurrency)
                    {
                        //It should only return for the net notional value local, but for the base currency it should just add up.
                        if (_summaryColName == PMConstants.COL_NetNotionalValue)
                        {
                            isMultiple = true;
                            return;
                        }

                        if (_summaryColName == PMConstants.COL_NetNotionalForCostBasisBreakEven)
                        {
                            isMultiple = true;
                            return;
                        }
                    }

                    string notional = string.Empty;
                    object notionalValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (notionalValue == null)
                    {
                        return;
                    }
                    notional = notionalValue.ToString();

                    if (notional == null || notional.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    // string asset = string.Empty;
                    object assetValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Asset]);
                    if (assetValue == null)
                    {
                        return;
                    }
                    //asset = assetValue.ToString();
                    double nNotional = 0.0;

                    if (notional != string.Empty && notional != PMConstants.doubleEpsilonValString && notional != PMConstants.doubleMinValString)
                    {
                        nNotional = Convert.ToDouble(notional);
                    }

                    this.totals += nNotional;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                // This gets called when the every row has been processed so here is where we 
                // would return the calculated summary value. 
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderPNL [PNL]
        public class OrderPNL : ICustomSummaryCalculator
        {
            private decimal totals = 0;
            private double _totalsDenominator = 0.0;

            private bool isMultiple = false;
            private string previousCurrency = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            internal OrderPNL(string summaryColName)
            {
                if (!summaryColName.Equals(PMConstants.COL_DayPnL) && !summaryColName.Equals(PMConstants.COL_DayPnLInBaseCurrency)
                    && !summaryColName.Equals(PMConstants.COL_CashImpact) && !summaryColName.Equals(PMConstants.COL_CashImpactInBaseCurrency)
                    && !summaryColName.Equals(PMConstants.COL_EarnedDividendBase) && !summaryColName.Equals(PMConstants.COL_EarnedDividendLocal)
                     && !summaryColName.Equals(PMConstants.COL_GainLossIfExerciseAssign))
                {
                    throw new Exception("Wrong column name supplied for PNL related class");
                }
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this._totalsDenominator = 0;
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    string currency = string.Empty;
                    object currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_CurrencySymbol]);
                    if (currencyValue == null)
                    {
                        return;
                    }
                    currency = currencyValue.ToString();

                    if (currency == null || currency == string.Empty)
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        previousCurrency = currency;
                    }

                    if (currency != previousCurrency)
                    {
                        ///If this is day pnl local then don't add summaries otherwise add in case of base currency column
                        if (_summaryColName == PMConstants.COL_DayPnL || _summaryColName == PMConstants.COL_CashImpact || _summaryColName == PMConstants.COL_EarnedDividendLocal)
                        {
                            isMultiple = true;
                            return;
                        }
                    }
                    object pnl = 0;
                    object denominator = 0;

                    pnl = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]) ?? string.Empty;

                    if (pnl is DBNull || pnl.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }
                    if (denominator is DBNull || denominator.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    double nPNL1 = 0.0;
                    double ndenominator = 0.0;

                    //string asset = string.Empty;
                    object assetValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Asset]);
                    if (assetValue == null)
                    {
                        return;
                    }
                    //asset = assetValue.ToString();
                    if (_summaryColName == PMConstants.COL_DayPnL || _summaryColName == PMConstants.COL_CashImpact)
                    {
                        nPNL1 = Convert.ToDouble(pnl);
                    }
                    else
                    {
                        if (pnl.ToString() != string.Empty || pnl.ToString() != PMConstants.doubleEpsilonValString || pnl.ToString() != PMConstants.intMinValString)
                        {
                            nPNL1 = Convert.ToDouble(pnl);
                        }
                        if (denominator.ToString() != string.Empty || denominator.ToString() != PMConstants.doubleEpsilonValString || denominator.ToString() != PMConstants.intMinValString)
                        {
                            ndenominator = Convert.ToDouble(denominator);
                        }
                    }

                    this.totals += (decimal)nPNL1;
                    this._totalsDenominator += ndenominator;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                // This gets called when the every row has been processed so here is where we
                // would return the calculated summary value.
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class PercentagePositionLongSummary [Percentage Position Long]
        public class PercentagePositionLongSummary : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private double totalQty = 0.0;
            private bool isMultiple = false;
            private string previousSide = string.Empty;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;

            internal PercentagePositionLongSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.totalQty = 0;
                this.previousSide = string.Empty;
                this.previousSymbol = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    string symbol = string.Empty;
                    object symbolValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]);
                    if (symbolValue == null)
                    {
                        return;
                    }
                    symbol = symbolValue.ToString();
                    if (previousSide == string.Empty && previousSymbol == string.Empty)
                    {
                        previousSymbol = symbol;
                    }

                    object unitPrice = null;
                    if ((summarySettings.SourceColumn.Key.Equals(PMConstants.COL_PercentageGainLossCostBasis)))
                    {
                        unitPrice = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_PercentageGainLossCostBasis]);
                    }
                    else
                    {
                        unitPrice = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_PercentageGainLoss]);
                    }
                    object unitNotionalBase = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_NetNotionalValueBase]);

                    if (symbol == null || symbol == string.Empty)
                    {
                        return;
                    }

                    if (unitPrice is DBNull || unitPrice.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }
                    if (unitNotionalBase is DBNull || unitNotionalBase.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    double nUnitPrice = 0.0;
                    double nUnitNotional = 0.0;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != PMConstants.doubleEpsilonValString || unitPrice.ToString() != PMConstants.intMinValString)
                    {
                        nUnitPrice = Convert.ToDouble(unitPrice);
                        if (double.IsNaN(nUnitPrice))
                        {
                            nUnitPrice = 0.0;
                        }
                    }
                    if (unitNotionalBase.ToString() != string.Empty || unitNotionalBase.ToString() != PMConstants.doubleEpsilonValString || unitNotionalBase.ToString() != PMConstants.doubleMinValString)
                    {
                        nUnitNotional = Convert.ToDouble(unitNotionalBase);
                    }
                    this.totals += nUnitPrice * nUnitNotional;
                    this.totalQty += nUnitNotional;
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
            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                if (rows.Count == 0)
                {
                    return 0;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    if (this.totalQty != 0.0)
                    {
                        return this.totals / this.totalQty;
                    }
                    else
                    {
                        return 0.0;
                    }
                }
            }
        }
        #endregion

        #region Custom  Class TextColumnsSummary [Text Columns Summary]
        public class TextSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;
            private string current = string.Empty;
            private string previous = string.Empty;
            private string _columnName = string.Empty;
            //private string key = string.Empty;
            private string previousSymbol = string.Empty;

            internal TextSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.current = string.Empty;
                this.previous = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
                previousSymbol = string.Empty;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);
                    if (value == null)
                    {
                        return;
                    }

                    if (_columnName == PMConstants.COL_PositionSideExposure || _columnName == PMConstants.COL_PositionSideMV || _columnName == PMConstants.COL_PositionSideExposureUnderlying)
                    {
                        string symbol = string.Empty;
                        object valueSymbol;
                        if (_columnName == PMConstants.COL_PositionSideExposureUnderlying)
                        {
                            valueSymbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_UnderlyingSymbol]);
                        }
                        else
                        {
                            valueSymbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]);

                        }
                        if (valueSymbol == null)
                        {
                            return;
                        }
                        symbol = valueSymbol.ToString();
                        if (previousSymbol == string.Empty)
                        {
                            previousSymbol = symbol;
                        }

                        if (symbol != previousSymbol || symbol == ApplicationConstants.C_Multiple)
                        {
                            isMultiple = true;
                            return;
                        }
                    }
                    if (String.IsNullOrEmpty(value.ToString()) == true || value == DBNull.Value)
                    {
                        isUndefined = true;
                        return;
                    }

                    current = value.ToString();
                    if (previous == string.Empty)
                    {
                        if (current == string.Empty || current == null)
                        {
                            isUndefined = true;
                            return;
                        }

                        previous = current.ToString();
                    }

                    if (current != previous)
                    {
                        isMultiple = true;
                        return;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    switch (_columnName)
                    {
                        case PMConstants.COL_FXRateOnTradeDateStr:
                        case PMConstants.COL_YesterdayMarkPriceStr:
                        case PMConstants.COL_YesterdayUnderlyingMarkPriceStr:
                            return PMConstants.SUMMARY_DASH;
                    }

                    return PMConstants.SUMMARY_MULTIPLE;
                }
                else if (isUndefined)
                {
                    if (current != string.Empty)
                    {
                        switch (_columnName)
                        {
                            case PMConstants.COL_FXRateOnTradeDateStr:
                                return PMConstants.SUMMARY_DASH;
                        }
                        return PMConstants.SUMMARY_MULTIPLE;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_UNDEFINED;
                    }
                }
                else
                {
                    return current;
                }
            }
        }
        #endregion

        #region Custom  Class IdenticalNumber [IdenticalNumber Columns Summary]
        public class IdenticalNumberSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;
            private double current = 0.0;
            private double previous = double.MinValue;
            private string _columnName = string.Empty;

            internal IdenticalNumberSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.current = 0.0;
                this.previous = double.MinValue;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple || isUndefined)
                    {
                        return;
                    }
                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);

                    if (_columnName.Equals(PMConstants.COL_AskPrice) || _columnName.Equals(PMConstants.COL_BidPrice) ||
                        _columnName.Equals(PMConstants.COL_MidPrice) ||
                        _columnName.Equals(PMConstants.COL_DividendYield) || _columnName.Equals(PMConstants.COL_LastPrice) ||
                        _columnName.Equals(PMConstants.COL_PercentageChange))
                    {
                        if (value == null)
                        {
                            isMultiple = true;
                        }
                    }
                    else if (_columnName.Equals(PMConstants.COL_FXRateDisplay) || _columnName.Equals(PMConstants.COL_ClosingPrice) || _columnName.Equals((PMConstants.COL_FXRate)))
                    {
                        if (value == null)
                        {
                            isMultiple = true;
                        }
                    }
                    else
                    {
                        if (value == null)
                        {
                            return;
                        }
                    }

                    if (value.ToString() == ApplicationConstants.C_Multiple)
                    {
                        isMultiple = true;
                        return;
                    }

                    if ((String.IsNullOrEmpty(value.ToString()) == true) || value == DBNull.Value)
                    {
                        isUndefined = true;
                        return;
                    }

                    //Bharat Kumar Jangir (14 October 2013)
                    //Handling for Premium column values
                    if (value.ToString() == PMConstants.SUMMARY_DASH)
                    {
                        isMultiple = true;
                        return;
                    }

                    bool isDoubleValue = double.TryParse(value.ToString(), out current);
                    if (previous == double.MinValue)
                    {
                        if (current == 0.0)
                            previous = 0.0;
                        else
                            previous = current;
                        return;
                    }
                    if (!isDoubleValue)
                    {
                        current = 0.0;
                    }

                    if (previous == 0.0)
                    {
                        if (current == 0.0)
                        {
                            return;
                        }
                    }

                    if (current != previous)
                    {
                        isMultiple = true;
                        previous = current;
                        return;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined)
                {
                    return 0.0;
                }
                else
                {
                    return current;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class OrderDelta [Delta]
        public class OrderDelta : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private bool isMultiple = false;
            private string previousValue = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            internal OrderDelta(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.previousValue = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }

                    object delta = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (delta == null)
                    {
                        return;
                    }

                    if (String.IsNullOrEmpty(delta.ToString()) == true)
                    {
                        return;
                    }

                    if (previousValue == string.Empty)
                    {
                        previousValue = delta.ToString();
                    }

                    if ((delta.ToString() != previousValue) || delta.Equals(ApplicationConstants.DEFAULTDELTA))
                    {
                        isMultiple = true;
                        return;
                    }

                    object deltaNet = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]) ?? string.Empty;

                    if (deltaNet is DBNull || deltaNet.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    if (delta.ToString().Equals(PMConstants.SUMMARY_MULTIPLE))
                    {
                        isMultiple = true;
                        return;
                    }

                    double nDelta = 0.0;

                    if (delta.ToString() != string.Empty || delta.ToString() != PMConstants.doubleEpsilonValString
                        || delta.ToString() != PMConstants.intMinValString
                        || delta.ToString() != "0.0")
                    {
                        nDelta = Convert.ToDouble(delta);
                    }

                    this.totals = nDelta;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class PNLContribution Summary [PNLBontributionBP]
        public class PNLContributionBPSummary : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;
            private bool isMultiple = false;

            internal PNLContributionBPSummary(string summaryColName)
            {
                if (!summaryColName.Equals(PMConstants.COL_ExposureBPInBaseCurrency)
                    && !summaryColName.Equals(PMConstants.COL_PercentExposureInBaseCurrency)
                     && !summaryColName.Equals(PMConstants.COL_DayReturn)
                    && !summaryColName.Equals(PMConstants.COL_PercentagePNLContribution)
                    && !summaryColName.Equals(PMConstants.COL_PercentNetExposureInBaseCurrency)
                    && !summaryColName.Equals(PMConstants.COL_PercentNetMarketValueInBaseCurrency))
                {
                    throw new Exception("Wrong column name supplied for Notional related class");
                }
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.totals = 0;
                this.isUndefined = false;
                this.isMultiple = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    object pnlcontri = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (pnlcontri == null)
                    {
                        return;
                    }

                    if (pnlcontri.ToString() == ApplicationConstants.C_Multiple)
                    {
                        isMultiple = true;
                        return;
                    }

                    if (pnlcontri is DBNull || pnlcontri.Equals(string.Empty) || pnlcontri.ToString() == ApplicationConstants.C_Multiple)
                    {
                        isUndefined = true;
                        return;
                    }


                    double npnlcontribution = 0.0;

                    if (pnlcontri.ToString() != string.Empty || pnlcontri.ToString() != PMConstants.doubleEpsilonValString || pnlcontri.ToString() != PMConstants.intMinValString)
                    {
                        npnlcontribution = Convert.ToDouble(pnlcontri);
                    }

                    this.totals += npnlcontribution;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }

                if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return PMConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class Gross Calculations [Returns Abs on Symbol]
        public class GrossCalculator : ICustomSummaryCalculator
        {
            private string _summaryColName = string.Empty;
            private decimal _totals = 0;
            private bool isMultiple = false;
            private bool _isNetRequired = false;
            private string previousCurrency = string.Empty;
            private Dictionary<string, decimal> _symbolWiseExposure;
            private Dictionary<string, double> _symbolWiseBeta;
            private List<string> _symbolList;
            private double _nav = 0.0;
            private double _position = 0.0;

            /// <summary>
            /// Only pass NetExposure or NetExposureBase
            /// </summary>
            /// <param name="summaryColName"></param>
            internal GrossCalculator(string summaryColName)
            {
                _summaryColName = summaryColName;
                if (_summaryColName == PMConstants.COL_PercentDayPnLNetMV)
                    _isNetRequired = true;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this._symbolWiseExposure = new Dictionary<string, decimal>();
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this._totals = 0;
                this._symbolList = new List<string>();
                this._symbolWiseBeta = new Dictionary<string, double>();
                this._position = 0.0;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    string symbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]).ToString() ?? string.Empty;
                    string underlyingSymbol = string.Empty;
                    if (_summaryColName == PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency || _summaryColName == PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency ||
                        _summaryColName == PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency || _summaryColName == PMConstants.COL_UnderlyingGrossExposureInBaseCurrency ||
                        _summaryColName == PMConstants.COL_BetaAdjGrossExposureUnderlying || _summaryColName == PMConstants.COL_UnderlyingGrossExposure)
                    {
                        underlyingSymbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_UnderlyingSymbol]).ToString() ?? string.Empty;
                    }
                    if (String.IsNullOrEmpty(symbol) == true)
                    {
                        return;
                    }

                    string currency = string.Empty;
                    object currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_CurrencySymbol]);
                    if (currencyValue == null)
                    {
                        return;
                    }
                    currency = currencyValue.ToString();

                    if (String.IsNullOrEmpty(currency))
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        previousCurrency = currency;
                    }
                    if (currency != previousCurrency)
                    {
                        if (_summaryColName == PMConstants.COL_GrossExposureLocal || _summaryColName == PMConstants.COL_UnderlyingGrossExposure || _summaryColName == PMConstants.COL_BetaAdjGrossExposureUnderlying)
                        {
                            isMultiple = true;
                            return;
                        }
                    }

                    //string account = string.Empty;
                    object accountValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[OrderFields.PROPERTY_LEVEL1NAME]);
                    if (accountValue == null)
                    {
                        return;
                    }
                    //account = accountValue.ToString();

                    //string masterFund = string.Empty;
                    object masterFundValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_MasterFund]);
                    if (masterFundValue == null)
                    {
                        return;
                    }
                    // masterFund = masterFundValue.ToString();
                    object cellObjectTotal = 0;
                    object cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    if (_summaryColName == PMConstants.COL_GrossExposure || _summaryColName == PMConstants.COL_BetaAdjGrossExposure)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_NetExposureInBaseCurrency]);
                    }
                    if (_summaryColName == PMConstants.COL_GrossExposureLocal)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_NetExposure]);
                    }
                    if (_summaryColName == PMConstants.COL_GrossMarketValue || _summaryColName == PMConstants.COL_PercentDayPnLGrossMV || _summaryColName == PMConstants.COL_PercentDayPnLNetMV)
                    {

                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_MarketValueInBaseCurrency]);
                        if (_summaryColName == PMConstants.COL_PercentDayPnLGrossMV || _summaryColName == PMConstants.COL_PercentDayPnLNetMV)
                        {
                            cellObjectTotal = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_DayPnLInBaseCurrency]);
                            _position += double.Parse(row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Quantity]).ToString().Trim());
                        }

                    }

                    if (_summaryColName == PMConstants.COL_PercentGrossExposureInBaseCurrency)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_PercentNetExposureInBaseCurrency]);
                    }
                    if (_summaryColName == PMConstants.COL_PercentagePNLContribution)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_PercentagePNLContribution]);
                    }

                    if (_summaryColName == PMConstants.COL_PercentExposureInBaseCurrency)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_PercentExposureInBaseCurrency]);
                    }

                    if (_summaryColName == PMConstants.COL_PercentGrossMarketValueInBaseCurrency)
                    {
                        cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_MarketValueInBaseCurrency]);

                        object cellNAV = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_NAV]).ToString() ?? string.Empty;

                        if (cellNAV == null)
                        {
                            return;
                        }
                        double.TryParse(cellNAV.ToString(), out _nav);
                    }

                    string cellValue = string.Empty;
                    double nValue = 0.0;
                    if (cellObject == null)
                    {
                        return;
                    }
                    else
                    {
                        cellValue = cellObject.ToString();
                        // Handle null values
                        if (cellValue.Equals(string.Empty) || cellValue == PMConstants.doubleMinValString)
                        {
                            return;
                        }
                        else
                        {
                            double.TryParse(cellValue, out nValue);
                        }
                    }

                    string cellValueTotal = string.Empty;
                    double nValueTotal = 0.0;
                    if (cellObjectTotal == null)
                    {
                        return;
                    }
                    else
                    {
                        cellValueTotal = cellObjectTotal.ToString();
                        if (cellValueTotal.Equals(string.Empty) || cellValueTotal == PMConstants.doubleMinValString)
                        {
                            return;
                        }
                        else
                        {
                            double.TryParse(cellValueTotal, out nValueTotal);
                        }
                        this._totals += (decimal)nValueTotal;
                    }
                    if (_symbolWiseExposure != null)
                    {
                        if (
                           _summaryColName == PMConstants.COL_PercentUnderlyingGrossExposureInBaseCurrency || _summaryColName == PMConstants.COL_PercentBetaAdjGrossExposureInBaseCurrency ||
                           _summaryColName == PMConstants.COL_BetaAdjGrossExposureUnderlyingInBaseCurrency || _summaryColName == PMConstants.COL_UnderlyingGrossExposureInBaseCurrency
                            || _summaryColName == PMConstants.COL_UnderlyingGrossExposure || _summaryColName == PMConstants.COL_BetaAdjGrossExposureUnderlying)
                        {
                            if (!_symbolList.Contains(symbol))
                            {
                                _symbolList.Add(symbol);
                                if (!_symbolWiseExposure.ContainsKey(underlyingSymbol))
                                {
                                    _symbolWiseExposure.Add(underlyingSymbol, (decimal)nValue);
                                }
                            }
                        }
                        else
                        {
                            object cellAssetCategory = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Asset]);
                            var assetCategoryValue = cellAssetCategory.ToString();
                            object cellBetaObjectLeveragedFactor = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_LeveragedFactor]);
                            double leverageFactorValue = 0;
                            double.TryParse(cellBetaObjectLeveragedFactor.ToString(), out leverageFactorValue);

                            if (_symbolWiseExposure.ContainsKey(symbol))
                            {
                                if (!(assetCategoryValue.Equals(AssetCategory.FX.ToString()) || assetCategoryValue.Equals(AssetCategory.FXForward.ToString())) && _summaryColName == PMConstants.COL_BetaAdjGrossExposure)
                                {
                                    if (leverageFactorValue != 0)
                                        _symbolWiseExposure[symbol] += (decimal)(nValue / leverageFactorValue);
                                }
                                else
                                {
                                    _symbolWiseExposure[symbol] += (decimal)nValue;
                                }
                            }
                            else
                            {
                                if (!(assetCategoryValue.Equals(AssetCategory.FX.ToString()) || assetCategoryValue.Equals(AssetCategory.FXForward.ToString())) && _summaryColName == PMConstants.COL_BetaAdjGrossExposure)
                                {
                                    if (leverageFactorValue != 0)
                                        _symbolWiseExposure.Add(symbol, (decimal)(nValue / leverageFactorValue));
                                }
                                else
                                {
                                    _symbolWiseExposure.Add(symbol, (decimal)nValue);
                                }

                                if (_summaryColName == PMConstants.COL_BetaAdjGrossExposure && !_symbolWiseBeta.ContainsKey(symbol))
                                {
                                    object cellBetaObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Beta]);
                                    double betaValue = 0;
                                    double.TryParse(cellBetaObject.ToString(), out betaValue);
                                    _symbolWiseBeta.Add(symbol, betaValue);
                                }
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
            }

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                decimal totalGross = 0;
                decimal totalNet = 0;

                if (isMultiple == true)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                foreach (KeyValuePair<string, decimal> symbolwiseNetExpo in _symbolWiseExposure)
                {
                    if (_isNetRequired)
                        totalNet += symbolwiseNetExpo.Value;

                    if (_summaryColName == PMConstants.COL_BetaAdjGrossExposure && _symbolWiseBeta.ContainsKey(symbolwiseNetExpo.Key))
                    {
                        totalGross += Math.Abs(symbolwiseNetExpo.Value) * Math.Abs((decimal)_symbolWiseBeta[symbolwiseNetExpo.Key]);
                    }
                    else
                    {
                        totalGross += Math.Abs(symbolwiseNetExpo.Value);
                    }
                }

                if (_summaryColName == PMConstants.COL_PercentDayPnLGrossMV)
                {
                    if (totalGross != 0 && totalGross != decimal.MinValue && this._position != 0)
                        return this._totals / totalGross * 100;
                    else
                        return 0;
                }
                else if (_summaryColName == PMConstants.COL_PercentDayPnLNetMV)
                {
                    if (totalNet != 0 && totalNet != decimal.MinValue)
                        return this._totals / totalNet * 100;
                    else
                        return 0;
                }
                else if (_summaryColName == PMConstants.COL_PercentGrossMarketValueInBaseCurrency)
                {
                    if (_nav > 0)
                        return (totalGross * 100) / (decimal)_nav;
                    else
                        return 0;
                }
                return totalGross;
            }
        }
        #endregion

        #region Custom Summary Calculator Class Gross Calculations [Returns value on Symbol]
        public class LiquidationCostCalculator : ICustomSummaryCalculator
        {
            private string _summaryColName = string.Empty;
            private string previousCurrency = string.Empty;
            private Dictionary<string, double> _symbolWiseLiquidationCost;

            /// <summary>
            /// Only pass NetExposure or NetExposureBase
            /// </summary>
            /// <param name="summaryColName"></param>
            internal LiquidationCostCalculator(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                _symbolWiseLiquidationCost = new Dictionary<string, double>();
                this.previousCurrency = string.Empty;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    string symbol = string.Empty;
                    object symbolValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]).ToString() ?? string.Empty;
                    if (symbolValue == null)
                    {
                        return;
                    }
                    symbol = symbolValue.ToString();

                    if (String.IsNullOrEmpty(symbol) == true)
                    {
                        return;
                    }

                    string currency = string.Empty;
                    object currencyValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_CurrencySymbol]);
                    if (currencyValue == null)
                    {
                        return;
                    }

                    currency = currencyValue.ToString();

                    if (String.IsNullOrEmpty(currency))
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        previousCurrency = currency;
                    }
                    if (currency != previousCurrency)
                    {
                        return;
                    }

                    object cellObject = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    string cellValue = string.Empty;
                    double nValue = 0.0;
                    if (cellObject == null)
                    {
                        return;
                    }
                    else
                    {
                        cellValue = cellObject.ToString();
                        if (cellValue.Equals(string.Empty) || cellValue == PMConstants.doubleMinValString)
                        {
                            return;
                        }
                        else
                        {
                            double.TryParse(cellValue, out nValue);
                        }
                    }

                    //string asset = string.Empty;
                    object assetValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Asset]);
                    if (assetValue == null)
                    {
                        return;
                    }
                    //asset = assetValue.ToString();

                    if (_symbolWiseLiquidationCost.ContainsKey(symbol))
                    {
                        _symbolWiseLiquidationCost[symbol] += nValue;
                    }
                    else
                    {
                        _symbolWiseLiquidationCost.Add(symbol, nValue);
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
            }

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                double total = 0.0;

                foreach (KeyValuePair<string, double> symbolwiseNetCost in _symbolWiseLiquidationCost)
                {
                    total += symbolwiseNetCost.Value;
                }
                return total;
            }
        }
        #endregion

        #region Custom Summary Calculator Class StartTradeDate
        public class StartDateSummary : ICustomSummaryCalculator
        {
            private DateTime finalStartDate;

            internal StartDateSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.finalStartDate = DateTime.MinValue;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    DateTime currentStartDate = DateTime.Parse(row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_StartTradeDate]).ToString());

                    if (finalStartDate != DateTime.MinValue)
                    {
                        if (currentStartDate.CompareTo(finalStartDate) < 0)
                        {
                            finalStartDate = currentStartDate;
                        }
                    }
                    else
                    {
                        finalStartDate = currentStartDate;
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
            }

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                return finalStartDate;
            }
        }
        #endregion

        #region Custom Summary Calculator Class Dates
        public class DateSummary : ICustomSummaryCalculator
        {
            private DateTime _finalDate = DateTime.MinValue;
            private Boolean _isMultiple = false;
            private string _summaryColName = string.Empty;

            internal DateSummary(string summaryColName)
            {
                _summaryColName = summaryColName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this._finalDate = DateTime.MinValue;
                _isMultiple = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (_isMultiple)
                    {
                        return;
                    }

                    object tempDate = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]);
                    DateTime currentDate = Convert.ToDateTime(tempDate.ToString());
                    if (currentDate.CompareTo(_finalDate) != 0 && _finalDate.CompareTo(DateTime.MinValue) != 0)
                    {
                        _isMultiple = true;
                        return;
                    }
                    _finalDate = currentDate;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (_isMultiple)
                {
                    return PMConstants.SUMMARY_MULTIPLE;
                }
                else if (summarySettings.SourceColumn.Key == "ExpirationMonth" && _finalDate == DateTimeConstants.MinValue)
                {
                    return "Non-Expiring Positions";
                }
                return _finalDate;
            }
        }
        #endregion

        #region Custom Summary Calculator Class GroupingWise Summary
        public class GroupingWiseSummary : ICustomSummaryCalculator
        {
            private string _summaryColName = string.Empty;
            private string previousSymbol = string.Empty;
            private double previousSymbolValue = double.MinValue;
            private double positions = 0;
            private double returnValue = 0;
            private bool isMultiple = false;

            internal GroupingWiseSummary(string columnName)
            {
                this._summaryColName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.previousSymbol = string.Empty;
                this.previousSymbolValue = double.MinValue;
                this.positions = 0;
                this.returnValue = 0;
                this.isMultiple = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    string symbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Symbol]).ToString() ?? string.Empty;
                    string cellValue = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_summaryColName]).ToString().Trim();

                    if (cellValue.Equals(PMConstants.SUMMARY_DASH))
                    {
                        isMultiple = true;
                        return;
                    }

                    if (double.TryParse(cellValue, out returnValue))
                    {
                        if (!string.IsNullOrEmpty(symbol) && (string.IsNullOrEmpty(previousSymbol) || symbol.Equals(previousSymbol)))
                        {
                            previousSymbol = symbol;
                            previousSymbolValue = returnValue;
                            positions += double.Parse(row.GetCellValue(summarySettings.SourceColumn.Band.Columns[PMConstants.COL_Quantity]).ToString().Trim());
                        }
                        else
                        {
                            if (previousSymbolValue != double.MinValue && previousSymbolValue != returnValue)
                            {
                                isMultiple = true;
                                return;
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (this.positions > 0 && returnValue < 0)
                {
                    return returnValue * -1;
                }
                return returnValue;
            }
        }
        #endregion

        #region Custom  Class IdenticalColumnWiseNumberSummary [IdenticalNumber Columns Summary]
        public class IdenticalColumnWiseNumberSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;
            private double _summaryValue = 0.0;
            private string _columnName = string.Empty;
            private string _currentValue = string.Empty;
            private string _previousSymbol = null;
            private List<string> _listColumnKey = new List<string>();

            internal IdenticalColumnWiseNumberSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this._summaryValue = 0.0;
                this._currentValue = string.Empty;
                this._previousSymbol = null;
                this.isMultiple = false;
                this.isUndefined = false;
                this._listColumnKey = new List<string>();
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {
                try
                {
                    if (isMultiple || isUndefined)
                    {
                        return;
                    }
                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);
                    if (value == null)
                    {
                        isMultiple = true;
                        return;
                    }

                    if (_columnName == PMConstants.COL_DayTradedPosition)
                    {
                        object symbolName = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[OrderFields.PROPERTY_SYMBOL]);

                        if (_previousSymbol != null && _previousSymbol != symbolName.ToString())
                        {
                            isMultiple = true;
                            return;
                        }

                        _previousSymbol = symbolName.ToString();
                    }

                    object accountName = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[OrderFields.PROPERTY_LEVEL1NAME]);
                    _currentValue = accountName.ToString();

                    if (!_listColumnKey.Contains(_currentValue))
                    {
                        _listColumnKey.Add(_currentValue);
                        _summaryValue = _summaryValue + double.Parse(value.ToString());
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

            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return PMConstants.SUMMARY_DASH;
                }
                else if (isUndefined)
                {
                    return 0.0;
                }
                else
                {
                    return _summaryValue;
                }
            }
        }
        #endregion
    }
}