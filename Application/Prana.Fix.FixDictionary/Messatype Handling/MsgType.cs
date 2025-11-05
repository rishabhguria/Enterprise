namespace Prana.Fix.FixDictionary
{
    class MessgeType
    {
        private string _msgType;

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
        private string _msgID;

        public string MsgID
        {
            get { return _msgID; }
            set { _msgID = value; }
        }
        private MessageContentList _msgContentList = new MessageContentList();

        public MessageContentList MsgContentList
        {
            get { return _msgContentList; }
            set { _msgContentList = value; }
        }

    }
}
