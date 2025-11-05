//using Prana.BusinessObjects.Classes.CommonObjects;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.CronUtility;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class NAVLockManager
    {
        /// <summary>
        /// DataTable for holding the NAV lock details
        /// </summary>
        internal static DataTable dtNavLock = new DataTable("dtNavLock");
        public static Dictionary<int, string> _dictUsers = new Dictionary<int, string>();

        /// <summary>
        /// Get the data from the database
        /// </summary>
        /// <returns>The datatable of the records</returns>
        public static DataTable InitializeData()
        {
            try
            {
                GetAllUsers();
                dtNavLock = NAVLockDAL.GetNavDataFromDB();
                if (!dtNavLock.Columns.Contains("SuggestedLockDate"))
                    dtNavLock.Columns.Add("SuggestedLockDate", typeof(string));
                if (!dtNavLock.Columns.Contains("PreviousLockDate"))
                    dtNavLock.Columns.Add("PreviousLockDate", typeof(string));
                if (!dtNavLock.Columns.Contains("ActionID"))
                    dtNavLock.Columns.Add("ActionID", typeof(int));
                if (!dtNavLock.Columns.Contains("Status"))
                    dtNavLock.Columns.Add("Status", typeof(string));
                if (!dtNavLock.Columns.Contains("LockedByUserName"))
                    dtNavLock.Columns.Add("LockedByUserName", typeof(string));
                if (!dtNavLock.Columns.Contains("UnlockedByUserName"))
                    dtNavLock.Columns.Add("UnlockedByUserName", typeof(string));


                foreach (DataRow dr in dtNavLock.Rows)
                {
                    //if (dr["LokedOn"] != DBNull.Value)
                    //{
                    //    dr["LokedOn"] = Convert.ToDateTime(dr["LokedOn"]).ToLocalTime();
                    //}
                    //if (dr["UnLokedOn"] != DBNull.Value)
                    //{
                    //    dr["UnLokedOn"] = Convert.ToDateTime(dr["UnLokedOn"]).ToLocalTime();
                    //}
                    //if (dr["LockAppliedOn"] != DBNull.Value)
                    //{
                    //    dr["LockAppliedOn"] = Convert.ToDateTime(dr["LockAppliedOn"]).ToLocalTime();
                    //}
                    ExtendDataTable(dr);
                    if (!String.IsNullOrEmpty(dr["LockAppliedOn"].ToString()))
                    {
                        DateTime dtLock = DateTime.Parse(dr["LockAppliedOn"].ToString());//.ToUniversalTime();
                        int accountID = int.Parse(dr["CompanyFundID"].ToString());
                        Tuple<string, int> auditData = GetNewLastLockdate(dtLock, accountID);
                        dr["PreviousLockDate"] = auditData.Item1;
                        dr["LockedAuditTrailId"] = auditData.Item2;
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
            return dtNavLock;
        }

        /// <summary>
        /// Get the list of all the users
        /// </summary>
        private static void GetAllUsers()
        {
            try
            {
                Users listUsers = UserManager.GetUsers();
                if (listUsers != null && listUsers.Count > 0)
                    foreach (User user in listUsers)
                    {
                        if (!string.IsNullOrEmpty(user.UserID.ToString()) && !string.IsNullOrEmpty(user.FirstName) && !_dictUsers.ContainsKey(user.UserID))
                        {
                            _dictUsers.Add(user.UserID, user.FirstName);
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
        }

        /// <summary>
        /// Set the values for the new columns
        /// </summary>
        /// <param name="dr">DataRow for which the values are to be set</param>
        public static void ExtendDataTable(DataRow dr)
        {
            DateTime suggestedLockDate = new DateTime();
            try
            {
                if (!string.IsNullOrEmpty(dr["LockAppliedOn"].ToString()))
                {
                    if (DateTime.Parse(dr["LockAppliedOn"].ToString()) > DateTimeConstants.MinValue)
                    {
                        dr["Status"] = "Locked";
                        if (!string.IsNullOrEmpty(dr["PostingLockScheduleID"].ToString()))
                        {
                            switch (Convert.ToInt32(dr["PostingLockScheduleID"]))
                            {
                                case 0:
                                    DateTime dtLock = DateTime.Parse(dr["LockAppliedOn"].ToString());
                                    if (dtLock < DateTime.Now)
                                        suggestedLockDate = DateTime.MinValue;// DateTime.Parse(dr["LockAppliedOn"].ToString()).AddHours(24);
                                    break;
                                case 1:
                                    suggestedLockDate = DateTime.Parse(dr["LockAppliedOn"].ToString()).AddHours(24);
                                    break;
                                case 2:
                                    suggestedLockDate = DateTime.Parse(dr["LockAppliedOn"].ToString()).AddDays(7);
                                    break;
                                case 3:
                                    suggestedLockDate = DateTime.Parse(dr["LockAppliedOn"].ToString()).AddMonths(1);
                                    break;
                                //case 4:
                                //    suggestedLockDate = DateTime.Parse(dr["LockAppliedOn"].ToString()).AddYears(1);
                                //    break;
                                default:
                                    suggestedLockDate = DateTimeConstants.MinValue;// DateTime.Parse(dr["LockAppliedOn"].ToString()).AddHours(24);
                                    break;
                            }
                        }
                        else
                        {
                            suggestedLockDate = DateTimeConstants.MinValue;
                        }
                    }
                    else
                    {
                        dr["Status"] = "Unlocked";
                    }

                }
                else
                {
                    dr["Status"] = "Unlocked";
                    suggestedLockDate = DateTimeConstants.MinValue;
                }
                if (suggestedLockDate > DateTimeConstants.MinValue)
                {
                    dr["SuggestedLockDate"] = suggestedLockDate.ToString("yyyy-MM-dd HH:mm");//.ToUniversalTime();
                }
                else
                {
                    dr["SuggestedLockDate"] = "N/A";
                }
                //dr["LockDate"] = (DateTime)dr["SuggestedLockDate"];
                dr["ActionID"] = 0;
                if (!string.IsNullOrEmpty(dr["LokedBy"].ToString()))
                {
                    int lockUserID = Convert.ToInt32(dr["LokedBy"].ToString());
                    dr["LockedByUserName"] = CachedDataManager.GetInstance.GetUserText(lockUserID);
                }
                if (!string.IsNullOrEmpty(dr["UnLokedBy"].ToString()))
                {
                    int unLockUserID = Convert.ToInt32(dr["UnLokedBy"].ToString());
                    dr["UnlockedByUserName"] = CachedDataManager.GetInstance.GetUserText(unLockUserID);
                }
                dr.EndEdit();
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
        }

        /// <summary>
        /// Create table of the records for saving
        /// </summary>
        /// <param name="status">True for Lock, false for unlock</param>
        /// <returns>Datatbale of the locked or unlocked data</returns>
        private static DataTable CreateLockTable(bool status)
        {

            DataTable dtLock = new DataTable("dtLock");
            dtLock.Columns.Add("CompanyID", typeof(int));
            dtLock.Columns.Add("CompanyFundID", typeof(int));
            dtLock.Columns.Add("UserID", typeof(int));
            dtLock.Columns.Add("ActionID", typeof(int));
            dtLock.Columns.Add("ExecutionTime", typeof(string));
            dtLock.Columns.Add("ActualActionDate", typeof(string));
            dtLock.Columns.Add("LockedAuditTrailId", typeof(int));
            //dtLock.Columns.Add("PreviousLockDate", typeof(string));

            try
            {
                if (status == true)
                {
                    foreach (DataRow dr in dtNavLock.Rows)
                    {
                        if (Convert.ToInt32(dr["ActionID"]) == 1 && dr["ActionID"] != DBNull.Value)
                        {
                            int companyID = (int)dr["CompanyID"];
                            int accountID = (int)dr["CompanyFundID"];
                            int actionID = (int)dr["ActionID"];
                            int userID = (int)dr["LokedBy"];
                            DateTime actionDate = ((DateTime)dr["LockAppliedOn"]);
                            DateTime execDate = ((DateTime)(DateTime)dr["LokedOn"]);

                            //Modified by omshiv, 25 july, Added LockedAuditTrailId in datatable, 
                            //LockedAuditTrailId is using for update last locked record.
                            int LockedAuditTrailId = 0;
                            if (dr["LockedAuditTrailId"] != DBNull.Value)
                            {
                                LockedAuditTrailId = (int)dr["LockedAuditTrailId"];
                            }
                            dtLock.Rows.Add(companyID, accountID, userID, actionID, execDate, actionDate, LockedAuditTrailId);
                        }
                        //dtLock.Rows.Add(dr["CompanyID"], dr["CompanyFundID"],
                        //                dr["LokedBy"], dr["ActionID"],
                        //                ((DateTime)dr["LokedOn"]).ToUniversalTime(),
                        //                ((DateTime)dr["LockAppliedOn"]).ToUniversalTime());

                        ExtendDataTable(dr);
                    }
                    return dtLock;
                }
                else if (status == false)
                {
                    foreach (DataRow dr in dtNavLock.Rows)
                    {
                        if (Convert.ToInt32(dr["ActionID"]) == 2 && dr["ActionID"] != DBNull.Value)
                        {
                            dtLock.Rows.Add(dr["CompanyID"], dr["CompanyFundID"],
                                            dr["UnLokedBy"], dr["ActionID"],
                                            dr["UnLokedOn"], dr["UnLokedOn"], dr["LockedAuditTrailId"]);//,dr["LokedOn"]);

                        }
                    }
                    return dtLock;
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
            return null;
        }

        /// <summary>
        /// Save the lock details in the database 
        /// </summary>
        /// <returns>affected records in the db</returns>
        public static int SaveRecords(bool status)
        {
            int i = 0;
            DataSet dsNavLock = new DataSet("dsNavLock");
            try
            {
                dsNavLock.Tables.Add(CreateLockTable(status));
                string xmlLock = dsNavLock.GetXml();
                i = NAVLockDAL.SaveLockRecords(xmlLock);
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
            //ExtendDataTable();
            return i;
        }

        /// <summary>
        /// Get the new last lock date for the account
        /// </summary>
        /// <param name="dtLastLockDate">current lock date</param>
        /// <param name="accountID">ID of the account</param>
        /// <returns>string form of the new last lock date</returns>
        public static Tuple<string, int> GetNewLastLockdate(DateTime dtLastLockDate, int accountID)
        {
            return NAVLockDAL.GetLastLockDate(dtLastLockDate, accountID);
        }

        /// <summary>
        /// Update the lock schedule of the account when the changes are done in the account details from account setup
        /// </summary>
        /// <param name="accountID">ID of the account</param>
        /// <param name="lockScheduleID">new lock schedule</param>
        public static void UpdateAccountLockSchedule(Dictionary<int, int> dictSchedules)
        {
            try
            {
                foreach (int accountID in dictSchedules.Keys)
                {
                    int lockScheduleID = dictSchedules[accountID];
                    if (lockScheduleID >= 0)
                    {
                        foreach (DataRow dr in dtNavLock.Rows)
                        {
                            if (Convert.ToInt32(dr["CompanyFundID"]) == accountID)
                            {
                                dr["PostingLockScheduleID"] = lockScheduleID;
                                dr["scheduleName"] = ((ScheduleType)lockScheduleID).ToString();
                                ExtendDataTable(dr);
                            }
                        }
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
        }

        /// <summary>
        /// Get lock dates of accounts so thatg they can be updated in the account setup
        /// </summary>
        /// <returns>The dictionary holding the accountID-lockdate</returns>
        public static Dictionary<int, string> GetLockDatesForAccounts()
        {
            Dictionary<int, string> dictAccountLockDates = new Dictionary<int, string>();
            try
            {
                if (dtNavLock != null && dtNavLock.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNavLock.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["CompanyFundID"].ToString()))// && !string.IsNullOrEmpty(dr["LockAppliedOn"].ToString()))
                        {
                            int accountID = Convert.ToInt32(dr["CompanyFundID"]);
                            string lockDate = "N/A";
                            if (!string.IsNullOrEmpty(dr["LockAppliedOn"].ToString()))
                            {
                                lockDate = DateTime.Parse(dr["LockAppliedOn"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            }
                            if (!dictAccountLockDates.ContainsKey(accountID))
                            {
                                dictAccountLockDates.Add(accountID, lockDate);
                            }
                        }
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
            return dictAccountLockDates;
        }
    }
}
