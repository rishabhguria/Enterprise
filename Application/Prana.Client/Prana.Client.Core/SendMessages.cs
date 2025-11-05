using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects.FIX;

namespace Prana.Client.Core
{
    public class SentMessages
    {
        private string _referenceID;

        public string ReferenceID
        {
            get { return _referenceID; }
            set { _referenceID = value; }
        }

        private DateTime _sentTime;

        public DateTime SentTime
        {
            get { return _sentTime; }
            set { _sentTime = value; }
        }

        private PranaMessage _sentMessage;

        public PranaMessage SentMessage
        {
            get { return _sentMessage; }
            set { _sentMessage = value; }
        }

        private string _sentDisplayMessage;

        public string SentDisplayMessage
        {
            get { return _sentDisplayMessage; }
            set { _sentDisplayMessage = value; }
        }

        StringBuilder str = new StringBuilder();
        public SentMessages(string refID, DateTime sentTime, PranaMessage sentMessage)
        {
            
            _referenceID = refID;
            _sentTime = sentTime;
            _sentMessage = sentMessage;
            //str.Append(sentMessage.FIXMessage.ExternalInformation[Prana.BusinessObjects.FIXConstants.TagSymbol])
            //.Append(sentMessage.FIXMessage.ExternalInformation[Prana.BusinessObjects.FIXConstants.TagSide])
            //.Append(sentMessage.FIXMessage.ExternalInformation[Prana.BusinessObjects.FIXConstants.TagOrdType])
            //.Append(sentMessage.FIXMessage.ExternalInformation[Prana.BusinessObjects.FIXConstants.TagPrice]);
            //_sentDisplayMessage = str.ToString();
        }

        public SentMessages(string refID, DateTime sentTime)
        {
            _referenceID = refID;
            _sentTime = sentTime;
        }
    }
}
