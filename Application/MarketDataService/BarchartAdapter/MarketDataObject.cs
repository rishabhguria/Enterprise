using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarchartAdapter
{
    public class MarketData
    {
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string name { get; set; }
        public string dayCode { get; set; }
        public string serverTimestamp { get; set; }
        public string mode { get; set; }
        public string lastPrice { get; set; }
        public string tradeTimestamp { get; set; }
        public string netChange { get; set; }
        public string percentChange { get; set; }
        public string unitCode { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string flag { get; set; }
        public string volume { get; set; }
    }
}
