using Prana.ATDLLibrary.Model.Elements;

namespace Prana.ATDLLibrary.Model.Controls.Support
{
    /// <summary>
    /// Generic base class for all controls.
    /// </summary>
    /// <typeparam name="T">Specified the type of the InitValue.  Note that this may not be the same as the type that the
    /// control uses to store its data, for example InitValue for list controls is of type string whereas this type of
    /// control uses EnumState to store its state.</typeparam>
    public abstract class InitializableControl<T> : Control_t
    {

        /// <summary>
        /// Initializes a new <see cref="InitializableControl"/> instance with the specified identifier as id.
        /// </summary>
        /// <param name="id">Id of this control.</param>
        protected InitializableControl(string id)
            :base(id)
        {
        }

        /// <summary>The value used to pre-populate the GUI component when the order entry screen is initially rendered.</summary>
        public T InitValue { get; set; }
    }
}
