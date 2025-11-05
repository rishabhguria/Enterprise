namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FileFormat.
    /// </summary>
    public class FileFormat
    {
        #region Private Members
        private int _accountID = int.MinValue;
        private int _primeBrokerClearerID = int.MinValue;
        private int _custodianID = int.MinValue;
        private int _adminstratorID = int.MinValue;
        #endregion
        public FileFormat()
        {
        }
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public int PrimeBrokerClearerID
        {
            get { return _primeBrokerClearerID; }
            set { _primeBrokerClearerID = value; }
        }

        public int CustodianID
        {
            get { return _custodianID; }
            set { _custodianID = value; }
        }

        public int AdminstratorID
        {
            get { return _adminstratorID; }
            set { _adminstratorID = value; }
        }
    }
}
