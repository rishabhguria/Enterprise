using Prana.BusinessObjects;

namespace Prana.ShortLocate.Preferences
{
    public class ShortLocateUIPreferences : IPreferenceData
    {
        #region members

        private string _rebatefees;

        private string _alert;

        private string _yTD;

        private string _defaultBorrowBroker;

        private double _lastPxDecimal = 0;

        private double _rebatefeesDecimal = 0;

        private double _totalAmountDecimal = 0;

        #endregion

        #region Properties

        public string Rebatefees
        {
            get { return _rebatefees; }
            set { _rebatefees = value; }
        }

        public string Alert
        {
            get { return _alert; }
            set { _alert = value; }
        }

        public string YTD
        {
            get { return _yTD; }
            set { _yTD = value; }
        }

        public string DefaultBorrowBroker
        {
            get { return _defaultBorrowBroker; }
            set { _defaultBorrowBroker = value; }
        }

        public double LastPxDecimal
        {
            get { return _lastPxDecimal; }
            set { _lastPxDecimal = value; }
        }

        public double RebatefeesDecimal
        {
            get { return _rebatefeesDecimal; }
            set { _rebatefeesDecimal = value; }
        }

        public double TotalAmountDecimal
        {
            get { return _totalAmountDecimal; }
            set { _totalAmountDecimal = value; }
        }

        #endregion

    }
}
