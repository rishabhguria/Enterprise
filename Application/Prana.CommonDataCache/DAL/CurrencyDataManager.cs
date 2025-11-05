#region Using namespaces
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#endregion

namespace Prana.CommonDataCache
{
    /// <summary>
    /// Summary description for CurrencyDataManager.
    /// </summary>
    public class CurrencyDataManager
    {

        private static readonly CurrencyDataManager instance = new CurrencyDataManager();

        private Dictionary<string, string> _currencyCodeSymbolDict;
        /// <summary>
        /// This dictionary keeps the mapping for the currency 3 letter code and the symbol
        /// e.g. - USD - $ 
        /// </summary>
        public Dictionary<string, string> CurrencyCodeSymbolDict
        {
            get { return _currencyCodeSymbolDict; }
            set { _currencyCodeSymbolDict = value; }
        }


        private CurrencyDataManager()
        {
            try
            {
                FillCurrencySymbols();
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

        public static CurrencyDataManager GetInstance()
        {
            return instance;
        }

        #region Get the Currency Conversion Details
        /// <summary>
        /// Fills the row of Currency Converter <see cref="CurrencyConversion"/> object.
        /// Changed Rajat - Added CURRENCY_CONVERSION_TYPE 
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="CounterParty"/></returns>
        private CurrencyConversion FillCurrencyConversion(object[] row, int offset)
        {

            if (offset < 0)
            {
                offset = 0;
            }

            CurrencyConversion currencyConversion = null;
            try
            {
                if (row != null)
                {
                    currencyConversion = new CurrencyConversion();

                    int FROM_CURRENCY_ID = offset + 0;
                    int FROM_CURRENCY_NAME = offset + 1;
                    int TO_CURRENCY_ID = offset + 2;
                    int TO_CURRENCY_NAME = offset + 3;
                    int CURRENCY_CONVERSION_FACTOR = offset + 4;
                    int CURRENCY_CONVERSION_TYPE = offset + 5;

                    currencyConversion.FromCurrencyID = Convert.ToInt32(row[FROM_CURRENCY_ID]);
                    string fromCurrencyName = Convert.ToString(row[FROM_CURRENCY_NAME]);
                    currencyConversion.FromCurrencyName = fromCurrencyName;
                    currencyConversion.ToCurrencyID = Convert.ToInt32(row[TO_CURRENCY_ID]);
                    string toCurrencyName = Convert.ToString(row[TO_CURRENCY_NAME]);
                    currencyConversion.ToCurrencyName = toCurrencyName;
                    currencyConversion.CurrencyConversionFactor = Convert.ToDouble(row[CURRENCY_CONVERSION_FACTOR]);
                    currencyConversion.IsDirectConversion = Convert.ToBoolean(row[CURRENCY_CONVERSION_TYPE]);
                    currencyConversion.CurrencyPairSymbol = fromCurrencyName + "-" + toCurrencyName;
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
            return currencyConversion;
        }


        /// <summary>
        /// Gets the currency conversions.
        /// </summary>
        /// <returns>A normal collection</returns>
        public Prana.BusinessObjects.CurrencyConversionCollection GetCurrencyConversions()
        {

            CurrencyConversionCollection currencyConversionCollection = new CurrencyConversionCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrencyConversion";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyConversionCollection.Add(FillCurrencyConversion(row, 0));
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
            return currencyConversionCollection;
        }


        /// <summary>
        /// Author : Rajat - 25 Sep 2006, Specially for Forex utility
        /// Gets the currency conversions.
        /// </summary>
        /// <returns>A generic collection</returns>
        public List<CurrencyConversion> GetCurrencyConversionList()
        {
            List<CurrencyConversion> currencyConversionCollection = new List<CurrencyConversion>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrencyCodeConversion";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyConversionCollection.Add(FillCurrencyConversion(row, 0));
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
            return currencyConversionCollection;
        }

        /// <summary>
        /// Update the conversion rate into database.
        /// </summary>
        /// <param name="forexCurrencyConversions">The forex currency conversions.</param>
        public void SaveCurrencyConversionList(ICollection<CurrencyConversion> forexCurrencyConversions)
        {
            #region try
            try
            {
                foreach (CurrencyConversion currencyConversion in forexCurrencyConversions)
                {
                    object[] parameter = new object[3];

                    parameter[0] = currencyConversion.FromCurrencyID;
                    parameter[1] = currencyConversion.ToCurrencyID;
                    parameter[2] = currencyConversion.CurrencyConversionFactor;

                    DatabaseManager.DatabaseManager.ExecuteScalar("[P_SaveForexRates]", parameter);
                }
            }
            # endregion

            #region Catch
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);


                //if (rethrow)
                //{
                //    throw;
                //}
            }
            #endregion

        }


        #endregion

        #region Get Currency
        /// <summary>
        /// Fills the row of Currency Converter <see cref="CurrencyConversion"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="CounterParty"/></returns>
        private Currency FillCurrency(object[] row, int offset)
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

                    int CURRENCY_ID = offset + 0;
                    int CURRENCY_NAME = offset + 1;

                    currency.CurrencyID = Convert.ToInt32(row[CURRENCY_ID]);
                    currency.CurrencyName = Convert.ToString(row[CURRENCY_NAME]);
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

        public Prana.BusinessObjects.CurrencyCollection GetCurrencies()
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
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return currencyCollection;
        }

        public Prana.BusinessObjects.CurrencyCollection GetCurrenciesWithSymbol()
        {

            CurrencyCollection currencyCollection = new CurrencyCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrenciesWithSymbol";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currencyCollection.Add(FillCurrencyWithSymbol(row, 0));
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
            return currencyCollection;
        }

        private Currency FillCurrencyWithSymbol(object[] row, int offset)
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

                    int CURRENCY_ID = offset + 0;
                    int CURRENCY_NAME = offset + 1;
                    int CURRENCY_SYMBOL = offset + 2;

                    currency.CurrencyID = Convert.ToInt32(row[CURRENCY_ID]);
                    currency.CurrencyName = Convert.ToString(row[CURRENCY_NAME]);
                    currency.Symbol = Convert.ToString(row[CURRENCY_SYMBOL]);
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

        #endregion


        private void FillCurrencySymbols()
        {
            try
            {
                CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                foreach (CultureInfo culture in allCultures)
                {
                    //Build the regionInfo object from the cultureInfo object
                    int lcid = culture.LCID;
                    RegionInfo ri = new RegionInfo(lcid);
                    string currencyCode = ri.ISOCurrencySymbol;
                    string currencySymbol = ri.CurrencySymbol;

                    if (!_currencyCodeSymbolDict.ContainsKey(currencyCode))
                    {
                        _currencyCodeSymbolDict.Add(currencyCode, currencySymbol);
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

    }
}
