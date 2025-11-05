using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.Allocation
{
    [Serializable]
    public class AllocationCompanyWisePref
    {
        /// <summary>
        /// Company Id for preference.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Default allocation rule for company
        /// </summary>
        public AllocationRule DefaultRule { get; set; }

        //https://jira.nirvanasolutions.com:8443/browse/PRANA-33956
        ///// <summary>
        ///// Check side to applied while allocating or not
        ///// </summary>
        //public bool DoCheckSide { get; set; }

        /// <summary>
        /// Show default rule control on Allocation UI
        /// </summary>
        public bool AllowEditPreferences { get; set; }

        /// <summary>
        /// Gets or sets the name of the selected prorata scheme.
        /// </summary>
        /// <value>
        /// The name of the selected prorata scheme.
        /// </value>
        public string SelectedProrataSchemeName { get; set; }

        /// <summary>
        /// Gets or sets the selected prorata scheme basis.
        /// </summary>
        /// <value>
        /// The selected prorata scheme basis.
        /// </value>
        public string SelectedProrataSchemeBasis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set scheme generation attributes prorata].
        /// </summary>
        /// <value>
        /// <c>true</c> if [set scheme generation attributes prorata]; otherwise, <c>false</c>.
        /// </value>
        public bool SetSchemeGenerationAttributesProrata { get; set; }

        ///// <summary>
        ///// Disable assests for checkside on Allocation
        ///// </summary>
        //public List<int> DisableCheckSideForAssets { get; set; }

        /// <summary>
        /// Number of digits after decimal point for precision value
        /// </summary>
        public int PrecisionDigit { get; set; }

        /// <summary>
        /// Assets with commission added in net amount
        /// </summary>
        public List<int> AssetsWithCommissionInNetAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show commission message on broker change].
        /// </summary>
        public bool MsgOnBrokerChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [recalculate commission on broker change].
        /// </summary>
        public bool RecalculateOnBrokerChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show commission message on allocation].
        /// </summary>
        public bool MsgOnAllocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [recalculate commission on allocation].
        /// </summary>
        public bool RecalculateOnAllocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable master fund allocation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable master fund allocation]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMasterFundAllocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is one symbol one master fund allocation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is one symbol one master fund allocation; otherwise, <c>false</c>.
        /// </value>
        public bool IsOneSymbolOneMasterFundAllocation { get; set; }



        /// <summary>
        /// Allocation Check Side Pref
        /// </summary>
        public AllocationCheckSidePref AllocationCheckSidePref { get; set; }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public bool IsValid(out string errorMessage)
        {
            bool isValid = true;
            errorMessage = string.Empty;
            try
            {
                isValid = DefaultRule.IsValid(out errorMessage);
                if (SelectedProrataSchemeName == null || SelectedProrataSchemeName.Trim().Equals(string.Empty))
                {
                    isValid = false;
                    errorMessage += AllocationStringConstants.PRORATA_SCHEME_NAME_NOT_BLANK;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isValid;
        }

    }
}
