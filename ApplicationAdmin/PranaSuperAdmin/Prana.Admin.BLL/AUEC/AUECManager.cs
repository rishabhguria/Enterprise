#region Using namespaces

using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CommonObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AUECManager.
    /// </summary>
    public class AUECManager : ServicedComponent
    {
        private AUECManager()
        {
        }

        #region AUEC only

        public static AUEC FillAUECDetails(object[] row, int offSet)
        {
            int AUECID = 0 + offSet;
            int AssetID = 1 + offSet;
            int UnderlyingID = 2 + offSet;
            int ExchangeID = 3 + offSet;

            int FullName = 4 + offSet;
            int DisplayName = 5 + offSet;
            int UnitID = 6 + offSet;
            int TimeZone = 7 + offSet;
            int TimeZoneOffSet = 8 + offSet;
            int PreMarket = 9 + offSet;
            int PreMarketTradingStartTime = 10 + offSet;
            int PreMarketTradingEndTime = 11 + offSet;
            int RegularTime = 12 + offSet;
            int RegularTradingStartTime = 13 + offSet;
            int RegularTradingEndTime = 14 + offSet;
            int LunchTime = 15 + offSet;
            int LunchTimeStartTime = 16 + offSet;
            int LunchTimeEndTime = 17 + offSet;
            int PostMarket = 18 + offSet;
            int PostMarketTradingStartTime = 19 + offSet;
            int PostMarketTradingEndTime = 20 + offSet;
            int SettlementDaysBuy = 21 + offSet;
            int DayLightSaving = 22 + offSet;
            int Country = 23 + offSet;
            int StateID = 24 + offSet;
            int CountryFlagID = 25 + offSet;
            int ExchangeLogoID = 26 + offSet;
            int CurrencyConversion = 27 + offSet;

            int SymbolConventionID = 28 + offSet;
            int ExchangeIdentifier = 29 + offSet;
            int MarketDataProviderExchangeIdentifier = 30 + offSet;

            int BaseCurrencyID = 31 + offSet;
            int OtherCurrencyID = 32 + offSet;
            int Multiplier = 33 + offSet;

            int IsShortSaleConfirmation = 34 + offSet;
            int ProvideAccountNameWithTrade = 35 + offSet;
            int ProvideIdentifierNameWithTrade = 36 + offSet;
            int IdentifierID = 37 + offSet;

            int PurchaseSecFees = 38 + offSet;
            int SaleSecFees = 39 + offSet;
            int PurchaseStamp = 40 + offSet;
            int SaleStamp = 41 + offSet;
            int PurchaseLevy = 42 + offSet;
            int SaleLevy = 43 + offSet;
            int SettlementDaysSell = 44 + offSet;
            // set roundlot index , PRANA-11159
            int RoundLot = 45 + offSet;

            AUEC auec = new AUEC();
            try
            {
                if (row[AUECID] != null)
                {
                    auec.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (row[AssetID] != System.DBNull.Value)
                {
                    auec.AssetID = int.Parse(row[AssetID].ToString());
                }
                if (row[UnderlyingID] != System.DBNull.Value)
                {
                    auec.UnderlyingID = int.Parse(row[UnderlyingID].ToString());
                }
                if (row[ExchangeID] != System.DBNull.Value)
                {
                    auec.ExchangeID = int.Parse(row[ExchangeID].ToString());
                }

                if (row[FullName] != System.DBNull.Value)
                {
                    auec.FullName = row[FullName].ToString();
                }
                if (row[DisplayName] != System.DBNull.Value)
                {
                    auec.DisplayName = row[DisplayName].ToString();
                }
                if (row[UnitID] != System.DBNull.Value)
                {
                    auec.Unit = int.Parse(row[UnitID].ToString());
                }
                if (row[TimeZone] != System.DBNull.Value)
                {
                    auec.TimeZone = row[TimeZone].ToString();
                }
                if (row[TimeZoneOffSet] != System.DBNull.Value)
                {
                    auec.TimeZoneOffSet = double.Parse(row[TimeZoneOffSet].ToString());
                }
                if (row[PreMarket] != System.DBNull.Value)
                {
                    auec.PreMarketCheck = int.Parse(row[PreMarket].ToString());
                }
                if (row[PreMarketTradingStartTime] != System.DBNull.Value)
                {
                    auec.PreMarketTradingStartTime = DateTime.Parse(row[PreMarketTradingStartTime].ToString());
                }
                if (row[PreMarketTradingEndTime] != System.DBNull.Value)
                {
                    auec.PreMarketTradingEndTime = DateTime.Parse(row[PreMarketTradingEndTime].ToString());
                }
                if (row[RegularTime] != System.DBNull.Value)
                {
                    auec.RegularTimeCheck = int.Parse(row[RegularTime].ToString());
                }
                if (row[RegularTradingStartTime] != System.DBNull.Value)
                {
                    auec.RegularTradingStartTime = DateTime.Parse(row[RegularTradingStartTime].ToString());
                }
                if (row[RegularTradingEndTime] != System.DBNull.Value)
                {
                    auec.RegularTradingEndTime = DateTime.Parse(row[RegularTradingEndTime].ToString());
                }
                if (row[LunchTime] != System.DBNull.Value)
                {
                    auec.LunchTimeCheck = int.Parse(row[LunchTime].ToString());
                }
                if (row[LunchTimeStartTime] != System.DBNull.Value)
                {
                    auec.LunchTimeStartTime = DateTime.Parse(row[LunchTimeStartTime].ToString());
                }
                if (row[LunchTimeEndTime] != System.DBNull.Value)
                {
                    auec.LunchTimeEndTime = DateTime.Parse(row[LunchTimeEndTime].ToString());
                }
                if (row[PostMarket] != System.DBNull.Value)
                {
                    auec.PostMarketCheck = int.Parse(row[PostMarket].ToString());
                }
                if (row[PostMarketTradingStartTime] != System.DBNull.Value)
                {
                    auec.PostMarketTradingStartTime = DateTime.Parse(row[PostMarketTradingStartTime].ToString());
                }
                if (row[PostMarketTradingEndTime] != System.DBNull.Value)
                {
                    auec.PostMarketTradingEndTime = DateTime.Parse(row[PostMarketTradingEndTime].ToString());
                }
                if (row[SettlementDaysBuy] != System.DBNull.Value)
                {
                    auec.SettlementDaysBuy = int.Parse(row[SettlementDaysBuy].ToString());
                }
                if (row[DayLightSaving] != System.DBNull.Value)
                {
                    auec.DayLightSaving = row[DayLightSaving].ToString();
                }
                if (row[Country] != System.DBNull.Value)
                {
                    auec.Country = int.Parse(row[Country].ToString());
                }
                if (row[StateID] != System.DBNull.Value)
                {
                    auec.StateID = int.Parse(row[StateID].ToString());
                }
                if (row[CountryFlagID] != System.DBNull.Value)
                {
                    auec.CountryFlagID = int.Parse(row[CountryFlagID].ToString());
                }
                if (row[ExchangeLogoID] != System.DBNull.Value)
                {
                    auec.LogoID = int.Parse(row[ExchangeLogoID].ToString());
                }
                if (row[CurrencyConversion] != System.DBNull.Value)
                {
                    auec.CurrencyConversion = int.Parse(row[CurrencyConversion].ToString());
                }

                if (row[SymbolConventionID] != System.DBNull.Value)
                {
                    auec.SymbolConventionID = int.Parse(row[SymbolConventionID].ToString());
                }
                if (row[ExchangeIdentifier] != System.DBNull.Value)
                {
                    auec.ExchangeIdentifier = row[ExchangeIdentifier].ToString();
                }
                if (row[MarketDataProviderExchangeIdentifier] != System.DBNull.Value)
                {
                    auec.MarketDataProviderExchangeIdentifier = row[MarketDataProviderExchangeIdentifier].ToString();
                }

                if (row[BaseCurrencyID] != System.DBNull.Value)
                {
                    auec.CurrencyID = int.Parse(row[BaseCurrencyID].ToString());
                }
                if (row[OtherCurrencyID] != System.DBNull.Value)
                {
                    auec.OtherCurrencyID = int.Parse(row[OtherCurrencyID].ToString());
                }

                if (row[Multiplier] != System.DBNull.Value)
                {
                    auec.Multiplier = double.Parse(row[Multiplier].ToString());
                }
                if (row[IsShortSaleConfirmation] != System.DBNull.Value)
                {
                    auec.IsShortSaleConfirmation = int.Parse(row[IsShortSaleConfirmation].ToString());
                }
                if (row[ProvideAccountNameWithTrade] != System.DBNull.Value)
                {
                    auec.ProvideAccountNameWithTrade = int.Parse(row[ProvideAccountNameWithTrade].ToString());
                }
                if (row[ProvideIdentifierNameWithTrade] != System.DBNull.Value)
                {
                    auec.ProvideIdentifierNameWithTrade = int.Parse(row[ProvideIdentifierNameWithTrade].ToString());
                }
                if (row[IdentifierID] != System.DBNull.Value)
                {
                    auec.IdentifierID = int.Parse(row[IdentifierID].ToString());
                }

                if (row[PurchaseSecFees] != System.DBNull.Value)
                {
                    auec.PurchaseSecFees = Double.Parse(row[PurchaseSecFees].ToString());
                }
                if (row[SaleSecFees] != System.DBNull.Value)
                {
                    auec.SaleSecFees = Double.Parse(row[SaleSecFees].ToString());
                }
                if (row[PurchaseStamp] != System.DBNull.Value)
                {
                    auec.PurchaseStamp = Double.Parse(row[PurchaseStamp].ToString());
                }
                if (row[SaleStamp] != System.DBNull.Value)
                {
                    auec.SaleStamp = Double.Parse(row[SaleStamp].ToString());
                }
                if (row[PurchaseLevy] != System.DBNull.Value)
                {
                    auec.PurchaseLevy = Double.Parse(row[PurchaseLevy].ToString());
                }
                if (row[SaleLevy] != System.DBNull.Value)
                {
                    auec.SaleLevy = Double.Parse(row[SaleLevy].ToString());
                }
                if (row[SettlementDaysSell] != System.DBNull.Value)
                {
                    auec.SettlementDaysSell = int.Parse(row[SettlementDaysSell].ToString());
                }
                //set roundlot value, PRANA-11159
                if (row[RoundLot] != System.DBNull.Value)
                {
                    auec.RoundLot = Convert.ToDecimal(row[RoundLot].ToString());
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
            return auec;
        }


        /// <summary>
        /// FillAUEC is a method to fill an object of <see cref="AUEC"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="AUEC"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="AUEC"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of AUEC class. So, if the row only contains result from T_AUEC table then value of Offset would be zero ("0"), and, lets say the row contains value of AUEC as well as any other table then we have to specify the offset from where the values of AUEC starts. Note: Sequence of AUEC class whould be always same as in this method.</remarks>		 
        public static AUEC FillAUEC(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int assetID = 1 + offSet;
            int underlyingID = 2 + offSet;
            int exchangeID = 3 + offSet;
            int currencyID = 4 + offSet;
            int displayName = 5 + offSet;

            AUEC auec = new AUEC();
            try
            {
                if (row[ID] != null && row[ID] != System.DBNull.Value)
                {
                    auec.AUECID = int.Parse(row[ID].ToString());
                }
                if (row[assetID] != null && row[assetID] != System.DBNull.Value)
                {
                    auec.AssetID = int.Parse(row[assetID].ToString());
                }
                if (row[underlyingID] != null && row[underlyingID] != System.DBNull.Value)
                {
                    auec.UnderlyingID = int.Parse(row[underlyingID].ToString());
                }
                if (row[exchangeID] != null && row[exchangeID] != System.DBNull.Value)
                {
                    auec.ExchangeID = int.Parse(row[exchangeID].ToString());
                }
                if (row[currencyID] != null && row[currencyID] != System.DBNull.Value)
                {
                    auec.CurrencyID = int.Parse(row[currencyID].ToString());
                }
                if (row[displayName] != null && row[displayName] != System.DBNull.Value)
                {
                    auec.DisplayName = row[displayName].ToString();
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
            return auec;
        }

        /// <summary>
        /// Saves <see cref="AUEC"/> in the database.
        /// </summary>
        /// <param name="auec"><see cref="AUEC"/> to be saved.</param>
        /// <returns>ID of the <see cref="AUEC"/> saved(inserted/updated)</returns>
        [AutoComplete]
        public static int SaveAUEC(AUEC auec)
        {
            int result = int.MinValue;
            try
            {
                Object[] parameter = new object[4];
                parameter[0] = auec.AssetID;
                parameter[1] = auec.UnderlyingID;
                //Commented for now the following line.
                //parameter[2] = auec.AUECExchangeID; 
                //	parameter[3] = auec.CurrencyID;
                parameter[3] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_InsertAUEC", parameter).ToString());
                auec.AUECID = result;
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
        /// Gets all <see cref="AUEC"/> in <see cref="AUECs"/> collection.
        /// </summary>
        /// <returns>Object of <see cref="AUECs"/>.</returns>
        public static AUECs GetAUEC()
        {
            AUECs auecs = new AUECs();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAUECs";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //exchanges.Add(ExchangeManager.FillExchanges(row, 0)); //0 is offset.	
                        auecs.Add(FillAUEC(row, 0));
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
            return auecs;
        }

        public static AUECs GetCompanyAUEC(int CompanyID)
        {
            AUECs auecs = new AUECs();

            try
            {
                object[] parameters = new object[1];
                parameters[0] = CompanyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyAUECs", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //exchanges.Add(ExchangeManager.FillExchanges(row, 0)); //0 is offset.	
                        auecs.Add(FillAUEC(row, 0));
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
            return auecs;
        }

        public static AUECs GetUserAUEC(int companyUserID)
        {
            AUECs auecs = new AUECs();

            try
            {
                object[] parameters = new object[1];
                parameters[0] = companyUserID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUsersAUECs", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //exchanges.Add(ExchangeManager.FillExchanges(row, 0)); //0 is offset.	
                        auecs.Add(FillAUEC(row, 0));
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
            return auecs;
        }

        public static AUECs GetClientAUEC(int clientID)
        {
            AUECs auecs = new AUECs();

            try
            {
                object[] parameters = new object[1];
                parameters[0] = clientID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClientAUECs", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //exchanges.Add(ExchangeManager.FillExchanges(row, 0)); //0 is offset.	
                        auecs.Add(FillAUEC(row, 0));
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
            return auecs;
        }


        public static AUEC FillAUECAssetUnderLyingExchange(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int assetID = 1 + offSet;
            int underlyingID = 2 + offSet;
            int exchangeID = 3 + offSet;
            int displayName = 4 + offSet;

            AUEC auec = new AUEC();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    auec.AUECID = int.Parse(row[ID].ToString());
                }
                if (row[assetID] != System.DBNull.Value)
                {
                    auec.AssetID = int.Parse(row[assetID].ToString());
                }
                if (row[underlyingID] != System.DBNull.Value)
                {
                    auec.UnderlyingID = int.Parse(row[underlyingID].ToString());
                }
                if (row[exchangeID] != System.DBNull.Value)
                {
                    auec.ExchangeID = int.Parse(row[exchangeID].ToString());
                }
                if (row[displayName] != System.DBNull.Value)
                {
                    auec.DisplayName = row[displayName].ToString();
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
            return auec;
        }


        /// <summary>
        /// Gets all <see cref="AUEC"/> in <see cref="AUECs"/> collection for the combination of given assetID & underLyingID.
        /// </summary>
        /// <param name="assetID">assetID for which <see cref="AUEC"/> is required.</param>
        /// <param name="underlyingID">underLyingID for which <see cref="AUEC"/> is required.</param>
        /// <returns>Object of <see cref="AUECs"/>.</returns>
        public static AUECs GetAUEC(int assetID, int underlyingID)
        {
            AUECs auecs = new AUECs();
            try
            {
                Object[] parameter = new object[2];
                parameter[0] = (int)assetID;
                parameter[1] = (int)underlyingID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECByAssetAndUnderLying", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //exchanges.Add(ExchangeManager.FillExchanges(row, 0)); //0 is offset.	
                        auecs.Add(FillAUECAssetUnderLyingExchange(row, 0));
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
            return auecs;
        }

        /// <summary>
        /// Gets <see cref="AUEC"/> for a given auecID.
        /// </summary>
        /// <param name="auecID">AssetID for which <see cref="AUEC"/> is required.</param>
        /// <returns>Object of <see cref="AUEC"/>.</returns>
        public static AUEC GetAUEC(int auecID)
        {
            AUEC auec = null;
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = (int)auecID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auec = FillAUEC(row, 0);
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
            return auec;
        }

        public static bool DeleteAUECExchange(int auecID, int assetID, int underLyingID)
        {
            bool result = false;
            try
            {
                Object[] parameter = new object[3];
                parameter[0] = auecID;
                parameter[1] = assetID;
                parameter[2] = underLyingID;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAUEC", parameter) > 0)
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

        #region AUEC MarketFees

        /// <summary>
        /// FillAUECMarketFees is a method to fill a object of <see cref="MarketFee"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="MarketFee"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="MarketFee"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of MarketFee class. So, if the row only contains result from T_AUECMarketFee table then value of Offset would be zero ("0"), and, lets say the row contains value of MarketFee as well as any other table then we have to specify the offset from where the values of MarketFee starts. Note: Sequence of MarketFee class whould be always same as in this method.</remarks>		 
        public static MarketFee FillAUECMarketFees(object[] row, int offSet)
        {
            int auecExchangeID = 0 + offSet;
            int purchaseSecFees = 1 + offSet;
            int saleSecFees = 2 + offSet;
            int purchaseStamp = 3 + offSet;
            int saleStamp = 4 + offSet;
            int purchaseLevy = 5 + offSet;
            int saleLevy = 6 + offSet;

            MarketFee marketFees = new MarketFee();
            try
            {
                if (row[auecExchangeID] != System.DBNull.Value)
                {
                    marketFees.AUECExchangeID = int.Parse(row[auecExchangeID].ToString());
                }
                if (row[purchaseSecFees] != System.DBNull.Value)
                {
                    marketFees.PurchaseSecFees = Convert.ToDouble(row[purchaseSecFees].ToString());
                }
                if (row[saleSecFees] != System.DBNull.Value)
                {
                    marketFees.SaleSecFees = Convert.ToDouble(row[saleSecFees].ToString());
                }
                if (row[purchaseStamp] != System.DBNull.Value)
                {
                    marketFees.PurchaseStamp = Convert.ToDouble(row[purchaseStamp].ToString());
                }
                if (row[saleStamp] != System.DBNull.Value)
                {
                    marketFees.SaleStamp = Convert.ToDouble(row[saleStamp].ToString());
                }

                if (row[purchaseLevy] != System.DBNull.Value)
                {
                    marketFees.PurchaseLevy = Convert.ToDouble(row[purchaseLevy].ToString());
                }
                if (row[saleLevy] != System.DBNull.Value)
                {
                    marketFees.SaleLevy = Convert.ToDouble(row[saleLevy].ToString());
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
            return marketFees;
        }

        /// <summary>
        /// Gets <see cref="MarketFee"/> for a given auecID.
        /// </summary>
        /// <param name="auecID">auecID for which <see cref="MarketFee"/> is required.</param>
        /// <returns>Object of <see cref="MarketFee"/>.</returns>
        public static MarketFee GetMarketFees(int auecExchangeID)
        {
            MarketFee marketFees = null;

            Object[] parameter = new object[1];
            parameter[0] = (int)auecExchangeID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECMarketFees", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        marketFees = FillAUECMarketFees(row, 0);
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
            return marketFees;
        }

        /// <summary>
        /// Saves <see cref="MarketFee"/> in the database.
        /// </summary>
        /// <param name="auecID"><see cref="MarketFee"/> to be saved corresponding to the auecID passed.</param>
        /// <param name="marketFees"><see cref="MarketFee"/> to be saved.</param>
        /// <returns>ID of the <see cref="MarketFee"/> saved(inserted/updated)</returns>
        public static bool SaveAUECMarketFees(int auecExchangeID, MarketFee marketFees)
        {
            bool result = false;

            object[] parameter = new object[7];
            parameter[0] = auecExchangeID;
            parameter[1] = marketFees.PurchaseSecFees;
            parameter[2] = marketFees.SaleSecFees;
            parameter[3] = marketFees.PurchaseStamp;
            parameter[4] = marketFees.SaleStamp;
            parameter[5] = marketFees.PurchaseLevy;
            parameter[6] = marketFees.SaleLevy;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAUECMarketFees", parameter) > 0)
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

        #region OtherFee
        public static int SaveAUECOtherFees(int auecID, List<OtherFeeRule> otherFeeRuleList)
        {
            //otherFeeRuleList.RemoveAt(0);
            int result = 0;
            object[] parameter = new object[1];
            parameter[0] = auecID;
            try
            {
                string xml = XMLUtilities.SerializeToXML(otherFeeRuleList);
                result = XMLSaveManager.SaveThroughXML("P_SaveAUECOtherFeeRules", xml);
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

        public static int SaveAUECCounterPartyVenues(int auecID)
        {
            int result = int.MinValue;
            object[] parameter = new object[1];

            try
            {
                parameter = new object[1];
                parameter[0] = auecID;
                result = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCVAUECForAUEC", parameter);
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

        public static List<OtherFeeRule> GetAUECOtherFeeRules(int auecID)
        {
            List<OtherFeeRule> otherFeeRules = new List<OtherFeeRule>();

            Object[] parameter = new object[1];
            parameter[0] = (int)auecID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECOtherFeeRules", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        OtherFeeRule otherFeeRule = FillAUECOtherFeeRules(row, 0);
                        if (otherFeeRule.IsCriteriaApplied)
                        {
                            List<OtherFeesCriteria> criteriaList = GetAUECOtherFeeRulesCriteria(otherFeeRule.RuleID);
                            otherFeeRule.LongFeeRuleCriteriaList = criteriaList;
                            otherFeeRule.ShortFeeRuleCriteriaList = new List<OtherFeesCriteria>(criteriaList);
                        }
                        otherFeeRules.Add(otherFeeRule);
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
            return otherFeeRules;
        }

        public static List<OtherFeesCriteria> GetAUECOtherFeeRulesCriteria(Guid ruleID)
        {
            List<OtherFeesCriteria> otherFeeRulesCriteria = new List<OtherFeesCriteria>();

            Object[] parameter = new object[1];
            parameter[0] = ruleID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECOtherFeeCriteria", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        otherFeeRulesCriteria.Add(FillAUECOtherFeeRulesCriteria(row, 0));
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
            return otherFeeRulesCriteria;
        }

        public static OtherFeeRule FillAUECOtherFeeRules(object[] row, int offSet)
        {
            int otherFeeRuleID = 0 + offSet;
            int longFeeRate = 1 + offSet;
            int shortFeeRate = 2 + offSet;
            int longCalculationBasis = 3 + offSet;
            int shortCalculationBasis = 4 + offSet;
            int roundOffPrecision = 5 + offSet;
            int maxValue = 6 + offSet;
            int minValue = 7 + offSet;
            int auecID = 8 + offSet;
            int feeTypeID = 9 + offSet;
            int roundUpPrecision = 10 + offSet;
            int roundDownPrecision = 11 + offSet;
            int feePrecisionType = 12 + offSet;
            int isCriteriaApplied = 13 + offSet;

            OtherFeeRule otherFeeRule = new OtherFeeRule();
            try
            {
                if (row[otherFeeRuleID] != null)
                {
                    otherFeeRule.RuleID = (Guid)row[otherFeeRuleID];
                }
                if (row[longFeeRate] != null)
                {
                    otherFeeRule.LongRate = double.Parse(row[longFeeRate].ToString());
                }
                if (row[shortFeeRate] != null)
                {
                    otherFeeRule.ShortRate = double.Parse(row[shortFeeRate].ToString());
                }
                if (row[longCalculationBasis] != null)
                {
                    otherFeeRule.LongCalculationBasis = (CalculationFeeBasis)row[longCalculationBasis];
                }

                if (row[shortCalculationBasis] != null)
                {
                    otherFeeRule.ShortCalculationBasis = (CalculationFeeBasis)row[shortCalculationBasis];
                }
                if (row[roundOffPrecision] != null)
                {
                    otherFeeRule.RoundOffPrecision = Convert.ToInt16(row[roundOffPrecision].ToString());
                }
                if (row[maxValue] != null)
                {
                    otherFeeRule.MaxValue = double.Parse(row[maxValue].ToString());
                }
                if (row[minValue] != null)
                {
                    otherFeeRule.MinValue = Double.Parse(row[minValue].ToString());
                }

                if (row[auecID] != null)
                {
                    otherFeeRule.AUECID = int.Parse(row[auecID].ToString());
                }
                if (row[feeTypeID] != null)
                {
                    otherFeeRule.OtherFeeType = (OtherFeeType)row[feeTypeID];
                }
                if (row[roundUpPrecision] != null)
                {
                    otherFeeRule.RoundUpPrecision = Convert.ToInt32(row[roundUpPrecision].ToString());
                }
                if (row[roundDownPrecision] != null)
                {
                    otherFeeRule.RoundDownPrecision = Convert.ToInt32(row[roundDownPrecision].ToString());
                }
                if (row[feePrecisionType] != null)
                {
                    otherFeeRule.FeePrecisionType = (FeePrecisionType)Enum.Parse(typeof(FeePrecisionType), row[feePrecisionType].ToString());
                }
                if (row[isCriteriaApplied] != null)
                {
                    otherFeeRule.IsCriteriaApplied = Convert.ToBoolean(row[isCriteriaApplied].ToString());
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
            return otherFeeRule;
        }

        public static OtherFeesCriteria FillAUECOtherFeeRulesCriteria(object[] row, int offSet)
        {
            int otherFeesCriteriaId = 0 + offSet;
            int longValueGreaterThan = 1 + offSet;
            int longValueLessThanOrEqual = 2 + offSet;
            int longFeeRate = 3 + offSet;
            int longCalculationBasis = 4 + offSet;
            int shortValueGreaterThan = 6 + offSet;
            int shortValueLessThanOrEqual = 7 + offSet;
            int shortFeeRate = 8 + offSet;
            int shortCalculationBasis = 9 + offSet;

            OtherFeesCriteria otherFeeRule = new OtherFeesCriteria();
            try
            {
                if (row[otherFeesCriteriaId] != null)
                {
                    otherFeeRule.OtherFeesCriteriaId = Convert.ToInt32(row[otherFeesCriteriaId].ToString());
                }
                if (row[longValueGreaterThan] != null)
                {
                    otherFeeRule.LongValueGreaterThan = double.Parse(row[longValueGreaterThan].ToString());
                }
                if (row[longValueLessThanOrEqual] != null)
                {
                    otherFeeRule.LongValueLessThanOrEqual = double.Parse(row[longValueLessThanOrEqual].ToString());
                }
                if (row[longFeeRate] != null)
                {
                    otherFeeRule.LongFeeRate = double.Parse(row[longFeeRate].ToString());
                }
                if (row[longCalculationBasis] != null)
                {
                    otherFeeRule.LongCalculationBasis = Convert.ToInt32(row[longCalculationBasis].ToString());
                    otherFeeRule.LongFeeCriteriaUnit = GetRateUnitByValue(otherFeeRule.LongCalculationBasis);
                }
                if (row[shortValueGreaterThan] != null)
                {
                    otherFeeRule.ShortValueGreaterThan = double.Parse(row[shortValueGreaterThan].ToString());
                }
                if (row[shortValueLessThanOrEqual] != null)
                {
                    otherFeeRule.ShortValueLessThanOrEqual = double.Parse(row[shortValueLessThanOrEqual].ToString());
                }
                if (row[shortFeeRate] != null)
                {
                    otherFeeRule.ShortFeeRate = double.Parse(row[shortFeeRate].ToString());
                }
                if (row[shortCalculationBasis] != null)
                {
                    otherFeeRule.ShortCalculationBasis = Convert.ToInt32(row[shortCalculationBasis].ToString());
                    otherFeeRule.ShortFeeCriteriaUnit = GetRateUnitByValue(otherFeeRule.ShortCalculationBasis);
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
            return otherFeeRule;
        }


        #endregion

        private static string GetRateUnitByValue(int selectedValue)
        {
            CalculationBasis criteria = (CalculationBasis)selectedValue;
            switch (criteria)
            {
                case CalculationBasis.Shares:
                    return "Per Share";
                case CalculationBasis.Notional:
                    return "Basis Points";
                case CalculationBasis.Contracts:
                    return "Per Contract";
                case CalculationBasis.AvgPrice:
                    return "Per Share/Contract";
                case CalculationBasis.FlatAmount:
                    return "Per Trade/Taxlot";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region AUEC Exchange

        /// <summary>
        /// FillExchanges is a method to fill a object of <see cref="Exchange"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Exchange"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Exchange"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Exchange class. So, if the row only contains result from T_Exchange table then value of Offset would be zero ("0"), and, lets say the row contains value of Exchange as well as any other table then we have to specify the offset from where the values of Exchange starts. Note: Sequence of Exchange class whould be always same as in this method.</remarks> 
        public static Exchange FillExchanges(object[] row, int offSet)
        {
            int AUECID = 0 + offSet; //This is actually the AUECExchangeID.
            int EXCHANGEID = 1 + offSet;
            int FULL_NAME = 2 + offSet;
            int DISPLAY_NAME = 3 + offSet;
            //			int SYMBOL_CONVENTION_ID = 4 + offSet;
            //			int EXCHANGE_IDENTIFIER = 5 + offSet;
            int TIME_ZONE = 4 + offSet;
            int REGULAR_TRADING_STARTTIME = 5 + offSet;
            int REGULAR_TRADING_ENDTIME = 6 + offSet;
            int PRE_MARKETTRADING_STARTTIME = 7 + offSet;
            int PRE_MARKETTRADING_ENDTIME = 8 + offSet;
            int LUNCH_TIME_STARTTIME = 9 + offSet;
            int LUNCH_TIME_ENDTIME = 10 + offSet;
            int POST_MARKETTRADING_STARTTIME = 11 + offSet;
            int POST_MARKETTRADING_ENDTIME = 12 + offSet;
            int COUNTRY = 13 + offSet;
            int STATE = 14 + offSet;

            int UNIT = 15 + offSet;
            int SETTLEMENT_DAYS = 16 + offSet;
            int DAY_LIGHT_SAVING = 17 + offSet;

            int PRE_MARKETTIME_CHECK = 18 + offSet;
            int POST_MARKETIME_CHECK = 19 + offSet;
            int REGULAR_TIME_CHECK = 20 + offSet;
            int LUNCH_TIME_CHECK = 21 + offSet;
            int CURRENCY_CONVERSION = 22 + offSet;
            int COUNTRY_FLAG_ID = 23 + offSet;
            int EXCHANGE_LOGO_ID = 24 + offSet;
            int TIMEZONE_OFFSET = 25 + offSet;

            Exchange exchange = new Exchange();
            try
            {
                if (row[AUECID] != System.DBNull.Value)
                {
                    exchange.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (row[EXCHANGEID] != System.DBNull.Value)
                {
                    exchange.ExchangeID = int.Parse(row[EXCHANGEID].ToString());
                }
                if (row[FULL_NAME] != System.DBNull.Value)
                {
                    exchange.Name = row[FULL_NAME].ToString();
                }
                if (row[DISPLAY_NAME] != System.DBNull.Value)
                {
                    exchange.DisplayName = row[DISPLAY_NAME].ToString();
                }
                //				if(row[SYMBOL_CONVENTION_ID] != System.DBNull.Value)
                //				{			
                //					exchange.SymbolConventionID = int.Parse(row[SYMBOL_CONVENTION_ID].ToString());
                //				}
                //				if(row[EXCHANGE_IDENTIFIER] != System.DBNull.Value)
                //				{			
                //					exchange.ExchangeIdentifier = row[EXCHANGE_IDENTIFIER].ToString();
                //				}

                if (row[TIME_ZONE] != System.DBNull.Value)
                {
                    exchange.TimeZone = row[TIME_ZONE].ToString();
                }
                if (row[REGULAR_TRADING_STARTTIME] != System.DBNull.Value)
                {
                    exchange.RegularTradingStartTime = DateTime.Parse(row[REGULAR_TRADING_STARTTIME].ToString());
                }
                if (row[REGULAR_TRADING_ENDTIME] != System.DBNull.Value)
                {
                    exchange.RegularTradingEndTime = DateTime.Parse(row[REGULAR_TRADING_ENDTIME].ToString());
                }
                if (row[PRE_MARKETTRADING_STARTTIME] != System.DBNull.Value)
                {
                    exchange.PreMarketTradingStartTime = DateTime.Parse(row[PRE_MARKETTRADING_STARTTIME].ToString());
                }
                if (row[PRE_MARKETTRADING_ENDTIME] != System.DBNull.Value)
                {
                    exchange.PreMarketTradingEndTime = DateTime.Parse(row[PRE_MARKETTRADING_ENDTIME].ToString());
                }
                if (row[LUNCH_TIME_STARTTIME] != System.DBNull.Value)
                {
                    exchange.LunchTimeStartTime = DateTime.Parse(row[LUNCH_TIME_STARTTIME].ToString());
                }

                if (row[LUNCH_TIME_ENDTIME] != System.DBNull.Value)
                {
                    exchange.LunchTimeEndTime = DateTime.Parse(row[LUNCH_TIME_ENDTIME].ToString());
                }
                if (row[POST_MARKETTRADING_STARTTIME] != System.DBNull.Value)
                {
                    exchange.PostMarketTradingStartTime = DateTime.Parse(row[POST_MARKETTRADING_STARTTIME].ToString());
                }
                if (row[POST_MARKETTRADING_ENDTIME] != System.DBNull.Value)
                {
                    exchange.PostMarketTradingEndTime = DateTime.Parse(row[POST_MARKETTRADING_ENDTIME].ToString());
                }
                if (row[COUNTRY] != System.DBNull.Value)
                {
                    exchange.Country = int.Parse(row[COUNTRY].ToString());
                }
                if (row[STATE] != System.DBNull.Value)
                {
                    exchange.StateID = int.Parse(row[STATE].ToString());
                }

                if (row[UNIT] != System.DBNull.Value)
                {
                    exchange.Unit = int.Parse(row[UNIT].ToString());
                }
                if (row[SETTLEMENT_DAYS] != System.DBNull.Value)
                {
                    exchange.SettlementDays = int.Parse(row[SETTLEMENT_DAYS].ToString());
                }
                if (row[DAY_LIGHT_SAVING] != System.DBNull.Value)
                {
                    exchange.DayLightSaving = row[DAY_LIGHT_SAVING].ToString();
                }
                if (row[PRE_MARKETTIME_CHECK] != System.DBNull.Value)
                {
                    exchange.PreMarketCheck = int.Parse(row[PRE_MARKETTIME_CHECK].ToString());
                }
                if (row[POST_MARKETIME_CHECK] != System.DBNull.Value)
                {
                    exchange.PostMarketCheck = int.Parse(row[POST_MARKETIME_CHECK].ToString());
                }
                if (row[REGULAR_TIME_CHECK] != System.DBNull.Value)
                {
                    exchange.RegularTimeCheck = int.Parse(row[REGULAR_TIME_CHECK].ToString());
                }
                if (row[LUNCH_TIME_CHECK] != System.DBNull.Value)
                {
                    exchange.LunchTimeCheck = int.Parse(row[LUNCH_TIME_CHECK].ToString());
                }
                if (row[CURRENCY_CONVERSION] != System.DBNull.Value)
                {
                    exchange.CurrencyConversion = int.Parse(row[CURRENCY_CONVERSION].ToString());
                }
                if (row[COUNTRY_FLAG_ID] != System.DBNull.Value)
                {
                    exchange.CountryFlagID = int.Parse(row[COUNTRY_FLAG_ID].ToString());
                }
                if (row[EXCHANGE_LOGO_ID] != System.DBNull.Value)
                {
                    exchange.LogoID = int.Parse(row[EXCHANGE_LOGO_ID].ToString());
                }
                if (row[TIMEZONE_OFFSET] != System.DBNull.Value)
                {
                    exchange.TimeZoneOffSet = Double.Parse(row[TIMEZONE_OFFSET].ToString());
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



        public static int SaveAUECDetails(AUEC auecDetails)
        {
            int result = int.MinValue;

            object[] parameter = new object[47];
            parameter[0] = auecDetails.AUECID;
            parameter[1] = auecDetails.ExchangeID;
            parameter[2] = auecDetails.FullName;
            parameter[3] = auecDetails.DisplayName;
            parameter[4] = auecDetails.UnitID;
            parameter[5] = auecDetails.TimeZone;
            parameter[6] = auecDetails.PreMarketTradingStartTime;
            parameter[7] = auecDetails.PreMarketTradingEndTime;
            parameter[8] = auecDetails.LunchTimeStartTime;
            parameter[9] = auecDetails.LunchTimeEndTime;
            parameter[10] = auecDetails.RegularTradingStartTime;
            parameter[11] = auecDetails.RegularTradingEndTime;
            parameter[12] = auecDetails.PostMarketTradingStartTime;
            parameter[13] = auecDetails.PostMarketTradingEndTime;
            parameter[14] = auecDetails.SettlementDaysBuy;
            parameter[15] = auecDetails.DayLightSaving;
            parameter[16] = auecDetails.Country;
            parameter[17] = auecDetails.StateID;

            parameter[18] = auecDetails.PreMarketCheck;
            parameter[19] = auecDetails.PostMarketCheck;
            parameter[20] = auecDetails.RegularTimeCheck;
            parameter[21] = auecDetails.LunchTimeCheck;
            parameter[22] = auecDetails.CurrencyConversion;
            parameter[23] = auecDetails.CountryFlagID;
            parameter[24] = auecDetails.LogoID;
            parameter[25] = auecDetails.TimeZoneOffSet;

            parameter[26] = auecDetails.SymbolConventionID;
            parameter[27] = auecDetails.ExchangeIdentifier;
            parameter[28] = auecDetails.MarketDataProviderExchangeIdentifier;

            parameter[29] = auecDetails.CurrencyID;
            parameter[30] = auecDetails.OtherCurrencyID;
            parameter[31] = auecDetails.Multiplier;
            parameter[32] = auecDetails.IsShortSaleConfirmation;
            parameter[33] = auecDetails.ProvideAccountNameWithTrade;
            parameter[34] = auecDetails.ProvideIdentifierNameWithTrade;
            parameter[35] = auecDetails.IdentifierID;
            parameter[36] = auecDetails.AssetID;
            parameter[37] = auecDetails.UnderlyingID;

            parameter[38] = auecDetails.PurchaseSecFees;
            parameter[39] = auecDetails.SaleSecFees;
            parameter[40] = auecDetails.PurchaseStamp;
            parameter[41] = auecDetails.SaleStamp;
            parameter[42] = auecDetails.PurchaseLevy;
            parameter[43] = auecDetails.SaleLevy;
            parameter[44] = auecDetails.SettlementDaysSell;
            //save roundlot value in parameter
            parameter[45] = auecDetails.RoundLot;
            parameter[46] = result;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveAUECDetails", parameter).ToString());
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


        public static void SaveAUECDetailsForVenue(AUEC auecDetails)
        {
            //int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = auecDetails.DisplayName;
            parameter[1] = 1;// Venue Type Id it is 1 bydefault           
            parameter[2] = auecDetails.FullName;
            parameter[3] = auecDetails.ExchangeID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveVenueDetailsFor", parameter);
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
        /// Saves <see cref="Exchange"/> in the database.
        /// </summary>
        /// <param name="auecID"><see cref="Exchange"/> to be saved corresponding to the auecID passed.</param>
        /// <param name="exchange"><see cref="Exchange"/> to be saved.</param>
        /// <returns>the AUECExchangeID which further is used to save the details along with AUEC SymbolConvention</returns>
        public static int SaveAUECExchange(int auecExchangeID, Exchange exchange)
        {
            int result = int.MinValue;

            object[] parameter = new object[28];
            parameter[0] = auecExchangeID;
            parameter[1] = exchange.ExchangeID;
            parameter[2] = exchange.Name;
            parameter[3] = exchange.DisplayName;
            //			parameter[4] = exchange.SymbolConventionID;
            //			parameter[5] = exchange.ExchangeIdentifier;
            parameter[4] = exchange.Unit;
            parameter[5] = exchange.TimeZone;
            parameter[6] = exchange.PreMarketTradingStartTime;
            parameter[7] = exchange.PreMarketTradingEndTime;
            parameter[8] = exchange.LunchTimeStartTime;
            parameter[9] = exchange.LunchTimeEndTime;
            parameter[10] = exchange.RegularTradingStartTime;
            parameter[11] = exchange.RegularTradingEndTime;
            parameter[12] = exchange.PostMarketTradingStartTime;
            parameter[13] = exchange.PostMarketTradingEndTime;
            parameter[14] = exchange.SettlementDays;
            parameter[15] = exchange.DayLightSaving;
            parameter[16] = exchange.Country;
            parameter[17] = exchange.StateID;

            parameter[18] = exchange.PreMarketCheck;
            parameter[19] = exchange.PostMarketCheck;
            parameter[20] = exchange.RegularTimeCheck;
            parameter[21] = exchange.LunchTimeCheck;
            parameter[22] = exchange.CurrencyConversion;
            parameter[23] = exchange.CountryFlagID;
            parameter[24] = exchange.LogoID;
            parameter[25] = exchange.ExchangeIdentifier;
            parameter[26] = exchange.TimeZoneOffSet;
            parameter[27] = result;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_InsertAUECExchange", parameter).ToString());
                //				if(db.ExecuteNonQuery("P_InsertAUECExchange", parameter) > 0)
                //				{
                //					result = true;
                //				}
                //				else
                //				{
                //					result = false;
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
        /// This method saves the AUECSymbolConvention details
        /// </summary>
        /// <param name="auecExchangeID"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public static int SaveAUECSymbolConvention(int auecExchangeID, Exchange exchange)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            parameter[0] = auecExchangeID;
            parameter[1] = exchange.SymbolConventionID;
            parameter[2] = exchange.ExchangeIdentifier;
            parameter[3] = result;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_InsertAUECSymbolConvention", parameter).ToString());
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


        public static AUEC GetAUECDetails(int auecID)
        {
            AUEC auecDetails = new AUEC();
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = (int)auecID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auecDetails = FillAUECDetails(row, 0);
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
            return auecDetails;
        }

        public static Exchange FillAUECExchangeSymbolConvention(object[] row, int offSet)
        {
            int AUECEXCHANGEID = 0 + offSet;
            int SYMBOLCONVENTIONID = 1 + offSet;
            int EXCHANGEIDENTIFIER = 2 + offSet;

            Exchange exchange = new Exchange();
            try
            {
                if (row[AUECEXCHANGEID] != System.DBNull.Value)
                {
                    exchange.AUECID = int.Parse(row[AUECEXCHANGEID].ToString());
                }
                if (row[SYMBOLCONVENTIONID] != System.DBNull.Value)
                {
                    exchange.SymbolConventionID = int.Parse(row[SYMBOLCONVENTIONID].ToString());
                }
                if (row[EXCHANGEIDENTIFIER] != System.DBNull.Value)
                {
                    exchange.ExchangeIdentifier = row[EXCHANGEIDENTIFIER].ToString();
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

        /// <summary>
        /// This is a special method which brings all the exchanges from the table except the one supplied to it.
        /// </summary>
        /// <param name="_auecExchangeID">This parameter is used to fetch the records from database.</param>
        /// <returns>Object of exchanges a collection of exchange object.</returns>
        public static Exchanges GetAUECExchanges(int auecExchangeID)
        {
            Exchanges auecExchanges = new Exchanges();
            try
            {
                Object[] parameter = new object[1];
                parameter[0] = (int)auecExchangeID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECExchangesByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auecExchanges.Add(FillExchanges(row, 0));
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
            return auecExchanges;
        }



        #endregion

        #region AUEC Compliance

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auecID"></param>
        /// <param name="orderTypeID"></param>
        /// <returns></returns>
        public static bool SaveAUECOrderType(int auecID, OrderTypes orderTypes)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = auecID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAUECOrderTypes", parameter).ToString();

                foreach (OrderType orderType in orderTypes)
                {
                    parameter = new object[2];
                    parameter[0] = auecID;
                    parameter[1] = orderType.OrderTypesID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAUECOrderType", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        #endregion

        #region Currencies, CurrencyTypes & Units

        /// <summary>
        /// Gets all <see cref="Currency"/> in <see cref="Currencies"/>
        /// </summary>
        /// <returns>Object of <see cref="Currencies"/>.</returns>
        public static Currencies GetCurrencies()
        {
            Currencies currencies = new Currencies();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCurrencies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencies.Add(FillCurrencies(row, 0));
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
            return currencies;
        }


        /// <summary>
        /// FillUnits is a method to fill a object of <see cref="Unit"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Unit"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Unit"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Unit class. So, if the row only contains result from T_Units table then value of Offset would be zero ("0"), and, lets say the row contains value of Units as well as any other table then we have to specify the offset from where the values of Unit starts. Note: Sequence of Unit class whould be always same as in this method.</remarks>		
        public static Unit FillUnits(object[] row, int offSet)
        {
            int UNIT_ID = 0 + offSet;
            int UNIT_NAME = 1 + offSet;

            Unit unit = new Unit();
            try
            {
                if (row[UNIT_ID] != null)
                {
                    unit.UnitID = int.Parse(row[UNIT_ID].ToString());
                }
                if (row[UNIT_NAME] != null)
                {
                    unit.UnitName = row[UNIT_NAME].ToString();
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

            return unit;
        }

        /// <summary>
        /// Gets all <see cref="Unit"/> in <see cref="Units"/> collection.
        /// </summary>
        /// <returns>Object of <see cref="Units"/>.</returns>
        public static Units GetUnits()
        {
            Units units = new Units();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllUnits";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        units.Add(FillUnits(row, 0));
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
            return units;
        }

        public static int SaveUnit(Unit unit)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = unit.UnitID;
            parameter[1] = unit.UnitName;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUnit", parameter).ToString());
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

        public static bool DeleteUnit(int unitID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = unitID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUnit", parameter) > 0)
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

        public static Currency GetCurrency(int currencyID)
        {
            //TODO: Write SP of	Get Currency
            Currency currency = new Currency();

            object[] parameter = new object[1];
            parameter[0] = currencyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCurrency", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currency = FillCurrencies(row, 0);
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
            return currency;
        }


        public static Currency FillCurrencies(object[] row, int offSet)
        {
            int CURRENCY_ID = 0 + offSet;
            int CURRENCY_NAME = 1 + offSet;
            int CURRENCY_SYMBOL = 2 + offSet;

            Currency currency = new Currency();
            try
            {
                if (row[CURRENCY_ID] != null)
                {
                    currency.CurencyID = int.Parse(row[CURRENCY_ID].ToString());
                }
                if (row[CURRENCY_NAME] != null)
                {
                    currency.CurrencyName = row[CURRENCY_NAME].ToString();
                }
                if (row[CURRENCY_SYMBOL] != null)
                {
                    currency.CurrencySymbol = row[CURRENCY_SYMBOL].ToString();
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

        public static bool SaveCurrency(Currencies currencies)
        {
            bool result = false;
            try
            {
                foreach (Currency currency in currencies)
                {

                    object[] parameter = new object[3];
                    parameter[0] = currency.CurencyID;
                    parameter[1] = currency.CurrencyName;
                    parameter[2] = currency.CurrencySymbol;

                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertRowCurrency", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        public static int SaveCurrency(Currency currency)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = currency.CurencyID;
                parameter[1] = currency.CurrencyName;
                parameter[2] = currency.CurrencySymbol;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_InsertRowCurrency", parameter).ToString());
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

        public static void SaveSMCurrencyDetails(Currency currency, int currencyID)
        {
            try
            {
                object[] parameter = new object[3];
                parameter[0] = currencyID;
                parameter[1] = currency.CurrencyName;
                parameter[2] = currency.CurrencySymbol;

                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSMCurrencyDetails", parameter, ApplicationConstants.SMConnectionString);
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

        public static bool DeleteCurrency(int id)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = id;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCurrency", parameter) > 0)
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


        public static CurrencyType FillCurrencyTypes(object[] row, int offSet)
        {
            int CURRENCY_TYPE_ID = 0 + offSet;
            int CURRENCY_TYPE = 1 + offSet;

            CurrencyType currencyType = new CurrencyType();
            try
            {
                if (row[CURRENCY_TYPE_ID] != null)
                {
                    currencyType.CurrencyTypeID = int.Parse(row[CURRENCY_TYPE_ID].ToString());
                }
                if (row[CURRENCY_TYPE] != null)
                {
                    currencyType.Type = row[CURRENCY_TYPE].ToString();
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

            return currencyType;
        }

        public static CurrencyTypes GetCurrencyTypes()
        {
            CurrencyTypes currencyTypes = new CurrencyTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCurrencyTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyTypes.Add(FillCurrencyTypes(row, 0));
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
            return currencyTypes;
        }

        public static int SaveCurrencyType(CurrencyType currencyType)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = currencyType.CurrencyTypeID;
                parameter[1] = currencyType.Type;
                parameter[2] = result;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCurrencyType", parameter).ToString());

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

        public static bool DeleteCurrencyType(int currencyTypeID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = currencyTypeID;
                //TODO: Modify the delete currency type procedure.
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCurrencyType", parameter) > 0)
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

        #region Holidays

        public static Holiday FillAUECHolidays(object[] row, int offSet)
        {
            int holidayID = 0 + offSet;
            int auecExchangeID = 1 + offSet;
            int description = 2 + offSet;
            int date = 3 + offSet;
            // Added by bhavana on April 9, 2014
            int marketOff = 4 + offSet;
            int settlementOff = 5 + offSet;

            Holiday holiday = new Holiday();
            try
            {
                if (row[holidayID] != null)
                {
                    holiday.HolidayID = int.Parse(row[holidayID].ToString());
                }
                if (row[auecExchangeID] != null)
                {
                    holiday.AUECExchangeID = int.Parse(row[auecExchangeID].ToString());
                }
                if (row[description] != null)
                {
                    holiday.Description = row[description].ToString();
                }
                if (row[date] != null)
                {
                    holiday.Date = DateTime.Parse(row[date].ToString());
                }
                if (row[marketOff] != null && row[marketOff].ToString() != "")
                {
                    holiday.MarketOff = Convert.ToBoolean(row[marketOff].ToString());
                }
                if (row[settlementOff] != null && row[settlementOff].ToString() != "")
                {
                    holiday.SettlementOff = Convert.ToBoolean(row[settlementOff].ToString());
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
            return holiday;
        }

        public static Holidays GetHolidays(int auecID, int exchangeID, int year, int choice)
        {
            Holidays holidays = new Holidays();

            Object[] parameter = new object[4];
            parameter[0] = auecID;
            parameter[1] = exchangeID;
            if (choice == 0)
            {
                parameter[2] = year;
                parameter[3] = 0;
            }
            if (choice == 1)
            {
                parameter[2] = int.MinValue;
                parameter[3] = 1;
            }

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECHolidays", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        holidays.Add(FillAUECHolidays(row, 0));
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
            return holidays;
        }

        public static bool SaveAUECHolidays(Holidays holidays, int auecID)
        {
            //TODO: Write SP
            bool result = false;
            //			DateTime dt = new DateTime();
            //			dt = holiday.Date.ToShortDateString();
            //			dt.ToShortDateString();
            foreach (Holiday holiday in holidays)
            {
                object[] parameter = new object[6];
                parameter[0] = auecID;
                parameter[1] = holiday.HolidayID;
                parameter[2] = holiday.Date.ToShortDateString();
                parameter[3] = holiday.Description;

                // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                parameter[4] = holiday.MarketOff;
                parameter[5] = holiday.SettlementOff;

                try
                {
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAUECHolidays", parameter) > 0)
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
            }
            return result;
        }

        public static bool SaveAUECHolidaysExchangeDefault(Holidays holidays, int auecID)
        {
            bool result = false;
            object[] parameter = new object[1];
            parameter[0] = auecID;
            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAUECHolidays", parameter) > 0)
                {
                    result = true;
                }
                //				if(result == true)
                //				{
                foreach (Holiday holiday in holidays)
                {
                    parameter = new object[4];
                    parameter[0] = auecID;
                    parameter[1] = holiday.HolidayID;
                    parameter[2] = holiday.Date.ToShortDateString();
                    parameter[3] = holiday.Description;

                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAUECHolidaysExchangeDefault", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
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

        public static bool SaveAUECHolidays(Holiday holiday, int auecID)
        {
            //TODO: Write SP
            bool result = false;

            object[] parameter = new object[6];
            parameter[0] = auecID;
            parameter[1] = holiday.HolidayID;
            parameter[2] = holiday.Date;
            parameter[3] = holiday.Description;

            // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
            parameter[4] = holiday.MarketOff;
            parameter[5] = holiday.SettlementOff;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAUECHolidays", parameter) > 0)
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

        public static bool DeleteHoliday(int holidayID)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = holidayID;
            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteHoliday", parameter) > 0)
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

        public static bool CopyAUECExchangeHolidays(Exchanges exchanges, int sourceAUECID)
        {
            bool result = false;
            foreach (Exchange exchange in exchanges)
            {
                object[] parameter = new object[2];
                parameter[0] = exchange.AUECID;
                parameter[1] = sourceAUECID;

                try
                {
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CopyAUECExchangeHolidays", parameter) > 0)
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
            }
            return result;
        }


        #region WeeklyHolidays

        public static WeeklyHoliday FillAllWeeklyHolidays(object[] row, int offSet)
        {
            int weeklyHolidayID = 0 + offSet;
            int weeklyHolidayName = 1 + offSet;

            WeeklyHoliday weeklyHoliday = new WeeklyHoliday();
            try
            {
                if (row[weeklyHolidayID] != null)
                {
                    weeklyHoliday.WeeklyHolidayID = int.Parse(row[weeklyHolidayID].ToString());
                }
                if (row[weeklyHolidayName] != null)
                {
                    weeklyHoliday.WeeklyHolidayName = row[weeklyHolidayName].ToString();
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
            return weeklyHoliday;
        }

        public static List<Prana.BusinessObjects.WeeklyHoliday> GetAllWeeklyHolidaysCollection()
        {
            //WeeklyHolidaysCollection weeklyHolidaysCollection = new WeeklyHolidaysCollection();
            List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection = new List<Prana.BusinessObjects.WeeklyHoliday>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllWeeklyHolidays";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        weeklyHolidaysCollection.Add(FillAllWeeklyHolidays(row, 0));
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
            return weeklyHolidaysCollection;
        }

        //public static WeeklyHolidays FillAUECWeeklyHolidays(object[] row, int offSet)
        //{
        //    int weeklyHolidayID = 0 + offSet;
        //    int weeklyHolidayName = 1 + offSet;

        //    WeeklyHolidays weeklyHoliday = new WeeklyHolidays();
        //    try
        //    {
        //        if (row[weeklyHolidayID] != null)
        //        {
        //            weeklyHoliday.WeeklyHolidayID = int.Parse(row[weeklyHolidayID].ToString());
        //        }
        //        if (row[weeklyHolidayName] != null)
        //        {
        //            weeklyHoliday.WeeklyHolidayName = row[weeklyHolidayName].ToString();
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //    return weeklyHoliday;
        //}

        public static List<Prana.BusinessObjects.WeeklyHoliday> GetAUECWeeklyHolidaysCollection(int auecID)
        {
            //WeeklyHolidaysCollection weeklyHolidaysCollection = new WeeklyHolidaysCollection();
            List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection = new List<Prana.BusinessObjects.WeeklyHoliday>();

            Object[] parameter = new object[1];
            parameter[0] = (int)auecID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAUECWeeklyHolidays", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        weeklyHolidaysCollection.Add(FillAllWeeklyHolidays(row, 0));
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
            return weeklyHolidaysCollection;
        }

        public static int SaveAUECWeeklyHolidays(List<Prana.BusinessObjects.WeeklyHoliday> weeklyHolidaysCollection, int auecID)
        {
            int result = 0;
            object[] parameter = new object[1];
            parameter[0] = auecID;
            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAUECWeeklyHolidays", parameter) > 0)
                {
                    result = 1;
                }

                string xml = XMLUtilities.SerializeToXML(weeklyHolidaysCollection);
                result = XMLSaveManager.SaveThroughXML("P_SaveAUECWeeklyHolidays", xml);

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

        #endregion


        #endregion

        #region Symbols

        public static Symbol FillSymbols(object[] row, int offSet)
        {
            int symbolID = 0 + offSet;
            int companySymbol = 1 + offSet;
            int companyName = 2 + offSet;


            Symbol symbol = new Symbol();
            try
            {
                if (row[symbolID] != null)
                {
                    symbol.SymbolID = int.Parse(row[symbolID].ToString());
                }
                if (row[companySymbol] != null)
                {
                    symbol.CompanySymbol = row[companySymbol].ToString();
                }
                if (row[companyName] != null)
                {
                    symbol.Company = row[companyName].ToString();
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
            return symbol;
        }

        public static Symbols GetSymbols()
        {
            Symbols symbols = new Symbols();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetSymbols";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbols.Add(FillSymbols(row, 0));
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
            return symbols;
        }


        #endregion

        #region Identifiers
        public static Identifier FillIdentifiers(object[] row, int offSet)
        {
            int identifierID = 0 + offSet;
            int identifierName = 1 + offSet;

            Identifier identifier = new Identifier();
            try
            {
                if (row[identifierID] != null)
                {
                    identifier.IdentifierID = int.Parse(row[identifierID].ToString());
                }
                if (row[identifierName] != null)
                {
                    identifier.IdentifierName = row[identifierName].ToString();
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
            return identifier;
        }

        public static Identifier GetIdentifier(int identiferID)
        {
            Identifier identifier = new Identifier();

            object[] parameter = new object[1];
            parameter[0] = identiferID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        identifier = FillIdentifiers(row, 0);
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
            return identifier;
        }

        public static Identifiers GetIdentifiers()
        {
            Identifiers identifiers = new Identifiers();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllIdentifiers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        identifiers.Add(FillIdentifiers(row, 0));
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
            return identifiers;
        }

        public static int SaveIdentifier(Identifier identifier)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = identifier.IdentifierID;
            parameter[1] = identifier.IdentifierName;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveIdentifier", parameter).ToString());
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

        public static bool DeleteIdentifier(int identifierID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = identifierID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteIdentifier", parameter) > 0)
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

        #region Calendars

        public static Calendars GetCalendar()
        {
            Calendars calendars = new Calendars();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCalendar";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        calendars.Add(FillCalendar(row, 0));
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
            #endregion
            return calendars;
        }

        public static string GetCalendar(int auecID, int year)
        {
            string calendarName = string.Empty;

            object[] parameter = new object[2];
            parameter[0] = auecID;
            parameter[1] = year;
            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteScalar("P_GetAuecCalendar", parameter) != null)
                {
                    calendarName = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetAuecCalendar", parameter).ToString();
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
            return calendarName;

        }


        private static Calendar FillCalendar(object[] row, int offset)
        {
            int id = 0 + offset;
            int name = 1 + offset;
            int year = 2 + offset;

            Calendar calendar = new Calendar();
            try
            {
                if (int.Parse(row[id].ToString()) != int.MinValue)
                {
                    calendar.CalendarID = int.Parse(row[id].ToString());
                }
                if (row[name].ToString() != string.Empty)
                {
                    calendar.CalendarName = row[name].ToString();
                }
                if (int.Parse(row[year].ToString()) != int.MinValue)
                {
                    calendar.CalendarYear = int.Parse(row[year].ToString());
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
            return calendar;

        }

        public static AUECs auecAssociated(string name, int year)
        {
            AUECs auecs = new AUECs();
            object[] parameter = new object[2];
            parameter[0] = name;
            parameter[1] = year;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCalendarAUEC", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        auecs.Add(FillAUEC(row, 0));
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
            return auecs;
        }

        public static Holidays GetCalendarHolidays(int ID)
        {
            Holidays holidays = new Holidays();
            object[] parameter = new object[1];
            parameter[0] = ID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCalendarHolidays", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        holidays.Add(FillHoliday(row, 0));
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
            return holidays;
        }

        private static Holiday FillHoliday(object[] row, int offset)
        {
            int date = 0 + offset;
            int description = 1 + offset;
            int marketOff = 2 + offset;
            int settlementOff = 3 + offset;

            Holiday holiday = new Holiday();
            if (Convert.ToDateTime(row[date].ToString()) != Prana.BusinessObjects.DateTimeConstants.MinValue)
            {
                holiday.Date = Convert.ToDateTime(row[date].ToString());
            }
            if (row[description].ToString() != string.Empty)
            {
                holiday.Description = row[description].ToString();
            }

            // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
            if (Convert.ToBoolean(row[marketOff].ToString()) != false)
            {
                holiday.MarketOff = true;
            }
            if (Convert.ToBoolean(row[settlementOff].ToString()) != false)
            {
                holiday.SettlementOff = true;
            }
            return holiday;

        }

        public static void UpdateCalendarHolidays(Holidays holidays, int ID)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = ID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCalendarHolidays", parameter);


                foreach (Holiday holiday in holidays)
                {
                    object[] param = new object[6];
                    param[0] = int.MinValue;
                    param[1] = holiday.Date;
                    param[2] = holiday.Description;
                    param[3] = ID;
                    // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                    param[4] = holiday.MarketOff;
                    param[5] = holiday.SettlementOff;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCalendarHolidays", param);
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
        }

        public static bool UpdateAuecHolidays(Holidays holidays, int year, AUECs auecs)
        {
            bool result = false;
            try
            {
                foreach (AUEC auec in auecs)
                {
                    object[] parameter = new object[2];
                    parameter[0] = year;
                    parameter[1] = auec.AUECID;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAuecHolidays", parameter);
                }

                foreach (AUEC auec in auecs)
                {
                    foreach (Holiday holiday in holidays)
                    {
                        object[] param = new object[6];
                        param[0] = auec.AUECID;
                        param[1] = int.MinValue;
                        param[2] = holiday.Date;
                        param[3] = holiday.Description;
                        // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                        param[4] = holiday.MarketOff;
                        param[5] = holiday.SettlementOff;
                        if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAuecHolidays", param) > 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
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

            return result;

        }

        public static void SaveCalendarHolidays(Holidays holidays)
        {
            try
            {
                foreach (Holiday holiday in holidays)
                {
                    object[] param = new object[6];
                    param[0] = int.MinValue;
                    param[1] = holiday.Date;
                    param[2] = holiday.Description;
                    param[3] = int.MinValue;
                    // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                    param[4] = holiday.MarketOff;
                    param[5] = holiday.SettlementOff;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCalendarHolidays", param);
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
        }

        public static bool DeleteCalendar(string calendarName, int year, AUECs auecs)
        {
            bool result = false;
            try
            {
                if (auecs.Count != 0)
                {
                    foreach (AUEC auec in auecs)
                    {
                        object[] parameter = new object[3];
                        parameter[0] = calendarName;
                        parameter[1] = year;
                        parameter[2] = auec.AUECID;
                        if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCalendar", parameter) > 0)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    object[] parameter = new object[3];
                    parameter[0] = calendarName;
                    parameter[1] = year;
                    parameter[2] = int.MinValue;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCalendar", parameter) > 0)
                    {
                        result = true;
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
            return result;

        }

        public static bool SaveCalendar(Calendar cal)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[3];
                if (cal.CalendarID == int.MinValue)
                {
                    parameter[0] = int.MinValue;
                }
                else
                {
                    parameter[0] = cal.CalendarID;
                }
                parameter[1] = cal.CalendarName;
                parameter[2] = cal.CalendarYear;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCalendar", parameter) > 0)
                {
                    result = true;
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

        public static bool SaveCalendarAUEC(string name, int year, int resultantAUECID)
        {

            bool result = false;

            try
            {
                object[] parameter = new object[3];
                parameter[0] = name;
                parameter[1] = year;
                parameter[2] = resultantAUECID;

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAUECCalendar", parameter) > 0)
                {
                    result = true;
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
        #endregion

        #region AssetWisePermissions

        public static bool SaveAssetSide(int assetID, Sides sides)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = assetID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAssetSides", parameter);

                foreach (Side side in sides)
                {
                    parameter = new object[2];
                    parameter[0] = assetID;
                    parameter[1] = side.SideID;
                    result = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertAssetSide", parameter) > 0;
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

        public static Sides GetAssetSides(int assetID)
        {
            Sides sides = new Sides();

            object[] parameter = new object[1];
            parameter[0] = assetID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAssetSides", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    sides.Add(FillAssetSide(row, 0));
                }
            }
            return sides;
        }

        public static Side FillAssetSide(object[] row, int offSet)
        {
            int assetID = 0 + offSet;
            int sideid = 1 + offSet;

            Side side = new Side();
            try
            {
                if (row[assetID] != null)
                {
                    side.AssetID = int.Parse(row[assetID].ToString());
                }
                if (row[sideid] != null)
                {
                    side.SideID = int.Parse(row[sideid].ToString());
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
            return side;
        }

        #endregion

        public static int SaveSMAuecDetails(AUEC auecDetails, int auecID)
        {
            int isSavedOnCSM = 1;

            try
            {

                object[] parameter = new object[46];
                parameter[0] = auecID;
                parameter[1] = auecDetails.ExchangeID;
                parameter[2] = auecDetails.FullName;
                parameter[3] = auecDetails.DisplayName;
                parameter[4] = auecDetails.UnitID;
                parameter[5] = auecDetails.TimeZone;
                parameter[6] = auecDetails.PreMarketTradingStartTime;
                parameter[7] = auecDetails.PreMarketTradingEndTime;
                parameter[8] = auecDetails.LunchTimeStartTime;
                parameter[9] = auecDetails.LunchTimeEndTime;
                parameter[10] = auecDetails.RegularTradingStartTime;
                parameter[11] = auecDetails.RegularTradingEndTime;
                parameter[12] = auecDetails.PostMarketTradingStartTime;
                parameter[13] = auecDetails.PostMarketTradingEndTime;
                parameter[14] = auecDetails.SettlementDaysBuy;
                parameter[15] = auecDetails.DayLightSaving;
                parameter[16] = auecDetails.Country;
                parameter[17] = auecDetails.StateID;

                parameter[18] = auecDetails.PreMarketCheck;
                parameter[19] = auecDetails.PostMarketCheck;
                parameter[20] = auecDetails.RegularTimeCheck;
                parameter[21] = auecDetails.LunchTimeCheck;
                parameter[22] = auecDetails.CurrencyConversion;
                parameter[23] = auecDetails.CountryFlagID;
                parameter[24] = auecDetails.LogoID;
                parameter[25] = auecDetails.TimeZoneOffSet;

                parameter[26] = auecDetails.SymbolConventionID;
                parameter[27] = auecDetails.ExchangeIdentifier;
                parameter[28] = auecDetails.MarketDataProviderExchangeIdentifier;

                parameter[29] = auecDetails.CurrencyID;
                parameter[30] = auecDetails.OtherCurrencyID;
                parameter[31] = auecDetails.Multiplier;
                parameter[32] = auecDetails.IsShortSaleConfirmation;
                parameter[33] = auecDetails.ProvideAccountNameWithTrade;
                parameter[34] = auecDetails.ProvideIdentifierNameWithTrade;
                parameter[35] = auecDetails.IdentifierID;
                parameter[36] = auecDetails.AssetID;
                parameter[37] = auecDetails.UnderlyingID;

                parameter[38] = auecDetails.PurchaseSecFees;
                parameter[39] = auecDetails.SaleSecFees;
                parameter[40] = auecDetails.PurchaseStamp;
                parameter[41] = auecDetails.SaleStamp;
                parameter[42] = auecDetails.PurchaseLevy;
                parameter[43] = auecDetails.SaleLevy;
                parameter[44] = auecDetails.SettlementDaysSell;
                parameter[45] = int.MinValue;

                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSMAUECDetails", parameter, ApplicationConstants.SMConnectionString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSavedOnCSM;
        }

        public static int SaveCountry(Country country)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = country.CountryID;
                parameter[1] = country.Name;
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCountry", parameter).ToString());
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

        public static void SaveSMCountryDetails(Country country, int countryID)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = countryID;
                parameter[1] = country.Name;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSMCountryDetails", parameter, ApplicationConstants.SMConnectionString);
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

        public static int SaveState(State state)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[3];
                parameter[0] = state.StateID;
                parameter[1] = state.CountryID;
                parameter[2] = state.StateName;
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveState", parameter).ToString());
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

        public static void SaveSMStateDetails(State state, int stateID)
        {
            try
            {
                object[] parameter = new object[3];
                parameter[0] = stateID;
                parameter[1] = state.CountryID;
                parameter[2] = state.StateName;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSMStateDetails", parameter, ApplicationConstants.SMConnectionString);
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

        public static void DeleteCountry(int ID)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = ID;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteCountry", parameter);
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

        public static void DeleteState(int ID)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = ID;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteState", parameter);
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
        /// It will delete the data for particular Id according to the given options:  
        /// 0 - auecID from T_AUEC        ,1 - exchangeID from T_Exchange  ,2 - currencyID from T_Curency  
        /// 3 - countryID from T_Country  ,4 - stateID from T_State  
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="option"></param>
        public static void DeleteSM_AECCS(int ID, int option)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = ID;
                parameter[1] = option;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteSM_AECCS", parameter, ApplicationConstants.SMConnectionString);
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
    }
}
