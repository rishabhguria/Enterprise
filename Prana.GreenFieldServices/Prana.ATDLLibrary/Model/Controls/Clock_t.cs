using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the Clock_t control element within FIXatdl.
    /// </summary>
    public class Clock_t : InitializableControl<DateTime?>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Clock_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public Clock_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New Clock_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        // TODO: Implement LocalMktTz as a type.
        /// <summary>The timezone in which initValue is represented in.  Required when initValue is supplied. Applicable when 
        /// xsi:type is Clock_t.</summary>
        public string LocalMktTz { get; set; }

        /// <summary>Defines the treatment of initValue time. 0: use initValue; 1: use current time if initValue time has passed.
        /// The default value is 0.</summary>
        public int? InitValueMode { get; set; }

        public readonly string Type = AtdlControlName.Clock_t;

    }
}
