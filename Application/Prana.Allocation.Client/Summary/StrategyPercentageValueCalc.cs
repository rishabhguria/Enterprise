using Infragistics;
using Prana.Allocation.Client.Constants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Allocation.Client.Summary
{
    class StrategyPercentageValueCalc : SynchronousSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The total percentage
        /// </summary>
        private double _totalPercentage = 100.00;

        /// <summary>
        /// The _total value
        /// </summary>
        private double _totalQuantity;

        /// <summary>
        /// The _is remaining percentage calculation
        /// </summary>
        private bool _isRemainingPercentageCalculation = false;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is remaining percentage calculation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is remaining percentage calculation; otherwise, <c>false</c>.
        /// </value>
        public bool IsRemainingPercentageCalculation
        {
            get { return _isRemainingPercentageCalculation; }
            set { _isRemainingPercentageCalculation = value; }
        }

        /// <summary>
        /// Gets the <see cref="P:Infragistics.SummaryCalculatorBase.SummaryExecution" />, indicating when the summary will be applied.
        /// </summary>
        /// <remarks>
        /// When overridden, this can be used to indicate when an individual summary should be evaluated.   Depending
        /// on when the summary is executed the final result of the evaluation can change.
        /// </remarks>
        public override SummaryExecution? SummaryExecution
        {
            get
            {
                return Infragistics.SummaryExecution.PriorToFilteringAndPaging;
            }
        }

        /// <summary>
        /// Gets or sets the total value.
        /// </summary>
        /// <value>
        /// The total value.
        /// </value>
        public double TotalQuantity
        {
            get { return _totalQuantity; }
            set { _totalQuantity = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemainingValueCalc"/> class.
        /// </summary>
        /// <param name="totalQuantity">The total value.</param>
        public StrategyPercentageValueCalc(double totalQuantity, bool isRemainingPercentageCalculation)
        {
            try
            {
                TotalQuantity = totalQuantity;
                IsRemainingPercentageCalculation = isRemainingPercentageCalculation;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Calculates the summary information from the records provided by the query.
        /// </summary>
        /// <param name="data">The LINQ that provides the data which is currently available.</param>
        /// <param name="fieldKey">The name of the field being acted on.</param>
        /// <returns></returns>
        public override object Summarize(IQueryable data, string fieldKey)
        {
            double calculatedPercentage = 0.0;
            double allocatedQuantity = 0.0;
            try
            {
                string columnKey = fieldKey.Substring(0, (fieldKey.Length - AllocationUIConstants.PERCENTAGE.Length)) + AllocationUIConstants.QUANTITY;
                IEnumerable<DataRowView> dataRows = (IEnumerable<DataRowView>)data;
                bool isContainsCoulmnKey = false;
                if (dataRows.Count() > 0 && dataRows.First().Row.Table.Columns.Contains(columnKey))
                    isContainsCoulmnKey = true;
                foreach (DataRowView dataRow in dataRows)
                {
                    if (isContainsCoulmnKey)
                    {
                        allocatedQuantity += Convert.ToDouble(dataRow.Row[columnKey]);
                    }
                }
                calculatedPercentage = (TotalQuantity == 0.0) ? 0.0 : (allocatedQuantity * 100) / TotalQuantity;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return IsRemainingPercentageCalculation ? (_totalPercentage - calculatedPercentage) : calculatedPercentage;
        }

        #endregion Methods
    }
}
