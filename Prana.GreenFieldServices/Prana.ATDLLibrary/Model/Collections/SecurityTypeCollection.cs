using Prana.ATDLLibrary.Model.Elements;
using System.Collections.ObjectModel;

namespace Prana.ATDLLibrary.Model.Collections
{
    public class SecurityTypeCollection : KeyedCollection<string, SecurityType_t>
    {
        protected override string GetKeyForItem(SecurityType_t item)
        {
            return item.Name;
        }
    }
}
