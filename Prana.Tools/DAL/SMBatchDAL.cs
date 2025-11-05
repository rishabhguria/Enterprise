using Prana.BusinessObjects.SMObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Prana.Tools
{
    /// <summary>
    /// class for database connection of SM batch
    /// </summary>
    internal class SMBatchDAL
    {
        /// <summary>
        /// get the SM batch data from the database
        /// </summary>
        /// <returns>SMBatch object</returns>
        public static BindingList<SMBatch> GetSMBatchDetails()
        {
            BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSMBatchDetails";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    Dictionary<string, int> columnOrderInfo = Utils.GetColumnOrderList(reader);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        smBatchList.Add(FillSMBatchDetails(row, columnOrderInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return smBatchList;
        }

        private static SMBatch FillSMBatchDetails(object[] row, Dictionary<string, int> columnOrderInfo)
        {
            SMBatch smBatch = null;

            if (row != null)
            {
                smBatch = new SMBatch();

                try
                {

                    smBatch.SMBatchID = Convert.ToInt32(row[columnOrderInfo["SMBatchID"]]);
                    smBatch.SystemLevelName = Convert.ToString(row[columnOrderInfo["SystemLevelName"]]);
                    smBatch.UserDefinedName = Convert.ToString(row[columnOrderInfo["UserDefinedName"]]);
                    smBatch.CronExpression = Convert.ToString(row[columnOrderInfo["CronExpression"]]);
                    smBatch.Fields = Convert.ToString(row[columnOrderInfo["Fields"]]);
                    smBatch.RunTime = Convert.ToInt32(row[columnOrderInfo["RunTimeTypeID"]]);
                    smBatch.IsHistoric = Convert.ToBoolean(row[columnOrderInfo["IsHistoricDataRequired"]]);
                    smBatch.HistoricDaysRequired = Convert.ToInt32(row[columnOrderInfo["DaysOfHistoricData"]]);
                    smBatch.AccountIDs = Convert.ToString(row[columnOrderInfo["FundID"]]);
                    smBatch.Indices = Convert.ToString(row[columnOrderInfo["Indices"]]);
                    smBatch.FilterClause = Convert.ToString(row[columnOrderInfo["FilterClause"]]);
                    smBatch.BatType = (SMBatch.BatchType)row[columnOrderInfo["BatchType"]];
                }
                catch (Exception ex)
                {

                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
            return smBatch;
        }


        /// <summary>
        /// Overloaded version of the method for saving the single batch at a time
        /// </summary>
        /// <param name="values">array of values</param>
        /// <returns>number of affected values</returns>
        internal static int SaveBatchDetailsInDB(SMBatch smBatch)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveSMBatchSetup";
                object[] parameter = {
                                       smBatch.SMBatchID,
                                       string.IsNullOrEmpty(smBatch.SystemLevelName)?string.Empty:smBatch.SystemLevelName,
                                       string.IsNullOrEmpty(smBatch.UserDefinedName)?string.Empty:smBatch.UserDefinedName,
                                       string.IsNullOrEmpty(smBatch.AccountIDs)?string.Empty:smBatch.AccountIDs,
                                       string.IsNullOrEmpty(smBatch.Fields)?string.Empty:smBatch.Fields,
                                       smBatch.IsHistoric,
                                       smBatch.HistoricDaysRequired,
                                       smBatch.RunTime,
                                       string.IsNullOrEmpty(smBatch.CronExpression)?string.Empty:smBatch.CronExpression,
                                       string.IsNullOrEmpty(smBatch.FilterClause)?string.Empty:smBatch.FilterClause,
                                       string.IsNullOrWhiteSpace(smBatch.Indices)?string.Empty:smBatch.Indices,
                                       (int)smBatch.BatType
                                     };

                i = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(sProc, parameter) as object);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }

        /// <summary>
        /// Delete the sm batch from db
        /// </summary>
        /// <param name="smBatchID"></param>
        /// <returns></returns>
        internal static int DeleteSMBatchFromDB(int smBatchID)
        {
            int i = 0;
            try
            {
                object[] param = { smBatchID };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSecMasterBatch", param);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }

        /// <summary>
        /// Load all accounts from Data base according to client and add into accountCollection dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadAccountByClientFromDb()
        {
            Dictionary<int, List<int>> accountClientCollection = new Dictionary<int, List<int>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyFundsC";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        int clientId = Convert.ToInt32(dr[3]);
                        int accountId = Convert.ToInt32(dr[0]);
                        if (accountClientCollection.ContainsKey(clientId))
                            accountClientCollection[clientId].Add(accountId);
                        else
                        {
                            List<int> clientCollection = new List<int>();
                            clientCollection.Add(accountId);
                            accountClientCollection.Add(clientId, clientCollection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accountClientCollection;
        }

        internal static DataSet GetIndexSymbols()
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetIndexSymbols";

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return ds;
        }

        /// <summary>
        /// get SMBatchStatus from Work flow data 
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        internal static Dictionary<string, int> GetSMBatchStatusfromWorkFlow(int TaskID)
        {
            Dictionary<string, int> dictSMBatchStatus = new Dictionary<string, int>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = TaskID;
                string sProc = "P_GetSMBatchStatusFromWorkFlow";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(sProc, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictSMBatchStatus.Add(row[0].ToString(), int.Parse(row[1].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer. 
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictSMBatchStatus;
        }

    }
}
