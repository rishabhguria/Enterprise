using System;

namespace Prana.BusinessObjects.Classes.ThirdParty
{
    [Serializable]
    public class ThirdPartyToleranceProfile : ThirdPartyToleranceProfileCommon
    {
        private int _ToleranceProfileId;
        private DateTime _LastModified;
        private int _MatchingField = 1;
        private decimal _AvgPrice = 0;
        private decimal _NetMoney = 0;
        private decimal _Commission = 0;
        private decimal _MiscFees = 0;

        public int ToleranceProfileId
        {
            get { return _ToleranceProfileId; }
            set { _ToleranceProfileId = value; }
        }

        public DateTime LastModified
        {
            get { return _LastModified; }
            set { _LastModified = value; }
        }

        public int MatchingField
        {
            get { return _MatchingField; }
            set { _MatchingField = value; }
        }

        public decimal AvgPrice
        {
            get { return _AvgPrice; }
            set { _AvgPrice = value; }
        }

        public decimal NetMoney
        {
            get { return _NetMoney; }
            set { _NetMoney = value; }
        }

        public decimal Commission
        {
            get { return _Commission; }
            set { _Commission = value; }
        }

        public decimal MiscFees
        {
            get { return _MiscFees; }
            set { _MiscFees = value; }
        }
    }
}
