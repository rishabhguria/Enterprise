using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Common.Helper
{
    public static class CommonHelper
    {
        /// <summary>
        /// If Group is Swap then return Symbol-Swap
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isSwapped"></param>
        /// <returns></returns>
        public static string GetSwapSymbol(string symbol, bool isSwapped)
        {
            return isSwapped ? symbol + "-Swap" : symbol;
        }
        /// <summary>
        /// Gets the position tag by side.
        /// </summary>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <returns></returns>
        public static int GetPositionTagBySide(string orderSideTagValue)
        {
            try
            {
                switch (orderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_BuyMinus:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                    default:
                        return (int)PositionTag.Long;

                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_Sell_Closed:
                        return (int)PositionTag.Short;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return (int)PositionTag.Long;
        }

        /// <summary>
        /// Creates the un allocated tax lot.
        /// </summary>
        /// <param name="baseMsg">The base MSG.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        public static TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID)
        {
            TaxLot taxLot = null;
            try
            {
                taxLot = new TaxLot();
                taxLot.TaxLotQty = baseMsg.CumQty;
                taxLot.TaxLotID = groupID;
                taxLot.GroupID = groupID;
                taxLot.SideMultiplier = Calculations.GetSideMultilpier(baseMsg.OrderSideTagValue);
                taxLot.PositionTag = (PositionTag)(GetPositionTagBySide(baseMsg.OrderSideTagValue));
                taxLot.CopyBasicDetails(baseMsg);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return taxLot;
        }
    }
}
