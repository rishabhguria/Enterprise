using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Preferences.ViewModels;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.EventArguments;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Forms.ViewModels
{
    public class EditAllocationPreferencesUIViewModel : ViewModelBase, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        /// <summary>
        /// Occurs when [unsubscribe proxy event].
        /// </summary>
        internal event EventHandler UnsubscribeProxyEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _selected index
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// The _status bar text
        /// </summary>
        private string _statusBarText;

        /// <summary>
        /// The updated allocationpreferences
        /// </summary>
        Dictionary<int, AllocationOperationPreference> UpdatedAllocationpreferences = new Dictionary<int, AllocationOperationPreference>();

        /// <summary>
        /// The updated mf calculatedpreferences
        /// </summary>
        Dictionary<int, AllocationOperationPreference> UpdatedMFCalculatedpreferences = new Dictionary<int, AllocationOperationPreference>();

        /// <summary>
        /// The updated master fundpreferences
        /// </summary>
        Dictionary<int, AllocationMasterFundPreference> UpdatedMasterFundPreferences = new Dictionary<int, AllocationMasterFundPreference>();

        /// <summary>
        /// The added allocationpreferences
        /// </summary>
        List<int> _addedAllocationPreferences = new List<int>();

        /// <summary>
        /// The _added master fund allocation prefs
        /// </summary>
        List<int> _addedMasterFundAllocationPrefs = new List<int>();

        /// <summary>
        /// The _fixed allocation preference control view model
        /// </summary>
        private FixedAllocationPreferenceControlViewModel _fixedAllocationPreferenceControlViewModel;

        /// <summary>
        /// The _calculated allocation preference control view model
        /// </summary>
        private CalculatedAllocationPreferenceControlViewModel _calculatedAllocationPreferenceControlViewModel;

        /// <summary>
        /// The _master fund preferences view model
        /// </summary>
        private MasterFundPreferencesControlViewModel _masterFundPreferencesControlViewModel;

        private MasterFundRatioControlViewModel _masterFundRatioControlViewModel;

        /// <summary>
        /// The set visibile new mf
        /// </summary>
        private bool _isNewMFEnabled;

        /// <summary>
        /// The set visibile old mf
        /// </summary>
        private bool _isOneSymbolOneMasterFundEnable;

        /// <summary>
        /// The set visibile Calculated Preference tab
        /// </summary>
        private bool _setVisibileCalculatedPref;

        /// <summary>
        /// The set name of new mf
        /// </summary>
        private string _setNameOfNewMF;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;


        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the set visibile new mf.
        /// </summary>
        /// <value>
        /// The set visibile new mf.
        /// </value>
        public bool IsNewMFEnabled
        {
            get { return _isNewMFEnabled; }
            set
            {
                _isNewMFEnabled = value;
                RaisePropertyChangedEvent("IsNewMFEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the set visibile old mf.
        /// </summary>
        /// <value>
        /// The set visibile old mf.
        /// </value>
        public bool IsOldMFEnabled
        {
            get { return _isOneSymbolOneMasterFundEnable; }
            set
            {
                _isOneSymbolOneMasterFundEnable = value;
                RaisePropertyChangedEvent("IsOldMFEnabled");
            }
        }

        public bool SetVisibileCalculatedPref
        {
            get { return _setVisibileCalculatedPref; }
            set
            {
                _setVisibileCalculatedPref = value;
                RaisePropertyChangedEvent("SetVisibileCalculatedPref");
            }
        }

        public string SetNameOfNewMF
        {
            get { return _setNameOfNewMF; }
            set
            {
                _setNameOfNewMF = value;
                RaisePropertyChangedEvent("SetNameOfNewMF");
            }
        }
        /// <summary>
        /// Gets or sets the calculated allocation preference control view model.
        /// </summary>
        /// <value>
        /// The calculated allocation preference control view model.
        /// </value>
        public CalculatedAllocationPreferenceControlViewModel CalculatedAllocationPreferenceControlViewModel
        {
            get { return _calculatedAllocationPreferenceControlViewModel; }
            set
            {
                _calculatedAllocationPreferenceControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedAllocationPreferenceControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the fixed allocation preference control view model.
        /// </summary>
        /// <value>
        /// The fixed allocation preference control view model.
        /// </value>
        public FixedAllocationPreferenceControlViewModel FixedAllocationPreferenceControlViewModel
        {
            get { return _fixedAllocationPreferenceControlViewModel; }
            set
            {
                _fixedAllocationPreferenceControlViewModel = value;
                RaisePropertyChangedEvent("FixedAllocationPreferenceControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the master fund preferences view model.
        /// </summary>
        /// <value>
        /// The master fund preferences view model.
        /// </value>
        public MasterFundPreferencesControlViewModel MasterFundPreferencesControlViewModel
        {
            get { return _masterFundPreferencesControlViewModel; }
            set
            {
                _masterFundPreferencesControlViewModel = value;
                RaisePropertyChangedEvent("MasterFundPreferencesControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the master fund ratio control view model.
        /// </summary>
        /// <value>
        /// The master fund ratio control view model.
        /// </value>
        public MasterFundRatioControlViewModel MasterFundRatioControlViewModel
        {
            get { return _masterFundRatioControlViewModel; }
            set
            {
                _masterFundRatioControlViewModel = value;
                RaisePropertyChangedEvent("MasterFundRatioControlViewModel");
            }
        }
        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>
        /// The index of the selected.
        /// </value>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SelectedIndexChanged(_selectedIndex);
                _selectedIndex = value;
                RaisePropertyChangedEvent("SelectedIndex");
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
                _statusBarText = string.Format(" [{0}] {1}", DateTime.Now, value);
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
        /// Gets or sets the close edit allocation preferences.
        /// </summary>
        /// <value>
        /// The close edit allocation preferences.
        /// </value>
        public RelayCommand<object> CloseEditAllocationPreferences { get; set; }

        /// <summary>
        /// Gets or sets the edit allocation preferences UI loaded.
        /// </summary>
        /// <value>
        /// The edit allocation preferences UI loaded.
        /// </value>
        public RelayCommand<object> EditAllocationPreferencesUILoaded { get; set; }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public RelayCommand<object> FormCloseButton { get; set; }

        /// <summary>
        /// Gets or sets the form closed button.
        /// </summary>
        /// <value>
        /// The form closed button.
        /// </value>
        public RelayCommand<object> FormClosed { get; set; }

        /// <summary>
        /// Gets or sets the save edit allocation preference data.
        /// </summary>
        /// <value>
        /// The save edit allocation preference data.
        /// </value>
        public RelayCommand<object> SaveEditAllocationPreferenceData { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditAllocationPreferencesUIViewModel"/> class.
        /// </summary>
        public EditAllocationPreferencesUIViewModel()
        {
            try
            {
                SaveEditAllocationPreferenceData = new RelayCommand<object>((parameter) => SaveEditAllocationPreferences(parameter));
                EditAllocationPreferencesUILoaded = new RelayCommand<object>((parameter) => OnLoadEditAllocationPreferences(parameter));
                CloseEditAllocationPreferences = new RelayCommand<object>((parameter) => CloseEditAllocationPreferencesUI(parameter));
                FormCloseButton = new RelayCommand<object>((parameter) => OnCloseButton(parameter));
                FormClosed = new RelayCommand<object>((parameter) => OnFormClosed(parameter));
                _fixedAllocationPreferenceControlViewModel = new FixedAllocationPreferenceControlViewModel();
                _calculatedAllocationPreferenceControlViewModel = new CalculatedAllocationPreferenceControlViewModel();
                _masterFundPreferencesControlViewModel = new MasterFundPreferencesControlViewModel();
                _masterFundRatioControlViewModel = new MasterFundRatioControlViewModel();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the master fund target ratio data set.
        /// </summary>
        /// <returns></returns>
        private DataSet GetMasterFundTargetRatioDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                ds.Tables.Add(MasterFundRatioControlViewModel.MasterFundRatioCollection.Copy());
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
        /// Wires the events.
        /// </summary>
        internal void WireEvents()
        {
            try
            {
                AllocationClientPreferenceManager.GetInstance.AllocationFixedPreferenceUpdated += AllocationClientPreferenceManager_AllocationFixedPreferenceUpdated;
                AllocationClientPreferenceManager.GetInstance.AllocationMFPreferencesSaved += AllocationClientPreferenceManager_AllocationMFPreferencesSaved;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Validates the master fund ratios.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns></returns>
        private bool ValidateMasterFundRatios(DataSet ds, out string errorMsg)
        {
            bool isValid = false;
            errorMsg = string.Empty;
            try
            {
                float totalAllocationPct = (from DataRow row in ds.Tables[0].Rows where row["TargetRatioPct"] != DBNull.Value select Convert.ToSingle(row["TargetRatioPct"])).Sum();
                isValid = (totalAllocationPct == 100) ? true : false;
                if (!isValid)
                    errorMsg = "Sum of Percentage is not 100! \nAllocation % Entered: " + totalAllocationPct.ToString() + "\nRemaining %: " + (100 - totalAllocationPct).ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isValid;
        }

        /// <summary>
        /// Handles the ApplyPreferenceBulkChangeEvent event of the _calculatedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        private void _calculatedAllocationPreferenceControlViewModel_ApplyPreferenceBulkChangeEvent(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                if (e.ApplyOnSelectedPref)
                {
                    foreach (int id in e.PreferenceList)
                    {
                        if (!UpdatedAllocationpreferences.ContainsKey(id))
                        {
                            AllocationOperationPreference pref = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(id);
                            if (pref != null)
                                UpdatedAllocationpreferences.Add(id, pref);
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, AllocationOperationPreference> pref in AllocationClientPreferenceManager.GetInstance.GetAllAllocationOperationPreferences().Where(x => !UpdatedAllocationpreferences.ContainsKey(x.Key)))
                    {
                        if (pref.Value != null)
                            UpdatedAllocationpreferences.Add(pref.Key, pref.Value.Clone());
                    }
                }

                foreach (int prefId in UpdatedAllocationpreferences.Keys)
                {
                    if (!e.ApplyOnSelectedPref || e.PreferenceList.Contains(prefId))
                    {
                        foreach (int checkListId in UpdatedAllocationpreferences[prefId].CheckListWisePreference.Keys)
                        {
                            CheckListWisePreference checkList = new CheckListWisePreference();
                            checkList = UpdatedAllocationpreferences[prefId].CheckListWisePreference[checkListId];

                            if (e.allocationBaseChecked)
                                checkList.Rule.BaseType = e.Rule.BaseType;
                            if (e.matchingRuleChecked)
                            {
                                checkList.Rule.RuleType = e.Rule.RuleType;
                                checkList.Rule.ProrataDaysBack = e.Rule.ProrataDaysBack;
                                checkList.Rule.ProrataAccountList = e.Rule.ProrataAccountList;
                            }
                            if (e.preferencedAccountChecked)
                                checkList.Rule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                            if (e.matchPortfolioPostionChecked)
                                checkList.Rule.MatchClosingTransaction = e.Rule.MatchClosingTransaction;
                            UpdatedAllocationpreferences[prefId].TryUpdateCheckList(checkList, true);
                        }

                        if (e.ApplyOnDefaultRule)
                        {
                            AllocationRule rule = new AllocationRule();
                            rule = UpdatedAllocationpreferences[prefId].DefaultRule;
                            if (e.allocationBaseChecked)
                                rule.BaseType = e.Rule.BaseType;
                            if (e.matchingRuleChecked)
                            {
                                rule.RuleType = e.Rule.RuleType;
                                rule.ProrataDaysBack = e.Rule.ProrataDaysBack;
                                rule.ProrataAccountList = e.Rule.ProrataAccountList;
                            }
                            if (e.preferencedAccountChecked)
                                rule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                            if (e.matchPortfolioPostionChecked)
                                rule.MatchClosingTransaction = e.Rule.MatchClosingTransaction;
                            UpdatedAllocationpreferences[prefId].TryUpdateDefaultRule(rule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PreferenceEvent event of the _calculatedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void _calculatedAllocationPreferenceControlViewModel_PreferenceEvent(object sender, EventArgs<AllocationPrefOperationEventArgs> e)
        {
            try
            {
                List<PreferenceUpdateResult> preferenceUpdateResult = new List<PreferenceUpdateResult>();
                bool isSuccess = false;
                switch (e.Value.AllocationPrefOperation)
                {
                    case AllocationPrefOperation.Import:
                        for (int i = 0; i < e.Value.ImportExportPath.Count(); i++)
                        {
                            preferenceUpdateResult.Add(ImportPreferences(e.Value.ImportExportPath[i]));
                        }
                        isSuccess = PrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "Preference imported successfully." : "Error occured while importing preference.";
                        break;

                    case AllocationPrefOperation.ExportAll:
                        ExportAllPreferences(e.Value.ImportExportPath[0]);
                        StatusBarText = "Preferences exported successfully.";
                        break;

                    case AllocationPrefOperation.Export:
                        AllocationOperationPreference exportPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(e.Value.PrefId);
                        if (exportPreference != null)
                        {
                            ExportPreference(e.Value.ImportExportPath[0], exportPreference);
                            StatusBarText = "Preference exported successfully.";
                        }
                        else
                            StatusBarText = "Preference does not exist. So, can not export.";
                        break;

                    case AllocationPrefOperation.Add:
                        preferenceUpdateResult.Add(AddPreference(e.Value.PrefName, AllocationPreferencesType.CalculatedAllocationPreference, true));
                        isSuccess = PrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "Preference added successfully." : "Error occured while adding preference.";
                        break;

                    case AllocationPrefOperation.Copy:
                        AllocationOperationPreference originalPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(e.Value.CopyPrefId);
                        if (originalPreference != null && originalPreference.IsValid())
                        {
                            preferenceUpdateResult.Add(CopyPreference(e.Value.CopyPrefId, e.Value.PrefName, AllocationPreferencesType.CalculatedAllocationPreference));
                            isSuccess = PrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                            StatusBarText = (isSuccess) ? "Preference copied successfully." : "Error occured while copying preference.";
                        }
                        else
                        {
                            MessageBox.Show("Preference is invalid. \n So, can not copy.", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                            _calculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.RemoveFromCollection(e.Value.PrefId);
                        }
                        break;

                    case AllocationPrefOperation.Delete:
                        preferenceUpdateResult.Add(DeletePreference(e.Value.PrefId, AllocationPreferencesType.CalculatedAllocationPreference));
                        isSuccess = PrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        if (isSuccess)
                        {
                            if (_addedAllocationPreferences.Contains(e.Value.PrefId))
                                _addedAllocationPreferences.Remove(e.Value.PrefId);
                            if (UpdatedAllocationpreferences.ContainsKey(e.Value.PrefId))
                                UpdatedAllocationpreferences.Remove(e.Value.PrefId);
                        }
                        StatusBarText = (isSuccess) ? "Preference deleted successfully." : "Error occured while deleting preference.";
                        break;

                    case AllocationPrefOperation.Rename:
                        preferenceUpdateResult.Add(RenamePreference(e.Value.PrefId, e.Value.PrefName, AllocationPreferencesType.CalculatedAllocationPreference));
                        isSuccess = PrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "Preference renamed successfully." : "Error occured while renaming preference.";
                        break;
                }
                if (isSuccess)
                    ResetCalculatedPreferencesControl();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the PreviewPreferenceEvent event of the _calculatedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _calculatedAllocationPreferenceControlViewModel_PreviewPreferenceEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                PreviewSelectedCalculatedPreference(e.Value, false);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the UpdatePreferenceCacheEvent event of the _calculatedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _calculatedAllocationPreferenceControlViewModel_UpdatePreferenceCacheEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                GetSelectedUpdatedPreference(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the DeleteAllocationSchemeEvent event of the _fixedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _fixedAllocationPreferenceControlViewModel_DeleteAllocationSchemeEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                bool isDeleted = AllocationClientPreferenceManager.GetInstance.DeleteAllocationScheme(e.Value.Value, e.Value.Key);
                StatusBarText = isDeleted ? e.Value.Value + " preference successfully deleted!" : "Groups have been allocated using this preference! Hence,it can't be deleted!";
                if (isDeleted)
                {
                    _fixedAllocationPreferenceControlViewModel.DataProvider = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SaveFixedPreferenceEvent event of the _fixedAllocationPreferenceControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        void _fixedAllocationPreferenceControlViewModel_SaveFixedPreferences(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                SaveFixedAllocationPrefrence(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Adds the preference.
        /// </summary>
        /// <param name="prefName">Name of the preference.</param>
        /// <param name="allocationPrefType">Type of the allocation preference.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns></returns>
        private PreferenceUpdateResult AddPreference(string prefName, AllocationPreferencesType allocationPrefType, bool isPrefVisible)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.AddPreference(prefName, CommonDataCache.CachedDataManager.GetInstance.GetCompanyID(), allocationPrefType, isPrefVisible);
                if (preferenceUpdateResult.Error == null && preferenceUpdateResult.Preference != null)
                    _addedAllocationPreferences.Add(preferenceUpdateResult.Preference.OperationPreferenceId);

                if (preferenceUpdateResult.Error == null && preferenceUpdateResult.MasterFundPreference != null)
                    _addedMasterFundAllocationPrefs.Add(preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceId);

                return preferenceUpdateResult;
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
        /// Handles the AllocationFixedPreferenceUpdated event of the AllocationClientPreferenceManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{Dictionary{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void AllocationClientPreferenceManager_AllocationFixedPreferenceUpdated(object sender, EventArgs<Dictionary<int, string>> e)
        {
            try
            {
                _fixedAllocationPreferenceControlViewModel.OnLoadGetSchemeList(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Closes the edit allocation preferences UI.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object CloseEditAllocationPreferencesUI(object parameter)
        {
            try
            {
                Window allocationPreferenceWindow = parameter as Window;
                if (allocationPreferenceWindow != null)
                    allocationPreferenceWindow.Close();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Copies the preference.
        /// </summary>
        /// <param name="copyPrefID">The copy preference identifier.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        private PreferenceUpdateResult CopyPreference(int copyPrefID, string prefName, AllocationPreferencesType prefType)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.CopyPreference(copyPrefID, prefName, prefType);
                return preferenceUpdateResult;
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
        /// Deletes the preference.
        /// </summary>
        /// <param name="prefID">The preference identifier.</param>
        /// <returns></returns>
        private PreferenceUpdateResult DeletePreference(int prefID, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.DeletePreference(prefID, allocationPrefType);
                return preferenceUpdateResult;
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
        /// Handles the preferences window closing event
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        internal bool EditPreferencesWindowClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                GetSelectedUpdatedPreference();
                GetSelectedUpdatedMFPreference();

                bool isClose = true;
                bool isCloseMFPref = true;

                if (UpdatedAllocationpreferences.Count > 0 || FixedAllocationPreferenceControlViewModel.IsModified || UpdatedMasterFundPreferences.Count > 0 || UpdatedMFCalculatedpreferences.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("There are changes in some preference(s). Do you want to Save all preference(s) ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        isClose = SaveCalculatedAllocationPreferences(true);
                        SaveFixedAllocationPrefrence(_fixedAllocationPreferenceControlViewModel.SelectedfixedAllocationPreference);
                        isCloseMFPref = SaveMasterFundAllocationPreferences(true);
                        if (MasterFundRatioControlViewModel.IsModified)
                            MasterFundTabPreferenceSave();
                    }
                    else
                    {
                        UpdatedAllocationpreferences.Clear();
                        UpdatedMasterFundPreferences.Clear();
                    }

                    if (!isClose || !isCloseMFPref)
                    {
                        e.Cancel = true;
                        return e.Cancel;
                    }
                }

                //Calculated Preferences
                e.Cancel = DeleteAddedInvalidCalculatedPreferences();
                if (e.Cancel)
                    return e.Cancel;

                //Master Fund Preferences
                e.Cancel = DeleteAddedInvalidMFPreferences();
                if (e.Cancel)
                    return e.Cancel;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return e.Cancel;
        }

        /// <summary>
        /// Deletes the added invalid mf preferences.
        /// </summary>
        /// <param name="isAllocationClientClosing">if set to <c>true</c> [is allocation client closing].</param>
        /// <returns></returns>
        private bool DeleteAddedInvalidMFPreferences(bool isAllocationClientClosing = false)
        {
            bool isCancelClose = false;
            try
            {
                if (_addedMasterFundAllocationPrefs.Count > 0)
                {
                    Dictionary<int, string> inValidMFPreferences = new Dictionary<int, string>();
                    StringBuilder invalidMFPreferencesList = new StringBuilder();
                    string errorMessage = string.Empty;
                    foreach (int mfPrefId in _addedMasterFundAllocationPrefs)
                    {
                        AllocationMasterFundPreference mfPreference = UpdatedMasterFundPreferences.ContainsKey(mfPrefId) ? UpdatedMasterFundPreferences[mfPrefId] : AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(mfPrefId);

                        if (mfPreference != null && (!mfPreference.IsValid() || !AreCalculatePreferenceValid(mfPreference.MasterFundPreference, out errorMessage)))
                        {
                            inValidMFPreferences.Add(mfPrefId, mfPreference.MasterFundPreferenceName);
                            invalidMFPreferencesList.Append(string.Format("- {0}\n", mfPreference.MasterFundPreferenceName));
                        }
                    }
                    if (inValidMFPreferences.Count > 0)
                    {
                        MessageBoxResult result = MessageBox.Show(string.Format("The following MF preference(s) are invalid:\n{0}\n Do you want to delete these MF preference(s)?", invalidMFPreferencesList.ToString()), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (isAllocationClientClosing)
                            {
                                if (UnsubscribeProxyEvent != null)
                                    UnsubscribeProxyEvent(this, EventArgs.Empty);
                            }
                            foreach (KeyValuePair<int, string> pref in inValidMFPreferences)
                                DeletePreference(pref.Key, AllocationPreferencesType.MasterFundAllocationPreference);
                        }
                        else
                        {
                            isCancelClose = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isCancelClose;
        }

        /// <summary>
        /// Deletes the added invalid calculated preferences.
        /// </summary>
        /// <param name="isAllocationClientClosing">if set to <c>true</c> [is allocation client closing].</param>
        /// <returns></returns>
        private bool DeleteAddedInvalidCalculatedPreferences(bool isAllocationClientClosing = false)
        {
            bool isCancelClose = false;
            try
            {
                if (_addedAllocationPreferences.Count > 0)
                {
                    Dictionary<int, string> inValidPreferences = new Dictionary<int, string>();
                    StringBuilder invalidPreferencesList = new StringBuilder();

                    foreach (int prefId in _addedAllocationPreferences)
                    {
                        AllocationOperationPreference preference = UpdatedAllocationpreferences.ContainsKey(prefId) ? UpdatedAllocationpreferences[prefId] : AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(prefId);

                        if (preference != null && !preference.IsValid())
                        {
                            inValidPreferences.Add(prefId, preference.OperationPreferenceName);
                            invalidPreferencesList.Append(string.Format("- {0}\n", preference.OperationPreferenceName));
                        }
                    }
                    if (inValidPreferences.Count > 0)
                    {
                        MessageBoxResult result = MessageBox.Show(string.Format("The following preference(s) are invalid:\n{0} Do you want to delete these preference(s)?", invalidPreferencesList.ToString()), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (isAllocationClientClosing)
                            {
                                if (UnsubscribeProxyEvent != null)
                                    UnsubscribeProxyEvent(this, EventArgs.Empty);
                            }
                            foreach (KeyValuePair<int, string> pref in inValidPreferences)
                                DeletePreference(pref.Key, AllocationPreferencesType.CalculatedAllocationPreference);
                        }
                        else
                        {
                            isCancelClose = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isCancelClose;
        }

        /// <summary>
        /// Exports all preferences.
        /// </summary>
        /// <param name="importExportPath">The import export path.</param>
        private void ExportAllPreferences(string importExportPath)
        {
            try
            {
                foreach (int key in _calculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.GetPreferenceIdList())
                {
                    AllocationOperationPreference allocationOperationPref = new AllocationOperationPreference();
                    allocationOperationPref = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(key);
                    if (allocationOperationPref != null)
                    {
                        string exportPath = string.Format("{0}\\{1}.npref", importExportPath, allocationOperationPref.OperationPreferenceName);
                        using (Stream stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            IFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, allocationOperationPref);
                        }
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
        /// Exports the preference.
        /// </summary>
        /// <param name="importExportPath">The import export path.</param>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        private void ExportPreference(string importExportPath, object allocationOperationPref)
        {
            try
            {
                if (allocationOperationPref != null)
                {
                    using (Stream stream = new FileStream(importExportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, allocationOperationPref);
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
        /// Gets the selected updated preference.
        /// </summary>
        /// <param name="selectedPrefernce">The selected prefernce.</param>
        /// <returns></returns>
        private AllocationOperationPreference GetSelectedUpdatedPreference(KeyValuePair<int, string> selectedPrefernce)
        {
            AllocationOperationPreference selectedOperationPreference = null;
            try
            {
                AllocationOperationPreference cachePreference = null;
                if (AllocationClientPreferenceManager.GetInstance != null)
                    cachePreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(selectedPrefernce.Key);

                if (UpdatedAllocationpreferences.ContainsKey(selectedPrefernce.Key))
                    selectedOperationPreference = UpdatedAllocationpreferences[selectedPrefernce.Key];
                else
                    selectedOperationPreference = cachePreference != null ? cachePreference.Clone() : cachePreference;

                if (selectedOperationPreference != null)
                {
                    AllocationOperationPreference originalPreference = cachePreference ?? selectedOperationPreference;

                    CalculatedAllocationPreferenceControlViewModel.UpdateAllocationpreferenceWithUIValues(selectedOperationPreference);

                    if (!UpdatedAllocationpreferences.ContainsKey(selectedPrefernce.Key) && !originalPreference.Equals(selectedOperationPreference))
                        UpdatedAllocationpreferences.Add(selectedOperationPreference.OperationPreferenceId, selectedOperationPreference);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedOperationPreference;
        }

        /// <summary>
        /// Imports the preferences.
        /// </summary>
        /// <param name="importExportPath">The import export path.</param>
        /// <returns></returns>
        private PreferenceUpdateResult ImportPreferences(string importExportPath)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                if (File.Exists(importExportPath))
                {
                    AllocationOperationPreference importedPref;
                    using (Stream stream = new FileStream(importExportPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        importedPref = (AllocationOperationPreference)formatter.Deserialize(stream);
                    }
                    if (importedPref != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(importExportPath);
                        var preferenceName = importedPref.OperationPreferenceName;
                        var containsId = _calculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.CollectionContainsId(importedPref.OperationPreferenceId);
                        var containsName = _calculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.CollectionContainsName(importedPref.OperationPreferenceName);

                        if (containsId && containsName && (fileName == preferenceName))
                            preferenceUpdateResult.Error = ("Preference with same name exists. So, cannot import " + importedPref.OperationPreferenceName);
                        else if (containsName || containsId)
                            preferenceUpdateResult.Error = ("Similar preference exists. So, cannot import " + importedPref.OperationPreferenceName);
                        else
                            preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.ImportPreference(importedPref);
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
            return preferenceUpdateResult;
        }
        /// <summary>
        /// Called when Import the master fund preference
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private PreferenceUpdateResult ImportMasterFundPreferences(string importExportPath)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                if (File.Exists(importExportPath))
                {
                    AllocationMasterFundPreference importedPref;
                    object[] mypreflist = new object[2];
                    using (Stream stream = new FileStream(importExportPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        mypreflist = (object[])formatter.Deserialize(stream);
                    }
                    importedPref = (AllocationMasterFundPreference)mypreflist[0];
                    List<AllocationOperationPreference> mfCalcPreference = (List<AllocationOperationPreference>)mypreflist[1];
                    if (importedPref != null && mfCalcPreference != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(importExportPath);
                        var preferenceName = importedPref.MasterFundPreferenceName;
                        var containsId = _masterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.CollectionContainsId(importedPref.MasterFundPreferenceId);
                        var containsName = _masterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.CollectionContainsName(importedPref.MasterFundPreferenceName);

                        if (containsId && containsName && (fileName == preferenceName))
                            preferenceUpdateResult.Error = ("Preference with same name exists. So, cannot import " + importedPref.MasterFundPreferenceName);
                        else if (containsName || containsId)
                            preferenceUpdateResult.Error = ("Similar preference exists. So, cannot import " + importedPref.MasterFundPreferenceName);
                        else
                            preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.ImportMasterfundPreference(importedPref, mfCalcPreference);
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
            return preferenceUpdateResult;
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
                System.ComponentModel.CancelEventArgs e = parameter as System.ComponentModel.CancelEventArgs;
                bool cancel = EditPreferencesWindowClosing(e);
                if (OnFormCloseButtonEvent != null && !cancel)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [form closed].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnFormClosed(object parameter)
        {
            try
            {
                Dispose();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load edit allocation preferences].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnLoadEditAllocationPreferences(object parameter)
        {
            try
            {
                AllocationClientPreferenceManager.GetInstance.UpdateAllocationOperationPref();
                AllocationClientPreferenceManager.GetInstance.UpdateMasterFundAllocationPref();

                //binding events
                _calculatedAllocationPreferenceControlViewModel.PreferenceEvent += _calculatedAllocationPreferenceControlViewModel_PreferenceEvent;
                _calculatedAllocationPreferenceControlViewModel.ApplyPreferenceBulkChangeEvent += _calculatedAllocationPreferenceControlViewModel_ApplyPreferenceBulkChangeEvent;
                _calculatedAllocationPreferenceControlViewModel.PreviewPreferenceEvent += _calculatedAllocationPreferenceControlViewModel_PreviewPreferenceEvent;
                _calculatedAllocationPreferenceControlViewModel.UpdatePreferenceCacheEvent += _calculatedAllocationPreferenceControlViewModel_UpdatePreferenceCacheEvent;
                _fixedAllocationPreferenceControlViewModel.DeleteAllocationSchemeEvent += _fixedAllocationPreferenceControlViewModel_DeleteAllocationSchemeEvent;
                _fixedAllocationPreferenceControlViewModel.SaveFixedPreferences += _fixedAllocationPreferenceControlViewModel_SaveFixedPreferences;
                _masterFundPreferencesControlViewModel.PreviewPreferenceEvent += _masterFundPreferencesViewModel_PreviewPreferenceEvent;
                _masterFundPreferencesControlViewModel.UpdateMFPreferenceCacheEvent += _masterFundPreferencesViewModel_UpdatePreferenceCacheEvent;
                _masterFundPreferencesControlViewModel.PreferenceEvent += _masterFundPreferencesViewModel_PreferenceEvent;
                _masterFundPreferencesControlViewModel.UpdateCalculatedPrefCacheEvent += _masterFundPreferencesViewModel_UpdateCalculatedPrefCacheEvent;
                _masterFundPreferencesControlViewModel.PreviewPreferenceSelectedMF += _masterFundPreferencesViewModel_PreviewPreferenceSelectedMF;

                //binding data
                _fixedAllocationPreferenceControlViewModel.OnLoadGetSchemeList(AllocationClientPreferenceManager.GetInstance.GetFixedPreferencesList());
                AllocationCompanyWisePref pref = AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences();

                IsOldMFEnabled = (pref.IsOneSymbolOneMasterFundAllocation) ? true : false;
                IsNewMFEnabled = (pref.IsOneSymbolOneMasterFundAllocation || !pref.EnableMasterFundAllocation) ? false : true;
                SetVisibileCalculatedPref = (!pref.IsOneSymbolOneMasterFundAllocation && pref.EnableMasterFundAllocation) ? false : true;

                if (SetVisibileCalculatedPref)
                {
                    _calculatedAllocationPreferenceControlViewModel.OnLoadCalculatedAllocationPreferenceControl(pref);
                }

                if (IsNewMFEnabled)
                {
                    _masterFundPreferencesControlViewModel.OnLoadMasterFundPreferenceControl();
                }

                // load master fund control
                if (IsOldMFEnabled)
                {
                    DataSet masterFundRatioSet = AllocationClientPreferenceManager.GetInstance.GetMasterFundsRatio();
                    _masterFundRatioControlViewModel.OnLoadMasterFundRatioControl(masterFundRatioSet.Tables[0], pref.EnableMasterFundAllocation, true);
                }


                SetNameOfNewMF = IsNewMFEnabled ? "Calculated Preferences" : "MasterFund Preferences";
                if (IsNewMFEnabled)
                    SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Update the preferences cache and UI .
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        /// <param name="prefID">The preference identifier, old Pref key in case of copy and add.</param>
        /// <param name="userAction">The user action.</param>
        /// <returns>Is update success or not</returns>
        private bool PrefCacheAndUIUpdate(List<PreferenceUpdateResult> preferenceUpdateResultList, int prefID, AllocationPrefOperation userAction)
        {
            bool isSuccess = true;
            try
            {
                StringBuilder preferenceErrorMsg = new StringBuilder();
                foreach (PreferenceUpdateResult preferenceUpdateResult in preferenceUpdateResultList)
                {
                    if (preferenceUpdateResult != null)
                    {
                        if (preferenceUpdateResult.Error == null)
                        {
                            AllocationClientPreferenceManager.GetInstance.UpDatePrefCache(preferenceUpdateResult, prefID);
                            if (preferenceUpdateResult.Preference != null)
                                _calculatedAllocationPreferenceControlViewModel.AddRemoveUpdatePrefCollection(preferenceUpdateResult.Preference.OperationPreferenceId, preferenceUpdateResult.Preference.OperationPreferenceName, prefID);
                            else
                                _calculatedAllocationPreferenceControlViewModel.DeletePrefFromCollection(prefID);
                        }
                        else
                        {
                            preferenceErrorMsg.AppendLine(preferenceUpdateResult.Error);
                            if (userAction == AllocationPrefOperation.Rename)
                            {
                                AllocationOperationPreference pref = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(prefID);
                                _calculatedAllocationPreferenceControlViewModel.AddRemoveUpdatePrefCollection(prefID, pref.OperationPreferenceName, prefID);
                            }
                            else
                                _calculatedAllocationPreferenceControlViewModel.DeletePrefFromCollection(prefID);
                        }
                    }
                }
                if (preferenceErrorMsg.Length > 0)
                {
                    isSuccess = false;
                    if (userAction == AllocationPrefOperation.Import)
                        preferenceErrorMsg.Insert(0, "Following preferences cannot be imported: \n");

                    MessageBox.Show(preferenceErrorMsg.ToString(), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                    preferenceErrorMsg.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSuccess;
        }

        /// <summary>
        /// Renames the preference.
        /// </summary>
        /// <param name="prefID">The preference identifier.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        private PreferenceUpdateResult RenamePreference(int prefID, string prefName, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceManager.GetInstance.RenamePreference(prefID, prefName, allocationPrefType);
                return preferenceUpdateResult;
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
        /// Saves the calculated allocation preferences.
        /// </summary>
        private bool SaveCalculatedAllocationPreferences(bool isClosing)
        {
            bool result = true;
            try
            {
                AllocationOperationPreference selectedOperationPreference = GetSelectedUpdatedPreference();

                if (UpdatedAllocationpreferences.Count > 0)
                {
                    if (UpdatedAllocationpreferences.Count == 1)
                        result = SavePreferences(false, selectedOperationPreference, isClosing);
                    else
                    {
                        MessageBoxResult userChoice = MessageBoxResult.Yes;
                        if (!isClosing)
                            userChoice = MessageBox.Show("There are changes in other preferences. Do you want to Save all preferences?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        result = SavePreferences(userChoice == MessageBoxResult.Yes, selectedOperationPreference, isClosing);
                    }
                }
                else
                    result = SavePreferences(false, selectedOperationPreference, isClosing);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the selected updated preference.
        /// </summary>
        /// <returns></returns>
        private AllocationOperationPreference GetSelectedUpdatedPreference()
        {
            AllocationOperationPreference selectedOperationPreference = null;
            try
            {
                if (CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.SelectedAllocationPreferences != null)
                {
                    DictionaryImpersonation<int, string> operationPreference = (DictionaryImpersonation<int, string>)CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.SelectedAllocationPreferences;
                    KeyValuePair<int, string> selectedPrefernce = new KeyValuePair<int, string>(operationPreference.Key, operationPreference.Value);
                    selectedOperationPreference = GetSelectedUpdatedPreference(selectedPrefernce);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedOperationPreference;
        }

        /// <summary>
        /// Saves the edit allocation preferences.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object SaveEditAllocationPreferences(object parameter)
        {
            try
            {
                StatusBarText = string.Empty;
                KeyValuePair<int, string> selectedfixedAllocationPreference = _fixedAllocationPreferenceControlViewModel.SelectedfixedAllocationPreference;

                //TODO: Need to Improve Logic here
                if (SelectedIndex == 0)
                {
                    if (FixedAllocationPreferenceControlViewModel.IsModified || UpdatedMasterFundPreferences.Count > 0 || MasterFundRatioControlViewModel.IsModified)
                    {
                        MessageBoxResult result = MessageBox.Show("Do you want to save changes in fixed/ MasterFund preferences also ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveFixedAllocationPrefrence(selectedfixedAllocationPreference);
                            SaveMasterFundAllocationPreferences(false);
                            if (MasterFundRatioControlViewModel.IsModified)
                                MasterFundTabPreferenceSave();
                        }
                    }

                    SaveCalculatedAllocationPreferences(false);
                }
                else if (SelectedIndex == 1)
                {
                    if (FixedAllocationPreferenceControlViewModel.IsModified || UpdatedAllocationpreferences.Count > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Do you want to save changes in fixed/ calculated preferences also ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveCalculatedAllocationPreferences(false);
                            SaveFixedAllocationPrefrence(selectedfixedAllocationPreference);
                        }
                    }

                    SaveMasterFundAllocationPreferences(false);
                }
                else if (SelectedIndex == 2)
                {
                    if (UpdatedAllocationpreferences.Count > 0 || UpdatedMasterFundPreferences.Count > 0 || MasterFundRatioControlViewModel.IsModified)
                    {
                        MessageBoxResult result = MessageBox.Show("Do you want to save changes in calculated/ MasterFund preferences also ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveCalculatedAllocationPreferences(false);
                            SaveMasterFundAllocationPreferences(false);
                            if (MasterFundRatioControlViewModel.IsModified)
                                MasterFundTabPreferenceSave();
                        }
                    }

                    if (FixedAllocationPreferenceControlViewModel.IsModified)
                        SaveFixedAllocationPrefrence(selectedfixedAllocationPreference);
                }
                else if (SelectedIndex == 3)
                {
                    if (UpdatedAllocationpreferences.Count > 0 || FixedAllocationPreferenceControlViewModel.IsModified)
                    {
                        MessageBoxResult result = MessageBox.Show("Do you want to save changes in calculated/ Fixed preferences also ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (FixedAllocationPreferenceControlViewModel.IsModified)
                                SaveFixedAllocationPrefrence(selectedfixedAllocationPreference);
                            SaveCalculatedAllocationPreferences(false);
                            SaveMasterFundAllocationPreferences(false);
                        }
                    }
                    if (MasterFundRatioControlViewModel.IsModified)
                        MasterFundTabPreferenceSave();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Masterfund tab preference save.
        /// </summary>
        private void MasterFundTabPreferenceSave()
        {
            try
            {
                bool isValid = false;
                bool isSaved = false;
                StatusBarText = string.Empty;
                DataSet masterFundTargetRatioDataSet = new DataSet();

                //Get Preferences
                string errorMsg = string.Empty;
                masterFundTargetRatioDataSet = GetMasterFundTargetRatioDataSet();
                isValid = ValidateMasterFundRatios(masterFundTargetRatioDataSet, out errorMsg);
                if (isValid)
                {
                    isSaved = AllocationClientPreferenceManager.GetInstance.SaveOldMasterFundAllocationPreferences(masterFundTargetRatioDataSet);
                    StatusBarText = isSaved ? "Preferences saved successfully" : "Preferences are not saved";
                    MasterFundRatioControlViewModel.IsModified = false;
                }
                else
                {
                    MessageBox.Show(errorMsg, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                    StatusBarText = "Master Fund preferences is not valid. Please update and save again";
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
        /// Saves the fixed allocation prefrence.
        /// </summary>
        private void SaveFixedAllocationPrefrence(KeyValuePair<int, string> selectedfixedAllocationPreference)
        {
            try
            {
                int resultantID = 0;
                if (FixedAllocationPreferenceControlViewModel.IsModified)
                {
                    XmlDataProvider newGridData = FixedAllocationPreferenceControlViewModel.DataProvider;
                    if (newGridData != null)
                    {
                        string doc = CommonAllocationMethods.GetXMLAsString(newGridData.Document);
                        AllocationFixedPreference fixedPref = new AllocationFixedPreference(selectedfixedAllocationPreference.Key, selectedfixedAllocationPreference.Value, doc, DateTime.Now, true, FixedPreferenceCreationSource.None);
                        resultantID = AllocationClientPreferenceManager.GetInstance.SaveAllocationSchemeData(fixedPref);
                    }
                }
                if (resultantID > 0)
                {
                    StatusBarText = "Allocation Scheme successfully saved at " + DateTime.Now;
                    FixedAllocationPreferenceControlViewModel.IsModified = false;
                }
                else
                    StatusBarText = "Nothing to Save";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the preferences.
        /// </summary>
        /// <param name="saveAll">if set to <c>true</c> [save all].</param>
        /// <param name="selectedOperationPreference">The selected operation preference.</param>
        /// <param name="isClosing">if set to <c>true</c> [is closing].</param>
        /// <returns></returns>
        private bool SavePreferences(bool saveAll, AllocationOperationPreference selectedOperationPreference, bool isClosing)
        {
            bool isClose = true;
            try
            {
                List<AllocationOperationPreference> validAllocationOperationPreferences = new List<AllocationOperationPreference>();
                List<AllocationOperationPreference> invalidAllocationOperationPreferences = new List<AllocationOperationPreference>();
                string errorMessage = string.Empty;

                if (selectedOperationPreference != null)
                {
                    if (selectedOperationPreference.IsValid(out errorMessage))
                        validAllocationOperationPreferences.Add(selectedOperationPreference);
                    else
                    {
                        if (isClosing)
                        {
                            MessageBoxResult result = MessageBox.Show(string.Format(" Invalid Preference: {0} {1}\n Changes will revert for this preference", selectedOperationPreference.OperationPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                            if (result == MessageBoxResult.OK)
                                selectedOperationPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(selectedOperationPreference.OperationPreferenceId);
                            else
                                isClose = false;
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show(string.Format(" Invalid Preference: {0} {1}\n Do you want to revert this preference?", selectedOperationPreference.OperationPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                selectedOperationPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(selectedOperationPreference.OperationPreferenceId);
                                CalculatedAllocationPreferenceControlViewModel.ShowAllocationPreference(selectedOperationPreference);
                            }
                        }
                    }
                }

                if (saveAll)
                {
                    if (selectedOperationPreference != null)
                    {
                        foreach (int preferenceId in UpdatedAllocationpreferences.Keys)
                        {
                            if (preferenceId != selectedOperationPreference.OperationPreferenceId)
                            {
                                if (UpdatedAllocationpreferences[preferenceId].IsValid(out errorMessage))
                                    validAllocationOperationPreferences.Add(UpdatedAllocationpreferences[preferenceId]);
                                else
                                    invalidAllocationOperationPreferences.Add(UpdatedAllocationpreferences[preferenceId]);
                            }
                        }
                    }
                    if (invalidAllocationOperationPreferences != null && invalidAllocationOperationPreferences.Count > 0)
                    {
                        foreach (AllocationOperationPreference preference in invalidAllocationOperationPreferences)
                        {
                            if (isClosing)
                            {
                                MessageBoxResult result = MessageBox.Show(string.Format(" Invalid Preference: {0} {1}\n Changes will revert for this preference", preference.OperationPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                                if (result == MessageBoxResult.OK)
                                    UpdatedAllocationpreferences[preference.OperationPreferenceId] = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(preference.OperationPreferenceId);
                                else
                                    isClose = false;
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show(string.Format(" Invalid Preference: {0} {1}\n Do you want to revert this preference?", preference.OperationPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                    UpdatedAllocationpreferences[preference.OperationPreferenceId] = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(preference.OperationPreferenceId);
                            }
                        }
                    }
                }

                if (validAllocationOperationPreferences.Count > 0)
                {
                    if (AllocationClientPreferenceManager.GetInstance.SaveCalculatedPreferences(validAllocationOperationPreferences))
                    {
                        StatusBarText = "Preferences saved successfully";
                        validAllocationOperationPreferences.ForEach(x => UpdatedAllocationpreferences.Remove(x.OperationPreferenceId));
                        ResetCalculatedPreferencesControl();
                    }
                }
                else
                    StatusBarText = "Nothing to Save";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isClose;
        }

        /// <summary>
        /// Resets the calculated preferences control.
        /// </summary>
        private void ResetCalculatedPreferencesControl()
        {
            try
            {
                CalculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.ClearSelectedItems();
                CalculatedAllocationPreferenceControlViewModel.EnableDisablePrefControl(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Selecteds the index changed.
        /// </summary>
        /// <param name="previouslySelectedIndex">Index of the _previously selected.</param>
        private void SelectedIndexChanged(int previouslySelectedIndex)
        {
            try
            {
                if (previouslySelectedIndex == 0)
                {
                    GetSelectedUpdatedPreference();
                    GetSelectedUpdatedMFPreference();
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
        /// Handles the UpdatePreferenceCacheEvent event of the _masterFundPreferencesViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        void _masterFundPreferencesViewModel_UpdatePreferenceCacheEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                GetSelectedUpdatedMFPreference(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PreviewPreferenceEvent event of the _masterFundPreferencesViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void _masterFundPreferencesViewModel_PreviewPreferenceEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                AllocationMasterFundPreference allocationMasterFundref = new AllocationMasterFundPreference();
                if (UpdatedMasterFundPreferences.ContainsKey(e.Value.Key))
                    allocationMasterFundref = UpdatedMasterFundPreferences[e.Value.Key];
                else
                    allocationMasterFundref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(e.Value.Key);

                if (allocationMasterFundref != null)
                    _masterFundPreferencesControlViewModel.ShowMFAllocationPrefPreview(allocationMasterFundref.Clone());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets the selected updated preference.
        /// </summary>
        /// <param name="selectedPrefernce">The selected prefernce.</param>
        /// <returns></returns>
        private AllocationMasterFundPreference GetSelectedUpdatedMFPreference(KeyValuePair<int, string> selectedPrefernce)
        {
            AllocationMasterFundPreference selectedAllocationMFPreference = null;
            try
            {
                AllocationMasterFundPreference cachePreference = null;
                if (AllocationClientPreferenceManager.GetInstance != null)
                    cachePreference = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(selectedPrefernce.Key); ;

                if (UpdatedMasterFundPreferences.ContainsKey(selectedPrefernce.Key))
                    selectedAllocationMFPreference = UpdatedMasterFundPreferences[selectedPrefernce.Key];
                else
                    selectedAllocationMFPreference = cachePreference != null ? cachePreference.Clone() : cachePreference; //TODO: Add Clone with cachePreference

                if (selectedAllocationMFPreference != null)
                {
                    AllocationMasterFundPreference originalPreference = cachePreference ?? selectedAllocationMFPreference;
                    UpdateAllocationMFPrefWithUIValues(selectedAllocationMFPreference);

                    if (!UpdatedMasterFundPreferences.ContainsKey(selectedPrefernce.Key) && !originalPreference.Equals(selectedAllocationMFPreference))
                        UpdatedMasterFundPreferences.Add(selectedAllocationMFPreference.MasterFundPreferenceId, selectedAllocationMFPreference);

                    KeyValuePair<int, string> calculatedPrefdAndName = MasterFundPreferencesControlViewModel.GetSelectedCalculatedPrefIdAndName(selectedAllocationMFPreference.MasterFundPreferenceId);

                    if (!calculatedPrefdAndName.Equals(default(KeyValuePair<int, string>)))
                        GetSelectedUpdatedMFCalculatedPref(calculatedPrefdAndName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedAllocationMFPreference;
        }

        /// <summary>
        /// Updates the allocationpreference with UI values.
        /// </summary>
        /// <param name="allocationMasterFundPref">The allocation operation preference.</param>
        private void UpdateAllocationMFPrefWithUIValues(AllocationMasterFundPreference allocationMasterFundPref)
        {
            try
            {
                allocationMasterFundPref.UpdateTargetPercentage(MasterFundPreferencesControlViewModel.MasterFundRatioControlViewModel.GetMFGridValues());
                allocationMasterFundPref.UpdateMasterFundPreference(MasterFundPreferencesControlViewModel._allocationMasterFundPref.MasterFundPreference);
                allocationMasterFundPref.UpdateDefaultRule(MasterFundPreferencesControlViewModel.MasterFundDefaultRuleViewModel.GetDefaultRule());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the PreferenceEvent event of the _masterFundPreferencesViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationPrefOperationEventArgs}"/> instance containing the event data.</param>
        private void _masterFundPreferencesViewModel_PreferenceEvent(object sender, EventArgs<AllocationPrefOperationEventArgs> e)
        {
            try
            {
                List<PreferenceUpdateResult> preferenceUpdateResult = new List<PreferenceUpdateResult>();
                bool isSuccess = false;
                switch (e.Value.AllocationPrefOperation)
                {
                    case AllocationPrefOperation.ExportAll:
                        ExportAllMasterFundPreferences(e.Value.ImportExportPath[0]);
                        StatusBarText = "All MasterFund preferences exported successfully.";
                        break;

                    case AllocationPrefOperation.Export:
                        AllocationMasterFundPreference ExportPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(e.Value.PrefId);
                        if (ExportPreference != null)
                        {
                            ExportMasterFundPreference(e.Value.ImportExportPath[0], ExportPreference);
                            StatusBarText = "Preference exported successfully.";
                        }
                        else
                            StatusBarText = "Preference does not exist. So, can not export.";
                        break;

                    case AllocationPrefOperation.Import:
                        for (int i = 0; i < e.Value.ImportExportPath.Count(); i++)
                        {
                            preferenceUpdateResult.Add(ImportMasterFundPreferences(e.Value.ImportExportPath[i]));
                        }
                        isSuccess = MasterFundPrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "Preference imported successfully." : "Error occured while importing preference.";
                        break;

                    case AllocationPrefOperation.Add:
                        _masterFundPreferencesControlViewModel.EnableDisablePrefControl(false);
                        preferenceUpdateResult.Add(AddPreference(e.Value.PrefName, AllocationPreferencesType.MasterFundAllocationPreference, true));
                        isSuccess = MasterFundPrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "MasterFund Preference added successfully." : "Error occured while adding master fund preference.";
                        break;

                    case AllocationPrefOperation.Delete:
                        preferenceUpdateResult.Add(DeletePreference(e.Value.PrefId, AllocationPreferencesType.MasterFundAllocationPreference));
                        isSuccess = MasterFundPrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        if (isSuccess)
                        {
                            if (_addedMasterFundAllocationPrefs.Contains(e.Value.PrefId))
                                _addedMasterFundAllocationPrefs.Remove(e.Value.PrefId);
                            if (UpdatedMasterFundPreferences.ContainsKey(e.Value.PrefId))
                            {
                                foreach (int calcPrefId in UpdatedMasterFundPreferences[e.Value.PrefId].MasterFundPreference.Values)
                                {
                                    if (UpdatedMFCalculatedpreferences.ContainsKey(calcPrefId))
                                        UpdatedMFCalculatedpreferences.Remove(calcPrefId);
                                }
                                UpdatedMasterFundPreferences.Remove(e.Value.PrefId);
                            }
                        }
                        StatusBarText = (isSuccess) ? "MasterFund Preference deleted successfully." : "Error occured while deleting preference.";
                        break;

                    case AllocationPrefOperation.Rename:
                        preferenceUpdateResult.Add(RenamePreference(e.Value.PrefId, e.Value.PrefName, AllocationPreferencesType.MasterFundAllocationPreference));
                        isSuccess = MasterFundPrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                        StatusBarText = (isSuccess) ? "MasterFund Preference renamed successfully." : "Error occured while renaming preference.";
                        break;

                    case AllocationPrefOperation.Copy:
                        AllocationMasterFundPreference originalPreference = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(e.Value.CopyPrefId);
                        if (originalPreference != null && originalPreference.IsValid())
                        {
                            preferenceUpdateResult.Add(CopyPreference(e.Value.CopyPrefId, e.Value.PrefName, AllocationPreferencesType.MasterFundAllocationPreference));
                            isSuccess = MasterFundPrefCacheAndUIUpdate(preferenceUpdateResult, e.Value.PrefId, e.Value.AllocationPrefOperation);
                            StatusBarText = (isSuccess) ? "Preference copied successfully." : "Error occured while copying preference.";
                        }
                        else
                        {
                            MessageBox.Show("Preference is invalid. \n So, can not copy.", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                            _calculatedAllocationPreferenceControlViewModel.CalculatedPreferencesListControlViewModel.RemoveFromCollection(e.Value.PrefId);
                        }
                        break;
                }
                if (isSuccess)
                    ResetMaterFundPreferencesControl();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Export the master fund preference.
        /// </summary>
        /// <param name="copyPrefID">The export preference identifier.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        private void ExportMasterFundPreference(string importExportPath, AllocationMasterFundPreference exportpref)
        {
            try
            {
                if (exportpref != null)
                {
                    List<AllocationOperationPreference> mfCalculatedPrefs = new List<AllocationOperationPreference>();
                    exportpref.MasterFundPreference.Values.ToList().ForEach(prefId => mfCalculatedPrefs.Add(AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(prefId)));
                    Object[] obj = new Object[] { exportpref, mfCalculatedPrefs };
                    using (Stream stream = new FileStream(importExportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, obj);
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
        /// Export All the master fund preference.
        /// </summary>
        /// <param name="copyPrefID">The export preference identifier.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        private void ExportAllMasterFundPreferences(string importExportPath)
        {
            try
            {
                foreach (int key in _masterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.GetPreferenceIdList())
                {
                    AllocationMasterFundPreference allocationMFOperationPref = new AllocationMasterFundPreference();
                    allocationMFOperationPref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(key);
                    if (allocationMFOperationPref != null)
                    {
                        List<AllocationOperationPreference> mfCalculatedPrefs = new List<AllocationOperationPreference>();
                        allocationMFOperationPref.MasterFundPreference.Values.ToList().ForEach(prefId => mfCalculatedPrefs.Add(AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(prefId)));
                        Object[] obj = new Object[] { allocationMFOperationPref, mfCalculatedPrefs };
                        //createXml(allocationMFOperationPref, mfCalculatedPrefs);
                        string exportPath = string.Format("{0}\\{1}.npref", importExportPath, allocationMFOperationPref.MasterFundPreferenceName);
                        using (Stream stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            IFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, obj);
                        }
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
        /// Handles the UpdateCalculatedPrefCacheEvent event of the _masterFundPreferencesViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        void _masterFundPreferencesViewModel_UpdateCalculatedPrefCacheEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                GetSelectedUpdatedMFCalculatedPref(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Previews the selected calculated preference.
        /// </summary>
        /// <param name="keyValuePair">The key value pair.</param>
        /// <param name="isMasterFundAllocation">if set to <c>true</c> [is master fund allocation].</param>
        private void PreviewSelectedCalculatedPreference(KeyValuePair<int, string> keyValuePair, bool isMasterFundAllocation)
        {
            try
            {
                AllocationOperationPreference allocationOperationPref = new AllocationOperationPreference();

                if (isMasterFundAllocation)
                {
                    if (UpdatedMFCalculatedpreferences.ContainsKey(keyValuePair.Key))
                        allocationOperationPref = UpdatedMFCalculatedpreferences[keyValuePair.Key];
                    else
                        allocationOperationPref = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(keyValuePair.Key);

                    if (allocationOperationPref != null)
                        _masterFundPreferencesControlViewModel.ShowAllocationPreference(allocationOperationPref);
                }
                else
                {
                    if (UpdatedAllocationpreferences.ContainsKey(keyValuePair.Key))
                        allocationOperationPref = UpdatedAllocationpreferences[keyValuePair.Key];
                    else
                        allocationOperationPref = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(keyValuePair.Key);

                    if (allocationOperationPref != null)
                        _calculatedAllocationPreferenceControlViewModel.ShowAllocationPreference(allocationOperationPref);
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
        /// Handles the PreviewPreferenceSelectedMF event of the _masterFundPreferencesViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        void _masterFundPreferencesViewModel_PreviewPreferenceSelectedMF(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                PreviewSelectedCalculatedPreference(e.Value, true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Masters the fund preference cache and UI update.
        /// </summary>
        /// <param name="preferenceUpdateResultList">The preference update result list.</param>
        /// <param name="prefID">The preference identifier.</param>
        /// <param name="userAction">The user action.</param>
        /// <returns></returns>
        private bool MasterFundPrefCacheAndUIUpdate(List<PreferenceUpdateResult> preferenceUpdateResultList, int prefID, AllocationPrefOperation userAction)
        {
            bool isSuccess = true;
            try
            {
                StringBuilder preferenceErrorMsg = new StringBuilder();
                foreach (PreferenceUpdateResult preferenceUpdateResult in preferenceUpdateResultList)
                {
                    if (preferenceUpdateResult != null)
                    {
                        if (preferenceUpdateResult.Error == null)
                        {
                            AllocationClientPreferenceManager.GetInstance.AddOrUpdateMasterFundPrefCache(preferenceUpdateResult, prefID);
                            if (userAction.Equals(AllocationPrefOperation.Copy) || userAction.Equals(AllocationPrefOperation.Import))
                                AllocationClientPreferenceManager.GetInstance.UpDateCalcPrefCacheforMFUpdate(preferenceUpdateResult);
                            if (preferenceUpdateResult.MasterFundPreference != null)
                                _masterFundPreferencesControlViewModel.AddRemoveUpdatePrefCollection(preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceId, preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceName, prefID);
                            else
                                _masterFundPreferencesControlViewModel.DeletePrefFromCollection(prefID);
                        }
                        else
                        {
                            preferenceErrorMsg.AppendLine(preferenceUpdateResult.Error);
                            if (userAction == AllocationPrefOperation.Rename)
                            {
                                AllocationMasterFundPreference pref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(prefID);
                                _masterFundPreferencesControlViewModel.AddRemoveUpdatePrefCollection(prefID, pref.MasterFundPreferenceName, prefID);
                            }
                            else
                                _masterFundPreferencesControlViewModel.DeletePrefFromCollection(prefID);
                        }
                    }
                }
                if (preferenceErrorMsg.Length > 0)
                {
                    isSuccess = false;
                    if (userAction == AllocationPrefOperation.Import)
                        preferenceErrorMsg.Insert(0, "Following preferences cannot be imported: \n");

                    MessageBox.Show(preferenceErrorMsg.ToString(), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                    preferenceErrorMsg.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSuccess;
        }

        /// <summary>
        /// Gets the selected updated mf calculated preference.
        /// </summary>
        /// <param name="selectedPrefernce">The selected prefernce.</param>
        /// <returns></returns>
        private AllocationOperationPreference GetSelectedUpdatedMFCalculatedPref(KeyValuePair<int, string> selectedPrefernce)
        {
            AllocationOperationPreference selectedMFCalculatedPref = null;
            try
            {
                AllocationOperationPreference cachePreference = null;

                if (AllocationClientPreferenceManager.GetInstance != null)
                {
                    cachePreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(selectedPrefernce.Key);

                    if (cachePreference == null)
                    {
                        AllocationRule defaultRule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, PreferenceAccountId = -1 };

                        AllocationOperationPreference myNewPref = new AllocationOperationPreference(selectedPrefernce.Key, CachedDataManager.GetInstance.GetCompanyID(), Int32.MinValue, selectedPrefernce.Value, defaultRule, DateTime.Now, false);

                        PreferenceUpdateResult updateResult = new PreferenceUpdateResult() { Preference = myNewPref };
                        cachePreference = myNewPref.Clone();
                        AllocationClientPreferenceManager.GetInstance.UpDatePrefCache(updateResult, selectedPrefernce.Key);
                    }
                }

                if (UpdatedMFCalculatedpreferences.ContainsKey(selectedPrefernce.Key))
                    selectedMFCalculatedPref = UpdatedMFCalculatedpreferences[selectedPrefernce.Key];
                else
                    selectedMFCalculatedPref = cachePreference != null ? cachePreference.Clone() : cachePreference;

                if (selectedMFCalculatedPref != null)
                {
                    AllocationOperationPreference originalPreference = cachePreference ?? selectedMFCalculatedPref;
                    MasterFundPreferencesControlViewModel.UpdateAllocationpreferenceWithUIValues(selectedMFCalculatedPref);

                    if (!UpdatedMFCalculatedpreferences.ContainsKey(selectedPrefernce.Key) && !originalPreference.Equals(selectedMFCalculatedPref))
                        UpdatedMFCalculatedpreferences.Add(selectedMFCalculatedPref.OperationPreferenceId, selectedMFCalculatedPref);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedMFCalculatedPref;
        }

        /// <summary>
        /// Save MasterFund Allocation Preferences
        /// </summary>
        /// <param name="isClosing"></param>
        /// <returns></returns>
        private bool SaveMasterFundAllocationPreferences(bool isClosing)
        {
            bool result = true;
            try
            {
                AllocationMasterFundPreference selectedMasterFundPref = GetSelectedUpdatedMFPreference();

                if (UpdatedMasterFundPreferences.Count > 0)
                {
                    if (UpdatedMasterFundPreferences.Count == 1)
                        result = SaveMasterFundPreferences(false, selectedMasterFundPref, isClosing);
                    else
                    {
                        MessageBoxResult userChoice = MessageBoxResult.Yes;
                        if (!isClosing)
                            userChoice = MessageBox.Show("There are changes in other masterfund preference(s). Do you want to Save all masterfund preferences?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        result = SaveMasterFundPreferences(userChoice == MessageBoxResult.Yes, selectedMasterFundPref, isClosing);
                    }
                }
                else
                    result = SaveMasterFundPreferences(false, selectedMasterFundPref, isClosing);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the selected updated mf preference.
        /// </summary>
        /// <returns></returns>
        private AllocationMasterFundPreference GetSelectedUpdatedMFPreference()
        {
            AllocationMasterFundPreference selectedMasterFundPref = null;
            try
            {
                if (MasterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.SelectedAllocationPreferences != null)
                {
                    DictionaryImpersonation<int, string> masterFundPref = (DictionaryImpersonation<int, string>)MasterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.SelectedAllocationPreferences;
                    KeyValuePair<int, string> selectedPrefernce = new KeyValuePair<int, string>(masterFundPref.Key, masterFundPref.Value);
                    selectedMasterFundPref = GetSelectedUpdatedMFPreference(selectedPrefernce);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedMasterFundPref;
        }

        /// <summary>
        /// Save MasterFund Preferences
        /// </summary>
        /// <param name="saveAll"></param>
        /// <param name="selectedMasterFundPref"></param>
        /// <param name="isClosing"></param>
        /// <returns></returns>
        private bool SaveMasterFundPreferences(bool saveAll, AllocationMasterFundPreference selectedMasterFundPref, bool isClosing)
        {
            bool isClose = true;
            try
            {
                List<AllocationMasterFundPreference> validMasterFundPreferences = new List<AllocationMasterFundPreference>();
                List<AllocationMasterFundPreference> invalidMasterFundPreferences = new List<AllocationMasterFundPreference>();

                string errorMessage = string.Empty;

                if (selectedMasterFundPref != null)
                {
                    if (selectedMasterFundPref.IsValid(out errorMessage) && AreCalculatePreferenceValid(selectedMasterFundPref.MasterFundPreference, out errorMessage))
                        validMasterFundPreferences.Add(selectedMasterFundPref);
                    else
                    {
                        if (isClosing)
                        {
                            MessageBoxResult result = MessageBox.Show(string.Format("Invalid masterfund preference: {0}{1}\nChanges will revert for this masterfund preference", selectedMasterFundPref.MasterFundPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                            if (result == MessageBoxResult.OK)
                                selectedMasterFundPref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(selectedMasterFundPref.MasterFundPreferenceId);
                            else
                                isClose = false;
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show(string.Format("Invalid masterfund preference: {0}{1}\nDo you want to revert this masterfund preference?", selectedMasterFundPref.MasterFundPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                ResetMaterFundPreferencesControl(false);
                                selectedMasterFundPref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(selectedMasterFundPref.MasterFundPreferenceId);
                                //Revert master fund related calculated preferences, remove from updated pref cache as they are reset to default
                                foreach (int calPrefId in selectedMasterFundPref.MasterFundPreference.Values)
                                {
                                    if (UpdatedMFCalculatedpreferences.ContainsKey(calPrefId))
                                        UpdatedMFCalculatedpreferences.Remove(calPrefId);
                                }
                                MasterFundPreferencesControlViewModel.ShowMFAllocationPrefPreview(selectedMasterFundPref);
                            }
                        }
                    }
                }

                if (saveAll)
                {
                    if (selectedMasterFundPref != null)
                    {
                        foreach (int masterFundPreferenceId in UpdatedMasterFundPreferences.Keys)
                        {
                            if (masterFundPreferenceId != selectedMasterFundPref.MasterFundPreferenceId)
                            {
                                if (UpdatedMasterFundPreferences[masterFundPreferenceId].IsValid(out errorMessage) && AreCalculatePreferenceValid(selectedMasterFundPref.MasterFundPreference, out errorMessage))
                                    validMasterFundPreferences.Add(UpdatedMasterFundPreferences[masterFundPreferenceId]);
                                else
                                    invalidMasterFundPreferences.Add(UpdatedMasterFundPreferences[masterFundPreferenceId]);
                            }
                        }
                    }
                    if (invalidMasterFundPreferences != null && invalidMasterFundPreferences.Count > 0)
                    {
                        foreach (AllocationMasterFundPreference mfPreference in invalidMasterFundPreferences)
                        {
                            if (isClosing)
                            {
                                MessageBoxResult result = MessageBox.Show(string.Format("Invalid masterfund preference(s): {0}\n{1}\nChanges will revert for this preference(s)", mfPreference.MasterFundPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                                if (result == MessageBoxResult.OK)
                                    UpdatedMasterFundPreferences[mfPreference.MasterFundPreferenceId] = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(mfPreference.MasterFundPreferenceId);
                                else
                                    isClose = false;
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show(string.Format("Invalid masterfund preference(s): {0}\n{1}\nDo you want to revert this preference(s)?", mfPreference.MasterFundPreferenceName, errorMessage), AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                    UpdatedMasterFundPreferences[mfPreference.MasterFundPreferenceId] = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(mfPreference.MasterFundPreferenceId);
                            }
                        }
                    }
                }

                if (validMasterFundPreferences.Count > 0)
                {
                    Dictionary<int, List<AllocationOperationPreference>> mfCalculatedPrefs = new Dictionary<int, List<AllocationOperationPreference>>();
                    List<AllocationOperationPreference> validMasterFundCalculatedPrefs = new List<AllocationOperationPreference>();

                    validMasterFundPreferences.ForEach(mfPref =>
                     {
                         foreach (int calculatedPrefId in mfPref.MasterFundPreference.Values)
                         {
                             if (UpdatedMFCalculatedpreferences.ContainsKey(calculatedPrefId))
                                 validMasterFundCalculatedPrefs.Add(UpdatedMFCalculatedpreferences[calculatedPrefId]);
                             else
                             {
                                 if (AllocationClientPreferenceManager.GetInstance != null)
                                 {
                                     AllocationOperationPreference cachePreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(calculatedPrefId);
                                     if (cachePreference != null)
                                         validMasterFundCalculatedPrefs.Add(cachePreference.Clone());
                                 }
                             }
                         }

                         if (validMasterFundCalculatedPrefs.Count > 0 && !mfCalculatedPrefs.ContainsKey(mfPref.MasterFundPreferenceId))
                             mfCalculatedPrefs.Add(mfPref.MasterFundPreferenceId, validMasterFundCalculatedPrefs);
                     });

                    if (mfCalculatedPrefs.Count > 0 && AllocationClientPreferenceManager.GetInstance.SaveMasterFundPreferences(validMasterFundPreferences, mfCalculatedPrefs))
                    {
                        StatusBarText = "MasterFund preferences saved successfully";
                        validMasterFundPreferences.ForEach(x => UpdatedMasterFundPreferences.Remove(x.MasterFundPreferenceId));
                        validMasterFundCalculatedPrefs.ForEach(x => UpdatedMFCalculatedpreferences.Remove(x.OperationPreferenceId));
                        ResetMaterFundPreferencesControl();
                    }
                    else
                        StatusBarText = "Nothing to Save";
                }
                else
                    StatusBarText = "Nothing to Save";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isClose;
        }

        /// <summary>
        /// Resets the mater fund preferences control.
        /// </summary>
        /// <param name="isRemoveSelectedMFPref">if set to <c>true</c> [is remove selected mf preference].</param>
        private void ResetMaterFundPreferencesControl(bool isRemoveSelectedMFPref = true)
        {
            try
            {
                if (isRemoveSelectedMFPref)
                    MasterFundPreferencesControlViewModel.CalculatedPreferencesListControlViewModel.ClearSelectedItems();

                MasterFundPreferencesControlViewModel.ClearSelectedItemAndDisableControl();
                MasterFundPreferencesControlViewModel.MasterFundRatioControlViewModel.ClearGridOnly();
                MasterFundPreferencesControlViewModel.MasterFundRatioControlViewModel.IsMasterFundAllocationEnable = false;
                MasterFundPreferencesControlViewModel.MasterFundDefaultRuleViewModel.IsDefaultRuleControlEnabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Are Calculate Preference Valid
        /// </summary>
        /// <param name="serializableDictionary"></param>
        /// <returns></returns>
        private bool AreCalculatePreferenceValid(SerializableDictionary<int, int> masterFundPrefs, out string errorMessage)
        {
            bool isValid = false;
            errorMessage = string.Empty;
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                foreach (int calPrefId in masterFundPrefs.Values)
                {
                    if (UpdatedMFCalculatedpreferences.ContainsKey(calPrefId))
                    {
                        isValid = UpdatedMFCalculatedpreferences[calPrefId].IsValid(out errorMessage);
                        if (!isValid)
                        {
                            errorMessages.Append(string.Format("\nInvalid calculated preference of MasterFund: {0}{1}\n", CachedDataManager.GetInstance.GetMasterFund(masterFundPrefs.FirstOrDefault(x => x.Value == calPrefId).Key), errorMessage));
                        }
                    }
                    else
                    {
                        AllocationOperationPreference cachePreference = null;
                        if (AllocationClientPreferenceManager.GetInstance != null)
                            cachePreference = AllocationClientPreferenceManager.GetInstance.GetAllocationOperationPreference(calPrefId);

                        if (cachePreference != null)
                        {
                            isValid = cachePreference.IsValid(out errorMessage);
                            if (!isValid)
                            {
                                errorMessages.Append(string.Format("\nInvalid calculated preference of MasterFund: {0}{1}", CachedDataManager.GetInstance.GetMasterFund(masterFundPrefs.FirstOrDefault(x => x.Value == calPrefId).Key), errorMessage));
                            }
                        }
                        else
                            errorMessages.Append(string.Format("\nCalculated preference is not defined for MasterFund: {0}", CachedDataManager.GetInstance.GetMasterFund(masterFundPrefs.FirstOrDefault(x => x.Value == calPrefId).Key)));
                    }
                }
                errorMessage = errorMessages.ToString();
                if (errorMessage.Length > 0)
                {
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return isValid;
        }

        /// <summary>
        /// Handles the AllocationMFPreferencesSaved event of the AllocationClientPreferenceManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationMasterFundPreference}"/> instance containing the event data.</param>
        void AllocationClientPreferenceManager_AllocationMFPreferencesSaved(object sender, EventArgs<AllocationMasterFundPreference> e)
        {
            try
            {
                MasterFundPreferencesControlViewModel.UpdateAllocationMFPreference(e.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Exports all master fund preferences.
        /// </summary>
        /// <param name="importExportPath">The import export path.</param>
        //private void ExportAllMasterFundPreferences(string importExportPath)
        //{
        //    try
        //    {
        //        foreach (int key in _masterFundPreferencesViewModel.CalculatedPreferencesListControlViewModel.GetPreferenceIdList())
        //        {
        //            AllocationMasterFundPreference mfPref = new AllocationMasterFundPreference();
        //            mfPref = AllocationClientPreferenceManager.GetInstance.GetAllocationMFPreference(key);
        //            if (mfPref != null)
        //            {
        //                string exportPath = importExportPath + "\\" + mfPref.MasterFundPreferenceName + ".npref";
        //                using (Stream stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write, FileShare.None))
        //                {
        //                    IFormatter formatter = new BinaryFormatter();
        //                    formatter.Serialize(stream, mfPref);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //            throw;
        //    }
        //}

        /// <summary>
        /// Closes the edit allocation preferences UI.
        /// </summary>
        /// <returns></returns>
        internal bool CloseEditAllocationPreferencesUI()
        {
            bool closeUI = true;
            try
            {
                GetSelectedUpdatedPreference();
                GetSelectedUpdatedMFPreference();
                if (UpdatedAllocationpreferences.Count > 0 || FixedAllocationPreferenceControlViewModel.IsModified || UpdatedMasterFundPreferences.Count > 0 || UpdatedMFCalculatedpreferences.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("There are changes in some preference(s). Do you want to revert all preference(s) ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        closeUI = true;
                    else
                        closeUI = false;
                }
                if (DeleteAddedInvalidCalculatedPreferences(true))
                    closeUI = false;
                if (DeleteAddedInvalidMFPreferences())
                    closeUI = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return closeUI;
        }
        #endregion Methods

        #region Dispose Methods and Unwire Events

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    UnwireEvents();

                    if (UpdatedAllocationpreferences != null)
                        UpdatedAllocationpreferences = null;

                    if (UpdatedMFCalculatedpreferences != null)
                        UpdatedMFCalculatedpreferences = null;

                    if (UpdatedMasterFundPreferences != null)
                        UpdatedMasterFundPreferences = null;

                    if (_addedAllocationPreferences != null)
                        _addedAllocationPreferences = null;

                    if (_addedMasterFundAllocationPrefs != null)
                        _addedMasterFundAllocationPrefs = null;

                    if (_fixedAllocationPreferenceControlViewModel != null)
                    {
                        _fixedAllocationPreferenceControlViewModel.Dispose();
                        _fixedAllocationPreferenceControlViewModel = null;
                    }

                    if (_calculatedAllocationPreferenceControlViewModel != null)
                    {
                        _calculatedAllocationPreferenceControlViewModel.Dispose();
                        _calculatedAllocationPreferenceControlViewModel = null;
                    }

                    if (_masterFundPreferencesControlViewModel != null)
                    {
                        _masterFundPreferencesControlViewModel.Dispose();
                        _masterFundPreferencesControlViewModel = null;
                    }

                    if (_masterFundRatioControlViewModel != null)
                    {
                        _masterFundRatioControlViewModel.Dispose();
                        _masterFundRatioControlViewModel = null;
                    }

                    CloseEditAllocationPreferences = null;
                    EditAllocationPreferencesUILoaded = null;
                    FormCloseButton = null;
                    FormClosed = null;
                    SaveEditAllocationPreferenceData = null;
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
        /// Unwires the events.
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                if (AllocationClientPreferenceManager.GetInstance != null)
                {
                    AllocationClientPreferenceManager.GetInstance.AllocationFixedPreferenceUpdated -= AllocationClientPreferenceManager_AllocationFixedPreferenceUpdated;
                    AllocationClientPreferenceManager.GetInstance.AllocationMFPreferencesSaved -= AllocationClientPreferenceManager_AllocationMFPreferencesSaved;
                }
                if (_calculatedAllocationPreferenceControlViewModel != null)
                {
                    _calculatedAllocationPreferenceControlViewModel.PreferenceEvent -= _calculatedAllocationPreferenceControlViewModel_PreferenceEvent;
                    _calculatedAllocationPreferenceControlViewModel.ApplyPreferenceBulkChangeEvent -= _calculatedAllocationPreferenceControlViewModel_ApplyPreferenceBulkChangeEvent;
                    _calculatedAllocationPreferenceControlViewModel.PreviewPreferenceEvent -= _calculatedAllocationPreferenceControlViewModel_PreviewPreferenceEvent;
                    _calculatedAllocationPreferenceControlViewModel.UpdatePreferenceCacheEvent -= _calculatedAllocationPreferenceControlViewModel_UpdatePreferenceCacheEvent;
                }
                if (_fixedAllocationPreferenceControlViewModel != null)
                {
                    _fixedAllocationPreferenceControlViewModel.DeleteAllocationSchemeEvent -= _fixedAllocationPreferenceControlViewModel_DeleteAllocationSchemeEvent;
                    _fixedAllocationPreferenceControlViewModel.SaveFixedPreferences -= _fixedAllocationPreferenceControlViewModel_SaveFixedPreferences;
                }
                if (_masterFundPreferencesControlViewModel != null)
                {
                    _masterFundPreferencesControlViewModel.PreviewPreferenceEvent -= _masterFundPreferencesViewModel_PreviewPreferenceEvent;
                    _masterFundPreferencesControlViewModel.UpdateMFPreferenceCacheEvent -= _masterFundPreferencesViewModel_UpdatePreferenceCacheEvent;
                    _masterFundPreferencesControlViewModel.PreferenceEvent -= _masterFundPreferencesViewModel_PreferenceEvent;
                    _masterFundPreferencesControlViewModel.UpdateCalculatedPrefCacheEvent -= _masterFundPreferencesViewModel_UpdateCalculatedPrefCacheEvent;
                    _masterFundPreferencesControlViewModel.PreviewPreferenceSelectedMF -= _masterFundPreferencesViewModel_PreviewPreferenceSelectedMF;
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