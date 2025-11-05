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

namespace Prana.Client.Core
{
    //Public delegates	


    //Custom EventArgs

    public class CommunicationManager : ICommunicationManager
    {
        #region Private Variables
        private SocketConnection _socketConn = null;
        private IQueueProcessor _socketQueue;
        private Dictionary<int, CounterPartyDetails> _counterPartiesConnectionStatus = new Dictionary<int, CounterPartyDetails>();
        #region Events
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event MessageDelegate MessageReceived;
        public event QueuedDelegate OrderQueued;
        public event CounterPartyConnectHandler CounterPartyStatusUpdate;
        #endregion
        #endregion

        public CommunicationManager()
        {
            _socketQueue = new QueueProcessor(this);
            _socketQueue.MessageQueued += new MessageReceivedHandler(_socketQueue_MessageQueued);
        }

        void _socketQueue_MessageQueued(object sender, QueueMessage msg)
        {
            try
            {
                string message = msg.Message.ToString();
                
                        if (MessageReceived != null)
                        {

                            if (MessageReceived != null)
                            {
                                MessageReceived(message);
                            }
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
        

        void _socketConn_MessageReceived(string msg)
        {
            try
            {
                QueueMessage queueMessage = new QueueMessage(string.Empty, msg, int.MinValue);
                _socketQueue.SendMessage(queueMessage);
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
        void _socketConn_Disconnected(object sender, EventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(this, null);
            }
        }
        #region Properties

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get {
                if (_socketConn == null)
                    return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                else 
                return
                _socketConn.ConnectionStatus ; }
        }

        #endregion

        /// <summary>
        /// Set CounterPartyStatus
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="statusID"></param>
        private void SetCounterPartyStatus(int counterPartyID, int statusID)
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
            return _counterPartiesConnectionStatus[counterPartyID].ConnStatus;
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

        public event OrderSingleMessageDelegate OrderSingleReceived;
        public event BasketOrderMessageDelegate BasketOrderReceived;
        public event BasketMessageDelegate BasketReceived;
        public event DropCopyOrderRecievedDelegate InBoxOrderReceived;
        public event DropCopyOrderRecievedDelegate OutBoxOrderReceived;
       


        #region ICommunicationManager Members

        public PranaInternalConstants.ConnectionStatus Connect(ConnectionProperties connProperties)
        {
            _socketConn = new SocketConnection();
            _socketConn.MessageReceived += new MessageDelegate(_socketConn_MessageReceived);
            _socketConn.Disconnected += new EventHandler(_socketConn_Disconnected);

            if (_socketConn.Connect(connProperties) == PranaInternalConstants.ConnectionStatus.CONNECTED )
            {
                if (connProperties.User != null)
                {
                    connProperties.IdentifierID = connProperties.User.CompanyUserID.ToString();
                    connProperties.IdentifierName = connProperties.User.FirstName + " " + connProperties.User.LastName;
                }
                _socketConn.SendMessage(PranaMessageFormatter.CreateLogOnMessage(connProperties.IdentifierID, connProperties.IdentifierName).ToString());
            }

            return _socketConn.ConnectionStatus;
        }

       

        
        public void SendMessage(Prana.BusinessObjects.FIX.PranaMessage message)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void SendMessage(string message)
        {
            _socketConn.SendMessage(message);
        }
        public void DisConnect()
        {
            _socketConn.DisConnect("User Requested to Disconnect Server");
        }
        #endregion



        
    }
}

