using Prana.InstallerUtilities;
using Prana.InstallerUtilities.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Prana.Installer
{
    public partial class MainWindow : Window
    {
        SplashMessageScreen _splashLogger;
        bool _isConsoleMode;

        public MainWindow()
        {
            InitializeComponent();
            Thread.Sleep(1000);
            _splashLogger = new SplashMessageScreen();
            DatabaseHelper.Log += DatabaseHelper_Log;
            BackupHelper.Log += DatabaseHelper_Log;
            LoggingHelper.GetInstance().LoggerWrite("Prana installer started");
        }

        private async void Next_click(object sender, RoutedEventArgs e)
        {
            EnableDisableMainWindow(false);

            if (NewRegistration.IsChecked == true)
            {
                #region New Registration
                try
                {
                    LoggingHelper.GetInstance().LoggerWrite("Registering a new release");
                    NewReleaseForm newInstall = new NewReleaseForm() { Owner = this };
                    if (newInstall.ShowDialog() == false)
                    {
                        EnableDisableMainWindow(true);
                        return;
                    }

                    #region Installing Release
                    MSIHelper.RenewMSI("installer.MSI", newInstall.ClientName);
                    System.Diagnostics.Process install = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/c msiexec /i installer.msi /l*v log.txt ";
                    install.StartInfo = startInfo;
                    install.Start();
                    install.WaitForExit();
                    #endregion

                    if (install.ExitCode != 0)
                    {
                        ShowExitCodeMessage(install.ExitCode, string.Empty);
                    }
                    else
                    {
                        if (!RegistryHelper.IsBrowserEmulationSet("Prana.exe"))
                        {
                            if (RegistryHelper.SetBrowserEmulationVersion("Prana.exe"))
                            {
                                LoggingHelper.GetInstance().LoggerWrite("BrowserEmulationVersion has been set for compliance.");
                            }
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("BrowserEmulationVersion is already set for compliance.");
                        }
                        LoggingHelper.GetInstance().LoggerWrite("Release registration completed. Please upgrade the same for complete installation", true, System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    UpdateExistingInstallation.IsChecked = true;
                }
                catch (Exception ex)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    EnableDisableMainWindow(true);
                }
                #endregion
            }
            else if (UpdateExistingInstallation.IsChecked == true)
            {
                #region Upgrade Existing Installation
                await UpdateClient();
                #endregion
            }
            else if (OnlyUpdateDatabase.IsChecked == true)
            {
                #region Only Update Database
                try
                {
                    DatabasePublisher updateDatabase = new DatabasePublisher() { Owner = this };
                    if (updateDatabase.ShowDialog() == false)
                    {
                        EnableDisableMainWindow(true);
                        return;
                    }

                    #region Database Publish
                    string displayDBVersion = DatabaseHelper.GetRealDBVersion(updateDatabase.ClientConString);

                    if (!string.IsNullOrWhiteSpace(displayDBVersion))
                    {
                        _splashLogger.Show();
                        await System.Threading.Tasks.Task.Run(() => { PublishDatabases(updateDatabase.ClientConString, updateDatabase.SMDBConString, displayDBVersion); });
                        _splashLogger.Hide();

                        #region Client Specific Scripts Publish
                        CustomizeControl customizeControl = new CustomizeControl() { Owner = this };
                        if (customizeControl.ShowDialog() == true && !string.IsNullOrWhiteSpace(customizeControl.ClientDacpacFileName))
                        {
                            _splashLogger.Show();
                            await System.Threading.Tasks.Task.Run(() => { PublishClientSpecificScripts(updateDatabase.ClientConString, updateDatabase.SMDBConString, displayDBVersion, customizeControl.ClientDacpacFileName); });
                            _splashLogger.Hide();
                        }
                        #endregion
                    }
                    else
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Unable to find database version. Database publish failed.", true, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    EnableDisableMainWindow(true);
                }
                #endregion
            }
        }

        private async Task UpdateClient()
        {
            try
            {
                string consoleModeClientSpecificScripts_ClientName = string.Empty;
                if (_isConsoleMode)
                {
                    string[] args = Environment.GetCommandLineArgs();

                    if (args.Length > 1)
                    {
                        string clientName = args[1];
                        consoleModeClientSpecificScripts_ClientName = args[2];

                        for (int i = 0; i < ClientList.Items.Count; i++)
                        {
                            InstallationDetails item = (InstallationDetails)ClientList.Items[i];
                            if (item.Name.ToString() == clientName)
                            {
                                ClientList.SelectedItem = item;
                                break;
                            }
                        }
                    }

                    LoggingHelper.GetInstance().LoggerWrite("Release backup before upgrade.");
                    BackupReleaseWithDatabase(true);
                }

                var selectedFile = (dynamic)ClientList.SelectedItem;

                if (selectedFile != null)
                {
                    string releasePath = selectedFile.Path + selectedFile.Name.Substring(InstallerConstants.ProductName.Length);
                    string[] conStrings = ReleaseHelper.GetConnectionStringForRelease(releasePath);
                    if (conStrings == null)
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Unable to fetch connection string from existing installation");
                        EnableDisableMainWindow(true);
                        return;
                    }

                    LoggingHelper.GetInstance().LoggerWrite("Upgrading an existing installation");
                    LoggingHelper.GetInstance().LoggerWrite("Release Path: " + releasePath);

                    #region Backup For Auto Restore
                    _splashLogger.SetSplashMessage("Backing up client specific files for auto restore", string.Empty);
                    _splashLogger.Show();

                    Boolean isSuccessfullyCreatedBackupForRestore = await System.Threading.Tasks.Task.Run(() => { return BackupHelper.BackupRelease(releasePath, false, true); });
                    LoggingHelper.GetInstance().LoggerWrite("Backup completed of client specific files for auto restore");
                    _splashLogger.Hide();
                    #endregion

                    if (isSuccessfullyCreatedBackupForRestore)
                    {
                        #region Upgrading Release
                        MSIHelper.CreateUpgradeMSI("installer.msi", selectedFile.Name, selectedFile.UpdateCode);
                        System.Diagnostics.Process install = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        string path = selectedFile.Path;
                        string arguments1 = string.Empty;
                        string arguments2 = string.Empty;

                        if (_isConsoleMode)
                        {
                            arguments1 = string.Format(@"/c msiexec /I installer.msi /quiet /l*v log.txt INSTALLFOLDER=""{0}""", path);
                            arguments2 = "/c msiexec /i installer.msi /quiet /l*v log.txt ";
                        }
                        else
                        {
                            arguments1 = string.Format(@"/c msiexec /I installer.msi /l*v log.txt INSTALLFOLDER=""{0}""", path);
                            arguments2 = "/c msiexec /i installer.msi /l*v log.txt ";
                        }

                        if (!string.IsNullOrEmpty(path))
                            startInfo.Arguments = arguments1;
                        else
                            startInfo.Arguments = arguments2;

                        install.StartInfo = startInfo;
                        install.Start();
                        install.WaitForExit();
                        #endregion

                        #region Restore From Auto Backup
                        _splashLogger.SetSplashMessage("Restoring client specific files from auto backup", string.Empty);
                        _splashLogger.Show();
                        Boolean resultRestore = await System.Threading.Tasks.Task.Run(() => { return BackupHelper.RestoreFilesFromBackup(releasePath); });
                        if (!resultRestore)
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Unable to restore client specific files from auto backup");
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Restore completed of client specific files from auto backup");

                            BackupHelper.DeleteBackupForRestoreDirectory(releasePath);
                        }
                        _splashLogger.Hide();
                        #endregion

                        if (install.ExitCode == 0 || install.ExitCode == 3010)
                        {
                            #region Database Publish
                            string displayDBVersion = DatabaseHelper.GetRealDBVersion(conStrings[0]);

                            if (!string.IsNullOrWhiteSpace(displayDBVersion))
                            {
                                _splashLogger.Show();
                                await System.Threading.Tasks.Task.Run(() => { PublishDatabases(conStrings[0], conStrings[1], displayDBVersion); });
                                _splashLogger.Hide();

                                #region Client Specific Scripts Publish
                                CustomizeControl customizeControl = new CustomizeControl() { Owner = this, IsConsoleMode = _isConsoleMode, ConsoleModeClientName = consoleModeClientSpecificScripts_ClientName };
                                if (customizeControl.ShowDialog() == true && !string.IsNullOrWhiteSpace(customizeControl.ClientDacpacFileName))
                                {
                                    _splashLogger.Show();
                                    await System.Threading.Tasks.Task.Run(() => { PublishClientSpecificScripts(conStrings[0], conStrings[1], displayDBVersion, customizeControl.ClientDacpacFileName); });
                                    _splashLogger.Hide();
                                }
                                #endregion
                            }
                            else
                            {
                                LoggingHelper.GetInstance().LoggerWrite("Unable to find database version. Database publish failed.", true, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                            #endregion

                            if (install.ExitCode != 0)
                            {
                                ShowExitCodeMessage(install.ExitCode, string.Empty);
                            }
                            else
                            {
                                if (!RegistryHelper.IsBrowserEmulationSet("Prana.exe"))
                                {
                                    if (RegistryHelper.SetBrowserEmulationVersion("Prana.exe"))
                                    {
                                        LoggingHelper.GetInstance().LoggerWrite("BrowserEmulationVersion has been set for compliance.");
                                    }
                                }
                                else
                                {
                                    LoggingHelper.GetInstance().LoggerWrite("BrowserEmulationVersion is already set for compliance.");
                                }
                                LoggingHelper.GetInstance().LoggerWrite("Release updation completed", true, System.Windows.Forms.MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            ShowExitCodeMessage(install.ExitCode, releasePath);
                        }
                    }
                    else
                    {
                        BackupHelper.DeleteBackupForRestoreDirectory(releasePath);
                    }

                    if (_isConsoleMode)
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Release backup after upgrade.");
                        BackupReleaseWithDatabase(true);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a client first", "Prana Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
            finally
            {
                if (_isConsoleMode)
                {
                    Environment.Exit(0);
                }
                EnableDisableMainWindow(true);
            }
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoggingHelper.GetInstance().LoggerWrite("Prana installer closed");
                this.Close();
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private async void UpdateExistingInstallation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableDisableMainWindow(false);
                _splashLogger.SetSplashMessage("Getting list of installed clients", "Getting list of installed clients... This may take some time...");
                _splashLogger.Show();
                await System.Threading.Tasks.Task.Run(() => { ListClients(RegistryHelper.Instance.GetInstalledClients()); });
                _splashLogger.Hide();
                LoggingHelper.GetInstance().LoggerWrite("Received list of installed clients");

                if (_isConsoleMode)
                {
                    await UpdateClient();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
            finally
            {
                EnableDisableMainWindow(true);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    _isConsoleMode = true;

                    LoggingHelper.GetInstance().IsConsoleMode = true;
                }

                if (!DatabaseHelper.CheckSqlPackagePath())
                {
                    LoggingHelper.GetInstance().LoggerWrite("SqlPackage.exe not exist. Set SqlPackage.exe path in system variables and restart installer", true, System.Windows.Forms.MessageBoxIcon.Hand);
                }
                _splashLogger.Owner = this;

                if (_isConsoleMode)
                {
                    UpdateExistingInstallation_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void ListClients(List<InstallationDetails> lst)
        {
            try
            {
                if (ClientList.Dispatcher.CheckAccess())
                    ClientList.ItemsSource = lst;
                else
                    ClientList.Dispatcher.Invoke(() => { ListClients(lst); });
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void DatabaseHelper_Log(object sender, string e)
        {
            try
            {
                _splashLogger.SetLogText(e);
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void BackupCompleteRelease(object sender, RoutedEventArgs e)
        {
            BackupReleaseWithDatabase(false);
        }

        private void BackupPartialRelease(object sender, RoutedEventArgs e)
        {
            BackupReleaseWithDatabase(true);
        }

        private async void BackupReleaseWithDatabase(bool isPartialBackup)
        {
            try
            {
                var selectedFile = (dynamic)ClientList.SelectedItem;
                string releasePath = selectedFile.Path + selectedFile.Name.Substring(InstallerConstants.ProductName.Length);

                _splashLogger.SetSplashMessage("Backing up Release", "Backing up Release...  This may take some time...");
                LoggingHelper.GetInstance().LoggerWrite("Release Path: " + releasePath);
                _splashLogger.Show();

                Boolean result = await System.Threading.Tasks.Task.Run(() => { return BackupHelper.BackupRelease(releasePath, isPartialBackup); });
                _splashLogger.Hide();

                if (!result)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Release backup failed", true, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    LoggingHelper.GetInstance().LoggerWrite("Release backup completed", true, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void PublishDatabases(string clientConString, string smDBConString, string displayDBVersion)
        {
            try
            {
                _splashLogger.SetSplashMessage("Connecting to Database", "Connecting to Database... This may take some time...");
                LoggingHelper.GetInstance().LoggerWrite("Client Connection: " + clientConString);
                LoggingHelper.GetInstance().LoggerWrite("SM Connection: " + smDBConString);

                _splashLogger.SetSplashMessage("Updating Database: Version " + displayDBVersion, "Updating databases... This may take some time...");
                DatabaseHelper.PublishDataBase(clientConString, "Prana.NirvanaClient.dacpac", smDBConString, "Prana.SecurityMaster.dacpac");
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void PublishClientSpecificScripts(string clientConString, string smDBConString, string displayDBVersion, string clientName)
        {
            try
            {
                string realDBVersion = DatabaseHelper.GetRealDBVersion(clientConString);
                LoggingHelper.GetInstance().LoggerWrite("Running client specific scripts of " + clientName);

                _splashLogger.SetSplashMessage("Updating Client Specific Scripts: Version " + displayDBVersion, "Updating client specific scripts... This may take some time...");
                LoggingHelper.GetInstance().LoggerWrite("Client Connection: " + clientConString);
                LoggingHelper.GetInstance().LoggerWrite("SM Connection: " + smDBConString);
                DatabaseHelper.PublishDataBase(clientConString, @"ClientSpecificScripts\" + clientName + @"\Prana.NirvanaClient.dacpac", smDBConString, null);
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void ShowExitCodeMessage(int exitCode, string releasePath)
        {
            try
            {
                if (exitCode == 1602)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error: The installation was cancelled by the user. Exit Code: " + exitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (exitCode == 1625)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error: This installation is forbidden by system policy. Contact your system administrator. Exit Code: " + exitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (exitCode == 3010)
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error: A restart is required to complete the install. Exit Code: " + exitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    LoggingHelper.GetInstance().LoggerWrite("Error: The installation was not completed. Exit Code: " + exitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void EnableDisableMainWindow(bool isEnable)
        {
            try
            {
                this.IsEnabled = isEnable;
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }
    }
}