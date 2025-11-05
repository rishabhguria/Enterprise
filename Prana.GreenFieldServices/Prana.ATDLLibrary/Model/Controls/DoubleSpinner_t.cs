using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the DoubleSpinner_t control element within FIXatdl.
    /// </summary>
    public class DoubleSpinner_t : InitializableControl<decimal?>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DoubleSpinner_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public DoubleSpinner_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New DoubleSpinner_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        /// <summary>Limits the granularity of the inner spinner of a double spinner control. Useful in spinner objects to enforce
        ///  odd-lot and sub-penny restrictions.  Applicable when xsi:type is DoubleSpinner_t.</summary>
        public decimal? InnerIncrement { get; set; }

        /// <summary>For double spinner control, defines how to determine the increment for the inner set of spinners. Applicable 
        /// when xsi:type is DoubleSpinner_t only.</summary>
        public IncrementPolicy_t? InnerIncrementPolicy { get; set; }

        /// <summary>Limits the granularity of the outer spinner of a double spinner control. Useful in spinner objects to enforce
        ///  odd-lot and sub-penny restrictions.  Applicable when xsi:type is DoubleSpinner_t.</summary>
        public decimal? OuterIncrement { get; set; }

        /// <summary>For double spinner control, defines how to determine the increment for the outer set of spinners. Applicable 
        /// when xsi:type is DoubleSpinner_t only.</summary>
        public IncrementPolicy_t? OuterIncrementPolicy { get; set; }

        public readonly string Type = AtdlControlName.DoubleSpinner_t;

    }
}
