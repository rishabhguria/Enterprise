using Prana.Allocation.Client.Constants;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using static Prana.BusinessObjects.TradeAuditActionType;

namespace Prana.Allocation.Client.Helper
{
    internal class EditTradeHelper
    {
        #region Group Editing Methods

        /// <summary>
        /// Updates the datafor trade attributes.
        /// </summary>
        /// <param name="group">The g parent.</param>
        /// <param name="taxLotID">The taxlot identifier.</param>
        /// <returns></returns>
        internal static TradeAttributes UpdateDataforTradeAttributes(AllocationGroup group, string taxLotID)
        {
            TradeAttributes tradeAttr = new TradeAttributes();
            try
            {
                if (taxLotID == string.Empty)
                {
                    tradeAttr.TradeAttribute1 = group.TradeAttribute1;
                    tradeAttr.TradeAttribute2 = group.TradeAttribute2;
                    tradeAttr.TradeAttribute3 = group.TradeAttribute3;
                    tradeAttr.TradeAttribute4 = group.TradeAttribute4;
                    tradeAttr.TradeAttribute5 = group.TradeAttribute5;
                    tradeAttr.TradeAttribute6 = group.TradeAttribute6;
                    tradeAttr.SetTradeAttribute(group.GetTradeAttributesAsDict());
                }
                else
                {
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (taxlot.TaxLotID == taxLotID)
                        {
                            tradeAttr.TradeAttribute1 = taxlot.TradeAttribute1;
                            tradeAttr.TradeAttribute2 = taxlot.TradeAttribute2;
                            tradeAttr.TradeAttribute3 = taxlot.TradeAttribute3;
                            tradeAttr.TradeAttribute4 = taxlot.TradeAttribute4;
                            tradeAttr.TradeAttribute5 = taxlot.TradeAttribute5;
                            tradeAttr.TradeAttribute6 = taxlot.TradeAttribute6;
                            tradeAttr.SetTradeAttribute(taxlot.GetTradeAttributesAsDict());
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tradeAttr;

        }

        /// <summary>
        /// Updates the dependent field values.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="group">The group.</param>
        internal static void UpdateGroupFields(string fieldName, object cellValue, AllocationGroup group)
        {
            try
            {
                switch (fieldName)
                {
                    case AllocationUIConstants.ORDER_SIDE:
                        group.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(group.OrderSide);
                        foreach (AllocationOrder order in group.Orders)
                        {
                            order.OrderSideTagValue = group.OrderSideTagValue;
                        }
                        if (SetTransactionTypeBasedOnSide(group))
                        {
                            group.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                            group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.TransactionType_Changed);
                            group.TransactionType = Regex.Replace(group.OrderSide, @"\s+", "");
                        }
                        group.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                        break;

                    case AllocationUIConstants.COUNTERPARTY_NAME:
                        group.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(group.CounterPartyName);
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        RecalculateCommission(group);
                        group.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Counterparty_Changed);
                        break;

                    case AllocationUIConstants.VENUE:
                        group.VenueID = CachedDataManager.GetInstance.GetVenueID(group.Venue);
                        group.AddTradeAction(TradeAuditActionType.ActionType.Venue_Changed);
                        break;

                    case AllocationUIConstants.CUMQTY:
                        group.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                        break;

                    case AllocationUIConstants.AVGPRICE:
                        group.UpdateTaxlotAvgPrice();
                        if (group.AssetID == (int)AssetCategory.FX || group.AssetID == (int)AssetCategory.FXForward || group.AssetID == (int)AssetCategory.Forex)
                        {
                            if (group.LeadCurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                                group.FXRate = group.AvgPrice;
                            else
                                group.FXRate = group.AvgPrice != 0 ? 1 / group.AvgPrice : 0;

                        }
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
                        //Recalculate Sec fee section
                        List<OtherFeeType> listofFeesToApply = new List<OtherFeeType>();
                        listofFeesToApply.Add(OtherFeeType.SecFee);
                        //group.IsRecalculateCommission = true;
                        ReCalculateOtherFeeForGroup(group, listofFeesToApply);
                        break;

                    case AllocationUIConstants.CAPTION_DESCRIPTION:
                        group.UpdateTaxlotDescription();
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.Description_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Description_Changed);
                        break;

                    case AllocationUIConstants.INTERNAL_COMMENTS:
                        group.UpdateTaxlotIneternalComments();
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.InternalComments_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.InternalComments_Changed);
                        break;

                    case AllocationUIConstants.ACCRUED_INTEREST:
                        group.UpdateTaxlotAccruedInterest();
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        break;

                    case AllocationUIConstants.TRANSACTION_TYPE:
                        group.UpdateTaxlotTransactionType(group.TransactionType);
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                        group.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.TransactionType_Changed);
                        break;

                    case AllocationUIConstants.SETTLEMENT_CURRENCY:
                        group.SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cellValue.ToString());
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.UpdateSettlementCurrencyInTaxlots(group);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(group, TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        break;

                    case AllocationUIConstants.BorrowerBroker:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.BorrowBroker_Changed);
                        break;

                    case AllocationUIConstants.ShortRebate:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.BorrowRate_Changed);
                        break;

                    case AllocationUIConstants.BorrowerID:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.BorrowId_Changed);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the security fields.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="group">The group.</param>
        internal static void UpdateSecurityFields(string fieldName, AllocationGroup group)
        {
            try
            {
                switch (fieldName)
                {

                    case AllocationUIConstants.UNDERLYING_DELTA:
                        group.AddTradeAction(TradeAuditActionType.ActionType.UnderlyingDelta_Changed);
                        break;

                    case AllocationUIConstants.FXRATE:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(group, TradeAuditActionType.ActionType.FxRate_Changed);
                        break;

                    case AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(group, TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                        break;

                    case AllocationUIConstants.SETTLEMENT_CURRENCY:
                        if (group.SettlementCurrencyID == group.CurrencyID)
                        {
                            SetTradeFXinGroupsAndTaxlots(group);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static void SetTradeFXinGroupsAndTaxlots(AllocationGroup group)
        {
            try
            {
                group.FXRate = group.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() ? 1 : 0;
                group.UpdateTaxlotFXRate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the dependent date fields.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="group">The group.</param>
        internal static void UpdateGroupDates(string fieldName, AllocationGroup group)
        {
            try
            {
                switch (fieldName)
                {
                    case AllocationUIConstants.AUEC_LOCAL_DATE:
                        group.ProcessDate = group.AUECLocalDate;
                        group.OriginalPurchaseDate = group.AUECLocalDate;
                        group.SettlementDate = CommonAllocationMethods.GetSettlementDate(group);
                        group.AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                        group.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                        group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        group.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                        break;

                    case AllocationUIConstants.PROCESS_DATE:
                        group.OriginalPurchaseDate = group.ProcessDate;
                        group.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                        group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        break;

                    case AllocationUIConstants.ORIGINAL_PURCHASE_DATE:
                        group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        break;

                    case AllocationUIConstants.SETTLEMENT_DATE:
                        group.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the fields for fixed income and convertible bond.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="group">The group.</param>
        internal static void UpdateFieldsForFixedIncomeAndConvertibleBond(string fieldName, AllocationGroup group)
        {
            try
            {
                switch (fieldName)
                {
                    case AllocationUIConstants.AUEC_LOCAL_DATE:
                    case AllocationUIConstants.ORIGINAL_PURCHASE_DATE:
                    case AllocationUIConstants.NIRVANA_PROCESS_DATE:
                    case AllocationUIConstants.SETTLEMENT_DATE:
                    case AllocationUIConstants.CUMQTY:
                        group.AccruedInterest = AllocationClientManager.GetInstance().CalculateAccuredInterest(DeepCopyHelper.Clone(group));
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the taxlotand orders after cell value changed.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="group">The group.</param>
        internal static void UpdateTaxlotandOrders(string fieldName, string fieldValue, AllocationGroup group)
        {
            try
            {
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    group.UpdateGroupTaxlots(fieldName, fieldValue);
                else
                {
                    //TODO: Taxlot should be created in Taxlot Object
                    TaxLot updatedTaxlot = new TaxLot();
                    updatedTaxlot.TaxLotQty = group.CumQty;
                    updatedTaxlot.TaxLotID = group.GroupID;
                    updatedTaxlot.GroupID = group.GroupID;
                    updatedTaxlot.SideMultiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                    updatedTaxlot.CopyBasicDetails((PranaBasicMessage)group);
                    //updatedTaxlot.PositionTag = (PositionTag)(GetPositionTagBySide(ModifiedGroup.OrderSideTagValue));
                    group.UpdateTaxlotState(updatedTaxlot);
                }
                UpdateGroupOrder(group);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the group order.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void UpdateGroupOrder(AllocationGroup group)
        {
            try
            {
                if (group.Orders.Count == 1)
                {
                    group.Orders[0].IsModified = true;
                    group.Orders[0].AvgPrice = group.AvgPrice;
                    group.Orders[0].CumQty = group.CumQty;
                    group.Orders[0].Description = group.Description;
                    group.Orders[0].InternalComments = group.InternalComments;
                    group.Orders[0].AUECLocalDate = group.AUECLocalDate;
                    group.Orders[0].OriginalPurchaseDate = group.OriginalPurchaseDate;
                    group.Orders[0].ProcessDate = group.ProcessDate;
                    group.Orders[0].SettlementDate = group.SettlementDate;
                    group.Orders[0].Venue = group.Venue;
                    group.Orders[0].VenueID = group.VenueID;
                    group.Orders[0].CounterPartyID = group.CounterPartyID;
                    group.Orders[0].CounterPartyName = group.CounterPartyName;
                    group.Orders[0].OrderSideTagValue = group.OrderSideTagValue;
                    group.Orders[0].OrderSide = group.OrderSide;
                    group.Orders[0].FXRate = group.FXRate;
                    group.Orders[0].FXConversionMethodOperator = group.FXConversionMethodOperator;
                    group.Orders[0].TradeAttribute1 = group.TradeAttribute1;
                    group.Orders[0].TradeAttribute2 = group.TradeAttribute2;
                    group.Orders[0].TradeAttribute3 = group.TradeAttribute3;
                    group.Orders[0].TradeAttribute4 = group.TradeAttribute4;
                    group.Orders[0].TradeAttribute5 = group.TradeAttribute5;
                    group.Orders[0].TradeAttribute6 = group.TradeAttribute6;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// this function checks whether Transaction Type will be changed on the basis of side
        /// Case 1: If side and Transaction Type have same values i.e. Transaction Type is super set of side, then Transaction Type will automatically change to side
        /// Case 2: If side and Transaction Type have not same value then ask to make same transaction type as Side
        /// </summary>
        /// <param name="e">Cell value</param>
        /// <param name="group">The group</param>
        /// <returns>Yes/No/Cancel</returns>
        internal static bool SetTransactionTypeBasedOnSide(AllocationGroup group)
        {
            bool isTransactionTypeChanged = false;
            try
            {
                string orderSideTagValue = group.OrderSideTagValue;
                string orderSideValue_Original = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideTagValue);
                string transactionType = group.TransactionType;
                if (!orderSideValue_Original.Equals(transactionType))
                {
                    MessageBoxResult result = MessageBox.Show("Transaction Type is '" + transactionType + "'. Changing the Side, Transaction Type will also change, do you want to change Transaction Type also?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information);
                    isTransactionTypeChanged = (result == MessageBoxResult.Yes) ? true : false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isTransactionTypeChanged;
        }

        /// <summary>
        /// Recalculates the commission.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static AllocationGroup RecalculateCommission(AllocationGroup group)
        {
            AllocationGroup allocationGroup = null;
            try
            {
                AllocationCompanyWisePref companyWisePref = AllocationClientManager.GetInstance().GetCompanyWisePreferences();
                if (companyWisePref.MsgOnBrokerChange)
                {
                    MessageBoxResult choice = MessageBox.Show("Would you like to calculate commission and fee again?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (choice == MessageBoxResult.Yes)
                        group.IsRecalculateCommission = true;
                }
                else if (companyWisePref.RecalculateOnBrokerChange)
                {
                    group.IsRecalculateCommission = true;
                }

                if (group.IsRecalculateCommission)
                {
                    // Commission is calculated without using backgroundworkerThread as commission in not updating while saving on change of broker, PRANA-15199
                    allocationGroup = AllocationClientManager.GetInstance().CalculateCommission(group);
                    if (allocationGroup != null)
                    {
                        group.UpdateCommissionAndFees(allocationGroup);
                        group.UpdateCommissionAndFeesAtTaxlotLevel(allocationGroup);
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return DeepCopyHelper.Clone(allocationGroup);
        }

        /// <summary>
        /// Re-Calculates the other fee for group.
        /// </summary>
        /// <param name="allGroup">All group.</param>
        internal static AllocationGroup ReCalculateOtherFeeForGroup(AllocationGroup group, List<OtherFeeType> listofFeesToApply)
        {
            AllocationGroup allocationGroup = null;
            try
            {
                allocationGroup = AllocationClientManager.GetInstance().ReCalculateOtherFeeForGroup(group, listofFeesToApply);
                if (allocationGroup != null)
                {
                    group.UpdateSecFee(allocationGroup);
                    group.UpdateSecFeeAtTaxlotLevel(allocationGroup);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return DeepCopyHelper.Clone(allocationGroup);
        }

        #endregion

        #region Taxlot Editing Methods

        /// <summary>
        /// aftions after fields value updated for taxlot.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="group">The group.</param>
        /// <param name="taxlot">The taxlot.</param>
        internal static void UpdateTaxlotFields(string fieldName, AllocationGroup group, TaxLot taxlot)
        {
            try
            {
                switch (fieldName)
                {
                    case OrderFields.PROPERTY_SETTLEMENTCURRENCY:
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        group.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                        break;

                    case AllocationUIConstants.FXRATE:
                        group.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.FxRate_Changed);
                        break;

                    case AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR:
                        group.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                        group.IsAnotherTaxlotAttributesUpdated = true;
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                        break;

                    case AllocationUIConstants.LOT_ID:
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.LotId_Changed);
                        break;

                    case AllocationUIConstants.EXTERNAL_TRANS_ID:
                        if (taxlot.ExternalTransId != null)
                        {
                            string externalTranID = taxlot.Level2ID + ":" + taxlot.ExternalTransId + ",";

                            if (externalTranID.Length > 0)
                                externalTranID = externalTranID.Substring(0, externalTranID.Length - 1);

                            taxlot.ExternalTransId = externalTranID;
                        }
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.ExternalTransId_Changed);
                        break;
                    case AllocationUIConstants.ACCRUED_INTEREST:
                        group.UpdateGroupAccruedInterest();
                        group.UpdateTaxlotState(taxlot);
                        taxlot.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        break;
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

        #region Common Editing Methods

        /// <summary>
        /// Updates the dependent trade attribute fields.
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <param name="group">The group.</param>
        /// <param name="taxlot">The taxlot</param>
        internal static void UpdateTradeAttributes(string fieldName, AllocationGroup group, TaxLot taxlot)
        {
            try
            {
                TradeAttributes tradeAttributes = new TradeAttributes();
                bool isTradeAttributeUpdated = false;
                TradeAuditActionType.ActionType tradeAction = TradeAuditActionType.ActionType.TradeEdited;
                switch (fieldName)
                {
                    case AllocationUIConstants.TradeAttribute1:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute1_Changed;
                        isTradeAttributeUpdated = true;
                        break;

                    case AllocationUIConstants.TradeAttribute2:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute2_Changed;
                        isTradeAttributeUpdated = true;
                        break;

                    case AllocationUIConstants.TradeAttribute3:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute3_Changed;
                        isTradeAttributeUpdated = true;
                        break;

                    case AllocationUIConstants.TradeAttribute4:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute4_Changed;
                        isTradeAttributeUpdated = true;
                        break;

                    case AllocationUIConstants.TradeAttribute5:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute5_Changed;
                        isTradeAttributeUpdated = true;
                        break;

                    case AllocationUIConstants.TradeAttribute6:
                        tradeAction = TradeAuditActionType.ActionType.TradeAttribute6_Changed;
                        isTradeAttributeUpdated = true;
                        break;
                    default:
                        if (fieldName.StartsWith("TradeAttribute"))
                        {
                            string attributeNumber = fieldName.Replace("TradeAttribute", "");
                            string enumName = $"TradeAttribute{attributeNumber}_Changed";
                            if (Enum.TryParse(enumName, out ActionType result))
                            {
                                tradeAction = result;
                                isTradeAttributeUpdated = true;
                            }
                        }
                            break;
                }
                if (isTradeAttributeUpdated)
                {
                    string taxlotID = (taxlot != null) ? taxlot.TaxLotID : string.Empty;
                    tradeAttributes = UpdateDataforTradeAttributes(group, taxlotID);
                    group.IsAnotherTaxlotAttributesUpdated = true;

                    // Update Audit Trail
                    if (taxlot == null)
                    {
                        group.UpdateTaxlotTradeAttributes(tradeAttributes);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(group, tradeAction);
                    }
                    else
                    {
                        group.UpdateTaxlotTradeAttributes(tradeAttributes, taxlot.TaxLotID);
                        taxlot.AddTradeAction(tradeAction);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the dependent commission and fee fields.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="group">The group.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="cellValue">The cell value.</param>
        internal static void UpdateCommissionAndFees(string fieldName, AllocationGroup group, TaxLot taxlot, object cellValue)
        {
            try
            {
                CommissionFields commFields = new CommissionFields();
                bool isCommissionChanged = false;
                TradeAuditActionType.ActionType tradeAction = TradeAuditActionType.ActionType.TradeEdited;
                switch (fieldName)
                {
                    case OrderFields.PROPERTY_COMMISSION:
                        commFields.Commission = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.Commission_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_SOFTCOMMISSION:
                        commFields.SoftCommission = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.SoftCommission_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_OTHERBROKERFEES:
                        commFields.OtherBrokerFees = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.OtherBrokerFees_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                        commFields.ClearingBrokerFee = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.ClearingBrokerFee_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_STAMPDUTY:
                        commFields.StampDuty = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.StampDuty_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_TRANSACTIONLEVY:
                        commFields.TransactionLevy = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.TransactionLevy_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_CLEARINGFEE:
                        commFields.ClearingFee = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.ClearingFee_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                        commFields.TaxOnCommissions = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.TaxOnCommission_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_MISCFEES:
                        commFields.MiscFees = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.MiscFees_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_SECFEE:
                        commFields.SecFee = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.SecFee_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_OCCFEE:
                        commFields.OccFee = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.OccFee_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_ORFFEE:
                        commFields.OrfFee = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.OrfFee_Changed;
                        isCommissionChanged = true;
                        break;

                    case OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT:
                        commFields.OptionPremiumAdjustment = Convert.ToDouble(cellValue);
                        tradeAction = TradeAuditActionType.ActionType.OptionPremiumAdjustment_Changed;
                        isCommissionChanged = true;
                        break;
                }

                if (isCommissionChanged)
                {
                    group.CommSource = CommisionSource.Manual;
                    group.SoftCommSource = CommisionSource.Manual;
                    group.CommissionSource = (int)CommisionSource.Manual;
                    group.SoftCommissionSource = (int)CommisionSource.Manual;
                    group.IsAnotherTaxlotAttributesUpdated = true;

                    if (taxlot == null)
                    {
                        group.UpdateTaxlotCommissionAndFees(commFields);
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(group, tradeAction);
                    }
                    else
                    {
                        group.UpdateGroupCommissionAndFees(commFields);
                        group.UpdateTaxlotState(taxlot);
                        group.AddTradeAction(tradeAction);
                        taxlot.AddTradeAction(tradeAction);
                    }
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
