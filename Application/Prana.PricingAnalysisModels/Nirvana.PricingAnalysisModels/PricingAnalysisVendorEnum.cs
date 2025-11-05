namespace Prana.PricingAnalysisModels
{
    public enum PricingAnalysisVendorEnum
    {
        Numerix,
        WinDale
    }

    public enum ModelType    // See Windale Help for Binomial method
    {
        Stock,                       //0 = Stock, no dividend 
        StockWithContinuousDividend, //1 = Stock with continuous dividend yield 
        Futures,                     //2 = Futures 
        Currencies                   //3 = Currencies 
    }

    public enum FlagEurAm   // See Windale Help for Binomial method
    {
        American,   //0 = American 
        European    //1 = European
    }
}
