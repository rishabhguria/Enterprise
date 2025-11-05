using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;


namespace Nirvana.Middleware.Extensions
{

    /// <summary>
    /// Linq Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class LinqExtensions
    {
        /// <summary>
        /// Is the enumerable.
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="records">The records.</param>
        /// <param name="table">The table.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <remarks></remarks>
        public static void IEnumerable<T_Entity>(IEnumerable<T_Entity> records, string table, string ConnectionString) where T_Entity : class
        {

        }
        /// <summary>
        /// Linq In Clause
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="value">The value.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<string> In(this IEnumerable<string> entities, string value, Predicate<string> match) 
        {
            foreach (string entity in entities)
            {
                if (entity.Contains(value))
                    yield return entity;
            }
        }

        /// <summary>
        /// Linq Clause
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="value">The value.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<Int32> In(this IQueryable<Int32> entities, Int32 value, Predicate<Int32> match)
        {
            foreach (Int32 entity in entities)
            {
                if (entity == value)
                    yield return entity;
            }
        }

   

    }
}
