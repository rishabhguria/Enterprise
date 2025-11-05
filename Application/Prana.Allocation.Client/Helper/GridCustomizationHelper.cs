using Infragistics.Windows.DataPresenter;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ShortLocate.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Helper
{
    internal class GridCustomizationHelper
    {
        #region Allocation Group

        /// <summary>
        /// Columnses the customization for group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        /// <param name="isDefaultLayout">if set to <c>true</c> [is default layout].</param>
        /// <param name="gridName">Name of the grid.</param>
        /// <param name="resource">The resource.</param>
        internal static void ColumnsCustomizationForGroup(FieldLayout allocationGroupFieldLayout, bool isDefaultLayout, string gridName, ResourceDictionary resource)
        {
            try
            {
                SetHeaderCaptionsAllocationGroup(allocationGroupFieldLayout);
                SetEditablePropertyGroup(allocationGroupFieldLayout, gridName);
                RemoveFromColumnChooseGroup(allocationGroupFieldLayout);
                SetColumnVisibilityGroups(allocationGroupFieldLayout, gridName);
                SetColumnFormatGroup(allocationGroupFieldLayout, resource);

                //To Load DefaultLayout for Group
                if (isDefaultLayout)
                    SetDefaultLayoutForGroup(allocationGroupFieldLayout, gridName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the header captions allocation group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        private static void SetHeaderCaptionsAllocationGroup(FieldLayout allocationGroupFieldLayout)
        {
            try
            {
                ShortLocateUIPreferences _shortLocatePreferences = null;
                ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();
                _shortLocatePreferences = Dataobj.GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                Dictionary<string, string> groupFields = new Dictionary<string, string>();
                groupFields.Add(AllocationUIConstants.COUNTERPARTY_NAME, AllocationUIConstants.CAPTION_COUNTERPARTY_NAME);
                groupFields.Add(AllocationUIConstants.GROUP_ID, AllocationUIConstants.CAPTION_GROUP_ID);
                groupFields.Add(AllocationUIConstants.AUECID, AllocationUIConstants.CAPTION_AUEC_ID);
                groupFields.Add(AllocationUIConstants.IS_PRORATA_ACTIVE, AllocationUIConstants.CAPTION_IS_PRORATA_ACTIVE);
                groupFields.Add(AllocationUIConstants.IS_MANUAL_GROUP, AllocationUIConstants.CAPTION_IS_MANUAL_GROUP);
                groupFields.Add(AllocationUIConstants.ISNDF, AllocationUIConstants.CAPTION_ISNDF);
                groupFields.Add(AllocationUIConstants.FXRATE, AllocationUIConstants.CAPTION_FXRATE);
                groupFields.Add(AllocationUIConstants.ISIN_SYMBOL, AllocationUIConstants.CAPTION_ISIN_SYMBOL);
                groupFields.Add(AllocationUIConstants.OSI_SYMBOL, AllocationUIConstants.CAPTION_OSI_SYMBOL);
                groupFields.Add(AllocationUIConstants.IDCO_SYMBOL, AllocationUIConstants.CAPTION_IDCO_SYMBOL);
                groupFields.Add(AllocationUIConstants.M2M_PROFIT_LOSS, AllocationUIConstants.CAPTION_M2M_PROFIT_LOSS);
                groupFields.Add(AllocationUIConstants.SEC_FEE, AllocationUIConstants.CAPTION_SEC_FEE);
                groupFields.Add(AllocationUIConstants.OCC_FEE, AllocationUIConstants.CAPTION_OCC_FEE);
                groupFields.Add(AllocationUIConstants.ORF_FEE, AllocationUIConstants.CAPTION_ORF_FEE);
                groupFields.Add(AllocationUIConstants.UNALLOCATED_QTY, AllocationUIConstants.CAPTION_UNALLOCATED_QTY);
                groupFields.Add(AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR, AllocationUIConstants.CAPTION_FX_CONVERSION_OPERATOR);
                groupFields.Add(AllocationUIConstants.IS_SWAPPED, AllocationUIConstants.CAPTION_IS_SWAPPED);
                groupFields.Add(AllocationUIConstants.COMPANY_NAME, AllocationUIConstants.CAPTION_COMPANY_NAME);
                groupFields.Add(AllocationUIConstants.IS_PREALLOCATED, AllocationUIConstants.CAPTION_IS_PREALLOCATED);
                groupFields.Add(AllocationUIConstants.AUEC_LOCAL_DATE, AllocationUIConstants.CAPTION_AUEC_LOCAL_DATE);
                groupFields.Add(AllocationUIConstants.ORDER_SIDE, AllocationUIConstants.CAPTION_ORDER_SIDE);
                groupFields.Add(AllocationUIConstants.UNDERLYING_NAME, AllocationUIConstants.CAPTION_UNDERLYING_NAME);
                groupFields.Add(AllocationUIConstants.EXCHANGE_NAME, AllocationUIConstants.CAPTION_EXCHANGE_NAME);
                groupFields.Add(AllocationUIConstants.CURRENCY_NAME, AllocationUIConstants.CAPTION_CURRENCY_NAME);
                groupFields.Add(AllocationUIConstants.BLOOMBERG_SYMBOL, AllocationUIConstants.CAPTION_BLOOMBERG_SYMBOL);
                groupFields.Add(AllocationUIConstants.BLOOMBERG_SYMBOL_WITH_EXCHANGE_CODE, AllocationUIConstants.CAPTION_BLOOMBERG_SYMBOL_WITH_EXCHANGE_CODE);
                groupFields.Add(AllocationUIConstants.ACTIV_SYMBOL, AllocationUIConstants.CAPTION_ACTIV_SYMBOL);
                groupFields.Add(AllocationUIConstants.FACTSET_SYMBOL, AllocationUIConstants.CAPTION_FACTSET_SYMBOL);
                groupFields.Add(AllocationUIConstants.COMPANY_USER_NAME, AllocationUIConstants.CAPTION_COMPANY_USER_NAME);
                groupFields.Add(AllocationUIConstants.TRADING_ACCOUNT_NAME, AllocationUIConstants.CAPTION_TRADING_ACCOUNT_NAME);
                groupFields.Add(AllocationUIConstants.CUMQTY, AllocationUIConstants.CAPTION_CUMQTY);
                groupFields.Add(AllocationUIConstants.CLEARING_FEE, AllocationUIConstants.CAPTION_CLEARING_FEE);
                groupFields.Add(AllocationUIConstants.MISC_FEES, AllocationUIConstants.CAPTION_MISC_FEES);
                groupFields.Add(AllocationUIConstants.CLOSING_ALGO_TEXT, AllocationUIConstants.CAPTION_CLOSING_ALGO_TEXT);
                groupFields.Add(AllocationUIConstants.AVGPRICE, AllocationUIConstants.CAPTION_AVGPRICELOCAL);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION_AND_FEES, AllocationUIConstants.CAPTION_TOTALCOMMISSIONANDFEE);
                groupFields.Add(AllocationUIConstants.IS_ANOTHER_TAXLOT_ATTRIBUTES_UPDATED, AllocationUIConstants.CAPTION_ISANOTHERTAXLOTUPDATED);
                groupFields.Add(AllocationUIConstants.TRANSACTION_SOURCE, AllocationUIConstants.CAPTION_TRANSACTIONSOURCE);
                groupFields.Add(AllocationUIConstants.AVGPRICE_BASE, AllocationUIConstants.CAPTION_AVGPRICE_BASE);
                groupFields.Add(AllocationUIConstants.BorrowerID, AllocationUIConstants.CAPTION_BorrowID);
                if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                    groupFields.Add(AllocationUIConstants.ShortRebate, AllocationUIConstants.CAPTION_BorrowRateBPS);
                else
                    groupFields.Add(AllocationUIConstants.ShortRebate, AllocationUIConstants.CAPTION_BorrowRateCENT);
                groupFields.Add(AllocationUIConstants.BorrowerBroker, AllocationUIConstants.CAPTION_BorrowBroker);
                foreach (string field in groupFields.Keys)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].Label = groupFields[field];
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
        /// Sets the editable property group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        /// <param name="gridName">Name of the grid.</param>
        private static void SetEditablePropertyGroup(FieldLayout allocationGroupFieldLayout, string gridName)
        {
            try
            {
                List<string> groupFields = new List<string>();
                groupFields.Add(AllocationUIConstants.ASSET_CATEGORY);
                groupFields.Add(AllocationUIConstants.SYMBOL);
                groupFields.Add(AllocationUIConstants.ALLOCATED_QTY);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION_AND_FEES);
                groupFields.Add(AllocationUIConstants.TOTAL_QUANTITY);
                groupFields.Add(AllocationUIConstants.UNDERLYING_NAME);
                groupFields.Add(AllocationUIConstants.EXCHANGE_NAME);
                groupFields.Add(AllocationUIConstants.CURRENCY_NAME);
                groupFields.Add(AllocationUIConstants.COMPANY_USER_NAME);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION_PER_SHARE);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION);
                groupFields.Add(AllocationUIConstants.CONTRACT_MULTIPLIER);
                groupFields.Add(AllocationUIConstants.COMPANY_NAME);
                groupFields.Add(AllocationUIConstants.UNDERLYING_SYMBOL);
                groupFields.Add(AllocationUIConstants.BLOOMBERG_SYMBOL);
                groupFields.Add(AllocationUIConstants.BLOOMBERG_SYMBOL_WITH_EXCHANGE_CODE);
                groupFields.Add(AllocationUIConstants.SEDOL_SYMBOL);
                groupFields.Add(AllocationUIConstants.CUSIP_SYMBOL);
                groupFields.Add(AllocationUIConstants.OSI_SYMBOL);
                groupFields.Add(AllocationUIConstants.IDCO_SYMBOL);
                groupFields.Add(AllocationUIConstants.CLOSING_ALGO_TEXT);
                groupFields.Add(AllocationUIConstants.NAV_LOCK_STATUS);
                groupFields.Add(AllocationUIConstants.CHANGE_TYPE);
                groupFields.Add(AllocationUIConstants.GROUP_ID);
                groupFields.Add(AllocationUIConstants.CLOSING_STATUS);
                groupFields.Add(AllocationUIConstants.STRATEGY_NAME);
                groupFields.Add(AllocationUIConstants.ACCOUNT_NAME);
                groupFields.Add(AllocationUIConstants.MASTER_FUND_NAME);
                groupFields.Add(AllocationUIConstants.EXPIRATION_DATE);
                groupFields.Add(AllocationUIConstants.IS_PREALLOCATED);
                groupFields.Add(AllocationUIConstants.IS_MANUAL_GROUP);
                groupFields.Add(AllocationUIConstants.ISNDF);
                groupFields.Add(AllocationUIConstants.IS_ANOTHER_TAXLOT_ATTRIBUTES_UPDATED);
                groupFields.Add(AllocationUIConstants.ALLOCATION_SCHEME_NAME);
                groupFields.Add(AllocationUIConstants.ORDER_TYPE);
                groupFields.Add(AllocationUIConstants.PRANA_MSG_TYPE);

                groupFields.Add(AllocationUIConstants.TRADING_ACCOUNT_NAME);
                groupFields.Add(AllocationUIConstants.GROUP_STATUS);
                groupFields.Add(AllocationUIConstants.ACCRUAL_BASIS);
                groupFields.Add(AllocationUIConstants.BOND_TYPE);
                groupFields.Add(AllocationUIConstants.PUTCALL);
                groupFields.Add(AllocationUIConstants.AUECID);
                groupFields.Add(AllocationUIConstants.M2M_PROFIT_LOSS);
                groupFields.Add(AllocationUIConstants.PROXY_SYMBOL);
                groupFields.Add(AllocationUIConstants.UNDERLYING_DELTA);
                groupFields.Add(AllocationUIConstants.PUT_OR_CALL);
                groupFields.Add(AllocationUIConstants.TRANSACTION_SOURCE);
                groupFields.Add(AllocationUIConstants.COUPON_RATE);

                foreach (string field in groupFields)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].AllowEdit = false;
                }
                if (gridName.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.QUANTITY) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.QUANTITY].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.CUMQTY) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.CUMQTY].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.UNALLOCATED_QTY) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.UNALLOCATED_QTY].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.AUEC_LOCAL_DATE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.AUEC_LOCAL_DATE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.PROCESS_DATE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.PROCESS_DATE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ORIGINAL_PURCHASE_DATE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ORIGINAL_PURCHASE_DATE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.SETTLEMENT_DATE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.SETTLEMENT_DATE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ORDER_SIDE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ORDER_SIDE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.VENUE) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.VENUE].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.INTERNAL_COMMENTS) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.INTERNAL_COMMENTS].AllowEdit = false;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ISIN_SYMBOL) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ISIN_SYMBOL].AllowEdit = false;
                }
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.TradeAttribute6) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.TradeAttribute6].AllowEdit = false;
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
        /// Removes from column choose group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        private static void RemoveFromColumnChooseGroup(FieldLayout allocationGroupFieldLayout)
        {
            try
            {
                List<string> groupRemovedFields = new List<string>();
                groupRemovedFields.Add(AllocationUIConstants.ACCOUNT_ID_COLUMN);
                groupRemovedFields.Add(AllocationUIConstants.ALLOCATION_SCHEME_ID);
                groupRemovedFields.Add(AllocationUIConstants.ALLOCATIONS);
                groupRemovedFields.Add(AllocationUIConstants.AUTO_GROUPED);
                groupRemovedFields.Add(AllocationUIConstants.ASSET_ID);
                groupRemovedFields.Add(AllocationUIConstants.CURRENCY_ID);
                groupRemovedFields.Add(AllocationUIConstants.EXCHANGE_ID);
                groupRemovedFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID);
                groupRemovedFields.Add(AllocationUIConstants.COMPANY_USER_ID);
                groupRemovedFields.Add(AllocationUIConstants.INT_PRANA_MSG_TYPE);
                groupRemovedFields.Add(AllocationUIConstants.IS_GROUP_ALLOCATED_TO_ONE_TAXLOT);
                groupRemovedFields.Add(AllocationUIConstants.IS_CAL_COUNTERPARTY_COMMISSION);
                groupRemovedFields.Add(AllocationUIConstants.IS_COMMISSION_CALCULATED);
                groupRemovedFields.Add(AllocationUIConstants.IS_SOFT_COMMISSION_CHANGED);
                groupRemovedFields.Add(AllocationUIConstants.IS_COMMISSION_CHANGED);
                groupRemovedFields.Add(AllocationUIConstants.COMMISSION_SOURCE);
                groupRemovedFields.Add(AllocationUIConstants.SOFT_COMMISSION_SOURCE);
                groupRemovedFields.Add(AllocationUIConstants.IS_CURRENCY_FUTURE);
                groupRemovedFields.Add(AllocationUIConstants.LEAD_CURRENCY_ID);
                groupRemovedFields.Add(AllocationUIConstants.STATE_ID);
                groupRemovedFields.Add(AllocationUIConstants.STRATEGY_ID);
                groupRemovedFields.Add(AllocationUIConstants.TRADING_ACCOUNT_ID);
                groupRemovedFields.Add(AllocationUIConstants.UNDERLYING_ID);
                groupRemovedFields.Add(AllocationUIConstants.USER_ID);
                groupRemovedFields.Add(AllocationUIConstants.VENUE_ID);
                groupRemovedFields.Add(AllocationUIConstants.VS_CURRENCY_ID);
                groupRemovedFields.Add(AllocationUIConstants.LOT_ID);
                groupRemovedFields.Add(AllocationUIConstants.SETTLEMENT_CURRENCYID);
                groupRemovedFields.Add(AllocationUIConstants.COUNTERPARTY_ID);
                groupRemovedFields.Add(AllocationUIConstants.FIRST_COUPON_DATE);
                groupRemovedFields.Add(AllocationUIConstants.FIXING_DATE);
                groupRemovedFields.Add(AllocationUIConstants.FOREX_REQ);
                groupRemovedFields.Add(AllocationUIConstants.FREQ);
                groupRemovedFields.Add(AllocationUIConstants.GROUP_ALLOCATION_STATUS);
                groupRemovedFields.Add(AllocationUIConstants.IS_ZERO);
                groupRemovedFields.Add(AllocationUIConstants.IS_PRORATA_ACTIVE);
                groupRemovedFields.Add(AllocationUIConstants.ISSUE_DATE);
                groupRemovedFields.Add(AllocationUIConstants.IS_SWAPPED);
                groupRemovedFields.Add(AllocationUIConstants.LIST_ID);
                groupRemovedFields.Add(AllocationUIConstants.MATURITY_DATE);
                groupRemovedFields.Add(AllocationUIConstants.NOT_ALL_EXECUTED);
                groupRemovedFields.Add(AllocationUIConstants.NOTIONAL_CHANGE);
                groupRemovedFields.Add(AllocationUIConstants.ORDER_COUNT);
                groupRemovedFields.Add(AllocationUIConstants.ORDER_TYPE_TAG_VALUE);
                groupRemovedFields.Add(AllocationUIConstants.ORDERSIDE_TAGVALUE);
                groupRemovedFields.Add(AllocationUIConstants.OTHER_CHK_BOX);
                groupRemovedFields.Add(AllocationUIConstants.PARENT_TAXLOT_PK);
                groupRemovedFields.Add(AllocationUIConstants.PERSISTENCE_STATUS);
                groupRemovedFields.Add(AllocationUIConstants.POSITION_TAG_VALUE);
                groupRemovedFields.Add(AllocationUIConstants.PUT_CALL);
                //groupRemovedFields.Add(AllocationUIConstants.PUT_OR_CALL);
                groupRemovedFields.Add(AllocationUIConstants.REUTERS_SYMBOL);
                groupRemovedFields.Add(AllocationUIConstants.ROUNDLOT);
                groupRemovedFields.Add(AllocationUIConstants.STATE);
                groupRemovedFields.Add(AllocationUIConstants.STRIKE_PRICE);
                groupRemovedFields.Add(AllocationUIConstants.SWAP_PARAMETERS);
                groupRemovedFields.Add(AllocationUIConstants.TAXLOT_CLOSING_ID);
                groupRemovedFields.Add(AllocationUIConstants.TAXLOT_IDS_WITH_ATTRIBUTES);
                groupRemovedFields.Add(AllocationUIConstants.UPDATED);
                groupRemovedFields.Add(AllocationUIConstants.WORK_FLOW_STATE);
                groupRemovedFields.Add(AllocationUIConstants.COMMISSION_TEXT);
                groupRemovedFields.Add(AllocationUIConstants.CORP_ACTION_ID);
                groupRemovedFields.Add(AllocationUIConstants.COMMISION_CALCULATION_TIME);
                groupRemovedFields.Add(AllocationUIConstants.ALLOCATED_EQUAL_TOTAL_QTY);
                groupRemovedFields.Add(AllocationUIConstants.ASSET_NAME);
                groupRemovedFields.Add(AllocationUIConstants.COMM_SOURCE);
                groupRemovedFields.Add(AllocationUIConstants.SOFT_COMM_SOURCE);
                groupRemovedFields.Add(AllocationUIConstants.IS_MODIFIED);
                groupRemovedFields.Add(AllocationUIConstants.IS_RECALCULATE_COMMISSION);
                groupRemovedFields.Add(AllocationUIConstants.COMMISSION_AMT);
                groupRemovedFields.Add(AllocationUIConstants.SOFT_COMMISSION_AMT);
                groupRemovedFields.Add(AllocationUIConstants.COMMISSION_RATE);
                groupRemovedFields.Add(AllocationUIConstants.SOFT_COMMISSION_RATE);
                groupRemovedFields.Add(AllocationUIConstants.CALCBASIS);
                groupRemovedFields.Add(AllocationUIConstants.SOFT_COMMISSION_CALCBASIS);
                groupRemovedFields.Add(AllocationUIConstants.COUPON);
                groupRemovedFields.Add(AllocationUIConstants.ALLOCATION_DATE);
                groupRemovedFields.Add(AllocationUIConstants.TRANSACTION_SOURCE_TAG);
                groupRemovedFields.Add(AllocationUIConstants.ERROR_MESSAGE);
                groupRemovedFields.Add(AllocationUIConstants.IS_SELECTED);
                groupRemovedFields.Add(AllocationUIConstants.OriginalAllocationPreferenceID);
                groupRemovedFields.Add(AllocationUIConstants.NAV_LOCK_STATUS);
                groupRemovedFields.Add(AllocationUIConstants.ORIGINAL_CUM_QTY);
                groupRemovedFields.Add(AllocationUIConstants.CUM_QTY_FORLIVEORDER);
                groupRemovedFields.Add(AllocationUIConstants.IS_STAGE_REQUIRED);
                groupRemovedFields.Add(AllocationUIConstants.IS_MANUAL_ORDER);
                groupRemovedFields.Add(AllocationUIConstants.CUM_QTY_FOR_SUB_ORDER);
                groupRemovedFields.Add(AllocationUIConstants.ModifiedUserId);
                groupRemovedFields.Add(AllocationUIConstants.ExecutionTimeLastFill);
                groupRemovedFields.Add(AllocationUIConstants.AVG_PRICE_FOR_COMPLIANCE);
                groupRemovedFields.Add(AllocationUIConstants.IsOverbuyOversellAccepted);
                groupRemovedFields.Add(AllocationUIConstants.Is_Samsara_User);
                groupRemovedFields.Add(AllocationUIConstants.IS_MULTIBROKER_TRADE);

                foreach (string field in groupRemovedFields)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].AllowHiding = AllowFieldHiding.Never;
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
        /// Sets the column visibility groups.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        /// <param name="gridName">Name of the grid.</param>
        private static void SetColumnVisibilityGroups(FieldLayout allocationGroupFieldLayout, string gridName)
        {
            try
            {
                List<string> groupCollapsedFields = new List<string>();
                groupCollapsedFields.Add(AllocationUIConstants.ACCOUNT_ID_COLUMN);
                groupCollapsedFields.Add(AllocationUIConstants.ALLOCATION_SCHEME_ID);
                groupCollapsedFields.Add(AllocationUIConstants.ALLOCATIONS);
                groupCollapsedFields.Add(AllocationUIConstants.AUTO_GROUPED);
                groupCollapsedFields.Add(AllocationUIConstants.ASSET_ID);
                groupCollapsedFields.Add(AllocationUIConstants.CURRENCY_ID);
                groupCollapsedFields.Add(AllocationUIConstants.EXCHANGE_ID);
                groupCollapsedFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID);
                groupCollapsedFields.Add(AllocationUIConstants.COMPANY_USER_ID);
                groupCollapsedFields.Add(AllocationUIConstants.INT_PRANA_MSG_TYPE);
                groupCollapsedFields.Add(AllocationUIConstants.IS_GROUP_ALLOCATED_TO_ONE_TAXLOT);
                groupCollapsedFields.Add(AllocationUIConstants.IS_CAL_COUNTERPARTY_COMMISSION);
                groupCollapsedFields.Add(AllocationUIConstants.IS_COMMISSION_CALCULATED);
                groupCollapsedFields.Add(AllocationUIConstants.IS_SOFT_COMMISSION_CHANGED);
                groupCollapsedFields.Add(AllocationUIConstants.IS_COMMISSION_CHANGED);
                groupCollapsedFields.Add(AllocationUIConstants.COMMISSION_SOURCE);
                groupCollapsedFields.Add(AllocationUIConstants.SOFT_COMMISSION_SOURCE);
                groupCollapsedFields.Add(AllocationUIConstants.IS_CURRENCY_FUTURE);
                groupCollapsedFields.Add(AllocationUIConstants.LEAD_CURRENCY_ID);
                groupCollapsedFields.Add(AllocationUIConstants.STATE_ID);
                groupCollapsedFields.Add(AllocationUIConstants.STRATEGY_ID);
                groupCollapsedFields.Add(AllocationUIConstants.TRADING_ACCOUNT_ID);
                groupCollapsedFields.Add(AllocationUIConstants.UNDERLYING_ID);
                groupCollapsedFields.Add(AllocationUIConstants.USER_ID);
                groupCollapsedFields.Add(AllocationUIConstants.VENUE_ID);
                groupCollapsedFields.Add(AllocationUIConstants.VS_CURRENCY_ID);
                groupCollapsedFields.Add(AllocationUIConstants.LOT_ID);
                groupCollapsedFields.Add(AllocationUIConstants.SETTLEMENT_CURRENCYID);
                groupCollapsedFields.Add(AllocationUIConstants.COUNTERPARTY_ID);
                groupCollapsedFields.Add(AllocationUIConstants.FIRST_COUPON_DATE);
                groupCollapsedFields.Add(AllocationUIConstants.FIXING_DATE);
                groupCollapsedFields.Add(AllocationUIConstants.FOREX_REQ);
                groupCollapsedFields.Add(AllocationUIConstants.FREQ);
                groupCollapsedFields.Add(AllocationUIConstants.GROUP_ALLOCATION_STATUS);
                groupCollapsedFields.Add(AllocationUIConstants.IS_ZERO);
                groupCollapsedFields.Add(AllocationUIConstants.IS_PRORATA_ACTIVE);
                groupCollapsedFields.Add(AllocationUIConstants.ISSUE_DATE);
                groupCollapsedFields.Add(AllocationUIConstants.IS_SWAPPED);
                groupCollapsedFields.Add(AllocationUIConstants.LIST_ID);
                groupCollapsedFields.Add(AllocationUIConstants.MATURITY_DATE);
                groupCollapsedFields.Add(AllocationUIConstants.NOT_ALL_EXECUTED);
                groupCollapsedFields.Add(AllocationUIConstants.NOTIONAL_CHANGE);
                groupCollapsedFields.Add(AllocationUIConstants.ORDER_COUNT);
                groupCollapsedFields.Add(AllocationUIConstants.ORDER_TYPE_TAG_VALUE);
                groupCollapsedFields.Add(AllocationUIConstants.ORDERSIDE_TAGVALUE);
                groupCollapsedFields.Add(AllocationUIConstants.OTHER_CHK_BOX);
                groupCollapsedFields.Add(AllocationUIConstants.PARENT_TAXLOT_PK);
                groupCollapsedFields.Add(AllocationUIConstants.PERSISTENCE_STATUS);
                groupCollapsedFields.Add(AllocationUIConstants.POSITION_TAG_VALUE);
                groupCollapsedFields.Add(AllocationUIConstants.PUT_CALL);
                //groupCollapsedFields.Add(AllocationUIConstants.PUT_OR_CALL);
                groupCollapsedFields.Add(AllocationUIConstants.REUTERS_SYMBOL);
                groupCollapsedFields.Add(AllocationUIConstants.ROUNDLOT);
                groupCollapsedFields.Add(AllocationUIConstants.STATE);
                groupCollapsedFields.Add(AllocationUIConstants.STRIKE_PRICE);
                groupCollapsedFields.Add(AllocationUIConstants.SWAP_PARAMETERS);
                groupCollapsedFields.Add(AllocationUIConstants.TAXLOT_CLOSING_ID);
                groupCollapsedFields.Add(AllocationUIConstants.TAXLOT_IDS_WITH_ATTRIBUTES);
                groupCollapsedFields.Add(AllocationUIConstants.UPDATED);
                groupCollapsedFields.Add(AllocationUIConstants.WORK_FLOW_STATE);
                groupCollapsedFields.Add(AllocationUIConstants.COMMISSION_TEXT);
                groupCollapsedFields.Add(AllocationUIConstants.CORP_ACTION_ID);
                groupCollapsedFields.Add(AllocationUIConstants.COMMISION_CALCULATION_TIME);
                groupCollapsedFields.Add(AllocationUIConstants.ALLOCATED_EQUAL_TOTAL_QTY);
                groupCollapsedFields.Add(AllocationUIConstants.ASSET_NAME);
                groupCollapsedFields.Add(AllocationUIConstants.COMM_SOURCE);
                groupCollapsedFields.Add(AllocationUIConstants.SOFT_COMM_SOURCE);
                groupCollapsedFields.Add(AllocationUIConstants.IS_MODIFIED);
                groupCollapsedFields.Add(AllocationUIConstants.IS_RECALCULATE_COMMISSION);
                groupCollapsedFields.Add(AllocationUIConstants.COMMISSION_AMT);
                groupCollapsedFields.Add(AllocationUIConstants.SOFT_COMMISSION_AMT);
                groupCollapsedFields.Add(AllocationUIConstants.COMMISSION_RATE);
                groupCollapsedFields.Add(AllocationUIConstants.SOFT_COMMISSION_RATE);
                groupCollapsedFields.Add(AllocationUIConstants.CALCBASIS);
                groupCollapsedFields.Add(AllocationUIConstants.SOFT_COMMISSION_CALCBASIS);
                groupCollapsedFields.Add(AllocationUIConstants.COUPON);
                groupCollapsedFields.Add(AllocationUIConstants.ALLOCATION_DATE);
                groupCollapsedFields.Add(AllocationUIConstants.TRANSACTION_SOURCE_TAG);
                groupCollapsedFields.Add(AllocationUIConstants.ERROR_MESSAGE);
                groupCollapsedFields.Add(AllocationUIConstants.IS_SELECTED);
                groupCollapsedFields.Add(AllocationUIConstants.OriginalAllocationPreferenceID);
                groupCollapsedFields.Add(AllocationUIConstants.NAV_LOCK_STATUS);
                groupCollapsedFields.Add(AllocationUIConstants.IS_STAGE_REQUIRED);
                groupCollapsedFields.Add(AllocationUIConstants.IS_MANUAL_ORDER);
                groupCollapsedFields.Add(AllocationUIConstants.ExecutionTimeLastFill);
                groupCollapsedFields.Add(AllocationUIConstants.IsOverbuyOversellAccepted);

                foreach (string field in groupCollapsedFields)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].Visibility = Visibility.Collapsed;
                }
                if (gridName.Equals(AllocationClientConstants.CONST_GIRD_UNALLOCATED))
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.TAXLOTS) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.TAXLOTS].Visibility = Visibility.Collapsed;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ModifiedUserId) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ModifiedUserId].Visibility = Visibility.Collapsed;
                }
                if (gridName.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ORDERS) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ORDERS].Visibility = Visibility.Collapsed;
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ModifiedUserId) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ModifiedUserId].Visibility = Visibility.Collapsed;
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
        /// Sets the column format group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        /// <param name="resource">The resource.</param>
        internal static void SetColumnFormatGroup(FieldLayout allocationGroupFieldLayout, ResourceDictionary resource)
        {
            try
            {
                List<string> groupFields = new List<string>();
                List<string> groupQtyFields = new List<string>();
                groupFields.Add(AllocationUIConstants.COMMISSION);
                groupFields.Add(AllocationUIConstants.SOFT_COMMISSION);
                groupFields.Add(AllocationUIConstants.ORF_FEE);
                groupFields.Add(AllocationUIConstants.SEC_FEE);
                groupFields.Add(AllocationUIConstants.OCC_FEE);
                groupFields.Add(AllocationUIConstants.MISC_FEES);
                groupFields.Add(AllocationUIConstants.CLEARING_FEE);
                groupFields.Add(AllocationUIConstants.STAMP_DUTY);
                groupFields.Add(AllocationUIConstants.CLEARING_BROKER_FEE);
                groupFields.Add(AllocationUIConstants.OTHER_BROKER_FEES);
                groupFields.Add(AllocationUIConstants.TAX_ON_COMMISSIONS);
                groupFields.Add(AllocationUIConstants.TRANSACTION_LEVY);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION_AND_FEES);
                groupFields.Add(AllocationUIConstants.TOTAL_COMMISSION);
                groupFields.Add(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT);

                groupQtyFields.Add(AllocationUIConstants.ALLOCATED_QTY);
                groupQtyFields.Add(AllocationUIConstants.CUMQTY);
                groupQtyFields.Add(AllocationUIConstants.UNALLOCATED_QTY);
                groupQtyFields.Add(AllocationUIConstants.TOTAL_QUANTITY);

                foreach (string field in groupFields)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].Settings.EditorStyle = resource["CommissionColumnPrecision"] as Style;
                }
                foreach (string field in groupQtyFields)
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(field) != -1)
                        allocationGroupFieldLayout.Fields[field].Settings.EditorStyle = resource["QtyColumnPrecision"] as Style;
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
        /// Sets the default layout for group.
        /// </summary>
        /// <param name="allocationGroupFieldLayout">The allocation group field layout.</param>
        /// <param name="gridName">Name of the grid.</param>
        private static void SetDefaultLayoutForGroup(FieldLayout allocationGroupFieldLayout, string gridName)
        {
            try
            {
                foreach (Field field in allocationGroupFieldLayout.Fields)
                    field.Visibility = Visibility.Collapsed;

                List<string> allocatedGroups = new List<string>();
                List<string> unAllocatedGroups = new List<string>();

                allocatedGroups.Add(AllocationUIConstants.AUEC_LOCAL_DATE);
                allocatedGroups.Add(AllocationUIConstants.SYMBOL);
                allocatedGroups.Add(AllocationUIConstants.ORDER_SIDE);
                allocatedGroups.Add(AllocationUIConstants.CUMQTY);
                allocatedGroups.Add(AllocationUIConstants.AVGPRICE);
                allocatedGroups.Add(AllocationUIConstants.NETAMOUNT_LOCAL);
                allocatedGroups.Add(AllocationUIConstants.NETAMOUNT_BASE);
                allocatedGroups.Add(AllocationUIConstants.TOTAL_COMMISSION_AND_FEES);
                allocatedGroups.Add(AllocationUIConstants.OPTION_PREMIUM_ADJUSTMENT);
                allocatedGroups.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL);
                allocatedGroups.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_BASE);
                allocatedGroups.Add(AllocationUIConstants.FXRATE);
                allocatedGroups.Add(AllocationUIConstants.SETTLEMENT_CURRENCY);
                allocatedGroups.Add(AllocationUIConstants.COUNTERPARTY_NAME);
                allocatedGroups.Add(AllocationUIConstants.COMPANY_USER_NAME);
                allocatedGroups.Add(AllocationUIConstants.ASSET_CATEGORY);
                allocatedGroups.Add(AllocationUIConstants.CHANGE_TYPE);

                unAllocatedGroups.Add(AllocationUIConstants.AUEC_LOCAL_DATE);
                unAllocatedGroups.Add(AllocationUIConstants.SYMBOL);
                unAllocatedGroups.Add(AllocationUIConstants.ORDER_SIDE);
                unAllocatedGroups.Add(AllocationUIConstants.TRANSACTION_TYPE);
                unAllocatedGroups.Add(AllocationUIConstants.TOTAL_QUANTITY);
                unAllocatedGroups.Add(AllocationUIConstants.CUMQTY);
                unAllocatedGroups.Add(AllocationUIConstants.TRADING_ACCOUNT_NAME);
                unAllocatedGroups.Add(AllocationUIConstants.AVGPRICE);
                unAllocatedGroups.Add(AllocationUIConstants.AVGPRICE_BASE);
                unAllocatedGroups.Add(AllocationUIConstants.FXRATE);
                unAllocatedGroups.Add(AllocationUIConstants.COMPANY_USER_NAME);
                unAllocatedGroups.Add(AllocationUIConstants.ASSET_CATEGORY);
                unAllocatedGroups.Add(AllocationUIConstants.COMMISSION_PER_SHARE);
                unAllocatedGroups.Add(AllocationUIConstants.SOFT_COMMISSION_PER_SHARE);
                unAllocatedGroups.Add(AllocationUIConstants.TOTAL_COMMISSION_PER_SHARE);
                unAllocatedGroups.Add(AllocationUIConstants.CHANGE_TYPE);

                if (gridName.Equals(AllocationClientConstants.CONST_GIRD_UNALLOCATED))
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.ORDERS) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.ORDERS].Visibility = Visibility.Visible;

                    for (int i = 0; i < unAllocatedGroups.Count; i++)
                    {
                        if (allocationGroupFieldLayout.Fields.IndexOf(unAllocatedGroups[i]) != -1)
                        {
                            allocationGroupFieldLayout.Fields[unAllocatedGroups[i]].Visibility = Visibility.Visible;
                            allocationGroupFieldLayout.Fields[unAllocatedGroups[i]].ActualPosition = new FieldPosition(i, 0, 0, 0);
                        }
                    }
                }
                if (gridName.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                {
                    if (allocationGroupFieldLayout.Fields.IndexOf(AllocationUIConstants.TAXLOTS) != -1)
                        allocationGroupFieldLayout.Fields[AllocationUIConstants.TAXLOTS].Visibility = Visibility.Visible;

                    for (int i = 0; i < allocatedGroups.Count; i++)
                    {
                        if (allocationGroupFieldLayout.Fields.IndexOf(allocatedGroups[i]) != -1)
                        {
                            allocationGroupFieldLayout.Fields[allocatedGroups[i]].Visibility = Visibility.Visible;
                            allocationGroupFieldLayout.Fields[allocatedGroups[i]].ActualPosition = new FieldPosition(i, 0, 0, 0);
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
        }

        /// <summary>
        /// Adds the unbound columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        internal static void AddUnboundColumns(XamDataGrid dataGrid)
        {
            try
            {
                if (dataGrid.FieldLayouts.Any(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME)))
                {
                    FieldLayout fieldLayout = dataGrid.FieldLayouts.FirstOrDefault(x => x.Description.ToString().Equals(AllocationUIConstants.ALLOCATION_GROUP_FIELD_LAYOUT_NAME));

                    if (!fieldLayout.Fields.Any(x => x.Name.Equals(AllocationUIConstants.ASSET_CATEGORY)))
                    {
                        Field field = new Field(AllocationUIConstants.ASSET_CATEGORY);
                        field.BindingType = BindingType.Unbound;
                        field.AllowEdit = false;
                        field.Label = AllocationUIConstants.CAPTION_ASSET_CATEGORY;
                        fieldLayout.Fields.Add(field);
                    }

                    if (!fieldLayout.Fields.Any(x => x.Name.Equals(AllocationUIConstants.IMPORT_FILE_NAME)))
                    {
                        Field field = new Field(AllocationUIConstants.IMPORT_FILE_NAME);
                        field.BindingType = BindingType.Unbound;
                        field.AllowEdit = false;
                        field.Label = AllocationUIConstants.CAPTION_IMPORT_FILE_NAME;
                        fieldLayout.Fields.Add(field);
                    }

                    if (!fieldLayout.Fields.Any(x => x.Name.Equals(AllocationUIConstants.MASTER_FUND_NAME)))
                    {
                        Field field = new Field(AllocationUIConstants.MASTER_FUND_NAME);
                        field.BindingType = BindingType.Unbound;
                        field.AllowEdit = false;
                        field.Label = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? AllocationUIConstants.CAPTION_CLIENT_NAME : AllocationUIConstants.CAPTION_MASTER_FUND;
                        fieldLayout.Fields.Add(field);
                    }

                    if (dataGrid.Name.Equals(AllocationClientConstants.CONST_GIRD_ALLOCATED))
                    {
                        if (!fieldLayout.Fields.Any(x => x.Name.Equals(AllocationUIConstants.ACCOUNT_NAME)))
                        {
                            Field field = new Field(AllocationUIConstants.ACCOUNT_NAME);
                            field.BindingType = BindingType.Unbound;
                            field.AllowEdit = false;
                            field.Label = AllocationUIConstants.CAPTION_ACCOUNT_NAME;
                            fieldLayout.Fields.Add(field);
                        }

                        if (!fieldLayout.Fields.Any(x => x.Name.Equals(AllocationUIConstants.STRATEGY_NAME)))
                        {
                            Field field = new Field(AllocationUIConstants.STRATEGY_NAME);
                            field.BindingType = BindingType.Unbound;
                            field.AllowEdit = false;
                            field.Label = AllocationUIConstants.CAPTION_STRATEGY_NAME;
                            fieldLayout.Fields.Add(field);
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
        }

        #endregion

        #region Taxlot

        /// <summary>
        /// Columnses the customization for taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        /// <param name="defaultLayout">if set to <c>true</c> [default layout].</param>
        /// <param name="resource">The resource.</param>
        internal static void ColumnsCustomizationForTaxlots(FieldLayout taxlotFieldLayout, bool defaultLayout, ResourceDictionary resource)
        {
            try
            {
                SetHeaderCaptionForTaxlots(taxlotFieldLayout);
                SetEditablePropertyTaxlots(taxlotFieldLayout, resource);
                SetVisibilityForTaxlots(taxlotFieldLayout);
                SetColumnFormatForTaxlots(taxlotFieldLayout, resource);

                //To Load DefaultLayout for Taxlot
                if (defaultLayout)
                    SetDefaultLayoutForTaxlots(taxlotFieldLayout);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the header caption for taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        private static void SetHeaderCaptionForTaxlots(FieldLayout taxlotFieldLayout)
        {
            try
            {
                Dictionary<string, string> taxlotFields = new Dictionary<string, string>();
                taxlotFields.Add(AllocationUIConstants.LEVEL1_NAME, AllocationUIConstants.CAPTION_ACCOUNT_NAME);
                taxlotFields.Add(AllocationUIConstants.LEVEL2_NAME, AllocationUIConstants.CAPTION_STRATEGY_NAME);
                taxlotFields.Add(AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR, AllocationUIConstants.CAPTION_FX_CONVERSION_OPERATOR);
                taxlotFields.Add(AllocationUIConstants.CLEARING_FEE, AllocationUIConstants.CAPTION_CLEARING_FEE);
                taxlotFields.Add(AllocationUIConstants.MISC_FEES, AllocationUIConstants.CAPTION_MISC_FEES);
                taxlotFields.Add(AllocationUIConstants.SEC_FEE, AllocationUIConstants.CAPTION_SEC_FEE);
                taxlotFields.Add(AllocationUIConstants.OCC_FEE, AllocationUIConstants.CAPTION_OCC_FEE);
                taxlotFields.Add(AllocationUIConstants.ORF_FEE, AllocationUIConstants.CAPTION_ORF_FEE);
                taxlotFields.Add(AllocationUIConstants.CLOSING_ALGO_TEXT, AllocationUIConstants.CAPTION_CLOSING_ALGO_TEXT);
                taxlotFields.Add(AllocationUIConstants.LOT_ID, AllocationUIConstants.CAPTION_LOT_ID);
                taxlotFields.Add(AllocationUIConstants.SETTLEMENT_CURRENCY, AllocationUIConstants.CAPTION_SETTLEMENT_CURRENCY);
                taxlotFields.Add(AllocationUIConstants.CLEARING_BROKER_FEE, AllocationUIConstants.CAPTION_CLEARING_BROKER_FEE);
                taxlotFields.Add(AllocationUIConstants.OTHER_BROKER_FEES, AllocationUIConstants.CAPTION_OTHER_BROKER_FEES);
                taxlotFields.Add(AllocationUIConstants.STAMP_DUTY, AllocationUIConstants.CAPTION_STAMP_DUTY);
                taxlotFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID, AllocationUIConstants.CAPTION_EXTERNAL_TRANS_ID);
                taxlotFields.Add(AllocationUIConstants.TAXLOT_QTY, AllocationUIConstants.CAPTION_TAXLOT_QTY);
                taxlotFields.Add(AllocationUIConstants.ACCRUED_INTEREST, AllocationUIConstants.CAPTION_ACCRUED_INTEREST);
                taxlotFields.Add(AllocationUIConstants.CHANGE_TYPE, AllocationUIConstants.CAPTION_CHANGE_TYPE);
                taxlotFields.Add(AllocationUIConstants.CLOSING_STATUS, AllocationUIConstants.CAPTION_CLOSING_STATUS);
                taxlotFields.Add(AllocationUIConstants.OPTION_PREMIUM_ADJUSTMENT, AllocationUIConstants.CAPTION_OPTION_PREMIUM_ADJUSTMENT);
                taxlotFields.Add(AllocationUIConstants.SOFT_COMMISSION, AllocationUIConstants.CAPTION_SOFT_COMMISSION);
                taxlotFields.Add(AllocationUIConstants.TAX_ON_COMMISSIONS, AllocationUIConstants.CAPTION_TAX_ON_COMMISSIONS);
                taxlotFields.Add(AllocationUIConstants.TRANSACTION_LEVY, AllocationUIConstants.CAPTION_TRANSACTION_LEVY);

                foreach (string field in taxlotFields.Keys)
                {
                    if (taxlotFieldLayout.Fields.IndexOf(field) != -1)
                        taxlotFieldLayout.Fields[field].Label = taxlotFields[field];
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
        /// Sets the editable property taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        /// <param name="resource">The resource.</param>
        private static void SetEditablePropertyTaxlots(FieldLayout taxlotFieldLayout, ResourceDictionary resource)
        {
            try
            {
                List<string> taxlotFields = new List<string>();
                taxlotFields.Add(AllocationUIConstants.LEVEL1_NAME);
                taxlotFields.Add(AllocationUIConstants.LEVEL2_NAME);
                taxlotFields.Add(AllocationUIConstants.CAPTION_CLOSING_ALGO_TEXT);
                taxlotFields.Add(AllocationUIConstants.CHANGE_TYPE);
                taxlotFields.Add(AllocationUIConstants.TAXLOT_PERCENTAGE);
                taxlotFields.Add(AllocationUIConstants.TAXLOT_QTY);
                taxlotFields.Add(AllocationUIConstants.TAXLOT_ID);
                taxlotFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID);
                taxlotFields.Add(AllocationUIConstants.CLOSING_STATUS);

                foreach (string field in taxlotFields)
                {
                    if (taxlotFieldLayout.Fields.IndexOf(field) != -1)
                        taxlotFieldLayout.Fields[field].AllowEdit = false;
                }
                taxlotFieldLayout.Settings.HeaderPrefixAreaDisplayMode = HeaderPrefixAreaDisplayMode.None;
                taxlotFieldLayout.Settings.HeaderPrefixAreaStyle = resource["HideHeaderPrefixArea"] as Style;
                taxlotFieldLayout.Settings.RecordSelectorStyle = resource["HideRecordSelector"] as Style;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the visibility for taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        private static void SetVisibilityForTaxlots(FieldLayout taxlotFieldLayout)
        {
            try
            {
                List<string> taxlotVisibleFields = new List<string>();
                taxlotVisibleFields.Add(AllocationUIConstants.LEVEL1_NAME);
                taxlotVisibleFields.Add(AllocationUIConstants.LEVEL2_NAME);
                taxlotVisibleFields.Add(AllocationUIConstants.TAXLOT_ID);
                taxlotVisibleFields.Add(AllocationUIConstants.TAXLOT_QTY);
                taxlotVisibleFields.Add(AllocationUIConstants.COMMISSION);
                taxlotVisibleFields.Add(AllocationUIConstants.SOFT_COMMISSION);
                taxlotVisibleFields.Add(AllocationUIConstants.FXRATE);
                taxlotVisibleFields.Add(AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR);
                taxlotVisibleFields.Add(AllocationUIConstants.OTHER_BROKER_FEES);
                taxlotVisibleFields.Add(AllocationUIConstants.CLEARING_BROKER_FEE);
                taxlotVisibleFields.Add(AllocationUIConstants.STAMP_DUTY);
                taxlotVisibleFields.Add(AllocationUIConstants.TRANSACTION_LEVY);
                taxlotVisibleFields.Add(AllocationUIConstants.CLEARING_FEE);
                taxlotVisibleFields.Add(AllocationUIConstants.MISC_FEES);
                taxlotVisibleFields.Add(AllocationUIConstants.TAX_ON_COMMISSIONS);
                taxlotVisibleFields.Add(AllocationUIConstants.SEC_FEE);
                taxlotVisibleFields.Add(AllocationUIConstants.OCC_FEE);
                taxlotVisibleFields.Add(AllocationUIConstants.ORF_FEE);
                taxlotVisibleFields.Add(AllocationUIConstants.LOT_ID);
                taxlotVisibleFields.Add(AllocationUIConstants.FXRATE);
                taxlotVisibleFields.Add(AllocationUIConstants.EXTERNAL_TRANS_ID);
                taxlotVisibleFields.Add(AllocationUIConstants.SETTLEMENT_CURRENCY);
                taxlotVisibleFields.Add(AllocationUIConstants.CLOSING_STATUS);
                taxlotVisibleFields.Add(AllocationUIConstants.CLOSING_ALGO_TEXT);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute1);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute2);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute3);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute4);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute5);
                taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute6);
                for (int i = 7; i <= 45; i++)
                {
                    taxlotVisibleFields.Add(AllocationUIConstants.TradeAttribute + i);
                }         
                taxlotVisibleFields.Add(AllocationUIConstants.AVGPRICE_BASE);
                taxlotVisibleFields.Add(AllocationUIConstants.NETAMOUNT_LOCAL);
                taxlotVisibleFields.Add(AllocationUIConstants.NETAMOUNT_BASE);
                taxlotVisibleFields.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL);
                taxlotVisibleFields.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_BASE);
                taxlotVisibleFields.Add(AllocationUIConstants.CHANGE_TYPE);
                taxlotVisibleFields.Add(AllocationUIConstants.TOTAL_PERCENTAGE);
                taxlotVisibleFields.Add(AllocationUIConstants.OPTION_PREMIUM_ADJUSTMENT);
                taxlotVisibleFields.Add(AllocationUIConstants.ACCRUED_INTEREST);

                foreach (Field field in taxlotFieldLayout.Fields)
                {
                    if (!taxlotVisibleFields.Contains(field.Name))
                    {
                        field.Visibility = Visibility.Collapsed;
                        // Used to hide the columns from the field-chooser, PRANA-14858
                        field.AllowHiding = AllowFieldHiding.Never;
                    }
                    else
                    {
                        if (!field.Visibility.Equals(Visibility.Collapsed))
                            field.Visibility = Visibility.Visible;
                    }
                }

                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    if (taxlotFieldLayout.Fields.IndexOf(AllocationUIConstants.TradeAttribute6) != -1)
                        taxlotFieldLayout.Fields[AllocationUIConstants.TradeAttribute6].AllowEdit = false;
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
        /// Sets the column format for taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        /// <param name="resource">The resource.</param>
        internal static void SetColumnFormatForTaxlots(FieldLayout taxlotFieldLayout, ResourceDictionary resource)
        {
            try
            {
                List<string> taxlotFields = new List<string>();
                taxlotFields.Add(AllocationUIConstants.COMMISSION);
                taxlotFields.Add(AllocationUIConstants.SOFT_COMMISSION);
                taxlotFields.Add(AllocationUIConstants.OCC_FEE);
                taxlotFields.Add(AllocationUIConstants.SEC_FEE);
                taxlotFields.Add(AllocationUIConstants.ORF_FEE);
                taxlotFields.Add(AllocationUIConstants.CLEARING_BROKER_FEE);
                taxlotFields.Add(AllocationUIConstants.OTHER_BROKER_FEES);
                taxlotFields.Add(AllocationUIConstants.STAMP_DUTY);
                taxlotFields.Add(AllocationUIConstants.CLEARING_FEE);
                taxlotFields.Add(AllocationUIConstants.MISC_FEES);
                taxlotFields.Add(AllocationUIConstants.TAX_ON_COMMISSIONS);
                taxlotFields.Add(AllocationUIConstants.TRANSACTION_LEVY);

                //Taxlot Fields
                foreach (string field in taxlotFields)
                {
                    if (taxlotFieldLayout.Fields.IndexOf(field) != -1)
                        taxlotFieldLayout.Fields[field].Settings.EditorStyle = resource["CommissionColumnPrecision"] as Style;
                }
                if (taxlotFieldLayout.Fields.IndexOf(AllocationUIConstants.TAXLOT_QTY) != -1)
                    taxlotFieldLayout.Fields[AllocationUIConstants.TAXLOT_QTY].Settings.EditorStyle = resource["QtyColumnPrecision"] as Style;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default layout for taxlots.
        /// </summary>
        /// <param name="taxlotFieldLayout">The taxlot field layout.</param>
        private static void SetDefaultLayoutForTaxlots(FieldLayout taxlotFieldLayout)
        {
            try
            {
                List<string> taxlotVisibleFields = new List<string>();

                taxlotVisibleFields.Add(AllocationUIConstants.LEVEL1_NAME);
                taxlotVisibleFields.Add(AllocationUIConstants.LEVEL2_NAME);
                taxlotVisibleFields.Add(AllocationUIConstants.TOTAL_PERCENTAGE);
                taxlotVisibleFields.Add(AllocationUIConstants.TAXLOT_ID);
                taxlotVisibleFields.Add(AllocationUIConstants.COMMISSION);
                taxlotVisibleFields.Add(AllocationUIConstants.SOFT_COMMISSION);
                taxlotVisibleFields.Add(AllocationUIConstants.CLOSING_STATUS);
                taxlotVisibleFields.Add(AllocationUIConstants.CLOSING_ALGO_TEXT);
                taxlotVisibleFields.Add(AllocationUIConstants.NETAMOUNT_LOCAL);
                taxlotVisibleFields.Add(AllocationUIConstants.NETAMOUNT_BASE);
                taxlotVisibleFields.Add(AllocationUIConstants.AVGPRICE_BASE);
                taxlotVisibleFields.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL);
                taxlotVisibleFields.Add(AllocationUIConstants.PRINCIPAL_AMOUNT_BASE);

                if (taxlotFieldLayout != null)
                {
                    foreach (Field field in taxlotFieldLayout.Fields)
                        field.Visibility = Visibility.Collapsed;

                    for (int i = 0; i < taxlotVisibleFields.Count; i++)
                    {
                        if (taxlotFieldLayout.Fields.IndexOf(taxlotVisibleFields[i]) != -1)
                        {
                            taxlotFieldLayout.Fields[taxlotVisibleFields[i]].Visibility = Visibility.Visible;
                            taxlotFieldLayout.Fields[taxlotVisibleFields[i]].ActualPosition = new FieldPosition(i, 0, 0, 0);
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
        }

        #endregion

        #region Allocation Orders

        /// <summary>
        /// Columnses the customization for orders.
        /// </summary>
        /// <param name="orderFieldLayout">The order field layout.</param>
        /// <param name="resource">The resource.</param>
        internal static void ColumnsCustomizationForOrders(FieldLayout orderFieldLayout, ResourceDictionary resource)
        {
            try
            {
                SetHeaderCaptionForOrders(orderFieldLayout);
                SetEditablePropertyOrders(orderFieldLayout, resource);
                SetVisibilityForOrders(orderFieldLayout);
                SetColumnFormatForOrders(orderFieldLayout, resource);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the header caption for orders.
        /// </summary>
        /// <param name="orderFieldLayout">The order field layout.</param>
        private static void SetHeaderCaptionForOrders(FieldLayout orderFieldLayout)
        {
            try
            {
                if (orderFieldLayout.Fields.IndexOf(AllocationUIConstants.PRANA_MSG_TYPE) != -1)
                    orderFieldLayout.Fields[AllocationUIConstants.PRANA_MSG_TYPE].Label = AllocationUIConstants.CAPTION_PRANA_MSG_TYPE;
                if (orderFieldLayout.Fields.IndexOf(AllocationUIConstants.AVGPRICE) != -1)
                    orderFieldLayout.Fields[AllocationUIConstants.AVGPRICE].Label = AllocationUIConstants.CAPTION_AVGPRICELOCAL;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the editable property orders.
        /// </summary>
        /// <param name="orderFieldLayout">The order field layout.</param>
        /// <param name="resource">The resource.</param>
        private static void SetEditablePropertyOrders(FieldLayout orderFieldLayout, ResourceDictionary resource)
        {
            try
            {
                List<string> orderFields = new List<string>();
                orderFields.Add(AllocationUIConstants.AVGPRICE);
                orderFields.Add(AllocationUIConstants.CUMQTY);
                orderFields.Add(AllocationUIConstants.ORDER_ID);
                orderFields.Add(AllocationUIConstants.PRANA_MSG_TYPE);
                orderFields.Add(AllocationUIConstants.MULTITRADE_NAME);

                foreach (string field in orderFields)
                {
                    if (orderFieldLayout.Fields.IndexOf(field) != -1)
                        orderFieldLayout.Fields[field].AllowEdit = false;
                }

                //For field chooser properties
                orderFieldLayout.Settings.HeaderPrefixAreaDisplayMode = HeaderPrefixAreaDisplayMode.None;
                orderFieldLayout.Settings.HeaderPrefixAreaStyle = resource["HideHeaderPrefixArea"] as Style;
                orderFieldLayout.Settings.RecordSelectorStyle = resource["HideRecordSelector"] as Style;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the visibility for orders.
        /// </summary>
        /// <param name="orderFieldLayout">The order field layout.</param>
        private static void SetVisibilityForOrders(FieldLayout orderFieldLayout)
        {
            try
            {
                List<string> orderVisibleFields = new List<string>();
                orderVisibleFields.Add(AllocationUIConstants.ORDER_ID);
                orderVisibleFields.Add(AllocationUIConstants.PRANA_MSG_TYPE);
                orderVisibleFields.Add(AllocationUIConstants.MULTITRADE_NAME);
                orderVisibleFields.Add(AllocationUIConstants.AVGPRICE);
                orderVisibleFields.Add(AllocationUIConstants.CUMQTY);

                foreach (Field field in orderFieldLayout.Fields)
                {
                    if (!orderVisibleFields.Contains(field.Name))
                    {
                        field.Visibility = Visibility.Collapsed;
                        // Used to hide the columns from the field-chooser, PRANA-14858
                        field.AllowHiding = AllowFieldHiding.Never;
                    }
                    else
                    {
                        field.Visibility = Visibility.Visible;
                        // Used to fix the columns at order level in unallocated grid, PRANA-14858
                        field.AllowFixing = AllowFieldFixing.Near;
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
        /// Sets the column format for orders.
        /// </summary>
        /// <param name="orderFieldLayout">The order field layout.</param>
        /// <param name="resource">The resource.</param>
        private static void SetColumnFormatForOrders(FieldLayout orderFieldLayout, ResourceDictionary resource)
        {
            try
            {
                if (orderFieldLayout.Fields.IndexOf(AllocationUIConstants.CUMQTY) != -1)
                    orderFieldLayout.Fields[AllocationUIConstants.CUMQTY].Settings.EditorStyle = resource["QtyColumnPrecision"] as Style;
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
