using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class PranaServerConstants
    {
        public enum OriginatorType
        {
            BuySide,
            SellSide,
            DropCopy,
            DropCopy_PM,
            Allocation
        }
        public enum BrokerConnectionType
        {
            [Description("None")]
            None,
            [Description("Send Only")]
            SendOnly,
            [Description("Send and Confirm back")]
            SendAndConfirmBack
        }
        public enum FixConnectionType
        {
            OMS,
            EOD
        }

        public enum OriginatorTypeCategory
        {
            OMS, //OriginatorType >> BuySide,SellSide,DropCopy,DropCopy_PM,
            EOD  //OriginatorType >> Allocation
        }

        public const string COUNTERPARTY_UP = "1";
        public const string COUNTERPARTY_DOWN = "2";
        public const string COMIN_QUEUE = "COMINQUEUE";
        public const string COMOUT_QUEUE = "COMINQUEUE";
        public const string DBQUEUE_PATH = "DBQUEUE_PATH";
        public const string ERRORQUEUE_PATH = "ERRORQUEUE_PATH";
        public const string CONNECTION_UNAVAILABLE_PATH = "ConnectionUnAvailable_PATH";
        public const string CP_SENT_MSGS_PATH = "CP_SENT_MSGS_PATH";
        public const string CP_RECEIVED_MSGS_PATH = "CP_RECEIVED_MSGS_PATH";
        public const string CLIENT_RECEIVED_PATH = "CLIENT_RECEIVED_PATH";
        public const string EXPOSURE_PNL_SERVICES = "ExposurePNLServices";
        public const string DRP_CPY_ERROR_MSGS_PATH = "DRP_CPY_ERROR_MSGS_PATH";
        public const string OLDTRADES_QUEUE_PATH = "OLDTRADES_QUEUE_PATH";
        public const string CASHACTIVITY_QUEUE_PATH = "CASHACTIVITY_QUEUE_PATH";
    }
}
