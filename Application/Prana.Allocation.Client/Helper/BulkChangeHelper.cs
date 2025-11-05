using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Allocation.Client.Helper
{
    internal static class BulkChangeHelper
    {
        #region Commission Bulk Change Methods

        /// <summary>
        /// Updates the bulk change groups.
        /// </summary>
        /// <param name="bulkChanges">The bulk changes.</param>
        /// <param name="groups">The groups.</param>
        internal static void UpdateBulkChangeGroups(BulkChangesGroupLevel bulkChanges, List<AllocationGroup> groups)
        {
            try
            {
                if (bulkChanges.GroupWise)
                    groups.ForEach(group =>
                    {
                        ApplyBulkChangeOnGroup(bulkChanges, group);
                    });
                else
                    groups.ForEach(group =>
                    {
                        ApplyBulkChangeOnTaxlot(bulkChanges, group);
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Checks if bulk change is allowed for exercised groups.
        /// </summary>
        /// <param name="bulkChanges">The bulk changes.</param>
        /// <returns></returns>
        public static bool AllowBulkChangeForExerciseGroups(BulkChangesGroupLevel bulkChanges)
        {
            if (bulkChanges.AvgPrice == 0 && bulkChanges.AvgPriceRounding < 0 && bulkChanges.FXRate == 0
                && string.IsNullOrEmpty(bulkChanges.FXConversionOperator) && bulkChanges.AccruedInterest != 0)
                return true;
            return false;
        }

        /// <summary>
        /// Applies the bulk change on group.
        /// </summary>
        /// <param name="bulkChanges">The bulk changes.</param>
        /// <param name="allocatedGroup">The allocated group.</param>
        private static void ApplyBulkChangeOnGroup(BulkChangesGroupLevel bulkChanges, AllocationGroup allocatedGroup)
        {
            try
            {
                List<TradeAuditActionType.ActionType> tradeActions = new List<TradeAuditActionType.ActionType>();
                if (bulkChanges.AvgPrice != 0)
                {
                    ChangeAvgPriceOfGroup((double)bulkChanges.AvgPrice, allocatedGroup);
                    tradeActions.Add(TradeAuditActionType.ActionType.AvgPrice_Changed);
                }

                if (bulkChanges.AvgPriceRounding >= 0)
                {
                    //PRANA-28896
                    Double avgPrice = Math.Round(allocatedGroup.AvgPrice, bulkChanges.AvgPriceRounding, MidpointRounding.AwayFromZero);
                    ChangeAvgPriceOfGroup(avgPrice, allocatedGroup);
                    tradeActions.Add(TradeAuditActionType.ActionType.AvgPrice_Changed);
                }

                if (bulkChanges.FXRate != 0 && (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() != allocatedGroup.CurrencyID))
                {
                    allocatedGroup.FXRate = double.Parse(bulkChanges.FXRate.ToString());
                    allocatedGroup.UpdateTaxlotFXRate();
                    tradeActions.Add(TradeAuditActionType.ActionType.FxRate_Changed);
                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.FxRate_Changed);
                }
                if ((!bulkChanges.FXConversionOperator.Equals(string.Empty)) && (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() != allocatedGroup.CurrencyID))
                {
                    allocatedGroup.FXConversionMethodOperator = bulkChanges.FXConversionOperator;
                    allocatedGroup.UpdateTaxlotFXConversionMethodOperator(bulkChanges.FXConversionOperator);
                    tradeActions.Add(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                }
                if (bulkChanges.AccruedInterest != 0 && (allocatedGroup.AssetName.Equals(AssetCategory.FixedIncome.ToString()) || allocatedGroup.AssetName.Equals(AssetCategory.ConvertibleBond.ToString())))
                {
                    allocatedGroup.AccruedInterest = double.Parse(bulkChanges.AccruedInterest.ToString());
                    allocatedGroup.UpdateTaxlotAccruedInterest();
                    tradeActions.Add(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                }
                if (!string.IsNullOrEmpty(bulkChanges.Description))
                {
                    allocatedGroup.Description = bulkChanges.Description;
                    allocatedGroup.UpdateTaxlotDescription();
                    tradeActions.Add(TradeAuditActionType.ActionType.Description_Changed);
                }
                if (!string.IsNullOrEmpty(bulkChanges.InternalComments))
                {
                    allocatedGroup.InternalComments = bulkChanges.InternalComments;
                    allocatedGroup.UpdateTaxlotIneternalComments();
                    tradeActions.Add(TradeAuditActionType.ActionType.InternalComments_Changed);
                }
                if (bulkChanges.CounterPartyID != 0 && bulkChanges.CounterPartyID != int.MinValue)
                {
                    allocatedGroup.CounterPartyID = bulkChanges.CounterPartyID;
                    allocatedGroup.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(allocatedGroup.CounterPartyID);
                    allocatedGroup.UpdateGroupTaxlots(string.Empty, string.Empty);
                    tradeActions.Add(TradeAuditActionType.ActionType.Counterparty_Changed);
                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Counterparty_Changed);
                }
                if (tradeActions.Count > 0)
                    allocatedGroup.AddTradeActionsToGroup(tradeActions);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Changes the average price of group.
        /// </summary>
        /// <param name="avgPrice">The average price.</param>
        /// <param name="allocatedGroup">The allocated group.</param>
        private static void ChangeAvgPriceOfGroup(Double avgPrice, AllocationGroup allocatedGroup)
        {
            try
            {
                allocatedGroup.AvgPrice = avgPrice;
                allocatedGroup.UpdateTaxlotAvgPrice();
                List<OtherFeeType> listofFeesToApply = new List<OtherFeeType>();
                listofFeesToApply.Add(OtherFeeType.SecFee);
                //allocatedGroup.IsRecalculateCommission = true;
                EditTradeHelper.ReCalculateOtherFeeForGroup(allocatedGroup, listofFeesToApply);
                allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
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
        /// Applies the bulk change on taxlot.
        /// </summary>
        /// <param name="bulkChanges">The bulk changes.</param>
        /// <param name="updatedGroup">if set to <c>true</c> [updated group].</param>
        /// <param name="allocatedGroup">The allocated group.</param>
        private static void ApplyBulkChangeOnTaxlot(BulkChangesGroupLevel bulkChanges, AllocationGroup allocatedGroup)
        {
            try
            {
                if (bulkChanges.AccountIDs != null && bulkChanges.AccountIDs.Count > 0)
                {
                    allocatedGroup.TaxLots.ForEach(taxlot =>
                    {
                        if (bulkChanges.AccountIDs.Contains(taxlot.Level1ID))
                        {
                            List<TradeAuditActionType.ActionType> tradeActions = new List<TradeAuditActionType.ActionType>();
                            if (bulkChanges.FXRate != 0)
                            {
                                taxlot.FXRate = double.Parse(bulkChanges.FXRate.ToString());
                                tradeActions.Add(TradeAuditActionType.ActionType.FxRate_Changed);
                                allocatedGroup.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                            }
                            if (!bulkChanges.FXConversionOperator.Equals(string.Empty))
                            {
                                taxlot.FXConversionMethodOperator = bulkChanges.FXConversionOperator;
                                tradeActions.Add(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                                allocatedGroup.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                            }
                            if (tradeActions.Count > 0)
                                taxlot.AddTradeActions(tradeActions);
                        }
                    });

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Trade Attribute Bulk Change Methods

        /// <summary>
        /// Updates the trade attribute groups.
        /// </summary>
        /// <param name="tradeAttributeGroups">The trade attribute groups.</param>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        internal static void UpdateTradeAttributeGroups(TradeAttributes tradeAttributeGroups, List<AllocationGroup> groups)
        {
            try
            {
                groups.ForEach(group =>
                {
                    UpdateTradeAttributes(tradeAttributeGroups, group, null);
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the trade attribute taxlot levels.
        /// </summary>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="groups">The groups.</param>
        /// <param name="tradeAttributeGroups">The trade attribute groups.</param>
        /// <returns></returns>
        internal static void UpdateTradeAttributeTaxlotLevels(List<int> accountIDs, List<AllocationGroup> groups, TradeAttributes tradeAttributeGroups)
        {
            try
            {
                groups.ForEach(allocatedGroup =>
                {
                    foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                    {
                        if (accountIDs.Contains(taxlot.Level1ID))
                        {
                            UpdateTradeAttributes(tradeAttributeGroups, allocatedGroup, taxlot);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the trade attributes.
        /// </summary>
        /// <param name="tradeAttributes">The trade attribute groups.</param>
        /// <param name="group">The allocated group.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <returns></returns>
        private static void UpdateTradeAttributes(TradeAttributes tradeAttributes, AllocationGroup group, TaxLot taxlot)
        {
            try
            {
                if (taxlot == null)
                {
                    group.UpdateNonEmptyTradeAttributes(tradeAttributes);
                    group.UpdateTaxlotNonEmptyTradeAttributes(tradeAttributes);
                    group.UpdateOrderNonEmptyTradeAttributes(tradeAttributes);
                }
                else
                {
                    taxlot.UpdateNonEmptyTradeAttributes(tradeAttributes);
                    group.UpdateTaxlotState(taxlot);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
