using Prana.Global;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdParty.
    /// </summary>
    [Serializable]
    public class ThirdParty
    {
        private int _thirdPartyID = int.MinValue;
        private string _thirdPartyName = string.Empty;
        private string _description = string.Empty;
        private int _companyID = int.MinValue;

        private int _thirdPartyTypeID = int.MinValue;
        private string _shortName = string.Empty;
        private string _contactPerson = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _cellPhone = string.Empty;
        private string _workTelephone = string.Empty;
        private string _fax = string.Empty;
        private string _email = string.Empty;

        private string _thirdPartyTypeName = string.Empty;

        private int _companyThirdPartyID = int.MinValue;

        private int _thirdPartyCVID = int.MinValue;
        private int _companyCounterPartyVenueID = int.MinValue;
        private string _cvIdentifier = string.Empty;

        private int _countryID = int.MinValue;
        private int _stateID = int.MinValue;
        private string _zip = string.Empty;

        private string _primaryContactLastName = string.Empty;
        private string _primaryContactTitle = string.Empty;
        private string _primaryContactWorkTelephone = string.Empty;
        private string _primaryContactFax = string.Empty;
        private string _brokerCode = string.Empty;

        private int _counterPartyID = int.MinValue;
        //private string _PranaToThirdParty = string.Empty;
        //private string _thirdPartyToPrana = string.Empty;

        public ThirdParty()
        {
        }

        public ThirdParty(int thirdPartyID, string thirdPartyName)
        {
            _thirdPartyID = thirdPartyID;
            _thirdPartyName = thirdPartyName;
        }

        public int ThirdPartyID
        {
            get
            {
                return _thirdPartyID;
            }

            set
            {
                _thirdPartyID = value;
            }
        }


        public string ThirdPartyName
        {
            get
            {
                return _thirdPartyName;
            }

            set
            {
                _thirdPartyName = value;
            }
        }


        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }


        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }
        public string ContactPerson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; }
        }
        public int ThirdPartyTypeID
        {
            get
            {
                return _thirdPartyTypeID;
            }
            set { _thirdPartyTypeID = value; }
        }
        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }
        public string CellPhone
        {
            get { return _cellPhone; }
            set { _cellPhone = value; }
        }
        public string WorkTelephone
        {
            get { return _workTelephone; }
            set { _workTelephone = value; }
        }
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string ThirdPartyTypeName
        {
            get
            {
                return _thirdPartyTypeName;
            }
            set { _thirdPartyTypeName = value; }
        }

        public int CompanyThirdPartyID
        {
            get { return _companyThirdPartyID; }
            set { _companyThirdPartyID = value; }
        }

        public int ThirdPartyCVID
        {
            get { return _thirdPartyCVID; }
            set { _thirdPartyCVID = value; }
        }

        public int CompanyCounterPartyVenueID
        {
            get { return _companyCounterPartyVenueID; }
            set { _companyCounterPartyVenueID = value; }
        }

        public string CVIdentifier
        {
            get { return _cvIdentifier; }
            set { _cvIdentifier = value; }
        }

        public int CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }

        public int StateID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }

        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        public string PrimaryContactLastName
        {
            get { return _primaryContactLastName; }
            set { _primaryContactLastName = value; }
        }

        public string PrimaryContactTitle
        {
            get { return _primaryContactTitle; }
            set { _primaryContactTitle = value; }
        }

        public string PrimaryContactWorkTelephone
        {
            get { return _primaryContactWorkTelephone; }
            set { _primaryContactWorkTelephone = value; }
        }

        public string PrimaryContactFax
        {
            get { return _primaryContactFax; }
            set { _primaryContactFax = value; }
        }

        private int _symbolConvention = int.MinValue;

        public int SymbolConvention
        {
            get { return _symbolConvention; }
            set { _symbolConvention = value; }
        }

        private ApplicationConstants.SymbologyCodes _securityIdentifierType;
        public ApplicationConstants.SymbologyCodes SecurityIdentifierType
        {
            get { return _securityIdentifierType; }
            set { _securityIdentifierType = value; }
        }

        public string BrokerCode
        {
            get { return _brokerCode; }
            set { _brokerCode = value; }
        }

        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        //string _delimiter = string.Empty;
        //public string Delimiter
        //{
        //    get { return _delimiter; }
        //    set { _delimiter = value; }
        //}

        //string _delimitorName = string.Empty;
        //public string DelimiterName
        //{
        //    get { return _delimitorName; }
        //    set { _delimitorName = value; }
        //}

        //private string  _fileExtension=string.Empty ;

        //public string  FileExtension
        //{
        //    get { return _fileExtension; }
        //    set { _fileExtension = value; }
        //}


    }
}
