#region using
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RiskCompanyManager.
    /// </summary>
    public class RiskCompanyManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>

        public RiskCompanyManager()
        {

        }
        #endregion

        /* COMPANY OVERALLLIMITS*/

        #region Basic methods like Save/Get/Fill for CompanyOverallLimit

        //fillCompanyOverallLimitDetails

        /// <summary>
        ///  FillCompanyOverallLimit is a method to fill a object of <see cref="CompanyOverallLimit"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="CompanyOverallLimit"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="CompanyOverallLimit"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". 
        /// And, this row contains all values of CompanyOverallLimit class. So, if the row only contains result from T_RMCompanyOverallLimit table 
        /// then value of Offset would be zero ("0"), and, lets say the row contains value of CompanyOverallLimit as well as any other table then
        /// we have to specify the offset from where the values of CompanyOverallLimit starts. 
        /// Note: Sequence of CompanyOverallLimit class whould be always same as in this method.</remarks>
        public static CompanyOverallLimit FillCompanyOverallLimit(object[] row, int offSet)
        {
            int rMCompanyOverallLimitID = 0 + offSet;
            int companyID = 1 + offSet;
            int rMBaseCurrencyID = 2 + offSet;
            int calculateRiskLimit = 3 + offSet;
            int exposureLimit = 4 + offSet;
            int positivePNL = 5 + offSet;
            int negativePNL = 6 + offSet;

            CompanyOverallLimit companyOverallLimit = new CompanyOverallLimit();
            try
            {
                if (row[rMCompanyOverallLimitID] != null)
                {
                    companyOverallLimit.RMCompanyOverallLimitID = int.Parse(row[rMCompanyOverallLimitID].ToString());
                }
                if (row[companyID] != null)
                {
                    companyOverallLimit.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[rMBaseCurrencyID] != null)
                {
                    companyOverallLimit.RMBaseCurrencyID = int.Parse(row[rMBaseCurrencyID].ToString());
                }
                if (row[calculateRiskLimit] != null)
                {
                    companyOverallLimit.CalculateRiskLimit = int.Parse(row[calculateRiskLimit].ToString());
                }
                if (row[exposureLimit] != null)
                {
                    companyOverallLimit.ExposureLimit = int.Parse(row[exposureLimit].ToString());
                }
                if (row[positivePNL] != null)
                {
                    companyOverallLimit.PositivePNL = int.Parse(row[positivePNL].ToString());
                }
                if (row[negativePNL] != null)
                {
                    companyOverallLimit.NegativePNL = int.Parse(row[negativePNL].ToString());
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
            return companyOverallLimit;

        }

        //Save method

        /// <summary>
        /// This is to save the CompanyOverall limits details as input by the user into DB.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveCompanyOverallLimit(CompanyOverallLimit companyOverallLimit, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[6];

                parameter[0] = companyID;
                parameter[1] = companyOverallLimit.RMBaseCurrencyID;
                parameter[2] = companyOverallLimit.CalculateRiskLimit;
                parameter[3] = companyOverallLimit.ExposureLimit;
                parameter[4] = companyOverallLimit.PositivePNL;
                parameter[5] = companyOverallLimit.NegativePNL;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyOverallLimitDetail", parameter).ToString());
                companyOverallLimit.RMCompanyOverallLimitID = result;
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

        //Get Collection of CompanyOverallLimit

        /// <summary>
        /// this gets the companyOverallLimits of all the companies.
        /// </summary>
        /// <returns>Object of <see cref="CompanyOverallLimits"/> collection.</returns>
        public static CompanyOverallLimits GetCompanyOverallLimits()
        {
            CompanyOverallLimits companyOverallLimits = new CompanyOverallLimits();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyOverallLimits";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyOverallLimits.Add(FillCompanyOverallLimit(row, 0));
                    }
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
            return companyOverallLimits;
        }

        //Get an object of CompanyOverallLimit

        /// <summary>
        /// This gets the CompanyOverallLimits details as per the selected CompanyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompanyOverallLimit GetCompanyOverallLimit(int companyID)
        {
            CompanyOverallLimit companyOverallLimit = new CompanyOverallLimit();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOverallLimitsbyCompanyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyOverallLimit = FillCompanyOverallLimit(row, 0);
                    }
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
            return companyOverallLimit;
        }

        //Delete CompanyOverallLimit

        /// <summary>
        /// The method is used to delete the dat corresponding to a particular companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteRMCompany(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyOverallLimit", parameter) > 0)
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

        #endregion

        /*COMPANY ALERTS*/

        #region Basic methods like Save/Get/Fill for CompanyAlert

        //Fill CompanyAlert

        /// <summary>
        /// The method is used to fill a object of <see cref="CompanyAlert"/> class.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static CompanyAlert FillCompanyAlert(object[] row, int offSet)
        {
            int rMCompanyAlertID = 0 + offSet;
            int companyExposureLower = 1 + offSet;
            int companyExposureUpper = 2 + offSet;
            int alertTypeID = 3 + offSet;
            int refreshRateCalculation = 4 + offSet;
            int alertMessage = 6 + offSet;
            int emailAddress = 7 + offSet;
            int blockTrading = 8 + offSet;
            int companyID = 9 + offSet;
            int rank = 10 + offSet;

            CompanyAlert companyAlert = new CompanyAlert();
            try
            {
                if (row[rMCompanyAlertID] != null)
                {
                    companyAlert.RMCompanyAlertID = int.Parse(row[rMCompanyAlertID].ToString());
                }
                if (row[companyExposureLower] != null)
                {
                    companyAlert.CompanyExposureLower = int.Parse(row[companyExposureLower].ToString());
                }
                if (row[companyExposureUpper] != null)
                {
                    companyAlert.CompanyExposureUpper = int.Parse(row[companyExposureUpper].ToString());
                }
                if (row[alertTypeID] != null)
                {
                    companyAlert.AlertTypeID = int.Parse(row[alertTypeID].ToString());
                }
                if (row[refreshRateCalculation] != null)
                {
                    companyAlert.RefreshRateCalculation = int.Parse(row[refreshRateCalculation].ToString());
                }
                if (row[alertMessage] != null)
                {
                    companyAlert.AlertMessage = row[alertMessage].ToString();
                }
                if (row[emailAddress] != null)
                {
                    companyAlert.EmailAddress = row[emailAddress].ToString();
                }
                if (row[blockTrading] != null)
                {
                    companyAlert.BlockTrading = int.Parse(row[blockTrading].ToString());
                }
                if (row[companyID] != null)
                {
                    companyAlert.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[rank] != null)
                {
                    companyAlert.Rank = int.Parse(row[rank].ToString());
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
            return companyAlert;
        }

        //Delete CompanyAlert

        /// <summary>
        /// The method is used to delete the companyAlerts data corresponding a particular companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteRMCompanyAlerts(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteRMCompanyAlerts", parameter) > 0)
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

        //Save CompanyAlert

        /// <summary>
        /// The method is used to save a companyalerts object for a particular companyID.
        /// </summary>
        /// <param name="companyAlert"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveCompanyAlert(CompanyAlert companyAlert, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[9];

                parameter[0] = companyAlert.CompanyExposureLower;
                parameter[1] = companyAlert.CompanyExposureUpper;
                parameter[2] = companyAlert.AlertTypeID;
                parameter[3] = companyAlert.RefreshRateCalculation;
                parameter[4] = companyAlert.AlertMessage;
                parameter[5] = companyAlert.EmailAddress;
                parameter[6] = companyAlert.BlockTrading;
                parameter[7] = companyID;
                parameter[8] = companyAlert.Rank;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyAlertDetail", parameter).ToString());
                companyAlert.RMCompanyAlertID = result;
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

        //Get the Collection Of CompanyAlerts data

        /// <summary>
        /// Gets all companyOverallLimits' details.
        /// </summary>
        /// <returns>Object of <see cref="CompanyOverallLimits"/> collection.</returns>
        public static CompanyAlerts GetCompanyAlerts(int companyID)
        {
            CompanyAlerts companyAlerts = new CompanyAlerts();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyAlertsbyCompanyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyAlerts.Add(FillCompanyAlert(row, 0));
                    }
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
            return companyAlerts;
        }

        #endregion

        /*DEFAULT ALERTS*/

        #region Default Alert

        //Fill Default Company Alerts

        /// <summary>
        /// the method is used to fill an object of the companyAlert class.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static DefaultAlert FillDefaultAlert(object[] row, int offSet)
        {
            int rMDefaultID = 0 + offSet;
            int alertTypeID = 1 + offSet;
            int refreshRateCalculation = 2 + offSet;
            int companyID = 3 + offSet;

            DefaultAlert defaultAlert = new DefaultAlert();
            try
            {
                if (row[rMDefaultID] != null)
                {
                    defaultAlert.RMDefaultID = int.Parse(row[rMDefaultID].ToString());
                }
                if (row[alertTypeID] != null)
                {
                    defaultAlert.AlertTypeID = int.Parse(row[alertTypeID].ToString());
                }
                if (row[refreshRateCalculation] != null)
                {
                    defaultAlert.RefreshRateCalculation = int.Parse(row[refreshRateCalculation].ToString());
                }
                if (row[companyID] != null)
                {
                    defaultAlert.CompanyID = int.Parse(row[companyID].ToString());
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
            return defaultAlert;

        }

        //Save Default CompanyAlert

        /// <summary>
        /// The method is used to save the details of Default Company Alert.
        /// </summary>
        /// <param name="defaultAlert"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveDefaultAlert(DefaultAlert defaultAlert, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[3];

                parameter[0] = defaultAlert.AlertTypeID;
                parameter[1] = defaultAlert.RefreshRateCalculation;
                parameter[2] = companyID;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("dbo.P_SaveDefaultAlertDetail", parameter).ToString());
                defaultAlert.RMDefaultID = result;
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

        //Get Default Alert

        /// <summary>
        /// the method is used to fetch the default company alerts for a particular companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static DefaultAlert GetDefaultAlert(int companyID)
        {
            DefaultAlert defaultAlert = new DefaultAlert();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetDefaultAlertsbyCompanyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        defaultAlert = FillDefaultAlert(row, 0);
                    }
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
            return defaultAlert;
        }

        //Delete Default Alert

        /// <summary>
        /// the method is used to delete the default alert data corresponding a particular companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteDefaultAlert(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteRMDefaultCompanyAlerts", parameter) > 0)
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

        #endregion

        /*ALERT TYPE*/

        #region get/fill method for Alert Type

        //Fill AlertType

        /// <summary>
        /// Fills alert type for refresh calculation notification. <see cref="CompanyType"/>.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static AlertType FillAlertType(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int type = 1 + offSet;

            AlertType alertType = new AlertType();
            try
            {
                if (row[ID] != null)
                {
                    alertType.AlertTypeID = int.Parse(row[ID].ToString());
                }
                if (row[type] != null)
                {
                    alertType.Type = row[type].ToString();
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
            return alertType;
        }

        //Get Alert Type

        /// <summary>
        /// Get Alert Type Object for a particular AlertTypeID <see cref="AlertTypes"/> from database.
        /// </summary>
        /// <returns><see cref="AlertTypes"/> fetched.</returns>
        public static AlertType GetAlertType(int alertTypeID)
        {
            AlertType alertType = new AlertType();

            object[] parameter = new object[1];
            parameter[0] = alertTypeID;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAlertbyAlertTypeID";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        alertType = FillAlertType(row, 0);
                    }
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
            return alertType;
        }

        #endregion

        /*RM CURRENCY CONVERSION*/

        #region RMCurrencyConversionGrid

        //fillCompanyOverallLimitDetails

        /// <summary>
        /// the method is used to fill an object of RMCurrencyRate.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static RMCurrencyRate FillRMCurrencyRate(object[] row, int offSet)
        {
            int rMBaseCurrency = 0 + offSet;
            int allOtherCurrencies = 1 + offSet;
            int conversion = 2 + offSet;
            int currencyRates = 3 + offSet;

            RMCurrencyRate rMCurrencyRate = new RMCurrencyRate();
            try
            {
                if (row[rMBaseCurrency] != null)
                {
                    rMCurrencyRate.FromCurrencyID = int.Parse(row[rMBaseCurrency].ToString());
                }
                if (row[allOtherCurrencies] != null)
                {
                    rMCurrencyRate.ToCurrency = int.Parse(row[allOtherCurrencies].ToString());
                }
                if (row[conversion] != null)
                {
                    rMCurrencyRate.Conversion = row[conversion].ToString();
                }
                if (row[currencyRates] != null)
                {
                    rMCurrencyRate.CurrencyRates = row[currencyRates].ToString();

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
            return rMCurrencyRate;

        }

        //Get the conversion of one currency into all currencies

        /// <summary>
        /// It gets the currency converion of the selected RM Base Currency from DropDown
        /// into other currencies .
        /// </summary>
        /// <param name="currencyID"></param>
        /// <returns></returns>
        public static RMCurrencyRates GetRMCurrencyRates(int currencyID)
        {
            RMCurrencyRates rMCurrencyRates = new RMCurrencyRates();

            object[] parameter = new object[1];
            parameter[0] = currencyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMCurrencyRateByCurrencyID", parameter))
                {
                    while (reader.Read())
                    {

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMCurrencyRates.Add(FillRMCurrencyRate(row, 0));
                    }
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

            return rMCurrencyRates;
        }

        //Get a Currency conversion to other currency.

        /// <summary>
        /// The method fetches the conversion rate data for one currency to another.
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        public static RMCurrencyRate GetMCurrencyRate(int fromCurrencyID, int toCurrencyID)
        {
            RMCurrencyRate rMCurrencyRate = new RMCurrencyRate();

            object[] parameter = new object[2];
            parameter[0] = fromCurrencyID;
            parameter[1] = toCurrencyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCurrencyConverion", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        rMCurrencyRate = FillRMCurrencyRate(row, 0);

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
            return rMCurrencyRate;

        }

        #endregion RMCurrencyGrid

        /// <summary>
        /// TODO : Move from here to some general manager class 
        /// It gets only those currency conversion rates which converts the currency to RM basecurrency.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, RMCurrencyRate> GetBaseCurrencyRates(int companyID)
        {
            Dictionary<int, RMCurrencyRate> currencyRatesHash = new Dictionary<int, RMCurrencyRate>();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetBaseCurrencyRates", parameter))
                {
                    while (reader.Read())
                    {

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        RMCurrencyRate rmCurrencyRate = FillRMCurrencyRate(row, 0);
                        currencyRatesHash.Add(rmCurrencyRate.FromCurrencyID, rmCurrencyRate);
                    }
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

            return currencyRatesHash;
        }

    }
}
