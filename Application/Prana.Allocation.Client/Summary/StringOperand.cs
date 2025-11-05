using Infragistics;
using Infragistics.Controls.Grids;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Summary
{
    class StringOperand : SumSummaryOperand
    {
        #region Members

        /// <summary>
        /// The _my calculate
        /// </summary>
        StringValueCalc _myCalc;

        /// <summary>
        /// The _string value
        /// </summary>
        private string _stringValue;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get's the default text that will be displayed in the <see cref="T:Infragistics.Controls.Grids.Primitives.SummaryRow" /> when this <see cref="T:Infragistics.SummaryOperandBase" /> is selected.
        /// </summary>
        protected override string DefaultRowDisplayLabel
        {
            get { return StringValue; }
        }

        /// <summary>
        /// Get's the default text that will be displayed in the drop down for this <see cref="T:Infragistics.SummaryOperandBase" />
        /// </summary>
        protected override string DefaultSelectionDisplayLabel
        {
            get { return StringValue; }
        }

        /// <summary>
        /// Gets / sets if the summary should processed for this summary operand.
        /// </summary>
        public override bool IsApplied
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value;
                if (_myCalc != null)
                    _myCalc.StringValue = StringValue;
            }
        }

        /// <summary>
        /// Gets the <see cref="P:Infragistics.Controls.Grids.SumSummaryOperand.SummaryCalculator" /> which will be used to calculate the summary.
        /// </summary>
        public override SummaryCalculatorBase SummaryCalculator
        {
            get
            {
                if (_myCalc == null)
                {
                    this._myCalc = new StringValueCalc(StringValue);
                }

                return this._myCalc;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringOperand"/> class.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        public StringOperand(string stringValue)
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
    }
}
