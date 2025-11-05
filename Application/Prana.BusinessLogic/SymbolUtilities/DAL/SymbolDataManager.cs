using Prana.BusinessObjects;
using Prana.BusinessObjects.LiveFeed;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessLogic.Symbol
{
    public class SymbolDataManager
    {
        public static Dictionary<int, OptionSymbolMapper> GetOptionSymbolMapping()
        {
            Dictionary<int, OptionSymbolMapper> dictOptionSymbolMapper = new Dictionary<int, OptionSymbolMapper>();
            try
            {
                DataSet dsOptionSymbolMapper = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;
                dsOptionSymbolMapper = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                #region Assign Values
                if (dsOptionSymbolMapper != null && dsOptionSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    for (int counter = 0; counter < dsOptionSymbolMapper.Tables[0].Rows.Count; counter++)
                    {
                        OptionSymbolMapper optionSymbolMapper = new OptionSymbolMapper();

                        optionSymbolMapper.AUECID = Convert.ToInt32(dsOptionSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                        optionSymbolMapper.AssetID = Convert.ToInt32(dsOptionSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                        optionSymbolMapper.ExchangeIdentifier = (dsOptionSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                        optionSymbolMapper.ExchangeToken = (dsOptionSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                        optionSymbolMapper.EsignalOptionFormatString = (dsOptionSymbolMapper.Tables[0].Rows[counter]["EsignalOptionFormatString"]).ToString().Trim();
                        optionSymbolMapper.BloombergOptionFormatString = (dsOptionSymbolMapper.Tables[0].Rows[counter]["BloombergOptionFormatString"]).ToString().Trim();
                        optionSymbolMapper.EsignalRootToken = (dsOptionSymbolMapper.Tables[0].Rows[counter]["EsignalRootToken"]).ToString().Trim();
                        optionSymbolMapper.BloombergRootToken = (dsOptionSymbolMapper.Tables[0].Rows[counter]["BloombergRootToken"]).ToString().Trim();

                        dictOptionSymbolMapper.Add(optionSymbolMapper.AUECID, optionSymbolMapper);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictOptionSymbolMapper;
        }

        public static Dictionary<int, PSSymbolMapper> GetPSSymbolMapping()
        {
            Dictionary<int, PSSymbolMapper> dictPSSymbolMapper = new Dictionary<int, PSSymbolMapper>();
            try
            {
                DataSet dsPSSymbolMapper = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;
                dsPSSymbolMapper = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                #region Assign Values
                if (dsPSSymbolMapper != null && dsPSSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    for (int counter = 0; counter < dsPSSymbolMapper.Tables[0].Rows.Count; counter++)
                    {
                        PSSymbolMapper psSymbolMapper = new PSSymbolMapper();

                        psSymbolMapper.AUECID = Convert.ToInt32(dsPSSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                        psSymbolMapper.ExchangeIdentifier = (dsPSSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                        psSymbolMapper.Year = (dsPSSymbolMapper.Tables[0].Rows[counter]["Year"]).ToString().Trim();
                        psSymbolMapper.Month = (dsPSSymbolMapper.Tables[0].Rows[counter]["Month"]).ToString().Trim();
                        psSymbolMapper.Day = (dsPSSymbolMapper.Tables[0].Rows[counter]["Day"]).ToString().Trim();
                        psSymbolMapper.Type = (dsPSSymbolMapper.Tables[0].Rows[counter]["Type"]).ToString().Trim();
                        psSymbolMapper.Strike = (dsPSSymbolMapper.Tables[0].Rows[counter]["Strike"]).ToString().Trim();
                        psSymbolMapper.ExchangeToken = (dsPSSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                        psSymbolMapper.PSRootToken = (dsPSSymbolMapper.Tables[0].Rows[counter]["PSRootToken"]).ToString().Trim();
                        psSymbolMapper.PSFormatString = (dsPSSymbolMapper.Tables[0].Rows[counter]["PSFormatString"]).ToString().Trim();
                        psSymbolMapper.TranslateRoot = Convert.ToBoolean(dsPSSymbolMapper.Tables[0].Rows[counter]["TranslateRoot"]);
                        psSymbolMapper.TranslateType = Convert.ToBoolean(dsPSSymbolMapper.Tables[0].Rows[counter]["TranslateType"]);
                        psSymbolMapper.ExerciseStyle = (dsPSSymbolMapper.Tables[0].Rows[counter]["ExerciseStyle"]).ToString().Trim();

                        dictPSSymbolMapper.Add(psSymbolMapper.AUECID, psSymbolMapper);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictPSSymbolMapper;
        }

        public static Dictionary<string, MarketDataSymbolMapper> GetMarketDataSymbolMapping()
        {
            Dictionary<string, MarketDataSymbolMapper> dictMarketDataSymbolMapper = new Dictionary<string, MarketDataSymbolMapper>();
            try
            {
                DataSet dsMarketDataSymbolMapper = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;
                dsMarketDataSymbolMapper = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                #region Assign Values
                if (dsMarketDataSymbolMapper != null && dsMarketDataSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    for (int counter = 0; counter < dsMarketDataSymbolMapper.Tables[0].Rows.Count; counter++)
                    {
                        MarketDataSymbolMapper marketDataSymbolMapper = new MarketDataSymbolMapper();

                        marketDataSymbolMapper.AUECID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                        marketDataSymbolMapper.ExchangeIdentifier = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                        marketDataSymbolMapper.ExchangeToken = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                        marketDataSymbolMapper.AssetID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                        marketDataSymbolMapper.EsignalExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetRegionCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetRegionCode"]).ToString().Trim();
                        marketDataSymbolMapper.EsignalFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.ActivFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ActivFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.BloombergCompositeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE]).ToString().Trim();
                        marketDataSymbolMapper.BloombergExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE]).ToString().Trim();
                        marketDataSymbolMapper.BloombergFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING]).ToString().Trim();

                        if (!dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode))
                            dictMarketDataSymbolMapper.Add(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode, marketDataSymbolMapper);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictMarketDataSymbolMapper;
        }

        public static Dictionary<string, MarketDataSymbolMapper> GetTickerSymbolMapping(bool addEsignalExchangeCode)
        {
            Dictionary<string, MarketDataSymbolMapper> dictMarketDataSymbolMapper = new Dictionary<string, MarketDataSymbolMapper>();
            try
            {
                DataSet dsMarketDataSymbolMapper = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;
                dsMarketDataSymbolMapper = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                #region Assign Values
                if (dsMarketDataSymbolMapper != null && dsMarketDataSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    for (int counter = 0; counter < dsMarketDataSymbolMapper.Tables[0].Rows.Count; counter++)
                    {
                        MarketDataSymbolMapper marketDataSymbolMapper = new MarketDataSymbolMapper();

                        marketDataSymbolMapper.AUECID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                        marketDataSymbolMapper.ExchangeIdentifier = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                        marketDataSymbolMapper.ExchangeToken = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                        marketDataSymbolMapper.AssetID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                        marketDataSymbolMapper.EsignalExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetRegionCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetRegionCode"]).ToString().Trim();
                        marketDataSymbolMapper.EsignalFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.ActivFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ActivFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.BloombergCompositeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE]).ToString().Trim();
                        marketDataSymbolMapper.BloombergExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE]).ToString().Trim();
                        if (addEsignalExchangeCode)
                        {
                            if (!dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode))
                                dictMarketDataSymbolMapper.Add(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode, marketDataSymbolMapper);
                        }
                        else
                        {
                            if (!dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.FactSetExchangeCode))
                                dictMarketDataSymbolMapper.Add(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.FactSetExchangeCode, marketDataSymbolMapper);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictMarketDataSymbolMapper;
        }

        /// <summary>
        /// Get Ticker symbol mapping data exchange identifier wise.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MarketDataSymbolMapper> GetTickerSymbolMappingExchangeIdentifier()
        {
            Dictionary<string, MarketDataSymbolMapper> dictMarketDataSymbolMapper = new Dictionary<string, MarketDataSymbolMapper>();
            try
            {
                DataSet dsMarketDataSymbolMapper = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAUECMapping";
                queryData.CommandTimeout = 200;
                dsMarketDataSymbolMapper = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                #region Assign Values
                if (dsMarketDataSymbolMapper != null && dsMarketDataSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    for (int counter = 0; counter < dsMarketDataSymbolMapper.Tables[0].Rows.Count; counter++)
                    {
                        MarketDataSymbolMapper marketDataSymbolMapper = new MarketDataSymbolMapper();

                        marketDataSymbolMapper.AUECID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                        marketDataSymbolMapper.ExchangeIdentifier = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                        marketDataSymbolMapper.ExchangeToken = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                        marketDataSymbolMapper.AssetID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                        marketDataSymbolMapper.EsignalExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetExchangeCode"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetRegionCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetRegionCode"]).ToString().Trim();
                        marketDataSymbolMapper.EsignalFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.FactSetFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.ActivFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ActivFormatString"]).ToString().Trim();
                        marketDataSymbolMapper.BloombergCompositeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE]).ToString().Trim();
                        marketDataSymbolMapper.BloombergExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE]).ToString().Trim();

                        if (!dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapper.ExchangeIdentifier))
                            dictMarketDataSymbolMapper.Add(marketDataSymbolMapper.ExchangeIdentifier, marketDataSymbolMapper);

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dictMarketDataSymbolMapper;
        }
    }
}