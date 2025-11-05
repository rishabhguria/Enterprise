using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the CheckBox_t control element within FIXatdl.
    /// </summary>
    public class CheckBox_t : BinaryControlBase
    {
        public readonly string Type = AtdlControlName.CheckBox_t;
        /// <summary>
        /// Initializes a new instance of <see cref="CheckBox_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public CheckBox_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New CheckBox_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        } 
    }
}
