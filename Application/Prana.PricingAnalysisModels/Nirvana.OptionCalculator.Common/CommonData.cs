using Prana.BusinessObjects.NewLiveFeed;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    public class CommonData
    {
        private static CommonData _singleton = null;
        private static object _locker = new object();
        static Dictionary<string, CurrencyConversions> _currenciesToUpdate = new Dictionary<string, CurrencyConversions>();
        public Dictionary<string, CurrencyConversions> CurrenciesToUpdate
        {
            get { return _currenciesToUpdate; }
            set { _currenciesToUpdate = value; }
        }

        static CommonData()
        {
            try
            {
                SetCommonData();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private static void SetCommonData()
        {
            try
            {
                _currenciesToUpdate = GetCurrenciesToUpdate();
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
        private static Dictionary<string, CurrencyConversions> GetCurrenciesToUpdate()
        {
            Dictionary<string, CurrencyConversions> dictLowerCurrencies = new Dictionary<string, CurrencyConversions>();
            try
            {
                System.Collections.Specialized.NameValueCollection lowerCurrencies = new System.Collections.Specialized.NameValueCollection();
                lowerCurrencies = ConfigurationHelper.Instance.LoadSectionBySectionName("CurrenciesToUpdate");
                if (lowerCurrencies != null && lowerCurrencies.Count > 0)
                {
                    for (int counter = 0; counter < lowerCurrencies.Count; counter++)
                    {
                        List<string> valueList = new List<string>(lowerCurrencies[lowerCurrencies.GetKey(counter)].Split(','));
                        CurrencyConversions higherCurrencyConversion = new CurrencyConversions();
                        higherCurrencyConversion.HigherCurrency = valueList[0];
                        higherCurrencyConversion.OperatorToApply = valueList[1];
                        higherCurrencyConversion.AdjustValue = Convert.ToDouble(valueList[2]);

                        if (dictLowerCurrencies.ContainsKey(lowerCurrencies.GetKey(counter)))
                        {
                            dictLowerCurrencies[lowerCurrencies.GetKey(counter)] = higherCurrencyConversion;
                        }
                        else
                        {
                            dictLowerCurrencies.Add(lowerCurrencies.GetKey(counter), higherCurrencyConversion);
                        }
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
            return dictLowerCurrencies;
        }
        public static CommonData GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new CommonData();
                    }
                }
            }
            return _singleton;
        }
    }
}
