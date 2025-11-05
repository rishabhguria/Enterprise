using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientOverallLimit.
    /// </summary>
    public class ClientOverallLimit
    {
        #region Private methods

        private int _companyClientRMID = int.MinValue;
        private int _clientID = int.MinValue;
        private Int64 _clientExposureLimit = Int64.MinValue;
        private int _companyID = int.MinValue;
        private string _clientName = string.Empty;

        #endregion

        #region Constructors
        public ClientOverallLimit()
        {

        }
        #endregion


        #region Properties
        public int CompanyClientRMID
        {
            get { return _companyClientRMID; }
            set { _companyClientRMID = value; }
        }
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }
        public Int64 ClientExposureLimit
        {
            get { return _clientExposureLimit; }
            set { _clientExposureLimit = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public string ClientName
        {
            get { return this._clientName; }
            set { this._clientName = value; }
        }
        #endregion

    }
}
