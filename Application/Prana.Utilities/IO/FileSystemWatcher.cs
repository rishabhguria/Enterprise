using Prana.LogManager;
using System;
using System.IO;

namespace Prana.Utilities
{
    public class FileWatcher
    {
        string _outputPath = null;

        public void CreateWatcher(string PathToWatch, string BaseOutPutPath)
        {
            try
            {
                //Create a new FileSystemWatcher.
                FileSystemWatcher watcher = new FileSystemWatcher();

                //Set the filter to only catch TXT files.
                watcher.Filter = "*.*";

                _outputPath = BaseOutPutPath;


                //Subscribe to the Created event.
                watcher.Created += new FileSystemEventHandler(FileCreated);
                watcher.Changed += new FileSystemEventHandler(FileChanged);

                //Set the path to C:\Temp\
                //watcher.Path = @"C:\Temp\";
                watcher.Path = PathToWatch;

                //Enable the FileSystemWatcher events.
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }


            }
        }

        void FileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                copyFile(e.FullPath, e.Name);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void FileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                copyFile(e.FullPath, e.Name);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        void copyFile(string fullInputFilePath, string fileName)
        {
            try
            {
                if (_outputPath != null)
                {
                    File.Copy(fullInputFilePath, createPath(fileName), true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        string createPath(string fileName)
        {
            string createdPath = null, clientName, ThirdPartyName, date, ImportTypeWithExt;
            try
            {
                if (_outputPath != null)
                {
                    string[] arrName = fileName.Split('_');
                    clientName = arrName[0]; ThirdPartyName = arrName[1];
                    date = arrName[2]; ImportTypeWithExt = arrName[3];
                    createdPath = _outputPath + "\\" + clientName + "\\" + ThirdPartyName + "\\" + date + "\\" + "Input\\";

                    //If Directory Not Exist It Will Crete The Directory
                    Directory.CreateDirectory(createdPath);
                    createdPath += ImportTypeWithExt;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return createdPath;
        }

    }
}
