using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.LogManager;


namespace Prana.MonitoringServices
{
    public partial class AddNewServerCtrl : Form
    {
        public event MonitoringCache.ServerAdditionHandler ServerAdded;
        public AddNewServerCtrl()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MonitoringConnection conn = new MonitoringConnection();
                conn.IpAddress = txtbxIP.Text.Trim();
                conn.Ports = txtbxPorts.Text.Trim();
                conn.Name = txtbxName.Text.Trim();
                conn.ServiceNames = txtSeviceNames.Text.Trim();
                
                if (ServerAdded != null)
                {
                    ServerAdded(conn);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
