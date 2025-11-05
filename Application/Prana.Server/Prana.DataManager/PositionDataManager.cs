using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.DataManager
{
    public class PositionDataManager
    {
        public static DataSet GetOpenPositions()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetFundOpenPositionsForDateBase_New";
                queryData.CommandTimeout = 900;
                string ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);

                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ToAllAUECDatesString
                });

                DataSet productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return productsDataSet;
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
    }
}
