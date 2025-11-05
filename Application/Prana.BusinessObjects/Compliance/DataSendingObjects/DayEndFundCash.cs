namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of DayEndAccountCash
    /// Used to send data to esper
    /// </summary>
    public class DayEndAccountCash
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
        /// Initial cash
        /// </summary>
        private double _cash;

        public double Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }

        /// <summary>
        /// Sum of Inflow and OutFlow Cash
        /// </summary>
        private double _dayCash;

        public double DayCash
        {
            get { return _dayCash; }
            set { _dayCash = value; }
        }

    }
}
