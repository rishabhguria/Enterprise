using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IActivityServices : IServiceOnDemandStatus
    {
        /// <summary>
        /// Initializes the cache and other state of the manager
        /// </summary>
        void Initialize();

        List<CashActivity> GenerateCashActivity<T>(T data, CashTransactionType activitySource, CashTransactionType? callingFrom = null);

        /// <summary>
        /// Function made to serialize the Cash Activity creation coming from the Trading workflow
        /// </summary>
        /// <param name="groups"></param>
        void GenerateTradingCashActivity(List<AllocationGroup> groups);

        [OperationContract(Name = "Taxlot")]
        List<CashActivity> CreateActivity(List<TaxLot> data, CashTransactionType activitySource);

        //PRANA-9776
        [OperationContract(Name = "Dataset")]
        List<CashActivity> CreateActivity(DataSet data, CashTransactionType activitySource, int userId, bool isSymbolLevelAccruals);

        [OperationContract]
        int SaveActivity(List<CashActivity> lsCashActivity, int userId, bool isSymbolLevelAccruals);

        [OperationContract]
        Task<int> SaveActivityforMTM(List<CashActivity> lsCashActivity, int userID);

        [OperationContract]
        //modified by: Bharat raturi, 15 sep 2014
        //purpose: get the details accountwise by providing comma separated string of account ids 
        //List<CashActivity> GetAlreadyCalculatedDailyCashData(DateTime FromDate, DateTime ToDate);
        Task<List<CashActivity>> GetAlreadyCalculatedDailyCashData(DateTime FromDate, DateTime ToDate, string accountIDs);

        [OperationContract(Name = "ByDateRange")]
        //List<CashActivity> GetActivity(DateTime fromDate, DateTime toDate);
        //Added activity date type as a default argument for P_GetAllActivities for other procedures this is not needed
        List<CashActivity> GetActivity(DateTime fromDate, DateTime toDate, String accountIDs, int userId, String activityDateType, bool isActivitySource);

        [OperationContract]
        //List<CashActivity> GetActivityExceptions(DateTime fromDate, DateTime toDate);
        List<CashActivity> GetActivityExceptions(DateTime fromDate, DateTime toDate, string accountIDs);

        [OperationContract]
        List<CashActivity> GetOverridingActivity(DateTime fromDate, DateTime toDate, string accountIDs);

        // This will Generate & save the Manual journal exception
        [OperationContract]
        long SaveJournalException(DateTime fromDate, DateTime toDate, string accountIDs);

        //Added by Nishant Jain, 04-08-2015 
        [OperationContract]
        List<CashActivity> GetActivitiesByActivityId(string activityIds);

        //Get Activities By TransactionId
        [OperationContract]
        List<CashActivity> GetActivitiesByTransactionId(string transactionId);

        [OperationContract]
        void ProcessCashActivityQueue(string accountIds);

        [OperationContract]
        void AddToRevalRunningCache(string[] accountIDsArray);

        [OperationContract]
        void DeleteFromRevalRunningCache(string[] accountIDsArray);
    }
}
