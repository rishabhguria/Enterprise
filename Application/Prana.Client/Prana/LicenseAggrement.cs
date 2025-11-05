using System;
using System.Windows.Forms;

namespace Prana
{
    public partial class LicenseAggrement : Form
    {
        public LicenseAggrement()
        {
            InitializeComponent();
            this.Icon = WhiteLabelTheme.AppIcon;
            richtxtbxLicense.LoadFile(Application.StartupPath + "\\License.rtf");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}