using Prana.BusinessObjects.SMObjects;
using Prana.LogManager;
using Prana.SecurityMasterNew.BLL;
using Prana.Utilities.UI.CronUtility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.Tools
{
    /// <summary>
    /// Class for business logic of SM batch
    /// </summary>
    class SMBatchManager
    {
        /// <summary>
        /// Get the data for the grid from the database
        /// </summary>
        /// <returns>datatable</returns>
        internal static BindingList<SMBatch> GetSMBatchData()
        {
            BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();

            try
            {
                smBatchList = SMBatchDAL.GetSMBatchDetails();
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

        /// <summary>
        /// get Batch types for batch
        /// </summary>
        /// <returns>dictionary of batch types</returns>
        //public static Dictionary<int, string> GetSMBatchTypes()
        //{
        //    int formatID = 0;
        //    Dictionary<int, string> dictReportFormat = new Dictionary<int, string>();
        //    try
        //    {
        //        foreach (SMBatchType val in SMBatchType.GetValues(typeof(SMBatchType)))
        //        {
        //            dictReportFormat.Add(formatID++, val.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return dictReportFormat;
        //}

        /// <summary>
        /// get Batch run types for batch
        /// </summary>
        /// <returns>dictionary of Batch Run types</returns>
        //public static Dictionary<int, string> GetSMBatchRunTypes()
        //{
        //    int runTypeID = 0;
        //    Dictionary<int, string> dictRunType = new Dictionary<int, string>();
        //    try
        //    {
        //        foreach (ScheduleType val in ScheduleType.GetValues(typeof(ScheduleType)))
        //        {
        //            dictRunType.Add(runTypeID++, val.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return dictRunType;
        //}




        /// <summary>
        /// Get the run time from the cron expression
        /// </summary>
        /// <param name="cronExp">cron expression</param>
        /// <returns>run time in string form</returns>
        public static string GetRunTime(string cronExp)
        {
            try
            {
                if (!string.IsNullOrEmpty(cronExp))
                {
                    return CronUtility.GetCronDescription(cronExp);
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
        /// Get the schedule from the cron expression
        /// </summary>
        /// <param name="cronExp">cron expression</param>
        /// <returns>schedule in string form</returns>
        public static string GetRunSchedule(string cronExp)
        {
            try
            {
                if (!string.IsNullOrEmpty(cronExp))
                {
                    CronDescription cronDetail = CronUtility.GetCronDescriptionObject(cronExp);
                    //String runtime = CronUtility.GetCronDescription(cronExp);
                    return cronDetail.Type.ToString();
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
        /// Save the single batch details
        /// </summary>
        /// <param name="values">array of values</param>
        /// <returns>ID of the batch</returns>
        internal static int SaveBatchDetailsFromCreator(SMBatch smBatch)
        {
            int i = 0;
            try
            {
                i = SMBatchDAL.SaveBatchDetailsInDB(smBatch);
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
        /// Delete SM batch record from db
        /// </summary>
        /// <param name="smBatchID">ID of the batch</param>
        /// <returns>Number of rows affected</returns>
        internal static int DeleteSMBatch(int smBatchID)
        {
            int i = 0;
            try
            {
                i = SMBatchDAL.DeleteSMBatchFromDB(smBatchID);
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
        /// returns true if systemLevelName already exists in DB
        /// </summary>
        /// <param name="systemLevelName"></param>
        /// <returns></returns>
        internal static bool IsSMBatchExist(string systemLevelName, int smBatchID)
        {
            BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();
            try
            {
                smBatchList = SMBatchDAL.GetSMBatchDetails();
                foreach (SMBatch smBatch in smBatchList)
                {
                    if (!string.IsNullOrWhiteSpace(smBatch.SystemLevelName) && smBatchID != smBatch.SMBatchID)
                    {
                        if (smBatch.SystemLevelName.Equals(systemLevelName))
                            return true;
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
            return false;
        }

        /// <summary>
        /// get ClientAccountMapping dictionary
        /// </summary>
        /// <returns>dictionary of ClientAccountMapping</returns>
        public static Dictionary<int, List<int>> GetClientAccountMapping()
        {
            Dictionary<int, List<int>> clientAccountMapping = new Dictionary<int, List<int>>();
            try
            {
                clientAccountMapping = SMBatchDAL.LoadAccountByClientFromDb();
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
            return clientAccountMapping;
        }

        /// <summary>
        /// get fields for batch
        /// </summary>
        /// <returns>dictionary of Fields</returns>
        public static ConcurrentDictionary<string, StructPricingField> GetSecurityFields()
        {
            ConcurrentDictionary<string, StructPricingField> dictFields = new ConcurrentDictionary<string, StructPricingField>();
            try
            {
                dictFields = SecMasterCommonCache.Instance.PricingField;
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
            return dictFields;
        }

        /// <summary>
        /// get SMBatchStatus from Work flow data 
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetSMBatchStatusfromWorkFlow(int TaskID)
        {
            Dictionary<string, int> dictSMBatchStatus = new Dictionary<string, int>();
            try
            {
                dictSMBatchStatus = SMBatchDAL.GetSMBatchStatusfromWorkFlow(TaskID);
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

        /// <summary>
        /// function to get smBatchID and systemLevelName
        /// </summary>
        /// <returns>returns dictionary of smBatch</returns>
        public static ConcurrentDictionary<int, string> GetSMBatchList()
        {
            ConcurrentDictionary<int, string> dictSMBatchList = new ConcurrentDictionary<int, string>();
            BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();
            try
            {
                smBatchList = SMBatchDAL.GetSMBatchDetails();
                foreach (SMBatch smBatch in smBatchList)
                {
                    dictSMBatchList.TryAdd(smBatch.SMBatchID, smBatch.SystemLevelName);
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
            return dictSMBatchList;
        }
    }
}
