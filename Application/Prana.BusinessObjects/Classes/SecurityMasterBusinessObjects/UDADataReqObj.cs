using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class UDADataReqObj
    {

        private Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType _viewSymbol;
        public Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType ViewSymbol
        {
            get { return _viewSymbol; }
            set { _viewSymbol = value; }
        }

        private string _userID;
        public string CompanyUserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private string _requestID = string.Empty;

        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }

        private int _hashCode;

        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }

    }
}
