using Infragistics.Controls.Grids;

namespace Prana.Allocation.Client.Summary
{
    class TotalPercentageOperand : SumSummaryOperand
    {
        #region Properties

        /// <summary>
        /// Get's the default text that will be displayed in the <see cref="T:Infragistics.Controls.Grids.Primitives.SummaryRow" /> when this <see cref="T:Infragistics.SummaryOperandBase" /> is selected.
        /// </summary>
        protected override string DefaultRowDisplayLabel
        {
            get { return "Total Percentage"; }
        }

        /// <summary>
        /// Get's the default text that will be displayed in the drop down for this <see cref="T:Infragistics.SummaryOperandBase" />
        /// </summary>
        protected override string DefaultSelectionDisplayLabel
        {
            get { return "Total Percentage"; }
        }

        /// <summary>
        /// Gets / sets if the summary should processed for this summary operand.
        /// </summary>
        public override bool IsApplied
        {
            get { return true; }
        }

        #endregion Properties
    }
}
