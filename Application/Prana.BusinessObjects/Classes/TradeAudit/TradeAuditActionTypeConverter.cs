using Prana.LogManager;
using System;
using System.ComponentModel;
using static Prana.BusinessObjects.TradeAuditActionType;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Converter used by ultragrid in audit ui for data conversion for the action field
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class TradeAuditActionTypeConverter : EnumConverter
    {
        public TradeAuditActionTypeConverter(Type enumtype)
            : base(enumtype)
        { }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                TradeAuditActionType.ActionType action;
                int intValue;
                if (int.TryParse(value.ToString(), out intValue))
                {
                    switch (intValue)
                    {
                        case 1:
                            action = TradeAuditActionType.ActionType.REALLOCATE;
                            break;
                        case 2:
                            action = TradeAuditActionType.ActionType.UNALLOCATE;
                            break;
                        case 3:
                            action = TradeAuditActionType.ActionType.GROUP;
                            break;
                        case 4:
                            action = TradeAuditActionType.ActionType.UNGROUP;
                            break;
                        case 5:
                            action = TradeAuditActionType.ActionType.DELETE;
                            break;
                        case 6:
                            action = TradeAuditActionType.ActionType.TradeDate_Changed;
                            break;
                        case 7:
                            action = TradeAuditActionType.ActionType.OrderSide_Changed;
                            break;
                        case 8:
                            action = TradeAuditActionType.ActionType.Counterparty_Changed;
                            break;
                        case 9:
                            action = TradeAuditActionType.ActionType.ExecutedQuantity_Changed;
                            break;
                        case 10:
                            action = TradeAuditActionType.ActionType.AvgPrice_Changed;
                            break;
                        case 11:
                            action = TradeAuditActionType.ActionType.SettlementDate_Changed;
                            break;
                        case 12:
                            action = TradeAuditActionType.ActionType.FxRate_Changed;
                            break;
                        case 13:
                            action = TradeAuditActionType.ActionType.Commission_Changed;
                            break;
                        case 14:
                            action = TradeAuditActionType.ActionType.OtherBrokerFees_Changed;
                            break;
                        case 15:
                            action = TradeAuditActionType.ActionType.StampDuty_Changed;
                            break;
                        case 16:
                            action = TradeAuditActionType.ActionType.TransactionLevy_Changed;
                            break;
                        case 17:
                            action = TradeAuditActionType.ActionType.ClearingFee_Changed;
                            break;
                        case 18:
                            action = TradeAuditActionType.ActionType.TaxOnCommission_Changed;
                            break;
                        case 19:
                            action = TradeAuditActionType.ActionType.MiscFees_Changed;
                            break;
                        case 20:
                            action = TradeAuditActionType.ActionType.Venue_Changed;
                            break;
                        case 21:
                            action = TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed;
                            break;
                        case 22:
                            action = TradeAuditActionType.ActionType.ProcessDate_Changed;
                            break;
                        case 23:
                            action = TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed;
                            break;
                        case 24:
                            action = TradeAuditActionType.ActionType.Description_Changed;
                            break;
                        case 25:
                            action = TradeAuditActionType.ActionType.AccruedInterest_Changed;
                            break;
                        case 26:
                            action = TradeAuditActionType.ActionType.UnderlyingDelta_Changed;
                            break;
                        case 27:
                            action = TradeAuditActionType.ActionType.LotId_Changed;
                            break;
                        case 28:
                            action = TradeAuditActionType.ActionType.CommissionAmount_Changed;
                            break;
                        case 29:
                            action = TradeAuditActionType.ActionType.CommissionRate_Changed;
                            break;
                        case 30:
                            action = TradeAuditActionType.ActionType.TradeAttribute1_Changed;
                            break;
                        case 31:
                            action = TradeAuditActionType.ActionType.TradeAttribute2_Changed;
                            break;
                        case 32:
                            action = TradeAuditActionType.ActionType.TradeAttribute3_Changed;
                            break;
                        case 33:
                            action = TradeAuditActionType.ActionType.TradeAttribute4_Changed;
                            break;
                        case 34:
                            action = TradeAuditActionType.ActionType.TradeAttribute5_Changed;
                            break;
                        case 35:
                            action = TradeAuditActionType.ActionType.TradeAttribute6_Changed;
                            break;
                        case 36:
                            action = TradeAuditActionType.ActionType.ExternalTransId_Changed;
                            break;
                        case 37:
                            action = TradeAuditActionType.ActionType.TradeEdited;
                            break;
                        case 38:
                            action = TradeAuditActionType.ActionType.SecFee_Changed;
                            break;
                        case 39:
                            action = TradeAuditActionType.ActionType.OccFee_Changed;
                            break;
                        case 40:
                            action = TradeAuditActionType.ActionType.OrfFee_Changed;
                            break;
                        case 41:
                            action = TradeAuditActionType.ActionType.ClearingBrokerFee_Changed;
                            break;
                        case 42:
                            action = TradeAuditActionType.ActionType.SoftCommission_Changed;
                            break;
                        case 43:
                            action = TradeAuditActionType.ActionType.SoftCommissionAmount_Changed;
                            break;
                        case 44:
                            action = TradeAuditActionType.ActionType.SoftCommissionRate_Changed;
                            break;
                        case 45:
                            action = TradeAuditActionType.ActionType.TransactionType_Changed;
                            break;
                        case 46:
                            action = TradeAuditActionType.ActionType.InternalComments_Changed;
                            break;
                        case 47:
                            action = TradeAuditActionType.ActionType.SettlCurrency_Changed;
                            break;
                        case 48:
                            action = TradeAuditActionType.ActionType.OptionPremiumAdjustment_Changed;
                            break;
                        case 49:
                            action = TradeAuditActionType.ActionType.MarkPrice_Changed;
                            break;
                        case 50:
                            action = TradeAuditActionType.ActionType.ForexRate_Changed;
                            break;
                        case 51:
                            action = TradeAuditActionType.ActionType.Exchange_Applied_CA;
                            break;
                        case 52:
                            action = TradeAuditActionType.ActionType.Exchange_UnApplied_CA;
                            break;
                        case 53:
                            action = TradeAuditActionType.ActionType.Merger_Applied_CA;
                            break;
                        case 54:
                            action = TradeAuditActionType.ActionType.Merger_UnApplied_CA;
                            break;
                        case 55:
                            action = TradeAuditActionType.ActionType.SpinOff_Applied_CA;
                            break;
                        case 56:
                            action = TradeAuditActionType.ActionType.SpinOff_UnApplied_CA;
                            break;
                        case 57:
                            action = TradeAuditActionType.ActionType.NameChange_Applied_CA;
                            break;
                        case 58:
                            action = TradeAuditActionType.ActionType.NameChange_Applied_CA;
                            break;
                        case 59:
                            action = TradeAuditActionType.ActionType.StockDividend_Applied_CA;
                            break;
                        case 60:
                            action = TradeAuditActionType.ActionType.StockDividend_UnApplied_CA;
                            break;
                        case 61:
                            action = TradeAuditActionType.ActionType.Split_Applied_CA;
                            break;
                        case 62:
                            action = TradeAuditActionType.ActionType.Split_UnApplied_CA;
                            break;
                        case 63:
                            action = TradeAuditActionType.ActionType.CashDividend_Applied_CA;
                            break;
                        case 64:
                            action = TradeAuditActionType.ActionType.CashDividend_UnApplied_CA;
                            break;
                        case 65:
                            action = TradeAuditActionType.ActionType.Dividend_Applied_CashTransaction;
                            break;
                        case 66:
                            action = TradeAuditActionType.ActionType.Dividend_UnApplied_CashTransaction;
                            break;
                        case 67:
                            action = TradeAuditActionType.ActionType.CashSettlementAtCost;
                            break;
                        case 68:
                            action = TradeAuditActionType.ActionType.CashSettlementAtZeroPrice;
                            break;
                        case 69:
                            action = TradeAuditActionType.ActionType.CashSettlementAtClosingDateSpotPx;
                            break;
                        case 70:
                            action = TradeAuditActionType.ActionType.DeliverFXAtCost;
                            break;
                        case 71:
                            action = TradeAuditActionType.ActionType.DeliverFXAtCostandPNLAtClosingDateSpotPx;
                            break;
                        case 72:
                            action = TradeAuditActionType.ActionType.PhysicalSettlement;
                            break;
                        case 73:
                            action = TradeAuditActionType.ActionType.Expire;
                            break;
                        case 74:
                            action = TradeAuditActionType.ActionType.Exercise_Assignment;
                            break;
                        case 75:
                            action = TradeAuditActionType.ActionType.Exercise_AssignmentatZero;
                            break;
                        case 76:
                            action = TradeAuditActionType.ActionType.Unwinding_for_Exercised_Option;
                            break;
                        case 77:
                            action = TradeAuditActionType.ActionType.Unwinding_for_Expired_Option;
                            break;
                        case 78:
                            action = TradeAuditActionType.ActionType.Unwinding_for_PhysicalSettled_Option;
                            break;
                        case 79:
                            action = TradeAuditActionType.ActionType.Unwinding_for_Settlement_FX;
                            break;
                        case 80:
                            action = TradeAuditActionType.ActionType.CashTransaction_Type_Changed;
                            break;
                        case 81:
                            action = TradeAuditActionType.ActionType.CashTransaction_ExDate_Changed;
                            break;
                        case 82:
                            action = TradeAuditActionType.ActionType.CashTransaction_PayoutDate_Changed;
                            break;
                        case 83:
                            action = TradeAuditActionType.ActionType.CashTransaction_Amount_Changed;
                            break;
                        case 84:
                            action = TradeAuditActionType.ActionType.Closing;
                            break;
                        case 85:
                            action = TradeAuditActionType.ActionType.Unwinding;
                            break;
                        case 86:
                            action = TradeAuditActionType.ActionType.MarkPriceChangeFundWise;
                            break;
                        case 87:
                            action = TradeAuditActionType.ActionType.ForexRateChangeFundWise;
                            break;
                        case 90:
                            action = TradeAuditActionType.ActionType.BookAsSwap;
                            break;
                        case 91:
                            action = TradeAuditActionType.ActionType.SwapDetailsUpdated;
                            break;
                        case 92:
                            action = TradeAuditActionType.ActionType.PricingInput;
                            break;
                        case 93:
                            action = TradeAuditActionType.ActionType.ActivityException;
                            break;
                        case 94:
                            action = TradeAuditActionType.ActionType.JournalException;
                            break;
                        case 95:
                            action = TradeAuditActionType.ActionType.DailyFundCash_Changed;
                            break;
                        case 96:
                            action = TradeAuditActionType.ActionType.DailyFundNAV_Changed;
                            break;
                        case 97:
                            action = TradeAuditActionType.ActionType.OrderTransferToUser;
                            break;
                        case 98:
                            action = TradeAuditActionType.ActionType.OrderRemoved;
                            break;
                        case 99:
                            action = TradeAuditActionType.ActionType.OrderPendingNew;
                            break;
                        case 100:
                            action = TradeAuditActionType.ActionType.OrderCancelRequested;
                            break;
                        case 101:
                            action = TradeAuditActionType.ActionType.OrderReplaced;
                            break;
                        case 102:
                            action = TradeAuditActionType.ActionType.OrderRejected;
                            break;
                        case 103:
                            action = TradeAuditActionType.ActionType.SubOrderRollover;
                            break;
                        case 104:
                            action = TradeAuditActionType.ActionType.OrderStaged;
                            break;
                        case 105:
                            action = TradeAuditActionType.ActionType.OrderNewSub;
                            break;
                        case 106:
                            action = TradeAuditActionType.ActionType.SubOrderCancelled;
                            break;
                        case 107:
                            action = TradeAuditActionType.ActionType.OrderPendingApproval;
                            break;
                        case 108:
                            action = TradeAuditActionType.ActionType.OrderApproved;
                            break;
                        case 109:
                            action = TradeAuditActionType.ActionType.OrderLive;
                            break;
                        case 110:
                            action = TradeAuditActionType.ActionType.OrderDoneAway;
                            break;
                        case 111:
                            action = TradeAuditActionType.ActionType.OrderExecuted;
                            break;
                        case 112:
                            action = TradeAuditActionType.ActionType.OrderCancelReject;
                            break;
                        case 113:
                            action = TradeAuditActionType.ActionType.OrderCancelReplaceRequest;
                            break;
                        case 114:
                            action = TradeAuditActionType.ActionType.OrderManualSub;
                            break;
                        case 115:
                            action = TradeAuditActionType.ActionType.OrderPartiallyFilled;
                            break;
                        case 116:
                            action = TradeAuditActionType.ActionType.OrderCancelled;
                            break;
                        case 117:
                            action = TradeAuditActionType.ActionType.OrderDoneForDay;
                            break;
                        case 118:
                            action = TradeAuditActionType.ActionType.OrderFilled;
                            break;
                        case 119:
                            action = TradeAuditActionType.ActionType.OrderPendingCancel;
                            break;
                        case 120:
                            action = TradeAuditActionType.ActionType.OrderSuspended;
                            break;
                        case 121:
                            action = TradeAuditActionType.ActionType.OrderPendingRollOver;
                            break;
                        case 122:
                            action = TradeAuditActionType.ActionType.OrderNew;
                            break;
                        case 123:
                            action = TradeAuditActionType.ActionType.OrderPendingReplace;
                            break;
                        case 124:
                            action = TradeAuditActionType.ActionType.OrderAcknowledged;
                            break;
                        case 125:
                            action = TradeAuditActionType.ActionType.SubOrderTransferToUser;
                            break;
                        case 126:
                            action = TradeAuditActionType.ActionType.SubOrderReplaced;
                            break;
                        case 127:
                            action = TradeAuditActionType.ActionType.ComplianceAlert;
                            break;
                        case 128:
                            action = TradeAuditActionType.ActionType.TradeOverriden;
                            break;
                        case 129:
                            action = TradeAuditActionType.ActionType.OrderBlocked;
                            break;
                        case 130:
                            action = TradeAuditActionType.ActionType.SubOrderCancelRequested;
                            break;
                        case 132:
                            action = TradeAuditActionType.ActionType.SubOrderRemoveManualExcecution;
                            break;

                        case 133:
                            action = TradeAuditActionType.ActionType.StrategyName_Changed;
                            break;
                        case 134:
                            action = TradeAuditActionType.ActionType.BorrowBroker_Changed;
                            break;
                        case 135:
                            action = TradeAuditActionType.ActionType.BorrowRate_Changed;
                            break;
                        case 136:
                            action = TradeAuditActionType.ActionType.BorrowId_Changed;
                            break;
                        case 137:
                            action = TradeAuditActionType.ActionType.MergeOrder;
                            break;
                        case 138:
                            action = TradeAuditActionType.ActionType.GlobalLockAdded;
                            break;
                        case 139:
                            action = TradeAuditActionType.ActionType.GlobalLockDeleted;
                            break;
                        default:
                            action = (TradeAuditActionType.ActionType)intValue; ;
                            break;
                    }
                }
                else
                {
                    action = (TradeAuditActionType.ActionType)value;
                }
                switch (action)
                {
                    case TradeAuditActionType.ActionType.AccruedInterest_Changed: return "Accrued Interest Changed";
                    case TradeAuditActionType.ActionType.AvgPrice_Changed: return "Average Price Changed";
                    case TradeAuditActionType.ActionType.ClearingFee_Changed: return "AUEC Fee1 Changed";
                    case TradeAuditActionType.ActionType.Commission_Changed: return "Commission Changed";
                    case TradeAuditActionType.ActionType.SoftCommission_Changed: return "Soft Commission Changed";
                    case TradeAuditActionType.ActionType.CommissionAmount_Changed: return "Commission Amount Changed";
                    case TradeAuditActionType.ActionType.CommissionRate_Changed: return "Commission Rate Changed";
                    case TradeAuditActionType.ActionType.SoftCommissionAmount_Changed: return "Soft Commission Amount Changed";
                    case TradeAuditActionType.ActionType.SoftCommissionRate_Changed: return "Soft Commission Rate Changed";
                    case TradeAuditActionType.ActionType.Counterparty_Changed: return "Counterparty Changed";
                    case TradeAuditActionType.ActionType.DELETE: return "Trade Deleted";
                    case TradeAuditActionType.ActionType.Description_Changed: return "Description Changed";
                    case TradeAuditActionType.ActionType.ExecutedQuantity_Changed: return "Executed Quantity Changed";
                    case TradeAuditActionType.ActionType.ExternalTransId_Changed: return "External TransId Changed";
                    case TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed: return "FxConversionMethodOperator Changed";
                    case TradeAuditActionType.ActionType.FxRate_Changed: return "FxRate Changed";
                    case TradeAuditActionType.ActionType.GROUP: return "Trades Grouped";
                    case TradeAuditActionType.ActionType.LotId_Changed: return "LotId Changed";
                    case TradeAuditActionType.ActionType.MiscFees_Changed: return "AUEC Fee2 Changed";
                    case TradeAuditActionType.ActionType.OrderSide_Changed: return "OrderSide Changed";
                    case TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed: return "Original Purchase Date Changed";
                    case TradeAuditActionType.ActionType.OtherBrokerFees_Changed: return "Other Broker Fees Changed";
                    case TradeAuditActionType.ActionType.ClearingBrokerFee_Changed: return "Clearing Broker Fee Changed";
                    case TradeAuditActionType.ActionType.ProcessDate_Changed: return "Process Date Changed";
                    case TradeAuditActionType.ActionType.REALLOCATE: return "Trades Allocated/Reallocated";
                    case TradeAuditActionType.ActionType.SettlementDate_Changed: return "Settlement Date Changed";
                    case TradeAuditActionType.ActionType.StampDuty_Changed: return "Stamp Duty Changed";
                    case TradeAuditActionType.ActionType.TaxOnCommission_Changed: return "Tax On Commission Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute1_Changed: return "Trade Attribute1 Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute2_Changed: return "Trade Attribute2 Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute3_Changed: return "Trade Attribute3 Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute4_Changed: return "Trade Attribute4 Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute5_Changed: return "Trade Attribute5 Changed";
                    case TradeAuditActionType.ActionType.TradeAttribute6_Changed: return "Trade Attribute6 Changed";
                    case TradeAuditActionType.ActionType.TradeDate_Changed: return "Trade Date Changed";
                    case TradeAuditActionType.ActionType.TradeEdited: return "Trade Edited";
                    case TradeAuditActionType.ActionType.TransactionLevy_Changed: return "Transaction Levy Changed";
                    case TradeAuditActionType.ActionType.UNALLOCATE: return "Trades Unallocated";
                    case TradeAuditActionType.ActionType.UnderlyingDelta_Changed: return "Underlying Delta Changed";
                    case TradeAuditActionType.ActionType.UNGROUP: return "Trades Ungrouped";
                    case TradeAuditActionType.ActionType.Venue_Changed: return "Venue Changed";
                    case TradeAuditActionType.ActionType.SecFee_Changed: return "Sec Fee Changed";
                    case TradeAuditActionType.ActionType.OccFee_Changed: return "Occ Fee Changed";
                    case TradeAuditActionType.ActionType.OrfFee_Changed: return "Orf Fee Changed";
                    case TradeAuditActionType.ActionType.TransactionType_Changed: return "Transaction Type Changed";
                    case TradeAuditActionType.ActionType.MarkPrice_Changed: return "Mark Price Changed";
                    case TradeAuditActionType.ActionType.ForexRate_Changed: return "Forex Rate Changed";
                    case TradeAuditActionType.ActionType.Exchange_Applied_CA: return "Exchange CA Applied";
                    case TradeAuditActionType.ActionType.Exchange_UnApplied_CA: return "Exchange CA UnApplied";
                    case TradeAuditActionType.ActionType.Merger_Applied_CA: return "Merger CA Applied";
                    case TradeAuditActionType.ActionType.Merger_UnApplied_CA: return "Merger CA UnApplied";
                    case TradeAuditActionType.ActionType.SpinOff_Applied_CA: return "Spinoff CA Applied";
                    case TradeAuditActionType.ActionType.SpinOff_UnApplied_CA: return "Spinoff CA UnApplied";
                    case TradeAuditActionType.ActionType.NameChange_Applied_CA: return "Name Change CA Applied";
                    case TradeAuditActionType.ActionType.NameChange_UnApplied_CA: return "Name Change CA UnApplied";
                    case TradeAuditActionType.ActionType.StockDividend_Applied_CA: return "Stock Dividend CA Applied";
                    case TradeAuditActionType.ActionType.StockDividend_UnApplied_CA: return "Stock Dividend CA UnApplied";
                    case TradeAuditActionType.ActionType.Split_Applied_CA: return "Split CA Applied";
                    case TradeAuditActionType.ActionType.Split_UnApplied_CA: return "Split CA UnApplied";
                    case TradeAuditActionType.ActionType.CashDividend_Applied_CA: return "Cash Dividend CA Applied";
                    case TradeAuditActionType.ActionType.CashDividend_UnApplied_CA: return "Cash Dividend CA UnApplied";
                    case TradeAuditActionType.ActionType.Dividend_Applied_CashTransaction: return "Cash Transaction Dividend Applied";
                    case TradeAuditActionType.ActionType.Dividend_UnApplied_CashTransaction: return "Cash Transaction Dividend UnApplied";
                    case TradeAuditActionType.ActionType.CashSettlementAtCost: return "Cash Settlement At Cost";
                    case TradeAuditActionType.ActionType.CashSettlementAtZeroPrice: return "Cash Settlement At Zero Price";
                    case TradeAuditActionType.ActionType.CashSettlementAtClosingDateSpotPx: return "Cash Settlement At Closing Date Spot Px";
                    case TradeAuditActionType.ActionType.DeliverFXAtCost: return "Deliver FX At Cost";
                    case TradeAuditActionType.ActionType.DeliverFXAtCostandPNLAtClosingDateSpotPx: return "Deliver FX At Cost, PNL At Closing Date Spot Px";
                    case TradeAuditActionType.ActionType.PhysicalSettlement: return "Physical Settlement for Option";
                    case TradeAuditActionType.ActionType.Expire: return "Expiration for Option";
                    case TradeAuditActionType.ActionType.Exercise_Assignment: return "Exercise/Assignment for Option";
                    case TradeAuditActionType.ActionType.Exercise_AssignmentatZero: return "Exercise/Assignment At Zero for Option";
                    case TradeAuditActionType.ActionType.Unwinding_for_Exercised_Option: return "Data Unwinded for Exercised Option";
                    case TradeAuditActionType.ActionType.Unwinding_for_Expired_Option: return "Data Unwinded for Expired Option";
                    case TradeAuditActionType.ActionType.Unwinding_for_PhysicalSettled_Option: return "Data Unwinded for Physical Settled Option";
                    case TradeAuditActionType.ActionType.Unwinding_for_Settlement_FX: return "Data Unwinded for Cash Settled FX";
                    case TradeAuditActionType.ActionType.CashTransaction_Type_Changed: return "Cash Transaction Type Changed";
                    case TradeAuditActionType.ActionType.CashTransaction_ExDate_Changed: return "Cash Transaction Ex Date Changed";
                    case TradeAuditActionType.ActionType.CashTransaction_PayoutDate_Changed: return "Cash Transaction Payout Date Changed";
                    case TradeAuditActionType.ActionType.CashTransaction_Amount_Changed: return "Cash Transaction Amount Changed";
                    case TradeAuditActionType.ActionType.Closing: return "Data Closed";
                    case TradeAuditActionType.ActionType.Unwinding: return "Data Unwinded";
                    case TradeAuditActionType.ActionType.BookAsSwap: return "Booked As Swap";
                    case TradeAuditActionType.ActionType.SwapDetailsUpdated: return "Swap Details Updated";
                    case TradeAuditActionType.ActionType.OrderTransferToUser: return "Order Transfered To User";
                    case TradeAuditActionType.ActionType.SubOrderTransferToUser: return "Sub-Order Transferred To User";
                    case TradeAuditActionType.ActionType.StrategyName_Changed: return "Strategy Name";

                    case TradeAuditActionType.ActionType.OrderReplaced: return "Order Replaced";
                    case TradeAuditActionType.ActionType.SubOrderReplaced: return "Sub-Order Replaced";
                    case TradeAuditActionType.ActionType.OrderRemoved: return "Order Removed";
                    case TradeAuditActionType.ActionType.SubOrderRollover: return "Sub-Order Rollover Requested";
                    case TradeAuditActionType.ActionType.OrderRejected: return "Order Rejected";
                    case TradeAuditActionType.ActionType.OrderCancelRequested: return "Order Cancel Requested";
                    case TradeAuditActionType.ActionType.OrderStaged: return "Stage Order Created";
                    case TradeAuditActionType.ActionType.OrderNewSub: return "Order New Sub Created";
                    case TradeAuditActionType.ActionType.SubOrderCancelled: return "Sub-Order Cancelled";
                    case TradeAuditActionType.ActionType.OrderPendingApproval: return "Order Pending Approval(Compliance)";
                    case TradeAuditActionType.ActionType.OrderApproved: return "Order Approved(Compliance)";
                    case TradeAuditActionType.ActionType.OrderLive: return "New Live Order Created";
                    case TradeAuditActionType.ActionType.OrderDoneAway: return "Done Away Traded";
                    case TradeAuditActionType.ActionType.OrderExecuted: return "Order Executed";
                    case TradeAuditActionType.ActionType.OrderCancelReject: return "Cancel Order Rejected";
                    case TradeAuditActionType.ActionType.OrderCancelReplaceRequest: return "Order Replace Requested";
                    case TradeAuditActionType.ActionType.OrderManualSub: return "Order Manual Sub Created";
                    case TradeAuditActionType.ActionType.OrderPartiallyFilled: return "Order Partially Filled";
                    case TradeAuditActionType.ActionType.OrderCancelled: return "Order Cancelled";
                    case TradeAuditActionType.ActionType.OrderDoneForDay: return "Order Done For Day";
                    case TradeAuditActionType.ActionType.OrderFilled: return "Order Filled";
                    case TradeAuditActionType.ActionType.OrderPendingCancel: return "Order Pending Cancel";
                    case TradeAuditActionType.ActionType.OrderSuspended: return "Order Suspended";
                    case TradeAuditActionType.ActionType.OrderPendingRollOver: return "Order Pending RollOver";
                    case TradeAuditActionType.ActionType.OrderNew: return "Order New";
                    case TradeAuditActionType.ActionType.OrderPendingReplace: return "Order Pending Replace";
                    case TradeAuditActionType.ActionType.OrderAcknowledged: return "Order Acknowledged";
                    case TradeAuditActionType.ActionType.ComplianceAlert: return "Compliance Alert";
                    case TradeAuditActionType.ActionType.TradeOverriden: return "Trade Overriden (Compliance)";
                    case TradeAuditActionType.ActionType.OrderBlocked: return "Order Blocked (Compliance)";
                    case TradeAuditActionType.ActionType.SubOrderCancelRequested: return "Sub Order Cancel Requested";
                    case TradeAuditActionType.ActionType.SubOrderRemoveManualExcecution: return "Sub Order Manual Excecution Removed";
                    case TradeAuditActionType.ActionType.BorrowBroker_Changed: return "Borrow Broker Changed";
                    case TradeAuditActionType.ActionType.BorrowRate_Changed: return "Borrow Rate Changed";
                    case TradeAuditActionType.ActionType.BorrowId_Changed: return "Borrow Id Changed";
                    case TradeAuditActionType.ActionType.MergeOrder: return "Merge Orders";
                    case TradeAuditActionType.ActionType.GlobalLockAdded: return "Global Lock Added ";
                    case TradeAuditActionType.ActionType.GlobalLockDeleted: return "Global Lock Deleted";
                    default:
                        string actionName = action.ToString();
                        if (actionName.StartsWith("TradeAttribute") && actionName.EndsWith("_Changed"))
                        {
                            string attributeKey = actionName.Replace("_Changed", "").Replace("TradeAttribute", "");
                            if (!string.IsNullOrEmpty(attributeKey))
                                return "Trade Attribute" + attributeKey + " Changed";
                        }
                        return action.ToString();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return "Trade Edited";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                switch (value.ToString().Trim())
                {
                    case "Accrued Interest Changed": return TradeAuditActionType.ActionType.AccruedInterest_Changed;
                    case "Average Price Changed": return TradeAuditActionType.ActionType.AvgPrice_Changed;
                    case "AUEC Fee1 Changed": return TradeAuditActionType.ActionType.ClearingFee_Changed;
                    case "Commission Changed": return TradeAuditActionType.ActionType.Commission_Changed;
                    case "Soft Commission Changed": return TradeAuditActionType.ActionType.SoftCommission_Changed;
                    case "Commission Amount Changed": return TradeAuditActionType.ActionType.CommissionAmount_Changed;
                    case "Commission Rate Changed": return TradeAuditActionType.ActionType.CommissionRate_Changed;
                    case "Soft Commission Amount Changed": return TradeAuditActionType.ActionType.SoftCommissionAmount_Changed;
                    case "Soft Commission Rate Changed": return TradeAuditActionType.ActionType.SoftCommissionRate_Changed;
                    case "Counterparty Changed": return TradeAuditActionType.ActionType.Counterparty_Changed;
                    case "Trade Deleted": return TradeAuditActionType.ActionType.DELETE;
                    case "Description Changed": return TradeAuditActionType.ActionType.Description_Changed;
                    case "Executed Quantity Changed": return TradeAuditActionType.ActionType.ExecutedQuantity_Changed;
                    case "External TransId Changed": return TradeAuditActionType.ActionType.ExternalTransId_Changed;
                    case "FxConversionMethodOperator Changed": return TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed;
                    case "FxRate Changed": return TradeAuditActionType.ActionType.FxRate_Changed;
                    case "Trades Grouped": return TradeAuditActionType.ActionType.GROUP;
                    case "LotId Changed": return TradeAuditActionType.ActionType.LotId_Changed;
                    case "AUEC Fee2 Changed": return TradeAuditActionType.ActionType.MiscFees_Changed;
                    case "OrderSide Changed": return TradeAuditActionType.ActionType.OrderSide_Changed;
                    case "Original Purchase Date Changed": return TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed;
                    case "Other Broker Fees Changed": return TradeAuditActionType.ActionType.OtherBrokerFees_Changed;
                    case "Clearing Broker Fee Changed": return TradeAuditActionType.ActionType.ClearingBrokerFee_Changed;
                    case "Process Date Changed": return TradeAuditActionType.ActionType.ProcessDate_Changed;
                    case "Trades Allocated/Reallocated": return TradeAuditActionType.ActionType.REALLOCATE;
                    case "Settlement Date Changed": return TradeAuditActionType.ActionType.SettlementDate_Changed;
                    case "Stamp Duty Changed": return TradeAuditActionType.ActionType.StampDuty_Changed;
                    case "Tax On Commission Changed": return TradeAuditActionType.ActionType.TaxOnCommission_Changed;
                    case "Trade Attribute1 Changed": return TradeAuditActionType.ActionType.TradeAttribute1_Changed;
                    case "Trade Attribute2 Changed": return TradeAuditActionType.ActionType.TradeAttribute2_Changed;
                    case "Trade Attribute3 Changed": return TradeAuditActionType.ActionType.TradeAttribute3_Changed;
                    case "Trade Attribute4 Changed": return TradeAuditActionType.ActionType.TradeAttribute4_Changed;
                    case "Trade Attribute5 Changed": return TradeAuditActionType.ActionType.TradeAttribute5_Changed;
                    case "Trade Attribute6 Changed": return TradeAuditActionType.ActionType.TradeAttribute6_Changed;
                    case "Trade Date Changed": return TradeAuditActionType.ActionType.TradeDate_Changed;
                    case "Trade Edited": return TradeAuditActionType.ActionType.TradeEdited;
                    case "Transaction Levy Changed": return TradeAuditActionType.ActionType.TransactionLevy_Changed;
                    case "Trades Unallocated": return TradeAuditActionType.ActionType.UNALLOCATE;
                    case "Underlying Delta Changed": return TradeAuditActionType.ActionType.UnderlyingDelta_Changed;
                    case "Trades Ungrouped": return TradeAuditActionType.ActionType.UNGROUP;
                    case "Venue Changed": return TradeAuditActionType.ActionType.Venue_Changed;
                    case "Sec Fee Changed": return TradeAuditActionType.ActionType.SecFee_Changed;
                    case "Occ Fee Changed": return TradeAuditActionType.ActionType.OccFee_Changed;
                    case "Orf Fee Changed": return TradeAuditActionType.ActionType.OrfFee_Changed;
                    case "Transaction Type Changed": return TradeAuditActionType.ActionType.TransactionType_Changed;
                    case "Mark Price Changed": return TradeAuditActionType.ActionType.MarkPrice_Changed;
                    case "Forex Rate Changed": return TradeAuditActionType.ActionType.ForexRate_Changed;
                    case "Exchange CA Applied": return TradeAuditActionType.ActionType.Exchange_Applied_CA;
                    case "Exchange CA UnApplied": return TradeAuditActionType.ActionType.Exchange_UnApplied_CA;
                    case "Merger CA Applied": return TradeAuditActionType.ActionType.Merger_Applied_CA;
                    case "Merger CA UnApplied": return TradeAuditActionType.ActionType.Merger_UnApplied_CA;
                    case "Spinoff CA Applied": return TradeAuditActionType.ActionType.SpinOff_Applied_CA;
                    case "Spinoff CA UnApplied": return TradeAuditActionType.ActionType.SpinOff_UnApplied_CA;
                    case "Name Change CA Applied": return TradeAuditActionType.ActionType.NameChange_Applied_CA;
                    case "Name Change CA UnApplied": return TradeAuditActionType.ActionType.NameChange_UnApplied_CA;
                    case "Stock Dividend CA Applied": return TradeAuditActionType.ActionType.StockDividend_Applied_CA;
                    case "Stock Dividend CA UnApplied": return TradeAuditActionType.ActionType.StockDividend_UnApplied_CA;
                    case "Split CA Applied": return TradeAuditActionType.ActionType.Split_Applied_CA;
                    case "Split CA UnApplied": return TradeAuditActionType.ActionType.Split_UnApplied_CA;
                    case "Cash Dividend CA Applied": return TradeAuditActionType.ActionType.CashDividend_Applied_CA;
                    case "Cash Dividend CA UnApplied": return TradeAuditActionType.ActionType.CashDividend_UnApplied_CA;
                    case "Cash Transaction Dividend Applied": return TradeAuditActionType.ActionType.Dividend_Applied_CashTransaction;
                    case "Cash Transaction Dividend UnApplied": return TradeAuditActionType.ActionType.Dividend_UnApplied_CashTransaction;
                    case "Cash Settlement At Cost": return TradeAuditActionType.ActionType.CashSettlementAtCost;
                    case "Cash Settlement At Zero Price": return TradeAuditActionType.ActionType.CashSettlementAtZeroPrice;
                    case "Cash Settlement At Closing Date Spot Px": return TradeAuditActionType.ActionType.CashSettlementAtClosingDateSpotPx;
                    case "Deliver FX At Cost": return TradeAuditActionType.ActionType.DeliverFXAtCost;
                    case "Deliver FX At Cost, PNL At Closing Date Spot Px": return TradeAuditActionType.ActionType.DeliverFXAtCostandPNLAtClosingDateSpotPx;
                    case "Physical Settlement for Option": return TradeAuditActionType.ActionType.PhysicalSettlement;
                    case "Expiration for Option": return TradeAuditActionType.ActionType.Expire;
                    case "Exercise/Assignment for Option": return TradeAuditActionType.ActionType.Exercise_Assignment;
                    case "Exercise/Assignment At Zero for Option": return TradeAuditActionType.ActionType.Exercise_AssignmentatZero;
                    case "Data Unwinded for Exercised Option": return TradeAuditActionType.ActionType.Unwinding_for_Exercised_Option;
                    case "Data Unwinded for Expired Option": return TradeAuditActionType.ActionType.Unwinding_for_Expired_Option;
                    case "Data Unwinded for Physical Settled Option": return TradeAuditActionType.ActionType.Unwinding_for_PhysicalSettled_Option;
                    case "Data Unwinded for Cash Settled FX": return TradeAuditActionType.ActionType.Unwinding_for_Settlement_FX;
                    case "Cash Transaction Type Changed": return TradeAuditActionType.ActionType.CashTransaction_Type_Changed;
                    case "Cash Transaction Ex Date Changed": return TradeAuditActionType.ActionType.CashTransaction_ExDate_Changed;
                    case "Cash Transaction Payout Date Changed": return TradeAuditActionType.ActionType.CashTransaction_PayoutDate_Changed;
                    case "Cash Transaction Amount Changed": return TradeAuditActionType.ActionType.CashTransaction_Amount_Changed;
                    case "Data Closed": return TradeAuditActionType.ActionType.Closing;
                    case "Data Unwinded": return TradeAuditActionType.ActionType.Unwinding;
                    case "Booked As Swap": return TradeAuditActionType.ActionType.BookAsSwap;
                    case "Swap Details Updated": return TradeAuditActionType.ActionType.SwapDetailsUpdated;
                    case "Borrow Broker Changed": return TradeAuditActionType.ActionType.BorrowBroker_Changed;
                    case "Borrow Rate Changed": return TradeAuditActionType.ActionType.BorrowRate_Changed;
                    case "Borrow Id Changed": return TradeAuditActionType.ActionType.BorrowId_Changed;
                    case "Global Lock Added": return TradeAuditActionType.ActionType.GlobalLockAdded;
                    case "Global Lock Deleted": return TradeAuditActionType.ActionType.GlobalLockDeleted;
                    default:
                        string actionName = value.ToString().Trim();
                        if (actionName.StartsWith("Trade Attribute") && actionName.EndsWith(" Changed"))
                        {
                            string attributeNumber = actionName.Replace(" Changed", "").Replace("Trade Attribute", "");
                            string enumName = $"TradeAttribute{attributeNumber}_Changed";
                            if (Enum.TryParse(enumName, out ActionType result))
                                return result;
                        }
                        return ActionType.TradeEdited;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return TradeAuditActionType.ActionType.TradeEdited;
        }     
    }
}
