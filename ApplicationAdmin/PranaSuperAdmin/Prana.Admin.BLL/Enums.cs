namespace Prana.Admin.BLL
{
    /// <summary>
    /// These Compliance should be mapped to database.
    /// </summary>
    public enum ComplianceOptions
    {
        ShortSaleAllowed = 1,
        //		ShortSaleConfirmation = 2,
        //		ProvidentAccountNameWithTrade = 3,
        //		ProvidentIdentifierWithTrade = 4
        ShortSaleConfirmation = 1,
        ProvidentAccountNameWithTrade = 1,
        ProvidentIdentifierWithTrade = 1
    }

    /// <summary>
    /// These order sides should be mapped to database.
    /// </summary>
    public enum OrderSides
    {
        Buy = 1,
        Sell = 2,
        Short = 3,
        BuyToCover = 4,
        BuyToOpen = 5,
        BuyToClose = 6,
        SellToOpen = 7,
        SellToClose = 8
    }

    /// <summary>
    /// Options to be set in combo.
    /// </summary>
    public enum Options
    {
        Yes = 1,
        No = 0
    }

    /// <summary>
    /// ContractMonthCode to be set in combo.
    /// </summary>
    public enum ContractMonthCode
    {
        Jan_Apr_Jul_Oct = 1,
        Feb_May_Aug_Nov = 2,
        Mar_Jun_Sep_Dec = 3,
        All = 4
    }

    public enum ModuleName
    {
        //Have the values directly from the DB. Depends upon the changes in DB.

        SLSU = 1,
        AUEC = 2,
        CV_MASTER = 3,
        COMMISSION_RULES = 4,
        THIRD_PARTIES = 5,
        VENDOR = 6,
        COMPANY_MASTER = 7,
        POSITION_MANAGEMENT = 8
    }

    //public enum ModuleResources
    //{
    //    //Have the values directly from the DB. Depends upon the changes in DB.

    //    COMPANY = 1,
    //    AUEC = 2,
    //    ASSET = 3,
    //    UNDERLYING = 4,
    //    EXCHANGE = 5,
    //    CURRENCY = 6,




    //}
    public enum ModuleResources
    {
        None = 0,
        SLSU = 1,
        AUEC = 2,
        CV_Master = 3,
        Third_Party = 4,
        Company_Master = 5,
        Position_Management = 6,
        EditTrades = 7,
        SymbolLookUp = 8,
        ReconCancelAmend = 9,
        User = 10,
        FileUpload = 11,
        ImportFileSetupTab = 12,
        ImportDataNew = 13,
        AdminTradingAcountSetup = 14,
        PermissionSetup = 16,
        AuditTrail = 17,
        Allocation = 18,
        TradingTicket = 19,
        ReleaseSetup = 20,
        AccountLock = 21,
        AuditTrailUI = 22,
        MappingForm = 23,
        //Added By Faisal Shah
        //Dated 01/07/14
        //Needed to Add User Permission Tab for User as well as Client in CH release
        UserPermissionsTab = 24,
        ClientPermissionsTab = 25,
        EnableDisabledItemsTab = 26,
        Global_Preferences = 27,
        CorporateAction = 28,
        ClosingUI = 29,
        PricingDataLookUp = 30,
        ClosingPrefsUI = 31,
        MasterDashboard = 32,
        CashManagementUI = 33,
        CashTransactions = 34,
        CashAccountsUI = 35,
        Exchanges = 36,
        Assets = 37,
        Currencies = 38,
        Underlyings = 39,
        AccountWiseUDA = 40,
        ThirdPartyManager = 41,
        LiveFeedValidationForm = 42,
        //added by amit on 23/04/2014
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3428
        FileFormatSection = 43
    }

    // Modified by Ankit Gupta on 10 Oct, 2014.
    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1595
    // If a third party is deleted, it should not be removed from Database, instead it should be marked as Inactive.
    public enum DisabledItemsList
    {
        Client = 1,
        MasterFunds = 2,
        Accounts = 3,
        MasterStrategies = 4,
        Strategy = 5,
        Users = 6,
        ThirdParty = 7
    } 
}
