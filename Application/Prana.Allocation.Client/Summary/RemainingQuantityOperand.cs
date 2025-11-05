using Infragistics;

namespace Prana.Allocation.Client.Summary
{
    class RemainingQuantityOperand : SummaryOperandBase
    {
        #region Members

        /// <summary>
        /// The calculator
        /// </summary>
        RemainingValueCalc _myCalc;

        /// <summary>
        /// The _total quantity
        /// </summary>
        private double _totalQuantity;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get's the default text that will be displayed in a SummaryRow when this <see cref="T:Infragistics.SummaryOperandBase" /> is selected.
        /// </summary>
        protected override string DefaultRowDisplayLabel
        {
            get { return "Remaining Quantity"; }
        }

        /// <summary>
        /// Get's the default text that will be displayed in the drop down for this <see cref="T:Infragistics.SummaryOperandBase" />
        /// </summary>
        protected override string DefaultSelectionDisplayLabel
        {
            get { return "Remaining Quantity"; }
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
                    this._myCalc = new RemainingValueCalc(TotalQuantity);
                }

                return this._myCalc;
            }
        }

        /// <summary>
        /// Gets or sets the total quantity.
        /// </summary>
        /// <value>
        /// The total quantity.
        /// </value>
        public double TotalQuantity
        {
            get { return _totalQuantity; }
            set
            {
                _totalQuantity = value;
                _myCalc.TotalValue = TotalQuantity;
            }
        }

        #endregion Properties
    }
}
