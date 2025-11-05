namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Identifier.
    /// </summary>
    public class Identifier
    {
        #region Private members

        private int _identifierID = int.MinValue;
        private string _identifierName = string.Empty;
        private string _clientIdentifierName = string.Empty;
        private string _primaryKey = string.Empty;

        #endregion

        public Identifier()
        {
        }

        public Identifier(int identifierID, string identifierName)
        {
            _identifierID = identifierID;
            _identifierName = identifierName;
        }
        public Identifier(int identifierID, string identifierName, string clientIdentifierName)
        {
            _identifierID = identifierID;
            _identifierName = identifierName;
            _clientIdentifierName = clientIdentifierName;
        }
        public bool Equal(Identifier identifier)
        {
            if (identifier.IdentifierID == _identifierID
                && identifier.IdentifierName == _identifierName
                && identifier.ClientIdentifierName == _clientIdentifierName

                )
                return true;
            else
                return false;
        }

        #region Properties

        public int IdentifierID
        {
            get { return _identifierID; }
            set { _identifierID = value; }
        }

        public string IdentifierName
        {
            get { return _identifierName; }
            set { _identifierName = value; }
        }
        public string ClientIdentifierName
        {
            get { return _clientIdentifierName; }
            set { _clientIdentifierName = value; }

        }
        public string PrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        #endregion


    }
}
