using Infragistics.Win;
using Prana.HeatMap.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.HeatMap.BLL
{
    class GroupAttributesCache
    {
        /// <summary>
        /// Cache for storing values of grouping attributes
        /// </summary>
        private Dictionary<GroupingAttributes, ValueList> _valueListDict = new Dictionary<GroupingAttributes, ValueList>();

        /// <summary>
        /// Locker object for group cache
        /// </summary>
        private static object _locker = new object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static GroupAttributesCache singiltonInstance;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private GroupAttributesCache()
        {

        }

        /// <summary>
        /// Provides the singiltan instance
        /// </summary>
        /// <returns></returns>
        internal static GroupAttributesCache GetInstance()
        {
            lock (_locker)
            {
                if (singiltonInstance == null)
                    singiltonInstance = new GroupAttributesCache();
                return singiltonInstance;
            }
        }

        /// <summary>
        /// Updates the cache(GroupingAttributes-ValueList pair)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateCache(GroupingAttributes key, ValueList value)
        {
            try
            {
                lock (_locker)
                {
                    if (_valueListDict.ContainsKey(key))
                        _valueListDict[key] = value;
                    else
                        _valueListDict.Add(key, value);
                }
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
            }
        }

        /// <summary>
        /// returns the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueList GetValuelist(GroupingAttributes key)
        {
            try
            {
                lock (_locker)
                {
                    if (_valueListDict.ContainsKey(key))
                        return _valueListDict[key];
                    else
                        return null;
                }
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
                return null;
            }
        }
    }
}
