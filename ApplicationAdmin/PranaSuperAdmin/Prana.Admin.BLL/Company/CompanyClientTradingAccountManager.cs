using Prana.LogManager;
using System;
using System.Data;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClientTradingAccountManager.
    /// </summary>
    public class CompanyClientTradingAccountManager
    {


        public CompanyClientTradingAccountManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Private Methods
        public static CompanyClientTradingAccounts GetCompanyClientTradingAccount(int CompanyClientID)
        {

            CompanyClientTradingAccounts companyClientTradingAccounts = new CompanyClientTradingAccounts();

            Object[] parameter = new object[1];
            parameter[0] = CompanyClientID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientTrAccByCompanyClientID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClientTradingAccounts.Add(FillCompanyClientTradingAccounts(row, 0));
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
            return companyClientTradingAccounts;
        }

        private static CompanyClientTradingAccount FillCompanyClientTradingAccounts(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            CompanyClientTradingAccount companyClientTradingAccount = null;

            try
            {
                if (row != null)
                {
                    companyClientTradingAccount = new CompanyClientTradingAccount();
                    int CompClientTradingAccount = offset + 0;
                    int CompanyTradingAccountID = offset + 1;
                    int CompanyAccountName = offset + 2;
                    int CompanyClientID = offset + 3;
                    int ClientTraderID = offset + 4;
                    int ClientTraderShortName = offset + 5;



                    companyClientTradingAccount.CompanyTradingAccountName = row[CompanyAccountName].ToString();
                    companyClientTradingAccount.ClientTraderID = int.Parse(row[ClientTraderID].ToString());
                    companyClientTradingAccount.ClientTraderShortName = Convert.ToString(row[ClientTraderShortName]);
                    companyClientTradingAccount.CompanyClientID = int.Parse(row[CompanyClientID].ToString());
                    companyClientTradingAccount.CompanyTradingAccountID = int.Parse(row[CompanyTradingAccountID].ToString());
                    companyClientTradingAccount.CompClientTradingAccount = Convert.ToString(row[CompClientTradingAccount]);

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
            return companyClientTradingAccount;
        }

        public static int SaveCompanyClientTradingAccount(CompanyClientTradingAccounts companyClientTradingAccounts, int CompanyClientID)
        {


            int result = int.MinValue;
            object[] parameter;
            object[] parameterdelete = new object[1];
            try
            {
                parameterdelete[0] = CompanyClientID;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCompanyClientTradingAccounts", parameterdelete);
                parameter = new object[5];
                foreach (CompanyClientTradingAccount companyClientTradingAccount in companyClientTradingAccounts)
                {
                    parameter[0] = companyClientTradingAccount.CompanyClientTradingAccountID;
                    parameter[1] = companyClientTradingAccount.CompClientTradingAccount;
                    parameter[2] = companyClientTradingAccount.CompanyTradingAccountID;
                    parameter[3] = CompanyClientID;
                    parameter[4] = companyClientTradingAccount.ClientTraderID;

                    DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyClientTradingAccount", parameter);
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
        #endregion
    }
}
