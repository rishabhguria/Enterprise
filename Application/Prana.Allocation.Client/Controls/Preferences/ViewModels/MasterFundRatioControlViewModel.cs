using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    /// <summary>
    /// MasterFund Ratio Control ViewModel
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase"/>
    public class MasterFundRatioControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _master fund ratio collection
        /// </summary>
        private DataTable _masterFundRatioCollection;

        /// <summary>
        /// The _one mf one symbol preference visible
        /// </summary>
        private Visibility _oneMFOneSymbolPreferenceVisible;

        /// <summary>
        /// The _selected master fund item
        /// </summary>
        private object _selectedMasterFundItem;

        /// <summary>
        /// The is master fund allocation enable
        /// </summary>
        private bool _isMasterFundAllocationEnable;

        /// <summary>
        /// The selected mf list
        /// </summary>
        private Dictionary<int, string> _selectedMFList = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the master fund grid changed.
        /// </summary>
        /// <value>
        /// The master fund grid changed.
        /// </value>
        public RelayCommand<object> MasterFundGridCellUpdated { get; set; }

        /// <summary>
        /// IsModified
        /// </summary>
        private bool _isModified = false;
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
        /// Occurs when [selected master fund list changed].
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, string>>> SelectedMasterFundListChanged;

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
        /// Gets or sets the master fund ratio collection.
        /// </summary>
        /// <value>
        /// The master fund ratio collection.
        /// </value>
        public DataTable MasterFundRatioCollection
        {
            get { return _masterFundRatioCollection; }
            set
            {
                _masterFundRatioCollection = value;
                RaisePropertyChangedEvent("MasterFundRatioCollection");
            }
        }

        /// <summary>
        /// Gets or sets the one mf one symbol preference visible.
        /// </summary>
        /// <value>
        /// The one mf one symbol preference visible.
        /// </value>
        public Visibility OneMFOneSymbolPreferenceVisible
        {
            get { return _oneMFOneSymbolPreferenceVisible; }
            set
            {
                _oneMFOneSymbolPreferenceVisible = value;
                RaisePropertyChangedEvent("OneMFOneSymbolPreferenceVisible");
            }
        }

        /// <summary>
        /// Gets or sets the selected master fund item.
        /// </summary>
        /// <value>
        /// The selected master fund item.
        /// </value>
        public object SelectedMasterFundItem
        {
            get { return _selectedMasterFundItem; }
            set
            {
                _selectedMasterFundItem = value;
                RaisePropertyChangedEvent("SelectedMasterFundItem");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is master fund allocation enable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is master fund allocation enable; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterFundAllocationEnable
        {
            get { return _isMasterFundAllocationEnable; }
            set
            {
                _isMasterFundAllocationEnable = value;
                RaisePropertyChangedEvent("IsMasterFundAllocationEnable");
            }
        }

        /// <summary>
        /// Gets or sets the selected mf list.
        /// </summary>
        /// <value>
        /// The selected mf list.
        /// </value>
        public Dictionary<int, string> SelectedMFList
        {
            get { return _selectedMFList; }
            set
            {
                _selectedMFList = value;
                if (SelectedMasterFundListChanged != null)
                    SelectedMasterFundListChanged(this, new EventArgs<Dictionary<int, string>>(_selectedMFList));
            }
        }

        /// <summary>
        /// EditModeEnded Command
        /// </summary>
        public RelayCommand<object> EditModeEnd { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundRatioControlViewModel"/> class.
        /// </summary>
        public MasterFundRatioControlViewModel()
        {
            EditModeEnd = new RelayCommand<object>((parameter) => AddToSelectedMFList(parameter));
            MasterFundGridCellUpdated = new RelayCommand<object>((parameter) => MasterFundGridCellChanged(parameter));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Masterfund grid edited.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private void MasterFundGridCellChanged(object parameter)
        {
            try
            {
                IsModified = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [load master fund ratio control].
        /// </summary>
        /// <param name="masterFundRatioSet">The master fund ratio set.</param>
        /// <param name="isMasterFundRatioAllocation">if set to <c>true</c> [is master fund ratio allocation].</param>
        /// <param name="isOneMFOneSymbolPreferenceVisible">if set to <c>true</c> [is one mf one symbol preference visible].</param>
        internal void OnLoadMasterFundRatioControl(DataTable masterFundRatioSet, bool isMasterFundRatioAllocation, bool isOneMFOneSymbolPreferenceVisible)
        {
            try
            {
                MasterFundRatioCollection = masterFundRatioSet;
                IsMasterFundAllocationEnable = isMasterFundRatioAllocation;
                OneMFOneSymbolPreferenceVisible = isOneMFOneSymbolPreferenceVisible ? Visibility.Visible : Visibility.Collapsed;
                SelectedMFList = new Dictionary<int, string>();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the mf grid values.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, decimal> GetMFGridValues()
        {
            SerializableDictionary<int, decimal> mfGridValue = new SerializableDictionary<int, decimal>();
            try
            {
                foreach (DataRow row in MasterFundRatioCollection.Rows)
                {
                    decimal mfValue = Convert.ToDecimal(row[AllocationUIConstants.TARGET_RATIO_PCT]);
                    if (mfValue != 0.0M)
                        mfGridValue.Add(Convert.ToInt32(row[AllocationUIConstants.COMPANY_MASTER_FUND_ID]), mfValue);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfGridValue;
        }

        /// <summary>
        /// Sets the values in master fund grid.
        /// </summary>
        internal void SetValuesInMasterFundGrid(SerializableDictionary<int, decimal> masterFundRatioSet)
        {
            try
            {
                lock (MasterFundRatioCollection)
                {
                    ClearGridOnly();
                    IsMasterFundAllocationEnable = true;
                    foreach (DataRow row in MasterFundRatioCollection.Rows)
                    {
                        int masterFundId = Convert.ToInt32(row[AllocationUIConstants.COMPANY_MASTER_FUND_ID]);
                        if (masterFundRatioSet.ContainsKey(masterFundId) && masterFundRatioSet[masterFundId] != 0)
                        {
                            row[AllocationUIConstants.TARGET_RATIO_PCT] = masterFundRatioSet[masterFundId];
                            if (!SelectedMFList.ContainsKey(masterFundId))
                                SelectedMFList.Add(masterFundId, row[AllocationUIConstants.MASTER_FUND_NAME].ToString());
                        }
                    }
                }
                //while setting values raise this event only if masterFund dictionary conatins any values
                if (SelectedMFList.Count > 0)
                {
                    if (SelectedMasterFundListChanged != null)
                        SelectedMasterFundListChanged(this, new EventArgs<Dictionary<int, string>>(SelectedMFList));
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the grid
        /// </summary>
        internal void ClearGridOnly()
        {
            try
            {
                foreach (DataRow row in MasterFundRatioCollection.Rows)
                {
                    row[AllocationUIConstants.TARGET_RATIO_PCT] = 0;
                }
                SelectedMFList = new Dictionary<int, string>();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds to selected mf list.
        /// </summary>
        private void AddToSelectedMFList(object parameter)
        {
            try
            {
                DataRowView dr = ((DataRowView)parameter);
                if (Convert.ToDecimal(dr[AllocationUIConstants.TARGET_RATIO_PCT]) != 0.0M)
                {
                    if (!SelectedMFList.ContainsKey(Convert.ToInt32(dr[AllocationUIConstants.COMPANY_MASTER_FUND_ID])))
                        SelectedMFList.Add(Convert.ToInt32(dr[AllocationUIConstants.COMPANY_MASTER_FUND_ID]), dr[AllocationUIConstants.MASTER_FUND_NAME].ToString());
                }
                else
                {
                    if (SelectedMFList.ContainsKey(Convert.ToInt32(dr[AllocationUIConstants.COMPANY_MASTER_FUND_ID])))
                        SelectedMFList.Remove(Convert.ToInt32(dr[AllocationUIConstants.COMPANY_MASTER_FUND_ID]));
                }

                if (SelectedMasterFundListChanged != null)
                    SelectedMasterFundListChanged(this, new EventArgs<Dictionary<int, string>>(SelectedMFList));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
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
                    _masterFundRatioCollection = null;
                    _selectedMasterFundItem = null;
                    _selectedMFList = null;
                    MasterFundGridCellUpdated = null;
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
