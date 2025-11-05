using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class AccountGroupSetupMappingDAL
    {

        /// <summary>
        /// Load all accounts from Data base and add into accountCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadAccountFromDb()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyFundsC";

            Dictionary<int, string> accountCollection = new Dictionary<int, string>();
            try
            {
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        accountCollection.Add(dr.GetInt32(0), dr.GetString(2));
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
        /// Load all accounts from Data base according to client and add into accountCollection dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadAccountByClientFromDb()
        {
            Dictionary<int, List<int>> accountClientCollection = new Dictionary<int, List<int>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyFundsC";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        int clientId = Convert.ToInt32(dr[3]);
                        int accountId = Convert.ToInt32(dr[0]);
                        if (accountClientCollection.ContainsKey(clientId))
                            accountClientCollection[clientId].Add(accountId);
                        else
                        {
                            List<int> clientCollection = new List<int>();
                            clientCollection.Add(accountId);
                            accountClientCollection.Add(clientId, clientCollection);
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
            return accountClientCollection;
        }

        /// <summary>
        /// Load all Mapping or releationship  for account & group and add into groupAccountMapping dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadAccountGroupMappingFromDb(int GroupID)
        {
            Dictionary<int, List<int>> groupAccountMapping = new Dictionary<int, List<int>>();
            try
            {
                object[] param = { GroupID };
                string sProc = "P_GroupFundAssociation";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int groupId = Convert.ToInt32(dr[0]);
                        int accountId = Convert.ToInt32(dr[1]);
                        if (groupAccountMapping.ContainsKey(groupId))
                            groupAccountMapping[groupId].Add(accountId);
                        else
                        {
                            List<int> groupCollection = new List<int>();
                            groupCollection.Add(accountId);
                            groupAccountMapping.Add(groupId, groupCollection);
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
            return groupAccountMapping;
        }

        /// <summary>
        /// save xml document ot data base using stored procedure  name P_SaveFundGroupMapping
        /// </summary>
        /// <param name="ds">passed xml document as parameter </param>
        internal static int SaveDataSetInDb(String xmlDataTable, int groupID, string groupName)
        {
            int i = 0;
            try
            {
                string sProc = "P_SaveFundGroupMapping";
                object[] parameter = { xmlDataTable,
                                       groupID,
                                       groupName,
                                       "CouldNotSaveMapping",
                                       -1 };

                i = DatabaseManager.DatabaseManager.ExecuteNonQuery(sProc, parameter);
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
        /// Load all clients from Data base and add into clientCollection dictionary
        /// </summary>
        internal static Dictionary<int, string> LoadClientsFromDb()
        {
            Dictionary<int, string> clientCollection = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllClientNames";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (dr.Read())
                    {
                        clientCollection.Add(dr.GetInt32(0), dr.GetString(1));
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
            return clientCollection;
        }

        /// <summary>
        /// Load all clients from Data base and add into clientAccountCollection dictionary
        /// </summary>
        internal static Dictionary<int, List<int>> LoadClientsForMappedAccountsFromDb(int groupID)
        {
            Dictionary<int, List<int>> clientAccountCollection = new Dictionary<int, List<int>>();
            try
            {
                object[] param = { groupID };
                string sProc = "P_GetAllClientsByMappedFunds";

                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        if (clientAccountCollection.ContainsKey(groupID))
                            clientAccountCollection[groupID].Add(dr.GetInt32(0));
                        else
                        {
                            List<int> clientCollection = new List<int>();
                            clientCollection.Add(dr.GetInt32(0));
                            clientAccountCollection.Add(groupID, clientCollection);
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
            return clientAccountCollection;
        }

        /// <summary>
        /// Returns max account group ID
        /// </summary>
        /// <returns></returns>
        internal static int GetMaxGroupID()
        {
            int groupID = -1;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMaxFundGroupID";

            try
            {
                object idValue = DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
                if (idValue != DBNull.Value)
                {
                    groupID = Convert.ToInt32(idValue);
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
            return groupID;
        }
    }
}
