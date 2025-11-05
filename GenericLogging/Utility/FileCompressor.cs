using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GenericLogging.ApplicationConstants;

namespace GenericLogging.Utility
{
    public static class FileCompressor
    {
        //Method to Compress old .txt files and also delete old .zip files based on no. of days specified in config
        public static void CompressOldFiles()
        {
            try
            {
                string path = ApplicationConstant.StartupPath + ApplicationConstant.FolderName;

                if (!Directory.Exists(path)) return;

                var zipFiles = Directory.GetFiles(path, "*.zip");
                foreach (var fi in zipFiles.Select(file => new FileInfo(file)).Where(fi => fi.CreationTime < DateTime.Now.AddDays(ApplicationConstant.NoOfDays)))
                {
                    fi.Delete();
                }

                var textFiles = Directory.GetFiles(path, "*.txt");
                foreach (var file in textFiles)
                {
                    var zip = ZipFile.Open(Path.ChangeExtension(file, "zip"), ZipArchiveMode.Create);
                    zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    zip.Dispose();

                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
