using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomSummaries : ICustomSummaryCalculator 
    {
        /// <summary>
        /// The remaining
        /// </summary>
        private decimal remaining = 0;

        /// <summary>
        /// The allocated
        /// </summary>
        private decimal allocated = 0;

        /// <summary>
        /// The total. 100 for percentage and total quantity for qauntity
        /// </summary>
        private decimal total = 0;

        /// <summary>
        /// The total qunatity.
        /// WIll always be quantity as needed to calculate percentage
        /// </summary>
        private decimal totalQuantity = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSummaries"/> class.
        /// </summary>
        /// <param name="total">The total.</param>
        public CustomSummaries(decimal total, decimal totalQuantity)
        {
            try
            {
                this.total = total;
                this.totalQuantity = totalQuantity;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        

        #region ICustomSummaryCalculator Members

        /// <summary>
        /// Implementing code processes the value of the cell associated with passed in row and the SourceColumn of the passed in SummarySettings parameter.
        /// </summary>
        /// <param name="summarySettings">The SummarySettings</param>
        /// <param name="row">The UltraGridRow</param>
        public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
        {
            try
            {
                string cellValue = row.Cells[summarySettings.SourceColumn].Text;
                decimal val = 0;
                if (decimal.TryParse(cellValue, out val))
                {
                    if (val > 0)
                    {

                        if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX))
                        {
                            decimal value = Convert.ToDecimal(row.Cells[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Value);
                            if (value > 0)
                                val = val * value / totalQuantity;
                        }
                        this.allocated += (val);
                        this.remaining -= (val);
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
                this.allocated = 0;
                this.remaining = total;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
            try
            {
                if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE) || summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_QUANTITY))
                    return decimal.Round(this.allocated, 2) + "/" + decimal.Round(this.total, 2);
                else if (summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_QUANTITY))
                    return decimal.Round(this.remaining, 2) + "/" + decimal.Round(this.total, 2);
                else if (summarySettings.Key.Equals(AllocationUIConstants.TOTAL_NAME))
                    return "Total: ";
                else if (summarySettings.Key.Equals(AllocationUIConstants.REMAINING_NAME))
                    return "Remaining: ";

                return string.Empty;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        #endregion
    }
}
