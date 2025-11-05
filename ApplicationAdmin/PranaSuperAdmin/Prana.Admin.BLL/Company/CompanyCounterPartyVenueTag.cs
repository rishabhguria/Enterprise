namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenueTag.
    /// </summary>

    public class CompanyCounterPartyVenueTag
    {
        #region Private Members
        private int _companyCounterPartyVenueTagID = int.MinValue;
        private string _tagType = string.Empty;
        #endregion

        #region Constructors
        public CompanyCounterPartyVenueTag()
        {
        }
        #endregion

        #region Properties
        public int CompanyCounterPartyVenueTagID
        {
            get { return _companyCounterPartyVenueTagID; }
            set { _companyCounterPartyVenueTagID = value; }
        }

        public string TagType
        {
            get { return _tagType; }
            set { _tagType = value; }
        }
        #endregion
    }
}
