using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Prana.Client.Installer.Utilities
{
    public static class eSignalInstallerHelper
    {
        public static bool IsESignalInstalled()
        {
            try
            {

                //search registry for E-Signal
                RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
                string subkey = RegistryHelper.SoftwareRegKey + "\\eSignal";

                RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);
                bool isInstalled = false;
                if (SUBKEY == null)
                {
                    isInstalled = false;
                }
                else
                {
                    object dsn = SUBKEY.GetValue("InstallDir");

                    if (dsn == null)
                        isInstalled = false;
                    else
                        isInstalled = true;
                }

                if (!isInstalled)   // if eSignal is not installed check for DM Client
                {
                    
                    subkey = RegistryHelper.SoftwareRegKey + "\\eSignal\\DM Client";
                    SUBKEY = RegKey.OpenSubKey(subkey);
                    isInstalled = false;
                    if (SUBKEY == null)
                    {
                        isInstalled = false;
                    }
                    else
                    {
                        isInstalled = true;
                    } 
                } 
                
                return isInstalled;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace);
                return false;
            }
        }
    }
}
