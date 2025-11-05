namespace Prana.LiveFeed.UI
{
    public class VolatilityData
    {
        private string _callSymbol;
        private string _putSymbol;
        private double _strikePrice;
        private double _callImpVol;
        private double _putImpVol;
        private double _callUserVol;
        private double _putUserVol;

        public string CallSymbol
        {
            get { return _callSymbol; }
            set { _callSymbol = value; }
        }
        public string PutSymbol
        {
            get { return _putSymbol; }
            set { _putSymbol = value; }
        }
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        public double CallImpVol
        {
            get { return _callImpVol; }
            set { _callImpVol = value; }
        }
        public double PutImpVol
        {
            get { return _putImpVol; }
            set { _putImpVol = value; }
        }
        public double CallUserVol
        {
            get { return _callUserVol; }
            set { _callUserVol = value; }
        }
        public double PutUserVol
        {
            get { return _putUserVol; }
            set { _putUserVol = value; }
        }

    }
}
