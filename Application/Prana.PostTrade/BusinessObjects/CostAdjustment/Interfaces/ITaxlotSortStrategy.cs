// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 03-03-2015
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 03-03-2015
// ***********************************************************************
// <copyright file="ITaxlotSortStrategy.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.CostAdjustment.Definitions;
using System.Collections.Generic;

/// <summary>
/// The Interfaces namespace.
/// </summary>
namespace Prana.PostTrade.BusinessObjects.CostAdjustment.Interfaces
{
    /// <summary>
    /// Interface ITaxlotSortStrategy
    /// </summary>
    public interface ITaxlotSortStrategy
    {
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>List&lt;CostAdjustmentTaxlot&gt;.</returns>
        List<CostAdjustmentTaxlot> Sort(List<CostAdjustmentTaxlot> list);
    }
}
