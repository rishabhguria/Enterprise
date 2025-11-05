using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.CashManagement
{
    class CashAuditManager
    {
        #region Constants
        //Reval Related
        public static string Com_Man_Reval = "Manual Revaluation Run";
        public static string Com_RevalGetAccountBalances = "Revaluation Run using Get Account Balances";
        public static string Com_RevalwithoutGetAccountBalances = "Revaluation Run without using Get Account Balances";

        //Daily Calc related
        public static string Com_GetMTMData = "Daily MTM Calculations Get Data";
        public static string Com_CalculateMTMData = "Daily MTM Calculations Calculate Data";
        public static string Com_SaveMTMData = "Daily MTM Calculations Data Saved";

        //Day End Cash related
        public static string Com_DayEndCash = "Day End Cash Saved";
        public static string Com_GetDayEndCash = "Day End Cash Get Data";
        #endregion

        #region Methods
        /// <summary>
        /// Saves the cash actions in audit.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="action">The action.</param>
        public static void SaveCashActionsInAudit(int userId, string accountIds, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, string action)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveCashAuditAction";
                queryData.CommandTimeout = 60;
                queryData.DictionaryDatabaseParameter.Add("@userId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@userId",
                    ParameterType = DbType.Int32,
                    ParameterValue = userId
                });
                queryData.DictionaryDatabaseParameter.Add("@accountIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@accountIds",
                    ParameterType = DbType.String,
                    ParameterValue = accountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@fromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fromDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@toDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@toDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });
                queryData.DictionaryDatabaseParameter.Add("@action", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@action",
                    ParameterType = DbType.String,
                    ParameterValue = action
                });

                DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
