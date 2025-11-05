using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.CommonDataCache;
using System.Collections.Generic;

namespace Prana.ClientPreferences
{
    public class TradingPreference
    {
        ConfirmationPopUp _confirmationPopUp = null;
        PriceSymbolValidation _riskAndValidateSettings = null;
        CompanyUser _user = null;
        CounterPartyWiseCommissionBasis _cpwiseCommissionBasis = null;
        TTGeneralPrefs _ttGeneralPrefs = null;
        QuickTTPrefs _quickTTPrefs = null;
        QTTFieldPreference[] _qTTFieldPreference = null;
        private TradingTicketUIPrefs _userTradingTicketUiPrefs = null;

        public TradingTicketUIPrefs UserTradingTicketUiPrefs
        {
            get
            {
                return _userTradingTicketUiPrefs ?? (_userTradingTicketUiPrefs = GetTTUserUIPreferences(_user.CompanyUserID));
            }
            set
            {
                _userTradingTicketUiPrefs = value;
            }
        }

        private TradingTicketUIPrefs _companyTradingTicketUiPrefs = null;
        public TradingTicketUIPrefs CompanyTradingTicketUiPrefs
        {
            get
            {
                return _companyTradingTicketUiPrefs ?? (_companyTradingTicketUiPrefs = GetTTCompanyUIPreferences(_user.CompanyID));
            }
            set
            {
                _companyTradingTicketUiPrefs = value;
            }
        }

        /// <summary>
        /// The trading ticket rules prefs
        /// </summary>
        private TradingTicketRulesPrefs _tradingTicketRulesPrefs = null;
        /// <summary> 
        /// Gets or sets the trading rules preference
        /// </summary>
        public TradingTicketRulesPrefs TradingTicketRulesPrefs
        {
            get
            {
                return _tradingTicketRulesPrefs ?? (_tradingTicketRulesPrefs = GetTradingRulesPreferences(_user.CompanyID));
            }
            set
            {
                _tradingTicketRulesPrefs = value;
            }
        }

        /// <summary>
        /// Transfer trade rules
        /// </summary>
        TranferTradeRules transfertraderules = CachedDataManager.GetInstance.GetTransferTradeRules();

        /// <summary>
        /// The securities list
        /// </summary>
        private List<string> _restrictedAllowedSecuritiesList = null;
        /// <summary> 
        /// Gets or sets the Restricted or Allowed Securities List
        /// </summary>
        public List<string> RestrictedAllowedSecuritiesList
        {
            get
            {
                return _restrictedAllowedSecuritiesList;
            }
            set
            {
                _restrictedAllowedSecuritiesList = value;
            }
        }

        /// <summary>
        /// The IsTickerSymbologySecuritiesList
        /// </summary>
        private bool _isTickerSymbologySecuritiesList;
        /// <summary> 
        /// Gets or sets IsTickerSymbologySecuritiesList
        /// </summary>
        public bool IsTickerSymbologySecuritiesList
        {
            get
            {
                return _isTickerSymbologySecuritiesList;
            }
            set
            {
                _isTickerSymbologySecuritiesList = value;
            }
        }

        /// <summary>
        /// Gets the Security List symbology
        /// </summary>
        private bool GetRestrictedAllowedSymbology(int companyID)
        {
            if (transfertraderules != null && transfertraderules.IsAllowRestrictedSecuritiesList)
                return PrefsCommonDataManager.GetRestrictedAllowedTickerSymbology(companyID, "Restricted");
            else
                return PrefsCommonDataManager.GetRestrictedAllowedTickerSymbology(companyID, "Allowed");
        }

        /// <summary>
        /// Gets the Security List based on restricted or allowed type
        /// </summary>
        private List<string> GetRestrictedAllowedSecuritiesList(int companyID)
        {
            if (transfertraderules != null && transfertraderules.IsAllowRestrictedSecuritiesList)
                return PrefsCommonDataManager.GetRestrictedAllowedSecuritiesList(companyID, "Restricted");
            else
                return PrefsCommonDataManager.GetRestrictedAllowedSecuritiesList(companyID, "Allowed");
        }

        public CounterPartyWiseCommissionBasis CpwiseCommissionBasis
        {
            get
            {
                _cpwiseCommissionBasis = _cpwiseCommissionBasis ?? (_cpwiseCommissionBasis = GetCommissionBasisPreferences());

                SetCommisionDefaultPreferences();
                return _cpwiseCommissionBasis;
            }

            set
            {
                _cpwiseCommissionBasis = value;
            }
        }

        private void SetCommisionDefaultPreferences()
        {
            //ToDO: Need to remove the block of code Desc: Save the default venue preferences if exist User wise, save as a commision pref with counterpartwise.
            if (UserTradingTicketUiPrefs.Venue.HasValue && UserTradingTicketUiPrefs.Broker.HasValue && _cpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.Count == 0)
            {
                Dictionary<int, string> allCounterParties = CachedDataManager.GetInstance.GetAllCounterParties();
                foreach (KeyValuePair<int, string> item in allCounterParties)
                {
                    VenueCollection permittedAlgo = TradingTicketPreferenceDataManager.GetVenues(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, item.Key, TradingTicketPreferenceType.User);
                    foreach (Venue venueItem in permittedAlgo)
                    {
                        if (venueItem.VenueID.Equals(UserTradingTicketUiPrefs.Venue.Value))
                        {
                            if (!_cpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(item.Key))
                                _cpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.Add(item.Key, venueItem.VenueID);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the quick tt prefs.
        /// </summary>
        /// <value>
        /// The quick tt prefs.
        /// </value>
        public QuickTTPrefs QuickTTPrefs
        {
            get
            {
                if (_quickTTPrefs == null)
                {
                    _quickTTPrefs = GetQuickTTPreferences();
                }
                return _quickTTPrefs;
            }
            set
            {
                _quickTTPrefs = value;
                SaveQuickTTPrefs();
            }
        }

        public QTTFieldPreference[] QTTFieldPreference
        {
            get
            {
                if (_qTTFieldPreference == null)
                {
                    _qTTFieldPreference = GetQTTFieldPreferences();
                }
                return _qTTFieldPreference;
            }
            set
            {
                _qTTFieldPreference = value;
            }
        }

        public TTGeneralPrefs TTGeneralPrefs
        {
            get
            {
                if (_ttGeneralPrefs == null)
                {
                    _ttGeneralPrefs = GetTTGeneralPreferences();
                }
                return _ttGeneralPrefs;
            }
            set
            {
                _ttGeneralPrefs = value;
            }
        }

        public void SetClientCache(CompanyUser user)
        {
            if (_user == null || user.CompanyUserID != _user.CompanyUserID)
            {
                _user = user;
                _confirmationPopUp = PrefsCommonDataManager.GetConfirmationPopUpPreferences(user.CompanyUserID);
                _riskAndValidateSettings = PrefsCommonDataManager.GetRiskValidationPreferences(user.CompanyUserID);
                IsTTPrefInitialized = true;
            }
            _user = user;
            _restrictedAllowedSecuritiesList = GetRestrictedAllowedSecuritiesList(user.CompanyID);
            _isTickerSymbologySecuritiesList = GetRestrictedAllowedSymbology(user.CompanyID);
        }
        /// <summary>
        /// to get basket complaince permission from the DB
        /// </summary>
        private bool? _isBasketComplianceEnabled = null;
        public bool? IsBasketComplianceEnabled
        {
            get
            {
                return _isBasketComplianceEnabled ?? (_isBasketComplianceEnabled = ComplianceCacheManager.GetIsBasketComplianceEnabledCompany());
            }
        }
        public PreferencesUniversalSettingsCollection GetPreferences()
        {
            return PrefsCommonDataManager.GetPreferences(_user.CompanyUserID);
        }

        /// <summary>
        /// to get Dollar Amount Permission for TT from the DB
        /// </summary>
        private bool? _dollarAmountPermission = null;
        public bool? DollarAmountPermission
        {
            get
            {
                return _dollarAmountPermission ?? (_dollarAmountPermission = GetDollarAmountPermission());
            }
        }
        /// <summary>
        /// to get Dollar Amount Permission for PTT from the DB
        /// </summary>
        private bool? _dollarAmountPTTPermission = null;
        public bool? DollarAmountPTTPermission
        {
            get
            {
                return _dollarAmountPTTPermission ?? (_dollarAmountPTTPermission = GetDollarAmountPTTPermission());
            }
        }
        /// <summary>
        /// to get Dollar Amount Permission for PTT from the DB
        /// </summary>
        private bool? _pstCompanyModuleForUserPermission = null;
        public bool? PSTCompanyModuleForUserPermission
        {
            get
            {
                return _pstCompanyModuleForUserPermission ?? (_pstCompanyModuleForUserPermission = GetPSTCompanyModuleForUserPermission(_user.CompanyUserID));
            }
        }
        public bool GetDollarAmountPermission()
        {
            return PrefsCommonDataManager.GetDollarAmountPermission();
        }

        public bool GetDollarAmountPTTPermission()
        {
            return PrefsCommonDataManager.GetDollarAmountPTTPermission();
        }
        public bool GetPSTCompanyModuleForUserPermission(int companyUserID)
        {
            return PrefsCommonDataManager.GetPSTCompanyModuleForUserPermission(companyUserID);
        }
        public CounterPartyWiseCommissionBasis GetCommissionBasisPreferences()
        {
            return PrefsCommonDataManager.GetCounterPartyWiseCommissionBasisPreferences(_user.CompanyUserID);
        }

        public TTGeneralPrefs GetTTGeneralPreferences()
        {
            return PrefsCommonDataManager.GetTTGeneralPrefs(_user.CompanyUserID);
        }

        /// <summary>
        /// Gets the quick tt preferences.
        /// </summary>
        /// <returns></returns>
        private QuickTTPrefs GetQuickTTPreferences()
        {
            return PrefsCommonDataManager.GetQuickTTPrefs(_user.CompanyUserID);
        }

        private QTTFieldPreference[] GetQTTFieldPreferences()
        {
            return PrefsCommonDataManager.GetQTTFieldPreference(_user.CompanyUserID);
        }

        public ConfirmationPopUp ConfirmationPopUpPrefs
        {
            get
            {
                if (_confirmationPopUp == null)
                {
                    _confirmationPopUp = new ConfirmationPopUp();
                }
                return _confirmationPopUp;
            }
        }
        public PriceSymbolValidation PriceSymbolValidationData
        {
            get
            {
                if (_riskAndValidateSettings == null)
                {
                    _riskAndValidateSettings = new PriceSymbolValidation();
                }
                return _riskAndValidateSettings;
            }

        }
        public void UpdateCacheTradingPrefs(PriceSymbolValidation priceSymbolPrefs, ConfirmationPopUp confirmationPopUpPrefs)
        {
            _confirmationPopUp = confirmationPopUpPrefs;
            _riskAndValidateSettings = priceSymbolPrefs;
        }

        public void SaveGeneralPrefs()
        {
            PrefsCommonDataManager.SaveGeneralPrefs(_ttGeneralPrefs, _user.CompanyUserID);
        }

        /// <summary>
        /// Saves the quick tt prefs.
        /// </summary>
        private void SaveQuickTTPrefs()
        {
            PrefsCommonDataManager.SaveQuickTTPrefs(_quickTTPrefs, _user.CompanyUserID);
        }
        public void SaveQTTFieldPreference()
        {
            PrefsCommonDataManager.SaveQTTFieldPreference(_qTTFieldPreference, _user.CompanyUserID);
        }
        public bool DeleteAllTradingTicketPreferences(int _userID, int assetID, int underlyingID, int counterpartyID, int venueID)
        {
            return PrefsCommonDataManager.DeleteAllTradingTicketPreferences(_userID, assetID, underlyingID, counterpartyID, venueID);
        }
        public PriceSymbolValidation GetRiskValidationPreferences(int _userID)
        {
            return PrefsCommonDataManager.GetRiskValidationPreferences(_userID);
        }
        public bool SaveRiskValidationPreferences(Prana.BusinessObjects.PriceSymbolValidation riskValidate)
        {
            return PrefsCommonDataManager.SaveRiskValidationPreferences(riskValidate);
        }


        public TradingTicketUIPrefs GetTTUserUIPreferences(int userID)
        {
            return PrefsCommonDataManager.GetTTUserUIPreferences(userID);
        }

        public TradingTicketUIPrefs GetTTCompanyUIPreferences(int companyID)
        {
            return PrefsCommonDataManager.GetTTCompanyUIPreferences(companyID);
        }

        /// <summary>
        /// Gets the trading rules preferences.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public TradingTicketRulesPrefs GetTradingRulesPreferences(int companyID)
        {
            return PrefsCommonDataManager.GetTradingRulesPreferences(companyID);
        }

        public bool IsTTPrefInitialized { get; set; }
    }

}
