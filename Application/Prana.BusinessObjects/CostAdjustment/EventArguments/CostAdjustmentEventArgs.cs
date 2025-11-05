// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Shagoon.Gurtata
// Created          : 11-12-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 11-14-2014
// ***********************************************************************
// <copyright file="CostAdjustmentEventArgs.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Prana.BusinessObjects.CostAdjustment.Enums;
using System;

/// <summary>
/// The EventArguments namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.EventArguments
{
    /// <summary>
    /// Class CostAdjustmentEventArgs.
    /// </summary>
    public class CostAdjustmentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public CostAdjustmentType Type { get; set; }

        /// <summary>
        /// Gets or sets the adjust cost.
        /// </summary>
        /// <value>The adjust cost.</value>
        public decimal AdjustCost { get; set; }

        /// <summary>
        /// Gets or sets the adjust qty.
        /// </summary>
        /// <value>The adjust qty.</value>
        public decimal AdjustQty { get; set; }

        /// <summary>
        /// Gets or sets the total qty.
        /// </summary>
        /// <value>The total qty.</value>
        public decimal TotalQty { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost { get; set; }

    }
}
