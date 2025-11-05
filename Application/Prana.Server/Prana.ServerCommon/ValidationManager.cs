using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;

namespace Prana.ServerCommon
{
    public class ValidationManager
    {
        private static ValidationManager _validationManager = null;
        static ValidationManager()
        {
            _validationManager = new ValidationManager();
        }

        public static ValidationManager GetInstance()
        {
            return _validationManager;
        }

        public bool ValidateOrder(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                switch (pranaMessage.MessageType)
                {
                    case CustomFIXConstants.MsgDropCopyReceived:
                        string tagMsgType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString();
                        if (tagMsgType == FIXConstants.MSGOrder)
                        {
                            isValid = true;
                        }
                        else if (tagMsgType == FIXConstants.MSGExecution)
                        {
                            isValid = ValidateExecutionReport(pranaMessage);
                        }
                        else if (tagMsgType == FIXConstants.MSGOrderCancelRequest)
                        {
                            isValid = ValidateOrderCancelRequest(pranaMessage);
                        }
                        else if (tagMsgType == FIXConstants.MSGOrderRollOverRequest)
                        {
                            isValid = ValidateOrderRolloverRequest(pranaMessage);
                        }
                        else if (tagMsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            isValid = ValidateOrderCancelReplaceRequest(pranaMessage);
                        }
                        else if (tagMsgType == FIXConstants.MSGOrderCancelReject)
                        {
                            isValid = OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        }
                        break;

                    case FIXConstants.MSGOrder:
                        isValid = true;
                        break;

                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                        isValid = true;
                        break;

                    case FIXConstants.MSGOrderCancelRequest:
                        isValid = ValidateOrderCancelRequest(pranaMessage);
                        break;

                    case FIXConstants.MSGOrderRollOverRequest:
                        isValid = ValidateOrderCancelRequest(pranaMessage);
                        break;

                    case FIXConstants.MSGOrderCancelReplaceRequest:
                        isValid = ValidateOrderCancelReplaceRequest(pranaMessage);
                        break;

                    case FIXConstants.MSGExecution:
                        isValid = ValidateExecutionReport(pranaMessage);
                        break;

                    case FIXConstants.MSGOrderCancelReject:
                        isValid = OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        break;

                    case FIXConstants.MSGTransferUser:
                        isValid = OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                        break;

                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                        isValid = true;
                        break;

                    case PranaMessageConstants.MSGTradingInstInternalAccept:
                    case PranaMessageConstants.MSGTradingInstClient:
                    case PranaMessageConstants.MSGTradingInstInternal:
                    case PranaMessageConstants.MSGTradingInstClientAccept:
                        isValid = true;
                        break;

                    case FIXConstants.MSGBusinessMessageReject:
                    case FIXConstants.MSGReject:
                        isValid = true;
                        Logger.HandleException(new Exception("Business Message Reject Received for PranaMessage:=" + pranaMessage.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                        break;

                    case FIXConstants.MSGOrderCancelRequestFroze:
                    case FIXConstants.MSGOrderCancelRequestUnFroze:
                        isValid = true;
                        break;

                    default:
                        Logger.HandleException(new Exception("MsgType Not Recognised at Validating Message at Message Validator"), LoggingConstants.POLICY_LOGANDSHOW);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateOrder", pranaMessage.ToString());
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return isValid;
        }

        private bool ValidateOrderCancelRequest(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                if (OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value))
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateOrderCancelRequest", pranaMessage.ToString());
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }

        private bool ValidateOrderRolloverRequest(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                if (OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value))
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateOrderRolloverRequest", pranaMessage.ToString());
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }

        private bool ValidateOrderCancelReplaceRequest(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                if (OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value))
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateOrderCancelReplaceRequest", pranaMessage.ToString());
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }

        private bool ValidateExecutionReport(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                if (OrderCacheManager.DoesOrderExist(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value))
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateExecutionReport", pranaMessage.ToString());
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }

        public bool ValidateClientOrders(PranaMessage pranaMessage)
        {
            bool isValid = false;
            try
            {
                if (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value != string.Empty)
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("ValidateClientOrders", pranaMessage.ToString());
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return isValid;
        }

        private void ExceptinoHandlingLocal(string methodName, string msgToLog)
        {
            string error = "Exception in Message:=" + msgToLog + " at " + methodName;
            Logger.LoggerWrite(msgToLog, LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
            Logger.HandleException(new Exception(error), LoggingConstants.POLICY_LOGANDSHOW);
        }

        public bool IsBasketOrder(PranaMessage pranaMsg)
        {
            if (!pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ListID))
            {
                return false;
            }
            if (pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ListID].Value == string.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
