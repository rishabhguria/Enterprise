using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// The model object for PTT request
    /// </summary>
    public class PTTRequestObject : BindableBase, IDataErrorInfo
    {
        /// <summary>
        /// The data error information support
        /// </summary>
        [NonSerialized]
        private readonly DataErrorInfoSupport dataErrorInfoSupport;

        /// <summary>
        /// Initializes a new instance of the <see cref="PTTRequestObject"/> class.
        /// </summary>
        public PTTRequestObject()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol = string.Empty;
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        [Required(ErrorMessage = "Please enter a valid Equity Symbol")]
        public string Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }

        /// <summary>
        /// The _target
        /// </summary>
        private decimal _target;
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public decimal Target
        {
            get { return _target; }
            set { SetProperty(ref _target, value); }
        }

        /// <summary>
        /// The _of or to
        /// </summary>
        private EnumerationValue _addOrSet;
        /// <summary>
        /// Gets or sets the of or to.
        /// </summary>
        /// <value>
        /// The of or to.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTChangeType), "Please select either ADD or SET")]
        public EnumerationValue AddOrSet
        {
            get { return _addOrSet; }
            set { SetProperty(ref _addOrSet, value); }
        }

        /// <summary>
        /// The _calculation value
        /// </summary>
        private EnumerationValue _masterFundOrAccount;
        /// <summary>
        /// Gets or sets the calculation value.
        /// </summary>
        /// <value>
        /// The calculation value.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTMasterFundOrAccount), "Please select a valid Calculation Value")]
        public EnumerationValue MasterFundOrAccount
        {
            get { return _masterFundOrAccount; }
            set { SetProperty(ref _masterFundOrAccount, value); }
        }

        /// <summary>
        /// The _consolidation value
        /// </summary>
        private bool _combinedAccountsTotalValue;
        /// <summary>
        /// Gets or sets a value indicating whether [consolidation value].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [consolidation value]; otherwise, <c>false</c>.
        /// </value>
        [Range(typeof(bool), "false", "true", ErrorMessage = "Please select a valid Combined Accounts Total Value")]
        public bool CombinedAccountsTotalValue
        {
            get { return _combinedAccountsTotalValue; }
            set { SetProperty(ref _combinedAccountsTotalValue, value); }
        }

        /// <summary>
        /// The _selected feed price
        /// </summary>
        private decimal _selectedFeedPrice;
        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>
        /// The selected feed price.
        /// </value>
        [ValidateGreaterThanZero("Price")]
        public decimal SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { SetProperty(ref _selectedFeedPrice, value); }
        }

        /// <summary>
        /// The _selected feed price in base currency
        /// </summary>
        private decimal _selectedFeedPriceInBaseCurrency;
        /// <summary>
        /// Gets or sets the selected feed price in base currency.
        /// </summary>
        /// <value>
        /// The selected feed price in base currency.
        /// </value>
        public decimal SelectedFeedPriceInBaseCurrency
        {
            get { return _selectedFeedPriceInBaseCurrency; }
            set { SetProperty(ref _selectedFeedPriceInBaseCurrency, value); }
        }

        /// <summary>
        /// The _account
        /// </summary>
        private ObservableCollection<object> _account = new ObservableCollection<object>();
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [EnsureMinimumElements(1, ErrorMessage = "At least an account/masterfund is required")]
        public ObservableCollection<object> Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        /// <summary>
        /// The _type
        /// </summary>
        private EnumerationValue _type;
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTType), "Please select either Basis Point and Percentage")]
        public EnumerationValue Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        /// <summary>
        /// The _sec master base object
        /// </summary>
        private SecMasterBaseObj _secMasterBaseObj;

        private ObservableCollection<object> _masterFund = new ObservableCollection<object>();

        /// <summary>
        /// Gets or sets the sec master base object.
        /// </summary>
        /// <value>
        /// The sec master base object.
        /// </value>
        public SecMasterBaseObj SecMasterBaseObj
        {
            get { return _secMasterBaseObj; }
            set { SetProperty(ref _secMasterBaseObj, value); }
        }

        /// <summary>
        /// The ticker symbol
        /// </summary>
        private string _tickerSymbol = string.Empty;

        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        /// <value>
        /// The ticker symbol.
        /// </value>
        public string TickerSymbol
        {
            get { return _tickerSymbol; }
            set { SetProperty(ref _tickerSymbol, value); }
        }

        #region IDataErrorInfo
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get { return dataErrorInfoSupport.Error; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get { return dataErrorInfoSupport[columnName]; }
        }

        /// <summary>
        /// Gets or sets the master fund.
        /// </summary>
        /// <value>
        /// The master fund.
        /// </value>
        public ObservableCollection<object> MasterFund
        {
            get { return _masterFund; }
            set { SetProperty(ref _masterFund, value); }
        }

        /// <summary>
        /// The combine account enum value
        /// </summary>
        private EnumerationValue _combineAccountEnumValue;

        /// <summary>
        /// Gets or sets the combine account enum value.
        /// </summary>
        /// <value>
        /// The combine account enum value.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTCombineAccountTotalValue), "Please select a valid Combined Accounts Total Value")]
        public EnumerationValue CombineAccountEnumValue
        {
            get { return _combineAccountEnumValue; }
            set { SetProperty(ref _combineAccountEnumValue, value); }
        }

        /// <summary>
        /// The _target
        /// </summary>
        private bool _isUseRoundLot;
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public bool IsUseRoundLot
        {
            get { return _isUseRoundLot; }
            set { _isUseRoundLot = value; }
        }

        /// <summary>
        /// The _target
        /// </summary>
        private bool _isUseCustodianBroker;
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public bool IsUseCustodianBroker
        {
            get { return _isUseCustodianBroker; }
            set { _isUseCustodianBroker = value; }
        }

        /// <summary>
        /// The round lot preference enum value
        /// </summary>
        private EnumerationValue _roundLotPreferenceEnumValue;

        /// <summary>
        /// The custodian broker preference enum value
        /// </summary>
        private EnumerationValue _custodianBrokerPreferenceEnumValue;
        /// <summary>
        /// Gets or sets the round lot preference enum value.
        /// </summary>
        /// <value>
        /// The combine account enum value.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTRoundLotPreferenceValue), "Please select a valid Round Lot Preference")]
        public EnumerationValue RoundLotPreferenceEnumValue
        {
            get
            {
                if (_roundLotPreferenceEnumValue != null)
                    IsUseRoundLot = _roundLotPreferenceEnumValue.Value.Equals((int)PTTRoundLotPreferenceValue.Yes);
                return _roundLotPreferenceEnumValue;
            }
            set { SetProperty(ref _roundLotPreferenceEnumValue, value); }
        }

        /// <summary>
        /// Gets or sets the custodian broker preference enum value.
        /// </summary>
        /// <value>
        /// The combine account enum value.
        /// </value>
        [ValidateEnumerationValue(typeof(PTTCustodianBrokerPreferenceValue), "Please select a valid Custodian Broker Preference")]
        public EnumerationValue CustodianBrokerPreferenceEnumValue
        {
            get
            {
                if (_custodianBrokerPreferenceEnumValue != null)
                    IsUseCustodianBroker = _custodianBrokerPreferenceEnumValue.Value.Equals((int)PTTCustodianBrokerPreferenceValue.Yes);
                return _custodianBrokerPreferenceEnumValue;
            }
            set { SetProperty(ref _custodianBrokerPreferenceEnumValue, value); }
        }
        #endregion
    }
}