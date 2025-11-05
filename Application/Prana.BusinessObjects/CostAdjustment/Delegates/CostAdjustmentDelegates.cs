// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Shagoon.Gurtata
// Created          : 11-12-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 11-14-2014
// ***********************************************************************
// <copyright file="CostAdjustmentDelegates.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.CostAdjustment.EventArguments;
using System;

/// <summary>
/// The Delegates namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.Delegates
{
    /// <summary>
    /// Delegate CostAdjustmentHandler
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="CostAdjustmentEventArgs"/> instance containing the event data.</param>
    public delegate void CostAdjustmentHandler(Object sender, CostAdjustmentEventArgs e);
}
