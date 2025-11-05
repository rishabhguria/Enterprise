// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-26-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AllocationOutput.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    /// This definition holds data for each group id (Quantity)
    /// </summary>
    [Serializable]
    public class AllocationOutput
    {
        //private string _groupId = string.Empty;
        /// <summary>
        /// Public property for groupId
        /// </summary>
        /// <value>The group identifier.</value>
        public string GroupId { get; set; }

        public bool CheckSideViolated { get; set; }

        private SerializableDictionary<int, AccountValue> _accountTargetPercentageCollection = new SerializableDictionary<int, AccountValue>();

        /// <summary>
        /// Gets or sets the account target percentage collection for this group created by using different allocation methods
        /// </summary>
        /// <value>
        /// The account target percentage collection.
        /// </value>
        public SerializableDictionary<int, AccountValue> AccountTargetPercentageCollection
        {
            get { return _accountTargetPercentageCollection; }
            set { _accountTargetPercentageCollection = value; }
        }

        /// <summary>
        /// Collection of account-value
        /// </summary>
        private List<AccountValue> _accountValueCollection = new List<AccountValue>();

        /// <summary>
        /// Read-only collection of all account-value
        /// </summary>
        /// <value>The account value collection.</value>
        public IReadOnlyList<AccountValue> AccountValueCollection { get { return _accountValueCollection.AsReadOnly(); } }

        /// <summary>
        /// Default constructor
        /// <para>Update group id after calling this constructor</para><para>This is created for XML generation only for serialization purpose, ideally use overload</para>
        /// </summary>
        public AllocationOutput()
        {
            this.CheckSideViolated = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupId">GroupId of this object</param>
        public AllocationOutput(string groupId)
            : this()
        {
            this.GroupId = groupId;
        }

        /// <summary>
        /// Adds the given account-value to this object
        /// <para>Overwrite the existing accountValue collection of this object if already exists.</para><para>So if you want to just update the value first get the value for given accountId and then update through this method</para>
        /// </summary>
        /// <param name="accountValue">Account-value to updated</param>
        public void Add(AccountValue accountValue)
        {
            try
            {
                AccountValue val = _accountValueCollection.Find(i => i.AccountId == accountValue.AccountId);
                if (val != null)
                    _accountValueCollection.Remove(val);

                _accountValueCollection.Add(accountValue);
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
        /// Returns the Account-Value for given accountId
        /// </summary>
        /// <param name="accountId">AccountId for which data is required</param>
        /// <returns>AccountValue object if exists otherwise NULL</returns>
        public AccountValue GetAccountValueFor(int accountId)
        {
            try
            {
                AccountValue val = _accountValueCollection.Find(i => i.AccountId == accountId);
                if (val != null)
                    return val.Clone();
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
        /// Merges account value 
        /// </summary>
        /// <param name="accountValue"></param>
        public void Merge(AccountValue accountValue)
        {
            try
            {
                AccountValue val = _accountValueCollection.Find(i => i.AccountId == accountValue.AccountId);
                if (val != null)
                {
                    //
                    val.AddValue(accountValue.Value);
                }
                else
                {
                    _accountValueCollection.Add(accountValue);
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
            }
        }

        /// <summary>
        /// Gets the value for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        public decimal GetValueForAccount(int accountId)
        {
            decimal value = 0.0M;
            try
            {
                AccountValue val = _accountValueCollection.Find(i => i.AccountId == accountId);
                if (val != null)
                    value = val.Value;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return value;
        }
    }
}
