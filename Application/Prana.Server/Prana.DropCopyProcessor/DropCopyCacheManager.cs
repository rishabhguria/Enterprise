using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DatabaseManager;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.DropCopyProcessor
{
    public class DropCopyCacheManager
    {
        static Dictionary<string, PranaMessage> _receivedMessages = new Dictionary<string, PranaMessage>();
        static Dictionary<string, PranaMessage> _sentMessages = new Dictionary<string, PranaMessage>();
        static Dictionary<string, List<string>> _parentChildCollection = new Dictionary<string, List<string>>();

        public static void AddReceivedMessage(PranaMessage pranaMessage)
        {
            string clientOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
            // need to clone it to keep original order . as processing may change the object
            _receivedMessages.Add(clientOrderID, pranaMessage.Clone());
            _parentChildCollection.Add(clientOrderID, new List<string>());
        }

        public static void AddSentMessage(PranaMessage pranaMessage)
        {
            string executionID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value;
            _sentMessages.Add(executionID, pranaMessage);
        }
        public static void AddChildMessage(PranaMessage pranaMessage)
        {
            string clientOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
            string orderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
            if (_parentChildCollection.ContainsKey(clientOrderID))
            {
                if (!_parentChildCollection[clientOrderID].Contains(orderID))
                {
                    _parentChildCollection[clientOrderID].Add(orderID);
                }
            }
            else
            {
                _parentChildCollection.Add(clientOrderID, new List<string>());
            }
        }
        /// <summary>
        /// updates the fill of drop copy and received copy 
        /// </summary>
        /// <param name="childMessage"></param>
        /// <returns></returns>
        public static bool UpdateParentOrderAndChildOrders(PranaMessage childMessage)
        {
            bool shouldSend = true;
            try
            {
                string ClientOrderID = childMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientOrderID].Value;

                PranaMessage parentMessage = GetReceivedMessage(ClientOrderID);

                //copy details from cached order
                childMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value;
                childMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value = parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value;
                //clorder Id mapping

                childMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value = childMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                childMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = parentMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;

                //if status new add this message 
                if (childMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_New)
                {
                    AddChildMessage(childMessage);
                    parentMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                    parentMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_New);
                    shouldSend = false;
                    //if (!parentMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                    //{
                    //    parentMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_New;
                    //}
                    //else
                    //{
                    //    shouldSend = false;
                    //}
                }
                else
                {
                    // symbol mapping to be done
                    // status calculation
                    double totalQty = double.Parse(parentMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                    childMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = totalQty.ToString();

                    List<string> childOrders = null;
                    if (_parentChildCollection.ContainsKey(ClientOrderID))
                    {
                        childOrders = _parentChildCollection[ClientOrderID];
                    }
                    double parentCumQty = 0;
                    double tempQtyPrice = 0;

                    if (childOrders != null)
                    {
                        foreach (string clorderID in childOrders)
                        {
                            PranaMessage childItem = OrderCacheManager.GetCachedOrder(clorderID);
                            double cumQty = double.Parse(childItem.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);

                            double lastPx = 0;
                            if (childItem.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastPx))
                            {
                                lastPx = double.Parse(childItem.FIXMessage.ExternalInformation[FIXConstants.TagLastPx].Value);
                            }
                            parentCumQty += cumQty;
                            tempQtyPrice += cumQty * lastPx;
                        }
                    }
                    double avgPrice = 0;
                    if (parentCumQty != 0)
                    {
                        avgPrice = tempQtyPrice / parentCumQty;
                    }
                    parentMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = parentCumQty.ToString();
                    childMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = parentCumQty.ToString();
                    double leavesQty = totalQty - parentCumQty;
                    childMessage.FIXMessage.ExternalInformation[FIXConstants.TagLeavesQty].Value = leavesQty.ToString();
                    childMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value = avgPrice.ToString();

                    // avg price cum qty calcultion
                    //receivedMsg.FIXMessage.ExternalInformation[FIXConstants.TagCumQty] = messageToSend.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty];
                    if (childMessage.MessageType == CustomFIXConstants.MsgDropCopyReject)
                    {
                        childMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagRefMsgType, parentMessage.MessageType);
                        childMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagBusinessRejectReason, childMessage.FIXMessage.ExternalInformation[FIXConstants.TagText].Value);
                    }
                    else
                    {
                        ServerCommonBusinessLogic.SetOrderStatus(childMessage);
                        ServerCommonBusinessLogic.SetOrderStatus(parentMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return shouldSend;
        }


        public static PranaMessage GetReceivedMessage(string clientOrderID)
        {
            return _receivedMessages[clientOrderID];
        }
        public static Dictionary<string, PranaMessage> ReceivedMessages
        {
            get { return _receivedMessages; }
        }
        public static Dictionary<string, PranaMessage> SentMessages
        {
            get { return _sentMessages; }
        }

        /// <summary>
        /// Fills the DropCopy Orders from the Database
        /// used when the server is down and we want to reload the drop copy orders from the database.
        /// </summary>
        public static void FillClientDropCopyOrdersFromDB()
        {
            try
            {
                DCOCNew dropcopyOrdersRecieved = GetClientRecievedDropCopyOrders();
                DCOCNew dropcopyOrdersSent = GetClientSentDropCopyOrders();
                GetDropCopyChildOrdres();

                foreach (DropCopyOrder dropcopyOrder in dropcopyOrdersRecieved)
                {
                    PranaMessage pranaMessage = Transformer.CreatePranaMessageThroughReflection(dropcopyOrder);
                    pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginatorType].Value = ((int)PranaServerConstants.OriginatorType.DropCopy).ToString();
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientOrderID].Value);
                    _receivedMessages.Add(dropcopyOrder.ClientOrderID, pranaMessage);
                }

                foreach (DropCopyOrder dropcopyOrder in dropcopyOrdersSent)
                {
                    PranaMessage pranaMessage = Transformer.CreatePranaMessageThroughReflection(dropcopyOrder);
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientOrderID].Value);
                    string executionID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value;
                    _sentMessages.Add(executionID, pranaMessage);
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
        }

        /// <summary>
        /// For Inbox Orders
        /// </summary>
        /// <returns>DCOCNew DropCopyOrder Collection</returns>
        /// 
        private static DCOCNew GetClientRecievedDropCopyOrders()
        {
            DCOCNew dropcopyOrdersRecieved = new DCOCNew();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDropCopyOrdersFromClientSub";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        DropCopyOrder dropcopyOrder = new DropCopyOrder();
                        dropcopyOrder.ClientOrderID = Convert.ToString(reader[0]);
                        dropcopyOrder.Symbol = reader[1].ToString();
                        dropcopyOrder.Broker = Convert.ToString(reader[2]);
                        dropcopyOrder.OrderSideTagValue = reader[3].ToString();
                        dropcopyOrder.OrderTypeTagValue = Convert.ToString(reader[4]);
                        dropcopyOrder.Quantity = Convert.ToDouble(reader[5]);
                        dropcopyOrder.Price = Convert.ToDouble(reader[6]);
                        dropcopyOrder.TIF = Convert.ToString(reader[7]);
                        dropcopyOrder.MsgType = Convert.ToString(reader[8]);
                        dropcopyOrder.CounterPartyID = Convert.ToInt32(reader[9]);
                        dropcopyOrder.VenueID = Convert.ToInt32(reader[10]);
                        dropcopyOrder.AUECID = int.Parse(reader[11].ToString());
                        dropcopyOrder.Text = Convert.ToString(reader[12]);
                        dropcopyOrdersRecieved.Add(dropcopyOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dropcopyOrdersRecieved;
        }

        /// <summary>
        /// For OutBox Orders
        /// </summary>
        /// <returns>DCOCNew DropCopyOrder Collection</returns>
        /// 
        private static DCOCNew GetClientSentDropCopyOrders()
        {
            DCOCNew dropcopyOrdersSent = new DCOCNew();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDropCopyOrdersFromClientFills";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        DropCopyOrder dropcopyOrder = new DropCopyOrder();
                        dropcopyOrder.ExecID = reader[0].ToString();
                        dropcopyOrder.ClientOrderID = Convert.ToString(reader[8]);
                        dropcopyOrder.Text = Convert.ToString(reader[32]);
                        dropcopyOrder.Price = Convert.ToDouble(reader[15]);
                        dropcopyOrder.Symbol = Convert.ToString(reader[11]);
                        dropcopyOrder.Quantity = Convert.ToDouble(reader[13]);
                        dropcopyOrder.MsgType = Convert.ToString(reader[2]);
                        dropcopyOrder.CumQty = Convert.ToDouble(reader[21]);
                        dropcopyOrder.AvgPrice = Convert.ToDouble(reader[20]);
                        dropcopyOrder.OrderStatusTagValue = Convert.ToString(reader[24]);
                        dropcopyOrder.OrderSideTagValue = Convert.ToString(reader[12]);
                        dropcopyOrder.OrderTypeTagValue = Convert.ToString(reader[14]);
                        dropcopyOrdersSent.Add(dropcopyOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return dropcopyOrdersSent;
        }

        /// <summary>
        /// For OutBox Orders
        /// </summary>
        /// <returns>DCOCNew DropCopyOrder Collection</returns>
        /// 
        private static DCOCNew GetDropCopyChildOrdres()
        {
            DCOCNew dropcopyOrdersSent = new DCOCNew();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDropCopyChildOrdres";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        string clientOrderID = reader[0].ToString();
                        string clOrderID = reader[1].ToString();
                        if (!_parentChildCollection.ContainsKey(clientOrderID))
                        {
                            _parentChildCollection.Add(clientOrderID, new List<string>());
                        }
                        else
                        {
                            if (!_parentChildCollection[clientOrderID].Contains(clOrderID))
                            {
                                _parentChildCollection[clientOrderID].Add(clOrderID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return dropcopyOrdersSent;
        }
    }
}
