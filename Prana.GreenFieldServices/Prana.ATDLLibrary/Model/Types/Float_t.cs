using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents a sequence of digits with optional decimal point and sign character (ASCII characters "-", "0" - "9" and "."); the
    /// absence of the decimal point within the string will be interpreted as the float representation of an integer value.
    /// All float fields must accommodate up to fifteen significant digits. The number of decimal places used should be a
    /// factor of business/market needs and mutual agreement between counterparties. Note that float values may contain
    /// leading zeros (e.g. "00023.23" = "23.23") and may contain or omit trailing zeros after the decimal point 
    /// (e.g. "23.0" = "23.0000" = "23" = "23.").
    /// Note that fields which are derived from float may contain negative values unless explicitly specified otherwise.'
    /// </summary>
    public class Float_t : AtdlValueType<decimal>
    {
        /// <summary>
        /// Gets/sets the maximum value for this parameter.<br/>
        /// Maximum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The maximum value.</value>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// Gets/sets the minimum value for this parameter.<br/>
        /// Minimum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The minimum value.</value>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// Gets/sets the precision of this value, taken as the number of digits to the right of the decimal point in 
        /// which to round when populating the FIX message. Lack of this attribute indicates that the value entered by 
        /// the user should be taken as-is without rounding.
        /// </summary>
        public int? Precision { get; set; }
    }
}
