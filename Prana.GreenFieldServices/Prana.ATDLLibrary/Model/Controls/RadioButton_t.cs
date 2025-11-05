using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the RadioButton_t control element within FIXatdl.
    /// </summary>
    public class RadioButton_t : BinaryControlBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RadioButton_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public RadioButton_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New RadioButton_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        /// <summary>Identifies a common group name used by a set of RadioButton_t among which only one radio button 
        /// may be selected at a time.  Applicable when xsi:type is RadioButton_t.</summary>
        public string RadioGroup { get; set; }

        public readonly string Type = AtdlControlName.RadioButton_t;

    }
}
