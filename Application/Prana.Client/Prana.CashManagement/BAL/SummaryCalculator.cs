using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.CashManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcDate : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The final date
        /// </summary>
        private DateTime _finalDate = DateTime.MinValue;

        /// <summary>
        /// The is multiple
        /// </summary>
        private Boolean _isMultiple = false;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                if (_isMultiple)
                {
                    return;
                }

                object tempDate = row.Cells[summarySettings.SourceColumn.Key].Value;
                if (tempDate == null)
                {
                    tempDate = DateTime.MinValue;
                }
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
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _finalDate = DateTime.MinValue;
            _isMultiple = false;
        }

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            DateTime finalDate = _finalDate;
            Boolean isMultiple = _isMultiple;
            _finalDate = DateTime.MinValue;
            _isMultiple = false;

            summarySettings.Appearance.TextHAlign = HAlign.Right;
            if (isMultiple)
            {
                return "Multiple";
            }
            if (DateTime.Compare(finalDate, DateTime.MinValue) == 0)
                return String.Empty;
            return finalDate;
        }

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcDisplayText : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The result text
        /// </summary>
        string _resultText;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary;
                string text = (row.Cells[summarySettings.SourceColumn.Key].Text != null) ? row.Cells[summarySettings.SourceColumn.Key].Text.ToString() : String.Empty;
                if (String.IsNullOrEmpty(_resultText) || _resultText == text)
                {
                    _resultText = text;
                }
                else
                {
                    _resultText = "Multiple";
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

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultText = String.Empty;
        }

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            string finalText = _resultText;
            _resultText = String.Empty;

            summarySettings.Appearance.TextHAlign = HAlign.Right;
            return (finalText != String.Empty) ? finalText : "-";
        }

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcNum : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The result number
        /// </summary>
        double _resultNum;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                if (_resultNum != double.MinValue)
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

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultNum = double.MaxValue;//can not take zero, as zero is a valid result
        }

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalNum = _resultNum;
            _resultNum = double.MaxValue;

            summarySettings.Appearance.TextHAlign = HAlign.Right;
            if (finalNum != double.MinValue && finalNum != double.MaxValue)
            {
                return finalNum;
            }
            else
            {
                return "-";
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcNumText : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The result text
        /// </summary>
        string _resultText;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                string text = (row.Cells[summarySettings.SourceColumn.Key].Value != null) ? row.Cells[summarySettings.SourceColumn.Key].Value.ToString() : String.Empty;
                if (String.IsNullOrEmpty(_resultText) || _resultText == text)
                {
                    summarySettings.Appearance.TextHAlign = HAlign.Right;
                    _resultText = text;
                }
                else
                {
                    summarySettings.Appearance.TextHAlign = HAlign.Right;
                    _resultText = "Multiple";
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

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultText = String.Empty;
        }

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            string finalText = _resultText;
            _resultText = String.Empty;

            return (finalText != String.Empty) ? finalText : "-";
        }

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcSum : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The total
        /// </summary>
        double _total;

        /// <summary>
        /// The is difference currency
        /// </summary>
        bool _IsDiffCurrency;

        /// <summary>
        /// The previous currency
        /// </summary>
        string _prevCurrency = string.Empty;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                object value = row.Cells[summarySettings.SourceColumn.Key].Value;
                if (string.IsNullOrEmpty(_prevCurrency) && Convert.ToDouble(value.ToString()) != 0)
                {
                    if (row.Band.Columns.Exists("CurrencyName"))
                    {
                        _prevCurrency = Convert.ToString(row.Cells["CurrencyName"].Value);
                    }
                    else if (row.Band.Columns.Exists("CurrencySymbol"))
                    {
                        _prevCurrency = Convert.ToString(row.Cells["CurrencySymbol"].Value);
                    }
                }
                if (value == null || Convert.ToDouble(value.ToString()) == 0)
                {
                    return;
                }
                if (row.Band.Columns.Exists("CurrencyName") && !_prevCurrency.Equals(Convert.ToString(row.Cells["CurrencyName"].Value)) && _total != 0)
                {
                    _IsDiffCurrency = true;
                }
                if (row.Band.Columns.Exists("CurrencySymbol") && !_prevCurrency.Equals(Convert.ToString(row.Cells["CurrencySymbol"].Value)) && _total != 0)
                {
                    _IsDiffCurrency = true;
                }
                _total += Convert.ToDouble(value.ToString());
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

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            try
            {
                _total = 0.0;
                _IsDiffCurrency = false;
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

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            double finalResult = _total;
            bool finalIsCurrencyDiff = _IsDiffCurrency;
            _total = 0.0;
            _IsDiffCurrency = false;
            _prevCurrency = string.Empty;

            if (!finalIsCurrencyDiff || summarySettings.SourceColumn.Key.Equals("DrBase") || summarySettings.SourceColumn.Key.Equals("CrBase"))
            {
                summarySettings.Appearance.TextHAlign = HAlign.Right;
                return finalResult;
            }
            else
            {
                summarySettings.Appearance.TextHAlign = HAlign.Right;
                return "Multiple";
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Infragistics.Win.UltraWinGrid.ICustomSummaryCalculator" />
    public class SummaryCalcText : ICustomSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The result text
        /// </summary>
        string _resultText;

        #endregion Members

        #region Methods

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                //Calculate Summary
                string text = (row.Cells[summarySettings.SourceColumn.Key].Value != null) ? row.Cells[summarySettings.SourceColumn.Key].Value.ToString() : String.Empty;
                if (String.IsNullOrEmpty(_resultText) || _resultText == text)
                {
                    summarySettings.Appearance.TextHAlign = HAlign.Right;
                    _resultText = text;
                }
                else
                {
                    summarySettings.Appearance.TextHAlign = HAlign.Right;
                    _resultText = "Multiple";
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

        /// <summary>
        /// Begins a custom summary for the SummarySettings object passed in. Implementation of this method would clear up and reset any state variables used for calculating the summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            _resultText = String.Empty;
        }

        /// <summary>
        /// Ends previously begun summary and returns the calculated summary value. This is called after AggregateCustomSummary is called for all the rows to be summarized in a summary.
        /// </summary>
        /// <param name="summarySettings">SummarySettings object associated with the summary being calcualted.</param>
        /// <param name="rows">RowsCollection for which the summary is being calculated for.</param>
        /// <returns>
        /// Returns the summary value.
        /// </returns>
        /// <remarks>
        /// <p class="body">
        ///   <code>rows</code> argument is the rows collection from the band that the <code>summarySettings</code> object is assigned to.</p>
        /// </remarks>
        public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
        {
            //return the summary & reset fields
            string finalText = _resultText;
            _resultText = String.Empty;

            return (finalText != String.Empty) ? finalText : "-";
        }

        #endregion Methods
    }
}
