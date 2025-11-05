using System;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Controls
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
