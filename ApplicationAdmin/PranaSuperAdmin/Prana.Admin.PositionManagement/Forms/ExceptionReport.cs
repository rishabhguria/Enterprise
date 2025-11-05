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
    public partial class ExceptionReport : Form
    {

        public ExceptionReport(DataSourceNameID DataSource)
        {
            InitializeComponent();
            ctrlExceptionReport1.InitControl(DataSource);
            //WireUpEvents();
        }

        /// <summary>
        /// TODO : Need to send using the controller
        /// </summary>
        private void WireUpEvents()
        {
            //ctrlExceptionReport1.OnOpeningManualEntryReport += new EventHandler(ctrlExceptionReport1_OnOpeningManualEntryReport);
            //ctrlExceptionReport1.OnOpeningTransactionReport += new EventHandler(ctrlExceptionReport1_OnOpeningTransactionReport);
        }

        void ctrlExceptionReport1_OnOpeningTransactionReport(object sender, EventArgs e)
        {
            //TransactionReport transactionReport = new TransactionReport();
            //transactionReport.Show();
        }

        void ctrlExceptionReport1_OnOpeningManualEntryReport(object sender, EventArgs e)
        {
            //ManualEntry manualEntry = new ManualEntry();
            //manualEntry.Show();
        }
    }
}