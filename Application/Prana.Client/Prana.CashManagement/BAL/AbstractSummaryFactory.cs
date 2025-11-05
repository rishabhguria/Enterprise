using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.CashManagement
{
    public abstract class AbstractSummaryFactory
    {
        #region Methods

        /// <summary>
        /// Gets the summary calculator.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the summary from concrete factory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected abstract ICustomSummaryCalculator GetSummaryFromConcreteFactory(string key);

        #endregion Methods
    }
}
