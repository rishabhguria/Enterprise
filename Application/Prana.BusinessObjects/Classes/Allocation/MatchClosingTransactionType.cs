using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Classes.Allocation
{
    public enum MatchClosingTransactionType
    {
        /// <summary>
        /// Matching of historical data will be not considered
        /// </summary>
        [EnumDescriptionAttribute("None")]
        None = 0,
        /// <summary>
        /// According to this, matching will be done for entire portfolio
        /// </summary>
        [EnumDescriptionAttribute("Complete Portfolio")]
        CompletePortfolio = 1,

        /// <summary>
        /// According to this, matching will be done for selected accounts/master funds only
        /// Currently being used only for masterFundPreference
        /// </summary>
        [EnumDescriptionAttribute("Selected MasterFunds")]
        SelectedAccounts = 2
    }
}
