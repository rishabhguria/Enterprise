using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
namespace Prana.CommonDataCache
{
    public class NameValueFiller
    {
        public static void FillNameDetailsOfOrder(Order order)
        {
            try
            {
                if (order.CounterPartyID != int.MinValue)
                {
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                }
                if (order.VenueID != int.MinValue)
                {
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                }
                if (order.ExchangeID != int.MinValue)
                {
                    order.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(order.ExchangeID);
                }
                if (order.OrderSideTagValue.Trim() != string.Empty)
                {
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                }
                if (order.OrderTypeTagValue.Trim() != string.Empty)
                {
                    order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(order.OrderTypeTagValue);
                }
                if (order.OrderStatusTagValue.Trim() != string.Empty)
                {
                    order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(order.OrderStatusTagValue);
                }
                if (order.AssetID != int.MinValue)
                {
                    order.AssetName = CachedDataManager.GetInstance.GetAssetText(order.AssetID);
                }
                if (order.UnderlyingID != int.MinValue)
                {
                    order.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(order.UnderlyingID);

                }
                if (order.TradingAccountID != int.MinValue)
                {
                    order.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);

                }
                if (order.TIF != string.Empty && order.TIF != ApplicationConstants.C_COMBO_SELECT)
                {
                    order.TIFText = TagDatabaseManager.GetInstance.GetTIFText(order.TIF);

                }
                if (order.HandlingInstruction != string.Empty)
                {
                    order.HandlingInstructionText = TagDatabaseManager.GetInstance.GetHandlingInstructionText(order.HandlingInstruction);

                }
                if (order.ExecutionInstruction != string.Empty && order.ExecutionInstruction != ApplicationConstants.C_COMBO_SELECT)
                {
                    order.ExecutionInstructionText = TagDatabaseManager.GetInstance.GetExecutionInstructionText(order.ExecutionInstruction);

                }

            }
            catch (Exception)
            {
                throw new Exception(" Format Error");
            }

        }
        public static void FillIDDetailsOfOrder(Order order)
        {
            try
            {
                order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(order.CounterPartyName);
                order.VenueID = CachedDataManager.GetInstance.GetVenueID(order.Venue);
                order.ExchangeID = CachedDataManager.GetInstance.GetExchangeID(order.ExchangeName);
                order.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(order.OrderSide);
                order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValue(order.OrderType);

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

        }
        public static void FillNameDetailsOfMessage(PranaBasicMessage order)
        {
            try
            {
                //order.AssetID=  CachedDataManager.GetInstance.GetAssetIdByAUECId(order.AUECID);
                //order.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID);
                //order.CurrencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(order.AUECID);

                if (order.CounterPartyID != int.MinValue)
                {
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                }
                if (order.VenueID != int.MinValue)
                {
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                }
                if (order.ExchangeID != int.MinValue)
                {
                    order.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(order.ExchangeID);
                }
                if (order.CurrencyID != int.MinValue)
                {
                    order.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(order.CurrencyID);
                }
                if (order.OrderSideTagValue.Trim() != string.Empty)
                {
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                }
                if (order.OrderTypeTagValue.Trim() != string.Empty)
                {
                    order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(order.OrderTypeTagValue);
                }

                if (order.AssetID != int.MinValue)
                {
                    order.AssetName = CachedDataManager.GetInstance.GetAssetText(order.AssetID);
                }
                if (order.UnderlyingID != int.MinValue)
                {
                    order.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(order.UnderlyingID);
                }
                if (order.TradingAccountID != int.MinValue)
                {
                    order.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);
                }
                if (order.CompanyUserID != int.MinValue)
                {
                    order.CompanyUserName = CachedDataManager.GetInstance.GetUserText(order.CompanyUserID);

                }
                if (order.CompanyName == string.Empty)
                {
                    order.CompanyName = order.Description;
                }
            }
            catch (Exception)
            {
                throw new Exception(" Format Error");
            }

        }
        public static bool IsLongSide(string orderSideTagValue)
        {
            if (orderSideTagValue == FIXConstants.SIDE_Buy ||
                orderSideTagValue == FIXConstants.SIDE_Buy_Closed ||
                orderSideTagValue == FIXConstants.SIDE_Buy_Open ||
                orderSideTagValue == FIXConstants.SIDE_BuyMinus ||
                orderSideTagValue == FIXConstants.SIDE_Cross ||
                orderSideTagValue == FIXConstants.SIDE_CrossShort)
            //orderSideTagValue == FIXConstants.SIDE_CrossShortExempt ||
            //orderSideTagValue == FIXConstants.SIDE_Opposite)
            {
                return true;
            }
            else
                return false;
        }
        public static void SetNameValues<T>(T data)
        {
            try
            {
                if (data != null)
                {

                    IList<CashActivity> lsCashActivity = data as IList<CashActivity>;
                    if (lsCashActivity != null)
                    {

                        foreach (CashActivity cashActivity in lsCashActivity)
                        {
                            cashActivity.ActivityType = Prana.CommonDataCache.CachedDataManager.GetActivityText(cashActivity.ActivityTypeId);
                            cashActivity.AccountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(cashActivity.AccountID);
                            cashActivity.CurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(cashActivity.CurrencyID);
                            cashActivity.LeadCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(cashActivity.LeadCurrencyID);
                            cashActivity.VsCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(cashActivity.VsCurrencyID);

                            cashActivity.SettlCurrency = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(cashActivity.SettlCurrencyID);

                            //PRANA-9776
                            cashActivity.UserName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserText(cashActivity.UserId);
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
        }
    }
}
