using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class CloseTradeMain : Form
    {
        public CloseTradeMain()
        {
            InitializeComponent();
            
        }


        private void ctrlCloseTrades1_Load(object sender, EventArgs e)
        {
            ctrlCloseTradePreferencesRunSave1.InitControl();

            this.ctrlCloseTrades1.PopulateCloseTradesInterface(false);            
        }

        private void CloseTradeMain_Load(object sender, EventArgs e)
        {
            
        }

        private void tabCloseTradeMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            
        }
    }
}