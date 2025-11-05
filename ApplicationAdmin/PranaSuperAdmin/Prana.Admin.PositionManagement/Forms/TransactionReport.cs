using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class TransactionReport : Form
    {
        public TransactionReport(DataSourceNameID dataSource)
        {
            InitializeComponent();
            ctrlTransactionReport1.InitControl(dataSource);
        }

        private void ctrlTransactionReport1_Load(object sender, EventArgs e)
        {
            
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}