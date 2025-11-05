using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Interfaces;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class PM : Form, IPositionManagement
    {
        private PMMain frmPMMain;
        private CashManagementMain frmCashManagement;
        private ReconcilliationMain frmReconcilliationMain;
        private CloseTradeMain frmCloseTradeMain;

        public PM()
        {
            InitializeComponent();
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
          
            switch (e.Tool.Key.ToUpper())
            {
                    //Sugandh - have used to upper in the above condition so, please type the key in upper case. 
                case "ADMIN":

                    if (frmPMMain == null)
                    {
                        frmPMMain = new PMMain();
                        frmPMMain.Owner = this;
                        frmPMMain.ShowInTaskbar = false;
                    }
                    frmPMMain.Show();
                    frmPMMain.Activate();
                    frmPMMain.Disposed += new EventHandler(frmPMMain_Disposed);
                    break;
                case "RECONCILIATION":

                    if (frmReconcilliationMain == null)
                    {
                        frmReconcilliationMain = new ReconcilliationMain();
                        frmReconcilliationMain.Owner = this;
                        frmReconcilliationMain.ShowInTaskbar = false;
                    }
                    frmReconcilliationMain.Show();
                    frmReconcilliationMain.Activate();
                    frmReconcilliationMain.Disposed += new EventHandler(frmReconcilliationMain_Disposed);
                    break;
                case "CASHMANAGEMENT":

                    if (frmCashManagement == null)
                    {
                        frmCashManagement = new CashManagementMain();
                        frmCashManagement.Owner = this;
                        frmCashManagement.ShowInTaskbar = false;
                    }
                    frmCashManagement.Show();
                    frmCashManagement.Activate();
                    frmCashManagement.Disposed += new EventHandler(frmCashManagement_Disposed);
                    break;        
                    
                case "CLOSETRADES":

                    if (frmCloseTradeMain == null)
                    {
                        frmCloseTradeMain = new CloseTradeMain();
                        frmCloseTradeMain.Owner = this;
                        frmCloseTradeMain.ShowInTaskbar = false;
                    }
                    frmCloseTradeMain.Show();
                    frmCloseTradeMain.Activate();
                    frmCloseTradeMain.Disposed += new EventHandler(frmCloseTradeMain_Disposed);
                    break;

                case "EXIT":
                    Application.Exit();
                    break;
                default:
                    MessageBox.Show("Under Construction!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        
        /// <summary>
        /// Handles the Disposed event of the frmPMMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmPMMain_Disposed(object sender, EventArgs e)
        {
            frmPMMain = null;
        }

        /// <summary>
        /// Handles the Disposed event of the frmCashManagement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmCashManagement_Disposed(object sender, EventArgs e)
        {
            frmCashManagement = null;
        }

        /// <summary>
        /// Handles the Disposed event of the frmCloseTradeMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmCloseTradeMain_Disposed(object sender, EventArgs e)
        {
            frmCloseTradeMain = null;
        }

        /// <summary>
        /// Handles the Disposed event of the frmReconcilliationMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmReconcilliationMain_Disposed(object sender, EventArgs e)
        {
            frmReconcilliationMain = null;
        }

        private void tbcConsolidation_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

            //switch (e.Tab.Key)
            //{
            //    case "Consolidated" :
            //        break;
            //    case "Customize" :
            //        break;
            //    case "Performance" :
            //        break;

            //}
            if (e.Tab.Key.Equals("Consolidated"))
            {
                ctrlMainConsolidationView1.InitControl(false);
            }
            if (e.Tab.Key.Equals("Customize"))
            {
                ctrlMainConsolidationView4.InitControl(true);
            }

        }



        #region IPositionManagement Members

        public event EventHandler PositionManagementClosed;

        public Form Reference()
        {
            return this;
        }

        #endregion

        private void PM_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PositionManagementClosed != null)
            {
                PositionManagementClosed(this, EventArgs.Empty);
            } 
        }
    }
}
