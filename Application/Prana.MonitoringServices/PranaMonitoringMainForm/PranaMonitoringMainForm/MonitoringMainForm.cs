using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.SocketCommunication;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.FIX;
using Prana.Utilities.MiscUtilities;
using System.Collections;
using Prana.BusinessObjects.AppConstants;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Prana.LogManager;

namespace Prana.MonitoringServices
{
    public partial class MonitoringMainForm : Form
    {
        delegate void SetTextCallback2(ServiceEndPoint serviceEndPoint);
        public MonitoringMainForm()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void toolStripButtonStartProcess_Click(object sender, EventArgs e)
        {
            try
            {
                Process pPro = Process.Start("notepad.exe");
                pPro.WaitForExit();
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

        private void toolStripButtonPerformance_Click(object sender, EventArgs e)
        {
            try
            {
                PerformanceHelper p = new PerformanceHelper();
                p.SetValues();
                string value = p.GetAvailableRAM();
                string value2 = p.GetCurrentCpuUsage();
                MessageBox.Show(value +":"+value2);
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

        private void MonitoringMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                manageConnectionsCtrl1.Initilise();
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