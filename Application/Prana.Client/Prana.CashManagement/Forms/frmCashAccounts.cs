using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class frmCashAccounts : Form
    {
        public frmCashAccounts()
        {
            InitializeComponent();
            ctrlCashAccounts1.SetUp();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CashAccountDataManager.FillDS();
            DataSet ds = CashAccountDataManager.GetCashAccountTablesFromDB();
        }
    }
}