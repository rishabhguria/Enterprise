using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.TradeManager.Extension.CacheStore;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Blotter.Classes
{
    internal class BlotterUICommonMethods
    {
        /// <summary>
        /// Updates the allocation status of sub orders.
        /// </summary>
        /// <param name="orderAllocationState">State of the order allocation.</param>
        internal static List<string> UpdateAllocationDetails(List<AllocationDetails> allocationDetails, PranaUltraGrid dgBlotter, bool isOrderGrid=false)
        {
            List<string> updatedOrderParentClOrderIDs = new List<string>();
            try
            {
                Dictionary<string, OrderSingle> selectedOrdersDict = BlotterOrderCollections.GetInstance().getSelectedOrderDetails(allocationDetails);
                foreach (UltraGridRow row in dgBlotter.Rows)
                {
                    string rowParentClOrderId = row.Cells[OrderFields.PROPERTY_PARENT_CL_ORDERID].Value.ToString();
                    allocationDetails.ForEach(details =>
                    {

                        if (details.ClOrderID.Equals(rowParentClOrderId))
                        {
                            //Update Values
                            UpdateAllocationColumnsInBlotter(details, (OrderSingle)row.ListObject, row);

                            //Adding updated order parent ClOrderId
                            if (!updatedOrderParentClOrderIDs.Contains(details.ClOrderID))
                                updatedOrderParentClOrderIDs.Add(details.ClOrderID);
                        }
                        else if (selectedOrdersDict.ContainsKey(details.ClOrderID))
                        {
                            OrderSingle orderDetails = selectedOrdersDict[details.ClOrderID];
                            if (orderDetails.StagedOrderID.Equals(rowParentClOrderId) && !isOrderGrid && (orderDetails.TIF.Equals(FIXConstants.TIF_GTC) || orderDetails.TIF.Equals(FIXConstants.TIF_GTD)))
                            {
                                //Update Values
                                UpdateAllocationColumnsInBlotter(details, (OrderSingle)row.ListObject, row);

                                //Adding updated order parent ClOrderId
                                if (!updatedOrderParentClOrderIDs.Contains(details.ClOrderID))
                                    updatedOrderParentClOrderIDs.Add(details.ClOrderID);
                            }
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
            return updatedOrderParentClOrderIDs;
        }

        /// <summary>
        /// Updates the allocation details.
        /// </summary>
        /// <param name="details">The details.</param>
        internal static void UpdateAllocationColumnsInBlotter(AllocationDetails details, OrderSingle order, UltraGridRow row)
        {
            try
            {
                //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
                //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
                if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                {
                    order.AllocationSchemeName = order.AllocationStatus = order.Account = order.MasterFund = order.Strategy = OrderFields.PROPERTY_DASH;
                    return;
                }

                //Allocation Status
                order.AllocationStatus = details.AllocationStatus.ToString();

                //Update Account, Master Fund and Strategy column Values
                if (details.Level1Allocation.Count > 0)
                {
                    order.Account = GetAccountName(details);
                    order.MasterFund = GetMasterFundText(details);
                    order.Strategy = GetStrategyText(details);
                    order.AllocationSchemeName = details.AllocationSchemeName;
                }
                else
                {
                    order.AllocationSchemeName = order.Account = order.MasterFund = order.Strategy = OrderFields.PROPERTY_DASH;
                    order.MasterFund = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(order.TradeAttribute6) ? order.TradeAttribute6 : OrderFields.PROPERTY_DASH;
                }

                if (order.IsUseCustodianBroker && order.Account == OrderFields.PROPERTY_MULTIPLE && order.CounterPartyID == int.MinValue)
                {
                    order.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                }
                else if (order.CounterPartyID != int.MinValue)
                {
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                }

                order.TradeAttribute1 = details.TradeAttribute1;
                order.TradeAttribute2 = details.TradeAttribute2;
                order.TradeAttribute3 = details.TradeAttribute3;
                order.TradeAttribute4 = details.TradeAttribute4;
                order.TradeAttribute5 = details.TradeAttribute5;
                order.TradeAttribute6 = details.TradeAttribute6;
                order.SetTradeAttribute(details.GetTradeAttributesAsDict());
                //To update details of the row in the Grid
                row.Refresh();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the master fund text.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        internal static string GetMasterFundText(AllocationDetails details)
        {
            try
            {
                //Update Master Fund Column Vallue
                HashSet<int> masterFunds = new HashSet<int>();

                details.Level1Allocation.ForEach(level1Allocation =>
                {
                    //Master Fund IDs
                    int masterFundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(level1Allocation.LevelnID);
                    if (!masterFunds.Contains(masterFundId))
                        masterFunds.Add(masterFundId);
                });


                if (masterFunds.Count > 1)
                {
                    return OrderFields.PROPERTY_MULTIPLE;
                }
                else if (masterFunds.Count == 1)
                {
                    //Master Fund Name
                    string masterFundName = CachedDataManager.GetInstance.GetMasterFund(masterFunds.FirstOrDefault());
                    return String.IsNullOrEmpty(masterFundName) ? OrderFields.PROPERTY_DASH : masterFundName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Gets the strategy text.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        private static string GetStrategyText(AllocationDetails details)
        {
            try
            {
                //Update Strategy Column Vallue
                HashSet<int> strategyIDs = new HashSet<int>();

                details.Level1Allocation.Where(level1Allocation => level1Allocation.Childs != null && level1Allocation.Childs.Collection.Count > 0).ToList().ForEach(level1Allocation =>
                {
                    //Strategy IDs
                    level1Allocation.Childs.Collection.ForEach(strategy =>
                    {
                        strategyIDs.Add(strategy.LevelnID);
                    });
                });

                //Strategy
                if (strategyIDs.Count() > 0)
                    return strategyIDs.Count() > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetStrategyText(strategyIDs.FirstOrDefault());
                else
                    return "Strategy Unallocated";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        internal static string GetAccountName(AllocationDetails details)
        {
            try
            {
                return details.Level1Allocation.Count > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetAccountText(details.Level1Allocation[0].LevelnID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Splits the camel case.
        /// </summary>
        /// <param name="colHeader">The col header.</param>
        /// <returns></returns>
        internal static string SplitCamelCase(string colHeader)
        {
            try
            {
                string fieldName = System.Text.RegularExpressions.Regex.Replace(colHeader, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
                return System.Text.RegularExpressions.Regex.Replace(fieldName, @"\s+", " ");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return String.Empty;
            }
        }
    }
}
