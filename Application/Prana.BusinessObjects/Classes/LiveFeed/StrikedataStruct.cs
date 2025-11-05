namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// We are using this structure to keep the strike data
    /// </summary>
    public struct StrikedataStruct
    {
        public string Symbol;
        public string UnderlyingSymbol;
        public double StrikePrice;
        public int ExpirationMonth;
        public int ExpirationDate;
        public int DaysToExpiration;
        public char PutorCall;
        public string SortingKey;
        //public string Currency;
    }
}
