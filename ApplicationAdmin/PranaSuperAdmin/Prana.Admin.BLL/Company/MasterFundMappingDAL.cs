using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    class MasterFundMappingDAL
    {
        //Modified By: Bharat raturi, date: 05/03/2014
        //Purpose: Change the database connection style, use Microsoft Enterprise Library objects 

        /// <summary>
        /// Initialitied Connection string for data base 
        /// </summary>
        //static String _connectionString = "Database=NirvanaClient;Server=(local);Integrated Security=false;User ID=sa;Password=NIRvana2@@6;";
        const string _connectionString = "PranaConnectionString";


        /// <summary>
        /// Load all master fund from Data base and add into _masterFundCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadMasterFundFromDb(int CompanyID)
        {
            Dictionary<int, string> masterfundCollection = new Dictionary<int, string>();
            try
            {


                //Modified By: Bharat raturi, date: 05/03/2014
                //Purpose: Change the database connection style, use Microsoft Enterprise Library objects
                object[] parameter = { CompanyID };
                string sProc = "P_GetAllMasterFunds";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, parameter, _connectionString))
                {
                    while (dr.Read())
                    {
                        masterfundCollection.Add(dr.GetInt32(0), dr.GetString(1));
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
            return masterfundCollection;
        }


        /// <summary>
        /// Load all Accounts From Data base and add into _accountCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadAccountsFromDb(int CompanyID)
        {
            Dictionary<int, string> accountCollection = new Dictionary<int, string>();
            try
            {
                //accountCollection.Clear();

                //Modified By: Bharat raturi, date: 05/03/2014
                //Purpose: Change the database connection style, use Microsoft Enterprise Library objects
                object[] param = { CompanyID };
                string sProc = "P_GetAllCompanyFunds";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, _connectionString))
                {

                    while (dr.Read())
                    {
                        accountCollection.Add(dr.GetInt32(0), dr.GetString(1));
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
            return accountCollection;
        }

        /// <summary>
        /// Load all Mapping or releationship  For Account MasterFund and add into _accountMasterFundMapping dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadAccountMasterFundMappingFromDb(int CompanyID)
        {
            Dictionary<int, List<int>> accountMasterFundMapping = new Dictionary<int, List<int>>();
            try
            {
                //Modified By: Bharat raturi, date: 05/03/2014
                //Purpose: Change the database connection style, use Microsoft Enterprise Library objects

                //accountMasterFundMapping.Clear();
                object[] param = { CompanyID };
                string sProc = "P_CompanyMasterFundSubAccountAssociation";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, _connectionString))
                {
                    //using (SqlConnection _connection = new SqlConnection(_connectionString))
                    //{
                    //    SqlCommand cmd = new SqlCommand();
                    //    cmd.CommandText = "P_CompanyMasterFundSubAccountAssociation";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Connection = _connection;
                    //    _connection.Open();
                    //    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int companyMasterFundId = Convert.ToInt32(dr[0]);
                        int companyAccountId = Convert.ToInt32(dr[1]);
                        if (accountMasterFundMapping.ContainsKey(companyMasterFundId))
                            accountMasterFundMapping[companyMasterFundId].Add(companyAccountId);
                        else
                        {
                            List<int> accountCollection = new List<int>();
                            accountCollection.Add(companyAccountId);
                            accountMasterFundMapping.Add(companyMasterFundId, accountCollection);
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
            return accountMasterFundMapping;
        }

        /// <summary>
        /// Load all Mapping or releationship  For Account MasterFund and add into _accountMasterFundMapping dictionary
        /// </summary>
        internal static Dictionary<int, int> LoadMasterFundTradingAccountMappingFromDb(int CompanyID)
        {
            Dictionary<int, int> masterFundTradingAccountMapping = new Dictionary<int, int>();
            try
            {
                //Modified By: Bharat raturi, date: 05/03/2014
                //Purpose: Change the database connection style, use Microsoft Enterprise Library objects

                //accountMasterFundMapping.Clear();
                object[] param = { CompanyID };
                string sProc = "P_GetCompanyMasterFundTradingAccountMapping";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, _connectionString))
                {
                    //using (SqlConnection _connection = new SqlConnection(_connectionString))
                    //{
                    //    SqlCommand cmd = new SqlCommand();
                    //    cmd.CommandText = "P_CompanyMasterFundSubAccountAssociation";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Connection = _connection;
                    //    _connection.Open();
                    //    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int companyMasterFundId = Convert.ToInt32(dr[0]);
                        int companyTradingAccountId = Convert.ToInt32(dr[1]);
                        masterFundTradingAccountMapping[companyMasterFundId] = companyTradingAccountId;
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
            return masterFundTradingAccountMapping;
        }



        /// <summary>
        /// save xml document ot data base using stored procedure  name P_SaveFundMasterFundMapping
        /// </summary>
        /// <param name="ds">passed xml document as parameter </param>
        internal static int SaveDataSetInDb(String xmlDataTable, string xmlTradingAccount, string xmlMasterFund, int companyID, bool isDeletedForceFully)
        {
            int i = 0;
            try
            {
                //using (SqlConnection _connection = new SqlConnection(_connectionString))
                //{
                //    _connection.Open();
                //    SqlParameter[] parameters = new SqlParameter[4];
                //    parameters[0] = new SqlParameter("@XMLDoc", xmlDataTable);
                //    parameters[1] = new SqlParameter("@XMLMDoc", xmlMasterFund);
                //    parameters[2] = new SqlParameter("@ErrorMessage", "CouldNotSaveMapping");
                //    parameters[3] = new SqlParameter("@ErrorNumber", -1);
                //    SqlCommand command = new SqlCommand("P_SaveFundMasterFundMapping", _connection);
                //    command.CommandType = CommandType.StoredProcedure;
                //    foreach (SqlParameter param in parameters)
                //    {
                //        command.Parameters.Add(param);
                //    }
                //    //passing the string form of XML generated above
                //     i = command.ExecuteNonQuery();
                //}               

                //modified by: Bharat Raturi, march-2014
                //purpose: provide correct way to save data in the Database


                object[] parameter = { xmlDataTable, xmlTradingAccount, xmlMasterFund, "CouldNotSaveMapping", -1, companyID, isDeletedForceFully };

                //Modified by Faisal Shah 18/07/14
                object result = DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveFundMasterFundMapping", parameter, _connectionString);
                if (result != null)
                    i = result == null ? 0 : (int)result;
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

        /// <summary>
        /// Get the value (maximum MasterFund ID+1) from the database
        /// </summary>
        /// <returns>The account id that will be used for the new created master fund</returns>
        internal static int GetNewMasterFundID()
        {
            int masterFundID = 1;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMaxMasterFundID";

            try
            {
                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData, _connectionString);
                if (idValue != DBNull.Value)
                {
                    masterFundID = Convert.ToInt32(idValue);
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
            return masterFundID;
        }

        /// <summary>
        /// Determines whether [is master fund associated] [the specified company identifier].
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="masterFundId">The master fund identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is master fund associated] [the specified company identifier]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool isMasterFundAssociated(int companyId, int masterFundId)
        {
            object[] param = { companyId, masterFundId };
            string sProc = "P_AL_CheckMFinMasterFundPreferences";
            try
            {
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, _connectionString))
                {
                    if (dr.Read())
                        return true;
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
            return false;
        }

        /// <summary>
        /// Determines whether fund is associated with the any master fund preference.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="masterFundId">The master fund identifier.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is fund associated] [the specified company identifier]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool isFundAssociated(int companyId, int masterFundId, int accountId)
        {
            object[] param = { companyId, masterFundId, accountId };
            try
            {
                string sProc = "P_AL_CheckFundAssociatedInMasterFundPreferences";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param, _connectionString))
                {
                    if (dr.Read())
                        return true;
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
            return false;
        }
    }
}



