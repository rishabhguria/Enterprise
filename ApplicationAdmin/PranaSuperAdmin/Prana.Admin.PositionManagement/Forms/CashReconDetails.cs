using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class CashReconDetails : Form
    {
        private CashReconManualEntry _cashTransactionDetailRecon;

        public CashReconDetails(DataSourceNameID dataSource)
        {
            InitializeComponent();
            ctrlCashReconDetails1.InitControl(dataSource);
        }

        private void CashReconDetails_Load(object sender, EventArgs e)
        {
            //ctrlCashReconDetails1.OpenManualEntry += new EventHandler(ctrlCashReconDetails1_OpenManualEntry);
        }

        void ctrlCashReconDetails1_OpenManualEntry(object sender, EventArgs e)
        {
            //if (_cashTransactionDetailRecon ==null)
            //{
            //    _cashTransactionDetailRecon = new CashTransactionDetailRecon();            
            //}
     
            //_cashTransactionDetailRecon.Show();
        }

    }
}