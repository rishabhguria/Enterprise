using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.TTView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Prana.TradingTicket.TTPresenter
{
    internal class QuickTradingTicketPresenter : TicketPresenterBase
    {
        private new IQuickTradingTicketView iTicketView;

        public bool IsTradeSending = false;
        public override string getReceiverUniqueName()
        {
            try
            {
                return "QuickTradingTicket";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// Updates the security details.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        protected override void UpdateSecurityDetails(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (iTicketView.SymbolText.Trim().ToUpper() == secMasterObj.SymbologyMapping[(int)DefaultSymbology].ToUpper())
                {
                    _assetID = secMasterObj.AssetID;
                    _auecID = secMasterObj.AUECID;
                    if (secMasterObj.AssetCategory == AssetCategory.Equity || secMasterObj.AssetCategory == AssetCategory.EquityOption || secMasterObj.AssetCategory == AssetCategory.Future || secMasterObj.AssetCategory == AssetCategory.FutureOption || secMasterObj.AssetCategory == AssetCategory.FixedIncome)
                    {
                        if (!IsTradeSending)
                        {
                            SetSecMasterObjForTicket(secMasterObj);
                            Symbol = secMasterObj.TickerSymbol;
                            ValidateSymbolSetByUser();
                            _accountqty = null;
                            iTicketView.DrawControl(secMasterObj);
                            iTicketView.SetAUECDateInTicket(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(secMasterObj.AUECID)));
                        }
                    }
                    else
                    {
                        iTicketView.SetLabelMessage(secMasterObj.AssetCategory + " not allowed to trade from QTT, try placing order through Trading Ticket");
                        iTicketView.SetValidationTokenSource();
                    }
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
        }

        /// <summary>
        /// Fills the combo boxes.
        /// </summary>
        internal override void FillComboBoxes()
        {
            try
            {
                base.FillComboBoxes();
                SetTicketPreferences();
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

        private void SetTicketPreferences()
        {
            iTicketView.SetTicketPreferences(_userTradingTicketUiPrefs, _companyTradingTicketUiPrefs);
        }

        /// <summary>
        /// Validates the quantity and order side.
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateQuantityAndOrderSide()
        {
            try
            {
                if (iTicketView.Quantity == 0.0m)
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_VALID_QUANTITY);
                    return false;
                }
                Regex rg1 = new Regex(@"^\d*\.{0,1}\d+$");

                if (!(rg1.IsMatch(iTicketView.Quantity.ToString())))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_NUMERIC_QUANTITY);
                    return false;
                }
                if (String.IsNullOrEmpty(iTicketView.OrderSide))
                {
                    iTicketView.SetMessageBoxText(TradingTicketConstants.MSG_SELECT_A_VALID_ORDER_SIDE);
                    return false;
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
            return true;
        }

        /// <summary>
        /// Gets the accounts for expnl request.
        /// </summary>
        /// <returns></returns>
        internal override List<int> GetAccountsForExpnlRequest()
        {
            List<int> result = new List<int>();
            try
            {
                int accountValue;
                bool isValid = int.TryParse(iTicketView.Account, out accountValue);
                if (isValid)
                {
                    int prefID = accountValue;
                    var funds = prefID <= 0 ? CachedDataManager.GetInstance.GetAllUserAccountList() : _allocationProxy.InnerChannel.GetSelectedAccountsFromPref(prefID);
                    if (funds != null)
                        result = new List<int>(funds);
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

        protected override string ValidateTradeWithAlgoControls()
        {
            return string.Empty;
        }

        /// <summary>
        /// Adds the specified object i trading ticket view.
        /// </summary>
        /// <param name="objITradingTicketView">The object i trading ticket view.</param>
        internal override void Add(ITicketView objITradingTicketView)
        {
            base.Add(objITradingTicketView);
            iTicketView = (IQuickTradingTicketView)objITradingTicketView;
        }

        /// <summary>
        /// Creates the new live order.
        /// </summary>
        /// <returns></returns>
        internal bool CreateNewLiveOrder()
        {
            try
            {
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));
                if (ComplianceCacheManager.GetPreTradeCheck(_loginUser.CompanyUserID))
                {
                    if (CompliancePriceCheckRequired && iTicketView.Price == Convert.ToDecimal(0))
                    {
                        if (this.PriceForComplianceNotAvailable != null)
                        {
                            this.PriceForComplianceNotAvailable(this, new EventArgs());
                        }
                        return false;
                    }
                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    {
                        iTicketView.SetMessageBoxText(ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE);
                        return false;
                    }
                }
                if (AuecID == int.MinValue)
                {
                    iTicketView.SetLabelMessage(TradingTicketConstants.MSG_SYMBOL_CANNOT_VERIFIED);
                    return false;
                }

                if (!GetTradingTicket())
                {
                    return false;
                }
                OrderSingle or = GetOrderFromTicket();

                if (or == null)
                {
                    return false;
                }
                or.IsManualOrder = false;
                or.IsStageRequired = true;
                string error = ValidateTradeWithAlgoControls();

                if (error != string.Empty)
                {
                    iTicketView.SetMessageBoxText(error);
                    return false;
                }
                if (or.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)// if selected order type is Limit
                {
                    if (!TradeManagerCore.GetInstance().IsWithinLimits(or, _marketPrice))
                    {
                        return false;
                    }
                }
                or.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                or.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                or.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                or.AUECID = AuecID;
                or.IsPricingAvailable = _isPricingAvailable;
                or.TransactionSource = TransactionSource.TradingTicket;
                or.TransactionSourceTag = (int)TransactionSource.TradingTicket;

                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out1] Before SendBlotterTrades In QuickTradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ",Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                bool isTradingRulesPassed = true;
                if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(or.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    isTradingRulesPassed = TradingRulesValidator.ValidateCompanyTradingRules(or, (double)iTicketView.Price, UseQuantityFieldAsNotional);
                }
                if (!isTradingRulesPassed || !(_tradeManager.SendBlotterTrades(or, _marketPrice, true)))
                {
                    return false;
                }
                _orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                or.PranaMsgType = (int)OrderFields.PranaMsgTypes.InternalOrder;
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow Out1] After SendBlotterTrades In QuickTradingTicketPresenter, Fix Message: userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol: " + Convert.ToString(or.Symbol) + " , OrderID: " + Convert.ToString(or.OrderID) + " , Quantity: " + Convert.ToString(or.Quantity) + " , Transaction Time: " + Convert.ToString(or.TransactionTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
