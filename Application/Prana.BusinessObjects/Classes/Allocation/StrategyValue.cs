// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-27-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="StrategyValue.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// This class contains definition as StrategyId-Value
    /// Ideally suitable for TargetPercentage
    /// </summary>
    [Serializable]
    public class StrategyValue
    {

        /// <summary>
        /// Return the Id of strategy for this object
        /// </summary>
        /// <value>The strategy identifier.</value>
        public int StrategyId { get; set; }

        /// <summary>
        /// Returns the value for this Strategy
        /// </summary>
        /// <value>The value.</value>
        public decimal Value { get; set; }

        /// <summary>
        /// Returns the Quantity for this Strategy
        /// </summary>
        /// <value>The quantity.</value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public StrategyValue()
            : this(-1, 0.0M, 0.0M)
        {
        }

        /// <summary>
        /// Constructor of the definition
        /// </summary>
        /// <param name="strategyId">The strategy identifier.</param>
        /// <param name="value">Value for this Strategy</param>
        /// <param name="quantity">Quantity for this Strategy</param>
        public StrategyValue(int strategyId, decimal value, decimal quantity)
        {
            try
            {
                this.StrategyId = strategyId;
                this.Value = value;
                this.Quantity = quantity;
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
        public StrategyValue Clone()
        {
            try
            {
                return new StrategyValue(this.StrategyId, this.Value, this.Quantity);
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

    }
}
