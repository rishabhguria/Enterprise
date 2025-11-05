using System.Xml.Serialization;

namespace Prana.Import
{
    public class Constants
    {
        public const string DATEFORMAT = "MM/dd/yyyy";

        /// <summary>
        /// Enum for Import symbologies
        /// </summary>
        public enum ImportSymbologies
        {
            [XmlEnumAttribute("Symbol")]
            Symbol = 0,
            [XmlEnumAttribute("RIC")]
            RIC = 1,
            [XmlEnumAttribute("ISIN")]
            ISIN = 2,
            [XmlEnumAttribute("SEDOL")]
            SEDOL = 3,
            [XmlEnumAttribute("CUSIP")]
            CUSIP = 4,
            [XmlEnumAttribute("Bloomberg")]
            Bloomberg = 5,
            [XmlEnumAttribute("OSIOptionSymbol")]
            OSIOptionSymbol = 6,
            [XmlEnumAttribute("IDCOOptionSymbol")]
            IDCOOptionSymbol = 7,
            [XmlEnumAttribute("OpraOptionSymbol")]
            OpraOptionSymbol = 8
        }

        /// <summary>
        /// Enum to show Import completion status
        /// </summary>
        public enum ImportCompletionStatus
        {
            /// <summary>
            /// All of the trades imported successfully.
            /// </summary>
            [XmlEnumAttribute("Success")]
            Success = 0,
            /// <summary>
            /// Some error occured while importing trades
            /// </summary>
            [XmlEnumAttribute("ImportError")]
            ImportError = 1,
            /// <summary>
            /// Some/All of the trades were not imported.
            /// </summary>
            [XmlEnumAttribute("FileWriteError")]
            FileWriteError = 2
        }

        /// <summary>
        /// Enum to show Symbol Validation status
        /// </summary>
        public enum SymbolValidationStatus
        {
            [XmlEnumAttribute("Success")]
            Success = 0,
            [XmlEnumAttribute("Partial Success")]
            PartialSuccess = 1,
            [XmlEnumAttribute("Failure")]
            Failure = 2,
            [XmlEnumAttribute("Not Applicable")]
            NotApplicable = 3
        }
    }
}
