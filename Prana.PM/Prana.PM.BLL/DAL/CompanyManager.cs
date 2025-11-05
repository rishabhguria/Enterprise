using Prana.BusinessLogic;
using Prana.BusinessObjects.PositionManagement;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.PM.DAL
{
    public class CompanyManager
    {

        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;


        public static CompanyNameIDList GetCompanyNameIDListWithSelect()
        {
            CompanyNameIDList companyNameIDList = GetCompanyNameIDList();
            companyNameIDList.Insert(0, new CompanyNameID(-1, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            return companyNameIDList;
        }

        /// <summary>
        /// Gets all <see cref="Companies"/> from datatbase.
        /// </summary>
        /// <returns><see cref="Companies"/> fetched.</returns>
        public static CompanyNameIDList GetCompanyNameIDList()
        {
            CompanyNameIDList companies = new CompanyNameIDList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyNameIDList";

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    Dictionary<string, int> columnorderInfo = new Dictionary<string, int>();

                    for (int counter = 0; counter < reader.FieldCount; counter++)
                    {
                        columnorderInfo.Add(reader.GetName(counter), counter);
                    }


                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];

                        reader.GetValues(row);
                        companies.Add(FillCompanyNameID(row, columnorderInfo));
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
            return companies;
        }

        /// <summary>
        /// Fills the company name ID.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnorderInfo">The columnorder info.</param>
        /// <returns></returns>
        private static CompanyNameID FillCompanyNameID(object[] row, Dictionary<string, int> columnorderInfo)
        {
            CompanyNameID company = null;

            if (row != null)
            {
                company = new CompanyNameID();
                try
                {
                    company.ID = Convert.ToInt32(row[columnorderInfo["COMPANYID"]]);
                    company.FullName = Convert.ToString(row[columnorderInfo["COMPANYNAME"]]);
                    company.ShortName = Convert.ToString(row[columnorderInfo["SHORTNAME"]]);
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
            }
            return company;
        }

        /// <summary>
        /// Gets the List of All Company Types.
        /// </summary>
        /// <returns></returns>
        public static CompanyTypeList GetCompanyTypeList()
        {
            CompanyTypeList companyTypeList = new CompanyTypeList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyTypes";

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyTypeList.Add(FillCompanyList(row, 0));
                        //dataSourceTypes.Add(FillDataSourceType(row, 0));
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

            return companyTypeList;
        }

        /// <summary>
        /// Fills the Company Type.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static CompanyType FillCompanyList(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            CompanyType companyType = null;

            if (row != null)
            {
                companyType = new CompanyType();
                int companyTypeID = offset + 0;
                int companyTypeName = offset + 1;

                try
                {
                    companyType.CompanyTypeID = Convert.ToInt32(row[companyTypeID]);
                    companyType.Type = Convert.ToString(row[companyTypeName]);

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
            }

            return companyType;
        }


        /// <summary>
        /// Gets the List of All user for this Company .
        /// </summary>
        /// <returns></returns>
        public static UserList GetCompanyUserList(int companyID)
        {
            UserList usersList = new UserList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyUser";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);


                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        usersList.Add(FillUser(row, 0));

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
            usersList.Insert(0, new User(0, ApplicationConstants.C_COMBO_SELECT));

            return usersList;
        }

        /// <summary>
        /// Gets the List of All user for this Company .
        /// </summary>
        /// <returns></returns>
        public static TradingAccountList GetTradingAccountsByUserID(int userID)
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetTradingAccountsForUser";
            queryData.DictionaryDatabaseParameter.Add("@userID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@userID",
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            TradingAccountList tradingAccountList = new TradingAccountList();


            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    //CommonManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, commandSP);

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccountList.Add(FillTradingAccount(row, 0));

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
            tradingAccountList.Insert(0, new TradingAccount(-1, ApplicationConstants.C_COMBO_SELECT));

            return tradingAccountList;
        }


        public static TradingAccount FillTradingAccount(object[] row, int offSet)
        {
            int tradingAccountsID = 0 + offSet;
            int tradingAccountName = 1 + offSet;
            int tradingShortName = 2 + offSet;
            int companyID = 3 + offSet;

            TradingAccount tradingAccount = new TradingAccount();
            try
            {
                if (row[tradingAccountsID] != null)
                {
                    tradingAccount.TradingAccountsID = int.Parse(row[tradingAccountsID].ToString());
                }
                if (row[tradingAccountName] != null)
                {
                    tradingAccount.TradingAccountName = row[tradingAccountName].ToString();
                }
                if (row[tradingShortName] != null)
                {
                    tradingAccount.TradingShortName = row[tradingShortName].ToString();
                }
                if (row[companyID] != null)
                {
                    tradingAccount.CompanyID = int.Parse(row[companyID].ToString());
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
            return tradingAccount;
        }


        /// <summary>
        /// Fills the User.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static User FillUser(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            User user = null;

            if (row != null)
            {
                user = new User();
                int companyUserID = offset + 0;
                int login = offset + 1;
                int name = offset + 2;
                int password = offset + 3;

                try
                {
                    user.CompanyUserID = Convert.ToInt32(Convert.ToString(row[companyUserID]));
                    user.ID = Convert.ToString(row[login]);
                    user.UserName = Convert.ToString(row[name]);
                    user.Password = Convert.ToString(row[password]);
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

            }

            return user;
        }

        /// <summary>
        /// Gets the company details for ID.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static Company GetCompanyDetailsForID(int companyID)
        {

            Company companyDetails = new Company();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyDetailsForID";
            queryData.DictionaryDatabaseParameter.Add("@ID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyDetails = FillCompanyDetails(row, 0);

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

            return companyDetails;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Company FillCompanyDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Company company = null;

            if (row != null)
            {
                company = new Company();
                int ID = offset + 0;
                int FullName = offset + 1;
                int ShortName = offset + 2;
                int companyTypeID = offset + 3;

                // Address details
                int Address1 = offset + 4;
                int Address2 = offset + 5;
                int CountryID = offset + 6;
                int StateID = offset + 7;
                int Zip = offset + 8;
                int WorkNumber = offset + 9;
                int FaxNumber = offset + 10;

                // Admin details
                int AdminContactLoginName = offset + 11;
                int AdminContactFirstName = offset + 12;
                int AdminContactLastName = offset + 13;
                int AdminContactTitle = offset + 14;
                int AdminContactUserID = offset + 15;
                int AdminContactPassword = offset + 16;
                int AdminContactEmail = offset + 17;
                int AdminContactWorkNumber = offset + 18;
                int AdminContactCellNumber = offset + 19;
                int AdminContactPagerNumber = offset + 20;
                int AdminContactHomeNumber = offset + 21;
                int AdminContactFaxNumber = offset + 22;

                int NumberOfUserLicences = offset + 23;
                int CompanyUserID = offset + 24;

                try
                {
                    company.CompanyNameID.ID = Convert.ToInt32(row[ID]);
                    company.CompanyNameID.FullName = Convert.ToString(row[FullName]);
                    company.CompanyNameID.ShortName = Convert.ToString(row[ShortName]);
                    company.CompanyType.CompanyTypeID = Convert.ToInt32(row[companyTypeID]);

                    // Address details
                    company.AddressDetails.Address1 = Convert.ToString(row[Address1]);
                    company.AddressDetails.Address2 = Convert.ToString(row[Address2]);
                    company.AddressDetails.CountryId = Convert.ToInt32(row[CountryID]);
                    company.AddressDetails.StateId = Convert.ToInt32(row[StateID]);
                    company.AddressDetails.Zip = Convert.ToString(row[Zip]);
                    company.AddressDetails.WorkNumber = Convert.ToString(row[WorkNumber]);
                    company.AddressDetails.FaxNumber = Convert.ToString(row[FaxNumber]);

                    // Admin details

                    company.AdminUser.CompanyUserID = Convert.ToInt32(Convert.ToString(row[CompanyUserID]));
                    company.AdminUser.UserName = Convert.ToString(row[AdminContactLoginName]);
                    company.AdminUser.AdminFirstName = Convert.ToString(row[AdminContactFirstName]);
                    company.AdminUser.AdminLastName = Convert.ToString(row[AdminContactLastName]);
                    company.AdminUser.AdminTitle = Convert.ToString(row[AdminContactTitle]);
                    company.AdminUser.ID = Convert.ToString(row[AdminContactUserID]);
                    company.AdminUser.Password = Convert.ToString(row[AdminContactPassword]);
                    company.AdminUser.AdminEmail = Convert.ToString(row[AdminContactEmail]);
                    company.AdminUser.AdminWorkNumber = Convert.ToString(row[AdminContactWorkNumber]);
                    company.AdminUser.AdminCellNumber = Convert.ToString(row[AdminContactCellNumber]);
                    company.AdminUser.AdminPagerNumber = Convert.ToString(row[AdminContactPagerNumber]);
                    company.AdminUser.AdminHomeNumber = Convert.ToString(row[AdminContactHomeNumber]);
                    company.AdminUser.AdminFaxNumber = Convert.ToString(row[AdminContactFaxNumber]);

                    company.NumberOfUserLicences = Convert.ToInt32(row[NumberOfUserLicences]);
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



            }
            return company;
        }

        /// <summary>
        /// PMs the get company admin contact details for ID.
        /// </summary>
        /// <param name="userLoginID">The user login ID.</param>
        /// <returns></returns>
        public static User GetCompanyAdminContactDetailsForID(int companyUserID)
        {
            User userDetails = new User();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyAdminContactDetailsForID";
            queryData.DictionaryDatabaseParameter.Add("@ID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ID",
                ParameterType = DbType.Int32,
                ParameterValue = companyUserID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userDetails = FillCompanyAdminUserDetails(row, 0);
                        userDetails.CompanyUserID = companyUserID;

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

            return userDetails;

        }

        /// <summary>
        /// Fills the company admin user details.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static User FillCompanyAdminUserDetails(object[] row, int offset)
        {
            User userDetails = new User();

            if (offset < 0)
            {
                offset = 0;
            }
            if (row != null)
            {

                // Admin details
                int AdminContactLoginName = offset + 0;
                int AdminContactFirstName = offset + 1;
                int AdminContactLastName = offset + 2;
                int AdminContactTitle = offset + 3;
                int AdminContactUserID = offset + 4;
                int AdminContactPassword = offset + 5;
                int AdminContactEmail = offset + 6;
                int AdminContactWorkNumber = offset + 7;
                int AdminContactCellNumber = offset + 8;
                int AdminContactPagerNumber = offset + 9;
                int AdminContactHomeNumber = offset + 10;
                int AdminContactFaxNumber = offset + 11;
                // Admin details

                try
                {
                    userDetails.UserName = Convert.ToString(row[AdminContactLoginName]);
                    userDetails.AdminFirstName = Convert.ToString(row[AdminContactFirstName]);
                    userDetails.AdminLastName = Convert.ToString(row[AdminContactLastName]);
                    userDetails.AdminTitle = Convert.ToString(row[AdminContactTitle]);
                    userDetails.ID = Convert.ToString(row[AdminContactUserID]);
                    userDetails.Password = Convert.ToString(row[AdminContactPassword]);
                    userDetails.AdminEmail = Convert.ToString(row[AdminContactEmail]);
                    userDetails.AdminWorkNumber = Convert.ToString(row[AdminContactWorkNumber]);
                    userDetails.AdminCellNumber = Convert.ToString(row[AdminContactCellNumber]);
                    userDetails.AdminPagerNumber = Convert.ToString(row[AdminContactPagerNumber]);
                    userDetails.AdminHomeNumber = Convert.ToString(row[AdminContactHomeNumber]);
                    userDetails.AdminFaxNumber = Convert.ToString(row[AdminContactFaxNumber]);
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

            }

            return userDetails;

        }


        /// <summary>
        /// Gets the company application details for the company ID from the DataBase.
        /// </summary>
        /// <param name="companyID">The companyID.</param>
        /// <returns></returns>
        public static CompanyApplicationDetails GetCompanyApplicationDetailsForID(int companyID)
        {
            CompanyApplicationDetails companyApplicationDetails = new CompanyApplicationDetails();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyApplicationDetailsForID";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int16,
                ParameterValue = companyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyApplicationDetails = FillCompanyApplicationDetails(row, 0);

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
            return companyApplicationDetails;
        }

        private static CompanyApplicationDetails FillCompanyApplicationDetails(object[] row, int offset)
        {
            CompanyApplicationDetails companyApplicationDetails = new CompanyApplicationDetails();

            if (offset < 0)
            {
                offset = 0;
            }

            if (row != null)
            {


                // Admin details
                int riskModelID = offset + 0;
                int RiskModelName = offset + 1;
                int AllowDailyImport = offset + 2;

                try
                {
                    if (row[riskModelID] != System.DBNull.Value)
                    {
                        companyApplicationDetails.PricingModel.ID = Convert.ToInt16(row[riskModelID]);
                    }
                    if (row[RiskModelName] != System.DBNull.Value)
                    {
                        companyApplicationDetails.PricingModel.Name = Convert.ToString(row[RiskModelName]);
                    }
                    if (row[AllowDailyImport] != System.DBNull.Value)
                    {
                        companyApplicationDetails.AllowDailyImport = Convert.ToBoolean(row[AllowDailyImport]);
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


            }



            return companyApplicationDetails;


        }

        /// <summary>
        /// Saves the company details.
        /// </summary>
        /// <param name="CompanyDetails">The company details.</param>
        public static int SavePMCompanyDetails(Company CompanyDetails)
        {
            int companyID = CompanyDetails.CompanyNameID.ID;
            int adminCompanyUserID = CompanyDetails.AdminUser.CompanyUserID;
            int numberOfUserLicences = CompanyDetails.NumberOfUserLicences;
            string transactionPassword = CompanyDetails.AdminUser.Password;

            int numberOfRowsEffected = 0;
            ///DataSourceNameID dataSourceNameID = new DataSourceNameID();
            //string dbMessage = string.Empty;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMSaveCompanyDetails";
            queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@CompanyID",
                ParameterType = DbType.Int16,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@NumberOfUserLicences", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@NumberOfUserLicences",
                ParameterType = DbType.Int32,
                ParameterValue = numberOfUserLicences
            });
            queryData.DictionaryDatabaseParameter.Add("@AdminCompanyUserID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AdminCompanyUserID",
                ParameterType = DbType.Int32,
                ParameterValue = adminCompanyUserID
            });
            queryData.DictionaryDatabaseParameter.Add("@TransactionPassword", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@TransactionPassword",
                ParameterType = DbType.String,
                ParameterValue = transactionPassword
            });

            XMLSaveManager.AddOutErrorParameters(queryData);


            //string errorMessage = string.Empty;
            try
            {
                numberOfRowsEffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
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

            return numberOfRowsEffected;

        }

        /// <summary>
        /// Saves the company application details.
        /// </summary>
        /// <param name="companyApplicationDetails">The company application details.</param>
        /// <returns></returns>
        public static int SaveCompanyApplicationDetails(CompanyApplicationDetails companyApplicationDetails)
        {
            int companyID = companyApplicationDetails.CompanyNameID.ID;
            int riskModelID = companyApplicationDetails.PricingModel.ID;
            bool allowDailyImport = companyApplicationDetails.AllowDailyImport;

            int numberOfRowsEffected = 0;

            //string dbMessage = string.Empty;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMSaveCompanyApplicationDetails";
            queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@CompanyID",
                ParameterType = DbType.Int16,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@RiskModelID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@RiskModelID",
                ParameterType = DbType.Int32,
                ParameterValue = riskModelID
            });
            queryData.DictionaryDatabaseParameter.Add("@AllowDailyImport", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AllowDailyImport",
                ParameterType = DbType.Boolean,
                ParameterValue = allowDailyImport
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                numberOfRowsEffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
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

            // result = Convert.ToInt32(commandSP.Parameters["@Error"].Value);
            // dbMessage = Convert.ToString(commandSP.Parameters["@ErrorMessage"].Value);


            return numberOfRowsEffected;
        }

        /// <summary>
        /// Gets the application accounts for company.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static AccountList GetApplicationAccountsForCompany(int companyID, int thirdPartyID)
        {
            AccountList accountList = new AccountList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetApplicationAccountsForCompany";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountList.Add(FillAccount(row, 0));

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
            accountList.Insert(0, new Account(0, ApplicationConstants.C_COMBO_SELECT));
            //accountList.Insert(-1, new Account(0, ApplicationConstants.C_COMBO_SELECT));

            return accountList;
        }

        /// <summary>
        /// Fills the account.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Account FillAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Account account = null;

            if (row != null)
            {
                account = new Account();
                int ID = offset + 0;
                int fullName = offset + 1;
                int shortName = offset + 2;

                try
                {
                    account.ID = Convert.ToInt32(row[ID]);
                    account.FullName = Convert.ToString(row[fullName]);
                    if (row.Length > 2)
                    {
                        account.ShortName = Convert.ToString(row[shortName]);
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
            }

            return account;
        }

        /// <summary>
        /// Gets the application accounts for company.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static AccountList GetCompanyAccounts(int companyID)
        {
            AccountList accountList = new AccountList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAccountsForCompany";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountList.Add(FillAccount(row, 0));

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
            //accountList.Insert(0, new Account(0, ApplicationConstants.C_COMBO_SELECT));
            //accountList.Insert(-1, new Account(0, ApplicationConstants.C_COMBO_SELECT));

            return accountList;
        }

        /// <summary>
        /// Gets the data source company accounts.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="thirdPartyID">The data source ID.</param>
        /// <returns></returns>
        public static AccountList GetDataSourceCompanyAccounts(int companyID, int thirdPartyID)
        {
            AccountList accountList = new AccountList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetDataSourceCompanyAccounts";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);



                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountList.Add(FillAccount(row, 0));
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
            accountList.Insert(0, new Account(0, ApplicationConstants.C_COMBO_SELECT));

            return accountList;
        }

        public static MappingItemList GetApplicationAccountsDataForCompanyWithDataSource(int companyID, int thirdPartyID)
        {
            MappingItemList mappingItemList = new MappingItemList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetApplicationAccountsDataForCompanyWithDataSource";
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);

            DataSet productsDataSet = null;

            productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            XMLSaveManager.GetErrorParameterValues(ref CompanyManager._errorMessage, ref CompanyManager._errorNumber, queryData.DictionaryDatabaseParameter);

            try
            {
                mappingItemList = FillAccountMappings(productsDataSet);
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


            return mappingItemList;
        }

        /// <summary>
        /// Fills the account mappings.
        /// </summary>
        /// <param name="productsDataSet">The products data set.</param>
        /// <returns></returns>
        private static MappingItemList FillAccountMappings(DataSet productsDataSet)
        {
            MappingItemList list = null;


            const int DataSourceAccountID = 0;
            const int DataSourceAccountName = 1;
            const int AccountID = 2;


            if (productsDataSet != null && productsDataSet.Tables != null && productsDataSet.Tables.Count > 0 && productsDataSet.Tables[0].Rows != null && productsDataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dt = productsDataSet.Tables[0];
                list = new MappingItemList();

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MappingItem item = new MappingItem();

                        if (!(row[DataSourceAccountID] is System.DBNull))
                        {
                            int sourceId = int.Parse(row[DataSourceAccountID].ToString());
                            if (sourceId >= 0)
                            {
                                item.SourceItemID = sourceId;
                            }
                            else
                            {
                                continue;
                            }
                        }


                        if (!(row[DataSourceAccountName] is System.DBNull))
                        {
                            item.SourceItemName = row[DataSourceAccountName].ToString();
                        }
                        if (!(row[AccountID] is System.DBNull))
                        {
                            item.ApplicationItemId = int.Parse(row[AccountID].ToString());
                        }

                        list.Add(item);

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


            }
            return list;
        }


        /// <summary>
        /// Saves the account mappings.
        /// </summary>
        /// <param name="mappingItemList">The mapping item list.</param>
        /// <param name="companyID">The company ID.</param>
        /// <param name="thirdPartyID">The data source ID.</param>
        /// <returns></returns>
        public static int SaveAccountMappings(MappingItemList mappingItemList, int companyID, int thirdPartyID)
        {
            int result = 0;

            try
            {
                string xml = XMLUtilities.SerializeToXML(mappingItemList);
                result = CommonManager.SaveThroughXML("PMSaveAccountMappings", xml, thirdPartyID, companyID);
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

            return result;
        }
    }
}
