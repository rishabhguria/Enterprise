using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class UserWisePreferenceControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _is save without state checked
        /// </summary>
        private bool _isSaveWithoutStateChecked;

        /// <summary>
        /// The _is save with state checked
        /// </summary>
        private bool _isSaveWithStateChecked;

        /// <summary>
        /// The _is clear allocation qty checked
        /// </summary>
        private bool _isClearAllocationQtyChecked;

        /// <summary>
        /// The is static accounts checked
        /// </summary>IsQtyFirstonGridChecked
        private bool _isStaticAccountsChecked;

        /// <summary>
        /// The _is default allocation checked
        /// </summary>
        private bool _isDefaultAllocationChecked;

        /// <summary>
        /// The _is allocation by account
        /// </summary>
        private bool _isAllocationByAccount;

        /// <summary>
        /// The _is allocation by symbol
        /// </summary>
        private bool _isAllocationBySymbol;

        /// <summary>
        /// The _is PTT CheckBox checked
        /// </summary>
        private bool _isCustomCheckBoxChecked;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocation by account.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocation by account; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocationByAccount
        {
            get { return _isAllocationByAccount; }
            set
            {
                _isAllocationByAccount = value;
                RaisePropertyChangedEvent("IsAllocationByAccount");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocation by symbol.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocation by symbol; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocationBySymbol
        {
            get { return _isAllocationBySymbol; }
            set
            {
                _isAllocationBySymbol = value;
                RaisePropertyChangedEvent("IsAllocationBySymbol");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clear allocation qty checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is clear allocation qty checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsClearAllocationQtyChecked
        {
            get { return _isClearAllocationQtyChecked; }
            set
            {
                _isClearAllocationQtyChecked = value;
                RaisePropertyChangedEvent("IsClearAllocationQtyChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default allocation checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default allocation checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefaultAllocationChecked
        {
            get { return _isDefaultAllocationChecked; }
            set
            {
                _isDefaultAllocationChecked = value;
                RaisePropertyChangedEvent("IsDefaultAllocationChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is static accounts checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is static accounts checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsStaticAccountsChecked
        {
            get { return _isStaticAccountsChecked; }
            set
            {
                _isStaticAccountsChecked = value;
                RaisePropertyChangedEvent("IsStaticAccountsChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocation by pst.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocation by ptt; otherwise, <c>false</c>.
        /// </value>
        public bool IsCustomCheckBoxChecked
        {
            get { return _isCustomCheckBoxChecked; }
            set
            {
                _isCustomCheckBoxChecked = value;
                RaisePropertyChangedEvent("IsCustomCheckBoxChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save without state checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save without state checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveWithoutStateChecked
        {
            get { return _isSaveWithoutStateChecked; }
            set
            {
                _isSaveWithoutStateChecked = value;
                RaisePropertyChangedEvent("IsSaveWithoutStateChecked");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save with state checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save with state checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveWithStateChecked
        {
            get { return _isSaveWithStateChecked; }
            set
            {
                _isSaveWithStateChecked = value;
                RaisePropertyChangedEvent("IsSaveWithStateChecked");
            }
        }

        #endregion Properties

        #region Constructors

        public UserWisePreferenceControlViewModel()
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
        /// Gets the user wise preferences.
        /// </summary>
        /// <returns></returns>
        internal GeneralRules GetUserWisePreferences()
        {
            GeneralRules generalRules = new GeneralRules();
            try
            {
                generalRules.IncludeSavewtoutState = IsSaveWithoutStateChecked;
                generalRules.IncludeSavewtState = IsSaveWithStateChecked;
                generalRules.ClearAllocationFundControlNumber = IsClearAllocationQtyChecked;
                generalRules.AllocationMethodologyRevertToDefault = IsDefaultAllocationChecked;
                generalRules.KeepAccountsGridFixed = IsStaticAccountsChecked;
                generalRules.IsAllocationByPST = IsCustomCheckBoxChecked;
                generalRules.AllocationPrefType = IsAllocationByAccount ? AllocationPreferenceType.AllocationByAccount : AllocationPreferenceType.AllocationBySymbol;
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
        /// Called when [load set preferences].
        /// </summary>
        /// <param name="userWisePreferences">The user wise preferences.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void OnLoadSetPreferences(AllocationPreferences userWisePreferences)
        {
            try
            {
                IsSaveWithoutStateChecked = userWisePreferences.GeneralRules.IncludeSavewtoutState;
                IsSaveWithStateChecked = userWisePreferences.GeneralRules.IncludeSavewtState;
                IsClearAllocationQtyChecked = userWisePreferences.GeneralRules.ClearAllocationFundControlNumber;
                IsDefaultAllocationChecked = userWisePreferences.GeneralRules.AllocationMethodologyRevertToDefault;
                IsStaticAccountsChecked = userWisePreferences.GeneralRules.KeepAccountsGridFixed;
                IsCustomCheckBoxChecked = userWisePreferences.GeneralRules.IsAllocationByPST;
                switch (userWisePreferences.GeneralRules.AllocationPrefType)
                {
                    case AllocationPreferenceType.AllocationByAccount:
                        IsAllocationByAccount = true;
                        break;
                    case AllocationPreferenceType.AllocationBySymbol:
                        IsAllocationBySymbol = true;
                        break;
                    default:
                        IsAllocationByAccount = true;
                        break;
                }
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
