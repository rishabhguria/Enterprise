using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enumerators;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface ICashManagementService : IServiceOnDemandStatus
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<List<Transaction>> GetCashImpact(DateTime fromDate, DateTime toDate, String accountIDs, int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Transaction> GetTransactionsBeetweenTwoDates(DateTime StartDate, DateTime EndDate, String accountIDs, int activitySource, int userId);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveTransactions(List<Transaction> lsTransactions, Dictionary<string, TransactionEntry> lsTransactionEntries, string source, int userId, bool isSymbolLevelAccruals);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<List<CompanyAccountCashCurrencyValue>> GetDayEndCash(DateTime givenDate, DateTime dtEndDate, string accountIDs);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        GenericDayEndData GetDayEndDataInBaseCurrency(DateTime givenDate, bool IsAccrualsNeeded, bool IsIncludeTradingDayAccruals = true);
        
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool IsAutoUpdateOptionsUDAWithUnderlyingUpdate();
       

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveDayEndData(Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> DateWiseDayEndDictionary, List<CompanyAccountCashCurrencyValue> lstDeletedData, int userID, string accountIDs = null);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveCIData(Dictionary<string, GenericBindingList<CollateralInterestValue>> DateWiseCIDictionary, List<CollateralInterestValue> lstDeletedData, int userID, string accountIDs = null);


        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveDailyCreditLimitValues(DataTable dtDailyCreditLimitValue, bool isSavingFromImport);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataTable GetDailyCreditLimitValues();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<TaxLot> GetExceptionalTradingData(DateTime startDate, DateTime endDate, string accountIDs);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet GetCashDividendFromActivities(string auecDateString);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<PranaBasicMessage> CalculateAccruedInterest(List<PranaBasicMessage> Taxlots);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<Dictionary<string, List<TaxLot>>> GetOpenPositionsForDailyCalculation(DateTime FromDate, DateTime ToDate, String accountIDs, int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<TaxLot> CalculateDailyCashImpact(Dictionary<string, List<TaxLot>> DicOpenPositionsAndTransactionsDateWise, int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<int, CashPreferences> GetCashPreferences();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UpdateDayEndBaseCashByForexRate(DataTable dtForexRate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        string GenerateID();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int UpdateCashAccountsTablesInDB(DataSet UpdatedDataSet, int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int UpdateCashActivityTablesInDB(DataSet UpdatedDataSet, int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet GetAccountBalancesAsOnDate(DateTime balDate, string accountIDs, int userId);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void CalculateAndSaveBalances(DateTime endDate, int userID, string accountIDs);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet GetSubAccountTransactionEntriesForDateRange(int subAccountID, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet SaveManualCashDividend(DataSet ds);


        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet GetCashDividends(string symbol, string commaSeparatedAccountIds, string dateType, DateTime dateFrom, DateTime dateTo, int userId);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int CreateJournalEntries(DataSet dataSet, CashTransactionType transactionSource, int userId, bool IsMultileg);

        //get the transactions for the Accounts sent as the string of comma separated values
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        //List<Transaction> GetTransactionsForOverriding(DateTime fromDate, DateTime toDate);
        List<Transaction> GetTransactionsForOverriding(DateTime fromDate, DateTime toDate, String accountIDs);

        List<Transaction> GenerateJournalEntry<T>(T data, int userId, bool isSymbolLevelAccruals);

        int SaveCashActionsforMTM(int userID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Transaction> GetJournalExceptions(DateTime startDate, DateTime endDate, string accountIDs);

        /// <summary>
        /// It Runs the revaluation till the given date..
        /// </summary>
        /// <param name="balDate"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        //modified by: Bharat raturi, 17 jul 2014
        //purpose: send the userID to find the default AUECID for the company using that user and comma separated accountIDs
        //void RunRevaluationProcess(DateTime balDate);
        Task<bool> RunRevaluationProcess(Nullable<DateTime> fromDate, DateTime toDate, int userID, string accountIDs, bool isManualRevaluation, bool isGetAccountBalancesClicked);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool GetIsRevaluationInProgress(string accountIds);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void CreateSubscriptionServicesProxyPricing(bool flag);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UpdateLastRevaluationCalculatedDateToGivenDate(DateTime date, String accountIDs, bool isUpdated);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<int, RevaluationUpdateDetail> GetLastCalcRevalutionDetail();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<int, BalanceUpdateDetail> GetLastCalcBalanceDetails();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Transaction> GetTransactionsByTransactionID(string transactionID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Transaction> GetTransactionsByCAID(string CAID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SetActicityServices(IActivityServices activityServices);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        string GetInvalidFundsForSymbolLevel(string fundIds, Nullable<DateTime> startDate, Nullable<DateTime> endDate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<CashManagementEnums.OperationMode, List<int>> GetAccountIdByRevaluationOperationMode();
    }
}
