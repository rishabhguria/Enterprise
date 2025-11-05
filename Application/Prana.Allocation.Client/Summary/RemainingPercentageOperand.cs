using Infragistics;

namespace Prana.Allocation.Client.Summary
{
    class RemainingPercentageOperand : SummaryOperandBase
    {
        #region Members

        /// <summary>
        /// The calculator
        /// </summary>
        RemainingValueCalc _myCalc;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get's the default text that will be displayed in a SummaryRow when this <see cref="T:Infragistics.SummaryOperandBase" /> is selected.
        /// </summary>
        protected override string DefaultRowDisplayLabel
        {
            get { return "Remaining Percentage"; }
        }

        /// <summary>
        /// Get's the default text that will be displayed in the drop down for this <see cref="T:Infragistics.SummaryOperandBase" />
        /// </summary>
        protected override string DefaultSelectionDisplayLabel
        {
            get { return "Remaining Percentage"; }
        }

        /// <summary>
        /// Gets / sets if the summary should processed for this summary operand.
        /// </summary>
        public override bool IsApplied
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the <see cref="P:Infragistics.SummaryOperandBase.SummaryCalculator" /> which will be used to calculate the summary.
        /// </summary>
        public override SummaryCalculatorBase SummaryCalculator
        {
            get
            {
                if (_myCalc == null)
                {
                    this._myCalc = new RemainingValueCalc(100.00);
                }

                return this._myCalc;
            }
        }

        #endregion Properties
    }
}
