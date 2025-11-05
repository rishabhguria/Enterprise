using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
namespace Prana.OrderProcessor
{
    class ManualMessageHandler
    {
        public static PranaMessage GenerateExecutionReport(PranaMessage pranaMessage)
        {
            PranaMessage fill = new PranaMessage();
            try
            {
                OrderCacheManager.UpdateChildDetailsFromParent(pranaMessage);
                double leavesQty = 0.0;
                fill = pranaMessage.Clone();
                fill.MessageType = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecTransType, FIXConstants.EXECTYPE_New);

                // needed here as seq number is assigned as soon as a fill is received
                // here we generate fill for manual so need to assign Next Sequence number
                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                string cumQty = "0";
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty] != null)
                    cumQty = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value;
                string avgPx = "0";
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx] != null)
                    avgPx = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value;

                fill.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                Logger.LoggerWrite("OrderSeqNumber Used=" + orderIDSeqNumber, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                if (Convert.ToInt32(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != 3)
                {
                    leavesQty = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) - Convert.ToDouble(cumQty);
                }
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, cumQty);
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, avgPx);

                //orderRequest.MsgType = AdapterClient.FIX.Constants.MSGExecutionReport;
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, cumQty);
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, avgPx);

                //  ServerCommonBusinessLogic.SetOrderStatus(PranaMessage);
                ServerCommonBusinessLogic.SetOrderStatus(fill);
                OrderCacheManager.UpdateExecutionReport(fill);
                OrderCacheManager.UpdateCachedMessage(fill);
            }
            #region Catch
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
            #endregion
            return fill;
        }

        public static PranaMessage GenerateReplacedReport(PranaMessage pranaMessage)
        {
            PranaMessage fill = new PranaMessage();
            try
            {
                fill = pranaMessage.Clone();
                //MsgType is set to Replace request because details are to filled from the Cached accordingly.
                //fill.MessageType = PranaMessage.MsgType;
                //OrderCacheManager.FillOrderDetailsFromCache(fill);
                fill.MessageType = FIXConstants.MSGExecutionReport;

                fill.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());
                double leavesQty = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) - Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");// orderRequest.CumQty;
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");// orderRequest.AvgPrice;

                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();

                fill.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                Logger.LoggerWrite("OrderSeqNumber Used=" + orderIDSeqNumber, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Replaced);
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");// orderRequest.CumQty;
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");// orderRequest.AvgPrice;
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Replaced);
            }
            #region Catch
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
            #endregion
            return fill;
        }

        public static PranaMessage GenerateCancelledReport(PranaMessage pranaMessage)
        {
            PranaMessage fill = new PranaMessage();
            try
            {
                fill = pranaMessage.Clone();
                //MsgType is set to Cancel request because details are to filled from the Cached accordingly.
                //OrderCacheManager.FillOrderDetailsFromCache(fill);
                fill.MessageType = FIXConstants.MSGExecutionReport;

                fill.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, "0");
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");
                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                fill.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                Logger.LoggerWrite("OrderSeqNumber Used=" + orderIDSeqNumber, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);

                int PranaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                if ((PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild))
                {
                    fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Cancelled);
                }
                else
                {
                    fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
                }
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, "0");
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");
                if ((PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild))
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Cancelled);
                }
                else
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
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
            return fill;
        }

        public static PranaMessage GenerateRolloverReport(PranaMessage pranaMessage)
        {
            PranaMessage fill = new PranaMessage();
            try
            {
                fill = pranaMessage.Clone();
                fill.MessageType = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGExecutionReport;
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, "0");
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");
                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                fill.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                Logger.LoggerWrite("OrderSeqNumber Used=" + orderIDSeqNumber, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);

                int PranaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_RollOver);
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, "0");
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_RollOver);
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
            return fill;
        }

        internal static PranaMessage GenerateReplacedCancelledReport(PranaMessage pranaMessage)
        {
            try
            {
                string clOrderId = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                string orignalClOrderId = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(FIXConstants.TagText))
                    pranaMessage.FIXMessage.InternalInformation.RemoveField(FIXConstants.TagText);
                PranaMessage originalMessage = new PranaMessage(OrderCacheManager.GetCachedOrder(orignalClOrderId).ToString());
                if (originalMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                    originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value = orignalClOrderId;
                else
                    originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, orignalClOrderId);
                originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = clOrderId;
                originalMessage.MessageType = FIXConstants.MSGOrderCancelReject;
                if (originalMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMsgType))
                    originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrderCancelReject;
                else
                    originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderCancelReject);
                originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());


                double leavesQty = Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) - Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastShares, "0");// orderRequest.CumQty;
                originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLastPx, "0");// orderRequest.AvgPrice;
                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();

                originalMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                Logger.LoggerWrite("OrderSeqNumber Used=" + orderIDSeqNumber, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);

                int nirvanaMsgType = Convert.ToInt32(originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual || nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub || nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged)
                {
                    string newOrderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                    string orderStatus = string.Empty;
                    if (newOrderStatus == FIXConstants.ORDSTATUS_Filled)
                        orderStatus = newOrderStatus;
                    else
                        orderStatus = originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                    switch (orderStatus)
                    {
                        case FIXConstants.ORDSTATUS_PendingNew:
                            orderStatus = FIXConstants.ORDSTATUS_New;
                            break;
                        case FIXConstants.ORDSTATUS_PendingReplace:
                            orderStatus = FIXConstants.ORDSTATUS_Replaced;
                            break;
                    }
                    originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = orderStatus;
                }
                return originalMessage;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
