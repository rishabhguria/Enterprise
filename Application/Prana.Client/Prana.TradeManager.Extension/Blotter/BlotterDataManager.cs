using Prana.LogManager;
using System;
using System.Data;

namespace Prana.TradeManager.Extension
{
    /// <summary>/*g
    /// </summary>
    public class BlotterDataManager
    {
        private BlotterDataManager()
        {
        }

        private static readonly BlotterDataManager instance = new BlotterDataManager();
        public static BlotterDataManager GetInstance()
        {
            return instance;
        }

        public DataTable GetAllUsersByUserID(int _userID)
        {
            DataTable dt = new DataTable();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = _userID;
                dt.Columns.Add("TradinAccID");
                dt.Columns.Add("UserID");
                dt.Columns.Add("ShortName");
                dt.Columns.Add("AUECID");
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingAccountUsersByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        object[] newObj = new object[4];
                        newObj[0] = row[0].ToString();
                        newObj[1] = row[1].ToString();
                        newObj[2] = row[2].ToString();
                        newObj[3] = row[3].ToString();
                        dt.Rows.Add(newObj);
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

        /// <summary>
        /// Gets the allocation details by cl order identifier.
        /// </summary>
        /// <param name="parentClOrderID">The parent cl order identifier.</param>
        /// <returns></returns>
        internal DataTable GetAllocationDetailsByClOrderID(string parentClOrderIDs)
        {
            DataSet ds = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = parentClOrderIDs;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAllocationDetailsByClOrderID", parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// Gets the group identifier date by cl order identifier.
        /// </summary>
        /// <param name="parentClOrderID">The parent cl order identifier.</param>
        /// <returns></returns>
        internal DataTable GetGroupIdDateByClOrderID(string parentClOrderID)
        {
            DataSet ds = new DataSet();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = parentClOrderID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetGroupIdDateByClOrderID", parameter);
            }
            catch (Exception ex)
            {
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