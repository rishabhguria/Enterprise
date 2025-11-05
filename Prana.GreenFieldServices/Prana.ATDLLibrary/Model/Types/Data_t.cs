using System;
using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Types.Support;
using Prana.ATDLLibrary.Resources;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents a string field containing raw data with no format or content restrictions. Data fields are always immediately preceded
    /// by a length field. The length field should specify the number of bytes of the value of the data field (up to but not 
    /// including the terminating SOH).
    /// Caution: the value of one of these fields may contain the delimiter (SOH) character. Note that the value specified for
    /// this field should be followed by the delimiter (SOH) character as all fields are terminated with an "SOH".
    /// </summary>
    /// <remarks>As there is no way within FIXatdl of associating one field with another to provide the length, Data_t fields
    /// do not appear to be particularly useful; they are implemented within Atdl4net for completeness.</remarks>
    public class Data_t : AtdlReferenceType<char[]>
    {
        /// <summary>
        /// Gets/sets the maximum length of this field.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets/sets the minimum length of this field.
        /// </summary>
        public int? MinLength { get; set; }
    }
}
