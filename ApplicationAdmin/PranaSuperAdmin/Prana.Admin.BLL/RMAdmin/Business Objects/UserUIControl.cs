namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for UserUIControl.
    /// </summary>
    public class UserUIControl
    {
        #region Private methods

        private int _rMCompanyUserUIID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private int _companyUserAUECID = int.MinValue;
        private int _ticketSize = int.MinValue;
        private int _priceDeviation = int.MinValue;
        private int _allowUsertoOverwrite = int.MinValue;
        private string _allowUser = string.Empty;
        private int _notifyUserWhenLiveFeedsAreDown = int.MinValue;
        private string _notifyUser = string.Empty;
        private string _shortName = string.Empty;
        private string _auec = string.Empty;

        #endregion

        #region Contructors
        public UserUIControl()
        {

        }
        #endregion

        #region Properties
        public int RMCompanyUserUIID
        {
            get { return _rMCompanyUserUIID; }
            set { _rMCompanyUserUIID = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public int CompanyUserAUECID
        {
            get { return _companyUserAUECID; }
            set { _companyUserAUECID = value; }
        }
        public int TicketSize
        {
            get { return _ticketSize; }
            set { _ticketSize = value; }
        }
        public int PriceDeviation
        {
            get { return _priceDeviation; }
            set { _priceDeviation = value; }
        }
        public int AllowUsertoOverwrite
        {
            get { return _allowUsertoOverwrite; }
            set { _allowUsertoOverwrite = value; }
        }
        public int NotifyUserWhenLiveFeedsAreDown
        {
            get { return _notifyUserWhenLiveFeedsAreDown; }
            set { _notifyUserWhenLiveFeedsAreDown = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string AllowUsertoOverWriteTrueFalse
        {

            get
            {
                if (_allowUsertoOverwrite == 0)
                {
                    _allowUser = "No";
                }
                else
                {
                    _allowUser = "Yes";
                }
                return _allowUser;
            }
        }

        public string NotifyUserTrueFalse
        {

            get
            {

                if (_notifyUserWhenLiveFeedsAreDown == 0)
                {
                    _notifyUser = "No";
                }
                else
                {
                    _notifyUser = "Yes";
                }
                return _notifyUser;
            }
        }

        public string AUEC
        {
            get { return _auec; }
            set { _auec = value; }
        }
        #endregion
    }
}
