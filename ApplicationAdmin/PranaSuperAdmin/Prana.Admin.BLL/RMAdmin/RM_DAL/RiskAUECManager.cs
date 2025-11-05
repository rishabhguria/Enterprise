using Prana.LogManager;
using System;
using System.Data;


namespace Prana.Admin.BLL
{
    /// <summary>
    ///  Summary description for RiskAUECManager.
    /// </summary>
    public class RiskAUECManager
    {
        #region Constructors

        public RiskAUECManager()
        {

        }

        #endregion Constructors

        /*RM COMPANY AUECs*/

        #region Basics methods of RM_AUECs

        //Fill RM CompanyAUECs

        /// <summary>
        ///  FillRMAUEC is a method to fill a object of <see cref="RM_AUEC"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="RM_AUEC"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="RM_AUEC"/> class.</returns>
        /// And, this row contains all values of RM_AUEC class. So, if the row only contains result from T_RMAUEC table 
        /// then value of Offset would be zero ("0"), and, lets say the row contains value of RM_AUEC as well as any other table then
        /// we have to specify the offset from where the values of RM_AUEC starts. 
        /// Note: Sequence of RM_AUEC class whould be always same as in this method.</remarks>
        public static RMAUEC FillRMAUEC(object[] row, int offSet)
        {
            int rMAUECID = 0 + offSet;
            int aUECID = 1 + offSet;
            int exposureLimitRMBaseCurrency = 2 + offSet;
            int exposureLimitBaseCurrency = 3 + offSet;
            int maximumPNLLossRMBaseCurrency = 4 + offSet;
            int maximumPNLLossBaseCurrency = 5 + offSet;
            int companyID = 6 + offSet;

            RMAUEC rM_AUEC = new RMAUEC();
            try
            {
                if (row[rMAUECID] != null)
                {
                    rM_AUEC.RMAUECID = int.Parse(row[rMAUECID].ToString());
                }
                if (row[companyID] != null)
                {
                    rM_AUEC.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[exposureLimitRMBaseCurrency] != null)
                {
                    rM_AUEC.ExposureLimit_RMBaseCurrency = int.Parse(row[exposureLimitRMBaseCurrency].ToString());
                }
                if (row[exposureLimitBaseCurrency] != null)
                {
                    rM_AUEC.ExposureLimit_BaseCurrency = int.Parse(row[exposureLimitBaseCurrency].ToString());
                }
                if (row[maximumPNLLossRMBaseCurrency] != null)
                {
                    rM_AUEC.MaximumPNLLoss_RMBaseCurrency = int.Parse(row[maximumPNLLossRMBaseCurrency].ToString());
                }
                if (row[maximumPNLLossBaseCurrency] != null)
                {
                    rM_AUEC.MaximumPNLLoss_BaseCurrency = int.Parse(row[maximumPNLLossBaseCurrency].ToString());
                }
                if (row[aUECID] != null)
                {
                    rM_AUEC.AUECID = int.Parse(row[aUECID].ToString());
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
            return rM_AUEC;

        }

        //Save RM CompanyAUECs

        /// <summary>
        /// This is to save the RMAUEC details for one CompanyAUEC as input by the user .
        /// </summary>
        /// <param name="rMAUEC"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveRMAUEC(RMAUEC rMAUEC, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[8];

                parameter[0] = rMAUEC.RMAUECID;
                parameter[1] = companyID;
                parameter[2] = rMAUEC.AUECID;
                parameter[3] = rMAUEC.ExposureLimit_RMBaseCurrency;
                parameter[4] = rMAUEC.ExposureLimit_BaseCurrency;
                parameter[5] = rMAUEC.MaximumPNLLoss_RMBaseCurrency;
                parameter[6] = rMAUEC.MaximumPNLLoss_BaseCurrency;
                parameter[7] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveRMAUECs", parameter).ToString());
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

        //Get collection of AUECs RM details 

        /// <summary>
        /// To get all the AUECs' details for a companyID passed.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMAUECs GetRMAUECs(int companyID)
        {
            RMAUECs rMAUECs = new RMAUECs();
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllRMAUECs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMAUECs.Add(FillRMAUEC(row, 0));
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
            return rMAUECs;
        }

        //Get object of RM AUEC 

        /// <summary>
        /// This gets the details of one particular AUEC of a company as per the CompanyID and AUECID passed.
        /// </summary>
        /// <param name="compantID"></param>
        /// <param name="aUECID"></param>
        /// <returns></returns>
        public static RMAUEC GetRMAUEC(int companyID, int aUECID)
        {
            RMAUEC rMAUEC = new RMAUEC();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = aUECID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMAUEC = FillRMAUEC(row, 0);

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
            return rMAUEC;
        }

        //Delete One RMAUEC

        /// <summary>
        /// the method is used to delete a particular Rm companyAUEC details.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public static bool DeleteRMAUEC(int companyID, int auecID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = auecID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteRMAUEC", parameter) > 0)
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

        //Delete all RMAUEC of a companyID.

        /// <summary>
        /// the method is used to delete all the RM details of all companyAUECs of a particular company
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteAllRMAUEC(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if ((DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllRMAUEC", parameter)) > 0)
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
        /// Created by : <Kanupriya>
        /// purpose :<To obtain currencyconversion of an amt.>
        /// date : <10/28/2006>
        /// </summary>
        /// <param name="fromCurrencyID"></param>
        /// <param name="toCurrencyID"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static double CurrencyConversion(int fromCurrencyID, int toCurrencyID, double amt)
        {
            double value = double.MinValue;
            try
            {
                if (fromCurrencyID > 0 && toCurrencyID > 0 && (fromCurrencyID != toCurrencyID))
                {
                    RMCurrencyRate rMCurrencyRate = RiskCompanyManager.GetMCurrencyRate(fromCurrencyID, toCurrencyID);
                    if (rMCurrencyRate != null)
                    {
                        double rate = double.Parse(rMCurrencyRate.CurrencyRates);

                        double convertedExpLt = rate * amt;

                        value = convertedExpLt;
                    }
                    else
                    {
                        //The conversion rate is not available.
                    }
                }
                else
                {
                    value = amt;
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
            return value;
        }

        #endregion Basics methods of RM_AUECs
    }
}