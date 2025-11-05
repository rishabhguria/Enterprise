using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClearanceData : EventArgs, IKeyable
    {
        private string _auec = string.Empty;
        public string AUEC
        {
            get { return _auec; }
            set { _auec = value; }
        }

        private int _auecID = int.MinValue;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private int _companyAUECID = int.MinValue;
        public int CompanyAUECID
        {
            get { return _companyAUECID; }
            set { _companyAUECID = value; }
        }

        private string _AUECIDStr = string.Empty;
        public string AUECIDStr
        {
            get { return _AUECIDStr; }
            set { _AUECIDStr = value; }
        }

        private string _exchangeFullName = string.Empty;
        public string ExchangeFullName
        {
            get { return _exchangeFullName; }
            set { _exchangeFullName = value; }
        }

        private string _exchangeDisplayName = string.Empty;
        public string ExchangeDisplayName
        {
            get { return _exchangeDisplayName; }
            set { _exchangeDisplayName = value; }
        }

        private string _exchangeRegularTradingStartTime = string.Empty;
        public string ExchangeRegularTradingStartTime
        {
            get { return _exchangeRegularTradingStartTime; }
            set { _exchangeRegularTradingStartTime = value; }
        }

        private string _exchangeRegularTradingEndTime = string.Empty;
        public string ExchangeRegularTradingEndTime
        {
            get { return _exchangeRegularTradingEndTime; }
            set { _exchangeRegularTradingEndTime = value; }
        }

        private DateTime _clearanceTime = DateTimeConstants.MinValue;
        public DateTime ClearanceTime
        {
            get { return _clearanceTime; }
            set { _clearanceTime = value; }
        }

        private int _clearanceTimeID = int.MinValue;
        public int ClearanceTimeID
        {
            get { return _clearanceTimeID; }
            set { _clearanceTimeID = value; }
        }

        private string _exchangeIdentifier;
        public string ExchangeIdentifier
        {
            get { return _exchangeIdentifier; }
            set { _exchangeIdentifier = value; }
        }

        private bool _permitRollover;
        public bool PermitRollover
        {
            get { return _permitRollover; }
            set { _permitRollover = value; }
        }

        #region IKeyable Members
        public string GetKey()
        {
            return _clearanceTime.ToString();
        }

        public void Update(IKeyable item)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}