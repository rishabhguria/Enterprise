using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects;
using AdapterClient.FIX;
using Prana.Fix.FixDictionary;
using System.Data;
using System.Reflection;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
namespace Prana.ServerClientCommon
{
    public class Transformer
    {
        static PropertyInfo[] propertiesOrder=null;
        static PropertyInfo[] propertiesOrderSingle = null;
        static PropertyInfo[] propertiesDropCpyOrder = null;
        static Transformer()
        {
            Order order = new Order();
            OrderSingle orderSingle = new OrderSingle();
            DropCopyOrder dropCopyOrder = new DropCopyOrder();
            propertiesOrder = order.GetType().GetProperties();
            propertiesOrderSingle = orderSingle.GetType().GetProperties();
            propertiesDropCpyOrder = dropCopyOrder.GetType().GetProperties();
        }
        //Not Used
        #region OrderRelated Methods
        //private PranaMessage ConvertOrderToFIXMessage(Order order)
        //{
        //    // for creating OPen Close fix tags


        //    PranaMessage msg = new PranaMessage(order.MsgType,order.TradingAccountID);
        //    try
        //    {
        //        CreateOpenCloseTags(order);
        //        AddBasicTags(order,msg);
        //        AddTagsBasedOnFIXMsgType(order, msg);
        //        AddTagsBasedOnSecurityType(order, msg);
        //        AddTagsBasedOnOrderType(order, msg);

               

        //        return msg;
        //    }

        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        throw ex;
        //    }
        //    #endregion
        //}

        /// <summary>
        /// Create open close tags based on side tag value
        /// </summary>
        /// <param name="order"></param>
        //private static void CreateOpenCloseTags(Order order)
        //{

        //    if (order.OrderSideTagValue == FIXConstants.SIDE_Buy_Open)
        //    {
        //        order.OrderSideTagValue = FIXConstants.SIDE_Buy;
        //        order.OpenClose = FIXConstants.Open;
        //    }
        //    else if (order.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed)
        //    {
        //        order.OrderSideTagValue = FIXConstants.SIDE_Buy;
        //        order.OpenClose = FIXConstants.Close;
        //    }
        //    else if (order.OrderSideTagValue == FIXConstants.SIDE_Sell_Open)
        //    {
        //        order.OrderSideTagValue = FIXConstants.SIDE_Sell;
        //        order.OpenClose = FIXConstants.Open;
        //    }
        //    else if (order.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed)
        //    {
        //        order.OrderSideTagValue = FIXConstants.SIDE_Sell;
        //        order.OpenClose = FIXConstants.Close;
        //    }
        //}

        //private static void AddBasicTags(Order order, PranaMessage msg)
        //{
        //    #region session Variables
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagTargetCompID, order.CounterPartyID.ToString());
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagExDestination, order.VenueID.ToString());
        //    if (order.TargetSubID != string.Empty)
        //    {
        //        msg.FIXMessage.SetExternalField(FIXConstants.TagTargetSubID, order.TargetSubID);
        //    }
        //    #endregion

        //    msg.FIXMessage.SetExternalField(FIXConstants.TagMsgType, order.MsgType);
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagOrderQty, order.Quantity.ToString());
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagSide, order.OrderSideTagValue);
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagSymbol, order.Symbol);
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagClOrdID, order.ClOrderID);
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagPossResend, order.PossDupFlag);

        //    #region timings variables
        //    msg.FIXMessage.SetExternalField(FIXConstants.TagTransactTime, order.TransactionTime);
        //    #endregion
        //}

        //private static void AddTagsBasedOnFIXMsgType(Order order, PranaMessage msg)
        //{
        //    switch (order.MsgType)
        //    {
        //        case FIXConstants.MSGOrder:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrdType, order.OrderTypeTagValue);


        //            break;
        //        case FIXConstants.MSGOrderCancelRequest:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrderID, order.OrderID);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrigClOrdID, order.OrigClOrderID);
        //            break;
        //        case FIXConstants.MSGOrderCancelReplaceRequest:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrderID, order.OrderID);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrdType, order.OrderTypeTagValue);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOrigClOrdID, order.OrigClOrderID);




        //            break;
        //    }

        //    if (order.MsgType == FIXConstants.MSGOrder
        //           || order.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
        //    {
        //        if (order.SecurityType != FIXConstants.SECURITYTYPE_Options &&
        //            order.SecurityType != FIXConstants.SECURITYTYPE_Futures)
        //        {
        //            #region Set Discretion OFfset, Max Floor and Locate Reqd Tags
        //            ///Set Discretion Offset.
        //            if (order.DiscretionInst.ToString() == "0"
        //                && order.DiscretionOffset > 0
        //                && order.DiscretionOffset != double.MinValue
        //                && order.DiscretionOffset != double.Epsilon)
        //            {
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagDiscretionInst, order.DiscretionInst);
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagDiscretionOffset, order.DiscretionOffset.ToString());

        //            }

        //            ///Set Max Floor
        //            if (order.DisplayQuantity > 0
        //                && order.DisplayQuantity != double.MinValue
        //                && order.DisplayQuantity != double.Epsilon)
        //            {
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagMaxFloor, order.DisplayQuantity.ToString());
        //            }

        //            ///If Side == Sell short set borrower ID and Loacte required tags. 
        //            if (order.OrderSideTagValue == FIXConstants.SIDE_SellShort
        //                || order.OrderSideTagValue == FIXConstants.SIDE_SellShortExempt)
        //            {
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagLocateReqd, (order.LocateReqd == true ? "Y" : "N"));
        //                /// Send the borrowerID field in CustomTag 5700 when LocateReqd is false. 
        //                if (!order.LocateReqd)
        //                {
        //                    ///Check if BorrowerID is present or not. if present send it 
        //                    if (order.BorrowerID != string.Empty)
        //                    {
        //                        msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_BorrowerID, order.BorrowerID);
        //                    }
        //                    ///Else remove the Locate Reqd field as well. 
        //                    else
        //                    {
        //                        msg.FIXMessage.RemoveExternalField(FIXConstants.TagLocateReqd);
        //                    }
        //                }
        //            }
        //            #endregion Set Discretion OFfset, Max Floor and Locate Reqd Tags
        //        }

        //        //Set execution instruction
        //        if (order.ExecutionInstruction != string.Empty)
        //        {
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagExecInst, order.ExecutionInstruction);

        //        }


        //    }

        //}

        //private static void AddTagsBasedOnSecurityType(Order order, PranaMessage msg)
        //{
        //    switch (order.SecurityType)
        //    {
        //        case FIXConstants.SECURITYTYPE_Options:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagSecurityType, order.SecurityType);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOpenClose, order.OpenClose);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagStrikePrice, order.StrikePrice.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagMaturityMonthYear, order.MaturityMonthYear.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagPutOrCall, order.PutOrCall.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagCustomerOrFirm, "0");
        //            if (order.MsgType == FIXConstants.MSGOrder && order.CMTAID != int.MinValue)
        //            {
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagClearingFirm, order.CMTA.ToString());
        //            }
                    
        //            // Over write the symbol field for Options with only the option class
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagSymbol, order.GetOptionSymbol());
        //            break;

        //        case FIXConstants.SECURITYTYPE_Futures:
        //            // add fields
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagSecurityType, order.SecurityType);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagOpenClose, order.OpenClose);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagMaturityMonthYear, order.MaturityMonthYear.ToString());
        //            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_Tag_GiveUpID, order.GiveUpID.ToString());                    
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagSymbol, order.GetOptionSymbol());

        //            break;
        //        case FIXConstants.SECURITYTYPE_Equity:

        //            break;
        //        default:
        //            // basket trading handles BCV in different way and TT in differet way so have to check for both in case of buy to cover
        //            break;
        //    }
        //}

        //private static void AddTagsBasedOnOrderType(Order order, PranaMessage msg)
        //{
        //    switch (order.OrderTypeTagValue)
        //    {
        //        case FIXConstants.ORDTYPE_Market:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            break;
        //        case FIXConstants.ORDTYPE_Limit:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagPrice, order.Price.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            break;
        //        case FIXConstants.ORDTYPE_Stop:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagStopPx, order.StopPrice.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            break;
        //        case FIXConstants.ORDTYPE_Stoplimit:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagStopPx, order.StopPrice.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagPrice, order.Price.ToString());
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            break;
        //        case FIXConstants.ORDTYPE_MarketOnClose:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            break;
        //        case FIXConstants.ORDTYPE_Pegged:
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagTimeInForce, order.TIF);
        //            msg.FIXMessage.SetExternalField(FIXConstants.TagHandlInst, order.HandlingInstruction);
        //            if (order.PegDifference != 0)
        //            {
        //                msg.FIXMessage.SetExternalField(FIXConstants.TagPegDifference, order.PegDifference.ToString());
        //            }
        //            break;
        //    }
        //}

        #endregion

        #region Admin Messages
        public static PranaMessage CreateLogOnMessage(string ID, string name)
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGLogon,int.MinValue );
            msg.FIXMessage.SetInternalField(FIXConstants.TagMsgType, FIXConstants.MSGLogon);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserID, ID);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserName, name);
            return msg;
        }
        public static PranaMessage CreateLogOnMessage(CompanyUser user)
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGLogon, int.MinValue);
            msg.FIXMessage.SetInternalField(FIXConstants.TagMsgType, FIXConstants.MSGLogon);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserID, user.CompanyUserID.ToString());
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserName, user.FirstName + " " + user.LastName);
            string tradingAccts = string.Empty;
            for (int i = 0; i < user.TradingAccounts.Count; i++)
            {
                
                string tradingacc = ((TradingAccount)user.TradingAccounts[i]).TradingAccountID.ToString();
                if (tradingAccts != string.Empty)
                {
                    tradingAccts = tradingAccts + Seperators.SEPERATOR_1 + tradingacc;
                }
                else
                {
                    tradingAccts = tradingacc;
                }
            }
            
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_TradingAccountID, tradingAccts);
            return msg;
        }
        public static PranaMessage CreateLogOutMessage(CompanyUser user)
        {
            return CreateLogOutMessage(user.CompanyUserID.ToString(),user.FirstName+" "+user.LastName);
        }
        public static PranaMessage CreateLogOutMessage(string ID, string name)
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGLogout, int.MinValue);
            msg.FIXMessage.SetInternalField(FIXConstants.TagMsgType, FIXConstants.MSGLogout);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserID, ID);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserName, name);
            return msg;
        }


        public static PranaMessage CreateHeartBeatForUser(string ID)
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGHeartbeat, int.MinValue);
            msg.FIXMessage.SetInternalField(FIXConstants.TagMsgType, FIXConstants.MSGHeartbeat);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CompanyUserID, ID);
            return msg;


        }

        public static PranaMessage CreateCounterPartyStatusReport(int counterPartyID, int status)
        {
            PranaMessage msg = new PranaMessage(PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT, int.MinValue);
            msg.FIXMessage.SetInternalField(FIXConstants.TagMsgType, PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CounterPartyID, counterPartyID.ToString());
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CounterPartyStatus, status.ToString());
            return msg;
        }

        public static PranaMessage CreateCounterPartyDownMsg(string clOrderID)
        {
            PranaMessage msg = new PranaMessage(PranaMessageConstants.MSG_CounterPartyDown, int.MinValue);
            msg.FIXMessage.SetExternalField(FIXConstants.TagMsgType, PranaServerConstants.COUNTERPARTY_DOWN);
            msg.FIXMessage.SetExternalField(FIXConstants.TagClOrdID, clOrderID);
            return msg;
        }
        public static PranaMessage CreateCounterPartyUpMsg(string cpId)
        {
            PranaMessage msg = new PranaMessage(PranaMessageConstants.MSG_CounterPartyUp, int.MinValue);
            msg.FIXMessage.SetExternalField(FIXConstants.TagMsgType, PranaServerConstants.COUNTERPARTY_UP);
            msg.FIXMessage.SetInternalField(CustomFIXConstants.CUST_TAG_CounterPartyID, cpId);
            return msg;
        }
        public static PranaMessage CreateTestRequestMsg(string timeStamp)
        {
            PranaMessage msg = new PranaMessage(FIXConstants.MSGTestRequest);
            msg.FIXMessage.SetExternalField(FIXConstants.TagTestReqID, timeStamp);
            return msg;
        }
        #endregion
        //Not Used
        //private static PranaMessage CreatePranaMessage(Order order)
        //{
        //    PranaMessage PranaMessage = new PranaMessage();

        //    #region External Info


        //    if (order.Symbol != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol] = order.Symbol;
        //    }
        //    if (order.ClOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID] = order.ClOrderID;
        //    }
        //    if (order.Quantity != 0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty] = order.Quantity.ToString();
        //    }
        //    //Dileep

        //    if (order.OrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID] = order.OrderID.ToString();
        //    }
        //    if (order.OrigClOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID] = order.OrigClOrderID.ToString();
        //    }

        //    //end

        //    if (order.Price != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagPrice] = order.Price.ToString();
        //    }
        //    if (order.OrderSideTagValue != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide] = order.OrderSideTagValue;
        //    }



        //    if (order.MsgType != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType] = order.MsgType.ToString();
        //    }
        //    if (order.OrderSideTagValue != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide] = order.OrderSideTagValue.ToString();
        //    }
        //    if (order.OrderTypeTagValue != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdType] = order.OrderTypeTagValue.ToString();
        //    }
        //    if (order.OrderStatusTagValue != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus] = order.OrderStatusTagValue.ToString();
        //    }
        //    if (order.StopPrice != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagStopPx] = order.StopPrice.ToString();
        //    }



        //    if (order.HandlingInstruction != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagHandlInst] = order.HandlingInstruction.ToString();
        //    }
        //    if (order.ExecutionInstruction != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecInst] = order.ExecutionInstruction.ToString();
        //    }
        //    if (order.TIF != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTimeInForce] = order.TIF.ToString();
        //    }



        //    if (order.TargetSubID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTargetSubID] = order.TargetSubID.ToString();
        //    }
        //    if (order.SenderSubID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSenderSubID] = order.SenderSubID.ToString();
        //    }
        //    if (order.SenderCompID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSenderCompID] = order.SenderCompID.ToString();
        //    }
        //    if (order.TargetCompID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTargetCompID] = order.TargetCompID.ToString();
        //    }



        //    if (order.SendingTime != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSendingTime] = order.SendingTime.ToString();
        //    }
        //    if (order.ExecTransType != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecTransType] = order.ExecTransType.ToString();
        //    }
        //    if (order.MsgSeqNum != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgSeqNum] = order.MsgSeqNum.ToString();
        //    }
        //    if (order.ExecID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID] = order.ExecID.ToString();
        //    }
        //    if (order.LastShares != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastShares] = order.LastShares.ToString();
        //    }
        //    if (order.LastPrice != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastPx] = order.LastPrice.ToString();
        //    }
        //    if (order.CumQty != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty] = order.CumQty.ToString();
        //    }
        //    if (order.AvgPrice != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx] = order.AvgPrice.ToString();
        //    }
        //    if (order.LeavesQty != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLeavesQty] = order.LeavesQty.ToString();
        //    }
        //    if (order.ExecType != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecType] = order.ExecType.ToString();
        //    }
        //    if (order.DiscretionInst != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagDiscretionInst] = order.DiscretionInst.ToString();
        //    }
        //    if (order.DiscretionOffset != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagDiscretionOffset] = order.DiscretionOffset.ToString();
        //    }
        //    if (order.PegDifference != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagPegDifference] = order.PegDifference.ToString();
        //    }



        //    if (order.LastMarket != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagLastMkt] = order.LastMarket.ToString();
        //    }
        //    if (order.TransactionTime != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime] = order.TransactionTime.ToString();
        //    }


        //    if (order.CMTAID != 0.0)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClearingFirm] = order.CMTAID.ToString();
        //    }



        //    if (order.PossDupFlag != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagPossDupFlag] = order.PossDupFlag.ToString();
        //    }

        //    #endregion External Info


        //    #region Internal Info

        //    if (order.TradingAccountID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID] = order.TradingAccountID.ToString();
        //    }

        //    if (order.CounterPartyID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID] = order.CounterPartyID.ToString();
        //    }

        //    if (order.VenueID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_VenueID] = order.VenueID.ToString();
        //    }

        //    if (order.AssetID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AssetID] = order.AssetID.ToString();
        //    }

        //    if (order.UnderlyingID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_UnderlyingID] = order.UnderlyingID.ToString();
        //    }

        //    if (order.ExchangeID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ExchangeID] = order.ExchangeID.ToString();
        //    }

        //    if (order.CurrencyID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CurrencyID] = order.CurrencyID.ToString();
        //    }

        //    if (order.AUECID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID] = order.AUECID.ToString();
        //    }

        //    if (order.CompanyUserID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID] = order.CompanyUserID.ToString();
        //    }

        //    if (order.ListID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ListID] = order.ListID;
        //    }

        //    if (order.WaveID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_WaveID] = order.WaveID;
        //    }

        //    if (order.GroupID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_GroupID] = order.GroupID;
        //    }

        //    if (order.BasketSequenceNumber != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BasketSequenceNumber] = order.BasketSequenceNumber.ToString();
        //    }

        //    if (order.FundID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_FundID] = order.FundID.ToString();
        //    }

        //    if (order.StrategyID != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StrategyID] = order.StrategyID.ToString();
        //    }

        //    if (order.PranaMsgType != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType] = order.PranaMsgType.ToString();
        //    }

        //    if (order.SendQty != int.MinValue)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SendQty] = order.SendQty.ToString();
        //    }

        //    if (order.Text != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Text] = order.Text;
        //    }

        //    if (order.StagedOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID] = order.StagedOrderID;
        //    }

        //    if (order.ParentClOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID] = order.ParentClOrderID;
        //    }

        //    if (order.ClientOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientOrderID] = order.ClientOrderID;
        //    }

        //    if (order.ParentClientOrderID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClientOrderID] = order.ParentClientOrderID;
        //    }

        //    if (order.ClientTime != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientTime] = order.ClientTime;
        //    }
        //    if (order.BorrowerID != string.Empty)
        //    {
        //        PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BorrowerID] = order.BorrowerID.ToString();
        //    }
        //    #endregion Internal Info


        //    //ClientTime


        //    return PranaMessage;
        //}

        public static PranaMessage CreatePranaMessageThroughReflection(Order order)
        {
            PranaMessage PranaMessage = new PranaMessage();
            PranaMessage.MessageType = order.MsgType;
           
            foreach (PropertyInfo property in propertiesOrder)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                if (fixfield != null)
                {
                    if (fixfield.Tag != string.Empty)
                    {
                        string value = property.GetValue(order, null).ToString();
                        if (value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString())
                        {

                        }
                        else
                        {
                            if (fixfield.IsExternal)
                            {
                                PranaMessage.FIXMessage.ExternalInformation[fixfield.Tag] = value;
                            }
                            else
                            {
                                PranaMessage.FIXMessage.InternalInformation[fixfield.Tag] = value;
                            }
                        }
                    }
                }
            }
          
            CreateOpenCloseTags(PranaMessage);
            return PranaMessage;

        }
        public static PranaMessage CreatePranaMessageThroughReflection(OrderSingle order)
        {
            PranaMessage PranaMessage = new PranaMessage();
            PranaMessage.MessageType = order.MsgType;

            foreach (PropertyInfo property in propertiesOrderSingle)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                if (fixfield != null)
                {
                    if (fixfield.Tag != string.Empty)
                    {
                        string value = property.GetValue(order, null).ToString();
                        if (value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString())
                        {

                        }
                        else
                        {
                            if (fixfield.IsExternal)
                            {
                                PranaMessage.FIXMessage.ExternalInformation[fixfield.Tag] = value;
                            }
                            else
                            {
                                PranaMessage.FIXMessage.InternalInformation[fixfield.Tag] = value;
                            }
                        }
                    }
                }
            }
            CreateOpenCloseTags(PranaMessage);
            return PranaMessage;

        }
        public static PranaMessage CreatePranaMessageThroughReflection(BasketDetail basket,OrderCollection orders,string msgType)
        {
            PranaMessage PranaMessage = new PranaMessage();
            PranaMessage.MessageType = msgType;
            PranaMessage.FIXMessageList.GroupID = basket.CurrentGroupID;
            PranaMessage.FIXMessageList.WaveID = basket.CurrentWaveID;
            PranaMessage.FIXMessageList.UserID = basket.UserID.ToString();
            PranaMessage.FIXMessageList.BasketID = basket.BasketID ;
            PranaMessage.FIXMessageList.TradedBasketID = basket.TradedBasketID;
            PranaMessage.FIXMessageList.TradingAccountID = basket.TradingAccountID.ToString();
            foreach (Order order in orders)
            {
                PranaMessage singleMessageOrder= CreatePranaMessageThroughReflection(order);
                PranaMessage.FIXMessageList.AddMessage(singleMessageOrder.FIXMessage);
            }
            return PranaMessage;
        }
        public static PranaMessage CreatePranaMessageThroughReflection(DropCopyOrder order)
        {
            PranaMessage PranaMessage = new PranaMessage();

            try
            {
                PranaMessage.MessageType = order.MsgType;
                foreach (PropertyInfo property in propertiesDropCpyOrder)
                {
                    //property.Name 
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                    if (fixfield != null)
                    {
                        if (fixfield.Tag != string.Empty)
                        {
                            string value = property.GetValue(order, null).ToString();
                            if (value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString())
                            {

                            }
                            else
                            {
                                if (fixfield.IsExternal)
                                {
                                    PranaMessage.FIXMessage.ExternalInformation[fixfield.Tag] = value;
                                }
                                else
                                {
                                    PranaMessage.FIXMessage.InternalInformation[fixfield.Tag] = value;
                                }
                            }
                        }
                    }
                }

                CreateOpenCloseTags(PranaMessage);
            }
            catch (Exception ex)
            {
                
                throw ex ;
            }
            return PranaMessage;

        }
        public static Order CreateOrder(PranaMessage PranaMessage)
        {
            Order order = new Order();

            foreach (PropertyInfo property in propertiesOrder)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                if (fixfield != null)
                {

                    string tag = fixfield.Tag;
                    if (tag != string.Empty)
                    {
                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.ExternalInformation[tag];
                            property.SetValue(order, GetObjectValue(property.PropertyType.FullName, value), null);
                        }
                        else if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.InternalInformation[tag];
                            property.SetValue(order, GetObjectValue(property.PropertyType.FullName, value), null);
                        }



                    }
                }
            }
            return order;



        }
        public static OrderSingle CreateOrderSingle(PranaMessage PranaMessage)
        {
            OrderSingle order = new OrderSingle();

            foreach (PropertyInfo property in propertiesOrderSingle)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                if (fixfield != null)
                {

                    string tag = fixfield.Tag;
                    if (tag != string.Empty)
                    {
                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.ExternalInformation[tag];
                            property.SetValue(order, GetObjectValue(property.PropertyType.FullName, value), null);
                        }
                        else if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.InternalInformation[tag];
                            property.SetValue(order, GetObjectValue(property.PropertyType.FullName, value), null);
                        }



                    }
                }
            }
            return order;



        }
        public static DropCopyOrder CreateDropCopy(PranaMessage PranaMessage)
        {
            DropCopyOrder dropCopyOrder = new DropCopyOrder();

            foreach (PropertyInfo property in propertiesDropCpyOrder)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                //fixfield.
                if (fixfield != null)
                {

                    string tag = fixfield.Tag;
                    if (tag != string.Empty)
                    {
                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.ExternalInformation[tag];
                            property.SetValue(dropCopyOrder, GetObjectValue(property.PropertyType.FullName, value), null);
                        }
                        else if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                        {
                            string value = PranaMessage.FIXMessage.InternalInformation[tag];
                            property.SetValue(dropCopyOrder, GetObjectValue(property.PropertyType.FullName, value), null);
                        }
                    }
                                           
                }
            }
            return dropCopyOrder;



        }
        public static BasketDetail CreateBasket(PranaMessage PranaMessage)
        {
            BasketDetail basket = new BasketDetail();
            basket.BasketID = PranaMessage.FIXMessageList.BasketID;
            basket.TradedBasketID = PranaMessage.FIXMessageList.TradedBasketID;
            int tradingID = int.MinValue;
            int.TryParse(PranaMessage.FIXMessageList.TradingAccountID, out tradingID);
            basket.TradingAccountID=tradingID;
            int userID = int.MinValue;
            int.TryParse(PranaMessage.FIXMessageList.UserID, out userID);
            basket.UserID = userID;

            foreach (FIXMessage  fixMsg in PranaMessage.FIXMessageList.ListMessages)
            {
                PranaMessage PranaMsgOrder = new PranaMessage();
                PranaMsgOrder.FIXMessage = fixMsg;
                PranaMsgOrder.MessageType = fixMsg.ExternalInformation[FIXConstants.TagMsgType];
                Order order = CreateOrder(PranaMsgOrder);
                basket.BasketOrders.Add(order);
            }

            return basket;
        }
        public static object GetObjectValue(string name, string value)
        {
            object valueActual = null;
            if (name == "System.Double")
            {
                valueActual = double.Parse(value);
            }
            else if (name == "System.Int32")
            {
                valueActual = Int32.Parse(value);
            }
            else if (name == "System.Int64")
            {
                valueActual = Int64.Parse(value);
            }
            else if (name == "System.Boolean")
            {
                valueActual = bool.Parse(value);
            }
            else if (name == "System.DateTime")
            {
                valueActual = DateTime.Parse(value);
            }
            else if (name == "Prana.BusinessObjects.OrderAlgoStartegyParameters")
            {
                valueActual = new OrderAlgoStartegyParameters(value);
            }
            else
            {
                valueActual = value;
            }
            return valueActual;
        }

        public static Message CreateFixMessage(PranaMessage PranaMsg)
        {
            try
            {
                Message fixMessage = new Message();
                fixMessage.SetField(FIXConstants.TagMsgType, PranaMsg.MessageType);
                foreach (string tag in PranaMsg.FIXMessage.RequiredFixFields)
                {
                    fixMessage.SetField(tag, PranaMsg.FIXMessage.ExternalInformation[tag]);
                }
                foreach (KeyValuePair<string, string> customInfo in PranaMsg.FIXMessage.CustomInformation)
                {
                    fixMessage.SetField(customInfo.Key, customInfo.Value);
                }
                if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AlgoProperties))
                {
                    string algoParameters = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AlgoProperties];
                    OrderAlgoStartegyParameters AlgoProperties = new OrderAlgoStartegyParameters(algoParameters);
                    foreach (KeyValuePair<string, string> keyvaluepairAlgo in AlgoProperties.TagValueDictionary)
                    {
                        fixMessage.SetField(keyvaluepairAlgo.Key, keyvaluepairAlgo.Value);
                    }

                }
                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                {
                    CreateOpenCloseTags(fixMessage);
                }
                return fixMessage;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        public static DataTable CreateDataTable(List<string> columns, Dictionary<string, PranaMessage> PranaMsgCollection)
        {
            DataTable dt = new DataTable();
            foreach (string column in columns)
            {
                dt.Columns.Add(column);
            }

            foreach (KeyValuePair<string, PranaMessage> PranaMsgKeyValuePair in PranaMsgCollection)
            {
                object[] row = new object[columns.Count];
                int i = 0;

                foreach (string column in columns)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagName(column);
                    if (fixField != null)
                    {
                        string tag = fixField.Tag;
                        if (fixField.IsExternal)
                        {
                            if (PranaMsgKeyValuePair.Value.FIXMessage.ExternalInformation.ContainsKey(tag))
                            {
                                row[i] = PranaMsgKeyValuePair.Value.FIXMessage.ExternalInformation[tag];
                            }
                            else
                            {
                                row[i] = string.Empty;
                            }
                        }
                        else
                        {
                            if (PranaMsgKeyValuePair.Value.FIXMessage.InternalInformation.ContainsKey(tag))
                            {
                                row[i] = PranaMsgKeyValuePair.Value.FIXMessage.InternalInformation[tag];
                            }
                            else
                            {
                                row[i] = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        row[i] = string.Empty;
                    }
                    i++;

                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static PranaMessage CreatePranaMessage(Message fixMessage)
        {
            PranaMessage PranaMessage = new PranaMessage();
            foreach (AdapterClient.FIX.Field field in fixMessage.Fields)
            {
                PranaMessage.FIXMessage.ExternalInformation.Add(field.Name, field.Value);
            }
            PranaMessage.MessageType = fixMessage[FIXConstants.TagMsgType].Value;
            
            return PranaMessage;
        }

        private static void CreateOpenCloseTags(Message fixMessage)
        {
           
            switch (fixMessage[FIXConstants.TagSide].Value)
            {
                case FIXConstants.SIDE_Buy_Open:
                    fixMessage.SetField(FIXConstants.TagSide,FIXConstants.SIDE_Buy);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Buy_Closed:
                    fixMessage.SetField(FIXConstants.TagSide,FIXConstants.SIDE_Buy);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
                case FIXConstants.SIDE_Sell_Open:
                    fixMessage.SetField(FIXConstants.TagSide,FIXConstants.SIDE_Sell);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Sell_Closed:
                    fixMessage.SetField(FIXConstants.TagSide,FIXConstants.SIDE_Sell);
                    fixMessage.SetField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
            }

        }
        private static void CreateOpenCloseTags(PranaMessage PranaMessage)
        {
            if (!PranaMessage.FIXMessage.ExternalInformation.ContainsKey (FIXConstants.TagSide))
                return;
            switch (PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide])
            {
                case FIXConstants.SIDE_Buy_Open:

                    PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOpenClose] = FIXConstants.Open;
                    break;
                case FIXConstants.SIDE_Buy_Closed:
                    
                    PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOpenClose]= FIXConstants.Close;
                    break;
                case FIXConstants.SIDE_Sell_Open:

                    PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOpenClose] = FIXConstants.Open;
                    break;
                case FIXConstants.SIDE_Sell_Closed:
                    
                    PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOpenClose]= FIXConstants.Close;
                    break;
            }

        }
    }
}
