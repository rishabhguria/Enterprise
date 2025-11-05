namespace Prana.CashManagement
{
    public static class CashManagementConstants
    {
        #region Activity tables and relationship
        public const string TABLE_ACTIVITYTYPE = "ActivityType";
        public const string TABLE_ACTIVITYAMOUNTTYPE = "AmountType";
        //public const string TABLE_CASHTRANSACTIONTYPE = "CashTransactionType";
        public const string TABLE_ACTIVITYJOURNALMAPPING = "ActivityJournalMapping";
        public const string TABLE_SUBACCOUNTS = "SubCashAccounts";
        //public const string TABLE_ACTIVITYTRANSACTIONMAPPING = "ActivityTransactionMapping";
        public const string RELATION_ACTIVITYJOURNAL = "ActivityType";
        public const string RELATION_ACTIVITYAMOUNT = "AmountType";
        public const string RELATION_ACTIVITYSUBACCOUNTCR = "CreditAccount";
        public const string RELATION_ACTIVITYSUBACCOUNTDR = "DebitAccount";
        public const string RELATION_ISENABLED = "IsEnabled";
        public const string TABLE_SUBACCOUNTTYPELIST = "SubAccountTypeList";
        public const string TABLE_TRANSACTIONSOURCE = "TransactionSource";
        public const string TABLE_SUBACCOUNTSTYPE = "SubAccountType";
        #endregion

        #region tab keys
        public const string TAB_CASHACCOUNTS = "tabCashAccounts";
        public const string TAB_ACTIVITYTYPES = "tabActivityTypes";
        //public const string TAB_CASHTRANSACTIONTYPES = "tabCashTransactionTypes";
        public const string TAB_ACTIVITYJOURNALMAPPING = "tabActivityJournalMapping";
        //public const string TAB_ACTIVITYTRANSACTIONMAPPING = "tabCashTransactionActivityMapping";
        public const string TAB_SUBACCOUNTTYPE = "tabSubAccountType";
        #endregion

        #region Cash Account columns
        public const string COLUMN_SUBACCOUNTTYPEID = "SubAccountTypeId";
        public const string COLUMN_ACCOUNTTYPEIDCAPITAL = "TransactionTypeID";
        public const string COLUMN_NAME = "Name";
        public const string COLUMN_ACRONYM = "Acronym";
        public const string COLUMN_TYPEID = "TypeID";
        public const string COLUMN_SUBCATEGORYNAME = "SubCategoryName";
        public const string COLUMN_SUBCATEGORYID = "SubCategoryId";
        public const string COLUMN_SUBCATEGORY = "SubCategory";
        public const string COLUMN_MASTERCATEGORYID = "MasterCategoryId";
        public const string COLUMN_SUBACCOUNTID = "SubAccountID";
        public const string COLUMN_SUBACCOUNT = "SubAccount";
        public const string COLUMN_SUBACCOUNTS = "SubCashAccounts";
        public const string COLUMN_ISFIXEDACCOUNT = "IsFixedAccount";
        public const string TABLE_MASTERCATEGORY = "MasterCategory";
        public const string TABLE_SUBCATEGORY = "SubCategory";
        public const string COLUMN_MASTERCATEGORY = "MasterCategory";
        // public const string TABLE_ACCOUNTTYPE = "AccountType";
        public const string TABLE_ACCOUNTTYPE = "TransactionType";
        //public const string COLUMN_ACCOUNTTYPE = "AccountType";
        public const string COLUMN_ACCOUNTTYPE = "TransactionType";
        public const string CAPTION_SUBCATEGORY = "Sub Category";
        public const string CAPTION_MASTERCATEGORY = "Master Category";
        // public const string COLUMN_ACCOUNTTYPEID = "AccountTypeId";
        public const string COLUMN_ACCOUNTTYPEID = "TransactionTypeId";
        public const string CAPTION_ACCOUNTTYPE = "Account Type";
        public const string RELATION_MASTERCATEGORYSUBCATEGORY = "masterCategorySubCategory";
        public const string RELATION_SUBCATEGORYSUBACCOUNTS = "subCategorySubAccounts";
        public const string COLUMN_MASTERCATEGORYNAME = "MasterCategoryName";
        #endregion

        public const string COLUMN_CASHID = "CashID";
        public const string COLUMN_TAXLOTSTATE = "TaxLotState";

        #region All Activity Columns
        public const string COLUMN_DATE = "Date";
        public const string CAPTION_DATE = "Date";
        public const string COLUMN_TRADEDATE = "Date";
        public const string CAPTION_TRADEDATE = "Trade Date";
        public const string COLUMN_SETTLEMENTDATE = "SettlementDate";
        public const string CAPTION_SETTLEMENTDATE = "Settlement Date";
        public const string COLUMN_ACCOUNT = "AccountName";
        //public const string CAPTION_FUND = "Account";
        public const string COLUMN_CURRENCY = "Currency";
        public const string COLUMN_CURRENCYNAME = "CurrencyName";
        public const string COLUMN_ACTIVITYSOURCE = "ActivitySource";
        public const string CAPTION_ACTIVITYSOURCE = "Activity Source";
        public const string COLUMN_TRANSACTIONSOURCE = "TransactionSource";
        public const string CAPTION_TRANSACTIONSOURCE = "Transaction Source";
        public const string COLUMN_BALANCETYPE = "BalanceType";
        public const string CAPTION_BALANCETYPE = "Balance Type";
        public const string COLUMN_LEADCURRENCY = "LeadCurrencyName";
        public const string CAPTION_LEADCURRENCY = "Lead Currency";
        public const string COLUMN_VSCURRENCY = "VsCurrencyName";
        public const string CAPTION_VSCURRENCY = "Vs Currency";
        public const string COLUMN_CLOSEDQTY = "ClosedQty";
        public const string CAPTION_CLOSEDQTY = "Quantity";
        public const string COLUMN_TOTALCOMMISSION = "TotalCommission";
        public const string CAPTION_TOTALCOMMISSION = "Total Commissions";
        public const string COLUMN_TOTALAMOUNT = "TotalAmount";
        public const string CAPTION_TOTALAMOUNT = "Total Amount";
        //PRANA-9777
        public const string COLUMN_MODIFYDATE = "ModifyDate";
        public const string CAPTION_MODIFYDATE = "Modify Date";
        //PRANA-9777
        public const string COLUMN_ENTRYDATE = "EntryDate";
        public const string CAPTION_ENTRYDATE = "Entry Date";

        public const string COLUMN_TRANSACTIONDATE = "TransactionDate";
        //PRANA-9776
        public const string COLUMN_USERID = "UserId";
        public const string COLUMN_USERNAME = "UserName";
        public const string CAPTION_USERNAME = "User";
        // PRANA-32889
        public const string CAPTION_SUBACCOUNTTYPE = "Sub Account Type";
        #endregion

        #region activity type columns
        public const string COLUMN_ACTIVITYTYPEID = "ActivityTypeId";
        public const string COLUMN_ACTIVITYAMOUNTTYPEID = "AmountTypeId";
        //public const string COLUMN_CASHTRANSACTIONTYPEID = "CashTransactionTypeId";
        public const string COLUMN_ACTIVITYTYPE = "ActivityType";
        public const string COLUMN_ACTIVITYACRONYM = "Acronym";
        public const string COLUMN_AMOUNTTYPE = "AmountType";
        public const string COLUMN_DEBITACCOUNT = "DebitAccount";
        public const string COLUMN_CREDITACCOUNT = "CreditAccount";
        public const string COLUMN_ACTIVITYTYPEID_FK = "ActivityTypeId_FK";
        public const string COLUMN_ACTIVITYDATETYPE = "ActivityDateType";
        public const string COLUMN_AMOUNTTYPEID_FK = "AmountTypeId_FK";
        public const string COLUMN_ISDIVIDENDTYPE = "IsDividendType";
        public const string CAPTION_AMOUNTTYPE = "Amount Type";
        public const string CAPTION_CREDITACCOUNT = "Credit Account";
        public const string CAPTION_DEBITACCOUNT = "Debit Account";
        public const string COLUMN_CASHVALUETYPE = "CashValueType";
        public const string CAPTION_CASHVALUETYPE = "Default Cash Value Type";
        //public const string COLUMN_CASHTRANSACTIONTYPENAME = "CashTransactionTypeName";
        //public const string COLUMN_CASHTRANSACTIONTYPEACRONYM = "CashTransactionTypeAcronym";
        public const string CAPTION_ISDIVIDENDTYPE = "Is Dividend Type";
        public const string CAPTION_ACTIVITYTYPE = "Activity Type";
        public const string CAPTION_ACTIVITYACRONYM = "Acronym";
        public const string CAPTION_ACTIVITYDATETYPE = "Activity Date Type";
        public const string COLUMN_SUBACCOUNTTYPE = "SubAccountType";
        #endregion

        #region Dividend Columns
        public const string COLUMN_CASHTRANSACTIONID = "CashTransactionId";
        public const string COLUMN_TAXLOTID = "TaxlotId";
        public const string COLUMN_CORPACTIONID = "CorpActionId";
        public const string COLUMN_PARENTROWPK = "ParentRow_PK";
        public const string COLUMN_FUNDID = "FundID";
        public const string COLUMN_STRATEGYID = "Level2Id";
        public const string COLUMN_EXDATE = "ExDate";
        public const string COLUMN_PAYOUTDATE = "PayoutDate";
        public const string COLUMN_SYMBOL = "Symbol";
        public const string COLUMN_CURRENCYID = "CurrencyID";
        public const string COLUMN_OTHERCURRENCYID = "OtherCurrencyID";
        public const string COLUMN_AMOUNT = "Amount";
        public const string COLUMN_DIVRATE = "DivRate";
        public const string COLUMN_RECORDDATE = "RecordDate";
        public const string COLUMN_DECLARATIONDATE = "DeclarationDate";
        public const string COLUMN_DESCRIPTION = "Description";
        public const string COLUMN_COMPANYNAME = "CompanyName";
        public const string COLUMN_FXRATE = "FxRate";
        public const string COLUMN_FXCONVERSIONMETHODOPERATOR = "FXConversionMethodOperator";
        //public const string COLUMN_CASHTRANSACTIONTYPEID = "CashTransactionTypeId";
        #endregion

        #region Dividend Captions
        public const string CAPTION_FUND = "Account";
        public const string CAPTION_STRATEGY = "Strategy";
        public const string CAPTION_EXDATE = "Ex Date";
        public const string CAPTION_PAYOUTDATE = "Payout Date";
        public const string CAPTION_CURRENCY = "Currency";
        public const string CAPTION_OTHERCURRENCY = "Other Currency";
        public const string CAPTION_AMOUNT = "Amount";
        public const string CAPTION_DIVRATE = "Div Rate";
        public const string CAPTION_RECORDDATE = "Record Date";
        public const string CAPTION_DECLARATIONDATE = "Declaration Date";
        public const string CAPTION_DESCRIPTION = "Description";
        public const string CAPTION_COMPANYNAME = "Company Name";
        public const string CAPTION_CASHTRANSACTIONTYPE = "Cash Transaction Type";
        #endregion

        #region Search Box Default Texts
        public const string DEFAULTACTIVITYSEARCHTEXT = "Filter Activity Type";
        public const string DEFAULTACCOUNTSEARCHTEXT = "Filter Sub Account";
        #endregion
    }
}
