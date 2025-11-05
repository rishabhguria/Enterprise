using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Enum AssetType
    /// </summary>
    [Serializable]
    public enum ProductType
    {
        /// <summary>
        /// All - Used for Product Type Search
        /// </summary>
        [ElementValue("All")]
        All,
        /// <summary>
        /// Government
        /// </summary>
        [ElementValue("Govt")]
        Govt,
        /// <summary>
        /// Corporate
        /// </summary>
        [ElementValue("Corp")]
        Corp,
        /// <summary>
        /// Mortgage
        /// </summary>
        [ElementValue("Mtge")]
        Mtge,
        /// <summary>
        /// The M MKT
        /// </summary>
        [ElementValue("M-Mkt")]
        MMkt,
        /// <summary>
        /// Municipal
        /// </summary>
        [ElementValue("Muni")]
        Muni,
        /// <summary>
        /// Preferred
        /// </summary>
        [ElementValue("Pfd")]
        Pfd,
        /// <summary>
        /// Equity
        /// </summary>
        [ElementValue("Equity")]
        Equity,
        /// <summary>
        /// Commodity
        /// </summary>
        [ElementValue("Cmdty")]
        Cmdty,
        /// <summary>
        /// Index
        /// </summary>
        [ElementValue("Index")]
        Index,
        /// <summary>
        /// Currency
        /// </summary>
        [ElementValue("Curncy")]
        Curncy
    }
}
