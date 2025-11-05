using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Utilities
{
    public static class DataFolderUploader
    {
        /// <summary>
        /// Uploads the folder.
        /// </summary>
        /// <param name="ReportFolderId">The report folder identifier.</param>
        /// <param name="LogPath">The log path.</param>
        /// <param name="applicationStartupPath">The application startup path.</param>
        /// <param name="isFolder">if set to <c>true</c> [is folder].</param>
        public static void UploadFolder(string ReportFolderId, string LogPath, string applicationStartupPath, bool isFolder)
        {
            try
            {
                //connect to google drive
                DriveService service = ConnectToDriveService(applicationStartupPath);
                if (service != null)
                {
                    if (isFolder)
                        UploadFilesandFolders(service, LogPath, ReportFolderId);
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Uploads the filesand folders.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="ReportFolderId">The report folder identifier.</param>
        private static void UploadFilesandFolders(DriveService service, string folderName, string ReportFolderId)
        {
            if (System.IO.File.Exists(folderName))
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = System.IO.Path.GetFileName(folderName),
                    Parents = new List<string> { ReportFolderId }
                };

                byte[] byteArray = System.IO.File.ReadAllBytes(folderName);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = service.Files.Create(fileMetadata,stream, "");
                    request.Upload();
                   var uploadedFileID = request.ResponseBody.Id;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Occured");
                }
            }
        }

        /// <summary>
        /// Searches the file.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void SearchFile(string applicationStartupPath,string fileName)
        {
            try
            {
                //connect to google drive
                DriveService service = ConnectToDriveService(applicationStartupPath);
                if (service != null)
                {                      
                    try
                        {
                            FilesResource.ListRequest listRequest = service.Files.List();
                            // List files.
                            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                           
                            if (files != null && files.Count > 0)
                            {
                                foreach (var file in files)
                                {
                                    if (file.Name.Equals(fileName))
                                        ApplicationArguments.ReportFile = file.Id;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Information("Error: " + ex.Message);
                        }
                    }
                }
            
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }

        }

        /// <summary>
        /// Connects to drive service.
        /// </summary>
        /// <param name="applicationStartupPath">The application startup path.</param>
        /// <returns></returns>
        private static DriveService ConnectToDriveService(string applicationStartupPath)
        {
            DriveService service = null;
            try
            {
                string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
                string applicationName = "Nirvana.TestAutomation.Core";

                UserCredential credential;
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = applicationStartupPath;
                    credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                }
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = applicationName,
                });
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return service;
        }
    }
}
