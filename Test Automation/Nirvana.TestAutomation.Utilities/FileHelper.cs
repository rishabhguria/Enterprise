using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public class FileHelper
    {
         public static void DeleteFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                    Console.WriteLine("Deleted folder: " + folderPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting folder: " + ex.Message);
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Deleted file: " + filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting file: " + ex.Message);
            }
        }

        public static bool CheckIfFileExists(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking file existence: " + ex.Message);
                return false;
            }
        }

        
        public static bool DeleteFilesOlderThanToday(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return false;
                }

                string[] files = Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    DateTime fileDate = File.GetLastWriteTime(file);
                    if (fileDate.Date != DateTime.Today)
                    {
                        try
                        {
                            File.Delete(file);
                            Console.WriteLine("Deleted file older than today: " + file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error deleting file: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting files older than today: " + ex.Message);
                return true;
            }
            return true;
        }
         public static void CopyAndRenameFolder(string sourcePath, string destinationPath, string renameFolderName, string renamedFileName)
            {
                try
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        return;
                    }

                    string destinationFolder = Path.Combine(destinationPath, renameFolderName);

                    CopyDirectory(sourcePath, destinationFolder);

                    string originalFolderName = Path.GetFileName(sourcePath);
                    string[] xlsxFiles = Directory.GetFiles(destinationFolder, originalFolderName + ".xlsx", SearchOption.AllDirectories);

                    foreach (string xlsxFile in xlsxFiles)
                    {
                        string newFilePath = Path.Combine(Path.GetDirectoryName(xlsxFile), renamedFileName + ".xlsx");
                        File.Move(xlsxFile, newFilePath);
                    }

                    Console.WriteLine("Folder copied and renamed successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    
                }
            }

            public static bool CopyDirectory(string sourceDir, string destinationDir)
            {
                try
                {
                    if (!Directory.Exists(destinationDir))
                    {
                        Directory.CreateDirectory(destinationDir);
                    }

                    foreach (string file in Directory.GetFiles(sourceDir))
                    {
                        string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                        File.Copy(file, destFile, true);
                    }

                    foreach (string dir in Directory.GetDirectories(sourceDir))
                    {
                        string destDir = Path.Combine(destinationDir, Path.GetFileName(dir));
                        CopyDirectory(dir, destDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
                return true;
            }

            public static void CleanUpFolders(List<string> allowedFolders, string folder, bool deleteBeforeMainWork)
            {
                try
                {
                    if (!Directory.Exists(folder))
                    {
                        return;
                    }

                    string[] subDirectories = Directory.GetDirectories(folder);

                    foreach (string subDirectory in subDirectories)
                    {
                        string subDirName = Path.GetFileName(subDirectory);


                        if (!allowedFolders.Contains(subDirName) && deleteBeforeMainWork)
                        {
                            try
                            {
                                Directory.Delete(subDirectory, true);
                                Console.WriteLine("Deleted: "+subDirectory);
                            }
                            catch (Exception ex)
                            {
                                  Console.WriteLine("Error deleting "+subDirectory + "+: "+ ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                  
                }
            }
            public static void CleanUpFiles(List<string> allowedFiles, string folder, bool deleteBeforeMainWork)
            {
                try
                {
                    if (!Directory.Exists(folder))
                    {
                        return;
                    }

                    string[] files = Directory.GetFiles(folder);

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        if (!allowedFiles.Contains(fileName) && deleteBeforeMainWork)
                        {
                            try
                            {
                                File.Delete(file); 
                                Console.WriteLine("Deleted: "+file);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error deleting "+file + "+: "+ ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                   
                }
            }


        }
}
