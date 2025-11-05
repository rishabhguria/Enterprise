using Prana.LogManager;
using System;




namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RMAdminBusinessLogic.
    /// </summary>
    public class RMAdminBusinessLogic
    {

        public RMAdminBusinessLogic()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /*COMPANY*/

        #region CompanyOverallLimits

        /// <summary>
        /// It gets all the companies in the Prana Admin DB through the CompanyManager layer.
        /// This can be filtered using necessary conditions to fectch only companies with permission for RM.
        /// </summary>
        /// <returns></returns>
        public static Companies GetCompanies()
        {
            Companies companies = new Companies();
            try
            {

                companies = CompanyManager.GetCompanies();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return companies;
        }


        /// <summary>
        /// This gets the companyOverallLimit details for the companyID passed through DAL.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompanyOverallLimit GetCompanyOverallLimit(int companyID)
        {
            CompanyOverallLimit companyOverallLimit = new CompanyOverallLimit();
            try
            {

                companyOverallLimit = RiskCompanyManager.GetCompanyOverallLimit(companyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return companyOverallLimit;
        }

        /// <summary>
        /// This method calls the GetCurrency method from DAL to fetch all the currencies.
        /// </summary>
        /// <returns></returns>
        public static Currencies GetCurrencies()
        {
            Currencies currencies = new Currencies();
            try
            {

                currencies = AUECManager.GetCurrencies();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return currencies;
        }

        /// <summary>
        /// this gives the details respective to the specified currencyID.  
        /// </summary>
        /// <param name="currencyID"></param>
        /// <returns></returns>
        public static Currency GetCurrency(int currencyID)
        {
            Currency currency = new Currency();
            try
            {

                currency = AUECManager.GetCurrency(currencyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return currency;
        }

        /// <summary>
        /// This method calls the SaveCompanyOverallLimit from DAL to save all the data input by user in CompanyOverallLimits UI.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveCompanyOverallLimit(CompanyOverallLimit companyOverallLimit, int companyID)
        {
            return RiskCompanyManager.SaveCompanyOverallLimit(companyOverallLimit, companyID);
        }

        /// <summary>
        /// This calls the getRMCurrencyRates method from DAL.
        /// </summary>
        /// <param name="currencyID"></param>
        /// <returns></returns>
        public static RMCurrencyRates GetRMCurrencyConversion(int currencyID)
        {
            RMCurrencyRates rMCurrencyRates = new RMCurrencyRates();
            try
            {

                rMCurrencyRates = RiskCompanyManager.GetRMCurrencyRates(currencyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return rMCurrencyRates;
        }


        /// <summary>
        /// The method is used to check whether as per the new exposure limit, exp lt at sub levels is equivalent or less than the companyExpLt.
        /// if not , then , it reduces the exp lt at the sub level equal to the company exp lt.
        /// </summary>
        /// <param name="companyID"></param>
        public static void VerifySubLevelExpLtValidity(int companyID)
        {
            CompanyOverallLimit compOverall = RiskCompanyManager.GetCompanyOverallLimit(companyID);
            if (compOverall != null)
            {
                int compExpLt = Convert.ToInt32(compOverall.ExposureLimit);
                RMAUECs rMAUECs = RiskAUECManager.GetRMAUECs(companyID);
                if (rMAUECs.Count > 0)
                {
                    foreach (RMAUEC rMAUEC in rMAUECs)
                    {
                        if (rMAUEC.ExposureLimit_RMBaseCurrency > compExpLt)
                        {
                            rMAUEC.ExposureLimit_RMBaseCurrency = compExpLt;
                            RiskAUECManager.SaveRMAUEC(rMAUEC, companyID);
                        }
                    }
                }

                RMTradingAccounts rMTAs = RiskTradingAccountManager.GetRMTradingAccounts(companyID);
                foreach (RMTradingAccount rMTA in rMTAs)
                {
                    if (rMTA.ExposureLimit > compExpLt)
                    {
                        rMTA.ExposureLimit = compExpLt;
                        RiskTradingAccountManager.SaveRMTradingAccnt(rMTA, companyID);
                    }
                }

                UserTradingAccounts userTAs = RiskTradingAccountManager.GetRMUserTradingAccounts(companyID);
                foreach (UserTradingAccount userTA in userTAs)
                {
                    int tradAccntID = userTA.UserTradingAccntID;
                    if (userTA.UserTAExposureLimit > compExpLt)
                    {
                        userTA.UserTAExposureLimit = compExpLt;
                        RiskTradingAccountManager.SaveUserTradingAccount(userTA, companyID, tradAccntID);
                    }
                }

                UserLevelOverallLimits userOveralls = RiskUserLevelManager.GetAllUserLevelOverallLimits(companyID);
                foreach (UserLevelOverallLimit userOverall in userOveralls)
                {
                    if (userOverall.UserExposureLimit > compExpLt)
                    {
                        userOverall.UserExposureLimit = compExpLt;
                        RiskUserLevelManager.SaveUserLevelOverallLimit(userOverall, companyID);
                    }

                }

                RMFundAccounts rMFAs = RiskFundAccountManager.GetRMFundAccounts(companyID);
                foreach (RMFundAccount rMFA in rMFAs)
                {
                    if (rMFA.ExposureLimit_RMBaseCurrency > compExpLt)
                    {
                        rMFA.ExposureLimit_RMBaseCurrency = compExpLt;
                        RiskFundAccountManager.SaveAccountAccntDetail(rMFA, companyID);
                    }
                }

                ClientOverallLimits clientOveralls = RiskClientManager.GetAllClientOverallLimits(companyID);
                foreach (ClientOverallLimit clientOverall in clientOveralls)
                {
                    if (clientOverall.ClientExposureLimit > compExpLt)
                    {
                        clientOverall.ClientExposureLimit = compExpLt;
                        RiskClientManager.SaveClientOverallLimit(clientOverall, companyID);
                    }
                }
            }

        }

        /// <summary>
        /// The method is used to get all the companyOverallLimits details for RMAdmin.
        /// </summary>
        /// <returns></returns>
        public static CompanyOverallLimits GetCompanyOverallLimits()
        {
            CompanyOverallLimits companyOverallLimits = new CompanyOverallLimits();
            try
            {

                companyOverallLimits = RiskCompanyManager.GetCompanyOverallLimits();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return companyOverallLimits;
        }

        #endregion CompanyOverallLimits

        #region Company Alerts

        /// <summary>
        /// this method gets the companyAlerts details as per the companyID passed.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompanyAlerts GetCompanyAlert(int companyID)
        {
            CompanyAlerts companyAlerts = new CompanyAlerts();
            try
            {

                companyAlerts = RiskCompanyManager.GetCompanyAlerts(companyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return companyAlerts;

        }

        /// <summary>
        /// This method calls the GetUser Method from UserManager DAL to get the details of the user whose userID is passed.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User GetCompanyUser(int userID)
        {
            User user = new User();
            try
            {

                user = UserManager.GetCompanyUser(userID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return user;
        }

        /// <summary>
        /// This method calls SaveCompanyAlert method from DAl to save the data input by user in company Alerts.
        /// </summary>
        /// <param name="companyAlert"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveCompanyAlerts(CompanyAlert companyAlert, int companyID)
        {
            return RiskCompanyManager.SaveCompanyAlert(companyAlert, companyID);
        }

        #endregion Company Alerts

        //TODO
        #region DefaultAlerts

        public static int SaveDefaultAlert(DefaultAlert defaultAlert, int companyID)
        {
            return RiskCompanyManager.SaveDefaultAlert(defaultAlert, companyID);
        }

        /// <summary>
        /// The method is used to fetch the default Alert data from database for a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static DefaultAlert GetDefaultAlert(int companyID)
        {
            DefaultAlert defaultAlert = new DefaultAlert();
            try
            {
                defaultAlert = RiskCompanyManager.GetDefaultAlert(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return defaultAlert;
        }

        #endregion DefaultAlerts

        /*RM AUEC*/

        #region RM_AUECs

        /// <summary>
        /// The method is to get the details per auecid.
        /// </summary>
        /// <param name="aUECID"></param>
        /// <returns></returns>
        public static AUEC GetAUECDetail(int aUECID)
        {
            AUEC auec = new AUEC();
            try
            {
                auec = AUECManager.GetAUEC(aUECID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return auec;
        }



        /// <summary>
        /// This gets all the AUECs for a particular companyID 
        /// to display in dropdown.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static AUECs GetAllCompanyAUECs(int companyID)
        {
            AUECs auecs = new AUECs();
            try
            {

                auecs = AUECManager.GetCompanyAUEC(companyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return auecs;
        }

        /// <summary>
        /// This method calls the SaveMethod from DAl to save the details entered for a particular AUEC.
        /// </summary>
        /// <param name="rM_AUEC"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveRMAUEC(RMAUEC rM_AUEC, int companyID)
        {
            return RiskAUECManager.SaveRMAUEC(rM_AUEC, companyID);
        }

        /// <summary>
        /// It gets the details of all the AUECs of a passed companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMAUECs GetRM_AUECs(int companyID)
        {
            RMAUECs rMAUECs = new RMAUECs();
            try
            {
                rMAUECs = RiskAUECManager.GetRMAUECs(companyID);

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

        /// <summary>
        /// It gets the details of one AUEC of a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="aUECID"></param>
        /// <returns></returns>
        public static RMAUEC GetRM_AUEC(int companyID, int aUECID)
        {
            RMAUEC rMAUEC = new RMAUEC();
            try
            {
                rMAUEC = RiskAUECManager.GetRMAUEC(companyID, aUECID);
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

        /// <summary>
        /// The method converts exposure limit from one currency to another.
        /// </summary>
        /// <param name="RMCurrencyID"></param>
        /// <param name="BaseCurrencyID"></param>
        /// <returns></returns>
        public static Int64 ConvertExpLt(int fromCurrencyID, int toCurrencyID, Int64 exposureLt)
        {
            Int64 value = int.MinValue;
            try
            {
                if (fromCurrencyID > 0 && toCurrencyID > 0)
                {
                    RMCurrencyRate rMCurrencyRate = RiskCompanyManager.GetMCurrencyRate(fromCurrencyID, toCurrencyID);
                    if (rMCurrencyRate != null)
                    {
                        double rate = double.Parse(rMCurrencyRate.CurrencyRates);

                        double convertedExpLt = rate * exposureLt;

                        value = Convert.ToInt64(convertedExpLt);
                    }
                    else
                    {
                        //The conversion rate is not available.
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
            return value;
        }

        /// <summary>
        /// The method is used to validate the exposure limit of the company AUEC w.r.t. Company Exp Lt. 
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Int64 ValidRMAUECExpLt(int companyID, Int64 aUEC_ExpLt)
        {
            Int64 maxPermittedExpLt = 0;
            try
            {
                if (companyID != int.MinValue)
                {

                    CompanyOverallLimit companyOverallLimit = RiskCompanyManager.GetCompanyOverallLimit(companyID);
                    Int64 companyExpLt = companyOverallLimit.ExposureLimit;

                    if (aUEC_ExpLt > companyExpLt)
                    {
                        maxPermittedExpLt = companyExpLt;
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
            return maxPermittedExpLt;
        }

        #endregion RM_AUECs

        /*TRADING ACCOUNT*/

        #region RMTradingAccounts

        /// <summary>
        /// The method is used to fetch the details pertaining to on eparticular trading account of a user.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static TradingAccount GetCompanyTradingAccntDetail(int companyID, int tradingAccntID)
        {
            Prana.Admin.BLL.TradingAccount tradAccnt = new TradingAccount();
            try
            {
                tradAccnt = CompanyManager.GetTradingAccountDetail(companyID, tradingAccntID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return tradAccnt;
        }

        /// <summary>
        /// This method calls the save method from DAL to save the details of the trading Account as input by user.
        /// </summary>
        /// <param name="rMtradingAccount"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveRMTradingAccnt(RMTradingAccount rMtradingAccount, int companyID)
        {

            return RiskTradingAccountManager.SaveRMTradingAccnt(rMtradingAccount, companyID);

        }

        /// <summary>
        /// It gets the details of all the trading accounts of a company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMTradingAccounts GetRMTradingAccnts(int companyID)
        {
            RMTradingAccounts rMtradingAccounts = new RMTradingAccounts();
            try
            {
                rMtradingAccounts = RiskTradingAccountManager.GetRMTradingAccounts(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return rMtradingAccounts;
        }

        /// <summary>
        /// This gets the details of the particular trading account of a company. 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static RMTradingAccount GetRMTradingAccnt(int companyID, int tradingAccntID)
        {
            RMTradingAccount rMTradingAccount = new RMTradingAccount();
            try
            {
                rMTradingAccount = RiskTradingAccountManager.GetRMTradingAccount(companyID, tradingAccntID);
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

        /// <summary>
        /// This method fetches all the trading accounts available for a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static TradingAccounts GetCompanyTradingAccnts(int companyID)
        {
            TradingAccounts tradingAccounts = new TradingAccounts();
            try
            {

                tradingAccounts = CompanyManager.GetTradingAccount(companyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return tradingAccounts;
        }

        //Not required

        /// <summary>
        /// the method is used to calculate the available exposure limit for a tradingaccount with respect to the parent companyExposure limit.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        //public static Int64 ValidTradAccntExpLtasperCompany(int companyID)
        //{
        //    Int64 result = Int64.MinValue;
        //    Int64 sum = 0;
        //    try
        //    {
        //        if (companyID != int.MinValue)
        //        {
        //            RMTradingAccounts rMTradingAccounts = RiskTradingAccountManager.GetRMTradingAccounts(companyID);
        //            if (rMTradingAccounts.Count > 0)
        //            {
        //                foreach (RMTradingAccount rMTradingAccount in rMTradingAccounts)
        //                {

        //                    Int64 expLt = rMTradingAccount.ExposureLimit;

        //                    sum += expLt;
        //                }
        //            }

        //            CompanyOverallLimit companyOverallLimit = RiskCompanyManager.GetCompanyOverallLimit(companyID);
        //            Int64 companyExpLt = companyOverallLimit.ExposureLimit;
        //            result = companyExpLt - sum;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {

        //            throw;

        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// The method is used to fetch all the users of a particular tradingAccount.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="tradingAccountID"></param>
        /// <returns></returns>
        public static Users GetUsersforTradingAccount(int tradingAccountID)
        {
            Users users = new Users();
            try
            {
                users = UserManager.GetTradingAccntUsers(tradingAccountID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return users;
        }

        #endregion RMTradingAccounts

        #region UserTradingAccounts


        /// <summary>
        /// The method is used to fetch the details of a single tradingAccount User .
        /// </summary>
        /// <param name="tradingAccountID"></param>
        /// <param name="tradAccntUserID"></param>
        /// <returns></returns>
        public static UserTradingAccount GetUserTradingAccount(int tradingAccountID, int tradAccntUserID)
        {
            UserTradingAccount userTradAccnt = new UserTradingAccount();
            try
            {
                userTradAccnt = RiskTradingAccountManager.GetRMUserTradingAccount(tradingAccountID, tradAccntUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return userTradAccnt;
        }


        /// <summary>
        /// This method calls save m,ethod from DAL to save the UserTradingAccounts for a company.
        /// </summary>
        /// <param name="userTradingAccount"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveUserTradingAccount(UserTradingAccount userTradingAccount, int companyID, int tradAccntID)
        {

            return RiskTradingAccountManager.SaveUserTradingAccount(userTradingAccount, companyID, tradAccntID);

        }

        ///// <summary>
        ///// This gets the details of all the UserTradingAccounts as per the companyID passed.
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <returns></returns>
        //public static UserTradingAccounts GetUserTradingAccounts(int companyID)
        //{
        //    UserTradingAccounts userTradingAccounts = new UserTradingAccounts();
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {

        //            throw;

        //        }
        //    }
        //    return userTradingAccounts;
        //}

        /// <summary>
        /// This gets the details of a particular UserTradingAccount.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userTradingAccountID"></param>
        /// <returns></returns>
        public static UserTradingAccounts GetUserTradingAccounts(int companyID, int userTradingAccountID)
        {
            UserTradingAccounts userTradingAccounts = new UserTradingAccounts();
            try
            {
                userTradingAccounts = RiskTradingAccountManager.GetAllRMUserTradingAccounts(companyID, userTradingAccountID);
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


        /// <summary>
        /// This method is to fetch the users of tradingAccounts.
        /// </summary>
        /// <param name="tradingAccntID"></param>
        /// <returns></returns>
        public static TradingAccounts GetUserofTradingAccounts(int tradingAccntID)
        {
            TradingAccounts userTradingAccounts = new TradingAccounts();
            try
            {
                userTradingAccounts = CompanyManager.GetTradingAccountsForUser(tradingAccntID);
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

        #endregion UserTradingAccounts


        /*USERLEVEL*/

        #region UserLevelOverallLimits

        /// <summary>
        /// This method saves the UserLevel OverallLimits details  entered by the user.
        /// </summary>
        /// <param name="userLevelOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveUserLevelOverallLimit(UserLevelOverallLimit userLevelOverallLimit, int companyID)
        {
            int result = int.MinValue;
            try
            {

                CompanyOverallLimit companyOverallLimit = RiskCompanyManager.GetCompanyOverallLimit(companyID);
                if (companyOverallLimit != null)
                {
                    result = RiskUserLevelManager.SaveUserLevelOverallLimit(userLevelOverallLimit, companyID);

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
            return result;
        }

        /// <summary>
        /// This method gets the fetches the details of all the userOverallLimits as per the selected CompanyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static UserLevelOverallLimits GetUserLevelOverallLimits(int companyID)
        {
            UserLevelOverallLimits userLevelOverallLimits = new UserLevelOverallLimits();
            try
            {

                userLevelOverallLimits = RiskUserLevelManager.GetAllUserLevelOverallLimits(companyID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return userLevelOverallLimits;

        }

        /// <summary>
        /// This method gets the fetches the details of the userOverallLimits as per the selected CompanyID and UserID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static UserLevelOverallLimit GetUserLevelOverallLimit(int companyID, int userID)
        {
            UserLevelOverallLimit userLevelOverallLimit = new UserLevelOverallLimit();
            try
            {

                userLevelOverallLimit = RiskUserLevelManager.GetUserLevelOverallLimit(companyID, userID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return userLevelOverallLimit;

        }

        /// <summary>
        /// The method is used to fetch all teh users of the selected companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Users GetCompanyUsers(int companyID)
        {
            Users users = new Users();
            try
            {
                users = UserManager.GetCompanyUserforRM(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return users;
        }

        #endregion UserLevelOverallLimits

        #region UserUIControls

        /// <summary>
        /// The method is used to fetch all the AUECs of a particular user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AUECs GetUserAUECs(int userID)
        {
            AUECs auecs = new AUECs();
            try
            {
                auecs = AUECManager.GetUserAUEC(userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return auecs;
        }

        /// <summary>
        /// This method calls the save method from DAl layer to save the details of a particular UserUIControl.
        /// </summary>
        /// <param name="userUIControl"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveUserUIControls(UserUIControl userUIControl, int companyID, int companyUserID)
        {

            return RiskUserLevelManager.SaveUserLevelUI(userUIControl, companyID, companyUserID);
        }

        /// <summary>
        /// This gets all the UserUIControls details for a particular companyID.  
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static UserUIControls GetUserUIControls(int companyID, int userID)
        {
            UserUIControls userUIControls = new UserUIControls();
            try
            {
                userUIControls = RiskUserLevelManager.GetAllUserUIControls(companyID, userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return userUIControls;
        }



        /// <summary>
        /// This gets one particular userUICOntrol AUEC detail as per companyId and userId passed.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userID"></param>
        /// <param name="aUECID"></param>
        /// <returns></returns>
        public static UserUIControl GetUserUIControl(int companyID, int selectedUserID, int selectedAUECID)
        {
            UserUIControl userUIControl = new UserUIControl();
            try
            {
                userUIControl = RiskUserLevelManager.GetUserUIControl(companyID, selectedUserID, selectedAUECID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return userUIControl;
        }


        #endregion UserUIControls

        /*FUND ACCOUNT*/

        #region RMAccountAcconts

        /// <summary>
        /// This is to fetch the details of all the RMFundAccounts for a company selected.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static RMFundAccounts GetRMFundAccounts(int companyID)
        {
            RMFundAccounts rMFundAccounts = new RMFundAccounts();
            try
            {
                rMFundAccounts = RiskFundAccountManager.GetRMFundAccounts(companyID);
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

        /// <summary>
        /// This is to fetch the details of the RMFundAccount for the accountAcct selected.
        /// </summary>
        /// <param name="compnayID"></param>
        /// <param name="accountAcctID"></param>
        /// <returns></returns>
        public static RMFundAccount GetRMFundAccount(int compnayID, int accountAcctID)
        {
            RMFundAccount rMFundAccount = new RMFundAccount();
            try
            {
                rMFundAccount = RiskFundAccountManager.GetRMFundAccount(compnayID, accountAcctID);
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

        /// <summary>
        /// the method is used to fetch the accountaccnts of a selected company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Accounts GetCompanyaccounts(int companyID)
        {
            Accounts accounts = new Accounts();
            try
            {
                accounts = CompanyManager.GetAccount(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return accounts;
        }

        /// <summary>
        /// The methos is used to save the details entered for a particular FundAccount.
        /// </summary>
        /// <param name="rMFundAccount"></param>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public static int SaveRMFundAccnt(RMFundAccount rMFundAccount, int companyID)
        {
            return RiskFundAccountManager.SaveAccountAccntDetail(rMFundAccount, companyID);
        }


        public static Account GetCompanyAccount(int accountID)
        {
            Account account = new Account();
            try
            {

                account = CompanyManager.GetsAccount(accountID);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return account;
        }

        #endregion RMAccountAcconts

        /*CLIENT*/

        #region ClientOverallLimits

        /// <summary>
        /// The method is used to fetch all the Clients of a company in a dropdown from superadmin.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompanyClients GetCompanyClients(int companyID)
        {
            CompanyClients companyClients = new CompanyClients();
            try
            {
                companyClients = CompanyManager.GetCompanyClientsRM(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return companyClients;
        }

        /// <summary>
        /// This method calls the Save method from DAL to save the ClientOverallLimit details.
        /// </summary>
        /// <param name="clientOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveClientOverallLimit(ClientOverallLimit clientOverallLimit, int companyID)
        {
            int result = int.MinValue;
            try
            {

                CompanyOverallLimit companyOverallLimit = RiskCompanyManager.GetCompanyOverallLimit(companyID);
                if (companyOverallLimit != null)
                {
                    result = RiskClientManager.SaveClientOverallLimit(clientOverallLimit, companyID);

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
            return result;
        }

        /// <summary>
        /// This method gets the ClientOverallLimits for a particular companyID.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static ClientOverallLimits GetClientOverallLimits(int companyID)
        {
            ClientOverallLimits clientOverallLimits = new ClientOverallLimits();
            try
            {

                clientOverallLimits = RiskClientManager.GetAllClientOverallLimits(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return clientOverallLimits;
        }


        /// <summary>
        /// This method gets the ClientOverallLimit for a particular client.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public static ClientOverallLimit GetClientOverallLimit(int companyID, int clientID)
        {
            ClientOverallLimit clientOverallLimit = new ClientOverallLimit();
            try
            {

                clientOverallLimit = RiskClientManager.GetClientOverallLimit(companyID, clientID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return clientOverallLimit;

        }
        #endregion ClientOverallLimits

        #region Deletion Methods for all Levels

        /// <summary>
        /// The method is used to delete all the RM Details of a particular company.
        /// </summary>
        /// <param name ="companyID"></param>
        /// <returns></returns>
        public static bool DeleteRMCompanyDetails(int companyID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskCompanyManager.DeleteRMCompany(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        /// <summary>
        /// The method is used to delete a particular AUEC details.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyAUECID"></param>
        /// <returns></returns>
        public static bool DeleteSelectedAUECfromRM(int companyID, int companyAUECID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskAUECManager.DeleteRMAUEC(companyID, companyAUECID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;

        }

        /// <summary>
        /// The method is used to delete the Trading Accnt details along with the user.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="companyAUECID"></param>
        /// <returns></returns>
        public static bool DeleteSelectedTradingAccountfromRM(int companyID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskTradingAccountManager.DeleteRMTradingAccnt(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        public static bool DeleteSelectedUserfromRM(int companyID, int companyUserID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskUserLevelManager.DeleteUserOverallLimit(companyID, companyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        public static bool DeleteSelectedAccountAccntfromRM(int companyID, int companyAccountAccntID)
        {

            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskFundAccountManager.DeleteFundAccount(companyID, companyAccountAccntID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        public static bool DeleteSelectedClient(int companyID, int clientID)
        {

            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskClientManager.DeleteClientOverallLimit(companyID, clientID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        public static void DeleteRMCompanyAlerts(int companyID)
        {
            try
            {
                RiskCompanyManager.DeleteRMCompanyAlerts(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
        }

        public static bool DeleteRMTradAccntUserDetails(int tradAccntID, int tradAccntUserID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskTradingAccountManager.DeleteUserTradingAccount(tradAccntID, tradAccntUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        public static bool DeleteRMUserAUECDetails(int companyUserID, int userAUECID)
        {
            bool IsDeleted = true;
            try
            {
                IsDeleted = RiskUserLevelManager.DeleteUserUIControl(companyUserID, userAUECID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return IsDeleted;
        }

        #endregion Deletion Methods for all Levels







    }
}
