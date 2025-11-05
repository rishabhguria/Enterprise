using Prana.Global;
using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterSymbolSearchReq
    {
        private string _startwith;
        private ApplicationConstants.SymbologyCodes _symbology;

        public SecMasterSymbolSearchReq(string startwith, ApplicationConstants.SymbologyCodes symbology)
        {
            _startwith = startwith;
            _symbology = symbology;
        }

        public string StartWith
        {
            get { return _startwith; }
        }

        public ApplicationConstants.SymbologyCodes Symbology
        {
            get { return _symbology; }
        }

        private int _hashCode = int.MinValue;
        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }

        private int _userID = int.MinValue;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}