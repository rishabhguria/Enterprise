using Prana.BusinessObjects.AppConstants;
// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="MatchingRuleType.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Enums namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// This enum defines the matching of allocation
    /// </summary>
    public enum MatchingRuleType
    {
        /// <summary>
        /// Matching of historical data will be not considered
        /// </summary>
        [EnumDescriptionAttribute("Current Preference")]
        None = 1,
        /// <summary>
        /// Data since last change in allocation target percentage will be considered
        /// </summary>
        [EnumDescriptionAttribute("Last Update")]
        SinceLastChange = 2,

        /// <summary>
        /// All data since inception will be considered
        /// </summary>
        [EnumDescriptionAttribute("Account Inception")]
        SinceInception = 3,

        /// <summary>
        /// All data since inception to date defined in prorata
        /// </summary>
        [EnumDescriptionAttribute("Prorata")]
        Prorata = 4,

        /// <summary>
        /// Levelling
        /// </summary>
        [EnumDescriptionAttribute("Leveling")]
        Leveling = 5,

        /// <summary>
        /// Pro-rata (NAV)
        /// </summary>
        [EnumDescriptionAttribute("Pro-rata (NAV)")]
        ProrataByNAV = 6
    }
}
