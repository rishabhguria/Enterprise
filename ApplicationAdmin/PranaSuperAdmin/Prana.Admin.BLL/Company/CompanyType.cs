namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyType.
    /// </summary>
    public class CompanyType
    {
        #region Private and protected members.

        private int _companyTypeID = int.MinValue;
        private string _companyType = string.Empty;

        #endregion

        public CompanyType()
        {
        }

        public CompanyType(int companyTypeID, string companyType)
        {
            _companyTypeID = companyTypeID;
            _companyType = companyType;
        }

        #region Properties

        public int CompanyTypeID
        {
            get { return _companyTypeID; }
            set { _companyTypeID = value; }
        }

        public string Type
        {
            get { return _companyType; }
            set { _companyType = value; }
        }

        #endregion
    }
}
