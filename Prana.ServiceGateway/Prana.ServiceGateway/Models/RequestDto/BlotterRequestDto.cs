using Newtonsoft.Json;

namespace Prana.ServiceGateway.Models.RequestDto
{
    public class BlotterRequestDto : BaseRequestDto
    {
        public string BlotterId { get; set; }
        public bool? IsComTransTradeRulesRequired { get; set; }
        public string CommaSeparatedParentClOrderId { get; set; }
        public string Data { get; set; }
        public string ParentClOrderId { get; set; }
        public string FillsDataTable { get; set; }
        public string AllocationDetails { get; set; }
        public BlotterTabRenameInfo BlotterTabRenameInfo { get; set; }
        public BlotterTabRemoveInfo BlotterTabRemoveInfo { get; set; }
    }

    public class PstDataRequestDto : BaseRequestDto
    {
        public int AllocationPrefID { get; set; }
        public string Symbol { get; set; }
        public int OrderSideId { get; set; }
    }

    public class OrdersActionRequestDto : BaseRequestDto
    {
        /// <summary>
        /// Comma-separated list of ParentClOrderIds to act upon.
        /// </summary>
        public string CommaSeparatedParentClOrderIds { get; set; }
    }

    public class RemoveManualOrderDto : BaseRequestDto
    {
        /// <summary>
        /// ParentClOrderId is used to reference and remove the specific manual order from the system.
        /// </summary>
        public string ParentClOrderId { get; set; }
    }

    public class SaveManualFillsDto : BaseRequestDto
    {
        public string FillsDataTable { get; set; }
    }

    public class SaveAllocationDetailsDto : BaseRequestDto
    {
        public string AllocationDetails { get; set; }
    }

    public class TransferUserRequestDto
    {
        [JsonProperty("orderIds")]
        public List<string> OrderIds { get; set; }

        [JsonProperty("targetUserId")]
        public string TargetUserId { get; set; }

        [JsonProperty("includeSubOrders")]
        public bool IncludeSubOrders { get; set; }

        [JsonProperty("isOrderTab")]
        public bool IsOrderTab { get; set; }

        [JsonProperty("isAllowUserToTansferTrade")]
        public bool IsAllowUserToTansferTrade { get; set; }

        [JsonProperty("uniqueIdentifier")]
        public string UniqueIdentifier { get; set; }
    }

    public class TradeOrderDto : BaseRequestDto
    {
        public string Data { get; set; }
    }
}