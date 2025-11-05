using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Allocation.Views;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class CommissionFeesBulkChangeControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [bulk change group].
        /// </summary>
        public event EventHandler<EventArgs<BulkChangesGroupLevel>> BulkChangeGroup;

        /// <summary>
        /// Occurs when [re calculate commission].
        /// </summary>
        public event EventHandler<EventArgs<CommissionRule, bool>> ReCalculateCommission;

        #endregion Events

        #region Members

        /// <summary>
        /// The _third party details
        /// </summary>
        private ObservableDictionary<int, string> _thirdPartyDetails;

        /// <summary>
        /// The _selected third party details
        /// </summary>
        private KeyValuePair<int, string> _selectedThirdPartyDetails;

        /// <summary>
        /// The _master funds
        /// </summary>
        private ObservableDictionary<int, string> _masterFunds;

        /// <summary>
        /// The _selected master funds
        /// </summary>
        private KeyValuePair<int, string> _selectedMasterFunds;

        /// <summary>
        /// The _counter parties
        /// </summary>
        private ObservableDictionary<int, string> _counterParties;

        /// <summary>
        /// The _selected counter parties
        /// </summary>
        private KeyValuePair<int, string> _selectedCounterParties;

        /// <summary>
        /// The _calculation basis
        /// </summary>
        private ObservableDictionary<int, string> _calculationBasis;

        /// <summary>
        /// The _commission rules
        /// </summary>
        private ObservableCollection<CommissionRule> _commissionRules;

        /// <summary>
        /// The _selected commission rule
        /// </summary>
        private CommissionRule _selectedCommissionRule;

        /// <summary>
        /// The _FX conversion operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _fxConversionOperator;

        /// <summary>
        /// The _selected fx conversion operator
        /// </summary>
        private EnumerationValue _selectedFxConversionOperator;

        /// <summary>
        /// The _account pb details
        /// </summary>
        private Dictionary<int, string> _accountPBDetails;

        /// <summary>
        /// The _list box data
        /// </summary>
        private ObservableCollection<string> _listBoxData;

        /// <summary>
        /// The _enable average price
        /// </summary>
        private bool _enableAvgPrice;

        /// <summary>
        /// The _enable accrued interest
        /// </summary>
        private bool _enableAccruedInterest;

        /// <summary>
        /// The _enable broker
        /// </summary>
        private bool _enableBroker;

        /// <summary>
        /// The _enable internal comments
        /// </summary>
        private bool _enableInternalComments;

        /// <summary>
        /// The _enable description
        /// </summary>
        private bool _enableDescription;

        /// <summary>
        /// The _enable fx rate
        /// </summary>
        private bool _enableFxRate;

        /// <summary>
        /// The _enable fx rate operator
        /// </summary>
        private bool _enableFxRateOperator;

        /// <summary>
        /// The _enable average price rounding
        /// </summary>
        private bool _enableAvgPriceRounding;

        /// <summary>
        /// The _enable all
        /// </summary>
        private bool _enableAll;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _master fund association
        /// </summary>
        private Dictionary<int, List<int>> _masterFundAssociation;

        /// <summary>
        /// The _avg price editor
        /// </summary>
        private double _avgPriceEditor;

        /// <summary>
        /// The _accrued interest editor
        /// </summary>
        private double _accruedInterestEditor;

        /// <summary>
        /// The _FX rate editor
        /// </summary>
        private double _fxRateEditor;

        /// <summary>
        /// The _avg price rounding editor
        /// </summary>
        private int _avgPriceRoundingEditor;

        /// <summary>
        /// The _description box
        /// </summary>
        private string _descriptionBox;

        /// <summary>
        /// The _internal comments box
        /// </summary>
        private string _internalCommentsBox;

        /// <summary>
        /// The _group level
        /// </summary>
        private bool _groupLevel = true;

        /// <summary>
        /// The _taxlot level
        /// </summary>
        private bool _taxlotLevel;

        /// <summary>
        /// The _default rule
        /// </summary>
        private bool _defaultRule;

        /// <summary>
        /// The _selected accounts list
        /// </summary>
        private string _selectedAccountsList;

        /// <summary>
        /// The _CHKBX select all
        /// </summary>
        private bool _chkbxSelectAll;

        /// <summary>
        /// The _RD BTN specify comm rule
        /// </summary>
        private bool _rdBtnSpecifyCommRule;

        /// <summary>
        /// The _RD BTN select commission rule
        /// </summary>
        private bool _rdBtnSelectCommissionRule;

        /// <summary>
        /// The _commission rule form view model
        /// </summary>
        private CommissionRuleFormViewModel _commissionRuleFormViewModel;

        /// <summary>
        /// The _selected account i ds list
        /// </summary>
        private ObservableCollection<object> _selectedAccountIDsList;

        /// <summary>
        /// The commission rule display
        /// </summary>
        private static CommissionRuleFormViewModel commissionRuleDisplay = null;

        /// <summary>
        /// The commissionrule object
        /// </summary>
        private CommissionRule _specifiedCommissionRule;

        /// <summary>
        /// The _check for whether the unallocated trade is selected
        /// </summary>
        public bool _taxlotEnabled;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account pb details.
        /// </summary>
        /// <value>
        /// The account pb details.
        /// </value>
        public Dictionary<int, string> AccountPBDetails
        {
            get { return _accountPBDetails; }
            set { _accountPBDetails = value; }
        }

        /// <summary>
        /// Gets or sets the accrued interest editor.
        /// </summary>
        /// <value>
        /// The accrued interest editor.
        /// </value>
        public double AccruedInterestEditor
        {
            get { return _accruedInterestEditor; }
            set
            {
                _accruedInterestEditor = value;
                RaisePropertyChangedEvent("AccruedInterestEditor");
            }
        }

        /// <summary>
        /// Gets or sets the average price editor.
        /// </summary>
        /// <value>
        /// The average price editor.
        /// </value>
        public double AvgPriceEditor
        {
            get { return _avgPriceEditor; }
            set
            {
                _avgPriceEditor = value;
                RaisePropertyChangedEvent("AvgPriceEditor");
            }
        }

        /// <summary>
        /// Gets or sets the average price rounding editor.
        /// </summary>
        /// <value>
        /// The average price rounding editor.
        /// </value>
        public int AvgPriceRoundingEditor
        {
            get { return _avgPriceRoundingEditor; }
            set
            {
                _avgPriceRoundingEditor = value;
                RaisePropertyChangedEvent("AvgPriceRoundingEditor");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [CHKBX select all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CHKBX select all]; otherwise, <c>false</c>.
        /// </value>
        public bool ChkbxSelectAll
        {
            get { return _chkbxSelectAll; }
            set
            {
                _chkbxSelectAll = value;
                // sets applied values for commission and fees
                SpecifiedCommissionRule.SetAppliedCommissionAndFees(value);
                RaisePropertyChangedEvent("ChkbxSelectAll");
            }
        }

        /// <summary>
        /// Gets or sets the calculation basis.
        /// </summary>
        /// <value>
        /// The calculation basis.
        /// </value>
        public ObservableDictionary<int, string> CalculationBasis
        {
            get { return _calculationBasis; }
            set
            {
                _calculationBasis = value;
                RaisePropertyChangedEvent("CalculationBasis");
            }
        }

        /// <summary>
        /// Gets or sets the commission rule form view model.
        /// </summary>
        /// <value>
        /// The commission rule form view model.
        /// </value>
        public CommissionRuleFormViewModel CommissionRuleFormViewModel
        {
            get { return _commissionRuleFormViewModel; }
            set
            {
                _commissionRuleFormViewModel = value;
                RaisePropertyChangedEvent("CommissionRuleFormViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the commission rules.
        /// </summary>
        /// <value>
        /// The commission rules.
        /// </value>
        public ObservableCollection<CommissionRule> CommissionRules
        {
            get { return _commissionRules; }
            set
            {
                _commissionRules = value;
                RaisePropertyChangedEvent("CommissionRules");
            }
        }

        /// <summary>
        /// Gets or sets the counter parties.
        /// </summary>
        /// <value>
        /// The counter parties.
        /// </value>
        public ObservableDictionary<int, string> CounterParties
        {
            get { return _counterParties; }
            set
            {
                _counterParties = value;
                RaisePropertyChangedEvent("CounterParties");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [default rule].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [default rule]; otherwise, <c>false</c>.
        /// </value>
        public bool DefaultRule
        {
            get { return _defaultRule; }
            set
            {
                _defaultRule = value;
                RaisePropertyChangedEvent("DefaultRule");
            }
        }

        /// <summary>
        /// Gets or sets the description box.
        /// </summary>
        /// <value>
        /// The description box.
        /// </value>
        public string DescriptionBox
        {
            get { return _descriptionBox; }
            set
            {
                _descriptionBox = value;
                RaisePropertyChangedEvent("DescriptionBox");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable accrued interest].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable accrued interest]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAccruedInterest
        {
            get { return _enableAccruedInterest; }
            set
            {
                _enableAccruedInterest = value;
                RaisePropertyChangedEvent("EnableAccruedInterest");
            }

        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable all]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAll
        {
            get { return _enableAll; }
            set
            {
                _enableAll = value;
                if (_enableAll)
                {
                    EnableAvgPrice = true;
                    EnableBroker = true;
                    EnableDescription = true;
                    EnableFxRate = true;
                    EnableFxRateOperator = true;
                    EnableInternalComments = true;
                    EnableAccruedInterest = true;
                    EnableAvgPriceRounding = true;
                }
                else
                {
                    EnableAvgPrice = false;
                    EnableBroker = false;
                    EnableDescription = false;
                    EnableFxRate = false;
                    EnableFxRateOperator = false;
                    EnableInternalComments = false;
                    EnableAccruedInterest = false;
                    EnableAvgPriceRounding = false;
                }
                RaisePropertyChangedEvent("EnableAll");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable average price].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable average price]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAvgPrice
        {
            get { return _enableAvgPrice; }
            set
            {
                _enableAvgPrice = value;
                RaisePropertyChangedEvent("EnableAvgPrice");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable average price rounding].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable average price rounding]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAvgPriceRounding
        {
            get { return _enableAvgPriceRounding; }
            set
            {
                _enableAvgPriceRounding = value;
                RaisePropertyChangedEvent("EnableAvgPriceRounding");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable broker].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable broker]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableBroker
        {
            get { return _enableBroker; }
            set
            {
                _enableBroker = value;
                RaisePropertyChangedEvent("EnableBroker");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable description].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable description]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDescription
        {
            get { return _enableDescription; }
            set
            {
                _enableDescription = value;
                RaisePropertyChangedEvent("EnableDescription");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable fx rate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable fx rate]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableFxRate
        {
            get { return _enableFxRate; }
            set
            {
                _enableFxRate = value;
                RaisePropertyChangedEvent("EnableFxRate");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable fx rate operator].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable fx rate operator]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableFxRateOperator
        {
            get { return _enableFxRateOperator; }
            set
            {
                _enableFxRateOperator = value;
                RaisePropertyChangedEvent("EnableFxRateOperator");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable internal comments].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable internal comments]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableInternalComments
        {
            get { return _enableInternalComments; }
            set
            {
                _enableInternalComments = value;
                RaisePropertyChangedEvent("EnableInternalComments");
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
        /// Gets or sets the fx rate editor.
        /// </summary>
        /// <value>
        /// The fx rate editor.
        /// </value>
        public double FxRateEditor
        {
            get { return _fxRateEditor; }
            set
            {
                _fxRateEditor = value;
                RaisePropertyChangedEvent("FxRateEditor");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [group level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [group level]; otherwise, <c>false</c>.
        /// </value>
        public bool GroupLevel
        {
            get { return _groupLevel; }
            set
            {
                _groupLevel = value;
                if (_groupLevel)
                    DefaultRule = true;
                RaisePropertyChangedEvent("GroupLevel");
            }
        }

        /// <summary>
        /// Gets or sets the internal comments box.
        /// </summary>
        /// <value>
        /// The internal comments box.
        /// </value>
        public string InternalCommentsBox
        {
            get { return _internalCommentsBox; }
            set
            {
                _internalCommentsBox = value;
                RaisePropertyChangedEvent("InternalCommentsBox");
            }
        }

        /// <summary>
        /// Gets or sets the ListBox data.
        /// </summary>
        /// <value>
        /// The ListBox data.
        /// </value>
        public ObservableCollection<string> ListBoxData
        {
            get { return _listBoxData; }
            set
            {
                _listBoxData = value;
                RaisePropertyChangedEvent("ListBoxData");
            }
        }

        /// <summary>
        /// Gets or sets the master fund association.
        /// </summary>
        /// <value>
        /// The master fund association.
        /// </value>
        public Dictionary<int, List<int>> MasterFundAssociation
        {
            get { return _masterFundAssociation; }
            set { _masterFundAssociation = value; }
        }

        /// <summary>
        /// Gets or sets the master funds.
        /// </summary>
        /// <value>
        /// The master funds.
        /// </value>
        public ObservableDictionary<int, string> MasterFunds
        {
            get { return _masterFunds; }
            set
            {
                _masterFunds = value;
                RaisePropertyChangedEvent("MasterFunds");
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
        /// Gets or sets a value indicating whether [rd BTN select commission rule].
        /// </summary>
        /// <value>
        /// <c>true</c> if [rd BTN select commission rule]; otherwise, <c>false</c>.
        /// </value>
        public bool RdBtnSelectCommissionRule
        {
            get { return _rdBtnSelectCommissionRule; }
            set
            {
                _rdBtnSelectCommissionRule = value;
                RaisePropertyChangedEvent("RdBtnSelectCommissionRule");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [rd BTN specify comm rule].
        /// </summary>
        /// <value>
        /// <c>true</c> if [rd BTN specify comm rule]; otherwise, <c>false</c>.
        /// </value>
        public bool RdBtnSpecifyCommRule
        {
            get { return _rdBtnSpecifyCommRule; }
            set
            {
                _rdBtnSpecifyCommRule = value;
                RaisePropertyChangedEvent("RdBtnSpecifyCommRule");
            }
        }

        /// <summary>
        /// Gets or sets the selected account i ds list.
        /// </summary>
        /// <value>
        /// The selected account i ds list.
        /// </value>
        public ObservableCollection<object> SelectedAccountIDsList
        {
            get { return _selectedAccountIDsList; }
            set
            {
                _selectedAccountIDsList = value;
                RaisePropertyChangedEvent("SelectedAccountIDsList");
            }
        }

        /// <summary>
        /// Gets or sets the selected accounts list.
        /// </summary>
        /// <value>
        /// The selected accounts list.
        /// </value>
        public string SelectedAccountsList
        {
            get { return _selectedAccountsList; }
            set
            {
                _selectedAccountsList = value;
                RaisePropertyChangedEvent("SelectedAccountsList");
            }
        }

        /// <summary>
        /// Gets or sets the selected commission rule.
        /// </summary>
        /// <value>
        /// The selected commission rule.
        /// </value>
        public CommissionRule SelectedCommissionRule
        {
            get { return _selectedCommissionRule; }
            set
            {
                _selectedCommissionRule = value;
                RaisePropertyChangedEvent("SelectedCommissionRule");
            }
        }

        /// <summary>
        /// Gets or sets the selected counter parties.
        /// </summary>
        /// <value>
        /// The selected counter parties.
        /// </value>
        public KeyValuePair<int, string> SelectedCounterParties
        {
            get { return _selectedCounterParties; }
            set
            {
                _selectedCounterParties = value;
                RaisePropertyChangedEvent("SelectedCounterParties");
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
        /// Gets or sets the selected master funds.
        /// </summary>
        /// <value>
        /// The selected master funds.
        /// </value>
        public KeyValuePair<int, string> SelectedMasterFunds
        {
            get { return _selectedMasterFunds; }
            set
            {
                _selectedMasterFunds = value;
                SelectedThirdPartyDetails = new KeyValuePair<int, string>(Int32.MinValue, ApplicationConstants.C_COMBO_SELECT);
                SetAccountCombo();
                RaisePropertyChangedEvent("SelectedMasterFunds");
            }
        }

        /// <summary>
        /// Gets or sets the selected third party details.
        /// </summary>
        /// <value>
        /// The selected third party details.
        /// </value>
        public KeyValuePair<int, string> SelectedThirdPartyDetails
        {
            get { return _selectedThirdPartyDetails; }
            set
            {
                _selectedThirdPartyDetails = value;
                SetAccountCombo();
                RaisePropertyChangedEvent("SelectedThirdPartyDetails");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [Unallocated_trade is checked].
        /// </summary>
        public bool TaxlotEnabled
        {
            get
            {
                return _taxlotEnabled;
            }
            set
            {
                _taxlotEnabled = value;
                RaisePropertyChangedEvent("TaxlotEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [taxlot level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [taxlot level]; otherwise, <c>false</c>.
        /// </value>
        public bool TaxlotLevel
        {
            get { return _taxlotLevel; }
            set
            {
                _taxlotLevel = value;
                if (_taxlotLevel)
                {
                    EnableFxRate = false;
                    EnableFxRateOperator = false;
                    DefaultRule = false;
                    EnableAvgPrice = false;
                    EnableAccruedInterest = false;
                    EnableBroker = false;
                    EnableDescription = false;
                }
                RaisePropertyChangedEvent("TaxlotLevel");
            }
        }

        /// <summary>
        /// Gets or sets the third party details.
        /// </summary>
        /// <value>
        /// The third party details.
        /// </value>
        public ObservableDictionary<int, string> ThirdPartyDetails
        {
            get { return _thirdPartyDetails; }
            set
            {
                _thirdPartyDetails = value;
                RaisePropertyChangedEvent("ThirdPartyDetails");
            }
        }

        /// <summary>
        /// Gets or sets the commission rule.
        /// </summary>
        /// <value>
        /// The commission rule.
        /// </value>
        public CommissionRule SpecifiedCommissionRule
        {
            get { return _specifiedCommissionRule; }
            set
            {
                _specifiedCommissionRule = value;
                RaisePropertyChangedEvent("SpecifiedCommissionRule");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the commission bulk change command.
        /// </summary>
        /// <value>
        /// The commission bulk change command.
        /// </value>
        public RelayCommand<object> CommissionBulkChangeCommand { get; set; }

        /// <summary>
        /// Gets or sets the load commission rule form.
        /// </summary>
        /// <value>
        /// The load commission rule form.
        /// </value>
        public RelayCommand<object> LoadCommissionRuleForm { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommissionFeesBulkChangeControlViewModel"/> class.
        /// </summary>
        public CommissionFeesBulkChangeControlViewModel()
        {
            try
            {
                CommissionBulkChangeCommand = new RelayCommand<object>((parameter) => CommissionBulkChangeUpdate(parameter));
                LoadCommissionRuleForm = new RelayCommand<object>((parameter) => ViewRuleClick(parameter));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Bulks the change groups.
        /// </summary>
        internal void BulkChangeGroups()
        {
            try
            {
                BulkChangesGroupLevel bulkChangeGroups = GetSelectedDataItems();
                if (BulkChangeGroup != null)
                    BulkChangeGroup(this, new EventArgs<BulkChangesGroupLevel>(bulkChangeGroups));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Commissions the bulk change update.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object CommissionBulkChangeUpdate(object parameter)
        {
            try
            {
                if (parameter.ToString() == "UpdateAllBtn")
                {
                    BulkChangeGroups();
                }
                else if (parameter.ToString() == "CalculateBtn")
                {
                    ReCalculateCommissions();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the commissionRuleDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void commissionRuleDisplay_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (commissionRuleDisplay != null)
                    commissionRuleDisplay = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (commissionRuleDisplay != null)
                        commissionRuleDisplay = null;
                    if (_commissionRuleFormViewModel != null)
                        _commissionRuleFormViewModel = null;
                    if (_specifiedCommissionRule != null)
                        _specifiedCommissionRule = null;
                    if (_fxConversionOperator != null)
                        _fxConversionOperator = null;
                    if (_selectedCommissionRule != null)
                        _selectedCommissionRule = null;
                    if (_commissionRules != null)
                        _commissionRules = null;
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
        /// Gets the account associated with mf.
        /// </summary>
        /// <param name="selectedMasterFundId">The selected master fund identifier.</param>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithMF(int selectedMasterFundId)
        {
            try
            {
                List<string> accounts = new List<string>();
                if (!selectedMasterFundId.Equals(int.MinValue))
                {
                    List<int> accountIDs = MasterFundAssociation[selectedMasterFundId];
                    if (accountIDs != null)
                    {
                        foreach (int accountID in accountIDs)
                        {
                            string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                            accounts.Add(accountName);
                        }
                    }
                }
                return new ObservableCollection<string>(accounts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the account associated with mf and pb.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithMFAndPB(int selectedMasterFundId)
        {
            try
            {
                List<string> accounts = new List<string>();
                List<int> pbAccountIDs = GetThirdPartyName();

                if (!selectedMasterFundId.Equals(0) && selectedMasterFundId != int.MinValue)
                {
                    List<int> masterFundIDs = MasterFundAssociation[selectedMasterFundId];

                    if (pbAccountIDs != null)
                    {
                        foreach (int masterFundId in masterFundIDs)
                        {
                            foreach (int pbAccountID in pbAccountIDs)
                            {
                                if (masterFundId.Equals(pbAccountID))
                                {
                                    string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(masterFundId);
                                    accounts.Add(accountName);
                                }
                            }
                        }
                    }
                }
                return new ObservableCollection<string>(accounts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the account associated with pb.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithPB()
        {
            try
            {
                List<string> accounts = new List<string>();
                List<int> pbAccountIDs = GetThirdPartyName();
                if (pbAccountIDs != null)
                {
                    foreach (int accountID in pbAccountIDs)
                    {
                        string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                        accounts.Add(accountName);
                    }
                }
                return new ObservableCollection<string>(accounts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the name of the account identifier by pb.
        /// </summary>
        /// <param name="pbName">Name of the pb.</param>
        /// <returns></returns>
        public List<int> GetAccountIDByPBName(string pbName)
        {
            List<int> accountIDs = new List<int>();
            try
            {
                if (_accountPBDetails != null)
                {
                    foreach (KeyValuePair<int, string> kvp in _accountPBDetails)
                    {
                        if (string.Compare(kvp.Value, pbName, true) == 0)
                        {
                            accountIDs.Add(kvp.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accountIDs;
        }

        /// <summary>
        /// Gets the accounts identifier.
        /// </summary>
        /// <returns></returns>
        private List<int> GetAccountsIDForListBox()
        {
            List<int> accountList = new List<int>();
            try
            {
                if (SelectedAccountIDsList != null)
                {
                    foreach (var item in SelectedAccountIDsList)
                        accountList.Add(CachedDataManager.GetInstance.GetAccountID(item.ToString()));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountList;
        }

        /// <summary>
        /// Gets the specified commission rule.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private CommissionRule GetSpecifiedCommissionRule(out String errorMessage)
        {
            CommissionRule commissionRules = null;
            string error = string.Empty;
            try
            {
                if (SpecifiedCommissionRule.IsValid(out error))
                {
                    commissionRules = SpecifiedCommissionRule;
                    commissionRules.IsCommissionRuleSelected = false;

                    if (ListBoxData != null)
                        commissionRules.AccountIDs = GetAccountsIDForListBox();
                    else
                        commissionRules.AccountIDs = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            errorMessage = error;
            return commissionRules;
        }

        /// <summary>
        /// Gets the selected commission rule.
        /// </summary>
        /// <returns></returns>
        private CommissionRule GetSelectedCommissionRule()
        {
            CommissionRule commissionRules = null;
            try
            {
                if (SelectedCommissionRule != null)
                {
                    commissionRules = SelectedCommissionRule;
                    commissionRules.IsCommissionRuleSelected = true;

                    if (commissionRules.Commission.CommissionRate != double.MinValue)
                        commissionRules.IsCommissionApplied = true;
                    if (commissionRules.SoftCommission.CommissionRate != double.MinValue)
                        commissionRules.IsSoftCommissionApplied = true;

                    commissionRules.IsStampDutyApplied = false;
                    commissionRules.IsClearingFee_AApplied = false;
                    commissionRules.IsMiscFeesApplied = false;
                    commissionRules.IsOccFeeApplied = false;
                    commissionRules.IsOrfFeeApplied = false;
                    commissionRules.IsSecFeeApplied = false;
                    commissionRules.IsTaxonCommissionsApplied = false;
                    commissionRules.IsTransactionLevyApplied = false;


                    if (ListBoxData != null)
                        commissionRules.AccountIDs = GetAccountsIDForListBox();
                    else
                        commissionRules.AccountIDs = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return commissionRules;
        }

        /// <summary>
        /// Gets the selected data items.
        /// </summary>
        /// <returns></returns>
        internal BulkChangesGroupLevel GetSelectedDataItems()
        {
            BulkChangesGroupLevel bulkGroups = new BulkChangesGroupLevel();
            try
            {
                if (GroupLevel)
                {
                    if (_enableAvgPrice == true)
                    {
                        bulkGroups.AvgPrice = (decimal)AvgPriceEditor;
                    }

                    if (_enableAvgPriceRounding == true)
                    {
                        bulkGroups.AvgPriceRounding = AvgPriceRoundingEditor;
                    }

                    if (_enableAccruedInterest == true)
                    {
                        bulkGroups.AccruedInterest = (decimal)AccruedInterestEditor;
                    }

                    if (_enableBroker == true)
                    {
                        bulkGroups.CounterPartyID = SelectedCounterParties.Key;
                    }

                    if (_enableDescription == true)
                    {
                        bulkGroups.Description = DescriptionBox;
                    }

                    if (_enableInternalComments == true)
                    {
                        bulkGroups.InternalComments = InternalCommentsBox;
                    }

                    if (_enableFxRateOperator == true && !_enableFxRateOperator.Equals(string.Empty) && SelectedFxConversionOperator != null)
                    {
                        bulkGroups.FXConversionOperator = SelectedFxConversionOperator.DisplayText.ToString();
                    }

                    if (_enableFxRate == true && FxRateEditor > 0)
                    {
                        bulkGroups.FXRate = (decimal)FxRateEditor;
                    }
                }
                else
                {
                    bulkGroups.GroupWise = false;

                    if (_enableFxRate == true && FxRateEditor > 0)
                    {
                        bulkGroups.FXRate = (decimal)FxRateEditor;
                    }

                    if (_enableFxRateOperator == true && !_enableFxRateOperator.Equals(string.Empty) && SelectedFxConversionOperator != null)
                    {
                        bulkGroups.FXConversionOperator = SelectedFxConversionOperator.DisplayText.ToString();
                    }
                    if (ListBoxData != null)
                    {
                        bulkGroups.AccountIDs = GetAccountsIDForListBox();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return bulkGroups;
        }

        /// <summary>
        /// Gets the name of the third party.
        /// </summary>
        /// <returns></returns>
        private List<int> GetThirdPartyName()
        {
            List<int> accountIDs = null;
            try
            {
                if (SelectedThirdPartyDetails.Value != null)
                {
                    string thirdPartyName = SelectedThirdPartyDetails.Value;
                    if (!thirdPartyName.Equals(ApplicationConstants.C_COMBO_SELECT) && !thirdPartyName.Equals(string.Empty))
                    {
                        accountIDs = GetAccountIDByPBName(thirdPartyName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accountIDs;
        }

        /// <summary>
        /// Called when [load commission fees bulk change control].
        /// </summary>
        /// <param name="accountPBDetails">The account pb details.</param>
        internal void OnLoadCommissionFeesBulkChangeControl(Dictionary<int, string> accountPBDetails)
        {
            try
            {
                Dictionary<int, string> brokerDict = new Dictionary<int, string>();
                brokerDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                brokerDict.AddRangeThreadSafely(Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserCounterParties());

                Dictionary<int, string> commissionDict = new Dictionary<int, string>();
                commissionDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                commissionDict.AddRangeThreadSafely(CommissionEnumHelper.GetOldListForCalculationBasisAsDic());

                Dictionary<int, string> thirdPartyDict = new Dictionary<int, string>();
                thirdPartyDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                thirdPartyDict.AddRangeThreadSafely(CachedDataManager.GetInstance.GetAllThirdParties());

                List<CommissionRule> commissionRuleList = new List<CommissionRule>();
                CommissionRule defaultRule = new CommissionRule();
                defaultRule.RuleID = Guid.Empty;
                defaultRule.RuleName = ApplicationConstants.C_COMBO_SELECT;
                commissionRuleList.Add(defaultRule);
                commissionRuleList.AddRangeThreadSafely(AllocationClientManager.GetInstance().GetAllCommissionRule());

                Dictionary<int, string> masterFundsDict = new Dictionary<int, string>();
                masterFundsDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                masterFundsDict.AddRangeThreadSafely(Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllMasterFunds());

                List<EnumerationValue> fxConversionList = new List<EnumerationValue>();
                fxConversionList.AddRangeThreadSafely(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(Prana.BusinessObjects.AppConstants.Operator)));
                fxConversionList = new List<EnumerationValue>(fxConversionList.Where(x => x.DisplayText != Operator.Multiple.ToString()));

                GroupLevel = true;
                MasterFunds = new ObservableDictionary<int, string>(masterFundsDict);
                ThirdPartyDetails = new ObservableDictionary<int, string>(thirdPartyDict);
                CounterParties = new ObservableDictionary<int, string>(brokerDict);
                CommissionRules = new ObservableCollection<CommissionRule>(commissionRuleList);
                CalculationBasis = new ObservableDictionary<int, string>(commissionDict);
                FxConversionOperator = new ObservableCollection<EnumerationValue>(fxConversionList);
                MasterFundAssociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                AccountPBDetails = accountPBDetails;
                SpecifiedCommissionRule = new CommissionRule(false, Prana.BusinessObjects.AppConstants.CalculationBasis.Auto, 0.0);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Res the calculate commissions.
        /// </summary>
        internal void ReCalculateCommissions()
        {
            try
            {
                CommissionRule commissionRules = null;
                string errorMessage = string.Empty;
                if (RdBtnSpecifyCommRule)
                {
                    commissionRules = GetSpecifiedCommissionRule(out errorMessage);
                }
                else if (RdBtnSelectCommissionRule)
                {
                    commissionRules = GetSelectedCommissionRule();
                }

                if (!String.IsNullOrWhiteSpace(errorMessage))
                {
                    MessageBox.Show(errorMessage, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (ReCalculateCommission != null)
                        ReCalculateCommission(this, new EventArgs<CommissionRule, bool>(commissionRules, GroupLevel));
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
        /// Sets the account combo.
        /// </summary>
        private void SetAccountCombo()
        {
            try
            {
                if (ListBoxData != null)
                    ListBoxData.Clear();

                int selectedMasterFundId = SelectedMasterFunds.Key;
                int selectedThirdPartyId = SelectedThirdPartyDetails.Key;

                if (selectedMasterFundId != int.MinValue && selectedThirdPartyId != int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithMFAndPB(selectedMasterFundId));

                else if (selectedMasterFundId != int.MinValue && selectedThirdPartyId == int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithMF(selectedMasterFundId));

                else if (selectedMasterFundId == int.MinValue && selectedThirdPartyId != int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithPB());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="p">The p.</param>
        internal void SetPreferences(int precisionDigit)
        {
            try
            {
                //set precision format
                PrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(precisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Shows the commission form UI.
        /// </summary>
        /// <param name="showCommissionFormUIViewModel">The show commission form UI view model.</param>
        private void ShowCommissionFormUI(Action<CommissionRuleFormViewModel> showCommissionFormUIViewModel)
        {
            try
            {
                CommissionRule commissionRule = new CommissionRule();
                commissionRule = SelectedCommissionRule;
                if (commissionRule.RuleName != "-Select-")
                {
                    commissionRuleDisplay = new CommissionRuleFormViewModel();
                    commissionRuleDisplay.OnFormCloseButtonEvent += commissionRuleDisplay_OnFormCloseButtonEvent;
                    commissionRuleDisplay.SetRuleDetails(commissionRule);
                    showCommissionFormUIViewModel(commissionRuleDisplay);
                }
                else
                    MessageBox.Show("Please select a commission rule.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Views the rule click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ViewRuleClick(object parameter)
        {
            try
            {
                if (commissionRuleDisplay == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowCommissionFormUI(viewModel => dialogService.ShowDialog<CommissionRuleForm>(this, viewModel));
                }
                else
                    commissionRuleDisplay.BringToFront = WindowState.Normal;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        #endregion Methods
    }
}
