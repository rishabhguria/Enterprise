namespace Prana.BusinessObjects
{
    public class TradeManagerConstants
    {
        /// <summary>
        /// The duplicate trade timer
        /// </summary>
        public const string DUPLICATE_TRADE_TIMER = "DuplicateTradeCheckExpirationTime";

        /// <summary>
        /// The traded orders file name prefix
        /// </summary>
        public const string TRADED_ORDERS_FILE_PREFIX = @"\TradesEnteredFromTT_";

        /// <summary>
        /// The XML version tag
        /// </summary>
        public const string XML_VERSION_TAG = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        public const string CAPTION_MERGE_ORDERS = "Merge Orders";
        public const string PROPERTY_NIRVANA_BLOTTER = "Nirvana Blotter";
        public const string MSG_ORDERS_HAS_BEEN_REMOVED_OR_MERGED = "The order you selected is either Removed or Merged by other user";
    }
}
