// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-27-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="CheckListWisePreference.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.Constants;
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
    /// ChecklistWise preference. This definition contains the checklist and denotes a given allocation rule
    /// </summary>
    [Serializable]
    public class CheckListWisePreference : IEquatable<CheckListWisePreference>
    {
        #region CheckList identification

        /// <summary>
        /// Private CheckListId
        /// </summary>
        /// <value>The checklist identifier.</value>
        public int ChecklistId { get; set; }

        #endregion

        /// <summary>
        /// Allocation Rule for current checklist
        /// </summary>
        private AllocationRule _rule = new AllocationRule();

        /// <summary>
        /// Read-only property for allocation rule of the object
        /// </summary>
        /// <value>The rule.</value>
        public AllocationRule Rule { get { return _rule; } set { _rule = value; } }

        /// <summary>
        /// Private object of accountWise collection of target percentage
        /// </summary>
        private SerializableDictionary<int, AccountValue> _targetPercentage;

        /// <summary>
        /// Returns the cloned object of target percentage
        /// </summary>
        /// <value>The target percentage.</value>
        public SerializableDictionary<int, AccountValue> TargetPercentage { get { return _targetPercentage; } set { _targetPercentage = value; } }

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
        /// Gets the allocation account list.
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllocationAccountsList()
        {
            List<int> accounts = new List<int>();
            try
            {
                if (_parentPrefAccountsList != null && _parentPrefAccountsList.Count > 0)
                    accounts = new List<int>(_parentPrefAccountsList);
                else
                {
                    switch (_rule.RuleType)
                    {
                        case MatchingRuleType.None:
                        case MatchingRuleType.SinceInception:
                        case MatchingRuleType.SinceLastChange:
                            accounts = _targetPercentage.Keys.ToList();
                            break;
                        case MatchingRuleType.Leveling:
                        case MatchingRuleType.Prorata:
                        case MatchingRuleType.ProrataByNAV:
                            accounts = new List<int>(_rule.ProrataAccountList);
                            break;
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

        #region CheckList Base Properties

        ///// <summary>
        ///// Base type of allocation
        ///// </summary>
        //private AllocationBaseType _baseType = AllocationBaseType.Notional;

        ///// <summary>
        ///// Base type of the allocation which will be used to allocate the given allocationGroup
        ///// </summary>
        //public AllocationBaseType BaseType { get { return _baseType; } }

        ///// <summary>
        ///// Type of matching rule. It can be either None, SinceInception, SinceLastChange
        ///// </summary>
        //private MatchingRuleType _ruleType = MatchingRuleType.None;

        ///// <summary>
        ///// This defines that whether historical allocation will 
        ///// be considered while allocating the current allocationGroup or not and if considered since when it will be considered
        ///// </summary>
        //public MatchingRuleType RuleType { get { return _ruleType; } }

        ///// <summary>
        ///// Match portfolio position before allocation
        ///// </summary>
        //private bool _matchPortfolioPosition = true;

        ///// <summary>
        ///// This property is used to set neutral position across accounts.
        ///// If new trades comes and position of that symbol becomes zero at portfolio level 
        ///// then setting if true will allocate each account in such a way that all account becomes zero for that given symbol
        ///// </summary>
        //public bool MatchPortfolioPosition { get { return _matchPortfolioPosition; } }

        ///// <summary>
        ///// In case of fractional quantity which account will take remaining quantity
        ///// </summary>
        //private int _preferenceAccountId = -1;

        ///// <summary>
        ///// If it is not -1 then remaining quantity will go the specified account
        ///// </summary>
        //public int PreferenceAccountId { get { return _preferenceAccountId; } }

        #endregion

        #region ExchangeCheck Properties

        /// <summary>
        /// Operator for exchageList. It can be either All, Include or Exclude
        /// </summary>
        /// <value>The exchange operator.</value>
        public CustomOperator ExchangeOperator { get; set; }

        /// <summary>
        /// List of exchanges. Usable only in case of Exchange operator is Include or Exclude
        /// </summary>
        /// <value>The exchange list.</value>
        public List<int> ExchangeList { get; set; }

        /// <summary>
        /// This method updates the exchange check and discard any previous values
        /// </summary>
        /// <param name="exchangeOperator">CustomOperator which will used while applying exchange check</param>
        /// <param name="exchangeList">List of exchange which will be used, default value is null</param>
        /// <returns>True if updated successfully or false</returns>
        /// <exception cref="System.Exception">When operator is not All, there should be some values in exchange list</exception>
        public string TryUpdateExchangeCheck(CustomOperator exchangeOperator, List<int> exchangeList = null)
        {
            try
            {
                lock (ExchangeList)
                {
                    //Returned error message instead of throwing new exception
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                    if (exchangeOperator != CustomOperator.All && exchangeList == null)
                        return "exchange";
                    //throw new Exception("When operator is not All, there should be some values in exchange list");
                    else
                    {
                        this.ExchangeOperator = exchangeOperator;
                        this.ExchangeList = exchangeList;
                        return string.Empty;
                    }
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
                return string.Empty;
            }
        }

        /// <summary>
        /// This will add the exchange for this object
        /// </summary>
        /// <param name="exchangeId">Id of the exchange which will be added</param>
        /// <returns>True if added successfully otherwise false</returns>
        public bool TryAddExchange(int exchangeId)
        {
            try
            {
                if (!ExchangeList.Contains(exchangeId))
                    ExchangeList.Add(exchangeId);
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

        #endregion

        #region Order Side Properties

        /// <summary>
        /// Gets or sets the order side operator.
        /// </summary>
        /// <value>
        /// The order side operator.
        /// </value>
        public CustomOperator OrderSideOperator { get; set; }

        /// <summary>
        /// Gets or sets the order side list.
        /// </summary>
        /// <value>
        /// The order side list.
        /// </value>
        public List<string> OrderSideList { get; set; }

        /// <summary>
        /// Tries to update order side discarding all previous values.
        /// </summary>
        /// <param name="orderSideOperator">The order side operator.</param>
        /// <param name="orderSideList">The order side list.</param>
        /// <returns></returns>
        public string TryUpdateOrderSide(CustomOperator orderSideOperator, List<string> orderSideList = null)
        {
            try
            {
                lock (OrderSideList)
                {
                    //Returned error message instead of throwing new exception
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                    if (orderSideOperator != CustomOperator.All && orderSideList == null)
                        return "orderside";
                    //throw new Exception("When operator is not All, there should be some values in exchange list");
                    else
                    {
                        this.OrderSideOperator = orderSideOperator;
                        this.OrderSideList = orderSideList;
                        return string.Empty;
                    }
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Tries to add order side discarding all previous values.
        /// </summary>
        /// <param name="ordersideId">The orderside identifier.</param>
        /// <returns></returns>
        public bool TryAddOrderSide(string ordersideId)
        {
            try
            {
                if (!OrderSideList.Contains(ordersideId))
                    OrderSideList.Add(ordersideId);
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

        #endregion

        #region AssetCheck Properties

        /// <summary>
        /// Operator for exchageList. It can be either All, Include or Exclude
        /// </summary>
        /// <value>The asset operator.</value>
        public CustomOperator AssetOperator { get; set; }

        /// <summary>
        /// List of assets. Usable only in case of asset operator is Include or Exclude
        /// </summary>
        /// <value>The asset list.</value>
        public List<int> AssetList { get; set; }

        /// <summary>
        /// This method updates the asset check and discard any previous values
        /// </summary>
        /// <param name="assetOperator">The asset operator.</param>
        /// <param name="assetList">The asset list.</param>
        /// <returns>True if updated successfully or false</returns>
        /// <exception cref="System.Exception">When operator is not All, there should be some values in asset list</exception>
        public string TryUpdateAssetCheck(CustomOperator assetOperator, List<int> assetList = null)
        {
            try
            {
                lock (AssetList)
                {
                    //Returned error message instead of throwing new exception
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                    if (assetOperator != CustomOperator.All && assetList == null)
                        return "asset";
                    //throw new Exception("When operator is not All, there should be some values in asset list");
                    else
                    {
                        this.AssetOperator = assetOperator;
                        this.AssetList = assetList;
                        return string.Empty;
                    }
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Try to add asset in this object
        /// </summary>
        /// <param name="assetId">Id of the asset which to be added</param>
        /// <returns>True if added successfully otherwise false</returns>
        public bool TryAddAsset(int assetId)
        {
            try
            {
                if (!AssetList.Contains(assetId))
                    AssetList.Add(assetId);
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

        #endregion

        #region PRCheck Properties

        /// <summary>
        /// Gets or sets the pr operator.
        /// </summary>
        /// <value>The pr operator.</value>
        public CustomOperator PROperator { get; set; }

        /// <summary>
        /// List of PR. Usable only in case of PROperator is Include or Exclude
        /// </summary>
        /// <value>The pr list.</value>
        public List<string> PRList { get; set; }

        /// <summary>
        /// This method updates the PR check and discard any previous values
        /// </summary>
        /// <param name="prOperator">The pr operator.</param>
        /// <param name="prList">The pr list.</param>
        /// <returns>True if updated successfully or false</returns>
        /// <exception cref="System.Exception">When operator is not All, there should be some values in PR list</exception>
        public string TryUpdatePRCheck(CustomOperator prOperator, List<string> prList = null)
        {
            try
            {
                lock (PRList)
                {
                    //Returned error message instead of throwing new exception
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                    if (prOperator != CustomOperator.All && prList == null)
                        return "PR list";
                    //throw new Exception("When operator is not All, there should be some values in PR list");
                    else
                    {
                        this.PROperator = prOperator;
                        this.PRList = prList;
                        return string.Empty;
                    }
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Try to add PR in this object
        /// </summary>
        /// <param name="pr">PR which to be added</param>
        /// <returns>True if added successfully otherwise false</returns>
        public bool TryAddPR(string pr)
        {
            try
            {
                if (!PRList.Contains(pr))
                    PRList.Add(pr);
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

        #endregion

        /// <summary>
        /// This method tries to update default rule for this object
        /// </summary>
        /// <param name="defaultRule">Rule which to be updated</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryUpdateDefaultRule(AllocationRule defaultRule)
        {
            try
            {
                this.Rule = defaultRule.Clone();
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
        public bool TryUpdateTargetPercentage(SerializableDictionary<int, AccountValue> targetPercentage)
        {
            try
            {
                this.TargetPercentage = (SerializableDictionary<int, AccountValue>)targetPercentage.Clone();
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
        /// Initializes a new instance of the <see cref="CheckListWisePreference"/> class.
        /// </summary>
        public CheckListWisePreference()
            : this(-1, AllocationBaseType.CumQuantity, MatchingRuleType.None, -1, MatchClosingTransactionType.None, new List<int>(), 0, new SerializableDictionary<int, AccountValue>())
        { }

        /// <summary>
        /// Constructor for this definition. This initialize the mandatory properties from constructor parameters
        /// </summary>
        /// <param name="checkListId">The check list identifier.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="ruleType">Type of the rule</param>
        /// <param name="preferenceAccountId">The preference account identifier.</param>
        /// <param name="MatchClosingTransaction">if set to <c>true</c> [match portfolio position].</param>
        public CheckListWisePreference(int checkListId, AllocationBaseType baseType, MatchingRuleType ruleType, int preferenceAccountId, MatchClosingTransactionType MatchClosingTransaction, List<int> prorataAccountList, int prorataDaysBack, SerializableDictionary<int, AccountValue> TargetPercentage)
        {
            try
            {
                this.ChecklistId = checkListId;
                //this._checkListName = checkListName;
                this._rule.BaseType = baseType;
                //this._targetPercentage = targetPercentage;
                this._rule.RuleType = ruleType;
                this._rule.PreferenceAccountId = preferenceAccountId;
                this._rule.MatchClosingTransaction = MatchClosingTransaction;
                this.ExchangeList = new List<int>();
                this.OrderSideList = new List<string>();
                AssetList = new List<int>();
                PRList = new List<string>();
                this.AssetOperator = CustomOperator.All;
                this.ExchangeOperator = CustomOperator.All;
                this.OrderSideOperator = CustomOperator.All;
                this.PROperator = CustomOperator.All;
                this._rule.ProrataDaysBack = prorataDaysBack;
                this._rule.ProrataAccountList = prorataAccountList;
                this.TargetPercentage = TargetPercentage;
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
        /// Deep Clones this object
        /// </summary>
        /// <returns>CheckListWisePreference.</returns>
        public CheckListWisePreference Clone()
        {
            try
            {
                CheckListWisePreference pref = new CheckListWisePreference(this.ChecklistId, this.Rule.BaseType, this.Rule.RuleType, this.Rule.PreferenceAccountId, this.Rule.MatchClosingTransaction, this.Rule.ProrataAccountList, this.Rule.ProrataDaysBack, this.TargetPercentage);

                pref.AssetList = this.AssetList == null ? null : new List<int>(this.AssetList);
                pref.AssetOperator = this.AssetOperator;

                pref.ExchangeList = this.ExchangeList == null ? null : new List<int>(this.ExchangeList);
                pref.ExchangeOperator = this.ExchangeOperator;

                pref.OrderSideList = this.OrderSideList == null ? null : new List<string>(this.OrderSideList);
                pref.OrderSideOperator = this.OrderSideOperator;

                pref.PRList = this.PRList == null ? null : new List<string>(this.PRList);
                pref.PROperator = this.PROperator;
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
        /// Checks whether current checklist is valid or not
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if the specified error message is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(out string errorMessage)
        {
            bool isValid = true;
            errorMessage = string.Empty;
            try
            {
                string errorMsg = string.Empty;
                if (ExchangeOperator != CustomOperator.All && (ExchangeList == null || ExchangeList.Count == 0))
                {
                    errorMessage += AllocationStringConstants.NO_EXCHANGE_SELECTED + ExchangeOperator.ToString().ToLower();
                    isValid = false;
                }
                else if (ExchangeOperator == CustomOperator.All && ExchangeList != null && ExchangeList.Count > 0)
                {
                    errorMessage += AllocationStringConstants.LIST_EMPTY + AllocationStringConstants.EXCHANGE;
                    isValid = false;
                }
                if (OrderSideOperator != CustomOperator.All && (OrderSideList == null || OrderSideList.Count == 0))
                {
                    errorMessage = AllocationStringConstants.NO_ORDERSIDE_SELECTED + OrderSideOperator.ToString().ToLower();
                    isValid = false;
                }
                else if (OrderSideOperator == CustomOperator.All && OrderSideList != null && OrderSideList.Count > 0)
                {
                    errorMessage += AllocationStringConstants.LIST_EMPTY + AllocationStringConstants.ORDER_SIDE;
                    isValid = false;
                }
                else if (AssetOperator != CustomOperator.All && (AssetList == null || AssetList.Count == 0))
                {
                    errorMessage += AllocationStringConstants.NO_ASSET_SELECTED + AssetOperator.ToString().ToLower();
                    isValid = false;
                }
                else if (AssetOperator == CustomOperator.All && AssetList != null && AssetList.Count > 0)
                {
                    errorMessage += AllocationStringConstants.LIST_EMPTY + AllocationStringConstants.ASSET;
                    isValid = false;
                }
                else if (PROperator != CustomOperator.All && (PRList == null || PRList.Count == 0))
                {
                    errorMessage += AllocationStringConstants.NO_PR_SELECTED + PROperator.ToString().ToLower();
                    isValid = false;
                }
                else if (PROperator == CustomOperator.All && PRList != null && PRList.Count > 0)
                {
                    errorMessage += AllocationStringConstants.LIST_EMPTY + AllocationStringConstants.PR;
                    isValid = false;
                }
                else if (Rule == null || !Rule.IsValid(out errorMsg))
                {
                    errorMessage += errorMsg;
                    isValid = false;
                }
                else if (TargetPercentage == null)
                {
                    errorMessage += AllocationStringConstants.TARGET_PERCENTAGE_NULL;
                    isValid = false;
                }
                else if (!IsTargetPercentageValid(out errorMsg, _rule, _targetPercentage))
                {
                    errorMessage += errorMsg;
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Updates Account list in rule
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool TryAddProrataAccount(int accountId)
        {
            try
            {
                return Rule.TryAddProrataAccount(accountId);

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
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if the specified error message is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTargetPercentageValid(out string errorMessage, AllocationRule defaultRule, SerializableDictionary<int, AccountValue> targetPercentage)
        {
            try
            {
                if (targetPercentage == null)
                {
                    errorMessage = AllocationStringConstants.PERCENTAGE_SUM_NOT_100 + AllocationStringConstants.ACCOUNTS;
                    return false;
                }

                if (defaultRule.RuleType != MatchingRuleType.Prorata && defaultRule.RuleType != MatchingRuleType.Leveling && defaultRule.RuleType != MatchingRuleType.ProrataByNAV)
                {
                    // Checking for sum of percentage
                    decimal sumPercentage = 0.0M;
                    foreach (int accountId in targetPercentage.Keys)
                    {
                        //if value is negative invalid preference
                        if (targetPercentage[accountId].Value < 0)
                        {
                            errorMessage = AllocationStringConstants.PERCENTAGE_NOT_NEGATIVE + AllocationStringConstants.ACCOUNTS;
                            return false;
                        }
                        decimal strategyPercentage = 0.0M;
                        sumPercentage += targetPercentage[accountId].Value;
                        foreach (StrategyValue strategy in targetPercentage[accountId].StrategyValueList)
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

        #region Equals

        /// <summary>
        /// Equals method to use this as key for dictionary
        /// </summary>
        /// <param name="checkListPref">CheckListWisePreference Object to be checked</param>
        /// <returns>True if equals otherwise false</returns>
        public bool Equals(CheckListWisePreference checkListPref)
        {
            try
            {
                if (checkListPref == null)
                    return false;

                if (this.ExchangeOperator != checkListPref.ExchangeOperator || this.OrderSideOperator != checkListPref.OrderSideOperator || this.AssetOperator != checkListPref.AssetOperator || this.PROperator != checkListPref.PROperator)
                    return false;
                else if ((this.ExchangeOperator != CustomOperator.All && !this.ExchangeList.OrderBy(t => t).SequenceEqual(checkListPref.ExchangeList.OrderBy(t => t)))
                    || (this.OrderSideOperator != CustomOperator.All && !this.OrderSideList.OrderBy(t => t).SequenceEqual(checkListPref.OrderSideList.OrderBy(t => t))) || (this.AssetOperator != CustomOperator.All && !this.AssetList.OrderBy(t => t).SequenceEqual(checkListPref.AssetList.OrderBy(t => t)))
                    || (this.PROperator != CustomOperator.All && !this.PRList.OrderBy(t => t).SequenceEqual(checkListPref.PRList.OrderBy(t => t)))
                    )
                    return false;

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
        /// Overridden Equals method to use this as key for dictionary
        /// </summary>
        /// <param name="obj">Object to be checked</param>
        /// <returns>True if equals otherwise false</returns>
        public override bool Equals(Object obj)
        {
            try
            {
                if (obj == null)
                    return false;

                if (GetType() != obj.GetType())
                    return false;

                CheckListWisePreference checkListObj = obj as CheckListWisePreference;
                if (checkListObj == null)
                    return false;
                else
                    return Equals(checkListObj);
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
        /// Determines whether general rule is equal for the specified check list preference.
        /// </summary>
        /// <param name="checkListPref">The check list preference.</param>
        /// <returns>
        ///   <c>true</c> if general rule is equal for the specified check list preference; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCheckListEqual(CheckListWisePreference checkListPref)
        {
            try
            {
                if (checkListPref == null)
                    return false;

                if (this.ExchangeOperator != checkListPref.ExchangeOperator || this.OrderSideOperator != checkListPref.OrderSideOperator || this.AssetOperator != checkListPref.AssetOperator || this.PROperator != checkListPref.PROperator)
                    return false;
                else if ((this.ExchangeOperator != CustomOperator.All && !this.ExchangeList.OrderBy(t => t).SequenceEqual(checkListPref.ExchangeList.OrderBy(t => t)))
                    || (this.OrderSideOperator != CustomOperator.All && !this.OrderSideList.OrderBy(t => t).SequenceEqual(checkListPref.OrderSideList.OrderBy(t => t))) || (this.AssetOperator != CustomOperator.All && !this.AssetList.OrderBy(t => t).SequenceEqual(checkListPref.AssetList.OrderBy(t => t)))
                    || (this.PROperator != CustomOperator.All && !this.PRList.OrderBy(t => t).SequenceEqual(checkListPref.PRList.OrderBy(t => t)))
                    )
                    return false;
                if (!checkListPref.Rule.Equals(this.Rule))
                    return false;
                if (!checkListPref.TargetPercentage.Equals(this.TargetPercentage))
                    return false;
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
        /// Determines whether the specified target percentage is equal.
        /// </summary>
        /// <param name="targetPercentage">The target percentage.</param>
        /// <returns>
        ///   <c>true</c> if the specified target percentage is equal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTargetPercentageEqual(SerializableDictionary<int, AccountValue> targetPercentage)
        {
            try
            {
                if (targetPercentage != null)
                {
                    if (this.TargetPercentage.Keys.Count != targetPercentage.Keys.Count)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (int key in this.TargetPercentage.Keys)
                        {
                            if (!targetPercentage.ContainsKey(key))
                            {
                                return false;
                            }
                            if (!targetPercentage[key].Equals(this.TargetPercentage[key]))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
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
                    hash = (hash * multiplier) + ExchangeOperator.GetHashCode();
                    hash = (hash * multiplier) + OrderSideOperator.GetHashCode();
                    hash = (hash * multiplier) + AssetOperator.GetHashCode();
                    hash = (hash * multiplier) + PROperator.GetHashCode();
                    hash = (hash * multiplier) + (ExchangeList == null || ExchangeList.Count == 0 ? 0 : GetHashCode<int>(ExchangeList));
                    hash = (hash * multiplier) + (OrderSideList == null || OrderSideList.Count == 0 ? 0 : GetHashCode<string>(OrderSideList));
                    hash = (hash * multiplier) + (AssetList == null || AssetList.Count == 0 ? 0 : GetHashCode<int>(AssetList));
                    hash = (hash * multiplier) + (PRList == null || PRList.Count == 0 ? 0 : GetHashCode<string>(PRList));

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
        /// Returns a hash code for this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        private int GetHashCode<T>(List<T> list)
        {
            int hash = 1;
            try
            {
                list.ForEach(x => hash = hash ^ x.GetHashCode());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return hash;
        }

        #endregion
    }
}