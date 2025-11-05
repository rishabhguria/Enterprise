using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the TextField_t control element within FIXatdl.
    /// </summary>
    public class TextField_t : InitializableControl<string>
    {
        public readonly string Type = AtdlControlName.TextField_t;

        /// <summary>
        /// Initializes a new instance of <see cref="TextField_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public TextField_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New TextField_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
