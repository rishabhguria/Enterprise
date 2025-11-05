using Prana.InstallerUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Prana.Installer
{
    public partial class CustomizeControl : Window
    {
        private List<string> _clientNames = new List<string>();
        public string ClientDacpacFileName { get; set; }

        public bool IsConsoleMode { get; set; }

        public string ConsoleModeClientName { get; set; }

        public CustomizeControl()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsConsoleMode && string.IsNullOrEmpty(ConsoleModeClientName))
                {
                    this.Close();
                }

                string pathStartup = System.AppDomain.CurrentDomain.BaseDirectory;
                _clientNames.Add("-None-");
                int counter = 0;
                int index = -1;

                if (Directory.Exists(pathStartup + "ClientSpecificScripts"))
                {
                    string[] clientFolders = Directory.GetDirectories(pathStartup + "ClientSpecificScripts");

                    foreach (string clientFolder in clientFolders)
                    {
                        counter++;

                        string clientName = System.IO.Path.GetFileName(clientFolder);
                        if (IsConsoleMode && !string.IsNullOrEmpty(ConsoleModeClientName) && clientName.Equals(ConsoleModeClientName))
                            index = counter;

                        _clientNames.Add(clientName);
                    }

                    clientList.ItemsSource = _clientNames;
                    clientList.SelectedItem = _clientNames[0];
                    clientList.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                    if (IsConsoleMode)
                    {
                        if (index != -1)
                        {
                            clientList.SelectedItem = _clientNames[index];
                            RunClientSpecificScript();
                        }
                        else if (index == -1 || counter <= 0)
                        {
                            if (index == -1)
                            {
                                LoggingHelper.GetInstance().LoggerWrite("Provided client specific scripts not found.");
                            }

                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        private void Ok_click(object sender, RoutedEventArgs e)
        {
            RunClientSpecificScript();
        }

        private void RunClientSpecificScript()
        {
            try
            {
                LoggingHelper.GetInstance().LoggerWrite("Running Client Specific Script");
                if (clientList.SelectedItem != null && !clientList.SelectedItem.ToString().Equals("-None-"))
                    ClientDacpacFileName = clientList.SelectedItem.ToString();
                else
                    ClientDacpacFileName = string.Empty;

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }
    }
}