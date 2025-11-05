using Prana.LogManager;
using System;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    /// This class contains definition as AccountId-Value
    /// Ideally suitable for TargetPercentage, CurrentAccountState, NewTargetPercentage, NewAccountState etc
    /// </summary>
    [Serializable]
    public class AllocationState
    {

        /// <summary>
        /// Return the Id of account for this object
        /// </summary>
        /// <value>The account identifier.</value>
        public int AccountId { get; set; }

        /// <summary>
        /// Returns the Level2ID for this account
        /// </summary>
        /// <value>The Level2ID .</value>
        public int Level2ID { get; set; }

        /// <summary>
        /// Returns the OrderSideTagValue for this account
        /// </summary>
        /// <value>The Order Side Tag Value.</value>
        public string OrderSideTagValue { get; set; }

        /// <summary>
        /// Returns the cumQuantity for this account
        /// </summary>
        /// <value>The cumQuantity.</value>
        public decimal cumQuantity { get; set; }

        /// <summary>
        /// Returns the Notional for this account
        /// </summary>
        /// <value>The Notional.</value>
        public decimal Notional { get; set; }

        /// <summary>
        /// Clone the current object and returns
        /// </summary>
        /// <returns>Cloned instance of current object</returns>
        public AllocationState Clone()
        {
            try
            {
                return new AllocationState() { AccountId = this.AccountId, cumQuantity = this.cumQuantity, Level2ID = this.Level2ID, OrderSideTagValue = this.OrderSideTagValue };
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure informationLevel2ID
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
        /// Adds the value to existing cumQuantity of this object
        /// </summary>
        /// <param name="increment">Value to added</param>
        public void AddValueCumQuantity(decimal increment)
        {
            try
            {
                this.cumQuantity += increment;
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
        /// Adds the value to existing Notional of this object
        /// </summary>
        /// <param name="increment">Value to added</param>
        public void AddValueNotional(decimal increment)
        {
            try
            {
                this.Notional += increment;
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
