using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterSymbolSearchRes
    {
        private string _startwith;
        private ApplicationConstants.SymbologyCodes _symbology;
        private IList<string> _result;

        public SecMasterSymbolSearchRes(string startwith, ApplicationConstants.SymbologyCodes symbology, IList<string> result)
        {
            _startwith = startwith;
            _symbology = symbology;
            _result = result;
        }

        public IList<string> Result
        {
            get { return _result; }
        }

        public string StartWith
        {
            get { return _startwith; }
        }

        public ApplicationConstants.SymbologyCodes Symbology
        {
            get { return _symbology; }
        }

        private int _hashCode;
        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }
        private int _userID;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
