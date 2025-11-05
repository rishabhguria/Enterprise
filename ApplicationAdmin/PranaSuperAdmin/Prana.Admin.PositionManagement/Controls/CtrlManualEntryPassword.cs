using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.Forms;

using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlManualEntryPassword : UserControl
    {
        CashReconManualEntry _frmCashManualEntry;
        ManualEntry _frmTradeManualEntry; 

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlManualEntryPassword"/> class.
        /// </summary>
        public CtrlManualEntryPassword()
        {
            InitializeComponent();
        }

        private string _formType;

        /// <summary>
        /// Gets or sets the type of the form ("CashManualEntry","TradeManualEntry").
        /// </summary>
        /// <value>The type of the form.</value>
        public string FormType
        {
            get { return _formType; }
            set { _formType = value; }
        }


        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.Equals(txtPassword.Text.Trim().ToUpper(), "PASSWORD"))
            {
                
                DialogResult result = MessageBox.Show("Are you sure you want to save?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Your changes are saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.FindForm().Close();
                }
                else if (result == DialogResult.No)
                {
                    this.FindForm().Close();
                }
                //switch (_formType)
                //{
                //    case Constants.CASHMANUALENTRYFORM :
                //        if (_frmCashManualEntry == null)
                //        {
                //            _frmCashManualEntry = new CashTransactionDetailRecon();
                //            _frmCashManualEntry.Owner = this.FindForm();
                //            _frmCashManualEntry.ShowInTaskbar = false;

                //        }
                //        _frmCashManualEntry.ShowDialog();
                //        _frmCashManualEntry.Activate();
                        
                //        if (_frmCashManualEntry != null)
                //            _frmCashManualEntry.Disposed += new EventHandler(frmManualEntry_Disposed);
                        
                //        break;

                //    case Constants.TRADEMANUALENTRYFORM:
                //        if (_frmTradeManualEntry == null)
                //        {
                //            _frmTradeManualEntry = new ManualEntry();
                //            //_frmTradeManualEntry.Owner = this.FindForm();
                //            _frmTradeManualEntry.ShowInTaskbar = false;

                //        }
                //        _frmTradeManualEntry.ShowDialog();
                //        _frmTradeManualEntry.Activate();

                //        if (_frmTradeManualEntry != null)
                //            _frmTradeManualEntry.Disposed += new EventHandler(frmTradeManualEntry_Disposed);
                        
                    
                //        break;

                //    default:
                //        break;
                //}
                
                
            }
            else
            {
                MessageBox.Show("Please enter the correct password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Disposed event of the frmManualEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void frmManualEntry_Disposed(object sender, EventArgs e)
        //{
        //    _frmCashManualEntry = null;
        //}

        /// <summary>
        /// Handles the Disposed event of the frmTradeManualEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        ////private void frmTradeManualEntry_Disposed(object sender, EventArgs e)
        ////{
        ////    _frmTradeManualEntry = null;
        ////}


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }
        
    }
}
