using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlMapSymbol : UserControl
    {
        public CtrlMapSymbol()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            this.FindForm().Close();
        }

    }
}
