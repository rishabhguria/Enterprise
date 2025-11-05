#region Using

using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientAccountManager.
    /// </summary>
    public class ClientAccountManager
    {
        private ClientAccountManager()
        {
        }

        private static ClientAccount FillClientAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ClientAccount clientAccount = null;

            try
            {
                if (row != null)
                {
                    clientAccount = new ClientAccount();

                    int COMPANYCLIENTACCOUNTID = offset + 0;
                    int COMPANYCLIENTACCOUNTNAME = offset + 1;
                    int COMPANYCLIENTACCOUNTSHORTNAME = offset + 2;
                    int COMPANYCLIENTID = offset + 3;

                    clientAccount.CompanyClientAccountID = Convert.ToInt32(row[COMPANYCLIENTACCOUNTID]);
                    clientAccount.CompanyClientAccountName = Convert.ToString(row[COMPANYCLIENTACCOUNTNAME]);
                    clientAccount.CompanyClientAccountShortName = Convert.ToString(row[COMPANYCLIENTACCOUNTSHORTNAME]);
                    clientAccount.CompanyClientID = Convert.ToInt32(row[COMPANYCLIENTID]);

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
            return clientAccount;
        }

        public static ClientAccounts GetCompanyClientAccounts(int companyClientID)
        {
            ClientAccounts clientAccounts = new ClientAccounts();

            Object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clientAccounts.Add(FillClientAccount(row, 0));
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
            return clientAccounts;
        }

        public static ClientAccount GetClientAccount(int companyID, int clientAccountID)
        {

            ClientAccount clientAccount = new ClientAccount();

            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = clientAccountID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClientFundByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clientAccount = FillClientAccount(row, 0);
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
            return clientAccount;
        }

        /// <summary>
        /// Deletes <see cref="Client"/> account.
        /// </summary>
        /// <param name="clientAccountID">Account id to be deleted.</param>
        /// <returns>
        /// true: Successfull
        /// false: UnSuccessfull
        /// </returns>
        public static bool DeleteClientAccount(int clientAccountID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = clientAccountID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientFundByID", parameter) > 0)
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
        /// 
        /// </summary>
        /// <param name="clientAccounts"></param>
        /// <param name="companyClientID"></param>
        /// <returns></returns>
        public static int SaveClientAccount(Prana.Admin.BLL.ClientAccounts clientAccounts, int companyClientID)
        {
            //TODO: Add transaction.
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientFunds", parameter).ToString();
                parameter = new object[4];
                foreach (ClientAccount clientAccount in clientAccounts)
                {
                    parameter[0] = companyClientID;
                    parameter[1] = clientAccount.CompanyClientAccountName;
                    parameter[2] = clientAccount.CompanyClientAccountShortName;
                    parameter[3] = clientAccount.CompanyClientAccountID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClientFund", parameter).ToString());
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
    }
}
