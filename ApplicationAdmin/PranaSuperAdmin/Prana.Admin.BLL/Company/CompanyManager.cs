#region Using

using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Compliance;
using Prana.BusinessObjects.Compliance.CompliancePref;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using Prana.PM.BLL;
//
#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyManager.
    /// </summary>
    public class CompanyManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        private CompanyManager()
        {
        }

        #region preferences

        //private DataTable SavePranaPreference()
        //{
        //    DataTable dt = new DataTable("dtPranaPref");
        //    dt.Columns.Add("PreferenceKey", typeof(string));
        //    dt.Columns.Add("PreferenceValue", typeof(int));
        //    dt.Rows.Add("IsFundManyToManyMappingAllowed", Convert.ToInt32(CachedDataManager.GetInstance.IsMasterFundManyToManyMappingAllowed()));
        //    dt.Rows.Add("IsStrategyManyToManyMappingAllowed", Convert.ToInt32(CachedDataManager.GetInstance.IsMasterStrategyManyToManyMappingAllowed()));
        //    dt.Rows.Add("IsNAVLockingEnabled", Convert.ToInt32(CachedDataManager.GetInstance.IsNAVLockingEnabled()));
        //    return dt;
        //}

        //Added by: Bharat raturi, 123 apr 2014
        //purpose: to save the mapping and nav lock preferences in the db
        /// <summary>
        /// Save the mapping and nav lock preferences in db
        /// </summary>
        /// <param name="isMasterFundManyToMany">true if many to many mapping enabled</param>
        /// <param name="isMasterStrategyManyToMany">true if many to many mapping enabled</param>
        /// <param name="isnavLockEnabled">true if nav locking enabled</param>
        /// <returns></returns>
        public static int SavePranaPreferencesinDB(bool isMasterFundManyToMany, bool isMasterStrategyManyToMany, bool isnavLockEnabled, bool isFeederAccountEnabled) //DataTable dt)
        {
            int i = 0;
            try
            {
                DataSet ds = new DataSet("dsPranaPref");

                DataTable dt = new DataTable("dtPranaPref");
                dt.Columns.Add("PreferenceKey", typeof(string));
                dt.Columns.Add("PreferenceValue", typeof(int));
                dt.Rows.Add("IsFundManyToManyMappingAllowed", Convert.ToInt32(isMasterFundManyToMany));
                dt.Rows.Add("IsStrategyManyToManyMappingAllowed", Convert.ToInt32(isMasterStrategyManyToMany));
                dt.Rows.Add("IsNAVLockingEnabled", Convert.ToInt32(isnavLockEnabled));
                dt.Rows.Add("IsFeederFundEnabled", Convert.ToInt32(isFeederAccountEnabled));
                ds.Tables.Add(dt);
                String xmlPranaPref = ds.GetXml();

                string sProc = "P_SavePranaKeyValuePreferences";
                object[] param = { xmlPranaPref };
                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, param);
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
            return i;
        }
        #endregion

        #region Company

        public static Company FillCompany(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int name = 1 + offSet;
            int address1 = 2 + offSet;
            int address2 = 3 + offSet;
            int companyTypeID = 4 + offSet;
            int telephone = 5 + offSet;
            int fax = 6 + offSet;
            int primaryContactFirstName = 7 + offSet;
            int primaryContactLastName = 8 + offSet;
            int primaryContactTitle = 9 + offSet;
            int primaryContactEMail = 10 + offSet;
            int primaryContactTelephone = 11 + offSet;
            int primaryContactCell = 12 + offSet;

            int shortName = 13 + offSet;
            int login = 14 + offSet;
            int password = 15 + offSet;
            int countryID = 16 + offSet;
            int stateID = 17 + offSet;
            int zip = 18 + offSet;
            int baseCurrencyID = 19 + offSet;
            int supportsMultipleCurrencies = 20 + offSet;
            int city = 21 + offSet;

            //modified by: Bharat Raturi, date: 05/Mar/2014
            //purpose: bind the data to the 'Region' field
            int region = 22 + offSet;

            //modified by: Bhavana, date: 28/April/2014
            //purpose: bind the data to the 'timeZone' and 'EmailAlert' field
            int timeZone = 23 + offSet;
            int emailAlert = 24 + offSet;
            int sendAllocationsViaFix = 25 + offSet;

            Company company = new Company();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    company.CompanyID = int.Parse(row[ID].ToString());
                }
                if (row[name] != System.DBNull.Value)
                {
                    company.Name = row[name].ToString();
                }
                if (row[address1] != System.DBNull.Value)
                {
                    company.Address1 = row[address1].ToString();
                }
                if (row[address2] != System.DBNull.Value)
                {
                    company.Address2 = row[address2].ToString();
                }
                if (row[companyTypeID] != System.DBNull.Value)
                {
                    company.CompanyTypeID = int.Parse(row[companyTypeID].ToString());
                }
                if (row[telephone] != System.DBNull.Value)
                {
                    company.Telephone = row[telephone].ToString();
                }
                if (row[fax] != System.DBNull.Value)
                {
                    company.Fax = row[fax].ToString();
                }
                if (row[primaryContactFirstName] != System.DBNull.Value)
                {
                    company.PrimaryContactFirstName = row[primaryContactFirstName].ToString();
                }
                if (row[primaryContactLastName] != System.DBNull.Value)
                {
                    company.PrimaryContactLastName = row[primaryContactLastName].ToString();
                }
                if (row[primaryContactTitle] != System.DBNull.Value)
                {
                    company.PrimaryContactTitle = row[primaryContactTitle].ToString();
                }
                if (row[primaryContactEMail] != System.DBNull.Value)
                {
                    company.PrimaryContactEMail = row[primaryContactEMail].ToString();
                }
                if (row[primaryContactTelephone] != System.DBNull.Value)
                {
                    company.PrimaryContactTelephone = row[primaryContactTelephone].ToString();
                }
                if (row[primaryContactCell] != System.DBNull.Value)
                {
                    company.PrimaryContactCell = row[primaryContactCell].ToString();
                }

                if (row[shortName] != System.DBNull.Value)
                {
                    company.ShortName = row[shortName].ToString();
                }
                if (row[login] != System.DBNull.Value)
                {
                    company.LoginName = row[login].ToString();
                }
                if (row[password] != System.DBNull.Value)
                {
                    company.Password = row[password].ToString();
                }
                if (row[countryID] != System.DBNull.Value)
                {
                    company.CountryID = int.Parse(row[countryID].ToString());
                }
                if (row[stateID] != System.DBNull.Value)
                {
                    company.StateID = int.Parse(row[stateID].ToString());
                }
                if (row[zip] != System.DBNull.Value)
                {
                    company.Zip = row[zip].ToString();
                }
                if (row[baseCurrencyID] != System.DBNull.Value)
                {
                    company.BaseCurrencyID = int.Parse(row[baseCurrencyID].ToString());
                }
                if (row[supportsMultipleCurrencies] != System.DBNull.Value)
                {
                    company.SupportsMultipleCurrency = int.Parse(row[supportsMultipleCurrencies].ToString());
                }
                if (row[city] != System.DBNull.Value)
                {
                    company.City = row[city].ToString();
                }
                if (row[timeZone] != System.DBNull.Value)
                {
                    company.TimeZone = int.Parse(row[timeZone].ToString());
                }
                if (row[emailAlert] != System.DBNull.Value)
                {
                    company.EmailAlert = row[emailAlert].ToString();
                }
                //modified by: Bharat Raturi, date: 05/Mar/2014
                //purpose: bind the data to the 'Region' field
                if (row[region] != System.DBNull.Value)
                {
                    company.Region = row[region].ToString();
                }
                if (row[sendAllocationsViaFix] != System.DBNull.Value)
                {
                    company.SendAllocationsViaFix = Convert.ToBoolean(row[sendAllocationsViaFix]);
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
            return company;
        }

        /// <summary>
        /// Adds/Updates <see cref="Company"/> into database.
        /// </summary>
        /// <param name="company"><see cref="Company"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static int SaveCompany(Company company)
        {
            //TODO: Write SP
            int result = int.MinValue;

            object[] parameter = new object[26];

            parameter[0] = company.CompanyID;
            parameter[1] = company.Address1;
            parameter[2] = company.Address2;
            parameter[3] = company.CompanyTypeID;
            parameter[4] = company.Fax;
            parameter[5] = company.Name;

            parameter[6] = company.PrimaryContactCell;
            parameter[7] = company.PrimaryContactEMail;
            parameter[8] = company.PrimaryContactFirstName;
            parameter[9] = company.PrimaryContactLastName;
            parameter[10] = company.PrimaryContactTelephone;
            parameter[11] = company.PrimaryContactTitle;

            parameter[12] = company.Telephone;

            parameter[13] = company.ShortName;
            parameter[14] = company.LoginName;
            parameter[15] = company.Password;
            parameter[16] = company.CountryID;
            parameter[17] = company.StateID;
            parameter[18] = company.Zip;
            parameter[19] = company.BaseCurrencyID;
            parameter[20] = company.SupportsMultipleCurrency;
            parameter[21] = company.City;

            //modified by: Bharat Raturi, Date: 05/Mar/2014
            //Purpose: Provide details of new field 'Region'
            parameter[22] = company.Region;

            //modified by: Bhavana, Date: 28/April/2014
            //Purpose: Provide details of new field 'Timezone' and 'EmailAlert'
            parameter[23] = company.TimeZone;
            parameter[24] = company.EmailAlert;
            parameter[25] = company.SendAllocationsViaFix;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyDetail", parameter).ToString());
                company.CompanyID = result;
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
        /// Deletes <see cref="Company"/> from the database.
        /// </summary>
        /// <param name="companyID">ID of the <see cref="Company"/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static bool DeleteCompany(int companyID, bool deleteForceFully)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = (deleteForceFully == true ? 1 : 0);

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompany", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyByID", parameter) > 0)
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
        /// Deletes <see cref="Company"/> from the database for CH Release.
        /// </summary>
        /// <param name="companyID">ID of the <see cref="Company"/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static string DeleteCompanyForCH(int companyID)
        {
            string result = String.Empty;
            Object[] parameter = new Object[1];
            parameter[0] = companyID;

            try
            {
                result = DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCompanyByIDForCH", parameter).ToString();
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
        /// Gets <see cref="Company"/> from the database.
        /// </summary>
        /// <param name="companyID">ID of <see cref="Company"/> to be fetched from database.</param>
        /// <returns><see cref="Company"/> fetched.</returns>
        public static Company GetCompany(int companyID)
        {
            Company company = new Company();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        company = FillCompany(row, 0);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                //					throw(ex);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return company;
        }

        /// <summary>
        /// Gets all <see cref="Companies"/> from datatbase.
        /// </summary>
        /// <returns><see cref="Companies"/> fetched.</returns>
        public static Companies GetCompanies()
        {
            Companies companies = new Companies();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companies.Add(FillCompany(row, 0));
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
            return companies;
        }

        #endregion

        #region CompanyMPID
        public static MPID FillCompanyMPID(object[] row, int offSet)
        {
            int COMPANYMPID = 0 + offSet;
            int COMPANYID = 1 + offSet;
            int MPID = 2 + offSet;

            MPID companyMPID = new MPID();
            try
            {

                if (!(row[COMPANYMPID] is System.DBNull))
                {
                    companyMPID.CompanyMPID = int.Parse(row[COMPANYMPID].ToString());
                }
                if (!(row[COMPANYID] is System.DBNull))
                {
                    companyMPID.CompanyID = int.Parse(row[COMPANYID].ToString());
                }
                if (!(row[MPID] is System.DBNull))
                {
                    companyMPID.MPIDName = row[MPID].ToString();
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
            return companyMPID;
        }


        public static MPID GetCompanyMPID(int companyMPID)
        {
            MPID mpid = new MPID();

            object[] parameter = new object[1];
            parameter[0] = companyMPID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyMPID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        mpid = FillCompanyMPID(row, 0);
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
            return mpid;
        }

        public static MPIDCollection GetCompanyMPIDs(int companyID)
        {
            //MPIDs companyMPIDs = new MPIDs();
            MPIDCollection companyMPIDs = new MPIDCollection();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyMPIDs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyMPIDs.Add(FillCompanyMPID(row, 0));
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
            return companyMPIDs;
        }

        public static bool SaveCompanyMPID(MPIDCollection mpids, int companyID)
        {
            int result = int.MinValue;
            StringBuilder mPIDStringBuilder = new StringBuilder();
            object[] parameter = new object[1];

            try
            {
                foreach (MPID mpid in mpids)
                {
                    parameter = new object[3];
                    parameter[0] = companyID;
                    parameter[1] = mpid.MPIDName;
                    parameter[2] = mpid.CompanyMPID;
                    if (mpid.MPIDName != "")
                    {
                        result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyMPID", parameter).ToString());
                    }

                    mPIDStringBuilder.Append("'");
                    mPIDStringBuilder.Append(result.ToString());
                    mPIDStringBuilder.Append("',");
                }
                //result = int.Parse(db.ExecuteScalar("P_SaveCompanyMPID", parameter).ToString());

                int len = mPIDStringBuilder.Length;
                if (mPIDStringBuilder.Length > 0)
                {
                    mPIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyID;
                parameter[1] = mPIDStringBuilder.ToString();
                if (mPIDStringBuilder.Length > 0)
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyMPIDs", parameter).ToString();
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

            return true;
        }
        public static bool DeleteCompanyMPID(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyMPID", parameter) > 0)
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

        public static bool DeleteCompanyMPIDByID(int companyMPID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyMPID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyMPIDByID", parameter) > 0)
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

        #region Company Type

        /// <summary>
        /// Fills <see cref="CompanyType"/>.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static CompanyType FillCompanyType(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int type = 1 + offSet;

            CompanyType companyType = new CompanyType();
            try
            {
                if (row[ID] != null)
                {
                    companyType.CompanyTypeID = int.Parse(row[ID].ToString());
                }
                if (row[type] != null)
                {
                    companyType.Type = row[type].ToString();
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
            return companyType;
        }

        /// <summary>
        /// Gets all <see cref="CompanyTypes"/> from database.
        /// </summary>
        /// <returns><see cref="CompanyTypes"/> fetched.</returns>
        public static CompanyTypes GetCompanyTypes()
        {
            CompanyTypes companyTypes = new CompanyTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyTypes.Add(FillCompanyType(row, 0));
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
            return companyTypes;
        }

        #endregion

        #region Clients

        /// <summary>
        /// Fills <see cref="Client"/>.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static Client FillClient(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int clientName = 1 + offSet;

            Client client = new Client();
            try
            {
                if (row[ID] != null)
                {
                    client.ClientID = int.Parse(row[ID].ToString());
                }
                if (row[clientName] != null)
                {
                    client.ClientName = row[clientName].ToString();
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
            return client;
        }

        /// <summary>
        /// Gets all <see cref="Clients"/> from database.
        /// </summary>
        /// <returns><see cref="Clients"/> fetched.</returns>
        public static Clients GetClients(int companyID)
        {
            Clients clients = new Clients();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClients", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clients.Add(FillClient(row, 0));
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
            return clients;
        }

        #endregion

        #region Permission Level Details

        public static AUEC FillCompanyAUEC(object[] row, int offSet)
        {
            int companyAUECID = 0 + offSet;
            int companyID = 1 + offSet;
            int auecID = 2 + offSet;
            int assetID = 3 + offSet;
            int underLyingID = 4 + offSet;
            int exchangeID = 5 + offSet;
            int currencyID = 6 + offSet;
            int displayName = 7 + offSet;

            AUEC companyAUEC = new AUEC();
            try
            {

                if (!(row[companyAUECID] is System.DBNull))
                {
                    companyAUEC.CompanyAUECID = int.Parse(row[companyAUECID].ToString());
                }
                if (!(row[companyID] is System.DBNull))
                {
                    companyAUEC.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (!(row[auecID] is System.DBNull))
                {
                    companyAUEC.AUECID = int.Parse(row[auecID].ToString());
                }
                if (!(row[assetID] is System.DBNull))
                {
                    companyAUEC.AssetID = int.Parse(row[assetID].ToString());
                }
                if (!(row[underLyingID] is System.DBNull))
                {
                    companyAUEC.UnderlyingID = int.Parse(row[underLyingID].ToString());
                }
                if (row[exchangeID] != null)
                {
                    companyAUEC.ExchangeID = int.Parse(row[exchangeID].ToString());
                }
                if (row[currencyID] != null)
                {
                    companyAUEC.CurrencyID = int.Parse(row[currencyID].ToString());
                }
                if (row[displayName] != null)
                {
                    companyAUEC.DisplayName = row[displayName].ToString();
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
            return companyAUEC;
        }

        /// <summary>
        /// This is a special method which gets the common AUEC's present for a particular CV and for a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns>The collection of <see cref="AUEC"/> objects.</returns>
        public static AUECs GetCompanyCVAUECs(int companyID)
        {
            AUECs companyCVAUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVAUECs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCVAUECs.Add(FillCompanyAUEC(row, 0));
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
            return companyCVAUECs;

        }

        public static AUECs GetCompanyAUECs(int companyID)
        {
            AUECs companyAUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyAUECs.Add(FillCompanyAUEC(row, 0));
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
            return companyAUECs;
        }
        /// <summary>
        /// to save the dollar amount permission in the database
        /// </summary>
        /// <param name="PTT"></param>
        /// <param name="TT"></param>
        public static void SaveDollarAmountPermission(bool PTT, bool TT)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = TT;
                parameter[1] = PTT;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveDollarAmountPermission", parameter).ToString();
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

        public static bool SaveCompanyAUECs(int companyID, AUECs aUECs)
        {
            int result = int.MinValue;
            StringBuilder auecIDStringBuilder = new StringBuilder();
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyAUECs", parameter).ToString();

                foreach (AUEC auec in aUECs)
                {
                    parameter = new object[3];
                    parameter[0] = auec.CompanyAUECID;
                    parameter[1] = companyID;
                    parameter[2] = auec.AUECID;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyAUECs", parameter).ToString());

                    auecIDStringBuilder.Append("'");
                    auecIDStringBuilder.Append(result.ToString());
                    auecIDStringBuilder.Append("',");
                }

                int len = auecIDStringBuilder.Length;
                if (auecIDStringBuilder.Length > 0)
                {
                    auecIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyID;
                parameter[1] = auecIDStringBuilder.ToString();
                //				if(auecIDStringBuilder.Length > 0)
                //				{
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyAUEC", parameter).ToString();
                //				}	
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
            return true;
        }


        public static bool SaveClientAUECs(int clientID, AUECs aUECs)
        {
            object[] parameterdelete = new object[1];
            object[] parameter = new object[2];
            parameterdelete[0] = clientID;
            parameter[0] = clientID;
            try
            {

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllClientAUEC", parameterdelete);
                foreach (AUEC auec in aUECs)
                {

                    parameter[1] = auec.AUECID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveClientAUEC", parameter).ToString();
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
            return true;
        }


        public static bool SaveCompanyModules(int companyID, Modules modules)
        {
            int result = int.MinValue;
            StringBuilder moduleIDStringBuilder = new StringBuilder();
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyModules", parameter).ToString();

                foreach (Module module in modules)
                {
                    parameter = new object[4];
                    parameter[0] = companyID;
                    parameter[1] = module.ModuleID;
                    parameter[2] = module.CompanyModuleID;
                    parameter[3] = module.ReadWriteID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyModules", parameter).ToString());

                    //moduleIDStringBuilder.Append("'");
                    moduleIDStringBuilder.Append(result.ToString());
                    moduleIDStringBuilder.Append(",");
                }

                int len = moduleIDStringBuilder.Length;
                if (moduleIDStringBuilder.Length > 0)
                {
                    moduleIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyID;
                parameter[1] = moduleIDStringBuilder.ToString();
                //				if(moduleIDStringBuilder.Length > 0)
                //				{
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyModules", parameter).ToString();
                //				}	

                //Delete/Update all those Modules of 'company users' which are now not present for company.
                parameter = new object[1];
                parameter[0] = companyID;
                //				db.ExecuteNonQuery("P_UpdateCompanyUserModules", parameter);
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
            return true;
        }
        /// <summary>
        /// save companyId, userId,pretradecheck and override permission in T_CA_pretradecheck 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyUserId"></param>
        /// <param name="preTradeCheck"></param>
        /// <param name="overrideAllowed"></param>
        /// <returns></returns>
        public static bool SaveOverridePowerUserAndPreTradeCheckPermission(int companyId, int companyUserId, bool preTradeCheck, bool overrideAllowed, bool powerUser, bool applyToManual)
        {
            try
            {
                object[] parameter = new object[6];
                parameter[0] = companyId;
                parameter[1] = companyUserId;
                parameter[2] = Convert.ToInt16(preTradeCheck);
                parameter[3] = Convert.ToInt16(overrideAllowed);
                parameter[4] = Convert.ToInt16(powerUser);
                parameter[5] = Convert.ToInt16(applyToManual);
                int result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveOverridePreCheckAndPowerUser", parameter).ToString());
                if (result > 0)
                    return true;
                else return false;
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
                return false;
            }
        }


        public static bool SaveCompanyModulesForUser(int companyUserID, Modules modules)
        {
            int result = int.MinValue;

            StringBuilder moduleIDStringBuilder = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyUserModules", parameter).ToString();

                foreach (Module module in modules)
                {
                    parameter = new object[4];
                    parameter[0] = companyUserID;
                    parameter[1] = module.CompanyModuleID;
                    parameter[2] = module.ReadWriteID;
                    parameter[3] = module.IsShowExport;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyModulesForUser", parameter).ToString());

                    //moduleIDStringBuilder.Append("'");
                    moduleIDStringBuilder.Append(result.ToString());
                    moduleIDStringBuilder.Append(",");
                }

                int len = moduleIDStringBuilder.Length;
                if (moduleIDStringBuilder.Length > 0)
                {
                    moduleIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyUserID;
                parameter[1] = moduleIDStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserModules", parameter).ToString();

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
            return true;
        }

        public static bool SaveCompanyAssetsForUser(int userID, Assets assets)
        {
            //TODO: Write Delete also

            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserAssets", parameter).ToString();
                foreach (Asset asset in assets)
                {
                    parameter = new object[2];
                    parameter[0] = userID;
                    parameter[1] = asset.AssetID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyUserAsset", parameter).ToString();
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
            return true;
        }

        public static bool SaveCompanyCounterPartyVenues(int companyID, CounterPartyVenues counterPartyVenues)
        {
            int result = int.MinValue;

            object[] parameter = new object[1];
            //object[] parameterNew = new object[1];

            try
            {
                //				foreach(CounterPartyVenue counterPartyVenue in counterPartyVenues)
                //				{
                //					parameter[0] = companyID;
                //					parameter[1] = counterPartyVenue.CounterPartyID;
                //					parameter[2] = counterPartyVenue.VenueID;
                //					db.ExecuteNonQuery("P_DeleteCompanyCounterPartyVenues", parameter).ToString();
                //				}
                StringBuilder companyCounterPartyCVIDStringBuilder = new StringBuilder();
                foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                {
                    parameter = new object[3];
                    parameter[0] = companyID;
                    parameter[1] = counterPartyVenue.CounterPartyID;
                    parameter[2] = counterPartyVenue.VenueID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCounterPartyVenuesPermissions", parameter).ToString());

                    companyCounterPartyCVIDStringBuilder.Append("'");
                    companyCounterPartyCVIDStringBuilder.Append(result.ToString());
                    companyCounterPartyCVIDStringBuilder.Append("',");
                }

                int len = companyCounterPartyCVIDStringBuilder.Length;
                if (companyCounterPartyCVIDStringBuilder.Length > 0)
                {
                    companyCounterPartyCVIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyID;
                parameter[1] = companyCounterPartyCVIDStringBuilder.ToString();
                //				if(companyCounterPartyCVIDStringBuilder.Length > 0)
                //				{
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCounterPartyVenues", parameter).ToString();
                //				}
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
            return true;
        }


        public static bool SaveCompanyUnderlyingsForUser(int userID, UnderLyings underLyings)
        {
            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserUnderLyings", parameter).ToString();
                foreach (UnderLying underLying in underLyings)
                {
                    parameter = new object[2];
                    parameter[0] = userID;
                    parameter[1] = underLying.UnderlyingID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyUserUnderLying", parameter).ToString();
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
            return true;
        }

        public static bool SaveCompanyThirdParty(int companyID, ThirdParties thirdParties)
        {
            int result = int.MinValue;
            StringBuilder thirdPartyIDStringBuilder = new StringBuilder();
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyThirdParty", parameter).ToString();

                foreach (ThirdParty thirdParty in thirdParties)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = thirdParty.ThirdPartyID;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyThirdParty", parameter).ToString());

                    thirdPartyIDStringBuilder.Append("'");
                    thirdPartyIDStringBuilder.Append(result.ToString());
                    thirdPartyIDStringBuilder.Append("',");
                }
                int len = thirdPartyIDStringBuilder.Length;
                if (thirdPartyIDStringBuilder.Length > 0)
                {
                    thirdPartyIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyID;
                parameter[1] = thirdPartyIDStringBuilder.ToString();
                //				if(thirdPartyIDStringBuilder.Length > 0)
                //				{
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyThirdParties", parameter).ToString();
                //				}
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
            return true;
        }

        #endregion

        #region AccountType
        public static AccountType FillAccountType(object[] row, int offSet)
        {
            int accountTypeID = 0 + offSet;
            int accountTypeName = 1 + offSet;

            AccountType accountType = new AccountType();
            try
            {
                if (row[accountTypeID] != null)
                {
                    accountType.AccountTypeID = int.Parse(row[accountTypeID].ToString());
                }
                if (row[accountTypeName] != null)
                {
                    accountType.AccountTypeName = row[accountTypeName].ToString();
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
            return accountType;
        }
        public static AccountTypes GetAccountTypes()
        {
            AccountTypes accountTypes = new AccountTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFundTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountTypes.Add(FillAccountType(row, 0));
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
            return accountTypes;
        }


        public static AccountType GetsAccountType(int accountTypeId)
        {
            AccountType accountType = new AccountType();
            object[] parameter = new object[1];
            parameter[0] = accountTypeId;

            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFundType", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountType = FillAccountType(row, 0);
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
            return accountType;
        }
        #endregion

        #region Internal Accounts

        #region Accounts
        public static Account FillAccount(object[] row, int offSet)
        {
            int accountID = 0 + offSet;
            int accountName = 1 + offSet;
            int accountShortName = 2 + offSet;
            int companyID = 3 + offSet;

            Account account = new Account();
            try
            {
                if (row[accountID] != null)
                {
                    account.CompanyAccountID = int.Parse(row[accountID].ToString());
                }
                if (row[accountName] != null)
                {
                    account.AccountName = row[accountName].ToString();
                }
                if (row[accountShortName] != null)
                {
                    account.AccountShortName = row[accountShortName].ToString();
                }
                if (row[companyID] != null)
                {
                    account.CompanyID = int.Parse(row[companyID].ToString());
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
            return account;
        }
        public static Accounts GetAccounts()
        {
            Accounts accounts = new Accounts();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyFunds";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accounts.Add(FillAccount(row, 0));
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
            return accounts;
        }

        public static Account GetsAccount(int accountID)
        {
            Account account = new Account();

            object[] parameter = new object[1];
            parameter[0] = accountID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFund", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        account = FillAccount(row, 0);
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
            return account;
        }

        public static DataSet GetDollarAmountPermission()
        {
            DataSet result = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetDollarAmountPermission";

            try
            {
                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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

        public static Account GetAccountDetail(string accountName)
        {
            Account account = new Account();

            object[] parameter = new object[1];
            parameter[0] = accountName;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFundDetail", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        account = FillAccount(row, 0);
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
            return account;
        }
        public static Accounts GetAccount(int companyID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            Accounts accounts = new Accounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyFunds", parameter))
                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetCompanyFundsNotAllocatedToanyParty", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accounts.Add(FillAccount(row, 0));
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
            return accounts;
        }
        public static bool DeleteAccount(int accountID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = accountID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyFund", parameter) > 0)
                if ((int.Parse((DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCompanyFundByID", parameter).ToString()))) > 0)
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
        public static int SaveAccount(Accounts accounts, int companyID)
        {
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //				StringBuilder companyAccountIDStringBuilder = new StringBuilder();
                //db.ExecuteNonQuery("P_DeleteCompanyFunds", parameter).ToString();
                parameter = new object[5];
                foreach (Account account in accounts)
                {
                    parameter[0] = account.CompanyAccountID; //account.AccountID;
                    parameter[1] = account.AccountName;
                    parameter[2] = account.AccountShortName;
                    parameter[3] = companyID;
                    parameter[4] = 1; //account.AccountTypeID have to stored but hard codded for now.

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyFund", parameter).ToString());
                    account.CompanyAccountID = result;

                    //					companyAccountIDStringBuilder.Append("'");
                    //					companyAccountIDStringBuilder.Append(result.ToString());
                    //					companyAccountIDStringBuilder.Append("',");
                }

                //				int len = companyAccountIDStringBuilder.Length;
                //				if(companyAccountIDStringBuilder.Length > 0)
                //				{
                //					companyAccountIDStringBuilder.Remove((len-1), 1);
                //				}
                //				parameter = new object[2];
                //				parameter[0] = companyID;
                //				parameter[1] = companyAccountIDStringBuilder.ToString();
                //				if(companyAccountIDStringBuilder.Length > 0)
                //				{
                //					db.ExecuteNonQuery("P_DeleteCompanyTradingAccounts", parameter).ToString();
                //				}
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
        /// method created by Kanupriya:for RM TreeView. 
        /// The method is used to fetch the CompanyAccounts for which the data exists in RMAccount Table too.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Accounts GetCommonAccounts(int companyID)
        {
            Accounts accounts = new Accounts();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMCommonFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accounts.Add(FillAccount(row, 0));
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
            return accounts;

        }
        #endregion

        #region Trading Accounts
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
        public static TradingAccounts GetTradingAccounts()
        {
            TradingAccounts tradingAccounts = new TradingAccounts();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyTradingAccounts";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccounts.Add(FillTradingAccount(row, 0));
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
            return tradingAccounts;
        }
        public static TradingAccounts GetTradingAccount(int companyID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            TradingAccounts tradingAccounts = new TradingAccounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyTradingAccounts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccounts.Add(FillTradingAccount(row, 0));
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
            return tradingAccounts;
        }

        public static TradingAccounts GetTradingAccountsForCompany(int companyID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            TradingAccounts tradingAccounts = new TradingAccounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyTradingAccountsForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccounts.Add(FillTradingAccount(row, 0));
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
            return tradingAccounts;
        }

        public static TradingAccounts GetTradingAccountsForUser(int userID)
        {
            object[] parameter = new object[1];
            parameter[0] = userID;
            TradingAccounts tradingAccounts = new TradingAccounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingAccountsForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccounts.Add(FillTradingAccount(row, 0));
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
            return tradingAccounts;
        }

        public static bool DeleteTradingAccount(int tradingAccountsID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = tradingAccountsID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyTradingAccount", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyTradingAccountByID", parameter) > 0)
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
        public static int SaveTradingAccount(TradingAccounts tradingAccounts, int companyID)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyTradingAccounts", parameter).ToString();
                StringBuilder tradingAccountIDStringBuilder = new StringBuilder();
                parameter = new object[4];
                if (tradingAccounts != null && tradingAccounts.Count == 0)
                {
                    return 1;
                }
                foreach (TradingAccount tradingAccount in tradingAccounts)
                {
                    parameter[0] = tradingAccount.TradingAccountsID;
                    parameter[1] = tradingAccount.TradingAccountName;
                    parameter[2] = tradingAccount.TradingShortName;
                    parameter[3] = companyID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyTradingAccount", parameter).ToString());
                    tradingAccount.TradingAccountsID = result;
                    tradingAccountIDStringBuilder.Append("'");
                    tradingAccountIDStringBuilder.Append(result.ToString());
                    tradingAccountIDStringBuilder.Append("',");
                }
                int len = tradingAccountIDStringBuilder.Length;
                if (tradingAccountIDStringBuilder.Length > 0)
                {
                    tradingAccountIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];
                //				foreach(TradingAccount tradingAccount in tradingAccounts)
                //				{
                //					parameter[0] = tradingAccount.TradingAccountsID;
                //					parameter[1] = companyID;
                //					db.ExecuteNonQuery("P_DeleteCompanyTradingAccounts", parameter).ToString();
                //				}
                parameter[0] = companyID;
                parameter[1] = tradingAccountIDStringBuilder.ToString();
                if (tradingAccountIDStringBuilder.Length > 0)
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyTradingAccounts", parameter).ToString();
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

        public static bool SaveTradingAccountsForUser(int userID, TradingAccounts tradingAccounts)
        {
            bool result = true;
            //Modified By Faisal Shah 08/08/14
            //Trading Accounts were being overwritten in case of CH Release
            if (tradingAccounts.Count > 0)
            {
                int resultSave = int.MinValue;
                StringBuilder taIDStringBuilder = new StringBuilder();

                object[] parameter = new object[1];
                parameter[0] = userID;

                try
                {
                    //db.ExecuteNonQuery("P_DeleteTradingAccountsUser", parameter).ToString();
                    foreach (TradingAccount tradingAccount in tradingAccounts)
                    {
                        parameter = new object[2];
                        parameter[0] = tradingAccount.TradingAccountsID;
                        parameter[1] = userID;

                        resultSave = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveTradingAccountsForUser", parameter).ToString());

                        taIDStringBuilder.Append("'");
                        taIDStringBuilder.Append(resultSave.ToString());
                        taIDStringBuilder.Append("',");
                    }

                    int len = taIDStringBuilder.Length;
                    if (taIDStringBuilder.Length > 0)
                    {
                        taIDStringBuilder.Remove((len - 1), 1);
                    }
                    parameter = new object[2];

                    parameter[0] = userID;
                    parameter[1] = taIDStringBuilder.ToString();

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteTradingAccountsUser", parameter).ToString();

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
            return result;
        }

        /// <summary>
        /// Kanupriya: This method is used to fetch the details of a particular trading account.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static TradingAccount GetTradingAccountDetail(int companyID, int tradingAccontID)
        {
            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = tradingAccontID;
            TradingAccount tradingAccount = new TradingAccount();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingAccountDetail", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccount = FillTradingAccount(row, 0);
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
            return tradingAccount;
        }
        #endregion

        /// <summary>
        /// Gets override permissions and pre-trade check enabled for a particular user Id
        /// </summary>
        /// <param name="companyUserId">Id of the user company specific</param>
        /// <param name="companyId">Id of the company</param>
        /// <returns></returns>
        public static CompliancePermissions GetPermmissionForUser(int companyUserId, int companyId)
        {
            try
            {
                object[] parameter = new object[2];
                object[] parameter1 = new object[1];

                parameter[0] = companyId;
                parameter[1] = companyUserId;
                parameter1[0] = companyUserId;

                //Gets data from T_CA_UserReadWritePermission and T_CA_OtherCompliancePermission for companyId and userId
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetCompliancePermissionsUser", parameter);
                //Getting data for individual rule permission
                DataSet dsOverridenPermission = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetRuleOverRiddenPermission", parameter1);

                CompliancePermissions compliancePermissions = new CompliancePermissions();
                if ((ds.Tables[0].Rows.Count) > 0)
                {
                    compliancePermissions = new CompliancePermissions(ds.Tables[0].Rows[0]);
                }
                else if (ds.Tables[0].Rows.Count == 0)
                {
                    compliancePermissions.CompanyId = companyId;
                    compliancePermissions.CompanyUserId = companyUserId;
                    compliancePermissions.IsPowerUser = false;
                    compliancePermissions.EnableBasketComplianceCheck = false;

                    compliancePermissions.RuleCheckPermission.IsApplyToManual = false;
                    compliancePermissions.RuleCheckPermission.IsOverridePermission = false;
                    compliancePermissions.RuleCheckPermission.IsPreTradeEnabled = false;

                    compliancePermissions.complianceUIPermissions.Add(RuleType.PreTrade, new ComplianceUIPermissions());
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsCreate = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsRename = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsExport = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsEnable = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsDelete = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsImport = false;

                    compliancePermissions.complianceUIPermissions.Add(RuleType.PostTrade, new ComplianceUIPermissions());
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsCreate = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsRename = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsExport = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsEnable = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsDelete = false;
                    compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsImport = false;

                    compliancePermissions.RuleCheckPermission.DefaultOverRideType = RuleOverrideType.Soft;
                    compliancePermissions.RuleCheckPermission.DefaultPostPopUpEnabled = true;
                    compliancePermissions.RuleCheckPermission.DefaultPrePopUpEnabled = true;
                }
                else return null;

                //update pre override permission
                string postExclude = "PostTrade";
                var preFilteredRows = from row in dsOverridenPermission.Tables[0].AsEnumerable()
                                      where !postExclude.Contains(row.Field<string>("PackageName"))
                                      select row;

                if (preFilteredRows != null && preFilteredRows.Count() > 0)
                    compliancePermissions.PreRuleLevelPermissions = preFilteredRows.CopyToDataTable();
                else
                    compliancePermissions.PreRuleLevelPermissions = GetDefaultRuleLevelPermissionTable();

                //update pre override permission
                string preExclude = "PreTrade";
                var postFilteredRows = from row in dsOverridenPermission.Tables[0].AsEnumerable()
                                       where !preExclude.Contains(row.Field<string>("PackageName"))
                                       select row;

                if (postFilteredRows != null && postFilteredRows.Count() > 0)
                    compliancePermissions.PostRuleLevelPermissions = postFilteredRows.CopyToDataTable();
                else
                    compliancePermissions.PostRuleLevelPermissions = GetDefaultRuleLevelPermissionTable();

                if ((dsOverridenPermission.Tables[0].Rows.Count) > 0)
                {
                    foreach (DataRow row in dsOverridenPermission.Tables[0].Rows)
                    {
                        if (row["PackageName"].ToString().Equals("PreTrade"))
                            compliancePermissions.RuleLevelPermission.Add(new RuleLevelPermission(row["RuleId"].ToString(), Convert.ToBoolean(row["AlertTypePermission"]), row["RuleName"].ToString(), RuleOverrideType.Soft));
                    }
                }
                return compliancePermissions;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the default rule level permission table.
        /// </summary>
        /// <returns></returns>
        private static DataTable GetDefaultRuleLevelPermissionTable()
        {
            DataTable rulePermission = new DataTable();
            try
            {
                rulePermission.Columns.Add("RuleId");
                rulePermission.Columns.Add("RuleName");
                rulePermission.Columns.Add("AlertTypePermission");
                rulePermission.Columns.Add("PopUp");
                rulePermission.Columns.Add("PackageName");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rulePermission;
        }



        /// <summary>
        /// saves the comma separated ignored users string to db
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <param name="ignoredUsersCommaSeparated"></param>
        /// <returns></returns>
        public static bool SaveAuditTrailIgnoredUsers(int companyID, int companyUserID, string ignoredUsersCommaSeparated)
        {
            bool result = false;

            object[] parameter = new object[3];
            try
            {
                parameter[0] = companyUserID;
                parameter[1] = companyID;
                parameter[2] = ignoredUsersCommaSeparated;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAuditTrailIgnoredUsers", parameter).ToString();
                result = true;
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
        /// gets the ignored users for Audit trail from db
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="CompanyId"></param>
        /// <returns>string of comma separated userids to ignore</returns>
        public static string GetIgnoredUserForAuditTrail(int companyUserId, int CompanyId)
        {
            object[] parameter = new object[2];
            DataSet ds = new DataSet();
            try
            {
                parameter[0] = companyUserId;
                parameter[1] = CompanyId;

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAuditTrailIgnoredUsers", parameter);
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
            if ((ds.Tables[0].Rows.Count) > 0)
            {
                return ds.Tables[0].Rows[0]["IgnoredUsers"].ToString();
            }
            else
                return "";
        }

        #region CompanyUserAllocationTradingAccounts

        public static TradingAccounts GetAllocationTradingAccountsForUser(int companyUserID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            TradingAccounts tradingAccountsAllocation = new TradingAccounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllocationTradingAccountsForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccountsAllocation.Add(FillTradingAccount(row, 0));
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
            return tradingAccountsAllocation;
        }

        public static bool SaveAllocationTradingAccountsForUser(int companyUserID, TradingAccounts tradingAccountsAllocation)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllocationTradingAccountsUser", parameter).ToString();
                foreach (TradingAccount tradingAccount in tradingAccountsAllocation)
                {
                    parameter = new object[2];
                    parameter[0] = tradingAccount.TradingAccountsID;
                    parameter[1] = companyUserID;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAllocationTradingAccountsForUser", parameter).ToString();
                }
                result = true;
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

        #region Strategies

        public static Strategy FillStrategy(object[] row, int offSet)
        {
            int strategyID = 0 + offSet;
            int strategyName = 1 + offSet;
            int strategyShortName = 2 + offSet;
            int companyID = 3 + offSet;

            Strategy strategy = new Strategy();
            try
            {
                if (row[strategyID] != null)
                {
                    strategy.StrategyID = int.Parse(row[strategyID].ToString());
                }
                if (row[strategyName] != null)
                {
                    strategy.StrategyName = row[strategyName].ToString();
                }
                if (row[strategyShortName] != null)
                {
                    strategy.StrategyShortName = row[strategyShortName].ToString();
                }
                if (row[companyID] != null)
                {
                    strategy.CompanyID = int.Parse(row[companyID].ToString());
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
            return strategy;
        }
        public static Strategies GetStrategies()
        {
            Strategies strategies = new Strategies();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyStrategies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        strategies.Add(FillStrategy(row, 0));
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
            return strategies;
        }
        public static Strategies GetStrategy(int companyID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            Strategies strategies = new Strategies();

            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyStrategies", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        strategies.Add(FillStrategy(row, 0));
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
            return strategies;
        }

        public static Strategy GetsStrategy(int strategyID)
        {
            Strategy strategy = new Strategy();
            object[] parameter = new object[1];
            parameter[0] = strategyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetStrategy", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        strategy = FillStrategy(row, 0);
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
            return strategy;
        }
        // Modified By : Manvendra P.
        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8501
        public static bool DeleteStrategy(int strategyID, bool isDeletedForceFully)
        {
            bool result = false;
            int i = int.MinValue;
            Object[] parameter = new object[2];
            parameter[0] = strategyID;
            parameter[1] = (isDeletedForceFully == true ? 1 : 0);

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyStrategy", parameter) > 0)
                i = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCompanyStrategyByID", parameter).ToString());
                if (i > 0)
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

        public static String SaveStrategy(Strategies strategies, int companyID, ref Dictionary<int, List<int>> strategyAuditStatus)
        {
            int result = int.MinValue;
            StringBuilder errorMessage = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                int _newStrategyID = 1;
                //db.ExecuteNonQuery("P_DeleteCompanyStrategies", parameter).ToString();
                parameter = new object[4];
                _newStrategyID = GetMaxStrategyID();
                foreach (Strategy strategy in strategies)
                {
                    //Modified By Faisal Shah
                    //Dated 07/07/14
                    if (strategy.StrategyID < 0)
                    {
                        strategyAuditStatus[1].Remove(strategy.StrategyID);
                        strategyAuditStatus[1].Add(_newStrategyID);
                        strategy.StrategyID = _newStrategyID;
                        _newStrategyID++;
                    }
                    parameter[0] = strategy.StrategyID;
                    parameter[1] = strategy.StrategyName;
                    parameter[2] = strategy.StrategyShortName;
                    parameter[3] = companyID;


                    //result = int.Parse(db.ExecuteScalar("P_SaveCompanyStrategy", parameter).ToString());
                    //strategy.StrategyID = result;
                    //Modified By: Bharat Raturi, 14-03-2014
                    //purpose: Save the Strategy Details in the DataBase
                    //Modified By Faisal Shah 06/08/14
                    //Needed to return Values on the basis of existing Strategies in Inactive State
                    object value = DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyStrategy", parameter);
                    result = value == null ? 0 : (int)value;
                    if (result < 0)
                    {
                        if (result == -3)
                        {
                            errorMessage.Append("strategy already exists with name: " + strategy.StrategyName + '\n');
                        }
                        else if (result == -4)
                        {
                            errorMessage.Append("strategy already exists with short name: " + strategy.StrategyShortName + "\n");
                        }
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
            //Returns all Strategy Names and ShortNames that are Duplicate and in InActive State
            return errorMessage.ToString();
        }

        /// <summary>
        /// Added By Faisal Shah
        /// Dated 07/07/14
        /// Gets Maximum StrategyID from DB
        /// </summary>
        /// <returns>Maximum Strategy ID</returns>
        private static int GetMaxStrategyID()
        {
            int maxStrategyID = 1;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxStrategyID";

                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData, "PranaConnectionString");
                if (idValue != DBNull.Value)
                {
                    maxStrategyID = Convert.ToInt32(idValue);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return maxStrategyID;
        }
        #endregion

        #region ClearingFirmsPrimeBrokers
        public static ClearingFirmPrimeBroker FillClearingFirmPrimeBroker(object[] row, int offSet)
        {
            int clearingFirmsPrimeBrokersID = 0 + offSet;
            int clearingFirmsPrimeBrokersName = 1 + offSet;
            int clearingFirmsPrimeBrokersShortName = 2 + offSet;
            int companyID = 3 + offSet;

            ClearingFirmPrimeBroker clearingFirmPrimeBroker = new ClearingFirmPrimeBroker();
            try
            {
                if (row[clearingFirmsPrimeBrokersID] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID = int.Parse(row[clearingFirmsPrimeBrokersID].ToString());
                }
                if (row[clearingFirmsPrimeBrokersName] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = row[clearingFirmsPrimeBrokersName].ToString();
                }
                if (row[clearingFirmsPrimeBrokersShortName] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = row[clearingFirmsPrimeBrokersShortName].ToString();
                }
                if (row[companyID] != null)
                {
                    clearingFirmPrimeBroker.CompanyID = int.Parse(row[companyID].ToString());
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
            return clearingFirmPrimeBroker;
        }

        public static ClearingFirmPrimeBroker GetsClearingFirmPrimeBroker(int clearingFirmPrimeBrokerID)
        {
            ClearingFirmPrimeBroker clearingFirmPrimeBroker = new ClearingFirmPrimeBroker();

            object[] parameter = new object[1];
            parameter[0] = clearingFirmPrimeBrokerID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClearingFirmPrimeBroker", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clearingFirmPrimeBroker = FillClearingFirmPrimeBroker(row, 0);
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
            return clearingFirmPrimeBroker;
        }

        public static ClearingFirmsPrimeBrokers GetClearingFirmsPrimeBrokers()
        {
            ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyClearingFirmsPrimeBrokers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clearingFirmsPrimeBrokers.Add(FillClearingFirmPrimeBroker(row, 0));
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
            return clearingFirmsPrimeBrokers;
        }
        public static ClearingFirmsPrimeBrokers GetClearingFirmPrimeBroker(int companyID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClearingFirmsPrimeBrokers", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clearingFirmsPrimeBrokers.Add(FillClearingFirmPrimeBroker(row, 0));
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
            return clearingFirmsPrimeBrokers;
        }
        public static bool DeleteClearingFirmPrimeBroker(int clearingFirmsPrimeBrokersID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = clearingFirmsPrimeBrokersID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyClearingFirmPrimeBroker", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClearingFirmPrimeBrokerByID", parameter) > 0)
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
        public static int SaveClearingFirmPrimeBroker(ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers, int companyID)
        {
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyClearingFirmsPrimeBrokers", parameter).ToString();
                parameter = new Object[4];
                foreach (ClearingFirmPrimeBroker clearingFirmPrimeBroker in clearingFirmsPrimeBrokers)
                {
                    parameter[0] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID;
                    parameter[1] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName;
                    parameter[2] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName;
                    parameter[3] = companyID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyClearingFirmPrimeBroker", parameter).ToString());

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

        #endregion

        #region Company Client

        public static CompanyClient FillCompanyClient(object[] row, int offSet)
        {

            int ID = 0 + offSet;
            int name = 1 + offSet;
            int address1 = 2 + offSet;
            int address2 = 3 + offSet;
            int companyTypeID = 4 + offSet;
            int telephone = 5 + offSet;
            int fax = 6 + offSet;
            int primaryContactFirstName = 7 + offSet;
            int primaryContactLastName = 8 + offSet;
            int primaryContactTitle = 9 + offSet;
            int primaryContactEMail = 10 + offSet;
            int primaryContactTelephone = 11 + offSet;
            int primaryContactCell = 12 + offSet;
            int secondaryContactFirstName = 13 + offSet;
            int secondaryContactLastName = 14 + offSet;
            int secondaryContactTitle = 15 + offSet;
            int secondaryContactEMail = 16 + offSet;
            int secondaryContactTelephone = 17 + offSet;
            int secondaryContactCell = 18 + offSet;
            int companyID = 19 + offSet;
            int shortName = 20 + offSet;
            int countryID = 21 + offSet;
            int stateID = 22 + offSet;
            int zip = 23 + offSet;


            CompanyClient companyClient = new CompanyClient();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    companyClient.CompanyClientID = int.Parse(row[ID].ToString());
                }
                if (row[name] != System.DBNull.Value)
                {
                    companyClient.Name = row[name].ToString();
                }
                if (row[address1] != System.DBNull.Value)
                {
                    companyClient.Address1 = row[address1].ToString();
                }
                if (row[address2] != System.DBNull.Value)
                {
                    companyClient.Address2 = row[address2].ToString();
                }
                if (row[companyTypeID] != System.DBNull.Value)
                {
                    companyClient.CompanyTypeID = int.Parse(row[companyTypeID].ToString());
                }
                if (row[telephone] != System.DBNull.Value)
                {
                    companyClient.Telephone = row[telephone].ToString();
                }
                if (row[fax] != System.DBNull.Value)
                {
                    companyClient.Fax = row[fax].ToString();
                }
                if (row[primaryContactFirstName] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactFirstName = row[primaryContactFirstName].ToString();
                }
                if (row[primaryContactLastName] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactLastName = row[primaryContactLastName].ToString();
                }
                if (row[primaryContactTitle] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactTitle = row[primaryContactTitle].ToString();
                }
                if (row[primaryContactEMail] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactEMail = row[primaryContactEMail].ToString();
                }
                if (row[primaryContactTelephone] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactTelephone = row[primaryContactTelephone].ToString();
                }
                if (row[primaryContactCell] != System.DBNull.Value)
                {
                    companyClient.PrimaryContactCell = row[primaryContactCell].ToString();
                }

                if (row[secondaryContactFirstName] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactFirstName = row[secondaryContactFirstName].ToString();
                }
                if (row[secondaryContactLastName] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactLastName = row[secondaryContactLastName].ToString();
                }
                if (row[secondaryContactTitle] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactTitle = row[secondaryContactTitle].ToString();
                }
                if (row[secondaryContactEMail] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactEMail = row[secondaryContactEMail].ToString();
                }
                if (row[secondaryContactTelephone] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactTelephone = row[secondaryContactTelephone].ToString();
                }
                if (row[secondaryContactCell] != System.DBNull.Value)
                {
                    companyClient.SecondaryContactCell = row[secondaryContactCell].ToString();
                }
                if (row[companyID] != System.DBNull.Value)
                {
                    companyClient.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[shortName] != System.DBNull.Value)
                {
                    companyClient.ShortName = row[shortName].ToString();
                }
                if (row[countryID] != System.DBNull.Value)
                {
                    companyClient.CountryID = int.Parse(row[countryID].ToString());
                }
                if (row[stateID] != System.DBNull.Value)
                {
                    companyClient.StateID = int.Parse(row[stateID].ToString());
                }
                if (row[zip] != System.DBNull.Value)
                {
                    companyClient.Zip = row[zip].ToString();
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
            return companyClient;
        }


        /// <summary>
        /// Adds/Updates <see cref="CompanyClient"/> into database.
        /// </summary>
        /// <param name="companyClient"><see cref="CompanyClient"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static int SaveCompanyClient(int companyID, CompanyClient companyClient)
        {
            int result = int.MinValue;

            object[] parameter = new object[24];

            parameter[0] = companyID;
            parameter[1] = companyClient.Name;
            parameter[2] = companyClient.Address1;
            parameter[3] = companyClient.Address2;
            parameter[4] = companyClient.CompanyTypeID;
            parameter[5] = companyClient.Telephone;
            parameter[6] = companyClient.Fax;
            parameter[7] = companyClient.PrimaryContactFirstName;
            parameter[8] = companyClient.PrimaryContactLastName;
            parameter[9] = companyClient.PrimaryContactTitle;
            parameter[10] = companyClient.PrimaryContactEMail;
            parameter[11] = companyClient.PrimaryContactTelephone;
            parameter[12] = companyClient.PrimaryContactCell;
            parameter[13] = companyClient.SecondaryContactFirstName;
            parameter[14] = companyClient.SecondaryContactLastName;
            parameter[15] = companyClient.SecondaryContactTitle;
            parameter[16] = companyClient.SecondaryContactEMail;
            parameter[17] = companyClient.SecondaryContactTelephone;
            parameter[18] = companyClient.SecondaryContactCell;

            parameter[19] = companyClient.ShortName;
            parameter[20] = companyClient.CountryID;
            parameter[21] = companyClient.StateID;
            parameter[22] = companyClient.Zip;
            parameter[23] = companyClient.CompanyClientID;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyClientDetail", parameter).ToString());

                //companyClient.CompanyID = result;
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
        /// Deletes <see cref="CompanyClient"/> from the database.
        /// </summary>
        /// <param name="companyID">ID of the <see cref="CompanyClient"/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static bool DeleteCompanyClient(int companyID, int companyClientID, bool deleteForceFully)
        {
            bool result = false;
            Object[] parameter = new object[3];
            parameter[0] = companyID;
            parameter[1] = companyClientID;
            parameter[2] = (deleteForceFully == true ? 1 : 0);

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyClient", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientByID", parameter) > 0)
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
        /// TODO : Important -- We need to have CompanyClients (the collection) instead of a single class companyclient, Please check
        /// Gets <see cref="CompanyClient"/> from the database.
        /// </summary>
        /// <param name="companyID">ID of <see cref="CompanyClient"/> to be fetched from database.</param>
        /// <returns><see cref="CompanyClient"/> fetched.</returns>
        public static CompanyClient GetCompanyClient(int companyID)
        {
            CompanyClient companyClient = new CompanyClient();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClient", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClient = FillCompanyClient(row, 0);
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
            return companyClient;

        }


        /// <summary>
        /// Gets all <see cref="Companies"/> from datatbase.
        /// </summary>
        /// <returns><see cref="Companies"/> fetched.</returns>
        public static CompanyClients GetCompanyClients()
        {
            CompanyClients companyClients = new CompanyClients();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyClients";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClients.Add(FillCompanyClient(row, 0));
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
            return companyClients;
        }

        public static CompanyClients GetCompanyClientsByCompanyID(int CompanyID)
        {
            CompanyClients companyClients = new CompanyClients();
            object[] parameter = new object[1];
            parameter[0] = CompanyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientsByCompanyID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClients.Add(FillCompanyClient(row, 0));
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
            return companyClients;
        }


        public static CompanyClient GetCompanyClient(int companyID, int companyClientID)
        {
            CompanyClient companyClient = new CompanyClient();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyClientID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientsBoth", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClient = FillCompanyClient(row, 0);
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
            return companyClient;
        }


        #region Client names for RMadmin


        /// <summary>
        /// The methhod is used to get all the details of all the Clients of a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompanyClients GetCompanyClientsRM(int companyID)
        {
            CompanyClients companyClients = new CompanyClients();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientsRM", parameter))
                {
                    object[] row = null;
                    while (reader.Read())
                    {
                        row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClients.Add(FillCompanyClient(row, 0));
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
            return companyClients;

        }

        #endregion
        #endregion

        #region CompanyCVIdentifier
        //Not used for now as this info is clubbed with the companyCountePartyVenueDetails fetch info.
        public static CompanyCounterPartyVenueDetail FillCompanyCounterPartyVenueIdentifier(object[] row, int offSet)
        {
            int companyCounterPartyVenueIdentifierID = 0 + offSet;
            int companyCounterPartyVenueID = 1 + offSet;
            //int identifierID = 2 + offSet;
            //int identifier = 3 + offSet;
            int cmtaIdentifier = 2 + offSet;
            int giveUpIdentifier = 3 + offSet;

            CompanyCounterPartyVenueDetail companyCounterPartyVenueIdentifier = new CompanyCounterPartyVenueDetail();
            try
            {
                if (row[companyCounterPartyVenueIdentifierID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueIdentifierID = int.Parse(row[companyCounterPartyVenueIdentifierID].ToString());
                }

                if (row[companyCounterPartyVenueID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueID = int.Parse(row[companyCounterPartyVenueID].ToString());
                }

                //if (row[identifierID] != System.DBNull.Value)
                //{
                //    companyCounterPartyVenueIdentifier.CMTAGiveUp = int.Parse(row[identifierID].ToString());
                //}

                //if (row[identifier] != System.DBNull.Value)
                //{
                //    companyCounterPartyVenueIdentifier.CMTAGiveUPName = row[identifier].ToString();
                //}

                if (row[cmtaIdentifier] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CMTAIdentifier = row[cmtaIdentifier].ToString();
                }
                if (row[giveUpIdentifier] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.GiveUpIdentifier = row[giveUpIdentifier].ToString();
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
            return companyCounterPartyVenueIdentifier;
        }

        //Ad Hoc CompanyCounterParty Company Level Tags
        public static int SaveCompanyCounterPartyVenueIdentifierAdHoc(CompanyCounterPartyVenueDetail companyCounterPartyVenueIdentifier)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            try
            {
                parameter[0] = companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueID;
                //parameter[1] = companyCounterPartyVenueIdentifier.CMTAGiveUp;
                //parameter[2] = companyCounterPartyVenueIdentifier.IdentifierName;
                parameter[1] = companyCounterPartyVenueIdentifier.CMTAIdentifier;
                parameter[2] = companyCounterPartyVenueIdentifier.GiveUpIdentifier;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCounterPartyVenueIdentifier", parameter).ToString());
                //company.CompanyID = result;
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


        // CompanyCounterParty Venue GiveUp Identifiers
        public static int SaveCompanyCVenueGiveUpIdentifiers(CompanyCVGiveUpIdentifiers companyCVGiveUpIdentifiers, int counterpartyvenueID)
        {
            int result = int.MinValue;
            StringBuilder GiveUpStringBuilder = new StringBuilder();

            object[] parameter = new object[3];
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyCVGiveUpIdentifier", counterpartyvenueID);

                foreach (CompanyCVGiveUpIdentifier companyCVGiveUpId in companyCVGiveUpIdentifiers)
                {
                    parameter[0] = counterpartyvenueID;
                    parameter[1] = companyCVGiveUpId.CompanyCVGiveUpIdentifierID;
                    parameter[2] = companyCVGiveUpId.GiveUpIdentifier;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCVGiveUpIdentifier", parameter).ToString());

                    GiveUpStringBuilder.Append("'");
                    GiveUpStringBuilder.Append(result.ToString());
                    GiveUpStringBuilder.Append("',");
                }

                parameter = new object[2];
                int len = GiveUpStringBuilder.Length;
                if (GiveUpStringBuilder.Length > 0)
                {
                    GiveUpStringBuilder.Remove((len - 1), 1);
                }
                parameter[0] = counterpartyvenueID;
                parameter[1] = GiveUpStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCVGiveUpIdentifier", parameter).ToString();
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

        public static int DeleteCompanyCVenueGiveUpIdentifiers(int counterpartyvenueID)
        {
            int result = int.MinValue;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCVGiveUpIdentifierNew", new object[] { counterpartyvenueID });

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

        // CompanyCounterParty Venue CMTA Identifiers
        public static int SaveCompanyCVenueCMTAIdentifiers(CompanyCVCMTAIdentifiers companyCVCMTAIdentifiers, int counterpartyvenueID)
        {
            int result = int.MinValue;

            StringBuilder CMTAStringBuilder = new StringBuilder();
            object[] parameter = new object[3];
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyCVCMTAIdentifier", counterpartyvenueID);

                foreach (CompanyCVCMTAIdentifier companyCVCMTAId in companyCVCMTAIdentifiers)
                {
                    parameter[0] = counterpartyvenueID;
                    parameter[1] = companyCVCMTAId.CompanyCVCMTAIdentifierID;
                    parameter[2] = companyCVCMTAId.CMTAIdentifier;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCVCMTAIdentifier", parameter).ToString());

                    CMTAStringBuilder.Append("'");
                    CMTAStringBuilder.Append(result.ToString());
                    CMTAStringBuilder.Append("',");
                }

                parameter = new object[2];
                int len = CMTAStringBuilder.Length;
                if (CMTAStringBuilder.Length > 0)
                {
                    CMTAStringBuilder.Remove((len - 1), 1);
                }
                parameter[0] = counterpartyvenueID;
                parameter[1] = CMTAStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCVCMTAIdentifier", parameter).ToString();

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

        // Delete Company CV CMTA Identifier
        public static int DeleteCompanyCVenueCMTAIdentifiers(int counterpartyvenueID)
        {
            int result = int.MinValue;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCVCMTAIdentifierNew", new object[] { counterpartyvenueID });

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


        //CompanyCounterParty Company Level Tags
        //Save Function
        public static int SaveCompanyCounterPartyVenueIdentifier(CompanyCounterPartyVenueDetails companyCounterPartyVenueIdentifiers)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            try
            {
                //				foreach(CompanyCounterPartyVenueDetail companyCounterPartyVenueIdentifier in companyCounterPartyVenueIdentifiers)
                //				{
                //					parameter[0] = companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueID;//companyCounterPartyVenueDetail.CompanyCounterPartyVenueTagID;
                //					db.ExecuteNonQuery("P_DeleteCompanyCounterPartyCompanyLevelTagDetail", parameter).ToString();
                //				}

                foreach (CompanyCounterPartyVenueDetail companyCounterPartyVenueIdentifier in companyCounterPartyVenueIdentifiers)
                {
                    parameter[0] = companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueID;
                    //parameter[1] = companyCounterPartyVenueIdentifier.CMTAGiveUp;
                    //parameter[2] = companyCounterPartyVenueIdentifier.IdentifierName;
                    parameter[1] = companyCounterPartyVenueIdentifier.CMTAIdentifier;
                    parameter[2] = companyCounterPartyVenueIdentifier.GiveUpIdentifier;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCounterPartyVenueIdentifier", parameter).ToString());
                    //company.CompanyID = result;
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

        public static CompanyCounterPartyVenueDetail GetCompanyCounterPartyVenueIdentifier(int companyCounterPartyVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;
            CompanyCounterPartyVenueDetail companyCounterPartyVenueIdentifier = new CompanyCounterPartyVenueDetail();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenueIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenueIdentifier = FillCompanyCounterPartyVenueIdentifier(row, 0);
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
            return companyCounterPartyVenueIdentifier;
        }
        #endregion

        #region CompanyCounterPartyLevelTags

        //Fill Functions for CompanyCounterPartyDetails along with the CompanyCounterPartyIdentifier.
        public static CompanyCounterPartyVenueDetail FillCompanyCounterPartyVenueDetail(object[] row, int offSet)
        {
            int companyCounterPartyVenueDetailsID = 0 + offSet;
            int companyCounterPartyVenueID = 1 + offSet;
            int clearingFirmPrimeBrokerID = 2 + offSet;
            int companyMPID = 3 + offSet;

            int deliverToCompanyID = 4 + offSet;
            int senderCompanyID = 5 + offSet;
            int targetCompID = 6 + offSet;
            int clearingFirm = 7 + offSet;
            int companyAccountID = 8 + offSet;
            int companyStrategyID = 9 + offSet;
            int onBehalfOfSubID = 10 + offSet;

            int companyCounterPartyVenueIdentifierID = 11 + offSet;
            //int identifierID = 12 + offSet;
            //int identifier = 13 + offSet;
            int cmtaIdentifier = 12 + offSet;
            int giveUpIdentifier = 13 + offSet;

            CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail();
            try
            {
                if (row[companyCounterPartyVenueDetailsID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(row[companyCounterPartyVenueDetailsID].ToString());
                }

                if (row[companyCounterPartyVenueID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(row[companyCounterPartyVenueID].ToString());
                }

                if (row[clearingFirmPrimeBrokerID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(row[clearingFirmPrimeBrokerID].ToString());
                }

                if (row[companyMPID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.MPID = int.Parse(row[companyMPID].ToString());
                }

                if (row[deliverToCompanyID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.DeliverToCompanyID = row[deliverToCompanyID].ToString();
                }
                if (row[senderCompanyID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.SenderCompanyID = row[senderCompanyID].ToString();
                }
                if (row[targetCompID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.TargetCompID = row[targetCompID].ToString();
                }
                if (row[clearingFirm] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.ClearingFirm = row[clearingFirm].ToString();
                }
                if (row[companyAccountID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.AccountID = int.Parse(row[companyAccountID].ToString());
                }
                if (row[companyStrategyID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.StrategyID = int.Parse(row[companyStrategyID].ToString());
                }

                if (row[onBehalfOfSubID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.OnBehalfOfSubID = row[onBehalfOfSubID].ToString();
                }

                if (row[companyCounterPartyVenueIdentifierID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueIdentifierID = int.Parse(row[companyCounterPartyVenueIdentifierID].ToString());
                }

                if (row[cmtaIdentifier] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.CMTAIdentifier = row[cmtaIdentifier].ToString();
                }

                if (row[giveUpIdentifier] != System.DBNull.Value)
                {
                    companyCounterPartyVenueDetail.GiveUpIdentifier = row[giveUpIdentifier].ToString();
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
            return companyCounterPartyVenueDetail;
        }

        //Ad-Hoc Function CompanyCounterParty Company Level Tags
        public static int SaveCompanyCounterPartyCompanyLevelTagAdHoc(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
        {
            int result = int.MinValue;

            object[] parameter = new object[10];
            try
            {
                parameter[0] = companyCounterPartyVenueDetail.CompanyCounterPartyVenueID;
                parameter[1] = companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID;
                parameter[2] = companyCounterPartyVenueDetail.MPID;
                parameter[3] = companyCounterPartyVenueDetail.DeliverToCompanyID;

                parameter[4] = companyCounterPartyVenueDetail.SenderCompanyID;
                parameter[5] = companyCounterPartyVenueDetail.TargetCompID;

                parameter[6] = companyCounterPartyVenueDetail.ClearingFirm;
                parameter[7] = companyCounterPartyVenueDetail.AccountID;
                parameter[8] = companyCounterPartyVenueDetail.StrategyID;

                parameter[9] = companyCounterPartyVenueDetail.OnBehalfOfSubID;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCounterPartyCompanyLevelTagDetail", parameter).ToString());
                //company.CompanyID = result;
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

        //CompanyCounterParty Company Level Tags
        //Save Function
        public static int SaveCompanyCounterPartyCompanyLevelTags(CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails)
        {
            int result = int.MinValue;

            object[] parameter = new object[10];
            try
            {
                foreach (CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail in companyCounterPartyVenueDetails)
                {
                    parameter[0] = companyCounterPartyVenueDetail.CompanyCounterPartyVenueID;
                    parameter[1] = companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID;
                    parameter[2] = companyCounterPartyVenueDetail.MPID;
                    parameter[3] = companyCounterPartyVenueDetail.DeliverToCompanyID;

                    parameter[4] = companyCounterPartyVenueDetail.SenderCompanyID;
                    parameter[5] = companyCounterPartyVenueDetail.TargetCompID;

                    parameter[6] = companyCounterPartyVenueDetail.ClearingFirm;
                    parameter[7] = companyCounterPartyVenueDetail.AccountID;
                    parameter[8] = companyCounterPartyVenueDetail.StrategyID;

                    parameter[9] = companyCounterPartyVenueDetail.OnBehalfOfSubID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCounterPartyCompanyLevelTagDetail", parameter).ToString());
                    //company.CompanyID = result;
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

        public static CompanyCounterPartyVenueDetail GetCompanyCounterPartyVenueDetail(int companyCounterPartyVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;
            CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenueDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenueDetail = FillCompanyCounterPartyVenueDetail(row, 0);
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
            return companyCounterPartyVenueDetail;
        }


        // Get the Company Counter Party Venue GiveUp Identifiers
        public static CompanyCVGiveUpIdentifiers GetCompanyCVGiveUpIdentifiers(int companyCounterPartyVenueID)
        {
            CompanyCVGiveUpIdentifiers companyCVGiveupIdntifiers = new CompanyCVGiveUpIdentifiers();
            Object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVGiveUpIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCVGiveupIdntifiers.Add(FillCompanyCVGiveUpIdentifier(row, 0));
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
            return companyCVGiveupIdntifiers;
        }

        public static CompanyCVGiveUpIdentifier FillCompanyCVGiveUpIdentifier(object[] row, int offSet)
        {
            int CompantCounterPartyVenueID = 0 + offSet;
            int CompanyCVGiveUpIdentifierID = 1 + offSet;
            int GiveUpIdentifier = 2 + offSet;

            CompanyCVGiveUpIdentifier companyCVGiveUpIdentifier = new CompanyCVGiveUpIdentifier();
            try
            {
                if (row[CompantCounterPartyVenueID] != System.DBNull.Value)
                {
                    companyCVGiveUpIdentifier.CompanyCounterPartyVenueId = int.Parse(row[CompantCounterPartyVenueID].ToString());
                }
                if (row[CompanyCVGiveUpIdentifierID] != System.DBNull.Value)
                {
                    companyCVGiveUpIdentifier.CompanyCVGiveUpIdentifierID = int.Parse(row[CompanyCVGiveUpIdentifierID].ToString());
                }

                if (row[GiveUpIdentifier] != System.DBNull.Value)
                {
                    companyCVGiveUpIdentifier.GiveUpIdentifier = (row[GiveUpIdentifier].ToString());
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
            return companyCVGiveUpIdentifier;
        }

        // Get the Company Counter Party Venue CMTA Identifiers
        public static CompanyCVCMTAIdentifiers GetCompanyCVCMTAIdentifiers(int companyCounterPartyVenueID)
        {
            CompanyCVCMTAIdentifiers companyCVCMTAIdntifiers = new CompanyCVCMTAIdentifiers();
            Object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVCMTAIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCVCMTAIdntifiers.Add(FillCompanyCVCMTAIdentifier(row, 0));
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
            return companyCVCMTAIdntifiers;
        }

        public static CompanyCVCMTAIdentifier FillCompanyCVCMTAIdentifier(object[] row, int offSet)
        {
            int CompantCounterPartyVenueID = 0 + offSet;
            int CompanyCVCMTAIdentifierID = 1 + offSet;
            int CMTAIdentifier = 2 + offSet;

            CompanyCVCMTAIdentifier companyCVCMTAIdentifier = new CompanyCVCMTAIdentifier();
            try
            {
                if (row[CompantCounterPartyVenueID] != System.DBNull.Value)
                {
                    companyCVCMTAIdentifier.CompanyCounterPartyVenueId = int.Parse(row[CompantCounterPartyVenueID].ToString());
                }
                if (row[CompanyCVCMTAIdentifierID] != System.DBNull.Value)
                {
                    companyCVCMTAIdentifier.CompanyCVCMTAIdentifierID = int.Parse(row[CompanyCVCMTAIdentifierID].ToString());
                }

                if (row[CMTAIdentifier] != System.DBNull.Value)
                {
                    companyCVCMTAIdentifier.CMTAIdentifier = (row[CMTAIdentifier].ToString());
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
            return companyCVCMTAIdentifier;
        }


        public static CompanyCounterPartyVenueDetails GetCompanyCounterPartyVenueDetails(int companyCounterPartyVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;
            CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new CompanyCounterPartyVenueDetails();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenueDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenueDetails.Add(FillCompanyCounterPartyVenueDetail(row, 0));
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
            return companyCounterPartyVenueDetails;
        }

        public static bool DeleteCompanyCounterPartyVenueDetail(int companyCounterPartyVenueID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCounterPartyVenueDetailsByID", parameter) > 0)
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
        /// Deletes <see cref="CompanyCouterParty"/> from the database.
        /// </summary>
        /// <param name="companyID">ID of the <see cref="CompanyCounterParty "/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static bool DeleteCompanyCounterParty(int companyID, int companyCounterPartyID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyCounterPartyID;
            //parameter[2] = (deleteForceFully==true?1:0);

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCounterPartyByID", parameter) > 0)
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

        #region ClearingFirmsPrimeBrokers
        public static ClearingFirmPrimeBroker FillClearingFirmPrimeBrokerNew(object[] row, int offSet)
        {
            int clearingFirmsPrimeBrokersID = 0 + offSet;
            int clearingFirmsPrimeBrokersName = 1 + offSet;
            int clearingFirmsPrimeBrokersShortName = 2 + offSet;

            ClearingFirmPrimeBroker clearingFirmPrimeBroker = new ClearingFirmPrimeBroker();
            try
            {
                if (row[clearingFirmsPrimeBrokersID] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID = int.Parse(row[clearingFirmsPrimeBrokersID].ToString());
                }
                if (row[clearingFirmsPrimeBrokersName] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = row[clearingFirmsPrimeBrokersName].ToString();
                }
                if (row[clearingFirmsPrimeBrokersShortName] != null)
                {
                    clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = row[clearingFirmsPrimeBrokersShortName].ToString();
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
            return clearingFirmPrimeBroker;
        }

        public static ClearingFirmsPrimeBrokers GetClearingFirmsPrimeBrokersNew()
        {
            ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllClearingFirmsPrimeBrokers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clearingFirmsPrimeBrokers.Add(FillClearingFirmPrimeBrokerNew(row, 0));
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
            return clearingFirmsPrimeBrokers;
        }

        public static int SaveClearingFirmPrimeBrokerNew(ClearingFirmPrimeBroker clearingFirmPrimeBroker)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            parameter[0] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID;
            parameter[1] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName;
            parameter[2] = clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName;
            parameter[3] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClearingFirmPrimeBrokerNew", parameter).ToString());
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

        public static bool DeleteClearingFirmsPrimeBrokers(int clearingFirmsPrimeBrokersID, bool deleteForceFully)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = clearingFirmsPrimeBrokersID;
                parameter[1] = (deleteForceFully == true ? 1 : 0);
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteClearingFirmsPrimeBrokers", parameter) > 0)
                {
                    result = true;
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

        #region CompanyCompliance
        public static Company FillCompanyCompliance(object[] row, int offSet)
        {
            int companyComplianceID = 0 + offSet;
            int fixVersionID = 1 + offSet;
            int fixCapabilityID = 2 + offSet;
            int companyID = 3 + offSet;

            Company company = new Company();
            try
            {

                if (!(row[companyComplianceID] is System.DBNull))
                {
                    company.CompanyComplianceID = int.Parse(row[companyComplianceID].ToString());
                }
                if (!(row[fixVersionID] is System.DBNull))
                {
                    company.FixVersionID = int.Parse(row[fixVersionID].ToString());
                }
                if (!(row[fixCapabilityID] is System.DBNull))
                {
                    company.FixCapabilityID = int.Parse(row[fixCapabilityID].ToString());
                }
                if (!(row[companyID] is System.DBNull))
                {
                    company.CompanyID = int.Parse(row[companyID].ToString());
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
            return company;
        }

        public static Company GetCompanyCompliance(int companyID)
        {
            Company company = new Company();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCompliance", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        company = FillCompanyCompliance(row, 0);
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
            return company;
        }

        public static int SaveCompanyCompliance(Company company)
        {
            int result = int.MinValue;

            object[] parameter = new object[5];

            parameter[0] = company.CompanyComplianceID;
            parameter[1] = company.FixVersionID;
            parameter[2] = company.FixCapabilityID;
            parameter[3] = company.CompanyID;
            parameter[4] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyCompliance", parameter).ToString());
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

        public static void SaveTransferTradeRules(int companyID, TranferTradeRules transferTradeRules)
        {
            // int result = int.MinValue;

            object[] parameter = new object[19];

            parameter[0] = transferTradeRules.IsTIFChange;
            parameter[1] = transferTradeRules.IsTradingAccChange;
            parameter[2] = transferTradeRules.IsAccountChange;
            parameter[3] = transferTradeRules.IsStrategyChange;
            parameter[4] = transferTradeRules.IsHandlingInstrChange;
            parameter[5] = transferTradeRules.IsVenueCPChange;
            parameter[6] = transferTradeRules.IsAllowAllUserToCancelReplaceRemove;
            parameter[7] = transferTradeRules.IsAllowUserToChangeOrderType;
            parameter[8] = transferTradeRules.IsExecutionInstrChange;
            parameter[9] = companyID;
            parameter[10] = transferTradeRules.IsAllowUserToTansferTrade;
            parameter[11] = transferTradeRules.IsAllowUserToGenerateSub;
            parameter[12] = transferTradeRules.IsApplyLimitRulesForReplacingStagedOrders;
            parameter[13] = transferTradeRules.IsApplyLimitRulesForReplacingOtherOrders;
            parameter[14] = transferTradeRules.IsApplyLimitRulesForReplacingSubOrders;
            parameter[15] = transferTradeRules.IsAllowRestrictedSecuritiesList;
            parameter[16] = transferTradeRules.IsAllowAllowedSecuritiesList;
            if (transferTradeRules.MasterUsersIDs.Count == 0)
                parameter[17] = null;
            else
                parameter[17] = string.Join(",", transferTradeRules.MasterUsersIDs);
            parameter[18] = transferTradeRules.IsDefaultOrderTypeLimitForMultiDay;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTransferTradeRules", parameter);
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

        #region CompanyComplianceCurrency
        public static int SaveCompanyComplianceCurrencies(Companies companyComplianceCurrencies, int companyID)
        {
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                result = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyComplianceCurrencies", parameter);
                //				if(result > 0)
                //				{
                foreach (Company companyComplianceCurrency in companyComplianceCurrencies)
                {
                    parameter = new object[3];
                    parameter[0] = companyComplianceCurrency.BaseCurrencyID; //Here BasecurrencyID refers to just any currencyID, not any specific base or multiple currency.
                    parameter[1] = companyID;
                    parameter[2] = int.MinValue;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyComplianceCurrencies", parameter).ToString());

                }
                //				}

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

        public static Company FillCompanyComplianceCurrencies(object[] row, int offSet)
        {
            int companyAllCurrencyID = 0 + offSet;
            int currencyID = 1 + offSet;
            int companyID = 2 + offSet;

            Company companyComplianceCurrency = new Company();
            try
            {

                if (!(row[companyAllCurrencyID] is System.DBNull))
                {
                    companyComplianceCurrency.BaseCurrencyID = int.Parse(row[companyAllCurrencyID].ToString());
                }
                if (!(row[currencyID] is System.DBNull))
                {
                    companyComplianceCurrency.MultipleCurrencyID = int.Parse(row[currencyID].ToString());
                }
                if (!(row[companyID] is System.DBNull))
                {
                    companyComplianceCurrency.CompanyID = int.Parse(row[companyID].ToString());
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
            return companyComplianceCurrency;
        }

        /// <summary>
        /// Comment By Rajat 17 July 2006
        /// This method provides the information in form of Companies(Company collection), but fills in only 
        /// the compliance related information. Rest of the information is blank. This could create problem.
        /// Ideally the Company object behaves like a composite objects which contains various objects like
        /// HomeInformation, OfficeInformation, ComplianceInformation etc etc. And we should have separate methods 
        /// to fetch these separately.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Companies GetCompanyComplianceCurrencies(int companyID)
        {
            Companies companyComplianceCurrencies = new Companies();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyComplianceCurrencies", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyComplianceCurrencies.Add(FillCompanyComplianceCurrencies(row, 0));
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
            return companyComplianceCurrencies;
        }

        /// <summary>
        /// Author : Rajat
        /// TODO : Why not KeyValuePair
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns> KeyValuePair of CurrencyId and CurrencyName</returns>
        public static BaseCurrency GetCompanyBaseCurrency(int companyID)
        {
            BaseCurrency companayBaseCurrency = null;
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyBaseCurrency", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companayBaseCurrency = FillCompanyBaseCurrency(row, 0);
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
            return companayBaseCurrency;

        }



        /// <summary>
        /// Author : Rajat
        /// TODO : Why not KeyValuePair
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static BaseCurrency FillCompanyBaseCurrency(object[] row, int offSet)
        {
            int baseCurrencyID = 0 + offSet;
            int baseCurrencyNmae = 1 + offSet;

            BaseCurrency companyBaseCurrencyPair = new BaseCurrency();
            try
            {
                companyBaseCurrencyPair.BaseCurrencyId = int.Parse(row[baseCurrencyID].ToString());
                companyBaseCurrencyPair.BaseCurrencyName = row[baseCurrencyNmae].ToString();

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
            return companyBaseCurrencyPair;


        }

        #endregion

        #endregion

        #region CompanyBorrower
        public static Company FillCompanyBorrower(object[] row, int offSet)
        {
            int companyBorrowerID = 0 + offSet;
            int borrowerName = 1 + offSet;
            int borrowerShortName = 2 + offSet;
            int borrowerFirmID = 3 + offSet;
            int companyID = 4 + offSet;

            Company company = new Company();
            try
            {

                if (!(row[companyBorrowerID] is System.DBNull))
                {
                    company.CompanyBorrowerID = int.Parse(row[companyBorrowerID].ToString());
                }
                if (!(row[borrowerName] is System.DBNull))
                {
                    company.BorrowerName = row[borrowerName].ToString();
                }
                if (!(row[borrowerShortName] is System.DBNull))
                {
                    company.BorrowerShortName = row[borrowerShortName].ToString();
                }
                if (!(row[borrowerFirmID] is System.DBNull))
                {
                    company.FirmID = row[borrowerFirmID].ToString();
                }
                if (!(row[companyID] is System.DBNull))
                {
                    company.CompanyID = int.Parse(row[companyID].ToString());
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
            return company;
        }

        public static Companies GetCompanyBorrowers(int companyID)
        {
            Companies companyBorrowers = new Companies();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyBorrower", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyBorrowers.Add(FillCompanyBorrower(row, 0));
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
            return companyBorrowers;
        }

        public static int SaveCompanyBorrower(Companies companyBorrowerDetails, int companyID)
        {
            int result = int.MinValue;
            StringBuilder companyBorrowerIDStringBuilder = new StringBuilder();

            object[] parameter = new object[6];
            try
            {
                foreach (Company companyBorrowerDetail in companyBorrowerDetails)
                {
                    parameter = new object[6];
                    parameter[0] = companyBorrowerDetail.CompanyBorrowerID;
                    parameter[1] = companyBorrowerDetail.BorrowerName;
                    parameter[2] = companyBorrowerDetail.BorrowerShortName;
                    parameter[3] = companyBorrowerDetail.FirmID;
                    parameter[4] = companyID;
                    parameter[5] = int.MinValue;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyBorrower", parameter).ToString());

                    companyBorrowerIDStringBuilder.Append("'");
                    companyBorrowerIDStringBuilder.Append(result.ToString());
                    companyBorrowerIDStringBuilder.Append("',");
                }

                int len = companyBorrowerIDStringBuilder.Length;
                if (companyBorrowerIDStringBuilder.Length > 0)
                {
                    companyBorrowerIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = companyBorrowerIDStringBuilder.ToString();
                if (companyBorrowerIDStringBuilder.Length > 0)
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyBorrowerDetails", parameter).ToString();
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

        public static bool DeleteCompanyBorrower(int companyBorrowerID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyBorrowerID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteCompanyClearingFirmPrimeBroker", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyBorrowerByBorrowerID", parameter) > 0)
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

        #region CompanyVenue
        public static Company FillCompanyVenue(object[] row, int offSet)
        {
            int companyVenueID = 0 + offSet;
            int venueType = 1 + offSet;
            int venueName = 2 + offSet;
            int venueShortName = 3 + offSet;
            int timeZone = 4 + offSet;
            int preMarketTime = 5 + offSet;
            int preMarketStartTime = 6 + offSet;
            int preMarketEndTime = 7 + offSet;
            int regularMarketTime = 8 + offSet;
            int regularMarketStartTime = 9 + offSet;
            int lunchTime = 10 + offSet;
            int lunchStartTime = 11 + offSet;
            int lunchEndTime = 12 + offSet;
            int regularMarketEndTime = 13 + offSet;
            int postMarketTime = 14 + offSet;
            int postMarketStartTime = 15 + offSet;
            int postMarketEndTime = 16 + offSet;
            int companyID = 17 + offSet;

            Company companyVenue = new Company();
            try
            {

                if (!(row[companyVenueID] is System.DBNull))
                {
                    companyVenue.CompanyVenueID = int.Parse(row[companyVenueID].ToString());
                }
                if (!(row[venueType] is System.DBNull))
                {
                    companyVenue.VenueType = int.Parse(row[venueType].ToString());
                }
                if (!(row[venueName] is System.DBNull))
                {
                    companyVenue.VenueName = row[venueName].ToString();
                }
                if (!(row[venueShortName] is System.DBNull))
                {
                    companyVenue.VenueShortName = row[venueShortName].ToString();
                }
                if (!(row[timeZone] is System.DBNull))
                {
                    companyVenue.TimeZone = int.Parse(row[timeZone].ToString());
                }
                if (!(row[preMarketTime] is System.DBNull))
                {
                    companyVenue.PreMarketCheck = int.Parse(row[preMarketTime].ToString());
                }
                if (!(row[preMarketStartTime] is System.DBNull))
                {
                    companyVenue.PreMarketTradingStartTime = DateTime.Parse(row[preMarketStartTime].ToString());
                }
                if (!(row[preMarketEndTime] is System.DBNull))
                {
                    companyVenue.PreMarketTradingEndTime = DateTime.Parse(row[preMarketEndTime].ToString());
                }
                if (!(row[regularMarketTime] is System.DBNull))
                {
                    companyVenue.RegularTimeCheck = int.Parse(row[regularMarketTime].ToString());
                }
                if (!(row[regularMarketStartTime] is System.DBNull))
                {
                    companyVenue.RegularTradingStartTime = DateTime.Parse(row[regularMarketStartTime].ToString());
                }
                if (!(row[lunchTime] is System.DBNull))
                {
                    companyVenue.LunchTimeCheck = int.Parse(row[lunchTime].ToString());
                }
                if (!(row[lunchStartTime] is System.DBNull))
                {
                    companyVenue.LunchTimeStartTime = DateTime.Parse(row[lunchStartTime].ToString());
                }
                if (!(row[lunchEndTime] is System.DBNull))
                {
                    companyVenue.LunchTimeEndTime = DateTime.Parse(row[lunchEndTime].ToString());
                }
                if (!(row[regularMarketEndTime] is System.DBNull))
                {
                    companyVenue.RegularTradingEndTime = DateTime.Parse(row[regularMarketEndTime].ToString());
                }
                if (!(row[postMarketTime] is System.DBNull))
                {
                    companyVenue.PostMarketCheck = int.Parse(row[postMarketTime].ToString());
                }
                if (!(row[postMarketStartTime] is System.DBNull))
                {
                    companyVenue.PostMarketTradingStartTime = DateTime.Parse(row[postMarketStartTime].ToString());
                }
                if (!(row[postMarketEndTime] is System.DBNull))
                {
                    companyVenue.PostMarketTradingEndTime = DateTime.Parse(row[postMarketEndTime].ToString());
                }
                if (!(row[companyID] is System.DBNull))
                {
                    companyVenue.CompanyID = int.Parse(row[companyID].ToString());
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
            return companyVenue;
        }

        public static Company GetCompanyVenueDetails(int companyID)
        {
            Company companyVenue = new Company();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyVenueDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyVenue = FillCompanyVenue(row, 0);
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
            return companyVenue;
        }

        public static int SaveCompanyVenueDetails(Company companyVenue)
        {
            int result = int.MinValue;

            object[] parameter = new object[19];

            parameter[0] = companyVenue.CompanyVenueID;
            parameter[1] = companyVenue.VenueName;
            parameter[2] = companyVenue.VenueShortName;
            parameter[3] = companyVenue.VenueType;
            parameter[4] = companyVenue.TimeZone;
            parameter[5] = companyVenue.PreMarketCheck;

            parameter[6] = companyVenue.PreMarketTradingStartTime;
            parameter[7] = companyVenue.PreMarketTradingEndTime;
            parameter[8] = companyVenue.RegularTimeCheck;
            parameter[9] = companyVenue.RegularTradingStartTime;
            parameter[10] = companyVenue.LunchTimeCheck;
            parameter[11] = companyVenue.LunchTimeStartTime;

            parameter[12] = companyVenue.LunchTimeEndTime;
            parameter[13] = companyVenue.RegularTradingEndTime;
            parameter[14] = companyVenue.PostMarketCheck;
            parameter[15] = companyVenue.PostMarketTradingStartTime;
            parameter[16] = companyVenue.PostMarketTradingEndTime;
            parameter[17] = companyVenue.CompanyID;
            parameter[18] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyVenueDetails", parameter).ToString());
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

        #region CompanyUserAUEC
        public static AUEC FillCompanyUserAUEC(object[] row, int offSet)
        {
            int companyUserAUECID = 0 + offSet;
            int companyUserID = 1 + offSet;
            int companyAUECID = 2 + offSet;

            AUEC companyUserAUEC = new AUEC();
            try
            {

                if (!(row[companyUserAUECID] is System.DBNull))
                {
                    companyUserAUEC.CompanyUserAUECID = int.Parse(row[companyUserAUECID].ToString());
                }
                if (!(row[companyUserID] is System.DBNull))
                {
                    companyUserAUEC.CompanyUserID = int.Parse(row[companyUserID].ToString());
                }
                if (!(row[companyAUECID] is System.DBNull))
                {
                    companyUserAUEC.CompanyAUECID = int.Parse(row[companyAUECID].ToString());
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
            return companyUserAUEC;
        }

        public static AUECs GetCompanyUserAUECs(int companyUserID)
        {
            AUECs companyUserAUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUserAUECs.Add(FillCompanyUserAUEC(row, 0));
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
            return companyUserAUECs;
        }

        public static AUEC FillCompanyUserAUECDetails(object[] row, int offSet)
        {
            int AUECID = 0 + offSet;
            int ASSETID = 1 + offSet;
            int UNDERLYINGID = 2 + offSet;
            int EXCHANGEID = 3 + offSet;
            int CURRENCYID = 4 + offSet;
            int DISPLAYNAME = 5 + offSet;

            AUEC companyUserAUEC = new AUEC();
            try
            {

                if (!(row[AUECID] is System.DBNull))
                {
                    companyUserAUEC.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (!(row[ASSETID] is System.DBNull))
                {
                    companyUserAUEC.AssetID = int.Parse(row[ASSETID].ToString());
                }
                if (!(row[UNDERLYINGID] is System.DBNull))
                {
                    companyUserAUEC.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                }
                if (!(row[EXCHANGEID] is System.DBNull))
                {
                    companyUserAUEC.ExchangeID = int.Parse(row[EXCHANGEID].ToString());
                }
                if (!(row[CURRENCYID] is System.DBNull))
                {
                    companyUserAUEC.CurrencyID = int.Parse(row[CURRENCYID].ToString());
                }
                if (!(row[DISPLAYNAME] is System.DBNull))
                {
                    companyUserAUEC.DisplayName = row[DISPLAYNAME].ToString();
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
            return companyUserAUEC;
        }

        public static AUECs GetCompanyUserAUECDetails(int companyUserID)
        {
            AUECs companyUserAUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUsersAUECs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUserAUECs.Add(FillCompanyUserAUECDetails(row, 0));
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
            return companyUserAUECs;
        }

        public static bool SaveCompanyAUECsForUser(int companyUserID, AUECs companyUserAUECs)
        {
            int result = int.MinValue;
            StringBuilder auecIDStringBuilder = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {
                foreach (AUEC companyUserAUEC in companyUserAUECs)
                {
                    parameter = new object[2];
                    parameter[0] = companyUserID;
                    parameter[1] = companyUserAUEC.CompanyAUECID;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyUserAUECs", parameter).ToString());

                    auecIDStringBuilder.Append("'");
                    auecIDStringBuilder.Append(result.ToString());
                    auecIDStringBuilder.Append("',");
                }

                int len = auecIDStringBuilder.Length;
                if (auecIDStringBuilder.Length > 0)
                {
                    auecIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyUserID;
                parameter[1] = auecIDStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserAUEC", parameter).ToString();

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
            return true;
        }
        #endregion

        #region CompanyUserAccounts
        public static Account FillCompanyUserAccounts(object[] row, int offSet)
        {
            int companyUserAccountID = 0 + offSet;
            int companyAccountID = 1 + offSet;
            int accountName = 2 + offSet;

            Account companyUserAccount = new Account();
            try
            {
                if (row[companyUserAccountID] != null)
                {
                    companyUserAccount.CompanyUserAccountID = int.Parse(row[companyUserAccountID].ToString());
                }
                if (row[companyAccountID] != null)
                {
                    companyUserAccount.CompanyAccountID = int.Parse(row[companyAccountID].ToString());
                }
                if (row[accountName] != null)
                {
                    companyUserAccount.AccountName = row[accountName].ToString();
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
            return companyUserAccount;
        }

        public static Accounts GetAccountsForCompanyUser(int companyUserID)
        {
            Accounts companyUserAccounts = new Accounts();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyFundsForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUserAccounts.Add(FillCompanyUserAccounts(row, 0));

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
            return companyUserAccounts;
        }

        public static bool SaveCompanyAccountsForUser(int companyUserID, Accounts companyUserAccounts)
        {
            int result = int.MinValue;
            StringBuilder accountIDStringBuilder = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {
                foreach (Account companyUserAccount in companyUserAccounts)
                {
                    parameter = new object[2];
                    parameter[0] = companyUserID;
                    //parameter[1] = companyUserAccount.AccountID; //It is CompanyAccountID in actual but the name changed.
                    parameter[1] = companyUserAccount.CompanyAccountID;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyUserFunds", parameter).ToString());

                    accountIDStringBuilder.Append("'");
                    accountIDStringBuilder.Append(result.ToString());
                    accountIDStringBuilder.Append("',");
                }

                int len = accountIDStringBuilder.Length;
                if (accountIDStringBuilder.Length > 0)
                {
                    accountIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyUserID;
                parameter[1] = accountIDStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserFunds", parameter).ToString();

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
            return true;
        }
        #endregion

        #region CompanyStrategies
        public static Strategy FillCompanyStrategy(object[] row, int offSet)
        {
            int companyUserStrategyID = 0 + offSet;
            int companyStrategyID = 1 + offSet;
            int strategyName = 2 + offSet;

            Strategy companyUserStrategy = new Strategy();
            try
            {
                if (row[companyUserStrategyID] != null)
                {
                    companyUserStrategy.CompanyUserStrategyID = int.Parse(row[companyUserStrategyID].ToString());
                }
                if (row[companyStrategyID] != null)
                {
                    companyUserStrategy.CompanyStrategyID = int.Parse(row[companyStrategyID].ToString());
                }
                if (row[strategyName] != null)
                {
                    companyUserStrategy.StrategyName = row[strategyName].ToString();
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
            return companyUserStrategy;
        }

        public static Strategies GetStrategiesForCompanyUser(int companyUserID)
        {
            Strategies companyUserStrategies = new Strategies();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyStrategiesForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyUserStrategies.Add(FillCompanyStrategy(row, 0));

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
            return companyUserStrategies;
        }

        public static bool SaveCompanyStrategiesForUser(int companyUserID, Strategies companyUserStrategies)
        {
            int result = int.MinValue;
            StringBuilder strategyIDStringBuilder = new StringBuilder();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {
                foreach (Strategy companyUserStrategy in companyUserStrategies)
                {
                    parameter = new object[2];
                    parameter[0] = companyUserID;
                    parameter[1] = companyUserStrategy.StrategyID; //It is CompanyStrategyID in actual but the name changed.
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyUserStrategies", parameter).ToString());

                    strategyIDStringBuilder.Append("'");
                    strategyIDStringBuilder.Append(result.ToString());
                    strategyIDStringBuilder.Append("',");
                }

                int len = strategyIDStringBuilder.Length;
                if (strategyIDStringBuilder.Length > 0)
                {
                    strategyIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = companyUserID;
                parameter[1] = strategyIDStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserStrategies", parameter).ToString();

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
            return true;
        }
        #endregion

        #region CompanyThirdParty

        #region CompanyThirdPartySaveDetails
        public static ThirdPartyFlatFileSaveDetail FillCompanyThirdPartySaveDetail(object[] row, int offSet)
        {
            int companyThirdPartySaveDetailID = 0 + offSet;
            int companyThirdPartyID = 1 + offSet;
            int saveGeneratedFileIn = 2 + offSet;
            int namingConvention = 3 + offSet;
            int companyIdentifier = 4 + offSet;


            ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail = new ThirdPartyFlatFileSaveDetail();
            try
            {

                if (!(row[companyThirdPartySaveDetailID] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyThirdPartySaveDetailID = int.Parse(row[companyThirdPartySaveDetailID].ToString());
                }
                if (!(row[companyThirdPartyID] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyThirdPartyID = int.Parse(row[companyThirdPartyID].ToString());
                }
                if (!(row[saveGeneratedFileIn] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.SaveGeneratedFileIn = (row[saveGeneratedFileIn].ToString());
                }
                if (!(row[namingConvention] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.NamingConvention = row[namingConvention].ToString();
                }
                if (!(row[companyIdentifier] is System.DBNull))
                {
                    thirdPartyFlatFileSaveDetail.CompanyIdentifier = row[companyIdentifier].ToString();
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
            return thirdPartyFlatFileSaveDetail;
        }

        public static ThirdPartyFlatFileSaveDetail GetThirdPartyFlatFileSaveDetail(int thirdPartyID)
        {
            ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail = new ThirdPartyFlatFileSaveDetail();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFFThirdPartySaveDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFlatFileSaveDetail = FillCompanyThirdPartySaveDetail(row, 0);
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
            return thirdPartyFlatFileSaveDetail;
        }

        public static int SaveCompanyThirdPartyFileSaveDetail(ThirdPartyFlatFileSaveDetail thirdPartyFlatFileSaveDetail)
        {
            int result = int.MinValue;
            object[] parameter = new object[5];

            try
            {


                parameter[0] = thirdPartyFlatFileSaveDetail.CompanyThirdPartyID;
                parameter[1] = thirdPartyFlatFileSaveDetail.CompanyIdentifier;
                parameter[2] = thirdPartyFlatFileSaveDetail.NamingConvention;
                parameter[3] = thirdPartyFlatFileSaveDetail.SaveGeneratedFileIn;
                parameter[4] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveThirdPartyFileFormatsSaveDetails", parameter).ToString());

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

        #region CompanyThirdPartyCVIdentifiers
        public static ThirdPartyCVIdentifier FillCompanyThirdPartyCVIdentifiers(object[] row, int offSet)
        {
            int thirdPartyCVID = 0 + offSet;
            int companyThirdPartyID = 1 + offSet;
            int companyCounterPartyVenueID = 2 + offSet;
            int cvIdentifier = 3 + offSet;
            int cvName = 4 + offSet;

            ThirdPartyCVIdentifier companyThirdPartyCVIdentifier = new ThirdPartyCVIdentifier();
            try
            {

                if (!(row[thirdPartyCVID] is System.DBNull))
                {
                    companyThirdPartyCVIdentifier.ThirdPartyCVID = int.Parse(row[thirdPartyCVID].ToString());
                }
                if (!(row[companyThirdPartyID] is System.DBNull))
                {
                    companyThirdPartyCVIdentifier.CompanyThirdPartyID = int.Parse(row[companyThirdPartyID].ToString());
                }
                if (!(row[companyCounterPartyVenueID] is System.DBNull))
                {
                    companyThirdPartyCVIdentifier.CompanyCounterPartyVenueID = int.Parse(row[companyCounterPartyVenueID].ToString());
                }
                if (!(row[cvIdentifier] is System.DBNull))
                {
                    companyThirdPartyCVIdentifier.CVIdentifier = row[cvIdentifier].ToString();
                }
                if (!(row[cvName] is System.DBNull))
                {
                    companyThirdPartyCVIdentifier.CVName = row[cvName].ToString();
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
            return companyThirdPartyCVIdentifier;
        }

        public static ThirdPartyCVIdentifiers GetCompanyThirdPartyCVIdentifiers(int thirdpartyID)
        {
            ThirdPartyCVIdentifiers companyThirdPartyCVIdentifier = new ThirdPartyCVIdentifiers();

            object[] parameter = new object[1];
            parameter[0] = thirdpartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyCVIdentifiers", parameter))
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetCompanyThirdPartyFileFormats"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyThirdPartyCVIdentifier.Add(FillCompanyThirdPartyCVIdentifiers(row, 0));
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
            return companyThirdPartyCVIdentifier;
        }

        public static int SaveCompanyThirdPartyCVIdentifiers(ThirdPartyCVIdentifiers companyThirdPartyCVIdentifiers, int thirdPartyID)
        {
            int result = int.MinValue;

            try
            {
                Object[] parameter1 = new object[1];
                parameter1[0] = thirdPartyID;
                int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_ThirdPartyDeleteCVIdentifier", parameter1).ToString());

                object[] parameter = new object[4];

                foreach (ThirdPartyCVIdentifier companyThirdPartyCVIdentifier in companyThirdPartyCVIdentifiers)
                {
                    parameter[0] = companyThirdPartyCVIdentifier.CompanyThirdPartyID;
                    parameter[1] = companyThirdPartyCVIdentifier.CompanyCounterPartyVenueID;
                    parameter[2] = companyThirdPartyCVIdentifier.CVIdentifier;
                    parameter[3] = int.MinValue;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyThirdPartyCVIdentifiers", parameter).ToString());
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
        /// The method is used to fetch a CVIdentifier for a particular thirdParty of the company.
        /// </summary>
        /// <param name="thirdpartyID"></param>
        /// <param name="cVID"></param>
        /// <returns></returns>
        public static ThirdPartyCVIdentifier GetCompanyThirdPartyCVIdentifier(int thirdpartyID, int cVID)
        {
            ThirdPartyCVIdentifier companyThirdPartyCVIdentifier = new ThirdPartyCVIdentifier();

            object[] parameter = new object[2];
            parameter[0] = thirdpartyID;
            parameter[1] = cVID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyCVIdentifier", parameter))
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(CommandType.StoredProcedure, "P_GetCompanyThirdPartyFileFormats"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyThirdPartyCVIdentifier = FillCompanyThirdPartyCVIdentifiers(row, 0);
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
            return companyThirdPartyCVIdentifier;
        }

        #endregion

        #region CompanyThirdPartyMappingDetails

        public static ThirdPartyAccount FillCompanyThirdPartyMappingDetails(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            //int companyThirdPartyID_PK = 1 + offSet;
            int companyThirdPartyID = 1 + offSet;
            int internalAccountID = 2 + offSet;
            int mappedAccountName = 3 + offSet;
            int account = 4 + offSet;
            int accountTypeID = 5 + offSet;


            ThirdPartyAccount companyThirdPartyAccount = new ThirdPartyAccount();
            try
            {
                if (!(row[companyID] is System.DBNull))
                {
                    companyThirdPartyAccount.CompanyID_FK = int.Parse(row[companyID].ToString());
                }

                //if (!(row[companyThirdPartyID_PK] is System.DBNull))
                //{
                //    companyThirdPartyAccount.CompanyThirdPartyID_PK = int.Parse(row[companyThirdPartyID_PK].ToString());
                //}

                if (!(row[companyThirdPartyID] is System.DBNull))
                {
                    companyThirdPartyAccount.CompanyThirdPartyID_FK = int.Parse(row[companyThirdPartyID].ToString());
                }

                if (!(row[internalAccountID] is System.DBNull))
                {
                    companyThirdPartyAccount.InternalAccountID = int.Parse(row[internalAccountID].ToString());
                }
                if (!(row[mappedAccountName] is System.DBNull))
                {
                    companyThirdPartyAccount.MappedAccountName = row[mappedAccountName].ToString();
                }
                if (!(row[account] is System.DBNull))
                {
                    companyThirdPartyAccount.Account = row[account].ToString();
                }
                if (!(row[accountTypeID] is System.DBNull))
                {
                    companyThirdPartyAccount.AccountTypeID = int.Parse(row[accountTypeID].ToString());
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
            return companyThirdPartyAccount;
        }

        /// <summary>
        /// Created by: Bhupesh
        /// Modified by : Kanupriya 
        /// Change: The method now fetches a collection of ThirdPartyMapping Details for a partcular ThirdParty
        /// instead of a collection of  ThirdPartyMapping Details of all Third Parties for a Company. 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public static ThirdPartyAccounts GetCompanyThirdPartyMappingDetails(int thirdPartyID)
        {
            ThirdPartyAccounts companyThirdPartyAccounts = new ThirdPartyAccounts();

            object[] parameter = new object[1];
            parameter[0] = thirdPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyMappingDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyThirdPartyAccounts.Add(FillCompanyThirdPartyMappingDetails(row, 0));
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
            return companyThirdPartyAccounts;
        }


        public static ThirdPartyAccount GetCompanyThirdPartyMappingDetail(int thirdPartyID, int companyAccountID)
        {
            ThirdPartyAccount companyThirdPartyAccount = new ThirdPartyAccount();

            object[] parameter = new object[2];
            parameter[0] = thirdPartyID;
            parameter[1] = companyAccountID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_TPGetThirdPartyMappingDetailsperFund", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyThirdPartyAccount = FillCompanyThirdPartyMappingDetails(row, 0);
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
            return companyThirdPartyAccount;
        }

        public static int SaveCompanyThirdMappingDetails(ThirdPartyAccounts companyThirdPartyAccounts, int thirdPartyID)
        {
            int result = int.MinValue;

            object[] parameter1 = new object[1];
            object[] parameter = new object[7];

            try
            {
                parameter1[0] = thirdPartyID;
                int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_ThirdPartyDeleteFundMappingDetails", parameter1).ToString());

                foreach (ThirdPartyAccount companyThirdPartyAccount in companyThirdPartyAccounts)
                {
                    parameter[0] = companyThirdPartyAccount.CompanyThirdPartyID_FK;
                    parameter[1] = companyThirdPartyAccount.InternalAccountID;
                    parameter[2] = companyThirdPartyAccount.MappedAccountName;
                    parameter[3] = companyThirdPartyAccount.Account;
                    parameter[4] = companyThirdPartyAccount.AccountTypeID;
                    parameter[5] = companyThirdPartyAccount.CompanyID_FK;
                    parameter[6] = int.MinValue;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyThirdMappingDetails", parameter).ToString());
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

        #region CompanyThirdPartyAccountCommissionRules

        public static ThirdPartyAccountCommissionRule FillCompanyThirdPartyAccountCommissionRule(object[] row, int offSet)
        {
            int companyAccountID = 0 + offSet;
            int accountName = 1 + offSet;
            int companyCounterPartyCVID = 2 + offSet;
            int cVAUECID = 3 + offSet;
            int singleRuleID = 4 + offSet;
            int basketRuleID = 5 + offSet;

            ThirdPartyAccountCommissionRule thirdPartyAccountCommissionRule = new ThirdPartyAccountCommissionRule();
            try
            {

                if (!(row[companyAccountID] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.CompanyAccountID = int.Parse(row[companyAccountID].ToString());
                }
                if (!(row[accountName] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.AccountName = row[accountName].ToString();
                }
                if (!(row[companyCounterPartyCVID] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.CompanyCounterPartyCVID = int.Parse(row[companyCounterPartyCVID].ToString());
                }
                if (!(row[cVAUECID] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.CVAUECID = int.Parse(row[cVAUECID].ToString());
                }

                if (!(row[singleRuleID] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.SingleRuleID = int.Parse(row[singleRuleID].ToString());
                }
                if (!(row[basketRuleID] is System.DBNull))
                {
                    thirdPartyAccountCommissionRule.BasketRuleID = int.Parse(row[basketRuleID].ToString());
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
            return thirdPartyAccountCommissionRule;
        }

        public static ThirdPartyAccountCommissionRules GetCompanyThirdPartyAccountCommissionRules(int companyID)
        {
            ThirdPartyAccountCommissionRules thirdPartyAccountCommissionRules = new ThirdPartyAccountCommissionRules();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyThirdPartyFundCommissionRules", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyAccountCommissionRules.Add(FillCompanyThirdPartyAccountCommissionRule(row, 0));
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
            return thirdPartyAccountCommissionRules;
        }

        //The following method's implementation has been moved to the method name "SaveCompanyCommissionRulesForCVAUEC" in the class Nirana.CommissionRules.CommissionDBManager.
        public static int SaveCompanyThirdMappingAccountCommissionRules(ThirdPartyAccountCommissionRules companyThirdPartyAccountCommissionRules, int companyID)
        {
            int result = int.MinValue;

            //Database db = DatabaseFactory.CreateDatabase();

            //object[] parameter = new object[6];

            //try
            //{

            //    db.ExecuteNonQuery("P_DeleteCompanyThirdMappingFundCommissionRules", companyID);


            //    foreach (ThirdPartyAccountCommissionRule companyThirdPartyAccountCommissionRule in companyThirdPartyAccountCommissionRules)
            //    {
            //        parameter[0] = companyThirdPartyAccountCommissionRule.CompanyAccountID;
            //        parameter[1] = companyThirdPartyAccountCommissionRule.CompanyCounterPartyCVID;
            //        parameter[2] = companyThirdPartyAccountCommissionRule.CVAUECID;
            //        parameter[3] = companyThirdPartyAccountCommissionRule.SingleRuleID;
            //        parameter[4] = companyThirdPartyAccountCommissionRule.BasketRuleID;
            //        parameter[5] = int.MinValue;                  

            //        result = int.Parse(db.ExecuteScalar("P_SaveCompanyThirdMappingFundCommissionRules", parameter).ToString());
            //    }
            //}
            //#region Catch
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDTHROW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            //#endregion

            return result;
        }
        /// <summary>
        /// Created by : Kanupriya
        /// purpose: to fetch the commission rule applicable on a account for  a particular account and cv combination.
        /// </summary>
        /// <param name="companyFundID"></param>
        /// <param name="cVID"></param>
        /// <returns></returns>
        public static ThirdPartyAccountCommissionRule GetThirdPartyAccountCommissionRule(int companyAccountID, int cVID, int auecID)
        {
            ThirdPartyAccountCommissionRule thirdPartyAccountCommissionRule = new ThirdPartyAccountCommissionRule();

            object[] parameter = new object[3];
            parameter[0] = companyAccountID;
            parameter[1] = cVID;
            parameter[2] = auecID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_TPGetFundCommissionRule", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyAccountCommissionRule = FillCompanyThirdPartyAccountCommissionRule(row, 0);
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
            return thirdPartyAccountCommissionRule;
        }

        #endregion

        #region CompanyThirdPartyAccounts

        /// <summary>
        /// the method is used to fetch those company accounts which have not been allocated to the selected third party.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="thirdPartyID"></param>
        /// <returns></returns>
        public static Accounts GetCompanyAccountsNotPermittedToThirdParty(int companyID, int thirdPartyID)
        {
            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = thirdPartyID;
            Accounts accounts = new Accounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyFundsNotPermittedToThirdParty", parameter))
                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetCompanyFundsNotAllocatedToanyParty", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accounts.Add(FillAccount(row, 0));
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
            return accounts;
        }

        /// <summary>
        /// the method is used to fetch those companyaccounts which have not been allocated to any of the third party of the selected type.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="thirdPartyTypeID"></param>
        /// <returns></returns>
        public static Accounts GetThirdPartyCompanyUnallocatedAccounts(int companyID, int thirdPartyTypeID)
        {
            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = thirdPartyTypeID;
            Accounts accounts = new Accounts();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetThirdPartyCompanyUnallocatedFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accounts.Add(FillAccount(row, 0));
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
            return accounts;
        }
        #endregion CompanyThirdPartyAccounts

        #endregion

        #region CompanySetUpContracts
        public static SetUpContract FillCompanySetUpContracts(object[] row, int offSet)
        {
            int symbol = 0 + offSet;
            int auecID = 1 + offSet;
            int multiplier = 2 + offSet;
            int contractMonthID = 3 + offSet;
            int companyID = 4 + offSet;
            int companySetUpContractID = 5 + offSet;
            int description = 6 + offSet;

            SetUpContract setUpContract = new SetUpContract();
            try
            {

                if (!(row[symbol] is System.DBNull))
                {
                    setUpContract.Symbol = row[symbol].ToString();
                }
                if (!(row[auecID] is System.DBNull))
                {
                    setUpContract.AuecID = int.Parse(row[auecID].ToString());
                }
                if (!(row[multiplier] is System.DBNull))
                {
                    setUpContract.Multiplier = double.Parse(row[multiplier].ToString());
                }
                if (!(row[contractMonthID] is System.DBNull))
                {
                    setUpContract.ContractMonthID = int.Parse(row[contractMonthID].ToString());
                }
                if (!(row[companyID] is System.DBNull))
                {
                    setUpContract.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (!(row[companySetUpContractID] is System.DBNull))
                {
                    setUpContract.CompanySetUpContractID = int.Parse(row[companySetUpContractID].ToString());
                }
                if (!(row[description] is System.DBNull))
                {
                    setUpContract.Description = row[description].ToString();
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
            return setUpContract;
        }

        public static SetUpContracts GetSetUpContracts(int companyID)
        {
            SetUpContracts setUpContracts = new SetUpContracts();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSetUpContracts", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        setUpContracts.Add(FillCompanySetUpContracts(row, 0));
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
            return setUpContracts;
        }

        public static int SaveSetUpContracts(SetUpContracts setUpContracts, int companyID)
        {
            int result = int.MinValue;
            StringBuilder companyContractSetUpIDs = new StringBuilder();

            object[] parameter = new object[8];
            try
            {
                foreach (SetUpContract setUpContract in setUpContracts)
                {
                    parameter = new object[8];
                    parameter[0] = setUpContract.Symbol;
                    parameter[1] = setUpContract.AuecID;
                    parameter[2] = setUpContract.Multiplier;
                    parameter[3] = setUpContract.ContractMonthID;
                    //parameter[3] = 1; //Hard codded for now.
                    parameter[4] = companyID;
                    parameter[5] = setUpContract.CompanySetUpContractID;
                    parameter[6] = setUpContract.Description;
                    parameter[7] = int.MinValue;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSetUpContracts", parameter).ToString());

                    companyContractSetUpIDs.Append("'");
                    companyContractSetUpIDs.Append(result.ToString());
                    companyContractSetUpIDs.Append("',");
                }

                int len = companyContractSetUpIDs.Length;
                if (companyContractSetUpIDs.Length > 0)
                {
                    companyContractSetUpIDs.Remove((len - 1), 1);
                }
                parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = companyContractSetUpIDs.ToString();
                if (companyContractSetUpIDs.Length > 0)
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSetUpContracts", parameter).ToString();
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

        public static bool DeleteSetUpContract(int companySetUpContractID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companySetUpContractID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSetUpContractByID", parameter) > 0)
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

        #region CompanyEMSSources


        public static ImportTradeXSLTFileCollection GetCompanyEMSSources(int companyID)
        {

            ImportTradeXSLTFileCollection emsSources = new ImportTradeXSLTFileCollection();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyEMSSources", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        emsSources.Add(FillCompanyEMSSource(row));
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

            return emsSources;
        }

        private static ImportTradeXSLTFile FillCompanyEMSSource(object[] row)
        {
            ImportTradeXSLTFile importTrade = new ImportTradeXSLTFile();

            if (row != null)
            {

                int IMPORTSOURCEID = 0;
                int IMPORTSOURCENAME = 1;
                int XSLTFILEID = 2;
                int XSLTFILENAME = 3;

                importTrade.ImportSourceID = Convert.ToInt32(row[IMPORTSOURCEID]);
                importTrade.EMSSource = Convert.ToString(row[IMPORTSOURCENAME]);
                importTrade.FileID = Convert.ToInt32(row[XSLTFILEID]);
                importTrade.FileName = Convert.ToString(row[XSLTFILENAME]);
            }
            return importTrade;
        }

        public static void SaveCompanyEMSSource(int companyID, ImportTradeXSLTFileCollection emsSourcesChecked, ImportTradeXSLTFileCollection emsSourcesUnchecked)
        {
            try
            {
                foreach (ImportTradeXSLTFile emsSource in emsSourcesChecked)
                {
                    object[] parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = emsSource.ImportSourceID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyEMSSource", parameter);
                }
                foreach (ImportTradeXSLTFile emsSource in emsSourcesUnchecked)
                {
                    object[] parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = emsSource.ImportSourceID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyEMSSource", parameter);
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

        #endregion

        /// <summary>
        /// Getting Compliance Alerting Preferences.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static CompliancePref GetCompliancePreferences(int companyID)
        {
            CompliancePref compliancePref = new CompliancePref();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_CA_GetCompliancePreferences", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[1] != DBNull.Value)
                        {
                            compliancePref.ImportExportPath = row[1].ToString();
                        }
                        if (row[2] != DBNull.Value)
                        {
                            compliancePref.PrePostCrossImportAllowed = Convert.ToBoolean(row[2].ToString());
                        }
                        if (row[3] != DBNull.Value)
                        {
                            compliancePref.InMarket = Convert.ToBoolean(row[3].ToString());
                        }
                        if (row[4] != DBNull.Value)
                        {
                            compliancePref.InStage = Convert.ToBoolean(row[4].ToString());
                        }
                        if (row[5] != DBNull.Value)
                        {
                            compliancePref.PostInMarket = Convert.ToBoolean(row[5].ToString());
                        }
                        if (row[6] != DBNull.Value)
                        {
                            compliancePref.PostInStage = Convert.ToBoolean(row[6].ToString());
                        }
                        if (row[7] != DBNull.Value)
                        {
                            compliancePref.BlockTradeOnComplianceFaliure = Convert.ToBoolean(row[7].ToString());
                        }
                        if (row[8] != DBNull.Value)
                        {
                            compliancePref.StageValueFromField = Convert.ToBoolean(row[8].ToString());
                        }
                        if (row[9] != DBNull.Value)
                        {
                            compliancePref.StageValueFromFieldString = row[9].ToString();
                        }
                        if (row[10] != DBNull.Value)
                        {
                            compliancePref.IsBasketComplianceEnabledCompany = Convert.ToBoolean(row[10].ToString());
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
            return compliancePref;
        }


        public static PMUIPrefs GetCompanyPMPreferences(int companyID)
        {
            PMUIPrefs uiPrefs = new PMUIPrefs();

            try
            {

                object[] parameter = new object[1];
                parameter[0] = companyID;
                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPMUIPrefsNew", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            if (row[0] != DBNull.Value)
                            {
                                uiPrefs.NumberOfCustomViewsAllowed = Convert.ToInt32(row[0].ToString());
                            }
                            if (row[1] != DBNull.Value)
                            {
                                uiPrefs.NumberOfVisibleColumnsAllowed = Convert.ToInt32(row[1].ToString());
                            }
                            if (row[2] != DBNull.Value)
                            {
                                uiPrefs.FetchDataFromHistoricalDb = Convert.ToBoolean(row[2].ToString());
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
            return uiPrefs;
        }

        public static DataSet GetPMCalculationPreferences(int companyID)
        {
            DataSet dsPMCalculationPrefs = new DataSet();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                dsPMCalculationPrefs = DatabaseManager.DatabaseManager.ExecuteDataSet("GetPMCalculationPrefValues", parameter);
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
            return dsPMCalculationPrefs;
        }

        /// <summary>
        /// This function fatch the autogrouping preferences from database.
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAutoGroupingPreferences()
        {
            DataTable dsAutoGroupingPrefs = new DataTable();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAutoGroupingRulePref";

            try
            {
                dsAutoGroupingPrefs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData).Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dsAutoGroupingPrefs;
        }

        public static DataSet GetCompanyRiskPreferences(int companyID)
        {
            DataSet dsMaxViews = new DataSet();
            //ds.Tables.Add("MaximumViews");
            //ds.Tables[0].Columns.Add("MaxStressTestViewsWithVolSkew");
            //ds.Tables[0].Columns.Add("MaxStressTestViewsWithoutVolSkew");

            try
            {

                object[] parameter = new object[1];
                parameter[0] = companyID;

                dsMaxViews = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetCompanyRiskUIPrefs", parameter);
                //using (SqlDataReader  = (SqlDataReader)db.ExecuteReader("P_GetCompanyRiskUIPrefs", parameter))
                //{
                //    while (reader.Read())
                //    {
                //        DataRow dr = ds.Tables[0].NewRow();
                //        object[] row = new object[reader.FieldCount];
                //        reader.GetValues(row);
                //        if (row[0] != DBNull.Value)
                //        {
                //            dr["MaxStressTestViewsWithVolSkew"] = Convert.ToInt32(row[0].ToString());
                //        }
                //        if (row[1] != DBNull.Value)
                //        {
                //            dr["MaxStressTestViewsWithoutVolSkew"] = Convert.ToInt32(row[1].ToString());
                //        }
                //    }
                //}

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
            return dsMaxViews;

        }

        public static void SaveCompanyPMPreferences(int companyID, PMUIPrefs pmUIPrefs)
        {
            try
            {
                object[] parameter = new object[4];
                parameter[0] = companyID;
                parameter[1] = pmUIPrefs.NumberOfCustomViewsAllowed;
                parameter[2] = pmUIPrefs.NumberOfVisibleColumnsAllowed;
                parameter[3] = pmUIPrefs.FetchDataFromHistoricalDb;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyPMUIPrefs", parameter);
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
        /// <summary>
        /// this function is use to save the autogrouping preferences into database.
        /// </summary>
        /// <param name="companyID">int</param>
        /// <param name="prefDict">Dictionary<string,bool></param>
        public static void SaveCompanyAutoGroupingPrefs(int companyID, string funds, Dictionary<string, bool> prefDict)
        {
            try
            {
                object[] parameter = new object[14];
                parameter[0] = companyID;
                parameter[1] = prefDict["enableAutoGroup"];
                if (funds == string.Empty)
                    parameter[2] = -1;
                else
                    parameter[2] = funds;
                parameter[3] = prefDict["chkTradeAttribute1"];
                parameter[4] = prefDict["chkTradeAttribute2"];
                parameter[5] = prefDict["chkTradeAttribute3"];
                parameter[6] = prefDict["chkTradeAttribute4"];
                parameter[7] = prefDict["chkTradeAttribute5"];
                parameter[8] = prefDict["chkTradeAttribute6"];
                parameter[9] = prefDict["chkBroker"];
                parameter[10] = prefDict["chkVenue"];
                parameter[11] = prefDict["chkTradingAC"];
                parameter[12] = prefDict["chkTradeDate"];
                parameter[13] = prefDict["chkProcessDate"];
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyAutoGroupingPrefs", parameter);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            #endregion
        }

        public static int SaveDefaultAUECData(DataTable dt)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveDefaultAUECData";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }
        public static int SaveShortLocatePreferencesDB(DataTable dt)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveShortLocateGridPreferences";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        public static int SaveCompanyPMCalculationPreferences(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                //if (dtDataToSave != null)
                //{
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDataToSave.Copy());
                string generatedXml = string.Empty; ;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "SavePMCalculationPrefValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        public static void SaveCompanyRiskPreferences(int companyID, RiskPrefernece riskUIPrefs)
        {
            try
            {

                object[] parameter = new object[3];
                parameter[0] = companyID;
                parameter[1] = riskUIPrefs.MaxStressTestViewsWithVolSkew;
                parameter[2] = riskUIPrefs.MaxStressTestViewsWithoutVolSkew;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyRiskUIPrefs", parameter);


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

        public static void SaveCompliancePrefernces(int companyId, CompliancePref compliancePref)
        {
            try
            {
                object[] parameter = new object[11];
                parameter[0] = companyId;
                parameter[1] = compliancePref.ImportExportPath;
                parameter[2] = compliancePref.PrePostCrossImportAllowed;
                parameter[3] = compliancePref.InMarket;
                parameter[4] = compliancePref.InStage;
                parameter[5] = compliancePref.PostInMarket;
                parameter[6] = compliancePref.PostInStage;
                parameter[7] = compliancePref.BlockTradeOnComplianceFaliure;
                parameter[8] = compliancePref.StageValueFromField;
                parameter[9] = compliancePref.StageValueFromFieldString;
                parameter[10] = compliancePref.IsBasketComplianceEnabledCompany;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveCompliancePreference", parameter);
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

        /// <summary>
        /// Save compliance permissions.
        /// Read, Read/Write split into sub parts
        /// different permissions for enable, delete, update 
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-3753 
        /// </summary>
        /// <param name="compliancePermissions"></param>
        /// <returns></returns>

        public static bool SaveCompliancePermissions(CompliancePermissions compliancePermissions)
        {
            try
            {
                int result = 0;

                object[] parameter = new object[12];
                parameter[0] = compliancePermissions.CompanyId;
                parameter[1] = compliancePermissions.CompanyUserId;
                parameter[2] = compliancePermissions.IsPowerUser;
                parameter[3] = compliancePermissions.RuleCheckPermission.IsApplyToManual;
                parameter[4] = compliancePermissions.RuleCheckPermission.IsOverridePermission;
                parameter[5] = compliancePermissions.RuleCheckPermission.IsPreTradeEnabled;
                parameter[6] = compliancePermissions.RuleCheckPermission.IsTrading;
                parameter[7] = compliancePermissions.RuleCheckPermission.IsStaging;
                parameter[8] = compliancePermissions.RuleCheckPermission.DefaultPrePopUpEnabled;
                parameter[9] = compliancePermissions.RuleCheckPermission.DefaultPostPopUpEnabled;
                parameter[10] = (int)compliancePermissions.RuleCheckPermission.DefaultOverRideType;
                parameter[11] = compliancePermissions.EnableBasketComplianceCheck;
                result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveOverridePreCheckAndPowerUser", parameter).ToString());

                if (result > 0)
                {
                    result = 0;
                    foreach (RuleType ruleType in compliancePermissions.complianceUIPermissions.Keys)
                    {
                        object[] parameter1 = new object[9];

                        parameter1[0] = compliancePermissions.CompanyId;
                        parameter1[1] = compliancePermissions.CompanyUserId;
                        parameter1[2] = ruleType;

                        parameter1[3] = compliancePermissions.complianceUIPermissions[ruleType].IsCreate;
                        parameter1[4] = compliancePermissions.complianceUIPermissions[ruleType].IsRename;
                        parameter1[5] = compliancePermissions.complianceUIPermissions[ruleType].IsDelete;
                        parameter1[6] = compliancePermissions.complianceUIPermissions[ruleType].IsEnable;
                        parameter1[7] = compliancePermissions.complianceUIPermissions[ruleType].IsExport;
                        parameter1[8] = compliancePermissions.complianceUIPermissions[ruleType].IsImport;
                        result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveComplianceUserPermissions", parameter1).ToString());
                    }
                }


                if (result > 0)
                    return true;
                else return false;
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
                return false;
            }

        }

        public static void SaveCompanyCAPreferences(int companyID, CAPreferences caPreferences)
        {
            object[] parameter = new object[2];
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            #region try
            try
            {

                bf.Serialize(stream, caPreferences);
                byte[] data = new byte[stream.Length];
                stream.Write(data, 0, data.Length);
                stream.Seek(0, 0);

                parameter[0] = companyID;
                parameter[1] = stream.ToArray(); //Convert.ToBase64String(data);

                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCAPreferences", parameter);

            }
            # endregion
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

        public static CAPreferences GetCompanyCAPreferences(int companyID)
        {
            CAPreferences preferences = new CAPreferences();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCAPrefs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!row[0].Equals(System.DBNull.Value))
                        {
                            byte[] data = (byte[])row[0];

                            MemoryStream stream = new MemoryStream(data);
                            BinaryFormatter bf = new BinaryFormatter();
                            preferences = (CAPreferences)bf.Deserialize(stream);

                        }
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

            return preferences;
        }

        /// <summary>
        /// Author : Rajat
        /// To return the pair of basecurrencyid and basecurrencyname
        /// can be used for caching different base currencies needs like company
        /// </summary>
        public class BaseCurrency
        {
            public int BaseCurrencyId;
            public string BaseCurrencyName;
        }

        public static IList<MarketDataType> GetMarketDataTypes()
        {
            IList<MarketDataType> rtn = new List<MarketDataType>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMarketDataTypes";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        MarketDataType type = new MarketDataType();
                        if (row[0] != null)
                            type.MarketDataTypeID = int.Parse(row[0].ToString());
                        if (row[1] != null)
                            type.MarketDataTypeName = row[1].ToString();
                        rtn.Add(type);
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
            return rtn;
        }

        public static bool SaveMarketDataTypesForUser(int userID, IList<MarketDataType> types)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteMarketDataTypesForUser", parameter).ToString();
                foreach (MarketDataType type in types)
                {
                    parameter = new object[2];
                    parameter[0] = userID;
                    parameter[1] = type.MarketDataTypeID;

                    DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveMarketDataTypeForUser", parameter);

                }
                return true;
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

        public static bool SaveAssetPermissionForUser(int userID)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAssetPermissionForUser", parameter).ToString();
                return true;
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


        public static IList<MarketDataType> GetMarketDataTypesForUser(int userID)
        {
            object[] parameter = new object[1];
            parameter[0] = userID;
            IList<MarketDataType> types = new List<MarketDataType>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetMarketDataTypesForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        MarketDataType type = new MarketDataType();
                        if (row[0] != null)
                            type.MarketDataTypeID = int.Parse(row[0].ToString());
                        if (row[1] != null)
                            type.MarketDataTypeName = row[1].ToString();
                        types.Add(type);
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
            return types;
        }

        public static IDictionary<string, IList<string>> GetMarketDataModues()
        {
            IDictionary<string, IList<string>> typeModues = new Dictionary<string, IList<string>>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetModuleForMarketData";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string type = row[0].ToString();
                        string module = row[1].ToString();
                        IList<string> list = null;
                        if (typeModues.ContainsKey(type))
                        {
                            list = typeModues[type];
                        }
                        else
                        {
                            list = new List<string>();
                            typeModues.Add(type, list);
                        }
                        if (!list.Contains(module))
                        {
                            list.Add(module);
                        }
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
            return typeModues;
        }

        public static Dictionary<int, string> GetUserPermittedCompanyList(int companyUserID)
        {
            Clients clients = new Clients();
            Dictionary<int, string> userPermittedCompanies = new Dictionary<int, string>();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_UserPermittedCompanies", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clients.Add(FillClient(row, 0));
                    }
                }
                if (clients.Count > 0)
                {
                    foreach (Client client in clients)
                    {
                        if (!userPermittedCompanies.ContainsKey(client.ClientID))
                            userPermittedCompanies.Add(client.ClientID, client.ClientName);
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
            return userPermittedCompanies;
        }

        /// <summary>
        /// Save Individual Rule OverRidden Permission
        /// </summary>
        /// <param name="companyUserID">User Id</param>
        /// <param name="list">List Of Rules</param>
        /// <returns>True or False</returns>
        public static bool SaveOverRiddenRulePermission(int companyUserID, DataTable dt)
        {
            bool isSaved = false;
            try
            {
                int rowsAffected = 0;
                int errorNumber = 0;
                DataSet ds = new DataSet("Permissions");
                ds.Tables.Add(dt);
                string errorMessage = string.Empty;
                ds.Tables[0].TableName = "Permission";
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_SetRuleOverRiddenPermission";
                queryData.DictionaryDatabaseParameter.Add("@UserId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@UserId",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyUserID
                });
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, queryData.DictionaryDatabaseParameter);

                isSaved = true;
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
            return isSaved;
        }

        public static void SaveTTPreferencesForMultipleUsers(object[] parameter, int flag)
        {
            try
            {
                if (flag == 0)
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTTPrefsForMultipleUsers", parameter);
                }
                else
                {
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CopyTTPrefsFromOneUserToAnotherUsers", parameter);
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

        public static OrderTypes GetCompanyOrderTypes(int companyID)
        {
            OrderTypes orderTypes = new OrderTypes();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyOrderTypes", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillCompanyOrderType(row, 0));
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
            return orderTypes;
        }

        public static OrderType FillCompanyOrderType(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int ordertypeID = 1 + offSet;

            OrderType orderType = new OrderType();
            try
            {
                if (row[companyID] != null)
                {
                    orderType.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[ordertypeID] != null)
                {
                    orderType.OrderTypesID = int.Parse(row[ordertypeID].ToString());
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
            return orderType;
        }

        public static TimeInForces GetCompanyTimeInForces(int companyID)
        {
            TimeInForces timeInForces = new TimeInForces();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyTimeInForces", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    timeInForces.Add(FillCompanyTimeInForce(row, 0));
                }
            }
            return timeInForces;
        }

        public static TimeInForce FillCompanyTimeInForce(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int timeInForceID = 1 + offSet;

            TimeInForce timeInForce = new TimeInForce();
            try
            {
                if (row[companyID] != null)
                {
                    timeInForce.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[timeInForceID] != null)
                {
                    timeInForce.TimeInForceID = int.Parse(row[timeInForceID].ToString());
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
            return timeInForce;
        }

        public static HandlingInstructions GetCompanyHandlingInstructions(int companyID)
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyHandlingInstructions", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    handlingInstructions.Add(FillCompanyHandlingInstruction(row, 0));
                }
            }
            return handlingInstructions;
        }

        public static HandlingInstruction FillCompanyHandlingInstruction(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int handlingInstructionID = 1 + offSet;

            HandlingInstruction handlingInstruction = new HandlingInstruction();
            try
            {
                if (row[companyID] != null)
                {
                    handlingInstruction.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[handlingInstructionID] != null)
                {
                    handlingInstruction.HandlingInstructionID = int.Parse(row[handlingInstructionID].ToString());
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
            return handlingInstruction;
        }


        public static ExecutionInstructions GetCompanyExecutionInstructions(int companyID)
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyExecutionInstructions", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    executionInstructions.Add(FillCompanyExecutionInstruction(row, 0));
                }
            }
            return executionInstructions;
        }

        public static ExecutionInstruction FillCompanyExecutionInstruction(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int executionInstructionID = 1 + offSet;

            ExecutionInstruction executionInstruction = new ExecutionInstruction();
            try
            {
                if (row[companyID] != null)
                {
                    executionInstruction.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[executionInstructionID] != null)
                {
                    executionInstruction.ExecutionInstructionsID = int.Parse(row[executionInstructionID].ToString());
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
            return executionInstruction;
        }

        public static void DeleteBorrowerBrokerAccountDB(int id)
        {
            object[] parameter = new object[1];

            try
            {
                parameter[0] = id;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteBorrowerBrokerAccountRow", parameter);
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


        public static void SaveShortLocatePreferenceParameters(object[] parameter)
        {
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveShortLocateParemetersPreference", parameter);
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

        public static bool SaveCompanyOrderTypes(int companyID, OrderTypes orderTypes)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyOrderTypes", parameter).ToString();

                foreach (OrderType orderType in orderTypes)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = orderType.OrderTypesID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyOrderTypes", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        public static bool SaveCompanyTimeInForces(int companyID, TimeInForces timeInForces)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyTimeInForces", parameter).ToString();

                foreach (TimeInForce timeInForce in timeInForces)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = timeInForce.TimeInForceID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyTimeInForces", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        public static bool SaveCompanyHandlingInstructions(int companyID, HandlingInstructions handlingInstructions)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyHandlingInstructions", parameter).ToString();

                foreach (HandlingInstruction handlingInstruction in handlingInstructions)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = handlingInstruction.HandlingInstructionID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyHandlingInstructions", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        public static bool SaveCompanyExecutionInstructions(int companyID, ExecutionInstructions executionInstructions)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyExecutionInstructions", parameter).ToString();

                foreach (ExecutionInstruction executionInstruction in executionInstructions)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = executionInstruction.ExecutionInstructionsID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyExecutionInstructions", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }


        /// <summary>
        /// Save Company Allocation CheckSide Pref
        /// </summary>
        /// <param name="p"></param>
        /// <param name="checkSidePref"></param>
        public static bool SaveCompanyAllocationCheckSidePref(int companyId, AllocationCheckSidePref checkSidePref)
        {
            try
            {

                string disableCheckSidePrefJsonString = JsonHelper.SerializeObject(checkSidePref);

                int rows = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_AL_SaveAllocationCheckSidePref", new object[]
                        {
                            companyId,
                            disableCheckSidePrefJsonString

                        });
                return true;
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
                return false;
            }
        }

        public static AllocationCheckSidePref GetAllocationCheckSidePref(int companyId)
        {
            try
            {
                AllocationCheckSidePref checkSidePref = new AllocationCheckSidePref();

                DataSet dsAllocationRule = DatabaseManager.DatabaseManager.ExecuteDataSet("P_AL_GetAllocationCheckSidePref", new object[] { companyId });
                foreach (DataRow row in dsAllocationRule.Tables[0].Rows)
                {
                    checkSidePref = JsonHelper.DeserializeToObject<AllocationCheckSidePref>(row["CheckSidePreference"].ToString());
                }

                if (checkSidePref == null)
                {
                    checkSidePref = new AllocationCheckSidePref()
                    {
                        DoCheckSideSystem = true
                    };
                }

                return checkSidePref;
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
                return null;
            }
        }

        /// <summary>
        /// Saves the read write permission of Restricted/Allowed securities list
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="readWritePermissionID"></param>
        public static bool SaveUsersSecuritiesListPermission(int companyUserID, int readWritePermissionID)
        {
            bool result = false;
            object[] parameter = new object[2];
            try
            {
                parameter[0] = companyUserID;
                parameter[1] = readWritePermissionID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveUsersSecuritiesListPermission", parameter).ToString();
                result = true;
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

        /// <summary>
        /// This method inserts default value for hot key preferences if saving for new user
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        public static bool SaveHotKeyPreferences(int companyUserID)
        {
            bool result = false;
            object[] parameter = new object[1];
            try
            {
                parameter[0] = companyUserID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyHotKeyPreferences", parameter).ToString();
                result = true;
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

        /// <summary>
        /// Gets the read write permission of Restricted/Allowed securities list
        /// </summary>
        /// <param name="userID"></param>
        public static int GetUserSecuritiesListPermission(int companyUserID)
        {
            object[] parameters = new object[1];
            parameters[0] = companyUserID;
            int readWritePermissionID = 0;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUsersSecuritiesListPermission", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int readWriteID = 0;
                            if (row[readWriteID] != System.DBNull.Value)
                            {
                                readWritePermissionID = Convert.ToInt32(row[readWriteID]);
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

            return readWritePermissionID;
        }

        /// <summary>       
        /// purpose: Get the dictionary of the accountID-CashPreference pairs from DB
        /// </summary>
        public static Dictionary<int, CashPreferences> GetCashPreferencesFromDB()
        {
            Dictionary<int, CashPreferences> _dictCashPreferences = new Dictionary<int, CashPreferences>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCashPreferences";

                DataSet dsCash = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (dsCash != null && dsCash.Tables.Count > 0 && dsCash.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCash.Tables[0].Rows)
                    {
                        CashPreferences objCashPref = new CashPreferences();
                        if (!string.IsNullOrEmpty(dr["FundID"].ToString()))
                        {
                            objCashPref.AccountID = Convert.ToInt32(dr["FundID"]);
                        }
                        if (!string.IsNullOrEmpty(dr["CashMgmtStartDate"].ToString()))
                        {
                            objCashPref.CashMgmtStartDate = Convert.ToDateTime(dr["CashMgmtStartDate"]);
                        }
                        if (!string.IsNullOrEmpty(dr["MarginPercentage"].ToString()))
                        {
                            objCashPref.MarginPercentage = Convert.ToDouble(dr["MarginPercentage"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IsCalculatePnL"].ToString()))
                        {
                            objCashPref.IsCalculatePnL = Convert.ToBoolean(dr["IsCalculatePnL"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IsCalulateDividend"].ToString()))
                        {
                            objCashPref.IsCalculateDividend = Convert.ToBoolean(dr["IsCalulateDividend"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IsCalculateBondAccural"].ToString()))
                        {
                            objCashPref.IsCalculateBondAccurals = Convert.ToBoolean(dr["IsCalculateBondAccural"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IsCalculateCollateral"].ToString()))
                        {
                            objCashPref.IsCalculateCollateral = Convert.ToBoolean(dr["IsCalculateCollateral"]);
                        }
                        if (!string.IsNullOrEmpty(dr["CollateralFrequencyInterest"].ToString()))
                        {
                            objCashPref.IsCalculateCollateralFrequencyInterest = Convert.ToString(dr["CollateralFrequencyInterest"]);
                        }

                        if (!string.IsNullOrEmpty(dr["IsBreakRealizedPnlSubaccount"].ToString()))
                        {
                            objCashPref.IsRealizedPL = Convert.ToBoolean(dr["IsBreakRealizedPnlSubaccount"]);
                        }

                        if (!string.IsNullOrEmpty(dr["IsBreakTotalIntoTradingAndFxPnl"].ToString()))
                        {
                            objCashPref.IsTotalPL = Convert.ToBoolean(dr["IsBreakTotalIntoTradingAndFxPnl"]);
                        }

                        #region CHMW-3141
                        //[Foreign Positions Settling in Base Currency] Add preferences in cash management module to show/hide settlement journal entries
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3141
                        if (!string.IsNullOrEmpty(dr["IsCashSettlementEntriesVisible"].ToString()))
                        {
                            objCashPref.IsCashSettlementEntriesVisible = Convert.ToBoolean(dr["IsCashSettlementEntriesVisible"]);
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(dr["IsAccruedTillSettlement"].ToString()))
                        {
                            objCashPref.IsAccruedTillSettlement = Convert.ToBoolean(dr["IsAccruedTillSettlement"]);
                        }
                        if (!string.IsNullOrEmpty(dr["SymbolWiseRevaluationDate"].ToString()))
                        {
                            objCashPref.SymbolWiseRevaluationDate = Convert.ToDateTime(dr["SymbolWiseRevaluationDate"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IsCreateManualJournals"].ToString()))
                        {
                            objCashPref.IsCreateManualJournals = Convert.ToBoolean(dr["IsCreateManualJournals"]);
                        }
                        if (!_dictCashPreferences.ContainsKey(objCashPref.AccountID))
                        {
                            _dictCashPreferences.Add(objCashPref.AccountID, objCashPref);
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
            return _dictCashPreferences;
        }
    }

}
