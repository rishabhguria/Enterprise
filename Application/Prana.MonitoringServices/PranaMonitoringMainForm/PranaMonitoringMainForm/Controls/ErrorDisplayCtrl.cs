using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;

namespace Prana.MonitoringServices
{
    public partial class ErrorDisplayCtrl : UserControl
    {
        public ErrorDisplayCtrl()
        {
            InitializeComponent();
        }

        public void DisplayErrors( ServiceClient serviceClient,List<string> messages)
        {
            lstbxMessages.DataSource = null;
            lstbxMessages.DataSource = messages;
            serverStatusCtrl1.SetUp(serviceClient);
        }

        public void SetPerformanceDetails(PerformanceParameters parameter)
        {
            serverStatusCtrl1.SetPerformanceDetails(parameter);
        }
    }
}
