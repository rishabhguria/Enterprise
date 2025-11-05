using Infragistics;
using Prana.LogManager;
using System;
using System.Linq;

namespace Prana.Allocation.Client.Summary
{
    class StringValueCalc : SynchronousSummaryCalculator
    {
        #region Members

        /// <summary>
        /// The _string value
        /// </summary>
        private string _stringValue;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
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

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueCalc"/> class.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        public StringValueCalc(string stringValue)
        {
            try
            {
                StringValue = stringValue;
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
            try
            {
                return StringValue;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return string.Empty;
            }
        }

        #endregion Methods
    }
}
