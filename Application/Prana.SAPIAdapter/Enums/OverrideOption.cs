using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Override Options: Indicates whether to use the average or the closing price in quote calculation.
    /// </summary>
    [Serializable]
    public enum OverrideOption
    {
        /// <summary>
        /// Use the closing price in quote calculation
        /// </summary>
        [ElementValue("OVERRIDE_OPTION_CLOSE")]
        OverrideOptionClose,
        /// <summary>
        /// Use the average price in quote calculation
        /// </summary>
        [ElementValue("OVERRIDE_OPTION_GPA")]
        OverrideOptionGPA
    }
}
