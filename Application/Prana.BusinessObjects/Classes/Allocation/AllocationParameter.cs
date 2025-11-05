// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-29-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="AllocationParameter.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// Definition for allocation parameter according to which allocation will be done
    /// </summary>
    [Serializable]
    public class AllocationParameter
    {
        /// <summary>
        /// The _is virtual(used for MF pref generated parameter)
        /// </summary>
        private bool _isVirtual = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is virtual(for MF pref generated parameter); otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual
        {
            get { return _isVirtual; }
            set { _isVirtual = value; }
        }
        /// <summary>
        /// Private SerializableDictionary which contains accountWise target percentage
        /// </summary>
        private SerializableDictionary<int, AccountValue> _targetPercentage;
        /// <summary>
        /// Returns the collection of AccountWise TargetPercentage
        /// </summary>
        /// <value>The target percentage.</value>
        public SerializableDictionary<int, AccountValue> TargetPercentage { get { return _targetPercentage; } }

        /// <summary>
        /// private CheckListWisePreference
        /// </summary>
        private AllocationRule _checkListWisePreference;

        /// <summary>
        /// Returns the checklistWise preference
        /// </summary>
        /// <value>The check list wise preference.</value>
        public AllocationRule CheckListWisePreference { get { return _checkListWisePreference; } }

        /// <summary>
        /// Id of AllocationOperationPreference which will be used
        /// </summary>
        private int _preferenceId = -1;

        /// <summary>
        /// Id of AllocationOperationPreference which can be used to get state
        /// </summary>
        /// <value>The preference identifier.</value>
        public int PreferenceId { get { return _preferenceId; } }

        /// <summary>
        /// Defines whether allocation to be done by strategy or not
        /// </summary>
        private bool _doStrategyAllocation = false;

        /// <summary>
        /// Defines whether allocation to be done by strategy or not
        /// </summary>
        public bool DoStrategyAllocation { get { return _doStrategyAllocation; } }

        /// <summary>
        /// if isPreview then state will not be updated.
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// The _match closing transaction accounts
        /// </summary>
        private List<int> _matchClosingTransactionAccounts = new List<int>();

        /// <summary>
        /// Gets the match closing transaction accounts.
        /// </summary>
        /// <value>
        /// The match closing transaction accounts.
        /// </value>
        public IList<int> MatchClosingTransactionAccounts
        {
            get
            {
                if (_matchClosingTransactionAccounts != null)
                    return _matchClosingTransactionAccounts.AsReadOnly();
                else return new List<int>();
            }
        }

        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        /// 
        public AllocationParameter()
            : this(null, null, -1, -1, false, false)
        { }


        /// <summary>
        /// The _user identifier
        /// </summary>
        private int _userId = -1;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get { return _userId; } }

        /// <summary>
        /// if Do check side then state will be checked for order side.
        /// 
        /// </summary>
        public bool DoCheckSide { get; set; }

        /// <summary>
        /// Constructor which initializes the object along with values
        /// </summary>
        /// <param name="checkListWisePreference">Preference for this object</param>
        /// <param name="targetPercentage">TargetPercentage for this object</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public AllocationParameter(AllocationRule checkListWisePreference, SerializableDictionary<int, AccountValue> targetPercentage, int preferenceId, int userId, bool doStrategyAllocation, bool isPreview = false, List<int> accounts = null)
        {
            try
            {
                this._checkListWisePreference = checkListWisePreference;
                this._targetPercentage = targetPercentage;
                this._preferenceId = preferenceId;
                this._userId = userId;
                this._doStrategyAllocation = doStrategyAllocation;
                if (accounts != null)
                    this._matchClosingTransactionAccounts = accounts;
                this.IsPreview = isPreview;
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

            }
        }
        /// <summary>
        /// Updates Target percentage
        /// </summary>
        /// <param name="percentage">Cache of account id and account value</param>
        public void UpdatePercentage(SerializableDictionary<int, AccountValue> percentage)
        {
            try
            {
                _targetPercentage = percentage;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update user id in parameter.
        /// </summary>
        /// <param name="userId"></param>
        public void UpdateUserId(int userId)
        {
            try
            {
                this._userId = userId;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Equalses the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            try
            {
                if (obj == null)
                    return false;

                if (GetType() != obj.GetType())
                    return false;

                AllocationParameter parameter = obj as AllocationParameter;
                if (!this.TargetPercentage.Equals(parameter.TargetPercentage) || !this.CheckListWisePreference.Equals(parameter.CheckListWisePreference) || this.PreferenceId != parameter.PreferenceId || this.DoStrategyAllocation != parameter.DoStrategyAllocation || this.IsPreview != parameter.IsPreview || this.UserId != parameter.UserId || this.DoCheckSide != parameter.DoCheckSide)
                    return false;

                return true;
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
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            try
            {
                unchecked
                {
                    // Tested this hash code within possible range of values
                    int multiplier = 131101;
                    int hash = multiplier;
                    hash = (hash * multiplier) + TargetPercentage.GetHashCode();
                    hash = (hash * multiplier) + CheckListWisePreference.GetHashCode();
                    hash = (hash * multiplier) + PreferenceId.GetHashCode();
                    hash = (hash * multiplier) + DoStrategyAllocation.GetHashCode();
                    hash = (hash * multiplier) + IsPreview.GetHashCode();
                    hash = (hash * multiplier) + UserId.GetHashCode();
                    hash = (hash * multiplier) + DoCheckSide.GetHashCode();
                    hash = (hash * multiplier) + ((_matchClosingTransactionAccounts == null || _matchClosingTransactionAccounts.Count == 0) ? 0 : _matchClosingTransactionAccounts.GetHashCode());

                    return hash;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return -1;
            }
        }

        /// <summary>
        /// Determines whether [contains valid allocation rule].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [contains valid allocation rule]; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValidAllocationRule(out string errorMessage)
        {
            bool isValid = false;
            try
            {
                if (!CheckListWisePreference.IsValid(out errorMessage))
                    isValid = false;
                else
                {
                    switch (CheckListWisePreference.RuleType)
                    {
                        case MatchingRuleType.None:
                        case MatchingRuleType.SinceInception:
                        case MatchingRuleType.SinceLastChange:
                            if (TargetPercentage != null && TargetPercentage.Count > 0)
                                isValid = true;
                            else
                            {
                                errorMessage = "Target percentage is not given for this preference";
                                isValid = false;
                            }
                            break;

                        case MatchingRuleType.Prorata:
                        case MatchingRuleType.ProrataByNAV:
                        case MatchingRuleType.Leveling:
                            isValid = true;
                            break;

                        default:
                            isValid = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                errorMessage = "Something went wrong, Please contact administrator";
            }
            return isValid;
        }

        public bool ForceAllocation { get; set; }
    }
}
