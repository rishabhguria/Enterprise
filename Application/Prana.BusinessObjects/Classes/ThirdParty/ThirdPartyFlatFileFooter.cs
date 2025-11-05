namespace Prana.BusinessObjects
{
    public class ThirdPartyFlatFileFooter
    {
        public ThirdPartyFlatFileFooter()
        {

        }


        private int _recordCount = 0;

        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        private double _totalQty = 0.0;

        public double TotalQty
        {
            get { return _totalQty; }
            set { _totalQty = value; }
        }

        // Added by: Ankit Yaman
        // On: December 18, 2013
        // For: http://jira.nirvanasolutions.com:8080/browse/MAERISLAND-110

        private double _internalNetNotional = 0.0;

        public double InternalNetNotional
        {
            get { return _internalNetNotional; }
            set { _internalNetNotional = value; }
        }

        private double _internalGrossAmount = 0.0;

        public double InternalGrossAmount
        {
            get { return _internalGrossAmount; }
            set { _internalGrossAmount = value; }
        }

        private string _date = string.Empty;

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }
        private string _dateAndTime = string.Empty;

        public string DateAndTime
        {
            get { return _dateAndTime; }
            set { _dateAndTime = value; }
        }

    }
}
