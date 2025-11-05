namespace Prana.Utilities.IO
{
    public class Session
    {
        public Session(string sessionID)
        {
            _sessionID = sessionID;
        }

        private string _sessionID;

        public string SessionID
        {
            get { return _sessionID; }
            set { _sessionID = value; }
        }

    }
}
