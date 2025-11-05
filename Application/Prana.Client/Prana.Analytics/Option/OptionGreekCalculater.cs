using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Analytics
{
    class OptionGreekCalculater
    {
        public static double CalculateWeightedDelta(List<WeightedGreekCalcInputs> greeksInputs)
        {
            double weightedDelta = 0.0;
            try
            {
                if (greeksInputs.Count > 0)
                {
                    weightedDelta = greeksInputs[0].Delta;
                    foreach (WeightedGreekCalcInputs input in greeksInputs)
                    {
                        if (input.Delta != weightedDelta)
                        {
                            weightedDelta = 0.0;
                            break;
                        }
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
            }
            return weightedDelta;
        }
        public static double CalculateWeightedGamma(List<WeightedGreekCalcInputs> greeksInputs)
        {
            double weightedGamma = 0.0;
            try
            {
                if (greeksInputs.Count > 0)
                {
                    weightedGamma = greeksInputs[0].Gamma;
                    foreach (WeightedGreekCalcInputs input in greeksInputs)
                    {
                        if (input.Gamma != weightedGamma)
                        {
                            weightedGamma = 0.0;
                            break;
                        }
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
            }
            return weightedGamma;
        }
        public static double CalculateWeightedTheta(List<WeightedGreekCalcInputs> greeksInputs)
        {
            double weightedTheta = 0.0;
            try
            {
                if (greeksInputs.Count > 0)
                {
                    weightedTheta = greeksInputs[0].Theta;
                    foreach (WeightedGreekCalcInputs input in greeksInputs)
                    {
                        if (input.Theta != weightedTheta)
                        {
                            weightedTheta = 0.0;
                            break;
                        }
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
            }
            return weightedTheta;
        }
        public static double CalculateWeightedVega(List<WeightedGreekCalcInputs> greeksInputs)
        {
            double weightedVega = 0.0;
            try
            {
                if (greeksInputs.Count > 0)
                {
                    weightedVega = greeksInputs[0].Vega;
                    foreach (WeightedGreekCalcInputs input in greeksInputs)
                    {
                        if (input.Vega != weightedVega)
                        {
                            weightedVega = 0.0;
                            break;
                        }
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
            }
            return weightedVega;
        }
        public static double CalculateWeightedRho(List<WeightedGreekCalcInputs> greeksInputs)
        {
            double weightedRho = 0.0;
            try
            {
                if (greeksInputs.Count > 0)
                {
                    weightedRho = greeksInputs[0].Rho;
                    foreach (WeightedGreekCalcInputs input in greeksInputs)
                    {
                        if (input.Rho != weightedRho)
                        {
                            weightedRho = 0.0;
                            break;
                        }
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
            }
            return weightedRho;
        }

        public static double CalculateNetDollarDelta(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            return CalculateNetDeltaAdjExposure(weightedGreekCalcInputs);
        }
        public static double CalculateNetDollarGamma(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDollarGamma = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    //netDollarGamma += 0.5 * input.AbsQuantity * input.StockPrice * input.StockPrice * input.Gamma * input.SideMultiplier;

                    netDollarGamma += 0.01 * input.AbsQuantity * input.SimulatedUnderlyingStockPrice * input.SimulatedUnderlyingStockPrice * input.Gamma * input.SideMultiplier * input.Multiplier;
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
            return netDollarGamma;
        }
        public static double CalculateNetDollarRho(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDollarRho = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDollarRho += input.AbsQuantity * input.Multiplier * input.Rho * input.SideMultiplier;
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
            return netDollarRho;
        }
        public static double CalculateNetDollarTheta(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDollarTheta = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDollarTheta += input.AbsQuantity * input.Multiplier * input.Theta * input.SideMultiplier;
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
            return netDollarTheta;
        }
        public static double CalculateNetDollarVega(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDollarVega = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDollarVega += input.AbsQuantity * input.Multiplier * input.Vega * input.SideMultiplier;
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
            return netDollarVega;
        }

        public static double CalculateNetDeltaAdjExposure(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDeltaAdjExposure = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    //Bharat (31 December 2013)
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                    //if (input.Asset.Equals(AssetCategory.FixedIncome) || input.Asset.Equals(AssetCategory.ConvertibleBond))
                    //{
                    //    netDeltaAdjExposure += Formulae.Formulae.GetNetExposure(input.AbsQuantity, input.StockPrice, input.Multiplier / 100, input.SideMultiplier, input.Delta, input.UnderlyingDelta);
                    //}
                    //else
                    {
                        netDeltaAdjExposure += BusinessLogic.Calculations.GetNetExposure(input.AbsQuantity, input.SimulatedUnderlyingStockPrice, input.Multiplier, input.SideMultiplier, input.Delta, input.UnderlyingDelta);
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
            }
            return netDeltaAdjExposure;
        }
        public static double CalculateNetDeltaAdjPosition(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDeltaAdjPosition = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDeltaAdjPosition += CalculateNetDeltaAdjPosition(input.AbsQuantity, input.Delta, (int)input.Asset, input.SideMultiplier, input.Multiplier);
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
            return netDeltaAdjPosition;
        }
        public static double CalculateNetDeltaAdjPosition(double quantity, double delta, int assetID, int sideMultiplier, double contractMultiplier)
        {
            double netDeltaAdjPosition = 0;
            try
            {
                if (assetID.Equals((int)AssetCategory.Future) || assetID.Equals((int)AssetCategory.FutureOption))
                {
                    netDeltaAdjPosition = Math.Abs(quantity) * delta * sideMultiplier;
                }
                else
                {
                    netDeltaAdjPosition = Math.Abs(quantity) * contractMultiplier * delta * sideMultiplier;
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
            return netDeltaAdjPosition;
        }
        public static double CalculateNetDeltaAdjPositionLME(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDeltaAdjPositionLME = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    double deltaAdjPositionLME = CalculateNetDeltaAdjPosition(input.AbsQuantity, input.Delta, (int)input.Asset, input.SideMultiplier, input.Multiplier);
                    if (input.ExchangeName.ToUpper().Equals("COMX") && input.Symbol.ToUpper().StartsWith("HG") && (input.Asset.Equals(AssetCategory.Future) || input.Asset.Equals(AssetCategory.FutureOption)))
                    {
                        deltaAdjPositionLME *= ApplicationConstants.DELTAADJPOSITION_LME_MULTIPLIER;
                    }

                    netDeltaAdjPositionLME += deltaAdjPositionLME;
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
            return netDeltaAdjPositionLME;
        }
        public static double CalculateNetGammaAdjPosition(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netGammaAdjPosition = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (input.Asset.Equals(AssetCategory.EquityOption))
                    {
                        netGammaAdjPosition += input.AbsQuantity * input.Gamma * input.Multiplier * input.SideMultiplier;
                    }
                    else if (input.Asset.Equals(AssetCategory.FutureOption))
                    {
                        netGammaAdjPosition += input.AbsQuantity * input.Gamma * input.SideMultiplier;
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
            }
            return netGammaAdjPosition;
        }

        public static double CalculateNetCostBasisPnl(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netCostBasisPnl = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netCostBasisPnl += BusinessLogic.Calculations.GetPnL(input.AbsQuantity, input.SimulatedPrice, input.AvgPrice, input.Multiplier, input.SideMultiplier);
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
            return netCostBasisPnl;
        }
        public static double CalculateNetDayPnl(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDayPnl = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (input.PutOrCalls.Equals(' ') || (!input.PutOrCalls.Equals(' ')))
                    {
                        netDayPnl += input.AbsQuantity * input.Multiplier * input.SideMultiplier * (input.SimulatedPrice - input.SelectedFeedPrice);
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
            }
            return netDayPnl;
        }

        /// <summary>
        /// Calculate Market value(based on Simulated Market Price)
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-4318
        /// </summary>
        /// <param name="weightedGreekCalcInputs"></param>
        /// <returns></returns>
        public static double CalculateMarketValue(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netMarketValue = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netMarketValue += BusinessLogic.Calculations.GetMarketValue(input.AbsQuantity, input.SimulatedPrice, input.Multiplier, input.SideMultiplier);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return netMarketValue;
        }

        public static string CalculateMergedSymbol(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedSymbol = String.Empty;
            try
            {
                List<string> symbols = new List<string>();

                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedSymbol))
                        {
                            mergedSymbol = input.Symbol;
                        }
                        else
                        {
                            mergedSymbol += ", " + input.Symbol;
                        }
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
            }
            return mergedSymbol;
        }
        public static string CalculateMergedUnderlyingSymbol(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedUnderlyingSymbol = String.Empty;
            try
            {
                List<string> symbols = new List<string>();

                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedUnderlyingSymbol))
                        {
                            mergedUnderlyingSymbol = input.UnderlyingSymbol;
                        }
                        else
                        {
                            mergedUnderlyingSymbol += ", " + input.UnderlyingSymbol;
                        }
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
            }
            return mergedUnderlyingSymbol;
        }
        //public static string CalculateMergedStockPrice(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        //{
        //    string mergedStockPrice = String.Empty;
        //    try
        //    {
        //        List<string> symbols = new List<string>();

        //        foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
        //        {
        //            if (!symbols.Contains(input.Symbol))
        //            {
        //                symbols.Add(input.Symbol);
        //                if (String.IsNullOrEmpty(mergedStockPrice))
        //                {
        //                    mergedStockPrice = String.Format("{0:#.00}", input.SimulatedUnderlyingStockPrice);
        //                }
        //                else
        //                {
        //                    mergedStockPrice += ", " + String.Format("{0:#.00}", input.SimulatedUnderlyingStockPrice);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return mergedStockPrice;
        //}
        public static string CalculateMergedDaysToExpiration(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedDaysToExpiration = String.Empty;
            try
            {
                List<string> symbols = new List<string>();

                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedDaysToExpiration))
                        {
                            mergedDaysToExpiration = input.DaysToExpration.ToString();
                        }
                        else
                        {
                            mergedDaysToExpiration += ", " + input.DaysToExpration.ToString();
                        }
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
            }
            return mergedDaysToExpiration;
        }
        public static string CalculateMergedInterestRate(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedInterestRate = String.Empty;
            try
            {
                List<string> symbols = new List<string>();

                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedInterestRate))
                        {
                            mergedInterestRate = String.Format("{0:#.00}", input.InterestRate);
                        }
                        else
                        {
                            mergedInterestRate += ", " + String.Format("{0:#.00}", input.InterestRate);
                        }
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
            }

            return mergedInterestRate;
        }
        public static string CalculateMergedImpVolatility(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedImpVolatility = String.Empty;
            try
            {
                List<string> symbols = new List<string>();
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedImpVolatility))
                        {
                            mergedImpVolatility = String.Format("{0:#.00}", input.Volatility);
                        }
                        else
                        {
                            mergedImpVolatility += ", " + String.Format("{0:#.00}", input.Volatility);
                        }
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
            }
            return mergedImpVolatility;
        }
        //public static double CalculateMergedOptionPrice(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        //{
        //    double mergedOptionPrice = weightedGreekCalcInputs[0].SimulatedPrice;
        //    try
        //    {
        //        string symbol = weightedGreekCalcInputs[0].Symbol;
        //        foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
        //        {
        //            if (input.Symbol != symbol)
        //            {
        //                mergedOptionPrice = 0.0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return mergedOptionPrice;
        //}

        public static double CalculateNetDollarDeltaInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            return CalculateNetDeltaAdjExposureInBaseCurrency(weightedGreekCalcInputs);
        }
        public static double CalculateNetDeltaAdjExposureInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDeltaAdjExposure = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDeltaAdjExposure += BusinessLogic.Calculations.GetNetExposure(input.AbsQuantity, input.SimulatedUnderlyingStockPriceInBaseCurrency, input.Multiplier, input.SideMultiplier, input.Delta, input.UnderlyingDelta);
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
            return netDeltaAdjExposure;
        }
        public static double CalculateNetDollarGammaInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDollarGammaInBaseCurrency = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netDollarGammaInBaseCurrency += 0.01 * input.AbsQuantity * input.SimulatedUnderlyingStockPriceInBaseCurrency * input.SimulatedUnderlyingStockPriceInBaseCurrency * input.Gamma * input.SideMultiplier * input.Multiplier;
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
            return netDollarGammaInBaseCurrency;
        }
        public static double CalculateMergedOptionPriceInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double mergedOptionPriceInBaseCurrency = weightedGreekCalcInputs[0].SimulatedPriceInBaseCurrency;
            try
            {
                string symbol = weightedGreekCalcInputs[0].Symbol;
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (input.Symbol != symbol)
                    {
                        mergedOptionPriceInBaseCurrency = 0.0;
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
            }
            return mergedOptionPriceInBaseCurrency;
        }
        public static double CalculateNetCostBasisPnlInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netCostBasisPnlInBaseCurrency = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    netCostBasisPnlInBaseCurrency += BusinessLogic.Calculations.GetPnL(input.AbsQuantity, input.SimulatedPriceInBaseCurrency, input.AvgPriceInBaseCurrency, input.Multiplier, input.SideMultiplier);
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
            return netCostBasisPnlInBaseCurrency;
        }
        public static double CalculateNetDayPnlInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            double netDayPnlInBaseCurrency = 0;
            try
            {
                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (input.PutOrCalls.Equals(' ') || (!input.PutOrCalls.Equals(' ')))
                    {
                        netDayPnlInBaseCurrency += input.AbsQuantity * input.Multiplier * input.SideMultiplier * (input.SimulatedPriceInBaseCurrency - input.SelectedFeedPriceInBaseCurrency);
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
            }
            return netDayPnlInBaseCurrency;
        }

        public static string CalculateMergedStockPriceInBaseCurrency(List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            string mergedStockPriceInBaseCurrency = String.Empty;
            try
            {
                List<string> symbols = new List<string>();

                foreach (WeightedGreekCalcInputs input in weightedGreekCalcInputs)
                {
                    if (!symbols.Contains(input.Symbol))
                    {
                        symbols.Add(input.Symbol);
                        if (String.IsNullOrEmpty(mergedStockPriceInBaseCurrency))
                        {
                            mergedStockPriceInBaseCurrency = String.Format("{0:#.00}", input.SimulatedUnderlyingStockPriceInBaseCurrency);
                        }
                        else
                        {
                            mergedStockPriceInBaseCurrency += ", " + String.Format("{0:#.00}", input.SimulatedUnderlyingStockPriceInBaseCurrency);
                        }
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
            }
            return mergedStockPriceInBaseCurrency;
        }
    }

    class WeightedGreekCalcInputs
    {
        public WeightedGreekCalcInputs()
        {
        }
        public WeightedGreekCalcInputs(PranaPositionWithGreeks posGreek)
        {
            _symbol = posGreek.Symbol;
            _underlyingSymbol = posGreek.UnderlyingSymbol;
            _absQuantity = Math.Abs(posGreek.Quantity);
            _delta = posGreek.Delta;
            _gamma = posGreek.Gamma;
            _theta = posGreek.Theta;
            _vega = posGreek.Vega;
            _rho = posGreek.Rho;
            _sideMultiplier = posGreek.SideMultiplier;
            _multiplier = posGreek.ContractMultiplier;
            _simulatedUnderlyingStockPrice = posGreek.SimulatedUnderlyingStockPrice;
            _simulatedPrice = posGreek.SimulatedPrice;
            _selectedFeedPrice = posGreek.SelectedFeedPrice;
            _avgPrice = posGreek.AvgPrice;
            if (posGreek.PutOrCall == 0)
            {
                _putOrCalls = 'P';
            }
            else if (posGreek.PutOrCall == 1)
            {
                _putOrCalls = 'C';
            }
            //_putOrCalls = posGreek.PutOrCalls;
            _currencyID = posGreek.CurrencyID;
            _strikePrice = posGreek.StrikePrice;
            _daysToExpiration = posGreek.DaysToExpiration;
            _underlyingDelta = posGreek.UnderlyingDelta;
            _asset = posGreek.AssetCategoryValue;
            _exchangeName = posGreek.ExchangeName;
            _avgPriceInBaseCurrency = posGreek.AvgPriceInBaseCurrency;
            _simulatedPriceInBaseCurrency = posGreek.SimulatedPriceInBaseCurrency;
            _simulatedUnderlyingStockPriceInBaseCurrency = posGreek.SimulatedUnderlyingStockPriceInBaseCurrency;
            _selectedFeedPriceInBaseCurrency = posGreek.SelectedFeedPriceInBaseCurrency;
        }

        private string _symbol;
        private string _underlyingSymbol;
        private double _absQuantity;
        private double _delta;
        private double _gamma;
        private double _theta;
        private double _vega;
        private double _rho;
        private double _volatility;
        private int _sideMultiplier;
        private double _multiplier;
        private double _simulatedUnderlyingStockPrice;
        private double _simulatedPrice;
        private double _selectedFeedPrice;
        private double _avgPrice;
        private int _daysToExpiration;
        private double _interestRate;
        private char _putOrCalls;
        private int _currencyID;
        private double _strikePrice;
        private double _underlyingDelta;
        private AssetCategory _asset;
        private string _exchangeName;
        private double _simulatedUnderlyingStockPriceInBaseCurrency;
        private double _simulatedPriceInBaseCurrency;
        private double _selectedFeedPriceInBaseCurrency;
        private double _avgPriceInBaseCurrency;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
        public double AbsQuantity
        {
            get { return _absQuantity; }
            set { _absQuantity = value; }
        }
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }
        public double Theta
        {
            get { return _theta; }
            set { _theta = value; }
        }
        public double Vega
        {
            get { return _vega; }
            set { _vega = value; }
        }
        public double Rho
        {
            get { return _rho; }
            set { _rho = value; }
        }
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        public double SimulatedUnderlyingStockPrice
        {
            get { return _simulatedUnderlyingStockPrice; }
            set { _simulatedUnderlyingStockPrice = value; }
        }
        public double SimulatedPrice
        {
            get { return _simulatedPrice; }
            set { _simulatedPrice = value; }
        }
        public double SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public int DaysToExpration
        {
            get { return _daysToExpiration; }
            set { _daysToExpiration = value; }
        }
        public double InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }
        public char PutOrCalls
        {
            get { return _putOrCalls; }
            set { _putOrCalls = value; }
        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        public double UnderlyingDelta
        {
            get { return _underlyingDelta; }
            set { _underlyingDelta = value; }
        }
        public AssetCategory Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }
        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }
        public double SelectedFeedPriceInBaseCurrency
        {
            get { return _selectedFeedPriceInBaseCurrency; }
            set { _selectedFeedPriceInBaseCurrency = value; }
        }
        public double SimulatedPriceInBaseCurrency
        {
            get { return _simulatedPriceInBaseCurrency; }
            set { _simulatedPriceInBaseCurrency = value; }
        }
        public double SimulatedUnderlyingStockPriceInBaseCurrency
        {
            get { return _simulatedUnderlyingStockPriceInBaseCurrency; }
            set { _simulatedUnderlyingStockPriceInBaseCurrency = value; }
        }
        public double AvgPriceInBaseCurrency
        {
            get { return _avgPriceInBaseCurrency; }
            set { _avgPriceInBaseCurrency = value; }
        }
    }
}
