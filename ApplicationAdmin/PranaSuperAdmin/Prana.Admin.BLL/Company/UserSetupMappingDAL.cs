using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Class to save Usersetup details and query from database 
    /// </summary>
    public class UserSetupMappingDAL
    {

        /// <summary>
        /// Load all groups from Data base and add into _groupCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadGroupFromDb()
        {
            Dictionary<int, string> groupCollection = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllGroupNames";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        groupCollection.Add(dr.GetInt32(0), dr.GetString(1));
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
            return groupCollection;
        }

        /// <summary>
        /// Load all Mapping or releationship  for user & group and add into _groupCompanyUserMapping dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadGroupUserMappingFromDb(int UserID)
        {
            Dictionary<int, List<int>> userGroupMapping = new Dictionary<int, List<int>>();
            try
            {
                object[] param = { UserID };
                string sProc = "P_CompanyUserGroupAssociation";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int userId = Convert.ToInt32(dr[0]);
                        int groupId = Convert.ToInt32(dr[1]);
                        if (userGroupMapping.ContainsKey(userId))
                            userGroupMapping[userId].Add(groupId);
                        else
                        {
                            List<int> groupCollection = new List<int>();
                            groupCollection.Add(groupId);
                            userGroupMapping.Add(userId, groupCollection);
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
            return userGroupMapping;
        }

        /// <summary>
        /// save xml document ot data base using stored procedure  name P_SaveGroupCompanyUserMapping
        /// </summary>
        /// <param name="ds">passed xml document as parameter </param>
        internal static int SaveDataSetInDb(String xmlDataTable, User user, string xmlTrading)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveGroupCompanyUserMapping";
                object[] parameter = { xmlDataTable,
                                       xmlTrading,
                                       user.UserID,
                                       string.IsNullOrEmpty(user.FirstName)?string.Empty:user.FirstName,
                                       string.IsNullOrEmpty(user.LastName)?string.Empty:user.LastName,
                                       string.IsNullOrEmpty(user.LoginName)?string.Empty:user.LoginName,
                                       string.IsNullOrEmpty(user.Password)?string.Empty:user.Password,
                                       string.IsNullOrEmpty(user.ShortName)?string.Empty:user.ShortName,
                                       string.IsNullOrEmpty(user.EMail)?string.Empty:user.EMail,
                                       user.CompanyID,
                                       string.IsNullOrEmpty(user.Region)?string.Empty:user.Region,
                                       user.RoleID,
                                       user.IsAllGroupsAccess,
                                       "CouldNotSaveMapping",
                                       -1 };

                object ErrorNumber = DatabaseManager.DatabaseManager.ExecuteScalar(sProc, parameter);
                if (ErrorNumber != null)
                    i = ErrorNumber == null ? 0 : (int)ErrorNumber;
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

        /// <summary>
        /// Function for getting user details
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User GetCompanyUserDetails(int userID)
        {
            User companyUser = new User();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUser = FillCompanyUsers(row, 0);
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
            return companyUser;
        }

        /// <summary>
        /// To fill company user details to User entity
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static User FillCompanyUsers(object[] row, int offSet)
        {
            int userID = 0 + offSet;
            int lastName = 1 + offSet;
            int firstName = 2 + offSet;
            int eMail = 3 + offSet;
            int login = 4 + offSet;
            int password = 5 + offSet;
            int companyID = 6 + offSet;
            int region = 7 + offSet;
            int roleID = 8 + offSet;
            int isAllGroupAccess = 9 + offSet;
            int shortName = 10 + offSet;
            User user = new User();

            try
            {
                if (row[userID] != System.DBNull.Value)
                {
                    user.UserID = int.Parse(row[userID].ToString());
                }
                if (row[lastName] != System.DBNull.Value)
                {
                    user.LastName = row[lastName].ToString();
                }
                if (row[firstName] != System.DBNull.Value)
                {
                    user.FirstName = row[firstName].ToString();
                }
                if (row[eMail] != System.DBNull.Value)
                {
                    user.EMail = row[eMail].ToString();
                }
                if (row[login] != System.DBNull.Value)
                {
                    user.LoginName = row[login].ToString();
                }
                if (row[password] != System.DBNull.Value)
                {
                    user.Password = row[password].ToString();
                }
                if (row[companyID] != System.DBNull.Value)
                {
                    user.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[region] != System.DBNull.Value)
                {
                    user.Region = row[region].ToString();
                }
                if (row[roleID] != System.DBNull.Value)
                {
                    user.RoleID = int.Parse(row[roleID].ToString());
                }
                if (row[isAllGroupAccess] != System.DBNull.Value)
                {
                    user.IsAllGroupsAccess = Convert.ToBoolean(row[isAllGroupAccess].ToString());
                }
                if (row[shortName] != System.DBNull.Value)
                {
                    user.ShortName = row[shortName].ToString();
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
            return user;
        }

        /// <summary>
        /// Returns max user ID
        /// </summary>
        /// <returns></returns>
        internal static int GetMaxUserID()
        {
            int userID = -1;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMaxUserID";

            try
            {
                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
                if (idValue != DBNull.Value)
                {
                    userID = Convert.ToInt32(idValue);
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
            return userID;
        }

        /// <summary>
        /// Load all trading accout and user mapping from Data base and add into dictTradingAccountUserMapping dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadTradingAccountUserMappingFromDb(int UserID)
        {
            Dictionary<int, string> dictTradingAccountUserMapping = new Dictionary<int, string>();
            try
            {
                object[] param = { UserID };
                string sProc = "P_GetTradingAccountsForUser";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        if (!dictTradingAccountUserMapping.ContainsKey(dr.GetInt32(0)))
                            dictTradingAccountUserMapping.Add(dr.GetInt32(0), dr.GetString(1));
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
            return dictTradingAccountUserMapping;
        }
    }
}
