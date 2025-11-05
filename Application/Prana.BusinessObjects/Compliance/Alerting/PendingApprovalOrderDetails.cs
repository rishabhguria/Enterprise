using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    [Serializable]
    public class PendingApprovalOrderDetails
    {
        //data variable to be included in pre trade order details
        #region Properties

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the cl order identifier.
        /// </summary>
        /// <value>
        /// The cl order identifier.
        /// </value>
        public string ClOrderID { get; set; }

        /// <summary>
        /// Gets or sets the name of the Broker
        /// </summary>
        /// <value>
        /// The name of the Broker
        /// </value>
        public string Broker { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the notional.
        /// </summary>
        /// <value>
        /// The notional.
        /// </value>
        public double Notional { get; set; }

        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>
        /// The order side.
        /// </value>
        public string OrderSide { get; set; }

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
        /// Gets or sets the underlying symbol.
        /// </summary>
        /// <value>
        /// The underlying symbol.
        /// </value>
        public string UnderlyingSymbol { get; set; }

        /// <summary>
        /// Gets or sets the name of the Trader
        /// </summary>
        /// <value>
        /// The name of the Trader
        /// </value>
        public string Trader { get; set; }

        /// <summary>
        /// Gets or sets the trade price.
        /// </summary>
        /// <value>
        /// The trade price.
        /// </value>
        public double TradePrice { get; set; }

        /// <summary>
        /// Gets or sets the Trade Notes
        /// </summary>
        /// <value>
        /// The trade notes.
        /// </value>
        public string TradeNotes { get; set; }

        /// <summary>
        /// Trade Details, it shows Side, Symbol, Qty, Price (Limit OR Market), Notional Value
        /// </summary>
        public string TradeDetails { get; set; }

        /// <summary>
        /// Trade Date
        /// </summary>
        public string TradeDate { get; set; }

        /// <summary>
        /// Trade Origination
        /// </summary>
        public TransactionSource TradeOrigination { get; set; }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PendingApprovalOrderDetails()
        {
            try
            {
                this.UserName = String.Empty;
                this.ClOrderID = String.Empty;
                this.Broker = String.Empty;
                this.AccountName = String.Empty;
                this.Notional = 0;
                this.OrderSide = String.Empty;
                this.Quantity = 0;
                this.Symbol = String.Empty;
                this.UnderlyingSymbol = String.Empty;
                this.Trader = String.Empty;
                this.TradePrice = 0;
                this.TradeNotes = String.Empty;
                this.TradeDetails = String.Empty;
                this.TradeDate = String.Empty;
                this.TradeOrigination = TransactionSource.TradingTicket;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Return Order details from Data Table
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<PendingApprovalOrderDetails> GetOrderFromDataTable(DataTable dataTable)
        {
            try
            {
                List<PendingApprovalOrderDetails> orderDetails = new List<PendingApprovalOrderDetails>();
                foreach (DataRow row in dataTable.Rows)
                {
                    PendingApprovalOrderDetails order = new PendingApprovalOrderDetails();

                    if (dataTable.Columns.Contains("UserName"))
                        order.UserName = row["UserName"].ToString();

                    if (dataTable.Columns.Contains("ClOrderID"))
                        order.ClOrderID = row["ClOrderID"].ToString();

                    if (dataTable.Columns.Contains("CounterPartyName"))
                        order.Broker = row["CounterPartyName"].ToString();

                    if (dataTable.Columns.Contains("ExpirationDate"))
                        order.ExpirationDate = row["ExpirationDate"].ToString();

                    if (dataTable.Columns.Contains("FundName"))
                        order.AccountName = row["FundName"].ToString();

                    if (dataTable.Columns.Contains("Notional"))
                        order.Notional = Convert.ToDouble(row["Notional"]);

                    if (dataTable.Columns.Contains("OrderSide"))
                        order.OrderSide = row["OrderSide"].ToString();

                    if (dataTable.Columns.Contains("Quantity"))
                        order.Quantity = Convert.ToDouble(row["Quantity"]);

                    if (dataTable.Columns.Contains("Symbol"))
                        order.Symbol = row["Symbol"].ToString();

                    if (dataTable.Columns.Contains("UnderlyingSymbol"))
                        order.UnderlyingSymbol = row["UnderlyingSymbol"].ToString();

                    if (dataTable.Columns.Contains("Trader"))
                        order.Trader = row["Trader"].ToString();

                    if (dataTable.Columns.Contains("TradePrice"))
                        order.TradePrice = Convert.ToDouble(row["TradePrice"]);

                    if (dataTable.Columns.Contains("TradeNotes"))
                        order.TradeNotes = row["TradeNotes"].ToString();

                    if (dataTable.Columns.Contains("TradeDetails"))
                        order.TradeDetails = row["TradeDetails"].ToString();

                    if (dataTable.Columns.Contains("TradeDate"))
                        order.TradeDate = row["TradeDate"].ToString();

                    if (dataTable.Columns.Contains("Broker"))
                        order.Broker = row["Broker"].ToString();

                    if (dataTable.Columns.Contains("TradeOrigination"))
                        order.TradeOrigination = (TransactionSource)Enum.Parse(typeof(TransactionSource), row["TradeOrigination"].ToString());

                    if (!orderDetails.Contains(order))
                        orderDetails.Add(order);
                }
                return orderDetails;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }
    }
}

