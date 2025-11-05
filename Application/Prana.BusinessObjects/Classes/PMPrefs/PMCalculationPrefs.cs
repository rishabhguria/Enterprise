namespace Prana.BusinessObjects
{
    public class PMCalculationPrefs
    {
        private double _highWaterMark;
        public double HighWaterMark
        {
            get { return _highWaterMark; }
            set { _highWaterMark = value; }
        }

        private double _stopOut;
        public double StopOut
        {
            get { return _stopOut; }
            set { _stopOut = value; }
        }

        private double _traderPayoutPercent;
        public double TraderPayoutPercent
        {
            get { return _traderPayoutPercent; }
            set { _traderPayoutPercent = value; }
        }
    }
}