using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the EditableDropDownList_t control element within FIXatdl.
    /// </summary>
    public class EditableDropDownList_t : ListControlBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EditableDropDownList_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public EditableDropDownList_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New EditableDropDownList_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        /// <summary>
        /// Indicates whether the EnumState value for this control can be set to a value other than one of the enumerated
        /// values.
        /// </summary>
        protected override bool IsNonEnumValueAllowed { get { return true; } }

        public readonly string Type = AtdlControlName.EditableDropDownList_t;
    }
}
