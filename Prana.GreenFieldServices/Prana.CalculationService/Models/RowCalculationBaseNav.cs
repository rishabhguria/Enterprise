using Prana.BusinessObjects.AppConstants;
using Prana.CalculationService.Constants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Prana.CalculationService.Models
{
    internal class RowCalculationBaseNav : RowCalculationMarketDataBased
    {
        internal static RowCalculationBaseNav GetRowCalculationObject(string data, MarketDataProvider marketDataProvider)
        {
            RowCalculationBaseNav rowCalculationData = new RowCalculationBaseNav();
            try
            {
                JToken rootObject = JToken.Parse(data);
                JObject hashMap = rootObject[CompressionConstants.CONST_HashMap] as JObject;

                if (hashMap[CompressionConstants.CONST_Symbol] != null)
                    rowCalculationData.Symbol = hashMap[CompressionConstants.CONST_Symbol].ToString();
                if (hashMap[CompressionConstants.CONST_BloombergSymbol] != null)
                    rowCalculationData.BBGSym = hashMap[CompressionConstants.CONST_BloombergSymbol].ToString();
                if (hashMap[CompressionConstants.CONST_Underlying_Symbol] != null)
                    rowCalculationData.UndlSym = hashMap[CompressionConstants.CONST_Underlying_Symbol].ToString();

                #region LiveFeedProviderTicker set based on MarketDataProvider 
                if (marketDataProvider == MarketDataProvider.ACTIV && hashMap[CompressionConstants.CONST_ActivSymbol] != null)
                    rowCalculationData.LiveFTick = hashMap[CompressionConstants.CONST_ActivSymbol].ToString();
                else if (marketDataProvider == MarketDataProvider.FactSet && hashMap[CompressionConstants.CONST_FactsetSymbol] != null)
                    rowCalculationData.LiveFTick = hashMap[CompressionConstants.CONST_FactsetSymbol].ToString();
                else if (marketDataProvider == MarketDataProvider.SAPI && hashMap[CompressionConstants.CONST_BloombergSymbol] != null)
                {
                    rowCalculationData.LiveFTick = hashMap[CompressionConstants.CONST_BloombergSymbol].ToString();
                }
                else
                    rowCalculationData.LiveFTick = string.Empty;
                #endregion

                if (hashMap[CompressionConstants.CONST_AvgPrice] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_AvgPrice].ToString()))
                    rowCalculationData.AvgPrice = Convert.ToDouble(hashMap[CompressionConstants.CONST_AvgPrice].ToString());
                if (hashMap[CompressionConstants.CONST_AccountID] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_AccountID].ToString()))
                    rowCalculationData.AccId = Convert.ToInt32(hashMap[CompressionConstants.CONST_AccountID]);
                if (hashMap[CompressionConstants.CONST_AccountShortName] != null)
                    rowCalculationData.AccShrtName = hashMap[CompressionConstants.CONST_AccountShortName].ToString();
                if (hashMap[CompressionConstants.CONST_MasterFundID] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_MasterFundID].ToString()))
                    rowCalculationData.MFId = Convert.ToInt32(hashMap[CompressionConstants.CONST_MasterFundID]);
                if (hashMap[CompressionConstants.CONST_MasterFundName] != null)
                    rowCalculationData.MFName = hashMap[CompressionConstants.CONST_MasterFundName].ToString();
                if (hashMap[CompressionConstants.CONST_AssetID] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_AssetID].ToString()))
                    rowCalculationData.AssetId = Convert.ToInt32(hashMap[CompressionConstants.CONST_AssetID]);
                else
                    rowCalculationData.AssetId = int.MinValue;
                if (hashMap[CompressionConstants.CONST_Asset] != null)
                    rowCalculationData.Asset = hashMap[CompressionConstants.CONST_Asset].ToString();
                if (hashMap[CompressionConstants.CONST_Quantity] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_Quantity].ToString()))
                    rowCalculationData.Qty = Convert.ToDouble(hashMap[CompressionConstants.CONST_Quantity].ToString());
                if (hashMap[CompressionConstants.CONST_DefaultPositionSide] != null)
                {
                    string postionside = hashMap[CompressionConstants.CONST_DefaultPositionSide].ToString();
                    if (!string.IsNullOrWhiteSpace(postionside))
                    {
                        rowCalculationData.PosSide = char.ToUpper(postionside[0]) + postionside.Substring(1);
                    }
                }
                if (hashMap[CompressionConstants.CONST_ExposurePositionSide] != null)
                {
                    string postionside = hashMap[CompressionConstants.CONST_ExposurePositionSide].ToString();
                    if (!string.IsNullOrWhiteSpace(postionside))
                    {
                        rowCalculationData.ExpPosSide = char.ToUpper(postionside[0]) + postionside.Substring(1);
                    }
                }
                if (hashMap[CompressionConstants.CONST_NetMarketValueBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_NetMarketValueBase].ToString()))
                    rowCalculationData.NetMVBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_NetMarketValueBase].ToString());
                if (hashMap[CompressionConstants.CONST_NetMarketValueLocal] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_NetMarketValueLocal].ToString()))
                    rowCalculationData.NetMVLocal = Convert.ToDouble(hashMap[CompressionConstants.CONST_NetMarketValueLocal].ToString());
                if (hashMap[CompressionConstants.CONST_DayPnLBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_DayPnLBase].ToString()))
                    rowCalculationData.DPnLBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_DayPnLBase].ToString());
                if (hashMap[CompressionConstants.CONST_DayPnLLocal] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_DayPnLLocal].ToString()))
                    rowCalculationData.DPnLLocal = Convert.ToDouble(hashMap[CompressionConstants.CONST_DayPnLLocal].ToString());
                if (hashMap[CompressionConstants.CONST_NetExposureBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_NetExposureBase].ToString()))
                    rowCalculationData.NetExpBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_NetExposureBase].ToString());
                if (hashMap[CompressionConstants.CONST_NetExposureLocal] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_NetExposureLocal].ToString()))
                    rowCalculationData.NetExpLocal = Convert.ToDouble(hashMap[CompressionConstants.CONST_NetExposureLocal].ToString());
                if (hashMap[CompressionConstants.CONST_CostBasisPnLLocal] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_CostBasisPnLLocal].ToString()))
                    rowCalculationData.CostBPnLLoc = Convert.ToDouble(hashMap[CompressionConstants.CONST_CostBasisPnLLocal].ToString());
                if (hashMap[CompressionConstants.CONST_CostBasisPnLBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_CostBasisPnLBase].ToString()))
                    rowCalculationData.CostBPnLB = Convert.ToDouble(hashMap[CompressionConstants.CONST_CostBasisPnLBase].ToString());
                if (hashMap[CompressionConstants.CONST_SelectedFeedPrice] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_SelectedFeedPrice].ToString()))
                    rowCalculationData.FeedPriceL = Convert.ToDouble(hashMap[CompressionConstants.CONST_SelectedFeedPrice].ToString());
                if (hashMap[CompressionConstants.CONST_CurrentFxRate] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_CurrentFxRate].ToString()))
                    rowCalculationData.CurFxRate = Convert.ToDouble(hashMap[CompressionConstants.CONST_CurrentFxRate].ToString());
                if (hashMap[CompressionConstants.CONST_FxConversionMethod] != null)
                    rowCalculationData.FxConvM = hashMap[CompressionConstants.CONST_FxConversionMethod].ToString();
                if (hashMap[CompressionConstants.CONST_NotionalBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_NotionalBase].ToString()))
                    rowCalculationData.NtnlBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_NotionalBase].ToString());
                if (hashMap[CompressionConstants.CONST_SelectedFeedPriceBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_SelectedFeedPriceBase].ToString()))
                    rowCalculationData.FeedPriceB = Convert.ToDouble(hashMap[CompressionConstants.CONST_SelectedFeedPriceBase].ToString());

                if (rowCalculationData.AccId == -1)
                    rowCalculationData.AccShrtName = RtpnlConstants.CONST_EMPTY_STRING;

                if (rowCalculationData.MFId == -1)
                    rowCalculationData.MFName = RtpnlConstants.CONST_EMPTY_STRING;

                if (hashMap[CompressionConstants.CONST_ShortExposureBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_ShortExposureBase].ToString()))
                    rowCalculationData.ShExpBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_ShortExposureBase].ToString());
                if (hashMap[CompressionConstants.CONST_LongExposureBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_LongExposureBase].ToString()))
                    rowCalculationData.LoExpBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_LongExposureBase].ToString());
                if (hashMap[CompressionConstants.CONST_Accrual] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_Accrual].ToString()))
                    rowCalculationData.Accrual = Convert.ToDouble(hashMap[CompressionConstants.CONST_Accrual].ToString());
                if (hashMap[CompressionConstants.CONST_CurrentCash] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_CurrentCash].ToString()))
                    rowCalculationData.CurCash = Convert.ToDouble(hashMap[CompressionConstants.CONST_CurrentCash].ToString());
                if (hashMap[CompressionConstants.CONST_AccountNav] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_AccountNav].ToString()))
                    rowCalculationData.AccNav = Convert.ToDouble(hashMap[CompressionConstants.CONST_AccountNav].ToString());

                if (hashMap[CompressionConstants.CONST_OrderSide] != null)
                    rowCalculationData.OrdSide = hashMap[CompressionConstants.CONST_OrderSide].ToString();
                if (hashMap[CompressionConstants.CONST_StrategyId] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_StrategyId].ToString()))
                    rowCalculationData.StratId = Convert.ToInt32(hashMap[CompressionConstants.CONST_StrategyId].ToString());
                if (hashMap[CompressionConstants.CONST_StrategyName] != null)
                    rowCalculationData.Strategy = hashMap[CompressionConstants.CONST_StrategyName].ToString();
                if (hashMap[CompressionConstants.CONST_Issuer] != null)
                    rowCalculationData.SecN = hashMap[CompressionConstants.CONST_Issuer].ToString();

                if (hashMap[CompressionConstants.CONST_GrossExposureBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_GrossExposureBase].ToString()))
                    rowCalculationData.GrossExpBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_GrossExposureBase].ToString());

                if (hashMap[CompressionConstants.CONST_GrossExposureBaseAccount] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_GrossExposureBaseAccount].ToString()))
                    rowCalculationData.GrossExpBaseAcc = Convert.ToDouble(hashMap[CompressionConstants.CONST_GrossExposureBaseAccount].ToString());

                if (hashMap[CompressionConstants.CONST_GrossExposureBaseMasterFund] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_GrossExposureBaseMasterFund].ToString()))
                    rowCalculationData.GrossExpBaseMF = Convert.ToDouble(hashMap[CompressionConstants.CONST_GrossExposureBaseMasterFund].ToString());

                if (hashMap[CompressionConstants.CONST_BetaAdjustedExposureBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_BetaAdjustedExposureBase].ToString()))
                    rowCalculationData.BetaAdjExpBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_BetaAdjustedExposureBase].ToString());

                if (hashMap[CompressionConstants.CONST_DeltaAdjPosition] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_DeltaAdjPosition].ToString()))
                    rowCalculationData.DeltaAdjPos = Convert.ToDouble(hashMap[CompressionConstants.CONST_DeltaAdjPosition].ToString());

                if (hashMap[CompressionConstants.CONST_UdaAssetClass] != null)
                    rowCalculationData.UDAAsset = hashMap[CompressionConstants.CONST_UdaAssetClass].ToString();

                if (hashMap[CompressionConstants.CONST_UdaSector] != null)
                    rowCalculationData.UDASector = hashMap[CompressionConstants.CONST_UdaSector].ToString();

                if (hashMap[CompressionConstants.CONST_UdaSubSector] != null)
                    rowCalculationData.UDASubSect = hashMap[CompressionConstants.CONST_UdaSubSector].ToString();

                if (hashMap[CompressionConstants.CONST_UdaSecurityType] != null)
                    rowCalculationData.UDASecType = hashMap[CompressionConstants.CONST_UdaSecurityType].ToString();

                if (hashMap[CompressionConstants.CONST_UdaCountry] != null)
                    rowCalculationData.UDACountry = hashMap[CompressionConstants.CONST_UdaCountry].ToString();

                if (hashMap[CompressionConstants.CONST_Currency] != null)
                    rowCalculationData.Ccy = hashMap[CompressionConstants.CONST_Currency].ToString();

                if (hashMap[CompressionConstants.CONST_MasterFundNav] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_MasterFundNav].ToString()))
                    rowCalculationData.MFNav = Convert.ToDouble(hashMap[CompressionConstants.CONST_MasterFundNav].ToString());

                if (hashMap[CompressionConstants.CONST_YesterdayMarketValueBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_YesterdayMarketValueBase].ToString()))
                    rowCalculationData.YesterdayMVBase = Convert.ToDouble(hashMap[CompressionConstants.CONST_YesterdayMarketValueBase].ToString());

                if (hashMap[CompressionConstants.CONST_StartOfDayNavAccount] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_StartOfDayNavAccount].ToString()))
                    rowCalculationData.SODNavAcc = Convert.ToDouble(hashMap[CompressionConstants.CONST_StartOfDayNavAccount].ToString());

                if (hashMap[CompressionConstants.CONST_StartOfDayNavMasterFund] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_StartOfDayNavMasterFund].ToString()))
                    rowCalculationData.SODNavMF = Convert.ToDouble(hashMap[CompressionConstants.CONST_StartOfDayNavMasterFund].ToString());

                if (hashMap[CompressionConstants.CONST_TodayReturnValueBase] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TodayReturnValueBase].ToString()))
                    rowCalculationData.TdyRtrnValB = Convert.ToDouble(hashMap[CompressionConstants.CONST_TodayReturnValueBase].ToString());

                List<string> dynamicUDAKeys = DynamicUDAKeys;
                foreach (string dynamicUDAKey in dynamicUDAKeys)
                {
                    if (hashMap[dynamicUDAKey] != null)
                    {
                        string dynamicUDAValue = hashMap[dynamicUDAKey].ToString();
                        if (!rowCalculationData._dynamicUDAs.ContainsKey(dynamicUDAKey))
                        {
                            rowCalculationData._dynamicUDAs.Add(dynamicUDAKey, dynamicUDAValue);
                        }
                        else
                        {
                            rowCalculationData._dynamicUDAs[dynamicUDAKey] = dynamicUDAValue;
                        }
                    }
                }

                if (hashMap[CompressionConstants.CONST_TradeAttribute1] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute1].ToString()))
                    rowCalculationData.TradeAttribute1 = hashMap[CompressionConstants.CONST_TradeAttribute1].ToString();

                if (hashMap[CompressionConstants.CONST_TradeAttribute2] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute2].ToString()))
                    rowCalculationData.TradeAttribute2 = hashMap[CompressionConstants.CONST_TradeAttribute2].ToString();

                if (hashMap[CompressionConstants.CONST_TradeAttribute3] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute3].ToString()))
                    rowCalculationData.TradeAttribute3 = hashMap[CompressionConstants.CONST_TradeAttribute3].ToString();

                if (hashMap[CompressionConstants.CONST_TradeAttribute4] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute4].ToString()))
                    rowCalculationData.TradeAttribute4 = hashMap[CompressionConstants.CONST_TradeAttribute4].ToString();

                if (hashMap[CompressionConstants.CONST_TradeAttribute5] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute5].ToString()))
                    rowCalculationData.TradeAttribute5 = hashMap[CompressionConstants.CONST_TradeAttribute5].ToString();

                if (hashMap[CompressionConstants.CONST_TradeAttribute6] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_TradeAttribute6].ToString()))
                    rowCalculationData.TradeAttribute6 = hashMap[CompressionConstants.CONST_TradeAttribute6].ToString();

                if (hashMap[CompressionConstants.CONST_PricingStatus] != null && !string.IsNullOrWhiteSpace(hashMap[CompressionConstants.CONST_PricingStatus].ToString()))
                    rowCalculationData.PricingStatus = Convert.ToInt32(hashMap[CompressionConstants.CONST_PricingStatus]);

                if (hashMap[CompressionConstants.CONST_UniqueKey] != null)
                    rowCalculationData.UnqId = hashMap[CompressionConstants.CONST_UniqueKey].ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowCalculationData;
        }

        internal bool PropertiesEqual(RowCalculationBaseNav other)
        {
            try
            {
                if (other == null)
                    return false;

                // Compare all public instance properties except InsertionDateTime and PermOverride
                foreach (var prop in Properties)
                {
                    if (prop.Name.Equals("InsertionDateTime") || prop.Name.Equals("PermOverride"))
                        continue;

                    var thisValue = prop.GetValue(this);
                    var otherValue = prop.GetValue(other);

                    if (thisValue == null && otherValue == null)
                        continue;

                    if ((thisValue == null && otherValue != null) ||
                        (thisValue != null && !thisValue.Equals(otherValue)))
                    {
                        return false; // At least one property is different
                    }
                }

                // Compare _dynamicUDAs
                if (_dynamicUDAs == null && other._dynamicUDAs == null)
                    return true;

                if ((_dynamicUDAs == null && other._dynamicUDAs != null) ||
                    (_dynamicUDAs != null && other._dynamicUDAs == null))
                    return false;

                if (_dynamicUDAs.Count != other._dynamicUDAs.Count)
                    return false;

                foreach (var kvp in _dynamicUDAs)
                {
                    string otherValue;
                    if (!other._dynamicUDAs.TryGetValue(kvp.Key, out otherValue))
                        return false;
                    if (!string.Equals(kvp.Value, otherValue, StringComparison.Ordinal))
                        return false;
                }

                return true; // All properties and dynamic UDAs are equal
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return true;
            }
        }

        public bool Equals(RowCalculationBaseNav compressionObject)
        {
            if (compressionObject != null && Symbol == compressionObject.Symbol &&
                SecN == compressionObject.SecN && Asset == compressionObject.Asset &&
                MFId == compressionObject.MFId && AccId == compressionObject.AccId &&
                Qty == compressionObject.Qty && PosSide == compressionObject.PosSide)
            {
                return false;
            }
            return true;
        }

        internal bool AreBasePropertiesChanged(RowCalculationBaseNav other)
        {
            if (other == null)
                return false;

            foreach (var prop in BaseProperties)
            {
                var thisValue = prop.GetValue(this);
                var otherValue = prop.GetValue(other);

                if (thisValue == null && otherValue == null)
                    continue;

                if ((thisValue == null && otherValue != null) ||
                    (thisValue != null && !thisValue.Equals(otherValue)))
                {
                    return true; // At least one property is different
                }
            }

            // Compare _dynamicUDAs
            if (_dynamicUDAs == null && other._dynamicUDAs == null)
                return false;

            if ((_dynamicUDAs == null && other._dynamicUDAs != null) ||
                (_dynamicUDAs != null && other._dynamicUDAs == null))
                return true;

            if (_dynamicUDAs.Count != other._dynamicUDAs.Count)
                return true;

            foreach (var kvp in _dynamicUDAs)
            {
                string otherValue;
                if (!other._dynamicUDAs.TryGetValue(kvp.Key, out otherValue))
                    return true;
                if (!string.Equals(kvp.Value, otherValue, StringComparison.Ordinal))
                    return true;
            }

            return false; // All base properties are the same
        }

        /// <summary>
        /// This method combines the serialized data of RowCalculationBaseNav and its dynamic UDA properties into a single JSON string.
        /// </summary>
        /// <param name="rowCalculation"></param>
        /// <returns></returns>
        public static string GetCustomSerializedData(KeyValuePair<string, RowCalculationBaseNav> rowCalculation)
        {
            string serializedData = string.Empty;
            try
            {
                string rowCalculationJson = JsonConvert.SerializeObject(rowCalculation);
                string dynamicUDAJson = JsonConvert.SerializeObject(rowCalculation.Value._dynamicUDAs);
                serializedData = rowCalculationJson.Substring(0, rowCalculationJson.Length - 2) + "," + dynamicUDAJson.Substring(1, dynamicUDAJson.Length - 1) + "}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return serializedData;
        }

        /// <summary>
        /// This method combines the serialized data of all RowCalculationBaseNav objects in the dictionary into a single JSON string.
        /// </summary>
        /// <param name="dictRowCalculation"></param>
        /// <returns></returns>
        public static string GetCustomSerializedRowCalculationDictionaryData(Dictionary<string, RowCalculationBaseNav> dictRowCalculation)
        {
            string serializedData = string.Empty;
            try
            {
                if (dictRowCalculation == null || dictRowCalculation.Count == 0)
                {
                    return "{}";
                }

                serializedData = "{";
                foreach (KeyValuePair<string, RowCalculationBaseNav> rowCalculation in dictRowCalculation)
                {
                    string rowCalculationSerializedData = string.Empty;
                    rowCalculationSerializedData = rowCalculationSerializedData + JsonConvert.SerializeObject(rowCalculation.Key) + ":";
                    rowCalculationSerializedData = rowCalculationSerializedData + JsonConvert.SerializeObject(rowCalculation.Value);
                    string dynamicUDAJson = JsonConvert.SerializeObject(rowCalculation.Value._dynamicUDAs);
                    rowCalculationSerializedData = rowCalculationSerializedData.Substring(0, rowCalculationSerializedData.Length - 1) + ","
                        + dynamicUDAJson.Substring(1, dynamicUDAJson.Length - 1) + ",";

                    serializedData = serializedData + rowCalculationSerializedData;
                }
                serializedData = serializedData.Substring(0, serializedData.Length - 1) + "}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return serializedData;
        }

        internal static List<string> DynamicUDAKeys = new List<string>();
        #region Class Members

        // Cache the properties of the class once
        private static readonly PropertyInfo[] Properties = typeof(RowCalculationBaseNav).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private static readonly PropertyInfo[] BaseProperties = typeof(RowCalculationBaseNav).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

        private string _underlyingSymbol;
        public string UndlSym
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private string _bloombergSymbol = string.Empty;
        public string BBGSym
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        private string _liveFeedProviderTicker = string.Empty;
        public string LiveFTick
        {
            get { return _liveFeedProviderTicker; }
            set { _liveFeedProviderTicker = value; }
        }

        private string _securityName;
        public string SecN
        {
            get { return _securityName; }
            set { _securityName = value; }
        }

        private string _accountShortName;
        public string AccShrtName
        {
            get { return _accountShortName; }
            set { _accountShortName = value; }
        }

        private string _masterFundName;
        public string MFName
        {
            get { return _masterFundName; }
            set { _masterFundName = value; }
        }

        private string _asset;
        public string Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        private string _defaultPositionSide;
        public string PosSide
        {
            get { return _defaultPositionSide; }
            set { _defaultPositionSide = value; }
        }

        private string _exposurePositionSide;
        public string ExpPosSide
        {
            get { return _exposurePositionSide; }
            set { _exposurePositionSide = value; }
        }

        private string _fxConversionMethod;
        public string FxConvM
        {
            get { return _fxConversionMethod; }
            set { _fxConversionMethod = value; }
        }

        private string _orderSide;
        public string OrdSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        private int _strategyId;
        public int StratId
        {
            get { return _strategyId; }
            set { _strategyId = value; }
        }

        private string _strategyName;
        public string Strategy
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }

        #region Symbol level tags

        private string _udaAssetClass;
        public string UDAAsset
        {
            get { return _udaAssetClass; }
            set { _udaAssetClass = value; }
        }

        private string _udaSector;
        public string UDASector
        {
            get { return _udaSector; }
            set { _udaSector = value; }
        }

        private string _udaSubSector;
        public string UDASubSect
        {
            get { return _udaSubSector; }
            set { _udaSubSector = value; }
        }

        private string _udaSecurityType;
        public string UDASecType
        {
            get { return _udaSecurityType; }
            set { _udaSecurityType = value; }
        }

        private string _udaCountry;
        public string UDACountry
        {
            get { return _udaCountry; }
            set { _udaCountry = value; }
        }

        private string _currency;
        public string Ccy
        {
            get { return _currency; }
            set { _currency = value; }
        }

        #region Dynamic UDA 

        private Dictionary<string, string> _dynamicUDAs = new Dictionary<string, string>();

        #endregion

        #region Trade Attribues

        private string _tradeAttribute1 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = RtpnlConstants.CONST_Dash;
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        #endregion

        #endregion

        #endregion
    }
}
