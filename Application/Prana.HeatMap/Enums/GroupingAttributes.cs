using System.Xml.Serialization;

namespace Prana.HeatMap.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum GroupingAttributes
    {
        [XmlEnum(Name = "masterFundName")]
        MasterFund,

        [XmlEnum(Name = "accountLongName")]
        Account,

        [XmlEnum(Name = "symbol")]
        Symbol,

        [XmlEnum(Name = "underlyingSymbol")]
        UnderlyingSymbol,

        //BloombergSymbol,
        //SecurityDescription,

        [XmlEnum(Name = "asset")]
        AssetClass,

        [XmlEnum(Name = "sector")]
        Sector,

        [XmlEnum(Name = "subSector")]
        Subsector,

        [XmlEnum(Name = "udaSecurityType")]
        SecurityType,

        [XmlEnum(Name = "udaCountry")]
        Country,

        [XmlEnum(Name = "udaAsset")]
        UserAsset

        //Strategy,
        //TradeAttribute1,
        //TradeAttribute2,
        //TradeAttribute3,
        //TradeAttribute4,
        //TradeAttribute5,
        //TradeAttribute6,
    }
}
