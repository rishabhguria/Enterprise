using AdapterClient;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Prana.FixAdapters
{

    public class QuickFixAdapter : IFixEngineAdapter
    {
        private TcpClient tc;
        bool _loggingEnabled = false;
        internal IAdapterClient client = null;
        FixPartyDetails _fixPartyDetails;
        public event EventHandler<EventArgs<FixPartyDetails>> FixConnectionEvent;
        public event EventHandler<EventArgs<PranaMessage>> MessageReceivedEvent;
        private IQueueProcessor _cpsentQueue;
        private IQueueProcessor _cpReceivedQueue;

        #region IFixEngineAdapter Members

        public void SetUp(IQueueProcessor cpReceivedQueue, IQueueProcessor cpsentQueue)
        {
            _loggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
            _cpReceivedQueue = cpReceivedQueue;
            _cpsentQueue = cpsentQueue;
        }

        public void SendMessage(Prana.BusinessObjects.FIX.PranaMessage pranaMsg)
        {
            try
            {

                //pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, tempclorderid.ToString());
                //Message message = CreateFixMessage(pranaMsg);
                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTargetCompID, _fixPartyDetails.TargetCompID);

                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSenderCompID, _fixPartyDetails.SenderCompID);
                String msg = pranaMsg.getFixMessage();
                // int length = msg.Length;
                //if (_loggingEnabled)
                //{
                //    QueueMessage qMsg = new QueueMessage("String", "", "", message.ToString());
                //    _cpsentQueue.SendMessage(qMsg);
                //}
                //String msgExcludingcheckSum = "8=FIX.4.2" + Seperators.DELIMITER + "9=" + length + Seperators.DELIMITER + msg ;
                //int checkSum = msg.Length % 256;
                //String completeFixMsg = msg;// +"10=" + checkSum + Seperators.DELIMITER;
                //completeFixMsg = "8=FIX.4.29=13235=D49=BANZAI52=20100610-10:37:33.45556=EXEC11=127616625346121=138=90040=154=155=goog59=060=20100610-10:37:33.455";
                NetworkStream ns = tc.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                if (tc.Connected)
                {
                    sw.WriteLine(msg + "\n");
                    sw.Flush();
                    //EnterpriseLibraryManager.HandleException(new Exception("sending msg=" + completeFixMsg), EnterpriseLibraryConstants.POLICY_LOGANDSHOW);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        public void ReProcessMessage(Prana.BusinessObjects.FIX.PranaMessage pranaMsg)
        {

        }

        public void Connect(Prana.BusinessObjects.FixPartyDetails fixPartyDetails)
        {
            try
            {
                _fixPartyDetails = fixPartyDetails;
                Thread t2 = new Thread(ConnectToServer);
                t2.Start();
            }
            catch (Exception ex)
            {
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        public void Disconnect()
        {

            try
            {
                if (tc != null)
                {
                    tc.Close();
                    while (tc.Connected)
                    {
                        Thread.Sleep(1000);
                    }
                    tc = null;
                    _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    if (FixConnectionEvent != null)
                    {
                        FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                    }
                }

            }
            catch (Exception ex)
            {
                tc = null;
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing && tc.Connected)
                tc.Close();
        }
        public Prana.BusinessObjects.FIX.PranaMessage CreatePranaMessageFromFixMessage(string message)
        {
            return null;
        }



        #endregion

        public void HandleLogOnMessage()
        {
            try
            {
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
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
        public void HandleLogOutMessage()
        {
            _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
            if (FixConnectionEvent != null)
            {
                FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
            }
        }

        private void ConnectToServer()
        {
            try
            {

                while (true)
                {
                    Thread.Sleep(5000);

                    if (tc == null || !tc.Connected)
                    {

                        tc = new TcpClient(_fixPartyDetails.HostName, _fixPartyDetails.Port);// in the place of server, enter
                        if (tc.Connected)
                        {
                            _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                            if (FixConnectionEvent != null)
                            {
                                FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));

                            }
                            Int64 seqNumber = 0;
                            if (_fixPartyDetails.LastSeqNumberRecevied != Int64.MinValue)
                            {

                                seqNumber = _fixPartyDetails.LastSeqNumberRecevied + 1;
                            }

                            SendMessage(FixMessageCreator.createResendMsg(seqNumber));
                            Thread t1 = new Thread(StartReaderWriter);
                            t1.Start();
                            break;
                        }
                        else
                        {

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSOCKET);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void StartReaderWriter()
        {
            try
            {
                NetworkStream ns = tc.GetStream();
                StreamReader sr = new StreamReader(ns);
                while (tc.Connected)
                {
                    String msg = sr.ReadLine();
                    if (msg != null && !msg.Trim().Equals(String.Empty))
                    {
                        PranaMessage pranaMsg = new PranaMessage();
                        pranaMsg.createFromFixMessage(msg);

                        if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMsgSeqNum))
                        {
                            _fixPartyDetails.LastSeqNumberRecevied = Int64.Parse(pranaMsg.FIXMessage.ExternalInformation.GetField(FIXConstants.TagMsgSeqNum));
                        }

                        if (pranaMsg.MessageType == FIXConstants.MSGLogon)
                        {
                            HandleLogOnMessage();
                        }
                        else if (pranaMsg.MessageType == FIXConstants.MSGLogout)
                        {
                            HandleLogOutMessage();
                        }
                        else if (MessageReceivedEvent != null)
                        {
                            MessageReceivedEvent(null, new EventArgs<PranaMessage>(pranaMsg));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                tc = null;
                _fixPartyDetails.BuySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                _fixPartyDetails.BuyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                if (FixConnectionEvent != null)
                {
                    FixConnectionEvent(null, new EventArgs<FixPartyDetails>(_fixPartyDetails));
                }
            }
        }
    }
}
