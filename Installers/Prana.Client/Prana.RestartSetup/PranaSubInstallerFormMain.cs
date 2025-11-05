using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Prana.Client.Installer.Utilities;

namespace Prana.RestartSetup
{
    public partial class PranaSubInstallerFormMain : Form
    {
        public PranaSubInstallerFormMain()
        {
            InitializeComponent();
            //WhiteLabelHelper.Whitelable = 1; //Sepcify 1 for Concept, 2 for Cuttone. Comment this line for default branding
            pictureBox1.Image = WhiteLabelHelper.InstallerHeader;

            if (WhiteLabelHelper.WhitelableProductName.Equals("Nirvana"))
            {
                this.Text = "Nirvana "
                               + WhiteLabelHelper.Version
                               + ".";
            }
            else
            {
                this.Text = WhiteLabelHelper.WhitelableProductName
                               + " - Powered by Nirvana "
                               + WhiteLabelHelper.Version
                               + ".";
            }

            this.Icon = WhiteLabelHelper.InstallerIcon;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            StartPranaMSIInstaller();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RegistryHelper.RemoveEntriesInRegistryForStartup();
            this.Close();
        }

        private void StartPranaMSIInstaller()
        {
            string PranaInstallerStartupPath = Application.StartupPath + "\\" + Constants.CONST_PranaMSIInstaller;

            Process PranaInstallerProcess = new Process();
            PranaInstallerProcess.StartInfo.FileName = PranaInstallerStartupPath;
            PranaInstallerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            PranaInstallerProcess.Start();
            this.Close();
        }
    }
}