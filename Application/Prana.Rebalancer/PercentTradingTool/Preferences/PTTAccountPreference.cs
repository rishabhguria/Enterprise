using Prana.LogManager;
using System;
using System.ComponentModel;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    [Serializable]
    public class PTTAccountPreference
    {
        /// <summary>
        /// The account name
        /// </summary>
        private string _accountName;

        /// <summary>
        /// The accountID identifier
        /// </summary>
        private int _accountId;

        /// <summary>
        /// The account factor
        /// </summary>
        private float _accountFactor = 1;

        /// <summary>
        /// The percentage
        /// </summary>
        private decimal _percentage;

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        [Browsable(false)]
        public int AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public decimal Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }

        /// <summary>
        /// Gets or sets the account factor.
        /// </summary>
        /// <value>
        /// The account factor.
        /// </value>
        public float AccountFactor
        {
            get { return _accountFactor; }
            set { _accountFactor = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PTTAccountPreference"/> class.
        /// </summary>
        public PTTAccountPreference()
        {
            try
            {
                this._accountName = string.Empty;
                this._percentage = 0;
                this._accountId = int.MinValue;
                this._accountFactor = 1;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PTTAccountPreference"/> class.
        /// </summary>
        /// <param name="accname">The accname.</param>
        /// <param name="percent">The percent.</param>
        /// <param name="accId">The percent.</param>
        public PTTAccountPreference(string accname, int accId, decimal percent)
        {
            try
            {

                this._accountName = accname;
                this._percentage = percent;
                this._accountId = accId;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
    }
}
