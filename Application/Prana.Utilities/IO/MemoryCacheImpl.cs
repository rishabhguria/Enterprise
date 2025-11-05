using System;
using System.Collections.Generic;

namespace Prana.Utilities.IO
{
    class MemoryCacheImpl
    {
        private Dictionary<int, String> messages = new Dictionary<int, String>();
        private long nextSenderMsgSeqNum;
        private long nextTargetMsgSeqNum;
        private DateTime creationTime = DateTime.UtcNow;

        public MemoryCacheImpl()
        {
            reset();
        }



        public DateTime getCreationTime()
        {
            return creationTime;
        }

        /* package */
        //public void setCreationTime(DateTime creationTime)
        //{
        //    this.creationTime = creationTime;
        //}

        public long getNextSenderMsgSeqNum()
        {
            return nextSenderMsgSeqNum;
        }

        public long getNextTargetMsgSeqNum()
        {
            return nextTargetMsgSeqNum;
        }

        public void incrNextSenderMsgSeqNum()
        {
            setNextSenderMsgSeqNum(getNextSenderMsgSeqNum() + 1);
        }

        public void incrNextTargetMsgSeqNum()
        {
            setNextTargetMsgSeqNum(getNextTargetMsgSeqNum() + 1);
        }

        public void reset()
        {
            setNextSenderMsgSeqNum(1);
            setNextTargetMsgSeqNum(1);
            messages.Clear();
            creationTime = DateTime.UtcNow;
        }

        //public void set(int sequence, String message)
        //{
        //    messages.Add(sequence, message);// ? null =false :true;
        //}

        public void setNextSenderMsgSeqNum(long next)
        {
            nextSenderMsgSeqNum = next;
        }

        public void setNextTargetMsgSeqNum(long next)
        {
            nextTargetMsgSeqNum = next;
        }

        //public long getLastSeqNumber()
        //{
        //    return nextTargetMsgSeqNum;
        //}
    }
}
