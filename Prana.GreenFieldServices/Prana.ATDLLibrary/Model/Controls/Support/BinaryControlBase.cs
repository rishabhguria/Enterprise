namespace Prana.ATDLLibrary.Model.Controls.Support
{
    /// <summary>
    /// Represents control elements within FIXatdl that can support one of two states (<see cref="CheckBox_t"/>, <see cref="RadioButton_t"/>).
    /// </summary>
    public abstract class BinaryControlBase : InitializableControl<bool?>
    {

        /// <summary>
        /// Initializes a new instance of <see cref="BinaryControlBase"/> using the supplied ID.
        /// </summary>
        /// <param name="id">ID for this control.</param>
        protected BinaryControlBase(string id)
            : base(id)
        {
        }

        /// <summary>Output EnumID if checked/selected.  Applicable when xsi:type is CheckBox_t or RadioButton_t.</summary>
        public string CheckedEnumRef { get; set; }

        /// <summary>Output EnumID if unchecked/not selected.  Applicable when xsi:type is CheckBox_t or RadioButton_t.</summary>
        public string UncheckedEnumRef { get; set; }
    }
}
