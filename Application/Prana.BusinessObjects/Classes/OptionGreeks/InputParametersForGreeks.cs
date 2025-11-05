using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using System;
using System.Text;
namespace Prana.BusinessObjects
{
    [Serializable]
    public class InputParametersForGreeks
    {

        private double _volatility;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private double _interestRate;
        public double InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        private char _putOrCalls;
        public char PutOrCalls
        {
            get { return _putOrCalls; }
            set { _putOrCalls = value; }
        }

        private double _simulatedUnderlyingStockPrice;
        public double SimulatedUnderlyingStockPrice
        {
            get { return _simulatedUnderlyingStockPrice; }
            set { _simulatedUnderlyingStockPrice = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private double _simulatedPrice;
        public double SimulatedPrice
        {
            get { return _simulatedPrice; }
            set { _simulatedPrice = value; }
        }

        private double _lastPrice;
        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _underlyingSymbol = string.Empty;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private bool _stepAnalysis;
        public bool StepAnalysis
        {
            get { return _stepAnalysis; }
            set { _stepAnalysis = value; }
        }

        private bool _auto;
        public bool Auto
        {
            get { return _auto; }
            set { _auto = value; }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private double _dividendYield;
        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }

        string _listedExchange = string.Empty;
        public string ListedExchange
        {
            get { return _listedExchange; }
            set { _listedExchange = value; }
        }

        string _bloombergSymbol = string.Empty;
        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        AssetCategory _categoryCode = AssetCategory.None;
        public AssetCategory CategoryCode
        {
            get { return _categoryCode; }
            set { _categoryCode = value; }
        }

        private int _auecID = 0;
        public int AUECID
        {
            get
            {
                return _auecID;
            }
            set
            {
                if (_auecID == value)
                    return;
                _auecID = value;
            }
        }

        private DateTime _expirationDate = DateTimeConstants.MinValue;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }

        }

        public InputParametersForGreeks()
        {
            _volatility = 0.0;
            _putOrCalls = Char.MinValue;
            _strikePrice = 0.0;
            _simulatedUnderlyingStockPrice = 0.0;
            _symbol = string.Empty;
            _auto = true;
            _dividendYield = 0.0;
            //_userID = int.MinValue;

        }
        public void SetValues(PricingModelData pricingModelData)
        {
            _putOrCalls = pricingModelData.PutOrCall;
            _simulatedPrice = pricingModelData.OptionPrice;
            _strikePrice = pricingModelData.StrikePrice;
            _volatility = pricingModelData.Volatility;
            _simulatedUnderlyingStockPrice = pricingModelData.StockPrice;
            _lastPrice = pricingModelData.OptionPrice;
            _interestRate = pricingModelData.InterestRate;
            _underlyingSymbol = pricingModelData.UnderlyingSymbol;
            _symbol = pricingModelData.OptSymbol;
            _dividendYield = pricingModelData.DividendYield;
            _listedExchange = pricingModelData.ListedExchange;
            _categoryCode = pricingModelData.CategoryCode;
            _auecID = pricingModelData.AUECID;
            _expirationDate = pricingModelData.ExpirationDate;
        }
        public InputParametersForGreeks(string data)
        {
            string[] dataColl = data.Split(Seperators.SEPERATOR_2);
            _volatility = double.Parse(dataColl[0]);
            _putOrCalls = Char.Parse(dataColl[1]);
            _simulatedUnderlyingStockPrice = double.Parse(dataColl[2]);
            _symbol = dataColl[3];
            _underlyingSymbol = dataColl[4];
            _auto = Convert.ToBoolean(dataColl[5]);
            _interestRate = double.Parse(dataColl[6]);
            _dividendYield = double.Parse(dataColl[7]);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_volatility.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_putOrCalls.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_simulatedUnderlyingStockPrice.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_symbol);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_underlyingSymbol.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_auto.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_interestRate.ToString());
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_dividendYield.ToString());
            return sb.ToString();
        }
        public void SetValues(PranaPositionWithGreeks position)
        {
            if (position.PutOrCall == 0)
                _putOrCalls = 'P';
            else if (position.PutOrCall == 1)
                _putOrCalls = 'C';

            _simulatedPrice = position.SimulatedPrice;
            _strikePrice = position.StrikePrice;
            _volatility = position.Volatility / 100;
            _simulatedUnderlyingStockPrice = position.SimulatedUnderlyingStockPrice;
            _lastPrice = position.SimulatedPrice;
            _interestRate = position.InterestRate / 100;
            _underlyingSymbol = position.UnderlyingSymbol;
            _symbol = position.Symbol;
            _dividendYield = position.DividendYield / 100;
            _listedExchange = position.ExchangeName;
            _categoryCode = position.AssetCategoryValue;
            _auecID = position.AUECID;
            _bloombergSymbol = position.BloombergSymbol;
            _expirationDate = position.ExpirationDate;
        }
    }
    [Serializable]
    public class StepParameter
    {
        StepAnalParameterCode _parameterCode = StepAnalParameterCode.UnderlyingPrice;
        int _steps = int.MinValue;
        double _low = double.MinValue;
        double _high = double.MinValue;

        public StepParameter(StepAnalParameterCode code, int steps, double low, double high)
        {
            _parameterCode = code;
            _steps = steps;
            _low = low;
            _high = high;
            //_value = value;
        }
        public StepParameter(string data)
        {
            string[] dataArray = data.Split(Seperators.SEPERATOR_2);
            // zeroth for Msg Type
            _parameterCode = (StepAnalParameterCode)int.Parse(dataArray[0]);
            //_value = double.Parse(dataArray[1]);
            _steps = int.Parse(dataArray[2]);
            _low = double.Parse(dataArray[3]);
            _high = double.Parse(dataArray[4]);
        }

        public StepAnalParameterCode ParameterCode
        {
            get { return _parameterCode; }
        }

        public int Steps
        {
            get { return _steps; }
        }

        public double High
        {
            get
            {
                //if (_parameterCode == StepAnalParameterCode.StockPrice)
                //{
                //    return _high;
                //}
                //else
                //{
                return _high;
                //}
            }
        }

        public double Low
        {
            get
            {
                //if (_parameterCode == StepAnalParameterCode.StockPrice)
                //{
                //    return _low;
                //}
                //else
                //{
                return _low;
                //}
            }
        }

        public double StepDifference
        {
            get { return (High - Low) / _steps; }
        }

        public override string ToString()
        {

            return ((int)_parameterCode).ToString() + Seperators.SEPERATOR_2 + _steps.ToString() + Seperators.SEPERATOR_2 + _low.ToString() + Seperators.SEPERATOR_2 + _high.ToString();
        }
    }

    public enum StepAnalParameterCode
    {
        [EnumDescription("Underlying Price")]
        UnderlyingPrice = 1,
        [EnumDescription("Volatility")]
        Volatility = 2,
        [EnumDescription("Interest Rate")]
        InterestRate = 3,
        [EnumDescription("Days To Expiration")]
        DaysToExpiration = 4
    }
}
