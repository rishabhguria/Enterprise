// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Disha Sharma
// Created          : 05-05-2015
//
// Last Modified By : Disha Sharma
// Last Modified On : 05-05-2015
// ***********************************************************************
// <copyright file="CostAdjustmentDataManager.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessLogic;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment.DataAccess
{
    /// <summary>
    /// The CostAdjustmentDataManager class
    /// </summary>
    class CostAdjustmentDataManager
    {

        /// <summary>
        /// Saves data for cost adjustment taxlots to database
        /// </summary>
        /// <param name="savedTaxlots">List of cost adjustment taxlots</param>
        public static void SaveCostAdjustmentData(List<CostAdjustmentTaxlotsForSave> savedTaxlots)
        {
            try
            {
                string costAdjustmentCloseXML = XMLUtilities.SerializeToXML(savedTaxlots);
                string spName = "P_AL_SaveCostAdjustmentTaxlots";
                XMLSaveManager.SaveThroughXML(spName, costAdjustmentCloseXML);
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
        /// Gets data for cost adjustment taxlots from database
        /// </summary>
        /// <returns>List of cost adjustment taxlots</returns>
        public static List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentData()
        {
            List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            try
            {
                costAdjustmentTaxlots = AllocationDataManager.GetCostAdjustmentData();
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
            return costAdjustmentTaxlots;
        }

        /// <summary>
        /// Deleted data from database
        /// </summary>
        /// <param name="savedTaxlots">List of cost adjustment Ids</param>
        public static void DeleteCostAdjustmentData(List<string> caIds)
        {
            try
            {
                String csv = String.Join(",", caIds.ToArray());
                object[] parameter = new object[1];
                parameter[0] = csv;
                int rowsaffected = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_AL_DeleteCostAdjustmentTaxlots", parameter);
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
}
