namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ClearingFirmPrimeBroker.
    /// </summary>
    public class ClearingFirmPrimeBroker
    {
        #region Private members

        private int _clearingFirmsPrimeBrokersID = int.MinValue;
        private string _clearingFirmsPrimeBrokersName = string.Empty;
        private string _clearingFirmsPrimeBrokersShortName = string.Empty;
        private int _companyID = int.MinValue;

        #endregion

        #region Constructors
        public ClearingFirmPrimeBroker()
        {
        }

        public ClearingFirmPrimeBroker(int clearingFirmsPrimeBrokersID, string clearingFirmsPrimeBrokersName)
        {
            _clearingFirmsPrimeBrokersID = clearingFirmsPrimeBrokersID;
            _clearingFirmsPrimeBrokersName = clearingFirmsPrimeBrokersName;
        }
        #endregion

        #region Properties

        public int ClearingFirmsPrimeBrokersID
        {
            get { return _clearingFirmsPrimeBrokersID; }
            set { _clearingFirmsPrimeBrokersID = value; }
        }

        public string ClearingFirmsPrimeBrokersName
        {
            get { return _clearingFirmsPrimeBrokersName; }
            set { _clearingFirmsPrimeBrokersName = value; }
        }

        public string ClearingFirmsPrimeBrokersShortName
        {
            get { return _clearingFirmsPrimeBrokersShortName; }
            set { _clearingFirmsPrimeBrokersShortName = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        #endregion
    }
}
