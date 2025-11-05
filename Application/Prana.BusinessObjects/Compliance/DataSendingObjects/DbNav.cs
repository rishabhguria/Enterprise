namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of DbNav
    /// Used to send data to esper
    /// </summary>
    public class DbNav
    {
        private int _accountId;

        public int AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        private double _startOfDayNav;

        public double StartOfDayNav
        {
            get { return _startOfDayNav; }
            set { _startOfDayNav = value; }
        }


        private double _currentNav;

        public double CurrentNav
        {
            get { return _currentNav; }
            set { _currentNav = value; }
        }
    }
}
