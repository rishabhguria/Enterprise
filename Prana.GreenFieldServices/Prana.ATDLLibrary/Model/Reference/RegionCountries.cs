using System.Collections.Generic;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Reference
{
    /// <summary>
    /// Represents the Regions supported by FIXatdl.
    /// </summary>
    public static class Regions
    {
        /// <summary>
        /// Gets the FIXatdl region for the supplied country.
        /// </summary>
        /// <param name="country">ISO country code to determine the region for.</param>
        /// <returns>Applicable region, or Region.None if none is applicable.</returns>
        public static Region GetRegionForCountry(IsoCountryCode country)
        {
            if (TheAmericasCountries.Contains(country))
                return Region.TheAmericas;

            if (EuropeMiddleEastAfricaCountries.Contains(country))
                return Region.EuropeMiddleEastAfrica;

            if (AsiaPacificJapanCountries.Contains(country))
                return Region.AsiaPacificJapan;

            return Region.None;
        }

        /// <summary>
        /// Provides the set of ISO country codes that are in The Americas.
        /// </summary>
        public static readonly HashSet<IsoCountryCode> TheAmericasCountries = new HashSet<IsoCountryCode>
        {
            IsoCountryCode.AI,
            IsoCountryCode.AG,
            IsoCountryCode.AR,
            IsoCountryCode.AW,
            IsoCountryCode.BS,
            IsoCountryCode.BB,
            IsoCountryCode.BZ,
            IsoCountryCode.BM,
            IsoCountryCode.BO,
            IsoCountryCode.BR,
            IsoCountryCode.CA,
            IsoCountryCode.KY,
            IsoCountryCode.CL,
            IsoCountryCode.CO,
            IsoCountryCode.CR,
            IsoCountryCode.CU,
            IsoCountryCode.DM,
            IsoCountryCode.DO,
            IsoCountryCode.EC,
            IsoCountryCode.SV,
            IsoCountryCode.FK,
            IsoCountryCode.GD,
            IsoCountryCode.GP,
            IsoCountryCode.GT,
            IsoCountryCode.GY,
            IsoCountryCode.HT,
            IsoCountryCode.HN,
            IsoCountryCode.JM,
            IsoCountryCode.MQ,
            IsoCountryCode.MX,
            IsoCountryCode.MS,
            IsoCountryCode.AN,
            IsoCountryCode.NI,
            IsoCountryCode.PA,
            IsoCountryCode.PY,
            IsoCountryCode.PE,
            IsoCountryCode.PR,
            IsoCountryCode.BL,
            IsoCountryCode.KN,
            IsoCountryCode.LC,
            IsoCountryCode.MF,
            IsoCountryCode.PM,
            IsoCountryCode.VC,
            IsoCountryCode.TT,
            IsoCountryCode.TC,
            IsoCountryCode.US,
            IsoCountryCode.UY,
            IsoCountryCode.VG,
            IsoCountryCode.VI,
            IsoCountryCode.VE,
        };

        /// <summary>
        /// Provides the set of ISO country codes that are in Europe, the Middle East and Africa.
        /// </summary>
        public static readonly HashSet<IsoCountryCode> EuropeMiddleEastAfricaCountries = new HashSet<IsoCountryCode>()
        {
            IsoCountryCode.AD,
            IsoCountryCode.AE,
            IsoCountryCode.AL,
            IsoCountryCode.AM,
            IsoCountryCode.AO,
            IsoCountryCode.AT,
            IsoCountryCode.AX,
            IsoCountryCode.AZ,
            IsoCountryCode.BA,
            IsoCountryCode.BE,
            IsoCountryCode.BF,
            IsoCountryCode.BG,
            IsoCountryCode.BH,
            IsoCountryCode.BI,
            IsoCountryCode.BJ,
            IsoCountryCode.BW,
            IsoCountryCode.BY,
            IsoCountryCode.CD,
            IsoCountryCode.CF,
            IsoCountryCode.CG,
            IsoCountryCode.CH,
            IsoCountryCode.CI,
            IsoCountryCode.CM,
            IsoCountryCode.CV,
            IsoCountryCode.CY,
            IsoCountryCode.CZ,
            IsoCountryCode.DE,
            IsoCountryCode.DJ,
            IsoCountryCode.DK,
            IsoCountryCode.DZ,
            IsoCountryCode.EE,
            IsoCountryCode.EG,
            IsoCountryCode.EH,
            IsoCountryCode.ER,
            IsoCountryCode.ES,
            IsoCountryCode.ET,
            IsoCountryCode.FI,
            IsoCountryCode.FO,
            IsoCountryCode.FR,
            IsoCountryCode.GA,
            IsoCountryCode.GB,
            IsoCountryCode.GE,
            IsoCountryCode.GF,
            IsoCountryCode.GG,
            IsoCountryCode.GH,
            IsoCountryCode.GI,
            IsoCountryCode.GL,
            IsoCountryCode.GM,
            IsoCountryCode.GN,
            IsoCountryCode.GQ,
            IsoCountryCode.GR,
            IsoCountryCode.GS,
            IsoCountryCode.GW,
            IsoCountryCode.HR,
            IsoCountryCode.HU,
            IsoCountryCode.IE,
            IsoCountryCode.IL,
            IsoCountryCode.IM,
            IsoCountryCode.IQ,
            IsoCountryCode.IR,
            IsoCountryCode.IS,
            IsoCountryCode.IT,
            IsoCountryCode.JE,
            IsoCountryCode.JO,
            IsoCountryCode.KE,
            IsoCountryCode.KM,
            IsoCountryCode.KW,
            IsoCountryCode.LB,
            IsoCountryCode.LI,
            IsoCountryCode.LK,
            IsoCountryCode.LR,
            IsoCountryCode.LS,
            IsoCountryCode.LT,
            IsoCountryCode.LU,
            IsoCountryCode.LV,
            IsoCountryCode.LY,
            IsoCountryCode.MA,
            IsoCountryCode.MC,
            IsoCountryCode.MD,
            IsoCountryCode.ME,
            IsoCountryCode.MG,
            IsoCountryCode.MK,
            IsoCountryCode.ML,
            IsoCountryCode.MR,
            IsoCountryCode.MT,
            IsoCountryCode.MU,
            IsoCountryCode.MW,
            IsoCountryCode.MZ,
            IsoCountryCode.NA,
            IsoCountryCode.NE,
            IsoCountryCode.NG,
            IsoCountryCode.NL,
            IsoCountryCode.NO,
            IsoCountryCode.OM,
            IsoCountryCode.PL,
            IsoCountryCode.PN,
            IsoCountryCode.PS,
            IsoCountryCode.PT,
            IsoCountryCode.QA,
            IsoCountryCode.RE,
            IsoCountryCode.RO,
            IsoCountryCode.RS,
            IsoCountryCode.RU,
            IsoCountryCode.RW,
            IsoCountryCode.SA,
            IsoCountryCode.SC,
            IsoCountryCode.SD,
            IsoCountryCode.SE,
            IsoCountryCode.SH,
            IsoCountryCode.SI,
            IsoCountryCode.SJ,
            IsoCountryCode.SK,
            IsoCountryCode.SL,
            IsoCountryCode.SM,
            IsoCountryCode.SN,
            IsoCountryCode.SO,
            IsoCountryCode.SR,
            IsoCountryCode.ST,
            IsoCountryCode.SY,
            IsoCountryCode.SZ,
            IsoCountryCode.TD,
            IsoCountryCode.TG,
            IsoCountryCode.TN,
            IsoCountryCode.TR,
            IsoCountryCode.TZ,
            IsoCountryCode.UA,
            IsoCountryCode.UG,
            IsoCountryCode.VA,
            IsoCountryCode.YE,
            IsoCountryCode.YT,
            IsoCountryCode.ZA,
            IsoCountryCode.ZM,
            IsoCountryCode.ZW
        };

        /// <summary>
        /// Provides the set of ISO country codes that are in the Asia Pacific and Japan region.
        /// </summary>
        public static readonly HashSet<IsoCountryCode> AsiaPacificJapanCountries = new HashSet<IsoCountryCode>()
        {
            IsoCountryCode.AF,
            IsoCountryCode.AS,
            IsoCountryCode.AU,
            IsoCountryCode.BD,
            IsoCountryCode.BN,
            IsoCountryCode.BT,
            IsoCountryCode.CC,
            IsoCountryCode.CK,
            IsoCountryCode.CN,
            IsoCountryCode.CX,
            IsoCountryCode.FJ,
            IsoCountryCode.FM,
            IsoCountryCode.GU,
            IsoCountryCode.HK,
            IsoCountryCode.ID,
            IsoCountryCode.IN,
            IsoCountryCode.IO,
            IsoCountryCode.JP,
            IsoCountryCode.KG,
            IsoCountryCode.KH,
            IsoCountryCode.KI,
            IsoCountryCode.KP,
            IsoCountryCode.KR,
            IsoCountryCode.KZ,
            IsoCountryCode.LA,
            IsoCountryCode.MH,
            IsoCountryCode.MM,
            IsoCountryCode.MN,
            IsoCountryCode.MO,
            IsoCountryCode.MP,
            IsoCountryCode.MV,
            IsoCountryCode.MY,
            IsoCountryCode.NC,
            IsoCountryCode.NF,
            IsoCountryCode.NP,
            IsoCountryCode.NR,
            IsoCountryCode.NU,
            IsoCountryCode.NZ,
            IsoCountryCode.PF,
            IsoCountryCode.PG,
            IsoCountryCode.PH,
            IsoCountryCode.PK,
            IsoCountryCode.PW,
            IsoCountryCode.SB,
            IsoCountryCode.SG,
            IsoCountryCode.TH,
            IsoCountryCode.TJ,
            IsoCountryCode.TK,
            IsoCountryCode.TL,
            IsoCountryCode.TM,
            IsoCountryCode.TO,
            IsoCountryCode.TV,
            IsoCountryCode.TW,
            IsoCountryCode.UM,
            IsoCountryCode.UZ,
            IsoCountryCode.VN,
            IsoCountryCode.VU,
            IsoCountryCode.WF,
            IsoCountryCode.WS
        };
    }
    /// <summary>
    /// ISO country code A3 version to determine the region
    /// </summary>
    public static class RegionsA3
    {
        public static Region GetRegionForCountry(IsoCountryCodeA3 country)
        {
            if (TheAmericasCountriesA3.Contains(country))
                return Region.TheAmericas;

            if (EuropeMiddleEastAfricaCountriesA3.Contains(country))
                return Region.EuropeMiddleEastAfrica;

            if (AsiaPacificJapanCountriesA3.Contains(country))
                return Region.AsiaPacificJapan;

            return Region.None;
        }
        public static readonly HashSet<IsoCountryCodeA3> TheAmericasCountriesA3 = new HashSet<IsoCountryCodeA3>
        {
            IsoCountryCodeA3.AIA, // Anguilla
            IsoCountryCodeA3.ATG, // Antigua and Barbuda
            IsoCountryCodeA3.ARG, // Argentina
            IsoCountryCodeA3.ABW, // Aruba
            IsoCountryCodeA3.BHS, // Bahamas
            IsoCountryCodeA3.BRB, // Barbados
            IsoCountryCodeA3.BLZ, // Belize
            IsoCountryCodeA3.BMU, // Bermuda
            IsoCountryCodeA3.BOL, // Bolivia
            IsoCountryCodeA3.BRA, // Brazil
            IsoCountryCodeA3.CAN, // Canada
            IsoCountryCodeA3.CYM, // Cayman Islands
            IsoCountryCodeA3.CHL, // Chile
            IsoCountryCodeA3.COL, // Colombia
            IsoCountryCodeA3.CRI, // Costa Rica
            IsoCountryCodeA3.CUB, // Cuba
            IsoCountryCodeA3.DMA, // Dominica
            IsoCountryCodeA3.DOM, // Dominican Republic
            IsoCountryCodeA3.ECU, // Ecuador
            IsoCountryCodeA3.SLV, // El Salvador
            IsoCountryCodeA3.FLK, // Falkland Islands
            IsoCountryCodeA3.GRD, // Grenada
            IsoCountryCodeA3.GLP, // Guadeloupe
            IsoCountryCodeA3.GTM, // Guatemala
            IsoCountryCodeA3.GUY, // Guyana
            IsoCountryCodeA3.HTI, // Haiti
            IsoCountryCodeA3.HND, // Honduras
            IsoCountryCodeA3.JAM, // Jamaica
            IsoCountryCodeA3.MTQ, // Martinique
            IsoCountryCodeA3.MEX, // Mexico
            IsoCountryCodeA3.MSR, // Montserrat
            IsoCountryCodeA3.ANT, // Netherlands Antilles
            IsoCountryCodeA3.NIC, // Nicaragua
            IsoCountryCodeA3.PAN, // Panama
            IsoCountryCodeA3.PRY, // Paraguay
            IsoCountryCodeA3.PER, // Peru
            IsoCountryCodeA3.PRI, // Puerto Rico
            IsoCountryCodeA3.BLM, // Saint Barthélemy
            IsoCountryCodeA3.KNA, // Saint Kitts and Nevis
            IsoCountryCodeA3.LCA, // Saint Lucia
            IsoCountryCodeA3.MAF, // Saint Martin (French part)
            IsoCountryCodeA3.SPM, // Saint Pierre and Miquelon
            IsoCountryCodeA3.VCT, // Saint Vincent and the Grenadines
            IsoCountryCodeA3.TTO, // Trinidad and Tobago
            IsoCountryCodeA3.TCA, // Turks and Caicos Islands
            IsoCountryCodeA3.USA, // United States
            IsoCountryCodeA3.URY, // Uruguay
            IsoCountryCodeA3.VGB, // Virgin Islands, British
            IsoCountryCodeA3.VIR, // Virgin Islands, U.S.
            IsoCountryCodeA3.VEN // Venezuela
        };
        public static readonly HashSet<IsoCountryCodeA3> EuropeMiddleEastAfricaCountriesA3 = new HashSet<IsoCountryCodeA3>()
        {
            IsoCountryCodeA3.AND,
            IsoCountryCodeA3.ARE,
            IsoCountryCodeA3.ALB,
            IsoCountryCodeA3.ARM,
            IsoCountryCodeA3.AGO,
            IsoCountryCodeA3.AUT,
            IsoCountryCodeA3.ALA,
            IsoCountryCodeA3.AZE,
            IsoCountryCodeA3.BIH,
            IsoCountryCodeA3.BEL,
            IsoCountryCodeA3.BFA,
            IsoCountryCodeA3.BGR,
            IsoCountryCodeA3.BHR,
            IsoCountryCodeA3.BDI,
            IsoCountryCodeA3.BEN,
            IsoCountryCodeA3.BWA,
            IsoCountryCodeA3.BLR,
            IsoCountryCodeA3.COD,
            IsoCountryCodeA3.CAF,
            IsoCountryCodeA3.COG,
            IsoCountryCodeA3.CHE,
            IsoCountryCodeA3.CIV,
            IsoCountryCodeA3.CMR,
            IsoCountryCodeA3.CPV,
            IsoCountryCodeA3.CYP,
            IsoCountryCodeA3.CZE,
            IsoCountryCodeA3.DEU,
            IsoCountryCodeA3.DJI,
            IsoCountryCodeA3.DNK,
            IsoCountryCodeA3.DZA,
            IsoCountryCodeA3.EST,
            IsoCountryCodeA3.EGY,
            IsoCountryCodeA3.ESH,
            IsoCountryCodeA3.ERI,
            IsoCountryCodeA3.ESP,
            IsoCountryCodeA3.ETH,
            IsoCountryCodeA3.FIN,
            IsoCountryCodeA3.FRO,
            IsoCountryCodeA3.FRA,
            IsoCountryCodeA3.GAB,
            IsoCountryCodeA3.GBR,
            IsoCountryCodeA3.GEO,
            IsoCountryCodeA3.GUF,
            IsoCountryCodeA3.GGY,
            IsoCountryCodeA3.GHA,
            IsoCountryCodeA3.GIB,
            IsoCountryCodeA3.GRL,
            IsoCountryCodeA3.GMB,
            IsoCountryCodeA3.GIN,
            IsoCountryCodeA3.GNQ,
            IsoCountryCodeA3.GRC,
            IsoCountryCodeA3.SGS,
            IsoCountryCodeA3.GNB,
            IsoCountryCodeA3.HRV,
            IsoCountryCodeA3.HUN,
            IsoCountryCodeA3.IRL,
            IsoCountryCodeA3.ISR,
            IsoCountryCodeA3.IMN,
            IsoCountryCodeA3.IRQ,
            IsoCountryCodeA3.IRN,
            IsoCountryCodeA3.ISL,
            IsoCountryCodeA3.ITA,
            IsoCountryCodeA3.JEY,
            IsoCountryCodeA3.JOR,
            IsoCountryCodeA3.KEN,
            IsoCountryCodeA3.COM,
            IsoCountryCodeA3.KWT,
            IsoCountryCodeA3.LBN,
            IsoCountryCodeA3.LIE,
            IsoCountryCodeA3.LKA,
            IsoCountryCodeA3.LBR,
            IsoCountryCodeA3.LSO,
            IsoCountryCodeA3.LTU,
            IsoCountryCodeA3.LUX,
            IsoCountryCodeA3.LVA,
            IsoCountryCodeA3.LBY,
            IsoCountryCodeA3.MAR,
            IsoCountryCodeA3.MCO,
            IsoCountryCodeA3.MDA,
            IsoCountryCodeA3.MNE,
            IsoCountryCodeA3.MDG,
            IsoCountryCodeA3.MKD,
            IsoCountryCodeA3.MLI,
            IsoCountryCodeA3.MRT,
            IsoCountryCodeA3.MLT,
            IsoCountryCodeA3.MUS,
            IsoCountryCodeA3.MWI,
            IsoCountryCodeA3.MOZ,
            IsoCountryCodeA3.NAM,
            IsoCountryCodeA3.NER,
            IsoCountryCodeA3.NGA,
            IsoCountryCodeA3.NLD,
            IsoCountryCodeA3.NOR,
            IsoCountryCodeA3.OMN,
            IsoCountryCodeA3.POL,
            IsoCountryCodeA3.PCN,
            IsoCountryCodeA3.PSE,
            IsoCountryCodeA3.PRT,
            IsoCountryCodeA3.QAT,
            IsoCountryCodeA3.REU,
            IsoCountryCodeA3.ROU,
            IsoCountryCodeA3.SRB,
            IsoCountryCodeA3.RUS,
            IsoCountryCodeA3.RWA,
            IsoCountryCodeA3.SAU,
            IsoCountryCodeA3.SYC,
            IsoCountryCodeA3.SDN,
            IsoCountryCodeA3.SWE,
            IsoCountryCodeA3.SHN,
            IsoCountryCodeA3.SVN,
            IsoCountryCodeA3.SJM,
            IsoCountryCodeA3.SVK,
            IsoCountryCodeA3.SLE,
            IsoCountryCodeA3.SMR,
            IsoCountryCodeA3.SEN,
            IsoCountryCodeA3.SOM,
            IsoCountryCodeA3.SUR,
            IsoCountryCodeA3.STP,
            IsoCountryCodeA3.SYR,
            IsoCountryCodeA3.SWZ,
            IsoCountryCodeA3.TCD,
            IsoCountryCodeA3.TGO,
            IsoCountryCodeA3.TUN,
            IsoCountryCodeA3.TUR,
            IsoCountryCodeA3.TZA,
            IsoCountryCodeA3.UKR,
            IsoCountryCodeA3.UGA,
            IsoCountryCodeA3.VAT,
            IsoCountryCodeA3.YEM,
            IsoCountryCodeA3.MYT,
            IsoCountryCodeA3.ZAF,
            IsoCountryCodeA3.ZMB,
            IsoCountryCodeA3.ZWE
        };
        public static readonly HashSet<IsoCountryCodeA3> AsiaPacificJapanCountriesA3 = new HashSet<IsoCountryCodeA3>()
        {
             IsoCountryCodeA3.AFG,
             IsoCountryCodeA3.ASM,
             IsoCountryCodeA3.AUS,
             IsoCountryCodeA3.BGD,
             IsoCountryCodeA3.BRN,
             IsoCountryCodeA3.BTN,
             IsoCountryCodeA3.CCK,
             IsoCountryCodeA3.COK,
             IsoCountryCodeA3.CHN,
             IsoCountryCodeA3.CXR,
             IsoCountryCodeA3.FJI,
             IsoCountryCodeA3.FSM,
             IsoCountryCodeA3.GUM,
             IsoCountryCodeA3.HKG,
             IsoCountryCodeA3.IDN,
             IsoCountryCodeA3.IND,
             IsoCountryCodeA3.IOT,
             IsoCountryCodeA3.JPN,
             IsoCountryCodeA3.KGZ,
             IsoCountryCodeA3.KHM,
             IsoCountryCodeA3.KIR,
             IsoCountryCodeA3.PRK,
             IsoCountryCodeA3.KOR,
             IsoCountryCodeA3.KAZ,
             IsoCountryCodeA3.LAO,
             IsoCountryCodeA3.MHL,
             IsoCountryCodeA3.MMR,
             IsoCountryCodeA3.MNG,
             IsoCountryCodeA3.MAC,
             IsoCountryCodeA3.MNP,
             IsoCountryCodeA3.MDV,
             IsoCountryCodeA3.MYS,
             IsoCountryCodeA3.NCL,
             IsoCountryCodeA3.NFK,
             IsoCountryCodeA3.NPL,
             IsoCountryCodeA3.NRU,
             IsoCountryCodeA3.NIU,
             IsoCountryCodeA3.NZL,
             IsoCountryCodeA3.PYF,
             IsoCountryCodeA3.PNG,
             IsoCountryCodeA3.PHL,
             IsoCountryCodeA3.PAK,
             IsoCountryCodeA3.PLW,
             IsoCountryCodeA3.SLB,
             IsoCountryCodeA3.SGP,
             IsoCountryCodeA3.THA,
             IsoCountryCodeA3.TJK,
             IsoCountryCodeA3.TKL,
             IsoCountryCodeA3.TLS,
             IsoCountryCodeA3.TKM,
             IsoCountryCodeA3.TON,
             IsoCountryCodeA3.TUV,
             IsoCountryCodeA3.TWN,
             IsoCountryCodeA3.UMI,
             IsoCountryCodeA3.UZB,
             IsoCountryCodeA3.VNM,
             IsoCountryCodeA3.VUT,
             IsoCountryCodeA3.WLF,
             IsoCountryCodeA3.WSM
        };
    }
}
