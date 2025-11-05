using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the MultiSelectList_t control element within FIXatdl.
    /// </summary>
    public class MultiSelectList_t : ListControlBase
    {
        public readonly string Type = AtdlControlName.MultiSelectList_t;

        /// <summary>
        /// Initializes a new instance of <see cref="MultiSelectList_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public MultiSelectList_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New MultiSelectList_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
