// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Disha Sharma
// Created          : 05-05-2015
//
// Last Modified By : Disha Sharma
// Last Modified On : 05-05-2015
// ***********************************************************************
// <copyright file="CostAdjustmentCache.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment.Cache
{
    /// <summary>
    /// The CostAdjustmentCache class
    /// </summary>
    internal class CostAdjustmentCache
    {
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly CostAdjustmentCache _singeltonInstance = new CostAdjustmentCache();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static CostAdjustmentCache Instance
        {
            get
            {
                return _singeltonInstance;
            }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private CostAdjustmentCache()
        {
            try
            {
                // Do cache object initialization which should be done before Initialize() method
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

        #region Cache Objects

        /// <summary>
        /// Locker object for cache
        /// </summary>
        private readonly object _cacheLockerObject = new object();

        /// <summary>
        /// List of CostAdjustmentTaxlotsForSave taxlots
        /// </summary>
        private List<CostAdjustmentTaxlotsForSave> _savedCostAdjustmentCache;

        #endregion

        /// <summary>
        /// Do the initialization of cache from database
        /// <para>This method loads data from database so it can be used to re-initialize</para><para>Calling this method multiple time will only reload data from database, still avoid calling this more than once (if re-initialization is not required)</para>
        /// </summary>
        internal void Initialize()
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    _savedCostAdjustmentCache = (List<CostAdjustmentTaxlotsForSave>)CostAdjustmentManager.GetCostAdjustmentDataFromDB();
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
        /// Add taxlots to cost adjustment cache
        /// </summary>
        /// <param name="savedTaxlots">List of taxlots to be added</param>
        internal void AddTaxlotsToCache(List<CostAdjustmentTaxlotsForSave> savedTaxlots)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    _savedCostAdjustmentCache.AddRange(savedTaxlots);
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
        /// Remove taxlots from cost adjustment cache
        /// </summary>
        /// <param name="caIds">List of cost adjustment Ids</param>
        internal void RemoveTaxlotsFromCache(List<string> caIds)
        {
            lock (_cacheLockerObject)
            {
                try
                {
                    List<CostAdjustmentTaxlotsForSave> existingTaxlots = _savedCostAdjustmentCache.Where(x => caIds.Contains(x.CAID)).ToList();
                    existingTaxlots.ForEach(x => _savedCostAdjustmentCache.Remove(x));
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
        }

        /// <summary>
        /// Get CostAdjustmentTaxlotsForSave taxlots from cache
        /// </summary>
        /// <returns>list of CostAdjustmentTaxlotsForSave taxlots</returns>
        internal List<CostAdjustmentTaxlotsForSave> GetAllCostAdjustmentTaxlots()
        {
            List<CostAdjustmentTaxlotsForSave> costadjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            try
            {
                // Added lock so that other thread wait till one thread is doing some operation on _savedCostAdjustmentCache, PRANA-9002
                lock (_cacheLockerObject)
                {
                    costadjustmentTaxlots = _savedCostAdjustmentCache;
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
            return costadjustmentTaxlots;
        }

        /// <summary>
        /// Gets list of cost adjustment Ids in cache
        /// </summary>
        /// <returns>list of cost adjustment Ids</returns>
        internal List<string> GetCostAdjustmentDataTaxlotIDs()
        {
            List<string> costAdjustmentTaxlotIDs = new List<string>();
            try
            {
                // Added lock so that other thread wait till one thread is doing some operation on _savedCostAdjustmentCache, PRANA-9002
                lock (_cacheLockerObject)
                {
                    _savedCostAdjustmentCache.ForEach(x => costAdjustmentTaxlotIDs.Add(x.TaxlotID));
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
            return costAdjustmentTaxlotIDs;
        }
    }
}
