using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Enum Search Category
    /// </summary>
    [Serializable]
    public enum SearchCategory
    {
        /// <summary>
        /// The new fields
        /// </summary>
        [ElementValue("New Fields")]
        NewFields,
        /// <summary>
        /// The analysis
        /// </summary>
        [ElementValue("Analysis")]
        Analysis,
        /// <summary>
        /// The corporate
        /// </summary>
        [ElementValue("Corporate")]
        Corporate,
        /// <summary>
        /// The actions
        /// </summary>
        [ElementValue("Actions")]
        Actions,
        /// <summary>
        /// The custom fields
        /// </summary>
        [ElementValue("Custom Fields")]
        CustomFields,
        /// <summary>
        /// The descriptive
        /// </summary>
        [ElementValue("Descriptive")]
        Descriptive,
        /// <summary>
        /// The earnings
        /// </summary>
        [ElementValue("Earnings")]
        Earnings,
        /// <summary>
        /// The estimates
        /// </summary>
        [ElementValue("Estimates")]
        Estimates,
        /// <summary>
        /// The fundamentals
        /// </summary>
        [ElementValue("Fundamentals")]
        Fundamentals,
        /// <summary>
        /// The market activity
        /// </summary>
        [ElementValue("Market Activity")]
        MarketActivity,
        /// <summary>
        /// The metadata
        /// </summary>
        [ElementValue("Metadata")]
        Metadata,
        /// <summary>
        /// The ratings
        /// </summary>
        [ElementValue("Ratings")]
        Ratings,
        /// <summary>
        /// The trading
        /// </summary>
        [ElementValue("Trading")]
        Trading,
        /// <summary>
        /// The systems
        /// </summary>
        [ElementValue("Systems")]
        Systems
    }
}
