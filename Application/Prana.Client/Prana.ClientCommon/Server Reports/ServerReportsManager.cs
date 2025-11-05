#region Using namespaces
using Prana.LogManager;
using System;
using System.Data;
#endregion

namespace Prana.ClientCommon
{
    public class ServerReportsManager
    {
        /// <summary>
        /// Get server reports
        /// </summary>
        /// <returns>the module ID</returns>
        public static DataTable GetServerReports(int companyID, int userID)
        {
            DataSet ds = new DataSet();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = userID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("PMGetServerReports", parameter);
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
            return ds.Tables[0];
        }
    }
}
