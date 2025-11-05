using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Tools
{
    class SecReportTemplateDAL
    {
        /// <summary>
        /// Connection string
        /// </summary>
        static string connStr = ApplicationConstants.PranaConnectionString;
        /// <summary>
        /// Get the posting schedules from DB
        /// </summary>
        /// <returns>The dictionary of PostingScheduleID-PostingSchedule name</returns>
        internal static Dictionary<int, string> GetFrequencyFromDB()
        {
            Dictionary<int, string> dictPostSchedule = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllScheduleTypes";

                using (IDataReader drPostSchedule = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drPostSchedule.Read())
                    {
                        if (drPostSchedule.GetValue(0) != DBNull.Value && !dictPostSchedule.ContainsKey(drPostSchedule.GetInt32(0)))
                        {
                            dictPostSchedule.Add(drPostSchedule.GetInt32(0), drPostSchedule.GetString(1));
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
            return dictPostSchedule;
        }

        /// <summary>
        /// Get the list of columns for the report from the database
        /// </summary>
        /// <returns>Dictionary of column name-column name</returns>
        internal static Dictionary<string, string> GetColumnsListFromDB()
        {
            Dictionary<string, string> dictColumns = new Dictionary<string, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSecMasterDataColumnnames";

                using (IDataReader drColumns = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drColumns.Read())
                    {
                        if (drColumns.GetValue(0) != DBNull.Value && !dictColumns.ContainsKey(drColumns.GetString(0)))
                        {
                            dictColumns.Add(drColumns.GetString(0), drColumns.GetString(0));
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
            return dictColumns;
        }

        /// <summary>
        /// save the report in the database
        /// </summary>
        /// <param name="SaveParameter">Object array holding the parameters to save</param>
        /// <returns>number of rows affected</returns>
        internal static int SaveReportInDB(object[] SaveParameter)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveSecReportTemplate";
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, SaveParameter);
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
        /// get the report id names from the database
        /// </summary>
        /// <returns>dictionary of report id-report names</returns>
        internal static Dictionary<int, string> GetReportsIDNamesFromDB()
        {
            Dictionary<int, string> dictReports = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetReportsIDNames";

                using (IDataReader drReports = DatabaseManager.DatabaseManager.ExecuteReader(queryData, connStr))
                {
                    while (drReports.Read())
                    {
                        if (drReports.GetValue(0) != DBNull.Value && drReports.GetValue(1) != DBNull.Value)
                        {
                            int reportID = drReports.GetInt32(0);
                            string reportName = drReports.GetString(1);
                            if (!dictReports.ContainsKey(reportID))
                            {
                                dictReports.Add(reportID, reportName);
                            }
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
            return dictReports;
        }

        /// <summary>
        /// get the report details from db
        /// 
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="reportID">ID of the report</param>
        /// <returns>the datatable holding the details of the report</returns>
        internal static DataTable GetReportDetails(int reportID)
        {
            try
            {
                object[] param = { reportID };
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetSecReportDetails", param, connStr);
                return ds.Tables[0];
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
        /// Get the consolidated report data from db
        /// </summary>
        /// <param name="xmlDoc">XMl representation of the data</param>
        /// <returns>datatable holding the data</returns>
        internal static DataTable GetGridReportDataFromDB(string xmlDoc, int reportID)
        {
            try
            {
                object[] param = { xmlDoc, reportID };
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetSecReportForAccountSymbol", param, connStr);
                return ds.Tables[0];
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
        /// Delete the report from the database
        /// </summary>
        /// <param name="reportID">ID of the report</param>
        /// <returns>Number of rows affected</returns>
        internal static int DeleteReportFromDB(int reportID)
        {
            int i = 0;

            try
            {
                object[] param = { reportID };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSecReport", param, connStr);
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
        /// update last run time in Sec report template
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        internal static int UpdateReportInDB(int reportID)
        {
            int i = 0;
            try
            {
                object[] param = { reportID };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateSecReportTemplate", param);
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
