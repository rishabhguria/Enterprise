using Prana.InstallerUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Prana.Installer
{
    public partial class DatabasePublisher : Window
    {
        private const String CLIENT_CONNECTION_TEMPLATE = @"Database={0};Server={1};Integrated Security=SSPI;TrustServerCertificate=True;";
        private const String SM_CONNECTION_TEMPLATE = @"Database={0};Server={1};Integrated Security=SSPI;TrustServerCertificate=True;";
        private String _defaultSQLInstanceName = ConfigurationManager.AppSettings["DefaultSQLInstanceName"];

        public string SMDBConString = "";
        public string ClientConString = "";

        public DatabasePublisher()
        {
            InitializeComponent();
            serverIp.Text = _defaultSQLInstanceName;
        }

        SplashMessageScreen _splashLogger = new SplashMessageScreen();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _splashLogger.Owner = this;
        }

        private void Publish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(serverIp.Text) && !string.IsNullOrEmpty(clientDBName.Text) && !string.IsNullOrEmpty(smDBName.Text))
                {
                    FillProperties();

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please fill all required fields", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Please fill correct values", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                backupFolder.Text = dialog.SelectedPath + "\\";
            }
            else
            {
                backupFolder.Text = string.Empty;
            }
        }

        private async void BackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(serverIp.Text.Trim()) && !string.IsNullOrEmpty(clientDBName.Text.Trim()) && !string.IsNullOrEmpty(smDBName.Text.Trim()) && !string.IsNullOrEmpty(backupFolder.Text.Trim()))
                {
                    FillProperties();

                    if (Directory.Exists(backupFolder.Text.Trim()))
                    {
                        this.IsEnabled = false;
                        _splashLogger.SetSplashMessage("Backing up database", "Backing up database...  This may take some time...");
                        _splashLogger.Show();

                        string clientDBTemp = clientDBName.Text;
                        string smDBTemp = smDBName.Text;
                        string serverIpTemp = serverIp.Text;
                        string backupFolderTemp = backupFolder.Text;

                        string displayDBVersion = DatabaseHelper.GetRealDBVersion(String.Format(CLIENT_CONNECTION_TEMPLATE, clientDBTemp, serverIpTemp));

                        if (!string.IsNullOrWhiteSpace(displayDBVersion))
                        {
                            FileCompressor compress = new FileCompressor();
                            string tempClientDBName = clientDBName.Text + "_" + displayDBVersion;
                            string tempSMDBName = smDBName.Text + "_" + displayDBVersion;
                            String sourceFolder = backupFolder.Text;
                            String outputZipFile = backupFolder.Text + tempClientDBName + "_" + tempSMDBName + ".zip";
                            List<String> list = new List<String>();

                            if (!File.Exists(backupFolder.Text + tempClientDBName + ".bak") && !File.Exists(backupFolder.Text + tempSMDBName + ".bak"))
                            {
                                await System.Threading.Tasks.Task.Run(() => { DatabaseHelper.BackupDatabase(String.Format(CLIENT_CONNECTION_TEMPLATE, clientDBTemp, serverIpTemp), backupFolderTemp + tempClientDBName + ".bak"); });
                                await System.Threading.Tasks.Task.Run(() => { DatabaseHelper.BackupDatabase(String.Format(SM_CONNECTION_TEMPLATE, smDBTemp, serverIpTemp), backupFolderTemp + tempSMDBName + ".bak", true); });
                                list.Add(backupFolder.Text + tempClientDBName + ".bak");
                                list.Add(backupFolder.Text + tempSMDBName + ".bak");
                                compress.CreateZipFile(list, outputZipFile);

                                foreach (string databaseBackup in list)
                                    if (File.Exists(databaseBackup))
                                        File.Delete(databaseBackup);
                            }
                            else
                            {
                                LoggingHelper.GetInstance().LoggerWrite("Backup files already exists on selected path", true, System.Windows.Forms.MessageBoxIcon.Hand);
                            }
                        }

                        _splashLogger.Hide();
                        this.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select valid path", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all required fields", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                _splashLogger.Hide();
                this.IsEnabled = true;
                LoggingHelper.GetInstance().LoggerWrite("Database backup failed", true, System.Windows.Forms.MessageBoxIcon.Error);
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void backupFolder_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(backupFolder.Text.Trim()))
            {
                if (backupFolder.Text.LastIndexOf('\\') != backupFolder.Text.Length - 1)
                {
                    backupFolder.Text += "\\";
                }
            }
        }

        private void FillProperties()
        {
            ClientConString = String.Format(CLIENT_CONNECTION_TEMPLATE, clientDBName.Text, serverIp.Text);
            SMDBConString = String.Format(CLIENT_CONNECTION_TEMPLATE, smDBName.Text, serverIp.Text);
        }
    }
}