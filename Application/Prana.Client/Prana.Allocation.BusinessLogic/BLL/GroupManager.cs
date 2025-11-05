using System;
using Prana.Utilities.DateTimeUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for GroupManager.
	/// </summary>
	public class GroupManager

	{
		public GroupManager()
		{
			
		}
		
        public static bool isGroupingRulePassed(AllocationOrderCollection groupedOrders, AllocationPreferences _allocationPreferences)
        {

            bool bCounterParty = _allocationPreferences.AutoGroupingRules.CounterParty;
            bool bVenue = _allocationPreferences.AutoGroupingRules.Venue;
            bool bTradingAccount = _allocationPreferences.AutoGroupingRules.TradingAccount;

            bool bBuyAndBCV = _allocationPreferences.AutoGroupingRules.BuyAndBCV;

            bool result = false;
            bool firstTime = false;

            string side = string.Empty;
            string symbol = string.Empty;
            string counterParty = string.Empty;
            string venue = string.Empty;
            string tradingAccount = string.Empty;
            DateTime dt = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
            

            foreach (AllocationOrder order in groupedOrders)
            {
                if (!firstTime)
                {
                    side = order.OrderSide;
                    symbol = order.Symbol;
                    counterParty = order.CounterPartyName;
                    venue = order.Venue;
                    tradingAccount = order.TradingAccountName;
                     dt = order.AUECLocalDate;
                    firstTime = true;
                }

                //TBC Buy to Cover Tag Value 
                if ((order.OrderSide.Equals(side) || ((bBuyAndBCV) && ((side.Equals("Buy") && order.OrderSide.Equals("BCV")) || (side.Equals("Buy") && order.OrderSide.Equals("BCV")))))
                    && (order.Symbol.Equals(symbol))
                    && ((!bCounterParty) || (bCounterParty && order.CounterPartyName.Equals(counterParty)))

                    && ((!bVenue) || (bVenue && order.Venue.Equals(venue)))
                    && (order.AUECLocalDate.Date.Equals(dt.Date))
                    && ((!bTradingAccount) || (bTradingAccount && order.TradingAccountName.Equals(tradingAccount)))
                    && ((!bBuyAndBCV) || (bBuyAndBCV && order.OrderSide.Equals(side)))

                    )
                    result = true;
                else
                {
                    result = false;
                    break;
                }
            }

            return result;


        }
       
	}
}
