using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.BLL
{
    internal class BatchSetupDAL
    {
        /// <summary>
        /// Connection string for the database
        /// </summary>
        static string connStr = "PranaConnectionString";


        /// <summary>
        /// Get the formatID-FormatName from database
        /// </summary>
        /// <returns>The dictionary of formatID-FormatName</returns>
        internal static Dictionary<int, string> GetBatchFormatsFromDB()
        {
            Dictionary<int, string> dictFormat = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllBatchFormats";

                using (IDataReader drFormat = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drFormat.Read())
                    {
                        dictFormat.Add(drFormat.GetInt32(0), drFormat.GetString(1));
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
            return dictFormat;
        }

        /// <summary>
        /// Get the accounts for all the third parties from the database
        /// </summary>
        /// <returns>Dictionary of ThirdPartyID-(ThirdpartyaccountID-ThirdPartyAccountName)) pairs</returns>
        internal static Dictionary<int, Dictionary<int, string>> GetAccountsForSchedule()  //int primeBrokerID)
        {
            Dictionary<int, Dictionary<int, string>> dictScheduleAccounts = new Dictionary<int, Dictionary<int, string>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundsForSchedule";

                using (IDataReader drAccounts = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))    // param))
                {
                    while (drAccounts.Read())
                    {
                        int scheduleID = drAccounts.GetInt32(0);
                        if (dictScheduleAccounts.ContainsKey(scheduleID))
                        {
                            if (!dictScheduleAccounts[scheduleID].ContainsKey(drAccounts.GetInt32(1)))
                            {
                                dictScheduleAccounts[scheduleID].Add(drAccounts.GetInt32(1), drAccounts.GetString(2));
                            }
                        }
                        else
                        {
                            Dictionary<int, string> dictAccount = new Dictionary<int, string>();
                            dictAccount.Add(drAccounts.GetInt32(1), drAccounts.GetString(2));
                            dictScheduleAccounts.Add(scheduleID, dictAccount);
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
            return dictScheduleAccounts;
        }

        /// <summary>
        /// Get the details of the batch from the database
        /// </summary>
        /// <returns>The dictionary holding the batchID-Batch details pair</returns>
        internal static Dictionary<int, BatchItem> GetBatchDetailsFromDB(int thirdPartyID, List<int> userPermittedAccountIDs)
        {
            Dictionary<int, BatchItem> dictBatch = new Dictionary<int, BatchItem>();

            object[] param = { thirdPartyID };
            try
            {
                using (IDataReader drBatch = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllBatchDetails", param, connStr))
                {
                    while (drBatch.Read())
                    {
                        //int thirdPartyID = drBatch.GetInt32(7);
                        int batchID = 0;
                        //if (dictBatch.ContainsKey(thirdPartyID))
                        //{
                        // Adding only batches with user permitted accounts
                        if (userPermittedAccountIDs.Contains(drBatch.GetInt32(2)))
                        {
                            if (drBatch.GetValue(0) != DBNull.Value)
                            {
                                batchID = drBatch.GetInt32(0);
                            }
                            if (dictBatch.ContainsKey(batchID))
                            {
                                if (!dictBatch[batchID].accountID.Contains(drBatch.GetInt32(2)))
                                    dictBatch[batchID].accountID.Add(drBatch.GetInt32(2));
                            }
                            else
                            {
                                BatchItem batItem = new BatchItem();
                                batItem.BatchID = batchID;
                                if (drBatch.GetValue(1) != DBNull.Value)
                                {
                                    batItem.FormatName = drBatch.GetString(1);
                                    if (batItem.FormatName.Contains("Recon"))
                                        continue;
                                }
                                if (drBatch.GetValue(2) != DBNull.Value)
                                {
                                    if (!batItem.accountID.Contains(drBatch.GetInt32(2)))
                                        batItem.accountID.Add(drBatch.GetInt32(2));
                                }
                                if (drBatch.GetValue(3) != DBNull.Value)
                                {
                                    batItem.ThirdPartyType = drBatch.GetString(3);
                                }
                                if (drBatch.GetValue(4) != DBNull.Value)
                                {
                                    batItem.EnablePriceTolerance = drBatch.GetBoolean(4);
                                }
                                if (drBatch.GetValue(5) != DBNull.Value)
                                {
                                    batItem.PriceCheckTolerance = drBatch.GetDecimal(5);
                                }
                                if (drBatch.GetValue(6) != DBNull.Value)
                                {
                                    batItem.Schedule = drBatch.GetInt32(6);
                                }
                                if (drBatch.GetValue(7) != DBNull.Value)
                                {
                                    batItem.ExecTime = drBatch.GetString(7);
                                }
                                if (drBatch.GetValue(8) != DBNull.Value)
                                {
                                    batItem.AutoExec = drBatch.GetBoolean(8);
                                }
                                if (drBatch.GetValue(9) != DBNull.Value)
                                {
                                    batItem.CronExpression = drBatch.GetString(9);
                                }
                                if (drBatch.GetValue(10) != DBNull.Value)
                                {
                                    batItem.ThirdPartyID = drBatch.GetInt32(10);
                                }
                                dictBatch.Add(batchID, batItem);
                            }
                            //}
                            //else
                            //{
                            //    //Dictionary<int, BatchItem> dictBatchItem=new Dictionary<int,BatchItem>();
                            //    BatchItem batItem = new BatchItem();
                            //    batItem.BatchID = batchID;
                            //    batItem.FormatName = drBatch.GetString(1);
                            //    if (!batItem.accountID.Contains(drBatch.GetInt32(2)))
                            //        batItem.accountID.Add(drBatch.GetInt32(2));
                            //    batItem.PriceCheckTolerance = drBatch.GetDecimal(3);
                            //    batItem.Schedule = drBatch.GetInt32(4);
                            //    batItem.CronExpression = drBatch.GetString(5);
                            //    batItem.ExecTime = drBatch.GetDateTime(6);
                            //    batItem.ThirdPartyID = drBatch.GetInt32(7);
                            //    //dictBatchItem.Add(batchID, batItem);
                            //    dictBatch.Add(thirdPartyID, batItem);
                            //}
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
            return dictBatch;
        }

        internal static int SaveBatchDataInDB(string xmlSaveData)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveBatchSetupDetails";
                object[] param = { xmlSaveData };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, param, connStr);
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
    }
}
