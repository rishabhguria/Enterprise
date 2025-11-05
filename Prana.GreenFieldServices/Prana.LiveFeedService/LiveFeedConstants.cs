using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LiveFeedService
{
    public class LiveFeedConstants
    {
        public const string MSG_CompliancePermissionReceived = "Compliance Permission received";
        public const string MSG_CompliancePermissionProcessed = "Compliance Permission processed";
        public const string MSG_LiveFeedSubscriptionReceived = "Live Feed Subscription received by userId:";
        public const string MSG_LiveFeedSubscriptionProcessed = "Live Feed Subscription processed by userId:";
        public const string MSG_LiveFeedUnsubscriptionReceived = "Live Feed Unsubscription received by userId:";
        public const string MSG_LiveFeedUnsubscriptionProcessed = "Live Feed Unsubscription processed by userId:";
        public const string MSG_LiveFeedSubscriptionForSymbol = "Live Feed Subscription for symbol {0} by userId:{1}";
        public const string CONST_MarketDataAccessIPAddresses = "marketDataAccessIPAddresses";
        public const string MSG_MarketDataIPAddressesUpdated = "Market Data Access IP Addresses Updated for User ID:";
        public const string CONST_ASK = "Ask";
        public const string CONST_BID = "Bid";
        public const string CONST_CHANGE = "Change";
        public const string CONST_LAST_PRICE = "LastPrice";
        public const string CONST_UPDATE_TIME = "UpdateTime";
        public const string CONST_CompanyFactsetContractType = "companyFactsetContractType";
        public const string CONST_IsFactsetResellerLoginSuccess = "isFactsetResellerLoginSuccess";
        public const string MSG_FactsetResellerLoginSuccessUpdated = "Factset Reseller user login status updated as {0} for the User ID: {1}";
        public const string CONST_SAMSARA = "Samsara";
    }
}
