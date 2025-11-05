using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Prana.InstallerUtilities
{
    public class BackupHelper
    {
        private static String[] backup_folders = ConfigurationManager.AppSettings["backupfolders"].Split(',');
        private static string[] folders_merged_back = ConfigurationManager.AppSettings["folders_merged_back"].Split(',');
        private static string[] files_merged_back = ConfigurationManager.AppSettings["files_merged_back"].Split(',');
        private static List<String> skip_folders_in_release_backup = ConfigurationManager.AppSettings["SkipFoldersInReleaseBackup"].Split(',').ToList();
        private static string ComplianceCustomRuleRootPath = ConfigurationManager.AppSettings["ComplianceCustomRuleRootPath"];
        private static string ComplianceCustomRuleFileName = ConfigurationManager.AppSettings["ComplianceCustomRuleFileName"];

        public static event EventHandler<String> Log;

        /// <summary>
        /// Get the timestamp in desired string format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        /// <summary>
        /// Creates a backup of the release, This is called by the MSI created using wix
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Boolean BackupRelease(String path, bool isPartialBackup, bool isBackupForRestore = false)
        {
            try
            {
                string SourceDir;
                string DestDir = string.Empty;

                if (isBackupForRestore)
                {
                    DeleteBackupForRestoreDirectory(path);
                    DestDir = path + "\\BackupsForRestore\\";

                    foreach (String folder in folders_merged_back)
                    {
                        SourceDir = path + "\\" + folder;
                        if (Directory.Exists(SourceDir))
                        {
                            if (Log != null)
                            {
                                Log(null, "Backing up " + folder);
                            }

                            DirectoryCopy(SourceDir, DestDir + folder, true, isPartialBackup, false, path);
                        }
                    }

                    foreach (String file in files_merged_back)
                    {
                        if (file.Contains("\\"))
                        {
                            string dirName = file.Substring(0, file.LastIndexOf("\\"));

                            if (!Directory.Exists(DestDir + dirName))
                            {
                                Directory.CreateDirectory(DestDir + dirName);
                            }
                        }

                        if (File.Exists(path + "\\" + file))
                        {
                            if (Log != null)
                            {
                                Log(null, "Backing up " + file);
                            }

                            File.Copy(path + "\\" + file, DestDir + file, true);
                        }
                    }

                    if (Directory.Exists(path + "\\" + ComplianceCustomRuleRootPath))
                    {
                        foreach (string directoryPath in Directory.GetDirectories(path + "\\" + ComplianceCustomRuleRootPath, "*", SearchOption.TopDirectoryOnly))
                        {
                            string sourceDirectoryName = new DirectoryInfo(directoryPath).Name;

                            string destDirectoryPath = DestDir + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf";
                            if (!Directory.Exists(destDirectoryPath))
                            {
                                Directory.CreateDirectory(destDirectoryPath);
                            }

                            if (File.Exists(directoryPath + "\\conf\\" + ComplianceCustomRuleFileName))
                            {
                                if (Log != null)
                                {
                                    Log(null, "Backing up " + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName);
                                }

                                File.Copy(directoryPath + "\\conf\\" + ComplianceCustomRuleFileName, destDirectoryPath + "\\" + ComplianceCustomRuleFileName, true);
                            }
                        }
                    }
                }
                else
                {
                    DestDir = path + "\\Backups\\" + GetTimestamp(DateTime.Now) + "\\";

                    foreach (String folder in backup_folders)
                    {
                        SourceDir = path + "\\" + folder;
                        if (Directory.Exists(SourceDir))
                        {
                            if (Log != null)
                            {
                                Log(null, "Backing up " + folder);
                            }

                            DirectoryCopy(SourceDir, DestDir + folder, true, isPartialBackup, false, path);
                        }
                    }

                    String[] conStrings = ReleaseHelper.GetConnectionStringForRelease(path);
                    if (conStrings != null && conStrings.Length == 2)
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Client Connection: " + conStrings[0]);
                        LoggingHelper.GetInstance().LoggerWrite("SM Connection: " + conStrings[1]);

                        string displayDBVersion = DatabaseHelper.GetRealDBVersion(conStrings[0]);

                        if (!string.IsNullOrEmpty(displayDBVersion))
                        {
                            DatabaseHelper.BackupDatabase(conStrings[0], DestDir + GetStringBetween(conStrings[0], "Database=", ";Server") + "_" + displayDBVersion + ".bak");
                            DatabaseHelper.BackupDatabase(conStrings[1], DestDir + GetStringBetween(conStrings[1], "Database=", ";Server") + "_" + displayDBVersion + ".bak", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);

                return false;
            }
            return true;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool isPartialBackup, bool withoutFixInternalsFile, string releasePath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                // If the source directory does not exist, throw an exception.
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
                }

                // If the destination directory does not exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (!(withoutFixInternalsFile && (file.Name).ToLower().StartsWith("pranainternalfixtags")))
                    {
                        // Create the path to the new copy of the file.
                        string temppath = Path.Combine(destDirName, file.Name);

                        // Copy the file.
                        file.CopyTo(temppath, true);
                    }
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        if (!string.IsNullOrEmpty(releasePath) && isPartialBackup && skip_folders_in_release_backup.Contains(subdir.FullName.Substring(releasePath.Length + 1)))
                        {
                            continue;
                        }

                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, isPartialBackup, withoutFixInternalsFile, releasePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        public static Boolean RestoreFilesFromBackup(string path)
        {
            try
            {
                string destPath = path;
                string sourcePath = path + "\\BackupsForRestore\\";

                foreach (String folder in folders_merged_back)
                {
                    var sourcePathTemp = sourcePath + folder;
                    if (Directory.Exists(sourcePathTemp))
                    {
                        if (Log != null)
                        {
                            Log(null, "Copying from backup " + folder);
                        }

                        DirectoryCopy(sourcePathTemp, destPath + "\\" + folder, true, true, true, null);
                    }
                    else
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Directory not exist: " + sourcePathTemp);
                    }
                }

                foreach (string file in files_merged_back)
                {
                    var sourcePathTemp = sourcePath + file;
                    if (File.Exists(sourcePathTemp))
                    {
                        if (Log != null)
                        {
                            Log(null, "Copying from backup " + file);
                        }

                        FileInfo fileInfo = new FileInfo(sourcePathTemp);
                        FileInfo fileInPatch = new FileInfo(destPath + "\\" + file);

                        if (file.Contains("\\"))
                        {
                            string dirName = file.Substring(0, file.LastIndexOf("\\"));

                            if (File.Exists(destPath + "\\" + file))
                            {
                                fileInPatch.CopyTo(destPath + "\\" + dirName + "\\" + Path.GetFileNameWithoutExtension(file) + "_(patch)" + Path.GetExtension(file), true);
                            }
                            else
                            {
                                LoggingHelper.GetInstance().LoggerWrite("File not exist: " + destPath + "\\" + file);
                            }

                            if (Directory.Exists(destPath + "\\" + dirName))
                            {
                                fileInfo.CopyTo(destPath + "\\" + file, true);
                            }
                        }
                        else
                        {
                            if (File.Exists(destPath + "\\" + file))
                            {
                                fileInPatch.CopyTo(destPath + "\\" + Path.GetFileNameWithoutExtension(file) + "_(patch)" + Path.GetExtension(file), true);
                            }
                            else
                            {
                                LoggingHelper.GetInstance().LoggerWrite("File not exist: " + destPath + "\\" + file);
                            }

                            if (Directory.Exists(destPath))
                            {
                                fileInfo.CopyTo(destPath + "\\" + file, true);
                            }
                        }
                    }
                    else
                    {
                        LoggingHelper.GetInstance().LoggerWrite("File not exist: " + sourcePathTemp);
                    }
                }

                if (Directory.Exists(sourcePath + ComplianceCustomRuleRootPath))
                {
                    foreach (string directoryPath in Directory.GetDirectories(sourcePath + ComplianceCustomRuleRootPath, "*", SearchOption.TopDirectoryOnly))
                    {
                        string sourceDirectoryName = new DirectoryInfo(directoryPath).Name;

                        string sourcePathTemp = sourcePath + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName;
                        if (File.Exists(sourcePathTemp))
                        {
                            if (Log != null)
                            {
                                Log(null, "Copying from backup " + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName);
                            }

                            FileInfo fileInfo = new FileInfo(sourcePathTemp);
                            FileInfo fileInPatch = new FileInfo(destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName);

                            if (File.Exists(destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName))
                                fileInPatch.CopyTo(destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + Path.GetFileNameWithoutExtension(ComplianceCustomRuleFileName) + "_(patch)" + Path.GetExtension(ComplianceCustomRuleFileName), true);
                            else
                                LoggingHelper.GetInstance().LoggerWrite("File not exist: " + destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName);

                            if (Directory.Exists(destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf"))
                                fileInfo.CopyTo(destPath + "\\" + ComplianceCustomRuleRootPath + "\\" + sourceDirectoryName + "\\conf\\" + ComplianceCustomRuleFileName, true);
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("File not exist: " + sourcePathTemp);
                        }
                    }
                }
                else
                {
                    LoggingHelper.GetInstance().LoggerWrite("Directory not exist: " + sourcePath + ComplianceCustomRuleRootPath);
                }
                DeleteBackupForRestoreDirectory(path);
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);

                return false;
            }
            return true;
        }

        public static void DeleteBackupForRestoreDirectory(string releasePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(releasePath))
                {
                    string sourcePath = releasePath + "\\BackupsForRestore\\";
                    if (Directory.Exists(sourcePath))
                    {
                        LoggingHelper.GetInstance().LoggerWrite("Deleting the Backup for Restore directory");

                        Directory.Delete(sourcePath, true);

                        LoggingHelper.GetInstance().LoggerWrite("Deleted the Backup for Restore directory");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        public static string GetStringBetween(string token, string first, string second)
        {
            try
            {
                if (!token.Contains(first)) return "";

                var afterFirst = token.Split(new[] { first }, StringSplitOptions.None)[1];

                if (!afterFirst.Contains(second)) return "";

                var result = afterFirst.Split(new[] { second }, StringSplitOptions.None)[0];

                return result;
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
            return string.Empty;
        }
    }
}
