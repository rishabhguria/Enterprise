using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    class CommonDataManager
    {
        public static DataSet GetPranaPreference()
        {

            DataSet ds = new DataSet();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPranaPreferences";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

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

            return ds;

        }
    }
}
