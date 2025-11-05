using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModelingAdpater
{
    public class MarketData
    {
        public double? change { get; set; }
        public double? price { get; set; }
        public double? dayHigh { get; set; }
        public double? dayLow { get; set; }
        public long? volume { get; set; }
        public double? previousClose { get; set; }
        public double? open { get; set; }
        public double? ask { get; set; } //Extra added column
        public double? bid { get; set; } //Extra added column
        public long? sharesOutstanding { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double? changesPercentage { get; set; }
        public double? yearHigh { get; set; }
        public double? yearLow { get; set; }
        public double? marketCap { get; set; }
        public double? priceAvg50 { get; set; }
        public double? priceAvg200 { get; set; }
        public long? avgVolume { get; set; }
        public string exchange { get; set; }
        public double? eps { get; set; }
        public double? pe { get; set; }
        public DateTime? earningsAnnouncement { get; set; }
        public long? timestamp { get; set; }
        public string updateTime { get; set; }
    }

    public class Root
    {
        public List<MarketData> MarketData { get; set; }
    }
}
