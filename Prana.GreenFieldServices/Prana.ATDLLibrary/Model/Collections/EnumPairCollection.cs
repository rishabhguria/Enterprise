

using Prana.ATDLLibrary.Model.Elements;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection used to represent a set of EnumPairs.
    /// </summary>
    public class EnumPairCollection : KeyedCollection<string, EnumPair_t>
    {
        protected override string GetKeyForItem(EnumPair_t item)
        {
            return item.EnumId;
        }

        /// <summary>
        /// Gets the full set of EnumIds.
        /// </summary>
        /// <value>The enum ids.</value>
        public string[] EnumIds
        {
            get { return (from item in Items select item.EnumId).ToArray<string>(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has values.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public bool HasValues
        {
            get { return Count > 0; }
        }
    }
}
