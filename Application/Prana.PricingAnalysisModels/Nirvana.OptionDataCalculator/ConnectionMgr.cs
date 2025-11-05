//using System;
//using System.Collections.Generic;
//using System.Text;
//using Prana.QueueManager;
//using Prana.BusinessObjects;
//using Prana.OptionCalculator.Common;
//using Prana.SocketCommunication;
//using Prana.Interfaces;

//namespace Prana.OptionCalculator.CalculationComponent
//{
//    public class ConnectionMgr
//    {
//        private static  ConnectionMgr _connectionMgr = null;
//        private static object locker = new object();
//        public static ConnectionMgr GetInstance
//        {
//            get
//            {
//                lock (locker)
//                {
//                    if (_connectionMgr == null)
//                    {
//                        _connectionMgr = new ConnectionMgr();
//                    }
//                    return _connectionMgr;
//                }
//            }
//        }
//        private IQueueProcessor _incomingQueue = null;
//        public void Initlise(IQueueProcessor incomingQueue )
//        {
//            _incomingQueue = incomingQueue;
//            _incomingQueue.MessageQueued += new MessageReceivedHandler(_incomingQueue_MessageQueued);
//            //to start it from Main Thread
//            SubscriberCollection.Initiate();
//        }

//        void _incomingQueue_MessageQueued(object sender, Prana.BusinessObjects.QueueMessage message)
//        {
//            //process message here based on message types
//            try
//            {
//                switch (PranaMessageFormatter.GetMessageType(message.Message.ToString()))
//                {
//                    case OptionDataFormatter.MSGTYPE_SUBSCRIBE_SYMBOLS:
//                        OptionMessageSubscribe optionMessageForSymbol = new OptionMessageSubscribe(message.Message.ToString());
//                        SubscriberCollection.RegisterSymbols(optionMessageForSymbol.UserID, optionMessageForSymbol);
//                        break;

//                    case OptionDataFormatter.MSGTYPE_UNSUBSCRIBE_SYMBOLS:
//                        OptionMessageUnSubscribe optionMsgUnsubs = new OptionMessageUnSubscribe(message.Message.ToString());
//                        SubscriberCollection.UnRegisterSymbols(optionMsgUnsubs.UserID, optionMsgUnsubs.OptionSymbolsList);
//                        break;

//                }
//            }
//            catch (Exception ex)
//            {

//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                throw ex;
//            }
//        }


//    }
//}
