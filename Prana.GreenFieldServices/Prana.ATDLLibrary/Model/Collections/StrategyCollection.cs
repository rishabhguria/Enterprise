using Prana.ATDLLibrary.Model.Elements;
using System.Collections.ObjectModel;

namespace Prana.ATDLLibrary.Model.Collections
{
    public class StrategyCollection : KeyedCollection<string, Strategy_t>
    {
        public StrategyCollection()
        {
        }

        protected override string GetKeyForItem(Strategy_t strategy)
        {
            return strategy.Name;
        }

        public new void Add(Strategy_t item)
        {
            base.Add(item);
        }
    }
}
