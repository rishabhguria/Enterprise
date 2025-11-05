// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 03-03-2015
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 03-03-2015
// ***********************************************************************
// <copyright file="ICostAdjutmentGenerator.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.CostAdjustment.Enums;
using System.Collections.Generic;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BusinessObjects.CostAdjustment
{

    /// <summary>
    /// Interface ICostAdjustmentGenerator
    /// </summary>
    public interface ICostAdjustmentGenerator
    {
        /// <summary>
        /// Adjusts the cost.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>CostAdjustmentResult.</returns>
        CostAdjustmentResult AdjustCost(List<TaxLot> taxlotList, List<CostAdjustmentParameter> parameter);

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        CostAdjustmentType Type { get; }
    }
}
