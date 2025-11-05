using System;

namespace Nirvana.Middleware
{
    /// <summary>
    /// Date Range Class for (fromDate, toDate)
    /// </summary>
    /// <remarks></remarks>
    public class DateRange
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        /// <remarks></remarks>
        public DateTime From { get; set; }
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        /// <remarks></remarks>
        public DateTime To { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks></remarks>
        public double Value { get; set; }
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        /// <remarks></remarks>
        public string Key { get; set; }
    }
}
