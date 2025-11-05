using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.Analytics
{
    public abstract class AbstractSummaryFactory
    {
        protected abstract ICustomSummaryCalculator GetSummaryFromConcreteFactory(string key);
        public ICustomSummaryCalculator GetSummaryCalculator(string key)
        {
            ICustomSummaryCalculator summayCalculator = null;
            try
            {
                //Template Method
                summayCalculator = GetSummaryFromConcreteFactory(key);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return summayCalculator;
        }
    }
}
