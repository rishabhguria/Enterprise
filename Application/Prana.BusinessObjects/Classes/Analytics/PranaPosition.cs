using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaPosition : PranaBasicMessage
    {
        public PranaPosition()
        {
        }

        public double AbsQuantity
        {
            get { return Math.Abs(_quantity); }
        }

        protected double _multiplier = 1;
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        // adding new as derived contains an initial value for field
        new protected double _delta = 1;
        new public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        protected string _account;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        protected string _strategy;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        protected string _UDAAsset;
        public string UDAAsset
        {
            get { return _UDAAsset; }
            set { _UDAAsset = value; }
        }

        protected string _securityTypeName;
        public string SecurityTypeName
        {
            get { return _securityTypeName; }
            set { _securityTypeName = value; }
        }

        protected string _sectorName;
        public string SectorName
        {
            get { return _sectorName; }
            set { _sectorName = value; }
        }

        protected string _subSectorName;
        public string SubSectorName
        {
            get { return _subSectorName; }
            set { _subSectorName = value; }
        }

        protected string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        protected string _securityName;
        public string SecurityName
        {
            get { return _securityName; }
            set { _securityName = value; }
        }

        protected int _sideMultiplier = 1;
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        protected string _positionType;
        public string PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        // commenting as base and derived have same defination, derived has no initial value
        //protected char _putOrCall = ' ';
        //public char PutOrCall
        //{
        //    get { return _putOrCall; }
        //    set { _putOrCall = value; }
        //}

        //protected double _strikePrice;
        //public double StrikePrice
        //{
        //    get { return _strikePrice; }
        //    set { _strikePrice = value; }
        //}
    }
}
