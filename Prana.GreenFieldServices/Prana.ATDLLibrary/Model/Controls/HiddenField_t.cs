using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the HiddenField_t control element within FIXatdl.
    /// </summary>
    public class HiddenField_t : InitializableControl<string>
    {
        public readonly string Type = AtdlControlName.HiddenField_t;

        /// <summary>
        /// Initializes a new instance of <see cref="HiddenField_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public HiddenField_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New HiddenField_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
