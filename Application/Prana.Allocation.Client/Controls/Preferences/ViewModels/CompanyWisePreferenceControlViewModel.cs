using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class CompanyWisePreferenceControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _asset class collection
        /// </summary>
        private ObservableDictionary<int, string> _assetClassCollection;

        /// <summary>
        /// The _asset class collection
        /// </summary>
        private ObservableDictionary<int, string> _accountsCollection;

        /// <summary>
        /// The _asset class collection
        /// </summary>
        private ObservableDictionary<int, string> _counterPartiesCollection;

        /// <summary>
        /// The _prorata scheme name collection
        /// </summary>
        private ObservableCollection<string> _prorataSchemeNameCollection;

        /// <summary>
        /// The _prorata scheme basis collection
        /// </summary>
        private ObservableCollection<string> _prorataSchemeBasisCollection;

        /// <summary>
        /// The _selected asset class check side
        /// </summary>
        private ObservableCollection<object> _selectedAssetClassCheckSide;

        /// <summary>
        /// The _selected accounts check side
        /// </summary>
        private ObservableCollection<object> _selectedAccountsCheckSide;

        /// <summary>
        /// The _selected counter parties check side
        /// </summary>
        private ObservableCollection<object> _selectedCounterPartiesCheckSide;

        /// <summary>
        /// The _selected asset class commission
        /// </summary>
        private ObservableCollection<object> _selectedAssetClassCommission;

        /// <summary>
        /// The _selected prorata scheme name
        /// </summary>
        private string _selectedProrataSchemeName;

        /// <summary>
        /// The _selected prorata scheme basis
        /// </summary>
        private string _selectedProrataSchemeBasis;

        /// <summary>
        /// The _is allow edit preference checked
        /// </summary>
        private bool _isAllowEditPrefChecked;

        /// <summary>
        /// The _is control enabled
        /// </summary>
        private bool _isControlEnabled;

        /// <summary>
        /// The _is do check side checked
        /// </summary>
        private bool _isDoCheckSideChecked;

        /// <summary>
        /// The _precision value
        /// </summary>
        private int _precisionValue;

        /// <summary>
        /// The _MSG on broker change
        /// </summary>
        private bool _msgOnBrokerChange;

        /// <summary>
        /// The _MSG on allocation
        /// </summary>
        private bool _msgOnAllocation;

        /// <summary>
        /// The enable master fund ratio checked
        /// </summary>
        private bool _isMasterFundAllocation;

        /// <summary>
        /// The 1master fund 1symbol enabled
        /// </summary>
        private bool _oneMasterFund_1Symbol;

        /// <summary>
        /// The _set scheme generation attributes prorata
        /// </summary>
        private bool _setSchemeGenerationAttributesProrata;

        /// <summary>
        /// The _user choice collection
        /// </summary>
        private ObservableDictionary<bool, string> _userChoiceCollection;

        /// <summary>
        /// The _selected broker change choice
        /// </summary>
        private KeyValuePair<bool, string> _selectedBrokerChangeChoice;

        /// <summary>
        /// The _selected allocation choice
        /// </summary>
        private KeyValuePair<bool, string> _selectedAllocationChoice;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the asset class collection.
        /// </summary>
        /// <value>
        /// The asset class collection.
        /// </value>
        public ObservableDictionary<int, string> AssetClassCollection
        {
            get { return _assetClassCollection; }
            set
            {
                _assetClassCollection = value;
                RaisePropertyChangedEvent("AssetClassCollection");
            }
        }

        /// <summary>
        /// Gets or sets the Accounts Collection.
        /// </summary>
        /// <value>
        /// The asset class collection.
        /// </value>
        public ObservableDictionary<int, string> AccountsCollection
        {
            get { return _accountsCollection; }
            set
            {
                _accountsCollection = value;
                RaisePropertyChangedEvent("AccountsCollection");
            }
        }

        /// <summary>
        /// Gets or sets the Counter Party Collection.
        /// </summary>
        /// <value>
        /// The asset class collection.
        /// </value>
        public ObservableDictionary<int, string> CounterPartyCollection
        {
            get { return _counterPartiesCollection; }
            set
            {
                _counterPartiesCollection = value;
                RaisePropertyChangedEvent("CounterPartyCollection");
            }
        }

        /// <summary>
        /// Gets or sets the prorata scheme name collection.
        /// </summary>
        /// <value>
        /// The prorata scheme name collection.
        /// </value>
        public ObservableCollection<string> ProrataSchemeNameCollection
        {
            get { return _prorataSchemeNameCollection; }
            set
            {
                _prorataSchemeNameCollection = value;
                RaisePropertyChangedEvent("ProrataSchemeNameCollection");
            }
        }

        /// <summary>
        /// Gets or sets the prorata scheme basis collection.
        /// </summary>
        /// <value>
        /// The prorata scheme basis collection.
        /// </value>
        public ObservableCollection<string> ProrataSchemeBasisCollection
        {
            get { return _prorataSchemeBasisCollection; }
            set
            {
                _prorataSchemeBasisCollection = value;
                RaisePropertyChangedEvent("ProrataSchemeBasisCollection");
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is allow edit preference checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allow edit preference checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowEditPrefChecked
        {
            get { return _isAllowEditPrefChecked; }
            set
            {
                _isAllowEditPrefChecked = value;
                RaisePropertyChangedEvent("IsAllowEditPrefChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is control enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is control enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                RaisePropertyChangedEvent("IsControlEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is do check side checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is do check side checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsDoCheckSideChecked
        {
            get { return _isDoCheckSideChecked; }
            set
            {
                _isDoCheckSideChecked = value;
                if (_isDoCheckSideChecked == true)
                    IsControlEnabled = true;
                else
                    IsControlEnabled = false;
                RaisePropertyChangedEvent("IsDoCheckSideChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is1 master fund1symbol enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is1 master fund1symbol enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool OneMasterFund_1Symbol
        {
            get { return _oneMasterFund_1Symbol; }
            set
            {
                _oneMasterFund_1Symbol = value;
                RaisePropertyChangedEvent("OneMasterFund_1Symbol");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [set scheme generation attributes prorata].
        /// </summary>
        /// <value>
        /// <c>true</c> if [set scheme generation attributes prorata]; otherwise, <c>false</c>.
        /// </value>
        public bool SetSchemeGenerationAttributesProrata
        {
            get { return _setSchemeGenerationAttributesProrata; }
            set
            {
                _setSchemeGenerationAttributesProrata = value;
                RaisePropertyChangedEvent("SetSchemeGenerationAttributesProrata");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enable master fund ratio checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enable master fund ratio checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterFundAllocation
        {
            get { return _isMasterFundAllocation; }
            set
            {
                _isMasterFundAllocation = value;
                RaisePropertyChangedEvent("IsMasterFundAllocation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [MSG on allocation].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [MSG on allocation]; otherwise, <c>false</c>.
        /// </value>
        public bool MsgOnAllocation
        {
            get { return _msgOnAllocation; }
            set
            {
                _msgOnAllocation = value;
                RaisePropertyChangedEvent("MsgOnAllocation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [MSG on broker change].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [MSG on broker change]; otherwise, <c>false</c>.
        /// </value>
        public bool MsgOnBrokerChange
        {
            get { return _msgOnBrokerChange; }
            set
            {
                _msgOnBrokerChange = value;
                RaisePropertyChangedEvent("MsgOnBrokerChange");
            }
        }

        /// <summary>
        /// Gets or sets the precision value.
        /// </summary>
        /// <value>
        /// The precision value.
        /// </value>
        public int PrecisionValue
        {
            get { return _precisionValue; }
            set
            {
                _precisionValue = value;
                RaisePropertyChangedEvent("PrecisionValue");
            }
        }

        /// <summary>
        /// Allocation Check Side Preference.
        /// </summary>
        public AllocationCheckSidePref AllocationCheckSidePreference { get; set; }

        /// <summary>
        /// Gets or sets the selected allocation choice.
        /// </summary>
        /// <value>
        /// The selected allocation choice.
        /// </value>
        public KeyValuePair<bool, string> SelectedAllocationChoice
        {
            get { return _selectedAllocationChoice; }
            set
            {
                _selectedAllocationChoice = value;
                RaisePropertyChangedEvent("SelectedAllocationChoice");
            }
        }

        /// <summary>
        /// Gets or sets the selected asset class check side.
        /// </summary>
        /// <value>
        /// The selected asset class check side.
        /// </value>
        public ObservableCollection<object> SelectedAssetClassCheckSide
        {
            get { return _selectedAssetClassCheckSide; }
            set
            {
                _selectedAssetClassCheckSide = value;
                RaisePropertyChangedEvent("SelectedAssetClassCheckSide");
            }
        }


        /// <summary>
        /// Gets or sets the selected Accounts check side.
        /// </summary>
        /// <value>
        /// The selected asset class check side.
        /// </value>
        public ObservableCollection<object> SelectedAccountsCheckSide
        {
            get { return _selectedAccountsCheckSide; }
            set
            {
                _selectedAccountsCheckSide = value;
                RaisePropertyChangedEvent("SelectedAccountsCheckSide");
            }
        }

        /// <summary>
        /// Gets or sets the selected CounterParties check side.
        /// </summary>
        /// <value>
        /// The selected asset class check side.
        /// </value>
        public ObservableCollection<object> SelectedCounterPartiesCheckSide
        {
            get { return _selectedCounterPartiesCheckSide; }
            set
            {
                _selectedCounterPartiesCheckSide = value;
                RaisePropertyChangedEvent("SelectedCounterPartiesCheckSide");
            }
        }

        /// <summary>
        /// Gets or sets the selected asset class commission.
        /// </summary>
        /// <value>
        /// The selected asset class commission.
        /// </value>
        public ObservableCollection<object> SelectedAssetClassCommission
        {
            get { return _selectedAssetClassCommission; }
            set
            {
                _selectedAssetClassCommission = value;
                RaisePropertyChangedEvent("SelectedAssetClassCommission");
            }
        }

        /// <summary>
        /// Gets or sets the name of the selected prorata scheme.
        /// </summary>
        /// <value>
        /// The name of the selected prorata scheme.
        /// </value>
        public string SelectedProrataSchemeName
        {
            get { return _selectedProrataSchemeName; }
            set
            {
                _selectedProrataSchemeName = value;
                RaisePropertyChangedEvent("SelectedProrataSchemeName");
            }

        }

        /// <summary>
        /// Gets or sets the selected prorata scheme basis.
        /// </summary>
        /// <value>
        /// The selected prorata scheme basis.
        /// </value>
        public string SelectedProrataSchemeBasis
        {
            get { return _selectedProrataSchemeBasis; }
            set
            {
                _selectedProrataSchemeBasis = value;
                RaisePropertyChangedEvent("SelectedProrataSchemeBasis");
            }
        }

        /// <summary>
        /// Gets or sets the selected broker change choice.
        /// </summary>
        /// <value>
        /// The selected broker change choice.
        /// </value>
        public KeyValuePair<bool, string> SelectedBrokerChangeChoice
        {
            get { return _selectedBrokerChangeChoice; }
            set
            {
                _selectedBrokerChangeChoice = value;
                RaisePropertyChangedEvent("SelectedBrokerChangeChoice");
            }
        }

        /// <summary>
        /// Gets or sets the user choice collection.
        /// </summary>
        /// <value>
        /// The user choice collection.
        /// </value>
        public ObservableDictionary<bool, string> UserChoiceCollection
        {
            get { return _userChoiceCollection; }
            set
            {
                _userChoiceCollection = value;
                RaisePropertyChangedEvent("UserChoiceCollection");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyWisePreferenceControlViewModel"/> class.
        /// </summary>
        public CompanyWisePreferenceControlViewModel()
        {
            try
            {
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
        /// Gets the company wise preferences.
        /// </summary>
        /// <returns></returns>
        internal AllocationCompanyWisePref GetCompanyWisePreferences()
        {
            AllocationCompanyWisePref companyWisePreference = new AllocationCompanyWisePref();
            try
            {



                AllocationCheckSidePref checkSidePref = new AllocationCheckSidePref();
                checkSidePref.DoCheckSideSystem = IsDoCheckSideChecked;
                checkSidePref.DisableCheckSidePref = new Dictionary<OrderFilterLevels, List<int>>();

                if (IsDoCheckSideChecked)
                {
                    List<int> disabledAssets = SelectedAssetClassCheckSide.Select(x => ((KeyValuePair<int, string>)x).Key).ToList();
                    List<int> disabledAccounts = SelectedAccountsCheckSide.Select(x => ((KeyValuePair<int, string>)x).Key).ToList();
                    List<int> disabledCounterParties = SelectedCounterPartiesCheckSide.Select(x => ((KeyValuePair<int, string>)x).Key).ToList();
                    checkSidePref.DisableCheckSidePref.Add(OrderFilterLevels.Account, disabledAccounts);
                    checkSidePref.DisableCheckSidePref.Add(OrderFilterLevels.Asset, disabledAssets);
                    checkSidePref.DisableCheckSidePref.Add(OrderFilterLevels.CounterParty, disabledCounterParties);
                }





                //List<int> disabledAssets = new List<int>();
                //disabledAssets =  SelectedAssetClassCheckSide.Select(x => ((KeyValuePair<int, string>)x).Key).ToList();
                //foreach (object obj in SelectedAssetClassCheckSide)
                //{
                //    KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)obj;
                //    disabledAssets.Add(kvp.Key);
                //}

                //List<int> disabledAccounts = new List<int>();
                //foreach (object obj in SelectedAccountsCheckSide)
                //{
                //    KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)obj;
                //    disabledAccounts.Add(kvp.Key);
                //}

                //List<int> disabledCounterParties = new List<int>();
                //foreach (object obj in SelectedCounterPartiesCheckSide)
                //{
                //    KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)obj;
                //    disabledCounterParties.Add(kvp.Key);
                //}


                List<int> assetsWithCommissionInNetAmount = new List<int>();
                foreach (object obj in SelectedAssetClassCommission)
                {
                    KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)obj;
                    assetsWithCommissionInNetAmount.Add(kvp.Key);
                }
                int companyId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                bool recalculateOnBrokerChange = (SelectedBrokerChangeChoice.Value != null) ? SelectedBrokerChangeChoice.Key : false;
                bool recalculateOnAllocation = (SelectedAllocationChoice.Value != null) ? SelectedAllocationChoice.Key : false;
                companyWisePreference = new AllocationCompanyWisePref()
                {
                    CompanyId = companyId,
                    DefaultRule = new AllocationRule(),
                    //DoCheckSide = IsDoCheckSideChecked,
                    AllowEditPreferences = IsAllowEditPrefChecked,
                    //DisableCheckSideForAssets = disabledAssets,
                    AllocationCheckSidePref = checkSidePref,
                    PrecisionDigit = PrecisionValue,
                    AssetsWithCommissionInNetAmount = assetsWithCommissionInNetAmount,
                    MsgOnBrokerChange = MsgOnBrokerChange,
                    RecalculateOnBrokerChange = recalculateOnBrokerChange,
                    MsgOnAllocation = MsgOnAllocation,
                    RecalculateOnAllocation = recalculateOnAllocation,
                    EnableMasterFundAllocation = IsMasterFundAllocation,
                    IsOneSymbolOneMasterFundAllocation = OneMasterFund_1Symbol,
                    SelectedProrataSchemeName = SelectedProrataSchemeName,
                    SelectedProrataSchemeBasis = SelectedProrataSchemeBasis,
                    SetSchemeGenerationAttributesProrata = SetSchemeGenerationAttributesProrata
                };



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
        /// Sets the default rules.
        /// </summary>
        /// <param name="defaultRule">The default rules.</param>
        internal void SetCompanyWisePreference(AllocationCompanyWisePref defaultRules)
        {
            try
            {
                var assets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets().OrderBy(x => x.Value);
                var accounts = CommonDataCache.CachedDataManager.GetInstance.GetAccounts().OrderBy(x => x.Value);
                var counterParty = CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties().OrderBy(x => x.Value);
                AssetClassCollection = new ObservableDictionary<int, string>(assets);
                AccountsCollection = new ObservableDictionary<int, string>(accounts);
                CounterPartyCollection = new ObservableDictionary<int, string>(counterParty);

                ProrataSchemeNameCollection = new ObservableCollection<string>(AllocationClientPreferenceManager.GetInstance.GetProrataFixedPreferenceNames());
                ProrataSchemeBasisCollection = new ObservableCollection<string>(CommonAllocationMethods.GetAllocationSchemeKeys());
                UserChoiceCollection = new ObservableDictionary<bool, string>(CommonAllocationMethods.GetUserChoiceCollection());
                if (defaultRules != null)
                {
                    AllocationCheckSidePreference = defaultRules.AllocationCheckSidePref;

                    PrecisionValue = defaultRules.PrecisionDigit;
                    IsAllowEditPrefChecked = defaultRules.AllowEditPreferences;
                    IsMasterFundAllocation = defaultRules.EnableMasterFundAllocation;
                    OneMasterFund_1Symbol = defaultRules.IsOneSymbolOneMasterFundAllocation;

                    if (defaultRules.AllocationCheckSidePref != null)
                    {
                        // set force allocation button visibility
                        IsDoCheckSideChecked = defaultRules.AllocationCheckSidePref.DoCheckSideSystem;

                        //// set force allocation button checked or unchecked by default
                        //if (defaultRules.AllocationCheckSidePref.DoCheckSideSystem)
                        //{

                        // If all Accounts or all assets classes or all counter party selected then it will never check for sides. IsForceAllocationSelected = true;
                        var disableCheckSidePref = defaultRules.AllocationCheckSidePref.DisableCheckSidePref;

                        if (disableCheckSidePref.ContainsKey(OrderFilterLevels.Account))
                            SelectedAccountsCheckSide = CommonAllocationMethods.GetCollection(disableCheckSidePref[OrderFilterLevels.Account], CommonDataCache.CachedDataManager.GetInstance.GetAccounts());
                        else
                            SelectedAccountsCheckSide = new ObservableCollection<object>();

                        if (disableCheckSidePref.ContainsKey(OrderFilterLevels.Asset))
                            SelectedAssetClassCheckSide = CommonAllocationMethods.GetCollection(disableCheckSidePref[OrderFilterLevels.Asset], CommonDataCache.CachedDataManager.GetInstance.GetAllAssets());
                        else
                            SelectedAssetClassCheckSide = new ObservableCollection<object>();


                        if (disableCheckSidePref.ContainsKey(OrderFilterLevels.CounterParty))
                            SelectedCounterPartiesCheckSide = CommonAllocationMethods.GetCollection(disableCheckSidePref[OrderFilterLevels.CounterParty], CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties());
                        else
                            SelectedCounterPartiesCheckSide = new ObservableCollection<object>();

                        //}

                    }
                    //if (defaultRules.DisableCheckSideForAssets != null)
                    //    SelectedAssetClassCheckSide = CommonAllocationMethods.GetCollection(defaultRules.DisableCheckSideForAssets, CommonDataCache.CachedData.GetInstance().Asset);
                    //else
                    //    SelectedAssetClassCheckSide = new ObservableCollection<object>();

                    if (defaultRules.AssetsWithCommissionInNetAmount != null)
                        SelectedAssetClassCommission = CommonAllocationMethods.GetCollection(defaultRules.AssetsWithCommissionInNetAmount, CommonDataCache.CachedDataManager.GetInstance.GetAllAssets());
                    else
                        SelectedAssetClassCommission = new ObservableCollection<object>();

                    MsgOnBrokerChange = defaultRules.MsgOnBrokerChange;
                    SelectedBrokerChangeChoice = CommonAllocationMethods.GetCommissionPreference(defaultRules.RecalculateOnBrokerChange, CommonAllocationMethods.GetUserChoiceCollection());
                    MsgOnAllocation = defaultRules.MsgOnAllocation;
                    SelectedAllocationChoice = CommonAllocationMethods.GetCommissionPreference(defaultRules.RecalculateOnAllocation, CommonAllocationMethods.GetUserChoiceCollection());
                    if (ProrataSchemeNameCollection != null && !ProrataSchemeNameCollection.Contains(defaultRules.SelectedProrataSchemeName))
                    {
                        ProrataSchemeNameCollection.Add(defaultRules.SelectedProrataSchemeName);
                    }
                    SelectedProrataSchemeName = defaultRules.SelectedProrataSchemeName;
                    SelectedProrataSchemeBasis = defaultRules.SelectedProrataSchemeBasis;
                    SetSchemeGenerationAttributesProrata = defaultRules.SetSchemeGenerationAttributesProrata;
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

        #endregion Methods
    }
}
