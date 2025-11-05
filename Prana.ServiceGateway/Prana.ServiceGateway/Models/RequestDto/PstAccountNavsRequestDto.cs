namespace Prana.ServiceGateway.Models.RequestDto
{
    public class PstAccountNavsRequestDto
    {
        /// <summary>
        /// Unique identifier for the account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Unique identifier for the security.
        /// </summary>
        public string UnderlyingSymbol { get; set; }

        /// <summary>
        /// Symbol associated with the security.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Symbol AUEC ID (Asset Underlying Entity Code ID).
        /// </summary>
        public string SymbolAUECID { get; set; }

        public double Price { get; set; }

        public bool IsLimitPrice { get; set; }

        public string CorrelationId { get; set; }

        public int? UserId { get; set; }

    }

    public class RequestDto<T>
    {
       
        public string CorrelationId { get; set; }

        public T Data { get; set; }
    }

    public class PstAccountNavsResponseDto
    {

        public int AccountId { get; set; }

        /// <summary>
        /// Unique identifier for the security.
        /// </summary>
        public string UnderlyingSymbol { get; set; }

        /// <summary>
        /// Symbol associated with the security.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Symbol AUEC ID (Asset Underlying Entity Code ID).
        /// </summary>
        public string SymbolAUECID { get; set; }

        public double AccountNav { get; set; }
        public double StartingValue { get; set; }
        public double StartingPerc { get; set; }

    }
}
