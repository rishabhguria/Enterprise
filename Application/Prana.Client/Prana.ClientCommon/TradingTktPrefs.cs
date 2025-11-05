using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.ClientPreferences;
using System.Collections.Generic;

namespace Prana.ClientCommon
{
    public class TradingTktPrefs
    {
        static TradingPreference _tradingPreference = null;

        public static TradingTicketUIPrefs UserTradingTicketUiPrefs
        {
            get
            {
                return _tradingPreference.UserTradingTicketUiPrefs;
            }
            set
            {
                _tradingPreference.UserTradingTicketUiPrefs = value;
            }
        }

        public static TradingTicketUIPrefs CompanyTradingTicketUiPrefs
        {
            get
            {
                return _tradingPreference.CompanyTradingTicketUiPrefs;
            }
            set
            {
                _tradingPreference.CompanyTradingTicketUiPrefs = value;
            }
        }

        /// <summary> 
        /// Gets or sets the trading rules preference
        /// </summary>
        public static TradingTicketRulesPrefs TradingTicketRulesPrefs
        {
            get
            {
                return _tradingPreference.TradingTicketRulesPrefs;
            }
            set
            {
                _tradingPreference.TradingTicketRulesPrefs = value;
            }
        }
        /// <summary> 
        /// Gets or sets the Restricted or Allowed Securities List
        /// </summary>
        public static List<string> RestrictedAllowedSecuritiesList
        {
            get
            {
                return _tradingPreference.RestrictedAllowedSecuritiesList;
            }
            set
            {
                _tradingPreference.RestrictedAllowedSecuritiesList = value;
            }
        }

        public static bool IsTickerSymbologySecuritiesList
        {
            get
            {
                return _tradingPreference.IsTickerSymbologySecuritiesList;
            }
            set
            {
                _tradingPreference.IsTickerSymbologySecuritiesList = value;
            }
        }
        public static CounterPartyWiseCommissionBasis CpwiseCommissionBasis
        {
            get
            {
                return _tradingPreference.CpwiseCommissionBasis;
            }
            set
            {
                _tradingPreference.CpwiseCommissionBasis = value;
            }
        }

        /// <summary>
        /// Gets or sets the quick tt prefs.
        /// </summary>
        /// <value>
        /// The quick tt prefs.
        /// </value>
        public static QuickTTPrefs QuickTTPrefs
        {
            get
            {
                return _tradingPreference.QuickTTPrefs;
            }
            set
            {
                _tradingPreference.QuickTTPrefs = value;
            }
        }

        public static QTTFieldPreference[] QTTFieldPreference
        {
            get
            {
                return _tradingPreference.QTTFieldPreference;
            }
            set
            {
                _tradingPreference.QTTFieldPreference = value;
            }
        }

        public static TTGeneralPrefs TTGeneralPrefs
        {
            get
            {
                return _tradingPreference.TTGeneralPrefs;
            }
            set
            {
                _tradingPreference.TTGeneralPrefs = value;
            }
        }

        public static bool IsTTPrefInitialized 
        {
            get
            {
                return _tradingPreference.IsTTPrefInitialized;
            }
            set
            {
                _tradingPreference.IsTTPrefInitialized = value;
            }
        }

        static CompanyUser _user = null;
        public static void SetClientCache(CompanyUser user)
        {
            if (_user == null || user.CompanyUserID != _user.CompanyUserID)
            {
                _user = user;
                _tradingPreference = new TradingPreference();
                _tradingPreference.SetClientCache(user);
                TTHelperManager.GetInstance().SetHelperCollection(user.CompanyID, user.CompanyUserID);
                MTTHelperManager.GetInstance().SetHelperCollection(user.CompanyID, user.CompanyUserID);
            }
            _user=user;
        }

        public static PreferencesUniversalSettingsCollection GetPreferences()
        {
            return _tradingPreference.GetPreferences();
        }

        public static bool? DollarAmountPermission
        {
            get
            {
                return _tradingPreference.DollarAmountPermission;
            }
        }

        public static bool GetDollarAmountPermission()
        {
            return _tradingPreference.GetDollarAmountPermission();
        }
        public static CounterPartyWiseCommissionBasis GetCommissionBasisPreferences()
        {
            return _tradingPreference.GetCommissionBasisPreferences();
        }

        public static TTGeneralPrefs GetTTGeneralPreferences()
        {
            return _tradingPreference.GetTTGeneralPreferences();
        }

        public static ConfirmationPopUp ConfirmationPopUpPrefs
        {
            get
            {
                return _tradingPreference.ConfirmationPopUpPrefs;
            }
        }
        public static PriceSymbolValidation PriceSymbolValidationData
        {
            get
            {
                return _tradingPreference.PriceSymbolValidationData; ;
            }

        }
        public static void UpdateCacheTradingPrefs(PriceSymbolValidation priceSymbolPrefs, ConfirmationPopUp confirmationPopUpPrefs)
        {
            _tradingPreference.UpdateCacheTradingPrefs(priceSymbolPrefs, confirmationPopUpPrefs);
        }

        public static void SaveGeneralPrefs()
        {
            _tradingPreference.SaveGeneralPrefs();
        }

        public static void SaveQTTFieldPreference()
        {
            _tradingPreference.SaveQTTFieldPreference();
        }
        public static bool DeleteAllTradingTicketPreferences(int _userID, int assetID, int underlyingID, int counterpartyID, int venueID)
        {
            return _tradingPreference.DeleteAllTradingTicketPreferences(_userID, assetID, underlyingID, counterpartyID, venueID);
        }
        public static PriceSymbolValidation GetRiskValidationPreferences(int _userID)
        {
            return _tradingPreference.GetRiskValidationPreferences(_userID);
        }
        public static bool SaveRiskValidationPreferences(Prana.BusinessObjects.PriceSymbolValidation riskValidate)
        {
            return _tradingPreference.SaveRiskValidationPreferences(riskValidate);
        }


        public static TradingTicketUIPrefs GetTTUserUIPreferences(int userID)
        {
            return _tradingPreference.GetTTUserUIPreferences(userID);
        }

        public static TradingTicketUIPrefs GetTTCompanyUIPreferences(int companyID)
        {
            return _tradingPreference.GetTTCompanyUIPreferences(companyID);
        }

        /// <summary>
        /// Gets the trading rules preferences.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public static TradingTicketRulesPrefs GetTradingRulesPreferences(int companyID)
        {
            return _tradingPreference.GetTradingRulesPreferences(companyID);
        }
    }
}
