using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description of RiskTradingAccountManager.
    /// </summary>
    public class RiskTradingAccountManager
    {
        #region Constructors

        public RiskTradingAccountManager()
        {

        }

        #endregion Constructors

        /*TRADING ACCOUNTS OVERALLLIMITS*/

        #region Basics methods For RM TradingAccounts

        //Fill Trading Account OverallLimits

        /// <summary>
        ///  FillRMTradingAccnt is a method to fill a object of <see cref="RMTradingAccount"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="RMTradingAccount"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="RMTradingAccount"/> class.</returns>
        /// And, this row contains all values of RMTradingAccount class. So, if the row only contains result from T_RMTradingAccount table 
        /// then value of Offset would be zero ("0"), and, lets say the row contains value of RMTradingAccount as well as any other table then
        /// we have to specify the offset from where the values of RMTradingAccount starts. 
        /// Note: Sequence of RMTradingAccount class whould be always same as in this method.</remarks>
        public static RMTradingAccount FillRMTradingAccount(object[] row, int offSet)
        {
            int companyTradingAccntRMID = 0 + offSet;
            int companyID = 1 + offSet;
            int companyTradAccntID = 2 + offSet;
            int tAExposureLimit = 3 + offSet;
            int tAPositivePNL = 4 + offSet;
            int tANegativePNL = 5 + offSet;

            RMTradingAccount rMTradingAccount = new RMTradingAccount();
            try
            {
                if (row[companyTradingAccntRMID] != null)
                {
                    rMTradingAccount.CompanyTradAccntRMID = int.Parse(row[companyTradingAccntRMID].ToString());
                }
                if (row[companyID] != null)
                {
                    rMTradingAccount.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[companyTradAccntID] != null)
                {
                    rMTradingAccount.CompanyTradingAccountID = int.Parse(row[companyTradAccntID].ToString());
                }
                if (row[tAExposureLimit] != null)
                {
                    rMTradingAccount.ExposureLimit = int.Parse(row[tAExposureLimit].ToString());
                }
                if (row[tAPositivePNL] != null)
                {
                    rMTradingAccount.PositivePNL = int.Parse(row[tAPositivePNL].ToString());
                }
                if (row[tANegativePNL] != null)
                {
                    rMTradingAccount.NegativePNL = int.Parse(row[tANegativePNL].ToString());
                }

            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return rMTradingAccount;

        }

        //Get a collection of RM Trading Acounts details 

        /// <summary>
        /// To get all the RMTradingAccounts' details for a companyID passed.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMTradingAccounts GetRMTradingAccounts(int companyID)
        {
            RMTradingAccounts rMTradingAccounts = new RMTradingAccounts();
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMTradingAccounts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMTradingAccounts.Add(FillRMTradingAccount(row, 0));
                    }
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
            return rMTradingAccounts;
        }

        //Get One RM Trading Account Detail

        /// <summary>
        /// This gets the details of one particular TRADINGACCOUNT of a company as per the CompanyID and TradingAccntID passed.
        /// </summary>
        /// <param name="compantID"></param>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static RMTradingAccount GetRMTradingAccount(int companyID, int tradingAccountID)
        {
            RMTradingAccount rMTradingAccount = new RMTradingAccount();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = tradingAccountID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMTradingAccount", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMTradingAccount = FillRMTradingAccount(row, 0);

                    }
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
            return rMTradingAccount;
        }

        //Save RM Trading Account

        /// <summary>
        /// the method is used to save the RMTrading Accnts details as input by user.
        /// </summary>
        /// <param name="rMTradingAccount"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveRMTradingAccnt(RMTradingAccount rMTradingAccount, int companyID)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[7];

                parameter[0] = rMTradingAccount.CompanyTradAccntRMID;
                parameter[1] = companyID;
                parameter[2] = rMTradingAccount.CompanyTradingAccountID;
                parameter[3] = rMTradingAccount.ExposureLimit;
                parameter[4] = rMTradingAccount.PositivePNL;
                parameter[5] = rMTradingAccount.NegativePNL;
                parameter[6] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveRMTradingAccount", parameter).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }

            return result;
        }

        //Delete a Single RMTradAccnt Details

        /// <summary>
        /// The method is used to delete a single row of data in RMTradingAccount.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="tradAccntID"></param>
        /// <returns></returns>
        public static bool DeleteRMTradingAccnt(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteRMTradingAccnt", parameter) > 0)
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

        //Delete all RMTradAccount for a companyID

        /// <summary>
        /// The method deletes all the RM trad accnt details for a company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteAllRMTradingAccnts(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllRMTradingAccnts", parameter) > 0)
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

        #endregion Basics methods For RM TradingAccounts

        /*TRADING ACCOUNT'S USER DETAILS*/

        #region TradingAccount User

        //Fill UserTrading Accnt

        /// <summary>
        /// FillRMTradingAccnt is a method to fill a object of <see cref="UserTradingAccount"/> class.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static UserTradingAccount FillUserTradingAccount(object[] row, int offSet)
        {
            int rMUserTradingAccntID = 0 + offSet;
            int companyID = 1 + offSet;
            int companyUserID = 2 + offSet;
            int userTradingAccntID = 3 + offSet;
            int userTAExpLt = 4 + offSet;

            UserTradingAccount userTradingAccount = new UserTradingAccount();
            try
            {
                if (row[rMUserTradingAccntID] != null)
                {
                    userTradingAccount.RMUserTradingAccntID = int.Parse(row[rMUserTradingAccntID].ToString());
                }
                if (row[companyID] != null)
                {
                    userTradingAccount.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[companyUserID] != null)
                {
                    userTradingAccount.CompanyUserID = int.Parse(row[companyUserID].ToString());
                }
                if (row[userTradingAccntID] != null)
                {
                    userTradingAccount.UserTradingAccntID = int.Parse(row[userTradingAccntID].ToString());
                }
                if (row[userTAExpLt] != null)
                {
                    userTradingAccount.UserTAExposureLimit = int.Parse(row[userTAExpLt].ToString());
                }


            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userTradingAccount;
        }

        //Get collection of all RMUserTradingAccnts

        /// <summary>
        /// The method is used to fetch the entire collection of UserTradingAccounts for a particular TradingAccntID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userTradingAccountID"></param>
        /// <returns></returns>
        public static UserTradingAccounts GetAllRMUserTradingAccounts(int companyID, int userTradingAccountID)
        {
            UserTradingAccounts userTradingAccounts = new UserTradingAccounts();
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = userTradingAccountID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMUserTradingAccounts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userTradingAccounts.Add(FillUserTradingAccount(row, 0));
                    }
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
            return userTradingAccounts;
        }

        //Get a single UserTradingAccnt

        /// <summary>
        /// the method is used to fetch the details of a single trading accnt User 
        /// </summary>
        /// <param name="tradingAccountID"></param>
        /// <param name="tradAccntUserID"></param>
        /// <returns></returns>
        public static UserTradingAccount GetRMUserTradingAccount(int tradingAccountID, int tradAccntUserID)
        {
            UserTradingAccount userTradingAccount = new UserTradingAccount();

            object[] parameter = new object[2];
            parameter[0] = tradingAccountID;
            parameter[1] = tradAccntUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMUserTradingAccount", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userTradingAccount = FillUserTradingAccount(row, 0);

                    }
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
            return userTradingAccount;
        }

        //Save UserTradingAccount

        /// <summary>
        /// The method is used to save details of a RM tradingaccntUser
        /// </summary>
        /// <param name="userTradingAccount"></param>
        /// <param name="companyID"></param>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static int SaveUserTradingAccount(UserTradingAccount userTradingAccount, int companyID, int tradingAccntID)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[6];

                parameter[0] = userTradingAccount.RMUserTradingAccntID;
                parameter[1] = companyID;
                parameter[2] = tradingAccntID;
                parameter[3] = userTradingAccount.CompanyUserID;
                parameter[4] = userTradingAccount.UserTAExposureLimit;
                parameter[5] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveRMUserTradingAccount", parameter).ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }

            return result;
        }

        //Delete a particular TradAccntUser RM

        /// <summary>
        /// The method is used to delete the RM data for a particular Trad Accnt user.
        /// </summary>
        /// <param name="tradAccntID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DeleteUserTradingAccount(int tradAccntID, int userID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = tradAccntID;
            parameter[1] = userID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUserTradingAccount", parameter) > 0)
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

        //delete All users of Trad Accnt

        /// <summary>
        /// the method is used to delete all the Trad accnt users details for RM.
        /// </summary>
        /// <param name="tradAccntID"></param>
        /// <returns></returns>
        public static bool DeleteAllUserTradingAccnt(int tradAccntID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = tradAccntID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllUserTradingAccnt", parameter) > 0)
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
        /// the method is used to fetch all the userTradingAccounts data for a companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static UserTradingAccounts GetRMUserTradingAccounts(int companyID)
        {

            UserTradingAccounts userTradingAccounts = new UserTradingAccounts();
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCompanyRMUserTradingAccounts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userTradingAccounts.Add(FillUserTradingAccount(row, 0));
                    }
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
            return userTradingAccounts;

        }

        #endregion TradingAccount User





    }
}
