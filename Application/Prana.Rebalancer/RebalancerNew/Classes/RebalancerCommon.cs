using Infragistics.Win;
using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.Rebalancer.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    public class RebalancerCommon : IDisposable
    {
        #region Singleton

        private static volatile RebalancerCommon instance;
        private static object syncRoot = new Object();
        private RebalancerCommon()
        {
            WireEvent();
        }

        public static RebalancerCommon Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RebalancerCommon();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        private MarketDataHelper _marketDataHelperInstance;
        private System.Windows.Threading.DispatcherTimer _timerSnapShot;
        private List<string> _listRequestedSymbols = new List<string>();
        private List<string> _listOfNewSymbols = new List<string>();
        private bool _isSymbolValidated = false;
        Dictionary<string, decimal> _dictSymbolTargetPercentage = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> _dictSymbolWithPrice = new Dictionary<string, decimal>();
        private bool _isRealTimePositions = true;
        public List<CashFlowToCompliance> cashFlow = new List<CashFlowToCompliance>();
        private Dictionary<int, AllocationOperationPreference> _allocationPreferences;
        Dictionary<string, decimal> _dictSymbolTolerancePercentage = new Dictionary<string, decimal>();

        private void WireEvent()
        {
            _marketDataHelperInstance = MarketDataHelper.GetInstance();
            _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
            _timerSnapShot = new DispatcherTimer();
            _timerSnapShot.Tick += new EventHandler(_timerSnapShot_Tick);
        }

        private void _timerSnapShot_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_isSymbolValidated)
                {
                    if (_timerSnapShot != null)
                        _timerSnapShot.Stop();
                    if (_listOfNewSymbols.Count > 0)
                    {
                        List<ModelPortfolioSecurityDto> lstModelPortfolioSecurities = new List<ModelPortfolioSecurityDto>();
                        Dictionary<string, SecMasterBaseObj> secMasterSymbolData = SecMasterSyncServicesConnector.GetInstance().GetSecMasterSymbolData(_listOfNewSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol);
                        foreach (KeyValuePair<string, SecMasterBaseObj> kvp in secMasterSymbolData)
                        {
                            decimal price = 0;
                            if (_dictSymbolWithPrice.ContainsKey(kvp.Key))
                                price = _dictSymbolWithPrice[kvp.Key];

                            decimal tergetPercentage = 0;
                            if (_dictSymbolTargetPercentage.ContainsKey(kvp.Key))
                                tergetPercentage = _dictSymbolTargetPercentage[kvp.Key];

                            ModelPortfolioSecurityDto modelPortfolioSecurityDto = new ModelPortfolioSecurityDto()
                            {
                                AUECID = kvp.Value.AUECID,
                                RoundLot = kvp.Value.RoundLot,
                                Symbol = kvp.Key,
                                BloombergSymbol = kvp.Value.BloombergSymbol,
                                FactSetSymbol = kvp.Value.FactSetSymbol,
                                ActivSymbol = kvp.Value.ActivSymbol,
                                Asset = kvp.Value.AssetCategory.ToString(),
                                Delta = (decimal)kvp.Value.Delta,
                                LeveragedFactor = 1,
                                Multiplier = (decimal)kvp.Value.Multiplier,
                                Price = price,
                                FXRate = 1,
                                Sector = kvp.Value.Sector,
                                TargetPercentage = tergetPercentage,
                                BloombergSymbolWithExchangeCode = kvp.Value.BloombergSymbolWithExchangeCode
                            };
                            modelPortfolioSecurityDto.FXRate = RebalancerCache.Instance.GetSymbolFx(kvp.Key);
                            lstModelPortfolioSecurities.Add(modelPortfolioSecurityDto);
                            result.Add(modelPortfolioSecurityDto);
                        }
                        autoReset.Set();
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

        /// <summary>
        /// Handles the OnResponse event of the LOne control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arg">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                if (Dispatcher.CurrentDispatcher.CheckAccess())
                {
                    if (args != null)
                    {
                        SymbolData data = args.Value;
                        if (data != null)
                        {
                            onL1Response(data);
                        }
                    }
                }
                else
                {
                    Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                    {
                        LOne_OnResponse(sender, args);
                    }));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ons the l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private void onL1Response(SymbolData l1Data)
        {
            try
            {
                if (l1Data != null)
                {
                    if (_listRequestedSymbols.Contains(l1Data.Symbol))
                    {
                        _listRequestedSymbols.Remove(l1Data.Symbol);
                    }

                    decimal realTimePrice = 0;
                    string prefValue = RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalPricingFeld, 0);

                    SelectedFeedPrice enumName;
                    if (Enum.TryParse(prefValue, out enumName))
                    {
                        switch (enumName)
                        {
                            case SelectedFeedPrice.Ask:
                                Decimal.TryParse(Math.Round(l1Data.Ask, 4).ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Bid:
                                Decimal.TryParse(Math.Round(l1Data.Bid, 4).ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Last:
                                Decimal.TryParse(Math.Round(l1Data.LastPrice, 4).ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.Mid:
                                Decimal.TryParse(Math.Round(l1Data.Mid, 4).ToString(), out realTimePrice);
                                break;
                            case SelectedFeedPrice.iMid:
                                Decimal.TryParse(Math.Round(l1Data.iMid, 4).ToString(), out realTimePrice);
                                break;
                            default:
                                Decimal.TryParse(Math.Round(l1Data.LastPrice, 4).ToString(), out realTimePrice);
                                break;
                        }
                    }
                    if (realTimePrice != 0)
                    {
                        _dictSymbolWithPrice[l1Data.Symbol] = realTimePrice;
                        StringBuilder errorMessage = new StringBuilder();
                        Dictionary<int, decimal> currentAccountFxRateValue = new Dictionary<int, decimal>();
                        currentAccountFxRateValue = ExpnlServiceConnector.GetInstance().GetFxRateForSymbolAndAccounts(l1Data.Symbol, new List<int> { -1 }, l1Data.AUECID, CachedDataManager.GetInstance.GetCurrencyID(l1Data.CurencyCode), ref errorMessage);
                        decimal fx = currentAccountFxRateValue.Count > 0 ? currentAccountFxRateValue.FirstOrDefault().Value : 1;
                        RebalancerCache.Instance.AddOrUpdateSymbolWisePriceAndFx(l1Data.Symbol, realTimePrice, fx);
                    }

                    if (_listRequestedSymbols.Count == 0)
                    {
                        _isSymbolValidated = true;
                        _timerSnapShot_Tick(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        internal ObservableCollection<KeyValueItem> GetKeyValueItemFromEnum<T>()
        {
            ObservableCollection<KeyValueItem> collection = new ObservableCollection<KeyValueItem>();
            try
            {
                Type type = typeof(T);
                var names = Enum.GetNames(type);
                int i = 1;
                foreach (var name in names)
                {
                    var field = type.GetField(name);
                    var fds = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    collection.Add(new KeyValueItem() { Key = i, ItemValue = fds[0].Description });
                    i++;
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
            return collection;
        }

        internal ObservableCollection<KeyValueItemWithFlag> GetKeyValueItemWithFlagFromEnum<T>()
        {
            ObservableCollection<KeyValueItemWithFlag> collection = new ObservableCollection<KeyValueItemWithFlag>();
            try
            {
                Type type = typeof(T);
                var names = Enum.GetNames(type);
                int i = 1;
                foreach (var name in names)
                {
                    var field = type.GetField(name);
                    var fds = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    collection.Add(new KeyValueItemWithFlag() { Key = i, ItemValue = fds[0].Description });
                    i++;
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
            return collection;
        }

        AutoResetEvent autoReset = new AutoResetEvent(false);
        List<ModelPortfolioSecurityDto> result = new List<ModelPortfolioSecurityDto>();
        internal List<ModelPortfolioSecurityDto> GetModelProtfolioData(ModelPortfolioDto modelPortfolioDto, RebalancerEnums.RebalancerPositionsType positionType, ref StringBuilder errorMessage)
        {

            try
            {
                _listRequestedSymbols.Clear();
                _listOfNewSymbols.Clear();
                result.Clear();
                List<int> accountIds = new List<int>();
                if (modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.ModelPortfolio)
                {
                    List<PortfolioDto> portfolioDtos = JsonConvert.DeserializeObject<List<PortfolioDto>>(modelPortfolioDto.ModelPortfolioData);
                    _dictSymbolTargetPercentage = portfolioDtos.ToDictionary(key => key.Symbol, value => value.TargetPercentage);
                    _dictSymbolTolerancePercentage = portfolioDtos.ToDictionary(key => key.Symbol, value => value.TolerancePercentage);
                    //Fetch security information and price for the security.
                    Dictionary<string, ModelPortfolioSecurityDto> modelPortfolioData = ExpnlServiceConnector.GetInstance()
                        .GetModelPortfolios(_dictSymbolTargetPercentage, positionType, ref errorMessage, _dictSymbolTolerancePercentage);
                    List<string> symbols = _dictSymbolTargetPercentage.Keys.Except(modelPortfolioData.Keys).ToList();
                    //Handling for Cash Symbol as it does not have any Live feed provider symbol(like Active symbol or factset Symbol)
                    //So no need to request this symbol data from live feed ,we need to handle it manually 
                    if (symbols.Contains(RebalancerConstants.CONST_CASHSymbol))
                    {
                        ModelPortfolioSecurityDto CashmodelPortfolio = new ModelPortfolioSecurityDto();
                        CashmodelPortfolio.AUECID = 1;
                        CashmodelPortfolio.RoundLot = 1;
                        CashmodelPortfolio.Delta = CashmodelPortfolio.FXRate = CashmodelPortfolio.LeveragedFactor = CashmodelPortfolio.Multiplier = 1;
                        CashmodelPortfolio.Symbol = RebalancerConstants.CONST_CASHSymbol;
                        CashmodelPortfolio.Asset = "Equity";
                        CashmodelPortfolio.TargetPercentage = _dictSymbolTargetPercentage[RebalancerConstants.CONST_CASHSymbol];
                        modelPortfolioData.Add(RebalancerConstants.CONST_CASHSymbol, CashmodelPortfolio);
                        symbols.Remove(RebalancerConstants.CONST_CASHSymbol);
                    }
                    result = modelPortfolioData.Values.ToList();
                    if (symbols.Count > 0)
                    {
                        if (_marketDataHelperInstance != null && _marketDataHelperInstance.IsDataManagerConnected())
                        {
                            _isSymbolValidated = false;
                            _marketDataHelperInstance.RequestMultipleSymbols(symbols, true);
                            _listRequestedSymbols.AddRange(symbols);
                            _listOfNewSymbols.AddRange(symbols);
                            _timerSnapShot.Interval = new TimeSpan(15000);
                            _timerSnapShot.Start();
                            autoReset.WaitOne(15000);
                        }
                    }

                    foreach (ModelPortfolioSecurityDto modelPortfolioSecurityDto in result)
                    {
                        modelPortfolioSecurityDto.ModelType = (int)modelPortfolioDto.ReferenceId;
                    }
                }
                else if (modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.MasterFund
                    || modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.Account
                    || modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.CustomGroup)
                {
                    if (modelPortfolioDto.ModelPortfolioData != null && modelPortfolioDto.UseTolerance == RebalancerEnums.UseTolerance.Yes)
                    {
                        List<PortfolioDto> portfolioDtos = JsonConvert.DeserializeObject<List<PortfolioDto>>(modelPortfolioDto.ModelPortfolioData);
                        _dictSymbolTolerancePercentage = portfolioDtos.ToDictionary(key => key.Symbol, value => value.TolerancePercentage);
                    }
                    int accountOrMFOrCGId = (int)modelPortfolioDto.ReferenceId;
                    if (modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.MasterFund)
                    {
                        if (CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation().ContainsKey(accountOrMFOrCGId))
                            accountIds = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[accountOrMFOrCGId];
                    }
                    else if (modelPortfolioDto.ModelPortfolioType == RebalancerEnums.ModelPortfolioType.Account)
                    {
                        accountIds = new List<int> { accountOrMFOrCGId };
                    }
                    else
                    {
                        accountIds = RebalancerCache.Instance.GetCustomGroupAssociatedAccounts(accountOrMFOrCGId);

                    }
                    Dictionary<string, ModelPortfolioSecurityDto> modelPortfolioData = ExpnlServiceConnector.GetInstance()
                        .GetModelPortfolioData(accountIds, positionType, ref errorMessage, _dictSymbolTolerancePercentage);

                    if (modelPortfolioData != null && modelPortfolioData.Count > 0)
                    {
                        result.AddRange(modelPortfolioData.Values.ToList());
                    }
                }
                var SymbolList = result.Select(x => x.Symbol).ToList();
                var NewSymbolObjList = SecMasterSyncServicesConnector.GetInstance().GetSecMasterSymbolData(SymbolList, ApplicationConstants.SymbologyCodes.TickerSymbol);
                foreach (var PortfolioSecurityDto in result)
                {
                    if (NewSymbolObjList != null && NewSymbolObjList.ContainsKey(PortfolioSecurityDto.Symbol))
                    {
                        PortfolioSecurityDto.RoundLot = NewSymbolObjList[PortfolioSecurityDto.Symbol].RoundLot;
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
            return result;
        }

        /// <summary>
        /// Create a grouped staged orders to send on the blotter
        /// </summary>
        /// <param name="lstTrades"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal List<OrderSingle> GroupStagedOrders(List<TradeListModel> lstTrades, ref StringBuilder errMsg, int companyUserID, string smartName, bool isUseCustodianBroker = false)
        {
            List<OrderSingle> stageOrderCollection = new List<OrderSingle>();
            try
            {
                _allocationPreferences = new Dictionary<int, AllocationOperationPreference>();
                // Create a stage order collection
                foreach (TradeListModel tradeListModel in lstTrades)
                {
                    OrderSingle orderSingle = CreateOrderSingleFromTradeList(tradeListModel, ref errMsg, companyUserID, smartName, isUseCustodianBroker);

                    if (errMsg.Length > 0)
                    {
                        return stageOrderCollection;
                    }
                    stageOrderCollection.Add(orderSingle);
                }

                // Create a copy of staged order to get the original quantity
                List<OrderSingle> stageOrderCollectionTemp = DeepCopyHelper.Clone(stageOrderCollection);

                int symbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();

                //Grouping Staged Orders to get total quantity
                StageImportDataList stageImportDataList = new StageImportDataList();
                foreach (OrderSingle order in stageOrderCollection)
                {
                    stageImportDataList.Add(symbology, order.Symbol, order, accountCollection);
                }

                // Create a Symbol wise total quantity collection
                Dictionary<string, double> dictSymbolWiseTotalQty = new Dictionary<string, double>();
                foreach (OrderSingle item in stageImportDataList.SelectMany(stageItem => stageItem.getOrderSingleList()))
                {
                    string key = GetKey(item);
                    item.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    item.TransactionSource = TransactionSource.Rebalancer;
                    item.TransactionSourceTag = (int)TransactionSource.Rebalancer;
                    if (!dictSymbolWiseTotalQty.ContainsKey(key))
                        dictSymbolWiseTotalQty.Add(key, item.Quantity);
                    else
                        dictSymbolWiseTotalQty[key] += item.Quantity;
                }

                // Calculate the percentage allocation for all accounts and multiple symbols
                SerializableDictionary<string, SerializableDictionary<int, AccountValue>> dictTargetPercs = new SerializableDictionary<string, SerializableDictionary<int, AccountValue>>();
                foreach (OrderSingle order in stageOrderCollectionTemp)
                {
                    string key = GetKey(order);
                    if (dictSymbolWiseTotalQty.ContainsKey(key))
                    {
                        double totalQuantity = dictSymbolWiseTotalQty[key];
                        decimal accountPercentageAllocation = Convert.ToDecimal((order.Quantity * 100) / totalQuantity);
                        AccountValue fv = new AccountValue(CachedDataManager.GetInstance.GetAccountID(order.Account), accountPercentageAllocation);
                        // Adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                        fv.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                        if (!dictTargetPercs.ContainsKey(key))
                        {
                            SerializableDictionary<int, AccountValue> dictAccountPercentageAllocation = new SerializableDictionary<int, AccountValue>();
                            dictAccountPercentageAllocation.Add(CachedDataManager.GetInstance.GetAccountID(order.Account), fv);
                            dictTargetPercs.Add(key, dictAccountPercentageAllocation);
                        }
                        else
                        {
                            if (!dictTargetPercs[key].ContainsKey(CachedDataManager.GetInstance.GetAccountID(order.Account)))
                            {
                                dictTargetPercs[key].Add(CachedDataManager.GetInstance.GetAccountID(order.Account), fv);
                            }
                            else
                            {
                                dictTargetPercs[key][CachedDataManager.GetInstance.GetAccountID(order.Account)].Value += fv.Value;
                            }
                        }
                    }
                }

                // Create allocation prefrences and update the original allocation prefrence id and level1 id
                foreach (StageImportData stageItem in stageImportDataList)
                {
                    List<OrderSingle> lstStagedOrders = stageItem.getOrderSingleList();
                    foreach (OrderSingle orderSingle in lstStagedOrders)
                    {
                        string key = GetKey(orderSingle);
                        if (dictTargetPercs.ContainsKey(key))
                        {
                            StringBuilder preferenceErrorMessage = new StringBuilder();
                            HelperFunctionsForCompliance.CreateCompleteOrderForStageAndCompliance(orderSingle, ref preferenceErrorMessage);
                            if (!string.IsNullOrEmpty(preferenceErrorMessage.ToString()))
                            {
                                errMsg.Append(preferenceErrorMessage);
                            }
                            // Below condition is applied when more than one account is selected from rebalancer and then changing the value of Level1ID with OperationPreferenceId otherwise it will be set to AccountID itself
                            if (dictTargetPercs[key].Count > 1)
                            {
                                var allocPreference = CreateAllocationPreference(orderSingle.Symbol, dictTargetPercs[key], smartName);
                                if (allocPreference != null)
                                {
                                    orderSingle.Level1ID = allocPreference.OperationPreferenceId;
                                    orderSingle.OriginalAllocationPreferenceID = allocPreference.OperationPreferenceId;
                                    if (!_allocationPreferences.ContainsKey(orderSingle.Level1ID))
                                    {
                                        _allocationPreferences.Add(orderSingle.Level1ID, allocPreference);
                                    }
                                }
                            }
                            else
                            {
                                orderSingle.OriginalAllocationPreferenceID = int.MinValue;
                                if (!_allocationPreferences.ContainsKey(orderSingle.Level1ID))
                                {
                                    _allocationPreferences.Add(orderSingle.Level1ID, null);
                                }
                            }
                        }
                    }
                }

                stageOrderCollection.Clear();
                foreach (StageImportData data in stageImportDataList)
                {
                    List<OrderSingle> list = data.getOrderSingleList();
                    if (list != null)
                        stageOrderCollection.AddRange(list);
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
            return stageOrderCollection;
        }


        /// <summary>
        /// This method is to get allocation prefernce for account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public AllocationOperationPreference GetAllocationOperationPreference(int accountId)
        {
            try
            {
                if (_allocationPreferences.ContainsKey(accountId))
                {
                    return _allocationPreferences[accountId];
                }
                else
                {
                    return null;
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
            return null;
        }

        /// <summary>
        /// Creating OrderSingle from TradeList
        /// </summary>
        /// <param name="item"></param>
        /// <param name="errMsg"></param>
        /// <param name="companyUserID"></param>
        /// <param name="smartName"></param>
        /// <returns></returns>
        private OrderSingle CreateOrderSingleFromTradeList(TradeListModel item, ref StringBuilder errMsg, int companyUserID, string smartName, bool isUseCustodianBroker)
        {
            OrderSingle orderSingle = new OrderSingle();
            try
            {
                TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                CounterPartyWiseCommissionBasis CommisionUserTTUiPrefs = TradingTktPrefs.CpwiseCommissionBasis;
                int venueID = 0;
                int counterPartyID = 0;
                int tradingAccount = 0;
                string tif = 0.ToString();
                int strategyID = int.MinValue;
                string executionInstructionId = string.Empty;
                int assetID = Convert.ToInt32(CachedDataManager.GetInstance.GetAssetIdByAUECId(item.AUECID));
                int underlyingID = Convert.ToInt32(CachedDataManager.GetInstance.GetUnderlyingID(item.AUECID));
                int exchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(item.AUECID);
                ValueList userBrokerList = TTHelperManager.GetInstance().GetCounterparties(assetID, underlyingID, item.AUECID);
                var isInUserBrokerList = companyTradingTicketUiPrefs.Broker.HasValue ? userBrokerList.ValueListItems.Cast<ValueListItem>().Any(valueitem => valueitem.DataValue.Equals(companyTradingTicketUiPrefs.Broker.Value)) : false;
                if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null)
                {
                    if (IsAnyNullOrEmpty(userTradingTicketUiPrefs, companyTradingTicketUiPrefs))
                    {
                        errMsg.Append(RebalancerConstants.MSG_PREF_NOT_DEFINED);
                    }
                    else
                    {
                        if (isUseCustodianBroker)
                        {
                            venueID = 1;
                        }
                        else if (userTradingTicketUiPrefs != null && userTradingTicketUiPrefs.Broker.HasValue && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(userTradingTicketUiPrefs.Broker.Value))
                        {
                            venueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[userTradingTicketUiPrefs.Broker.Value];
                        }
                        else if (companyTradingTicketUiPrefs.Venue.HasValue && isInUserBrokerList)
                        {
                            venueID = companyTradingTicketUiPrefs.Venue.Value;
                        }
                        else
                        {
                            venueID = 0;
                        }
                        counterPartyID = userTradingTicketUiPrefs.Broker.HasValue
                            ? userTradingTicketUiPrefs.Broker.Value
                            : ((isInUserBrokerList) ? companyTradingTicketUiPrefs.Broker.Value : int.MinValue);
                        tif = userTradingTicketUiPrefs.TimeInForce.HasValue
                            ? TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(userTradingTicketUiPrefs.TimeInForce
                                .Value
                                .ToString())
                            : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(companyTradingTicketUiPrefs
                                .TimeInForce
                                .Value.ToString());
                        tradingAccount = userTradingTicketUiPrefs.TradingAccount.HasValue
                            ? userTradingTicketUiPrefs.TradingAccount.Value
                            : companyTradingTicketUiPrefs.TradingAccount.Value;
                        bool strategy = userTradingTicketUiPrefs.Strategy.HasValue
                            ? int.TryParse(userTradingTicketUiPrefs.Strategy.ToString(), out strategyID)
                            : int.TryParse(companyTradingTicketUiPrefs.Strategy.ToString(), out strategyID);
                        executionInstructionId = userTradingTicketUiPrefs.ExecutionInstruction.HasValue
                            ? userTradingTicketUiPrefs.ExecutionInstruction.ToString()
                            : companyTradingTicketUiPrefs.ExecutionInstruction.ToString();
                    }
                }
                else
                {
                    errMsg.Append(RebalancerConstants.MSG_PREF_NOT_DEFINED);
                }

                if (tradingAccount <= 0 && !errMsg.ToString().Contains(RebalancerConstants.MSG_PREF_NOT_DEFINED))
                    errMsg.Append("Trading account not defined, ");
                if (Convert.ToInt32(tif) < 0)
                    errMsg.Append("TIF not defined, ");

                if (item.Quantity != 0 && errMsg.Length == 0)
                {
                    orderSingle.OrderSingleGuid = item.TradeGuid;
                    orderSingle.MsgType = FIXConstants.MSGOrder;
                    orderSingle.Account = CachedDataManager.GetInstance.GetAccount(item.AccountId);
                    orderSingle.AUECID = item.AUECID;
                    orderSingle.InternalComments = item.Comments;
                    //TODO: Fill correct company id
                    orderSingle.CompanyName = string.Empty;
                    orderSingle.Description = string.Empty;

                    orderSingle.AUECLocalDate = DateTime.Now.Date; // item.AUECLocalDate;
                    orderSingle.ProcessDate = DateTime.Now.Date; //item.AUECLocalDate;
                    orderSingle.NirvanaProcessDate = DateTime.Now.Date; //item.AUECLocalDate;
                    orderSingle.OriginalPurchaseDate = DateTime.Now.Date; //item.AUECLocalDate;


                    orderSingle.Quantity = Math.Abs(Convert.ToDouble(item.Quantity));
                    orderSingle.Price = Convert.ToDouble(item.Price);
                    orderSingle.AvgPrice = Convert.ToDouble(item.Price);
                    orderSingle.Text = string.Empty;
                    orderSingle.Symbol = item.Symbol;
                    orderSingle.ContractMultiplier = (double)item.ContractMultiplier;
                    orderSingle.AssetID = assetID;

                    UpdateSide(item, orderSingle);
                    orderSingle.OrderSide =
                        TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
                    orderSingle.TransactionType =
                        TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
                    orderSingle.ChangeType = 2;

                    if (orderSingle.ChangeType == (int)ChangeType.Transfer)
                    {
                        orderSingle.AvgPrice = Convert.ToDouble(item.Price);
                        orderSingle.CumQty = Math.Abs(Convert.ToDouble(item.Quantity));
                    }

                    orderSingle.HandlingInstruction = "3";

                    // Update levelID if preference already created (Only execute this code when side and symbol is updated)
                    orderSingle.Level1ID = item.AccountId;

                    orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(venueID);
                    orderSingle.VenueID = venueID;
                    orderSingle.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(counterPartyID);
                    orderSingle.CounterPartyID = counterPartyID;
                    orderSingle.HandlingInstruction = "3";
                    orderSingle.OrderTypeTagValue = FIXConstants.ORDTYPE_Market;

                    orderSingle.TIF = tif;
                    orderSingle.Level2ID = strategyID;
                    orderSingle.TradingAccountID = tradingAccount;
                    orderSingle.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(executionInstructionId);
                    orderSingle.AssetID = assetID;
                    orderSingle.UnderlyingID = underlyingID;
                    orderSingle.UnderlyingName = CachedDataManager.GetInstance.GetAssetText(orderSingle.UnderlyingID); ;
                    orderSingle.CompanyUserID = companyUserID;

                    orderSingle.TransactionTime =
                        DateTime.Now.ToUniversalTime();
                    orderSingle.AlgoStrategyID = string.Empty;
                    orderSingle.AlgoProperties = new OrderAlgoStartegyParameters();
                    orderSingle.CurrencyID =
                        Convert.ToInt32(
                            CachedDataManager.GetInstance.GetCurrencyIdByAUECID(item.AUECID)); // item.CurrencyID;
                    orderSingle.ExchangeID = exchangeID;

                    orderSingle.TradeAttribute1 = string.Empty;
                    orderSingle.TradeAttribute2 = string.Empty;
                    orderSingle.TradeAttribute3 = string.Empty;
                    orderSingle.TradeAttribute4 = string.Empty;
                    orderSingle.TradeAttribute5 = string.Empty;
                    orderSingle.TradeAttribute6 = string.Empty;

                    orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    orderSingle.TransactionSource = TransactionSource.Rebalancer;
                    orderSingle.TransactionSourceTag = (int)TransactionSource.Rebalancer;
                    orderSingle.FXConversionMethodOperator = BusinessObjects.AppConstants.Operator.M.ToString();
                    orderSingle.FXRate = (double)item.FXRate;
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                    orderSingle.RebalancerFileName = smartName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return orderSingle;
        }

        /// <summary>
        /// Create Allocation Preference for rebalancer staged order
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="targetPercs"></param>
        /// <returns></returns>
        public static AllocationOperationPreference CreateAllocationPreference(String Symbol, SerializableDictionary<int, AccountValue> targetPercs, string rebalancerFileName)
        {
            AllocationOperationPreference allocationOperationPreference = null;
            try
            {
                string prefName = "*Rebal#_" + Symbol + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "_" + DateTime.Now.ToString("yyMMddHHmmssff");
                PreferenceUpdateResult result = ServiceManager.Instance.AllocationManager.InnerChannel.AddPreference(prefName, CachedDataManager.GetInstance.GetCompanyID(), AllocationPreferencesType.CalculatedAllocationPreference, false, rebalancerFileName);
                allocationOperationPreference = result.Preference;
                if (allocationOperationPreference != null)
                {
                    allocationOperationPreference.TryUpdateTargetPercentage(targetPercs);
                    //Set Default rule for Allocation
                    AllocationRule defaulfRule = new AllocationRule();
                    defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                    defaulfRule.RuleType = MatchingRuleType.None;
                    defaulfRule.PreferenceAccountId = -1;
                    defaulfRule.MatchClosingTransaction = MatchClosingTransactionType.None;
                    allocationOperationPreference.TryUpdateDefaultRule(defaulfRule);
                    ServiceManager.Instance.AllocationManager.InnerChannel.UpdatePreference(allocationOperationPreference);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationOperationPreference;
        }

        /// <summary>
        /// IsAnyNullOrEmpty Check
        /// </summary>
        /// <param name="userTradingTicketUiPrefsObj"></param>
        /// <param name="companyTradingTicketUiPrefsObj"></param>
        /// <returns></returns>
        private static bool IsAnyNullOrEmpty(TradingTicketUIPrefs userTradingTicketUiPrefsObj, TradingTicketUIPrefs companyTradingTicketUiPrefsObj)
        {
            bool returnValue = false;
            try
            {
                if (!userTradingTicketUiPrefsObj.TradingAccount.HasValue && !companyTradingTicketUiPrefsObj.TradingAccount.HasValue)
                {
                    returnValue = true;
                }
                else if (!userTradingTicketUiPrefsObj.TimeInForce.HasValue && !companyTradingTicketUiPrefsObj.TimeInForce.HasValue)
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return returnValue;
        }

        /// <summary>
        /// UpdateSide
        /// </summary>
        /// <param name="item"></param>
        /// <param name="orderSingle"></param>
        private static void UpdateSide(TradeListModel item, OrderSingle orderSingle)
        {
            try
            {
                if (item.Side == PTTOrderSide.Buy)
                    orderSingle.OrderSideTagValue = FIXConstants.SIDE_Buy;
                if (item.Side == PTTOrderSide.BuyToClose)
                    orderSingle.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;
                if (item.Side == PTTOrderSide.Sell)
                    orderSingle.OrderSideTagValue = FIXConstants.SIDE_Sell;
                if (item.Side == PTTOrderSide.SellShort)
                    orderSingle.OrderSideTagValue = FIXConstants.SIDE_SellShort;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the master fund i dfrom accounts.
        /// </summary>
        /// <param name="mfOrAccount">The mf or account.</param>
        /// <param name="accountList">The account list.</param>
        /// <returns></returns>
        internal int GetMasterFundIDfromAccounts(RebalancerEnums.AccountTypes mfOrAccount, List<int> accountList)
        {
            int mfID = int.MinValue;

            try
            {
                if (mfOrAccount == RebalancerEnums.AccountTypes.MasterFund && accountList.Count == 1)
                {
                    mfID = accountList.First();
                }
                else if (mfOrAccount == RebalancerEnums.AccountTypes.Account && accountList.Count >= 1)
                {
                    List<int> funds = new List<int>();
                    foreach (var account in accountList)
                    {
                        int fund = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(account);
                        if (!funds.Contains(fund))
                            funds.Add(fund);
                    }
                    if (funds.Count == 1)
                        mfID = funds.First();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfID;
        }

        /// <summary>
        /// Sets isRealTimePositions preference selected in Rebalancer
        /// </summary>
        /// <param name="isRealTimePositions"></param>
        public void SetIsRealTimePositionsPreference(bool isRealTimePositions)
        {
            try
            {
                _isRealTimePositions = isRealTimePositions;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets isRealTimePositions preference set in Rebalancer
        /// </summary>
        /// <returns></returns>
        public bool GetIsRealTimePositionsPreference()
        {
            try
            {
                return _isRealTimePositions;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        /// <summary>
        /// returns key value (symbol_orderSideTagValue)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetKey(OrderSingle order)
        {
            string key = string.Empty;
            try
            {
                key = order.Symbol + "_" + order.OrderSideTagValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return key;
        }

        #region IDisposable
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (instance != null)
                        instance = null;
                    if (_marketDataHelperInstance != null)
                    {
                        _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                        _marketDataHelperInstance.Dispose();
                        _marketDataHelperInstance = null;
                    }
                    if (_timerSnapShot != null)
                    {
                        _timerSnapShot.Tick -= new EventHandler(_timerSnapShot_Tick);
                        _timerSnapShot = null;
                    }
                    autoReset.Dispose();
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
        #endregion

    }
}
