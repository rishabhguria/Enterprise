using Infragistics;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Summary
{
    class StrategyPercentageOperand : SummaryOperandBase
    {
        #region Members

        /// <summary>
        /// The calculator
        /// </summary>
        StrategyPercentageValueCalc _myCalc;

        /// <summary>
        /// The _total quantity
        /// </summary>
        private double _totalQuantity;

        /// <summary>
        /// The _is remaining percentage calculation
        /// </summary>
        private bool _isRemainingPercentageCalculation = false;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get's the default text that will be displayed in a SummaryRow when this <see cref="T:Infragistics.SummaryOperandBase" /> is selected.
        /// </summary>
        protected override string DefaultRowDisplayLabel
        {
            get { return IsRemainingPercentageCalculation ? "Remaining Percentage" : "Total Percentage"; }
        }

        /// <summary>
        /// Get's the default text that will be displayed in the drop down for this <see cref="T:Infragistics.SummaryOperandBase" />
        /// </summary>
        protected override string DefaultSelectionDisplayLabel
        {
            get { return IsRemainingPercentageCalculation ? "Remaining Percentage" : "Total Percentage"; }
        }

        /// <summary>
        /// Gets / sets if the summary should processed for this summary operand.
        /// </summary>
        public override bool IsApplied
        {
            get { return true; }
        }

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
        /// Gets the <see cref="P:Infragistics.SummaryOperandBase.SummaryCalculator" /> which will be used to calculate the summary.
        /// </summary>
        public override SummaryCalculatorBase SummaryCalculator
        {
            get
            {
                if (_myCalc == null)
                {
                    this._myCalc = new StrategyPercentageValueCalc(TotalQuantity, IsRemainingPercentageCalculation);
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
                _myCalc.TotalQuantity = TotalQuantity;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyPercentageOperand"/> class.
        /// </summary>
        /// <param name="isRemainingPercentageCalculation">if set to <c>true</c> [is remaining percentage calculation].</param>
        public StrategyPercentageOperand(bool isRemainingPercentageCalculation)
        {
            try
            {
                IsRemainingPercentageCalculation = isRemainingPercentageCalculation;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors
    }
}
