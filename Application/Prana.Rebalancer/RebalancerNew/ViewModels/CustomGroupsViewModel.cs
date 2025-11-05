using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ServiceConnector;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms.Integration;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class CustomGroupsViewModel : RebalancerBase
    {
        #region Properties

        #region Custom Groups List which includes Custom Groups as well as Master Funds

        private ObservableCollection<KeyValueItemWithFlag> _customGroupsList;

        public ObservableCollection<KeyValueItemWithFlag> CustomGroupsList
        {
            get { return _customGroupsList; }
            set
            {
                _customGroupsList = value;
                OnPropertyChanged("CustomGroupsList");
            }
        }

        #endregion

        #region Accounts List

        private ObservableCollection<KeyValueItemWithFlag> _accountsList;

        public ObservableCollection<KeyValueItemWithFlag> AccountsList
        {
            get { return _accountsList; }
            set
            {
                _accountsList = value;
                OnPropertyChanged("AccountsList");
            }
        }

        #endregion

        #region Selected Custom Group

        private KeyValueItemWithFlag _selectedGroup;

        public KeyValueItemWithFlag SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                SetAvailableAndAssignedList(value);
                OnPropertyChanged("SelectedGroup");
            }
        }

        #endregion

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

        public FormatWindow _formatWindow;

        #region Custom Group Name

        private string _customGroupName;
        public string CustomGroupName
        {
            get { return _customGroupName; }
            set { _customGroupName = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Command To Move Accounts from assigned list to unassigned List
        /// </summary>
        public DelegateCommand<object> MoveToUnassignedCommand { get; set; }

        /// <summary>
        /// Command To Move Accounts from unassigned list to assigned List
        /// </summary>
        public DelegateCommand<object> MoveToAssignedCommand { get; set; }

        /// <summary>
        /// Command to add custom Group
        /// </summary>
        public DelegateCommand<string> AddCustomGroupCommand { get; set; }

        /// <summary>
        /// Command to save the changes in the custom group
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// Command to delete a Custom 
        /// </summary>
        public DelegateCommand DeleteCommand { get; set; }

        /// <summary>
        /// Command to open Format Window
        /// </summary>
        public DelegateCommand OpenFormatCommand { get; set; }

        /// <summary>
        /// Command to Importa and open Import Window
        /// </summary>
        public DelegateCommand ImportCommand { get; set; }

        #endregion

        #region constructor

        /// <inheritdoc />
        public CustomGroupsViewModel(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
            : base(securityMasterInstance, rebalancerHelperInstance)
        {
            OpenFormatCommand = new DelegateCommand(() => OpenFormatCommandAction());
            ImportCommand = new DelegateCommand(() => ImportCommandAction());
            MoveToUnassignedCommand = new DelegateCommand<object>(selectedItems => MoveToUnassignedList(selectedItems));
            SaveCommand = new DelegateCommand(() => SaveCustomGroup());
            MoveToAssignedCommand = new DelegateCommand<object>(selectedItems => MoveToAssignedList(selectedItems));
            AddCustomGroupCommand = new DelegateCommand<string>(customGroup => AddCustomGroup(customGroup));
            DeleteCommand = new DelegateCommand(() => DeleteCustomGroup());
            StatusAndErrorMessages = new StatusAndErrorMessageModel();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to do action on ImportCommand
        /// </summary>
        private void ImportCommandAction()
        {
            try
            {
                IImportFactory importFactoryInstance = new ImportFactory();
                string error = string.Empty;
                IImport importInstance = importFactoryInstance.CreateObject(RebalancerEnums.ImportType.CustomGroupsImport, ref error);
                if (string.IsNullOrWhiteSpace(error))
                {
                    ImportModel model = new ImportModel
                    {
                        CustomGroups = new HashSet<string>(CustomGroupsList.Select(s => s.ItemValue).Distinct()),
                        Accounts = new HashSet<string>(AccountsList.Select(s => s.ItemValue).Distinct())
                    };
                    List<ImportCustomGroupModel> importCustomGroupList = importInstance.ValidateAndGetData<ImportCustomGroupModel>(model);
                    if (importCustomGroupList == null)
                    {
                        ShowErrorAlert("Imported data is of wrong format.");
                        return;
                    }
                    ImportViewModel importViewModelInstance = new ImportViewModel(RebalancerEnums.ImportType.CustomGroupsImport);
                    ImportView importView = new ImportView(RebalancerEnums.ImportType.CustomGroupsImport)
                    {
                        DataContext = importViewModelInstance
                    };
                    importViewModelInstance.SetUp<ImportCustomGroupModel>(importCustomGroupList);
                    importView.Owner = _formatWindow.Owner;
                    importView.ShowInTaskbar = true;
                    importView.ShowDialog();
                    if (importViewModelInstance.IsSaveClicked)
                    {
                        SaveCustomGroupFomImport(importViewModelInstance);
                    }
                    importInstance.DisposeData();
                }
                else
                    ShowErrorAlert(error);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow =
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SaveCustomGroupFomImport(ImportViewModel importViewModelInstance)
        {
            Dictionary<string, CustomGroupDto> customGroupDict = new Dictionary<string, CustomGroupDto>();
            foreach (ImportCustomGroupModel importCustomGroupModel in importViewModelInstance.CustomGroupGrid)
            {
                if (importCustomGroupModel.IsValid && customGroupDict.ContainsKey(importCustomGroupModel.CustomGroupName))
                {
                    customGroupDict[importCustomGroupModel.CustomGroupName].FundGroupMapping.Add(CachedDataManager.GetInstance.GetAccountID(importCustomGroupModel.AccountName));
                }
                else if (importCustomGroupModel.IsValid)
                {
                    CustomGroupDto customGroupDto = new CustomGroupDto();
                    customGroupDto.GroupID = -1;
                    customGroupDto.GroupName = importCustomGroupModel.CustomGroupName;
                    customGroupDto.FundGroupMapping = new List<int>();
                    customGroupDto.FundGroupMapping.Add(CachedDataManager.GetInstance.GetAccountID(importCustomGroupModel.AccountName));
                    customGroupDict.Add(importCustomGroupModel.CustomGroupName, customGroupDto);
                }
            }
            int saveCount = 0;
            foreach (KeyValuePair<string, CustomGroupDto> kvp in customGroupDict)
            {
                bool saved = RebalancerServiceApi.GetInstance()
            .SaveCustomGroupMapping(kvp.Value);
                if (saved)
                    saveCount++;
            }
            if (saveCount == customGroupDict.Count && customGroupDict.Count > 0)
                DialogService.DialogServiceInstance.ShowMessageBox(this, "All Custom Group Saved",
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK, true);
        }

        /// <summary>
        /// Bind Format window and provide owner
        /// </summary>
        /// <param name="rebalancerMainView"></param>
        public void BindFormatWindow(RebalancerMainView rebalancerMainView)
        {
            _formatWindow = new FormatWindow(RebalancerEnums.ImportType.CustomGroupsImport);
            ElementHost.EnableModelessKeyboardInterop(_formatWindow);
            _formatWindow.Owner = rebalancerMainView;
            _formatWindow.Closing += FormatWindow_Closing;
            _formatWindow.ShowInTaskbar = true;
        }

        /// <summary>
        /// Closing of Format Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                _formatWindow.Visibility = Visibility.Hidden;
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
        /// To show Format Window
        /// </summary>
        private void OpenFormatCommandAction()
        {
            try
            {
                if (_formatWindow != null)
                {
                    _formatWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow =
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void DeleteCustomGroup()
        {
            try
            {
                StringBuilder accountName = new StringBuilder();
                bool isAllowedToDelete = RebalancerCache.Instance.IsCanDeleteCustomGroup(SelectedGroup.Key, accountName);
                if (isAllowedToDelete)
                {
                    MessageBoxResult rsltMessageBox = MessageBox.Show(String.Format("Do you really want to delete Custom Group: {0}?", SelectedGroup.ItemValue), RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                                   MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (rsltMessageBox == MessageBoxResult.Yes)
                    {

                        if (SelectedGroup.Key == -1) return;
                        bool deleted = RebalancerServiceApi.GetInstance()
                                .DeleteCustomGroupMapping(SelectedGroup.Key);
                        if (deleted)
                        {
                            DialogService.DialogServiceInstance.ShowMessageBox(this, "Custom Group Deleted",
                                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                                    MessageBoxImage.Information,
                                    MessageBoxResult.OK, true);
                        }
                    }
                }
                else
                    DialogService.DialogServiceInstance.ShowMessageBox(this, "Cannot delete custom group as non permitted accounts (" + accountName.ToString() + ") are mapped.",
                                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                                    MessageBoxImage.Warning,
                                    MessageBoxResult.OK, true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow =
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// method to save custom group
        /// </summary>
        private void SaveCustomGroup()
        {
            try
            {
                if (SelectedGroup == null) return;
                if (string.IsNullOrEmpty(SelectedGroup.ItemValue)) return;
                if (CheckDuplicacy(SelectedGroup.ItemValue, SelectedGroup.Key)) return;
                List<int> assignedFunds = AccountsList.Where(p => p.Flag == true).Select(fund => fund.Key)
                    .ToList();
                //Add non permitted accounts back to the custom group save list.
                assignedFunds.AddRange(RebalancerCache.Instance.GetCustomGroupNonPermittedAccounts(SelectedGroup.Key));
                if (!(assignedFunds.Count > 0))
                {
                    DialogService.DialogServiceInstance.ShowMessageBox(this, "No assigned accounts",
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                        MessageBoxImage.Warning,
                        MessageBoxResult.OK, true);
                    return;
                }
                CustomGroupDto customGroupDto = new CustomGroupDto
                {
                    GroupID = SelectedGroup.Key,
                    GroupName = SelectedGroup.ItemValue,
                    FundGroupMapping = assignedFunds
                };
                bool saved = RebalancerServiceApi.GetInstance()
                    .SaveCustomGroupMapping(customGroupDto);
                if (saved)
                    DialogService.DialogServiceInstance.ShowMessageBox(this, "Custom Group Saved",
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.OK, true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow =
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initialize the View Model
        /// </summary>
        internal void InitializeViewModel()
        {
            try
            {
                SetCustomGroupsList();
                GetValuesForAccountsList();
                if (CustomGroupsList != null) SelectedGroup = CustomGroupsList[0];
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
        /// Get and set values in the Account List
        /// </summary>
        private void GetValuesForAccountsList()
        {
            List<KeyValueItemWithFlag> keyValueItems = CachedDataManager.GetInstance.GetUserAccountsAsDict()
                .Select(p => new KeyValueItemWithFlag { ItemValue = p.Value, Key = p.Key }).OrderBy(x => x.ItemValue).ToList();
            AccountsList = new ObservableCollection<KeyValueItemWithFlag>(keyValueItems);
        }

        /// <summary>
        /// Get ans set Values int the CustomGroupsList
        /// </summary>
        private void SetCustomGroupsList()
        {
            List<KeyValueItemWithFlag> customGroups = RebalancerCache.Instance.GetCustomGroupsDictionary()
                .Select(customFund => new KeyValueItemWithFlag { Key = customFund.Key, ItemValue = customFund.Value, Flag = true }).OrderBy(x => x.ItemValue).ToList();

            List<KeyValueItemWithFlag> masterFunds =
                CachedDataManager.GetInstance.GetUserMasterFunds()
                    .Select(p => new KeyValueItemWithFlag { ItemValue = p.Value, Key = p.Key }).OrderBy(x => x.ItemValue).ToList();

            CustomGroupsList = new ObservableCollection<KeyValueItemWithFlag>(customGroups.Concat(masterFunds));
        }

        /// <summary>
        /// Add the newly added Custom group name to custom group list
        /// </summary>
        /// <param name="customGroup"></param>
        private void AddCustomGroup(string customGroup)
        {
            try
            {
                if (CustomGroupsList != null && !ValidateCustomGroupAddition(customGroup))
                    return;

                if (CustomGroupsList != null)
                {
                    CustomGroupsList.Insert(0,
                        (new KeyValueItemWithFlag { ItemValue = customGroup.Trim(), Key = -1, Flag = true }));
                    CustomGroupName = string.Empty;
                }
                RefreshAccountsList();
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
        /// Validate whether a new custom group can be added or not
        /// </summary>
        /// <returns></returns>
        private bool ValidateCustomGroupAddition(string customGroup)
        {
            if (string.IsNullOrWhiteSpace(customGroup))
            {
                ShowErrorAlert("Custom Group Name can not be empty.");
                return false;
            }
            else if (!ValidateName(customGroup))
            {
                ShowErrorAlert("Custom Group Name is not valid.");
                return false;
            }

            if (CheckDuplicacy(customGroup))
                return false;
            if (CustomGroupsList.Any(group => group.Key == -1))
            {
                DialogService.DialogServiceInstance.ShowMessageBox(this, "First save the already added custom group",
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK, true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check for duplicate custom group name
        /// </summary>
        /// <param name="customGroupName"></param>
        /// <returns></returns>
        private bool CheckDuplicacy(string customGroupName)
        {
            if (CustomGroupsList != null && CustomGroupsList.Where(x => x.Flag == true).Any(group =>
                    group.ItemValue.Replace(" ", String.Empty).ToLowerInvariant().Equals(customGroupName.Replace(" ", String.Empty).ToLowerInvariant())))
            {
                DialogService.DialogServiceInstance.ShowMessageBox(this, "Custom Group with the same name already exists",
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK, true);
                return true;
            }
            return CheckDuplicacyInAccounts(customGroupName);
        }

        /// <summary>
        /// Check for duplicate in accounts
        /// </summary>
        /// <param name="customGroupName"></param>
        /// <returns></returns>
        private bool CheckDuplicacyInAccounts(string customGroupName)
        {
            //if (AccountsList != null &&
            //   AccountsList.Any(account => account.ItemValue.Replace(" ", String.Empty).ToLowerInvariant().Equals(customGroupName.Replace(" ", String.Empty).ToLowerInvariant())))
            //{
            //    DialogService.DialogServiceInstance.ShowMessageBox(this, "Account with the same name already exists",
            //        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
            //        MessageBoxImage.Warning,
            //        MessageBoxResult.OK, true);
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Check for duplicate custom group name when already existing group is renamed
        /// </summary>
        /// <param name="customGroupName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckDuplicacy(string customGroupName, int id)
        {
            bool isDuplicateCustom = CustomGroupsList != null && CustomGroupsList.Any(group => (@group.Flag && @group.Key != id)
                                                                                          && @group.ItemValue.Replace(" ", String.Empty)
                                                                                              .ToLowerInvariant()
                                                                                              .Equals(customGroupName.Replace(" ", String.Empty)
                                                                                                  .ToLowerInvariant()));
            //bool isDuplicateMstr = CustomGroupsList != null && CustomGroupsList.Any(group => (!@group.Flag) &&
            //                                                                              @group.ItemValue.Replace(" ", String.Empty).ToLowerInvariant()
            //                                                                                  .Equals(customGroupName.Replace(" ", String.Empty)
            //                                                                                      .ToLowerInvariant()));
            if (isDuplicateCustom)
            {
                DialogService.DialogServiceInstance.ShowMessageBox(this, "Custom Group with the same name already exists",
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK, true);
                return true;
            }
            return CheckDuplicacyInAccounts(customGroupName);
        }

        private void RefreshAccountsList()
        {
            foreach (var account in AccountsList)
            {
                account.Flag = false;
            }
        }

        /// <summary>
        /// Move accounts to Unassigned List
        /// </summary>
        /// <param name="keyValueItemsWithFlag"></param>
        private void MoveToUnassignedList(object keyValueItemsWithFlag)
        {
            try
            {
                IEnumerable<KeyValueItemWithFlag> unassignedList
                        = ((System.Collections.IList)keyValueItemsWithFlag).Cast<KeyValueItemWithFlag>();
                foreach (KeyValueItemWithFlag account in unassignedList)
                {
                    account.Flag = false;
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
        /// Move accounts to Assigned List
        /// </summary>
        /// <param name="keyValueItemsWithFlag"></param>
        private void MoveToAssignedList(object keyValueItemsWithFlag)
        {
            try
            {
                IEnumerable<KeyValueItemWithFlag> assignedList
                       = ((System.Collections.IList)keyValueItemsWithFlag).Cast<KeyValueItemWithFlag>();
                foreach (KeyValueItemWithFlag account in assignedList)
                {
                    account.Flag = true;
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
        /// Set Available And assigned list
        /// </summary>
        /// <param name="selectedMasterFund"></param>
        private void SetAvailableAndAssignedList(KeyValueItemWithFlag selectedMasterFund)
        {
            try
            {
                if (selectedMasterFund != null)
                {
                    if (selectedMasterFund.Key == -1)
                    {
                        RefreshAccountsList();
                        return;
                    }

                    HashSet<int> assignedAccounts =
                        (!selectedMasterFund.Flag ?
                        CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[selectedMasterFund.Key] :
                        RebalancerCache.Instance.GetCustomGroupAssociatedAccounts(selectedMasterFund.Key)).ToHashSet();

                    foreach (var account in AccountsList)
                    {
                        account.Flag = assignedAccounts.Contains(account.Key);
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
        /// Add or Update Custom Groups
        /// </summary>
        /// <param name="customGroupDto"></param>
        internal void AddOrUpdateCustomGroups(CustomGroupDto customGroupDto)
        {
            try
            {
                if (CustomGroupsList != null)
                {
                    //Change On UI
                    var customGroup = CustomGroupsList.FirstOrDefault(group => group.Flag && (group.ItemValue.Equals(customGroupDto.GroupName) || group.Key == customGroupDto.GroupID));
                    if (customGroup != null)
                    {
                        customGroup.Key = customGroupDto.GroupID;
                        customGroup.ItemValue = customGroupDto.GroupName;
                    }
                    else
                        CustomGroupsList.Add(new KeyValueItemWithFlag { Key = customGroupDto.GroupID, ItemValue = customGroupDto.GroupName, Flag = true });
                    OnPropertyChanged("CustomGroupsList");
                }

                //Change in Cache

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

        protected virtual void Dispose(bool disposing)
        {           
            if (disposing)
            {
                if (_accountsList != null)
                    _accountsList = null;

                if (_customGroupsList != null)
                    _customGroupsList = null;

                _selectedGroup = null;

                _statusAndErrorMessages = null;

                SecurityMaster = null;

                RebalancerHelperInstance = null;
                if (_formatWindow != null)
                {
                    _formatWindow.Closing -= FormatWindow_Closing;
                    _formatWindow.Close();
                }
            }             
        }

        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        internal void DeleteCustomGroupUI(int customGroupId)
        {
            KeyValueItemWithFlag customGroup = CustomGroupsList.First(custom => custom.Key == customGroupId);
            //if (customGroup != null && customGroup.Equals(SelectedGroup))
            //{
            //    DialogService.DialogServiceInstance.ShowMessageBox(this, "Custom Group is deleted by another user",
            //        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
            //        MessageBoxImage.Warning,
            //        MessageBoxResult.OK, true);
            //}
            CustomGroupsList.Remove(customGroup);
        }

    }
}
