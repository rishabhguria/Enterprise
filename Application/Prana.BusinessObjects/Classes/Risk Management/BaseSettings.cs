using System;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class BaseSettings
    {
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private bool _success = true;
        [XmlIgnore]
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        private string _supoortMailID;

        public string SupoortMailID
        {
            get { return _supoortMailID; }
            set { _supoortMailID = value; }
        }

        private string _supportMailPassword;

        public string SupportMailPassword
        {
            get { return _supportMailPassword; }
            set { _supportMailPassword = value; }
        }
        private string _hostID;

        public string HostID
        {
            get { return _hostID; }
            set { _hostID = value; }
        }

        private string _mailSubject;

        public string MailSubject
        {
            get { return _mailSubject; }
            set { _mailSubject = value; }
        }
        private string _mailBody;

        public string MailBody
        {
            get { return _mailBody; }
            set { _mailBody = value; }
        }

        private string _cronExpression;

        public string CronExpression
        {
            get { return _cronExpression; }
            set { _cronExpression = value; }
        }



    }
}
