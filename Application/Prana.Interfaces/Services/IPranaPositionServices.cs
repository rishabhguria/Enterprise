using Prana.BusinessObjects;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IPranaPositionServices : IServiceOnDemandStatus
    {
        [OperationContract]
        List<TaxLot> GetOpenPositions(DateTime date, string commaSapratedAccountIds = "");

        [OperationContract]
        List<TaxLot> GetOpenPositionsFromAnyDB(DateTime date, string connectionString, List<string> listGrouping);

        [OperationContract]
        Task<List<TaxLot>> GetGroupedPositions(List<string> listGrouping, DateTime date, bool isUnallocatedTradesIncludedInPositions, string commaSapratedAccountIds);

        [OperationContract]
        Task<GenericPositionData> GetGroupedPositionsAndTransactions(List<string> listGrouping, DateTime date, string commaSapratedAssetIDs, string commaSapratedAccountIds, string symbols, string customConditions, bool isSameDateInAllAUEC, bool isTransactionsIncludedInPositions, bool IsAccrualsNeeded, bool isUnallocatedTradesIncludedInPositions, bool isBetaNeeded, bool isFxRateNeeded, bool isUnallocatedTradesPermitted);

        /// <summary>
        /// </summary>
        /// <param name="auecDatesString"></param>
        /// <param name="isTodaysTransactions">If this is true then the function returns transactions of today's date else the positions till the supplied date</param>
        /// <param name="isTodaysTransactions">If this is true then the function returns transactions of today's date else the positions 
        /// till the supplied date</param>
        /// <returns></returns>
        [OperationContract]
        List<TaxLot> GetOpenPositionsOrTransactions(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, int dateType = 0);

        /// <summary>
        /// </summary>
        /// <param name="auecDatesString"></param>
        /// <param name="isTodaysTransactions">If this is true then the function returns transactions of today's date else the postions till the supplied date</param>
        /// <param name="isTodaysTransactions">If this is true then the function returns transactions of today's date else the postions 
        /// till the supplied date</param>
        /// <returns></returns>
        [OperationContract(Name = "GetOpenPositionsOrTransactionsOverload")]
        List<TaxLot> GetOpenPositionsOrTransactions(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, string CommaSeparatedAssetIDs, string commaSeparatedSymbols, string CommaSeparatedCustomConditions, int dateType = 0);

        /// <summary>
        /// this method is used to get open positions for a symbol from database
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="auecDatesString"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <returns></returns>
        [OperationContract]
        List<TaxLot> GetOpenPositionsOrTransactionsForASymbol(string symbol, string auecDatesString, string CommaSeparatedAccountIds, string orderSideTagValue);

        [OperationContract]
        List<TaxLot> GetUnallocatedTradesForDateString(string auecDatesString);

        [OperationContract]
        List<TaxLot> GetOpenPositionsFromDB(DateTime GivenDate, bool IsTodaysTransactions, string CommaSapratedAssetIDs, string CommaSapratedAccountIds);

        //Modified by omshiv, get data based on pranProcessDate or AUEClocalDate
        [OperationContract]
        DataSet FetchDataForGivenSpName(ReconParameters reconParameters, string commaSeparatedAssetIDs, string commaSeparatedAccountIDs);

        /// <summary>
        /// It returns all of the transactions after the supplied date. 
        /// </summary>
        /// <param name="auecDatesString"></param>
        /// <returns></returns>
        [OperationContract]
        List<TaxLot> GetPostDatedTransactions(string auecDatesString, int dateType = 0);

        [OperationContract]
        List<TaxLot> GetTransactions(DateTime date);

        [OperationContract]
        List<TaxLot> GetTransactionsForDateRange(DateTime FromDate, DateTime ToDate);

        [OperationContract]
        bool ReProcessSnapShot();

        [OperationContract]
        bool CheckIfReprocessingRequired(List<TaxLot> taxLots);

        [OperationContract]
        DateTime GetSnapShotDate();

        [OperationContract]
        bool SaveSnapShotDate(DateTime givenDate);

        [OperationContract]
        Dictionary<string, string> GetAllPSSymbols(List<PSSymbolRequestObject> PSReqObjectList);

        [OperationContract]
        Task<Dictionary<string, string>> GetAllPSSymbolsforRisk(List<PSSymbolRequestObject> PSReqObjectListforRisk);

        ISecMasterServices SecMasterServices
        { set; }

        [OperationContract]
        void RefershPSSymbolMappingCache(DataSet dsPSMapping);

        [OperationContract]
        Dictionary<string, List<TaxLot>> GetOpenPositionsAndTransactions(DateTime FromDate, DateTime ToDate, string CommaSapratedAssetIDs, string CommaSapratedAccountIds, string CommaSeparatedCustomConditions);

        [OperationContract]
        List<TaxLot> GetTransactionsToUpdateSettlementFxRate(DateTime ToDate, string CommaSapratedAccountIds);

        [OperationContract]
        //modified by: Bharat raturi, 1 aug 2014
        //purpose: send the accountIDs to make sure that only the data after the cash management start dates of the accounts is picked
        //List<TaxLot> GetCashJournalExceptionalTransactions(DateTime fromDate, DateTime toDate);
        List<TaxLot> GetCashJournalExceptionalTransactions(DateTime fromDate, DateTime toDate, string accountIDs);

        [OperationContract]
        Dictionary<int, int> GetAccountsLockStatus();

        [OperationContract]
        bool SetAccountsLockStatus(int userID, List<int> accounts);

        [OperationContract]
        List<int> GetListOfUnlockedAccounts(List<int> accountList);

        [OperationContract]
        void ReconPreferenceSaved(int userID);

        /// <summary>
        /// Gets data for cost adjustment taxlots
        /// </summary>
        /// <returns>List of data for cost adjustment taxlots</returns>
        [OperationContract]
        List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentData();

        [OperationContract]
        int HideOrderFromBlotter(string parentClOrderIds, bool isAllSubOrdersRemovable, int companyUserID, List<int> uniqueTradingAccounts);

        [OperationContract]
        int HideSubOrderFromBlotter(string subOrdersClOrderId, int companyUserID, List<int> uniqueTradingAccounts);
    }
}