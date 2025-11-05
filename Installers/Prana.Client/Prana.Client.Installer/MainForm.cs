using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Prana.Client.Installer.Utilities;

namespace Prana.Client.Installer
{
    public partial class MainForm : Form
    {        
        //private System.Windows.Forms.Timer timer1;
        private BackgroundWorker bgworker = new BackgroundWorker();
        private Splash frmSplash = null;
        private long _whitelable = 0;

        public long Whitelable
        {
            set { _whitelable = value; }
        }

        private string _paranaInstallerStartupPathpath = Application.StartupPath;

        public MainForm()
        {
            InitializeComponent();

            //WhiteLabelHelper.Whitelable = 2; //Sepcify 1 for Concept, 2 for Cuttone remove this line for default branding
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

            label1.Text = "Welcome to " + WhiteLabelHelper.WhitelableProductName + " " + WhiteLabelHelper.Version + ". The setup will now search for the installed components and guide you through the installation process.";

            Thread thSplashScreen = null;
            thSplashScreen = new Thread(new ThreadStart(DoSplash));
            thSplashScreen.Start();
            Thread.Sleep(1000);
            
            //Background Worker            
            InitializeBGWorker();
            
            Thread.Sleep(1000);

            if (null != thSplashScreen)
            {
                thSplashScreen.Abort();
                thSplashScreen = null;
            }
        }

        private void RunInstallationThread()
        {
            if (!eSignalInstallerHelper.IsESignalInstalled())
            {
                DialogResult dlgresult = MessageBox.Show("The Current Version of Prana requires E-Signal to be installed! Do you want setup to Install E-Signal to your system.", "Prana Message", MessageBoxButtons.YesNo);
                if (dlgresult == DialogResult.Yes)
                {
                    try
                    {
                        this.Visible = false;
                        this.ShowInTaskbar = false;

                        StartESignalInstaller();

                        //run worker async of the background process
                        bgworker.RunWorkerAsync();
                    }
                    catch (Exception exp)
                    {
                        //throw new Exception("An Error Occured while installation. Unable to install ESignal! Rolling back changes!");
                        MessageBox.Show(exp.StackTrace);
                    }
                }
                else
                {
                    this.Visible = false;
                    this.ShowInTaskbar = false;
                    StartPranaMSIInstaller();
                }
            }
            else
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                StartPranaMSIInstaller();
            }
        }

        private static void StartESignalInstaller()
        {
            string eSignalInstallerPath = Application.StartupPath + "\\" + Constants.CONST_Components + Constants.CONST_eSignalInstaller;

            Process m_process = new Process();
            m_process.StartInfo.FileName = eSignalInstallerPath;
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            m_process.Start();
        }

        private void StartPranaMSIInstaller()
        {
            string PranaRestartSetupPath = Application.StartupPath + "\\" + Constants.CONST_Components + Constants.CONST_PranaRestartSetup;

            Process m_process = new Process();
            m_process.StartInfo.FileName = PranaRestartSetupPath;
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            m_process.Start();
            this.Close();
        }

        private void DoSplash()
        {
            frmSplash = new Splash();
            frmSplash.ShowDialog();
        }

        private void InitializeBGWorker()
        {
            bgworker.DoWork += new DoWorkEventHandler(bgworker_DoWork);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgworker_RunWorkerCompleted);
        }

        private void CheckForESignalInstaller()
        {
            while (CheckProcesses())
            {
            }
            return;
        }

        private bool CheckProcesses()
        {
            Process[] proc = Process.GetProcessesByName(Constants.CONST_eSignalInstaller.Replace(".exe",""));
            if (proc.Length > 0)
                return true;
            else
                return false;
        }

        private void bgworker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForESignalInstaller();
        }

        private void bgworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!eSignalInstallerHelper.IsESignalInstalled())
                {
                    MessageBox.Show("Esignal Installation has been cancelled by the user! The setup will continue without installing E-Signal");
                }
                //// Proceed the installation of the msi
                StartPranaMSIInstaller();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {

                RegistryHelper.WriteEntriesInRegistryForStartup(_paranaInstallerStartupPathpath);
                RunInstallationThread();
            } catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}