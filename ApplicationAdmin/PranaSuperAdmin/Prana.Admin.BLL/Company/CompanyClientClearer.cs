using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientClearer.
    /// </summary>
    public class CompanyClientClearer
    {

        #region Private members

        private int _CompanyClientID = int.MinValue;
        private string _CompanyClientClearerName = string.Empty;
        private string _CompanyClientClearerShortName = string.Empty;
        private string _ClearingFirmBrokerID = String.Empty;

        #endregion

        #region Constructors
        public CompanyClientClearer()
        {
            _CompanyClientID = int.MinValue;
            _CompanyClientClearerName = String.Empty;
            _CompanyClientClearerShortName = String.Empty;
            _ClearingFirmBrokerID = String.Empty;
        }
        public CompanyClientClearer(int CompanyClientID, string CompanyClientClearerName, string CompanyClientClearerShortName, string ClearingFirmBrokerID)
        {
            _CompanyClientID = CompanyClientID;
            _CompanyClientClearerName = CompanyClientClearerName;
            _CompanyClientClearerShortName = CompanyClientClearerShortName;
            _ClearingFirmBrokerID = ClearingFirmBrokerID;

        }

        #endregion






        #region Properties

        public int CompanyClientID
        {
            get { return _CompanyClientID; }
            set { _CompanyClientID = value; }
        }

        public string CompanyClientClearerName
        {
            get { return _CompanyClientClearerName; }
            set { _CompanyClientClearerName = value; }
        }

        public string CompanyClientClearerShortName
        {
            get { return _CompanyClientClearerShortName; }
            set { _CompanyClientClearerShortName = value; }
        }

        public string ClearingFirmBrokerID
        {
            get { return _ClearingFirmBrokerID; }
            set { _ClearingFirmBrokerID = value; }
        }
        #endregion





    }
}
