using Infragistics.Controls.Editors;
using Newtonsoft.Json;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class TradeAttributeBulkChangeControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [update groups level].
        /// </summary>
        public event EventHandler<EventArgs<TradeAttributes, List<object>>> UpdateGroupsLevel;

        /// <summary>
        /// Event to trigger when Taxlot level radio button is clicked
        /// </summary>
        public event GetAllocationGroupsHandler OnGetAllocationGroups;

        #endregion Events

        #region Delegates

        /// <summary>
        /// Delegate to get selected rows from AllocationGridControlViewModel
        /// </summary>
        /// <returns></returns>
        public delegate List<AllocationGroup> GetAllocationGroupsHandler();

        #endregion

        #region Members

        /// <summary>
        /// The _master funds
        /// </summary>
        private ObservableDictionary<int, string> _masterFunds;

        /// <summary>
        /// The _selected master funds
        /// </summary>
        private KeyValuePair<int, string> _selectedMasterFunds;

        /// <summary>
        /// The _third party details
        /// </summary>
        private ObservableDictionary<int, string> _thirdPartyDetails;

        /// <summary>
        /// The _selected third party details
        /// </summary>
        private KeyValuePair<int, string> _selectedThirdPartyDetails;

        /// <summary>
        /// The _select all
        /// </summary>
        private bool _selectAll;

        /// <summary>
        /// The _group level
        /// </summary>
        private bool _groupLevel = true;

        /// <summary>
        /// The _taxlot level
        /// </summary>
        private bool _taxlotLevel;

        /// <summary>
        /// The _list box data
        /// </summary>
        private ObservableCollection<string> _listBoxData;

        /// <summary>
        /// The _CHKBX select all
        /// </summary>
        private bool _chkbxSelectAll;

        /// <summary>
        ///  if show masterfund on TT is true 
        /// </summary>
        /// 
        private bool _isEnabledTradeAttributeMF = CachedDataManager.GetInstance.IsShowMasterFundonTT() ? false : true;

        /// <summary>
        /// The _trade attributes collection
        /// </summary>
        private ObservableCollection<string>[] _tradeAttributesCollection;

        /// <summary>
        /// The trade attributes keep records
        /// </summary>
        private CustomValueEnteredActions[] _tradeAttributesKeepRecords;

        /// <summary>
        /// The _master fund association
        /// </summary>
        private Dictionary<int, List<int>> _masterFundAssociation;

        /// <summary>
        /// The _account pb details
        /// </summary>
        private Dictionary<int, string> _accountPBDetails;

        /// <summary>
        /// The _selected account i ds list
        /// </summary>
        private ObservableCollection<object> _selectedAccountIDsList;

        /// <summary>
        /// The _check for whether the unallocated trade is selected
        /// </summary>
        public bool _taxlotEnabled;

        /// <summary>
        /// This variable stores the accountsIds of selected groups
        /// </summary>
        private List<int> _selectedAccountIDs = new List<int>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account pb details.
        /// </summary>
        /// <value>
        /// The account pb details.
        /// </value>
        public Dictionary<int, string> AccountPBDetails
        {
            get { return _accountPBDetails; }
            set { _accountPBDetails = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [CHKBX select all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CHKBX select all]; otherwise, <c>false</c>.
        /// </value>
        public bool ChkbxSelectAll
        {
            get { return _chkbxSelectAll; }
            set
            {
                _chkbxSelectAll = value;
                if (_chkbxSelectAll)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        if (i != 25 || IsEnabledTradeAttributeMF)
                        {
                            AllTradeAttributes[i].IsChecked = true;
                        }                     
                    }
                }
                else
                {
                    for (int i = 0; i < 45; i++)
                    {
                        AllTradeAttributes[i].IsChecked = false;
                    }
                }
                RaisePropertyChangedEvent("ChkbxSelectAll");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [group level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [group level]; otherwise, <c>false</c>.
        /// </value>
        public bool GroupLevel
        {
            get { return _groupLevel; }
            set
            {
                _groupLevel = value;
                RaisePropertyChangedEvent("GroupLevel");
            }
        }

        /// <summary>
        /// Gets or sets the ListBox data.
        /// </summary>
        /// <value>
        /// The ListBox data.
        /// </value>
        public ObservableCollection<string> ListBoxData
        {
            get { return _listBoxData; }
            set
            {
                _listBoxData = value;
                RaisePropertyChangedEvent("ListBoxData");
            }
        }

        /// <summary>
        /// Gets or sets the master fund association.
        /// </summary>
        /// <value>
        /// The master fund association.
        /// </value>
        public Dictionary<int, List<int>> MasterFundAssociation
        {
            get { return _masterFundAssociation; }
            set { _masterFundAssociation = value; }
        }

        /// <summary>
        /// Gets or sets the master funds.
        /// </summary>
        /// <value>
        /// The master funds.
        /// </value>
        public ObservableDictionary<int, string> MasterFunds
        {
            get { return _masterFunds; }
            set
            {
                _masterFunds = value;
                RaisePropertyChangedEvent("MasterFunds");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [select all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [select all]; otherwise, <c>false</c>.
        /// </value>
        public bool SelectAll
        {
            get { return _selectAll; }
            set
            {
                _selectAll = value;
                RaisePropertyChangedEvent("SelectAll");
            }
        }

        /// <summary>
        /// Gets or sets the selected account i ds list.
        /// </summary>
        /// <value>
        /// The selected account i ds list.
        /// </value>
        public ObservableCollection<object> SelectedAccountIDsList
        {
            get { return _selectedAccountIDsList; }
            set
            {
                _selectedAccountIDsList = value;
                RaisePropertyChangedEvent("SelectedAccountIDsList");
            }
        }

        /// <summary>
        /// Gets or sets the selected master funds.
        /// </summary>
        /// <value>
        /// The selected master funds.
        /// </value>
        public KeyValuePair<int, string> SelectedMasterFunds
        {
            get { return _selectedMasterFunds; }
            set
            {
                _selectedMasterFunds = value;
                SelectedThirdPartyDetails = new KeyValuePair<int, string>(Int32.MinValue, ApplicationConstants.C_COMBO_SELECT);
                UpdateAccountCombo();
                RaisePropertyChangedEvent("SelectedMasterFunds");
            }
        }

        /// <summary>
        /// Gets or sets the selected third party details.
        /// </summary>
        /// <value>
        /// The selected third party details.
        /// </value>
        public KeyValuePair<int, string> SelectedThirdPartyDetails
        {
            get { return _selectedThirdPartyDetails; }
            set
            {
                _selectedThirdPartyDetails = value;
                UpdateAccountCombo();
                RaisePropertyChangedEvent("SelectedThirdPartyDetails");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [Unallocated_trade is checked].
        /// </summary>
        public bool TaxlotEnabled
        {
            get
            {
                return _taxlotEnabled;
            }
            set
            {
                _taxlotEnabled = value;
                RaisePropertyChangedEvent("TaxlotEnabled");
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [taxlot level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [taxlot level]; otherwise, <c>false</c>.
        /// </value>
        public bool TaxlotLevel
        {
            get { return _taxlotLevel; }
            set
            {
                _taxlotLevel = value;
                if (value)
                    SetFilterTaxlotLevelData();
                SelectedMasterFunds = new KeyValuePair<int, string>(Int32.MinValue, ApplicationConstants.C_COMBO_SELECT);
                RaisePropertyChangedEvent("TaxlotLevel");
            }
        }

        /// <summary>
        /// Gets or sets the third party details.
        /// </summary>
        /// <value>
        /// The third party details.
        /// </value>
        public ObservableDictionary<int, string> ThirdPartyDetails
        {
            get { return _thirdPartyDetails; }
            set
            {
                _thirdPartyDetails = value;
                RaisePropertyChangedEvent("ThirdPartyDetails");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled trade attribute 6 combo for master fund workflow.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled trade attributes combo for master fund workflow; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledTradeAttributeMF
        {
            get { return _isEnabledTradeAttributeMF; }
            set
            {
                _isEnabledTradeAttributeMF = value;
                RaisePropertyChangedEvent("IsEnabledTradeAttributeMF");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes collection.
        /// </summary>
        /// <value>
        /// The trade attributes collection.
        /// </value>
        public ObservableCollection<string>[] TradeAttributesCollection
        {
            get { return _tradeAttributesCollection; }
            set
            {
                _tradeAttributesCollection = value;
                RaisePropertyChangedEvent("TradeAttributesCollection");
            }
        }

        /// <summary>
        /// Gets or sets the trade attributes keep records.
        /// </summary>
        /// <value>
        /// The trade attributes keep records.
        /// </value>
        public CustomValueEnteredActions[] TradeAttributesKeepRecords
        {
            get { return _tradeAttributesKeepRecords; }
            set
            {
                _tradeAttributesKeepRecords = value;
                RaisePropertyChangedEvent("TradeAttributesKeepRecords");
            }
        }

        /// <summary>
        /// The collection of all trade attributes
        /// </summary>
        public ObservableCollection<TradeAttributeViewModel> AllTradeAttributes { get; }
            = new ObservableCollection<TradeAttributeViewModel>();
        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the trade attribute bulk change control view loaded.
        /// </summary>
        /// <value>
        /// The trade attribute bulk change control view loaded.
        /// </value>
        public RelayCommand<object> TradeAttributeBulkChangeCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeAttributeBulkChangeControlViewModel"/> class.
        /// </summary>
        public TradeAttributeBulkChangeControlViewModel()
        {
            try
            {
                TradeAttributeBulkChangeCommand = new RelayCommand<object>((parameter) => TradeAttributeBulkChangeUpdate(parameter));
                // Grid follows rows major data, but we want to display in column major order. So storing data accordingly
                var reorderedLabels = new (string Label, int InputIndex)[45];
                for (int col = 0; col < 5; col++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        int inputIndex = col * 9 + row;   // column-major index
                        int gridIndex = row * 5 + col;    // row-major index
                        string label = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute + (inputIndex + 1));
                        reorderedLabels[gridIndex] = (label, inputIndex);
                    }
                }
                foreach (var item in reorderedLabels)
                {
                    AllTradeAttributes.Add(new TradeAttributeViewModel
                    {
                        Label = item.Label,
                        IsEnabled = item.InputIndex != 5 || (item.InputIndex == 5 && IsEnabledTradeAttributeMF),
                        AttributeName = AllocationUIConstants.TradeAttribute + (item.InputIndex + 1),
                        ElementAutomationName = "TradeAttr" + (item.InputIndex + 1),
                        CmbAutomationName = $"TradeAttr{item.InputIndex + 1}Combo"
                    });
                }
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
        /// Gets the account associated with mf.
        /// </summary>
        /// <param name="selectedMasterFundId">The selected master fund identifier.</param>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithMF(int selectedMasterFundId)
        {
            try
            {
                List<string> accounts = new List<string>();
                if (!selectedMasterFundId.Equals(int.MinValue))
                {
                    List<int> accountIDs = MasterFundAssociation[selectedMasterFundId];
                    if (accountIDs != null)
                    {
                        foreach (int accountID in accountIDs)
                        {
                            if (_selectedAccountIDs.Contains(accountID))
                            {
                                string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                                accounts.Add(accountName);
                            }                           
                        }
                    }
                }
                return new ObservableCollection<string>(accounts);
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
        /// Gets the account associated with mf and pb.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithMFAndPB(int selectedMasterFundId)
        {
            try
            {
                List<string> accounts = new List<string>();
                List<int> pbAccountIDs = GetThirdPartyName();

                if (!selectedMasterFundId.Equals(0) && selectedMasterFundId != int.MinValue)
                {
                    List<int> masterFundIDs = MasterFundAssociation[selectedMasterFundId];

                    if (pbAccountIDs != null)
                    {
                        foreach (int masterFundId in masterFundIDs)
                        {
                            foreach (int pbAccountID in pbAccountIDs)
                            {
                                if (masterFundId.Equals(pbAccountID) && _selectedAccountIDs.Contains(masterFundId))
                                {
                                    string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(masterFundId);
                                    accounts.Add(accountName);
                                }
                            }
                        }
                    }
                }
                return new ObservableCollection<string>(accounts);
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
        /// Gets the account associated with pb.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetAccountAssociatedWithPB()
        {
            try
            {
                List<string> accounts = new List<string>();
                List<int> pbAccountIDs = GetThirdPartyName();
                if (pbAccountIDs != null)
                {
                    foreach (int accountID in pbAccountIDs)
                    {
                        if (_selectedAccountIDs.Contains(accountID))
                        {
                            string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                            accounts.Add(accountName);
                        }
                    }
                }
                return new ObservableCollection<string>(accounts);
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
        /// Gets the name of the account identifier by pb.
        /// </summary>
        /// <param name="pbName">Name of the pb.</param>
        /// <returns></returns>
        public List<int> GetAccountIDByPBName(string pbName)
        {
            List<int> accountIDs = new List<int>();
            try
            {
                if (AccountPBDetails != null)
                {
                    foreach (KeyValuePair<int, string> kvp in AccountPBDetails)
                    {
                        if (string.Compare(kvp.Value, pbName, true) == 0)
                        {
                            accountIDs.Add(kvp.Key);
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
            return accountIDs;
        }

        /// <summary>
        /// Gets the selected data items.
        /// </summary>
        /// <returns></returns>
        private TradeAttributes GetSelectedDataItems()
        {
            TradeAttributes tradeGroups = new TradeAttributes();
            try
            {
                if (GroupLevel || TaxlotLevel)
                {
                    if (AllTradeAttributes[0].IsChecked)
                    {
                        tradeGroups.TradeAttribute1 = AllTradeAttributes[0].SelectedValue;
                    }
                    if (AllTradeAttributes[5].IsChecked)
                    {
                        tradeGroups.TradeAttribute2 = AllTradeAttributes[5].SelectedValue;
                    }
                    if (AllTradeAttributes[10].IsChecked)
                    {
                        tradeGroups.TradeAttribute3 = AllTradeAttributes[10].SelectedValue;
                    }
                    if (AllTradeAttributes[15].IsChecked)
                    {
                        tradeGroups.TradeAttribute4 = AllTradeAttributes[15].SelectedValue;
                    }
                    if (AllTradeAttributes[20].IsChecked)
                    {
                        tradeGroups.TradeAttribute5 = AllTradeAttributes[20].SelectedValue;
                    }
                    if (AllTradeAttributes[25].IsChecked)
                    {
                        tradeGroups.TradeAttribute6 = AllTradeAttributes[25].SelectedValue;
                    }

                    var list = AllTradeAttributes.Where(kvp => kvp.IsChecked)
                                   .Select(kvp => new { Name = kvp.AttributeName, Value = kvp.SelectedValue })
                                   .ToList();
                    tradeGroups.SetTradeAttribute(JsonConvert.SerializeObject(list, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tradeGroups;
        }

        /// <summary>
        /// Gets the name of the third party.
        /// </summary>
        /// <returns></returns>
        private List<int> GetThirdPartyName()
        {
            List<int> accountIDs = null;
            try
            {
                if (SelectedThirdPartyDetails.Value != null)
                {
                    string thirdPartyName = SelectedThirdPartyDetails.Value;
                    if (!thirdPartyName.Equals(ApplicationConstants.C_COMBO_SELECT) && !thirdPartyName.Equals(string.Empty))
                    {
                        accountIDs = GetAccountIDByPBName(thirdPartyName);
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
            return accountIDs;
        }

        /// <summary>
        /// Called when [load trade attribute bulk change control].
        /// </summary>
        /// <param name="tradeAttributes">The trade attributes.</param>
        /// <param name="accountPBDetails">The account pb details.</param>
        internal void OnLoadTradeAttributeBulkChangeControl(ObservableCollection<string>[] tradeAttributes, Dictionary<int, string> accountPBDetails, CustomValueEnteredActions[] custValueforTradeAttributes)
        {
            try
            {
                TradeAttributesCollection = tradeAttributes;
                TradeAttributesKeepRecords = custValueforTradeAttributes;
                SetTradeAttributesNames();

                MasterFundAssociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                AccountPBDetails = accountPBDetails;
                GroupLevel = true;
                Dictionary<int, string> thirdPartyDict = new Dictionary<int, string>();
                thirdPartyDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

                Dictionary<int, string> masterFundsDict = new Dictionary<int, string>();
                masterFundsDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

                MasterFunds = new ObservableDictionary<int, string>(masterFundsDict);
                ThirdPartyDetails = new ObservableDictionary<int, string>(thirdPartyDict);

                foreach (TradeAttributeViewModel tradeAttribute in AllTradeAttributes)
                {
                    int index = Convert.ToInt32(tradeAttribute.AttributeName.Replace(AllocationUIConstants.TradeAttribute, ""));
                    tradeAttribute.Values = TradeAttributesCollection[index - 1];
                    tradeAttribute.CustomValueEnteredAction = TradeAttributesKeepRecords[index - 1];
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
        /// Sets the account combo.
        /// </summary>
        private void UpdateAccountCombo()
        {
            try
            {
                int selectedMasterFundId = SelectedMasterFunds.Key;
                int selectedThirdPartyId = SelectedThirdPartyDetails.Key;

                if (selectedMasterFundId != int.MinValue && selectedThirdPartyId != int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithMFAndPB(selectedMasterFundId));

                else if (selectedMasterFundId != int.MinValue && selectedThirdPartyId == int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithMF(selectedMasterFundId));

                else if (selectedMasterFundId == int.MinValue && selectedThirdPartyId != int.MinValue)
                    ListBoxData = new ObservableCollection<string>(GetAccountAssociatedWithPB());
                else
                    SetAccountCombo();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        
        /// <summary>
        /// Sets the preferences.
        /// </summary>
        internal void SetPreferences()
        {
            try
            {
                SetTradeAttributesNames();
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the trade attributes names.
        /// </summary>
        private void SetTradeAttributesNames()
        {
            try
            {
                foreach (TradeAttributeViewModel tradeAttribute in AllTradeAttributes)
                {
                    int index = Convert.ToInt32(tradeAttribute.AttributeName.Replace(AllocationUIConstants.TradeAttribute, ""));
                    tradeAttribute.Label = CachedDataManager.GetInstance.GetAttributeNameForValue(AllocationUIConstants.CAPTION_TradeAttribute + index);
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
        /// Trades the attribute bulk change update.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object TradeAttributeBulkChangeUpdate(object parameter)
        {
            try
            {
                TradeAttributes tradeChangeGroups = GetSelectedDataItems();
                List<object> accountIDs = new List<object>();
                if (TaxlotLevel && SelectedAccountIDsList != null)
                    accountIDs = SelectedAccountIDsList.ToList();
                else
                    accountIDs = null;

                if (UpdateGroupsLevel != null)
                    UpdateGroupsLevel(this, new EventArgs<TradeAttributes, List<object>>(tradeChangeGroups, accountIDs));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// This method sets combo boxes in Filter taxlot level section
        /// </summary>
        private void SetFilterTaxlotLevelData()
        {
            try
            {
                _selectedAccountIDs.Clear();
                List<AllocationGroup> groups = OnGetAllocationGroups?.Invoke();
                foreach (AllocationGroup ag in groups)
                {
                    _selectedAccountIDs.AddRange(ag.TaxLots.Select(taxlot => taxlot.Level1ID).Distinct());
                }
                
                SetMasterFundCombo();
                SetAccountCombo();
                SetThirdPartyCombo();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This method sets the masterfund combo box according to selected groups
        /// </summary>
        private void SetMasterFundCombo()
        {
            try
            {
                MasterFunds?.Clear();

                var masterFundsDict = new Dictionary<int, string>
                {
                    { int.MinValue, ApplicationConstants.C_COMBO_SELECT }
                };

                Dictionary<int, string> allMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
                var availableMasterFunds = _selectedAccountIDs
                    .Select(id => CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(id))
                    .Distinct();

                Dictionary<int, string> filteredFunds = allMasterFunds
                    .Where(kvp => availableMasterFunds.Contains(kvp.Key))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                masterFundsDict.AddRangeThreadSafely(filteredFunds);

                MasterFunds = new ObservableDictionary<int, string>(masterFundsDict);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This method sets the account combo box according to selected groups
        /// </summary>
        private void SetAccountCombo()
        {
            try
            {
                HashSet<string> accounts = new HashSet<string>();

                foreach (int accountID in _selectedAccountIDs)
                {
                    string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                    accounts.Add(accountName);
                }
                ListBoxData = new ObservableCollection<string>(accounts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method sets the Prime broker combo box according to selected groups
        /// </summary>
        private void SetThirdPartyCombo()
        {
            try
            {
                Dictionary<int, string> thirdPartyDict = new Dictionary<int, string>();
                Dictionary<int, string> allThirdParties = CachedDataManager.GetInstance.GetAllThirdParties();
                Dictionary<int, string> filteredThirdParties = new Dictionary<int, string>();
                foreach (int accountID in _selectedAccountIDs)
                {
                    filteredThirdParties = AccountPBDetails.Where(kvp => _selectedAccountIDs.Contains(kvp.Key))
                        .GroupBy(kvp => kvp.Value)
                        .Select(g => g.First())
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                thirdPartyDict.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                thirdPartyDict.AddRangeThreadSafely(filteredThirdParties);
                ThirdPartyDetails = new ObservableDictionary<int, string>(thirdPartyDict);
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
