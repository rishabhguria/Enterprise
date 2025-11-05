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
    public partial class ServerStatusCtrl : UserControl
    {
        public ServerStatusCtrl()
        {
            InitializeComponent();
        }
        public void SetUp(ServiceClient serviceClient)
        {
            lblStatus.Text = serviceClient.Status.ToString();
        }
        public void SetPerformanceDetails(PerformanceParameters parameter)
        {
            lblAvailableMemory.Text = parameter.AvailableMemory;
            lblCPUUses.Text = parameter.CPUusage;
        }
    }
}
