using System;

namespace Prana.BusinessObjects.EventArguments
{
    public class PostTradeServicesErrorEventArgs : EventArgs
    {
        String _pranaMsgType;
        public String PranaMsgType { get { return _pranaMsgType; } }

        String _userID;
        public String UserId { get { return _userID; } }

        String _reqID;
        public String ReqId { get { return _reqID; } }

        public PostTradeServicesErrorEventArgs(String pranaMsgType, String userID, String reqID)
        {
            this._pranaMsgType = pranaMsgType;
            this._reqID = reqID;
            this._userID = userID;
        }
    }
}
