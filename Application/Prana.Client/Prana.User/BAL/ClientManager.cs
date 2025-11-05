#region Using
using Prana.LogManager;
using System;
using System.Data;
using System.EnterpriseServices;
#endregion

//// Supply the COM+ application name.
//[assembly: ApplicationName("PranaAdmin")]
//// Supply a strong-named assembly.
//[assembly: AssemblyKeyFileAttribute("PranaAdmin.snk")]
namespace Prana.User.BAL
{
    /// <summary>
    /// UserManager would be a static class to handel <see cref="User"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    [Transaction(TransactionOption.Required)]
    public class ClientManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private ClientManager()
        {
        }

        #endregion

        /// <summary>
        /// Get Company User
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static ClientUser GetCompanyUser(int userID)
        {
            ClientUser user = new ClientUser();

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


        #region Company User

        /// <summary>
        /// Fill Company Users
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static ClientUser FillCompanyUsers(object[] row, int offSet)
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
            ClientUser user = new ClientUser();

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
        /// Save User Profile
        /// </summary>
        /// <param name="user"></param>
        public static int SaveUserProfile(ClientUser user)
        {
            int result = int.MinValue;
            object[] parameter = new object[11];
            try
            {
                parameter[0] = user.UserID;
                parameter[1] = user.FirstName;
                parameter[2] = user.LastName;
                parameter[3] = user.EMail;
                parameter[4] = user.Address1;
                parameter[5] = user.Address2;
                parameter[6] = user.CountryID;
                parameter[7] = user.StateID;
                parameter[8] = user.Zip;
                parameter[9] = user.TelephoneWork;
                parameter[10] = user.TelephoneMobile;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserProfile", parameter).ToString());
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
        /// Save User Password
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        public static int SaveUserPassword(int userid, string password)
        {
            int result = int.MinValue;
            object[] parameter = new object[2];
            try
            {
                parameter[0] = userid;
                parameter[1] = password;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserPassword", parameter).ToString());
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

        #endregion
    }
}