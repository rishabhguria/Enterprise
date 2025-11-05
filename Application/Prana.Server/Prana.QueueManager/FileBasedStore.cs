using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Prana.QueueManager
{
    public class FileBasedStore : IQueueProcessor, IDisposable
    {
        FileStoreImpl _fileStore = null;
        readonly object locker = new object();
        Timer timer;
        Int64 _dblastSeqNumber = 0;
        //Int64 _incomingSeqNumber;

        #region IQueueProcessor Members

        public FileBasedStore(string path, Int64 lastSeqNumber)
        {
            // _dblastSeqNumber = lastSeqNumber;
            _fileStore = new FileStoreImpl(path + @"/", new Session("ARCA-TEST"), true, 10000);
            _dblastSeqNumber = lastSeqNumber;
            //List<string> messages= new List<string>();
            //_fileStore.get(_dblastSeqNumber, _dblastSeqNumber + 10, messages);

        }

        public void SendMessage(Prana.BusinessObjects.QueueMessage message)
        {
            try
            {
                lock (locker)
                {
                    PranaMessage pranaMsg = (PranaMessage)message.Message;
                    Int64 orderSeqNumber = pranaMsg.FIXMessage.InternalInformation.GetInt64Field(CustomFIXConstants.CUST_TAG_OrderSeqNumber);
                    //_incomingSeqNumber = orderSeqNumber;
                    _fileStore.setNextTargetMsgSeqNum(orderSeqNumber);
                    //if (_dblastSeqNumber == 0)
                    //{
                    //    _dblastSeqNumber = _incomingSeqNumber;
                    //}

                    _fileStore.set(orderSeqNumber, message.ToString());


                    //    listOfMessages.Add(message);
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

        //public event MessageReceivedHandler MessageQueued;
        public event EventHandler<EventArgs<QueueMessage>> MessageQueued;
        public Prana.BusinessObjects.HandlerType HandlerType
        {
            get;
            set;
        }

        public void StartListening()
        {
            timer = new Timer(NotifyObserver, null, 1000, 5000);
        }
        // bool isProcessed = false;

        private void NotifyObserver(object state)
        {
            try
            {
                //if (!isProcessed)
                //{
                // isProcessed = true;
                QueueMessage listMsgforClients = null;
                SortedList<long, string> messages = new SortedList<long, string>();
                int MESSAGE_TO_BE_PROCESSED = 100;
                // stop the timer till this process is complete
                timer.Change(Timeout.Infinite, 0);
                lock (locker)
                {
                    long beginNumber = _dblastSeqNumber;
                    long endNumber = _dblastSeqNumber + MESSAGE_TO_BE_PROCESSED;


                    //if (_incomingSeqNumber < endNumber)
                    //{
                    //    endNumber = _incomingSeqNumber;
                    //}
                    long lastSeqNumber = beginNumber;
                    if (endNumber > beginNumber)
                    {
                        _fileStore.get(beginNumber, endNumber, messages, out lastSeqNumber);
                    }
                    // if incoming seq number is greater and no messages are read it means messages are missing so keep going forward with endnumber



                    Logger.LoggerWrite("beginNumber=" + beginNumber + " endNumber=" + endNumber + " count=" + messages.Count, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                    long incomingSeqNumber = _fileStore.getNextTargetMsgSeqNum();
                    if (messages.Count == 0 && endNumber <= incomingSeqNumber)
                    {
                        _dblastSeqNumber = endNumber + 1;
                    }
                    else if (lastSeqNumber <= incomingSeqNumber)
                    {
                        _dblastSeqNumber = lastSeqNumber + 1;
                    }
                    else
                    {
                        //  _dblastSeqNumber = lastSeqNumber + 1;
                    }
                }


                // process all messages
                if (messages.Count > 0)
                {
                    listMsgforClients = new QueueMessage(CustomFIXConstants.MSG_Grp_Trade, getQueueMsg(messages));

                    if (MessageQueued != null)
                    {
                        MessageQueued(this, new EventArgs<QueueMessage>(listMsgforClients));
                    }
                    // new number will be begin number + count of all messages retrieved

                }


                //}
                //QueueMessage[] copyList = null;
                //lock (listOfMessages)
                //{
                //    copyList = listOfMessages.ToArray();
                //    listOfMessages.Clear();
                //}
                //QueueMessage listMsgforClients = new QueueMessage();
                //listMsgforClients.Message = listMsgforClients;
                //listMsgforClients.MsgType = "BUNCH_OF_MSG";
                //if (MessageQueued != null)
                //{
                //    MessageQueued(this,listMsgforClients);
                //}
            }
            catch (Exception ex)
            {

                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                timer.Change(1000, 5000);
                //isProcessed = false ;
            }

        }
        private List<QueueMessage> getQueueMsg(SortedList<long, string> listStrMsgs)
        {
            List<QueueMessage> listmsgQueue = new List<QueueMessage>();
            foreach (KeyValuePair<long, string> keyValuepair in listStrMsgs)
            {
                listmsgQueue.Add(new QueueMessage(keyValuepair.Value));
            }
            return listmsgQueue;
        }

        public long getLastSeqNumber()
        {
            return _fileStore.getNextTargetMsgSeqNum();
        }
        #endregion

        #region IDisposable Members

        //public void Dispose()
        //{
        //    timer.Change(Timeout.Infinite, 0);

        //}

        /// <summary>
        /// Dispose Internal Channel and remove connections from ConnectionManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    _fileStore.Dispose();
                    timer.Dispose();
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

        #endregion
    }
}
