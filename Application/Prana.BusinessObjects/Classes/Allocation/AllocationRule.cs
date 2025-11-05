// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-16-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AllocationRule.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// Combination of basic properties which comprises the allocation rule
    /// </summary>
    [Serializable]
    public class AllocationRule
    {
        /// <summary>
        /// Base type of the allocation which will be used to allocate the given allocationGroup
        /// </summary>
        /// <value>The type of the base.</value>
        public AllocationBaseType BaseType { get; set; }

        /// <summary>
        /// This defines that whether historical allocation will
        /// be considered while allocating the current allocationGroup or not and if considered since when it will be considered
        /// </summary>
        /// <value>The type of the rule.</value>
        public MatchingRuleType RuleType { get; set; }

        /// <summary>
        /// This property is used to set neutral position across accounts.
        /// If new trades comes and position of that symbol becomes zero at portfolio level
        /// then setting if true will allocate each account in such a way that all account becomes zero for that given symbol
        /// </summary>
        /// <value><c>true</c> if [match portfolio position]; otherwise, <c>false</c>.</value>
        public MatchClosingTransactionType MatchClosingTransaction { get; set; }

        /// <summary>
        /// If it is not -1 then remaining quantity will go the specified account
        /// </summary>
        /// <value>The preference account identifier.</value>
        public int PreferenceAccountId { get; set; }

        /// <summary>
        /// List of selected accounts
        /// </summary>
        public List<int> ProrataAccountList { get; set; }

        /// <summary>
        /// Number of days to be subtracted from today.
        /// </summary>
        public int ProrataDaysBack { get; set; }

        /// <summary>
        /// This method clones the current object and returns
        /// </summary>
        /// <returns>AllocationRule.</returns>
        public AllocationRule Clone()
        {
            try
            {
                AllocationRule rule = new AllocationRule();
                rule.BaseType = this.BaseType;
                rule.MatchClosingTransaction = this.MatchClosingTransaction;
                rule.PreferenceAccountId = this.PreferenceAccountId;
                rule.RuleType = this.RuleType;
                rule.ProrataAccountList = this.ProrataAccountList;
                rule.ProrataDaysBack = this.ProrataDaysBack;
                return rule;
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
                return null;
            }
        }

        /// <summary>
        /// Overridden Equals method to use this as key for dictionary
        /// </summary>
        /// <param name="obj">Object to be checked</param>
        /// <returns>True if equals otherwise false</returns>
        public override bool Equals(object obj)
        {
            try
            {
                if (obj == null)
                    return false;

                if (GetType() != obj.GetType())
                    return false;

                AllocationRule toCheck = obj as AllocationRule;
                if (this.BaseType == toCheck.BaseType &&
                    this.MatchClosingTransaction == toCheck.MatchClosingTransaction &&
                    this.PreferenceAccountId == toCheck.PreferenceAccountId &&
                    this.RuleType == toCheck.RuleType)
                {
                    if (this.RuleType == MatchingRuleType.Leveling || this.RuleType == MatchingRuleType.Prorata || this.RuleType == MatchingRuleType.ProrataByNAV)
                        return (toCheck.ProrataAccountList != null && this.ProrataAccountList.Count.Equals(this.ProrataAccountList.Intersect(toCheck.ProrataAccountList).Count()) &&
                                this.ProrataDaysBack == toCheck.ProrataDaysBack);
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            try
            {
                unchecked
                {
                    // Tested this hash code within possible range of values
                    int multiplier = 131101;
                    int hash = multiplier;
                    hash = (hash * multiplier) + BaseType.GetHashCode();
                    hash = (hash * multiplier) + RuleType.GetHashCode();
                    hash = (hash * multiplier) + MatchClosingTransaction.GetHashCode();
                    hash = (hash * multiplier) + PreferenceAccountId.GetHashCode();

                    return hash;
                }
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
                return -1;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return BaseType.ToString() + "_" + RuleType.ToString() + "_" + MatchClosingTransaction + "_" + PreferenceAccountId;
        }



        public bool TryAddProrataAccount(int accountId)
        {
            try
            {

                if (this.ProrataAccountList == null)
                    this.ProrataAccountList = new List<int>();
                if (!this.ProrataAccountList.Contains(accountId))
                    this.ProrataAccountList.Add(accountId);
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

        /// <summary>
        /// Check if rule is valid or not
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool IsValid(out string errorMessage)
        {
            bool isValid = true;
            errorMessage = string.Empty;
            try
            {
                switch (this.RuleType)
                {
                    case MatchingRuleType.Prorata:
                    case MatchingRuleType.ProrataByNAV:
                    case MatchingRuleType.Leveling:
                        if (this.ProrataAccountList == null || this.ProrataAccountList.Count == 0)
                        {
                            errorMessage = AllocationStringConstants.ACCOUNT_LIST_NOT_EMPTY + this.RuleType.ToString().ToLower();
                            isValid = false;
                        }
                        break;
                }
                return isValid;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isValid;
        }
    }
}
