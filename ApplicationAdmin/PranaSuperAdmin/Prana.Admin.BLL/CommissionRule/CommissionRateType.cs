namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRateType.
    /// </summary>
    public class CommissionRateType
    {
        #region Private Members
        private int _commissionRateID = int.MinValue;
        private string _commissionRateTypeName = string.Empty;
        #endregion
        public CommissionRateType()
        {
        }
        public CommissionRateType(int commissionRateID, string commissionRateTypeName)
        {
            _commissionRateID = commissionRateID;
            _commissionRateTypeName = commissionRateTypeName;
        }
        #region Public Properties
        public int CommissionRateID
        {
            get { return _commissionRateID; }
            set { _commissionRateID = value; }
        }
        public string CommissionRateTypeName
        {
            get { return _commissionRateTypeName; }
            set { _commissionRateTypeName = value; }
        }
        #endregion
    }
}
