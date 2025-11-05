namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of Accruals
    /// Used to send data to esper
    /// </summary>
    public class Accurals
    {
        /// <summary>
        /// Account Id that contains the Id of the Account
        /// </summary>
        private int _accountId;

        public int AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        /// <summary>
        /// Start Of Day Accruals that contains accrual till yesterday
        /// </summary>
        private double _startOfDayAccruals;

        public double StartOfDayAccruals
        {
            get { return _startOfDayAccruals; }
            set { _startOfDayAccruals = value; }
        }

        /// <summary>
        /// Days Accruals that contains todays accruals
        /// </summary>
        private double _dayAccruals;

        public double DayAccruals
        {
            get { return _dayAccruals; }
            set { _dayAccruals = value; }
        }
    }
}
