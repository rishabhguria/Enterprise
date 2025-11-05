using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using System;
using System.Globalization;

namespace Prana.BusinessLogic
{
    public class Calculations
    {

        //static private PMCalculations _PMCalculations = null;
        /// <summary>
        /// 
        /// </summary>
        public Calculations()
        {

        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static PMCalculations GetInstance()
        //{
        //    if (_PMCalculations == null)
        //    {
        //        _PMCalculations = new PMCalculations();
        //    }
        //    return _PMCalculations;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public static void SetAveragePriceRealizedPNL(Position position)
        {
            int sideMultiplier = 1;
            double intLegPnlForPosition = 0.0;

            if (!(position.PositionalTag == PositionTag.LongWithdrawal || position.PositionalTag == PositionTag.ShortWithdrawal || position.ClosingPositionTag == PositionTag.LongWithdrawal || position.ClosingPositionTag == PositionTag.ShortWithdrawal))
            {
                if (position.PositionalTag == PositionTag.Long || position.PositionalTag == PositionTag.LongAddition)
                {
                    sideMultiplier = 1;
                }
                else
                {
                    sideMultiplier = -1;
                }


                if (position.IsPositionSwapped)
                {
                    intLegPnlForPosition = GetInterestLegPnL(position.PositionSwapParameters, position.TradeDate, position.ClosingTradeDate, position.PositionalTag);

                }

                position.CostBasisRealizedPNL = 0.0;


                double pnlGenerated = 0.0;
                //Reduce the commission and fees of open quantity part.
                double totalExpensesForClosingTaxlot = position.ClosingTotalCommissionandFees;
                double openAvgPrice = position.OpenAveragePrice;
                double closeAvgPrice = position.ClosedAveragePrice;
                double closeQty = position.ClosedQty;

                //TODO : Future multipliers, we need to fetch from config file.
                //if (position.ClosingMode.Equals(ClosingMode.None) || position.ClosingMode.Equals(ClosingMode.Offset))
                //{
                if (position.AssetCategoryValue == AssetCategory.FX && position.CurrencyID != position.LeadCurrencyID)
                {
                    if (openAvgPrice != 0)
                        openAvgPrice = 1 / openAvgPrice;
                    if (closeAvgPrice != 0)
                        closeAvgPrice = 1 / closeAvgPrice;
                    pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);
                }
                else if (position.AssetCategoryValue == AssetCategory.FXForward)
                {
                    if (position.CurrencyID != position.LeadCurrencyID)
                    {
                        if (position.CurrencyID == 1)
                        {
                            if (openAvgPrice != 0)
                                closeQty = position.ClosedQty / openAvgPrice;
                            pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);
                            if (position.FxRate != 0)
                                pnlGenerated = pnlGenerated / position.FxRate;
                            pnlGenerated = -pnlGenerated;
                        }
                        else
                        {
                            if (openAvgPrice != 0)
                                openAvgPrice = 1 / openAvgPrice;
                            if (closeAvgPrice != 0)
                                closeAvgPrice = 1 / closeAvgPrice;
                            pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);
                        }
                    }
                    else
                    {
                        if (position.CurrencyID == 1)
                        {
                            closeQty = position.ClosedQty * openAvgPrice;
                            if (openAvgPrice != 0)
                                openAvgPrice = 1 / openAvgPrice;
                            if (closeAvgPrice != 0)
                                closeAvgPrice = 1 / closeAvgPrice;
                            pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);
                            pnlGenerated = pnlGenerated * position.FxRate;
                            pnlGenerated = -pnlGenerated;
                        }
                        else
                            pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);
                    }
                }
                else
                    pnlGenerated = GetPnL(closeQty, closeAvgPrice, openAvgPrice, position.Multiplier, sideMultiplier);

                //}
                //else //if (position.ClosingMode.Equals(ClosingMode.SwapExpire) || position.ClosingMode.Equals(ClosingMode.SwapExpireAndRollover)||position.ClosingMode.Equals(ClosingMode.Expire)||position.ClosingMode.Equals(ClosingMode.Cash) )
                //{
                //    pnlGenerated = Formulae.Formulae.GetPnL(position.ClosedQty, position.CashSettledPrice, position.OpenAveragePrice, position.Multiplier, sideMultiplier);


                //}


                double taxlotExpenseAdjustedPNL = pnlGenerated - totalExpensesForClosingTaxlot;
                double totalExpensesForPosition = position.PositionTotalCommissionandFees;
                position.CostBasisRealizedPNL = taxlotExpenseAdjustedPNL - totalExpensesForPosition;
                position.CostBasisGrossPNL = pnlGenerated;
            }

            position.CostBasisGrossPNL += intLegPnlForPosition;
            position.CostBasisRealizedPNL += intLegPnlForPosition;

        }




        private static double GetInterestLegPnL(SwapParameters swapParams, DateTime startDate, DateTime finalDate, PositionTag positionTag)
        {
            double intLegPnlToBeAdded = 0.0;
            double interestRate = swapParams.BenchMarkRate + swapParams.Differential;
            //TODO: check number of days from Current Auec time -Start Auec Local time
            TimeSpan tDiff = finalDate.Date - startDate.Date;
            double dayDiff = (double)Math.Ceiling(tDiff.TotalDays);
            double daycount = swapParams.DayCount;

            double intLegPnL = GetInterestValue(swapParams.NotionalValue, interestRate, daycount, dayDiff);
            if (positionTag == PositionTag.Long)
            {
                intLegPnlToBeAdded = 0.0 - intLegPnL;
            }
            else
            {
                intLegPnlToBeAdded = intLegPnL;
            }
            return intLegPnlToBeAdded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openingPositionTag"></param>
        /// <param name="closingPositionTag"></param>
        /// <param name="isOpeningSwapped"></param>
        /// <param name="benchMarkRate"></param>
        /// <param name="differential"></param>
        /// <param name="dayCount"></param>
        /// <param name="notionalValue"></param>
        /// <param name="positionTradeDate"></param>
        /// <param name="closingTradeDate"></param>
        /// <param name="closingTaxlotCommission"></param>
        /// <param name="closingTaxlotOtherBrokerFees"></param>
        /// <param name="closingTaxlotOtherFees"></param>
        /// <param name="closedQty"></param>
        /// <param name="closingPrice"></param>
        /// <param name="openPrice"></param>
        /// <param name="multiplier"></param>
        /// <param name="positionalTaxlotCommission"></param>
        /// <param name="positionalOtherBrokerFees"></param>
        /// <param name="positionalOtherFees"></param>
        /// <returns></returns>
        public static double GetRealizedPNL(int openingPositionTag, int closingPositionTag, int isOpeningSwapped, float benchMarkRate, float differential, int dayCount, float notionalValue, DateTime positionTradeDate, DateTime closingTradeDate, float closingTaxlotCommission, float closingTaxlotOtherBrokerFees, float closingTaxlotOtherFees, float closedQty, float closingPrice, float openPrice, double multiplier, float positionalTaxlotCommission, float positionalOtherBrokerFees, float positionalOtherFees)
        {
            double realizedPNL = 0;
            int sideMultiplier = 1;
            //Position positionFromParameters = new Position();

            if (!((PositionTag)openingPositionTag == PositionTag.LongWithdrawal || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.LongWithdrawal || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
            {
                if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
                {
                    sideMultiplier = 1;
                }
                else
                {
                    sideMultiplier = -1;
                }
                bool isPosSwapped = false;
                if (isOpeningSwapped.Equals(0))
                {
                    isPosSwapped = false;
                }
                else
                {
                    isPosSwapped = false;
                }

                if (isPosSwapped)
                {
                    GetInterestLegPnLForReports(benchMarkRate, differential, dayCount, notionalValue, positionTradeDate, closingTradeDate, (PositionTag)openingPositionTag);
                }

                double pnlGenerated = 0.0;
                //Reduce the commission and fees of open quantity part.
                double totalExpensesForClosingTaxlot = closingTaxlotCommission + closingTaxlotOtherBrokerFees + closingTaxlotOtherFees;

                //double totalFeesForClosingTaxlot = closingTaxlotOtherBrokerFees + closingTaxlotOtherFees;

                //TODO : Future multipliers, we need to fetch from config file.
                pnlGenerated = GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);
                //While determining the CostBasisRealizedPNL of the position and taxlot, expenses shd only be accounted for the closed part of the position because of current taxlot.
                double taxlotExpenseAdjustedPNL = pnlGenerated - totalExpensesForClosingTaxlot;
                double totalExpensesForPosition = positionalTaxlotCommission + positionalOtherBrokerFees + positionalOtherFees;
                realizedPNL = taxlotExpenseAdjustedPNL - totalExpensesForPosition;
            }

            return realizedPNL;
        }

        public static double GetRealizedPNL(int openingPositionTag, int closingPositionTag, int isOpeningSwapped, float benchMarkRate, float differential, int dayCount, float notionalValue, DateTime positionTradeDate, DateTime closingTradeDate, float closingTaxlotTotalCommissionFees, float closedQty, float closingPrice, float openPrice, double multiplier, float positionalTaxlotTotalCommissionFees)
        {
            double realizedPNL = 0;
            int sideMultiplier = 1;

            //if (!((PositionTag)openingPositionTag == PositionTag.TCloseLong || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.TCloseLong || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
            //{
            //    if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
            //    {
            if (!((PositionTag)openingPositionTag == PositionTag.LongWithdrawal || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.LongWithdrawal || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
            {
                if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
                {
                    sideMultiplier = 1;
                }
                else
                {
                    sideMultiplier = -1;
                }
                bool isPosSwapped = false;
                if (isOpeningSwapped.Equals(0))
                {
                    isPosSwapped = false;
                }
                else
                {
                    isPosSwapped = false;
                }

                if (isPosSwapped)
                {
                    GetInterestLegPnLForReports(benchMarkRate, differential, dayCount, notionalValue, positionTradeDate, closingTradeDate, (PositionTag)openingPositionTag);
                }

                double pnlGenerated = 0.0;
                //Reduce the commission and fees of open quantity part.

                pnlGenerated = GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);

                //While determining the CostBasisRealizedPNL of the position and taxlot, expenses shd only be accounted for the closed part of the position because of current taxlot.
                //double taxlotExpenseAdjustedPNL = pnlGenerated - closingTaxlotTotalCommissionFees;
                //realizedPNL = taxlotExpenseAdjustedPNL - positionalTaxlotTotalCommissionFees;
                realizedPNL = pnlGenerated - (closingTaxlotTotalCommissionFees + positionalTaxlotTotalCommissionFees);

            }

            return realizedPNL;
        }

        private static double GetInterestLegPnLForReports(float benchMarkRate, float differential, int dayCount, float notionalValue, DateTime startDate, DateTime finalDate, PositionTag positionTag)
        {
            double intLegPnlToBeAdded = 0.0;
            double interestRate = benchMarkRate + differential;
            //TODO: check number of days from Current Auec time -Start Auec Local time
            TimeSpan tDiff = finalDate.Date - startDate.Date;
            double dayDiff = (double)Math.Ceiling(tDiff.TotalDays);
            double daycount = dayCount;

            double intLegPnL = GetInterestValue(notionalValue, interestRate, daycount, dayDiff);
            if (positionTag == PositionTag.Long)
            {
                intLegPnlToBeAdded = 0.0 - intLegPnL;
            }
            else
            {
                intLegPnlToBeAdded = intLegPnL;
            }
            return intLegPnlToBeAdded;
        }

        //public static double GetRealizedPNLBase(int openingPositionTag, int closingPositionTag, int isOpeningSwapped, float benchMarkRate, float differential, int dayCount, float notionalValue, DateTime positionTradeDate, DateTime closingTradeDate, float closingTaxlotTotalCommissionFees, float closedQty, float closingPrice, float openPrice, int multiplier, float settlementPrice, float positionalTaxlotTotalCommissionFees, int closingMode, float otConvFactorComputed, float otFXConvFactorComputed, float ctConvFactorComputed, float ctFXConvFactorComputed, int assetID)
        //{
        //    double realizedPNL = 0;
        //    int sideMultiplier = 1;

        //    if (!((PositionTag)openingPositionTag == PositionTag.LongWithdrawal || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.LongWithdrawal || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
        //    {
        //        if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
        //        {
        //            sideMultiplier = 1;
        //        }
        //        else
        //        {
        //            sideMultiplier = -1;
        //        }
        //        double intLegPnlForPosition = 0.0;
        //        bool isPosSwapped = false;
        //        if (isOpeningSwapped.Equals(0))
        //        {
        //            isPosSwapped = false;
        //        }
        //        else
        //        {
        //            isPosSwapped = false;
        //        }

        //        if (isPosSwapped)
        //        {
        //            intLegPnlForPosition = GetInterestLegPnLForReports(benchMarkRate, differential, dayCount, notionalValue, positionTradeDate, closingTradeDate, (PositionTag)openingPositionTag);
        //        }

        //        double pnlGenerated = 0.0;
        //        //Reduce the commission and fees of open quantity part.

        //        if ((AssetCategory)assetID == AssetCategory.FX)
        //        {
        //            openPrice = openPrice * otFXConvFactorComputed;
        //            //if ((ClosingMode)closingMode == ClosingMode.None || (ClosingMode)closingMode == ClosingMode.Offset)
        //            //{
        //            closingPrice = closingPrice * ctFXConvFactorComputed;
        //            pnlGenerated = Formulae.Formulae.GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);
        //            //}
        //            //else if ((ClosingMode)closingMode == ClosingMode.SwapExpire || (ClosingMode)closingMode == ClosingMode.SwapExpireAndRollover || (ClosingMode)closingMode == ClosingMode.Expire || (ClosingMode)closingMode == ClosingMode.Cash || (ClosingMode)closingMode == ClosingMode.Exercise || (ClosingMode)closingMode == ClosingMode.Physical)
        //            //{
        //            //    settlementPrice = settlementPrice * ctFXConvFactorComputed;
        //            //    pnlGenerated = Formulae.Formulae.GetPnL(closedQty, settlementPrice, openPrice, multiplier, sideMultiplier);
        //            //}
        //        }
        //        else
        //        {
        //            openPrice = openPrice * otConvFactorComputed;
        //            //if ((ClosingMode)closingMode == ClosingMode.None || (ClosingMode)closingMode == ClosingMode.Offset)
        //            //{
        //            closingPrice = closingPrice * ctConvFactorComputed;
        //            pnlGenerated = Formulae.Formulae.GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);
        //            //}
        //            //else if ((ClosingMode)closingMode == ClosingMode.SwapExpire || (ClosingMode)closingMode == ClosingMode.SwapExpireAndRollover || (ClosingMode)closingMode == ClosingMode.Expire || (ClosingMode)closingMode == ClosingMode.Cash || (ClosingMode)closingMode == ClosingMode.Exercise || (ClosingMode)closingMode == ClosingMode.Physical)
        //            //{
        //            //    settlementPrice = settlementPrice * ctConvFactorComputed;
        //            //    pnlGenerated = Formulae.Formulae.GetPnL(closedQty, settlementPrice, openPrice, multiplier, sideMultiplier);
        //            //}
        //        }
        //        //While determining the CostBasisRealizedPNL of the position and taxlot, expenses shd only be accounted for the closed part of the position because of current taxlot.
        //        //double taxlotExpenseAdjustedPNL = pnlGenerated - closingTaxlotTotalCommissionFees;
        //        //realizedPNL = taxlotExpenseAdjustedPNL - positionalTaxlotTotalCommissionFees;

        //        if ((AssetCategory)assetID == AssetCategory.FX)
        //        {
        //            //if ((ClosingMode)closingMode == ClosingMode.None || (ClosingMode)closingMode == ClosingMode.Offset)
        //            //{
        //            realizedPNL = pnlGenerated - ((closingTaxlotTotalCommissionFees * ctFXConvFactorComputed) + (positionalTaxlotTotalCommissionFees * otFXConvFactorComputed));
        //            //}
        //            //else if ((ClosingMode)closingMode == ClosingMode.SwapExpire || (ClosingMode)closingMode == ClosingMode.SwapExpireAndRollover || (ClosingMode)closingMode == ClosingMode.Expire || (ClosingMode)closingMode == ClosingMode.Cash || (ClosingMode)closingMode == ClosingMode.Exercise || (ClosingMode)closingMode == ClosingMode.Physical)
        //            //{
        //            //    //realizedPNL = pnlGenerated - (closingTaxlotTotalCommissionFees * ctFXConvFactorComputed);
        //            //    realizedPNL = pnlGenerated - (positionalTaxlotTotalCommissionFees * otFXConvFactorComputed);
        //            //}
        //        }
        //        else
        //        {
        //            //if ((ClosingMode)closingMode == ClosingMode.None || (ClosingMode)closingMode == ClosingMode.Offset)
        //            //{
        //            realizedPNL = pnlGenerated - ((closingTaxlotTotalCommissionFees * ctConvFactorComputed) + (positionalTaxlotTotalCommissionFees * otConvFactorComputed));
        //            //}
        //            //else if ((ClosingMode)closingMode == ClosingMode.SwapExpire || (ClosingMode)closingMode == ClosingMode.SwapExpireAndRollover || (ClosingMode)closingMode == ClosingMode.Expire || (ClosingMode)closingMode == ClosingMode.Cash || (ClosingMode)closingMode == ClosingMode.Exercise || (ClosingMode)closingMode == ClosingMode.Physical)
        //            //{
        //            //    //realizedPNL = pnlGenerated - (closingTaxlotTotalCommissionFees * ctConvFactorComputed);
        //            //    realizedPNL = pnlGenerated - (positionalTaxlotTotalCommissionFees * otConvFactorComputed);
        //            //}
        //        }
        //    }

        //    return realizedPNL;
        //}

        public static double GetRealizedPNLBase(int openingPositionTag, int closingPositionTag, int isOpeningSwapped, float benchMarkRate, float differential, int dayCount, float notionalValue, DateTime positionTradeDate, DateTime closingTradeDate, float closingTaxlotTotalCommissionFees, float closedQty, float closingPrice, float openPrice, double multiplier, float positionalTaxlotTotalCommissionFees, float otConvFactorComputed, float otFXConvFactorComputed, float ctConvFactorComputed, float ctFXConvFactorComputed, int assetID)
        {
            double realizedPNL = 0;
            int sideMultiplier = 1;

            //if (!((PositionTag)openingPositionTag == PositionTag.LongWithdrawal || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.LongWithdrawal || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
            //{
            //    if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
            //    {
            if (!((PositionTag)openingPositionTag == PositionTag.LongWithdrawal || (PositionTag)openingPositionTag == PositionTag.ShortWithdrawal || (PositionTag)closingPositionTag == PositionTag.LongWithdrawal || (PositionTag)closingPositionTag == PositionTag.ShortWithdrawal))
            {
                if ((PositionTag)openingPositionTag == PositionTag.Long || (PositionTag)openingPositionTag == PositionTag.LongAddition)
                {
                    sideMultiplier = 1;
                }
                else
                {
                    sideMultiplier = -1;
                }
                bool isPosSwapped = false;
                if (isOpeningSwapped.Equals(0))
                {
                    isPosSwapped = false;
                }
                else
                {
                    isPosSwapped = false;
                }

                if (isPosSwapped)
                {
                    GetInterestLegPnLForReports(benchMarkRate, differential, dayCount, notionalValue, positionTradeDate, closingTradeDate, (PositionTag)openingPositionTag);
                }

                double pnlGenerated = 0.0;
                //Reduce the commission and fees of open quantity part.

                if ((AssetCategory)assetID == AssetCategory.FX)
                {
                    openPrice = openPrice * otFXConvFactorComputed;
                    closingPrice = closingPrice * ctFXConvFactorComputed;
                    pnlGenerated = GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);

                }
                else
                {
                    openPrice = openPrice * otConvFactorComputed;
                    closingPrice = closingPrice * ctConvFactorComputed;
                    pnlGenerated = GetPnL(closedQty, closingPrice, openPrice, multiplier, sideMultiplier);
                }


                if ((AssetCategory)assetID == AssetCategory.FX)
                {
                    realizedPNL = pnlGenerated - ((closingTaxlotTotalCommissionFees * ctFXConvFactorComputed) + (positionalTaxlotTotalCommissionFees * otFXConvFactorComputed));
                }
                else
                {
                    realizedPNL = pnlGenerated - ((closingTaxlotTotalCommissionFees * ctConvFactorComputed) + (positionalTaxlotTotalCommissionFees * otConvFactorComputed));
                }
            }

            return realizedPNL;
        }


        public static int GetSideMultilpier(string sidetagvalue)
        {
            int sideMul = 1;
            switch (sidetagvalue)
            {
                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Buy_Closed:
                    sideMul = 1;
                    break;

                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_SellShort:
                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_Sell_Closed:
                    sideMul = -1;
                    break;

                default:
                    sideMul = 1;
                    break;
            }
            return sideMul;
        }

        public static void SetLongShort(EPnlOrder order)
        {
            if (order.ClassID == EPnLClassID.EPnLOrderOption)
            {
                if (((EPnLOrderOption)order).ContractType == OptionType.CALL.ToString())
                {
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                            order.TransactionSide = PositionType.Long;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_Buy_Closed:
                            order.TransactionSide = PositionType.Short;
                            break;

                        default:
                            order.TransactionSide = PositionType.Long;
                            break;
                    }
                }
                else if (((EPnLOrderOption)order).ContractType == OptionType.PUT.ToString())
                {
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                            order.TransactionSide = PositionType.Short;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_Buy_Closed:
                            order.TransactionSide = PositionType.Long;
                            break;

                        default:
                            order.TransactionSide = PositionType.Long;
                            break;
                    }
                }
                {
                    order.TransactionSide = PositionType.Long;
                }
            }
            else
            {
                switch (order.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        order.TransactionSide = PositionType.Long;
                        break;

                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Buy_Closed:
                        order.TransactionSide = PositionType.Short;
                        break;

                    default:
                        order.TransactionSide = PositionType.Long;
                        break;
                }
            }

        }


        public static void SetDefaultPositionSideExposureBoxed(EPnlOrder order)
        {
            if (order.ClassID == EPnLClassID.EPnLOrderOption)
            {
                if (((EPnLOrderOption)order).ContractType.ToUpper() == OptionType.CALL.ToString().ToUpper())
                {
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                            order.PositionSideExposureBoxed = PositionType.Long;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_Buy_Closed:
                            order.PositionSideExposureBoxed = PositionType.Short;
                            break;

                        default:
                            order.PositionSideExposureBoxed = PositionType.Long;
                            break;
                    }
                }
                else if (((EPnLOrderOption)order).ContractType.ToUpper() == OptionType.PUT.ToString().ToUpper())
                {
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                            order.PositionSideExposureBoxed = PositionType.Short;
                            break;

                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_Buy_Closed:
                            order.PositionSideExposureBoxed = PositionType.Long;
                            break;

                        default:
                            order.PositionSideExposureBoxed = PositionType.Long;
                            break;
                    }
                }
            }
            else
            {
                switch (order.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        order.PositionSideExposureBoxed = PositionType.Long;
                        break;

                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Buy_Closed:
                        order.PositionSideExposureBoxed = PositionType.Short;
                        break;

                    default:
                        order.PositionSideExposureBoxed = PositionType.Long;
                        break;
                }
            }
        }
        /// <summary>
        /// Gets Pnl for one day 
        /// </summary>
        /// <param name="quantity">number of contracts </param>
        /// <param name="currentPrice">price for day for which day pnl is required </param>
        /// <param name="previousPrice">previous price from which pnl is required</param>
        /// <param name="multiplier">contract multiplier (1 for equity)</param>
        /// <param name="sideMultiplier"> +1 for side long -1 for side short</param>
        /// <returns>returns PNL for a day</returns>
        /// 
        public static double GetUnderlyingValueForOptions(double position, double multiplier, double underlyingStockPrice)
        {
            return (position * multiplier * underlyingStockPrice);
        }

        public static double GetPnL(double quantity, double currentPrice, double previousPrice, double multiplier, int sideMultiplier)
        {
            //Gaurav: these may cause infinity value
            if (currentPrice == double.MinValue || previousPrice == double.MinValue)
                return 0;
            return ((quantity * (currentPrice - previousPrice)) * multiplier) * sideMultiplier;
        }


        /// <summary>
        /// Period pnl = Fx pnl + Trading pnl
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="currentPrice"></param>
        /// <param name="previousPrice"></param>
        /// <param name="multiplier"></param>
        /// <param name="sideMultiplier"></param>
        /// <param name="currentFXRateToBase"></param>
        /// <param name="yesterdayFXRateToBase"></param>
        /// <returns></returns>

        public static double GetPnLInBaseCurrency(double quantity, double currentPrice, double previousPrice, double multiplier, int sideMultiplier, double currentFXRateToBase, double yesterdayFXRateToBase)
        {
            //Gaurav: these may cause infinity value
            if (currentPrice == double.MinValue || currentFXRateToBase == double.MinValue || previousPrice == double.MinValue || yesterdayFXRateToBase == double.MinValue)
                return 0;

            return ((quantity * (currentPrice * currentFXRateToBase - previousPrice * yesterdayFXRateToBase)) * multiplier) * sideMultiplier;

        }

        public static double GetLiquidationCostInPortfolio(string positionSideInPortfolio, double quantity, double bidPrice, double askPrice, double multiplier, int sideMultiplier)
        {
            if (positionSideInPortfolio == Prana.BusinessObjects.AppConstants.PositionType.Long.ToString())
            {
                return (quantity * bidPrice) * multiplier * sideMultiplier;

            }
            else if (positionSideInPortfolio == Prana.BusinessObjects.AppConstants.PositionType.Short.ToString())
            {
                return (quantity * askPrice) * multiplier * sideMultiplier;

            }
            else
            {
                return 0;
            }
        }


        public static double GetLiquidationCostInAccount(string positionSideInAccount, double quantity, double bidPrice, double askPrice, double multiplier, int sideMultiplier)
        {
            if (positionSideInAccount == Prana.BusinessObjects.AppConstants.PositionType.Long.ToString())
            {
                return (quantity * bidPrice) * multiplier * sideMultiplier;

            }
            else if (positionSideInAccount == Prana.BusinessObjects.AppConstants.PositionType.Short.ToString())
            {
                return (quantity * askPrice) * multiplier * sideMultiplier;

            }
            else
            {
                return 0;
            }
        }

        public static double? GetLiquidationCost(string positionSide, double quantity, double? bidPrice, double? askPrice, double multiplier)
        {
            if (positionSide == PositionType.Long.ToString())
            {
                return (quantity * bidPrice) * multiplier;
            }
            if (positionSide == PositionType.Short.ToString())
            {
                return (quantity * askPrice) * multiplier;
            }
            return 0;
        }

        public static double GetMarketValue(double quantity, double price, double multiplier, int sideMultiplier, int assetID, double fxMarkPrice)
        {

            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                return quantity * fxMarkPrice * sideMultiplier * multiplier;
            }
            else
            {
                if (price != 0)
                {
                    return quantity * price * sideMultiplier * multiplier;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static double GetMarketValueBase(double quantity, double price, double multiplier, int sideMultiplier, int assetID, double fxMarkPrice, double conversionRate, int conversionMethod, double fxConversionRate, int fxConversionMethod)
        {
            double marketValueBase = 0.0;
            marketValueBase = GetMarketValue(quantity, price, multiplier, sideMultiplier, assetID, fxMarkPrice);
            double conversionRateFinal = 0.0;
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                if (fxConversionRate > 0)
                {
                    conversionRateFinal = ConversionRateBasedOnConversionMethod(fxConversionMethod, fxConversionRate);
                }
            }
            else
            {
                if (conversionRate > 0)
                {
                    conversionRateFinal = ConversionRateBasedOnConversionMethod(conversionMethod, conversionRate);
                }
            }
            return marketValueBase * conversionRateFinal;
        }

        public static double GetInterestLegBase(double interest, int assetID, double conversionRate, int conversionMethod, double fxConversionRate, int fxConversionMethod)
        {
            double conversionRateFinal = 0.0;
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                if (fxConversionRate > 0)
                {
                    conversionRateFinal = ConversionRateBasedOnConversionMethod(fxConversionMethod, fxConversionRate);
                }
            }
            else
            {
                if (conversionRate > 0)
                {
                    conversionRateFinal = ConversionRateBasedOnConversionMethod(conversionMethod, conversionRate);
                }
            }
            return interest * conversionRateFinal;
        }

        /// <summary>
        /// Gets delta adjusted exposure for a contract
        /// </summary>
        /// <param name="quantity">number of contracts</param>
        /// <param name="underlyingStockPrice">price of 1 underlying contract</param>
        /// <param name="underlyingStockPrice">Ishant 20120112: For equities, the select feed price</param>
        /// <param name="multiplier">contract multiplier (1 for equity)</param>
        /// <param name="sideMultiplier">+1 for side long -1 for side short</param>
        /// <param name="delta">delta value for option (1 otherwise)</param>
        /// <returns>Delta adjusted exposure for a contract </returns>
        public static double GetNetExposure(double quantity, double underlyingStockPrice, double multiplier, int sideMultiplier, double delta, double leveragedFactor)
        {
            return quantity * underlyingStockPrice * multiplier * sideMultiplier * delta * leveragedFactor;
        }

        /// <summary>
        /// Gets the local exposure for future and future options.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="sideMultiplier">The side multiplier.</param>
        /// <param name="fxRate">The fx rate.</param>
        /// <returns></returns>
        public static double GetLocalExposureForFutureAndFutureOptions(double quantity, double multiplier, int sideMultiplier, double fxRate)
        {
            return quantity * multiplier * sideMultiplier * fxRate;
        }

        /// <summary>
        /// Gets the exposure in base currency for future and future options.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <param name="underlyingStockPrice">The underlying stock price.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="sideMultiplier">The side multiplier.</param>
        /// <param name="fxRate">The fx rate.</param>
        /// <returns></returns>
        public static double GetExposureInBaseCurrencyForFutureAndFutureOptions(double quantity, double underlyingStockPrice, double multiplier, int sideMultiplier, double fxRate)
        {
            return quantity * underlyingStockPrice * multiplier * sideMultiplier * fxRate;
        }

        /// <summary>
        /// Gets delta adjusted position for a contract
        /// </summary>
        /// <param name="quantity">number of contracts</param>
        /// <param name="multiplier">contract multiplier (1 for equity)</param>
        /// <param name="sideMultiplier">+1 for side long -1 for side short</param>
        /// <param name="delta">delta value for option (1 otherwise)</param>
        /// <returns>Delta adjusted position for a contract </returns>
        public static double GetDeltaAdjustedPosition(double quantity, double multiplier, int sideMultiplier, double delta)
        {
            return quantity * multiplier * sideMultiplier * delta;
        }

        public static double GetBetaAdjExposure(double netExposure, double beta, double leverageFactor)
        {
            if (leverageFactor != 0)
            {
                return ((netExposure / leverageFactor) * beta);
            }
            else
            {
                return netExposure * beta;
            }
        }

        public static double GetBetaAdjExposure(double netExposure, double beta)
        {
            return netExposure * beta;
        }

        public static double GetMarketValue(double quantity, double underlyingStockPrice, double multiplier, int sideMultiplier)
        {
            return quantity * underlyingStockPrice * multiplier * sideMultiplier;
        }


        public static double GetBasisPointExposure(double exposure, double nav)
        {
            if (nav == 0)
            {
                return 0;
            }
            else
            {
                return (((exposure * 1.0) / nav) * Prana.Global.ApplicationConstants.BASISPOINTVALUE);
            }
        }


        /// <summary>
        /// Returns basis point contribution of an individual value to a total value in basis points
        /// </summary>
        /// <param name="individualValue">contributing individual value </param>
        /// <param name="totalValue">total value</param>
        /// <returns></returns>
        public static double GetBasisPointContribution(double individualValue, double totalValue)
        {
            return (individualValue / totalValue) * Prana.Global.ApplicationConstants.BASISPOINTVALUE;
        }

        /// <summary>
        /// Returns percentage contribution of an individual value to a total value in basis points
        /// </summary>
        /// <param name="individualValue">contributing individual value </param>
        /// <param name="totalValue">total value</param>
        /// <returns></returns>
        public static double GetPercentageContribution(double individualValue, double totalValue)
        {
            return (individualValue / totalValue) * Prana.Global.ApplicationConstants.PERCENTAGEVALUE;
        }

        /// <summary>
        /// returns interest rate contribution based on some notional for number of days
        /// </summary>
        /// <param name="notional">total notinal</param>
        /// <param name="interestRate">yearly interest rate percentage</param>
        /// <param name="DayCount">day count convention (default 365)</param>
        /// <param name="numberOfDays">number of days for which interest rate componet is to be calculated</param>
        /// <returns></returns>
        public static double GetInterestValue(double notional, double interestRate, double DayCount, double numberOfDays)
        {
            if (DayCount.Equals(0))
            {
                DayCount = 365;
            }
            return (notional * interestRate * numberOfDays / 100) / DayCount;
        }

        public static double GetTotalCost(double price, double qty, double multiplier, int sideMultiplier, double totalCommissionAndFees, int assetID)
        {
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                return qty * price * multiplier * sideMultiplier + totalCommissionAndFees;
            }
            else
            {
                return price * qty * multiplier * sideMultiplier + totalCommissionAndFees;
            }
        }

        public static double GetTotalCost(double price, double qty, double commision, double multiplier, int sideMultiplier, double fees, int assetID)
        {
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                return qty * price * multiplier * sideMultiplier + commision + fees;
            }
            else
            {
                return price * qty * multiplier * sideMultiplier + commision + fees;
            }
        }

        public static double GetTotalCost(double price, double qty, double totalCommissionAndFees, double multiplier, int sideMultiplier, double conversionRate, int conversionMethod, double fxConversionRate, int fxConversionMethod, int assetID)
        {
            double totalCostBase = 0.0;
            totalCostBase = GetTotalCost(price, qty, multiplier, sideMultiplier, totalCommissionAndFees, assetID);
            double conversionRateFinal = 1.0;
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(fxConversionMethod, fxConversionRate);
            }
            else
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(conversionMethod, conversionRate);
            }
            return totalCostBase * conversionRateFinal;
        }

        public static double GetTotalCost(double price, double qty, double commision, double multiplier, int sideMultiplier, double fees, double conversionRate, int conversionMethod, double fxConversionRate, int fxConversionMethod, int assetID)
        {
            double totalCostBase = 0.0;
            totalCostBase = GetTotalCost(price, qty, commision, multiplier, sideMultiplier, fees, assetID);
            double conversionRateFinal = 1.0;
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(fxConversionMethod, fxConversionRate);
            }
            else
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(conversionMethod, conversionRate);
            }
            return totalCostBase * conversionRateFinal;
        }

        public static double GetDayDifference(DateTime finalDate, DateTime initialDate)
        {
            if (initialDate == null || finalDate == null)
            {
                return 0;
            }

            TimeSpan tDiff = finalDate.Date - initialDate.Date;
            int dayDiff = (int)Math.Ceiling(tDiff.TotalDays);
            return dayDiff;
        }

        public static double GetConversionRate(int assetID, double conversionRate, int conversionMethod, double fxConversionRate, int fxConversionMethod)
        {
            double conversionRateFinal = 1.0;
            if ((AssetCategory)assetID == AssetCategory.FX)
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(fxConversionMethod, fxConversionRate);
            }
            else
            {
                conversionRateFinal = ConversionRateBasedOnConversionMethod(conversionMethod, conversionRate);
            }
            return conversionRateFinal;
        }

        private static double ConversionRateBasedOnConversionMethod(int conversionMethod, double conversionRate)
        {
            double conversionRateCalculated = 0.0;

            if (((Operator)conversionMethod).Equals(Operator.M))
            {
                conversionRateCalculated = conversionRate;
            }
            else if (((Operator)conversionMethod).Equals(Operator.D))
            {
                if (conversionRate != 0)
                {
                    conversionRateCalculated = 1 / conversionRate;
                }
            }
            return conversionRateCalculated;
        }

        public static string GetCurrencyLanguageName(string currencySymbol)
        {
            string languageName = string.Empty;
            if (currencySymbol != null)
            {
                CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                foreach (CultureInfo culture in allCultures)
                {
                    //Build the regionInfo object from the cultureInfo object
                    int lcid = culture.LCID;
                    RegionInfo ri = new RegionInfo(lcid);
                    string currencyCode = ri.ISOCurrencySymbol;
                    if (currencySymbol.Equals(currencyCode))
                    {
                        //languageName = culture.EnglishName;
                        languageName = culture.Name;
                        return languageName;
                    }
                }
            }
            return languageName;
        }

        public static double GetGainORLoss(double firstValue, double secondValue)
        {
            double gainORLoss = 0.0;
            gainORLoss = firstValue - secondValue;
            return gainORLoss;
        }

        public static double GetGainORLoss(double firstValue, double secondValue, double interestLeg)
        {
            //double gainORLoss = 0.0;

            return GetGainORLoss(firstValue, secondValue) - interestLeg;

            //if (reportMode.Equals(0))
            //{
            //    if (!secondValue.Equals(0)) //As Avg price can be 0. And the first value is in this case is Total Cost & second value is market value.
            //    {
            //        if ((AssetCategory)assetID == AssetCategory.FX)
            //        {
            //            if (!fxMarkPrice.Equals(0))
            //            {
            //                gainORLoss = GetGainORLoss(firstValue, secondValue) - interestLeg;
            //            }
            //        }
            //        else
            //        {
            //            if (!markPrice.Equals(0))
            //            {
            //                gainORLoss = GetGainORLoss(firstValue, secondValue) - interestLeg;
            //            }
            //        }
            //    }
            //}
            //else if (reportMode.Equals(1))
            //{
            //    if (!firstValue.Equals(0) && !secondValue.Equals(0))
            //    {
            //        if ((AssetCategory)assetID == AssetCategory.FX)
            //        {
            //            if (!fxMarkPrice.Equals(0))
            //            {
            //                gainORLoss = GetGainORLoss(firstValue, secondValue) - interestLeg;
            //            }
            //        }
            //        else
            //        {
            //            if (!markPrice.Equals(0))
            //            {
            //                gainORLoss = GetGainORLoss(firstValue, secondValue) - interestLeg;
            //            }
            //        }
            //    }
            //}
            //return gainORLoss;
        }
        public static double GetClosingPrice(int closingMode, double settlementPrice, double closingPrice)
        {
            Prana.BusinessObjects.AppConstants.ClosingMode closingModeEnum = (ClosingMode)closingMode;

            switch (closingModeEnum)
            {
                case ClosingMode.Exercise:
                case ClosingMode.SwapExpire:
                case ClosingMode.SwapExpireAndRollover:
                case ClosingMode.Cash:
                    closingPrice = settlementPrice;
                    break;
            }
            return closingPrice;
        }

        /// <summary>
        /// Gets Notional value for a contract
        /// </summary>
        /// <param name="quantity">number of contracts</param>
        /// <param name="avgPrice">price of 1 contract (use 1 for FX as is cash)</param>
        /// <param name="multiplier">contract multiplier (use 1 for equity)</param>
        /// <param name="sideMultiplier"> +1 for side long -1 for side short</param>
        /// <returns>Notional value for a contract</returns>
        public static double GetNotional(double quantity, double avgPrice, double multiplier, int sideMultiplier)
        {
            return (quantity * avgPrice) * sideMultiplier * multiplier;
        }
    }
}
