using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class CashReconManualEntry : Form
    {
        private ManualEntryPassword _manualEntryPassword;

        public CashReconManualEntry()
        {
            InitializeComponent();
            ctrlCashReconManualEntry1.InitControl();
        }

        void ctrlCashReconManualEntry1_OpenManualEntryPassword(object sender, EventArgs e)
        {
            if (_manualEntryPassword == null)
            {
                _manualEntryPassword = new ManualEntryPassword();
            }
            if (_manualEntryPassword != null)
                _manualEntryPassword.Disposed += new EventHandler(manualEntryPassword_Disposed);
                        

            _manualEntryPassword.Show();
        }

        /// <summary>
        /// Handles the Disposed event of the manualEntryPassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void manualEntryPassword_Disposed(object sender, EventArgs e)
        {
            this.FindForm().Close();
            _manualEntryPassword = null;

        }

        private void ctrlCashReconManualEntry1_Load(object sender, EventArgs e)
        {
           // ctrlCashReconManualEntry1.OpenManualEntryPassword += new EventHandler(ctrlCashReconManualEntry1_OpenManualEntryPassword);
        }

        
    }
}