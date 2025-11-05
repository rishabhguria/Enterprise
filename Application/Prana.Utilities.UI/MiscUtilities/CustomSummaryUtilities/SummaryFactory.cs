using Infragistics.Win.UltraWinGrid;

namespace Prana.Utilities.UI.MiscUtilities
{
    // Derive Concrete factory to get instances of summary calculator
    public abstract class SummaryFactory
    {
        protected abstract ICustomSummaryCalculator GetSummaryFromConcreteFactory(string key);
        public ICustomSummaryCalculator GetSummaryCalculator(string key)
        {
            ICustomSummaryCalculator summayCalculator;

            summayCalculator = GetSummaryFromConcreteFactory(key);

            return summayCalculator;
        }

    }
}
