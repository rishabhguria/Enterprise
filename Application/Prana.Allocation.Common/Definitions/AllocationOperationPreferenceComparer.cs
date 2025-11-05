// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : disha
// Created          : 02-19-2015
//
// Last Modified By : disha
// Last Modified On : 02-19-2015
// ***********************************************************************
// <copyright file="AllocationOperationPreferenceComparer.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Definitions namespace.
/// </summary>

namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    /// Comparer for AllocationOperationPreference based on position priority
    /// </summary>
    public class AllocationOperationPreferenceComparer : IComparer<AllocationOperationPreference>
    {
        #region IComparer<AllocationOperationPreference> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public int Compare(AllocationOperationPreference x, AllocationOperationPreference y)
        {
            try
            {
                if (x.PositionPrefId > y.PositionPrefId) return 1;
                else if (x.PositionPrefId < y.PositionPrefId) return -1;
                else return 0;
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
                return 0;
            }
        }

        #endregion
    }
}
