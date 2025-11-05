// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-21-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="PeriodicitySelection.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Bloomberg.Library
{

    /// <summary>
    /// Period Selection: Determine the frequency of the output. To be used in conjunction with Period Adjustment.
    /// </summary>
    [Serializable]
    public enum PeriodicitySelection
    {
        /// <summary>
        /// Returns one data point per daily
        /// </summary>
        [ElementValue("DAILY")]
        Daily,

        /// <summary>
        /// Returns one data point per weekly
        /// </summary>
        [ElementValue("WEEKLY")]
        Weekly,

        /// <summary>
        /// Returns one data point per monthly
        /// </summary>
        [ElementValue("MONTHLY")]
        Monthly,

        /// <summary>
        /// Returns one data point per quarterly
        /// </summary>
        [ElementValue("QUARTERLY")]
        Quarterly,

        /// <summary>
        /// Returns one data point per semi annually
        /// </summary>
        [ElementValue("SEMI_ANNUALLY")]
        SemiAnnually,

        /// <summary>
        /// Returns one data point per yearly
        /// </summary>
        [ElementValue("YEARLY")]
        Yearly
    }



}