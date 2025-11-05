using System.Collections.ObjectModel;
using Prana.ATDLLibrary.Model.Elements;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection used to store typed instances of EditRef_t.
    /// </summary>z
    /// <typeparam name="T">Type.</typeparam>
    public class EditRefCollection<T> : KeyedCollection<string, EditRef_t<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditRefCollection{T}"/> class.
        /// </summary>
        public EditRefCollection()
        {
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(EditRef_t<T> item)
        {
            base.Add(item);
        }

        /// <summary>
        /// Gets the key for items in this collection, i.e., the Edit_t ID.
        /// </summary>
        /// <param name="item">EditRef_t.</param>
        /// <returns>Edit_t ID.</returns>
        protected override string GetKeyForItem(EditRef_t<T> item)
        {
            return item.Id;
        }
    }
}
