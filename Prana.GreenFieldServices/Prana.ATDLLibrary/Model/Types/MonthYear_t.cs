using System;
using System.Globalization;
using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Types.Support;
using Prana.ATDLLibrary.Resources;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'string field representing month of a year. An optional day of the month can be appended or an optional week code.
    /// Valid formats:  YYYYMM
    ///                 YYYYMMDD
    ///                 YYYYMMWW
    /// Valid values:   YYYY = 0000-9999; MM = 01-12; DD = 01-31; WW = w1, w2, w3, w4, w5.'
    /// </summary>
    public class MonthYear_t : AtdlValueType<MonthYear>
    {
        /// <summary>
        /// Gets/sets the maximum value for this parameter.<br/>
        /// Maximum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The maximum value.</value>
        public MonthYear? MaxValue { get; set; }

        /// <summary>
        /// Gets/sets the minimum value for this parameter.<br/>
        /// Minimum value of the parameter accepted by the algorithm provider.
        /// </summary>
        /// <value>The minimum value.</value>
        public MonthYear? MinValue { get; set; }

    }
}
