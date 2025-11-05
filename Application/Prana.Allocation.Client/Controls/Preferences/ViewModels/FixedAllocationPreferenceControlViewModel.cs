using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Xml;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class FixedAllocationPreferenceControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [delete allocation scheme event].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> DeleteAllocationSchemeEvent;

        /// <summary>
        /// Occurs when [save fixed preferences].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> SaveFixedPreferences;

        #endregion Events

        #region Members

        /// <summary>
        /// The get scheme BTN clicked
        /// </summary>
        internal bool _getSchemeBtnClicked = false;

        /// <summary>
        /// The _fixed allocation preferences
        /// </summary>
        private ObservableDictionary<int, string> _fixedAllocationPreferences;

        /// <summary>
        /// The _selectedfixed allocation preference
        /// </summary>
        private KeyValuePair<int, string> _selectedfixedAllocationPreference;

        /// <summary>
        /// The _data provider
        /// </summary>
        private XmlDataProvider _dataProvider;

        /// <summary>
        /// The _export grid
        /// </summary>
        private bool _exportGrid;

        /// <summary>
        /// The _previously selectedfixed allocation preference
        /// </summary>
        KeyValuePair<int, string> _previouslySelectedfixedAllocationPref = new KeyValuePair<int, string>();

        /// <summary>
        /// IsModified
        /// </summary>
        private bool _isModified = false;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the data provider.
        /// </summary>
        /// <value>
        /// The data provider.
        /// </value>
        public XmlDataProvider DataProvider
        {
            get { return _dataProvider; }
            set
            {
                _dataProvider = value;
                RaisePropertyChangedEvent("DataProvider");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGrid
        {
            get { return _exportGrid; }
            set
            {
                _exportGrid = value;
                RaisePropertyChangedEvent("ExportGrid");
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
        /// IsModified
        /// </summary>
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                RaisePropertyChangedEvent("IsModified");
            }
        }

        /// <summary>
        /// Gets or sets the selectedfixed allocation preference.
        /// </summary>
        /// <value>
        /// The selectedfixed allocation preference.
        /// </value>
        public KeyValuePair<int, string> SelectedfixedAllocationPreference
        {
            get { return _selectedfixedAllocationPreference; }
            set
            {
                _previouslySelectedfixedAllocationPref = _selectedfixedAllocationPreference;
                _getSchemeBtnClicked = false;
                _selectedfixedAllocationPreference = value;
                RaisePropertyChangedEvent("SelectedfixedAllocationPreference");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the delete allocation scheme command.
        /// </summary>
        /// <value>
        /// The delete allocation scheme command.
        /// </value>
        public RelayCommand<object> DeleteAllocationSchemeCommand { get; set; }

        /// <summary>
        /// Gets or sets the export allocation scheme command.
        /// </summary>
        /// <value>
        /// The export allocation scheme command.
        /// </value>
        public RelayCommand<object> ExportAllocationSchemeCommand { get; set; }

        /// <summary>
        /// Gets or sets the load fixed preference grid.
        /// </summary>
        /// <value>
        /// The load fixed preference grid.
        /// </value>
        public RelayCommand<object> LoadFixedPreferenceGridCommand { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedAllocationPreferenceControlViewModel"/> class.
        /// </summary>
        public FixedAllocationPreferenceControlViewModel()
        {
            try
            {
                LoadFixedPreferenceGridCommand = new RelayCommand<object>((parameter) => LoadFixedPreferenceGrid(parameter));
                ExportAllocationSchemeCommand = new RelayCommand<object>((parameter) => ExportAllocationScheme(parameter));
                DeleteAllocationSchemeCommand = new RelayCommand<object>((parameter) => DeleteAllocationScheme(parameter));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object DeleteAllocationScheme(object parameter)
        {
            try
            {
                if (SelectedfixedAllocationPreference.Value != null && !string.IsNullOrWhiteSpace(SelectedfixedAllocationPreference.Value.Trim()))
                {
                    MessageBoxResult result = MessageBox.Show("Do you really want to delete the " + SelectedfixedAllocationPreference.Value + " preference?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result.Equals(MessageBoxResult.Yes))
                    {
                        if (DeleteAllocationSchemeEvent != null)
                            DeleteAllocationSchemeEvent(this, new EventArgs<KeyValuePair<int, string>>(SelectedfixedAllocationPreference));
                        SelectedfixedAllocationPreference = new KeyValuePair<int, string>();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a fixed preference", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Exports the allocation scheme.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ExportAllocationScheme(object parameter)
        {
            try
            {
                if (DataProvider == null)
                    MessageBox.Show("Please select an allocation scheme to export", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    ExportGrid = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Loads the fixed preference grid.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object LoadFixedPreferenceGrid(object parameter)
        {
            try
            {
                if (SelectedfixedAllocationPreference.Value != null && !string.IsNullOrWhiteSpace(SelectedfixedAllocationPreference.Value.Trim()))
                {
                    if (IsModified)
                        SaveFixedAllocationPrefrence();

                    string xmlString = AllocationClientPreferenceManager.GetInstance.GetAllocationSchemeByName(SelectedfixedAllocationPreference.Value);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlString);
                    XmlDataProvider xmlDataProvider = new XmlDataProvider();
                    if (xmlDataProvider != null)
                    {
                        xmlDataProvider.Document = xmlDoc;
                        xmlDataProvider.XPath = "/DocumentElement";
                    }

                    DataProvider = xmlDataProvider;
                    _getSchemeBtnClicked = true;
                }
                else
                    MessageBox.Show("Please select a fixed preference", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load get scheme list].
        /// </summary>
        /// <param name="fixedPreferenceListDictionary">The fixed preference list dictionary.</param>
        internal void OnLoadGetSchemeList(Dictionary<int, string> fixedPreferenceListDictionary)
        {
            try
            {
                KeyValuePair<int, string> selectedPreference = SelectedfixedAllocationPreference;
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
        /// Saves the fixed allocation prefrence.
        /// </summary>
        internal void SaveFixedAllocationPrefrence()
        {
            try
            {
                if (IsModified)
                {
                    KeyValuePair<int, string> selectedPreference = SelectedfixedAllocationPreference;
                    if (!_getSchemeBtnClicked)
                        selectedPreference = _previouslySelectedfixedAllocationPref;

                    MessageBoxResult result = MessageBox.Show("Do you want to save recent changes in fixed preferences ?", AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SaveFixedPreferences != null)
                            SaveFixedPreferences(this, new EventArgs<KeyValuePair<int, string>>(selectedPreference));
                    }
                    else
                        IsModified = false;
                }
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
                    _dataProvider = null;
                    _fixedAllocationPreferences = null;
                    DeleteAllocationSchemeCommand = null;
                    ExportAllocationSchemeCommand = null;
                    LoadFixedPreferenceGridCommand = null;
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
