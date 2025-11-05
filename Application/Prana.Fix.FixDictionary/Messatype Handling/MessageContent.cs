namespace Prana.Fix.FixDictionary
{
    class MessageContent
    {
        private bool _Reqd;

        public bool Reqd
        {
            get { return _Reqd; }
            set { _Reqd = value; }
        }
        private string _tag;

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private int _position;

        /// <summary>
        /// This field contains the position of the tag for given message type. used in sorting data based on position
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }


    }
}
