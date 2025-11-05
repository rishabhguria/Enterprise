using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Prana.ConfigUpdater
{
    class ConfigUpdater
    {
        private static int MAX_ARGS = 2;
        private static int ARGS_INSTALLSETTINGSFILE_INDEX = 0;
        private static int ARGS_DESTINATIONFILE_INDEX = 1;

        static void Main(string[] args)
        {
            try
            {
                bool val = ReadInputArguments(args);

                InstallSettingsUpdater objInstallSettingsUpdater = new InstallSettingsUpdater();
                objInstallSettingsUpdater.Initialize();
                objInstallSettingsUpdater.UpdateInstallSettings();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in configuring installsettings." 
                                + "Please run the Prana.Configupdater.exe manually.\n\n" 
                                + ex.Message + "Inner Exception: " + ex.InnerException.Message 
                                + "\n\n\n" + "ApplicationStartupPath: " + Application.StartupPath 
                                + "\n\n StackTrace: " + ex.StackTrace);
            }
        }

        private static bool ReadInputArguments(string[] args)
        {
            bool UseDefaultSettings = false;
            try
            {
                if (args.Length != MAX_ARGS)
                {
                    ShowUsage();
                    UseDefaultSettings = true;
                }
                if (!UseDefaultSettings)
                {
                    ConfigUpdaterSettings ConfigUpdaterSettingsInstance = ConfigUpdaterSettings.GetInstance();
                    for (int i = 0; i < MAX_ARGS && (args.Length == MAX_ARGS); i++)
                    {
                        ConfigUpdaterSettingsInstance.InstallSettingsFile = args[ARGS_INSTALLSETTINGSFILE_INDEX];
                        ConfigUpdaterSettingsInstance.DestinationFile = args[ARGS_DESTINATIONFILE_INDEX];
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Invalid Arguments.", ex);
            }
            return UseDefaultSettings;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("\n " + Application.ProductName
                            + "\n" + Application.CompanyName
                            + "\n" + "Usage is: Prana.ConfigUpdater.exe InstallSettingsFile DestinationFile"
                            + "\n if no argument is supplied then default suorce file is: " + ConfigUpdaterSettings.GetInstance().InstallSettingsFile
                            + "\n and destination file is:" + ConfigUpdaterSettings.GetInstance().DestinationFile);

        }

   }
}
