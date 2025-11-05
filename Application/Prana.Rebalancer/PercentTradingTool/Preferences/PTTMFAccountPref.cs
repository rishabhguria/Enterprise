using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    [Serializable]
    public class PTTMFAccountPref
    {
        /// <summary>
        /// The master fund name
        /// </summary>
        private string _masterFundName;

        /// <summary>
        /// The masterID identifier
        /// </summary>
        private int _masterFundId;

        [Browsable(false)]
        public int MasterFundId
        {
            get { return _masterFundId; }
            set { _masterFundId = value; }
        }

        /// <summary>
        /// The total percentage
        /// </summary>
        private decimal _totalPercentage;

        /// <summary>
        /// The account wise percentage
        /// </summary>
        private List<PTTAccountPreference> _accountWisePercentage;

        /// <summary>
        /// The is prorata preference checked
        /// </summary>
        private bool _isProrataPrefChecked;

        public PTTMFAccountPref()
        {
            this._accountWisePercentage = new List<PTTAccountPreference>();
            this.MasterFundName = string.Empty;
            this._totalPercentage = 0;
        }

        /// <summary>
        /// Gets or sets the name of the master fund.
        /// </summary>
        /// <value>
        /// The name of the master fund.
        /// </value>
        public string MasterFundName
        {
            get { return _masterFundName; }
            set { _masterFundName = value; }
        }

        /// <summary>
        /// Gets or sets the total percentage.
        /// </summary>
        /// <value>
        /// The total percentage.
        /// </value>
        public decimal TotalPercentage
        {
            get { return _totalPercentage; }
            set { _totalPercentage = value; }
        }

        /// <summary>
        /// Gets or sets the account wise percentage.
        /// </summary>
        /// <value>
        /// The account wise percentage.
        /// </value>
        public List<PTTAccountPreference> AccountWisePercentage
        {
            get { return _accountWisePercentage; }
            set { _accountWisePercentage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is prorata preference checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is prorata preference checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsProrataPrefChecked
        {
            get { return _isProrataPrefChecked; }
            set { _isProrataPrefChecked = value; }
        }

        /// <summary>
        /// PreferenceType
        /// </summary>
        private PTTPreferenceType _preferenceType;
        /// <summary>
        /// Get set PreferenceType
        /// </summary>
        public PTTPreferenceType PreferenceType
        {
            get { return _preferenceType; }
            set { _preferenceType = value; }
        }
    }
}