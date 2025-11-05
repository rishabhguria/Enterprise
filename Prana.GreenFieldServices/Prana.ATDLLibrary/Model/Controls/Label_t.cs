using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the Label_t control element within FIXatdl.
    /// </summary>
    public class Label_t : InitializableControl<string>
    {
        public readonly string Type = AtdlControlName.Label_t;

        /// <summary>
        /// Initializes a new instance of <see cref="Label_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public Label_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New Label_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

    }
}
