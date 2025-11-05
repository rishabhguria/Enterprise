namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AlertType.
    /// </summary>
    public class AlertType
    {
        #region Private and protected members.

        private int _alertTypeID = int.MinValue;
        private string _alertType = string.Empty;

        #endregion

        #region Constructors
        public AlertType()
        {
        }

        public AlertType(int alertTypeID, string alertType)
        {
            _alertTypeID = alertTypeID;
            _alertType = alertType;
        }
        #endregion

        #region Properties

        public int AlertTypeID
        {
            get { return _alertTypeID; }
            set { _alertTypeID = value; }
        }

        public string Type
        {
            get { return _alertType; }
            set { _alertType = value; }
        }

        #endregion
    }
}
