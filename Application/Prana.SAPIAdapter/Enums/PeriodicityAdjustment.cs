using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Period Adjustments: Determine the frequency and calendar type of the output. To be used in conjunction with Period Selection.
    /// </summary>
    [Serializable]
    public enum PeriodicityAdjustment
    {
        /// <summary>
        /// These revert to the actual date from today (if the end date is left blank) or from the End Date
        /// </summary>
        [ElementValue("ACTUAL")]
        Actual,
        /// <summary>
        /// For pricing fields, these revert to the last business day of the specified calendar period. Calendar Quarterly (CQ), Calendar Semi-Annually (CS) or Calendar Yearly (CY).
        /// </summary>
        [ElementValue("CALENDAR")]
        Calendar,
        /// <summary>
        /// These periods revert to the fiscal period end for the company - Fiscal Quarterly (FQ), Fiscal Semi-Annually (FS) and Fiscal Yearly (FY) only
        /// </summary>
        [ElementValue("FISCAL")]
        Fiscal
    }
}
