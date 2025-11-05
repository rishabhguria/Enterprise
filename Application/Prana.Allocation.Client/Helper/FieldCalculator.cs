using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Utilities;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Client.Helper
{
    internal static class FieldCalculator
    {
        /// <summary>
        /// Calculates the field value.
        /// </summary>
        /// <param name="calculatedFields">The calculated fields.</param>
        /// <returns></returns>
        internal static object CalculateFieldValue(CalculatedFields calculatedFields)
        {
            try
            {
                string fieldName = calculatedFields.FieldName;
                if (calculatedFields.AssetId != (int)AssetCategory.FX && calculatedFields.AssetId != (int)AssetCategory.Forex && calculatedFields.AssetId != (int)AssetCategory.FXForward)
                {
                    if (calculatedFields.AssetsWithCommissionInNetAmount != null && calculatedFields.AssetsWithCommissionInNetAmount.Count > 0 && calculatedFields.AssetsWithCommissionInNetAmount.Contains(calculatedFields.AssetId))
                    {
                        switch (fieldName)
                        {
                            case AllocationUIConstants.NETAMOUNT_LOCAL:
                                return String.Format(calculatedFields.PrecisionFormat, (calculatedFields.TotalCommissionAndFee * calculatedFields.SideMultiplier) + calculatedFields.AccruedInterest);

                            case AllocationUIConstants.NETAMOUNT_BASE:
                                return String.Format(calculatedFields.PrecisionFormat, ((calculatedFields.TotalCommissionAndFee * calculatedFields.SideMultiplier) + calculatedFields.AccruedInterest) * calculatedFields.Fxrate);
                        }
                    }
                    else
                    {
                        switch (fieldName)
                        {
                            case AllocationUIConstants.NETAMOUNT_LOCAL:
                                return String.Format(calculatedFields.PrecisionFormat, (calculatedFields.NotionalValue + (calculatedFields.TotalCommissionAndFee * calculatedFields.SideMultiplier) + calculatedFields.AccruedInterest));

                            case AllocationUIConstants.NETAMOUNT_BASE:
                                return String.Format(calculatedFields.PrecisionFormat, ((calculatedFields.NotionalValue + (calculatedFields.TotalCommissionAndFee * calculatedFields.SideMultiplier) + calculatedFields.AccruedInterest) * calculatedFields.Fxrate));
                        }
                    }
                    switch (fieldName)
                    {
                        case AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL:
                            return String.Format(calculatedFields.PrecisionFormat, calculatedFields.NotionalValue);

                        case AllocationUIConstants.PRINCIPAL_AMOUNT_BASE:
                            return String.Format(calculatedFields.PrecisionFormat, (calculatedFields.NotionalValue * calculatedFields.Fxrate));

                        case AllocationUIConstants.AVGPRICE_BASE:
                            return String.Format(calculatedFields.PrecisionFormat, (calculatedFields.AvgPrice * calculatedFields.Fxrate));

                        case AllocationUIConstants.EXECUTION_TIME:
                            return calculatedFields.AuecLocaDateTime.ToString();
                        case AllocationUIConstants.COUNTER_CURRENCY_AMOUNT:
                        case AllocationUIConstants.COUNTER_CURRENCY:
                            return "NA";
                    }
                }
                else
                {
                    int baseCurr = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    switch (fieldName)
                    {
                        case AllocationUIConstants.NETAMOUNT_LOCAL:
                        case AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL:
                            {
                                double amountLocal = 0;
                                if (baseCurr != calculatedFields.CurrencyId)
                                {
                                    amountLocal = calculatedFields.CumQty;
                                }
                                else if (baseCurr != calculatedFields.VSCurrencyId)
                                {
                                    amountLocal = calculatedFields.CumQty * calculatedFields.AvgPrice;
                                }
                                else
                                {
                                    amountLocal = calculatedFields.AvgPrice.Equals(0) ? 0 : (calculatedFields.CumQty / calculatedFields.AvgPrice);
                                }
                                return String.Format(calculatedFields.PrecisionFormat, amountLocal);
                            }
                        case AllocationUIConstants.NETAMOUNT_BASE:
                        case AllocationUIConstants.PRINCIPAL_AMOUNT_BASE:
                            {
                                double amountBase = 0;
                                if (baseCurr.Equals(calculatedFields.CurrencyId))
                                {
                                    amountBase = calculatedFields.CumQty;
                                }
                                else if (baseCurr.Equals(calculatedFields.VSCurrencyId))
                                {
                                    amountBase = calculatedFields.CumQty * calculatedFields.AvgPrice;
                                }
                                else
                                {
                                    amountBase = calculatedFields.AvgPrice.Equals(0) ? 0 : (calculatedFields.CumQty / calculatedFields.AvgPrice);
                                }
                                return String.Format(calculatedFields.PrecisionFormat, amountBase);
                            }
                        case AllocationUIConstants.AVGPRICE_BASE:
                            {
                                double avgMultiplier = 0;
                                if (calculatedFields.VSCurrencyId.Equals(baseCurr))
                                    avgMultiplier = calculatedFields.AvgPrice;
                                else if (calculatedFields.AvgPrice != 0)
                                    avgMultiplier = 1 / calculatedFields.AvgPrice;
                                return String.Format(calculatedFields.PrecisionFormat, avgMultiplier);
                            }
                        case AllocationUIConstants.EXECUTION_TIME:
                            return calculatedFields.AuecLocaDateTime.ToString();
                        case AllocationUIConstants.COUNTER_CURRENCY_AMOUNT:
                            {
                                double avgMultiplier = 0;
                                if (calculatedFields.LeadCurrencyId.Equals(calculatedFields.CurrencyId))
                                {
                                    avgMultiplier = calculatedFields.AvgPrice;
                                }
                                else if (calculatedFields.AvgPrice != 0)
                                {
                                    avgMultiplier = 1 / calculatedFields.AvgPrice;
                                }
                                return String.Format(calculatedFields.PrecisionFormat, -(calculatedFields.CumQty * avgMultiplier * calculatedFields.SideMultiplier));
                            }
                        case AllocationUIConstants.COUNTER_CURRENCY:
                            return CachedDataManager.GetInstance.GetCurrencyText(calculatedFields.LeadCurrencyId.Equals(calculatedFields.CurrencyId) ? calculatedFields.VSCurrencyId : calculatedFields.LeadCurrencyId);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <returns></returns>
        internal static object GetAccountName(List<TaxLot> taxlots)
        {
            string result = string.Empty;
            try
            {
                result = (taxlots.DistinctBy(x => x.Level1Name).Count() > 1) ? ApplicationConstants.C_Multiple : taxlots.FirstOrDefault().Level1Name;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the name of the MasterFund.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <returns></returns>
        internal static string GetMasterFundName(AllocationGroup allocationGroup)
        {
            string result = string.Empty;
            try
            {
                if (allocationGroup.TaxLots.Count() == 0)
                {
                    result = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(allocationGroup.TradeAttribute6) ? allocationGroup.TradeAttribute6 : string.Empty;
                }
                else if ((allocationGroup.TaxLots.DistinctBy(x => CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(x.Level1ID)).Count() > 1))
                {

                    result = ApplicationConstants.C_Multiple;
                }
                else
                {
                    var masterfundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(allocationGroup.TaxLots.FirstOrDefault().Level1ID);
                    result = CachedDataManager.GetInstance.GetMasterFund(masterfundId);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }




        /// <summary>
        /// Gets the name of the strategy.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <returns></returns>
        internal static object GetStrategyName(List<TaxLot> taxlots)
        {
            string result = string.Empty;
            try
            {
                result = (taxlots.DistinctBy(x => x.Level2Name).Count() > 1) ? ApplicationConstants.C_Multiple : taxlots.FirstOrDefault().Level2Name;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the name of the import file.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        internal static object GetImportFileName(List<AllocationOrder> orders)
        {
            string result = string.Empty;
            try
            {
                bool isMultipleFileNames = false;

                if (orders.Count > 0 && orders[0].ImportFileLogObj != null)
                {
                    string firstFileName = orders[0].ImportFileLogObj.ImportFileName;
                    if (orders.Where(ord => ord.ImportFileLogObj != null).Any(ord => ord.ImportFileLogObj.ImportFileName != null && !ord.ImportFileLogObj.ImportFileName.Equals(firstFileName)))
                        isMultipleFileNames = true;
                    result = (isMultipleFileNames) ? "Multiple Files" : firstFileName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the asset category.
        /// </summary>
        /// <param name="isSwapped">if set to <c>true</c> [is swapped].</param>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        internal static object GetAssetCategory(bool isSwapped, int assetId, string assetName)
        {
            string result = string.Empty;
            try
            {
                result = (assetId == (int)AssetCategory.Equity && isSwapped) ? AllocationClientConstants.CONSTANT_EQUITY_SWAP : assetName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the settlement currency.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        internal static object GetSettlementCurrency(int settlementCurrencyID)
        {
            string result = string.Empty;
            try
            {
                Dictionary<int, string> currencies = CachedDataManager.GetInstance.GetAllCurrencies();
                result = currencies.ContainsKey(settlementCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[settlementCurrencyID] : string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }


    }
}
