// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 05-04-2017
// ***********************************************************************
// <copyright file="MFCumQuantityAllocationGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Enums;
using Prana.Allocation.Core.Allocator;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Prana.Allocation.Core.Generator
{
    internal class MFCumQuantityAllocationGenerator : MasterFundAllocator
    {
        /// <summary>
        /// The master fund wise start of day nav
        /// </summary>
        ImmutableSortedDictionary<int, decimal> _masterFundWiseStartOfDayNav;

        /// <summary>
        /// Updates the allocation base type to CumQuantity.
        /// </summary>
        public override void UpdateBaseType()
        {
            try
            {
                BaseType = AllocationBaseType.CumQuantity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the master fund wise start of day nav which will be used for allocation Prorata by NAV methodology
        /// </summary>
        /// <param name="prorataList">The prorata list.</param>
        /// <returns>Error Message</returns>
        public override string UpdateMasterFundWiseNAV(List<int> prorataList, List<AllocationGroup> groups)
        {
            string errorMessage = string.Empty;
            try
            {
                errorMessage = ProrataByNAVHelper.GetStartOfDayNAV(AllocationLevel.MasterFund, prorataList, out _masterFundWiseStartOfDayNav);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Gets the master fund percentage for allocation rule.
        /// </summary>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public override SerializableDictionary<int, AccountValue> GetMasterFundPercentageForAllocationRule(AllocationMasterFundPreference masterFundPref, string symbol, int userId, List<string> groupIds, out string errorMessage)
        {
            SerializableDictionary<int, AccountValue> mfTargetPercentage = new SerializableDictionary<int, AccountValue>();
            errorMessage = string.Empty;
            try
            {
                switch (masterFundPref.DefaultRule.RuleType)
                {
                    case MatchingRuleType.None:
                        if (masterFundPref.MasterFundTargetPercentage != null && masterFundPref.MasterFundTargetPercentage.Count > 0)
                            mfTargetPercentage = masterFundPref.MasterFundTargetPercentage.ToSerializableDictionary(t => t.Key, t => new AccountValue(t.Key, t.Value));
                        else
                            errorMessage = "Master fund target percentage is not defined.";
                        break;

                    case MatchingRuleType.Prorata:
                        // get masterfund quantity state for symbol
                        StringBuilder groupIdList = new StringBuilder();
                        groupIds.ForEach(groupId =>
                        {
                            groupIdList = groupIdList.Append(groupId + ",");
                        });
                        Dictionary<int, AccountValue> currentMasterFundQuantityStateForSymbol = UserWiseStateCache.Instance.GetMasterFundCurrentStateForDays(masterFundPref.DefaultRule.ProrataDaysBack, masterFundPref.DefaultRule.BaseType, symbol, userId, groupIdList);
                        errorMessage = ProrataHelper.GetPercentageForProrata(masterFundPref.DefaultRule.ProrataAccountList, out mfTargetPercentage, symbol, currentMasterFundQuantityStateForSymbol);
                        break;

                    case MatchingRuleType.ProrataByNAV:
                        errorMessage = ProrataByNAVHelper.GetPercentageForProrataByNAV(masterFundPref.DefaultRule.ProrataAccountList, _masterFundWiseStartOfDayNav, out mfTargetPercentage);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfTargetPercentage;
        }
    }
}
