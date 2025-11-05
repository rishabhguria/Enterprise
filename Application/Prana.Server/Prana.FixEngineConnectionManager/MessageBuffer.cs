using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;
namespace Prana.FixEngineConnectionManager
{
    class MessageBuffer
    {
        // private System.Timers.Timer timer = new System.Timers.Timer();
        // private bool isRunning=false;
        private FixEngineConnection _fixConnection = null;
        private Dictionary<string, PranaMessage> _bufferedMessages = new Dictionary<string, PranaMessage>();
        //bool isRunning = false;
        private readonly object locker = new object();
        public MessageBuffer(FixEngineConnection fixConnection)
        {
            try
            {
                _fixConnection = fixConnection;
                // timer.Interval = 5000;
                // timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
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
        private void StartCheckingConnection()
        {

            //timer.Start();
        }
        // private System.Timers.Timer timer = new System.Timers.Timer();
        public void SendBufferedMessages()
        {


            Dictionary<string, PranaMessage> copiedMsgs = new Dictionary<string, PranaMessage>();
            Logger.LoggerWrite("SendBufferedMessages", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
            Logger.LoggerWrite("BufferedMessages count=" + _bufferedMessages.Count.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
            try
            {
                if (_bufferedMessages.Count > 0)
                {
                    foreach (KeyValuePair<string, PranaMessage> keyValuePair in _bufferedMessages)
                    {
                        copiedMsgs.Add(keyValuePair.Key, keyValuePair.Value);
                        Logger.LoggerWrite("copiedMsgs=" + keyValuePair.Value.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
                    }
                    foreach (KeyValuePair<string, PranaMessage> keyValuePair in copiedMsgs)
                    {
                        Logger.LoggerWrite("Sending Buffered Message " + keyValuePair.Value, LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
                        if (_fixConnection.SendMessage(keyValuePair.Value) == string.Empty)
                        {
                            _bufferedMessages.Remove(keyValuePair.Key);
                            Logger.LoggerWrite("Buffered Message sent :" + keyValuePair.Value, LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
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
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (locker)
                {
                    Logger.LoggerWrite("At timer_Elapsed", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);
                    SendBufferedMessages();
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
        public void AddMessages(PranaMessage msg)
        {
            try
            {
                Logger.LoggerWrite("Adding at Buffer inside loop :=" + msg.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG);

                if (msg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID] != null)
                {
                    string clOrderID = msg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    if (clOrderID != string.Empty)
                    {
                        if (!_bufferedMessages.ContainsKey(clOrderID))
                        {
                            _bufferedMessages.Add(clOrderID, msg);
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
    }
}
