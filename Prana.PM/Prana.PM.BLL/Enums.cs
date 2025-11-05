using System.Xml.Serialization;




namespace Prana.PM.BLL
{




    public enum ReconStatus
    {
        Closed,
        Open
    }



    public enum EntryType
    {
        [XmlEnumAttribute("0")]
        Optional,
        [XmlEnumAttribute("1")]
        Required
    }

    public enum AcceptDataFrom
    {
        [XmlEnumAttribute("0")]
        Source,
        [XmlEnumAttribute("1")]
        Application
    }

    public enum SelectColumnsType
    {
        [XmlEnumAttribute("0")]
        Integer,
        [XmlEnumAttribute("1")]
        Decimal,
        [XmlEnumAttribute("2")]
        Text,
        [XmlEnumAttribute("3")]
        Boolean,
        [XmlEnumAttribute("4")]
        DateTime
    }

    public enum ImpactOnCash
    {
        Positive,
        Negative,
        None
    }


    public enum ImportFormat
    {
        xls,
        csv,
        tsv
    }

    public enum ImportMethod
    {
        fix,
        ftp
    }

    public enum DeviationSignList
    {
        [XmlEnumAttribute("0")]
        NotApplicable,
        [XmlEnumAttribute("1")]
        Plus,
        [XmlEnumAttribute("2")]
        Minus,
        [XmlEnumAttribute("3")]
        PlusOrMinus

    }

    /// <summary>
    /// Same as Allocation Constants
    /// </summary>
    public enum TYPE_OF_ALLOCATION
    {
        FUND,
        STRATEGY,
        BOTH
    }


    public enum CreatePositionInstrumentType : byte
    {
        Listed = 0,
        OTC = 1
    }

    public enum OptionsGreeksParameters
    {
        AssetPrice = 0,
        Volatility = 1,
        InterestRates = 2,
        DayStep = 3
    }







    ///// <summary>
    ///// Used to differentiate among the entries in the ConsolidationInfo collection. 
    ///// </summary>
    //public enum ConsolidationInfoType
    //{
    //    Trade,
    //    TaxLot,
    //    Position
    //}

}
