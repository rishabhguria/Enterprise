using Infragistics;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Allocation.Client.Summary
{
    class RemainingValueCalc : SynchronousSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The _total value
        /// </summary>
        private double _totalValue;

        #endregion Members

        #region Properties

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
        public double TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemainingValueCalc"/> class.
        /// </summary>
        /// <param name="totalValue">The total value.</param>
        public RemainingValueCalc(double totalValue)
        {
            try
            {
                TotalValue = totalValue;
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
            double totalQuantity = 0.0;
            double remainingQuantity = 0.0;
            try
            {
                IEnumerable<DataRowView> dataRows = (IEnumerable<DataRowView>)data;
                bool isContainsCoulmnKey = false;
                if (dataRows.Count() > 0 && dataRows.First().Row.Table.Columns.Contains(fieldKey))
                    isContainsCoulmnKey = true;
                foreach (DataRowView dataRow in dataRows)
                {
                    if (isContainsCoulmnKey)
                    {
                        if (dataRow.Row[fieldKey] != DBNull.Value)
                            totalQuantity += Convert.ToDouble(dataRow.Row[fieldKey]);
                        else
                            totalQuantity += 0.0;
                    }
                }
                remainingQuantity = TotalValue - totalQuantity;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return remainingQuantity;
        }

        #endregion Methods
    }
}
