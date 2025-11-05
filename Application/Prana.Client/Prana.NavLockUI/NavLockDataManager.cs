using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.NavLockUI
{
    /// <summary>
    /// Class responsible for Data Access in NAVLock UI
    /// </summary>
    public class NavLockDataManager
    {
        /// <summary>
        /// Adds the nav lock date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        internal static int AddNavLockDate(DateTime date, int userID, DateTime lockCreationDate)
        {
            int result = 0;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = date;
                parameter[1] = lockCreationDate;
                parameter[2] = userID;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_AddNAVLock", parameter).ToString());

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes the nav lock date.
        /// </summary>
        /// <param name="lockId">The lock identifier.</param>
        /// <param name="userID">The user identifier.</param>
        internal static async void DeleteNavLockDate(int lockId, int userID)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[3];
                    parameter[0] = lockId;
                    parameter[1] = DateTime.UtcNow;
                    parameter[2] = userID;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteNAVLock", parameter);
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Gets the nav lock dates.
        /// </summary>
        /// <returns></returns>
        internal static List<NavLockData> GetNavLockDates()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetNAVLocks";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return GetNAVLockFromData(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the nav lock from data.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        private static List<NavLockData> GetNAVLockFromData(DataSet ds)
        {
            List<NavLockData> data = new List<NavLockData>();

            try
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    NavLockData navLockData = new NavLockData();
                    navLockData.LockId = Convert.ToInt32(row["LockId"]);
                    DateTime lckDate = Convert.ToDateTime(row["LockDate"]);
                    navLockData.LockDate = lckDate.ToShortDateString();
                    navLockData.LockedById = Convert.ToInt32(row["CreatedBy"]);
                    navLockData.LockedByName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(navLockData.LockedById);
                    navLockData.LockCreationDate = row["CreationDate"].ToString();
                    data.Add(navLockData);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return data;

        }
    }
}
