using System.Configuration;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Text.RegularExpressions;



namespace Prana.BusinessLogic.SymbolUtilities.BAL
{
    public static class BloombergAssetCategory
    {
        private static string _regexOfEquityAsset = ConfigurationManager.AppSettings["BloombergEquityRegex"];
        private static string _regexOfEquityOptionAsset = ConfigurationManager.AppSettings["BloombergEquityOptionRegex"];
        private static string _regexOfFutureAsset = ConfigurationManager.AppSettings["BloombergFutureRegex"];
        private static string _regexOfFutureOptionAsset = ConfigurationManager.AppSettings["BloombergFutureOptionRegex"];
        private static string _regexOfFXAsset = ConfigurationManager.AppSettings["BloombergFXRegex"];
        private static string _regexOfFXForwardAsset = ConfigurationManager.AppSettings["BloombergFXForwardRegex"];
        private static string _regexOfFixedIncome = ConfigurationManager.AppSettings["BloombergFixedIncomeRegex"];

        /// <summary>
        /// using regex expression this method will return Asset category
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static AssetCategory GetAssetCategoryUsingRegex(string symbol)
        {
            try
            {
                Regex regexPattern = new Regex(_regexOfEquityAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.Equity;
                }

                regexPattern = new Regex(_regexOfEquityOptionAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.EquityOption;
                }

                regexPattern = new Regex(_regexOfFutureAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.Future;
                }

                regexPattern = new Regex(_regexOfFutureOptionAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FutureOption;
                }

                regexPattern = new Regex(_regexOfFXAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FX;
                }

                regexPattern = new Regex(_regexOfFXForwardAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FXForward;
                }

                regexPattern = new Regex(_regexOfFixedIncome);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FixedIncome;
                }
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
            return AssetCategory.None;
        }

    }
}
