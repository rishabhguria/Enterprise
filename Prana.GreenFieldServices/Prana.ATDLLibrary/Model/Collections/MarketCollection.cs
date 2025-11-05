using Prana.ATDLLibrary.Model.Elements;
using System.Collections.ObjectModel;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Represents a collection of Markets.
    /// </summary>
    public class MarketCollection : KeyedCollection<string, Market_t>
    {
        /// <summary>
        /// Gets the key for items in this collection, i.e., the MIC code.
        /// </summary>
        /// <param name="item">Market_t instance.</param>
        /// <returns>MIC code for this Market</returns>
        protected override string GetKeyForItem(Market_t item)
        {
            return item.MICCode;
        }
    }
}
