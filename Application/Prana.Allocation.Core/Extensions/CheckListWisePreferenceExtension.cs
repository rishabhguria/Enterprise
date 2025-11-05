// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 08-04-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="CheckListWisePreferenceExtension.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;

/// <summary>
/// The Extensions namespace.
/// </summary>
namespace Prana.Allocation.Core.Extensions
{
    /// <summary>
    /// Extension class definition for CheckListWisePreference. Extension methods are used to separate the logic from actual implementation
    /// </summary>
    internal static class CheckListWisePreferenceExtension
    {

        /// <summary>
        /// Checks whether this allocation group is allowed or not
        /// </summary>
        /// <param name="pref">Called from inner method as extension</param>
        /// <param name="group">AllocationGroup for which it will be checked</param>
        /// <returns>True, if allowed, otherwise false</returns>
        internal static bool IsAllowed(this CheckListWisePreference pref, AllocationGroup group)
        {

            try
            {
                bool containsExchange = pref.ExchangeOperator == CustomOperator.All ? true : pref.ExchangeList.Contains(group.ExchangeID);
                if ((pref.ExchangeOperator == CustomOperator.Exclude && containsExchange) || (pref.ExchangeOperator == CustomOperator.Include && !containsExchange))
                    return false;

                bool containsAsset = pref.AssetOperator == CustomOperator.All ? true : pref.AssetList.Contains(group.AssetID);
                if ((pref.AssetOperator == CustomOperator.Exclude && containsAsset) || (pref.AssetOperator == CustomOperator.Include && !containsAsset))
                    return false;

                bool containsOrderSide = pref.OrderSideOperator == CustomOperator.All ? true : pref.OrderSideList.Contains(group.OrderSideTagValue);
                if ((pref.OrderSideOperator == CustomOperator.Exclude && containsOrderSide) || (pref.OrderSideOperator == CustomOperator.Include && !containsOrderSide))
                    return false;

                if (group.AssetID == (int)AssetCategory.Future || group.AssetID == (int)AssetCategory.FutureOption)
                {
                    // Deciding root for a given symbol should be in security master service but the logic to find root of a future/future option is written here
                    // TODO: As discussed with Om shiv, this can be done here for now but should be moved to central place
                    string root = group.Symbol.Split(' ')[0];
                    bool containsPR = pref.PROperator == CustomOperator.All ? true : pref.PRList.Contains(root);
                    if ((pref.PROperator == CustomOperator.Exclude && containsPR) || (pref.PROperator == CustomOperator.Include && !containsPR))
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
