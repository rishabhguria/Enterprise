using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;

namespace Prana.PMLauncher
{
    public partial class ErrorProviderCheck : Form
    {
        AddressDetails addressDetails = new AddressDetails();
        public ErrorProviderCheck()
        {
            InitializeComponent();
            addressDetails.Address2 = "NY";
            addressDetailsBindingSource.DataSource = addressDetails;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.ValidateChildren();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}