namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for MPID.
    /// </summary>
    public class MPID
    {
        #region Private members

        private int _companyMPID = int.MinValue;
        private int _companyID = int.MinValue;
        private string _MPID = string.Empty;

        #endregion

        #region Constructors
        public MPID()
        {
        }
        public MPID(string MPID)
        {
        }
        public MPID(int companyMPID, int companyID, string MPID)
        {
            _companyMPID = companyMPID;
            _companyID = companyID;
            _MPID = MPID;
        }
        #endregion

        #region Properties

        public int CompanyMPID
        {
            get { return _companyMPID; }
            set { _companyMPID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public string MPIDName
        {
            get { return _MPID; }
            set { _MPID = value; }
        }
        #endregion

    }
}
