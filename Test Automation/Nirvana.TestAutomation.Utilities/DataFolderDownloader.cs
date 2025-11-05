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
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Utilities
{
    public static class DataFolderDownloader
    {
        /// <summary>
        /// Downloads the folder.
        /// </summary>
        /// <param name="driveFolderId">The drive folder identifier.</param>
        /// <param name="dataFolderPath">The data folder path.</param>
        /// <param name="applicationStartupPath">The application startup path.</param>
        public static void DownloadFolder(string driveFolderId, string dataFolderPath, string applicationStartupPath, bool isFolder)
        {
            try
            {
                //connect to google drive
                DriveService service = ConnectToDriveService(applicationStartupPath);
                if (service != null)
                {
                    if (isFolder)
                        DownloadFilesandFolders(service, driveFolderId, dataFolderPath);
                    else
                        ExportFileToExcel(driveFolderId, "Module Step Settings", service, dataFolderPath);
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

        /// <summary>
        /// Downloads the filesand folders.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="folderId">The folder identifier.</param>
        /// <param name="folderName">Name of the folder.</param>
        private static void DownloadFilesandFolders(DriveService service, string folderId, string folderName)
        {
            try
            {
                FilesResource.ListRequest request = service.Files.List();
                request.Q = "'" + folderId + "' in parents";
                FileList files = request.Execute();

                foreach (Google.Apis.Drive.v3.Data.File driveFile in files.Files)
                {
                    DateTime date = DateTime.Now;
                    date = date.Date;
                    if (!ApplicationArguments.DownloadData)
                    {
                        if (driveFile.Name.Contains("(Distributed)-" + date.ToString("yyyy-MM-dd")) && !driveFile.Name.Contains("Master") && !driveFile.MimeType.Equals("application/vnd.google-apps.spreadsheet"))
                        {
                            DownloadFileToExcel(driveFile.Id, driveFile.Name, service, folderName);
                        }
                        else if (driveFile.Name.Contains("(Distributed)-" + date.ToString("yyyy-MM-dd")) && !driveFile.Name.Contains("Master") && driveFile.MimeType.Equals("application/vnd.google-apps.spreadsheet"))
                            ExportFileToExcel(driveFile.Id, driveFile.Name, service, folderName);
                    }
                    else
                    {
                        if (ApplicationArguments.DownloadData&&driveFile.MimeType.Equals("application/vnd.google-apps.spreadsheet"))
                            ExportFileToExcel(driveFile.Id, driveFile.Name, service, folderName);
                        else if (ApplicationArguments.DownloadData && driveFile.MimeType.Equals("application/vnd.google-apps.folder"))
                            DownloadFilesandFolders(service, driveFile.Id, folderName + "\\" + driveFile.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Exports the file to excel.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="service">The service.</param>
        /// <param name="folderPath">The folder path.</param>
        private static void ExportFileToExcel(string fileId, string fileName, DriveService service, string folderPath)
        {
            try
            {
                var request = service.Files.Export(fileId, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                var stream1 = new System.IO.MemoryStream();
                request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("File: " + fileName + " is downloaded.");
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("File: " + fileName + " is not downloaded.");
                                break;
                            }
                    }
                };
                request.Download(stream1);


                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                System.IO.File.WriteAllBytes(folderPath + "\\" + fileName + ".xlsx", stream1.GetBuffer());
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Downloads The Excel File as well as All Other Files Too.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="service">The service.</param>
        /// <param name="folderPath">The folder path.</param>
        private static void DownloadFileToExcel(string fileId, string fileName, DriveService service, string folderPath)
        {
            try
            {
                var request = service.Files.Get(fileId);
                var stream = new System.IO.MemoryStream();

                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case Google.Apis.Download.DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case Google.Apis.Download.DownloadStatus.Completed:
                            {
                                Console.WriteLine("File: " + fileName + " is downloaded.");
                                // SaveStream(stream, folderPath);
                                break;
                            }
                        case Google.Apis.Download.DownloadStatus.Failed:
                            {
                                Console.WriteLine("File: " + fileName + " is not downloaded.");
                                break;
                            }
                    }

                };
                request.Download(stream);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                System.IO.File.WriteAllBytes(folderPath + "\\" + fileName, stream.GetBuffer());

            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }
    }
}
