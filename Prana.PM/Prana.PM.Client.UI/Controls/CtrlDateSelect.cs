using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlDateSelect : UserControl
    {
        public CtrlDateSelect()
        {
            InitializeComponent();
        }
        private CloseTradePreferences _closeTradePreferences = new CloseTradePreferences();

        public CloseTradePreferences CloseTradePreferences
        {
            get { return _closeTradePreferences; }
            set { _closeTradePreferences = value; }
        }
        private void rdCurrentDate_CheckedChanged(object sender, EventArgs e)
        {
            cmbCloseDate.Enabled = !rdCurrentDate.Checked;
            this._closeTradePreferences.IsCurrentDateClosing = rdCurrentDate.Checked;
        }

        private void rdHistoricalDate_CheckedChanged(object sender, EventArgs e)
        {
            cmbCloseDate.Enabled = rdHistoricalDate.Checked;
        }
    }
}
