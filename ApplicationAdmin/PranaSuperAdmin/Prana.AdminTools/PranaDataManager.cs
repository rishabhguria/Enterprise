using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;

namespace Prana.AdminTools
{
    class PranaDataManager
    {
        internal static string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        internal static byte[] TransformToBinary(string path)
        {
            if (path != "" && path.Contains("\\"))
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
                    //br.Close();
                }

                return data;
            }
            else
            {
                return null;
            }
        }

        internal static void SaveFile(string name, byte[] data, int fileType)
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

        internal static void DeleteFile(int fileID)
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

        internal static DataTable GetAllFiles()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FileID");
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileType");
            string strSPName = string.Empty;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFiles";

            try
            {
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
    }
}
