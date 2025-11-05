using System;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// 'float field representing a percentage (e.g. 0.05 represents 5% and 0.9525 represents 95.25%). Note the number of 
    /// decimal places may vary.'
    /// </summary>
    public class Percentage_t : Float_t
    {
        /// <summary>
        /// Applicable for xsi:type of Percentage_t. If true then percent values must be multiplied by 100 before being
        /// sent on the wire. For example, if multiplyBy100 were false then the percentage, 75%, would be sent as 0.75 
        /// on the wire. However, if multiplyBy100 were true then 75 would be sent on the wire.
        /// If not provided it should be interpreted as false.
        /// Use of this attribute is not recommended. The motivation for this attribute is to maximize compatibility 
        /// with algorithmic interfaces that are non-compliant with FIX in regard to their handling of percentages. In
        /// these cases an integer parameter should be used instead of a percentage.
        /// </summary>
        public bool? MultiplyBy100 { get; set; }
    }
}
