using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Globalization;

namespace Prana.WashSale.Classes
{
    internal class WashSaleCustomSummaries
    {
        #region Custom Summary Calculator Class Position Summary [Position]
        public class PositionSummary : ICustomSummaryCalculator
        {
            private double totals = 0.0;
            private bool isMultiple = false;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;
            private string _summaryColName = string.Empty;

            public PositionSummary(string summaryColName)
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
                    object valueSymbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[WashSaleConstants.CONST_SYMBOL]);
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

                    if (quantity != string.Empty || quantity != WashSaleConstants.doubleEpsilonValString || quantity.ToString() != WashSaleConstants.intMinValString)// || nSideMultiplier != 0)
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
                    return WashSaleConstants.SUMMARY_DASH;
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return WashSaleConstants.SUMMARY_DASH;
                    }
                }
                else
                {
                    return this.totals;
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

            public TextSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.current = string.Empty;
                this.previous = string.Empty;
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

                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);
                    if (value == null || _columnName == WashSaleConstants.CONST_TAXLOT_ID)
                    {
                        return;
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
                    return WashSaleConstants.SUMMARY_MULTIPLE;
                }
                else if (isUndefined)
                {
                    if (current != string.Empty)
                    {
                        return WashSaleConstants.SUMMARY_MULTIPLE;
                    }
                    else
                    {
                        return WashSaleConstants.SUMMARY_UNDEFINED;
                    }
                }
                else
                {
                    return current;
                }
            }
        }
        #endregion

        #region Custom  Class DateColumnSummary [Text Columns Summary]
        public class DateSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;
            private string current = string.Empty;
            private string previous = string.Empty;
            private string _columnName = string.Empty;

            public DateSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this.current = string.Empty;
                this.previous = string.Empty;
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

                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);
                    if (value == null)
                    {
                        return;
                    }
                    if (String.IsNullOrEmpty(value.ToString()) == true || value == DBNull.Value)
                    {
                        isUndefined = true;
                        if (_columnName.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE))
                            isUndefined = false;

                        return;
                    }

                    DateTime date;
                    DateTime.TryParse(value.ToString(), out date);
                    current = date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
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
                    return WashSaleConstants.SUMMARY_MULTIPLE;
                }
                else if (isUndefined)
                {
                    if (current != string.Empty)
                    {
                        return WashSaleConstants.SUMMARY_MULTIPLE;
                    }
                    else
                    {
                        return WashSaleConstants.SUMMARY_UNDEFINED;
                    }
                }
                else
                {
                    return current;
                }
            }
        }
        #endregion

        #region Custom  Class IdenticalColumnWiseNumberSummary [IdenticalNumber Columns Summary]
        public class IdenticalColumnWiseNumberSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;
            private decimal _summaryValue = 0;
            private string _columnName = string.Empty;

            public IdenticalColumnWiseNumberSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                this._summaryValue = 0;
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
                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]);
                    if (value == null || value.Equals(string.Empty))
                    {
                        if (_columnName.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS) || _columnName.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD)
                            || _columnName.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS))
                            isUndefined = true;
                        else
                            isMultiple = true;
                        return;
                    }
                    decimal result = 0;
                    if (decimal.TryParse(value.ToString(), out result))
                        _summaryValue = _summaryValue + result;
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
                    return WashSaleConstants.SUMMARY_DASH;
                }
                else if (isUndefined && _summaryValue == 0)
                {
                    return WashSaleConstants.SUMMARY_DASH;
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
