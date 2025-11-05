using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlotterDataService.DTO
{
    internal class EditTradeAttributesDto
    {
        public class TradeDetailsDto
        {
            public string Symbol { get; set; }
            public string OrderSide { get; set; }
            public string Broker { get; set; }
            public double TotalQuantity { get; set; }
        }

        public class EditTradeAttributesResponseDto
        {
            public TradeDetailsDto TradeDetails { get; set; }

            public AllocationGroup GroupOrdersDetails { get; set; }
        }
    }

    public class EditTradeAttributesPayload
    {
        public string ParentClOrderId { get; set; }
        public Dictionary<string, string> EditedTradeAttributes { get; set; }
    }

    public class AllocationGroupDetails
    {
        public AllocationGroup Group { get; set; }
        public string GroupId { get; set; }
        public DateTime TradeDate { get; set; }
    }

}
