using System;
using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Types.Support;
using Prana.ATDLLibrary.Resources;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'used to allow the expression of FX standard tenors in addition to the base valid enumerations defined for the field that
    /// uses this pattern data type. This pattern data type is defined as follows:
    /// Dx = tenor expression for "days", e.g. "D5", where "x" is any integer > 0
    /// Mx = tenor expression for "months", e.g. "M3", where "x" is any integer > 0
    /// Wx = tenor expression for "weeks", e.g. "W13", where "x" is any integer > 0
    /// Yx = tenor expression for "years", e.g. "Y1", where "x" is any integer > 0'
    /// </summary>
    public class Tenor_t : AtdlValueType<Tenor>
    {
        /// <summary>
        /// Gets/sets the maximum value of this parameter.
        /// </summary>
        public Tenor? MaxValue { get; set; }

        /// <summary>
        /// Gets/sets the minimum value of this parameter.
        /// </summary>
        public Tenor? MinValue { get; set; }

    }
}
