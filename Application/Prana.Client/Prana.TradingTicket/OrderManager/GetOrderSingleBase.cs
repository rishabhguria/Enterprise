using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;
using System.Linq;
using System.Reflection;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle common order fields for all asset
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.IGetOrderSingle" />
    public class GetOrderSingleBase : IGetOrderSingle
    {
        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <param name="iTicketView">The i trading ticket view.</param>
        /// <param name="ticketPresenter">The tt presenter.</param>
        /// <param name="orderSingle">The order single.</param>
        public virtual void GetOrderFromTicket(ITicketView iTicketView, TicketPresenterBase ticketPresenter, OrderSingle orderSingle)
        {
            try
            {
                if (orderSingle.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && orderSingle.MsgType != FIXConstants.MSGExecutionReport && orderSingle.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX &&
                          orderSingle.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    orderSingle.MsgType = FIXConstants.MSGOrder;
                }

                if (ticketPresenter.UseQuantityFieldAsNotional)
                {
                    orderSingle.Quantity = Math.Floor(Convert.ToDouble(iTicketView.Quantity) / Convert.ToDouble(iTicketView.Price));
                }
                else
                {
                    orderSingle.Quantity = Convert.ToDouble(iTicketView.Quantity);
                }
                orderSingle.Price = Convert.ToDouble(iTicketView.Limit);
                orderSingle.StopPrice = Convert.ToDouble(iTicketView.Stop);
                orderSingle.Text = iTicketView.BrokerNotes;
                orderSingle.InternalComments = iTicketView.Notes;
                orderSingle.Symbol = ticketPresenter.Symbol;
                if (ticketPresenter.SecmasterObj != null)
                {
                    orderSingle.BloombergSymbol = ticketPresenter.SecmasterObj.BloombergSymbol;
                    orderSingle.BloombergSymbolWithExchangeCode = ticketPresenter.SecmasterObj.BloombergSymbolWithExchangeCode;
                    orderSingle.FactSetSymbol = ticketPresenter.SecmasterObj.FactSetSymbol;
                    orderSingle.ActivSymbol = ticketPresenter.SecmasterObj.ActivSymbol;
                    orderSingle.UnderlyingID = ticketPresenter.SecmasterObj.UnderLyingID;
                }

                orderSingle.OrderSideTagValue = String.IsNullOrEmpty(iTicketView.OrderSide) ? String.Empty : TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(iTicketView.OrderSide);
                orderSingle.CounterPartyName = iTicketView.Broker;
                orderSingle.CounterPartyID = String.IsNullOrEmpty(iTicketView.Brokerid) ? int.MinValue : int.Parse(iTicketView.Brokerid);
                orderSingle.Venue = iTicketView.Venue;
                orderSingle.VenueID = String.IsNullOrEmpty(iTicketView.VenueId) ? int.MinValue : int.Parse(iTicketView.VenueId);
                orderSingle.HandlingInstruction = iTicketView.HandlingInstruction == null ? String.Empty : TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(iTicketView.HandlingInstruction);
                orderSingle.OrderTypeTagValue = String.IsNullOrEmpty(iTicketView.OrderType) ? String.Empty : TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(iTicketView.OrderType);
                if (orderSingle.OrderTypeTagValue == FIXConstants.ORDTYPE_Market || orderSingle.OrderTypeTagValue == FIXConstants.ORDTYPE_MarketOnClose ||
                    orderSingle.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop)
                {
                    orderSingle.Price = 0.0;
                }
                if (orderSingle.OrderTypeTagValue != FIXConstants.ORDTYPE_Stop && orderSingle.OrderTypeTagValue != FIXConstants.ORDTYPE_Stoplimit)
                {
                    orderSingle.StopPrice = 0.0;
                }

                orderSingle.TIF = String.IsNullOrEmpty(iTicketView.TIF) ? String.Empty : TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(iTicketView.TIF);
                if (iTicketView.Account != null && (orderSingle.MsgType != FIXConstants.MSGOrderCancelReplaceRequest || orderSingle.PranaMsgType.Equals((int)OrderFields.PranaMsgTypes.ORDStaged)))
                {
                    int accountID;
                    orderSingle.Level1ID = int.TryParse(iTicketView.Account, out accountID) ? accountID : int.MinValue;
                }

                if (iTicketView.Strategy != null)
                    orderSingle.Level2ID = iTicketView.Strategy.HasValue ? iTicketView.Strategy.Value : int.MinValue;
                if (iTicketView.TradingAccount != null)
                {
                    orderSingle.TradingAccountID = Int32.Parse(iTicketView.TradingAccount);
                }
                orderSingle.ExecutionInstruction = String.IsNullOrEmpty(iTicketView.ExecutionInstructions) ? String.Empty : TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(iTicketView.ExecutionInstructions);
                orderSingle.AssetID = ticketPresenter.AssetID;
                if (orderSingle.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && orderSingle.PranaMsgType == 0)
                {
                    orderSingle.CompanyUserID = ticketPresenter.LoginUser.CompanyUserID;
                    orderSingle.ActualCompanyUserID = ticketPresenter.LoginUser.CompanyUserID;
                }
                orderSingle.ModifiedUserId = ticketPresenter.LoginUser.CompanyUserID;

                orderSingle.TransactionTime = DateTime.Now.ToUniversalTime();
                TradingTicketPresenter ttPresenter = ticketPresenter as TradingTicketPresenter;

                TradingTicketType ticketType = ttPresenter != null ? ttPresenter.TicketType : TradingTicketType.Live;
                switch (ticketType)
                {
                    case TradingTicketType.Manual:
                        // if TT is opened from Blotter or any other module then do not set CumQuantity to zero because it has executed quantity so
                        // when TT is opened from Blotter or any other module then the IncomingOrderRequest is not null
                        if (ttPresenter == null || ttPresenter.IncomingOrderRequest == null)
                        {
                            orderSingle.CumQty = 0;
                        }
                        double orderQunatity = 0.0;
                        if (orderSingle.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || orderSingle.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild)
                        {
                            orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDManualSub;
                        }
                        else
                        {
                            orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDManual;
                        }
                        if (iTicketView.TargetQuantity > 0)
                        {
                            if (ticketPresenter.UseQuantityFieldAsNotional)
                            {
                                orderQunatity = Math.Floor(GetTargetQuantityInNumericOrPercentage(iTicketView, ticketPresenter) / Convert.ToDouble(iTicketView.Price));
                            }
                            else
                            {
                                orderQunatity = GetTargetQuantityInNumericOrPercentage(iTicketView, ticketPresenter);
                            }
                            if (orderSingle.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                            {
                                orderSingle.AvgPrice = Convert.ToDouble(iTicketView.Price);
                            }
                        }
                        else
                        {
                            orderSingle.CumQty = 0;
                            orderSingle.AvgPrice = 0;
                        }
                        if (orderSingle.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                        {
                            orderSingle.CumQty = orderQunatity;
                        }
                        else
                        {
                            orderSingle.CumQtyForSubOrder = orderQunatity;
                        }
                        DateTime auecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(ticketPresenter.AuecID));
                        DateTime enteredTradeDate = iTicketView.TradeDate;
                        DateTime tradeDate = new DateTime(auecDate.Year, auecDate.Month, auecDate.Day, auecDate.Hour, auecDate.Minute, auecDate.Second);
                        if (enteredTradeDate.Date < auecDate.Date)
                        {
                            tradeDate = new DateTime(enteredTradeDate.Year, enteredTradeDate.Month, enteredTradeDate.Day, 08, 00, 00);
                        }
                        else if (enteredTradeDate.Date > auecDate.Date)
                        {
                            tradeDate = new DateTime(enteredTradeDate.Year, enteredTradeDate.Month, enteredTradeDate.Day, auecDate.Hour, auecDate.Minute, auecDate.Second);
                        }
                        orderSingle.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(tradeDate, CachedDataManager.GetInstance.GetAUECTimeZone(ticketPresenter.AuecID));
                        break;
                    case TradingTicketType.Live:
                        orderSingle.CumQty = 0;
                        if (ticketPresenter.UseQuantityFieldAsNotional)
                        {
                            orderSingle.CumQtyForSubOrder = Math.Floor(GetTargetQuantityInNumericOrPercentage(iTicketView, ticketPresenter) / Convert.ToDouble(iTicketView.Price));
                        }
                        else
                        {
                            orderSingle.CumQtyForSubOrder = GetTargetQuantityInNumericOrPercentage(iTicketView, ticketPresenter);
                        }
                        orderSingle.AvgPrice = 0;
                        orderSingle.AvgPriceForCompliance = Convert.ToDouble(iTicketView.Price);
                        orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDNewSub;
                        break;
                    default:
                        orderSingle.CumQty = 0.0;
                        orderSingle.AvgPrice = 0;
                        orderSingle.AvgPriceForCompliance = Convert.ToDouble(iTicketView.Price);
                        break;
                }

                if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(ticketPresenter.CounterPartyId) && ticketPresenter.AlgoStrategyId != string.Empty && ticketPresenter.AlgoStrategyId != int.MinValue.ToString())
                {
                    orderSingle.AlgoStrategyID = ticketPresenter.AlgoStrategyId;
                    if (iTicketView.TagValueDictionary != null)
                        orderSingle.AlgoProperties.TagValueDictionary = iTicketView.TagValueDictionary;
                    else
                        orderSingle.AlgoProperties.TagValueDictionary = iTicketView.AlgoStrategyControlProperty.GetSelectedStrategyFixTagValues();
                    orderSingle.AlgoStrategyName = ticketPresenter.AlgoStrategyName;
                }
                else
                {
                    orderSingle.AlgoStrategyID = string.Empty;
                    orderSingle.AlgoProperties = new OrderAlgoStartegyParameters();
                }

                orderSingle.ContractMultiplier = ticketPresenter.Multiplier;
                orderSingle.CurrencyID = ticketPresenter.CurrencyId;
                if (ticketPresenter.SecmasterObj != null)
                {
                    orderSingle.ExchangeID = ticketPresenter.SecmasterObj.ExchangeID;
                }
                orderSingle.CommissionRate = Convert.ToDouble(iTicketView.CommissionRate);
                orderSingle.SoftCommissionRate = Convert.ToDouble(iTicketView.SoftCommissionRate);
                orderSingle.CalcBasis = iTicketView.CommissionBasis;
                orderSingle.SoftCommissionCalcBasis = iTicketView.SoftCommissionBasis;
                orderSingle.TradeAttribute1 = iTicketView.TradeAttribute1;
                orderSingle.TradeAttribute2 = iTicketView.TradeAttribute2;
                orderSingle.TradeAttribute3 = iTicketView.TradeAttribute3;
                orderSingle.TradeAttribute4 = iTicketView.TradeAttribute4;
                orderSingle.TradeAttribute5 = iTicketView.TradeAttribute5;
                orderSingle.TradeAttribute6 = iTicketView.TradeAttribute6;
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    // Case-1 client selected but (account selected or not) - then prefer client name for masterfund (TradeAttribute6)
                    // Case-2 Client not selected but account selected - then get masterfund  name from account
                    //  Case-2 Client not selected and account not selected - then TradeAttribute6 will be blank
                    var fundIdOfSelectedAccount = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(orderSingle.Level1ID);
                    orderSingle.TradeAttribute6 = ticketPresenter.FundId > 0 ? CachedDataManager.GetInstance.GetMasterFund(ticketPresenter.FundId) : CachedDataManager.GetInstance.GetMasterFund(fundIdOfSelectedAccount);
                    orderSingle.MasterFund = orderSingle.TradeAttribute6;
                }

                orderSingle.SettlementCurrencyID = GetValueOfNullableInt(iTicketView.SettlementCurrency);
                orderSingle.FXConversionMethodOperator = iTicketView.FxOperator;
                orderSingle.FXRate = Convert.ToDouble(iTicketView.FxRate);
                if (iTicketView.OTCParameters != null)
                    orderSingle.OTCParameters = iTicketView.OTCParameters;
                CalculateFxRateFromCurrenyChecks(orderSingle);

                SetTTPropertiesMapping(iTicketView, ticketPresenter, orderSingle);

                orderSingle.IsUseCustodianBroker = iTicketView.IsUseCustodianAsExecutingBroker;
                orderSingle.AccountBrokerMapping = orderSingle.IsUseCustodianBroker ? JsonHelper.SerializeObject(iTicketView.AccountBrokerMapping): string.Empty;        
                orderSingle.TradeApplicationSource = Convert.ToInt32(TradeApplicationSource.Enterprise);
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

        private int GetValueOfNullableInt(int? nullableValue)
        {
            int value = int.MinValue;
            try
            {
                if (nullableValue.HasValue)
                {
                    Int32.TryParse(nullableValue.ToString(), out value);
                }
                else
                {
                    value = int.MinValue;
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

            return value;
        }

        /// <summary>
        /// Set TT Properties Mapping
        /// </summary>
        /// <param name="iTicketView"></param>
        /// <param name="ttPresenter"></param>
        /// <param name="orderSingle"></param>
        private static void SetTTPropertiesMapping(ITicketView iTicketView, TicketPresenterBase ttPresenter, OrderSingle orderSingle)
        {
            try
            {
                System.Collections.Generic.List<DefTTControlsMapping> listTTControlsMapping = ttPresenter.CompanyTradingTicketUiPrefs.listTTControlsMapping;
                if (listTTControlsMapping.Count >= 1)
                {
                    double _avgPrice = Convert.ToDouble((iTicketView.Price).ToString());
                    foreach (DefTTControlsMapping mappings in listTTControlsMapping)
                    {
                        string fromControl = "_" + mappings.FromControl.ToLower();
                        string toControl = "_" + mappings.ToControl.ToLower();


                        var fromFields = orderSingle.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        var fromField = fromFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromControl));
                        if (fromField == null)
                        {
                            fromFields = orderSingle.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                            if (fromFields != null)
                                fromField = fromFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromControl));
                        }

                        var toFields = orderSingle.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        var toField = toFields.SingleOrDefault(a => a.Name.ToLower().Contains(toControl));

                        if (toField == null)
                        {
                            toFields = orderSingle.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                            if (toFields != null)
                                toField = toFields.SingleOrDefault(a => a.Name.ToLower().Contains(toControl));
                        }
                        if (toField != null && fromField != null)
                        {
                            switch (toField.FieldType.Name)
                            {
                                case "Double":
                                    double _doubleValue;
                                    if (Double.TryParse((fromField.GetValue(orderSingle)).ToString(), out _doubleValue))
                                    {
                                        if (fromField.Name.Equals("_avgPrice"))
                                            _doubleValue = _avgPrice;
                                        toField.SetValue(orderSingle, _doubleValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Double Type " + fromField.FieldType.Name);
                                    }
                                    break;
                                case "String":
                                    var _stringValue = (fromField.GetValue(orderSingle)).ToString();
                                    if (fromField.Name.Equals("_avgPrice"))
                                        _stringValue = _avgPrice.ToString();
                                    toField.SetValue(orderSingle, _stringValue);
                                    break;
                                case "Int":
                                    int _intValue;
                                    if (int.TryParse((fromField.GetValue(orderSingle)).ToString(), out _intValue))
                                    {
                                        if (fromField.Name.Equals("_avgPrice"))
                                            _intValue = Convert.ToInt16(_avgPrice);
                                        toField.SetValue(orderSingle, _intValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Int Type " + fromField.FieldType.Name);
                                    }
                                    break;

                                case "Boolean":
                                    bool _boolValue;
                                    if (Boolean.TryParse((fromField.GetValue(orderSingle)).ToString(), out _boolValue))
                                    {
                                        toField.SetValue(orderSingle, _boolValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Boolean Type " + fromField.FieldType.Name);
                                    }
                                    break;

                                case "DateTime":
                                    DateTime _dateValue;
                                    if (DateTime.TryParse((fromField.GetValue(orderSingle)).ToString(), out _dateValue))
                                    {
                                        toField.SetValue(orderSingle, _dateValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Date Time Type " + fromField.FieldType.Name);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        /// <summary>
        /// Gets the target quantity in numeric or percentage.
        /// </summary>
        /// <param name="iTicketView">The i trading ticket view.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        /// <returns></returns>
        private double GetTargetQuantityInNumericOrPercentage(ITicketView iTicketView, TicketPresenterBase ttPresenter)
        {
            decimal targetQuantity = 0.0m;
            try
            {
                if (ttPresenter is TradingTicketPresenter)
                {
                    targetQuantity = (ttPresenter as TradingTicketPresenter).EnterTargetQuantityInPercentage ? Math.Round((iTicketView.Quantity * iTicketView.TargetQuantity) / ApplicationConstants.PERCENTAGEVALUE, 4) : iTicketView.TargetQuantity;
                }
                else
                {
                    targetQuantity = iTicketView.TargetQuantity;
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

            return Convert.ToDouble(targetQuantity);
        }


        /// <summary>
        /// Gets the value of nullable int.
        /// </summary>
        /// <param name="nullableValue">The nullable value.</param>
        /// <returns></returns>

        /// <summary>
        /// Calculates the fx rate from currency checks.
        /// </summary>
        /// <param name="orderSingle">The order single.</param>
        public void CalculateFxRateFromCurrenyChecks(OrderSingle orderSingle)
        {
            try
            {
                //PRANA-32843 if the asset id is fx or fxforward and settlement currency is equal to company base currency then it shoud not replace the value of FxRate to 1.
                if (orderSingle.AssetID == (int)AssetCategory.FX || orderSingle.AssetID == (int)AssetCategory.FXForward) return;

                int companyBaseCurrID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (orderSingle.SettlementCurrencyID == orderSingle.CurrencyID)
                {
                    if (companyBaseCurrID == orderSingle.CurrencyID)
                    {
                        orderSingle.FXRate = 1;
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
        }

        /// <summary>
        /// Sets the sec master object for ticket.
        /// </summary>
        /// <param name="secmasterObj">The secmaster object.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        public virtual void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj, TicketPresenterBase ttPresenter)
        {
            try
            {
                if (secmasterObj != null)
                {
                    ttPresenter.AuecID = secmasterObj.AUECID;
                    ttPresenter.CurrencyId = secmasterObj.CurrencyID;
                    ttPresenter.Symbol = secmasterObj.TickerSymbol;
                    ttPresenter.AssetID = secmasterObj.AssetID;
                    ttPresenter.UnderlyingSymbol = secmasterObj.UnderLyingSymbol;
                    ttPresenter.Multiplier = secmasterObj.Multiplier;
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
    }
}
