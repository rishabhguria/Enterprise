using Prana.LogManager;
using System;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using Prana.Authentication.Common;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Prana.BusinessObjects;
using Prana.CommonDataCache;

namespace Prana.Authentication
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class AuthenticateUser : IAuthenticateUser
    {
        public static ConcurrentDictionary<int, AuthenticatedUserInfo> _dictLoggedInUser = new ConcurrentDictionary<int, AuthenticatedUserInfo>();

        /// <summary>
        /// Dictionary for Logged in Users
        /// </summary>
        public ConcurrentDictionary<int, AuthenticatedUserInfo> GetLoggedInUser()
        {
            return _dictLoggedInUser;
        }

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static AuthenticateUser _authenticateUser = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static AuthenticateUser GetInstance()
        {
            lock (_lock)
            {
                if (_authenticateUser == null)
                    _authenticateUser = new AuthenticateUser();
                return _authenticateUser;
            }
        }
        #endregion

        /// <summary>
        /// ValidateCompanyUserLogin
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticatedUserInfo ValidateCompanyUserLogin(string login, string password, bool isLoggedInFromSamsara, string samsaraAzureId)
        {
            AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
            object[] parameter = new object[2];
            int userID = int.MinValue;
            try
            {
                parameter[0] = login.ToString();
                parameter[1] = samsaraAzureId;

                string hash = string.Empty;
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
                        if (reader.GetValue(2) != DBNull.Value)
                        {
                            login = reader.GetValue(2).ToString();
                        }
                    }
                }
                //samsaraAzureId will only be empty in case of Enterprise login or samsara login through Username/Password.
                if (string.IsNullOrEmpty(samsaraAzureId) && userID > 0 && !PBKDF2Encryption.VerifyPassword(password, hash))
                {
                    authUser.CompanyUserId = int.MinValue;
                    authUser.ErrorMessage = AuthenticationConstants.MSG_INCORRECT_PASSWORD;
                    authUser.AuthenticationType = AuthenticationTypes.InvalidPassword;
                }
                else if (userID < 0)
                {
                    authUser.CompanyUserId = userID;
                    authUser.ErrorMessage = AuthenticationConstants.MSG_INCORRECT_USER;
                    authUser.AuthenticationType = AuthenticationTypes.InvalidCredentials;
                }
                else
                {
                    authUser = CheckUserAlreadyLoggedIn(userID, login, isLoggedInFromSamsara, string.Empty, samsaraAzureId);
                    authUser.CompanyUserId = userID;

                    authUser.CompanyUser = GetCompanyUser(userID);
                    authUser.CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider;
                    authUser.CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType;
                    authUser.IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked;
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
            return authUser;
        }

        /// <summary>
        /// checks whether the user is already logged in or not
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="isLoggedInFromSamsara"></param>
        /// <returns>error message</returns>
        public AuthenticatedUserInfo CheckUserAlreadyLoggedIn(int companyUserId, string username, bool isLoggedInFromSamsara, string token, string samsaraAzureId = "")
        {
            AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
            try
            {
                // Checking if dictionary does not not contain the companyUserId and the login request is from Samsara --> Add the user to the dictionary
                if (!_dictLoggedInUser.ContainsKey(companyUserId) && isLoggedInFromSamsara)
                {
                    authUser = new AuthenticatedUserInfo()
                    {
                        CompanyUserId = companyUserId,
                        AuthenticationType = AuthenticationTypes.WebLoggedIn,
                        Token = token,
                        CompanyUser = GetCompanyUser(companyUserId),
                        CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider,
                        CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType,
                        IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked,
                        ErrorMessage = string.Empty,
                    };
                    _dictLoggedInUser.TryAdd(companyUserId, authUser);
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_CACHE_UPDATED + companyUserId + AuthenticationConstants.MSG_AUTHENTICATION_TYPE + authUser.AuthenticationType + ' ' + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                // Checking if dictionary does not not contain the companyUserId and the login request is from Enterprise --> Add the user to the dictionary
                else if (!_dictLoggedInUser.ContainsKey(companyUserId) && !isLoggedInFromSamsara)
                {
                    _dictLoggedInUser.TryAdd(companyUserId, new AuthenticatedUserInfo()
                    {
                        CompanyUserId = companyUserId,
                        AuthenticationType = AuthenticationTypes.EnterpriseLoggedIn,
                        Token = token,
                        CompanyUser = GetCompanyUser(companyUserId),
                        CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider,
                        CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType,
                        IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked,
                        ErrorMessage = string.Empty,
                    });
                    if (!isLoggedInFromSamsara)
                    {
                        authUser.AuthenticationType = AuthenticationTypes.EnterpriseLoggedIn;
                    }
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_CACHE_UPDATED + companyUserId + AuthenticationConstants.MSG_AUTHENTICATION_TYPE + authUser.AuthenticationType + ' ' + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                // Checking if dictionary contain the companyUserId, login request is from Samsara, token in the dictionary is null/empty and AuthenticationType is WebLoggedIn(this contition will render in the case of updating cache for samsara logged in user)
                else if (_dictLoggedInUser.ContainsKey(companyUserId) && string.IsNullOrEmpty(_dictLoggedInUser[companyUserId].Token) && isLoggedInFromSamsara && _dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                {
                    AuthenticatedUserInfo authenticatedUserInfo;
                    _dictLoggedInUser.TryRemove(companyUserId, out authenticatedUserInfo);
                    _dictLoggedInUser.TryAdd(companyUserId, new AuthenticatedUserInfo()
                    {
                        CompanyUserId = companyUserId,
                        AuthenticationType = AuthenticationTypes.WebLoggedIn,
                        Token = token,
                        CompanyUser = GetCompanyUser(companyUserId),
                        CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider,
                        CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType,
                        IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked,
                        ErrorMessage = string.Empty,
                    });
                }
                else
                {
                    string msgAcknowledgeName = username;
                    if (!string.IsNullOrWhiteSpace(samsaraAzureId))
                        msgAcknowledgeName = samsaraAzureId;
                    //Checking if the dictionary contains the companyUserId --> Generate the error message for already logged in user
                    if (_dictLoggedInUser.ContainsKey(companyUserId))
                    {
                        //Checking if user is already logged in on Enterprise
                        if (_dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.EnterpriseLoggedIn && string.IsNullOrEmpty(_dictLoggedInUser[companyUserId].Token) && !isLoggedInFromSamsara)
                            authUser.ErrorMessage = username + AuthenticationConstants.MSG_ENTERPRISE_LOGGED_IN;

                        //Checking if user is already logged in on Enterprise and same user is logging from Samsara
                        else if (_dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.EnterpriseLoggedIn)
                        {
                            authUser.ErrorMessage = msgAcknowledgeName + AuthenticationConstants.MSG_ENTERPRISE_LOGGED_IN_WEB;
                            authUser.AuthenticationType = AuthenticationTypes.EnterpriseLoggedIn;
                        }

                        //Checking if user is already logged in on Samsara and same user is logging again from Samsara
                        else if (_dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.WebLoggedIn && (token != _dictLoggedInUser[companyUserId].Token) && isLoggedInFromSamsara)
                        {
                            authUser.ErrorMessage = msgAcknowledgeName + AuthenticationConstants.MSG_WEB_LOGGED_IN;
                            authUser.AuthenticationType = AuthenticationTypes.WebAlreadyLoggedInForAnotherWebSession;
                        }

                        //Checking if user is already logged in on Samsara and same user is logging from Samsara(this condition will render when user login from Samsara then quits the application and relaunch the application)
                        else if (_dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.WebLoggedIn && (token == _dictLoggedInUser[companyUserId].Token) && isLoggedInFromSamsara)
                        {
                            authUser.ErrorMessage = username + AuthenticationConstants.MSG_ENTERPRISE_LOGGED_IN;
                            authUser.AuthenticationType = AuthenticationTypes.WebLoggedIn;
                        }

                        //Checking if user is already logged in on Samsara and same user is logging from Enterprise
                        else
                        {
                            authUser.ErrorMessage = username + AuthenticationConstants.MSG_WEB_LOGGED_IN_ENTERPRISE;
                            authUser.AuthenticationType = AuthenticationTypes.WebLoggedIn;
                        }

                        authUser.CompanyUser = GetCompanyUser(companyUserId);
                        authUser.CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider;
                        authUser.CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType;
                        authUser.IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked;
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
            return authUser;
        }

        /// <summary>
        /// Adds the logged in user to list.
        /// </summary>
        /// <param name="companyUserId"></param>
        public void AddLoggedInUser(int companyUserId)
        {
            try
            {
                if (!_dictLoggedInUser.ContainsKey(companyUserId))
                    _dictLoggedInUser.TryAdd(companyUserId, new AuthenticatedUserInfo()
                    {
                        CompanyUserId = companyUserId,
                        AuthenticationType = AuthenticationTypes.EnterpriseLoggedIn,
                        Token = string.Empty,
                        ErrorMessage = string.Empty,
                        CompanyUser = GetCompanyUser(companyUserId),
                        CompanyMarketDataProvider = CachedDataManager.CompanyMarketDataProvider,
                        CompanyFactSetContractType = CachedDataManager.CompanyFactSetContractType,
                        IsMarketDataBlocked = CachedDataManager.IsMarketDataBlocked
                    });
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

        /// <summary>
        /// Removes the logged in user from list.
        /// </summary>
        /// <param name="companyUserId"></param>
        public string RemoveLoggedInUser(int companyUserId)
        {
            string result = AuthenticationConstants.MSG_FAILED_LOGGED_OUT;
            try
            {
                if (_dictLoggedInUser.ContainsKey(companyUserId))
                {
                    AuthenticatedUserInfo authenticatedUserInfo;
                    _dictLoggedInUser.TryRemove(companyUserId, out authenticatedUserInfo);
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(AuthenticationConstants.MSG_REMOVED_USER_FROM_CACHE + companyUserId + ' ' + DateTime.Now + AuthenticationConstants.MSG_LOCAL_TIME, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    result = "CompanyUserId " + companyUserId + AuthenticationConstants.MSG_SUCCESSFUL_LOGGED_OUT;
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
        /// CompanyUserLogoutEnterprise
        /// </summary>
        /// <param name="companyUserId"></param>
        public string CompanyUserLogout(string companyUserId, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb)
        {
            string result = AuthenticationConstants.MSG_FAILED_LOGGED_OUT;
            try
            {
                result = RemoveLoggedInUser(Convert.ToInt32(companyUserId));
                if (isForcefulLogoutEnterprise || isForcefulLogoutWeb)
                    ClientConnectivity.GetInstance().RemoveClientInfoFromCache(Convert.ToInt32(companyUserId), isForcefulLogoutEnterprise, isForcefulLogoutWeb);
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

        public ServiceStatusInfo GetServiceStatus()
        {
            ServiceStatusInfo serviceStatus = new ServiceStatusInfo();
            serviceStatus.Status = true;
            return serviceStatus;
        }

        private CompanyUser GetCompanyUser(int companyUserID)
        {
            //Validate Login
            object[] parameter = new object[1];
            CompanyUser companyUser = null;
            try
            {
                parameter[0] = companyUserID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUser", parameter))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        companyUser = new CompanyUser();
                        companyUser.CompanyUserID = Convert.ToInt32(row[0].ToString());
                        companyUser.LastName = row[1].ToString();
                        companyUser.FirstName = row[2].ToString();
                        companyUser.ShortName = row[3].ToString();
                        companyUser.Title = row[4].ToString();
                        //companyUser.MailingAddress = row[5].ToString();
                        companyUser.EMail = row[5].ToString();
                        companyUser.TelephoneWork = row[6].ToString();
                        companyUser.TelephoneHome = row[7].ToString();
                        companyUser.TelephoneMobile = row[8].ToString();
                        companyUser.Fax = row[9].ToString();
                        companyUser.LoginID = row[10].ToString();
                        companyUser.CompanyID = Convert.ToInt32(row[18].ToString());
                        companyUser.CompanyName = row[21].ToString();
                        companyUser.FactSetUsernameAndSerialNumber = row[22].ToString();
                        companyUser.IsFactSetSupportUser = (bool)row[23];
                        companyUser.MarketDataAccessIPAddresses = row[24].ToString();
                        companyUser.ActivUsername = row[25].ToString();
                        companyUser.ActivPassword = row[26].ToString();
                        companyUser.HasPowerBIAccess = row[28] != DBNull.Value && (bool)row[28];
                        companyUser.SapiUsername = row[29].ToString();

                        using (IDataReader taReader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingAccountsForUser", parameter))
                        {
                            while (taReader.Read())
                            {
                                object[] taRow = new object[taReader.FieldCount];
                                taReader.GetValues(taRow);

                                Prana.BusinessObjects.TradingAccount tradingAccount = new Prana.BusinessObjects.TradingAccount();
                                tradingAccount.TradingAccountID = Convert.ToInt32(taRow[0].ToString());
                                tradingAccount.Name = taRow[1].ToString();

                                companyUser.TradingAccounts.Add(tradingAccount);
                            }
                        }
                        using (IDataReader mdReader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetMarketDataTypesForUser", parameter))
                        {
                            while (mdReader.Read())
                            {
                                object[] mdRow = new object[mdReader.FieldCount];
                                mdReader.GetValues(mdRow);

                                companyUser.MarketDataTypes.Add(mdRow[1].ToString());
                            }
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
            return companyUser;
        }
    }
}