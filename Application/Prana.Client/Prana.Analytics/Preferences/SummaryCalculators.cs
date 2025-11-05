using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.Analytics
{
    public class SummaryCalcSum : ICustomSummaryCalculator
    {
        double _total;
        bool _IsValid;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE"))
                {
                    _IsValid = true;
                    _total += Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            try
            {
                _total = 0.0;
                _IsValid = false;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalResult = _total;
            bool finalIsValid = _IsValid;
            _total = 0.0;
            _IsValid = false;

            if (finalIsValid)
            {
                return finalResult;
            }
            else
            {
                return "-";
            }
        }
    }

    public class SummaryCalcWeightedSum : ICustomSummaryCalculator
    {
        double _total;
        double _totalQuantity;
        int _sideMultiplier;
        string _symbol;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE") && _total != double.MinValue)
                {
                    int sideMultiplier = 1;
                    PranaPositionWithGreeks pranaPositionWithGreeks = row.ListObject as PranaPositionWithGreeks;
                    if (pranaPositionWithGreeks != null)
                    {
                        sideMultiplier = pranaPositionWithGreeks.SideMultiplier;
                    }

                    string symbol = row.Cells["Symbol"].Value.ToString();
                    if ((_sideMultiplier == int.MinValue && String.IsNullOrEmpty(_symbol)) || (_sideMultiplier == sideMultiplier && _symbol == symbol))
                    {
                        double quantity = Convert.ToDouble(row.Cells["TaxlotQty"].Value.ToString());
                        double value = Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());
                        _total += value * quantity;
                        _totalQuantity += quantity;

                        _sideMultiplier = sideMultiplier;
                        _symbol = symbol;
                    }
                    else
                    {
                        _total = double.MinValue;
                        _totalQuantity = 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            try
            {
                _sideMultiplier = int.MinValue;
                _symbol = String.Empty;
                _total = 0.0;
                _totalQuantity = 0.0;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalTotal = _total;
            double finalQty = _totalQuantity;
            _sideMultiplier = int.MinValue;
            _symbol = String.Empty;
            _total = 0.0;
            _totalQuantity = 0.0;

            if (finalQty != 0.0 && finalTotal != double.MinValue)
            {
                return finalTotal / finalQty;
            }
            else
            {
                return "-";
            }
        }
    }

    public class SummaryCalcText : ICustomSummaryCalculator
    {
        string _resultText;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE"))
                {
                    string text = (row.Cells[summarySettings.SourceColumn.Key].Value != null) ? row.Cells[summarySettings.SourceColumn.Key].Value.ToString() : String.Empty;

                    if (_resultText == null || _resultText == text)
                    {
                        _resultText = text;
                    }
                    else
                    {
                        _resultText = "Multiple";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultText = null;
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            string finalText = _resultText;
            _resultText = null;

            if ((summarySettings.Key.Equals("Level1Name") || summarySettings.Key.Equals("MasterFund")) && finalText == string.Empty)
                return ApplicationConstants.CONST_UNDEFINED;

            else if (summarySettings.Key.Equals("PutOrCall"))
            {
                if (finalText == int.MinValue.ToString())
                    return "-";
                else if (finalText == "0")
                    return "P";
                else if (finalText == "1")
                    return "C";
                else if (finalText == "Multiple")
                    return "Multiple";
            }
            return (finalText != String.Empty) ? finalText : "-";
        }
    }

    public class SummaryCalcNum : ICustomSummaryCalculator
    {
        double _resultNum;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE") && _resultNum != double.MinValue)
                {
                    double value = Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());

                    if (_resultNum == double.MaxValue || _resultNum == value)
                    {
                        _resultNum = value;
                    }
                    else
                    {
                        _resultNum = double.MinValue;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultNum = double.MaxValue;//can not take zero, as zero is a valid result
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalNum = _resultNum;
            _resultNum = double.MaxValue;

            if (finalNum != double.MinValue && finalNum != double.MaxValue)
            {
                if (summarySettings.Key.Equals("StrikePrice") && finalNum == 0.0)
                {
                    return "-";
                }
                else
                {
                    return finalNum;
                }
            }
            else
            {
                return "-";
            }
        }
    }

    public class SummaryCalcDate : ICustomSummaryCalculator
    {
        private DateTime _finalDate = DateTime.MinValue;
        private Boolean _isMultiple = false;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE"))
                {
                    if (_isMultiple)
                    {
                        return;
                    }

                    object tempDate = row.Cells[summarySettings.SourceColumn.Key].Value;
                    DateTime currentDate = Convert.ToDateTime(tempDate.ToString());

                    if (currentDate.CompareTo(_finalDate) != 0 && _finalDate.CompareTo(DateTime.MinValue) != 0)
                    {
                        _isMultiple = true;
                        return;
                    }
                    _finalDate = currentDate;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _finalDate = DateTime.MinValue;
            _isMultiple = false;
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            DateTime finalDate = _finalDate;
            Boolean isMultiple = _isMultiple;
            _finalDate = DateTime.MinValue;
            _isMultiple = false;

            if (isMultiple)
            {
                return "Multiple";
            }
            else if (summarySettings.SourceColumn.Key == "ExpirationMonth" && finalDate == DateTimeConstants.MinValue)
            {
                return "Non-Expiring Positions";
            }
            else if (summarySettings.SourceColumn.Key == "ExpirationDate" && finalDate == DateTimeConstants.MinValue)
            {
                return "N/A";
            }
            return finalDate;
        }
    }

    public class SummaryCalcSymbolSum : ICustomSummaryCalculator
    {
        double _result;
        string _symbol;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE") && _result != double.MinValue)
                {
                    if (String.IsNullOrEmpty(_symbol) || _symbol == row.Cells["Symbol"].Value.ToString())
                    {
                        _result += Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());
                        _symbol = row.Cells["Symbol"].Value.ToString();
                    }
                    else
                    {
                        _result = double.MinValue;
                    }
                }
            }
            catch (Exception ex)
            {

                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _result = 0.0;
            _symbol = String.Empty;
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalResult = _result;
            _result = 0.0;
            _symbol = String.Empty;

            if (finalResult != double.MinValue)
            {
                return finalResult;
            }
            else
            {
                return "-";
            }
        }
    }

    public class SummaryCalcUnderlyingSum : ICustomSummaryCalculator
    {
        double _result;
        string _underlying;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE") && _result != double.MinValue)
                {
                    if (String.IsNullOrEmpty(_underlying) || _underlying == row.Cells["UnderlyingSymbol"].Value.ToString())
                    {
                        _result += Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());
                        _underlying = row.Cells["UnderlyingSymbol"].Value.ToString();
                    }
                    else
                    {
                        _result = double.MinValue;
                    }
                }
            }
            catch (Exception ex)
            {

                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _result = 0.0;
            _underlying = String.Empty;
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalResult = _result;
            _result = 0.0;
            _underlying = String.Empty;

            if (finalResult != double.MinValue)
            {
                return finalResult;
            }
            else
            {
                return "-";
            }
        }
    }

    public class SummaryCalcLocalColumns : ICustomSummaryCalculator
    {
        double _total;
        bool _IsMultiple;
        string _currency;

        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE"))
                {
                    if (_IsMultiple)
                    {
                        return;
                    }
                    if (!String.IsNullOrEmpty(_currency) && !row.Cells["CurrencyName"].Text.Equals(_currency))
                    {
                        _IsMultiple = true;
                        return;
                    }
                    _total += Convert.ToDouble(row.Cells[summarySettings.SourceColumn.Key].Value.ToString());
                    if (String.IsNullOrEmpty(_currency))
                        _currency = row.Cells["CurrencyName"].Text;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            try
            {
                _total = 0.0;
                _IsMultiple = false;
                _currency = string.Empty;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            double finalTotal = _total;
            bool finalIsMultiple = _IsMultiple;

            _total = 0.0;
            _IsMultiple = false;
            _currency = string.Empty;

            if (!finalIsMultiple)
            {
                return finalTotal;
            }
            return "-";
        }
    }
}