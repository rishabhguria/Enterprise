using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using Prana.PostTrade;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects;
namespace Prana.AllocationNew
{
   public class AllocationRules
    {
        
       public static bool IsGroupingRulePassed(List<AllocationGroup> groups , AllocationPreferences _allocationPreferences)
       {
           bool result = true ;
           AllocationGroup group1 = groups[0];
           foreach (AllocationGroup group in groups)
           {
               if (!AreGroupsGroupable(group1, group,_allocationPreferences))
               {
                   result= false;
                   break;
               }
           }

           return result;

       }
       public static bool AreGroupsGroupable(AllocationGroup group1,AllocationGroup group2, AllocationPreferences _allocationPreferences)
       {
           bool result = false;
           try
           {
           if (group1.IsSwapped || group2.IsSwapped)
           {
               return false;
           }
           //add Currency same rule
           bool bCounterParty = _allocationPreferences.AutoGroupingRules.CounterParty;
           bool bVenue = _allocationPreferences.AutoGroupingRules.Venue;
           bool bTradingAccount = _allocationPreferences.AutoGroupingRules.TradingAccount;

           bool bBuyAndBCV = _allocationPreferences.AutoGroupingRules.BuyAndBCV;
               bool bProcessDate = _allocationPreferences.AutoGroupingRules.ProcessDate;
               bool bTradeDate = _allocationPreferences.AutoGroupingRules.TradeDate;


				//CHMW-3149	[Foreign Positions Settling in Base Currency] Handle grouping/ungrouping for settlement fields
           //TBC Buy to Cover Tag Value 
           if ((group1.OrderSide.Equals(group2.OrderSide) || ((bBuyAndBCV) && ((group2.OrderSide.Equals("Buy") && group1.OrderSide.Equals("BCV")) || (group1.OrderSide.Equals("Buy") && group2.OrderSide.Equals("BCV")))))
               && (group1.Symbol.Equals(group2.Symbol))
               && (group1.SettlementCurrencyID.Equals(group2.SettlementCurrencyID))
               && ((!bCounterParty) || (bCounterParty && group1.CounterPartyName.Equals(group2.CounterPartyName)))
               && ((!bVenue) || (bVenue && group1.Venue.Equals(group2.Venue)))
               && ((!bTradingAccount) || (bTradingAccount && group1.TradingAccountName.Equals(group2.TradingAccountName)))
                       && (AreTradeAttributesGroupable(group1, group2, _allocationPreferences)
                       ))
                   {
                       if (((bTradeDate && bProcessDate) && ((group1.AUECLocalDate.Date.Equals(group2.AUECLocalDate.Date)) && (group1.ProcessDate.Date.Equals(group2.ProcessDate.Date)))) 
                           || (bTradeDate && !bProcessDate && group1.AUECLocalDate.Date.Equals(group2.AUECLocalDate.Date))
                           || (bProcessDate && !bTradeDate && group1.ProcessDate.Date.Equals(group2.ProcessDate.Date)))
                           result = true;
                       else
                           result = false;
                   }
                   else
                   {
                           result = false;
                   }
                              
           }
           catch (Exception ex)
           {
               // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
           }
           return result;
       }

       /// <summary>
       /// Checks if attributes for group are equal or not for auto grouping.
       /// </summary>
       /// <param name="group1"></param>
       /// <param name="group2"></param>
       /// <returns></returns>
       private static bool AreTradeAttributesGroupable(AllocationGroup group1, AllocationGroup group2, AllocationPreferences allocationPreferences)
       {
           try
           {
               bool bTradeAttributes1 = allocationPreferences.AutoGroupingRules.TradeAttributes1;
               bool bTradeAttributes2 = allocationPreferences.AutoGroupingRules.TradeAttributes2;
               bool bTradeAttributes3 = allocationPreferences.AutoGroupingRules.TradeAttributes3;
               bool bTradeAttributes4 = allocationPreferences.AutoGroupingRules.TradeAttributes4;
               bool bTradeAttributes5 = allocationPreferences.AutoGroupingRules.TradeAttributes5;
               bool bTradeAttributes6 = allocationPreferences.AutoGroupingRules.TradeAttributes6;

               if (
                      ((!bTradeAttributes1) || (bTradeAttributes1 && IsTradeAttributeEqual(group1.TradeAttribute1, group2.TradeAttribute1)))
                   && ((!bTradeAttributes2) || (bTradeAttributes2 && IsTradeAttributeEqual(group1.TradeAttribute2, group2.TradeAttribute2)))
                   && ((!bTradeAttributes3) || (bTradeAttributes3 && IsTradeAttributeEqual(group1.TradeAttribute3, group2.TradeAttribute3)))
                   && ((!bTradeAttributes4) || (bTradeAttributes4 && IsTradeAttributeEqual(group1.TradeAttribute4, group2.TradeAttribute4)))
                   && ((!bTradeAttributes5) || (bTradeAttributes5 && IsTradeAttributeEqual(group1.TradeAttribute5, group2.TradeAttribute5)))
                   && ((!bTradeAttributes6) || (bTradeAttributes6 && IsTradeAttributeEqual(group1.TradeAttribute6, group2.TradeAttribute6)))
                  )
                   return true;
               else
                   return false;
           }
           catch (Exception ex)
           {

               bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
               if (rethrow)
               {
                   throw;
               }
               return false;
           }

       }
       
       /// <summary>
       /// Checks if two strings are equal or not.
       /// </summary>
       /// <param name="tradeAttribute1">string1</param>
       /// <param name="tradeAttribute2">string2</param>
       /// <returns></returns>
       private static bool IsTradeAttributeEqual(string tradeAttribute1, string tradeAttribute2)
       {
           try
           {
               if (string.IsNullOrEmpty(tradeAttribute1))
                   tradeAttribute1 = string.Empty;

               if (string.IsNullOrEmpty(tradeAttribute2))
                   tradeAttribute2 = string.Empty;

               return tradeAttribute1.Trim().Equals(tradeAttribute2.Trim());
    }
           catch (Exception ex)
           {

               bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
               if (rethrow)
               {
                   throw;
}
               return false;
           }
       }
       
    }
}
