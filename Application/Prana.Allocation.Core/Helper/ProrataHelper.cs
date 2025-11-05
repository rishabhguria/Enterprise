using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Core.Helper
{
    internal class ProrataHelper
    {
        /// <summary>
        /// Gets state for prorataAllocation and calculate percentage
        /// </summary>
        /// <param name="parameter">AllocationParameter</param>
        /// <param name="list">List<AllocationGroup></param>
        /// <param name="percentage">output</param>
        /// <param name="symbol"></param>
        /// <returns>Error message</returns>
        //internal static string GetPercentageForProrata(AllocationParameter parameter, out SerializableDictionary<int, AccountValue> percentage, string symbol, List<string> groupsToExcludeFromStateCalculation)
        // We have now sent state directly to this method instead of calculating state here, as we are already calculating it once in Generate method. Avoiding two DB calls PRANA-21444
        internal static string GetPercentageForProrata(List<int> prorataAccountList, out SerializableDictionary<int, AccountValue> percentage, string symbol, Dictionary<int, AccountValue> state)
        {
            try
            {
                //Dictionary<int, AccountValue> state = UserWiseStateCache.Instance.GetCurrentStateForDays(parameter.CheckListWisePreference.ProrataDaysBack, parameter.CheckListWisePreference.BaseType, symbol, parameter.UserId, groupsToExcludeFromStateCalculation);
                if (state == null || state.Count == 0)
                {
                    percentage = null;
                    return symbol + ": There is no state for symbol Prorata allocation.";
                }
                else
                {
                    percentage = GetPercentageForState(state, prorataAccountList);
                    bool percentageCalculated = (from g in percentage
                                                 where g.Value.Value == 0.0M
                                                 select g).ToList().Count != percentage.Count;

                    if (!percentageCalculated || (percentage == null || percentage.Count == 0))
                        return symbol + ": Not able to calculate percentage as there is no state for symbol in selected accounts.";
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                percentage = null;
                return "Something went Wrong, Please contact admisintrator.";
            }
        }

        /// <summary>
        /// Calculate percenatge for selected accounts.
        /// </summary>
        /// <param name="state">State of symbol</param>
        /// <param name="prorataAccountList">List of accounts</param>
        /// <returns>percentage</returns>
        private static SerializableDictionary<int, AccountValue> GetPercentageForState(Dictionary<int, AccountValue> state, List<int> prorataAccountList)
        {
            try
            {
                SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
                decimal totalvalue = (from s in state
                                      where prorataAccountList.Contains(s.Key)
                                      select Math.Abs(s.Value.Value)).Sum();

                foreach (int id in state.Keys)
                {
                    if (prorataAccountList.Contains(id))
                    {
                        decimal per = totalvalue == 0 ? 0 : (Math.Abs((decimal)state[id].Value) * 100) / totalvalue;
                        AccountValue account = new AccountValue(id, per);
                        percentage.Add(id, account);
                    }
                }
                return percentage;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
