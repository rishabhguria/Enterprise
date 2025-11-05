using Infragistics.Controls.Editors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class EditTradeControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [apply edit trade changes event].
        /// </summary>
        public event EventHandler ApplyEditTradeChangesEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _counter party
        /// </summary>
        private ObservableDictionary<int, string> _counterParty;

        /// <summary>
        /// The _selected counter party
        /// </summary>
        private KeyValuePair<int, string> _selectedCounterParty;

        /// <summary>
        /// The _venues
        /// </summary>
        private ObservableDictionary<int, string> _venues;

        /// <summary>
        /// The _venue
        /// </summary>
        private KeyValuePair<int, string> _venue;

        /// <summary>
        /// The _sides
        /// </summary>
        private ObservableDictionary<string, string> _sides;

        /// <summary>
        /// The _selected side
        /// </summary>
        private KeyValuePair<string, string> _selectedSide;

        /// <summary>
        /// The _FX conversion operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _fxConversionOperator;

        /// <summary>
        /// The _selected fx conversion operator
        /// </summary>
        private EnumerationValue _selectedFxConversionOperator;

        /// <summary>
        /// The _settl conversion operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _settlConversionOperator;

        /// <summary>
        /// The _selected settl conversion operator
        /// </summary>
        private EnumerationValue _selectedSettlConversionOperator;

        /// <summary>
        /// The _modifiedGroup
        /// </summary>
        private AllocationGroup _modifiedGroup;

        /// <summary>
        /// The _original group
        /// </summary>
        private AllocationGroup _originalGroup;

        /// <summary>
        /// The _ is enabled control
        /// </summary>
        private bool _IsEnabledControl;

        /// <summary>
        /// The _ is enabled basic fields
        /// </summary>
        private bool _isEnabledBasicFields;

        /// <summary>
        /// The _ IsEnabledAccruedInterest
        /// </summary>
        private bool _IsEnabledAccruedInterest;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _total commission and fees
        /// </summary>
        private string _totalCommissionAndFees;

        /// <summary>
        /// The group status
        /// </summary>
        private PostTradeEnums.Status _groupStatus;

        /// <summary>
        /// The _is enabled buttons
        /// </summary>
        private bool _isEnabledButtons;

        /// <summary>
        /// The _trade attributes collection
        /// </summary>
        private ObservableCollection<string>[] _tradeAttributesCollection;

        /// <summary>
        /// The trade attributes keep records
        /// </summary>
        private CustomValueEnteredActions[] _tradeAttributesKeepRecords;

        /// <summary>
        /// The _qty precision format
        /// </summary>
        private string _qtyPrecisionFormat;

        /// <summary>
        /// The updated group which contains commission and fees value after broker change
        /// </summary>
        private AllocationGroup _updatedGroup = null;

        /// <summary>
        /// The Settlement currency
        /// </summary>
        private ObservableDictionary<int, string> _settlCurrency;

        /// <summary>
        /// The _is enabled settlement currency
        /// </summary>
        private bool _isEnableCurrencyDependencies;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the counter party.
        /// </summary>
        /// <value>
        /// The counter party.
        /// </value>
        public ObservableDictionary<int, string> CounterParty
        {
            get { return _counterParty; }
            set
            {
                _counterParty = value;
                RaisePropertyChangedEvent("CounterParty");
            }
        }

        /// <summary>
        /// Gets or sets the fx conversion operator.
        /// </summary>
        /// <value>
        /// The fx conversion operator.
        /// </value>
        public ObservableCollection<EnumerationValue> FxConversionOperator
        {
            get { return _fxConversionOperator; }
            set
            {
                _fxConversionOperator = value;
                RaisePropertyChangedEvent("FxConversionOperator");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled buttons.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled buttons; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledButtons
        {
            get { return _isEnabledButtons; }
            set
            {
                _isEnabledButtons = value;
                RaisePropertyChangedEvent("IsEnabledButtons");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled control.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled control; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledControl
        {
            get { return _IsEnabledControl; }
            set
            {
                _IsEnabledControl = value;
                RaisePropertyChangedEvent("IsEnabledControl");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled control.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled control; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledBasicFields
        {
            get { return _isEnabledBasicFields; }
            set
            {
                _isEnabledBasicFields = value;
                RaisePropertyChangedEvent("IsEnabledBasicFields");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has enabled Accrued Interest.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has enabled Accrued Interest; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledAccruedInterest
        {
            get { return _IsEnabledAccruedInterest; }
            set
            {
                _IsEnabledAccruedInterest = value;
                RaisePropertyChangedEvent("IsEnabledAccruedInterest");
            }
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public AllocationGroup ModifiedGroup
        {
            get { return _modifiedGroup; }
            set
            {
                _modifiedGroup = value;
                SetTradeAttributeSelectedValues(value);
                RaisePropertyChangedEvent("ModifiedGroup");
            }
        }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set
            {
                _precisionFormat = value;
                RaisePropertyChangedEvent("PrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the qty precision format.
        /// </summary>
        /// <value>
        /// The qty precision format.
        /// </value>
        public string QtyPrecisionFormat
        {
            get { return _qtyPrecisionFormat; }
            set
            {
                _qtyPrecisionFormat = value;
                RaisePropertyChangedEvent("QtyPrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the selected counter party.
        /// </summary>
        /// <value>
        /// The selected counter party.
        /// </value>
        public KeyValuePair<int, string> SelectedCounterParty
        {
            get { return _selectedCounterParty; }
            set
            {
                _selectedCounterParty = value;
                RaisePropertyChangedEvent("SelectedCounterParty");
                OnCounterPartyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected fx conversion operator.
        /// </summary>
        /// <value>
        /// The selected fx conversion operator.
        /// </value>
        public EnumerationValue SelectedFxConversionOperator
        {
            get { return _selectedFxConversionOperator; }
            set
            {
                _selectedFxConversionOperator = value;
                RaisePropertyChangedEvent("SelectedFxConversionOperator");
            }
        }

        /// <summary>
        /// Gets or sets the selected settl conversion operator.
        /// </summary>
        /// <value>
        /// The selected settl conversion operator.
        /// </value>
        public EnumerationValue SelectedSettlConversionOperator
        {
            get { return _selectedSettlConversionOperator; }
            set
            {
                _selectedSettlConversionOperator = value;
                RaisePropertyChangedEvent("SelectedSettlConversionOperator");
            }
        }

        /// <summary>
        /// Gets or sets the selected side.
        /// </summary>
        /// <value>
        /// The selected side.
        /// </value>
        public KeyValuePair<string, string> SelectedSide
        {
            get { return _selectedSide; }
            set
            {
                _selectedSide = value;
                RaisePropertyChangedEvent("SelectedSide");
                OnSideChangedFromSidePanel();
            }
        }

        /// <summary>
        /// Gets or sets the settl conversion operator.
        /// </summary>
        /// <value>
        /// The settl conversion operator.
        /// </value>
        public ObservableCollection<EnumerationValue> SettlConversionOperator
        {
            get { return _settlConversionOperator; }
            set
            {
                _settlConversionOperator = value;
                RaisePropertyChangedEvent("SettlConversionOperator");
            }
        }

        /// <summary>
        /// Gets or sets the sides.
        /// </summary>
        /// <value>
        /// The sides.
        /// </value>
        public ObservableDictionary<string, string> Sides
        {
            get { return _sides; }
            set
            {
                _sides = value;
                RaisePropertyChangedEvent("Sides");
            }
        }

        /// <summary>
        /// Gets or sets the total commission and fees.
        /// </summary>
        /// <value>
        /// The total commission and fees.
        /// </value>
        public string TotalCommissionAndFees
        {
            get { return _totalCommissionAndFees; }
            set
            {
                _totalCommissionAndFees = value;
                RaisePropertyChangedEvent("TotalCommissionAndFees");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes collection.
        /// </summary>
        /// <value>
        /// The trade attributes collection.
        /// </value>
        public ObservableCollection<string>[] TradeAttributesCollection
        {
            get { return _tradeAttributesCollection; }
            set
            {
                _tradeAttributesCollection = value;
                foreach (var item in AllTradeAttributes)
                {
                    if ((item.Index) < value.Length)
                    {
                        item.Values = value[item.Index];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes keep records.
        /// </summary>
        /// <value>
        /// The trade attributes keep records.
        /// </value>
        public CustomValueEnteredActions[] TradeAttributesKeepRecords
        {
            get { return _tradeAttributesKeepRecords; }
            set
            {
                _tradeAttributesKeepRecords = value;
                foreach (var item in AllTradeAttributes)
                {
                    if (item.Index < value.Length)
                    {
                        item.CustomValueEnteredAction = value[item.Index];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the venue.
        /// </summary>
        /// <value>
        /// The venue.
        /// </value>
        public KeyValuePair<int, string> Venue
        {
            get { return _venue; }
            set
            {
                _venue = value;
                RaisePropertyChangedEvent("Venue");
            }
        }

        /// <summary>
        /// Gets or sets the venues.
        /// </summary>
        /// <value>
        /// The venues.
        /// </value>
        public ObservableDictionary<int, string> Venues
        {
            get { return _venues; }
            set
            {
                _venues = value;
                RaisePropertyChangedEvent("Venues");
            }
        }

        /// <summary>
        /// Gets or sets the Settlement currency.
        /// </summary>
        /// <value>
        /// The settl currency.
        /// </value>
        public ObservableDictionary<int, string> SettlCurrency
        {
            get { return _settlCurrency; }
            set
            {
                _settlCurrency = value;
                RaisePropertyChangedEvent("SettlCurrency");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether settlement currency in edit trade enabled or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if [enabled settlement currency]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCurrencyDependencies
        {
            get { return _isEnableCurrencyDependencies; }
            set
            {
                _isEnableCurrencyDependencies = value;
                RaisePropertyChangedEvent("EnableCurrencyDependencies");
            }
        }

        /// <summary>
        /// The collection of all trade attributes
        /// </summary>
        public ObservableCollection<TradeAttributeViewModel> AllTradeAttributes { get; } = new ObservableCollection<TradeAttributeViewModel>();
        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the apply button clicked.
        /// </summary>
        /// <value>
        /// The apply button clicked.
        /// </value>
        public RelayCommand<object> ApplyButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the cancel button clicked.
        /// </summary>
        /// <value>
        /// The cancel button clicked.
        /// </value>
        public RelayCommand<object> CancelButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the trade date changed.
        /// </summary>
        /// <value>
        /// The trade date changed.
        /// </value>
        public RelayCommand<object> TradeDateChanged { get; set; }

        /// <summary>
        /// Update Values
        /// </summary>
        public RelayCommand<object> UpdateValues { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditTradeControlViewModel" /> class.
        /// </summary>
        public EditTradeControlViewModel()
        {
            try
            {
                CancelButtonClicked = new RelayCommand<object>((parameter) => CancelGroupChanges(parameter));
                ApplyButtonClicked = new RelayCommand<object>((parameter) => ApplyEditTradeChanges(parameter));
                TradeDateChanged = new RelayCommand<object>((parameter) => OnTradeDateChanged(parameter));
                UpdateValues = new RelayCommand<object>((parameter) => OnUpdateValues(parameter));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Applies the edit trade changes.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ApplyEditTradeChanges(object parameter)
        {
            try
            {
                string message = ValidateDates();
                if (_groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                    MessageBox.Show("Corporate Action is applied cannot be modified.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                if (_groupStatus.Equals(PostTradeEnums.Status.Closed)
                    && (_originalGroup.TransactionType.Equals(TradingTransactionType.Exercise.ToString()) || _originalGroup.TransactionType.Equals(TradingTransactionType.Assignment.ToString())))
                    MessageBox.Show("Offset trade generated by Exercise/Assign cannot be modified.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                else if (!string.IsNullOrWhiteSpace(message))
                    MessageBox.Show(message, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    EditSelectedGroup();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Cancels the group changes.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object CancelGroupChanges(object parameter)
        {
            try
            {
                ModifiedGroup = (AllocationGroup)_originalGroup.Clone();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Edits the selected group.
        /// </summary>
        private void EditSelectedGroup()
        {
            try
            {
                AllocationClientManager.GetInstance().DictUnsavedAdd(_originalGroup.GroupID, (AllocationGroup)_originalGroup.Clone());
                _originalGroup.IsAnotherTaxlotAttributesUpdated = true;
                _originalGroup.CompanyUserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _originalGroup.IsModified = true;
                _originalGroup.UpdateGroupPersistenceStatus();

                UpdateBasicDetails();
                UpdateDates();
                UpdateCommissionAndFees();
                UpdateTradeAttributes();

                if (ApplyEditTradeChangesEvent != null)
                    ApplyEditTradeChangesEvent(this, EventArgs.Empty);
                _originalGroup.PropertyHasChanged();
                RaisePropertyChangedEvent("ModifiedGroup");
                _updatedGroup = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the settlement date.
        /// </summary>
        /// <param name="group">The group.</param>
        private DateTime GetSettlementDate(AllocationGroup group)
        {
            try
            {
                int auecID = Convert.ToInt32(group.AUECID);
                string sideText = group.OrderSide;
                if (sideText != "0")
                {
                    string sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);
                    int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                    DateTime tradeDate = Convert.ToDateTime(group.AUECLocalDate.ToString());
                    if (auecSettlementPeriod == 0)
                        return tradeDate;
                    else
                        return BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return group.SettlementDate;
        }

        /// <summary>
        /// Called when [load edit trade control].
        /// </summary>
        /// <param name="TradeAttributes">The trade attributes.</param>
        internal void OnLoadEditTradeControl(ObservableCollection<string>[] tradeAttributes, CustomValueEnteredActions[] custValueforTradeAttributes)
        {
            try
            {
                PopulateTradeAttributes();
                TradeAttributesCollection = tradeAttributes;
                TradeAttributesKeepRecords = custValueforTradeAttributes;
                List<EnumerationValue> conversionOperator = new List<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(Operator)));
                conversionOperator = new List<EnumerationValue>(conversionOperator.Where(x => x.DisplayText != Operator.Multiple.ToString()));

                Dictionary<int, string> brokerDict = new Dictionary<int, string>();
                brokerDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                brokerDict.AddRangeThreadSafely(Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserCounterParties());

                Dictionary<int, string> venueDict = new Dictionary<int, string>();
                venueDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                venueDict.AddRangeThreadSafely(CommonDataCache.CachedDataManager.GetInstance.GetAllVenues());

                Sides = new ObservableDictionary<string, string>(CommonAllocationMethods.GetOrderSides());
                CounterParty = new ObservableDictionary<int, string>(brokerDict);
                Venues = new ObservableDictionary<int, string>(venueDict);
                FxConversionOperator = new ObservableCollection<EnumerationValue>(conversionOperator);
                SettlConversionOperator = new ObservableCollection<EnumerationValue>(conversionOperator);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [counter party changed].
        /// </summary>
        private void OnCounterPartyChanged()
        {
            try
            {
                if ((ModifiedGroup.CounterPartyID != SelectedCounterParty.Key) && !(ModifiedGroup.CounterPartyID == 0 && SelectedCounterParty.Key == int.MinValue))
                {
                    ModifiedGroup.CounterPartyName = SelectedCounterParty.Value;
                    ModifiedGroup.CounterPartyID = SelectedCounterParty.Key;
                    _updatedGroup = EditTradeHelper.RecalculateCommission(ModifiedGroup);
                    RaisePropertyChangedEvent("ModifiedGroup");
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [trade date changed].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnTradeDateChanged(object parameter)
        {
            try
            {
                if (ModifiedGroup.AUECLocalDate != _originalGroup.AUECLocalDate)
                {
                    if (ModifiedGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated && ModifiedGroup.AUECLocalDate.Date != _originalGroup.AUECLocalDate.Date)
                    {
                        MessageBox.Show("Please Save Status of Trade Before Changing Trade Date.", "Warning", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                        ModifiedGroup.AUECLocalDate = _originalGroup.AUECLocalDate;
                        RaisePropertyChangedEvent("ModifiedGroup");
                        return null;
                    }
                    ModifiedGroup.ProcessDate = ModifiedGroup.AUECLocalDate;
                    ModifiedGroup.OriginalPurchaseDate = ModifiedGroup.AUECLocalDate;
                    ModifiedGroup.SettlementDate = GetSettlementDate(ModifiedGroup);
                    RaisePropertyChangedEvent("ModifiedGroup");
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// On Update Values
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object OnUpdateValues(object parameter)
        {
            try
            {
                if (ModifiedGroup != null && parameter.ToString() == "AvgPrice")
                {
                    List<OtherFeeType> listofFeesToApply = new List<OtherFeeType>();
                    listofFeesToApply.Add(OtherFeeType.SecFee);
                    _updatedGroup = EditTradeHelper.ReCalculateOtherFeeForGroup(ModifiedGroup, listofFeesToApply);
                    RaisePropertyChangedEvent("ModifiedGroup");
                }
                if (ModifiedGroup != null && (ModifiedGroup.AssetName == AssetCategory.FixedIncome.ToString() || ModifiedGroup.AssetName == AssetCategory.ConvertibleBond.ToString()))
                {
                    ModifiedGroup.AccruedInterest = AllocationClientManager.GetInstance().CalculateAccuredInterest(ModifiedGroup);
                    RaisePropertyChangedEvent("ModifiedGroup");
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Clears the edit trades field value.
        /// </summary>
        internal void ResetEditTradeFieldsValue()
        {
            try
            {
                _originalGroup = new AllocationGroup();
                ModifiedGroup = new AllocationGroup();
                SelectedCounterParty = new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                SelectedSide = new KeyValuePair<string, string>();
                Venue = new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                TotalCommissionAndFees = "0.0";
                IsEnabledButtons = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        internal void SetPreferences(int precisionDigit)
        {
            try
            {
                //set precision format
                PrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(precisionDigit);
                SetTradeAttributesNames();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the trade attributes names.
        /// </summary>
        private void SetTradeAttributesNames()
        {
            try
            {
                foreach (TradeAttributeViewModel tradeAttribute in AllTradeAttributes)
                {
                    tradeAttribute.Label = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute + tradeAttribute.AttributeNumber);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Shows the group details.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="status">The status.</param>
        internal void ShowGroupDetails(AllocationGroup group, PostTradeEnums.Status status)
        {
            try
            {
                EnableSelectSettlementCurrency(group);

                IsEnabledButtons = true;
                _originalGroup = group;
                _groupStatus = status;
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    IsEnabledControl = false;
                else
                    IsEnabledControl = true;
                if (status == PostTradeEnums.Status.Exercise || status == PostTradeEnums.Status.ExerciseAssignManually)
                    IsEnabledBasicFields = false;
                else
                    IsEnabledBasicFields = true;

                IsEnabledAccruedInterest = (group.AssetName == "ConvertibleBond" || group.AssetName == "FixedIncome") ? true : false;


                ModifiedGroup = (AllocationGroup)_originalGroup.Clone();
                TotalCommissionAndFees = ModifiedGroup.TotalCommissionandFees.ToString();

                foreach (var item in AllTradeAttributes)
                {
                    item.IsEnabled = item.AttributeNumber == 6 ? !CachedDataManager.GetInstance.IsShowMasterFundonTT() : !_groupStatus.Equals(PostTradeEnums.Status.CorporateAction);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// UpdateBasicDetails
        /// </summary>
        private void UpdateBasicDetails()
        {
            try
            {
                bool isGroupEditted = false;
                if (!ModifiedGroup.CumQty.Equals(_originalGroup.CumQty))
                {
                    if (ModifiedGroup.CumQty > ModifiedGroup.Quantity)
                        MessageBox.Show("Executed Quantity should be less than or equal to the Quantity!", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        _originalGroup.CumQty = ModifiedGroup.CumQty;
                        _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                        isGroupEditted = true;
                    }
                }
                if (!ModifiedGroup.AvgPrice.Equals(_originalGroup.AvgPrice))
                {
                    _originalGroup.AvgPrice = ModifiedGroup.AvgPrice;

                    if (_originalGroup.AssetID == (int)AssetCategory.FX || _originalGroup.AssetID == (int)AssetCategory.FXForward || _originalGroup.AssetID == (int)AssetCategory.Forex)
                    {
                        if (_originalGroup.LeadCurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                            ModifiedGroup.FXRate = ModifiedGroup.AvgPrice;
                        else
                            ModifiedGroup.FXRate = ModifiedGroup.AvgPrice != 0 ? 1 / ModifiedGroup.AvgPrice : 0;

                        _originalGroup.FXRate = ModifiedGroup.FXRate;
                    }

                    if (_updatedGroup != null)
                    {
                        _originalGroup.UpdateSecFee(_updatedGroup);
                        _originalGroup.UpdateSecFeeAtTaxlotLevel(_updatedGroup);
                    }
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                    _originalGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.OrderSide != _originalGroup.OrderSide)
                {
                    if (Sides.ContainsValue(ModifiedGroup.OrderSide))
                    {
                        _originalGroup.OrderSideTagValue = SelectedSide.Key;
                        ModifiedGroup.OrderSideTagValue = SelectedSide.Key;
                    }
                    _originalGroup.OrderSide = ModifiedGroup.OrderSide;
                    if (ModifiedGroup.TransactionType != _originalGroup.TransactionType)
                    {
                        _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                        _originalGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.TransactionType_Changed);
                        _originalGroup.TransactionType = ModifiedGroup.TransactionType;
                    }
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.CounterPartyName != _originalGroup.CounterPartyName)
                {
                    _originalGroup.CounterPartyName = ModifiedGroup.CounterPartyName;
                    _originalGroup.CounterPartyID = SelectedCounterParty.Key;
                    ModifiedGroup.CounterPartyID = SelectedCounterParty.Key;
                    if (_updatedGroup != null)
                    {
                        _originalGroup.UpdateCommissionAndFees(_updatedGroup);
                        _originalGroup.UpdateCommissionAndFeesAtTaxlotLevel(_updatedGroup);
                    }
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
                    _originalGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Counterparty_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.Venue != _originalGroup.Venue)
                {
                    _originalGroup.Venue = ModifiedGroup.Venue;
                    _originalGroup.VenueID = Venue.Key;
                    ModifiedGroup.VenueID = Venue.Key;
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.Venue_Changed);
                    _originalGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Venue_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.Description != _originalGroup.Description)
                {
                    _originalGroup.Description = ModifiedGroup.Description;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.Description_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.InternalComments != _originalGroup.InternalComments)
                {
                    _originalGroup.InternalComments = ModifiedGroup.InternalComments;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.InternalComments_Changed);
                }
                if (!ModifiedGroup.FXRate.Equals(_originalGroup.FXRate))
                {
                    _originalGroup.FXRate = ModifiedGroup.FXRate;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.FxRate_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.FXConversionMethodOperator != _originalGroup.FXConversionMethodOperator)
                {
                    _originalGroup.FXConversionMethodOperator = ModifiedGroup.FXConversionMethodOperator;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                    if (ModifiedGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        _originalGroup.UpdateGroupTaxlots("FXConversionMethodOperator", ModifiedGroup.FXConversionMethodOperator);
                    else
                        isGroupEditted = true;
                }
                if (!ModifiedGroup.AccruedInterest.Equals(_originalGroup.AccruedInterest))
                {
                    _originalGroup.AccruedInterest = ModifiedGroup.AccruedInterest;
                    _originalGroup.UpdateTaxlotAccruedInterest();
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.AccruedInterest_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.SettlementCurrencyID != _originalGroup.SettlementCurrencyID)
                {
                    _originalGroup.SettlementCurrencyID = ModifiedGroup.SettlementCurrencyID;
                    _originalGroup.IsAnotherTaxlotAttributesUpdated = true;
                    _originalGroup.UpdateSettlementCurrencyInTaxlots(_originalGroup);
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.SettlCurrency_Changed);
                }
                if (isGroupEditted)
                {
                    if (ModifiedGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                        _originalGroup.UpdateGroupTaxlots(string.Empty, string.Empty);
                    else
                        UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the commission source.
        /// </summary>
        private void UpdateCommissionAndFees()
        {
            try
            {
                CommissionFields commFields = new CommissionFields();
                bool isCommissionChanged = false;
                if (!ModifiedGroup.TotalCommissionandFees.Equals(_originalGroup.TotalCommissionandFees))
                    isCommissionChanged = true;

                if (!ModifiedGroup.Commission.Equals(_originalGroup.Commission))
                {
                    _originalGroup.Commission = ModifiedGroup.Commission;
                    commFields.Commission = ModifiedGroup.Commission;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.Commission_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.SoftCommission.Equals(_originalGroup.SoftCommission))
                {
                    _originalGroup.SoftCommission = ModifiedGroup.SoftCommission;
                    commFields.SoftCommission = ModifiedGroup.SoftCommission;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.SoftCommission_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.OtherBrokerFees.Equals(_originalGroup.OtherBrokerFees))
                {
                    _originalGroup.OtherBrokerFees = ModifiedGroup.OtherBrokerFees;
                    commFields.OtherBrokerFees = ModifiedGroup.OtherBrokerFees;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.ClearingBrokerFee.Equals(_originalGroup.ClearingBrokerFee))
                {
                    _originalGroup.ClearingBrokerFee = ModifiedGroup.ClearingBrokerFee;
                    commFields.ClearingBrokerFee = ModifiedGroup.ClearingBrokerFee;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.StampDuty.Equals(_originalGroup.StampDuty))
                {
                    _originalGroup.StampDuty = ModifiedGroup.StampDuty;
                    commFields.StampDuty = ModifiedGroup.StampDuty;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.StampDuty_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.TransactionLevy.Equals(_originalGroup.TransactionLevy))
                {
                    _originalGroup.TransactionLevy = ModifiedGroup.TransactionLevy;
                    commFields.TransactionLevy = ModifiedGroup.TransactionLevy;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.ClearingFee.Equals(_originalGroup.ClearingFee))
                {
                    _originalGroup.ClearingFee = ModifiedGroup.ClearingFee;
                    commFields.ClearingFee = ModifiedGroup.ClearingFee;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.ClearingFee_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.TaxOnCommissions.Equals(_originalGroup.TaxOnCommissions))
                {
                    _originalGroup.TaxOnCommissions = ModifiedGroup.TaxOnCommissions;
                    commFields.TaxOnCommissions = ModifiedGroup.TaxOnCommissions;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.MiscFees.Equals(_originalGroup.MiscFees))
                {
                    _originalGroup.MiscFees = ModifiedGroup.MiscFees;
                    commFields.MiscFees = ModifiedGroup.MiscFees;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.MiscFees_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.SecFee.Equals(_originalGroup.SecFee))
                {
                    _originalGroup.SecFee = ModifiedGroup.SecFee;
                    commFields.SecFee = ModifiedGroup.SecFee;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.SecFee_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.OccFee.Equals(_originalGroup.OccFee))
                {
                    _originalGroup.OccFee = ModifiedGroup.OccFee;
                    commFields.OccFee = ModifiedGroup.OccFee;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.OccFee_Changed);
                    isCommissionChanged = true;
                }
                if (!ModifiedGroup.OrfFee.Equals(_originalGroup.OrfFee))
                {
                    _originalGroup.OrfFee = ModifiedGroup.OrfFee;
                    commFields.OrfFee = ModifiedGroup.OrfFee;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.OrfFee_Changed);
                    isCommissionChanged = true;
                }
                if (isCommissionChanged)
                {
                    _originalGroup.CommSource = CommisionSource.Manual;
                    _originalGroup.SoftCommSource = CommisionSource.Manual;
                    _originalGroup.CommissionSource = (int)CommisionSource.Manual;
                    _originalGroup.SoftCommissionSource = (int)CommisionSource.Manual;
                }
                _originalGroup.UpdateTaxlotCommissionAndFees(commFields);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the datafor trade attributes.
        /// </summary>
        private void UpdateDataforTradeAttributes()
        {
            try
            {
                if (ModifiedGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                {
                    TradeAttributes tradeAttr = new TradeAttributes();
                    tradeAttr.TradeAttribute1 = ModifiedGroup.TradeAttribute1;
                    tradeAttr.TradeAttribute2 = ModifiedGroup.TradeAttribute2;
                    tradeAttr.TradeAttribute3 = ModifiedGroup.TradeAttribute3;
                    tradeAttr.TradeAttribute4 = ModifiedGroup.TradeAttribute4;
                    tradeAttr.TradeAttribute5 = ModifiedGroup.TradeAttribute5;
                    tradeAttr.TradeAttribute6 = ModifiedGroup.TradeAttribute6;
                    tradeAttr.SetTradeAttribute(ModifiedGroup.GetTradeAttributesAsDict());
                    _originalGroup.UpdateTaxlotTradeAttributes(tradeAttr);
                }
                else
                    UpdateUnAllocatedGroupTaxlotState();

                if (_originalGroup.Orders.Count == 1)
                {
                    foreach (AllocationOrder ord in _originalGroup.Orders)
                    {
                        ord.TradeAttribute1 = ModifiedGroup.TradeAttribute1;
                        ord.TradeAttribute2 = ModifiedGroup.TradeAttribute2;
                        ord.TradeAttribute3 = ModifiedGroup.TradeAttribute3;
                        ord.TradeAttribute4 = ModifiedGroup.TradeAttribute4;
                        ord.TradeAttribute5 = ModifiedGroup.TradeAttribute5;
                        ord.TradeAttribute6 = ModifiedGroup.TradeAttribute6;
                        ord.SetTradeAttribute(ModifiedGroup.GetTradeAttributesAsDict());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateDates()
        {
            try
            {
                bool isGroupEditted = false;
                if (ModifiedGroup.AUECLocalDate != _originalGroup.AUECLocalDate)
                {
                    _originalGroup.AUECLocalDate = ModifiedGroup.AUECLocalDate;
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                    isGroupEditted = true;
                }
                if (ModifiedGroup.ProcessDate != _originalGroup.ProcessDate)
                {
                    _originalGroup.ProcessDate = ModifiedGroup.ProcessDate;
                    isGroupEditted = true;
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                }
                if (ModifiedGroup.OriginalPurchaseDate != _originalGroup.OriginalPurchaseDate)
                {
                    _originalGroup.OriginalPurchaseDate = ModifiedGroup.OriginalPurchaseDate;
                    isGroupEditted = true;
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                }
                if (ModifiedGroup.SettlementDate != _originalGroup.SettlementDate)
                {
                    _originalGroup.SettlementDate = ModifiedGroup.SettlementDate;
                    isGroupEditted = true;
                    _originalGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                }

                if (isGroupEditted)
                    UpdateUnAllocatedGroupTaxlotState();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateTradeAttributes()
        {
            try
            {
                bool isUpdateTradeAttribute = false;
                UpdateModifiedGroupTradeAttributes();
                if (ModifiedGroup.TradeAttribute1 != _originalGroup.TradeAttribute1)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute1 = ModifiedGroup.TradeAttribute1;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                }
                if (ModifiedGroup.TradeAttribute2 != _originalGroup.TradeAttribute2)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute2 = ModifiedGroup.TradeAttribute2;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                }
                if (ModifiedGroup.TradeAttribute3 != _originalGroup.TradeAttribute3)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute3 = ModifiedGroup.TradeAttribute3;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                }
                if (ModifiedGroup.TradeAttribute4 != _originalGroup.TradeAttribute4)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute4 = ModifiedGroup.TradeAttribute4;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                }
                if (ModifiedGroup.TradeAttribute5 != _originalGroup.TradeAttribute5)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute5 = ModifiedGroup.TradeAttribute5;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                }
                if (ModifiedGroup.TradeAttribute6 != _originalGroup.TradeAttribute6)
                {
                    isUpdateTradeAttribute = true;
                    _originalGroup.TradeAttribute6 = ModifiedGroup.TradeAttribute6;
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                }

                foreach (var kvp in ModifiedGroup.GetTradeAttributesAsDict())
                {
                    if (_originalGroup.GetTradeAttributeValue(kvp.Key) != kvp.Value)
                    {
                        isUpdateTradeAttribute = true;
                        _originalGroup.SetTradeAttributeValue(kvp.Key, kvp.Value);

                        string enumName = kvp.Key + "_Changed";
                        if (Enum.TryParse<TradeAuditActionType.ActionType>(enumName, out var actionEnum))
                        {
                            AuditManager.Instance.AddActionToAllGroupAndTaxlots(_originalGroup, actionEnum);
                        }
                    }
                }

                if (isUpdateTradeAttribute)
                    UpdateDataforTradeAttributes();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the state of the un allocated group taxlot.
        /// </summary>
        private void UpdateUnAllocatedGroupTaxlotState()
        {
            try
            {
                if (ModifiedGroup != null && ModifiedGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    //TODO: Taxlot should be created in Taxlot Object
                    TaxLot updatedTaxlot = new TaxLot();
                    updatedTaxlot.TaxLotQty = ModifiedGroup.CumQty;
                    updatedTaxlot.TaxLotID = ModifiedGroup.GroupID;
                    updatedTaxlot.GroupID = ModifiedGroup.GroupID;
                    updatedTaxlot.SideMultiplier = Calculations.GetSideMultilpier(ModifiedGroup.OrderSideTagValue);
                    updatedTaxlot.CopyBasicDetails((PranaBasicMessage)ModifiedGroup);
                    _originalGroup.UpdateTaxlotState(updatedTaxlot);
                    if (_originalGroup.Orders.Count == 1)
                    {
                        _originalGroup.Orders[0].IsModified = true;
                        _originalGroup.Orders[0].AvgPrice = ModifiedGroup.AvgPrice;
                        _originalGroup.Orders[0].CumQty = ModifiedGroup.CumQty;
                        _originalGroup.Orders[0].Description = ModifiedGroup.Description;
                        _originalGroup.Orders[0].InternalComments = ModifiedGroup.InternalComments;
                        _originalGroup.Orders[0].AUECLocalDate = ModifiedGroup.AUECLocalDate;
                        _originalGroup.Orders[0].OriginalPurchaseDate = ModifiedGroup.OriginalPurchaseDate;
                        _originalGroup.Orders[0].ProcessDate = ModifiedGroup.ProcessDate;
                        _originalGroup.Orders[0].SettlementDate = ModifiedGroup.SettlementDate;
                        _originalGroup.Orders[0].Venue = ModifiedGroup.Venue;
                        _originalGroup.Orders[0].VenueID = ModifiedGroup.VenueID;
                        _originalGroup.Orders[0].CounterPartyID = ModifiedGroup.CounterPartyID;
                        _originalGroup.Orders[0].CounterPartyName = ModifiedGroup.CounterPartyName;
                        _originalGroup.Orders[0].OrderSideTagValue = ModifiedGroup.OrderSideTagValue;
                        _originalGroup.Orders[0].OrderSide = ModifiedGroup.OrderSide;
                        _originalGroup.Orders[0].FXRate = ModifiedGroup.FXRate;
                        _originalGroup.Orders[0].FXConversionMethodOperator = ModifiedGroup.FXConversionMethodOperator;
                    }
                    else
                    {
                        foreach (AllocationOrder order in _originalGroup.Orders)
                        {
                            order.OrderSideTagValue = ModifiedGroup.OrderSideTagValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Validate dates
        /// </summary>
        /// <returns></returns>
        private string ValidateDates()
        {
            StringBuilder message = new StringBuilder();
            try
            {
                if (ModifiedGroup.AllocatedQty == 0)
                {
                    message.AppendLine("Changes will not be applied as: ");

                    bool isValid = true;
                    if (ModifiedGroup.SettlementDate.Date < ModifiedGroup.ProcessDate.Date)
                    {
                        message.AppendLine();
                        message.AppendLine("Settlement Date can not be less than Process Date");
                        isValid = false;
                    }
                    if (ModifiedGroup.ProcessDate.Date < ModifiedGroup.AUECLocalDate.Date)
                    {
                        message.AppendLine();
                        message.AppendLine("Process Date cannot be less than Trade Date");
                        isValid = false;
                    }
                    if (ModifiedGroup.ProcessDate.Date < ModifiedGroup.OriginalPurchaseDate.Date)
                    {
                        message.AppendLine();
                        message.Append("OriginalPurchase Date cannot be greater than Process Date");
                        isValid = false;
                    }
                    if (isValid)
                        message.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return message.ToString();
        }

        /// <summary>
        /// Called when [side changed from side panel].
        /// </summary>
        private void OnSideChangedFromSidePanel()
        {
            try
            {
                if (ModifiedGroup.OrderSide != SelectedSide.Value && !(String.IsNullOrWhiteSpace(ModifiedGroup.OrderSide) && SelectedSide.Value == null))
                {
                    if (Sides.ContainsValue(ModifiedGroup.OrderSide))
                        ModifiedGroup.OrderSideTagValue = SelectedSide.Key;

                    if (EditTradeHelper.SetTransactionTypeBasedOnSide(ModifiedGroup))
                        ModifiedGroup.TransactionType = Regex.Replace(SelectedSide.Value, @"\s+", "");

                    RaisePropertyChangedEvent("ModifiedGroup");
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Enables the settlement currency dropdown.
        /// Adds the base local settlement currency to the dropdown.
        /// </summary>
        /// <param name="group">The group.</param>
        private void EnableSelectSettlementCurrency(AllocationGroup group)
        {
            try
            {
                Dictionary<int, string> CurrencyPair = new Dictionary<int, string>(CachedDataManager.GetInstance.GetAllCurrencies());

                if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() != group.CurrencyID)
                {
                    EnableCurrencyDependencies = true;

                    CurrencyPair = CurrencyPair.Where(
                    od => od.Key == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() ||
                        od.Key == group.CurrencyID).ToDictionary(od => od.Key, od => od.Value);

                    this.SettlCurrency = new ObservableDictionary<int, string>(CurrencyPair);
                }
                else
                {
                    EnableCurrencyDependencies = false;

                    CurrencyPair = CurrencyPair.Where(
                    od => od.Key == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()
                    ).ToDictionary(od => od.Key, od => od.Value);

                    this.SettlCurrency = new ObservableDictionary<int, string>(CurrencyPair);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Populates the AllTradeAttributes list in column-major order while the grid expects row-major data.
        /// </summary>
        private void PopulateTradeAttributes()
        {
            // Display in column-major order while the grid expects row-major data.
            const int rows = 15;
            const int columns = 3;
            const int totalAttributes = rows * columns;

            for (int i = 0; i < totalAttributes; i++)
            {
                int row = i / columns;
                int col = i % columns;

                int inputIndex = col * rows + row;  // column-major index
                int gridIndex = i;                  // row-major index (same as loop index)

                AllTradeAttributes.Add(new TradeAttributeViewModel
                {
                    Label = CachedDataManager.GetInstance.GetAttributeNameForValue($"{AllocationUIConstants.CAPTION_TradeAttribute}{inputIndex + 1}"),
                    AttributeName = $"{AllocationUIConstants.TradeAttribute}{inputIndex + 1}",
                    CmbAutomationName = $"{AllocationUIConstants.TradeAttribute}{inputIndex + 1}",
                    AttributeNumber = inputIndex + 1
                });
            }
        }

        /// <summary>
        /// Sets the SelectedValue for each TradeAttributeViewModel in AllTradeAttributes.
        /// For the first six attributes, values are explicitly mapped from ModifiedGroup properties.
        /// For all others, values are retrieved dynamically using the attribute name.
        /// </summary>
        /// <param name="value">Object used to fetch dynamic attribute values beyond the first six.</param>
        private void SetTradeAttributeSelectedValues(AllocationGroup value)
        {
            foreach (var item in AllTradeAttributes)
            {

                switch (item.Index)
                {
                    case 0:
                        item.SelectedValue = value.TradeAttribute1;
                        break;
                    case 1:
                        item.SelectedValue = value.TradeAttribute2;
                        break;
                    case 2:
                        item.SelectedValue = value.TradeAttribute3;
                        break;
                    case 3:
                        item.SelectedValue = value.TradeAttribute4;
                        break;
                    case 4:
                        item.SelectedValue = value.TradeAttribute5;
                        break;
                    case 5:
                        item.SelectedValue = value.TradeAttribute6;
                        break;
                    default:
                        item.SelectedValue = value.GetTradeAttributeValue(item.AttributeName);
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the ModifiedGroup's trade attribute values based on the current selection in AllTradeAttributes.
        /// The first six attributes are mapped explicitly to dedicated properties.
        /// Remaining attributes are updated using a dynamic setter method.
        /// </summary>
        private void UpdateModifiedGroupTradeAttributes()
        {
            foreach (var item in AllTradeAttributes)
            {
                switch (item.Index)
                {
                    case 0:
                        ModifiedGroup.TradeAttribute1 = item.SelectedValue;
                        break;
                    case 1:
                        ModifiedGroup.TradeAttribute2 = item.SelectedValue;
                        break;
                    case 2:
                        ModifiedGroup.TradeAttribute3 = item.SelectedValue;
                        break;
                    case 3:
                        ModifiedGroup.TradeAttribute4 = item.SelectedValue;
                        break;
                    case 4:
                        ModifiedGroup.TradeAttribute5 = item.SelectedValue;
                        break;
                    case 5:
                        ModifiedGroup.TradeAttribute6 = item.SelectedValue;
                        break;
                    default:
                        // Dynamically sets attribute value for attributes beyond the first six.
                        ModifiedGroup.SetTradeAttributeValue(item.AttributeName, item.SelectedValue);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads and initializes selected trade attribute values
        /// </summary>
        public void LoadTradeAttributes()
        {
            SetTradeAttributeSelectedValues(ModifiedGroup);
        }

        #endregion Methods
    }
}
