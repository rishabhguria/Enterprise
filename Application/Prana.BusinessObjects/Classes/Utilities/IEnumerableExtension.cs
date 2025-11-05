// ***********************************************************************
// Assembly         : Prana.Utilities
// Author           : Disha Sharma
// Created          : 09-14-2015
// ***********************************************************************
// <copyright file="IEnumerableExtension.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.BusinessObjects.Classes.Utilities
{
    /// <summary>
    /// Extension class for IEnumerable 
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// returns distinct records based on key selector provided
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            try
            {
                var knownKeys = new HashSet<TKey>();
                return source.Where(element => knownKeys.Add(keySelector(element)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// To the hash set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        /// <summary>
        /// Gets the matching i enumerable values.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> GetMatchingIEnumerableValues<TSource>(this IEnumerable<TSource> source, object parameter)
        {
            IEnumerable<TSource> matchedKeys = null;
            try
            {
                matchedKeys = source.Where(key => key.Equals(parameter));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return matchedKeys;
        }

        /// <summary>
        /// To the serializable dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <returns></returns>
        public static SerializableDictionary<TKey, TElement> ToSerializableDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            SerializableDictionary<TKey, TElement> serializableDict = new SerializableDictionary<TKey, TElement>();
            try
            {
                Dictionary<TKey, TElement> dict = source.ToDictionary(keySelector, elementSelector);
                foreach (TKey key in dict.Keys)
                {
                    serializableDict.Add(key, dict[key]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return serializableDict;
        }
    }
}
