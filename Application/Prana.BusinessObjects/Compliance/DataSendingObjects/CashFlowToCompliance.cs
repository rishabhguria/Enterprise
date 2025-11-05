namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of CashFlowToCompliance
    /// Used to send data to esper
    /// </summary>
    public class CashFlowToCompliance
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
        /// Sum of Inflow and
        /// </summary>
        private decimal _cash;

        public decimal Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }
    }
}
