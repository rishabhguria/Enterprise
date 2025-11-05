using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.TestAutomation.Utilities
{
    public static class ErrorMessages
    {
        /// <summary>
        /// For Gettting Error message print on the test log file
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="stepName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string getErrorMessages(string moduleName, string stepName, string error)
        {
            string message = null;
            try
            {
                switch (moduleName)
                {
                    case AutomationStepsConstants.ALLOCATION:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_CALCULATED_PREFERENCE_TOOLBAR:
                                return "Preference has not been added from toolbar.!\n ( " + error + " )";
                            case AutomationStepsConstants.ADD_CALCULATED_PREFERENCES:
                                return "Preference has not been added from right click.!\n ( " + error + " )";
                            case AutomationStepsConstants.ADD_MF_PREFERENCE_TOOLBAR:
                                return "Master Fund Preference has not been added from toolbar.!\n ( " + error + " )";
                            case AutomationStepsConstants.ADD_MF_PREFERENCES:
                                return " Master Fund Preference has not been added from right click.!\n ( " + error + " )";
                            case AutomationStepsConstants.ALLOCATE:
                                return "Trade has not been allocated.!\n ( " + error + " )";
                            case AutomationStepsConstants.ALLOCATE_WITH_POPUP:
                                return "Trade has not been allocated.!\n ( " + error + " )";
                            case AutomationStepsConstants.APPLY_BULK_UPDATE_ON_GROUPS:
                                return "Bulk update has not been applied on the groups.!\n ( " + error + " )";
                            case AutomationStepsConstants.APPLY_BULK_UPDATE_ON_TAXLOTS:
                                return "Bulk update has not been applied on the taxlots.!\n ( " + error + " )";
                            case AutomationStepsConstants.APPLY_PREFETCH_FILTERS:
                                return "Prefetch filter has not been applied.!\n ( " + error + " )";
                            case AutomationStepsConstants.BOOK_OR_UPDATE_SWAP:
                                return "Swap details have not been updated.!\n ( " + error + " )";
                            case AutomationStepsConstants.BULK_CHANGE_CALCULATED_PREFERENCES:
                                return "Bulk change on calculated preferences has not been applied!.\n( " + error + " )";
                            case AutomationStepsConstants.CHECK_ALLOCATED_GROUPS:
                                return "Groups not found in allocated grid.!\n ( " + error + " )";
                            case AutomationStepsConstants.CHECK_ALLOCATED_TAXLOAT:
                                return "Groups not found in allocated taxlot grid.!\n ( " + error + " )";
                            case AutomationStepsConstants.CHECK_FIXED_PREFERENCE:
                                return "Fixed preference details have not been matched!." + error + ")";
                            case AutomationStepsConstants.CHECK_UNALLOCATED_GROUPS:
                                return "Groups not found in unallocated grid!.(" + error + " )";
                            case AutomationStepsConstants.CLEAN_UP:
                                return "Allocation clean up has not been performed!.(" + error + " )";
                            case AutomationStepsConstants.CLOSE_ALLOCATION:
                                return "Error occured while closing allocation UI.!.(" + error + " )";
                            case AutomationStepsConstants.IS_Allocation_Open:
                                return "Not able to open Allocation from blotter!.(" + error + " )";
                            case AutomationStepsConstants.CLOSE_CALCULATED_PREFERENCES:
                                return "Error while closing calculated preference UI!.(" + error + " )";
                            case AutomationStepsConstants.CLOSE_ORDER:
                                return "Order has not been closed!.(" + error + " )";
                            case AutomationStepsConstants.COMMISSION_BULK_CHANGE_ON_GROUPS:
                                return "Commission bulk change on groups has not been applied!.(" + error + " )";
                            case AutomationStepsConstants.COMMISSION_BULK_CHANGE_ON_TAXLOTS:
                                return "Commission bulk change on taxlots has not been applied!.(" + error + " )";
                            case AutomationStepsConstants.COPY_CALCULATED_PERFERENCES:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for copy!.(" + error + " )";
                            case AutomationStepsConstants.COPY_CALCULATED_PREFERENCE_TOOL:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for copy from Toolbar !.(" + error + " )";
                            case AutomationStepsConstants.DELETE_CALCULATED_PREFERENCES:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for deletion!.(" + error + " )";
                            case AutomationStepsConstants.DELETE_CALCULATED_PREFERENCES_TOOL:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for deletion from Toolbar!.(" + error + " )";
                            case AutomationStepsConstants.DELETE_FIXED_PREFERENCE:
                                return "Fixed Preference has not been deleted!.(" + error + ")";
                            case AutomationStepsConstants.DELETE_GROUP:
                                return "Group has not been deleted!.(" + error + ")";
                            case AutomationStepsConstants.DELETE_GROUP_WITHOUT_SAVE:
                                return "Group has not been deleted!.(" + error + ")";
                            case AutomationStepsConstants.DELETE_MF_PREFERENCES:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for deletion!.(" + error + ")";
                            case AutomationStepsConstants.DELETE_MF_PREFERENCES_TOOLBAR:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for deletion!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_ALLOCATED_GROUP_SIDE_PANEL:
                                return "Allocated group has not been edited from side panel!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_TRADES_ALLOCATED:
                                return "Allocated trade has not been edited.!\n ( " + error + " )";
                            case AutomationStepsConstants.EDIT_TRADES_UNALLOCATED:
                                return "Unallocated trade has not been edited.!\n ( " + error + " )";
                            case AutomationStepsConstants.EDIT_UNALLOCATED_GROUP_SIDE_PANEL:
                                return "Unallocated group has not been edited from side panel.!\n ( " + error + " )";
                            case AutomationStepsConstants.ENTER_ACCOUNT_VALUES_TO_ALLOCATE:
                                return "Trade has not been allocated by account values.!\n ( " + error + " )";
                            case AutomationStepsConstants.ENTER_ALLOCATE_PREFERENCE:
                                return "Trade has not been custom allocated by preference.!\n ( " + error + " )";
                            case AutomationStepsConstants.ENTER_CUSTOM_ALLOCATE_PREFERENCE:
                                return "Trade has not been allocated by preference.!\n ( " + error + " )";
                            case AutomationStepsConstants.ENTER_STRATEGY_VALUES_TO_ALLOCATE:
                                return "Trade has not been allocated by strategy values.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXPORT_ALL_CALCULATED_PREFERENCES:
                                return "All Calculated preference has not been exported.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXPORT_ALL_CALCULATED_PREFER_TOOL:
                                return "All calculated preference has not been exported from toolbar.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXPORT_CALCULATED_PREFERENCE:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for export.!\n ( " + error + " )";
                            case AutomationStepsConstants.GENERAL_PREF_COMPANY_USERWISE:
                                return "Company userwise general preferences have not been saved.!\n ( " + error + " )";
                            case AutomationStepsConstants.GENERAL_PREFERENCES_DEFAULT_RULE:
                                return "Company userwise default rule preferences have not been saved.!\n ( " + error + " )";
                            case AutomationStepsConstants.GET_DATA_ALLOCATION:
                                return "Data not fetched properly on allocation UI!.(" + error + ")";
                            case AutomationStepsConstants.GET_FIXED_PREFERENCE:
                                return "Fixed Preference has not been fetched!.(" + error + ")";
                            case AutomationStepsConstants.GROUP_TRADES:
                                return "Trades have not been grouped!.(" + error + ")";
                            case AutomationStepsConstants.IMPORT_CALCULATED_PREFERENCES:
                                return "Calculated preference has not been imported!.(" + error + ")";
                            case AutomationStepsConstants.IMPORT_CALCULATED_PREFERENCES_TOOL:
                                return "Calculated preference has not been imported from Toolbar!.(" + error + ")";
                            case AutomationStepsConstants.MODIFY_GR_PREFERENCE_VALUES:
                                return "Values of general rule Preferences not updated.!\n ( " + error + " )";
                            case AutomationStepsConstants.MODIFY_MF_PREF_GENERAL_RULE_VALUES:
                                return "Values of general rule MF Preferences not updated.!\n ( " + error + " )";
                            case AutomationStepsConstants.OPEN_AUDIT_TRAIL_ALLOCATED_GROUP:
                                return "Audit trail for allocated group has not been opened!.(" + error + ")";
                            case AutomationStepsConstants.OPEN_AUDIT_TRAIL_UNALLOCATED_GROUP:
                                return "Audit Trail has not been opened for unallocated group!.(" + error + ")";
                            case AutomationStepsConstants.OPEN_SYMBOL_LOOKUP_ALLOCATED_GROUP:
                                return "Symbol Lookup for allocated group has not been opened !.(" + error + ")";
                            case AutomationStepsConstants.OPEN_SYMBOL_LOOKUP_UNALLOCATED_GROUP:
                                return "Symbol Lookup has not been opened for unallocated group!.(" + error + ")";
                            case AutomationStepsConstants.REALLOCATE:
                                return "Trade has not been reallocated!.(" + error + ")";
                            case AutomationStepsConstants.RENAME_CALCULATED_PREFERENCES:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for rename.";
                            case AutomationStepsConstants.RENAME_MF_PREFERENCES:
                                return MessageConstants.MSG_PREF_NOT_FOUND + " for rename in MF Pref.";
                            case AutomationStepsConstants.RENAME_TRADE_ATTRIBUTES:
                                return "Trade attribute has not been renamed!.(" + error + ")";
                            case AutomationStepsConstants.RUN_AUTO_GROUP:
                                return "Auto group has not been performed!.(" + error + ")";
                            case AutomationStepsConstants.RUN_PRORATA:
                                return "Prorata calculation failed!(" + error + ")";
                            case AutomationStepsConstants.SAVE_CALCULATED_PREFERENCES:
                                return "Calculated preferences have not been saved!(" + error + ")";
                            case AutomationStepsConstants.SAVE_CLOSE_MF_PREFERENCES:
                                return "Master fund preferences have not been saved and close!(" + error + ")";
                            case AutomationStepsConstants.SELECT_ALLOCATED_GROUP:
                                return "Allocated groups have not been selected!(" + error + ")";
                            case AutomationStepsConstants.SELECT_UNALLOCATED_GROUP:
                                return "Unallocated groups have not been selected!.(" + error + ")";
                            case AutomationStepsConstants.SET_ACCOUNT_VALUE_CALCULATED_PREF:
                                return "Account values of preference has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_AUTO_GROUPING_ACCOUNT:
                                return "Auto grouping for account has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_AUTO_GROUPING_PREFERENCES:
                                return "Auto grouping for preferences have not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_AVG_PRICE_ROUNDING:
                                return "Average price rounding has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_BROKER_ACCOUNT_MAPPING:
                                return "Broker Account Mapping has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_DEFAULT_RULE_CALCULATED_PERF:
                                return "Default rule for preferences has not been set!.(" + error + ")";
                            case AutomationStepsConstants.SET_GENERAL_PREFERENCE:
                                return "General Preference has not been set!(" + error + ")";
                            case AutomationStepsConstants.SET_GENERALRULE_CALCULATED_PREF:
                                return "General Rule has not set on Calculated Preference.!\n ( " + error + " )";
                            case AutomationStepsConstants.SET_GROUPING_RULES:
                                return "Grouping rules have not been set!(" + error + ")";
                            case AutomationStepsConstants.SET_MASTER_FUND_RATIO:
                                return "Master fund ratio has not set!(" + error + ")";
                            case AutomationStepsConstants.SET_MF_PREF_ACCOUNT_VALUES:
                                return "Account values of master fund preference has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_MF_PREF_FUND_DEFAULT_RULE:
                                return "Default rule for master fund preferences has not been set!.(" + error + ")";
                            case AutomationStepsConstants.SET__MF_PREFERENCE_DEFAULT_RULE:
                                return "Default rule for master fund preferences has not been set!.(" + error + ")";
                            case AutomationStepsConstants.SET_MF_PREF_FUND_DISTRIBUTION:
                                return "Master fund distribution of master fund preference has not been set! (" + error + ")";
                            case AutomationStepsConstants.SET_MF_PREF_GENERAL_RULE:
                                return "General Rule has not set on Master Fund Preference.!\n ( " + error + " )";
                            case AutomationStepsConstants.SET_MF_PREF_STRATEGY_VALUES:
                                return "Strategy value for master fund preferences have not been set!(" + error + ")";
                            case AutomationStepsConstants.SET_STRATEGY_VALUE_CALCULATED_PREF:
                                return "Strategy value for preferences have not been set!(" + error + ")";
                            case AutomationStepsConstants.SET_TRADE_ATTRIBUTE_PREFERENCES:
                                return "Trade Attribute preferences have not been set!(" + error + ")";
                            case AutomationStepsConstants.SHOW_MASTER_FUND_AS_CLIENT:
                                return "Show master fund as client has not been set!(" + error + ")";
                            case AutomationStepsConstants.SHOW_MASTER_FUND_ON_TT:
                                return "Show Master Fund On TT has not been set!(" + error + ")";
                            case AutomationStepsConstants.TRADE_ATTRIBUTE_BULK_CHANGE_GROUPS:
                                return "Trade attribute bulk change on groups have not been applied!.(" + error + ")";
                            case AutomationStepsConstants.TRADE_ATTRIBUTE_BULK_CHANGE_TAXLOTS:
                                return "Trade Attribute bulk change on taxlots have not been applied!.(" + error + ")";
                            case AutomationStepsConstants.UNALLOCATE:
                                return "Trade has not been unallocated!.(" + error + ")";
                            case AutomationStepsConstants.UNGROUP_TRADES:
                                return "Trade has not been ungrouped!.(" + error + ")";
                            case AutomationStepsConstants.CLEAR_PREFETCH_FILTERS:
                                return "Clear pre fetch filters not performed!.(" + error + ")";
                            case AutomationStepsConstants.SAVE_ALLOCATION:
                                return "Allocation status has not saved!.(" + error + ")";
                            case AutomationStepsConstants.UNALLOCATE_WITHOUT_SAVE:
                                return "Trades have not been unallocated without save status.!(" + error + ")";
                            case AutomationStepsConstants.SAVE_LAYOUT_ALLOCATEDGRID:
                                return "Save Layout can not be performed on allocated grid!.(" + error + ")";
                            case AutomationStepsConstants.SAVE_LAYOUT_UNALLOCATEDGRID:
                                return "Save Layout can not be performed on unallocated grid!.(" + error + ")";
                            case AutomationStepsConstants.CLEAR_FILTERS_ALLOCATEDGRID:
                                return "Clear Filters can not be performed on allocated grid!.(" + error + ")";
                            case AutomationStepsConstants.CLEAR_FILTERS_UNALLOCATEDGRID:
                                return "Clear Filters can not be performed on unallocated grid!.(" + error + ")";
                            case AutomationStepsConstants.EXPAND_COLLAPSE_ALLOCATEDGRID:
                                return "Expand/Collapse All can not be performed on allocated grid!.(" + error + ")";
                            case AutomationStepsConstants.EXPAND_COLLAPSE_UNALLOCATEDGRID:
                                return "Expand/Collapse All can not be performed on unallocated grid!.(" + error + ")";
                            case AutomationStepsConstants.SET_GENERAL_PREFERENCES:
                                return "Error occured in SetGeneralPreferences module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.VERIFY_LAYOUT_ALLOCATEDGRID:
                                return "Allocation grid layout has failed verification.!(" + error + ")";
                            case AutomationStepsConstants.VERIFY_LAYOUT_UNALLOCATEDGRID:
                                return "Unallocation grid layout has failed verification.!(" + error + ")";
                            case AutomationStepsConstants.SAVE_LAYOUT_ALLOCATION_UI:
                                return "Save Layout can not be performed on allocation UI!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_ALLOCATED_GRID:
                                return "Allocated Grid Could not be edited!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_UNALLOCATED_GRID:
                                return "Unallocated Grid Could not be edited!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_ALLOCATED_GRID_WPF:
                                return "Allocated Grid Could not be edited at Group level!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_UNALLOCATED_GRID_WPF:
                                return "Unallocated Grid Could not be edited at Group level!.(" + error + ")";
                            case AutomationStepsConstants.EDIT_ALLOCATED_GRID_TAXLOT:
                                return "Allocated Grid Could not be edited at Taxlot level!.(" + error + ")";
                            case AutomationStepsConstants.DISABLE_CHECKSIDE_PREFERENCES:
                                return "Error occured in disabiling allocation checkside preferences module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.DISABLE_CHECKSIDE:
                                return "Error occured in disabiling allocation checkside module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.DISABLE_CHECKSIDE_ACCOUNT:
                                return "Error occured in disabiling allocation checkside for account module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.DISABLE_CHECKSIDE_ASSET:
                                return "Error occured in disabiling allocation checkside for asset module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.DISABLE_CHECKSIDE_COUNTERPARTY:
                                return "Error occured in disabiling allocation checkside for counterparty module in Allocation!.(" + error + ")";
                            case AutomationStepsConstants.CHECK_PREFERENCES:
                                return "Preference is not as per requirement in drop down.!\n ( " + error + " )";
                            case AutomationStepsConstants.UPDATE_GENERAL_RULE_PREF:
                                return "General Rule Preference have not been updated.! ( " + error + " )";
                            case AutomationStepsConstants.UPDATE_GRACCOUNT_VALUE:
                                return "General Rule Account Values have not been updated.! ( " + error + " )";
                            case AutomationStepsConstants.UPDATE_GRDEFAULT_RULE:
                                return "General Rule  Default Rule has not been updated.! ( " + error + " )";
                            case AutomationStepsConstants.UPDATE_GRSTRATEGY_VALUE:
                                return "General Rule Startegy Values have not been updated.! ( " + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Allocation).!(" + error + ")";

                        }
                    case AutomationStepsConstants.BLOTTER:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_FILLS_SUB_ORDERS:
                                return "Fills have not been added for suborder.!\n (" + error + " )";
                            case AutomationStepsConstants.ADD_FILL_WORKINGS_SUBS:
                                return "Fills have not been added for order on working subs tab.!\n (" + error + " )";
                            case AutomationStepsConstants.AUDIT_TRAIL_ORDER_BLOTTER:
                                return "Error occured while opening audit trail from blotter!\n (" + error + " )";
                            case AutomationStepsConstants.BLOTTER_FUNCTIONALITIES:
                                return "Failed to give blotter functionalities. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.BLOTTER_HELPER:
                                return "Blotter helper failed. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CANCLE_ALL_SELECTED_SUBS:
                                return "All selected subs have not been cancelled!\n (" + error + " )";
                            case AutomationStepsConstants.CANCLE_ALL_SUBS:
                                return "All subs have not been cancelled on OrderGrid.!\n (" + error + " )";
                            case AutomationStepsConstants.CANCEL_SUB_ORDER:
                                return "Error occured while cancelling sub order.!\n (" + error + " )";
                            case AutomationStepsConstants.CANCEL_WORKING_SUBS:
                                return "Cancel not clicked on WorkingSubs Grid.!\n (" + error + " )";
                            case AutomationStepsConstants.CHECK_BLOTTER:
                                return "Failed to check blotter. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.DETAILS_FOR_ALGO_TRADE_SUB_ORDER:
                                return "Details for Algo(Show Details) on SubOrder Grid not Clicked.!\n (" + error + " )";
                            case AutomationStepsConstants.DETAILS_FOR_ALGO_TRADE_ORDER:
                                return "Details for Algo(Show Details) on Order Grid not Clicked.!\n (" + error + " )";
                            case AutomationStepsConstants.DETAILS_FOR_ALGO_TRADE_WS:
                                return "Details for Algo(Show Details) on WorkingSubs Grid not Clicked.!\n (" + error + " )";
                            case AutomationStepsConstants.EDIT_ADD_FILLS_DETAILS:
                                return "Edit Add Fills Details not done.!\n (" + error + " )";
                            case AutomationStepsConstants.EDIT_ORDERS:
                                return "Not Able to Edit this Trade in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.EDIT_STAGE_ORDERS:
                                return "Not Able to Edit this Trade in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_EXECUTION_REPORT_ORDERS:
                                return "Not able to fetch execution report orders.!\n (" + error + " )";
                            case AutomationStepsConstants.MULTI_TREADING_TICKET_FROM_BLOTTER:
                                return "Not Able to Open MultiTradingTicket.!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_SELECTED_ORDERS:
                                return "Not Able to Remove Selected Orders.!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_STAGE_ORDER:
                                return "Not Able to Remove Stage Order.!\n (" + error + " )";
                            case AutomationStepsConstants.REFRESH_BLOTTER:
                                return "Not able to refresh blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.REPLACE_FILLS_ON_BLOTTER:
                                return "Fills are not replaced on Boltter.!\n (" + error + " )";
                            case AutomationStepsConstants.REPLACE_STAGE_ORDER:
                                return "Not Able to Remove Stage Order.!\n (" + error + " )";
                            case AutomationStepsConstants.REPLACE_SUB_ORDER:
                                return "Not Able to Replace Trade in SubOrders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.REPLACE_WORKING_SUBS:
                                return "Not Able to replace tarde in Working Subs.!\n (" + error + " )";
                            case AutomationStepsConstants.SAVE_LAYOUT_ORDER_GRID:
                                return "Failed to save layout order grid. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SELECT_TRADE_ON_ORDER_GRID:
                                return "Not Able to select Trade in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_TRADE_SUB_ORDER_BLOTTER:
                                return "Not Able to select Trade in SubOrder Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_TRADE_WORKING_SUBS_BLOTTER:
                                return "Not Able to select Trade in Working Subs Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.TRADE_NEW_SUB_ORDER:
                                return "Not Able to click on Trade(New Sub) in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.TRANSFER_TO_USER_ORDER:
                                return "Not Able to Transfer Trade to User in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.TRANSFER_TO_USER_SUB_ORDER:
                                return "Not Able to Transfer to User Trade in SubOrder Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.TRANSFER_TO_USER_WORKING_SUB:
                                return "Not Able to Transfer to User Trade in WorkingSubs Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ALGO_DETAILS:
                                return "Not Able to Verify Algo Details.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ORDER:
                                return "Not Able to Verify Order in Orders Tab.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_SUB_ORDER:
                                return "Not Able to Verify SubOrder Grid.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_WORKING_SUBS:
                                return "Not Able to Verify trade in WorkingSubs.!\n (" + error + " )";
                            case AutomationStepsConstants.VIEW_ALLOCATIONS_DETAILS_WORKING_SUB:
                                return "Allocation details wroking sub not viewed.!\n (" + error + " )";
                            case AutomationStepsConstants.VIEW_ALLOCATION_DETAILS_SUB_ORDER:
                                return "Allocation details sub order not viewed.!\n (" + error + " )";
                            case AutomationStepsConstants.ADD_TAB:
                                return "The new Tab could not be created.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_TAB:
                                return "The Tab does not exist.!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_TAB:
                                return "The Tab does not exist which you want to remove.!\n (" + error + " )";
                            case AutomationStepsConstants.RENAME_TAB:
                                return "The Tab does not exist which you want to rename.!\n (" + error + " )";
                            case AutomationStepsConstants.SAVE_LAYOUT_BLOTTER:
                                return "Not Able to Save the Layout of blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.EXPORT_TO_EXCEL:
                                return "Not able to Export Data From Blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.BLOTTER_EXECUTION_REPORT_SNAPSHOT:
                                return "Not able to take the Screenshot of Blotter Execution Report.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_EXECUTION_REPORT_DETAILS:
                                return "Not able to get the execution report details.!\n (" + error + " )";
                            case AutomationStepsConstants.EXPORT_EXECUTION_REPORT_DATA:
                                return "Not able to export the execution report data.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_EXECUTION_REPORT:
                                return "Not able to verify the execution report data.!\n (" + error + " )";
                            case AutomationStepsConstants.ADD_TAB_BLOTTER:
                                return "The new Tab could not be created.!\n (" + error + " )";
                            case AutomationStepsConstants.ALLOCATE_FROM_BLOTTER:
                                return "Not able to allocate orders from blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.ALLOCATE_FROM_WORKING_SUBS_BLOTTER:
                                return "Not able to allocate orders from working subs blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.CANCEL_FROM_SUMMARY:
                                return "Error occured while cancelling summary orders.!\n (" + error + " )";
                            case AutomationStepsConstants.GO_TO_ALLOCATION:
                                return "Error occured while trying to open allocation from blotter.!\n (" + error + " )";
                            case AutomationStepsConstants.MERGE_ORDER:
                                return "Not able to Merge Orders.!\n (" + error + " )";
                            case AutomationStepsConstants.MULTIPLE_TRADE_TRANSFER_TO_USER:
                                return "Not able to transfer multiple trades to the user .!\n (" + error + " )";
                            case AutomationStepsConstants.OPEN_BLOTTER:
                                return "Not able to open blotter .!\n (" + error + " )";
                            case AutomationStepsConstants.REALLOCATE_FROM_BLOTTER_BY_ALLOCATE:
                                return "Error Occured while reallocate from blotter by allocate.!\n (" + error + " )";
                            case AutomationStepsConstants.RELOAD_FROM_SUMMARY:
                                return "Not able to Reload order from summary .!\n (" + error + " )";
                            case AutomationStepsConstants.RELOAD_ORDER_FROM_BLOTTER:
                                return "Not able to reload from blotter .!\n (" + error + " )";
                            case AutomationStepsConstants.RELOAD_ORDER_FROM_SUBORDER:
                                return "Not able to reload order from suborder .!\n (" + error + " )";
                            case AutomationStepsConstants.RELOAD_ORDER_FROM_WORKING_SUBS:
                                return "Not able to reload order from workingsubs .!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_EXECUTION_WORKINGSUBS:
                                return "Not able to remove execution from workingsubs .!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_MANUAL_EXECUTION:
                                return "Not able to manual execution.!\n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_ORDER_FROM_CUSTOM_TAB:
                                return "Not able to remove orders from custom tab!\n (" + error + " )";
                            case AutomationStepsConstants.SAVE_OR_REMOVE_SUMMARY_TAB:
                                return "Error occured while trying to save or removein summary tab!\n (" + error + " )";
                            case AutomationStepsConstants.SAVE_OR_REMOVE_WORKINGSUBS_TAB:
                                return "Error occured while trying to save or removein workingsubs tab!\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_CHECK_BOX_ON_CUSTOM_TAB:
                                return "Not able to select check box on cutom tab !\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_CHECK_BOX_ON_ORDER_GRID:
                                return "Not able to select check box on order grid !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_AUDIT_TRAIL:
                                return "Not able to verify audit trail !\n (" + error + " )";
                            case AutomationStepsConstants.UPLOAD_STAGE_ORDER:
                                return "Not able to upload stage order !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_CUSTOM_TABS:
                                return "Not able to verify custom tabs !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_LOWER_STRIP_BLOTTERUI:
                                return "Not able to verify lower strip of blotter ui !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_MERGE_ORDER:
                                return "Not able to verify merge orders !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ROLLOVER:
                                return "Not able to verify rollover orders !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_SUBORDER_AFTER_ROLLOVER:
                                return "Not able to verify suborders after rollover !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_VIEW_ALLOCATION_DETAILS:
                                return "Not able to verify view allocation details !\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_VIEW_ALLOCATION_ORDER:
                                return "Not able to verify view allocation order !\n (" + error + " )";
                            case AutomationStepsConstants.VIEW_PTT_ALLOCATION_DETAILS:
                                return "Not able to view PTT allocation details !\n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Blotter).!(" + error + ")";

                        }
                    case AutomationStepsConstants.CLEAN_UP:
                        return "Failed to delte data using script. Reason : \n(" + error + ")";
                    case AutomationStepsConstants.CLOSING:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CLOSING_PREFERENCES:
                                return "Not Able to Set Closing Preferences.!\n (" + error + " )";
                            case AutomationStepsConstants.AUTOMATIC_CLOSING:
                                return "Trade has not been closed automatically.!\n ( " + error + " )";
                            case AutomationStepsConstants.CASH_SETTLEMENT_AT_CLOSING_DT_SPOT_PX:
                                return "Future trade has not been settled at closing date spot price.!\n ( " + error + " )";
                            case AutomationStepsConstants.CASH_SETTLEMENT_AT_COST:
                                return "Future trade has not been settled at cost.!\n ( " + error + " )";
                            case AutomationStepsConstants.CASH_SETTLEMENT_AT_ZERO_PRICE:
                                return "Future trade has not been settled at zero price.!\n ( " + error + " )";
                            case AutomationStepsConstants.CLOSE_CLOSING_UI:
                                return "Closing UI has not been closed.!\n ( " + error + " )";
                            case AutomationStepsConstants.CLOSE_ORDER:
                                return "Close Order from Closing not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.CLOSING_CLEAN_UP:
                                return "Closing of trades has not been cleaned.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXERCISE_ASSIGNMENT:
                                return "Trade has not been exercise assigned.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXERCISE_ASSIGNMENT_AT_ZERO:
                                return "Trade has not been exercise assigned at zero.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXPIRE:
                                return "Trade has not been expired.!\n ( " + error + " )";
                            case AutomationStepsConstants.GET_DATA_CLOSING:
                                return "Data has not been loaded.!\n ( " + error + " )";
                            case AutomationStepsConstants.MANUAL_CLOSING:
                                return "Trade has not been closed manually.!\n ( " + error + " )";
                            case AutomationStepsConstants.PHYSICAL_SETTLEMENT:
                                return "Trade has not been settled physically.!\n ( " + error + " )";
                            case AutomationStepsConstants.SELECT_TRADE_UNEXPIRED_UNSETTLED:
                                return "Unexpired or unsettled trades have not been selected.!\n ( " + error + " )";
                            case AutomationStepsConstants.SWAP_EXPIRE:
                                return "Swap trade has not been expired!\n ( " + error + " )";
                            case AutomationStepsConstants.SWAP_EXPIRE_AND_ROLLOVER:
                                return "Swap trade has not been expired and rolled over.!\n ( " + error + " )";
                            case AutomationStepsConstants.UNWIND_CLOSE_ORDER:
                                return "Close order trades have not been unwinded.!\n ( " + error + " )";
                            case AutomationStepsConstants.UNWIND_CLOSING:
                                return "Close trades have not been unwinded.!\n ( " + error + " )";
                            case AutomationStepsConstants.UNWIND_TRADE_SETTLEMENT:
                                return "Settled trades have not been unwinded.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_CLOSED_TRADES:
                                return "Close trades have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_CLOSE_ORDER:
                                return "Close order has not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_EXPIRED_SETTLED_GRID:
                                return "Expired settled trades have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_LONG_POSITIONS_GRID:
                                return "Long positions have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_SHORT_POSITIONS_GRID:
                                return "Short positions have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_UNEXPIRED_UNSETTLED_GRID:
                                return "Unexpired/Unsettled trade has not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.ADD_DUPLICATE_ROW:
                                return "not able to add duplicate rows.!\n ( " + error + " )";
                            case AutomationStepsConstants.AUTO_EXERCISE:
                                return "Not able to auto exercise trades.!\n ( " + error + " )";
                            case AutomationStepsConstants.EDIT_CLOSING_TRANSACTION:
                                return "Not able to edit closing transactions.!\n ( " + error + " )";
                            case AutomationStepsConstants.EDIT_TRADE_UNEXPIRED_UNSETTLED:
                                return "Not able to edit trade unexpired unsettled.!\n ( " + error + " )";
                            case AutomationStepsConstants.EXERCISE_ASSIGNMENT_WITH_OUT_SAVE:
                                return "Trades has not been exercised assigned with out save.!\n ( " + error + " )";
                            case AutomationStepsConstants.SAVE_CT_DATA:
                                return "create transaction data has not been save.!\n ( " + error + " )";
                            case AutomationStepsConstants.SELECT_ACCOUNT_WISE_ASSET:
                                return "Not able to select account wise asset.!\n ( " + error + " )";
                            case AutomationStepsConstants.SELECT_CREATE_TRANSACTION:
                                return "Not able to select create transaction.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_CT_DATA:
                                return "Create Transaction data has not been verified.!\n ( " + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Closing).!(" + error + ")";

                        }
                    case AutomationStepsConstants.Corporate_Action:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CLEAN_UP:
                                return "CleanUp of Corporate Action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.Corporate_Action:
                                return "Corporate Action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.CHECK_CORPORATE_ACTION:
                                return "Corporate Action positions have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.CHECK_CORPORATE_ACTION_DETAILS:
                                return "Corporate Action details have not been verified.!\n ( " + error + " )";
                            case AutomationStepsConstants.OPEN_SPIN_OFF:
                                return "Open Spin off action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.SPIN_OFF:
                                return "Spin off action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.SPLIT:
                                return "Split action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_AND_APPLY_CORP_ACTION:
                                return "Verfy and apply corporate action not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_AND_SAVE_CORP_ACTION:
                                return "Verfy and save corporate action not performed.!\n ( " + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Corporate Action).!(" + error + ")";

                        }
                    case AutomationStepsConstants.COMPLIANCE:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CHECK_COMPLIANCE:
                                return "Compliance is not running properly.!\n ( " + error + " )";
                            case AutomationStepsConstants.RESTART_ESPER:
                                return "Esper couldn't be restarted.!\n ( " + error + " )";
                            case AutomationStepsConstants.VERIFY_ACCOUNT_DIVISOR_WINDOW:
                                return "Failed to Verify Account Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_ACCOUNT_NAV_PREFERENCE_WINDOW:
                                return "Failed to Verify Account Nav Preference Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_ACCOUNT_SYMBOL_DIVISOR_WINDOW:
                                return "Failed to Verify Account Symbol Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_ACCOUNT_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Account Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_ACCRUAL_FOR_ACCOUNT_WINDOW:
                                return "Failed to Verify Accrual For Account Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_ACCOUNT_UNDERLYING_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Account Underlying Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_ACCOUNT_WITH_NAV:
                                return "Failed to Verify Aggregation Account With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_ASSET_WITH_NAV:
                                return "Failed to Verify Aggregation Asset With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_GLOBAL_WITH_NAV:
                                return "Failed to Verify Aggregation Global With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_MASTER_FUND_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Master Fund Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_MASTER_FUND_UNDERLYING_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Master Fund Underlying Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_MASTER_FUND_WITH_NAV:
                                return "Failed to Verify Aggregation Master Fund With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_SECTOR_WITH_NAV:
                                return "Failed to Verify Aggregation Sector With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_SUB_SECTOR_WITH_NAV:
                                return "Failed to Verify Aggregation Sub Sector With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AGGREGATION_UNDERLYING_SYMBOL_WITH_NAV:
                                return "Failed to Verify Aggregation Underlying Symbol With Nav. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AUEC_WINDOW:
                                return "Failed to Verify Auec Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_BETA_WINDOW:
                                return "Failed to Verify Beta Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_CUSTOM_SYMBOL_DATA_WINDOW:
                                return "Failed to Verify Custom Symbol Data Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_DAY_END_CASH_ACCOUNT_WINDOW:
                                return "Failed to Verify Day End Cash Account Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_DB_NAV_WINDOW:
                                return "Failed to Verify Db Nav Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_GLOBAL_DIVISOR_WINDOW:
                                return "Failed to Verify Global Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_MASTER_FUND_DIVISOR_WINDOW:
                                return "Failed to Verify Master Fund Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_MASTER_FUND_SYMBOL_DIVISOR_WINDOW:
                                return "Failed to Verify Master Fund Symbol Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_PM_CALCULATION_PREFERENCE_WINDOW:
                                return "Failed to Verify Pm Calculation Preference Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_ROW_CALCULATION_BASE_WINDOW:
                                return "Failed to Verify Row Calculation Base Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SECURITY_WINDOW:
                                return "Failed to Verify Security Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SYMBOL_DATA_WINDOW:
                                return "Failed to Verify Symbol Data Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SYMBOL_DIVISOR_WINDOW:
                                return "Failed to Verify Symbol Divisor Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_TAXLOT_WINDOW:
                                return "Failed to Verify Taxlot Window. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.APPROVE_OR_REJECT_PENDING_APPROVAL:
                                return "Not able to approve or reject trades. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EDIT_INTERCEPTOR_FILE:
                                return "Not able to edit interceptor file. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REPLACE_INTERCEPTOR_FILE:
                                return "Not able to replace interceptor file. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.RESTART_RELEASES_AND_SERVICES:
                                return "Not able to restart release and services. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Create Transaction).!(" + error + ")";
                        }

                    case AutomationStepsConstants.CREATE_TRANSACTION:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_NEW_TRANSACTION:
                                return "Add new Transaction not performed.!\n ( " + error + " )";
                            case AutomationStepsConstants.CREATE_TRANSACTION:
                                return "Create transaction not perofrmed.!\n ( " + error + " )";
                            case AutomationStepsConstants.SAVE_TRANSACTION:
                                return "Save transaction not perofrmed.!\n ( " + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Create Transaction).!(" + error + ")";

                        }
                    case AutomationStepsConstants.DAILY_VALUATION:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.GET_DAILY_BETA:
                                return "Failed to get the data for daily beta. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_CASH:
                                return "Failed to get the data for daily cash. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_DELTA:
                                return "Failed to get the data for daily delta. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_DIVIDEND_YIELD:
                                return "Failed to get the data for daily dividend yield. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_OUTSTANDINGS:
                                return "Failed to get the data for daily outstandings. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_TRADING_VOLUME:
                                return "Failed to get the data for daily trading volume. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_DAILY_VOLATILITY:
                                return "Failed to get the data for daily volatility. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_FOREX_CONVERSION:
                                return "Failed to get the data for forex conversion. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_FORWARD_POINTS:
                                return "Failed to get the data for forward points. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_MARK_PRICE_DATA:
                                return "Failed to get the data for mark price. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_NAV:
                                return "Failed to get NAV. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REMOVE_DAILY_CASH:
                                return "Failed to remove the data for daily cash. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REMOVE_DAILY_CREDIT_LIMIT:
                                return "Failed to remove the data of daily credit limit. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REMOVE_NAV:
                                return "Failed to remove NAV. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_BETA:
                                return "Failed to update beta. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DAILY_CASH:
                                return "Daily Cash has not updated Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DAILY_CREDIT_LIMIT:
                                return "Failed to update daily credit limit. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DAILY_DELTA:
                                return "Failed to update daily delta. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DAILY_OUTSTANDINGS:
                                return "Failed to update daily outstandings. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DAILY_VOLATILITY:
                                return "Failed to update daily volatility. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_DIVIDEND_YIELD:
                                return "Failed to update dividend yield. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_FOREX_CONVERSTION:
                                return "Failed to update forex conversion. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_FOREX_CONVERSION_MONTHLY:
                                return "Failed to update forex conversion monthly. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_FORWARD_POINTS:
                                return "Failed to update forward points. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_MARK_PRICE:
                                return "Failed to update mark price. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_NAV:
                                return "Failed to update NAV. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_TRADING_VOLUME:
                                return "Failed to update trading volume. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_DAILY_CASH:
                                return "Failed to verify daily cash. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_MARKPRICE_MONTHLY:
                                return "Failed to update mark price monthly : \n(" + error + ")";
                            case AutomationStepsConstants.ENABLE_BLANK_MARK_PRICES:
                                return "Failed to Enable Blank Mark Prices : \n(" + error + ")";
                            case AutomationStepsConstants.FILTER_BLANK_MP:
                                return "Failed to Filter Blank MP : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_SYMBOL_BLANK_MP:
                                return "Failed to Update Symbol Blank mp : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_BLANK_BP:
                                return "Failed to Update Blank BP : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_MULTIPLE_DAILY_CASH:
                                return "Failed to Update Multiple Daily Cash : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class in (Daily Valuation).!(" + error + ")";

                        }
                    case AutomationStepsConstants.EXPNL:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.RESTART_EXPNL:
                                return "Expnl has not re started. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REFRESH_EXPNL_UI:
                                return "Failed to refresh Expnl UI. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_EXPNL_CONFIG:
                                return "Failed to Update Expnl Config. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class in (Daily Valuation).!(" + error + ")";
                        }
                    case AutomationStepsConstants.GENERAL_LEDGER:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_CASH_TRANSACTION:
                                return "Cash transaction has not been added.! \n ( " + error + " )";
                            case AutomationStepsConstants.ADD_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been added.! \n (" + error + " )";
                            case AutomationStepsConstants.ADD_NON_TRADING_TRANSACTION_ITEM:
                                return "Non trading transaction item has not been added.! \n (" + error + " )";
                            case AutomationStepsConstants.ADD_OPENING_BALANCE_TRANSACTION_ITEM:
                                return "Opening balance transaction item has not been added.!\n (" + error + " )";
                            case AutomationStepsConstants.ADD_OPENING_BALANCE_TRANSACTION:
                                return "Opening balance transaction has not been added.! \n (" + error + " )";
                            case AutomationStepsConstants.CALCULATE_DAILY_CALCULATION:
                                return "Daily calculations have not been calculated.!\n (" + error + " )";
                            case AutomationStepsConstants.CALCULATE_DAY_END_CASH:
                                return "Day end cash has not been calculated.! \n (" + error + " )";
                            case AutomationStepsConstants.CLOSE_GENERAL_LEDGER:
                                return "General Ledger has not been closed.! \n (" + error + " )";
                            case AutomationStepsConstants.DELETE_CASH_TRANSACTION:
                                return "Cash transaction has not been deleted.! \n (" + error + " )";
                            case AutomationStepsConstants.DELETE_NON_TRADING_TRANSACTION_ITEM:
                                return "Non trading transaction item has not been deleted.! \n (" + error + " )";
                            case AutomationStepsConstants.DELETE_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been deleted.! \n (" + error + " )";
                            case AutomationStepsConstants.DELETE_OPENING_BALANCE_TRANSACTION_ITEM:
                                return "Opening balance transaction item has not been deleted.!\n (" + error + " )";
                            case AutomationStepsConstants.DELETE_OPENING_BALANCE_TRANSACTION:
                                return "Opening balance transaction has not been deleted.!\n (" + error + " )";
                            case AutomationStepsConstants.EDIT_CASH_TRANSACTION:
                                return "Cash transaction has not been editted.!\n (" + error + " )";
                            case AutomationStepsConstants.EDIT_NON_TRADING_TRANSACTION_ITEM:
                                return "Non trading transaction item has not been editted.! \n (" + error + " )";
                            case AutomationStepsConstants.EDIT_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been editted.!\n \n (" + error + " )";
                            case AutomationStepsConstants.EDIT_OPENIGN_BALANCE_TRANSACTION_ITEM:
                                return "Opening balance transaction item has not been editted.! \n (" + error + " )";
                            case AutomationStepsConstants.EDIT_OPENIGN_BALANCE_TRANSACTION:
                                return "Opening balance transaction has not been editted.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_ACCOUNT_BALANCE:
                                return "Account balance has not been fetched.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_ACCOUNT_DETAILS:
                                return "Account details have not been fetched.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_ACTIVITY_DATA:
                                return "Activity data has not been fetched.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_CASH_TRANSACTION:
                                return "Cash transaction has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_DAILY_CALCULATION_DATA:
                                return "Daily calculations data has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_DAY_END_CASH:
                                return "Day end cash has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_DIVIDEND_DATA:
                                return "Dividend data has not been fetched.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been fetched.!\n (" + error + " )";
                            case AutomationStepsConstants.GET_OPENING_BALANCE_DATA:
                                return "Opening balance data has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_REVALUATION_DATA:
                                return "Revaluation data has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.GET_TRADING_TRANSACTION:
                                return "Trading transaction has not been fetched.! \n (" + error + " )";
                            case AutomationStepsConstants.RUN_REVALUATION:
                                return "Revaluation not run properly.!\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_CASH_TRANSACTION:
                                return "Cash transaction has not been selected.!\n (" + error + " )";
                            case AutomationStepsConstants.SAVE_TRANSACTION:
                                return "Save Transaction has not been performed.!\n (" + error + " )";
                            case AutomationStepsConstants.SELECT_NON_TRADING_TRANSACTION_ITEM:
                                return "Non trading transaction item has not been selected.! \n (" + error + " )";
                            case AutomationStepsConstants.SELECT_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been selected.! \n (" + error + " )";
                            case AutomationStepsConstants.SELECT_OPENNING_BALANCE_TRANSACTION_ITEM:
                                return "Opening balance transaction item has not been selected.! \n (" + error + " )";
                            case AutomationStepsConstants.SELECT_OPENNING_BALANCE_TRANSACTION:
                                return "Opening balance transaction has not been selected.! \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ACCOUNT_DETAILS:
                                return "Account details have not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ACTIVITY:
                                return "Activity has not been verified.! \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_DAILY_CAL_TRANSACTION:
                                return "Daily calulation transactions has not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_DAY_END_CASH:
                                return "Day end cash has not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_DIVIDEND:
                                return "Dividend has not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_NON_TRADING_TRANSACTION:
                                return "Non trading transaction has not been verified.! \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_OPENING_BALANCE_DATA:
                                return "Opening balance data has not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_REVALUATION_DATA:
                                return "Revaluation data has not been verified.!\n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_TRADING_TRANSACTION:
                                return "Trading transaction has not been verified.! \n (" + error + " )";
                            case AutomationStepsConstants.RUN_MANUAL_REVALUATION:
                                return "Run Manual Revaluation has not been executed! \n (" + error + " )";
                            case AutomationStepsConstants.ADD_MULTIPLE_OPENING_BAL_TRAN_ITEM:
                                return "Not able to Add multiple opening balance transaction items! \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_ACCOUNT_BALANCES:
                                return "Account Balances data has not been verified! \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_CASH_TRANSACTION:
                                return "Cash Transaction data has not been verified! \n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class in (General Ledger).!(" + error + ")";
                        }
                    case AutomationStepsConstants.IMPORT:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.IMPORT:
                                return "Third Party has not imported.! \n (" + error + " )";
                            case AutomationStepsConstants.APPLY_FILTER_ON_IMPORT:
                                return "Failed to Apply Filter On Import. \n(" + error + ")";
                            case AutomationStepsConstants.SELECT_ALL_ON_IMPORT:
                                return "Failed to select all on import. \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class in (General Ledger).!(" + error + ")";
                        }
                    case AutomationStepsConstants.MULTI_TRADING_TICKET:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.BULK_CHANGE_TRADE_DONE_AWAY:
                                return "Failed to do bulk change using done away on MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.DONE_AWAY_USING_MTT:
                                return "Failed to do done away using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EDIT_TRADE_USING_MTT:
                                return "Failed to edit trade using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GET_PRICE_USING_MTT:
                                return "Failed to send order using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SELECT_TRADE_USING_MTT:
                                return "Failed to select trade using Multi TT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SEND_ORDER_USING_MTT:
                                return "Failed to send order using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_MTT:
                                return "Failed to verify the MTT grid data.Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_OR_REMOVE_COLUMN:
                                return "Failed to add or remove column. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.APPLY_FILTER_ON_MTT_GRID:
                                return "Failed to apply filter on mtt grid. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.BULK_CHANGE_CREATE_STAGE_ORDER:
                                return "Failed to do bulk change using create stage on MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.BULK_EDIT_UPDATE_FROM_MTT:
                                return "Failed to do bulk edit update on MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.BULK_UPDATE_USING_MTT:
                                return "Failed to do bulk update using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REFRESH_PRICE_USING_MTT:
                                return "Failed to do refresh price using MTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SAVE_LAYOUT_USING_MTT:
                                return "Failed to do Save layout using MTT. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Multi Trading Ticket).!(" + error + ")";
                        }
                    case AutomationStepsConstants.PORTFOLIO_MANAGEMENT:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CHECK_DASHBOARD_PM:
                                return "Failed to check dashboard on PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_DASHBOARD_WITH_PM:
                                return "Failed to check dashboard with PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_PORTFOLIO_MANAGEMENT:
                                return "Failed to check PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_SUMMARY_ROWS_PM:
                                return "Failed to check summary rows on PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_PM:
                                return "Error occured while closing portfolio management. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.FILTER_PM:
                                return "Failed to filter data on PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.GROUP_ROW_PM:
                                return "Failed to Group the row on PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_PTT_FROM_PM:
                                return "Failed to open PTT from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REMOVE_FILTER_PM:
                                return "Failed to remove filter from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_CLOSING_FROM_PM:
                                return "Failed to open closing from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_CORPORATE_ACTION_FROM_PM:
                                return "Failed to open corporate action from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_CREATE_TRANSACTION_FROM_PM:
                                return "Failed to open create transaction from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_DAILY_VALUATION_FROM_PM:
                                return "Failed to open daily valuation from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_ACCOUNT_POSITION_FROM_PM:
                                return "Failed to close account position from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_POSITION_FROM_PM:
                                return "Failed to close position from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SHOW_SUMMARY_TOOL_FROM_PM:
                                return "Failed to show summary tool from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.HIDE_SUMMARY_TOOL_FROM_PM:
                                return "Failed to hide summary tool from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_SELECTED_TAXLOTS_FROM_PM:
                                return "Failed to close selected taxlots from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EXPAND_COLLAPSE_PM:
                                return "Failed to expand and collapse from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_CUSTOM_VIEW_PM:
                                return "Failed to add custom view from PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_CUSTOM_VIEW_RIGHT_CLICK_PM:
                                return "Failed to add custom view from right click on PM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.RENAME_CUSTOM_VIEW_PM:
                                return "Failed to rename custom view from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.DELETE_CUSTOM_VIEW_PM:
                                return "Failed to delete custom view from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_TRADE_FROM_PM:
                                return "Failed to add trades from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_PM_LIVE_FEED:
                                return "Failed to check live feed on PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_TAXLOT_DETAILS:
                                return "Failed to check taxlot details on PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_ACCOUNT_POSITION_FROM_PM:
                                return "Failed to check account position from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.INCREASE_POSITION_FROM_PM:
                                return "Failed to Increase position from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_CUSTOM_VIEW:
                                return "Failed to custom view from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_PI_FROM_PM:
                                return "Failed to open PI from PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SAVE_LAYOUT_PM:
                                return "Failed to save layout of PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SORT_AND_VERIFY_PM:
                                return "Failed to sort and verify PM . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VIEW_SETTINGS:
                                return "Failed to load view settings of PM . Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Portfolio Management).!(" + error + ")";
                        }
                    case AutomationStepsConstants.PRANA_CLIENT:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.RESTART_CLIENT:
                                return "Prana Client has not restarted. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.LOGIN_CLIENT:
                                return "Failed to login into Client . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CLOSE_CLIENT:
                                return "Failed to close the Client. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.MOVE_FILE:
                                return "Failed to Move the file. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.RUN_PORTFOLIO:
                                return "Failed to run the portfolio. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.UPDATE_CLIENT_CONFIG:
                                return "Failed to update client config. Reason : \n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class (PranaClient Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.Pricing_Input:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.EODTPreferences:
                                return "Failed to fetch the EODT Preferences. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.Live_Data:
                                return "Failed to fetch live data on pricing input. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.Pricing_Input:
                                return "Failed to update the pricing input. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.Update_Shares_Outstanding:
                                return "Failed to update shares outstanding. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UNCHECK_DIVIDEND_YIELD:
                                return "Failed to uncheck dividend yield. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Pricing Input Module).!(" + error + ")";

                        }
                    case AutomationStepsConstants.PRICING_SERVER:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.RESTART_PRICING_SERVER:
                                return "Pricing Server has not restarted. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_PRICING_CONFIG:
                                return "Failed to update pricing config. Reason : \n(" + error + ")";   
                            default:
                                return "Step does not exist in Error Message class (PricingServer).!(" + error + ")";
                        }
                    case AutomationStepsConstants.PTT:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CALCULATE_PTT:
                                return "Failed to calculate PTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CREATE_ORDER_PTT:
                                return "Failed to create order PTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.TRADE_PTT:
                                return "Failed to trade PTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_PTT:
                                return "Failed to  Verify PTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SET_PTT_PREFERENCE:
                                return "Failed to set Input Parameters in PTT Preference. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SET_LONGSHORT_PTT_PREFERENCE:
                                return "Failed to set Long Short Preference in PTT Preference. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SET_ACCOUNT_FACTOR:
                                return "Failed to set Account Factors in PTT Preference. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SELECT_PTT_RECORD:
                                return "Failed to select the PTT record. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EDIT_PTT_RECORD:
                                return "Failed to Edit the PTT Record.: \n(" + error + ")";
                            case AutomationStepsConstants.AUTO_STAGE_IMPORT_FROM_PTT_EXPORT:
                                return "Failed to Auto stage import by export from ptt. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EDIT_PTT_RECORED_AND_EXECUTE_ORDER:
                                return "Failed to Edit the PTT Record and execute order. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SET_DEFAULT_SYMBOLOGY:
                                return "Failed to set default symbology. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_PREFERENCES_FROM_PTT:
                                return "Failed to open preferences from PTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_PTT_COMPLIANCE:
                                return "Failed to verify compliance popup on PTT. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (PTT Module).!(" + error + ")";

                        }
                    case AutomationStepsConstants.SYMBOL_LOOKUP:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.GETDATA_SYMBOL_LOOKUP:
                                return "Unable to fetch symbol through Symbol look up. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SYMBOL_LOOKUP:
                                return "Data is not present on symbol lookup grid. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SYMBOL_LOOKUP_FROM_TT:
                                return "Symbol look up can not open through TT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_UDA:
                                return "UDA has not updated. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_SYMBOL:
                                return "Failed to add symbol on SM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.EDIT_SM_GRID_UI:
                                return "Failed to edit sm grid. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SELECT_FROM_SM_GRID_UI:
                                return "Failed to Select symbol on SM grid. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.TRADE_SYMBOL_FROM_SM:
                                return "Failed to trade symbol from SM. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_SECURITY_INFO_ON_SM:
                                return "Failed to update security info on sm. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Symbol lookup Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.THIRD_PARTY:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.SELECT_THIRD_PARTY:
                                return "Third Party preferences are not selected. Reason : \n("+error+")";
                            case AutomationStepsConstants.THIRD_PARTY_GENERATE:
                                return "Third Party preferences are not generated. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.THIRD_PARTY_SEND:
                                return "Third party preferences not send. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VIEW_THIRD_PARTY:
                                return "Third party preferences are not viewed. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_THIRD_PARTY_VIEW:
                                return "Third party View data are not verified. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_THIRD_PARTY_GENERATE:
                                return "Third party Generate data are not verified. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_THIRD_PARTY_SEND:
                                return "Third party Send data are not verified. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Third party Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.TRADE_SERVER:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.RESTART_SERVER:
                                return "Trade Server has not started. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.BROKER_CONNECTION_THROUGH_TUI:
                                return "Failed to Set broker connection through tui. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_TRADE_SERVER:
                                return "Trade server not closed. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CONNECT_TO_SIMULATOR:
                                return "Failed to connect with simulator. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.START_TRADE_SERVER:
                                return "Trade server not started. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPDATE_SERVER_CONFIG:
                                return "Failed to update server config. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Trade Server).!(" + error + ")";
                        }
                    case AutomationStepsConstants.TRADING_TICKET:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CLICK_SYSMBOL_LOOK_UP:
                                return "Not Able to Open Symbol Lookup.!\n (" + error + " )";
                            case AutomationStepsConstants.CREATE_DONE_AWAY_ORDER:
                                return "Not Able Enter Trade using DoneAway Button.!\n (" + error + " )";
                            case AutomationStepsConstants.CREATE_NEW_SUB_ORDER:
                                return "Not Able Create Sub Order of Stage Order.!\n (" + error + " )";
                            case AutomationStepsConstants.CREATE_REPLACE_ORDER:
                                return "Not Able Enter Trade using Replace Button.!\n (" + error + " )";
                            case AutomationStepsConstants.CREATE_STAGE_ORDER:
                                return "Not Able Create Stage Order.!\n (" + error + " )";
                            case AutomationStepsConstants.ENTER_DETAILS_ON_TT:
                                return "Failed to enter details on TT. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.OPEN_TT_FROM_PST:
                                return "Failed to open PTT. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SET_BROKER_WISE_PREFERENCES_TT:
                                return "Failed to set broker wie preferences. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SET_GENERAL_PREFERENCE:
                                return "Not Able to Set General Preferences.!\n (" + error + " )";
                            case AutomationStepsConstants.SET_UI_PREFERENCES:
                                return "Not Able to Set UI Preferences.!\n (" + error + " )";
                            case AutomationStepsConstants.TRADE_BLOOMBERG:
                                return "Not Able Enter Trade Bloomberg Trade.!\n (" + error + " )";
                            case AutomationStepsConstants.TRADE_NEW_SUB:
                                return "Failed to trade new sub. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VALIDATE_SYMBOL_TT:
                                return "Failed to trade new sub. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_DATA_TT:
                                return "Failed to verify data TT . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_TT:
                                return "Failed to verify TT . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.ADD_SYMBOL_ON_RA_LIST:
                                return "Failed to add symbol on RA List . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CHECK_PENDING_NEW_ORDER_ALERT:
                                return "Failed to check pending new order alert . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CREATE_COMPLIANCE_ORDER:
                                return "Not able to create compliance order . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CREATE_LIVE_ORDER:
                                return "Not able to create live order . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CREATE_REPLACE_LIVE_ORDER:
                                return "Not able to create replace live order . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CUSTOM_ORDER_TT:
                                return "Not able to create custom order on TT. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.DELETE_SYMBOL_LIST_FROM_RA:
                                return "Failed to delete symbol list from RA . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.EXPORT_SYMBOL_FROM_RA_LIST:
                                return "Failed to export symbol from RA list . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.IMPORT_SYMBOL:
                                return "Failed to Import Symbol . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.REMOVE_SYMBOL_FROM_RA_LIST:
                                return "Not able to remove symbol fron RA list . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SEND_LIVE_BLOOMBERG:
                                return "Failed to send live bloomberg trade . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SET_TT_COMPLIANCE_PREFERENCE:
                                return "Failed to set tt compliance preference . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.TOGGLE_BLOOMBERG_ON_RA:
                                return "Not able to toggle bloomberg on RA . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.UPDATE_MASTER_FUND_ON_TT:
                                return "Failed to update master fund on TT . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_COMPLIANCE_ORDER:
                                return "Failed to verify compliance order . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_CUSTOM_ALLOCATION_ON_TT:
                                return "Failed to verify custom allocation on TT . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_CUSTOM_ALLOCATION_SECOND_UI:
                                return "Failed to verify custom allocation second ui of TT . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VIEW_ALLOCATION:
                                return "Not able to view allocation on TT . Reason : \n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Trading Ticket).!(" + error + ")";
                        }
                    case AutomationStepsConstants.CAMERON_SIMULATOR:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ACKNOWLEDGE_TRADE:
                                return "Failed to Acknowlege the trade on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CANCEL_TRADE:
                                return "Failed to cancel the trade on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.CLEAR_UI:
                                return "Failed to Clear UI of the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.DONE_FOR_DAY:
                                return "Not able to click on the done for day on the simulator . Reason : \n (" + error + " )";
                            case AutomationStepsConstants.EXECUTE_TRADE:
                                return "Failed to Execute the trade on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.OPEN_LIVE_TT:
                                return "Failed to open live TT. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.REJECT_TRADE:
                                return "Failed to reject the trade on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SELECT_TRADE:
                                return "Failed to Select the trade on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SET_MANUAL_RESPONSE:
                                return "Failed to set the manual response on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.SET_TO_AUTOMATIC_RESPONSE:
                                return "Failed to set the automatic response on the simulator. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_TAG_IN_ALL_FIXLOGS:
                                return "Not able to verify tagin all fix logs. Reason : \n (" + error + " )";
                            case AutomationStepsConstants.VERIFY_TRADES:
                                return "Not able to verify trade on the simulator. Reason : \n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Cameron Simulator).!(" + error + ")";

                        }
                    case AutomationStepsConstants.DROPCOPY:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.UPLOAD_DROPCOPY_FILE:
                                return "Failed to upload dropcopy file. Reason :\n("+ error +")";
                            default:
                               return "Step does not exist in Error Message class (Rebalancer Module).!(" + error + ")";

                        }
                    case AutomationStepsConstants.REBALANCER:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.FETCH_ACCOUNT_POSITIONS:
                                return "Fetch account positions preferences are not selected. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_CASH:
                                return "Cash is not added. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.RUN_REBALANCE:
                                return "Run Rebalance is not clicked. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_GRID_DATA:
                                return "Rebalancer Grid Data is not available. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_CUSTOM_CASH_USING_IMPORT:
                                return "Failed to add custom cash using import . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ADD_NEW_MODEL_PORTFOLIO:
                                return "Failed to add new model portfolio . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CHECK_BASKET_COMPLIANCE:
                                return "Failed to check data on compliance. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLEAR_CALCULATION:
                                return "Failed to clear calculations . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CLOSE_REBALANCER:
                                return "Not able to close the rebalancer . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ENABLE_COMPLIANCE_TAB:
                                return "Failed to enable compliance tab . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.IMPORT_MODEL_PORTFOLIO:
                                return "Failed to import model portfolio. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.LOCK_AND_UNLOCK_GRID_DATA:
                                return "Failed to Lock and unlock grid data of RB . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.MODIFY_REBALANCE:
                                return "Failed to modify rebalance data on RB . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.NAV_IMPACTING_COMPONENET:
                                return "Failed to update nav impacting component on RB. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REBALANCE_ACROSS_SECURITIES:
                                return "Failed to rebalance data across securities on RB . Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REBALANCE_PREFERENCE:
                                return "Failed to update rebalance preference. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REBALANCE_VIA_BLOOMBERG:
                                return "Failed to rebalance via bloomberg. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.REFRESH_GRID_DATA:
                                return "Failed to update refresh grid data. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SEND_TO_STAGING:
                                return "Failed to send the stages by RB. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SEND_TO_STAGING_EXPORT:
                                return "Failed to send the stage using export by RB. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SYMBOLOGY_CHANGE_RB:
                                return "Failed to change symbology on RB. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Rebalancer Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.NAVLOCK:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_NAVLOCK:
                                return "Failed to add navlock. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.DELETE_EXISTING_NAVLOCK:
                                return "Failed to delete existing navlock. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_NAVLOCK_POPUP:
                                return "Failed to verify navlock popup. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_ROWS_NAVLOCK:
                                return "Failed to verify navlock ui rows. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (Navlock Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.WATCH_LIST:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.ADD_SYMBOL_TO_WATCHLIST:
                                return "Failed to add symbol to the watchlist. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.DELETE_SYMBOL_FROM_WATCHLIST:
                                return "Failed to delete symbol from watchlist. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.TRADE_FROM_WATCHLIST:
                                return "Not able to trade from watchlist. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SYMBOLS_ON_WATCHLIST:
                                return "Failed to verify symbols on watchlist. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (WatchList Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.AUDITTRAIL:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.GET_DATA_FROM_AUDITTRAIL:
                                return "Failed to get data from audit trail. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_AUDIT_TRAIL:
                                return "Failed to verify trades on AudiTrail. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (AuditTrail Module).!(" + error + ")";
                        }
                    case AutomationStepsConstants.SHORT_LOCATE:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.APPLY_FILTER_ON_SHORT_LOCATE:
                                return "Failed to apply filter on shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.DOWNLOAD_FILE:
                                return "Failed to download file from shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.IMPORT_ON_SHORT_LOCATE:
                                return "Failed to import on shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OPEN_TT_FROM_SHORT_LOCATE:
                                return "Failed to open tt from shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SEARCH_SHORT_LOCATE_UI:
                                return "Failed to search short locate UI. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SET_SHORT_LOCATE_PREFERENCES:
                                return "Failed to set short locate preferences. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UPLOAD:
                                return "Failed to upload file on shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VERIFY_SHORT_LOCATE_GRID:
                                return "Failed to verify shortlocate grid data. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (ShortLocate Module).!(" + error + ")";
                        }
                    case MessageConstants.MODULE_APPLICATION:
                        switch (stepName)
                        {
                            case MessageConstants.APPLICATION_START_UP:
                                return ""+MessageConstants.STARTUP_ERROR+" \n (" + error + " )";
                            case MessageConstants.LOGIN_TO_APPLICATION:
                                return "" + MessageConstants.LOGIN_ERROR + " \n (" + error + " )";
                            default:
                                return "Step does not exist in Error Message class (Prana Application).!(" + error + ")";
                        }
                   case AutomationStepsConstants.RTPNL:
                        switch (stepName) 
                        {
                            case AutomationStepsConstants.CheckSummaryDashboard:
                                return "Verification failed on step CheckSummaryDashboard. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CheckFundLevelAggregation:
                                return "Verification failed on step CheckFundLevelAggregation. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CheckAccountLevelAggregation:
                                return "Verification failed on step CheckAccountLevelAggregation. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CheckSymbolLevelAggregation:
                                return "Verification failed on step CheckSymbolLevelAggregation. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CheckRealTimeSymbolFundMonitor:
                                return "Verification failed on step CheckRealTimeSymbolFundMonitor. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CheckRealTimeSymAccountMonitor:
                                return "Verification failed on step CheckRealTimeSymAccountMonitor. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (RTPNL Module).!(" + error + ")";
                        }
                   case AutomationStepsConstants.OpenFin:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.CheckOpenFinWindowName:
                                return "Window verification failed. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OpenWindowFromSearch:
                                return "Failed while opening window from dock search. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.APPLY_FILTER_ON_SHORT_LOCATE:
                                return "Failed to apply filter on shortlocate. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.VerifyColour:
                                return "Failed while doing verification of colour. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.OpenReleaseClient:
                                return "Failed on step OpenReleaseClient. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ActionOnPageFromSearch:
                                return "Failed while doing action on window from dock search. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.SavePageView:
                                return "Failed on SavePageView step. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.ChangeThemeOnModule:
                                return "Failed on ChangeThemeOnModule step. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.CloseService:
                                return "Failed on CloseService step. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.LogOutSamsara:
                                return "Failed on LogOutSamsara step. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (OpenFin Module).!(" + error + ")";
                        }
                   case AutomationStepsConstants.QTT:
                        switch (stepName)
                        {
                            case AutomationStepsConstants.OpenQtt:
                                return "Failed to open QTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.TradeQTT:
                                return "Failed to do Trade on QTT. Reason : \n(" + error + ")";
                            case AutomationStepsConstants.UseOrVerifyHotKeys:
                                return "Failed on using or on verifying HotKeys on QTT. Reason : \n(" + error + ")";
                            default:
                                return "Step does not exist in Error Message class (QTT Module).!(" + error + ")";
                        }
                    default:
                        return "Module does not exist in Error Message class.!(" + error + ")";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return message;
        }
    }
}
