using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class MailDeliveryParameters
    {
        public MailDeliveryParameters(string hostID, string senderMailID, string senderPassword, string receiverMailID, string mailSubject, string mailBody, List<string> fileToBeAttachedWithFullPath)
        {
            _hostID = hostID;
            _senderMailID = senderMailID;
            _senderPassword = senderPassword;
            _receiverMailID = receiverMailID;
            _mailSubject = mailSubject;
            _mailBody = mailBody;
            _fileToBeAttachedWithFullPath = fileToBeAttachedWithFullPath;
        }

        private string _hostID;
        /// <summary>
        ///  host:
        ///   A System.String that contains the name or IP address of the host computer
        ///    used for SMTP transactions.
        /// like smtp.gmail.com
        /// </summary>
        public string HostID
        {
            get { return _hostID; }
            set { _hostID = value; }
        }

        private string _senderMailID;

        public string SenderMailID
        {
            get { return _senderMailID; }
            set { _senderMailID = value; }
        }

        private string _senderPassword;

        public string SenderPassword
        {
            get { return _senderPassword; }
            set { _senderPassword = value; }
        }

        private string _receiverMailID;

        public string ReceiverMailID
        {
            get { return _receiverMailID; }
            set { _receiverMailID = value; }
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

        private List<string> _fileToBeAttachedWithFullPath;

        public List<string> FileToBeAttachedWithFullPath
        {
            get { return _fileToBeAttachedWithFullPath; }
            set { _fileToBeAttachedWithFullPath = value; }
        }
        List<string> _Attachments = new List<string>();
        public List<string> Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }


        }




    }
}
