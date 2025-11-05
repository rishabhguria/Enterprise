namespace Prana.Admin.BLL
{
    public class ThirdPartyCVIdentifier
    {
        private int _thirdPartyCVID = int.MinValue;
        private int _companyThirdPartyID = int.MinValue;
        private int _companyCounterPartyVenueID = int.MinValue;
        private string _cVIdentifier = string.Empty;
        private string _cvName = string.Empty;

        public ThirdPartyCVIdentifier()
        {

        }

        public ThirdPartyCVIdentifier(int thirdPartyCVID, int companyThirdPartyID, int companyCounterPartyVenueID, string cVIdentifier, string cVName)
        {
            _thirdPartyCVID = thirdPartyCVID;
            _companyThirdPartyID = companyThirdPartyID;
            _companyCounterPartyVenueID = companyCounterPartyVenueID;
            _cVIdentifier = cVIdentifier;
            _cvName = cVName;
        }
        public int ThirdPartyCVID
        {
            get { return _thirdPartyCVID; }
            set { _thirdPartyCVID = value; }
        }
        public int CompanyThirdPartyID
        {
            get { return _companyThirdPartyID; }
            set { _companyThirdPartyID = value; }
        }
        public int CompanyCounterPartyVenueID
        {
            get { return _companyCounterPartyVenueID; }
            set { _companyCounterPartyVenueID = value; }
        }
        public string CVIdentifier
        {
            get { return _cVIdentifier; }
            set { _cVIdentifier = value; }
        }

        public string CVName
        {
            get { return _cvName; }
            set { _cvName = value; }
        }
    }
}
