using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDatabaseAccess;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.CommonDataCache
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ForexConverter : IPublishing, IDisposable
    {
        /// <summary>
        /// TODO : Need to have a multiple key dictionary
        /// CHMW-3132	Account wise fx rate handling for expiration settlement
        /// </summary>
        Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> _forexConversionHash = null;
        Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> _forexConversionHashForGivenDate = null;
        private static readonly object _forexConversionHashLocker = new object();
        DuplexProxyBase<ISubscription> _proxy;
        List<string> _standardCurrencyPairs = null;
        private static ForexConverter _forexConverter = null;
        private static int _companyID = int.MinValue;
        private static DateTime _date = DateTime.Now.Date;
        private IClientsCommonDataManager _clientsCommonDataManager;

        private ForexConverter()
        {
            try
            {
                if (_companyID == int.MinValue)
                {
                    throw new Exception("Company Id information not supplied.");
                }
                _clientsCommonDataManager = WindsorContainerManager.Container.Resolve<IClientsCommonDataManager>();
                if (_forexConversionHash == null)
                {
                    GetForexData();
                }
                GetStandardCurrencyPairs();
                CreateSubscriptionServicesProxy();
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

        public static ForexConverter GetInstance(int companyID)
        {
            try
            {
                if (_forexConverter == null)
                {
                    _companyID = companyID;
                    _forexConverter = new ForexConverter();
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
            return _forexConverter;
        }

        public static ForexConverter GetInstance(int companyID, DateTime date)
        {
            try
            {
                if (_forexConverter == null)
                {
                    _companyID = companyID;
                    _date = date;
                    _forexConverter = new ForexConverter();
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
            return _forexConverter;
        }

        private void GetStandardCurrencyPairs()
        {
            try
            {
                _standardCurrencyPairs = new List<string>();
                DataSet dt = _clientsCommonDataManager.GetAllStandardCurrencyPairs();
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    string fromCurrency = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(int.Parse(dr["FromCurrencyID"].ToString()));
                    string toCurrency = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(int.Parse(dr["ToCurrencyID"].ToString()));
                    _standardCurrencyPairs.Add(fromCurrency + Seperators.SEPERATOR_7 + toCurrency);
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

        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_ForexRate, null);
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

        public ConversionRate GetConversionRateForCurrencyToBaseCurrency(long orderdAUECID, int accountID)
        {
            ConversionRate objConversionRate = null;
            try
            {
                objConversionRate = GetDefaultConversionrateObject();
                if (_forexConversionHash == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                int leadCurrencyID = _clientsCommonDataManager.GetCurrencyByAUECID(orderdAUECID).CurrencyID;
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == leadCurrencyID)
                {
                    // do nothing, as it would be default to 1
                    objConversionRate.RateValue = 1.0;
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrencies(leadCurrencyID, accountBaseCurrencyID, accountID);
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
            return objConversionRate;
        }

        public ConversionRate GetConversionRateForAUECIdToBaseCurrency(long orderdAUECID, DateTime auecTradeDate, int accountID)
        {
            ConversionRate objConversionRate = null;
            try
            {
                objConversionRate = GetDefaultConversionrateObject();
                if (_forexConversionHash == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                int leadCurrencyID = _clientsCommonDataManager.GetCurrencyByAUECID(orderdAUECID).CurrencyID;
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == leadCurrencyID)
                {
                    // do nothing, as it would be default to 1
                    objConversionRate.RateValue = 1.0;
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrencies(leadCurrencyID, accountBaseCurrencyID, auecTradeDate, accountID);
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
            return objConversionRate;
        }

        public ConversionRate GetConversionRateForCurrencyToBaseCurrency(int fromCurrencyID, DateTime date, int accountID)
        {
            ConversionRate objConversionRate = null;
            try
            {
                objConversionRate = GetDefaultConversionrateObject();
                if (_forexConversionHash == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == fromCurrencyID)
                {
                    // do nothing, as it would be default to 1
                    objConversionRate.RateValue = 1;
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrencies(fromCurrencyID, accountBaseCurrencyID, date, accountID);
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
            return objConversionRate;
        }

        public ConversionRate GetConversionRateForCurrencyToBaseCurrencyForGivenDate(int fromCurrencyID, DateTime date, int accountID)
        {
            ConversionRate objConversionRate = null;
            try
            {
                objConversionRate = GetDefaultConversionrateObject();
                if (_forexConversionHashForGivenDate == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == fromCurrencyID)
                {
                    // do nothing, as it would be default to 1
                    objConversionRate.RateValue = 1;
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrenciesForGivenDate(fromCurrencyID, accountBaseCurrencyID, date, accountID);
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
            return objConversionRate;
        }

        public ConversionRate GetConversionRateForCurrencyToBaseCurrency(int fromCurrencyID, int accountID)
        {
            try
            {
                return GetConversionRateForCurrencyToBaseCurrency(fromCurrencyID, DateTime.UtcNow, accountID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public ConversionRate GetConversionRateFromCurrencies(int fromCurrencyID, int toCurrencyID, int accountID)
        {
            try
            {
                return GetConversionRateFromCurrencies(fromCurrencyID, toCurrencyID, DateTime.UtcNow.Date, accountID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public ConversionRate GetConversionRateFromCurrencies(string tickerSymbol, int accountID)
        {
            try
            {
                return GetConversionRateFromCurrencies(tickerSymbol, DateTime.UtcNow.Date, accountID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the conversion rate from currencies.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="date">The date.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        public ConversionRate GetConversionRateFromCurrencies(string key, DateTime date, int accountID)
        {
            try
            {
                lock (_forexConversionHashLocker)
                {
                    if (!_forexConversionHash.ContainsKey(accountID) || !_forexConversionHash[accountID].ContainsKey(key))
                    {
                        //set default account if account preference does not exist
                        accountID = 0;
                    }
                    ////CHMW-3132	Account wise fx rate handling for expiration settlement
                    if (_forexConversionHash.ContainsKey(accountID) && _forexConversionHash[accountID].ContainsKey(key))
                    {
                        ConversionRate objAvailabeConversionRateForZero = null;
                        ConversionRate objAvailabeConversionRate = GetAvailabeConversionRate(_forexConversionHash[accountID][key], date);
                        if (_forexConversionHash.ContainsKey(0) && _forexConversionHash[0].ContainsKey(key))
                        {
                            objAvailabeConversionRateForZero = GetAvailabeConversionRate(_forexConversionHash[0][key], date);
                        }
                        if (objAvailabeConversionRateForZero != null && objAvailabeConversionRate != null)
                        {
                            return objAvailabeConversionRateForZero.Date > objAvailabeConversionRate.Date ? objAvailabeConversionRateForZero : objAvailabeConversionRate;
                        }
                        else if (objAvailabeConversionRate != null)
                        {
                            return objAvailabeConversionRate;
                        }
                        else if (objAvailabeConversionRateForZero != null)
                        {
                            return objAvailabeConversionRateForZero;
                        }
                    }
                }
                return GetDefaultConversionrateObject();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }

        }

        public ConversionRate GetConversionRateFromCurrencies(int fromCurrencyID, int toCurrencyID, DateTime date, int accountID)
        {
            try
            {
                string fromCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(fromCurrencyID);
                string toCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(toCurrencyID);
                if (fromCurrName.Equals(toCurrName))
                {
                    return GetDefaultConversionRateObjectForTwinPair(date);
                }
                else
                {
                    string key = Convert.ToString(fromCurrName + Seperators.SEPERATOR_7 + toCurrName).ToString();
                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                    return GetConversionRateFromCurrencies(key, date, accountID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public ConversionRate GetConversionRateFromCurrenciesForGivenDate(int fromCurrencyID, int toCurrencyID, DateTime date, int accountID)
        {
            try
            {
                string key = Convert.ToString(fromCurrencyID + Seperators.SEPERATOR_7 + toCurrencyID).ToString() + Seperators.SEPERATOR_7 + Convert.ToDateTime(date.ToString()).Date;
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                lock (_forexConversionHashLocker)
                {
                    if (!_forexConversionHashForGivenDate.ContainsKey(accountID) || !_forexConversionHashForGivenDate[accountID].ContainsKey(key))
                    {
                        //set default account if account preference does not exist
                        accountID = 0;
                    }
                    if (_forexConversionHashForGivenDate.ContainsKey(accountID) && _forexConversionHashForGivenDate[accountID].ContainsKey(key))
                    {
                        SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate = _forexConversionHashForGivenDate[accountID][key];
                        ConversionRate objavailabeConversionRate = GetAvailabeConversionRate(dateWiseConversionRate, date);
                        if (objavailabeConversionRate != null)
                        {
                            return objavailabeConversionRate;
                        }
                    }
                }
                return GetDefaultConversionrateObject();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public ConversionRate GetLatestAvailableFxRatesLessThanToday(int currencyId)
        {
            ConversionRate conversionRate = new ConversionRate();
            try
            {
                if (!currencyId.Equals(1))
                {
                    DataSet ds = CachedDataManager.GetInstance.GetLatestAvailableFxRatesLessThanToday();
                    if (ds.Tables.Count > 0)
                    {
                        DataRow[] dr = ds.Tables[0].Select("ToCurrencyId=" + currencyId.ToString());
                        if (dr.Length > 0)
                        {
                            conversionRate.RateValue = Convert.ToDouble(dr[0]["ConversionRate"]);
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
            return conversionRate;
        }

        private ConversionRate GetAvailabeConversionRate(SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate, DateTime date)
        {
            ConversionRate objConversionRate = null;
            try
            {
                if (!dateWiseConversionRate.TryGetValue(date.Date, out objConversionRate))
                {
                    foreach (KeyValuePair<DateTime, ConversionRate> keyVal in dateWiseConversionRate)
                    {
                        if (keyVal.Key.Date < date.Date)
                        {
                            if (dateWiseConversionRate.TryGetValue(keyVal.Key.Date, out objConversionRate))
                                break;
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
            return objConversionRate;
        }

        //private ConversionRate GetAvailableConversionRate(SortedDictionary<DateTime, ConversionRate> dateWiseConversionRate, DateTime date, bool isFetchForGivenDateOnly)
        //{
        //    ConversionRate objConversionRate = null;
        //    if (dateWiseConversionRate.ContainsKey(date.Date))
        //    {
        //        objConversionRate = dateWiseConversionRate[date.Date];
        //    }
        //    else if (!isFetchForGivenDateOnly)
        //    {
        //        foreach (KeyValuePair<DateTime, ConversionRate> keyVal in dateWiseConversionRate)
        //        {
        //            if (keyVal.Key.Date < date.Date)
        //            {
        //                if (dateWiseConversionRate.ContainsKey(keyVal.Key.Date))
        //                {
        //                    objConversionRate = dateWiseConversionRate[keyVal.Key.Date];
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return objConversionRate;
        //}

        private ConversionRate GetDefaultConversionrateObject()
        {
            ConversionRate objConversionRate = new ConversionRate();
            try
            {
                objConversionRate.RateValue = 0.0;
                objConversionRate.ConversionMethod = Operator.M;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return objConversionRate;
        }

        private ConversionRate GetDefaultConversionRateObjectForTwinPair(DateTime date)
        {
            ConversionRate objConversionRate = new ConversionRate();
            try
            {
                objConversionRate.RateValue = 1.0;
                objConversionRate.ConversionMethod = Operator.M;
                objConversionRate.Date = date;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return objConversionRate;
        }

        public string GetTradedCurrencyToBaseConversionRateCaption(long orderdAUECID, int accountID)
        {
            ConversionRate objConversionRate = null;
            string conversionFactorCaption = "1";
            try
            {
                if (_forexConversionHash == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                //TODO: Instead of making a DB call. Store AUEC and Currency ID's in cache.
                ///Found Traded currency
                int currencyID = _clientsCommonDataManager.GetCurrencyByAUECID(orderdAUECID).CurrencyID;
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == currencyID)
                {
                    // do nothing, as it would be default to 1
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrencies(currencyID, accountBaseCurrencyID, accountID);
                    conversionFactorCaption = GetFXRateCaptionFromConversionRate(objConversionRate);
                }
                return conversionFactorCaption;
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
                return conversionFactorCaption;
            }
        }

        public string GetFXRateCaptionFromConversionRate(ConversionRate objConversionRate)
        {
            string conversionFactorCaption = string.Empty;
            try
            {
                if (objConversionRate != null)
                {
                    if (objConversionRate.ConversionMethod == Operator.M)
                    {
                        conversionFactorCaption = objConversionRate.RateValue.ToString();
                    }
                    else if (objConversionRate.ConversionMethod == Operator.D)
                    {
                        conversionFactorCaption = "1 / " + objConversionRate.RateValue.ToString();
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
            return conversionFactorCaption;
        }

        public string GetPranaForexSymbolFromCurrencies(int fromCurrency, int toCurrency)
        {
            try
            {
                string fromCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(fromCurrency);
                string toCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(toCurrency);
                string forexSymbol = Convert.ToString(fromCurrName + Seperators.SEPERATOR_7 + toCurrName).ToString();
                return forexSymbol;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public string GetPranaForexSymbolFromCurrencyToBaseCurrency(int fromCurrency)
        {
            try
            {
                string fromCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(fromCurrency);
                string toCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID());
                string forexSymbol = Convert.ToString(fromCurrName + Seperators.SEPERATOR_7 + toCurrName).ToString();
                return forexSymbol;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public string GetStandradPairSymbol(int fromCurrencyID, int toCurrencyID, ref bool samePairExist)
        {
            try
            {
                if (_standardCurrencyPairs.Contains(GetPranaForexSymbolFromCurrencies(fromCurrencyID, toCurrencyID)))
                {
                    samePairExist = true;
                    return GetPranaForexSymbolFromCurrencies(fromCurrencyID, toCurrencyID);
                }
                else if (_standardCurrencyPairs.Contains(GetPranaForexSymbolFromCurrencies(toCurrencyID, fromCurrencyID)))
                {
                    samePairExist = false;
                    return GetPranaForexSymbolFromCurrencies(toCurrencyID, fromCurrencyID);
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
            return string.Empty;
        }

        public double GetTradedCurrencyToBaseConversionRate(long orderdAUECID, int accountID)
        {
            ConversionRate objConversionRate = null;
            double conversionFactor = 1;
            try
            {
                if (_forexConversionHash == null)
                {
                    throw new Exception("Forex conversion rates cache not available.");
                }
                //TODO: Instead of making a DB call. Store AUEC and Currency ID's in cache.
                ///Found Traded currency
                int leadCurrencyID = _clientsCommonDataManager.GetCurrencyByAUECID(orderdAUECID).CurrencyID;
                //CHMW-3132	Account wise fx rate handling for expiration settlement
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID == leadCurrencyID)
                {
                    // do nothing, as it would be default to 1
                }
                else
                {
                    objConversionRate = GetConversionRateFromCurrencies(leadCurrencyID, accountBaseCurrencyID, accountID);
                    if (objConversionRate != null)
                    {
                        conversionFactor = objConversionRate.RateValue;
                        //if (objConversionRate.ConversionMethod == Operator.M)
                        //{
                        //    conversionFactor = objConversionRate.RateValue;
                        //}
                        //else if (objConversionRate.ConversionMethod == Operator.D)
                        //{
                        //    conversionFactor = 1.0 / objConversionRate.RateValue;
                        //}
                    }
                }
                return conversionFactor;
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
                return conversionFactor;
            }
        }

        public double GetFxRateFromCurrenciesForGivenDateOnly(int fromCurrencyID, int toCurrencyID, DateTime date, int accountID)
        {
            double fxRateFromToVs = 0;
            try
            {
                ConversionRate objConversionRate = GetConversionRateFromCurrencies(fromCurrencyID, toCurrencyID, date, accountID);
                if (objConversionRate == null)
                {
                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                    int accountBaseCurrencyID;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                    }
                    else
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    //check for cross currency pair as it might be possible that the rate doesnt exist in cache in that case we will fetch the rate by dividing the fromtoBaseRate by VsToBase rate...
                    if (fromCurrencyID != accountBaseCurrencyID && toCurrencyID != accountBaseCurrencyID)
                    {
                        ConversionRate conversionRateFromToBase = GetConversionRateForCurrencyToBaseCurrency(fromCurrencyID, date, accountID);
                        ConversionRate conversionRateVsToBase = GetConversionRateForCurrencyToBaseCurrency(toCurrencyID, date, accountID);
                        if (conversionRateFromToBase != null && conversionRateVsToBase != null)
                        {
                            double RateFromTOBase = conversionRateFromToBase.RateValue;
                            double RateVsTOBase = conversionRateVsToBase.RateValue;
                            if (conversionRateFromToBase.ConversionMethod == Operator.D && conversionRateFromToBase.RateValue != 0)
                            {
                                RateFromTOBase = 1 / RateFromTOBase;
                            }
                            if (conversionRateVsToBase.ConversionMethod == Operator.D && conversionRateVsToBase.RateValue != 0)
                            {
                                RateVsTOBase = 1 / RateVsTOBase;
                            }
                            if (RateVsTOBase != 0)
                            {
                                fxRateFromToVs = RateFromTOBase / RateVsTOBase;
                            }
                        }
                    }
                }
                else if (objConversionRate.Date.Date <= date.Date)
                {
                    fxRateFromToVs = objConversionRate.RateValue;
                    if (objConversionRate.ConversionMethod == Operator.D && objConversionRate.RateValue != 0)
                    {
                        fxRateFromToVs = 1 / fxRateFromToVs;
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
            return fxRateFromToVs;
        }

        public void GetForexData()
        {
            try
            {
                ///Fill values in _forexConversionHash;
                //_companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID(); //_clientsCommonDataManager.GetCompanyBaseCurrency(_companyID);
                _forexConversionHash = _clientsCommonDataManager.GetFXConversionRates_New();
                _forexConversionHashForGivenDate = _clientsCommonDataManager.GetFXConversionRatesForGivenDate(_date, null);
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

        public void GetForexData(DateTime date)
        {
            try
            {
                _date = date;
                lock (_forexConversionHashLocker)
                {
                    _forexConversionHashForGivenDate = _clientsCommonDataManager.GetFXConversionRatesForGivenDate(_date, null);
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

        public Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> GetForexData(DateTime startDate, DateTime endDate)
        {
            Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> forexGivenDate = new Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>>();
            try
            {
                forexGivenDate = _clientsCommonDataManager.GetFXConversionRatesForGivenDate(startDate, endDate);
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
            return forexGivenDate;
        }


        public void ClearForexConversionHashForGivenDateCache()
        {
            try
            {
                lock (_forexConversionHashLocker)
                {
                    _forexConversionHashForGivenDate.Clear();
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
        /// Updates the forex conversion cache.
        /// </summary>
        /// <param name="dtforexRates">The dtforex rates.</param>
        public void UpdateForexConversionCache(DataTable dtforexRates)
        {
            ConversionRate conversionRateStanPair = null;
            try
            {
                lock (_forexConversionHashLocker)
                {
                    bool isAccountColumn = dtforexRates.Columns.Contains("AccountID");
                    foreach (DataRow dRow in dtforexRates.Rows)
                    {
                        int fromCurrencyID = int.Parse((dRow["FromCurrencyID"]).ToString());
                        int toCurrencyID = int.Parse((dRow["ToCurrencyID"]).ToString());
                        DateTime forexDate = DateTime.Parse(dRow["Date"].ToString());
                        double fxRate = double.Parse(dRow["ConversionFactor"].ToString());
                        string symbol = dRow["Symbol"].ToString();
                        //TODO Handle in Daily valuation while Integration
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-7214
                        int accountID = 0;
                        if (isAccountColumn)
                        {
                            accountID = Int32.Parse(dRow["AccountID"].ToString());
                        }
                        conversionRateStanPair = new ConversionRate();
                        conversionRateStanPair.RateValue = fxRate;
                        conversionRateStanPair.FXeSignalSymbol = symbol;
                        conversionRateStanPair.Date = forexDate;
                        conversionRateStanPair.ConversionMethod = Operator.M;
                        conversionRateStanPair.AccountID = accountID;
                        string fromCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(fromCurrencyID);
                        string toCurrName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(toCurrencyID);
                        string key = Convert.ToString(fromCurrName + Seperators.SEPERATOR_7 + toCurrName).ToString();
                        DateTime date = conversionRateStanPair.Date.Date;
                        ////CHMW-3132	Account wise fx rate handling for expiration settlement

                        UpdateForexConversionDictForValues(_forexConversionHash, accountID, date, key, conversionRateStanPair);

                        string keyInverse = Convert.ToString(toCurrName + Seperators.SEPERATOR_7 + fromCurrName).ToString();
                        ConversionRate conversionRateInversePair = (ConversionRate)conversionRateStanPair.Clone();
                        conversionRateInversePair.ConversionMethod = Operator.D;

                        UpdateForexConversionDictForValues(_forexConversionHash, accountID, date, keyInverse, conversionRateInversePair);

                        //MUKUL: update forexConversionHash for given date if the fxrate has been updated for any date with inthe last 20 days...
                        if (date.Date <= _date.Date && date.Date >= _date.AddDays(-20).Date)
                        {
                            string keyNew = Convert.ToString(fromCurrencyID + Seperators.SEPERATOR_7 + toCurrencyID).ToString() + Seperators.SEPERATOR_7 + Convert.ToDateTime(date.ToString()).Date;
                            string keyInverseNew = Convert.ToString(toCurrencyID + Seperators.SEPERATOR_7 + fromCurrencyID).ToString() + Seperators.SEPERATOR_7 + Convert.ToDateTime(date.ToString()).Date;
                            UpdateForexConversionDictForValues(_forexConversionHashForGivenDate, accountID, date, keyNew, conversionRateStanPair);
                            UpdateForexConversionDictForValues(_forexConversionHashForGivenDate, accountID, date, keyInverseNew, conversionRateInversePair);
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
        /// Updates the given forex conversion dictionary for the values passed as parameters.
        /// </summary>
        /// <param name="forexDict">The forex dictionary.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="key">The key.</param>
        /// <param name="rate">The rate.</param>
        private void UpdateForexConversionDictForValues(Dictionary<int, Dictionary<string, SortedDictionary<DateTime, ConversionRate>>> forexDict, int accountID, DateTime date, string key, ConversionRate rate)
        {
            try
            {
                if (forexDict.ContainsKey(accountID))
                {
                    if (forexDict[accountID].ContainsKey(key))
                    {
                        forexDict[accountID][key][date] = rate;
                    }
                    else
                    {
                        forexDict[accountID].Add(key, new SortedDictionary<DateTime, ConversionRate>() { { date, rate } });
                    }
                }
                else
                {
                    SortedDictionary<DateTime, ConversionRate> sortedDicDateWiseNew = new SortedDictionary<DateTime, ConversionRate>() { { date, rate } };
                    Dictionary<string, SortedDictionary<DateTime, ConversionRate>> dict = new Dictionary<string, SortedDictionary<DateTime, ConversionRate>>() { { key, sortedDicDateWiseNew } };
                    forexDict.Add(accountID, dict);
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

        public ConversionRate GetConversionRateFromAndToCurrenciesForAGivenDateAndAccount(int fromCurrencyId, int toCurrencyId, int accountId, DateTime date)
        {
            ConversionRate conversionRate = null;
            try
            {
                conversionRate = _clientsCommonDataManager.GetFXConversionRatesForADateAndAccount(fromCurrencyId, toCurrencyId, accountId, date);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return conversionRate;
        }

        /// <summary>
        /// Retrieves all standard currency pairs as a list of tuples containing FromCurrencyID and ToCurrencyID.
        /// </summary>
        public List<(int FromCurrencyID, int ToCurrencyID)> GetStandardCurrencyPairsWithIDs()
        {
            var pairs = new List<(int, int)>();
            try
            {
                DataSet dt = _clientsCommonDataManager.GetAllStandardCurrencyPairs();
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    int fromID = int.Parse(dr["FromCurrencyID"].ToString());
                    int toID = int.Parse(dr["ToCurrencyID"].ToString());
                    pairs.Add((fromID, toID));
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
            return pairs;
        }

        #region IPublishing Members
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                System.Object[] dataList = null;
                switch (topicName)
                {
                    case Topics.Topic_ForexRate:
                        dataList = (System.Object[])e.EventData;
                        List<string> listData = new List<string>();
                        foreach (object obj in dataList)
                        {
                            listData.Add(obj.ToString());
                        }
                        DataTable dt = DataTableToListConverter.GetDataTableFromList(listData);
                        UpdateForexConversionCache(dt);
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

        public string getReceiverUniqueName()
        {
            return "ForexConverter";
        }
        #endregion

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (_forexConversionHashForGivenDate != null)
                {
                    _forexConversionHashForGivenDate.Clear();
                    _forexConversionHashForGivenDate = null;
                }
                if (_forexConversionHash != null)
                {
                    _forexConversionHash.Clear();
                    _forexConversionHash = null;
                }
                if (isDisposing && _proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_ForexRate);
                    _proxy.Dispose();
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
        #endregion
    }
}