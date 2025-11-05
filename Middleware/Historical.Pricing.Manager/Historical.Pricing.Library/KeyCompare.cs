using System;
using System.Collections.Generic;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// IEquality Comparer - Symbol Compare by Mark Symbol
    /// </summary>
    /// <remarks></remarks>
    public class KeyCompare : IEqualityComparer<SymbolRequest>
    {
        #pragma warning disable 1734
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"></paramref> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"></paramref> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <remarks></remarks>
        public bool Equals(SymbolRequest x, SymbolRequest y)
        {
            return x.MarkSymbol.ToUpper() == y.MarkSymbol.ToUpper();
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"></paramref> is a reference type and <paramref name="obj"></paramref> is null.
        /// </exception>
        /// <remarks></remarks>
        public int GetHashCode(SymbolRequest obj)
        {
            return obj.MarkSymbol.GetHashCode();
        }
    }
}
