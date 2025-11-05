using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;

namespace Prana.FixEngineConnectionManager
{
    public class FixEngineConnection : IDisposable
    {
        #region Events and Delegates
        public event EventHandler<EventArgs<FixPartyDetails>> ConnectionStatusUpdate;
        public event EventHandler<EventArgs<PranaMessage, FixEngineConnection>> MessageReceived;
        private IFixEngineAdapter _fixConnAdapter = null;
        #endregion

        #region Private Variables
        private SortedList _incomingSortedMessages = SortedList.Synchronized(new SortedList());
        private bool _underTroubleShooting = false;
        private Int64 _msgSeqNumToBeReceived = Int64.MinValue;
        private bool _resendRequestSent = false;
        private object lockAdapter = new object();
        private IQueueProcessor _cpsentQueue;
        private IQueueProcessor _cpReceivedQueue;
        private bool _loggingEnabled = false;
        FixPartyDetails _fixPartyDetails;
        private bool isDisposed = false;
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        #endregion

        #region Constructors
        public FixEngineConnection(IQueueProcessor cpReceivedQueue, IQueueProcessor cpsentQueue, FixPartyDetails fIXPartyDetails, bool isTroubleShootmode)
        {
            _fixPartyDetails = fIXPartyDetails;
            _loggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
            _underTroubleShooting = isTroubleShootmode;
            _cpReceivedQueue = cpReceivedQueue;
            _cpsentQueue = cpsentQueue;
        }
        #endregion

        #region Dispose Methods
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.isDisposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    try
                    {
                        if (_fixConnAdapter != null)
                            _fixConnAdapter.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.LoggerWrite("Unable to terminate connection: " + ex.ToString() + ex.StackTrace);
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }
            }
            isDisposed = true;
        }
        #endregion

        #region Connection To Universal Server
        /// <summary>
        /// Connects to Cameron server with given settings.
        /// </summary>
        public void Connect(IFixEngineAdapter fixConnAdapter)
        {
            try
            {
                _fixConnAdapter = fixConnAdapter;
                _fixConnAdapter.SetUp(_cpReceivedQueue, _cpsentQueue);
                _fixConnAdapter.FixConnectionEvent += new EventHandler<EventArgs<FixPartyDetails>>(_fixConnAdapter_ConnectionEvent);
                _fixConnAdapter.MessageReceivedEvent += new EventHandler<EventArgs<PranaMessage>>(_fixConnAdapter_MessageEvent);
                _fixConnAdapter.Connect(_fixPartyDetails);
                if (_underTroubleShooting)
                {
                    if (_fixPartyDetails.LastSeqNumberRecevied != Int64.MinValue)
                    {
                    }
                    else
                    {
                        _underTroubleShooting = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception ex1 = new Exception("For CounterParty : " + _fixPartyDetails.PartyName + " Exception =" + ex.Message);
                Logger.HandleException(ex1, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void Disconnect()
        {
            if (_fixConnAdapter != null)
            {
                _fixConnAdapter.Disconnect();
            }
        }

        public void ConnectToSamePort(FixEngineConnection samePortConnection)
        {
            if (_fixConnAdapter != null)
            {
                _fixPartyDetails.SetConnectionValues(samePortConnection.FixPartyDetails);
                _fixConnAdapter.FixConnectionEvent += new EventHandler<EventArgs<FixPartyDetails>>(_fixConnAdapter_ConnectionEvent);
            }
            _fixConnAdapter = samePortConnection.FixConnAdapter;
        }

        void _fixConnAdapter_MessageEvent(object sender, EventArgs<Prana.BusinessObjects.FIX.PranaMessage> e)
        {
            try
            {
                PranaMessage pranaMsg = e.Value;
                if (_underTroubleShooting)
                {
                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMsgSeqNum))
                    {
                        Int64 msgSeqNumber = 0;
                        Int64.TryParse(pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagMsgSeqNum].Value, out msgSeqNumber);
                        AbnormalMessageSequenceNumberReceived(pranaMsg, msgSeqNumber);
                    }
                }
                else
                {
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow3] Before MessageReceived(_fixConnAdapter_MessageEvent) Event Raise In FixEngineConnection, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                        }
                    }
                    MessageReceived(this, new EventArgs<PranaMessage, FixEngineConnection>(pranaMsg, this));
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow3] After MessageReceived(_fixConnAdapter_MessageEvent) Event Raised In FixEngineConnection, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                        }
                    }
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

        void _fixConnAdapter_ConnectionEvent(object sender, EventArgs<FixPartyDetails> e)
        {
            if (ConnectionStatusUpdate != null)
                ConnectionStatusUpdate(this, new EventArgs<FixPartyDetails>(_fixPartyDetails));
        }
        #endregion

        #region Sending To Univarsal Server
        /// <summary>
        ///  send Message to the server. returns false if delivery is unsucessfull
        /// </summary>
        /// <param name="orderRequest"></param>
        public string SendMessage(PranaMessage pranaMsg)
        {
            string result = string.Empty;
            try
            {
                if (this.FixPartyDetails.BuySideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED && this.FixPartyDetails.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow Out4] SendMessage(SendMessage) In FixEngineConnection, Fix Message: " + Convert.ToString(pranaMsg.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                        }
                    }
                    _fixConnAdapter.SendMessage(pranaMsg);
                    result = string.Empty;
                }
                else
                {
                    result = PranaInternalConstants.ConnectionStatus.DISCONNECTED.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptinoHandlingLocal("SendMessage", pranaMsg.ToString());
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                result = "Problem While Sending Message to CounterParty";
            }
            return result;
        }
        #endregion

        #region  Message and Exception Processing
        /// <summary>
        /// Processing when Normal Sequence Number is Received
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgSeqNumber"></param>
        private void NormalMessageSeqNumberReceived()
        {
            try
            {
                if (_underTroubleShooting)
                {
                    int count = _incomingSortedMessages.Count;
                    foreach (DictionaryEntry de in _incomingSortedMessages)
                    {
                        PranaMessage msg = (PranaMessage)de.Value;
                        Logger.HandleException(new Exception("Message Dequeued,SeqNumber : " + de.Key.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                        MessageReceived(this, new EventArgs<PranaMessage, FixEngineConnection>(msg, this));
                    }
                    _underTroubleShooting = false;
                    _resendRequestSent = false;
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
        /// Processing When Abnormal Message Sequence Number Received
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgSeqNumber"></param>
        private void AbnormalMessageSequenceNumberReceived(PranaMessage pranaMsg, Int64 msgSeqNumber)
        {
            try
            {
                // Enqueue Orders
                if (!_incomingSortedMessages.ContainsKey(msgSeqNumber))
                {
                    _incomingSortedMessages.Add(msgSeqNumber, pranaMsg);
                    Logger.HandleException(new Exception("Message Enquequed, SeqNumber : " + msgSeqNumber), LoggingConstants.POLICY_LOGANDSHOW);

                }

                if (_resendRequestSent && _msgSeqNumToBeReceived == msgSeqNumber)
                {
                    NormalMessageSeqNumberReceived();
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

        public void ResendRequest(Int64 beginSeqNumber, Int64 endSeqNumber, string conunterPartyName)
        {
            try
            {
                PranaMessage pranaMsg = new PranaMessage();
                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGResendRequest);
                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagBeginSeqNo, beginSeqNumber.ToString());
                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagEndSeqNo, endSeqNumber.ToString());

                if (_incomingSortedMessages.Count > 0)
                {
                    pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagEndSeqNo].Value = _incomingSortedMessages.GetKey(0).ToString();
                    endSeqNumber = Int64.Parse(pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagEndSeqNo].Value);
                    _msgSeqNumToBeReceived = endSeqNumber;
                }
                else
                {
                    pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagEndSeqNo].Value = "0";
                }
                SendMessage(pranaMsg);
                _resendRequestSent = true;
                if (endSeqNumber == 0)
                {
                    NormalMessageSeqNumberReceived();
                }

                Logger.HandleException(new Exception("Resend Request to : " + conunterPartyName + " BeginSeqNumber " + beginSeqNumber + " EndSeqNumber:" + endSeqNumber), LoggingConstants.POLICY_LOGANDSHOW);
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
        #endregion

        private void UpdateCounterPartyStatusToAllSubscriber()
        {
            try
            {
                if (ConnectionStatusUpdate != null)
                    ConnectionStatusUpdate(this, new EventArgs<FixPartyDetails>(_fixPartyDetails));
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

        private void ExceptinoHandlingLocal(string methodName, string msgToLog)
        {
            string error = "Exception in Message:=" + msgToLog + " at " + methodName;
            Logger.LoggerWrite(msgToLog, LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
            Logger.HandleException(new Exception(error), LoggingConstants.POLICY_LOGANDSHOW);
        }

        #region Properties
        public FixPartyDetails FixPartyDetails
        {
            get { return _fixPartyDetails; }
        }
        public IFixEngineAdapter FixConnAdapter
        {
            get { return _fixConnAdapter; }
        }
        #endregion

        internal void ReProcessMsg(PranaMessage pranaMsg)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new EventArgs<PranaMessage, FixEngineConnection>(pranaMsg, this));
            }
        }
    }
}