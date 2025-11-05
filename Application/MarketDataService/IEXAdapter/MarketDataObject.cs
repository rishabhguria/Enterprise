using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEXAdapter
{
    public class MarketData
    {
        public double? change { get; set; }
        public double? latestPrice { get; set; }
        public object high { get; set; }
        public object low { get; set; }
        public double? avgTotalVolume { get; set; }
        public double? previousClose { get; set; }
        public object iexOpen { get; set; }
        public double? iexAskPrice { get; set; }
        public double? iexBidPrice { get; set; }
        public object sharesOutstanding { get; set; } //Extra Added Column
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string calculationPrice { get; set; }
        public object open { get; set; }
        public object openTime { get; set; }
        public string openSource { get; set; }
        public object close { get; set; }
        public object closeTime { get; set; }
        public string closeSource { get; set; }
        public long? highTime { get; set; }
        public string highSource { get; set; }
        public long? lowTime { get; set; }
        public string lowSource { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public long? latestUpdate { get; set; }
        public object latestVolume { get; set; }
        public double? iexRealtimePrice { get; set; }
        public double? iexRealtimeSize { get; set; }
        public long? iexLastUpdated { get; set; }
        public object delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public object oddLotDelayedPrice { get; set; }
        public object oddLotDelayedPriceTime { get; set; }
        public object extendedPrice { get; set; }
        public object extendedChange { get; set; }
        public object extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double? previousVolume { get; set; }
        public double? changePercent { get; set; }
        public object volume { get; set; }
        public object iexMarketPercent { get; set; }
        public double? iexVolume { get; set; }
        public double? iexBidSize { get; set; }
        public double? iexAskSize { get; set; }
        public object iexOpenTime { get; set; }
        public double? iexClose { get; set; }
        public long? iexCloseTime { get; set; }
        public long? marketCap { get; set; }
        public double? peRatio { get; set; }
        public double? week52High { get; set; }
        public double? week52Low { get; set; }
        public double? ytdChange { get; set; }
        public long? lastTradeTime { get; set; }
        public bool isUSMarketOpen { get; set; }
        public double? rate { get; set; }
        public long? timestamp { get; set; }
        public string updateTime { get; set; }
        public bool? isDerived { get; set; }
    }
}
