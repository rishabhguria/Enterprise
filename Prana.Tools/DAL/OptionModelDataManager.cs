using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Tools
{
    public class OptionModelDataManager
    {
        //private static int _errorNumber = 0;
        //private static string _errorMessage = string.Empty;

        public static System.Data.DataTable GetOptionModelUserDataFromDB(bool isOptions)
        {
            DataSet ds = new DataSet();

            Object[] parameter = new object[1];
            parameter[0] = isOptions;

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetOptionModelUserData", parameter);
            }
            #region Catch
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
            #endregion

            return ds.Tables[0];
        }
    }
}
