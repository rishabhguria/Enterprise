using Prana.LogManager;
using System;
using System.Configuration;
using System.Xml.Serialization;
namespace Prana.BusinessObjects
{
    public class SecMasterConstants
    {

        public static readonly int DefaultEquityAUECID;
        public static readonly int DefaultOptionAUECID;
        public static readonly int DefaultFutureOptionAUECID;
        public static readonly int DefaultFutureAUECID;
        public static readonly int DefaultForwardAUECID;
        public static readonly int DefaultFixedIncomeAUECID;

        public const string CONST_GET_UDAASSET_SP = "P_UDAGetAllAssets";
        public const string CONST_GET_UDASectors_SP = "P_UDAGetAllSectors";
        public const string CONST_GET_UDASubSectors_SP = "P_UDAGetAllSubSector";
        public const string CONST_GET_SecurityTypes_SP = "P_UDAGetAllSecurityType";
        public const string CONST_GET_UDACountries_SP = "P_UDAGetAllCountry";
        public const string CONST_GET_DYNAMIC_UDA_SP = "P_UDA_GetDynamicUDA";

        public const string CONST_DELETE_UDAASSET_SP = "P_DeleteUDAssets";
        public const string CONST_DELETE_UDASectors_SP = "P_DeleteUDASector";
        public const string CONST_DELETE_UDASubSectors_SP = "P_DeleteUDASubSector";
        public const string CONST_DELETE_UDASecurityTypes_SP = "P_DeleteUDASecurityType";
        public const string CONST_DELETE_UDACountries_SP = "P_DeleteUDACountry";

        public const string CONST_INSERT_UDAASSET_SP = "P_InsertUDAAsset";
        public const string CONST_INSERT_UDASectors_SP = "P_InsertUDASector";
        public const string CONST_INSERT_UDASubSectors_SP = "P_InsertUDASubSector";
        public const string CONST_INSERT_UDASecurityTypes_SP = "P_InsertUDASecurityType";
        public const string CONST_INSERT_UDACountries_SP = "P_InsertUDACountry";

        public const string CONST_INUSED_UDAASSET_SP = "P_GetInUseUDAAsset";
        public const string CONST_INUSED_UDASectors_SP = "P_GetInUseUDASector";
        public const string CONST_INUSED_UDASubSectors_SP = "P_GetInUseUDASubSector";
        public const string CONST_INUSED_UDASecurityTypes_SP = "P_GetInUseUDASecurityType";
        public const string CONST_INUSED_UDACountries_SP = "P_GetInUseUDACountry";

        public const string CONST_TAG = "Tag";
        public const string CONST_HEADERCAPTION = "HeaderCaption";
        public const string CONST_CAPTION_HEADERCAPTION = "Header Caption";
        public const string CONST_DEFAULTVALUE = "DefaultValue";
        public const string CONST_CAPTION_DEFAULTVALUE = "Default Value";

        public const string CONST_UDAAsset = "UDAAsset";
        public const string CONST_UDACountry = "UDACountry";
        public const string CONST_UDASector = "UDASector";
        public const string CONST_UDASubSector = "UDASubSector";
        public const string CONST_UDASecurityType = "UDASecurityType";

        public const string Const_UDAReq = "UDA_AttributesReq";
        public const string Const_UDARes = "UDA_AttributesRes";

        public const string CONST_AllOpenSymbolsReq = "AllOpenSymbolsSMReq";
        public const string CONST_AllHistTradedSymbolsReq = "AllHistTradedSymbolsSMReq";
        public const string CONST_TradedSMDataUIRes = "TradedSMDataUIRes";

        public const string CONST_InUsedUDADataRes = "InUsedUDADataRes";
        public const string CONST_InUsedUDADataReq = "InUsedUDADataReq";

        //request for future root data for symbol      
        public const string CONST_SymbolRootData = "SymbolRootData";

        //Advanced search  filter on sm UI 
        public const string CONST_SMAdvncdSearch = "SMAdvncdSearch";

        //added by: Bharat Raturi
        //purpose: provide key for refreshing the server cache
        public const string CONST_REFRESH_CACHE = "RefreshCache";

        public const string CONST_SMGenericPriceRequest = "SMGenericPriceRequest";

        public const string CONST_SMGetFields = "SMGetFields";
        public const string CONST_SMFieldsResponse = "SMFieldsResponse";

        public const string CONST_CentralSMConnected = "CentralSMConnected";
        public const string CONST_CentralSMDisconnected = "CentralSMDisconnected";

        // Added regular expression constants for dynamic uda name and value input validation
        // http://jira.nirvanasolutions.com:8080/browse/PRANA-9369
        public const string CONST_DynamicUDANameRegex = @"^[a-zA-Z_][ a-zA-Z0-9_-]*$";
        public const string CONST_DynamicUDAValueRegex = @"^[ a-zA-Z0-9()+={}~`\[\];'<>?|_"",.$^:&%@*!#/\\-]*$";

        #region sec Master info/ error messages


        public const string MSG_SetSearchParams = "Please set search parameters to fetch security! ";
        public const string MSG_NotValidAUECError = "This is not a valid AUEC, Please re enter the AUEC details";
        public const string MSG_NoDataToSave = "Symbols Already Saved, No New Data to Save.";
        public const string MSG_SecurityAlreadyApproved = "Already Approved, Select un-approved securities!";
        public const string MSG_SelectSecToApprove = "Please select security for approve!";
        public const string MSG_CurrencyNotFoundLiveFeed = "CurrencyID not received from Live Feed But Processed with BaseCurrencyID";
        public const string MSG_AUECNotFoundLiveFeed = "AUECID not received from Live Feed But Processed with Default AUECID";
        public const string MSG_SearchAllOpenSymbols = "This will fetch all open traded symbol's information.";
        public const string MSG_SearchAllHistTradedSymbols = "This will fetch all historical traded symbol's information.";
        public const string MSG_SelectSMView = "";
        public const string MSG_ServerNotConnected = "TradeService not connected";
        public const string MSG_FullQuickScan = "Quick scan might not return all the results in case of wild card searches (e.g. - contains, * etc)";
        public const string MSG_CurrencyNotExistInSystem = "CurrencyID does not exist in system but processed with BaseCurrencyID";



        #endregion





        /// <summary>
        /// data sources of security 
        /// </summary>
        public enum SecMasterSourceOfData
        {
            [XmlEnumAttribute("0")]
            None,
            [XmlEnumAttribute("1")]
            ESignal,
            [XmlEnumAttribute("2")]
            SymbolLookup,
            [XmlEnumAttribute("3")]
            ImportData,
            [XmlEnumAttribute("4")]
            YahooFinance,
            [XmlEnumAttribute("5")]
            BloombergDLWS,
            [XmlEnumAttribute("6")]
            Database,
            [XmlEnumAttribute("7")]
            CorporateAction,
            [XmlEnumAttribute("8")]
            Xignite,
            [XmlEnumAttribute("9")]
            Others,
            [XmlEnumAttribute("10")]
            Internal,
            [XmlEnumAttribute("11")]
            FactSet,
            [XmlEnumAttribute("12")]
            ACTIV,
            [XmlEnumAttribute("13")]
            SAPI
        }
        /// <summary>
        /// Comments or error message for security
        /// </summary>
        public enum SecMasterComments
        {
            None = 0,
            InvalidAUECID = 1,
            InvalidAssetID = 2,
            InvalidExchangeID = 3,
            InvalidUnderLyingID = 4,
            InvalidCurrencyID = 5,
            InvalidMultiplier = 6,
            MissingTickerSymbol = 7,
            MissingLongName = 8,
            MissingPutOrCall = 9,
            MissingStrikePrice = 10,
            MissingUnderLyingSymbol = 11,
            InvalidExpirationDate = 12,
            // InvalidExpirationDate = 13,
            MissingLeadCurrency = 14,
            MissingVsCurrency = 15,
            InvalidMaturityDate = 16,
            InvalidDaysToSettlement = 17,
            InvalidFirstCouponDate = 18,
            InvalidIsZero = 19,
            MissingCoupon = 20,
            SymbolExistsInSM = 21,
            TradeExists = 22,

            DefaultAUECID = 23,
            DefaultCurrency = 24

        }

        /// <summary>
        /// UDA symbol data current or historical  on UI 
        /// </summary>
        public enum UDASymbolsViewType
        {
            Current,
            Historical
        }

        // enum for search filters
        public enum SearchCriteria
        {
            CompanyName = 0,
            UnderlyingSymbol = 1,
            Ticker = 2,
            ReutersSymbol = 3,
            ISIN = 4,
            SEDOL = 5,
            CUSIP = 6,
            Bloomberg = 7,
            OSIOption = 8,
            IDCOOption = 9,
            OPRAOption = 10,
            BBGID = 11,
            FactSetSymbol = 12,
            ActivSymbol = 13
        }
        // match conditions
        public enum SearchMatchOn
        {
            Contains = 0,
            Exact = 1,
            StartsWith = 2
        }
        // match conditions for intergers/Decimals/DateTime
        public enum SearchIntMatchOn
        {
            EqualTo = 0,
            LessThan = 1,
            greaterThan = 2,
            LessThanOrEqualTo = 3,
            greaterThanOrEqualTo = 4,
            NotEqualTo = 5
        }

        // Search And/ Or conditions
        public enum SearchAndOr
        {
            And = 0,
            Or = 1,

        }

        /// <summary>
        /// Search All/ Any conditions
        /// </summary>
        public enum AllAnyCondition
        {
            [XmlEnumAttribute("All of the Following")]
            And = 0,
            [XmlEnumAttribute("Any of the Following")]
            Or = 1
        }

        //enum for true value
        public enum EnumTrueFalse
        {
            True = 0,
            False = 1,

        }

        // Security actions
        public enum SecurityActions
        {
            ADD = 0,
            APPROVE = 1,
            SEARCH = 2,
            UPDATE = 3
        }

        // Security Views on symbol lookup UI
        public enum SmViewType
        {
            AllColumns = 0,
            UDAColumns = 1,
            CustomView = 2
        }

        public enum UDATypes
        {
            AssetClass,
            SecurityType,
            Sector,
            SubSector,
            Country
        }

        /// <summary>
        /// Default AUEC Ids
        /// </summary>
        static SecMasterConstants()
        {
            try
            {
                DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                DefaultOptionAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultOptionAUECID"]);
                DefaultFutureOptionAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFutureOptionAUECID"]);
                DefaultFutureAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFutureAUECID"]);
                DefaultForwardAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultForwardAUECID"]);
                DefaultFixedIncomeAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFixedIncomeAUECID"]);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



    }
}
