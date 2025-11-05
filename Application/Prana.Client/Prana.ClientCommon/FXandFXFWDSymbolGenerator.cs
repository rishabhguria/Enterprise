using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.ClientCommon
{
    /// <summary>
    /// 
    /// </summary>
    public class FXandFXFWDSymbolGenerator
    {
        /// <summary>
        /// Determines whether [is valid fx and forward symbol] [the specified sec master base object].
        /// </summary>
        /// <param name="secMasterBaseObj">The sec master base object.</param>
        /// <returns></returns>
        public static bool IsValidFxAndFwdSymbol(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                bool restrictIncorrectFormatedFxAndFwdSymbol = false;
                Boolean.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("RestrictIncorrectFormatedFxAndFwdSymbol"), out restrictIncorrectFormatedFxAndFwdSymbol);
                if (restrictIncorrectFormatedFxAndFwdSymbol)
                {
                    AssetCategory assetCategory = (AssetCategory)secMasterBaseObj.AssetID;

                    if (assetCategory == AssetCategory.FX)
                    {
                        SecMasterFxObj secMasterFxObj = (SecMasterFxObj)secMasterBaseObj;
                        if ((secMasterFxObj.LeadCurrencyID != int.MinValue && secMasterFxObj.VsCurrencyID != int.MinValue) && (secMasterFxObj.LeadCurrencyID != secMasterFxObj.VsCurrencyID) && secMasterBaseObj.CurrencyID != int.MinValue)
                        {
                            string tempTickerSymbol = CachedDataManager.GetInstance.GetCurrencyText(secMasterFxObj.LeadCurrencyID) + "/" + CachedDataManager.GetInstance.GetCurrencyText(secMasterFxObj.VsCurrencyID);

                            if (tempTickerSymbol.Equals(secMasterBaseObj.TickerSymbol))
                                return true;
                        }
                    }
                    else if (assetCategory == AssetCategory.FXForward)
                    {
                        SecMasterFXForwardObj secMasterFXForwardObj = (SecMasterFXForwardObj)secMasterBaseObj;
                        if ((secMasterFXForwardObj.LeadCurrencyID != int.MinValue && secMasterFXForwardObj.VsCurrencyID != int.MinValue) && (secMasterFXForwardObj.LeadCurrencyID != secMasterFXForwardObj.VsCurrencyID) && secMasterFXForwardObj.CurrencyID != int.MinValue)
                        {
                            string tempTickerSymbol = CachedDataManager.GetInstance.GetCurrencyText(secMasterFXForwardObj.LeadCurrencyID) + "/" + CachedDataManager.GetInstance.GetCurrencyText(secMasterFXForwardObj.VsCurrencyID);
                            string dateFormat = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FXTickerSymbolDateFormat);
                            if (!string.IsNullOrWhiteSpace(dateFormat))
                                tempTickerSymbol = tempTickerSymbol + " " + secMasterFXForwardObj.ExpirationDate.ToString(dateFormat).ToUpper();
                            else
                                tempTickerSymbol = tempTickerSymbol + " " + secMasterFXForwardObj.ExpirationDate.ToString("MM/dd/yyyy").ToUpper();

                            if (tempTickerSymbol.Equals(secMasterBaseObj.TickerSymbol))
                                return true;
                        }
                    }
                }
                else
                {
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
        /// Determines whether [is valid fx and forward symbol] [the specified order single].
        /// </summary>
        /// <param name="orderSingle">The order single.</param>
        /// <returns></returns>
        public static bool IsValidFxAndFwdSymbol(OrderSingle orderSingle)
        {
            try
            {
                bool restrictIncorrectFormatedFxAndFwdSymbol = false;
                Boolean.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("RestrictIncorrectFormatedFxAndFwdSymbol"), out restrictIncorrectFormatedFxAndFwdSymbol);
                if (restrictIncorrectFormatedFxAndFwdSymbol)
                {
                    AssetCategory assetCategory = (AssetCategory)orderSingle.AssetID;

                    if (assetCategory == AssetCategory.FX)
                    {
                        if (orderSingle.CurrencyID != int.MinValue)
                        {
                            string tempTickerSymbol = CachedDataManager.GetInstance.GetCurrencyText(orderSingle.LeadCurrencyID) + "/" + CachedDataManager.GetInstance.GetCurrencyText(orderSingle.VsCurrencyID);

                            if (tempTickerSymbol.Equals(orderSingle.Symbol))
                                return true;
                        }
                    }
                    else if (assetCategory == AssetCategory.FXForward)
                    {
                        if (orderSingle.CurrencyID != int.MinValue)
                        {
                            string tempTickerSymbol = CachedDataManager.GetInstance.GetCurrencyText(orderSingle.LeadCurrencyID) + "/" + CachedDataManager.GetInstance.GetCurrencyText(orderSingle.VsCurrencyID);
                            string dateFormat = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FXTickerSymbolDateFormat);

                            if (!string.IsNullOrWhiteSpace(dateFormat))
                                tempTickerSymbol = tempTickerSymbol + " " + orderSingle.ExpirationDate.ToString(dateFormat).ToUpper();
                            else
                                tempTickerSymbol = tempTickerSymbol + " " + orderSingle.ExpirationDate.ToString("MM/dd/yyyy").ToUpper();

                            if (tempTickerSymbol.Equals(orderSingle.Symbol))
                                return true;
                        }
                    }
                }
                else
                {
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
    }
}


