using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.EventArguments;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class CalculatedAllocationPreferenceControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [apply preference bulk change event].
        /// </summary>
        public event ApplyBulkChangeHandler ApplyPreferenceBulkChangeEvent;

        /// <summary>
        /// Occurs when [preference event].
        /// </summary>
        public event EventHandler<EventArgs<AllocationPrefOperationEventArgs>> PreferenceEvent;

        /// <summary>
        /// Occurs when [preview preference event].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> PreviewPreferenceEvent;

        /// <summary>
        /// Occurs when [update preference cache event].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> UpdatePreferenceCacheEvent;

        #endregion Events

        #region Delegates

        /// <summary>
        /// bulk change handler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        public delegate void ApplyBulkChangeHandler(Object sender, ApplyBulkChangeEventArgs e);

        #endregion Delegates

        #region Members

        /// <summary>
        /// The _calculated preferences list control view model
        /// </summary>
        private CalculatedPreferencesListControlViewModel _calculatedPreferencesListControlViewModel;

        /// <summary>
        /// The _calculated preference default rule control view model
        /// </summary>
        private CalculatedPreferenceDefaultRuleControlViewModel _calculatedPreferenceDefaultRuleControlViewModel;

        /// <summary>
        /// The _calculated preference general rule control view model
        /// </summary>
        private CalculatedPreferenceGeneralRuleControlViewModel _calculatedPreferenceGeneralRuleControlViewModel;

        /// <summary>
        /// The _preference Account strategy control view model
        /// </summary>
        private PreferenceAccountStrategyControlViewModel _preferenceAccountStrategyControlViewModel;

        /// <summary>
        /// The _bulk change control view model
        /// </summary>
        private BulkChangeControlViewModel _bulkChangeControlViewModel;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the bulk change control view model.
        /// </summary>
        /// <value>
        /// The bulk change control view model.
        /// </value>
        public BulkChangeControlViewModel BulkChangeControlViewModel
        {
            get { return _bulkChangeControlViewModel; }
            set
            {
                _bulkChangeControlViewModel = value;
                RaisePropertyChangedEvent("BulkChangeControlViewModel");
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
        /// Gets or sets the calculated preference general rule control view model.
        /// </summary>
        /// <value>
        /// The calculated preference general rule control view model.
        /// </value>
        public CalculatedPreferenceGeneralRuleControlViewModel CalculatedPreferenceGeneralRuleControlViewModel
        {
            get { return _calculatedPreferenceGeneralRuleControlViewModel; }
            set
            {
                _calculatedPreferenceGeneralRuleControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferenceGeneralRuleControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the calculated preferences list control view model.
        /// </summary>
        /// <value>
        /// The calculated preferences list control view model.
        /// </value>
        public CalculatedPreferencesListControlViewModel CalculatedPreferencesListControlViewModel
        {
            get { return _calculatedPreferencesListControlViewModel; }
            set
            {
                _calculatedPreferencesListControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferencesListControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the preference Account strategy control view model.
        /// </summary>
        /// <value>
        /// The preference Account strategy control view model.
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

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedAllocationPreferenceControlViewModel"/> class.
        /// </summary>
        public CalculatedAllocationPreferenceControlViewModel()
        {
            try
            {
                _calculatedPreferencesListControlViewModel = new CalculatedPreferencesListControlViewModel();
                _calculatedPreferenceDefaultRuleControlViewModel = new CalculatedPreferenceDefaultRuleControlViewModel();
                _calculatedPreferenceGeneralRuleControlViewModel = new CalculatedPreferenceGeneralRuleControlViewModel();
                _preferenceAccountStrategyControlViewModel = new PreferenceAccountStrategyControlViewModel();
                _calculatedPreferencesListControlViewModel.PreviewPreferenceEvent += _calculatedPreferencesListControlViewModel_PreviewPreference;
                _calculatedPreferencesListControlViewModel.PreferenceEvent += _calculatedPreferencesListControlViewModel_PreferenceEvent;
                _calculatedPreferencesListControlViewModel.UpdatePreferenceCacheEvent += _calculatedPreferencesListControlViewModel_UpdatePreferenceCacheEvent;
                _calculatedPreferencesListControlViewModel.PreferenceBulkChangeEvent += CalculatedAllocationPreferenceControlViewModel_PreferenceBulkChangeEvent;
                _calculatedPreferenceGeneralRuleControlViewModel.AddNewPreference += _calculatedPreferenceGeneralRuleControlViewModel_AddNewPreference;
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

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Handles the AddNewPreference event of the _calculatedPreferenceGeneralRuleControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void _calculatedPreferenceGeneralRuleControlViewModel_AddNewPreference(object sender, EventArgs e)
        {
            try
            {
                _calculatedPreferenceGeneralRuleControlViewModel.AddNewCheckListWisePreference(_calculatedPreferenceDefaultRuleControlViewModel.DefaultRule, _preferenceAccountStrategyControlViewModel.AccountValueDictionary);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PreferenceEvent event of the _calculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationPreferencesConstants.AllocationPrefOperation}"/> instance containing the event data.</param>
        private void _calculatedPreferencesListControlViewModel_PreferenceEvent(object sender, EventArgs<AllocationPrefOperationEventArgs> e)
        {
            try
            {
                if (PreferenceEvent != null)
                    PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(e.Value));
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
        /// Handles the PreviewPreference event of the _calculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Global.EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _calculatedPreferencesListControlViewModel_PreviewPreference(object sender, Global.EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                if (PreviewPreferenceEvent != null)
                    PreviewPreferenceEvent(this, e);
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
        /// Handles the UpdatePreferenceCacheEvent event of the _calculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _calculatedPreferencesListControlViewModel_UpdatePreferenceCacheEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                if (UpdatePreferenceCacheEvent != null)
                    UpdatePreferenceCacheEvent(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Adds the remove update preference collection.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="preferenceName">Name of the preference.</param>
        /// <param name="oldPrefKey">The old preference key.</param>
        internal void AddRemoveUpdatePrefCollection(int preferenceId, string preferenceName, int oldPrefKey)
        {
            try
            {
                _calculatedPreferencesListControlViewModel.AddRemoveUpdatePrefCollection(preferenceId, preferenceName, oldPrefKey);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the PreferenceBulkChangeEvent event of the _bulkChangeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CalculatedAllocationPreferenceControlViewModel_PreferenceBulkChangeEvent(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                if (ApplyPreferenceBulkChangeEvent != null)
                    ApplyPreferenceBulkChangeEvent(this, e);
                if ((_calculatedPreferencesListControlViewModel.SelectedAllocationPreferences != null && (e.ApplyOnSelectedPref && e.PreferenceList.Contains(((DictionaryImpersonation<int, string>)_calculatedPreferencesListControlViewModel.SelectedAllocationPreferences).Key))) || !e.ApplyOnSelectedPref)
                {
                    if (e.ApplyOnDefaultRule)
                        _calculatedPreferenceDefaultRuleControlViewModel.ApplyBulkChange(e);
                    _calculatedPreferenceGeneralRuleControlViewModel.ApplyBulkChanges(e);
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
        /// Deletes the preference from collection.
        /// </summary>
        /// <param name="prefKey">The preference key.</param>
        internal void DeletePrefFromCollection(int prefKey)
        {
            try
            {
                _calculatedPreferencesListControlViewModel.DeletePrefFromCollection(prefKey);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the calculated preference default rule control.
        /// </summary>
        internal void LoadCalculatedPreferenceDefaultRuleControl()
        {
            try
            {
                _calculatedPreferenceDefaultRuleControlViewModel.OnLoadCalculatedPreferenceDefaultRuleControl(CachedDataManager.GetInstance.GetUserAccountsAsDict());
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
        /// Loads the calculated preference general rule control.
        /// </summary>
        internal void LoadCalculatedPreferenceGeneralRuleControl()
        {
            try
            {
                _calculatedPreferenceGeneralRuleControlViewModel.OnLoadCalculatedPreferenceGeneralRuleControl();
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
        /// Loads the calculated preferences list control.
        /// </summary>
        internal void LoadCalculatedPreferencesListControl()
        {
            try
            {
                Dictionary<int, string> preferenceList = AllocationClientPreferenceManager.GetInstance.GetPreferencesList();
                _calculatedPreferencesListControlViewModel.OnLoadCalculatedPreferencesListControl(preferenceList);
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
        /// Loads the preference Account strategy control.
        /// </summary>
        internal void LoadPreferenceAccountStrategyControl(AllocationCompanyWisePref pref)
        {
            try
            {
                _preferenceAccountStrategyControlViewModel.OnLoadPreferenceAccountStrategyControl(CachedDataManager.GetInstance.GetUserAccountsAsDict(), pref);
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
        /// Called when [load calculated allocation preference control].
        /// </summary>
        internal void OnLoadCalculatedAllocationPreferenceControl(AllocationCompanyWisePref pref)
        {
            try
            {
                LoadCalculatedPreferencesListControl();
                LoadCalculatedPreferenceDefaultRuleControl();
                LoadCalculatedPreferenceGeneralRuleControl();
                LoadPreferenceAccountStrategyControl(pref);
                EnableDisablePrefControl(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the Account strategy grid.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        private void SetAccountStrategyGrid(AllocationOperationPreference allocationOperationPref)
        {
            try
            {
                if (allocationOperationPref != null)
                    PreferenceAccountStrategyControlViewModel.SetAccountStrategyGrid(allocationOperationPref.TargetPercentage, CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.ToList());
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
        /// Sets the calculated default rule control.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        private void SetCalculatedDefaultRuleControl(AllocationRule defaultRule)
        {
            try
            {
                _calculatedPreferenceDefaultRuleControlViewModel.SetCalculatedDefaultRuleControl(defaultRule);
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
        /// Sets the calculated general rule control.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        private void SetCalculatedGeneralRuleControl(AllocationOperationPreference allocationOperationPref)
        {
            try
            {
                _calculatedPreferenceGeneralRuleControlViewModel.CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>(allocationOperationPref.CheckListWisePreference.Values);
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
        /// Shows the allocation preference.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        internal void ShowAllocationPreference(AllocationOperationPreference allocationOperationPref)
        {
            try
            {
                SetAccountStrategyGrid(allocationOperationPref);
                SetCalculatedDefaultRuleControl(allocationOperationPref.DefaultRule);
                SetCalculatedGeneralRuleControl(allocationOperationPref);
                EnableDisablePrefControl(true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the allocationpreference with UI values.
        /// </summary>
        /// <param name="selectedOperationPreference">The selected operation preference.</param>
        internal void UpdateAllocationpreferenceWithUIValues(AllocationOperationPreference selectedOperationPreference)
        {
            try
            {
                selectedOperationPreference.TryUpdateDefaultRule(CalculatedPreferenceDefaultRuleControlViewModel.DefaultRule);
                SerializableDictionary<int, CheckListWisePreference> checkListWisePrefrences = CalculatedPreferenceGeneralRuleControlViewModel.GetCheckListWisePreferencesDictionary();
                if (checkListWisePrefrences != null)
                    selectedOperationPreference.UpdateCheckList(checkListWisePrefrences);
                selectedOperationPreference.TryUpdateTargetPercentage(PreferenceAccountStrategyControlViewModel.GetAccountStrategyValues());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Enables the disable preference control.
        /// </summary>
        /// <param name="isEnable">if set to <c>true</c> [is enable].</param>
        internal void EnableDisablePrefControl(bool isEnable)
        {
            try
            {
                if (!isEnable)
                {
                    _preferenceAccountStrategyControlViewModel.ClearAccountStrategyTable();
                    _calculatedPreferenceGeneralRuleControlViewModel.CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>();
                }
                _preferenceAccountStrategyControlViewModel.IsPrefAccountStrategyEnabled = isEnable;
                _calculatedPreferenceDefaultRuleControlViewModel.IsPrefDefaultRuleControlEnabled = isEnable;
                _calculatedPreferenceGeneralRuleControlViewModel.IsPrefGeneralRuleControlEnabled = isEnable;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion Methods

        #region Dispose Methods and Unwire Events

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
                    if (_calculatedPreferencesListControlViewModel != null)
                    {
                        _calculatedPreferencesListControlViewModel.PreviewPreferenceEvent -= _calculatedPreferencesListControlViewModel_PreviewPreference;
                        _calculatedPreferencesListControlViewModel.PreferenceEvent -= _calculatedPreferencesListControlViewModel_PreferenceEvent;
                        _calculatedPreferencesListControlViewModel.UpdatePreferenceCacheEvent -= _calculatedPreferencesListControlViewModel_UpdatePreferenceCacheEvent;
                        _calculatedPreferencesListControlViewModel.PreferenceBulkChangeEvent -= CalculatedAllocationPreferenceControlViewModel_PreferenceBulkChangeEvent;
                    }

                    if (_calculatedPreferenceGeneralRuleControlViewModel != null)
                    {
                        _calculatedPreferenceGeneralRuleControlViewModel.AddNewPreference -= _calculatedPreferenceGeneralRuleControlViewModel_AddNewPreference;
                    }

                    if (CalculatedPreferencesListControlViewModel != null)
                    {
                        CalculatedPreferencesListControlViewModel.Dispose();
                        _calculatedPreferencesListControlViewModel = null;
                    }
                    if (CalculatedPreferenceDefaultRuleControlViewModel != null)
                    {
                        CalculatedPreferenceDefaultRuleControlViewModel.Dispose();
                        _calculatedPreferenceDefaultRuleControlViewModel = null;
                    }
                    if (CalculatedPreferenceGeneralRuleControlViewModel != null)
                    {
                        CalculatedPreferenceGeneralRuleControlViewModel.Dispose();
                        _calculatedPreferenceGeneralRuleControlViewModel = null;
                    }
                    if (PreferenceAccountStrategyControlViewModel != null)
                    {
                        PreferenceAccountStrategyControlViewModel.Dispose();
                        _preferenceAccountStrategyControlViewModel = null;
                    }
                    if (BulkChangeControlViewModel != null)
                    {
                        BulkChangeControlViewModel.Dispose();
                        _bulkChangeControlViewModel = null;
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
        #endregion
    }
}