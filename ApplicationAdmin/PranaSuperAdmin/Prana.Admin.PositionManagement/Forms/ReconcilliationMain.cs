using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class ReconcilliationMain : Form
    {
        public ReconcilliationMain()
        {
            InitializeComponent();
        }

        private void tabReconciliation_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key.Equals("SetUp Recon"))
            {
                ctrlSetupTradeRecon1.InitControl();
            }
            else if (e.Tab.Key.Equals("Run Re-con"))
            {
                ctrlRunTradeRecon1.InitControl();
            }
        }

        private void ReconcilliationMain_Load(object sender, EventArgs e)
        {
            ctrlSetupTradeRecon1.InitControl();
            InitControl();
        }
        private void InitControl()
        {
            ctrlSetupTradeRecon1.CancelClicked += new EventHandler(ctrlSetupTradeRecon1_CancelClicked);            
        }

        void ctrlSetupTradeRecon1_CancelClicked(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}