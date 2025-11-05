using Newtonsoft.Json;

namespace Prana.ServiceGateway.Models.RequestDto
{
    public class CreateOptionSymbolRequestDto
    {
        [JsonProperty("optionSymbolData")]
        public OptionSymbolDataDto OptionSymbolData { get; set; }
        public string TradingTicketId { get; set; }
    }

    public class OptionSymbolDataDto
    {
        public double? StrikePrice { get; set; }
        public string UnderlyingSymbol { get; set; }
        public int? AUECID { get; set; }
        public string ExpirationDate { get; set; }
        public string OptionType { get; set; }
        public double? StrikePriceMultiplier { get; set; }
        public string BloombergOptionRoot { get; set; }
        public string EsignalOptionRoot { get; set; }
        public int? Asset { get; set; }
    }

    public class GetCustomAllocationRequestDto
    {
        [JsonProperty("PreferenceId")]
        public string PreferenceId { get; set; }

        [JsonProperty("TradingTicketId")]
        public string tradingTicketId { get; set; }
    }
}
