using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Core.CacheStore
{
    internal class TradeAttributeCache
    {
        #region SingletonInstance
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly TradeAttributeCache _singeltonInstance = new TradeAttributeCache();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static TradeAttributeCache Instance
        {
            get
            {
                return _singeltonInstance;
            }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private TradeAttributeCache()
        {
            try
            {

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
        #endregion

        #region Cache Objects

        /// <summary>
        /// Locker object for cache
        /// </summary>
        private readonly object _cacheLockerObject = new object();

        /// <summary>
        /// List of TradeAttribute
        /// </summary>
        //TODO: Need to find better way than array of List of strings
        private List<string>[] _tradeAttributeCache;

        // Stores default attribute values
        private List<string>[] _defaultAttributeValues;

        // Stores manually entered trade attribute values by users
        private List<string>[] _userInputAttributeValues;

        /// <summary>
        /// The keep records
        /// </summary>
        private bool[] _keepRecords;

        #endregion

        #region Internal Methods

        /// <summary>
        /// returns Trade attribute list
        /// </summary>
        /// <returns></returns>
        internal List<string>[] GetTradeAttributes()
        {
            return _tradeAttributeCache;
        }

        /// <summary>
        /// Updates trade attribute lists
        /// </summary>
        /// <param name="groups"></param>
        internal void UpdateTradeAttribLists(List<AllocationGroup> groups)
        {
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    UpdateAttribList(group);
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
        /// Initializes cache with the start up values.
        /// </summary>
        /// <param name="userInputValues">Initial user input trade attribute values</param>
        internal void Initialize(List<string>[] userInputValues)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    _userInputAttributeValues = userInputValues;
                    _tradeAttributeCache = new List<string>[45]; // 45 trade attributes
                    RefreshAttributeCache();
                }
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
        /// Refreshes the trade attribute cache by combining default and manual values.
        /// </summary>
        internal void RefreshAttributeCache()
        {
            _keepRecords = CachedDataManager.GetInstance.GetAttributeKeepRecords();
            _defaultAttributeValues = CachedDataManager.GetInstance.GetAttributeDefaultValues();

            for (int i = 0; i < _keepRecords.Length; i++)
            {
                // Use combined default and manual values if manual records should be kept; otherwise, use only default values
                _tradeAttributeCache[i] = _keepRecords[i] ? _defaultAttributeValues[i].Union(_userInputAttributeValues[i]).ToList() : _defaultAttributeValues[i];
            }
        }

        /// <summary>
        /// Adds the attribute to the manual attribute values if not already present.
        /// </summary>
        internal void UpdateUserInputAttributeValues(string attrib, int index)
        {
            if (!_userInputAttributeValues[index].Contains(attrib))
            {
                _userInputAttributeValues[index].Add(attrib);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates trade attribute lists
        /// </summary>
        /// <param name="attrib">attribute to add</param>
        /// <param name="index">attribute to be added to list at which index</param>
        private void UpdateAttribList(string attrib, int index)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    if (!_tradeAttributeCache[index].Contains(attrib))
                    {
                        _tradeAttributeCache[index].Add(attrib);
                    }
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
        /// Updates trade attribute lists
        /// </summary>
        /// <param name="group"></param>
        private void UpdateAttribList(AllocationGroup group)
        {
            try
            {
                if (_tradeAttributeCache != null && _keepRecords != null)
                {

                    String attrb = group.TradeAttribute1;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[0])
                            UpdateAttribList(attrb, 0);
                        UpdateUserInputAttributeValues(attrb, 0);
                    }
                    attrb = group.TradeAttribute2;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[1])
                            UpdateAttribList(attrb, 1);
                        UpdateUserInputAttributeValues(attrb, 1);
                    }
                    attrb = group.TradeAttribute3;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[2])
                            UpdateAttribList(attrb, 2);
                        UpdateUserInputAttributeValues(attrb, 2);
                    }
                    attrb = group.TradeAttribute4;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[3])
                            UpdateAttribList(attrb, 3);
                        UpdateUserInputAttributeValues(attrb, 3);
                    }
                    attrb = group.TradeAttribute5;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[4])
                            UpdateAttribList(attrb, 4);
                        UpdateUserInputAttributeValues(attrb, 4);
                    }
                    attrb = group.TradeAttribute6;
                    if (attrb != null && attrb.Length > 0)
                    {
                        if(_keepRecords[5])
                            UpdateAttribList(attrb, 5);
                        UpdateUserInputAttributeValues(attrb, 5);
                    }

                    // Process Additional Trade Attributes (AttributeNumbers 7–45)
                    List<string> tradeAttributes = group.GetTradeAttributesAsList();
                    for (int i = 0; i < tradeAttributes.Count; i++)
                    {
                        string value = tradeAttributes[i];
                        int index = 6 + i;

                        if (index < _keepRecords.Length && !string.IsNullOrWhiteSpace(value))
                        {
                            if(_keepRecords[index])
                                UpdateAttribList(value, index);
                            UpdateUserInputAttributeValues(value, index);
                        }
                    }
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
        #endregion


    }
}
