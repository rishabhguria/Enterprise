namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenueDetails.
    /// </summary>
    public class CompanyCounterPartyVenueDetail
    {

        #region Private members

        private int _companyCounterPartyID = int.MinValue;
        private int _companyCounterPartyVenueDetailsID = int.MinValue;
        private int _companyCounterPartyVenueIdentifierID = int.MinValue;
        private int _companyCounterPartyVenueID = int.MinValue;
        private int _clearingFirmPrimeBrokerID = int.MinValue;
        private string _deliverToCompanyID = string.Empty;
        private string _senderCompanyID = string.Empty;
        private int _accountID = int.MinValue;
        private int _strategyID = int.MinValue;
        private int _cMTAGiveUp = int.MinValue;
        private string _onBehalfOfSubID = string.Empty;
        private string _targetCompID = string.Empty;

        private int _mpid = int.MinValue;
        private string _clearingFirm = string.Empty;
        //private string _identifierName = string.Empty;

        private string _companyCounterPartyVenueName = string.Empty;
        private string _counterPartyFullName = string.Empty;
        private string _clearingFirmPrimeBroker = string.Empty;
        private string _accountName = string.Empty;
        private string _strategyName = string.Empty;
        private string _cMTAGiveUpName = string.Empty;

        private string _mpidName = string.Empty;

        private string _cmtaIdentifier = string.Empty;
        private string _giveUpIdentifier = string.Empty;
        #endregion

        #region Constructors
        public CompanyCounterPartyVenueDetail()
        {
        }

        public CompanyCounterPartyVenueDetail(int companyCounterPartyID)
        {
            _companyCounterPartyID = companyCounterPartyID;
        }

        public CompanyCounterPartyVenueDetail(int companyCounterPartyVenueDetailsID, string deliverToCompanyID, string senderCompanyID, string targetCompID, string onBehalfOfSubID, string clearingFirm, string identifierName, string counterPartyFullName, string companyCounterPartyVenueName, string clearingFirmPrimeBroker, string accountName, string strategyName, string mPIDName)
        {
            _companyCounterPartyVenueDetailsID = companyCounterPartyVenueDetailsID;
            _deliverToCompanyID = deliverToCompanyID;
            _senderCompanyID = senderCompanyID;
            _targetCompID = targetCompID;
            _onBehalfOfSubID = onBehalfOfSubID;
            _clearingFirm = clearingFirm;
            //_identifierName = identifierName;
            _cMTAGiveUpName = identifierName;

            _counterPartyFullName = counterPartyFullName;
            _companyCounterPartyVenueName = companyCounterPartyVenueName;
            _clearingFirmPrimeBroker = clearingFirmPrimeBroker;
            _accountName = accountName;
            _strategyName = strategyName;
            _mpidName = mPIDName;

        }
        #endregion

        #region Properties

        public int CompanyCounterPartyVenueDetailsID
        {
            get { return _companyCounterPartyVenueDetailsID; }
            set { _companyCounterPartyVenueDetailsID = value; }
        }

        public int CompanyCounterPartyVenueIdentifierID
        {
            get { return _companyCounterPartyVenueIdentifierID; }
            set { _companyCounterPartyVenueIdentifierID = value; }
        }

        public int CompanyCounterPartyID
        {
            get { return _companyCounterPartyID; }
            set { _companyCounterPartyID = value; }
        }

        public int CompanyCounterPartyVenueID
        {
            get { return _companyCounterPartyVenueID; }
            set { _companyCounterPartyVenueID = value; }
        }

        public int ClearingFirmPrimeBrokerID
        {
            get { return _clearingFirmPrimeBrokerID; }
            set { _clearingFirmPrimeBrokerID = value; }
        }

        public string DeliverToCompanyID
        {
            get { return _deliverToCompanyID; }
            set { _deliverToCompanyID = value; }
        }

        public string SenderCompanyID
        {
            get { return _senderCompanyID; }
            set { _senderCompanyID = value; }
        }

        public string TargetCompID
        {
            get { return _targetCompID; }
            set { _targetCompID = value; }
        }

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        public int CMTAGiveUp
        {
            get { return _cMTAGiveUp; }
            set { _cMTAGiveUp = value; }
        }

        public string OnBehalfOfSubID
        {
            get { return _onBehalfOfSubID; }
            set { _onBehalfOfSubID = value; }
        }


        public int MPID
        {
            get { return _mpid; }
            set { _mpid = value; }
        }
        public string ClearingFirm
        {
            get { return _clearingFirm; }
            set { _clearingFirm = value; }
        }
        //public string IdentifierName
        //{
        //    get{return _identifierName;}
        //    set{_identifierName = value;}
        //}

        public string CounterPartyFullName
        {
            get
            {
                CounterParty counterParty = CounterPartyManager.GetCounterParty(_companyCounterPartyID);
                if (counterParty == null)
                {
                    return "";
                }
                else
                {
                    return (counterParty.CounterPartyFullName);
                }
            }
            set { _counterPartyFullName = value; }
        }


        public string CompanyCounterPartyVenueName
        {
            get
            {
                CompanyCounterPartyVenue companyCounterPartyVenue = CounterPartyManager.GetCompanyCounterPartyVenue(_companyCounterPartyVenueID);
                if (companyCounterPartyVenue == null)
                {
                    return "";
                }
                else
                {
                    return (companyCounterPartyVenue.CounterPartyVenueDisplayName);
                }
            }
            set { _companyCounterPartyVenueName = value; }
        }

        public string ClearingFirmPrimeBroker
        {
            get
            {
                ClearingFirmPrimeBroker clearingFirmPrimeBroker = CompanyManager.GetsClearingFirmPrimeBroker(_clearingFirmPrimeBrokerID);
                if (clearingFirmPrimeBroker == null)
                {
                    return "";
                }
                else
                {
                    return (clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName);
                }
            }
            set { _clearingFirmPrimeBroker = value; }
        }

        public string AccountName
        {
            get
            {
                Account account = CompanyManager.GetsAccount(_accountID);
                if (account == null)
                {
                    return "";
                }
                else
                {
                    return (account.AccountName);
                }
            }
            set { _accountName = value; }
        }

        public string StrategyName
        {
            get
            {
                Strategy strategy = CompanyManager.GetsStrategy(_strategyID);
                if (strategy == null)
                {
                    return "";
                }
                else
                {
                    return (strategy.StrategyName);
                }
            }
            set { _strategyName = value; }
        }

        public string CMTAGiveUPName
        {
            get
            {
                Identifier identifier = AUECManager.GetIdentifier(_cMTAGiveUp);
                if (identifier == null)
                {
                    return "";
                }
                else
                {
                    return (identifier.IdentifierName);
                }
            }
            set { _cMTAGiveUpName = value; }
        }

        public string MPIDName
        {
            get
            {
                MPID mpid = CompanyManager.GetCompanyMPID(_mpid);
                if (mpid == null)
                {
                    return "";
                }
                else
                {
                    return (mpid.MPIDName);
                }
            }
            set { _mpidName = value; }
        }

        public string CMTAIdentifier
        {
            get { return _cmtaIdentifier; }
            set { _cmtaIdentifier = value; }
        }
        public string GiveUpIdentifier
        {
            get { return _giveUpIdentifier; }
            set { _giveUpIdentifier = value; }
        }

        #endregion
    }
}
