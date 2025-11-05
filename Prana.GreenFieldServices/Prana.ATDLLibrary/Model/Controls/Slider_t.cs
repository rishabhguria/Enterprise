using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the Slider_t control element within FIXatdl.
    /// </summary>
    /// <remarks>The FIXatdl 1.1 specification is a little unclear on what a Slider_t can do.  The current Atdl4net implementation supports 
    /// selecting from a set of options (ListItems) but not selecting a numerical value.</remarks>
    public class Slider_t : ListControlBase
    {
        public readonly string Type = AtdlControlName.Slider_t;

        /// <summary>
        /// Initializes a new instance of <see cref="Slider_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public Slider_t(string id)
            : base(id) 
        {
            Logger.LoggerWrite(string.Format("New Slider_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
