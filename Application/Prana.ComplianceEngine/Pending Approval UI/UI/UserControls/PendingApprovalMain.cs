using Prana.BusinessObjects.Compliance.Alerting;
using Prana.ComplianceEngine.Pending_Approval_UI.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.Pending_Approval_UI.UI
{
    public partial class PendingApprovalMain : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PendingApprovalMain()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Form Load and then Bind all events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendingApprovalMain_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, PreTradeApprovalInfo> pendingApprovalData = PendingApprovalManager.GetInstance().GetPendingApprovalData();
                if (pendingApprovalData != null)
                    pendingApprovalGrid1.UpdateGrid(pendingApprovalData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the pengind approval grid UI open.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        internal void UpdatePengindApprovalGridUIOpen(DataSet dataSet)
        {
            try
            {
                //TODO: Need to remove this call from here, to update PreTradeApprovalCache. (Reduce server call)
                PendingApprovalCache.GetInstance().PreTradeApprovalCache = PendingApprovalManager.GetInstance().GetPendingApprovalData();
                pendingApprovalGrid1.BindPendingApprovalData(dataSet);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the pending frozen or unfrozen.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        internal void UpdatePendingFrozenUnfrozen(DataSet dataSet, bool isFrozen)
        {
            try
            {
                pendingApprovalGrid1.BindPendingFrozeUnFroze(dataSet, isFrozen);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Bulk approve pending orders.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void btn_bulkApproveButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                pendingApprovalGrid1.PerformBulkApprove();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Bulk block pending orders.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void btn_bulkBlockButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                pendingApprovalGrid1.PerformBulkBlock();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Mouse hover message on bulk approve/block
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void message_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.bulkApproveButton, "Bulk Approve");
            ToolTip1.SetToolTip(this.bulkBlockButton, "Bulk Block");
        }

        public void ExportData(string gridName, string filePath)
        {
            try
            {
                this.pendingApprovalGrid1.ExportData(gridName, filePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
