using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Non Trading Day Fill Method: If data is to be displayed for non trading days what is the data to be returned.
    /// </summary>
    [Serializable]
    public enum NonTradingDayFillMethod
    {
        /// <summary>
        /// None
        /// </summary>
        [ElementValue(null)]
        None,
        /// <summary>
        /// Search back and retrieve the previous value available for this security field pair. The search back period is up to one month.
        /// </summary>
        [ElementValue("PREVIOUS_VALUE")]
        PreviousValue,
        /// <summary>
        /// Returns blank for the "value" value within the data element for this field.
        /// </summary>
        [ElementValue("NIL_VALUE")]
        NilValue
    }
}
