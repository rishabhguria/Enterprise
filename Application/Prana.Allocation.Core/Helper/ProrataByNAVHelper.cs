using Prana.Allocation.Common.Enums;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Prana.Allocation.Core.Helper
{
    internal static class ProrataByNAVHelper
    {
        /// <summary>
        /// Gets the percentage for prorata by nav.
        /// </summary>
        /// <param name="prorataAccountList">The prorataAccountList.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns></returns>
        internal static string GetPercentageForProrataByNAV(List<int> prorataAccountList, ImmutableSortedDictionary<int, decimal> accountWiseStartOfDayNav, out SerializableDictionary<int, AccountValue> percentage)
        {
            try
            {
                percentage = GetPercentageForNAV(accountWiseStartOfDayNav, prorataAccountList);
                bool percentageCalculated = (from g in percentage
                                             where g.Value.Value == 0.0M
                                             select g).ToList().Count != percentage.Count;

                if (!percentageCalculated || (percentage == null || percentage.Count == 0))
                    return "Not able to calculate percentage, so Prorata by NAV cannot be done";
                else
                    return "";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;

                percentage = null;
                return "Something went Wrong, Please contact admisintrator.";
            }
        }

        /// <summary>
        /// Gets the percentage for nav.
        /// </summary>
        /// <param name="accountWiseStartOfDayNav">The account wise start of day nav.</param>
        /// <param name="accountList">The account list.</param>
        /// <returns></returns>
        private static SerializableDictionary<int, AccountValue> GetPercentageForNAV(ImmutableSortedDictionary<int, decimal> accountWiseStartOfDayNav, List<int> accountList)
        {
            try
            {
                SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
                decimal totalNAV = accountWiseStartOfDayNav.Where(x => accountList.Contains(x.Key)).Sum(y => y.Value);
                foreach (int id in accountWiseStartOfDayNav.Keys)
                {
                    decimal per = (Math.Abs(accountWiseStartOfDayNav[id]) * 100) / totalNAV;
                    percentage.Add(id, new AccountValue(id, per));
                }
                return percentage;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;

                return null;
            }
        }

        /// <summary>
        /// Gets the start of day nav.
        /// </summary>
        /// <param name="allocLevel">The alloc level.</param>
        /// <param name="fundList">The fund list.</param>
        /// <param name="_startOfDayNav">The start of day nav.</param>
        /// <returns></returns>
        internal static string GetStartOfDayNAV(AllocationLevel allocLevel, List<int> fundList, out ImmutableSortedDictionary<int, decimal> _startOfDayNav)
        {
            StringBuilder errorMessage = new StringBuilder();
            var navBuilder = ImmutableSortedDictionary.CreateBuilder<int, decimal>();
            try
            {
                if (!ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected)
                {
                    errorMessage.Append("Calculation Service disconnected, so Prorata by NAV cannot be done");
                    _startOfDayNav = navBuilder.ToImmutable();
                    return errorMessage.ToString();
                }

                Dictionary<int, decimal> startOfDayNavDictionary = new Dictionary<int, decimal>();
                switch (allocLevel)
                {
                    case AllocationLevel.Account:
                        startOfDayNavDictionary = ExpnlServiceConnector.GetInstance().GetAccountsStartOfDayNAV(fundList, ref errorMessage);
                        break;

                    case AllocationLevel.MasterFund:
                        startOfDayNavDictionary = ExpnlServiceConnector.GetInstance().GetMasterFundStartofDayNAV(fundList, ref errorMessage);
                        break;
                }
                // if nav is negative then set nav as 0
                foreach (int key in startOfDayNavDictionary.Keys)
                {
                    if (startOfDayNavDictionary[key] > 0.0M)
                        navBuilder.Add(key, startOfDayNavDictionary[key]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            _startOfDayNav = navBuilder.ToImmutable();
            if (_startOfDayNav.Count == 0)
                errorMessage.Append("As all selected accounts have zero NAV, so Prorata by NAV cannot be done");
            return errorMessage.ToString();
        }
    }
}
