// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 11-14-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 12-11-2014
// ***********************************************************************
// <copyright file="CostAdjustmentManager.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.CostAdjustment.Enums;
using Prana.LogManager;
using Prana.PostTrade.BLL.CostAdjustment.Cache;
using Prana.PostTrade.BLL.CostAdjustment.DataAccess;
using Prana.PostTrade.BLL.CostAdjustment.SortStrategy;
using Prana.PostTrade.BusinessObjects.CostAdjustment;
using Prana.PostTrade.BusinessObjects.CostAdjustment.Interfaces;
using System;
using System.Collections.Generic;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentManager.
    /// </summary>
    internal class CostAdjustmentManager
    {
        #region singletonInstance

        /// <summary>
        /// The _cost adjustment manager
        /// </summary>
        private static CostAdjustmentManager _costAdjustmentManager;
        /// <summary>
        /// The _cost adjustment manager locker
        /// </summary>
        private static readonly object _costAdjustmentManagerLocker = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>CostAdjustmentManager.</returns>
        internal static CostAdjustmentManager GetInstance()
        {
            try
            {
                lock (_costAdjustmentManagerLocker)
                {
                    if (_costAdjustmentManager == null)
                        _costAdjustmentManager = new CostAdjustmentManager();
                    return _costAdjustmentManager;
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
            return null;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CostAdjustmentManager"/> class from being created.
        /// </summary>
        private CostAdjustmentManager()
        {

        }


        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                LoadCostGenerator();
                LoadSortStrategies();
                CostAdjustmentCache.Instance.Initialize();
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
        /// Loads the cost generator.
        /// </summary>
        private void LoadCostGenerator()
        {
            try
            {
                lock (_generatorCache)
                {
                    if (!_generatorCache.ContainsKey(CostAdjustmentType.Total))
                        _generatorCache.Add(CostAdjustmentType.Total, new TotalCostAdjustmentGenerator());
                    if (!_generatorCache.ContainsKey(CostAdjustmentType.Unit))
                        _generatorCache.Add(CostAdjustmentType.Unit, new UnitCostAdjustmentGenerator());
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
        /// Loads the sort strategies.
        /// </summary>
        private void LoadSortStrategies()
        {
            try
            {
                lock (_sortStrategyCache)
                {
                    if (!_sortStrategyCache.ContainsKey(CostAdjustmentMethodology.FIFO))
                        _sortStrategyCache.Add(CostAdjustmentMethodology.FIFO, new SortByDate(true));
                    if (!_sortStrategyCache.ContainsKey(CostAdjustmentMethodology.LIFO))
                        _sortStrategyCache.Add(CostAdjustmentMethodology.LIFO, new SortByDate(false));
                    if (!_sortStrategyCache.ContainsKey(CostAdjustmentMethodology.HIFO))
                        _sortStrategyCache.Add(CostAdjustmentMethodology.HIFO, new SortByAvgPrice(true));
                    if (!_sortStrategyCache.ContainsKey(CostAdjustmentMethodology.HIHO))
                        _sortStrategyCache.Add(CostAdjustmentMethodology.HIHO, new SortByAvgPrice(false));
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
        /// The _generator cache
        /// </summary>
        readonly Dictionary<CostAdjustmentType, ICostAdjustmentGenerator> _generatorCache = new Dictionary<CostAdjustmentType, ICostAdjustmentGenerator>();
        /// <summary>
        /// The _sort strategy cache
        /// </summary>
        readonly Dictionary<CostAdjustmentMethodology, ITaxlotSortStrategy> _sortStrategyCache = new Dictionary<CostAdjustmentMethodology, ITaxlotSortStrategy>();

        /// <summary>
        /// Gets the handler for generator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ICostAdjustmentGenerator.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Behavior key not found</exception>
        private ICostAdjustmentGenerator GetHandlerForGenerator(CostAdjustmentType type)
        {
            try
            {
                lock (_generatorCache)
                {
                    if (_generatorCache.ContainsKey(type))
                        return _generatorCache[type];
                    else
                        throw new KeyNotFoundException("Behavior key not found");
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

        /// <summary>
        /// Gets the handler for generator.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>ITaxlotSortStrategy.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Sort strategy not found</exception>
        private ITaxlotSortStrategy GetHandlerForGenerator(CostAdjustmentMethodology method)
        {
            try
            {
                lock (_sortStrategyCache)
                {
                    if (_sortStrategyCache.ContainsKey(method))
                        return _sortStrategyCache[method];
                    else
                        throw new KeyNotFoundException("Sort strategy not found");
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

        /// <summary>
        /// Adjusts the cost.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <param name="parameterList">The parameter.</param>
        /// <returns>CostAdjustmentResult.</returns>
        internal CostAdjustmentResult AdjustCost(List<TaxLot> taxlotList, List<CostAdjustmentParameter> parameterList)
        {
            try
            {
                // Done changes for changing CostAdjustmentParameter to list of CostAdjustmentParameter in input parameter
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
                List<CostAdjustmentParameter> totalTypeParameters = new List<CostAdjustmentParameter>();
                List<CostAdjustmentParameter> unitTypeParameters = new List<CostAdjustmentParameter>();

                CostAdjustmentResult costAdjustmentResult = new CostAdjustmentResult();
                foreach (CostAdjustmentParameter parameter in parameterList)
                {
                    //Checked if parameter quantity is not equal to adjust quantity, then apply sort
                    if (parameter.TotalQuantity != parameter.AdjustQty)
                        parameter.Taxlots = GetHandlerForGenerator(parameter.CostAdjustmentMethod).Sort(parameter.Taxlots);

                    // add parameter to specific list  on basis of its Type
                    if (parameter.Type == CostAdjustmentType.Total)
                        totalTypeParameters.Add(parameter);
                    else
                        unitTypeParameters.Add(parameter);
                }

                // calculated CostAdjustmentResult for each Type(Total,Unit) of parameters and added it to costAdjustmentResult
                if (totalTypeParameters != null && totalTypeParameters.Count > 0)
                    costAdjustmentResult.Merge(GetHandlerForGenerator(CostAdjustmentType.Total).AdjustCost(taxlotList, totalTypeParameters));
                if (unitTypeParameters != null && unitTypeParameters.Count > 0)
                    costAdjustmentResult.Merge(GetHandlerForGenerator(CostAdjustmentType.Unit).AdjustCost(taxlotList, unitTypeParameters));

                return costAdjustmentResult;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }




        /// <summary>
        /// Gets the taxlot.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <returns>CostAdjustmentTaxlot.</returns>
        internal CostAdjustmentTaxlot GetTaxlot(TaxLot taxlot)
        {
            try
            {
                CostAdjustmentTaxlot newTaxlot = CostAdjustmentTaxlot.GetTaxlot(taxlot);
                newTaxlot.TotalCost = NotionalCalculator.GetNotional(taxlot, (decimal)taxlot.TaxLotQty);
                return newTaxlot;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Get CostAdjustmentTaxlotsForSave taxlots 
        /// </summary>
        /// <returns>list of CostAdjustmentTaxlotsForSave taxlots</returns>
        internal static List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentData()
        {
            List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            try
            {
                costAdjustmentTaxlots = CostAdjustmentCache.Instance.GetAllCostAdjustmentTaxlots();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return costAdjustmentTaxlots;
        }

        /// <summary>
        /// Saves data for cost adjustment taxlots
        /// </summary>
        /// <param name="saveTaxlots">List of cost adjustment taxlots</param>
        internal static void SaveCostAdjustmentData(List<CostAdjustmentTaxlotsForSave> saveTaxlots)
        {
            try
            {
                CostAdjustmentDataManager.SaveCostAdjustmentData(saveTaxlots);
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
        /// Deletes cost adjustment data
        /// </summary>
        /// <param name="caIds">List of cost adjustment Ids</param>
        internal static void DeleteCostAdjustmentData(List<string> caIds)
        {
            try
            {
                CostAdjustmentDataManager.DeleteCostAdjustmentData(caIds);
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
        /// Gets data for cost adjustment taxlots
        /// </summary>
        /// <returns>List of cost adjustment taxlots</returns>
        internal static List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentDataFromDB()
        {
            List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            try
            {
                costAdjustmentTaxlots = CostAdjustmentDataManager.GetCostAdjustmentData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return costAdjustmentTaxlots;
        }


        /// <summary>
        /// Get taxlot Ids for Cost Adjustment Taxlots
        /// </summary>
        /// <returns>List of taxlot Ids</returns>
        public static List<string> GetTaxlotsLatestCostAdjustmentTaxlotIds()
        {
            List<string> taxlotIdToCostAdjustmentDateDict = new List<string>();
            try
            {
                taxlotIdToCostAdjustmentDateDict = CostAdjustmentCache.Instance.GetCostAdjustmentDataTaxlotIDs();
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

            return taxlotIdToCostAdjustmentDateDict;
        }

        /// <summary>
        /// Get taxlot Ids for Cost Adjustment Taxlots from database
        /// </summary>
        /// <returns>List of taxlot Ids</returns>
        internal static List<string> GetTaxlotsLatestCostAdjustmentTaxlotIdsFromDB()
        {
            List<string> taxlotIdToCostAdjustmentDateDict = new List<string>();
            try
            {
                List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = GetCostAdjustmentDataFromDB();
                costAdjustmentTaxlots.ForEach(x => taxlotIdToCostAdjustmentDateDict.Add(x.TaxlotID));
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

            return taxlotIdToCostAdjustmentDateDict;
        }
    }
}
