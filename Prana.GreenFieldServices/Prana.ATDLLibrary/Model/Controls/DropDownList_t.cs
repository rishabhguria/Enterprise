using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the DropDownList_t control element within FIXatdl.
    /// </summary>
    public class DropDownList_t : ListControlBase
    {
        public readonly string Type = AtdlControlName.DropDownList_t;

        /// <summary>
        /// Initializes a new instance of <see cref="DropDownList_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public DropDownList_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New DropDownList_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
