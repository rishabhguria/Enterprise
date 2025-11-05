using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;

namespace Prana.Blotter.Controls
{
    public partial class SubOrderBlotterGrid : Blotter.WorkingSubBlotterGrid
    {
        public SubOrderBlotterGrid()
        {
            InitializeComponent();
        }
        OrderBindingList _blotterOdrColl = new OrderBindingList();

        /// <summary>
        /// Initializes the contol.
        /// </summary>
        /// <param name="blotterOdrColl">The blotter odr coll.</param>
        /// <param name="key">The key.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="blotterColumnPrefs">The blotter column prefs.</param>
        /// <param name="blotterPreferenceData">The blotter color prefs.</param>
        public override void InitContol(OrderBindingList blotterOdrColl, string key, CompanyUser loginUser, BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                _blotterOdrColl = blotterOdrColl;
                base.InitContol(_blotterOdrColl, key, loginUser, blotterPreferenceData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the dgBlotter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs" /> instance containing the event data.</param>
        protected override void dgBlotter_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                base.dgBlotter_InitializeLayout(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the active order data.
        /// </summary>
        /// <param name="order">The order.</param>
        internal void LoadActiveOrderData(OrderSingle order)
        {
            try
            {
                ClearSubOrdersGrid();
                if (order != null && order.OrderCollection != null && order.OrderCollection.Count > 0)
                {
                    foreach (OrderSingle orderSingle in order.OrderCollection)
                    {
                        if (!_blotterOdrColl.Contains(orderSingle))
                            _blotterOdrColl.Add(orderSingle);
                    }
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
        /// Clears the sub orders grid.
        /// </summary>
        internal void ClearSubOrdersGrid()
        {
            try
            {
                _blotterOdrColl.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the manual sub orders grid.
        /// </summary>
        internal void RemoveOrderFromGrid(OrderSingle order)
        {
            try
            {
                _blotterOdrColl.Remove(order);
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
