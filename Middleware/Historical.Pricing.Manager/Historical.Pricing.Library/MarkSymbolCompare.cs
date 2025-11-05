using System;
using System.Collections.Generic;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Mark Symbol Compare
    /// </summary>
    /// <remarks></remarks>
    public class MarkSymbolCompare : IEqualityComparer<SymbolRequest>
    {
        #pragma warning disable 1734
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <remarks></remarks>
        public bool Equals(SymbolRequest x, SymbolRequest y)
        {
            return x.MarkSymbol.ToUpper() == y.MarkSymbol.ToUpper();
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///   </exception>
        /// <remarks></remarks>
        public int GetHashCode(SymbolRequest obj)
        {
            return obj.MarkSymbol.GetHashCode();
        }
    }
}
