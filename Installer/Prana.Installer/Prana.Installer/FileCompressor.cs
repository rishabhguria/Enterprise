using Prana.InstallerUtilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Prana.Installer
{
    class FileCompressor
    {
        public void CreateZipFile(List<String> items, String destination)
        {
            try
            {
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    // loop through all the items
                    foreach (String item in items)
                    {
                        // if item is in file 
                        if (File.Exists(item))
                        {
                            // Add the file in the root folder inside our zip file
                            zip.AddFile(item, "");
                        }
                        // if the item is a folder
                        else if (Directory.Exists(item))
                        {
                            zip.AddDirectory(item, new DirectoryInfo(item).Name);
                        }
                    }
                    zip.Save(destination);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        public void CreateZipFile(string source, string destination)
        {
            try
            {
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    zip.AddFile(source, "");
                    zip.Save(destination);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }
    }
}
