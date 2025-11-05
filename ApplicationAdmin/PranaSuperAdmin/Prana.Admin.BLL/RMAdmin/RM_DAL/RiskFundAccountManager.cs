using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    public class RiskFundAccountManager
    {
        #region Constructors

        public RiskFundAccountManager()
        {

        }

        #endregion Constructors

        /*RM FUND ACCOUNT*/

        #region RMFundAccount

        // Fill Account Account

        /// <summary>
        ///  FillRMFundAccount is a method to fill a object of <see cref="RMFundAccount"/> class.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static RMFundAccount FillRMFundAccount(object[] row, int offSet)
        {
            int companyFundAccntRMID = 0 + offSet;
            int exposureLimitRMBaseCurrency = 1 + offSet;
            int companyID = 2 + offSet;
            int fAPositivePNL = 3 + offSet;
            int fANegativePNL = 4 + offSet;
            int companyAccountAccntID = 5 + offSet;

            RMFundAccount rMFundAccount = new RMFundAccount();
            try
            {
                if (row[companyFundAccntRMID] != null)
                {
                    rMFundAccount.CompanyFundAccntRMID = int.Parse(row[companyFundAccntRMID].ToString());
                }
                if (row[exposureLimitRMBaseCurrency] != null)
                {
                    rMFundAccount.ExposureLimit_RMBaseCurrency = int.Parse(row[exposureLimitRMBaseCurrency].ToString());
                }
                if (row[companyID] != null)
                {
                    rMFundAccount.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[fAPositivePNL] != null)
                {
                    rMFundAccount.Positive_PNL_Loss = int.Parse(row[fAPositivePNL].ToString());
                }
                if (row[fANegativePNL] != null)
                {
                    rMFundAccount.Negative_PNL_Loss = int.Parse(row[fANegativePNL].ToString());
                }
                if (row[companyAccountAccntID] != null)
                {
                    rMFundAccount.CompanyFundAccntID = int.Parse(row[companyAccountAccntID].ToString());
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
            return rMFundAccount;
        }

        //Get single Account Account Detail
        /// <summary>
        /// the method is used to fetch the single row of details for a selected AccountAccnt of the selected company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyFundAccntID"></param>
        /// <returns></returns>
        public static RMFundAccount GetRMFundAccount(int companyID, int companyFundAccntID)
        {
            RMFundAccount rMFundAccount = new RMFundAccount();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyFundAccntID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMFundAccount", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMFundAccount = FillRMFundAccount(row, 0);

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
            return rMFundAccount;
        }

        //Get Collection of Account CashAccounts

        /// <summary>
        /// The method is used to fetch all the RMAccountAccnts details for a selected companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMFundAccounts GetRMFundAccounts(int companyID)
        {
            RMFundAccounts rMFundAccounts = new RMFundAccounts();
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMFundAccounts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMFundAccounts.Add(FillRMFundAccount(row, 0));
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
            return rMFundAccounts;
        }

        //Save FundAccount

        /// <summary>
        /// The method is to save the details of a single AccountAccnt of the selected company.
        /// </summary>
        /// <param name="rMFundAccount"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveAccountAccntDetail(RMFundAccount rMFundAccount, int companyID)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[7];

                parameter[0] = rMFundAccount.CompanyFundAccntRMID;
                parameter[1] = companyID;
                parameter[2] = rMFundAccount.CompanyFundAccntID;
                parameter[3] = rMFundAccount.ExposureLimit_RMBaseCurrency;
                parameter[4] = rMFundAccount.Positive_PNL_Loss;
                parameter[5] = rMFundAccount.Negative_PNL_Loss;
                parameter[6] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveRMFundAccount", parameter).ToString());
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

        //Delete a particular Account Account

        /// <summary>
        /// The method is used to delete a particular RM accountAccnt detail.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="rMFundAccntID"></param>
        /// <returns></returns>
        public static bool DeleteFundAccount(int companyID, int rMFundAccntID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = rMFundAccntID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteFundAccount", parameter) > 0)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Delete a collection 

        /// <summary>
        /// The method is use to delete all the RMFundAccounts for a particular companyID
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteAllRMFundAccounts(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllRMFundAccounts", parameter) > 0)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;

        }

        #endregion RMFundAccount

    }
}
