using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// Enumerator Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Joins the strings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string JoinStrings<T>(this IEnumerable<T> values, string separator)
        {
            var stringValues = values.Select(item =>
                (item == null ? string.Empty : item.ToString()));
            return string.Join(separator, stringValues.ToArray());
        }
    }
}
