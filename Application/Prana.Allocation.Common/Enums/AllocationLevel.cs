// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : Disha Sharma
// Created          : 05-08-2017
// ***********************************************************************
// <copyright file="AllocationLevel.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Prana.Allocation.Common.Enums
{
    /// <summary>
    /// The level for allocation of trades
    /// </summary>
    public enum AllocationLevel
    {
        /// <summary>
        /// The account level allocation
        /// </summary>
        Account = 1,

        /// <summary>
        /// The master fund level allocation
        /// </summary>
        MasterFund = 2
    }
}
