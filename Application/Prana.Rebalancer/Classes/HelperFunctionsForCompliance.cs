using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.Classes
{
    internal class HelperFunctionsForCompliance
    {
        /// <summary>
        /// Creates the complete order for stage and compliance.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="errorMessage"></param>
        internal static void CreateCompleteOrderForStageAndCompliance(OrderSingle order, ref StringBuilder errorMessage)
        {
            try
            {
                TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                // int venueID = 0;
                // int counterPartyID = 0;
                int tradingAccount = 0;
                string tif = 0.ToString();
                int strategyID = int.MinValue;
                int assetID = Convert.ToInt32(CachedDataManager.GetInstance.GetAssetIdByAUECId(order.AUECID));
                int underlyingID = Convert.ToInt32(CachedDataManager.GetInstance.GetUnderlyingID(order.AUECID));
                int exchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID);
                ValueList userBrokerList = TTHelperManager.GetInstance().GetCounterparties(assetID, underlyingID, order.AUECID);
                var isInUserBrokerList = companyTradingTicketUiPrefs.Broker.HasValue ? userBrokerList.ValueListItems.Cast<ValueListItem>().Any(valueitem => valueitem.DataValue.Equals(companyTradingTicketUiPrefs.Broker.Value)) : false;
                if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null)
                {
                    if (IsAnyNullOrEmpty(userTradingTicketUiPrefs, companyTradingTicketUiPrefs))
                    {
                        errorMessage.Append(RebalancerConstants.MSG_PREF_NOT_DEFINED);
                        return;
                    }
                    //if CounterPartyID in order not set then set from from TT preferences
                    if (order.CounterPartyID <= 0 && !order.IsUseCustodianBroker)
                        order.CounterPartyID = userTradingTicketUiPrefs.Broker.HasValue ? userTradingTicketUiPrefs.Broker.Value : ((isInUserBrokerList) ? companyTradingTicketUiPrefs.Broker.Value : int.MinValue);

                    //if VenueID in order not set then set from from TT preferences
                    if (order.VenueID <= 0 && order.CounterPartyID > 0)
                    {
                        if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(order.CounterPartyID))
                        {
                            order.VenueID = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[order.CounterPartyID];
                        }
                        else if (companyTradingTicketUiPrefs.Venue.HasValue && isInUserBrokerList)
                        {
                            order.VenueID = companyTradingTicketUiPrefs.Venue.Value;
                        }
                        else
                        {
                            order.VenueID = 0;
                        }

                    }

                    tif = userTradingTicketUiPrefs.TimeInForce.HasValue ? TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(userTradingTicketUiPrefs.TimeInForce.Value.ToString()) : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(companyTradingTicketUiPrefs.TimeInForce.Value.ToString());
                    tradingAccount = userTradingTicketUiPrefs.TradingAccount.HasValue ? userTradingTicketUiPrefs.TradingAccount.Value : companyTradingTicketUiPrefs.TradingAccount.Value;
                    if (userTradingTicketUiPrefs.Strategy != null)
                        strategyID = userTradingTicketUiPrefs.Strategy.HasValue ? int.Parse(userTradingTicketUiPrefs.Strategy.ToString()) : int.Parse(companyTradingTicketUiPrefs.Strategy.ToString());
                }

                if (tradingAccount <= 0 || Convert.ToInt32(tif) < 0)
                    errorMessage.Append(RebalancerConstants.MSG_PREF_NOT_DEFINED);

                if (order.Quantity != 0 && errorMessage.Length == 0)
                {
                    int companyUserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    order.MsgType = FIXConstants.MSGOrder;
                    order.CompanyName = string.Empty;
                    order.Description = string.Empty;
                    order.AUECLocalDate = DateTime.Now;
                    order.ProcessDate = DateTime.Now;
                    order.NirvanaProcessDate = DateTime.Now;
                    order.OriginalPurchaseDate = DateTime.Now;
                    order.Quantity = Math.Abs(Convert.ToDouble(order.Quantity));
                    order.AvgPrice = Convert.ToDouble(order.Price);
                    order.Price = 0.0;
                    order.Text = string.Empty;
                    order.InternalComments = string.Empty;
                    order.Symbol = order.Symbol;
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                    order.TransactionType = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                    //order.VenueID = venueID;
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                    // order.CounterPartyID = counterPartyID;
                    if (string.IsNullOrWhiteSpace(order.HandlingInstruction))
                        order.HandlingInstruction = "3";
                    order.OrderTypeTagValue = FIXConstants.ORDTYPE_Market;
                    order.TIF = tif;
                    order.TradingAccountID = tradingAccount;
                    if (string.IsNullOrWhiteSpace(order.ExecutionInstruction))
                        order.ExecutionInstruction = "G";
                    order.AssetID = assetID;
                    order.UnderlyingID = underlyingID;
                    order.CompanyUserID = companyUserID;
                    order.ActualCompanyUserID = companyUserID;
                    order.TransactionTime = DateTime.Now.ToUniversalTime();
                    order.AlgoStrategyID = string.Empty;
                    order.AlgoProperties = new OrderAlgoStartegyParameters();
                    order.ExchangeID = exchangeID;
                    order.TradeAttribute1 = string.Empty;
                    order.TradeAttribute2 = string.Empty;
                    order.TradeAttribute3 = string.Empty;
                    order.TradeAttribute4 = string.Empty;
                    order.TradeAttribute5 = string.Empty;
                    order.TradeAttribute6 = string.Empty;
                    order.InternalComments = string.Empty;
                    order.Level2ID = strategyID;

                    order.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    order.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(order.CurrencyID);
                    //switch (order.CurrencyName)
                    //{
                    //    case "EUR":
                    //    case "GBP":
                    //    case "NZD":
                    //    case "AUD":
                    //        order.FXConversionMethodOperator = Operator.M.ToString();
                    //        break;

                    //    default:
                    //        order.FXConversionMethodOperator = Operator.D.ToString();
                    //        break;
                    //}
                    order.FXConversionMethodOperator = Operator.M.ToString();
                    if (order.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    {
                        order.FXRate = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static bool IsAnyNullOrEmpty(TradingTicketUIPrefs userTradingTicketUiPrefsObj, TradingTicketUIPrefs companyTradingTicketUiPrefsObj)
        {
            bool returnValue = false;
            try
            {
                if (!userTradingTicketUiPrefsObj.TradingAccount.HasValue && !companyTradingTicketUiPrefsObj.TradingAccount.HasValue)
                {
                    returnValue = true;
                }

                else if (!userTradingTicketUiPrefsObj.TimeInForce.HasValue && !companyTradingTicketUiPrefsObj.TimeInForce.HasValue)
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return returnValue;
        }

        /// <summary>
        /// CheckComplianceConnector FeedbackMessageReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        internal static string CheckComplianceConnector_FeedbackMessageReceived(EventArgs<DataSet> e, int companyUserID)
        {
            try
            {
                if (e.Value != null && e.Value.Tables.Count > 0 && e.Value.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(e.Value.Tables[0].Rows[0][ComplainceConstants.CONST_USER_ID]) == companyUserID && !String.IsNullOrEmpty(e.Value.Tables[0].Rows[0][ComplainceConstants.CONST_FEEDBACK_MESSAGE].ToString()))
                        return e.Value.Tables[0].Rows[0][ComplainceConstants.CONST_FEEDBACK_MESSAGE].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }
    }
}
