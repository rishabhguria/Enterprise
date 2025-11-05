using Prana.BusinessObjects.AppConstants;
// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AllocationBaseType.cs" company="Nirvana">
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
    /// This enum defines the base type on which allocation will be performed
    /// </summary>
    public enum AllocationBaseType
    {
        /// <summary>
        /// According to this, allocation will be done on the basis of cumulative quantity
        /// </summary>
        [EnumDescriptionAttribute("Trade Quantity")]
        CumQuantity = 1,

        /// <summary>
        /// According to this, allocation will be done on the basis of Notional
        /// </summary>
        [EnumDescriptionAttribute("Fair Pricing")]
        Notional = 2
    }
}
