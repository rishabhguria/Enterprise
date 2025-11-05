using Newtonsoft.Json;

namespace Prana.ServiceGateway.Models.RequestDto
{
    public class ValidateSymbolRequestDto
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        public string RequestID { get; set; }
    }

    public class SymbolSearchRequestDto
    {
        public string Symbol { get; set; }
        public double? HashCode { get; set; }

    }
    public class SmSymbolDto
    {
        public int? AUECID { get; set; }
        public int? AssetID { get; set; }
        public int? ExchangeID { get; set; }
        public int? UnderlyingID { get; set; }
        public int? CurrencyID { get; set; }
        public string TickerSymbol { get; set; }
        public double Multiplier { get; set; }
        public string Description { get; set; }
        public string UnderlyingSymbol { get; set; }
        public string BloombergSymbol { get; set; }
        public string FactsetSymbol { get; set; }
        public string ActivSymbol { get; set; }
        public string PutAndCall { get; set; }
        public double StrikePrice { get; set; }
        public double RoundLot { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityAction { get; set; }
        public string Symbol_PK { get; set; }
        public string SedolSymbol { get; set; }
    }

    public class SymbolAccountPositionRequestDto
    {
        public string Symbol { get; set; }
        public int CurrencyID { get; set; }
        public string RequestID { get; set; }
    }

    public class ValidateSymbolUnifiedRequestDto
    {
        // For ValidateSymbol and ValidateOptionSymbol
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("OptionSymbol")]
        public string OptionSymbol { get; set; }

        [JsonProperty("UnderLyingSymbol")]
        public string UnderLyingSymbol { get; set; }

        [JsonProperty("RequestID")]
        public string RequestID { get; set; }

        [JsonProperty("Symbology")]
        public int Symbology { get; set; }

        // For ValidateMultipleSymbols
        [JsonProperty("symbols")]
        public List<string> Symbols { get; set; }
    }
}
