namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCVGiveUpIdentifier.
    /// </summary>
    public class CompanyCVGiveUpIdentifier
    {
        #region Private members

        private int _companyCounterPartyVenueID = int.MinValue;

        private int _companyCVGiveUpIdentifierID = int.MinValue;

        private string _giveUpIdentifier = string.Empty;

        #endregion
        public CompanyCVGiveUpIdentifier()
        {

        }

        public CompanyCVGiveUpIdentifier(int companyCVGiveUpIdentifierID, string giveUpIdentifier)
        {
            _companyCVGiveUpIdentifierID = companyCVGiveUpIdentifierID;
            _giveUpIdentifier = giveUpIdentifier;

        }

        #region Public Members

        public int CompanyCounterPartyVenueId
        {
            get { return _companyCounterPartyVenueID; }
            set { _companyCounterPartyVenueID = value; }
        }

        public int CompanyCVGiveUpIdentifierID
        {
            get { return _companyCVGiveUpIdentifierID; }
            set { _companyCVGiveUpIdentifierID = value; }
        }

        public string GiveUpIdentifier
        {
            get { return _giveUpIdentifier; }
            set { _giveUpIdentifier = value; }
        }



        #endregion

    }
}
