using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientFix.
    /// </summary>
    public class ClientFix
    {

        #region private members
        private string _SenderCompID;
        private string _OnBehalfOfCompID;
        private string _TargetCompID;
        private string _IP;
        private Int64 _Port;
        private int _IdentifierID;
        private string _Identifier;

        #endregion



        public ClientFix()
        {
            _SenderCompID = string.Empty;
            _OnBehalfOfCompID = string.Empty;
            _TargetCompID = string.Empty;
            _IP = String.Empty;
            _Port = int.MinValue;
            _IdentifierID = int.MinValue;
            _Identifier = String.Empty;
        }

        public ClientFix(string SenderCompID, string OnBehalfOfCompID, string TargetCompID, string IP, int Port, int identifierID, string identifier)
        {

            _SenderCompID = SenderCompID;
            _OnBehalfOfCompID = OnBehalfOfCompID;
            _TargetCompID = TargetCompID;
            _IP = IP;
            _Port = Port;
            _IdentifierID = identifierID;
            _Identifier = identifier;
            //
            // TODO: Add constructor logic here
            //
        }


        #region Properties

        public string SenderCompID
        {
            get { return _SenderCompID; }
            set { _SenderCompID = value; }
        }

        public string OnBehalfOfCompID
        {
            get { return _OnBehalfOfCompID; }
            set { _OnBehalfOfCompID = value; }
        }

        public string TargetCompID
        {
            get { return _TargetCompID; }
            set { _TargetCompID = value; }
        }

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }
        public Int64 Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        public int IdentifierID
        {
            get { return _IdentifierID; }
            set { _IdentifierID = value; }
        }

        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }


        #endregion



    }
}
