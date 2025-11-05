using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FTP_Multiple
{
    public static class ProcessedFileService
    {
        private static readonly string _processedFilesFilePath;

        static ProcessedFileService()
        {
            _processedFilesFilePath = $@"{Program.currentDirectory}\Store\Processed_Files_{Program.processDate.ToString("yyyyMMdd")}.txt";
            LoadProcessedFiles();
        }

        private static HashSet<string> _processedFiles = new HashSet<string>();

        private static void LoadProcessedFiles()
        {
            _processedFiles.Clear(); // Clear existing list to avoid duplicates if called multiple times
            Program.CreateLogs("Reading file " + _processedFilesFilePath);
            if (File.Exists(_processedFilesFilePath))
            {
                _processedFiles = File.ReadAllLines(_processedFilesFilePath).Select(Path.GetFileNameWithoutExtension).ToHashSet();
            }
        }

        public static bool IsFileProcessed(string fileName)
        {
            Program.CreateLogs("Called this method for " + fileName);
            if (_processedFiles == null || _processedFiles.Count == 0)
            {
                return false; // No processed files loaded
            }

            return _processedFiles.Contains(Path.GetFileNameWithoutExtension(fileName), StringComparer.OrdinalIgnoreCase);
        }
    }
}