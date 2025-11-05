namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientAccount.
    /// </summary>
    public class ClientAccount
    {
        #region Private members

        int _companyClientAccountID = int.MinValue;
        string _companyClientAccountName = string.Empty;
        string _companyClientAccountShortName = string.Empty;
        int _companyClientID = int.MinValue;

        #endregion

        #region Constructor

        public ClientAccount()
        {
        }
        public ClientAccount(int companyClientAccountID, string companyClientAccountName)
        {
            _companyClientAccountID = companyClientAccountID;
            _companyClientAccountName = companyClientAccountName;
        }
        #endregion

        #region Properties

        public int CompanyClientAccountID
        {
            get
            {
                return _companyClientAccountID;
            }

            set
            {
                _companyClientAccountID = value;
            }
        }


        public string CompanyClientAccountName
        {
            get
            {
                return _companyClientAccountName;
            }

            set
            {
                _companyClientAccountName = value;
            }
        }


        public string CompanyClientAccountShortName
        {
            get
            {
                return _companyClientAccountShortName;
            }

            set
            {
                _companyClientAccountShortName = value;
            }
        }


        public int CompanyClientID
        {
            get
            {
                return _companyClientID;
            }

            set
            {
                _companyClientID = value;
            }
        }

        #endregion
    }
}
