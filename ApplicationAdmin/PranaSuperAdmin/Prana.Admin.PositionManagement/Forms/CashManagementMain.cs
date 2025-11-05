using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class CashManagementMain : Form
    {
        public CashManagementMain()
        {
            InitializeComponent();
        }

        private void tabCashManagement_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key.Equals("Reconciliation"))
            {
                ctrlCashRecon1.InitControl();
            }
            else if (string.Equals(e.Tab.Key, "BalanceManagement"))
            {
                ctrlCashBalanceManagement1.InitControl();
            }
            else if (string.Equals(e.Tab.Key, "TransactionManagement"))
            {
                ctrlTransactionManagement1.InitControl();
            }
        }

        private void CashManagementMain_Load(object sender, EventArgs e)
        {

        }
    }
}