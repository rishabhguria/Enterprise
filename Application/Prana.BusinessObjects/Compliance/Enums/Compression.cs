namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum Compression
    {
        None,
        AccountSymbol,
        AccountUnderlyingSymbol,
        Account,
        MasterFundSymbol,
        MasterFundUnderlyingSymbol,
        MasterFund,
        Symbol,
        SubSector,
        Sector,
        Asset,
        UnderlyingSymbol,
        Global,

        // Adding raw data windows as well. 
        SymbolData,
        SecurityData
    }
}
