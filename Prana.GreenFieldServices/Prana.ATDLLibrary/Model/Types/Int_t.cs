using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'Sequence of digits without commas or decimals and optional sign character (ASCII characters "-" and "0" - "9" ). 
    /// The sign character utilizes one byte (i.e. positive int is "99999" while negative int is "-99999"). Note that int
    /// values may contain leading zeros (e.g. "00023" = "23").'
    /// </summary>
    public class Int_t : AtdlValueType<int>
    {
        /// <summary>
        /// Gets/sets the minimum value for this parameter.<br/>
        /// Minimum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The minimum value.</value>
        public int? MinValue { get; set; }

        /// <summary>
        /// Gets/sets the maximum value for this parameter.<br/>
        /// Maximum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The maximum value.</value>
        public int? MaxValue { get; set; }
    }
}
