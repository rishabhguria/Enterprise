using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    internal class NAVLockDAL
    {
        /// <summary>
        /// Load the NAV Lock information from the database
        /// </summary>
        /// <returns>The datatable holding the NAV lock information</returns>
        internal static DataTable GetNavDataFromDB()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundsNAVLockDetails";

                DataTable dtNavLock = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, ApplicationConstants.PranaConnectionString).Tables[0];
                return dtNavLock;
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

        /// <summary>
        /// save the lock records 
        /// </summary>
        /// <param name="xmlLock">XML docuemnt representing the information</param>
        /// <returns>count of rows affected by the query in Database</returns>
        internal static int SaveLockRecords(string xmlLock)//, int status)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveFundsNAVLockDetails";
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, new object[] { xmlLock }, ApplicationConstants.PranaConnectionString);//, status);
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

        internal static Tuple<string, int> GetLastLockDate(DateTime lockDate, int accountID)
        {
            //Modified by omshiv, get Last lock date with last audit trail Id 
            // Last audit trail id is usig for update status unlocked on unlocking 
            string lastLockDate = string.Empty;
            int auditTrailId = 0;
            Tuple<String, Int32> auditData = new Tuple<string, int>(lastLockDate, auditTrailId);
            // = new DateTime();
            //if (lockDate <= DateTimeConstants.MinValue)
            //{
            //    return auditData; // return lastLockDate;
            //}
            string sProc = "P_GetLastFundNavlockedDate";
            try
            {
                using (IDataReader drLockdate = DatabaseManager.DatabaseManager.ExecuteReader(sProc, new object[] { lockDate, accountID }, ApplicationConstants.PranaConnectionString))
                {
                    while (drLockdate.Read())
                    {
                        if (drLockdate.GetValue(0) != DBNull.Value)
                            lastLockDate = drLockdate.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        if (drLockdate.GetValue(1) != DBNull.Value)
                            auditTrailId = drLockdate.GetInt32(1);
                    }
                }
                //object value=dbLastLockDate.ExecuteScalar(sProc, param);
                //lastLockDate = (value == null) ? string.Empty : value.ToString();
                //if (!String.IsNullOrEmpty(lastLockDate))
                //{
                //    DateTime date = DateTime.Parse(lastLockDate);
                //    //lastLockDate = date.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                //    lastLockDate = date.ToString("yyyy-MM-dd HH:mm");
                //}

                auditData = new Tuple<string, int>(lastLockDate, auditTrailId);
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
            return auditData;
        }
    }
}

