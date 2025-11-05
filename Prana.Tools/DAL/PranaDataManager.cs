//using Prana.Reconciliation;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Prana.Tools
{
    public class PranaDataManager
    {
        public static List<GenericNameID> GetAllDataSourceNames()
        {
            List<GenericNameID> dataSourceList = new List<GenericNameID>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetAllDataSourceNames";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(new GenericNameID(row));

                    }
                    dataSourceList.Add(new GenericNameID(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
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

            return dataSourceList;
        }

        public static List<GenericNameID> GetAllCountries()
        {
            List<GenericNameID> dataSourceList = new List<GenericNameID>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCountries";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(new GenericNameID(row));

                    }
                    dataSourceList.Add(new GenericNameID(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
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

            return dataSourceList;
        }
        public static DataTable GetAllFiles()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FileID");
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileType");
            //string strSPName = string.Empty;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllFiles";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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

            return dt;
        }

        public static void SaveFile(string name, byte[] data, int fileType)
        {
            object[] parameters = new object[4];
            try
            {
                parameters[0] = name;
                parameters[1] = data;
                parameters[2] = fileType;
                parameters[3] = DateTime.UtcNow; ;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveFile", parameters);
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
        public static void DeleteFile(int fileID)
        {
            object[] parameters = new object[1];
            try
            {
                parameters[0] = fileID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteFile", parameters);
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

        internal static string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] TransformToBinary(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && path.Contains("\\"))
                {
                    FileStream fs = null;
                    BinaryReader br = null;
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    br = new BinaryReader(fs);
                    byte[] data = new byte[fs.Length];
                    int length = (int)fs.Length;
                    br.Read(data, 0, length);

                    if (fs != null)
                    {
                        fs.Close();
                    }
                    return data;
                }
                else
                {
                    return null;
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
            return null;
        }

        internal static DataSet GetMinimalDataForGroup(DateTime localAUECDate)
        {
            DataSet ds = new DataSet();
            Object[] parameter = new object[1];
            parameter[0] = localAUECDate;

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetGroupMinimalForDate", parameter);
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
        /// Returns all company users along with company
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllCompanyUsers()
        {
            Dictionary<int, string> dictCompanyUsers = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSMReportCompanyUser";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        if (!dictCompanyUsers.ContainsKey(dr.GetInt32(0)))
                            dictCompanyUsers.Add(dr.GetInt32(0), dr.GetString(1));
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
            return dictCompanyUsers;
        }
    }
}