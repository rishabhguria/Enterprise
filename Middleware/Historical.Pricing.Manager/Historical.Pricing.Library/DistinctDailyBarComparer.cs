using System;
using System.Collections.Generic;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Distinct DailyBar Comparer
    /// </summary>
    /// <remarks></remarks>
    class DistinctDailyBarComparer : IEqualityComparer<DailyBar>
    {

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <remarks></remarks>
        public bool Equals(DailyBar x, DailyBar y)
        {
            return x.Symbol == y.Symbol && x.Date == y.Date && x.ProviderId == y.ProviderId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
        /// <remarks></remarks>
        public int GetHashCode(DailyBar obj)
        {
            return obj.Symbol.GetHashCode() ^ obj.Date.GetHashCode() ^ obj.ProviderId.GetHashCode();
        }
    }
}
