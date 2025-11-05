namespace Prana.BusinessObjects.Classes.EPNL_Business_Objects
{
    public class PMPrefData
    {
        private bool _useClosingMark = false;

        public bool UseClosingMark
        {
            get { return _useClosingMark; }
            set { _useClosingMark = value; }
        }

        private double _xPercentOfAvgVolume = 100;

        public double XPercentofAvgVolume
        {
            get { return _xPercentOfAvgVolume; }
            set { _xPercentOfAvgVolume = value; }
        }
    }
}
