using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class MessageEventArgs : EventArgs
    {
        private string title;
        private string statusFormattedMessage;
        private string logFile;
        private string description;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string StatusFormattedMessage
        {
            get { return statusFormattedMessage; }
            set { statusFormattedMessage = value; }
        }

        public string LogFile
        {
            get { return logFile; }
            set { logFile = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public MessageEventArgs() { }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
        public MessageEventArgs(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public MessageEventArgs(string message, string title, string description, string logFile)
        {
            this.message = message;
            this.title = title;
            this.description = description;
            this.logFile = logFile;
        }
    }
}
