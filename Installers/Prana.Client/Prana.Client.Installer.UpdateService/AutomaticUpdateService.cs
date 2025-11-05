using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Timers;
using Prana.Client.Installer.Utilities;

namespace Prana.Client.Installer.UpdateService
{
    public partial class AutomaticUpdateService : ServiceBase
    {
        private System.Timers.Timer timer1 = new System.Timers.Timer();

        private string _latestPatchVersion = string.Empty;
        private string _currentPranaVersion = string.Empty;
        private string _ftpAddress = FTPServerConstants.CONSTFtpAddress;
        private string _downloadedUpdateLocation = "C:\\PranaTemp";
        private string _ftpLogin = FTPServerConstants.CONSTFtpLogin;
        private string _ftpPassword = FTPServerConstants.CONSTFtpPassword;
        private bool _updateOptionSetByUser = false;
        private string _InstallDir = string.Empty;

        private bool _isPatchVersion = false;

        string _userFtpDirectory = string.Empty;

        List<string> filesListMain = new List<string>();
        List<string> filesListComponents = new List<string>();
        List<string> filesListdotnetfx = new List<string>();
        List<string> filesListReportViewer = new List<string>();
        List<string> filesListSqlExpress = new List<string>();
        
        
        public AutomaticUpdateService()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
            CreateTempFolderForDownload();            
           
        }


        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer1.Interval = 1000 * 360 * 60;

            if (CheckServerConnection() == true)
            {
                CheckForUpdates();

                if (_updateOptionSetByUser == true)
                {
                    DownloadAndInstallUpdates();
                }
            }

        }

        private bool CheckServerConnection()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpAddress+"Patches Description.txt");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            

            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception exp)
            {
                EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                return false;
            }

            return true;
                
        }


        private bool FileExistOnServer(string filePath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //request.

            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception exp)
            {
                EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                return false;
            }

            return true;

        }
        

        protected override void OnStart(string[] args)
        {
            //bgworker.RunWorkerAsync();
            timer1.Enabled = true;
        }


        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            timer1.Enabled = false;
        }


        private void DownloadAndInstallUpdates()
        {
            ConnectToDBandRead();

            if (!_isPatchVersion)
            {
                //Download and Install Full Version
                GetCompleteFolder();
                DownloadCompleteFolder();
                InstallFullVersion();

            }
            else
            {
                //Download and Install Patch Version
                DownloadPatchUpdates();
                InstallPatchUpdates();
            }
        }


        private void ConnectToDBandRead()
        {

            string finalConnStr = string.Empty;

            ConfigXmlDocument m_SourceConfigxmlDoc = new ConfigXmlDocument();
            m_SourceConfigxmlDoc.Load(_InstallDir + "\\Prana.exe.config");

            string DBConnectionStringXPath = "//configuration/connectionStrings";
            string strPranaConnectionStringName = "PranaConnectionString";
            try
            {
                XmlNode DBSourceNode = m_SourceConfigxmlDoc.SelectSingleNode(DBConnectionStringXPath);
                if (DBSourceNode != null)
                {
                    XmlNodeList DBSourceNodes = m_SourceConfigxmlDoc.SelectNodes(DBConnectionStringXPath);
                    foreach (XmlNode DBSrcNode in DBSourceNodes.Item(0).ChildNodes)
                    {
                        if (DBSrcNode.NodeType != XmlNodeType.Element) continue;
                        if (DBSrcNode.Name.ToLower() == "add" && DBSrcNode.Attributes["name"].Value.ToLower() == strPranaConnectionStringName.ToLower())
                        {
                            finalConnStr = DBSrcNode.Attributes["connectionString"].Value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(this.ServiceName, ex.Message, EventLogEntryType.Error);
                MessageBox.Show("Unable to connect to the Database");
            }

            if (finalConnStr != string.Empty)
            {

                SqlConnection con = new SqlConnection(finalConnStr);

                try
                {
                    string sqlQuery = "select Login from T_Company";

                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    //SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    con.Open();
                    SqlDataReader dataread = cmd.ExecuteReader();
                    while (dataread.Read())
                    {
                        _userFtpDirectory = dataread.GetValue(0).ToString();
                        break;
                    }
                }
                catch (Exception exp)
                {
                    EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                    MessageBox.Show("Unable to read from the Database");
                }
                finally
                {
                    con.Close();
                }

            }
        }


        private void DownloadCompleteFolder()
        {

            foreach (string filename in filesListMain)
            {
                if (!filename.Contains("."))
                    continue;
                string sourcePath = _ftpAddress + _latestPatchVersion + "//" + filename;
                if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion))
                    Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion);
                string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\" + filename;
                
                DownloadFile(sourcePath, destinationPath);
            }

            foreach (string filename in filesListComponents)
            {
                if (!filename.Contains("."))
                    continue;
                string sourcePath = _ftpAddress + _latestPatchVersion + "//Components//" + filename;
                if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion + "\\Components"))
                    Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion + "\\Components");
                string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\Components\\" + filename;
                
                DownloadFile(sourcePath, destinationPath);
            }

            foreach (string filename in filesListdotnetfx)
            {
                if (!filename.Contains("."))
                    continue;
                string sourcePath = _ftpAddress + _latestPatchVersion + "//Components//dotnetfx//" + filename;
                if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion + "\\dotnetfx"))
                    Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion + "\\Components\\dotnetfx");
                string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\Components\\dotnetfx\\" + filename;
                
                DownloadFile(sourcePath, destinationPath);
            }

            foreach (string filename in filesListReportViewer)
            {
                if (!filename.Contains("."))
                    continue;
                string sourcePath = _ftpAddress + _latestPatchVersion + "//Components//ReportViewer//" + filename;
                if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion + "\\ReportViewer"))
                    Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion + "\\Components\\ReportViewer");
                string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\Components\\ReportViewer\\" + filename;
                
                DownloadFile(sourcePath, destinationPath);
            }

            foreach (string filename in filesListSqlExpress)
            {
                if (!filename.Contains("."))
                    continue;
                string sourcePath = _ftpAddress + _latestPatchVersion + "//Components//SqlExpress//" + filename;
                if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion + "\\Components\\SqlExpress"))
                    Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion + "\\Components\\SqlExpress");
                string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\Components\\SqlExpress\\" + filename;
                
                DownloadFile(sourcePath, destinationPath);
            }

        }


        private void GetCompleteFolder()
        {
            string downloadLoc = _ftpAddress + _latestPatchVersion + "//";
            filesListMain = GetListofFiles(downloadLoc);

            downloadLoc = downloadLoc + "Components//";
            filesListComponents = GetListofFiles(downloadLoc);

            string downloadDotnetLoc = downloadLoc + "dotnetfx//";
            filesListdotnetfx = GetListofFiles(downloadDotnetLoc);

            string downloadReportViewerLoc = downloadLoc + "ReportViewer//";
            filesListReportViewer = GetListofFiles(downloadReportViewerLoc);

            string downloadSqlExpressLoc = downloadLoc + "SqlExpress//";
            filesListSqlExpress = GetListofFiles(downloadSqlExpressLoc);
        }


        private List<string> GetListofFiles(string sourcePath)
        {
            List<string> files = new List<string>();

            FtpWebRequest request = (FtpWebRequest)(WebRequest.Create(new Uri(sourcePath)));
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                StreamReader sr = new StreamReader(responseStream);


                while (!sr.EndOfStream)
                {
                    files.Add(sr.ReadLine());
                }
                responseStream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                MessageBox.Show(exp.Message);
            }

            request = null;
            return files;

        }


        private void InstallPatchUpdates()
        {
            string patchUpdateInstallerPath = _downloadedUpdateLocation + _latestPatchVersion + "\\" + "Patch" + _latestPatchVersion + ".msp";

            if (File.Exists(patchUpdateInstallerPath))
            {
                Process m_process = new Process();
                m_process.StartInfo.FileName = patchUpdateInstallerPath;
                m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                m_process.Start();
            }
            else
                MessageBox.Show("The path to Prana Patch File could not be found!");
        }


        private void InstallFullVersion()
        {
            string fullVersionInstallerPath = _downloadedUpdateLocation + _latestPatchVersion + "\\" + "Prana.Client.Installer.exe";

            if (File.Exists(fullVersionInstallerPath))
            {
                Process m_process = new Process();
                m_process.StartInfo.FileName = fullVersionInstallerPath;
                m_process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                m_process.Start(); 
            }
            else
                MessageBox.Show("The path to Prana.Client.Installer.exe could not be found!");
            
        }


        private void CreateTempFolderForDownload()
        {

            RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
            string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";
            RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);

            object dsn = SUBKEY.GetValue("InstallDir");

            if (dsn != null)
            {
                _downloadedUpdateLocation = dsn.ToString() + "Patch Installers\\";
                _InstallDir = dsn.ToString();
            }

            //string _downloadedUpdateLocation = Application.StartupPath.Replace("", "Patch Installers");            
            if (!Directory.Exists(_downloadedUpdateLocation))
                Directory.CreateDirectory(_downloadedUpdateLocation);
        }


        private void DownloadPatchUpdates()
        {
            string sourcePath = _ftpAddress + _latestPatchVersion + "//" + "Patch" + _latestPatchVersion + ".msp";
            string destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\" + "Patch" + _latestPatchVersion + ".msp";

            if (!Directory.Exists(_downloadedUpdateLocation + _latestPatchVersion))
            {
                Directory.CreateDirectory(_downloadedUpdateLocation + _latestPatchVersion);
            }

            
            DownloadFile(sourcePath, destinationPath);

            //We have to add the string like this
            sourcePath = _ftpAddress + _latestPatchVersion + "//" + _userFtpDirectory + "//" + "InstallSettings.config";



            //sourcePath = _ftpAddress + _latestPatchVersion + "//" + "InstallSettings.config";
            destinationPath = _downloadedUpdateLocation + _latestPatchVersion + "\\" + "InstallSettings.config";

            
            DownloadFile(sourcePath, destinationPath);
        }


        private void DownloadFile(string sourcePath, string destinationPath)
        {
            if (FileExistOnServer(sourcePath))
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sourcePath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    FileStream fs = new FileStream(destinationPath, FileMode.Create);
                    Byte[] buffer = new Byte[2047];
                    int read = 1;
                    while (read != 0)
                    {
                        read = responseStream.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, read);
                    }
                    responseStream.Close();
                    fs.Flush();
                    fs.Close();
                    response.Close();
                }
                catch (Exception exp)
                {
                    EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                    MessageBox.Show(exp.Message);
                }

                request = null;
            }
        }


        private void CheckForUpdates()
        {
            _currentPranaVersion = GetVersionOfPrana();

            _latestPatchVersion = GetLatestVersionOfPatch();

            if (_latestPatchVersion == _currentPranaVersion)
            {
                _updateOptionSetByUser = false;
            }
            else
            {
                //SET THE SERVICE TO DOWNLOAD AND INSTALL THE PATCH UPDATES OR NOT

                DialogResult dlgResult = new UpdatesDialog(_latestPatchVersion).ShowDialog();

                if (dlgResult == DialogResult.Yes)
                {
                    _updateOptionSetByUser = true;                   
                }
                else
                {
                    //to check when we have to restart this option again
                    _updateOptionSetByUser = false;
                   
                }
            }

        }

        private string GetLatestVersionOfPatch()
        {

            //Downloads the file from the FTP and calculates the lastest version.

            List<string> patchVersionsList = new List<string>();
            string _selectedFile = "Patches Description.txt";
            //string _downloadedUpdateLocation = "c:\\PranaTemp\\";

            string sourcePath = _ftpAddress + _selectedFile;
            string destinationPath = _downloadedUpdateLocation + _selectedFile;

            
            DownloadFile(sourcePath, destinationPath);

            if (File.Exists(destinationPath))
            {
                FileStream fstream = new FileStream(destinationPath, FileMode.Open);
                StreamReader sr = new StreamReader(fstream);
                string str = sr.ReadLine();

                while (str != null)
                {
                    patchVersionsList.Add(str);
                    str = sr.ReadLine();
                }

                sr.Close();
                sr = null;
            }


            string maximumPatchVersion = string.Empty;
            char seperator = '.';
            char tildeSeperator = '~';


            if (patchVersionsList.Count > 1)
            {
                string[] greatestversion = null;
                string[][] allstrings = new string[patchVersionsList.Count][];
                int i = 0;
                foreach (string var in patchVersionsList)
                {
                    string[] temp = var.Split(tildeSeperator);
                    allstrings[i] = temp[0].Split(seperator);
                    i++;
                }

                for (int j = 0; j < allstrings.Length - 1; j++)
                {
                    for (int k = 0; k < allstrings[j].Length; k++)
                    {
                        if (Convert.ToInt32(allstrings[j][k]) < Convert.ToInt32(allstrings[j + 1][k]))
                        {
                            greatestversion = allstrings[j + 1];
                        }
                    }
                }

               maximumPatchVersion = greatestversion[0] + "." + greatestversion[1] + "." + greatestversion[2];


            }
            else if (patchVersionsList.Count == 1)
            {
                string[] temp = patchVersionsList[0].Split(tildeSeperator);
                maximumPatchVersion = temp[0];
            }
            else
                maximumPatchVersion = string.Empty;



            foreach (string strvar in patchVersionsList)
            {
                if (strvar.Contains(maximumPatchVersion))
                {

                    if (strvar.Contains("~Patch"))
                    {
                        //Install Patch Version
                        _isPatchVersion = true;
                    }
                    else if (strvar.Contains("~FullVersion"))
                    {
                        //Install Full Version
                        _isPatchVersion = false;
                    }
                }
            }


            return maximumPatchVersion;
                      

        }


        private string GetVersionOfPrana()
        {
            try
            {

                RegistryKey RegKey = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, "");
                string subkey = RegistryHelper.SoftwareRegKey + "\\Nirvana Financial";

                RegistryKey SUBKEY = RegKey.OpenSubKey(subkey);

                string pranaInstalledVersion = SUBKEY.GetValue("LatestVersion").ToString();

                return pranaInstalledVersion;
               
            }
            catch (Exception exp)
            {
                EventLog.WriteEntry(this.ServiceName, exp.Message, EventLogEntryType.Error);
                return string.Empty;
            }

        }

    }

}
