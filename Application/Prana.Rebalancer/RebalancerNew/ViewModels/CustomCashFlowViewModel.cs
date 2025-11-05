using Prana.BusinessObjects;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms.Integration;


namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class CustomCashFlowViewModel : BindableBase
    {

        /// <summary>
        /// Command executed when close button on window clicked
        /// </summary>
        public DelegateCommand CloseButtonClickedCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        public DelegateCommand OpenFormatCommand { get; set; }
        /// <summary>
        /// Check if Ok button clicked 
        /// </summary>
        public bool IsOkButtonClicked { get; set; }

        public FormatWindow _formatWindow;

        private decimal _customCashFlow;

        public decimal CustomCashFlow
        {
            get { return _customCashFlow; }
            set
            {
                _customCashFlow = value;
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

        public Dictionary<int, decimal> CustomCashFlowDictionary { get; set; }

        public CustomCashFlowViewModel()
        {
            OpenFormatCommand = new DelegateCommand(() => OpenFormatCommandAction());
            ImportCommand = new DelegateCommand(() => ImportCommandAction());
            CustomCashFlowDictionary = null;
            IsOkButtonClicked = false;
        }

        /// <summary>
        /// Method to do action on ImportCommand
        /// </summary>
        private void ImportCommandAction()
        {
            try
            {
                IImportFactory importFactoryInstance = new ImportFactory();
                string error = string.Empty;
                IImport importInstance = importFactoryInstance.CreateObject(RebalancerEnums.ImportType.CashFlowImport, ref error);
                if (string.IsNullOrWhiteSpace(error))
                {
                    ImportModel model = new ImportModel
                    {
                        Accounts = new HashSet<string>()
                    };
                    foreach (var val in NAVGridData)
                    {
                        if (!model.Accounts.Contains(val.AccountName))
                            model.Accounts.Add(val.AccountName);
                    }
                    List<ImportCashFlowModel> lst = importInstance.ValidateAndGetData<ImportCashFlowModel>(model);
                    if (lst == null)
                    {
                        ShowErrorAlert("Imported data is of wrong format.");
                        return;
                    }
                    ImportViewModel importViewModelInstance = new ImportViewModel(RebalancerEnums.ImportType.CashFlowImport);
                    ImportView importView = new ImportView(RebalancerEnums.ImportType.CashFlowImport)
                    {
                        DataContext = importViewModelInstance
                    };
                    importViewModelInstance.SetUp<ImportCashFlowModel>(lst);
                    importView.Owner = _formatWindow.Owner;
                    importView.ShowInTaskbar = true;
                    importView.ShowDialog();
                    if (importViewModelInstance.IsSaveClicked)
                    {
                        BindCashFlowImport(importViewModelInstance);

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

        /// <summary>
        /// Bind Cash flow Import
        /// </summary>
        /// <param name="importViewModelInstance"></param>
        private void BindCashFlowImport(ImportViewModel importViewModelInstance)
        {
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            foreach (ImportCashFlowModel importCashFlowModel in importViewModelInstance.CashFlowGrid)
            {
                if (importCashFlowModel.IsValid && !dict.ContainsKey(importCashFlowModel.AccountName))
                {
                    dict.Add(importCashFlowModel.AccountName, importCashFlowModel.Cash);
                }
            }

            foreach (AdjustedAccountLevelNAV adjustedAccountLevelNAV in NAVGridData)
            {
                if (dict.ContainsKey(adjustedAccountLevelNAV.AccountName))
                {
                    adjustedAccountLevelNAV.CustomCashFlow = dict[adjustedAccountLevelNAV.AccountName];
                }
            }
        }

        /// <summary>
        /// Show Alert Error.
        /// </summary>
        /// <param name="msg"></param>
        public void ShowErrorAlert(string msg)
        {
            MessageBox.Show(msg, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION,
                       MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        internal void AssignCurrentTotalNavToCashFlow()
        {
            try
            {
                foreach (var adjustedNav in NAVGridData)
                {
                    adjustedNav.CustomCashFlow = adjustedNav.CurrentTotalNAV;
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

        /// <summary>
        /// Bind Format window and provide owner
        /// </summary>
        /// <param name="owner"></param>
        public void BindFormatWindow(Window owner)
        {
            _formatWindow = new FormatWindow(RebalancerEnums.ImportType.CashFlowImport);
            ElementHost.EnableModelessKeyboardInterop(_formatWindow);
            _formatWindow.Owner = owner;
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
        /// Changes done in cashflow stored in dictionary and used by rebalancerviewmodel
        /// </summary>
        internal void AddDataToCashFlowDictionary()
        {
            try
            {
                //If window is closed by clicking on close button then Cash Flow is not sent to Rebalancer View Model
                if (!IsOkButtonClicked) return;
                CustomCashFlowDictionary = new Dictionary<int, decimal>();
                foreach (var adjustedNav in NAVGridData)
                {
                    CustomCashFlowDictionary[adjustedNav.AccountId] = adjustedNav.CustomCashFlow;
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
        /// Dispose 
        /// </summary>
        internal void Dispose()
        {
            if (_formatWindow != null)
            {
                _formatWindow.Closing -= FormatWindow_Closing;
            }
            _formatWindow = null;

        }

    }
}
