using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// class for Data access for user permissions mapping
    /// </summary>
    public class UserPermissionDAL
    {
        /// <summary>
        /// Get the permissions details from DB
        /// </summary>
        /// <returns>Datatable holding the permission details</returns>
        internal static DataTable GetPermisssionsFromDB()
        {
            DataTable dtPermission = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAuthPermissions";

                dtPermission = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData).Tables[0];
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
            return dtPermission;
        }

        /// <summary>
        /// Get the authorization details from the db
        /// </summary>
        /// <returns>Dataset ho0lding the authorization details </returns>
        public static DataSet GetAuthKeyValuePairs()
        {
            DataSet keyAuthValuePairs = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAuthKeyValuePairs";

            try
            {
                keyAuthValuePairs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return keyAuthValuePairs;
        }

        /// <summary>
        /// Save the permissions in the database
        /// </summary>
        /// <param name="xmlPermission">XML document of permissions</param>
        /// <returns>number of affected rows in the db</returns>
        internal static int SavePermissionsInDB(string xmlPermission)
        {
            int i = 0;
            try
            {
                object[] param = { xmlPermission };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePermissions", param);
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
