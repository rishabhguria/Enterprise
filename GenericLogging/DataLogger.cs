using System;
using System.IO;
using System.Messaging;
using System.Text;
using System.Threading;
using GenericLogging.ApplicationConstants;
using GenericLogging.Utility;
using Prana.BusinessObjects;
using System.Timers;

namespace GenericLogging
{
    internal class DataLogger
    {

        private MessageQueue _myQueue;
        private readonly ManualResetEvent _signal = new ManualResetEvent(false);
        private Message[] _myMessage;
        private string _loggingpath;
        private System.Timers.Timer _timer;
        private readonly string _queueName = String.Empty;
        private void SetFilePath()
        {
            if (!Directory.Exists(ApplicationConstant.StartupPath + ApplicationConstant.FolderName))
            {
                Directory.CreateDirectory(ApplicationConstant.StartupPath + ApplicationConstant.FolderName);
            }
            _loggingpath = (ApplicationConstant.StartupPath + ApplicationConstant.FolderName + @"\" + _queueName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt");
        }

        public DataLogger()
        {
            
        }

        public DataLogger(string queuename)
        {
            _queueName = queuename;
        }
        private void CreateQueue(object sender, ElapsedEventArgs e)
        {
            if (MessageQueue.Exists(ApplicationConstant.QueuePath + _queueName))
            {
                _myQueue = new MessageQueue(ApplicationConstant.QueuePath + _queueName)
                {
                    Formatter = new XmlMessageFormatter(new[] { typeof(TaxLot) })
                };

                _myQueue.ReceiveCompleted += DataReceivedAfterInterval;

                _myQueue.BeginReceive();
                _timer.Stop();
                _signal.WaitOne();
            }
            else
            {
                _signal.WaitOne();
            }
        }

        private void DataReceivedAfterInterval(Object source, ReceiveCompletedEventArgs asyncResult)
        {

            _myMessage = _myQueue.GetAllMessages();

            var mq = (MessageQueue)source;

            var m = mq.EndReceive(asyncResult.AsyncResult);
            var taxlot = (TaxLot)m.Body;

            if (File.Exists(_loggingpath))
            {
                var fi = new FileInfo(_loggingpath);
                if (fi.Length >= 2000000)
                {
                    SetFilePath();
                    var sw = new StreamWriter(_loggingpath);
                    var sb = new StringBuilder();
                    DataWriter.WriteDataToFile(sw, sb, taxlot);
                }
                else
                {
                    var sw = new StreamWriter(_loggingpath, true);
                    var sb = new StringBuilder();
                    DataWriter.WriteDataToFile(sw, sb, taxlot);
                }
            }
            else
            {
                SetFilePath();
                var sw = new StreamWriter(_loggingpath);
                var sb = new StringBuilder();
                DataWriter.WriteDataToFile(sw, sb, taxlot);
            }

            Console.WriteLine(ApplicationConstant.MessageInQueue + _myMessage.Length);
            if (_myMessage.Length.Equals(0))
            {
                Console.WriteLine(ApplicationConstant.MessageConsumed);
            }
            mq.BeginReceive();

        }

        internal void BeginWork()
        {
            _timer = new System.Timers.Timer { Interval = ApplicationConstant.QueueReadTimer };
            _timer.Elapsed += CreateQueue;
            _timer.Start();
            FileCompressor.CompressOldFiles();
            SetFilePath();
            CreateQueue(null, null);
        }
    }
}
