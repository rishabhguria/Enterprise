using Prana.Authentication.Common;
using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Data;


namespace Prana.Auth.Authentication.DAL
{
    internal static class NirvanaAuthenticationDataManager
    {
        /// <summary>
        /// Validates User
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <param name="identity"></param>
        internal static void ValidateUser(string loginId, string password, out NirvanaIdentity identity)
        {
            // Permissions _permissions = null;
            identity = new NirvanaIdentity();
            try
            {
                object[] parameter = new object[2];

                int userID = 0;
                string hash = string.Empty;
                parameter[0] = loginId.ToString();
                parameter[1] = string.Empty;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUserHash", parameter))
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            userID = Convert.ToInt32(reader.GetValue(0).ToString());
                        }
                        if (reader.GetValue(1) != DBNull.Value)
                        {
                            hash = reader.GetValue(1).ToString();
                        }
                    }
                }
                if (userID > 0 && PBKDF2Encryption.VerifyPassword(password, hash))
                {
                    NirvanaRoles userRole = NirvanaRoles.User;
                    int companyID = 0;
                    object[] parameters = new object[1];
                    parameters[0] = userID;
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserRole", parameters))
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(0) != DBNull.Value)
                            {
                                userRole = (NirvanaRoles)Convert.ToInt32(reader.GetValue(0).ToString()); ;
                            }
                            if (reader.GetValue(1) != DBNull.Value)
                            {
                                companyID = Convert.ToInt32(reader.GetValue(1).ToString()); ;
                            }
                        }

                    }

                    identity.IsAuthenticated = true;
                    identity.Name = loginId;
                    identity.UserId = userID;
                    identity.Role = userRole;
                    identity.CompanyID = companyID;
                    //Put user details in Cache or Get user preferences and save in some file.
                    //  _permissions = PermissionManager.GetPermissions(userID);
                }
                else
                {
                    identity.IsAuthenticated = false;
                    identity.Name = loginId;
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



        }
    }
}
