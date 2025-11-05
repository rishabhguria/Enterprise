using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities.Constants
{
   public static class AutomationStepsConstants
    {
       /// <summary>
        /// UpdateForexConversionMonthly step in Daily Valuation
       /// </summary>
       public const string UPDATE_FOREX_CONVERSION_MONTHLY = "UpdateForexConversionMonthly";

       /// <summary>
       /// Set Closing Preference step in Closing Module
       /// </summary>
       public const string CLOSING_PREFERENCES = "SetClosingPreference";

        /// <summary>
       /// Portfolio mangement module.
       /// </summary>
       public const string PORTFOLIO_MANAGEMENT = "PortfolioManagement"; 

       /// <summary>
       /// Portfolio mangement module.
       /// </summary>
       public const string OPEN_PTT_FROM_PM = "OpenPTTFromPM";
       
       #region
        /// <summary>
        /// Check portfolio mangement step.
        /// </summary>
       public const string CHECK_PORTFOLIO_MANAGEMENT = "CheckPortfolioManagement";

       /// <summary>
       /// The check dashboard pm
       /// </summary>
       public const string CHECK_DASHBOARD_PM = "CheckDashboardPM";
       
       /// <summary>
       /// The check dashboard with pm
       /// </summary>
       public const string CHECK_DASHBOARD_WITH_PM = "CheckDashboardWithPM";
       
       /// <summary>
       /// The check summary rows pm
       /// </summary>
       public const string CHECK_SUMMARY_ROWS_PM = "CheckSummaryRowsPM";

       /// <summary>
       /// The close pm
       /// </summary>
       public const string CLOSE_PM = "ClosePM";

       /// <summary>
       /// The filter pm
       /// </summary>
       public const string FILTER_PM = "FilterPM";

       /// <summary>
       /// The group row pm
       /// </summary>
       public const string GROUP_ROW_PM = "GroupRowPM";

       /// <summary>
       /// The open PST from pm
       /// </summary>
       public const string OPEN_PST_FROM_PM = "OpenPSTFromPM";

       /// <summary>
       /// The remove filter pm
       /// </summary>
       public const string REMOVE_FILTER_PM = "RemoveFilterPM";

       /// <summary>
       /// Open Daily Valuation From PM
       /// </summary>
       public const string OPEN_DAILY_VALUATION_FROM_PM = "OpenDailyValuationFromPM";

       /// <summary>
       /// Open Create Transaction From PM
       /// </summary>
       public const string OPEN_CREATE_TRANSACTION_FROM_PM = "OpenCreateTransactionFromPM";

       /// <summary>
       /// Open Closing From PM
       /// </summary>
       public const string OPEN_CLOSING_FROM_PM = "OpenClosingFromPM";

       /// <summary>
       /// Open Corporate Action From PM
       /// </summary>
       public const string OPEN_CORPORATE_ACTION_FROM_PM = "OpenCorporateActionFromPM";

       /// <summary>
       /// Close Account Position From PM
       /// </summary>
       public const string CLOSE_ACCOUNT_POSITION_FROM_PM = "CloseAccountPositionFromPM";

       /// <summary>
       /// Close Position From PM
       /// </summary>
       public const string CLOSE_POSITION_FROM_PM = "ClosePositionFromPM";

       /// <summary>
       /// Close Selected Taxlots From PM
       /// </summary>
       public const string CLOSE_SELECTED_TAXLOTS_FROM_PM = "CloseSelectedTaxlotsFromPM";

       /// <summary>
       /// Hide Summary Tool From PM
       /// </summary>
       public const string HIDE_SUMMARY_TOOL_FROM_PM = "HideSummaryToolFromPM";

       /// <summary>
       /// Show Summary Tool From PM
       /// </summary>
       public const string SHOW_SUMMARY_TOOL_FROM_PM = "ShowSummaryToolFromPM";

       /// <summary>
       /// Add Trade From PM
       /// </summary>
       public const string ADD_TRADE_FROM_PM = "AddTradeFromPM";

       /// <summary>
       /// Rename Custom View PM
       /// </summary>
       public const string RENAME_CUSTOM_VIEW_PM = "RenameCustomViewPM";

       /// <summary>
       /// Add Custom View PM
       /// </summary>
       public const string ADD_CUSTOM_VIEW_PM = "AddCustomViewPM";

       /// <summary>
       /// Add Custom View Right Click PM
       /// </summary>
       public const string ADD_CUSTOM_VIEW_RIGHT_CLICK_PM = "AddCustomViewRightClickPM";

       /// <summary>
       /// Expand Collapse PM
       /// </summary>
       public const string EXPAND_COLLAPSE_PM = "ExpandCollapsePM";

       /// <summary>
       /// Delete Custom View PM
       /// </summary>
       public const string DELETE_CUSTOM_VIEW_PM = "DeleteCustomViewPM";
        #endregion

       /// <summary>
       /// Closing module
       /// </summary>
       public const string CLOSING = "Closing";
       #region       
       /// <summary>
       /// The automatic closing
       /// </summary>
       public const string AUTOMATIC_CLOSING = "AutomaticClosing";

       /// <summary>
       /// The cash settlement at closing dt spot px
       /// </summary>
       public const string CASH_SETTLEMENT_AT_CLOSING_DT_SPOT_PX = "CashSettlementAtClosingDtSpotPx";

       /// <summary>
       /// The cash settlement at cost
       /// </summary>
       public const string CASH_SETTLEMENT_AT_COST = "CashSettlementAtCost";

       /// <summary>
       /// The cash settlement at zero price
       /// </summary>
       public const string CASH_SETTLEMENT_AT_ZERO_PRICE = "CashSettlementAtZeroPrice";

       /// <summary>
       /// The close closing UI
       /// </summary>
       public const string CLOSE_CLOSING_UI = "CloseClsoingUI";

       /// <summary>
       /// The close order
       /// </summary>
       public const string CLOSE_ORDER = "CloseOrder";

       /// <summary>
       /// The closing clean up
       /// </summary>
       public const string CLOSING_CLEAN_UP = "ClosingCleanUp";

       /// <summary>
       /// The exercise assignment
       /// </summary>
       public const string EXERCISE_ASSIGNMENT = "ExerciseAssignment";

       /// <summary>
       /// The exercise assignment at zero
       /// </summary>
       public const string EXERCISE_ASSIGNMENT_AT_ZERO = "ExerciseAssignmentAtZero";

       /// <summary>
       /// The expire
       /// </summary>
       public const string EXPIRE = "Expire";
       
       /// <summary>
       /// The get data closing
       /// </summary>
       public const string GET_DATA_CLOSING = "GetDataClosing";

       /// <summary>
       /// The manual closing
       /// </summary>
       public const string MANUAL_CLOSING = "ManualClosing";

       /// <summary>
       /// The physical settlement
       /// </summary>
       public const string PHYSICAL_SETTLEMENT = "PhysicalSettlement";

       /// <summary>
       /// The select trade unexpired unsettled
       /// </summary>
       public const string SELECT_TRADE_UNEXPIRED_UNSETTLED = "SelectTradeUnexpiredUnsettled";

       /// <summary>
       /// The swap expire
       /// </summary>
       public const string SWAP_EXPIRE = "SwapExpire";

       /// <summary>
       /// The swap exipre and rollover
       /// </summary>
       public const string SWAP_EXPIRE_AND_ROLLOVER = "SwapExpireAndRollOver";

       /// <summary>
       /// The unwind close order
       /// </summary>
       public const string UNWIND_CLOSE_ORDER = "UnwindCloseOrder";

       /// <summary>
       /// The unwind closing
       /// </summary>
       public const string UNWIND_CLOSING = "UnwindClosing";

       /// <summary>
       /// The unwind trade settlement
       /// </summary>
       public const string UNWIND_TRADE_SETTLEMENT = "UnwindTradeSettlement";

       /// <summary>
       /// The verify closed trades
       /// </summary>
       public const string VERIFY_CLOSED_TRADES = "VerifyClosedTrades";

       /// <summary>
       /// The verify close order
       /// </summary>
       public const string VERIFY_CLOSE_ORDER = "VerifyCloseOrder";

       /// <summary>
       /// The verify expired settled grid
       /// </summary>
       public const string VERIFY_EXPIRED_SETTLED_GRID = "VerifyExpiredSettledGrid";

       /// <summary>
       /// The verify long positions grid
       /// </summary>
       public const string VERIFY_LONG_POSITIONS_GRID = "VerifyLongPositionsGrid";

       /// <summary>
       /// The verify short positions grid
       /// </summary>
       public const string VERIFY_SHORT_POSITIONS_GRID = "VerifyShortPositionsGrid";

       /// <summary>
       /// The verify unexpired unsettled grid
       /// </summary>
       public const string VERIFY_UNEXPIRED_UNSETTLED_GRID = "VerifyUnexpiredUnsettledGrid";   
       #endregion

       /// <summary>
       /// Third Party Module
       /// </summary>
       public const string THIRD_PARTY = "ThirdParty";

       /// <summary>
       /// Select Third Party
       /// </summary>
       public const string SELECT_THIRD_PARTY = "SelectThirdParty";

       /// <summary>
       /// The Generate Third Party
       /// </summary>
       public const string THIRD_PARTY_GENERATE = "ThirdPartyGenerate";

       /// <summary>
       /// The Send Third Party
       /// </summary>
       public const string THIRD_PARTY_SEND = "ThirdPartySend";

       /// <summary>
       /// The View Third Party
       /// </summary>
       public const string VIEW_THIRD_PARTY = "ViewThirdParty";

       /// <summary>
       /// Verify Third Party View
       /// </summary>
       public const string VERIFY_THIRD_PARTY_VIEW = "VerifyThirdPartyView";


       /// <summary>
       /// Verify Third party Generate
       /// </summary>
       public const string VERIFY_THIRD_PARTY_GENERATE = "VerifyThirdPartyGenerate";

       /// <summary>
       /// Verify Third party Send
       /// </summary>
       public const string VERIFY_THIRD_PARTY_SEND = "VerifyThirdPartySend";

       /// <summary>
       /// CorporateAction Module
       /// </summary>
       public const string Corporate_Action = "CorporateAction";

       /// <summary>
       /// Check Corporate Action Details
       /// </summary>
       public const string CHECK_CORPORATE_ACTION_DETAILS = "CheckCorporateActionDetails";

       /// <summary>
       /// Check Corporate Action
       /// </summary>
       public const string CHECK_CORPORATE_ACTION= "CheckCorporateAction";

       /// <summary>
       /// Pricing Input Module
       /// </summary>
       public const string Pricing_Input = "PricingInput";

       /// <summary>
       /// The EODT Preferences
       /// </summary>
       public const string EODTPreferences = "EODTPreferences";

       /// <summary>
       /// The Live Data
       /// </summary>
       public const string Live_Data = "LiveData";

       /// <summary>
       /// The Update Shares Outstanding
       /// </summary>
       public const string Update_Shares_Outstanding = "UpdateSharesOutstanding";

       /// <summary>
       /// Uncheck Dividend Yield
       /// </summary>
       public const string UNCHECK_DIVIDEND_YIELD = "UncheckDividendYield";

       /// <summary>
       /// Allocation module
       /// </summary>       
       public const string ALLOCATION = "Allocation";


        #region       
       /// <summary>
       /// The add calculated preferences
       /// </summary>
       public const string ADD_CALCULATED_PREFERENCES = "AddCalculatedPreferences";

       /// <summary>
       /// The add calculated preference toolbar
       /// </summary>
       public const string ADD_CALCULATED_PREFERENCE_TOOLBAR = "AddCalculatedPreferenceToolbar";

       /// <summary>
       /// The add master fund preferences
       /// </summary>
       public const string ADD_MF_PREFERENCES = "AddMFPreferences";

       /// <summary>
       /// The add master fund preferences from toolbar
       /// </summary>
       public const string ADD_MF_PREFERENCE_TOOLBAR = "AddMFPreferenceToolbar";

       /// <summary>
       /// The set generalrule calpreference
       /// </summary>
       public const string SET_GENERALRULE_CALCULATED_PREF = "SetGeneralRuleCalculatedPref";

       /// <summary>
       /// The set general rule master fund preference
       /// </summary>
       public const string SET_MF_PREF_GENERAL_RULE= "SetMFPrefGeneralRule";

       // <summary>
       /// The modify generalrule preference
       /// </summary>
       public const string MODIFY_GR_PREFERENCE_VALUES = "ModifyGRPreferenceValues";

       // <summary>
       /// The modify generalrule preference of Master fund pref
       /// </summary>
       public const string MODIFY_MF_PREF_GENERAL_RULE_VALUES = "ModifyMFPrefGeneralRuleValues";

       /// <summary>
       /// The allocate
       /// </summary>
       public const string ALLOCATE = "Allocate";

       /// <summary>
       /// The allocate with popup
       /// </summary>
       public const string ALLOCATE_WITH_POPUP = "AllocateWithPopUp";

       /// <summary>
       /// The save allocation
       /// </summary>
       public const string SAVE_ALLOCATION = "SaveAllocation";

       /// <summary>
       /// The unallocate without save
       /// </summary>
       public const string UNALLOCATE_WITHOUT_SAVE = "UnallocateWithOutSave";
       
       /// <summary>
       /// The apply bulk update on groups
       /// </summary>
       public const string APPLY_BULK_UPDATE_ON_GROUPS = "ApplyBulkUpdateOnGroups";

       /// <summary>
       /// The apply bulk update on taxlots
       /// </summary>
       public const string APPLY_BULK_UPDATE_ON_TAXLOTS = "ApplyBulkUpdateOnTaxlots";

       /// <summary>
       /// The apply prefetch filters
       /// </summary>
       public const string APPLY_PREFETCH_FILTERS = "ApplyPrefetchFilters";

       /// <summary>
       /// The clear prefetch filters
       /// </summary>
       public const string CLEAR_PREFETCH_FILTERS = "ClearPrefetchFilters";

       /// <summary>
       /// The book or update swap
       /// </summary>
       public const string BOOK_OR_UPDATE_SWAP = "BookOrUpdateSwap";

       /// <summary>
       /// The bulk change calculated preferences
       /// </summary>
       public const string BULK_CHANGE_CALCULATED_PREFERENCES = "BulkChangeCalculatedPreferences";

       /// <summary>
       /// The check allocated groups
       /// </summary>
       public const string CHECK_ALLOCATED_GROUPS = "CheckAllocatedGroups";

       /// <summary>
       /// The check allocated taxloat
       /// </summary>
       public const string CHECK_ALLOCATED_TAXLOAT = "CheckAllocatedTaxlots";

       /// <summary>
       /// The check fixed preference
       /// </summary>
       public const string CHECK_FIXED_PREFERENCE = "CheckFixedPreference";

       /// <summary>
       /// The check unallocated groups
       /// </summary>
       public const string CHECK_UNALLOCATED_GROUPS = "CheckUnallocatedGroups";

       /// <summary>
       /// The clean up
       /// </summary>
       public const string CLEAN_UP = "CleanUp";

       /// <summary>
       /// The close allocation
       /// </summary>
       public const string CLOSE_ALLOCATION = "CloseAllocation";

       /// <summary>
       /// The close calculated preferences
       /// </summary>
       public const string CLOSE_CALCULATED_PREFERENCES = "CloseCalculatedPreferences";

       /// <summary>
       /// Is Allocation opens Through GO To allocation
       /// </summary>
       public const string IS_Allocation_Open = "IsAllocationOpen";

       /// <summary>
       /// The commission bulk change on groups
       /// </summary>
       public const string COMMISSION_BULK_CHANGE_ON_GROUPS = "CommissionBulkChangeOnGroups";

       /// <summary>
       /// The commission bulk change on taxlots
       /// </summary>
       public const string COMMISSION_BULK_CHANGE_ON_TAXLOTS = "CommissionBulkChangeOnTaxlots";

       /// <summary>
       /// The copy calculated perferences
       /// </summary>
       public const string COPY_CALCULATED_PERFERENCES = "CopyCalculatedPreferences";

       /// <summary>
       /// The copy calculated preference tool
       /// </summary>
       public const string COPY_CALCULATED_PREFERENCE_TOOL = "CopyCalculatedPreferenceTool";
       
       /// <summary>
       /// The delete calculated preferences
       /// </summary>
       public const string DELETE_CALCULATED_PREFERENCES = "DeleteCalculatedPreferences";

       /// <summary>
       /// The delete calculated preferences tool
       /// </summary>
       public const string DELETE_CALCULATED_PREFERENCES_TOOL = "DeleteCalculatedPreferencesTool";

       /// <summary>
       /// The delete master fund preferences
       /// </summary>
       public const string DELETE_MF_PREFERENCES = "DeleteMFPreferences";

       /// <summary>
       /// The delete master fund preferences from toolbar
       /// </summary>
       public const string DELETE_MF_PREFERENCES_TOOLBAR = "DeleteMFPreferencesToolbar";
       
       /// <summary>
       /// The delete fixed preference
       /// </summary>
       public const string DELETE_FIXED_PREFERENCE = "DeleteFixedPreference";

       /// <summary>
       /// The delete group
       /// </summary>
       public const string DELETE_GROUP = "DeleteGroup";

       /// <summary>
       /// The delete group
       /// </summary>
       public const string DELETE_GROUP_WITHOUT_SAVE = "DeleteGroupWithOutSave";

       /// <summary>
       /// The edit allocated group side panel
       /// </summary>
       public const string EDIT_ALLOCATED_GROUP_SIDE_PANEL = "EditAllocatedGroupSidePanel";

       /// <summary>
       /// The edit trades allocated
       /// </summary>
       public const string EDIT_TRADES_ALLOCATED = "EditTradesAllocated";

       /// <summary>
       /// The edit trades unallocated
       /// </summary>
       public const string EDIT_TRADES_UNALLOCATED = "EditTradesUnallocated";

       /// <summary>
       /// The edit unallocated group side panel
       /// </summary>
       public const string EDIT_UNALLOCATED_GROUP_SIDE_PANEL = "EditUnallocatedGroupSidePanel";

       /// <summary>
       /// The enter account values to allocate
       /// </summary>
       public const string ENTER_ACCOUNT_VALUES_TO_ALLOCATE = "EnterAccountValuesToAllocate";

       /// <summary>
       /// The enter allocate preference
       /// </summary>
       public const string ENTER_ALLOCATE_PREFERENCE = "EnterAllocatePreference";

       /// <summary>
       /// The enter custom allocate preference
       /// </summary>
       public const string ENTER_CUSTOM_ALLOCATE_PREFERENCE = "EnterCustomAllocatePreference";

       /// <summary>
       /// The enter strategy values to allocate
       /// </summary>
       public const string ENTER_STRATEGY_VALUES_TO_ALLOCATE = "EnterStrategyVAluesToAllocate";

       /// <summary>
       /// The export all calculatedpreferences
       /// </summary>
       public const string EXPORT_ALL_CALCULATED_PREFERENCES = "ExportAllCalculatedPreferences";

       /// <summary>
       /// The export all calculated prefer tool
       /// </summary>
       public const string EXPORT_ALL_CALCULATED_PREFER_TOOL = "ExportAllCalculatedPreferTool";

       /// <summary>
       /// The export calculated preference
       /// </summary>
       public const string EXPORT_CALCULATED_PREFERENCE = "ExportCalculatedPreference";

       /// <summary>
       /// The general preference company userwise
       /// </summary>
       public const string GENERAL_PREF_COMPANY_USERWISE = "GeneralPrefCompanyUserWise";

       /// <summary>
       /// The general preferences default rule
       /// </summary>
       public const string GENERAL_PREFERENCES_DEFAULT_RULE = "GeneralPreferencesDefaultRule";

       /// <summary>
       /// The get data allocation
       /// </summary>
       public const string GET_DATA_ALLOCATION = "GetDataAllocation";

       /// <summary>
       /// The get fixed preference
       /// </summary>
       public const string GET_FIXED_PREFERENCE = "GetFixedPreference";

       /// <summary>
       /// The group trade
       /// </summary>
       public const string GROUP_TRADES = "GroupTrades";

       /// <summary>
       /// The import calculated preferences
       /// </summary>
       public const string IMPORT_CALCULATED_PREFERENCES = "ImportCalculatedPreferences";

       /// <summary>
       /// The import calculated preferences tool
       /// </summary>
       public const string IMPORT_CALCULATED_PREFERENCES_TOOL = "ImportCalculatedPreferencesTool";

       /// <summary>
       /// The open audit trail allocated group
       /// </summary>
       public const string OPEN_AUDIT_TRAIL_ALLOCATED_GROUP = "OpenAuditTrailAllocatedGroup";

       /// <summary>
       /// The open audit trail unallocated group
       /// </summary>
       public const string OPEN_AUDIT_TRAIL_UNALLOCATED_GROUP = "OpenAuditTrailUnAllocatedGroup";

       /// <summary>
       /// The open symbol lookup allocated group
       /// </summary>
       public const string OPEN_SYMBOL_LOOKUP_ALLOCATED_GROUP = "OpenSymbolLookupAllocatedGroup";

       /// <summary>
       /// The open symbol lookup unallocated group
       /// </summary>
       public const string OPEN_SYMBOL_LOOKUP_UNALLOCATED_GROUP = "OpenSymbolLookupUnAllocatedGroup";

       /// <summary>
       /// The reallocate
       /// </summary>
       public const string REALLOCATE = "Reallocate";

       /// <summary>
       /// The rename calculated preferences
       /// </summary>
       public const string RENAME_CALCULATED_PREFERENCES = "RenameCalculatedPreferences";

       /// <summary>
       /// The rename master fund preferences
       /// </summary>
       public const string RENAME_MF_PREFERENCES = "RenameMFPreferences";

       /// <summary>
       /// The rename trade attributes
       /// </summary>
       public const string RENAME_TRADE_ATTRIBUTES = "RenameTradeAttributes";

       /// <summary>
       /// The run automatic group
       /// </summary>
       public const string RUN_AUTO_GROUP = "RunAutoGroup";

       /// <summary>
       /// The run prorata
       /// </summary>
       public const string RUN_PRORATA = "RunProrata";

       /// <summary>
       /// The save calculated preferences
       /// </summary>
       public const string SAVE_CALCULATED_PREFERENCES = "SaveCalculatedPreferences";

       /// <summary>
       /// The save and close master fund preferences
       /// </summary>
       public const string SAVE_CLOSE_MF_PREFERENCES = "SaveCloseMFPreferences";

       /// <summary>
       /// The update calculated preferences
       /// </summary>
       public const string UPDATE_CALCULATED_PREFERENCES = "UpdateCalculatedPreferences";

       /// <summary>
       /// The select allocated group
       /// </summary>
       public const string SELECT_ALLOCATED_GROUP = "SelectAllocatedGroup";

       /// <summary>
       /// The select unallocated group
       /// </summary>
       public const string SELECT_UNALLOCATED_GROUP = "SelectUnallocatedGroup";

       /// <summary>
       /// The set account value calculated preference
       /// </summary>
       public const string SET_ACCOUNT_VALUE_CALCULATED_PREF = "SetAccountValueCAlculatedPref";

       /// <summary>
       /// The set auto grouping account
       /// </summary>
       public const string SET_AUTO_GROUPING_ACCOUNT = "SetAutoGroupingAccount";

       /// <summary>
       /// The set auto grouping preference
       /// </summary>
       public const string SET_AUTO_GROUPING_PREFERENCES = "SetAutoGroupingPreferences";

       /// <summary>
       /// The set average price rounding
       /// </summary>
       public const string SET_AVG_PRICE_ROUNDING = "SetAvgPriceRounding";

       /// <summary>
       /// The set broker account mapping
       /// </summary>
       public const string SET_BROKER_ACCOUNT_MAPPING = "SetBrokerAccountMapping";

       /// <summary>
       /// The set account value master fund preference
       /// </summary>
       public const string SET_MF_PREF_ACCOUNT_VALUES = "SetMFPrefAccountValues";

       /// <summary>
       /// The update account value calculated preference
       /// </summary>
       public const string UPDATE_ACCOUNT_VALUE_CALCULATED_PREF = "UpdateAccountValueCAlculatedPref";
       
       /// <summary>
       /// The set default rule calculated perf
       /// </summary>
       public const string SET_DEFAULT_RULE_CALCULATED_PERF = "SetDefaultRuleCalculatedPref";

       /// <summary>
       /// The set default rule accounts in the master fund pref
       /// </summary>
       public const string SET_MF_PREF_FUND_DEFAULT_RULE = "SetMFPrefFundDefaultRule";

       /// <summary>
       /// The set default rule Master Fund Preference
       /// </summary>
       public const string SET__MF_PREFERENCE_DEFAULT_RULE = "SetMFPreferencesDefaultRule";

       /// <summary>
       /// The update default rule calculated perf
       /// </summary>
       public const string UPDATE_DEFAULT_RULE_CALCULATED_PERF = "UpdateDefaultRuleCalculatedPref";

       /// <summary>
       /// The set general preferences
       /// </summary>
       public const string SET_GENERAL_PREFERENCES = "SetGeneralPreferences";

       /// <summary>
       /// The set grouping rules
       /// </summary>
       public const string SET_GROUPING_RULES = "SetGroupingRules";

       /// <summary>
       /// The set master fund ratio
       /// </summary>
       public const string SET_MASTER_FUND_RATIO = "SetMasterFundRatio";

       /// <summary>
       /// The set master fund ratio in MF Preference
       /// </summary>
       public const string SET_MF_PREF_FUND_DISTRIBUTION = "SetMFPrefFundDistribution";

       /// <summary>
       /// The set strategy value calculated preference
       /// </summary>
       public const string SET_STRATEGY_VALUE_CALCULATED_PREF = "SetStrategyValueCalculatedPref";

       /// <summary>
       /// The set trade attribute preferences
       /// </summary>
       public const string SET_TRADE_ATTRIBUTE_PREFERENCES = "SetTradeAttributePreferences";

       /// <summary>
       /// Show master fund as clinet
       /// </summary>
       public const string SHOW_MASTER_FUND_AS_CLIENT = "ShowMasterFundAsClient";

       /// <summary>
       /// Show Master Fund on TT
       /// </summary>
       public const string SHOW_MASTER_FUND_ON_TT = "ShowMasterFundOnTT";

       /// <summary>
       /// The set strategy value Master fund preference
       /// </summary>
       public const string SET_MF_PREF_STRATEGY_VALUES = "SetMFPrefStrategyValues";

       /// <summary>
       /// The update strategy value calculated preference
       /// </summary>
       public const string UPDATE_STRATEGY_VALUE_CAL_PREF = "UpdateStrategyValueCalPref";

       /// <summary>
       /// The trade attribute bulk change groups
       /// </summary>
       public const string TRADE_ATTRIBUTE_BULK_CHANGE_GROUPS = "TradeAttributeBulkChangeGroups";

       /// <summary>
       /// The trade attribute bulk change taxlots
       /// </summary>
       public const string TRADE_ATTRIBUTE_BULK_CHANGE_TAXLOTS = "TradeAttributeBulkChangeTaxlots";

       /// <summary>
       /// The unallocate
       /// </summary>
       public const string UNALLOCATE = "Unallocate";

       /// <summary>
       /// The ungroup trades
       /// </summary>
       public const string UNGROUP_TRADES = "UngroupTrades";
        #endregion

       /// <summary>
       /// Daily valuation module.
       /// </summary>
       public const string DAILY_VALUATION = "DailyValuation";

       /// <summary>
       /// The create transaction module
       /// </summary>
       public const string CREATE_TRANSACTION = "CreateTransaction";

       /// <summary>
       /// The Save transaction module
       /// </summary>
       public const string SAVE_TRANSACTION = "SaveTransaction";
       #region       
       /// <summary>
       /// The add new transaction
       /// </summary>
       public const string ADD_NEW_TRANSACTION = "AddNewTransaction";
       #endregion

       /// <summary>
       /// The PST module
       /// </summary>
       public const string PST = "PST";
       #region       
       /// <summary>
       /// The run PST
       /// </summary>
       public const string RUN_PST = "RunPST";
       #endregion

       /// <summary>
       /// The import Module
       /// </summary>
       public const string IMPORT = "Import";

       /// <summary>
       /// The Apply Filter On Import
       /// </summary>
       public const string APPLY_FILTER_ON_IMPORT = "ApplyFilterOnImport";

       /// <summary>
       /// The Select All On Import
       /// </summary>
       public const string SELECT_ALL_ON_IMPORT = "SelectAllOnImport";

       /// <summary>
       /// GetDataSymbolLookup
       /// </summary>
       public const string SYMBOL_LOOKUP = "SymbolLookup";

       #region   
       /// <summary>
       /// GetDataSymbolLookup
       /// </summary>
       public const string GETDATA_SYMBOL_LOOKUP = "GetDataSymbolLookup";

       /// <summary>
       /// VerifySymbolLookup
       /// </summary>
       public const string VERIFY_SYMBOL_LOOKUP = "VerifySymbolLookup";

       /// <summary>
       /// The symbol lookup from tt
       /// </summary>
       public const string SYMBOL_LOOKUP_FROM_TT = "SymbolLookupFromTT";
       /// <summary>
       /// The update uda
       /// </summary>
       public const string UPDATE_UDA = "UpdateUDA";

       /// <summary>
       /// Add symbol
       /// </summary>
       public const string ADD_SYMBOL = "AddSymbol";

       /// <summary>
       /// Edit sm grid UI
       /// </summary>
       public const string EDIT_SM_GRID_UI = "EditSMGridUi";

       /// <summary>
       /// Select from sm grid ui step
       /// </summary>
       public const string SELECT_FROM_SM_GRID_UI = "SelectFromSMGridUI";

       /// <summary>
       /// Trade symbol from SM
       /// </summary>
       public const string TRADE_SYMBOL_FROM_SM = "TradeSymbolFromSM";

       /// <summary>
       /// Update security info on sm
       /// </summary>
       public const string UPDATE_SECURITY_INFO_ON_SM = "UpdateSecurityInfoOnSM";
       #endregion

       /// <summary>
       /// The expnl module
       /// </summary>
       public const string EXPNL = "Expnl";
       #region       
       /// <summary>
       /// The restart expnl
       /// </summary>
       public const string RESTART_EXPNL = "RestartExpnl";

       /// <summary>
       /// The refresh expnl UI
       /// </summary>
       public const string REFRESH_EXPNL_UI = "RefreshExpnlUi";

       /// <summary>
       /// The Update expnl config
       /// </summary>
       public const string UPDATE_EXPNL_CONFIG = "UpdateExpnlConfig";
       #endregion

       /// <summary>
       /// The pricing server module
       /// </summary>
       public const string PRICING_SERVER = "PricingServer";
       #region       
       /// <summary>
       /// The restart pricing server
       /// </summary>
       public const string RESTART_PRICING_SERVER = "RestartPricingServer";

       /// <summary>
       /// The update pricing config
       /// </summary>
       public const string UPDATE_PRICING_CONFIG = "UpdatePricingConfig";
       #endregion

       /// <summary>
       /// Remove daily cash step.
       /// </summary>
       public const string REMOVE_DAILY_CASH = "RemoveDailyCash";

        /// <summary>
       /// Get daily beta step.
       /// </summary>
       public const string GET_DAILY_BETA = "GetDailyBeta";
       
        /// <summary>
       /// Get daily CASH step.
       /// </summary>
       public const string GET_DAILY_CASH = "GetDailyCash";   
    
        /// <summary>
       /// Get daily delta step.
       /// </summary>
       public const string GET_DAILY_DELTA = "GetDailyDelta";  
       
       /// <summary>
       /// Get daily dividend yield step.
       /// </summary>
       public const string GET_DAILY_DIVIDEND_YIELD = "GetDailyDividendYield";
       
       /// <summary>
       /// Get daily outstandings step.
       /// </summary>
       public const string GET_DAILY_OUTSTANDINGS = "GetDailyOutstandings";
       
       /// <summary>
       /// Get daily trading volume step.
       /// </summary>
       public const string GET_DAILY_TRADING_VOLUME = "GetDailyTradingVolume";
       
       /// <summary>
       /// Get daily volatility step.
       /// </summary>
       public const string GET_DAILY_VOLATILITY = "GetDailyVolatility";
       
       /// <summary>
       /// Get forex conversion step.
       /// </summary>
       public const string GET_FOREX_CONVERSION = "GetForexConversion";
       
       /// <summary>
       /// Get mark price data step.
       /// </summary>
       public const string GET_MARK_PRICE_DATA = "GetMarkPriceData";
       
       /// <summary>
       /// Get get NAV step.
       /// </summary>
       public const string GET_NAV = "GetNAV";
       
       /// <summary>
       /// Get forwards points step.
       /// </summary>
       public const string GET_FORWARD_POINTS = "GetForwardPoints";
       
       /// <summary>
       /// Remove daily credit limit step.
       /// </summary>
       public const string REMOVE_DAILY_CREDIT_LIMIT = "RemoveDailyCreditLimit";

       /// <summary>
       /// Remove NAV step.
       /// </summary>
       public const string REMOVE_NAV = "RemoveNAV";

       /// <summary>
       /// Update beta step.
       /// </summary>
       public const string UPDATE_BETA = "UpdateBeta";

       /// <summary>
       /// Update daily cash step.
       /// </summary>
       public const string UPDATE_DAILY_CASH = "UpdateDailyCash";

        /// <summary>
       /// Update daily credit limit step.
       /// </summary>
       public const string UPDATE_DAILY_CREDIT_LIMIT = "UpdateDailyCreditLimt";

       /// <summary>
       /// Update daily delta step.
       /// </summary>
       public const string UPDATE_DAILY_DELTA = "UpdateDailyDelta";

       /// <summary>
       /// Update daily outstandings step.
       /// </summary>
       public const string UPDATE_DAILY_OUTSTANDINGS = "UpdateDailyOutstandings";

       /// <summary>
       /// Update daily volatility step.
       /// </summary>
       public const string UPDATE_DAILY_VOLATILITY = "UpdateDailyVolatility";

       /// <summary>
       /// Update dividend yield step.
       /// </summary>
       public const string UPDATE_DIVIDEND_YIELD = "UpdateDividendYield";

       /// <summary>
       /// Update forex converstion step.
       /// </summary>
       public const string UPDATE_FOREX_CONVERSTION = "UpdateForexConversion";

       /// <summary>
       /// Update forward points step.
       /// </summary>
       public const string UPDATE_FORWARD_POINTS = "UpdateForwardPoints";

       /// <summary>
       /// Update mark price step.
       /// </summary>
       public const string UPDATE_MARK_PRICE = "UpdateMarkPrice";

       /// <summary>
       /// Update NAV step.
       /// </summary>
       public const string UPDATE_NAV = "UpdateNAV";

       /// <summary>
       /// Update trading volumne step.
       /// </summary>
       public const string UPDATE_TRADING_VOLUME = "UpdateTradingVolume";

       /// <summary>
       /// Trading Ticket module.
       /// </summary>
       public const string TRADING_TICKET = "TradingTicket";

       /// <summary>
       /// Create done away step.
       /// </summary>
       public const string CREATE_DONE_AWAY_ORDER = "CreateDoneAwayOrder";

        /// <summary>
       /// Create stage order step.
       /// </summary>
       public const string CREATE_STAGE_ORDER = "CreateStageOrder";

        /// <summary>
       /// Enter details on TT  step.
       /// </summary>
       public const string ENTER_DETAILS_ON_TT = "EnterDetailsOnTT";

        /// <summary>
       /// Open TT from PST  step.
       /// </summary>
       public const string OPEN_TT_FROM_PST = "OpenTTFromPST";
       
       /// <summary>
       /// Set broker wie preferences TT  step.
       /// </summary>
       public const string SET_BROKER_WISE_PREFERENCES_TT = "SetBrokerWisePreferencesTT";

        /// <summary>
       /// Set general preference step.
       /// </summary>
       public const string SET_GENERAL_PREFERENCE = "SetGeneralPreference";

        /// <summary>
       /// Set UI preference step.
       /// </summary>
       public const string SET_UI_PREFERENCES = "SetUIPreferencesTT";    

        /// <summary>
       /// Trade new SUB step.
       /// </summary>
       public const string TRADE_NEW_SUB = "TradeNewSub";

       /// <summary>
       /// Validate symbol TT step.
       /// </summary>
       public const string VALIDATE_SYMBOL_TT = "ValidateSymbolTT";  

       /// <summary>
       /// Verify data TT step.
       /// </summary>
       public const string VERIFY_DATA_TT = "VerifyDataTT"; 

        /// <summary>
       /// Trade bloomberg step.
       /// </summary>
       public const string TRADE_BLOOMBERG = "TradeBloomberg";

       /// <summary>
       /// Click sysmbol lookup step.
       /// </summary>
       public const string CLICK_SYSMBOL_LOOK_UP = "ClickSymbolLookup";   

       /// <summary>
       /// create new sub order step.
       /// </summary>
       public const string CREATE_NEW_SUB_ORDER = "CreateNewSubOrder";  
 
       /// <summary>
       /// create replace order step.
       /// </summary>
       public const string CREATE_REPLACE_ORDER = "CreateReplaceOrder";     
       

       /// <summary>
       /// Blotter module.
       /// </summary>
       public const string BLOTTER = "Blotter";


       /// <summary>
       /// Refresh Blotter.
       /// </summary>
       public const string REFRESH_BLOTTER = "RefreshBlotter";

       /// <summary>
       /// Blotter functionalities step.
       /// </summary>
       public const string BLOTTER_FUNCTIONALITIES = "BlotterFunctionalities";

        /// <summary>
       /// Check blotter step.
       /// </summary>
       public const string CHECK_BLOTTER = "CheckBlotter";

        /// <summary>
       /// Edit add fils details step.
       /// </summary>
       public const string EDIT_ADD_FILLS_DETAILS = "EditAddFillsDetails";


       /// <summary>
       /// Edit order step.
       /// </summary>
       public const string EDIT_ORDERS = "EditOrder";

       /// <summary>
       /// Edit stage order step.
       /// </summary>
       public const string EDIT_STAGE_ORDERS = "EditStageOrder";

       /// <summary>
       /// Edit stage order step.
       /// </summary>
       public const string GET_EXECUTION_REPORT_ORDERS = "GetExecutionReportOrders";

       /// <summary>
       /// Remove stage order step.
       /// </summary>
       public const string REMOVE_STAGE_ORDER = "RemoveStageOrder";

       /// <summary>
       /// Replace stage order step.
       /// </summary>
       public const string REPLACE_STAGE_ORDER = "ReplaceStageOrder";

       /// <summary>
       /// Replace sub order step.
       /// </summary>
       public const string REPLACE_SUB_ORDER = "ReplaceSubOrder";

       /// <summary>
       /// Replace working subs step.
       /// </summary>
       public const string REPLACE_WORKING_SUBS = "ReplaceWorkingSubs";

       /// <summary>
       /// Select trade wrokings subs blotter step.
       /// </summary>
       public const string SELECT_TRADE_WORKING_SUBS_BLOTTER = "SelectTradeWorkingSubsBlotter";

       /// <summary>
       /// Select trade subs order blotter step.
       /// </summary>
       public const string SELECT_TRADE_SUB_ORDER_BLOTTER = "SelectTradeSubOrderBlotter";

       /// <summary>
       /// Trade new sub order step.
       /// </summary>
       public const string TRADE_NEW_SUB_ORDER = "TradeNewSubOrder";

       /// <summary>
       /// Transfer to the user order step.
       /// </summary>
       public const string TRANSFER_TO_USER_ORDER = "TransferToUserOrder";

       /// <summary>
       /// Transfer to the user sub-order step.
       /// </summary>
       public const string TRANSFER_TO_USER_SUB_ORDER = "TransferToUserSubOrder";

       /// <summary>
       /// Transfer to the user sub-order step.
       /// </summary>
       public const string TRANSFER_TO_USER_WORKING_SUB = "TransferToUserWorkingSubs";

       /// <summary>
       /// Verify alog details step.
       /// </summary>
       public const string VERIFY_ALGO_DETAILS = "VerifyAlgoDetails";

       /// <summary>
       /// Verify order step.
       /// </summary>
       public const string VERIFY_ORDER = "VerifyOrder";

       /// <summary>
       /// Verify sub-order step.
       /// </summary>
       public const string VERIFY_SUB_ORDER = "VerifySubOrder";

        /// <summary>
       /// Verify working sub step.
       /// </summary>
       public const string VERIFY_WORKING_SUBS = "VerifyWorkingSubs";

        /// <summary>
       /// view allocations details working sub step.
       /// </summary>
       public const string VIEW_ALLOCATIONS_DETAILS_WORKING_SUB = "ViewAllocationDetailsWorkingSub";

       /// <summary>
       /// The view allocation details sub order
       /// </summary>
       public const string VIEW_ALLOCATION_DETAILS_SUB_ORDER = "ViewAllocationDetailsSubOrder";

       /// <summary>
       /// Save layout order gird step.
       /// </summary>
       public const string SAVE_LAYOUT_ORDER_GRID = "SaveLayoutOrderGrid";

        /// <summary>
       /// Replace fills on blotter step.
       /// </summary>
       public const string REPLACE_FILLS_ON_BLOTTER = "ReplaceFillsOnBlotter";

        /// <summary>
       /// Remove selected orders step.
       /// </summary>
       public const string REMOVE_SELECTED_ORDERS = "RemoveSelectedOrders";

        /// <summary>
       /// Multi trading ticket step.
       /// </summary>
       public const string MULTI_TREADING_TICKET_FROM_BLOTTER = "MultiTradingTicketFromBlotter";

         /// <summary>
       /// Details for algo trade WS step.
       /// </summary>
       public const string DETAILS_FOR_ALGO_TRADE_WS = "DetailsForAlgoTradeWS";

        /// <summary>
       /// Details for algo trade order step.
       /// </summary>
       public const string DETAILS_FOR_ALGO_TRADE_ORDER = "DetailsForAlgoTradeOrder";

       /// <summary>
       /// Details for algo trade SUB-order step.
       /// </summary>
       public const string DETAILS_FOR_ALGO_TRADE_SUB_ORDER = "DetailsForAlgoSubOrders";

       
       /// <summary>
       /// Cancle SUB-order step.
       /// </summary>
       public const string CANCEL_SUB_ORDER = "CancelSubOrder";

       /// <summary>
       /// The cancel working subs
       /// </summary>
       public const string CANCEL_WORKING_SUBS = "CancelWorkingSubs";

       /// <summary>
       /// Cancle all subs step.
       /// </summary>
       public const string CANCLE_ALL_SUBS = "CancelAllSubs";

        /// <summary>
       /// Cancle all selected subs step.
       /// </summary>
       public const string CANCLE_ALL_SELECTED_SUBS = "CancelAllSelectedSubs";

       /// <summary>
       /// Blotter helper step.
       /// </summary>
       public const string BLOTTER_HELPER = "BlotterHelper";

       /// <summary>
       /// audit trail order blotter step.
       /// </summary>
       public const string AUDIT_TRAIL_ORDER_BLOTTER = "AuditTrailOrderBlotter";

       /// <summary>
       /// Add fills workings subs step.
       /// </summary>
       public const string ADD_FILL_WORKINGS_SUBS = "AddFillsWorkingSubs";

       /// <summary>
       /// Add fills sub order step.
       /// </summary>
       public const string ADD_FILLS_SUB_ORDERS = "AddFillsSubOrder";

       /// <summary>
       /// Select trade on order gird step.
       /// </summary>
       public const string SELECT_TRADE_ON_ORDER_GRID = "SelectTradeOnOrderGrid";
       
       /// <summary>
       /// General Ledger Module.
       /// </summary>
       public const string GENERAL_LEDGER = "GeneralLedger";

       /// <summary>
       /// Add cash transaction step.
       /// </summary>
       public const string ADD_CASH_TRANSACTION = "AddCashTransaction";
       
       /// <summary>
       /// Add non-trading transaction item step.
       /// </summary>
       public const string ADD_NON_TRADING_TRANSACTION_ITEM = "AddNonTradingTranItem";
       
       /// <summary>
       /// Add non-trading transaction  step.
       /// </summary>
       public const string ADD_NON_TRADING_TRANSACTION = "AddNonTradingTransaction";
       
       /// <summary>
       /// Add opening balance transaction item step.
       /// </summary>
       public const string ADD_OPENING_BALANCE_TRANSACTION_ITEM = "AddOpeningBalanceTranItem";
       
        /// <summary>
       /// Add opening balance transaction  step.
       /// </summary>
       public const string ADD_OPENING_BALANCE_TRANSACTION = "AddOpeningBalanceTransaction";       
       
        /// <summary>
       /// Calculate daily calculation step.
       /// </summary>
       public const string CALCULATE_DAILY_CALCULATION = "CalculateDailyCalculations";
       
       /// <summary>
       /// Calculate day end cash step.
       /// </summary>
       public const string CALCULATE_DAY_END_CASH = "CalculateDayEndCash";
       
        /// <summary>
       /// Close general ledger step.
       /// </summary>
       public const string CLOSE_GENERAL_LEDGER = "CloseGeneralLedger";       
       
        /// <summary>
       /// Delete cash transaction step.
       /// </summary>
       public const string DELETE_CASH_TRANSACTION = "DeleteCashTransaction";
       
        /// <summary>
       /// Delete non trading transaction item step.
       /// </summary>
       public const string DELETE_NON_TRADING_TRANSACTION_ITEM = "DeleteNonTradingTranItem";
       
        /// <summary>
       /// Delete non trading transaction step.
       /// </summary>
       public const string DELETE_NON_TRADING_TRANSACTION = "DeleteNonTradingTransaction";
       
        /// <summary>
       /// Delete opening balance trasaction item step.
       /// </summary>
       public const string DELETE_OPENING_BALANCE_TRANSACTION_ITEM = "DeleteOpeningBalanceTranItem";
       
       /// <summary>
       /// Delete opening balance trasaction step.
       /// </summary>
       public const string DELETE_OPENING_BALANCE_TRANSACTION = "DeleteOpeningBalanceTransaction";
       
        /// <summary>
       /// Edit cash trasaction step.
       /// </summary>
       public const string EDIT_CASH_TRANSACTION = "EditCashTransaction";
       
        /// <summary>
       /// Edit non trading trasaction item step.
       /// </summary>
       public const string EDIT_NON_TRADING_TRANSACTION_ITEM = "EditNonTradingTranItem";
       
       /// <summary>
       /// Edit non trading trasaction step.
       /// </summary>
       public const string EDIT_NON_TRADING_TRANSACTION = "EditNonTradingTransaction";
       
        /// <summary>
       /// Edit opening balance trasaction item step.
       /// </summary>
       public const string EDIT_OPENIGN_BALANCE_TRANSACTION_ITEM = "EditOpeningBalanceTranItem";
       
        /// <summary>
       /// Edit opening balance trasaction step.
       /// </summary>
       public const string EDIT_OPENIGN_BALANCE_TRANSACTION = "EditOpeningBalanceTransaction";
       
         /// <summary>
       /// Get account balance step.
       /// </summary>
       public const string GET_ACCOUNT_BALANCE = "GetAccountBalance";
       
         /// <summary>
       /// Get account details step.
       /// </summary>
       public const string GET_ACCOUNT_DETAILS = "GetAccountDetails";
       
        /// <summary>
       /// Get activity data step.
       /// </summary>
       public const string GET_ACTIVITY_DATA = "GetActivityData";
       
       /// <summary>
       /// Get cash trasanction step.
       /// </summary>
       public const string GET_CASH_TRANSACTION = "GetCashTransaction";
       
       /// <summary>
       /// Get daily calculation data step.
       /// </summary>
       public const string GET_DAILY_CALCULATION_DATA = "GetDailyCalculationsData";
       
       /// <summary>
       /// Get day end cash step.
       /// </summary>
       public const string GET_DAY_END_CASH = "GetDayEndCash";
       
       /// <summary>
       /// Get dividend data step.
       /// </summary>
       public const string GET_DIVIDEND_DATA = "GetDividendData";
       
        /// <summary>
       /// Get non trading transaction step.
       /// </summary>
       public const string GET_NON_TRADING_TRANSACTION = "GetNonTradingTransaction";
       
        /// <summary>
       /// Get opening balance data step.
       /// </summary>
       public const string GET_OPENING_BALANCE_DATA = "GetOpeningBalanceData";
       
        /// <summary>
       /// Get revaluation data step.
       /// </summary>
       public const string GET_REVALUATION_DATA = "GetRevaluationData";
       
       
       /// <summary>
       /// Get trading transaction step.
       /// </summary>
       public const string GET_TRADING_TRANSACTION = "GetTradingTransaction";
       
       /// <summary>
       /// Run revaluation step.
       /// </summary>
       public const string RUN_REVALUATION = "RunRevaluation";
       
       /// <summary>
       /// Select cash transaction step.
       /// </summary>
       public const string SELECT_CASH_TRANSACTION = "SelectCashTransaction";
       
       /// <summary>
       /// Select non trading transaction item step.
       /// </summary>
       public const string SELECT_NON_TRADING_TRANSACTION_ITEM = "SelectNonTradingTranItem";
       
       /// <summary>
       /// Select non trading transaction step.
       /// </summary>
       public const string SELECT_NON_TRADING_TRANSACTION = "SelectNonTradingTransaction";
       
       /// <summary>
       /// Select opening balacne transaction item step.
       /// </summary>
       public const string SELECT_OPENNING_BALANCE_TRANSACTION_ITEM = "SelectOpeningBalanceTranItem";
       
       /// <summary>
       /// Select opening balacne transaction step.
       /// </summary>
       public const string SELECT_OPENNING_BALANCE_TRANSACTION = "SelectOpeningBalanceTransaction";
       
       /// <summary>
       /// Verify account details step.
       /// </summary>
       public const string VERIFY_ACCOUNT_DETAILS = "VerifyAccountDetails";
       
         /// <summary>
       /// Verify activity step.
       /// </summary>
       public const string VERIFY_ACTIVITY = "VerifyActivity";
       
       /// <summary>
       /// Verify daily cal transaction step.
       /// </summary>
       public const string VERIFY_DAILY_CAL_TRANSACTION = "VerifyDailyCalTransactions";
       
       /// <summary>
       /// Verify day end cash step.
       /// </summary>
       public const string VERIFY_DAY_END_CASH = "VerifyDayEndCash";
       
       /// <summary>
       /// Verify dividend step.
       /// </summary>
       public const string VERIFY_DIVIDEND = "VerifyDividend";
       
       /// <summary>
       /// Verify non trading transaction step.
       /// </summary>
       public const string VERIFY_NON_TRADING_TRANSACTION = "VerifyNonTradingTransaction";
       
       /// <summary>
       /// Verify openign balance data step.
       /// </summary>
       public const string VERIFY_OPENING_BALANCE_DATA = "VerifyOpeningBalanceData";
       
       /// <summary>
       /// Verify revaluatain data step.
       /// </summary>
       public const string VERIFY_REVALUATION_DATA = "VerifyRevaluationData";
       
       /// <summary>
       /// Verify  trading transaction step.
       /// </summary>
       public const string VERIFY_TRADING_TRANSACTION= "VerifyTradingTransaction";

       /// <summary>
       /// Add Multiple Opening BalTran Item step.
       /// </summary>
       public const string ADD_MULTIPLE_OPENING_BAL_TRAN_ITEM = "AddMultipleOpeningBalTranItem";

       /// <summary>
       /// Verify Account Balance step.
       /// </summary>
       public const string VERIFY_ACCOUNT_BALANCES = "VerifyAccountBalances";

       /// <summary>
       /// Verify cash transactions step.
       /// </summary>
       public const string VERIFY_CASH_TRANSACTION = "VerifyCashTransaction";

       /// <summary>
       ///  PTT module.
       /// </summary>
       public const string PTT = "PTT";

       /// <summary>
       /// Calculate PTT .
       /// </summary>
       public const string CALCULATE_PTT = "CalculatePTT";

       /// <summary>
       /// Set Preferences of PTT (Default Preference-Input Parameters)
       /// </summary>
       public const string SET_PTT_PREFERENCE = "SetPTTPreference";

       /// <summary>
       /// Set Long Short Master Fund/AccountPTT Preference 
       /// </summary>
       public const string SET_LONGSHORT_PTT_PREFERENCE = "SetLongShortPTTPreference";

       /// <summary>
       /// Calculate PTT .
       /// </summary>
       public const string VERIFY_PTT = "VerifyPTT";

       /// <summary>
       /// Set account factor in PTT.
       /// </summary>
       public const string SET_ACCOUNT_FACTOR = "SetAccountFactor";

        /// <summary>
       /// Trade through PTT .
       /// </summary>
       public const string TRADE_PTT = "TradePTT";

        /// <summary>
       /// Create stage through PTT .
       /// </summary>
       public const string CREATE_ORDER_PTT = "CreateOrderPTT";

       /// <summary>
       /// Multi TT Module.
       /// </summary>
       public const string MULTI_TRADING_TICKET = "MultiTradingTicket";

       /// <summary>
       /// Done away using  multi TT.
       /// </summary>
       public const string DONE_AWAY_USING_MTT = "DoneAwayUsingMTT";

       /// <summary>
       /// Edit trade using MTT.
       /// </summary>
       public const string EDIT_TRADE_USING_MTT = "EditTradeUsingMTT";

       /// <summary>
       /// Select trade using MTT.
       /// </summary>
       public const string SELECT_TRADE_USING_MTT = "SelectTradeUsingMTT";       

       /// <summary>
       /// Send order using MTT.
       /// </summary>
       public const string SEND_ORDER_USING_MTT = "SendOrderUsingMTT";

       /// <summary>
       /// Get price using MTT using MTT.
       /// </summary>
       public const string GET_PRICE_USING_MTT = "GetPriceUsingMTT"; 

       /// <summary>
       /// Bulk change trade donw away  using MTT.
       /// </summary>
       public const string BULK_CHANGE_TRADE_DONE_AWAY = "BulkChangeTradeDoneAway";

       /// <summary>
       /// Clean up test id .
       /// </summary>
       public const string CLEAN_UP_0000 = "CleanUp-0000";

        /// <summary>
       /// Clean up module .
       /// </summary>
       public const string CLEAN_UP_MODULE = "CleanUp";

       /// <summary>
       /// Config
       /// </summary>
       public const string CONFIG = "Config";

        /// <summary>
       /// Delete client data.
       /// </summary>
       public const string DELETE_CLIENT_DATA = "DeleteClientData";
       /// <summary>
       /// Create Live Order.
       /// </summary>
       public const string CREATE_LIVE_ORDER = "CreateLiveOrder";

       /// <summary>
       /// The prana client
       /// </summary>
       public const string PRANA_CLIENT = "PranaClient";

       /// <summary>
       /// The trade server
       /// </summary>
       public const string TRADE_SERVER = "TradeServer";

       /// <summary>
       /// The Save layout
       /// </summary>
       public const string SAVE_LAYOUT_ALLOCATEDGRID = "SaveLayoutAllocatedGrid";

       /// <summary>
       /// The Save layout
       /// </summary>
       public const string SAVE_LAYOUT_UNALLOCATEDGRID = "SaveLayoutUnallocatedGrid";

       /// <summary>
       /// The Clear Filters
       /// </summary>
       public const string CLEAR_FILTERS_ALLOCATEDGRID = "ClearFiltersAllocatedGrid";

       /// <summary>
       /// The Clear Filters
       /// </summary>
       public const string CLEAR_FILTERS_UNALLOCATEDGRID = "ClearFiltersUnallocatedGrid";

       /// <summary>
       /// The Expand Collapse
       /// </summary>
       public const string EXPAND_COLLAPSE_ALLOCATEDGRID = "ExpandCollapseAllocatedGrid";

       /// <summary>
       /// The Expand Collapse
       /// </summary>
       public const string EXPAND_COLLAPSE_UNALLOCATEDGRID = "ExpandCollapseUnallocatedGrid";
       
	   /// <summary>
       /// Verify Save Layout Allocation Grid
       /// </summary>
       public const string VERIFY_LAYOUT_ALLOCATEDGRID = "VerifyLayoutAllocatedGrid";
        
       /// <summary>
       /// Verify Save Layout UnAllocated Grid
       /// </summary> 
       public const string VERIFY_LAYOUT_UNALLOCATEDGRID = "VerifyLayoutUnallocatedGrid";
	  
       /// <summary>
       /// Verify TT step.
       /// </summary>
       public const string VERIFY_TT = "VerifyTT";

       /// <summary>
       /// For Login into Client Apllication (PranaClient Module)
       /// </summary>
       public const string LOGIN_CLIENT = "LoginClient";

       /// <summary>
       /// For Restarting Prana Client (PranaClient Module)
       /// </summary>
       public const string RESTART_CLIENT="RestartClient";

       /// <summary>
       /// For Closing the Client Application (PranaClient Module)
       /// </summary>
       public const string CLOSE_CLIENT = "CloseClient";
      
       /// <summary>
       ///Save Layout  Allocation UI
       /// </summary> 
       public const string SAVE_LAYOUT_ALLOCATION_UI = "SaveLayoutAllocationUI";

       /// <summary>
       ///Adding New Tab in Blotter
       /// </summary> 
       public const string ADD_TAB = "AddTab";

       /// <summary>
       ///
       /// Verify Tab in Blotter
       /// </summary> 
       public const string VERIFY_TAB = "VerifyTab";

       /// <summary>
       ///
       /// Remove Tab in Blotter
       /// </summary> 
       public const string REMOVE_TAB = "RemoveTab";

       /// <summary>
       ///
       /// Rename Tab in Blotter
       /// </summary> 
       public const string RENAME_TAB = "RenameBlotterTab";
       
       /// <summary>
       ///
       /// Save the Layout of Blotter
       /// </summary> 
       public const string SAVE_LAYOUT_BLOTTER = "SaveLayoutBlotter";

       /// <summary>
       ///
       /// Export data from blotter to excel
       /// </summary> 
       public const string EXPORT_TO_EXCEL = "ExportToExcel";

       /// <summary>
       ///
       /// Verify the MTT data
       /// </summary> 
       public const string VERIFY_MTT="VerifyMTT";

       /// <summary>
       ///
       /// Run Manual Revaluation from General ledger
       /// </summary> 
       public const string RUN_MANUAL_REVALUATION = "RunManualRevaluation";

       /// <summary>
       ///
       /// Verify Daily Cash from Daily Valuation
       /// </summary> 
       public const string VERIFY_DAILY_CASH = "VerifyDailyCash";
      
       /// <summary>
       /// Edit Unallocated Grid
       /// </summary>
       public const string EDIT_UNALLOCATED_GRID = "EditUnallocatedGrid";

       /// <summary>
       /// Edit Allocated Grid
       /// </summary>
       public const string EDIT_ALLOCATED_GRID = "EditAllocatedGrid";

       /// <summary>
       /// Edit Unallocated Grid
       /// </summary>
       public const string EDIT_UNALLOCATED_GRID_WPF = "EditUnallocatedGridWPF";

       /// <summary>
       /// Edit Allocated Grid
       /// </summary>
       public const string EDIT_ALLOCATED_GRID_WPF = "EditAllocatedGridWPF";

       /// <summary>
       /// Edit Allocated Grid
       /// </summary>
       public const string EDIT_ALLOCATED_GRID_TAXLOT = "EditAllocatedGridTaxlot";
      
       /// <summary>
       ///
       /// Update MarkPrice Monthly From Daily Valuation
       /// </summary> 
       public const string UPDATE_MARKPRICE_MONTHLY = "UpdateMarkPriceMonthly";

       
       /// <summary>
       ///
       /// Select the PTT record
       /// </summary> 
       public const string SELECT_PTT_RECORD = "SelectPTTRecord";

       /// <summary>
       ///
       /// Edit the PTT Record
       /// </summary> 
       public const string EDIT_PTT_RECORD = "EditPTTRecord";

       /// <summary>
       ///
       /// Take Screenshot of blotter execution report
       /// </summary> 
       public const string BLOTTER_EXECUTION_REPORT_SNAPSHOT = "BlotterExecutionReportSnapshot";

       /// <summary>
       ///
       /// Get Execution Report Details
       /// </summary> 
       public const string GET_EXECUTION_REPORT_DETAILS = "GetExecutionReportDetails";

       /// <summary>
       ///
       /// Export Execution Report Data
       /// </summary> 
       public const string EXPORT_EXECUTION_REPORT_DATA = "ExportExecutionReportData";

       /// <summary>
       ///
       /// verify execution report data
       /// </summary> 
       public const string VERIFY_EXECUTION_REPORT = "VerifyExecutionReport";

       /// <summary>
       /// Compliance Module
       /// </summary>
       public const string COMPLIANCE= "Compliance";

       /// <summary>
       /// Check Compliance step
       /// </summary>
       public const string CHECK_COMPLIANCE="CheckCompliance";

       /// <summary>
       /// Restart Esper step
       /// </summary>
       public const string RESTART_ESPER = "RestartEsper";

       /// <summary>
       /// Verify Account Divisor Window step
       /// </summary>
       public const string VERIFY_ACCOUNT_DIVISOR_WINDOW="VerifyAccountDivisorWindow";

       /// <summary>
       /// Verify Account Nav Preference Window step
       /// </summary>
       public const string VERIFY_ACCOUNT_NAV_PREFERENCE_WINDOW = "VerifyAccountNavPreferenceWindow";

       /// <summary>
       /// Verify Account Symbol Divisor Window step
       /// </summary>
       public const string VERIFY_ACCOUNT_SYMBOL_DIVISOR_WINDOW = "VerifyAccountSymbolDivisorWindow";
       
       /// <summary>
       /// Verify Aggregation Account Symbol WithNav
       /// </summary>
       public const string VERIFY_AGGREGATION_ACCOUNT_SYMBOL_WITH_NAV = "VerifyAggregationAccountSymbolWithNav";

       /// <summary>
       /// Verify Accrual For Account Window step
       /// </summary>
       public const string VERIFY_ACCRUAL_FOR_ACCOUNT_WINDOW = "VerifyAccrualForAccountWindow";

       /// <summary>
       /// Verify Aggregation Account Underlying Symbol With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_ACCOUNT_UNDERLYING_SYMBOL_WITH_NAV = "VerifyAggregationAccountUnderlyingSymbolWithNav";

       /// <summary>
       /// Verify Aggregation Account With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_ACCOUNT_WITH_NAV = "VerifyAggregationAccountWithNav";

       /// <summary>
       /// Verify Aggregation Asset With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_ASSET_WITH_NAV = "VerifyAggregationAssetWithNav";

       /// <summary>
       /// Verify Aggregation Global With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_GLOBAL_WITH_NAV = "VerifyAggregationGlobalWithNav";

       /// <summary>
       /// Verify Aggregation Master Fund Symbol With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_MASTER_FUND_SYMBOL_WITH_NAV = "VerifyAggregationMasterFundSymbolWithNav";

       /// <summary>
       /// Verify Account Nav Preference Window step
       /// </summary>
       public const string VERIFY_AGGREGATION_MASTER_FUND_UNDERLYING_SYMBOL_WITH_NAV = "VerifyAggregationMasterFundUnderlyingSymbolWithNav";

       /// <summary>
       /// Verify Aggregation Master Fund With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_MASTER_FUND_WITH_NAV = "VerifyAggregationMasterFundWithNav";

       /// <summary>
       /// Verify Aggregation Sector With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_SECTOR_WITH_NAV = "VerifyAggregationSectorWithNav";

       /// <summary>
       /// Verify Aggregation Sub Sector With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_SUB_SECTOR_WITH_NAV = "VerifyAggregationSubSectorWithNav";

       /// <summary>
       /// Verify Aggregation Symbol With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_SYMBOL_WITH_NAV = "VerifyAggregationSymbolWithNav";

       /// <summary>
       /// Verify Aggregation Underlying Symbol With Nav step
       /// </summary>
       public const string VERIFY_AGGREGATION_UNDERLYING_SYMBOL_WITH_NAV = "VerifyAggregationUnderlyingSymbolWithNav";

       /// <summary>
       /// Verify Auec Window step
       /// </summary>
       public const string VERIFY_AUEC_WINDOW = "VerifyAuecWindow";

       /// <summary>
       /// Verify Beta Window step
       /// </summary>
       public const string VERIFY_BETA_WINDOW = "VerifyBetaWindow";

       /// <summary>
       /// Verify Custom Symbol Data Window step
       /// </summary>
       public const string VERIFY_CUSTOM_SYMBOL_DATA_WINDOW = "VerifyCustomSymbolDataWindow";

       /// <summary>
       /// Verify Day End Cash Account Window step
       /// </summary>
       public const string VERIFY_DAY_END_CASH_ACCOUNT_WINDOW = "VerifyDayEndCashAccountWindow";

       /// <summary>
       /// Verify Db Nav Window step
       /// </summary>
       public const string VERIFY_DB_NAV_WINDOW = "VerifyDbNavWindow";

       /// <summary>
       /// Verify Global Divisor Window
       /// </summary>
       public const string VERIFY_GLOBAL_DIVISOR_WINDOW = "VerifyGlobalDivisorWindow";

       /// <summary>
       /// Verify Master Fund Divisor Window step
       /// </summary>
       public const string VERIFY_MASTER_FUND_DIVISOR_WINDOW = "VerifyMasterFundDivisorWindow";

       /// <summary>
       /// Verify Master Fund Symbol Divisor Window step
       /// </summary>
       public const string VERIFY_MASTER_FUND_SYMBOL_DIVISOR_WINDOW = "VerifyMasterFundSymbolDivisorWindow";

       /// <summary>
       /// Verify Pm Calculation Preference Window step
       /// </summary>
       public const string VERIFY_PM_CALCULATION_PREFERENCE_WINDOW = "VerifyPmCalculationPreferenceWindow";

       /// <summary>
       /// Verify Row Calculation Base Window
       /// </summary>
       public const string VERIFY_ROW_CALCULATION_BASE_WINDOW = "VerifyRowCalculationBaseWindow";

       /// <summary>
       /// Verify Security Window
       /// </summary>
       public const string VERIFY_SECURITY_WINDOW = "VerifySecurityWindow";

       /// <summary>
       /// Verify Symbol Data Window
       /// </summary>
       public const string VERIFY_SYMBOL_DATA_WINDOW = "VerifySymbolDataWindow";

       /// <summary>
       /// Verify Symbol Divisor Window
       /// </summary>
       public const string VERIFY_SYMBOL_DIVISOR_WINDOW = "VerifySymbolDivisorWindow";

       /// <summary>
       /// Verify Taxlot Window
       /// </summary>
       public const string VERIFY_TAXLOT_WINDOW = "VerifyTaxlotWindow";

       /// <summary>
       /// Approve or reject trades 
       /// </summary>
       public const string APPROVE_OR_REJECT_PENDING_APPROVAL = "ApproveorRejectpendingapproval";

       /// <summary>
       /// Edit Interceptor file
       /// </summary>
       public const string EDIT_INTERCEPTOR_FILE = "EditInterceptorFile";

       /// <summary>
       /// Get Email
       /// </summary>
       public const string GET_EMAIL = "GetEmail";

       /// <summary>
       /// Replace Interceptor File
       /// </summary>
       public const string REPLACE_INTERCEPTOR_FILE = "ReplaceInterceptorFile";

       /// <summary>
       /// Restart Releases and services
       /// </summary>
       public const string RESTART_RELEASES_AND_SERVICES = "RestartReleasesAndServices";

       /// <summary>
       /// Verify Alert History
       /// </summary>
       public const string VERIFY_ALERT_HISTORY = "VerifyAlertHistory";

       /// <summary>
       ///Rebalance module 
       /// </summary>
       public const string REBALANCER = "Rebalancer";

       /// <summary>
       /// Fetch Account Positions
       /// </summary>
       public const string FETCH_ACCOUNT_POSITIONS = "FetchAccountPositions";

       /// <summary>
       /// Add Cash
       /// </summary>
       public const string ADD_CASH = "AddCash";

       /// <summary>
       /// Run Rebalance
       /// </summary>
       public const string RUN_REBALANCE = "RunRebalance";

       /// <summary>
       /// Verify Grid Data
       /// </summary>
       public const string VERIFY_GRID_DATA = "VerifyGridData";

       /// <summary>
       /// Add symbol through Rebalance Across securities
       /// </summary>
       public const string REBALANCE_ACROSS_SECURITIES = "RebalanceAcrossSecurities";

       /// <summary>
       /// Disable allocation checkside preferences
       /// </summary>
       public const string DISABLE_CHECKSIDE_PREFERENCES = "DisableChecksidePreferences";

       /// <summary>
       /// Disable allocation checkside
       /// </summary>
       public const string DISABLE_CHECKSIDE = "DisableCheckside";

       /// <summary>
       /// Disable allocation checkside for allocation
       /// </summary>
       public const string DISABLE_CHECKSIDE_ACCOUNT = "DisableChecksideForAccount";

       /// <summary>
       /// Disable allocation checkside for asset
       /// </summary>
       public const string DISABLE_CHECKSIDE_ASSET = "DisableChecksideForAsset";

       /// <summary>
       /// Disable allocation checkside for counterparty
       /// </summary>
       public const string DISABLE_CHECKSIDE_COUNTERPARTY = "DisableChecksideForCounterParty";

       /// <summary>
       /// The add calculated preference toolbar
       /// </summary>
       public const string CHECK_PREFERENCES = "CheckPreferences";
       /// <summary>
       /// RestartServer
       /// </summary>     
       public const string RESTART_SERVER = "RestartServer";

       /// <summary>
       /// RestartServerOnly
       /// </summary>     
       public const string RESTART_SERVERONLY = "RestartServerOnly";

       /// <summary>
       /// Genera rule preferences
       /// </summary>     
       public const string UPDATE_GENERAL_RULE_PREF = "UpdateGeneralRulePref";

       /// <summary>
       /// general account value
       /// </summary>     
       public const string UPDATE_GRACCOUNT_VALUE = "UpdateGRAccountValue";

       /// <summary>
       /// General default rule
       /// </summary>     
       public const string UPDATE_GRDEFAULT_RULE = "UpdateGRDefaultRule";

       /// <summary>
       /// General Strategy value
       /// </summary>     
       public const string UPDATE_GRSTRATEGY_VALUE = "UpdateGRStrategyValue";

       /// <summary>
       /// Add Tab on the Blotter
       /// </summary>     
       public const string ADD_TAB_BLOTTER = "AddTabBlotter";

       /// <summary>
       /// Allocate orders from Blotter
       /// </summary>     
       public const string ALLOCATE_FROM_BLOTTER = "AllocateFromBlotter";

       /// <summary>
       /// Allocate orders from WorkingSubsBlotter
       /// </summary>     
       public const string ALLOCATE_FROM_WORKING_SUBS_BLOTTER = "AllocateFromWorkingSubsBlotter";

       /// <summary>
       /// Cancel Summary step
       /// </summary>     
       public const string CANCEL_FROM_SUMMARY = "CancelFromSummary";

       /// <summary>
       /// Open Allocation from Blotter
       /// </summary>     
       public const string GO_TO_ALLOCATION = "GoToAllocation";

       /// <summary>
       /// Merge Orders
       /// </summary>     
       public const string MERGE_ORDER = "MergeOrder";

       /// <summary>
       /// Multiple Trade Transfer to User
       /// </summary>     
       public const string MULTIPLE_TRADE_TRANSFER_TO_USER = "MultipleTradeTransferToUser";

       /// <summary>
       /// Open Blotter
       /// </summary>     
       public const string OPEN_BLOTTER = "OpenBlotter";

       /// <summary>
       /// Reallocate from Blotter by Allocate
       /// </summary>     
       public const string REALLOCATE_FROM_BLOTTER_BY_ALLOCATE = "ReAllocateFromBlotterByAllocate";

       /// <summary>
       /// Reload From Summary
       /// </summary>     
       public const string RELOAD_FROM_SUMMARY = "ReloadFromSummary";

       /// <summary>
       /// Reload order from blotter
       /// </summary>     
       public const string RELOAD_ORDER_FROM_BLOTTER = "ReloadOrderFromBlotter";

       /// <summary>
       /// Reload order from suborder
       /// </summary>     
       public const string RELOAD_ORDER_FROM_SUBORDER = "ReloadOrderFromSubOrder";

       /// <summary>
       /// Reload order from working subs
       /// </summary>     
       public const string RELOAD_ORDER_FROM_WORKING_SUBS = "ReloadOrderFromWorkingSubs";

       /// <summary>
       /// Remove Execution Working Subs
       /// </summary>     
       public const string REMOVE_EXECUTION_WORKINGSUBS = "RemoveExecutionWorkingSubs";

       /// <summary>
       /// Remove Manual Execution
       /// </summary>     
       public const string REMOVE_MANUAL_EXECUTION = "RemoveManualExecution";

       /// <summary>
       /// Remove Order From Custom Tab 
       /// </summary>     
       public const string REMOVE_ORDER_FROM_CUSTOM_TAB = "RemoveOrderFromCustomTab";

       /// <summary>
       /// Save or removein summary tab
       /// </summary>     
       public const string SAVE_OR_REMOVE_SUMMARY_TAB = "SaveOrRemoveInSummaryTab";

       /// <summary>
       /// Save or removein workingsubs tab
       /// </summary>     
       public const string SAVE_OR_REMOVE_WORKINGSUBS_TAB = "SaveOrRemoveInWorkingSubsTab";

       /// <summary>
       /// Select check box on custom tab
       /// </summary>     
       public const string SELECT_CHECK_BOX_ON_CUSTOM_TAB = "SelectCheckBoxOnCustomTab";

       /// <summary>
       /// Select check box on order grid
       /// </summary>     
       public const string SELECT_CHECK_BOX_ON_ORDER_GRID = "SelectCheckBoxOnOrderGrid";

       /// <summary>
       /// Upload stage order on blotter
       /// </summary>     
       public const string UPLOAD_STAGE_ORDER = "UploadStageOrder";

       /// <summary>
       /// Verify Audit Trail step
       /// </summary>     
       public const string VERIFY_AUDIT_TRAIL = "VerifyAuditTrail";

       /// <summary>
       /// Verify custom tabs
       /// </summary>     
       public const string VERIFY_CUSTOM_TABS = "VerifyCustomTabs";

       /// <summary>
       /// Verify lower strip of blotter ui
       /// </summary>     
       public const string VERIFY_LOWER_STRIP_BLOTTERUI = "VerifyLowerStripBlotterUI";

       /// <summary>
       /// Verify merge orders
       /// </summary>     
       public const string VERIFY_MERGE_ORDER = "VerifyMergeOrder";

       /// <summary>
       /// Verify rollover order 
       /// </summary>     
       public const string VERIFY_ROLLOVER = "VerifyRollover";

       /// <summary>
       /// Verify suborder after rollover order 
       /// </summary>     
       public const string VERIFY_SUBORDER_AFTER_ROLLOVER = "VerifySubOrderAfterRollover";

       /// <summary>
       /// Verify view allocation details
       /// </summary>     
       public const string VERIFY_VIEW_ALLOCATION_DETAILS = "VerifyViewAllocationDetails";

       /// <summary>
       /// Verify view allocation orders
       /// </summary>     
       public const string VERIFY_VIEW_ALLOCATION_ORDER = "VerifyViewAllocationOrder";

       /// <summary>
       /// View PTT Allocation Details
       /// </summary>     
       public const string VIEW_PTT_ALLOCATION_DETAILS = "ViewPTTAllocationDetails";

        /// <summary>
        /// Module Simulator
        /// </summary> 
       public const string CAMERON_SIMULATOR = "CameronSimulator";

       /// <summary>
       /// Acknowledge Trade
       /// </summary>    
       public const string ACKNOWLEDGE_TRADE = "AcknowledgeTrade";

       /// <summary>
       /// Cancel Trade
       /// </summary>    
       public const string CANCEL_TRADE = "CancelTrade";

       /// <summary>
       /// Clear UI of Simulator
       /// </summary>    
       public const string CLEAR_UI = "ClearUi";

       /// <summary>
       /// Done for the Day step
       /// </summary>    
       public const string DONE_FOR_DAY = "DoneForDay";

       /// <summary>
       /// Execute Trade
       /// </summary>    
       public const string EXECUTE_TRADE = "ExecuteTrade";

       /// <summary>
       /// Open Live TT
       /// </summary>    
       public const string OPEN_LIVE_TT = "OpenLiveTT";

       /// <summary>
       /// Select Trade
       /// </summary>    
       public const string SELECT_TRADE = "SelectTrade";

       /// <summary>
       /// Reject Trade
       /// </summary>    
       public const string REJECT_TRADE = "RejectTrade";

       /// <summary>
       /// Set Manual Response
       /// </summary>    
       public const string SET_MANUAL_RESPONSE = "SetManualResponse";

       /// <summary>
       /// Set to automatic response
       /// </summary>    
       public const string SET_TO_AUTOMATIC_RESPONSE = "SetToAutomaticResponse";

       /// <summary>
       /// verify tag in all fixlogs
       /// </summary>    
       public const string VERIFY_TAG_IN_ALL_FIXLOGS = "VerifyTagInAllFixLogs";

       /// <summary>
       /// verify trades on simulator
       /// </summary>    
       public const string VERIFY_TRADES = "VerifyTrades";

       /// <summary>
       /// Add Duplicate Row
       /// </summary>    
       public const string ADD_DUPLICATE_ROW = "AddDuplicateRow";

       /// <summary>
       /// Auto Exercise
       /// </summary>    
       public const string AUTO_EXERCISE = "AutoExcercise";

       /// <summary>
       /// Edit Closing Transaction
       /// </summary>    
       public const string EDIT_CLOSING_TRANSACTION = "EditClosingTransactions";

       /// <summary>
       /// Edit Trade Unexpired Unsettled
       /// </summary>    
       public const string EDIT_TRADE_UNEXPIRED_UNSETTLED = "EditTradeUnexpiredUnsettled";

       /// <summary>
       /// Exercise Assignment with out save
       /// </summary>    
       public const string EXERCISE_ASSIGNMENT_WITH_OUT_SAVE = "ExerciseAssignmentWithOutSave";

       /// <summary>
       /// Save create transactions
       /// </summary>    
       public const string SAVE_CT_DATA = "SaveCTData";

       /// <summary>
       /// Select Account wise asset
       /// </summary>    
       public const string SELECT_ACCOUNT_WISE_ASSET = "SelectAssetWiseAsset";

       /// <summary>
       /// Select Create Transaction
       /// </summary>    
       public const string SELECT_CREATE_TRANSACTION = "SelectCreateTransaction";

       /// <summary>
       /// Verify CT Data
       /// </summary>    
       public const string VERIFY_CT_DATA = "VerifyCTData";

       /// <summary>
       /// Open Spin Off
       /// </summary>    
       public const string OPEN_SPIN_OFF = "OpenSpinOff";

       /// <summary>
       /// Spin Off
       /// </summary>    
       public const string SPIN_OFF = "SpinOff";

       /// <summary>
       /// Split CA
       /// </summary>    
       public const string SPLIT = "Split";

       /// <summary>
       /// Verify and apply corporate action
       /// </summary>    
       public const string VERIFY_AND_APPLY_CORP_ACTION = "VerifyAndAppplyCorpAction";

       /// <summary>
       /// Verify and Save corporate action
       /// </summary>    
       public const string VERIFY_AND_SAVE_CORP_ACTION = "VerifyAndSaveCorpAction";

       /// <summary>
       /// Enable Blank Mark Prices
       /// </summary>    
       public const string ENABLE_BLANK_MARK_PRICES = "EnableBlankMarkPrices";

       /// <summary>
       /// Filter Blank Mark Prices
       /// </summary>    
       public const string FILTER_BLANK_MP = "FilterBlankMP";

       /// <summary>
       /// Update Symbol Blank MP
       /// </summary>    
       public const string UPDATE_SYMBOL_BLANK_MP = "UpdateSymbolBlankMP";

       /// <summary>
       /// Update Blank BP
       /// </summary>    
       public const string UPDATE_BLANK_BP = "UpdateBlankBP";

       /// <summary>
       /// Update Multiple Daily Cash
       /// </summary>    
       public const string UPDATE_MULTIPLE_DAILY_CASH = "UpdateMultipleDailyCash";

       /// <summary>
       /// Module DropCopy
       /// </summary>    
       public const string DROPCOPY = "Dropcopy";

       /// <summary>
       /// Upload DropCopy File
       /// </summary>    
       public const string UPLOAD_DROPCOPY_FILE = "UploadDropcopyFile";

       /// <summary>
       /// Add or remove column
       /// </summary>    
       public const string ADD_OR_REMOVE_COLUMN = "AddOrRemoveColumn";

       /// <summary>
       /// Apply Filter on MTT Grid 
       /// </summary>    
       public const string APPLY_FILTER_ON_MTT_GRID = "ApplyFilterOnMTTGrid";

       /// <summary>
       /// Bulk Change Create Stage Order 
       /// </summary>    
       public const string BULK_CHANGE_CREATE_STAGE_ORDER = "BulkChangeCreateStageOrder";

       /// <summary>
       /// Bulk Edit update from mtt step
       /// </summary>    
       public const string BULK_EDIT_UPDATE_FROM_MTT = "BulkEditUpdateFromMTT";

       /// <summary>
       /// Bulk update using mtt step
       /// </summary>    
       public const string BULK_UPDATE_USING_MTT = "BulkUpdateUsingMTT";

       /// <summary>
       /// Refresh Price using mtt step
       /// </summary>    
       public const string REFRESH_PRICE_USING_MTT = "RefreshPriceUsingMTT";

       /// <summary>
       /// Save layout using mtt step
       /// </summary>    
       public const string SAVE_LAYOUT_USING_MTT = "SaveLayoutUsingMTT";

       /// <summary>
       /// Module Navlock
       /// </summary>    
       public const string NAVLOCK = "Navlock";

       /// <summary>
       /// Add Navlock
       /// </summary>    
       public const string ADD_NAVLOCK = "AddNavlock";

       /// <summary>
       /// Delete Existing Navlock step
       /// </summary>    
       public const string DELETE_EXISTING_NAVLOCK = "DeleteExistingNavLock";

       /// <summary>
       /// Verify Navlock popup step
       /// </summary>    
       public const string VERIFY_NAVLOCK_POPUP = "VerifyNavLockPopup";

       /// <summary>
       /// Verify Rows Navlock step
       /// </summary>    
       public const string VERIFY_ROWS_NAVLOCK = "VerifyRowsNavLock";

       /// <summary>
       /// Check PM Live Feed step
       /// </summary>    
       public const string CHECK_PM_LIVE_FEED = "CheckPMLiveFeed";

       /// <summary>
       /// Check Taxlot details step
       /// </summary>    
       public const string CHECK_TAXLOT_DETAILS = "CheckTaxlotDetails";

       /// <summary>
       /// Check Account Position from PM step
       /// </summary>    
       public const string CHECK_ACCOUNT_POSITION_FROM_PM = "CheckAccountPositionFromPM";

       /// <summary>
       /// Increase Position from PM step
       /// </summary>    
       public const string INCREASE_POSITION_FROM_PM = "IncreasePositionFromPM";

       /// <summary>
       /// Open Custom View step
       /// </summary>    
       public const string OPEN_CUSTOM_VIEW = "OpenCustomView";

       /// <summary>
       /// Open PI from PM step
       /// </summary>    
       public const string OPEN_PI_FROM_PM = "OpenPIFromPM";

       /// <summary>
       /// Open Taxlot Details step
       /// </summary>    
       public const string OPEN_TAXLOT_DETAILS = "OpenTaxlotDetails";

       /// <summary>
       /// Save Layout PM step
       /// </summary>    
       public const string SAVE_LAYOUT_PM = "SaveLayoutPM";

       /// <summary>
       /// Sort and verify Pm step
       /// </summary>    
       public const string SORT_AND_VERIFY_PM = "SortAndVerifyPM";

       /// <summary>
       /// View Settings step
       /// </summary>    
       public const string VIEW_SETTINGS = "ViewSettings";

       /// <summary>
       /// Move File step
       /// </summary>    
       public const string MOVE_FILE = "MoveFile";

       /// <summary>
       /// Run Portfolio step
       /// </summary>    
       public const string RUN_PORTFOLIO = "RunPortfolio";

       /// <summary>
       /// Update Client Config step
       /// </summary>    
       public const string UPDATE_CLIENT_CONFIG = "UpdateClientConfig";

       /// <summary>
       /// Auto Stage Import from ptt export step
       /// </summary>    
       public const string AUTO_STAGE_IMPORT_FROM_PTT_EXPORT = "AutoStageImportFromPTTExport";

       /// <summary>
       /// Edit PTT Record and execute order step
       /// </summary>    
       public const string EDIT_PTT_RECORED_AND_EXECUTE_ORDER = "EditPTTRecordAndExecuteOrder";

       /// <summary>
       /// Open Preferences from PTT step
       /// </summary>    
       public const string OPEN_PREFERENCES_FROM_PTT = "OpenPreferencesFromPTT";

       /// <summary>
       /// Set default symbology step
       /// </summary>    
       public const string SET_DEFAULT_SYMBOLOGY = "SetDefaultSymbology";

       /// <summary>
       /// Verify PTT Compliance step
       /// </summary>    
       public const string VERIFY_PTT_COMPLIANCE = "VerifyPTTCompliance";

       /// <summary>
       /// Add custom cash using import step
       /// </summary>    
       public const string ADD_CUSTOM_CASH_USING_IMPORT = "AddCustomCashUsingImport";

       /// <summary>
       /// Add new model portfolio step
       /// </summary>    
       public const string ADD_NEW_MODEL_PORTFOLIO = "AddNewModelPortfolio";

       /// <summary>
       /// Check Basket Complaince step
       /// </summary>    
       public const string CHECK_BASKET_COMPLIANCE = "CheckBasketCompliance";

       /// <summary>
       /// Clear calculation step
       /// </summary>    
       public const string CLEAR_CALCULATION = "ClearCalculation";

       /// <summary>
       /// Close Rebalancer step
       /// </summary>    
       public const string CLOSE_REBALANCER = "CloseRebalancer";

       /// <summary>
       /// Enable complaince tab step
       /// </summary>    
       public const string ENABLE_COMPLIANCE_TAB = "EnableComplianceTab";

       /// <summary>
       /// Import Model portfolio step
       /// </summary>    
       public const string IMPORT_MODEL_PORTFOLIO = "ImportModelPortfolio";

       /// <summary>
       /// Lock and unlock grid data
       /// </summary>    
       public const string LOCK_AND_UNLOCK_GRID_DATA = "LockAndUnlockGridData";

       /// <summary>
       /// Modify Rebalance step
       /// </summary>    
       public const string MODIFY_REBALANCE = "ModifyRebalance";

       /// <summary>
       /// Nav Impacting Componenet step
       /// </summary>    
       public const string NAV_IMPACTING_COMPONENET = "NavImpactingComponenet";

       /// <summary>
       /// Refresh grid data step
       /// </summary>    
       public const string REFRESH_GRID_DATA = "RefreshGridData";

       /// <summary>
       /// Send to staging step
       /// </summary>    
       public const string SEND_TO_STAGING = "SendtoStaging";

       /// <summary>
       /// Send to staging export step
       /// </summary>    
       public const string SEND_TO_STAGING_EXPORT = "SendToStagingExport";

       /// <summary>
       /// Symbology change RB step
       /// </summary>    
       public const string SYMBOLOGY_CHANGE_RB = "SymbologyChangeRB";

       /// <summary>
       /// Rebalance Preference step
       /// </summary>    
       public const string REBALANCE_PREFERENCE = "RebalancePreference";

       /// <summary>
       /// Rebalance Via Bloomberg step
       /// </summary>    
       public const string REBALANCE_VIA_BLOOMBERG = "RebalanceViaBloomberg";

       /// <summary>
       /// ShortLocate Module
       /// </summary>    
       public const string SHORT_LOCATE = "ShortLocate";

       /// <summary>
       /// Apply Filter on shotlocate
       /// </summary>    
       public const string APPLY_FILTER_ON_SHORT_LOCATE = "ApplyFilterOnShortLocate";

       /// <summary>
       /// Download File
       /// </summary>    
       public const string DOWNLOAD_FILE = "DownloadFile";

       /// <summary>
       /// Import on short locate
       /// </summary>    
       public const string IMPORT_ON_SHORT_LOCATE = "ImportOnShortLocate";

       /// <summary>
       /// Open short locate
       /// </summary>    
       public const string OPEN_SHORT_LOCATE = "OpenShortLocate";

       /// <summary>
       /// Open TT From short locate
       /// </summary>    
       public const string OPEN_TT_FROM_SHORT_LOCATE = "OpenTTFromShortLocate";

       /// <summary>
       /// Search short locate UI
       /// </summary>    
       public const string SEARCH_SHORT_LOCATE_UI = "SearchShortLocateUI";

       /// <summary>
       /// Set short locate preference
       /// </summary>    
       public const string SET_SHORT_LOCATE_PREFERENCES = "SetShortLocatePreferences";

       /// <summary>
       /// Upload
       /// </summary>    
       public const string UPLOAD = "Upload";

       /// <summary>
       /// Verify short locate grid
       /// </summary>    
       public const string VERIFY_SHORT_LOCATE_GRID = "VerifyShortLocateGrid";

       /// <summary>
       /// Broker connection through TUI
       /// </summary>    
       public const string BROKER_CONNECTION_THROUGH_TUI = "BrokerConnetionThroughTUI";

       /// <summary>
       /// Close Trade Server
       /// </summary>    
       public const string CLOSE_TRADE_SERVER = "CloseTradeServer";

       /// <summary>
       /// Connect to simulator
       /// </summary>    
       public const string CONNECT_TO_SIMULATOR = "ConnectToSimulator";

       /// <summary>
       /// Start trade server
       /// </summary>    
       public const string START_TRADE_SERVER = "StratTradeServer";

       /// <summary>
       /// Update Server config
       /// </summary>    
       public const string UPDATE_SERVER_CONFIG = "UpdateServerConfig";

       /// <summary>
       /// Add symbol on RA list step
       /// </summary>    
       public const string ADD_SYMBOL_ON_RA_LIST = "AddSymbolOnRAList";

       /// <summary>
       /// Check Pending New order alert step
       /// </summary>    
       public const string CHECK_PENDING_NEW_ORDER_ALERT = "CheckPendingNewOrderAlert";

       /// <summary>
       /// Create Compliance Order
       /// </summary>    
       public const string CREATE_COMPLIANCE_ORDER = "CreateComplianceOrder";

       /// <summary>
       /// Create Replace Live Order
       /// </summary>    
       public const string CREATE_REPLACE_LIVE_ORDER = "CreateReplaceLiveOrder";

       /// <summary>
       /// Custom Order TT
       /// </summary>    
       public const string CUSTOM_ORDER_TT = "CustomOrderTT";

       /// <summary>
       /// Delete Symbol list from RA
       /// </summary>    
       public const string DELETE_SYMBOL_LIST_FROM_RA = "DeleteSymbolListFromRA";

       /// <summary>
       /// Export symbol from RA list
       /// </summary>    
       public const string EXPORT_SYMBOL_FROM_RA_LIST = "ExportSymbolFromRAList";

       /// <summary>
       /// Import Symbol
       /// </summary>    
       public const string IMPORT_SYMBOL = "ImportSymbol";

       /// <summary>
       /// Remove symbol from RA list
       /// </summary>    
       public const string REMOVE_SYMBOL_FROM_RA_LIST = "RemoveSymbolFromRAList";

       /// <summary>
       /// Send live bloomberg
       /// </summary>    
       public const string SEND_LIVE_BLOOMBERG = "SendLiveBloomberg";

       /// <summary>
       /// Set TT Compliance Preference
       /// </summary>    
       public const string SET_TT_COMPLIANCE_PREFERENCE = "SetTTCompliancePreference";

       /// <summary>
       /// Toggle Bloomberg on RA
       /// </summary>    
       public const string TOGGLE_BLOOMBERG_ON_RA = "ToggleBloombergOnRA";

       /// <summary>
       /// Update Master Fund On TT
       /// </summary>    
       public const string UPDATE_MASTER_FUND_ON_TT = "UpdateMasterFundOnTT";

       /// <summary>
       /// Verify Compliance Order
       /// </summary>    
       public const string VERIFY_COMPLIANCE_ORDER = "VerifyComplianceOrder";

       /// <summary>
       /// Verify Cutsom allocation on TT
       /// </summary>    
       public const string VERIFY_CUSTOM_ALLOCATION_ON_TT = "VerifyCutomAllocationonTT";

       /// <summary>
       /// Verify custom allocation second Ui
       /// </summary>    
       public const string VERIFY_CUSTOM_ALLOCATION_SECOND_UI = "VerifyCustomAllocationSecondUI";

       /// <summary>
       /// View Allocation
       /// </summary>    
       public const string VIEW_ALLOCATION = "ViewAllocation";

       /// <summary>
       /// Module WatchList
       /// </summary>    
       public const string WATCH_LIST = "Watchlist";

       /// <summary>
       /// Add symbol to watchlist
       /// </summary>    
       public const string ADD_SYMBOL_TO_WATCHLIST = "AddSymbolToWatchlist";

       /// <summary>
       /// Delete Symbol From Watchlist
       /// </summary>    
       public const string DELETE_SYMBOL_FROM_WATCHLIST = "DeleteSymbolFromWatchlist";

       /// <summary>
       ///Trade from Watchlist
       /// </summary>    
       public const string TRADE_FROM_WATCHLIST = "TradeFromWatclist";

       /// <summary>
       /// Verify symbols on watchlist
       /// </summary>    
       public const string VERIFY_SYMBOLS_ON_WATCHLIST = "VerifySymbolsOnWatchlist";

       /// <summary>
       /// AuditTrail Module
       /// </summary>    
       public const string AUDITTRAIL = "AuditTrail";

       /// <summary>
       /// Get data form audit trail
       /// </summary>    
       public const string GET_DATA_FROM_AUDITTRAIL = "GetDataFromAuditTrail";


       // for RTPNL module
       public const string RTPNL = "RTPNL";

       // for OpenFin module
       public const string OpenFin = "OpenFin";

       // for QTT module
       public const string QTT = "QTT";

       //QTT steps
       public const string OpenQtt = "OpenQtt";
       public const string TradeQTT = "TradeQTT";
       public const string UseOrVerifyHotKeys = "UseOrVerifyHotKeys";


       //OpenFin steps
       public const string CheckOpenFinWindowName = "CheckOpenFinWindowName";
       public const string OpenWindowFromSearch = "OpenWindowFromSearch";
       public const string VerifyColour = "VerifyColour";
       public const string OpenReleaseClient = "OpenReleaseClient";
       public const string ActionOnPageFromSearch = "ActionOnPageFromSearch";
       public const string SavePageView = "SavePageView";
       public const string ChangeThemeOnModule = "ChangeThemeOnModule";
       public const string CloseService = "CloseService";
       public const string LogOutSamsara = "LogOutSamsara";


       //RTPNL steps
       public const string CheckSummaryDashboard = "CheckSummaryDashboard";
       public const string CheckFundLevelAggregation = "CheckFundLevelAggregation";
       public const string CheckAccountLevelAggregation = "CheckAccountLevelAggregation";
       public const string CheckSymbolLevelAggregation = "CheckSymbolLevelAggregation";
       public const string CheckRealTimeSymbolFundMonitor = "CheckRealTimeSymbolFundMonitor";
       public const string CheckRealTimeSymAccountMonitor = "CheckRealTimeSymAccountMonitor";

       /// <summary>
       /// Restart Release and Services
       /// </summary>     
       public const string RESTART_RELEASE_AND_SERVICES = "RestartReleasesAndServices";

    }
}