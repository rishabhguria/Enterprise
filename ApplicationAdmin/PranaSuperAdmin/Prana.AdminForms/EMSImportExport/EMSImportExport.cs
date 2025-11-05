using System;
using System.Windows.Forms;

namespace Prana.Admin
{
    public partial class EMSImportExport : Form
    {
        public EMSImportExport()
        {
            InitializeComponent();
        }

        private void ctrlSaveImportXSLT1_Load(object sender, EventArgs e)
        {

        }

        private void EMSImportExport_Load(object sender, EventArgs e)
        {
            ctrlSaveImportXSLT.SetUpBinding();
        }
    }
}