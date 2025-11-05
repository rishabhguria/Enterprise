using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Pricing Options: Sets quote to Price or Yield for a debt instrument whose default value is quoted in yield (depending on pricing source).
    /// </summary>
    [Serializable]
    public enum PricingOption
    {
        /// <summary>
        /// Set quote to price
        /// </summary>
        [ElementValue("PRICING_OPTION_PRICE")]
        PricingOptionPrice,
        /// <summary>
        /// Set quote to yield
        /// </summary>
        [ElementValue("PRICING_OPTION_YIELD")]
        PricingOptionYield
    }
}
