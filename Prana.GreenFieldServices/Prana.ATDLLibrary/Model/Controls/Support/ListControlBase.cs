using Prana.ATDLLibrary.Model.Collections;

namespace Prana.ATDLLibrary.Model.Controls.Support
{
    /// <summary>
    /// Base class for the subset of FIXatdl controls that allow ListItems.
    /// </summary>
    /// <remarks>The following controls support ListItems:
    /// <list type="bullet">
    /// <item><description>CheckBoxList_t</description></item>
    /// <item><description>DropDownList_t</description></item>
    /// <item><description>EditableDropDownList_t</description></item>
    /// <item><description>MultiSelectList_t</description></item>
    /// <item><description>RadioButtonList_t</description></item>
    /// <item><description>SingleSelectList_t</description></item>
    /// <item><description>Slider_t</description></item>
    /// </list>
    /// </remarks>
    public abstract class ListControlBase : InitializableControl<string>
    {

        /// <summary>
        /// The ListItems for this control; will be empty if no ListItem sub-elements are present.
        /// </summary>
        protected readonly ListItemCollection _listItems = new ListItemCollection();

        /// <summary>
        /// Indicates whether the EnumState value for this control can be set to a value other than one of the enumerated
        /// values.  (This property is present to support editable drop-down list controls.)
        /// </summary>
        protected virtual bool IsNonEnumValueAllowed { get { return false; } }

        /// <summary>
        /// Initializes the base Control_t class with the supplied control identifier.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        protected ListControlBase(string id)
            : base(id)
        {
        }
        /// <summary>
        /// Gets the collection of ListItems for this control.
        /// </summary>
        public ListItemCollection ListItems { get { return _listItems; } }

    }
}
