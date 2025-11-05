namespace Prana.ServiceGateway.Models
{
    public class MultipleSymbolRequestDto
    {
        public List<string> RequestedSymbols { get; set; }
        public string RequestedInstance { get; set; }
    }
}
