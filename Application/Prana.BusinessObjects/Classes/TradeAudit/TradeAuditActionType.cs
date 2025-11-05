using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class TradeAuditActionType
    {
        private TradeAuditActionType() { }

        /// <summary>
        /// Used as Action Types for the TradeAudit. Before changing check the isActionDeleted and isActionEdited function in TradeAuditEntry Class. It uses the numbers assigned to determine the value
        /// </summary>
        [TypeConverter(typeof(TradeAuditActionTypeConverter))]
        public enum ActionType
        {
            REALLOCATE = 1,
            UNALLOCATE = 2,
            GROUP = 3,
            UNGROUP = 4,
            DELETE = 5,
            TradeDate_Changed = 6,
            OrderSide_Changed = 7,
            Counterparty_Changed = 8,
            ExecutedQuantity_Changed = 9,
            AvgPrice_Changed = 10,
            SettlementDate_Changed = 11,
            FxRate_Changed = 12,
            Commission_Changed = 13,
            OtherBrokerFees_Changed = 14,
            StampDuty_Changed = 15,
            TransactionLevy_Changed = 16,
            ClearingFee_Changed = 17,
            TaxOnCommission_Changed = 18,
            MiscFees_Changed = 19,
            Venue_Changed = 20,
            FxConversionMethodOperator_Changed = 21,
            ProcessDate_Changed = 22,
            OriginalPurchaseDate_Changed = 23,
            Description_Changed = 24,
            AccruedInterest_Changed = 25,
            UnderlyingDelta_Changed = 26,
            LotId_Changed = 27,
            CommissionAmount_Changed = 28,
            CommissionRate_Changed = 29,
            TradeAttribute1_Changed = 30,
            TradeAttribute2_Changed = 31,
            TradeAttribute3_Changed = 32,
            TradeAttribute4_Changed = 33,
            TradeAttribute5_Changed = 34,
            TradeAttribute6_Changed = 35,
            ExternalTransId_Changed = 36,
            //only used as default not to be used in normal course and also show error message for data not saved 
            TradeEdited = 37,
            SecFee_Changed = 38,
            OccFee_Changed = 39,
            OrfFee_Changed = 40,
            ClearingBrokerFee_Changed = 41,
            SoftCommission_Changed = 42,
            SoftCommissionAmount_Changed = 43,
            SoftCommissionRate_Changed = 44,
            TransactionType_Changed = 45,
            InternalComments_Changed = 46,
            SettlCurrency_Changed = 47,
            OptionPremiumAdjustment_Changed = 48,
            MarkPrice_Changed = 49,
            ForexRate_Changed = 50,
            Exchange_Applied_CA = 51,
            Exchange_UnApplied_CA = 52,
            Merger_Applied_CA = 53,
            Merger_UnApplied_CA = 54,
            SpinOff_Applied_CA = 55,
            SpinOff_UnApplied_CA = 56,
            NameChange_Applied_CA = 57,
            NameChange_UnApplied_CA = 58,
            StockDividend_Applied_CA = 59,
            StockDividend_UnApplied_CA = 60,
            Split_Applied_CA = 61,
            Split_UnApplied_CA = 62,
            CashDividend_Applied_CA = 63,
            CashDividend_UnApplied_CA = 64,
            Dividend_Applied_CashTransaction = 65,
            Dividend_UnApplied_CashTransaction = 66,
            CashSettlementAtCost = 67,
            CashSettlementAtZeroPrice = 68,
            CashSettlementAtClosingDateSpotPx = 69,
            DeliverFXAtCost = 70,
            DeliverFXAtCostandPNLAtClosingDateSpotPx = 71,
            PhysicalSettlement = 72,
            Expire = 73,
            Exercise_Assignment = 74,
            Exercise_AssignmentatZero = 75,
            Unwinding_for_Exercised_Option = 76,
            Unwinding_for_Expired_Option = 77,
            Unwinding_for_PhysicalSettled_Option = 78,
            Unwinding_for_Settlement_FX = 79,
            CashTransaction_Type_Changed = 80,
            CashTransaction_ExDate_Changed = 81,
            CashTransaction_PayoutDate_Changed = 82,
            CashTransaction_Amount_Changed = 83,
            Closing = 84,
            Unwinding = 85,
            //This is not reffd. anywhere in the code.It's being used in SP's "P_SaveMarkPriceChangesinAuditTrail" and "P_SaveForexRateChangesinAuditTrail"
            MarkPriceChangeFundWise = 86,
            ForexRateChangeFundWise = 87,
            DailyCalculation = 88,
            MissingSettlementFxRate_Activity = 89,
            BookAsSwap = 90,
            SwapDetailsUpdated = 91,
            PricingInput = 92,
            ActivityException = 93,
            JournalException = 94,
            DailyFundCash_Changed = 95,
            DailyFundNAV_Changed = 96,

            OrderTransferToUser = 97,
            OrderRemoved = 98,

            OrderPendingNew = 99,
            OrderCancelRequested = 100,

            OrderReplaced = 101,
            OrderRejected = 102,
            SubOrderRollover = 103,
            OrderStaged = 104,
            OrderNewSub = 105,
            SubOrderCancelled = 106,
            OrderPendingApproval = 107,
            OrderApproved = 108,
            OrderLive = 109,
            OrderDoneAway = 110,
            OrderExecuted = 111,
            OrderCancelReject = 112,
            OrderCancelReplaceRequest = 113,
            OrderManualSub = 114,
            OrderPartiallyFilled = 115,
            OrderCancelled = 116,
            OrderDoneForDay = 117,
            OrderFilled = 118,
            OrderPendingCancel = 119,
            OrderSuspended = 120,
            OrderPendingRollOver = 121,
            OrderNew = 122,
            OrderPendingReplace = 123,
            OrderAcknowledged = 124,
            SubOrderTransferToUser = 125,
            SubOrderReplaced = 126,
            ComplianceAlert = 127,
            TradeOverriden = 128,
            OrderBlocked = 129,
            SubOrderCancelRequested = 130,
            CollateralInterest_Changed = 131,
            SubOrderRemoveManualExcecution = 132,
            StrategyName_Changed = 133,
            BorrowBroker_Changed = 134,
            BorrowRate_Changed = 135,
            BorrowId_Changed = 136,
            MergeOrder = 137,
            GlobalLockAdded = 138,
            GlobalLockDeleted = 139,
            // Trade Attribute changes (7 to 45), starting from 140
            TradeAttribute7_Changed = 140,
            TradeAttribute8_Changed = 141,
            TradeAttribute9_Changed = 142,
            TradeAttribute10_Changed = 143,
            TradeAttribute11_Changed = 144,
            TradeAttribute12_Changed = 145,
            TradeAttribute13_Changed = 146,
            TradeAttribute14_Changed = 147,
            TradeAttribute15_Changed = 148,
            TradeAttribute16_Changed = 149,
            TradeAttribute17_Changed = 150,
            TradeAttribute18_Changed = 151,
            TradeAttribute19_Changed = 152,
            TradeAttribute20_Changed = 153,
            TradeAttribute21_Changed = 154,
            TradeAttribute22_Changed = 155,
            TradeAttribute23_Changed = 156,
            TradeAttribute24_Changed = 157,
            TradeAttribute25_Changed = 158,
            TradeAttribute26_Changed = 159,
            TradeAttribute27_Changed = 160,
            TradeAttribute28_Changed = 161,
            TradeAttribute29_Changed = 162,
            TradeAttribute30_Changed = 163,
            TradeAttribute31_Changed = 164,
            TradeAttribute32_Changed = 165,
            TradeAttribute33_Changed = 166,
            TradeAttribute34_Changed = 167,
            TradeAttribute35_Changed = 168,
            TradeAttribute36_Changed = 169,
            TradeAttribute37_Changed = 170,
            TradeAttribute38_Changed = 171,
            TradeAttribute39_Changed = 172,
            TradeAttribute40_Changed = 173,
            TradeAttribute41_Changed = 174,
            TradeAttribute42_Changed = 175,
            TradeAttribute43_Changed = 176,
            TradeAttribute44_Changed = 177,
            TradeAttribute45_Changed = 178
        }

        public enum ActionSource
        {
            None = 0,
            Allocation = 1,
            Blotter = 2,
            CorporateAction = 3,
            Cash = 4,
            Closing = 5,
            DailyValuation = 6,
            Import = 7,
            PricingInput = 8,
            Trade = 9,
            Compliance = 10,
            NAVLock = 11
        }

        public enum AllocationAuditComments
        {
            [EnumDescription("None")]
            None = 0,
            [EnumDescription("Group Created For Trade")]
            GroupCreatedForTrade = 1,
            [EnumDescription("Taxlot Created For Trade")]
            TaxlotCreatedForTrade = 2,
            [EnumDescription("Group Created")]
            GroupCreated = 3,
            [EnumDescription("Taxlot Created")]
            TaxlotCreated = 4,
            [EnumDescription("Group Deleted")]
            GroupDeleted = 5,
            [EnumDescription("Taxlot Deleted")]
            TaxlotDeleted = 6,
            [EnumDescription("Taxlot Modified")]
            TaxlotModified = 7,
            [EnumDescription("Group Modified")]
            GroupModified = 8,
            [EnumDescription("Data Closed")]
            DataClosed = 9,
            [EnumDescription("Partial Data Closed")]
            PartialDataClosed = 10,
            [EnumDescription("Data Unwinded")]
            DataUnwinded = 11,
            [EnumDescription("Group Created After option exercise")]
            GroupCreatedAfterOptionExercise = 12,
            [EnumDescription("Taxlot Created After option exercise")]
            TaxlotCreatedAfterOptionExercise = 13,
            [EnumDescription("Group Created After option expire")]
            GroupCreatedAfterOptionExpire = 14,
            [EnumDescription("Taxlot Created After option expire")]
            TaxlotCreatedAfterOptionExpire = 15,
            [EnumDescription("Group Deleted After unwinding option exercise")]
            GroupDeletedAfterUnwindingOptionExercise = 16,
            [EnumDescription("Taxlot Deleted After unwinding option exercise")]
            TaxlotDeletedAfterUnwindingOptionExercise = 17,
            [EnumDescription("Group Deleted After unwinding option expire")]
            GroupDeletedAfterUnwindingOptionExpire = 18,
            [EnumDescription("Taxlot Deleted After unwinding option expire")]
            TaxlotDeletedAfterUnwindingOptionExpire = 19,
            [EnumDescription("UNALLOCATE (AUTOMATED)")]
            UnallocatedAutomated = 20,
            [EnumDescription("Trades Grouped(Deleted)")]
            TradesGroupedDeleted = 21,
            [EnumDescription("Trades Grouped(Created)")]
            TradesGroupedCreated = 22,
            [EnumDescription("Groups Ungrouped(Deleted)")]
            GroupsUngroupedDeleted = 23,
            [EnumDescription("Ungrouped Groups(Created)")]
            UngroupedGroupsCreated = 24,
            [EnumDescription("Grouped order allocated from Blotter")]
            GroupedOrderAllocatedFromBlotter = 25,
        }
        /// <summary>
        /// gets value from the column of the allocationgroup for specified action
        /// </summary>
        /// <param name="action">the trade action type for which data has to be extracted</param>
        /// <param name="group">the allocation group object from which data has to be extracted</param>
        /// <returns></returns>
        public static string GetColumnValue(ActionType action, AllocationGroup group)
        {
            try
            {
                switch (action)
                {
                    case ActionType.TradeDate_Changed: return group.AUECLocalDate.ToString();
                    case ActionType.OrderSide_Changed: return group.OrderSide.ToString();
                    case ActionType.Counterparty_Changed: return group.CounterPartyName.ToString();
                    case ActionType.ExecutedQuantity_Changed: return group.CumQty.ToString();
                    case ActionType.AvgPrice_Changed: return group.AvgPrice.ToString();
                    case ActionType.SettlementDate_Changed: return group.SettlementDate.ToString();
                    case ActionType.FxRate_Changed: return group.FXRate.ToString();
                    case ActionType.Commission_Changed: return group.Commission.ToString();
                    case ActionType.SoftCommission_Changed: return group.SoftCommission.ToString();
                    case ActionType.OtherBrokerFees_Changed: return group.OtherBrokerFees.ToString();
                    case ActionType.ClearingBrokerFee_Changed: return group.ClearingBrokerFee.ToString();
                    case ActionType.StampDuty_Changed: return group.StampDuty.ToString();
                    case ActionType.TransactionLevy_Changed: return group.TransactionLevy.ToString();
                    case ActionType.ClearingFee_Changed: return group.ClearingFee.ToString();
                    case ActionType.TaxOnCommission_Changed: return group.TaxOnCommissions.ToString();
                    case ActionType.MiscFees_Changed: return group.MiscFees.ToString();
                    case ActionType.SecFee_Changed: return group.SecFee.ToString();
                    case ActionType.OccFee_Changed: return group.OccFee.ToString();
                    case ActionType.OrfFee_Changed: return group.OrfFee.ToString();
                    case ActionType.Venue_Changed: return group.Venue.ToString();
                    case ActionType.FxConversionMethodOperator_Changed: return group.FXConversionMethodOperator.ToString();
                    case ActionType.ProcessDate_Changed: return group.ProcessDate.ToString();
                    case ActionType.OriginalPurchaseDate_Changed: return group.OriginalPurchaseDate.ToString();
                    case ActionType.Description_Changed: return group.Description.ToString();
                    case ActionType.AccruedInterest_Changed: return group.AccruedInterest.ToString();
                    case ActionType.UnderlyingDelta_Changed: return group.UnderlyingDelta.ToString();
                    case ActionType.LotId_Changed: return group.LotId.ToString();
                    case ActionType.CommissionAmount_Changed: return group.CommissionAmt.ToString();
                    case ActionType.SoftCommissionAmount_Changed: return group.SoftCommissionAmt.ToString();
                    case ActionType.CommissionRate_Changed: return group.CommissionRate.ToString();
                    case ActionType.SoftCommissionRate_Changed: return group.SoftCommissionRate.ToString();
                    case ActionType.TradeAttribute1_Changed: return group.TradeAttribute1.ToString();
                    case ActionType.TradeAttribute2_Changed: return group.TradeAttribute2.ToString();
                    case ActionType.TradeAttribute3_Changed: return group.TradeAttribute3.ToString();
                    case ActionType.TradeAttribute4_Changed: return group.TradeAttribute4.ToString();
                    case ActionType.TradeAttribute5_Changed: return group.TradeAttribute5.ToString();
                    case ActionType.TradeAttribute6_Changed: return group.TradeAttribute6.ToString();
                    case ActionType.ExternalTransId_Changed: return group.ExternalTransId.ToString();
                    case ActionType.TransactionType_Changed: return group.TransactionType.ToString();
                    case ActionType.SettlCurrency_Changed: return group.SettlementCurrencyID.ToString();
                    case ActionType.InternalComments_Changed: return group.InternalComments.ToString();
                    case ActionType.BorrowBroker_Changed: return group.BorrowerBroker != null ? group.BorrowerBroker.ToString() : "";
                    case ActionType.BorrowRate_Changed: return group.ShortRebate.ToString();
                    case ActionType.BorrowId_Changed: return group.BorrowerID != null ? group.BorrowerID.ToString() : "";
                    case ActionType.BookAsSwap: return group.AssetName.ToString();
                    case ActionType.TradeEdited: return "";
                    default:
                        string attributeKey = ExtractTradeAttributeKey(action);
                        if (!string.IsNullOrEmpty(attributeKey))
                            return group.GetTradeAttributeValue(attributeKey);
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return "";
            }
        }

        /// <summary>
        /// gets the column value from the taxlot object for the specified object
        /// </summary>
        /// <param name="action">the action for which data is needed</param>
        /// <param name="taxlot">the taxlot from which the data has to be extracted</param>
        /// <returns></returns>
        public static string GetColumnValue(ActionType action, TaxLot taxlot)
        {
            try
            {
                switch (action)
                {
                    case ActionType.TradeDate_Changed: return taxlot.AUECLocalDate.ToString();
                    case ActionType.OrderSide_Changed: return taxlot.OrderSide.ToString();
                    case ActionType.Counterparty_Changed: return taxlot.CounterPartyName.ToString();
                    case ActionType.ExecutedQuantity_Changed: return taxlot.CumQty.ToString();
                    case ActionType.AvgPrice_Changed: return taxlot.AvgPrice.ToString();
                    case ActionType.SettlementDate_Changed: return taxlot.SettlementDate.ToString();
                    case ActionType.FxRate_Changed: return taxlot.FXRate.ToString();
                    case ActionType.Commission_Changed: return taxlot.Commission.ToString();
                    case ActionType.SoftCommission_Changed: return taxlot.SoftCommission.ToString();
                    case ActionType.OtherBrokerFees_Changed: return taxlot.OtherBrokerFees.ToString();
                    case ActionType.ClearingBrokerFee_Changed: return taxlot.ClearingBrokerFee.ToString();
                    case ActionType.StampDuty_Changed: return taxlot.StampDuty.ToString();
                    case ActionType.TransactionLevy_Changed: return taxlot.TransactionLevy.ToString();
                    case ActionType.ClearingFee_Changed: return taxlot.ClearingFee.ToString();
                    case ActionType.TaxOnCommission_Changed: return taxlot.TaxOnCommissions.ToString();
                    case ActionType.MiscFees_Changed: return taxlot.MiscFees.ToString();
                    case ActionType.SecFee_Changed: return taxlot.SecFee.ToString();
                    case ActionType.OccFee_Changed: return taxlot.OccFee.ToString();
                    case ActionType.OrfFee_Changed: return taxlot.OrfFee.ToString();
                    case ActionType.Venue_Changed: return taxlot.Venue.ToString();
                    case ActionType.FxConversionMethodOperator_Changed: return taxlot.FXConversionMethodOperator.ToString();
                    case ActionType.ProcessDate_Changed: return taxlot.ProcessDate.ToString();
                    case ActionType.OriginalPurchaseDate_Changed: return taxlot.OriginalPurchaseDate.ToString();
                    case ActionType.Description_Changed: return taxlot.Description.ToString();
                    case ActionType.AccruedInterest_Changed: return taxlot.AccruedInterest.ToString();
                    case ActionType.UnderlyingDelta_Changed: return taxlot.UnderlyingDelta.ToString();
                    case ActionType.LotId_Changed: return taxlot.LotId.ToString();
                    case ActionType.CommissionAmount_Changed: return taxlot.CommissionAmt.ToString();
                    case ActionType.CommissionRate_Changed: return taxlot.CommissionRate.ToString();
                    case ActionType.SoftCommissionAmount_Changed: return taxlot.SoftCommissionAmt.ToString();
                    case ActionType.SoftCommissionRate_Changed: return taxlot.SoftCommissionRate.ToString();
                    case ActionType.TradeAttribute1_Changed: return taxlot.TradeAttribute1.ToString();
                    case ActionType.TradeAttribute2_Changed: return taxlot.TradeAttribute2.ToString();
                    case ActionType.TradeAttribute3_Changed: return taxlot.TradeAttribute3.ToString();
                    case ActionType.TradeAttribute4_Changed: return taxlot.TradeAttribute4.ToString();
                    case ActionType.TradeAttribute5_Changed: return taxlot.TradeAttribute5.ToString();
                    case ActionType.TradeAttribute6_Changed: return taxlot.TradeAttribute6.ToString();
                    case ActionType.ExternalTransId_Changed: return taxlot.ExternalTransId.ToString();
                    case ActionType.TransactionType_Changed: return taxlot.TransactionType.ToString();
                    case ActionType.InternalComments_Changed: return taxlot.InternalComments.ToString();
                    case ActionType.StrategyName_Changed: return taxlot.Level2Name.ToString();
                    case ActionType.BorrowBroker_Changed: return taxlot.BorrowerBroker.ToString();
                    case ActionType.BorrowRate_Changed: return taxlot.ShortRebate.ToString();
                    case ActionType.BorrowId_Changed: return taxlot.BorrowerID.ToString();
                    case ActionType.BookAsSwap: return taxlot.AssetName.ToString();
                    case ActionType.TradeEdited: return "";
                    default:
                        string attributeKey = ExtractTradeAttributeKey(action);
                        if (!string.IsNullOrEmpty(attributeKey))
                            return taxlot.GetTradeAttributeValue(attributeKey);
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return "";
            }
        }

        private static string ExtractTradeAttributeKey(ActionType action)
        {
            string actionName = action.ToString();
            if (actionName.StartsWith("TradeAttribute") && actionName.EndsWith("_Changed"))
            {
                return actionName.Replace("_Changed", "");
            }
           return string.Empty;
        }
    }
}
