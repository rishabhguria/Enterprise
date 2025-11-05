using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.RiskManagement;

namespace RMStart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        RM newRMInst = new RM();
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            newRMInst = new RM();
            newRMInst.Show();
            newRMInst = null;

        }
      
    }
}