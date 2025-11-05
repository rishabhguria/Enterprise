using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Analytics
{
    public class CentralRiskPositionsManager
    {
        private Dictionary<int, string> _requestIDHashList = new Dictionary<int, string>();
        public event EventHandler<EventArgs<List<PranaPosition>>> PositionReceived;
        static CentralRiskPositionsManager _centralRiskPositionsManager = null;
        IPostTradeServices _postTradeServices = null;
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        delegate void AsyncInvokeDelegate(Delegate del, params object[] args);
        Dictionary<string, double> _symbolWiseBeta = new Dictionary<string, double>();
        Dictionary<int, DateTime> _currentOffsetAdjustedAUECDates = new Dictionary<int, DateTime>();

        static ProxyBase<IPranaPositionServices> _positionManagementServices = null;

        private DuplexProxyBase<IPricingService> _pricingServiceProxy;
        public DuplexProxyBase<IPricingService> PricingServiceProxy
        {
            set { _pricingServiceProxy = value; }
        }

        private static void CreatePositionManagementProxy()
        {
            _positionManagementServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
        }

        public static CentralRiskPositionsManager GetInstance
        {
            get
            {
                if (_centralRiskPositionsManager == null)
                {
                    _centralRiskPositionsManager = new CentralRiskPositionsManager();
                    if (_positionManagementServices != null)
                    {
                        _positionManagementServices.Dispose();
                    }
                    CreatePositionManagementProxy();
                }
                return _centralRiskPositionsManager;
            }
        }

        #region Public Functions
        public void Setup(IPostTradeServices postTradeServices)
        {
            try
            {
                _postTradeServices = postTradeServices;
                _postTradeServices.MessageReceived += new EventHandler<EventArgs<QueueMessage>>(_postTradeServices_MessageReceived);
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
        }

        public async Task<PranaPositionWithGreekColl> GetPositionsAsRiskPref()
        {
            PranaPositionWithGreekColl positions = new PranaPositionWithGreekColl();
            try
            {
                List<string> listGrouping = RiskPreferenceManager.RiskPrefernece.Grouping;

                //Accruals Data Needed based on configuration
                bool isAccrualsNeeded = bool.Parse(ConfigurationManager.AppSettings["IsAccrualsNeededOnRisk"]);
                bool isUnallocatedTradesPermitted = false;
                if (CachedDataManager.GetInstance.GetAllAccountsCount() == CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                {
                    isUnallocatedTradesPermitted = true;
                }
                GenericPositionData genericPositionData = await _positionManagementServices.InnerChannel.GetGroupedPositionsAndTransactions(listGrouping, DateTime.UtcNow, null, GetCommaSepratedPermittedAccounts().ToString(), string.Empty, string.Empty, false, true, isAccrualsNeeded, true, true, true, isUnallocatedTradesPermitted).ConfigureAwait(false);
                List<TaxLot> taxlots = genericPositionData.Positions;
                _symbolWiseBeta = genericPositionData.SymbolWiseBeta;
                _currentOffsetAdjustedAUECDates = genericPositionData.CurrentOffsetAdjustedAUECDates;
                //Changed the Quantity type from int to double
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4339
                Dictionary<string, double> totalQuantityPortfolio = new Dictionary<string, double>();
                Dictionary<string, double> totalQuantityExposurePortfolio = new Dictionary<string, double>();
                foreach (TaxLot taxlot in taxlots)
                {
                    PranaPositionWithGreeks position = CreatePranaGreekPositionFromPosition(SetDefaultDelta(taxlot));
                    if (totalQuantityPortfolio.ContainsKey(taxlot.Symbol))
                    {
                        totalQuantityPortfolio[taxlot.Symbol] += taxlot.Quantity;
                    }
                    else
                    {
                        totalQuantityPortfolio.Add(taxlot.Symbol, taxlot.Quantity);
                    }

                    //Changed the Quantity type from int to double
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-4339
                    double newQuantity = 0;
                    if (taxlot.PutOrCall == 0)
                    {
                        newQuantity = taxlot.Quantity * Math.Sign(taxlot.UnderlyingDelta) * (-1);
                    }
                    else if (taxlot.PutOrCall == 1)
                    {
                        newQuantity = taxlot.Quantity * Math.Sign(taxlot.UnderlyingDelta);
                    }
                    else
                    {
                        newQuantity = taxlot.Quantity * Math.Sign(taxlot.Delta);
                    }
                    if (totalQuantityExposurePortfolio.ContainsKey(taxlot.Symbol))
                    {
                        totalQuantityExposurePortfolio[taxlot.Symbol] += newQuantity;
                    }
                    else
                    {
                        totalQuantityExposurePortfolio.Add(taxlot.Symbol, newQuantity);
                    }
                    positions.Add(position);
                }
                foreach (PranaPositionWithGreeks position in positions)
                {
                    if (totalQuantityPortfolio.ContainsKey(position.Symbol))
                    {
                        if (totalQuantityPortfolio[position.Symbol] >= 0)
                        {
                            position.PositionSideMVInPortfolio = PositionTag.Long.ToString();
                        }
                        else if (totalQuantityPortfolio[position.Symbol] < 0)
                        {
                            position.PositionSideMVInPortfolio = PositionTag.Short.ToString();
                        }
                    }
                    if (totalQuantityExposurePortfolio.ContainsKey(position.Symbol))
                    {
                        if (totalQuantityExposurePortfolio[position.Symbol] >= 0)
                        {
                            position.PositionSideExposureInPortfolio = PositionTag.Long.ToString();
                        }
                        else if (totalQuantityExposurePortfolio[position.Symbol] < 0)
                        {
                            position.PositionSideExposureInPortfolio = PositionTag.Short.ToString();
                        }
                    }
                }
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
            return positions;
        }

        private TaxLot SetDefaultDelta(TaxLot taxlot)
        {
            try
            {
                if (taxlot.AssetID.Equals((int)AssetCategory.EquityOption) || taxlot.AssetID.Equals((int)AssetCategory.FutureOption) || taxlot.AssetID.Equals((int)AssetCategory.FXOption))
                {
                    if (taxlot.ExpirationDate.Date.Subtract(DateTime.Now.Date).Days <= 0)
                    {
                        taxlot.Delta = 0;
                    }
                    else
                    {
                        if (taxlot.PutOrCall == 1)
                        {
                            taxlot.Delta = 1;
                        }
                        else
                        {
                            taxlot.Delta = -1;
                        }
                    }
                }
                else
                {
                    taxlot.Delta = 1;
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
            return taxlot;
        }

        private PranaRiskObjColl ConvertRiskObjFromTaxlot(List<TaxLot> taxlots)
        {
            PranaRiskObjColl riskObjColl = null;
            try
            {
                riskObjColl = new PranaRiskObjColl();
                foreach (TaxLot taxlot in taxlots)
                {
                    PranaRiskObj riskObj = new PranaRiskObj();
                    riskObj.CopyBasicDetails(taxlot);
                    FillDetails(riskObj, taxlot);
                    riskObjColl.Add(riskObj);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return riskObjColl;
        }

        /// <summary>
        /// Bharat (24 December 2013)
        /// </summary>
        /// <param name="needPositionsWithAccountSymbolGrouping">If cash included in MarketValue then taxlots must requires atleast Account-Symbol Grouping</param>
        /// <param name="dictAccountWiseCash">AccountWise Cash with/without Accruals</param>
        /// <returns></returns>
        public async Task<List<object>> GetPSPositionsAsRiskPref(bool needPositionsWithAccountSymbolGrouping, Dictionary<int, double> dictAccountWiseCash)
        {
            List<object> riskData = new List<object>();
            PranaRiskObjColl riskObjColl = null;
            try
            {
                //Clone Grouping List for maintaining old preferences of grouping
                List<string> listGrouping = new List<string>();
                List<TaxLot> riskPositions = new List<TaxLot>();

                if (needPositionsWithAccountSymbolGrouping)
                {
                    foreach (string grouping in RiskPreferenceManager.RiskPrefernece.Grouping)
                    {
                        listGrouping.Add((string)grouping.Clone());
                    }
                    if (!listGrouping.Contains("Fund"))
                    {
                        listGrouping.Add("Fund");
                    }

                    //Accruals Data Needed based on configuration
                    bool isAccrualsNeeded = bool.Parse(ConfigurationManager.AppSettings["IsAccrualsNeededOnRisk"]);
                    bool isUnallocatedTradesPermitted = false;
                    if (CachedDataManager.GetInstance.GetAllAccountsCount() == CachedDataManager.GetInstance.GetAllAccountIDsForUser().Count)
                    {
                        isUnallocatedTradesPermitted = true;
                    }
                    GenericPositionData genericPositionData = await _positionManagementServices.InnerChannel.GetGroupedPositionsAndTransactions(listGrouping, DateTime.UtcNow, null, GetCommaSepratedPermittedAccounts().ToString(), string.Empty, string.Empty, false, true, isAccrualsNeeded, true, false, false, isUnallocatedTradesPermitted).ConfigureAwait(false);
                    riskPositions = genericPositionData.Positions;

                    //Adding Accountwise cash and Accountwise accruals
                    dictAccountWiseCash = AddCashAndAccruals(genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseCash, genericPositionData.GenericDayEndDataValue.StartOfDayAccountWiseAccruals);
                }
                else
                {
                    listGrouping = RiskPreferenceManager.RiskPrefernece.Grouping;
                    riskPositions = await _positionManagementServices.InnerChannel.GetGroupedPositions(listGrouping, DateTime.UtcNow, true, GetCommaSepratedPermittedAccounts().ToString()).ConfigureAwait(false);
                }
                Dictionary<string, PSSymbolRequestObject> psReqObjectList = new Dictionary<string, PSSymbolRequestObject>();
                Dictionary<string, SymbolData> dictSymbolData = new Dictionary<string, SymbolData>();
                foreach (TaxLot taxlot in riskPositions)
                {
                    SymbolData symbolData = GetDynamicSymbolData(taxlot.Symbol);
                    if (symbolData != null && !dictSymbolData.ContainsKey(symbolData.Symbol))
                    {
                        dictSymbolData.Add(symbolData.Symbol, symbolData);
                        if (!psReqObjectList.ContainsKey(taxlot.Symbol))
                        {
                            psReqObjectList.Add(taxlot.Symbol, new PSSymbolRequestObject(SetDefaultDelta(taxlot), symbolData.FinalImpliedVol));
                        }
                        if ((taxlot.AssetID.Equals((int)AssetCategory.EquityOption) || taxlot.AssetID.Equals((int)AssetCategory.FutureOption) || taxlot.AssetID.Equals((int)AssetCategory.FXOption)) && !psReqObjectList.ContainsKey(taxlot.UnderlyingSymbol))
                        {
                            SymbolData underlyingSymbolData = GetDynamicSymbolData(taxlot.UnderlyingSymbol);
                            if (underlyingSymbolData != null)
                            {
                                TaxLot underlyingTaxlot = new TaxLot();
                                underlyingTaxlot.Symbol = underlyingSymbolData.Symbol;
                                underlyingTaxlot.UnderlyingSymbol = underlyingSymbolData.UnderlyingSymbol;
                                underlyingTaxlot.AUECID = underlyingSymbolData.AUECID;
                                underlyingTaxlot.ExchangeID = underlyingSymbolData.ExchangeID;

                                if (taxlot.AssetID.Equals((int)AssetCategory.EquityOption))
                                {
                                    underlyingTaxlot.AssetID = (int)AssetCategory.Equity;
                                }
                                else if (taxlot.AssetID.Equals((int)AssetCategory.FutureOption))
                                {
                                    underlyingTaxlot.AssetID = (int)AssetCategory.Future;
                                    underlyingTaxlot.ExpirationDate = underlyingSymbolData.ExpirationDate;
                                }
                                else if (taxlot.AssetID.Equals((int)AssetCategory.FXOption))
                                {
                                    underlyingTaxlot.AssetID = (int)AssetCategory.FX;
                                }

                                psReqObjectList.Add(taxlot.UnderlyingSymbol, new PSSymbolRequestObject(SetDefaultDelta(underlyingTaxlot), underlyingSymbolData.FinalImpliedVol));
                            }
                        }
                    }
                }

                Dictionary<string, string> psSymbolResponse = await _positionManagementServices.InnerChannel.GetAllPSSymbolsforRisk(psReqObjectList.Values.ToList());
                riskObjColl = ConvertRiskObjFromTaxlot(riskPositions);

                foreach (PranaRiskObj riskObj in riskObjColl)
                {
                    if (dictSymbolData.ContainsKey(riskObj.Symbol))
                    {
                        riskObj.Delta = dictSymbolData[riskObj.Symbol].Delta;
                        riskObj.DeltaAdjPosition = OptionGreekCalculater.CalculateNetDeltaAdjPosition(riskObj.Quantity, riskObj.Delta, riskObj.AssetID, riskObj.SideMultiplier, riskObj.ContractMultiplier);
                        //We are not calculating Implied Vol for 0 or negative strike prices, for these cases GetDynamicSymbolData returns -1 implied volatility.
                        if (riskObj.AssetID.Equals((int)AssetCategory.EquityOption) || riskObj.AssetID.Equals((int)AssetCategory.FutureOption) || riskObj.AssetID.Equals((int)AssetCategory.FXOption) && riskObj.StrikePrice > 0)
                        {
                            riskObj.Volatility = dictSymbolData[riskObj.Symbol].FinalImpliedVol;
                        }
                    }

                    if (psSymbolResponse.ContainsKey(riskObj.Symbol))
                        riskObj.PSSymbol = psSymbolResponse[riskObj.Symbol];
                    else
                        riskObj.PSSymbol = riskObj.Symbol;

                    if (psSymbolResponse.ContainsKey(riskObj.UnderlyingSymbol))
                        riskObj.UnderlyingPSSymbol = psSymbolResponse[riskObj.UnderlyingSymbol];
                    else
                        riskObj.UnderlyingPSSymbol = riskObj.UnderlyingSymbol;
                }
                riskData.Add(riskObjColl);
                if (dictAccountWiseCash == null)
                {
                    dictAccountWiseCash = new Dictionary<int, double>();
                }
                riskData.Add(dictAccountWiseCash);
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
            return riskData;
        }

        public StringBuilder GetCommaSepratedPermittedAccounts()
        {
            List<string> accountIdForLoggedInUser = CachedDataManager.GetInstance.GetAllAccountIDsForUser();

            StringBuilder commaSepratedAccounts = new StringBuilder();
            foreach (var accountId in accountIdForLoggedInUser)
            {
                commaSepratedAccounts.Append(accountId);
                commaSepratedAccounts.Append(",");
            }
            if (commaSepratedAccounts.Length > 0)
                commaSepratedAccounts.Remove((commaSepratedAccounts.Length - 1), 1);

            return commaSepratedAccounts;
        }

        public SymbolData GetDynamicSymbolData(string symbol)
        {
            if (_pricingServiceProxy != null)
                return _pricingServiceProxy.InnerChannel.GetDynamicSymbolData(symbol);
            return null;
        }

        public SymbolData GetDynamicSymbolData(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode)
        {
            if (_pricingServiceProxy != null)
                return _pricingServiceProxy.InnerChannel.GetDynamicSymbolData(symbol, fromCurrency, toCurrency, categoryCode);
            return null;
        }

        public Dictionary<string, string> GetPSSymbols(List<PSSymbolRequestObject> PSReqObjectList)
        {
            Dictionary<string, string> PSSymbolresponse = new Dictionary<string, string>();
            try
            {
                PSSymbolresponse = _positionManagementServices.InnerChannel.GetAllPSSymbols(PSReqObjectList);
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
            return PSSymbolresponse;
        }

        /// <summary>
        /// Bharat (24 February 2014)
        /// </summary>
        /// <param name="accountWiseCash">AccountWise StartOfDayCash</param>
        /// <param name="accountCurrencyWiseAccruals">AccountWise Accruals</param>
        /// <returns>Merged Dictionary of AccountWise Cash and AccountWise Accruals</returns>
        public static Dictionary<int, double> AddCashAndAccruals(Dictionary<int, double> accountWiseCash, Dictionary<int, Dictionary<int, double>> accountCurrencyWiseAccruals)
        {
            Dictionary<int, double> finalAccountWiseCash = new Dictionary<int, double>();
            try
            {
                if (accountWiseCash != null && accountWiseCash.Count > 0 && accountCurrencyWiseAccruals != null && accountCurrencyWiseAccruals.Count > 0)
                {
                    finalAccountWiseCash = accountWiseCash;
                    foreach (KeyValuePair<int, Dictionary<int, double>> kvp in accountCurrencyWiseAccruals)
                    {
                        double totalAccrualsForAccount = GetSODAccrualsInBase(kvp.Value, kvp.Key);
                        if (finalAccountWiseCash.ContainsKey(kvp.Key))
                        {
                            finalAccountWiseCash[kvp.Key] += totalAccrualsForAccount;
                        }
                        else
                        {
                            finalAccountWiseCash.Add(kvp.Key, totalAccrualsForAccount);
                        }
                    }
                }
                else if (accountWiseCash != null && accountWiseCash.Count > 0)
                {
                    return accountWiseCash;
                }
                else if (accountCurrencyWiseAccruals != null && accountCurrencyWiseAccruals.Count > 0)
                {
                    return getAccountWiseAccruals(accountCurrencyWiseAccruals);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return finalAccountWiseCash;
        }

        private static Dictionary<int, double> getAccountWiseAccruals(Dictionary<int, Dictionary<int, double>> accountCurrencyWiseAccruals)
        {
            Dictionary<int, double> accountWiseAccruals = new Dictionary<int, double>();
            try
            {
                foreach (KeyValuePair<int, Dictionary<int, double>> kvp in accountCurrencyWiseAccruals)
                {
                    double totalAccrualsForAccount = GetSODAccrualsInBase(kvp.Value, kvp.Key);
                    if (accountWiseAccruals.ContainsKey(kvp.Key))
                    {
                        accountWiseAccruals[kvp.Key] += totalAccrualsForAccount;
                    }
                    else
                    {
                        accountWiseAccruals.Add(kvp.Key, totalAccrualsForAccount);
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
            return accountWiseAccruals;
        }
        static int _companyId = CachedDataManager.GetInstance.GetCompanyID();
        private static double GetSODAccrualsInBase(Dictionary<int, double> fundWiseAccrualsinLocalCurrency, int accountId)
        {
            double fundWiseAccrualsInBaseCurrency = 0;
            try
            {
                foreach (KeyValuePair<int, double> localCash in fundWiseAccrualsinLocalCurrency)
                {
                    int accountBaseCurrency;
                    double yesterdayFxRate = 1;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountId))
                    {
                        accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountId];
                    }
                    else
                    {
                        accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    if (localCash.Key != accountBaseCurrency)
                    {
                        ConversionRate yesterdaysConversionRate = ForexConverter.GetInstance(_companyId).GetConversionRateForCurrencyToBaseCurrency(localCash.Key, DateTime.Now.AddDays(-1), accountId);
                        if (yesterdaysConversionRate != null)
                        {
                            if (yesterdaysConversionRate.ConversionMethod == Operator.M)
                            {
                                yesterdayFxRate = yesterdaysConversionRate.RateValue;
                            }
                            else if (yesterdaysConversionRate.RateValue != 0 && yesterdaysConversionRate.ConversionMethod == Operator.D)
                            {
                                yesterdayFxRate = (1 / yesterdaysConversionRate.RateValue);
                            }
                        }
                        fundWiseAccrualsInBaseCurrency += localCash.Value * yesterdayFxRate;
                    }
                    else
                    {
                        fundWiseAccrualsInBaseCurrency += localCash.Value * 1;
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
            return fundWiseAccrualsInBaseCurrency;
        }

        public void Refresh()
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_REFRESH_POSITIONS, string.Empty);
                _postTradeServices.SendMessage(qMsg);
                _isBusy = true;
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
        }

        void _postTradeServices_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                switch (e.Value.MsgType)
                {
                    case CustomFIXConstants.MSG_GET_POSITIONS:
                        List<PranaPosition> listPos = (List<PranaPosition>)binaryFormatter.DeSerialize(e.Value.Message.ToString());
                        SendPositionsToRequestedModule(listPos, e.Value.RequestID);
                        break;
                }
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
        }

        private void FillDetails(PranaRiskObj obj, TaxLot taxlot)
        {
            obj.StrikePrice = taxlot.StrikePrice;
            obj.SideMultiplier = taxlot.SideMultiplier;
            obj.SecurityTypeName = taxlot.SecurityTypeName;
            obj.SectorName = taxlot.SectorName;
            obj.SubSectorName = taxlot.SubSectorName;
            obj.CountryName = taxlot.CountryName;
            obj.UDAAsset = taxlot.UDAAsset;
            obj.PositionType = taxlot.PositionType;
            obj.MasterFund = taxlot.MasterFund;
            obj.Analyst = taxlot.Analyst;
            obj.CountryOfRisk = taxlot.CountryOfRisk;
            obj.CustomUDA1 = taxlot.CustomUDA1;
            obj.CustomUDA2 = taxlot.CustomUDA2;
            obj.CustomUDA3 = taxlot.CustomUDA3;
            obj.CustomUDA4 = taxlot.CustomUDA4;
            obj.CustomUDA5 = taxlot.CustomUDA5;
            obj.CustomUDA6 = taxlot.CustomUDA6;
            obj.CustomUDA7 = taxlot.CustomUDA7;
            obj.Issuer = taxlot.Issuer;
            obj.LiquidTag = taxlot.LiquidTag;
            obj.MarketCap = taxlot.MarketCap;
            obj.Region = taxlot.Region;
            obj.RiskCurrency = taxlot.RiskCurrency;
            obj.UcitsEligibleTag = taxlot.UcitsEligibleTag;
        }

        public PranaPositionWithGreeks CreatePranaGreekPositionFromPosition(TaxLot pos)
        {
            PranaPositionWithGreeks pranaGreekPos = new PranaPositionWithGreeks();
            pranaGreekPos.CopyBasicDetails(pos);
            pranaGreekPos.AssetCategoryValue = pos.AssetCategoryValue;
            pranaGreekPos.Symbol = pos.Symbol;
            pranaGreekPos.OrderSideTagValue = pos.OrderSideTagValue;
            pranaGreekPos.Quantity = pos.Quantity;
            pranaGreekPos.TaxLotQty = pos.TaxLotQty;
            pranaGreekPos.Level2Name = pos.Level2Name;
            pranaGreekPos.Level1Name = pos.Level1Name;
            pranaGreekPos.UDAAsset = pos.UDAAsset;
            pranaGreekPos.SecurityTypeName = pos.SecurityTypeName;
            pranaGreekPos.SectorName = pos.SectorName;
            pranaGreekPos.SubSectorName = pos.SubSectorName;
            pranaGreekPos.CountryName = pos.CountryName;
            pranaGreekPos.AssetID = pos.AssetID;
            pranaGreekPos.AssetName = pos.AssetName;
            pranaGreekPos.AvgPrice = pos.AvgPrice;
            pranaGreekPos.CompanyName = pos.CompanyName;
            pranaGreekPos.ContractMultiplier = pos.ContractMultiplier;
            pranaGreekPos.Delta = pos.Delta;
            pranaGreekPos.PositionType = pos.PositionType;
            pranaGreekPos.UnderlyingSymbol = pos.UnderlyingSymbol;
            pranaGreekPos.SideMultiplier = (pos.Quantity >= 0) ? 1 : -1;
            pranaGreekPos.ExpirationDate = pos.ExpirationDate;
            pranaGreekPos.ExpirationMonth = pos.ExpirationDate;
            pranaGreekPos.ExpirationMonth = new DateTime(pos.ExpirationDate.Year, pos.ExpirationDate.Month, 01);
            pranaGreekPos.CurrencyID = pos.CurrencyID;
            pranaGreekPos.ExchangeName = pos.ExchangeName;
            pranaGreekPos.UnderlyingName = pos.UnderlyingName;
            pranaGreekPos.CurrencyName = pos.CurrencyName;
            pranaGreekPos.CompanyUserName = pos.CompanyUserName;
            pranaGreekPos.CounterPartyName = pos.CounterPartyName;
            pranaGreekPos.StrikePrice = pos.StrikePrice;
            pranaGreekPos.PutOrCall = pos.PutOrCall;
            pranaGreekPos.AUECLocalDate = pos.AUECLocalDate;
            pranaGreekPos.DaysToExpiration = GetDaysToExpirationForOption(pranaGreekPos);
            pranaGreekPos.LeadCurrencyID = pos.LeadCurrencyID;
            pranaGreekPos.VsCurrencyID = pos.VsCurrencyID;
            pranaGreekPos.MasterFund = pos.MasterFund;
            pranaGreekPos.ProxySymbol = pos.ProxySymbol;
            pranaGreekPos.TradeAttribute1 = pos.TradeAttribute1;
            pranaGreekPos.TradeAttribute2 = pos.TradeAttribute2;
            pranaGreekPos.TradeAttribute3 = pos.TradeAttribute3;
            pranaGreekPos.TradeAttribute4 = pos.TradeAttribute4;
            pranaGreekPos.TradeAttribute5 = pos.TradeAttribute5;
            pranaGreekPos.TradeAttribute6 = pos.TradeAttribute6;

            if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
            {
                SymbolData symbolData = GetDynamicSymbolData(pos.Symbol);

                if (symbolData != null)
                {
                    pranaGreekPos.SelectedFeedPrice = symbolData.SelectedFeedPrice;
                    GetFXRate(pranaGreekPos);
                    pranaGreekPos.SelectedFeedPriceInBaseCurrency = symbolData.SelectedFeedPrice * pranaGreekPos.FXRate;
                    pranaGreekPos.PricingSource = EnumHelper.GetDescription(symbolData.PricingSource);
                }
            }

            switch (pos.AssetCategoryValue)
            {
                case AssetCategory.Option:
                case AssetCategory.FXOption:
                case AssetCategory.FutureOption:
                case AssetCategory.EquityOption:
                    pranaGreekPos.Beta = GetBetaFromSymbol(pos.UnderlyingSymbol);
                    break;
                default:
                    pranaGreekPos.Beta = GetBetaFromSymbol(pos.Symbol);
                    break;
            }
            pranaGreekPos.BetaAdjExposure = pranaGreekPos.DeltaAdjExposure * pranaGreekPos.Beta;
            pranaGreekPos.Analyst = pos.Analyst;
            pranaGreekPos.CountryOfRisk = pos.CountryOfRisk;
            pranaGreekPos.CustomUDA1 = pos.CustomUDA1;
            pranaGreekPos.CustomUDA2 = pos.CustomUDA2;
            pranaGreekPos.CustomUDA3 = pos.CustomUDA3;
            pranaGreekPos.CustomUDA4 = pos.CustomUDA4;
            pranaGreekPos.CustomUDA5 = pos.CustomUDA5;
            pranaGreekPos.CustomUDA6 = pos.CustomUDA6;
            pranaGreekPos.CustomUDA7 = pos.CustomUDA7;
            pranaGreekPos.Issuer = pos.Issuer;
            pranaGreekPos.LiquidTag = pos.LiquidTag;
            pranaGreekPos.MarketCap = pos.MarketCap;
            pranaGreekPos.Region = pos.Region;
            pranaGreekPos.RiskCurrency = pos.RiskCurrency;
            pranaGreekPos.UcitsEligibleTag = pos.UcitsEligibleTag;
            pranaGreekPos.AvgPriceInBaseCurrency = pranaGreekPos.AvgPrice * pranaGreekPos.FXRate;
            return pranaGreekPos;
        }

        private void GetFXRate(PranaPositionWithGreeks pranaGreekPos)
        {
            SymbolData fxLevel1Data = null;

            int accountBaseCurrency;
            if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(pranaGreekPos.Level1ID))
            {
                accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[pranaGreekPos.Level1ID];
            }
            else
            {
                accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
            }
            if (pranaGreekPos.CurrencyID != accountBaseCurrency)
            {
                string forexSymbol = ForexConverter.GetInstance(CachedDataManager.GetInstance.LoggedInUser.CompanyID).GetPranaForexSymbolFromCurrencies(pranaGreekPos.CurrencyID, accountBaseCurrency);
                fxLevel1Data = GetDynamicSymbolData(forexSymbol, pranaGreekPos.CurrencyID, accountBaseCurrency, AssetCategory.Forex);

                DateTime latestAUECDate = DateTime.Now;
                if (_currentOffsetAdjustedAUECDates != null && _currentOffsetAdjustedAUECDates.ContainsKey(pranaGreekPos.AUECID))
                {
                    latestAUECDate = _currentOffsetAdjustedAUECDates[pranaGreekPos.AUECID];
                }
                if (fxLevel1Data != null)
                {
                    pranaGreekPos.FXRate = fxLevel1Data.SelectedFeedPrice;
                }
                else
                {
                    ConversionRate todaysConversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.LoggedInUser.CompanyID).GetConversionRateForCurrencyToBaseCurrency(pranaGreekPos.CurrencyID, latestAUECDate, pranaGreekPos.Level1ID);
                    if (todaysConversionRate != null)
                    {
                        if (todaysConversionRate.ConversionMethod == Operator.M)
                        {
                            pranaGreekPos.FXRate = todaysConversionRate.RateValue;
                        }
                        else if (todaysConversionRate.RateValue != 0 && todaysConversionRate.ConversionMethod == Operator.D)
                        {
                            pranaGreekPos.FXRate = (1 / todaysConversionRate.RateValue);
                        }
                    }
                }
            }
            else
            {
                pranaGreekPos.FXRate = 1;
            }
        }

        private double GetBetaFromSymbol(string symbol)
        {
            try
            {
                if (_symbolWiseBeta.ContainsKey(symbol))
                {
                    return _symbolWiseBeta[symbol];
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
            return 1;
        }
        #endregion

        #region Private & Help Functions
        public int GetDaysToExpirationForOption(PranaPositionWithGreeks posGreek)
        {
            int baseAssetID = Mapper.GetBaseAsset(posGreek.AssetID);
            int daysToExpiration = 0;
            switch (baseAssetID)
            {
                case (int)AssetCategory.Option:
                    if (posGreek.ExpirationDate > DateTime.Now)
                    {
                        TimeSpan dateDiff = posGreek.ExpirationDate.Date - DateTime.Now.Date;
                        daysToExpiration = dateDiff.Days;
                    }
                    break;

                default:
                    break;
            }
            return daysToExpiration;
        }

        private void SendPositionsToRequestedModule(List<PranaPosition> posList, string msgID)
        {
            try
            {
                if (PositionReceived != null)
                {
                    Delegate[] subscriberList = PositionReceived.GetInvocationList();
                    AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                    foreach (Delegate subscriber in subscriberList)
                    {
                        int subscriberHashCode = subscriber.Target.GetHashCode();
                        if (_requestIDHashList.ContainsKey(subscriberHashCode)
                            && msgID == _requestIDHashList[subscriberHashCode])
                        {
                            invoker.BeginInvoke(subscriber, new object[1] { posList }, null, null);
                            _requestIDHashList.Remove(subscriberHashCode);
                        }
                    }
                }
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
        }

        private static void InvokeDelegate(Delegate sink, params object[] args)
        {
            try
            {
                sink.DynamicInvoke(args);
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
        }
        #endregion

        public bool IsTradeServerConnected
        {
            get { return _postTradeServices.IsConnected; }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; }
        }
    }
}
