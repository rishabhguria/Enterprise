using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents an alpha-numeric free format strings, can include any character or punctuation except the delimiter. All
    /// String fields are case sensitive (i.e. morstatt != Morstatt).
    /// </summary>
    public class String_t : AtdlReferenceType<string>
    {
        /// <summary>Gets or sets the maximum length of this parameter.<br/>
        /// Applicable when xsi:type is String_t.  The maximum allowable length of the parameter.
        /// </summary>
        /// <value>The maximum length.</value>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of this parameter.<br/>
        /// Applicable when xsi:type is String_t.  The minimum allowable length of the parameter.
        /// </summary>
        /// <value>The minimum length.</value>
        public int? MinLength { get; set; }
    }
}
