using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;

namespace Prana.PostTrade
{
    public class PostTradeServerManager
    {
        static IQueueProcessor _queueCommMgrOut = null;
        static PostTradeServerManager _postTradeServerManager = null;
        static int _hashCode = int.MinValue;
        static PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static PostTradeServerManager()
        {
            _postTradeServerManager = new PostTradeServerManager();
        }
        public static PostTradeServerManager GetInstance
        {
            get
            {
                return _postTradeServerManager;
            }
        }

        public void Initilise(IQueueProcessor queueCommMgrIn, IQueueProcessor queueCommMgrOut)
        {
            try
            {
                _queueCommMgrOut = queueCommMgrOut;
                _hashCode = this.GetHashCode();
                queueCommMgrIn.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(queueCommMgrIn_MessageQueued);
                //new MessageReceivedHandler(queueCommMgrIn_MessageQueued);

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


        public void SendDataToClient(object data)
        {
            try
            {
                string request = binaryFormatter.Serialize(data);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_RESPONSE, request);
                _queueCommMgrOut.SendMessage(qMsg);
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

        void queueCommMgrIn_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                switch (e.Value.MsgType)
                {
                    default:
                        break;
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
    }
}
