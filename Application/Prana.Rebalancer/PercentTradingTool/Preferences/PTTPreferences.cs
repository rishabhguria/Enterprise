using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    public class PTTPreferences : IPreferenceData
    {
        #region members

        /// <summary>
        /// The combine accounts total
        /// </summary>
        private PTTCombineAccountTotalValue _combineAccountsTotal;

        /// <summary>
        /// The increase decimal precision
        /// </summary>
        private int _increaseDecimalPrecision;

        /// <summary>
        /// The calculationtype
        /// </summary>
        private PTTType _calculationtype;

        /// <summary>
        /// The change type
        /// </summary>
        private PTTChangeType _changeType;

        /// <summary>
        /// The master fund or account
        /// </summary>
        private PTTMasterFundOrAccount _masterFundOrAccount;

        /// <summary>
        /// The remove accounts with zero nav
        /// </summary>
        private bool _removeAccountsWithZeroNAV;

        /// <summary>
        /// The comma separated accounts
        /// </summary>
        private string commaSeparatedAccounts = string.Empty;

        /// <summary>
        /// Short Long Pref
        /// </summary>
        private bool _useShortLongPref = false;

        #endregion

        #region Properties
        /// Gets or sets the increase decimal precision.
        /// </summary>
        /// <value>
        /// The increase decimal precision.
        /// </value>
        public int IncreaseDecimalPrecision
        {
            get { return _increaseDecimalPrecision; }
            set { _increaseDecimalPrecision = value; }
        }

        /// <summary>
        /// Gets or sets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        public PTTType CalculationType
        {
            get { return _calculationtype; }
            set { _calculationtype = value; }
        }

        /// <summary>
        /// Gets or sets the type of the change.
        /// </summary>
        /// <value>
        /// The type of the change.
        /// </value>
        public PTTChangeType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        /// <summary>
        /// Gets or sets the master fund or account.
        /// </summary>
        /// <value>
        /// The master fund or account.
        /// </value>
        public PTTMasterFundOrAccount MasterFundOrAccount
        {
            get { return _masterFundOrAccount; }
            set { _masterFundOrAccount = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [remove accounts with zero nav].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [remove accounts with zero nav]; otherwise, <c>false</c>.
        /// </value>
        public bool RemoveAccountsWithZeroNAV
        {
            get { return _removeAccountsWithZeroNAV; }
            set { _removeAccountsWithZeroNAV = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [combine accounts total].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [combine accounts total]; otherwise, <c>false</c>.
        /// </value>
        public PTTCombineAccountTotalValue CombineAccountsTotal
        {
            get { return _combineAccountsTotal; }
            set { _combineAccountsTotal = value; }
        }

        /// <summary>
        /// Gets or sets the comma separated accounts.
        /// </summary>
        /// <value>
        /// The comma separated accounts.
        /// </value>
        public string CommaSeparatedAccounts
        {
            get { return commaSeparatedAccounts; }
            set { commaSeparatedAccounts = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UseShortLongPref
        {
            get { return _useShortLongPref; }
            set { _useShortLongPref = value; }
        }
    }
    #endregion
}
