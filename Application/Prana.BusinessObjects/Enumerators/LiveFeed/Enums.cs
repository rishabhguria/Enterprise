namespace Prana.BusinessObjects.LiveFeed
{
    public enum MonthEnum
    {
        JAN = 1,
        FEB,
        MAR,
        APR,
        MAY,
        JUN,
        JUL,
        AUG,
        SEP,
        OCT,
        NOV,
        DEC
    }

    /// <summary>
    /// Enum for defining generically the fields which can be used systemwide and while requesting from different feed providers.
    /// </summary>
    public enum PricingDataType
    {
        Undefined = 0,
        Ask = 1,
        Bid = 2,
        Last = 3,
        Mid = 4,
        IMID = 5,
        Open = 6,
        Close = 7,
        High = 8,
        Low = 9,
        Avg_AskBid = 10   // Calculation of mid price using average(Ask, Bid)
    }
}
