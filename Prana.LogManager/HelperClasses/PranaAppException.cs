using System;

namespace Prana.LogManager
{
    [Serializable]
    public class PranaAppException
    {
        private string _defaultMessage = "A error occurred in Server. Please contact Nirvana Support!";
        public string DefaultMessage
        {
            get { return _defaultMessage; }
        }

        private string _stackTrace = string.Empty;
        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        private string _message = string.Empty;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public PranaAppException(Exception ex)
        {
            _stackTrace = ex.StackTrace;
            _message = ex.Message;
        }
    }
}

