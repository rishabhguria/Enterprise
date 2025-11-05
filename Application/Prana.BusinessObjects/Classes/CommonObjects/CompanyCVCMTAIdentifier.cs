namespace Prana.BusinessObjects
{

    /// <summary>
    /// Summary description for CompanyCVGiveUpIdentifier.
    /// </summary>
    public class CompanyCVCMTAIdentifier
    {

        #region Private members

        private int _companyCounterPartyVenueID = int.MinValue;

        private int _companyCVCMTAIdentifierID = int.MinValue;

        private string _cMTAIdentifier = string.Empty;

        #endregion
        public CompanyCVCMTAIdentifier()
        {

        }
        public CompanyCVCMTAIdentifier(int companyCounterPartyVenueID, string cMTAIdentifier)
        {
            _companyCounterPartyVenueID = companyCounterPartyVenueID;
            _cMTAIdentifier = cMTAIdentifier;
        }

        #region Public Members

        public int CompanyCounterPartyVenueId
        {
            get { return _companyCounterPartyVenueID; }
            set { _companyCounterPartyVenueID = value; }
        }

        public int CompanyCVCMTAIdentifierID
        {
            get { return _companyCVCMTAIdentifierID; }
            set { _companyCVCMTAIdentifierID = value; }
        }

        public string CMTAIdentifier
        {
            get { return _cMTAIdentifier; }
            set { _cMTAIdentifier = value; }
        }

        #endregion


    }
}
