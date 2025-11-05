using Prana.DatabaseManager;
using System.Data;

namespace Prana.DropCopyProcessor_PostTrade
{
    class DropCopyPersistence
    {
        public static DataTable GetDropCopyOrderRequest()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "DC_GetDropCopyOrderRequests";

            DataSet ds = new DataSet();
            ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            DataTable dt = ds.Tables[0];
            return dt;
        }
        public static DataTable GetDropCopyOrderFills()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "DC_GetDropCopyOrderFills";

            DataSet ds = new DataSet();
            ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
