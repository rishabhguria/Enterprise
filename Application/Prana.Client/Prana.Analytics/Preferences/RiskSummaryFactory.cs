using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.Analytics
{
    public class RiskSummaryFactory : AbstractSummaryFactory
    {
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
                    case "SummaryCalcWeightedSum":
                        summCalc = new SummaryCalcWeightedSum();
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
                    case "SummaryCalcSymbolSum":
                        summCalc = new SummaryCalcSymbolSum();
                        break;
                    case "SummaryCalcUnderlyingSum":
                        summCalc = new SummaryCalcUnderlyingSum();
                        break;
                    case "SummaryCalcLocalColumns":
                        summCalc = new SummaryCalcLocalColumns();
                        break;
                    default:
                        summCalc = new SummaryCalcText();
                        break;
                }
                return summCalc;
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
            return (new SummaryCalcText());
        }
    }
}
