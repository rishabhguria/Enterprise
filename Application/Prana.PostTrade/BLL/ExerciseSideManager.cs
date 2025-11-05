using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.PostTrade.BLL
{
    internal class ExerciseSideManager
    {
        /// <summary>
        /// Ordertaxlotses the based on algo.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        internal static List<TaxLot> OrdertaxlotsBasedOnAlgo(ref List<TaxLot> taxlots, ClosingMethodology methodology)
        {
            try
            {
                PostTradeEnums.CloseTradeAlogrithm algo = PostTradeEnums.CloseTradeAlogrithm.FIFO;
                PostTradeEnums.SecondarySortCriteria secondary = PostTradeEnums.SecondarySortCriteria.None;
                if (methodology.OverrideGlobal)
                {
                    DataTable table = methodology.AccountingMethodsTable.Tables[0];
                    DataRow[] results = table.Select("FundID = '" + taxlots[0].Level1ID.ToString() + "' AND AssetName = 'Equity'");
                    if (results.Length > 0)
                    {
                        algo = (PostTradeEnums.CloseTradeAlogrithm)Convert.ToInt32(results[0][OrderFields.PROPERTY_CLOSINGALGO]);
                        secondary = (PostTradeEnums.SecondarySortCriteria)Convert.ToInt32(results[0]["SecondarySort"]);
                    }
                }
                else
                {
                    algo = methodology.ClosingAlgo;
                    secondary = methodology.SecondarySort;
                }
                DateTime currentTime = DateTime.Now;
                switch (algo)
                {
                    case PostTradeEnums.CloseTradeAlogrithm.NONE:
                    case PostTradeEnums.CloseTradeAlogrithm.FIFO:
                    case PostTradeEnums.CloseTradeAlogrithm.MFIFO:
                    case PostTradeEnums.CloseTradeAlogrithm.ACA:
                        OrderFIFOTaxlotsBasedOnSecondary(ref taxlots, secondary);
                        break;

                    case PostTradeEnums.CloseTradeAlogrithm.LIFO:
                        OrderLIFOTaxlotsBasedOnSecondary(ref taxlots, secondary);
                        break;

                    case PostTradeEnums.CloseTradeAlogrithm.HCST:
                    case PostTradeEnums.CloseTradeAlogrithm.HIHO:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.StrikePrice)
                            .ThenBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                            .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.CloseTradeAlogrithm.ETM:
                    case PostTradeEnums.CloseTradeAlogrithm.TAXADV:
                    case PostTradeEnums.CloseTradeAlogrithm.HIFO:
                    case PostTradeEnums.CloseTradeAlogrithm.LOWCOST:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.StrikePrice)
                            .ThenBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                            .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.CloseTradeAlogrithm.BTAX:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.StrikePrice)
                            .ThenByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                            .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return taxlots;
        }

        /// <summary>
        /// Orders the lifo taxlots based on secondary.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="secondaryCriteria">The secondary criteria.</param>
        private static void OrderLIFOTaxlotsBasedOnSecondary(ref List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria secondaryCriteria)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                switch (secondaryCriteria)
                {
                    case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.StrikePrice)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenByDescending(taxlot => taxlot.StrikePrice)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.TaxLotID).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenByDescending(taxlot => taxlot.TaxLotID).ToList();
                        break;
                    default:
                        taxlots = taxlots.OrderByDescending(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
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
        /// Orders the fifo taxlots based on secondary.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="secondaryCriteria">The secondary criteria.</param>
        private static void OrderFIFOTaxlotsBasedOnSecondary(ref List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria secondaryCriteria)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                switch (secondaryCriteria)
                {
                    case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.StrikePrice)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenByDescending(taxlot => taxlot.StrikePrice)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.TaxLotID).ToList();
                        break;
                    case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenByDescending(taxlot => taxlot.TaxLotID).ToList();
                        break;
                    default:
                        taxlots = taxlots.OrderBy(taxlot => taxlot.ExpirationDate > currentTime ? currentTime : taxlot.ExpirationDate)
                                .ThenBy(taxlot => taxlot.AUECLocalDate).ToList();
                        break;
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
    }
}
