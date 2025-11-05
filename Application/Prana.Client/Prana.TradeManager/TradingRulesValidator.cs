using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.TradeManager.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.TradeManager
{
    public class TradingRulesValidator
    {
        private static Dictionary<int, List<int>> _masterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();

        /// <summary>
        /// Validates the company trading rules.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public static bool ValidateCompanyTradingRules(OrderSingle order, double avgPrice, bool usedQuantityFieldAsNotional = false)
        {
            try
            {
                TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                double orderQty;
                //https://jira.nirvanasolutions.com:8443/browse/PRANA-38875
                if (order.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub ||
                    order.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                    orderQty = order.Quantity;
                else if (order.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub || order.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild)
                    orderQty = order.CumQtyForSubOrder;
                else if (order.IsManualOrder)
                    orderQty = order.Quantity;
                else
                    orderQty = order.CumQtyForSubOrder;

                if (ValidateUnallocatedRule(order, orderQty) && ValidateRestrictedAllowedRule(order, transferTradeRules))
                {
                    Dictionary<TradingRuleType, TradingRuleViolatedData> violatedTradingRules = new Dictionary<TradingRuleType, TradingRuleViolatedData>();
                    if ((bool)TradingTktPrefs.TradingTicketRulesPrefs.IsOverbuyTradingRule ||
                        (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsFatFingerTradingRule ||
                        (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsSharesOutstandingRule)
                    {
                        double notionalMultiplier = 1;
                        if (!usedQuantityFieldAsNotional)
                        {
                            if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
                            {
                                notionalMultiplier = order.Price * order.ContractMultiplier;
                            }
                            else if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop)
                            {
                                notionalMultiplier = order.StopPrice * order.ContractMultiplier;
                            }
                            else
                                notionalMultiplier = avgPrice * order.ContractMultiplier;
                        }

                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-39071
                        string parentClorderID = string.Empty;
                        if (order.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            if (order.ClOrderID != order.ParentClOrderID)
                            {
                                parentClorderID = order.ParentClOrderID;
                                order.ParentClOrderID = order.ClOrderID;
                            }
                        }

                        List<TaxLot> taxlots = ComplianceServiceConnector.GetInstance().GetTaxlotsForPreOrder(order, orderQty);

                        if (order.MsgType != FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            if (!string.IsNullOrEmpty(parentClorderID))
                                order.ParentClOrderID = parentClorderID;
                        }
                        List<TaxLot> deletedTaxlots = new List<TaxLot>();
                        if (taxlots == null)
                            taxlots = new List<TaxLot>();
                        else
                        {
                            deletedTaxlots = taxlots.Where(x => x.TaxLotState == Global.ApplicationConstants.TaxLotState.Deleted).ToList();
                            taxlots = taxlots.Where(x => x.Level1ID > 0 && x.TaxLotState != Global.ApplicationConstants.TaxLotState.Deleted).ToList();
                        }

                        Dictionary<int, double> ordAccountWisePosition = new Dictionary<int, double>();
                        Dictionary<int, double> ordMasterFundWisePosition = new Dictionary<int, double>();

                        foreach (TaxLot taxlot in taxlots)
                        {
                            double accountPostion = taxlot.ExecutedQty;
                            if (ordAccountWisePosition.ContainsKey(taxlot.Level1ID))
                                ordAccountWisePosition[taxlot.Level1ID] += accountPostion;
                            else
                                ordAccountWisePosition.Add(taxlot.Level1ID, accountPostion);

                            foreach (var mfAccountPair in _masterFundSubAccountAssociation)
                            {
                                if (mfAccountPair.Value.Contains(taxlot.Level1ID))
                                {
                                    int mfId = mfAccountPair.Key;
                                    if (ordMasterFundWisePosition.ContainsKey(mfId))
                                    {
                                        ordMasterFundWisePosition[mfId] += accountPostion;
                                    }
                                    else
                                    {
                                        ordMasterFundWisePosition.Add(mfId, accountPostion);
                                    }
                                    break;
                                }
                            }
                        }

                        TradingRuleViolatedData tradingRuleViolatedData = new TradingRuleViolatedData();
                        //isUnallocated will be required later if Product asks to check for individual accounts
                        if (ValidateOverbuyOversellRule(order, ordAccountWisePosition, orderQty, deletedTaxlots, out tradingRuleViolatedData))
                        {
                            if (tradingRuleViolatedData.TradingRuleViolatedParameter != null && tradingRuleViolatedData.TradingRuleViolatedParameter.Count > 0)
                                violatedTradingRules.Add(TradingRuleType.OverBuyOverSellRule, tradingRuleViolatedData);
                        }
                        else
                            return false;

                        if (ValidateFatFingerRule(order, notionalMultiplier, orderQty, ordAccountWisePosition, ordMasterFundWisePosition, out tradingRuleViolatedData))
                        {
                            if (tradingRuleViolatedData.TradingRuleViolatedParameter != null && tradingRuleViolatedData.TradingRuleViolatedParameter.Count > 0)
                                violatedTradingRules.Add(TradingRuleType.FatFingerRule, tradingRuleViolatedData);
                        }
                        else
                            return false;

                        if (ValidateSharesOutstandingRule(order, orderQty, ordAccountWisePosition, ordMasterFundWisePosition, deletedTaxlots, out tradingRuleViolatedData))
                        {
                            if (tradingRuleViolatedData.TradingRuleViolatedParameter != null && tradingRuleViolatedData.TradingRuleViolatedParameter.Count > 0)
                                violatedTradingRules.Add(TradingRuleType.SharesOutstandingRule, tradingRuleViolatedData);
                        }
                        else
                            return false;
                    }
                    if (violatedTradingRules.Count > 0)
                    {
                        if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                           && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            if (violatedTradingRules.ContainsKey(TradingRuleType.FatFingerRule))
                            {
                                for (int i = 0; i < violatedTradingRules[TradingRuleType.FatFingerRule].TradingRuleViolatedParameter.Count; i++)
                                {
                                    violatedTradingRules[(TradingRuleType.FatFingerRule)].TradingRuleViolatedParameter[i].NavPercent = null;
                                }
                            }
                            if (violatedTradingRules.ContainsKey(TradingRuleType.SharesOutstandingRule))
                            {
                                for (int i = 0; i < violatedTradingRules[TradingRuleType.SharesOutstandingRule].TradingRuleViolatedParameter.Count; i++)
                                {
                                    violatedTradingRules[(TradingRuleType.SharesOutstandingRule)].TradingRuleViolatedParameter[i].SharesOutstandingPercent = null;
                                }
                            }
                        }
                        TradingRulesViolatedPopUp tradingRulePopUp = new TradingRulesViolatedPopUp();
                        tradingRulePopUp.DataBind(violatedTradingRules);
                        tradingRulePopUp.ShowDialog();
                        if (tradingRulePopUp.ShouldTrade && violatedTradingRules.Keys.Contains(TradingRuleType.OverBuyOverSellRule))
                            order.IsOverbuyOversellAccepted = true;
                        return tradingRulePopUp.ShouldTrade;
                    }
                    else
                        return true;
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
            return false;
        }

        /// <summary>
        /// Validates the shares outstanding rule.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="orderAccountsWithPostionPct">The order accounts with postion PCT.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="cumQty">The cum qty.</param>
        /// <param name="isInMarketIncluded">if set to <c>true</c> [is in market included].</param>
        /// <param name="ruleParameters">The rule parameter.</param>
        /// <returns></returns>
        private static bool ValidateSharesOutstandingRule(OrderSingle order, double orderQty, Dictionary<int, double> ordAccountWisePosition, Dictionary<int, double> ordMasterFundWisePosition, List<TaxLot> deletedTaxlots, out TradingRuleViolatedData tradingRuleViolatedData)
        {
            tradingRuleViolatedData = new TradingRuleViolatedData();
            try
            {
                if ((bool)TradingTktPrefs.TradingTicketRulesPrefs.IsSharesOutstandingRule && order.AssetID == (int)AssetCategory.Equity && order.SwapParameters == null)
                {
                    int sideMultiplier;

                    if (order.OrderSideTagValue == FIXConstants.SIDE_Buy || order.OrderSideTagValue == FIXConstants.SIDE_Buy_Open)
                        sideMultiplier = 1;
                    else if (order.OrderSideTagValue == FIXConstants.SIDE_SellShort || order.OrderSideTagValue == FIXConstants.SIDE_Sell_Open)
                        sideMultiplier = -1;
                    else
                        return true;

                    double definedValue = (double)TradingTktPrefs.TradingTicketRulesPrefs.SharesOutstandingValue;
                    int fundSelectionType = (int)TradingTktPrefs.TradingTicketRulesPrefs.SharesOutstandingAccOrMF;
                    bool isInMarketIncluded = (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsInMarketIncluded;

                    Dictionary<int, decimal> dictAccountWisePosition = null;
                    decimal shareOutstanding = decimal.MinValue;
                    try
                    {
                        StringBuilder temp = new StringBuilder();
                        dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(order.Symbol, CachedDataManager.GetInstance.GetAllUserAccountList(), ref temp, false, isInMarketIncluded);
                        AdjustAccountWisePosition(dictAccountWisePosition, deletedTaxlots, sideMultiplier);
                        shareOutstanding = ExpnlServiceConnector.GetInstance().GetSymbolShareOutStanding(order.Symbol);
                        if (shareOutstanding <= 0)
                        {
                            shareOutstanding = GetSharesOutstandingFromSMorUser(order);
                        }
                    }
                    catch (Exception)
                    {
                        DialogResult dr = MessageBox.Show("Share Outstanding could not calculate due to expnl down" + Environment.NewLine + "Do you want to proceed?", "Nirvana Alert", MessageBoxButtons.YesNo);
                        return dr == DialogResult.Yes ? true : false;
                    }

                    if (ordAccountWisePosition == null || ordAccountWisePosition.Count == 0 || fundSelectionType == (int)FundSelectionType.Portfolio)
                    {
                        decimal existingPostion = dictAccountWisePosition.Values.Sum();

                        double newPositionPersentage = shareOutstanding == 0 ? 0 :
                            (((Convert.ToDouble(existingPostion) + (orderQty * sideMultiplier)) * 100d) / Convert.ToDouble(shareOutstanding));
                        if (Math.Abs(newPositionPersentage) >= definedValue)
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, isInMarketIncluded, orderQty, Math.Abs(newPositionPersentage), -1, Convert.ToDouble(existingPostion)));
                            tradingRuleViolatedData.TitleMessage = "The trade makes the portfolio position greater than " + definedValue + " % of the shares outstanding ";
                        }
                        else if (newPositionPersentage == 0)
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(new TradingRuleViolatedParameter());
                            tradingRuleViolatedData.TitleMessage = "Shares Outstanding not available for the " + order.Symbol;
                            tradingRuleViolatedData.AllowExpand = false;
                        }
                    }
                    else
                    {
                        if (fundSelectionType == (int)FundSelectionType.MasterFund)
                        {
                            foreach (var mfId in ordMasterFundWisePosition.Keys)
                            {
                                decimal totalMfExistingPosition = 0.0m;
                                _masterFundSubAccountAssociation[mfId].ForEach(accId => totalMfExistingPosition += dictAccountWisePosition.ContainsKey(accId) ? dictAccountWisePosition[accId] : 0);

                                double mfPositionPercentage = shareOutstanding == 0 ? 0 : ((ordMasterFundWisePosition[mfId] + Convert.ToDouble(totalMfExistingPosition)) * 100d) / Convert.ToDouble(shareOutstanding);

                                if (Math.Abs(mfPositionPercentage) >= definedValue)
                                {
                                    tradingRuleViolatedData.TradingRuleViolatedParameter.AddRange(GetRuleParameterListForMasterFunds(order, _masterFundSubAccountAssociation[mfId], isInMarketIncluded, ordAccountWisePosition, dictAccountWisePosition, sideMultiplier, Convert.ToDouble(shareOutstanding), true));
                                    tradingRuleViolatedData.TitleMessage = "The trade makes the masterfund position greater than " + definedValue + " % of the shares outstanding ";
                                }
                                else if (mfPositionPercentage == 0)
                                {
                                    tradingRuleViolatedData.TradingRuleViolatedParameter.Add(new TradingRuleViolatedParameter());
                                    tradingRuleViolatedData.TitleMessage = "Shares Outstanding not available for the " + order.Symbol;
                                    tradingRuleViolatedData.AllowExpand = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (int accountId in ordAccountWisePosition.Keys)
                            {
                                double newPosition = ordAccountWisePosition[accountId];
                                double existingPostion = dictAccountWisePosition.ContainsKey(accountId) ? Convert.ToDouble(dictAccountWisePosition[accountId]) : 0d;

                                double accountNewPositionPercentage = shareOutstanding == 0 ? 0 : ((existingPostion + newPosition) * 100d) / Convert.ToDouble(shareOutstanding);
                                if (Math.Abs(accountNewPositionPercentage) >= definedValue)
                                {
                                    tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, isInMarketIncluded, newPosition, Math.Abs(accountNewPositionPercentage), accountId, existingPostion));
                                    tradingRuleViolatedData.TitleMessage = "The trade makes the account position greater than " + definedValue + " % of the shares outstanding ";
                                }
                                else if (accountNewPositionPercentage == 0)
                                {
                                    tradingRuleViolatedData.TradingRuleViolatedParameter.Add(new TradingRuleViolatedParameter());
                                    tradingRuleViolatedData.TitleMessage = "Shares Outstanding not available for the " + order.Symbol;
                                    tradingRuleViolatedData.AllowExpand = false;
                                }
                            }
                        }
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
            }
            return false;
        }

        /// <summary>
        /// Adjusts the account wise position.
        /// </summary>
        /// <param name="accountWisePosition">The account wise position.</param>
        /// <param name="oldTaxlots">The old taxlots.</param>
        private static void AdjustAccountWisePosition(Dictionary<int, decimal> accountWisePosition, List<TaxLot> oldTaxlots, int sideMultiplier)
        {
            try
            {
                bool inMarket = (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsInMarketIncluded;
                if (accountWisePosition != null && oldTaxlots != null)
                {
                    foreach (TaxLot taxlot in oldTaxlots)
                    {
                        if (!accountWisePosition.ContainsKey(taxlot.Level1ID))
                        {
                            accountWisePosition[taxlot.Level1ID] = 0m;
                        }
                        if (inMarket)
                        {
                            accountWisePosition[taxlot.Level1ID] -= sideMultiplier * (decimal)taxlot.Quantity;
                        }
                        else
                        {
                            accountWisePosition[taxlot.Level1ID] -= sideMultiplier * (decimal)taxlot.ExecutedQty;
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
        }

        /// <summary>
        /// Validates the fat finger rule.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="avgPrice">The average price.</param>
        /// <param name="usedQuantityFieldAsNotional">if set to <c>true</c> [used quantity field as notional].</param>
        /// <param name="orderAccountsWithPostionPct">The order accounts with postion PCT.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="cumQty">The cum qty.</param>
        /// <param name="isInMarketIncluded">if set to <c>true</c> [is in market included].</param>
        /// <returns></returns>
        private static bool ValidateFatFingerRule(OrderSingle order, double notionalMultiplier, double orderQty, Dictionary<int, double> ordAccountWisePosition, Dictionary<int, double> ordMasterFundWisePosition, out TradingRuleViolatedData tradingRuleViolatedData)
        {
            tradingRuleViolatedData = new TradingRuleViolatedData();
            try
            {
                if ((bool)TradingTktPrefs.TradingTicketRulesPrefs.IsFatFingerTradingRule)
                {
                    StringBuilder errorString = new StringBuilder();
                    double definedValue = (double)TradingTktPrefs.TradingTicketRulesPrefs.DefineFatFingerValue;
                    int masterFundOrAccount = (int)TradingTktPrefs.TradingTicketRulesPrefs.FatFingerAccountOrMasterFund;
                    int AbsAmountOrDefinePercent = (int)TradingTktPrefs.TradingTicketRulesPrefs.IsAbsoluteAmountOrDefinePercent;
                    bool isInMarketIncluded = (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsInMarketIncluded;

                    //Calculating the FxRate
                    double fxRate = order.FXRate;
                    string fXConversionMethodOperator = order.FXConversionMethodOperator;

                    if (order.CurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    {
                        //FxRate not entered on the TT so fetching from Daily Valuation
                        if (fxRate == 0.0)
                        {
                            ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID())
                       .GetConversionRateFromCurrencies(order.CurrencyID, CachedDataManager.GetInstance.GetCompanyBaseCurrencyID(),0);
                            if (conversionRate != null)
                            {
                                if (conversionRate.RateValue > double.Epsilon)
                                {
                                    fxRate = (double)conversionRate.RateValue;
                                    fXConversionMethodOperator = conversionRate.ConversionMethod.ToString();
                                }
                            }
                        }
                        //FxRate not avaialble on DailyValuation as well so FatFinger rule cannot be validated
                        if (fxRate == 0.0)
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(new TradingRuleViolatedParameter());
                            tradingRuleViolatedData.TitleMessage = "Cannot evaluate Fat-finger rule because FX Rate isn’t available or entered";
                            tradingRuleViolatedData.AllowExpand = false;
                            return true;
                        }
                        else
                        {
                            fxRate = fXConversionMethodOperator.Equals("M") ? fxRate : (1 / fxRate);
                            notionalMultiplier = notionalMultiplier * fxRate;
                        }
                    }

                    //Unallocated order
                    if (ordAccountWisePosition == null || ordAccountWisePosition.Count == 0)
                    {
                        if (AbsAmountOrDefinePercent == (int)AbsoluteAmountOrDefinePercent.AbsoluteAmount)
                        {
                            double notional = notionalMultiplier * orderQty;
                            if (notional > definedValue)
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, false, orderQty, notional, -1));
                                tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue;
                            }
                        }
                        else
                        {
                            List<int> accountIds = CachedDataManager.GetInstance.GetAllUserAccountList();
                            decimal existingNav = 0m;
                            try
                            {
                                Dictionary<int, decimal> dictExistingNav = ExpnlServiceConnector.GetInstance().GetAccountNAV(accountIds, ref errorString);
                                existingNav = Math.Abs(dictExistingNav.Values.Sum());
                            }
                            catch (Exception)
                            {
                                DialogResult dr = MessageBox.Show("NAV could not calculate due to expnl down" + Environment.NewLine + "Do you want to proceed?", "Nirvana Alert", MessageBoxButtons.YesNo);
                                return dr == DialogResult.Yes ? true : false;
                            }
                            double newPositionPersentage = existingNav == 0m ? 0 : ((notionalMultiplier * orderQty * 100) / Convert.ToDouble(existingNav));
                            if (newPositionPersentage > definedValue)
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, false, orderQty, newPositionPersentage, -1));
                                tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue + " % of the NAV ";
                            }
                        }
                    }
                    else //Allocated order
                    {
                        if (AbsAmountOrDefinePercent == (int)AbsoluteAmountOrDefinePercent.AbsoluteAmount)
                        {
                            if (masterFundOrAccount == (int)PTTMasterFundOrAccount.MasterFund)
                            {
                                foreach (var mfId in ordMasterFundWisePosition.Keys)
                                {
                                    double notional = ordMasterFundWisePosition[mfId] * notionalMultiplier;
                                    if (notional > definedValue)
                                    {
                                        tradingRuleViolatedData.TradingRuleViolatedParameter.AddRange(GetRuleParameterListForMasterFunds(order, _masterFundSubAccountAssociation[mfId], false, ordAccountWisePosition, null, notionalMultiplier, notional, false));
                                        tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var accountId in ordAccountWisePosition.Keys)
                                {
                                    double notional = ordAccountWisePosition[accountId] * notionalMultiplier;
                                    if (notional > definedValue)
                                    {
                                        tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, false, ordAccountWisePosition[accountId], notional, accountId, 0));
                                        tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Dictionary<int, Decimal> dictExistingNav = null;
                            List<int> accountIds = new List<int>();

                            if (masterFundOrAccount == (int)PTTMasterFundOrAccount.MasterFund)
                            {
                                foreach (var mfId in ordMasterFundWisePosition.Keys)
                                {
                                    accountIds.AddRange(_masterFundSubAccountAssociation[mfId]);
                                }
                                try
                                {
                                    dictExistingNav = ExpnlServiceConnector.GetInstance().GetAccountNAV(accountIds, ref errorString);
                                }
                                catch (Exception)
                                {
                                    DialogResult dr = MessageBox.Show("NAV could not calculate due to expnl down" + Environment.NewLine + "Do you want to proceed?", "Nirvana Alert", MessageBoxButtons.YesNo);
                                    return dr == DialogResult.Yes ? true : false;
                                }
                                foreach (var mfId in ordMasterFundWisePosition.Keys)
                                {
                                    decimal totalMfNav = 0.0m;
                                    _masterFundSubAccountAssociation[mfId].ForEach(accId => totalMfNav += dictExistingNav[accId]);
                                    totalMfNav = Math.Abs(totalMfNav);
                                    double mfNavPercentage = totalMfNav == 0 ? 0 : ((ordMasterFundWisePosition[mfId] * notionalMultiplier * 100d) / Convert.ToDouble(totalMfNav));
                                    if (mfNavPercentage > definedValue)
                                    {
                                        tradingRuleViolatedData.TradingRuleViolatedParameter.AddRange(GetRuleParameterListForMasterFunds(order, _masterFundSubAccountAssociation[mfId], false, ordAccountWisePosition, dictExistingNav, notionalMultiplier, Convert.ToDouble(totalMfNav), false));
                                        tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue + " % of the NAV ";
                                    }
                                }
                            }
                            else
                            {
                                accountIds = ordAccountWisePosition.Keys.ToList();
                                try
                                {
                                    dictExistingNav = ExpnlServiceConnector.GetInstance().GetAccountNAV(accountIds, ref errorString);
                                }
                                catch (Exception)
                                {
                                    DialogResult dr = MessageBox.Show("NAV could not calculate due to expnl down" + Environment.NewLine + "Do you want to proceed?", "Nirvana Alert", MessageBoxButtons.YesNo);
                                    return dr == DialogResult.Yes ? true : false;
                                }
                                foreach (var accountId in ordAccountWisePosition.Keys)
                                {
                                    decimal accountNav = Math.Abs(dictExistingNav[accountId]);
                                    double accountNavPercentage = accountNav == 0 ? 0 : ((ordAccountWisePosition[accountId] * notionalMultiplier * 100d) / Convert.ToDouble(accountNav));
                                    if (accountNavPercentage > definedValue)
                                    {
                                        tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleViolatedParamaterForAccount(order, false, ordAccountWisePosition[accountId], accountNavPercentage, accountId, 0));
                                        tradingRuleViolatedData.TitleMessage = "The notional value of the trade exceeds " + definedValue + " % of the NAV ";
                                    }
                                }
                            }
                        }
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
            }
            return false;
        }

        /// <summary>
        /// Validates the overbuy oversell rule.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="cumQty">The cum qty.</param>
        /// <param name="isInMarketIncluded">if set to <c>true</c> [is in market included].</param>
        /// <param name="tradingRulePopUp">The trading rule pop up.</param>
        /// <returns></returns>
        private static bool ValidateOverbuyOversellRule(OrderSingle order, Dictionary<int, double> ordAccountWisePosition, double cumQty, List<TaxLot> deletedTaxlots, out TradingRuleViolatedData tradingRuleViolatedData)
        {
            tradingRuleViolatedData = new TradingRuleViolatedData();
            try
            {
                bool isInMarketIncluded = (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsInMarketIncluded;
                List<int> accountIds = ordAccountWisePosition.Keys.ToList();

                if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) && (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsOverbuyTradingRule)
                {
                    StringBuilder temp = new StringBuilder();
                    // Unallocated case
                    if (ordAccountWisePosition == null || ordAccountWisePosition.Count == 0)
                    {
                        Dictionary<int, decimal> dictAccountWisePosition = null;
                        dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(order.Symbol, CachedDataManager.GetInstance.GetAllUserAccountList(), ref temp, false, isInMarketIncluded);
                        AdjustAccountWisePosition(dictAccountWisePosition, deletedTaxlots, 1);
                        double portfolioPosition = Convert.ToDouble(dictAccountWisePosition.Values.Sum());
                        //position is already positive and we are trying to increase it by adding more buy to close then rule should trigger
                        if (portfolioPosition >= 0)
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, -1, portfolioPosition, cumQty));
                            tradingRuleViolatedData.TitleMessage = "The trade is an Overbuy, as it is adding more to the current position";
                        }
                        else if (cumQty > Math.Abs(portfolioPosition))
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, -1, portfolioPosition, cumQty));
                            tradingRuleViolatedData.TitleMessage = "The trade is an Overbuy, as its quantity is more than the current position";
                        }
                    }
                    else
                    {
                        Dictionary<int, decimal> dictAccountWisePosition = null;
                        bool isViolatedMessageApplied = false;
                        dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(order.Symbol, accountIds, ref temp, false, isInMarketIncluded);
                        AdjustAccountWisePosition(dictAccountWisePosition, deletedTaxlots, 1);
                        foreach (var accountId in ordAccountWisePosition.Keys)
                        {
                            double currentPostion = dictAccountWisePosition.Count > 0 && dictAccountWisePosition.ContainsKey(accountId) ? Convert.ToDouble(dictAccountWisePosition[accountId]) : 0;
                            double tradeQty = ordAccountWisePosition[accountId];
                            //position is already positive and we are trying to increase it by adding more buy to close then rule should trigger
                            if (currentPostion >= 0)
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, accountId, currentPostion, tradeQty));
                                if (!isViolatedMessageApplied)
                                    tradingRuleViolatedData.TitleMessage = "The trade is an Overbuy, as it is adding more to the current position";
                            }
                            else if (tradeQty > Math.Abs(currentPostion))
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, accountId, currentPostion, tradeQty));
                                tradingRuleViolatedData.TitleMessage = "The trade is an Overbuy, as its quantity is more than the current position";
                                isViolatedMessageApplied = true;
                            }
                        }
                    }
                }
                else if ((order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)) && (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsOversellTradingRule)
                {
                    StringBuilder temp = new StringBuilder();
                    // Unallocated case
                    if (ordAccountWisePosition == null || ordAccountWisePosition.Count == 0)
                    {
                        Dictionary<int, decimal> dictAccountWisePosition = null;
                        dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(order.Symbol, CachedDataManager.GetInstance.GetAllUserAccountList(), ref temp, false, isInMarketIncluded);
                        AdjustAccountWisePosition(dictAccountWisePosition, deletedTaxlots, -1);
                        double portfolioPosition = Convert.ToDouble(dictAccountWisePosition.Values.Sum());
                        //position is already negative and we are trying to over sell by adding more sell and sell to close
                        if (portfolioPosition <= 0)
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, -1, portfolioPosition, cumQty));
                            tradingRuleViolatedData.TitleMessage = "The trade is an Oversell, as it is adding more to the current position";
                        }
                        else if (cumQty > Math.Abs(portfolioPosition))
                        {
                            tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, -1, portfolioPosition, cumQty));
                            tradingRuleViolatedData.TitleMessage = "The trade is an Oversell, as its quantity is more than the current position";
                        }
                    }
                    else
                    {
                        //  double buyPostion = double.MinValue;
                        Dictionary<int, decimal> dictAccountWisePosition = null;
                        bool isViolatedMessageApplied = false;
                        dictAccountWisePosition = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(order.Symbol, accountIds, ref temp, false, isInMarketIncluded);
                        AdjustAccountWisePosition(dictAccountWisePosition, deletedTaxlots, -1);
                        foreach (var accountId in ordAccountWisePosition.Keys)
                        {
                            double currentPosition = dictAccountWisePosition.Count > 0 && dictAccountWisePosition.ContainsKey(accountId) ? Convert.ToDouble(dictAccountWisePosition[accountId]) : 0;
                            double tradeQty = ordAccountWisePosition[accountId];
                            //position is already negative and we are trying to over sell by adding more sell and sell to close
                            if (currentPosition <= 0)
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, accountId, currentPosition, tradeQty));
                                if (!isViolatedMessageApplied)
                                    tradingRuleViolatedData.TitleMessage = "The trade is an Oversell, as it is adding more to the current position";
                            }
                            else if (tradeQty > Math.Abs(currentPosition))
                            {
                                tradingRuleViolatedData.TradingRuleViolatedParameter.Add(GetRuleParameterListForOverByOverSell(order, accountId, currentPosition, tradeQty));
                                tradingRuleViolatedData.TitleMessage = "The trade is an Oversell, as its quantity is more than the current position";
                                isViolatedMessageApplied = true;
                            }
                        }
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

        /// <summary>
        /// Validates the unallocated rule.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="orderQty">The order qty.</param>
        /// <returns></returns>
        private static bool ValidateUnallocatedRule(OrderSingle order, double orderQty)
        {
            try
            {
                string errorMsg = (order.Level1ID == int.MinValue && (bool)TradingTktPrefs.TradingTicketRulesPrefs.IsUnallocatedTradeAlert && ComplianceCacheManager.GetPreTradeCheck(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID)) ? string.Format("Transaction of {0} {1} is not allocated to any Account. This may result in some pre-trade compliance rules unable to be checked - Do you want to proceed ?", orderQty, order.Symbol) : string.Empty;
                if (errorMsg != string.Empty)
                {
                    Prana.TradeManager.PromptWindow promptWin = new Prana.TradeManager.PromptWindow(errorMsg, "Warning");
                    promptWin.SetPropertiesForRestrictedSymbol();
                    promptWin.ShowInTaskbar = false;
                    promptWin.ShowDialog();
                    return promptWin.ShouldTrade;
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

        /// <summary>
        /// Validates the restricted allowed rule.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="transferTradeRules">The transfer trade rules.</param>
        /// <returns></returns>
        private static bool ValidateRestrictedAllowedRule(OrderSingle order, TranferTradeRules transferTradeRules)
        {
            try
            {
                string errorMsg = string.Empty;
                string bloombergOrTickerSymbol = (bool)TradingTktPrefs.IsTickerSymbologySecuritiesList ? order.Symbol : order.BloombergSymbol;
                if ((bool)transferTradeRules.IsAllowAllowedSecuritiesList)
                {
                    if (!TradingTktPrefs.RestrictedAllowedSecuritiesList.Contains(bloombergOrTickerSymbol))
                    {
                        errorMsg = "The trade is blocked because the symbol is not present in the allowed list." + Environment.NewLine + Environment.NewLine + "Do you want to continue?";
                    }
                }
                else if ((bool)transferTradeRules.IsAllowRestrictedSecuritiesList)
                {
                    if (TradingTktPrefs.RestrictedAllowedSecuritiesList.Contains(bloombergOrTickerSymbol))
                    {
                        errorMsg = "The trade is blocked because the symbol is present in the restricted list." + Environment.NewLine + Environment.NewLine + "Do you want to continue?";
                    }
                }
                if (errorMsg != string.Empty)
                {
                    Prana.TradeManager.PromptWindow promptWin = new Prana.TradeManager.PromptWindow(errorMsg, "Trading Rule Violated");
                    promptWin.SetPropertiesForRestrictedSymbol();
                    promptWin.ShowInTaskbar = false;
                    promptWin.ShowDialog();
                    return promptWin.ShouldTrade;
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

        /// <summary>
        /// Show the pop up and update the value of shareOutstanding in SM 
        /// </summary>
        private static decimal GetSharesOutstandingFromSMorUser(OrderSingle order)
        {
            decimal sharesOutstanding = 0;
            try
            {
                double resp = TradeManager.GetInstance().GetShareOutstandingValueFromSM(order.Symbol);
                if (resp > double.Epsilon)
                    sharesOutstanding = (decimal)resp;
                else
                {
                    PromptWindow promptWin = new PromptWindow("Please enter shares outstanding for " + order.Symbol, "No Shares Outstanding defined for " + order.Symbol);
                    promptWin.SetPropertiesForSharesOutstanding();
                    promptWin.ShowInTaskbar = false;
                    promptWin.ShowDialog();

                    if (promptWin.ShouldTrade)
                    {
                        TradeManager.GetInstance().SaveShareOutstandingValueInSM(order.Symbol, (Double)promptWin.SharesPresent);
                        sharesOutstanding = promptWin.SharesPresent;
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
            return sharesOutstanding;
        }

        /// <summary>
        /// Create the list for binding the data
        /// </summary>
        private static TradingRuleViolatedParameter GetRuleViolatedParamaterForAccount(OrderSingle order, bool isInMarketIncluded, double cumQty, double definedValue, int accountId, double currentPosition = 0)
        {
            TradingRuleViolatedParameter ruleParameter = new TradingRuleViolatedParameter();
            try
            {
                if (accountId.Equals(-1))
                {
                    ruleParameter.Symbol = order.Symbol;
                    ruleParameter.TradeQuantity = cumQty;
                    ruleParameter.CurrentPosition = Convert.ToDouble(currentPosition);
                    ruleParameter.SharesOutstandingPercent = definedValue;
                    ruleParameter.NavPercent = definedValue;
                }
                else
                {
                    List<int> id1 = new List<int> { accountId };
                    int masterFundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(accountId);
                    ruleParameter.Symbol = order.Symbol;
                    ruleParameter.CurrentPosition = Convert.ToDouble(currentPosition);
                    ruleParameter.TradeQuantity = cumQty;
                    ruleParameter.AccountName = CachedDataManager.GetInstance.GetAccount(accountId);
                    ruleParameter.MasterFund = CachedDataManager.GetInstance.GetMasterFund(masterFundId);
                    ruleParameter.SharesOutstandingPercent = definedValue;
                    ruleParameter.NavPercent = definedValue;
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
            return ruleParameter;
        }

        /// <summary>
        /// Create the List for Binding the Data
        /// </summary>
        private static TradingRuleViolatedParameter GetRuleParameterListForOverByOverSell(OrderSingle order, int accountId, double currentPosition, double tradeQty)
        {
            TradingRuleViolatedParameter tradingRulePopUpParameter = new TradingRuleViolatedParameter();
            try
            {
                //when trade is unallocated
                if (accountId == -1)
                {
                    tradingRulePopUpParameter.Symbol = order.Symbol;
                    tradingRulePopUpParameter.TradeQuantity = tradeQty;
                    tradingRulePopUpParameter.CurrentPosition = currentPosition;
                }
                else
                {
                    tradingRulePopUpParameter.Symbol = order.Symbol;
                    tradingRulePopUpParameter.TradeQuantity = tradeQty;
                    tradingRulePopUpParameter.CurrentPosition = currentPosition;
                    tradingRulePopUpParameter.AccountName = CachedDataManager.GetInstance.GetAccount(accountId);
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
            return tradingRulePopUpParameter;
        }

        /// <summary>
        /// Create the List for Binding the Data
        /// </summary>
        private static List<TradingRuleViolatedParameter> GetRuleParameterListForMasterFunds(OrderSingle order, List<int> accountIds, bool isInMarketIncluded, Dictionary<int, double> accountWiseQty, Dictionary<int, decimal> dictExistingPositionOrNav, double notionalMultiplier, double shareOutstandingOrMfNav, bool isShareOutstanding)
        {
            List<TradingRuleViolatedParameter> ruleParameterList = new List<TradingRuleViolatedParameter>();
            List<int> allAccountIds = accountWiseQty.Keys.ToList();
            try
            {
                foreach (int id in allAccountIds)
                {
                    int masterFundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(id);
                    if (accountIds.Contains(id))
                    {
                        TradingRuleViolatedParameter tradingRulePopUpParameter = new TradingRuleViolatedParameter();
                        tradingRulePopUpParameter.Symbol = order.Symbol;
                        tradingRulePopUpParameter.TradeQuantity = accountWiseQty[id];
                        tradingRulePopUpParameter.AccountName = CachedDataManager.GetInstance.GetAccount(id);
                        tradingRulePopUpParameter.MasterFund = CachedDataManager.GetInstance.GetMasterFund(masterFundId);

                        if (isShareOutstanding)
                        {
                            double newPosition = Convert.ToDouble(accountWiseQty[id]);
                            double existingPostion = (dictExistingPositionOrNav != null && dictExistingPositionOrNav.ContainsKey(id)) ? Convert.ToDouble(dictExistingPositionOrNav[id]) : 0d;
                            double accountNewPositionPercentage = shareOutstandingOrMfNav == 0 ? 0 : ((existingPostion + newPosition) * 100d) / Convert.ToDouble(shareOutstandingOrMfNav);
                            if (dictExistingPositionOrNav.Count > 0 && dictExistingPositionOrNav.ContainsKey(id))
                            {
                                tradingRulePopUpParameter.CurrentPosition = Convert.ToDouble(dictExistingPositionOrNav[id]);
                            }
                            else
                                tradingRulePopUpParameter.CurrentPosition = 0;
                            tradingRulePopUpParameter.SharesOutstandingPercent = accountNewPositionPercentage;
                            tradingRulePopUpParameter.NavPercent = accountNewPositionPercentage;
                        }
                        else
                        {
                            if ((int)TradingTktPrefs.TradingTicketRulesPrefs.IsAbsoluteAmountOrDefinePercent == 0)
                            {
                                double accountNavPercentageForMf = shareOutstandingOrMfNav == 0 ? 0 : ((accountWiseQty[id] * notionalMultiplier * 100d) / Convert.ToDouble(shareOutstandingOrMfNav));
                                tradingRulePopUpParameter.SharesOutstandingPercent = accountNavPercentageForMf;
                                tradingRulePopUpParameter.NavPercent = accountNavPercentageForMf;
                            }

                            else
                            {
                                double notional = accountWiseQty[id] * notionalMultiplier;
                                tradingRulePopUpParameter.SharesOutstandingPercent = notional;
                                tradingRulePopUpParameter.NavPercent = notional;
                            }
                        }

                        ruleParameterList.Add(tradingRulePopUpParameter);
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
            return ruleParameterList;
        }

    }
}
