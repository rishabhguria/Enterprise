using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class StatusEventArgs : EventArgs
    {
        private string text;
        private string logFile;
        private string description;
        private string statusFormattedMessage;

        public string Text
        {
            get { return text; }
            set { text = value; }
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

        public string StatusFormattedMessage
        {
            get { return statusFormattedMessage; }
            set { statusFormattedMessage = value; }
        }

        public StatusEventArgs(string text)
        {
            Text = text;
        }

        public StatusEventArgs(string text, string logFile, string description)
        {
            this.text = text;
            this.logFile = logFile;
            this.description = description;
        }

        public StatusEventArgs() { }

    }
}
