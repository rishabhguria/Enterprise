// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 03-03-2015
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 03-03-2015
// ***********************************************************************
// <copyright file="SortByDate.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.LogManager;
using Prana.PostTrade.BusinessObjects.CostAdjustment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The SortStrategy namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment.SortStrategy
{
    /// <summary>
    /// Class SortByDate.
    /// </summary>
    class SortByDate : ITaxlotSortStrategy
    {
        /// <summary>
        /// The _is ascending
        /// </summary>
        bool _isAscending = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortByDate"/> class.
        /// </summary>
        /// <param name="isAscending">if set to <c>true</c> [is ascending].</param>
        public SortByDate(bool isAscending)
        {
            _isAscending = isAscending;
        }

        #region ITaxlotSortStrategy Members

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>List&lt;CostAdjustmentTaxlot&gt;.</returns>
        public List<CostAdjustmentTaxlot> Sort(List<CostAdjustmentTaxlot> list)
        {
            try
            {
                if (_isAscending)
                    return list.OrderBy(o => o.TransactionDate).ToList();
                else
                    return list.OrderByDescending(o => o.TransactionDate).ToList();
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        #endregion
    }
}
