using Prana.LogManager;
using System;

namespace Prana.PM.Client.UI.Classes
{
    class PMConstantsHelper
    {
        public static int GetDefaultCloumnWiseDecimalDigits(string caption)
        {
            try
            {
                switch (caption)
                {
                    case PMConstants.CAP_PercentageAverageVolume:
                    case PMConstants.CAP_PercentageAverageVolumeDeltaAdjusted:
                    case PMConstants.CAP_PercentBetaAdjGrossExposureInBaseCurrency:
                    case PMConstants.CAP_PercentageChange:
                    case PMConstants.CAP_PercentDayPnLGrossMV:
                    case PMConstants.CAP_PercentDayPnLNetMV:
                    case PMConstants.CAP_DividendYield:
                    case PMConstants.CAP_PercentExposureInBaseCurrency:
                    case PMConstants.CAP_PercentageGainLossCostBasis:
                    case PMConstants.CAP_PercentageGainLoss:
                    case PMConstants.CAP_PercentGrossExposureInBaseCurrency:
                    case PMConstants.CAP_PercentUnderlyingGrossExposureInBaseCurrency:
                    case PMConstants.CAP_PercentGrossMarketValueInBaseCurrency:
                    case PMConstants.CAP_PercentNetExposureInBaseCurrency:
                    case PMConstants.CAP_PercentNetMarketValueInBaseCurrency:
                    case PMConstants.CAP_PercentagePNLContribution:
                    case PMConstants.CAP_Volatility:
                    case PMConstants.CAP_PercentageUnderlyingChange:
                    case PMConstants.CAP_PercentOfITMOTM:
                    case PMConstants.CAP_IntrinsicValue:
                    case PMConstants.CAP_GainLossIfExerciseAssign:
                        return 2;

                    case PMConstants.CAP_ClosingPrice:
                    case PMConstants.CAP_Delta:
                    case PMConstants.CAP_ForwardPoints:
                    case PMConstants.CAP_AskPrice:
                    case PMConstants.CAP_BidPrice:
                    case PMConstants.CAP_LastPrice:
                    case PMConstants.CAP_MidPrice:
                    case PMConstants.CAP_SelectedFeedPriceInBaseCurrency:
                    case PMConstants.CAP_SelectedFeedPrice:
                    case PMConstants.CAP_FXRate:
                    case PMConstants.CAP_FXRateDisplay:
                    case PMConstants.CAP_YesterdayFXRate:
                        return 4;

                    default:
                        return 0;
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
                return 4;
            }
        }

        public static string GetFormatStringByCaption(string caption)
        {
            try
            {
                PMAppearances pMAppearances = PMAppearanceManager.PMAppearance;
                int decPlacesLimit = 0;
                if (pMAppearances.DecimalPlaceLimitsForColumns != null)
                {
                    if (pMAppearances.DecimalPlaceLimitsForColumns.ContainsKey(caption))
                    {
                        decPlacesLimit = pMAppearances.DecimalPlaceLimitsForColumns[caption];
                    }
                    else
                    {
                        decPlacesLimit = GetDefaultCloumnWiseDecimalDigits(caption);
                    }
                }
                else
                {
                    decPlacesLimit = GetDefaultCloumnWiseDecimalDigits(caption);
                }
                if (pMAppearances.ShowNegativeValuesWithBrackets)
                {
                    switch (decPlacesLimit)
                    {
                        case 0:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 1:
                            return PMConstants.FORMAT_ONE_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 2:
                            return PMConstants.FORMAT_TWO_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 3:
                            return PMConstants.FORMAT_THREE_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 4:
                            return PMConstants.FORMAT_FOUR_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 5:
                            return PMConstants.FORMAT_FIVE_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 6:
                            return PMConstants.FORMAT_SIX_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 7:
                            return PMConstants.FORMAT_SEVEN_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 8:
                            return PMConstants.FORMAT_EIGHT_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 9:
                            return PMConstants.FORMAT_NINE_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 10:
                            return PMConstants.FORMAT_TEN_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 11:
                            return PMConstants.FORMAT_ELEVEN_DECIMAL_DIGITS_WITH_BRACKETS;
                        case 12:
                            return PMConstants.FORMAT_TWELVE_DECIMAL_DIGITS_WITH_BRACKETS;
                        default:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS_WITH_BRACKETS;
                    }
                }
                else if (caption.Equals(PMConstants.CAP_SharesOutstanding))
                {
                    switch (decPlacesLimit)
                    {
                        case 0:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS_MILLIONS;
                        case 1:
                            return PMConstants.FORMAT_ONE_DECIMAL_DIGITS_MILLIONS;
                        case 2:
                            return PMConstants.FORMAT_TWO_DECIMAL_DIGITS_MILLIONS;
                        case 3:
                            return PMConstants.FORMAT_THREE_DECIMAL_DIGITS_MILLIONS;
                        case 4:
                            return PMConstants.FORMAT_FOUR_DECIMAL_DIGITS_MILLIONS;
                        case 5:
                            return PMConstants.FORMAT_FIVE_DECIMAL_DIGITS_MILLIONS;
                        case 6:
                            return PMConstants.FORMAT_SIX_DECIMAL_DIGITS_MILLIONS;
                        case 7:
                            return PMConstants.FORMAT_SEVEN_DECIMAL_DIGITS_MILLIONS;
                        case 8:
                            return PMConstants.FORMAT_EIGHT_DECIMAL_DIGITS_MILLIONS;
                        case 9:
                            return PMConstants.FORMAT_NINE_DECIMAL_DIGITS_MILLIONS;
                        case 10:
                            return PMConstants.FORMAT_TEN_DECIMAL_DIGITS_MILLIONS;
                        case 11:
                            return PMConstants.FORMAT_ELEVEN_DECIMAL_DIGITS_MILLIONS;
                        case 12:
                            return PMConstants.FORMAT_TWELVE_DECIMAL_DIGITS_MILLIONS;
                        default:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS_MILLIONS;
                    }
                }
                else
                {
                    switch (decPlacesLimit)
                    {
                        case 0:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS;
                        case 1:
                            return PMConstants.FORMAT_ONE_DECIMAL_DIGITS;
                        case 2:
                            return PMConstants.FORMAT_TWO_DECIMAL_DIGITS;
                        case 3:
                            return PMConstants.FORMAT_THREE_DECIMAL_DIGITS;
                        case 4:
                            return PMConstants.FORMAT_FOUR_DECIMAL_DIGITS;
                        case 5:
                            return PMConstants.FORMAT_FIVE_DECIMAL_DIGITS;
                        case 6:
                            return PMConstants.FORMAT_SIX_DECIMAL_DIGITS;
                        case 7:
                            return PMConstants.FORMAT_SEVEN_DECIMAL_DIGITS;
                        case 8:
                            return PMConstants.FORMAT_EIGHT_DECIMAL_DIGITS;
                        case 9:
                            return PMConstants.FORMAT_NINE_DECIMAL_DIGITS;
                        case 10:
                            return PMConstants.FORMAT_TEN_DECIMAL_DIGITS;
                        case 11:
                            return PMConstants.FORMAT_ELEVEN_DECIMAL_DIGITS;
                        case 12:
                            return PMConstants.FORMAT_TWELVE_DECIMAL_DIGITS;
                        default:
                            return PMConstants.FORMAT_ZERO_DECIMAL_DIGITS;
                    }
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
                return string.Empty;
            }
        }

        public static string GetFormatStringByCaptionForSummary(string caption)
        {
            try
            {
                string formatString = string.Empty;
                string formatStringForSummary = string.Empty;
                formatString = GetFormatStringByCaption(caption);
                formatStringForSummary = "{0:" + formatString + "}";
                return formatStringForSummary;
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
                return string.Empty;
            }
        }

        public static string GetColumnNameByCaption(string caption)
        {
            try
            {
                if (PMConstants.columnFromCaptionMappingDictionary.ContainsKey(caption))
                {
                    return PMConstants.columnFromCaptionMappingDictionary[caption];
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

            return caption;
        }

        public static string GetCaptionByColumnName(string columnName)
        {
            try
            {
                if (PMConstants.captionFromColumnMappingDictionary.ContainsKey(columnName))
                    return PMConstants.captionFromColumnMappingDictionary[columnName];
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
            return columnName;
        }
    }
}