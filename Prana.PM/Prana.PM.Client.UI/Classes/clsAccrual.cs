namespace Prana.PM.Client
{
    public class clsAccrual
    {

        #region Private Members

        private int _companyId = int.MinValue;
        private int _year = int.MinValue;
        private int _monthId = int.MinValue;
        private string _month = string.Empty;
        private double _accrualVal = double.MinValue;

        #endregion Private Members

        public clsAccrual()
        {

        }


        #region Public Properties

        public int CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }


        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public int MonthId
        {
            get { return _monthId; }
            set { _monthId = value; }
        }

        public string Month
        {
            get { return _month; }
            set { _month = value; }
        }



        public double AccrualVal
        {
            get { return _accrualVal; }
            set { _accrualVal = value; }
        }

        #endregion Public Properties
    }
}
