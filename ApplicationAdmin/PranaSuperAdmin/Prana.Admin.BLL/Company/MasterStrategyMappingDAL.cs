//Created by:Bharat Raturi, Date: 02/13/2014
//Purpose: Data access Layer to show the Strategy to master strategy mapping
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    class MasterStrategyMappingDAL
    {
        /// <summary>
        /// Initialize Connection string for database 
        /// </summary>
        private const string pranaConnectionString = "PranaConnectionString";
        /// <summary>
        /// Load all master Strategies from database and add into masterStrategyCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadMasterStrategyFromDb(int companyID)
        {
            Dictionary<int, string> masterStrategyCollection = new Dictionary<int, string>();
            try
            {
                string sprocMStrategy = "P_GetAllMasterStrategies";
                object[] param = { companyID };
                using (IDataReader drMasterStrategy = DatabaseManager.DatabaseManager.ExecuteReader(sprocMStrategy, param, pranaConnectionString))
                {
                    while (drMasterStrategy.Read())
                    {
                        int strategyId = drMasterStrategy.GetInt32(0);
                        String strategy = drMasterStrategy.GetString(1);
                        if (!masterStrategyCollection.ContainsKey(strategyId))
                        {
                            masterStrategyCollection.Add(strategyId, strategy);
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
            return masterStrategyCollection;
        }

        /// <summary>
        /// Load all strategies from database and add into strategyCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadStrategyFromDb(int companyID)
        {
            Dictionary<int, string> strategyCollection = new Dictionary<int, string>();
            try
            {
                //strategyCollection.Clear();
                string sProcAllStrategy = "P_GetAllCompStrategies";
                object[] param = { companyID };
                using (IDataReader drStrategy = DatabaseManager.DatabaseManager.ExecuteReader(sProcAllStrategy, param, pranaConnectionString))
                {
                    while (drStrategy.Read())
                    {
                        strategyCollection.Add(drStrategy.GetInt32(0), drStrategy.GetString(1));
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
            return strategyCollection;
        }

        /// <summary>
        /// Load all Mapping or releationship  For strategy to master strategy and add into strategyMasterStrategyMapping dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadStrategyMasterStrategyMappingFromDb(int companyID)
        {
            Dictionary<int, List<int>> strategyMasterStrategyMapping = new Dictionary<int, List<int>>();
            try
            {
                strategyMasterStrategyMapping.Clear();
                string sProcMapping = "P_CompanyMasterStrategySubAccountAssociation";
                object[] param = { companyID };
                using (IDataReader drMapping = DatabaseManager.DatabaseManager.ExecuteReader(sProcMapping, param, pranaConnectionString))
                {
                    while (drMapping.Read())
                    {
                        int companyMasterStrategyId = Convert.ToInt32(drMapping[0]);
                        int companyStrategyId = Convert.ToInt32(drMapping[1]);
                        if (strategyMasterStrategyMapping.ContainsKey(companyMasterStrategyId))
                            strategyMasterStrategyMapping[companyMasterStrategyId].Add(companyStrategyId);
                        else
                        {
                            List<int> strategyCollection = new List<int>();
                            strategyCollection.Add(companyStrategyId);
                            strategyMasterStrategyMapping.Add(companyMasterStrategyId, strategyCollection);
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
            return strategyMasterStrategyMapping;
        }

        /// <summary>
        /// save xml document to data base using stored procedure named P_SaveStrategyMasterStrategyMapping
        /// </summary>
        /// <param name="ds">passed xml document as parameter </param>
        internal static int SaveDataSetInDB(String xmlDataTable, string xmlMasterStrategy, int companyID, bool isDeleteForceFully)
        {
            int i = 0;
            try
            {
                // Modified By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8504

                object[] parameter = { xmlDataTable, xmlMasterStrategy, "CouldNotSaveMapping", -1, companyID, isDeleteForceFully };
                //Modified by Faisal Shah 18/07/14
                object result = DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveStrategyMasterStrategyMapping", parameter, pranaConnectionString);
                if (result != null)
                    i = result == null ? 0 : (int)result;
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
            //object[] parameter = new object[5];

            //parameter[0] = xmlDataTable;
            //parameter[1] = xmlMasterStrategy;
            //parameter[2] = "CouldNotSaveMapping";
            //parameter[3] = 0;
            //parameter[4] = companyID;

            // Modified by Bhavana for CH Release 

            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            return i;
        }
        /// <summary>
        /// Get the value (maximum MasterStrategy ID+1) from the database
        /// </summary>
        /// <returns>The Strategy id that will be used for the new created master Strategy</returns>
        //Added By Faisal Shah on 20/06/14
        internal static int GetNewMasterStrategyID()
        {
            int masterStrategyID = 1;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMaxMasterStrategyID";

            try
            {
                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData, "PranaConnectionString");
                if (idValue != DBNull.Value)
                {
                    masterStrategyID = Convert.ToInt32(idValue);
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
            return masterStrategyID;
        }
    }
}