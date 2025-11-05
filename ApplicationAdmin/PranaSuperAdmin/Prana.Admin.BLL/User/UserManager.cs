#region Using

using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
#endregion

//// Supply the COM+ application name.
//[assembly: ApplicationName("PranaAdmin")]
//// Supply a strong-named assembly.
//[assembly: AssemblyKeyFileAttribute("PranaAdmin.snk")]
namespace Prana.Admin.BLL
{
    /// <summary>
    /// UserManager would be a static class to handel <see cref="User"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    [Transaction(TransactionOption.Required)]
    public class UserManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private UserManager()
        {
        }

        #endregion

        #region Basec methods like Add/Update/Delete/Get

        public static User FillUsers(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int lastName = 1 + offSet;
            int firstName = 2 + offSet;
            int shortName = 3 + offSet;
            int title = 4 + offSet;
            int eMail = 5 + offSet;
            int telphoneWork = 6 + offSet;
            int telphoneHome = 7 + offSet;
            int telphoneMobile = 8 + offSet;
            int fax = 9 + offSet;
            int login = 10 + offSet;
            int password = 11 + offSet;
            int telphonePager = 12 + offSet;
            int address1 = 13 + offSet;
            int address2 = 14 + offSet;
            int countryID = 15 + offSet;
            int stateID = 16 + offSet;
            int zip = 17 + offSet;
            int city = 18 + offSet;
            int superUser = 19 + offSet;

            User user = new User();
            if (row[ID] != System.DBNull.Value)
            {
                user.UserID = int.Parse(row[ID].ToString());
            }
            if (row[lastName] != System.DBNull.Value)
            {
                user.LastName = row[lastName].ToString();
            }
            if (row[firstName] != System.DBNull.Value)
            {
                user.FirstName = row[firstName].ToString();
            }
            if (row[shortName] != System.DBNull.Value)
            {
                user.ShortName = row[shortName].ToString();
            }
            if (row[title] != System.DBNull.Value)
            {
                user.Title = row[title].ToString();
            }
            if (row[eMail] != System.DBNull.Value)
            {
                user.EMail = row[eMail].ToString();
            }
            if (row[telphoneWork] != System.DBNull.Value)
            {
                user.TelephoneWork = row[telphoneWork].ToString();
            }
            if (row[telphoneHome] != System.DBNull.Value)
            {
                user.TelephoneHome = row[telphoneHome].ToString();
            }
            if (row[telphoneMobile] != System.DBNull.Value)
            {
                user.TelephoneMobile = row[telphoneMobile].ToString();
            }
            if (row[fax] != System.DBNull.Value)
            {
                user.Fax = row[fax].ToString();
            }
            if (row[login] != System.DBNull.Value)
            {
                user.LoginName = row[login].ToString();
            }
            if (row[password] != System.DBNull.Value)
            {
                user.Password = row[password].ToString();
            }
            if (row[telphonePager] != System.DBNull.Value)
            {
                user.TelephonePager = row[telphonePager].ToString();
            }
            if (row[address1] != System.DBNull.Value)
            {
                user.Address1 = row[address1].ToString();
            }
            if (row[address2] != System.DBNull.Value)
            {
                user.Address2 = row[address2].ToString();
            }
            if (row[countryID] != System.DBNull.Value)
            {
                user.CountryID = int.Parse(row[countryID].ToString());
            }
            if (row[stateID] != System.DBNull.Value)
            {
                user.StateID = int.Parse(row[stateID].ToString());
            }
            if (row[zip] != System.DBNull.Value)
            {
                user.Zip = row[zip].ToString();
            }
            if (row[city] != System.DBNull.Value)
            {
                user.City = row[city].ToString();
            }
            if (row[superUser] != System.DBNull.Value)
            {
                user.SuperUser = int.Parse(row[superUser].ToString());
            }
            return user;
        }

        /// <summary>
        /// Gets all users' details.
        /// </summary>
        /// <returns>Object of <see cref="Users"/> collection.</returns>
        public static Users GetUsers()
        {
            Users users = new Users();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllUsers";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    users.Add(FillUsers(row, 0));
                }
            }
            return users;
        }

        public static Users GetUsers(int companyID)
        {
            Users users = new Users();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUsers", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    users.Add(FillCompanyUsers(row, 0));
                }
            }
            return users;
        }

        /// <summary>
        /// Function to get all company users
        /// </summary>
        /// <returns></returns>
        public static Users GetAllUsers()
        {
            Users users = new Users();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetUsers";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    users.Add(FillAllCompanyUsers(row, 0));
                }
            }
            return users;
        }


        /// <summary>
        /// Function to get all  users
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllAccountGroups()
        {


            Dictionary<int, string> accountGroups = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllFundGroups";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!accountGroups.ContainsKey(int.Parse(row[0].ToString())))
                        {
                            accountGroups.Add(int.Parse(row[0].ToString()), row[1].ToString());
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
            return accountGroups;

        }

        public static User GetCompanyUser(int userID)
        {
            User user = new User();

            object[] parameter = new object[1];
            parameter[0] = userID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUser", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    user = FillCompanyUsers(row, 0);
                }
            }
            return user;
        }

        /// <summary>
        /// Gets <see cref="User"/> details whose ID is passed.
        /// </summary>
        /// <param name="userID">UserID of the user to be fetched from Database.</param>
        /// <returns><see cref="Users"/> collection containing single user.</returns>
        public static User GetUser(int userID)
        {
            User user = new User();

            object[] parameter = new object[1];
            parameter[0] = userID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUser", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    user = FillUsers(row, 0);
                }
            }
            return user;
        }

        #endregion

        /// <summary>
        /// Validates user login.
        /// </summary>
        /// <param name="login">Login to be validated.</param>
        /// <param name="pwd">Password to be validated.</param>
        /// <returns>User Permissions.</returns>
        public static Permissions ValidateLogin(string login, string password)
        {
            //Validate Login
            //bool result = false;
            Permissions _permissions = null;
            object[] parameter = new object[2];

            try
            {
                parameter[0] = login.ToString();
                parameter[1] = password.ToString();

                int userID = (int)DatabaseManager.DatabaseManager.ExecuteScalar("P_ValidateLogin", parameter);

                if (userID > 0)
                {
                    //result = true;
                    //Put user details in Cache or Get user preferences and save in some file.
                    _permissions = PermissionManager.GetPermissions(userID);
                }
                //				else
                //				{
                //					result = false;
                //				}
                //throw new Exception("Test Message");				
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
            return _permissions;
        }

        /// <summary>
        /// insert user in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool AddUser(User user)
        {
            try
            {
                object[] parameter = new object[1];

                parameter[0] = user.FirstName;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_AddUser", parameter);

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
            return false;
        }

        /// <summary>
        /// Delete User.
        /// </summary>
        /// <param name="userID">user id of the user to be deleted.</param>
        /// <returns></returns>
        public static bool DeleteUser(int userID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            try
            {
                parameter[0] = userID;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUser", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
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
            return result;
        }

        public static int SaveUser(User user)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[20];

                parameter[0] = user.FirstName;
                parameter[1] = user.LastName;
                parameter[2] = user.ShortName;
                parameter[3] = user.Title;
                parameter[4] = user.EMail;
                parameter[5] = user.TelephoneWork;
                parameter[6] = user.TelephoneHome;
                parameter[7] = user.TelephoneMobile;
                parameter[8] = user.TelephonePager;
                parameter[9] = user.LoginName;
                parameter[10] = user.Password;
                parameter[11] = user.Address1;
                parameter[12] = user.Address2;
                parameter[13] = user.CountryID;
                parameter[14] = user.StateID;
                parameter[15] = user.Zip;
                parameter[16] = user.Fax;
                parameter[17] = user.UserID;
                parameter[18] = user.City;
                parameter[19] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserDetail", parameter).ToString());
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
            return result;

        }

        public static int SaveUsers(Users users)
        {
            int result = int.MinValue;
            foreach (User user in users)
            {
                try
                {
                    object[] parameter = new object[18];

                    parameter[0] = user.FirstName;
                    parameter[1] = user.LastName;
                    parameter[2] = user.ShortName;
                    parameter[3] = user.Title;
                    parameter[4] = user.EMail;
                    parameter[6] = user.TelephoneWork;
                    parameter[7] = user.TelephoneHome;
                    parameter[8] = user.TelephoneMobile;
                    parameter[9] = user.TelephonePager;
                    parameter[10] = user.LoginName;
                    parameter[11] = user.Password;
                    parameter[12] = user.Address1;
                    parameter[13] = user.Address2;
                    parameter[14] = user.Zip;
                    parameter[15] = user.Fax;
                    parameter[16] = user.UserID;
                    parameter[17] = int.MinValue;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserDetail", parameter).ToString());
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

            }
            return result;
        }

        public static void AddPermissions(int userID, Permissions permissions)
        {
            //TODO: Chaneg the method of inserting permissions.
            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUserPermission", parameter);

                parameter = new object[3];
                parameter[0] = userID;
                foreach (Permission permission in permissions)
                {
                    parameter[1] = permission.PermissionID;
                    parameter[2] = (permission.IsSelected == true ? 1 : 0);
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_AddUserPermission", parameter) < 0)
                    {
                        //ContextUtil.SetAbort();
                        return;
                    }
                }
                //ContextUtil.SetComplete();
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
        }

        /// <summary>
        /// Checks whether the userID provided is of Admin user or a non admin user. 
        /// </summary>
        /// <param name="userID">ID os user to be tested for Admin rights.</param>
        /// <returns>
        /// true - If Admin <see cref="User"/>.
        /// false - If a non admin <see cref="User"/>.
        /// </returns>
        public static bool IsAdmin(int userID)
        {
            bool isAdmin = false;

            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_IsAdmin", parameter))
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) > 0)
                        {
                            isAdmin = true;
                        }
                    }
                }
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
            return isAdmin;
        }

        #region Company User

        public static User FillCompanyUsers(object[] row, int offSet)
        {
            int userID = 0 + offSet;
            int lastName = 1 + offSet;
            int firstName = 2 + offSet;
            int shortName = 3 + offSet;
            int title = 4 + offSet;
            int eMail = 5 + offSet;
            int telphoneWork = 6 + offSet;
            int telphoneHome = 7 + offSet;
            int telphoneMobile = 8 + offSet;
            int fax = 9 + offSet;
            int login = 10 + offSet;
            int password = 11 + offSet;
            int telphonePager = 12 + offSet;
            int address1 = 13 + offSet;
            int address2 = 14 + offSet;
            int countryID = 15 + offSet;
            int stateID = 16 + offSet;
            int zip = 17 + offSet;
            int companyID = 18 + offSet;
            int tradingPermission = 19 + offSet;
            int city = 20 + offSet;
            int factSetUsernameAndSerialNumber = 21 + offSet;
            int isFactSetSupportUser = 22 + offSet;
            int marketDataAccessIPAddresses = 23 + offSet;
            int activUsername = 24 + offSet;
            int activPassword = 25 + offSet;
            int samsaraAzureId = 26 + offSet;
            int sapiUsername = 27 + offSet;
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
                if (row[shortName] != System.DBNull.Value)
                {
                    user.ShortName = row[shortName].ToString();
                }
                if (row[title] != System.DBNull.Value)
                {
                    user.Title = row[title].ToString();
                }
                if (row[eMail] != System.DBNull.Value)
                {
                    user.EMail = row[eMail].ToString();
                }
                if (row[telphoneWork] != System.DBNull.Value)
                {
                    user.TelephoneWork = row[telphoneWork].ToString();
                }
                if (row[telphoneHome] != System.DBNull.Value)
                {
                    user.TelephoneHome = row[telphoneHome].ToString();
                }
                if (row[telphoneMobile] != System.DBNull.Value)
                {
                    user.TelephoneMobile = row[telphoneMobile].ToString();
                }
                if (row[fax] != System.DBNull.Value)
                {
                    user.Fax = row[fax].ToString();
                }
                if (row[login] != System.DBNull.Value)
                {
                    user.LoginName = row[login].ToString();
                }
                if (row[password] != System.DBNull.Value)
                {
                    user.Password = row[password].ToString();
                }
                if (row[telphonePager] != System.DBNull.Value)
                {
                    user.TelephonePager = row[telphonePager].ToString();
                }
                if (row[address1] != System.DBNull.Value)
                {
                    user.Address1 = row[address1].ToString();
                }
                if (row[address2] != System.DBNull.Value)
                {
                    user.Address2 = row[address2].ToString();
                }
                if (row[countryID] != DBNull.Value)
                {
                    user.CountryID = int.Parse(row[countryID].ToString());
                }
                if (row[stateID] != DBNull.Value)
                {
                    user.StateID = int.Parse(row[stateID].ToString());
                }
                if (row[zip] != System.DBNull.Value)
                {
                    user.Zip = row[zip].ToString();
                }
                if (row[companyID] != System.DBNull.Value)
                {
                    user.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[tradingPermission] != System.DBNull.Value)
                {
                    user.TradingPermission = int.Parse(row[tradingPermission].ToString());
                }
                if (row[city] != System.DBNull.Value)
                {
                    user.City = row[city].ToString();
                }
                if (row[factSetUsernameAndSerialNumber] != System.DBNull.Value)
                {
                    user.FactSetUsernameAndSerialNumber = row[factSetUsernameAndSerialNumber].ToString();
                }
                if (row[isFactSetSupportUser] != System.DBNull.Value)
                {
                    user.IsFactSetSupportUser = (bool)row[isFactSetSupportUser];
                }
                if (row[marketDataAccessIPAddresses] != System.DBNull.Value)
                {
                    user.MarketDataAccessIPAddresses = row[marketDataAccessIPAddresses].ToString();
                }
                if (row[activUsername] != System.DBNull.Value)
                {
                    user.ActivUsername = row[activUsername].ToString();
                }
                if (row[activPassword] != System.DBNull.Value)
                {
                    user.ActivPassword = row[activPassword].ToString();
                }
                if (row[samsaraAzureId] != System.DBNull.Value)
                {
                    user.SamsaraAzureId = row[samsaraAzureId].ToString();
                }
                if (row[sapiUsername] != System.DBNull.Value)
                {
                    user.SapiUsername = row[sapiUsername].ToString();
                }
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
            return user;
        }

        /// <summary>
        ///  To Fill company user details
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static User FillAllCompanyUsers(object[] row, int offSet)
        {
            int userID = 0 + offSet;
            int lastName = 1 + offSet;
            int firstName = 2 + offSet;
            int shortName = 3 + offSet;
            int title = 4 + offSet;
            int eMail = 5 + offSet;
            int telphoneWork = 6 + offSet;
            int telphoneHome = 7 + offSet;
            int telphoneMobile = 8 + offSet;
            int fax = 9 + offSet;
            int login = 10 + offSet;
            int password = 11 + offSet;
            int telphonePager = 12 + offSet;
            int address1 = 13 + offSet;
            int address2 = 14 + offSet;
            int countryID = 15 + offSet;
            int stateID = 16 + offSet;
            int zip = 17 + offSet;
            int companyID = 18 + offSet;
            int tradingPermission = 19 + offSet;
            int city = 20 + offSet;
            int region = 21 + offSet;
            int roleID = 22 + offSet;
            int isAllGroupsAccess = 23 + offSet;
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
                if (row[shortName] != System.DBNull.Value)
                {
                    user.ShortName = row[shortName].ToString();
                }
                if (row[title] != System.DBNull.Value)
                {
                    user.Title = row[title].ToString();
                }
                if (row[eMail] != System.DBNull.Value)
                {
                    user.EMail = row[eMail].ToString();
                }
                if (row[telphoneWork] != System.DBNull.Value)
                {
                    user.TelephoneWork = row[telphoneWork].ToString();
                }
                if (row[telphoneHome] != System.DBNull.Value)
                {
                    user.TelephoneHome = row[telphoneHome].ToString();
                }
                if (row[telphoneMobile] != System.DBNull.Value)
                {
                    user.TelephoneMobile = row[telphoneMobile].ToString();
                }
                if (row[fax] != System.DBNull.Value)
                {
                    user.Fax = row[fax].ToString();
                }
                if (row[login] != System.DBNull.Value)
                {
                    user.LoginName = row[login].ToString();
                }
                if (row[password] != System.DBNull.Value)
                {
                    user.Password = row[password].ToString();
                }
                if (row[telphonePager] != System.DBNull.Value)
                {
                    user.TelephonePager = row[telphonePager].ToString();
                }
                if (row[address1] != System.DBNull.Value)
                {
                    user.Address1 = row[address1].ToString();
                }
                if (row[address2] != System.DBNull.Value)
                {
                    user.Address2 = row[address2].ToString();
                }
                if (row[countryID] != DBNull.Value)
                {
                    user.CountryID = int.Parse(row[countryID].ToString());
                }
                if (row[stateID] != DBNull.Value)
                {
                    user.StateID = int.Parse(row[stateID].ToString());
                }
                if (row[zip] != System.DBNull.Value)
                {
                    user.Zip = row[zip].ToString();
                }
                if (row[companyID] != System.DBNull.Value)
                {
                    user.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[tradingPermission] != System.DBNull.Value)
                {
                    user.TradingPermission = int.Parse(row[tradingPermission].ToString());
                }
                if (row[city] != System.DBNull.Value)
                {
                    user.City = row[city].ToString();
                }
                if (row[region] != System.DBNull.Value)
                {
                    user.Region = row[region].ToString();
                }
                if (row[roleID] != System.DBNull.Value)
                {
                    user.RoleID = int.Parse(row[roleID].ToString());
                }
                if (row[isAllGroupsAccess] != System.DBNull.Value)
                {
                    user.IsAllGroupsAccess = Convert.ToBoolean(row[isAllGroupsAccess].ToString());
                }
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
            return user;
        }

        public static Users GetCompanyUsers()
        {
            Users companyUsers = new Users();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyUsers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUsers.Add(FillCompanyUsers(row, 0));
                    }
                }
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
            return companyUsers;
        }

        public static Users GetCompanyUserforRM(int companyID)
        {
            //ToDo: Implimentation required.
            Users companyUsers = new Users();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUsers", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    companyUsers.Add(FillCompanyUsers(row, 0));
                }
            }
            return companyUsers;
        }

        //Check if it required or not..
        public static User GetCompanyUserBoth(int companyID, int userID)
        {
            User companyUser = new User();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = userID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUsersBoth", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUser = FillCompanyUsers(row, 0);
                    }
                }
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
            return companyUser;
        }


        public static bool DeleteCompanyUser(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            try
            {
                parameter[0] = companyID;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUsers", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
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
            return result;
        }

        /// <summary>
        /// Deletes the Company User
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="isDeletedForceFully"></param>
        /// <returns></returns>
        public static string DeleteCompanyUser(int companyUserID, bool isDeletedForceFully)
        {
            string result = string.Empty;
            Object[] parameter = new object[2];
            try
            {
                parameter[0] = companyUserID;
                parameter[1] = isDeletedForceFully;

                var login = DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCompanyUserByID", parameter);
                if (login != null)
                {
                    result = login.ToString();
                }
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
            return result;
        }

        /// <summary>
        /// To delete group user from T_AccountGroups
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static bool DeleteSelectedGroup(int groupID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            try
            {
                parameter[0] = groupID;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAccountGroup", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
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
            return result;
        }

        /// <summary>
        /// Saves Company User Details to database
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int SaveCompanyUser(int companyID, User user, ref string userName)
        {
            //TODO: Write SP
            int result = int.MinValue;
            object[] parameter = new object[27];
            try
            {
                parameter[0] = user.UserID;
                parameter[1] = user.LastName;
                parameter[2] = user.FirstName;
                parameter[3] = user.ShortName;
                parameter[4] = user.Title;
                parameter[5] = user.EMail;
                parameter[6] = user.FactSetUsernameAndSerialNumber;
                parameter[7] = user.TelephoneWork;
                parameter[8] = user.TelephoneHome;
                parameter[9] = user.TelephoneMobile;
                parameter[10] = user.Fax;
                parameter[11] = user.LoginName;
                parameter[12] = user.Password.Equals(ApplicationConstants.DUMMY_PASSWORD) ? string.Empty : PBKDF2Encryption.GetPBKDF2HashData(user.Password);
                parameter[13] = user.TelephonePager;
                parameter[14] = user.Address1;
                parameter[15] = user.Address2;
                parameter[16] = user.CountryID;
                parameter[17] = user.StateID;
                parameter[18] = user.Zip;
                parameter[19] = companyID;
                parameter[20] = user.TradingPermission;
                parameter[21] = user.City;
                parameter[22] = user.IsFactSetSupportUser;
                parameter[23] = user.ActivUsername;
                parameter[24] = user.ActivPassword;
                parameter[25] = user.SamsaraAzureId;
                parameter[26] = user.SapiUsername;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_SaveCompanyUser", parameter))
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            result = Convert.ToInt32(reader.GetValue(0).ToString());

                            if (result > 0 && reader.GetValue(1) != DBNull.Value)
                            {
                                userName = reader.GetValue(1).ToString();
                            }
                        }
                    }
                }
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
            return result;
        }

        public static int CheckMarketDataProviderAccessIDDuplication(string userLoginName, string accessID)
        {
            object[] parameter = new object[2];
            try
            {
                parameter[0] = userLoginName;
                parameter[1] = accessID;

                DataSet ds = null;

                if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.FactSet)
                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CheckFactSetUsernameAndSerialNumberDuplication", parameter);
                else if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.ACTIV)
                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CheckActivUsernameDuplication", parameter);
                else if (Prana.CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI)
                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CheckSapiUsernameDuplication", parameter);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["Occurance"]);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return 1;
            }
            return 0;
        }
        #endregion


        /// <summary>
        /// The method is used to fetch all the Users for a particular TradingAccount. 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static Users GetTradingAccntUsers(int tradingAccntID)
        {
            Users users = new Users();
            try
            {
                object[] parameter = new object[1];

                parameter[0] = tradingAccntID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUsersForTradingAccount", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        users.Add(FillCompanyUsers(row, 0));
                    }
                }
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
            return users;
        }

        /// <summary>
        /// The method is used to check if a user already exists with same login or short Name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckCompanyUser(int userID, string shortName, ref string loginName)
        {
            bool isDuplicateUser = true;
            object[] parameter = new object[3];
            try
            {
                parameter[0] = userID;
                parameter[1] = shortName;
                parameter[2] = loginName;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_CheckCompanyUser", parameter))
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            isDuplicateUser = Convert.ToInt32(reader.GetValue(0).ToString()) > 0;

                            if (!isDuplicateUser && reader.GetValue(1) != DBNull.Value)
                            {
                                loginName = reader.GetValue(1).ToString();
                            }
                        }
                    }
                }
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
            return isDuplicateUser;
        }
    }
}
