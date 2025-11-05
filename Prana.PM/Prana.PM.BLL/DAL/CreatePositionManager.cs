using Infragistics.Win;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
namespace Prana.PM.BLL
{
    public class CreatePositionManager
    {
        private static int _errorNumber;
        private static string _errorMessage = string.Empty;

        /// <summary>
        /// Gets the strategies for company.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static BindingList<Prana.BusinessObjects.Strategy> GetStrategiesForCompanyID(int userID)
        {
            BindingList<Prana.BusinessObjects.Strategy> strategies = new BindingList<Prana.BusinessObjects.Strategy>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetStrategiesByUserID";
            queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@UserID",
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    //CommonManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, commandSP);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        strategies.Add(FillStrategy(row, 0));

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
            strategies.Insert(0, new Prana.BusinessObjects.Strategy(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));


            return strategies;
        }

        /// <summary>
        /// Fills the strategy.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Prana.BusinessObjects.Strategy FillStrategy(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.Strategy strategy = null;

            if (row != null)
            {
                strategy = new Prana.BusinessObjects.Strategy();
                int StrategyID = offset + 0;
                int Name = offset + 1;

                try
                {
                    strategy.StrategyID = Convert.ToInt32(row[StrategyID]);
                    strategy.Name = Convert.ToString(row[Name]);

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

            return strategy;
        }

        public static BindingList<Venue> GetVenuesbyUserID(int userID)
        {
            BindingList<Venue> venues = new BindingList<Venue>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetVenuesByUserID";
            queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@UserID",
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    Dictionary<string, int> columnOrderInfo = Utils.GetColumnOrderList(reader);
                    // CommonManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, commandSP);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenue(row, columnOrderInfo));

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


            return venues;
        }

        private static Venue FillVenue(object[] row, Dictionary<string, int> columnOrderInfo)
        {
            Venue venue = null;

            if (row != null)
            {
                venue = new Venue();

                try
                {

                    venue.VenueID = Convert.ToInt32(row[columnOrderInfo["VenueID"]]);
                    venue.Name = Convert.ToString(row[columnOrderInfo["VenueName"]]);
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

            return venue;
        }

        /// <summary>
        /// Gets the counter parties for company ID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public static BindingList<CounterParty> GetCounterParties(int userID)
        {
            BindingList<CounterParty> counterparties = new BindingList<CounterParty>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCounterPartiesByUserID";
            queryData.DictionaryDatabaseParameter.Add("@UserID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@UserID",
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    Dictionary<string, int> columnOrderInfo = Utils.GetColumnOrderList(reader);
                    // CommonManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, commandSP);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterparties.Add(FillCounterParty(row, columnOrderInfo));

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
            //counterparties.Insert(0, new CounterParty(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));


            return counterparties;
        }

        /// <summary>
        /// Fills the counter party.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static CounterParty FillCounterParty(object[] row, Dictionary<string, int> columnOrderInfo)
        {

            CounterParty counterParty = null;

            if (row != null)
            {
                counterParty = new CounterParty(int.MinValue, Global.ApplicationConstants.C_COMBO_SELECT);

                try
                {

                    counterParty.CounterPartyID = Convert.ToInt32(row[columnOrderInfo["CounterPartyID"]]);
                    counterParty.Name = Convert.ToString(row[columnOrderInfo["ShortName"]]);
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

            return counterParty;
        }

        /// <summary>
        /// Gets the symbol conventions.
        /// </summary>
        /// <returns></returns>
        public static SymbolConventionList GetSymbolConventions()
        {
            SymbolConventionList result = new SymbolConventionList();


            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetSymbolConventionsList";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    Dictionary<string, int> columnorderInfo = Utils.GetColumnOrderList(reader);

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        result.Add(FillSymbolConvention(row, columnorderInfo));

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
        /// Fills the symbol convention.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnorderInfo">The columnorder info.</param>
        /// <returns></returns>
        private static SymbolConvention FillSymbolConvention(object[] row, Dictionary<string, int> columnorderInfo)
        {
            SymbolConvention result = null;

            if (row != null)
            {

                result = new SymbolConvention();
                try
                {
                    result.ID = Convert.ToInt32(row[columnorderInfo["ID"]]);
                    result.Name = Convert.ToString(row[columnorderInfo["NAME"]]);
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

            return result;
        }




        //SP_OBSOLETE:
        //This functionality is not being used for now.
        /// <summary>
        /// Saves the close trades data.
        /// </summary>
        /// <param name="closeTradeState">State of the close trade.</param>
        public static void SaveManuallyCreatedPositions(OTCPositionList OTCPositionList)
        {


            try
            {
                string OTCpositionsXml = XMLUtilities.SerializeToXML(OTCPositionList);
                XMLSaveManager.SaveThroughXML("PMSaveManuallyCreatedPositions", OTCpositionsXml);
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
        /// Gets the application accounts for company.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <returns></returns>
        public static ValueList GetApplicationAccountsForCompany(int companyID)
        {
            ValueList accountList = new ValueList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetMappedApplicationAccountsForCompany";
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
                    Dictionary<string, int> columnorderInfo = Utils.GetColumnOrderList(reader);
                    XMLSaveManager.GetErrorParameterValues(ref CreatePositionManager._errorMessage, ref CreatePositionManager._errorNumber, queryData.DictionaryDatabaseParameter);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountList.ValueListItems.Add(Convert.ToInt32(row[columnorderInfo["FundID"]]), Convert.ToString(row[columnorderInfo["FundName"]]));
                        //accountList.Add(FillAccount(row, 0));

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
            //  accountList.Insert(0, new Account(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));


            return accountList;
        }

        /// <summary>
        /// Gets the assets.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="thirdPartyID">The data source ID.</param>
        /// <returns></returns>
        public static ValueList GetAssets(int companyID)
        {
            ValueList result = new ValueList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetPermittedAssetsForCompany";
            queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@CompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    Dictionary<string, int> columnorderInfo = Utils.GetColumnOrderList(reader);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            if (row[columnorderInfo["ApplicationAssetID"]] != System.DBNull.Value && row[columnorderInfo["AssetName"]] != System.DBNull.Value)
                            {
                                //reader.GetValues(row);
                                result.ValueListItems.Add(Convert.ToInt32(row[columnorderInfo["ApplicationAssetID"]]), Convert.ToString(row[columnorderInfo["AssetName"]]));

                            }

                            //result.Add(item);

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

            //ValueListItem valItem = new ValueListItem(-3, "Sel");
            //result.ValueListItems.Insert(0, valItem);
            return result;

        }

        /// <summary>
        /// Gets the system currencies.
        /// </summary>
        /// <param name="assetID">The company ID.</param>
        /// <returns></returns>
        public static ValueList GetAllCurrenciesWithIDANDSymbol()
        {
            ValueList result = new ValueList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllCurrenciesWithIDANDSymbol";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    Dictionary<string, int> columnorderInfo = Utils.GetColumnOrderList(reader);
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            if (row[columnorderInfo["CurrencyID"]] != System.DBNull.Value && row[columnorderInfo["CurrencySymbol"]] != System.DBNull.Value)
                            {
                                //reader.GetValues(row);
                                result.ValueListItems.Add(Convert.ToInt32(row[columnorderInfo["CurrencyID"]]), Convert.ToString(row[columnorderInfo["CurrencySymbol"]]));

                            }

                            //result.Add(item);

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


            return result;

        }

        /// <summary>
        /// Gets the AUEC.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="assetID">The asset ID.</param>
        /// <returns></returns>
        public static AUECList GetAUEC(int companyID, Int16 assetID)
        {
            AUECList auecList = new AUECList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "[PMGetAUECForCompanyAndAsset]";
            queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@CompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@AssetID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AssetID",
                ParameterType = DbType.Int32,
                ParameterValue = assetID
            });

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
                            int auecID = 0;
                            int auecName = 1;
                            int AssetID = 2;
                            int UnderlyingID = 3;
                            int ExchangeID = 4;
                            int CurrencyID = 5;

                            AUEC item = new AUEC();

                            if (row[auecID] != System.DBNull.Value)
                            {
                                item.AUECID = Convert.ToInt32(row[auecID]);
                            }
                            if (row[auecName] != System.DBNull.Value)
                            {
                                item.AUECName = row[auecName].ToString();
                            }
                            if (row[AssetID] != System.DBNull.Value)
                            {
                                item.AssetID = Convert.ToInt32(row[AssetID]);
                            }
                            if (row[UnderlyingID] != System.DBNull.Value)
                            {
                                item.UnderlyingID = Convert.ToInt32(row[UnderlyingID]);
                            }
                            if (row[ExchangeID] != System.DBNull.Value)
                            {
                                item.ExchangeID = Convert.ToInt32(row[ExchangeID]);
                            }
                            if (row[CurrencyID] != System.DBNull.Value)
                            {
                                item.CurrencyID = Convert.ToInt32(row[CurrencyID]);
                            }
                            auecList.Add(item);

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

            return auecList;
            //throw new Exception("The method or operation is not implemented.");
        }

        //public static ValueList GetMappedAccountsForCompany(int companyID)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}


        #region AddPositions
        public static ValueList GetCompanyAUECs(int companyID)
        {
            ValueList auecList = new ValueList();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAUECsForCompany";
            queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@CompanyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });

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
                            int auecID = 0;
                            int auecName = 1;
                            ValueListItem item = new ValueListItem();

                            if (row[auecID] != System.DBNull.Value)
                            {
                                item.DataValue = Convert.ToInt32(row[auecID]);
                            }
                            if (row[auecName] != System.DBNull.Value)
                            {
                                item.DisplayText = row[auecName].ToString();
                            }
                            auecList.ValueListItems.Add(item);

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

            return auecList;
        }

        /// <summary>
        /// Saves simulated positions.
        /// </summary>
        /// <param name="simulatedPostions">State of the simulated positions.</param>
        public static void SaveSimulatedPositions(OTCPositionList simulatedPostions)
        {
            try
            {
                string simulatedPositionsXml = XMLUtilities.SerializeToXML(simulatedPostions);
                XMLSaveManager.SaveThroughXML("PMSaveSimulatedPositions", simulatedPositionsXml);
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
        #endregion
    }
}
