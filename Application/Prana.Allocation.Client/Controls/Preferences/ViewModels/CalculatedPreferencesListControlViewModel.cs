using Infragistics.Windows.Controls;
using Microsoft.Win32;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Preferences.Views;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.EventArguments;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class CalculatedPreferencesListControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [preference bulk change event].
        /// </summary>
        public event ApplyBulkChangeHandler PreferenceBulkChangeEvent;

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
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        public delegate void ApplyBulkChangeHandler(Object sender, ApplyBulkChangeEventArgs e);

        #endregion Delegates

        #region Members

        /// <summary>
        /// The _allocation preference collection
        /// </summary>
        private BindingListCollectionView _allocationPreferenceCollection;

        /// <summary>
        /// The _selected allocation preferences
        /// </summary>
        private object _selectedAllocationPreferences;

        /// <summary>
        /// The _bulk change control view model
        /// </summary>
        private BulkChangeControlViewModel _bulkChangeControlViewModel;

        /// <summary>
        /// The _item context menu visibility
        /// </summary>
        private Visibility _itemContextMenuVisibility;

        /// <summary>
        /// The _context menu visibility
        /// </summary>
        private Visibility _contextMenuVisibility;

        /// <summary>
        /// The _item enter edit mode
        /// </summary>
        private bool _itemEnterEditMode;

        /// <summary>
        /// The _end edit mode
        /// </summary>
        private bool _endEditMode;

        /// <summary>
        /// Captures the user action
        /// </summary>
        private AllocationPrefOperation _userAction;

        /// <summary>
        /// The _new preference identifier
        /// </summary>
        private int _newPreferenceID = -2;

        /// <summary>
        /// The _copy preference identifier
        /// </summary>
        private int _copyPreferenceID = -1;

        /// <summary>
        /// The _copy from preference identifier
        /// </summary>
        private int _copyFromPreferenceID = int.MinValue;

        /// <summary>
        /// The _rename preference name
        /// </summary>
        private string _renamePreferenceName = string.Empty;

        /// <summary>
        /// The bulk change UI
        /// </summary>
        private static BulkChangeControlViewModel _bulkChangeUI = null;

        /// <summary>
        /// The _enable disable tool bar
        /// </summary>
        private bool _enableDisableToolBar;

        /// <summary>
        /// The _is enable menu for mf
        /// </summary>
        private bool _isEnabledMenuForMF = true;

        /// <summary>
        /// The _is visible button for mf
        /// </summary>
        private Visibility _isVisibleButtonForMF = Visibility.Visible;

        /// <summary>
        /// The _selected allocation preference item
        /// </summary>
        private object _selectedAllocationPreferenceItem;

        public bool _exportGrid;

        public bool ExportGrid
        {
            get { return _exportGrid; }
            set
            {
                _exportGrid = value;
                RaisePropertyChangedEvent("ExportGrid");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; RaisePropertyChangedEvent("ExportFilePathForAutomation"); }
        }

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the allocation preference collection.
        /// </summary>
        /// <value>
        /// The allocation preference collection.
        /// </value>
        public BindingListCollectionView AllocationPreferenceCollection
        {
            get { return _allocationPreferenceCollection; }
            set
            {
                _allocationPreferenceCollection = value;
                RaisePropertyChangedEvent("AllocationPreferenceCollection");
            }
        }

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
        /// Gets or sets the context menu visibility.
        /// </summary>
        /// <value>
        /// The context menu visibility.
        /// </value>
        public Visibility ContextMenuVisibility
        {
            get { return _contextMenuVisibility; }
            set
            {
                _contextMenuVisibility = value;
                RaisePropertyChangedEvent("ContextMenuVisibility");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditMode
        {
            get { return _endEditMode; }
            set
            {
                _endEditMode = value;
                RaisePropertyChangedEvent("EndEditMode");
            }
        }

        /// <summary>
        /// Gets or sets the item context menu visibility.
        /// </summary>
        /// <value>
        /// The item context menu visibility.
        /// </value>
        public Visibility ItemContextMenuVisibility
        {
            get { return _itemContextMenuVisibility; }
            set
            {
                _itemContextMenuVisibility = value;
                RaisePropertyChangedEvent("ItemContextMenuVisibility");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [item enter edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [item enter edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool ItemEnterEditMode
        {
            get { return _itemEnterEditMode; }
            set
            {
                _itemEnterEditMode = value;
                RaisePropertyChangedEvent("ItemEnterEditMode");
            }
        }

        /// <summary>
        /// Gets or sets the selected allocation preferences.
        /// </summary>
        /// <value>
        /// The selected allocation preferences.
        /// </value>
        public object SelectedAllocationPreferences
        {
            get { return _selectedAllocationPreferences; }
            set
            {
                if (_selectedAllocationPreferences != null && ((DictionaryImpersonation<int, string>)_selectedAllocationPreferences).Value != null)
                    UpdatePreferenceCache();

                _selectedAllocationPreferences = value;

                if (_selectedAllocationPreferences != null)
                    PreferencePreview();

                RaisePropertyChangedEvent("SelectedAllocationPreferences");
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
        /// Gets or sets a value indicating whether this instance is enabled menu for mf.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled menu for mf; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledMenuForMF
        {
            get { return _isEnabledMenuForMF; }
            set
            {
                _isEnabledMenuForMF = value;
                RaisePropertyChangedEvent("IsEnabledMenuForMF");
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

        /// <summary>
        /// Gets or sets the selected allocation preference item.
        /// </summary>
        /// <value>
        /// The selected allocation preference item.
        /// </value>
        public object SelectedAllocationPreferenceItem
        {
            get { return _selectedAllocationPreferenceItem; }
            set
            {
                _selectedAllocationPreferenceItem = value;
                RaisePropertyChangedEvent("SelectedAllocationPreferenceItem");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the context menu opening.
        /// </summary>
        /// <value>
        /// The context menu opening.
        /// </value>
        public RelayCommand<object> ContextMenuOpening { get; set; }

        /// <summary>
        /// Gets or sets the edit mode ended command.
        /// </summary>
        /// <value>
        /// The edit mode ended command.
        /// </value>
        public RelayCommand<object> EditModeEndedCommand { get; set; }

        /// <summary>
        /// Gets or sets the load bulk change UI.
        /// </summary>
        /// <value>
        /// The load bulk change UI.
        /// </value>
        public RelayCommand<object> LoadBulkChangeUI { get; set; }

        /// <summary>
        /// Gets or sets the preference context menu clicked.
        /// </summary>
        /// <value>
        /// The preference context menu clicked.
        /// </value>
        public RelayCommand<object> PreferenceContextMenuClicked { get; set; }

        /// <summary>
        /// Gets or sets the tool bar click command.
        /// </summary>
        /// <value>
        /// The tool bar click command.
        /// </value>
        public RelayCommand<object> ToolBarClickCommand { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedPreferencesListControlViewModel"/> class.
        /// </summary>
        public CalculatedPreferencesListControlViewModel()
        {
            try
            {
                PreferenceContextMenuClicked = new RelayCommand<object>((parameter) => OnPreferenceContextMenuClick(parameter));
                LoadBulkChangeUI = new RelayCommand<object>((parameter) => LoadBulkChangeUIClick(parameter));
                ContextMenuOpening = new RelayCommand<object>((parameter) => BeforeContextMenuOpening(parameter));
                EditModeEndedCommand = new RelayCommand<object>((parameter) => EditModeEnded(parameter));
                ToolBarClickCommand = new RelayCommand<object>((parameter) => OnToolBarClicked(parameter));
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
        /// Adds the preference.
        /// </summary>
        internal void AddPreference(DictionaryImpersonation<int, string> preference)
        {
            try
            {
                string newPreferenceName = preference.Value;
                if (IsValidPreferenceName(preference))
                {
                    if (PreferenceEvent != null)
                        PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Add, PrefName = newPreferenceName, PrefId = _newPreferenceID }));
                }
                else
                    RemoveFromCollection(_newPreferenceID);

                EndEditMode = true;
                EnableDisableToolBar = true;
                _userAction = AllocationPrefOperation.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the allocation preference collection.
        /// </summary>
        /// <param name="prefKey">The preference key.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <param name="oldPrefKey">The old preference key.</param>
        internal void AddRemoveUpdatePrefCollection(int prefKey, string prefName, int oldPrefKey)
        {
            try
            {
                if (oldPrefKey == _newPreferenceID)
                    RemoveFromCollection(_newPreferenceID);
                else if (oldPrefKey == _copyPreferenceID)
                    RemoveFromCollection(_copyPreferenceID);

                DictionaryImpersonation<int, string> preference = new DictionaryImpersonation<int, string>(prefKey, prefName);
                if (!((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Contains(preference))
                    ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Add(preference);
                else if (((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Contains(preference))
                    ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).UpdateItem(preference);

                SelectedAllocationPreferences = preference;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Befores the context menu opening.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object BeforeContextMenuOpening(object parameter)
        {
            try
            {
                object source = ((ContextMenuEventArgs)parameter).OriginalSource;
                if (source is SimpleTextBlock)
                {
                    ItemContextMenuVisibility = Visibility.Visible;
                    ContextMenuVisibility = Visibility.Collapsed;
                }
                else
                {
                    ItemContextMenuVisibility = Visibility.Collapsed;
                    ContextMenuVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [tool bar clicked].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private object OnToolBarClicked(object parameter)
        {
            try
            {
                if (!EnableDisableToolBar)
                    EndEditMode = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Handles the ApplyPref event of the BulkChangeControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        void BulkChangeControlViewModel_ApplyPref(object sender, ApplyBulkChangeEventArgs e)
        {
            try
            {
                if (PreferenceBulkChangeEvent != null)
                    PreferenceBulkChangeEvent(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CloseBulkChangeForm event of the CalculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void CalculatedPreferencesListControlViewModel_CloseBulkChangeForm(object sender, EventArgs e)
        {
            try
            {
                if (_bulkChangeUI != null)
                    _bulkChangeUI = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Collections the contains identifier.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal bool CollectionContainsId(int prefId)
        {
            bool isContains = false;
            try
            {
                isContains = ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Any(x => x.Key == prefId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isContains;
        }

        /// <summary>
        /// Collections the name of the contains.
        /// </summary>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        internal bool CollectionContainsName(string prefName)
        {
            bool isContains = false;
            try
            {
                isContains = ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Any(x => x.Value == prefName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isContains;
        }

        /// <summary>
        /// Copies the preference.
        /// </summary>
        /// <param name="preference">The preference.</param>
        internal void CopyPreference(DictionaryImpersonation<int, string> preference)
        {
            try
            {
                string copiedPreferenceName = preference.Value;
                if (IsValidPreferenceName(preference))
                {
                    if (PreferenceEvent != null)
                        PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Copy, PrefName = copiedPreferenceName, PrefId = _copyPreferenceID, CopyPrefId = _copyFromPreferenceID }));
                }
                else
                    RemoveFromCollection(_copyPreferenceID);

                EndEditMode = true;
                EnableDisableToolBar = true;
                _userAction = AllocationPrefOperation.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes the preference.
        /// </summary>
        internal void DeletePreference()
        {
            try
            {
                if (SelectedAllocationPreferences != null && ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Contains((DictionaryImpersonation<int, string>)SelectedAllocationPreferences))
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to delete " + ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Value + " preference?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result.Equals(MessageBoxResult.Yes))
                    {
                        if (PreferenceEvent != null) PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs
                        {
                            AllocationPrefOperation = AllocationPrefOperation.Delete,
                            PrefId = ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Key
                        }));
                    }
                }
                else
                    MessageBox.Show("Preference does not exists. So, can not delete.", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);

                _userAction = AllocationPrefOperation.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
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
                if (CollectionContainsId(prefKey))
                {
                    RemoveFromCollection(prefKey);
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (AllocationClientPreferenceManager.GetInstance != null)
                        AllocationClientPreferenceManager.GetInstance.AllocationOperationPreferenceUpdated -= GetInstance_AllocationOperationPreferenceUpdated;
                    if (BulkChangeControlViewModel.GetInstance() != null)
                    {
                        BulkChangeControlViewModel.GetInstance().ApplyPref -= BulkChangeControlViewModel_ApplyPref;
                        BulkChangeControlViewModel.GetInstance().CloseBulkChangeForm -= CalculatedPreferencesListControlViewModel_CloseBulkChangeForm;
                        BulkChangeControlViewModel.GetInstance().Dispose();
                    }
                    _bulkChangeUI = null;
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
        /// Edits the mode ended.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object EditModeEnded(object parameter)
        {
            try
            {
                DictionaryImpersonation<int, string> preference = (DictionaryImpersonation<int, string>)parameter ?? new DictionaryImpersonation<int, string>(int.MinValue, string.Empty);
                switch (_userAction)
                {
                    case AllocationPrefOperation.Add:
                        AddPreference(preference);
                        break;

                    case AllocationPrefOperation.Rename:
                        RenamePreference(preference);
                        break;

                    case AllocationPrefOperation.Copy:
                        CopyPreference(preference);
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
        /// Exports all preference.
        /// </summary>
        internal void ExportAllPreference()
        {
            try
            {
                //TODO: Replace windows forms folder brower with wpf browser if exists
                System.Windows.Forms.FolderBrowserDialog exportBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (exportBrowserDialog.ShowDialog().ToString().Equals(MessageBoxResult.OK.ToString()))
                {
                    string importExportPath = exportBrowserDialog.SelectedPath;
                    if (PreferenceEvent != null)
                        PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.ExportAll, ImportExportPath = new List<string> { importExportPath } }));
                }

                _userAction = AllocationPrefOperation.None;
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
        internal void ExportPreference()
        {
            try
            {
                if (SelectedAllocationPreferences != null)
                {
                    SaveFileDialog exportBrowserDialog = new SaveFileDialog
                    {
                        Title = "Choose location to save your file",
                        DefaultExt = ".npref",
                        Filter = "Nirvana Allocation preference files (*.npref)|*.npref",
                        FilterIndex = 1,
                        CheckPathExists = true,
                        FileName = ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Value
                    };
                    if ((bool)exportBrowserDialog.ShowDialog())
                    {
                        List<string> importExportPath = exportBrowserDialog.FileNames.ToList();
                        if (PreferenceEvent != null)
                            PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Export, PrefId = ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Key, ImportExportPath = importExportPath }));
                    }
                }
                else
                    MessageBox.Show("Please select a preference", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);

                _userAction = AllocationPrefOperation.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AllocationOperationPreferenceUpdated event of the GetInstance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        void GetInstance_AllocationOperationPreferenceUpdated(object sender, EventArgs<Dictionary<int, string>> e)
        {
            try
            {
                if (e.Value != null)
                {
                    if (e.Value.ContainsKey(int.MinValue))
                        e.Value.Remove(int.MinValue);
                    GenericBindingList<DictionaryImpersonation<int, string>> list = new GenericBindingList<DictionaryImpersonation<int, string>>();
                    e.Value.ToList().ForEach(x => list.Add(new DictionaryImpersonation<int, string>(x.Key, x.Value)));
                    AllocationPreferenceCollection = new BindingListCollectionView(list);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets the preference identifier list.
        /// </summary>
        /// <returns></returns>
        internal List<int> GetPreferenceIdList()
        {
            List<int> preferenceIdList = new List<int>();
            try
            {
                preferenceIdList = ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Select(x => x.Key).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceIdList;
        }

        /// <summary>
        /// Imports the preference.
        /// </summary>
        internal void ImportPreference()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Nirvana Allocation preference files (*.npref)|*.npref",
                    FilterIndex = 1,
                    Multiselect = true
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    List<string> importExportPaths = openFileDialog.FileNames.ToList();

                    if (PreferenceEvent != null)
                        PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Import, ImportExportPath = importExportPaths }));

                }

                _userAction = AllocationPrefOperation.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check if preference name is valid
        /// </summary>
        /// <param name="preference">The preference.</param>
        /// <returns></returns>
        private bool IsValidPreferenceName(DictionaryImpersonation<int, string> preference)
        {
            string preferenceName = preference.Value;
            string error = String.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(preferenceName))
                    error = "Preference name should not be blank.";
                else if (((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Any(x => x.Value.ToUpper() == preference.Value.ToUpper() && x.Key != preference.Key))
                    error = "Preference with same name already exist.";
                else if (preferenceName.Length > 50)
                    error = "Preference Name can not be greater than 50 characters";
                else if (preferenceName.StartsWith("*Custom#_"))
                    error = "Preference Name can not start with *Custom#_";
                else if (preferenceName.StartsWith("*WorkArea#_"))
                    error = "Preference Name can not start with *WorkArea#__";
                else if (preferenceName.StartsWith("*PTT#_"))
                    error = "Preference Name can not be start with *PTT#_";

                if (!String.IsNullOrWhiteSpace(error))
                {
                    MessageBox.Show(error, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Loads the bulk change UI click.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadBulkChangeUIClick(object parameter)
        {
            try
            {
                if (_bulkChangeUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowBulkChangeUI(viewModel => dialogService.Show<BulkChangeControl>(this, viewModel));
                }
                else
                    _bulkChangeUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Bulk Change Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load calculated preferences list control].
        /// </summary>
        internal void OnLoadCalculatedPreferencesListControl(Dictionary<int, string> preferenceList)
        {
            try
            {
                if (preferenceList.ContainsKey(int.MinValue))
                    preferenceList.Remove(int.MinValue);
                GenericBindingList<DictionaryImpersonation<int, string>> list = new GenericBindingList<DictionaryImpersonation<int, string>>();
                preferenceList.ToList().ForEach(x => list.Add(new DictionaryImpersonation<int, string>(x.Key, x.Value)));
                AllocationPreferenceCollection = new BindingListCollectionView(list);
                EnableDisableToolBar = true;

                //bind events
                AllocationClientPreferenceManager.GetInstance.AllocationOperationPreferenceUpdated += GetInstance_AllocationOperationPreferenceUpdated;
                BulkChangeControlViewModel.GetInstance().ApplyPref += BulkChangeControlViewModel_ApplyPref;
                BulkChangeControlViewModel.GetInstance().CloseBulkChangeForm += CalculatedPreferencesListControlViewModel_CloseBulkChangeForm;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [preference context menu click].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnPreferenceContextMenuClick(object parameter)
        {
            try
            {
                EndEditMode = true;
                EnableDisableToolBar = true;
                _userAction = (AllocationPrefOperation)Enum.Parse(typeof(AllocationPrefOperation), parameter.ToString());
                switch (_userAction)
                {
                    case AllocationPrefOperation.Add:
                        ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Add(new DictionaryImpersonation<int, string>(_newPreferenceID, String.Empty));
                        SelectedAllocationPreferences = new DictionaryImpersonation<int, string>(_newPreferenceID, String.Empty);
                        EnableDisableToolBar = false;
                        ItemEnterEditMode = true;
                        break;

                    case AllocationPrefOperation.Delete:
                        DeletePreference();
                        break;

                    case AllocationPrefOperation.Copy:
                        if (SelectedAllocationPreferences != null)
                        {
                            _copyFromPreferenceID = ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Key;
                            ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Add(new DictionaryImpersonation<int, string>(_copyPreferenceID, String.Empty));
                            SelectedAllocationPreferences = new DictionaryImpersonation<int, string>(_copyPreferenceID, String.Empty);
                            EnableDisableToolBar = false;
                            ItemEnterEditMode = true;
                        }
                        else
                        {
                            MessageBox.Show("Please select a preference", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                            _userAction = AllocationPrefOperation.None;
                        }
                        break;

                    case AllocationPrefOperation.Rename:
                        if (SelectedAllocationPreferences != null)
                        {
                            _renamePreferenceName = ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Value;
                            EnableDisableToolBar = false;
                            ItemEnterEditMode = true;
                        }
                        else
                        {
                            MessageBox.Show("Please select a preference", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                            _userAction = AllocationPrefOperation.None;
                        }
                        break;

                    case AllocationPrefOperation.Import:
                        ImportPreference();
                        break;

                    case AllocationPrefOperation.Export:
                        ExportPreference();
                        break;

                    case AllocationPrefOperation.ExportAll:
                        ExportAllPreference();
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
        /// Preferences the preview.
        /// </summary>
        private void PreferencePreview()
        {
            try
            {
                if (SelectedAllocationPreferences != null && ((DictionaryImpersonation<int, string>)SelectedAllocationPreferences).Value != null)
                {
                    DictionaryImpersonation<int, string> selectedPreference = (DictionaryImpersonation<int, string>)SelectedAllocationPreferences;
                    if (PreviewPreferenceEvent != null)
                        PreviewPreferenceEvent(this, new EventArgs<KeyValuePair<int, string>>(new KeyValuePair<int, string>(selectedPreference.Key, selectedPreference.Value)));
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
        /// Removes from collection.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        internal void RemoveFromCollection(int prefId)
        {
            try
            {
                ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).Remove(((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).FirstOrDefault(x => x.Key == prefId));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Renames the preference.
        /// </summary>
        internal void RenamePreference(DictionaryImpersonation<int, string> preference)
        {
            try
            {
                string renamedPreferenceName = preference.Value;
                if (!_renamePreferenceName.Equals(renamedPreferenceName) && IsValidPreferenceName(preference))
                {
                    if (PreferenceEvent != null)
                        PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(new AllocationPrefOperationEventArgs { AllocationPrefOperation = AllocationPrefOperation.Rename, PrefName = renamedPreferenceName, PrefId = preference.Key }));

                    EndEditMode = true;
                    EnableDisableToolBar = true;
                    _userAction = AllocationPrefOperation.None;
                }
                else
                {
                    if (!_renamePreferenceName.Equals(renamedPreferenceName))
                    {
                        ((GenericBindingList<DictionaryImpersonation<int, string>>)AllocationPreferenceCollection.SourceCollection).FirstOrDefault(x => x.Key == preference.Key).Value = _renamePreferenceName;
                        ItemEnterEditMode = true;
                    }
                    else
                    {
                        EndEditMode = true;
                        EnableDisableToolBar = true;
                        _userAction = AllocationPrefOperation.None;
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
        /// Shows the bulk change UI.
        /// </summary>
        /// <param name="showBulkChangeUIViewModel">The show bulk change UI view model.</param>
        private static void ShowBulkChangeUI(Action<BulkChangeControlViewModel> showBulkChangeUIViewModel)
        {
            try
            {
                _bulkChangeUI = BulkChangeControlViewModel.GetInstance();
                showBulkChangeUIViewModel(_bulkChangeUI);

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates the preference cache.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdatePreferenceCache()
        {
            try
            {
                if (SelectedAllocationPreferences != null)
                {
                    DictionaryImpersonation<int, string> selectedPreference = (DictionaryImpersonation<int, string>)SelectedAllocationPreferences;
                    if (UpdatePreferenceCacheEvent != null)
                        UpdatePreferenceCacheEvent(this, new EventArgs<KeyValuePair<int, string>>(new KeyValuePair<int, string>(selectedPreference.Key, selectedPreference.Value)));
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
        /// Clears the selected items.
        /// </summary>
        internal void ClearSelectedItems()
        {
            try
            {
                _selectedAllocationPreferences = null;
                _selectedAllocationPreferenceItem = null;
                RaisePropertyChangedEvent("SelectedAllocationPreferences");
                RaisePropertyChangedEvent("SelectedAllocationPreferenceItem");
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
