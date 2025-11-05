using ExportGridsData;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class UpdatePreferenceUIViewModel : ViewModelBase, IModalDialogViewModel, IExportGridData
    {
        #region Events

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The preference account strategy control view model
        /// </summary>
        private PreferenceAccountStrategyControlViewModel _preferenceAccountStrategyControlViewModel;

        /// <summary>
        /// The calculated preference default rule control view model
        /// </summary>
        private CalculatedPreferenceDefaultRuleControlViewModel _calculatedPreferenceDefaultRuleControlViewModel;

        /// <summary>
        /// The calculated preference General rule control view model
        /// </summary>
        private CalculatedPreferenceGeneralRuleControlViewModel _calculatedPreferenceGeneralRuleControlViewModel;

        /// <summary>
        /// The _status bar text
        /// </summary>
        private string _statusBarText;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        /// <summary>
        /// The check list wise preference
        /// </summary>
        CheckListWisePreference _checkListWisePreference = new CheckListWisePreference();

        /// <summary>
        /// The account list
        /// </summary>
        public List<int> _accountList;


        /// <summary>
        /// The Visibilty of masterfund
        /// </summary>
        private Visibility _isVisibleButtonForMF;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        /// <summary>
        /// Gets or sets the calculated preference default rule control view model.
        /// </summary>
        /// <value>
        /// The calculated preference default rule control view model.
        /// </value>
        public CalculatedPreferenceDefaultRuleControlViewModel CalculatedPreferenceDefaultRuleControlViewModel
        {
            get { return _calculatedPreferenceDefaultRuleControlViewModel; }
            set
            {
                _calculatedPreferenceDefaultRuleControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferenceDefaultRuleControlViewModel");
            }
        }

        /// <summary>
        /// Gets the dialog result value, which is the value that is returned from the
        /// <see cref="IDialogService.ShowDialog" /> and <see cref="IDialogService.ShowDialog{T}" />
        /// methods.
        /// </summary>
        public bool? DialogResult
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the preference account strategy control view model.
        /// </summary>
        /// <value>
        /// The preference account strategy control view model.
        /// </value>
        public PreferenceAccountStrategyControlViewModel PreferenceAccountStrategyControlViewModel
        {
            get { return _preferenceAccountStrategyControlViewModel; }
            set
            {
                _preferenceAccountStrategyControlViewModel = value;
                RaisePropertyChangedEvent("PreferenceAccountStrategyControlViewModel");
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
                _statusBarText = value;
                RaisePropertyChangedEvent("StatusBarText");
            }
        }


        /// <summary>
        /// Gets or sets the is visible button for mf.
        /// </summary>
        /// <value>
        /// The is visible button for mf.
        /// </value>
        public Visibility IsVisibleButtonForMF
        {
            get { return _isVisibleButtonForMF; }
            set
            {
                _isVisibleButtonForMF = value;
                RaisePropertyChangedEvent("IsVisibleButtonForMF");
            }
        }





        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the update add or update preference data.
        /// </summary>
        /// <value>
        /// The update add or update preference data.
        /// </value>
        public RelayCommand<object> UpdatePreferenceDataCommand { get; set; }

        /// <summary>
        /// Gets or sets the update preference UI closing.
        /// </summary>
        /// <value>
        /// The update preference UI closing.
        /// </value>
        public RelayCommand<object> UpdatePreferenceUIClosing { get; set; }

        /// <summary>
        /// Gets or sets the close add or update preference.
        /// </summary>
        /// <value>
        /// The close add or update preference.
        /// </value>
        public RelayCommand<object> CloseUpdatePreferenceUICommand { get; set; }

        /// <summary>
        /// Gets or sets the add or update preferences UI loaded.
        /// </summary>
        /// <value>
        /// The add or update preferences UI loaded.
        /// </value>
        public RelayCommand<object> UpdatePreferenceUILoaded { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePreferenceUIViewModel" /> class.
        /// </summary>
        public UpdatePreferenceUIViewModel()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePreferenceUIViewModel"/> class.
        /// </summary>
        /// <param name="selectedCheckList">The selected check list.</param>
        public UpdatePreferenceUIViewModel(CheckListWisePreference selectedCheckList, Dictionary<int, string> accountslist, Visibility IsVisibleButtonForMF)
        {
            try
            {
                _checkListWisePreference = selectedCheckList;
                UpdatePreferenceUILoaded = new RelayCommand<object>((parameter) => OnLoadUpdatePreferenceUI(parameter, accountslist, IsVisibleButtonForMF));
                UpdatePreferenceUIClosing = new RelayCommand<object>((parameter) => OnUpdatePreferenceUIClosing(parameter));
                UpdatePreferenceDataCommand = new RelayCommand<object>((parameter) => UpdatePreferenceData(parameter));
                CloseUpdatePreferenceUICommand = new RelayCommand<object>((parameter) => CloseUpdatePreferenceUI(parameter));
                _preferenceAccountStrategyControlViewModel = new PreferenceAccountStrategyControlViewModel();
                _calculatedPreferenceDefaultRuleControlViewModel = new CalculatedPreferenceDefaultRuleControlViewModel();
                _calculatedPreferenceDefaultRuleControlViewModel.IsVisibleButtonForMF = IsVisibleButtonForMF;
                _calculatedPreferenceGeneralRuleControlViewModel = new CalculatedPreferenceGeneralRuleControlViewModel();
                _calculatedPreferenceGeneralRuleControlViewModel.IsVisibleButtonForMF = IsVisibleButtonForMF;
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
        /// Closes the add or update preference UI.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object CloseUpdatePreferenceUI(object parameter)
        {
            try
            {
                Window allocationPreferenceWindow = parameter as Window;
                if (allocationPreferenceWindow != null)
                    allocationPreferenceWindow.Close();
                InstanceManager.ReleaseInstance(typeof(UpdatePreferenceUIViewModel));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnUpdatePreferenceUIClosing(object parameter)
        {
            try
            {
                CheckListWisePreference updatedCheckList = GetUpdatedCheckListWisePref();
                string errorMessage = string.Empty;
                if (!(updatedCheckList.Rule.IsValid(out errorMessage) || _checkListWisePreference.Rule.Equals(updatedCheckList.Rule) && (updatedCheckList.IsTargetPercentageValid(out errorMessage, updatedCheckList.Rule, updatedCheckList.TargetPercentage) || _checkListWisePreference.IsTargetPercentageEqual(updatedCheckList.TargetPercentage))))
                {
                    MessageBoxResult result = MessageBox.Show("There are some changes. Do you want to Update?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        UpdatePreferenceData(parameter);
                    }
                }
                CancelEventArgs e = parameter as CancelEventArgs;
                if (!e.Cancel && OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load add or update preference UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnLoadUpdatePreferenceUI(object parameter, Dictionary<int, string> accountsList, Visibility _isVisibleButtonForMF)
        {
            try
            {
                AllocationCompanyWisePref pref = AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences();
                _calculatedPreferenceDefaultRuleControlViewModel.OnLoadCalculatedPreferenceDefaultRuleControl(accountsList);
                _preferenceAccountStrategyControlViewModel.OnLoadPreferenceAccountStrategyControl(accountsList, pref);
                _accountList = new List<int>(accountsList.Keys.ToList());
                SetUpUpdatePreferenceUI();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Sets up update preference UI.
        /// </summary>
        /// <param name="accountsList">The accounts list.</param>
        private void SetUpUpdatePreferenceUI()
        {
            try
            {

                SetCalculatedDefaultRuleControl();
                SetAccountStrategyGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the account strategy grid.
        /// </summary>
        private void SetAccountStrategyGrid()
        {
            try
            {
                _preferenceAccountStrategyControlViewModel.SetAccountStrategyGrid(DeepCopyHelper.Clone(_checkListWisePreference.TargetPercentage), _accountList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the calculated default rule control.
        /// </summary>
        private void SetCalculatedDefaultRuleControl()
        {
            try
            {
                if (_checkListWisePreference.Rule != null)
                {
                    AllocationRule defaultRule = _checkListWisePreference.Rule.Clone();
                    _calculatedPreferenceDefaultRuleControlViewModel.SetCalculatedDefaultRuleControl(defaultRule);
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
        /// Updates the add or update preference.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object UpdatePreferenceData(object parameter)
        {
            try
            {
                CancelEventArgs e = parameter as CancelEventArgs;
                StatusBarText = string.Empty;
                CheckListWisePreference updatedCheckList = GetUpdatedCheckListWisePref();

                if (_checkListWisePreference.Rule != null)
                {
                    if (!(_checkListWisePreference.Rule.Equals(updatedCheckList.Rule) && _checkListWisePreference.IsTargetPercentageEqual(updatedCheckList.TargetPercentage)))
                    {
                        string errorMessage = string.Empty;
                        if (updatedCheckList.Rule.IsValid(out errorMessage) && updatedCheckList.IsTargetPercentageValid(out errorMessage, updatedCheckList.Rule, updatedCheckList.TargetPercentage))
                        {
                            _checkListWisePreference.TryUpdateDefaultRule(updatedCheckList.Rule);
                            _checkListWisePreference.TryUpdateTargetPercentage(updatedCheckList.TargetPercentage);
                            StatusBarText = "Preference rule updated successfully.";
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show(string.Format(" Invalid Preference: {0}\n Do you want to revert this preference?", errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                SetUpUpdatePreferenceUI();
                            }
                            else
                            {
                                if (e != null)
                                    e.Cancel = true;
                            }
                        }
                    }
                    else
                    {
                        StatusBarText = "Nothing to update.";
                    }
                }
                else
                {
                    StatusBarText = "Not saved as no default rule exist for this preference.";
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Gets the updated check list wise preference.
        /// </summary>
        /// <returns></returns>
        private CheckListWisePreference GetUpdatedCheckListWisePref()
        {
            CheckListWisePreference updatedCheckList = _checkListWisePreference.Clone();
            try
            {
                if (_calculatedPreferenceDefaultRuleControlViewModel != null)
                    updatedCheckList.TryUpdateDefaultRule(_calculatedPreferenceDefaultRuleControlViewModel.DefaultRule);
                updatedCheckList.TryUpdateTargetPercentage(_preferenceAccountStrategyControlViewModel.AccountValueDictionary);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedCheckList;
        }

        #endregion Methods

        #region IDisposable Memebers

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                    _preferenceAccountStrategyControlViewModel.Dispose();
                    _calculatedPreferenceDefaultRuleControlViewModel.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (this._preferenceAccountStrategyControlViewModel.ExportAccountAndStrategyGrid == true)
                _preferenceAccountStrategyControlViewModel.ExportAccountAndStrategyGrid = false;
            _preferenceAccountStrategyControlViewModel.ExportAccountAndStrategyGrid = true;
        }
        #endregion
    }
}
