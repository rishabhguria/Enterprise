using Newtonsoft.Json;
using System;

namespace Prana.APIAdapter.Models
{
    [JsonObject]
    [Serializable]
    public class InstrumentPrice
    {

        public int Id { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Symbol { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Cusip { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sedol { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Isin { get; set; }



        public double Bid { get; set; }

        public double Ask { get; set; }

        public double Last { get; set; }

        public double Best { get; set; }

        public double Close { get; set; }

        public double Vwap { get; set; }

        public double TClose { get; set; }



        private string _currency;
        [JsonProperty(PropertyName = "CCY", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency
        {
            get { return string.IsNullOrEmpty(_currency) ? "USD" : _currency; }
            set { _currency = value; }
        }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }



        [JsonProperty(PropertyName = "CCR", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double ExchangeRate { get; set; }



        private int _exchangeRateDisplayMultiplier;



        [JsonProperty(PropertyName = "CCRDispMult", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ExchangeRateDisplayMultiplier
        {
            get { return _exchangeRateDisplayMultiplier == 1 ? 0 : _exchangeRateDisplayMultiplier; }

            set { _exchangeRateDisplayMultiplier = value; }
        }




        private string _securityType;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SecurityType
        {

            get { return _securityType == "EQT" ? null : _securityType; }

            set { _securityType = value; }

        }



        public string Name { get; set; }

    }
}
