using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using System;

namespace Prana.FixAdapters
{
    class FixMessageCreator
    {
        public static PranaMessage createResendMsg(Int64 startSeqNumber)
        {
            PranaMessage msg = new PranaMessage();

            msg.MessageType = FIXConstants.MSGResendRequest;
            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGResendRequest);
            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagBeginSeqNo, startSeqNumber.ToString());
            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagEndSeqNo, "0");
            msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSendingTime, DateTimeConstants.GetCurrentTimeInFixFormat());

            return msg;
        }
    }
}
