using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Common.Definitions;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class AutoGroupRulesControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _is broker checked
        /// </summary>
        private bool _isBrokerChecked;

        /// <summary>
        /// The _is trade date checked
        /// </summary>
        private bool _isTradeDateChecked;

        /// <summary>
        /// The _is venue checked
        /// </summary>
        private bool _isVenueChecked;

        /// <summary>
        /// The _is process date checked
        /// </summary>
        private bool _isProcessDateChecked;

        /// <summary>
        /// The _is trading account checked
        /// </summary>
        private bool _isTradingAccountChecked;

        /// <summary>
        /// The _is asset class checked
        /// </summary>
        private bool _isAssetClassChecked;

        /// <summary>
        /// The _is trade attribute1 checked
        /// </summary>
        private bool _isTradeAttribute1Checked;

        /// <summary>
        /// The _is trade attribute2 checked
        /// </summary>
        private bool _isTradeAttribute2Checked;

        /// <summary>
        /// The _is trade attribute3 checked
        /// </summary>
        private bool _isTradeAttribute3Checked;

        /// <summary>
        /// The _is trade attribute4 checked
        /// </summary>
        private bool _isTradeAttribute4Checked;

        /// <summary>
        /// The _is trade attribute5 checked
        /// </summary>
        private bool _isTradeAttribute5Checked;

        /// <summary>
        /// The _is trade attribute6 checked
        /// </summary>
        private bool _isTradeAttribute6Checked;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is asset class checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is asset class checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssetClassChecked
        {
            get { return _isAssetClassChecked; }
            set
            {
                _isAssetClassChecked = value;
                RaisePropertyChangedEvent("IsAssetClassChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is broker checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is broker checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsBrokerChecked
        {
            get { return _isBrokerChecked; }
            set
            {
                _isBrokerChecked = value;
                RaisePropertyChangedEvent("IsBrokerChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is process date checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is process date checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessDateChecked
        {
            get { return _isProcessDateChecked; }
            set
            {
                _isProcessDateChecked = value;
                if (!_isTradeDateChecked && !_isProcessDateChecked)
                {
                    _isProcessDateChecked = true;
                    MessageBox.Show("Please Select Either Trade Date or Process Date.", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                RaisePropertyChangedEvent("IsProcessDateChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute1 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute1 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute1Checked
        {
            get { return _isTradeAttribute1Checked; }
            set
            {
                _isTradeAttribute1Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute1Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute2 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute2 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute2Checked
        {
            get { return _isTradeAttribute2Checked; }
            set
            {
                _isTradeAttribute2Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute2Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute3 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute3 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute3Checked
        {
            get { return _isTradeAttribute3Checked; }
            set
            {
                _isTradeAttribute3Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute3Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute4 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute4 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute4Checked
        {
            get { return _isTradeAttribute4Checked; }
            set
            {
                _isTradeAttribute4Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute4Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute5 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute5 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute5Checked
        {
            get { return _isTradeAttribute5Checked; }
            set
            {
                _isTradeAttribute5Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute5Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade attribute6 checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade attribute6 checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeAttribute6Checked
        {
            get { return _isTradeAttribute6Checked; }
            set
            {
                _isTradeAttribute6Checked = value;
                RaisePropertyChangedEvent("IsTradeAttribute6Checked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trade date checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trade date checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradeDateChecked
        {
            get { return _isTradeDateChecked; }
            set
            {
                _isTradeDateChecked = value;
                if (!_isTradeDateChecked && !_isProcessDateChecked)
                {
                    _isTradeDateChecked = true;
                    MessageBox.Show("Please Select Either Trade Date or Process Date.", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                RaisePropertyChangedEvent("IsTradeDateChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trading account checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trading account checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsTradingAccountChecked
        {
            get { return _isTradingAccountChecked; }
            set
            {
                _isTradingAccountChecked = value;
                RaisePropertyChangedEvent("IsTradingAccountChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is venue checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is venue checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsVenueChecked
        {
            get { return _isVenueChecked; }
            set
            {
                _isVenueChecked = value;
                RaisePropertyChangedEvent("IsVenueChecked");
            }
        }

        // <summary>
        /// Gets or sets the additional trade attribute grouping preference used for auto-grouping functionality.     
        /// </summary>
        public List<ObservableCollection<TradeAttributeGroupingPreference>> AdditionalTradeAttributeList { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGroupRulesControlViewModel"/> class.
        /// </summary>
        public AutoGroupRulesControlViewModel()
        {
            try
            {
                // Initialize the TradeAttributes list with two collections
                AdditionalTradeAttributeList = new List<ObservableCollection<TradeAttributeGroupingPreference>>
                {
                    new ObservableCollection<TradeAttributeGroupingPreference>(), // Odd indexes
                    new ObservableCollection<TradeAttributeGroupingPreference>()  // Even indexes
                };

                // Populate TradeAttributes with models for Trade Attribute 7 to 45
                for (int i = 7; i <= 45; i++)
                {
                    var model = new TradeAttributeGroupingPreference
                    {
                        Name = ApplicationConstants.CONST_TRADE_ATTRIBUTE + i,
                        Label = $"And Trade Attribute {i}",
                        ChkBoxAutomationName = "chk" + ApplicationConstants.CONST_TRADE_ATTRIBUTE + i,
                        LblAutomationName = "lbl" + ApplicationConstants.CONST_TRADE_ATTRIBUTE + i
                    };
                    // Add to index 0 if odd, index 1 if even
                    AdditionalTradeAttributeList[i % 2 == 0 ? 1 : 0].Add(model);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the automatic grouping rules.
        /// </summary>
        /// <returns></returns>
        internal AutoGroupingRules GetAutoGroupingRules()
        {
            AutoGroupingRules groupingRules = new AutoGroupingRules();
            try
            {
                groupingRules.AssetClass = IsAssetClassChecked;
                groupingRules.TradeDate = IsTradeDateChecked;
                groupingRules.ProcessDate = IsProcessDateChecked;
                groupingRules.Broker = IsBrokerChecked;
                groupingRules.Venue = IsVenueChecked;
                groupingRules.TradingAccount = IsTradingAccountChecked;
                groupingRules.TradeAttributes1 = IsTradeAttribute1Checked;
                groupingRules.TradeAttributes2 = IsTradeAttribute2Checked;
                groupingRules.TradeAttributes3 = IsTradeAttribute3Checked;
                groupingRules.TradeAttributes4 = IsTradeAttribute4Checked;
                groupingRules.TradeAttributes5 = IsTradeAttribute5Checked;
                groupingRules.TradeAttributes6 = IsTradeAttribute6Checked;

                foreach (var attribute in AdditionalTradeAttributeList.SelectMany(group => group))
                {
                    groupingRules.SetTradeAttributeValue(attribute.Name, attribute.IsChecked);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groupingRules;

        }

        /// <summary>
        /// Sets the allocation preferences.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void OnLoadAutoGroupRulesControl(AllocationPreferences allocationPreferences)
        {
            try
            {
                IsBrokerChecked = allocationPreferences.AutoGroupingRules.Broker;
                IsAssetClassChecked = true;
                if (allocationPreferences.AutoGroupingRules.ProcessDate)
                {
                    IsProcessDateChecked = allocationPreferences.AutoGroupingRules.ProcessDate;
                    IsTradeDateChecked = allocationPreferences.AutoGroupingRules.TradeDate;
                }
                else if (allocationPreferences.AutoGroupingRules.TradeDate)
                {
                    IsTradeDateChecked = allocationPreferences.AutoGroupingRules.TradeDate;
                    IsProcessDateChecked = allocationPreferences.AutoGroupingRules.ProcessDate;
                }
                IsVenueChecked = allocationPreferences.AutoGroupingRules.Venue;
                IsTradingAccountChecked = allocationPreferences.AutoGroupingRules.TradingAccount;
                IsTradeAttribute1Checked = allocationPreferences.AutoGroupingRules.TradeAttributes1;
                IsTradeAttribute2Checked = allocationPreferences.AutoGroupingRules.TradeAttributes2;
                IsTradeAttribute3Checked = allocationPreferences.AutoGroupingRules.TradeAttributes3;
                IsTradeAttribute4Checked = allocationPreferences.AutoGroupingRules.TradeAttributes4;
                IsTradeAttribute5Checked = allocationPreferences.AutoGroupingRules.TradeAttributes5;
                IsTradeAttribute6Checked = allocationPreferences.AutoGroupingRules.TradeAttributes6;
                var tradeAttributesDict = allocationPreferences.AutoGroupingRules.GetTradeAttributesAsDict();

                // Iterate through all trade attributes (flattened from nested collections)
                foreach (var tradeAttribute in AdditionalTradeAttributeList.SelectMany(group => group))
                {
                    // Try to find the current attribute name in the dictionary
                    if (tradeAttributesDict.TryGetValue(tradeAttribute.Name, out var isChecked))
                    {
                        tradeAttribute.IsChecked = isChecked;
                    }                   
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
