// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-30-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AllocationOperationPreference.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.Constants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// This class contains preference of a given company
    /// </summary>
    [Serializable]
    public class AllocationOperationPreference
    {
        /// <summary>
        /// The _is virtual(used for MF pref generated preference)
        /// </summary>
        private bool _isVirtual = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is virtual(for MF pref generated preference); otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual
        {
            get { return _isVirtual; }
            set { _isVirtual = value; }
        }

        /// <summary>
        /// This private object contains the Id of the given company
        /// </summary>
        private int _companyId = int.MinValue;
        /// <summary>
        /// Returns the Id of the company for this object
        /// </summary>
        /// <value>The company identifier.</value>
        public int CompanyId { get { return _companyId; } }

        /// <summary>
        /// Priority position of given preference
        /// </summary>
        private int _positionPrefId = int.MinValue;
        /// <summary>
        /// Returns the priority position of given preference for this object
        /// </summary>
        /// <value>The company identifier.</value>
        public int PositionPrefId { get { return _positionPrefId; } }

        /// <summary>
        /// Id of the operation preference
        /// </summary>
        private int _operationPreferenceId = int.MinValue;

        /// <summary>
        /// Gets OperationPreferenceId for this object
        /// </summary>
        /// <value>The operation preference identifier.</value>
        public int OperationPreferenceId
        {
            get { return _operationPreferenceId; }
            set { _operationPreferenceId = value; }
        }

        /// <summary>
        /// Name of the operationPreference
        /// </summary>
        private string _operationPreferenceName = string.Empty;

        /// <summary>
        /// Gets the name of the operationPreference
        /// </summary>
        /// <value>The name of the operation preference.</value>
        public string OperationPreferenceName { get { return _operationPreferenceName; } }

        /// <summary>
        /// Update time of the current object
        /// </summary>
        private DateTime _updateDateTime = DateTime.MinValue;

        /// <summary>
        /// This read-only property returns the date time when this object was modified
        /// </summary>
        /// <value>The update date time.</value>
        public DateTime UpdateDateTime { get { return _updateDateTime; } }

        /// <summary>
        /// Private object of accountWise collection of target percentage
        /// </summary>
        private SerializableDictionary<int, AccountValue> _targetPercentage;
        private readonly object _targetPercentageLock = new object();

        /// <summary>
        /// Returns the cloned object of target percentage
        /// </summary>
        /// <value>The target percentage.</value>
        public SerializableDictionary<int, AccountValue> TargetPercentage
        {
            get
            {
                SerializableDictionary<int, AccountValue> result = new SerializableDictionary<int, AccountValue>();
                foreach (int accountId in _targetPercentage.Keys)
                {
                    result.Add(accountId, _targetPercentage[accountId].Clone());
                }
                return result;
            }
        }

        /// <summary>
        /// The is preference visible
        /// </summary>
        private bool _isPrefVisible = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefVisible
        {
            get { return _isPrefVisible; }
            set { _isPrefVisible = value; }
        }

        //public void Add(int key, AccountValue accountVal)
        //{
        //    _targetPercentage.Add(key, accountVal);
        //}

        //public void Add(int key, CheckListWisePreference pref)
        //{
        //    _checkListPreference.Add(key, pref);
        //}


        /// <summary>
        /// This method update target percentage
        /// </summary>
        /// <param name="accountValue">Account value to be added</param>
        /// <returns>true if added successfully otherwise false</returns>
        /// <exception cref="System.Exception">This account already has been added in this operation preference</exception>
        public bool TryUpdateTargetPercentage(AccountValue accountValue)
        {
            if (this._targetPercentage.ContainsKey(accountValue.AccountId))
                throw new Exception("This account already has been added in this operation preference");

            this._targetPercentage.Add(accountValue.AccountId, accountValue);
            return true;
        }

        /// <summary>
        /// Updates the target percentage for this object (First clone and store)
        /// </summary>
        /// <param name="target">Target to be updated</param>
        /// <returns>True if updated successfully otherwise false</returns>
        public bool TryUpdateTargetPercentage(SerializableDictionary<int, AccountValue> target)
        {
            try
            {
                lock (_targetPercentageLock)
                {
                    this._targetPercentage = target;
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

        /// <summary>
        /// private SerializableDictionary for checkListWisePreference
        /// </summary>
        private SerializableDictionary<int, CheckListWisePreference> _checkListPreference;
        private readonly object _checkListPreferenceLock = new object();

        /// <summary>
        /// Returns the cloned object of CheckListWisePreference
        /// </summary>
        /// <value>The check list wise preference.</value>
        public SerializableDictionary<int, CheckListWisePreference> CheckListWisePreference
        {
            get
            {
                SerializableDictionary<int, CheckListWisePreference> result = new SerializableDictionary<int, CheckListWisePreference>();
                foreach (int checkListid in _checkListPreference.Keys)
                {
                    result.Add(checkListid, _checkListPreference[checkListid].Clone());
                }
                return result;
            }
        }

        /// <summary>
        /// Return cloned instance of the checklist id or null if not exists
        /// </summary>
        /// <param name="checkListId">Id for which checklist is required</param>
        /// <returns>Instance of CheckListWisePreference</returns>
        public CheckListWisePreference GetCheckListPreferenceForId(int checkListId)
        {
            try
            {
                if (_checkListPreference.ContainsKey(checkListId))
                    return _checkListPreference[checkListId].Clone();
                else
                    return null;
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
        /// Default rule for this preference
        /// </summary>
        private AllocationRule _defaultRule;

        /// <summary>
        /// Read-only property (returns cloned object, so modification will not have any impact)
        /// </summary>
        /// <value>The default rule.</value>
        public AllocationRule DefaultRule { get { return _defaultRule.Clone(); } }

        /// <summary>
        /// This method tries to update default rule for this object
        /// </summary>
        /// <param name="defaultRule">Rule which to be updated</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryUpdateDefaultRule(AllocationRule defaultRule)
        {
            try
            {
                this._defaultRule = defaultRule.Clone();
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
        /// Returns the list of ids of checklists that are present in this preference
        /// </summary>
        /// <returns>List&lt;System.Int32&gt;.</returns>
        public List<int> GetCheckListIds()
        {
            try
            {
                return _checkListPreference.Keys.ToList();
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
        /// This method will try to update the checklistwisePref of this object
        /// <para>This method will throw exception if this id has already in this list</para>
        /// </summary>
        /// <param name="checkListWisePref">Preference which will be added</param>
        /// <param name="isOverwrite">if set to <c>true</c> [is overwrite].</param>
        /// <returns>True if added successfully otherwise false</returns>
        /// <exception cref="System.Exception">This Checklist already has been added in this operation preference</exception>
        public bool TryUpdateCheckList(CheckListWisePreference checkListWisePref, bool isOverwrite = false)
        {
            try
            {
                if (this._checkListPreference.ContainsKey(checkListWisePref.ChecklistId) && !isOverwrite)
                    throw new Exception("This Checklist already has been added in this operation preference");
                else if (this._checkListPreference.ContainsKey(checkListWisePref.ChecklistId) && isOverwrite)
                    this._checkListPreference.Remove(checkListWisePref.ChecklistId);

                this._checkListPreference.Add(checkListWisePref.ChecklistId, checkListWisePref);
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
        /// Updates the checkListWisePref for this object (First clone and store)
        /// </summary>
        /// <param name="checkListWisePref">checkListWisePref to be updated</param>
        /// <returns>True if updated successfully otherwise false</returns>
        public void UpdateCheckList(SerializableDictionary<int, CheckListWisePreference> checkListWisePref)
        {
            try
            {
                SerializableDictionary<int, CheckListWisePreference> checkListWisePreferenceDictionary = new SerializableDictionary<int, CheckListWisePreference>();
                foreach (CheckListWisePreference cPref in checkListWisePref.Values)
                {
                    checkListWisePreferenceDictionary.Add(cPref.ChecklistId, cPref.Clone());
                }
                lock (_checkListPreferenceLock)
                {
                    this._checkListPreference = checkListWisePreferenceDictionary;
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
        /// Default constructor (created for XML serialization)
        /// </summary>
        public AllocationOperationPreference()
            : this(-1, -1, -1, String.Empty, new AllocationRule(), DateTimeConstants.MinValue, true)
        { }

        /// <summary>
        /// Constructor for this object
        /// </summary>
        /// <param name="operationPreferenceId">Id of the preference for this object</param>
        /// <param name="companyId">Id of the company for which this preference is set</param>
        /// <param name="operationPreferenceName">Name of the preference for this object</param>
        /// <param name="defaultRule">Default rule Object which will be used in case there is no checklistId present</param>
        /// <param name="updateDateTime">Update time of this object</param>
        public AllocationOperationPreference(int operationPreferenceId, int companyId, int positionPrefId, string operationPreferenceName, AllocationRule defaultRule, DateTime updateDateTime, bool isPrefVisible)
        {
            try
            {
                this._companyId = companyId;
                this._positionPrefId = positionPrefId;
                this._operationPreferenceId = operationPreferenceId;
                this._operationPreferenceName = operationPreferenceName;
                this._updateDateTime = updateDateTime;
                this._defaultRule = defaultRule;
                this._isPrefVisible = isPrefVisible;
                this._checkListPreference = new SerializableDictionary<int, CheckListWisePreference>();
                this._targetPercentage = new SerializableDictionary<int, AccountValue>();
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
        /// Clones(Deep) the object
        /// </summary>
        /// <returns>Cloned instance</returns>
        public AllocationOperationPreference Clone()
        {
            try
            {
                AllocationOperationPreference pref = new AllocationOperationPreference(this._operationPreferenceId, this._companyId, this._positionPrefId, this._operationPreferenceName, this._defaultRule.Clone(), this._updateDateTime, this._isPrefVisible);

                foreach (CheckListWisePreference cPref in this._checkListPreference.Values)
                {
                    pref._checkListPreference.Add(cPref.ChecklistId, cPref.Clone());
                }

                foreach (AccountValue targetAccountValue in this._targetPercentage.Values)
                {
                    pref._targetPercentage.Add(targetAccountValue.AccountId, targetAccountValue.Clone());
                }
                pref._parentPrefAccountsList = this._parentPrefAccountsList == null ? null : new List<int>(_parentPrefAccountsList);
                return pref;
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
        /// Checks if data of rule is valid
        /// </summary>
        /// <returns>True if valid otherwise false</returns>
        public bool IsValid()
        {
            string errorMessage;
            return IsValid(out errorMessage);
        }

        /// <summary>
        /// Checks if data of rule is valid and returns error string
        /// </summary>
        /// <param name="errorMessage">Return error message string</param>
        /// <returns>True if valid otherwise false</returns>
        public bool IsValid(out string errorMessage)
        {
            try
            {
                if (_defaultRule.RuleType != MatchingRuleType.Prorata && _defaultRule.RuleType != MatchingRuleType.Leveling && _defaultRule.RuleType != MatchingRuleType.ProrataByNAV)
                {
                    // Checking for sum of percentage
                    decimal sumPercentage = 0.0M;
                    foreach (int accountId in _targetPercentage.Keys)
                    {
                        //if value is negative invalid preference
                        if (_targetPercentage[accountId].Value < 0)
                        {
                            errorMessage = AllocationStringConstants.PERCENTAGE_NOT_NEGATIVE + AllocationStringConstants.ACCOUNTS;
                            return false;
                        }
                        decimal strategyPercentage = 0.0M;
                        sumPercentage += _targetPercentage[accountId].Value;
                        foreach (StrategyValue strategy in _targetPercentage[accountId].StrategyValueList)
                        {
                            //if value is negative invalid preference
                            if (strategy.Value < 0)
                            {
                                errorMessage = AllocationStringConstants.PERCENTAGE_NOT_NEGATIVE + AllocationStringConstants.STRATEGY;
                                return false;
                            }
                            strategyPercentage += strategy.Value;
                        }
                        if (strategyPercentage != 0 && !strategyPercentage.EqualsPrecise(100M))
                        {
                            errorMessage = AllocationStringConstants.PERCENTAGE_SUM_NOT_100 + AllocationStringConstants.STRATEGY;
                            return false;
                        }
                    }
                    if (!sumPercentage.EqualsPrecise(100M))
                    {
                        errorMessage = AllocationStringConstants.PERCENTAGE_SUM_NOT_100 + AllocationStringConstants.ACCOUNTS;
                        return false;
                    }
                }

                if (_defaultRule == null)
                {
                    errorMessage = AllocationStringConstants.NO_DEFAULT_RULE;
                    return false;
                }
                else if (!_defaultRule.IsValid(out errorMessage))
                {
                    errorMessage = string.Concat(AllocationStringConstants.DEFAULT_RULE_INVALID, errorMessage);
                    return false;
                }

                //check for duplicate checkLists
                if (_checkListPreference.Values.Count != _checkListPreference.Values.Distinct().Count())
                {
                    errorMessage = AllocationStringConstants.DUPLICATE_GENERAL_RULE;
                    return false;
                }

                foreach (int checkListId in _checkListPreference.Keys)
                {
                    if (!_checkListPreference[checkListId].IsValid(out errorMessage))
                    {
                        errorMessage = string.Concat(AllocationStringConstants.GENERAL_RULE_INVALID, errorMessage);
                        return false;
                    }
                }

                // TODO: Check for combination of checklists, whether they are exclusive or not
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                errorMessage = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Check if AllocationOperationPreference are equal, PRANA-12383
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AllocationOperationPreference item = obj as AllocationOperationPreference;
            if (item == null)
                return false;

            bool isEqual = (IsCheckListWisePreferenceEqual(this.CheckListWisePreference, item.CheckListWisePreference)
                && this.CompanyId == item.CompanyId
                && this.DefaultRule.Equals(item.DefaultRule)
                && this.OperationPreferenceId == item.OperationPreferenceId
                && this.OperationPreferenceName == item.OperationPreferenceName
                && this.PositionPrefId == item.PositionPrefId
                && IsTargetPercentageEqual(this.TargetPercentage, item.TargetPercentage)
                && this.UpdateDateTime == item.UpdateDateTime);
            return isEqual;
        }

        /// <summary>
        /// Check if CheckListWisePreference are equal, PRANA-12383
        /// </summary>
        /// <param name="checkListWisePreferenceExiting"></param>
        /// <param name="checkListWisePreferenceNew"></param>
        /// <returns></returns>
        private bool IsCheckListWisePreferenceEqual(SerializableDictionary<int, CheckListWisePreference> checkListWisePreferenceExiting, SerializableDictionary<int, CheckListWisePreference> checkListWisePreferenceNew)
        {
            bool isEqual = true;
            try
            {
                if (checkListWisePreferenceExiting.Count != checkListWisePreferenceNew.Count)
                    return false;
                Parallel.ForEach(checkListWisePreferenceExiting.Keys, key =>
                {
                    if (!checkListWisePreferenceNew.ContainsKey(key) || (checkListWisePreferenceNew.ContainsKey(key) && (!checkListWisePreferenceExiting[key].IsCheckListEqual(checkListWisePreferenceNew[key]))))
                        isEqual = false;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isEqual;
        }


        /// <summary>
        /// Check if TargetPercentage are equal, PRANA-12383
        /// </summary>
        /// <param name="targetPercentageExisting"></param>
        /// <param name="targetPercentageNew"></param>
        /// <returns></returns>
        private bool IsTargetPercentageEqual(SerializableDictionary<int, AccountValue> targetPercentageExisting, SerializableDictionary<int, AccountValue> targetPercentageNew)
        {
            bool isEqual = true;
            try
            {
                if (targetPercentageExisting.Count != targetPercentageNew.Count)
                    return false;
                Parallel.ForEach(targetPercentageExisting.Keys, key =>
                {
                    if (!targetPercentageNew.ContainsKey(key) || (targetPercentageNew.ContainsKey(key) && (!targetPercentageExisting[key].Equals(targetPercentageNew[key]))))
                        isEqual = false;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isEqual;
        }

        /// <summary>
        /// Determines whether [is match closing used].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is match closing used]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatchClosingUsed()
        {
            try
            {
                if (_defaultRule.MatchClosingTransaction != MatchClosingTransactionType.None || (_checkListPreference != null && _checkListPreference.Values.Any(x => x.Rule.MatchClosingTransaction != MatchClosingTransactionType.None)))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is leveling used].
        /// </summary>
        /// <returns></returns>
        public bool IsLevelingUsed()
        {
            try
            {
                if (_defaultRule.RuleType == MatchingRuleType.Leveling || _checkListPreference.Values.Any(x => x.Rule.RuleType == MatchingRuleType.Leveling))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is Prorata/Nav used].
        /// </summary>
        /// <returns></returns>
        public bool IsProrataByNAVUsed()
        {
            try
            {
                if (_defaultRule.RuleType == MatchingRuleType.ProrataByNAV || _checkListPreference.Values.Any(x => x.Rule.RuleType == MatchingRuleType.ProrataByNAV))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// The _parent preference accounts list
        /// </summary>
        private List<int> _parentPrefAccountsList;


        /// <summary>
        /// Tries to update the parent Pref accounts list.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns></returns>
        public bool TryUpdateAccountsList(List<int> accounts)
        {
            try
            {
                if (accounts != null)
                {
                    this._parentPrefAccountsList = accounts;
                    return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the selected accounts list.
        /// </summary>
        /// <returns></returns>
        public List<int> GetSelectedAccountsList(bool isGeneralRuleIncluded = false)
        {
            List<int> accounts = new List<int>();
            try
            {
                if (_parentPrefAccountsList != null && _parentPrefAccountsList.Count > 0)
                    accounts = new List<int>(_parentPrefAccountsList);
                else
                {
                    switch (_defaultRule.RuleType)
                    {
                        case MatchingRuleType.None:
                        case MatchingRuleType.SinceInception:
                        case MatchingRuleType.SinceLastChange:
                            accounts = _targetPercentage.Keys.ToList();
                            break;
                        case MatchingRuleType.Leveling:
                        case MatchingRuleType.Prorata:
                        case MatchingRuleType.ProrataByNAV:
                            accounts = new List<int>(_defaultRule.ProrataAccountList);
                            break;
                    }
                    if (isGeneralRuleIncluded && _checkListPreference != null)
                    {
                        foreach (CheckListWisePreference clwp in _checkListPreference.Values)
                        {
                            accounts.AddRange(clwp.GetAllocationAccountsList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accounts;

        }

        /// <summary>
        /// Gets the selected accounts with postion.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, decimal> GetSelectedAccountsWithPostionPct()
        {
            Dictionary<int, decimal> accountsWithPostionPct = new Dictionary<int, decimal>();
            try
            {
                foreach (var accountValue in _targetPercentage)
                {
                    accountsWithPostionPct.Add(accountValue.Key, accountValue.Value.Value);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountsWithPostionPct;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}