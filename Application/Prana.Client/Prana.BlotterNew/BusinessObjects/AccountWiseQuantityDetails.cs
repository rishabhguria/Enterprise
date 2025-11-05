using System.Collections.Generic;

namespace Prana.Blotter.BusinessObjects
{
    internal class AccountWiseQuantityDetails
    {

        /// <summary>
        /// Get AccountQuantity Wise Preference
        /// </summary>
        private Dictionary<int, double> _accountWiseQuantity = null;

        public Dictionary<int, double> AccountWiseQuantity
        {
            get { return _accountWiseQuantity; }
            set { _accountWiseQuantity = value; }
        }

        /// <summary>
        /// Get ParentClOrderIds 
        /// </summary>
        private List<string> _parentClOrderIds = null;

        public List<string> ParentClOrderIds
        {
            get { return _parentClOrderIds; }
            set { _parentClOrderIds = value; }
        }

        /// <summary>
        /// Get Unique Trading Accounts
        /// </summary>
        private List<int> _uniqueTradingAccounts;

        public List<int> UniqueTradingAccounts
        {
            get { return _uniqueTradingAccounts; }
            set { _uniqueTradingAccounts = value; }
        }

    }
}
