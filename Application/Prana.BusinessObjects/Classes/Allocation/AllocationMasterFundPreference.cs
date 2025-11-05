using Prana.BusinessObjects.Classes.Utilities;
using Prana.BusinessObjects.Constants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.BusinessObjects.Classes.Allocation
{
    [Serializable]
    public class AllocationMasterFundPreference
    {
        #region Members

        /// <summary>
        /// The company identifier
        /// </summary>
        private int _companyId = int.MinValue;

        /// <summary>
        /// The master fund preference identifier
        /// </summary>
        private int _masterFundPreferenceId = int.MinValue;

        /// <summary>
        /// The master fund preference name
        /// </summary>
        private string _masterFundPreferenceName = string.Empty;

        /// <summary>
        /// The master fund target percentage
        /// </summary>
        private SerializableDictionary<int, decimal> _masterFundTargetPercentage;
        private readonly object _masterFundTargetPercentageLock = new object();

        /// <summary>
        /// The master fund preference
        /// </summary>
        private SerializableDictionary<int, int> _masterFundPreference;
        private readonly object _masterFundPreferenceLock = new object();

        /// <summary>
        /// The update date time
        /// </summary>
        private DateTime _updateDateTime = DateTime.MinValue;

        /// <summary>
        /// Default rule for this preference
        /// </summary>
        private AllocationRule _defaultRule;

        /// <summary>
        /// the target percentage
        /// </summary>
        private SerializableDictionary<int, AccountValue> _targetPercentage;
        private readonly object _targetPercentageLock = new object();

        /// <summary>
        /// checklist wise preference
        /// </summary>
        private SerializableDictionary<int, CheckListWisePreference> _checkListPreference;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public int CompanyId
        {
            get { return _companyId; }
        }

        /// <summary>
        /// Gets or sets the calculated preference for each master fund .
        /// </summary>
        /// <value>
        /// The master fund preference.
        /// </value>
        public SerializableDictionary<int, int> MasterFundPreference
        {
            get { return _masterFundPreference; }
        }

        /// <summary>
        /// Gets the master fund preference identifier.
        /// </summary>
        /// <value>
        /// The master fund preference identifier.
        /// </value>
        public int MasterFundPreferenceId
        {
            get { return _masterFundPreferenceId; }
            set { _masterFundPreferenceId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the master fund preference.
        /// </summary>
        /// <value>
        /// The name of the master fund preference.
        /// </value>
        public string MasterFundPreferenceName
        {
            get { return _masterFundPreferenceName; }
            set { _masterFundPreferenceName = value; }
        }

        /// <summary>
        /// Gets the master fund target percentage.
        /// </summary>
        /// <value>
        /// The master fund target percentage.
        /// </value>
        public SerializableDictionary<int, decimal> MasterFundTargetPercentage
        {
            get { return _masterFundTargetPercentage; }
        }

        /// Gets the update date time.
        /// </summary>
        /// <value>
        /// The update date time.
        /// </value>
        public DateTime UpdateDateTime
        {
            get { return _updateDateTime; }
        }

        /// <summary>
        /// Read-only property (returns cloned object, so modification will not have any impact)
        /// </summary>
        /// <value>The default rule.</value>
        public AllocationRule DefaultRule { get { return _defaultRule.Clone(); } }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationMasterFundPreference"/> class.
        /// </summary>
        public AllocationMasterFundPreference()
            : this(-1, -1, string.Empty, DateTimeConstants.MinValue, new AllocationRule())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationMasterFundPreference"/> class.
        /// </summary>
        /// <param name="masterFundPreferenceId">The master fund preference identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="masterFundPreferenceName">Name of the master fund preference.</param>
        /// <param name="updateDateTime">The update date time.</param>
        public AllocationMasterFundPreference(int masterFundPreferenceId, int companyId, string masterFundPreferenceName, DateTime updateDateTime, AllocationRule defaultRule)
        {
            try
            {
                this._companyId = companyId;
                this._masterFundPreferenceId = masterFundPreferenceId;
                this._masterFundPreferenceName = masterFundPreferenceName;
                this._updateDateTime = updateDateTime;
                this._masterFundPreference = new SerializableDictionary<int, int>();
                this._masterFundTargetPercentage = new SerializableDictionary<int, decimal>();
                this._checkListPreference = new SerializableDictionary<int, CheckListWisePreference>();
                this._defaultRule = defaultRule;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

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
        /// Tries the update target percentage.
        /// </summary>
        /// <param name="accountValue">The Account Value.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">This account already has been added in this operation preference.</exception>

        public bool TryUpdateTargetPercentage(AccountValue accountValue)
        {
            if (this._targetPercentage.ContainsKey(accountValue.AccountId))
                throw new Exception("This account already has been added in this operation preference");

            this._targetPercentage.Add(accountValue.AccountId, accountValue);
            return true;
        }

        /// <summary>
        /// Tries the update the Check List.
        /// </summary>
        /// <param name="checkListWisePref">The checklistwise preference.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">This Checklist already has been added in this operation preference.</exception>
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
        /// Tries the update target percentage.
        /// </summary>
        /// <param name="targetPercentage">The target percentage.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Sum of percentage is not 100 for master fund allocation ratio.</exception>
        public void UpdateTargetPercentage(SerializableDictionary<int, decimal> targetPercentage)
        {
            try
            {
                lock (_masterFundTargetPercentageLock)
                {
                    this._masterFundTargetPercentage = targetPercentage;
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
        /// Tries the update master fund preference.
        /// </summary>
        /// <param name="mfId">The mf identifier.</param>
        /// <param name="calculatedPrefId">The calculated preference identifier.</param>
        /// <returns></returns>
        public void AddUpdateMasterFundPreference(int mfId, int calculatedPrefId)
        {
            try
            {
                lock (_masterFundPreferenceLock)
                {
                    if (_masterFundPreference.ContainsKey(mfId))
                        _masterFundPreference[mfId] = calculatedPrefId;
                    else
                        _masterFundPreference.Add(mfId, calculatedPrefId);
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
        /// Updates the master fund preference.
        /// </summary>
        /// <param name="masterFundPref">The master fund preference.</param>
        public void UpdateMasterFundPreference(SerializableDictionary<int, int> masterFundPref)
        {
            try
            {
                lock (_masterFundPreferenceLock)
                {
                    this._masterFundPreference = masterFundPref;
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
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public AllocationMasterFundPreference Clone()
        {
            AllocationMasterFundPreference pref = new AllocationMasterFundPreference(this._masterFundPreferenceId, this._companyId, this._masterFundPreferenceName, this._updateDateTime, this._defaultRule.Clone());

            foreach (KeyValuePair<int, int> masterFundPref in this._masterFundPreference)
            {
                pref._masterFundPreference.Add(masterFundPref.Key, masterFundPref.Value);
            }

            foreach (KeyValuePair<int, decimal> masterFundTargetPerc in this._masterFundTargetPercentage)
            {
                pref._masterFundTargetPercentage.Add(masterFundTargetPerc.Key, masterFundTargetPerc.Value);
            }

            return pref;
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
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public bool IsValid(out string errorMessage)
        {
            try
            {
                if (_defaultRule.RuleType != MatchingRuleType.Prorata && _defaultRule.RuleType != MatchingRuleType.Leveling && _defaultRule.RuleType != MatchingRuleType.ProrataByNAV)
                {
                    // Checking for sum of percentage
                    decimal sumPercentage = 0.0M;
                    foreach (int accountId in _masterFundTargetPercentage.Keys)
                    {
                        //if value is negative invalid preference
                        if (_masterFundTargetPercentage[accountId] < 0)
                        {
                            errorMessage = "\n\nPercentage cannot be negative.";
                            return false;
                        }

                        sumPercentage += _masterFundTargetPercentage[accountId];
                    }
                    if (!sumPercentage.EqualsPrecise(100M))
                    {
                        errorMessage = "\n\nSum of Percentage is not 100! \nAllocation % Entered: " + sumPercentage.ToString() + "\nRemaining %: " + (100 - sumPercentage).ToString();
                        return false;
                    }
                    if (!_masterFundPreference.Keys.Count.Equals(_masterFundTargetPercentage.Keys.Count))
                    {
                        errorMessage = "\n\nThe calculated preference should be defined for all selected master funds";
                        return false;
                    }
                }
                else
                {
                    if (!_masterFundPreference.Keys.Count.Equals(_defaultRule.ProrataAccountList.Count))
                    {
                        errorMessage = "\n\nThe calculated preference should be defined for all selected master funds";
                        return false;
                    }
                }
                if (_defaultRule == null)
                {
                    errorMessage = "No default rule exist for this preference."; ;
                    return false;
                }
                else if (!_defaultRule.IsValid(out errorMessage))
                {
                    return false;
                }

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
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            AllocationMasterFundPreference item = obj as AllocationMasterFundPreference;
            if (item == null)
                return false;

            bool isEqual = this.CompanyId == item.CompanyId
            && this.MasterFundPreferenceId == item.MasterFundPreferenceId
            && this.MasterFundPreferenceName == item.MasterFundPreferenceName
            && this.UpdateDateTime == item.UpdateDateTime
            && this.DefaultRule.Equals(item.DefaultRule)
            && IsTargetPercentageEqual(this.MasterFundTargetPercentage, item.MasterFundTargetPercentage)
            && IsMasterFundPreferencesEqual(this.MasterFundPreference, item.MasterFundPreference);
            return isEqual;
        }

        /// <summary>
        /// Determines whether [is master fund preferences equal] [the specified serializable dictionary1].
        /// </summary>
        /// <param name="serializableDictionary1">The serializable dictionary1.</param>
        /// <param name="serializableDictionary2">The serializable dictionary2.</param>
        /// <returns></returns>
        private bool IsMasterFundPreferencesEqual(SerializableDictionary<int, int> masterFundPreferenceExisting, SerializableDictionary<int, int> masterFundPreferenceNew)
        {
            bool isEqual = true;
            try
            {
                if (masterFundPreferenceExisting.Count != masterFundPreferenceNew.Count)
                    return false;
                Parallel.ForEach(masterFundPreferenceExisting.Keys, key =>
                {
                    if (!masterFundPreferenceNew.ContainsKey(key) || (masterFundPreferenceNew.ContainsKey(key) && (!masterFundPreferenceExisting[key].Equals(masterFundPreferenceNew[key]))))
                        isEqual = false;
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
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
            return _defaultRule.MatchClosingTransaction == MatchClosingTransactionType.CompletePortfolio;
        }

        /// <summary>
        /// Determines whether [is target percentage equal] [the specified target percentage existing].
        /// </summary>
        /// <param name="targetPercentageExisting">The target percentage existing.</param>
        /// <param name="targetPercentageNew">The target percentage new.</param>
        /// <returns></returns>
        private bool IsTargetPercentageEqual(SerializableDictionary<int, decimal> targetPercentageExisting, SerializableDictionary<int, decimal> targetPercentageNew)
        {
            bool isEqual = true;
            try
            {
                targetPercentageExisting = targetPercentageExisting.Where(x => x.Value != 0.0M).ToSerializableDictionary(x => x.Key, x => x.Value);
                targetPercentageNew = targetPercentageNew.Where(x => x.Value != 0.0M).ToSerializableDictionary(x => x.Key, x => x.Value);
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
                    throw;
            }
            return isEqual;
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
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// This method tries to update default rule for this object
        /// </summary>
        /// <param name="defaultRule">Rule which to be updated</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateDefaultRule(AllocationRule defaultRule)
        {
            try
            {
                this._defaultRule = defaultRule.Clone();
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

        #endregion Methods
    }
}
