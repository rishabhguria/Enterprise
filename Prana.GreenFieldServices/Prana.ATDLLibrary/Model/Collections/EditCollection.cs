using System.Collections.ObjectModel;
using Prana.ATDLLibrary.Model.Elements;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection used for storing instances of Edit_t, keyed on Edit ID.  This collection is used at the root Strategies_t and Strategy_t level.
    /// </summary>
    public class EditCollection : KeyedCollection<string, Edit_t>
    {
        protected override string GetKeyForItem(Edit_t item)
        {
            return item.Id;
        }
    }
}
