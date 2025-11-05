using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Prana.QueueManager;
using Prana.BusinessObjects;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.SocketCommunication;
using System.Collections.Specialized;
using Prana.Interfaces;

namespace Prana.LiveFeedEngine
{
    public class ServiceManager
    {

        static ServiceManager _serviceManager = null;
        
        IQueueProcessor _inQueueClientBroadCasting = null;
        IQueueProcessor _outQueueClientMarkPriceBroadCasting = null;

        int _chunkSize = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("MessageChunkSize"));

        LiveFeedSubscriber _liveFeedSubscriber = null;

        ServerCustomCommunicationManager _clientBroadCastingManager = null;
        public ServerCustomCommunicationManager ClientBroadCastingManager
        {
            get { return _clientBroadCastingManager; }
            set { _clientBroadCastingManager = value; }
        }

        public event EventHandler SMPricingServerDisconnected;

        /// <summary>
        /// Singleton instance of ServiceManager
        /// </summary>
        /// <returns></returns>
        public static ServiceManager GetInstance()
        {
            if (_serviceManager == null)
            {
                _serviceManager = new ServiceManager();
            }
            return _serviceManager;
        }

        /// <summary>
        /// Starting point of ExposureAndPnl Calculator Service. It fetches Forex Data, and invokes OrderFillManager and CalculationService!
        /// </summary>
        public bool Start()
        {
            bool isStarted = false;

            try
            {
                //Client BroadCasting Related Connection
                _inQueueClientBroadCasting = new QueueProcessor(this);
                _inQueueClientBroadCasting.HandlerType = HandlerType.MarkPriceHandler;
                _inQueueClientBroadCasting.MessageQueued += new MessageReceivedHandler(_inQueueClientBroadCasting_MessageQueued);
                _outQueueClientMarkPriceBroadCasting = new QueueProcessor(this);
                _outQueueClientMarkPriceBroadCasting.HandlerType = HandlerType.MarkPriceHandler;
                List<string> tradingAccounts = Prana.CommonDataCache.ClientsCommonDataManager.GetAllTradingAccounts();

                _clientBroadCastingManager = ServerCustomCommunicationManager.GetInstance();
                _clientBroadCastingManager.Initialise(_inQueueClientBroadCasting,_outQueueClientMarkPriceBroadCasting, tradingAccounts, "Live Feed Engine");
                
                _liveFeedSubscriber = LiveFeedSubscriber.GetInstance();
                _liveFeedSubscriber.Start();
                _liveFeedSubscriber.SendLevel1DataList += new Level1DataListHandler(_liveFeedSubscriber_SendLevel1DataList);
                isStarted = true;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                isStarted = false;
                if (rethrow)
                {
                    throw;
                }

            }
            return isStarted;
        }

        void _inQueueClientBroadCasting_MessageQueued(object sender, QueueMessage message)
        {
            try
            {
                string msg = message.Message.ToString();
                List<string> msgList = new List<string>();
                PranaMessageFormatter.FromLiveFeedSnapshotRequestMsg(msg,out msgList);
                _liveFeedSubscriber.GetLiveFeedSnapshotForSymbols(msgList);
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
        /// Stops the service.
        /// </summary>
        public void Stop()
        {
            try
            {
               // _clientBroadCastingManager.GetLiveFeedSnapshot -= new StringListHandler(_clientBroadCastingManager_GetLiveFeedSnapshot);
                _liveFeedSubscriber.Stop();

                if (_clientBroadCastingManager != null)
                {
                    _clientBroadCastingManager.DisConnectAllClients();
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
        }

        void _liveFeedSubscriber_SendLevel1DataList(List<Level1Data> l1DataList)
        {
            foreach (string userID in _clientBroadCastingManager.ConnectedUsers)
            {
                int iUserId = int.MinValue;
                int.TryParse(userID, out iUserId);
                SendLiveFeedResponseToClient(l1DataList, iUserId);
            }
        }

        void _clientBroadCastingManager_GetLiveFeedSnapshot(List<string> strList)
        {
            _liveFeedSubscriber.GetLiveFeedSnapshotForSymbols(strList);
        }


        /// <summary>
        /// Sends the orders to client.
        /// </summary>
        /// <param name="ordersCollection">The orders collection.</param>
        private void SendLiveFeedResponseToClient(List<Level1Data> l1DataList, int userID)
        {
            ///created message chunks
            string[] messages = PranaMessageFormatter.CreateLiveFeedSnapshotResponseChunk(l1DataList, _chunkSize);

            foreach (string message in messages)
            {
                QueueMessage queueMessage = new QueueMessage(PranaMessageConstants.MSG_GetLiveFeedSnapShot,"",userID.ToString(), message);//, _pmTradingAccount);

                _outQueueClientMarkPriceBroadCasting.SendMessage(queueMessage);
            }

        }


       

        public bool RestartLiveFeeds()
        {
            return LiveFeedSubscriber.GetInstance().RestartLiveFeeds();
        }


    }
}
