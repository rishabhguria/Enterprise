using Infragistics.Windows.DataPresenter;
using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Interfaces;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.Classes;
using Prana.Rebalancer.CommonViewModel;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Calculator;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ServiceConnector;
using Prana.Utilities.UI;
using Prana.Utilities.ImportExportUtilities;
using Prism.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Forms.Application;
using System.Configuration;
using Prana.Utilities.MiscUtilities.ImportExportUtilities;
using Prana.Global.Utilities;
using ExportGridsData;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class RebalancerViewModel : RebalancerBase, IDisposable, IExportGridData
    {
        #region Commands

        public ICommand CheckComplianceButtonClicked { get; set; }
        public DelegateCommand FetchCommand { get; set; }
        public DelegateCommand RunRebalanceCommand { get; set; }
        public DelegateCommand ModifyRebalanceCommand { get; set; }
        public DelegateCommand AddSecurityCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand ClearRASGridCommand { get; set; }
        public DelegateCommand<object> RemoveSecurityCommand { get; set; }
        public DelegateCommand<object> SaveLayout { get; set; }
        public DelegateCommand<object> RemoveFilters { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand RecalculateCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand ExportRebalancerGridCommand { get; set; }
        public DelegateCommand<object> LockAndUnlockCommand { get; set; }
        public DelegateCommand ViewTradeListCommand { get; set; }
        public DelegateCommand AccountOrGroupChangedCommand { get; set; }
        public DelegateCommand AccountPositionChangedCommand { get; set; }
        public DelegateCommand ModelPortfolioChangedCommand { get; set; }
        public DelegateCommand CashFlowImpactChangedCommand { get; set; }
        public DelegateCommand OpenCustomCashFlowWindowCommand { get; set; }
        public DelegateCommand ClearCalculationCommand { get; set; }
        public DelegateCommand<object> TickerKeyUpCommand { get; set; }
        public DelegateCommand ValidateSecurityCommand { get; set; }
        public DelegateCommand CalulationLevelChangedCommand { get; set; }
        public DelegateCommand<object> RebalacerDataGrid_PreviewKeyDown { get; set; }
        public DelegateCommand LostFocusCommand { get; set; }
        #endregion

        #region Properties

        private SynchronizationContext _uiSyncContext;

        private bool _isCheckComplianceAllowed;
        public bool IsCheckComplianceAllowed
        {
            get { return _isCheckComplianceAllowed; }
            set { SetProperty(ref _isCheckComplianceAllowed, value); }
        }

        private int _rebalancerGroupBoxWidth;
        public int RebalancerGroupBoxWidth
        {
            get { return _rebalancerGroupBoxWidth; }
            set { SetProperty(ref _rebalancerGroupBoxWidth, value); }
        }

        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { SetProperty(ref _loginUser, value); }
        }

        private bool _enableButtons;
        public bool EnableButtons
        {
            get { return _enableButtons; }
            set { SetProperty(ref _enableButtons, value); }
        }

        private bool _complianceTabIsPinned;
        public bool ComplianceTabIsPinned
        {
            get { return _complianceTabIsPinned; }
            set { SetProperty(ref _complianceTabIsPinned, value); }
        }

        private bool _enableAlertsTab;
        public bool EnableAlertsTab
        {
            get { return _enableAlertsTab; }
            set { SetProperty(ref _enableAlertsTab, value); }
        }

        private bool _isFetchPressed;
        public bool IsFetchPressed
        {
            get { return _isFetchPressed; }
            set { SetProperty(ref _isFetchPressed, value); }
        }

        public static Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDict = new Dictionary<int, Dictionary<string, SecurityDataGridModel>>();
        public static Dictionary<string, decimal> symbolWiseYesterDayPrice = new Dictionary<string, decimal>();

        public IBaseCalculator RebalCalculatorInstance { get; set; }
        private TradeListView _tradeListViewInstance;
        private TradeListViewModel _tradeListViewModelInstance;
        private RASImportView _rasImportViewInstance;
        public RASImportViewModel _rasImportViewModelInstance;
        private CustomCashFlowView _customCashFlowWindow;
        BackgroundWorker _bgFetchRebalancerData = null;
        BackgroundWorker _bgRunRebalance = null;
        Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseDictWithUserModifiedModels = new Dictionary<int, Dictionary<string, SecurityDataGridModel>>();
        private System.Windows.Threading.DispatcherTimer _timerSnapShot;
        public bool IsConfirmationRequired = true;
        public List<OrderSingle> listOfOrders;
        public StringBuilder errorMsg = new StringBuilder();

        public delegate Window GetParentWindow();
        public event GetParentWindow GetParentWindowEvent;

        private List<int> _accountIDs;
        public List<int> AccountIDs
        {
            get { return _accountIDs; }
            set { SetProperty(ref _accountIDs, value); }
        }

        private bool _exportRebalancerGrid;
        public bool ExportRebalancerGrid
        {
            get { return _exportRebalancerGrid; }
            set
            {
                _exportRebalancerGrid = value;
                OnPropertyChanged("ExportRebalancerGrid");
            }
        }

        private bool _exportRebalancerGridForAutomation;
        public bool ExportRebalancerGridForAutomation
        {
            get { return _exportRebalancerGridForAutomation; }
            set
            {
                _exportRebalancerGridForAutomation = value;
                OnPropertyChanged("ExportRebalancerGridForAutomation");
            }
        }

        private bool _exportNavGridForAutomation;
        public bool ExportNavGridForAutomation
        {
            get { return _exportNavGridForAutomation; }
            set
            {
                _exportNavGridForAutomation = value;
                OnPropertyChanged("ExportNavGridForAutomation");
            }
        }

        private bool _exportSecurityDataGridForAutomation;
        public bool ExportSecurityDataGridForAutomation
        {
            get { return _exportSecurityDataGridForAutomation; }
            set
            {
                _exportSecurityDataGridForAutomation = value;
                OnPropertyChanged("ExportSecurityDataGridForAutomation");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        private bool _exportEnabled;
        public bool ExportEnabled
        {
            get { return _exportEnabled; }
            set { SetProperty(ref _exportEnabled, value); }
        }

        private bool _isModelSelected;
        public bool IsModelSelected
        {
            get
            {
                if (!IsModifyRebalanceAllowed)
                    return _isModelSelected;
                else
                    return false;
            }
            set
            {
                _isModelSelected = value;
                OnPropertyChanged("IsModelSelected");
            }
        }
        private ComplianceAlertsViewModel _complianceAlertsViewModel;
        public ComplianceAlertsViewModel ComplianceAlertsViewModel
        {
            get { return _complianceAlertsViewModel; }
            set
            {
                _complianceAlertsViewModel = value;
                RaisePropertyChangedEvent("ComplianceAlertsViewModel");
            }
        }

        public TradingRulesBase tradingRulesInstance = new TradingRulesBase();
        public TradingRulesBase TradingRulesInstance
        {
            get { return tradingRulesInstance; }
            set
            {
                tradingRulesInstance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The is ticker symbology
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
            set
            {
                _symbology = value;
                OnPropertyChanged();
            }
        }

        private AccountsModel _oldSelectedAccountOrGroup = null;
        private AccountsModel _selectedAccountOrGroup = null;
        public AccountsModel SelectedAccountOrGroup
        {
            get { return _selectedAccountOrGroup; }
            set
            {
                _selectedAccountOrGroup = value;
                IsRefreshEnable = false;
                OnPropertyChanged();
            }
        }

        private bool AccountOrGroupChanged()
        {
            MessageBoxResult rsltMessageBox = MessageBox.Show("Are you sure you want to change the Account or Position Type?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                            MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (rsltMessageBox == MessageBoxResult.Yes)
            {
                ClearRebalData();
                return true;
            }
            return false;
        }
        private KeyValueItem _oldSelectedPositionType;
        private KeyValueItem _selectedPositionType;
        public KeyValueItem SelectedPositionType
        {
            get { return _selectedPositionType; }
            set
            {

                _selectedPositionType = value;
                IsRefreshEnable = false;

                OnPropertyChanged();

            }
        }
        private KeyValueItem _selectedCalculationLevel;
        public KeyValueItem SelectedCalculationLevel
        {
            get { return _selectedCalculationLevel; }
            set
            {
                _selectedCalculationLevel = value;
                RebalancerCache.Instance.SetCalculationLevel(value.Key);
                OnPropertyChanged();
            }
        }
        private ObservableCollection<KeyValueItem> _positionTypeList;
        public ObservableCollection<KeyValueItem> PositionTypeList
        {
            get { return _positionTypeList; }
            set
            {
                _positionTypeList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<KeyValueItem> _calculationLevelList;
        public ObservableCollection<KeyValueItem> CalculationLevelList
        {
            get { return _calculationLevelList; }
            set
            {
                _calculationLevelList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<AccountsModel> _accountsAndGroupsList = new ObservableCollection<AccountsModel>();

        public ObservableCollection<AccountsModel> AccountsAndGroupsList
        {
            get { return _accountsAndGroupsList; }
            set
            {
                _accountsAndGroupsList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RebalancerModel> _portfolioData = new ObservableCollection<RebalancerModel>();
        public ObservableCollection<RebalancerModel> PortfolioData
        {
            get { return _portfolioData; }
            set
            {
                _portfolioData = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem selectedAccountOrGroupInRAS;
        public KeyValueItem SelectedAccountOrGroupInRAS
        {
            get { return selectedAccountOrGroupInRAS; }
            set
            {
                selectedAccountOrGroupInRAS = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _accountOrGroupLstinRAS = new ObservableCollection<KeyValueItem>();
        public ObservableCollection<KeyValueItem> AccountOrGroupLstinRAS
        {
            get { return _accountOrGroupLstinRAS; }
            set
            {

                _accountOrGroupLstinRAS = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<AdjustedAccountLevelNAV> _navGridData = new ObservableCollection<AdjustedAccountLevelNAV>();
        public ObservableCollection<AdjustedAccountLevelNAV> NAVGridData
        {
            get { return _navGridData; }
            set
            {

                _navGridData = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _rasIncreaseDecreaseOrSetList = null;
        public ObservableCollection<string> RASIncreaseDecreaseOrSetList
        {
            get { return _rasIncreaseDecreaseOrSetList; }
            set
            {
                _rasIncreaseDecreaseOrSetList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _bpsOrPercentageList;
        public ObservableCollection<string> BPSOrPercentageList
        {
            get { return _bpsOrPercentageList; }
            set
            {
                _bpsOrPercentageList = value;
                OnPropertyChanged();
            }
        }


        private string _selectedRASIncreaseDecreaseOrSet = string.Empty;
        public string SelectedRASIncreaseDecreaseOrSet
        {
            get { return _selectedRASIncreaseDecreaseOrSet; }
            set
            {
                _selectedRASIncreaseDecreaseOrSet = value;
                OnPropertyChanged();
            }
        }

        private string _selectedBPSOrPercentage;
        public string SelectedBPSOrPercentage
        {
            get { return _selectedBPSOrPercentage; }
            set
            {
                _selectedBPSOrPercentage = value;
                _targetPercentage = _selectedBPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? _target / 100M : _target;
                OnPropertyChanged();
            }
        }

        private string tickerSymbol = string.Empty;
        public string TickerSymbol
        {
            get { return tickerSymbol; }
            set
            {
                tickerSymbol = value;

                OnPropertyChanged();
            }
        }

        private bool _isRASBusy = true;
        public bool IsRASBusy
        {
            get { return _isRASBusy; }
            set
            {
                _isRASBusy = value;
                OnPropertyChanged();
            }
        }

        private bool _isRASEnable = false;
        public bool IsRASEnable
        {
            get { return _isRASEnable; }
            set
            {
                _isRASEnable = value;
                IsRASBusy = !IsRASEnable;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// InProgress for check compliance
        /// </summary>
        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            set { SetProperty(ref _inProgress, value); }
        }

        /// <summary>
        /// InProgressMessage for check compliance
        /// </summary>
        private string _inProgressMessage;
        public string InProgressMessage
        {
            get { return _inProgressMessage; }
            set { SetProperty(ref _inProgressMessage, value); }
        }

        private ObservableCollection<KeyValueItem> _refreshTypeList;
        public ObservableCollection<KeyValueItem> RefreshTypeList
        {
            get { return _refreshTypeList; }
            set
            {
                _refreshTypeList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Cash Specific Rule
        /// </summary>
        private ObservableCollection<object> _selectedCashSpecificRule = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedCashSpecificRule
        {
            get { return _selectedCashSpecificRule; }
            set
            {
                _selectedCashSpecificRule = value;
                SetCashSpecificRules();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Cash Specific Rule
        /// </summary>
        private ObservableCollection<KeyValueItem> _cashSpecificRule = new ObservableCollection<KeyValueItem>();
        public ObservableCollection<KeyValueItem> CashSpecificRules
        {
            get { return _cashSpecificRule; }
            set
            {
                _cashSpecificRule = value;
            }
        }

        private bool isCashSpecificRule = false;
        public bool IsCashSpecificRule
        {
            get { return isCashSpecificRule; }
            set
            {
                isCashSpecificRule = value;
                OnPropertyChanged();
            }
        }

        private bool isShowCashBlankValidation = false;
        public bool IsShowCashBlankValidation
        {
            get { return isShowCashBlankValidation; }
            set
            {
                isShowCashBlankValidation = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedRefreshType;
        public KeyValueItem SelectedRefreshType
        {
            get { return _selectedRefreshType; }
            set
            {
                _selectedRefreshType = value;
                OnPropertyChanged();
            }
        }

        private decimal _targetPercentage = 0;
        public decimal TargetPercentage
        {
            get { return _targetPercentage; }
            set
            {
                _targetPercentage = value;
                OnPropertyChanged();
            }
        }

        private decimal _target = 0;
        public decimal Target
        {
            get { return _target; }
            set
            {
                _target = value;
                _targetPercentage = _selectedBPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()) ? _target / 100M : _target;
                OnPropertyChanged();
            }
        }

        private decimal _priceValue = 0;
        public decimal PriceValue
        {
            get { return _priceValue; }
            set
            {
                _priceValue = value;
                OnPropertyChanged();
            }
        }



        private decimal _fxValue = 0;
        public decimal FxValue
        {
            get { return _fxValue; }
            set
            {
                _fxValue = value;
                OnPropertyChanged();
            }
        }

        private SecurityDataGridModel currentSecurityDataGridModel = new SecurityDataGridModel();

        public SecurityDataGridModel CurrentSecurityDataGridModel
        {
            get { return currentSecurityDataGridModel; }
            set { currentSecurityDataGridModel = value; }
        }


        private ObservableCollection<SecurityDataGridModel> _securityDataGridList;
        public ObservableCollection<SecurityDataGridModel> SecurityDataGridList
        {
            get { return _securityDataGridList; }
            set
            {
                _securityDataGridList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _actionsOnCashList;
        public ObservableCollection<KeyValueItem> ActionsOnCashList
        {
            get { return _actionsOnCashList; }
            set
            {
                _actionsOnCashList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedCashAction;
        public KeyValueItem SelectedCashAction
        {
            get { return _selectedCashAction; }
            set
            {
                _selectedCashAction = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _modelPortfolioList;
        public ObservableCollection<KeyValueItem> ModelPortfolioList
        {
            get { return _modelPortfolioList; }
            set
            {
                _modelPortfolioList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedCashFlowImpact;
        public KeyValueItem SelectedCashFlowImpact
        {
            get { return _selectedCashFlowImpact; }
            set
            {
                _selectedCashFlowImpact = value;
                RebalancerCache.Instance.SetCashFlowImpact(value.Key);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _cashFlowImpactList;
        public ObservableCollection<KeyValueItem> CashFlowImpactList
        {
            get { return _cashFlowImpactList; }
            set
            {
                _cashFlowImpactList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _oldSelectedModelPortfolio;
        private KeyValueItem _selectedModelPortfolio;
        public KeyValueItem SelectedModelPortfolio
        {
            get { return _selectedModelPortfolio; }
            set
            {
                _selectedModelPortfolio = value;
                if (value != null)
                    IsModelSelected = !value.ItemValue.Equals("None");
                OnPropertyChanged();
            }
        }


        private int _decimalPrecision = 4;

        public int DecimalPrecision
        {
            get { return _decimalPrecision; }
            set
            {
                _decimalPrecision = value;
                OnPropertyChanged();
            }
        }

        private decimal? _cashFlow = 0;
        public decimal? CashFlow
        {
            get { return _cashFlow; }
            set
            {
                if (value != _cashFlow)
                    ResetCustomCashFlow();
                _cashFlow = value;
                OnPropertyChanged();
            }
        }


        private bool _isRefreshEnable = false;
        public bool IsRefreshEnable
        {
            get { return _isRefreshEnable; }
            set
            {
                _isRefreshEnable = value;
            }
        }

        private StatusAndErrorMessageModel _statusAndErrorMessages;

        public StatusAndErrorMessageModel StatusAndErrorMessages
        {
            get { return _statusAndErrorMessages; }
            set
            {
                _statusAndErrorMessages = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<KeyValueItem> _roundingTypeList;
        public ObservableCollection<KeyValueItem> RoundingTypeList
        {
            get { return _roundingTypeList; }
            set
            {
                _roundingTypeList = value;
                OnPropertyChanged();
            }
        }

        private bool _isUseRoundLot;
        public bool IsUseRoundLot
        {
            get { return _isUseRoundLot; }
            set
            {
                _isUseRoundLot = value;
                RebalancerCache.Instance.IsUseRoundLot = value;
                OnPropertyChanged();
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        private bool _isGroupByAreaExpanded;
        public bool IsGroupByAreaExpanded
        {
            get { return _isGroupByAreaExpanded; }
            set
            {
                _isGroupByAreaExpanded = value;
                OnPropertyChanged();
            }
        }

        private GridLength _upperSectionHeight;
        public GridLength UpperSectionHeight
        {
            get { return _upperSectionHeight; }
            set
            {
                _upperSectionHeight = value;
                OnPropertyChanged();
            }
        }

        private GridLength _lowerSectionHeight;
        public GridLength LowerSectionHeight
        {
            get { return _lowerSectionHeight; }
            set
            {
                _lowerSectionHeight = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedRoundingType;
        public KeyValueItem SelectedRoundingType
        {
            get { return _selectedRoundingType; }
            set
            {
                _selectedRoundingType = value;
                RebalancerCache.Instance.SetRoundingType(value.Key);
                OnPropertyChanged();
            }
        }

        private bool _isSymbolValidated;

        public bool IsSymbolValidated
        {
            get { return _isSymbolValidated; }
            set { _isSymbolValidated = value; }
        }

        private bool _isModifyRebalanceAllowed = false;
        public bool IsModifyRebalanceAllowed
        {
            get { return _isModifyRebalanceAllowed; }
            set
            {
                _isModifyRebalanceAllowed = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private ObservableCollection<SymbolModel> _lstSuggestedSymbol = new ObservableCollection<SymbolModel>();

        private bool _clearButtonAction;
        public bool ClearButtonAction
        {
            get { return _clearButtonAction; }
            set
            {
                _clearButtonAction = value;
                RaisePropertyChangedEvent("ClearButtonAction");
            }
        }

        public ObservableCollection<SymbolModel> LstSuggestedSymbol
        {
            get { return _lstSuggestedSymbol; }
            set
            {
                _lstSuggestedSymbol = value;
                OnPropertyChanged();
            }
        }

        private SymbolModel _selectedTickerSymbol;

        public SymbolModel SelectedTickerSymbol
        {
            get { return _selectedTickerSymbol; }
            set
            {
                _selectedTickerSymbol = value;
                OnPropertyChanged();
            }
        }

        #region Constructor

        /// <summary>
        /// RebalancerViewModel
        /// </summary>
        /// <param name="securityMasterInstance"></param>
        /// <param name="rebalancerHelperInstance"></param>
        public RebalancerViewModel(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
            : base(securityMasterInstance, rebalancerHelperInstance)
        {
            try
            {
                LoginUser = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
                LoadMiscellaneousLayoutData();
                if (ComplianceCacheManager.CheckIsBasketComplianceEnabled(_loginUser.CompanyUserID))
                {
                    IsCheckComplianceAllowed = true;
                    RebalancerGroupBoxWidth = 850;
                    CheckComplianceFeedback checkComplianceFeedback = new CheckComplianceFeedback();
                    checkComplianceFeedback.FeedbackMessageReceived += _checkComplianceConnector_FeedbackMessageReceived;
                }
                else
                {
                    IsCheckComplianceAllowed = false;
                    RebalancerGroupBoxWidth = 700;
                }
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                    ExportEnabled = false;
                else
                    ExportEnabled = true;

                _uiSyncContext = SynchronizationContext.Current;
                CheckComplianceButtonClicked = new Prism.Commands.DelegateCommand<object>(CheckComplianceAction, canExecute => IsCheckComplianceAllowed);
                SelectedTickerSymbol = new SymbolModel();
                NAVGridData = new ObservableCollection<AdjustedAccountLevelNAV>();
                SecurityDataGridList = new ObservableCollection<SecurityDataGridModel>();
                BindRebalancerEnums();
                BindModelPortfolios();
                BindAccountsAndCustomGroupsFromCache();
                BindAccountinRAS();
                FetchCommand = new DelegateCommand(() => FetchCommandAction());
                SaveLayout = new DelegateCommand<object>((objRebalancerGrid) => SaveRebalancerGridLayoutAction(objRebalancerGrid));
                RemoveFilters = new DelegateCommand<object>((objRebalancerGrid) => RemoveFilterRebalancerGridLayoutAction(objRebalancerGrid));
                RunRebalanceCommand = new DelegateCommand(() => RunRebalanceAction());
                ModifyRebalanceCommand = new DelegateCommand(() => ModifyRebalanceAction());
                RefreshCommand = new DelegateCommand(() => RefreshCommandAction());
                ClearCommand = new DelegateCommand(() => ClearCommandAction());
                ExportRebalancerGridCommand = new DelegateCommand(() => { ExportRebalancerGrid = true; });
                AddSecurityCommand = new DelegateCommand(() => AddSecurityCommandAction());
                ImportCommand = new DelegateCommand(() => ImportCommandAction());
                ClearRASGridCommand = new DelegateCommand(() => ClearRASGridCommandAction());
                RemoveSecurityCommand = new DelegateCommand<object>(obj => RemoveSecurityCommandAction(obj));
                LockAndUnlockCommand = new DelegateCommand<object>(obj => SetLockUnlockPosition(obj));
                RecalculateCommand = new DelegateCommand(() => RecalculateOrValidateCommandAction());
                ViewTradeListCommand = new DelegateCommand(() => ViewTradeListCommandAction());
                AccountOrGroupChangedCommand = new DelegateCommand(() => AccountOrGroupChangedCommandAction());
                AccountPositionChangedCommand = new DelegateCommand(() => AccountPositionChangedCommandAction());
                CalulationLevelChangedCommand = new DelegateCommand(() => CalulationLevelChangedCommandAction());
                ModelPortfolioChangedCommand = new DelegateCommand(() => ModelPortfolioChangedCommandAction());
                CashFlowImpactChangedCommand = new DelegateCommand(() => CashFlowImpactChangedCommandAction());
                OpenCustomCashFlowWindowCommand = new DelegateCommand(() => OpenCustomCashFlowWindowCommandAction());
                ClearCalculationCommand = new DelegateCommand(() => ClearCalculationCommandAction());
                TickerKeyUpCommand = new DelegateCommand<object>(obj => TickerKeyUpCommandAction(obj));
                ValidateSecurityCommand = new DelegateCommand(() => ValidateSecurityCommandAction());
                LostFocusCommand = new DelegateCommand(() => ValidateCashTargetPercentage());
                RebalacerDataGrid_PreviewKeyDown = new DelegateCommand<object>((objRebalancerGrid) => RebalacerDataGrid_PreviewKeyDownAction(objRebalancerGrid));
                _bgFetchRebalancerData = new BackgroundWorker();
                _bgFetchRebalancerData.DoWork += _bgFetchRebalancerData_DoWork;
                _bgFetchRebalancerData.RunWorkerCompleted += _bgFetchRebalancerData_RunWorkerCompleted;

                _bgRunRebalance = new BackgroundWorker();
                _bgRunRebalance.DoWork += _bgRunRebalance_DoWork;
                _bgRunRebalance.RunWorkerCompleted += _bgRunRebalance_RunWorkerCompleted;

                SecurityMaster.SecMstrDataResponse += _secMasterClient_SecMstrDataResponse;

                _marketDataHelperInstance = MarketDataHelper.GetInstance();
                _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                _timerSnapShot = new DispatcherTimer();
                _timerSnapShot.Tick += new EventHandler(_timerSnapShot_Tick);
                RebalancerCache.Instance.UpdateCustomGroups += RebalancerCache_UpdateCustomGroups;
                RebalancerCache.Instance.UpdateModelPortfolios += RebalancerCache_UpdateModelPortfolios;
                RebalancerCache.Instance.UpdateDataPreferencsOnRebalancerView += RebalanceCache_UpdateDataPreferencsOnRebalancerView;
                StatusAndErrorMessages = new StatusAndErrorMessageModel();
                RebalCalculatorInstance = new BaseCalculator();
                _complianceAlertsViewModel = new ComplianceAlertsViewModel();
                EnableButtonsAndGrid(true);
                SetDockManagerProps(false);
                InstanceManager.RegisterInstance(this);
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
        /// Sets InProgressMessage
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

        private void ValidateSecurityCommandAction()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TickerSymbol))
                {
                    PriceValue = RebalancerCache.Instance.GetSymbolPrice(TickerSymbol);
                    FxValue = RebalancerCache.Instance.GetSymbolFx(TickerSymbol);

                    SendRequestForValidation(Symbology);
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

        private void BindAccountinRAS()
        {
            if (AccountOrGroupLstinRAS == null || AccountOrGroupLstinRAS.Count == 0)
            {
                KeyValueItem kv = new KeyValueItem()
                {
                    Key = 0,
                    ItemValue = RebalancerConstants.CONST_ProRata
                };
                if (!RebalancerCache.Instance.AccountsOrGroupsList.ContainsKey(kv.Key))
                {
                    RebalancerCache.Instance.AccountsOrGroupsList.Add(kv.Key, kv.ItemValue);
                }
                AccountOrGroupLstinRAS.Add(kv);
            }
            if (AccountOrGroupLstinRAS.Count > 0)
                SelectedAccountOrGroupInRAS = AccountOrGroupLstinRAS[0];
        }


        #endregion

        bool isRecursiveCallForModelPortfolio = false;
        private void ModelPortfolioChangedCommandAction()
        {
            try
            {
                if (!isRecursiveCallForModelPortfolio)
                {
                    if (!isRecursiveCallForModelPortfolio && SecurityDataGridList.Count > 0)
                    {
                        ShowErrorAlert("Rebalances Accross Security is in use. First delete the trades in Rebalances Accross Security.");
                        isRecursiveCallForModelPortfolio = true;
                        SelectedModelPortfolio = _oldSelectedModelPortfolio;
                    }
                    _oldSelectedPositionType = SelectedPositionType;
                }
                else
                {
                    isRecursiveCallForModelPortfolio = false;
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
        /// CashFlowImpactChangedCommandAction
        /// </summary>
        private void CashFlowImpactChangedCommandAction()
        {
        }

        /// <summary>
        /// Clear Calculation helper method
        /// Clears all the updated values in the RB grid that comes after Rebalance and reverts them back to he original values
        /// </summary>
        private void ClearCalculation()
        {
            try
            {
                ObservableCollection<RebalancerModel> rebalModels = new ObservableCollection<RebalancerModel>();
                foreach (RebalancerModel rebalancerModel in PortfolioData)
                {
                    if (!rebalancerModel.IsNewlyAdded)
                    {
                        rebalancerModel.TargetPosition = Math.Round(rebalancerModel.Quantity);
                        rebalancerModel.Quantity = Math.Round(rebalancerModel.Quantity);
                        rebalancerModel.IsCalculatedModel = false;
                        rebalancerModel.TolerancePercentage = RebalancerConstants.CONST_NotApplicable;
                        rebalancerModel.ModelPercentage = RebalancerConstants.CONST_NotApplicable;
                        rebalModels.Add(rebalancerModel);
                    }
                }
                foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in NAVGridData)
                {
                    adjustedAccountLevelNAV.CashFlow = 0;
                    adjustedAccountLevelNAV.CashFlowNeeded = 0;
                    adjustedAccountLevelNAV.TargetSecuritiesMarketValue = adjustedAccountLevelNAV.MarketValueForCalculation = adjustedAccountLevelNAV.CurrentSecuritiesMarketValue;
                }
                PortfolioData = rebalModels;
                IsModifyRebalanceAllowed = false;
                if (IsModelSelected)
                {
                    AccountWiseSecurityDataGridModel.Instance.ClearData(AccountWiseDict);
                    SecurityDataGridList = new ObservableCollection<SecurityDataGridModel>();
                }
                OnPropertyChanged("IsModelSelected");
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
        /// ClearCalculationCommandAction
        /// Displays a popup to clear calculation after Rebalnce is done
        /// If yes is selected, values are reveretd back to their original values before rebalance is done
        /// If No, popup is simply dismissed
        /// </summary>
        private void ClearCalculationCommandAction()
        {
            try
            {
                bool result = ShowConfirmationAlert("Do you really want to clear calculation?");
                if (result)
                {
                    ClearCalculation();
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

        private void TickerKeyUpCommandAction(object obj)
        {
            try
            {

                Key keyData = ((System.Windows.Input.KeyEventArgs)obj).Key;
                if (keyData.Equals(Key.Up) || keyData.Equals(Key.Down) || keyData.Equals(Key.Enter) || keyData.Equals(Key.Right) || keyData.Equals(Key.Left))
                    return;

                SecMasterSymbolSearchReq request = new SecMasterSymbolSearchReq(TickerSymbol.Trim(), Symbology);
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
            if (symbolList != null)
            {
                var data = symbolList.Result;
                ObservableCollection<SymbolModel> tempSymbolCollection = new ObservableCollection<SymbolModel>();
                foreach (string item in data)
                {
                    if (tempSymbolCollection.Count(x => x.Symbol == item) == 0)
                        tempSymbolCollection.Add(new SymbolModel { Symbol = item });
                }
                LstSuggestedSymbol = tempSymbolCollection;
            }
        }

        bool isRecursiveCallForPositionType = false;
        private void AccountPositionChangedCommandAction()
        {
            try
            {
                if (!isRecursiveCallForPositionType)
                {
                    if (SelectedPositionType != null)
                    {
                        bool result = false;
                        result = ShowConfirmationAlert("Are you sure you want to change the Position Type?");
                        if (result)
                            ClearRebalData();
                        else
                        {
                            isRecursiveCallForPositionType = true;
                            SelectedPositionType = _oldSelectedPositionType;
                        }
                        _oldSelectedPositionType = SelectedPositionType;
                    }
                }
                else
                {
                    isRecursiveCallForPositionType = false;
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
        private KeyValueItem _oldSelectedCalculationLevel;
        bool isRecursiveCallForCalulationLevel = false;
        private void CalulationLevelChangedCommandAction()
        {
            try
            {
                if (!isRecursiveCallForCalulationLevel)
                {
                    if (SelectedCalculationLevel != null)
                    {
                        bool result = false;
                        result = ShowConfirmationAlert("Are you sure you want to change the Calculation Level?");
                        if (result)
                            ClearRebalData();
                        else
                        {
                            isRecursiveCallForCalulationLevel = true;
                            SelectedCalculationLevel = _oldSelectedCalculationLevel;
                        }
                        _oldSelectedCalculationLevel = SelectedCalculationLevel;
                    }
                }
                else
                {
                    isRecursiveCallForCalulationLevel = false;
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

        private MarketDataHelper _marketDataHelperInstance;


        bool isRecursiveCallForAccountOrGroup = false;
        private void AccountOrGroupChangedCommandAction()
        {
            try
            {
                if (!isRecursiveCallForAccountOrGroup)
                {
                    if (SelectedAccountOrGroup != null)
                    {
                        bool result = false;
                        result = ShowConfirmationAlert("Are you sure you want to change the Account or Group?");
                        if (result)
                            ClearRebalData();
                        else
                        {
                            isRecursiveCallForAccountOrGroup = true;
                            SelectedAccountOrGroup = _oldSelectedAccountOrGroup;
                        }
                        _oldSelectedAccountOrGroup = SelectedAccountOrGroup;
                    }
                }
                else
                {
                    isRecursiveCallForAccountOrGroup = false;
                }
                IsFetchPressed = false;
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
        /// Called to calculate custom cash flow from the dictionary send by the Custom Cash Flow ViewMOdel
        /// </summary>
        /// <param name="customCashFlow"></param>
        /// <param name="customCashFlowDictionary"></param>
        private void CalculateCustomCashFlow(decimal customCashFlow, Dictionary<int, decimal> customCashFlowDictionary)
        {
            try
            {
                if (NAVGridData != null && customCashFlowDictionary != null)
                {
                    CashFlow = customCashFlow;
                    bool isCustomCashFlow = customCashFlow != 0 ? true : false;
                    foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in NAVGridData)
                    {
                        adjustedAccountLevelNAV.IsCustomCashFlow = isCustomCashFlow;
                        adjustedAccountLevelNAV.CustomCashFlow =
                            customCashFlowDictionary[adjustedAccountLevelNAV.AccountId];
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

        private void ResetCustomCashFlow()
        {
            if (NAVGridData != null)
            {
                foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in NAVGridData)
                {
                    adjustedAccountLevelNAV.IsCustomCashFlow = false;
                    adjustedAccountLevelNAV.CustomCashFlow = 0;
                }
            }
        }

        #region Methods


        /// <summary>
        /// OPen Custom Cash Flow Window
        /// </summary>
        private void OpenCustomCashFlowWindowCommandAction()
        {
            try
            {
                if (_customCashFlowWindow == null)
                {
                    CustomCashFlowViewModel customCashFlowModelInstance = new CustomCashFlowViewModel
                    {
                        CustomCashFlow = 0,
                        NAVGridData = NAVGridData
                    };
                    customCashFlowModelInstance.AssignCurrentTotalNavToCashFlow();
                    _customCashFlowWindow = new CustomCashFlowView
                    {
                        DataContext = customCashFlowModelInstance
                    };
                    ElementHost.EnableModelessKeyboardInterop(_customCashFlowWindow);
                    _customCashFlowWindow.Owner = _tradeListViewInstance.Owner;
                    _customCashFlowWindow.Closed += CustomCashFlowWindow_Closed;
                    customCashFlowModelInstance.BindFormatWindow(_tradeListViewInstance.Owner);
                    _customCashFlowWindow.ShowDialog();
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

        private void CustomCashFlowWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (_customCashFlowWindow != null)
                {
                    CustomCashFlowViewModel customCashFlowViewModel = _customCashFlowWindow.DataContext as CustomCashFlowViewModel;
                    if (customCashFlowViewModel != null)
                    {
                        customCashFlowViewModel.AddDataToCashFlowDictionary();
                        CalculateCustomCashFlow(customCashFlowViewModel.CustomCashFlow,
                            customCashFlowViewModel.CustomCashFlowDictionary);
                    }
                    _customCashFlowWindow.Closed -= CustomCashFlowWindow_Closed;
                    customCashFlowViewModel.Dispose();
                    _customCashFlowWindow = null;
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

        internal void BindTradeListView(Window owner)
        {
            _tradeListViewModelInstance = new TradeListViewModel();
            _tradeListViewInstance = new TradeListView(_tradeListViewModelInstance);
            ElementHost.EnableModelessKeyboardInterop(_tradeListViewInstance);
            _tradeListViewInstance.Closing += TradeListViewInstance_Closing;
            _tradeListViewInstance.ShowInTaskbar = true;
            _tradeListViewInstance.Owner = owner;
        }

        public event EventHandler LaunchForm;
        internal void BindRASImportView()
        {
            _rasImportViewModelInstance = new RASImportViewModel(SecurityMaster, RebalancerCache.Instance.RebalancerHelperInstance);
            _rasImportViewModelInstance.UpdatedListFromImport += btnImportContinue_Click;
            _rasImportViewModelInstance.LaunchForm += LaunchForm;
            _rasImportViewInstance = new RASImportView();
            _rasImportViewInstance.DataContext = _rasImportViewModelInstance;
            _rasImportViewModelInstance.SetSedolSymbolColumnVisibility += _rasImportViewInstance.SetSedolSymbolColumnVisibility;
            ElementHost.EnableModelessKeyboardInterop(_rasImportViewInstance);
            _rasImportViewInstance.Closing += RASImportViewInstance_Closing;
            _rasImportViewInstance.ShowInTaskbar = true;
            _rasImportViewInstance.Owner = GetParentWindowEvent();
        }

        private void btnImportContinue_Click(object sender, EventArgs e)
        {
            SecurityDataGridList = AccountWiseSecurityDataGridModel.Instance.GetList(AccountWiseDict);
        }

        private void TradeListViewInstance_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                _tradeListViewInstance.Visibility = Visibility.Hidden;
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

        private void RASImportViewInstance_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (!(_rasImportViewModelInstance.ValidSecuritiesList.Count == 0 && _rasImportViewModelInstance.InvalidSecuritiesList.Count == 0))
                {
                    e.Cancel = MessageBox.Show("Are you sure you want to abort the Import process?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning).Equals(MessageBoxResult.No);
                    if (!e.Cancel)
                    {
                        DisposeEvents();
                        if (!_rasImportViewModelInstance.IsContinueClicked)
                            ImportCommandAction();
                        else
                            _rasImportViewModelInstance.IsContinueClicked = false;
                    }
                }
                else
                {
                    DisposeEvents();
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
        /// Dispose Events
        /// </summary>
        private void DisposeEvents()
        {
            _rasImportViewInstance.Visibility = Visibility.Collapsed;
            _rasImportViewModelInstance.Dispose();
            _rasImportViewModelInstance.LaunchForm -= LaunchForm;
            RebalancerHelperInstance.IsComingFromImport = false;
            RASImportViewModel.AccountWiseDict.Clear();
        }

        private void ViewTradeListCommandAction()
        {
            try
            {
                if (!PortfolioData.Any(x => x.IsCalculatedModel))
                {
                    ShowErrorAlert("Run Rebalance not performed, first Run Rebalance.");
                    return;
                }
                ObservableCollection<TradeListModel> tradeList = GetTradeList();
                if (tradeList.Count > 0)
                {
                    _tradeListViewModelInstance.SetUp(tradeList, SelectedAccountOrGroup, AccountIDs);
                    _tradeListViewInstance.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Trades to send to staging.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                        MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Fetches securities present in the provided file and displays the validated ones on RAS grid.
        /// </summary>
        private void ImportCommandAction()
        {
            try
            {
                int importSymbologyPreference;
                ApplicationConstants.SymbologyCodes importSymbology = SymbologyHelper.DefaultSymbology;
                if (int.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalImportSymbologyPreference, 0),
                    out importSymbologyPreference))
                {
                    if (importSymbologyPreference == (int)RebalancerEnums.ImportSymbology.Sedol)
                    {
                        importSymbology = ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                    }
                }

                // Fetching file and changing it to desired XML schema.
                OpenFileDialogHelper.isComingFromRASImport = true;
                string strFileName = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(true);
                OpenFileDialogHelper.isComingFromRASImport = false;
                DataTable inputDataTable, outputDataTable;
                if (File.Exists(strFileName))
                {
                    string fileFormat = strFileName.Substring(strFileName.LastIndexOf(".") + 1);
                    if (fileFormat.ToUpperInvariant().Equals("XLS") || fileFormat.ToUpperInvariant().Equals("XLSX") || fileFormat.ToUpperInvariant().Equals("CSV"))
                    {
                        inputDataTable = fileFormat.ToUpperInvariant().Equals("CSV") ? FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(strFileName) : CustomRASImportReadingStrategy.GetDataTableFromUploadedDataFile(strFileName, fileFormat);
                        if (inputDataTable == null)
                        {
                            MessageBox.Show("File in use! Please close the file and retry.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        inputDataTable.TableName = "RASImport";
                        outputDataTable = XSLTTransform(inputDataTable);
                    }
                    else
                    {
                        MessageBox.Show("Uploaded file schema is not as per requirement.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                    return;

                // Fetching the securities from DataTable and sending them to Import UI.
                ObservableCollection<SecurityDataGridModel> importedSecuritiesList = new ObservableCollection<SecurityDataGridModel>();
                if (outputDataTable != null)
                {
                    RebalancerHelperInstance.DataTableToObservableCollection(outputDataTable, ref importedSecuritiesList);
                }
                else
                {
                    MessageBox.Show("Uploaded file schema is not as per requirement.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (importedSecuritiesList.Count > 0)
                {
                    if (SecurityDataGridList != null && SecurityDataGridList.Count > 0)
                    {
                        SecurityDataGridList.Clear();
                    }
                    AccountWiseSecurityDataGridModel.Instance.ClearData(AccountWiseDict);
                    AccountWiseSecurityDataGridModel.Instance.ClearData(RASImportViewModel.AccountWiseDict);
                    RebalancerHelperInstance.IsComingFromImport = true;
                    BindRASImportView();

                    _rasImportViewModelInstance.SetUp(importedSecuritiesList, importSymbology);
                    _rasImportViewInstance.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Securities to display.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                if (ex is Newtonsoft.Json.JsonSerializationException)
                {
                    DialogService.DialogServiceInstance.ShowMessageBox(this, "Imported data is of wrong format.",
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning,
                        MessageBoxResult.OK, true);
                }
                else
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable XSLTTransform(DataTable dTable)
        {
            try
            {
                string tempPath = Application.StartupPath;

                string inputXML = tempPath + "\\InputXML.xml";
                string outputXML = tempPath + "\\OutPutXML.xml";

                string path = Application.StartupPath;
                string xsltName = ConfigurationManager.AppSettings["RebalancerImport"];
                string xsltPath = path + "\\" + xsltName;

                dTable.WriteXml(inputXML);

                System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
                xslt.Load(xsltPath);
                xslt.Transform(inputXML, outputXML);

                DataSet ds = new DataSet();
                ds.ReadXml(outputXML);
                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    dTable = ds.Tables[0];
                }
                else
                {
                    dTable = null;
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
            return dTable;
        }


        /// <summary>
        /// GetTradeList
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<TradeListModel> GetTradeList()
        {
            ObservableCollection<TradeListModel> tradeList = new ObservableCollection<TradeListModel>();
            ConcurrentDictionary<int, List<int>> customGroups = RebalancerCache.Instance.GetCustomGroupAndAccountsAssociation();
            string _accountOrGroupNameToRebalance = string.Empty;
            foreach (RebalancerModel rebalancerModel in PortfolioData)
            {
                _accountOrGroupNameToRebalance = string.Empty;
                if (rebalancerModel.BuySellQty != 0 && !rebalancerModel.IsCustomModel)
                {
                    foreach (var item in customGroups)
                    {
                        if (item.Value.Contains(rebalancerModel.AccountId) && !RebalancerCache.Instance.GetCustomGroupName(item.Key).ToLower().ToString().Equals("all"))
                        {
                            _accountOrGroupNameToRebalance = RebalancerCache.Instance.GetCustomGroupName(item.Key);
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(_accountOrGroupNameToRebalance))
                    {
                        var masterFundid = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(rebalancerModel.AccountId);
                        _accountOrGroupNameToRebalance = CachedDataManager.GetInstance.GetMasterFund(masterFundid).Trim();
                    }

                    rebalancerModel.AccountOrGroupNameToRebalance = _accountOrGroupNameToRebalance.Trim();
                    SetYesterdayPrice(rebalancerModel);
                    tradeList.Add(new TradeListModel(rebalancerModel));
                }
            }
            return tradeList;
        }

        private void RebalancerCache_UpdateModelPortfolios(object sender, EventArgs e)
        {
            BindModelPortfolios();
        }

        private void RebalancerCache_UpdateCustomGroups(object sender, EventArgs e)
        {
            IsConfirmationRequired = false;
            BindAccountsAndCustomGroupsFromCache();
            IsConfirmationRequired = true;
        }

        private void RebalanceCache_UpdateDataPreferencsOnRebalancerView(object sender, EventArgs e)
        {
            BindTradingRulesFromCache();
            BindRASIncreaseDecreaseOrSetFromCache();
            BindIsExpandedFromCache();
            BindRoundingTypeFromCache();
            IsSetCashSpecificRules();
        }

        private void BindTradingRulesFromCache()
        {
            try
            {
                TradingRulesBase tradingRulesBase = JsonConvert.DeserializeObject<TradingRulesBase>(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalTradingRulesPref, 0));
                if (tradingRulesBase != null)
                {
                    TradingRulesInstance = tradingRulesBase;
                }

                ObservableCollection<object> selectedCashSpecificRules = new ObservableCollection<object>();
                foreach (KeyValueItem item in _cashSpecificRule)
                {
                    bool isChecked = false;
                    switch (item.Key)
                    {
                        case (int)RebalancerEnums.CashSpecificRules.SetCashTarget:
                            isChecked = TradingRulesInstance.IsSetCashTarget;
                            break;
                        case (int)RebalancerEnums.CashSpecificRules.SellToRaiseCash:
                            isChecked = TradingRulesInstance.IsSellToRaiseCash;
                            break;
                        case (int)RebalancerEnums.CashSpecificRules.AllowNegativeCash:
                            isChecked = TradingRulesInstance.IsNegativeCashAllowed;
                            break;
                    }
                    if (isChecked) selectedCashSpecificRules.Add(item);
                }
                SelectedCashSpecificRule = selectedCashSpecificRules;
                TradingRulesInstance.CashTarget = null;
                IsShowCashBlankValidation = false;
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
        }

        /// <summary>
        /// Set Cash Specific Trading Rules
        /// </summary>
        private void SetCashSpecificRules()
        {
            try
            {
                foreach (KeyValueItem item in _cashSpecificRule)
                {
                    bool isChecked = _selectedCashSpecificRule.Contains(item);
                    switch (item.Key)
                    {
                        case (int)RebalancerEnums.CashSpecificRules.SetCashTarget:
                            if (TradingRulesInstance.IsSetCashTarget != isChecked)
                                IsShowCashBlankValidation = false;
                            TradingRulesInstance.IsSetCashTarget = isChecked;
                            break;
                        case (int)RebalancerEnums.CashSpecificRules.SellToRaiseCash:
                            TradingRulesInstance.IsSellToRaiseCash = isChecked;
                            break;
                        case (int)RebalancerEnums.CashSpecificRules.AllowNegativeCash:
                            TradingRulesInstance.IsNegativeCashAllowed = isChecked;
                            break;
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
        }

        /// <summary>
        /// Set value of IsCashSpecificRule property
        /// </summary>
        public void IsSetCashSpecificRules()
        {
            try
            {
                IsCashSpecificRule = TradingRulesInstance.IsSellToRaiseCash || TradingRulesInstance.IsNegativeCashAllowed || TradingRulesInstance.IsSetCashTarget;
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
        }

        private void BindIsExpandedFromCache()
        {
            bool isExpanded;
            if (bool.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalExpandAcrossSecurities, 0), out isExpanded))
                IsExpanded = isExpanded;
        }

        private void BindRASIncreaseDecreaseOrSetFromCache()
        {
            int selectedRASPref;
            if (Int32.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalRASPrefrence, 0), out selectedRASPref))
                SelectedRASIncreaseDecreaseOrSet = RASIncreaseDecreaseOrSetList[selectedRASPref];
        }

        private void BindRoundingTypeFromCache()
        {
            int selectedRoundingType;
            if (Int32.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalRoundingType, 0), out selectedRoundingType))
            {
                SelectedRoundingType = RoundingTypeList.FirstOrDefault(type =>
                       type.Key.Equals(selectedRoundingType));
            }
            else
            {
                SelectedRoundingType = RoundingTypeList.SingleOrDefault(p => p.Key == 3);
            }
        }

        private void SaveRebalancerGridLayoutAction(object objRebalancerGrid)
        {
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string rebalancerPreferencesDirectoryPath = startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                if (objRebalancerGrid != null)
                {
                    XamDataGrid rebalancerGrid = objRebalancerGrid as XamDataGrid;

                    if (rebalancerGrid != null)
                    {
                        string rebalancerGridPreferencesFilePath = rebalancerPreferencesDirectoryPath + @"\RebalancerGridLayout.xml";

                        using (FileStream fs = new FileStream(rebalancerGridPreferencesFilePath, FileMode.Create, FileAccess.Write))
                        {
                            rebalancerGrid.SaveCustomizations(fs);
                        }
                    }
                }

                string rebalancerMiscPreferencesFilePath = rebalancerPreferencesDirectoryPath + @"\RebalancerMiscLayout.xml";
                File.WriteAllText(rebalancerMiscPreferencesFilePath, UpperSectionHeight.ToString() + Environment.NewLine + LowerSectionHeight.ToString() + Environment.NewLine + IsGroupByAreaExpanded.ToString());
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

        private void LoadMiscellaneousLayoutData()
        {
            try
            {
                string rebalancerMiscPreferencesFilePath = System.Windows.Forms.Application.StartupPath +
                                                            "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" +
                                                            CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString() +
                                                            @"\RebalancerMiscLayout.xml";
                string[] lines = File.Exists(rebalancerMiscPreferencesFilePath) ? File.ReadAllLines(rebalancerMiscPreferencesFilePath) : null;

                if (lines != null && lines.Length > 2)
                {
                    double value;
                    if (lines[0].EndsWith("*") && double.TryParse(lines[0].TrimEnd('*'), out value))
                        UpperSectionHeight = new GridLength(value, GridUnitType.Star);
                    else if (double.TryParse(lines[0], out value))
                        UpperSectionHeight = new GridLength(value, GridUnitType.Pixel);
                    else
                        UpperSectionHeight = new GridLength(4, GridUnitType.Star);

                    if (lines[1].EndsWith("*") && double.TryParse(lines[1].TrimEnd('*'), out value))
                        LowerSectionHeight = new GridLength(value, GridUnitType.Star);
                    else if (double.TryParse(lines[1], out value))
                        LowerSectionHeight = new GridLength(value, GridUnitType.Pixel);
                    else
                        LowerSectionHeight = new GridLength(5, GridUnitType.Star);

                    bool openOrClosed;
                    if (bool.TryParse(lines[2], out openOrClosed))
                        IsGroupByAreaExpanded = openOrClosed;
                    else
                        IsGroupByAreaExpanded = false;
                }
                else
                {
                    UpperSectionHeight = new GridLength(4, GridUnitType.Star);
                    LowerSectionHeight = new GridLength(5, GridUnitType.Star);
                    IsGroupByAreaExpanded = false;
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

        private void RemoveFilterRebalancerGridLayoutAction(object objRebalancerGrid)
        {
            try
            {
                if (objRebalancerGrid != null)
                {
                    XamDataGrid rebalancerGrid = objRebalancerGrid as XamDataGrid;

                    if (rebalancerGrid != null)
                    {
                        rebalancerGrid.ClearCustomizations(CustomizationType.RecordFilters);
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
        private void LoadRebalancerGridLayout(object objRebalancerGrid)
        {
            try
            {
                if (objRebalancerGrid != null)
                {
                    XamDataGrid rebalancerGrid = objRebalancerGrid as XamDataGrid;
                    if (rebalancerGrid != null)
                    {
                        string startPath = System.Windows.Forms.Application.StartupPath;
                        string rebalancerPreferencesDirectoryPath = startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                        string rebalancerGridPreferencesFilePath = rebalancerPreferencesDirectoryPath + @"\RebalancerGridLayout.xml";

                        if (File.Exists(rebalancerGridPreferencesFilePath))
                        {
                            using (FileStream fs = new FileStream(rebalancerGridPreferencesFilePath, FileMode.Open, FileAccess.Read))
                            {
                                rebalancerGrid.LoadCustomizations(fs);
                            }
                        }
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

        private bool ShowConfirmationAlert(string message)
        {
            bool result = true;
            try
            {
                if ((PortfolioData.Count > 0 || NAVGridData.Count > 0 || CashFlow != 0 || SecurityDataGridList.Count > 0) && IsConfirmationRequired)
                {
                    MessageBoxResult rsltMessageBox = MessageBox.Show(message, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                            MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (rsltMessageBox != MessageBoxResult.Yes)
                    {
                        result = false;
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
            return result;
        }

        /// <summary>
        /// Clear Command Action
        /// </summary>
        private void ClearCommandAction()
        {
            try
            {
                bool result = ShowConfirmationAlert("Do you want to clear the data?");
                if (result)
                {
                    ClearRebalData();
                    ClearButtonAction = true;
                    ClearButtonAction = false;
                    IsShowCashBlankValidation = false;
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

        private void ClearRebalData()
        {
            try
            {
                if (IsConfirmationRequired)
                {
                    ClearRebalancerData();
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

        private void ClearRebalancerData()
        {
            if (PortfolioData != null && PortfolioData.Count > 0)
                PortfolioData.Clear();
            if (NAVGridData != null && NAVGridData.Count > 0)
                NAVGridData.Clear();
            if (SecurityDataGridList != null && SecurityDataGridList.Count > 0)
                SecurityDataGridList.Clear();
            if (RebalancerCache.Instance.AccountsOrGroupsList != null && RebalancerCache.Instance.AccountsOrGroupsList.Count > 0)
                RebalancerCache.Instance.AccountsOrGroupsList.Clear();
            if (AccountOrGroupLstinRAS != null && AccountOrGroupLstinRAS.Count > 0)
            {
                AccountOrGroupLstinRAS = new ObservableCollection<KeyValueItem>();
                BindAccountinRAS();
            }
            if (ComplianceAlertsViewModel.AlertsList != null)
            {
                ComplianceAlertsViewModel.AlertsList.Clear();
                SetDockManagerProps(false);
            }

            AccountWiseSecurityDataGridModel.Instance.ClearData(AccountWiseDict);
            EnableRebalancerSecurityUI(false);
            CashFlow = 0;
        }

        private void ClearRASGridCommandAction()
        {
            if (SecurityDataGridList != null && SecurityDataGridList.Count > 0 &&
                    MessageBox.Show("Are you sure you want to delete all the Securities?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning).Equals(MessageBoxResult.Yes))
            {
                SecurityDataGridList.Clear();
                AccountWiseSecurityDataGridModel.Instance.ClearData(AccountWiseDict);
            }
        }

        /// <summary>
        /// Refresh Command Action
        /// </summary>
        private void RefreshCommandAction()
        {
            try
            {
                if (SelectedRefreshType.Key == (int)(RebalancerEnums.RefreshTypes.Both))
                {
                    Parallel.Invoke(() => { RefreshPrices(); }
                 , () => { RefreshPositions(); });
                }
                if (SelectedRefreshType.Key == (int)(RebalancerEnums.RefreshTypes.Prices))
                {
                    RefreshPrices();
                }
                if (SelectedRefreshType.Key == (int)(RebalancerEnums.RefreshTypes.Positions))
                {
                    RefreshPositions();

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

        private void RefreshPositions()
        {
            Dictionary<string, List<int>> symbolsAndAccountIdsDict = new Dictionary<string, List<int>>();
            StringBuilder errorMessage = new StringBuilder();
            foreach (RebalancerModel rebalancerModel in PortfolioData)
            {
                if (rebalancerModel.IsLock == false && rebalancerModel.IsCustomModel == false)
                {
                    if (!symbolsAndAccountIdsDict.ContainsKey(rebalancerModel.Symbol))
                    {
                        List<int> lstAccountIds = new List<int>();
                        lstAccountIds.Add(rebalancerModel.AccountId);
                        symbolsAndAccountIdsDict.Add(rebalancerModel.Symbol, lstAccountIds);
                    }
                    else
                    {
                        if (!symbolsAndAccountIdsDict[rebalancerModel.Symbol].Contains(rebalancerModel.AccountId))
                            symbolsAndAccountIdsDict[rebalancerModel.Symbol].Add(rebalancerModel.AccountId);
                    }
                }
            }
            Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>> dictRefreshPositions = ExpnlServiceConnector.GetInstance().RefreshPositions(symbolsAndAccountIdsDict, ref errorMessage);
            foreach (RebalancerModel rebalancerModel in PortfolioData)
            {
                if (rebalancerModel.IsLock == false && dictRefreshPositions.ContainsKey(rebalancerModel.Symbol))
                {
                    if (dictRefreshPositions[rebalancerModel.Symbol].ContainsKey(rebalancerModel.AccountId))
                    {
                        if (rebalancerModel.Asset.Equals("EquitySwap"))
                        {
                            rebalancerModel.Quantity = Math.Abs(dictRefreshPositions[rebalancerModel.Symbol][rebalancerModel.AccountId].Item2);
                            rebalancerModel.Side = dictRefreshPositions[rebalancerModel.Symbol][rebalancerModel.AccountId].Item2 > 0 ? PositionType.Long : PositionType.Short;

                        }
                        else
                        {
                            rebalancerModel.Quantity = Math.Abs(dictRefreshPositions[rebalancerModel.Symbol][rebalancerModel.AccountId].Item1);
                            rebalancerModel.Side = dictRefreshPositions[rebalancerModel.Symbol][rebalancerModel.AccountId].Item1 > 0 ? PositionType.Long : PositionType.Short;
                        }
                        rebalancerModel.TolerancePercentage = RebalancerConstants.CONST_NotApplicable;
                        rebalancerModel.ModelPercentage = RebalancerConstants.CONST_NotApplicable;
                    }
                }
            }
        }

        private void RefreshPrices()
        {
            List<string> symbolLst = new List<string>();
            StringBuilder errorMessage = new StringBuilder();
            symbolLst = PortfolioData.Where(s => s.IsLock == false && s.IsCustomModel == false).Select(s => s.Symbol).Distinct().ToList<string>();
            Dictionary<string, decimal> dictRefreshPrices = ExpnlServiceConnector.GetInstance().RefreshPrices(symbolLst, ref errorMessage);
            if (errorMessage == null || string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                foreach (RebalancerModel rebalancerModel in PortfolioData)
                {
                    if (rebalancerModel.IsLock == false && dictRefreshPrices.ContainsKey(rebalancerModel.Symbol))
                    {
                        rebalancerModel.Price = dictRefreshPrices[rebalancerModel.Symbol];
                        rebalancerModel.IsStaleClosingMark = false;
                        rebalancerModel.TolerancePercentage = RebalancerConstants.CONST_NotApplicable;
                        rebalancerModel.ModelPercentage = RebalancerConstants.CONST_NotApplicable;
                    }
                }
            }
            foreach (KeyValuePair<string, decimal> kvp in dictRefreshPrices)
            {
                RebalancerCache.Instance.AddOrUpdateSymbolWisePriceAndFx(kvp.Key, kvp.Value, RebalancerCache.Instance.GetSymbolFx(kvp.Key));
            }
        }

        /// <summary>
        /// Remove Security Command Action
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveSecurityCommandAction(object obj)
        {
            try
            {
                if (obj != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete this Security?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning).Equals(MessageBoxResult.Yes))
                    {
                        SecurityDataGridModel securityDataGridModel = obj as SecurityDataGridModel;
                        AccountWiseSecurityDataGridModel.Instance.RemoveData(securityDataGridModel, AccountWiseDict);
                        BindSecurityDataGridList();
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
        /// 
        /// </summary>
        private void BindRebalancerEnums()
        {
            try
            {
                PositionTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.RebalancerPositionsType>();
                if (PositionTypeList.Count > 0)
                {
                    SelectedPositionType = PositionTypeList.SingleOrDefault(p => p.Key == 1);
                    _oldSelectedPositionType = SelectedPositionType;
                }

                CashFlowImpactList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.CashFlowImpactOnNAV>();
                if (CashFlowImpactList.Count > 0)
                {
                    SelectedCashFlowImpact = CashFlowImpactList.SingleOrDefault(p => p.Key == 1);
                }
                CalculationLevelList =
                    RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.CalculationLevel>();
                if (CalculationLevelList.Count > 0)
                {
                    SelectedCalculationLevel = CalculationLevelList.SingleOrDefault(p => p.Key == 1);
                    _oldSelectedCalculationLevel = SelectedCalculationLevel;
                }
                RASIncreaseDecreaseOrSetList = new ObservableCollection<string>(Enum.GetNames(typeof(RebalancerEnums.RASIncreaseDecreaseOrSet)));
                if (RASIncreaseDecreaseOrSetList.Count > 0)
                {
                    BindRASIncreaseDecreaseOrSetFromCache();
                }
                BPSOrPercentageList = new ObservableCollection<string>(Enum.GetNames(typeof(RebalancerEnums.BPSOrPercentage)));
                if (BPSOrPercentageList.Count > 0)
                {
                    SelectedBPSOrPercentage = BPSOrPercentageList[(int)RebalancerEnums.BPSOrPercentage.Percentage];
                }
                BindIsExpandedFromCache();
                CashSpecificRules = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.CashSpecificRules>();
                BindTradingRulesFromCache();
                RefreshTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.RefreshTypes>();
                if (RefreshTypeList.Count > 0)
                    SelectedRefreshType = RefreshTypeList.SingleOrDefault(p => p.Key == 1);
                ActionsOnCashList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.ActionsOnCash>();
                if (ActionsOnCashList.Count > 0)
                    SelectedCashAction = ActionsOnCashList.SingleOrDefault(p => p.Key == 1);
                RoundingTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.RoundingTypes>();
                if (RoundingTypeList.Count > 0)
                    BindRoundingTypeFromCache();
                IsUseRoundLot = TradingTktPrefs.UserTradingTicketUiPrefs.IsUseRoundLots;
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
        /// Add Security Command Action
        /// </summary>
        private void AddSecurityCommandAction()
        {
            try
            {
                if (!IsModifyRebalanceAllowed && !SelectedModelPortfolio.ItemValue.Equals("None"))
                {
                    ShowErrorAlert("Mode Portfolio is selected. First unselect model portfolio.");
                    return;
                }

                bool isValidated = ValidateRabalanceAcrossSecurityData();

                if (isValidated)
                {
                    AddUpdateStatusAndMessage(string.Empty, string.Empty);
                    SecurityDataGridModel model = new SecurityDataGridModel()
                    {
                        Symbol = Symbology == ApplicationConstants.SymbologyCodes.TickerSymbol ? TickerSymbol.Trim() : currentSecurityDataGridModel.Symbol,
                        FactSetSymbol = currentSecurityDataGridModel.FactSetSymbol,
                        ActivSymbol = currentSecurityDataGridModel.ActivSymbol,
                        BloombergSymbol = currentSecurityDataGridModel.BloombergSymbol,
                        AUECID = currentSecurityDataGridModel.AUECID,
                        RoundLot = currentSecurityDataGridModel.RoundLot,
                        IncreaseDecreaseOrSet = SelectedRASIncreaseDecreaseOrSet,
                        BPSOrPercentage = SelectedBPSOrPercentage,
                        Target = Target,
                        TargetPercentage = TargetPercentage,
                        Price = PriceValue,
                        AccountOrGroupId = SelectedAccountOrGroupInRAS.Key,
                        AccountOrGroupName = SelectedAccountOrGroupInRAS.ItemValue,
                        Asset = currentSecurityDataGridModel.Asset,
                        FXRate = FxValue,
                        Multiplier = currentSecurityDataGridModel.Multiplier,
                        Sector = currentSecurityDataGridModel.Sector,
                        Delta = currentSecurityDataGridModel.Delta,
                        LeveragedFactor = currentSecurityDataGridModel.LeveragedFactor,
                        BloombergSymbolWithExchangeCode = currentSecurityDataGridModel.BloombergSymbolWithExchangeCode
                    };

                    string error = AccountWiseSecurityDataGridModel.Instance.IsModelValid(model, AccountWiseDict);
                    if (error.Length > 0)
                    {
                        ShowErrorAlert(error);
                        return;
                    }
                    else
                        AccountWiseSecurityDataGridModel.Instance.AddModel(model, AccountWiseDict);
                    SelectedBPSOrPercentage = BPSOrPercentageList[(int)RebalancerEnums.BPSOrPercentage.Percentage];
                    BindSecurityDataGridList();
                    EnableRebalancerSecurityUI(false);
                    TickerSymbol = string.Empty;
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

        private static void SetYesterdayPrice(RebalancerModel model)
        {
            try
            {
                if (symbolWiseYesterDayPrice != null && symbolWiseYesterDayPrice.ContainsKey(model.Symbol))
                {
                    model.YesterdayMarkPrice = symbolWiseYesterDayPrice[model.Symbol];
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

        private bool ValidateRabalanceAcrossSecurityData()
        {
            decimal targetPercentage = Target;
            if (SelectedBPSOrPercentage.Equals(RebalancerEnums.BPSOrPercentage.BPS.ToString()))
            {
                targetPercentage /= 100M;
            }
            if (!SelectedRASIncreaseDecreaseOrSet.Equals(RebalancerConstants.CONST_SET) && targetPercentage <= 0)
            {
                ShowErrorAlert("Target percentage should be greater than zero.");
                AddUpdateStatusAndMessage(string.Empty, "Target percentage should be greater than zero.");
                return false;
            }
            else if (SelectedRASIncreaseDecreaseOrSet.Equals(RebalancerConstants.CONST_SET) && targetPercentage < -100)
            {
                ShowErrorAlert("Target percentage should be greater than -100%.");
                AddUpdateStatusAndMessage(string.Empty, "Target percentage should be greater than -100%.");
                return false;
            }
            else if (targetPercentage > 100)
            {
                ShowErrorAlert("Target percentage should be less than 100%.");
                AddUpdateStatusAndMessage(string.Empty, "Target percentage should be less than 100%.");
                return false;
            }
            else if (PriceValue <= 0)
            {
                ShowErrorAlert("Price should be greater than zero.");
                AddUpdateStatusAndMessage(string.Empty, "Price should be greater than zero.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TickerSymbol))
            {
                ShowErrorAlert("Ticker Symbol can not be blank.");
                AddUpdateStatusAndMessage(string.Empty, "Ticker Symbol can not be blank.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates data in Set Cash % field
        /// </summary>
        private bool ValidateCashTargetPercentage()
        {
            try
            {
                decimal? targetPercentage = TradingRulesInstance.CashTarget;
                if (TradingRulesInstance.CashTarget != null) IsShowCashBlankValidation = false;
                if (targetPercentage < 0)
                {
                    ShowErrorAlert("Target percentage should be greater than 0%.");
                    TradingRulesInstance.CashTarget = null;
                    return false;
                }
                else if (targetPercentage > 100)
                {
                    ShowErrorAlert("Target percentage should be less than 100%.");
                    TradingRulesInstance.CashTarget = null;
                    return false;
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
            return true;
        }

        /// <summary>
        /// BindSecurityDataGridList
        /// </summary>
        private void BindSecurityDataGridList()
        {
            SecurityDataGridList.Clear();
            SecurityDataGridList = AccountWiseSecurityDataGridModel.Instance.GetList(AccountWiseDict);
        }

        /// <summary>
        /// Bind Model Portfolios
        /// </summary>
        private void BindModelPortfolios()
        {
            ObservableCollection<KeyValueItem> modelPortfolios = new ObservableCollection<KeyValueItem>();
            modelPortfolios.Add(new KeyValueItem()
            {
                Key = 0,
                ItemValue = RebalancerConstants.CONST_None
            });
            modelPortfolios.AddRange(new ObservableCollection<KeyValueItem>(RebalancerCache.Instance.GetAllModelPortfolioNames()));
            ModelPortfolioList = modelPortfolios;
            if (ModelPortfolioList != null && ModelPortfolioList.Count > 0)
            {
                SelectedModelPortfolio = ModelPortfolioList.SingleOrDefault(p => p.Key == 0);
                _oldSelectedModelPortfolio = SelectedModelPortfolio;
            }
        }

        private void AddUpdateStatusAndMessage(string statusMessage, string errorMessage)
        {
            StatusAndErrorMessages.StatusMessage = statusMessage;
            StatusAndErrorMessages.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Sends the request for validation.
        /// </summary>
        private void SendRequestForValidation(ApplicationConstants.SymbologyCodes tickerSymbology)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.AddData(TickerSymbol.Trim(), tickerSymbology);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = GetHashCode();
                if (SecurityMaster != null)
                {
                    EnableRebalancerSecurityUI(false);
                    AddUpdateStatusAndMessage(RebalancerConstants.MSG_WAITING_FOR_VALIDATION, string.Empty);
                    SecurityMaster.SendRequest(reqObj);
                    _timerSnapShot.Interval = new TimeSpan(TimeSpan.TicksPerSecond * 5);
                    _timerSnapShot.Start();
                }
                else
                {
                    AddUpdateStatusAndMessage(string.Empty, string.Empty);
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

        public override int GetHashCode()
        {
            try
            {
                return base.GetHashCode();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return Int32.MinValue;
            }
        }

        /// <summary>
        /// SecMasterClient SecMstr DataResponse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _secMasterClient_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (Dispatcher.CurrentDispatcher.CheckAccess())
                {
                    if (e.Value != null)
                    {
                        string securitySymbol = string.Empty;
                        switch (Symbology)
                        {
                            case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                                securitySymbol = e.Value.FactSetSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.ActivSymbol:
                                securitySymbol = e.Value.ActivSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                securitySymbol = e.Value.BloombergSymbol;
                                break;
                            default:
                                securitySymbol = e.Value.TickerSymbol;
                                break;
                        }
                        if (securitySymbol.ToUpper() == TickerSymbol.ToUpper())
                        {
                            IsSymbolValidated = true;

                            SecMasterBaseObj iSecMasterBaseObj = e.Value;
                            if (!(iSecMasterBaseObj.AssetCategory.Equals(AssetCategory.Equity) || iSecMasterBaseObj.AssetCategory.Equals(AssetCategory.PrivateEquity) || iSecMasterBaseObj.AssetCategory == AssetCategory.FixedIncome))
                            {
                                AddUpdateStatusAndMessage(string.Empty, string.Format(RebalancerConstants.ERR_NON_PERMITTED_ASSET, iSecMasterBaseObj.AssetCategory.ToString()));
                            }
                            else
                            {
                                if (Symbology == ApplicationConstants.SymbologyCodes.TickerSymbol)
                                {
                                    if (!RebalancerCache.Instance.IsSymbolExistInGrid(TickerSymbol) || PriceValue.Equals(0) || FxValue.Equals(0))
                                    {
                                        if (_marketDataHelperInstance != null)
                                        {
                                            if (!_marketDataHelperInstance.IsDataManagerConnected())
                                            {
                                                ShowErrorAlert(RebalancerConstants.MSG_LIVE_FEED_DISCONNECTED);
                                            }
                                            else
                                            {
                                                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                                                {
                                                    _marketDataHelperInstance.RequestSingleSymbol(((SecMasterBaseObj)e.Value).TickerSymbol, true);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!RebalancerCache.Instance.IsSymbolExistInGrid(iSecMasterBaseObj.TickerSymbol) || PriceValue.Equals(0) || FxValue.Equals(0))
                                    {
                                        if (_marketDataHelperInstance != null)
                                        {
                                            if (!_marketDataHelperInstance.IsDataManagerConnected())
                                            {
                                                ShowErrorAlert(RebalancerConstants.MSG_LIVE_FEED_DISCONNECTED);
                                            }
                                            else
                                            {
                                                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                                                {
                                                    _marketDataHelperInstance.RequestSingleSymbol(((SecMasterBaseObj)e.Value).TickerSymbol, true);
                                                }
                                            }
                                        }
                                    }
                                }
                                currentSecurityDataGridModel.AUECID = iSecMasterBaseObj.AUECID;
                                currentSecurityDataGridModel.Symbol = iSecMasterBaseObj.TickerSymbol;
                                currentSecurityDataGridModel.FactSetSymbol = iSecMasterBaseObj.FactSetSymbol;
                                currentSecurityDataGridModel.ActivSymbol = iSecMasterBaseObj.ActivSymbol;
                                currentSecurityDataGridModel.BloombergSymbol = iSecMasterBaseObj.BloombergSymbol;
                                currentSecurityDataGridModel.Asset = iSecMasterBaseObj.AssetCategory.ToString();
                                currentSecurityDataGridModel.RoundLot = iSecMasterBaseObj.RoundLot;
                                currentSecurityDataGridModel.BloombergSymbolWithExchangeCode = iSecMasterBaseObj.BloombergSymbolWithExchangeCode;

                                //Assuming base currency USD

                                currentSecurityDataGridModel.Sector = iSecMasterBaseObj.Sector;
                                currentSecurityDataGridModel.Multiplier = (decimal)iSecMasterBaseObj.Multiplier;
                                currentSecurityDataGridModel.Delta = (decimal)iSecMasterBaseObj.Delta;
                                //TODO: Harcoded 1 for now
                                currentSecurityDataGridModel.LeveragedFactor = 1;
                                EnableRebalancerSecurityUI(true);
                            }
                            if (String.IsNullOrEmpty(StatusAndErrorMessages.ErrorMessage))
                                AddUpdateStatusAndMessage(string.Empty, string.Empty);
                            else
                                AddUpdateStatusAndMessage(string.Empty, StatusAndErrorMessages.ErrorMessage);
                        }
                    }
                }
                else
                {
                    Dispatcher.CurrentDispatcher.Invoke(() => _secMasterClient_SecMstrDataResponse(sender, e));
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
        /// Handles the OnResponse event of the LOne control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arg">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                if (Dispatcher.CurrentDispatcher.CheckAccess())
                {
                    if (args != null)
                    {
                        SymbolData data = args.Value;
                        if (data.Symbol == TickerSymbol.Trim() || data.Symbol == currentSecurityDataGridModel.Symbol)
                        {
                            onL1Response(data);
                        }
                    }
                }
                else
                {
                    Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                    {
                        LOne_OnResponse(sender, args);
                    }));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ons the l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private void onL1Response(SymbolData l1Data)
        {
            try
            {
                if (_marketDataHelperInstance != null)
                {
                    decimal realTimePrice = 0;
                    string prefValue = RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalPricingFeld, 0);

                    SelectedFeedPrice enumName;
                    if (Enum.TryParse(prefValue, out enumName))
                    {
                        switch (enumName)
                        {
                            case SelectedFeedPrice.Ask:
                                Decimal.TryParse(l1Data.Ask.ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Bid:
                                Decimal.TryParse(l1Data.Bid.ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Last:
                                Decimal.TryParse(l1Data.LastPrice.ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Mid:
                                Decimal.TryParse(l1Data.Mid.ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.iMid:
                                Decimal.TryParse(l1Data.iMid.ToString(), out realTimePrice);
                                break;
                            default:
                                Decimal.TryParse(l1Data.LastPrice.ToString(), out realTimePrice);
                                break;
                        }
                    }
                    if (realTimePrice != 0)
                    {
                        PriceValue = realTimePrice;
                        StringBuilder errorMessage = new StringBuilder();
                        Dictionary<int, decimal> currentAccountFxRateValue = new Dictionary<int, decimal>();
                        currentAccountFxRateValue = ExpnlServiceConnector.GetInstance().GetFxRateForSymbolAndAccounts(l1Data.Symbol, new List<int> { -1 }, l1Data.AUECID, CachedDataManager.GetInstance.GetCurrencyID(l1Data.CurencyCode), ref errorMessage);
                        FxValue = currentAccountFxRateValue.Count > 0 ? currentAccountFxRateValue.FirstOrDefault().Value : 1;
                        RebalancerCache.Instance.AddOrUpdateSymbolWisePriceAndFx(TickerSymbol, realTimePrice, FxValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        /// <summary>
        /// Enable RAS UI
        /// </summary>
        /// <param name="flag"></param>
        internal void EnableRebalancerSecurityUI(bool flag)
        {
            try
            {
                if (flag)
                {
                    IsRASEnable = flag;

                }
                else
                {
                    IsRASEnable = flag;
                    Target = 0;
                    if (RASIncreaseDecreaseOrSetList.Count > 0)
                    {
                        int selectedRASPref;
                        if (Int32.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalRASPrefrence, 0),
                            out selectedRASPref))
                            SelectedRASIncreaseDecreaseOrSet = RASIncreaseDecreaseOrSetList[selectedRASPref];
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

        #region Fetch Rebalance Data

        /// <summary>
        /// Fetch Command Action
        /// </summary>
        /// <returns></returns>
        private void FetchCommandAction()
        {
            try
            {
                //Clear data before fetching.
                ClearRebalData();
                AddUpdateStatusAndMessage(RebalancerConstants.MSG_FETCHINGPOSITIONS, string.Empty);
                List<int> accountIDs = new List<int>();
                if (SelectedAccountOrGroup != null)
                {
                    if (!RebalancerCache.Instance.AccountsOrGroupsList.ContainsKey(0))
                    {
                        RebalancerCache.Instance.AccountsOrGroupsList.Add(0, RebalancerConstants.CONST_ProRata);
                    }
                    switch (SelectedAccountOrGroup.Type)
                    {
                        case RebalancerEnums.AccountTypes.MasterFund:
                            if (CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation().ContainsKey(SelectedAccountOrGroup.Key))
                            {
                                accountIDs = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[SelectedAccountOrGroup.Key].Intersect(CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.ToList().Select(s => s)).ToList();
                            }
                            break;
                        case RebalancerEnums.AccountTypes.Account:
                            accountIDs.Add(SelectedAccountOrGroup.Key);
                            break;
                        case RebalancerEnums.AccountTypes.CustomGroup:
                            accountIDs.AddRange(RebalancerCache.Instance.GetCustomGroupAssociatedAccounts(SelectedAccountOrGroup.Key));
                            break;
                        default:
                            break;
                    }
                    foreach (var id in accountIDs)
                    {
                        RebalancerCache.Instance.AccountsOrGroupsList.Add(id, CachedDataManager.GetInstance.GetAccount(id).Trim());
                    }
                }
                bool isRealTimePos = false;
                if (SelectedPositionType.Key.Equals((int)RebalancerEnums.RebalancerPositionsType.RealTimePositions))
                {
                    isRealTimePos = true;
                }
                if (PortfolioData.Count > 0)
                    PortfolioData.Clear();
                List<object> arguments = new List<object>();
                arguments.Add(accountIDs);
                AccountIDs = accountIDs;
                arguments.Add(isRealTimePos);
                if (!_bgFetchRebalancerData.IsBusy)
                    _bgFetchRebalancerData.RunWorkerAsync(arguments);
                SetDockManagerProps(false);
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
        /// FetchRebalancerDatab DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bgFetchRebalancerData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object> arguments = e.Argument as List<object>;
                if (arguments != null && arguments.Count == 2)
                {
                    List<int> accountIDs = arguments[0] as List<int>;
                    bool isRealTimePos = (bool)arguments[1];
                    if (accountIDs != null)
                    {
                        StringBuilder errorMessage = new StringBuilder();
                        RebalancerData rebalancerData = new RebalancerData();
                        if (isRealTimePos)
                        {
                            rebalancerData = ExpnlServiceConnector.GetInstance().GetRebalancerData(accountIDs, RebalancerEnums.RebalancerPositionsType.RealTimePositions, ref errorMessage);
                        }
                        else
                        {
                            rebalancerData = ExpnlServiceConnector.GetInstance().GetRebalancerData(accountIDs, RebalancerEnums.RebalancerPositionsType.PreviousDaysPositons, ref errorMessage);
                        }

                        var SymbolList = rebalancerData.RebalancerDtos.Select(x => x.Symbol).ToList();
                        Dictionary<string, SecMasterBaseObj> NewSymbolObjList = SecMasterSyncServicesConnector.GetInstance().GetSecMasterSymbolData(SymbolList, ApplicationConstants.SymbologyCodes.TickerSymbol);
                        foreach (RebalancerDto rebalancerDto in rebalancerData.RebalancerDtos)
                        {
                            if (NewSymbolObjList != null && NewSymbolObjList.ContainsKey(rebalancerDto.Symbol))
                            {
                                rebalancerDto.RoundLot = NewSymbolObjList[rebalancerDto.Symbol].RoundLot;
                            }
                        }
                        symbolWiseYesterDayPrice = rebalancerData.SymbolWiseYesterDayPrice;

                        AddUpdateStatusAndMessage(RebalancerConstants.MSG_FETCHINGPOSITIONS, errorMessage.ToString());
                        //first create account wise AdjustedAccountLevelNAV
                        List<AdjustedAccountLevelNAV> lstAdjustedAccountLevelNAV = RebalancerMapper.Instance.MapAccountLevelNAVDtosToAdjustedAccountLevelNAVDtos(rebalancerData.AccountWiseNAV);
                        ObservableCollection<AdjustedAccountLevelNAV> nAVGridData = new ObservableCollection<AdjustedAccountLevelNAV>(lstAdjustedAccountLevelNAV);

                        foreach (var navData in nAVGridData)
                        {
                            string navPreference =
                                RebalancerCache.Instance.GetRebalPreference(
                                    RebalancerConstants.CONST_OtherItemsImpactingNAV, navData.AccountId);
                            NavPreferencesModel navPreferencesModel =
                                JsonConvert.DeserializeObject<NavPreferencesModel>(navPreference);

                            if (IsCheckComplianceAllowed)
                            {
                                navData.IsIncludeCashInBaseCurrency = true;
                                navData.IsIncludeAccrualsInBaseCurrency = true;
                                navData.IsIncludeOtherAssetsNAV = true;
                                navData.IsIncludeSwapNavAdjustement = true;
                                navData.IsIncludeUnrealizedPNLOfSwaps = true;
                            }
                            else
                            {
                                navData.IsIncludeCashInBaseCurrency = navPreferencesModel.IsIncludeCash;
                                navData.IsIncludeAccrualsInBaseCurrency = navPreferencesModel.IsIncludeAccruals;
                                navData.IsIncludeOtherAssetsNAV = navPreferencesModel.IsIncludeOtherAssetsMarketValue;
                                navData.IsIncludeSwapNavAdjustement = navPreferencesModel.IsIncludeSwapNavAdjustment;
                                navData.IsIncludeUnrealizedPNLOfSwaps = navPreferencesModel.IsIncludeUnrealizedPnlOfSwaps;
                            }
                        }

                        //Set AdjustedAccountLevelNAV in each RebalancerModel based on fund id.
                        ObservableCollection<RebalancerModel> portfolioData = new ObservableCollection<RebalancerModel>(RebalancerMapper.Instance.MapRebalancerDtosToRebalancerModel(rebalancerData.RebalancerDtos, lstAdjustedAccountLevelNAV));
                        RebalancerCache.Instance.SetSymbolWisePriceAndFx(rebalancerData.SymbolWisePriceAndFx);

                        List<object> resultArguments = new List<object>();
                        resultArguments.Add(portfolioData);
                        resultArguments.Add(nAVGridData);
                        e.Result = resultArguments;
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
        /// FetchRebalancerData RunWorkerCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bgFetchRebalancerData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // check error, check cancel, then use result
                if (e.Error != null)
                {
                    AddUpdateStatusAndMessage(string.Empty, e.Error.Message.ToString());
                }
                else if (e.Cancelled)
                {
                    // handle cancellation
                }

                List<object> arguments = e.Result as List<object>;
                if (arguments != null && arguments.Count == 2)
                {
                    ObservableCollection<RebalancerModel> portfolioData = arguments[0] as ObservableCollection<RebalancerModel>;
                    ObservableCollection<AdjustedAccountLevelNAV> nAVGridData = arguments[1] as ObservableCollection<AdjustedAccountLevelNAV>;
                    if (portfolioData != null && nAVGridData != null)
                    {
                        NAVGridData = nAVGridData;
                        PortfolioData = portfolioData;
                    }
                }
                List<KeyValueItem> AccountOrGroupLstinRASList = new List<KeyValueItem>() {
                    new KeyValueItem(){Key = 0, ItemValue = RebalancerConstants.CONST_ProRata}
                };
                foreach (AdjustedAccountLevelNAV obj in NAVGridData)
                {
                    KeyValueItem kv = new KeyValueItem()
                    {
                        Key = obj.AccountId,
                        ItemValue = obj.AccountName
                    };
                    AccountOrGroupLstinRASList.Add(kv);
                }
                if (AccountOrGroupLstinRASList.Count == 2 && AccountOrGroupLstinRASList.FirstOrDefault(p => p.Key == 0) != null)
                {
                    AccountOrGroupLstinRASList.RemoveAt(0);
                    RebalancerCache.Instance.AccountsOrGroupsList.Remove(0);
                }
                AccountOrGroupLstinRAS = new ObservableCollection<KeyValueItem>(AccountOrGroupLstinRASList);
                if (AccountOrGroupLstinRAS.Count > 0)
                    SelectedAccountOrGroupInRAS = AccountOrGroupLstinRAS[0];
                if (PortfolioData.Count == 0)
                    AddUpdateStatusAndMessage(RebalancerConstants.MSG_NOPOSITIONSTOSHOW, string.Empty);
                else
                    AddUpdateStatusAndMessage(string.Empty, string.Empty);
                IsModifyRebalanceAllowed = false;
                IsFetchPressed = true;
            }
            catch (Exception ex)
            {
                IsFetchPressed = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Run Rebalance
        /// <summary>
        /// Run Rebalance Action
        /// </summary>
        /// <returns></returns>
        private void RunRebalanceAction()
        {
            try
            {
                bool result = RecalculateOrValidateCommandAction(true);
                if (result)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(CashFlow);
                    arguments.Add(new List<RebalancerModel>(PortfolioData));
                    arguments.Add(AccountWiseDictWithUserModifiedModels);
                    arguments.Add(SelectedModelPortfolio);
                    if (!_bgRunRebalance.IsBusy)
                        _bgRunRebalance.RunWorkerAsync(arguments);

                    RebalancerCommon.Instance.cashFlow.Clear();
                    foreach (var adjustedNav in NAVGridData)
                    {
                        CashFlowToCompliance cashFlowToCompliance = new CashFlowToCompliance();
                        cashFlowToCompliance.AccountId = adjustedNav.AccountId;
                        cashFlowToCompliance.Cash = RebalancerCache.Instance.GetCashFlowImpact() == RebalancerEnums.CashFlowImpactOnNAV.ImpactNAV ? adjustedNav.CashFlow : 0;
                        RebalancerCommon.Instance.cashFlow.Add(cashFlowToCompliance);
                    }
                    RebalancerCommon.Instance.SetIsRealTimePositionsPreference(SelectedPositionType.Key == 1 ? true : false);
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

        private void _bgRunRebalance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AddUpdateStatusAndMessage(RebalancerConstants.MSG_CalculatingData, string.Empty);
                List<object> arguments = e.Argument as List<object>;
                if (arguments != null && arguments.Count == 4)
                {
                    StringBuilder message = new StringBuilder();
                    decimal cashFlow = arguments[0] != null ? (decimal)arguments[0] : 0;
                    List<RebalancerModel> portfolioData = arguments[1] as List<RebalancerModel>;
                    Dictionary<int, Dictionary<string, SecurityDataGridModel>> securityDataGridDict = arguments[2] as Dictionary<int, Dictionary<string, SecurityDataGridModel>>;
                    KeyValueItem selectedModelPortfolio = arguments[3] as KeyValueItem;
                    if (portfolioData != null)
                    {
                        CalculationModel calculationModel = new CalculationModel()
                        {
                            RebalancerModels = portfolioData,
                            CashFlow = cashFlow,
                            ModelPortfolioId = selectedModelPortfolio != null ? selectedModelPortfolio.Key : 0,
                            AccountWiseSecurityDataGridDict = securityDataGridDict,
                            RebalPositionType = (RebalancerEnums.RebalancerPositionsType)SelectedPositionType.Key,
                            RoundingTypes = (RebalancerEnums.RoundingTypes)SelectedRoundingType.Key
                        };
                        bool result = RebalCalculatorInstance.CalculateData(calculationModel, NAVGridData.ToList(), ref message);
                        List<object> argumentsResult = new List<object>();
                        argumentsResult.Add(result);
                        argumentsResult.Add(calculationModel.RebalancerModels);
                        argumentsResult.Add(message);
                        argumentsResult.Add(calculationModel.ModelPortfolioId);
                        e.Result = argumentsResult;
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

        private void _bgRunRebalance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    List<object> arguments = e.Result as List<object>;
                    if (arguments != null && arguments.Count == 4)
                    {
                        bool result = (bool)arguments[0];
                        List<RebalancerModel> resultPortfolioData = arguments[1] as List<RebalancerModel>;
                        StringBuilder message = arguments[2] as StringBuilder;
                        int selectedModelPortfolio = (int)arguments[3];
                        ModelPortfolioDto modelPortfolioDto = RebalancerCache.Instance.GetModelPortfolio(selectedModelPortfolio);
                        if (result && resultPortfolioData != null)
                        {
                            PortfolioData = new ObservableCollection<RebalancerModel>(resultPortfolioData);
                            IsModifyRebalanceAllowed = result;
                            OnPropertyChanged("IsModelSelected");
                            errorMsg = new StringBuilder();
                            List<TradeListModel> tradeList = GetTradeList().ToList();
                            listOfOrders = RebalancerCommon.Instance.GroupStagedOrders(tradeList, ref errorMsg, LoginUser.CompanyUserID, RebalancerServiceApi.GetInstance().GetSmartName());
                            // Display Popup when cash is going negative when rebalancing using tolerance
                            if (modelPortfolioDto != null && modelPortfolioDto.UseTolerance == RebalancerEnums.UseTolerance.Yes)
                            {
                                decimal cash = PortfolioData.Where(item => item.Asset == "Cash").Sum(item => item.TargetMarketValueBase);
                                if (cash < 0)
                                {
                                    string msg = $"You need additional ${Math.Abs(Math.Round(cash)).ToString("N0")} in cash to rebalance. Do you want to allow negative cash?";
                                    MessageBoxResult rsltMessageBox = MessageBox.Show(msg, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                    if (rsltMessageBox != MessageBoxResult.Yes)
                                    {
                                        ClearCalculation();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(message.ToString()))
                                MessageBox.Show(message.ToString(), RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                            else
                                MessageBox.Show("Nothing to Rebalance.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    AddUpdateStatusAndMessage(errorMsg.ToString(), string.Empty);
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
        #endregion

        #region modify Rebalance
        private void ModifyRebalanceAction()
        {
            try
            {
                bool result = RecalculateOrValidateCommandAction(true);
                if (result)
                {
                    List<object> arguments = new List<object>();
                    //Cashflow wont be considered in the modify rebalance operation so setting it to null.
                    arguments.Add(null);
                    arguments.Add(new List<RebalancerModel>(PortfolioData));
                    arguments.Add(AccountWiseDictWithUserModifiedModels);
                    arguments.Add(null);
                    if (!_bgRunRebalance.IsBusy)
                        _bgRunRebalance.RunWorkerAsync(arguments);
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
        #endregion

        private bool ValidateCommandAction(ref StringBuilder errorMessage)
        {
            string dictionaryJson = JsonConvert.SerializeObject(AccountWiseDict);
            AccountWiseDictWithUserModifiedModels =
                JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, SecurityDataGridModel>>>(dictionaryJson);
            decimal cashFlow = CashFlow == null ? 0 : Convert.ToDecimal(CashFlow);
            //Set current selection of trading rules in cache.
            RebalancerCache.Instance.SetTradingRules(new TradingRules(TradingRulesInstance, false));
            ModelPortfolioDto modelPortfolioDto = RebalancerCache.Instance.GetModelPortfolio(SelectedModelPortfolio.Key);
            bool returnResult = RebalViewModelExtension.Instance.ValidateRebalancerData(_portfolioData.ToList(), _navGridData.ToList(), ref AccountWiseDictWithUserModifiedModels, cashFlow, IsModifyRebalanceAllowed, ref errorMessage, modelPortfolioDto);
            return returnResult;
        }

        private bool RecalculateCommandAction()
        {
            List<RebalancerModel> rebalModels = RebalViewModelExtension.Instance.RecalculateRebalancerData(_portfolioData.ToList(), _navGridData.ToList());
            PortfolioData = new ObservableCollection<RebalancerModel>(rebalModels);
            foreach (RebalancerModel rebalancerModel in PortfolioData)
            {
                rebalancerModel.RaisePropertyChanged("CurrentPercentage");
                rebalancerModel.RaisePropertyChanged("TargetPercentage");
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isRunRebalance"></param>
        /// <returns>
        /// if true is returned then rebal calculation will happen.
        /// if false is returned then rebal calculation will not happen.
        /// </returns>
        private bool RecalculateOrValidateCommandAction(bool isRunRebalance = false)
        {
            try
            {
                StringBuilder errorMessage = new StringBuilder();

                if (!ValidateCashTargetRule()) return false;

                if (!IsModifyRebalanceAllowed)
                {
                    if (PortfolioData.Any(x => x.IsCalculatedModel))
                    {
                        MessageBoxResult rsltMessageBox = MessageBox.Show("Modified rebal positions will reset, do you want to continue?", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                        if (rsltMessageBox != MessageBoxResult.Yes)
                            return false;
                    }
                }
                bool returnResult = false;
                if (isRunRebalance)
                {
                    if (!IsModifyRebalanceAllowed)
                    {
                        RecalculateCommandAction();
                    }
                    returnResult = ValidateCommandAction(ref errorMessage);
                }
                else
                    RecalculateCommandAction();
                if (!returnResult)
                {
                    if (errorMessage.Length > 0)
                    {
                        ShowErrorAlert(errorMessage.ToString());
                    }
                    return false;
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
            return true;
        }

        /// <summary>
        /// Validate Set Cash Target Trading Rule
        /// </summary>
        private bool ValidateCashTargetRule()
        {
            try
            {
                if (tradingRulesInstance.IsSetCashTarget)
                {
                    if (IsModelSelected)
                    {
                        ShowErrorAlert(RebalancerConstants.MSG_CASH_TARGET_PERCENT_AND_MODEL_VALIDATION);
                        return false;
                    }
                    if (tradingRulesInstance.CashTarget == null)
                    {
                        IsShowCashBlankValidation = true;
                        ShowErrorAlert(RebalancerConstants.MSG_CASH_TARGET_PERCENT_VALUE_VALIDATION);
                        return false;
                    }
                    //Show alert if cash target % + Rebalance across securities target % is greater than 100
                    decimal cash = (decimal)tradingRulesInstance.CashTarget;
                    foreach (var kvp in AccountWiseDict)
                    {
                        decimal totalAccountTargetPercent = kvp.Value.Sum(p => p.Value.MultiplierFactor * p.Value.TargetPercentage);
                        if (Math.Abs(totalAccountTargetPercent + cash) > 100)
                        {
                            ShowErrorAlert(RebalancerConstants.MSG_CASH_TARGET_PERCENT_AND_RAS_VALIDATION);
                            return false;
                        }
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
            return true;
        }

        /// <summary>
        /// Bind the acocunts and custom groups in drop down list
        /// </summary>
        private void BindAccountsAndCustomGroupsFromCache()
        {
            AccountGroupDropdownPrefModel accountGroupDropdownPref = JsonConvert.DeserializeObject<AccountGroupDropdownPrefModel>
                (RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalAccountGroupVisibilityPref, 0));
            ObservableCollection<AccountsModel> accountsAndGroupsList = new ObservableCollection<AccountsModel>();

            //Code to Bind Account/Groups dropdown preferences.
            if (accountGroupDropdownPref.IsAccountIncluded)
            {
                if (accountGroupDropdownPref.IsMasterFundIncluded)
                {
                    Dictionary<int, List<int>> dictMasterFundAndAccountAccociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                    BindAccountGroupWithAccounts(accountsAndGroupsList, dictMasterFundAndAccountAccociation, RebalancerEnums.AccountTypes.MasterFund, true);
                }
                if (accountGroupDropdownPref.IsCustomGroupIncluded)
                {
                    ConcurrentDictionary<int, List<int>> customGroups = RebalancerCache.Instance.GetCustomGroupAndAccountsAssociation();
                    BindAccountGroupWithAccounts(accountsAndGroupsList, customGroups, RebalancerEnums.AccountTypes.CustomGroup, true);
                }
                if (!accountGroupDropdownPref.IsMasterFundIncluded && !accountGroupDropdownPref.IsCustomGroupIncluded)
                {
                    accountsAndGroupsList = new ObservableCollection<AccountsModel>(CachedDataManager.GetInstance.GetUserAccountsAsDict().OrderBy(p => p.Value)
                    .Select(p => new AccountsModel { ItemValue = p.Value, Key = p.Key, IsSpaceRequired = false, Type = RebalancerEnums.AccountTypes.Account }).ToList());
                }
            }
            else
            {
                if (accountGroupDropdownPref.IsMasterFundIncluded)
                {
                    Dictionary<int, List<int>> dictMasterFundAndAccountAccociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                    BindAccountGroupWithAccounts(accountsAndGroupsList, dictMasterFundAndAccountAccociation, RebalancerEnums.AccountTypes.MasterFund, false);
                }
                if (accountGroupDropdownPref.IsCustomGroupIncluded)
                {
                    ConcurrentDictionary<int, List<int>> customGroups = RebalancerCache.Instance.GetCustomGroupAndAccountsAssociation();
                    BindAccountGroupWithAccounts(accountsAndGroupsList, customGroups, RebalancerEnums.AccountTypes.CustomGroup, false);
                }

            }
            AccountsAndGroupsList = accountsAndGroupsList;

            if (_accountsAndGroupsList.Count > 0)
            {

                if (SelectedAccountOrGroup == null)
                    SelectedAccountOrGroup = _accountsAndGroupsList[0];
                else
                {
                    SelectedAccountOrGroup = _accountsAndGroupsList.FirstOrDefault(p => p.Key == SelectedAccountOrGroup.Key);
                    if (SelectedAccountOrGroup == null)
                    {
                        if (RebalancerCache.Instance.SelectedTab.Equals(RebalancerConstants.CONST_RebalancerTab))
                            ShowInformationAlert("Selected Group for Rebalancer no longer available");
                        SelectedAccountOrGroup = _accountsAndGroupsList[0];
                        ClearRebalancerData();
                    }
                }
                _oldSelectedAccountOrGroup = SelectedAccountOrGroup;
            }
        }

        private void BindAccountGroupWithAccounts(ObservableCollection<AccountsModel> accountsAndGroupsList, ICollection<KeyValuePair<int, List<int>>> accountGroupAndAccountsAssociation, RebalancerEnums.AccountTypes accountType, bool isSubItemRequired)
        {
            Dictionary<int, string> UserAccountsAsDict = CachedDataManager.GetInstance.GetUserAccountsAsDict();
            SortedDictionary<string, AccountsModel> dictAccounts = new SortedDictionary<string, AccountsModel>();
            Dictionary<string, ObservableCollection<AccountsModel>> dictSubAccounts = new Dictionary<string, ObservableCollection<AccountsModel>>();
            foreach (var accountGroup in accountGroupAndAccountsAssociation)
            {
                if (accountType == RebalancerEnums.AccountTypes.MasterFund && !CachedDataManager.GetInstance.GetUserMasterFunds().ContainsKey(accountGroup.Key))
                    continue;
                AccountsModel itemToAdd = new AccountsModel()
                {
                    Key = accountGroup.Key,
                    ItemValue = accountType == RebalancerEnums.AccountTypes.MasterFund
                    ? CachedDataManager.GetInstance.GetMasterFund(accountGroup.Key)
                    : RebalancerCache.Instance.GetCustomGroupName(accountGroup.Key),
                    Type = accountType
                };
                //accountsAndGroupsList.Add(itemToAdd);
                dictAccounts.Add(itemToAdd.ItemValue, itemToAdd);
                if (isSubItemRequired)
                {
                    SortedDictionary<string, AccountsModel> dictSortedSubAccounts = new SortedDictionary<string, AccountsModel>();
                    foreach (int accountId in accountGroup.Value)
                    {
                        if (!UserAccountsAsDict.ContainsKey(accountId))
                            continue;
                        AccountsModel subItemToAdd = new AccountsModel()
                        {
                            Key = accountId,
                            ItemValue = CachedDataManager.GetInstance.GetAccount(accountId),
                            Type = RebalancerEnums.AccountTypes.Account
                        };
                        dictSortedSubAccounts.Add(CachedDataManager.GetInstance.GetAccount(accountId), subItemToAdd);
                    }
                    ObservableCollection<AccountsModel> subAccountsAndGroupsList = new ObservableCollection<AccountsModel>(dictSortedSubAccounts.Values);
                    //accountsAndGroupsList.AddRange(subAccountsAndGroupsList);
                    dictSubAccounts.Add(itemToAdd.ItemValue, subAccountsAndGroupsList);
                }
            }

            foreach (KeyValuePair<string, AccountsModel> kvp in dictAccounts)
            {
                accountsAndGroupsList.Add(kvp.Value);
                if (dictSubAccounts.ContainsKey(kvp.Key))
                    accountsAndGroupsList.AddRange(dictSubAccounts[kvp.Key]);
            }
        }

        private void SetLockUnlockPosition(object rebalancerRowObj)
        {
            try
            {
                if (rebalancerRowObj != null)
                {
                    RebalancerModel rebalancerGridModel = rebalancerRowObj as RebalancerModel;
                    if (rebalancerGridModel != null)
                    {
                        if (rebalancerGridModel.IsLock)
                            rebalancerGridModel.IsLock = false;
                        else
                            rebalancerGridModel.IsLock = true;
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

        private void _timerSnapShot_Tick(object sender, EventArgs e)
        {
            try
            {
                _timerSnapShot.Stop();
                if (IsSymbolValidated)
                    IsSymbolValidated = false;
                else
                {
                    AddUpdateStatusAndMessage(string.Empty, string.Empty);
                    MessageBox.Show("Symbol Not Found.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Error);

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

        private void RebalacerDataGrid_PreviewKeyDownAction(object keyEventArgs)
        {
            try
            {
                if (keyEventArgs != null)
                {
                    KeyEventArgs arg = keyEventArgs as KeyEventArgs;
                    if (arg.Source != null && arg.Key == System.Windows.Input.Key.F1)
                    {
                        XamDataGrid rebalancerGrid = arg.Source as XamDataGrid;

                        if (rebalancerGrid != null && rebalancerGrid.SelectedDataItem != null && (PortfolioData.Any(x => x.IsCalculatedModel) || SelectedModelPortfolio == null || SelectedModelPortfolio.ItemValue.Equals("None")))
                        {
                            ((RebalancerModel)rebalancerGrid.SelectedDataItem).IsLock = !((RebalancerModel)rebalancerGrid.SelectedDataItem).IsLock;
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
                    if (!PortfolioData.Any(x => x.IsCalculatedModel))
                    {
                        ShowErrorAlert("Run Rebalance not performed, first Run Rebalance.");
                        return;
                    }

                    EnableButtonsAndGrid(false);
                    InProgress = true;
                    AddUpdateStatusAndMessage(String.Format("Sending {0} orders for compliance validation.", listOfOrders.Count), string.Empty);
                    if (listOfOrders.Count > 0 && string.IsNullOrEmpty(errorMsg.ToString()))
                    {
                        SendOrdersForComplianceCheck(listOfOrders, errorMsg);
                    }
                    else
                    {
                        InProgressMessage = RebalancerConstants.MSG_NOTHING_TO_CHECK;
                        DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerConstants.MSG_NOTHING_TO_CHECK,
                                   RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                        AddUpdateStatusAndMessage(String.Format("Compliance validated successfully for {0} orders.", listOfOrders.Count), string.Empty);
                    }
                    InProgress = false;
                }).ContinueWith(o =>
                {
                    EnableButtonsAndGrid(true);
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                    int mfID = RebalancerCommon.Instance.GetMasterFundIDfromAccounts(_selectedAccountOrGroup.Type, AccountIDs);
                    int tradingAccountID = CachedDataManager.GetTradingAccountForMasterFund(mfID);
                    string masterFundName = CachedDataManager.GetInstance.GetMasterFund(mfID);
                    if (mfID > 0)
                    {
                        orderList.ForEach((OrderSingle order) =>
                        {
                            order.MasterFund = masterFundName;
                            if (tradingAccountID > 0)
                                order.TradingAccountID = tradingAccountID;
                        });
                    }

                    bool isRealTimePositions = RebalancerCommon.Instance.GetIsRealTimePositionsPreference();
                    ComplianceCommon.SendCashAmountForAccountsFromRebalancer(RebalancerCommon.Instance.cashFlow);
                    Logger.LoggerWrite(string.Format("Sending {0} orders for compliance validation.", orderList.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    SimulationResult result = ComplianceServiceConnector.GetInstance().SimulateTrade(orderList, PreTradeType.ComplianceCheck, _loginUser.CompanyUserID, isRealTimePositions, true);
                    var complianceCheckResult = ComplianceCommon.CheckComplianceForRebalancer(orderList, result);
                    // var complianceCheckResult = ComplianceCommon.CheckCompliance(orderList, _loginUser.CompanyUserID);
                    if (complianceCheckResult == null)
                    {
                        AddUpdateStatusAndMessage(string.Format(RebalancerConstants.MSG_COMPLIANCE_NOT_VALIDATED, orderList.Count), string.Empty);
                        AddUpdateStatusAndMessage(preferenceErrorMessage.ToString(), string.Empty);
                    }
                    else if (complianceCheckResult == false)
                    {
                        DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerConstants.MSG_NO_RULES_VIOLATED,
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, true);
                        AddUpdateStatusAndMessage(RebalancerConstants.MSG_NO_RULES_VIOLATED, string.Empty);
                    }
                    else if (_complianceAlertsViewModel != null)
                    {
                        _complianceAlertsViewModel.BindAlerts(result.Alerts);
                        SetDockManagerProps(true);
                        string statusBarMsg = result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID)
                            ? result.Alerts[0].RuleName
                            : string.Format(RebalancerConstants.MSG_COMPLIANCE_VALIDATED, orderList.Count);

                        AddUpdateStatusAndMessage(statusBarMsg, string.Empty);
                        Logger.LoggerWrite(string.Format(RebalancerConstants.MSG_COMPLIANCE_VALIDATED, orderList.Count), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    }
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
        /// Send
        /// </summary>
        /// <param name="showDialog"></param>
        /// <param name="dialogViewModel"></param>
        /// <param name="uiSyncContext"></param>
        /// <returns></returns>
        public bool? Send(Func<BlockedAlertsViewModel, bool?> showDialog, BlockedAlertsViewModel dialogViewModel, SynchronizationContext uiSyncContext)
        {
            bool? dialogResult = false;
            try
            {
                uiSyncContext.Send(new SendOrPostCallback((x) =>
                {
                    dialogResult = showDialog(dialogViewModel);
                    if (dialogResult == null)
                        dialogResult = false;
                }), null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dialogResult;
        }

        /// <summary>
        /// EnableButtonsAndGrid
        /// </summary>
        /// <param name="isEnabled"></param>
        private void EnableButtonsAndGrid(bool isEnabled)
        {
            try
            {
                EnableButtons = isEnabled;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SetDockManagerProps
        /// </summary>
        /// <param name="propValue"></param>
        private void SetDockManagerProps(bool propValue)
        {
            EnableAlertsTab = propValue;
            ComplianceTabIsPinned = propValue;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                InstanceManager.ReleaseInstance(typeof(RebalancerViewModel));
                if (disposing)
                {
                    if (_bgFetchRebalancerData != null)
                    {
                        _bgFetchRebalancerData.DoWork -= _bgFetchRebalancerData_DoWork;
                        _bgFetchRebalancerData.RunWorkerCompleted -= _bgFetchRebalancerData_RunWorkerCompleted;
                        _bgFetchRebalancerData.Dispose();
                    }
                    if (_bgRunRebalance != null)
                    {
                        _bgRunRebalance.DoWork -= _bgRunRebalance_DoWork;
                        _bgRunRebalance.RunWorkerCompleted -= _bgRunRebalance_RunWorkerCompleted;
                        _bgRunRebalance.Dispose();
                    }
                    if (SecurityMaster != null)
                    {
                        SecurityMaster.SecMstrDataResponse -= _secMasterClient_SecMstrDataResponse;
                        SecurityMaster = null;
                    }
                    if (_marketDataHelperInstance != null)
                    {
                        _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                        _marketDataHelperInstance.Dispose();
                        _marketDataHelperInstance = null;
                    }
                    if (_timerSnapShot != null)
                    {
                        _timerSnapShot.Tick -= new EventHandler(_timerSnapShot_Tick);
                    }
                    RebalancerCache.Instance.UpdateCustomGroups -= RebalancerCache_UpdateCustomGroups;
                    RebalancerCache.Instance.UpdateModelPortfolios -= RebalancerCache_UpdateModelPortfolios;
                    RebalancerCache.Instance.UpdateDataPreferencsOnRebalancerView -= RebalanceCache_UpdateDataPreferencsOnRebalancerView;
                    RebalancerHelperInstance = null;

                    _modelPortfolioList = null;
                    if (_tradeListViewInstance != null)
                    {
                        _tradeListViewInstance.Closing -= TradeListViewInstance_Closing;
                    }
                    if (_tradeListViewInstance != null) _tradeListViewInstance.Close();
                    if (_customCashFlowWindow != null) _customCashFlowWindow.Close();
                    if (_rasImportViewModelInstance != null)
                        _rasImportViewModelInstance.UpdatedListFromImport -= btnImportContinue_Click;
                    _customCashFlowWindow = null;
                    _tradeListViewInstance = null;

                    if (_navGridData != null)
                        _navGridData = null;

                    if (_portfolioData != null)
                        _portfolioData = null;

                    if (_positionTypeList != null)
                        _positionTypeList = null;

                    if (_rasIncreaseDecreaseOrSetList != null)
                        _rasIncreaseDecreaseOrSetList = null;

                    if (_refreshTypeList != null)
                        _refreshTypeList = null;
                    AccountWiseSecurityDataGridModel.Instance.ClearData(AccountWiseDict);
                    RebalancerCache.Instance.AccountsOrGroupsList.Clear();
                    if (_securityDataGridList != null)
                        _securityDataGridList = null;
                    _selectedAccountOrGroup = null;
                    _selectedCashAction = null;
                    _selectedModelPortfolio = null;
                    _selectedPositionType = null;
                    _selectedRASIncreaseDecreaseOrSet = null;
                    _selectedRefreshType = null;
                    _selectedCalculationLevel = null;
                    _oldSelectedAccountOrGroup = null;
                    _oldSelectedCalculationLevel = null;
                    if (_tradeListViewModelInstance != null)
                    {
                        _tradeListViewModelInstance.Dispose();
                        _tradeListViewModelInstance = null;
                    }
                    if (_complianceAlertsViewModel != null)
                    {
                        _complianceAlertsViewModel.Dispose();
                        _complianceAlertsViewModel = null;
                    }
                    if (_accountsAndGroupsList != null)
                        _accountsAndGroupsList = null;

                    if (_calculationLevelList != null)
                        _calculationLevelList = null;


                    if (_accountOrGroupLstinRAS != null)
                        _accountOrGroupLstinRAS = null;

                    if (_actionsOnCashList != null)
                        _actionsOnCashList = null;

                    if (_cashFlowImpactList != null)
                        _cashFlowImpactList = null;

                    if (_roundingTypeList != null)
                        _roundingTypeList = null;

                    RebalancerCommon.Instance.Dispose();
                    RebalancerServiceApi.GetInstance().Dispose();
                    RebalancerMapper.Instance.Dispose();

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
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                if (WindowName == "RebalancerWindow")
                {
                    ExportFilePathForAutomation = filePath;
                    if (gridName == "RebalacerDataGrid")
                    {
                        if (ExportRebalancerGridForAutomation == true)
                            ExportRebalancerGridForAutomation = false;
                        ExportRebalancerGridForAutomation = true;
                    }
                    else if (gridName == "NAVGrid")
                    {
                        if (ExportNavGridForAutomation == true)
                            ExportNavGridForAutomation = false;
                        ExportNavGridForAutomation = true;
                    }
                    else if (gridName == "SecurityDataGrid")
                    {
                        if (ExportSecurityDataGridForAutomation == true)
                            ExportSecurityDataGridForAutomation = false;
                        ExportSecurityDataGridForAutomation = true;
                    }
                }
                else if (WindowName == "RASImportWindow")
                {
                    _rasImportViewModelInstance.ExportFilePathForAutomation = filePath;
                    if (gridName == "InvalidSecurities")
                    {
                        if (_rasImportViewModelInstance.ExportInvalidSecuritiesGridForAutomation == true)
                            _rasImportViewModelInstance.ExportInvalidSecuritiesGridForAutomation = false;
                        _rasImportViewModelInstance.ExportInvalidSecuritiesGridForAutomation = true;
                    }
                    else if (gridName == "ValidSecurities")
                    {
                        if (_rasImportViewModelInstance.ExportValidSecuritiesGridForAutomation == true)
                            _rasImportViewModelInstance.ExportValidSecuritiesGridForAutomation = false;
                        _rasImportViewModelInstance.ExportValidSecuritiesGridForAutomation = true;
                    }
                }
                else if (WindowName == "TradeBuySellListView")
                {
                    if (_tradeListViewModelInstance.ExportTradeListGridForAutomation == true)
                        _tradeListViewModelInstance.ExportTradeListGridForAutomation = false;
                    _tradeListViewModelInstance.ExportFilePathForAutomation = filePath;
                    _tradeListViewModelInstance.ExportTradeListGridForAutomation = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
