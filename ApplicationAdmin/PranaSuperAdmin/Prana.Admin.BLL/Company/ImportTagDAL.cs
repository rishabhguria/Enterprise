using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    class ImportTagDAL
    {
        internal static DataTable GetImportTagDataTableFromDB()
        {
            DataTable dt = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetImportTag";

                dt.Columns.Add("Acronym");
                dt.Columns.Add("ImportTagName");

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(new Object[2] { row[0].ToString(), row[1].ToString() });
                    }
                }
                return dt;
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

        internal static bool SaveImportTagData(string importTagXML)
        {
            object[] parameter = { importTagXML };

            string sProcSave = "P_SaveImportTag";
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery(sProcSave, parameter);
                return true;
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
    }
}
