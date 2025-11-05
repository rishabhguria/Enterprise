using System;

namespace Prana.BusinessObjects.Classes.Allocation
{
    public class AllocationStringConstants
    {
        /// <summary>
        /// The master fund calculated preference prefix
        /// </summary>
        public const String MF_CALCULATED_PREF_PREFIX = "*MFPref#_";

        /// <summary>
        /// The custom tt preference prefix
        /// </summary>
        public const String CUSTOM_TT_PREF_PREFIX = "*Custom#_";

        /// <summary>
        /// The account
        /// </summary>
        public const string ACCOUNTS = "accounts";

        /// <summary>
        /// The strategy
        /// </summary>
        public const string STRATEGY = "strategy";

        /// <summary>
        /// The exchange
        /// </summary>
        public const string EXCHANGE = "exchange";

        /// <summary>
        /// The order side
        /// </summary>
        public const string ORDER_SIDE = "order side";

        /// <summary>
        /// The asset
        /// </summary>
        public const string ASSET = "asset";

        /// <summary>
        /// The pr
        /// </summary>
        public const string PR = "PR";

        /// <summary>
        /// The target percentage null
        /// </summary>
        public const String TARGET_PERCENTAGE_NULL = "\n Target perecentage is not provided";

        /// <summary>
        /// The percentage not negative
        /// </summary>
        public const string PERCENTAGE_NOT_NEGATIVE = "\n Percentage cannot be negative for ";

        /// <summary>
        /// The percentage sum not 100
        /// </summary>
        public const string PERCENTAGE_SUM_NOT_100 = "\n Sum of Percentage is not 100 for ";

        /// <summary>
        /// The account list not empty
        /// </summary>
        public const string ACCOUNT_LIST_NOT_EMPTY = "\n Account list cannot be empty for ";

        /// <summary>
        /// The accoun t_ lis t_ no t_ empty
        /// </summary>
        public const string PRORATA_SCHEME_NAME_NOT_BLANK = "\n Prorata Scheme Name cannot be blank.";

        /// <summary>
        /// The no default rule
        /// </summary>
        public const string NO_DEFAULT_RULE = "\n No default rule exist for this preference.";

        /// <summary>
        /// The default rule invalid
        /// </summary>
        public const string DEFAULT_RULE_INVALID = "\n Default rule is invalid.";

        /// <summary>
        /// The general rule invalid
        /// </summary>
        public const string GENERAL_RULE_INVALID = "\n General rule is invalid.";

        /// <summary>
        /// The list empty
        /// </summary>
        public const string LIST_EMPTY = "\n - Selected list should be empty for ";

        /// <summary>
        /// The no exchange selected
        /// </summary>
        public const String NO_EXCHANGE_SELECTED = "\n - No exchange is selected to ";

        /// <summary>
        /// The no orderside selected
        /// </summary>
        public const String NO_ORDERSIDE_SELECTED = "\n - No order side is selected to ";

        /// <summary>
        /// The no asset selected
        /// </summary>
        public const String NO_ASSET_SELECTED = "\n - No asset is selected to ";

        /// <summary>
        /// The no pr selected
        /// </summary>
        public const String NO_PR_SELECTED = "\n - No PR is selected to ";

        /// <summary>
        /// The duplicate general rule
        /// </summary>
        public const string DUPLICATE_GENERAL_RULE = "\n Duplicate general rule exists for this preference";
    }
}
