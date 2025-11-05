using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;

namespace Prana.Allocation.Client.Definitions
{
    public class AllocationPreferencesCollection
    {
        #region Members

        /// <summary>
        /// The _allocation preferences
        /// </summary>
        private AllocationPreferences _allocationPreferences;

        /// <summary>
        /// The _allocation company wise preference
        /// </summary>
        private AllocationCompanyWisePref _allocationCompanyWisePref;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the preferences.
        /// </summary>
        /// <value>
        /// The preferences.
        /// </value>
        public AllocationPreferences AllocationPreferences
        {
            get { return _allocationPreferences; }
            set { _allocationPreferences = value; }
        }

        /// <summary>
        /// Gets or sets the company wise preference.
        /// </summary>
        /// <value>
        /// The company wise preference.
        /// </value>
        public AllocationCompanyWisePref CompanyWisePref
        {
            get { return _allocationCompanyWisePref; }
            set { _allocationCompanyWisePref = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AllocationPreferencesCollection"/> class from being created.
        /// </summary>
        AllocationPreferencesCollection()
        {
            try
            {
                _allocationPreferences = new AllocationPreferences();
                _allocationCompanyWisePref = new AllocationCompanyWisePref();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPreferencesCollection"/> class.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="allocationCompanyWisePref">The allocation company wise preference.</param>
        public AllocationPreferencesCollection(AllocationPreferences allocationPreferences, AllocationCompanyWisePref allocationCompanyWisePref)
        {
            try
            {
                _allocationPreferences = allocationPreferences;
                _allocationCompanyWisePref = allocationCompanyWisePref;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors
    }
}
