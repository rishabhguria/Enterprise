using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SymbolLookupRequestObject
    {
        private string _underlying;

        public string Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        private string _tickersymbol;

        public string TickerSymbol
        {
            get { return _tickersymbol; }
            set { _tickersymbol = value; }
        }
        private string _bloombergSymbol;

        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }
        private string _factSetSymbol;

        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }
        private string _activSymbol;

        public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }
        private string _isinSymbol;

        public string ISINSymbol
        {
            get { return _isinSymbol; }
            set { _isinSymbol = value; }
        }
        private string _sEDOLSymbol;

        public string SEDOLSymbol
        {
            get { return _sEDOLSymbol; }
            set { _sEDOLSymbol = value; }
        }
        private string _cUSIPSymbol;

        public string CUSIPSymbol
        {
            get { return _cUSIPSymbol; }
            set { _cUSIPSymbol = value; }
        }
        private string _reutersSymbol;

        public string ReutersSymbol
        {
            get { return _reutersSymbol; }
            set { _reutersSymbol = value; }
        }


        private string _osiOptionSymbol;
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol;
        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _opraOptionSymbol;

        public string OPRAOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }


        private int _startIndex;

        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
        private int _endIndex;

        public int EndIndex
        {
            get { return _endIndex; }
            set { _endIndex = value; }
        }

        // Added for search Approved/UnApproved security
        private bool _isSecApproved = true;
        public bool IsSecApproved
        {
            get { return _isSecApproved; }
            set { _isSecApproved = value; }
        }

        // TODO we can add these fields in search criteria for searching security by UDA data like 
        // find security of IT Sector.
        //Added by omshiv,

        #region UDA DATA related fields

        private string _UDAAsset = string.Empty;

        public string UDAAsset
        {
            get { return _UDAAsset; }
            set { _UDAAsset = value; }
        }

        private string _UDASecurityType = string.Empty;

        public string UDASecurityType
        {
            get { return _UDASecurityType; }
            set { _UDASecurityType = value; }
        }

        private string _UDASector = string.Empty;

        public string UDASector
        {
            get { return _UDASector; }
            set { _UDASector = value; }
        }

        private string _UDASubSector = string.Empty;

        public string UDASubSector
        {
            get { return _UDASubSector; }
            set { _UDASubSector = value; }
        }

        private string _UDACountry = string.Empty;

        public string UDACountry
        {
            get { return _UDACountry; }
            set { _UDACountry = value; }
        }
        #endregion

        //Added by omshiv,June 14, to fetch security by BBID
        private String _BBGID = String.Empty;
        public String BBGID
        {
            get { return _BBGID; }
            set { _BBGID = value; }
        }

        //Added by omshiv,SEp 14, for local search or from CSM (Full Scan)
        private bool _isFullScan;
        public bool IsFullScan
        {
            get { return _isFullScan; }
            set { _isFullScan = value; }
        }


    }
}
