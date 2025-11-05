// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 08-13-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="NotionalComparer.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.FormulaStore;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Comparers namespace.
/// </summary>
namespace Prana.Allocation.Core.Comparers
{
    /// <summary>
    /// Comparer for allocation group based on notional value
    /// </summary>
    public class NotionalComparer : IComparer<AllocationGroup>
    {
        #region IComparer<AllocationGroup> Members

        /// <summary>
        /// Defines sorting is ascending or descending
        /// </summary>
        private bool _isAscending = true;

        /// <summary>
        /// Constructor that sets value for ascending.
        /// </summary>
        /// <param name="isAscending"></param>
        public NotionalComparer(bool isAscending)
        {
            try
            {
                _isAscending = isAscending;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public int Compare(AllocationGroup x, AllocationGroup y)
        {
            try
            {
                decimal notionalX = NotionalCalculator.GetNotional(x, (decimal)x.CumQty);
                decimal notionalY = NotionalCalculator.GetNotional(y, (decimal)y.CumQty);

                if (_isAscending)
                {
                    if (notionalX > notionalY)
                        return 1;
                    else if (notionalX == notionalY)
                    {
                        if (Convert.ToInt64(x.GroupID) > Convert.ToInt64(y.GroupID))
                            return 1;
                        else if (Convert.ToInt64(x.GroupID) == Convert.ToInt64(y.GroupID))
                            return 0;
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
                else
                {
                    if (notionalX > notionalY)
                        return -1;
                    else if (notionalX == notionalY)
                    {
                        if (Convert.ToInt64(x.GroupID) > Convert.ToInt64(y.GroupID))
                            return -1;
                        else if (Convert.ToInt64(x.GroupID) == Convert.ToInt64(y.GroupID))
                            return 0;
                        else
                            return 1;
                    }
                    else
                        return 1;
                }

                //return Convert.ToInt32(notionalX - notionalY);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return -1;
            }
        }

        #endregion
    }
}
