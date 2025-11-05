using ExportGridsData;
using Infragistics.Windows.DataPresenter;
using Newtonsoft.Json;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ServiceConnector;
using Prana.Utilities.ImportExportUtilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Task = System.Threading.Tasks.Task;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class ModelPortfolioViewModel : RebalancerBase, IExportGridData
    {
        public EventHandler<EventArgs<bool>> CheckUncheckFilteredRecords;

        #region Properties

        public FormatWindow _formatWindow;
        /// <summary>
        /// Position Type
        /// </summary>
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

        /// <summary>
        /// Selected Position Type
        /// </summary>
        private KeyValueItem _selectedPositionType;
        public KeyValueItem SelectedPositionType
        {
            get { return _selectedPositionType; }
            set
            {
                _selectedPositionType = value;
                IsChangesMade = true;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tolerance List
        /// </summary>
        private ObservableCollection<KeyValueItem> _useToleranceList;
        public ObservableCollection<KeyValueItem> UseToleranceList
        {
            get { return _useToleranceList; }
            set
            {
                _useToleranceList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Use Tolerance
        /// </summary>
        private KeyValueItem _selectedUseTolerance;
        public KeyValueItem SelectedUseTolerance
        {
            get { return _selectedUseTolerance; }
            set
            {
                _selectedUseTolerance = value;
                if(_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                {
                    IsToleranceFactorValid = true;
                    if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                        VisibilityTolerancePercentage = Visibility.Visible;
                    else
                        VisibilityToleranceBPS = Visibility.Visible;
                    VisibilityIsChecked = Visibility.Visible;
                }
                else
                {
                    IsToleranceFactorValid = false;
                    VisibilityTolerancePercentage = Visibility.Collapsed;
                    VisibilityToleranceBPS = Visibility.Collapsed;
                    VisibilityIsChecked = Visibility.Collapsed;
                }
                IsChangesMade = true;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tolerance Factor List
        /// </summary>
        private ObservableCollection<KeyValueItem> _toleranceFactorList;
        public ObservableCollection<KeyValueItem> ToleranceFactorList
        {
            get { return _toleranceFactorList; }
            set
            {
                _toleranceFactorList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Tolerance Factor
        /// </summary>
        private KeyValueItem _selectedToleranceFactor;
        public KeyValueItem SelectedToleranceFactor
        {
            get { return _selectedToleranceFactor; }
            set
            {
                _selectedToleranceFactor = value;
                if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                {
                    if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                    {
                        VisibilityTolerancePercentage = Visibility.Visible;
                        VisibilityToleranceBPS = Visibility.Collapsed;
                    }
                    else
                    {
                        VisibilityToleranceBPS = Visibility.Visible;
                        VisibilityTolerancePercentage = Visibility.Collapsed;
                    }
                }
                IsChangesMade = true;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Target Percent Type List
        /// </summary>
        private ObservableCollection<KeyValueItem> _targetPercentTypeList;
        public ObservableCollection<KeyValueItem> TargetPercentTypeList
        {
            get { return _targetPercentTypeList; }
            set
            {
                _targetPercentTypeList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Target Percent Type
        /// </summary>
        private KeyValueItem _selectedTargetPercentType;
        public KeyValueItem SelectedTargetPercentType
        {
            get { return _selectedTargetPercentType; }
            set
            {
                _selectedTargetPercentType = value;
                IsChangesMade = true;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Export Model Portfolio
        /// </summary>
        private bool _exportModelPortfolio;
        public bool ExportModelPortfolio
        {
            get { return _exportModelPortfolio; }
            set { _exportModelPortfolio = value; OnPropertyChanged("ExportModelPortfolio"); }
        }

        private bool _exportModelPortfolioForAutomation;
        public bool ExportModelPortfolioForAutomation
        {
            get { return _exportModelPortfolioForAutomation; }
            set { _exportModelPortfolioForAutomation = value; OnPropertyChanged("ExportModelPortfolioForAutomation"); }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }

        /// <summary>
        /// Field to identify whether Edit page UI or add Page UI
        /// </summary>
        private bool _isAddPage;
        public bool IsAddPage
        {
            get { return _isAddPage; }
            set
            {
                _isAddPage = value;
                OnPropertyChanged("IsAddPage");
            }
        }

        /// <summary>
        /// Check whether Selected Model Portfolio Null
        /// </summary>
        private bool _isSelectedModelPortfolioNotNull;
        public bool IsSelectedModelPortfolioNotNull
        {
            get { return _isSelectedModelPortfolioNotNull; }
            set
            {
                _isSelectedModelPortfolioNotNull = value;
                OnPropertyChanged("IsSelectedModelPortfolioNotNull");
            }
        }

        /// <summary>
        /// Field to check whether Portfolio type is Model Portfolio
        /// </summary>
        private bool _isPortfolioTypeModelPortfolio;
        public bool IsPortfolioTypeModelPortfolio
        {
            get { return _isPortfolioTypeModelPortfolio; }
            set
            {
                _isPortfolioTypeModelPortfolio = value;
                OnPropertyChanged("IsPortfolioTypeModelPortfolio");
            }
        }

        /// <summary>
        /// Field to check whether Portfolio type is Account
        /// </summary>
        private bool _isPortfolioTypeAccount;
        public bool IsPortfolioTypeAccount
        {
            get { return _isPortfolioTypeAccount; }
            set
            {
                _isPortfolioTypeAccount = value;
                OnPropertyChanged("IsPortfolioTypeAccount");
            }
        }

        /// <summary>
        /// Field to check whether Portfolio type is Account
        /// </summary>
        private bool _isPortfolioTypeCustomGroup;
        public bool IsPortfolioTypeCustomGroup
        {
            get { return _isPortfolioTypeCustomGroup; }
            set
            {
                _isPortfolioTypeCustomGroup = value;
                OnPropertyChanged("IsPortfolioTypeCustomGroup");
            }
        }
        /// <summary>
        /// Field to check whether Portfolio type is Account
        /// </summary>
        private bool _isPortfolioTypeMasterFund;
        public bool IsPortfolioTypeMasterFund
        {
            get { return _isPortfolioTypeMasterFund; }
            set
            {
                _isPortfolioTypeMasterFund = value;
                OnPropertyChanged("IsPortfolioTypeMasterFund");
            }
        }
        /// <summary>
        /// Field to check whether Use Tolerance is Valid or Not
        /// </summary>
        private bool _isUseToleranceValid;
        public bool IsUseToleranceValid
        {
            get { return _isUseToleranceValid; }
            set
            {
                _isUseToleranceValid = value;
                OnPropertyChanged("IsUseToleranceValid");
            }
        }
        /// <summary>
        /// Field to check whether Tolerance Factor is Valid to use
        /// </summary>
        private bool _isToleranceFactorValid;
        public bool IsToleranceFactorValid
        {
            get { return _isToleranceFactorValid; }
            set
            {
                _isToleranceFactorValid = value;
                OnPropertyChanged("IsToleranceFactorValid");
                IsMultiSelectPanelVisible = value && (PortfolioList != null);
            }
        }

        /// <summary>
        /// This is used to check if any changes made in UI by user
        /// </summary>
        private bool _isChangesMade;
        private bool IsChangesMade
        {
            get { return _isChangesMade; }
            set
            {
                _isChangesMade = value;
                OnPropertyChanged("_IsChangesMade");
                if (IsChangesMade && IsSelectedModelPortfolioNotNull)
                {
                    AddUpdateStatusAndMessage("Model portfolio has unsaved changes. Please save it before Rebalancing", string.Empty);
                }
                else
                {
                    AddUpdateStatusAndMessage(string.Empty, string.Empty);
                }
            }
        }

        /// <summary>
        /// Imported Model Portfolio data
        /// </summary>
        private ObservableCollection<PortfolioDto> _portfolioList;
        public ObservableCollection<PortfolioDto> PortfolioList
        {
            get { return _portfolioList; }
            set
            {
                _portfolioList = value;
                OnPropertyChanged();
                if (PortfolioList != null)
                {
                    IsMultiSelectPanelVisible = IsToleranceFactorValid;
                }
                else
                {
                    IsMultiSelectPanelVisible = false;
                }
            }
        }

        /// <summary>
        /// Field to check whether MultiSelect elements should visible or not
        /// </summary>
        private bool _isMultiSelectPanelVisible = false;
        public bool IsMultiSelectPanelVisible
        {
            get { return _isMultiSelectPanelVisible; }
            set
            {
                _isMultiSelectPanelVisible = value;
                OnPropertyChanged("IsMultiSelectPanelVisible");
            }
        }

        /// <summary>
        /// Selected Model Portfolio
        /// </summary>
        private KeyValueItem _selectedPortfolioType;
        public KeyValueItem SelectedPortfolioType
        {
            get { return _selectedPortfolioType; }
            set
            {
                _selectedPortfolioType = value;
                if (value != null)
                {
                    IsPortfolioTypeModelPortfolio = value.ItemValue.Equals("Model Portfolio");
                    IsPortfolioTypeAccount = value.ItemValue.Equals("Account") && !IsPortfolioTypeModelPortfolio;
                    IsPortfolioTypeMasterFund = value.ItemValue.Equals("Master Fund") && !IsPortfolioTypeModelPortfolio;
                    IsPortfolioTypeCustomGroup = value.ItemValue.Equals("Custom Group") && !IsPortfolioTypeModelPortfolio;

                    if (IsPortfolioTypeModelPortfolio && _selectedModelType != null && (int)_selectedModelType.Key == (int)RebalancerEnums.ModelType.TargetCash)
                    {
                        IsUseToleranceValid = false;
                    }
                    else
                    {
                        IsUseToleranceValid = true;
                    }
                    if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                    {
                        if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                            VisibilityTolerancePercentage = Visibility.Visible;
                        else
                            VisibilityToleranceBPS = Visibility.Visible;
                        VisibilityIsChecked = Visibility.Visible;
                    }
                    else
                    {
                        VisibilityTolerancePercentage = Visibility.Collapsed;
                        VisibilityToleranceBPS = Visibility.Collapsed;
                        VisibilityIsChecked = Visibility.Collapsed;
                    }
                }
                IsChangesMade = true;
                OnPropertyChanged("SelectedPortfolioType");
                PortfolioList = null;
            }
        }

        /// <summary>
        /// Portfolio Type List
        /// </summary>
        private ObservableCollection<KeyValueItem> _portfolioTypeList;
        public ObservableCollection<KeyValueItem> PortfolioTypeList
        {
            get { return _portfolioTypeList; }
            set
            {
                _portfolioTypeList = value;
                OnPropertyChanged("PortfolioTypeList");
            }
        }

        /// <summary>
        /// Selected Model Portfolio
        /// </summary>
        private KeyValueItem _selectedModelPortfolio;
        public KeyValueItem SelectedModelPortfolio
        {
            get { return _selectedModelPortfolio; }
            set
            {
                _selectedModelPortfolio = value;
                OnPropertyChanged("SelectedModelPortfolio");
                if (value != null)
                {
                    GetDataForModelPortfolio();
                    IsSelectedModelPortfolioNotNull = true;
                }
                else
                    IsSelectedModelPortfolioNotNull = false;
                IsChangesMade = false;
            }
        }

        /// <summary>
        /// Model Portfolio Name
        /// </summary>
        private string _modelPortfolioName;
        public string ModelPortfolioName
        {
            get { return _modelPortfolioName; }
            set
            {
                _modelPortfolioName = value.ToString().Trim(); ;
                OnPropertyChanged("ModelPortfolioName");
            }
        }

        /// <summary>
        /// Selected Account
        /// </summary>
        private KeyValueItem _selectedAccount;
        public KeyValueItem SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged("SelectedAccount");
                PortfolioList = null;
                IsChangesMade = true;
            }
        }

        /// <summary>
        /// Selected Account
        /// </summary>
        private KeyValueItem _selectedCustomGroup;
        public KeyValueItem SelectedCustomGroup
        {
            get { return _selectedCustomGroup; }
            set
            {
                _selectedCustomGroup = value;
                OnPropertyChanged("SelectedCustomGroup");
                PortfolioList = null;
                IsChangesMade = true;
            }
        }
        /// <summary>
        /// Selected MasterFund
        /// </summary>
        private KeyValueItem _selectedMasterFund;
        public KeyValueItem SelectedMasterFund
        {
            get { return _selectedMasterFund; }
            set
            {
                _selectedMasterFund = value;
                OnPropertyChanged("SelectedMasterFund");
                PortfolioList = null;
                IsChangesMade = true;
            }
        }

        /// <summary>
        /// Model Portfolio List
        /// </summary>
        private ObservableCollection<KeyValueItem> _modelPortfolioList;
        public ObservableCollection<KeyValueItem> ModelPortfolioList
        {
            get { return _modelPortfolioList; }
            set
            {
                _modelPortfolioList = value;
                OnPropertyChanged("ModelPortfolioList");
            }
        }

        /// <summary>
        /// Master Funds List
        /// </summary>
        private ObservableCollection<KeyValueItem> _masterFunds;
        public ObservableCollection<KeyValueItem> MasterFunds
        {
            get { return _masterFunds; }
            set
            {
                _masterFunds = value;
                OnPropertyChanged("MasterFunds");
            }
        }

        /// <summary>
        /// Accounts List
        /// </summary>
        private ObservableCollection<KeyValueItem> _accounts;
        public ObservableCollection<KeyValueItem> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged("Accounts");
            }
        }

        /// <summary>
        /// Custom Group List
        /// </summary>
        private ObservableCollection<KeyValueItem> _customGroups;
        public ObservableCollection<KeyValueItem> CustomGroups
        {
            get { return _customGroups; }
            set
            {
                _customGroups = value;
                OnPropertyChanged("CustomGroups");
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

        /// <summary>
        /// Model Type
        /// </summary>
        private ObservableCollection<KeyValueItem> _modelType;
        public ObservableCollection<KeyValueItem> ModelType
        {
            get { return _modelType; }
            set
            {
                _modelType = value;
                OnPropertyChanged("ModelType");
            }
        }

        /// <summary>
        /// selected Model Type
        /// </summary>
        private KeyValueItem _selectedModelType;
        public KeyValueItem SelectedModelType
        {
            get { return _selectedModelType; }
            set
            {
                _selectedModelType = value;
                if (value != null && IsPortfolioTypeModelPortfolio)
                {
                    if ((int)value.Key == (int)RebalancerEnums.ModelType.TargetCash)
                    {
                        IsUseToleranceValid = false;
                        IsToleranceFactorValid = false;
                        VisibilityTolerancePercentage = Visibility.Collapsed;
                        VisibilityToleranceBPS = Visibility.Collapsed;
                        VisibilityIsChecked = Visibility.Collapsed;
                    }
                    else
                    {
                        IsUseToleranceValid = true;

                        if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                        {
                            if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                                VisibilityTolerancePercentage = Visibility.Visible;
                            else
                                VisibilityToleranceBPS = Visibility.Visible;
                            VisibilityIsChecked = Visibility.Visible;
                            IsToleranceFactorValid = true;
                        }
                    }
                }
                IsChangesMade = true;
                OnPropertyChanged("SelectedModelType");
            }
        }

        private Visibility _visibilityTolerancePercentage = Visibility.Collapsed;
        public Visibility VisibilityTolerancePercentage
        {
            get { return _visibilityTolerancePercentage; }
            set
            {
                _visibilityTolerancePercentage = value;
                OnPropertyChanged("VisibilityTolerancePercentage");
            }
        }

        private Visibility _visibilityToleranceBPS = Visibility.Collapsed;
        public Visibility VisibilityToleranceBPS
        {
            get { return _visibilityToleranceBPS; }
            set
            {
                _visibilityToleranceBPS = value;
                OnPropertyChanged("VisibilityToleranceBPS");
            }
        }

        private Visibility _visibilityIsChecked = Visibility.Collapsed;
        public Visibility VisibilityIsChecked
        {
            get { return _visibilityIsChecked; }
            set
            {
                _visibilityIsChecked = value;
                OnPropertyChanged("VisibilityIsChecked");
            }
        }

        private bool _isAllTradesChecked;
        public bool IsAllTradesChecked
        {
            get { return _isAllTradesChecked; }
            set
            {
                _isAllTradesChecked = value;
                if (CheckUncheckFilteredRecords != null)
                    CheckUncheckFilteredRecords(this, new EventArgs<bool>(_isAllTradesChecked));
                OnPropertyChanged("IsAllTradesChecked");
            }
        }        

        private string _tolrenaceinBPS = "0";
        public string TolrenaceinBPS
        {
            get { return _tolrenaceinBPS; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _tolrenaceinBPS = "0";
                }
                else
                {
                    _tolrenaceinBPS = value;
                }
                OnPropertyChanged("TolrenaceinBPS");

                _tolrenaceinPercentage = ((decimal)(int.Parse(_tolrenaceinBPS) / 100)).ToString();
                OnPropertyChanged("TolerancePercentage");
            }
        }

        private string _tolrenaceinPercentage = "0.00";
        public string TolrenaceinPercentage
        {
            get { return _tolrenaceinPercentage; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _tolrenaceinPercentage = "0.00";
                }
                else
                {
                    _tolrenaceinPercentage = value;
                }
                OnPropertyChanged("TolrenaceinPercentage");

                _tolrenaceinBPS = ((int)(decimal.Parse(_tolrenaceinPercentage) * 100)).ToString();
                OnPropertyChanged("TolrenaceinBPS");

            }
        }
        #endregion

        #region Commands

        public DelegateCommand OpenFormatCommand { get; set; }

        /// <summary>
        /// Import model portfolio Command
        /// </summary>
        public DelegateCommand ImportModelPortfolioCommand { get; set; }

        /// <summary>
        /// Save Model Portfolio Command
        /// </summary>
        public DelegateCommand SavePortfolioCommand { get; set; }

        /// <summary>
        /// New Portfolio Command
        /// </summary>
        public DelegateCommand NewPortfolioCommand { get; set; }

        /// <summary>
        /// Edit Portfolio Command
        /// </summary>
        public DelegateCommand EditPortfolioCommand { get; set; }

        /// <summary>
        /// Cancel the add operation Command
        /// </summary>
        public DelegateCommand CancelPortfolioCommand { get; set; }

        /// <summary>
        /// Delete Model Portfolio Command
        /// </summary>
        public DelegateCommand DeletePortfolioCommand { get; set; }

        /// <summary>
        /// Command to view Model Portfolio
        /// </summary>
        public DelegateCommand<KeyValueItem> ViewModelPortfolioForAccountCommand { get; set; }

        /// <summary>
        /// Command to view Model Portfolio
        /// </summary>
        public DelegateCommand<KeyValueItem> ViewModelPortfolioForMasterFundCommand { get; set; }

        /// <summary>
        /// Command to view Model Portfolio
        /// </summary>
        public DelegateCommand<KeyValueItem> ViewModelPortfolioFoCustomGroupCommand { get; set; }
        /// <summary>
        /// Export Model Portfolio
        /// </summary>
        public DelegateCommand ExportModelPortfolioCommand { get; set; }

        public DelegateCommand AddNewRowCommand { get; set; }

        public DelegateCommand<object> DeleteRowCommand { get; set; }

        public DelegateCommand<object> ModelPortfolioGrid_CellUpdated { get; set; }
        public DelegateCommand<object> ModelPortfolioGrid_CellChanged { get; set; }

        public DelegateCommand<object> PortfolionNameChanged { get; set; }

        public DelegateCommand SetToleranceCommand { get; set; }

        public DelegateCommand ClearToleranceCommand { get; set; }

        #endregion

        public ModelPortfolioViewModel(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
            : base(securityMasterInstance, rebalancerHelperInstance)
        {
            ImportModelPortfolioCommand = new DelegateCommand(() => ImportModelPortfolio());
            OpenFormatCommand = new DelegateCommand(() => OpenFormatCommandAction());
            SavePortfolioCommand = new DelegateCommand(async () => await SavePortfolio());
            EditPortfolioCommand = new DelegateCommand(async () => await EditPortfolio());
            DeletePortfolioCommand = new DelegateCommand(() => DeleteFromModelPortfolio());
            SetToleranceCommand = new DelegateCommand(() => SetToleranceMethod());
            ClearToleranceCommand = new DelegateCommand(() => ClearToleranceMethod());
            ExportModelPortfolioCommand = new DelegateCommand(() => { ExportModelPortfolio = true; });
            ViewModelPortfolioForAccountCommand = new DelegateCommand<KeyValueItem>((selectedAccount) => GetModelProtfolioDataForFundOrMsterFund(selectedAccount));
            ViewModelPortfolioForMasterFundCommand = new DelegateCommand<KeyValueItem>((selectedMasterFund) => GetModelProtfolioDataForFundOrMsterFund(selectedMasterFund));
            ViewModelPortfolioFoCustomGroupCommand = new DelegateCommand<KeyValueItem>((selectedCustomGroup) => GetModelProtfolioDataForFundOrMsterFund(selectedCustomGroup));
            AddNewRowCommand = new DelegateCommand(() => AddNewRowToXamGrid());
            DeleteRowCommand = new DelegateCommand<object>(obj => DeleteRowFromXamGrid(obj));
            ModelPortfolioGrid_CellUpdated = new DelegateCommand<object>(e => ModelPortfolioGridCellUpdated(this, e));
            ModelPortfolioGrid_CellChanged = new DelegateCommand<object>(e => ModelPortfolioGridCellChanged(this, e));
            PortfolionNameChanged = new DelegateCommand<object>(e => PortfolionNameChangedMethod(this, e));
            if (SecurityMaster != null)
            {
                SecurityMaster.SecMstrDataResponse += SecMasterClientSecMstrDataResponse;
            }
            StatusAndErrorMessages = new StatusAndErrorMessageModel();
            InitializeViewModel();
            RebalancerCache.Instance.UpdateCustomGroups += CustomGroup_UpdateCustomGroups;
            InstanceManager.RegisterInstance(this);
        }

        private void CustomGroup_UpdateCustomGroups(object sender, EventArgs e)
        {
            CustomGroups = GetCustomGroups();
        }

        /// <summary>
        /// This method set the unsaved message at the bottom
        /// </summary>
        private void AddUpdateStatusAndMessage(string statusMessage, string errorMessage)
        {
            StatusAndErrorMessages.StatusMessage = statusMessage;
            StatusAndErrorMessages.ErrorMessage = errorMessage;
        }

        private void GetModelProtfolioDataForFundOrMsterFund(KeyValueItem selectedItem)
        {
            try
            {
                if (selectedItem == null) return;
                StringBuilder errorMessage = new StringBuilder();
                List<ModelPortfolioSecurityDto> modelPortfolioData = null;
                ModelPortfolioDto modelPortfolioDto = new ModelPortfolioDto
                {

                    ModelPortfolioType = IsPortfolioTypeMasterFund ? RebalancerEnums.ModelPortfolioType.MasterFund :
                    (IsPortfolioTypeAccount ? RebalancerEnums.ModelPortfolioType.Account : RebalancerEnums.ModelPortfolioType.CustomGroup),
                    ReferenceId = selectedItem.Key
                };
                modelPortfolioData = RebalancerCommon.Instance.GetModelProtfolioData(modelPortfolioDto, (RebalancerEnums.RebalancerPositionsType)SelectedPositionType.Key, ref errorMessage);
                PortfolioList = new ObservableCollection<PortfolioDto>(modelPortfolioData.Select(x => new PortfolioDto() { Symbol = x.Symbol, TargetPercentage = x.TargetPercentage }).ToList());
                if (_selectedModelPortfolio != null)
                {
                    ModelPortfolioDto savedModelPortfolioDto = RebalancerCache.Instance.GetModelPortfolio(_selectedModelPortfolio.Key);
                    if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes
                        && savedModelPortfolioDto.ReferenceId == modelPortfolioDto.ReferenceId)
                    {
                        UpdateListToMergeTolerance(savedModelPortfolioDto);
                    }
                }
                SendRequestForValidation(PortfolioList);
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
        /// This method merges the tolerance to the symbol in portfolioList
        /// </summary>
        private void UpdateListToMergeTolerance(ModelPortfolioDto modelPortfolioDto)
        {
            try
            {
                if (!string.IsNullOrEmpty(modelPortfolioDto.ModelPortfolioData))
                {
                    Dictionary<string, decimal> dictSymbolTolerancePercentage = new Dictionary<string, decimal>();
                    List<PortfolioDto> portfolioDtos = JsonConvert.DeserializeObject<List<PortfolioDto>>(modelPortfolioDto.ModelPortfolioData);
                    dictSymbolTolerancePercentage = portfolioDtos.ToDictionary(key => key.Symbol, value => value.TolerancePercentage);
                    foreach (var portfolio in PortfolioList)
                    {
                        if (dictSymbolTolerancePercentage.ContainsKey(portfolio.Symbol))
                            portfolio.TolerancePercentage = dictSymbolTolerancePercentage[portfolio.Symbol];
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

        private void AddNewRowToXamGrid()
        {
            PortfolioList.Add(new PortfolioDto());
            IsChangesMade = true;
        }

        private void DeleteRowFromXamGrid(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Infragistics.Windows.DataPresenter.XamDataGrid dataGrid = (Infragistics.Windows.DataPresenter.XamDataGrid)obj;
                    if (dataGrid.ActiveRecord != null)
                    {
                        var activeRecord = dataGrid.ActiveRecord as DataRecord;
                        var item = activeRecord.DataItem as PortfolioDto;
                        if (item != null)
                        {
                            PortfolioList.Remove(item);
                            IsChangesMade = true;
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

        private void ModelPortfolioGridCellUpdated(object sender, object e)
        {
            try
            {

                Infragistics.Windows.DataPresenter.Events.CellUpdatedEventArgs arg = (Infragistics.Windows.DataPresenter.Events.CellUpdatedEventArgs)e;
                if (arg.Field.Name.Equals(RebalancerConstants.COL_Symbol))
                {
                    if (arg.Cell.Value != null)
                    {
                        PortfolioDto portfolioDto = PortfolioList.SingleOrDefault(p => p.Symbol.ToUpper().Equals(arg.Cell.Value.ToString().ToUpper()));
                        if (portfolioDto != null)
                        {
                            if (!arg.Cell.Value.ToString().ToUpper().Equals("CASH"))
                            {
                                portfolioDto.IsSymbolValid = false;
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(arg.Cell.Value.ToString().ToUpper(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = GetHashCode();
                                if (SecurityMaster != null)
                                {
                                    SecurityMaster.SendRequest(reqObj);
                                }
                            }
                            else
                                portfolioDto.IsSymbolValid = true;
                        }

                    }
                    else
                    {
                        PortfolioDto portfolioDto = PortfolioList.SingleOrDefault(p => string.IsNullOrWhiteSpace(p.Symbol));
                        portfolioDto.IsSymbolValid = false;
                        ShowErrorAlert("Cell Value Can't be blank.");
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

        private void ModelPortfolioGridCellChanged(object sender, object e)
        {
            try
            {
                Infragistics.Windows.DataPresenter.Events.CellChangedEventArgs arg = (Infragistics.Windows.DataPresenter.Events.CellChangedEventArgs)e;
                if (arg != null)
                {
                    if (arg.Cell.Field.Name.Equals(RebalancerConstants.COL_IsChecked))
                        return;
                    IsChangesMade = true;
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

        private void PortfolionNameChangedMethod(object sender, object e)
        {
            try
            {
                System.Windows.Controls.TextChangedEventArgs arg = (System.Windows.Controls.TextChangedEventArgs)e;
                if (arg != null && Keyboard.FocusedElement == arg.OriginalSource)
                {
                    IsChangesMade = true;
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

        #region Methods
        /// <summary>
        /// Get data for selected model portfolio and bind it with UI
        /// </summary>
        private void GetDataForModelPortfolio()
        {
            try
            {
                ModelPortfolioName = _selectedModelPortfolio.ItemValue;
                PortfolioList = null;
                ModelPortfolioDto modelPortfolioDto =
                    RebalancerCache.Instance.GetModelPortfolio(_selectedModelPortfolio.Key);
                SelectedPortfolioType =
                    PortfolioTypeList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.ModelPortfolioType);
                if (IsPortfolioTypeAccount)
                    SelectedAccount = Accounts.SingleOrDefault(p => p.Key == modelPortfolioDto.ReferenceId);
                else if (IsPortfolioTypeModelPortfolio)
                {
                    PortfolioList =
                        JsonConvert.DeserializeObject<ObservableCollection<PortfolioDto>>(modelPortfolioDto
                            .ModelPortfolioData);
                    SendRequestForValidation(PortfolioList);
                    SelectedModelType = ModelType.SingleOrDefault(p => p.Key == modelPortfolioDto.ReferenceId);
                }
                else if (IsPortfolioTypeMasterFund)
                    SelectedMasterFund = MasterFunds.SingleOrDefault(p => p.Key == modelPortfolioDto.ReferenceId);
                else
                    SelectedCustomGroup = CustomGroups.SingleOrDefault(p => p.Key == modelPortfolioDto.ReferenceId);
                if (modelPortfolioDto.PositionsType != null)
                {
                    _selectedPositionType = PositionTypeList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.PositionsType);
                    SelectedPositionType = PositionTypeList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.PositionsType);
                }

                if (modelPortfolioDto.UseTolerance != null)
                {
                    _selectedUseTolerance = UseToleranceList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.UseTolerance);
                    SelectedUseTolerance = UseToleranceList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.UseTolerance);
                    _selectedToleranceFactor = ToleranceFactorList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.ToleranceFactor);
                    SelectedToleranceFactor = ToleranceFactorList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.ToleranceFactor);
                    _selectedTargetPercentType = TargetPercentTypeList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.TargetPercentType);
                    SelectedTargetPercentType = TargetPercentTypeList.SingleOrDefault(p => p.Key == (int)modelPortfolioDto.TargetPercentType);
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
        /// Get MasterFunds from cache and convert them into observable collection
        /// </summary>
        /// <returns>MasterFunds</returns>
        private ObservableCollection<KeyValueItem> GetMasterFunds()
        {
            List<KeyValueItem> keyValueItems = CachedDataManager.GetInstance.GetUserMasterFunds()
                .Select(p => new KeyValueItem { ItemValue = p.Value, Key = p.Key }).ToList();
            return new ObservableCollection<KeyValueItem>(keyValueItems);
        }

        /// <summary>
        /// Get accounts from cache and convert then into observable collection
        /// </summary>
        /// <returns>MasterFunds</returns>
        private ObservableCollection<KeyValueItem> GetAccounts()
        {
            return new ObservableCollection<KeyValueItem>(CachedDataManager.GetInstance.GetUserAccountsAsDict()
                .Select(p => new KeyValueItem { ItemValue = p.Value, Key = p.Key }).ToList());
        }

        private ObservableCollection<KeyValueItem> GetCustomGroups()
        {
            return new ObservableCollection<KeyValueItem>(RebalancerCache.Instance.GetCustomGroupsDictionaryOnTheBasisOfPermittedFund()
                .Select(p => new KeyValueItem { ItemValue = p.Value, Key = p.Key }).ToList());
        }
        /// <summary>
        /// Initialize Model Portfolio View Model
        /// </summary>
        internal void InitializeViewModel()
        {
            try
            {
                NewPortfolioCommand = new DelegateCommand(() =>
                {
                    IsAddPage = true;
                    IsSelectedModelPortfolioNotNull = true;
                    ResetAllValues();
                });
                CancelPortfolioCommand = new DelegateCommand(() =>
                {
                    IsAddPage = false;
                    SelectedModelPortfolio = null;
                    ResetAllValues();
                });
                PortfolioTypeList = GetPortfolioTypeList();
                Accounts = GetAccounts();
                MasterFunds = GetMasterFunds();
                CustomGroups = GetCustomGroups();
                ModelPortfolioList = RebalancerCache.Instance.GetAllModelPortfolioNames();
                PositionTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.RebalancerPositionsType>();
                ResetAllValues();
                ModelType = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.ModelType>();
                SelectedModelType = ModelType.SingleOrDefault(p => p.Key == (int)RebalancerEnums.ModelType.TargetSecurity);
                UseToleranceList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.UseTolerance>();
                SelectedUseTolerance = UseToleranceList.SingleOrDefault(p => p.Key == (int)RebalancerEnums.UseTolerance.No);
                ToleranceFactorList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.ToleranceFactor>();
                SelectedToleranceFactor = ToleranceFactorList.SingleOrDefault(p => p.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage);
                TargetPercentTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.TargetPercentType>();
                SelectedTargetPercentType = TargetPercentTypeList.SingleOrDefault(p => p.Key == (int)RebalancerEnums.TargetPercentType.ModelTargetPercent);
                IsChangesMade = false;
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
        /// Delete Model Portfolio
        /// </summary>
        private void DeleteFromModelPortfolio()
        {
            try
            {
                MessageBoxResult rsltMessageBox = MessageBox.Show(String.Format("Do you really want to delete Model Portfolio: {0} ?", _selectedModelPortfolio.ItemValue), RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                               MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (rsltMessageBox == MessageBoxResult.Yes)
                {

                    bool result = RebalancerServiceApi.GetInstance().DeleteModelPortfolio(_selectedModelPortfolio.Key);
                    if (result)
                        DialogService.DialogServiceInstance.ShowMessageBox(this, "Model Portfolio Deleted",
                            RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information,
                            MessageBoxResult.OK, true);
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
        /// Edit Portfolio 
        /// </summary>
        /// <returns></returns>
        private async Task EditPortfolio()
        {
            try
            {
                await SavePortfolio(true);
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
        /// Reset all Values on UI
        /// </summary>
        private void ResetAllValues()
        {
            SelectedPortfolioType =
                PortfolioTypeList.SingleOrDefault(p => p.Key == 1);
            ModelPortfolioName = String.Empty;
            SelectedAccount = null;
            SelectedMasterFund = null;
            PortfolioList = null;
            OnPropertyChanged("PortfolioList");
        }

        /// <summary>
        /// Save Model Portfolio
        /// </summary>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        async Task SavePortfolio(bool isEdit = false)
        {
            try
            {
                if (ValidateBeforeSaving(isEdit))
                {
                    ModelPortfolioDto saveModelPortfolioInstance = new ModelPortfolioDto();
                    saveModelPortfolioInstance.ModelPortfolioName = ModelPortfolioName;

                    if (isEdit)
                        saveModelPortfolioInstance.Id = _selectedModelPortfolio.Key;

                    saveModelPortfolioInstance.ModelPortfolioType =
                        (RebalancerEnums.ModelPortfolioType)SelectedPortfolioType.Key;
                    if (SelectedPortfolioType.Key.Equals((int)RebalancerEnums.ModelPortfolioType.ModelPortfolio))
                    {
                        if (SelectedModelType != null)
                            saveModelPortfolioInstance.ReferenceId = SelectedModelType.Key;
                        else
                            ShowErrorAlert("Model Type not selected, first select Model Type.");
                        saveModelPortfolioInstance.ModelPortfolioData = JsonConvert.SerializeObject(GetReducedPortfolioList(PortfolioList));
                    }
                    else if (SelectedPortfolioType.Key.Equals((int)RebalancerEnums.ModelPortfolioType.Account))
                    {
                        if (SelectedAccount != null)
                            saveModelPortfolioInstance.ReferenceId = SelectedAccount.Key;
                        else
                            ShowErrorAlert("Account not selected, first select Account.");
                        if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                        {
                            if (PortfolioList != null)
                                saveModelPortfolioInstance.ModelPortfolioData = JsonConvert.SerializeObject(GetReducedPortfolioList(PortfolioList));
                            else if (_selectedModelPortfolio != null)
                                saveModelPortfolioInstance.ModelPortfolioData =
                                RebalancerCache.Instance.GetModelPortfolio(_selectedModelPortfolio.Key).ModelPortfolioData;
                        }

                    }
                    else if (SelectedPortfolioType.Key.Equals((int)RebalancerEnums.ModelPortfolioType.MasterFund))
                    {
                        if (SelectedMasterFund != null)
                            saveModelPortfolioInstance.ReferenceId = SelectedMasterFund.Key;
                        else
                            ShowErrorAlert("Master Fund not selected, first select Master Fund.");
                        if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                        {
                            if (PortfolioList != null)
                                saveModelPortfolioInstance.ModelPortfolioData = JsonConvert.SerializeObject(GetReducedPortfolioList(PortfolioList));
                            else if (_selectedModelPortfolio != null)
                                saveModelPortfolioInstance.ModelPortfolioData =
                                RebalancerCache.Instance.GetModelPortfolio(_selectedModelPortfolio.Key).ModelPortfolioData;
                        }
                    }
                    else
                    {
                        if (SelectedCustomGroup != null)
                            saveModelPortfolioInstance.ReferenceId = SelectedCustomGroup.Key;
                        else
                            ShowErrorAlert("Custom Group not selected, first select Custom Group.");
                        if (_selectedUseTolerance != null && (int)_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                        {
                            if (PortfolioList != null)
                                saveModelPortfolioInstance.ModelPortfolioData = JsonConvert.SerializeObject(GetReducedPortfolioList(PortfolioList));
                            else if (_selectedModelPortfolio != null)
                                saveModelPortfolioInstance.ModelPortfolioData =
                                RebalancerCache.Instance.GetModelPortfolio(_selectedModelPortfolio.Key).ModelPortfolioData;
                        }
                    }
                    if (SelectedPortfolioType.Key.Equals((int)RebalancerEnums.ModelPortfolioType.ModelPortfolio))
                        saveModelPortfolioInstance.PositionsType = null;
                    else
                        saveModelPortfolioInstance.PositionsType = (RebalancerEnums.RebalancerPositionsType)SelectedPositionType.Key;
                    
                    if (saveModelPortfolioInstance.PositionsType == null && SelectedModelType.Key.Equals((int)RebalancerEnums.ModelType.TargetCash))
                    {
                        saveModelPortfolioInstance.UseTolerance = null;
                        saveModelPortfolioInstance.ToleranceFactor = null;
                    }
                    else
                    {
                        saveModelPortfolioInstance.UseTolerance = (RebalancerEnums.UseTolerance)SelectedUseTolerance.Key;
                        saveModelPortfolioInstance.ToleranceFactor = (RebalancerEnums.ToleranceFactor)SelectedToleranceFactor.Key;
                    }
                    if(saveModelPortfolioInstance.UseTolerance != null)
                        saveModelPortfolioInstance.TargetPercentType = (RebalancerEnums.TargetPercentType)SelectedTargetPercentType.Key;
                    else
                        saveModelPortfolioInstance.TargetPercentType = null;

                    bool isSaved = await (Task.Run(() =>
                        RebalancerServiceApi.GetInstance().SaveEditModelPortfolio(saveModelPortfolioInstance, isEdit)));
                    if (isSaved)
                    {
                        DialogService.DialogServiceInstance.ShowMessageBox(this, "Successfully Saved.",
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Information,
                        MessageBoxResult.OK, true);
                    }
                    IsChangesMade = false;
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
        /// This Method reset the value of checkbox in the portfolio grid
        /// </summary>
        private ObservableCollection<PortfolioDto> GetReducedPortfolioList(ObservableCollection<PortfolioDto> portfolioList)
        {
            ObservableCollection<PortfolioDto> updatedPortfolioList = new ObservableCollection<PortfolioDto>();
            try
            {
                if (portfolioList == null) return portfolioList;
                if (portfolioList.Count > 0)
                {
                    foreach(var portfolio in portfolioList)
                    {
                        PortfolioDto updatedPortfolio = new PortfolioDto();
                        updatedPortfolio.Symbol = portfolio.Symbol;
                        updatedPortfolio.IsSymbolValid = portfolio.IsSymbolValid;
                        updatedPortfolio.TargetPercentage = portfolio.TargetPercentage;
                        updatedPortfolio.TolerancePercentage = portfolio.TolerancePercentage;
                        updatedPortfolioList.Add(updatedPortfolio);
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
            return updatedPortfolioList;
        }

        /// <summary>
        /// Validate all the values before saving
        /// </summary>
        /// <returns></returns>
        private bool ValidateBeforeSaving(bool isEdit)
        {
            bool isValid = true;
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrWhiteSpace(ModelPortfolioName))
                {
                    isValid = false;
                    errors.AppendLine("Model Portfolio Name can not be empty");
                }
                else if (!ValidateName(ModelPortfolioName))
                {
                    isValid = false;
                    errors.AppendLine("Model Portfolio Name is not valid.");
                }
                if (ModelPortfolioList != null && (isEdit && ModelPortfolioList.Any(modelPortfolio => modelPortfolio.Key != _selectedModelPortfolio.Key && modelPortfolio.ItemValue.ToLowerInvariant().Equals(ModelPortfolioName.ToLowerInvariant()))))
                {
                    isValid = false;
                    errors.AppendLine("Model Portfolio with the same name already Exists");
                }
                if (ModelPortfolioList != null && (!isEdit && ModelPortfolioList.Any(modelPortfolio => modelPortfolio.ItemValue.ToLowerInvariant().Equals(ModelPortfolioName.ToLowerInvariant()))))
                {
                    isValid = false;
                    errors.AppendLine("Model Portfolio with the same name already Exists");
                }
                if (SelectedPortfolioType.ItemValue.Equals("Model Portfolio"))
                {
                    if (PortfolioList == null)
                    {
                        isValid = false;
                        errors.AppendLine("Model Portfolio not available");
                    }
                    else
                    {
                        if (!(PortfolioList.Count > 0))
                        {
                            isValid = false;
                            errors.AppendLine("No information in Model Portfolio");
                        }
                        if (PortfolioList.Any(p => p.IsSymbolValid == false))
                        {
                            isValid = false;
                            errors.AppendLine("All symbols in the Model Portfolio are not valid");
                        }

                        double portfolioTargetPercentage = Math.Round((double)PortfolioList.Sum(x => (double)x.TargetPercentage), 6);

                        if (portfolioTargetPercentage > 100)
                        {
                            isValid = false;
                            errors.AppendLine("Sum of Target Percentages is greater than 100");
                        }

                        //if (portfolioTargetPercentage < 100)
                        //{
                        //    isValid = false;
                        //    errors.AppendLine("Sum of Target Percentages is less than 100");
                        //}
                    }
                    if (SelectedModelType == null)
                    {
                        isValid = false;
                        errors.AppendLine("Select Model Type");
                    }

                }

                if (SelectedPortfolioType.ItemValue.Equals("Account") && SelectedAccount == null)
                {
                    isValid = false;
                    errors.AppendLine("Select Account");
                }
                if (SelectedPortfolioType.ItemValue.Equals("Master Fund") && SelectedMasterFund == null)
                {
                    isValid = false;
                    errors.AppendLine("Select Master Fund");
                }
                if (!isValid)
                    DialogService.DialogServiceInstance.ShowMessageBox(this, errors.ToString(),
                        RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning,
                        MessageBoxResult.OK, true);
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
            return isValid;
        }
        /// <summary>
        /// Bind Format window and provide owner
        /// </summary>
        /// <param name="rebalancerMainView"></param>
        public void BindFormatWindow(RebalancerMainView rebalancerMainView)
        {
            _formatWindow = new FormatWindow(RebalancerEnums.ImportType.ModelPortfolioImport);
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
                    _formatWindow.SetFormatImage(_selectedUseTolerance, _selectedToleranceFactor, _selectedModelType);
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

        /// <summary>
        /// Import Model Portfolio
        /// </summary>
        /// <returns></returns>
        private void ImportModelPortfolio()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "Desktop";
                openFileDialog.Title = "Select File to Import";
                openFileDialog.Filter = "All (*.xls,*.xlsx,*.csv)|*.xls;*.xlsx;*.csv|Excel Files (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv";
                DataTable dt = new DataTable();
                if (openFileDialog.ShowDialog() == true)
                {
                    dt = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(openFileDialog.FileName);
                    if (dt == null)
                    {
                        MessageBox.Show("File in use! Please close the file and retry.", RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    PortfolioList = new ObservableCollection<PortfolioDto>();
                    OnPropertyChanged("PortfolioList");
                    dt.Rows[0].Delete();
                    dt.Columns[0].ColumnName = RebalancerConstants.COL_Symbol;
                    dt.Columns[1].ColumnName = RebalancerConstants.COL_Target;
                    
                    if (_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                    {
                        if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InPercentage)
                        {
                            if(dt.Columns.Count>2)
                                dt.Columns[2].ColumnName = RebalancerConstants.COL_TolerancePercentage;
                        }
                        else if (_selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InBPS)
                        {
                            if (dt.Columns.Count > 2)
                                dt.Columns[2].ColumnName = RebalancerConstants.COL_ToleranceBPS;
                        }     
                    }
                    dt.AcceptChanges();
                }
                else
                    return;


                //if (!RebalancerHelperInstance.CheckColumnsExistsInFile(dt.Columns, typeof(PortfolioDto)))
                //{
                //    DialogService.DialogServiceInstance.ShowMessageBox(this, RebalancerNewConstants.Msg_ColumnsNotFound, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, true);
                //    return;
                //}

                bool isDataValidated = true;
                dt = RemoveTheBlankRowFromDataTableAndValidateTheData(dt, ref isDataValidated);

                if (isDataValidated)
                {
                    RebalancerHelperInstance.DataTableToObservableCollection(dt, ref _portfolioList);
                    OnPropertyChanged("PortfolioList");
                    SendRequestForValidation(PortfolioList);
                    if (_selectedUseTolerance.Key == (int)RebalancerEnums.UseTolerance.Yes)
                        IsAllTradesChecked = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
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

        private DataTable RemoveTheBlankRowFromDataTableAndValidateTheData(DataTable dt, ref bool isDataValidated)
        {
            try
            {
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

                List<string> lstSymbols = new List<string>();
                List<string> lstQuantities = new List<string>();
                decimal tolernacePercentage = 0;
                int toleranceBPS = 0;

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(dt.Rows[i][RebalancerConstants.COL_Target].ToString()))
                    {
                        isDataValidated = false;
                        lstSymbols.Add(dt.Rows[i][RebalancerConstants.COL_Symbol].ToString());
                    }
                    if (string.IsNullOrWhiteSpace(dt.Rows[i][RebalancerConstants.COL_Symbol].ToString()))
                    {
                        isDataValidated = false;
                        lstQuantities.Add(dt.Rows[i][RebalancerConstants.COL_Target].ToString());
                    }
                    if (dt.Columns.Contains(RebalancerConstants.COL_TolerancePercentage))
                    {
                        if(string.IsNullOrWhiteSpace(dt.Rows[i][RebalancerConstants.COL_TolerancePercentage].ToString()))
                            dt.Rows[i][RebalancerConstants.COL_TolerancePercentage] = 0;
                        else if(decimal.TryParse(dt.Rows[i][RebalancerConstants.COL_TolerancePercentage].ToString(),out tolernacePercentage) && tolernacePercentage < 0)
                            dt.Rows[i][RebalancerConstants.COL_TolerancePercentage] = -1 * tolernacePercentage;
                    }
                    if (dt.Columns.Contains(RebalancerConstants.COL_ToleranceBPS))
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i][RebalancerConstants.COL_ToleranceBPS].ToString()))
                            dt.Rows[i][RebalancerConstants.COL_ToleranceBPS] = 0;
                        else if (int.TryParse(dt.Rows[i][RebalancerConstants.COL_ToleranceBPS].ToString(), out toleranceBPS) && toleranceBPS < 0)
                            dt.Rows[i][RebalancerConstants.COL_ToleranceBPS] = -1 * toleranceBPS;
                    }
                }
                dt.AcceptChanges();

                if (lstSymbols.Count > 0 && lstQuantities.Count > 0)
                {
                    string symbols = String.Join(", ", lstSymbols.ToArray());
                    string quantities = String.Join(", ", lstQuantities.ToArray());
                    ShowErrorAlert(String.Format("Quantity can not be blank for Symbol: {0} {1} Symbol can not be blank for Quantity: {2}", symbols, Environment.NewLine, quantities));
                }
                else if (lstSymbols.Count > 0)
                {
                    string symbols = String.Join(", ", lstSymbols.ToArray());
                    ShowErrorAlert(String.Format("Quantity can not be blank for Symbol: {0}", symbols));
                }
                else if (lstQuantities.Count > 0)
                {
                    string quantities = String.Join(", ", lstQuantities.ToArray());
                    ShowErrorAlert(String.Format("Symbol can not be blank for Quantity: {0}", quantities));
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
            return dt;
        }

        /// <summary>
        /// Sends Request for validation of symbols
        /// </summary>
        /// <param name="portfolioListForValidation"></param>
        private void SendRequestForValidation(ObservableCollection<PortfolioDto> portfolioListForValidation)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                foreach (var portfolioDto in portfolioListForValidation)
                {
                    reqObj.AddData(portfolioDto.Symbol.ToUpper(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                }
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = GetHashCode();
                if (SecurityMaster != null)
                {
                    SecurityMaster.SendRequest(reqObj);
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
        /// Data response of symbol validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecMasterClientSecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (e.Value != null &&
                    (e.Value.AssetCategory == BusinessObjects.AppConstants.AssetCategory.Equity
                    || e.Value.AssetCategory == BusinessObjects.AppConstants.AssetCategory.PrivateEquity
                    || e.Value.AssetCategory == BusinessObjects.AppConstants.AssetCategory.FixedIncome)
                    )
                {
                    PortfolioDto portfolioDto = PortfolioList.SingleOrDefault(p => p.Symbol.ToUpper().Equals(e.Value.TickerSymbol));
                    if (portfolioDto != null) portfolioDto.IsSymbolValid = true;
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
        /// Convert the portfolio type enumm to observable collection 
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<KeyValueItem> GetPortfolioTypeList()
        {
            ObservableCollection<KeyValueItem> collection = new ObservableCollection<KeyValueItem>();
            try
            {
                Type type = typeof(RebalancerEnums.ModelPortfolioType);
                var names = Enum.GetNames(type);
                int i = 1;
                foreach (var name in names)
                {
                    var field = type.GetField(name);
                    var fds = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    collection.Add(new KeyValueItem() { Key = i, ItemValue = fds[0].Description });
                    i++;
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
            return collection;
        }

        /// <summary>
        /// Add Or Update Model Portfolio through Publishing
        /// </summary>
        /// <param name="modelPortfolioDto"></param>
        internal void AddOrUpdateModelPortfolio(ModelPortfolioDto modelPortfolioDto)
        {
            try
            {
                var item = ModelPortfolioList.FirstOrDefault(i => i.Key == modelPortfolioDto.Id);
                if (item != null)
                    item.ItemValue = modelPortfolioDto.ModelPortfolioName;
                else
                    ModelPortfolioList.Add(new KeyValueItem { Key = modelPortfolioDto.Id, ItemValue = modelPortfolioDto.ModelPortfolioName });
                OnPropertyChanged("ModelPortfolioList");
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
        /// Delete Model Portfolio through publishing
        /// </summary>
        /// <param name="modelPortfolioId"></param>
        internal void DeleteModelPortfolio(int modelPortfolioId)
        {
            try
            {
                var item = ModelPortfolioList.FirstOrDefault(i => i.Key == modelPortfolioId);
                if (item != null)
                    ModelPortfolioList.Remove(item);
                OnPropertyChanged("ModelPortfolioList");
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
                if (_portfolioList != null)
                    _portfolioList = null;

            if (_portfolioTypeList != null)
                _portfolioTypeList = null;

            if (_accounts != null)
                _accounts = null;

            if (_masterFunds != null)
                _masterFunds = null;

            if (_modelPortfolioList != null)
                _modelPortfolioList = null;

            if (_selectedAccount != null)
                _selectedAccount = null;
            if (_selectedMasterFund != null)
                _selectedMasterFund = null;
            if (_selectedPortfolioType != null)
                _selectedPortfolioType = null;

            if (_positionTypeList != null)
                _positionTypeList = null;

            _selectedPositionType = null;
            _selectedModelPortfolio = null;
            _selectedUseTolerance = null;

            _statusAndErrorMessages = null;
            if (SecurityMaster != null)
            {
                SecurityMaster.SecMstrDataResponse -= SecMasterClientSecMstrDataResponse;
                SecurityMaster = null;
                }
                RebalancerHelperInstance = null;
                if (_formatWindow != null)
                {
                    _formatWindow.Closing -= FormatWindow_Closing;
                    _formatWindow.Close();
                }
                InstanceManager.ReleaseInstance(typeof(ModelPortfolioViewModel));
            }
        }

        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void SetToleranceMethod()
        {
            try
            {
                bool isBPS = _selectedToleranceFactor.Key == (int)RebalancerEnums.ToleranceFactor.InBPS;
                if (PortfolioList != null)
                {
                    foreach (var item in PortfolioList)
                    {
                        if (item.IsChecked)
                        {
                            if (isBPS)
                            {
                                if (item.ToleranceBPS != int.Parse(TolrenaceinBPS))
                                {
                                    item.ToleranceBPS = int.Parse(TolrenaceinBPS);
                                    IsChangesMade = true;
                                }
                            }
                            else
                            {
                                if (item.TolerancePercentage != decimal.Parse(TolrenaceinPercentage))
                                {
                                    item.TolerancePercentage = decimal.Parse(TolrenaceinPercentage);
                                    IsChangesMade = true;
                                }
                            }
                        }
                    }
                }
                TolrenaceinPercentage = "0.00";
                TolrenaceinBPS = "0";
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

        private void ClearToleranceMethod()
        {
            try
            {
                if (PortfolioList != null)
                {
                    foreach (var item in PortfolioList)
                    {
                        if (item.IsChecked)
                        {
                            if (item.TolerancePercentage != (decimal)0.00)
                            {
                                item.TolerancePercentage = (decimal)0.00;
                                IsChangesMade = true;
                            }
                            if (item.ToleranceBPS != 0)
                            {
                                item.ToleranceBPS = 0;
                                IsChangesMade = true;
                            }
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
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (ExportModelPortfolioForAutomation == true)
                ExportModelPortfolioForAutomation = false;
            ExportFilePathForAutomation = filePath;
            ExportModelPortfolioForAutomation = true;
        }

        #endregion

    }
}
