using System.Collections.Generic;

namespace Prana.DataManager
{
    public class MultiBrokersSubsCollection
    {
        public string clOrderID = string.Empty;
        public string OrderID = string.Empty;
        public string origClOrderID = string.Empty;
        public string parentClOrderID = string.Empty;
        public string orderStatus = "A";
        // OrderID wise CLOrderids that were newly generated.
        public Dictionary<string, MultiBrokerChildOrders> dictOrderIDWiseNewCLOrderIDs = new Dictionary<string, MultiBrokerChildOrders>();
        public double StartOfDayCumQty = 0;
        public double StartOfDayAveragePrice = 0;
        public System.DateTime CurrentAuecDate = System.DateTime.UtcNow;
        public int AUECID = 0;
    }

    public class MultiBrokerChildOrders
    {
        public string clOrderID = string.Empty;
        public string origClOrderID = string.Empty;
        public string parentClOrderID = string.Empty;
        public double CumQty = 0;
        public double AveragePrice = 0;
        public System.DateTime AuecLocalDate = System.DateTime.Now;
    }
}


