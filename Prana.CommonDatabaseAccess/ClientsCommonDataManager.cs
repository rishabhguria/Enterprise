using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.CommonDatabaseAccess
{
    public class ClientsCommonDataManager : IClientsCommonDataManager
    {
        static Dictionary<long, Prana.BusinessObjects.Currency> _currencyLookup = new Dictionary<long, Prana.BusinessObjects.Currency>();
        private static Dictionary<int, string> _assets;
        private static Dictionary<int, string> _underLyings;
        private static Dictionary<int, string> _exchanges;

        #region Assets

        public List<string> GetAccountsForTheUser(int userID)
        {

            List<string> accountsList = new List<string>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFundsForTheUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountsList.Add(row[0].ToString());
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
            return accountsList;
        }

        /// <summary>
        /// Added for Prana CLIENT 16 01 06
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Assets GetCompanyUserAssets(int userID)
        {
            Assets assets = new Assets();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAssetsByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assets.Add(FillCompanyAssets(row, 0));

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
            return assets;
        }
        /// <summary>
        /// FillCompanyAssets is a method to fill a object of <see cref="Asset"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Asset"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Asset"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Asset class. So, if the row only contains result from T_Asset table then value of Offset would be zero ("0"), and, lets say the row contains value of Asset as well as any other table then we have to specify the offset from where the values of Asset starts. Note: Sequence of Asset class whould be always same as in this method.</remarks>
        public Asset FillCompanyAssets(object[] row, int offSet)
        {
            //int companyID = 0 + offSet;
            int assetID = 1 + offSet;
            int assetName = 2 + offSet;
            //int comment = 3 + offSet;
            Asset asset = null;
            try
            {
                asset = new Asset(int.Parse(row[assetID].ToString()), row[assetName].ToString());
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
            return asset;
        }
        /// <summary>
        /// FillUnderLyings is a method to fill a object of <see cref="Underlying"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Asset"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="UnderLying"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of UnderLying class. So, if the row only contains result from T_UnderLying table then value of Offset would be zero ("0"), and, lets say the row contains value of UnderLying as well as any other table then we have to specify the offset from where the values of UnderLying starts. Note: Sequence of UnderLying class whould be always same as in this method.</remarks>
        ///
        #endregion

        #region Underlying
        public UnderLying FillUnderLyings(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int name = 1 + offSet;

            UnderLying underLying = new UnderLying();
            try
            {
                if (!(row[ID] is System.DBNull))
                {
                    underLying.UnderlyingID = int.Parse(row[ID].ToString());
                }
                if (!(row[name] is System.DBNull))
                {
                    underLying.Name = row[name].ToString();
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
            return underLying;
        }

        public UnderLyings GetUnderLyingsByUserID(int userID)
        {
            UnderLyings underLyings = new UnderLyings();
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }
        public UnderLyings GetUnderLyingsByAssetAndUserID(int userID, int assetID)
        {
            UnderLyings underLyings = new UnderLyings();

            Object[] parameter = new object[2];
            try
            {
                parameter[0] = userID;
                parameter[1] = assetID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUnderLyingsByAssetAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        underLyings.Add(FillUnderLyings(row, 0));
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
            return underLyings;
        }
        #endregion

        #region AUECs
        public AUEC FillCompanyUserAUECDetails(object[] row, int offSet)
        {
            int AUECID = 0 + offSet;
            int ASSETID = 1 + offSet;
            int UNDERLYINGID = 2 + offSet;
            int EXCHANGEID = 3 + offSet;
            int DISPLAYNAME = 5 + offSet;

            AUEC companyUserAUEC = new AUEC();
            try
            {

                if (!(row[ASSETID] is System.DBNull))
                {
                    companyUserAUEC.Asset.AssetID = int.Parse(row[ASSETID].ToString());
                    companyUserAUEC.Asset.Name = GetAssetText(companyUserAUEC.Asset.AssetID);
                }
                if (!(row[UNDERLYINGID] is System.DBNull))
                {
                    companyUserAUEC.UnderLying.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                    companyUserAUEC.UnderLying.Name = GetUnderLyingText(companyUserAUEC.UnderLying.UnderlyingID);
                }
                if (!(row[EXCHANGEID] is System.DBNull))
                {
                    companyUserAUEC.Exchange.ExchangeID = int.Parse(row[EXCHANGEID].ToString());
                    companyUserAUEC.Exchange.Name = GetExchangeText(companyUserAUEC.Exchange.ExchangeID);
                }

                if (!(row[AUECID] is System.DBNull))
                {
                    companyUserAUEC.AUECID = int.Parse(row[AUECID].ToString());
                }

                if (!(row[DISPLAYNAME] is System.DBNull))
                {
                    companyUserAUEC.Name = row[DISPLAYNAME].ToString();
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

        private static string GetExchangeText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = _exchanges;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        private static string GetAssetText(int assetID)
        {
            try
            {
                Dictionary<int, string> dt = _assets;
                if (dt.ContainsKey(assetID))
                {
                    return dt[assetID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        private static string GetUnderLyingText(int ID)
        {
            try
            {
                Dictionary<int, string> dt = _underLyings;
                if (dt.ContainsKey(ID))
                {
                    return dt[ID];
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public AUECs GetCompanyUserAUECDetails(int companyUserID)
        {
            AUECs companyUserAUECs = new AUECs();
            DataSet keyValuePairs = DataManagerInternalRepository.GetKeyValuePairs();
            _assets = DataManagerInternalRepository.FillKeyValuePairs(keyValuePairs.Tables[0], 0);
            _underLyings = DataManagerInternalRepository.FillKeyValuePairs(keyValuePairs.Tables[4], 0);
            _exchanges = DataManagerInternalRepository.FillKeyValuePairs(keyValuePairs.Tables[5], 0);

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
        #endregion

        #region CompanyClientAccounts
        private static ClientAccount FillClientAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ClientAccount clientAccount = null;

            try
            {
                if (row != null)
                {
                    clientAccount = new ClientAccount();

                    int COMPANYCLIENTFUNDID = offset + 0;
                    int COMPANYCLIENTFUNDNAME = offset + 1;
                    int COMPANYCLIENTFUNDSHORTNAME = offset + 2;
                    int COMPANYCLIENTID = offset + 3;

                    clientAccount.CompanyClientAccountID = Convert.ToInt32(row[COMPANYCLIENTFUNDID]);
                    clientAccount.CompanyClientAccountName = Convert.ToString(row[COMPANYCLIENTFUNDNAME]);
                    clientAccount.CompanyClientAccountShortName = Convert.ToString(row[COMPANYCLIENTFUNDSHORTNAME]);
                    clientAccount.CompanyClientID = Convert.ToInt32(row[COMPANYCLIENTID]);

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
            return clientAccount;
        }
        public ClientAccounts GetCompanyClientAccounts(int companyClientID)
        {
            ClientAccounts clientAccounts = new ClientAccounts();

            Object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clientAccounts.Add(FillClientAccount(row, 0));
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
            return clientAccounts;
        }
        #endregion

        #region ClearingFirmsPrimeBrokers
        public ClearingFirmsPrimeBrokers GetClearingFirmsPrimeBrokers()
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
        public ClearingFirmPrimeBroker FillClearingFirmPrimeBroker(object[] row, int offSet)
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
        #endregion

        #region Company Clients
        /// <summary>
        /// Gets all <see cref="Clients"/> from database.
        /// </summary>
        /// <returns><see cref="Clients"/> fetched.</returns>

        public ClientCollection GetClients(int companyID)
        {
            ClientCollection clients = new ClientCollection();

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
        private static Client FillClient(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int clientName = 1 + offSet;

            Client client = new Client(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
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
        #endregion

        #region CounterPartyCollection
        public CounterParty FillCounterPartiesForTradingTicket(object[] row, int offSet)
        {

            int counterPartyID = 0 + offSet;

            //int fullName = 1 + offSet;
            int shortName = 2 + offSet;

            int isAlgoBroker = 3 + offSet;

            int isOTDorEMS = 4 + offSet;

            CounterParty counterParty = new CounterParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            try
            {

                if (row[counterPartyID] != null)
                {
                    counterParty.CounterPartyID = int.Parse(row[counterPartyID].ToString());
                }

                if (row[shortName] != null)
                {
                    counterParty.Name = row[shortName].ToString();
                }

                if (row.Length > isAlgoBroker && row[isAlgoBroker] != null && row[isAlgoBroker].ToString() != "")
                {
                    counterParty.IsAlgoBroker = bool.Parse(row[isAlgoBroker].ToString());
                }

                if (row.Length > isOTDorEMS && row[isOTDorEMS] != null && row[isOTDorEMS].ToString() != "")
                {
                    counterParty.IsOTDorEMS = bool.Parse(row[isOTDorEMS].ToString());
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
            return counterParty;
        }
        public CounterPartyCollection GetCounterPartiesByAUIDAndUserID(int userID, int AssetID, int UnderlyingID)
        {
            CounterPartyCollection companyCounterParties = new CounterPartyCollection();

            object[] parameter = new object[3];
            parameter[0] = userID;
            parameter[1] = AssetID;
            parameter[2] = UnderlyingID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartiesByAUIDAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCounterPartiesForTradingTicket(row, 0));

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
            return companyCounterParties;
        }
        public CounterPartyCollection GetCompanyUserCounterParties(int userID)
        {
            CounterPartyCollection companyCounterParties = new CounterPartyCollection();

            object[] parameter = new object[1];

            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartiesByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCounterPartiesForTradingTicket(row, 0));

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
            return companyCounterParties;
        }
        public CounterPartyCollection GetCompanyCounterParties(int companyID)
        {
            CounterPartyCollection companyCounterParties = new CounterPartyCollection();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterParties", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCompanyCounterParties(row, 0));

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
            return companyCounterParties;
        }
        public CounterParty FillCompanyCounterParties(object[] row, int offSet)
        {
            //int companyID = 0 + offSet;
            int counterPartyID = 1 + offSet;

            //int fullName = 2 + offSet;
            int shortName = 3 + offSet;
            //int address = 4 + offSet;
            //int phone = 5 + offSet;
            //int fax = 6 + offSet;
            //int contactname1 = 7 + offSet;
            //int title1 = 8 + offSet;
            //int email1 = 9 + offSet;
            //int contactname2 = 10 + offSet;
            //int title2 = 11 + offSet;
            //int email2 = 12 + offSet;
            //int counterpartytypeid = 13 + offSet;


            CounterParty counterParty = new CounterParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            try
            {

                if (row[counterPartyID] != null)
                {
                    counterParty.CounterPartyID = int.Parse(row[counterPartyID].ToString());
                }

                if (row[shortName] != null)
                {
                    counterParty.Name = row[shortName].ToString();
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
            return counterParty;
        }
        #endregion

        #region Company EMS Sources

        public ImportTradeXSLTFileCollection GetCompanyEMSSource(int companyID, string path)
        {
            ImportTradeXSLTFileCollection emsSources = new ImportTradeXSLTFileCollection();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyEMSSourcesAndXSLT", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        emsSources.Add(FillandSaveEMSImportSources(row, 0, path));
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

            return emsSources;
            #endregion


        }

        private static ImportTradeXSLTFile FillandSaveEMSImportSources(object[] row, int offset, string path)
        {
            int EMSSourceID = offset + 0;
            int ImportSourceName = offset + 1;
            int XSLTFileID = offset + 2;
            int FileNames = offset + 3;
            //int FileData = offset + 4;

            ImportTradeXSLTFile emsSource = new ImportTradeXSLTFile();

            if (row[EMSSourceID] != System.DBNull.Value)
            {
                emsSource.ImportSourceID = Convert.ToInt32(row[EMSSourceID]);
            }
            if (row[ImportSourceName] != System.DBNull.Value)
            {
                emsSource.EMSSource = Convert.ToString(row[ImportSourceName]);
            }
            if (row[XSLTFileID] != System.DBNull.Value)
            {
                emsSource.FileID = Convert.ToInt32(row[XSLTFileID]);
            }
            if (row[FileNames] != System.DBNull.Value)
            {
                emsSource.FileName = Convert.ToString(row[FileNames]);
            }

            FileAndDbSyncManager.SyncFileWithDataBase(path, ApplicationConstants.MappingFileType.EMSImportXSLT);
            FileAndDbSyncManager.SyncFileWithDataBase(path, ApplicationConstants.MappingFileType.ReconMappingXml);

            #region Commented

            //string directoryPath = "XSLT\\EMSXSLT";
            //string fullPath = path + "\\" + directoryPath;
            //if(!Directory.Exists(fullPath))
            //{
            //    Directory.CreateDirectory(fullPath);
            //}
            //string fileName = emsSource.FileName;
            //byte[] data = (byte[])row[FileData];
            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(fullPath + "\\" + fileName, FileMode.Create);
            //    fs.Write(data, 0, data.Length);
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            //finally
            //{
            //    if (fs != null)
            //        fs.Close();
            //}

            #endregion

            return emsSource;
        }

        #endregion

        #region Exchange Methods
        public Exchange FillExchangesForTradingTicket(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int FULL_NAME = 1 + offSet;
            int DISPLAY_NAME = 2 + offSet;

            Exchange exchange = new Exchange();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    exchange.ExchangeID = int.Parse(row[ID].ToString());
                }
                if (row[FULL_NAME] != System.DBNull.Value)
                {
                    exchange.Name = row[FULL_NAME].ToString();
                }
                if (row[DISPLAY_NAME] != System.DBNull.Value)
                {
                    exchange.Name = row[DISPLAY_NAME].ToString();
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
            return exchange;
        }
        public Exchanges GetExchnagesByUserID(int userID)
        {
            Exchanges exchanges = new Exchanges();

            Object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExchnagesByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchanges.Add(FillExchangesForTradingTicket(row, 0));
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
            return exchanges;
        }
        #endregion

        #region OrderSides
        private static Side FillSide(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            Side side = new Side();

            try
            {
                if (row != null)
                {
                    int SIDEID = offset + 0;
                    int SIDE = offset + 1;
                    int SIDETAGVALUE = offset + 2;

                    side.SideID = Convert.ToInt32(row[SIDEID]);
                    if (DBNull.Value != row[SIDE])
                    {

                        side.Name = Convert.ToString(row[SIDE]);
                    }
                    if (DBNull.Value != row[SIDETAGVALUE])
                    {

                        side.TagValue = Convert.ToString(row[SIDETAGVALUE]);
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
            return side;
        }
        public Sides GetSides()
        {
            Sides sides = new Sides();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSides";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        sides.Add(FillSide(row, 0));
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
            return sides;
        }
        public Sides GetOrderSidesByCVAUEC(int assetID, int UnderLyingID, int counterPartyID, int venueID)
        {
            //TODO: Write SP to get OrderSides for a user according to his permission.
            Sides orderSides = new Sides();

            try
            {
                object[] parameter = new object[4];
                parameter[0] = assetID;
                parameter[1] = UnderLyingID;
                parameter[2] = counterPartyID;
                parameter[3] = venueID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderSidesByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderSides.Add(FillSide(row, 0));
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
            return orderSides;
        }

        #endregion

        #region OrderTypes
        private static OrderType FillOrderType(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderType orderType = new OrderType();
            try
            {
                if (row != null)
                {
                    int ORDERTYPESID = offset + 0;
                    int ORDERTYPES = offset + 1;
                    int ORDERTYPESTAGVALUE = offset + 2;
                    orderType.OrderTypesID = Convert.ToInt32(row[ORDERTYPESID]);
                    if (DBNull.Value != row[ORDERTYPES])
                    {

                        orderType.Type = Convert.ToString(row[ORDERTYPES]);
                    }
                    orderType.TagValue = Convert.ToString(row[ORDERTYPESTAGVALUE]);
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
            return orderType;
        }
        public OrderTypes GetOrderTypes()
        {
            OrderTypes orderTypes = new OrderTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllOrderTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillOrderType(row, 0));
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
            return orderTypes;
        }
        public OrderTypes GetOrderTypesByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            OrderTypes orderTypes = new OrderTypes();
            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderTypesByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillOrderType(row, 0));
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
            return orderTypes;
        }
        #endregion

        #region TimeInForce

        private static TimeInForce FillTimeInForce(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            TimeInForce timeInForce = new TimeInForce();
            try
            {
                if (row != null)
                {
                    int TIMEINFORCEID = offset + 0;
                    int TIMEINFORCE = offset + 1;
                    int TIMEINFORCETAGVALUE = offset + 2;



                    timeInForce.TimeInForceID = Convert.ToInt32(row[TIMEINFORCEID]);
                    if (DBNull.Value != row[TIMEINFORCE])
                    {

                        timeInForce.Name = Convert.ToString(row[TIMEINFORCE]);
                    }
                    if (DBNull.Value != row[TIMEINFORCETAGVALUE])
                    {

                        timeInForce.TagValue = Convert.ToString(row[TIMEINFORCETAGVALUE]);
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
            return timeInForce;
        }
        public TimeInForces GetTimeInForces()
        {
            TimeInForces timeInForces = new TimeInForces();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllTimeInForce";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForces.Add(FillTimeInForce(row, 0));
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
            return timeInForces;
        }
        public TimeInForces GetTIFByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            TimeInForces timeInForces = new TimeInForces();

            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTIFByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForces.Add(FillTimeInForce(row, 0));
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
            return timeInForces;
        }
        #endregion

        #region HandlingInst
        private static HandlingInstruction FillHandlingInstruction(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            HandlingInstruction handlingInstruction = new HandlingInstruction();
            try
            {
                if (row != null)
                {
                    int HANDLINGINSTRUCTIONSID = offset + 0;
                    int HANDLINGINSTRUCTIONS = offset + 1;
                    int HANDLINGINSTRUCTIONSTAGVALUE = offset + 2;



                    handlingInstruction.HandlingInstructionID = Convert.ToInt32(row[HANDLINGINSTRUCTIONSID]);
                    if (DBNull.Value != row[HANDLINGINSTRUCTIONS])
                    {

                        handlingInstruction.Name = Convert.ToString(row[HANDLINGINSTRUCTIONS]);
                    }
                    if (DBNull.Value != row[HANDLINGINSTRUCTIONSTAGVALUE])
                    {

                        handlingInstruction.TagValue = Convert.ToString(row[HANDLINGINSTRUCTIONSTAGVALUE]);
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
            return handlingInstruction;


        }
        public HandlingInstructions GetHandlingInstructions()
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllHandlingInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstructions.Add(FillHandlingInstruction(row, 0));
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
            return handlingInstructions;
        }
        public HandlingInstructions GetHandlingInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();
            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetHandlingInstructionByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstructions.Add(FillHandlingInstruction(row, 0));
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
            return handlingInstructions;
        }
        #endregion

        #region ExecInst
        private static ExecutionInstruction FillExecutionInstruction(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ExecutionInstruction executionInstruction = new ExecutionInstruction();
            try
            {
                if (row != null)
                {
                    int T_EXECUTIONINSTRUCTIONSID = offset + 0;
                    int EXECUTIONINSTRUCTIONS = offset + 1;
                    int EXECUTIONINSTRUCTIONSTAGVALUE = offset + 2;



                    executionInstruction.ExecutionInstructionsID = Convert.ToInt32(row[T_EXECUTIONINSTRUCTIONSID]);
                    if (DBNull.Value != row[EXECUTIONINSTRUCTIONS])
                    {

                        executionInstruction.ExecutionInstructions = Convert.ToString(row[EXECUTIONINSTRUCTIONS]);
                    }
                    executionInstruction.TagValue = Convert.ToString(row[EXECUTIONINSTRUCTIONSTAGVALUE]);


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
            return executionInstruction;
        }
        public ExecutionInstructions GetExecutionInstructions()
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllExecutionInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstructions.Add(FillExecutionInstruction(row, 0));
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
            return executionInstructions;
        }
        public ExecutionInstructions GetExecutionInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();

            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExecutionInstructionByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstructions.Add(FillExecutionInstruction(row, 0));
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
            return executionInstructions;
        }
        #endregion

        #region Trader

        private static Trader FillTrader(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            Trader trader = null;

            try
            {
                if (row != null)
                {
                    trader = new Trader();

                    int TRADERID = offset + 0;
                    int FIRSTNAME = offset + 1;
                    int LASTNAME = offset + 2;
                    int SHORTNAME = offset + 3;
                    int TITLE = offset + 4;
                    int EMAIL = offset + 5;
                    int TELEPHONEWORK = offset + 6;
                    int TELEPHONECELL = offset + 7;
                    int PAGER = offset + 8;
                    int TELEPHONEHOME = offset + 9;
                    int FAX = offset + 10;
                    int COMPANYID = offset + 11;

                    trader.TraderID = Convert.ToInt32(row[TRADERID]);
                    trader.FirstName = Convert.ToString(row[FIRSTNAME]);
                    if (DBNull.Value != row[LASTNAME])
                    {
                        trader.LastName = Convert.ToString(row[LASTNAME]);
                    }

                    if (DBNull.Value != row[SHORTNAME])
                    {
                        trader.ShortName = Convert.ToString(row[SHORTNAME]);
                    }

                    if (DBNull.Value != row[TITLE])
                    {
                        trader.Title = Convert.ToString(row[TITLE]);
                    }

                    trader.EMail = Convert.ToString(row[EMAIL]);
                    trader.TelephoneWork = Convert.ToString(row[TELEPHONEWORK]);
                    if (DBNull.Value != row[TELEPHONECELL])
                    {
                        trader.TelephoneCell = Convert.ToString(row[TELEPHONECELL]);
                    }

                    if (DBNull.Value != row[PAGER])
                    {
                        trader.Pager = Convert.ToString(row[PAGER]);
                    }

                    if (DBNull.Value != row[TELEPHONEHOME])
                    {
                        trader.TelephoneHome = Convert.ToString(row[TELEPHONEHOME]);
                    }

                    if (DBNull.Value != row[FAX])
                    {
                        trader.Fax = Convert.ToString(row[FAX]);
                    }
                    trader.CompanyID = Convert.ToInt32(row[COMPANYID]);
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
            return trader;
        }
        public Traders GetTraders(int companyClientID)
        {
            Traders traders = new Traders();

            object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientTraders", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        traders.Add(FillTrader(row, 0));
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
            return traders;
        }

        #endregion

        #region Venues

        /// <summary>
        /// Fills the row of Venue to <see cref="Venue"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Venue"/></returns>
        private static Prana.BusinessObjects.Venue FillVenues(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.Venue venue = null;
            try
            {
                if (row != null)
                {
                    venue = new Prana.BusinessObjects.Venue();
                    int VENUE_ID = offset + 0;
                    int VENUE_NAME = offset + 1;
                    //int VENUETYPEID = offset + 2;
                    //int ROUTE = offset + 3;

                    venue.VenueID = Convert.ToInt32(row[VENUE_ID]);
                    venue.Name = Convert.ToString(row[VENUE_NAME]);
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
            return venue;
        }

        public Prana.BusinessObjects.VenueCollection GetVenues(int userID, int counterPartyID)
        {
            //TODO: Write SP to get CounterParty for a user according to his permission.
            Prana.BusinessObjects.VenueCollection venues = new Prana.BusinessObjects.VenueCollection();

            try
            {
                object[] parameter = new object[2];
                parameter[0] = userID;
                parameter[1] = counterPartyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUserVenues", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }

        public Prana.BusinessObjects.VenueCollection GetVenuesByAUIDCounterPartyAndUserID(int userID, int counterPartyID, int assetID, int underlyingID)
        {
            //TODO: Write SP to get CounterParty for a user according to his permission.
            Prana.BusinessObjects.VenueCollection venues = new Prana.BusinessObjects.VenueCollection();

            try
            {
                object[] parameter = new object[4];
                parameter[0] = userID;
                parameter[1] = counterPartyID;
                parameter[2] = assetID;
                parameter[3] = underlyingID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVenuesByAUIDCounterPartyAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }
        #endregion

        #region TradingAccounts

        /// <summary>
        /// Fills the row of Venue to <see cref="Venue"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Venue"/></returns>
        private static Prana.BusinessObjects.TradingAccount FillTradingAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.TradingAccount tradingAccount = null;
            try
            {
                if (row != null)
                {
                    tradingAccount = new Prana.BusinessObjects.TradingAccount();
                    int TRADINGACCOUNT_ID = offset + 0;
                    int TRADINGACCOUNT_NAME = offset + 1;

                    tradingAccount.TradingAccountID = Convert.ToInt32(row[TRADINGACCOUNT_ID]);
                    tradingAccount.Name = Convert.ToString(row[TRADINGACCOUNT_NAME]);
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

        public List<string> GetAllTradingAccounts()
        {
            List<string> tradingAccountIDs = new List<string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllTradingAccounts";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccountIDs.Add(row[0].ToString());
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
            return tradingAccountIDs;
            #endregion
        }

        public Prana.BusinessObjects.TradingAccountCollection GetTradingAccounts(int userID)
        {
            Prana.BusinessObjects.TradingAccountCollection tradingAccounts = new Prana.BusinessObjects.TradingAccountCollection();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUserTradingAccounts", parameter))
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

        public DataTable GetSidesByAsset(int assetID)
        {

            object[] parameter = new object[1];
            parameter[0] = assetID;

            try
            {
                DataTable keyWiseTable = new DataTable();
                keyWiseTable.Columns.Add(OrderFields.PROPERTY_ORDER_SIDEID);
                keyWiseTable.Columns.Add(OrderFields.PROPERTY_ORDER_SIDE);
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSidesByAsset", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        keyWiseTable.Rows.Add(new object[] { row[0].ToString(), row[1].ToString() });
                    }
                }
                return keyWiseTable;
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
            return new DataTable();
        }


        #endregion

        #region Strategies

        /// <summary>
        /// Fills the row of Strategy to <see cref="Strategy"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Strategy"/></returns>
        private static Prana.BusinessObjects.Strategy FillStrategy(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.Strategy strategy = null;
            try
            {
                if (row != null)
                {
                    strategy = new Prana.BusinessObjects.Strategy();
                    int STRATEGY_ID = offset + 0;
                    int STRATEGY_NAME = offset + 1;
                    int STRATEGY_FULL_NAME = offset + 2;

                    strategy.StrategyID = Convert.ToInt32(row[STRATEGY_ID]);
                    strategy.Name = Convert.ToString(row[STRATEGY_NAME]);
                    strategy.FullName = Convert.ToString(row[STRATEGY_FULL_NAME]);
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

        public Prana.BusinessObjects.StrategyCollection GetStrategies(int userID)
        {

            Prana.BusinessObjects.StrategyCollection strategies = new Prana.BusinessObjects.StrategyCollection();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                DataSet dataSet = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetStrategiesByUserID", parameter);
                if (dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        strategies.Add(FillStrategy(row.ItemArray, 0));
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
        public Prana.BusinessObjects.StrategyCollection GetStrategies()
        {

            Prana.BusinessObjects.StrategyCollection strategies = new Prana.BusinessObjects.StrategyCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyStrategiesC";

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

        #endregion

        #region Accounts

        /// <summary>
        /// Fills the row of Account to <see cref="Account"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Account"/></returns>
        private static Prana.BusinessObjects.Account FillAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.Account account = null;
            try
            {
                if (row != null)
                {
                    account = new Prana.BusinessObjects.Account();
                    int FUND_ID = offset + 0;
                    int FUND_NAME = offset + 1;
                    int FUND_FULL_NAME = offset + 2;

                    account.AccountID = Convert.ToInt32(row[FUND_ID]);
                    account.Name = Convert.ToString(row[FUND_NAME]);
                    ///Added - rajat 26 nov 2007
                    account.FullName = Convert.ToString(row[FUND_FULL_NAME]);
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
            return account;
        }
        public Prana.BusinessObjects.AccountCollection GetAccounts(int userID)
        {
            //Here we are getting the FUnds corresponding to the User that logs into Prana Client machine
            Prana.BusinessObjects.AccountCollection accounts = new Prana.BusinessObjects.AccountCollection();

            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                DataSet dataSet = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetFundsByUserID", parameter);
                if (dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        accounts.Add(FillAccount(row.ItemArray, 0));
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
            return accounts;
        }

        //KashishG.
        //PRANA-12486
        private static AccountCollection ConvertDatatabletoAccountCollection(DataTable dt)
        {
            Prana.BusinessObjects.Account account = null;
            Prana.BusinessObjects.AccountCollection accounts = new Prana.BusinessObjects.AccountCollection();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row != null && row.Table.Columns.Contains("FundID") && row.Table.Columns.Contains("FundShortName") && row.Table.Columns.Contains("FundName"))
                    {
                        account = new Prana.BusinessObjects.Account();
                        account.AccountID = Convert.ToInt32(row["FundID"]);
                        account.Name = Convert.ToString(row["FundShortName"]);
                        account.FullName = Convert.ToString(row["FundName"]);
                    }
                    accounts.Add(account);
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
            return accounts;
        }

        /// <summary>
        /// returns dictionary of all accounts with companyID and ThirdpartyID as per user with access to permitted account groups.
        /// for CH release
        /// Added By Faisal Shah from DatabaseManager.cs on 06/25/2014
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPermittedAccounts(int UserID)
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("FundID");
            dtAccounts.Columns.Add("FundName");
            dtAccounts.Columns.Add("CompanyID");
            dtAccounts.Columns.Add("ThirdPartyID");
            dtAccounts.Columns.Add("FundShortName");

            try
            {
                Object[] parameter = new object[1];
                parameter[0] = UserID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_CompanyUserFunds", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        dtAccounts.Rows.Add(new Object[5] { row[0], row[1].ToString(), row[2], row[3], row[4].ToString() });
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
            return dtAccounts;
        }

        /// <summary>
        /// returns dictionary of all Acronym with ImportTagName
        /// for CH release
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllImportTags()
        {
            DataTable dtImportTags = new DataTable();
            dtImportTags.Columns.Add("Acronym");
            dtImportTags.Columns.Add("ImportTagName");

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetImportTag";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dtImportTags.Rows.Add(new Object[2] { row[0].ToString(), row[1].ToString() });
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
            return dtImportTags;
        }

        public Prana.BusinessObjects.AccountCollection GetAccounts()
        {
            Prana.BusinessObjects.AccountCollection accounts = new Prana.BusinessObjects.AccountCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyFundsC";

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
            return accounts;
        }

        /// <summary>
        /// Atul Dislay (18-06-2015)
        /// Gets the master funds.
        /// </summary>
        /// <returns></returns>
        public Prana.BusinessObjects.AccountCollection GetMasterFunds()
        {
            Prana.BusinessObjects.AccountCollection masterFunds = new Prana.BusinessObjects.AccountCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyMasterFundsC";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        masterFunds.Add(FillAccount(row, 0));
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
            return masterFunds;
        }
        #endregion

        #region activity relationship
        private const string TABLE_ACTIVITYTYPE = "ActivityType";
        private const string TABLE_ACTIVITYAMOUNTTYPE = "AmountType";
        private const string TABLE_ACTIVITYJOURNALMAPPING = "ActivityJournalMapping";
        private const string TABLE_SUBACCOUNTS = "SubAccounts";
        private const string TABLE_ACTIVITYDATETYPE = "ActivityDateType";
        private const string TABLE_LASTREVALCALCDATE = "LastRevaluationCalcDate";
        private const string RELATION_ACTIVITYJOURNAL = "ActivityType";
        private const string RELATION_ACTIVITYAMOUNT = "AmountType";
        private const string RELATION_ACTIVITYSUBACCOUNTCR = "CreditAccount";
        private const string RELATION_ACTIVITYSUBACCOUNTDR = "DebitAccount";
        private const string TABLE_SUBACCOUNTTYPE = "SubAccountType";
        private const string TABLE_TRANSACTIONSOURCE = "TransactionSource";
        #endregion

        public DataSet GetLatestAvailableFxRatesLessThanToday()
        {
            DataSet ds = new DataSet();
            try
            {
                string[] tables = { "AvailableCurrencyRates" };
                DatabaseManager.DatabaseManager.LoadDataSet("P_getLatestAvailablefxRatesLessThanToday", ds, tables, new object[0]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return ds;
        }


        //Added by: Bharat raturi, 123 apr 2014
        //purpose: to save the mapping and nav lock preferences in the db
        /// <summary>
        /// Save the mapping and nav lock preferences in db
        /// </summary>
        /// <param name="isMasterFundManyToMany">true if many to many mapping enabled</param>
        /// <param name="isMasterStrategyManyToMany">true if many to many mapping enabled</param>
        /// <param name="isnavLockEnabled">true if nav locking enabled</param>
        /// <returns></returns>
        public int SavePranaPreferencesinDB(bool? isMasterFundManyToMany, bool? isMasterStrategyManyToMany, bool? isnavLockEnabled, bool? isFeederAccountEnabled, int? pricingSource, bool? isPermanentDeletion, int? SettlementAutoCalculateProperty, bool? isZeroCommissionForSwaps, int? avgPriceRounding, bool? isShowmasterFundAsClient, bool? isShowMasterFundonTT, bool? isEquityOptionManualValidation, bool? isCollateralMarkPriceValidation, bool? isShowTillSettlementDate) //DataTable dt)
        {
            int i = 0;
            try
            {
                DataSet ds = new DataSet("dsPranaPref");

                DataTable dt = new DataTable("dtPranaPref");
                dt.Columns.Add("PreferenceKey", typeof(string));
                dt.Columns.Add("PreferenceValue", typeof(int));
                if (isMasterFundManyToMany != null)
                    dt.Rows.Add("IsFundManyToManyMappingAllowed", Convert.ToInt32(isMasterFundManyToMany));
                if (isMasterStrategyManyToMany != null)
                    dt.Rows.Add("IsStrategyManyToManyMappingAllowed", Convert.ToInt32(isMasterStrategyManyToMany));
                if (isnavLockEnabled != null)
                    dt.Rows.Add("IsNAVLockingEnabled", Convert.ToInt32(isnavLockEnabled));
                if (isFeederAccountEnabled != null)
                    dt.Rows.Add("IsFeederFundEnabled", Convert.ToInt32(isFeederAccountEnabled));
                if (pricingSource != null)
                    dt.Rows.Add("PricingSource", pricingSource);
                if (isPermanentDeletion != null)
                    dt.Rows.Add("IsPermanentDeletion", Convert.ToInt32(isPermanentDeletion));
                if (SettlementAutoCalculateProperty != null)
                    dt.Rows.Add(ApplicationConstants.CONST_SettlementAutoCalculateField, Convert.ToInt32(SettlementAutoCalculateProperty));
                if (isZeroCommissionForSwaps != null)
                    dt.Rows.Add(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS, Convert.ToInt32(isZeroCommissionForSwaps));
                if (avgPriceRounding != null)
                    dt.Rows.Add(ApplicationConstants.CONST_AVGPRICEROUNDING, Convert.ToInt32(avgPriceRounding));
                if (isShowMasterFundonTT != null)
                    dt.Rows.Add("IsShowMasterFundonTT", Convert.ToInt32(isShowMasterFundonTT));
                if (isShowmasterFundAsClient != null)
                    dt.Rows.Add("IsShowmasterFundAsClient", Convert.ToInt32(isShowmasterFundAsClient));
                if (isEquityOptionManualValidation != null)
                    dt.Rows.Add("IsEquityOptionManualValidation", Convert.ToInt32(isEquityOptionManualValidation));
                if (isCollateralMarkPriceValidation != null)
                    dt.Rows.Add("IsCollateralMarkPriceValidation", Convert.ToInt32(isCollateralMarkPriceValidation));
                if (isShowTillSettlementDate != null)
                    dt.Rows.Add("IsPriceEnterTillSettlementDateInDailyValuation", Convert.ToInt32(isShowTillSettlementDate));
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

        public Dictionary<int, string> GetAllMasterFunds()
        {
            Dictionary<int, string> masterFunds = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCompanyFundMasterFundRelationShip";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //master fund ID coming at third position
                        if (!masterFunds.ContainsKey(int.Parse(row[2].ToString().Trim())))
                        {
                            masterFunds.Add(int.Parse(row[2].ToString()), row[3].ToString());
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
            return masterFunds;
        }

        public Dictionary<int, int> GetAllMasterFundsTradingAccounts(int companyUserID)
        {
            Dictionary<int, int> masterFundsTradingAccounts = new Dictionary<int, int>();
            object[] parameter = new object[1];
            parameter[0] = companyUserID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyMasterFundTradingAccountMapping", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        masterFundsTradingAccounts.Add(int.Parse(row[0].ToString()), int.Parse(row[1].ToString()));
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
            return masterFundsTradingAccounts;
        }

        public List<int> GetMasterFundsAndAccount(List<int> accounts)
        {
            Dictionary<int, List<int>> masterFunds = new Dictionary<int, List<int>>();

            List<int> masterFundsList = new List<int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMasterFundAndFund";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int key = Convert.ToInt16(row[0].ToString());
                        int accountID = Convert.ToInt16(row[1].ToString());
                        if (masterFunds.ContainsKey(key))
                        {
                            masterFunds[key].Add(accountID);
                        }
                        else
                        {
                            List<int> accountList = new List<int>();
                            accountList.Add(accountID);
                            masterFunds.Add(key, accountList);
                        }
                    }
                }

                foreach (int accountID in accounts)
                {
                    foreach (KeyValuePair<int, List<int>> kvp in masterFunds)
                    {
                        if (kvp.Value.Contains(accountID))
                        {
                            if (!masterFundsList.Contains(kvp.Key))
                            {
                                masterFundsList.Add(kvp.Key);
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
            return masterFundsList;
        }

        #region Company Borrowers
        public DataTable GetCompanyBorrowers(int userID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BorrowerFirmID");
            dt.Columns.Add("BorrowerShortName");
            object[] parameter = new object[1];
            parameter[0] = userID;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetBorrowersDetails";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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
            return dt;
        }
        #endregion

        #region Get Prana.BusinessObjects.Currency

        public int GetCompanyBaseCurrency(int companyID)
        {
            int result = int.MinValue;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyBaseCurreny";
                queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(queryData));
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

        public bool IsSendRealtimeManualOrderViaFix(int companyID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                DataTable dt = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetCompanyClearanceCommonData", parameter, "PranaConnectionString").Tables[0];
                DataRow row = dt.Rows[0];
                if (row.Table.Columns.Contains("IsSendRealtimeManualOrderViaFix") && row["IsSendRealtimeManualOrderViaFix"] != DBNull.Value)
                    result = Convert.ToBoolean(row["IsSendRealtimeManualOrderViaFix"]);
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
        /// Gets the is in market included.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public bool GetIsInMarketIncludedForTradingRules(int companyID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetIsInMarketIncludedForTradingRules";
                queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                int result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(queryData));
                if (result > 0)
                {
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
        /// Get Prana.BusinessObjects.Currency by supplying AUECId, also passed this method in Business logic.
        /// </summary>
        /// <param name="AUECID"></param>
        /// <returns></returns>
        public Prana.BusinessObjects.Currency GetCurrencyByAUECID(long AUECID)
        {
            try
            {

                if (!_currencyLookup.ContainsKey(AUECID))
                {
                    Prana.BusinessObjects.Currency currency = GetAUECandCurrencyIDFromDB(AUECID);
                    _currencyLookup.Add(AUECID, currency);

                }
                return _currencyLookup[AUECID];
            }
            #region catch

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }

                #endregion
            }

            // should never come here
            return null;
        }

        private static Prana.BusinessObjects.Currency GetAUECandCurrencyIDFromDB(long AUECID)
        {
            Prana.BusinessObjects.Currency currency = new Prana.BusinessObjects.Currency();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCurrencyByAUECID";
                queryData.DictionaryDatabaseParameter.Add("@AUECID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AUECID",
                    ParameterType = DbType.Int64,
                    ParameterValue = AUECID
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        currency.CurrencyID = Convert.ToInt32(row[0].ToString());
                        currency.Symbol = row[1].ToString();
                        currency.CurrencyName = row[2].ToString();


                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                #endregion
            }
            return currency;
        }

        #endregion

        public Dictionary<string, SortedDictionary<DateTime, ConversionRate>> GetFXConversionRates()
        {
            Dictionary<string, SortedDictionary<DateTime, ConversionRate>> currenyConversions = new Dictionary<string, SortedDictionary<DateTime, ConversionRate>>();
            try
            {
                string key = string.Empty;

                QueryData queryData = new QueryData();
                queryData.Query = "Select * from dbo.GetAllFXConversionRates()";

                //For now we are fetching all the fx rates. the complete table. We need to modify it in such a way that we only pickup
                //for the available open positions and open trades in our system.
                //System.Data.Common.DbCommand cmd = db.GetSqlStringCommand("Select * from dbo.GetFXConversionRatesForDate(@Date)"); //.GetStoredProcCommand("P_GetFXConversionRatesForDate");
                //db.AddInParameter(cmd, "@Date", DbType.DateTime, System.DateTime.UtcNow);
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        ConversionRate conversionRate = new ConversionRate();
                        key = Convert.ToString(Convert.ToInt64(reader[0].ToString())
                                                + Seperators.SEPERATOR_7
                                                + Convert.ToInt64(reader[1].ToString()));
                        conversionRate.Date = (DateTime)reader[4];
                        conversionRate.RateValue = (reader[2] != DBNull.Value ? Convert.ToDouble(reader[2].ToString()) : -1);
                        conversionRate.ConversionMethod = (Operator)Convert.ToInt32(reader[3].ToString());
                        conversionRate.FXeSignalSymbol = ((string)reader[5]).Trim().ToUpper();

                        if (currenyConversions.ContainsKey(key))
                        {
                            SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = currenyConversions[key];
                            if (dateWiseConversionRate.ContainsKey(conversionRate.Date.Date))
                            {
                                dateWiseConversionRate[conversionRate.Date.Date] = conversionRate;
                            }
                            else
                            {
                                dateWiseConversionRate.Add(conversionRate.Date.Date, conversionRate);
                            }
                        }
                        else
                        {
                            DateComparer dateComparer = new DateComparer(SortingOrder.Descending);
                            SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = new SortedDictionary<DateTime, ConversionRate>(dateComparer);
                            dateWiseConversionRate.Add(conversionRate.Date.Date, conversionRate);
                            currenyConversions.Add(key, dateWiseConversionRate);
                        }

                        //if (!currenyConversions.ContainsKey(key))
                        //{
                        //    currenyConversions.Add(key, conversionRate);
                        //}
                        //else
                        //{
                        //    currenyConversions[key] = conversionRate;
                        //}
                    }
                }

                return currenyConversions;
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

        public ConversionRate GetFXConversionRatesForADateAndAccount(int fromCurrencyId, int toCurrencyId, int accountId, DateTime date)
        {
            ConversionRate conversionRate = null;
            try
            {
                object[] parameter = new object[4];
                parameter[0] = fromCurrencyId;
                parameter[1] = toCurrencyId;
                parameter[2] = accountId;
                parameter[3] = date;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFXConversionRateForAGivenDateAndAccount", parameter))
                {
                    while (reader.Read())
                    {
                        conversionRate = new ConversionRate();

                        conversionRate.RateValue = (reader[2] != DBNull.Value ? Convert.ToDouble(reader[2].ToString()) : 0);
                        conversionRate.ConversionMethod = (Operator)Convert.ToInt32(reader[3].ToString());
                        conversionRate.Date = (DateTime)reader[4];
                        conversionRate.FXeSignalSymbol = ((string)reader[5]).Trim().ToUpper();
                        conversionRate.AccountID = (reader[6] != DBNull.Value ? Convert.ToInt32(reader[6].ToString()) : 0);
                    }
                }
                return conversionRate;
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
        /// Gets the fx conversion rates new.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> GetFXConversionRates_New()
        {
            //CHMW-3132	Account wise fx rate handling for expiration settlement
            Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> currenyConversions = new Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>>();
            try
            {
                string currency1 = string.Empty;
                string currency2 = string.Empty;

                QueryData queryData = new QueryData();
                queryData.Query = "Select * from dbo.GetAllFXConversionRates_New()";

                //For now we are fetching all the fx rates. the complete table. We need to modify it in such a way that we only pickup
                //for the available open positions and open trades in our system.
                //System.Data.Common.DbCommand cmd = db.GetSqlStringCommand("Select * from dbo.GetFXConversionRatesForDate(@Date)"); //.GetStoredProcCommand("P_GetFXConversionRatesForDate");
                //db.AddInParameter(cmd, "@Date", DbType.DateTime, System.DateTime.UtcNow);
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        ConversionRate conversionRate = new ConversionRate();
                        currency1 = reader[0].ToString();
                        currency2 = reader[1].ToString();
                        conversionRate.RateValue = (reader[2] != DBNull.Value ? Convert.ToDouble(reader[2].ToString()) : -1);
                        conversionRate.ConversionMethod = Operator.M;
                        conversionRate.Date = (DateTime)reader[3];
                        conversionRate.FXeSignalSymbol = ((string)reader[4]).Trim().ToUpper();
                        conversionRate.AccountID = Convert.ToInt32(reader[5].ToString());
                        ConversionRate conversionRate1 = (ConversionRate)conversionRate.Clone();
                        conversionRate1.ConversionMethod = Operator.D;
                        if (currenyConversions.ContainsKey(conversionRate.AccountID))
                        {
                            AddConversionRateToCurrencyConversions(currenyConversions[conversionRate.AccountID], currency1 + '-' + currency2, conversionRate);
                            AddConversionRateToCurrencyConversions(currenyConversions[conversionRate.AccountID], currency2 + '-' + currency1, conversionRate1);
                        }
                        else
                        {
                            DateComparer dateComparer = new DateComparer(SortingOrder.Descending);
                            SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = new SortedDictionary<DateTime, ConversionRate>(dateComparer) { { conversionRate.Date.Date, conversionRate } };
                            SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate1 = new SortedDictionary<DateTime, ConversionRate>(dateComparer) { { conversionRate1.Date.Date, conversionRate1 } };
                            currenyConversions.Add(conversionRate.AccountID, new Dictionary<string, SortedDictionary<DateTime, ConversionRate>>() { { currency1 + '-' + currency2, dateWiseConversionRate }, { currency2 + '-' + currency1, dateWiseConversionRate1 } });
                        }
                    }
                }

                return currenyConversions;
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
        /// Adds the conversion rate to currency conversions.
        /// </summary>
        /// <param name="currencyConversions">The currency conversions.</param>
        /// <param name="key">The key.</param>
        /// <param name="conversionRate">The conversion rate.</param>
        private static void AddConversionRateToCurrencyConversions(Dictionary<string, SortedDictionary<DateTime, ConversionRate>> currencyConversions, string key, ConversionRate conversionRate)
        {
            try
            {
                if (currencyConversions.ContainsKey(key))
                {
                    currencyConversions[key][conversionRate.Date.Date] = conversionRate;
                }
                else
                {
                    DateComparer dateComparer = new DateComparer(SortingOrder.Descending);
                    SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = new SortedDictionary<DateTime, ConversionRate>(dateComparer) { { conversionRate.Date.Date, conversionRate } };
                    currencyConversions.Add(key, dateWiseConversionRate);
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

        public Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> GetFXConversionRatesForGivenDate(DateTime startDate, DateTime? endDate)
        {
            //CHMW-3132	Account wise fx rate handling for expiration settlement
            Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> currenyConversionsForGivenDate = new Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>>();
            try
            {
                string key = string.Empty;

                string startDateString = startDate.ToShortDateString();
                if (endDate.Equals(null))
                    startDateString = startDate.AddDays(-20).Date.ToShortDateString();//20 days are deducted for start
                //date to include last arbitary 20 days conversion factor so as to accomodate last business day
                //for every AUEC. 20 days, I think is enough to accomodate last business day for any AUEC.
                string startDateTemp = startDateString.Insert(0, "'");
                string finalStartDate = startDateTemp.Insert(startDateTemp.Length, "'");

                string endDateString = startDate.Date.ToShortDateString();
                if (!endDate.Equals(null))
                    endDateString = Convert.ToDateTime(endDate).ToShortDateString();
                string endDateTemp = endDateString.Insert(0, "'");
                string finalEndDate = endDateTemp.Insert(endDateTemp.Length, "'");

                QueryData queryData = new QueryData();
                queryData.Query = "Select * from dbo.GetAllFXConversionRatesForGivenDateRange(" + finalStartDate + ", " + finalEndDate + ")";//.GetStoredProcCommand("P_GetFXConversionRatesForDate");
                //For now we are fetching all the fx rates. the complete table. We need to modify it in such a way that we only pickup
                //for the available open positions and open trades in our system.
                //System.Data.Common.DbCommand cmd = db.GetSqlStringCommand("Select * from dbo.GetFXConversionRatesForDate(@Date)"); //.GetStoredProcCommand("P_GetFXConversionRatesForDate");
                //db.AddInParameter(cmd, "@Date", DbType.DateTime, System.DateTime.UtcNow);
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        ConversionRate conversionRate = new ConversionRate();
                        key = Convert.ToString(Convert.ToInt64(reader[0].ToString())
                                                + Seperators.SEPERATOR_7
                                                + Convert.ToInt64(reader[1].ToString())
                                                + Seperators.SEPERATOR_7
                                                + Convert.ToDateTime(reader[4].ToString()).Date);
                        conversionRate.Date = (DateTime)reader[4];
                        conversionRate.RateValue = (reader[2] != DBNull.Value ? Convert.ToDouble(reader[2].ToString()) : -1);
                        conversionRate.ConversionMethod = (Operator)Convert.ToInt32(reader[3].ToString());
                        conversionRate.FXeSignalSymbol = ((string)reader[5]).Trim().ToUpper();
                        conversionRate.AccountID = Convert.ToInt32(reader[6].ToString());
                        if (currenyConversionsForGivenDate.ContainsKey(conversionRate.AccountID))
                        {
                            if (currenyConversionsForGivenDate[conversionRate.AccountID].ContainsKey(key))
                            {
                                SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = currenyConversionsForGivenDate[conversionRate.AccountID][key];
                                if (dateWiseConversionRate.ContainsKey(conversionRate.Date.Date))
                                {
                                    dateWiseConversionRate[conversionRate.Date.Date] = conversionRate;
                                }
                                else
                                {
                                    dateWiseConversionRate.Add(conversionRate.Date.Date, conversionRate);
                                }
                            }
                            else
                            {
                                DateComparer dateComparer = new DateComparer(SortingOrder.Descending);
                                SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = new SortedDictionary<DateTime, ConversionRate>(dateComparer);
                                dateWiseConversionRate.Add(conversionRate.Date.Date, conversionRate);
                                currenyConversionsForGivenDate[conversionRate.AccountID].Add(key, dateWiseConversionRate);
                            }
                        }
                        else
                        {
                            DateComparer dateComparer = new DateComparer(SortingOrder.Descending);
                            SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = new SortedDictionary<DateTime, ConversionRate>(dateComparer);
                            dateWiseConversionRate.Add(conversionRate.Date.Date, conversionRate);
                            currenyConversionsForGivenDate.Add(conversionRate.AccountID, new Dictionary<string, SortedDictionary<DateTime, ConversionRate>>() { { key, dateWiseConversionRate } });
                        }
                        //if (!currenyConversions.ContainsKey(key))
                        //{
                        //    currenyConversions.Add(key, conversionRate);
                        //}
                        //else
                        //{
                        //    currenyConversions[key] = conversionRate;
                        //}
                    }
                }

                return currenyConversionsForGivenDate;
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

        public Dictionary<string, List<FxPranaSymbol>> GetFxSymbolMapping()
        {
            Dictionary<string, List<FxPranaSymbol>> forexMapping = new Dictionary<string, List<FxPranaSymbol>>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFxSymbolMapping";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    string fxesignalSymbol = reader[0].ToString();
                    string fxtickerSymbol = reader[1].ToString();
                    Operator conversionMethod = (Operator)Convert.ToInt32(reader[2].ToString());

                    if (forexMapping.ContainsKey(fxesignalSymbol))
                    {
                        FxPranaSymbol fxtickerData = new FxPranaSymbol();
                        fxtickerData.ConversionMethod = conversionMethod;
                        fxtickerData.PranaSymbol = fxtickerSymbol;

                        forexMapping[fxesignalSymbol].Add(fxtickerData);
                    }
                    else
                    {
                        List<FxPranaSymbol> listfxtickerDataNew = new List<FxPranaSymbol>();

                        FxPranaSymbol fxtickerData = new FxPranaSymbol();

                        fxtickerData.ConversionMethod = conversionMethod;
                        fxtickerData.PranaSymbol = fxtickerSymbol;
                        listfxtickerDataNew.Add(fxtickerData);

                        forexMapping.Add(fxesignalSymbol, listfxtickerDataNew);
                    }
                }
            }

            return forexMapping;
        }

        public DataSet GetAllStandardCurrencyPairs()
        {
            DataSet currencyPairs = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllStandardCurrencyPairs";

            try
            {
                currencyPairs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return currencyPairs;
        }

        public bool GetPMModulePermission(int companyID)
        {
            bool isPermitted = false;
            int isPer = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetPMModulePermission", parameter))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != System.DBNull.Value)
                        {
                            isPer = int.Parse(row[0].ToString());
                        }
                    }
                }

                if (isPer == 1)
                {
                    isPermitted = true;
                }
                else
                {
                    isPermitted = false;
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
            return isPermitted;
        }

        #region Closing and Corporate Action

        /// <summary>
        /// Returns the latest closing dates for taxlots
        /// </summary>
        public Dictionary<string, DateTime> GetTaxlotsLatestClosingDates()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetFundTaxlotsLatestClosingDates";

            Dictionary<string, DateTime> taxlotIdToCloseDateDict = new Dictionary<string, DateTime>();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int taxlotIdIndex = 0;
                            int auecLocalLastCloseDateIndex = 1;

                            string taxlotIdValue = string.Empty;
                            DateTime auecLocalLastCloseDateValue = DateTimeConstants.MinValue;

                            if (row[taxlotIdIndex] != System.DBNull.Value)
                            {
                                taxlotIdValue = (row[taxlotIdIndex]).ToString();
                            }
                            if (row[auecLocalLastCloseDateIndex] != System.DBNull.Value)
                            {
                                auecLocalLastCloseDateValue = Convert.ToDateTime(row[auecLocalLastCloseDateIndex]);
                            }

                            if (taxlotIdValue != string.Empty && auecLocalLastCloseDateValue != DateTimeConstants.MinValue)
                            {
                                if (taxlotIdToCloseDateDict.ContainsKey(taxlotIdValue))
                                {
                                    taxlotIdToCloseDateDict[taxlotIdValue] = auecLocalLastCloseDateValue;
                                }
                                else
                                {
                                    taxlotIdToCloseDateDict.Add(taxlotIdValue, auecLocalLastCloseDateValue);
                                }
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

            return taxlotIdToCloseDateDict;
        }

        public Dictionary<string, Tuple<DateTime, int>> GetTaxlotsLatestCADates()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetTaxlotsLatestCADates";

            Dictionary<string, Tuple<DateTime, int>> taxlotIdToCADateDict = new Dictionary<string, Tuple<DateTime, int>>();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int taxlotIdIndex = 0;
                            int auecLocalLastCADateIndex = 1;
                            int closingStatusIndex = 2;

                            string taxlotIdValue = string.Empty;
                            DateTime auecLocalLastCADateValue = DateTimeConstants.MinValue;
                            int closingStatus = 0;

                            if (row[taxlotIdIndex] != System.DBNull.Value)
                            {
                                taxlotIdValue = (row[taxlotIdIndex]).ToString();
                            }
                            if (row[auecLocalLastCADateIndex] != System.DBNull.Value)
                            {
                                auecLocalLastCADateValue = Convert.ToDateTime(row[auecLocalLastCADateIndex]);
                            }
                            if (row[closingStatusIndex] != System.DBNull.Value)
                            {
                                closingStatus = Int16.Parse((row[closingStatusIndex]).ToString());
                            }

                            if (taxlotIdValue != string.Empty && auecLocalLastCADateValue != DateTimeConstants.MinValue)
                            {
                                Tuple<DateTime, int> tuple = new Tuple<DateTime, int>(auecLocalLastCADateValue, closingStatus);

                                if (taxlotIdToCADateDict.ContainsKey(taxlotIdValue))
                                {
                                    taxlotIdToCADateDict[taxlotIdValue] = tuple;
                                }
                                else
                                {
                                    taxlotIdToCADateDict.Add(taxlotIdValue, tuple);
                                }
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

            return taxlotIdToCADateDict;
        }

        #endregion

        public Dictionary<string, DateTime> GetExcercisetaxlots()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetFundTaxlotsLatestExerciseDates";

            Dictionary<string, DateTime> taxlotIdExcercisetaxlots = new Dictionary<string, DateTime>();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int taxlotIdIndex = 0;
                            int auecLocalLastCADateIndex = 1;

                            string taxlotIdValue = string.Empty;
                            DateTime auecLocalLastCADateValue = DateTimeConstants.MinValue;

                            if (row[taxlotIdIndex] != System.DBNull.Value)
                            {
                                taxlotIdValue = (row[taxlotIdIndex]).ToString();
                            }
                            if (row[auecLocalLastCADateIndex] != System.DBNull.Value)
                            {
                                auecLocalLastCADateValue = Convert.ToDateTime(row[auecLocalLastCADateIndex]);
                            }

                            if (taxlotIdValue != string.Empty && auecLocalLastCADateValue != DateTimeConstants.MinValue)
                            {
                                if (taxlotIdExcercisetaxlots.ContainsKey(taxlotIdValue))
                                {
                                    taxlotIdExcercisetaxlots[taxlotIdValue] = auecLocalLastCADateValue;
                                }
                                else
                                {
                                    taxlotIdExcercisetaxlots.Add(taxlotIdValue, auecLocalLastCADateValue);
                                }
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

            return taxlotIdExcercisetaxlots;
        }

        public bool GetBreakOrderPreference(int CompanyId)
        {
            object[] parameters = new object[1];
            parameters[0] = CompanyId;
            bool isBreakOrderPreference = false;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetBreakOrderPreferenceFromDB", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            if (row[0] != System.DBNull.Value)
                            {
                                isBreakOrderPreference = Convert.ToBoolean(row[0]);
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
            return isBreakOrderPreference;
        }

        public TranferTradeRules GetTransferTradeRules(int CompanyId)
        {
            object[] parameters = new object[1];
            parameters[0] = CompanyId;

            TranferTradeRules transferTradeRules = new TranferTradeRules();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTransferTradeRules", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int isTIFChange = 0;
                            int isTradingAccChange = 1;
                            int isAccountChange = 2;
                            int isStrategyChange = 3;
                            int isHandlingInstrChange = 4;
                            int isVenueCPChange = 5;
                            int isAllowAllUserToCancelReplaceRemove = 6;
                            int isAllowUserToChangeOrderType = 7;
                            int isExecutionInstrChange = 8;
                            int isAllowAllUserToTransferTrade = 10;
                            int isAllowAllUserToGenerateSub = 11;
                            int isApplyLimitRulesForReplacingStagedOrders = 12;
                            int isApplyLimitRulesForReplacingOtherOrders = 13;
                            int isApplyLimitRulesForReplacingSubOrders = 14;
                            int isAllowRestrictedSecuritiesList = 15;
                            int isAllowAllowedSecuritiesList = 16;
                            int MasterUsersIDs = 17;
                            int isDefaultOrderTypeLimitForMultiDay = 18;


                            if (row[isTIFChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsTIFChange = Convert.ToBoolean(row[isTIFChange]);
                            }
                            if (row[isTradingAccChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsTradingAccChange = Convert.ToBoolean(row[isTradingAccChange]);
                            }
                            if (row[isAccountChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAccountChange = Convert.ToBoolean(row[isAccountChange]);
                            }
                            if (row[isStrategyChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsStrategyChange = Convert.ToBoolean(row[isStrategyChange]);
                            }
                            if (row[isHandlingInstrChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsHandlingInstrChange = Convert.ToBoolean(row[isHandlingInstrChange]);
                            }
                            if (row[isVenueCPChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsVenueCPChange = Convert.ToBoolean(row[isVenueCPChange]);
                            }
                            if (row[isAllowAllUserToCancelReplaceRemove] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowAllUserToCancelReplaceRemove = Convert.ToBoolean(row[isAllowAllUserToCancelReplaceRemove]);
                            }
                            if (row[isAllowUserToChangeOrderType] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowUserToChangeOrderType = Convert.ToBoolean(row[isAllowUserToChangeOrderType]);
                            }
                            if (row[isAllowAllUserToTransferTrade] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowUserToTansferTrade = Convert.ToBoolean(row[isAllowAllUserToTransferTrade]);
                            }
                            if (row[isAllowAllUserToGenerateSub] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowUserToGenerateSub = Convert.ToBoolean(row[isAllowAllUserToGenerateSub]);
                            }
                            if (row[isExecutionInstrChange] != System.DBNull.Value)
                            {
                                transferTradeRules.IsExecutionInstrChange = Convert.ToBoolean(row[isExecutionInstrChange]);
                            }

                            if (row[isApplyLimitRulesForReplacingStagedOrders] != System.DBNull.Value)
                            {
                                transferTradeRules.IsApplyLimitRulesForReplacingStagedOrders = Convert.ToBoolean(row[isApplyLimitRulesForReplacingStagedOrders]);
                            }
                            if (row[isApplyLimitRulesForReplacingOtherOrders] != System.DBNull.Value)
                            {
                                transferTradeRules.IsApplyLimitRulesForReplacingOtherOrders = Convert.ToBoolean(row[isApplyLimitRulesForReplacingOtherOrders]);
                            }

                            if (row[isApplyLimitRulesForReplacingSubOrders] != System.DBNull.Value)
                            {
                                transferTradeRules.IsApplyLimitRulesForReplacingSubOrders = Convert.ToBoolean(row[isApplyLimitRulesForReplacingSubOrders]);
                            }

                            if (row[isAllowRestrictedSecuritiesList] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowRestrictedSecuritiesList = Convert.ToBoolean(row[isAllowRestrictedSecuritiesList]);
                            }

                            if (row[isAllowAllowedSecuritiesList] != System.DBNull.Value)
                            {
                                transferTradeRules.IsAllowAllowedSecuritiesList = Convert.ToBoolean(row[isAllowAllowedSecuritiesList]);
                            }

                            if (row[MasterUsersIDs] != System.DBNull.Value)
                            {
                                string commaSpearatedIds = row[MasterUsersIDs].ToString();
                                List<string> masterUsersIdsFetched = commaSpearatedIds.Split(',').ToList();
                                masterUsersIdsFetched.Remove("");
                                transferTradeRules.MasterUsersIDs = masterUsersIdsFetched.Select(id => Convert.ToInt32(id)).ToList(); ;
                            }

                            if (row[isDefaultOrderTypeLimitForMultiDay] != System.DBNull.Value)
                            {
                                transferTradeRules.IsDefaultOrderTypeLimitForMultiDay = Convert.ToBoolean(row[isDefaultOrderTypeLimitForMultiDay]);
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
            return transferTradeRules;
        }

        public Dictionary<string, Dictionary<string, string>> GetExcerciseGroupIDs(string FromAuecDatesString)
        {
            object[] parameters = new object[1];
            parameters[0] = FromAuecDatesString;

            Dictionary<string, Dictionary<string, string>> GroupIDExcercisedtaxlots = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExercisedGroupIDs", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int ParentTaxlotIdIndex = 0;
                            int underlyingGroupIDIndex = 1;
                            int ClosingGroupIdIndex = 2;

                            string ParentTaxlotIdValue = string.Empty;
                            string underlyingGroupIDValue = string.Empty;
                            string ClosingGroupIDValue = string.Empty;

                            if (row[ParentTaxlotIdIndex] != System.DBNull.Value)
                            {
                                ParentTaxlotIdValue = (row[ParentTaxlotIdIndex]).ToString();
                            }
                            if (row[underlyingGroupIDIndex] != System.DBNull.Value)
                            {
                                underlyingGroupIDValue = (row[underlyingGroupIDIndex]).ToString();
                            }
                            if (row[ClosingGroupIdIndex] != System.DBNull.Value)
                            {
                                ClosingGroupIDValue = (row[ClosingGroupIdIndex]).ToString();
                            }
                            if (ParentTaxlotIdValue != string.Empty && underlyingGroupIDValue != string.Empty && ClosingGroupIDValue != string.Empty)
                            {
                                if (!GroupIDExcercisedtaxlots.ContainsKey(ParentTaxlotIdValue))
                                {
                                    Dictionary<string, string> dictGroupIDs = new Dictionary<string, string>();
                                    dictGroupIDs.Add(underlyingGroupIDValue, ClosingGroupIDValue);
                                    GroupIDExcercisedtaxlots.Add(ParentTaxlotIdValue, dictGroupIDs);
                                }
                                else
                                {
                                    GroupIDExcercisedtaxlots[ParentTaxlotIdValue].Add(underlyingGroupIDValue, ClosingGroupIDValue);
                                }
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

            return GroupIDExcercisedtaxlots;

        }

        #region Account SubAccount
        public DataSet GetAllAccountTablesFromDB()
        {
            return DataManagerInternalRepository.GetAllAccountTablesFromDB();
        }

        public DataSet GetAllAccountsWithRelation(DataSet _masterCategorySubCategory)
        {
            return DataManagerInternalRepository.GetAllAccountsWithRelation(_masterCategorySubCategory);
        }
        //private static DataSet _dataSetActivities = new DataSet();
        public DataSet GetAllActivitiesFromDB()
        {
            return DataManagerInternalRepository.GetAllActivitiesFromDB();
        }

        //public string GetAccountText(int SubAccountID)
        //{

        //}

        #endregion

        #region getting All Symbols
        public SecMasterRequestObj GetAllSymbols()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetAllTradedSymbols";
            queryData.CommandTimeout = 300;

            SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            int taxlotIdIndex = 0;


                            if (row[taxlotIdIndex] != System.DBNull.Value)
                            {
                                secMasterReqobj.AddData(row[taxlotIdIndex].ToString(), ApplicationConstants.SymbologyCodes.TickerSymbol);
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

            return secMasterReqobj;
        }
        #endregion

        public string GetApplicationVersionFromDB()
        {
            string applicationVersion = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetLatestApplicationVersion";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    applicationVersion = ds.Tables[0].Rows[0][0].ToString();
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
            return applicationVersion;
        }

        public Dictionary<Tuple<int, string>, int> GetDefaultAUECDict(int companyID)
        {
            Dictionary<Tuple<int, string>, int> keyValuePairs = new Dictionary<Tuple<int, string>, int>();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetDefaultAUECMappedData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        keyValuePairs.Add(new Tuple<int, string>(Convert.ToInt32(row[0]), Convert.ToString(row[1])), Convert.ToInt32(row[2]));
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
            return keyValuePairs;
        }

        /// <summary>
        /// Gets the cash preference funds from database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tuple<DateTime, bool>> GetCashPreferenceFundsFromDB()
        {
            Dictionary<int, Tuple<DateTime, bool>> cashPrefFunds = new Dictionary<int, Tuple<DateTime, bool>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCashPreferences";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cashPrefFunds.Add(Convert.ToInt32(dr["FundId"].ToString()), new Tuple<DateTime, bool>(Convert.ToDateTime(dr["CashMgmtStartDate"].ToString()), Convert.ToBoolean(dr["IsCalulateDividend"].ToString())));
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
            return cashPrefFunds;
        }

        public Dictionary<string, string> GetTransactionTypeFromDB()
        {
            Dictionary<string, string> dictTransactionType = new Dictionary<string, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTradingTransactionType";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!dictTransactionType.ContainsKey(row[0].ToString()))
                        {
                            dictTransactionType.Add(row[0].ToString(), row[1].ToString());
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
            return dictTransactionType;
        }

        public IList<string[]> GetCounterPartyVernuesSymbolConvertions()
        {
            IList<string[]> rtn = new List<string[]>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCounterPartyVenueSymbolConvention";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string[] srow = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            srow[i] = (row[i] != null && row[i] != DBNull.Value) ? row[i].ToString() : null;
                            rtn.Add(srow);
                        }
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
            return rtn;
        }

        /// <summary>
        /// Function used to check if the total number of Accounts & trading accounts are same permitted to the user,PRANA-11985
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool CheckPermissionAllAccountsAndTradingAccounts(int userID)
        {
            bool resultBool = false;
            try
            {
                TradingAccountCollection _userTradingAccounts = GetTradingAccounts(userID);
                AccountCollection _userAccounts = GetAccounts(userID);

                AccountCollection _totalAccounts = GetAccounts();
                IList<string> _totalTradingAcc = GetAllTradingAccounts();

                if ((_userTradingAccounts.Count == _totalTradingAcc.Count) && (_userAccounts.Count == _totalAccounts.Count))
                    resultBool = true;
                else
                    resultBool = false;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return resultBool;
        }

        public TradingAccountCollection GetAllUsersTradingAccounts()
        {
            TradingAccountCollection tradingAccounts = new TradingAccountCollection();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllTradingAccounts";

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return tradingAccounts;
            #endregion
        }

        public VenueCollection GetAllUsersVenues()
        {
            VenueCollection venues = new VenueCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllUsersVenues";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }

        public void DeleteCustomViewPreference(int userID, string tabname)
        {
            try
            {
                object[] param = new object[2];
                param[0] = userID;
                param[1] = tabname;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeletePranaUserPrefs", param);
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

        public string GetInvalidFundsForSymbolLevel(string fundIds, Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            string validFunds = "";

            try
            {
                object[] param = new object[3];
                param[0] = fundIds;
                if (startDate == null)
                    param[1] = DBNull.Value;
                else
                    param[1] = startDate;
                param[2] = endDate;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_ChangedFundBeforeSymbolWiseAcrrualDate", param))
                {
                    while (reader.Read())
                    {
                        validFunds += reader[1].ToString() + ",";
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
            return validFunds.Trim(',');
        }

        /// <summary>
        /// https://jira.nirvanasolutions.com:8443/browse/PRANA-19274
        /// Here we are using Symbol_PK because, when we update ticker symbol, we get new ticker symbol,
        /// and we cannot use that to find if the original symbol is traded or not. Only Symbol_pk remains same.
        /// </summary>
        /// <param name="symbol_pk"></param>
        /// <returns></returns>
        ///
        #region Check if the security itself or its option is traded in client DB
        public bool IsSymbolTraded(long symbol_pk)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTradeCountBySymbolPK";
                queryData.DictionaryDatabaseParameter.Add("@Symbol_PK", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Symbol_PK",
                    ParameterType = DbType.Int64,
                    ParameterValue = symbol_pk
                });

                int result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(queryData));

                if (result > 0)
                {
                    return true;
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
            return false;
        }
        #endregion

        public SecondaryMarketDataProvider GetSecondaryCompanyMarketDataProvider(int companyID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSecondaryCompanyMarketDataProvider";
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    ParameterName = "@CompanyId",
                    ParameterType = System.Data.DbType.Int32,
                    ParameterValue = companyID,
                    IsOutParameter = false
                });
                queryData.DictionaryDatabaseParameter.Add("@SecondaryMarketDataProvider", new DatabaseParameter()
                {
                    ParameterName = "@SecondaryMarketDataProvider",
                    ParameterType = System.Data.DbType.Int32,
                    IsOutParameter = true
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                return (SecondaryMarketDataProvider)Convert.ToInt32(queryData.DictionaryDatabaseParameter["@SecondaryMarketDataProvider"].ParameterValue);
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
            return SecondaryMarketDataProvider.None;
        }
        public MarketDataProvider GetCompanyMarketDataProvider(int companyID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyMarketDataProvider";
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    ParameterName = "@CompanyId",
                    ParameterType = System.Data.DbType.Int32,
                    ParameterValue = companyID,
                    IsOutParameter = false
                });
                queryData.DictionaryDatabaseParameter.Add("@MarketDataProvider", new DatabaseParameter()
                {
                    ParameterName = "@MarketDataProvider",
                    ParameterType = System.Data.DbType.Int32,
                    IsOutParameter = true
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                return (MarketDataProvider)Convert.ToInt32(queryData.DictionaryDatabaseParameter["@MarketDataProvider"].ParameterValue);
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
            return MarketDataProvider.Esignal;
        }

        /// <summary>
        ///  Gets the information if market data is blocked
        /// </summary>
        public bool IsMarketDataBlocked(int companyID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMarketDataBlockedInformation";
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    ParameterName = "@CompanyId",
                    ParameterType = System.Data.DbType.Int32,
                    ParameterValue = companyID,
                    IsOutParameter = false
                });
                queryData.DictionaryDatabaseParameter.Add("@IsMarketDataBlocked", new DatabaseParameter()
                {
                    ParameterName = "@IsMarketDataBlocked",
                    ParameterType = System.Data.DbType.Boolean,
                    IsOutParameter = true
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                return Convert.ToBoolean(queryData.DictionaryDatabaseParameter["@IsMarketDataBlocked"].ParameterValue);
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

        /// <summary>
        ///  Gets the information for FactSet Contract Type.
        /// </summary>
        public int GetFactSetContractType(int companyID)
        {
            int factSetContractTypeInt = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFactSetContractTypeQueryDataInformation";
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    ParameterName = "@CompanyId",
                    ParameterType = System.Data.DbType.Int32,
                    ParameterValue = companyID,
                    IsOutParameter = false
                });
                queryData.DictionaryDatabaseParameter.Add("@FactSetContractType", new DatabaseParameter()
                {
                    ParameterName = "@FactSetContractType",
                    ParameterType = System.Data.DbType.Int32,
                    IsOutParameter = true
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                factSetContractTypeInt = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@FactSetContractType"].ParameterValue);
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
            return factSetContractTypeInt;
        }

        /// <summary>
        ///  Save in the DB for Prana Preference of RevaluationDailyProcessDays
        /// </summary>
        public int SavePranaPreferencesRevaluationPrefinDB(int dailyProcessDay)
        {
            int i = 0;
            try
            {
                DataSet ds = new DataSet("dsPranaPref");

                DataTable dt = new DataTable("dtPranaPref");
                dt.Columns.Add("PreferenceKey", typeof(string));
                dt.Columns.Add("PreferenceValue", typeof(int));

                dt.Rows.Add("RevaluationDailyProcessDays", dailyProcessDay);

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

        /// <summary>
        /// Retrieves fund-wise executing broker mapping from the db
        /// </summary>
        /// <param name="companyID"></param>
        public Dictionary<int, int> GetAccountWiseExecutingBrokerMappingFromDB(int companyID)
        {
            Dictionary<int, int> accountWiseExecutingBrokerMapping = new Dictionary<int, int>();
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetFundWiseExecutingBroker";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int accountID = Convert.ToInt32(dr[0]);
                        int brokerId = Convert.ToInt32(dr[1]);
                        accountWiseExecutingBrokerMapping.Add(accountID, brokerId);
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
            return accountWiseExecutingBrokerMapping;
        }
    }

    public class FxPranaSymbol
    {
        private string _pranaSymbol;
        public string PranaSymbol
        {
            get { return _pranaSymbol; }
            set { _pranaSymbol = value; }
        }

        private Operator _conversionMethod = Operator.M;
        public Operator ConversionMethod
        {
            get
            {
                return _conversionMethod;
            }
            set
            {
                _conversionMethod = value;
            }
        }
    }
}
