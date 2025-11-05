using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStackAdapter
{
    public class Pagination
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public int? count { get; set; }
        public int? total { get; set; }
    }

    public class MarketDataParameter
    {
        public double? open { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? last { get; set; }
        public double? close { get; set; }
        public double? volume { get; set; }
        public string date { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
    }

    public class MarketDataRoot
    {
        public Pagination pagination { get; set; }
        public List<MarketDataParameter> data { get; set; }
    }


}
