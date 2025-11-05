using Prana.BusinessObjects.AppConstants;
using Prana.CalculationService.Constants;
using Prana.LogManager;
using System;

namespace Prana.CalculationService
{
    internal static class HelperFunctions
    {
        /// <summary>
        /// Method to calculate selected feed price in base currency for different Assets.
        /// </summary>
        /// <param name="selectedFeedPriceLocal"></param>
        /// <param name="currentFxRate"></param>
        /// <param name="assetID"></param>
        /// <param name="fxConversionMethod"></param>
        /// <returns>selectedFeedPriceBase</returns>
        internal static double GetSelectedFeedPriceInBaseCurrency(double selectedFeedPriceLocal, double currentFxRate, int assetID, string fxConversionMethod)
        {
            double selectedFeedPriceBase = 0;
            try
            {
                if (assetID == (int)AssetCategory.FX || assetID == (int)AssetCategory.FXForward || assetID == (int)AssetCategory.Forex)
                {
                    if (fxConversionMethod.Equals(RtpnlConstants.CONST_Multiply))
                        selectedFeedPriceBase = 1 * selectedFeedPriceLocal;
                    else if (fxConversionMethod.Equals(RtpnlConstants.CONST_Divide) && selectedFeedPriceLocal != 0)
                        selectedFeedPriceBase = 1 / selectedFeedPriceLocal;
                }
                else
                {
                    if (fxConversionMethod.Equals(RtpnlConstants.CONST_Multiply))
                        selectedFeedPriceBase = selectedFeedPriceLocal * currentFxRate;
                    else if (fxConversionMethod.Equals(RtpnlConstants.CONST_Divide) && currentFxRate != 0)
                        selectedFeedPriceBase = selectedFeedPriceLocal / currentFxRate;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedFeedPriceBase;
        }
    }
}
