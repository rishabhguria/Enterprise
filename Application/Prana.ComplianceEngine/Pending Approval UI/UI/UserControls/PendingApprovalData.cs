using Prana.BusinessObjects.Compliance.Alerting;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.Pending_Approval_UI.UI.UserControls
{
    class PendingApprovalData
    {
        //data and properties for pending approval 

        public string TradeDate { get; set; }
        public string Symbols { get; set; }
        public string TradeOrigination { get; set; }
        public string Trader { get; set; }
        public string TradeDetails { get; set; }
        public string TradeNotes { get; set; }
        public string ComplianceOfficerNotes { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the trade price. This is nullable.
        /// </summary>
        /// <value>
        /// The trade price.
        /// </value>
        public double? TradePrice { get; set; }

        /// <summary>
        /// Gets or sets the name of the basket.
        /// </summary>
        /// <value>
        /// The name of the basket.
        /// </value>
        public string BasketName { get; set; }

        /// <summary>
        /// Gets or sets the quantity. This is nullable.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the name of the Broker
        /// </summary>
        /// <value>
        /// The name of the Broker
        /// </value>
        public string Broker { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>
        /// The order side.
        /// </value>
        public string OrderSide { get; set; }

        /// <summary>
        /// Gets or sets the hard alerts.
        /// </summary>
        /// <value>
        /// The hard alerts.
        /// </value>
        public List<Alert> Alerts { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PendingApprovalData()
        {
            try
            {
                this.BasketName = String.Empty;
                this.Symbols = String.Empty;
                this.Quantity = null;
                this.TradePrice = null;
                this.Broker = String.Empty;
                this.OrderSide = String.Empty;
                this.ComplianceOfficerNotes = String.Empty;
                this.Alerts = new List<Alert>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
