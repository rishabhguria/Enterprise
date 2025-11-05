using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.BusinessObjects
{
    public class AllocationUIConstants
    {
        /// <summary>
        /// The account
        /// </summary>
        public const String FUND = "Account";
        /// <summary>
        /// The fun d_ identifier
        /// </summary>
        public const String FUND_ID = "AccountId";
        /// <summary>
        /// The percentage
        /// </summary>
        public const String PERCENTAGE = "_%";
        /// <summary>
        /// The quantity
        /// </summary>
        public const String QUANTITY = "_Qty";
        /// <summary>
        /// The strateg y_ prefix
        /// </summary>
        public const String STRATEGY_PREFIX = "S_";

        /// <summary>
        /// The save_ layout
        /// </summary>
        public const String SAVE_LAYOUT = "SaveLayout";

        /// <summary>
        /// The total _ percentage
        /// </summary>
        public const String TOTAL_PERCENTAGE = "Percentage";

        /// <summary>
        /// The total _ quantity
        /// </summary>
        public const String TOTAL_QUANTITY = "Quantity";

        /// <summary>
        /// The remainin g_ percentage
        /// </summary>
        public const String REMAINING_PERCENTAGE = "RemainingPercentage";

        /// <summary>
        /// The remainin g_ quantity
        /// </summary>
        public const String REMAINING_QUANTITY = "RemainingQuantity";

        /// <summary>
        /// Total
        /// </summary>
        public const String TOTAL_NAME = "Total";

        /// <summary>
        /// Remaining
        /// </summary>
        public const String REMAINING_NAME = "Remaining";

        /// <summary>
        /// Selected Trade
        /// </summary>
        public const String SELECTED_TRADE = "SelecedTrade";

        /// <summary>
        /// Total No. of Trades
        /// </summary>
        public const String TOTAL_NO_OF_TRADES = "TotalNoOfTrades";

        /// <summary>
        /// The version of saved layout. The version number needs to changed whenever columns or summaries are added in the grid.
        /// </summary>
        public const String LAYOUT_VERSION = "<Version>V1.7.138</Version>";

        /// <summary>
        /// The version of saved layout for allocated and unallocated grid
        /// </summary>
        public const String LAYOUT_VERSION_ALLOCATIONGRID = "<Version>V1.7.138.3</Version>";
    }
}
