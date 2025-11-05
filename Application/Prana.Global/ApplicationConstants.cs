using System;
using System.ComponentModel;

namespace Prana.Global
{
    public class ApplicationConstants
    {
        /// <summary>
        /// Ignore , For different AUECs the settlement days could be different.
        /// Saved in the db per auec wise so don't need to fetch from here.
        /// </summary>
        public const int NO_OF_SETTLEMENTDAYS = 3;

        /// <summary>
        /// Added By: Aman Seth
        /// Common date format for file saving
        /// </summary>
        public const string DateFormat = "MM-dd-yyyy";

        //UI ApplicationConstantss
        public const string C_COMBO_SELECT = "-Select-";
        public const string C_LIT_UNALLOCATED = "Unallocated";
        public const string C_COMBO_ALL = "- All -";
        public const string C_COMBO_NONE = "None";
        public const string C_Multiple = "Multiple";
        public const string C_Dash = "-";
        public const string C_NotAvailable = "N/A";
        public const string PREFS_FOLDER_NAME = "Prana Preferences";
        public const string TRADED_ORDERS_FOLDER_NAME = "Traded Orders";
        public const string DUMMY_PASSWORD = "********";

        #region Security master constants
        /// <summary>
        /// Here we have put all the Column names (property/field names). Once should refer these throughout the application.
        /// </summary>
        public const string CONST_SYMBOL = "Symbol";
        public const string CONST_UNDERLYINGSYMBOL = "UnderlyingSymbol";
        public const string CONST_POSITIONSTARTQUANTITY = "PositionStartQuantity";
        public const string CONST_AVERAGEPRICE = "AveragePrice";
        public const string CONST_AUECID = "AUECID";
        public const string CONST_ASSETNAME = "AssetName";
        public const string CONST_Multiplier = "Multiplier";
        public const string CONST_ASSETID = "AssetID";
        public const string CONST_UNDERLYINGID = "UnderlyingID";
        public const string CONST_EXCHANGEID = "ExchangeID";
        public const string CONST_CURRENCYID = "CurrencyID";
        public const string CONST_VSCURRENCYID = "VsCurrencyID";
        public const string CONST_TRADEDCURRENCYID = "TradedCurrencyID";
        public const string CONST_SETTLEMENTDATE = "SettlementDate";
        public const string CONST_EXPIRATIONDATE = "ExpirationDate";
        public const string CONST_PROCESSDATE = "ProcessDate";
        public const string CONST_ORIGINALPURCHASEDATE = "OriginalPurchaseDateCheck";
        public const string CONST_STARTDATE = "StartDate";
        public const string CONST_NETPOSITION = "NetPosition";
        public const string CONST_SIDETAGVALUE = "SideTagValue";
        public const string CONST_SIDE = "Side";
        public const string CONST_COSTBASIS = "CostBasis";
        public const string CONST_FUNDNAME = "AccountName";
        public const string CONST_USERID = "UserID";
        public const string CONST_COMPANYID = "CompanyID";
        public const string CONST_TRADINGACCOUNTID = "TradingAccountID";
        public const string CONST_COUNTERPARTYID = "CounterPartyID";
        public const string CONST_CurrentPosition = "CurrentPosition";
        public const string CONST_CensorValue = "****";
        public const string CONST_SharesOutstandingPercent = "SharesOutstandingPercent";
        public const string CONST_TradeQuantity = "TradeQuantity";
        public const string CONST_NavPercent = "NavPercent";
        public const int IndexOfNotional = 0;
        public const int IndexOfTradePrice =3;
        
        public const string CONST_VALIDATION_STATUS = "ValidationStatus";
        public const string CONST_IS_SECURITY_APPROVED = "IsSecApproved";
        public const string CONST_IS_SYMBOL_MAPPED = "IsSymbolMapped";
        public const string CONST_SEC_APPROVED_STATUS = "SecApprovalStatus";
        public const string CONST_APPROVED = "Approved";
        public const string CONST_UN_APPROVED = "UnApproved";

        public const string CONST_SYMBOL_LOOKUP = "Security Master";
        public const string CONST_WATCHLIST = "Watchlist";
        public const string CONST_COMPLIANCE = "Compliance Engine";
        public const string CONST_COMPLIANCE_ENGINE = "ComplianceEngine";
        public const string CONST_PRICING_INPUT = "Pricing Inputs";
        public const string CONST_UDA_UI = "UDAForm";
        public const string CONST_DataMapping_UI = "MappingForm";
        public const string CONST_COMPLIANCE_MODULE = "Compliance";
        public const string CONST_COMPLIANCE_ALERT_HISTORY = "AlertHistory";
        public const string CONST_COMPLIANCE_PENDING_APPROVAL = "PendingApproval";
        public const string CONST_COMPLIANCE_RULE_DEFINITION = "RuleDefinition";
        public const string CONST_ALERT_HISTORY = "Alert History";
        public const string CONST_PENDING_APPROVAL = "Pending Approval";
        public const string CONST_RULE_DEFINITION = "Rule Definition";
        //To match the Compliance Tab names to their names in DB
        public const string CONST_CE_RULE_DEFINITION = "Compliance Engine (Rule Definitions)";
        public const string CONST_CE_ALERT_HISTORY = "Compliance Engine (Alerts history)";
        public const string CONST_CE_PENDING_APPROVAL = "Compliance Engine (Pending Approval)";

        public const string CONST_REPORTS = "Reports";
        public const string CONST_BACK_OFFICE = "Back Office";
        public const string CONST_QUICKTT_PREFIX = "Quick TT(";
        public const string CONST_QUICK_TRADING_TICKET = "QuickTradingTicket";
        public const string CONST_TOOLS = "Tools";
        public const string CONST_HELP_AND_SUPPORT = "Help and Support";
        public const string CONST_MODULE_SHORTCUTS = "Module Shortcuts";
        public const string CONST_ABOUT_NIRVANA = "Software Details";
        public const string CONST_DISCLAIMER = "Disclaimer";
        public const string CONST_PREFERENCES = "Preferences";
        public const string CONST_BROKERCONNECTIONS = "Broker Connections";
        public const string CONST_RELOADSETTINGS = "Reload Settings";

        public const string CONST_RECON = "Recon";
        public const string CONST_RECONCILIATION = "Reconciliation";

        public const string CONST_CLOSING = "Closing";
        public const string CONST_CLOSEPOSITIONS = "Close Positions";

        public const string CONST_IMPORT = "Import";
        public const string CONST_IMPORTDATA = "Import Data";

        public const string CONST_AUTOIMPORT = "Auto Import";
        public const string CONST_AUTOIMPORTDATA = "Auto Import Data";

        public const string CONST_UNDEFINED = "Undefined";
        public const string CONST_MANUAL_TRADING_TICKET_UI = "ManualTradingTicket";
        public const string CONST_POST_RECON_AMENDMENTS = "PostReconAmendments";
        public const string CONST_Allocation = "Allocation";
        public const string CONST_PricingDataLookUp = "PricingDataLookUp";

        public const string CONST_LIVEFEEDVALIDATION_UI = "LiveFeedValidationForm";
        public const int CONST_ZERO = 0;
        public const double CONST_AVG_PRICE_ZERO = 0.0;

        public const string CONST_COLTradeDate = "AUECLocalDate";
        public const string CONST_COLClosingTradeDate = "ClosingTradeDate";

        public const string CONST_Trading = "Trading";

        #endregion

        public const int BASISPOINTVALUE = 10000;
        public const int BASISPOINTTOPERCENTAGE = 100;
        public const double DEFAULTDELTA = 999.00;
        public const int PERCENTAGEVALUE = 100;
        public const string APP_NAME = "Nirvana";

        public const String SMConnectionString = "SMConnectionString";
        public const String PranaConnectionString = "PranaConnectionString";
        public const String CSMConnectionString = "CSMConnectionString";
        public const int USDollar = 1; //PKID of US doller in Database
        public static bool IsLiveFeedLoggingEnabled = false;

        public const string NO_SYMBOLOGY_SELECT = "-1";

        public const string QUANTITY = "Quantity";
        public const string INCREASE_ON_QUANTITY = "Increase On Qty.";
        public const string DOLLAR_NOTIONAL = "$Amount";
        public const string DOLLAR_INCREASE_ON_NOTIONAL = "Increase on $Amount";

        public const string CRITERIA = "SearchCriteria";
        public const string SYMBOL = "Symbol";
        public const string ACTION = "Action";
        #region Filter Details CtrlMainConsolidationView PM
        public const string FilterDetails_All = "(All)";
        public const string FilterDetails_Custom = "(Custom)";
        public const string FilterDetails_Blanks = "(Blanks)";
        public const string FilterDetails_NonBlanks = "(NonBlanks)";
        public const string FilterDetails_DBNullAccount = "[Account] = '(DBNull)'";
        public const string FilterDetails_DBEmptyAccount = "[Account] = ''";
        public const string FilterDetails_DBNullMasterFund = "[MasterFund] = '(DBNull)'";
        public const string FilterDetails_DBEmptyMasterFund = "[MasterFund] = ''";
        public const string FilterDetails_DBNull = "(DBNull)";
        public const string FilterDetails_Unallocated = "Unallocated";
        public const string FilterDetails_UnallocatedLoadLayout = "= '(Unallocated)'";
        public const string FilterDetails_DBNullAccountBlank = "[Account] = ''";
        public const string FilterDetails_DBNullString = "= '(DBNull)'";
        #endregion

        public enum SymbologyCodes
        {
            [Description("Ticker Symbol")]
            TickerSymbol = 0,
            [Description("Reuters Symbol")]
            ReutersSymbol = 1,
            [Description("ISIN Symbol")]
            ISINSymbol = 2,
            [Description("SEDOL Symbol")]
            SEDOLSymbol = 3,
            [Description("CUSIP Symbol")]
            CUSIPSymbol = 4,
            [Description("Bloomberg Symbol")]
            BloombergSymbol = 5,
            [Description("OSIOption Symbol")]
            OSIOptionSymbol = 6,
            [Description("IDCOOption Symbol")]
            IDCOOptionSymbol = 7,
            [Description("OPRAOption Symbol")]
            OPRAOptionSymbol = 8,
            [Description("FactSet Symbol")]
            FactSetSymbol = 9,
            [Description("Activ Symbol")]
            ActivSymbol = 10
        }


        // validation status for Import UI
        public enum ValidationStatus
        {
            None = 0,
            UnApproved = 1,
            MissingData = 2,
            Validated = 3,
            NotExists = 4,
            InvalidAUEC = 5,
            AlreadyExists = 6,
            NonValidated = 7,
            NonPermittedAccounts = 8,
            InvalidData = 9
        }

        public const int SymbologyCodesCount = 11;
        public static readonly SymbologyCodes PranaSymbology = (SymbologyCodes)int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("PranaSymbology"));
        public enum PersistenceStatus
        {
            NotChanged,
            New,
            UnGrouped,//prevoiusly deleted Now Remove group only
            Deleted,
            CorporateAction,
            ReAllocated
        }
        /// <summary>
        /// used in Taxlot
        /// </summary>
        public enum TaxLotState
        {
            NotChanged,
            New,
            Updated,
            Deleted,
        }

        public enum MappingFileType
        {
            ThirdPartyXSLT,
            ReconXSLT,
            PMImportXSLT,
            EMSImportXSLT,
            ReconMappingXml,
            PranaXSD,
            ReconRulesFile,
            ReconXSD


        }

        public const string MAPPING_FILE_DIRECTORY = "MappingFiles";
        public const string RECON_DATA_DIRECTORY = "ReconData";
        public const string RECON_AmendmentsFileName = "Amendmendts.xml";
        public enum FormatterType
        {
            StringMsgFormatter,
            TagvalueFormatter,
            BinaryMsgFormatter

        }
        // SecurityMasterDataSource enum merged with SecMasterSourceOfData enum in SecMasterConstants
        //public enum SecurityMasterDataSource
        //{
        //    Xignite,
        //    LiveFeed,
        //    CorporateAction,
        //    Manual,
        //    Others
        //}
        // this is used in the Third Party module 
        public enum ThirdPartyNodeType
        {
            PrimeBrokerClearer = 1,
            Vendor = 2,
            ExecutingBroker = 3,
            AllDataParties = 4
        }

        public enum PlugInType
        {
            Menu = 1,
            StartUp = 2,
        }

        public enum CashInLieu
        {
            NoCashInLieu = 0,
            CloseAtMarkPrice = 1,
            CloseAtGivenPrice = 2
        }

        public enum TransmissionType
        {
            [Description("File")]
            File = 0,
            [Description("FIX")]
            FIX = 1
        }

        public enum AllocationMatchStatus
        {
            [Description("Not Sent")]
            NotSent = 0,
            [Description("Pending")]
            Pending = 1,
            [Description("Complete Match")]
            CompleteMatch = 2,
            [Description("Partial Mismatch")]
            PartialMismatch = 3,
            [Description("Complete Mismatch")]
            CompleteMismatch = 4,
            [Description("Partial Match")]
            PartialMatch = 5,
            [Description("Acknowledged")]
            AllocationAcknowledged = 6,
            [Description("Pending Acknowledgement")]
            PendingAcknowledgment = 7
        }

        public enum BlockMatchStatus
        {
            [Description("Pending")]
            Pending = -1,
            [Description("Matched")]
            Accepted = 0,
            [Description("Rejected")]
            BlockLevelReject = 1,
            [Description("Mismatched")]
            AccountLevelReject = 2,
            [Description("Received")]
            Received = 3,
            [Description("Incomplete")]
            Incomplete = 4,
            [Description("Rejected")]
            RejectedByIntermediary = 5,
            [Description("Pending Ack")]
            PendingAck = 6,
            [Description("Acknowledged")]
            Acknowledged = 7,
            [Description("Pending User Action")]
            PendingUserAction = 8,
            [Description("Cancelled")]
            Cancelled = 9,
            [Description("Mismatched")]
            MismatchedWithAllocRejCode = 10,
        }

        public enum BlockSubStatus
        {
            [Description("Sent to broker")]
            SentToBroker = -1,
            [Description("Exact Match")]
            ExactMatch = 0,
            [Description("Received by Broker")]
            ReceiveByBroker = 1,
            [Description("Block Accepted")]
            BlockAccepted = 2,
            [Description("Partial Confirmation")]
            PartialConfirmation = 3,
            [Description("Tolerance Match")]
            ToleranceMatch = 4,
            [Description("User accepted tolerance match")]
            UserAcceptedToleranceMatch = 5,
            [Description("Force Match")]
            ForceMatch = 6,
            [Description("Confirmation Rejected")]
            ConfirmationRejected = 7,
            [Description("Average Px Mismatch")]
            AvgPx = 9,
            [Description("Commission Mismatch")]
            Commission = 10,
            [Description("Misc Fee Mismatch")]
            MiscFee = 11,
            [Description("Net Money Mismatch")]
            NetMoney = 12,
            [Description("Multiple Mismatches")]
            Multiple = 13,
            [Description("Mismatched account")]
            MismatchedAccount = 15,
            [Description("Missing settlement instructions")]
            MissingSettlementInstructions = 16,
            [Description("Alloc Reject Code")]
            AllocRejCode = 17,
            [Description("Partial Tolerance Match")]
            PartialToleranceMatch = 18,
            [Description("NA")]
            NA = 19,
            [Description("Quantity Mismatch")]
            QtyMismatch = 23,
            [Description("Incomplete")]
            Incomplete = 24,
            [Description("Rejected by intermediary")]
            RejectedByIntermediary = 25,
            [Description("Account level reject")]
            AccountLevelReject = 26,
            [Description("Cancelled")]
            Cancelled = 27
        }

        public enum AllocMatchStatus
        {
            [Description("Pending")]
            Pending = -1,
            [Description("Compared")]
            Compared = 0,
            [Description("Uncompared")]
            Uncompared = 1,
            [Description("Advisory Or Alert")]
            AdvisoryOrAlert = 2,
            [Description("Exact match : Affirmed")]
            AllocationAcknowledged = 3,
            [Description("Pending Ack")]
            PendingAcknowledgment = 4,
            [Description("NA")]
            NA = 5,
            [Description("Affirmed")]
            ExactMatch = 6,
            [Description("Rejected")]
            Rejected = 7,
            [Description("Value within tolerance")]
            ToleranceMatch = 8,
            [Description("Average Px Mismatch")]
            AvgPx = 9,
            [Description("Commission Mismatch")]
            Commission = 10,
            [Description("Misc Fee Mismatch")]
            MiscFee = 11,
            [Description("Net Money Mismatch")]
            NetMoney = 12,
            [Description("Multiple Mismatches")]
            Multiple = 13,
            [Description("Received")]
            Received = 14,
            [Description("Mismatched account")]
            MismatchedAccount = 15,
            [Description("Missing settlement instructions")]
            MissingSettlementInstructions = 16,
            [Description("Confirmed")]
            Confirmed = 18,
            [Description("Request rejected")]
            RequestRejected = 19,
            [Description("Affirmed on Tolerance")]
            AffirmedOnTolerance = 20,
            [Description("Affirmed by force")]
            AffirmedByForce = 21,
            [Description("Rejected by Nirvana")]
            RejectByNirvana = 22,
            [Description("Quantity Mismatch")]
            QtyMismatch = 23
        }

        public enum AllocRejCode
        {
            [Description("unknown account(s)")]
            UnknownAccounts = 0,
            [Description("incorrect quantity")]
            IncorrectQuantity = 1,
            [Description("incorrect average price")]
            IncorrectAvgPrice = 2,
            [Description("unknown executing broker mnemonic")]
            UnknownExecBroker = 3,
            [Description("commission difference")]
            CommissionDifference = 4,
            [Description("unknown OrderID Tag <37>")]
            UnknownOrderID = 5,
            [Description("unknown ListID Tag <66>")]
            UnknownListID = 6,
            [Description("other")]
            Other = 7,
            [Description(" incorrect allocated quantity")]
            IncorrectAllocatedQty = 8,
            [Description("calculation difference")]
            CalculationDifference = 9,
            [Description("unknown or stale ExecID Tag <17>")]
            UnknownExecID = 10,
            [Description("mismatched data value")]
            MismatchedDataValue = 11,
            [Description("unknown ClOrdID Tag <11>")]
            UnknownCLOrderID = 12,
            [Description("warehouse request rejected")]
            WareHouseReqRejected = 13,
            [Description("Allocation Report Rejected")]
            AllocationReportRejected = 14,
            [Description("Rejected by Nirvana")]
            RejectedByNirvana = 15,
            [Description("NA")]
            NA = 16,
            [Description("Multiple Mismatches")]
            MultipleMismatches = 17
        }

        public enum AffirmStatus
        {
            [Description("Received")]
            Received = 1,
            [Description("Confirm rejected, i.e. not affirmed")]
            Rejected = 2,
            [Description("Affirmed")]
            Affirmed = 3,
        }

        // Tag 774 Confirm reject reason
        public enum ConfirmRejReason
        {
            [Description("Mismatched account")]
            MismatchedAccount = 1,
            [Description("Missing settlement instructions")]
            MissingSettlementInstructions = 2,
            [Description("Other")]
            Other = 99,
        }

        public enum AllocTransType
        {
            [Description("New")]
            New = 0,
            [Description("Replace")]
            Replace = 1,
            [Description("Cancel")]
            Cancel = 2
        }

        public enum ConfirmStatus
        {
            [Description("Received")]
            Received = 1,
            [Description("Mismatched account")]
            MismatchedAccount = 2,
            [Description("Missing Settlement Instructions")]
            MissingSettlementInstructions = 3,
            [Description("Confirmed")]
            Confirmed = 4,
            [Description("Request rejected")]
            RequestRejected = 5
        }

        public const string FORMAT_COSTBASIS = "#,0.0000";
        //public const string FORMAT_AVGPRICE = "#,0.########";
        public const string FORMAT_AVGPRICE = "#,0.####";
        public const string FORMAT_QTY = "#,##,###.##";
        public const string FORMAT_UNEXECUTED_QTY = "#,##,##0.##";
        public const string FORMAT_COMMISSIONANDFEES = "#,0.00000000";
        public const string FORMAT_RATE = "#,0.####";
        public const string FORMAT_QTY_TWO_DIGIT_PRECISION = "#,0.##";

        //For displaying summaries of the columns
        public const string SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION = "{0:#,0.##}";

        //Added constant for ReleaseViewType - omshiv, March 2014
        public const string CONST_RELEASE_VIEW_TYPE = "ReleaseViewType";
        public const string CONST_IS_NAVLOCK_ENABLED = "IsNAVLockingEnabled";
        public const string CONST_IS_AccountLOCK_ENABLED = "IsAccountLockingEnabled";
        public const string CONST_IS_PERMANENTDELETION_ENABLED = "IsPermanentDeletion";
        public const string CONST_IS_SHOWMASTERFUNCONTT_ENABLED = "IsShowMasterFundonTT";
        public const string CONST_IS_SHOWMASTERFUNDASCLIENT_ENABLED = "IsShowmasterFundAsClient";
        public const string CONST_IS_EquityOptionManualValidation_ENABLED = "IsEquityOptionManualValidation";
        public const string CONST_IS_CollateralMarkPriceValidation_ENABLED = "IsCollateralMarkPriceValidation";
        public const string Const_Is_ShowTillSettlementDate_ENABLED = "IsPriceEnterTillSettlementDateInDailyValuation";
        public const string CONST_IS_ShowmasterFundOnShortLocate_ENABLED = "IsShowmasterFundOnShortLocate";
        public const string CONST_IS_ImportOverrideOnShortLocate_ENABLED = "IsImportOverrideOnShortLocate";
        public const string CONST_IS_FILEPRICINGFORTOUCH_ENABLED = "IsFilePricingForTouch";

        //Added By faisal shah
        public const string CONST_LOCKED = "Locked";
        public const string CONST_UNLOCKED = "UnLocked";

        //added by: Bharat raturi, apr 2014
        //purpose: add constants for storing the value whether many to many strategy mapping and many to many master fund mapping is enabled
        public const string CONST_IS_FUND_MANYTOMANY_ENABLED = "IsAccountManyToManyMappingAllowed";
        public const string CONST_IS_STRATEGY_MANYTOMANY_ENABLED = "IsStrategyManyToManyMappingAllowed";

        //added by: Bharat raturi, 6 may 2014
        //purpose: add constants for storing the value whether many to many strategy mapping and many to many master fund mapping is enabled
        public const string CONST_IS_FEEDERFUND_ENABLED = "IsFeederAccountEnabled";

        public const string CONST_PRICINGSOURCE = "PricingSource";
        public const string CONST_SettlementAutoCalculateField = "SettlementAutoCalculateField";
        public const string CONST_ZEROCOMMISSIONFORSWAPS = "IsZeroCommissionAndFeesForSwaps";
        public const string CONST_AVGPRICEROUNDING = "AvgPriceRounding";

        //Bharat Kumar Jangir (04 October 2013)
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-2487
        public const double DELTAADJPOSITION_LME_MULTIPLIER = 0.453592;


        // Ankit Gupta on June 13, 2014:
        // Constants for CA Data Flow
        public const string CA_ORIGINAL = "Original";
        public const string CA_WITHDRAWAL = "Withdrawal";
        public const string CA_ADDITION = "Addition";
        public const string CA_FRACTIONALWITHDRAWAL = "FractionalSharesWithdrawal";
        public const string CA_FRACTIONALADDITION = "FractionalSharesAddition";


        // Renaming the CounterParty to Broker throughout the application, PRANA-11722
        public const string CONST_BROKER = "Broker";

        public const string TRADE_SERVER_ID_FOR_EXPNL = "-2";
        public const string TRADE_SERVER_UI_ID_FOR_EXPNL = "-12";

        public const string CONST_Is_Window_User_Req = "IsWindowUserReq";


        public const string CONST_CLOSING_SERVICE = "ClosingService: ";
        public const string CONST_ALLOCATION_SERVICE = "AllocationService: ";
        public const string CONST_SECMASTER_SERVICE = "SecMasterService: ";

        // constant for prana preference of RevaluationDailyProcessDays
        public const string CONST_REVALUATION_DAILY_PROCESS_DAYS = "RevaluationDailyProcessDays";

        public const string CONST_TRADE_ATTRIBUTE = "TradeAttribute";
    }
}