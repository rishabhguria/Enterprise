using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.CashManagement
{
    public class CashSummaryFactory : AbstractSummaryFactory
    {
        #region Methods

        /// <summary>
        /// Gets the summary from concrete factory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected override ICustomSummaryCalculator GetSummaryFromConcreteFactory(string key)
        {
            ICustomSummaryCalculator summCalc;
            try
            {
                switch (key)
                {
                    case "SummaryCalcSum":
                        summCalc = new SummaryCalcSum();
                        break;

                    case "SummaryCalcText":
                        summCalc = new SummaryCalcText();
                        break;

                    case "SummaryCalcNum":
                        summCalc = new SummaryCalcNum();
                        break;

                    case "SummaryCalcDate":
                        summCalc = new SummaryCalcDate();
                        break;

                    case "SummaryCalcDisplayText":
                        summCalc = new SummaryCalcDisplayText();
                        break;

                    case "SummaryCalcNumText":
                        summCalc = new SummaryCalcNumText();
                        break;

                    default:
                        summCalc = new SummaryCalcText();
                        break;
                }
                return summCalc;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return (new SummaryCalcText());
        }

        #endregion Methods
    }
}
