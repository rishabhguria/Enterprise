using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Preferences.ViewModels;
using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Data;
using System.Windows;

namespace Prana.Allocation.Client.Forms.ViewModels
{
    public class AllocationPreferencesUIViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _gerneral preferences view model
        /// </summary>
        private GeneralPreferencesViewModel _gerneralPreferencesViewModel;

        /// <summary>
        /// The _attribute renaming control view model
        /// </summary>
        private AttributeRenamingControlViewModel _attributeRenamingControlViewModel;

        /// <summary>
        /// The _auto group rules control view model
        /// </summary>
        private AutoGroupRulesControlViewModel _autoGroupRulesControlViewModel;

        /// <summary>
        /// The _status bar text
        /// </summary>
        private string _statusBarText;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the attribute renaming control view model.
        /// </summary>
        /// <value>
        /// The attribute renaming control view model.
        /// </value>
        public AttributeRenamingControlViewModel AttributeRenamingControlViewModel
        {
            get { return _attributeRenamingControlViewModel; }
            set
            {
                _attributeRenamingControlViewModel = value;
                RaisePropertyChangedEvent("AttributeRenamingControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the automatic group rules control view model.
        /// </summary>
        /// <value>
        /// The automatic group rules control view model.
        /// </value>
        public AutoGroupRulesControlViewModel AutoGroupRulesControlViewModel
        {
            get { return _autoGroupRulesControlViewModel; }
            set
            {
                _autoGroupRulesControlViewModel = value;
                RaisePropertyChangedEvent("AutoGroupRulesControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the gerneral preferences view model.
        /// </summary>
        /// <value>
        /// The gerneral preferences view model.
        /// </value>
        public GeneralPreferencesViewModel GerneralPreferencesViewModel
        {
            get { return _gerneralPreferencesViewModel; }
            set
            {
                _gerneralPreferencesViewModel = value;
                RaisePropertyChangedEvent("GerneralPreferencesViewModel");
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

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the allocation preferences save.
        /// </summary>
        /// <value>
        /// The allocation preferences save.
        /// </value>
        public RelayCommand<object> AllocationPreferencesSave { get; set; }

        /// <summary>
        /// Gets or sets the allocation preferences UI loaded command.
        /// </summary>
        /// <value>
        /// The allocation preferences UI loaded command.
        /// </value>
        public RelayCommand<object> AllocationPreferencesUILoadedCommand { get; set; }

        /// <summary>
        /// Gets or sets the close allocation preferences.
        /// </summary>
        /// <value>
        /// The close allocation preferences.
        /// </value>
        public RelayCommand<object> CloseAllocationPreferences { get; set; }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public RelayCommand<object> FormCloseButton { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPreferencesUIViewModel"/> class.
        /// </summary>
        public AllocationPreferencesUIViewModel()
        {
            try
            {
                AllocationPreferencesUILoadedCommand = new RelayCommand<object>((parameter) => OnLoadAllocationPreferencesUI(parameter));
                AllocationPreferencesSave = new RelayCommand<object>((parameter) => SavePreferences(parameter));
                CloseAllocationPreferences = new RelayCommand<object>((parameter) => CloseAllocationPreferencesWindow(parameter));
                FormCloseButton = new RelayCommand<object>((parameter) => OnCloseButton(parameter));
                _gerneralPreferencesViewModel = new GeneralPreferencesViewModel();
                _attributeRenamingControlViewModel = new AttributeRenamingControlViewModel();
                _autoGroupRulesControlViewModel = new AutoGroupRulesControlViewModel();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Closes the allocation preferences window.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object CloseAllocationPreferencesWindow(object parameter)
        {
            try
            {
                Window allocationPreferenceWindow = parameter as Window;
                allocationPreferenceWindow.Close();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Gets the allocation company wise preferences.
        /// </summary>
        /// <returns></returns>
        private AllocationCompanyWisePref GetAllocationCompanyWisePreferences()
        {
            AllocationCompanyWisePref companyWisePreference = new AllocationCompanyWisePref();
            try
            {
                companyWisePreference = GerneralPreferencesViewModel.GetCompanyWisePreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return companyWisePreference;
        }

        /// <summary>
        /// Gets the allocation user wise preferences.
        /// </summary>
        /// <returns></returns>
        private AllocationPreferences GetAllocationUserWisePreferences()
        {
            AllocationPreferences allocationPreferences = new AllocationPreferences();
            try
            {
                allocationPreferences.AutoGroupingRules = AutoGroupRulesControlViewModel.GetAutoGroupingRules();
                allocationPreferences.GeneralRules = GerneralPreferencesViewModel.GetUserWisePreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationPreferences;
        }

        /// <summary>
        /// Gets the attribute names data set.
        /// </summary>
        /// <returns></returns>
        private DataSet GetAttributeNamesDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                ds.Tables.Add(AttributeRenamingControlViewModel.AttributeRenamingCollection.Copy());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return ds;
        }


        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                if (_attributeRenamingControlViewModel != null)
                {
                    _attributeRenamingControlViewModel.Dispose();
                }
                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);             
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load allocation preferences UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnLoadAllocationPreferencesUI(object parameter)
        {
            try
            {
                // load general preferences
                AllocationPreferences userWisePreferences = AllocationClientPreferenceManager.GetInstance.GetUserWisePreferences();
                AllocationCompanyWisePref allocationCompanyWisePref = AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences();
                _gerneralPreferencesViewModel.OnLoadGeneralPreferencesControl(userWisePreferences, allocationCompanyWisePref);

                //load trade attributes
                _attributeRenamingControlViewModel.OnLoadAttributeRenamingControl();

                // load auto grouping rules
                _autoGroupRulesControlViewModel.OnLoadAutoGroupRulesControl(userWisePreferences);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Saves the preferences.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object SavePreferences(object parameter)
        {
            try
            {
                bool isValid = false;
                bool isSaved = false;
                StatusBarText = string.Empty;

                //Get Preferences
                AllocationCompanyWisePref allocationCompanyWisePref = GetAllocationCompanyWisePreferences();
                string errorMsg = string.Empty;
                isValid = allocationCompanyWisePref.IsValid(out errorMsg);
                if (isValid)
                {
                    DataSet attributeNameDataSet = GetAttributeNamesDataSet();
                    AllocationPreferences allocationPreferences = GetAllocationUserWisePreferences();

                    //Save Preferences
                    isSaved = AllocationClientPreferenceManager.GetInstance.SaveAllocationGeneralPreferences(allocationCompanyWisePref, attributeNameDataSet, allocationPreferences);
                }
                else
                {
                    MessageBox.Show(errorMsg.Trim(), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    StatusBarText = "Preferences Not Saved";
                }
                StatusBarText = isSaved ? "Preferences Saved" : "Preferences Not Saved";
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
