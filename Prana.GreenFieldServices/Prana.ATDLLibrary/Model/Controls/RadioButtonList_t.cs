using System;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Controls
{
    /// <summary>
    /// Represents the RadioButtonList_t control element within FIXatdl.
    /// </summary>
    public class RadioButtonList_t : ListControlBase, IOrientableControl
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RadioButtonList_t"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        public RadioButtonList_t(string id)
            : base(id)
        {
            Logger.LoggerWrite(string.Format("New RadioButtonList_t created as control {0}", id), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }

        public readonly string Type = AtdlControlName.RadioButtonList_t;

        #region IOrientableControl Members

        /// <summary>Must be “HORIZONTAL” or “VERTICAL”. Declares the orientation of the radio buttons within a RadioButtonList
        ///  or the checkboxes within a CheckBoxList.  Applicable when xsi:type is RadioButtonList_t or CheckBoxList_t.</summary>
        public Orientation_t? Orientation { get; set; }
        
        #endregion
    }
}
