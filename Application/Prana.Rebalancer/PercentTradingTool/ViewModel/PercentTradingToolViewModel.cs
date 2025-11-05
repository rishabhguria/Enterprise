using ExportGridsData;
using Infragistics.Controls.Editors;
using Infragistics.Windows.DataPresenter.Events;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.Classes;
using Prana.Rebalancer.CommonViewModel;
using Prana.Rebalancer.PercentTradingTool.BusinessLogic;
using Prana.Rebalancer.PercentTradingTool.CustomControls;
using Prana.Rebalancer.PercentTradingTool.Preferences;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.ServiceConnector;
using Prana.TradeManager.Extension;
using Prana.UIEventAggregator;
using Prana.UIEventAggregator.Events;
using Prana.Utilities;
using Prana.Utilities.UI;
using Prana.WCFConnectionMgr;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Color = System.Drawing.Color;
using Task = System.Threading.Tasks.Task;

namespace Prana.Rebalancer.PercentTradingTool.ViewModel
{
    /// <summary>
    /// The view model for position sizing tool
    /// </summary>
    public class PercentTradingToolViewModel : BindableBase, IEventAggregatorSubscriber<PTTExpnlStatus>, IDisposable, IExportGridData
    {
        /// <summary>
        /// The PTT request binding list collection view
        /// </summary>
        private BindingListCollectionView _pttRequestBindingListCollectionView;

        private SynchronizationContext _uiSyncContext;

        /// <summary>
        /// The PTT response binding list collection view
        /// </summary>
        private BindingListCollectionView _pttResponseBindingListCollectionView;

        /// <summary>
        /// The _type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _typeCollection;

        /// <summary>
        /// The _calculation value collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _masterFundOrAccount;

        /// <summary>
        /// The _consolidation value collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _combineAccountsTotalValueCollection;

        /// <summary>
        /// The _consolidation value collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _roundLotPreferenceValueCollection;

        /// <summary>
        /// The _consolidation value collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _custodianBrokerPreferenceValueCollection;

        /// <summary>
        /// The PTT change type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _pttChangeTypeCollection;

        /// <summary>
        /// The _user account collection
        /// </summary>
        private ObservableCollection<Account> _userAccountCollection;

        /// <summary>
        /// The _cell type collection
        /// </summary>
        private ObservableDictionary<string, string> _cellTypeCollection;

        /// <summary>
        /// The _security master
        /// </summary>
        private ISecurityMasterServices _securityMaster;

        /// <summary>
        /// The Security Master Sync Service
        /// </summary>
        ProxyBase<ISecMasterSyncServices> _secMasterSyncService;

        /// <summary>
        /// The _is symbol validated
        /// </summary>
        private bool _isSymbolValidated;

        /// <summary>
        /// The _xam validation helper
        /// </summary>
        private XamValidationHelper _xamValidationHelper = new XamValidationHelper();

        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol = string.Empty;

        /// <summary>
        /// The _price
        /// </summary>
        private double _price;

        /// <summary>
        /// The _account list
        /// </summary>
        private List<int> _accountList;

        /// <summary>
        /// The _symbol description
        /// </summary>
        private string _symbolDescription;

        /// <summary>
        /// The _error object
        /// </summary>
        private WPFErrorObject _errorObject = new WPFErrorObject();

        /// <summary>
        /// The _ask
        /// </summary>
        private string _ask = PTTConstants.LIT_DEFAULT_FEED_VALUES;

        /// <summary>
        /// The _last
        /// </summary>
        private string _last = PTTConstants.LIT_DEFAULT_FEED_VALUES;

        /// <summary>
        /// The _bid
        /// </summary>
        private string _bid = PTTConstants.LIT_DEFAULT_FEED_VALUES;

        /// <summary>
        /// The _vwap
        /// </summary>
        private string _vwap = PTTConstants.LIT_DEFAULT_FEED_VALUES;

        /// <summary>
        /// The _in progress
        /// </summary>
        private bool _inProgress;

        /// <summary>
        /// The _is calculations completed
        /// </summary>
        private bool _enableGrid;

        /// <summary>
        /// The _enable Export
        /// </summary>
        private bool _enableExport;

        /// <summary>
        /// The _decimal precision
        /// </summary>
        private int _decimalPrecision;

        /// <summary>
        /// The _is data recalculated
        /// </summary>
        private bool _isDataRecalculated;

        /// <summary>
        /// The _status message
        /// </summary>
        private string _statusMessage = String.Empty;

        /// <summary>
        /// The _is expnl service connected
        /// </summary>
        private bool _isExpnlServiceConnected;

        /// <summary>
        /// The _edit mode original cell value
        /// </summary>
        private string _editModeOriginalCellValue;

        /// <summary>
        /// The is live feed allowed
        /// </summary>
        private bool _isLiveFeedAllowed;

        private CompanyUser _loginUser;

        private bool _exportGrid;

        /// <summary>
        /// The foreground color of security description
        /// </summary>
        private Color _symbolDescriptionColor;

        /// <summary>
        /// The is check compliance allowed
        /// </summary>
        private bool _isCheckComplianceAllowed;
        /// <summary>
        /// The master fund collection
        /// </summary>
        private ObservableDictionary<int, string> _masterFundCollection;
        /// <summary>
        /// The is account visble
        /// </summary>
        private bool _isAccountVisble = false;
        /// <summary>
        /// The is round lot visble
        /// </summary>
        private bool _isRoundLotVisble = false;
        /// <summary>
        /// The is mastser fund visible
        /// </summary>
        private bool _isMastserFundVisible;
        /// <summary>
        /// Check whether custodian pref is enabled or not
        /// </summary>
        private bool _isCustodianColVisible;
        /// <summary>
        /// The company master fund collection
        /// </summary>
        private ObservableCollection<Account> _companyMasterFundCollection;
        /// <summary>
        /// The is equity symbol
        /// </summary>
        private bool _isEquitySymbol;
        /// <summary>
        /// The company accounts collection
        /// </summary>
        private ObservableCollection<Account> _companyAccountsCollection;
        /// <summary>
        /// The account text
        /// </summary>
        private string _accountText;
        /// <summary>
        /// The mf or acc from pm
        /// </summary>
        private PTTMasterFundOrAccount _mfOrAccFromPM;
        /// <summary>
        /// The master fund nav text
        /// </summary>
        private string _masterFundNavText;

        private string _startingPercentageOrBasisPointText;

        /// <summary>
        /// variable used to enable or disable the Percentage Type in PTT on changing +/-/=
        /// When PTT is started, the default +/-/= is Increase so the Percentage type must be enabled.
        /// </summary>
        private bool _percentageTypeEnableDisable = true;

        /// <summary>
        /// variable used to enable or disable the Ending Percentage in PTT on changing +/-/=
        /// When PTT is started, the default +/-/= is Increase so the Ending Percentage must be disabled.
        /// </summary>
        private bool _endingPercentageEnableDisable = false;

        private string _percentageOrBasisPointChangeText;

        private string _endingPercentageOrPointBasisText;
        /// <summary>
        /// The temporary add set collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _tempAddSetCollection;

        /// <summary>
        /// whether to enable
        /// </summary>
        private bool _enableButtons;

        private ObservableCollection<string> _symbolList = new ObservableCollection<string>();
        public ObservableCollection<string> SymbolList
        {
            get { return _symbolList; }
            set
            {
                _symbolList = value;
                RaisePropertyChangedEvent("SymbolList");
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is equity symbol.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is equity symbol; otherwise, <c>false</c>.
        /// </value>
        public bool IsEquitySymbol
        {
            get { return _isEquitySymbol; }
            set { SetProperty(ref _isEquitySymbol, value); }
        }
        /// <summary>
        /// Gets or sets the decimal precision.
        /// </summary>
        /// <value>
        /// The decimal precision.
        /// </value>
        public int DecimalPrecision
        {
            get { return _decimalPrecision; }
            set { SetProperty(ref _decimalPrecision, value); }
        }

        /// <summary>
        /// Gets or sets the PTT change type collection.
        /// </summary>
        /// <value>
        /// The PTT change type collection.
        /// </value>
        public ObservableCollection<EnumerationValue> PTTAddOrSetCollection
        {
            get { return _pttChangeTypeCollection; }
            set { SetProperty(ref _pttChangeTypeCollection, value); }
        }

        /// <summary>
        /// Gets or sets the PTT request binding list collection view.
        /// </summary>
        /// <value>
        /// The PTT request binding list collection view.
        /// </value>
        public BindingListCollectionView PTTRequestBindingListCollectionView
        {
            get { return _pttRequestBindingListCollectionView; }
            set { SetProperty(ref _pttRequestBindingListCollectionView, value); }
        }

        /// <summary>
        /// Gets or sets the PTT response binding list collection view.
        /// </summary>
        /// <value>
        /// The PTT response binding list collection view.
        /// </value>
        public BindingListCollectionView PTTResponseBindingListCollectionView
        {
            get { return _pttResponseBindingListCollectionView; }
            set { SetProperty(ref _pttResponseBindingListCollectionView, value); }
        }

        /// <summary>
        /// Gets or sets the type collection.
        /// </summary>
        /// <value>
        /// The type collection.
        /// </value>
        public ObservableCollection<EnumerationValue> TypeCollection
        {
            get { return _typeCollection; }
            set { SetProperty(ref _typeCollection, value); }
        }

        /// <summary>
        /// Gets or sets the calculation value collection.
        /// </summary>
        /// <value>
        /// The calculation value collection.
        /// </value>
        public ObservableCollection<EnumerationValue> MasterFundOrAccount
        {
            get { return _masterFundOrAccount; }
            set { SetProperty(ref _masterFundOrAccount, value); }
        }

        /// <summary>
        /// Gets or sets the consolidation value collection.
        /// </summary>
        /// <value>
        /// The consolidation value collection.
        /// </value>
        public ObservableCollection<EnumerationValue> CombinedAccountsTotalValueCollection
        {
            get { return _combineAccountsTotalValueCollection; }
            set { SetProperty(ref _combineAccountsTotalValueCollection, value); }
        }

        /// <summary>
        /// Gets or sets the consolidation value collection.
        /// </summary>
        /// <value>
        /// The consolidation value collection.
        /// </value>
        public ObservableCollection<EnumerationValue> RoundLotPreferenceValueCollection
        {
            get { return _roundLotPreferenceValueCollection; }
            set { SetProperty(ref _roundLotPreferenceValueCollection, value); }
        }

        /// <summary>
        /// Gets or sets the consolidation value collection.
        /// </summary>
        /// <value>
        /// The consolidation value collection.
        /// </value>
        public ObservableCollection<EnumerationValue> CustodianBrokerPreferenceValueCollection
        {
            get { return _custodianBrokerPreferenceValueCollection; }
            set { SetProperty(ref _custodianBrokerPreferenceValueCollection, value); }
        }

        /// <summary>
        /// Gets or sets the user account collection.
        /// </summary>
        /// <value>
        /// The user account collection.
        /// </value>
        public ObservableCollection<Account> UserAccountCollection
        {
            get { return _userAccountCollection; }
            set { SetProperty(ref _userAccountCollection, value); }
        }

        /// <summary>
        /// Gets or sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            get { return _securityMaster; }
            set { SetProperty(ref _securityMaster, value); }
        }

        public ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set { SetProperty(ref _secMasterSyncService, value); }
        }

        /// <summary>
        /// The is ticker symbology{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        private ApplicationConstants.SymbologyCodes _symbology = SymbologyHelper.DefaultSymbology;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ticker symbology.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is ticker symbology; otherwise, <c>false</c>.
        /// </value>
        public ApplicationConstants.SymbologyCodes Symbology
        {
            get { return _symbology; }
            set { SetProperty(ref _symbology, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is symbol validated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is symbol validated; otherwise, <c>false</c>.
        /// </value>
        public bool IsSymbolValidated
        {
            get { return _isSymbolValidated; }
            set { SetProperty(ref _isSymbolValidated, value); }
        }

        private string _symbolUI = string.Empty;

        /// <summary>
        /// Gets or sets the symbol for UI.
        /// </summary>
        /// <value>
        /// The symbol for UI.
        /// </value>
        public string SymbolUI
        {
            get { return _symbolUI; }
            set { SetProperty(ref _symbolUI, value); }
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }

        /// <summary>
        /// Gets or sets the symbol description.
        /// </summary>
        /// <value>
        /// The symbol description.
        /// </value>
        public string SymbolDescription
        {
            get { return _symbolDescription; }
            set { SetProperty(ref _symbolDescription, value); }
        }

        /// <summary>
        /// Gets or sets the account list.
        /// </summary>
        /// <value>
        /// The account list.
        /// </value>
        public List<int> AccountList
        {
            get { return _accountList; }
            set { SetProperty(ref _accountList, value); }
        }

        /// <summary>
        /// Gets or sets the ask.
        /// </summary>
        /// <value>
        /// The ask.
        /// </value>
        public string Ask
        {
            get { return _ask; }
            set { SetProperty(ref _ask, value); }
        }

        /// <summary>
        /// Gets or sets the last.
        /// </summary>
        /// <value>
        /// The last.
        /// </value>
        public string Last
        {
            get { return _last; }
            set { SetProperty(ref _last, value); }
        }

        /// <summary>
        /// Gets or sets the bid.
        /// </summary>
        /// <value>
        /// The bid.
        /// </value>
        public string Bid
        {
            get { return _bid; }
            set { SetProperty(ref _bid, value); }
        }

        /// <summary>
        /// Gets or sets the vwap.
        /// </summary>
        /// <value>
        /// The vwap.
        /// </value>
        public string VWAP
        {
            get { return _vwap; }
            set { SetProperty(ref _vwap, value); }
        }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [in progress].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in progress]; otherwise, <c>false</c>.
        /// </value>
        public bool InProgress
        {
            get { return _inProgress; }
            set { SetProperty(ref _inProgress, value); }
        }

        private string _inProgressMessage;
        public string InProgressMessage
        {
            get { return _inProgressMessage; }
            set { SetProperty(ref _inProgressMessage, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is calculations completed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is calculations completed; otherwise, <c>false</c>.
        /// </value>
        public bool EnableGrid
        {
            get { return _enableGrid; }
            set { SetProperty(ref _enableGrid, value); }
        }

        /// <summary>
        /// Gets or sets the value of Export functionallity.
        /// </summary>
        public bool EnableExport
        {
            get { return _enableExport; }
            set { SetProperty(ref _enableExport, value); }
        }

        /// <summary>
        /// Gets or sets the error object.
        /// </summary>
        /// <value>
        /// The error object.
        /// </value>
        public WPFErrorObject ErrorObject
        {
            get { return _errorObject; }
            set { _errorObject = value; }
        }

        /// <summary>
        /// Gets or sets the validation helper.
        /// </summary>
        /// <value>
        /// The validation helper.
        /// </value>
        public XamValidationHelper ValidationHelper
        {
            get { return _xamValidationHelper; }
            set { SetProperty(ref _xamValidationHelper, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data recalculated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is data recalculated; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataRecalculated
        {
            get { return _isDataRecalculated; }
            set { SetProperty(ref _isDataRecalculated, value); }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the cell type collection.
        /// </summary>
        /// <value>
        /// The cell type collection.
        /// </value>
        public ObservableDictionary<string, string> CellTypeCollection
        {
            get { return _cellTypeCollection; }
            set { SetProperty(ref _cellTypeCollection, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expnl service connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expnl service connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpnlServiceConnected
        {
            get { return _isExpnlServiceConnected; }
            set { SetProperty(ref _isExpnlServiceConnected, value); }
        }

        /// <summary>
        /// The is live feed allowed
        /// </summary>
        public bool IsLiveFeedAllowed
        {
            get { return _isLiveFeedAllowed; }
            set { SetProperty(ref _isLiveFeedAllowed, value); }
        }

        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { SetProperty(ref _loginUser, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is check compliance allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is check compliance allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckComplianceAllowed
        {
            get { return _isCheckComplianceAllowed; }
            set { SetProperty(ref _isCheckComplianceAllowed, value); }
        }
   
        /// <summary>
        /// The foreground color of security description
        /// </summary>
        public Color SymbolDescriptionColor
        {
            get { return _symbolDescriptionColor; }
            set { SetProperty(ref _symbolDescriptionColor, value); }
        }

        public bool ExportGrid
        {
            get { return _exportGrid; }
            set
            {
                _exportGrid = value;
                OnPropertyChanged("ExportGrid");
            }
        }

        public bool IsAccountVisible
        {
            get { return _isAccountVisble; }
            set { SetProperty(ref _isAccountVisble, value); }
        }

        public bool IsRoundLotVisible
        {
            get { return _isRoundLotVisble; }
            set { SetProperty(ref _isRoundLotVisble, value); }
        }

        public bool IsMasterFundVisible
        {
            get { return _isMastserFundVisible; }
            set { SetProperty(ref _isMastserFundVisible, value); }
        }

        public bool IsCustodianColVisible
        {
            get { return _isCustodianColVisible; }
            set { SetProperty(ref _isCustodianColVisible, value); }
        }

        public ObservableCollection<Account> CompanyMasterFundCollection
        {
            get { return _companyMasterFundCollection; }
            set { SetProperty(ref _companyMasterFundCollection, value); }
        }

        public ObservableCollection<Account> CompanyAccountsCollection
        {
            get { return _companyAccountsCollection; }
            set { SetProperty(ref _companyAccountsCollection, value); }
        }


        /// <summary>
        /// Gets or sets the account text.
        /// </summary>
        /// <value>
        /// The account text.
        /// </value>
        public string AccountText
        {
            get { return _accountText; }
            set { SetProperty(ref _accountText, value); }
        }

        /// <summary>
        /// Gets or sets the master fund or account from PM .
        /// </summary>
        /// <value>
        /// The bool value whether account or master fund.
        /// </value>
        public PTTMasterFundOrAccount MFOrAccFromPM
        {
            get { return _mfOrAccFromPM; }
            set { SetProperty(ref _mfOrAccFromPM, value); }
        }

        /// <summary>
        /// Gets or sets the temporary add set collection.
        /// </summary>
        /// <value>
        /// The temporary add set collection.
        /// </value>
        public ObservableCollection<EnumerationValue> TempAddSetCollection
        {
            get { return _tempAddSetCollection; }
            set { SetProperty(ref _tempAddSetCollection, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can execute.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </value>
        private bool CanExecute
        {
            get { return !(ErrorObject.ErrorCount > 0); }
        }

        /// <summary>
        /// Gets or sets the calculate button clicked.
        /// </summary>
        /// <value>
        /// The calculate button clicked.
        /// </value>
        public ICommand CalculateButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the pstui loaded.
        /// </summary>
        /// <value>
        /// The pstui loaded.
        /// </value>
        public ICommand PTTUILoaded { get; set; }

        /// <summary>
        /// Gets or sets the trade clicked.
        /// </summary>
        /// <value>
        /// The trade clicked.
        /// </value>
        public ICommand TradeClicked { get; set; }

        /// <summary>
        /// Gets or sets the check compliance button clicked.
        /// </summary>
        /// <value>
        /// The check compliance button clicked.
        /// </value>
        public ICommand CheckComplianceButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets the recalculate editable cell.
        /// </summary>
        /// <value>
        /// The recalculate editable cell.
        /// </value>
        public ICommand RecalculateEditableCellCommand { get; set; }

        /// <summary>
        /// Gets or sets the stage clicked.
        /// </summary>
        /// <value>
        /// The stage clicked.
        /// </value>
        public ICommand CreateOrderClicked { get; set; }

        /// <summary>
        /// Gets or sets the reload PTT UI.
        /// </summary>
        /// <value>
        /// The reload PTT UI.
        /// </value>
        public ICommand ReloadPttUI { get; set; }

        /// <summary>
        /// Gets or sets the close pstui.
        /// </summary>
        /// <value>
        /// The close pstui.
        /// </value>
        public ICommand ClosePTTUI { get; set; }

        /// <summary>
        /// Gets or sets the try expnl service connect command.
        /// </summary>
        /// <value>
        /// The try expnl service connect command.
        /// </value>
        public ICommand TryExpnlServiceConnectCommand { get; set; }

        /// <summary>
        /// Gets or sets the record old cell value.
        /// </summary>
        /// <value>
        /// The record old cell value.
        /// </value>
        public ICommand RecordOldEditableCellValue { get; set; }

        /// <summary>
        /// Gets or sets the recalculation needed command.
        /// </summary>
        /// <value>
        /// The recalculation needed command.
        /// </value>
        public ICommand RecalculationNeededCommand { get; set; }

        /// <summary>
        /// Gets or sets the display master fund or account command.
        /// </summary>
        /// <value>
        /// The display master fund or account command.
        /// </value>
        public ICommand DisplayMasterFundOrAccountCommand { get; set; }

        public ICommand DisplayPercentageOrBasisPointCommand { get; set; }

        /// <summary>
        /// Gets or sets the export button clicked.
        /// </summary>
        /// <value>
        /// The export button clicked.
        /// </value>
        public ICommand ExportClicked { get; set; }

        /// <summary>
        /// preference clicked command.
        /// </summary>
        /// <value>
        /// The preference clicked.
        /// </value>
        public ICommand PreferenceClicked { get; set; }

        public ICommand EnableEndTargetPercentageCommand { get; set; }

        /// <summary>
        /// Gets or sets the user account collection.
        /// </summary>
        /// <value>
        /// The user account collection.
        /// </value>
        public ObservableDictionary<int, string> MasterFundCollection
        {
            get { return _masterFundCollection; }
            set { SetProperty(ref _masterFundCollection, value); }
        }

        public DelegateCommand<object> TickerKeyUpCommand { get; set; }
        public DelegateCommand ValidateSecurityCommand { get; set; }
        public DelegateCommand DropDownOpenedCommand { get; set; }

        private ObservableCollection<SymbolModel> _lstSuggestedSymbol = new ObservableCollection<SymbolModel>();

        public ObservableCollection<SymbolModel> LstSuggestedSymbol
        {
            get { return _lstSuggestedSymbol; }
            set
            {
                _lstSuggestedSymbol = value;
                OnPropertyChanged();
            }
        }
        private bool _isSymbolDropDownOpen = false;

        public bool IsSymbolDropDownOpen
        {
            get { return _isSymbolDropDownOpen; }
            set
            {
                _isSymbolDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        private MarketDataHelper _marketDataHelperInstance = MarketDataHelper.GetInstance();

        /// <summary>
        /// Gets or sets the Master Nav Text.
        /// </summary>
        /// <value>
        /// The Master Nav Text.
        /// </value>
        public string MasterFundNavText
        {
            get { return _masterFundNavText; }
            set { SetProperty(ref _masterFundNavText, value); }
        }

        public string StartingPercentageOrBasisPointText
        {
            get { return _startingPercentageOrBasisPointText; }
            set { SetProperty(ref _startingPercentageOrBasisPointText, value); }
        }

        public bool PercentageTypeEnableDisable
        {
            get { return _percentageTypeEnableDisable; }
            set { SetProperty(ref _percentageTypeEnableDisable, value); }
        }

        public bool EndingPercentageEnableDisable
        {
            get { return _endingPercentageEnableDisable; }
            set { SetProperty(ref _endingPercentageEnableDisable, value); }
        }

        public string PercentageOrBasisPointChangeText
        {
            get { return _percentageOrBasisPointChangeText; }
            set { SetProperty(ref _percentageOrBasisPointChangeText, value); }
        }

        public string EndingPercentageOrPointBasisText
        {
            get { return _endingPercentageOrPointBasisText; }
            set { SetProperty(ref _endingPercentageOrPointBasisText, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable buttons].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable buttons]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableButtons
        {
            get { return _enableButtons; }
            set { SetProperty(ref _enableButtons, value); }
        }

        /// <summary>
        /// property to Export data for automation 
        /// </summary>
        private bool _exportDataGridForAutomation;
        public bool ExportDataGridForAutomation
        {
            get { return _exportDataGridForAutomation; }
            set { _exportDataGridForAutomation = value; OnPropertyChanged("ExportDataGridForAutomation"); }
        }

        /// <summary>
        /// property to store the file path for export data
        /// </summary>
        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        /// <summary>
        /// _checkComplianceConnector_FeedbackMessageReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _checkComplianceConnector_FeedbackMessageReceived(object sender, EventArgs<DataSet> e)
        {
            try
            {
                InProgressMessage = HelperFunctionsForCompliance.CheckComplianceConnector_FeedbackMessageReceived(e, _loginUser.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentTradingToolViewModel"/> class.
        /// </summary>
        public PercentTradingToolViewModel()
        {
            try
            {
                LoginUser = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
                if (ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID) || ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                {
                    IsCheckComplianceAllowed = true;
                    CheckComplianceFeedback checkComplianceFeedback = new CheckComplianceFeedback();
                    checkComplianceFeedback.FeedbackMessageReceived += _checkComplianceConnector_FeedbackMessageReceived;
                }

                IsCustodianColVisible = TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker;
                PTTManager.AllocationPrefID = int.MinValue;
                EventAggregator.GetInstance.SubsribeEvent(this, SynchronizationContext.Current);
                CalculateButtonClicked = new DelegateCommand<object>(parameter => CalculateButtonAction(), canExecute => CanExecute && IsExpnlServiceConnected);
                RecalculateEditableCellCommand = new DelegateCommand<object>(RecalculateEditableCellAction);
                RecordOldEditableCellValue = new DelegateCommand<object>(RecordOldEditableCellValueAction);
                PTTUILoaded = new DelegateCommand<object>(parameter => PttUiLoadedAction());
                TradeClicked = new DelegateCommand<object>(TradeClickedAction, canExecute => CanExecute);
                CheckComplianceButtonClicked = new DelegateCommand<object>(CheckComplianceAction, canExecute => CanExecute && IsCheckComplianceAllowed);
                CreateOrderClicked = new DelegateCommand<object>(parentWindow => CreateOrderClickedAction(), canExecute => CanExecute);
                ReloadPttUI = new DelegateCommand<object>(parameter => ReloadNewData());
                TryExpnlServiceConnectCommand = new DelegateCommand<object>(parameter => TryExpnlServiceConnect());
                RecalculationNeededCommand = new DelegateCommand<object>(RecalculationNeededAction);
                ExportClicked = new DelegateCommand<object>(parameter => ExportClickedAction());
                PreferenceClicked = new DelegateCommand<object>(parameter => PreferernceClickedAction());
                DisplayMasterFundOrAccountCommand = new DelegateCommand<SelectionChangedEventArgs>(DisplayMasterFundOrAccountAction);
                DisplayPercentageOrBasisPointCommand = new DelegateCommand<SelectionChangedEventArgs>(DisplayPercentageOrBasisPointAction);
                TickerKeyUpCommand = new DelegateCommand<object>(obj => TickerKeyUpCommandAction(obj));
                ValidateSecurityCommand = new DelegateCommand(() => ValidateSecurityCommandAction());
                DropDownOpenedCommand = new DelegateCommand(() => DropDownOpenedCommandAction());
                EnableEndTargetPercentageCommand = new DelegateCommand<SelectionChangedEventArgs>(EnableDisableAddOrSet);

                CellTypeCollection = new ObservableDictionary<string, string>
                {
                {PTTConstants.COL_SYMBOL, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_TYPE,  PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_ADDORSET, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_MASTERFUNDORACCOUNT, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_COMBINEDACCOUNTSTOTALVALUE, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_ACCOUNT, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_ISCUSTODIANBROKER, PTTConstants.LIT_XAMCOMBOEDITOR},
                {PTTConstants.COL_ISUSEROUNDLOT, PTTConstants.LIT_XAMCOMBOEDITOR}
                };
                SymbolDescriptionColor = ColorTranslator.FromHtml("#FF3A4E6C");

                CreateSMSyncPoxy();
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Initialize()
        {
            try
            {
                _securityMaster.SecMstrDataResponse += _secMasterClient_SecMstrDataResponse;
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

        public void SymbolEntered(bool isOpenFromPM = false)
        {
            try
            {
                ValidateSecurityCommandAction(isOpenFromPM);
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

        private void ValidateSecurityCommandAction(bool isOpenFromPM = false)
        {
            try
            {
                if (!Symbol.Equals(SymbolUI))
                {
                    if (!string.IsNullOrWhiteSpace(SymbolUI))
                        SendRequestForValidation(isOpenFromPM);
                    else
                        ClearDetails();
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

        private void SendRequestForValidation(bool isOpenFromPM = false)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.AddData(SymbolUI.Trim(), Symbology);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = GetHashCode();
                if (SecurityMaster != null)
                {
                    if (!isOpenFromPM)
                    {
                        ClearDetails();
                    }
                    SecurityMaster.SendRequest(reqObj);
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

        private void _secMasterClient_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (PTTRequestBindingListCollectionView == null) return;
                SecMasterBaseObj secMasterBaseObj = e.Value;
                PTTRequestObject requestObject = PTTRequestBindingListCollectionView.SourceCollection.Cast<PTTRequestObject>().ToList().ElementAt(0);
                if (SymbolUI.ToUpper().Equals(secMasterBaseObj.SymbologyMapping[(int)SymbologyHelper.DefaultSymbology].ToUpper()))
                {
                    if (!secMasterBaseObj.AssetCategory.Equals(AssetCategory.Equity))
                    {
                        SetCustomErrorMessages("Non Equity Asset Category", true);
                        IsEquitySymbol = false;
                    }
                    else
                    {
                        SetCustomErrorMessages("Non Equity Asset Category", false);
                        IsEquitySymbol = true;
                    }
                    PTTManager.PermittedOrderSidesForAsset = PTTManager.GetOrderSideDictionary(secMasterBaseObj.AssetID);
                    StringBuilder descriptionStringBuilder = new StringBuilder();
                    descriptionStringBuilder.Append(secMasterBaseObj.LongName);
                    descriptionStringBuilder.Append(" (");
                    descriptionStringBuilder.Append(CachedDataManager.GetInstance.GetCurrencyText(secMasterBaseObj.CurrencyID));
                    descriptionStringBuilder.Append(")");
                    SymbolDescription = descriptionStringBuilder.ToString();
                    Symbol = SymbolUI;
                    Symbology = SymbologyHelper.DefaultSymbology;
                    requestObject.TickerSymbol = requestObject.Symbol = secMasterBaseObj.TickerSymbol;
                    requestObject.SecMasterBaseObj = secMasterBaseObj;
                    StringBuilder errorMessage = new StringBuilder();
                    PTTManager.GetSelectedFeedForRequestObject(requestObject, ref errorMessage);
                    bool isSymbolExist = PTTManager.DoesSymbolExistInPortfolio(requestObject, ref errorMessage);
                    if (!isSymbolExist)
                    {
                        PTTAddOrSetCollection =
                              new ObservableCollection<EnumerationValue>(
                              Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTChangeType)).Where(a => (PTTChangeType)a.Value == PTTChangeType.Buy || (PTTChangeType)a.Value == PTTChangeType.SellShort));
                    }
                    else
                    {
                        PTTAddOrSetCollection = TempAddSetCollection;
                    }

                    SetChangeTypeFromPreference(requestObject, isSymbolExist);
                    if (!String.IsNullOrWhiteSpace(errorMessage.ToString()))
                    {
                        StatusMessage = errorMessage.ToString();
                    }
                    else if (StatusMessage.Equals((PTTConstants.MSG_WAITING_FOR_VALIDATION)))
                    {
                        StatusMessage = String.Empty;
                    }
                    if (String.IsNullOrWhiteSpace(errorMessage.ToString()))
                    {
                        IsSymbolValidated = true;

                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                            _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                            _marketDataHelperInstance.RequestSingleSymbol(((SecMasterBaseObj)e.Value).TickerSymbol, true);
                        }
                    }
                    EnableButtonsAndGrid(false);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LOne_OnResponse(object sender, EventArgs<SymbolData> e)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && PTTRequestBindingListCollectionView != null)
                {
                    PTTRequestObject pttRequestObject = PTTRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0);
                    if (pttRequestObject != null && pttRequestObject.TickerSymbol.Equals(e.Value.Symbol) && SymbolUI.ToUpper().Equals(pttRequestObject.SecMasterBaseObj.SymbologyMapping[(int)SymbologyHelper.DefaultSymbology]))
                    {
                        Last = e.Value.LastPrice.ToString();
                        Ask = e.Value.Ask.ToString();
                        Bid = e.Value.Bid.ToString();
                        VWAP = e.Value.VWAP.ToString();
                        if (pttRequestObject.SelectedFeedPrice == Decimal.Zero)
                        {
                            pttRequestObject.SelectedFeedPrice = Decimal.Parse(Last);
                        }
                        if (StatusMessage.Equals(PTTConstants.MSG_LIVE_FEED_DISCONNECTED))
                            StatusMessage = string.Empty;
                        _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void TickerKeyUpCommandAction(object obj)
        {
            try
            {
                Key keyData = ((System.Windows.Input.KeyEventArgs)obj).Key;
                if (keyData.Equals(Key.Up) || keyData.Equals(Key.Down) || keyData.Equals(Key.Enter) || keyData.Equals(Key.Right) || keyData.Equals(Key.Left))
                    return;

                int keyCode = KeyInterop.VirtualKeyFromKey(keyData);
                if (!(keyCode == 8 || (keyCode >= 48 && keyCode <= 90) || (keyCode >= 96 && keyCode <= 105) || (keyCode >= 186 && keyCode <= 226)))
                    return;

                SecMasterSymbolSearchReq request = new SecMasterSymbolSearchReq(SymbolUI.Trim(), Symbology);
                SecMasterSymbolSearchRes symbolList = ServiceManager.Instance.SecMasterServices.InnerChannel.SearchSymbols(request);

                UpdateDropDown(symbolList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateDropDown(SecMasterSymbolSearchRes symbolList)
        {
            try
            {
                if (symbolList != null)
                {
                    ObservableCollection<SymbolModel> tempSymbolCollection = new ObservableCollection<SymbolModel>();
                    var data = symbolList.Result;

                    foreach (string item in data)
                        if (tempSymbolCollection.Count(x => x.Symbol == item) == 0)
                            tempSymbolCollection.Add(new SymbolModel { Symbol = item });

                    LstSuggestedSymbol = tempSymbolCollection;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void DropDownOpenedCommandAction()
        {
            try
            {
                if (LstSuggestedSymbol == null || LstSuggestedSymbol.Count == 0)
                    IsSymbolDropDownOpen = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreateSMSyncPoxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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
        /// clicked action of Preferernces btn.
        /// </summary>
        /// <returns></returns>
        private void PreferernceClickedAction()
        {
            try
            {
                EventAggregator.GetInstance.PublishEvent(new PTTPreferenceClicked());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        ///<summary>
        /// Records the old editable cell action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void RecordOldEditableCellValueAction(object parameter)
        {
            try
            {
                EditModeStartedEventArgs editModeStartedEventArgs = (EditModeStartedEventArgs)parameter;
                _editModeOriginalCellValue = FormatEditorCellValue(editModeStartedEventArgs.Editor.Format, editModeStartedEventArgs.Editor.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Formats the editor cell value.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <returns></returns>
        private static string FormatEditorCellValue(string format, object cellValue)
        {
            String editorCellValue = string.Empty;
            try
            {
                string decimalFormat = "{0:" + format + "}";
                editorCellValue = decimal.Parse(String.Format(decimalFormat, cellValue), CultureInfo.InvariantCulture).ToString();
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
            return editorCellValue;
        }

        /// <summary>
        /// Stages the clicked action.
        /// </summary>
        private void CreateOrderClickedAction()
        {
            try
            {
                InProgressMessage = ComplainceConstants.Feedback_Initial_Msg;
                bool isOrderValid = false;
                Thread thread = new Thread((ThreadStart)(() =>
                {
                    EnableButtonsAndGrid(false);
                    InProgress = true;
                    PTTRequestObject pttRequestObject =
                    PTTRequestBindingListCollectionView.SourceCollection.Cast<PTTRequestObject>().ToList().FirstOrDefault();
                    StringBuilder errorMessage = new StringBuilder();
                    int orderNo = 0;
                    if (PTTManager.DictOrdersAndResponse != null && PTTManager.DictOrdersAndResponse.Count > 0)
                    {
                        //first Order is the first ordr in the collection of orders and is used to create global allocation preference
                        var firstOrder = PTTManager.DictOrdersAndResponse.Values.FirstOrDefault();
                        var allocPreference = PTTManager.CreateAllocationPreference(pttRequestObject, firstOrder);
                        if (firstOrder != null)
                        {
                            if (CachedDataManager.GetInstance.IsBreakOrderPreference())
                            {
                                if (PTTManager.DictOrdersAndResponse.Count > 1)
                                {
                                    foreach (var orderResponseList in PTTManager.DictOrdersAndResponse.Values)
                                    {
                                        if (!firstOrder.Equals(orderResponseList))
                                        {  // Incase of multiple orders, order side wise preference is created and is linked with allocation preference of first order
                                            foreach (var individualOrder in orderResponseList)
                                            {
                                                var orderItemList = new List<PTTResponseObject>();
                                                orderItemList.Add(individualOrder);
                                                var order = PTTManager.CreateCheckListWisePrefAndOrder(pttRequestObject, orderItemList, orderNo, allocPreference);
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                                }
                                                SendOrderForStage(ref isOrderValid, pttRequestObject, orderResponseList, errorMessage, order);
                                            }
                                        }
                                        else
                                        {
                                            foreach (var individualOrder in firstOrder)
                                            {
                                                var orderItemList = new List<PTTResponseObject>();
                                                orderItemList.Add(individualOrder);
                                                var order = PTTManager.CreateOrderSingle(pttRequestObject, orderItemList, allocPreference.OperationPreferenceId);
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                                }
                                                SendOrderForStage(ref isOrderValid, pttRequestObject, firstOrder, errorMessage, order);
                                            }
                                        }
                                        orderNo++;
                                    }
                                }
                                else
                                {
                                    foreach (var orderItem in firstOrder)
                                    {
                                        var orderItemList = new List<PTTResponseObject>();
                                        orderItemList.Add(orderItem);
                                        var order = PTTManager.CreateOrderSingle(pttRequestObject, orderItemList, allocPreference.OperationPreferenceId);
                                        if (pttRequestObject.IsUseCustodianBroker)
                                        {
                                            order.IsUseCustodianBroker = true;
                                            List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                            order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                        }
                                        SendOrderForStage(ref isOrderValid, pttRequestObject, firstOrder, errorMessage, order);
                                    }
                                    orderNo++;
                                }
                            }
                            else
                            {
                                if (PTTManager.DictOrdersAndResponse.Count > 1)
                                {
                                    foreach (var orderResponseList in PTTManager.DictOrdersAndResponse.Values)
                                    {
                                        if (!firstOrder.Equals(orderResponseList))
                                        {
                                            // Incase of multiple orders, order side wise preference is created and is linked with allocation preference of first order
                                            var order = PTTManager.CreateCheckListWisePrefAndOrder(pttRequestObject, orderResponseList, orderNo, allocPreference);
                                            if (allocPreference != null && allocPreference.CheckListWisePreference.Count == 1 && allocPreference.CheckListWisePreference.Values.FirstOrDefault().TargetPercentage.Count == 1)
                                            {
                                                order.Level1ID = allocPreference.CheckListWisePreference.Values.FirstOrDefault().TargetPercentage.Keys.First();
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = new List<int>();
                                                    accountIds.Add(order.Level1ID);
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                                }
                                            }
                                            else if (allocPreference != null && allocPreference.CheckListWisePreference.Count == 1)
                                            {
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = allocPreference.CheckListWisePreference.Values.FirstOrDefault().TargetPercentage.Keys.ToList();
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                            }
                                            }
                                            SendOrderForStage(ref isOrderValid, pttRequestObject, orderResponseList, errorMessage, order);
                                        }
                                        else
                                        {
                                            var order = PTTManager.CreateOrderSingle(pttRequestObject, firstOrder, allocPreference.OperationPreferenceId);
                                            if (allocPreference.TargetPercentage.Count == 1)
                                            {
                                                order.Level1ID = allocPreference.TargetPercentage.Keys.First();
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = new List<int>();
                                                    accountIds.Add(order.Level1ID);
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                                }
                                            }
                                            else
                                            {
                                                if (pttRequestObject.IsUseCustodianBroker)
                                                {
                                                    order.IsUseCustodianBroker = true;
                                                    List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                            }
                                            }
                                            SendOrderForStage(ref isOrderValid, pttRequestObject, firstOrder, errorMessage, order);
                                        }
                                        orderNo++;
                                    }
                                }
                                else
                                {
                                    var order = PTTManager.CreateOrderSingle(pttRequestObject, firstOrder, allocPreference.OperationPreferenceId);
                                    if (allocPreference.TargetPercentage.Count == 1)
                                    {
                                        order.Level1ID = allocPreference.TargetPercentage.Keys.First();
                                        if (pttRequestObject.IsUseCustodianBroker)
                                        {
                                            order.IsUseCustodianBroker = true;
                                            List<int> accountIds = new List<int>();
                                            accountIds.Add(order.Level1ID);
                                            order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                        }
                                    }
                                    else
                                    {
                                        if (pttRequestObject.IsUseCustodianBroker)
                                        {
                                            order.IsUseCustodianBroker = true;
                                            List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                            order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                    }
                                    }
                                    SendOrderForStage(ref isOrderValid, pttRequestObject, firstOrder, errorMessage, order);
                                    orderNo++;
                                }
                            }
                        }
                    }
                    else
                    {
                        StatusMessage = PTTConstants.MSG_TRADEQAUNTITY_ZERO;
                    }
                    if (isOrderValid)
                        EventAggregator.GetInstance.PublishEvent(new ClosePTTUI());
                    InProgress = false;
                    EnableButtonsAndGrid(true);
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                InProgress = false;
                EnableButtonsAndGrid(true);
            }
        }

        /// <summary>
        /// Stages the method.
        /// </summary>
        /// <param name="isOrderValid">if set to <c>true</c> [is order valid].</param>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="pttResponseObjectList">The PTT response object list.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="order">The order.</param>
        private void SendOrderForStage(ref bool isOrderValid, PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjectList, StringBuilder errorMessage, OrderSingle order)
        {
            try
            {
                StatusMessage = errorMessage.ToString();
                if (order != null)
                {
                    isOrderValid = true;
                    bool isOrderValidated = false;
                    StringBuilder preferenceErrorMessage = new StringBuilder();
                    HelperFunctionsForCompliance.CreateCompleteOrderForStageAndCompliance(order, ref preferenceErrorMessage);
                    if (preferenceErrorMessage.Length == 0)
                    {
                        int mfID = GetMasterFundIDfromAccounts(MFOrAccFromPM, AccountList);
                        if (mfID > 0)
                        {
                            order.MasterFund = CachedDataManager.GetInstance.GetMasterFund(mfID);
                            int tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(mfID);
                            if (tradingAccountID > 0)
                                order.TradingAccountID = tradingAccountID;
                        }
                        order.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                        order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                        PTTManager.SavePTTPreferenceDetails(pttRequestObject, pttResponseObjectList);
                        if (ComplianceCacheManager.GetPreTradeCheckStaging(_loginUser.CompanyUserID))
                        {
                            if (!TradeManager.ValidationManager.ValidateOrder(order, _loginUser.CompanyUserID))
                            {
                                StatusMessage = PTTConstants.MSG_CANNOT_STAGE;
                                return;
                            }
                            isOrderValidated = true;
                            List<OrderSingle> orders = new List<OrderSingle>() { order };
                            if (!ComplianceCommon.ValidateOrderInCompliance_New(orders, null, _loginUser.CompanyUserID))
                            {
                                isOrderValid = false;
                                InProgress = false;
                                return;
                            }
                        }
                        StatusMessage = !(TradeManager.TradeManager.GetInstance().SendBlotterTrades(order, order.Price, isOrderValidated: isOrderValidated))
                            ? PTTConstants.MSG_CANNOT_STAGE
                            : PTTConstants.MSG_STAGE_SUCCESS;
                    }
                    else
                    {
                        StatusMessage = preferenceErrorMessage.ToString();
                        isOrderValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                InProgress = false;
                EnableButtonsAndGrid(true);
            }
        }

        /// <summary>
        /// Checks the compliance action.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        private void CheckComplianceAction(object parentWindow)
        {
            try
            {
                InProgressMessage = ComplainceConstants.Feedback_Initial_Msg;
                System.Threading.Tasks.Task.Run(() =>
                {
                    EnableButtonsAndGrid(false);
                    InProgress = true;

                    PTTRequestObject pttRequestObject = PTTRequestBindingListCollectionView.SourceCollection.Cast<PTTRequestObject>().ToList().FirstOrDefault();
                    StringBuilder errorMessage = new StringBuilder();
                    int orderNo = 0;
                    //first Order is the first ordr in the collection of orders and is used to create global allocation preference
                    List<PTTResponseObject> firstOrder = PTTManager.DictOrdersAndResponse.FirstOrDefault().Value;
                    if (firstOrder == null)
                    {
                        StatusMessage = PTTConstants.MSG_TRADEQAUNTITY_ZERO;
                        return;
                    }
                    var allocPreference = PTTManager.CreateAllocationPreference(pttRequestObject, firstOrder);
                    if (PTTManager.DictOrdersAndResponse != null && PTTManager.DictOrdersAndResponse.Count > 1)
                    {
                        List<OrderSingle> ordersList = new List<OrderSingle>();
                        foreach (var orderResponseList in PTTManager.DictOrdersAndResponse.Values)
                        {
                            OrderSingle order = null;
                            if (!firstOrder.Equals(orderResponseList))
                            {// Incase of multiple orders, order side wise preference is created and is linked with allocation preference of first order
                                order = PTTManager.CreateCheckListWisePrefAndOrder(pttRequestObject, orderResponseList, orderNo, allocPreference);
                                if (pttRequestObject.IsUseCustodianBroker)
                                {
                                    order.IsUseCustodianBroker = true;
                                    List<int> accountIds = allocPreference.CheckListWisePreference.Values.FirstOrDefault().TargetPercentage.Keys.ToList();
                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                }
                            }
                            else
                            {
                                order = PTTManager.CreateOrderSingle(pttRequestObject, orderResponseList, allocPreference.OperationPreferenceId);
                                if (pttRequestObject.IsUseCustodianBroker)
                                {
                                    order.IsUseCustodianBroker = true;
                                    List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                    order.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, order.CounterPartyID));
                                }
                            }
                            if (allocPreference.TargetPercentage.Count == 1)
                            {
                                order.Level1ID = allocPreference.TargetPercentage.Keys.First();
                            }
                            AddCompleteOrderToList(ordersList, order);
                            orderNo++;
                        }
                        List<OrderSingle> multiOrder = new List<OrderSingle>();
                        StringBuilder prefMessage = new StringBuilder(string.Empty);
                        foreach (var singleOrder in ordersList)
                        {
                            StringBuilder preferenceErrorMessage = ValidateAndCreatOrderForCompliance(singleOrder);
                            if (!string.IsNullOrEmpty(preferenceErrorMessage.ToString()) && string.IsNullOrEmpty(prefMessage.ToString()))
                            {
                                prefMessage.Append(preferenceErrorMessage);
                            }
                            multiOrder.Add(singleOrder);
                        }
                        SendOrdersForComplianceCheck(multiOrder, prefMessage);
                    }
                    else
                    {
                        if (firstOrder != null && firstOrder.Count > 0)
                        {
                            var singleOrder = PTTManager.CreateOrderSingle(pttRequestObject, firstOrder, allocPreference.OperationPreferenceId);
                            if (pttRequestObject.IsUseCustodianBroker)
                            {
                                singleOrder.IsUseCustodianBroker = true;
                                List<int> accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                singleOrder.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, singleOrder.CounterPartyID));
                            }
                            if (allocPreference.TargetPercentage.Count == 1)
                            {
                                singleOrder.Level1ID = allocPreference.TargetPercentage.Keys.First();
                            }
                            StatusMessage = errorMessage.ToString();
                            if (singleOrder != null)
                            {
                                StringBuilder preferenceErrorMessage = ValidateAndCreatOrderForCompliance(singleOrder);
                                SendOrdersForComplianceCheck(new List<OrderSingle> { singleOrder }, preferenceErrorMessage);
                            }
                            else
                            {
                                InProgressMessage = PTTConstants.MSG_NO_TRADE_COMP_CHECK;
                                DialogService.DialogServiceInstance.ShowMessageBox(this, PTTConstants.MSG_NO_TRADE_COMP_CHECK,
                                RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                            }

                        }
                    }
                }).ContinueWith(o =>
                {
                    EnableButtonsAndGrid(true);
                    InProgress = false;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                EnableButtonsAndGrid(true);
            }
        }

        /// <summary>
        /// Validates the order and create complete order for compliance.
        /// </summary>
        /// <param name="singleOrder">The single order.</param>
        /// <returns></returns>
        private static StringBuilder ValidateAndCreatOrderForCompliance(OrderSingle singleOrder)
        {
            StringBuilder preferenceErrorMessage = new StringBuilder();
            try
            {
                HelperFunctionsForCompliance.CreateCompleteOrderForStageAndCompliance(singleOrder, ref preferenceErrorMessage);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return preferenceErrorMessage;
        }

        /// <summary>
        /// Adds the complete order to list.
        /// </summary>
        /// <param name="ordersList">The orders list.</param>
        /// <param name="order">The order.</param>
        private static void AddCompleteOrderToList(List<OrderSingle> ordersList, OrderSingle order)
        {
            try
            {
                if (order != null)
                {
                    StringBuilder preferenceErrorMessage = new StringBuilder();
                    HelperFunctionsForCompliance.CreateCompleteOrderForStageAndCompliance(order, ref preferenceErrorMessage);
                    ordersList.Add(order);
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
        /// Sends the List of orders for compliance check.
        /// </summary>
        /// <param name="orderList">The order list.</param>
        /// <param name="preferenceErrorMessage">The preference error message.</param>
        private void SendOrdersForComplianceCheck(List<OrderSingle> orderList, StringBuilder preferenceErrorMessage)
        {
            try
            {
                if (preferenceErrorMessage.Length == 0)
                {
                    foreach (OrderSingle order in orderList)
                    {
                        int mfID = GetMasterFundIDfromAccounts(MFOrAccFromPM, AccountList);
                        if (mfID > 0)
                        {
                            order.MasterFund = CachedDataManager.GetInstance.GetMasterFund(mfID);
                            int tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(mfID);
                            if (tradingAccountID > 0)
                                order.TradingAccountID = tradingAccountID;
                        }
                    }
                    var result = ComplianceCommon.CheckCompliance(orderList, _loginUser.CompanyUserID);
                    if (result.Equals(false))
                        DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerConstants.MSG_NO_RULES_VIOLATED,
                                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                    else if (result == null)
                        StatusMessage = preferenceErrorMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Trades the clicked action.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        private void TradeClickedAction(object parentWindow)
        {
            try
            {
                bool isOrderValid = false;
                Task.Run(() =>
                {
                    TradeClickedBackgroundTask(parentWindow, ref isOrderValid);
                }).ContinueWith(o =>
                {
                    if (isOrderValid)
                        EventAggregator.GetInstance.PublishEvent(new ClosePTTUI());
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Trades the clicked background task.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="isOrderValid">if set to <c>true</c> [is order valid].</param>
        private void TradeClickedBackgroundTask(object parentWindow, ref bool isOrderValid)
        {
            try
            {
                InProgress = true;
                PTTRequestObject pttRequestObject =
                PTTRequestBindingListCollectionView.SourceCollection.Cast<PTTRequestObject>().ToList().FirstOrDefault();
                int orderNo = 0;
                if (PTTManager.DictOrdersAndResponse != null && PTTManager.DictOrdersAndResponse.Count > 0)
                {
                    if (PTTManager.DictOrdersAndResponse.Count > 1)
                    {
                        TradeUsingMultiTT(parentWindow, ref isOrderValid, pttRequestObject, ref orderNo, MFOrAccFromPM, AccountList);
                    }
                    else
                    {
                        TradeUsingTT(parentWindow, ref isOrderValid, pttRequestObject, orderNo, MFOrAccFromPM, AccountList);
                    }
                }
                else
                {
                    StatusMessage = PTTConstants.MSG_TRADEQAUNTITY_ZERO;
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
            finally
            {
                InProgress = false;
                EnableButtonsAndGrid(true);
            }
        }

        /// <summary>
        /// Enables the buttons and grid.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void EnableButtonsAndGrid(bool isEnabled)
        {
            try
            {
                if (EnableGrid != isEnabled)
                {
                    EnableGrid = isEnabled;
                    if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                    {
                        EnableExport = false;
                    }
                    else
                        EnableExport = EnableGrid;
                }
                if (EnableButtons != isEnabled)
                    EnableButtons = isEnabled;
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
        /// Trades the using Trading Ticket for single orders.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="isOrderValid">if set to <c>true</c> [is order valid].</param>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="orderNo"> this tracks the current order.</param>
        private void TradeUsingTT(object parentWindow, ref bool isOrderValid, PTTRequestObject pttRequestObject, int orderNo, PTTMasterFundOrAccount mfOrAccount, List<int> accountList)
        {
            OrderSingle order = null;
            try
            {
                List<PTTResponseObject> orderResponseList = PTTManager.DictOrdersAndResponse.FirstOrDefault().Value;
                if (orderResponseList != null)
                {
                    var allocPreference = PTTManager.CreateAllocationPreference(pttRequestObject, orderResponseList);
                    if (allocPreference != null)
                    {
                        order = PTTManager.CreateOrderSingle(pttRequestObject, orderResponseList, allocPreference.OperationPreferenceId);
                        order.Symbol = pttRequestObject.Symbol;
                        int mfID = GetMasterFundIDfromAccounts(mfOrAccount, accountList);
                        if (mfID > 0)
                        {
                            order.MasterFund = CachedDataManager.GetInstance.GetMasterFund(mfID);
                            //int tradingAccountID = CachedData.GetTradingAccountForMasterFund(mfID);
                            //if (tradingAccountID > 0)
                            //    order.TradingAccountID = tradingAccountID;
                        }
                    }
                }
                if (order != null)
                {
                    isOrderValid = true;
                    PTTManager.SavePTTPreferenceDetails(pttRequestObject, orderResponseList);
                    EventAggregator.GetInstance.PublishEvent(new PTTTradeClicked
                    {
                        Order = order,
                        ParentWindow = (Window)parentWindow
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the master fund i dfrom accounts.
        /// </summary>
        /// <param name="mfOrAccount">The mf or account.</param>
        /// <param name="accountList">The account list.</param>
        /// <returns></returns>
        private static int GetMasterFundIDfromAccounts(PTTMasterFundOrAccount mfOrAccount, List<int> accountList)
        {
            int mfID = int.MinValue;

            try
            {
                if (mfOrAccount == PTTMasterFundOrAccount.MasterFund && accountList.Count == 1)
                {
                    mfID = accountList.First();
                }
                else if (mfOrAccount == PTTMasterFundOrAccount.Account && accountList.Count >= 1)
                {
                    List<int> funds = new List<int>();
                    foreach (var account in accountList)
                    {
                        int fund = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(account);
                        if (!funds.Contains(fund))
                            funds.Add(fund);
                    }
                    if (funds.Count == 1)
                        mfID = funds.First();
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
            return mfID;
        }

        /// <summary>
        /// Trades the using Multi Order Trading TicketerrorMessage.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="isOrderValid">if set to <c>true</c> [is order valid].</param>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="orderNo">Incase of multiorders this track the current order.</param>
        private static void TradeUsingMultiTT(object parentWindow, ref bool isOrderValid, PTTRequestObject pttRequestObject, ref int orderNo, PTTMasterFundOrAccount mfOrAccount, List<int> accountList)
        {
            OrderBindingList multiOrders = new OrderBindingList();
            OrderSingle order = null;
            try
            {    //first Order is the first ordr in the collection of orders and is used to create global allocation preference
                List<PTTResponseObject> firstOrder = PTTManager.DictOrdersAndResponse.FirstOrDefault().Value;
                var allocPreference = PTTManager.CreateAllocationPreference(pttRequestObject, firstOrder);
                foreach (var orderResponseList in PTTManager.DictOrdersAndResponse.Values)
                {
                    if (firstOrder.Equals(orderResponseList))
                    {
                        order = PTTManager.CreateOrderSingle(pttRequestObject, firstOrder, allocPreference.OperationPreferenceId);
                        var accIDs = firstOrder.Select(o => o.AccountId).Distinct();
                        if(accIDs.Count() == 1)
                            order.Level1ID = accIDs.First();
                    }
                    else
                    {// Incase of multiple orders, order side wise preference is created and is linked with allocation preference of first order
                        order = PTTManager.CreateCheckListWisePrefAndOrder(pttRequestObject, orderResponseList, orderNo, allocPreference);
                        var accIDs = orderResponseList.Select(o => o.AccountId).Distinct();
                        if (accIDs.Count() == 1)
                            order.Level1ID = accIDs.First();
                    }
                    if (order != null)
                    {
                        isOrderValid = true;
                        multiOrders.Add(order);
                        int mfID = GetMasterFundIDfromAccounts(mfOrAccount, accountList);
                        if (mfID > 0)
                        {
                            order.MasterFund = CachedDataManager.GetInstance.GetMasterFund(mfID);
                            int tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(mfID);
                            if (tradingAccountID > 0)
                                order.TradingAccountID = tradingAccountID;
                        }
                    }
                    PTTManager.SavePTTPreferenceDetails(pttRequestObject, orderResponseList);
                    orderNo++;
                }

                EventAggregator.GetInstance.PublishEvent(new OpenMultiTTFromPTT
                {
                    OrderList = multiOrders,
                    ParentWindow = (Window)parentWindow
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Snapshots the response action.
        /// </summary>
        /// <param name="SanpshotResponseEventArgs">The <see cref="SanpshotResponseEventArgs"/> instance containing the event data.</param>
        private void SnapshotResponseAction(SanpshotResponseEventArgs SanpshotResponseEventArgs)
        {
            try
            {

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clears the details action.
        /// </summary>
        private void ClearDetails()
        {
            try
            {
                IsSymbolValidated = false;
                SymbolDescription = string.Empty;
                Symbol = string.Empty;
                Last = PTTConstants.LIT_DEFAULT_FEED_VALUES;
                Ask = PTTConstants.LIT_DEFAULT_FEED_VALUES;
                Bid = PTTConstants.LIT_DEFAULT_FEED_VALUES;
                VWAP = PTTConstants.LIT_DEFAULT_FEED_VALUES;
                EnableButtonsAndGrid(false);
                PTTResponseBindingListCollectionView.DetachFromSourceCollection();
                List<PTTResponseObject> pttResponseObject = new List<PTTResponseObject> { new PTTResponseObject() };
                PTTResponseBindingListCollectionView = new BindingListCollectionView((new BindingList<PTTResponseObject>(pttResponseObject)));
                PTTRequestObject pttRequestObject = PTTRequestBindingListCollectionView.SourceCollection.Cast<PTTRequestObject>().ToList().FirstOrDefault();
                pttRequestObject.Target = 0;
                PTTPreferences pttPreferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(_loginUser.CompanyUserID);
                bool dollarAmountPermission = Boolean.Parse(PTTPrefDataManager.GetInstance.GetPTTDollorAmountPermission().ToString());
                pttRequestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault();
                pttRequestObject.CombineAccountEnumValue = CombinedAccountsTotalValueCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.CombineAccountsTotal).ToString());
                if (pttPreferences.CalculationType == PTTType.DollarAmount && !dollarAmountPermission)
                {
                    pttRequestObject.Type = TypeCollection.FirstOrDefault();
                }
                else
                {
                    pttRequestObject.Type = TypeCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.CalculationType).ToString());
                }
                pttRequestObject.SelectedFeedPrice = Decimal.Zero;
                pttRequestObject.MasterFundOrAccount = MasterFundOrAccount.First(x => x.Value.ToString() == ((int)pttPreferences.MasterFundOrAccount).ToString());
                HideUnhideMasterFundOrAccount((PTTMasterFundOrAccount)pttRequestObject.MasterFundOrAccount.Value);
                HideUnhideRoundLot((PTTRoundLotPreferenceValue)pttRequestObject.RoundLotPreferenceEnumValue.Value);
                HideUnhidePercentageOrPointBasis((PTTType)pttRequestObject.Type.Value);
                SetAccountListFromPreference(pttRequestObject, pttPreferences);

                #region Setting Select/UnselectAll for MF and Accounts
                if (CompanyAccountsCollection != null)
                {
                    SetSelectOrUnselectAllOnStartup(pttRequestObject, CompanyAccountsCollection);
                }

                if (CompanyMasterFundCollection != null)
                {
                    SetSelectOrUnselectAllOnStartup(pttRequestObject, CompanyMasterFundCollection);
                }
                #endregion
                if (pttRequestObject != null && !string.IsNullOrEmpty(pttRequestObject.Symbol))
                {
                    StatusMessage = PTTConstants.MSG_WAITING_FOR_VALIDATION;
                }
                else
                {
                    StatusMessage = String.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetChangeTypeFromPreference(PTTRequestObject requestObject, bool symbolExist)
        {
            try
            {
                PTTPreferences pttPreferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(_loginUser.CompanyUserID);
                if (!symbolExist)
                {
                    if (pttPreferences.ChangeType == PTTChangeType.Buy || pttPreferences.ChangeType == PTTChangeType.SellShort)
                    {
                        requestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.ChangeType).ToString());
                    }
                    else
                    {
                        requestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault();
                    }
                }
                else
                {
                    if (pttPreferences.ChangeType == PTTChangeType.Increase || pttPreferences.ChangeType == PTTChangeType.Decrease || pttPreferences.ChangeType == PTTChangeType.Set)
                    {
                        requestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.ChangeType).ToString());
                        EnableDisableFields(pttPreferences.ChangeType);
                    }
                    else
                    {
                        requestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// PTTs the UI loaded action.
        /// </summary>
        private void PttUiLoadedAction()
        {
            try
            {
                PTTPreferences pttPreferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(_loginUser.CompanyUserID);
                bool dollarAmountPermission = Boolean.Parse(PTTPrefDataManager.GetInstance.GetPTTDollorAmountPermission().ToString());
                IsLiveFeedAllowed = true;
                DecimalPrecision = pttPreferences.IncreaseDecimalPrecision;
                _uiSyncContext = SynchronizationContext.Current;
                TypeCollection =
                new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTType)));
                //dollar Amount permission is set to false then remove dollar amount option from drop down
                if (!dollarAmountPermission)
                {
                    TypeCollection.RemoveAt((int)PTTType.DollarAmount);
                }
                MasterFundOrAccount =
                new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTMasterFundOrAccount)));
                TempAddSetCollection = new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTChangeType)).Where(a => (PTTChangeType)a.Value != PTTChangeType.Buy && (PTTChangeType)a.Value != PTTChangeType.SellShort));
                PTTAddOrSetCollection = TempAddSetCollection;
                CombinedAccountsTotalValueCollection =
                new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTCombineAccountTotalValue)));
                RoundLotPreferenceValueCollection =
                new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTRoundLotPreferenceValue)));
                AccountCollection masterFundsCollection = new AccountCollection();
                CustodianBrokerPreferenceValueCollection =
                new ObservableCollection<EnumerationValue>(
                Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(PTTCustodianBrokerPreferenceValue)));

                CompanyAccountsCollection = new ObservableCollection<Account>(CachedDataManager.GetInstance.GetUserAccounts().Cast<Account>().OrderBy(account => account.FullName));

                foreach (KeyValuePair<int, string> currMasterFund in CachedDataManager.GetInstance.GetUserMasterFunds())
                {
                    masterFundsCollection.Add(new Account(currMasterFund.Key, currMasterFund.Value, currMasterFund.Value));
                }
                CompanyMasterFundCollection = new ObservableCollection<Account>(masterFundsCollection.Cast<Account>().OrderBy(account => account.FullName));
                PTTRequestObject pttRequestObject = new PTTRequestObject
                {
                    AddOrSet = PTTAddOrSetCollection.FirstOrDefault(),
                    CombineAccountEnumValue = CombinedAccountsTotalValueCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.CombineAccountsTotal).ToString()),
                    RoundLotPreferenceEnumValue = TradingTktPrefs.UserTradingTicketUiPrefs.IsUseRoundLots ?
                                                    RoundLotPreferenceValueCollection.FirstOrDefault(x => x.Value.Equals((int)PTTRoundLotPreferenceValue.Yes)) :
                                                    RoundLotPreferenceValueCollection.FirstOrDefault(x => x.Value.Equals((int)PTTRoundLotPreferenceValue.No)),
                    CustodianBrokerPreferenceEnumValue = TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker ?
                                                    CustodianBrokerPreferenceValueCollection.FirstOrDefault(x => x.Value.Equals((int)PTTCustodianBrokerPreferenceValue.Yes)) :
                                                    CustodianBrokerPreferenceValueCollection.FirstOrDefault(x => x.Value.Equals((int)PTTCustodianBrokerPreferenceValue.No))
                };
                if (pttPreferences.CalculationType == PTTType.DollarAmount && !dollarAmountPermission)
                {
                    pttRequestObject.Type = TypeCollection.FirstOrDefault();
                }
                else
                {
                    pttRequestObject.Type = TypeCollection.FirstOrDefault(x => x.Value.ToString() == ((int)pttPreferences.CalculationType).ToString());
                }


                List<PTTRequestObject> pttRequestObjectList = new List<PTTRequestObject> { pttRequestObject };
                PTTRequestBindingListCollectionView = new BindingListCollectionView((new BindingList<PTTRequestObject>(pttRequestObjectList)));
                ErrorObject.ErrorDescription = string.Empty;
                TryExpnlServiceConnect();
                ReloadNewData();
                PTTManager.MfAccountPrefBindList = PTTPrefDataManager.GetInstance.PTTMfAccountPrefBindingList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void HideUnhideMasterFundOrAccount(PTTMasterFundOrAccount CalcValue)
        {
            try
            {
                if (CalcValue == PTTMasterFundOrAccount.Account)
                {
                    AccountText = PTTConstants.CAP_ACCOUNTTEXT;
                    UserAccountCollection = CompanyAccountsCollection;
                    IsAccountVisible = true;
                    IsMasterFundVisible = false;
                    MasterFundNavText = PTTConstants.CAP_ACCOUNTNAV;
                }
                else
                {
                    AccountText = PTTConstants.CAP_MASTERFUNDTEXT;
                    UserAccountCollection = CompanyMasterFundCollection;
                    IsAccountVisible = false;
                    IsMasterFundVisible = true;
                    MasterFundNavText = PTTConstants.CAP_MASTERFUNDNAV;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                throw;
            }
        }

        private void HideUnhideRoundLot(PTTRoundLotPreferenceValue roundLotSelection)
        {
            try
            {
                if (roundLotSelection == PTTRoundLotPreferenceValue.No)
                {
                    IsRoundLotVisible = false;
                }
                else
                {
                    IsRoundLotVisible = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                throw;
            }
        }

        private void HideUnhidePercentageOrPointBasis(PTTType CalcValue)
        {
            try
            {
                if (CalcValue == PTTType.BasisPoints)
                {
                    StartingPercentageOrBasisPointText = PTTConstants.CAP_STARTINGBASISPOINT;
                    PercentageOrBasisPointChangeText = PTTConstants.CAP_BASISPOINTCHANGE;
                    EndingPercentageOrPointBasisText = PTTConstants.CAP_ENDINGBASISPOINT;
                }
                else
                {
                    StartingPercentageOrBasisPointText = PTTConstants.CAP_STARTINGPERCENTAGE;
                    PercentageOrBasisPointChangeText = PTTConstants.CAP_PERCENTAGECHANGE;
                    EndingPercentageOrPointBasisText = PTTConstants.CAP_ENDINGPERCENTAGE;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                throw;
            }
        }


        private void EnableDisableFields(PTTChangeType Value)
        {
            try
            {
                if (Value == PTTChangeType.Increase || Value == PTTChangeType.Decrease)
                {
                    PercentageTypeEnableDisable = true;
                    EndingPercentageEnableDisable = false;
                }
                else
                {
                    PercentageTypeEnableDisable = false;
                    EndingPercentageEnableDisable = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                throw;
            }
        }


        /// <summary>
        /// Tries the expnl service connect.
        /// </summary>
        private void TryExpnlServiceConnect()
        {
            try
            {
                string message = ExpnlServiceConnector.GetInstance().TryGetChannel();
                if (!String.IsNullOrEmpty(message))
                {
                    DialogService.DialogServiceInstance.ShowMessageBox(this, message);
                }
                else
                {
                    IsExpnlServiceConnected = true;
                }

                SetCustomErrorMessages("Calculation Service Disconnected", !IsExpnlServiceConnected);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Reloads the new data.
        /// </summary>
        private void ReloadNewData()
        {
            try
            {
                PTTRequestObject pttRequestObject = PTTRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0);
                PTTPreferences prefs = PTTPrefDataManager.GetInstance.PTTPreferences;
                if (AccountList == null || AccountList.Count == 0)
                {
                    pttRequestObject.MasterFundOrAccount = MasterFundOrAccount.First(x => x.Value.ToString() == ((int)prefs.MasterFundOrAccount).ToString());
                    HideUnhideMasterFundOrAccount((PTTMasterFundOrAccount)pttRequestObject.MasterFundOrAccount.Value);
                    HideUnhideRoundLot((PTTRoundLotPreferenceValue)pttRequestObject.RoundLotPreferenceEnumValue.Value);
                    HideUnhidePercentageOrPointBasis((PTTType)pttRequestObject.Type.Value);
                }

                #region PTTOpenedFromPM

                if (AccountList != null)
                {
                    pttRequestObject.Target = 0;
                    pttRequestObject.SelectedFeedPrice = Convert.ToDecimal(Last);
                    pttRequestObject.AddOrSet = PTTAddOrSetCollection.FirstOrDefault(x => x.Value.ToString() == ((int)prefs.ChangeType).ToString());
                    EnableDisableFields(prefs.ChangeType);
                    pttRequestObject.CombineAccountEnumValue = CombinedAccountsTotalValueCollection.FirstOrDefault(x => x.Value.ToString() == ((int)prefs.CombineAccountsTotal).ToString());
                    pttRequestObject.Type = TypeCollection.FirstOrDefault(x => x.Value.ToString() == ((int)prefs.CalculationType).ToString());
                    pttRequestObject.Symbol = _symbol;
                    if (AccountList.Count > 0)
                    {
                        pttRequestObject.MasterFundOrAccount = MasterFundOrAccount.First(x => x.Value.ToString() == ((int)MFOrAccFromPM).ToString());
                        HideUnhideMasterFundOrAccount((PTTMasterFundOrAccount)pttRequestObject.MasterFundOrAccount.Value);
                        HideUnhideRoundLot((PTTRoundLotPreferenceValue)pttRequestObject.RoundLotPreferenceEnumValue.Value);
                        HideUnhidePercentageOrPointBasis((PTTType)pttRequestObject.Type.Value);
                        List<Account> accountList = UserAccountCollection.Where(v => AccountList.Contains(v.AccountID)).ToList();
                        pttRequestObject.Account.Clear();
                        pttRequestObject.Account.AddRangeThreadSafely(accountList);
                        SetCustomErrorMessages("At least an account/masterfund is required", false);
                    }

                    else
                    {   // If no filter is applied from PM and PTT is opened then it will fetch accounts from %trading tool preference.
                        SetAccountListFromPreference(pttRequestObject, prefs);
                    }
                }
                #endregion
                // If  PTT is opened directly, fetching accounts from %trading tool preference.
                if (AccountList == null)
                {
                    SetAccountListFromPreference(pttRequestObject, prefs);
                }

                #region Setting Select/UnselectAll for MF and Accounts
                if (CompanyAccountsCollection != null)
                {
                    SetSelectOrUnselectAllOnStartup(pttRequestObject, CompanyAccountsCollection);
                }

                if (CompanyMasterFundCollection != null)
                {
                    SetSelectOrUnselectAllOnStartup(pttRequestObject, CompanyMasterFundCollection);
                }
                #endregion

                List<PTTResponseObject> pttResponseObject = new List<PTTResponseObject> { new PTTResponseObject() };
                PTTResponseBindingListCollectionView = new BindingListCollectionView((new BindingList<PTTResponseObject>(pttResponseObject)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetAccountListFromPreference(PTTRequestObject pttRequestObject, PTTPreferences prefs)
        {
            try
            {
                pttRequestObject.Account.Clear();
                if (!string.IsNullOrEmpty(prefs.CommaSeparatedAccounts))
                {
                    List<int> accList = prefs.CommaSeparatedAccounts.Split(',').Select(x => int.Parse(x)).ToList();
                    List<Account> accountList = UserAccountCollection.Where(v => accList.Contains(v.AccountID)).ToList();
                    if (accountList != null && accountList.Count > 0)
                    {
                        pttRequestObject.Account.AddRangeThreadSafely(accountList);
                        SetCustomErrorMessages("At least an account/masterfund is required", false);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetSelectOrUnselectAllOnStartup(PTTRequestObject pttRequestObject, ObservableCollection<Account> accounts)
        {
            try
            {
                Account accountToRemove = accounts.FirstOrDefault(item => item.AccountID == int.MinValue);
                int index = accounts.IndexOf(accountToRemove);
                if (index != 0)
                {
                    if (index > 0)
                        accounts.RemoveAt(index);
                    accounts.Insert(0, pttRequestObject.Account.Count == accounts.Count ? new Account(Int32.MinValue, PTTConstants.LIT_UNSELECT_ALL) : new Account(Int32.MinValue, PTTConstants.LIT_SELECT_ALL));
                }
                else
                {
                    accountToRemove.Name = pttRequestObject.Account.Count == accounts.Count ? PTTConstants.LIT_UNSELECT_ALL : PTTConstants.LIT_SELECT_ALL;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Recalculates the trade quantity action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void RecalculateEditableCellAction(object parameter)
        {
            try
            {
                EditModeEndedEventArgs editModeEndedEventArgs = (EditModeEndedEventArgs)parameter;
                int updatedAccount = (editModeEndedEventArgs.Cell.Record.DataItem as PTTResponseObject).AccountId;
                string afterEditCellValue = FormatEditorCellValue(editModeEndedEventArgs.Editor.Format, editModeEndedEventArgs.Editor.Value);
                bool childBand = editModeEndedEventArgs.Cell.Record.HasChildren;
                if (afterEditCellValue.Equals(_editModeOriginalCellValue))
                {
                    _editModeOriginalCellValue = String.Empty;
                    return;
                }
                _editModeOriginalCellValue = String.Empty;
                StringBuilder errorMessg = new StringBuilder(string.Empty);
                StatusMessage = errorMessg.ToString();
                List<PTTResponseObject> pttResponseList = PTTResponseBindingListCollectionView.SourceCollection.OfType<PTTResponseObject>().ToList();
                PTTRequestObject pttRequestObject = _pttRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0);

                PTTEditedColumn editedColumn = GetEditedColumn(editModeEndedEventArgs);
                PTTCalculator.Recalculate(ref pttRequestObject, ref pttResponseList, editedColumn, childBand, ref errorMessg, updatedAccount);
                if (!string.IsNullOrEmpty(errorMessg.ToString()))
                {
                    StatusMessage = errorMessg.ToString();
                    if (!errorMessg.ToString().Equals(PTTConstants.MSG_INVALIDPREFERENCESET))
                    {
                        EnableButtonsAndGrid(false);
                    }
                }
                IsDataRecalculated = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private PTTEditedColumn GetEditedColumn(EditModeEndedEventArgs editModeEndedEventArgs)
        {
            PTTEditedColumn editedColumn = PTTEditedColumn.None;
            try
            {
                switch (editModeEndedEventArgs.Cell.Field.Name)
                {
                    case PTTConstants.COL_TRADEQUANTITY:
                        editedColumn = PTTEditedColumn.TradeQuantity;
                        break;
                    case PTTConstants.COL_ENDINGPERCENTAGE:
                        editedColumn = PTTEditedColumn.EndingPercentage;
                        break;
                    case PTTConstants.COL_PERCENTAGETYPE:
                        editedColumn = PTTEditedColumn.PercentageType;
                        break;
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
            return editedColumn;
        }

        /// <summary>
        /// Calculates the button action.
        /// </summary>
        private void CalculateButtonAction()
        {
            try
            {
                Task.Run(() =>
                {
                    InProgress = true; MessageBoxResult result = MessageBoxResult.Yes;
                    if (IsDataRecalculated)
                    {
                        result = DialogService.DialogServiceInstance.ShowMessageBox(this, PTTConstants.MSG_IS_RECALCULATE_REQUIRED, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.OK, true);
                    }
                    if (result == MessageBoxResult.Yes)
                    {
                        StatusMessage = string.Empty;
                        StringBuilder errorMessage = new StringBuilder();
                        List<PTTResponseObject> pttResponseObjects = PTTManager.GetDataAndCalculate(_pttRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0), ref errorMessage);
                        PTTRequestObject pttRequestObject = _pttRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0);
                        if ((PTTMasterFundOrAccount)pttRequestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund)
                        {
                            pttResponseObjects = pttResponseObjects.OrderBy(pttResponseObject => CachedDataManager.GetInstance.GetMasterFund(pttResponseObject.AccountId)).ToList();
                        }
                        else
                        {
                            pttResponseObjects = pttResponseObjects.OrderBy(pttResponseObject => CachedDataManager.GetInstance.GetAccountText(pttResponseObject.AccountId)).ToList();
                        }
                        AccountList = pttResponseObjects.Select(x => x.AccountId).ToList();
                        MFOrAccFromPM = (PTTMasterFundOrAccount)pttRequestObject.MasterFundOrAccount.Value;
                        StatusMessage = errorMessage.ToString();
                        PTTResponseBindingListCollectionView = new BindingListCollectionView(new BindingList<PTTResponseObject>(pttResponseObjects));
                        if (PTTManager.DictOrdersAndResponse != null && PTTManager.DictOrdersAndResponse.Count > 0)
                        {
                            foreach (string orderSide in PTTManager.DictOrdersAndResponse.Keys)
                            {
                                if (!PTTManager.PermittedOrderSidesForAsset.ContainsKey(TagDatabaseManager.GetInstance.GetOrderSideIdBasedOnSideTagValue(orderSide)))
                                {
                                    StatusMessage = PTTConstants.MSG_NO_PERMISSION_FOR_ORDERSIDE;
                                    break;
                                }
                            }
                        }
                        if (String.IsNullOrWhiteSpace(StatusMessage))
                        {
                            StatusMessage = PTTConstants.MSG_TIME_CALC_COMPLETED + DateTime.Now.ToLocalTime();
                        }
                        HideUnhideRoundLot((PTTRoundLotPreferenceValue)pttRequestObject.RoundLotPreferenceEnumValue.Value);
                        IsDataRecalculated = false;
                        EnableButtonsAndGrid(true);
                        if (StatusMessage.Equals(PTTConstants.MSG_NO_PERMISSION_FOR_ORDERSIDE))
                        {
                            EnableButtons = false;
                        }

                        if (StatusMessage.Equals(PTTConstants.MSG_CONFLICTING_POSITIONS_FOR_CHILD_ACCOUNTS) || StatusMessage.Equals(PTTConstants.MSG_CONFLICTING_POSITIONS))
                        {
                            EnableButtons = false;
                        }
                    }
                    InProgress = false;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RecalculationNeededAction(object parameter)
        {
            try
            {
                if (EnableGrid)
                {
                    StatusMessage = PTTConstants.MSG_CALCULATION_NEEDED;
                    EnableButtonsAndGrid(false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ExportClickedAction()
        {
            try
            {
                ExportGrid = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool? Send(Func<BlockedAlertsViewModel, bool?> showDialog, BlockedAlertsViewModel dialogViewModel)
        {
            bool? dialogResult = false;
            try
            {
                _uiSyncContext.Send(new SendOrPostCallback((x) =>
                {
                    dialogResult = showDialog(dialogViewModel);
                    if (dialogResult == null)
                    {
                        dialogResult = false;
                    }
                }), null);
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
            return dialogResult;
        }

        /// <summary>
        /// Called when [event handler].
        /// </summary>
        /// <param name="e">The e.</param>
        public void OnEventHandler(PTTExpnlStatus e)
        {
            IsExpnlServiceConnected = e.IsExpnlServiceConnected;
            SetCustomErrorMessages("Calculation Service Disconnected", !IsExpnlServiceConnected);
        }

        /// <summary>
        /// For Setting Error messages.
        /// </summary>
        /// <param name="errorMessage"> Error to be displayed</param>
        /// <param name="isError"> True to log error False to remove error</param>
        /// <returns></returns>
        public void SetCustomErrorMessages(string errorMessage, bool isError)
        {
            try
            {
                if (ErrorObject != null && ErrorObject.ErrorDescription != null)
                {
                    if (isError)
                    {
                        if (!ErrorObject.ErrorDescription.ToString().Contains(errorMessage))
                        {
                            ErrorObject.ErrorDescription += errorMessage + "\r\n";

                        }
                    }
                    else
                    {
                        ErrorObject.ErrorDescription = ErrorObject.ErrorDescription.ToString()
                            .Replace(errorMessage + "\r\n", "");
                    }
                    string[] stringSeparators = new string[] { "\r\n" };
                    ErrorObject.ErrorCount =
                        ErrorObject.ErrorDescription.ToString().Split(stringSeparators, StringSplitOptions.None).Length - 1;

                    ErrorObject.ErrorMsg = "Cannot calculate unless you resolve " + ErrorObject.ErrorCount + " errors";
                    if (ErrorObject.ErrorCount == 0)
                    {
                        ErrorObject.ErrorDescription = string.Empty;
                        ErrorObject.ErrorCount = 0;
                        ErrorObject.ErrorMsg = String.Empty;
                    }
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

        private void DisplayMasterFundOrAccountAction(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    EnumerationValue SelectedValue = (EnumerationValue)selectionChangedEventArgs.AddedItems[0];
                    HideUnhideMasterFundOrAccount((PTTMasterFundOrAccount)SelectedValue.Value);
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

        private void DisplayPercentageOrBasisPointAction(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    EnumerationValue SelectedValue = (EnumerationValue)selectionChangedEventArgs.AddedItems[0];
                    HideUnhidePercentageOrPointBasis((PTTType)SelectedValue.Value);
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

        private void EnableDisableAddOrSet(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    EnumerationValue SelectedValue = (EnumerationValue)selectionChangedEventArgs.AddedItems[0];
                    EnableDisableFields((PTTChangeType)SelectedValue.Value);
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

        public void SetTypeThroughPM(string type)
        {
            try
            {
                PTTPreferences pttPreferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(_loginUser.CompanyUserID);
                if (type == PTTChangeType.Set.ToString())
                    pttPreferences.ChangeType = PTTChangeType.Set;
                else if (type == PTTChangeType.Increase.ToString())
                    pttPreferences.ChangeType = PTTChangeType.Increase;
                else
                    pttPreferences.ChangeType = PTTChangeType.Decrease;

                if (PTTRequestBindingListCollectionView != null)
                {
                    PTTRequestObject pttRequestObject = PTTRequestBindingListCollectionView.SourceCollection.OfType<PTTRequestObject>().ElementAt(0);
                    StringBuilder errorMessage = new StringBuilder();
                    bool isSymbolExist = PTTManager.DoesSymbolExistInPortfolio(pttRequestObject, ref errorMessage);
                    SetChangeTypeFromPreference(pttRequestObject, isSymbolExist);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the symbol based on default symbology.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="bbgSymbol">The BBG symbol.</param>
        public void SetSymbolBasedOnSymbology(string symbol)
        {
            try
            {
                string convertedSymbol = string.Empty;

                if (SymbologyHelper.DefaultSymbology != ApplicationConstants.SymbologyCodes.TickerSymbol)
                {
                    Dictionary<string, SecMasterBaseObj> symbolObjList = _secMasterSyncService.InnerChannel.GetSecMasterSymbolData(new List<string>() { symbol }, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    if (symbolObjList != null && symbolObjList.ContainsKey(symbol))
                    {
                        switch (SymbologyHelper.DefaultSymbology)
                        {
                            case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                convertedSymbol = symbolObjList[symbol].BloombergSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                                convertedSymbol = symbolObjList[symbol].FactSetSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.ActivSymbol:
                                convertedSymbol = symbolObjList[symbol].ActivSymbol;
                                break;
                            default:
                                convertedSymbol = symbolObjList[symbol].TickerSymbol;
                                break;
                        }
                    }
                }
                else
                {
                    convertedSymbol = symbol;
                }

                Symbology = SymbologyHelper.DefaultSymbology;
                SymbolUI = convertedSymbol.ToUpper();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    try
                    {
                        UserAccountCollection = null;
                        PTTAddOrSetCollection = null;
                        TypeCollection = null;
                        CombinedAccountsTotalValueCollection = null;
                        RoundLotPreferenceValueCollection = null;
                        MasterFundOrAccount = null;
                        DisplayMasterFundOrAccountCommand = null;

                        if (_pttRequestBindingListCollectionView != null)
                        {
                            _pttRequestBindingListCollectionView.DetachFromSourceCollection();
                            PTTRequestBindingListCollectionView = null;
                        }

                        if (_pttResponseBindingListCollectionView != null)
                        {
                            _pttResponseBindingListCollectionView.DetachFromSourceCollection();
                            _pttResponseBindingListCollectionView = null;
                        }

                        _securityMaster.SecMstrDataResponse -= _secMasterClient_SecMstrDataResponse;
                        if (_marketDataHelperInstance != null)
                        {
                            _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                            _marketDataHelperInstance.Dispose();
                            _marketDataHelperInstance = null;
                        }
                        if (_secMasterSyncService != null)
                        {
                            _secMasterSyncService.Dispose();
                        }
                        InstanceManager.ReleaseInstance(typeof(PercentTradingToolViewModel));
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
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To Export data for Automation.
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (ExportDataGridForAutomation == true)
                ExportDataGridForAutomation = false;
            ExportFilePathForAutomation = filePath;
            ExportDataGridForAutomation = true;
        }
    }
}