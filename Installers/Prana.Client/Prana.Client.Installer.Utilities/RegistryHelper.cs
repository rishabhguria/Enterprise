using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using Prana.Client.Installer.Utilities;


namespace Prana.Client.Installer.Utilities
{
    public static class RegistryHelper
    {
        public static string SoftwareRegKey
        {
            get {
                    if (Is64bitOS)
                    {
                        return "SOFTWARE\\Wow6432Node";
                    }
                    else
                    {
                        return "SOFTWARE";
                    }
                }
        }

        public static bool Is64bitOS
        {
            get { return (Environment.GetEnvironmentVariable("ProgramFiles(x86)") != null); }
        }

        public static string ProgramFilesX86
        {
            get
            {
                string programFiles = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                if (programFiles == null)
                {
                    programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
                }

                return programFiles;
            }
        }

        public static void WriteEntriesInRegistryForStartup(string PranaInstallerStartupPath)
        {
            try
            {
                //search registry for Startup Information
                RegistryKey RegKeyInstallerPath = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");

                RegKeyInstallerPath.SetValue("InstallerPath", PranaInstallerStartupPath);

                RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");

                string subkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
                RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);

                object dsn = SUBKEY.GetValue("Vexpo");
                string path = PranaInstallerStartupPath + "\\" + Constants.CONST_Components + Constants.CONST_PranaRestartSetup;
                RegistryKey RegKey2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

                RegKey2.SetValue("Vexpo", path);
                Registry.LocalMachine.Flush();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + " " + exp.StackTrace);
            }

        }
        public static void RemoveEntriesInRegistryForStartup()
        {
            try
            {
                //search registry for Startup Information
                RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");

                string subkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
                RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);

                object dsn = SUBKEY.GetValue("Vexpo");

                if (dsn != null)
                {
                    //Delete the key
                    RegistryKey RegKey2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                    RegKey2.DeleteValue("Vexpo", false);

                    Registry.LocalMachine.Flush();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace);
            }
        }

        public static void RemoveEntriesInRegistryForVersionAndDirectoryPath()
        {
            try
            {
                //search registry for Startup Information
                RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");

                string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";
                RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);

                object dsn = SUBKEY.GetValue("InstallDir");

                if (dsn != null)
                {
                    //Delete the key
                    RegistryKey RegKey2 = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");
                    RegKey2.DeleteValue("InstallDir", false);
                    Registry.LocalMachine.Flush();
                }


                dsn = SUBKEY.GetValue("LatestVersion");

                if (dsn != null)
                {
                    //Delete the key
                    RegistryKey RegKey2 = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");
                    RegKey2.DeleteValue("LatestVersion", false);
                    Registry.LocalMachine.Flush();
                }


                dsn = SUBKEY.GetValue("InstallerPath");

                if (dsn != null)
                {
                    //Delete the key
                    RegistryKey RegKey2 = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");
                    RegKey2.DeleteValue("InstallerPath", false);
                    Registry.LocalMachine.Flush();
                } 
            
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace);
            }
        }
    }
}
