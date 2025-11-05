using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the SingleSpinner_t control element within FIXatdl.
    /// </summary>
    public class SingleSpinner_t : InitializableControl<string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SingleSpinner_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public SingleSpinner_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New SingleSpinner_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        /// <summary>Limits the granularity of a spinner control. Useful in spinner objects to enforce odd-lot and sub-penny
        ///  restrictions.  Applicable when xsi:type is SingleSpinner_t or Slider_t.</summary>
        public decimal? Increment { get; set; }

        /// <summary>For single spinner control, defines how to determine the increment. Applicable when xsi:type is SingleSpinner_t.</summary>
        public IncrementPolicy_t? IncrementPolicy { get; set; }

        public readonly string Type = AtdlControlName.SingleSpinner_t;
    }
}
