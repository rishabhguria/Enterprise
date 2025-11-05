using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FTP_Multiple
{
    public class ModifiedFile
    {
        public string Name { get; set; }

        public string ModifiedDate { get; set; }
    }

    public static class ModifiedFileService
    {
        private static readonly string _modifiedFilesFilePath;
        private static readonly string _modifiedFilesFileDirectoryPath;

        static ModifiedFileService()
        {
            _modifiedFilesFilePath = $@"{Program.currentDirectory}\Store\Modified_Files.json";
            _modifiedFilesFileDirectoryPath = Path.GetDirectoryName(_modifiedFilesFilePath);
            LoadModifiedFiles();
        }

        private static List<ModifiedFile> _modifiedFiles = new List<ModifiedFile>();

        private static void LoadModifiedFiles()
        {
            _modifiedFiles.Clear(); // Clear existing list to avoid duplicates if called multiple times
            Program.CreateLogs("Reading file " + _modifiedFilesFilePath);

            if (File.Exists(_modifiedFilesFilePath))
            {
                if (File.Exists(_modifiedFilesFilePath))
                {
                    string json = File.ReadAllText(_modifiedFilesFilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        string modifiedFilesJSON = File.ReadAllText(_modifiedFilesFilePath);
                        _modifiedFiles = JsonConvert.DeserializeObject<List<ModifiedFile>>(modifiedFilesJSON);
                    }
                }
            }
        }

        public static bool IsSameLastModifiedFileAlreadyDownloaded(string fileName, string lastModifiedDate)
        {
            Program.CreateLogs("Called this method for " + fileName);

            if (_modifiedFiles == null || _modifiedFiles.Count == 0)
            {
                return false; // No processed files loaded
            }

            return _modifiedFiles.Any(x => fileName.Equals(x.Name, StringComparison.OrdinalIgnoreCase)
            && lastModifiedDate.Equals(x.ModifiedDate, StringComparison.OrdinalIgnoreCase));
        }

        public static void UpdateLastModifiedDateInFile(string fileName, string lastModifiedDate)
        {
            var modifiedFile = _modifiedFiles.FirstOrDefault(x => fileName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

            if (modifiedFile != null)
            {
                modifiedFile.ModifiedDate = lastModifiedDate;
            }
            else
            {
                modifiedFile = new ModifiedFile() { ModifiedDate = lastModifiedDate, Name = fileName };
                _modifiedFiles.Add(modifiedFile);
            }

            if (!Directory.Exists(_modifiedFilesFileDirectoryPath))
            {
                Directory.CreateDirectory(_modifiedFilesFileDirectoryPath);
            }

            File.WriteAllText(_modifiedFilesFilePath, JsonConvert.SerializeObject(_modifiedFiles, Formatting.Indented));
        }
    }
}