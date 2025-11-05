using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the CheckBoxList_t control element within FIXatdl.
    /// </summary>
    public class CheckBoxList_t : ListControlBase, IOrientableControl
    {
        public readonly string Type = AtdlControlName.CheckBoxList_t;

        /// <summary>
        /// Initializes a new instance of <see cref="CheckBoxList_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public CheckBoxList_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New CheckBoxList_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }


        #region IOrientableControl Members

        /// <summary>Must be “HORIZONTAL” or “VERTICAL”. Declares the orientation of the radio buttons within a RadioButtonList
        ///  or the checkboxes within a CheckBoxList.  Applicable when xsi:type is RadioButtonList_t or CheckBoxList_t.</summary>
        public Orientation_t? Orientation { get; set; }

        #endregion
    }
}
