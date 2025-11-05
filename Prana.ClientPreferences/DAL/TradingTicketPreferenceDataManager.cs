using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace Prana.ClientPreferences
{
    public static class TradingTicketPreferenceDataManager
    {
        private static TradingTicketPreferenceType _ttPreferenceType;

        public static void SetupManager(TradingTicketPreferenceType ttPreferenceType)
        {
            _ttPreferenceType = ttPreferenceType;
        }
        #region Asset
        public static Dictionary<Asset, Sides> GetCompanyAssets()
        {
            string spName = "P_GetAllAssets";
            Dictionary<Asset, Sides> assets = new Dictionary<Asset, Sides>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assets.FillCompanyAssets(row, 0);

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
            return assets;
        }

        public static Asset FillCompanyAssets(this Dictionary<Asset, Sides> assets, object[] row, int offSet)
        {
            int assetID = 0 + offSet;
            int assetName = 1 + offSet;
            Asset asset = null;
            try
            {
                asset = new Asset(int.Parse(row[assetID].ToString()), row[assetName].ToString());
                Sides sides = GetSidesByAsset(int.Parse(row[assetID].ToString()));
                assets.Add(asset, sides);
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
            return asset;
        }
        #endregion

        #region OrderSides
        public static Sides GetSidesByAsset(int assetID)
        {
            Sides sides = new Sides();

            object[] parameter = new object[1];
            parameter[0] = assetID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSidesByAsset", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        sides.Add(FillOrderSide(row, 0));
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
            return sides;
        }

        private static Side FillOrderSide(object[] row, int offset)
        {
            Side orderSide = new Side();
            try
            {
                if (row != null)
                {
                    int sideid = offset + 0;
                    int side = offset + 1;
                    int sidetagvalue = offset + 2;

                    orderSide.SideID = Convert.ToInt32(row[sideid]);
                    if (DBNull.Value != row[side])
                    {
                        orderSide.Name = Convert.ToString(row[side]);
                    }
                    if (DBNull.Value != row[sidetagvalue])
                    {
                        orderSide.TagValue = Convert.ToString(row[sidetagvalue]);
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
            return orderSide;
        }
        #endregion

        #region CounterParty
        public static CounterParty FillCounterParties(object[] row, int offSet)
        {

            int counterPartyID = 0 + offSet;
            int shortName = 1 + offSet;
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
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return counterParty;
        }

        public static CounterPartyCollection GetAllCompanyPermittedCounterparties(int companyID)
        {
            string spName = (_ttPreferenceType == TradingTicketPreferenceType.Company) ? "P_GetCompanyPermittedCounterParties" : "P_GetUserCounterParties";
            CounterPartyCollection companyCounterParties = new CounterPartyCollection();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCounterParties(row, 0));

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
            return companyCounterParties;
        }
        #endregion

        #region Venue

        private static Venue FillVenues(object[] row, int offset)
        {

            Venue venue = null;
            try
            {
                if (row != null)
                {
                    venue = new Venue();
                    int venueId = offset + 0;
                    int venueName = offset + 1;
                    venue.VenueID = Convert.ToInt32(row[venueId]);
                    venue.Name = Convert.ToString(row[venueName]);
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
            return venue;
        }

        public static VenueCollection GetVenues(int companyID, int counterPartyID, TradingTicketPreferenceType _ttPrefType)
        {
            VenueCollection venues = new VenueCollection();
            string spName = (_ttPrefType == TradingTicketPreferenceType.Company) ? "P_GetCompanyPermittedVenues" : "P_GetUserVenues";

            try
            {
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = counterPartyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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

        #endregion

        #region OrderTypes
        private static OrderType FillOrderType(object[] row, int offset)
        {
            OrderType orderType = new OrderType();
            try
            {
                if (row != null)
                {
                    int ordertypesid = offset + 0;
                    int ordertypes = offset + 1;
                    int ordertypestagvalue = offset + 2;
                    orderType.OrderTypesID = Convert.ToInt32(row[ordertypesid]);
                    if (DBNull.Value != row[ordertypes])
                    {

                        orderType.Type = Convert.ToString(row[ordertypes]);
                    }
                    orderType.TagValue = Convert.ToString(row[ordertypestagvalue]);
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

        public static OrderTypes GetOrderTypes(int companyID)
        {
            OrderTypes orderTypes = new OrderTypes();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPermittedOrderTypes", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillOrderType(row, 0));
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
        #endregion

        #region TimeInForce

        private static TimeInForce FillTimeInForce(object[] row, int offset)
        {

            TimeInForce timeInForce = new TimeInForce();
            try
            {
                if (row != null)
                {
                    int timeinforceid = offset + 0;
                    int timeinforce = offset + 1;
                    int timeinforcetagvalue = offset + 2;

                    timeInForce.TimeInForceID = Convert.ToInt32(row[timeinforceid]);
                    if (DBNull.Value != row[timeinforce])
                    {
                        timeInForce.Name = Convert.ToString(row[timeinforce]);
                    }
                    if (DBNull.Value != row[timeinforcetagvalue])
                    {
                        timeInForce.TagValue = Convert.ToString(row[timeinforcetagvalue]);
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
            return timeInForce;
        }

        public static TimeInForces GetTimeInForces(int companyID)
        {
            TimeInForces timeInForces = new TimeInForces();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPermittedTimeInForce", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForces.Add(FillTimeInForce(row, 0));
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
                    int handlinginstructionsid = offset + 0;
                    int handlinginstructions = offset + 1;
                    int handlinginstructionstagvalue = offset + 2;

                    handlingInstruction.HandlingInstructionID = Convert.ToInt32(row[handlinginstructionsid]);
                    if (DBNull.Value != row[handlinginstructions])
                    {
                        handlingInstruction.Name = Convert.ToString(row[handlinginstructions]);
                    }
                    if (DBNull.Value != row[handlinginstructionstagvalue])
                    {
                        handlingInstruction.TagValue = Convert.ToString(row[handlinginstructionstagvalue]);
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
            return handlingInstruction;

        }

        public static HandlingInstructions GetHandlingInstructions(int companyID)
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPermittedHandlingInstructions", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstructions.Add(FillHandlingInstruction(row, 0));
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
                    int executioninstructionsid = offset + 0;
                    int executioninstructions = offset + 1;
                    int executioninstructionstagvalue = offset + 2;

                    executionInstruction.ExecutionInstructionsID = Convert.ToInt32(row[executioninstructionsid]);
                    if (DBNull.Value != row[executioninstructions])
                    {
                        executionInstruction.ExecutionInstructions = Convert.ToString(row[executioninstructions]);
                    }
                    executionInstruction.TagValue = Convert.ToString(row[executioninstructionstagvalue]);
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
        public static ExecutionInstructions GetExecutionInstructions(int companyID)
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPermittedExecutionInstructions", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstructions.Add(FillExecutionInstruction(row, 0));
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
            return executionInstructions;
        }
        #endregion

        #region TradingAccounts

        private static TradingAccount FillTradingAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            TradingAccount tradingAccount = null;
            try
            {
                if (row != null)
                {
                    tradingAccount = new TradingAccount();
                    int tradingaccountId = offset + 0;
                    int tradingaccountName = offset + 1;

                    tradingAccount.TradingAccountID = Convert.ToInt32(row[tradingaccountId]);
                    tradingAccount.Name = Convert.ToString(row[tradingaccountName]);
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
            return tradingAccount;
        }

        public static TradingAccountCollection GetTradingAccounts(int companyID)
        {
            TradingAccountCollection tradingAccounts = new TradingAccountCollection();

            string spName = (_ttPreferenceType == TradingTicketPreferenceType.Company) ? "P_GetCompanyTradingAccounts" : "P_GetUserTradingAccounts";

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccounts.Add(FillTradingAccount(row, 0));
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
            return tradingAccounts;
        }

        #endregion

        #region Strategies

        private static Strategy FillStrategy(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Strategy strategy = null;
            try
            {
                if (row != null)
                {
                    strategy = new Strategy();
                    int strategyId = offset + 0;
                    int strategyShortName = offset + 2;

                    strategy.StrategyID = Convert.ToInt32(row[strategyId]);
                    strategy.Name = Convert.ToString(row[strategyShortName]);
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
            return strategy;
        }

        public static StrategyCollection GetStrategies(int companyID)
        {

            StrategyCollection strategies = new StrategyCollection();
            string spName = (_ttPreferenceType == TradingTicketPreferenceType.Company) ? "P_GetCompanyStrategies" : "P_GetUserStrategies";

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
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
            return strategies;
        }

        #endregion

        #region Accounts

        private static Account FillAccount(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Account account = null;
            try
            {
                if (row != null)
                {
                    account = new Account();
                    int accountID = offset + 0;
                    int accountName = offset + 1;

                    account.AccountID = Convert.ToInt32(row[accountID]);
                    account.Name = Convert.ToString(row[accountName]);
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

        public static AccountCollection GetAccounts(int companyID)
        {
            AccountCollection accounts = new AccountCollection();
            accounts.Add(new Account(int.MinValue, ApplicationConstants.C_LIT_UNALLOCATED));
            string spName = (_ttPreferenceType == TradingTicketPreferenceType.Company) ? "P_GetCompanyFunds" : "P_GetUserFunds";

            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accounts;
        }

        #endregion

        #region Currency

        /// <summary>
        /// Fills the row of Currency Converter <see cref="CurrencyConversion"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="CounterParty"/></returns>
        private static Currency FillCurrency(object[] row, int offset)
        {

            if (offset < 0)
            {
                offset = 0;
            }
            Currency currency = null;
            try
            {
                if (row != null)
                {
                    currency = new Currency();
                    int currencyId = offset + 0;
                    int currencyName = offset + 1;
                    int currencySymbol = offset + 2;
                    currency.CurrencyID = Convert.ToInt32(row[currencyId]);
                    currency.CurrencyName = Convert.ToString(row[currencyName]);
                    currency.Symbol = Convert.ToString(row[currencySymbol]);
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
            return currency;
        }

        public static CurrencyCollection GetCurrencies()
        {
            CurrencyCollection currencyCollection = new CurrencyCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrencies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyCollection.Add(FillCurrency(row, 0));
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
            return currencyCollection;
        }

        #endregion

        #region Preferences
        public static TradingTicketUIPrefs GetTTCompanyUIPreferences(int companyID)
        {
            string spName = "P_GetTTCompanyPreferences";
            return GetTTPeferences(companyID, spName, true);
        }

        public static TradingTicketUIPrefs GetTTUserUIPreferences(int userID)
        {
            string spName = "P_GetTTUserPreferences";
            return GetTTPeferences(userID, spName, false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static TradingTicketUIPrefs GetTTPeferences(int id, string spName, bool isCompanyLevelPref)
        {
            TradingTicketUIPrefs preferences = new TradingTicketUIPrefs();
            try
            {
                DataSet ds = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@id",
                    ParameterType = DbType.Int32,
                    ParameterValue = id
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables.Count > 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DefAssetSide defAssetSide = new DefAssetSide();
                        defAssetSide.Asset = (int?)ReturnValueAfterConversion(row, 0, typeof(Int32));
                        defAssetSide.OrderSide = (int?)ReturnValueAfterConversion(row, 1, typeof(Int32));
                        preferences.DefAssetSides.Add(defAssetSide);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                        FillPreferenceObject(ds.Tables[1].Rows[0], 0, ref preferences, isCompanyLevelPref);
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
                return null;
            }
            return preferences;
        }

        private static void FillPreferenceObject(DataRow row, int offset, ref TradingTicketUIPrefs preferences, bool isCompanyLevelPref)
        {
            int CounterPartyID = 0 + offset;
            int VenueID = 1 + offset;
            int OrderTypeID = 2 + offset;
            int TimeInForceID = 3 + offset;
            int ExecutionInstructionID = 4 + offset;
            int HandlingInstructionID = 5 + offset;
            int TradingAccountID = 6 + offset;
            int StrategyID = 7 + offset;
            int AccountID = 8 + offset;
            int isSettlementCurrencyBase = 9 + offset;
            int Quantity = 10 + offset;
            int IncrementOnQty = 11 + offset;
            int IncrementOnStop = 12 + offset;
            int IncrementOnLimit = 13 + offset;
            int QuantityType = 14 + offset;
            int isShowTargetQTY = 15 + offset;
            int TTControlsMappings = 16 + offset;


            preferences.Broker = (int?)ReturnValueAfterConversion(row, CounterPartyID, typeof(Int32));
            preferences.Venue = (int?)ReturnValueAfterConversion(row, VenueID, typeof(Int32));
            preferences.OrderType = (int?)ReturnValueAfterConversion(row, OrderTypeID, typeof(Int32));
            preferences.TimeInForce = (int?)ReturnValueAfterConversion(row, TimeInForceID, typeof(Int32));
            preferences.ExecutionInstruction = (int?)ReturnValueAfterConversion(row, ExecutionInstructionID, typeof(Int32));
            preferences.HandlingInstruction = (int?)ReturnValueAfterConversion(row, HandlingInstructionID, typeof(Int32));
            preferences.TradingAccount = (int?)ReturnValueAfterConversion(row, TradingAccountID, typeof(Int32));
            preferences.Strategy = (int?)ReturnValueAfterConversion(row, StrategyID, typeof(Int32));
            preferences.Account = (int?)ReturnValueAfterConversion(row, AccountID, typeof(Int32));
            preferences.IsSettlementCurrencyBase = (bool?)ReturnValueAfterConversion(row, isSettlementCurrencyBase, typeof(Boolean));
            preferences.Quantity = (double?)ReturnValueAfterConversion(row, Quantity, typeof(Double));
            preferences.IncrementOnQty = (double?)ReturnValueAfterConversion(row, IncrementOnQty, typeof(Double));
            preferences.IncrementOnStop = (double?)ReturnValueAfterConversion(row, IncrementOnStop, typeof(Double));
            preferences.IncrementOnLimit = (double?)ReturnValueAfterConversion(row, IncrementOnLimit, typeof(Double));
            preferences.QuantityType = (QuantityTypeOnTT)ReturnValueAfterConversion(row, QuantityType, typeof(Int32));
            if (isCompanyLevelPref)
            {
                preferences.IsShowTargetQTY = (bool?)ReturnValueAfterConversion(row, isShowTargetQTY, typeof(Boolean));
                preferences.DefTTControlsMapping = (ReturnValueAfterConversion(row, TTControlsMappings, typeof(String)).ToString());
            }
            else
            {
                int isUseRoundLots = 15 + offset;
                preferences.IsUseRoundLots = (bool)ReturnValueAfterConversion(row, isUseRoundLots, typeof(Boolean));
            }
        }


        #endregion

        private static object ReturnValueAfterConversion(DataRow row, int rowIndex, Type type)
        {
            object value = null;
            if (!row.IsNull(rowIndex))
            {
                if (type == typeof(Int32))
                {
                    value = Convert.ToInt32(row[rowIndex]);
                }
                else if (type == typeof(Double))
                {
                    value = Convert.ToDouble(row[rowIndex]);
                }
                else if ((type == typeof(Boolean)))
                {
                    value = Convert.ToBoolean(row[rowIndex]);
                }
                else if ((type == typeof(String)))
                {
                    value = row[rowIndex].ToString();
                }
            }

            return value;
        }

        public static void SaveTTCompanyUIPreferences(int companyID, TradingTicketUIPrefs preferences)
        {
            string spName = "P_SaveTradingTicketCompanyUIPreferences";
            SavePreferences(companyID, preferences, spName);
        }

        public static void SaveTTUserUIPreferences(int userID, TradingTicketUIPrefs preferences)
        {
            string spName = "P_SaveTradingTicketUserUIPreferences";
            SavePreferences(userID, preferences, spName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void SavePreferences(int idToUse, TradingTicketUIPrefs preferences, string spName)
        {
            int errorNumber = 0;
            string errorMessage = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                List<TradingTicketUIPrefs> lstTradingTicketUIPrefs = new List<TradingTicketUIPrefs>();
                lstTradingTicketUIPrefs.Add(preferences);
                DataTable dt = GeneralUtilities.GetDataTableFromList(lstTradingTicketUIPrefs);
                ds.Tables.Add(dt);
                if (ds != null)
                {
                    dt.TableName = "TradingTicketUIPreferences";
                    string generatedXml = ds.GetXml();
                    generatedXml = RemoveNilNodesFromXml(generatedXml);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = spName;
                    queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Xml",
                        ParameterType = DbType.String,
                        ParameterValue = generatedXml
                    });
                    queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@id",
                        ParameterType = DbType.Int32,
                        ParameterValue = idToUse
                    });

                    XMLSaveManager.AddOutErrorParameters(queryData);
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                    XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, queryData.DictionaryDatabaseParameter);
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

        public static string RemoveNilNodesFromXml(string generatedXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(generatedXml);

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("OrderSide");
            List<XmlElement> xNodes = nodes.Cast<XmlElement>().ToList();
            foreach (XmlElement node in xNodes)
            {
                if (node.InnerText == String.Empty && node.Attributes != null && node.Attributes.Count == 1 && Convert.ToBoolean(node.Attributes["xsi:nil"].Value))
                {
                    XmlNode xNode = node.ParentNode;
                    if (xNode != null)
                        xNode.RemoveChild(node);
                }
            }
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// to save _company trading rules prefs into the database
        /// </summary>
        ///<param name="companyID"></param>
        /// <returns></returns>
        public static void SaveTTRulesPreferences(int companyID, TradingTicketRulesPrefs ttRulesPreferences)
        {
            int errorNumber = 0;
            string errorMessage = string.Empty;
            try
            {
                string spName = "P_SaveTradingRulesPreferences";
                DataSet ds = new DataSet();
                List<TradingTicketRulesPrefs> lstTradingTicketUIPrefs = new List<TradingTicketRulesPrefs>();
                lstTradingTicketUIPrefs.Add(ttRulesPreferences);
                DataTable dt = GeneralUtilities.GetDataTableFromList(lstTradingTicketUIPrefs);
                ds.Tables.Add(dt);
                if (ds != null)
                {
                    dt.TableName = "TradingTicketRulesPreferences";
                    string generatedXml = ds.GetXml();
                    generatedXml = RemoveNilNodesFromXml(generatedXml);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = spName;
                    queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Xml",
                        ParameterType = DbType.String,
                        ParameterValue = generatedXml
                    });
                    queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@id",
                        ParameterType = DbType.Int32,
                        ParameterValue = companyID
                    });

                    XMLSaveManager.AddOutErrorParameters(queryData);
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                    XMLSaveManager.GetErrorParameterValues(ref errorMessage, ref errorNumber, queryData.DictionaryDatabaseParameter);
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

        /// <summary>
        /// to get _company trading rules prefs from the database
        /// </summary>
        ///<param name="companyID"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static TradingTicketRulesPrefs GetTradingRulesPreferences(int companyID)
        {
            string spName = "P_GetTradingRulesPreferences";
            TradingTicketRulesPrefs tradingRulesPref = new TradingTicketRulesPrefs();
            try
            {
                DataSet ds = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@id",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    FillTradingRulesPrefObject(ds.Tables[0].Rows[0], 0, ref tradingRulesPref);
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
                return null;
            }
            return tradingRulesPref;
        }

        /// <summary>
        /// Fills the trading rules preference object.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="tradingRulesPref">The trading rules preference.</param>
        private static void FillTradingRulesPrefObject(DataRow row, int offset, ref TradingTicketRulesPrefs tradingRulesPref)
        {
            try
            {
                int IsOversellTradingRule = 0 + offset;
                int IsOverbuyTradingRule = 1 + offset;
                int IsUnallocatedTradeAlert = 2 + offset;
                int IsFatFingerTradingRule = 3 + offset;
                int IsDuplicateTradeAlert = 4 + offset;
                int IsPendingNewTradeAlert = 5 + offset;
                int DefineFatFingerPercent = 6 + offset;
                int DuplicateTradeAlertTime = 7 + offset;
                int PendingNewOrderAlertTime = 8 + offset;
                int FatFingerAccountOrMasterFund = 9 + offset;
                int IsbsoluteAmountOrDefinePercent = 10 + offset;
                int IsInMarketIncluded = 11 + offset;
                int IsSharesOutstandingRule = 12 + offset;
                int SharesOutstandingAccountOrMF = 13 + offset;
                int SharesOutstandingPercent = 14 + offset;

                tradingRulesPref.IsOversellTradingRule = (bool?)ReturnValueAfterConversion(row, IsOversellTradingRule, typeof(Boolean));
                tradingRulesPref.IsOverbuyTradingRule = (bool?)ReturnValueAfterConversion(row, IsOverbuyTradingRule, typeof(Boolean));
                tradingRulesPref.IsUnallocatedTradeAlert = (bool?)ReturnValueAfterConversion(row, IsUnallocatedTradeAlert, typeof(Boolean));
                tradingRulesPref.IsFatFingerTradingRule = (bool?)ReturnValueAfterConversion(row, IsFatFingerTradingRule, typeof(Boolean));
                tradingRulesPref.IsDuplicateTradeAlert = (bool?)ReturnValueAfterConversion(row, IsDuplicateTradeAlert, typeof(Boolean));
                tradingRulesPref.IsPendingNewTradeAlert = (bool?)ReturnValueAfterConversion(row, IsPendingNewTradeAlert, typeof(Boolean));
                tradingRulesPref.DefineFatFingerValue = (double?)ReturnValueAfterConversion(row, DefineFatFingerPercent, typeof(double));
                tradingRulesPref.DuplicateTradeAlertTime = (int?)ReturnValueAfterConversion(row, DuplicateTradeAlertTime, typeof(Int32));
                tradingRulesPref.PendingNewOrderAlertTime = (int?)ReturnValueAfterConversion(row, PendingNewOrderAlertTime, typeof(Int32));
                tradingRulesPref.FatFingerAccountOrMasterFund = (int?)ReturnValueAfterConversion(row, FatFingerAccountOrMasterFund, typeof(Int32));
                tradingRulesPref.IsAbsoluteAmountOrDefinePercent = (int?)ReturnValueAfterConversion(row, IsbsoluteAmountOrDefinePercent, typeof(Int32));
                tradingRulesPref.IsInMarketIncluded = (bool?)ReturnValueAfterConversion(row, IsInMarketIncluded, typeof(Boolean));
                tradingRulesPref.IsSharesOutstandingRule = (bool?)ReturnValueAfterConversion(row, IsSharesOutstandingRule, typeof(Boolean));
                tradingRulesPref.SharesOutstandingAccOrMF = (int?)ReturnValueAfterConversion(row, SharesOutstandingAccountOrMF, typeof(Int32));
                tradingRulesPref.SharesOutstandingValue = (double?)ReturnValueAfterConversion(row, SharesOutstandingPercent, typeof(double));
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
        /// Saves the Security List based on restricted or allowed type
        /// </summary>
        public static bool SaveSecuritiesList(int companyID, string securitiesListType, string securitiesListToSave, bool isTickerSymbology)
        {
            try
            {
                int AllowedOrRestricted;
                if (securitiesListType == "Restricted")
                    AllowedOrRestricted = 0;
                else
                    AllowedOrRestricted = 1;
                object[] parameter = new object[4];
                parameter[0] = companyID;
                parameter[1] = securitiesListToSave;
                parameter[2] = AllowedOrRestricted;
                parameter[3] = isTickerSymbology;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveSecuritiesList", parameter);
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
        /// Deletes the Security List based on restricted or allowed type
        /// </summary>
        public static void DeleteSecuritiesList(int companyID, string securitiesListType)
        {
            try
            {
                int AllowedOrRestricted;
                if (securitiesListType == "Restricted")
                    AllowedOrRestricted = 0;
                else
                    AllowedOrRestricted = 1;
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = AllowedOrRestricted;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSecuritiesList", parameter);
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
        /// Gets the Security List and symbology based on restricted or allowed type
        /// </summary>
        public static Tuple<string, bool> GetSecuritiesList(int companyID, string securitiesListType)
        {
            string securitiesListFetched = string.Empty;
            bool _isTickerSymbology = true;
            try
            {
                int AllowedOrRestricted;
                if (securitiesListType == "Restricted")
                    AllowedOrRestricted = 0;
                else
                    AllowedOrRestricted = 1;
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = AllowedOrRestricted;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSecuritiesListAndSymbology", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int Symbol = 0;
                        int IsTickerSymbology = 1;
                        if (row != null)
                        {
                            if (row[Symbol] != System.DBNull.Value)
                            {
                                securitiesListFetched = Convert.ToString(row[Symbol]);
                            }
                            if (row[IsTickerSymbology] != System.DBNull.Value)
                            {
                                _isTickerSymbology = Convert.ToBoolean(row[IsTickerSymbology]);
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

            return Tuple.Create(securitiesListFetched, _isTickerSymbology);
        }
    }
}