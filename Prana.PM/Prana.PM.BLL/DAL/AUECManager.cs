using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.DatabaseManager;
using Prana.LogManager;
//
using Prana.PM.BLL;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.DAL
{
    public class AUECManager
    {
        #region GetAssets
        public static List<EnumerationValue> GetAllAssets()
        {
            List<EnumerationValue> list = null;

            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllAssets";

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillAssetList(productsDataSet);
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

            return list;
        }

        private static List<EnumerationValue> FillAssetList(DataSet ds)
        {
            List<EnumerationValue> list = null;
            try
            {

                int assetID = 0; // +offSet;
                int assetName = 1; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new List<EnumerationValue>();
                    int value = int.MinValue;
                    string text = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!(row[assetID] is System.DBNull))
                        {
                            value = int.Parse(row[assetID].ToString());
                        }
                        if (!(row[assetName] is System.DBNull))
                        {
                            text = row[assetName].ToString();
                        }

                        if (value != int.MinValue)
                        {
                            list.Add(new EnumerationValue(text, value));
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
            return list;
        }
        #endregion

        #region GetAssetMappings
        /// <summary>
        /// Gets the asset mappings.
        /// </summary>
        /// <param name="thirdPartyNameID">The data source name ID.</param>
        /// <returns></returns>
        public static MappingItemList GetAssetMappings(ThirdPartyNameID thirdPartyNameID)
        {
            MappingItemList list = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDataSourceAssetMappings";
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyNameID.ID
                });

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillAssetMappings(productsDataSet);
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

            return list;
        }

        private static MappingItemList FillAssetMappings(DataSet ds)
        {

            MappingItemList list = null;
            try
            {

                int dataSourceAssetID = 0; // +offSet;
                int dataSourceAssetName = 1; // +offSet;
                int applicationAssetId = 2; // +offSet;
                int applicationAssetName = 3; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new MappingItemList();

                    foreach (DataRow row in dt.Rows)
                    {
                        MappingItem item = new MappingItem();

                        if (!(row[dataSourceAssetID] is System.DBNull))
                        {
                            int sourceId = int.Parse(row[dataSourceAssetID].ToString());
                            if (sourceId >= 0)
                            {
                                item.SourceItemID = sourceId;
                            }
                            else
                            {
                                continue;
                            }

                        }
                        if (!(row[dataSourceAssetName] is System.DBNull))
                        {
                            item.SourceItemName = row[dataSourceAssetName].ToString();
                        }
                        if (!(row[applicationAssetId] is System.DBNull))
                        {
                            item.ApplicationItemId = int.Parse(row[applicationAssetId].ToString());
                        }
                        if (!(row[applicationAssetName] is System.DBNull))
                        {
                            item.ApplicationItemName = row[applicationAssetName].ToString();
                        }

                        list.Add(item);

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
            return list;
        }
        #endregion

        #region SaveAssetMappings
        public static int SaveAssetMappings(MappingItemList itemList, ThirdPartyNameID dataSourceNameID)
        {
            int rowsAffected = 0;
            try
            {
                string xml = XMLUtilities.SerializeToXML(itemList);
                rowsAffected = CommonManager.SaveThroughXML("PMAddUpdateAssetMappings", xml, dataSourceNameID.ID);
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
            return rowsAffected;
        }
        #endregion


        #region GetUnderlying
        public static List<EnumerationValue> GetAllUnderlyings()
        {
            List<EnumerationValue> list = null;

            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllUnderLyings";


                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillUnderlyingList(productsDataSet);
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

            return list;
        }

        private static List<EnumerationValue> FillUnderlyingList(DataSet ds)
        {
            List<EnumerationValue> list = null;

            int underlyingID = 0; // +offSet;
            int underlyingName = 1; // +offSet;
            //int comment = 2; // +offSet;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                list = new List<EnumerationValue>();
                int value = int.MinValue;
                string text = string.Empty;

                foreach (DataRow row in dt.Rows)
                {
                    if (!(row[underlyingID] is System.DBNull))
                    {
                        value = int.Parse(row[underlyingID].ToString());
                    }
                    if (!(row[underlyingName] is System.DBNull))
                    {
                        text = row[underlyingName].ToString();
                    }

                    if (value != int.MinValue)
                    {
                        list.Add(new EnumerationValue(text, value));
                    }
                }
            }
            return list;
        }
        #endregion

        #region GetUnderlyingMappings
        /// <summary>
        /// Gets the asset mappings.
        /// </summary>
        /// <param name="thirdPartyNameID">The data source name ID.</param>
        /// <returns></returns>
        public static MappingItemList GetUnderlyingMappings(ThirdPartyNameID thirdPartyNameID)
        {
            MappingItemList list = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDataSourceUnderlyingMappings";
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyNameID.ID
                });

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillUnderlyingMappings(productsDataSet);

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
            return list;
        }

        private static MappingItemList FillUnderlyingMappings(DataSet ds)
        {

            MappingItemList list = null;

            try
            {
                int dataSourceUnderlyingID = 0; // +offSet;
                int dataSourceUnderlyingName = 1; // +offSet;
                int applicationUnderlyingId = 2; // +offSet;
                int applicationUnderlyingName = 3; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new MappingItemList();

                    foreach (DataRow row in dt.Rows)
                    {
                        MappingItem item = new MappingItem();

                        if (!(row[dataSourceUnderlyingID] is System.DBNull))
                        {
                            int sourceId = int.Parse(row[dataSourceUnderlyingID].ToString());
                            if (sourceId >= 0)
                            {
                                item.SourceItemID = sourceId;
                            }
                            else
                            {
                                continue;
                            }

                        }
                        if (!(row[dataSourceUnderlyingName] is System.DBNull))
                        {
                            item.SourceItemName = row[dataSourceUnderlyingName].ToString();
                        }
                        if (!(row[applicationUnderlyingId] is System.DBNull))
                        {
                            item.ApplicationItemId = int.Parse(row[applicationUnderlyingId].ToString());
                        }
                        if (!(row[applicationUnderlyingName] is System.DBNull))
                        {
                            item.ApplicationItemName = row[applicationUnderlyingName].ToString();
                        }
                        list.Add(item);

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

            return list;
        }
        #endregion

        #region SaveUnderlyingMappings
        public static int SaveUnderlyingMappings(MappingItemList itemList, ThirdPartyNameID dataSourceNameID)
        {
            int rowAffected = 0;
            try
            {

                string xml = XMLUtilities.SerializeToXML(itemList);
                rowAffected = CommonManager.SaveThroughXML("PMAddUpdateUnderlyingMappings", xml, dataSourceNameID.ID);


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
            return rowAffected;
        }
        #endregion

        #region GetExchanges
        public static List<EnumerationValue> GetAllExchanges()
        {
            List<EnumerationValue> list = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllExchanges";

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillExchangeList(productsDataSet);

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
            return list;
        }

        private static List<EnumerationValue> FillExchangeList(DataSet ds)
        {
            List<EnumerationValue> list = null;

            try
            {
                int exchangeID = 0; // +offSet;
                int displayName = 2; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new List<EnumerationValue>();
                    int value = int.MinValue;
                    string text = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!(row[exchangeID] is System.DBNull))
                        {
                            value = int.Parse(row[exchangeID].ToString());
                        }
                        if (!(row[displayName] is System.DBNull))
                        {
                            text = row[displayName].ToString();
                        }

                        if (value != int.MinValue)
                        {
                            list.Add(new EnumerationValue(text, value));
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
            return list;
        }
        public static Dictionary<string, string> GetAllExchangesForLookup()
        {
            Dictionary<string, string> lookupTable = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllExchanges";

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                lookupTable = FillExchangeLookupList(productsDataSet);

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
            return lookupTable;

        }

        private static Dictionary<string, string> FillExchangeLookupList(DataSet ds)
        {
            Dictionary<string, string> list = null;

            try
            {
                //int exchangeID = 0; // +offSet;

                int fullName = 1;

                int displayName = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new Dictionary<string, string>();

                    string disp_Name = string.Empty;
                    string full_Name = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {

                        if (!(row[displayName] is System.DBNull))
                        {
                            disp_Name = row[displayName].ToString();
                        }

                        if (!(row[fullName] is System.DBNull))
                        {
                            full_Name = row[fullName].ToString();
                        }

                        list.Add(disp_Name, full_Name);

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

            return list;
        }

        #endregion

        #region GetExchangeMappings
        /// <summary>
        /// Gets the asset mappings.
        /// </summary>
        /// <param name="thirdPartyNameID">The data source name ID.</param>
        /// <returns></returns>
        public static MappingItemList GetExchangeMappings(ThirdPartyNameID thirdPartyNameID)
        {
            MappingItemList list = null;

            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDataSourceExchangeMappings";
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyNameID.ID
                });

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillExchangeMappings(productsDataSet);
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

            return list;
        }

        private static MappingItemList FillExchangeMappings(DataSet ds)
        {

            MappingItemList list = null;

            try
            {
                int dataSourceExchangeID = 0; // +offSet;
                int dataSourceExchangeName = 1; // +offSet;
                int applicationExchangeId = 2; // +offSet;
                int applicationExchangeName = 3; // +offSet;
                int applicationExchangeFullName = 4; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new MappingItemList();

                    foreach (DataRow row in dt.Rows)
                    {
                        MappingItem item = new MappingItem();

                        if (!(row[dataSourceExchangeID] is System.DBNull))
                        {
                            int sourceId = int.Parse(row[dataSourceExchangeID].ToString());
                            if (sourceId >= 0)
                            {
                                item.SourceItemID = sourceId;
                            }
                            else
                            {
                                continue;
                            }

                        }
                        if (!(row[dataSourceExchangeName] is System.DBNull))
                        {
                            item.SourceItemName = row[dataSourceExchangeName].ToString();
                        }
                        if (!(row[applicationExchangeId] is System.DBNull))
                        {
                            item.ApplicationItemId = int.Parse(row[applicationExchangeId].ToString());
                        }
                        if (!(row[applicationExchangeName] is System.DBNull))
                        {
                            item.ApplicationItemName = row[applicationExchangeName].ToString();
                        }
                        if (!(row[applicationExchangeFullName] is System.DBNull))
                        {
                            item.ApplicationItemFullName = row[applicationExchangeFullName].ToString();
                        }
                        list.Add(item);

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

            return list;
        }
        #endregion

        #region SaveExchangeMappings
        public static int SaveExchangeMappings(MappingItemList itemList, ThirdPartyNameID dataSourceNameID)
        {
            int rowsAffected = 0;

            try
            {
                string xml = XMLUtilities.SerializeToXML(itemList);
                rowsAffected = CommonManager.SaveThroughXML("PMAddUpdateExchangeMappings", xml, dataSourceNameID.ID);

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
            return rowsAffected;
        }
        #endregion

        #region GetCurrencies
        public static List<EnumerationValue> GetAllCurrencies()
        {
            List<EnumerationValue> list = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCurrencies";

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillCurrencyList(productsDataSet);

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
            return list;

        }


        private static List<EnumerationValue> FillCurrencyList(DataSet ds)
        {
            List<EnumerationValue> list = null;

            try
            {
                int currencyID = 0; // +offSet;
                int currencySymbol = 2; // +offSet;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new List<EnumerationValue>();
                    int value = int.MinValue;
                    string text = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!(row[currencyID] is System.DBNull))
                        {
                            value = int.Parse(row[currencyID].ToString());
                        }
                        if (!(row[currencySymbol] is System.DBNull))
                        {
                            text = row[currencySymbol].ToString();
                        }

                        if (value != int.MinValue)
                        {
                            list.Add(new EnumerationValue(text, value));
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

            return list;
        }

        public static Dictionary<string, string> GetAllCurrenciesForLookup()
        {
            Dictionary<string, string> list = null;

            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCurrencies";

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillCurrencyLookupList(productsDataSet);

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
            return list;

        }

        private static Dictionary<string, string> FillCurrencyLookupList(DataSet ds)
        {
            Dictionary<string, string> list = null;

            try
            {
                //int currencyID = 0; // +offSet;
                int currencySymbol = 2; // +offSet;
                int currencyName = 1;
                //int comment = 2; // +offSet;

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new Dictionary<string, string>();
                    //int value = int.MinValue;

                    string symbol = string.Empty;
                    string name = string.Empty;

                    foreach (DataRow row in dt.Rows)
                    {

                        //if (!(row[currencyID] is System.DBNull))
                        //{
                        //    currency.ID = int.Parse(row[currencyID].ToString());
                        //}
                        if (!(row[currencySymbol] is System.DBNull))
                        {
                            symbol = row[currencySymbol].ToString();
                        }

                        if (!(row[currencyName] is System.DBNull))
                        {
                            name = row[currencyName].ToString();
                        }

                        list.Add(symbol, name);

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

            return list;
        }
        #endregion

        #region GetCurrencyMappings
        /// <summary>
        /// Gets the asset mappings.
        /// </summary>
        /// <param name="thirdPartyNameID">The data source name ID.</param>
        /// <returns></returns>
        public static MappingItemList GetCurrencyMappings(ThirdPartyNameID thirdPartyNameID)
        {
            MappingItemList list = null;
            try
            {

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDataSourceCurrencyMappings";
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyNameID.ID
                });

                // DataSet that will hold the returned results		
                DataSet productsDataSet = null;

                productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                list = FillCurrencyMappings(productsDataSet);

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
            return list;
        }

        private static MappingItemList FillCurrencyMappings(DataSet ds)
        {

            MappingItemList list = null;

            int dataSourceCurrencyID = 0; // +offSet;
            int dataSourceCurrencyName = 1; // +offSet;
            int applicationCurrencyId = 2; // +offSet;
            int applicationCurrencyName = 3; // +offSet;
            int applicationCurrencyFullName = 4; // +offSet;
            //int comment = 2; // +offSet;

            try
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = new MappingItemList();

                    foreach (DataRow row in dt.Rows)
                    {
                        MappingItem item = new MappingItem();

                        if (!(row[dataSourceCurrencyID] is System.DBNull))
                        {
                            int sourceId = int.Parse(row[dataSourceCurrencyID].ToString());
                            if (sourceId >= 0)
                            {
                                item.SourceItemID = sourceId;
                            }
                            else
                            {
                                continue;
                            }

                        }
                        if (!(row[dataSourceCurrencyName] is System.DBNull))
                        {
                            item.SourceItemName = row[dataSourceCurrencyName].ToString();
                        }
                        if (!(row[applicationCurrencyId] is System.DBNull))
                        {
                            item.ApplicationItemId = int.Parse(row[applicationCurrencyId].ToString());
                        }
                        if (!(row[applicationCurrencyName] is System.DBNull))
                        {
                            item.ApplicationItemName = row[applicationCurrencyName].ToString();
                        }
                        if (!(row[applicationCurrencyFullName] is System.DBNull))
                        {
                            item.ApplicationItemFullName = row[applicationCurrencyFullName].ToString();
                        }
                        list.Add(item);

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

            return list;
        }
        #endregion

        #region SaveCurrencyMappings
        public static int SaveCurrencyMappings(MappingItemList itemList, ThirdPartyNameID dataSourceNameID)
        {
            int rowsAffected = 0;

            try
            {
                string xml = XMLUtilities.SerializeToXML(itemList);
                rowsAffected = CommonManager.SaveThroughXML("PMAddUpdateCurrencyMappings", xml, dataSourceNameID.ID);

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
            return rowsAffected;
        }
        #endregion

    }
}
