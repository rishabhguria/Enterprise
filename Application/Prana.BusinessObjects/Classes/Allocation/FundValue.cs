// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-27-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AccountValue.cs" company="Nirvana">
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
    /// This class contains definition as AccountId-Value
    /// Ideally suitable for TargetPercentage, CurrentAccountState, NewTargetPercentage, NewAccountState etc
    /// </summary>
    [Serializable]
    public class AccountValue
    {

        /// <summary>
        /// Return the Id of account for this object
        /// </summary>
        /// <value>The account identifier.</value>
        public int AccountId { get; set; }

        /// <summary>
        /// Returns the value for this account
        /// </summary>
        /// <value>The value.</value>
        public decimal Value { get; set; }

        /// <summary>
        /// List of strategy Id and values.
        /// </summary>
        /// <value>The strategy value list.</value>
        public List<StrategyValue> StrategyValueList { get; set; }

        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public AccountValue()
            : this(-1, 0.0M, null)
        {
        }

        /// <summary>
        /// Constructor of the definition
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="value">Value for this account</param>
        /// <param name="strategyValue">The strategy value.</param>
        public AccountValue(int accountId, decimal value, List<StrategyValue> strategyValue)
        {
            try
            {
                this.AccountId = accountId;
                this.Value = value;
                this.StrategyValueList = strategyValue;
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
        /// Constructor of the definition
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="value">Value for this account</param>
        public AccountValue(int accountId, decimal value)
        {
            try
            {
                this.AccountId = accountId;
                this.Value = value;
                this.StrategyValueList = new List<StrategyValue>();
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
        /// Clone the current object and returns
        /// </summary>
        /// <returns>Cloned instance of current object</returns>
        public AccountValue Clone()
        {
            try
            {
                return new AccountValue(this.AccountId, this.Value, this.StrategyValueList);
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
        /// Adds the value to existing value of this object
        /// </summary>
        /// <param name="increment">Value to added</param>
        public void AddValue(decimal increment)
        {
            try
            {
                this.Value += increment;
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
        /// Check if Account value are equal, PRANA-12383
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AccountValue item = obj as AccountValue;
            if (item == null)
                return false;
            bool isStrategyValueListEqual = IsStrategyValueListEqual(this.StrategyValueList, item.StrategyValueList);
            return (this.AccountId == item.AccountId
                && this.Value == item.Value
                && isStrategyValueListEqual);
        }

        /// <summary>
        /// Check if StrategyValue are equal, PRANA-12383
        /// </summary>
        /// <param name="strategyValueListExisting"></param>
        /// <param name="strategyValueListNew"></param>
        /// <returns></returns>
        private bool IsStrategyValueListEqual(List<StrategyValue> strategyValueListExisting, List<StrategyValue> strategyValueListNew)
        {
            bool isEqual = true;
            try
            {
                if (strategyValueListExisting.Count != strategyValueListNew.Count)
                    return false;
                for (int i = 0; i < strategyValueListNew.Count; i++)
                {
                    if (strategyValueListNew[i].StrategyId != strategyValueListExisting[i].StrategyId || strategyValueListNew[i].Value != strategyValueListExisting[i].Value)
                    {
                        isEqual = false;
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
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}
