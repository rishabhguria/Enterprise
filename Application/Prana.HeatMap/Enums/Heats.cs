using System.Xml.Serialization;

namespace Prana.HeatMap.Enums
{
    public enum Heats
    {
        [XmlEnum(Name = "netExposureLocal")]
        Exposure,

        [XmlEnum(Name = "costBasisPnLLocal")]
        PnL
    }
}
