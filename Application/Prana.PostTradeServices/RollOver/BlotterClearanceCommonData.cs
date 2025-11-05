using System;

namespace Prana.PostTradeServices.RollOver
{
    public class BlotterClearanceCommonData
    {
        private string _timeZone;
        public string TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }

        private bool _autoClearing = true;
        public bool AutoClearing
        {
            get { return _autoClearing; }
            set { _autoClearing = value; }
        }

        private DateTime _baseTime;
        public DateTime BaseTime
        {
            get { return _baseTime; }
            set { _baseTime = value; }
        }

        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private int _rolloverPermittedUserID;
        public int RolloverPermittedUserID
        {
            get { return _rolloverPermittedUserID; }
            set { _rolloverPermittedUserID = value; }
        }
    }
}