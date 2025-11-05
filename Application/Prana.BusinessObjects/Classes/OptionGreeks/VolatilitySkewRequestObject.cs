using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class VolSkewObject
    {
        public VolSkewObject(PranaPositionWithGreeks position)
        {
            _strikePrice = position.StrikePrice;
            _daysToExpiration = GetDaysToExpiration(position.ExpirationDate);
            _symbol = position.Symbol;
            _underlyingSymbol = position.UnderlyingSymbol;
            _expirationDate = position.ExpirationDate;
            _putOrCall = position.PutOrCall;
            _categoryCode = (AssetCategory)position.AssetID;
        }

        //maintaining a dictionary for step Analysis as for each step there will be a new proxy strike value
        Dictionary<string, double> _dictProxyStrikes = new Dictionary<string, double>();

        public Dictionary<string, double> DictProxyStrikes
        {
            get { return _dictProxyStrikes; }
            set { _dictProxyStrikes = value; }
        }

        private readonly Dictionary<string, List<string>> _dictProxySymbols = new Dictionary<string, List<string>>();
        public VolSkewObject()
        {

        }

        private Dictionary<string, DateTime> _dictProxyExpirationDates = new Dictionary<string, DateTime>();

        public Dictionary<string, DateTime> DictProxyExpirationDates
        {
            get { return _dictProxyExpirationDates; }
            set { _dictProxyExpirationDates = value; }
        }


        StepAnalParameterCode _parameterCode = StepAnalParameterCode.UnderlyingPrice;

        public StepAnalParameterCode ParameterCode
        {
            get { return _parameterCode; }
            set { _parameterCode = value; }
        }

        private int _putOrCall;
        public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _proxySymbol;
        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private double _proxyVolatility;
        public double ProxyVolatility
        {
            get { return _proxyVolatility; }
            set { _proxyVolatility = value; }
        }

        private double _strikePrice = double.MinValue;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        AssetCategory _categoryCode = AssetCategory.None;
        public AssetCategory CategoryCode
        {
            get { return _categoryCode; }
            set { _categoryCode = value; }
        }

        private double _proxyStrikePrice = double.MinValue;
        public double ProxyStrikePrice
        {
            get { return _proxyStrikePrice; }
            set { _proxyStrikePrice = value; }
        }

        private double _percentageIN_OUTofMoney = double.MinValue;
        public double PercentageIN_OUTofMoney
        {
            get { return _percentageIN_OUTofMoney; }
            set { _percentageIN_OUTofMoney = value; }
        }


        private double _proxyStrikeMin = double.MinValue;
        [Browsable(false)]
        public double ProxyStrikeMin
        {
            get { return _proxyStrikeMin; }
            set { _proxyStrikeMin = value; }
        }

        private double _proxyStrikeMax = double.MinValue;
        [Browsable(false)]
        public double ProxyStrikeMax
        {
            get { return _proxyStrikeMax; }
            set { _proxyStrikeMax = value; }
        }

        private int _daysToExpiration;
        public int DaysToExpiration
        {
            get { return _daysToExpiration; }
            set { _daysToExpiration = value; }
        }

        private int _proxyDaysToExpiration;
        public int ProxyDaysToExpiration
        {
            get { return _proxyDaysToExpiration; }
            set { _proxyDaysToExpiration = value; }
        }

        private double _underlyingPrice;
        public double UnderlyingPrice
        {
            get { return _underlyingPrice; }
            set { _underlyingPrice = value; }
        }

        private double _optionPrice;
        public double OptionPrice
        {
            get { return _optionPrice; }
            set { _optionPrice = value; }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private DateTime _proxyExpirationDate;
        public DateTime ProxyExpirationDate
        {
            get { return _proxyExpirationDate; }
            set { _proxyExpirationDate = value; }
        }

        public void SetProxyStrikeRangeValues()
        {
            List<double> listProxyStrikes = new List<double>();
            listProxyStrikes.AddRange(_dictProxyStrikes.Values);

            //sorting in ascending order
            listProxyStrikes.Sort((delegate (double p1, double p2) { return (p1.CompareTo(p2)); }));
            if (listProxyStrikes.Count > 0)
            {
                _proxyStrikeMin = listProxyStrikes[0];
                _proxyStrikeMax = listProxyStrikes[listProxyStrikes.Count - 1];
            }
        }

        //for step Analysis
        public void UpdateProxySymbolDictionary(string stepValue, string proxySymbol, int proxyExpirationMonth)
        {
            lock (_dictProxySymbols)
            {
                if (_parameterCode.Equals(StepAnalParameterCode.UnderlyingPrice))
                {
                    if (_dictProxySymbols.ContainsKey(proxySymbol))
                    {
                        _dictProxySymbols[proxySymbol].Add(stepValue);
                    }
                    else
                    {
                        List<string> listStepValues = new List<string>();
                        listStepValues.Add(stepValue);
                        _dictProxySymbols.Add(proxySymbol, listStepValues);
                    }
                    // _dictProxyStrikes.Remove(stepValue);
                }
                else if (_parameterCode.Equals(StepAnalParameterCode.DaysToExpiration))
                {
                    List<string> listKeys = new List<string>();

                    foreach (KeyValuePair<string, DateTime> kp in _dictProxyExpirationDates)
                    {
                        DateTime expirationDate = kp.Value;
                        if (expirationDate.Month == proxyExpirationMonth)
                        {
                            if (_dictProxySymbols.ContainsKey(proxySymbol))
                            {
                                _dictProxySymbols[ProxySymbol].Add(kp.Key);
                            }
                            else
                            {
                                List<string> listStepValues = new List<string>();
                                listStepValues.Add(kp.Key);
                                _dictProxySymbols.Add(proxySymbol, listStepValues);
                            }
                            listKeys.Add(kp.Key);
                        }
                    }

                    foreach (string key in listKeys)
                    {
                        _dictProxyExpirationDates.Remove(key);
                    }
                }
            }
        }

        public void RemoveFromProxyStrikeDictionary(List<string> listStepKeys)
        {
            foreach (string key in listStepKeys)
            {
                if (_dictProxyStrikes.ContainsKey(key))
                {
                    _dictProxyStrikes.Remove(key);
                }
            }
        }

        public List<string> GetStepkeyValuesForProxySymbol(string proxySymbol)
        {
            List<string> listKeys = null;

            lock (_dictProxySymbols)
            {
                if (_dictProxySymbols.ContainsKey(proxySymbol))
                {
                    listKeys = _dictProxySymbols[proxySymbol];
                    _dictProxySymbols.Remove(proxySymbol);
                }

            }
            return listKeys;
        }

        public List<DateTime> GetProxyExpirationDatesForUniqueMonths()
        {

            List<DateTime> listUniqueExpirationDates = new List<DateTime>();
            List<int> listMonths = new List<int>();

            foreach (DateTime date in _dictProxyExpirationDates.Values)
            {
                if (!listMonths.Contains(date.Month))
                {
                    listMonths.Add(date.Month);
                    listUniqueExpirationDates.Add(date);
                }
            }


            return listUniqueExpirationDates;
        }

        public int GetDaysToExpiration(DateTime expirationDate)
        {
            int daysToExpiration = 0;
            if (expirationDate > DateTime.Now)
            {
                TimeSpan dateDiff = expirationDate - DateTime.Now;
                daysToExpiration = dateDiff.Days + 1;
            }
            return daysToExpiration;
        }

        public bool IsProxyVolCalculatedForAllSymbols()
        {
            lock (_dictProxySymbols)
            {
                if (_dictProxySymbols.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
