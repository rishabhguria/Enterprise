using Prana.Allocation.Client.Controls.Allocation.Views;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    /// <summary>
    /// AccountStrategyControlViewModel
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class AccountStrategyControlViewModel : ViewModelBase, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when [allocate group].
        /// </summary>
        public event EventHandler<EventArgs> AllocateGroup;

        /// <summary>
        /// Occurs when [fixed allocation preference event].
        /// </summary>
        public event EventHandler FixedAllocationPreferenceEvent;

        /// <summary>
        /// Occurs when [preview allocation group data event].
        /// </summary>
        public event EventHandler PreviewAllocationGroupDataEvent;

        /// <summary>
        /// Occurs when [update status allocation].
        /// </summary>
        public event EventHandler<EventArgs<string>> UpdateStatusAllocation;

        #endregion Events

        #region Members

        /// <summary>
        /// The _allocation preference collection
        /// </summary>
        private ObservableDictionary<int, string> _allocationPreferenceCollection;

        /// <summary>
        /// The _selected allocation preferences
        /// </summary>
        private KeyValuePair<int, string>? _selectedAllocationPreferences;

        /// <summary>
        /// The _fixed allocation preferences
        /// </summary>
        private ObservableDictionary<int, string> _fixedAllocationPreferences;

        /// <summary>
        /// The _selectedfixed allocation preference
        /// </summary>
        private KeyValuePair<int, string>? _selectedfixedAllocationPreference;

        /// <summary>
        /// The _is fixed allocation selected
        /// </summary>
        private bool _isFixedAllocationSelected;

        /// <summary>
        /// The _is calculated allocation selected
        /// </summary>
        private bool _isCalculatedAllocationSelected;

        /// <summary>
        /// The _is force allocation selected
        /// </summary>
        private bool? _isForceAllocationSelected;

        /// <summary>
        /// The _is force allocation enabled
        /// </summary>
        private bool _isForceAllocationEnabled;

        /// <summary>
        /// The _selected group
        /// </summary>
        private string _selectedGroup;

        /// <summary>
        /// The _is edit preferences button enabled
        /// </summary>
        private bool _isEditPreferencesButtonEnabled = true;

        /// <summary>
        /// The enable control
        /// </summary>
        private bool _enableControl;

        /// <summary>
        /// is control on unallocated grid
        /// </summary>
        private bool _isControlOnUnallocatedGrid = true;

        /// <summary>
        /// The _is Ptt allocation selected
        /// </summary>
        private bool _isCustomCheckBoxChecked;

        /// <summary>
        /// The temporary Data Table
        /// </summary>
        DataTable dtTemp = new DataTable();

        /// <summary>
        /// The _edit preference control view model
        /// </summary>
        private EditPreferenceControlViewModel _editPreferenceControlViewModel;

        /// <summary>
        /// The _account and strategy grid control view model
        /// </summary>
        private AccountAndStrategyGridControlViewModel _accountAndStrategyGridControlViewModel;

        /// <summary>
        /// The edit preference control
        /// </summary>
        private static EditPreferenceControlViewModel editPrefControl = null;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account and strategy grid control view model.
        /// </summary>
        /// <value>
        /// The account and strategy grid control view model.
        /// </value>
        public AccountAndStrategyGridControlViewModel AccountAndStrategyGridControlViewModel
        {
            get { return _accountAndStrategyGridControlViewModel; }
            set
            {
                _accountAndStrategyGridControlViewModel = value;
                RaisePropertyChangedEvent("AccountAndStrategyGridControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the allocation preference collection.
        /// </summary>
        /// <value>
        /// The allocation preference collection.
        /// </value>
        public ObservableDictionary<int, string> AllocationPreferenceCollection
        {
            get { return _allocationPreferenceCollection; }
            set
            {
                _allocationPreferenceCollection = value;
                RaisePropertyChangedEvent("AllocationPreferenceCollection");
            }
        }

        /// <summary>
        /// Gets or sets the edit preference control view model.
        /// </summary>
        /// <value>
        /// The edit preference control view model.
        /// </value>
        public EditPreferenceControlViewModel EditPreferenceControlViewModel
        {
            get { return _editPreferenceControlViewModel; }
            set
            {
                _editPreferenceControlViewModel = value;
                RaisePropertyChangedEvent("EditPreferenceControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the fixed allocation preferences.
        /// </summary>
        /// <value>
        /// The fixed allocation preferences.
        /// </value>
        public ObservableDictionary<int, string> FixedAllocationPreferences
        {
            get { return _fixedAllocationPreferences; }
            set
            {
                _fixedAllocationPreferences = value;
                RaisePropertyChangedEvent("FixedAllocationPreferences");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is calculated allocation selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is calculated allocation selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsCalculatedAllocationSelected
        {
            get { return _isCalculatedAllocationSelected; }
            set
            {
                _isCalculatedAllocationSelected = value;
                if (_isCalculatedAllocationSelected)
                    PreviewAllocationGroupData();
                else
                    SelectedAllocationPreferences = new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                IsEditPreferencesButtonEnabled = (IsControlOnUnallocatedGrid && _isCalculatedAllocationSelected && SelectedAllocationPreferences.Value.Key < 0);
                AccountAndStrategyGridControlViewModel.IsGridEnable = _isCalculatedAllocationSelected;
                RaisePropertyChangedEvent("IsCalculatedAllocationSelected");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is edit preferences button enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is edit preferences button enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditPreferencesButtonEnabled
        {
            get { return _isEditPreferencesButtonEnabled; }
            set
            {
                _isEditPreferencesButtonEnabled = value;
                RaisePropertyChangedEvent("IsEditPreferencesButtonEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable control].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable control]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableControl
        {
            get { return _enableControl; }
            set
            {
                if (_enableControl != value)
                {
                    _enableControl = value;
                    RaisePropertyChangedEvent("EnableControl");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is control on unallocated grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is control on unallocated grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsControlOnUnallocatedGrid
        {
            get { return _isControlOnUnallocatedGrid; }
            set
            {
                _isControlOnUnallocatedGrid = value;
                RaisePropertyChangedEvent("IsControlOnUnallocatedGrid");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed allocation selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fixed allocation selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixedAllocationSelected
        {
            get { return _isFixedAllocationSelected; }
            set
            {
                _isFixedAllocationSelected = value;
                if (_isFixedAllocationSelected)
                    FixedAllocationPreferenceSelected();
                RaisePropertyChangedEvent("IsFixedAllocationSelected");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is force allocation enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is force allocation enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsForceAllocationEnabled
        {
            get { return _isForceAllocationEnabled; }
            set
            {
                _isForceAllocationEnabled = value;
                RaisePropertyChangedEvent("IsForceAllocationEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is force allocation selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is force allocation selected; otherwise, <c>false</c>.
        /// </value>
        public bool? IsForceAllocationSelected
        {
            get { return _isForceAllocationSelected; }
            set
            {
                _isForceAllocationSelected = value;
                RaisePropertyChangedEvent("IsForceAllocationSelected");
            }
        }

        /// <summary>
        /// The is Ptt allocation selected
        /// </summary>
        public bool IsCustomCheckBoxChecked
        {
            get { return _isCustomCheckBoxChecked; }
            set
            {
                _isCustomCheckBoxChecked = value;
                PreviewAllocationGroupData();
                RaisePropertyChangedEvent("IsCustomCheckBoxChecked");
            }
        }

        /// <summary>
        /// Gets or sets the selected allocation preferences.
        /// </summary>
        /// <value>
        /// The selected allocation preferences.
        /// </value>
        public KeyValuePair<int, string>? SelectedAllocationPreferences
        {
            get 
            {
                if (_selectedAllocationPreferences == null)
                {
                    _selectedAllocationPreferences = new KeyValuePair<int, string>(0, null);
                }
                return _selectedAllocationPreferences; 
            }
            set
            {
                _selectedAllocationPreferences = value;
                if (IsCalculatedAllocationSelected)
                    PreviewAllocationGroupData();
                if (_selectedAllocationPreferences.Value.Value != Prana.Global.ApplicationConstants.C_COMBO_SELECT)
                {
                    _accountAndStrategyGridControlViewModel.ISPrefSelected = true;
                    _accountAndStrategyGridControlViewModel.IsPrefChanged = true;
                }

                //Refresh sorting in Account strategy grid
                _accountAndStrategyGridControlViewModel.RefreshSorting();
                IsEditPreferencesButtonEnabled = IsControlOnUnallocatedGrid && IsCalculatedAllocationSelected && _selectedAllocationPreferences.Value.Key < 0;

                RaisePropertyChangedEvent("SelectedAllocationPreferences");
            }
        }

        /// <summary>
        /// Gets or sets the selectedfixed allocation preference.
        /// </summary>
        /// <value>
        /// The selectedfixed allocation preference.
        /// </value>
        public KeyValuePair<int, string>? SelectedfixedAllocationPreference
        {
            get 
            {
                if (_selectedfixedAllocationPreference == null)
                {
                    _selectedfixedAllocationPreference = new KeyValuePair<int, string>(0, null);
                }
                return _selectedfixedAllocationPreference; 
            }
            set
            {
                _selectedfixedAllocationPreference = value;
                RaisePropertyChangedEvent("SelectedfixedAllocationPreference");
            }
        }

        /// <summary>
        /// Gets or sets the selected group.
        /// </summary>
        /// <value>
        /// The selected group.
        /// </value>
        public string SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                RaisePropertyChangedEvent("SelectedGroup");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the account strategy grid command.
        /// </summary>
        /// <value>
        /// The account strategy grid command.
        /// </value>
        public RelayCommand<object> AccountStrategyGridCommand { get; set; }

        /// <summary>
        /// Gets or sets the load edit preference UI.
        /// </summary>
        /// <value>
        /// The load edit preference UI.
        /// </value>
        public RelayCommand<object> LoadEditPrefUI { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountStrategyControlViewModel"/> class.
        /// </summary>
        public AccountStrategyControlViewModel()
        {
            try
            {
                AccountStrategyGridCommand = new RelayCommand<object>((parameter) => AccountStrategyGridCommandExecuted(parameter));
                LoadEditPrefUI = new RelayCommand<object>((parameter) => LoadEditPrefUIClick(parameter));
                _accountAndStrategyGridControlViewModel = new AccountAndStrategyGridControlViewModel();

                EditPreferenceControlViewModel.GetInstance().UpdateStatusAllocation += AccountStrategyControlViewModel_UpdateStatusAllocation;
                EditPreferenceControlViewModel.GetInstance().PreviewAllocationData += AccountStrategyControlViewModel_PreviewAllocationData;
                EditPreferenceControlViewModel.GetInstance().CloseEditPrefUI += AccountStrategyControlViewModel_CloseEditPrefUI;
                _accountAndStrategyGridControlViewModel.PrefCmbValueChange += _accountAndStrategyGridControlViewModel_PrefCmbValueChange;
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
        /// Handles the PrefCmbValueChange event of the _accountAndStrategyGridControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _accountAndStrategyGridControlViewModel_PrefCmbValueChange(object sender, EventArgs e)
        {
            try
            {
                if (SelectedAllocationPreferences.Value.Value != Prana.Global.ApplicationConstants.C_COMBO_SELECT)
                {
                    SelectedAllocationPreferences = new KeyValuePair<int, string>(int.MinValue, Prana.Global.ApplicationConstants.C_COMBO_SELECT);
                    _accountAndStrategyGridControlViewModel.ISPrefSelected = false;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CloseEditPrefUI event of the AccountStrategyControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AccountStrategyControlViewModel_CloseEditPrefUI(object sender, EventArgs e)
        {
            try
            {
                if (editPrefControl != null)
                    editPrefControl = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PreviewAllocationData event of the AccountStrategyControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void AccountStrategyControlViewModel_PreviewAllocationData(object sender, EventArgs e)
        {
            try
            {
                PreviewAllocationGroupData();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UpdateStatusAllocation event of the AccountStrategyControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        void AccountStrategyControlViewModel_UpdateStatusAllocation(object sender, EventArgs<string> e)
        {
            try
            {
                if (UpdateStatusAllocation != null)
                    UpdateStatusAllocation(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Accounts the strategy grid command executed.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object AccountStrategyGridCommandExecuted(object parameter)
        {
            try
            {
                switch (parameter.ToString())
                {
                    case "btnAllocate":
                        AllocateGroups();
                        break;
                    case "btnClick":
                        ClearGrid();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Allocates the groups.
        /// </summary>
        internal void AllocateGroups()
        {
            try
            {
                if (AllocateGroup != null)
                    AllocateGroup(this, new EventArgs());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the grid.
        /// </summary>
        internal void ClearGrid()
        {
            try
            {
                _accountAndStrategyGridControlViewModel.ClearGrid();
                // To clear the allocation Pref selected in combobox for calculated, PRANA-15291
                if (SelectedAllocationPreferences.Value.Value != Prana.Global.ApplicationConstants.C_COMBO_SELECT)
                    SelectedAllocationPreferences = new KeyValuePair<int, string>(int.MinValue, Prana.Global.ApplicationConstants.C_COMBO_SELECT);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the grid only.
        /// </summary>
        internal void ClearGridOnly()
        {
            try
            {
                _accountAndStrategyGridControlViewModel.ClearGridOnly();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Uns the wire events.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (EditPreferenceControlViewModel.GetInstance() != null)
                    {
                        EditPreferenceControlViewModel.GetInstance().UpdateStatusAllocation -= AccountStrategyControlViewModel_UpdateStatusAllocation;
                        EditPreferenceControlViewModel.GetInstance().PreviewAllocationData -= AccountStrategyControlViewModel_PreviewAllocationData;
                        EditPreferenceControlViewModel.GetInstance().Dispose();
                    }
                    if (dtTemp != null)
                    {
                        dtTemp.Dispose();
                        dtTemp = null;
                    }
                    if (editPrefControl != null)
                        editPrefControl = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Fixeds the allocation preference selected.
        /// </summary>
        private void FixedAllocationPreferenceSelected()
        {
            try
            {
                if (FixedAllocationPreferenceEvent != null)
                    FixedAllocationPreferenceEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Return Allocation Parameters.
        /// </summary>
        /// <returns></returns>
        internal AllocationParameter GetAllocationParameter(bool isReallocate)
        {
            AllocationParameter parameter = null;
            try
            {
                SerializableDictionary<int, AccountValue> targetpercentage = new SerializableDictionary<int, AccountValue>();
                targetpercentage = AccountAndStrategyGridControlViewModel.GetTargetPercantage();
                decimal totalPercentage = 0;
                foreach (KeyValuePair<int, AccountValue> pair in targetpercentage)
                {
                    totalPercentage += targetpercentage[pair.Key].Value;
                }

                if (isReallocate && totalPercentage != 100)
                {
                    targetpercentage = AccountAndStrategyGridControlViewModel.GetOriginalAllocationPercantage();
                    AccountAndStrategyGridControlViewModel.setTargetDictFromOriginalDict(targetpercentage);
                }
                else
                {
                    targetpercentage = AccountAndStrategyGridControlViewModel.GetTargetPercantage();
                }
                AllocationRule rule = GetDefault();

                parameter = new AllocationParameter(rule, targetpercentage, -1, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, true);
            }
            catch (Exception ex)
            {
                parameter = null;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return parameter;
        }

        /// <summary>
        /// Returns Allocation Type
        /// </summary>
        /// <returns></returns>
        internal AllocationPreferenceType GetAllocationType()
        {
            //TODO: AllocationPreferenceType should have a default value
            return IsFixedAllocationSelected ? AllocationPreferenceType.AllocationBySymbol : AllocationPreferenceType.AllocationByAccount;
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <returns></returns>
        internal AllocationRule GetDefault()
        {
            try
            {

                return EditPreferenceControlViewModel.GetInstance().GetAllocationRule();
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
        /// Loads the data account strategy control.
        /// </summary>
        internal void LoadDataAccountStrategyControl(Dictionary<int, string> operationPrefList, Dictionary<int, string> fixedPrefList, Dictionary<int, string> masterFundPrefList, AllocationCompanyWisePref companyWisePref)
        {
            try
            {
                if (companyWisePref.EnableMasterFundAllocation && !companyWisePref.IsOneSymbolOneMasterFundAllocation)
                    AllocationPreferenceCollection = new ObservableDictionary<int, string>(masterFundPrefList);
                else
                    AllocationPreferenceCollection = new ObservableDictionary<int, string>(operationPrefList);
                FixedAllocationPreferences = new ObservableDictionary<int, string>(fixedPrefList);
                _accountAndStrategyGridControlViewModel.LoadAccountStrategyGridData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Load Data Account Strategy Control
        /// </summary>
        /// <param name="accountList"></param>
        internal void LoadDataAccountStrategyControl(List<int> accountList)
        {
            _accountAndStrategyGridControlViewModel.AccountFilterList = accountList;
            _accountAndStrategyGridControlViewModel.LoadAccountStrategyGridData();
        }

        /// <summary>
        /// Loads the edit preference UI click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadEditPrefUIClick(object parameter)
        {
            try
            {
                if (editPrefControl == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowEditPrefUI(viewModel => dialogService.Show<EditPreferenceControl>(this, viewModel));
                }
                else
                    editPrefControl.BringToFront = WindowState.Normal;
                //MessageBox.Show("Edit Preference Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Selecteds the allocation preferences changed.
        /// </summary>
        private void PreviewAllocationGroupData()
        {
            try
            {
                if (PreviewAllocationGroupDataEvent != null)
                    PreviewAllocationGroupDataEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Refreshes the sorting.
        /// </summary>
        internal void RefreshSorting()
        {
            try
            {
                _accountAndStrategyGridControlViewModel.IsSortByPercentage = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets the allocation accounts.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        public void SetAllocationAccounts(AllocationGroup allocationGroup)
        {
            try
            {
                SerializableDictionary<int, AccountValue> targetDict = CommonAllocationMethods.GetAllocationDistributionDict(allocationGroup);
                SetValues(targetDict);
                // AccountAndStrategyGridControlViewModel.TargetDic1 = targetDict;
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
        /// Sets the allocation methodology to default AllocationPreferenceType.
        /// </summary>
        internal void SetAllocationMethodologyToDefault(AllocationPreferenceType allocationPreferenceType)
        {
            try
            {
                switch (allocationPreferenceType)
                {
                    case AllocationPreferenceType.AllocationByAccount:
                        IsCalculatedAllocationSelected = true;
                        break;
                    case AllocationPreferenceType.AllocationBySymbol:
                        IsFixedAllocationSelected = true;
                        break;
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
        /// Sets the fixed preference data.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        internal void SetFixedPreferenceData(Dictionary<int, string> fixedPreferenceListDictionary)
        {
            try
            {
                KeyValuePair<int, string> selectedPreference = SelectedfixedAllocationPreference.Value;
                FixedAllocationPreferences = new ObservableDictionary<int, string>(fixedPreferenceListDictionary);
                if (!selectedPreference.Equals(new KeyValuePair<int, string>()) && FixedAllocationPreferences.ContainsKey(selectedPreference.Key))
                    SelectedfixedAllocationPreference = selectedPreference;

                if (FixedAllocationPreferences.Count <= 0)
                    SelectedfixedAllocationPreference = new KeyValuePair<int, string>();
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
        /// <param name="companyWisePref">The _allocation company wise preference.</param>
        /// <param name="allocationOperationPreferences"></param>
        /// <param name="allocationPreferenceType">This is default allocation preference type .</param>
        internal void SetPreferences(AllocationCompanyWisePref companyWisePref, Dictionary<int, string> allocationOperationPreferences, GeneralRules generalRules, Dictionary<int, string> allocationMasterFundPreferences)
        {
            try
            {
                IsForceAllocationEnabled = true;
                if (companyWisePref.AllocationCheckSidePref != null)
                {
                    // set force allocation button visibility
                    IsForceAllocationEnabled = companyWisePref.AllocationCheckSidePref.DoCheckSideSystem;

                    // set force allocation button checked or unchecked by default
                    if (companyWisePref.AllocationCheckSidePref.DoCheckSideSystem)
                    {
                        if (companyWisePref.AllocationCheckSidePref.DisableCheckSidePref.Count() == 0)
                        {
                            IsForceAllocationSelected = false;
                        }
                        else
                        {
                            // If all Accounts or all assets classes or all counter party selected then it will never check for sides. IsForceAllocationSelected = true;
                            var disableCheckSidePref = companyWisePref.AllocationCheckSidePref.DisableCheckSidePref;

                            if (disableCheckSidePref.ContainsKey(OrderFilterLevels.Account) && disableCheckSidePref[OrderFilterLevels.Account].Count == CommonDataCache.CachedDataManager.GetInstance.GetAccounts().Count)
                                IsForceAllocationSelected = true;

                            if (disableCheckSidePref.ContainsKey(OrderFilterLevels.Asset) && disableCheckSidePref[OrderFilterLevels.Asset].Count == CommonDataCache.CachedDataManager.GetInstance.GetAllAssets().Count)
                                IsForceAllocationSelected = true;

                            if (disableCheckSidePref.ContainsKey(OrderFilterLevels.CounterParty) && disableCheckSidePref[OrderFilterLevels.CounterParty].Count == CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties().Count)
                                IsForceAllocationSelected = true;
                        }
                    }
                    else
                    {
                        IsForceAllocationSelected = companyWisePref.AllocationCheckSidePref.DoCheckSideSystem;
                    }
                }

                //set Edit Preferences default rule
                EditPreferenceControlViewModel.GetInstance().SetDefaultRules(companyWisePref.DefaultRule);

                //Set updated preferences
                if (companyWisePref.EnableMasterFundAllocation && !companyWisePref.IsOneSymbolOneMasterFundAllocation && allocationMasterFundPreferences != null)
                    AllocationPreferenceCollection = new ObservableDictionary<int, string>(allocationMasterFundPreferences);
                else if (allocationOperationPreferences != null)
                    AllocationPreferenceCollection = new ObservableDictionary<int, string>(allocationOperationPreferences);

                //set precision format
                _accountAndStrategyGridControlViewModel.SetPreferences(companyWisePref.PrecisionDigit, generalRules.KeepAccountsGridFixed);

                switch (generalRules.AllocationPrefType)
                {
                    case AllocationPreferenceType.AllocationByAccount:
                        IsCalculatedAllocationSelected = true;
                        break;
                    case AllocationPreferenceType.AllocationBySymbol:
                        IsFixedAllocationSelected = true;
                        break;
                    default:
                        IsCalculatedAllocationSelected = true;
                        break;
                }
                IsCustomCheckBoxChecked = generalRules.IsAllocationByPST;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the quantity.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        internal void SetQuantity(double quantity)
        {
            try
            {
                _accountAndStrategyGridControlViewModel.EndEditModeGrid = true;
                _accountAndStrategyGridControlViewModel.UpdateQuantity(quantity);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="targetDictionary">The target dictionary.</param>
        /// <param name="level">The level.</param>
        internal void SetValues(SerializableDictionary<int, AccountValue> targetDictionary)
        {
            try
            {
                _accountAndStrategyGridControlViewModel.SetValuesInGrid(targetDictionary);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Shows the edit preference UI.
        /// </summary>
        /// <param name="showEditPrefUIViewModel">The show edit preference UI view model.</param>
        private static void ShowEditPrefUI(Action<EditPreferenceControlViewModel> showEditPrefUIViewModel)
        {
            try
            {
                editPrefControl = EditPreferenceControlViewModel.GetInstance();
                showEditPrefUIViewModel(editPrefControl);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Saves the account strategy grid layout.
        /// </summary>
        internal void SaveAccountStrategyGridLayout()
        {
            try
            {
                AccountAndStrategyGridControlViewModel.SaveAccountStrategyGridLayout();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the total no of trades.
        /// </summary>
        /// <param name="totalTrades">The total trades.</param>
        /// <param name="selectedTrades">The selected trades.</param>
        internal void UpdateTotalNoOfTrades(int selectedTrades, int totalTrades)
        {
            try
            {
                EnableControl = selectedTrades > 0;
                AccountAndStrategyGridControlViewModel.UpdateTotalNoOfTrades(selectedTrades, totalTrades);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the account strategy control.
        /// </summary>
        /// <param name="isUnallocatedGridSelected">if set to <c>true</c> [is unallocated grid selected].</param>
        internal void SetAccountStrategyControl(bool isUnallocatedGridSelected)
        {
            try
            {
                IsControlOnUnallocatedGrid = isUnallocatedGridSelected;
                if (!isUnallocatedGridSelected)
                    SelectedAllocationPreferences = new KeyValuePair<int, string>(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                IsEditPreferencesButtonEnabled = isUnallocatedGridSelected;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
