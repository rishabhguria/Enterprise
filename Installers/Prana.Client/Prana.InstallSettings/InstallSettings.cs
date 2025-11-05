using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration.Install;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Prana.Client.Installer.Utilities;

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("PublicPrivateKeyPair.snk")]    
 
namespace Prana.InstallSettings
{
       
    [RunInstaller(true)]
    public class PranaInstallSettings:Installer
    {
        private string _path = string.Empty;

        public PranaInstallSettings()
            : base()
        {
           
        }        


        // The Installation will call this method to run the Custom Action
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            WriteCurrentVersionToRegistry();

            _path = Context.Parameters["assemblypath"].ToString();

            string installDir = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "");
            WritePathToRegistry(installDir);

            CopyParamConfigFile();

            RegistryHelper.RemoveEntriesInRegistryForStartup();

            RunConfigUpdater();

            //InstallAutomaticUpdateService();
            //if (eSignalInstallerHelper.IsESignalInstalled())
            //{
            //    RegisterDlls();
            //}
        }

        private void InstallAutomaticUpdateService()
        {
            UnInstallExistingServiceAndInstallNewService();
        }

        private void UnInstallExistingServiceAndInstallNewService()
        {
            RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
            string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";
            RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);
            string path = string.Empty;

            object dsn = SUBKEY.GetValue("InstallDir");
            if (dsn!=null)
            {
                path = dsn.ToString();
            }

            FileStream fs = null;
            StreamWriter sw = null;


            //uninstall existing service
            try
            {
                fs = new FileStream(path + "\\UnInstallAndStartService.bat", FileMode.Create);
                sw = new StreamWriter(fs);
                string argurment = "%WINDIR%\\Microsoft.NET\\Framework\\v2.0.50727\\InstallUtil.exe /u \"" + path + "\\Prana.Client.Installer.UpdateService.exe\"";
                sw.WriteLine(argurment);
                sw.Flush();
            }
            catch (Exception exp)
            {
            }
            finally
            {
                sw.Close();
                fs.Close();
            }

            string FilePath = path + "\\UnInstallAndStartService.bat";

            Process m_process = new Process();
            m_process.StartInfo.FileName = FilePath;
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            m_process.Start();
            m_process.WaitForExit();
            
            
            //ProcessStartInfo processStartInfo = new ProcessStartInfo(FilePath);

            //processStartInfo.UseShellExecute = false;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Process process = Process.Start(processStartInfo);
            //process.WaitForExit();

            FilePath = null;
            //processStartInfo = null;
            m_process = null;

            //Install existing service
            try
            {
                fs = new FileStream(path + "\\InstallAndStartService.bat", FileMode.Create);
                sw = new StreamWriter(fs);
                string arguement = "%WINDIR%\\Microsoft.NET\\Framework\\v2.0.50727\\InstallUtil.exe \"" + path + "\\Prana.Client.Installer.UpdateService.exe\"";
                sw.WriteLine(arguement);
                string argurment2 = "Net start PranaAutoUpdateService";
                sw.WriteLine(argurment2);
                sw.Flush();
            }
            catch (Exception exp)
            {
            }
            finally
            {
                sw.Close();
                fs.Close();
            }

            FilePath = path + "\\InstallAndStartService.bat";

            m_process = new Process();
            m_process.StartInfo.FileName = FilePath;
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            m_process.Start();
            m_process.WaitForExit();
            
            
            //processStartInfo = new ProcessStartInfo(FilePath);

            //processStartInfo.UseShellExecute = false;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //process = Process.Start(processStartInfo);
            //process.WaitForExit();

        }

        

        /// <summary>
        /// Writes the version of the current product in the registry 
        /// </summary>
        private void WriteCurrentVersionToRegistry()
        {
            string greatestversion = WhiteLabelHelper.Version;
            RegistryKey RegKey = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");
            RegKey.SetValue("LatestVersion", greatestversion);
        }


        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            _path = Context.Parameters["assemblypath"].ToString();

            try
            {
                DeleteESignalDlls();

               // DetachSMDB();

               // UninstallAutomaticUpdateServiceAndClean();

                RegistryHelper.RemoveEntriesInRegistryForVersionAndDirectoryPath();

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void UninstallAutomaticUpdateServiceAndClean()
        {
            string UninstallFilePath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "UnInstallAndStartService.bat");

            Process m_process = new Process();
            m_process.StartInfo.FileName = UninstallFilePath;
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            m_process.Start();
            m_process.WaitForExit();


            string DirectoryPath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "Patch Installers");
            
            if (Directory.Exists(DirectoryPath))
                Directory.Delete(DirectoryPath, true);

            DirectoryPath = _path.Replace("\\" + InstallSettingConstants.CONST_PranaInstallSettings, "");
            string[] files = Directory.GetFiles(DirectoryPath);

            
            foreach (string filename in files)
            {
                if (filename.Contains("Prana.Client.Installer.UpdateService") || filename.Contains(".bat") || filename.Contains("InstallSettings.config"))
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                } 
            }
            
        }




        private void WritePathToRegistry(string path)
        {
            RegistryKey RegKey = Registry.LocalMachine.CreateSubKey(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial");

            RegKey.SetValue("InstallDir", path);
            //RegKey2.SetValue("PatchVersion", path);
            Registry.LocalMachine.Flush();
        }

        private void DetachSMDB()
        {
            try
            {
                string strSMDBPath = "-d \"" + _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "\\SecurityMasterDB\\*") + "\"";

                ProcessStartInfo processStartInfo = new ProcessStartInfo(_path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "SSEUtil.exe"), strSMDBPath);
                processStartInfo.UseShellExecute = false;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();                
                
            }
            catch (Exception)
            {
                
                //do nothing
            }

            //Delete SM DB Directory
            Directory.Delete(_path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "\\SecurityMasterDB"), true);

        }

        private void RunConfigUpdater()
        {
            string FilePath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "Prana.ConfigUpdater.exe");

            Process m_process = new Process();
            m_process.StartInfo.FileName = FilePath;//Application.StartupPath + "\\esignal.exe";
            m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            m_process.Start();
        }

        /// <summary>
        /// Copies the PRanaConfigPArams file into the destination Folder
        /// </summary>
        private void CopyParamConfigFile()
        {
            string destinationPath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "InstallSettings.config");

            //string dllPath = Application.StartupPath;
            //string FilePath1 = dllPath + "\\InstallSettings.config";           


            string sourcePath = string.Empty;

            RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
            string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";
            RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);


            if (SUBKEY != null)
            {
                object dsn = SUBKEY.GetValue("InstallerPath");
                if (dsn != null)
                    sourcePath = Convert.ToString(dsn) + "\\InstallSettings.config";
            }

            //MessageBox.Show("Source: " + sourcePath + "Destination: " + destinationPath);

            //IT WILL CHECK FOR THE PRESENCE OF THE INSTALLSETTINGS FILES IN THE CASE OF THE PATCH UPDATE
            if (!File.Exists(sourcePath))
            {
                sourcePath = Application.StartupPath + "InstallSettings.config";
                // Constants.CONST_PranaPatchInstallSettingsFilePath;
            }
            
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
           
        }
        
        private void RegisterDlls()
        {
            string FilePath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "DbcCtrl");
            //string DbcCtrlPAth = "/u C:\\ESignalDLLs\\DbcCtrl.dll";

            //ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", DbcCtrlPAth);
            string DbcCtrlPAth = "/s \"" + string.Format("{0}.dll", FilePath) + "\"";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", DbcCtrlPAth);

            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();

        }

        private void UnregisterESignalDlls()
        {
            string FilePath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "DbcCtrl");

            string DbcCtrlPAth = "/s /u \"" + string.Format("{0}.dll", FilePath) + "\"";
            
            ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", DbcCtrlPAth);
            //ProcessStartInfo processStartInfo = new ProcessStartInfo("regsvr32.exe", string.Format("/s {0}.dll", FilePath));

            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();

        }

        private void DeleteESignalDlls()
        {

            string DbcCtrlPAth = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "DbcCtrl.dll");
            string dbcapiPath = _path.Replace(InstallSettingConstants.CONST_PranaInstallSettings, "dbcapi.dll");

            if (File.Exists(DbcCtrlPAth))
                File.Delete(DbcCtrlPAth);

            if (File.Exists(dbcapiPath))
                File.Delete(dbcapiPath);
        }

        //private bool FindExistingVersionsOfPrana()
        //{
        //    RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
        //    string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";

        //    //StringBuilder sb = new StringBuilder(RegistryHelper.SoftwareRegKey + "\\Nirvana Financial\\Prana");

        //    RegistryKey TEMPKEY = RegKey.OpenSubKey(subkey);

        //    try
        //    {
        //        if (TEMPKEY != null)
        //        {
        //            RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);
        //            string[] subkeyArray = SUBKEY.GetSubKeyNames();

        //            foreach (string var in subkeyArray)
        //            {
        //                if (var.Contains("Prana"))
        //                {
        //                    return true;
        //                }
        //            }

        //            return false;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        return false;
        //    }


        //}

    }
}
