using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Prana.BusinessObjects;
using Prana.Global;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Prana.ServerClientCommon;
using Prana.Interfaces;
using System.Collections.Generic;
using Prana.QueueManager;
using Prana.BusinessObjects.FIX;
namespace Prana.Client.Core
{
    //Public delegates	
  

    //Custom EventArgs
    
	public class ClientTradeCommManager: ICommunicationManager
    {
        #region Private Variables
        private Dictionary<int, CounterPartyDetails> _counterPartiesConnectionStatus = new Dictionary<int, CounterPartyDetails>();
        private SocketConnection _socketConn = null;
        private ITradeQueueProcessor _socketQueue;
        #region Events 
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        

        public event OrderSingleMessageDelegate OrderSingleReceived;
        public event BasketOrderMessageDelegate BasketOrderReceived;
        public event BasketMessageDelegate BasketReceived;
        public event QueuedDelegate OrderQueued;
        public event CounterPartyConnectHandler CounterPartyStatusUpdate;
        #endregion
        #endregion

        public  ClientTradeCommManager()
		{
            _socketQueue = new TradeQueueManager(this);
            _socketQueue.MessageQueued +=new PranaMessageReceivedHandler(_socketQueue_MessageQueued);
        }

        void _socketQueue_MessageQueued(object sender, PranaMessage  pranaMsg)
        {
            try
            {
                switch (pranaMsg.MessageType)
                {

                    case PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT:
                        int id = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID]);
                        int connStatus = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyStatus]);
                        //string[] messageArray = message.Split(',');
                        //int id = int.Parse(messageArray[1]);
                        //int connStatus = int.Parse(messageArray[2]);
                        if (_counterPartiesConnectionStatus.ContainsKey(id))
                        {
                            SetCounterPartyStatus(id, connStatus);
                            if (CounterPartyStatusUpdate != null)
                            {
                                CounterPartyStatusUpdate(_counterPartiesConnectionStatus[id]);
                            }
                        }

                        break;
                    case PranaMessageConstants.MSG_CounterPartyDown:
                        Order ordercpDown = Transformer.CreateOrder(pranaMsg);
                        OrderQueued(ordercpDown, PranaInternalConstants.ConnectionStatus.DISCONNECTED);
                        break;
                    case PranaMessageConstants.MSG_CounterPartyUp:
                        //Order order2 = PranaMessageFormatter.FromOrderSingle(message);
                        //OrderQueued(order2, PranaInternalConstants.ConnectionStatus.CONNECTED);
                        break;
                    //// added by vinod

                    case FIXConstants.MSGOrderList:
                        if (BasketReceived != null)
                        {
                            BasketDetail basket = Transformer.CreateBasket(pranaMsg);
                            BasketReceived(basket);
                        }
                        break;
                    case CustomFIXConstants.MsgDropCopyReceived:
                        DropCopyOrder dropCopyOrder = Transformer.CreateDropCopy(pranaMsg);
                        if (InBoxOrderReceived != null)
                        {
                            InBoxOrderReceived(dropCopyOrder);
                        }

                        break;
                    case CustomFIXConstants.MsgDropCopyExecution:
                        DropCopyOrder dropCopyOrder1 = Transformer.CreateDropCopy(pranaMsg);
                        if (OutBoxOrderReceived != null)
                        {
                            OutBoxOrderReceived(dropCopyOrder1);
                        }
                        InBoxOrderReceived(dropCopyOrder1);

                        break;
                    //case CustomFIXConstants.MsgDropCopyReject:
                    //    DropCopyOrder dropCopyOrder2 = Transformer.CreateDropCopy(pranaMsg);
                    //    if (OutBoxOrderReceived != null)
                    //    {
                    //        OutBoxOrderReceived(dropCopyOrder2);
                    //    }
                    //    InBoxOrderReceived(dropCopyOrder2);

                    //    break;
                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                    case FIXConstants.MSGExecutionReport:
                    case FIXConstants.MSGOrderCancelReject:
                    case FIXConstants.MSGTransferUser:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:



                        //if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ReferID) &&
                        //            PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ReferID] != string.Empty)
                        //{
                        //    if (_sentMessages.ContainsKey(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ReferID]))
                        //    {
                        //        _sentMessages.Remove(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ReferID]);
                        //    }
                        //}
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ListID))
                        {
                            if (BasketOrderReceived != null)
                            {
                                Order order = Transformer.CreateOrder(pranaMsg);
                                BasketOrderReceived(order);
                            }
                        }
                        else
                        {
                            if (OrderSingleReceived != null)
                            {
                                OrderSingleReceived(pranaMsg);
                            }
                        }
                        break;

                    default:


                        break;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
              
        
        #region Socket Connection Related Methods
        public PranaInternalConstants.ConnectionStatus  Connect(ConnectionProperties connProperties)
        {
            try
            {
                _socketConn = new SocketConnection();
                _socketConn.MessageReceived += new MessageDelegate(_socketConn_MessageReceived);
                _socketConn.Disconnected += new EventHandler(_socketConn_Disconnected);
                if (_socketConn.Connect(connProperties) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    if(connProperties.User!=null)
                    _socketConn.SendMessage(Transformer.CreateLogOnMessage(connProperties.User).ToString());
                    else
                    _socketConn.SendMessage(Transformer.CreateLogOnMessage(connProperties.IdentifierID, connProperties.IdentifierName).ToString());
                }
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
            }
            return _socketConn.ConnectionStatus;
        }

        void _socketConn_Disconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
                Disconnected(sender, e);
        }

        void _socketConn_MessageReceived(string msg)
        {

            _socketQueue.SendMessage(new PranaMessage(msg));
        }    
                    
             
        
        public void SendMessage(string message)
        {
            _socketConn.SendMessage(message);
        }
     
        #endregion

        #region Properties

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get
            {
                if (_socketConn == null)
                    return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                else
                    return
                    _socketConn.ConnectionStatus;
            }
        }
       
        #endregion

        /// <summary>
        /// Set CounterPartyStatus
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="statusID"></param>
        private   void SetCounterPartyStatus(int counterPartyID,int statusID)
        {
            try
            {
                

                    switch (statusID)
                    {
                        case (int)PranaInternalConstants.ConnectionStatus.CONNECTED:
                            _counterPartiesConnectionStatus[counterPartyID].ConnStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                            break;
                        case (int)PranaInternalConstants.ConnectionStatus.DISCONNECTED:
                            _counterPartiesConnectionStatus[counterPartyID].ConnStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                            break;

                    }
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        /// <summary>
        /// Get the Status of CounterParty
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <returns></returns>
        public PranaInternalConstants.ConnectionStatus GetCounterPartyConnectionSatus(int counterPartyID)
        {
             return    _counterPartiesConnectionStatus[counterPartyID].ConnStatus;
        }
        /// <summary>
        /// Sets Initial Status of CounterParties to DISCONNECTED
        /// </summary>
        /// <param name="counterPartyNameIDS"></param>
        public void SetUserCounterPartyDetails(Dictionary<int, string> counterPartyNameIDS)
        {
            try
            {
                _counterPartiesConnectionStatus.Clear();
                foreach (KeyValuePair<int, string> counterPartyDetail in counterPartyNameIDS)
                {
                    CounterPartyDetails cpdetails = new CounterPartyDetails();
                    cpdetails.ConnStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    cpdetails.CounterPartyID = counterPartyDetail.Key;
                    cpdetails.CounterPartyName = counterPartyDetail.Value;
                    _counterPartiesConnectionStatus.Add(counterPartyDetail.Key, cpdetails);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }
        public void DisConnect()
        {
            try
            {
                _socketConn.DisConnect("User Requested to Disconnect Server");
                
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public event DropCopyOrderRecievedDelegate InBoxOrderReceived;
        public event DropCopyOrderRecievedDelegate OutBoxOrderReceived;

        #region ICommunicationManager Members


        public event MessageDelegate MessageReceived;

        #endregion
    }
}

