using System;

namespace Prana.LogManager
{
    public class InformationReporter
    {
        public delegate void InformationReceivedHandler(object sender, LoggingEventArgs<string> e);
        public event InformationReceivedHandler InformationReceived;
        private static InformationReporter _informationReporter = null;
        private static readonly object _locker = new object();

        private InformationReporter()
        {
        }

        public static InformationReporter GetInstance
        {
            get
            {
                lock (_locker)
                {
                    if (_informationReporter == null)
                    {
                        _informationReporter = new InformationReporter();
                    }
                }
                return _informationReporter;
            }
        }

        public void Write(string message)
        {
            if (InformationReceived != null)
            {
                string messageWithTime = DateTime.UtcNow.ToString("M/d/yyyy hh:mm:ss tt") + " : " + message;
                InformationReceived(this, new LoggingEventArgs<string>(messageWithTime));

                // Logging all messages displayed on Information Reporter
                Logger.LoggerWrite(messageWithTime, LoggingConstants.CATEGORY_INFORMATION_REPORTER);
            }
        }
        public void ComplianceLogWrite(string message)
        {
            if (InformationReceived != null)
            {
                string messageWithTime = DateTime.UtcNow.ToString("M/d/yyyy hh:mm:ss tt") + " : " + message;
                InformationReceived(this, new LoggingEventArgs<string>(messageWithTime));

                // Logging all compliance related messages displayed on Information Reporter
                Logger.LoggerWrite(messageWithTime, LoggingConstants.CATEGORY_INFORMATION_REPORTER_COMPLIANCE);
            }
        }
                
    }
}