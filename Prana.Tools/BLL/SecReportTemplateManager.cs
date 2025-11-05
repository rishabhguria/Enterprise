using Prana.LogManager;
using Prana.PM.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Prana.Tools
{
    /// <summary>
    /// class for the business logic of the security management report
    /// </summary>
    public class SecReportTemplateManager
    {
        /// <summary>
        /// Get the frequency types
        /// </summary>
        /// <returns>Dictionary of frequency types</returns>
        public static Dictionary<int, string> GetFrequency()
        {
            try
            {
                return SecReportTemplateDAL.GetFrequencyFromDB();
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
        /// get file formats for report
        /// </summary>
        /// <returns>dictionary of file formats</returns>
        public static Dictionary<int, string> GetReportFormats()
        {
            int formatID = 0;
            Dictionary<int, string> dictReportFormat = new Dictionary<int, string>();
            try
            {
                foreach (ImportFormat val in ImportFormat.GetValues(typeof(ImportFormat)))
                {
                    dictReportFormat.Add(formatID++, val.ToString());
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
            return dictReportFormat;
        }

        /// <summary>
        /// Get the list of columns for report
        /// </summary>
        /// <returns>List of columns</returns>
        public static Dictionary<string, string> GetColumns()
        {
            Dictionary<string, string> dictColumnsWithCaption = new Dictionary<string, string>();
            try
            {
                Dictionary<string, string> dictColumns = SecReportTemplateDAL.GetColumnsListFromDB();
                if (dictColumns.Keys.Count > 0)
                {
                    foreach (string columnName in dictColumns.Keys)
                    {
                        string colCaption = GetCaption(dictColumns[columnName]);
                        dictColumnsWithCaption.Add(columnName, colCaption);
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
            return dictColumnsWithCaption;
        }

        /// <summary>
        /// get the caption for the column with column name 
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <returns>caption for the column name</returns>
        public static string GetCaption(string columnName)
        {
            string caption = string.Empty;
            try
            {
                var r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |(?<=[^A-Z])(?=[A-Z]) |(?<=[A-Za-z])(?=[^A-Za-z])",
                    RegexOptions.IgnorePatternWhitespace);
                caption = r.Replace(columnName, " ");
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
            return caption;
        }

        /// <summary>
        /// get data source for the column chooser combo
        /// </summary>
        /// <param name="dictColumns">dictionary to get the values from</param>
        /// <returns>datatable holding the data</returns>
        internal static DataTable GetColumnsForReports(Dictionary<string, string> dictColumns)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ColumnName", typeof(string));
            dt.Columns.Add("ColumnCaption", typeof(string));

            try
            {
                foreach (string columnName in dictColumns.Keys)
                {
                    dt.Rows.Add(columnName, dictColumns[columnName]);
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
            return dt;
        }

        /// <summary>
        /// save the report in the database
        /// </summary>
        /// <param name="reportName">name of the report</param>
        /// <param name="startDate">Start date for the report</param>
        /// <param name="endDate">End date for the report</param>
        /// <param name="thirdparty">ID of the third party</param>
        /// <param name="account">ID of the account</param>
        /// <param name="selectedColumns">Comma separated list of columns to be shown in the report</param>
        /// <param name="grouping1">first Column to group by</param>
        /// <param name="grouping2">second Column to group by</param>
        /// <param name="grouping3">third Column to group by</param>
        /// <param name="reportFormat">Format of the report</param>
        internal static int SaveReport(int reportID, string reportName, DateTime startDate, DateTime endDate, string thirdpartyIDs, string account, string selectedColumns, string grouping, string cronExpression, string reportFormat, string whereClause)
        {
            int i = 0;
            try
            {
                object[] SaveParameter = { reportID, reportName, startDate, endDate, thirdpartyIDs, account, selectedColumns, grouping, cronExpression, reportFormat, whereClause };
                i = SecReportTemplateDAL.SaveReportInDB(SaveParameter);
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
        /// Get the data table of accounts for the selected third party
        /// </summary>
        /// <param name="dictAccounts">dictionary of accounts</param>
        /// <returns>datatable holding the account details</returns>
        internal static DataTable GetAccounts(Dictionary<int, string> dictAccounts)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FundID", typeof(int));
            dt.Columns.Add("FundName", typeof(string));
            try
            {
                foreach (int accountID in dictAccounts.Keys)
                {
                    dt.Rows.Add(accountID, dictAccounts[accountID]);
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
            return dt;
        }

        /// <summary>
        /// Get the report id-report names from the database
        /// </summary>
        /// <returns>dictionary of report id-report names</returns>
        internal static Dictionary<int, string> GetReports()
        {
            Dictionary<int, string> dictReports = new Dictionary<int, string>();
            try
            {
                dictReports = SecReportTemplateDAL.GetReportsIDNamesFromDB();
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
        /// Get report data from DB
        /// </summary>
        /// <returns>Datarow holding the report details</returns>
        internal static DataRow GetReportData(int reportID)
        {
            try
            {
                DataTable dt = SecReportTemplateDAL.GetReportDetails(reportID);
                return dt.Rows[0];
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
        /// get the details for grid report from the database
        /// </summary>
        /// <param name="accountIDColl">Collection of the account IDs</param>
        /// <param name="reportID">ID of the report</param>
        /// <returns>datatable holding the details</returns>
        internal static DataTable GetReportGridData(string[] accountIDColl, int reportID)
        {
            DataTable dt = new DataTable();
            try
            {
                string xmlDoc = "";
                if (accountIDColl != null && accountIDColl.Length > 0 && !String.IsNullOrWhiteSpace(accountIDColl[0]))
                    xmlDoc = CreateXML(accountIDColl);
                dt = SecReportTemplateDAL.GetGridReportDataFromDB(xmlDoc, reportID);
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
            return dt;
        }

        /// <summary>
        /// Create XML of the data of the grid
        /// </summary>
        /// <param name="accountIDColl">Collection of the accounts</param>
        /// <returns>String XML representaion of the details</returns>
        private static string CreateXML(string[] accountIDColl)
        {
            //string to hold the xml document
            string xmlDoc = string.Empty;

            try
            {
                DataSet ds = new DataSet("dsAccount");
                DataTable dt = new DataTable("dtAccount");
                dt.Columns.Add("FundID", typeof(int));
                foreach (string strAccountID in accountIDColl)
                {
                    if (!string.IsNullOrEmpty(strAccountID))
                    {
                        int accountId = Convert.ToInt32(strAccountID);
                        dt.Rows.Add(accountId);
                    }
                }
                ds.Tables.Add(dt);
                xmlDoc = ds.GetXml();
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
            return xmlDoc;
        }

        internal static int DeleteReport(int reportID)
        {
            int i = 0;
            try
            {
                i = SecReportTemplateDAL.DeleteReportFromDB(reportID);
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
        /// updates last run date for report template
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        internal static int UpdateReport(int reportID)
        {
            int i = 0;
            try
            {
                i = SecReportTemplateDAL.UpdateReportInDB(reportID);
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
