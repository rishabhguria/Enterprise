using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    public class OrderDetails
    {
        #region Properties

        /// <summary>
        /// Gets or sets the counter party identifier.
        /// </summary>
        /// <value>
        /// The counter party identifier.
        /// </value>
        public int CounterPartyID { get; set; }

        /// <summary>
        /// Gets or sets the order side tag value.
        /// </summary>
        /// <value>
        /// The order side tag value.
        /// </value>
        public string OrderSideTagValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public OrderFields.PranaMsgTypes OrderType { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the trade time.
        /// </summary>
        /// <value>
        /// The trade time.
        /// </value>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// Gets or sets the transaction time.
        /// </summary>
        /// <value>
        /// The transaction time.
        /// </value>
        public string TransactionTime { get; set; }

        /// <summary>
        /// Gets or sets the user action.
        /// </summary>
        /// <value>
        /// The user action.
        /// </value>
        public UserAction UserAction { get; set; }

        /// <summary>
        /// Gets or sets the type of the user action.
        /// </summary>
        /// <value>
        /// The type of the user action.
        /// </value>
        public string UserActionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the master fund.
        /// </summary>
        /// <value>
        /// The type of the master fund.
        /// </value>
        public string MasterFund { get; set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetails"/> class.
        /// </summary>
        public OrderDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetails"/> class.
        /// </summary>
        /// <param name="os">The os.</param>
        /// <param name="tradingTicketType">Type of the trading ticket.</param>
        /// <param name="userAction">The user action.</param>
        /// <param name="actionType">Type of the action.</param>
        public OrderDetails(OrderSingle os, UserAction userAction, string actionType)
        {
            try
            {
                this.OrderType = (OrderFields.PranaMsgTypes)os.PranaMsgType;
                this.TradeTime = DateTime.Now;
                this.Symbol = os.Symbol;
                this.OrderSideTagValue = os.OrderSideTagValue;
                this.CounterPartyID = os.CounterPartyID;
                this.UserAction = userAction;
                this.UserActionType = actionType;
                this.TransactionTime = os.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                this.MasterFund = os.MasterFund;
                switch ((OrderFields.PranaMsgTypes)os.PranaMsgType)
                {
                    case OrderFields.PranaMsgTypes.ORDStaged:
                        this.Quantity = os.CumQtyForSubOrder;
                        break;

                    case OrderFields.PranaMsgTypes.ORDNewSub:
                    case OrderFields.PranaMsgTypes.ORDNewSubChild:
                    case OrderFields.PranaMsgTypes.ORDManual:
                    case OrderFields.PranaMsgTypes.ORDManualSub:
                        this.Quantity = os.Quantity;
                        break;
                }
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
        /// Determines whether the specified od is duplicate.
        /// </summary>
        /// <param name="od">The od.</param>
        /// <returns>
        ///   <c>true</c> if the specified od is duplicate; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckForDuplicateOrder(OrderDetails od, int timeInterval = 0)
        {
            bool isDuplicate = false;
            try
            {
                if (timeInterval != 0)
                {
                    isDuplicate = this.Symbol == od.Symbol && this.Quantity == od.Quantity && this.OrderSideTagValue == od.OrderSideTagValue && this.CounterPartyID == od.CounterPartyID && this.MasterFund == od.MasterFund;
                    int diffInSeconds = Convert.ToInt32((od.TradeTime - this.TradeTime).TotalSeconds);
                    if (diffInSeconds > timeInterval)
                        isDuplicate = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isDuplicate;
        }

        /// <summary>
        /// Removes for compliance reject.
        /// </summary>
        /// <param name="od">The od.</param>
        /// <returns></returns>
        public bool GetComplianceRejectOrder(OrderDetails od)
        {
            bool isSameOrder = false;
            try
            {
                isSameOrder = CheckForDuplicateOrder(od) && this.TransactionTime == od.TransactionTime;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSameOrder;
        }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueKey()
        {
            string uniqueKey = string.Empty;
            try
            {
                uniqueKey = this.TradeTime.ToString("mmddyyyy_hh:mm:ss") + this.Symbol + this.Quantity.ToString() + this.OrderSideTagValue + this.CounterPartyID.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return uniqueKey;
        }

        #endregion Methods
    }
}
