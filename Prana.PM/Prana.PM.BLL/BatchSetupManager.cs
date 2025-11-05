using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.CronUtility;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.BLL
{
    public class BatchSetupManager
    {
        //DataTable dtBatch=new DataTable();
        public static Dictionary<int, Dictionary<int, string>> dictAccount = new Dictionary<int, Dictionary<int, string>>();
        public static Dictionary<int, BatchItem> dictBatch = new Dictionary<int, BatchItem>();
        public static Dictionary<int, string> dictFormat = new Dictionary<int, string>();

        /// <summary>
        /// Load the variables with data from database
        /// </summary>
        public static void InitializeData(int thirdPartyID)
        {
            try
            {
                List<int> userPermittedAccountIDs = GetUserPermittedAccountID();
                dictAccount = BatchSetupDAL.GetAccountsForSchedule();
                dictBatch = BatchSetupDAL.GetBatchDetailsFromDB(thirdPartyID, userPermittedAccountIDs);
                dictFormat = BatchSetupDAL.GetBatchFormatsFromDB();
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

        /// <summary>
        /// Get the details of the batches for the current third party
        /// </summary>
        /// <param name="thirdpartyid">Third party ID</param>
        /// <returns>The datatable of the information</returns>
        public static DataTable GetBatchDetails()//int thirdpartyid)
        {
            DataTable dtBatch = new DataTable();
            dtBatch.Columns.Add("BatchID", typeof(int));
            dtBatch.Columns.Add("FormatName", typeof(string));
            dtBatch.Columns.Add("Account", typeof(object));
            dtBatch.Columns.Add("ThirdPartyType", typeof(string));
            dtBatch.Columns.Add("EnablePriceTolerance", typeof(bool));
            dtBatch.Columns.Add("PriceCheckTolerance", typeof(decimal));
            dtBatch.Columns.Add("Schedule", typeof(int));
            dtBatch.Columns.Add("ExecTime", typeof(string));
            dtBatch.Columns.Add("AutoExec", typeof(string));
            dtBatch.Columns.Add("NxtExecTime", typeof(string));
            dtBatch.Columns.Add("LastExecTime", typeof(string));
            dtBatch.Columns.Add("LastScanResult", typeof(int));
            dtBatch.Columns.Add("CronExpression", typeof(string));

            Dictionary<int, List<int>> companyThirdPartyAccounts = CachedDataManager.GetInstance.CompanyThirdPartyAccounts();
            try
            {
                foreach (int batchID in dictBatch.Keys)
                {
                    //CHMW-1745	[Import] user is able to see other accounts data on Reports tab
                    if (companyThirdPartyAccounts.ContainsKey(dictBatch[batchID].ThirdPartyID))
                    {
                        dtBatch.Rows.Add(batchID, dictBatch[batchID].FormatName,
                                dictBatch[batchID].accountID, dictBatch[batchID].ThirdPartyType,
                            dictBatch[batchID].EnablePriceTolerance, dictBatch[batchID].PriceCheckTolerance,
                            dictBatch[batchID].Schedule, dictBatch[batchID].ExecTime,
                            dictBatch[batchID].AutoExec, dictBatch[batchID].NxtExecTime,
                            dictBatch[batchID].LastExecTime, dictBatch[batchID].LastScanResult, dictBatch[batchID].CronExpression);
                    }
                }
                foreach (DataRow row in dtBatch.Rows)
                {
                    string cron = Convert.ToString(row["CronExpression"]);
                    if (!string.IsNullOrEmpty(cron))
                    {
                        FillCronDetails(Convert.ToString(row["CronExpression"]), row);
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
            return dtBatch;
        }

        /// <summary>
        /// File the details related to cron expression
        /// </summary>
        /// <param name="cronExp"></param>
        /// <param name="row"></param>
        public static void FillCronDetails(string cronExp, DataRow row)
        {
            //TaskSchedulerForm ctrlTaskScheduler = new TaskSchedulerForm();
            // = new TaskSchedulerForm();
            //string cronExp = ctrlTaskScheduler.GetCronExpression();
            //grdBatchSetup.ActiveRow.Cells["CronExpression"].Value =cronExp;
            try
            {
                //if (cronExp == string.Empty)
                //{
                //    row["Schedule"] = -1;
                //    row["ExecTime"] = string.Empty;
                //    row["NxtExecTime"] = string.Empty;
                //    return;
                //}
                CronDescription cronDetail = CronUtility.GetCronDescriptionObject(cronExp);
                String runtime = CronUtility.GetCronDescription(cronExp);
                int schedule = (int)cronDetail.Type;
                row["Schedule"] = schedule;

                DateTime actionTime = new DateTime();
                DateTime nxtExecTime = new DateTime();
                switch (schedule)
                {
                    case 0:
                        nxtExecTime = DateTime.MinValue;// execTime;
                        string startTime = Convert.ToString(cronDetail.StartTime.ToShortTimeString());
                        string startDate = Convert.ToString(cronDetail.StartDate.ToShortDateString());
                        actionTime = Convert.ToDateTime(startDate + " " + startTime);

                        if (DateTime.Compare(DateTime.Now, actionTime) > 0)
                        {
                            runtime = "Expired";
                            row["NxtExecTime"] = string.Empty;
                        }
                        else
                        {
                            row["NxtExecTime"] = actionTime;
                            //row["LastExecTime"] = string.Empty;
                        }
                        break;
                    case 1:
                        string time = string.Empty;
                        if (cronDetail.StartTime.TimeOfDay > DateTime.Now.TimeOfDay)
                        {
                            time = DateTime.Now.ToShortDateString() + " " + cronDetail.StartTime.ToShortTimeString();
                            actionTime = Convert.ToDateTime(time);
                        }
                        else
                        {
                            time = DateTime.Now.AddDays(1).ToShortDateString() + " " + cronDetail.StartTime.ToShortTimeString();
                            actionTime = Convert.ToDateTime(time);
                        }
                        //if (cronDetail.StartTime > DateTime.Now)
                        //{
                        //    row["NxtExecTime"] = cronDetail.StartTime.ToLongDateString();
                        //}
                        //else
                        //{
                        //    string time = cronDetail.StartTime.ToShortTimeString();
                        //    string todayDate = DateTime.Now.ToShortDateString();
                        //    actionTime = Convert.ToDateTime(todayDate + " " + time);
                        //    row["NxtExecTime"] = actionTime.AddDays(1);
                        //    //row["LastExecTime"] = cronDetail.StartTime.ToLongDateString();
                        //}
                        //nxtExecTime = actionTime.AddDays(1);
                        break;
                    case 2:
                        if (cronDetail.StartTime > DateTime.Now)
                        {
                            row["NxtExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        else
                        {
                            row["NxtExecTime"] = cronDetail.StartTime.AddDays(7);
                            //row["LastExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        //nxtExecTime = actionTime.AddDays(7);
                        break;
                    case 3:
                        if (cronDetail.StartTime > DateTime.Now)
                        {
                            row["NxtExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        else
                        {
                            row["NxtExecTime"] = cronDetail.StartTime.AddMonths(1);
                            //row["LastExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        //nxtExecTime = actionTime.AddMonths(1);
                        break;
                    default:
                        break;
                }

                row["ExecTime"] = runtime;
                if (nxtExecTime == DateTime.MinValue)
                {
                    //row["NxtExecTime"] = string.Empty;
                }
                else
                {
                    //row["NxtExecTime"] = nxtExecTime.ToLongDateString();
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
        }

        /// <summary>
        /// Get the accounts for the currenct third party
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        /// <returns>The datatable of Account details</returns>
        public static DataTable GetCurrentScheduleAccounts(int scheduleID)
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("FundID", typeof(int));
            dtAccounts.Columns.Add("FundName", typeof(string));
            try
            {
                if (scheduleID == 0)
                {
                    foreach (int schedule in dictAccount.Keys)
                    {
                        foreach (int accountID in dictAccount[schedule].Keys)
                        {
                            dtAccounts.Rows.Add(accountID, dictAccount[schedule][accountID]);
                        }
                    }
                }
                if (dictAccount.ContainsKey(scheduleID))
                {
                    foreach (int accountID in dictAccount[scheduleID].Keys)
                    {
                        dtAccounts.Rows.Add(accountID, dictAccount[scheduleID][accountID]);
                    }
                }
                return dtAccounts;
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
            return null;
        }

        /// <summary>
        /// Save the details of the batch
        /// </summary>
        /// <param name="dtBatch"></param>
        /// <returns></returns>
        public static int SaveBatchDetails(DataTable dtBatch)
        {
            int i = 0;
            DataTable dtSaveData = new DataTable("dtBatch");
            DataSet dsSaveData = new DataSet("dsBatch");
            dtSaveData.Columns.Add("BatchID", typeof(int));
            dtSaveData.Columns.Add("EnablePriceTolerance", typeof(bool));
            dtSaveData.Columns.Add("PriceCheckTolerance", typeof(decimal));
            dtSaveData.Columns.Add("AutoExec", typeof(bool));
            dtSaveData.Columns.Add("CronExpression", typeof(string));
            dtSaveData.Columns.Add("ExecutionTime", typeof(string));
            dtSaveData.Columns.Add("ScheduleTypeID", typeof(int));
            try
            {
                foreach (DataRow dr in dtBatch.Rows)
                {
                    dtSaveData.Rows.Add(dr["BatchID"], dr["EnablePriceTolerance"], dr["PriceCheckTolerance"], dr["AutoExec"],
                        dr["CronExpression"], dr["ExecTime"], dr["Schedule"]);
                }
                dsSaveData.Tables.Add(dtSaveData);
                string xmlSaveData = dsSaveData.GetXml();
                i = BatchSetupDAL.SaveBatchDataInDB(xmlSaveData);
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
        /// added by: Bharat raturi, 13 jun 2014
        /// purpose: show only the active batches on the dashboard
        /// </summary>
        /// <returns>List of active batches</returns>
        public static List<string> GetBatchNamesForImportDashBoard()
        {
            List<string> listBatchNames = new List<string>();
            foreach (int batchID in dictBatch.Keys)
            {
                if (!string.IsNullOrWhiteSpace(dictBatch[batchID].FormatName))
                {
                    listBatchNames.Add(dictBatch[batchID].FormatName);
                }
            }
            return listBatchNames;
        }

        /// <summary>
        /// added by: Bharat raturi, 16 jun 2014
        /// get the batch name from the ID
        /// </summary>
        /// <param name="settingID">ID of the format</param>
        /// <returns>batch name</returns>
        public static string GetBatchNameFromID(int settingID)
        {
            try
            {
                if (dictBatch.ContainsKey(settingID))
                {
                    return dictBatch[settingID].FormatName;
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
            return string.Empty;
        }

        /// <summary>
        /// To get list of user permitted account IDs
        /// </summary>
        /// <returns></returns>
        internal static List<int> GetUserPermittedAccountID()
        {
            List<int> userPermittedAccountIDs = new List<int>();
            try
            {
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                DataTable userPermittedAccounts = WindsorContainerManager.GetAllPermittedAccounts(userID);

                foreach (DataRow dr in userPermittedAccounts.Rows)
                {
                    if (!userPermittedAccountIDs.Contains(Convert.ToInt32(dr["FundID"])))
                        userPermittedAccountIDs.Add(Convert.ToInt32(dr["FundID"]));
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
            return userPermittedAccountIDs;
        }
    }
}
