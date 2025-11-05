using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Configuration;

namespace Prana.LayoutDataDecryptor
{
    public class LayoutDecryptor
    {

        public void DecryptLayout()
        {

            try
            {
                // Retrieve the connection string from the app.config file
                string connectionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString;

                #region pages

                string folderPathPages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PageLayout");

                // Ensure the directory exists and clean it up
                if (Directory.Exists(folderPathPages))
                {
                    DirectoryInfo di = new DirectoryInfo(folderPathPages);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete(); // Delete all files
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderPathPages); // Create directory if it doesn't exist
                }

                // Query to fetch data for pages
                string queryPages = "SELECT UserID, PageId, PageLayout FROM T_Samsara_OpenfinPageInfo";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand commandPages = new SqlCommand(queryPages, connection))
                        using (SqlDataReader reader = commandPages.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string userId = reader["UserID"].ToString();
                                string pageId = reader["PageId"].ToString();
                                byte[] compressedData = (byte[])reader["PageLayout"];

                                // Decompress data
                                string decompressedData = DecompressData(compressedData);

                                // Create file name
                                string fileName = $"{userId}_{pageId}.txt";

                                // Write data to file
                                string filePath = Path.Combine(folderPathPages, fileName);

                                File.WriteAllText(filePath, decompressedData);

                                Console.WriteLine($"File created: {fileName}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Occured while Decompressing Pages: {ex.Message}");
                }

                #endregion

                #region views

                string folderPathViews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ViewLayout");

                // Ensure the directory exists and clean it up
                if (Directory.Exists(folderPathViews))
                {
                    DirectoryInfo di = new DirectoryInfo(folderPathViews);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete(); // Delete all files
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderPathViews); // Create directory if it doesn't exist
                }

                // Query to fetch data for views
                string queryView = "SELECT UserID, ViewId, ViewLayout FROM T_Samsara_CompanyUserLayouts";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand commandView = new SqlCommand(queryView, connection))
                        using (SqlDataReader reader = commandView.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string userId = reader["UserID"].ToString();
                                string viewId = reader["ViewId"].ToString();
                                byte[] compressedData = (byte[])reader["ViewLayout"];

                                // Decompress data
                                string decompressedData = DecompressData(compressedData);

                                // Create file name
                                string fileName = $"{userId}_{viewId}.txt";

                                // Write data to file
                                string filePath = Path.Combine(folderPathViews, fileName);

                                File.WriteAllText(filePath, decompressedData);

                                Console.WriteLine($"File created: {fileName}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while decompressing views: {ex.Message}");
                }

                #endregion

                #region workspace

                string folderPathApplication = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationLayout");

                // Ensure the directory exists and clean it up
                if (Directory.Exists(folderPathApplication))
                {
                    DirectoryInfo di = new DirectoryInfo(folderPathApplication);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete(); // Delete all files
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderPathApplication); // Create directory if it doesn't exist
                }

                // Query to fetch data for views
                string queryApplication = "select UserID , WorkspaceId , WorkspaceLayout from T_Samsara_OpenfinWorkspaceInfo";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand commandApplication = new SqlCommand(queryApplication, connection))
                        using (SqlDataReader reader = commandApplication.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string userId = reader["UserID"].ToString();
                                string workspaceId = reader["WorkspaceId"].ToString();
                                byte[] compressedData = (byte[])reader["WorkspaceLayout"];

                                // Decompress data
                                string decompressedData = DecompressData(compressedData);

                                // Create file name
                                string fileName = $"{userId}_{workspaceId}.txt";

                                // Write data to file
                                string filePath = Path.Combine(folderPathApplication, fileName);

                                File.WriteAllText(filePath, decompressedData);

                                Console.WriteLine($"File created: {fileName}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while decompressing workspace: {ex.Message}");
                }

                #endregion

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Occurred in Utility  : " + ex.Message);
            }
        }

        // Method to decompress data
        private static string DecompressData(byte[] compressedData)
        {
            try
            {
                using (var compressedStream = new MemoryStream(compressedData))
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
