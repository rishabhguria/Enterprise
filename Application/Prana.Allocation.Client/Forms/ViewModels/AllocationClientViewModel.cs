using ExportGridsData;
using Infragistics.Controls.Editors;
using Prana.Allocation.Client.CacheStore;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Allocation.ViewModels;
using Prana.Allocation.Client.Controls.Allocation.Views;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.Forms.Views;
using Prana.Allocation.Client.Helper;
using Prana.Allocation.Common.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Prana.Allocation.Client.Forms.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    /// <seealso cref="System.IDisposable" />
    public class AllocationClientViewModel : ViewModelBase, IDisposable, IExportGridData
    {
        #region Events

        /// <summary>
        /// Occurs when [allocation data change].
        /// </summary>
        public event EventHandler<EventArgs<bool>> AllocationDataChange;

        /// <summary>
        /// Occurs when [close allocation client].
        /// </summary>
        public event EventHandler CloseAllocationClient;

        /// <summary>
        /// Occurs when [load audit UI event].
        /// </summary>
        public event EventHandler<EventArgs> LoadAuditUIEvent;

        /// <summary>
        /// Occurs when [load cash transaction UI event].
        /// </summary>
        public event EventHandler<EventArgs> LoadCashTransactionUIEvent;

        /// <summary>
        /// Occurs when [load close trade UI event].
        /// </summary>
        public event EventHandler<EventArgs<AllocationGroup>> LoadCloseTradeUIEvent;

        /// <summary>
        /// Occurs when [load symbol lookup event].
        /// </summary>
        public event EventHandler<EventArgs<string>> LoadSymbolLookupEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _is new OTC Workflow
        /// </summary>
        private bool _isNewOTCWorkflow;

        /// <summary>
        /// when filter apply button clicked
        /// </summary>
        private bool _isApplyFilterClicked;


        /// <summary>
        /// The _is current rb checked
        /// </summary>
        private bool _isCurrentRBChecked;

        /// <summary>
        /// The _is historical rb checked
        /// </summary>
        private bool _isHistoricalRBChecked;

        /// <summary>
        /// The _selected from date
        /// </summary>
        private DateTime _selectedFromDate;

        /// <summary>
        /// The _selected to date
        /// </summary>
        private DateTime _selectedToDate;

        /// <summary>
        /// The _is from date picker enabled
        /// </summary>
        private bool _isFromDatePickerEnabled;

        /// <summary>
        /// The _is to date picker enabled
        /// </summary>
        private bool _isToDatePickerEnabled;

        /// <summary>
        /// The _is allocation dock pane visible
        /// </summary>
        private Visibility _isAllocationDockPaneVisible;

        /// <summary>
        /// The _is Swap dock pane visible
        /// </summary>
        private Visibility _isSwapDockPaneVisible;

        /// <summary>
        /// The _is OTC dock pane visible
        /// </summary>
        private Visibility _isOTCDockPaneVisible;

        /// <summary>
        /// The _is edit trade dock pane visible
        /// </summary>
        private Visibility _isEditTradeDockPaneVisible;

        /// <summary>
        /// The _is save layout
        /// </summary>
        private bool _isSaveLayout;

        /// <summary>
        /// The _is load layout
        /// </summary>
        private bool _isLoadLayout;

        /// <summary>
        /// The _allocation grid control view model
        /// </summary>
        private AllocationGridControlViewModel _allocationGridControlViewModel;

        /// <summary>
        /// The account strategy control view model
        /// </summary>
        private AccountStrategyControlViewModel _accountStrategyControlViewModel;

        /// <summary>
        /// The _allocation filters control view model
        /// </summary>
        private AllocationFiltersControlViewModel _allocationFiltersControlViewModel;

        /// <summary>
        /// The _trade attribute bulk change control view model
        /// </summary>
        private TradeAttributeBulkChangeControlViewModel _tradeAttributeBulkChangeControlViewModel;

        /// <summary>
        /// The _commission fees bulk change control view model
        /// </summary>
        private CommissionFeesBulkChangeControlViewModel _commissionFeesBulkChangeControlViewModel;

        /// <summary>
        /// The _allocation preferences UI view model
        /// </summary>
        private CurrentSymbolStateControlViewModel _currentSymbolStateControlViewModel;

        /// <summary>
        /// The _is save with state enabled
        /// </summary>
        private bool _isSaveWithStateEnabled;

        /// <summary>
        /// The _is save without state enabled
        /// </summary>
        private bool _isSaveWithoutStateEnabled;

        /// <summary>
        /// The _allocation user wise preference
        /// </summary>
        private AllocationPreferences _allocationUserWisePref;

        /// <summary>
        /// The _enable disable UI elements
        /// </summary>
        private bool _enableDisableUIElements;

        /// <summary>
        /// Allocation Operation Preferences
        /// </summary>
        Dictionary<int, string> _allocationOperationPreferences;

        /// <summary>
        /// The allocation master fund preferences
        /// </summary>
        Dictionary<int, string> _allocationMasterFundPreferences;

        /// <summary>
        /// The _commission rule and fee
        /// </summary>
        private CommissionRule _commissionRuleAndFee;

        /// <summary>
        /// The _allocation swap control view model
        /// </summary>
        private AllocationSwapControlViewModel _allocationSwapControlViewModel;

        /// <summary>
        /// The allocation OTC control view model
        /// </summary>
        private AllocationOTCControlViewModel _allocationOTCControlViewModel;


        /// <summary>
        /// The _edit trade control view model
        /// </summary>
        private EditTradeControlViewModel _editTradeControlViewModel;

        /// <summary>
        /// The _status bar text
        /// </summary>
        private string _statusBarText;

        /// <summary>
        /// The _enable disable get data button
        /// </summary>
        private bool _enableDisableGetDataButton;

        /// <summary>
        /// The _enable disable tool bar
        /// </summary>
        private bool _enableDisableToolBar;

        /// <summary>
        /// The _is dirty data
        /// </summary>
        public bool _isDirtyData;

        /// <summary>
        /// The _trade attributes
        /// </summary>
        private ObservableCollection<string>[] _tradeAttributes;

        /// <summary>
        /// The _is edit mode
        /// </summary>
        private bool _isEditMode;

        /// <summary>
        /// The prorata UI
        /// </summary>
        private static CalculateProrataUIViewModel _prorataUI = null;

        /// <summary>
        /// The edit alloc preference UI/
        /// </summary>
        private static EditAllocationPreferencesUIViewModel _editAllocPrefUI = null;

        /// <summary>
        /// The alloc preference UI
        /// </summary>
        private static AllocationPreferencesUIViewModel _allocPrefUI = null;

        /// <summary>
        /// The is allocated grid selected
        /// </summary>
        private bool IsAllocatedGridSelected = true;

        /// <summary>
        /// The _FRM external transaction identifier
        /// </summary>
        private static ExternalTransactionIdViewModel _frmExternalTransaction = null;

        /// <summary>
        /// The _selected taxlot for external transcation update
        /// </summary>
        private TaxLot _selectedTaxlotForExternalTransUpdate = null;

        /// <summary>
        /// The _form height
        /// </summary>
        private int _formHeight;

        /// <summary>
        /// The _form width
        /// </summary>
        private int _formWidth;

        /// <summary>
        /// The is pinned account strategy grid
        /// </summary>
        private bool _isPinnedAccountStrategyGrid;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the Account strategy control view model.
        /// </summary>
        /// <value>
        /// The Account strategy control view model.
        /// </value>
        public AccountStrategyControlViewModel AccountStrategyControlViewModel
        {
            get { return _accountStrategyControlViewModel; }
            set
            {
                _accountStrategyControlViewModel = value;
                RaisePropertyChangedEvent("AccountStrategyControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the allocation filters control view model.
        /// </summary>
        /// <value>
        /// The allocation filters control view model.
        /// </value>
        public AllocationFiltersControlViewModel AllocationFiltersControlViewModel
        {
            get { return _allocationFiltersControlViewModel; }
            set
            {
                _allocationFiltersControlViewModel = value;
                RaisePropertyChangedEvent("AllocationFiltersControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the allocation grid control view model.
        /// </summary>
        /// <value>
        /// The allocation grid control view model.
        /// </value>
        public AllocationGridControlViewModel AllocationGridControlViewModel
        {
            get { return _allocationGridControlViewModel; }
            set
            {
                _allocationGridControlViewModel = value;
                RaisePropertyChangedEvent("AllocationGridControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the allocation swap control view model.
        /// </summary>
        /// <value>
        /// The allocation swap control view model.
        /// </value>
        public AllocationSwapControlViewModel AllocationSwapControlViewModel
        {
            get { return _allocationSwapControlViewModel; }
            set
            {
                _allocationSwapControlViewModel = value;
                RaisePropertyChangedEvent("AllocationSwapControlViewModel");
            }
        }


        public AllocationOTCControlViewModel AllocationOTCControlViewModel
        {
            get { return _allocationOTCControlViewModel; }
            set
            {
                _allocationOTCControlViewModel = value;
                RaisePropertyChangedEvent("AllocationOTCControlViewModel");
            }
        }



        /// <summary>
        /// Gets or sets the allocation preferences UI view model.
        /// </summary>
        /// <value>
        /// The allocation preferences UI view model.
        /// </value>
        public CommissionFeesBulkChangeControlViewModel CommissionFeesBulkChangeControlViewModel
        {
            get { return _commissionFeesBulkChangeControlViewModel; }
            set
            {
                _commissionFeesBulkChangeControlViewModel = value;
                RaisePropertyChangedEvent("CommissionFeesBulkChangeControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the commission rule and fee.
        /// </summary>
        /// <value>
        /// The commission rule and fee.
        /// </value>
        public CommissionRule CommissionRuleAndFee
        {
            get { return _commissionRuleAndFee; }
            set
            {
                _commissionRuleAndFee = value;
                RaisePropertyChangedEvent("CommissionRuleAndFee");
            }
        }

        /// <summary>
        /// The _current symbol state control view model
        /// </summary>
        /// <value>
        /// The current symbol state control view model.
        /// </value>
        public CurrentSymbolStateControlViewModel CurrentSymbolStateControlViewModel
        {
            get { return _currentSymbolStateControlViewModel; }
            set
            {
                _currentSymbolStateControlViewModel = value;
                RaisePropertyChangedEvent("CurrentSymbolStateControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the edit trade control view model.
        /// </summary>
        /// <value>
        /// The edit trade control view model.
        /// </value>
        public EditTradeControlViewModel EditTradeControlViewModel
        {
            get { return _editTradeControlViewModel; }
            set
            {
                _editTradeControlViewModel = value;
                RaisePropertyChangedEvent("EditTradeControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable disable get data button].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable disable get data button]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDisableGetDataButton
        {
            get { return _enableDisableGetDataButton; }
            set
            {
                _enableDisableGetDataButton = value;
                RaisePropertyChangedEvent("EnableDisableGetDataButton");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable disable tool bar].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable disable tool bar]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDisableToolBar
        {
            get { return _enableDisableToolBar; }
            set
            {
                _enableDisableToolBar = value;
                RaisePropertyChangedEvent("EnableDisableToolBar");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable disable UI elements].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable disable UI elements]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDisableUiElements
        {
            get { return _enableDisableUIElements; }
            set
            {
                _enableDisableUIElements = value;
                RaisePropertyChangedEvent("EnableDisableUiElements");
            }
        }

        /// <summary>
        /// Gets or sets the is allocation dock pane visible.
        /// </summary>
        /// <value>
        /// The is allocation dock pane visible.
        /// </value>
        public Visibility IsAllocationDockPaneVisible
        {
            get { return _isAllocationDockPaneVisible; }
            set
            {
                _isAllocationDockPaneVisible = value;
                RaisePropertyChangedEvent("IsAllocationDockPaneVisible");
            }
        }

        /// <summary>
        /// Gets or sets the is allocation dock pane visible.
        /// </summary>
        /// <value>
        /// The is allocation dock pane visible.
        /// </value>
        public Visibility IsSwapDockPaneVisible
        {
            get { return _isSwapDockPaneVisible; }
            set
            {
                _isSwapDockPaneVisible = value;
                RaisePropertyChangedEvent("IsSwapDockPaneVisible");
            }
        }


        /// <summary>
        /// Gets or sets the is allocation dock pane visible.
        /// </summary>
        /// <value>
        /// The is allocation dock pane visible.
        /// </value>
        public Visibility IsOTCDockPaneVisible
        {
            get { return _isOTCDockPaneVisible; }
            set
            {
                _isOTCDockPaneVisible = value;
                RaisePropertyChangedEvent("IsOTCDockPaneVisible");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current rb checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is current rb checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentRBChecked
        {
            get { return _isCurrentRBChecked; }
            set
            {
                _isCurrentRBChecked = value;
                if (_isCurrentRBChecked)
                {
                    SelectedFromDate = DateTime.Now;
                    SelectedToDate = DateTime.Now;
                    IsFromDatePickerEnabled = false;
                    IsToDatePickerEnabled = false;
                    if (PromptForDataSaving(ActionAfterSavingData.GetData) != MessageBoxResult.Yes)
                        LoadDataAndClearGroupId();
                }
                RaisePropertyChangedEvent("IsCurrentRBChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is edit mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is edit mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                RaisePropertyChangedEvent("IsEditMode");
            }
        }

        /// <summary>
        /// Gets or sets the is edit trade dock pane visible.
        /// </summary>
        /// <value>
        /// The is edit trade dock pane visible.
        /// </value>
        public Visibility IsEditTradeDockPaneVisible
        {
            get { return _isEditTradeDockPaneVisible; }
            set
            {
                _isEditTradeDockPaneVisible = value;
                RaisePropertyChangedEvent("IsEditTradeDockPaneVisible");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is from date picker enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is from date picker enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFromDatePickerEnabled
        {
            get { return _isFromDatePickerEnabled; }
            set
            {
                _isFromDatePickerEnabled = value;
                RaisePropertyChangedEvent("IsFromDatePickerEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is historical rb checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is historical rb checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsHistoricalRBChecked
        {
            get { return _isHistoricalRBChecked; }
            set
            {
                _isHistoricalRBChecked = value;
                if (_isHistoricalRBChecked)
                {
                    IsFromDatePickerEnabled = true;
                    IsToDatePickerEnabled = true;
                    AllocationClientManager.GetInstance().ClearData();
                }
                RaisePropertyChangedEvent("IsHistoricalRBChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is load layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is load layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoadLayout
        {
            get { return _isLoadLayout; }
            set
            {
                _isLoadLayout = value;
                RaisePropertyChangedEvent("IsLoadLayout");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayout
        {
            get { return _isSaveLayout; }
            set
            {
                _isSaveLayout = value;
                RaisePropertyChangedEvent("IsSaveLayout");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save without state enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save without state enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveWithoutStateEnabled
        {
            get { return _isSaveWithoutStateEnabled; }
            set
            {
                _isSaveWithoutStateEnabled = value;
                RaisePropertyChangedEvent("IsSaveWithoutStateEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save with state enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save with state enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveWithStateEnabled
        {
            get { return _isSaveWithStateEnabled; }
            set
            {
                _isSaveWithStateEnabled = value;
                RaisePropertyChangedEvent("IsSaveWithStateEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is to date picker enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is to date picker enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsToDatePickerEnabled
        {
            get { return _isToDatePickerEnabled; }
            set
            {
                _isToDatePickerEnabled = value;
                RaisePropertyChangedEvent("IsToDatePickerEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the selected from date.
        /// </summary>
        /// <value>
        /// The selected from date.
        /// </value>
        public DateTime SelectedFromDate
        {
            get { return _selectedFromDate; }
            set
            {
                _selectedFromDate = value;
                RaisePropertyChangedEvent("SelectedFromDate");
            }
        }

        /// <summary>
        /// Gets or sets the selected to date.
        /// </summary>
        /// <value>
        /// The selected to date.
        /// </value>
        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set
            {
                _selectedToDate = value;
                RaisePropertyChangedEvent("SelectedToDate");
            }
        }

        /// <summary>
        /// Gets or sets the status bar text.
        /// </summary>
        /// <value>
        /// The status bar text.
        /// </value>
        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                _statusBarText = " [" + DateTime.Now + "] " + value;
                RaisePropertyChangedEvent("StatusBarText");
            }
        }

        /// <summary>
        /// Gets or sets the trade attribute bulk change control view model.
        /// </summary>
        /// <value>
        /// The trade attribute bulk change control view model.
        /// </value>
        public TradeAttributeBulkChangeControlViewModel TradeAttributeBulkChangeControlViewModel
        {
            get { return _tradeAttributeBulkChangeControlViewModel; }
            set
            {
                _tradeAttributeBulkChangeControlViewModel = value;
                RaisePropertyChangedEvent("TradeAttributeBulkChangeControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes.
        /// </summary>
        /// <value>
        /// The trade attributes.
        /// </value>
        public ObservableCollection<string>[] TradeAttributes
        {
            get { return _tradeAttributes; }
            set
            {
                _tradeAttributes = value;
                RaisePropertyChangedEvent("TradeAttributes");
            }
        }

        /// <summary>
        /// Gets or sets the height of the form.
        /// </summary>
        /// <value>
        /// The height of the form.
        /// </value>
        public int FormHeight
        {
            get { return _formHeight; }
            set
            {
                _formHeight = value;
                RaisePropertyChangedEvent("FormHeight");
            }
        }

        /// <summary>
        /// Gets or sets the width of the form.
        /// </summary>
        /// <value>
        /// The width of the form.
        /// </value>
        public int FormWidth
        {
            get { return _formWidth; }
            set
            {
                _formWidth = value;
                RaisePropertyChangedEvent("FormWidth");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pinned account strategy grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pinned account strategy grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsPinnedAccountStrategyGrid
        {
            get { return _isPinnedAccountStrategyGrid; }
            set
            {
                _isPinnedAccountStrategyGrid = value;
                LoadAccountStrategyGridForSelectedGroup();
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the allocation client view loaded.
        /// </summary>
        /// <value>
        /// The allocation client view loaded.
        /// </value>
        public RelayCommand<object> AllocationClientViewLoaded { get; set; }

        /// <summary>
        /// Gets or sets the allow edit grids.
        /// </summary>
        /// <value>
        /// The allow edit grids.
        /// </value>
        public RelayCommand<object> AllowEditGrids { get; set; }

        /// <summary>
        /// Gets or sets the group allocation groups.
        /// </summary>
        /// <value>
        /// The group allocation groups.
        /// </value>
        public RelayCommand<object> AutoGroup { get; set; }

        /// <summary>
        /// Gets or sets the close data.
        /// </summary>
        /// <value>
        /// The close data.
        /// </value>
        public RelayCommand<object> CloseData { get; set; }

        /// <summary>
        /// Gets or sets the load allocation filtered data.
        /// </summary>
        /// <value>
        /// The load allocation filtered data.
        /// </value>
        public RelayCommand<object> LoadAllocationFilteredData { get; set; }

        /// <summary>
        /// Gets or sets the load edit alloc preference UI.
        /// </summary>
        /// <value>
        /// The load edit alloc preference UI.
        /// </value>
        public RelayCommand<object> LoadEditAllocationPreferencesUI { get; set; }

        /// <summary>
        /// Gets or sets the load preference UI.
        /// </summary>
        /// <value>
        /// The load preference UI.
        /// </value>
        public RelayCommand<object> LoadAllocationPreferencesUI { get; set; }

        /// <summary>
        /// Gets or sets the load prorata UI.
        /// </summary>
        /// <value>
        /// The load prorata UI.
        /// </value>
        public RelayCommand<object> LoadProrataUI { get; set; }

        /// <summary>
        /// Gets or sets the save allocation groups.
        /// </summary>
        /// <value>
        /// The save allocation groups.
        /// </value>
        public RelayCommand<object> SaveAllocationGroups { get; set; }

        /// <summary>
        /// Gets or sets the screen shot.
        /// </summary>
        /// <value>
        /// The screen shot.
        /// </value>
        public RelayCommand<object> ScreenShot { get; set; }

        /// <summary>
        /// Gets or sets the export allocation data.
        /// </summary>
        /// <value>
        /// The export allocation data.
        /// </value>
        public RelayCommand<object> ExportAllocationData { get; set; }

        /// <summary>
        /// Gets or sets the allocation client deactivated.
        /// </summary>
        /// <value>
        /// The allocation client deactivated.
        /// </value>
        public RelayCommand<object> AllocationClientDeactivated { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationClientViewModel"/> class.
        /// </summary>
        public AllocationClientViewModel()
        {
            try
            {

                _isNewOTCWorkflow = CachedDataManager.GetInstance.IsNewOTCWorkflow;
                _allocationGridControlViewModel = new AllocationGridControlViewModel();
                _accountStrategyControlViewModel = new AccountStrategyControlViewModel();
                _currentSymbolStateControlViewModel = new CurrentSymbolStateControlViewModel();
                _allocationFiltersControlViewModel = new AllocationFiltersControlViewModel();
                _tradeAttributeBulkChangeControlViewModel = new TradeAttributeBulkChangeControlViewModel();
                _commissionFeesBulkChangeControlViewModel = new CommissionFeesBulkChangeControlViewModel();
                _allocationSwapControlViewModel = new AllocationSwapControlViewModel();
                _allocationOTCControlViewModel = new AllocationOTCControlViewModel();
                _editTradeControlViewModel = new EditTradeControlViewModel();
                LoadAllocationFilteredData = new RelayCommand<object>((parameter) => GetAllocationData(parameter));
                AllocationClientViewLoaded = new RelayCommand<object>((parameter) => LoadAllocationClientViewData(parameter));
                AllocationClientDeactivated = new RelayCommand<object>((parameter) => OnAllocationClientDeactivated(parameter));
                AutoGroup = new RelayCommand<object>((parameter) => ApplyAutoGrouping(parameter));
                SaveAllocationGroups = new RelayCommand<object>((parameter) => SaveAllocationData(parameter));
                CloseData = new RelayCommand<object>((parameter) => LoadClosingWizardUI(parameter));
                ScreenShot = new RelayCommand<object>((parameter) => ScreenShotClick(parameter));
                LoadAllocationPreferencesUI = new RelayCommand<object>((parameter) => LoadPrefUIClick(parameter));
                LoadEditAllocationPreferencesUI = new RelayCommand<object>((parameter) => LoadEditAllocPrefUIClick(parameter));
                LoadProrataUI = new RelayCommand<object>((parameter) => LoadProrataUIClick(parameter));
                ExportAllocationData = new RelayCommand<object>((parameter) => ExportAllocationDataClick(parameter));
                AllowEditGrids = new RelayCommand<object>((parameter) => EditGridButtonClick(parameter));
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Handles the UpdateStatusAllocation event of the _accountStrategyControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void _accountStrategyControlViewModel_UpdateStatusAllocation(object sender, EventArgs<string> e)
        {
            try
            {
                ToggleUIElements(e.Value, true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ApplyFilterEvent event of the _allocationFiltersControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void _allocationFiltersControlViewModel_ApplyFilterEvent(object sender, EventArgs e)
        {
            try
            {
                if (PromptForDataSaving(ActionAfterSavingData.GetData) != MessageBoxResult.Yes)
                {
                    _isApplyFilterClicked = true;
                    LoadDataAndClearGroupId();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Active item of Allocated Grid
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationGroup}" /> instance containing the event data.</param>
        private void _allocationGridControlViewModel_ActiveAllocatedGroup(object sender, EventArgs<AllocationGroup> e)
        {
            try
            {
                _commissionFeesBulkChangeControlViewModel.TaxlotEnabled = true;
                _tradeAttributeBulkChangeControlViewModel.TaxlotEnabled = true;

                IsAllocatedGridSelected = true;
                AllocationGroup group = e.Value;
                // Update the status of the selected group ove Account-Strategy Grid, PRANA-15197
                _accountStrategyControlViewModel.SelectedGroup = AllocationClientConstants.SYMBOL_STATUS + group.Symbol + "\t " + AllocationClientConstants.STATE_STATUS + group.State + "\t " + AllocationClientConstants.CLOSING_STATUS + group.ClosingStatus;
                _accountStrategyControlViewModel.SetAccountStrategyControl(false);
                _accountStrategyControlViewModel.SetQuantity(group.CumQty);
                if (IsPinnedAccountStrategyGrid)
                {
                    _accountStrategyControlViewModel.SetAllocationAccounts(group);
                }

                SetAllocationOTCView(group);

                if (AllocationPermissions.EditTradeModulePermission)
                {
                    TradeAttributesCache.updateCache(GetTradeAttributesListCollection(_allocationGridControlViewModel.TradeAttributesCollection));
                    _editTradeControlViewModel.TradeAttributesCollection = _allocationGridControlViewModel.TradeAttributesCollection;
                    Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { group });
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];
                    _editTradeControlViewModel.ShowGroupDetails(group, groupStatus);
                    _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection = _allocationGridControlViewModel.TradeAttributesCollection;
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Set Allocation OTC View
        /// </summary>
        /// <param name="group"></param>
        private void SetAllocationOTCView(AllocationGroup group)
        {
            try
            {

                if (_isNewOTCWorkflow)
                {
                    //if (group.OTCParameters != null)
                    //{
                    _allocationOTCControlViewModel.SetPreviewToSwapUI(group);
                    _allocationOTCControlViewModel.EnableControl = true;
                    //}
                }
                else
                {
                    if (group.AssetName.Equals("Equity"))
                    {
                        _allocationSwapControlViewModel.IsSwapUpdateButton = Visibility.Visible;
                        _allocationSwapControlViewModel.IsReadOnly = false;
                        _allocationSwapControlViewModel.SetPreviewToSwapUI(group);
                        _allocationSwapControlViewModel.EnableControl = true;
                    }
                    else
                    {
                        _allocationSwapControlViewModel.IsSwapUpdateButton = Visibility.Hidden;
                        _allocationSwapControlViewModel.EnableControl = false;
                        _allocationSwapControlViewModel.SetDefaultValues();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// Active item of UnAllocated Grid
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationGroup}" /> instance containing the event data.</param>
        private void _allocationGridControlViewModel_ActiveUnAllocatedGroup(object sender, EventArgs<AllocationGroup> e)
        {
            try
            {
                _commissionFeesBulkChangeControlViewModel.TaxlotEnabled = false;
                _commissionFeesBulkChangeControlViewModel.GroupLevel = true;
                _tradeAttributeBulkChangeControlViewModel.TaxlotEnabled = false;
                _tradeAttributeBulkChangeControlViewModel.GroupLevel = true;

                AllocationGroup group = e.Value;
                // Update the status of the selected group ove Account-Strategy Grid, PRANA-15197
                _accountStrategyControlViewModel.SelectedGroup = AllocationClientConstants.SYMBOL_STATUS + group.Symbol + "\t " + AllocationClientConstants.STATE_STATUS + group.State + "\t " + AllocationClientConstants.CLOSING_STATUS + group.ClosingStatus;
                _accountStrategyControlViewModel.SetAccountStrategyControl(true);

                if (IsAllocatedGridSelected)
                    IsAllocatedGridSelected = false;

                if (_isPinnedAccountStrategyGrid)
                {
                    if (_accountStrategyControlViewModel.IsCalculatedAllocationSelected)
                        PreviewAllocationGridData(e.Value);
                    else
                        _accountStrategyControlViewModel.ClearGridOnly();
                }

                SetAllocationOTCView(group);

                if (AllocationPermissions.EditTradeModulePermission)
                {
                    TradeAttributesCache.updateCache(GetTradeAttributesListCollection(_allocationGridControlViewModel.TradeAttributesCollection));
                    _editTradeControlViewModel.TradeAttributesCollection = _allocationGridControlViewModel.TradeAttributesCollection;
                    Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { group });
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];
                    _editTradeControlViewModel.ShowGroupDetails(group, groupStatus);
                    _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection = _allocationGridControlViewModel.TradeAttributesCollection;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UnAllocatedGroupGrid HeaderCheckboxChange event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">List of AllocationGroup</param>
        private void _allocationGridControlViewModel_HeaderCheckboxChangeUnAllocatedGroupEvent(object sender, EventArgs<List<AllocationGroup>> e)
        {
            try
            {
                var totalrows = e.Value.Count;
                if (totalrows > 0)
                {
                    AllocationGroup group = e.Value[totalrows - 1];
                    _allocationGridControlViewModel.ActiveUnallocatedDataItem = group;
                    _allocationGridControlViewModel_ActiveUnAllocatedGroup(this, new EventArgs<AllocationGroup>(group));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        /// <summary>
        /// Handles the SetAccountStrategyControlEvent event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.Boolean}"/> instance containing the event data.</param>
        void _allocationGridControlViewModel_SetAccountStrategyControlEvent(object sender, EventArgs<bool> e)
        {
            try
            {
                bool isUnallocatedGridSelected = e.Value;
                _commissionFeesBulkChangeControlViewModel.TaxlotEnabled = !isUnallocatedGridSelected;
                _tradeAttributeBulkChangeControlViewModel.TaxlotEnabled = !isUnallocatedGridSelected;
                if (isUnallocatedGridSelected == true)
                {
                    _commissionFeesBulkChangeControlViewModel.TaxlotLevel = !isUnallocatedGridSelected;
                    _tradeAttributeBulkChangeControlViewModel.TaxlotLevel = !isUnallocatedGridSelected;
                }
                _commissionFeesBulkChangeControlViewModel.GroupLevel = true;
                _tradeAttributeBulkChangeControlViewModel.GroupLevel = true;
                _accountStrategyControlViewModel.SetAccountStrategyControl(isUnallocatedGridSelected);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocateGroup event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void _allocationGridControlViewModel_AllocateGroup(object sender, EventArgs e)
        {
            string statusResult = string.Empty;
            try
            {
                _allocationGridControlViewModel.EndEditModeAndCommitChanges = true;
                ToggleUIElements("Data Allocation Started", false);
                bool isReallocate = _allocationGridControlViewModel.GetSelectedUnAllocatedGroups().Count == 0 ? true : false;
                List<AllocationGroup> groups = isReallocate ? _allocationGridControlViewModel.GetSelectedAllocatedGroups() : _allocationGridControlViewModel.GetSelectedUnAllocatedGroups();
                foreach (var group in groups)
                {
                    if (group.IsOverbuyOversellAccepted)
                        group.IsOverbuyOversellAccepted = false;
                }
                if (groups.Count <= 0)
                {
                    statusResult = "Please Select trade/group(s) to allocate.";
                    return;
                }
                bool isForceAllocationSelected = _accountStrategyControlViewModel.IsForceAllocationSelected ?? false;
                bool isPTTAllocationSelected = _accountStrategyControlViewModel.IsCustomCheckBoxChecked;
                AllocationPreferenceType accountType = _accountStrategyControlViewModel.GetAllocationType();
                CommissionCalculationValue(groups);
                AllocationResponse responseObject = null;
                switch (accountType)
                {
                    case AllocationPreferenceType.AllocationByAccount:
                        int prefrenceSelected = _accountStrategyControlViewModel.SelectedAllocationPreferences.Value.Key;
                        AllocationParameter allocParameter = null;
                        if (prefrenceSelected == Int32.MinValue)
                            allocParameter = _accountStrategyControlViewModel.GetAllocationParameter(isReallocate);

                        string errorMessage = string.Empty;
                        if (prefrenceSelected == Int32.MinValue && allocParameter == null)
                            statusResult = "Please select a valid preference. " + errorMessage;
                        else
                        {
                            if (IsDataSavedBeforeLeveling(allocParameter, prefrenceSelected))
                            {
                                responseObject = AllocationClientManager.GetInstance().AllocateByAccount(groups, allocParameter, prefrenceSelected, isReallocate, isForceAllocationSelected, isPTTAllocationSelected);
                                statusResult = ShowAllocationResponse(responseObject);
                            }
                            else
                                statusResult = "Data is not allocated. Need to save changes before allocating trades using Leveling";
                        }
                        break;
                    case AllocationPreferenceType.AllocationBySymbol:
                        KeyValuePair<int, string> pref = _accountStrategyControlViewModel.SelectedfixedAllocationPreference.Value;
                        if (pref.Value != null)
                        {
                            responseObject = AllocationClientManager.GetInstance().AllocateByFixedPreference(groups, pref, isForceAllocationSelected, isPTTAllocationSelected, isReallocate);
                            statusResult = ShowAllocationResponse(responseObject);
                        }
                        else
                            statusResult = "Please select a valid preference";
                        break;
                }
            }
            catch (Exception ex)
            {
                statusResult = "Something went wrong, please contact administrator.";
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                ToggleUIElements(statusResult, true);
            }
        }

        /// <summary>
        /// Determines whether [is data saved before leveling] [the specified alloc parameter].
        /// </summary>
        /// <param name="allocParameter">The alloc parameter.</param>
        /// <param name="prefrenceSelected">The prefrence selected.</param>
        /// <returns></returns>
        private bool IsDataSavedBeforeLeveling(AllocationParameter allocParameter, int prefrenceSelected)
        {
            try
            {
                bool isLevelingUsed = false;
                if (allocParameter == null)
                {
                    AllocationOperationPreference pref = GetAllocationPreference(prefrenceSelected);
                    if (pref == null)
                    {
                        AllocationMasterFundPreference mfPref = AllocationClientManager.GetInstance().GetMasterFundPreference(prefrenceSelected);
                        if (mfPref != null && mfPref.DefaultRule.RuleType != MatchingRuleType.Leveling)
                        {
                            foreach (int prefID in mfPref.MasterFundPreference.Values)
                            {
                                AllocationOperationPreference childPref = GetAllocationPreference(prefID);
                                if (childPref == null || childPref.IsLevelingUsed())
                                {
                                    isLevelingUsed = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isLevelingUsed = true;
                        }
                    }
                    else
                    {
                        isLevelingUsed = pref.IsLevelingUsed();
                    }
                }
                else
                {
                    isLevelingUsed = allocParameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling;
                }
                if (isLevelingUsed)
                {
                    return PromptForDataSaving(ActionAfterSavingData.DoNothing, AllocationClientConstants.SAVE_ALLOCATION_DATA_LEVELING) == MessageBoxResult.None;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the DeleteAllocationGroup event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationGroup}" /> instance containing the event data.</param>
        private void _allocationGridControlViewModel_DeleteAllocationGroup(object sender, EventArgs<List<AllocationGroup>> e)
        {
            try
            {
                List<AllocationGroup> groupList = e.Value;
                if (groupList != null && groupList.Count > 0)
                {
                    ToggleUIElements("Deleting selected trades, Please wait..", false);
                    groupList.ForEach(group => AllocationClientManager.GetInstance().DeleteAllocationGroup(group));
                    ToggleUIElements("Trades deleted successfully.", true);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the GetAuditClick event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void _allocationGridControlViewModel_GetAuditClick(object sender, EventArgs e)
        {
            try
            {
                if (LoadAuditUIEvent != null)
                    LoadAuditUIEvent(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the GroupUngroupAllocationGroups event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _allocationGridControlViewModel_GroupUngroupAllocationGroups(object sender, EventArgs<List<AllocationGroup>, string> e)
        {
            try
            {
                if (e.Value2.Equals(AllocationClientConstants.MENU_GROUP))
                    ApplyGrouping(e.Value);
                else if (e.Value2.Equals(AllocationClientConstants.MENU_UNGROUP))
                    UngroupGroups(e.Value);
                else if (e.Value2.Equals(AllocationClientConstants.MENU_UNGROUP_ALLOCATED))
                    UngroupGroupsAllocated(e.Value);

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the LoadCashTransactionUI event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void _allocationGridControlViewModel_LoadCashTransactionUI(object sender, EventArgs e)
        {
            try
            {
                if (LoadCashTransactionUIEvent != null)
                    LoadCashTransactionUIEvent(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the LoadCloseTradeUI event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationGroup}" /> instance containing the event data.</param>
        void _allocationGridControlViewModel_LoadCloseTradeUI(object sender, EventArgs<AllocationGroup> e)
        {
            try
            {
                if (LoadCloseTradeUIEvent != null)
                    LoadCloseTradeUIEvent(this, new EventArgs<AllocationGroup>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the loadSymbolLookUp event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _allocationGridControlViewModel_loadSymbolLookUp(object sender, EventArgs<string> e)
        {
            try
            {
                if (LoadSymbolLookupEvent != null)
                    LoadSymbolLookupEvent(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the OpenExternalTransactionUI event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void _allocationGridControlViewModel_OpenExternalTransactionUI(object sender, EventArgs e)
        {
            try
            {
                TaxLot taxLot = (TaxLot)_allocationGridControlViewModel.ActiveAllocatedDataItem;
                AllocationGroup group = AllocationClientGroupCache.GetInstance.GetGroup(taxLot.GroupID);
                if (_frmExternalTransaction != null)
                {
                    MessageBox.Show("External Transaction is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (group.PersistenceStatus.Equals(ApplicationConstants.PersistenceStatus.ReAllocated) && group.IsTaxlotDictChanged())
                {
                    MessageBox.Show("The group allocation has been changed. Please save the changes before updating External TransactionID.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowExternalTransactionUi(viewModel => dialogService.ShowDialog<AddAndUpdateExternalTransactionID>(this, viewModel));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SaveLayout event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _allocationGridControlViewModel_SaveLayout(object sender, EventArgs<bool> e)
        {
            try
            {
                if (e.Value)
                    IsSaveLayout = true;
                _allocationUserWisePref.AllocationFormHeight = FormHeight;
                _allocationUserWisePref.AllocationFormWidth = FormWidth;
                _accountStrategyControlViewModel.SaveAccountStrategyGridLayout();
                AllocationClientManager.GetInstance().SaveLayout(_allocationUserWisePref);

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the UnallocateGroup event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _allocationGridControlViewModel_UnallocateGroup(object sender, EventArgs<List<AllocationGroup>> e)
        {
            try
            {
                ToggleUIElements("Unallocating selected data. Please wait..", false);
                AllocationClientManager.GetInstance().UnAllocateGroupAsync(e.Value);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the UpdateTotalNoOfTrades event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void _allocationGridControlViewModel_UpdateTotalNoOfTrades(object sender, EventArgs<int, int> e)
        {
            try
            {
                _accountStrategyControlViewModel.UpdateTotalNoOfTrades(e.Value, e.Value2);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SwapUpdateClick event of the _allocationSwapControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _allocationOTCControlViewModel_SaveBtnClick(object sender, EventArgs<OTCTradeData> e)
        {
            try
            {
                AllocationGroup group = _allocationGridControlViewModel.GetCurrentActiveGroup();
                if (group != null)
                {
                    Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { group });
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];

                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        if (e.Value != null)
                        {
                            group.OTCParameters = e.Value;
                            group.OTCParameters.GroupID = group.GroupID;
                            //if (!group.IsSwapped)
                            //{
                            //    group.IsSwapped = true;
                            //    group.AddTradeAction(TradeAuditActionType.ActionType.BookAsSwap);
                            //    group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.BookAsSwap);
                            //}
                            //else
                            //{
                            //    group.AddTradeAction(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                            //    group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                            //}
                            AllocationClientManager.GetInstance().DictUnsavedAdd(group.GroupID, group);
                        }
                        else
                            return;
                        //MessageBoxResult choice = MessageBox.Show("Would you like to calculate commission and fee again?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //if (choice == MessageBoxResult.Yes)
                        //{
                        //    group.IsRecalculateCommission = true;
                        //    AllocationGroup allocationGroup = AllocationClientManager.GetInstance().CalculateCommission(group);
                        //    if (allocationGroup != null)
                        //    {
                        //        group.UpdateCommissionAndFees(allocationGroup);
                        //        group.UpdateCommissionAndFeesAtTaxlotLevel(allocationGroup);
                        //    }
                        //}
                        if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                        }
                        if (group.TaxLots != null && group.TaxLots.Count > 0)
                        {
                            foreach (TaxLot taxlotVar in group.TaxLots)
                            {
                                //taxlotVar.Ass = true;
                                taxlotVar.OTCParameters = group.OTCParameters.Clone();
                                group.UpdateTaxlotState(taxlotVar);
                            }
                        }
                        else
                        {
                            TaxLot updatedTaxlot = AllocationClientManager.GetInstance().CreateUnAllocTaxlot(group);
                            updatedTaxlot.ISSwap = true;
                            updatedTaxlot.OTCParameters = group.OTCParameters;
                            group.UpdateTaxlotState(updatedTaxlot);
                        }

                        group.PropertyHasChanged();
                        ToggleUIElements("Swap information updated", true);
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                            MessageBox.Show("Group is Partially or Fully Closed. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                            MessageBox.Show(" Corporate Action has been applied on this Group. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SwapUpdateClick event of the _allocationSwapControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _allocationSwapControlViewModel_SwapUpdateClick(object sender, EventArgs<SwapParameters> e)
        {
            try
            {
                AllocationGroup group = _allocationGridControlViewModel.GetCurrentActiveGroup();
                if (group != null)
                {
                    Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { group });
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];

                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        if (e.Value != null && e.Value.DayCount > 0 && e.Value.OrigCostBasis > 0 && DateTime.Parse(e.Value.FirstResetDate.ToString()) >= DateTime.Parse(e.Value.OrigTransDate.ToString()))
                        {
                            group.SwapParameters = e.Value;
                            group.SwapParameters.GroupID = group.GroupID;
                            if (!group.IsSwapped)
                            {
                                group.IsSwapped = true;
                                group.AddTradeAction(TradeAuditActionType.ActionType.BookAsSwap);
                                group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.BookAsSwap);
                            }
                            else
                            {
                                group.AddTradeAction(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                                group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                            }
                            AllocationClientManager.GetInstance().DictUnsavedAdd(group.GroupID, group);
                        }
                        else
                            return;
                        MessageBoxResult choice = MessageBox.Show("Would you like to calculate commission and fee again?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (choice == MessageBoxResult.Yes)
                        {
                            group.IsRecalculateCommission = true;
                            AllocationGroup allocationGroup = AllocationClientManager.GetInstance().CalculateCommission(group);
                            if (allocationGroup != null)
                            {
                                group.UpdateCommissionAndFees(allocationGroup);
                                group.UpdateCommissionAndFeesAtTaxlotLevel(allocationGroup);
                            }
                        }
                        if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                        }
                        if (group.TaxLots != null && group.TaxLots.Count > 0)
                        {
                            foreach (TaxLot taxlotVar in group.TaxLots)
                            {
                                //taxlotVar.Ass = true;
                                taxlotVar.SwapParameters = group.SwapParameters.Clone();
                                taxlotVar.SwapParameters.NotionalValue = (taxlotVar.TaxLotQty * group.SwapParameters.NotionalValue) / group.CumQty;
                                taxlotVar.ISSwap = true;
                                group.UpdateTaxlotState(taxlotVar);
                            }
                        }
                        else
                        {
                            TaxLot updatedTaxlot = AllocationClientManager.GetInstance().CreateUnAllocTaxlot(group);
                            updatedTaxlot.ISSwap = true;
                            updatedTaxlot.SwapParameters = group.SwapParameters;
                            group.UpdateTaxlotState(updatedTaxlot);
                        }

                        group.PropertyHasChanged();
                        ToggleUIElements("Swap information updated", true);
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                            MessageBox.Show("Group is Partially or Fully Closed. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                            MessageBox.Show(" Corporate Action has been applied on this Group. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the BulkChangeGroup event of the _commissionFeesBulkChangeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{BulkChangesGroupLevel}" /> instance containing the event data.</param>
        void _commissionFeesBulkChangeControlViewModel_BulkChangeGroup(object sender, EventArgs<BulkChangesGroupLevel> e)
        {
            string result = string.Empty;
            try
            {
                ToggleUIElements("Applying commission and fees bulk changes. Please Wait..", false);
                BulkChangesGroupLevel bulkChanges = e.Value;
                List<AllocationGroup> groups = new List<AllocationGroup>();
                if (bulkChanges.GroupWise)
                    groups = _allocationGridControlViewModel.GetSelectedAllocatedGroups();
                else
                    groups = _allocationGridControlViewModel.GetSelectedGroupsForAccounts(bulkChanges.AccountIDs);

                if (groups.Count > 0)
                {
                    result = AllocationClientManager.GetInstance().UpdateBulkChangeGroups(bulkChanges, groups);
                }
                else
                    result = "Please select atleast one valid record";
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                ToggleUIElements(result, true);
            }
        }

        /// <summary>
        /// Handles the ReCalculateCommission event of the _commissionFeesBulkChangeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void _commissionFeesBulkChangeControlViewModel_ReCalculateCommission(object sender, EventArgs<CommissionRule, bool> e)
        {
            string response = string.Empty;
            try
            {
                ToggleUIElements("Applying commission and fees bulk changes. Please Wait..", false);
                CommissionRuleAndFee = e.Value;
                List<AllocationGroup> groups = _allocationGridControlViewModel.GetSelectedAllocatedGroups();
                if (groups.Count > 0)
                    response = AllocationClientManager.GetInstance().ReCalculateCommissions(CommissionRuleAndFee, groups, e.Value2);
                else
                    response = "Please select atleast one record!";
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                ToggleUIElements(response, true);
            }
        }

        /// <summary>
        /// Handles the UpdateCurrentStateForSymbols event of the _allocationGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _currentSymbolStateControlViewModel_UpdateCurrentStateForSymbols(object sender, EventArgs<List<string>> e)
        {
            try
            {
                List<string> uniqueSymbolList = new List<string>();
                uniqueSymbolList.AddRange(_allocationGridControlViewModel.GetUniqueSelectedSymbols());
                AllocationClientManager.GetInstance().GetCurrentStateForSymbol(uniqueSymbolList);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        /// <summary>
        /// Handles the ApplyEditTradeChangesEvent event of the _editTradeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationGroup}" /> instance containing the event data.</param>
        private void _editTradeControlViewModel_ApplyEditTradeChangesEvent(object sender, EventArgs e)
        {
            try
            {
                TradeAttributesCache.updateCache(GetTradeAttributesListCollection(_editTradeControlViewModel.TradeAttributesCollection));
                _allocationGridControlViewModel.TradeAttributesCollection = _editTradeControlViewModel.TradeAttributesCollection;
                _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection = _editTradeControlViewModel.TradeAttributesCollection;
                ToggleUIElements("Selected group modified.", true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CloseExternalTransaction event of the _frmExternalTransactionId control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _frmExternalTransactionId_CloseExternalTransaction(object sender, EventArgs e)
        {
            try
            {
                if (_frmExternalTransaction != null)
                    _frmExternalTransaction = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UpdateExternalTransactionID event of the _frmExternalTransactionId control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void _frmExternalTransactionId_UpdateExternalTransactionID(object sender, EventArgs<string> e)
        {
            try
            {
                _selectedTaxlotForExternalTransUpdate.ExternalTransId = e.Value;
                AllocationGroup group = AllocationClientGroupCache.GetInstance.GetGroup(_selectedTaxlotForExternalTransUpdate.GroupID);
                group.UpdateGroupPersistenceStatus();
                _selectedTaxlotForExternalTransUpdate.PropertyHasChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the UpdateGroupsLevel event of the _tradeAttributeBulkChangeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{TradeAttributesLevel}"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void _tradeAttributeBulkChangeControlViewModel_UpdateGroupsLevel(object sender, EventArgs<TradeAttributes, List<object>> e)
        {
            string result = string.Empty;
            try
            {
                ToggleUIElements("Applying trade attribute bulk changes. Please Wait..", false);

                //Update trade attributes cache
                TradeAttributesCache.updateCache(GetTradeAttributesListCollection(_tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection));
                _allocationGridControlViewModel.TradeAttributesCollection = _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection;
                _editTradeControlViewModel.TradeAttributesCollection = _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection;

                List<int> accountIDs = new List<int>();
                List<AllocationGroup> groupList = new List<AllocationGroup>();

                if (_tradeAttributeBulkChangeControlViewModel.GroupLevel)
                    groupList = _allocationGridControlViewModel.GetSelectedGroups();
                else
                {
                    List<object> accountsNames = e.Value2;
                    if (accountsNames != null && accountsNames.Count > 0)
                    {
                        accountsNames.ForEach(accountName => { accountIDs.Add(CachedDataManager.GetInstance.GetAccountID(accountName.ToString())); });
                        groupList = _allocationGridControlViewModel.GetSelectedGroupsForAccounts(accountIDs);
                    }
                    else
                        result = "Please select atleast one account!";
                }

                if (groupList.Count > 0)
                {
                    TradeAttributes tradeAttributeGroups = e.Value;
                    result = AllocationClientManager.GetInstance().UpdateTradeAttributeBulkChanges(_tradeAttributeBulkChangeControlViewModel.GroupLevel, tradeAttributeGroups, groupList, accountIDs);
                }
                else
                    result = (string.IsNullOrWhiteSpace(result)) ? "Please select atleast one valid record" : result;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                ToggleUIElements(result, true);
            }

        }

        /// <summary>
        /// This method returns list of selected groups
        /// </summary>
        /// <returns></returns>
        private List<AllocationGroup> GetSelectedGroups()
        {
            List<AllocationGroup> groups = new List<AllocationGroup>(); 
            try
            {
                return _allocationGridControlViewModel.GetSelectedGroups();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return groups;
        }

        /// <summary>
        /// Handles the ActionAfterSave event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void AllocationClientViewModel_ActionAfterSave(object sender, EventArgs<ActionAfterSavingData> e)
        {
            try
            {
                ActionAfterSavingData action = e.Value;
                switch (action)
                {
                    case ActionAfterSavingData.GetData:
                        LoadDataAndClearGroupId();
                        break;
                    case ActionAfterSavingData.CloseAllocation:
                        if (CloseAllocationClient != null)
                            CloseAllocationClient(this, EventArgs.Empty);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocationDataChange event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void AllocationClientViewModel_AllocationDataChange(object sender, EventArgs<bool> e)
        {
            try
            {
                if (AllocationDataChange != null)
                    AllocationDataChange(this, new EventArgs<bool>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocationDataSaved event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AllocationClientViewModel_AllocationDataSaved(object sender, EventArgs e)
        {
            try
            {
                ClearSelectionAfterResponse();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocationPreferencesSaved event of the EditPref control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationRule}" /> instance containing the event data.</param>
        void AllocationClientViewModel_AllocationPreferencesSaved(object sender, EventArgs<AllocationPreferencesCollection> e)
        {
            try
            {
                AllocationPreferencesCollection allocationPreferencesCollection = e.Value;
                _allocationUserWisePref = allocationPreferencesCollection.AllocationPreferences;
                SetPreferences();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocationPreferenceUpdated event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void AllocationClientViewModel_AllocationPreferenceUpdated(object sender, EventArgs e)
        {
            try
            {               
                _allocationUserWisePref = AllocationClientManager.GetInstance().GetUserWisePreferences();
                
                var temp = new Dictionary<int, string>();
                temp.Add(int.MinValue, "-Select-");
                var allocationPrefList = AllocationClientManager.GetInstance().GetOperationPreferencesList();
                foreach (var item in allocationPrefList)
                {
                    temp.Add(item.Key, item.Value);
                }
                _allocationOperationPreferences = temp;

                var temp1 = new Dictionary<int, string>();
                temp1.Add(int.MinValue, "-Select-");
                var allocationPrefList1 = AllocationClientManager.GetInstance().GetMasterFundPreferenceList();
                foreach (var item in allocationPrefList1)
                {
                    temp1.Add(item.Key, item.Value);
                }
                _allocationMasterFundPreferences = temp1;

                SetPreferences();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the AllocationSchemeUpdated event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void AllocationClientViewModel_AllocationSchemeUpdated(object sender, EventArgs e)
        {
            try
            {
                var fixedPrefList = AllocationClientPreferenceManager.GetInstance.UpdateSorting(AllocationClientPreferenceManager.GetInstance.GetFixedPreferencesList());
                _accountStrategyControlViewModel.SetFixedPreferenceData(fixedPrefList);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the NewGroupReceived event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void AllocationClientViewModel_NewGroupReceived(object sender, EventArgs<List<AllocationGroup>> e)
        {
            try
            {
                AccountCollection accounts = CachedDataManager.GetInstance.GetUserAccounts();
                List<AllocationGroup> updatedGroupList = new List<AllocationGroup>();
                TradingAccountCollection tradingAccounts = WindsorContainerManager.GetTradingAccounts(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                var accountIdList = new List<int>();
                foreach (TradingAccount acc in tradingAccounts)
                {
                    if (!accountIdList.Contains(acc.TradingAccountID))
                        accountIdList.Add(acc.TradingAccountID);
                }
                foreach (AllocationGroup group in e.Value)
                {
                    bool isGroupAllowed = true;

                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && !accountIdList.Contains(group.TradingAccountID) && group.TradingAccountID != 0)
                    {
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                    }

                    #region remove group if Dates are not in range of Selected Dates

                    //No need to check for Deleted and Ungrouped Groups
                    if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.Deleted && group.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped)
                    {
                        DateTime fromDate = DateTime.UtcNow;
                        DateTime toDate = DateTime.UtcNow;
                        if (!IsHistoricalRBChecked)
                        {
                            fromDate = DateTime.UtcNow.Date.AddDays(-1 * Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation)));
                        }
                        else
                        {
                            toDate = _selectedToDate;
                            fromDate = _selectedFromDate;
                        }
                        if (!(fromDate.Date <= group.AUECLocalDate.Date && toDate.Date >= group.AUECLocalDate.Date))
                        {
                            isGroupAllowed = false;
                        }
                    }

                    #endregion

                    #region remove group if user don't have the permission of account in it

                    foreach (TaxLot taxLot in group.TaxLots)
                    {
                        if (!accounts.Contains(taxLot.Level1ID))
                        {
                            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                            break;
                        }
                    }
                    #endregion

                    if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.Deleted && group.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped)
                    {
                        NameValueFiller.FillNameDetailsOfMessage(group);
                        AllocationClientManager.GetInstance().SetDefaultPersistenceStatus(group);
                        bool isGroupDirty = AllocationClientManager.GetInstance().IsGroupsDirty(group);
                        bool isGroupDeleted = AllocationClientManager.GetInstance().IsGroupDeleted(group.GroupID);

                        if (isGroupDirty || isGroupDeleted)
                        {
                            if (AllocationDataChange != null)
                                AllocationDataChange(this, new EventArgs<bool>(true));
                            string allocationClientMsg = "Allocation Data is being changed on Allocation UI. Please click 'Get Data' to get updated data.";
                            ToggleUIElements(allocationClientMsg, false);
                            AllocationLoggingHelper.LoggerWriteMessage(allocationClientMsg);
                            EnableDisableToolBar = true;
                            EnableDisableGetDataButton = true;
                            _isDirtyData = true;
                            return;
                        }
                        group.IsSelected = false;
                        AllocationClientManager.GetInstance().AddGroup(group);
                    }
                    else
                        AllocationClientManager.GetInstance().DeleteGroup(group.GroupID);
                    if (isGroupAllowed)
                        updatedGroupList.Add(group);
                }
                UpdateAttributeLists(updatedGroupList);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the GroupChangedAtServerSide event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void AllocationClientViewModel_GroupChangedAtServerSide(object sender, EventArgs<string> e)
        {
            try
            {
                string allocationClientMsg = "Allocation Data was changed due to new fills came in. Please click 'Get Data' to get updated data.";
                ToggleUIElements(allocationClientMsg, false);
                AllocationLoggingHelper.LoggerWriteMessage(allocationClientMsg);
                EnableDisableToolBar = true;
                EnableDisableGetDataButton = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the DataMismatchedforClosing event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        private void AllocationClientViewModel_AllocationDataChanged(object sender, EventArgs e)
        {
            try
            {
                string allocationClientMsg = "Allocation Data was changed due to closing. Please click 'Get Data' to get updated data.";
                ToggleUIElements(allocationClientMsg, false);
                AllocationLoggingHelper.LoggerWriteMessage(allocationClientMsg);
                EnableDisableToolBar = true;
                EnableDisableGetDataButton = true;
                _isDirtyData = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the ServerProxyConnectDisconnectEvent event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void AllocationClientViewModel_ServerProxyConnectDisconnectEvent(object sender, EventArgs<bool> e)
        {
            try
            {
                if (e.Value)
                    ToggleUIElements("Trade server is Disconnected", false);
                else
                {
                    ToggleUIElements("Trade server is connected", true);
                    LoadDataAndClearGroupId();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UpdateAllocationData event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AllocationClientViewModel_UpdateAllocationData(object sender, EventArgs<AllocationResponse, bool> e)
        {
            try
            {
                AllocationResponse response = e.Value;
                ToggleUIElements(response.Response, true);

                if (e.Value2)
                    ClearSelectionAfterResponse();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UpdateStateOfSymbol event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void AllocationClientViewModel_UpdateStateOfSymbol(object sender, EventArgs<List<CurrentSymbolState>> e)
        {
            try
            {
                List<CurrentSymbolState> currentSymbolState = e.Value;
                _currentSymbolStateControlViewModel.UpdateCurrentState(currentSymbolState);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the UpdateStatusBar event of the AllocationClientViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void AllocationClientViewModel_UpdateStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                ToggleUIElements(e.Value, true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Allocations the window closing.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        internal void AllocationWindowClosing(CancelEventArgs e)
        {
            try
            {
                if (PromptForDataSaving(ActionAfterSavingData.CloseAllocation) == MessageBoxResult.Yes)
                    e.Cancel = true;
                else if (_editAllocPrefUI != null && !_editAllocPrefUI.CloseEditAllocationPreferencesUI())
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the allocPrefUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        static void allocPrefUI_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_allocPrefUI != null)
                    _allocPrefUI = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Applies the automatic grouping.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ApplyAutoGrouping(object parameter)
        {
            try
            {
                _allocationGridControlViewModel.EndEditModeAndCommitChanges = true;
                ApplyGrouping(_allocationGridControlViewModel.GetAllUnallocatedGroups());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Applies the grouping.
        /// </summary>
        /// <param name="unallocatedGroups">The unallocated groups.</param>
        internal void ApplyGrouping(List<AllocationGroup> unallocatedGroups)
        {
            try
            {
                if (unallocatedGroups != null && unallocatedGroups.Count > 0)
                {
                    _allocationGridControlViewModel.IsClearUnAllocatedSelectedItems = true;
                    ToggleUIElements("Grouping selected data. Please wait..", false);
                    AllocationClientManager.GetInstance().ApplyGrouping(unallocatedGroups);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the selection after response.
        /// </summary>
        private void ClearSelectionAfterResponse()
        {
            try
            {
                _allocationGridControlViewModel.IsAllocatedHeaderChecked = false;
                _allocationGridControlViewModel.IsUnAllocatedHeaderChecked = false;
                _allocationGridControlViewModel.IsRefreshAfterGetData = true;
                _accountStrategyControlViewModel.SelectedGroup = string.Empty;
                _accountStrategyControlViewModel.ClearGrid();
                _accountStrategyControlViewModel.IsControlOnUnallocatedGrid = true;
                _accountStrategyControlViewModel.IsEditPreferencesButtonEnabled = true;

                //Update the properties after Get Data action performed JIRA PRANA-15709
                _allocationGridControlViewModel.IsClearAllocatedSelectedItems = true;
                _allocationGridControlViewModel.IsClearUnAllocatedSelectedItems = true;
                _allocationGridControlViewModel.UnallocateMenuVisibility = false;
                _allocationGridControlViewModel.IsGroupMenuEnabled = false;
                _allocationGridControlViewModel.IsUnGroupMenuEnabled = false;
                _allocationGridControlViewModel.IsAllocatedUnGroupMenuEnabled = false;
                _allocationGridControlViewModel.TaxlotMenuItemVisibility = Visibility.Collapsed;

                _editTradeControlViewModel.ResetEditTradeFieldsValue();
                _allocationSwapControlViewModel.SetDefaultValues();
                // _allocationOTCControlViewModel.SetDefaultValues();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method shows message to calculate commission if any of selected groups have commission or soft commission source as manual
        /// </summary>
        /// <param name="groups">list of groups</param>
        private void CommissionCalculationValue(List<AllocationGroup> groups)
        {
            try
            {
                AllocationCompanyWisePref pref = AllocationClientManager.GetInstance().GetCompanyWisePreferences();
                if (pref.MsgOnAllocation || pref.RecalculateOnAllocation)
                {
                    ////show message to calculate commission again only if any of selected groups have commission or soft commission source as manual, PRANA-13009
                    bool isAnyCommissionSourceManual = false;
                    Parallel.ForEach(groups, (group, state) =>
                    {
                        if (group.CommSource == CommisionSource.Manual || group.SoftCommSource == CommisionSource.Manual ||
                            group.CommissionSource == (int)CommisionSource.Manual ||
                            group.SoftCommissionSource == (int)CommisionSource.Manual)
                        {
                            isAnyCommissionSourceManual = true;
                            state.Break();
                        }
                    });

                    if (isAnyCommissionSourceManual)
                    {
                        bool isRecalculate = false;
                        if (pref.MsgOnAllocation)
                        {
                            MessageBoxResult choice = MessageBox.Show("For some groups commission is specified manually, Would you like to calculate commission again for these groups?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                            isRecalculate = choice == MessageBoxResult.Yes;
                        }
                        else if (pref.RecalculateOnAllocation)
                            isRecalculate = pref.RecalculateOnAllocation;

                        Parallel.ForEach(groups, group =>
                        {
                            group.IsRecalculateCommission = isRecalculate;
                        });
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_allocationGridControlViewModel != null)
                    {
                        _allocationGridControlViewModel.ActiveAllocatedGroupEvent -= _allocationGridControlViewModel_ActiveAllocatedGroup;
                        _allocationGridControlViewModel.ActiveUnAllocatedGroupEvent -= _allocationGridControlViewModel_ActiveUnAllocatedGroup;
                        _allocationGridControlViewModel.HeaderCheckboxChangeUnAllocatedGroupEvent -= _allocationGridControlViewModel_HeaderCheckboxChangeUnAllocatedGroupEvent;
                        _allocationGridControlViewModel.GroupUngroupEvent -= _allocationGridControlViewModel_GroupUngroupAllocationGroups;
                        _allocationGridControlViewModel.UnallocateGroupsEvent -= _allocationGridControlViewModel_UnallocateGroup;
                        _allocationGridControlViewModel.SaveLayoutEvent -= _allocationGridControlViewModel_SaveLayout;
                        _allocationGridControlViewModel.LoadSymbolLookUpUI -= _allocationGridControlViewModel_loadSymbolLookUp;
                        _allocationGridControlViewModel.LoadAuditTrailUI -= _allocationGridControlViewModel_GetAuditClick;
                        _allocationGridControlViewModel.LoadCloseTradeUI -= _allocationGridControlViewModel_LoadCloseTradeUI;
                        _allocationGridControlViewModel.LoadCashTransactionUI -= _allocationGridControlViewModel_LoadCashTransactionUI;
                        _allocationGridControlViewModel.DeleteAllocationGroupEvent -= _allocationGridControlViewModel_DeleteAllocationGroup;
                        _allocationGridControlViewModel.UpdateTotalNoOfTrades -= _allocationGridControlViewModel_UpdateTotalNoOfTrades;
                        _allocationGridControlViewModel.SetAccountStrategyControlEvent -= _allocationGridControlViewModel_SetAccountStrategyControlEvent;
                        _allocationGridControlViewModel.Dispose();
                    }
                    if (_commissionFeesBulkChangeControlViewModel != null)
                    {
                        _commissionFeesBulkChangeControlViewModel.BulkChangeGroup -= _commissionFeesBulkChangeControlViewModel_BulkChangeGroup;
                        _commissionFeesBulkChangeControlViewModel.ReCalculateCommission -= _commissionFeesBulkChangeControlViewModel_ReCalculateCommission;
                    }
                    if (_tradeAttributeBulkChangeControlViewModel != null)
                    {
                        _tradeAttributeBulkChangeControlViewModel.UpdateGroupsLevel -= _tradeAttributeBulkChangeControlViewModel_UpdateGroupsLevel;
                    }
                    if (_allocationSwapControlViewModel != null)
                        _allocationSwapControlViewModel.SwapUpdateClick -= _allocationSwapControlViewModel_SwapUpdateClick;

                    if (_allocationOTCControlViewModel != null)
                    {
                        _allocationOTCControlViewModel.SaveOTCClickedEvent -= _allocationOTCControlViewModel_SaveBtnClick;
                        _allocationOTCControlViewModel.Dispose();
                    }


                    if (_allocationFiltersControlViewModel != null)
                        _allocationFiltersControlViewModel.ApplyFilterEvent -= _allocationFiltersControlViewModel_ApplyFilterEvent;

                    if (_accountStrategyControlViewModel != null)
                    {
                        _accountStrategyControlViewModel.AllocateGroup -= _allocationGridControlViewModel_AllocateGroup;
                        _accountStrategyControlViewModel.PreviewAllocationGroupDataEvent -= PreviewAllocationGroupData;
                        _accountStrategyControlViewModel.FixedAllocationPreferenceEvent -= _accountStrategyControlViewModel_FixedAllocationPreferenceEvent;
                        _accountStrategyControlViewModel.Dispose();
                    }

                    if (_editTradeControlViewModel != null)
                    {
                        _editTradeControlViewModel.ApplyEditTradeChangesEvent -= _editTradeControlViewModel_ApplyEditTradeChangesEvent;
                    }

                    if (_currentSymbolStateControlViewModel != null)
                    {
                        _currentSymbolStateControlViewModel.UpdateCurrentStateForSymbols -= _currentSymbolStateControlViewModel_UpdateCurrentStateForSymbols;
                    }

                    if (AllocationClientManager.GetInstance() != null)
                    {
                        AllocationClientManager.GetInstance().AllocationPreferencesSaved -= AllocationClientViewModel_AllocationPreferencesSaved;
                        AllocationClientManager.GetInstance().AllocationPreferenceUpdated -= AllocationClientViewModel_AllocationPreferenceUpdated;
                        AllocationClientManager.GetInstance().UpdateAllocationData -= AllocationClientViewModel_UpdateAllocationData;
                        AllocationClientManager.GetInstance().NewGroupReceived -= AllocationClientViewModel_NewGroupReceived;
                        AllocationClientManager.GetInstance().UpdateStateOfSymbol -= AllocationClientViewModel_UpdateStateOfSymbol;
                        AllocationClientManager.GetInstance().ActionAfterSave -= AllocationClientViewModel_ActionAfterSave;
                        AllocationClientManager.GetInstance().UpdateStatusBar -= AllocationClientViewModel_UpdateStatusBar;
                        AllocationClientManager.GetInstance().AllocationDataChanged -= AllocationClientViewModel_AllocationDataChanged;
                        AllocationClientManager.GetInstance().AllocationSchemeUpdated -= AllocationClientViewModel_AllocationSchemeUpdated;
                        AllocationClientManager.GetInstance().AllocationDataChange -= AllocationClientViewModel_AllocationDataChange;
                        AllocationClientManager.GetInstance().AllocationDataSaved -= AllocationClientViewModel_AllocationDataSaved;
                        AllocationClientManager.GetInstance().ServerProxyConnectedDisconnectedEvent -= AllocationClientViewModel_ServerProxyConnectDisconnectEvent;
                        AllocationClientManager.GetInstance().GroupChangedAtServerSide -= AllocationClientViewModel_GroupChangedAtServerSide;
                        // This is singleton class so no need to dispose it' Also causing memory leak issues
                        // AllocationClientManager.GetInstance().Dispose();
                    }
                    if (_editAllocPrefUI != null)
                    {
                        _editAllocPrefUI.OnFormCloseButtonEvent -= editAllocPrefUI_OnFormCloseButtonEvent;
                        _editAllocPrefUI.UnsubscribeProxyEvent -= _editAllocPrefUI_UnsubscribeProxyEvent;
                        _editAllocPrefUI.Dispose();
                        _editAllocPrefUI = null;
                    }
                    _allocPrefUI = null;
                    _frmExternalTransaction = null;
                    if (_prorataUI != null)
                    {
                        _prorataUI.OnFormCloseButtonEvent -= prorataUI_OnFormCloseButtonEvent;
                        _prorataUI = null;
                    }
                    if (_commissionFeesBulkChangeControlViewModel != null)
                        _commissionFeesBulkChangeControlViewModel.Dispose();

                    if (AllocationDataChange != null)
                        AllocationDataChange(this, new EventArgs<bool>(false));

                }
                InstanceManager.ReleaseInstance(typeof(AllocationClientViewModel));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _allocationOTCControlViewModel_SwapUpdateClick(object sender, EventArgs<SwapParameters> e)
        {
            try
            {
                AllocationGroup group = _allocationGridControlViewModel.GetCurrentActiveGroup();
                if (group != null)
                {
                    Dictionary<string, PostTradeEnums.Status> statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(new List<AllocationGroup> { group });
                    PostTradeEnums.Status groupStatus = statusDictionary[group.GroupID];

                    if (groupStatus.Equals(PostTradeEnums.Status.None))
                    {
                        if (e.Value != null && e.Value.DayCount > 0 && e.Value.OrigCostBasis > 0 && DateTime.Parse(e.Value.FirstResetDate.ToString()) >= DateTime.Parse(e.Value.OrigTransDate.ToString()))
                        {
                            group.SwapParameters = e.Value;
                            group.SwapParameters.GroupID = group.GroupID;
                            if (!group.IsSwapped)
                            {
                                group.IsSwapped = true;
                                group.AddTradeAction(TradeAuditActionType.ActionType.BookAsSwap);
                                group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.BookAsSwap);
                            }
                            else
                            {
                                group.AddTradeAction(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                                group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SwapDetailsUpdated);
                            }
                            AllocationClientManager.GetInstance().DictUnsavedAdd(group.GroupID, group);
                        }
                        else
                            return;
                        MessageBoxResult choice = MessageBox.Show("Would you like to calculate commission and fee again?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (choice == MessageBoxResult.Yes)
                        {
                            group.IsRecalculateCommission = true;
                            AllocationGroup allocationGroup = AllocationClientManager.GetInstance().CalculateCommission(group);
                            if (allocationGroup != null)
                            {
                                group.UpdateCommissionAndFees(allocationGroup);
                                group.UpdateCommissionAndFeesAtTaxlotLevel(allocationGroup);
                            }
                        }
                        if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                        {
                            group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                        }
                        if (group.TaxLots != null && group.TaxLots.Count > 0)
                        {
                            foreach (TaxLot taxlotVar in group.TaxLots)
                            {
                                //taxlotVar.Ass = true;
                                taxlotVar.SwapParameters = group.SwapParameters.Clone();
                                taxlotVar.SwapParameters.NotionalValue = (taxlotVar.TaxLotQty * group.SwapParameters.NotionalValue) / group.CumQty;
                                taxlotVar.ISSwap = true;
                                group.UpdateTaxlotState(taxlotVar);
                            }
                        }
                        else
                        {
                            TaxLot updatedTaxlot = AllocationClientManager.GetInstance().CreateUnAllocTaxlot(group);
                            updatedTaxlot.ISSwap = true;
                            updatedTaxlot.SwapParameters = group.SwapParameters;
                            group.UpdateTaxlotState(updatedTaxlot);
                        }

                        group.PropertyHasChanged();
                        ToggleUIElements("Swap information updated", true);
                    }
                    else
                    {
                        if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                            MessageBox.Show("Group is Partially or Fully Closed. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                            MessageBox.Show(" Corporate Action has been applied on this Group. Can't be booked as Swap", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the editAllocPrefUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        static void editAllocPrefUI_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_editAllocPrefUI != null)
                {
                    _editAllocPrefUI.OnFormCloseButtonEvent -= editAllocPrefUI_OnFormCloseButtonEvent;
                    _editAllocPrefUI.UnsubscribeProxyEvent -= _editAllocPrefUI_UnsubscribeProxyEvent;
                    _editAllocPrefUI = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Edits the grid button click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object EditGridButtonClick(object parameter)
        {
            try
            {
                if (IsEditMode == true)
                    _allocationGridControlViewModel.AllowEditGrid = true;
                else
                    _allocationGridControlViewModel.AllowEditGrid = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Enable/Disable Dock Pane of Allocation according to permission
        /// </summary>
        private void EnableDisableDockPane()
        {
            try
            {
                if (!AllocationPermissions.EditTradeModulePermission)
                    IsEditTradeDockPaneVisible = Visibility.Collapsed;

                if (!AllocationPermissions.AllocationModulePermission)
                    IsAllocationDockPaneVisible = Visibility.Collapsed;

                if (_isNewOTCWorkflow)
                {
                    IsSwapDockPaneVisible = Visibility.Collapsed;
                    IsOTCDockPaneVisible = Visibility.Visible;
                }
                else
                {
                    IsSwapDockPaneVisible = Visibility.Visible;
                    IsOTCDockPaneVisible = Visibility.Collapsed;
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
        /// Exports the allocation data click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private object ExportAllocationDataClick(object parameter)
        {
            try
            {
                _allocationGridControlViewModel.ExportBothGrids = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Gets the allocation data.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object GetAllocationData(object parameter)
        {
            try
            {
                if (_isDirtyData || PromptForDataSaving(ActionAfterSavingData.GetData) != MessageBoxResult.Yes)
                {
                    LoadDataAndClearGroupId();
                    _isDirtyData = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Gets the allocation preference.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private AllocationOperationPreference GetAllocationPreference(int key)
        {
            try
            {
                return AllocationClientManager.GetInstance().GetAllocationPreference(key);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return null;
            }
        }

        /// <summary>
        /// Gets the trade attributes collection.
        /// </summary>
        /// <param name="tradeAttributes">The trade attributes.</param>
        /// <returns></returns>
        private ObservableCollection<string>[] GetTradeAttributesCollection(List<string>[] tradeAttributes)
        {
            ObservableCollection<string>[] tradeAttributesCollection = new ObservableCollection<string>[45];
            try
            {
                if (tradeAttributes != null)
                {
                    tradeAttributesCollection[0] = tradeAttributes[0] != null ? new ObservableCollection<string>(tradeAttributes[0]) : new ObservableCollection<string>();
                    tradeAttributesCollection[1] = tradeAttributes[1] != null ? new ObservableCollection<string>(tradeAttributes[1]) : new ObservableCollection<string>();
                    tradeAttributesCollection[2] = tradeAttributes[2] != null ? new ObservableCollection<string>(tradeAttributes[2]) : new ObservableCollection<string>();
                    tradeAttributesCollection[3] = tradeAttributes[3] != null ? new ObservableCollection<string>(tradeAttributes[3]) : new ObservableCollection<string>();
                    tradeAttributesCollection[4] = tradeAttributes[4] != null ? new ObservableCollection<string>(tradeAttributes[4]) : new ObservableCollection<string>();
                    tradeAttributesCollection[5] = tradeAttributes[5] != null ? new ObservableCollection<string>(tradeAttributes[5]) : new ObservableCollection<string>();

                    for (int i = 6; i < 45; i++)
                    {
                        tradeAttributesCollection[i] = tradeAttributes[i] != null ? new ObservableCollection<string>(tradeAttributes[i]) : new ObservableCollection<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tradeAttributesCollection;
        }

        /// <summary>
        /// Gets the trade attributes list collection.
        /// </summary>
        /// <param name="tradeAttributes">The trade attributes.</param>
        /// <returns></returns>
        private List<string>[] GetTradeAttributesListCollection(ObservableCollection<string>[] tradeAttributes)
        {
            List<string>[] tradeAttributesCollection = new List<string>[45];
            try
            {
                if (tradeAttributes != null)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        tradeAttributesCollection[i] = tradeAttributes[i] != null ? tradeAttributes[i].Where(x => !string.IsNullOrEmpty(x)).ToList() : new List<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tradeAttributesCollection;
        }

        /// <summary>
        /// Loads the allocation client view data.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadAllocationClientViewData(object parameter)
        {
            try
            {
                LoadPermissions();
                EnableDisableDockPane();
                IsCurrentRBChecked = true;

                //Initialize Cache and Lock in Preference Manager
                AllocationClientPreferenceManager.GetInstance.InitializeCache();

                // Load data of all controls
                LoadControlsData();

                // Binding Events
                BindEvents();

                //Get and apply allocation preferences
                var temp = new Dictionary<int, string>();
                temp.Add(int.MinValue, "-Select-");
                var allocationPrefList = AllocationClientManager.GetInstance().GetMasterFundPreferenceList();
                foreach (var item in allocationPrefList)
                {
                    temp.Add(item.Key, item.Value);
                }
                _allocationMasterFundPreferences = temp;
                _allocationUserWisePref = AllocationClientManager.GetInstance().GetUserWisePreferences();
                _allocationOperationPreferences = AllocationClientPreferenceManager.GetInstance.GetFilteredPreferencesList();
                SetPreferences();

                //Set default preferences which need to be set once
                _allocationGridControlViewModel.QtyPrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(15);
                _editTradeControlViewModel.QtyPrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(15);
                IsEditMode = false;

                //set Allocation UI height and width
                FormHeight = (_allocationUserWisePref.AllocationFormHeight > 0) ? _allocationUserWisePref.AllocationFormHeight : AllocationUIConstants.DEFAULT_HEIGHT;
                FormWidth = (_allocationUserWisePref.AllocationFormWidth > 0) ? _allocationUserWisePref.AllocationFormWidth : AllocationUIConstants.DEFAULT_WIDTH;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the controls data.
        /// </summary>
        private void LoadControlsData()
        {
            try
            {
                //LoadAllocationData();
                TradeAttributes = GetTradeAttributesCollection(AllocationClientManager.GetInstance().GetAttributeList());
                CustomValueEnteredActions[] custValueforTradeAttributes = TradeAttributesCache.KeepRecords.Select(x => x ? CustomValueEnteredActions.Add : CustomValueEnteredActions.Allow).ToArray();
                GenericBindingList<AllocationGroup> allocatedGroups = AllocationClientManager.GetInstance().GetAllocatedGroups();
                GenericBindingList<AllocationGroup> unallocatedGroups = AllocationClientManager.GetInstance().GetUnallocatedGroups();
                AllocationGridControlViewModel.OnLoadGrid(TradeAttributes, allocatedGroups, unallocatedGroups, custValueforTradeAttributes);

                //Load Allocation Filter Data
                _allocationFiltersControlViewModel.LoadAllocationFilterData();

                // Loads the state of the current symbol.
                _currentSymbolStateControlViewModel.OnLoadCurrentSymbolStateControl();

                //Load Data of AccountStrategy Control
                Dictionary<int, string> preferenceList = new Dictionary<int, string>();
                preferenceList.Add(int.MinValue, "-Select-");
                Dictionary<int, string> temp = AllocationClientPreferenceManager.GetInstance.UpdateSorting(AllocationClientPreferenceManager.GetInstance.GetMasterFundPreferenceList());
                foreach (var item in temp)
                {
                    preferenceList.Add(item.Key, item.Value);
                }
                _accountStrategyControlViewModel.LoadDataAccountStrategyControl(AllocationClientPreferenceManager.GetInstance.GetFilteredPreferencesList(), AllocationClientPreferenceManager.GetInstance.UpdateSorting(AllocationClientPreferenceManager.GetInstance.GetFixedPreferencesList()), preferenceList, AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences());

                //Load Edit Trade control 
                _editTradeControlViewModel.OnLoadEditTradeControl(TradeAttributes, custValueforTradeAttributes);

                //Load OTC Control or Swap control
                if (!_isNewOTCWorkflow)
                    _allocationSwapControlViewModel.OnLoadSwapControl();

                //Load Commission Fees Bulk Change control
                Dictionary<int, string> accountPBDetails = AllocationClientManager.GetInstance().GetAccountPBDetails();
                _commissionFeesBulkChangeControlViewModel.OnLoadCommissionFeesBulkChangeControl(accountPBDetails);

                //Load Trade Attribute Bulk Change control
                _tradeAttributeBulkChangeControlViewModel.OnLoadTradeAttributeBulkChangeControl(TradeAttributes, accountPBDetails, custValueforTradeAttributes);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Binds the events.
        /// </summary>
        private void BindEvents()
        {
            try
            {
                _allocationGridControlViewModel.ActiveAllocatedGroupEvent += _allocationGridControlViewModel_ActiveAllocatedGroup;
                _allocationGridControlViewModel.ActiveUnAllocatedGroupEvent += _allocationGridControlViewModel_ActiveUnAllocatedGroup;
                _allocationGridControlViewModel.HeaderCheckboxChangeUnAllocatedGroupEvent += _allocationGridControlViewModel_HeaderCheckboxChangeUnAllocatedGroupEvent;
                _allocationGridControlViewModel.GroupUngroupEvent += _allocationGridControlViewModel_GroupUngroupAllocationGroups;
                _allocationGridControlViewModel.UnallocateGroupsEvent += _allocationGridControlViewModel_UnallocateGroup;
                _allocationGridControlViewModel.SaveLayoutEvent += _allocationGridControlViewModel_SaveLayout;
                _allocationGridControlViewModel.UpdateTotalNoOfTrades += _allocationGridControlViewModel_UpdateTotalNoOfTrades;
                _allocationGridControlViewModel.LoadSymbolLookUpUI += _allocationGridControlViewModel_loadSymbolLookUp;
                _allocationGridControlViewModel.LoadAuditTrailUI += _allocationGridControlViewModel_GetAuditClick;
                _allocationGridControlViewModel.LoadCloseTradeUI += _allocationGridControlViewModel_LoadCloseTradeUI;
                _allocationGridControlViewModel.LoadCashTransactionUI += _allocationGridControlViewModel_LoadCashTransactionUI;
                _allocationGridControlViewModel.DeleteAllocationGroupEvent += _allocationGridControlViewModel_DeleteAllocationGroup;
                _allocationGridControlViewModel.OpenExternalTransactionUI += _allocationGridControlViewModel_OpenExternalTransactionUI;
                _allocationGridControlViewModel.SetAccountStrategyControlEvent += _allocationGridControlViewModel_SetAccountStrategyControlEvent;
                _accountStrategyControlViewModel.AllocateGroup += _allocationGridControlViewModel_AllocateGroup;
                _accountStrategyControlViewModel.PreviewAllocationGroupDataEvent += new EventHandler(PreviewAllocationGroupData);
                _accountStrategyControlViewModel.UpdateStatusAllocation += _accountStrategyControlViewModel_UpdateStatusAllocation;
                _accountStrategyControlViewModel.FixedAllocationPreferenceEvent += _accountStrategyControlViewModel_FixedAllocationPreferenceEvent;
                _allocationSwapControlViewModel.SwapUpdateClick += _allocationSwapControlViewModel_SwapUpdateClick;
                _allocationOTCControlViewModel.SaveOTCClickedEvent += _allocationOTCControlViewModel_SaveBtnClick;
                _allocationFiltersControlViewModel.ApplyFilterEvent += _allocationFiltersControlViewModel_ApplyFilterEvent;
                _editTradeControlViewModel.ApplyEditTradeChangesEvent += _editTradeControlViewModel_ApplyEditTradeChangesEvent;
                _currentSymbolStateControlViewModel.UpdateCurrentStateForSymbols += _currentSymbolStateControlViewModel_UpdateCurrentStateForSymbols;
                _commissionFeesBulkChangeControlViewModel.BulkChangeGroup += _commissionFeesBulkChangeControlViewModel_BulkChangeGroup;
                _commissionFeesBulkChangeControlViewModel.ReCalculateCommission += _commissionFeesBulkChangeControlViewModel_ReCalculateCommission;
                _tradeAttributeBulkChangeControlViewModel.UpdateGroupsLevel += _tradeAttributeBulkChangeControlViewModel_UpdateGroupsLevel;
                _tradeAttributeBulkChangeControlViewModel.OnGetAllocationGroups += GetSelectedGroups;
                AllocationClientManager.GetInstance().AllocationPreferencesSaved += AllocationClientViewModel_AllocationPreferencesSaved;
                AllocationClientManager.GetInstance().AllocationPreferenceUpdated += AllocationClientViewModel_AllocationPreferenceUpdated;
                AllocationClientManager.GetInstance().UpdateAllocationData += AllocationClientViewModel_UpdateAllocationData;
                AllocationClientManager.GetInstance().NewGroupReceived += AllocationClientViewModel_NewGroupReceived;
                AllocationClientManager.GetInstance().UpdateStateOfSymbol += AllocationClientViewModel_UpdateStateOfSymbol;
                AllocationClientManager.GetInstance().UpdateStatusBar += AllocationClientViewModel_UpdateStatusBar;
                AllocationClientManager.GetInstance().ActionAfterSave += AllocationClientViewModel_ActionAfterSave;
                AllocationClientManager.GetInstance().AllocationSchemeUpdated += AllocationClientViewModel_AllocationSchemeUpdated;
                AllocationClientManager.GetInstance().AllocationDataChange += AllocationClientViewModel_AllocationDataChange;
                AllocationClientManager.GetInstance().AllocationDataSaved += AllocationClientViewModel_AllocationDataSaved;
                AllocationClientManager.GetInstance().AllocationDataChanged += AllocationClientViewModel_AllocationDataChanged;
                AllocationClientManager.GetInstance().ServerProxyConnectedDisconnectedEvent += AllocationClientViewModel_ServerProxyConnectDisconnectEvent;
                AllocationClientManager.GetInstance().GroupChangedAtServerSide += AllocationClientViewModel_GroupChangedAtServerSide;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the allocation data.
        /// </summary>
        private void LoadAllocationData()
        {
            try
            {
                if (SelectedToDate.Date >= SelectedFromDate.Date)
                {
                    string toAllAUECDatesString = string.Empty;
                    string fromAllAUECDatesString = string.Empty;
                    string fromAllocatedAllAUECDatesString = string.Empty;

                    if (_isCurrentRBChecked)
                    {
                        toAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                        DateTime fromDate = DateTime.UtcNow.Date.AddDays(-1 * Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation)));
                        fromAllAUECDatesString = fromDate.AddDays(1).ToString();
                        fromAllocatedAllAUECDatesString = DateTime.UtcNow.Date.ToString();
                    }
                    else
                    {
                        toAllAUECDatesString = _selectedToDate.Date.ToString();
                        fromAllAUECDatesString = _selectedFromDate.Date.ToString();
                        fromAllocatedAllAUECDatesString = _selectedFromDate.Date.ToString();
                    }

                    AllocationPrefetchFilter filterList = AllocationFiltersControlViewModel.GetFilterList();

                    if (filterList.Allocated.ContainsKey(AllocationClientConstants.FROM_DATE))
                        filterList.Allocated.Remove(AllocationClientConstants.FROM_DATE);

                    filterList.Allocated.Add(AllocationClientConstants.FROM_DATE, fromAllocatedAllAUECDatesString);

                    AllocationClientManager.GetInstance().ClearData();
                    ToggleUIElements("Getting Data...", false);
                    AllocationClientManager.GetInstance().GetAllocationData(toAllAUECDatesString, fromAllAUECDatesString, filterList);


                    if (_isApplyFilterClicked && filterList.Allocated.ContainsKey(AllocationUIConstants.ACCOUNT_ID_COLUMN))
                    {
                        _isApplyFilterClicked = false;
                        //Load Data of AccountStrategy Control
                        string accountsString = filterList.Allocated[AllocationUIConstants.ACCOUNT_ID_COLUMN];
                        var accountList = accountsString.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        _accountStrategyControlViewModel.LoadDataAccountStrategyControl(accountList);
                    }


                }
                else
                    MessageBox.Show("To date cannot be less than From date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Loads the closing wizard UI.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadClosingWizardUI(object parameter)
        {
            try
            {
                AllocationClientManager.GetInstance().LaunchClosingWizard();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the edit alloc preference UI click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadEditAllocPrefUIClick(object parameter)
        {
            try
            {
                if (_editAllocPrefUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowEditPreferenceControl(viewModel => dialogService.Show<EditAllocationPreferencesUI>(this, viewModel));
                }
                else
                    _editAllocPrefUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Edit Allocation Preference Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the permissions.
        /// </summary>
        private void LoadPermissions()
        {
            try
            {
                if (AllocationPermissions.CheckModulePermissionAllocation(PranaModules.ALLOCATION_EDIT_TRADE_MODULE))
                    AllocationPermissions.EditTradeModulePermission = true;

                if (AllocationPermissions.CheckModulePermissionAllocation(PranaModules.ALLOCATION_MODULE))
                    AllocationPermissions.AllocationModulePermission = true;

                if (AllocationPermissions.CheckModulePermissionAllocation(PranaModules.PERCENT_TRADING_TOOL))
                    AllocationPermissions.PTTCheckBoxPermission = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the preference UI click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadPrefUIClick(object parameter)
        {
            try
            {
                if (_allocPrefUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowAllocationPreferencesUIViewModel(viewModel => dialogService.Show<AllocationPreferencesUI>(this, viewModel));
                }
                else
                    _allocPrefUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Allocation Preference Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the prorata UI click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadProrataUIClick(object parameter)
        {
            try
            {
                if (_prorataUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowCalculateProrataUI(viewModel => dialogService.Show<CalculateProrataUI>(this, viewModel));
                }
                else
                    _prorataUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Prorata Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Previews the allocation grid data.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        private void PreviewAllocationGridData(AllocationGroup allocationGroup)
        {
            try
            {
                AllocationGroup group = allocationGroup;
                bool isPTTAllocationSelected = _accountStrategyControlViewModel.IsCustomCheckBoxChecked;
                KeyValuePair<int, string> prefrenceSelected = _accountStrategyControlViewModel.SelectedAllocationPreferences.Value;
                bool isForceAllocationSelected = _accountStrategyControlViewModel.IsForceAllocationSelected ?? false;
                AllocationOperationPreference pref;
                if (isPTTAllocationSelected && group.OriginalAllocationPreferenceID > 0)
                {
                    pref = AllocationClientManager.GetInstance().GetPTTAllocationPreference(group.OriginalAllocationPreferenceID);
                }
                else
                {
                    pref = GetAllocationPreference(prefrenceSelected.Key);
                }
                _accountStrategyControlViewModel.SetQuantity(group.CumQty);
                bool isChanged = false;
                if (prefrenceSelected.Key == Int32.MinValue && !(group.OriginalAllocationPreferenceID > 0 && isPTTAllocationSelected))
                {
                    Dictionary<int, AccountValue> targetpercentage = _accountStrategyControlViewModel.AccountAndStrategyGridControlViewModel.GetTargetPercantage();
                    AllocationRule rule = _accountStrategyControlViewModel.GetDefault();

                    pref = new AllocationOperationPreference();
                    pref.TryUpdateDefaultRule(rule);
                    foreach (int key in targetpercentage.Keys)
                    {
                        pref.TryUpdateTargetPercentage(targetpercentage[key]);
                    }
                    isChanged = true;
                }

                if (pref != null)
                {
                    if ((pref.DefaultRule.RuleType == MatchingRuleType.Prorata || pref.TargetPercentage.Count > 0) && pref.IsValid())
                    {
                        group.ErrorMessage = string.Empty;
                        List<AllocationGroup> groupList = new List<AllocationGroup>();
                        groupList.Add(allocationGroup);
                        AllocationResponse response = AllocationClientManager.GetInstance().PreviewAllocationData(groupList, pref, isForceAllocationSelected, isChanged);
                        List<AllocationGroup> allGroup = response.GroupList;
                        if (!string.IsNullOrWhiteSpace(response.Response))
                        {
                            ToggleUIElements(response.Response.Contains("\n") ? response.Response.Substring(0, response.Response.IndexOf("\n")) : response.Response, true);
                            // MessageBox.Show(this, response.Response, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            ToggleUIElements("Data allocated by account.", true);
                        if (allGroup != null && allGroup.Count > 0)
                        {
                            if (allGroup[0].ErrorMessage.Equals(string.Empty))
                                _accountStrategyControlViewModel.SetAllocationAccounts(allGroup[0]);
                        }
                        else
                            _accountStrategyControlViewModel.SetAllocationAccounts(group);
                    }
                    else
                    {
                        _accountStrategyControlViewModel.SetValues(pref.TargetPercentage);
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
        /// Previews the allocation group data while preference changed from Account strategy control
        /// </summary>
        /// <param name="sender">The Account strategy view model.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PreviewAllocationGroupData(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup allocationGroup = null;
                if (_allocationGridControlViewModel.ActiveUnallocatedDataItem is AllocationGroup)
                    allocationGroup = (AllocationGroup)_allocationGridControlViewModel.ActiveUnallocatedDataItem;
                if (allocationGroup != null)
                    PreviewAllocationGridData(allocationGroup);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Prompts for data saving.
        /// </summary>
        /// <param name="actionAfterSaving">The action after saving.</param>
        /// <returns>
        /// None -> If data saving not required
        /// Yes ->If user saves data (with/without status) or clicks exit
        /// No -> If user does not save data
        /// </returns>
        internal MessageBoxResult PromptForDataSaving(ActionAfterSavingData actionAfterSaving, string message = AllocationClientConstants.SAVE_ALLOCATION_DATA_WARNING)
        {
            MessageBoxResult isSaveData = MessageBoxResult.None;
            try
            {
                if (AllocationClientManager.GetInstance().AnythingChanged())
                {
                    if (IsSaveWithStateEnabled && IsSaveWithoutStateEnabled)
                    {
                        var userChoice = MsgBox.Show(message, "Warning!", MsgBox.Buttons.YesNoCancel, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "Save (w/o Status)", "No");
                        if (userChoice.ToString().Equals(MessageBoxResult.Yes.ToString()))
                        {
                            ToggleUIElements("Saving data and saving State.", false);
                            SaveDataAsync(actionAfterSaving, true);
                            isSaveData = MessageBoxResult.Yes;
                        }
                        else if (userChoice.ToString().Equals(MessageBoxResult.No.ToString()))
                        {
                            ToggleUIElements("Saving data without saving State.", false);
                            SaveDataAsync(actionAfterSaving, false);
                            isSaveData = MessageBoxResult.Yes;
                        }
                        else if (userChoice.ToString().Equals(MessageBoxResult.None.ToString()))
                            isSaveData = MessageBoxResult.Yes;
                        else
                            isSaveData = MessageBoxResult.No;
                    }
                    else if (IsSaveWithStateEnabled)
                    {
                        var userChoice = MsgBox.Show(message, "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "No");
                        if (userChoice.ToString().Equals(MessageBoxResult.Yes.ToString()))
                        {
                            ToggleUIElements("Saving data and saving State.", false);
                            SaveDataAsync(actionAfterSaving, true);
                            isSaveData = MessageBoxResult.Yes;
                        }
                        else if (userChoice.ToString().Equals(MessageBoxResult.None.ToString()))
                            isSaveData = MessageBoxResult.Yes;
                        else
                            isSaveData = MessageBoxResult.No;
                    }
                    else if (IsSaveWithoutStateEnabled)
                    {
                        var userChoice = MsgBox.Show(message, "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/o Status)", "No");
                        if (userChoice.ToString().Equals(MessageBoxResult.Yes.ToString()))
                        {
                            ToggleUIElements("Saving data without saving State.", false);
                            SaveDataAsync(actionAfterSaving, false);
                            isSaveData = MessageBoxResult.Yes;
                        }
                        else if (userChoice.ToString().Equals(MessageBoxResult.None.ToString()))
                            isSaveData = MessageBoxResult.Yes;
                        else
                            isSaveData = MessageBoxResult.No;
                    }
                    else if (!IsSaveWithoutStateEnabled && !IsSaveWithStateEnabled)
                    {
                        var userChoice = Prana.Utilities.UI.MiscUtilities.MsgBox.Show(message, "Warning!", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn, "Save (w/Status)", "No");
                        if (userChoice.ToString().Equals(MessageBoxResult.Yes.ToString()))
                        {
                            ToggleUIElements("Saving data and saving State.", false);
                            SaveDataAsync(actionAfterSaving, true);
                            isSaveData = MessageBoxResult.Yes;
                        }
                        else if (userChoice.ToString().Equals(MessageBoxResult.None.ToString()))
                            isSaveData = MessageBoxResult.Yes;
                        else
                            isSaveData = MessageBoxResult.No;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSaveData;
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the prorataUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        static void prorataUI_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_prorataUI != null)
                {
                    _prorataUI.OnFormCloseButtonEvent -= prorataUI_OnFormCloseButtonEvent;
                    _prorataUI = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Saves the allocation data.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object SaveAllocationData(object parameter)
        {
            try
            {
                bool isSaveState = parameter.ToString().Equals("SaveWStatus") ? true : false;
                ActionAfterSavingData saveDataAction = ActionAfterSavingData.DoNothing;
                SaveDataAsync(saveDataAction, isSaveState);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="saveDataAction">The save data action.</param>
        /// <param name="isSaveState">if set to <c>true</c> [is save state].</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void SaveDataAsync(ActionAfterSavingData saveDataAction, bool isSaveState)
        {
            try
            {
                _allocationGridControlViewModel.EndEditModeAndCommitChanges = true;
                ToggleUIElements("Saving Data. Please wait...", false);
                AllocationClientManager.GetInstance().SaveDataAsync(saveDataAction, isSaveState);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Screens the shot click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ScreenShotClick(object parameter)
        {
            try
            {
                Window allocationClientWindow = parameter as Window;
                SnapshotHelper.ClickSnapShot(allocationClientWindow);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Sets the allocation client preferences.
        /// </summary>
        private void SetAllocationClientPreferences()
        {
            try
            {
                //set save button visibility
                IsSaveWithStateEnabled = _allocationUserWisePref.GeneralRules.IncludeSavewtState;
                IsSaveWithoutStateEnabled = _allocationUserWisePref.GeneralRules.IncludeSavewtoutState;
                if (!_allocationUserWisePref.GeneralRules.IncludeSavewtState && !_allocationUserWisePref.GeneralRules.IncludeSavewtoutState)
                    IsSaveWithStateEnabled = true;
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
        private void SetPreferences()
        {
            try
            {
                AllocationCompanyWisePref pref = AllocationClientManager.GetInstance().GetCompanyWisePreferences();
                _accountStrategyControlViewModel.SetPreferences(pref, _allocationOperationPreferences, _allocationUserWisePref.GeneralRules, _allocationMasterFundPreferences);
                _allocationGridControlViewModel.SetPreferences(pref);
                _editTradeControlViewModel.SetPreferences(pref.PrecisionDigit);
                _allocationSwapControlViewModel.SetPreferences(pref.PrecisionDigit);
                _commissionFeesBulkChangeControlViewModel.SetPreferences(pref.PrecisionDigit);
                _currentSymbolStateControlViewModel.SetPreferences(pref.PrecisionDigit);
                _tradeAttributeBulkChangeControlViewModel.SetPreferences();
                SetAllocationClientPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Shows the allocation preferences UI view model.
        /// </summary>
        /// <param name="showAllocationPreferencesUI">The show allocation preferences UI.</param>
        private static void ShowAllocationPreferencesUIViewModel(Action<AllocationPreferencesUIViewModel> showAllocationPreferencesUI)
        {
            try
            {
                _allocPrefUI = new AllocationPreferencesUIViewModel();
                _allocPrefUI.OnFormCloseButtonEvent += allocPrefUI_OnFormCloseButtonEvent;
                showAllocationPreferencesUI(_allocPrefUI);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates Status bar and show message after allocation
        /// </summary>
        /// <param name="responseObjectList">The response object list.</param>
        /// <returns></returns>
        private string ShowAllocationResponse(AllocationResponse allocationResponse)
        {
            string result = String.Empty;
            try
            {
                if (allocationResponse == null)
                {
                    result = "Something went wrong, please contact administrator.";
                    MessageBox.Show(result, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (allocationResponse.GroupList != null && allocationResponse.GroupList.Count == 0)
                    {
                        result = "Data is not allocated.";
                        FormatandDisplayResponse(result, allocationResponse.Response);
                    }
                    else
                    {
                        result = string.IsNullOrWhiteSpace(allocationResponse.Response) ? "Data allocation is completed" : "Data is not allocated for some groups. ";

                        if (!string.IsNullOrWhiteSpace(allocationResponse.Response))
                        {
                            FormatandDisplayResponse(result, allocationResponse.Response);
                        }
                        else
                        {
                            if (_allocationUserWisePref != null)
                            {
                                if (_allocationUserWisePref.GeneralRules.ClearAllocationFundControlNumber)
                                    AccountStrategyControlViewModel.ClearGrid();
                                if (_allocationUserWisePref.GeneralRules.AllocationMethodologyRevertToDefault)
                                    _accountStrategyControlViewModel.SetAllocationMethodologyToDefault(_allocationUserWisePref.GeneralRules.AllocationPrefType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Something went wrong, please contact administrator.";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Formats the response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="response">The response.</param>
        private void FormatandDisplayResponse(string result, string response)
        {
            try
            {
                var index = response.Select((c, i) => new { c, i }).Where(x => x.c == '\n').Skip(7).FirstOrDefault();
                if (index != null)
                {
                    StringBuilder boxMessage = new StringBuilder();
                    boxMessage.AppendLine(result);
                    string message = AllocationClientManager.GetInstance().WriteResponseToFile(response.Replace("\n", System.Environment.NewLine), @"\Logs\AllocationLog.txt");
                    if (message == UnAllocationCompletionStatus.FileWriteError.ToString())
                    {
                        boxMessage.Append("While writing the details in the file, some issue occured. Please fetch the data and Allocate the trades again.");
                        MessageBox.Show(boxMessage.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        boxMessage.Append(response.Substring(0, index.i) + "\n...\n\n");
                        boxMessage.Append("Do you want to view complete details?");
                        MessageBoxResult dr = MessageBox.Show(boxMessage.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (dr == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start(message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(result + "\n" + response, AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Shows the calculate prorata UI.
        /// </summary>
        /// <param name="showCalculateProrataUIViewModel">The show calculate prorata UI view model.</param>
        private static void ShowCalculateProrataUI(Action<CalculateProrataUIViewModel> showCalculateProrataUIViewModel)
        {
            try
            {
                _prorataUI = new CalculateProrataUIViewModel();
                _prorataUI.SetPreferences(AllocationClientManager.GetInstance().GetCompanyWisePreferences());
                _prorataUI.OnFormCloseButtonEvent += prorataUI_OnFormCloseButtonEvent;
                showCalculateProrataUIViewModel(_prorataUI);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Shows the edit preference control.
        /// </summary>
        /// <param name="showEditPreferenceControl">The show edit preference control.</param>
        private static void ShowEditPreferenceControl(Action<EditAllocationPreferencesUIViewModel> showEditPreferenceControl)
        {
            try
            {
                _editAllocPrefUI = new EditAllocationPreferencesUIViewModel();
                _editAllocPrefUI.WireEvents();
                _editAllocPrefUI.OnFormCloseButtonEvent += editAllocPrefUI_OnFormCloseButtonEvent;
                _editAllocPrefUI.UnsubscribeProxyEvent += _editAllocPrefUI_UnsubscribeProxyEvent;
                showEditPreferenceControl(_editAllocPrefUI);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UnsubscribeProxyEvent event of the _editAllocPrefUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void _editAllocPrefUI_UnsubscribeProxyEvent(object sender, EventArgs e)
        {
            try
            {
                if (AllocationClientManager.GetInstance() != null)
                    AllocationClientManager.GetInstance().UnSubscribeProxy();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Shows the add and update external transaction UI.
        /// </summary>
        /// <param name="showExternalTransactionUIViewModel">The show add and update external transaction identifier UI view model.</param>
        private void ShowExternalTransactionUi(Action<ExternalTransactionIdViewModel> showExternalTransactionUIViewModel)
        {
            try
            {
                if (_frmExternalTransaction == null)
                {
                    _frmExternalTransaction = new ExternalTransactionIdViewModel();
                    _frmExternalTransaction.UpdateExternalTransactionID += _frmExternalTransactionId_UpdateExternalTransactionID;
                    _frmExternalTransaction.CloseExternalTransaction += _frmExternalTransactionId_CloseExternalTransaction;
                }

                if (_allocationGridControlViewModel.ActiveAllocatedDataItem is TaxLot)
                {
                    _selectedTaxlotForExternalTransUpdate = (TaxLot)_allocationGridControlViewModel.ActiveAllocatedDataItem;
                    _frmExternalTransaction.ExternalTransactionTable = AllocationClientManager.GetInstance().GetTaxlotDetails(_selectedTaxlotForExternalTransUpdate.TaxLotID);
                }
                showExternalTransactionUIViewModel(_frmExternalTransaction);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Toggles the UI elements.
        /// </summary>
        /// <param name="statusBarMessage">The status bar message.</param>
        /// <param name="isEnableUIElements">if set to <c>true</c> [is enable UI elements].</param>
        private void ToggleUIElements(string statusBarMessage, bool isEnableUIElements)
        {
            try
            {
                StatusBarText = statusBarMessage;
                EnableDisableUiElements = isEnableUIElements;
                EnableDisableGetDataButton = isEnableUIElements;
                EnableDisableToolBar = isEnableUIElements;
                if (IsHistoricalRBChecked)
                {
                    IsFromDatePickerEnabled = isEnableUIElements;
                    IsToDatePickerEnabled = isEnableUIElements;
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
        /// Ungroups the groups.
        /// </summary>
        /// <param name="unallocatedGroups">The unallocated groups.</param>
        private void UngroupGroups(List<AllocationGroup> unallocatedGroups)
        {
            try
            {
                if (unallocatedGroups != null && unallocatedGroups.Count > 0)
                {
                    _allocationGridControlViewModel.IsClearUnAllocatedSelectedItems = true;
                    ToggleUIElements("UnGrouping selected data. Please wait..", false);
                    AllocationClientManager.GetInstance().ApplyUnGrouping(unallocatedGroups);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Ungroups the groups.
        /// </summary>
        /// <param name="allocatedGroups">The unallocated groups.</param>
        private void UngroupGroupsAllocated(List<AllocationGroup> allocatedGroups)
        {
            try
            {
                if (allocatedGroups != null && allocatedGroups.Count > 0)
                {
                    _allocationGridControlViewModel.IsClearAllocatedSelectedItems = true;
                    ToggleUIElements("UnGrouping selected data. Please wait..", false);
                    AllocationClientManager.GetInstance().ApplyUnGrouping(allocatedGroups);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the attribute lists.
        /// </summary>
        /// <param name="updatedGroupList">The updated group list.</param>
        private void UpdateAttributeLists(List<AllocationGroup> updatedGroupList)
        {
            try
            {
                if (updatedGroupList == null || updatedGroupList.Count == 0)
                    return;
                Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                {
                    TradeAttributes = Prana.Global.Utilities.DeepCopyHelper.Clone<ObservableCollection<string>[]>(_allocationGridControlViewModel.TradeAttributesCollection);
                    CustomValueEnteredActions[] keepRec = _allocationGridControlViewModel.TradeAttributesKeepRecords;
                    foreach (AllocationGroup group in updatedGroupList)
                    {
                        if (keepRec[0] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute1) && !TradeAttributes[0].Contains(group.TradeAttribute1))
                            TradeAttributes[0].Add(group.TradeAttribute1);
                        if (keepRec[1] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute2) && !TradeAttributes[1].Contains(group.TradeAttribute2))
                            TradeAttributes[1].Add(group.TradeAttribute2);
                        if (keepRec[2] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute3) && !TradeAttributes[2].Contains(group.TradeAttribute3))
                            TradeAttributes[2].Add(group.TradeAttribute3);
                        if (keepRec[3] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute4) && !TradeAttributes[3].Contains(group.TradeAttribute4))
                            TradeAttributes[3].Add(group.TradeAttribute4);
                        if (keepRec[4] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute5) && !TradeAttributes[4].Contains(group.TradeAttribute5))
                            TradeAttributes[4].Add(group.TradeAttribute5);
                        if (keepRec[5] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(group.TradeAttribute6) && !TradeAttributes[5].Contains(group.TradeAttribute6))
                            TradeAttributes[5].Add(group.TradeAttribute6);
                        List<string> tradeAttributes = group.GetTradeAttributesAsList();
                        for (int i = 0; i < tradeAttributes.Count; i++)
                        {
                            string value = tradeAttributes[i];
                            int index = 6 + i;
                            if (keepRec[index] == CustomValueEnteredActions.Add && !string.IsNullOrWhiteSpace(value) && !TradeAttributes[index].Contains(value))
                            {
                                TradeAttributes[index].Add(value);
                            }
                        }
                    }
                    _allocationGridControlViewModel.TradeAttributesCollection = TradeAttributes;
                    _editTradeControlViewModel.TradeAttributesCollection = TradeAttributes;
                    _tradeAttributeBulkChangeControlViewModel.TradeAttributesCollection = TradeAttributes;
                }));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the FixedAllocationPreferenceEvent event of the _accountStrategyControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _accountStrategyControlViewModel_FixedAllocationPreferenceEvent(object sender, EventArgs e)
        {
            try
            {
                //To check which item is activated, Allocated/ Unallocated
                bool isAllocateSelected = _allocationGridControlViewModel.ActiveAllocatedDataItem != null ? true : false;

                //If Fixed radio button is selected, and user select pref2 from combo. And then select Calculated radio button and again select Fixed radio button then First preference should selected always.
                _accountStrategyControlViewModel.SelectedfixedAllocationPreference = _accountStrategyControlViewModel.FixedAllocationPreferences.FirstOrDefault();

                //If Allocated trade is selected then preview of that Trade will come otherwise grid will clear.
                if (isAllocateSelected)
                {
                    AllocationGroup group = _allocationGridControlViewModel.GetCurrentActiveGroup();
                    _accountStrategyControlViewModel.SetQuantity(group.CumQty);
                    _accountStrategyControlViewModel.SetAllocationAccounts(group);
                }
                else
                    _accountStrategyControlViewModel.ClearGridOnly();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Loads the account strategy grid for selected group.
        /// </summary>
        private void LoadAccountStrategyGridForSelectedGroup()
        {
            try
            {
                if (_isPinnedAccountStrategyGrid)
                {
                    AllocationGroup group = _allocationGridControlViewModel.GetCurrentActiveGroup();
                    if (group != null)
                    {
                        if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            if (_accountStrategyControlViewModel.IsCalculatedAllocationSelected)
                                PreviewAllocationGridData(group);
                            else
                                _accountStrategyControlViewModel.ClearGridOnly();
                        }
                        else
                        {
                            _accountStrategyControlViewModel.SetQuantity(group.CumQty);
                            _accountStrategyControlViewModel.SetAllocationAccounts(group);
                        }
                    }
                }
                else
                {
                    _accountStrategyControlViewModel.ClearGridOnly();
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
        /// Apply Filter And GetData
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="tradeDates"></param>
        /// <param name="loadData"></param>
        internal void ApplyFilterAndGetData(string groupIds, DateTime fromDate, DateTime toDate, bool loadData)
        {
            try
            {
                IsHistoricalRBChecked = true;
                IsFromDatePickerEnabled = true;
                SelectedFromDate = fromDate;
                SelectedToDate = toDate;

                AllocationFiltersControlViewModel.ClearFilter();
                _allocationGridControlViewModel.ClearGridFilters(AllocationClientConstants.ALLOCATED_GRID);
                _allocationGridControlViewModel.ClearGridFilters(AllocationClientConstants.UNALLOCATED_GRID);
                AllocationFiltersControlViewModel.AllFilterControl.GroupId = groupIds;

                if (loadData)
                    LoadAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the data and clear group identifier.
        /// </summary>
        private void LoadDataAndClearGroupId()
        {
            try
            {
                AllocationFiltersControlViewModel.AllFilterControl.GroupId = string.Empty;
                LoadAllocationData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [allocation client deactivated].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnAllocationClientDeactivated(object parameter)
        {
            try
            {
                _allocationGridControlViewModel.EndEditModeAndCommitChanges = true;
                _accountStrategyControlViewModel.AccountAndStrategyGridControlViewModel.EndEditModeGrid = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (gridName == "GridUnallocated")
            {
                if (_allocationGridControlViewModel.ExportDataGridUnallocated == true)
                    _allocationGridControlViewModel.ExportDataGridUnallocated = false;
                _allocationGridControlViewModel.ExportFilePathForAutomation = filePath;
                _allocationGridControlViewModel.ExportDataGridUnallocated = true;
            }
            else if (gridName == "GridAllocated")
            {
                if (_allocationGridControlViewModel.ExportDataGridAllocated == true)
                    _allocationGridControlViewModel.ExportDataGridAllocated = false;
                _allocationGridControlViewModel.ExportFilePathForAutomation = filePath;
                _allocationGridControlViewModel.ExportDataGridAllocated = true;
            }
            else if (WindowName == "UpdatePreferenceWindow")
            {
                if (_accountStrategyControlViewModel.AccountAndStrategyGridControlViewModel.ExportAccountAndStrategyGrid == true)
                    _accountStrategyControlViewModel.AccountAndStrategyGridControlViewModel.ExportAccountAndStrategyGrid = false;
                _accountStrategyControlViewModel.AccountAndStrategyGridControlViewModel.ExportAccountAndStrategyGrid = true;
            }
            else
            {
                if (gridName == "MasterFundRatioDataGrid")
                {
                    if (_editAllocPrefUI.MasterFundRatioControlViewModel.ExportGrid == true)
                        _editAllocPrefUI.MasterFundRatioControlViewModel.ExportGrid = false;
                    _editAllocPrefUI.MasterFundRatioControlViewModel.ExportFilePathForAutomation = filePath;
                    _editAllocPrefUI.MasterFundRatioControlViewModel.ExportGrid = true;
                }
                else if (gridName == "listViewAllocationPref")
                {
                    if (_editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.ExportGrid == true)
                        _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.ExportGrid = false;
                    _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.ExportFilePathForAutomation = filePath;
                    _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.ExportGrid = true;
                }
                else if (gridName == "GeneralRuleDataGrid")
                {
                    if (_editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferenceGeneralRuleControlViewModel.ExportGrid == true)
                        _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferenceGeneralRuleControlViewModel.ExportGrid = false;
                    _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferenceGeneralRuleControlViewModel.ExportFilePathForAutomation = filePath;
                    _editAllocPrefUI.CalculatedAllocationPreferenceControlViewModel.CalculatedPreferenceGeneralRuleControlViewModel.ExportGrid = true;
                }
            }
        }

        #endregion Methods


    }
}
