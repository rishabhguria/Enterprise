using Prana.Allocation.Client.Controls.Common.ViewModels;
using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class GeneralPreferencesViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _default rule control view model
        /// </summary>
        private DefaultRuleControlViewModel _defaultRuleControlViewModel;

        /// <summary>
        /// The _company wise preference control view model
        /// </summary>
        private CompanyWisePreferenceControlViewModel _companyWisePreferenceControlViewModel;

        /// <summary>
        /// The _user wise preference control view model
        /// </summary>
        private UserWisePreferenceControlViewModel _userWisePreferenceControlViewModel;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the company wise preference control view model.
        /// </summary>
        /// <value>
        /// The company wise preference control view model.
        /// </value>
        public CompanyWisePreferenceControlViewModel CompanyWisePreferenceControlViewModel
        {
            get { return _companyWisePreferenceControlViewModel; }
            set
            {
                _companyWisePreferenceControlViewModel = value;
                RaisePropertyChangedEvent("CompanyWisePreferenceControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the default rule control view model.
        /// </summary>
        /// <value>
        /// The default rule control view model.
        /// </value>
        public DefaultRuleControlViewModel DefaultRuleControlViewModel
        {
            get { return _defaultRuleControlViewModel; }
            set
            {
                _defaultRuleControlViewModel = value;
                RaisePropertyChangedEvent("DefaultRuleControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the user wise preference control view model.
        /// </summary>
        /// <value>
        /// The user wise preference control view model.
        /// </value>
        public UserWisePreferenceControlViewModel UserWisePreferenceControlViewModel
        {
            get { return _userWisePreferenceControlViewModel; }
            set
            {
                _userWisePreferenceControlViewModel = value;
                RaisePropertyChangedEvent("UserWisePreferenceControlViewModel");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPreferencesViewModel"/> class.
        /// </summary>
        public GeneralPreferencesViewModel()
        {
            try
            {
                _defaultRuleControlViewModel = new DefaultRuleControlViewModel();
                _companyWisePreferenceControlViewModel = new CompanyWisePreferenceControlViewModel();
                _userWisePreferenceControlViewModel = new UserWisePreferenceControlViewModel();
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
                companyWisePreference = CompanyWisePreferenceControlViewModel.GetCompanyWisePreferences();
                //set default rule
                AllocationRule defaultRule = GetCurrentDefaultRule();
                companyWisePreference.DefaultRule = defaultRule;
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
        /// Gets the current default rule.
        /// </summary>
        /// <returns></returns>
        private AllocationRule GetCurrentDefaultRule()
        {
            AllocationRule defaultRule = new AllocationRule();
            try
            {
                defaultRule = DefaultRuleControlViewModel.GetDefaultRule();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return defaultRule;
        }

        /// <summary>
        /// Gets the user wise preferences.
        /// </summary>
        /// <returns></returns>
        internal GeneralRules GetUserWisePreferences()
        {
            GeneralRules generalRules = new GeneralRules();
            try
            {
                generalRules = UserWisePreferenceControlViewModel.GetUserWisePreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return generalRules;
        }

        /// <summary>
        /// Called when [load general preferences control].
        /// </summary>
        /// <param name="userWisePreferences">The user wise preferences.</param>
        /// <param name="allocationCompanyWisePref">The allocation company wise preference.</param>
        internal void OnLoadGeneralPreferencesControl(AllocationPreferences userWisePreferences, AllocationCompanyWisePref allocationCompanyWisePref)
        {
            try
            {
                _defaultRuleControlViewModel.OnLoadDefaultRuleControl(false);
                _defaultRuleControlViewModel.SetDefaultRule(allocationCompanyWisePref.DefaultRule, false);
                _companyWisePreferenceControlViewModel.SetCompanyWisePreference(allocationCompanyWisePref);
                _userWisePreferenceControlViewModel.OnLoadSetPreferences(userWisePreferences);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
