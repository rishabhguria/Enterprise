using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace Prana.QueueManager
{
    public class MSMQQueueManager : IQueueProcessor, IDisposable
    {
        PranaBinaryFormatter _binaryFormatter = new PranaBinaryFormatter();
        public delegate void MQError(object sender, EventArgs<string> e);
        public event MQError QueueError;
        public MessageQueue _queue = null;

        public MSMQQueueManager(string queueName)
        {
            try
            {
                if (!MessageQueue.Exists(queueName))
                {
                    MessageQueue.Create(queueName, true);
                }
                _queue = new MessageQueue(queueName);
                _queue.MessageReadPropertyFilter.CorrelationId = true;

                if ("true".ToUpper().Equals(ConfigurationHelper.Instance.GetAppSettingValueByKey("UseJournalMSMQ").ToUpper()))
                {
                    _queue.UseJournalQueue = true;
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
        /// Start Listening to Order Response Queue
        /// </summary>
        public void StartListening()
        {
            try
            {
                _queue.ReceiveCompleted += new ReceiveCompletedEventHandler(RecieveQ_ReceiveCompleted);
                _queue.BeginReceive();
            }
            #region Catch
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                //if (QueueError == null)
                //{
                //    QueueError("Error in starting  listening operation of Queue." + ex.Message);
                //}
            }
            #endregion
        }

        public void StartListeningCashActivityQueue()
        {
            try
            {
                _queue.ReceiveCompleted -= new ReceiveCompletedEventHandler(RecieveQ_ReceiveCompleted);
                _queue.ReceiveCompleted += new ReceiveCompletedEventHandler(RecieveQ_ReceiveCompleted);
                if (IsContainsMessages(_queue))
                {
                    _queue.BeginReceive();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Convert Incoming Message to Cameron Fix Message.
        /// Send Response To Client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecieveQ_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            string messageType = string.Empty;
            bool isCallBeginReceive = false;
            try
            {
                e.Message.Formatter = new BinaryMessageFormatter();
                QueueMessage queueMessage = (QueueMessage)_binaryFormatter.DeSerialize(e.Message.Body.ToString());
                messageType = queueMessage.MsgType;
                if (queueMessage.MsgType.Equals(CustomFIXConstants.MSG_CounterPartyDown))
                {
                    Logger.HandleException(new Exception("CounterParty Down"), LoggingConstants.POLICY_LOGANDSHOW);
                    return;
                }
                if (queueMessage.MsgType.Equals(CustomFIXConstants.MSG_CASHACTIVITY_QUEUE))
                {
                    _queue.EndReceive(e.AsyncResult);
                    MessageQueue queue = (MessageQueue)sender;
                    string queueName = queue.QueueName;
                    string accountID = queueName.Substring(queueName.LastIndexOf('_') + 1);
                    if (!CashManagementRevalState._revalRunningForFundIDsMSMQ.Contains(accountID))
                    {
                        isCallBeginReceive = true;
                    }
                }
                if (MessageQueued != null)
                {
                    MessageQueued(sender, new EventArgs<QueueMessage>(queueMessage));
                }
            }
            #region Catch
            catch (Exception)
            {
                if (QueueError != null)
                {
                    QueueError(this, new EventArgs<string>("Error in receiving from  Queue." + e.Message));
                }
            }
            #endregion
            finally
            {
                if (messageType.Equals(CustomFIXConstants.MSG_CASHACTIVITY_QUEUE))
                {
                    if (isCallBeginReceive && IsContainsMessages(_queue))
                    {
                        _queue.BeginReceive();
                    }
                }
                else
                {
                    _queue.BeginReceive();
                }
            }
        }

        public static bool IsContainsMessages(MessageQueue queue)
        {
            try
            {
                MessageEnumerator enumerator = queue.GetMessageEnumerator2();
                if (enumerator.MoveNext())
                {
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }

        public List<string> GetAllMessages()
        {
            List<string> list = new List<string>();
            try
            {
                Message[] messages = _queue.GetAllMessages();
                foreach (Message msg in messages)
                {
                    msg.Formatter = new BinaryMessageFormatter();
                    QueueMessage queueMessage = (QueueMessage)_binaryFormatter.DeSerialize(msg.Body.ToString());
                    list.Add(queueMessage.Message.ToString());
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
            return list;
        }

        public void Purge()
        {
            _queue.Purge();
        }
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _queue.Close();
                _queue.Dispose();
            }
        }

        #endregion

        #region IQueueProcessor Members


        //public event MessageReceivedHandler MessageQueued;
        public event EventHandler<EventArgs<QueueMessage>> MessageQueued;


        public HandlerType HandlerType
        {
            get; set;
        }
        /// <summary>
        /// Compose Cameron Message
        /// Insert message into the queue
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(QueueMessage sentMsg)
        {
            try
            {
                string serlizedMsg = _binaryFormatter.Serialize(sentMsg);
                System.Messaging.Message qMessage = new System.Messaging.Message(serlizedMsg);
                qMessage.Label = sentMsg.MsgType;
                qMessage.Formatter = new BinaryMessageFormatter();
                try
                {
                    _queue.Send(qMessage, MessageQueueTransactionType.Single);
                }
                catch (Exception e)
                {
                    Logger.HandleException(e, LoggingConstants.POLICY_LOGANDSHOW);
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        public long getLastSeqNumber()
        {
            return long.MinValue;
        }
        #endregion



    }

}
